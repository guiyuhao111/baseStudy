
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_Region:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Region
	{
		public M_Region()
		{}
		#region Model
		private int _regionid;
		private int _parentid=0;
		private string _regionname;
		private int _regiontype=2;
		private int _agencyid=0;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int RegionID
		{
			set{ _regionid=value;}
			get{return _regionid;}
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
		public string RegionName
		{
			set{ _regionname=value;}
			get{return _regionname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RegionType
		{
			set{ _regiontype=value;}
			get{return _regiontype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AgencyID
		{
			set{ _agencyid=value;}
			get{return _agencyid;}
		}
		#endregion Model

	}
}

