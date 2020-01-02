/// <summary>
/// C# version of nngcat.
/// https://nanomsg.github.io/nng/man/v1.1.0/nngcat.1
/// </summary>
/// <example>
/// <code>
/// 
/// ========= Original code: https://github.com/jeikabu/nng.NETCore/blob/master/examples/nngcat/Program.cs ============
/// =================== Modified by @rahulraj80 [GitHub] == 2020/01/03 ================================================
/// 
/// Sending or Receiving toKill stops the loop
/// 
/// Pair 1 Example:
/// ===================================================================================================================
/// Run the following in two separate terminals - or, NNGCat can speak to any other NNG enabled speaker.
/// Listener waits for dialer to send the first message
/// 
/// 
/// dotnet run -- --pair --listen tcp://localhost:8080  // This will use Pair1 protocol, best to run listener first
/// dotnet run -- --pair --dial tcp://localhost:8080    // This will use Pair1 protocol.
/// 
/// Or for the compiled exe:
/// nngcat.exe --pair --listen tcp://127.0.0.1:8064     
/// nngcat.exe --pair --dial tcp://127.0.0.1:8064
///
/// Pair0/ Rep/Req example
/// ===================================================================================================================
/// dotnet run -- --rep --listen tcp://localhost:8080
/// dotnet run -- --req --dial tcp://localhost:8080
/// 
/// =================== End Modification by @rahulraj80 [GitHub] == 2020/01/03 ========================================
/// 
/// 
/// 
/// </code>
/// </example>
using Microsoft.Extensions.CommandLineUtils;
using nng;
using System;
using System.IO;
using System.Text;

namespace nngcat
{
    class Program
    {
        static string listenForMessage(IPairSocket socket)
        {
            Console.WriteLine("Waiting for message .....");
            var request = socket.RecvMsg().Unwrap();
            var str = Encoding.ASCII.GetString(request.AsSpan());
            Console.WriteLine("Received: " + str); // will exit if this is 'toKill' as well
            return str;
        }
        static string sendMessage(IPairSocket socket, IAPIFactory<IMessage> factory)
        {
            Console.WriteLine("What to send?");
            var reply_text = Console.ReadLine();
            var reply = factory.CreateMessage();
            var replyBytes = Encoding.ASCII.GetBytes(reply_text == "" ? " " : reply_text);
            reply.Append(replyBytes);
            socket.SendMsg(reply);
            return reply_text;
        }
        
        static string listenForMessage(IRepSocket socket)
        {
            Console.WriteLine("Waiting for message .....");
            var request = socket.RecvMsg().Unwrap();
            var str = Encoding.ASCII.GetString(request.AsSpan());
            Console.WriteLine("Received: " + str); // will exit if this is 'toKill' as well
            return str;
        }
        static string sendMessage(IRepSocket socket, IAPIFactory<IMessage> factory)
        {
            Console.WriteLine("What to send?");
            var reply_text = Console.ReadLine();
            var reply = factory.CreateMessage();
            var replyBytes = Encoding.ASCII.GetBytes(reply_text == "" ? " " : reply_text);
            reply.Append(replyBytes);
            socket.SendMsg(reply);
            return reply_text;
        }
        
        static string listenForMessage(IReqSocket socket)
        {
            Console.WriteLine("Waiting for message .....");
            var request = socket.RecvMsg().Unwrap();
            var str = Encoding.ASCII.GetString(request.AsSpan());
            Console.WriteLine("Received: " + str); // will exit if this is 'toKill' as well
            return str;
        }
        static string sendMessage(IReqSocket socket, IAPIFactory<IMessage> factory)
        {
            Console.WriteLine("What to send?");
            var reply_text = Console.ReadLine();
            var reply = factory.CreateMessage();
            var replyBytes = Encoding.ASCII.GetBytes(reply_text == "" ? " " : reply_text);
            reply.Append(replyBytes);
            socket.SendMsg(reply);
            return reply_text;
        }

        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "nngcat";
            app.HelpOption("-? | -h | --help");
            var pairOpt = app.Option("--pair", "Pair Option", CommandOptionType.NoValue);
            var repOpt = app.Option("--rep", "Rep protocol", CommandOptionType.NoValue);
            var reqOpt = app.Option("--req", "Req protocol", CommandOptionType.NoValue);
            var listenOpt = app.Option("--bind | --listen <URL>", "Connect to the peer at the address specified by URL.", CommandOptionType.SingleValue);
            var dialOpt = app.Option("--connect | --dial <URL>", "Bind to, and accept connections from peers, at the address specified by URL.", CommandOptionType.SingleValue);
            // Receive options
            var quotedOpt = app.Option("-Q | --quoted", "Currently always enabled", CommandOptionType.NoValue);
            // Transmit options
            var dataOpt = app.Option("-D | --data <DATA>", "Use DATA for the body of outgoing messages.", CommandOptionType.SingleValue);

            app.OnExecute(() => {
                var path = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var ctx = new nng.NngLoadContext(path);
                var factory = nng.NngLoadContext.Init(ctx);

                string str = "";

                if (pairOpt.HasValue())
                {
                    using (var socket = factory.PairOpen().listenOrDial(listenOpt, dialOpt))
                    {
                        
                        if (listenOpt.HasValue()) // Listener listens first
                        {
                            while (str != "toKill") // This message will shut this down, either when you send, or receive. 
                            {
                                if(listenForMessage(socket) == "toKill") break;
                                if (sendMessage(socket, factory) == "toKill") break;
                            }
                        }
                        else
                        {
                            while (str != "toKill") // This message will shut this down, either when you send, or receive. 
                            {
                                if (sendMessage(socket, factory) == "toKill") break;
                                if (listenForMessage(socket) == "toKill") break;
                            }
                        }
                    }
                }
                else if (repOpt.HasValue())
                {
                    using (var socket = factory.ReplierOpen().listenOrDial(listenOpt, dialOpt))
                    {
                        while (str != "toKill")
                        {
                            if (listenForMessage(socket) == "toKill") break;
                            if (sendMessage(socket, factory) == "toKill") break;
                        }
                    }
                }
                else if (reqOpt.HasValue())
                {
                    using (var socket = factory.RequesterOpen().listenOrDial(listenOpt, dialOpt))
                    {
                        while (str != "toKill")
                        {
                            if (sendMessage(socket, factory) == "toKill") break;
                            if (listenForMessage(socket) == "toKill") break;
                        }
                    }
                }
                return 0;
            });

            app.Execute(args);
        }
    }

    static class SocketExt
    {
        public static T listenOrDial<T>(this NngResult<T> socket, CommandOption listenOpt, CommandOption dialOpt)
            where T : ISocket
        {
            if (listenOpt.HasValue())
                return socket.ThenListen(listenOpt.Value()).Unwrap();
            else if (dialOpt.HasValue())
                return socket.ThenDial(dialOpt.Value()).Unwrap();
            else
                throw new ArgumentException("Must --listen or --dial");
        }
    }
}
