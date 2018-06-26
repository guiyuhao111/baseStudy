using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Windows.Forms;

namespace MYTESTITME
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text=  new DESHelper().Encrypt(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //获取需要进行状态查询的订单
            //DSMTMALL.DB.Model.M_OrderInfo orderInfo = new DSMTMALL.DB.BLL(DBEnum.Slave).GetModal();
        }
    }
}
