using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.web.thirdAdmin
{
    public partial class data_backgoods : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                if (!IsPostBack)
                {
                    BindDdl();
                    BindGrid(sltByOrderStatus.Value.Trim(),sltReBackType.Value.Trim());
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
            if (!string.IsNullOrEmpty(Request.QueryString["byReBackType"]))
            {
                sltReBackType.SelectedIndex = sltReBackType.Items.IndexOf(sltReBackType.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byReBackType"], "")));
            }
        }

        public void BindGrid(string orderStatus,string byReBackType)
        {
            object dt = GetGoodsList(orderStatus, byReBackType);
            repeaterOrderList.DataSource = dt;
            repeaterOrderList.DataBind();
        }

        public object GetGoodsList(string orderStatus,string reBackType)
        {
            string devSort = string.Empty;
            string searchName = ToolHelper.UrlParDecode(Request.QueryString["searchName"], "");
            string byTimeStart = ToolHelper.UrlParDecode(Request.QueryString["byTimeStart"], "");
            string byTimeEnd = ToolHelper.UrlParDecode(Request.QueryString["byTimeEnd"], "");
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            sltByTimeStart.Value = byTimeStart;
            sltByTimeEnd.Value = byTimeEnd;
            iptSearch.Value = searchName;
            int pageSize = 30;
            string suppliersID = WebLoginHelper.GetAdminSupplier();
            int listCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageReBackDataThirdGoodsListCount(byTimeStart, byTimeEnd, suppliersID, orderStatus, reBackType, searchName);
            string url = "/web/thirdAdmin/data_backgoods.aspx?";
            url += "byTimeStart=" + ToolHelper.UrlParEncode(byTimeStart, "") + "&";
            url += "byTimeEnd=" + ToolHelper.UrlParEncode(byTimeEnd, "") + "&";
            url += "byOrderStatus=" + ToolHelper.UrlParEncode(orderStatus, "") + "&";
            url += "byReBackType=" + ToolHelper.UrlParEncode(reBackType, "") + "&";
            url += "searchName=" + ToolHelper.UrlParEncode(searchName, "") + "&";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageReBackDataThirdGoodsList(byTimeStart, byTimeEnd, suppliersID, orderStatus,reBackType, searchName, (pageNow - 1) * pageSize, pageSize);

            PageNavHelper pageNavHelper = new PageNavHelper(listCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }
    }
}