/**M_UserAddress.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_UserAddress:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_UserAddress
	{
		public M_UserAddress()
		{}
		#region Model
		private int _addressid;
		private int _addressfirst=0;
		private string _userid= "0";
		private string _consignee;
		private string _consigneecardno;
		private int _country=1;
		private int _province=0;
		private int _city=0;
		private int _district=0;
		private string _address;
		private string _mobile;
		private DateTime _updatetime;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int AddressID
		{
			set{ _addressid=value;}
			get{return _addressid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AddressFirst
		{
			set{ _addressfirst=value;}
			get{return _addressfirst;}
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
		public string Consignee
		{
			set{ _consignee=value;}
			get{return _consignee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ConsigneeCardNo
		{
			set{ _consigneecardno=value;}
			get{return _consigneecardno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Country
		{
			set{ _country=value;}
			get{return _country;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Province
		{
			set{ _province=value;}
			get{return _province;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int City
		{
			set{ _city=value;}
			get{return _city;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int District
		{
			set{ _district=value;}
			get{return _district;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		#endregion Model

	}
}

