using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using System;
using System.IO;
using System.Web;

namespace DSMTMALL.BACKMAG.web
{
    public partial class web_save : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod.ToLower() == "post")
            {
                string msg = "ERR,文件传输出错";
                string sysID = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request["id"], "");
                string uType = ToolHelper.GetPostOrGetPar(HttpContext.Current.Request["utype"], "");
                HttpFileCollection httpFileCollection = HttpContext.Current.Request.Files;
                if (!string.IsNullOrEmpty(sysID) && !string.IsNullOrEmpty(uType))
                {
                    if (sysID == "uploadExcel")
                    {
                        if (httpFileCollection.Count > 0)
                        {
                            string excelPath = "/files/excel/" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + ".xlsx";
                            string path = Server.MapPath("/") + excelPath;
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                            httpFileCollection[0].SaveAs(path);
                            if (File.Exists(path))
                            {
                                if (uType == "uploadGoodsExcel")
                                {
                                    msg = new WebSaveHelper().ReadExclToSqlProduct(excelPath);//写入数据库
                                }else if(uType== "uploadKouKuanExcel")
                                {
                                   // msg = new WebSaveHelper().ReadExclToSqlKouKuan(excelPath);//进行批量扣款
                                }

                                FileHelper.DeleteFile(path);//删除文件
                            }
                        }
                    }
                    else if (httpFileCollection.Count > 0)
                    {
                        string fileName = httpFileCollection[0].FileName;
                        string path_1 = string.Empty;
                        string path_2 = string.Empty;
                        string imgName = string.Empty;
                        string sqlPath_1 = string.Empty;
                        string sqlPath_2 = string.Empty;
                        bool ifCanSave = false;
                        string[] fileInfoArr = fileName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                        string newFileName = DateTime.Now.ToString("yyyyMMddhhmmssffff");
                        if (fileInfoArr.Length >= 2)
                        {
                            string fileExit = fileInfoArr[fileInfoArr.Length - 1];
                            if (fileExit == "jpg" || fileExit == "png" || fileExit == "gif" || fileExit == "bng" || fileExit == "jpeg")
                            {
                                try
                                {
                                    if (uType == "mainPicture")
                                    {
                                        path_1 = Server.MapPath("/files/imgs/") + DateTime.Now.ToString("yyyyMMdd")+"\\"+ newFileName+ "main." + fileExit;//保存路径
                                        path_2 = Server.MapPath("/files/imgs/") + DateTime.Now.ToString("yyyyMMdd")+"\\" + newFileName + "mainThemb." + fileExit;
                                        sqlPath_1 = "/files/imgs/" + DateTime.Now.ToString("yyyyMMdd") +"/"+ newFileName + "main." + fileExit;
                                        sqlPath_2 = "/files/imgs/" + DateTime.Now.ToString("yyyyMMdd")+"/" + newFileName + "mainThemb." + fileExit;
                                        ifCanSave = true;
                                    }
                                    else if (uType == "categoryPicture")
                                    {
                                        path_1 = Server.MapPath("/files/imgs/") + DateTime.Now.ToString("yyyyMMdd") + "\\" + newFileName + "category." + fileExit;//保存路径
                                        sqlPath_1 = "/files/imgs/" + DateTime.Now.ToString("yyyyMMdd") + "/" + newFileName + "category." + fileExit;
                                        ifCanSave = true;
                                    }
                                    else if(uType== "rollPicture")
                                    {
                                        path_1 = Server.MapPath("/files/imgs/") + DateTime.Now.ToString("yyyyMMdd") + "\\" + newFileName + "roll." + fileExit;//保存路径
                                        sqlPath_1 = "/files/imgs/" + DateTime.Now.ToString("yyyyMMdd") + "/" + newFileName + "roll." + fileExit;
                                        ifCanSave = true;
                                    }
                                    else if(uType== "galleryPicture")
                                    {
                                        path_1 = Server.MapPath("/files/imgs/") + DateTime.Now.ToString("yyyyMMdd") + "\\" + newFileName + "gallery." + fileExit;//保存路径
                                        sqlPath_1 = "/files/imgs/" + DateTime.Now.ToString("yyyyMMdd") + "/" + newFileName + "gallery." + fileExit;
                                        ifCanSave = true;
                                    }
                                    if (ifCanSave)
                                    {
                                        Directory.CreateDirectory(Path.GetDirectoryName(path_1));
                                        httpFileCollection[0].SaveAs(path_1);
                                        if (File.Exists(path_1))//上传图片保存成功
                                        {
                                            if (uType == "mainPicture")//上传的是产品主图
                                            {
                                                if (WebSaveHelper.MakeThumbnail(path_1, path_2, 80, 80, "HW"))//生成缩略图
                                                {
                                                    if (WebSaveHelper.UploadPictureSql(sysID, sqlPath_1, sqlPath_2, "goodsMainImg"))
                                                    {
                                                        msg = "SUCCESS";
                                                    }
                                                    else
                                                    {
                                                        msg = "ERR,文件上传失败";
                                                    }

                                                }
                                                else
                                                {
                                                    msg = "ERR,保存数据库出错";
                                                }
                                            }else if(uType== "categoryPicture")//上传的是类目主图
                                            {
                                                if(WebSaveHelper.UploadPictureSql(sysID,sqlPath_1,string.Empty, "categoryImg"))
                                                {
                                                    msg = "SUCCESS";
                                                }else
                                                {
                                                    msg = "ERR,文件上传失败";
                                                }
                                            }else if(uType== "rollPicture")//上传的是轮播主图
                                            {
                                                if (WebSaveHelper.UploadPictureSql(sysID, sqlPath_1, string.Empty, "rollImg"))
                                                {
                                                    msg = "SUCCESS";
                                                }
                                                else
                                                {
                                                    msg = "ERR,文件上传失败";
                                                }
                                            }else if(uType== "galleryPicture")
                                            {
                                                if (WebSaveHelper.UploadPictureSql(sysID, sqlPath_1, string.Empty, "galleryImg"))
                                                {
                                                    msg = sqlPath_1; //"SUCCESS";
                                                }
                                                else
                                                {
                                                    msg = "ERR,文件上传失败";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            msg = "ERR,文件保存出错";
                                        }
                                    }
                                }
                                catch { msg = "ERR,文件保存出错"; }
                            }else
                            {
                                msg = "ERR,上传文件格式不正确";
                            }
                        }
                    }
                }
                else
                {
                    msg = "ERR,系统出错，请重新登陆";
                }
                string res = "{ msg:'" + msg + "',}";
                Response.Write(res);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
}


