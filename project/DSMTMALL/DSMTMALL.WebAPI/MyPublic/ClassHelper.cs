using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSMTMALL.WebAPI.MyPublic
{
    public class ClassHelper
    {
    }
    public class GetClientReadCardEntity
    {
        public GetClientReadCardEntity()
        {
            AdminToken = string.Empty;
            CardNo = string.Empty;
            CheckCode = string.Empty;
            Sign = string.Empty;
            TimeStamp = string.Empty;
        }
        public string AdminToken { get; set; }
        public string CardNo { get; set; }
        public string CheckCode { get; set; }
        public string Sign { get; set; }
        public string TimeStamp { get; set; }
    }
    public class BackClientReadCardEntity
    {
        public BackClientReadCardEntity()
        {
            IsSuccess = string.Empty;
            Sign = string.Empty;
            TimeStamp = string.Empty;
        }
        public string IsSuccess { get; set; }
        public string Sign { get; set; }
        public string TimeStamp { get; set; }
    }
}