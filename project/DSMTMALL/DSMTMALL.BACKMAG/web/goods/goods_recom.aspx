<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="goods_recom.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.goods.goods_recom" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_confirm.ascx" TagPrefix="uc1" TagName="uc_confirm" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_headers.ascx" TagPrefix="uc1" TagName="uc_headers" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>后台管理</title>
    <uc1:uc_title runat="server" ID="uc_title" />
    <script>
        $(function () {
            $("#tableOrderList").find("tr").each(function () {    //判断是否启用后对按钮的显示内容进行更改
                if ($.trim($(this).find("[name=tIsEnable]").eq(0).attr("mydata")) == "1") {
                    $(this).find("[name=tIsEnable]").eq(0).find("[name=isCheckEnable]").attr("checked", "checked");
                }
            });
        });
    </script>
</head>
<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                <ol class="breadcrumb">
                    <li>后台管理</li>
                    <li class="active">商品推荐</li>
                </ol>
            </div>
            <div class="wrapper">
                <div class="form-inline pull-right">
                    <div class="input-group input-group-sm">
                        <input type="text" runat="server" id="iptSearch" class="form-control input-sm" mydata="searchName" placeholder="请输入商品编号或名称" />
                        <span class="input-group-btn">
                            <button runat="server" id="btnSearch" type="button" class="btn btn-primary btn-sm" onclick="btnSearch_Click(this)">
                                <span class="fa fa-search" aria-hidden="true"></span>&nbsp;
                            </button>
                        </span>
                    </div>
                </div>
                <div id="divTable">
                    <h4 class="my-h4">商品列表</h4>
                    <table id="tableOrderList" class="table table-striped table-condensed table-hover table-bordered">
                        <asp:Repeater ID="repeaterOrderList" runat="server">
                            <HeaderTemplate>
                                <thead>
                                    <tr class="info">
                                        <th>图片</th>
                                        <th>商品编号</th>
                                        <th>商品名称</th>
                                        <th>所属公司</th>
                                        <th>商品价格</th>
                                        <th>是否启用</th>
                                        <th>商品编辑</th>
                                    </tr>
                                </thead>
                                <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="info_val_height">
                                    <td name="tImg" style="width: 50px;">
                                        <img src="<%# Eval("GoodsImg")%>" style="width: 50px" onerror="nofind()" /></td>
                                    <td name="tSn"><%# Eval("GoodsSn")%></td>
                                    <td name="tName"><%# Eval("GoodsName") %></td>
                                    <td name="tName_2"><%# Eval("CpyName")%></td>
                                    <td name="tPrice"><%# Eval("MarketPrice")%></td>
                                    <td name="tIsEnable" mydata="<%# Eval("IsEnable") %>" class="width_3">
                                        <input type="checkbox" name="isCheckEnable" onchange="ChangeCheckBox(this,'goodsIsEnable')"></td>
                                    <td name="tOperation" mydata="<%# Eval("CpyGoodsID")%>">
                                        <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#confirmModal" onclick="DeleteThis(this,'确认要删除吗？','deleteCpyGoods')"><span class="fa fa-trash-o"></span></button>
                                    </td>
                                    <td name="tID" hidden="hidden"><%# Eval("GoodsID") %></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                    <div runat="server" id="pageNav">
                    </div>
                </div>
            </div>
            <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>
        <uc1:uc_confirm runat="server" ID="uc_confirm" />
    </section>
</body>
</html>
