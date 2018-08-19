using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.web.order
{
    public partial class reback_trade : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                if (!IsPostBack)
                {
                    BindDdl();
                    BindGrid(sltByComfirmStatus.Value.Trim(), sltByPayID.Value.Trim());
                }
            }
            else
            {
                Response.Redirect("/web/web_login.aspx", false);
            }
        }

        public void BindDdl()
        {
            IEnumerable<dynamic> paymentInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList("SELECT * FROM M_Payment WHERE 1=1 AND IsEnabled =1 ORDER BY PayOrder DESC", null);
            new DdlHelper().BindDdlMore(sltByPayID, paymentInfo, "全部", x => { return new ListItem(Convert.ToString(x.PayName), Convert.ToString(x.PayID)); });
            if (!string.IsNullOrEmpty(Request.QueryString["byComfirmStatus"]))
            {
                sltByComfirmStatus.SelectedIndex = sltByComfirmStatus.Items.IndexOf(sltByComfirmStatus.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byComfirmStatus"], "")));
            }
            if (!string.IsNullOrEmpty(Request.QueryString["byPayID"]))
            {
                sltByPayID.SelectedIndex = sltByPayID.Items.IndexOf(sltByPayID.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byPayID"], "")));
            }
        }
        public void BindGrid(string comfirmStatus, string byPayID)
        {
            repeaterOrderList.DataSource = GetGoodsList(comfirmStatus, byPayID);
            repeaterOrderList.DataBind();
        }
        public object GetGoodsList(string comfirmStatus, string byPayID)
        {
            string searchName = ToolHelper.UrlParDecode(Request.QueryString["searchName"], "");
            string byTimeStart = ToolHelper.UrlParDecode(Request.QueryString["byTimeStart"], "");
            string byTimeEnd = ToolHelper.UrlParDecode(Request.QueryString["byTimeEnd"], "");
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            iptSearch.Value = searchName;
            sltByTimeStart.Value = byTimeStart;
            sltByTimeEnd.Value = byTimeEnd;
            int pageSize = 20;
            decimal moneyAmount = 0;
            int listCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageUnusualRebackTradeListCount(byTimeStart, byTimeEnd, comfirmStatus, byPayID, searchName);
            string url = "/web/order/reback_trade.aspx?";
            url += "byTimeStart=" + ToolHelper.UrlParEncode(byTimeStart, "") + "&";
            url += "byTimeEnd=" + ToolHelper.UrlParEncode(byTimeEnd, "") + "&";
            url += "byComfirmStatus=" + ToolHelper.UrlParEncode(comfirmStatus, "") + "&";
            url += "byPayID=" + ToolHelper.UrlParEncode(byPayID, "") + "&";
            url += "searchName=" + ToolHelper.UrlParEncode(searchName, "") + "&";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageUnusualRebackTradeList(byTimeStart, byTimeEnd, comfirmStatus, byPayID, searchName, (pageNow - 1) * pageSize, pageSize,out moneyAmount);
            PageNavHelper pageNavHelper = new PageNavHelper(listCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            moneyAccount.InnerHtml =Convert.ToString( moneyAmount);
            return dt;
        }
    }
}