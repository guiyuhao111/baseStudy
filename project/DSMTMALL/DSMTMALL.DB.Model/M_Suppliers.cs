
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_Suppliers:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Suppliers
	{
		public M_Suppliers()
		{}
		#region Model
		private int _suppliersid;
		private string _suppliersname;
		private string _suppliersdesc;
		private int _ischeck=1;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int SuppliersID
		{
			set{ _suppliersid=value;}
			get{return _suppliersid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SuppliersName
		{
			set{ _suppliersname=value;}
			get{return _suppliersname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SuppliersDesc
		{
			set{ _suppliersdesc=value;}
			get{return _suppliersdesc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsCheck
		{
			set{ _ischeck=value;}
			get{return _ischeck;}
		}
		#endregion Model

	}
}

