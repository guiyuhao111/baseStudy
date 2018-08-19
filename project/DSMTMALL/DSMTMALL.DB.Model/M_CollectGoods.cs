
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_CollectGoods:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_CollectGoods
	{
		public M_CollectGoods()
		{}
		#region Model
		private int _recid;
		private string _userid;
		private string _goodsid;
		private DateTime _addtime= Convert.ToDateTime("1970-01-01 00:00:00");
		private int _isattention=1;
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
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsAttention
		{
			set{ _isattention=value;}
			get{return _isattention;}
		}
		#endregion Model

	}
}

