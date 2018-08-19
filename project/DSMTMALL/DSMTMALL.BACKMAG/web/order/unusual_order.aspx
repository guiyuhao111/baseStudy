<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="unusual_order.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.order.abnormal_order" %>

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
    <script src="../../js/web/UnusualOrder.js"></script>
</head>
<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                <ol class="breadcrumb">
                    <li>后台管理</li>
                    <li>异常订单处理</li>
                </ol>
                </div>
            <div class="wrapper">
                <div class="form-inline pull-right">
                    <div class="btn-group">
                        <button type="button" class="btn btn-info dropdown-toggle btn-sm" data-toggle="dropdown">
                            下载 <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu" role="menu">
                            <li><a href="#" class="btn btn-success" mydata="AllList" onclick="ExportExcel(this)">导出异常订单信息</a></li>
                        </ul>
                    </div>
                    <div class="input-group input-group-sm">
                        <input type="text" runat="server" id="iptSearch" class="form-control input-sm" mydata="searchName" placeholder="请输入订单编号" />
                        <span class="input-group-btn">
                            <button runat="server" id="btnSearch" type="button" class="btn btn-primary btn-sm" onclick="btnSearch_Click(this)">
                                <span class="glyphicon glyphicon-search" aria-hidden="true"></span>&nbsp;
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
                                        <th>订单编号</th>
                                        <th>用户信息</th>
                                        <th>订单状态</th>
                                        <th>支付状态</th>
                                        <th>配送状态</th>
                                        <th>订单来源</th>
                                        <th>创建时间</th>
                                        <th>系统操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td name="tNo" class="my-overflow-title" title="<%# Eval("OrderSn") %>"><a tabindex="0" role="button" data-toggle="popover" data-container="body" data-trigger="focus" title="统一下单编号：<%# Eval("OrderUnifySn")%>" data-content="第三方系统单号：<%# Eval("TPLOrderNo")%>=><%# Eval("NIsVerify") %>"><%# Eval("OrderSn") %></a></td>
                                    <td name="tName" class="my-overflow-title" title="<%# Eval("UserName") %>"><a tabindex="0" role="button" data-toggle="popover" data-container="body" data-trigger="focus" title="所属公司名：<%# Eval("CpyName")%>" data-content="消费总额：<%# Eval("CreditLine")%>"><%# Eval("NickName") %></a></td>
                                    <td name="tStatus_1" mydata="<%# Eval("OrderStatus") %>"><a tabindex="0" role="button" data-toggle="popover" data-trigger="focus" data-content="异常状态码：<%# Eval("Postscript")%>"><%# Eval("NOrderStatus") %></a></td>
                                    <td name="tStatus_2" mydata="<%# Eval("PayStatus") %>"><a role="button" data-toggle="modal" data-target="#payInfoModal" onclick="ShowPayInfo(this)"><%# Eval("NPayStatus") %></a></td>
                                    <td name="tStatus_3"><%# Eval("NShippingStatus") %></td>
                                    <td name="tSuppliersName"><%# Eval("SuppliersName") %></td>
                                    <td name="tCreateTime"><%# Eval("AddTime") %></td>
                                    <td name="tOperation" mydata="<%# Eval("OrderID") %>">
                                        <div class="btn-group btn-group-sm">
                                            <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#showInfo" onclick="ShowInfo(this)"><span class="fa fa-search-minus"></span></button>
                                             <div class="btn-group btn-group-sm">
                                                <button data-toggle="dropdown" class="btn btn-info dropdown-toggle" type="button">
                                                    editor <span class="caret"></span>
                                                </button>
                                                <ul role="menu" class="dropdown-menu">
                                                    <li>
                                                         <a href="javascript:;"  data-toggle="modal" data-target="#confirmModal" onclick="ComfirmMsg(this,'确认将当前订单恢复正常？','htOrderToSuccess')">海淘订单推送异常处理</a>
                                                         <a href="javascript:;" data-toggle="modal" data-target="#confirmModal" onclick="ComfirmMsg(this,'确认将重新获取订单流水信息？','xfOrderTradeGetAgain')">XF系统返回流水号异常</a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </td>
                                    <td hidden="hidden" name="tID"><%# Eval("OrderID") %></td>
                                    <td hidden="hidden" name="tPayInfo">
                                        <span><%# Eval("PayName") %></span>
                                        <span><%# Eval("TradeNo") %></span>
                                        <span><%# Eval("MoneyPaid") %></span>
                                        <span><%# Eval("CpyName") %></span>
                                        <span><%# Eval("PayTime") %></span>
                                        <span><%# Eval("NPayStatus") %></span>
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
            <%--=========支付信息=====--%>
            <div class="modal fade" id="payInfoModal" tabindex="-1" role="dialog" aria-labelledby="showInfo" aria-hidden="true">
                <div class="modal-dialog" style="min-width: 100px">
                    <div class="modal-content">
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12 ">
                                    <div class="thumbnail">
                                        <div class="caption">
                                            <h3>交易信息</h3>
                                            <p>流水编号：<span name="pTradeNo"></span></p>
                                            <p>付款方式：<span name="pPayName"></span></p>
                                            <p>付款金额：<span name="pMoneyPaid"></span></p>
                                            <p>付款时间：<span name="pPayTime"></span></p>
                                            <p>所属公司：<span name="pCpyName"></span></p>
                                            <p>支付结果：<span name="pStatus"></span></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
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
