using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Web;

namespace PMIS.Common
{
    //This is a common class for encrypt and decrypt information
    //Currently we use this to encrypt the passwords in the DB
    //It is used the AES method
    public static class Cryptography
    {
        private static string mykey = GetEncryptionKey();

        public static string Encrypt(string str)
        {
            if (str == "")
                return "";

            byte[] Key;
            byte[] IV;
            byte[] clearData;

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(mykey, new byte[] { });

            Key = pdb.GetBytes(32);
            IV = pdb.GetBytes(16);

            clearData = System.Text.Encoding.Unicode.GetBytes(str);

            MemoryStream ms = new MemoryStream();

            Rijndael alg = Rijndael.Create();

            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new CryptoStream(ms,
               alg.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(clearData, 0, clearData.Length);

            cs.Close();

            byte[] encryptedData = ms.ToArray();

            return Convert.ToBase64String(encryptedData);
        }

        public static string Decrypt(string str)
        {
            if (str == "")
                return "";

            byte[] Key;
            byte[] IV;

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(mykey, new byte[] { });

            Key = pdb.GetBytes(32);
            IV = pdb.GetBytes(16);

            byte[] cipherBytes = Convert.FromBase64String(str);

            MemoryStream ms = new MemoryStream();

            Rijndael alg = Rijndael.Create();

            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new CryptoStream(ms,
               alg.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(cipherBytes, 0, cipherBytes.Length);

            cs.Close();

            byte[] decryptedData = ms.ToArray();

            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        //The encryption key is stored in an separate file in the filesystem on the server
        private static string GetEncryptionKey()
        {
            string key = "";
            string path = Config.GetWebSetting("CommonFilesPath");

            TextReader tr = new StreamReader(path + "key.txt");
            key = tr.ReadLine();
            tr.Close();

            return key;
        }
    }
}
