/**M_CpyGoods.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_CpyGoods:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_CpyGoods
	{
		public M_CpyGoods()
		{}
		#region Model
		private int _cpygoodsid;
		private string _cpysysid;
		private string _goodsid;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int CpyGoodsID
		{
			set{ _cpygoodsid=value;}
			get{return _cpygoodsid;}
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
		public string GoodsID
		{
			set{ _goodsid=value;}
			get{return _goodsid;}
		}
		#endregion Model

	}
}

