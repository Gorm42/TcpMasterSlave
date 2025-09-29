using System.Threading.Tasks;


namespace Mastah
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Dominus sum est!");
            TcpChatServer server = new TcpChatServer(8080);
            await server.RunAsync();
        }
    }
}


//https://grok. com/c/19e21c97-7d8e-470d-a482-c9355fa027b5