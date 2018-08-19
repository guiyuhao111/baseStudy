using DSMTMALL.MyPublic;
using System;

namespace DSMTMALL.web
{
    public partial class goods_cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            userID.InnerHtml = WebLoginHelper.GetUserID();
        }
    }
}