using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.MyPublic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.web
{
    public partial class user_address_editor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindDl();
        }

        public void BindDl()
        {
            //DataTable regionInfo = new DB.BLL.M_Region(DBEnum.Slave).GetList(" ParentID = 1 ").Tables[0];
            //new DdlHelper().BindDdlMore(ddlUserProvince, regionInfo, "RegionName", "RegionID", "省份");
        }
    }
}