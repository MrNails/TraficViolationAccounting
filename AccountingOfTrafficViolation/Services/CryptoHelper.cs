using System;
using System.Security.Cryptography;

namespace AccountingOfTrafficViolation.Services;

public static class CryptoHelper
{
    private static readonly SHA256 s_sha256Encryptor = SHA256.Create();

    public static byte[] EncryptData(byte[] data, byte[] salt)
    {
        if (data.Length == 0 || salt.Length == 0)
            return Array.Empty<byte>();
        
        var encryptedStr = new byte[data.Length + salt.Length];

        for (int i = 0; i < data.Length; i++)
            encryptedStr[i] = data[i];

        for (int i = 0; i < salt.Length; i++)
            encryptedStr[data.Length + i] = salt[i];

        return s_sha256Encryptor.ComputeHash(encryptedStr);
    } 
}