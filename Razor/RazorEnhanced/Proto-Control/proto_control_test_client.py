import asyncio
import websockets
import time

from google.protobuf.message import DecodeError
from ProtoControl_pb2 import PlayRequest, PlayResponse, StopPlayRequest, StopPlayResponse 
from ProtoControl_pb2 import ProtoMessageType, ProtoLanguage
from ProtoControl_pb2 import RecordRequest, RecordResponse, StopRecordRequest

async def receive_message_with_timeout(websocket, timeout=5):
    try:
        data = await asyncio.wait_for(websocket.recv(), timeout=timeout)
        response = RecordResponse()
        response.ParseFromString(data)
        return response
    except asyncio.TimeoutError:
        print("Timeout waiting for WebSocket message.")
    except websockets.ConnectionClosedOK:
        print("Connection closed.")
    return None



async def websocket_client():
    uri = "ws://localhost:15454/proto"
    
    async with websockets.connect(uri) as websocket:
        # Create a PlayRequest message
        request = PlayRequest(
            type=ProtoMessageType.PlayRequestType,
            language=ProtoLanguage.Python,
            commands=["print('Hello from Python')", "print(Player.Name)", "print('Success')"]
        )

        # Serialize the message
        request_data = request.SerializeToString()
        await websocket.send(request_data)
        print("PlayRequest sent to the server.")

        # Receive and deserialize the PlayResponse message
        try:
            more = True
            while True:
                data = await websocket.recv()
                response = PlayResponse()                
                response.ParseFromString(data)
                if not response.more:
                    break;
                print(f"PlayResponse: {response.result}")

        except websockets.exceptions.ConnectionClosedOK:
            print("Record stream ended")
        except DecodeError as e:
            print("Failed to parse response:", e)

        # Create a RecordRequest message
        request = RecordRequest(
            type=ProtoMessageType.RecordRequestType,
            language=ProtoLanguage.Python
        )

        # Serialize the message
        request_data = request.SerializeToString()
        await websocket.send(request_data)
        print("RecordRequest sent to the server.")

        # Read from the stream for a specified duration
        duration = 15  # Record test duration
        start_time = time.time()
        timeout = 1
        try:
            more = True
            while more:
                current_time = time.time()
                if current_time - start_time > duration:
                    print("Desired duration reached")
                    stop = StopRecordRequest(
                        type=ProtoMessageType.StopRecordRequestType
                        )
                    stop_data = request.SerializeToString()
                    await websocket.send(stop_data)
                    more = False
              
                remaining_time = duration - (current_time - start_time)
                read_timeout = min(timeout, remaining_time)
                read_timeout = remaining_time
                response = await receive_message_with_timeout(websocket, read_timeout)
                if response is None:
                    continue  

                print(f"RecordResponse: {response.data}")

        except websockets.exceptions.ConnectionClosedOK:
            print("Record stream ended")
        except DecodeError as e:
            print("Failed to parse response:", e)



asyncio.get_event_loop().run_until_complete(websocket_client())
