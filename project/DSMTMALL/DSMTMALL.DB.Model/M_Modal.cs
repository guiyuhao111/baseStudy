/**M_Modal.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_Modal:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Modal
	{
		public M_Modal()
		{}
		#region Model
		private string _modalsysid;
		private string _modalname;
		private int _modalno;
		private string _modalpage;
		private int _modalorder=10;
		/// <summary>
		/// 
		/// </summary>
		public string ModalSysID
		{
			set{ _modalsysid=value;}
			get{return _modalsysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ModalName
		{
			set{ _modalname=value;}
			get{return _modalname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ModalNo
		{
			set{ _modalno=value;}
			get{return _modalno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ModalPage
		{
			set{ _modalpage=value;}
			get{return _modalpage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ModalOrder
		{
			set{ _modalorder=value;}
			get{return _modalorder;}
		}
		#endregion Model

	}
}

