using DSMTMALL.MyPublic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.web
{
    public partial class user_address : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            userID.InnerHtml = WebLoginHelper.GetUserID();
        }
    }
}