using grpc_core_server_lib;
using System;

namespace grpc_server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = Server.Start("localhost", 50001);
            server.ShutdownTask.Wait();
            Console.WriteLine("Shutdown gRPC server");
            Console.ReadKey();
        }
    }
}
