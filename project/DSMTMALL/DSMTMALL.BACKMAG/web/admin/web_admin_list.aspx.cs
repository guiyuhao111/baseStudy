using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.web.admin
{
    public partial class web_admin_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
            {
                if (!IsPostBack)
                {
                    BindGrid();
                    IEnumerable<dynamic> SuppliersInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList("SELECT * FROM M_Suppliers WHERE 1=1", null);
                    new DdlHelper().BindDdlMore(addSuppliers, SuppliersInfo, "请选择", x => { return new ListItem(Convert.ToString(x.SuppliersName), Convert.ToString(x.SuppliersID)); });
                }
            }else
            {
                Response.Redirect("/web/web_login.aspx", false);
            }
        }

        public void BindGrid()
        {
            object dt = GetBrandList();
            repeaterAdminList.DataSource = dt;
            repeaterAdminList.DataBind();
        }

        public object GetBrandList()
        {
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            int pageSize = 15;
            int listCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageAdminListCount(string.Empty);
            string url = "/web/admin/web_admin_list.aspx?";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageAdminList((pageNow - 1) * pageSize, pageSize);
            PageNavHelper pageNavHelper = new PageNavHelper(listCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }

    }
}