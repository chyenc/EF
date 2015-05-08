using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace MyChy.Frame.Common.Helper
{
    public static class SafeSecurity
    {

        //默认密钥向量
        private static readonly byte[] RgbIv = new byte[] { 0x14, 0x34, 0x56, 120, 0x10, 0xcb, 0xcd, 0xaf };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="pToEncrypt">待加密的字符串</param>
        /// <param name="sKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string pToEncrypt, string sKey)
        {
            try
            {
                var des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                //建立加密对象的密钥和偏移量
                //原文使用ASCIIEncoding.ASCII方法的GetBytes方法
                //使得输入密码必须输入英文文本
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = RgbIv;
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0,
                    inputByteArray.Length);
                cs.FlushFinalBlock();
                var ret = new StringBuilder();
                foreach (var b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch { return ""; }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="pToDecrypt">待解密的字符串</param>
        /// <param name="sKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string pToDecrypt, string sKey)
        {
            try
            {
                var des = new
                    DESCryptoServiceProvider();
                var inputByteArray = new byte
                    [pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32
                        (pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                //建立加密对象的密钥和偏移量，此值重要，不能修改
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = RgbIv;
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch { return ""; }
        }

        /// <summary>
        /// MD5加密字符串 32位
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string strText)
        {
            var m5 = new MD5CryptoServiceProvider();
            //创建md5对象
            var inputBye = Encoding.ASCII.GetBytes(strText);
            //使用ascii编码方式把字符串转化为字节数组．
            var outputBye = m5.ComputeHash(inputBye);
            var retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToLower();
            return (retStr);

        }

        public static string SHA1(string strText)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(strText, "SHA1");
        }
    }
}
