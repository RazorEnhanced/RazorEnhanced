using Assistant;
using Google.Protobuf;
using IronPython.Runtime;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using RazorEnhanced.UOS;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace RazorEnhanced
{
    public class ProtoMessageParser : WebSocketBehavior
    {
        internal class SessionData
        {
            internal int _sessionID;
            internal ScriptRecorder _recorder;
            internal Thread _playThread;
            internal bool _shouldTerminate;

            internal SessionData(int sessionID, ScriptRecorder scriptRecorder = null, Thread playThread = null)
            {
                _sessionID = sessionID;
                _recorder = scriptRecorder;
                _playThread = playThread;
                _shouldTerminate = false;
            }
        }
        internal ProtoMessageType _messageType = 0;
        internal int _sessionID = 0;
        private readonly Stopwatch _stopwatch = new();
        private readonly long _timeGate = 100; // 100 milliseconds max output to remote. note tried 50 and it overflows

        private readonly SemaphoreSlim _writeLock = new(1, 1);

        internal static ConcurrentDictionary<int, SessionData> _sessionTracker = new();

        protected override void OnMessage(MessageEventArgs e)
        {
            // Convert the buffer to string
            string receivedMessage = Encoding.UTF8.GetString(e.RawData);

            // Deserialize the received message            
            var msgId = GetMessageTypeAndSessionID(e.RawData);
            _messageType = msgId.type;
            _sessionID = msgId.sessionId;
            if (_messageType == 0 || _sessionID == 0)
            {
                Utility.Logger.Error($"Invalid messageType: {_messageType} and/or sessionID: {_sessionID}");
                return;
            }
            switch (_messageType)
            {
                case ProtoMessageType.PlayRequestType:
                    {
                        var request = PlayRequest.Parser.ParseFrom(e.RawData);
                        Play(request);
                        OutputPlayMessage("finished", false);
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
                    Utility.Logger.Debug($"Unreasonable message received: {_messageType}");
                    return;
            }
        }
        private TracebackDelegate TraceCallback(TraceBackFrame frame, string result, object payload)
        {
            if (_sessionID == 0)
                return TraceCallback;

            //bool shouldTerminate = false;
            if (_sessionTracker.ContainsKey(_sessionID))
            {
                SessionData sessionData = _sessionTracker[_sessionID];
                if (sessionData._shouldTerminate == true)
                    throw new OperationCanceledException("Script execution aborted");
            }
            return TraceCallback;
        }

        public async Task Play(PlayRequest request)
        {
            if (World.Player == null) return;
            if (request == null) return;

            SessionData sessionData =
                new(request.Sessionid, scriptRecorder: null, playThread: null);
            _sessionTracker[request.Sessionid] = sessionData;


            switch (request.Language)
            {
                case ProtoLanguage.Python:
                    {
                        PythonEngine pyEngine = new();

                        // Set up the trace function so we can force python to terminate
                        sessionData._shouldTerminate = false;

                        //Python.SetTrace(pyEngine.Engine, TraceCallback);

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

                        string combinedString = "";
                        foreach (var statement in request.Commands)
                        {
                            combinedString += statement.TrimEnd(new char[] { '\r', '\n' }) + "\n";
                        }

                        if (!pyEngine.Load(combinedString, null))
                        {
                            throw new OperationCanceledException();
                        }
                        try
                        {
                            //TracebackDelegate traceFn = TraceCallback;
                            TracebackDelegate traceFn = (frame, result, _) => TraceCallback(frame, result, null);
                            pyEngine.SetTrace(traceFn);
                            sessionData._playThread = new Thread(() => RunPython(pyEngine));
                            sessionData._playThread.Start();
                            sessionData._playThread.Join();
                            sessionData._playThread = null;
                            OutputPlayMessage("", false);
                        }
                        catch (Exception ex)
                        {
                            Utility.Logger.Error($"Play Python failed {ex}");
                        }
                    }
                    break;
                case ProtoLanguage.Uosteam:
                    {
                        UOSteamEngine uosEngine = new();
                        if (uosEngine != null)
                            try
                            {
                                uosEngine.SetStderr(
                                    async (string message) =>
                                    {
                                        message = message.TrimEnd(new char[] { '\r', '\n' });
                                        OutputPlayMessage(message, true);
                                    }
                                );
                                uosEngine.SetStdout(
                                    async (string message) =>
                                    {
                                        message = message.TrimEnd(new char[] { '\r', '\n' });
                                        OutputPlayMessage(message, true);
                                    }
                                );
                                string combinedString = "";
                                foreach (var statement in request.Commands)
                                {
                                    combinedString += statement.TrimEnd(new char[] { '\r', '\n' }) + "\n";
                                }

                                uosEngine.Load(combinedString, "");
                                sessionData._playThread = new Thread(() => RunUOS(uosEngine));
                                sessionData._playThread.Start();
                                sessionData._playThread.Join();
                                sessionData._playThread = null;

                            }
                            catch (Exception ex)
                            {
                                Utility.Logger.Error($"Play UOS failed {ex}");
                            }
                        OutputPlayMessage("", false);
                    }
                    break;
                case ProtoLanguage.Csharp:
                    {
                        CSharpEngine csEngine = CSharpEngine.Instance;
                        if (csEngine != null)
                            try
                            {
                                /*
                                csEngine.SetStderr(
                                    async (string message) =>
                                    {
                                        message = message.TrimEnd(new char[] { '\r', '\n' });
                                        OutputPlayMessage(message, true);
                                    }
                                );
                                csEngine.SetStdout(
                                    async (string message) =>
                                    {
                                        message = message.TrimEnd(new char[] { '\r', '\n' });
                                        OutputPlayMessage(message, true);
                                    }
                                );
                                */
                                string combinedString = "";
                                foreach (var statement in request.Commands)
                                {
                                    combinedString += statement.TrimEnd(new char[] { '\r', '\n' }) + "\n";
                                }

                                List<string> compileMessages;
                                Assembly assembly;
                                if (!csEngine.CompileFromText(combinedString, out compileMessages, out assembly))
                                {
                                    sessionData._playThread = new Thread(() => RunCSharp(csEngine, assembly));
                                    sessionData._playThread.Start();
                                    sessionData._playThread.Join();
                                    sessionData._playThread = null;
                                }
                                else
                                {
                                    foreach (var line in compileMessages)
                                    {
                                        OutputPlayMessage(line, true, false);
                                    }
                                }
                                OutputPlayMessage("", false);

                            }
                            catch (Exception ex)
                            {
                                Utility.Logger.Error($"Play UOS failed {ex}");
                            }
                    }

                    break;
                default:
                    throw new OperationCanceledException();
                    break;
            }

            if (_sessionTracker.ContainsKey(request.Sessionid)
                && !_sessionTracker.TryRemove(request.Sessionid, out _))
                Utility.Logger.Error($"Unable to remove Play session key from dictionary");
        }

        // I think these run functions could be factored to all be the same
        internal void RunPython(PythonEngine engine)
        {
            string message = "";
            try
            {
                engine.Execute();
            }
            catch (OperationCanceledException stop)
            {
                message = stop.Message;
            }
            catch (Exception ex)
            {
                message += "Python Error:";
                ExceptionOperations eo = engine.Engine.GetService<ExceptionOperations>();
                string error = eo.FormatException(ex);
                message += Regex.Replace(error.Trim(), "\n\n", "\n");     //remove empty lines
            }

            OutputPlayMessage(message, true);
        }
        internal void RunUOS(UOSteamEngine engine)
        {
            string message = "";
            try
            {
                engine.Execute();
            }
            catch (OperationCanceledException stop)
            {
                message = stop.Message;
            }
            catch (UOSSyntaxError ex)
            {
                message = ex.Message;
            }
            catch (UOSStopError ex)
            {
                message = $"stop keyword encountered at line {ex.LineNumber + 1}";
            }
            catch (Exception ex)
            {
                message += "UOS Error:";
                string error = ex.ToString();
                message += Regex.Replace(error.Trim(), "\n\n", "\n");     //remove empty lines
            }

            OutputPlayMessage(message, true);
        }
        internal void RunCSharp(CSharpEngine engine, Assembly program)
        {
            string message = "";
            try
            {
                engine.Execute(program);
            }
            catch (Exception ex)
            {
                message += "C# Error:"; ;
                string error = ex.ToString().Trim();
                message += Regex.Replace(error, "\n\n", "\n");     //remove empty lines
            }
            OutputPlayMessage(message, true);
        }

        public async Task StopPlay(StopPlayRequest request)
        {
            if (World.Player == null) return;
            if (request == null) return;
            if (_sessionTracker.ContainsKey(request.Sessionid))
            {
                SessionData sessionData = _sessionTracker[request.Sessionid];
                sessionData._shouldTerminate = true;
                Thread.Sleep(2000); // it should end itself
                if (sessionData._playThread != null) // if not kill it
                    sessionData._playThread.Abort();
            }
        }

        private async Task OutputPlayMessage(string message, bool more, bool sendInGame = true)
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

                if (sendInGame)
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
            if (World.Player == null) return;
            if (request == null) return;

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
                SessionData sessionData =
                    new(request.Sessionid, ScriptRecorderService.RecorderForLanguage(language));
                _sessionTracker[request.Sessionid] = sessionData;

                try
                {
                    sessionData._recorder.Output = async (code) =>
                    {
                        // Create a response based on the request
                        code = code.TrimEnd(new char[] { '\r', '\n' });
                        OutputRecordMessage(request.Sessionid, code);
                    };
                    sessionData._recorder.Start();
                    while (sessionData._recorder != null && sessionData._recorder.IsRecording())
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
                    if (sessionData._recorder != null)
                        sessionData._recorder.Stop();
                }
            }
            else
            {
                throw new OperationCanceledException();
            }

            var response = new RecordResponse
            {
                Type = ProtoMessageType.RecordResponseType,
                Sessionid = request.Sessionid,
                More = false,
                Data = "Recording Stopped"
            };
            Send(response.ToByteArray());
            if (_sessionTracker.ContainsKey(request.Sessionid)
                && !_sessionTracker.TryRemove(request.Sessionid, out _))
                Utility.Logger.Error($"Unable to remove Record session key from dictionary");
        }

        private async Task OutputRecordMessage(int sessionID, string message)
        {
            if (_sessionTracker.ContainsKey(sessionID))
            {
                SessionData sessionData = _sessionTracker[sessionID];
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

                    bool more = false;
                    if (sessionData._recorder != null)
                        more = sessionData._recorder.IsRecording();
                    var response = new RecordResponse
                    {
                        Type = ProtoMessageType.RecordResponseType,
                        Sessionid = sessionID,
                        More = more,
                        Data = message
                    };
                    Send(response.ToByteArray());

                }
                finally
                {
                    // Release the lock
                    _writeLock.Release();
                }
            }

        }

        public async Task StopRecorder(StopRecordRequest request)
        {
            if (World.Player == null) return;
            if (request == null) return;

            if (_sessionTracker.ContainsKey(request.Sessionid))
            {
                SessionData sessionData = _sessionTracker[request.Sessionid];

                if (sessionData._recorder != null)
                {
                    sessionData._recorder.Stop();
                }

                var stopResponse = new StopRecordResponse
                {
                    Type = ProtoMessageType.StopRecordResponseType,
                    Sessionid = request.Sessionid,
                    Success = true
                };
                Send(stopResponse.ToByteArray());
            }
        }


        (ProtoMessageType type, int sessionId) GetMessageTypeAndSessionID(byte[] buffer)
        {
            // The first field (tag 1) in the message is the MessageType enum
            // We only need to read the first few bytes to determine the message type
            using var input = new CodedInputStream(buffer);
            ProtoMessageType messageType = 0;
            int sessionID = 0;
            uint tag = input.ReadTag();
            if (tag > 0 && WireFormat.GetTagFieldNumber(tag) == 1)
            {
                int enumValue = input.ReadEnum();
                messageType = (ProtoMessageType)enumValue;
            }
            tag = input.ReadTag();
            if (tag > 0 && WireFormat.GetTagFieldNumber(tag) == 2)
            {
                sessionID = input.ReadInt32();
            }
            return (messageType, sessionID);
        }


        protected override void OnOpen()
        {
            _stopwatch.Start();
            Utility.Logger.Debug("WebSocket connection established.");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            // attempt to terminate all sessions
            foreach (var session in _sessionTracker)
            {
                if (session.Value._recorder != null)
                {
                    var response = new RecordResponse
                    {
                        Type = ProtoMessageType.RecordResponseType,
                        Sessionid = session.Value._sessionID,
                        More = false,
                        Data = "Recording Stopped"
                    };
                    Send(response.ToByteArray());
                    session.Value._recorder.Stop();
                }
                if (session.Value._playThread != null)
                {
                    OutputPlayMessage("Process Ended", false, false);
                    session.Value._playThread.Abort();
                }
            }
            _stopwatch.Stop();
            Utility.Logger.Debug("WebSocket connection closed.");
        }
    }

    public class ProtoControlServer
    {
        internal int? AssignedPort { get; private set; }

        private static ProtoControlServer _instance = null;
        private static readonly object _lock = new();
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
            int port = FindAvailablePort(ip, startingPort, numberOfPorts);
            if (port < 1)
            {
                AssignedPort = null;
                Utility.Logger.Error($"Unable to find a suitable port");
                return false;
            }
            AssignedPort = port;

            try
            {
                string connection = $"ws://{ip.ToString()}:{AssignedPort}";
                _server = new WebSocketServer(connection);
                _server.ReuseAddress = true;
                _server.AddWebSocketService<ProtoMessageParser>("/proto");
                _server.Start();
                Utility.Logger.Info($"WebSocket Server started at {connection}.");
                return true;
            }
            catch (Exception)
            {
                AssignedPort = 0;
                _server = null;
            }
            return false;
        }

        public void Stop()
        {
            if (_server != null)
            {
                _server.Stop();
                Utility.Logger.Debug("WebSocket Server stopped.");
            }
            _server = null;
        }
        public static int FindAvailablePort(IPAddress ip, int startingPort, int range)
        {
            for (int port = startingPort; port < startingPort + range; port++)
            {
                if (IsPortAvailable(ip, port))
                {
                    return port;
                }
            }
            return -1;  // Return -1 if no port is available
        }

        private static bool IsPortAvailable(IPAddress ip, int port)
        {
            TcpListener listener = null;
            try
            {
                // Create a TCP listener on the specified port
                listener = new TcpListener(ip, port);
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
