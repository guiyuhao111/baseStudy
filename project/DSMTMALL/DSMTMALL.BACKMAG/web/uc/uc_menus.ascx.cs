using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Text;
using System.Web;

namespace DSMTMALL.BACKMAG.web.uc
{
    public partial class uc_menus : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder strMenu = new StringBuilder();
            if (WebLoginHelper.IsAuthority(AdminAuth.AdminManage))//超级管理员
            {
                strMenu.Append(" <li id=\"user_manage\" ><a href=\"/web/admin/web_user_list.aspx\"><i class=\"fa fa-users\"></i> <span>用户管理</span></a></li > ");
                strMenu.Append(" <li id=\"admin_manage\" ><a href=\"/web/admin/web_admin_list.aspx\"><i class=\"fa fa-user-secret\"></i> <span>账号管理</span></a></li > ");
            }
            if (WebLoginHelper.IsAuthority(AdminAuth.GoodsManage) || WebLoginHelper.IsAuthority(AdminAuth.BackgroundManage))//产品管理员
            {
                strMenu.Append("<li id=\"goodsManage\" class=\"menu-list \"><a href=\"#\"><i class=\"fa fa-home\"></i> <span>商品管理</span></a> ");
                strMenu.Append("<ul class=\"sub-menu-list\">");
                strMenu.Append("<li id=\"goods_manage\"><a href=\"/web/goods/goods_list.aspx\" ><i class=\"fa fa-envira\"></i>产品管理</a></li>");
                strMenu.Append("<li id=\"recom_manage\"><a href=\"/web/goods/goods_recom.aspx\" ><i class=\"fa fa-share-alt\"></i>商品推荐</a></li>");
                strMenu.Append("<li id=\"category_manage\"><a href=\"/web/database/category.aspx\" ><i class=\"fa fa-bars\"></i>类目管理</a></li>");
                strMenu.Append("<li id=\"brand_manage\"><a href=\"/web/database/brand.aspx\" ><i class=\"fa fa-flag\"></i>品牌管理</a></li>");
                strMenu.Append("<li id=\"recom_manage\"><a href=\"/web/database/supplier_list.aspx\" ><i class=\"fa fa-handshake-o\"></i>商家合作</a></li>");
                strMenu.Append("<li id=\"roll_manage\"><a href=\"/web/database/roll.aspx\" ><i class=\"fa fa-map-o\"></i>轮播管理</a></li>");
                strMenu.Append("<li id=\"fare_manage\"><a href=\"/web/database/fareDeliery_list.aspx\" ><i class=\"fa fa-twitter\"></i>运费管理</a></li>");
                strMenu.Append("<li id=\"file_manage\"><a href=\"/web/database/imges.aspx\" ><i class=\"fa fa-file-image-o\"></i>图片管理</a></li>");
                strMenu.Append("<li id=\"log_manage\"><a href=\"/web/database/log_error.aspx\" ><i class=\"fa fa-file-text-o\"></i>错误日志</a></li>");
                strMenu.Append("</ul>");
                strMenu.Append(" </li> ");
                strMenu.Append("<li id=\"orderManage\"  class=\"menu-list \"><a href=\"#\"><i class=\"fa fa-vimeo-square\"></i> <span>订单管理</span></a> ");
                strMenu.Append("<ul class=\"sub-menu-list\">");
                strMenu.Append("<li id=\"order_List\"><a href=\"/web/order/order_list.aspx\"><i class=\"fa fa-truck\"></i>订单查看</a></li>");
                strMenu.Append("<li id=\"reback_order\"><a href=\"/web/order/reback_order.aspx\"><i class=\"fa fa-bell-o\"></i>退换商品</a></li>");
                strMenu.Append("<li id=\"unusual_order\"><a href=\"/web/order/unusual_order.aspx\"><i class=\"fa fa-exclamation-triangle\"></i>异常订单</a></li>");
                strMenu.Append("<li id=\"trade_List\"><a href=\"/web/order/unusual_trade.aspx\"><i class=\"fa fa-money\"></i>支付流水</a></li>");
                strMenu.Append("<li id=\"reback_trade\"><a href=\"/web/order/reback_trade.aspx\"><i class=\"fa fa-money\"></i>退款流水</a></li>");
                strMenu.Append("</ul>");
                strMenu.Append(" </li> ");
            }
            else if (WebLoginHelper.IsAuthority(AdminAuth.SuppliersManage))//第三方托管
            {
                strMenu.Append("<li id=\"third_goodsmanage\"><a href=\"/web/thirdAdmin/goodsList.aspx\" ><i class=\"fa fa-envira\"></i><span>产品管理</span></a></li>");
                strMenu.Append("<li id=\"third_ordermanage\"><a href=\"/web/thirdAdmin/orderList.aspx\" ><i class=\"fa fa-vimeo-square \"></i><span>订单管理</span></a></li>");
                strMenu.Append("<li id=\"dataCenterManage\"  class=\"menu-list\"><a href=\"javascript:;\"><i class=\"fa fa-pie-chart\"></i> <span>数据中心</span></a> ");
                strMenu.Append("<ul class=\"sub-menu-list\">");
                strMenu.Append("<li id=\"data_order_count\"><a href=\"/web/thirdAdmin/data_orderInfo.aspx\"><i class=\"fa fa-bar-chart\"></i>订单统计</a></li>");
                strMenu.Append("<li id=\"data_goods_count\"><a href=\"/web/thirdAdmin/data_goodsInfo.aspx\"><i class=\"fa fa-line-chart\"></i>商品统计</a></li>");
                strMenu.Append("<li id=\"data_reback_order\"><a href=\"/web/thirdAdmin/data_backorder.aspx\"><i class=\"fa fa-bar-chart-o\"></i>售后统计</a></li>");
                strMenu.Append("<li id=\"data_reback_goods\"><a href=\"/web/thirdAdmin/data_backgoods.aspx\"><i class=\"fa fa-bar-chart-o\"></i>售后商品统计</a></li>");
                strMenu.Append("</ul>");
                strMenu.Append("</li> ");
            }
            strMenu.Append("<li id=\"systemManage\"  class=\"menu-list\"><a href=\"javascript:;\"><i class=\"fa fa-cog\"></i> <span>系统设置</span></a> ");
            strMenu.Append("<ul class=\"sub-menu-list\">");
            strMenu.Append("<li id=\"change_pwd\"><a href=\"/web/admin/web_admin_pwd.aspx\"><i class=\"fa fa-pencil\"></i>修改密码</a></li>");
            strMenu.Append("</ul>");
            strMenu.Append(" </li> ");
            strMenu.Append(" <li><a href=\"/web/web_login.aspx\"><i class=\"fa fa-sign-in\"></i> <span>退出</span></a></li>");
            ulMenu.InnerHtml = strMenu.ToString();
        }

    }
}