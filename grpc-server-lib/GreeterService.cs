using Grpc.Core;
using Helloworld;
using System;
using System.Threading.Tasks;

namespace grpc_server_lib
{
    public class GreeterService : Greeter.GreeterBase
    {
        private int count;

        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            count++;
            Console.WriteLine($"Request[{count}]: {request.Name}");
            var response = new HelloReply { Message = "Hello " + request.Name };
            return Task.FromResult(response);
        }

        // Server side handler of the GetHello RPC
        public override Task<HelloReply> GetHello(HelloIdRequest request, ServerCallContext context)
        {
            count++;
            Console.WriteLine($"Request[{count}]: {request.Id}");
            var response = new HelloReply { Message = "Hello " + request.Id };
            return Task.FromResult(response);
        }
    }
}
