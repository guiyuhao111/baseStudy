using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;

namespace DSMTMALL.BACKMAG.web.thirdAdmin
{
    public partial class data_orderInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage)){
                if (!IsPostBack)
                {
                    BindDdl();
                    BindGrid(sltByOrderStatus.Value.Trim(),  sltOrderBy.Value.Trim());
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
           
            if (!string.IsNullOrEmpty(Request.QueryString["byOrderBy"]))
            {
                sltOrderBy.SelectedIndex = sltOrderBy.Items.IndexOf(sltOrderBy.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byOrderBy"], "")));
            }
        }

        public void BindGrid(string orderStatus, string sltOrderBy)
        {
            object dt = GetGoodsList(orderStatus, sltOrderBy);
            repeaterOrderList.DataSource = dt;
            repeaterOrderList.DataBind();
        }

        public object GetGoodsList(string orderStatus,  string sltOrderBy)
        {
            string devSort = string.Empty;
            string searchName = ToolHelper.UrlParDecode(Request.QueryString["searchName"], "");
            string byTimeStart = ToolHelper.UrlParDecode(Request.QueryString["byTimeStart"], "");
            string byTimeEnd = ToolHelper.UrlParDecode(Request.QueryString["byTimeEnd"], "");
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            if (sltOrderBy == "0")
            {
                devSort = " ORDER BY O.PayTime DESC ";
            }
            if (sltOrderBy == "1")
            {
                devSort = " ORDER BY O.AddTime DESC ";
            }
            if (sltOrderBy == "2")
            {
                devSort = " ORDER BY O.UpdateTime DESC ";
            }
            iptSearch.Value = searchName;
            sltByTimeStart.Value = byTimeStart;
            sltByTimeEnd.Value = byTimeEnd;
            int pageSize = 20;
            string suppliersID = WebLoginHelper.GetAdminSupplier();
            int listCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageDataOrderListCount(byTimeStart, byTimeEnd, suppliersID, orderStatus, searchName);
            string url = "/web/order/order_list.aspx?";
            url += "byTimeStart=" + ToolHelper.UrlParEncode(byTimeStart, "") + "&";
            url += "byTimeEnd=" + ToolHelper.UrlParEncode(byTimeEnd, "") + "&";
            url += "byOrderStatus=" + ToolHelper.UrlParEncode(orderStatus, "") + "&";
            url += "byOrderBy=" + ToolHelper.UrlParEncode(sltOrderBy, "") + "&";
            url += "searchName=" + ToolHelper.UrlParEncode(searchName, "") + "&";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageDataOrderList(byTimeStart, byTimeEnd, suppliersID, orderStatus, devSort,searchName, (pageNow - 1) * pageSize, pageSize);
            PageNavHelper pageNavHelper = new PageNavHelper(listCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            orderCount.InnerHtml =Convert.ToString( listCount);
            return dt;
        }
    }
}