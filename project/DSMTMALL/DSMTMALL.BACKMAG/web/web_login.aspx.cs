using DSMTMALL.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.web
{
    public partial class web_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();//清除全部Session(在载入登录页的时候就要将所有的SESSION进行清除)
        }
    }
}