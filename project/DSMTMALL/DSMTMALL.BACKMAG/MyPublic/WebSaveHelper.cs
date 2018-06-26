using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using DSMTMALL.DB.Model;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;

namespace DSMTMALL.BACKMAG.MyPublic
{
    public class WebSaveHelper
    {
        #region MyRegion
        /// <summary>
        /// asp.net上传图片并生成缩略图//由前端直接上传图片
        /// </summary>
        /// <param name="upImage">HtmlInputFile控件</param>
        /// <param name="sSavePath">保存的路径,些为相对服务器路径的下的文件夹</param>
        /// <param name="sThumbExtension">缩略图的thumb</param>
        /// <param name="intThumbWidth">生成缩略图的宽度</param>
        /// <param name="intThumbHeight">生成缩略图的高度</param>
        /// <returns>缩略图名称</returns>
        public string UpLoadImage(HtmlInputFile upImage, string sSavePath, string sThumbExtension, int intThumbWidth, int intThumbHeight)
        {
            string sThumbFile = "";
            string sFilename = "";
            if (upImage.PostedFile != null)
            {
                HttpPostedFile myFile = upImage.PostedFile;
                int nFileLen = myFile.ContentLength;
                if (nFileLen == 0)
                    return "没有选择上传图片";
                //获取upImage选择文件的扩展名
                string extendName = System.IO.Path.GetExtension(myFile.FileName).ToLower();
                //判断是否为图片格式
                if (extendName != ".jpg" && extendName != ".jpge" && extendName != ".gif" && extendName != ".bmp" && extendName != ".png")
                    return "图片格式不正确";

                byte[] myData = new Byte[nFileLen];
                myFile.InputStream.Read(myData, 0, nFileLen);
                sFilename = System.IO.Path.GetFileName(myFile.FileName);
                int file_append = 0;
                //检查当前文件夹下是否有同名图片,有则在文件名+1
                while (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(sSavePath + sFilename)))
                {
                    file_append++;
                    sFilename = System.IO.Path.GetFileNameWithoutExtension(myFile.FileName)
                        + file_append.ToString() + extendName;
                }
                System.IO.FileStream newFile
                    = new System.IO.FileStream(System.Web.HttpContext.Current.Server.MapPath(sSavePath + sFilename),
                    System.IO.FileMode.Create, System.IO.FileAccess.Write);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();
                //以上为上传原图
                try
                {
                    //原图加载
                    using (System.Drawing.Image sourceImage = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath(sSavePath + sFilename)))
                    {
                        //原图宽度和高度
                        int width = sourceImage.Width;
                        int height = sourceImage.Height;
                        int smallWidth;
                        int smallHeight;
                        //获取第一张绘制图的大小,(比较 原图的宽/缩略图的宽  和 原图的高/缩略图的高)
                        if (((decimal)width) / height <= ((decimal)intThumbWidth) / intThumbHeight)
                        {
                            smallWidth = intThumbWidth;
                            smallHeight = intThumbWidth * height / width;
                        }
                        else
                        {
                            smallWidth = intThumbHeight * width / height;
                            smallHeight = intThumbHeight;
                        }
                        //判断缩略图在当前文件夹下是否同名称文件存在
                        file_append = 0;
                        sThumbFile = sThumbExtension + System.IO.Path.GetFileNameWithoutExtension(myFile.FileName) + extendName;
                        while (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(sSavePath + sThumbFile)))
                        {
                            file_append++;
                            sThumbFile = sThumbExtension + System.IO.Path.GetFileNameWithoutExtension(myFile.FileName) +
                                file_append.ToString() + extendName;
                        }
                        //缩略图保存的绝对路径
                        string smallImagePath = System.Web.HttpContext.Current.Server.MapPath(sSavePath) + sThumbFile;
                        //新建一个图板,以最小等比例压缩大小绘制原图
                        using (System.Drawing.Image bitmap = new System.Drawing.Bitmap(smallWidth, smallHeight))
                        {
                            //绘制中间图
                            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                            {
                                //高清,平滑
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                g.Clear(Color.Black);
                                g.DrawImage(
                                sourceImage,
                                new System.Drawing.Rectangle(0, 0, smallWidth, smallHeight),
                                new System.Drawing.Rectangle(0, 0, width, height),
                                System.Drawing.GraphicsUnit.Pixel
                                );
                            }
                            //新建一个图板,以缩略图大小绘制中间图
                            using (System.Drawing.Image bitmap1 = new System.Drawing.Bitmap(intThumbWidth, intThumbHeight))
                            {
                                //绘制缩略图  http://www.cnblogs.com/sosoft/
                                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap1))
                                {
                                    //高清,平滑
                                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                    g.Clear(Color.Black);
                                    int lwidth = (smallWidth - intThumbWidth) / 2;
                                    int bheight = (smallHeight - intThumbHeight) / 2;
                                    g.DrawImage(bitmap, new Rectangle(0, 0, intThumbWidth, intThumbHeight), lwidth, bheight, intThumbWidth, intThumbHeight, GraphicsUnit.Pixel);
                                    g.Dispose();
                                    bitmap1.Save(smallImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    //出错则删除
                    System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(sSavePath + sFilename));
                    return "图片格式不正确";
                }
                //返回缩略图名称
                return sThumbFile;
            }
            return "没有选择图片";
        }
        #endregion

