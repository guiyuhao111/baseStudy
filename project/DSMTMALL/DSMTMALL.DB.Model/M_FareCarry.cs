/**M_FareCarry.cs*/
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_FareCarry:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_FareCarry
	{
		public M_FareCarry()
		{}
		#region Model
		private int _carrysysid;
		private string _faresysid;
        private int _faretype=20;
		private string _carryaddresslist;
		private double _fistcount=15;
		private double _fistweight=1.00;
		private double _fistvolume=1.00;
		private decimal _fistmoney= 15.00M;
		private double _continuecount=2;
		private double _continueweight=2.00;
		private double _continuevolume=5.00;
		private decimal _continuemoney=10.00M;
		private string _carryway= "快递";
		private int _isdefault=0;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int CarrySysID
		{
			set{ _carrysysid=value;}
			get{return _carrysysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FareSysID
		{
			set{ _faresysid=value;}
			get{return _faresysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarryAddressList
		{
			set{ _carryaddresslist=value;}
			get{return _carryaddresslist;}
		}
		/// <summary>
		/// 
		/// </summary>
		public double FirstCount
		{
			set{ _fistcount=value;}
			get{return _fistcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public double FirstWeight
		{
			set{ _fistweight=value;}
			get{return _fistweight;}
		}
		/// <summary>
		/// 
		/// </summary>
		public double FirstVolume
		{
			set{ _fistvolume=value;}
			get{return _fistvolume;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal FirstMoney
		{
			set{ _fistmoney=value;}
			get{return _fistmoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public double ContinueCount
		{
			set{ _continuecount=value;}
			get{return _continuecount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public double ContinueWeight
		{
			set{ _continueweight=value;}
			get{return _continueweight;}
		}
		/// <summary>
		/// 
		/// </summary>
		public double ContinueVolume
		{
			set{ _continuevolume=value;}
			get{return _continuevolume;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal ContinueMoney
		{
			set{ _continuemoney=value;}
			get{return _continuemoney;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CarryWay
		{
			set{ _carryway=value;}
			get{return _carryway;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsDefault
		{
			set{ _isdefault=value;}
			get{return _isdefault;}
		}

        public int FareType
        {
            get
            {
                return _faretype;
            }

            set
            {
                _faretype = value;
            }
        }
        #endregion Model

    }
}

