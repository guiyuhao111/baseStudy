/**M_Roll.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_Roll:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Roll
	{
		public M_Roll()
		{}
		#region Model
		private int _rollsysid;
		private string _targetsysid;
        private string _targeturl;
		private int _rolltype=30;
		private string _picture;
		private int _isenable=0;
		private int _orderby=50;
		private string _modifyperson;
		private DateTime _updatetime= Convert.ToDateTime("1970-01-01 00:00:00");
		private DateTime _createtime= Convert.ToDateTime("1970-01-01 00:00:00");
		/// <summary>
		/// auto_increment
		/// </summary>
		public int RollSysID
		{
			set{ _rollsysid=value;}
			get{return _rollsysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TargetSysID
		{
			set{ _targetsysid=value;}
			get{return _targetsysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RollType
		{
			set{ _rolltype=value;}
			get{return _rolltype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Picture
		{
			set{ _picture=value;}
			get{return _picture;}
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
		public int OrderBy
		{
			set{ _orderby=value;}
			get{return _orderby;}
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

        public string TargetUrl
        {
            get
            {
                return _targeturl;
            }

            set
            {
                _targeturl = value;
            }
        }
        #endregion Model

    }
}

