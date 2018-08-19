
using System;
namespace DSMTMALL.DB.Model
{
	/// <summary>
	/// M_Goods:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Goods
	{
		public M_Goods()
		{}
		#region Model
		private string _goodsid;
		private int _cateid=0;
		private int _brandid=0;
		private int _suppliersid=0;
		private string _goodssn;
		private string _goodsname;
		private int _clickcount=0;
		private int _goodsnumber=0;
		private decimal _marketprice=0.00M;
		private decimal _shopprice=0.00M;
		private int _warnnumber=1;
		private string _keywords;
		private string _goodsbrief;
		private string _goodsdesc;
		private string _goodsthumb;
		private string _goodsimg;
		private string _originalimg;
		private int _isreal=1;
		private int _isenable=1;
		private DateTime _addtime= Convert.ToDateTime("1970-01-01 00:00:00");
		private int _orderby=100;
		private int _isdelete=0;
		private int _isbest=0;
		private int _isnew=0;
		private int _ishot=0;
		private int _ispromote=0;
		private DateTime _lastupdate= Convert.ToDateTime("1970-01-01 00:00:00");
		private string _sellernote;
		private int _salenumber=0;
        private int _quotanumber = 0;
        private int _version = 0;
        private double _weight = 0;
        private string _faresysid;
        private string _modifyperson;
        private int _IsUpdateBySupplier = 0;
        /// <summary>
        /// 
        /// </summary>
        public string GoodsID
		{
			set{ _goodsid=value;}
			get{return _goodsid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int CateID
		{
			set{ _cateid=value;}
			get{return _cateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int BrandID
		{
			set{ _brandid=value;}
			get{return _brandid;}
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
		public string GoodsSn
		{
			set{ _goodssn=value;}
			get{return _goodssn;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GoodsName
		{
			set{ _goodsname=value;}
			get{return _goodsname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ClickCount
		{
			set{ _clickcount=value;}
			get{return _clickcount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GoodsNumber
		{
			set{ _goodsnumber=value;}
			get{return _goodsnumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal MarketPrice
		{
			set{ _marketprice=value;}
			get{return _marketprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal ShopPrice
		{
			set{ _shopprice=value;}
			get{return _shopprice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int WarnNumber
		{
			set{ _warnnumber=value;}
			get{return _warnnumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string KeyWords
		{
			set{ _keywords=value;}
			get{return _keywords;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GoodsBrief
		{
			set{ _goodsbrief=value;}
			get{return _goodsbrief;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GoodsDesc
		{
			set{ _goodsdesc=value;}
			get{return _goodsdesc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GoodsThumb
		{
			set{ _goodsthumb=value;}
			get{return _goodsthumb;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GoodsImg
		{
			set{ _goodsimg=value;}
			get{return _goodsimg;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OriginalImg
		{
			set{ _originalimg=value;}
			get{return _originalimg;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsReal
		{
			set{ _isreal=value;}
			get{return _isreal;}
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
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
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
		public int IsDelete
		{
			set{ _isdelete=value;}
			get{return _isdelete;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsBest
		{
			set{ _isbest=value;}
			get{return _isbest;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsNew
		{
			set{ _isnew=value;}
			get{return _isnew;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsHot
		{
			set{ _ishot=value;}
			get{return _ishot;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int IsPromote
		{
			set{ _ispromote=value;}
			get{return _ispromote;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime LastUpdate
		{
			set{ _lastupdate=value;}
			get{return _lastupdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SellerNote
		{
			set{ _sellernote=value;}
			get{return _sellernote;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int SaleNumber
		{
			set{ _salenumber=value;}
			get{return _salenumber;}
		}

        public int Version
        {
            get
            {
                return _version;
            }

            set
            {
                _version = value;
            }
        }

        public double Weight
        {
            get
            {
                return _weight;
            }

            set
            {
                _weight = value;
            }
        }

        public string FareSysID
        {
            get
            {
                return _faresysid;
            }

            set
            {
                _faresysid = value;
            }
        }

        public string ModifyPerson
        {
            get
            {
                return _modifyperson;
            }

            set
            {
                _modifyperson = value;
            }
        }

        public int IsUpdateBySupplier
        {
            get
            {
                return _IsUpdateBySupplier;
            }

            set
            {
                _IsUpdateBySupplier = value;
            }
        }

        public int QuotaNumber
        {
            get
            {
                return _quotanumber;
            }

            set
            {
                _quotanumber = value;
            }
        }
        #endregion Model

    }
}

