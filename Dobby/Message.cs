using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dobby
{
    public class Message
    {
        public string Sender { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }

        public Message()
        {
            
        }

        public Message(string sender, string type, string data)
        {
            Sender = sender;
            Type = type;
            Data = data;
        }
    }
}
