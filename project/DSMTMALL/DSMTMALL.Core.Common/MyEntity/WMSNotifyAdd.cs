using System;
using System.Collections.Generic;

namespace DSMTMALL.Core.Common.MyEntity
{
    public class WMSNotifyAdd
    {
        public string Owner { get; set; }
        public AddContent Content {get;set;}

        public WMSNotifyAdd(string owner)
        {
            this.Owner = owner;
        }
    }

    /// <summary>
    /// 商品信息
    /// </summary>
    public class AddContent
    {
        public string Id { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
        public string PaymentTime { get; set; }
        public string ShopId { get; set; }
        public string OrderOuterId { get; set; }
        public string BuyerMessage { get; set; }
        public string SellerComment { get; set; }
        public string LogisticCompany { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverProvince { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverDistrict { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhone1 { get; set; }
        public string ReceiverPhone2 { get; set; }
        public string ReceiverPostcode { get; set; }
        public string HasInvoice { get; set; }
        public string InvoiceHead { get; set; }
        public string InvoiceComment { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerTrueName { get; set; }
        public string BuyerIdCardNo { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerPhone { get; set; }
        public string FeeAmount { get; set; }
        public string Discount { get; set; }
        public string PayAmount { get; set; }
        public List<AddRows> Rows { get; set; }
        public List<Payment> PayMents { get; set; }

        public AddContent()
        {
            this.ShopId = "12";
            this.HasInvoice = "0";
            this.FeeAmount = "0";
            this.Discount = "0";

        }
    }

    /// <summary>
    /// 发货通知单
    /// </summary>
    public class AddRows
    {
        public string OrderRowId { get; set; }
        public string MerchId { get; set; }
        public string MerchCode { get; set; }
        public string RowDesc { get; set; }
        public string Qty { get; set; }
        public string Price { get; set; }
        public string AdjustFee { get; set; }
        public string Amount { get; set; } 

        public AddRows()
        {
            this.AdjustFee = "0";
            this.Amount =(Convert.ToInt32(Qty) * Convert.ToDecimal(Price)).ToString();
        }
    }

    /// <summary>
    /// 支付信息
    /// </summary>
    public class Payment
    {
        public string PaymentMethod { get; set; }
        public string Amount { get; set; }
        public string PayNumber { get; set; }
        public string PaymentInfo { get; set; }
    }


}