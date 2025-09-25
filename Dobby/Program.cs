using System.Threading.Tasks;
using Dobby;

namespace Dobby
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Dobby here mastah!");
            TCPClientServer clientServer = new TCPClientServer("localhost", 8080);
            await clientServer.StartAsync();
        }
    }
}