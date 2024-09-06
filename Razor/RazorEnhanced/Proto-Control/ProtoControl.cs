using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Google.Protobuf;
using WebSocketSharp;
using WebSocketSharp.Server;
using RazorEnhanced;
using System.Diagnostics;
using Assistant;
using System.Net.Sockets;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting.Providers;
using Microsoft.Scripting.Hosting;
using RazorEnhanced.UOS;
using System.Text.RegularExpressions;
using CrashReporterDotNET.com.drdump;
using System.Windows.Forms;

namespace RazorEnhanced
{
    public class ProtoMessageParser : WebSocketBehavior
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly long _timeGate = 100; // 100 milliseconds max output to remote. note tried 50 and it overflows

        private readonly SemaphoreSlim _writeLock = new SemaphoreSlim(1, 1);

        ScriptRecorder _recorder = null;

        public ProtoMessageParser()
        {
        }


        protected override void OnMessage(MessageEventArgs e)
        {
            // Deserialize the received message            
            ProtoMessageType messageType = GetMessageType(e.RawData);
            switch (messageType)
            {
                case ProtoMessageType.PlayRequestType:
                    {
                        var request = PlayRequest.Parser.ParseFrom(e.RawData);
                        Play(request);
                    }
                    break;
                case ProtoMessageType.StopPlayRequestType:
                    {
                        var request = StopPlayRequest.Parser.ParseFrom(e.RawData);
                        StopPlay(request);
                    }
                    break;
                case ProtoMessageType.RecordRequestType:
                    {
                        var request = RecordRequest.Parser.ParseFrom(e.RawData);
                        Record(request);
                    }
                    break;
                case ProtoMessageType.StopRecordRequestType:
                    {
                        var request = StopRecordRequest.Parser.ParseFrom(e.RawData);
                        StopRecorder(request);
                    }
                    break;
                default:
                    Utility.Logger.Debug($"Unreasonable message received: {messageType}");
                    return;
            }



            
        }

        public async Task Play(PlayRequest request)
        {
            if (World.Player == null)
                return;
            switch (request.Language)
            {
                case ProtoLanguage.Python:
                    PythonEngine pyEngine = new();

                    //var tracingContext = new TracingContext(request.Commands, responseStream);
                    // Store the context in AsyncLocal for thread-safety
                    //AsyncLocal<TracingContext> asyncLocalContext = new AsyncLocal<TracingContext>();
                    //asyncLocalContext.Value = tracingContext;
                    // Set up the trace function
                    // For now I think trace is too much, maybe add in later
                    //pyEngine.Engine.SetTrace((frame, result, payload) => TraceFunction(asyncLocalContext, frame, result, payload));

                    pyEngine.SetStderr(
                        async (string message) =>
                        {
                            message = message.TrimEnd(new char[] { '\r', '\n' });
                            OutputPlayMessage(message, true);
                        }
                        );
                    pyEngine.SetStdout(
                        async (string message) =>
                        {
                            message = message.TrimEnd(new char[] { '\r', '\n' });
                            OutputPlayMessage(message, true);
                        }
                        );
                    var pc = HostingHelpers.GetLanguageContext(pyEngine.Engine) as PythonContext;
                    var hooks = pc.SystemState.Get__dict__()["path_hooks"] as PythonDictionary;
                    if (hooks != null) { hooks.Clear(); }

                    string combinedString = string.Join("\r", request.Commands);
                    if (!pyEngine.Load(combinedString, null))
                    {
                        throw new OperationCanceledException();
                    }
                    try
                    {
                        //Need to make a thread and wait on it so it can be killed
                        pyEngine.Execute();
                        OutputPlayMessage("", false);
                    }
                    catch (Exception ex)
                    {
                        string message = "";
                        message += "Python Error:";
                        ExceptionOperations eo = pyEngine.Engine.GetService<ExceptionOperations>();
                        string error = eo.FormatException(ex);
                        message += Regex.Replace(error.Trim(), "\n\n", "\n");     //remove empty lines
                        OutputPlayMessage(message, false);
                    }
                    break;
                case ProtoLanguage.Uosteam:
                    UOSteamEngine uosEngine = new();
                    break;
                default:
                    throw new OperationCanceledException();
                    break;
            }

        }

