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
    public partial class log_error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                if (!IsPostBack)
                {
                    BindGrid();
                }
            }else
            {
                Server.Transfer("/web/web_login.aspx");
            }
        }

        public void BindGrid()
        {
            repeaterErrLogList.DataSource = GetList();
            repeaterErrLogList.DataBind();
        }

        public object GetList()
        {
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            int pageSize = 20;
            int devCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelListCount<DB.Model.M_ErrorLog>(" 1=1 ",null);
            string url = "/web/database/log_error.aspx?";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<DB.Model.M_ErrorLog>(" 1=1 ORDER BY ErrLogTime DESC LIMIT @StartSize,@EndSize ",new { StartSize=(pageNow - 1) * pageSize, EndSize= pageSize });
            PageNavHelper pageNavHelper = new PageNavHelper(devCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }
    }
}