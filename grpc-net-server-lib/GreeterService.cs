using Helloworld;
using System;

namespace grpc_net_server_lib
{
    /// <summary>
    /// Grpc.Tools still depends on Grpc.Core
    /// It's still early to migrate to Grpc.AspNetCore
    /// </summary>
    public class GreeterService : Greeter.GreeterBase
    {
    }
}
