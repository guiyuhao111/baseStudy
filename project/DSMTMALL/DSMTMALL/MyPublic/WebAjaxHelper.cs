using Dapper;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.DB.Model;
using DSMTMALL.XFTAEAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace DSMTMALL.MyPublic
{
    public class WebAjaxHelper
    {
        /// <summary>
        /// 管理员做登录
        /// </summary>
        /// <param name="adminName">用户名</param>
        /// <param name="adminPwd">密码</param>
        //public void UserLogin(string adminName, string adminPwd)
        //{
        //    M_Users UserInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Users>(" Phone=@Phone  ", new { Phone = adminName});//返回获取数据的整个字段信息
        //    //M_Users UserInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Users>(" Phone=@Phone AND Password =@Password ", new { Phone = adminName, Password = new MD5Helper().Encrypt(adminPwd) });//返回获取数据的整个字段信息
        //    if (UserInfo != null && UserInfo.IsEnable == 1 && UserInfo.MistakeNum < 5)//返回数据条数大于0，用户是否启用，用户连续输错次数是否小于5条
        //    {
        //        UserInfo.LastLogin = DateTime.Now;//获取当前时间赋值给管理人员表的登录时间，进行更改
        //        UserInfo.MistakeNum = 0;        //将连续输错密码次数设为0

        //        if (new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Users>(" UserID=@_UserID ", new { LastLogin = UserInfo.LastLogin, MistakeNum = UserInfo.MistakeNum, _UserID = UserInfo.UserID }))//进行数据库更新，如果更新成功，则设置session，将最新的数据库该字段信息赋值给session，并输出“SUCCESS+管理员权限”
        //        {
        //            HttpContext.Current.Session[WebLoginHelper.SESSION_ADMIN] = UserInfo;
        //            HttpContext.Current.Response.Write("SUCCESS");
        //        }
        //        else
        //        {
        //            HttpContext.Current.Response.Write("提示：系统忙，请重试");
        //        }
        //    }
        //    else
        //    {
        //        M_Users err_UserInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Users>("Phone=@Phone", new { Phone = adminName });
        //        if (err_UserInfo != null)                        //判断是否有数据，数据条数大于0即存在这个数据字段
        //        {
        //            if (err_UserInfo.IsEnable != 1)              //判断这个用户是否被禁用了，只有1才是可以使用的
        //            {
        //                HttpContext.Current.Response.Write("提示：该账号已禁用");
        //            }
        //            else if (err_UserInfo.MistakeNum >= 5)      //判断用户的连续输错次数是否大于等于5次了
        //            {
        //                HttpContext.Current.Response.Write("提示：账号密码错误次数过多，已禁止登录");
        //            }
        //            else
        //            {
        //                err_UserInfo.MistakeNum += 1;           //输错次数自动加1，同时更新数据库数据，如果数据更新失败，直接返回登录失败，否则再进行情况判断
        //                if (err_UserInfo.MistakeNum > 5)
        //                {
        //                    err_UserInfo.IsEnable = 0;
        //                }
        //                if (new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Users>(" UserID=@_UserID  ", new { IsEnable = err_UserInfo.IsEnable, MistakeNum = err_UserInfo.MistakeNum, _UserID = err_UserInfo.UserID }))
        //                {
        //                    if (err_UserInfo.MistakeNum <= 4)
        //                    {
        //                        HttpContext.Current.Response.Write("提示：密码错误，剩余尝试次数为" + (5 - err_UserInfo.MistakeNum) + "次");
        //                    }
        //                    else
        //                    {
        //                        HttpContext.Current.Response.Write("提示：该账号密码输错次数过多，已禁止登录");
        //                    }
        //                }
        //                else
        //                {
        //                    HttpContext.Current.Response.Write("提示：系统忙，请重试");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            HttpContext.Current.Response.Write("用户名不存在");
        //        }
        //    }
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();//强制结束本页即cs页接下来的代码，但会去执行前端aspx页的代码
        //}
        
        /// <summary>
        /// 用户从微信跳转到商城--从COOKIE
        /// </summary>
        /// <param name="decrypeOpenID"></param>
        /// <param name="timestamp"></param>
        public void Trun_Index()
        {
            string resWrite = "false";
            string openID = GetOpenID();
            if (!string.IsNullOrEmpty(openID))
            {
                try
                {
                    M_Users userInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Users>("OpenID=@_OpenID", new { _OpenID = openID });
                    if (userInfo != null)//用户信息不为空的情况下
                    {
                        new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Users>(" UserID= @_UserID   ", new { LastLogin = DateTime.Now, _UserID = userInfo.UserID });////将微信请求过来的时间戳记录在用户表中//更新失败也让他登陆
                        HttpContext.Current.Session[WebLoginHelper.SESSION_ADMIN] = userInfo;
                        resWrite = "success";
                    }
                    else
                    {
                        resWrite = "nofinduser";
                    }
                }
                catch(Exception e) { FileHelper.logger.Debug(e.Message+e.ToString()); }
            }
            if (resWrite == "false") { FileHelper.logger.Error("登录验证时获取openID失败"); }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 获取微信的OPENID
        /// </summary>
        /// <returns></returns>
        private string GetOpenID()
        {
            string imgUrl = string.Empty;
            string openid = string.Empty;
            string access_token = string.Empty;
            string errcode = string.Empty;
            string errmsg = string.Empty;
            string code = Convert.ToString(HttpContext.Current.Session[WebLoginHelper.SESSION_TOKEN_CODE]);
            Dictionary<string, string> resDic = WeChatAPI.GetWeChatAPI(code);
            if (resDic != null && !resDic.TryGetValue("errcode", out errcode))
            {
                resDic.TryGetValue("openid", out openid);
                HttpContext.Current.Session[WebLoginHelper.SESSION_OPENID] = new DESHelper().Encrypt(openid);//直接进行赋值
                resDic.TryGetValue("access_token", out access_token);
                if (!string.IsNullOrEmpty(openid) && !string.IsNullOrEmpty(access_token))
                {
                    imgUrl = WeChatAPI.GetWeChatAPIUserImg(access_token, openid);
                }
                HttpContext.Current.Session[WebLoginHelper.SESSION_TKOEN_IMG] = imgUrl;
            }
            else
            {
                if (resDic != null) { resDic.TryGetValue("errmsg", out errmsg); }
                FileHelper.logger.Info(ToolHelper.IPAddress() + ":" + "获取用户信息失败;微信返回错误码:" + errcode + ";错误描述:" + errmsg);
            }
            return openid;
        }
        #region 用户的操作

        #region 注释掉的方法
        ///// <summary>
        ///// 用户验证码登陆--现在的模式只要是微信登陆新的微信必须新注册一个账号，所以暂时弃用该方法
        ///// </summary>
        ///// <param name="phone"></param>
        ///// <param name="code"></param>
        //public void UserLoginByCode(string phone, string code, string xfPhone, string openID)
        //{
        //    string resWrite = "提示：登陆失败";
        //    DicQueryHelper dicQueryHelper = new DicQueryHelper();
        //    if (dicQueryHelper.TelphoneModalCount(phone, code))
        //    {
        //        DB.Model.M_Users userInfo = dicQueryHelper.TelphoneUserModal(phone);
        //        if (userInfo != null)
        //        {
        //            DESHelper desHelper = new DESHelper();
        //            try
        //            {
        //                openID = new DESHelper(KeyHelper.wxDesIv, KeyHelper.wxDesKey).Decrypt(openID);//解密openID
        //                //if (!string.IsNullOrEmpty(xfPhone))
        //                //{
        //                //    xfPhone = desHelper.Decrypt(xfPhone);
        //                //    if (xfPhone != userInfo.Phone)
        //                //    {
        //                //        if (dicQueryHelper.TelphoneUserCount(xfPhone)<=0)
        //                //        {
        //                //            userInfo.Phone = xfPhone;
        //                //        }
        //                //    }
        //                //    //userInfo.Phone =xfPhone;
        //                //}
        //                userInfo.OpenID = openID;
        //                userInfo.LastTime = DateTime.Now;
        //                new DB.BLL.M_Users(DBEnum.Master).Update(userInfo);//更新用户信息
        //                HttpContext.Current.Session[WebLoginHelper.SESSION_ADMIN] = userInfo;
        //                resWrite = "SUCCESS";
        //            }
        //            catch
        //            {
        //                resWrite = "提示：非法请求";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        resWrite = "提示：验证码错误";
        //    }
        //    HttpContext.Current.Response.Write(resWrite);
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}

        ///// <summary>
        ///// 用户解绑手机--暂时不允许解绑手机
        ///// </summary>
        //public void UnwrapPhone()
        //{
        //    string resWrite = "UNLOGIN";
        //    string userID = WebLoginHelper.GetUserID();
        //    if (!string.IsNullOrEmpty(userID))
        //    {
        //        DB.Model.M_Users userInfo = new DB.BLL.M_Users(DBEnum.Slave).GetModel(userID);
        //        if (userInfo != null)
        //        {
        //            userInfo.Phone = string.Empty;
        //            if (new DB.BLL.M_Users(DBEnum.Master).Update(userInfo))
        //            { resWrite = "SUCCESS"; HttpContext.Current.Session[WebLoginHelper.SESSION_ADMIN] = userInfo; }
        //            else { resWrite = "提示:系统忙，请稍后再试"; }
        //        }
        //        else
        //        {
        //            resWrite = "提示：系统忙，请稍后再试";
        //        }
        //    }
        //    HttpContext.Current.Response.Write(resWrite);
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}
        #endregion

        /// <summary>
        /// 用户绑定手机
        /// </summary>
        /// <param name="phone">XF系统返回的用户手机</param>
        /// <param name="phone_1">用户输入的手机号码</param>
        /// <param name="openID">用户的OPENID自行加密</param>
        /// <param name="code">短信验证码</param>
        /// <param name="jsonInfo">XF系统返回的用户信息</param>
        public void SubmitBindingInfo(string phone, string phone_1, string code, string jsonInfo)
        {
            string resWrite = "提示：绑定失败";
            DateTime nowTime = DateTime.Now;
            DESHelper desHelper = new DESHelper();
            string decrypeOpenID =  Convert.ToString(HttpContext.Current.Session[WebLoginHelper.SESSION_OPENID]);
            string tempStr = string.Empty;
            BindingInfo backInfo = new BindingInfo();
            if (!string.IsNullOrEmpty(decrypeOpenID))
            {
                try
                {
                    decrypeOpenID = new DESHelper().Decrypt(decrypeOpenID);//解密openID
                    if (!string.IsNullOrEmpty(phone))
                    {
                        phone_1 = desHelper.Decrypt(phone);//解密用户输入的手机
                    }
                    jsonInfo = string.IsNullOrEmpty(jsonInfo) ? string.Empty: desHelper.Decrypt(jsonInfo);//对json进行解密，先判断是否为空，如果为空设空，如果不为空，则进行解密
                    backInfo= !string.IsNullOrEmpty(jsonInfo) ?  new JavaScriptSerializer().Deserialize<BindingInfo>(jsonInfo) : backInfo;//如果json不为空，则吧解密后的json进行反序列化
                }
                catch
                {
                    FileHelper.logger.Info("用户注册时openID或手机号码解密出错");
                    resWrite = "提示：非法请求";
                }
                if (MetarnetRegex.IsMobilePhone(phone_1))//校验手机号码
                {
                    if (WebToolHelper.TelphoneModalCount(phone_1,code,out tempStr)) //手机短信验证通过
                    {
                        M_Users userInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Users>(" OpenID=@_OpenID ", new { _OpenID = decrypeOpenID });  //根据OPENID判断用户是否已经注册
                        if (userInfo == null)//未注册
                        {
                            userInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Users>(" Phone=@_Telphone ", new { _Telphone = phone_1 });
                            if (userInfo == null)//查看当前手机号码是否已存在账户
                            {
                                if (new DB.BLL.MY_Bll(DBEnum.Master).BingUserAccount(phone_1, decrypeOpenID, new MD5Helper().Encrypt(code), backInfo))
                                {
                                    resWrite = ExceptionNotes.SystemSuccess;
                                    userInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Users>(" OpenID=@_OpenID ", new { _OpenID = decrypeOpenID });
                                    if (userInfo != null) { HttpContext.Current.Session[WebLoginHelper.SESSION_ADMIN] = userInfo; }
                                } else
                                {
                                    resWrite = "系统访问量过大，服务暂时失去响应";
                                }
                            } else
                            {
                                resWrite = "提示：绑定失败，该手机号码已有绑定的账户";
                            }
                        }else
                        {
                            resWrite = "提示：系统检测到您已注册，请关闭浏览器重新打开";
                        }
                    } else
                    {
                        resWrite = string.IsNullOrEmpty(tempStr) ? "短信校验失败，请重新获取" :tempStr ;
                    }
                }
                else
                {
                    resWrite = "系统访问量过大，注册手机号码获取失败";
                }
            }
            else
            {
                FileHelper.logger.Info("用户注册时从Cookie中获取openID失败");
                resWrite = "系统访问量过大，服务暂时失去响应";
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 用户换绑手机
        /// </summary>
        /// <param name="phone"></param>
        public void ChangePhone(string phone)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            string userID = WebLoginHelper.GetUserID();
            if (!string.IsNullOrEmpty(userID))
            {
                if (MetarnetRegex.IsMobilePhone(phone))
                {
                    M_Users userInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Users>(" Phone=@_Phone ", new { _Phone = phone });
                    if (userInfo != null)
                    {
                        resWrite= (userInfo.UserID == userID) ? ExceptionNotes.SystemSuccess: "提示：换绑失败，当前手机号已有绑定账户";
                    }
                    else
                    {
                        if(new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Users>(" UserID = @_UserID ", new { Phone = phone, _UserID = userID }))
                        {
                            M_Users userInfo_ID = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Users>(" UserID = @_UserID ",new { _UserID=userID });
                            if (userInfo_ID != null) { HttpContext.Current.Session[WebLoginHelper.SESSION_ADMIN] = userInfo_ID; }
                            resWrite = ExceptionNotes.SystemSuccess;
                        }else { resWrite = ExceptionNotes.SystemBusy; }
                    }
                }
                else
                {
                    resWrite = "提示：请输入正确的手机格式";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 用户换绑昵称
        /// </summary>
        /// <param name="nickName"></param>
        public void ChangeNikeName(string nickName)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            string userID = WebLoginHelper.GetUserID();
            if (!string.IsNullOrEmpty(userID))
            {
                if (new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Users>(" UserID=@_UserID" , new { _UserID= userID, NickName = nickName }))
                {
                    M_Users userInfo_ID = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Users>(" UserID = @_UserID ", new { _UserID = userID });
                    if (userInfo_ID != null) { HttpContext.Current.Session[WebLoginHelper.SESSION_ADMIN] = userInfo_ID; }
                    resWrite = ExceptionNotes.SystemSuccess;
                }else
                {
                    resWrite = ExceptionNotes.SystemBusy;
                }
            }
            EndResponseToWrite(resWrite);
        }

        #endregion

        #region 验证码发送

        /// <summary>
        /// 绑定时向用户发送手机验证码
        /// </summary>
        /// <param name="phone">XF系统返回的手机号-自加密</param>
        /// <param name="phone_1">自己输入的手机号码</param>
        public void SubmitCode(string phone, string phone_1)
        {
            string resWrite = "提示：不是合法的手机号码";
            if (!string.IsNullOrEmpty(phone))//加密手机号不为空的情况下解密手机号
            {
                try
                {
                    phone = new DESHelper().Decrypt(phone);//如果加密失败，说明是非法请求
                }
                catch { resWrite = ExceptionNotes.SystemUnLogin; }
                M_Users userInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Users>(" Phone=@_Phone",new { _Phone= phone } );
                if (userInfo==null)
                {
                    resWrite = WebToolHelper.GetTelphoneCode(phone, "您正在绑定51IPC购物平台账户");
                }
                else
                {
                    resWrite = "提示:该手机号码已被绑定，如有疑问请联系管理员";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(phone_1) && MetarnetRegex.IsMobilePhone(phone_1))
                {
                    M_Users userInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Users>(" Phone=@_Phone", new { _Phone = phone_1 });
                    if (userInfo==null)
                    {
                        resWrite = WebToolHelper.GetTelphoneCode(phone_1, "您正在绑定51IPC购物平台账户");
                    }
                    else
                    {
                        resWrite = "提示:该手机号码已被绑定，如有疑问请联系管理员";
                    }
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 登陆时发送验证码到用户手机
        /// </summary>
        /// <param name="phone">手机号码</param>
        public void SubmitCodeForLogin(string phone)
        {
            string resWrite = WebToolHelper.GetTelphoneCode(phone, "您正在绑定51IPC购物平台账户");
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 发送短信验证码进行订单支付
        /// </summary>
        public void SetOrderCode()
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            string telphone = WebLoginHelper.GetUserPhone();
            if (!string.IsNullOrEmpty(telphone))
            {
                resWrite = WebToolHelper.GetTelphoneCode(telphone, "您正在51IPC购物平台进行消费");
            }
            else
            {
                resWrite = "提示：您当前账户下没有绑定的手机号码";
            }
            EndResponseToWrite(resWrite);
        }

        #endregion

        #region 获取列表集

        /// <summary>
        /// 首页获取热销产品列表
        /// </summary>
        /// <param name="pageNow"></param>
        /// <param name="pageSize"></param>
        public void GetGoodsListByIndexPage(string pageNow, string pageSize)
        {
            int outPageNow = ToolHelper.ConventToInt32(pageNow, 1);
            int outPageSize = ToolHelper.ConventToInt32(pageSize, 16);
            string url = WebToolHelper.GetProfilesUrl();
            string hotTopList = string.Empty;
            object goodsInfo = null;
            if (outPageNow == 1)//如果是第一页
            {
                if(HttpRuntime.Cache["hotTopList"] == null)//判断缓存是否存在
                {
                    goodsInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetGoodsListByIndexPage(new { PathAdd = url, StartSize = (outPageNow - 1) * outPageSize, EndSize = outPageSize });
                    hotTopList = new JavaScriptSerializer().Serialize(goodsInfo);
                    HttpRuntime.Cache.Insert("hotTopList", hotTopList, null, DateTime.Now.AddSeconds(600), System.Web.Caching.Cache.NoSlidingExpiration);  //这里给数据加缓存，设置缓存时间
                }
                else
                {
                    hotTopList =Convert.ToString( HttpRuntime.Cache["hotTopList"]);
                }
                
            }else//如果不是第一页
            {
                 goodsInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetGoodsListByIndexPage(new { PathAdd = url, StartSize = (outPageNow - 1) * outPageSize, EndSize = outPageSize });
                 hotTopList = new JavaScriptSerializer().Serialize(goodsInfo);
            }
            HttpContext.Current.Response.Write(hotTopList);
            HttpContext.Current.ApplicationInstance.CompleteRequest();//强制结束本页即cs页接下来的代码，但会去执行前端aspx页的代码
        }

        /// <summary>
        /// 获取首页公司的商品信息列表
        /// </summary>
        public void GetGoodsListByCpyIndexPage()
        {
            string cpySysID = WebLoginHelper.GetUserCpySysID();
            string resWrite = string.Empty;
            string url = WebToolHelper.GetProfilesUrl();
            if (!string.IsNullOrEmpty(cpySysID))
            {
                if (HttpRuntime.Cache[cpySysID] == null)
                {
                    object cpyGoodsInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetGoodsListByCpyIndexPage(new { PathAdd = url, StartSize = 0, EndSize = 60, CpySysID = cpySysID }).Select(info => new { info.GoodsID, info.NGoodsImg, info.GoodsName, info.ShopPrice, info.MarketPrice, info.GoodsNumber });
                    resWrite = new JavaScriptSerializer().Serialize(cpyGoodsInfo);
                    HttpRuntime.Cache.Insert(cpySysID, resWrite, null,DateTime.Now.AddSeconds(600), System.Web.Caching.Cache.NoSlidingExpiration);
                }else
                {
                    resWrite = Convert.ToString(HttpRuntime.Cache[cpySysID]);
                }
            }
            EndResponseToWrite(resWrite);
        }
        
        /// <summary>
        /// 获取个人中心页上新商品显示
        /// </summary>
        public void GetNewRecomGoodsList()
        {
            string resWrite = string.Empty;
            string url = WebToolHelper.GetProfilesUrl();
            if (!string.IsNullOrEmpty("newGoodsInfo"))
            {
                if (HttpRuntime.Cache["newGoodsInfo"] == null)
                {
                    object newGoodsInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetGoodsListByHomePage(new { PathAdd = url, StartSize = 0, EndSize = 8 });
                    resWrite = new JavaScriptSerializer().Serialize(newGoodsInfo);
                    HttpRuntime.Cache.Insert("newGoodsInfo", resWrite, null, DateTime.Now.AddSeconds(600), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                else
                {
                    resWrite = Convert.ToString(HttpRuntime.Cache["newGoodsInfo"]);
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 获取首页轮播图信息
        /// </summary>
        public void GetRollInfoList()
        {
            string resWrite = string.Empty;
            string url = WebToolHelper.GetProfilesUrl();
            var rollInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModelList("SELECT R.*,CONCAT(@PathAdd,R.Picture) AS NPicture FROM M_Roll AS R WHERE R.IsEnable =1 ORDER BY R.OrderBy ASC", new { PathAdd = url }).Select(info => { return new { info.NPicture, info.TargetSysID, info.TargetUrl }; });
            resWrite = new JavaScriptSerializer().Serialize(rollInfo);
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 商品显示页获取商品列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sort"></param>
        /// <param name="pageNow"></param>
        /// <param name="pageSize"></param>
        public void GetGoodsListByPage(string type, string sort, string pageNow, string pageSize, string searchName)
        {
            int outType = ToolHelper.ConventToInt32(type, 0); int outSort = ToolHelper.ConventToInt32(sort, 0); int outPageNow = ToolHelper.ConventToInt32(pageNow, 1); int outPageSize = ToolHelper.ConventToInt32(pageSize, 16);
            string url = WebToolHelper.GetProfilesUrl();
            object goodsInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetGoodsListByPage(url, outType, searchName, outSort, (outPageNow - 1) * outPageSize, outPageSize);
            HttpContext.Current.Response.Write(new JavaScriptSerializer().Serialize(goodsInfo));
            HttpContext.Current.ApplicationInstance.CompleteRequest();//强制结束本页即cs页接下来的代码，但会去执行前端aspx页的代码
        }

        /// <summary>
        /// 获取用户的订单列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageNow"></param>
        /// <param name="pageSize"></param>
        public void GetUserOrderList(string type, string pageNow, string pageSize)
        {
            string userSysID = WebLoginHelper.GetUserID();
            int outPageNow = ToolHelper.ConventToInt32(pageNow, 0); int outPageSize = ToolHelper.ConventToInt32(pageSize, 6);
            if (!string.IsNullOrEmpty(userSysID))
            {
                object orderInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetUserOrderList(userSysID, type, (outPageNow - 1) * outPageSize, outPageSize);
                HttpContext.Current.Response.Write(new JavaScriptSerializer().Serialize(orderInfo));
            }
            else
            {
                HttpContext.Current.Response.Write(ExceptionNotes.SystemUnLogin);
            }
            HttpContext.Current.ApplicationInstance.CompleteRequest();//强制结束本页即cs页接下来的代码，但会去执行前端aspx页的代码
        }

        /// <summary>
        /// 获取用户的订单的商品信息
        /// </summary>
        /// <param name="orderID"></param>
        public void GetUserOrderGoods(string orderID)
        {
            string userSysID = WebLoginHelper.GetUserID();
            string url = WebToolHelper.GetProfilesUrl();
            if (!string.IsNullOrEmpty(userSysID))
            {
                if (!string.IsNullOrEmpty(orderID))//根据订单编号获取该订单编号下的所有商品ID
                {
                    object orderGoodsInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetUserOrderGoods(url, orderID);
                    HttpContext.Current.Response.Write(new JavaScriptSerializer().Serialize(orderGoodsInfo));
                }
            }
            else
            {
                HttpContext.Current.Response.Write(ExceptionNotes.SystemUnLogin);
            }
            HttpContext.Current.ApplicationInstance.CompleteRequest();//强制结束本页即cs页接下来的代码，但会去执行前端aspx页的代码
        }

        /// <summary>
        /// 获取用户购物车列表
        /// </summary>
        /// <param name="userID"></param>
        public void GetUserCartList(string userID)
        {
            string url = WebToolHelper.GetProfilesUrl();
            object cartInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetUserCartList(url, userID);
            EndResponseToWrite(new JavaScriptSerializer().Serialize(cartInfo));
        }

        /// <summary>
        /// 获取用户进行提交的购物车商品
        /// </summary>
        /// <param name="userID"></param>
        public void GetUserCartListToSubmitOrder( string[] cartIDList,string provienceID)
        {
            string userID = WebLoginHelper.GetUserID();
            string url = WebToolHelper.GetProfilesUrl();
            List<GoodsCartToOrder> goodsCartInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetUserCartLsitToOrder(url, userID, cartIDList,provienceID);
            double goodsFareTempMoney = WebToolHelper.GetGoodsFareTempMoney(goodsCartInfo);
            object resGoodsCartInfo = goodsCartInfo.Select(info => new { info.BuyNumber,info.CartID,info.GoodsName,info.NGoodsImg,info.ShopPrice,info.MarketPrice,info.SaleNumber,info.GoodsID,info.IsDelete,info.IsEnable });
            EndResponseToWrite(new JavaScriptSerializer().Serialize(resGoodsCartInfo)+"&"+goodsFareTempMoney);
        }

        /// <summary>
        /// 获取当前选择的用户物流信息
        /// </summary>
        public void GetUserAddress(string id)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            string userID = WebLoginHelper.GetUserID();
            if (!string.IsNullOrEmpty(userID))
            {
                UserAddressToOrder resAddress = null;
                if (!string.IsNullOrEmpty(id))
                {
                    resAddress = new DB.BLL.MY_Bll(DBEnum.Slave).GetUserAddressInfo<UserAddressToOrder>(userID,ToolHelper.ConventToInt32(id,0));
                }else
                {
                    List<UserAddressToOrder> obj = new DB.BLL.MY_Bll(DBEnum.Slave).GetUserAddressInfo<UserAddressToOrder>(userID);
                    resAddress = (obj != null && obj.Count > 0) ? obj[0] : null;
                }
                if (resAddress != null)
                {
                    resWrite = resAddress.AddressID + "|" + resAddress.Consignee + "|" + resAddress.Mobile + "|" + resAddress.ProvinceName + resAddress.CityName + resAddress.DistrictName + resAddress.Address + "|" + resAddress.Province;
                }
                else
                {
                    resWrite = "请填写收货地址";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 获取用户的配送地址列表集合
        /// </summary>
        public void GetUserAddressList()
        {
            string userID = WebLoginHelper.GetUserID();
            List<UserAddressToOrder> dt = new DB.BLL.MY_Bll(DBEnum.Slave).GetUserAddressInfo<UserAddressToOrder>(userID);
            EndResponseToWrite(new JavaScriptSerializer().Serialize(dt));
        }

        /// <summary>
        /// 获取当前订单用户的地址信息
        /// </summary>
        /// <param name="id"></param>
        public void GetChoiceAddress(string id)
        {
            string userID = WebLoginHelper.GetUserID();
            UserAddressToOrder dt = new DB.BLL.MY_Bll(DBEnum.Slave).GetUserAddressInfo<UserAddressToOrder>(userID, ToolHelper.ConventToInt32(id));
            EndResponseToWrite(new JavaScriptSerializer().Serialize(dt));
        }

        #endregion
        
        #region 购物车操作

        /// <summary>
        /// 将商品加入购物车
        /// </summary>
        /// <param name="goodsID"></param>
        /// <param name="number"></param>
        public void AddGoodsToCart(string goodsID, string number)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            string userID = WebLoginHelper.GetUserID();
            int outNumber = ToolHelper.ConventToInt32(number, 1);
            if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(goodsID))
            {
                M_Goods goodsInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Goods>(" GoodsID =@GoodsID AND IsEnable=1 AND IsDelete=0 ", new { GoodsID = goodsID });//获取商品商品信息---从三层取-启用未删除
                if (goodsInfo != null)
                {
                    M_Cart cartInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Cart>(" UserID=@UserID AND GoodsID=@GoodsID ", new { UserID = userID, GoodsID = goodsID });
                    if (cartInfo != null)
                    {
                        cartInfo.BuyNumber += outNumber;
                        if (goodsInfo.QuotaNumber > 0)//商品限购判断
                        {
                            if (cartInfo.BuyNumber <= goodsInfo.QuotaNumber)
                            {
                                resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Cart>(" CartID=@_CartID ", new { BuyNumber = cartInfo.BuyNumber, _CartID = cartInfo.CartID }) ? ExceptionNotes.SystemSuccess : "ERROR,添加购物车失败,系统访问量过大";//如果已存在则加上这个数量
                            }else
                            {
                                resWrite = "ERROR,该商品的限购数量为" + goodsInfo.QuotaNumber + "件";
                            }
                        }else
                        {
                            resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Cart>(" CartID=@_CartID ", new { BuyNumber = cartInfo.BuyNumber, _CartID = cartInfo.CartID }) ? ExceptionNotes.SystemSuccess : "ERROR,添加购物车失败,系统访问量过大";//如果已存在则加上这个数量
                        }
                    }
                    else
                    {
                        cartInfo = new M_Cart();
                        cartInfo.UserID = userID;
                        cartInfo.GoodsID = goodsInfo.GoodsID;
                        cartInfo.GoodsName = goodsInfo.GoodsName;
                        cartInfo.GoodsSn = goodsInfo.GoodsSn;
                        cartInfo.BuyNumber = outNumber;
                        cartInfo.CanHandler = 1;
                        resWrite = new DB.BLL.MY_Bll(DBEnum.Master).AddModel(cartInfo) ? ExceptionNotes.SystemSuccess : "ERROR,添加购物车失败,系统访问量过大";//如果不存在则创建这条记录
                    }
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更新购物车商品数量
        /// </summary>
        /// <param name="goodsID"></param>
        /// <param name="type"></param>
        public void ChangeCartNumber(string cartID, string type)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            int outCartID = 0;
            string userID = WebLoginHelper.GetUserID();
            if (!string.IsNullOrEmpty(userID) && int.TryParse(cartID, out outCartID))
            {
                M_Cart cartInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Cart>(" CartID=@CartID ", new { UserID = userID, CartID = outCartID });
                if (cartInfo != null)
                {
                    int resNum = cartInfo.BuyNumber;
                    if (type == "add")
                    {
                        cartInfo.BuyNumber++;
                    }else
                    {
                        cartInfo.BuyNumber--;
                    }
                    resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Cart>(" CartID=@_CartID ", new { BuyNumber = cartInfo.BuyNumber, _CartID = cartInfo.CartID }) ? cartInfo.BuyNumber.ToString() : resNum.ToString();
                }else
                {
                    resWrite = "ERROR,购物车商品不存在，请刷新页面后再试";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更改购物车商品数量input输入
        /// </summary>
        /// <param name="cartID"></param>
        /// <param name="number"></param>
        public void ChangeCartNumbers(string cartID,string number)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            int outCartID = 0;
            int outNumber = 0;
            string userID = WebLoginHelper.GetUserID();
            if (!string.IsNullOrEmpty(userID) && int.TryParse(cartID, out outCartID))
            {
                int.TryParse(number, out outNumber);
                if (outNumber != 0)
                {
                    M_Cart cartInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Cart>(" CartID=@CartID ", new { UserID = userID, CartID = outCartID });
                    if (cartInfo != null)
                    {
                        int resNum = cartInfo.BuyNumber;
                        cartInfo.BuyNumber = outNumber;
                        resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Cart>(" CartID=@_CartID ", new { BuyNumber = cartInfo.BuyNumber, _CartID = cartInfo.CartID }) ? cartInfo.BuyNumber.ToString() : resNum.ToString();
                    }
                    else
                    {
                        resWrite = "ERROR,购物车商品不存在，请刷新页面后再试";
                    }
                }else
                {
                    resWrite = "ERROR,请输入大于0的正整数";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 删除购物车信息
        /// </summary>
        /// <param name="cartID"></param>
        public void DeleteUserCart(string cartID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            string userID = WebLoginHelper.GetUserID();
            int outCartID = ToolHelper.ConventToInt32(cartID, 0);
            if (!string.IsNullOrEmpty(userID))//是否登陆
            {
                M_Cart cartInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Cart>(" CartID=@CartID AND UserID =@UserID ", new { UserID = userID, CartID = cartID });
                if (cartID != null)//购物车信息是否存在
                {
                    resWrite = new DB.BLL.MY_Bll(DBEnum.Master).DeleteModel<M_Cart>(" CartID=@CartID AND UserID =@UserID ", new { UserID = userID, CartID = cartID }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
                }
                else
                {
                    resWrite = ExceptionNotes.SystemBusy;
                }
            }
            HttpContext.Current.Response.Write(resWrite);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion

        #region 订单的操作

        ///// <summary>
        ///// 提交用户订单的帮助类，用于计算提交的商品的总运费信息
        ///// </summary>
        ///// <param name="cartIDList"></param>
        ///// <param name="addressID"></param>
        ///// <returns></returns>
        //private double AddUserOrderHelperToGetFareTempMoney(string[] cartIDList,string addressID)
        //{
        //    string userID = WebLoginHelper.GetUserID();
        //    string url = WebToolHelper.GetProfilesUrl();
        //    M_UserAddress addressInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_UserAddress>(" AddressID=@_AddressID ",new { _AddressID =addressID});
        //    string provienceID = addressInfo != null ? Convert.ToString(addressInfo.Province) : string.Empty;
        //    List<GoodsCartToOrder> goodsCartInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetUserCartLsitToOrder(url, userID, cartIDList, provienceID);
        //    return WebToolHelper.GetGoodsFareTempMoney(goodsCartInfo);
        //}

        /// <summary>
        /// 提交用户订单的帮助类，用于计算提交的商品的各供应商分别的总运费信息
        /// </summary>
        /// <param name="cartIDList"></param>
        /// <param name="addressID"></param>
        /// <returns></returns>
        private Dictionary<int, double> AddUserOrderHelperToGetSubFareTempMoney(string[] cartIDList, string addressID,out List<GoodsCartToOrder> cartToOrderList)
        {
            string userID = WebLoginHelper.GetUserID();
            string url = WebToolHelper.GetProfilesUrl();
            M_UserAddress addressInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_UserAddress>(" AddressID=@_AddressID ", new { _AddressID = addressID });
            string provienceID = addressInfo != null ? Convert.ToString(addressInfo.Province) : string.Empty;
            List<GoodsCartToOrder> goodsCartInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetUserCartLsitToOrder(url, userID, cartIDList, provienceID);
            cartToOrderList = goodsCartInfo;
            return WebToolHelper.GetGoodsSubFareTempMoney(goodsCartInfo);
        }

        /// <summary>
        /// 提交用户订单的帮助类，用户判断用户提交的商品的购买数量今日是否已达商品限购数量
        /// </summary>
        /// <param name="cartIDList"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        private bool CheckGoodsListIsPassQuotaNumber(string[] cartIDList,string userID,out string resInfo)
        {
            resInfo = string.Empty;
            //先获取当前购物车所有的商品信息包括限购数量  //商品id,商品购买数量，商品限购数量
            List<CartGoodsNumber> cartGoodsNumber = new DB.BLL.MY_Bll(DBEnum.Slave).GetCartGoodsNumberInfo(cartIDList,userID);
            DateTime startTime =Convert.ToDateTime( DateTime.Now.ToString("yy-MM-dd 00:00:00"));
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string nowGoodsNum = string.Empty;
            //在获取当前用户今天所有订单（已付款、未付款）的购买商品总数
            List<UserTodayBuyOrderGoods> userTodayBuyOrderGoods = new DB.BLL.MY_Bll(DBEnum.Slave).GetUserTodayBuyOrderGoodsInfo(userID, startTime);
            if (userTodayBuyOrderGoods != null && userTodayBuyOrderGoods.Count > 0) { //讲订单商品信息转化为字典数据
                dic = ToolHelper.IEnumerableListToDic(userTodayBuyOrderGoods, (x, y) =>
                {
                    foreach (var item in x)
                    {
                        if (!y.ContainsKey(Convert.ToString(item.GoodsID)))
                        {
                            y.Add(Convert.ToString(item.GoodsID), Convert.ToString(item.SUMBuyNumber));
                        }
                    }
                });
            }
            if (cartGoodsNumber != null && cartGoodsNumber.Count > 0)
            {
                for (int i = 0; i < cartGoodsNumber.Count; i++)//遍历购物车商品数据，判断每一个商品的限购数量-要购买的数量-已购买的数量是否小于0，如果小于0说明已达今日购买上限，返回flase不允许购买
                {
                    nowGoodsNum = string.Empty;
                    dic.TryGetValue(cartGoodsNumber[i].GoodsID, out nowGoodsNum);
                    if (cartGoodsNumber[i].QuotaNumber>0 && ((cartGoodsNumber[i].QuotaNumber - cartGoodsNumber[i].BuyNumber - ToolHelper.ConventToInt32(nowGoodsNum, 0))<0))
                    {
                        resInfo = "购物车中的限购商品"+cartGoodsNumber[i].GoodsName+"今日购买上限为"+cartGoodsNumber[i].QuotaNumber+"件";
                        return false;
                    }else if(cartGoodsNumber[i].GoodsNumber<cartGoodsNumber[i].BuyNumber)
                    {
                        resInfo = "购物车中的商品"+cartGoodsNumber[i].GoodsName+"剩余库存"+cartGoodsNumber[i].GoodsNumber+"件";
                        return false;
                    }else if (cartGoodsNumber[i].BuyNumber <=0)
                    {
                        resInfo = "购物车中的商品" + cartGoodsNumber[i].GoodsName + "购买数量不能为0";
                        return false;
                    }
                }
                return true;//遍历完所有的购物车商品数据，都OK得情况下返回true
            }else
            {
                return false;//获取购物车商品信息数据失败，直接返回false
            }

        }
        
        /// <summary>
        /// 提交用户订单
        /// </summary>
        /// <param name="addressID">配送地址</param>
        /// <param name="payMonery">商品总金额</param>
        public void AddUserOrder(string addressID, string payMonery, string shippingID, string payID, string[] cartIDList)
        {
            string resWrite = "UNLOGIN";
            string userID = WebLoginHelper.GetUserID();
            string url = WebToolHelper.GetProfilesUrl();
            int outAddressID; int outPayID; decimal outpayMonery;
            string resInfo = string.Empty;
            List<GoodsCartToOrder> dt;
            if (!string.IsNullOrEmpty(userID))
            {
                if (!string.IsNullOrEmpty(addressID) && int.TryParse(addressID, out outAddressID) && int.TryParse(payID, out outPayID) && decimal.TryParse(payMonery, out outpayMonery)&&cartIDList.Length>0)
                {
                    if (CheckGoodsListIsPassQuotaNumber(cartIDList, userID, out resInfo))
                    {
                        resWrite = string.Empty;
                        string outErrInfo = string.Empty;
                        UserAddressToOrder addressInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetUserAddressInfo<UserAddressToOrder>(userID, outAddressID);
                        M_Payment paymentInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Payment>(" PayID=@PayID ", new { PayID = ToolHelper.ConventToInt32(payID, 0) });
                        Dictionary<int, double> feeAmount = AddUserOrderHelperToGetSubFareTempMoney(cartIDList, addressID, out dt);//获取用户需要支付的邮费
                        if (addressInfo != null && paymentInfo != null && feeAmount != null)
                        {
                            try
                            {
                                if (dt != null && dt.Count > 0)
                                {
                                    string orderUnifySn = WebToolHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString();
                                    Random rd = new Random();
                                    int num = rd.Next(100000, 1000000);
                                    orderUnifySn += num.ToString();
                                    //对订单进行拆分，按照供应商来源的不同，拆分成不同的子订单，由统一下单ID进行下单,统一下单ID就是传入的orderSn, 子订单ID就是在这个orderSn后面加上供应商数字ID
                                    List<string> orderSnList = new List<string>();
                                    List<M_Suppliers> shippingList = new DB.BLL.MY_Bll(DBEnum.Slave).GetModelList<M_Suppliers>(string.Empty, null);
                                    for (int j = 0; j < dt.Count; j++)
                                    {
                                        foreach (var item in shippingList)
                                        {
                                            if (item.SuppliersID == dt[j].SuppliersID && !orderSnList.Contains(orderUnifySn + item.SuppliersID))
                                            {
                                                orderSnList.Add(orderUnifySn + item.SuppliersID);
                                                break;
                                            }
                                        }
                                    }
                                    if (orderSnList != null && orderSnList.Count > 0)
                                    {
                                        string feeAmountPrice = new WebToolHelper().GetProfilesEncryptInfo(FieldName.FeeAmount, "159");
                                        //正式提交执行事务操作
                                        if (new DB.BLL.MY_Bll(DBEnum.Master).SubmitOrderList(dt, orderUnifySn, orderSnList, userID, ToolHelper.ConventToDecimal(payMonery), addressInfo, feeAmount, paymentInfo, out outErrInfo, WebLoginHelper.GetUserOpenID()))
                                        {
                                            resWrite = "SUCCESS," + orderUnifySn;
                                        }
                                        else
                                        {
                                            resWrite = outErrInfo;
                                        }
                                    }
                                    else
                                    {
                                        resWrite = "商品来源出错，请重新提交";
                                    }
                                }
                                else
                                {
                                    resWrite = "订单列表为空";
                                }
                            }
                            catch { resWrite = "系统忙，请重新提交"; }
                        }
                        else
                        {
                            resWrite = "收货地址出错，订单提交失败";
                        }
                    }else
                    {
                        resWrite = string.IsNullOrEmpty(resInfo) ?　"当前系统访问量过大,提交失败":resInfo ;
                    }
                }
                else
                {
                    resWrite = "请选择配送地址";
                }
            }
            HttpContext.Current.Response.Write(resWrite);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// 取消订单支付
        /// </summary>
        /// <param name="orderSn">订单编号</param>
        public void CancelOrder(string orderID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            string userID = WebLoginHelper.GetUserID();
            if (!string.IsNullOrEmpty(userID))
            {
                M_OrderInfo orderInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_OrderInfo>(" OrderID= @_OrderID AND UserID=@_UserID ", new { _OrderID = orderID, _UserID = userID });
                if (orderInfo != null)
                {
                    if (orderInfo.OrderStatus == 0 && orderInfo.PayStatus == 0)//只有当订单状态为未确认，并且支付状态为未付款才允许取消订单
                    {
                        resWrite = new DB.BLL.MY_Bll(DBEnum.Master).CancelOrder(orderID) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
                    }
                    else
                    {
                        resWrite = "提示：当前订单状态不允许取消订单";
                    }
                }
                else
                {
                    resWrite = ExceptionNotes.SystemBusy;
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 用户确认收货
        /// </summary>
        /// <param name="orderID"></param>
        public void ConrirmOrder(string orderID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            string userID = WebLoginHelper.GetUserID();
            if (!string.IsNullOrEmpty(userID))
            {
                M_OrderInfo orderInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_OrderInfo>(" OrderID=@_OrderID ", new { _OrderID = orderID });
                if (orderInfo != null && orderInfo.UserID == userID)
                {
                    if (orderInfo.PayStatus != 9 && orderInfo.ShippingStatus == 1)
                    {
                        if (new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_OrderInfo>(" OrderID=@_OrderID ", new { _OrderID = orderID, OrderStatus = 8, ShippingStatus = 2, ConfirmTime = DateTime.Now }))
                        {
                            resWrite = ExceptionNotes.SystemSuccess;
                            //WeChatAPI.SendMsg("确认收货成功", "您的订单" + orderInfo.OrderSn + "已交易完成，感谢您的购买", WebLoginHelper.GetUserOpenID());
                        }
                        else { resWrite= ExceptionNotes.SystemBusy; }
                       
                    }
                    else
                    {
                        resWrite = "提示：当前订单状态不允许确认收货";
                    }
                }
                else
                {
                    resWrite = ExceptionNotes.SystemBusy;
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 使用卡余额支付订单
        /// </summary>
        /// <param name="id">订单id-json格式</param>
        /// <param name="price">总价格</param>
        /// <param name="code">验证码</param>
        public void SubmitCardBanlance(string id, string price, string code)
        {
            string resWrite =ExceptionNotes.SystemUnLogin;
            string userID = WebLoginHelper.GetUserID();
            string phone = WebLoginHelper.GetUserPhone();
            if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(phone))
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                SQLEntityHelper sqlEntityHelper = new SQLEntityHelper();
                string orderToXFSn = string.Empty;
                decimal orderAmount = 0;//订单总金额
                string pushType = string.Empty;
                string pushOrderSn = string.Empty;
                decimal fareTempMonry = 0;
                string tempStr = string.Empty;
                DateTime orderCreateTime = DateTime.Now;
                //1.校验短信验证码是否通过
                if(WebToolHelper.TelphoneModalCount(phone, code, out tempStr))
                {
                    //验证码校验通过，在算一次钱，订单ID拆分//如果是统一订单支付时没有子订单号的，如果有子订单号，说明是子订单支付
                    dictionary = JsonHelper.JsonToDictionary<Dictionary<string, string>>(id);
                    if (!string.IsNullOrEmpty(dictionary["OrderSn"]) && string.IsNullOrEmpty(dictionary["OrderUntifySn"]))//子订单号付款
                    {
                        orderAmount = sqlEntityHelper.GetOrderInfoAmountByOrderSn(dictionary["OrderSn"],out fareTempMonry,out orderCreateTime);
                        pushType = "orderSn";
                        pushOrderSn = dictionary["OrderSn"];
                    }
                    else if (!string.IsNullOrEmpty(dictionary["OrderUntifySn"]) && string.IsNullOrEmpty(dictionary["OrderSn"]))//统一下单付款
                    {
                        orderAmount = sqlEntityHelper.GetOrderInfoAmountByOrderUnifySn(dictionary["OrderUntifySn"],out fareTempMonry, out orderCreateTime);
                        pushType = "unifySn";
                        pushOrderSn = dictionary["OrderUntifySn"];
                    }
                    if (!string.IsNullOrEmpty(pushType) && !string.IsNullOrEmpty(pushOrderSn))
                    {
                        if (orderAmount == Convert.ToDecimal(price))//后台算出来的价格与前台传过来的价格一致的情况下
                        {
                            if (XFServiceAPI.SetUserOrderToPayment(pushType, pushOrderSn, orderAmount,orderCreateTime, out tempStr)) {
                                resWrite = ExceptionNotes.SystemSuccess;//执行成功设置resWrite为SUCCESS；
                                //WeChatAPI.SendMsg("提交付款成功", "您的订单" + pushOrderSn + "已成功提交付款申请，此次付款金额为："+orderAmount+",请等待付款审核确认",WebLoginHelper.GetUserOpenID());
                            }else
                            {
                                resWrite =!string.IsNullOrEmpty(tempStr)? tempStr: "系统忙的没边了，请等下嘛" ;
                            }
                        }else
                        {
                            resWrite = "提示：资金对账出错";
                        }
                    }else
                    {
                        resWrite = "提示：结算出错，请重试";
                    }
                }else
                {
                    resWrite =!string.IsNullOrEmpty(tempStr) ? tempStr :"我了个去，大神，系统都被您搞崩溃了哇";
                }
            }
            HttpContext.Current.Response.Write(resWrite);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        #endregion

        #region 地址的操作

        ///// <summary>
        ///// 设置选择配送的收货地址
        ///// </summary>
        ///// <param name="addressID"></param>
        //public void SetFirstUserAddress(string addressID)
        //{
        //    string resWrite = ExceptionNotes.SystemUnLogin;
        //    string userID = WebLoginHelper.GetUserID();
        //    if (!string.IsNullOrEmpty(userID))//现在仅仅是重新将是否默认地址改为1 ，最终靠是否是最后更新时间来作判断
        //    {
        //        resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_UserAddress>(" UserID=@_UserID AND AddressID=@_AddressID ", new { _UserID = userID, _AddressID = ToolHelper.ConventToInt32(addressID, 0), AddressFirst = 1, UpdateTime = DateTime.Now }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
        //    }
        //    EndResponseToWrite(resWrite);
        //}

        /// <summary>
        /// 收货地址编辑选择城市
        /// </summary>
        /// <param name="sysID"></param>
        public void ChoseChildAddress(string sysID)
        {
            IEnumerable<dynamic> addressInfo = (IEnumerable<dynamic>)DataCache.GetCache("RegionList");
            if (addressInfo == null)
            {
                addressInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModelList(" SELECT RegionName ,RegionID,ParentID  FROM M_Region ", null);
                DataCache.SetCache("RegionList", addressInfo);
            }
            object obj = addressInfo.Where(lan => { if (lan.ParentID == ToolHelper.ConventToInt32(sysID, 1)) { return true; } else { return false; } }).Select(info => { return new { info.RegionID, info.RegionName }; });
            EndResponseToWrite(new JavaScriptSerializer().Serialize(obj));
        }

        /// <summary>
        /// 用户添加收货地址
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="district"></param>
        /// <param name="address"></param>
        /// <param name="mobile"></param>
        /// <param name="consignee"></param>
        public void AddUserAddress(string province, string city, string district, string address, string mobile, string consignee, string cardNo)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            string userID = WebLoginHelper.GetUserID();
            if (ToolHelper.IsCardNoIsPass(cardNo))
            {
                if (!string.IsNullOrEmpty(userID))
                {
                    M_UserAddress userAddressInfo = new M_UserAddress();
                    userAddressInfo.AddressFirst = 0;
                    userAddressInfo.UserID = userID;
                    userAddressInfo.Consignee = consignee;
                    userAddressInfo.Country = 1;
                    userAddressInfo.City = ToolHelper.ConventToInt32(city, 0);
                    userAddressInfo.Province = ToolHelper.ConventToInt32(province, 0);
                    userAddressInfo.District = ToolHelper.ConventToInt32(district, 0);
                    userAddressInfo.Address = address;
                    userAddressInfo.Mobile = mobile;
                    userAddressInfo.ConsigneeCardNo = cardNo;
                    userAddressInfo.UpdateTime = DateTime.Now;
                    resWrite = new DB.BLL.MY_Bll(DBEnum.Master).AddModel(userAddressInfo) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
                }
            }else
            {
                resWrite = "无效的身份证号码会导致海淘无法从海关进行发货";
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 用户更改收货地址
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="district"></param>
        /// <param name="address"></param>
        /// <param name="mobile"></param>
        /// <param name="consignee"></param>
        public void UpdateUserAddress(string addressID, string province, string city, string district, string address, string mobile, string consignee, string cardNo)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            string userID = WebLoginHelper.GetUserID();
            if (ToolHelper.IsCardNoIsPass(cardNo))
            {
                if (!string.IsNullOrEmpty(userID))
                {
                    resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_UserAddress>(" UserID=@_UserID AND AddressID=@_AddressID ", new
                    {
                        _UserID = userID,
                        _AddressID = addressID,
                        AddressFirst = 0,
                        Consignee = consignee,
                        Country = 1,
                        City = ToolHelper.ConventToInt32(city, 0),
                        Province = ToolHelper.ConventToInt32(province, 0),
                        District = ToolHelper.ConventToInt32(district, 0),
                        Address = address,
                        Mobile = mobile,
                        ConsigneeCardNo = cardNo,
                        UpdateTime = DateTime.Now
                    }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
                }
            }else
            {
                resWrite = "无效的身份证号码会导致海淘无法从海关进行发货";
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 删除用户的收货地址
        /// </summary>
        /// <param name="addressID"></param>
        public void DeleteUserAddress(string addressID)
        {
            string resWrite = "UNLOGIN";
            string userID = WebLoginHelper.GetUserID();
            if (!string.IsNullOrEmpty(userID))
            {
                resWrite = new DB.BLL.MY_Bll(DBEnum.Master).DeleteModel<M_UserAddress>(" AddressID=@AddressID " ,new { AddressID=ToolHelper.ConventToInt32(addressID,0)}) ? ExceptionNotes.SystemSuccess :ExceptionNotes.SystemBusy; 
            }
            EndResponseToWrite(resWrite);
        }

        #endregion
        
        /// <summary>
        /// 获取子类的类目列表
        /// </summary>
        /// <param name="type">父类类目ID</param>
        public void GetSubCategoryList(string type)
        {
            string path = WebToolHelper.GetProfilesUrl();
            string strSql = "SELECT *,CONCAT(@PathAdd,ShowImage) AS NShowImage  FROM M_Category WHERE ParentID =@ParentID AND IsDelete =0 AND IsEnable =1 ORDER BY OrderBy ASC";
            object cateInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModelList(strSql, new { PathAdd = path, ParentID = ToolHelper.ConventToInt32(type, 84) }).Select(info => { return new { info.CateName, info.NShowImage, info.CateID }; });
            string resWrite = new JavaScriptSerializer().Serialize(cateInfo);
            HttpContext.Current.Response.Write(resWrite);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// 最终输出数据返回前端
        /// </summary>
        /// <param name="resWrite">返回的数据</param>
        public void EndResponseToWrite(string resWrite)
        {
            HttpContext.Current.Response.Write(resWrite);
            HttpContext.Current.ApplicationInstance.CompleteRequest();//强制结束本页即cs页接下来的代码，但会去执行前端aspx页的代码
        }
    }
}
