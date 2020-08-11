using Sandbox.Core;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sandbox.Modules.jdavis1.Cryptography {
    [ModuleDescription("AES Cryptography Test", "aes", "Tests encrypting and decrypting data using the Aes algorithm.")]
    public class AesModule : SandboxModule {

        #region Fields

        private byte encryptionOffset = 0;
        private string encryptionKey = string.Empty;
        private string dataToEncrypt = string.Empty;

        #endregion

        #region Module Execution

        public override void Execute() {
            OnExecutionStarted();
            try {
                BeginSteppedExecution(this);
            } catch (Exception e) {
                OnExecutionFailed(new ModuleCommunicationData(ModuleCommunicationType.Failure, e));
            } finally {
                OnExecutionCompleted();
            }
        }
        protected override void ProcessResponse(ModuleCommunicationData data) => Step(this, data);

        #endregion

        #region Execution Steps

        [ModuleStep(0)]
        public void RequestEncryptionKey(ModuleCommunicationData data) => RequestInput(ModuleCommunicationType.None, "Please enter a key to encrypt data with.");
        [ModuleStep(1)]
        public void StoreEncryptionKey(ModuleCommunicationData data) {
            byte[] encryptionKeyAsBytes = HashEncryptionKey(data.Data.ToString());
            encryptionKey = Convert.ToBase64String(encryptionKeyAsBytes);
            Step(this, data);
        }
        [ModuleStep(2)]
        public void RequestOffset(ModuleCommunicationData data) => RequestInput(ModuleCommunicationType.None, "How many bytes should the encrypted data be shifted by?");
        [ModuleStep(3)]
        public void TryStoreOffset(ModuleCommunicationData data) {
            if (byte.TryParse(data.Data.ToString(), out byte offset)) {
                encryptionOffset = offset;
                Step(this, data);
            } else
                ReprocessPreviousStep(this, data);
        }
        [ModuleStep(4)]
        public void RequestDataToEncrypt(ModuleCommunicationData data) => RequestInput(ModuleCommunicationType.None, "What data should be encrypted?");
        [ModuleStep(5)]
        public void EncryptAndDecryptData(ModuleCommunicationData data) {
            dataToEncrypt = data.Data.ToString();

            // Encrypt the data.
            SendResponse(ModuleCommunicationType.Information, $"Encrypting: {dataToEncrypt}");
            string encryptedValue = Encrypt(encryptionKey, dataToEncrypt, encryptionOffset);
            SendResponse(ModuleCommunicationType.Success, $"Result: {encryptedValue}");

            // Decrypt the data.
            SendResponse(ModuleCommunicationType.Information, $"Decrypting the result.");
            string decryptedValue = Decrypt(encryptionKey, encryptedValue, encryptionOffset);
            SendResponse(ModuleCommunicationType.Success, $"Result: {decryptedValue}");
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