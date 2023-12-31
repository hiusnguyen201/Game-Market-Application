﻿using System.Security.Cryptography;
using System.Text;

namespace GMA.Utility;

public class EncryptionAES
{
    public const String STRING_PERMUTATION = "sinhnx.dev";
    public const Int32 BYTE_PERMUTATION_1 = 0x19;
    public const Int32 BYTE_PERMUTATION_2 = 0x59;
    public const Int32 BYTE_PERMUTATION_3 = 0x17;
    public const Int32 BYTE_PERMUTATION_4 = 0x41;

    // Encoding
    public static string Encrypt(string strData)
    {
        return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(strData)));
    }

    // Encrypt
    public static byte[] Encrypt(byte[] strData)
    {
        PasswordDeriveBytes passbytes = new PasswordDeriveBytes(STRING_PERMUTATION,
        new byte[] { BYTE_PERMUTATION_1, BYTE_PERMUTATION_2, BYTE_PERMUTATION_3, BYTE_PERMUTATION_4 });
        MemoryStream memstream = new MemoryStream();
        Aes aes = new AesManaged();
        aes.Key = passbytes.GetBytes(aes.KeySize / 8);
        aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

        CryptoStream cryptoStream = new CryptoStream(memstream,
        aes.CreateEncryptor(), CryptoStreamMode.Write);
        cryptoStream.Write(strData, 0, strData.Length);
        cryptoStream.Close();
        return memstream.ToArray();
    }
}

