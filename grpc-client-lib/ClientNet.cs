using Google.Protobuf;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Helloworld;
using System;
using System.Net.Http;

namespace grpc_lib
{
    public class ClientNet
    {
        public void SendTestRequest(string host, int port, string sender, int count)
        {
            try
            {
                var descriptor = Greeter.Descriptor;
                Console.WriteLine(descriptor);

                //build channel
                // Untrusted certificates should only be used during app development. Production apps should always use valid certificates.
                var httpHandler = new HttpClientHandler();
                httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator; // Return `true` to allow certificates that are untrusted/invalid

                var channel = GrpcChannel.ForAddress($"http://{host}:{port}", new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, httpHandler)
                });

                //build client
                var client = new Greeter.GreeterClient(channel);

                //build request
                var request = new HelloRequest { Name = sender + "-" + count, Surname = "abcdefg", Address = "aaaa" };

                //request to array
                var req = request.ToByteArray();
                var parsed_request = HelloRequest.Parser.ParseFrom(req);
                Console.WriteLine($"Sending[{count}]: Size={request.CalculateSize()}, StreamSize={req.Length}, ToString={request}, Parsed={parsed_request}");

                ////request to array
                //var req_output_stream = new Google.Protobuf.CodedOutputStream(data);
                //request.WriteTo(req_output_stream);
                ////parse array to request
                //len = req_output_stream.Position;
                //var req = new byte[len];
                //Array.Copy(data, req, len);
                //var parsed_request = HelloRequest.Parser.ParseFrom(req);
                //Console.WriteLine($"Sending[{count}]: Size={request.CalculateSize()}, StreamSize={req_output_stream.Position}/{req_output_stream.SpaceLeft}, ToString={request}, Parsed={parsed_request}");

                //send request
                var reply = client.SayHello(request);

                //response to array
                var rep = reply.ToByteArray();
                var parsed_reply = HelloReply.Parser.ParseFrom(rep);
                Console.WriteLine($"Reply[{count}]: Size={reply.CalculateSize()}, StreamSize={rep.Length}, ToString={reply}, Parsed={parsed_reply}");

                ////response to array
                //var rep_output_stream = new Google.Protobuf.CodedOutputStream(data);
                //reply.WriteTo(rep_output_stream);
                ////parse array to response
                //len = rep_output_stream.Position;
                //var rep = new byte[len];
                //Array.Copy(data, rep, len);
                //var parsed_reply = GetTestReplyFromArray(rep);
                //Console.WriteLine($"Reply[{count}]: Size={reply.CalculateSize()}, StreamSize={rep_output_stream.Position}/{rep_output_stream.SpaceLeft}, ToString={reply}, Parsed={parsed_reply}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("XXX | sending error: " + ex);
            }
        }

        public byte[] GetTestReplyAsArray()
        {
            var d = new byte[17] { 10, 15, 72, 101, 108, 108, 111, 32, 67, 111, 110, 115, 111, 108, 101, 45, 52 };
            return d;
        }

        public HelloReply GetTestReplyFromArray(byte[] d)
        {
            return HelloReply.Parser.ParseFrom(d);
        }
    }
}
