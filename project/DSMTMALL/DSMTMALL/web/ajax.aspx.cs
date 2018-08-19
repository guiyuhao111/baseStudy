using DSMTMALL.Core.Common;
using DSMTMALL.MyPublic;
using System;
using System.Web;

namespace DSMTMALL.web
{
    public partial class ajax : System.Web.UI.Page
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
                string sort = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["sort"], "");
                string pageNow = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["pageNow"], "");
                string pageSize = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["pageSize"], "");
                string number = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["number"], "");
                string address = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["address"], "");
                string id_1 = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["id_1"], "");
                string id_2 = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["id_2"], "");
                string id_3 = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["id_3"], "");
                string phone = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["phone"], "");
                string code = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["code"], "");
                string cardNo = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.Form["cardNo"], "");
                if (!string.IsNullOrEmpty(jsonType))
                {
                    if (jsonType == "logining")//用户进行登陆
                    {
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pwd))
                        {
                            //ajax.UserLogin(name, pwd);
                            return;
                        }
                    }
                    else if(jsonType== "trun_index")
                    {
                        ajax.Trun_Index();
                        return;
                    }
                    else if (jsonType == "getGoodsList")//首页获取商品的列表
                    {
                        if (!string.IsNullOrEmpty(pageNow) && !string.IsNullOrEmpty(pageSize))
                        {
                            ajax.GetGoodsListByIndexPage(pageNow, pageSize);
                        }
                        return;
                    }
                    else if (jsonType == "getByPageGoodsList")//获取商品的展示页商品列表
                    {
                        if (!string.IsNullOrEmpty(pageNow) && !string.IsNullOrEmpty(pageSize) && !string.IsNullOrEmpty(sort))
                        {
                            ajax.GetGoodsListByPage(type, sort, pageNow, pageSize, name);
                        }
                        return;
                    }
                    else if(jsonType== "getCpyGoodsList")//首页获取公司推荐的商品信息
                    {
                        ajax.GetGoodsListByCpyIndexPage();
                        return;
                    }
                    else if (jsonType == "getRollInfoList")
                    {
                        ajax.GetRollInfoList();
                        return;
                    }
                    else if(jsonType== "getNewRecomGoodsList")
                    {
                        ajax.GetNewRecomGoodsList();
                        return;
                    }
                    #region 购物车集
                    else if (jsonType == "getUserCartList")//获取购物车列表信息
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.GetUserCartList(id);
                            return;
                        }
                    }
                    else if (jsonType == "getUserCartListToSubmit")//获取购物车列表信息用于提交订单信息
                    {
                        if (!string.IsNullOrEmpty(sort))
                        {
                            string[] cartList = sort.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                            ajax.GetUserCartListToSubmitOrder(ToolHelper.DelRepeatData(cartList),id);
                            return;
                        }
                    }
                    else if (jsonType == "changeCartNumber")//改变购物车商品的数量
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(type))
                        {
                            ajax.ChangeCartNumber(id, type);
                            return;
                        }
                    }
                    else if (jsonType == "changeCartNumbers")//改变购物车商品的数量
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.ChangeCartNumbers(id, number);
                            return;
                        }
                    }
                    else if (jsonType == "addGoodsToCart")//添加商品进购物车
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.AddGoodsToCart(id, number);
                            return;
                        }
                    }
                    else if (jsonType == "deleteUserCart")//删除购物车的一件商品
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            ajax.DeleteUserCart(id);
                            return;
                        }
                    }
                    else if (jsonType == "addUserOrder")//提交购物车生成订单
                    {
                        if (!string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(number) && !string.IsNullOrEmpty(id_1) && !string.IsNullOrEmpty(id_2) && !string.IsNullOrEmpty(sort))
                        {
                            string[] cartList = sort.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                            ajax.AddUserOrder(address, number, id_1, id_2, ToolHelper.DelRepeatData(cartList));
                            return;
                        }
                    }
                    #endregion

                    #region 订单列表

                    else if (jsonType == "getUserOrderList")//获取用户的订单列表
                    {
                        ajax.GetUserOrderList(type, pageNow, pageSize);
                        return;
                    }
                    else if (jsonType == "getUserOrderGoods")//获取用户的订单商品
                    {
                        ajax.GetUserOrderGoods(id);
                        return;
                    }
                   

                    #endregion

                    #region 收货地址
                    else if (jsonType == "deleteUserAddress")//删除用户的收货地址
                    {
                        ajax.DeleteUserAddress(id);
                        return;
                    }
                    else if (jsonType == "getUserAddress")//获取用户的地址
                    {
                        ajax.GetUserAddress(id);
                        return;
                    }
                    else if (jsonType == "getUserAddressList")//获取当前用户的配送地址列表
                    {
                        ajax.GetUserAddressList();
                        return;
                    }
                    else if (jsonType == "choseChildAddress")//选择城市/地区
                    {
                        ajax.ChoseChildAddress(id);
                        return;
                    }
                    else if (jsonType == "addUserAddress")//添加用户的收货地址
                    {
                        if (!string.IsNullOrEmpty(id_1) && !string.IsNullOrEmpty(id_2) && !string.IsNullOrEmpty(id_3) && !string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(number) && !string.IsNullOrEmpty(cardNo))
                        {
                            ajax.AddUserAddress(id_1, id_2, id_3, address, number, name, cardNo);
                            return;
                        }
                    }
                    else if (jsonType == "updateUserAddress")//更改用户的收货地址
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(id_1) && !string.IsNullOrEmpty(id_2) && !string.IsNullOrEmpty(id_3) && !string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(number))
                        {
                            ajax.UpdateUserAddress(id, id_1, id_2, id_3, address, number, name, cardNo);
                            return;
                        }
                    }
                    else if (jsonType == "getThisAddress")//获取当前收货地址
                    {
                        ajax.GetChoiceAddress(id);
                        return;
                    }
                    #endregion

                    #region 订单集合

                    else if (jsonType == "cancelOrder")//用户取消订单支付
                    {
                        ajax.CancelOrder(id);
                        return;
                    }
                    else if (jsonType == "confirmOrder")//用户确认收货
                    {
                        ajax.ConrirmOrder(id);
                        return;
                    }
                    else if (jsonType == "toSubmitCardBanlance")//用户提交订单支付
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(number) && !string.IsNullOrEmpty(code))
                        {
                            ajax.SubmitCardBanlance(id, number, code);
                            return;
                        }
                    }
                    #endregion

                    #region 用户信息

                    else if (jsonType == "toSubmitCode")//用户发送验证码
                    {
                        ajax.SubmitCode(phone, number);
                        return;
                    }
                    else if (jsonType == "toSubmitCodeForLogin")//用户发送登陆验证码
                    {
                        if (!string.IsNullOrEmpty(phone))
                        {
                            ajax.SubmitCodeForLogin(phone);
                            return;
                        }
                    }
                    else if (jsonType == "toSetOrderCode")//用户发送订单手机验证码
                    {
                        ajax.SetOrderCode();
                        return;
                    }
                    else if (jsonType == "toSubmitBindingInfo")//用户提交绑定信息
                    {
                        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(code))
                        {
                            ajax.SubmitBindingInfo(phone, number, code, remark);
                            return;
                        }
                    }
                    else if (jsonType == "changePhone")//换绑手机
                    {
                        ajax.ChangePhone(phone);
                        return;
                    }
                    else if (jsonType == "changeNickName")//更换昵称
                    {
                        ajax.ChangeNikeName(name);
                        return;
                    }
                    #endregion
                  
                    else if (jsonType == "getSubCategoryList")//获取子类目的类目信息
                    {
                        ajax.GetSubCategoryList(type);
                        return;
                    }
                    //else if(jsonType== "unwrapPhone")//解绑手机
                    //{
                    //    ajax.UnwrapPhone();
                    //    return;
                    //}
                    //else if(jsonType== "getUserSysID")//获取用户的ID（用来判断用户是否登陆）
                    //{
                    //    Response.Write(WebLoginHelper.GetUserID());
                    //    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //    return;
                    //}



                    Response.Write("UNLOGIN");
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    Response.Write("UNLOGIN");
                    HttpContext.Current.ApplicationInstance.CompleteRequest();//结束执行
                }
            }
        }
    }
}