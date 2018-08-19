using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.web.uc
{
    public partial class un_theader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsLogin())
            {
                if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
                {
                    hMainTitle.InnerHtml = WebLoginHelper.GetAdminName();
                }
                if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
                {
                    hMainTitle.InnerHtml = WebLoginHelper.GetAdminName(); ;
                }
                if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
                {
                    hMainTitle.InnerHtml = WebLoginHelper.GetAdminName(); ;
                }
                string hrefUrl = string.Empty;
                string hrefUrl_1 = string.Empty;
                int order_Count = GetOrderInfoToDelivery(out hrefUrl);
                int backOrder_Count = GetReBackOrderToChangeGoods(out hrefUrl_1);
                int theMsgCount = 0;//提醒事件总和
                deliveryOrder.Attributes.Add("href", hrefUrl);
                orderCount.InnerHtml = Convert.ToString(order_Count);
                deliverReBack.Attributes.Add("href", hrefUrl_1);
                changeGoods.InnerHtml = Convert.ToString(backOrder_Count);
                theMsgCount += order_Count > 0 ? 1 : 0;
                theMsgCount += backOrder_Count > 0 ? 1 : 0;
                msgCount.InnerHtml = Convert.ToString(theMsgCount);
            }
            aExit.HRef = "javascript:;";
            aExit.Attributes.Add("data-toggle", "modal");
            aExit.Attributes.Add("data-target", "#confirmModal");
            aExit.Attributes.Add("onclick", "ExitConfirm()");
        }

        /// <summary>
        /// 获取当前账号需要发货的订单数量
        /// </summary>
        /// <returns></returns>
        private int GetOrderInfoToDelivery(out string hrefUrl)
        {
            int resMsg = 0;
            hrefUrl = "javascript:;";
            string suppliersID = WebLoginHelper.GetAdminSupplier();
            if (!string.IsNullOrEmpty(suppliersID))
            {
                if (WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
                {
                    hrefUrl = "/web/thirdAdmin/orderList.aspx?";
                    resMsg = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelListCount<DB.Model.M_OrderInfo>(" SuppliersID=@_SuppliersID AND OrderStatus=1 AND PayStatus=2 AND ShippingStatus=0 ", new { _SuppliersID = suppliersID });
                }
                else if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
                {
                    hrefUrl = "/web/order/order_list.aspx?";
                    resMsg = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelListCount<DB.Model.M_OrderInfo>(" OrderStatus=1 AND PayStatus=2 AND ShippingStatus=0 ", null);
                }
            }
            return resMsg;
        }
        
        /// <summary>
        /// 获取当前账户需要处理的售后换货订单数量
        /// </summary>
        /// <param name="hrefUrl"></param>
        /// <returns></returns>
        private int GetReBackOrderToChangeGoods(out string hrefUrl)
        {
            int resMsg = 0;
            hrefUrl = "javascript:;";
            string suppliersID = WebLoginHelper.GetAdminSupplier();
            if (!string.IsNullOrEmpty(suppliersID))
            {
                if (WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
                {
                    hrefUrl = "/web/thirdAdmin/data_backorder.aspx?";
                    resMsg = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelListCount<DB.Model.M_ReBackOrder>(" SuppliersID=@_SuppliersID AND ReBackType=20 AND AuthType=30 ", new { _SuppliersID =suppliersID});
                }
            }
            return resMsg;
        }
    }
}