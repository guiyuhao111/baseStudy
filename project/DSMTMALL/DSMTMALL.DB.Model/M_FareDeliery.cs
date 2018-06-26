
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_FareDeliery:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_FareDeliery
	{
		public M_FareDeliery()
		{}
		#region Model
		private int _faredelierysysid;
		private string _faredelieryaddress;
		private int _faredelierycount=1;
		private double _faredelieryweight=0.00;
		private double _faredelieryvolume=0.00;
		private decimal _faredelierymoney=999.00M;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int FareDelierySysID
		{
			set{ _faredelierysysid=value;}
			get{return _faredelierysysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FareDelieryAddress
		{
			set{ _faredelieryaddress=value;}
			get{return _faredelieryaddress;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int FareDelieryCount
		{
			set{ _faredelierycount=value;}
			get{return _faredelierycount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public double FareDelieryWeight
		{
			set{ _faredelieryweight=value;}
			get{return _faredelieryweight;}
		}
		/// <summary>
		/// 
		/// </summary>
		public double FareDelieryVolume
		{
			set{ _faredelieryvolume=value;}
			get{return _faredelieryvolume;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal FareDelieryMoney
		{
			set{ _faredelierymoney=value;}
			get{return _faredelierymoney;}
		}
		#endregion Model

	}
}

