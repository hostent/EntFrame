using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Ent.Common.Security
{
    /// <summary>
    /// ASE加解密
    /// </summary>
    public class AESHelper
    {
        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string Key
        {
            get
            {
                return "aijia.com.568987";    ////必须是16位
            }
        }
        //默认密钥向量 
        private static byte[] _key1 = Encoding.UTF8.GetBytes("www.aijia.com.cn");
        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <returns>将加密后的密文转换为Base64编码，以便显示</returns>
        public static string AESEncrypt(string plainText)
        {
            try
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Key);
                byte[] ivArray = _key1;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(plainText);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.IV = ivArray;
                rDel.Mode = CipherMode.CBC;
                rDel.Padding = PaddingMode.Zeros;

                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文字符串</param>
        /// <returns>返回解密后的明文字符串</returns>
        public static string AESDecrypt(string showText)
        {
            try
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Key);
                byte[] ivArray = _key1;
                byte[] toEncryptArray = Convert.FromBase64String(showText);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.IV = ivArray;
                rDel.Mode = CipherMode.CBC;
                rDel.Padding = PaddingMode.Zeros;

                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray).Replace("\0", "");

            }
            catch
            {
                return "";
            }
        }
    }
}
