<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="unusual_trade.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.order.unusual_trade" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_confirm.ascx" TagPrefix="uc1" TagName="uc_confirm" %>
<%@ Register Src="~/web/uc/uc_headers.ascx" TagPrefix="uc1" TagName="uc_headers" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>后台管理</title>
    <uc1:uc_title runat="server" ID="uc_title" />

</head>
<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                <ol class="breadcrumb">
                    <li>后台管理</li>
                    <li>账单流水</li>
                    <li>（默认当月）总付款流水：<span id="moneyAccount" runat="server"></span></li>
                </ol>
            </div>
            <div class="wrapper">
                <div class="form-inline pull-right">
                    <div class="input-group input-large custom-date-range" >
                        <input type="text" id="sltByTimeStart" class="form-control input-sm dpd1" mydata="byTimeStart" runat="server"  name="from" placeholder="开始时间" maxlength="25" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'sltByTimeEnd\')||\'2020-10-01\'}', dateFmt: 'yyyy-MM-dd HH:mm:ss' })"/>
                        <span class="input-group-addon">至</span>
                        <input type="text" id="sltByTimeEnd" class="form-control input-sm dpd2"  mydata="byTimeEnd" runat="server" name="to" placeholder="结束时间" maxlength="25" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'sltByTimeStart\')}', maxDate: '2020-10-01', dateFmt: 'yyyy-MM-dd HH:mm:ss' })"/>
                    </div>
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon">流水状态</span>
                        <select runat="server" id="sltByComfirmStatus" class="form-control input-sm" mydata="byComfirmStatus">
                            <option value="">全部</option>
                            <option value="10">待确认</option>
                            <option value="20">确认成功</option>
                            <option value="30">支付失败</option>
                            <option value="40">流水异常</option>
                        </select>
                    </div>
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon">支付方式</span>
                        <select runat="server" id="sltByPayID" class="form-control input-sm" mydata="byPayID">
                        </select>
                    </div>
                    <div class="input-group input-group-sm">
                        <input type="text" runat="server" id="iptSearch" class="form-control input-sm" mydata="searchName" placeholder="流水编号/公司名称" />
                        <span class="input-group-btn">
                            <button runat="server" id="btnSearch" type="button" class="btn btn-primary btn-sm" onclick="btnSearch_Click(this)">
                                <span class="fa fa-search" aria-hidden="true"></span>&nbsp;
                            </button>
                        </span>
                    </div>
                </div>

                <div id="divTable">
                    <h4 class="my-h4">流水列表</h4>
                    <table id="tableOrderList" class="table table-striped table-condensed table-hover table-bordered">
                        <asp:Repeater ID="repeaterOrderList" runat="server">
                            <HeaderTemplate>
                                <thead>
                                    <tr class="info">
                                        <th>流水编号</th>
                                        <th>订单支付编号</th>
                                        <th>付款金额</th>
                                        <th>流水状态</th>
                                        <th>支付方式</th>
                                        <th>支付所属公司</th>
                                        <th>支付时间</th>
                                        <th>系统操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td name="tNo" class="my-overflow-title"><%# Eval("TradeNo") %></td>
                                    <td name="tNo_1" class="my-overflow-title"><%# Eval("OrderPayNo") %></td>
                                    <td name="tPrice"><%# Eval("PayMoney") %></td>
                                    <td name="tStatus"><%# Eval("NComfirmStatus") %></td>
                                    <td name="tPayName"><%#Eval("PayName") %></td>
                                    <td name="tName"><%# Eval("CpyName") %></td>
                                    <td name="tTime"><%# Eval("PayTime") %></td>
                                    <td name="tOperation" mydata="<%# Eval("TradeSysID") %>">
                                        <div class="btn-group btn-group-sm">
                                           <%-- <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#showInfo" onclick="ShowInfo(this)"><span class="fa fa-search-minus"></span></button>--%>
                                            <button type="button" class="btn btn-info btn-sm" data-toggle="modal" data-target="#confirmModal" onclick="ComfirmMsg(this,'确认进行手动确认同步流水信息？','xfTradeSyncAgain')"><span class="fa fa-cogs"></span></button>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                    <div runat="server" id="pageNav"></div>
                </div>
            </div>
            <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>
        <div id="divHelper">
            <uc1:uc_confirm runat="server" ID="uc_confirm" />

            <%--=========编辑选项=====--%>
            <div class="modal fade" id="showInfo" tabindex="-1" role="dialog" aria-labelledby="showInfo" aria-hidden="true">
                <div class="modal-dialog" style="min-width: 100px">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12 ">
                                    <div class="thumbnail">
                                        <div class="caption">
                                            <h3>异常描述</h3>
                                            <p>异常原因：<span name="pUnusual"></span></p>
                                            <p>原因概述：<span name="pDescribe"></span></p>
                                            <p>异常状态：<span name="pPostscript"></span></p>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="thumbnail">
                                        <div class="caption" id="orderProductList">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</body>

</html>
