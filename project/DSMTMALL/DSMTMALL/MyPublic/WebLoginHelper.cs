using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSMTMALL.MyPublic
{
    public class WebLoginHelper
    {
        public const string SESSION_ADMIN = "session_user";//定义常量，用作session的变量名
        public const string SESSION_AUTH = "session_auth";
        public const string SESSION_TEMP_NAME = "session_temp_name";
        public const string SESSION_TEMP_DATA = "session_temp_data";
        public const string SESSION_OPENID = "session_openid";
        public const string SESSION_TOKEN_CODE = "session_token_code";
        public const string SESSION_TKOEN_IMG = "session_token_img";
       // private static DB.Model.M_Users userModel; //定义一个变量


        /// <summary>
        /// 判断用户是否登录
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin()
        {
            DB.Model.M_Users userModel = (DB.Model.M_Users)HttpContext.Current.Session[SESSION_ADMIN];//获取session中的值
            return userModel == null ? false : true;//判断是否有值，有返回true,无返回false
        }

        /// <summary>
        /// 获取用户登录ID
        /// </summary>
        /// <returns></returns>
        public static string GetUserID()
        {
            DB.Model.M_Users userModel = (DB.Model.M_Users)HttpContext.Current.Session[SESSION_ADMIN];//获取session中的值
            if (userModel != null)
            {
                return userModel.UserID;
            }
            return null;
        }

        /// <summary>
        /// 获取用户登陆名
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            DB.Model.M_Users userModel = (DB.Model.M_Users)HttpContext.Current.Session[SESSION_ADMIN];//获取session中的值
            if (userModel != null)
            {
                return userModel.UserName;
            }
            return null;
        }
        
        /// <summary>
        /// 获取用户名昵称
        /// </summary>
        /// <returns></returns>
        public static string GetNickName()
        {
            DB.Model.M_Users userModel = (DB.Model.M_Users)HttpContext.Current.Session[SESSION_ADMIN];//获取session中的值
            if (userModel != null)
            {
                return userModel.NickName;
            }
            return null;
        }

        /// <summary>
        /// 获取用户的微信IMG地址
        /// </summary>
        /// <returns></returns>
        public static string GetUserImg()
        {
            return Convert.ToString( HttpContext.Current.Session[WebLoginHelper.SESSION_TKOEN_IMG]);

        }

        /// <summary>
        /// 获取用户的手机号码
        /// </summary>
        /// <returns></returns>
        public static string GetUserPhone()
        {
            DB.Model.M_Users userModel = (DB.Model.M_Users)HttpContext.Current.Session[SESSION_ADMIN];//获取session中的值
            if (userModel != null)
            {
                return userModel.Phone;
            }
            return null;
        }

        /// <summary>
        /// 获取用户的OpenID
        /// </summary>
        /// <returns></returns>
        public static string GetUserOpenID()
        {
            DB.Model.M_Users userModel = (DB.Model.M_Users)HttpContext.Current.Session[SESSION_ADMIN];//获取session中的值
            if (userModel != null)
            {
                return userModel.OpenID;
            }
            return null;
        }

        /// <summary>
        /// 获取用户所属的公司ID
        /// </summary>
        /// <returns></returns>
        public static string GetUserCpySysID()
        {
            DB.Model.M_Users userModel = (DB.Model.M_Users)HttpContext.Current.Session[SESSION_ADMIN];//获取session中的值
            if (userModel != null)
            {
                return userModel.CpySysID;
            }
            return null;
        }

        /// <summary>
        /// 获取用户所属的公司ID
        /// </summary>
        /// <returns></returns>
        public static string GetCpyName()
        {
            DB.Model.M_Users userModel = (DB.Model.M_Users)HttpContext.Current.Session[SESSION_ADMIN];
            if (userModel != null)
            {
                return userModel.CpyName;
            }
            return null;
        }

        /// <summary>
        /// 获取用户所属公司简称
        /// </summary>
        /// <returns></returns>
        public static string GetSimpleName()
        {
            DB.Model.M_Users userModel = (DB.Model.M_Users)HttpContext.Current.Session[SESSION_ADMIN];
            if (userModel != null)
            {
                return userModel.SimpleName;
            }
            return null;
        }
    }
}