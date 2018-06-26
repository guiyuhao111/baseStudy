using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEntity;
using DSMTMALL.WebAPI.MyPublic;
using System;
using System.Web.Services;

namespace DSMTMALL.WebAPI
{
    /// <summary>
    /// DSMTALL_XFWEBAPI 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]

        

    public class DSMTALL_XFWEBAPI : WebService
    {
        private static VerifyHelper verifyHelper = new VerifyHelper();

        [WebMethod]
        public bool GetXFVerifyActualPayment(GetXFVerifyActPayment getXFVerifyActPayment)
        {
            if (verifyHelper.CheckPmtSign(ref getXFVerifyActPayment))
            {
                return  MALLServiceHelper.GetXFVerifyActualPayment(getXFVerifyActPayment);
            }
            return false;
        }


        /// <summary>
        /// 客户端读卡服务
        /// </summary>
        /// <param name="getClientReadCardEntity">参数实体</param>
        /// <returns>BackClientReadCardEntity</returns>
        [WebMethod]
        public BackClientReadCardEntity ClientReadCard(GetClientReadCardEntity getClientReadCardEntity)
        {
            if (verifyHelper.CheckCardSign(ref getClientReadCardEntity))
            {
                if (string.IsNullOrEmpty(getClientReadCardEntity.AdminToken) || string.IsNullOrEmpty(getClientReadCardEntity.CheckCode))
                {
                    return null;
                }
                BackClientReadCardEntity backClientReadCardEntity = MALLServiceHelper.ClientReadCard(getClientReadCardEntity);
                verifyHelper.EncryptCardEntity(backClientReadCardEntity);
                return backClientReadCardEntity;
            }
            return null;
        }


    }


}
