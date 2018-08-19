/**M_AdminAction.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_AdminAction:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_AdminAction
	{
		public M_AdminAction()
		{}
		#region Model
		private int _authorityid;
		private string _modalsysid;
		private string _adminsysid;
		private DateTime _createtime= Convert.ToDateTime("1970-01-01 00:00:00");
		/// <summary>
		/// auto_increment
		/// </summary>
		public int AuthorityID
		{
			set{ _authorityid=value;}
			get{return _authorityid;}
		}
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
		public string AdminSysID
		{
			set{ _adminsysid=value;}
			get{return _adminsysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

