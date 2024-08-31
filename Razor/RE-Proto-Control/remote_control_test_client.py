import grpc
import RemoteControl_pb2
import RemoteControl_pb2_grpc
import time

def run_record(stub):
    print("Testing Record API:")
    try:
        # Start recording
        record_stream = stub.Record(RemoteControl_pb2.RecordRequest(is_on=True, format="py"))
        
        # Read from the stream until it's closed or we decide to stop
        for i, response in enumerate(record_stream):
            print(f"Received: {response.data}")
            if i >= 9:  # Stop after 10 responses
                break
            time.sleep(0.1)  # Small delay to simulate real-time processing
        
        # Stop recording
        stub.Record(RemoteControl_pb2.RecordRequest(is_on=False, format="py"))
        print("Recording stopped.")
    except grpc.RpcError as e:
        print(f"Record API error: {e.code()}: {e.details()}")

def run_play(stub):
    print("\nTesting Play API:")
    play_request = RemoteControl_pb2.PlayRequest(
        format="py",
        commands=["command1", "command2", "command3"]
    )
    try:
        play_stream = stub.Play(play_request)
        
        for response in play_stream:
            print(f"Received: {response.result}")
            if response.is_finished:
                print("Play execution completed")
                break
    except grpc.RpcError as e:
        print(f"Play API error: {e.code()}: {e.details()}")

def run():
    server_address = '127.0.0.1:5454'
    
    with grpc.insecure_channel(server_address) as channel:
        try:
            stub = RemoteControl_pb2_grpc.RemoteControlStub(channel)
            run_record(stub)
            run_play(stub)
        except grpc.RpcError as e:
            print(f"Error connecting to server: {e.code()}: {e.details()}")

if __name__ == '__main__':
    run()
