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


