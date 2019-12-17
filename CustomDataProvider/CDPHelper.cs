using System;
using System.Security.Cryptography;
using System.Text;

namespace CustomDataProvider
{
    static class CDPHelper {

        /// <summary>
        /// Initializes a TripleDESCryptoServiceProvider
        /// </summary>
        /// <returns>an instance of TripleDESCryptoServiceProvider</returns>
        public static TripleDES GetCryptoServiceProvider()
        {
            var md5 = new MD5CryptoServiceProvider();
            var des = new TripleDESCryptoServiceProvider();
            var desKey = md5.ComputeHash(Encoding.UTF8.GetBytes("fjdoe85d"));
            des.Key = desKey;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;
            return des;
        }

        /// <summary>
        /// Convert string to Guid. Please note! Works for strings less than 16 bytes only.
        /// </summary>
        /// <param name="value">the string value</param>
        /// <returns>the Guid value</returns>
        public static Guid StringToGuid(string value)
        {
            var des = GetCryptoServiceProvider();
            var ct = des.CreateEncryptor();
            var input = Encoding.UTF8.GetBytes(value);
            var output = ct.TransformFinalBlock(input, 0, input.Length);
            return new Guid(output);
        }

        /// <summary>
        /// Convert Guid to string
        /// </summary>
        /// <param name="value">the Guid value</param>
        /// <returns>the string value</returns>
        public static string GuidToString(Guid value)
        {
            var des = GetCryptoServiceProvider();
            var guidInBytes = value.ToByteArray();
            var str = Convert.ToBase64String(guidInBytes);
            var ctd = des.CreateDecryptor();
            var input = Convert.FromBase64String(str);
            var output = ctd.TransformFinalBlock(input, 0, input.Length);
            return Encoding.UTF8.GetString(output);
        }
                      
        /// <summary>
        /// Convert int to Guid
        /// </summary>
        /// <param name="value">the int value</param>
        /// <returns>the Guid value</returns>
        public static Guid IntToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        /// <summary>
        /// Convert Guid to int
        /// </summary>
        /// <param name="value">the Guid value</param>
        /// <returns>the int value</returns>
        public static int GuidToInt(Guid value)
        {
            byte[] b = value.ToByteArray();
            int bint = BitConverter.ToInt32(b, 0);
            return bint;
        }
    }
}