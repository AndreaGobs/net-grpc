using Grpc.Core;
using Grpc.Reflection;
using Grpc.Reflection.V1Alpha;
using Helloworld;
using System;

namespace grpc_server_lib
{
    public class ServerCore
    {
        public static Server Start(string host, int port)
        {
            var descriptor = Greeter.Descriptor;
            Console.WriteLine("Starting gRPC server, descriptor=" + descriptor);

            Server server = new Server
            {
                Services =
                {
                    Greeter.BindService(new GreeterService()),                                                               //grpc service
                    ServerReflection.BindService(new ReflectionServiceImpl(Greeter.Descriptor, ServerReflection.Descriptor)) //grpc service reflection (https://github.com/grpc/grpc/blob/master/doc/csharp/server_reflection.md) (no proto file needed for gRPCurl...)
                },
                Ports = { new ServerPort(host, port, ServerCredentials.Insecure) }
            };
            server.Start();
            Console.WriteLine($"Started gRPC server at http://{host}:{port}");

            return server;
        }
    }
}
