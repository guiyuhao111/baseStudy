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
    public partial class category_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindDdl();
        }
        public void BindDdl()
        {
            string parentCateIDStr = string.Empty;
            if (HttpRuntime.Cache["parentCateID"] == null)
            {
                string url = WebToolHelper.GetProfilesUrl();
                string strSql = "SELECT *, CONCAT(@AddPath,ShowImage) AS NPicture  FROM M_Category WHERE IsDelete = 0 AND IsEnable =1 AND ShowInNav =1 AND ParentID = 0";
                IEnumerable<dynamic> info = new DB.BLL.MY_Bll(DBEnum.Slave).GetModelList(strSql, new { AddPath = url });
                StringBuilder innerText = new StringBuilder();
                foreach (var item in info)
                {
                    innerText.Append("<li name=\"parentCateID\"><a href=\"javascript:void(0);\" mydata=\"" + item.CateID + "\" > " + item.CateName + " </a></li>");
                }
                parentCateIDStr = innerText.ToString();
                HttpRuntime.Cache.Insert("parentCateID", parentCateIDStr, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
            }else
            {
                parentCateIDStr = Convert.ToString(HttpRuntime.Cache["parentCateID"]);
            }
            gridTopCategoryList.InnerHtml = parentCateIDStr;

        }
    }
}