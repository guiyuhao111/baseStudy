/**M_Users.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_Users:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Users
	{
		public M_Users()
		{}
		#region Model
		private string _userid;
		private string _username;
		private string _nickname;
		private string _password;
		private string _phone;
		private string _usercardno;
		private decimal _usermoney=0.00M;
		private string _platformsysid;
		private string _platformtype;
		private int _sex=0;
		private string _email;
		private int _mistakenum=0;
		private string _question;
		private string _answer;
		private DateTime _birthday= Convert.ToDateTime("1970-01-01");
		private decimal _frozenmoney=0.00M;
		private int _paypoints=0;
		private int _rankpoints=0;
		private int _addressid=0;
		private DateTime _regtime= Convert.ToDateTime("1970-01-01 00:00:00");
		private DateTime _lastlogin= Convert.ToDateTime("1970-01-01 00:00:00");
		private DateTime _lasttime= Convert.ToDateTime("1970-01-01 00:00:00");
		private string _lastip;
		private int _userrank=0;
		private int _isspecial=0;
		private int _isenable=0;
		private decimal _creditline=0.00M;
		private string _cpyname;
		private string _cpysysid;
		private string _openid;
		private string _cardpaypwd;
		private string _qcardpaypwd;
        private string _simplename;
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
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string NickName
		{
			set{ _nickname=value;}
			get{return _nickname;}
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
		public string UserCardNo
		{
			set{ _usercardno=value;}
			get{return _usercardno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal UserMoney
		{
			set{ _usermoney=value;}
			get{return _usermoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PlatFormSysID
		{
			set{ _platformsysid=value;}
			get{return _platformsysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PlatFormType
		{
			set{ _platformtype=value;}
			get{return _platformtype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Sex
		{
			set{ _sex=value;}
			get{return _sex;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Email
		{
			set{ _email=value;}
			get{return _email;}
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
		public string Question
		{
			set{ _question=value;}
			get{return _question;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Answer
		{
			set{ _answer=value;}
			get{return _answer;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime Birthday
		{
			set{ _birthday=value;}
			get{return _birthday;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal FrozenMoney
		{
			set{ _frozenmoney=value;}
			get{return _frozenmoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int PayPoints
		{
			set{ _paypoints=value;}
			get{return _paypoints;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RankPoints
		{
			set{ _rankpoints=value;}
			get{return _rankpoints;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AddressID
		{
			set{ _addressid=value;}
			get{return _addressid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime RegTime
		{
			set{ _regtime=value;}
			get{return _regtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime LastLogin
		{
			set{ _lastlogin=value;}
			get{return _lastlogin;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime LastTime
		{
			set{ _lasttime=value;}
			get{return _lasttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LastIP
		{
			set{ _lastip=value;}
			get{return _lastip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int UserRank
		{
			set{ _userrank=value;}
			get{return _userrank;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsSpecial
		{
			set{ _isspecial=value;}
			get{return _isspecial;}
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
		public decimal CreditLine
		{
			set{ _creditline=value;}
			get{return _creditline;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CpyName
		{
			set{ _cpyname=value;}
			get{return _cpyname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CpySysID
		{
			set{ _cpysysid=value;}
			get{return _cpysysid;}
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
		public string CardPayPwd
		{
			set{ _cardpaypwd=value;}
			get{return _cardpaypwd;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string QCardPayPwd
		{
			set{ _qcardpaypwd=value;}
			get{return _qcardpaypwd;}
		}

        public string SimpleName
        {
            get
            {
                return _simplename;
            }

            set
            {
                _simplename = value;
            }
        }
        #endregion Model

    }
}

