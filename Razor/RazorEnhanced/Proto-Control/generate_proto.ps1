#protoc --python_out=. your_proto_file.proto
#python -m grpc_tools.protoc -I. --python_out=. --grpc_python_out=. ProtoControl.proto

$toolsDir = "..\..\..\packages\Grpc.Tools.2.70.0\tools\windows_x64"
$protoc = "$toolsDir\protoc.exe"
$plugin = "$toolsDir\grpc_csharp_plugin.exe"

& $protoc `
  --csharp_out=. `
  --grpc_out=. `
  "--plugin=protoc-gen-grpc=$plugin" `
  ProtoControl.proto

Write-Host "Generated: ProtoControl.cs, ProtoControlGrpc.cs"