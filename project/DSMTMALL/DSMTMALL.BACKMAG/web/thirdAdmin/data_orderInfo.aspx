<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="data_orderInfo.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.thirdAdmin.data_orderInfo" %>

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
                    <li class="active">已发货订单</li>
                    <li>(默认当月)总发货订单量：<span id="orderCount" runat="server"></span></li>
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
                            <option value="0">已发货订单</option>
                            <option value="1">已完成订单</option>
                            <option value="2">发货未收货</option>
                        </select>
                    </div>
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon"><i class="fa fa-sort-alpha-asc"></i></span>
                        <select runat="server" id="sltOrderBy" class="form-control input-sm" mydata="byOrderBy">
                            <option value="0">按付款时间</option>
                            <option value="1">按创建时间</option>
                            <option value="2">按修改时间</option>
                        </select>
                    </div>
                    <div class="input-group input-group-sm">
                        <input type="text" runat="server" id="iptSearch" class="form-control input-sm" mydata="searchName" placeholder="请输入订单编号" />
                        <span class="input-group-btn">
                            <button runat="server" id="btnSearch" type="button" class="btn btn-primary btn-sm" onclick="btnSearch_Click(this)">
                                <span class="fa fa-search" aria-hidden="true"></span>&nbsp;
                            </button>
                        </span>
                    </div>
                </div>
                <div id="divTable">
                    <h4 class="my-h4">订单列表</h4>
                    <table id="tableOrderList" class="table table-striped table-condensed table-hover table-bordered">
                        <asp:Repeater ID="repeaterOrderList" runat="server">
                            <HeaderTemplate>
                                <thead>
                                    <tr class="info">
                                        <th>订单编号</th>
                                        <th>订单金额</th>
                                        <th>订单状态</th>
                                        <th>支付状态</th>
                                        <th>配送状态</th>
                                        <th>订单来源</th>
                                        <th>创建时间</th>
                                        <th>发货时间</th>
                                        <th>完成时间</th>
                                        <th>商品详情</th>
                                    </tr>
                                </thead>
                                <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td name="tSn" class="width_min_150"><%# Eval("OrderSn")%></td>
                                    <td name="tPrice"><%# Eval("OrderAmount")%></td>
                                    <td name="tStatus">
                                        <%# Eval("NOrderStatus") %>
                                    </td>
                                    <td name="tStatus_2"><%# Eval("NPayStatus")%></td>
                                    <td name="tStatus_3"><%# Eval("NShippingStatus")%></td>
                                    <td name="tSuppliersName" class="my-overflow-title"><%# Eval("SuppliersName") %></td>
                                    <td><%# Eval("AddTime")%></td>
                                    <td><%# Eval("ShippingTime")%></td>
                                    <td><%# Eval("ConfirmTime")%></td>
                                    <td><a tabindex="0" role="button" data-toggle="popover" data-trigger="focus" data-container="body" data-placement="left" data-content="商品详情：<%# Eval("NOrderInfo")%>">更多</a></td>
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
