using DSMTMALL.Core.Common;
using DSMTMALL.DB.Model;
using DSMTMALL.MyPublic;
using DSMTMALL.XFTAEAPI;
using System;
using System.Web;
using System.Web.Script.Serialization;

namespace DSMTMALL.web.login
{
    public partial class binding : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod.ToLower() == "get")//获取用户端的传输方式（全转化为小写）是否是get传值
            {
                string openid = Convert.ToString(HttpContext.Current.Session[WebLoginHelper.SESSION_OPENID]);

                if (!string.IsNullOrEmpty(openid))
                {
                    DSMT_TAE_ServiceSoapClient pmtCilent = new DSMT_TAE_ServiceSoapClient();
                    GetQueryUserInfoEntity userInfoEntity = new GetQueryUserInfoEntity();
                    try
                    {
                        userInfoEntity.OpenID = new DESHelper().Decrypt(openid);
                        new VerifyHelper().EncryptPmtEntity(userInfoEntity);
                        BackQueryUserInfoEntity backUserInfo = pmtCilent.QueryUserInfo(userInfoEntity);
                        new VerifyHelper().CheckPmtSign(ref backUserInfo);
                        uOpenID.InnerHtml = openid;//将加密后的openID写入隐藏控件
                        if (backUserInfo != null)
                        {
                            if (!string.IsNullOrEmpty(backUserInfo.UserNo) && !string.IsNullOrEmpty(backUserInfo.UserPhone))//判断职员编号是否为空//并且手机号码不为空
                            {
                                string telphoneNo = backUserInfo.UserPhone;
                                string telphoneEnc = new DESHelper().Encrypt(telphoneNo);
                                telephoneNo.Value = WebToolHelper.HiddenTelephoneNo(telphoneNo); //"18758305045";//替换*号符
                                telephoneUserName.InnerHtml = WebToolHelper.HiddenTelephoneUserName(backUserInfo.RealName);
                                telephoneNo.Attributes.Add("disabled", "disabled");
                                telephoneNo.Attributes.Add("mydata", telphoneEnc);
                                BindingInfo bindingInfo = new BindingInfo();
                                bindingInfo.CpySysID = backUserInfo.CpySysID;
                                bindingInfo.CpyName = backUserInfo.CpyName;
                                bindingInfo.SimpleName = backUserInfo.SimpleName;
                                bindingInfo.RealName = backUserInfo.RealName;
                                bindingInfo.UserGender = backUserInfo.UserGender;
                                bindingInfo.UserNo = backUserInfo.UserNo;
                                string jsonUserInfo = new JavaScriptSerializer().Serialize(bindingInfo);
                                telephoneUserName.Attributes.Add("mydata", new DESHelper().Encrypt(jsonUserInfo));
                            }
                        }
                    }
                    catch (Exception es) { FileHelper.logger.Error(Convert.ToString(es)); Response.Redirect("/web/mall_Index.aspx?", false); }
                }
                else
                {
                    FileHelper.logger.Info("获取COOKIE的openID失败");
                }
                
            }
        }
    }
}