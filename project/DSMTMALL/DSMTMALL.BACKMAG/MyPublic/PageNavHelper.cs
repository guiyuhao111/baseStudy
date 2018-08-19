using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSMTMALL.BACKMAG.MyPublic
{
    public class PageNavHelper
    {
        private int pageNow;// 当前页码
        private int pageMax;// 总页数
        private int pageSize;// 设置或获取每页显示行数
        private int recordCount;// 设置或获取总记录数
        private string linkUrl;// 设置分页链接
        private int DEFAULT_SPLIT_SIZE = 4; // 当前页左右显示页个数
        /*
         * @param recordCount
         *            总记录数
         * @param pageNow
         *            当前页码
         * @param linkUrl
         *            URL
         */
        public PageNavHelper(int recordCount, int pageNow, string linkUrl)
        {
            this.recordCount = recordCount;
            this.pageNow = pageNow;
            this.pageSize = 20;
            this.linkUrl = linkUrl;
        }
        /*
         * @param recordCount
         *            总记录数
         * @param pageNow
         *            当前页码
         * @param pageSize
         *            每页显示条数
         * @param linkUrl
         *            URL
         */
        public PageNavHelper(int recordCount, int pageNow, int pageSize, string linkUrl)
        {
            this.recordCount = recordCount;
            this.pageNow = pageNow;
            this.pageSize = pageSize;
            this.linkUrl = linkUrl;
        }
        // / <summary>
        // / 分页样式
        // / </summary>
        // / <returns></returns>
        public String ToHtml()
        {
            if (this.recordCount > this.pageSize)
            {
                this.pageMax = this.recordCount / this.pageSize;
                if (this.recordCount % this.pageSize != 0)
                {
                    this.pageMax = this.pageMax + 1;
                }
                // 计算分页
                if (this.pageMax == 0)
                {
                    return null;
                }
                string result = "";
                int indexStart = this.pageNow - DEFAULT_SPLIT_SIZE;
                int indexEnd = this.pageNow + DEFAULT_SPLIT_SIZE;
                // 修正
                if (indexStart < 1)
                {
                    indexStart = 1;
                }
                if (indexEnd > this.pageMax)
                {
                    indexEnd = this.pageMax;
                }
                if (this.pageMax <= (DEFAULT_SPLIT_SIZE * 2))
                {
                    indexStart = 1;
                    indexEnd = this.pageMax;
                }
                for (int i = indexStart; i <= indexEnd; i++)
                {
                    if (i == this.pageNow)
                    {
                        result += "<li class=\"active\"><a href=\"javascript:;\">" + i + " <span class=\"sr-only\">(current)</span></a></li>";
                    }
                    else
                    {
                        result += "<li><a href=\"" + this.linkUrl + i + "\">" + i + " <span class=\"sr-only\">(current)</span></a></li>";
                    }
                }
                if (this.pageNow - 1 >= 1)
                {
                    result = "<li><a href=\"" + this.linkUrl + (this.pageNow - 1) + "\">上一页</a></li>" + result;
                }
                else
                {
                    result = "<li class=\"disabled\"><a href=\"javascript:;\">上一页</a></li>" + result;
                }
                if (this.pageNow + 1 <= this.pageMax)
                {
                    result = result + "<li><a href=\"" + this.linkUrl + (this.pageNow + 1) + "\">下一页</a></li>";
                }
                else
                {
                    result = result + "<li class=\"disabled\"><a href=\"javascript:;\">下一页</a></li>";
                }
                if (this.pageNow > 1)
                {
                    result = "<li><a href=\"" + this.linkUrl + 1 + "\">第一页</a></li>" + result;
                }
                else
                {
                    result = "<li class=\"disabled\"><a href=\"javascript:;\">第一页</a></li>" + result;
                }
                if (this.pageNow < this.pageMax)
                {
                    result = result + "<li><a href=\"" + this.linkUrl + this.pageMax + "\">最后一页</a></li>";
                }
                else
                {
                    result = result + "<li class=\"disabled\"><a href=\"javascript:;\">最后一页</a></li>";
                }
                return "<ul class=\"pagination pull-right\" style=\"margin-top:-10px;\"><li class=\"disabled\"><a href=\"javascript:;\">共计" + this.recordCount + "条</a></li>" + result + "</ul>";
            }
            return null;
        }
    }
}