using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastah
{
    [Serializable]
    public class UserInfo
    {
        public string Username { get; set; }
        public string EncryptedPasswordBase64 { get; set; }
        public byte[] EncryptedPassword { get; set; }

        //for json deserialization
        public UserInfo()
        {
            
        }

        public UserInfo(string username, string encryptedPasswordBase64)
        {
            if(username == null)
            {
                throw new ArgumentNullException("Username cannot be null");
            }
            if(encryptedPasswordBase64 == null)
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
            EncryptedPasswordBase64 = Convert.ToBase64String(encryptedPassword);
            EncryptedPassword = encryptedPassword;
            
        }

        public override string ToString()
        {
            return Username + " : " + EncryptedPasswordBase64;
        }
    }
}
