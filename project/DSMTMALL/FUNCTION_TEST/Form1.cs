
using DSMTMALL.Core.Common;
using FUNCTION_TEST.CCC;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FUNCTION_TEST
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 正则表达式      

            //string str = "<img src=\"https://files/imgs/20161215/201612150352085186detial.jpg\" /><img src=\"http://files/imgs/20161215/201612150349185418detial.png\" alt=\"\" /> < h1 >uuuuuuu </ h1 > ";
            ////string reg = "<img(.*?)(src)=\"[^(http)]"; //     [^()]+  非括弧号内内容的其它任意字符
            //string reg = @"(http.*?.jpg)|(http.*?.png)";  //@"(http.*?jpg)";
            //Regex tmpreg = new Regex(reg);
            ////string ddd = string.Empty;

            //Match match = tmpreg.Match(str);
            //MatchCollection group = tmpreg.Matches(str);  //match.Groups;
            //foreach (var item in group)
            //{
            //    MessageBox.Show(item.ToString());
            //}

            ////string s= str;
            ////MatchCollection ce = tmpreg.Matches(str);
            ////foreach (var item in ce)
            ////{
            ////    ddd = item.ToString();
            ////}

            ////string ddsdfd=   tmpreg.Replace(str, "<img src=\"mallmanage.51ipc.com/");
            //// string ddd = string.Empty;

            ////str.Replace(str,    );

            #endregion



            #region 反序列话

            //string s = "[{ Address:'16&25&31',Count:'',Money:'',AddCount:1,AddFee:11},{Address:'16&25&31&33&34&35',Count:11,Money:11,AddCount:11,AddFee:111}]";
            //try
            //{
            //    //FareCarryTemp[] fareCarryTempList = new JavaScriptSerializer().Deserialize<FareCarryTemp[]>(s);

            //}
            //catch (Exception ess)
            //{

            //   string  resWite = ess.Message;
            //}

            #endregion

            #region 接口测试


            //DSMTALL_XFWEBAPISoapClient client = new DSMTALL_XFWEBAPISoapClient();
            //GetClientReadCardEntity entity = new GetClientReadCardEntity();
            //entity.AdminToken = "sss";
            //entity.CardNo = "1002";
            //entity.CheckCode = "2001";
            //new VerifyHelper().EncryptCardEntity(entity);
            //BackClientReadCardEntity backInfo = client.ClientReadCard(entity);
            //new VerifyHelper().CheckCardSign<BackClientReadCardEntity>(ref backInfo);


            #endregion

            //ToolHelper.Post("http://mall.51ipc.com/web/index.aspx",string.Empty);

           


        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ss = new DESHelper().Encrypt("123456");

            string s_1 = textBox1.Text.Trim();
            // textBox2.Text = new DESHelper().Encrypt(s_1);

            #region 身份证验证

            string reg =@"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";
            Regex tmpreg = new Regex(reg);
            bool isPass=  tmpreg.IsMatch(s_1);
            MessageBox.Show(Convert.ToString(isPass));
            #endregion
        }


    }
}
