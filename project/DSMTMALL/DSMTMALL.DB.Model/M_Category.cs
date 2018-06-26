/**M_Category.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_Category:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Category
	{
		public M_Category()
		{}
		#region Model
		private int _cateid;
		private string _catename;
		private string _keywords;
		private string _catedesc;
		private int _parentid=0;
		private int _orderby=50;
		private int _showinnav=1;
		private string _showimage;
		private int _isenable=1;
		private int _isdelete=0;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int CateID
		{
			set{ _cateid=value;}
			get{return _cateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CateName
		{
			set{ _catename=value;}
			get{return _catename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string KeyWords
		{
			set{ _keywords=value;}
			get{return _keywords;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CateDesc
		{
			set{ _catedesc=value;}
			get{return _catedesc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ParentID
		{
			set{ _parentid=value;}
			get{return _parentid;}
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
		public int ShowInNav
		{
			set{ _showinnav=value;}
			get{return _showinnav;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ShowImage
		{
			set{ _showimage=value;}
			get{return _showimage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsEnable
		{
			set{ _isenable=value;}
			get{return _isenable;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsDelete
		{
			set{ _isdelete=value;}
			get{return _isdelete;}
		}
		#endregion Model

	}
}

