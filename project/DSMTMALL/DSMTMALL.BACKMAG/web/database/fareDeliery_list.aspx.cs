using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Data;
using System.Linq;

namespace DSMTMALL.BACKMAG.web.database
{
    public partial class fareDeliery_list : System.Web.UI.Page
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


        public void BindDdl()
        {

        }

        public void BindGrid()
        {
            object dt = GetBrandList();
            repeaterFareList.DataSource = dt;
            repeaterFareList.DataBind();
        }

        public object GetBrandList()
        {
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            int pageSize = 5;
            int listCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelListCount(" SELECT COUNT(*) FROM M_FareTemplate ", null);
            string url = "/web/database/fareDeliery_list.aspx?";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList(" SELECT * FROM M_FareTemplate WHERE 1=1 LIMIT @StartSize,@EndSize", new { StartSize=(pageNow - 1) * pageSize,EndSize= pageSize }).Select(info => { return new { info.FareName, info.FareSysID, info.UpdateTime }; });
            PageNavHelper pageNavHelper = new PageNavHelper(listCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }



    }


}