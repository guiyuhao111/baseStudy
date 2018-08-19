using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;

namespace DSMTMALL.MyPublic
{
    //这个类用来处理try-catch到错误信息后对数据进行操作
    public class TryCatchErrHelper
    {

        /// <summary>
        /// 执行1001支付错误代码方法
        /// </summary>
        /// <param name="pushType">推送给XF系统的方式（统一/分单）</param>
        /// <param name="pushOrderSn">推送给XF系统的订单编号(统一/分单)</param>
        public void Handler1001Error(string pushType, string pushOrderSn, string pushDsc)
        {
            try
            {
                if (!new DB.BLL.MY_Bll(DBEnum.Master).UpdateOrderStatusByXFPaymentToUnusual(pushType, pushOrderSn, pushDsc))
                {
                    new DB.BLL.MB_Bll(DBEnum.Master).RecordErrInfoNote("XF系统进行卡余额扣款返回信息出错", pushDsc);
                }
            }
            catch (Exception e)
            {
                new DB.BLL.MB_Bll(DBEnum.Master).RecordErrInfoNote("严重警告数据库操作出错","XF系统进行卡余额扣款返回信息出错;严重警告数据库操作出错，错误信息：" + e.Message + "，请立即校对相应订单");
            }
        }
    }
}
 