        #region  生成缩略图

        /// <summary>
        /// 根据原图生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        public static bool MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            try
            {
                Image originalImage = Image.FromFile(originalImagePath);
                int towidth = width;
                int toheight = height;
                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;
                switch (mode)
                {
                    case "HW":  //指定高宽缩放（可能变形）  
                        break;
                    case "W":   //指定宽，高按比例     
                        toheight = originalImage.Height * width / originalImage.Width;
                        break;
                    case "H":   //指定高，宽按比例
                        towidth = originalImage.Width * height / originalImage.Height;
                        break;
                    case "Cut": //指定高宽裁减（不变形）       
                        if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * towidth / toheight;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * height / towidth;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    default:
                        break;
                }
                //新建一个bmp图片
                 Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                // Bitmap bitmap = new Bitmap(1024, 768, PixelFormat.Format24bppRgb);
                bitmap.SetResolution(72, 72);
                //新建一个画板
                Graphics g = Graphics.FromImage(bitmap);
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //清空画布并以透明背景色填充
                g.Clear(Color.Transparent);
                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);
                try
                {
                    //以jpg格式保存缩略图
                    bitmap.Save(thumbnailPath, ImageFormat.Jpeg);
                    return true;
                }
                catch
                {
                    if (File.Exists(originalImagePath))
                    {
                        File.Delete(originalImagePath);
                    }
                    return false;
                }
                finally
                {
                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                }
            }
            catch
            {
                if (File.Exists(originalImagePath))
                {
                    File.Delete(originalImagePath);
                }
                return false;
            }
        }

        #endregion

        #region 写入数据库

        /// <summary>
        /// 上传图片后更新数据库文件
        /// </summary>
        /// <param name="sysID">ID</param>
        /// <param name="imgUrl_1">图片路径一</param>
        /// <param name="imgUrl_2">图片路径二</param>
        /// <returns></returns>
        public static bool UploadPictureSql(string sysID , string imgUrl_1 ,string imgUrl_2,string type)
        {
            string path = string.Empty;
            if(new DB.BLL.MB_Bll(DBEnum.Master).UploadImgFile(sysID, imgUrl_1, imgUrl_2, type))//更新数据库数据
            {
                return true;
            }else//数据库更新失败，则删除相关文件
            {
                try
                {
                    if (!string.IsNullOrEmpty(imgUrl_1))
                    {
                        path = HttpContext.Current.Server.MapPath(imgUrl_1);
                        FileHelper.DeleteFile(path);
                    }
                    if (!string.IsNullOrEmpty(imgUrl_2))
                    {
                        path = HttpContext.Current.Server.MapPath(imgUrl_2);
                        FileHelper.DeleteFile(path);
                    }
                } catch
                {
                    WebLogHelper.WriteErrLog(DateTime.Now+WebLoginHelper.GetAdminName()+"删除文件出错：" + imgUrl_1 + "/" + imgUrl_2 ,string.Empty);
                    return false;
                }
                return false;
            }
        }

        /// <summary>
        /// 上传图片后更新图片管理表
        /// </summary>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public static bool UploadFileManageTable(string imgUrl)
        {
            M_FileManage fileManageInfo = new M_FileManage();
            fileManageInfo.FileName = Path.GetFileNameWithoutExtension(imgUrl);
            fileManageInfo.FilePath = imgUrl;
            fileManageInfo.CreatTime = DateTime.Now;
            fileManageInfo.FileType = 0;
            fileManageInfo.FileLabel = string.Empty;
            return new DB.BLL.MY_Bll(DBEnum.Master).AddModel<M_FileManage>(fileManageInfo);
        }

        /// <summary>
        /// EXCLE上传商品信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ReadExclToSqlProduct(string path)
        {
            string resInfo = string.Empty;
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
            {
                DataTable dt = new ExcelHelper().ReadExcel(HttpContext.Current.Server.MapPath(path));
                decimal outMarketPrice = 0;
                decimal outShopPrice = 0;
                string sqlPath_1 = string.Empty;
                string sqlPath_2 = string.Empty;
                string url = string.Empty;

                //复制DataTable表结构到错误表
                DataTable dtError = dt.Clone();
                if (!dtError.Columns.Contains("错误信息"))
                {
                    dtError.Columns.Add("错误信息", Type.GetType("System.String"));
                }
                if (dt != null)
                {
                    resInfo = "共上传" + dt.Rows.Count + "条数据";
                    DateTime nowTime = DateTime.Now;
                    IEnumerable<dynamic> dtCategoryList = new DB.BLL.MB_Bll(DBEnum.Slave).GetCategoryList();//获取所有的类目数据(包括父级)
                    IEnumerable<dynamic> dtFareTempList = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList(" SELECT FareName,FareSysID FROM M_FareTemplate WHERE 1=1 ", null);
                    List<M_Goods> dtGoodsList = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<M_Goods>("1=1 AND IsDelete = 0 ORDER BY AddTime DESC ", null);//获取当前已有的产品信息
                    List<M_Brand> dtBrandList = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<M_Brand>(" 1=1 ", null);//获取当前已有的品牌信息
                    List<M_Suppliers> dtSuppliers = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<M_Suppliers>(" 1=1 ",null);
                    string errInfo = string.Empty;
                    string tempErr = string.Empty;
                    M_Goods goodsInfo = new M_Goods();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        goodsInfo = new M_Goods();
                        errInfo = string.Empty;
                        sqlPath_1 = string.Empty;
                        sqlPath_2 = string.Empty;
                        url = string.Empty;
                        outMarketPrice = 0;
                        outShopPrice = 0;
                        try
                        {
                            if (dtGoodsList != null)
                            {
                                foreach (var item in dtGoodsList)//遍历商品信息表查看该商品编号是否存在，如果存在则跳出本次循环
                                {
                                    if (item.GoodsSn == dt.Rows[i]["GoodsSn"].ToString())
                                    {
                                        errInfo += "该商品编号已经存在，勿重复创建;";
                                        break;
                                    }
                                }
                            }
                            tempErr = "供应商不存在";
                            if (dtSuppliers != null)
                            {
                                foreach (var item in dtSuppliers)
                                {
                                    if (item.SuppliersName == Convert.ToString(dt.Rows[i]["SuppliersID"]))
                                    {
                                        goodsInfo.SuppliersID = item.SuppliersID;
                                        tempErr = string.Empty;
                                        break;
                                    }
                                }
                            }
                            errInfo += tempErr;
                            tempErr = "类目不存在;";//临时错误信息用来存储临时判断环节可能出现的错误
                            if (dtCategoryList != null)
                            {
                                foreach (var item in dtCategoryList)
                                {
                                    if (item.CateName_1 == dt.Rows[i]["CateName_1"].ToString() && item.CateName_2 == dt.Rows[i]["CateName_2"].ToString())
                                    {
                                        goodsInfo.CateID = Convert.ToInt32(item.CateID);
                                        tempErr = string.Empty;//符合条件后清空临时错误
                                        break;
                                    }
                                }
                            }
                            errInfo += tempErr;//判断一个环节结束后将临时错误加到这个要输出的错误信息后面去
                            tempErr = "运费模板不存在;";//临时错误信息用来存储临时判断环节可能出现的错误
                            if (dtFareTempList != null)
                            {
                                foreach (var item in dtFareTempList)
                                {
                                    if (item.FareName == dt.Rows[i]["FareName"].ToString())//遍历运费模板页，找到这个运费模板信息后清空错误信息
                                    {
                                        goodsInfo.FareSysID = item.FareSysID;
                                        tempErr = string.Empty;//符合条件后清空临时错误
                                        break;
                                    }
                                }
                            }
                            errInfo += tempErr;
                            tempErr = "金额出错;";
                            if (decimal.TryParse(dt.Rows[i]["MarketPrice"].ToString(), out outMarketPrice) && decimal.TryParse(dt.Rows[i]["ShopPrice"].ToString(), out outShopPrice))
                            {
                                goodsInfo.MarketPrice = outMarketPrice;
                                goodsInfo.ShopPrice = outShopPrice;
                                tempErr = string.Empty;//符合条件后清空临时错误
                            }
                            errInfo += tempErr;
                            if (!string.IsNullOrEmpty(dt.Rows[i]["BrandName"].ToString()))
                            {
                                tempErr = "品牌不存在";
                                if (dtBrandList != null)
                                {
                                    foreach (var item in dtBrandList)
                                    {
                                        if (item.BrandName == dt.Rows[i]["BrandName"].ToString())
                                        {
                                            goodsInfo.BrandID = item.BrandID;
                                            tempErr = string.Empty;
                                            break;
                                        }
                                    }
                                }
                                errInfo += tempErr;
                            }
                            if (string.IsNullOrEmpty(errInfo))//最后判断这个临时错误信息
                            {
                                url = dt.Rows[i]["GoodsImg"].ToString();
                                url = GetInterNetImgUrl(url);
                                if (!string.IsNullOrEmpty(url))
                                {
                                    DownloadFiles(url, out sqlPath_1, out sqlPath_2);
                                }
                                goodsInfo.GoodsImg = sqlPath_1;
                                goodsInfo.GoodsThumb = sqlPath_2;
                                goodsInfo.OriginalImg = sqlPath_1;
                                goodsInfo.GoodsID = Guid.NewGuid().ToString();
                                goodsInfo.GoodsSn = dt.Rows[i]["GoodsSn"].ToString();
                                goodsInfo.GoodsName = dt.Rows[i]["GoodsName"].ToString();
                                goodsInfo.GoodsNumber = Convert.ToInt32(dt.Rows[i]["GoodsNumber"].ToString());
                                goodsInfo.GoodsBrief = dt.Rows[i]["GoodsBrief"].ToString();
                                goodsInfo.IsReal = Convert.ToInt32(dt.Rows[i]["IsReal"].ToString());
                                goodsInfo.IsNew = Convert.ToInt32(dt.Rows[i]["IsNew"].ToString());
                                goodsInfo.IsHot = Convert.ToInt32(dt.Rows[i]["IsHot"].ToString());
                                goodsInfo.SellerNote = dt.Rows[i]["SellerNote"].ToString();
                                goodsInfo.Weight = Convert.ToDouble(dt.Rows[i]["Weight"].ToString());
                                if (!(new DB.BLL.MB_Bll(DBEnum.Master).AddGoodsInfo(goodsInfo.CateID, goodsInfo.MarketPrice, goodsInfo.ShopPrice, goodsInfo.BrandID, goodsInfo.GoodsImg, goodsInfo.GoodsThumb, goodsInfo.OriginalImg, goodsInfo.SuppliersID, goodsInfo.GoodsSn, goodsInfo.GoodsName, goodsInfo.GoodsNumber, goodsInfo.GoodsBrief, goodsInfo.IsReal, goodsInfo.IsNew, goodsInfo.IsHot, goodsInfo.Weight, goodsInfo.SellerNote, goodsInfo.FareSysID,WebLoginHelper.GetAdminName())))
                                {
                                    dtError.ImportRow(dt.Rows[i]);
                                    dtError.Rows[dtError.Rows.Count - 1]["错误信息"] = "该条记录写入数据库失败";
                                }
                                else
                                {
                                    dtGoodsList.Add(goodsInfo);//将创建成功后的这条信息加到数据集合中
                                }
                            }
                            else
                            {
                                dtError.ImportRow(dt.Rows[i]);
                                dtError.Rows[dtError.Rows.Count - 1]["错误信息"] = errInfo;
                            }
                        }
                        catch (Exception eMsg)
                        {
                            dtError.ImportRow(dt.Rows[i]);
                            dtError.Rows[dtError.Rows.Count - 1]["错误信息"] = "该条记录使系统产生严重错误，请联系管理员:" + eMsg.Message;
                        }
                    }
                    if (dtError != null)
                    {
                        resInfo += " 上传失败" + dtError.Rows.Count + "条";
                        new ExcelHelper().DataTableToExcel(dtError, "xls", "上传错误信息表", Path.GetDirectoryName(HttpContext.Current.Server.MapPath(path)) + "/上传错误信息表.xls");
                    }
                }
            }else
            {
                resInfo = "上传失败，当前账户无权执行ECXEL商品信息导入操作";
            }
            return resInfo;
        }

        /// <summary>
        /// EXCEL批量进行扣款
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //public string ReadExclToSqlKouKuan(string path)
        //{
        //    string resInfo = string.Empty;
        //    if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage))
        //    {
        //        DataTable dt = new ExcelHelper().ReadExcel(HttpContext.Current.Server.MapPath(path));
        //        decimal outMarketPrice = 0;
        //        decimal outShopPrice = 0;
        //        string sqlPath_1 = string.Empty;
        //        string sqlPath_2 = string.Empty;
        //        string url = string.Empty;

        //        //复制DataTable表结构到错误表
        //        DataTable dtError = dt.Clone();
        //        if (!dtError.Columns.Contains("错误信息"))
        //        {
        //            dtError.Columns.Add("错误信息", Type.GetType("System.String"));
        //        }
        //        if (dt != null)
        //        {
        //            resInfo = "共上传" + dt.Rows.Count + "条数据";
        //            DateTime nowTime = DateTime.Now;
        //            IEnumerable<dynamic> dtCategoryList = new DB.BLL.MB_Bll(DBEnum.Slave).GetCategoryList();//获取所有的类目数据(包括父级)
        //            IEnumerable<dynamic> dtFareTempList = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList(" SELECT FareName,FareSysID FROM M_FareTemplate WHERE 1=1 ", null);
        //            List<M_Goods> dtGoodsList = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<M_Goods>("1=1 AND IsDelete = 0 ORDER BY AddTime DESC ", null);//获取当前已有的产品信息
        //            List<M_Brand> dtBrandList = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<M_Brand>(" 1=1 ", null);//获取当前已有的品牌信息
        //            List<M_Suppliers> dtSuppliers = new DB.BLL.MB_Bll(DBEnum.Slave).GetModelList<M_Suppliers>(" 1=1 ", null);
        //            string errInfo = string.Empty;
        //            string tempErr = string.Empty;
        //            M_Goods goodsInfo = new M_Goods();
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                goodsInfo = new M_Goods();
        //                errInfo = string.Empty;
        //                sqlPath_1 = string.Empty;
        //                sqlPath_2 = string.Empty;
        //                url = string.Empty;
        //                outMarketPrice = 0;
        //                outShopPrice = 0;
        //                try
        //                {
        //                    if (dtGoodsList != null)
        //                    {
        //                        foreach (var item in dtGoodsList)//遍历商品信息表查看该商品编号是否存在，如果存在则跳出本次循环
        //                        {
        //                            if (item.GoodsSn == dt.Rows[i]["GoodsSn"].ToString())
        //                            {
        //                                errInfo += "该商品编号已经存在，勿重复创建;";
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    tempErr = "供应商不存在";
        //                    if (dtSuppliers != null)
        //                    {
        //                        foreach (var item in dtSuppliers)
        //                        {
        //                            if (item.SuppliersName == Convert.ToString(dt.Rows[i]["SuppliersID"]))
        //                            {
        //                                goodsInfo.SuppliersID = item.SuppliersID;
        //                                tempErr = string.Empty;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    errInfo += tempErr;
        //                    tempErr = "类目不存在;";//临时错误信息用来存储临时判断环节可能出现的错误
        //                    if (dtCategoryList != null)
        //                    {
        //                        foreach (var item in dtCategoryList)
        //                        {
        //                            if (item.CateName_1 == dt.Rows[i]["CateName_1"].ToString() && item.CateName_2 == dt.Rows[i]["CateName_2"].ToString())
        //                            {
        //                                goodsInfo.CateID = Convert.ToInt32(item.CateID);
        //                                tempErr = string.Empty;//符合条件后清空临时错误
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    errInfo += tempErr;//判断一个环节结束后将临时错误加到这个要输出的错误信息后面去
        //                    tempErr = "运费模板不存在;";//临时错误信息用来存储临时判断环节可能出现的错误
        //                    if (dtFareTempList != null)
        //                    {
        //                        foreach (var item in dtFareTempList)
        //                        {
        //                            if (item.FareName == dt.Rows[i]["FareName"].ToString())//遍历运费模板页，找到这个运费模板信息后清空错误信息
        //                            {
        //                                goodsInfo.FareSysID = item.FareSysID;
        //                                tempErr = string.Empty;//符合条件后清空临时错误
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    errInfo += tempErr;
        //                    tempErr = "金额出错;";
        //                    if (decimal.TryParse(dt.Rows[i]["MarketPrice"].ToString(), out outMarketPrice) && decimal.TryParse(dt.Rows[i]["ShopPrice"].ToString(), out outShopPrice))
        //                    {
        //                        goodsInfo.MarketPrice = outMarketPrice;
        //                        goodsInfo.ShopPrice = outShopPrice;
        //                        tempErr = string.Empty;//符合条件后清空临时错误
        //                    }
        //                    errInfo += tempErr;
        //                    if (!string.IsNullOrEmpty(dt.Rows[i]["BrandName"].ToString()))
        //                    {
        //                        tempErr = "品牌不存在";
        //                        if (dtBrandList != null)
        //                        {
        //                            foreach (var item in dtBrandList)
        //                            {
        //                                if (item.BrandName == dt.Rows[i]["BrandName"].ToString())
        //                                {
        //                                    goodsInfo.BrandID = item.BrandID;
        //                                    tempErr = string.Empty;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        errInfo += tempErr;
        //                    }
        //                    if (string.IsNullOrEmpty(errInfo))//最后判断这个临时错误信息
        //                    {
        //                        url = dt.Rows[i]["GoodsImg"].ToString();
        //                        url = GetInterNetImgUrl(url);
        //                        if (!string.IsNullOrEmpty(url))
        //                        {
        //                            DownloadFiles(url, out sqlPath_1, out sqlPath_2);
        //                        }
        //                        goodsInfo.GoodsImg = sqlPath_1;
        //                        goodsInfo.GoodsThumb = sqlPath_2;
        //                        goodsInfo.OriginalImg = sqlPath_1;
        //                        goodsInfo.GoodsID = Guid.NewGuid().ToString();
        //                        goodsInfo.GoodsSn = dt.Rows[i]["GoodsSn"].ToString();
        //                        goodsInfo.GoodsName = dt.Rows[i]["GoodsName"].ToString();
        //                        goodsInfo.GoodsNumber = Convert.ToInt32(dt.Rows[i]["GoodsNumber"].ToString());
        //                        goodsInfo.GoodsBrief = dt.Rows[i]["GoodsBrief"].ToString();
        //                        goodsInfo.IsReal = Convert.ToInt32(dt.Rows[i]["IsReal"].ToString());
        //                        goodsInfo.IsNew = Convert.ToInt32(dt.Rows[i]["IsNew"].ToString());
        //                        goodsInfo.IsHot = Convert.ToInt32(dt.Rows[i]["IsHot"].ToString());
        //                        goodsInfo.SellerNote = dt.Rows[i]["SellerNote"].ToString();
        //                        goodsInfo.Weight = Convert.ToDouble(dt.Rows[i]["Weight"].ToString());
        //                        if (!(new DB.BLL.MB_Bll(DBEnum.Master).AddGoodsInfo(goodsInfo.CateID, goodsInfo.MarketPrice, goodsInfo.ShopPrice, goodsInfo.BrandID, goodsInfo.GoodsImg, goodsInfo.GoodsThumb, goodsInfo.OriginalImg, goodsInfo.SuppliersID, goodsInfo.GoodsSn, goodsInfo.GoodsName, goodsInfo.GoodsNumber, goodsInfo.GoodsBrief, goodsInfo.IsReal, goodsInfo.IsNew, goodsInfo.IsHot, goodsInfo.Weight, goodsInfo.SellerNote, goodsInfo.FareSysID, WebLoginHelper.GetAdminName())))
        //                        {
        //                            dtError.ImportRow(dt.Rows[i]);
        //                            dtError.Rows[dtError.Rows.Count - 1]["错误信息"] = "该条记录写入数据库失败";
        //                        }
        //                        else
        //                        {
        //                            dtGoodsList.Add(goodsInfo);//将创建成功后的这条信息加到数据集合中
        //                        }
        //                    }
        //                    else
        //                    {
        //                        dtError.ImportRow(dt.Rows[i]);
        //                        dtError.Rows[dtError.Rows.Count - 1]["错误信息"] = errInfo;
        //                    }
        //                }
        //                catch (Exception eMsg)
        //                {
        //                    dtError.ImportRow(dt.Rows[i]);
        //                    dtError.Rows[dtError.Rows.Count - 1]["错误信息"] = "该条记录使系统产生严重错误，请联系管理员:" + eMsg.Message;
        //                }
        //            }
        //            if (dtError != null)
        //            {
        //                resInfo += " 上传失败" + dtError.Rows.Count + "条";
        //                new ExcelHelper().DataTableToExcel(dtError, "xls", "上传错误信息表", Path.GetDirectoryName(HttpContext.Current.Server.MapPath(path)) + "/上传错误信息表.xls");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        resInfo = "上传失败，当前账户无权执行ECXEL商品信息导入操作";
        //    }
        //    return resInfo;
        //}

        #endregion

        #region EXCEL 导出

        /// <summary>
        /// EXCEL导出信息表
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string ExportToExcel(DataSet ds)
        {
            string path = "/excel/" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + ".xls";
            new ExcelHelper().DataTableToExcel(ds.Tables[0], "xls", "导出信息表", HttpContext.Current.Server.MapPath(path));
            return path;
        }


        #endregion

        #region 下载网络图片文件

        /// <summary>
        /// 下载网络图片
        /// </summary>
        /// <param name="url">网络图片URL地址</param>
        /// <param name="sqlPath_1">保存到数据库的原图路径</param>
        /// <param name="sqlPath_2">保存到数据库的缩略图径</param>
        /// <returns></returns>
        public static bool DownloadFiles(string url, out string sqlPath_1, out string sqlPath_2)
        {
            sqlPath_1 = string.Empty;
            sqlPath_2 = string.Empty;
            string newFileName = DateTime.Now.ToString("yyyyMMddhhmmssffff");
            try
            {
                WebClient client = new WebClient();
                string fileExit = Path.GetExtension(url);
                string path_1 = HttpContext.Current.Server.MapPath("/files/imgs/") + DateTime.Now.ToString("yyyyMMdd") + "\\" + newFileName + "main.jpg";// + fileExit;//保存路径
                string path_2 = HttpContext.Current.Server.MapPath("/files/imgs/") + DateTime.Now.ToString("yyyyMMdd") + "\\" + newFileName + "mainThemb.jpg";// + fileExit;
                sqlPath_1 = "/files/imgs/" + DateTime.Now.ToString("yyyyMMdd") + "/" + newFileName + "main.jpg";// + fileExit;
                sqlPath_2 = "/files/imgs/" + DateTime.Now.ToString("yyyyMMdd") + "/" + newFileName + "mainThemb.jpg";// + fileExit;
                Directory.CreateDirectory(Path.GetDirectoryName(path_1));
                client.DownloadFile(url, path_1);//下载网络图片到本地
                if (File.Exists(path_1))
                {
                    if (!UploadFileManageTable(sqlPath_1))//添加数据进文件管理器表
                    {
                        File.Delete(path_1);
                    }
                    MakeThumbnail(path_1, path_2, 900, 900, "HW");
                    if (File.Exists(path_2))
                    {
                        if (!UploadFileManageTable(sqlPath_2))//添加数据进文件管理器表
                        {
                            File.Delete(path_2);
                        }
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        #endregion

        #region 获取互联网图片地址
        /// <summary>
        /// 获取互联网图片地址
        /// </summary>
        /// <param name="HTMLStr"></param>
        /// <returns></returns>
        public static string GetInterNetImgUrl(string sHtmlText)
        {
            //string str = string.Empty;
            //Regex r = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            //Match m = r.Match(HTMLStr.ToLower());
            //if (m.Success)
            //    str = m.Groups["imgUrl"].Value;
            //return str;
            Regex regImg = new Regex(@"(http.*?.jpg)|(http.*?.png)");
            MatchCollection group = regImg.Matches(sHtmlText);  //match.Groups;
            if (group != null && group.Count > 0)
            {
                foreach (var item in group)
                {
                   return  item.ToString();
                }
            }
            return string.Empty; ;
        }

        /// <summary>
        /// 取得HTML中所有图片的URL
        /// </summary>
        /// <param name="sHtmlText">HTML代码</param>
        /// <returns></returns>
        public static List<string> GetHtmlImageUrlList(string sHtmlText)
        {
            Regex regImg = new Regex(@"(http.*?.jpg)|(http.*?.png)");
            List<string> strList = new List<string>();
            MatchCollection group = regImg.Matches(sHtmlText);  //match.Groups;
            foreach (var item in group)
            {
                strList.Add(item.ToString());
            }
            return strList;
        }
        #endregion

    }
}