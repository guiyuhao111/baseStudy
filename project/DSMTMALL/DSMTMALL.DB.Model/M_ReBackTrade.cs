/**  版本信息模板在安装目录下，可自行修改。
* M_ReBackTrade.cs
*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_ReBackTrade:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_ReBackTrade
	{
		public M_ReBackTrade()
		{}
		#region Model
		private string _rebacktradesysid;
		private string _rebacktradeno;
		private string _rebackid;
		private string _tradesysid;
		private string _tradeno;
		private int _payid=0;
		private string _payname;
		private string _orderid;
		private DateTime _rebacktime= Convert.ToDateTime("1970-01-01 00:00:00");
		private decimal _rebackmoney=0.00M;
		private int _comfirmstatus=10;
		private DateTime _comfirmtime= Convert.ToDateTime("1970-01-01 00:00:00");
		private DateTime _createtime= Convert.ToDateTime("1970-01-01 00:00:00");
		private string _cpyname;
		private string _cpysysid;
		/// <summary>
		/// 
		/// </summary>
		public string ReBackTradeSysID
		{
			set{ _rebacktradesysid=value;}
			get{return _rebacktradesysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReBackTradeNo
		{
			set{ _rebacktradeno=value;}
			get{return _rebacktradeno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReBackID
		{
			set{ _rebackid=value;}
			get{return _rebackid;}
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
		public string OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ReBackTime
		{
			set{ _rebacktime=value;}
			get{return _rebacktime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal ReBackMoney
		{
			set{ _rebackmoney=value;}
			get{return _rebackmoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ComfirmStatus
		{
			set{ _comfirmstatus=value;}
			get{return _comfirmstatus;}
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

