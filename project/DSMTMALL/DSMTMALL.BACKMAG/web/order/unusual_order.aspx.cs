using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;

namespace DSMTMALL.BACKMAG.web.order
{
    public partial class abnormal_order : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                if (!IsPostBack)
                {
                    BindGrid();
                }
            }
            else
            {
                Response.Redirect("/web/web_login.aspx", false);
            }
        }
        

        public void BindGrid()
        {
            repeaterOrderList.DataSource = GetGoodsList();
            repeaterOrderList.DataBind();
        }

        public object GetGoodsList()
        {
            string searchName = ToolHelper.UrlParDecode(Request.QueryString["searchName"], "");
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            iptSearch.Value = searchName;
            int pageSize = 10;
            int listCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageUnusualOrderListCount(searchName);
            string url = "/web/order/unusual_order.aspx?";
            url += "searchName=" + ToolHelper.UrlParEncode(searchName, "") + "&";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageUnusualOrderList(searchName, (pageNow - 1) * pageSize, pageSize);
            PageNavHelper pageNavHelper = new PageNavHelper(listCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }
    }
}