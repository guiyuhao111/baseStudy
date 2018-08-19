
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_Payment:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Payment
	{
		public M_Payment()
		{}
		#region Model
		private int _payid;
		private string _paycode;
		private string _payname;
		private decimal _payfee=0.00M;
		private string _paydesc;
		private int _payorder=0;
		private int _isenabled=0;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int PayID
		{
			set{ _payid=value;}
			get{return _payid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PayCode
		{
			set{ _paycode=value;}
			get{return _paycode;}
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
		public decimal PayFee
		{
			set{ _payfee=value;}
			get{return _payfee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PayDesc
		{
			set{ _paydesc=value;}
			get{return _paydesc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int PayOrder
		{
			set{ _payorder=value;}
			get{return _payorder;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsEnabled
		{
			set{ _isenabled=value;}
			get{return _isenabled;}
		}
		#endregion Model

	}
}

