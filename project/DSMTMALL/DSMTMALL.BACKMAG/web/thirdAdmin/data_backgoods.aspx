<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="data_backgoods.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.thirdAdmin.data_backgoods" %>
<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_confirm.ascx" TagPrefix="uc1" TagName="uc_confirm" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>
<%@ Register Src="~/web/uc/un_theader.ascx" TagPrefix="uc1" TagName="un_theader" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>数据中心</title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
           
            <uc1:un_theader runat="server" id="un_theader" />
            <div class="page-heading">
             
                    <ol class="breadcrumb">
                        <li>数据中心</li>
                        <li class="active">销量统计</li>
                    </ol>   </div>
            <div class="wrapper">
                    <div class="form-inline pull-right">
                        <div class="input-group input-goup-sm time-css">
                            <input type="text" id="sltByTimeStart" mydata="byTimeStart" runat="server" class="form-control input-sm" placeholder="开始时间" maxlength="25" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'sltByTimeEnd\')||\'2020-10-01\'}', dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                            <span class="input-group-addon">至</span>
                            <input type="text" id="sltByTimeEnd" mydata="byTimeEnd" runat="server" class="form-control input-sm" placeholder="结束时间" maxlength="25" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'sltByTimeStart\')}', maxDate: '2020-10-01', dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        </div>
                       <div class="input-group input-group-sm">
                            <span class="input-group-addon">处理结果</span>
                            <select runat="server" id="sltByOrderStatus" class="form-control input-sm" mydata="byOrderStatus">
                                <option value="10">审核已通过</option>
                                <option value="20">审核不通过</option>
                                <option value="30">流程处理中</option>
                            </select>
                        </div>
                         <div class="input-group input-group-sm">
                            <span class="input-group-addon">售后类别</span>
                            <select runat="server" id="sltReBackType" class="form-control input-sm" mydata="byReBackType">
                                <option value="">全部</option>
                                <option value="10">退货退款</option>
                                <option value="20">仅换货</option>
                                <option value="30">仅退款</option>
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
                                            <th>退货退款商品数量合计</th>
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
