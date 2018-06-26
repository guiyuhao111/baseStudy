
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.DB.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DSMTMALL.MyPublic
{
    public class WebToolHelper
    {
        private readonly string AssemblyPath = ConfigurationManager.AppSettings["MySQLBLL"];//从配置文件读

        /// <summary>
        /// 获取配置文件的相关信息
        /// </summary>
        /// <param name = "fieldName" > 配置文件字段名 </ param >
        /// < param name="defaultVal">如果获取失败的默认返回值</param>
        /// <returns></returns>
        public string GetProfilesInfo(FieldName fieldName, string defaultVal)
        {
            string getVal = INIHelper.INIGetStringValue(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\Myconfig.ini", "SystemConfig", fieldName.ToString(), defaultVal);
            if (getVal == defaultVal)
            {
                return defaultVal;
            }
            else
            {
                return getVal; //new DESHelper().Decrypt(getVal);
            }
        }

        /// <summary>
        /// 获取配置文件的相关信息
        /// </summary>
        /// <param name = "fieldName" > 配置文件字段名 </ param >
        /// < param name="defaultVal">如果获取失败的默认返回值</param>
        /// <returns></returns>
        public string GetProfilesEncryptInfo(FieldName fieldName, string defaultVal)
        {
            string getVal = INIHelper.INIGetStringValue(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\Myconfig.ini", "SystemConfig", fieldName.ToString(), defaultVal);
            if (getVal == defaultVal)
            {
                return defaultVal;
            }
            else
            {
                return new DESHelper().Decrypt(getVal);
            }
        }

        /// <summary>
        /// 获取配置文件的图片url路径
        /// </summary>
        /// <returns></returns>
        public static string GetProfilesUrl()
        {
            return new WebToolHelper().GetProfilesInfo(FieldName.PictureUrl, "mallmanage.51ipc.com/");
        }

        /// <summary>
        /// 修改配置文件信息
        /// </summary>
        /// <param name = "fieldName" > 配置文件字段名 </ param >
        /// < param name="fieldValue">配置文件字段值</param>
        /// <returns></returns>
        public bool UpdateProfilesInfo(FieldName fieldName, string fieldValue)
        {
            return INIHelper.INIWriteValue(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\Myconfig.ini", "SystemConfig", fieldName.ToString(), fieldValue);
        }

        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            long intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt64((dateTime - startTime).TotalMilliseconds);
            return intResult;
        }

        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns></returns>
        public DateTime UnixTimestampToDateTime(DateTime target, long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0,0, target.Kind);
            return start.AddMilliseconds(timestamp);
        }

        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestampFromJava()
        {
            long intResult = 0;
            TimeSpan ts = new TimeSpan(System.DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
             intResult=   Convert.ToInt64( ts.TotalMilliseconds);
            return intResult;
        }

        /// <summary>
        /// 隐藏手机号码中间几位
        /// </summary>
        /// <param name="telephone">输入手机号码</param>
        /// <returns></returns>
        public static string HiddenTelephoneNo(string telephone)
        {
            string hiddenTelNo = Regex.Replace(telephone, @"(?im)(\d{3})(\d{4})(\d{4})", "$1***$3");
            return hiddenTelNo;
        }

        /// <summary>
        /// 隐藏用户的第一位姓
        /// </summary>
        /// <param name="telephoneName"></param>
        /// <returns></returns>
        public static string HiddenTelephoneUserName(string telephoneName)
        { 
            Regex reg =  new Regex(@".");
            return reg.Replace(telephoneName, "*", 1);
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="telPhone">手机号码</param>
        /// <param name="note">提示消息</param>
        public static string GetTelphoneCode(string telPhone, string note)
        {
            string resStrCode = string.Empty;
            if (MetarnetRegex.IsMobilePhone(telPhone))
            {
                DB.Model.M_TelPhoneCode newTelCode = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<DB.Model.M_TelPhoneCode>("Telphone=@_Telphone",new { _Telphone = telPhone });
                int randomCode = new Random().Next(100000, 999999);//随机生成6位数的随机数
                if (newTelCode == null)
                {
                    newTelCode =new DB.Model.M_TelPhoneCode();
                    newTelCode.Telphone = telPhone;
                    newTelCode.Code = randomCode.ToString();
                    newTelCode.CreatTime = DateTime.Now;
                    newTelCode.LoginTimes = 0;
                    if (new DB.BLL.MY_Bll(DBEnum.Master).AddModel(newTelCode))
                    {
                        string smsurl = "http://utf8.sms.webchinese.cn/?";
                        smsurl += "Uid=llkj&Key=llkjbyflood&";
                        smsurl += "smsMob=" + telPhone + "&smsText=" + note + "，您的验证码:" + randomCode;
                        string resStr = string.Empty;
                        if (SendSMS.GetHtmlFromUrl(smsurl, out resStr))
                        {
                            resStrCode = "SUCCESS";
                        }else
                        {
                            resStrCode = "ERR,发送失败，请重试";
                            ToolHelper.WriteLogInfoToLocalText(resStr);
                        }
                    }
                    else
                    {
                        resStrCode = "ERR,发送失败，请重试";
                    }
                }
                else//数据库已经存在这条数据
                {
                    if (DateTime.Now < newTelCode.CreatTime.AddMinutes(1))//间隔时间小于1分钟
                    {
                        resStrCode = "ERR,获取验证码的时间间隔为1分钟";
                    }
                    else
                    {
                        newTelCode.Code = randomCode.ToString();
                        newTelCode.CreatTime = DateTime.Now;
                        newTelCode.LoginTimes = 0;//更新验证码的时候归零次数
                        if (new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<DB.Model.M_TelPhoneCode>(" Telphone=@Telphone ",newTelCode))
                        {
                            string smsurl = "http://utf8.sms.webchinese.cn/?";
                            smsurl += "Uid=llkj&Key=llkjbyflood&";
                            smsurl += "smsMob=" + telPhone + "&smsText=" + note + "，您的验证码:" + randomCode;
                            string resStr = string.Empty;
                            if (SendSMS.GetHtmlFromUrl(smsurl, out resStr))
                            {
                                resStrCode = "SUCCESS";
                            }
                            else
                            {
                                resStrCode = "ERR,发送失败，请重试";
                            }
                        }
                        else
                        {
                            resStrCode = "ERR,发送失败，请重试";
                        }
                    }
                }
            }
            else
            {
                resStrCode = "提示：不是合法的手机号码";
            }
            return resStrCode;
        }

        /// <summary>
        /// 对电话号码与验证码进行校验
        /// </summary>
        /// <param name="phone">电话</param>
        /// <param name="code">验证码</param>
        /// <param name="resWrite">返回信息</param>
        /// <returns></returns>
        public static bool TelphoneModalCount(string phone, string code, out string resWrite)
        {
            resWrite = string.Empty;
            if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(code))
            {
                M_TelPhoneCode telPhoneInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_TelPhoneCode>(" Telphone=@_Telphone ", new { _Telphone = phone });
                if (telPhoneInfo != null)
                {
                    if (telPhoneInfo.Code == code)
                    {
                        if (telPhoneInfo.LoginTimes < 10)
                        {
                            long ss = ToolHelper.TimeQuantum(DateTime.Now, telPhoneInfo.CreatTime);
                            if (ToolHelper.TimeQuantum(DateTime.Now, telPhoneInfo.CreatTime) < 180000)
                            {
                                return true;
                            }else { resWrite = "验证码已失效，请重新获取"; }

                        } else
                        {
                            resWrite = "验证码错误次数已达上限，请重新获取";
                        }
                    }
                    else
                    {
                        resWrite = "验证码错误";
                        telPhoneInfo.LoginTimes++;
                        new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_TelPhoneCode>(" Telphone=@_Telphone ", new { _Telphone = phone, LoginTimes = telPhoneInfo.LoginTimes });
                    }
                }
                else
                {
                    resWrite = "验证码失效，请重新获取验证码";
                }
            }else
            {
                resWrite = "短信验证失败，验证码参数不正确";
            }
            return false;
        }

        /// <summary>
        /// 进行图片地址的http替换
        /// </summary>
        /// <param name="url">原路径字符串</param>
        /// <param name="replaceHttp">替换的http域名</param>
        /// <returns></returns>
        public static string ReplaceImgUrl(string url,string replaceHttp)
        {
            try
            {
                string regStr = "<img(.*?)(src)=\"[^(http)]";   //  [^()]+  非括弧号内内容的其它任意字符
                Regex reg = new Regex(regStr);
                return reg.Replace(url, "<img src=\"" + replaceHttp + "");
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 进行图片地址的http替换
        /// </summary>
        /// <param name="url"></param>
        /// <param name="replcaeHttp"></param>
        /// <returns></returns>
        public static string ReplaceImgUrl_1(string url ,string replaceHttp)
        {
            try {
                //string regStr = "&lt;img(.*?)(src)=&quot;[^(http)]";
                //Regex reg = new Regex(regStr);
                //return reg.Replace(url, "&lt;img src=&quot;" + replaceHttp + "");
                url = url.Replace("&lt;", "<");
                url = url.Replace("&gt;", ">");
                url = url.Replace("&quot;", "\"");
                string regStr = "<img(.*?)(src)=\"[^(http)]";   //  [^()]+  非括弧号内内容的其它任意字符
                Regex reg = new Regex(regStr);
                return reg.Replace(url, "<img src=\"" + replaceHttp + "");
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 对提交的商品组合进行分组计算运费金额
        /// </summary>
        /// <param name="goodsInfoList"></param>
        /// <returns></returns>
        public static double GetGoodsFareTempMoney(List<GoodsCartToOrder> goodsInfoList)
        {
            double fareContinueMoney = 0;
            try
            {
                var supplierMoneyList = goodsInfoList.GroupBy(a => new { a.SuppliersID, a.FareSysID }).Select(info =>
                new
                {
                    addWeight = info.Sum(w => (double)Convert.ToDouble(w.Weight * w.BuyNumber)),//一种模板只有一种计算方式  --所以得到总重  ，得到 首重， 得到计数单位，得到 续费金额 ,得到首费 ==》就能说当前模板总运费 
                    firstCount = info.ElementAtOrDefault(0).FirstCount,//首重
                    money = info.ElementAtOrDefault(0).FirstMoney,//首费
                    contuinCount = info.ElementAtOrDefault(0).ContinueCount,//计数单位
                    contuinMoney = info.ElementAtOrDefault(0).ContinueMoney,//续费金额
                    suppliersID = info.ElementAtOrDefault(0).SuppliersID
                });
                var moneyList = supplierMoneyList.GroupBy(y => y.suppliersID).Select(info => new { myu = info.Key, addMoney = info.Sum(w => (double)Convert.ToDouble((w.addWeight - Convert.ToDouble(w.firstCount)) > 0 ? (w.addWeight - Convert.ToDouble(w.firstCount)) / Convert.ToDouble(w.contuinCount) * Convert.ToDouble(w.contuinMoney) + Convert.ToDouble(w.money) : Convert.ToDouble(w.money))) });
                foreach (var item in moneyList)
                {
                    fareContinueMoney +=  item.addMoney ;
                }
                return fareContinueMoney;
            }
            catch
            {
                return 100;
            }
        }
        
        /// <summary>
        /// 对提交的商品组合进行分组计算运费金额--返回各个供应商合计
        /// </summary>
        /// <param name="goodsInfoList"></param>
        /// <returns></returns>
        public static Dictionary<int,double> GetGoodsSubFareTempMoney(List<GoodsCartToOrder> goodsInfoList)
        {
            try
            {
                var supplierMoneyList = goodsInfoList.GroupBy(a => new { a.SuppliersID, a.FareSysID }).Select(info =>
                new
                {
                    addWeight = info.Sum(w => (double)Convert.ToDouble(w.Weight * w.BuyNumber)),//一种模板只有一种计算方式  --所以得到总重  ，得到 首重， 得到计数单位，得到 续费金额 ,得到首费 ==》就能说当前模板总运费 
                    firstCount = info.ElementAtOrDefault(0).FirstCount,//首重
                    money = info.ElementAtOrDefault(0).FirstMoney,//首费
                    contuinCount = info.ElementAtOrDefault(0).ContinueCount,//计数单位
                    contuinMoney = info.ElementAtOrDefault(0).ContinueMoney,//续费金额
                    suppliersID = info.ElementAtOrDefault(0).SuppliersID
                });
                var moneyList = supplierMoneyList.GroupBy(y => y.suppliersID).Select(info => new { suppliersID = info.Key, addMoney = info.Sum(w => (double)Convert.ToDouble((w.addWeight -Convert.ToDouble(w.firstCount)) > 0 ? (w.addWeight -Convert.ToDouble(w.firstCount)) / Convert.ToDouble(w.contuinCount) * Convert.ToDouble( w.contuinMoney) + Convert.ToDouble( w.money) :Convert.ToDouble( w.money))) });
                Dictionary<int, double> dic = new Dictionary<int, double>();
                foreach (var item in moneyList)
                {
                    if (!dic.ContainsKey( item.suppliersID))
                    {
                        dic.Add(item.suppliersID, item.addMoney);
                    }
                }
                return dic;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将html文本转化为 文本内容方法NoHTML
        /// </summary>
        /// <param name="Htmlstring">HTML文本值</param>
        /// <returns></returns>
        public  string NoHTML(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(/d+);", "", RegexOptions.IgnoreCase);
            //替换掉 < 和 > 标记
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("/r/n", "");
            //返回去掉html标记的字符串
            return Htmlstring;
        }

    }
}