        private async Task OutputPlayMessage(string message, bool more)
        {
            try
            {
                // Wait for the lock before proceeding
                await _writeLock.WaitAsync();

                // Calculate the time passed since the last execution
                long elapsedTime = _stopwatch.ElapsedMilliseconds;
                if (elapsedTime < _timeGate)
                {
                    // If time gate has not expired, calculate the remaining time and wait
                    long remainingTime = _timeGate - elapsedTime;
                    Thread.Sleep((int)remainingTime);
                }

                // Reset the stopwatch for the next gate
                _stopwatch.Restart();


                Misc.SendMessage(message, 178);
                var response = new PlayResponse
                {
                    Type = ProtoMessageType.PlayResponseType,
                    More = more,
                    Result = message
                };
                Send(response.ToByteArray());
            }
            finally
            {
                // Release the lock
                _writeLock.Release();
            }
        }


            public async Task Record(RecordRequest request)
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
                _recorder = ScriptRecorderService.RecorderForLanguage(language);

                try
                {
                    _recorder.Output = async (code) =>
                    {
                        // Create a response based on the request
                        code = code.TrimEnd(new char[] { '\r', '\n' });
                        OutputRecordMessage(code);
                    };
                    _recorder.Start();
                    while (_recorder.IsRecording())
                    {
                        await Task.Delay(100); 
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
                    _recorder.Stop();
                }
            }
            else
            {
                throw new OperationCanceledException();
            }
        }

        private async Task OutputRecordMessage(string message)
        {
            try
            {
                // Wait for the lock before proceeding
                await _writeLock.WaitAsync();

                // Calculate the time passed since the last execution
                long elapsedTime = _stopwatch.ElapsedMilliseconds;
                if (elapsedTime < _timeGate)
                {
                    // If time gate has not expired, calculate the remaining time and wait
                    long remainingTime = _timeGate - elapsedTime;
                    Thread.Sleep((int)remainingTime);
                }

                // Reset the stopwatch for the next gate
                _stopwatch.Restart();

                var response = new RecordResponse { Type = ProtoMessageType.RecordResponseType, Data = message };
                Send(response.ToByteArray());
            }
            finally
            {
                // Release the lock
                _writeLock.Release();
            }

        }

        public async Task StopPlay(StopPlayRequest request)
        {
            if (World.Player == null)
                return;
        }

        public async Task StopRecorder(StopRecordRequest request)
        {
            if (World.Player == null)
                return;

            if (_recorder != null)
            {
                _recorder.Stop();
                _recorder = null;
            }
            var response = new StopRecordResponse
            {
                Type = ProtoMessageType.StopRecordResponseType,                
                Success = true
            };
            Send(response.ToByteArray());
        }


        static ProtoMessageType GetMessageType(byte[] buffer)
        {
            // The first field (tag 1) in the message is the MessageType enum
            // We only need to read the first few bytes to determine the message type
            using (var input = new CodedInputStream(buffer))
            {
                uint tag = input.ReadTag();
                if (tag>0 && WireFormat.GetTagFieldNumber(tag) == 1)
                {
                    int enumValue = input.ReadEnum();
                    return (ProtoMessageType)enumValue;
                }
            }
            throw new InvalidOperationException("Unable to determine message type");
        }


        protected override void OnOpen()
        {
            _stopwatch.Start();
            Utility.Logger.Debug("WebSocket connection established.");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            _stopwatch.Stop();
            Console.WriteLine("WebSocket connection closed.");
        }
    }

    public class ProtoControlServer
    {
        internal int? AssignedPort { get; private set; }

        private static ProtoControlServer _instance = null;
        private static readonly object _lock = new object();
        private WebSocketServer _server;

        private ProtoControlServer()
        {

        }

        public static ProtoControlServer Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ProtoControlServer();
                    }
                    return _instance;
                }
            }
        }

        public bool Start(IPAddress ip)
        {
            AssignedPort = 0;
            int startingPort = 15454;  // Starting port
            int numberOfPorts = 10;   // Number of ports to check
            int port = FindAvailablePort(startingPort, numberOfPorts);
            if (port < 1)
            {
                AssignedPort = null;
                Utility.Logger.Error($"Unable to find a suitable port");
                return false;
            }
            AssignedPort = port;

            string connection = $"ws://localhost:{AssignedPort}";
            _server = new WebSocketServer(connection);
            _server.AddWebSocketService<ProtoMessageParser>("/proto");
            _server.Start();
            Utility.Logger.Info($"WebSocket Server started at {connection}.");
            return true;
        }

        public void Stop()
        {
            _server.Stop();
            Console.WriteLine("WebSocket Server stopped.");
            _server = null;
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
