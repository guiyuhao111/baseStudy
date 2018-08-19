using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.web.goods
{
    public partial class goods_recom : System.Web.UI.Page
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
            object dt = GetGoodsList();
            repeaterOrderList.DataSource = dt;
            repeaterOrderList.DataBind();
        }

        public object GetGoodsList()
        {
            string devSort = string.Empty;
            string searchName = ToolHelper.UrlParDecode(Request.QueryString["searchName"], "");
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            iptSearch.Value = searchName;
            int pageSize = 10;
            string url = "/web/goods/goods_recom.aspx?";
            url += "searchName=" + ToolHelper.UrlParEncode(searchName, "") + "&";
            int listCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageGoodsRecomListCount(searchName);
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageGoodsRecomList(searchName, (pageNow - 1) * pageSize, pageSize);
            PageNavHelper pageNavHelper = new PageNavHelper(listCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }
    }
}