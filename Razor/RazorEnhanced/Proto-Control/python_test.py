import asyncio
import websockets
import time
from google.protobuf.message import DecodeError
from ProtoControl_pb2 import RecordRequest, PlayRequest, StopPlayRequest, ProtoMessageType, ProtoLanguage

async def record(websocket):
    record_request = RecordRequest(type=ProtoMessageType.RecordRequestType, language=ProtoLanguage.Python)
    await websocket.send(record_request.SerializeToString())
    try:
        while True:
            data = await websocket.recv()
            response = RecordResponse()
            response.ParseFromString(data)
            print(f"RecordResponse: {response.data}")
    except websockets.exceptions.ConnectionClosedOK:
        print("Record stream ended")

async def play(websocket):
    play_request = PlayRequest(
        type=ProtoMessageType.PlayRequestType,
        language=ProtoLanguage.Python,
        commands=['print("Hello World")', 'print(Player.Name)', 'print("Finished")']
    )
    await websocket.send(play_request.SerializeToString())
    try:
        while True:
            data = await websocket.recv()
            response = PlayResponse()
            response.ParseFromString(data)
            print(f"PlayResponse: {response.result}")
    except websockets.exceptions.ConnectionClosedOK:
        print("Play stream ended")

async def stop_play(websocket):
    stop_request = StopPlayRequest(type=ProtoMessageType.StopPlayRequestType)
    await websocket.send(stop_request.SerializeToString())
    data = await websocket.recv()
    response = StopPlayResponse()
    response.ParseFromString(data)
    print(f"StopPlayResponse: {response.success}")
    
async def main():
    async with websockets.connect("ws://localhost:15454/proto") as websocket:
        await record(websocket)
        await play(websocket)
        await stop_play(websocket)
        await asyncio.sleep(5)

if __name__ == "__main__":
    asyncio.run(main())
