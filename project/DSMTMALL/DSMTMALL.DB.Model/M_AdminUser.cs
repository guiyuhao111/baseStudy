/**M_AdminUser.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_AdminUser:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_AdminUser
	{
		public M_AdminUser()
		{}
		#region Model
		private string _adminid;
		private int _suppliersid=0;
		private string _adminname;
		private string _password;
		private string _phone;
		private DateTime _logintime= Convert.ToDateTime("1970-01-01 00:00:00");
		private int _isenable=1;
		private string _notelist= "0";
		private int _mistakenum;
		private string _remark;
		private string _modifyperson;
		private DateTime _updatetime= Convert.ToDateTime("1970-01-01 00:00:00");
		private DateTime _createtime= Convert.ToDateTime("1970-01-01 00:00:00");
		private string _readcardno;
		private string _tokencardno;
		private string _checkcode;
		private string _token;
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
		public int SuppliersID
		{
			set{ _suppliersid=value;}
			get{return _suppliersid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AdminName
		{
			set{ _adminname=value;}
			get{return _adminname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Password
		{
			set{ _password=value;}
			get{return _password;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Phone
		{
			set{ _phone=value;}
			get{return _phone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime LoginTime
		{
			set{ _logintime=value;}
			get{return _logintime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsEnable
		{
			set{ _isenable=value;}
			get{return _isenable;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string NoteList
		{
			set{ _notelist=value;}
			get{return _notelist;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int MistakeNum
		{
			set{ _mistakenum=value;}
			get{return _mistakenum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ModifyPerson
		{
			set{ _modifyperson=value;}
			get{return _modifyperson;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReadCardNo
		{
			set{ _readcardno=value;}
			get{return _readcardno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TokenCardNo
		{
			set{ _tokencardno=value;}
			get{return _tokencardno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CheckCode
		{
			set{ _checkcode=value;}
			get{return _checkcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Token
		{
			set{ _token=value;}
			get{return _token;}
		}
		#endregion Model

	}
}

