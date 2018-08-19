using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_FareTemplate:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_FareTemplate
	{
		public M_FareTemplate()
		{}
		#region Model
		private string _faresysid;
		private string _farename;
		private string _fareaddress;
		private int _faretype=20;
		private int _faredeliery=0;
		private int _faredelierysysid=0;
        private DateTime _updatetime = Convert.ToDateTime("1970-01-01 00:00:00");
        private int _faretime = 48;
        /// <summary>
        /// auto_increment
        /// </summary>
        public string FareSysID
		{
			set{ _faresysid=value;}
			get{return _faresysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FareName
		{
			set{ _farename=value;}
			get{return _farename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FareAddress
		{
			set{ _fareaddress=value;}
			get{return _fareaddress;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int FareType
		{
			set{ _faretype=value;}
			get{return _faretype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int FareDeliery
		{
			set{ _faredeliery=value;}
			get{return _faredeliery;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int FareDelierySysID
		{
			set{ _faredelierysysid=value;}
			get{return _faredelierysysid;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int FareTime
        {
            set { _faretime = value; }
            get { return _faretime; }
        }

        public DateTime UpdateTime
        {
            get
            {
                return _updatetime;
            }

            set
            {
                _updatetime = value;
            }
        }
        #endregion Model

    }
}

