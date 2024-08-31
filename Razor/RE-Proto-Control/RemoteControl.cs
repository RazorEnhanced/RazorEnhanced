using Assistant;
using Google.Protobuf.Collections;
using Grpc.Core;
using IronPython.Runtime;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting.Hosting.Providers;
using RazorEnhanced.UOS;
using System;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace RazorEnhanced
{
    public class RemoteControlService : RemoteControl.RemoteControlBase
    {
        private bool _isRecording = false;
        private string _recordingFormat;
        
        public static Server StartServer(string host, int port)
        {
            Server server = new Server
            {
                Services = { RemoteControl.BindService(new RemoteControlService()) },
                Ports = { new ServerPort(host, port, ServerCredentials.Insecure) }
            };
            server.Start();
            Utility.Logger.Info($"RemoteControl server listening on {host}:{port}");
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
                        (string message) => {
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

    }
}
