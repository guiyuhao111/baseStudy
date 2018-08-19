﻿/**M_OrderGoods.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_OrderGoods:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_OrderGoods
	{
		public M_OrderGoods()
		{}
		#region Model
		private int _recid;
		private string _ordersn;
		private string _orderid;
		private string _goodsid;
		private string _goodsname;
		private string _goodssn;
		private int _buynumber=1;
		private decimal _marketprice=0.00M;
		private decimal _goodsprice=0.00M;
		private string _goodsattr;
		private int _issend=0;
		private int _isreal=1;
		private string _orderunifysn;
        private DateTime _addtime = Convert.ToDateTime("1970-01-01 00:00:00");
        /// <summary>
        /// auto_increment
        /// </summary>
        public int RecID
		{
			set{ _recid=value;}
			get{return _recid;}
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
		public string OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GoodsID
		{
			set{ _goodsid=value;}
			get{return _goodsid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GoodsName
		{
			set{ _goodsname=value;}
			get{return _goodsname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GoodsSn
		{
			set{ _goodssn=value;}
			get{return _goodssn;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int BuyNumber
		{
			set{ _buynumber=value;}
			get{return _buynumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal MarketPrice
		{
			set{ _marketprice=value;}
			get{return _marketprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GoodsPrice
		{
			set{ _goodsprice=value;}
			get{return _goodsprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GoodsAttr
		{
			set{ _goodsattr=value;}
			get{return _goodsattr;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsSend
		{
			set{ _issend=value;}
			get{return _issend;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsReal
		{
			set{ _isreal=value;}
			get{return _isreal;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrderUnifySn
		{
			set{ _orderunifysn=value;}
			get{return _orderunifysn;}
		}

        public DateTime AddTime
        {
            get
            {
                return _addtime;
            }

            set
            {
                _addtime = value;
            }
        }
        #endregion Model

    }
}
