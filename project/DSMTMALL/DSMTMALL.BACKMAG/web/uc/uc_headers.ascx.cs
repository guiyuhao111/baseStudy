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
    public partial class uc_headers : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsLogin())
            {
                if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
                {
                    hMainTitle.InnerHtml =  WebLoginHelper.GetAdminName();
                }
                if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
                {
                    hMainTitle.InnerHtml =  WebLoginHelper.GetAdminName(); ;
                }
                if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
                {
                    hMainTitle.InnerHtml =  WebLoginHelper.GetAdminName(); ;
                }
                string hrefUrl = string.Empty;
                int order_Count =GetOrderInfoToDelivery(out hrefUrl);
                deliveryOrder.Attributes.Add("href",hrefUrl);
                orderCount.InnerHtml =Convert.ToString(order_Count);
                if (order_Count > 0)
                {
                    msgCount.InnerHtml = "1";
                }else 
                {
                    msgCount.InnerHtml = "0";
                }
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
    }
}