using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mastah
{
    public class TcpChatServer
    {
        private readonly int _port;

        public TcpChatServer(int port)
        {
            _port = port;
        }

        public async Task RunAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();
            Console.WriteLine($"Server started listening on port {_port}.");

            using TcpClient client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("Client connected.");

            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream);
            using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            // Task to receive messages continuously
            Task receiveTask = Task.Run(async () =>
            {
                //introducer noget switch-case statement evt. 

                while (true)
                {
                    try
                    {
                        string jsonReceived = await reader.ReadLineAsync();
                        if (jsonReceived == null || jsonReceived == "exitThisChat") 
                        {
                            break; 
                        }
                        
                        Message message = JsonSerializer.Deserialize<Message>(jsonReceived);
                        Console.WriteLine($"[{message.Sender}]: {message.Type}");

                        if (message.Type == "crack_result")
                        {
                            List<UserInfoClearText> results = JsonSerializer.Deserialize<List<UserInfoClearText>>(message.Data);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error receiving message: {ex.Message}");
                        break;
                    }
                }


                //this code works as well, but doesn't take json into account
                //string message;
                //while ((message = await reader.ReadLineAsync()) != null)
                //{
                //    Console.WriteLine($"Client: {message}");
                //}
            });

            //Loop to send messages from console
            while (true)
            {
                string input = Console.ReadLine();
                if (input?.ToLower() == "exit") break;

                Message message = new Message("Server", input);
                string jsonMessage = JsonSerializer.Serialize(message);
                await writer.WriteLineAsync(jsonMessage);

            }

            await receiveTask;
            Console.WriteLine("Connection Closed.");
        }

    }
}

