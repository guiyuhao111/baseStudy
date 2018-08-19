using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DSMTMALL.BACKMAG.web.admin
{
    public partial class web_user_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))
                {
                    BindDdl();
                    BindGrid(sltByUserType.Value.Trim());
                }
            }
        }
        private void BindDdl()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["byUserType"]))
            {
                sltByUserType.SelectedIndex = sltByUserType.Items.IndexOf(sltByUserType.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byUserType"], "")));
            }
            int count = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelListCount<DB.Model.M_Users>("  RegTime>@_RegTime ", new { _RegTime=Convert.ToDateTime( DateTime.Now.ToString("yyyy-MM-dd"))});
            register_count.InnerHtml = Convert.ToString(count);
        }

        private void BindGrid(string byUserType)
        {
            repeaterUserList.DataSource = GetUserList(byUserType);
            repeaterUserList.DataBind();
        }

        private object GetUserList(string byUserType) {
            string searchName = ToolHelper.UrlParDecode(Request.QueryString["searchName"], "");
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            iptSearch.Value = searchName;
            int pageSize = 50;
            int devCount =0;
            string url = "/web/admin/web_user_list.aspx?";
            url += "byScreenType=" + ToolHelper.UrlParEncode(byUserType, "") + "&";
            url += "searchName=" + ToolHelper.UrlParEncode(searchName, "") + "&";
            IEnumerable<dynamic> dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageUserList(byUserType,searchName, (pageNow - 1) * pageSize, pageSize,out devCount);
            PageNavHelper pageNavHelper = new PageNavHelper(devCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            int i = 1*pageSize*(pageNow-1)+1;
            return dt.Select(info => {
                return new
                {
                    RowNo=i++,
                    info.UserName,
                    info.NickName,
                    info.Phone,
                    info.RegTime,
                    info.LastLogin,
                    info.CpyName,
                    info.NSex
                };
            }); ;
        }
    }
}