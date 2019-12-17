namespace GetTechportData
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class Crypto
    {
        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 128;

        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  We use keysize 128, so the IV must be
        // 16 bytes long. We need a 16 bytes array in order to be able to create a GUID

        // Using a 8 character string here gives us 16 bytes when converted to a byte array.
     //  private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89gejifjsp");

        private static byte[] initVectorBytes = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
      //  RMCrypto.Key = key;

        public static byte[] Encrypt(string plainText, string passPhrase)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new PasswordDeriveBytes(passPhrase, null))
            {
                var keyBytes = password.GetBytes(keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var cipherTextBytes = memoryStream.ToArray();
                                return cipherTextBytes;
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(byte[] cipherTextBytes, string passPhrase)
        {
            using (var password = new PasswordDeriveBytes(passPhrase, null))
            {
                var keyBytes = password.GetBytes(keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }
    }
}