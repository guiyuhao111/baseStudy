<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="data_goodsInfo.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.thirdAdmin.data_goodsInfo" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_confirm.ascx" TagPrefix="uc1" TagName="uc_confirm" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>
<%@ Register Src="~/web/uc/un_theader.ascx" TagPrefix="uc1" TagName="un_theader" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>数据中心</title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:un_theader runat="server" ID="un_theader" />
            <div class="page-heading">

                <ol class="breadcrumb">
                    <li>数据中心</li>
                    <li class="active">销量统计</li>
                </ol>
            </div>
            <div class="wrapper">
                <div class="form-inline pull-right">
                    <div class="input-group input-goup-sm time-css">
                        <input type="text" id="sltByTimeStart" mydata="byTimeStart" runat="server" class="form-control input-sm" placeholder="开始时间" maxlength="25" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'sltByTimeEnd\')||\'2020-10-01\'}', dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <span class="input-group-addon">至</span>
                        <input type="text" id="sltByTimeEnd" mydata="byTimeEnd" runat="server" class="form-control input-sm" placeholder="结束时间" maxlength="25" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'sltByTimeStart\')}', maxDate: '2020-10-01', dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                    </div>
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon">检索条件</span>
                        <select runat="server" id="sltByOrderStatus" class="form-control input-sm" mydata="byOrderStatus">
                            <option value="">总订单合计</option>
                            <option value="1">已完成订单</option>
                            <option value="2">已发货订单</option>
                            <option value="3">发货未收货</option>
                            <option value="5">付款未发货</option>
                        </select>
                    </div>
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
                                        <th>商品编号</th>
                                        <th>商品名称</th>
                                        <th>销量合计</th>
                                        <th>商品价格</th>
                                    </tr>
                                </thead>
                                <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td name="tSn"><%# Eval("GoodsSn")%></td>
                                    <td name="tName"><%# Eval("GoodsName") %></td>
                                    <td name="tNumber"><%# Eval("SUMNumber")%></td>
                                    <td name="tStatus_2"><%# Eval("GoodsPrice")%></td>
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
