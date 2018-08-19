/**M_OrderTrade.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_OrderTrade:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_OrderTrade
	{
		public M_OrderTrade()
		{}
		#region Model
		private string _tradesysid;
		private int _payid=0;
		private string _payname;
		private string _orderunifysn;
		private string _orderpayno;
		private string _tradeno;
		private DateTime _paytime= Convert.ToDateTime("1970-01-01 00:00:00");
		private decimal _paymoney=0.00M;
		private string _remark;
		private string _modifyperson;
		private DateTime _comfirmtime = Convert.ToDateTime("1970-01-01 00:00:00");
		private DateTime _createtime= Convert.ToDateTime("1970-01-01 00:00:00");
		private string _cpyname;
		private string _cpysysid;
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
		public string OrderUnifySn
		{
			set{ _orderunifysn=value;}
			get{return _orderunifysn;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrderPayNo
		{
			set{ _orderpayno=value;}
			get{return _orderpayno;}
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
		public DateTime PayTime
		{
			set{ _paytime=value;}
			get{return _paytime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal PayMoney
		{
			set{ _paymoney=value;}
			get{return _paymoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
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
		public DateTime ComfirmTime
        {
			set{ _comfirmtime=value;}
			get{return _comfirmtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
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
		#endregion Model

	}
}

