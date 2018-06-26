using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSMTMALL.Core.Common.MyEntity
{
    public class WMSNotifyQuery
    {
        public string Owner { get; set; }
        public string Code { get; set; }
        public WMSNotifyQuery(string owner)
        {
            this.Owner = owner;
        }
    }


    public class WMSReturnQuery
    {
        public string Code { get; set; }//0表示成功 
        public string Desc { get; set; }//描述
        public string Status { get; set; }//发货通知单状态 10-已取消20待确认30已配送确认40已发货
        public string WhsStatus { get; set; }//仓库状态，订单状态描述
        public WMSStockOut Result { get; set; }//发货信息
    }

    public class WMSStockOut {
        public string Id { get; set; }//系统订单号
        public string Created { get; set; }//创建时间
        public string Modified { get; set; }//最后修改时间
        public string StockOutDate { get; set; }//发货时间
        public string LogisticCompany { get; set; }//物流公司名称
        public string LogisiticNumber { get; set; }//运单号，多个运单号的话用逗号隔开
        public List<WMSStockOutRow> Rows { get; set; }//单据行信息
    }

    public class WMSStockOutRow
    {
        public string MerchId { get; set; }//系统商品id
        public string RowDesc { get; set; }//行描述
        public string Qty { get; set; }//行数量

    }

}
