using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSMTMALL.BACKMAG.MyPublic
{
    public class WebModuleHelper : IHttpModule
    {
        #region IHttpModule 实现
        public void Dispose() { }
        public string ModuleName
        {
            get { return "WebModuleHelper"; }
        }
        public void Init(HttpApplication application)
        {
            application.AcquireRequestState += new EventHandler(application_AuthenticateRequest);
        }
        #endregion IHttpModule 实现

        private void application_AuthenticateRequest(Object source, EventArgs e)
        {
            
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            string filePath = context.Request.FilePath.ToString().ToLower();
            string fileName = VirtualPathUtility.GetFileName(filePath);
            string fileExt = VirtualPathUtility.GetExtension(filePath);
            string rawUrl = context.Request.RawUrl.ToLower();
            if (fileExt.CompareTo(".aspx") != 0)
            {
                if (rawUrl.Contains("/files/qr/"))
                {
                    context.Response.Redirect("/web/web_login.aspx");
                    return;
                }
                return;
            }
            if (rawUrl.Contains("/web/") && !rawUrl.Contains("/web/web_login.aspx") && !rawUrl.Contains("/web/web_ajax.aspx"))
            {
                if (!WebLoginHelper.IsLogin())
                {
                    context.Response.Redirect("/web/web_login.aspx");
                    return;
                }
                return;
            }

        }
    }
}