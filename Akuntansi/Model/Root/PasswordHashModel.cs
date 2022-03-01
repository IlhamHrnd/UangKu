using System;
using System.Security.Cryptography;
using System.Text;

namespace Akuntansi.Model.Root
{
    public class PasswordHashModel
    {
        //Method Hash Password
        public string EncodePassword(string PlainText)
        {
            SHA1CryptoServiceProvider sHA1 = new SHA1CryptoServiceProvider();

            byte[] PasswordBtyes = Encoding.ASCII.GetBytes(PlainText);
            byte[] EncryptBytes = sHA1.ComputeHash(PasswordBtyes);

            return Convert.ToBase64String(EncryptBytes);
        }
    }
}
