using Grpc.Core;
using Helloworld;
using System;

namespace grpc_lib
{
    /// <summary>
    /// Grpc.Core becomes “Maintenance Only”, When: May 2021
    /// Grpc.Core becomes “Deprecated”, When: May 2022
    /// </summary>
    [Obsolete]
    public class ClientCore
    {
        private static byte[] data = new byte[100]; //has to be longer than request data or throws exception
        private static long len = 0;

        public void SendTestRequest(string host, int port, string sender, int count)
        {
            try
            {
                var descriptor = Greeter.Descriptor;
                Console.WriteLine(descriptor);

                //build channel
                var channel = new Channel(host, port, ChannelCredentials.Insecure);

                //build client
                var client = new Greeter.GreeterClient(channel);

                //build request
                var request = new HelloRequest { Name = sender + "-" + count, Surname = "abcdefg", Address = "aaaa" };
                //request to array
                var req_output_stream = new Google.Protobuf.CodedOutputStream(data);
                request.WriteTo(req_output_stream);
                //parse array to request
                len = req_output_stream.Position;
                var req = new byte[len];
                Array.Copy(data, req, len);
                var parsed_request = HelloRequest.Parser.ParseFrom(req);

                Console.WriteLine($"Sending[{count}]: Size={request.CalculateSize()}, StreamSize={req_output_stream.Position}/{req_output_stream.SpaceLeft}, ToString={request}, Parsed={parsed_request}");

                //send request
                var reply = client.SayHello(request);
                

                //response to array
                var rep_output_stream = new Google.Protobuf.CodedOutputStream(data);
                reply.WriteTo(rep_output_stream);
                //parse array to response
                len = rep_output_stream.Position;
                var rep = new byte[len];
                Array.Copy(data, rep, len);
                var parsed_reply = GetTestReplyFromArray(rep);

                Console.WriteLine($"Reply[{count}]: Size={reply.CalculateSize()}, StreamSize={rep_output_stream.Position}/{rep_output_stream.SpaceLeft}, ToString={reply}, Parsed={parsed_reply}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("XXX | sending error: " + ex);
            }
        }

        public byte[] GetTestReplyAsArray()
        {
            var d = new byte[17] { 10,15,72,101,108,108,111,32,67,111,110,115,111,108,101,45,52 };
            return d;
        }

        public HelloReply GetTestReplyFromArray(byte[] d)
        {
            return HelloReply.Parser.ParseFrom(d);
        }
    }
}
