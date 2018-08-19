using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace DSMTMALL.Core.DBUtility
{
    public class PubConstant
    {
        private static Dictionary<string, string> dicConnectionString = new Dictionary<string, string>();
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
                if (ConStringEncrypt == "true")
                {
                    _connectionString = DESEncrypt.Decrypt(_connectionString);
                }
                return _connectionString;
            }
        }
        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            string connectionString = ConfigurationManager.AppSettings[configName];
            string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
            if (ConStringEncrypt == "true")
            {
                connectionString = DESEncrypt.Decrypt(connectionString);
            }
            return connectionString;
        }
        /// <summary>
        /// 得到MyConfig.ini里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="dbEnum"></param>
        /// <returns></returns>
        public static string GetConnectionStringFromINI(DBEnum dbEnum)
        {
            if (!dicConnectionString.ContainsKey(dbEnum.ToString()))
            {
                string configName = null;
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
                dicConnectionString.Add(dbEnum.ToString(), "server=" + server + ";port=" + port + ";database=" + database + ";uid=" + uid + ";pwd=" + new DESHelper().Decrypt(pwd));
            }
            return dicConnectionString[dbEnum.ToString()];
        }
    }
}