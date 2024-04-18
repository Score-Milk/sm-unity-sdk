/* 
    Encryption module
    Encrypts data to send it to the backend
*/

using System;
using System.Collections;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;

namespace ScoreMilk{
    public class Encryption : MonoBehaviour
    {
        [Tooltip("The encryption key used for requests")]
        [SerializeField] private string keyHex;
        private static byte[] key;
            
        void Start()
        {
            SetKey(keyHex);
        }

        private static byte[] HexStringToByteArray(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        private void SetKey(string hexKey)
        {
            if (hexKey.Length != 64)
            {
                Debug.LogError("Invalid key: the key must have 32 bytes.");
                return;
            }
            key = HexStringToByteArray(hexKey);
        }

        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.GenerateIV();
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
                        csEncrypt.Write(plaintextBytes, 0, plaintextBytes.Length);
                    }
                    byte[] iv = aesAlg.IV;
                    byte[] encryptedData = msEncrypt.ToArray();
                    string ivHex = BitConverter.ToString(iv).Replace("-", "");
                    string encryptedDataHex = BitConverter.ToString(encryptedData).Replace("-", "");
                    return ivHex + ":" + encryptedDataHex;
                }
            }
        }
    }
}
