using grpc_lib;
using System;

namespace grpc_client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("gRPC client tester");
            int count = 0;
            //var client = new ClientCore();
            var client = new ClientNet();

            while (true)
            {
                var key = Console.ReadLine();
                if (key == "q")
                {
                    break;
                }
                if (key == "s")
                {
                    count++;
                    client.SendTestRequest("localhost", 50001, "Console", count);
                }
            }
        }
    }
}
