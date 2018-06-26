/**M_AdminLog.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_AdminLog:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_AdminLog
	{
		public M_AdminLog()
		{}
		#region Model
		private int _logid;
		private DateTime _logtime= Convert.ToDateTime("1970-01-01 00:00:00");
		private string _adminid= "0";
		private string _loginfo;
		private string _ipadress;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int LogID
		{
			set{ _logid=value;}
			get{return _logid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime LogTime
		{
			set{ _logtime=value;}
			get{return _logtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AdminID
		{
			set{ _adminid=value;}
			get{return _adminid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LogInfo
		{
			set{ _loginfo=value;}
			get{return _loginfo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string IPAdress
		{
			set{ _ipadress=value;}
			get{return _ipadress;}
		}
		#endregion Model

	}
}

