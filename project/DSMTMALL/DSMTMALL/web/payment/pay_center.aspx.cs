using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.MyPublic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.web.payment
{
    public partial class pay_center : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userID = WebLoginHelper.GetUserID();
                if (!string.IsNullOrEmpty(userID))//不为空代表已经登陆
                {
                    BindGrid(userID);
                }
            }
        }

        private void BindGrid(string userID)
        {
            try
            {
                if (Request.HttpMethod.ToLower() == "get")//获取用户端的传输方式（全转化为小写）是否是get传值
                {
                    string orderSn = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.QueryString["OrderSn"], "");
                    string orderUnifySn = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.QueryString["OrderUnifySn"], "");
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    dictionary.Add("OrderSn", orderSn);
                    dictionary.Add("OrderUntifySn", orderUnifySn);
                    string resDictionary = JsonHelper.DictionaryToJson(dictionary);
                    toSubmitCardBanlance.Attributes.Add("mydata", resDictionary);
                    decimal orderAmount = 0;//订单初始化金额
                    decimal fareTempMoney = 0;
                    DateTime orderCreatTime = DateTime.Now;
                    if (!string.IsNullOrEmpty(orderSn) || !string.IsNullOrEmpty(orderUnifySn))
                    {
                        if (!string.IsNullOrEmpty(orderSn))
                        {
                            orderAmount = new SQLEntityHelper().GetOrderInfoAmountByOrderSn(orderSn, out fareTempMoney,out orderCreatTime);
                        }
                        else if (!string.IsNullOrEmpty(orderUnifySn))
                        {
                            orderAmount = new SQLEntityHelper().GetOrderInfoAmountByOrderUnifySn(orderUnifySn, out fareTempMoney,out orderCreatTime);
                        }
                        emCartGoodsPrice.InnerHtml = Convert.ToString(orderAmount - fareTempMoney);
                        emFeeAmount.InnerHtml = Convert.ToString(fareTempMoney);
                        emCartOrderAmount.InnerHtml = Convert.ToString(orderAmount);
                    }
                    //显示可用的卡余额
                    emCartBanlance.InnerHtml = XFServiceAPI.GetUserCardBalanceInfo((DB.Model.M_Users)HttpContext.Current.Session[WebLoginHelper.SESSION_ADMIN]);
                }
            }
            catch
            {
                Response.Redirect("/web/mall_Index.aspx", false);
            }
        }
    }
}