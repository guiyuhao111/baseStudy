using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DSMTMALL.APIService.Controllers
{
    public class IndexController : ApiController
    {
        [HttpPost]
        public WebApiClass CpyInfoSave([FromBody] WebApiClass webApiClass)
        {
            if (webApiClass != null)
            {
                APIDataBase apiDataBase = new APIDataBase();
                if (new VerifyHelper().CheckMallSign(webApiClass, out cpyInfo))
                {
                    return CenterHelper.SaveCpyInfoFromCenter(cpyInfo);
                }
            }
            return null;
        }
    }
}