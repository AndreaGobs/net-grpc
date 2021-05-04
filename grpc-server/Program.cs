using grpc_server_lib;
using System;

namespace grpc_server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = ServerCore.Start("localhost", 50001);
            server.ShutdownTask.Wait();
            Console.WriteLine("Shutdown gRPC server");
            Console.ReadKey();
        }
    }
}
