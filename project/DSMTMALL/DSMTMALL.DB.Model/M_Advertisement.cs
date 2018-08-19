using System;
using System.Collections.Generic;
namespace DSMTMALL.DB.Model
{
    /// <summary>
    /// M_Advertisement:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class M_Advertisement
    {
        public M_Advertisement()
        { }
        #region Model
        private int _advsysid;
        private string _advname;
        private string _advdesc;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int AdvSysID
        {
            set { _advsysid = value; }
            get { return _advsysid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AdvName
        {
            set { _advname = value; }
            get { return _advname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AdvDesc
        {
            set { _advdesc = value; }
            get { return _advdesc; }
        }
        #endregion Model

    }
}
