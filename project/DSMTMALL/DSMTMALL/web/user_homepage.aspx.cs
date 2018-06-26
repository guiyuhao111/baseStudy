using DSMTMALL.Core.Common;
using DSMTMALL.MyPublic;
using System;
using System.Web;

namespace DSMTMALL.web
{
    public partial class user_homepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            userID.InnerHtml = WebLoginHelper.GetUserID();
            tdUserName.InnerHtml = WebLoginHelper.GetNickName();
            string url = WebLoginHelper.GetUserImg();
            if (!string.IsNullOrEmpty(url)) { imgUrl.Attributes.Add("src", url); }
        }
    }
}