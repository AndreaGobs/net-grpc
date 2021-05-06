using Grpc.Core;
using Helloworld;
using System;
using System.Threading.Tasks;

namespace grpc_core_server_lib
{
    /// <summary>
    /// Grpc.Core becomes “Maintenance Only”, When: May 2021
    /// Grpc.Core becomes “Deprecated”, When: May 2022
    /// </summary>
    [Obsolete]
    public class GreeterService : Greeter.GreeterBase
    {
        private int count;

        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            count++;
            Console.WriteLine($"Request[{count}]: {request}");
            var response = new HelloReply { Message = "Hello " + request.Name };
            return Task.FromResult(response);
        }

        // Server side handler of the GetHello RPC
        public override Task<HelloReply> GetHello(HelloIdRequest request, ServerCallContext context)
        {
            count++;
            Console.WriteLine($"Request[{count}]: {request}");
            var response = new HelloReply { Message = "Hello " + request.Id };
            return Task.FromResult(response);
        }

        // Server side handler of the GetStreamHello RPC
        public override async Task GetStreamHello(HelloIdRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            count++;
            Console.WriteLine($"Request[{count}]: {request}");
            for (int i = 1; i <= request.Id; i++)
            {
                var response = new HelloReply { Message = "Hello stream " + i + "/" + request.Id };
                await responseStream.WriteAsync(response);
            }
        }
    }
}
