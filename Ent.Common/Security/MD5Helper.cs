using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ent.Common.Security
{
    public class MD5Helper
    {
        /// <summary>
        /// 标准MD5
        /// </summary>
        /// <param name="AppKey"></param>
        /// <returns></returns>
        public static string Encrypt_MD5(string AppKey)
        {
            MD5 MD5 = new MD5CryptoServiceProvider();
            byte[] datSource = Encoding.GetEncoding("utf-8").GetBytes(AppKey);
            byte[] newSource = MD5.ComputeHash(datSource);
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < newSource.Length; i++)
            {
                sb.Append(newSource[i].ToString("x").PadLeft(2, '0'));
            }
            string crypt = sb.ToString();
            return crypt;
        }


        /// <summary>
        /// 根据输入的源字符串产生一个加密的哈希字符串。
        /// </summary>
        /// <param name="plainText">
        /// 源字符串
        /// </param>
        /// <param name="hashAlgorithm">
        /// 加密方式："MD5", "SHA1", "SHA256", "SHA384", and "SHA512".
        /// </param>
        /// <param name="saltBytes">
        /// 随机加密串
        /// </param>
        /// <returns>
        /// 加密后的哈希字符串
        /// </returns>
        public static string ComputeHash(string plainText,
                                         string hashAlgorithm,
                                         byte[] saltBytes)
        {
            if (saltBytes == null)
            {
                int minSaltSize = 4;
                int maxSaltSize = 8;

                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                saltBytes = new byte[saltSize];

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                rng.GetNonZeroBytes(saltBytes);
            }

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash;

            if (hashAlgorithm == null)
                hashAlgorithm = "";

            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hash = new SHA1Managed();
                    break;

                case "SHA256":
                    hash = new SHA256Managed();
                    break;

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            return hashValue;
        }
    }
}
