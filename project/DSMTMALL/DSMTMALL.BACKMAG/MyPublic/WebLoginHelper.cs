using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Web;

namespace DSMTMALL.BACKMAG.MyPublic
{
    public class WebLoginHelper
    {
        public const string SESSION_ADMIN = "session_admin";//定义常量，用作session的变量名
        public const string SESSION_AUTH = "session_auth";
        public const string SESSION_TEMP_NAME = "session_temp_name";
        public const string SESSION_TEMP_DATA = "session_temp_data";
        //private static DB.Model.M_AdminUser adminModel; //定义一个变量


        /// <summary>
        /// 判断用户是否登录
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin()
        {
            DB.Model.M_AdminUser adminModel = (DB.Model.M_AdminUser)HttpContext.Current.Session[SESSION_ADMIN];//获取session中的值
            return adminModel == null ? false : true;//判断是否有值，有返回true,无返回false
        }

        /// <summary>
        ///  判断用户权限（是否有模块权限）
        /// </summary>
        /// <param name="modalNo">模块编号</param>
        /// <returns></returns>
        public static bool IsAuthority(AdminAuth modalNo)
        {
            DB.Model.M_AdminUser adminModel = (DB.Model.M_AdminUser)HttpContext.Current.Session[SESSION_ADMIN];//从sesion获取当前用户相关信息
            List<DB.Model.M_Modal> modalAuthVal = GetModalAuth();//获取当前用户的模块权限信息
            if (adminModel != null)//判断当前用户信息是否为空，既是否登录
            {
                if (modalAuthVal != null)//判断模块信息是否为空，如果为空，返回false
                {
                    for (int j = 0; j < modalAuthVal.Count; j++)//遍历循环，寻找当模块为相应编号的时候的相应模块权限是否为我们传入的值
                    {
                        if (modalAuthVal[j].ModalNo == (int)modalNo)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// 获取用户登录后的默认首页，如果有权限模块，则跳转到相应的默认排序第一个的权限模块中去，如果没有，则跳转到系统管理下的密码修改
        /// </summary>
        /// <returns></returns>
        public static string GetFirstPage()
        {
            if (GetModalAuth().Count > 0)
            {
                return GetModalAuth()[0].ModalPage;
            }
            return "admin/web_admin_pwd.aspx";
        }

        /// <summary>
        /// 获取管理员名称
        /// </summary>
        /// <returns></returns>
        public static string GetAdminName()
        {
            DB.Model.M_AdminUser adminModel = (DB.Model.M_AdminUser)HttpContext.Current.Session[SESSION_ADMIN];
            if (adminModel != null)
            {
                return adminModel.AdminName;
            }
            return null;
        }

        /// <summary>
        ///  获取管理员的SysID
        /// </summary>
        /// <returns></returns>
        public static string GetAdminSysID()
        {
            DB.Model.M_AdminUser adminModel = (DB.Model.M_AdminUser)HttpContext.Current.Session[SESSION_ADMIN];
            if (adminModel != null)
            {
                return adminModel.AdminID;
            }
            return null;
        }

        /// <summary>
        /// 获取用户模块权限==写入session中的是(模块，与相应的模块页面)
        /// </summary>
        /// <returns></returns>
        public static List<DB.Model.M_Modal> GetModalAuth()
        {
            List<DB.Model.M_Modal> dicModalAuth = (List<DB.Model.M_Modal>)HttpContext.Current.Session[SESSION_AUTH];//判断权限模块对应表是否在session中已经存在
            if (dicModalAuth == null|| dicModalAuth.Count<=0)//如果不存在，从数据库获取相关信息存入session中，同时返回这个字典内容
            {
                DB.Model.M_AdminUser adminModel = (DB.Model.M_AdminUser)HttpContext.Current.Session[SESSION_ADMIN];//获取存在session中的用户信息，检索时会用到当前登录用户的用户SysID
                if (adminModel != null)//不为空，即已经登录
                {
                    dicModalAuth = new DB.BLL.MB_Bll(DBEnum.Slave).GetModalList(adminModel.AdminID);//获取用户模块权限表的数据
                    HttpContext.Current.Session[SESSION_AUTH] = dicModalAuth;//对这个session进行赋值
                }
            }
            return dicModalAuth;//同时返回这个datatable类型变量
        }

        /// <summary>
        /// 返回当前账户隶属于那家供应商
        /// </summary>
        /// <returns></returns>
        public static string GetAdminSupplier()
        {
            DB.Model.M_AdminUser adminModel = (DB.Model.M_AdminUser)HttpContext.Current.Session[SESSION_ADMIN];
            if (adminModel != null)
            {
                return Convert.ToString(adminModel.SuppliersID);
            }
            return null;
        }


    }
}