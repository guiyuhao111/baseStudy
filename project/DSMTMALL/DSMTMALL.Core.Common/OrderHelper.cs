using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DSMTMALL.Core.Common
{
   public class OrderHelper
    {
        //初始化接口信息
        /// <summary>
        /// 传入一个字典数据组---转化为提交的SIGN信息
        /// </summary>
        /// <param name="dicQuery"></param>
        public string WMSStringHanlder(SortedDictionary<string ,string> dicQuery,string appsecret)
        {
            string wMSPostStr = appsecret;
            foreach (var item in dicQuery)
            {
                wMSPostStr += item.Key.Trim() + item.Value.Trim(); // dic.Add(item.Key, item.Value);
            }
            wMSPostStr = new MD5Helper(appsecret).Encrypt(wMSPostStr).ToLower();
            return wMSPostStr;
        }
    }
}
