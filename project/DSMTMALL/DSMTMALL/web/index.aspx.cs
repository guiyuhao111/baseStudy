using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.MyPublic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSMTMALL.web
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        public IEnumerable<dynamic> TheNEXT(IEnumerable<dynamic> info, IEnumerable<dynamic> temp, IEnumerable<dynamic> res)
        {
            if (temp!=null)
            {
                foreach (var item in temp)
                {
                    temp = info.Where(x => x.DptParentSysID == item.DptSysID);
                    if (temp != null && temp.Count() > 0)
                    {
                        res = res.Concat(temp);
                        res = TheNEXT(info, temp, res);
                    }
                }
                return res;
            }else
            {
                return res;
            }
        }
    }
}