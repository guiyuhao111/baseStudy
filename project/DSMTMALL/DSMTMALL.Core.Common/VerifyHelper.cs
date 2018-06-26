using System;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace DSMTMALL.Core.Common
{
    public class VerifyHelper
    {
        #region 校验实体参数

        /// <summary>
        /// 校验商城支付实体参数是否合法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">参数实体</param>
        /// <returns>bool</returns>
        public bool CheckPmtSign<T>(ref T entity) where T : new()
        {
            return CheckSign<T>(KeyHelper.pmtDesIv, KeyHelper.pmtDesKey, KeyHelper.pmtMd5Key, ref entity);
        }

        /// <summary>
        /// 校验读卡器回传实体参数是否合法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CheckCardSign<T>(ref T entity) where T : new()
        {
            return CheckSign<T>(KeyHelper.cardDesIv, KeyHelper.cardDesKey, KeyHelper.cardMd5Key, ref entity);
        }

        ///// <summary>
        ///// 校验实体参数是否合法
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="desIv">DES加密偏移量</param>
        ///// <param name="desKey">DES加密的私钥</param>
        ///// <param name="md5Key">DES加密的密钥</param>
        ///// <param name="entity">参数实体</param>
        ///// <returns>bool</returns>
        //private bool CheckSign<T>(string desIv, string desKey, string md5Key, ref T entity) where T : new()
        //{
        //    if (entity != null)
        //    {
        //        string sign = string.Empty;
        //        long timeStamp = 0;
        //        DESHelper desHelper = new DESHelper(desIv, desKey);
        //        PropertyInfo[] pArray = typeof(T).GetProperties();
        //        try
        //        {
        //            //试着DES解密
        //            for (int i = 0; i < pArray.Length; i++)
        //            {
        //                if (pArray[i].Name.ToLower() != "ExtensionData".ToLower())
        //                {
        //                    if (pArray[i].Name.ToLower() != "Sign".ToLower())
        //                    {
        //                        if (pArray[i].GetValue(entity, null) != null)
        //                        {
        //                            pArray[i].SetValue(entity, Convert.ChangeType(desHelper.Decrypt(pArray[i].GetValue(entity, null).ToString()), pArray[i].PropertyType), null);
        //                        }
        //                        if (pArray[i].Name.ToLower() == "TimeStamp".ToLower())
        //                        {
        //                            timeStamp = Convert.ToInt64(pArray[i].GetValue(entity, null));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        sign = pArray[i].GetValue(entity, null).ToString();
        //                        pArray[i].SetValue(entity, Convert.ChangeType(sign, pArray[i].PropertyType), null);
        //                    }
        //                }
        //            }
        //            return new MD5Helper(md5Key).CheckEncrypt(EntityHelper<T>.EntityToString(entity, '=', '&', Encoding.GetEncoding("utf-8"), new string[] { "ExtensionData", "Sign" }), sign) && Math.Abs(ToolHelper.DateTimeToUnixTimestamp(DateTime.Now) - timeStamp) < 180000;
        //        }
        //        catch { }
        //    }
        //    return false;
        //}

        /// <summary>
        /// 校验实体参数是否合法
        /// </summary>
        /// <typeparam name="T">实体类型<peparam>
        /// <param name="desIv">DES加密偏移量</param>
        /// <param name="desKey">DES加密的私钥</param>
        /// <param name="md5Key">DES加密的密钥</param>
        /// <param name="entity">参数实体</param>
        /// <returns>bool</returns>
        private bool CheckSign<T>(string desIv, string desKey, string md5Key, ref T entity) where T : new()
        {
            try
            {
                if (entity != null)
                {
                    object objSign = typeof(T).GetProperty("Sign") != null ? typeof(T).GetProperty("Sign").GetValue(entity, null) : null;
                    if (objSign != null && !string.IsNullOrEmpty(objSign.ToString()) && new MD5Helper(md5Key).CheckEncrypt(EntityHelper<T>.EntityToString(entity, '=', '&', Encoding.GetEncoding("utf-8"), new string[] { "ExtensionData", "Sign" }), objSign.ToString()))
                    {
                        DESHelper desHelper = new DESHelper(desIv, desKey);
                        object objTimeStamp = typeof(T).GetProperty("TimeStamp") != null ? typeof(T).GetProperty("TimeStamp").GetValue(entity, null) : null;
                        if (objTimeStamp != null && !string.IsNullOrEmpty(objTimeStamp.ToString()) && Math.Abs(ToolHelper.DateTimeToUnixTimestamp(DateTime.Now) - Convert.ToInt64(desHelper.Decrypt(objTimeStamp.ToString()))) < 300000)
                        {
                            PropertyInfo[] propertyInfoArr = typeof(T).GetProperties();
                            for (int i = 0; i < propertyInfoArr.Length; i++)
                            {
                                if (propertyInfoArr[i].Name.ToLower() != "ExtensionData".ToLower() && propertyInfoArr[i].Name.ToLower() != "Sign".ToLower() && propertyInfoArr[i].Name.ToLower() != "TimeStamp".ToLower() && propertyInfoArr[i].GetValue(entity, null) != null)
                                {
                                    propertyInfoArr[i].SetValue(entity, Convert.ChangeType(desHelper.Decrypt(propertyInfoArr[i].GetValue(entity, null).ToString()), propertyInfoArr[i].PropertyType), null);
                                }
                            }
                            return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        #endregion 校验实体参数

        #region 加密实体参数

        /// <summary>
        /// 加密商城支付实体参数
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">参数实体</param>
        /// <returns>bool</returns>
        public void EncryptPmtEntity<T>(T entity) where T : new()
        {
            EncryptEntity<T>(KeyHelper.pmtDesIv, KeyHelper.pmtDesKey, KeyHelper.pmtMd5Key, entity);
        }

        /// <summary>
        /// 加密读卡回传实体参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void EncryptCardEntity<T>(T entity) where T : new()
        {
            EncryptEntity<T>(KeyHelper.cardDesIv, KeyHelper.cardDesKey, KeyHelper.cardMd5Key, entity);
        }

        ///// <summary>
        ///// 加密实体参数
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="desIv">DES加密偏移量</param>
        ///// <param name="desKey">DES加密的私钥</param>
        ///// <param name="md5Key">MD5加密的密钥</param>
        ///// <param name="entity">参数实体</param>
        //private void EncryptEntity<T>(string desIv, string desKey, string md5Key, T entity) where T : new()
        //{
        //    if (entity != null)
        //    {
        //        PropertyInfo[] pArray = typeof(T).GetProperties();//通过PropertyInfo[] 与typeof(T) 可以获取到当前实体类的字段属性
        //        DESHelper desHelper = new DESHelper(desIv, desKey);//实例化加密类DES
        //        for (int i = 0; i < pArray.Length; i++)
        //        {
        //            if (pArray[i].Name.ToLower() == "TimeStamp".ToLower())//专用于接口的实体类的TimeStamp字段
        //            {   //给当前循环到的接口实体类赋值  //参数一：要操作的实体类  //参数二：新的属性值，即键值对中的值  //参数三：参数二对应的类型即键值的类型
        //                pArray[i].SetValue(entity, Convert.ChangeType(ToolHelper.DateTimeToUnixTimestamp(DateTime.Now), pArray[i].PropertyType), null);
        //            }
        //        }
        //        string sign = new MD5Helper(md5Key).Encrypt(EntityHelper<T>.EntityToString(entity, '=', '&', Encoding.GetEncoding("utf-8"), new string[] { "ExtensionData", "Sign" }));
        //        for (int i = 0; i < pArray.Length; i++)
        //        {
        //            if (pArray[i].GetValue(entity, null) != null)
        //            {
        //                pArray[i].SetValue(entity, Convert.ChangeType(desHelper.Encrypt(pArray[i].GetValue(entity, null).ToString()), pArray[i].PropertyType), null);
        //            }
        //            if (pArray[i].Name.ToLower() == "Sign".ToLower())
        //            {
        //                pArray[i].SetValue(entity, Convert.ChangeType(sign, pArray[i].PropertyType), null);
        //            }
        //        }
        //    }
        //}


        /// <summary>
        /// 加密实体参数
        /// </summary>
        /// <typeparam name="T">实体类型<peparam>
        /// <param name="desIv">DES加密偏移量</param>
        /// <param name="desKey">DES加密的私钥</param>
        /// <param name="md5Key">DES加密的密钥</param>
        /// <param name="entity">参数实体</param>
        private void EncryptEntity<T>(string desIv, string desKey, string md5Key, T entity) where T : new()
        {
            if (entity != null)
            {
                DESHelper desHelper = new DESHelper(desIv, desKey);
                PropertyInfo[] propertyInfoArr = typeof(T).GetProperties();
                for (int i = 0; i < propertyInfoArr.Length; i++)
                {
                    if (propertyInfoArr[i].Name.ToLower() != "ExtensionData".ToLower())
                    {
                        if (propertyInfoArr[i].Name.ToLower() != "TimeStamp".ToLower())
                        {
                            propertyInfoArr[i].SetValue(entity, Convert.ChangeType(desHelper.Encrypt(propertyInfoArr[i].GetValue(entity, null) != null ? propertyInfoArr[i].GetValue(entity, null).ToString() : null), propertyInfoArr[i].PropertyType), null);
                        }
                        else
                        {
                            propertyInfoArr[i].SetValue(entity, Convert.ChangeType(desHelper.Encrypt(ToolHelper.DateTimeToUnixTimestamp(DateTime.Now)), propertyInfoArr[i].PropertyType), null);
                        }
                    }
                }
                PropertyInfo signPropertyInfo = typeof(T).GetProperty("Sign");
                if (signPropertyInfo != null)
                {
                    signPropertyInfo.SetValue(entity, Convert.ChangeType(new MD5Helper(md5Key).Encrypt(EntityHelper<T>.EntityToString(entity, '=', '&', Encoding.GetEncoding("utf-8"), new string[] { "ExtensionData", "Sign" })), signPropertyInfo.PropertyType), null);
                }
            }
        }

        #endregion 加密实体参数


        #region 校验Api参数


        /// <summary>
        /// 校验中心参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="webApiClass"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CheckMallSign<T>(WebApiClass webApiClass, out T entity) where T : new()
        {
            return CheckApiSign<T>(KeyHelper.mallDesIv, KeyHelper.mallDesKey, KeyHelper.mallMd5Key, webApiClass, out entity);
        }

        /// <summary>
        /// 接收到webApi的参数后进行校验参数是否正确
        /// </summary>
        /// <param name="desIv"></param>
        /// <param name="desKey"></param>
        /// <param name="md5Key"></param>
        /// <param name="webApiClass"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool CheckApiSign<T>(string desIv, string desKey, string md5Key, WebApiClass webApiClass, out T entity) where T : new()
        {
            entity = default(T);
            if (webApiClass != null && !string.IsNullOrEmpty(webApiClass.JsonStr) && !string.IsNullOrEmpty(webApiClass.TimeStamp) && !string.IsNullOrEmpty(webApiClass.Sign))
            {
                if (new MD5Helper(md5Key).Encrypt(webApiClass.JsonStr + webApiClass.TimeStamp) == webApiClass.Sign)
                {
                    DESHelper desHelper = new DESHelper(desIv, desKey);
                    try
                    {
                        long timestampApi = Convert.ToInt64(desHelper.Decrypt(webApiClass.TimeStamp));
                        if (Math.Abs(DateHelper.DateTimeToUnixTimestamp(DateTime.Now) - timestampApi) < 180000)
                        {
                            string jsonStr = desHelper.Decrypt(webApiClass.JsonStr);
                            entity = new JavaScriptSerializer().Deserialize<T>(jsonStr);
                            return true;
                        }
                    }
                    catch { }
                }
            }
            return false;
        }

        #endregion

        #region 加密Api参数
        
        /// <summary>
        /// 加密中心参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public WebApiClass EncryptMallEntity<T>(T entity) where T : new()
        {
            return EncryptApiEntity(KeyHelper.mallDesIv, KeyHelper.mallDesKey, KeyHelper.mallMd5Key, entity);
        }
    
        /// <summary>
        /// 将实体类加密签名后生成WebApiClass
        /// </summary>
        /// <param name="desIv"></param>
        /// <param name="desKey"></param>
        /// <param name="md5Key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private WebApiClass EncryptApiEntity<T>(string desIv, string desKey, string md5Key, T entity) where T : new()
        {
            WebApiClass webApiClass = new WebApiClass();
            DESHelper desHelper = new DESHelper(desIv, desKey);
            long timestamp = DateHelper.DateTimeToUnixTimestamp(DateTime.Now);
            webApiClass.TimeStamp = desHelper.Encrypt(Convert.ToString(timestamp));
            webApiClass.JsonStr = string.Empty;
            if (entity != null)
            {
                string jsonStr = new JavaScriptSerializer().Serialize(entity);//实体类转化成字符串
                webApiClass.JsonStr = desHelper.Encrypt(jsonStr);
            }
            webApiClass.Sign = new MD5Helper(md5Key).Encrypt(webApiClass.JsonStr + webApiClass.TimeStamp);
            return webApiClass;
        }

        #endregion

    }
}

/// <summary>
/// 接口的基本传递实体类
/// </summary>
public class WebApiClass
{
    public string JsonStr { get; set; }
    public string TimeStamp { get; set; }
    public string Sign { get; set; }

}

/// <summary>
/// 接口返回成功失败状态码实体类
/// </summary>
public class APIResult
{
    public APIResult()
    {
        ResultStatus = "0";//0失败，1成功
        ResultInfo = string.Empty;
    }
    public string ResultStatus { get; set; }
    public string ResultInfo { get; set; }
}