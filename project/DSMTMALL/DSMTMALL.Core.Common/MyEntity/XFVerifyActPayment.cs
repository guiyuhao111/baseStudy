using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSMTMALL.Core.Common.MyEntity
{
    public class GetXFVerifyActPayment
    {
        public string TradeNo { get; set; }
        public string PayMoney { get; set; }
        public string TradeStatus { get; set; }//10成功//20失败
        public string TradeType { get; set; } //0 付款，1 退款
        public string TimeStamp { get; set; }
        public string Sign { get; set; }
    }

}
