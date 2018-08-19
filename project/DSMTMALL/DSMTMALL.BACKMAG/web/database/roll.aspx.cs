using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;

namespace DSMTMALL.BACKMAG.web.database
{
    public partial class roll : System.Web.UI.Page
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
            repeaterRollList.DataSource = GetList();
            repeaterRollList.DataBind();
        }

        public object GetList()
        {
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            int pageSize = 10;
            int devCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageRollListCount();
            string url = "/web/database/roll.aspx?";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageRollList((pageNow - 1) * pageSize, pageSize);
            PageNavHelper pageNavHelper = new PageNavHelper(devCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }
    }
}