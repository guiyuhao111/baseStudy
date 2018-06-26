using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.web.goods
{
    public partial class goods_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                if (!IsPostBack)
                {
                    BindDdl();
                    BindGrid(sltByCateType.Value.Trim(), sltBySuppliers.Value.Trim(), sltByIsEnable.Value.Trim(), sltOrderBy.Value.Trim());
                }
                else
                {
                    Response.Redirect("/web/web_login.aspx", false);
                }
            }
        }

        public void BindDdl()
        {
            string strWhere = " SELECT * FROM M_Category WHERE 1=1 AND IsDelete=0 ORDER BY ParentID ASC , OrderBy ASC ";
            IEnumerable<dynamic> CateInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList(strWhere,null);
            new DdlHelper().BindDdlMore(sltByCateType, CateInfo,"全部", x =>{ return new ListItem(Convert.ToString(x.CateName), Convert.ToString(x.CateID)); });
            IEnumerable<dynamic> SuppliersInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList("SELECT * FROM M_Suppliers WHERE 1=1", null);
            new DdlHelper().BindDdlMore(sltBySuppliers,SuppliersInfo, "全部", x => { return new ListItem(Convert.ToString(x.SuppliersName),Convert.ToString(x.SuppliersID)); });
            new DdlHelper().BindDdlMore(addSuppliers,SuppliersInfo,"请选择",x=> { return new ListItem(Convert.ToString(x.SuppliersName), Convert.ToString(x.SuppliersID)); });
            IEnumerable<dynamic> FareInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList(" SELECT FareName,FareSysID FROM M_FareTemplate WHERE 1=1  ",null);
            new DdlHelper().BindDdlMore(addFareTemp,FareInfo,"请选择",x=> { return new ListItem(Convert.ToString(x.FareName), Convert.ToString(x.FareSysID)); });
            if (!string.IsNullOrEmpty(Request.QueryString["byCateType"]))
            {
                sltByCateType.SelectedIndex = sltByCateType.Items.IndexOf(sltByCateType.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byCateType"], "")));
            }
            if (!string.IsNullOrEmpty(Request.QueryString["bySuppliers"]))
            {
                sltBySuppliers.SelectedIndex = sltBySuppliers.Items.IndexOf(sltBySuppliers.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["bySuppliers"], "")));
            }
            if (!string.IsNullOrEmpty(Request.QueryString["byIsEnable"]))
            {
                sltByIsEnable.SelectedIndex = sltByIsEnable.Items.IndexOf(sltByIsEnable.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byIsEnable"], "")));
            }
            if (!string.IsNullOrEmpty(Request.QueryString["byOrderBy"]))
            {
                sltOrderBy.SelectedIndex = sltOrderBy.Items.IndexOf(sltOrderBy.Items.FindByValue(ToolHelper.UrlParDecode(Request.QueryString["byOrderBy"], "")));
            }
            if (File.Exists(Server.MapPath("/files/excel/上传错误信息表.xls")))
            {
                downLoad.Attributes["href"] = "/files/excel/上传错误信息表.xls";
                downLoad.Style.Remove("display");
            }
        }

        public void BindGrid(string byCateType ,string bySuppliers,string byIsEnable , string byOrderBy)
        {
            object dt = GetGoodsList(byCateType, bySuppliers, byIsEnable, byOrderBy);
            repeaterGoodsList.DataSource = dt;
            repeaterGoodsList.DataBind();
        }

        public object GetGoodsList(string byCateType, string bySuppliers, string byIsEnable, string byOrderBy)
        {
            string devSort = string.Empty;
            string searchName = ToolHelper.UrlParDecode(Request.QueryString["searchName"], "");
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            if(byOrderBy =="0")
            {
                devSort = " ORDER BY GoodsNumber ASC, OrderBy ASC,GoodsID ";
            }
            if(byOrderBy == "1")
            {
                devSort = " ORDER BY ShopPrice ASC ,OrderBy ASC,GoodsID ";
            }
            if(byOrderBy == "2")
            {
                devSort = "  ORDER BY SaleNumber DESC ,OrderBy ASC,GoodsID ";
            }
            if(byOrderBy == "3")
            {
                devSort = " ORDER BY AddTime DESC ,OrderBy ASC,GoodsID ";
            }
            iptSearch.Value = searchName;
            int pageSize = 10;
            int listCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageGoodsListCount(byCateType,bySuppliers,byIsEnable,searchName);
            string url = "/web/goods/goods_list.aspx?";
            url += "byCateType=" + ToolHelper.UrlParEncode(byCateType, "") + "&";
            url += "bySuppliers=" + ToolHelper.UrlParEncode(bySuppliers, "") + "&";
            url += "byIsEnable=" + ToolHelper.UrlParEncode(byIsEnable, "") + "&";
            url += "searchName=" + ToolHelper.UrlParEncode(searchName, "") + "&";
            url += "byOrderBy=" + ToolHelper.UrlParEncode(byOrderBy, "") + "&";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageGoodsList(byCateType, bySuppliers, byIsEnable, devSort, searchName, (pageNow - 1) * pageSize, pageSize);
            PageNavHelper pageNavHelper = new PageNavHelper(listCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }


    }
}