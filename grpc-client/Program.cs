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
                if (key == "a")
                {
                    count++;
                    client.SendSayHelloRequest("localhost", 50001, "Console", count);
                }
                else if (key == "b")
                {
                    count++;
                    client.SendGetHelloRequest("localhost", 50001, "Console", count);
                }
                else if (key == "c")
                {
                    count++;
                    client.SendGetStreamHelloRequest("localhost", 50001, "Console", count);
                }
                else
                {
                    Console.WriteLine("Wrong key");
                }
            }
        }
    }
}
