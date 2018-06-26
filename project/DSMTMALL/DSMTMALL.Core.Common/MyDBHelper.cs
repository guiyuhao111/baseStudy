using DSMTMALL.Core.Common.MyEnum;
using MySql.Data.MySqlClient;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace DSMTMALL.Core.Common
{
    public class MyDBHelper
    {
        //数据库连接超时时间
        public const int CONNECTIONTIMEOUT = 60;
        //数据库命令超时时间
        public const int COMMANDTIMEOUT = 60;
 
        private static Dictionary<string, string> dicConnectionString = new Dictionary<string, string>();

        #region 从webconfig配置文件中读取
        ///// <summary>
        ///// 获取连接字符串//无参构造函数
        ///// </summary>
        //public static string ConnectionString
        //{
        //    get
        //    {
        //        string _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        //        string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];//获取链接字符串是否是加密的
        //        if (ConStringEncrypt == "true")//如果是加密的就要对链接字符串进行解密
        //        {
        //            _connectionString = new DESHelper().Decrypt(_connectionString);
        //        }
        //        return _connectionString;
        //    }
        //}

        ///// <summary>
        ///// 得到web.config里配置项的数据库连接字符串。//有参构造函数，重写方法
        ///// </summary>
        ///// <param name="configName"></param>
        ///// <returns></returns>
        //public static string GetConnectionString(string configName)
        //{
        //    string connectionString = ConfigurationManager.AppSettings[configName];
        //    string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
        //    if (ConStringEncrypt == "true")
        //    {
        //        connectionString =new DESHelper().Decrypt(connectionString);
        //    }
        //    return connectionString;
        //}
        #endregion

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="dbEnum">连接类型枚举</param>
        /// <returns>string</returns>
        public static string GetSQLType(DBEnum dbEnum)
        {
            if (!dicConnectionString.ContainsKey(dbEnum.ToString() + "_SQLType"))
            {
                string configName = string.Empty;
                switch ((int)dbEnum)
                {
                    case 0: configName = "OnlineMasterDBConfig"; break;
                    default: configName = "OnlineSlaveDBConfig"; break;
                }
                string programFilesPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string sqlType = INIHelper.INIGetStringValue(programFilesPath + @"MyConfig.ini", configName, "SQLType", "MySQL");
                dicConnectionString.Add(dbEnum.ToString() + "_SQLType", sqlType);
            }
            return dicConnectionString[dbEnum.ToString() + "_SQLType"];
        }
        /// <summary>
        /// 获取数据库驱动名称
        /// </summary>
        /// <param name="dbEnum">连接类型枚举</param>
        /// <returns>string</returns>
        public static string GetProviderName(DBEnum dbEnum)
        {
            if (!dicConnectionString.ContainsKey(dbEnum.ToString() + "_ProviderName"))
            {
                string providerName = "MySql.Data.MySqlClient";
                string sqlType = GetSQLType(dbEnum);
                if (sqlType.ToLower() == "MySQL".ToLower())
                {
                    providerName = "MySql.Data.MySqlClient";
                }
                dicConnectionString.Add(dbEnum.ToString() + "_ProviderName", providerName);
            }
            return dicConnectionString[dbEnum.ToString() + "_ProviderName"];
        }
        /// <summary>
        /// 获取数据库实现层
        /// </summary>
        /// <param name="dbEnum">连接类型枚举</param>
        /// <returns>string</returns>
        public static string GetAssemblyPath(DBEnum dbEnum)
        {
            if (!dicConnectionString.ContainsKey(dbEnum.ToString() + "_AssemblyPath"))
            {
                string configName = string.Empty;
                string assemblyPath = string.Empty;
                switch ((int)dbEnum)
                {
                    case 0: configName = "OnlineMasterDBConfig"; break;
                    default: configName = "OnlineSlaveDBConfig"; break;
                }
                string ProgramFilesPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                assemblyPath = INIHelper.INIGetStringValue(ProgramFilesPath + @"MyConfig.ini", configName, "SQLDAL", "DSMTMALL.DB");
                string sqlType = GetSQLType(dbEnum);
                if (sqlType.ToLower() == "MySQL".ToLower())
                {
                    assemblyPath += ".MySQLDAL";
                }
                dicConnectionString.Add(dbEnum.ToString() + "_AssemblyPath", assemblyPath);
            }
            return dicConnectionString[dbEnum.ToString() + "_AssemblyPath"];
        }

        /// <summary>
        /// 得到MyConfig.ini里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="dbEnum"></param>
        /// <returns></returns>
        public static string GetConnectionString(MyEnum.DBEnum dbEnum)
        {
            if (!dicConnectionString.ContainsKey(dbEnum.ToString()))
            {
                string configName = string.Empty;
                switch ((int)dbEnum)
                {
                    case 0: configName = "OnlineMasterDBConfig"; break;
                    default: configName = "OnlineSlaveDBConfig"; break;
                }
                string ProgramFilesPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string server = INIHelper.INIGetStringValue(ProgramFilesPath + @"MyConfig.ini", configName, "Server", "");
                string port = INIHelper.INIGetStringValue(ProgramFilesPath + @"MyConfig.ini", configName, "Port", "");
                string database = INIHelper.INIGetStringValue(ProgramFilesPath + @"MyConfig.ini", configName, "Database", "");
                string uid = INIHelper.INIGetStringValue(ProgramFilesPath + @"MyConfig.ini", configName, "Uid", "");
                string pwd = INIHelper.INIGetStringValue(ProgramFilesPath + @"MyConfig.ini", configName, "Pwd", "");
                string sqlType = GetSQLType(dbEnum);
                if (sqlType.ToLower() == "MySQL".ToLower())
                {
                    dicConnectionString.Add(dbEnum.ToString(), "server=" + server + ";port=" + port + ";database=" + database + ";uid=" + uid + ";pwd=" + new DESHelper().Decrypt(pwd) + ";ConnectionTimeout=" + CONNECTIONTIMEOUT.ToString() + ";");
                }
            }
            return dicConnectionString[dbEnum.ToString()];
        }

        /// <summary>
        /// 准备数据库执行命令对象----获取DATETABLE数据所需——1
        /// </summary>
        /// <param name="dbCommand">数据库命令对象</param>
        /// <param name="dbConnection">数据库连接对象</param>
        /// <param name="commandTimeout">数据库命令超时时间</param>
        /// <param name="dbCommandText">数据库命令语句</param>
        /// <param name="dbParameter">数据库命令参数</param>
        /// <param name="dbTransaction">数据库命令事务</param>
        /// <param name="commandType">数据库命令类型</param>
        private static void PrepareCommand(DbCommand dbCommand, DbConnection dbConnection, int commandTimeout, string dbCommandText, DbParameter[] dbParameter, DbTransaction dbTransaction = null, CommandType commandType = CommandType.Text)
        {
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }
            dbCommand.Connection = dbConnection;
            dbCommand.CommandTimeout = commandTimeout;
            dbCommand.CommandText = dbCommandText;
            if (dbTransaction != null)
            {
                dbCommand.Transaction = dbTransaction;
            }
            dbCommand.CommandType = commandType;
            if (dbParameter != null && dbParameter.Length > 0)
            {
                foreach (DbParameter parameter in dbParameter)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    dbCommand.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// 获取SQL查询结果---获取DATETABLE数据所需——2
        /// </summary>
        public static DataSet GetDataSetQuery(string strSql, object objParam,string providerName,string connectionString,int commandTimeout)
        {
            using (DbConnection dbConnection = DbProviderFactories.GetFactory(providerName).CreateConnection())
            {
                dbConnection.ConnectionString = connectionString;
                using (DbCommand dbCommand = DbProviderFactories.GetFactory(providerName).CreateCommand())
                {
                    PrepareCommand(dbCommand, dbConnection, commandTimeout, strSql, (DbParameter[])objParam);
                    using (DbDataAdapter dbDataAdapter = DbProviderFactories.GetFactory(providerName).CreateDataAdapter())
                    {
                        dbDataAdapter.SelectCommand = dbCommand;
                        DataSet dataSet = new DataSet();
                        try
                        {
                            dbDataAdapter.Fill(dataSet);
                        }
                        catch (DbException ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        return dataSet;
                    }
                }
            }
        }
    }

}
