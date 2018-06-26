using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.MyPublic;
using System;
using System.Linq;

namespace DSMTMALL.web
{
    public partial class mall_Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindDdl();
        }

        public void BindDdl()
        {
            string url = WebToolHelper.GetProfilesUrl();
            var rollInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModelList("SELECT R.*,CONCAT(@PathAdd,R.Picture) AS NPicture FROM M_Roll AS R WHERE R.IsEnable =1 ORDER BY R.OrderBy ASC", new { PathAdd = url }).Select(info => { return new { info.NPicture, info.TargetSysID, info.TargetUrl }; });
            repeaterNoticeList.DataSource = rollInfo;
            repeaterNoticeList.DataBind();
            string cpyName = WebLoginHelper.GetSimpleName();
            if (!string.IsNullOrEmpty(cpyName))
            {
                cpyNameSpan.InnerHtml = cpyName+"商品专供";
            }
        }

      
    }
}