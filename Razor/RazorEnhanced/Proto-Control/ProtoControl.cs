using Assistant;
using Grpc.Core;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting.Providers;
using RazorEnhanced.UOS;
using System;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;
using Newtonsoft.Json;
using static IronPython.Modules.PythonNT;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Navigation;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.Scripting;
using static Assistant.DLLImport;
using static Microsoft.Scripting.Hosting.Shell.ConsoleHostOptions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Microsoft.Scripting.Hosting;
using System.Text.RegularExpressions;
using CUO_APINetPipes;

namespace RazorEnhanced
{
    public class ProtoControlService : ProtoControl.ProtoControlBase
    {
        private bool _isRecording = false;
        private string _recordingFormat;
        private static Grpc.Core.Server server = null;

        internal static int? AssignedPort { get; private set; }

        public static Grpc.Core.Server StartServer(string host)
        {
            AssignedPort = 0;

            int startingPort = 15454;  // Starting port
            int numberOfPorts = 10;   // Number of ports to check

            int port = FindAvailablePort(startingPort, numberOfPorts);

            if (port < 1)
            {
                AssignedPort = null;
                Utility.Logger.Error($"Unable to find a suitable port");
                return null;
            }

            AssignedPort = port;

            // Set the channel options including the write buffer size
            var channelOptions = new List<ChannelOption>
            {
                new ChannelOption(ChannelOptions.MaxSendMessageLength, int.MaxValue),  // Set maximum send message length
                new ChannelOption(ChannelOptions.MaxReceiveMessageLength, int.MaxValue),  // Set maximum receive message length
                new ChannelOption("grpc.so_reuseport", 1), // Optionally enable port reuse                 
            };

            try
            {
                server = new Grpc.Core.Server(channelOptions)
                {
                    Services = { ProtoControl.BindService(new ProtoControlService()) },
                    Ports = { new ServerPort(host, port, ServerCredentials.Insecure) }
                };
                server.Start();
                Utility.Logger.Info($"ProtoControl server listening on {host}:{port}");
            }
            catch (Exception e)
            {
                Utility.Logger.Info($"ProtoControl server failed to load on {host}:{port}");
                server = null;
                AssignedPort = 0;
            }


            return server;
        }

        public static async Task StopServer()
        {
            AssignedPort = 0;
            if (server != null)
            {
                await server.ShutdownAsync();
                Console.WriteLine("Server shut down completed.");
            }
        }

        public override async Task Record(RecordRequest request, IServerStreamWriter<RecordResponse> responseStream, ServerCallContext context)
        {
            if (World.Player == null)
                return;
            ScriptLanguage language = ScriptLanguage.UNKNOWN;
            switch (request.Language)
            {
                case ProtoLanguage.Python:
                    language = ScriptLanguage.PYTHON;
                    break;
                case ProtoLanguage.Uosteam:
                    language = ScriptLanguage.UOSTEAM;
                    break;
                case ProtoLanguage.Csharp:
                    language = ScriptLanguage.CSHARP;
                    break;
                default:
                    language = ScriptLanguage.UNKNOWN;
                    break;
            }
            Utility.Logger.Debug($"Started recording in {request.Language} format");
            if (language == ScriptLanguage.PYTHON || language == ScriptLanguage.UOSTEAM)
            {
                var recorder = ScriptRecorderService.RecorderForLanguage(language);

                try
                {

                    recorder.Output = async (code) =>
                    {
                        code = code.TrimEnd(new char[] { '\r', '\n' });
                        try
                        {
                            // Wait for the lock before proceeding
                            await _writeLock.WaitAsync();
                            await responseStream.WriteAsync(new RecordResponse { Data = code });
                        }
                        finally
                        {
                            // Release the lock
                            _writeLock.Release();
                        }

                    };
                    recorder.Start();
                    while (!context.CancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(100, context.CancellationToken); // Simulate delay between data points
                    }
                }
                catch (OperationCanceledException)
                {
                    // This exception is thrown when the client cancels the stream
                }
                catch (Exception ex)
                {
                    Utility.Logger.Error($"An error occurred during recording: {ex.Message}");
                }
                finally
                {
                    Utility.Logger.Debug("Recording stopped");
                    recorder.Stop();
                }
            }
            else
            {
                throw new OperationCanceledException();
            }
        }

