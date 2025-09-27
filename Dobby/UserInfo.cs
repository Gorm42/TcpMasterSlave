using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dobby
{
    [Serializable]
    public class UserInfo
    {
        public string Username { get; set; }
        public string EncryptedPasswordBase64 { get; set; }
        public byte[] EncryptedPassword { get; set; }

        public UserInfo(string username, string encryptedPasswordBase64)
        {
            if (username == null)
            {
                throw new ArgumentNullException("Username is empty");
            }
            if (encryptedPasswordBase64 == null)
            {
                throw new ArgumentNullException("EncryptedPasswordBase64");
            }
            Username = username;
            EncryptedPasswordBase64 = encryptedPasswordBase64;
            EncryptedPassword = Convert.FromBase64String(encryptedPasswordBase64);
        }

        public UserInfo(string username, byte[] encryptedPassword)
        {
            Username = username;
            EncryptedPassword = encryptedPassword;
            EncryptedPasswordBase64 = Convert.ToBase64String(encryptedPassword);
        }

        public override string ToString()
        {
            return Username + " : " + EncryptedPasswordBase64;
        }
    }
}
