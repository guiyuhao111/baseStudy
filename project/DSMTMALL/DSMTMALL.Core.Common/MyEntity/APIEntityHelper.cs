using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSMTMALL.Core.Common.MyEntity
{
    public class APIDataBase
    {
        public APIDataBase() { JsonType = string.Empty; DataBase = string.Empty; }
        public string JsonType { get; set; }
        public string DataBase { get; set; }
    }
}
