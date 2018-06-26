using System.Security.Cryptography;

namespace DSMTMALL.Core.Common
{
    public class MD5Helper
    {
        private string key = "r3eydhfkja";
        /// <summary>
        /// 构造函数
        /// </summary>
        public MD5Helper()
        {
            key = "b81f5675c2";
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">MD5加密的私钥</param>
        public MD5Helper(string key)
        {
            this.key = key;
        }
        /// <summary>
        /// 用MD5Key加密字符串
        /// </summary>
        /// <param name="strOriginal">原始字符串</param>
        /// <returns>加密后字符串</returns>
        public string Encrypt(string strOriginal)
        {
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] result = System.Text.Encoding.UTF8.GetBytes(strOriginal + key);
                byte[] bytHash = md5.ComputeHash(result);
                string sTemp = string.Empty;
                for (int i = 0; i < bytHash.Length; i++)
                {
                    sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
                }
                return sTemp.ToLower();
            }
        }
        /// <summary>
        /// 校验MD5加密后的字符串
        /// </summary>
        /// <param name="strOriginal">原始字符串</param>
        /// <param name="strEncrypt">加密字符串</param>
        /// <returns>true-通过false-不通过</returns>
        public bool CheckEncrypt(string strOriginal, string strEncrypt)
        {
            return strEncrypt.ToLower() == Encrypt(strOriginal);
        }
    }
}