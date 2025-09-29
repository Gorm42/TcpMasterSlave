using System.Threading.Tasks;
using Dobby;

namespace Dobby
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Dobby here mastah!");
            TCPClientServer clientServer = new TCPClientServer("10.107.1.240", 8080);
            await clientServer.StartAsync();
        }
    }
}