using ServiceStack.Redis;
using System;
using System.Collections;
using System.Web;

namespace DSMTMALL.Core.Common
{
    /// <summary>
	/// 缓存相关的操作类
	/// </summary>
	public class DataCache
    {
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }
        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }
        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }
        /// <summary>  
        /// 清除单一键缓存  
        /// </summary>  
        public static void RemoveKeyCache(string CacheKey)
        {
            try
            {
                System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                objCache.Remove(CacheKey);
            }
            catch { }
        }
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            if (_cache.Count > 0)
            {
                ArrayList al = new ArrayList();
                while (CacheEnum.MoveNext())
                {
                    al.Add(CacheEnum.Key);
                }
                foreach (string key in al)
                {
                    _cache.Remove(key);
                }
            }
        }
        /// <summary>
        /// 以列表形式返回已存在的所有缓存
        /// </summary>
        public static ArrayList ShowAllCache()
        {
            ArrayList al = new ArrayList();
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            if (_cache.Count > 0)
            {
                IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
                while (CacheEnum.MoveNext())
                {
                    al.Add(CacheEnum.Key);
                }
            }
            return al;
        }
    }



    public sealed class RedisHelper
    {
        #region Redis帮助类
        private static readonly string MainMasterAddress = INIHelper.GetValue("MainMasterRedisConfig", "Address", "127.0.0.1");
        private static readonly string MainMasterPort = INIHelper.GetValue("MainMasterRedisConfig", "Port", "6379");
        private static readonly string MainMasterPwd = new DESHelper().Decrypt(INIHelper.GetValue("MainMasterRedisConfig", "Pwd", string.Empty));
        private static readonly string MainSlaveAddress = INIHelper.GetValue("MainSlaveRedisConfig", "Address", "127.0.0.1");
        private static readonly string MainSlavePort = INIHelper.GetValue("MainSlaveRedisConfig", "Port", "6379");
        private static readonly string MainSlavePwd = new DESHelper().Decrypt(INIHelper.GetValue("MainSlaveRedisConfig", "Pwd", string.Empty));
        private static PooledRedisClientManager MainRedisClientManager = CreateManager(new string[] { string.Format("{0}:{1}", MainMasterAddress, MainMasterPort) }, new string[] { string.Format("{0}:{1}", MainSlaveAddress, MainSlavePort) });
        private static readonly string CommonMasterAddress = INIHelper.GetValue("CommonMasterRedisConfig", "Address", "127.0.0.1");
        private static readonly string CommonMasterPort = INIHelper.GetValue("CommonMasterRedisConfig", "Port", "6379");
        private static readonly string CommonMasterPwd = new DESHelper().Decrypt(INIHelper.GetValue("CommonMasterRedisConfig", "Pwd", string.Empty));
        private static readonly string CommonSlaveAddress = INIHelper.GetValue("CommonSlaveRedisConfig", "Address", "127.0.0.1");
        private static readonly string CommonSlavePort = INIHelper.GetValue("CommonSlaveRedisConfig", "Port", "6379");
        private static readonly string CommonSlavePwd = new DESHelper().Decrypt(INIHelper.GetValue("CommonSlaveRedisConfig", "Pwd", string.Empty));
        private static PooledRedisClientManager CommonRedisClientManager = CreateManager(new string[] { string.Format("{0}:{1}", CommonMasterAddress, CommonMasterPort) }, new string[] { string.Format("{0}:{1}", CommonSlaveAddress, CommonSlavePort) });
        private static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            RedisClientManagerConfig redisClientManagerConfig = new RedisClientManagerConfig();
            redisClientManagerConfig.AutoStart = true;
            redisClientManagerConfig.MaxReadPoolSize = 5;
            redisClientManagerConfig.MaxWritePoolSize = 5;
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, redisClientManagerConfig);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        public static bool SetCache(string key, object val, DateTime? dateTime = null, RedisEnum redisEnum = RedisEnum.Main)
        {
            try
            {
                using (IRedisClient iRedisClient = redisEnum == RedisEnum.Main ? MainRedisClientManager.GetClient() : CommonRedisClientManager.GetClient())
                {
                    iRedisClient.Password = redisEnum == RedisEnum.Main ? MainMasterPwd : CommonMasterPwd;
                    if (dateTime != null)
                    {
                        return iRedisClient.Set(key, val, (DateTime)dateTime);
                    }
                    else
                    {
                        return iRedisClient.Set(key, val);
                    }
                }
            }
            catch (Exception ex)
            {
                FileHelper.logger.Warn("Redis设置缓存时异常 || " + ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        public static T GetCache<T>(string key, RedisEnum redisEnum = RedisEnum.Main)
        {
            try
            {
                using (IRedisClient iRedisClient = redisEnum == RedisEnum.Main ? MainRedisClientManager.GetReadOnlyClient() : CommonRedisClientManager.GetReadOnlyClient())
                {
                    iRedisClient.Password = redisEnum == RedisEnum.Main ? MainSlavePwd : CommonSlavePwd;
                    return iRedisClient.Get<T>(key);
                }
            }
            catch (Exception ex)
            {
                FileHelper.logger.Warn("Redis获取缓存时异常 || " + ex.Message);
            }
            return default(T);
        }
        /// <summary>
        /// 入队列
        /// </summary>
        public static bool SetQueue(string key, string val, RedisEnum redisEnum = RedisEnum.Main)
        {
            try
            {
                using (IRedisClient iRedisClient = redisEnum == RedisEnum.Main ? MainRedisClientManager.GetClient() : CommonRedisClientManager.GetClient())
                {
                    iRedisClient.Password = redisEnum == RedisEnum.Main ? MainMasterPwd : CommonMasterPwd;
                    iRedisClient.EnqueueItemOnList(key, val);
                    return true;
                }
            }
            catch (Exception ex)
            {
                FileHelper.logger.Warn("Redis入队列时异常 || " + ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 出队列
        /// </summary>
        public static string GetQueue(string key, RedisEnum redisEnum = RedisEnum.Main)
        {
            try
            {
                using (IRedisClient iRedisClient = redisEnum == RedisEnum.Main ? MainRedisClientManager.GetClient() : CommonRedisClientManager.GetClient())
                {
                    iRedisClient.Password = redisEnum == RedisEnum.Main ? MainMasterPwd : CommonMasterPwd;
                    return iRedisClient.DequeueItemFromList(key);
                }
            }
            catch (Exception ex)
            {
                FileHelper.logger.Warn("Redis出队列时异常 || " + ex.Message);
            }
            return string.Empty;
        }
        /// <summary>
        /// 压栈
        /// </summary>
        public static bool SetStack(string key, string val, RedisEnum redisEnum = RedisEnum.Main)
        {
            try
            {
                using (IRedisClient iRedisClient = redisEnum == RedisEnum.Main ? MainRedisClientManager.GetClient() : CommonRedisClientManager.GetClient())
                {
                    iRedisClient.Password = redisEnum == RedisEnum.Main ? MainMasterPwd : CommonMasterPwd;
                    iRedisClient.PushItemToList(key, val);
                    return true;
                }
            }
            catch (Exception ex)
            {
                FileHelper.logger.Warn("Redis压栈时异常 || " + ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 出栈
        /// </summary>
        public static string GetStack(string key, RedisEnum redisEnum = RedisEnum.Main)
        {
            try
            {
                using (IRedisClient iRedisClient = redisEnum == RedisEnum.Main ? MainRedisClientManager.GetClient() : CommonRedisClientManager.GetClient())
                {
                    iRedisClient.Password = redisEnum == RedisEnum.Main ? MainMasterPwd : CommonMasterPwd;
                    return iRedisClient.PopItemFromList(key);
                }
            }
            catch (Exception ex)
            {
                FileHelper.logger.Warn("Redis出栈时异常 || " + ex.Message);
            }
            return string.Empty;
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        public static bool DeleteCache(string key, RedisEnum redisEnum = RedisEnum.Main)
        {
            try
            {
                using (IRedisClient iRedisClient = redisEnum == RedisEnum.Main ? MainRedisClientManager.GetClient() : CommonRedisClientManager.GetClient())
                {
                    iRedisClient.Password = redisEnum == RedisEnum.Main ? MainMasterPwd : CommonMasterPwd;
                    return iRedisClient.Remove(key);
                }
            }
            catch (Exception ex)
            {
                FileHelper.logger.Warn("Redis删除缓存时异常 || " + ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 清空缓存
        /// </summary>
        public static bool DeleteAllCache(RedisEnum redisEnum = RedisEnum.Main)
        {
            try
            {
                using (IRedisClient iRedisClient = redisEnum == RedisEnum.Main ? MainRedisClientManager.GetClient() : CommonRedisClientManager.GetClient())
                {
                    iRedisClient.Password = redisEnum == RedisEnum.Main ? MainMasterPwd : CommonMasterPwd;
                    iRedisClient.FlushAll();
                    return true;
                }
            }
            catch (Exception ex)
            {
                FileHelper.logger.Warn("Redis清空缓存时异常 || " + ex.Message);
            }
            return false;
        }
        #endregion Redis帮助类

        public enum RedisEnum
        {
            Main,
            Common
        }

    }



}