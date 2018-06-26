
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_GoodsAttr:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_GoodsAttr
	{
		public M_GoodsAttr()
		{}
		#region Model
		private int _goodsattrid;
		private string _goodsid;
		private int _attrid=0;
		private string _attrvalue;
		private decimal _attrprice=0.00M;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int GoodsAttrID
		{
			set{ _goodsattrid=value;}
			get{return _goodsattrid;}
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
		public int AttrID
		{
			set{ _attrid=value;}
			get{return _attrid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AttrValue
		{
			set{ _attrvalue=value;}
			get{return _attrvalue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal AttrPrice
		{
			set{ _attrprice=value;}
			get{return _attrprice;}
		}
		#endregion Model

	}
}

