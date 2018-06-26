using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;

namespace DSMTMALL.BACKMAG.web.thirdAdmin
{
    public partial class data_goodsInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                if (!IsPostBack)
                {
                    BindDdl();
                    BindGrid(sltByOrderStatus.Value.Trim());
                }
            }
            else
            {
                Response.Redirect("/web/web_login.aspx", false);
            }
        }

        public void BindDdl()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["byOrderStatus"]))
            {
                sltByOrderStatus.SelectedIndex = sltByOrderStatus.Items.IndexOf(sltByOrderStatus.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byOrderStatus"], "")));
            }
        }

        public void BindGrid(string orderStatus)
        {
            object dt = GetGoodsList(orderStatus);
            repeaterOrderList.DataSource = dt;
            repeaterOrderList.DataBind();
        }

        public object GetGoodsList(string orderStatus)
        {
            string devSort = string.Empty;
            string searchName = ToolHelper.UrlParDecode(Request.QueryString["searchName"], "");
            string byTimeStart = ToolHelper.UrlParDecode(Request.QueryString["byTimeStart"], "");
            string byTimeEnd = ToolHelper.UrlParDecode(Request.QueryString["byTimeEnd"], "");
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            sltByTimeStart.Value = byTimeStart;
            sltByTimeEnd.Value = byTimeEnd;
            iptSearch.Value = searchName;
            int pageSize = 20;
            string suppliersID = WebLoginHelper.GetAdminSupplier();
            int listCount = 0;
            string url = "/web/thirdAdmin/data_goodsInfo.aspx?";
            url += "byTimeStart=" + ToolHelper.UrlParEncode(byTimeStart, "") + "&";
            url += "byTimeEnd=" + ToolHelper.UrlParEncode(byTimeEnd, "") + "&";
            url += "byOrderStatus=" + ToolHelper.UrlParEncode(orderStatus, "") + "&";
            url += "searchName=" + ToolHelper.UrlParEncode(searchName, "") + "&";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageDataGoodsList(byTimeStart, byTimeEnd, suppliersID, orderStatus,searchName, (pageNow - 1) * pageSize, pageSize,out listCount);
           
            PageNavHelper pageNavHelper = new PageNavHelper(listCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }
    }
}