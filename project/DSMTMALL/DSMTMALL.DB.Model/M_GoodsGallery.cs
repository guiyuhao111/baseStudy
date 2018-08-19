
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_GoodsGallery:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_GoodsGallery
	{
		public M_GoodsGallery()
		{}
		#region Model
		private int _imgid;
		private string _goodsid= "0";
		private string _imgurl;
		private string _imgdesc;
		private string _thumburl;
		private string _imgoriginal;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int ImgID
		{
			set{ _imgid=value;}
			get{return _imgid;}
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
		public string ImgUrl
		{
			set{ _imgurl=value;}
			get{return _imgurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ImgDesc
		{
			set{ _imgdesc=value;}
			get{return _imgdesc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ThumbUrl
		{
			set{ _thumburl=value;}
			get{return _thumburl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ImgOriginal
		{
			set{ _imgoriginal=value;}
			get{return _imgoriginal;}
		}
		#endregion Model

	}
}

