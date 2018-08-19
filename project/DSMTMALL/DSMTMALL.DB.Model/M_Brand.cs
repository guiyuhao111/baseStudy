
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_Brand:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Brand
	{
		public M_Brand()
		{}
		#region Model
		private int _brandid;
		private string _brandname;
		private string _brandlogo;
		private string _branddesc;
		private string _brandurl;
		private int _orderby=50;
		private int _isshow=1;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int BrandID
		{
			set{ _brandid=value;}
			get{return _brandid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BrandName
		{
			set{ _brandname=value;}
			get{return _brandname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BrandLogo
		{
			set{ _brandlogo=value;}
			get{return _brandlogo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BrandDesc
		{
			set{ _branddesc=value;}
			get{return _branddesc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BrandUrl
		{
			set{ _brandurl=value;}
			get{return _brandurl;}
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
		public int IsShow
		{
			set{ _isshow=value;}
			get{return _isshow;}
		}
		#endregion Model

	}
}

