using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace DSMTMALL.Core.Common
{
    public class DESHelper
    {
        private string iv = "ffgZkfgpo9h";
        private string key = "6ghh8hjhj";
        /// <summary>
        /// 构造函数
        /// </summary>
        public DESHelper()
        {
            iv = "fZVZ4npn6bc=";
            key = "6RH0LVk8";
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iv">DES加密偏移量，必须是>=8位长的字符串</param>
        /// <param name="key">DES加密的私钥，必须是8位长的字符串</param>
        public DESHelper(string iv, string key)
        {
            this.iv = iv;
            this.key = key;
        }
        /// <summary>
        /// 对字符串进行DES加密
        /// </summary>
        /// <param name="sourceString">待加密的字符串</param>
        /// <returns>加密后的BASE64编码的字符串</returns>
        public string Encrypt(string sourceString)
        {
            if (sourceString == null)
            {
                return EncryptEx(String.Empty);
            }
            else
            {
                return EncryptEx(sourceString);
            }
        }
        /// <summary>
        /// 对整型进行DES加密
        /// </summary>
        /// <param name="sourceInt">待加密的整型</param>
        /// <returns>加密后的BASE64编码的字符串</returns>
        public string Encrypt(int sourceInt)
        {
            return EncryptEx(sourceInt.ToString());
        }
        /// <summary>
        /// 对长整型进行DES加密
        /// </summary>
        /// <param name="sourceLong">待加密的长整型</param>
        /// <returns>加密后的BASE64编码的字符串</returns>
        public string Encrypt(long sourceLong)
        {
            return EncryptEx(sourceLong.ToString());
        }
        /// <summary>
        /// 对小数进行DES加密
        /// </summary>
        /// <param name="sourceDecimal">待加密的小数</param>
        /// <returns>加密后的BASE64编码的字符串</returns>
        public string Encrypt(decimal sourceDecimal)
        {
            return EncryptEx(sourceDecimal.ToString());
        }
        /// <summary>
        /// 对日期进行DES加密
        /// </summary>
        /// <param name="sourceDateTime">待加密的日期</param>
        /// <returns>加密后的BASE64编码的字符串</returns>
        public string Encrypt(DateTime sourceDateTime)
        {
            return EncryptEx(sourceDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        /// <summary>
        /// 对日期进行DES加密
        /// </summary>
        /// <param name="sourceDateTime">待加密的日期</param>
        /// <returns>加密后的BASE64编码的字符串</returns>
        public string Encrypt(DateTime? sourceDateTime)
        {
            if (sourceDateTime == null)
            {
                return EncryptEx(String.Empty);
            }
            else
            {
                return EncryptEx(Convert.ToDateTime(sourceDateTime).ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }
        private string EncryptEx(string sourceString)
        {
            byte[] btKey = Encoding.GetEncoding("UTF-8").GetBytes(key);
            byte[] btIV = Encoding.GetEncoding("UTF-8").GetBytes(iv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Encoding.GetEncoding("UTF-8").GetBytes(sourceString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
                catch
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 对DES加密后的字符串进行解密
        /// </summary>
        /// <param name="encryptedString">待解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public string Decrypt(string encryptedString)
        {
            if (encryptedString == String.Empty)
            {
                return String.Empty;
            }
            if (encryptedString == null)
            {
                return String.Empty;
            }
            byte[] btKey = Encoding.GetEncoding("UTF-8").GetBytes(key);
            byte[] btIV = Encoding.GetEncoding("UTF-8").GetBytes(iv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Convert.FromBase64String(encryptedString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }
                    return Encoding.GetEncoding("UTF-8").GetString(ms.ToArray());
                }
                catch
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 对文件内容进行DES加密
        /// </summary>
        /// <param name="sourceFile">待加密的文件绝对路径</param>
        /// <param name="destFile">加密后的文件保存的绝对路径</param>
        public void EncryptFile(string sourceFile, string destFile)
        {
            if (!File.Exists(sourceFile))
            {
                throw new FileNotFoundException("指定的文件路径不存在！", sourceFile);
            }
            byte[] btKey = Encoding.GetEncoding("UTF-8").GetBytes(key);
            byte[] btIV = Encoding.GetEncoding("UTF-8").GetBytes(iv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] btFile = File.ReadAllBytes(sourceFile);
            using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
        }
        /// <summary>
        /// 对文件内容进行DES加密，加密后覆盖掉原来的文件
        /// </summary>
        /// <param name="sourceFile">待加密的文件的绝对路径</param>
        public void EncryptFile(string sourceFile)
        {
            EncryptFile(sourceFile, sourceFile);
        }
        /// <summary>
        /// 对文件内容进行DES解密
        /// </summary>
        /// <param name="sourceFile">待解密的文件绝对路径</param>
        /// <param name="destFile">解密后的文件保存的绝对路径</param>
        public void DecryptFile(string sourceFile, string destFile)
        {
            if (!File.Exists(sourceFile))
            {
                throw new FileNotFoundException("指定的文件路径不存在！", sourceFile);
            }
            byte[] btKey = Encoding.GetEncoding("UTF-8").GetBytes(key);
            byte[] btIV = Encoding.GetEncoding("UTF-8").GetBytes(iv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] btFile = File.ReadAllBytes(sourceFile);
            using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(fs, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
        }
        /// <summary>
        /// 对文件内容进行DES解密，加密后覆盖掉原来的文件
        /// </summary>
        /// <param name="sourceFile">待解密的文件的绝对路径</param>
        public void DecryptFile(string sourceFile)
        {
            DecryptFile(sourceFile, sourceFile);
        }
        /// <summary>
        /// 对实体所有属性进行DES加密
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="myEntity">待加密实体</param>
        public void EncryptEntity<T>(T myEntity) where T : class, new()
        {
            PropertyInfo[] pArray = typeof(T).GetProperties();
            for (int i = 0; i < pArray.Length; i++)
            {
                if (pArray[i].GetValue(myEntity, null) != null)
                {
                    pArray[i].SetValue(myEntity, Convert.ChangeType(Encrypt(pArray[i].GetValue(myEntity, null).ToString()), pArray[i].PropertyType), null);
                }
            }
        }
        /// <summary>
        /// 对实体所有属性进行DES解密
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="myEntity">待解密实体</param>
        public void DecryptEntity<T>(T myEntity) where T : class, new()
        {
            PropertyInfo[] pArray = typeof(T).GetProperties();
            for (int i = 0; i < pArray.Length; i++)
            {
                if (pArray[i].GetValue(myEntity, null) != null)
                {
                    pArray[i].SetValue(myEntity, Convert.ChangeType(Decrypt(pArray[i].GetValue(myEntity, null).ToString()), pArray[i].PropertyType), null);
                }
            }
        }
    }
}