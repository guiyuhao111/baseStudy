using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSMTMALL.DB.Model
{
     
    /// <summary>
    /// 用户提交订单时获取当前商品信息实体类
    /// </summary>
    public class GoodsCartToOrder
    {
        public string GoodsID { get; set; }
        public string GoodsName { get; set; }
        public string GoodsImg { get; set; }
        public string NGoodsImg { get; set; }
        public decimal ShopPrice { get; set; }
        public decimal MarketPrice { get; set; }
        public int BuyNumber { get; set; }
        public int CartID { get; set; }
        public string GoodsSn { get; set; }
        public int Inventory { get; set; }
        public int SaleNumber { get; set; }
        public int SuppliersID { get; set; }
        public string SuppliersName { get; set; }
        public int IsDelete { get; set; }
        public int IsEnable { get; set; }
        public string FareSysID { get; set; }
        public decimal FirstMoney { get; set; }
        public double Weight { get; set; }
        public double ContinueCount { get; set; }
        public decimal ContinueMoney { get; set; }
        public double FirstCount { get; set; }
        public string CarryAddressList { get; set; }
    }

    /// <summary>
    /// 用户提交订单时获取当前配送地址实体类
    /// </summary>
    public class UserAddressToOrder : M_UserAddress {
        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }

    }

    #region 运费配置实体类

    public class DefaultFareTemp
    {
        public double DefaultCount { get; set; }
        public decimal DefaultMoney { get; set; }
        public double DefaultAddCount { get; set; }
        public decimal DefaultAddFee { get; set; }
    }

    public class FareCarryTemp
    {
        public string Address { get; set; }
        public double Count { get; set; }
        public decimal Money { get; set; }
        public double AddCount { get; set; }
        public decimal AddFee { get; set; }
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public class GoodsPayForFareCount
    {
        public string SupplierSysID { get; set; }
        public string FareSysID { get; set; }
        public int FareType { get; set; }
        public double FirstCount { get; set; }
        public decimal FirstMoney { get; set; }
        public double ContinueCount { get; set; }
        public decimal ContinueMoney { get; set; }
        public double Weight { get; set; }
    }

    /// <summary>
    ///  XF系统返回绑定用户信息
    /// </summary>
    public class BindingInfo
    {
        public BindingInfo()
        {
            CpyName = string.Empty;
            CpySysID = string.Empty;
            SimpleName = string.Empty;
            RealName = string.Empty;
            UserGender = string.Empty;
            UserNo = string.Empty;
        }
        public string CpySysID { get; set; }
        public string CpyName { get; set; }
        public string SimpleName { get; set; }//公司简称
        public string RealName { get; set; }
        public string UserGender { get; set; }
        public string UserNo { get; set; }
    }

    /// <summary>
    /// 用户获取订单列表信息
    /// </summary>
    public class UserOrderInfo
    {
        public string OrderSn { get; set; }
        public string Consignee { get; set; }
        public string PayName { get; set; }
        public string Mobile { get; set; }
        public string NOrderAddress { get; set; }
        public DateTime PayTime { get; set; }
        public DateTime AddTime { get; set; }
        public string NOrderStatus { get; set; }
        public int OrderStatus { get; set; }
        public string NPayStatus { get; set; }
        public int PayStatus { get; set; }
        public string NShippingStatus { get; set; }
        public int ShippingStatus { get; set; }
        public decimal OrderAmount { get; set; }
        public string OrderID { get; set; }
        public string SuppliersName { get; set; }
    }

    /// <summary>
    /// 管理员退回订单商品信息
    /// </summary>
    public class ResBackGoodsInfo
    {
        public string GoodsID { get; set; }
        public int GoodsNum { get; set; }
    }

    /// <summary>
    /// 管理员退款时获取订单流水信息
    /// </summary>
    public class ResBackTrasd
    {
        public string TradeNo { get; set; }
        public string TradeSysID { get; set; }
        public decimal PayMoney { get; set; }//这个支付金额是当前流水的支付金额
        public int PayID { get; set; }
        public string PayName { get; set; }
        public string CpySysID { get; set; }
        public string CpyName { get; set; }
        public string ReBackID { get; set; }
        public int AuthType { get; set; }
        public int ReBackType { get; set; }
        public string OrderSn { get; set; }
        public string OrderUnifySn { get; set; }
        public string OrderID { get; set; }
        public DateTime CreatTime { get; set; }//退款订单创建时间
        public DateTime PayTime { get; set; }//原流水支付时间
    }

    /// <summary>
    /// 提交商品订单时获取商品的信息（包括限购信息）
    /// </summary>
    public class CartGoodsNumber
    {
        public string GoodsID { get; set; }//商品ID
        public string GoodsName { get; set; }//商品名称
        public int BuyNumber { get; set; }//商品数量
        public int QuotaNumber { get; set; }//限购数量
        public int GoodsNumber { get; set; }//商品库存
    }

    /// <summary>
    /// 提交商品订单时获取用户今天购买的商品的所有商品的总和（付款成功与未付款的）
    /// </summary>
    public class UserTodayBuyOrderGoods
    {
        public string GoodsID { get; set; }
        public int SUMBuyNumber { get; set; }

    }
}
