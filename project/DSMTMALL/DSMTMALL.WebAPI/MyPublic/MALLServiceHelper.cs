using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEntity;
using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.MyPublic;

namespace DSMTMALL.WebAPI.MyPublic
{
    public class MALLServiceHelper
    {
        /// <summary>
        /// 获取XF系统的回调的订单扣款状态
        /// </summary>
        /// <param name="getXFVerifyActPayment"></param>
        /// <returns></returns>
        public static bool GetXFVerifyActualPayment(GetXFVerifyActPayment getXFVerifyActPayment)
        {
            string outMsg = string.Empty;
            string outOpenID = string.Empty;
            if (getXFVerifyActPayment.TradeType == "0")
            {
                if (new DB.BLL.MY_Bll(DBEnum.Master).UpdateOrderStatusByXFPaymentToComfirm(ToolHelper.ConventToDecimal(getXFVerifyActPayment.PayMoney, 999), getXFVerifyActPayment.TradeNo, getXFVerifyActPayment.TradeStatus, out outMsg, out outOpenID))
                {
                    //WeChatAPI.SendMsg("付款审核成功", "您有一笔订单流水付款审核已通过，正等待仓库发货", outOpenID);
                    return true;
                }
                if (!string.IsNullOrEmpty(outMsg))
                {
                    new DB.BLL.MB_Bll(DBEnum.Master).RecordErrInfoNote("XF系统回调的订单和扣款状态", outMsg);
                }
            }
            else if (getXFVerifyActPayment.TradeType == "1")
            {
                if (new DB.BLL.MY_Bll(DBEnum.Master).UpdateReBackPayAccByXFPaymentToComfirm(ToolHelper.ConventToDecimal(getXFVerifyActPayment.PayMoney, 999), getXFVerifyActPayment.TradeNo, getXFVerifyActPayment.TradeStatus, out outMsg, out outOpenID))
                {
                    //WeChatAPI.SendMsg("退款资金到账通知", "您有一笔退款金额已通过原支付方式退还至您的账户,请注意查收", outOpenID);
                    return true;
                }
                if (!string.IsNullOrEmpty(outMsg))
                {
                    new DB.BLL.MB_Bll(DBEnum.Master).RecordErrInfoNote("XF系统回调的订单和扣款状态", outMsg);
                }
            }
           
            return false;
        }

        /// <summary>
        /// 客户端读卡服务
        /// </summary>
        /// <param name="getClientReadCardEntity">参数实体</param>
        /// <returns>BackClientReadCardEntity</returns>
        public static BackClientReadCardEntity ClientReadCard(GetClientReadCardEntity getClientReadCardEntity)
        {
            BackClientReadCardEntity backClientReadCardEntity = new BackClientReadCardEntity();
            try
            {
                backClientReadCardEntity.IsSuccess = new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<DB.Model.M_AdminUser>("Token = @_Token", new { _Token = getClientReadCardEntity.AdminToken, ReadCardNo = getClientReadCardEntity.CardNo, CheckCode = getClientReadCardEntity.CheckCode }) ? "1" : "0";
            }
            catch {  }
            return backClientReadCardEntity;
        }



    }
}