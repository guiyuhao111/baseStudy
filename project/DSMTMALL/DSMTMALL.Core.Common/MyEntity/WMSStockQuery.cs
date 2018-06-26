using System.Collections.Generic;

namespace DSMTMALL.Core.Common.MyEntity
{
    public class WMSStockQuery
    {
        public string Owner { get; set; }
        public string ShopId { get; set; }
        public string SkuInfo { get; set; }
        public WMSStockQuery(string owner,string shopId)
        {
            this.Owner = owner;
            this.ShopId = shopId;
        }
    }

    public class WMSStockResult
    {
        public string Code { get; set; }
        public string Desc { get; set; }
        public WMSStockStocks Result { get; set; }
    }

    public class WMSStockStocks
    {
        public List<WMSStocksSku> Stocks { get; set; }
    }

    public class WMSStocksSku
    {
        public string WhsId { get; set; }
        public string SkuCode { get; set; }
        public string Quantity { get; set; } 
    }

}
