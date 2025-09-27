using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Dobby
{
    public class UserInfoClearText
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserInfoClearText(string username, string password)
        {
            if (username == null)
            {
                throw new ArgumentNullException("Username");
            }
            if (password == null)
            {
                throw new ArgumentNullException("Password");
            }
            Username = username;
            Password = password;
        }

        public override string ToString()
        {
            return Username + " : " + Password;
        }
        
    }
}
