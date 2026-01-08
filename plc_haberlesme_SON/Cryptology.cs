using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plc_haberlesme_SON
{
    internal class Cryptology
    {
        public static string Encryption(string text, int key)
        {
            char[] x = text.ToCharArray();
            StringBuilder encryptedText = new StringBuilder();

            foreach (char item in x)
            {
                encryptedText.Append(Convert.ToChar(item + key));
            }
            // Base64 ile encode et
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedText.ToString()));
        }

        public static string Decryption(string text, int key)
        {
            try
            {
                // Yeni algoritma: Base64 decode
                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(text));
                char[] x = decoded.ToCharArray();
                StringBuilder decryptedText = new StringBuilder();

                foreach (char item in x)
                {
                    decryptedText.Append(Convert.ToChar(item - key));
                }
                return decryptedText.ToString();
            }
            catch (FormatException)
            {
                // Eski algoritma: doğrudan karakter kaydırmalı çözüm
                char[] x = text.ToCharArray();
                StringBuilder decryptedText = new StringBuilder();

                foreach (char item in x)
                {
                    decryptedText.Append(Convert.ToChar(item - key));
                }
                return decryptedText.ToString();
            }
        }
    }
}
