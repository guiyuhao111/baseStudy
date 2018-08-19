using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DSMTMALL.BACKMAG.MyPublic
{
    public class DdlHelper
    {
        public void BindDdl(HtmlSelect dropdownlist, IEnumerable<dynamic> infoList, string msg, Func<dynamic, ListItem> action)
        {
            if (infoList != null && infoList.Count() > 0)
            {
                foreach (var item in infoList)
                {
                    dropdownlist.Items.Add(action(item));//调用委托
                }
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
            else
            {
                dropdownlist.Items.Clear();
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
        }
        
        /// <summary>
        /// 使用委托实现数据的绑定//在外面调用这个方法的实例：  new DdlHelper().BindDdlMore(sltByCateType, CateInfo,"全部", x =>{ return new ListItem(x.CateName.ToString(), x.CateID.ToString()); });
        /// 如果要实现多加显示的内容或者mydata等自定义属性的话，就在new ListItem(x.CateName.ToString(), x.CateID.ToString())里面加东西，比如加maydata就是new ListItem(x.CateName.ToString(), x.CateID.ToString()).Attributes["mydata"]="要加的值"
        /// 如果要实现文本内容提示只需要 new ListItem(x.CateName.ToString()+ "-" + x.CateName_2.ToString(), x.CateID.ToString())
        /// </summary>
        /// <param name="dropdownlist">select控件</param>
        /// <param name="infoList">绑定的数据源</param>
        /// <param name="msg">默认第一项</param>
        /// <param name="action">委托</param>
        public void BindDdlMore(HtmlSelect dropdownlist, IEnumerable<dynamic> infoList,string msg,Func< dynamic,ListItem > action)//Action<HtmlSelect, dynamic > action定义一个委托
        {
            if (infoList != null && infoList.Count() > 0)
            {
                foreach (var item in infoList)
                {
                  dropdownlist.Items.Add(action(item));//调用委托
                }
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
            else
            {
                dropdownlist.Items.Clear();
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
        }
        
        //用来研究委托的初稿
        //public void ASS(HtmlSelect dropdownlist, IEnumerable<dynamic> infoList)
        //{
        //    foreach (var item in infoList)
        //    {
        //        dropdownlist.Items.Add(new ListItem(item.CateID.ToString(), item.CateName.ToString()));
        //    }
        //}
      
    }
}