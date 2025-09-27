using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

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
            //consider making this part into its own method for SOLID principles.
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
                        

                        if (message.Type == "crack_result")
                        {
                            List<UserInfoClearText> results = JsonSerializer.Deserialize<List<UserInfoClearText>>(message.Data);
                            Console.WriteLine("Received crack results:");
                            foreach(UserInfoClearText result in results)
                            {
                                Console.WriteLine(result.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine($"[{message.Sender}]: {message.Type}");
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

            //Generate some sample UserInfo (hashed passwords)
            List<UserInfo> passwordList = GenerateSamplePasswords();

            //Split into chunks (e.g. 2 per chunk for demo)
            List<List<UserInfo>> chunks = ChunkList(passwordList, 2);

            //here we send the chunks to the slave
            foreach(List<UserInfo> chunk in chunks)
            {
                string chunkJson = JsonSerializer.Serialize(chunk);
                Message chunkMessage = new Message("Master", "crack_chunk", chunkJson);
                string jsonMessage = JsonSerializer.Serialize(chunkMessage);
                await writer.WriteLineAsync(jsonMessage);
                Console.WriteLine("Sent chunk to slave for cracking.");
            }

            //Loop to send messages from console
            while (true)
            {
                string input = Console.ReadLine();
                if (input?.ToLower() == "exit") break;

                Message message = new Message("Server", "chat", input);
                string jsonMessage = JsonSerializer.Serialize(message);
                await writer.WriteLineAsync(jsonMessage);

            }

            await receiveTask;
            Console.WriteLine("Connection Closed.");
        }

        //Method to generate sample hashed passwords (simple 4 digit for brute force)
        private List<UserInfo> GenerateSamplePasswords()
        {
            List<UserInfo> list = new List<UserInfo>
            {
                new UserInfo("User1", ComputeSHA1("0001")),
                new UserInfo("User2", ComputeSHA1("1234")),
                new UserInfo("User3", ComputeSHA1("5678")),
                new UserInfo("User4", ComputeSHA1("9999")),
            };
            return list;
        }


        //Method to compute SHA1 Hash
        private byte[] ComputeSHA1(string input)
        {
            using SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
        }

        private List<List<T>> ChunkList<T>(List<T> list, int chunkSize)
        {
            List<List<T>> chunks = new List<List<T>>();
            for (int i = 0; i < list.Count; i += chunkSize)
            {
                chunks.Add(list.GetRange(i, Math.Min(chunkSize, list.Count - i)));
            }
            return chunks;
        }

    }
}

