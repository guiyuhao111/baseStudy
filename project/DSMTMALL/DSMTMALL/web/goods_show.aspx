<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="goods_show.aspx.cs" Inherits="DSMTMALL.web.goods_show" %>

<%@ OutputCache Duration="60" VaryByParam="goodsId" %>
<%@ Register Src="~/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc_show.ascx" TagPrefix="uc1" TagName="uc_show" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>51IPC购物平台</title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="divPageGoodsShow" data-role="page">
            <script src="/js/html/goods_show.js"></script>
            <div data-role="header" data-tap-toggle="false" data-theme="j" style="background-color: transparent;">
                <span id="userID" runat="server" hidden="hidden"></span>
                <uc1:uc_show runat="server" ID="uc_show" />
            </div>
            <%--====头部结束===--%>
            <div data-role="content" class="my-content-color" data-theme="j">
                <div class="header-back-trans">
                    <a href="javascript:;" data-rel="back">
                        <img src="../../images/web/hpage/rightarr.png" />
                    </a>
                    <span></span>
                </div>
                <div class="flexslider" style="margin-top: 0px;">
                    <ul id="carousel-slider" class="slides">
                        <asp:Repeater ID="repeaterGoodsPicetureShow" runat="server">
                            <ItemTemplate>
                                <li><a>
                                    <img alt="pic" src="<%# Eval("NImgUrl")%>" style="width: 100%" onerror="nofind()" /></a></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <asp:Repeater ID="repeaterGoodsInfo" runat="server">
                    <ItemTemplate>
                        <span class="drink-show-name"><%# Eval("GoodsName")%></span>
                        <table id="tableGoodsShow" class="drink-show-table" style="width: 100%; display: none;">
                            <tr>
                                <td class="drink-show-left">品牌：</td>
                                <td class="drink-show-right">
                                    <input type="hidden" id="goodsId" value='<%# Eval("GoodsID")%>' />
                                    『<%# Eval("BrandName")%>』
                            <span id="spanShow" class="spanShow">更多&nbsp<i><img src="../../images/web/hpage/add.png" /></i></span></td>
                            </tr>
                            <tr id="tableGoodsShow_tr_type" style="display: none">
                                <td class="drink-show-left">类别：</td>
                                <td class="drink-show-right">
                                    <%# Eval("CateName")%></td>
                            </tr>
                            <tr id="tableGoodsShow_tr_capacity" style="display: none">
                                <td class="drink-show-left">库存：</td>
                                <td class="drink-show-right">
                                    <%# Eval("GoodsNumber")%><span>件</span></td>
                            </tr>
                            <tr id="tableGoodsShow_tr_degrees" style="display: none">
                                <td class="drink-show-left">编号：</td>
                                <td class="drink-show-right">
                                    <%# Eval("GoodsSn")%></td>
                            </tr>
                            <tr id="tableGoodsShow_tr_weight" style="display: none">
                                <td class="drink-show-left">重量：</td>
                                <td class="drink-show-right">
                                    <%# Eval("Weight")%><span>(KG)</span></td>
                            </tr>
                             <tr id="tableGoodsShow_tr_quotaNumber" style="display: none">
                                <td class="drink-show-left">限购：</td>
                                <td class="drink-show-right" ><%#Eval("QuotaNumber") %>
                                   <span id="quotaNumber" mydata="<%#Eval("QuotaNumber") %>">(<%# Eval("NQuotaNumber")%>)</span></td>
                            </tr>
                            <tr id="tableGoodsShow_tr_factory" style="display: none">
                                <td class="drink-show-left">货商：</td>
                                <td class="drink-show-right">
                                    <%# Eval("SuppliersName")%></td>
                            </tr>
                            <tr id="tableGoodsShow_tr_fareMoney" style="display: none">
                                <td class="drink-show-left">运费：</td>
                                <td class="drink-show-right">
                                    <%# Eval("NFareInfo")%></td>
                            </tr>
                            <tr id="tableGoodsShow_tr_fareTemp" style="display: none">
                                <td class="drink-show-left">配送：</td>
                                <td class="drink-show-right">
                                    <%# Eval("NFareType")%>&nbsp &nbsp<span>配送时间：<%# Eval("FareTime") %>小时内</span></td>
                            </tr>
                            <tr>
                                <td class="drink-show-left">售价：</td>
                                <td class="drink-show-price">&yen;<%# Eval("ShopPrice")%>
                                    <span><strong>&yen;<%# Eval("MarketPrice")%></strong></span>
                                </td>
                            </tr>
                            <tr>
                                <td class="drink-show-left">数量：</td>
                                <td class="drink-show-right">
                                    <button id="btnReduce" data-role="none" class="btnReduce">-</button>
                                    <input type="number" id="tbNumber" data-role="none" class="tbNumber" value="1" />
                                    <button id="btnAdd" data-role="none" class="btnAdd">+</button>
                                    <span class="drink-show-sale">已售：<strong><%# Eval("SaleNumber") %></strong></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="drink-show-colspan">
                                    <%# Eval("GoodsBrief")%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><span><%# Eval("GoodsDesc") %></span></td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
                <%--====表结束====--%>
            </div>
            <div data-role="footer" id="btnFooter" data-position="fixed" data-tap-toggle="false">
                <div data-role="navbar" class="footer_css">
                    <ul style="align-content: center;">
                        <li style="width: 25%">
                            <a id="btnFooterOne" href="/web/mall_index.aspx" data-transition="slide" data-direction="reverse">
                                <img src="../images/web/hpage/index1.png" /><span>首页</span></a>
                        </li>
                        <li style="width: 25%"><a id="btnFooterThree" href="/web/goods_cart.aspx" data-transition="slide">
                            <img src="../images/web/hpage/cart_1.png" /><span>购物车</span></a></li>
                        <li style="width: 50%; height: 60px;"><a id="btnAddToCart" href="javascript:;" data-role="button" style="display: block; height: 65px; line-height: 45px; background-color: #2AA9DC; color: #ffffff; text-shadow: none; font-weight: 100;">加入购物车</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
