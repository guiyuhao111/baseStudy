
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_Shipping:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Shipping
	{
		public M_Shipping()
		{}
		#region Model
		private int _shippingid;
		private string _shippingcode;
		private string _shippingname;
		private string _shippingdesc;
		private decimal _shippingmoney=15.00M;
		private int _supportcod=0;
		private int _isenabled=0;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int ShippingID
		{
			set{ _shippingid=value;}
			get{return _shippingid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ShippingCode
		{
			set{ _shippingcode=value;}
			get{return _shippingcode;}
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
		public string ShippingDesc
		{
			set{ _shippingdesc=value;}
			get{return _shippingdesc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal ShippingMoney
		{
			set{ _shippingmoney=value;}
			get{return _shippingmoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int SupportCod
		{
			set{ _supportcod=value;}
			get{return _supportcod;}
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

