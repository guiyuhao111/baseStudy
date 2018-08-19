using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;

namespace DSMTMALL.Core.Common
{
    public class JsonHelper
    {
       
        /// <summary>
        /// DataTable转Json
        /// </summary>
        public static string DataTableToJson(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return "\"\"";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> listDic = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc].ToString());
                }
                listDic.Add(dic);
            }
            return serializer.Serialize(listDic);
        }
      

        /// <summary>
        /// Dictionary转JSON格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string DictionaryToJson<T>(T t)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(t);
        }

        /// <summary>
        /// json字符串转字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T JsonToDictionary<T>(string strJson)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(strJson);
        }
        /// <summary>
        /// json字符串转实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T JsonToEntitys<T>(string strJson)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(strJson);
        }

    }
}