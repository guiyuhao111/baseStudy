using DSMTMALL.BACKMAG.MyPublic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.web.admin
{
    public partial class web_admin_pwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!WebLoginHelper.IsLogin())
            {
                Response.Redirect("/web/web_admin_login.aspx", false);
            }
        }
    }
}