using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using LitJson;
using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace DSMTMALL.BACKMAG.web
{
    /// <summary>
    /// upload_json 的摘要说明
    /// </summary>
    public class upload_file : IHttpHandler, IReadOnlySessionState
    {
        private HttpContext context;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string newFileName = DateTime.Now.ToString("yyyyMMddhhmmssffff");
            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "mp4");
            //最大文件大小
            int maxSize = 3000000;//1M=1048576字节  》3M
            this.context = context;
            HttpPostedFile imgFile = context.Request.Files["imgFile"];
            string dirName = context.Request.QueryString["dir"];
            if (string.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            string fileName = imgFile.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();//获取扩展名
            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                showError("上传文件大小超过限制。");
            }
            string s = fileExt.Substring(1).ToLower();
            Array ss = ((string)extTable[dirName]).Split(',');
            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(((string)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((string)extTable[dirName]) + "格式。");
            }
             //文件保存目录路径
            string path_1 = HttpContext.Current.Server.MapPath("/files/imgs/") + DateTime.Now.ToString("yyyyMMdd") + "\\" + newFileName + "detial" + fileExt;//保存路径
            //文件保存目录URL
            string sqlPath_1 = "/files/imgs/" + DateTime.Now.ToString("yyyyMMdd") + "/" + newFileName + "detial" + fileExt;
            //创建文件夹
            Directory.CreateDirectory(Path.GetDirectoryName(path_1));
            imgFile.SaveAs(path_1);//保存文件
            if (File.Exists(path_1))//判断是否存在
            {
                if(WebSaveHelper.UploadFileManageTable(sqlPath_1))
                {
                    Hashtable hash = new Hashtable();
                    hash["error"] = 0;
                    hash["url"] = sqlPath_1;
                    context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                    context.Response.Write(JsonMapper.ToJson(hash));
                    context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    FileHelper.DeleteFile(path_1);
                    showError("上传文件入库失败");
                }
            }
        }
        private void showError(string message)
        {
            Hashtable hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
            context.ApplicationInstance.CompleteRequest();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}