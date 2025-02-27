﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IEncryptService
    {
        string Decrypt(string senha);
        string Encrypt(string password);
    }

    public class EncryptService(EncryptKeys encryptKeys) : IEncryptService
    {
        public string Encrypt(string password)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(password);
            byte[] keyBytes = new Rfc2898DeriveBytes(encryptKeys.PASSWORDHASH, Encoding.ASCII.GetBytes(encryptKeys.SALTKEY)).GetBytes(256 / 8);
            byte[] cipherTextBytes;

            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.Zeros;

                ICryptoTransform encryptor = aes.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(encryptKeys.VIKEY));

                using MemoryStream memoryStream = new();
                using (CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public string Decrypt(string senha)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(senha);
            byte[] keyBytes = new Rfc2898DeriveBytes(encryptKeys.PASSWORDHASH, Encoding.ASCII.GetBytes(encryptKeys.SALTKEY)).GetBytes(256 / 8);

            using Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.Zeros;

            ICryptoTransform decryptor = aes.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(encryptKeys.VIKEY));

            using MemoryStream memoryStream = new(cipherTextBytes);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();

            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }
}
