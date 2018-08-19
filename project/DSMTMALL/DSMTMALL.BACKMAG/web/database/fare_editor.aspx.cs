using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;

namespace DSMTMALL.BACKMAG.web.database
{
    public partial class fare_editor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                if (!IsPostBack)
                {
                    //if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                    //{
                    //    fare_id.Attributes.Add("mydata",ToolHelper.UrlParDecode(Request.QueryString["id"], ""));
                    //}
                }
            }
            else
            {
                Response.Redirect("/web/web_login.aspx", false);
            }
            
        }
    }
}