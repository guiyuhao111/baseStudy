<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orderList.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.thirdAdmin.orderList" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_confirm.ascx" TagPrefix="uc1" TagName="uc_confirm" %>
<%@ Register Src="~/web/uc/uc_upload.ascx" TagPrefix="uc1" TagName="uc_upload" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>
<%@ Register Src="~/web/uc/un_theader.ascx" TagPrefix="uc1" TagName="un_theader" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>后台管理</title>
    <uc1:uc_title runat="server" ID="uc_title" />
    <script src="../../js/web/OrderList.js"></script>
</head>

<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:un_theader runat="server" ID="un_theader" />
            <div class="page-heading">
                <ol class="breadcrumb">
                    <li>后台管理</li>
                    <li class="active">订单列表</li>
                    <span class="li_a"><a href="javascript:;" onclick="ExportExcel(this)">导出当前筛选信息</a></span>
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
                        <span class="input-group-addon">订单状态</span>
                        <select runat="server" id="sltByOrderStatus" class="form-control input-sm" mydata="byOrderStatus">
                            <option value="">全部订单</option>
                            <option value="10">系统处理中</option>
                            <option value="20">待发货订单</option>
                            <option value="30">已发货订单</option>
                            <option value="40">退换货处理</option>
                            <option value="50">已失效订单</option>
                            <option value="60">已完成订单</option>
                        </select>
                    </div>

                    <div class="input-group input-group-sm" style="display: none">
                        <span class="input-group-addon">支付方式</span>
                        <select runat="server" id="sltByPayID" class="form-control input-sm" mydata="byPayID">
                        </select>
                    </div>
                    <div class="input-group input-group-sm" style="display: none">
                        <span class="input-group-addon">供货商家</span>
                        <select runat="server" id="sltBySuppliers" class="form-control input-sm" mydata="bySuppliers">
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
                                    <td name="tNo" class="width_min_150" title="<%# Eval("OrderSn") %>"><a tabindex="0" role="button" data-toggle="popover" data-container="body" data-trigger="focus" title="统一下单编号：<%# Eval("OrderUnifySn")%>" data-content="第三方系统单号：<%# Eval("TPLOrderNo")%>=><%# Eval("NIsVerify") %>"><%# Eval("OrderSn") %></a></td>
                                    <td name="tName" class="width_min_250" title="<%# Eval("UserName") %>"><a tabindex="0" role="button" data-toggle="popover" data-container="body" data-trigger="focus" title="所属公司名：<%# Eval("CpyName")%>" data-content="消费总额：<%# Eval("CreditLine")%>"><%# Eval("NickName") %>-<%# Eval("UserName") %></a></td>
                                    <td name="tStatus_1" mydata="<%# Eval("OrderStatus") %>"><a tabindex="0" role="button" data-toggle="popover" data-trigger="focus" data-content="异常信息：<%# Eval("Postscript")%>"><%# Eval("NOrderStatus") %></td>
                                    <td name="tStatus_2"  mydata="<%# Eval("PayStatus") %>"><a role="button" data-toggle="modal" data-target="#payInfoModal" onclick="ShowPayInfo(this)"><%# Eval("NPayStatus") %></a></td>
                                    <td name="tStatus_3"><%# Eval("NShippingStatus") %></td>
                                    <td name="tSuppliersName" ><%# Eval("SuppliersName") %></td>
                                    <td name="tCreateTime" ><%# Eval("AddTime") %></td>
                                    <td name="tOperation" class="width_min_100" mydata="<%# Eval("OrderID") %>">
                                        <div class="btn-group btn-group-sm">
                                            <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#showInfo" onclick="ThirdShowInfo(this)"><span class="fa fa-search-minus"></span></button>
                                            <button type="button" class="btn btn-success btn-sm" data-toggle="modal" data-target="#deliveryModal" onclick="ShowDelivery(this)"><span class="fa fa-truck"></span></button>
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
                                    <td hidden="hidden" name="tShippingInfo">
                                        <span><%# Eval("Consignee") %></span>
                                        <span><%# Eval("NAddress") %></span>
                                        <span><%# Eval("Mobile") %></span>
                                        <span><%# Eval("Mobile") %></span>
                                        <span><%# Eval("Logistical") %></span>
                                        <span><%# Eval("ShippingTime") %></span>
                                        <span><%# Eval("LogisticalNumber") %></span>
                                        <span><%# Eval("ShippingFee") %></span>
                                        <span><%# Eval("Remark") %></span>
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
            <uc1:uc_upload runat="server" ID="uc_upload" />
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
            <%--=========查看选项=====--%>
            <div class="modal fade" id="showInfo" tabindex="-1" role="dialog" aria-labelledby="showInfo" aria-hidden="true">
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
                                <div class="col-md-12 ">
                                    <div class="thumbnail">
                                        <div class="caption">
                                            <h3>物流信息</h3>
                                            <p>配送状态：<span name="pShippingStatus"></span></p>
                                            <p>物流名称：<span name="pLogistical"></span></p>
                                            <p>物流单号：<span name="pLogisticalNumber"></span></p>
                                            <p>发货时间：<span name="pShippingTime"></span></p>
                                            <p>收货人名：<span name="pConsignee"></span></p>
                                            <p>收货地址：<span name="pAddress"></span></p>
                                            <p>联系方式：<span name="pPhone"></span></p>
                                            <p>运费价格：<span name="pShippingFee"></span></p>
                                            <p>备注信息：<span name="pRemark"></p>
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
            <%--=========确认发货=====--%>
            <div class="modal fade" id="deliveryModal" tabindex="-1" role="dialog" aria-labelledby="deliveryModal" aria-hidden="true">
                <div class="modal-dialog" style="min-width: 100px">
                    <div class="modal-content">
                        <form>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                <h4 class="modal-title">发货处理</h4>
                            </div>

                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12 ">
                                        <div class="thumbnail">
                                            <div class="caption">
                                                <h3>物流信息</h3>
                                                <p>配送状态：<span name="pShippingStatus"></span></p>
                                                <p>收货人名：<span name="pConsignee"></span></p>
                                                <p>收货地址：<span name="pAddress"></span></p>
                                                <p>联系方式：<span name="pPhone"></span></p>
                                                <p>运费价格：<span name="pShippingFee"></span></p>
                                                <p>发货时间：<span name="pShippingTime"></span></p>
                                                <p>物流公司：<span name="pLogisticalName"></span></p>
                                                <p>物流单号：<span name="pLogisticalNumber"></span></p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="logisticalName">物流公司：</label>
                                            <select id="logisticalName" class="form-control input-sm" required="required">
                                                <option value="汇通快递">汇通快递</option>
                                                <option value="申通快递">申通快递</option>
                                                <option value="送货上门">送货上门</option>
                                                <option value="韵达快递">韵达快递</option>
                                                <option value="圆通快递">圆通快递</option>
                                                <option value="顺丰快递">顺丰快递</option>
                                                <option value="天天快递">天天快递</option>
                                                <option value="中通快递">中通快递</option>
                                                <option value="定点自取">已到达指定地点</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="logisticalNumber">物流单号：</label>
                                            <input type="text" class="form-control" id="logisticalNumber" placeholder="请输入物流单号..." required="required" />
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label for="remark">备注信息</label>
                                            <textarea id="remark" rows="3" class="form-control"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="modal-footer">
                                <button class="btn btn-primary btn-sm" onclick="ConfirmDelivery(this)">确认</button>
                                <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>

    </section>
</body>
</html>
