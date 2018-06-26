using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.web.goods
{
    public partial class editor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage)||WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))
            {
                if (!IsPostBack)
                {
                    BindDdl();
                }
            }
            else
            {
                Response.Redirect("/web/web_login.aspx", false);
            }
        }
        public void BindDdl()
        {
            string strWhere = " SELECT * FROM M_Category WHERE 1=1 AND IsDelete=0 ORDER BY ParentID ASC , OrderBy ASC ";
            IEnumerable<dynamic> CateInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList(strWhere, null);
            new DdlHelper().BindDdlMore(sltByCateName, CateInfo, "全部", x => { return new ListItem(Convert.ToString(x.CateName), Convert.ToString(x.CateID)); });
            IEnumerable<dynamic> SuppliersInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList("SELECT * FROM M_Suppliers WHERE 1=1", null);
            new DdlHelper().BindDdlMore(sltBySuppliers, SuppliersInfo, "全部", x => { return new ListItem(Convert.ToString(x.SuppliersName), Convert.ToString(x.SuppliersID)); });
            IEnumerable<dynamic> FareInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList(" SELECT FareName,FareSysID FROM M_FareTemplate WHERE 1=1  ", null);
            new DdlHelper().BindDdlMore(updateFareTemp, FareInfo, "请选择", x => { return new ListItem(Convert.ToString(x.FareName), Convert.ToString(x.FareSysID)); });
            string goodsSysID = ToolHelper.UrlParDecode(Request.QueryString["byGoodsSysID"], "");
            if (!string.IsNullOrEmpty(goodsSysID))
            {
                DB.Model.M_Goods dtGoodsInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<DB.Model.M_Goods>(" GoodsID=@_GoodsID ",new { _GoodsID=goodsSysID });
                if (dtGoodsInfo != null)
                {
                    thisGoodsName.InnerHtml = dtGoodsInfo.GoodsName+"--"+dtGoodsInfo.GoodsSn;
                    myContent.InnerHtml= dtGoodsInfo.GoodsDesc;
                    saveInfo.Attributes.Add("mydata", goodsSysID);
                    myGoodsName.Value = dtGoodsInfo.GoodsName;
                    sltByCateName.Value = Convert.ToString(dtGoodsInfo.CateID);
                    sltBySuppliers.Value = Convert.ToString(dtGoodsInfo.SuppliersID);
                    shopPrice.Value = Convert.ToString(dtGoodsInfo.ShopPrice);
                    marketPrice.Value = Convert.ToString(dtGoodsInfo.MarketPrice);
                    myGoodsBrief.InnerText = Convert.ToString(dtGoodsInfo.GoodsBrief);
                    myRemark.InnerText = Convert.ToString(dtGoodsInfo.SellerNote);
                    myKeyWords.Value = dtGoodsInfo.KeyWords;
                    updateFareTemp.Value = dtGoodsInfo.FareSysID;
                    updateBrand.Attributes.Add("mydata", Convert.ToString(dtGoodsInfo.BrandID));
                }
            }
            HttpCookie cok = Request.Cookies["editorUrl"];
            if (cok != null)
            {
                thisUrl.Attributes["href"] = HttpUtility.UrlDecode(cok.Value);
            }
            if (string.IsNullOrEmpty(goodsSysID))
            {
                Response.Redirect("/web/goods/goods_list.aspx?", false);
            }
        }
    }
}