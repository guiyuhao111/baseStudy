
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_Attribute:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Attribute
	{
		public M_Attribute()
		{}
		#region Model
		private int _attrid;
		private int _catid=0;
		private string _attrname;
		private int _orderby=50;
		private string _attrvalues;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int AttrID
		{
			set{ _attrid=value;}
			get{return _attrid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int CatID
		{
			set{ _catid=value;}
			get{return _catid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AttrName
		{
			set{ _attrname=value;}
			get{return _attrname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int OrderBy
		{
			set{ _orderby=value;}
			get{return _orderby;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AttrValues
		{
			set{ _attrvalues=value;}
			get{return _attrvalues;}
		}
		#endregion Model

	}
}

