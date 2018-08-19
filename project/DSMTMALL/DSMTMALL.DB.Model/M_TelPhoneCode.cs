/**M_TelPhoneCode.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_TelPhoneCode:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_TelPhoneCode
	{
		public M_TelPhoneCode()
		{}
		#region Model
		private string _telphone;
		private string _code;
		private DateTime _creattime= Convert.ToDateTime("1970-01-01 00:00:00");
		private int _logintimes=0;
		/// <summary>
		/// 
		/// </summary>
		public string Telphone
		{
			set{ _telphone=value;}
			get{return _telphone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Code
		{
			set{ _code=value;}
			get{return _code;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreatTime
		{
			set{ _creattime=value;}
			get{return _creattime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int LoginTimes
		{
			set{ _logintimes=value;}
			get{return _logintimes;}
		}
		#endregion Model

	}
}

