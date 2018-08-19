using DSMTMALL.Core.Common;
using DSMTMALL.MyPublic;
using System;
using System.Web;
namespace DSMTMALL.web.login
{
    public partial class turn_index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.HttpMethod.ToLower() == "get")//获取用户端的传输方式（全转化为小写）是否是get传值
                    {
                        string code = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request.QueryString["code"], "");
                        FileHelper.logger.Info(ToolHelper.IPAddress()+ "_"+code);
                        if (!string.IsNullOrEmpty(code))
                        {
                            HttpContext.Current.Session[WebLoginHelper.SESSION_TOKEN_CODE] = code;
                        }
                        else
                        {
                            FileHelper.logger.Error("没有code值");
                        }
                    }
                }
                catch (Exception esg)
                {
                    FileHelper.logger.Error(Convert.ToString(esg));
                }
            }
        }
        //private string GetClientIP()
        //{
        //    string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    if (null == result || result == string.Empty)
        //    {
        //        result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //    }
        //    if (null == result || result == string.Empty)
        //    {
        //        result = HttpContext.Current.Request.UserHostAddress;
        //    }
        //    return result;
        //}

    }
}