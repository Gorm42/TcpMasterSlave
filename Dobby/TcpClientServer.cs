using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using System.Net.Http;
//using System.Security.Cryptography;
//using SystemSecurity.Cryptography.X509Certificates; <-- is this neccessary?


namespace Dobby
{
    public class TCPClientServer
    {
        private readonly string _host;
        private readonly int _port;

        public TCPClientServer(string host = "localhost", int port = 8080)
        {
            _host = host;
            _port = port;
        }

        public async Task StartAsync()
        {
            using TcpClient client = new TcpClient();
            await client.ConnectAsync(_host, _port);

            Console.WriteLine($"Connected to server at {_host}:{_port}");

            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream);
            using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            //task to receive messages continuously
            Task receiveTask = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        string jsonReceived = await reader.ReadLineAsync();
                        if (jsonReceived == null) break;
                        // Connection closed
                        Message message = JsonSerializer.Deserialize<Message>(jsonReceived);
                        Console.WriteLine($"[{message.Sender}]: {message.Text}");
                        //Message messageReceived = System.Text.Json.JsonSerializer.Deserialize<Message>(jsonReceived);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error receiving message: {ex.Message}");
                        break;
                    }
                }
            });

            bool running = true;

            while (running)
            {
                string input = Console.ReadLine();

                if (input?.ToLower() == "sock")
                {
                    running = false;
                }
                else
                {
                    Message message = new Message("Client", input);
                    string jsonMessage = JsonSerializer.Serialize(message);
                    await writer.WriteLineAsync(jsonMessage);
                }



            }

            await receiveTask; //wait for the receive task to complete 
            Console.WriteLine("Connection closed.");
        }




    }
}
