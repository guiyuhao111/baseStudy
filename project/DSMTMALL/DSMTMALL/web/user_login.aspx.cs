using DSMTMALL.Core.Common;
using DSMTMALL.MyPublic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.web
{
    public partial class user_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsLogin())
            {
                Response.Redirect("/web/mall_Index.aspx?", false);
            }
            else
            {
                //Session.Abandon();//清除全部Session(在载入登录页的时候就要将所有的SESSION进行清除)
            }
        }
    }
}