using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.DB.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DSMTMALL.DB.DALFactory
{
    public sealed class FactoryHelper
    {
        /// <summary>
        /// 创建对象或从缓存获取
        /// </summary>
        public static object CreateObject(string AssemblyPath, string ClassNamespace, DBEnum dbEnum)
        {
            object objType = DataCache.GetCache(ClassNamespace + "_" + dbEnum.ToString());//从缓存读取
            if (objType == null)
            {
                try
                {                                                       //通过GetConstructor()这个方法可以在实例化的时候选择自己想要的构造函数
                    objType = Assembly.Load(AssemblyPath).GetType(ClassNamespace).GetConstructor(new Type[] { typeof(DBEnum) }).Invoke(new object[] { dbEnum });//反射创建
                    DataCache.SetCache(ClassNamespace + "_" + dbEnum.ToString(), objType);// 写入缓存
                }
                catch { }
            }
            return objType;
        }
        /// <summary>
		/// 创建MY_Dal数据层接口。
		/// </summary>
		public static IMY_Dal CreateMY_Dal(DBEnum dbEnum)
        {
            return (IMY_Dal)CreateObject(MyDBHelper.GetAssemblyPath(dbEnum), MyDBHelper.GetAssemblyPath(dbEnum) + ".MY_Dal", dbEnum);
        }
        /// <summary>
        /// 创建MY_Dal数据层接口。
        /// </summary>
        public static IMB_Dal CreateMB_Dal(DBEnum dbEnum)
        {
            return (IMB_Dal)CreateObject(MyDBHelper.GetAssemblyPath(dbEnum), MyDBHelper.GetAssemblyPath(dbEnum) + ".MB_Dal", dbEnum);
        }

        
    }
}