        private readonly SemaphoreSlim _writeLock = new SemaphoreSlim(1, 1);
        public override async Task Play(PlayRequest request, IServerStreamWriter<PlayResponse> responseStream, ServerCallContext context)
        {
            if (World.Player == null)
                return;
            switch (request.Language)
            {
                case ProtoLanguage.Python:
                    PythonEngine pyEngine = new();

                    var tracingContext = new TracingContext(request.Commands, responseStream);

                    // Store the context in AsyncLocal for thread-safety
                    AsyncLocal<TracingContext> asyncLocalContext = new AsyncLocal<TracingContext>();
                    asyncLocalContext.Value = tracingContext;

                    // Set up the trace function
                    // For now I think trace is too much, maybe add in later
                    //pyEngine.Engine.SetTrace((frame, result, payload) => TraceFunction(asyncLocalContext, frame, result, payload));
                    
                    pyEngine.SetStderr(
                        async (string message) =>
                        {
                            message = message.TrimEnd(new char[] { '\r', '\n' });
                            try
                            {
                                // Wait for the lock before proceeding
                                await _writeLock.WaitAsync();
                                Misc.SendMessage(message, 178);
                                await responseStream.WriteAsync(new PlayResponse { Result = message, IsFinished = false });
                            }
                            finally 
                            {
                                // Release the lock
                                _writeLock.Release();
                            }
                        }
                        );
                    pyEngine.SetStdout(
                    async (string message) =>
                    {
                        message = message.TrimEnd(new char[] { '\r', '\n' });
                        try
                        {
                            // Wait for the lock before proceeding
                            await _writeLock.WaitAsync();

                            Misc.SendMessage(message);
                            await responseStream.WriteAsync(new PlayResponse { Result = message, IsFinished = false });
                        }
                        finally
                        {
                            // Release the lock
                            _writeLock.Release();
                        }
                    }
                    );
                    var pc = HostingHelpers.GetLanguageContext(pyEngine.Engine) as PythonContext;
                    var hooks = pc.SystemState.Get__dict__()["path_hooks"] as PythonDictionary;
                    if (hooks != null) { hooks.Clear(); }

                    string combinedString = string.Join("", request.Commands);
                    if (!pyEngine.Load(combinedString, null))
                    {
                        throw new OperationCanceledException();
                    }
                    try
                    {
                        pyEngine.Execute();
                    }
                    catch (Exception ex)
                    {
                        string message = "";
                        message += "Python Error:";
                        ExceptionOperations eo = pyEngine.Engine.GetService<ExceptionOperations>();
                        string error = eo.FormatException(ex);
                        message += Regex.Replace(error.Trim(), "\n\n", "\n");     //remove empty lines
                        try
                        {
                            await _writeLock.WaitAsync();
                            Misc.SendMessage(message);
                            await responseStream.WriteAsync(new PlayResponse { Result = message, IsFinished = false });
                        }
                        finally
                        {
                            // Release the lock
                            _writeLock.Release();
                        }

                    }
                    break;
                case ProtoLanguage.Uosteam:
                    UOSteamEngine uosEngine = new();
                    break;
                default:
                    throw new OperationCanceledException();
                    break;
            }

            /*foreach (var command in request.Commands)
            {
                string result = ExecuteCommand(command, request.Language);
                await responseStream.WriteAsync(new PlayResponse { Result = result, IsFinished = false });
            }

            await responseStream.WriteAsync(new PlayResponse { Result = "Execution completed", IsFinished = true });
            */
        }

        private string GenerateRecordingData(string format)
        {
            // Implement the logic to generate recording data based on the format
            return $"Recorded data in {format} format: {DateTime.Now}";
        }

        private string ExecuteCommand(string command, RazorEnhanced.ProtoLanguage language)
        {
            // Implement the logic to execute the command based on the format
            return $"Executed command '{command}' in {language} format";
        }

        public static int FindAvailablePort(int startingPort, int range)
        {
            for (int port = startingPort; port < startingPort + range; port++)
            {
                if (IsPortAvailable(port))
                {
                    return port;
                }
            }
            return -1;  // Return -1 if no port is available
        }

        private static bool IsPortAvailable(int port)
        {
            TcpListener listener = null;
            try
            {
                // Create a TCP listener on the specified port
                listener = new TcpListener(IPAddress.Loopback, port);
                listener.Start();
                return true;
            }
            catch (SocketException)
            {
                // If the port is in use, a SocketException will be thrown
                return false;
            }
            finally
            {
                // Stop and dispose of the listener to release the port
                listener?.Stop();
            }
        }
        private TracebackDelegate TraceFunction(AsyncLocal<TracingContext> asyncLocalContext, TraceBackFrame frame, string result, object payload)
        {
            var context = asyncLocalContext.Value;
            if (context != null && result == "line")
            {
                string fileName = frame.f_code.co_filename;
                int lineNumber = (int)frame.f_lineno;
                string lineContent = "Error outside range of source code";
                if (lineNumber <= context.SourceCode.Count)
                    lineContent = context.SourceCode[lineNumber-1];

                var message = $"Executing: {fileName}:{lineNumber} - {lineContent}";
                try
                {
                    // Wait for the lock before proceeding
                    _writeLock.WaitAsync();
                    Misc.SendMessage(message, 178);
                    context.StreamWriter.WriteAsync(new PlayResponse { Result = message, IsFinished = false });
                }
                finally
                {
                    // Release the lock
                    _writeLock.Release();
                }
            }
            return (f, r, p) => TraceFunction(asyncLocalContext, f, r, p);
        }

    }
    internal class TracingContext
    {
        public IServerStreamWriter<PlayResponse> StreamWriter { get; }
        public Google.Protobuf.Collections.RepeatedField<string> SourceCode { get; }

        public TracingContext(Google.Protobuf.Collections.RepeatedField<string> _sourceCode,  IServerStreamWriter<PlayResponse> _streamWriter)
        {
            SourceCode = _sourceCode;
            StreamWriter = _streamWriter;
        }
    }
}
