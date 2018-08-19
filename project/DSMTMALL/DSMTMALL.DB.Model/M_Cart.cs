
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_Cart:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Cart
	{
		public M_Cart()
		{}
		#region Model
		private int _cartid;
		private string _userid;
		private string _goodsid;
		private string _goodssn;
		private string _goodsname;
		private int _buynumber=1;
		private int _canhandler=1;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int CartID
		{
			set{ _cartid=value;}
			get{return _cartid;}
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
		public string GoodsID
		{
			set{ _goodsid=value;}
			get{return _goodsid;}
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
		public string GoodsName
		{
			set{ _goodsname=value;}
			get{return _goodsname;}
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
		public int CanHandler
		{
			set{ _canhandler=value;}
			get{return _canhandler;}
		}
		#endregion Model

	}
}

