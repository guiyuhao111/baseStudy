
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_GoodsCate:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_GoodsCate
	{
		public M_GoodsCate()
		{}
		#region Model
		private string _goodsid;
		private int _cateid;
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
		public int CateID
		{
			set{ _cateid=value;}
			get{return _cateid;}
		}
		#endregion Model

	}
}

