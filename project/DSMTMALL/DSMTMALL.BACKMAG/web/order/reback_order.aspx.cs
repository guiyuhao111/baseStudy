using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.web.order
{
    public partial class reback_order : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                if (!IsPostBack)
                {
                    if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
                    {
                        BindDdl();
                        BindGrid(sltBySuppliers.Value.Trim(), sltByPayID.Value.Trim(),sltByReBackType.Value.Trim(),sltAuthType.Value.Trim(), sltOrderBy.Value.Trim());
                    }
                }
            }
            else
            {
                Response.Redirect("/web/web_login.aspx", false);
            }

        }

        public void BindDdl()
        {
            IEnumerable<dynamic> paymentInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList("SELECT * FROM M_Payment WHERE 1=1 AND IsEnabled =1 ORDER BY PayOrder DESC", null);
            new DdlHelper().BindDdlMore(sltByPayID, paymentInfo, "全部", x => { return new ListItem(Convert.ToString(x.PayName), Convert.ToString(x.PayID)); });
            IEnumerable<dynamic> SuppliersInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList(" SELECT * FROM M_Suppliers WHERE 1=1 AND IsCheck =1  ORDER BY SuppliersID ASC ", null);
            new DdlHelper().BindDdlMore(sltBySuppliers, SuppliersInfo, "全部", x => { return new ListItem(Convert.ToString(x.SuppliersName), Convert.ToString(x.SuppliersID)); });
            if (!string.IsNullOrEmpty(Request.QueryString["byPayID"]))
            {
                sltByPayID.SelectedIndex = sltByPayID.Items.IndexOf(sltByPayID.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byPayID"], "")));
            }
            if (!string.IsNullOrEmpty(Request.QueryString["bySuppliers"]))
            {
                sltBySuppliers.SelectedIndex = sltBySuppliers.Items.IndexOf(sltBySuppliers.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["bySuppliers"], "")));
            }
            if (!string.IsNullOrEmpty(Request.QueryString["byReBackType"]))
            {
                sltByReBackType.SelectedIndex = sltByReBackType.Items.IndexOf(sltByReBackType.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byReBackType"], "")));
            }
            if (!string.IsNullOrEmpty(Request.QueryString["byAuthType"]))
            {
                sltAuthType.SelectedIndex = sltAuthType.Items.IndexOf(sltAuthType.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byAuthType"], "")));
            }
            if (!string.IsNullOrEmpty(Request.QueryString["byOrderBy"]))
            {
                sltOrderBy.SelectedIndex = sltOrderBy.Items.IndexOf(sltOrderBy.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byOrderBy"], "")));
            }
        }

        public void BindGrid(string suppliersID, string payID,string byReBackType,string byAuthType ,string sltOrderBy)
        {
            object dt = GetGoodsList(suppliersID, payID,byReBackType,byAuthType, sltOrderBy);
            repeaterOrderList.DataSource = dt;
            repeaterOrderList.DataBind();
        }

        public object GetGoodsList(string suppliersID, string payID, string byReBackType, string byAuthType,string sltOrderBy)
        {
            string devSort = string.Empty;
            string searchName = ToolHelper.UrlParDecode(Request.QueryString["searchName"], "");
            string byTimeStart = ToolHelper.UrlParDecode(Request.QueryString["byTimeStart"], "");
            string byTimeEnd = ToolHelper.UrlParDecode(Request.QueryString["byTimeEnd"], "");
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            if (sltOrderBy == "0")
            {
                devSort = " ORDER BY O.PayTime DESC ";
            }
            if (sltOrderBy == "1")
            {
                devSort = " ORDER BY R.CreatTime DESC ";
            }
            if (sltOrderBy == "2")
            {
                devSort = " ORDER BY O.UpdateTime DESC ";
            }
            iptSearch.Value = searchName;
            sltByTimeStart.Value = byTimeStart;
            sltByTimeEnd.Value = byTimeEnd;
            int pageSize = 20;
            int listCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageReBackOrderListCount(byTimeStart, byTimeEnd, suppliersID, payID,byReBackType,byAuthType, searchName);
            string url = "/web/order/order_list.aspx?";
            url += "byTimeStart=" + ToolHelper.UrlParEncode(byTimeStart, "") + "&";
            url += "byTimeEnd=" + ToolHelper.UrlParEncode(byTimeEnd, "") + "&";
            url += "byPayID=" + ToolHelper.UrlParEncode(payID, "") + "&";
            url += "byReBackType=" + ToolHelper.UrlParEncode(byReBackType, "") + "&";
            url += "byAuthType=" + ToolHelper.UrlParEncode(byAuthType, "") + "&";
            url += "bySuppliers=" + ToolHelper.UrlParEncode(suppliersID, "") + "&";
            url += "byOrderBy=" + ToolHelper.UrlParEncode(sltOrderBy, "") + "&";
            url += "searchName=" + ToolHelper.UrlParEncode(searchName, "") + "&";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageReBackOrderList(byTimeStart, byTimeEnd, suppliersID, payID, byReBackType, byAuthType, searchName, devSort, (pageNow - 1) * pageSize, pageSize);
            PageNavHelper pageNavHelper = new PageNavHelper(listCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }
    }
}