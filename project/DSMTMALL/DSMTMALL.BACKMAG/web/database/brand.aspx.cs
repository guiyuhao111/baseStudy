using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;

namespace DSMTMALL.BACKMAG.web.database
{
    public partial class brand : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                if (!IsPostBack)
                {
                    BindGrid();
                }
            }else
            {
                Response.Redirect("/web/web_login.aspx", false);
            }
        }

        public void BindGrid()
        {
            object dt = GetBrandList();
            repeaterBrandList.DataSource = dt;
            repeaterBrandList.DataBind();
        }

        public object GetBrandList()
        {
            string searchName = ToolHelper.UrlParDecode(Request.QueryString["searchName"], "");
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
           
            iptSearch.Value = searchName;
            int pageSize = 10;
            int listCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageBrandListCount(searchName);
            string url = "/web/database/brand.aspx?";
            url += "searchName=" + ToolHelper.UrlParEncode(searchName, "") + "&";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageBrandList(searchName, (pageNow - 1) * pageSize, pageSize);
            PageNavHelper pageNavHelper = new PageNavHelper(listCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }

    }
}