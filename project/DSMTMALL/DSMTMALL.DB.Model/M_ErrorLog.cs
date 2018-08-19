/**M_ErrorLog.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_ErrorLog:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_ErrorLog
	{
		public M_ErrorLog()
		{}
		#region Model
		private int _errorlogid;
		private string _errlogname;
		private string _errlogdesc;
		private DateTime _errlogtime= Convert.ToDateTime("1970-01-01 00:00:00");
		/// <summary>
		/// auto_increment
		/// </summary>
		public int ErrorLogID
		{
			set{ _errorlogid=value;}
			get{return _errorlogid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ErrLogName
		{
			set{ _errlogname=value;}
			get{return _errlogname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ErrLogDesc
		{
			set{ _errlogdesc=value;}
			get{return _errlogdesc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ErrLogTime
		{
			set{ _errlogtime=value;}
			get{return _errlogtime;}
		}
		#endregion Model

	}
}

