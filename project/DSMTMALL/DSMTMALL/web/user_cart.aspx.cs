using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.MyPublic;
using System;

namespace DSMTMALL.web
{
    public partial class user_cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string feeAmount = new WebToolHelper().GetProfilesEncryptInfo(FieldName.FeeAmount, "159");
            emCartFeeAmount.Attributes.Add("mydata", feeAmount);
        }
    }
}