using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace DSMTMALL.BACKMAG.MyPublic
{
    public class WebToolHelper
    {
        private readonly string AssemblyPath = ConfigurationManager.AppSettings["MySQLBLL"];//从配置文件读
        /// <summary>
        /// 获取联想集合 (传入的是要查询的表，sql查询语句内容，要放入字典中的key值与value值，标签输入的值用来做字符串判断的，要排除在外的字符串-传入""时就是指不进行排除)
        /// </summary>
        /// <param name="sessionEnum">临时Session枚举名称</param>
        /// <param name="modelName">Model名称</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="id">待转换ID</param>
        /// <param name="name">待转换名称</param>
        /// <param name="queryName">查询名称标签输入的值用来查询的内容</param>
        /// <param name="excludeID">排除ID（排除传入的这个内容值比如要排除自己公司的sysid）</param>
        /// <returns>Dictionary</returns>
        public Dictionary<string, string> GetUlList(SessionEnum sessionEnum, string modelName, string strWhere, string id, string name, string queryName, string excludeID)
        {
            queryName = queryName.Replace(@"\", "");
            Dictionary<string, string> dicFrom = new Dictionary<string, string>();//定义两个字典类型的变量
            Dictionary<string, string> dicTo = new Dictionary<string, string>();
            object sessionTempName = HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_NAME];//尝试获取某个session的值（这个session存储的是上个session使用过的临时名）
            if (sessionTempName == null || (SessionEnum)sessionTempName != sessionEnum)//如果这个session的值为空（即获取不到这个session值，或者这个session的临时名字不叫sessionEnum）
            {   //以下是调用自己的接口文件的方法
                //Type typeBLL = Assembly.Load("DESIGN.DB.BLL").GetType("DESIGN.DB.BLL." + modelName);
                Type typeBLL = Assembly.Load(AssemblyPath).GetType(AssemblyPath + "." + modelName);
                object objConstructor = typeBLL.GetConstructor(new Type[] { typeof(DBEnum) }).Invoke(new object[] { DBEnum.Slave });
                MethodInfo methodInfo = typeBLL.GetMethod("GetList", new Type[] { typeof(string) });
                object objDataSet = methodInfo.Invoke(objConstructor, new object[] { strWhere });
                dicFrom = ToolHelper.DataTableToDic(((DataSet)objDataSet).Tables[0], id, name);
                HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_DATA] = dicFrom;
                HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_NAME] = sessionEnum;
            }
            else
            {
                dicFrom = (Dictionary<string, string>)HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_DATA];
            }
            if (dicFrom.Count > 0)
            {
                Regex regex = new Regex(queryName);
                Match match = null;
                foreach (var dic in dicFrom)
                {
                    if (dic.Key != excludeID)
                    {
                        match = regex.Match(dic.Value);
                        if (match.Success)
                        {
                            dicTo.Add(dic.Key, dic.Value);
                        }
                    }
                }
            }
            return dicTo;
        }

        /// <summary>
        /// 获取联想集合 (传入的是要查询的表，sql查询语句内容，要放入字典中的key值与value值，标签输入的值用来做字符串判断的，要排除在外的字符串-传入""时就是指不进行排除)
        /// </summary>
        /// <param name="sessionEnum">临时Session枚举名称</param>
        /// <param name="modelName">Model名称</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="id">待转换ID</param>
        /// <param name="name">编号列名称</param>
        /// <param name="name">待转换名称</param>
        /// <param name="queryName">查询名称标签输入的值用来查询的内容</param>
        /// <param name="excludeID">排除ID（排除传入的这个内容值比如要排除自己公司的sysid）</param>
        /// <returns>Dictionary</returns>
        public Dictionary<string, string> GetUlList(SessionEnum sessionEnum, string modelName, string strWhere, string id,string number, string name, string queryName, string excludeID)
        {
            queryName = queryName.Replace(@"\", "");
            Dictionary<string, string> dicFrom = new Dictionary<string, string>();//定义两个字典类型的变量
            Dictionary<string, string> dicTo = new Dictionary<string, string>();
            object sessionTempName = HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_NAME];//尝试获取某个session的值（这个session存储的是上个session使用过的临时名）
            if (sessionTempName == null || (SessionEnum)sessionTempName != sessionEnum)//如果这个session的值为空（即获取不到这个session值，或者这个session的临时名字不叫sessionEnum）
            {   //以下是调用自己的接口文件的方法
                Type typeBLL = Assembly.Load(AssemblyPath).GetType(AssemblyPath + "." + modelName);
                object objConstructor = typeBLL.GetConstructor(new Type[] { typeof(DBEnum) }).Invoke(new object[] { DBEnum.Slave });
                MethodInfo methodInfo = typeBLL.GetMethod("GetList", new Type[] { typeof(string) });
                object objDataSet = methodInfo.Invoke(objConstructor, new object[] { strWhere });
                dicFrom = ToolHelper.DataTableToDic(((DataSet)objDataSet).Tables[0], id,number, name);
                HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_DATA] = dicFrom;
                HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_NAME] = sessionEnum;
            }
            else
            {
                dicFrom = (Dictionary<string, string>)HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_DATA];
            }
            if (dicFrom.Count > 0)
            {
                Regex regex = new Regex(queryName);
                Match match = null;
                foreach (var dic in dicFrom)
                {
                    if (dic.Key != excludeID)
                    {
                        match = regex.Match(dic.Value);
                        if (match.Success)
                        {
                            dicTo.Add(dic.Key, dic.Value);
                        }
                    }
                }
            }
            return dicTo;
        }

        /// <summary>
        ///  获取联想集合，传入的值内容是DataTable类型的
        /// </summary>
        /// <param name="sessionEnum">临时session的名字</param>
        /// <param name="dt">传入的DataTable数据集名</param>
        /// <param name="id">字典的key</param>
        /// <param name="name">字典的value</param>
        /// <param name="queryName">标签获取的字符串</param>
        /// <param name="excludeID">要排除在外的内容</param>
        /// <returns></returns>
        public Dictionary<string, string> GetUlListByIEnumerable(SessionEnum sessionEnum, IEnumerable<dynamic> dt,  string queryName, string excludeID, Action<IEnumerable<dynamic>, Dictionary<string, string>> iEnumerable)
        {
            Dictionary<string, string> dicFrom = new Dictionary<string, string>();//定义两个字典类型的变量
            Dictionary<string, string> dicTo = new Dictionary<string, string>();
            object sessionTempName = HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_NAME];//尝试获取某个session的值（这个session存储的是上个session使用过的临时名）
            if (sessionTempName == null || (SessionEnum)sessionTempName != sessionEnum)//如果这个session的值为空（即获取不到这个session值，或者这个session的临时名字不叫我们现在传入的这个session的枚举值sessionEnum）
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dicFrom = ToolHelper.IEnumerableListToDic(dt,iEnumerable); //ToolHelper.DataTableToDic(dt, id, name);//将dataTable转化成字典类型
                HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_DATA] = dicFrom;//将转化后的字典内容赋值给这个临时session值内容
                HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_NAME] = sessionEnum;//将这个临时文session的临时名命名为我们传入的session枚举名
            }
            else
            {
                dicFrom = (Dictionary<string, string>)HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_DATA];//否则直接从session中读取字典内容
            }
            if (dicFrom.Count > 0)//如果字典内容不为空
            {
                try
                {
                    //我们要在目标字符串中找到我们传入的从标签获取到的值，看是否存在，而目标字符串就是使用代码 Match match = regex.Match(dic.Value);//这个就是确认我们要进行匹配的目标字符串（dic.Value）,通过这句代码实现。
                    Regex regex = new Regex(queryName);//调用系统本身的正则表达式类（类似于文件操作类，路径操作类）
                    Match match = null;
                    foreach (var dic in dicFrom)//dic代表的是字典的每一行的key与value的集合
                    {
                        if (dic.Key != excludeID)//判断如果字典的key值不等于要排除在外的内容
                        {
                            match = regex.Match(dic.Value);//调用系统的
                            if (match.Success)
                            {
                                dicTo.Add(dic.Key, dic.Value);
                            }
                        }
                    }
                }
                catch { dicTo.Add("ERR", "请不要输入非法字符"); }
            }
            return dicTo;
        }


        /// <summary>
        ///  获取联想集合，传入的值内容是DataTable类型的
        /// </summary>
        /// <param name="sessionEnum">临时session的名字</param>
        /// <param name="dt">传入的DataTable数据集名</param>
        /// <param name="id">字典的key</param>
        /// <param name="name">字典的value</param>
        /// <param name="queryName">标签获取的字符串</param>
        /// <param name="excludeID">要排除在外的内容</param>
        /// <returns></returns>
        public Dictionary<string, string> GetUlListByIEnumerable(SessionEnum sessionEnum, string strSql, string queryName, string excludeID, Action<IEnumerable<dynamic>, Dictionary<string, string>> iEnumerable)
        {
            Dictionary<string, string> dicFrom = new Dictionary<string, string>();//定义两个字典类型的变量
            Dictionary<string, string> dicTo = new Dictionary<string, string>();
            object sessionTempName = HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_NAME];//尝试获取某个session的值（这个session存储的是上个session使用过的临时名）
            if (sessionTempName == null || (SessionEnum)sessionTempName != sessionEnum)//如果这个session的值为空（即获取不到这个session值，或者这个session的临时名字不叫我们现在传入的这个session的枚举值sessionEnum）
            {
                IEnumerable<dynamic> info = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList(strSql, null);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dicFrom = ToolHelper.IEnumerableListToDic(info, iEnumerable); //ToolHelper.DataTableToDic(dt, id, name);//将dataTable转化成字典类型
                HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_DATA] = dicFrom;//将转化后的字典内容赋值给这个临时session值内容
                HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_NAME] = sessionEnum;//将这个临时文session的临时名命名为我们传入的session枚举名
            }
            else
            {
                dicFrom = (Dictionary<string, string>)HttpContext.Current.Session[WebLoginHelper.SESSION_TEMP_DATA];//否则直接从session中读取字典内容
            }
            if (dicFrom.Count > 0)//如果字典内容不为空
            {
                try
                {
                    //我们要在目标字符串中找到我们传入的从标签获取到的值，看是否存在，而目标字符串就是使用代码 Match match = regex.Match(dic.Value);//这个就是确认我们要进行匹配的目标字符串（dic.Value）,通过这句代码实现。
                    Regex regex = new Regex(queryName);//调用系统本身的正则表达式类（类似于文件操作类，路径操作类）
                    Match match = null;
                    foreach (var dic in dicFrom)//dic代表的是字典的每一行的key与value的集合
                    {
                        if (dic.Key != excludeID)//判断如果字典的key值不等于要排除在外的内容
                        {
                            match = regex.Match(dic.Value);//调用系统的
                            if (match.Success)
                            {
                                dicTo.Add(dic.Key, dic.Value);
                            }
                        }
                    }
                }
                catch { dicTo.Add("ERR", "请不要输入非法字符"); }
            }
            return dicTo;
        }

    }
}