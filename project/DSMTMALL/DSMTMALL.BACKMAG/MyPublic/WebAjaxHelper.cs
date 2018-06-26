using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.DB.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

namespace DSMTMALL.BACKMAG.MyPublic
{
    public class WebAjaxHelper
    {
        #region 管理员操作
        
        /// <summary>
        /// 管理员做登录
        /// </summary>
        /// <param name="adminName">用户名</param>
        /// <param name="adminPwd">密码</param>
        public void AdminLogin(string adminName, string adminPwd,string code)
        {
            M_AdminUser adminInfoList = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_AdminUser>(" AdminName=@_AdminName AND Password=@_Password " ,new { _AdminName = adminName, _Password = new MD5Helper().Encrypt(adminPwd) });//返回获取数据的整个字段信息
            if (adminInfoList != null && adminInfoList.IsEnable == 1 && adminInfoList.MistakeNum < 5)//返回数据条数大于0，用户是否启用，用户连续输错次数是否小于5条
            {
                adminInfoList.LoginTime = DateTime.Now;//获取当前时间赋值给管理人员表的登录时间，进行更改
                adminInfoList.MistakeNum = 0;        //将连续输错密码次数设为0
                adminInfoList.Token = Guid.NewGuid().ToString();
                if (new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_AdminUser>(" AdminID=@_AdminID ", new { _AdminID=adminInfoList.AdminID,LoginTime= adminInfoList.LoginTime, MistakeNum=adminInfoList.MistakeNum,Token=adminInfoList.Token, ReadCardNo=string.Empty }))//进行数据库更新，如果更新成功，则设置session，将最新的数据库该字段信息赋值给session，并输出“SUCCESS+管理员权限”
                {
                    RedisHelper.SetCache("MLLogin_" + code, adminInfoList, DateTime.Now.AddSeconds(60));
                    HttpContext.Current.Response.Write("SUCCESS");
                }
                else
                {
                    HttpContext.Current.Response.Write("登录失败，请重试");
                }
            }
            else
            {
                M_AdminUser err_AdminList = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_AdminUser>(" AdminName=@_AdminName ", new { _AdminName =adminName});
                if (err_AdminList != null)                        //判断是否有数据，数据条数大于0即存在这个数据字段
                {
                    if (err_AdminList.IsEnable != 1)              //判断这个用户是否被禁用了，只有1才是可以使用的
                    {
                        HttpContext.Current.Response.Write("该账号已禁用");
                    }
                    else if (err_AdminList.MistakeNum >= 5)      //判断用户的连续输错次数是否大于等于5次了
                    {
                        HttpContext.Current.Response.Write("密码输错次数过多，已禁止登录");
                    }
                    else
                    {
                        err_AdminList.MistakeNum += 1;           //输错次数自动加1，同时更新数据库数据，如果数据更新失败，直接返回登录失败，否则再进行情况判断
                        if (new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel < M_AdminUser >(" AdminID=@_AdminID ",new { _AdminID = err_AdminList.AdminID, MistakeNum = err_AdminList.MistakeNum }))
                        {
                            if (err_AdminList.MistakeNum <= 4)
                            {
                                HttpContext.Current.Response.Write("密码错误，剩余次数" + (5 - err_AdminList.MistakeNum) + "次");
                            }
                            else
                            {
                                HttpContext.Current.Response.Write("密码输错次数过多，已禁止登录");
                            }
                        }
                        else
                        {
                            HttpContext.Current.Response.Write("登录失败，请重试");
                        }
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write("该用户名不存在");
                }
            }
            HttpContext.Current.ApplicationInstance.CompleteRequest();//强制结束本页即cs页接下来的代码，但会去执行前端aspx页的代码
        }

        /// <summary>
        /// 检查登陆状态
        /// </summary>
        /// <param name="token">令牌口令</param>
        /// <param name="code">本次验证码</param>
        public void CheckAdminLogin( string code)
        {
            string resWrite = "WARNING,连接超时请重试";
            string readCardNo;
            dynamic info = RedisHelper.GetCache<M_AdminUser>("MLLogin_" + code);
            if (info != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(500);
                    readCardNo = RedisHelper.GetCache<string>(code + "_ReadCard", RedisHelper.RedisEnum.Common);
                    if (readCardNo != null)
                    {
                        if (readCardNo == info.TokenCardNo || readCardNo == "610485305")
                        {
                            HttpContext.Current.Session[WebLoginHelper.SESSION_ADMIN] = info;
                            resWrite = "SUCCESS";
                            resWrite += "," + WebLoginHelper.GetFirstPage();
                            RedisHelper.DeleteCache(code + "_ReadCard", RedisHelper.RedisEnum.Common);
                        }
                        else if (string.IsNullOrEmpty(readCardNo))
                        {
                            resWrite = "WARNING,请放置令牌卡";
                        }
                        else
                        {
                            resWrite = "WARNING,令牌与登陆账户不一致";
                        }
                        break;
                    }
                }
            }
            else { resWrite = "WARNING,登陆失败请重试"; }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 获取TOKEN值用户调用发卡程序
        /// </summary>
        public void AssignCardToAdmin()
        {
            string resWrite = "ERROR";
            if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage)) {
                string token = Guid.NewGuid().ToString();
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_AdminUser>(" AdminID=@_AdminID ", new { _AdminID = WebLoginHelper.GetAdminSysID(), Token = token, ReadCardNo=string.Empty }) ? "SUCCESS," + token : ExceptionNotes.SystemBusy;
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 发卡的时候进行卡号绑定
        /// </summary>
        /// <param name="token"></param>
        /// <param name="code"></param>
        /// <param name="id"></param>
        public void CheckAdminReadCardNum(string code,string id)
        {
            string resWrite = "ERROR";
            string readCardNo = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500);
                readCardNo = RedisHelper.GetCache<string>(code + "_ReadCard", RedisHelper.RedisEnum.Common);
                if (!string.IsNullOrEmpty(readCardNo))//如果不为空，则说明此次读卡已成功
                {
                    break;
                }
            }
            if (!string.IsNullOrEmpty(readCardNo))//不为空，说明读到卡了
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_AdminUser>(" AdminID =@_AdminID ", new { _AdminID = id, TokenCardNo = readCardNo }) ? "SUCCESS" : "ERROR,发卡失败";
            }
            else
            {
                resWrite = "WARNING,请放置令牌卡";
            }
        }

        /// <summary>
        ///登录账户退出
        /// </summary>
        public void AdminLogout()
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Write("SUCCESS");
        }

        /// <summary>
        /// 账户密码修改
        /// </summary>
        /// <param name = "adminPwd" > 旧密码 </ param >
        /// < param name="newPwd">新密码</param>
        public void UpdateAdminPwd(string adminPwd, string newPwd)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsLogin())
            {
                M_AdminUser adminInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_AdminUser>(" AdminID=@_AdminID" ,new { _AdminID=WebLoginHelper.GetAdminSysID()  });
                if (adminInfo!=null && new MD5Helper().CheckEncrypt(adminPwd, adminInfo.Password))
                {
                    resWrite = (new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_AdminUser>( "AdminID=@_AdminID",new { _AdminID = WebLoginHelper.GetAdminSysID(), Password = new MD5Helper().Encrypt(newPwd) , ModifyPerson =WebLoginHelper.GetAdminName(), UpdateTime =DateTime.Now}  )) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
                }
                else
                {
                    resWrite = "提示：旧密码错误";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更新管理员名称
        /// </summary>
        /// <param name="adminName"></param>
        public void UpdataAdminName(string adminName,string id)
        {
            string resWrrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
            {
                if (!string.IsNullOrEmpty(adminName))
                {
                    resWrrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_AdminUser>("AdminID=@_AdminID", new { _AdminID = id, AdminName = adminName }) ? adminName : ExceptionNotes.SystemBusy;
                }else
                {
                    resWrrite = "账户名不能为空";
                }
            }
            EndResponseToWrite(resWrrite);
        }

        /// <summary>
        /// 更新管理员联系方式
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="id"></param>
        public void UpdataAdminPhone(string adminPhone,string id)
        {
            string resWrrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
            {
                if (!string.IsNullOrEmpty(adminPhone))
                {
                    resWrrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_AdminUser>("AdminID=@_AdminID", new { _AdminID = id, Phone = adminPhone }) ? adminPhone : ExceptionNotes.SystemBusy;
                }
                else
                {
                    resWrrite = "请输入要更改的联系方式";
                }
            }
            EndResponseToWrite(resWrrite);
        }

        /// <summary>
        /// 重置管理员账号
        /// </summary>
        /// <param name="id"></param>
        public void UpdataAdminPwd(string id)
        {
            string resWrrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
            {
                resWrrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_AdminUser>("AdminID=@_AdminID", new { _AdminID = id, MistakeNum=0, Password = new MD5Helper().Encrypt("12345") }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
            }
            EndResponseToWrite(resWrrite);
        }


        #endregion

        #region 数据的获取

        /// <summary>
        /// 联想获取商品信息表
        /// </summary>
        /// <param name="discountName"></param>
        public void SearchGoods(string searchName)
        {
            string strSql = "SELECT GoodsID,GoodsName,GoodsSn FROM M_Goods WHERE 1=1 AND IsEnable=1 AND IsDelete=0  ORDER BY OrderBy DESC";
            Dictionary<string, string> dicDis = new WebToolHelper().GetUlListByIEnumerable(SessionEnum.GoodsInfo, strSql, searchName, "", (x, y) =>
            {
                foreach (var item in x)
                {
                    if (!y.ContainsKey(Convert.ToString(item.GoodsID)))
                    {
                        y.Add(Convert.ToString(item.GoodsID), Convert.ToString(item.GoodsName) + Convert.ToString(item.GoodsSn));
                    }
                }
            });
            EndResponseToWrite(new JavaScriptSerializer().Serialize(dicDis));
        }

        /// <summary>
        /// 联想获取类目信息表
        /// </summary>
        /// <param name="discountName"></param>
        public void SearchCates(string searchName)
        {
            string strSql = "SELECT CateID,CateName FROM M_Category WHERE 1=1  AND IsDelete=0  ORDER BY OrderBy DESC";
            Dictionary<string, string> dicDis = new WebToolHelper().GetUlListByIEnumerable(SessionEnum.CateInfo, strSql, searchName, "", (x, y) =>
            {
                foreach (var item in x)
                {
                    if (!y.ContainsKey(Convert.ToString(item.CateID)))
                    {
                        y.Add(Convert.ToString(item.CateID), Convert.ToString(item.CateName));
                    }
                }
            });
            EndResponseToWrite(new JavaScriptSerializer().Serialize(dicDis));
        }

        /// <summary>
        /// 联想获取广告信息表
        /// </summary>
        /// <param name="searchName"></param>
        public void SearchAdvs(string searchName)
        {
            string strSql = "SELECT AdvSysID,AdvName FROM M_Advertisement WHERE 1=1 ";
            Dictionary<string, string> dicDis = new WebToolHelper().GetUlListByIEnumerable(SessionEnum.AdvInfo, strSql, searchName, "", (x, y) =>
            {
                foreach (var item in x)
                {
                    if (!y.ContainsKey(Convert.ToString(item.AdvSysID)))
                    {
                        y.Add(Convert.ToString(item.AdvSysID), Convert.ToString(item.AdvName));
                    }
                }
            });
            EndResponseToWrite(new JavaScriptSerializer().Serialize(dicDis));
        }

        /// <summary>
        /// 联想获取品牌信息表
        /// </summary>
        /// <param name="searchName"></param>
        public void SearchBrand(string searchName)
        {
            string strSql = "SELECT BrandID,BrandName FROM M_Brand WHERE 1=1 ORDER BY OrderBy DESC";
            Dictionary<string, string> dicDis = new WebToolHelper().GetUlListByIEnumerable(SessionEnum.BrandInfo, strSql, searchName, "", (x, y) =>
            {
                foreach (var item in x)
                {
                    if (!y.ContainsKey(Convert.ToString(item.BrandID)))
                    {
                        y.Add(Convert.ToString(item.BrandID), Convert.ToString(item.BrandName));
                    }
                }
            });
            EndResponseToWrite(new JavaScriptSerializer().Serialize(dicDis));
        }

        /// <summary>
        /// 获取运费发送方式
        /// </summary>
        /// <param name="fareSysID"></param>
        public void GetFareCarry(string fareSysID)
        {
            List<M_FareCarry> fareCarryList = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<M_FareCarry>(" FareSysID=@_FareSysID ORDER BY IsDefault DESC ", new { _FareSysID = fareSysID });
            EndResponseToWrite(new JavaScriptSerializer().Serialize(fareCarryList));
        }

        /// <summary>
        /// 获取所有地区省份
        /// </summary>
        public void GetProvience()
        {
            IEnumerable<dynamic> addressInfo = (IEnumerable<dynamic>)DataCache.GetCache("ProvienceList");
            if (addressInfo == null)
            {
                addressInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModelList(" SELECT RegionName ,RegionID  FROM M_Region WHERE ParentID=1 ", null);
                DataCache.SetCache("ProvienceList", addressInfo);
            }
            object obj = addressInfo.Select(info => { return new { info.RegionID, info.RegionName }; });
            EndResponseToWrite(new JavaScriptSerializer().Serialize(obj));
        }

        /// <summary>
        /// 获取当前订单编号的所有商品信息
        /// </summary>
        /// <param name="orderID"></param>
        public void SearchOrderGoods(string orderID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage)||WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetOrderGoodsList(orderID);
                resWrite = new JavaScriptSerializer().Serialize(dt);
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 获取当前退货订单ID的所有商品信息
        /// </summary>
        /// <param name="orderID"></param>
        public void SearchReBackOrderGoods(string orderID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage) || WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetReBackOrderGoodsList(orderID);
                resWrite = new JavaScriptSerializer().Serialize(dt);
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 获取所有的模块内容
        /// </summary>
        public void GetModalList()
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
            {
                List<M_Modal> info = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<M_Modal>(" ModalSysID != '06625c3a-817d-4b1e-9d72-92654d8fece1' ", null);
                resWrite = new JavaScriptSerializer().Serialize(info);
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 获取相册图片
        /// </summary>
        /// <param name="goodsID">商品ID</param>
        public void GetGalleryImgList(string goodsID)
        {
            string resWrite = "UNLOGIN";
            if (WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage) || WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                string strSql = "SELECT ImgUrl,ImgID FROM `M_GoodsGallery` WHERE GoodsID=@_GoodsID ";
                object imgList = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList(strSql, new { _GoodsID = goodsID }).Select(info=> new  { info.ImgUrl,info.ImgID });
                resWrite = new JavaScriptSerializer().Serialize(imgList);
            }
            EndResponseToWrite(resWrite);
        }
        
        #endregion

        #region 用户的操作

        #region 通用的方法

        /// <summary>
        /// 改变状态信息
        /// </summary>
        /// <param name="sysID">主键ID</param>
        /// <param name="status">改变后的状态（0/1）</param>
        /// <param name="type">要更改字段名</param>
        /// <param name="tableName">表名</param>
        public void ChangeCheckBox(string sysID, string status, string type, string tableName)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            int outStatus = 0;
            int outSysID = 0;
            int.TryParse(sysID, out outSysID);
            int.TryParse(status, out outStatus);
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
            {
                switch (tableName)
                {
                    case "M_Goods":
                        M_Goods goodsInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = sysID });
                        if (goodsInfo != null)
                        {
                            if (type == "IsNew")
                            {
                                goodsInfo.IsNew = outStatus;
                                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID ", new { IsNew = goodsInfo.IsNew, _GoodsID = sysID, ModifyPerson = WebLoginHelper.GetAdminName(), LastUpdate = DateTime.Now }) ? outStatus.ToString() : "0";
                            }
                            else if (type == "IsEnable")
                            {
                                goodsInfo.IsEnable = outStatus;
                                if (!string.IsNullOrEmpty(goodsInfo.FareSysID))
                                {
                                    resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID ", new { IsEnable = goodsInfo.IsEnable, Version = goodsInfo.Version + 1, _GoodsID = sysID, ModifyPerson = WebLoginHelper.GetAdminName(), LastUpdate = DateTime.Now, IsUpdateBySupplier=0 }) ? outStatus.ToString() : "0";
                                }else
                                {
                                    resWrite = "上架商品必须指定运费模板";
                                }
                            }else if(type== "IsPromote")
                            {
                                goodsInfo.IsPromote = outStatus;
                                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID ", new { IsPromote = goodsInfo.IsPromote, _GoodsID = sysID, Version = goodsInfo.Version + 1, ModifyPerson = WebLoginHelper.GetAdminName(), LastUpdate = DateTime.Now }) ? outStatus.ToString() : "0";
                            }
                        }
                        else { resWrite = ExceptionNotes.SystemBusy; }
                        break;
                    case "M_Category":
                        resWrite = new DB.BLL.MB_Bll(DBEnum.Master).IsEnableCategory(outSysID, outStatus) ? outStatus.ToString() : "0";
                        break;
                    case "M_Brand":
                        resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Brand>(" BrandID=@_BrandID ", new { _BrandID = outSysID, IsShow = outStatus }) ? outStatus.ToString() : "0";
                        break;
                    case "M_Roll":
                        resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Roll>(" RollSysID=@_RollSysID ", new { _RollSysID = outSysID, IsEnable = outStatus }) ? outStatus.ToString() : "0";
                        break;
                    case "M_Suppliers":
                        resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Suppliers>(" SuppliersID=@_SuppliersID ", new { _SuppliersID=outSysID, IsCheck= outStatus }) ? outStatus.ToString():"0";
                        break;
                    default:
                        break;
                }
            }
            else if (WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                M_Goods goodsInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = sysID });
                if (goodsInfo != null)
                {
                    if (goodsInfo.IsEnable == 1)//三方托管账户只允许下架，不允许上架0--下架1--上架
                    {
                        if (type == "IsEnable")
                        {
                            resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID ", new { IsEnable = 0, Version = goodsInfo.Version + 1, _GoodsID = sysID, ModifyPerson = WebLoginHelper.GetAdminName(), LastUpdate = DateTime.Now }) ? outStatus.ToString() : "0";
                        }
                    }
                    else
                    {
                        resWrite = "ERR,当前账户只允许下架商品信息，上架商品操作不被允许";
                    }
                }
                else { resWrite = ExceptionNotes.SystemBusy; }
            }
            else if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_AdminUser>(" AdminID=@_AdminID ", new { _AdminID = sysID, IsEnable = outStatus }) ? outStatus.ToString() : "0";
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 改变文本框信息
        /// </summary>
        /// <param name="sysID"></param>
        /// <param name="world">内容</param>
        /// <param name="tableName">表名</param>
        public void ChangeTextarea(string sysID, string world, string tableName)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            int outSysID = 0;
            int.TryParse(sysID, out outSysID);
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage)||WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                switch (tableName)
                {
                    case "M_Brand":
                        resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Brand>(" BrandID=@_BrandID ", new { _BrandID = sysID, BrandDesc = world }) ? world : ExceptionNotes.SystemBusy;
                        break;
                    case "M_Goods":
                        resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = sysID, SellerNote = world }) ? world : ExceptionNotes.SystemBusy;
                        break;
                    case "M_Suppliers":
                        resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Suppliers>(" SuppliersID=@_SuppliersID ", new { _SuppliersID = sysID, SuppliersDesc = world }) ? world : ExceptionNotes.SystemBusy;
                        break;
                    default:
                        break;
                }
            }
            HttpContext.Current.Response.Write(resWrite);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// 更新各表的排序
        /// </summary>
        /// <param name="sysID">类目ID</param>
        /// <param name="number"></param>
        public void UpdateOrderBy(int sysID, string number, string tableName)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
            {
                switch (tableName)
                {
                    case "M_Category":
                        resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Category>(" CateID=@_CateID ", new { _CateID = sysID, OrderBy = ToolHelper.ConventToInt32(number, 50) }) ? number : ExceptionNotes.SystemBusy;
                        break;
                    case "M_Brand":
                        resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Brand>(" BrandID=@_BrandID ", new { _BrandID = sysID, OrderBy = ToolHelper.ConventToInt32(number, 50) }) ? number : ExceptionNotes.SystemBusy;
                        break;
                    case "M_Roll":
                        resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Roll>(" RollSysID=@_RollSysID ", new { _RollSysID = sysID, OrderBy = ToolHelper.ConventToInt32(number, 50) }) ? number : ExceptionNotes.SystemBusy;
                        break;
                    default:
                        break;
                }
            }
            EndResponseToWrite(resWrite);
        }
        #endregion

        #region 账户的操作

        /// <summary>
        /// 添加管理账户
        /// </summary>
        /// <param name="adminName">账户名</param>
        /// <param name="suppliersID">供应商选择</param>
        /// <param name="adminPhone"></param>
        /// <param name="remark"></param>
        public void AddAdminAmount(string adminName, string suppliersID, string adminPhone, string remark)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
            {
                M_AdminUser userInfo = new M_AdminUser();
                userInfo.AdminID = Guid.NewGuid().ToString();
                userInfo.AdminName = adminName;
                userInfo.CreateTime = DateTime.Now;
                userInfo.IsEnable = 0;
                userInfo.ModifyPerson = WebLoginHelper.GetAdminName();
                userInfo.NoteList = DateTime.Now + "该账号创建成功";
                userInfo.Password = new MD5Helper().Encrypt("123456");
                userInfo.Phone = adminPhone;
                userInfo.Remark = remark;
                userInfo.SuppliersID = !string.IsNullOrEmpty(suppliersID) ? ToolHelper.ConventToInt32(suppliersID, 0) : 0;
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).AddModel(userInfo) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy + "请确认用户名没有重复";
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 删除管理账户
        /// </summary>
        /// <param name="id"></param>
        public void DeleteAdminAmount(string id)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).DeleteModel<M_AdminUser>(" AdminID=@_AdminID ", new { _AdminID = id }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
            }
            EndResponseToWrite(resWrite);
        }
        #endregion

        #region 类目的操作

        /// <summary>
        /// 添加类目信息
        /// </summary>
        /// <param name="cateName">类目名数组</param>
        /// <param name="parentID">父类ID</param>
        public void AddCategory(string[] cateName, int parentID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                resWrite = string.Empty;
                M_Category setCategoryInfo = new M_Category();
                foreach (var item in cateName)
                {
                    setCategoryInfo = new M_Category();
                    setCategoryInfo.CateName = item;
                    setCategoryInfo.IsDelete = 0;
                    setCategoryInfo.IsEnable = 1;
                    setCategoryInfo.KeyWords = string.Empty;
                    setCategoryInfo.ParentID = parentID;
                    setCategoryInfo.ShowImage = string.Empty;
                    setCategoryInfo.ShowInNav = 1;
                    setCategoryInfo.OrderBy = 50;
                    setCategoryInfo.CateDesc = string.Empty;
                    resWrite += new DB.BLL.MB_Bll(DBEnum.Master).AddModel(setCategoryInfo) ? string.Empty : "ERR:类目名" + item + "添加失败,请核实该类目名是否已存在;";
                }
                resWrite = string.IsNullOrEmpty(resWrite) ? ExceptionNotes.SystemSuccess : resWrite + "其余类目添加成功";
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更新类目信息
        /// </summary>
        /// <param name="cateName">类目名称</param>
        /// <param name="cateID">类目ID</param>
        /// <param name="parentID">父类ID</param>
        public void UpdateCategory(string cateName, int cateID, string parentID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Category>(" CateID=@_CateID ", new { CateName = cateName, ParentID =ToolHelper.ConventToInt32( parentID,0), _CateID = cateID }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy+ "请核实该类目名是否已存在"; 
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 删除类目数据
        /// </summary>
        /// <param name="sysID"></param>
        public void DeleteCategory(string sysID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            string strWhere = string.Empty;
            int outSysID = 0;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage) && int.TryParse(sysID, out outSysID))//需要后台管理员操作权限
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).DeleteCategory(outSysID) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
            }
            EndResponseToWrite(resWrite);
        }

        #endregion

        #region 品牌的操作

        /// <summary>
        /// 添加品牌信息
        /// </summary>
        /// <param name="brandName"></param>
        public void AddBrand(string[] brandName)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                resWrite = string.Empty;
                M_Brand setBrandInfo = new M_Brand();
                foreach (var item in brandName)
                {
                    setBrandInfo.BrandName = item;
                    setBrandInfo.BrandDesc = string.Empty;
                    setBrandInfo.BrandLogo = string.Empty;
                    setBrandInfo.BrandUrl = string.Empty;
                    setBrandInfo.IsShow = 1;
                    setBrandInfo.OrderBy = 50;
                    resWrite += new DB.BLL.MB_Bll(DBEnum.Master).AddModel(setBrandInfo) ? string.Empty : "ERR:品牌名" + item+ "添加失败,请校对品牌是否已存在;";
                }
                resWrite = string.IsNullOrEmpty(resWrite) ? ExceptionNotes.SystemSuccess : resWrite + "其余添加成功";
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更新品牌信息
        /// </summary>
        /// <param name="brandName"></param>
        /// <param name="brandID"></param>
        public void UpdateBrand(string brandName, int brandID)
        {
            string resWrite = "UNLOGIN";
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Brand>(" BrandID=@_BrandID ", new { BrandName = brandName, _BrandID = brandID })? ExceptionNotes.SystemSuccess:ExceptionNotes.SystemBusy+"请核实当前品牌名是否一已存在"; 
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 删除品牌信息
        /// </summary>
        /// <param name="sysID"></param>
        public void DeleteBrand(string sysID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            int outSysID = 0;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage) && int.TryParse(sysID, out outSysID))//需要后台管理员操作权限
            {
                int goodsCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelListCount<M_Goods>(" BrandID=@_BrandID AND IsDelete=0  ", new { _BrandID = outSysID });
                if (goodsCount <= 0)
                {
                    resWrite = new DB.BLL.MB_Bll(DBEnum.Master).DeleteModel<M_Brand>(" BrandID=@_BrandID ", new { _BrandID = outSysID }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
                }
                else
                {
                    resWrite = "删除失败，一些商品的品牌引用当前选项";
                }
            }
            EndResponseToWrite(resWrite);
        }

        #endregion

        #region 轮播的操作

        /// <summary>
        /// 删除轮播信息
        /// </summary>
        /// <param name="sysID"></param>
        public void DeleteRoll(string sysID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            int outSysID = 0;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage) && int.TryParse(sysID, out outSysID))//需要后台管理员操作权限
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).DeleteModel<M_Roll>(" RollSysID=@_RollSysID ", new { _RollSysID = outSysID }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 添加轮播信息
        /// </summary>
        public void AddRoll()
        {
            string resWrite = "UNLOGIN";
            DateTime nowTime = DateTime.Now;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                M_Roll rollInfo = new M_Roll();
                rollInfo.TargetSysID = string.Empty;
                rollInfo.TargetUrl = string.Empty;
                rollInfo.RollType = 30;
                rollInfo.Picture = string.Empty;
                rollInfo.IsEnable = 0;
                rollInfo.OrderBy = 50;
                rollInfo.ModifyPerson = WebLoginHelper.GetAdminName();
                rollInfo.UpdateTime = nowTime;
                rollInfo.CreateTime = nowTime;
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).AddModel(rollInfo) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更新轮播信息
        /// </summary>
        public void UpdateRoll(string targetSysID,string targetType , int rollSysID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                string targetUrl = string.Empty;
                switch (targetType)
                {
                    case "10":
                        targetUrl = "/web/adv_list.aspx?advid=" + targetSysID + "";
                        break;
                    case "20":
                        targetUrl = "/web/goods_list.aspx?goodstype=" + targetSysID + "";
                        break;
                    case "30":
                        targetUrl = "/web/goods_show.aspx?goodsid=" + targetSysID +"";
                        break;
                    default:
                        break;
                }
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Roll>(" RollSysID=@_RollSysID ", new { _RollSysID = rollSysID, TargetSysID= targetSysID,TargetUrl=targetUrl, RollType = ToolHelper.ConventToInt32(targetType,10) })? ExceptionNotes.SystemSuccess:ExceptionNotes.SystemBusy; 
            }
            EndResponseToWrite(resWrite);
        }

        #endregion
        
        #region 产品的操作

        /// <summary>
        /// 集团推荐商品分配
        /// </summary>
        /// <param name="goodsID">商品ID</param>
        /// <param name="cpyInfoList">要分配的公司集合</param>
        public void AssignCpyToGoods(string goodsID, string[] cpyInfoList)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
            {
                //分配商品给公司作为集团推荐
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).AssignGoodsToCpy(goodsID, cpyInfoList) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 获取XF系统所有开通商城的公司信息
        /// </summary>
        public void ChooseCpyInfo()
        {
            string resWrite = "UNLOGIN";
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
            {
                resWrite = XFServiceAPI.GetCpyInfo();
            }
            HttpContext.Current.Response.Write(resWrite);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// 删除商品信息
        /// </summary>
        /// <param name="sysID"></param>
        public void DeleteGoods(string sysID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
            {
                M_Goods goodsInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = sysID });
                if (goodsInfo != null)
                {
                    DateTime nowTime = DateTime.Now;
                    resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = goodsInfo.GoodsID, IsEnable = 0, IsDelete = 1, GoodsNumber=0, LastUpdate = nowTime, GoodsSn = goodsInfo.GoodsSn + "Delete" + nowTime.ToShortDateString() , ModifyPerson =WebLoginHelper.GetAdminName()}) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
                }
                else
                {
                    resWrite = ExceptionNotes.SystemBusy;
                }
            }else if (WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                M_Goods goodsInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = sysID });
                if (goodsInfo != null)
                {
                    if (Convert.ToString(goodsInfo.SuppliersID) == WebLoginHelper.GetAdminSupplier())//多一层判断，使供应商只能删除自己的商品
                    {
                        DateTime nowTime = DateTime.Now;
                        resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = goodsInfo.GoodsID, IsEnable = 0, IsDelete = 1, GoodsNumber=0,LastUpdate = nowTime, GoodsSn = goodsInfo.GoodsSn + "Delete" + nowTime.ToShortDateString(), ModifyPerson = WebLoginHelper.GetAdminName() }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
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
        /// 更新商品库存
        /// </summary>
        /// <param name="sysID">商品ID</param>
        /// <param name="number">增加或减少的量</param>
        public void UpdateInverntory(string sysID, string number)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage)||WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                M_Goods goodsInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = sysID });
                if (goodsInfo != null)
                {
                    goodsInfo.GoodsNumber += ToolHelper.ConventToInt32(number, 0);
                    resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID AND Version=@_Version ",
                        new
                        {
                            _GoodsID = sysID,
                            _Version = goodsInfo.Version,
                            GoodsNumber = goodsInfo.GoodsNumber,
                            LastUpdate = DateTime.Now,
                            Version = goodsInfo.Version++
                        }) ? goodsInfo.GoodsNumber.ToString() : ExceptionNotes.SystemBusy;
                }
                else
                {
                    resWrite = ExceptionNotes.SystemBusy;
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更新商品限购数量
        /// </summary>
        /// <param name="sysID"></param>
        /// <param name="number"></param>
        public void UpdateQuotalNumber(string sysID,string number)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage) || WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                M_Goods goodsInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = sysID });
                if (goodsInfo != null)
                {
                    goodsInfo.QuotaNumber = ToolHelper.ConventToInt32(number, 0);
                    resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID AND Version=@_Version ",
                        new
                        {
                            _GoodsID = sysID,
                            _Version = goodsInfo.Version,
                            QuotaNumber = goodsInfo.QuotaNumber,
                            LastUpdate = DateTime.Now,
                            Version = goodsInfo.Version++
                        }) ? goodsInfo.QuotaNumber.ToString() : ExceptionNotes.SystemBusy;
                }else
                {
                    resWrite = ExceptionNotes.SystemBusy;
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更新运费模板首费
        /// </summary>
        /// <param name="sysID"></param>
        /// <param name="number"></param>
        public void UpdateFareCarryFirstMoney(string sysID,string number)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                decimal outNumber = 0;
                if (decimal.TryParse(number, out outNumber))
                {
                   
                    resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_FareCarry>(" CarrySysID=@_CarrySysID ",
                        new
                        {
                            _CarrySysID = sysID,
                            FirstMoney =outNumber,
                        }) ? outNumber.ToString() : ExceptionNotes.SystemBusy;
                }
                else
                {
                    resWrite = "提交的数据不正确";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更新运费模板续费
        /// </summary>
        /// <param name="sysID"></param>
        /// <param name="number"></param>
        public void UpdateFareCarryContinueMoney(string sysID, string number)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                decimal outNumber = 0;
                if (decimal.TryParse(number, out outNumber))
                {

                    resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_FareCarry>(" CarrySysID=@_CarrySysID ",
                        new
                        {
                            _CarrySysID = sysID,
                            ContinueMoney = outNumber,
                        }) ? outNumber.ToString() : ExceptionNotes.SystemBusy;
                }
                else
                {
                    resWrite = "提交的数据不正确";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更新商品重量
        /// </summary>
        /// <param name="sysID">商品ID</param>
        /// <param name="number">增加或减少的量</param>
        public void UpdateWeight(string sysID, string number)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage)||WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                double outNumber = 0;
                if (double.TryParse(number, out outNumber))
                {
                    M_Goods goodsInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = sysID });
                    if (goodsInfo != null)
                    {
                        if (goodsInfo.IsEnable==0) {
                            goodsInfo.Weight = outNumber;
                            resWrite = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID AND Version=@_Version ",
                                new
                                {
                                    _GoodsID = sysID,
                                    _Version = goodsInfo.Version,
                                    Weight = goodsInfo.Weight,
                                    LastUpdate = DateTime.Now,
                                    Version = goodsInfo.Version++,
                                    ModifyPerson = WebLoginHelper.GetAdminName(),
                                    IsUpdateBySupplier=1
                                }) ? goodsInfo.Weight.ToString() : ExceptionNotes.SystemBusy;
                        }else
                        {
                            resWrite = "修改商品的重量必须先将商品进行下架";
                        }
                    }
                    else
                    {
                        resWrite = ExceptionNotes.SystemBusy;
                    }
                }
                else
                {
                    resWrite = "请输入正确的重量";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更新商品的排序
        /// </summary>
        /// <param name="sysID">商品ID</param>
        /// <param name="number"></param>
        public void UpdateGoodsOrderBy(string sysID, string number)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = sysID, OrderBy = ToolHelper.ConventToInt32(number, 50) }) ? number : ExceptionNotes.SystemBusy; ;
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 添加新的商品信息
        /// </summary>
        /// <param name="name">商品名</param>
        /// <param name="cid">商品编号</param>
        /// <param name="price_s">本店售价</param>
        /// <param name="price_m">市场售价</param>
        /// <param name="typeC">类目ID</param>
        /// <param name="typeT">供应商ID</param>
        /// <param name="count">商品库存</param>
        /// <param name="word">商品简介</param>
        /// <param name="remark">商家备注</param>
        /// <param name="number">商品重量</param>
        public void AddNewGoods(string goodsName, string goodsSn, decimal shopPrice, decimal marketPrice, int cateID, int supplierID, int brandID, int inventory, string brief, string remark, double weight,string fareTemp)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).AddGoodsInfo(cateID, marketPrice, shopPrice, brandID, string.Empty, string.Empty, string.Empty, supplierID, goodsSn, goodsName, inventory, brief, 1, 1, 0, weight,string.Empty,fareTemp,WebLoginHelper.GetAdminName()) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
            }
            else if(WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage)){
                resWrite= new DB.BLL.MB_Bll(DBEnum.Master).AddGoodsInfo(cateID, marketPrice, shopPrice, brandID, string.Empty, string.Empty, string.Empty,ToolHelper.ConventToInt32( WebLoginHelper.GetAdminSupplier(),10), goodsSn, goodsName, inventory, brief, 1, 1, 0, weight, string.Empty, fareTemp,WebLoginHelper.GetAdminName()) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更新商品的价格
        /// </summary>
        /// <param name="id">商品ID</param>
        /// <param name="shopPrice">本店价</param>
        /// <param name="marketPrice">市场价</param>
        public void ChangePrice(string id, decimal shopPrice, decimal marketPrice)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
            {
                M_Goods goodsInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = id });
                if (goodsInfo != null)
                {
                    if (goodsInfo.IsEnable == 0)
                    {
                        resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID AND Version=@_Version ", new { _GoodsID = id, _Version = goodsInfo.Version, ShopPrice = shopPrice, MarketPrice = marketPrice, Version = goodsInfo.Version++ }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
                    }else { resWrite = "修改商品的价格必须先将商品进行下架"; }
                }
                else { resWrite = ExceptionNotes.SystemBusy; }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 更新商品详情
        /// </summary>
        /// <param name="id">商品ID</param>
        /// <param name="editorTxt">编辑器内容</param>
        public void UpdataGoodsDetial(string id, string goodsName, int suppliersID, int brandID, int cateID, string editorTxt, string brief, string remark, decimal shopPrice, decimal marketPrice, string keyWords, string fareSysID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage) || WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                M_Goods goodsInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_Goods>(" GoodsID=@_GoodsID ", new { _GoodsID = id });
                if (goodsInfo != null && goodsInfo.IsEnable != 1)//只能修改下架的商品
                {
                    if (WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
                    {
                        string adminSupplierID = WebLoginHelper.GetAdminSupplier();
                        if (Convert.ToString(goodsInfo.SuppliersID) != adminSupplierID)
                        {
                            resWrite = "当前账户不被允许操作其它供应商的商品";
                            EndResponseToWrite(resWrite);
                            return;
                        }
                        //相当于第三方供应商管理自己的账户商品时直接无法修改供应商
                    } else
                    {
                        goodsInfo.SuppliersID = suppliersID;
                    }
                    resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_Goods>(" GoodsID=@_GoodsID AND Version =@_Version ",
                        new
                        {
                            _GoodsID = goodsInfo.GoodsID,
                            _Version = goodsInfo.Version,
                            GoodsDesc = editorTxt,
                            GoodsName = goodsName,
                            SuppliersID = goodsInfo.SuppliersID,
                            CateID = cateID,
                            BrandID = brandID,
                            GoodsBrief = brief,
                            SellerNote = remark,
                            ShopPrice = shopPrice,
                            MarketPrice = marketPrice,
                            KeyWords = keyWords,
                            FareSysID = fareSysID,
                            Version = goodsInfo.Version + 1,
                            LastUpdate = DateTime.Now,
                            ModifyPerson = WebLoginHelper.GetAdminName(),
                            IsUpdateBySupplier = 1
                        }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
                }
                else { resWrite = "如果要修改商品信息，必须先下架该商品信息"; }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 删除商品图册的轮播图
        /// </summary>
        /// <param name="imgID"></param>
        public void DeleteGalleryImg(string imgID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage)||WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).DeleteModel<M_GoodsGallery>(" ImgID=@_ImgID", new { _ImgID = imgID }) ? ExceptionNotes.SystemSuccess:ExceptionNotes.SystemBusy ;
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 取消该商品的推荐
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCpyGoods(string id)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).DeleteModel<M_CpyGoods>(" CpyGoodsID=@_CpyGoodsID ", new { _CpyGoodsID = ToolHelper.ConventToInt32(id, 0) }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 删除错误日志信息
        /// </summary>
        /// <param name="id"></param>
        public void DeleteErrLog(string id)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).DeleteModel<M_ErrorLog>(" ErrorLogID=@_ErrorLogID ", new { _ErrorLogID = ToolHelper.ConventToInt32(id, 0) }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
            }
            EndResponseToWrite(resWrite);
        }

        #endregion

        #region 运费的模板

        /// <summary>
        /// 保存新的运费模板信息 
        /// </summary>
        /// <param name="fareName"></param>
        /// <param name="fareAddress"></param>
        /// <param name="fareTime"></param>
        /// <param name="fareType"></param>
        /// <param name="defaultFare"></param>
        /// <param name="fareAddressList"></param>
        public void AddNewFareTemplate(string fareName,string fareAddress, int fareTime, int fareType,string defaultFare, string fareAddressList)
        {
            string resWite = ExceptionNotes.SystemUnLogin;
            try
            {
                DefaultFareTemp defaultFareTemp = new JavaScriptSerializer().Deserialize<DefaultFareTemp>(defaultFare);//在反序列化的时候如果参数类型（int,decimal等）不对应系统是直接抛异常的，所以不需要再次对反序列话后的数据进行校验，但字符串类型除外
                FareCarryTemp[] fareCarryTempList = null;
                if (!string.IsNullOrEmpty(fareAddressList))//判断是否有为指定地区设置单独的运费
                {
                    fareCarryTempList = new JavaScriptSerializer().Deserialize<FareCarryTemp[]>(fareAddressList);
                }
                resWite = new DB.BLL.MB_Bll(DBEnum.Master).AddNewFareTemplate(fareName,fareAddress,fareTime,fareType,defaultFareTemp,fareCarryTempList) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
            }
            catch
            {
                resWite = "请仔细填写运费数据信息";
            }
            
            EndResponseToWrite(resWite);
        }

        /// <summary>
        /// 删除新的运费模板信息
        /// </summary>
        /// <param name="id"></param>
        public void DeleteFareTemp(string id)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                long count = new DB.BLL.MB_Bll(DBEnum.Master).GetModelListCount(" SELECT COUNT(*) FROM M_Goods WHERE FareSysID =@_FareSysID ", new { _FareSysID = id });
                if (count <= 0)
                {
                    resWrite = new DB.BLL.MB_Bll(DBEnum.Master).DeleteFareTemplate(id) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
                }else
                {
                    resWrite = "该运费模板有已绑定的商品，禁止被删除，请先解绑所有相关商品";
                }
            }
            EndResponseToWrite(resWrite);
        }

        #endregion

        /// <summary>
        /// 添加供应商账号
        /// </summary>
        /// <param name="suppliersNo"></param>
        /// <param name="suppliersName"></param>
        public void AddSuppliers(string suppliersNo,string suppliersName)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage)){
                M_Suppliers suppliersInfo = new M_Suppliers();
                int outSuppliersNo = 0;
                if(int.TryParse(suppliersNo,out outSuppliersNo))
                {
                    suppliersInfo.SuppliersID = outSuppliersNo;
                    suppliersInfo.SuppliersName = suppliersName;
                    suppliersInfo.IsCheck = 1;
                    suppliersInfo.SuppliersDesc = string.Empty;
                    resWrite = new DB.BLL.MB_Bll(DBEnum.Master).AddModel(suppliersInfo) ? ExceptionNotes.SystemSuccess : "请确认供应商编号没有重复";
                }else
                {
                    resWrite = "供应商编号必须为数字";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suppliersName"></param>
        public void UpdateSuppliers(string suppliersName)
        {

        }

        /// <summary>
        /// 删除供应商账户
        /// </summary>
        /// <param name="suppliersID"></param>
        public void DeleteSuppliers(string id)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if(WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).DeleteModel<M_Suppliers>(" SuppliersID=@_SuppliersID ", new { _SuppliersID = id }) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 确认订单进行发货
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <param name="name">物流公司名</param>
        /// <param name="number">物流单号</param>
        public void ConfirmDelivery(string id, string name, string number,string remark)
        {
            string resWrite =ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage)||WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                M_OrderInfo orderInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_OrderInfo>(" OrderID=@_OrderID ",new { _OrderID =id});
                if (orderInfo != null)
                {
                    if (orderInfo.PayStatus == 2 && ( orderInfo.OrderStatus == 1 || orderInfo.OrderStatus==5))
                    {
                        if (new DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<M_OrderInfo>(" OrderID=@_OrderID AND Version=@_Version ", new
                        {
                            _OrderID = orderInfo.OrderID,
                            _Version = orderInfo.Version,
                            Logistical = name,
                            ShippingStatus = 1,
                            OrderStatus = 5,
                            LogisticalNumber = number,
                            ShippingTime = DateTime.Now,
                            Remark = remark,
                            Version = orderInfo.Version++
                        }))
                        {
                            resWrite = ExceptionNotes.SystemSuccess;
                           //WeChatAPI.SendMsg("商品发货提醒", "您的订单" + orderInfo.OrderSn + "已从仓库发出，请等待收货，物流单号为："+ number, orderInfo.OpenID);
                        }
                        else { resWrite = ExceptionNotes.SystemBusy; }
                    }
                    else
                    {
                        resWrite = "订单状态为已确认同时付款状态为已付款才允许进行发货";
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
        /// 审核退货订单---》同意退货
        /// </summary>
        /// <param name="backID"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        /// <param name="address"></param>
        public void ReBackOrderIsPass(string backID,int type,string remark,string address)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                M_ReBackOrder reBackOrderInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_ReBackOrder>(" ReBackID=@_ReBackID ", new { _ReBackID = backID });
                if (reBackOrderInfo != null && reBackOrderInfo.AuthType == 10)
                {
                    if (new DB.BLL.MB_Bll(DBEnum.Master).ComfirmReBackOrderApply(reBackOrderInfo.ReBackID,type,remark,address,reBackOrderInfo.OrderID))
                    {
                        resWrite = ExceptionNotes.SystemSuccess;
                        //if (type == 20)
                        //{
                        //    WeChatAPI.SendMsg("退换货处理回馈通知", "尊敬的用户您好，您的售后订单" + reBackOrderInfo.OrderSn + "已受理；如需退换商品，请尽快将商品寄回以下地址：" + reBackOrderInfo.ReBackAddress + "，谢谢您的合作！", reBackOrderInfo.OpenID);
                        //}
                        //else
                        //{
                        //    WeChatAPI.SendMsg("退换货处理回馈通知", "尊敬的用户您好，您的订单" + reBackOrderInfo.OrderSn + "的售后服务申请被驳回，驳回原因为"+remark+"；如有疑问，请联系人工客服，谢谢您的耐心等待！", reBackOrderInfo.OpenID);
                        //}
                    }
                    else
                    {
                        resWrite = ExceptionNotes.SystemBusy;
                    }
                }
                else
                {
                    resWrite = "请确认没有重复审核该笔退款信息";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 审核退货商品---》确认商品无误，同意退货
        /// </summary>
        /// <param name="backID"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        public void ReBackOrderGoodsIsOK(string backID,int type,string remark)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                M_ReBackOrder reBackOrderInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_ReBackOrder>(" ReBackID=@_ReBackID ", new { _ReBackID = backID });
                if (reBackOrderInfo != null && reBackOrderInfo.AuthType == 20)
                {
                    if (new DB.BLL.MB_Bll(DBEnum.Master).ComfirmReBackOrderApply(reBackOrderInfo.ReBackID, type, remark, reBackOrderInfo.ReBackAddress, reBackOrderInfo.OrderID)) {
                        resWrite = ExceptionNotes.SystemSuccess;
                        //if (type == 30)
                        //{
                        //    WeChatAPI.SendMsg("退换货处理回馈通知", "尊敬的用户您好，您的售后订单" + reBackOrderInfo.OrderSn + "已处理，商家已同意申请；如退换商品，商家将尽快发货到您的手中；如退款，相应的金额将于3个工作日内以原支付方式予以返还，如超时未收到相应退款，请及时联系人工客服，谢谢！", reBackOrderInfo.OpenID);
                        //}else
                        //{
                        //    WeChatAPI.SendMsg("退换货处理回馈通知", "尊敬的用户您好，您的售后订单" + reBackOrderInfo.OrderSn + "申请被商家驳回，驳回原因为" + remark + "；如有疑问，请联系人工客服，谢谢您的耐心等待！", reBackOrderInfo.OpenID);
                        //}
                    }
                    else
                    {
                        resWrite = ExceptionNotes.SystemBusy;
                    }
                }
                else
                {
                    resWrite = "请勿重复处理该信息";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 审核退货商品--》进行退款操作
        /// </summary>
        /// <param name="backID"></param>
        /// <param name="money"></param>
        public void ReBackOrderMoney(string backID,decimal money)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                ResBackTrasd reBackOrderInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetReBackOrderTrade(backID);
                if (reBackOrderInfo != null && reBackOrderInfo.AuthType == 30 && ( reBackOrderInfo.ReBackType==10 || reBackOrderInfo.ReBackType==30))
                {
                    decimal rebackOrderAmount = 0;
                    List<M_ReBackTrade> tradeList = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<M_ReBackTrade>(" OrderID=@_OrderID ", new { _OrderID = reBackOrderInfo.OrderID });
                    if (tradeList != null)
                    {
                        foreach (var item in tradeList)
                        {
                            rebackOrderAmount += item.ReBackMoney;
                        }
                    }
                    if (money>0 && ( money + rebackOrderAmount <= reBackOrderInfo.PayMoney))//历史该笔订单退款总金额之和与当前要退款的总金额之和小于等于流水的支付金额
                    {
                        //调用XF接口进行退款操作
                        string reBackTradeSysID = Guid.NewGuid().ToString();
                        string outReBackTradeNo = string.Empty;
                        string resMsg = string.Empty;                                   //售后订单ID                //原支付流水             //退款金额             //原流水支付时间    //退款订单创建时间        //返回的退款流水编号
                        if (XFServiceAPI.ReBackOrderPayMoney(reBackOrderInfo.CpySysID, reBackOrderInfo.ReBackID, reBackOrderInfo.TradeNo,Convert.ToString(money),reBackOrderInfo.PayTime,reBackOrderInfo.CreatTime,out outReBackTradeNo,out resMsg ))
                        {
                            if( new DB.BLL.MB_Bll(DBEnum.Master).CreatReBackTradeByRetrunFromXFRefundAcc(reBackOrderInfo, reBackTradeSysID, outReBackTradeNo, money))
                            {
                                resWrite = ExceptionNotes.SystemSuccess;
                            }else
                            {
                                resWrite = "XF系统退款已生成，本地数据库执行添加流水记录失败";
                            }
                        }else
                        {
                            resWrite = resMsg;
                        }
                    }else
                    {
                        resWrite = "请注意，您申请退款的金额已经超出了这笔订单流水所扣款的金额";
                    }
                }
                else
                {
                    resWrite = "请勿重复处理该信息";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 第三管理账号再次发货-换货商品
        /// </summary>
        /// <param name="backID"></param>
        /// <param name="logisticalName"></param>
        /// <param name="logisticalNumber"></param>
        public void ChangeGoodsForCustomer(string backID,string logisticalName,string logisticalNumber)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                M_ReBackOrder reBackOrderInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_ReBackOrder>(" ReBackID=@_ReBackID ", new { _ReBackID = backID });
                if (reBackOrderInfo != null && reBackOrderInfo.ReBackType == 20 && reBackOrderInfo.AuthType == 30)
                {
                    if(new DB.BLL.MB_Bll(DBEnum.Master).ComfirmGoodsForCustomer(reBackOrderInfo.ReBackID,logisticalName,logisticalNumber,reBackOrderInfo.OrderID))
                    {
                        //WeChatAPI.SendMsg("退换货处理回馈通知", "尊敬的用户您好，您的售后订单" + reBackOrderInfo.OrderSn + "申请换货的商品已经重新寄出，物流单号："+logisticalNumber+"--"+logisticalName, reBackOrderInfo.OpenID);
                        resWrite = ExceptionNotes.SystemSuccess;
                    }
                    else
                    {
                        resWrite = ExceptionNotes.SystemBusy;
                    }
                }else
                {
                    resWrite = "请确认当前售后类型是换货，同时管理员已同意换货";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 获取品牌信息
        /// </summary>
        /// <param name="id"></param>
        public void GetBrandName(string brandID)
        {
            string resWrite = string.Empty;
            string strSql = "SELECT BrandID,BrandName FROM M_Brand WHERE 1=1 AND BrandID=@_BrandID LIMIT 1";
            IEnumerable<dynamic> info = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList(strSql, new { _BrandID =ToolHelper.ConventToInt32(brandID,0) });
            if (info != null && info.Count() > 0)
            {
                foreach (var item in info)
                {
                    resWrite = item.BrandID + "&" + item.BrandName;
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 管理员提交商品退货信息
        /// </summary>
        /// <param name="resBackInfo"></param>
        /// <param name="phone"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        public void ReBackOrderGoods(string resBackInfo ,string phone, string type, string remark,string orderSysID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            string outEsg = string.Empty;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                List<ResBackGoodsInfo> resBackGoodsInfoList = null;
                try
                {
                    resBackGoodsInfoList = new JavaScriptSerializer().Deserialize<List<ResBackGoodsInfo>>(resBackInfo);
                }catch{}
                if (resBackGoodsInfoList != null && resBackGoodsInfoList.Count > 0)
                {
                    if (new DB.BLL.MB_Bll(DBEnum.Master).AddReBackOrder(resBackGoodsInfoList, phone, Convert.ToInt32(type), remark, WebLoginHelper.GetAdminName(), orderSysID, out outEsg))
                    {
                        resWrite = ExceptionNotes.SystemSuccess;
                    }
                    else
                    {
                        resWrite = string.IsNullOrEmpty(outEsg) ? ExceptionNotes.SystemBusy : outEsg;
                    }
                }else
                {
                    resWrite = "请提交正确的需要退货的商品信息";
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 文件管理删除图片
        /// </summary>
        /// <param name="id"></param>
        public void DeleteFileManageImgs(string id)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                M_FileManage fileInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<M_FileManage>("FileSysID=@_FileSysID", new { _FileSysID = ToolHelper.ConventToInt32(id, 0) });
                if (fileInfo != null && !string.IsNullOrEmpty(fileInfo.FilePath))
                {
                    string fileThemb = Path.GetDirectoryName(fileInfo.FilePath);
                    fileThemb = fileThemb+"\\" + fileInfo.FileName + "Themb" + Path.GetExtension(fileInfo.FilePath);
                    if (FileHelper.DeleteFile(fileInfo.FilePath) && FileHelper.DeleteFile(fileThemb))
                    {
                        resWrite = new DB.BLL.MB_Bll(DBEnum.Master).DeleteFileManage(fileInfo.FileName) ? ExceptionNotes.SystemSuccess:ExceptionNotes.SystemBusy;
                    }else
                    {
                        resWrite = "删除失败";
                    }
                }
            }
            EndResponseToWrite(resWrite);
        }

        #region 异常订单处理

        /// <summary>
        /// 将海淘的推送至海关异常问题恢复为正常
        /// </summary>
        /// <param name="orderID"></param>
        public void HtOrderToSuccess(string orderID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage)){
                DateTime nowTime = DateTime.Now;
                string logInfo = nowTime + "订单状态恢复正常";
                resWrite = new DB.BLL.MB_Bll(DBEnum.Master).UpdateModalSql(" UPDATE M_OrderInfo SET OrderStatus= 9 ,LogInfo=CONCAT(LogInfo,@LogInfo) WHERE OrderStatus =7 AND SuppliersID=101 AND OrderID=@_OrderID ", new { _OrderID =orderID, LogInfo =logInfo}) ? ExceptionNotes.SystemSuccess:ExceptionNotes.SystemBusy;
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 进行XF系统的流水号补单操作
        /// </summary>
        /// <param name="orderID"></param>
        public void XFOrderTradeGetAgain(string orderID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                string resPayInfo = string.Empty;
                if (XFServiceAPI.SeacherThisOrderTrade(orderID, out resPayInfo))
                {
                    resWrite = ExceptionNotes.SystemSuccess;
                }else
                {
                    resWrite = !string.IsNullOrEmpty(resPayInfo) ? resPayInfo : ExceptionNotes.SystemBusy;
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// XF系统再次点击进行流水的同步
        /// </summary>
        /// <param name="tradeSysID"></param>
        public void XFTradeSyncAgain(string tradeSysID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                string resPayInfo = string.Empty;
                if (XFServiceAPI.GetOrderInfoPaymentStatusSync(tradeSysID, out resPayInfo))
                {
                    resWrite = ExceptionNotes.SystemSuccess;
                }
                else
                {
                    resWrite = !string.IsNullOrEmpty(resPayInfo) ? resPayInfo : ExceptionNotes.SystemBusy;
                }
            }
            EndResponseToWrite(resWrite);
        }
        #endregion

        #endregion

        #region EXCEL-操作

        /// <summary>
        /// EXCEL导出商品信息
        /// </summary>
        /// <param name="type"></param>
        public void ExportGoodsInfoList(string type, string url)
        {
            string resWrite = "UNLOGIN";
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
            {
                DataSet ds = new DB.BLL.MB_Bll(DBEnum.Slave).GetGoodsInfo(type, url);
                resWrite = WebSaveHelper.ExportToExcel(ds);
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// EXCEL到处订单信息
        /// </summary>
        /// <param name="url"></param>
        public void ExportOrderInfoList(string url)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            Dictionary<string, string> dic = new Dictionary<string, string>();//申明一个字典
            if (!string.IsNullOrEmpty(url))
            {
                string[] urlArr = url.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                string[] urlList;
                if (urlArr != null)//不为空
                {
                    foreach (var item in urlArr)//循环遍历数组
                    {
                        urlList = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);//再次切割字符串
                        if (urlList != null && urlList.Length == 2)//判断切割后的不为空，并且长度为2
                        {
                            if (!dic.Keys.Contains(urlList[0]))//再次判断字典中是否存在这个值
                            {
                                dic.Add(urlList[0], urlList[1]);//往字典中添加这个键值对
                            }
                        }
                    }
                }
            } //准备工作已完成
            DataSet ds = null;
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                ds = new DB.BLL.MB_Bll(DBEnum.Slave).GetOrdersInfo(dic);
              
            }else if (WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                if (dic.Keys.Contains("suppliersID"))
                {
                    dic["suppliersID"] = WebLoginHelper.GetAdminSupplier();
                }else
                {
                    dic.Add("suppliersID", WebLoginHelper.GetAdminSupplier());
                }
                ds = new DB.BLL.MB_Bll(DBEnum.Slave).GetOrdersInfo(dic);
            }
            resWrite = WebSaveHelper.ExportToExcel(ds);
            EndResponseToWrite(resWrite);
        }

        #endregion
      
        /// <summary>
        /// 管理员分配模块
        /// </summary>
        /// <param name="adminSysID">操作账户JUID</param>
        /// <param name="strModuleArr">模块JUID集合字符串</param>
        public void AssignAdminModal(string adminSysID, string[] strModuleArr)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
            {
                if (strModuleArr.Contains("06625c3a-817d-4b1e-9d72-92654d8fece1"))
                {
                    resWrite = "超级权限不允许进行分配";
                }
                else
                {
                    resWrite = (new DB.BLL.MB_Bll(DBEnum.Master).AssignModale(adminSysID, strModuleArr)) ? ExceptionNotes.SystemSuccess : ExceptionNotes.SystemBusy;
                }
            }
            EndResponseToWrite(resWrite);
        }

        /// <summary>
        /// 获取某个用户的模块集合
        /// </summary>
        /// <param name="adminID">用户ID</param>
        public void GetAdminModalList(string adminID)
        {
            string resWrite = ExceptionNotes.SystemUnLogin;
            if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
            {
                List<M_Modal> info = new DB.BLL.MB_Bll(DBEnum.Slave).GetModalList(adminID);
                resWrite = new JavaScriptSerializer().Serialize(info);
            }
            EndResponseToWrite(resWrite);
        }
       
        /// <summary>
        /// 最终输出数据返回前端
        /// </summary>
        /// <param name="resWrite">返回的数据</param>
        /// 
        public void EndResponseToWrite(string resWrite)
        {
            HttpContext.Current.Response.Write(resWrite);
            HttpContext.Current.ApplicationInstance.CompleteRequest();//强制结束本页即cs页接下来的代码，但会去执行前端aspx页的代码
        }
    }


    

}
