/**M_OrderInfo.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_OrderInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_OrderInfo
	{
		public M_OrderInfo()
		{}
		#region Model
		private string _orderid;
		private string _orderunifysn;
		private string _ordersn;
		private string _tplorderno;
		private int _isverify=0;
		private string _userid;
        private string _openid;
        private string _cpyname;
		private string _cpysysid;
		private int _orderstatus=0;
		private int _shippingstatus=0;
		private int _paystatus=0;
		private string _consignee;
		private int _country=0;
		private string _countryname;
		private int _province=0;
		private string _provincename;
		private int _city=0;
		private string _cityname;
		private int _district=0;
		private string _districtname;
		private string _address;
		private string _mobile;
		private string _postscript;
		private int _shippingid=0;
		private string _shippingname;
		private int _payid=0;
		private string _payname;
		private string _howoos;
		private decimal _goodsamount=0.00M;
		private decimal _shippingfee=0.00M;
		private decimal _payfee=0.00M;
		private decimal _moneypaid=0.00M;
		private decimal _orderamount=0.00M;
		private DateTime _addtime= Convert.ToDateTime("1970-01-01 00:00:00");
		private DateTime _confirmtime= Convert.ToDateTime("1970-01-01 00:00:00");
		private DateTime _paytime= Convert.ToDateTime("1970-01-01 00:00:00");
		private string _logistical;
		private DateTime _shippingtime= Convert.ToDateTime("1970-01-01 00:00:00");
		private string _logisticalnumber;
		private string _modifyperson;
		private string _tradesysid;
		private string _tradeno;
		private DateTime _updatetime= Convert.ToDateTime("1970-01-01 00:00:00");
		private int _suppliersid=0;
		private int _useraddressid;
		private string _consigneecardno;
		private string _suppliersname;
        private int _version = 0;
        private string _remark;
        private string _logInfo;
        /// <summary>
        /// 
        /// </summary>
        public string OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrderUnifySn
		{
			set{ _orderunifysn=value;}
			get{return _orderunifysn;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrderSn
		{
			set{ _ordersn=value;}
			get{return _ordersn;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TPLOrderNo
		{
			set{ _tplorderno=value;}
			get{return _tplorderno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsVerify
		{
			set{ _isverify=value;}
			get{return _isverify;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CpyName
		{
			set{ _cpyname=value;}
			get{return _cpyname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CpySysID
		{
			set{ _cpysysid=value;}
			get{return _cpysysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int OrderStatus
		{
			set{ _orderstatus=value;}
			get{return _orderstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ShippingStatus
		{
			set{ _shippingstatus=value;}
			get{return _shippingstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int PayStatus
		{
			set{ _paystatus=value;}
			get{return _paystatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Consignee
		{
			set{ _consignee=value;}
			get{return _consignee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Country
		{
			set{ _country=value;}
			get{return _country;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CountryName
		{
			set{ _countryname=value;}
			get{return _countryname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Province
		{
			set{ _province=value;}
			get{return _province;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProvinceName
		{
			set{ _provincename=value;}
			get{return _provincename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int City
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CityName
		{
			set{ _cityname=value;}
			get{return _cityname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int District
		{
			set{ _district=value;}
			get{return _district;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DistrictName
		{
			set{ _districtname=value;}
			get{return _districtname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Postscript
		{
			set{ _postscript=value;}
			get{return _postscript;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ShippingID
		{
			set{ _shippingid=value;}
			get{return _shippingid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ShippingName
		{
			set{ _shippingname=value;}
			get{return _shippingname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int PayID
		{
			set{ _payid=value;}
			get{return _payid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PayName
		{
			set{ _payname=value;}
			get{return _payname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HowOos
		{
			set{ _howoos=value;}
			get{return _howoos;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GoodsAmount
		{
			set{ _goodsamount=value;}
			get{return _goodsamount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal ShippingFee
		{
			set{ _shippingfee=value;}
			get{return _shippingfee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal PayFee
		{
			set{ _payfee=value;}
			get{return _payfee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal MoneyPaid
		{
			set{ _moneypaid=value;}
			get{return _moneypaid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal OrderAmount
		{
			set{ _orderamount=value;}
			get{return _orderamount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ConfirmTime
		{
			set{ _confirmtime=value;}
			get{return _confirmtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime PayTime
		{
			set{ _paytime=value;}
			get{return _paytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Logistical
		{
			set{ _logistical=value;}
			get{return _logistical;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ShippingTime
		{
			set{ _shippingtime=value;}
			get{return _shippingtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LogisticalNumber
		{
			set{ _logisticalnumber=value;}
			get{return _logisticalnumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ModifyPerson
		{
			set{ _modifyperson=value;}
			get{return _modifyperson;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TradeSysID
		{
			set{ _tradesysid=value;}
			get{return _tradesysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TradeNo
		{
			set{ _tradeno=value;}
			get{return _tradeno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int SuppliersID
		{
			set{ _suppliersid=value;}
			get{return _suppliersid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int UserAddressID
		{
			set{ _useraddressid=value;}
			get{return _useraddressid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ConsigneeCardNo
		{
			set{ _consigneecardno=value;}
			get{return _consigneecardno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SuppliersName
		{
			set{ _suppliersname=value;}
			get{return _suppliersname;}
		}

        public int Version
        {
            get
            {
                return _version;
            }

            set
            {
                _version = value;
            }
        }

        public string OpenID
        {
            get
            {
                return _openid;
            }

            set
            {
                _openid = value;
            }
        }

        public string Remark
        {
            get
            {
                return _remark;
            }

            set
            {
                _remark = value;
            }
        }

        public string LogInfo
        {
            get
            {
                return _logInfo;
            }

            set
            {
                _logInfo = value;
            }
        }
        #endregion Model

    }
}

