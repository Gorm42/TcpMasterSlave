using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastah
{
    public class UserInfoClearText
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserInfoClearText(string username, string password)
        {
            if (username == null)
            {
                throw new ArgumentNullException("Username cannot be null");
            }
            if(password == null)
            {
                throw new ArgumentNullException("Password cannot be null");
            }
            Username = username;
            Password = password;
        }

        public override string ToString()
        {
            return Username + ":" + Password;
        }
    }
}
