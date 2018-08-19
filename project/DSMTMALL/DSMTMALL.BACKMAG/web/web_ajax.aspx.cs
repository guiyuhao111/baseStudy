using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using System;
using System.Web;

namespace DSMTMALL.BACKMAG.web
{
    public partial class web_ajax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebAjaxHelper ajax = new WebAjaxHelper();//实例化 WebAjaxHelper类
            if (Request.HttpMethod.ToLower() == "post")//获取用户端的传输方法（全转化为小写）是否为post
            {
                string jsonType = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["jsonType"], "");
                string id = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["id"], "");
                string name = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["name"], "");
                string pwd = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["pwd"], "");
                string newPwd = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["newPwd"], "");
                string remark = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["remark"], "");
                string type = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["type"], "");
                string number = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["number"], "");
                string count = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["count"], "");
                string typeC = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["typeC"], "");
                string typeT = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["typeT"], "");
                string typeB = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["typeB"], "");
                string isVal = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["isVal"], "");
                string cid = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["cid"], "");
                string price_s = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["price_s"], "");
                string price_m = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["price_m"], "");
                string status = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["status"], "");
                string url = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["url"], "");
                string word = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["word"], "");
                string endTime = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["endTime"], "");
                string startTime = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["startTime"], "");
                string token = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["token"], "");
                string code = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["code"], "");
                if (!string.IsNullOrEmpty(jsonType))
                {
                    if (jsonType == "adminLogin")
                    {
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pwd))
                        {
                            ajax.AdminLogin(name, pwd,code);
                        }
                        else if (string.IsNullOrEmpty(name))
                        {
                            Response.Write("提示：用户名不能为空");
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            Response.Write("提示：密码不能为空");
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        return;
                    }
                    else if (jsonType == "checkAdminLogin")//校验读卡登陆
                    {
                        if (!string.IsNullOrEmpty(code))
                        {
                            ajax.CheckAdminLogin(code);
                        }
                        return;
                    }
                    else if(jsonType== "adminIsEnable")//账户的启用
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(status))
                        {
                            ajax.ChangeCheckBox(id, status, "IsEnable", "M_AdminUser");
                            return;
                        }
                    }
                    else if(jsonType== "addAdminAmount")//添加账户信息
                    {
                        if (!string.IsNullOrEmpty(name) ) {
                            ajax.AddAdminAmount(name, cid,number,remark);
                            return;
                        }
                    }
                    else if(jsonType== "AdminLogout")//用户退出
                    {
                        ajax.AdminLogout();
                        return;
                    }
                    else if(jsonType== "assignCard")//分配卡片
                    {
                        ajax.AssignCardToAdmin();
                        return;
                    }
                    else if(jsonType== "checkAdminAssign")//校验分配卡号
                    {
                        if (!string.IsNullOrEmpty(code)&&!string.IsNullOrEmpty(id))
                        {
                            ajax.CheckAdminReadCardNum(code,id);
                        }
                        return;
                    }
                    else if(jsonType== "getFareCarry")//获取配送信息
                    {
                        ajax.GetFareCarry(id);
                        return;
                    }
                    else if(jsonType== "getProvience")//获取所有的省
                    {
                        ajax.GetProvience();
                        return;
                    }
                    else if(jsonType== "addNewFareTemp")//添加运费模板
                    {
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(typeB))
                        {
                            ajax.AddNewFareTemplate(name, url,ToolHelper.ConventToInt32(startTime,48),ToolHelper.ConventToInt32(type,20), typeB, typeC);
                            return;
                        }
                    }
                    else if(jsonType== "deleteFareTemp")//删除运费模板
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.DeleteFareTemp(id);
                            return;
                        }
                    }
                    else if(jsonType== "updateFirstMoney")//更新首次运费金额
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.UpdateFareCarryFirstMoney(id,number);
                            return;
                        }
                    }
                    else if (jsonType == "updateContunueMoney")//更新续费金额
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.UpdateFareCarryContinueMoney(id, number);
                            return;
                        }
                    }
                    else if(jsonType== "deleteAdmin")//删除账户
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.DeleteAdminAmount(id);
                            return;
                        }
                    }
                    else if (jsonType == "updateAdminPwd")//更改密码
                    {
                        if (!string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(newPwd))
                        {
                            ajax.UpdateAdminPwd(pwd, newPwd);
                            return;
                        }
                    }
                    else if(jsonType== "updataAdminName")//更改管理员名称
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.UpdataAdminName(number,id);
                            return;
                        }
                    }
                    else if(jsonType== "updataAdminPhone")//更改管理员联系方式
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.UpdataAdminPhone(number, id);
                            return;
                        }
                    }
                    else if(jsonType== "updataAdminPwd")//重置管理员账户密码
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.UpdataAdminPwd(id);
                            return;
                        }
                    }
                    else if(jsonType== "xfOrderTradeGetAgain")//重新获取XF系统该笔订单的流水号信息
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.XFOrderTradeGetAgain(id);
                            return;
                        }
                    }
                    else if(jsonType== "xfTradeSyncAgain")
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.XFTradeSyncAgain(id);
                            return;
                        }
                    }
                    else if(jsonType== "addSuppliers")//添加供应商
                    {
                        if (!string.IsNullOrEmpty(number) && !string.IsNullOrEmpty(name))
                        {
                            ajax.AddSuppliers(number,name);
                            return;
                        }
                    }
                    else if(jsonType== "deleteSuppliers")//删除供应商
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.DeleteSuppliers(id);
                            return;
                        }
                    }
                    else if (jsonType == "suppliersIsEnable")//供应商的启用
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(status))
                        {
                            ajax.ChangeCheckBox(id, status, "IsEnable", "M_Suppliers");
                            return;
                        }
                    }
                    else if(jsonType== "suppliersDesc")//供应商描述
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.ChangeTextarea(id, number, "M_Suppliers");
                            return;
                        }
                    }
                    #region 类目管理

                    else if (jsonType == "categoryOrderBy")//更新类目排序
                    {
                        int outSysID;
                        if (!string.IsNullOrEmpty(id) && int.TryParse(id, out outSysID))
                        {
                            ajax.UpdateOrderBy(outSysID, number, "M_Category");
                            return;
                        }
                    }
                    else if (jsonType == "categoryIsEnable")//设置类目的启禁
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(status))
                        {
                            ajax.ChangeCheckBox(id, status, "IsEnable", "M_Category");
                            return;
                        }
                    }
                    else if (jsonType == "deleteCategory")//删除类目信息
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.DeleteCategory(id);
                            return;
                        }
                    }
                    else if (jsonType == "addCategory")//添加类目信息
                    {
                        int outParentID = 0;
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(id) && int.TryParse(id, out outParentID))//父类不为空
                        {
                            ajax.AddCategory(ToolHelper.DelRepeatData(name.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)), outParentID);//一维数组去重复
                            return;
                        }
                        else if (!string.IsNullOrEmpty(name) && string.IsNullOrEmpty(id))
                        {
                            ajax.AddCategory(ToolHelper.DelRepeatData(name.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)), outParentID);//一维数组去重复
                            return;
                        }
                    }
                    else if (jsonType == "updateCategory")//更新类目信息
                    {
                        int outSysID = 0;
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(cid) && int.TryParse(cid, out outSysID))//父类不为空
                        {
                            ajax.UpdateCategory(name, outSysID, id);
                            return;
                        }
                    }
                    #endregion

                    #region 品牌管理

                    else if (jsonType == "deleteBrand")//删除品牌信息
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.DeleteBrand(id);
                            return;
                        }
                    }
                    else if (jsonType == "brandIsShow")//设置品牌的启禁
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(status))
                        {
                            ajax.ChangeCheckBox(id, status, "IsShow", "M_Brand");
                            return;
                        }
                    }
                    else if (jsonType == "brandOrderBy")//更新品牌排序
                    {
                        int outSysID;
                        if (!string.IsNullOrEmpty(id) && int.TryParse(id, out outSysID))
                        {
                            ajax.UpdateOrderBy(outSysID, number, "M_Brand");
                            return;
                        }
                    }
                    else if (jsonType == "brandDescribe")//更改品牌描述
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.ChangeTextarea(id, word, "M_Brand");
                            return;
                        }
                    }

                    else if (jsonType == "addBrand")//添加品牌名称
                    {
                        if (!string.IsNullOrEmpty(name))
                        {
                            ajax.AddBrand(ToolHelper.DelRepeatData(name.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)));//一维数组去重复
                            return;
                        }
                    }
                    else if (jsonType == "updateBrand")//修改品牌信息
                    {
                        int outSysID = 0;
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(id) && int.TryParse(id, out outSysID))
                        {
                            ajax.UpdateBrand(name, outSysID);
                            return;
                        }
                    }
                    #endregion

                    #region 轮播管理

                    else if (jsonType == "rollIsEnable")//设置轮播的启禁
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(status))
                        {
                            ajax.ChangeCheckBox(id, status, "IsEnable", "M_Roll");
                            return;
                        }
                    }
                    else if (jsonType == "deleteRoll")//删除轮播信息
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.DeleteRoll(id);
                            return;
                        }
                    }
                    else if (jsonType == "rollOrderBy")//轮播信息排序
                    {
                        int outSysID;
                        if (!string.IsNullOrEmpty(id) && int.TryParse(id, out outSysID))
                        {
                            ajax.UpdateOrderBy(outSysID, number, "M_Roll");
                            return;
                        }
                    }
                    else if (jsonType == "addRoll")//添加轮播信息
                    {
                        ajax.AddRoll();
                        return;
                    }
                    else if (jsonType == "updateRoll")//更新轮播信息
                    {
                        int outSysID = 0;
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(cid) && int.TryParse(cid, out outSysID))
                        {
                            ajax.UpdateRoll(id,type, outSysID);
                            return;
                        }
                    }
                    #endregion
                    
                    #region 联想数据

                    else if (jsonType == "searchCates")//联想类目信息
                    {
                        ajax.SearchCates(name);
                        return;
                    }
                    else if (jsonType == "searchBrands")//联想品牌信息
                    {
                        ajax.SearchBrand(name);
                        return;
                    }
                    else if (jsonType == "searchGoods")//联想商品信息
                    {
                        ajax.SearchGoods(name);
                        return;
                    }
                    else if(jsonType== "searchAdvs")//联想广告信息
                    {
                        ajax.SearchAdvs(name);
                        return;
                    }
                    #endregion
                    
                    else if (jsonType == "exportOrderInfo")//导出订单信息
                    {
                        ajax.ExportOrderInfoList(url);
                        return;
                    }

                    #region 产品操作

                    else if (jsonType == "chooseCpyInfo")//选择公司信息
                    {
                        ajax.ChooseCpyInfo();
                        return;
                    }
                    else if (jsonType == "assignCpyToGoods")//分配当前商品指向公司
                    {
                        string[] cpyInfo = word.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        ajax.AssignCpyToGoods(id, ToolHelper.DelRepeatData(cpyInfo));
                        return;
                    }
                    else if (jsonType == "deleteGoods")//删除产品信息
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.DeleteGoods(id);
                            return;
                        }
                    }
                    else if (jsonType == "changePrice")//更改商品价格
                    {
                        decimal outShopPrice; decimal outMarketPrice;
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(price_s) && !string.IsNullOrEmpty(price_m) && decimal.TryParse(price_s, out outShopPrice) && decimal.TryParse(price_m, out outMarketPrice))
                        {
                            ajax.ChangePrice(id, outShopPrice, outMarketPrice);
                            return;
                        }
                    }
                    else if (jsonType == "updateInverntory")//更新商品库存
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.UpdateInverntory(id, number);
                            return;
                        }
                    }
                    else if(jsonType== "updateQuotaNumber")//更新商品限购件数
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.UpdateQuotalNumber(id, number);
                            return;
                        }
                    }
                    else if (jsonType == "updateWeight")//更新商品重量
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.UpdateWeight(id, number);
                            return;
                        }
                    }
                    else if (jsonType == "goodsSellerNote")//更改商品备注信息
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.ChangeTextarea(id, word, "M_Goods");
                            return;
                        }
                    }
                    else if (jsonType == "exportGoodsInfo")//导出商品信息
                    {
                        ajax.ExportGoodsInfoList(type, url);
                        return;
                    }
                    else if (jsonType == "goodsOrderBy")//更新产品排序
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.UpdateGoodsOrderBy(id, number);
                            return;
                        }
                    }
                    else if (jsonType == "goodsNew")//设置上新产品
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(status))
                        {
                            ajax.ChangeCheckBox(id, status, "IsNew", "M_Goods");
                            return;
                        }
                    }
                    else if (jsonType == "goodsIsEnable")//设置产品的启禁
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(status))
                        {
                            ajax.ChangeCheckBox(id, status, "IsEnable", "M_Goods");
                            return;
                        }
                    }
                    else if (jsonType == "goodsIsPromote")//设置产品的启禁
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(status))
                        {
                            ajax.ChangeCheckBox(id, status, "IsPromote", "M_Goods");
                            return;
                        }
                    }
                    else if(jsonType== "addGoodsInfo")//添加新的商品信息
                    {
                        if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(cid) && !string.IsNullOrEmpty(price_s) && !string.IsNullOrEmpty(price_m) && !string.IsNullOrEmpty(type)&& !string.IsNullOrEmpty(typeC) && !string.IsNullOrEmpty(typeT) && !string.IsNullOrEmpty(count))
                        {
                            try
                            {
                                ajax.AddNewGoods(name, cid, Convert.ToDecimal(price_s), Convert.ToDecimal(price_m), Convert.ToInt32(typeC), Convert.ToInt32(typeT),ToolHelper.ConventToInt32(typeB,0), Convert.ToInt32(count), word, remark, ToolHelper.ConventToDouble(number, 0),type);
                                return;
                            }
                            catch { }
                        }
                    }
                    else if (jsonType == "updataGoodsDetial")//更新商品详情
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(typeB) && !string.IsNullOrEmpty(typeC)  && !string.IsNullOrEmpty(price_m) && !string.IsNullOrEmpty(price_s) && !string.IsNullOrEmpty(isVal) && !string.IsNullOrEmpty(cid))
                        {
                            try
                            {
                                ajax.UpdataGoodsDetial(id, name, Convert.ToInt32(typeB), ToolHelper.ConventToInt32(typeT,0), Convert.ToInt32(typeC), word, url, remark, Convert.ToDecimal(price_s), Convert.ToDecimal(price_m), isVal, cid);
                                return;
                            }
                            catch { }

                        }
                    }
                    else if(jsonType== "deleteCpyGoods")//取消当前商品的推荐
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.DeleteCpyGoods(id);
                            return;
                        }
                    }
                    else if(jsonType== "deleteErrLog")//删除错误日志
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.DeleteErrLog(id);
                            return;
                        }
                    }
                    else if (jsonType == "getBrandName")//获取品牌名称
                    {
                        ajax.GetBrandName(id);
                        return;
                    }
                    #endregion

                    #region 模块处理
                    else if (jsonType == "getModalList")//获取所有模块
                    {
                        ajax.GetModalList();
                        return;
                    }
                    else if (jsonType == "getAdminModalList")//获取当前用户模块
                    {
                        ajax.GetAdminModalList(id);
                        return;
                    }
                    else if (jsonType == "assignAdminModal")//分配用户模块
                    {
                        ajax.AssignAdminModal(id, ToolHelper.DelRepeatData(word.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)));
                        return;
                    }
                    #endregion
                
                    #region 相册管理
                    else if (jsonType == "getGalleryImgList")//获取商品的相册图片
                    {
                        ajax.GetGalleryImgList(id);
                        return;
                    }
                    else if (jsonType == "deleteGalleryImg")//删除商品图册轮播图
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.DeleteGalleryImg(id);
                            return;
                        }
                    }
                    else if (jsonType == "deleteImgs")//图片管理-删除图片
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.DeleteFileManageImgs(id);
                            return;
                        }
                    }
                    #endregion

                    #region 退货商品处理

                    else if (jsonType == "orderGoods")//获取当前订单的商品信息
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.SearchOrderGoods(id);
                            return;
                        }
                    }
                    else if (jsonType == "reBackOrderGoods")//获取当前退货订单的退货商品信息
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.SearchReBackOrderGoods(id);
                            return;
                        }
                    }
                    else if (jsonType == "confirmDelivery")//确认当前订单的物流信息
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(number))
                        {
                            ajax.ConfirmDelivery(id, name, number, remark);
                            return;
                        }
                    }
                    else if (jsonType == "rebackGoods")//提交退货申请
                    {
                        if (!string.IsNullOrEmpty(number) && !string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(remark) && !string.IsNullOrEmpty(cid))
                        {
                            ajax.ReBackOrderGoods(id, number, type, remark, cid);
                            return;
                        }
                    }
                    else if (jsonType == "reBackOrderIsPass")//通过退换货订单申请
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(url))
                        {
                            int outType = 0;
                            if (int.TryParse(type, out outType))
                            {
                                ajax.ReBackOrderIsPass(id, outType, remark, url);
                                return;
                            }
                        }
                    }
                    else if (jsonType == "reBackOrderGoodsIsOK")//退货商品确认无误
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(remark) && !string.IsNullOrEmpty(type))
                        {
                            int outType = 0;
                            if (int.TryParse(type, out outType))
                            {
                                ajax.ReBackOrderGoodsIsOK(id, outType, remark);
                                return;
                            }
                        }
                    }
                    else if(jsonType== "reBackOrderMoney")//用户执行退款操作
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(number))
                        {
                            ajax.ReBackOrderMoney(id,ToolHelper.ConventToDecimal(number,0));
                            return;
                        }
                    }
                    else if(jsonType== "changeGoodsForCustomer")
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(number))
                        {
                            ajax.ChangeGoodsForCustomer(id, name, number);
                            return;
                        }
                    }
                    #endregion
                    
                    #region 异常订单处理
                    else if(jsonType== "htOrderToSuccess")//海淘推送至海关出错-恢复正常
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.HtOrderToSuccess(id);
                            return;
                        }
                    }

                    #endregion

                    Response.Write("UNLOGIN");
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            else
            {
                Response.Write("UNLOGIN");
                HttpContext.Current.ApplicationInstance.CompleteRequest();//结束执行
            }
        }
    }
}