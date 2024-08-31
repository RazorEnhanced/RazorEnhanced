using Assistant;
using Grpc.Core;
using System;
using System.Threading.Tasks;

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
            _isRecording = request.IsOn;
            _recordingFormat = request.Format;

            while (_isRecording && !context.CancellationToken.IsCancellationRequested)
            {
                string data = GenerateRecordingData(_recordingFormat);
                await responseStream.WriteAsync(new RecordResponse { Data = data });
                await Task.Delay(100); // Simulate delay between data points
            }
        }

        public override async Task Play(PlayRequest request, IServerStreamWriter<PlayResponse> responseStream, ServerCallContext context)
        {
            foreach (var command in request.Commands)
            {
                string result = ExecuteCommand(command, request.Format);
                await responseStream.WriteAsync(new PlayResponse { Result = result, IsFinished = false });
            }

            await responseStream.WriteAsync(new PlayResponse { Result = "Execution completed", IsFinished = true });
        }

        private string GenerateRecordingData(string format)
        {
            // Implement the logic to generate recording data based on the format
            return $"Recorded data in {format} format: {DateTime.Now}";
        }

        private string ExecuteCommand(string command, string format)
        {
            // Implement the logic to execute the command based on the format
            return $"Executed command '{command}' in {format} format";
        }

    }
}
