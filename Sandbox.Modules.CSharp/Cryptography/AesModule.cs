using Sandbox.Core;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sandbox.Modules.CSharp.Cryptography {
    [SandboxModule("AES Cryptography", "cs-aes", "I had never attempted to write anything related to cryptography before this module. I was challenged by a co-worker to come up with a way to secure a web token that was efficient and not easy to crack. What better way to do that than to allow the use of plain text as a key, offer an offset for the data, and finally encrypt with the hashed key and specified offset. This way, in order to decrypt, you have to know the plain text key, the offset, the type of algorithm used, and the settings used for it. It was more complex than I initially thought, but in the end, my determination triumphed.")]
    [ModuleTags("cryptography", "aes", "encrypt", "decrypt")]
    public class AesModule : SandboxModule {

        #region Parameters

        [ModuleParameter("Encryption Key", "What would you like your encryption key to be?", DisplayElement.Textbox, Required = true)]
        public string EncryptionKey { get; set; }
        [ModuleParameter("Encryption Offset", "How many bytes should the encrypted data be shifted by?", DisplayElement.Slider, Required = true, MinValue = 0, MaxValue = 128)]
        public byte EncryptionOffset { get; set; }
        [ModuleParameter("Data To Encrypt", "What data should be encrypted?", DisplayElement.RichTextbox, Required = true)]
        public string DataToEncrypt { get; set; }

        #endregion

        #region Module Execution

        protected override void Execute() {
            byte[] encryptionKeyAsBytes = HashEncryptionKey(EncryptionKey);
            EncryptionKey = Convert.ToBase64String(encryptionKeyAsBytes);

            // Encrypt the data.
            SendResponse(SandboxEventType.Information, $"Encrypting: {DataToEncrypt}");
            string encryptedValue = Encrypt(EncryptionKey, DataToEncrypt, EncryptionOffset);
            SendResponse(SandboxEventType.Success, $"Result: {encryptedValue}");

            // Decrypt the data.
            SendResponse(SandboxEventType.Information, $"Decrypting the result.");
            string decryptedValue = Decrypt(EncryptionKey, encryptedValue, EncryptionOffset);
            SendResponse(SandboxEventType.Success, $"Result: {decryptedValue}");
        }

        #endregion

        #region Encryption

        public static string Encrypt(string key, string data, int offset) {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Data");
            byte[] encryptedData;
            byte[] keyData = Convert.FromBase64String(key);
            using (Aes algo = Aes.Create()) {
                algo.Key = keyData;
                algo.GenerateIV();
                algo.Padding = PaddingMode.PKCS7;
                Random r = new Random();
                using (MemoryStream ms = new MemoryStream()) {
                    for (int i = 0; i < offset; i++) {
                        ms.WriteByte((byte)r.Next(0, 200));
                    }
                    ms.Write(algo.IV, 0, 16);

                    ICryptoTransform encryptor = algo.CreateEncryptor(algo.Key, algo.IV);
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                        using (BinaryWriter bw = new BinaryWriter(cs)) {
                            bw.Write(data);
                            cs.FlushFinalBlock();
                        }

                        encryptedData = ms.ToArray();
                    }
                }
            }

            if (encryptedData != null)
                return Convert.ToBase64String(encryptedData);
            throw new Exception("An unxpected error occurred and the provided data was not encrypted.");
        }

        #endregion

        #region Decryption

        public static string Decrypt(string key, string data, int offset) {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Data");
            string decryptedData;
            byte[] keyData = Convert.FromBase64String(key);
            using (Aes algo = Aes.Create()) {
                algo.Key = keyData;
                algo.Padding = PaddingMode.PKCS7;
                byte[] decodedData = Convert.FromBase64String(data);
                using (MemoryStream ms = new MemoryStream(decodedData)) {
                    for (int i = 0; i < offset; i++) ms.ReadByte();
                    byte[] iv = new byte[16];
                    ms.Read(iv, 0, 16);

                    algo.IV = iv;

                    ICryptoTransform decryptor = algo.CreateDecryptor(algo.Key, algo.IV);

                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                        using (StreamReader sr = new StreamReader(cs)) {
                            sr.Read(); // Removes leading character..
                            decryptedData = sr.ReadToEnd();
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(decryptedData))
                return decryptedData;
            throw new Exception("An unxpected error occurred and the provided data was not decrypted.");
        }

        #endregion

        #region Hashing

        private byte[] HashEncryptionKey(string key) {
            using (MD5 md5 = MD5.Create())
                return md5.ComputeHash(Encoding.Unicode.GetBytes(key));
        }

        #endregion

    }
}