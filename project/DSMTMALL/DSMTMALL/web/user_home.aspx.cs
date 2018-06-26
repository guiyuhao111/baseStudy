using DSMTMALL.MyPublic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.web
{
    public partial class user_home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(WebLoginHelper.GetUserID()))
            {
                realName.InnerHtml = WebLoginHelper.GetUserName();
                nickName.InnerText = WebLoginHelper.GetNickName();
                HttpCookie cookie = Request.Cookies["imgUrl"];
                if (cookie != null) { imgUrl.Attributes["src"] = cookie.Value; }
                emCartBanlance.InnerHtml = XFServiceAPI.GetUserCardBalanceInfo((DB.Model.M_Users)HttpContext.Current.Session[WebLoginHelper.SESSION_ADMIN]);
            }
            else
            {
                Response.Redirect("/web/user_login.aspx?", false);//验证错误跳转首页，但不登陆
            }
        }
    }
}