import grpc
import ProtoControl_pb2
import ProtoControl_pb2_grpc
import asyncio
import time

async def read_stream_with_timeout(stream, timeout):
    try:
        # Read from the stream with a timeout
        response = await asyncio.wait_for(stream.read(), timeout)
        return response
    except asyncio.TimeoutError:
        # If the timeout occurs, return None or handle the situation
        #print("No data received within the timeout period.")
        return None
    except asyncio.CancelledError:
        #print("No data received operation cancelled.")
        return None

async def run_record(stub):
    record_request = ProtoControl_pb2.RecordRequest(language=ProtoControl_pb2.ProtoLanguage.UOSTEAM)
    duration = 15  # Record test duration
    print(f"Testing Record API for {duration} seconds:")
    timeout = 1  # Timeout for each read attempt in seconds
    record_stream = stub.Record(record_request)
    
    # Read from the stream for a specified duration
    start_time = time.time()
    
    while True:
        current_time = time.time()
        if current_time - start_time > duration:
            print("Desired duration reached")
            break

        remaining_time = duration - (current_time - start_time)
        read_timeout = min(timeout, remaining_time)
        read_timeout = remaining_time

        try:
            response = await read_stream_with_timeout(record_stream, read_timeout)
            if response is None:
                continue
        
            print(f"Received: {response.data}")
        except grpc.RpcError as e:
            print(f"Record API error: {e.code()}: {e.details()}")

async def run_play(stub):
    print("\nTesting Play API:")
    play_request = ProtoControl_pb2.PlayRequest(
        language=ProtoControl_pb2.ProtoLanguage.PYTHON,
        commands=["print(\"Hello World\")", "Misc.Pause(5000)", "print(\"Bye!\")"]
    )
    try:
        play_stream = stub.Play(play_request)
        
        async for response in play_stream:
            print(f"Received: {response.result}")
            if response.is_finished:
                print("Play execution completed")
                break
    except grpc.RpcError as e:
        print(f"Play API error: {e.code()}: {e.details()}")

async def run():
    server_address = '127.0.0.1:5454'
    
    async with grpc.aio.insecure_channel(server_address) as channel:
        try:
            stub = ProtoControl_pb2_grpc.ProtoControlStub(channel)
            await run_record(stub)
            await run_play(stub)
        except grpc.RpcError as e:
            print(f"Error connecting to server: {e.code()}: {e.details()}")

if __name__ == '__main__':
    asyncio.run(run())