using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DSMTMALL.BACKMAG.web.database
{
    public partial class imges : System.Web.UI.Page
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
            repeaterImgList.DataSource = dt;
            repeaterImgList.DataBind();
        }

        public object GetBrandList()
        {
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            int pageSize = 18;
            string url = "/web/database/imges.aspx?";
            int listCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelListCount<DB.Model.M_FileManage>("1=1 AND FileName NOT LIKE '%Themb%'", null);
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<DB.Model.M_FileManage>(" 1=1 AND FileName NOT LIKE '%Themb%' ORDER BY CreatTime ASC LIMIT @PageNow ,@PageSize ", new { PageNow = (pageNow - 1) * pageSize, pageSize = pageSize });//.Select(info => new { FilePath = "http://mallmanage.51ipc.com/" + info.FilePath, info.FileName,info.FileSysID });
            PageNavHelper pageNavHelper = new PageNavHelper(listCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }

    }
}