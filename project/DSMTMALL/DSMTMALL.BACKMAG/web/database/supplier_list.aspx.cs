using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.web.database
{
    public partial class supplier_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                if (!IsPostBack)
                {
                    BindGrid();
                }
            }
            else
            {
                Response.Redirect("/web/web_login.aspx", false);
            }
        }
        public void BindGrid()
        {
            object dt = GetBrandList();
            repeaterSuppliersList.DataSource = dt;
            repeaterSuppliersList.DataBind();
        }

        public object GetBrandList()
        {
            string searchName = ToolHelper.UrlParDecode(Request.QueryString["searchName"], "");
            iptSearch.Value = searchName;
            string url = "/web/database/supplier_list.aspx?";
            url += "searchName=" + ToolHelper.UrlParEncode(searchName, "") + "&";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<DB.Model.M_Suppliers>(" 1=1 ",null);
            return dt;
        }
    }
}