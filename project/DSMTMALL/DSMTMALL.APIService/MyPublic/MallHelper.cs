using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSMTMALL.APIService.MyPublic
{
    public class MallHelper
    {
        public static VerifyHelper verifyHelper = new VerifyHelper();
        public static WebApiClass SendMessageToWeChat(APIDataBase apiDataBase)
        {
            APIResult apiResult = new APIResult();
            try
            {
                //switch (apiDataBase.JsonType)
                //{
                //    //case
                //}
            }
            catch(Exception e)
            {
                FileHelper.logger.Error(apiDataBase.JsonType + "_" + e.Message);
            }
            return verifyHelper.EncryptMallEntity(apiResult);
        }

        
    }
}