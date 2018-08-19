using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;

namespace DSMTMALL.Core.Common
{
    public class EntityHelper<T> where T : new()
    {
        #region 数组转换成实体类集合
        /// <summary>
        /// 数组转实体类集合
        /// </summary>
        /// <param name="arr">待转换数组</param>
        /// <param name="sign">分隔符</param>
        /// <returns>转换后实体类集合</returns>
        public static List<T> ArrToEntity(string[] arr, char sign)
        {
            List<T> tList = new List<T>();
            string[] tempArr = null;
            for (int i = 0; i < arr.Length; i++)
            {
                T t = new T();
                PropertyInfo[] pArray = typeof(T).GetProperties();
                tempArr = arr[i].Split(sign);
                for (int j = 0; j < tempArr.Length; j++)
                {
                    pArray[j].SetValue(t, Convert.ChangeType(tempArr[j], pArray[j].PropertyType), null);
                }
                tList.Add(t);
            }
            return tList;
        }
        #endregion 数组转换成实体类集合

        #region DataTable转换成实体类集合
        /// <summary>
        /// DataSet的第一个表转实体类集合
        /// </summary>
        /// <param name="ds">待转换DataSet</param>
        /// <returns>转换后实体类集合</returns>
        public static List<T> DataSetToEntity(DataSet ds)
        {
            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return DataTableToEntity(ds.Tables[0]);
            }
        }
        /// <summary>
        /// DataSet的第index个表转实体类集合
        /// </summary>
        /// <param name="ds">待转换DataSet</param>
        /// <returns>转换后实体类集合</returns>
        public static List<T> DataSetToEntity(DataSet ds, int index)
        {
            if (ds == null || ds.Tables.Count <= index || ds.Tables[index].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return DataTableToEntity(ds.Tables[index]);
            }
        }
        /// <summary>
        /// DataTable转实体类集合
        /// </summary>
        /// <param name="dt">待转换DataTable</param>
        /// <returns>转换后实体类集合</returns>
        public static List<T> DataTableToEntity(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> tList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();//初始化设置一个范型变量
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    PropertyInfo propertyInfo = typeof(T).GetProperty(dr.Table.Columns[i].ColumnName);
                    if (propertyInfo != null && dr[i] != DBNull.Value)
                    {
                        propertyInfo.SetValue(t, dr[i], null);
                        //propertyInfo.SetValue(t, dr[i].GetType() == typeof(System.UInt32) ? Convert.ToInt32(dr[i]) : dr[i], null);
                    }
                }
                tList.Add(t);
            }
            return tList;
        }
        /// <summary>
        /// DataRow转实体类
        /// </summary>
        /// <param name="dr">待转换DataRow</param>
        /// <returns>转换后实体类</returns>
        public static T DataRowToEntity(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }
            T t = new T();
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(dr.Table.Columns[i].ColumnName);
                if (propertyInfo != null && dr[i] != DBNull.Value)
                {
                    propertyInfo.SetValue(t, dr[i], null);
                }
            }
            return t;
        }
        #endregion DataTable转换成实体类集合

        #region 实体类集合转换成DataTable
        /// <summary>
        /// 实体类集合转DataSet
        /// </summary>
        /// <param name="tList">待转换实体类集合</param>
        /// <returns>转换后DataSet</returns>
        public static DataSet EntityToDataSet(List<T> tList)
        {
            if (tList == null || tList.Count == 0)
            {
                return null;
            }
            else
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(EntityToDataTable(tList));
                return ds;
            }
        }
        /// <summary>
        /// 实体类集合转DataTable
        /// </summary>
        /// <param name="tList">待转换实体类集合</param>
        /// <returns>转换后DataTable</returns>
        public static DataTable EntityToDataTable(List<T> tList)
        {
            if (tList == null || tList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateDataTable(tList[0]);
            foreach (var t in tList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (var propertyInfo in typeof(T).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(t, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }
        /// <summary>
        /// 实体类转表结构
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns>转换后DataTable</returns>
        private static DataTable CreateDataTable(T t)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }
        #endregion 实体类集合转换成DataTable

        #region 给实体类赋值
        /// <summary>
        /// 实体类赋值给实体类
        /// </summary>
        /// <typeparam name="A">实体类型</typeparam>
        /// <param name="a">实体类</param>
        /// <param name="dicRvePar">对应关系字典</param>
        /// <returns>赋值后实体类</returns>
        public static T EntityValueToEntity<A>(A a, Dictionary<string, string> dicRvePar) where A : new()
        {
            if (a == null || dicRvePar == null)
            {
                return default(T);
            }
            T t = new T();
            foreach (var rvePar in dicRvePar)
            {
                PropertyInfo propertyInfo = typeof(A).GetProperty(rvePar.Value);
                string aValue = propertyInfo == null ? string.Empty : propertyInfo.GetValue(a).ToString();
                propertyInfo = typeof(T).GetProperty(rvePar.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(t, aValue, null);
                }
            }
            return t;
        }
        /// <summary>
        /// 实体类集合赋值给实体类集合
        /// </summary>
        /// <typeparam name="A">实体类型</typeparam>
        /// <param name="aList">实体类集合</param>
        /// <param name="dicRvePar">对应关系字典</param>
        /// <returns>赋值后实体类集合</returns>
        public static List<T> EntityListValueToEntityList<A>(List<A> aList, Dictionary<string, string> dicRvePar) where A : new()
        {
            if (aList == null || aList.Count <= 0 || dicRvePar == null || dicRvePar.Count <= 0)
            {
                return null;
            }
            List<T> tList = new List<T>();
            foreach (var a in aList)
            {
                T t = new T();
                foreach (var rvePar in dicRvePar)
                {
                    PropertyInfo propertyInfo = typeof(A).GetProperty(rvePar.Value);
                    string aValue = propertyInfo == null ? string.Empty : propertyInfo.GetValue(a).ToString();
                    propertyInfo = typeof(T).GetProperty(rvePar.Key);
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(t, aValue, null);
                    }
                }
                tList.Add(t);
            }
            return tList;
        }
        /// <summary>
        /// DataTable赋值给实体类
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dicRvePar">对应关系字典</param>
        /// <returns>赋值后实体类</returns>
        public static T DataTableValueToEntity(DataTable dt, Dictionary<string, string> dicRvePar)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return default(T);
            }
            T t = new T();
            foreach (var rvePar in dicRvePar)
            {
                //DBNull.Value 相当于判断字符串是否为空的string.Empty,是用在datatable里判断某行某列的值是否为null的用法
                string colValue = dt.Rows[0][rvePar.Value] == DBNull.Value ? string.Empty : dt.Rows[0][rvePar.Value].ToString();
                PropertyInfo propertyInfo = typeof(T).GetProperty(rvePar.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(t, colValue, null);
                }
            }
            return t;
        }
        /// <summary>
        /// DataTable赋值给实体类集合
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dicRvePar">对应关系字典</param>
        /// <returns>赋值后实体类集合</returns>
        public static List<T> DataTableValueToEntityList(DataTable dt, Dictionary<string, string> dicRvePar)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> tList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                foreach (var rvePar in dicRvePar)
                {
                    string colValue = dr[rvePar.Value] == DBNull.Value ? string.Empty : dr[rvePar.Value].ToString();
                    PropertyInfo propertyInfo = typeof(T).GetProperty(rvePar.Key);
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(t, colValue, null);
                    }
                }
                tList.Add(t);
            }
            return tList;
        }
        #endregion 给实体类赋值
        
        #region 实体类转换成字典
        /// <summary>
        /// 实体类转换成Dictionary
        /// </summary>
        /// <param name="entity">待转换实体类</param>
        /// <param name="isSort">是否排序</param>
        /// <param name="removePropertyArr">去除属性数组</param>
        /// <returns>转换后Dictionary</returns>
        public static Dictionary<string, object> EntityToDictionary(T entity, bool isSort, string[] removePropertyArr)
        {
            if (entity == null)
            {
                return null;
            }
            bool isAdd = true;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                isAdd = true;
                MethodInfo methodInfo = propertyInfo.GetGetMethod();
                if (methodInfo != null && methodInfo.IsPublic)
                {
                    if (removePropertyArr != null && removePropertyArr.Length > 0)
                    {
                        for (int i = 0; i < removePropertyArr.Length; i++)
                        {
                            if (propertyInfo.Name.ToLower() == removePropertyArr[i].ToLower())
                            {
                                isAdd = false;
                                break;
                            }
                        }
                    }
                    if (isAdd)
                    {
                        dictionary.Add(propertyInfo.Name, methodInfo.Invoke(entity, new object[] { }));
                    }
                }
            }
            if (isSort)
            {
                dictionary = new Dictionary<string, object>(new SortedDictionary<string, object>(dictionary));
            }
            return dictionary;
        }
        #endregion 实体类转换成字典

        #region 实体类转换成字符串

        /// <summary>
        /// 实体类转换成字符串
        /// </summary>
        /// <param name="entity">待转换实体类</param>
        /// <param name="mySign1">分隔符1</param>
        /// <param name="mySign2">分隔符2</param>
        /// <param name="encoding">URL编码</param>
        /// <param name="removePropertyArr">去除属性数组</param>
        /// <returns>转换后字符串</returns>
        public static string EntityToString(T entity, char mySign1, char mySign2, Encoding encoding, string[] removePropertyArr)
        {
            if (entity == null)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, object> keyValuePair in EntityToDictionary(entity, true, removePropertyArr))
            {
                if (encoding == null)
                {
                    stringBuilder.Append(keyValuePair.Key + mySign1 + keyValuePair.Value == null ? string.Empty : keyValuePair.Value.ToString() + mySign2);
                }
                else
                {
                    stringBuilder.Append(keyValuePair.Key + mySign1 + HttpUtility.UrlEncode(keyValuePair.Value == null ? string.Empty : keyValuePair.Value.ToString(), encoding) + mySign2);
                }
            }
            return stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString();
        }
        /// <summary>
        /// 实体类转换成数据库添加字符串
        /// </summary>
        /// <param name="isBefore">是否在前</param>
        /// <returns>转换后字符串</returns>
        public static string EntityToAddSQLString(bool isBefore)
        {
            string cacheName = isBefore ? typeof(T).Name + "_AddSQLString_Before" : typeof(T).Name + "_AddSQLString_After";
            string sqlString = (string)DataCache.GetCache(cacheName);
            if (string.IsNullOrEmpty(sqlString))
            {
                sqlString = isBefore ? EntityToSQLString(0) : EntityToSQLString(1);
                DataCache.SetCache(cacheName, sqlString);
            }
            return sqlString;
        }

        /// <summary>
        /// 实体类转换成数据库修改字符串
        /// </summary>
        /// <param name="t">待转换实体类</param>
        /// <returns>转换后字符串</returns>
        public static string EntityToUpdateSQLString(object t)
        {
            return EntityToSQLString(2, t);
        }

        /// <summary>
        /// 实体类转换成数据库字符串
        /// </summary>
        /// <param name="isAddOrUpdate">添加还是修改</param>
        /// <returns>转换后字符串</returns>
        private static string EntityToSQLString(int isAddOrUpdate, object t = null)
        {
            PropertyInfo[] propertyInfoArr = isAddOrUpdate == 2 ? t.GetType().GetProperties() : typeof(T).GetProperties();
            if (propertyInfoArr != null && propertyInfoArr.Length > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < propertyInfoArr.Length; i++)
                {
                    if (isAddOrUpdate == 0)
                    {
                        stringBuilder.Append(propertyInfoArr[i].Name + ',');
                    }
                    else if (isAddOrUpdate == 1)
                    {
                        stringBuilder.Append('@' + propertyInfoArr[i].Name + ',');
                    }
                    else
                    {
                        if (!propertyInfoArr[i].Name.StartsWith("_"))
                        {
                            stringBuilder.Append(propertyInfoArr[i].Name + "=@" + propertyInfoArr[i].Name + ',');
                        }
                    }
                }
                return stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString();
            }
            return string.Empty;
        }
        #endregion 实体类转换成字符串
    }
}