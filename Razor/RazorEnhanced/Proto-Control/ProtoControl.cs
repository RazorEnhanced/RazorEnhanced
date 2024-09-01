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

namespace RazorEnhanced
{
    public class ProtoControlService : ProtoControl.ProtoControlBase
    {
        private bool _isRecording = false;
        private string _recordingFormat;

        internal static int? AssignedPort { get; private set; }

        public static Server StartServer(string host)
        {
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

            Server server = new Server
            {
                Services = { ProtoControl.BindService(new ProtoControlService()) },
                Ports = { new ServerPort(host, port, ServerCredentials.Insecure) }
            };
            server.Start();
            Utility.Logger.Info($"ProtoControl server listening on {host}:{port}");
            return server;
        }

        public override async Task Record(RecordRequest request, IServerStreamWriter<RecordResponse> responseStream, ServerCallContext context)
        {
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
            if (World.Player != null && (language == ScriptLanguage.PYTHON || language == ScriptLanguage.UOSTEAM))
            {
                var recorder = ScriptRecorderService.RecorderForLanguage(language);
                try
                {
                    recorder.Output = async (code) =>
                    {
                        responseStream.WriteAsync(new RecordResponse { Data = code });
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

        public override async Task Play(PlayRequest request, IServerStreamWriter<PlayResponse> responseStream, ServerCallContext context)
        {
            switch (request.Language)
            {
                case ProtoLanguage.Python:
                    PythonEngine pyEngine = new();
                    /*pyEngine.Engine.SetTrace(
                        (TraceBackFrame frame, string result, object payload) =>
                        {
                            //responseStream.WriteAsync(new PlayResponse { Result = frame, IsFinished = false });
                            responseStream.WriteAsync(new PlayResponse { Result = "TRACEBACK HERE", IsFinished = true });
                        }
                        );
                    */
                    pyEngine.SetStderr(
                        (string message) =>
                        {
                            Misc.SendMessage(message, 178);
                            responseStream.WriteAsync(new PlayResponse { Result = message, IsFinished = false });
                        }
                        );
                    pyEngine.SetStdout(
                    (string message) =>
                    {
                        Misc.SendMessage(message);
                        responseStream.WriteAsync(new PlayResponse { Result = message, IsFinished = false });
                    }
                    );
                    var pc = HostingHelpers.GetLanguageContext(pyEngine.Engine) as PythonContext;
                    var hooks = pc.SystemState.Get__dict__()["path_hooks"] as PythonDictionary;
                    if (hooks != null) { hooks.Clear(); }

                    string combinedString = string.Join("\r\n", request.Commands);
                    if (!pyEngine.Load(combinedString, null))
                    {
                        throw new OperationCanceledException();
                    }
                    pyEngine.Execute();
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


    }
}
