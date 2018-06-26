using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DSMTMALL.MyPublic
{
    public class DdlHelper
    {

        public void BindDdl(DropDownList dropdownlist, DataTable dt, string columeText, string columnValue)
        {
            if (dt.Rows.Count > 0)
            {
                dropdownlist.DataSource = dt;
                dropdownlist.DataTextField = columeText;
                dropdownlist.DataValueField = columnValue;
                dropdownlist.DataBind();
            }
            else
            {
                dropdownlist.Items.Clear();
            }
        }
        public void BindDdl(DropDownList dropdownlist, DataTable dt, string columeTextBefore, string columeTextAfter, string columnValue)
        {
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dropdownlist.Items.Add(new ListItem(dt.Rows[i][columeTextBefore].ToString() + "-" + dt.Rows[i][columeTextAfter].ToString(), dt.Rows[i][columnValue].ToString()));
                }
            }
            else
            {
                dropdownlist.Items.Clear();
            }
        }
        public void BindDdl(HtmlSelect dropdownlist, DataTable dt, string columeText, string columnValue)
        {
            if (dt.Rows.Count > 0)
            {
                dropdownlist.DataSource = dt;
                dropdownlist.DataTextField = columeText;
                dropdownlist.DataValueField = columnValue;
                dropdownlist.DataBind();
            }
            else
            {
                dropdownlist.Items.Clear();
            }
        }
        public void BindDdl(HtmlSelect dropdownlist, DataTable dt, string columeTextBefore, string columeTextAfter, string columnValue)
        {
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dropdownlist.Items.Add(new ListItem(dt.Rows[i][columeTextBefore].ToString() + "-" + dt.Rows[i][columeTextAfter].ToString(), dt.Rows[i][columnValue].ToString()));
                }
            }
            else
            {
                dropdownlist.Items.Clear();
            }
        }
        public void BindDdlMore(DropDownList dropdownlist, DataTable dt, string columeText, string columnValue, string msg)
        {
            if (dt.Rows.Count > 0)
            {
                dropdownlist.DataSource = dt;
                dropdownlist.DataTextField = columeText;
                dropdownlist.DataValueField = columnValue;
                dropdownlist.DataBind();
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
            else
            {
                dropdownlist.Items.Clear();
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
        }
        public void BindDdlMore(DropDownList dropdownlist, DataTable dt, string columeTextBefore, string columeTextAfter, string columnValue, string msg)
        {
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dropdownlist.Items.Add(new ListItem(dt.Rows[i][columeTextBefore].ToString() + "-" + dt.Rows[i][columeTextAfter].ToString(), dt.Rows[i][columnValue].ToString()));
                }
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
            else
            {
                dropdownlist.Items.Clear();
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
        }
        public void BindDdlMore(HtmlSelect dropdownlist, DataTable dt, string columeText, string columnValue, string msg)
        {
            if (dt.Rows.Count > 0)
            {
                dropdownlist.DataSource = dt;
                dropdownlist.DataTextField = columeText;
                dropdownlist.DataValueField = columnValue;
                dropdownlist.DataBind();
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
            else
            {
                dropdownlist.Items.Clear();
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
        }

        /// <summary>
        /// 绑定方法自定义
        /// </summary>
        /// <param name="dropdownlist">控件名称</param>
        /// <param name="dt">数据源</param>
        /// <param name="columeText">显示的文本</param>
        /// <param name="columnValue">隐藏在val里的值</param>
        /// <param name="columnParentsValue">mydata里的值</param>
        /// <param name="msg"></param>
        public void BindDdlParentsMore(HtmlSelect dropdownlist, DataTable dt, string columeText, string columnValue, string columnParentsValue, string msg)
        {
            if (dt.Rows.Count > 0)
            {
                dropdownlist.DataSource = dt;
                dropdownlist.DataTextField = columeText;
                dropdownlist.DataValueField = columnValue;
                dropdownlist.DataBind();
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
                for (int i = 1; i < dropdownlist.Items.Count; i++)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        string dd = dropdownlist.Items[i].Value;
                        string ddd = dt.Rows[j][columnValue].ToString();
                        if (dropdownlist.Items[i].Value == dt.Rows[j][columnValue].ToString())//判断如果该行option的val值与dt的该行val值相等
                        {
                            dropdownlist.Items[i].Attributes["mydata"] = dt.Rows[j][columnParentsValue].ToString();
                            break;
                        }
                    }
                }
            }
            else
            {
                dropdownlist.Items.Clear();
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
        }
        public void BindDdlMore(HtmlSelect dropdownlist, DataTable dt, string columeTextBefore, string columeTextAfter, string columnValue, string msg)
        {
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dropdownlist.Items.Add(new ListItem(dt.Rows[i][columeTextBefore].ToString() + "-" + dt.Rows[i][columeTextAfter].ToString(), dt.Rows[i][columnValue].ToString()));
                }
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
            else
            {
                dropdownlist.Items.Clear();
                dropdownlist.Items.Insert(0, new ListItem(msg, ""));
            }
        }

    }
}