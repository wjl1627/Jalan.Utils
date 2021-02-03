using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Jalan.Utils.Common
{
    public class DESCodeUtil
    {
        public static Encoding _encoding = Encoding.GetEncoding("UTF-8");
        private static string _defaultKey = "F3J$r6oP";

        /// <summary>
        /// DES加密算法
        /// </summary>
        /// <param name="encryptString">要加密的字符串</param>
        /// <param name="sKey">加密码Key</param>
        /// <returns>正确返回加密后的结果，错误返回源字符串</returns>
        public static string ToDES_Encrypt(string encryptString,string enKey=null)
        {
            try
            {
                if (!string.IsNullOrEmpty(enKey))
                    _defaultKey = enKey;
                byte[] keyBytes = _encoding.GetBytes(_defaultKey.Substring(0, 8));
                byte[] keyIV = _encoding.GetBytes(_defaultKey.Substring(0, 8));
                byte[] inputByteArray = _encoding.GetBytes(encryptString);

                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();

                // java 默认的是ECB模式，PKCS5padding；c#默认的CBC模式，PKCS7padding 所以这里我们默认使用ECB方式
                desProvider.Mode = CipherMode.ECB;
                MemoryStream memStream = new MemoryStream();
                CryptoStream crypStream = new CryptoStream(memStream, desProvider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);

                crypStream.Write(inputByteArray, 0, inputByteArray.Length);
                crypStream.FlushFinalBlock();
                return Convert.ToBase64String(memStream.ToArray());

            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密算法
        /// </summary>
        /// <param name="decryptString">要解密的字符串</param>
        /// <param name="sKey">加密Key</param>
        /// <returns>正确返回加密后的结果，错误返回源字符串</returns>
        public static string ToDES_Decrypt(string decryptString, string decKey = null)
        {
            if (!string.IsNullOrEmpty(decKey))
                _defaultKey = decKey;
            byte[] keyBytes = _encoding.GetBytes(_defaultKey.Substring(0, 8));
            byte[] keyIV = _encoding.GetBytes(_defaultKey.Substring(0, 8));
            byte[] inputByteArray = Convert.FromBase64String(decryptString);

            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();

            // java 默认的是ECB模式，PKCS5padding；c#默认的CBC模式，PKCS7padding 所以这里我们默认使用ECB方式
            desProvider.Mode = CipherMode.ECB;
            MemoryStream memStream = new MemoryStream();
            CryptoStream crypStream = new CryptoStream(memStream, desProvider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);

            crypStream.Write(inputByteArray, 0, inputByteArray.Length);
            crypStream.FlushFinalBlock();
            return _encoding.GetString(memStream.ToArray());

        }

        public static string ToMd5(string value) {
            if (string.IsNullOrEmpty(value))
                return null;
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes = md5Hasher.ComputeHash(_encoding.GetBytes(value));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            return tmp.ToString();
        }
    }
}
