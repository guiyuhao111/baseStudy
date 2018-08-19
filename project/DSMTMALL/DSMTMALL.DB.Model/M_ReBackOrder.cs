/**  版本信息模板在安装目录下，可自行修改。
* M_ReBackOrder.cs
*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_ReBackOrder:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_ReBackOrder
	{
		public M_ReBackOrder()
		{}
		#region Model
		private string _rebackid;
		private string _orderid;
		private string _ordersn;
		private string _orderunifysn;
		private int _rebacktype=10;
		private int _authtype=10;
		private string _reconnection;
		private string _rebackaddress;
		private string _rebackdesc;
		private string _rebackremark;
		private string _logistical;
		private string _logisticalnumber;
		private DateTime _creattime= Convert.ToDateTime("1970-01-01 00:00:00");
		private DateTime _updatetime= Convert.ToDateTime("1970-01-01 00:00:00");
		private string _modifyperson;
		private string _userid;
		private string _openid;
		private string _rebacktradesysid;
        private string _rlogistical;
        private string _rlogisticalNumber;
        /// <summary>
        /// 
        /// </summary>
        public string ReBackID
		{
			set{ _rebackid=value;}
			get{return _rebackid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrderSn
		{
			set{ _ordersn=value;}
			get{return _ordersn;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrderUnifySn
		{
			set{ _orderunifysn=value;}
			get{return _orderunifysn;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ReBackType
		{
			set{ _rebacktype=value;}
			get{return _rebacktype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AuthType
		{
			set{ _authtype=value;}
			get{return _authtype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReConnection
		{
			set{ _reconnection=value;}
			get{return _reconnection;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReBackAddress
		{
			set{ _rebackaddress=value;}
			get{return _rebackaddress;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReBackDesc
		{
			set{ _rebackdesc=value;}
			get{return _rebackdesc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReBackRemark
		{
			set{ _rebackremark=value;}
			get{return _rebackremark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Logistical
		{
			set{ _logistical=value;}
			get{return _logistical;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LogisticalNumber
		{
			set{ _logisticalnumber=value;}
			get{return _logisticalnumber;}
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
		public DateTime UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
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
		public string UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OpenID
		{
			set{ _openid=value;}
			get{return _openid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ReBackTradeSysID
		{
			set{ _rebacktradesysid=value;}
			get{return _rebacktradesysid;}
		}

        public string RLogistical
        {
            get
            {
                return _rlogistical;
            }
            set
            {
                _rlogistical = value;
            }
        }

        public string RLogisticalNumber
        {
            get
            {
                return _rlogisticalNumber;
            }

            set
            {
                _rlogisticalNumber = value;
            }
        }
        #endregion Model

    }
}

