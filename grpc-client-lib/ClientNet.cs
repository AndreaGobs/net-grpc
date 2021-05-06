using Google.Protobuf;
using Grpc.Net.Client;
using Helloworld;
using System;
using System.Net.Http;
using System.Threading;

namespace grpc_lib
{
    public class ClientNet
    {
        public async void SendSayHelloRequest(string host, int port, string sender, int count)
        {
            try
            {
                //build client
                var client = BuildClient(host, port);

                //build request
                var request = BuildHelloRequest(sender, count);

                //send request
                var reply = await client.SayHelloAsync(request);

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

        public async void SendGetHelloRequest(string host, int port, string sender, int count)
        {
            try
            {
                //build client
                var client = BuildClient(host, port);

                //build request
                var request = BuildHelloIdRequest(sender, count);

                //send request
                var reply = await client.GetHelloAsync(request);

                //response to array
                var rep = reply.ToByteArray();
                var parsed_reply = HelloReply.Parser.ParseFrom(rep);
                Console.WriteLine($"Reply[{count}]: Size={reply.CalculateSize()}, StreamSize={rep.Length}, ToString={reply}, Parsed={parsed_reply}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("XXX | sending error: " + ex);
            }
        }

        public async void SendGetStreamHelloRequest(string host, int port, string sender, int count)
        {
            try
            {
                //build client
                var client = BuildClient(host, port);

                //build request
                var request = BuildHelloIdRequest(sender, count);

                //send request
                var reply = client.GetStreamHello(request);
                var header = await reply.ResponseHeadersAsync;
                var cancTokenSource = new CancellationTokenSource(); 
                while (await reply.ResponseStream.MoveNext(cancTokenSource.Token))
                {
                    var current = reply.ResponseStream.Current;

                    //current response to array
                    var rep = current.ToByteArray();
                    var parsed_reply = HelloReply.Parser.ParseFrom(rep);
                    Console.WriteLine($"Reply[{count}]: Size={current.CalculateSize()}, StreamSize={rep.Length}, ToString={reply}, Parsed={parsed_reply}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("XXX | sending error: " + ex);
            }
        }

        public byte[] GetSayHelloTestReplyAsArray()
        {
            var d = new byte[17] { 10, 15, 72, 101, 108, 108, 111, 32, 67, 111, 110, 115, 111, 108, 101, 45, 52 };
            return d;
        }

        public HelloReply GetSayHelloTestReplyFromArray(byte[] d)
        {
            return HelloReply.Parser.ParseFrom(d);
        }

        private Greeter.GreeterClient BuildClient(string host, int port)
        {
            var descriptor = Greeter.Descriptor;
            Console.WriteLine(descriptor);

            //build channel
            var channel = GrpcChannel.ForAddress($"http://{host}:{port}", new GrpcChannelOptions
            {
                //HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, httpHandler) //Grpc.Net.Client.Web
                HttpHandler = new HttpClientHandler()
                {
                    // Untrusted certificates should only be used during app development. Production apps should always use valid certificates.
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator // Return `true` to allow certificates that are untrusted/invalid
                }
            });

            //build client
            return new Greeter.GreeterClient(channel);
        }

        private HelloRequest BuildHelloRequest(string sender, int count)
        {
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

            return request;
        }

        private HelloIdRequest BuildHelloIdRequest(string sender, int count)
        {
            //build request
            var request = new HelloIdRequest { Id = new Random().Next(1, 10) };

            //request to array
            var req = request.ToByteArray();
            var parsed_request = HelloIdRequest.Parser.ParseFrom(req);
            Console.WriteLine($"Sending[{count}]: Size={request.CalculateSize()}, StreamSize={req.Length}, ToString={request}, Parsed={parsed_request}");

            return request;
        }
    }
}
