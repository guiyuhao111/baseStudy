using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.MyPublic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.web
{
    public partial class goods_show : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
        }
        private void BindGrid()
        {
            try
            {
                string goodsSysID = Request.QueryString["goodsId"].Trim();
                string url = WebToolHelper.GetProfilesUrl();
                if (!string.IsNullOrEmpty(goodsSysID))
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(" SELECT *,CONCAT(@PathAdd,GLY.ImgUrl) AS NImgUrl FROM M_GoodsGallery AS GLY WHERE GLY.GoodsID =@GoodsID ");
                    object dt = new DB.BLL.MY_Bll(DBEnum.Slave).GetModelList(strSql.ToString(),new { PathAdd=url, GoodsID =goodsSysID}).Select(info=> { return new { info.NImgUrl }; });
                    repeaterGoodsPicetureShow.DataSource = dt;
                    repeaterGoodsPicetureShow.DataBind();
                    strSql.Clear();
                    strSql.Append(" SELECT G.GoodsID,G.GoodsSn,G.GoodsName,G.BrandID,G.Weight,G.GoodsBrief,G.GoodsDesc,G.MarketPrice,G.ShopPrice,G.QuotaNumber,G.GoodsNumber,B.BrandName,S.SuppliersName,C.CateName,G.SaleNumber,FT.FareTime,");
                    strSql.Append(" CASE WHEN FT.FareType=10 THEN '按件数计费' WHEN FT.FareType= 20 THEN '按重量计费' WHEN FT.FareType= 30 THEN '按体积计费' ELSE '未指定运费模板' END NFareType , ");
                    strSql.Append(" CONCAT('默认运费:',ROUND(FC.FirstCount),'(件/公斤)内:',FC.FirstMoney,'元，','续:',FC.ContinueMoney,'/(件/公斤)')  AS NFareInfo ");
                    strSql.Append(" FROM M_Goods AS G ");
                    strSql.Append(" LEFT JOIN M_Brand AS B ON B.BrandID = G.BrandID  ");
                    strSql.Append(" LEFT JOIN M_Category AS C ON G.CateID = C.CateID ");
                    strSql.Append(" LEFT JOIN M_Suppliers AS S ON S.SuppliersID = G.SuppliersID ");
                    strSql.Append(" LEFT JOIN M_FareTemplate AS FT ON G.FareSysID= FT.FareSysID ");
                    strSql.Append(" LEFT JOIN M_FareCarry AS FC ON G.FareSysID =FC.FareSysID ");
                    strSql.Append(" WHERE G.GoodsID = @GoodsID AND G.IsDelete=0 ORDER BY FC.IsDefault DESC LIMIT 1 ");
                    IEnumerable<dynamic> goodsInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModelList(strSql.ToString(), new { GoodsID = goodsSysID });
                    string fileUrl = WebToolHelper.GetProfilesUrl();
                    foreach (var item in goodsInfo)
                    {
                        item.GoodsDesc =  WebToolHelper.ReplaceImgUrl_1(item.GoodsDesc.ToString(), fileUrl);
                    }
                    repeaterGoodsInfo.DataSource = goodsInfo.Select(info => { return new { info.GoodsID, info.GoodsSn, info.GoodsName, info.GoodsDesc, info.MarketPrice, info.ShopPrice, info.GoodsNumber, info.BrandName, info.SuppliersName, info.SaleNumber,info.CateName ,info.GoodsBrief,Weight= info.Weight<=0.1 ? "小于 1":info.Weight ,info.NFareType,info.NFareInfo,info.FareTime, info.QuotaNumber,NQuotaNumber= (info.QuotaNumber<=0) ? "不限购":"件" }; });
                    repeaterGoodsInfo.DataBind();
                }
            }
            catch
            {
                Response.Redirect("/web/mall_Index.aspx", false);
            }

        }
    }
}