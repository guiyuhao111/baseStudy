using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.web.database
{
    public partial class category : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))
            {
                if (!IsPostBack)
                {
                    StringBuilder strList = new StringBuilder();
                    string strWhere = "  ParentID = 0 AND IsDelete = 0 ORDER BY OrderBy ASC ";
                    List<DB.Model.M_Category> categoryInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<DB.Model.M_Category>(strWhere, null);
                    for (int i = 0; i < categoryInfo.Count; i++)
                    {
                        if (i < 10)
                        {
                            strList.Append(" <li id=\"" + categoryInfo[i].CateName + "\" ><a href=\"/web/database/category.aspx?bySysID=" + categoryInfo[i].CateID + "\" onclick=\"FocusThis(this)\" >" + categoryInfo[i].CateName + "</a></li> ");
                        }
                        else
                        {
                            if (i == 10)
                            {
                                strList.Append(" <li class=\"dropdown\"><a href=\"\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">更多<span class=\"caret\"></span></a><ul class=\"dropdown-menu\" role=\"menu\"> ");
                                strList.Append(" <li id=\"" + categoryInfo[i].CateName + "\"><a href=\"/web/database/category.aspx?bySysID=" + categoryInfo[i].CateID + "\" onclick=\"FocusThis(this)\" >" + categoryInfo[i].CateName + "</a></li> ");
                            }
                            else
                            {
                                strList.Append(" <li id=\"" + categoryInfo[i].CateName + "\" ><a href=\"/web/database/category.aspx?bySysID=" + categoryInfo[i].CateID + "\" onclick=\"FocusThis(this)\" >" + categoryInfo[i].CateName + "</a></li> ");
                            }
                        }
                    }
                    if (categoryInfo.Count >= 10)
                    {
                        strList.Append("  </ul> </ li > ");
                    }
                    sltFirstType.InnerHtml = strList.ToString();
                    BindGrid();
                    BindDdl();
                }
            }else
            {
                Response.Redirect("/web/web_login.aspx", false);
            }
        }

        public void BindDdl()
        {
            IEnumerable<dynamic> cateInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList("SELECT * FROM M_Category WHERE ParentID = 0", null);
            new DdlHelper().BindDdlMore(choiceParentsID, cateInfo, "无", x => { return new ListItem(Convert.ToString(x.CateName), Convert.ToString(x.CateID)); });
            new DdlHelper().BindDdlMore(updateChoiceParentsID, cateInfo,"无", x => { return new ListItem(Convert.ToString(x.CateName), Convert.ToString(x.CateID)); });
        }

        public void BindGrid()
        {
            repeaterCGList.DataSource = GetSizeList();
            repeaterCGList.DataBind();
        }

        public object GetSizeList()
        {
            string bySysID = ToolHelper.UrlParDecode(Request.QueryString["bySysID"], "");
            string searchName = string.Empty; 
            int pageNow = Convert.ToInt32(ToolHelper.UrlParDecode(Request.QueryString["pageNow"], "1"));
            int pageSize = 20;
            int productCount = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageCagegoryListCount(bySysID);
            string url = "/web/database/category.aspx?";
            url += "bySysID=" + ToolHelper.UrlParEncode(bySysID, "") + "&";
            object dt = new DB.BLL.MB_Bll(DBEnum.Slave).GetByPageCagegoryList(bySysID, (pageNow - 1) * pageSize, pageSize);
            PageNavHelper pageNavHelper = new PageNavHelper(productCount, pageNow, pageSize, url + "pageNow=");
            pageNav.InnerHtml = pageNavHelper.ToHtml();
            return dt;
        }
    }
}