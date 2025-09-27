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
using System.Security.Cryptography;
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
                        if (jsonReceived == null)
                        {
                            break;
                        }
                        // Connection closed
                        Message message = JsonSerializer.Deserialize<Message>(jsonReceived);
                        //Console.WriteLine($"[{message.Sender}]: {message.Type}");
                        //Message messageReceived = System.Type.Json.JsonSerializer.Deserialize<Message>(jsonReceived);
                        if (message.Type == "crack_chunk")
                        {
                            List<UserInfo> chunk ? JsonSerializer.Deserialize<List<UserInfo>>(message.Data);

                            //Crack the chunk
                            List<UserInfoClearText> results = CrackChunk(chunk);

                            //send back the results
                            string resultsJson = JsonSerializer.Serialize(results);
                            Message resultsMessage = new Message("slave", "crack_result", resultsJson);
                            string resultJsonMessage = JsonSerializer.Serialize(resultsMessage);
                            await writer.WriteLineAsync(resultJsonMessage);

                            Console.WriteLine("Cracked chunk and sent results back.");
                        }
                        else
                        {
                            Console.WriteLine($"{message.Sender} : {message.Data}");
                        }
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
                    Message message = new Message("Client", "chat", input);
                    string jsonMessage = JsonSerializer.Serialize(message);
                    await writer.WriteLineAsync(jsonMessage);
                }
            }

            await receiveTask; //wait for the receive task to complete 
            Console.WriteLine("Connection closed.");
        }

        private List<UserInfoClearText> CrackChunk(List<UserInfo> chunk)
        {
            List<UserInfoClearText> results = new List<UserInfoClearText>();

            foreach (var userInfo in chunk)
            {
                bool cracked = false;
                for (int i = 0; i< 10000; i++)
                {
                    string candidate = i.ToString("D4");
                    byte[] candidateHash = ComputeSHA1(candidate);

                    if (candidateHash.SequenceEqual(unserInfo.EncryptedPassword))
                    {
                        results.Add(new UserInfoClearText(userInfo.Username, candidate));
                        Console.WriteLine($"Cracked {userInfo.Username} : {candidate}");
                        cracked = true;
                        break;
                    }
                }

                if (!cracked)
                {
                    results.Add(new UserInfoClearText(userInfo.Username, "NOT CRACKED"));
                }
            }

            return results;

        }

        private byte[] ComputeSHA1(string input)
        {
            using SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
        }


    }
}
