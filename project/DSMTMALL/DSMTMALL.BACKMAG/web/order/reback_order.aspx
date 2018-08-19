<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reback_order.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.order.reback_order" %>

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
    <script src="../../js/web/ReBackOrder.js"></script>
</head>
<body class="sticky-header">
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
                <ol class="breadcrumb">
                    <li>后台管理</li>
                    <li class="active">退货订单</li>
                </ol>
            </div>
            <div class="wrapper">
                <div class="form-inline pull-right">
                    <div class="input-group input-goup-sm time-css">
                        <input type="text" id="sltByTimeStart" mydata="byTimeStart" runat="server" class="form-control input-sm" placeholder="开始时间" maxlength="25" onclick="WdatePicker({ maxDate: '#F{$dp.$D(\'sltByTimeEnd\')||\'2020-10-01\'}', dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                        <span class="input-group-addon">至</span>
                        <input type="text" id="sltByTimeEnd" mydata="byTimeEnd" runat="server" class="form-control input-sm" placeholder="结束时间" maxlength="25" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'sltByTimeStart\')}', maxDate: '2020-10-01', dateFmt: 'yyyy-MM-dd HH:mm:ss' })" />
                    </div>
                    <div class="input-group input-group-sm" style="display: none">
                        <span class="input-group-addon">支付方式</span>
                        <select runat="server" id="sltByPayID" class="form-control input-sm" mydata="byPayID">
                        </select>
                    </div>
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon">供货商家</span>
                        <select runat="server" id="sltBySuppliers" class="form-control input-sm" mydata="bySuppliers">
                        </select>
                    </div>
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon">售后类别</span>
                        <select runat="server" id="sltByReBackType" class="form-control input-sm" mydata="byReBackType">
                            <option value="">全部</option>
                            <option value="10">退货退款</option>
                            <option value="20">退换商品</option>
                            <option value="30">仅退款</option>
                        </select>
                    </div>
                      <div class="input-group input-group-sm">
                        <span class="input-group-addon">审核状态</span>
                        <select runat="server" id="sltAuthType" class="form-control input-sm" mydata="byAuthType">
                            <option value="">全部</option>
                            <option value="10">处理中</option>
                            <option value="20">已完成</option>
                        </select>
                    </div>
                    <div class="input-group input-group-sm">
                        <span class="input-group-addon"><i class="fa fa-sort-alpha-asc"></i></span>
                        <select runat="server" id="sltOrderBy" class="form-control input-sm" mydata="byOrderBy">
                            <option value="0">按订单付款时间</option>
                            <option value="1">按申请创建时间</option>
                            <option value="2">按申请修改时间</option>
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
                                        <th>退换货类型</th>
                                        <th>审核状态</th>
                                        <th>订单状态</th>
                                        <th>支付状态</th>
                                        <th>配送状态</th>
                                        <th>订单来源</th>
                                        <th>申请时间</th>
                                        <th>系统操作</th>
                                    </tr>
                                </thead>
                                <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td name="tNo" class="width_min_150"><a tabindex="0" role="button" data-toggle="popover" data-container="body" data-trigger="focus" title="统一下单编号：<%# Eval("OrderUnifySn")%>"><%# Eval("OrderSn") %></a></td>
                                    <td name="tName" class="width_min_150" title="<%# Eval("UserName") %>"><a tabindex="0" role="button" data-toggle="popover" data-container="body" data-trigger="focus" title="所属公司名：<%# Eval("CpyName")%>"><%# Eval("NickName") %>-<%# Eval("UserName") %></a></td>
                                    <td name="tType"><%# Eval("NReBackType") %></td>
                                    <td name="tStatus" mydata="<%# Eval("AuthType") %>"><%# Eval("NAuthType") %></td>
                                    <td name="tStatus_1"><%# Eval("NOrderStatus") %></td>
                                    <td name="tStatus_2"><%# Eval("NPayStatus") %></td>
                                    <td name="tStatus_3"><%# Eval("NShippingStatus") %></td>
                                    <td name="tSuppliersName"><%# Eval("SuppliersName") %></td>
                                    <td name="tCreateTime"><a tabindex="0" role="button" data-toggle="popover" data-container="body" data-trigger="focus" data-title="最后操作人： <%# Eval("ModifyPerson") %>--<%# Eval("AddTime") %> " data-content="订单日志：<%# Eval("LogInfo")%>"><%# Eval("CreatTime") %></a></td>
                                    <td name="tOperation" mydata="<%# Eval("ReBackID") %>">
                                        <div class="btn-group btn-group-sm">
                                            <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#showInfo" onclick="ShowInfo(this)"><span class="fa fa-search-minus"></span></button>
                                            <button type="button" class="btn btn-success btn-sm" data-toggle="modal" data-target="#followUpModal" onclick="ShowFollowUp(this)"><span class="fa fa-check-square-o"></span></button>
                                            <button type="button" class="btn btn-info btn-sm" data-toggle="modal" data-target="#refundModal" onclick="ShowRefund(this)"><span class="fa fa-credit-card-alt"></span></button>
                                        </div>
                                    </td>
                                    <td hidden="hidden" name="tID"><%# Eval("ReBackID") %></td>
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
                                        <span><%# Eval("ConsigneeCardNo") %></span>
                                        <span><%# Eval("OLogistical") %></span>
                                        <span><%# Eval("ShippingTime") %></span>
                                        <span><%# Eval("OLogisticalNumber") %></span>
                                        <span><%# Eval("ShippingFee") %></span>
                                        <span><%# Eval("Remark") %></span>
                                    </td>
                                    <td hidden="hidden" name="tOrderInfo">
                                        <span><%# Eval("OrderSn") %></span>
                                        <span><%# Eval("NOrderStatus") %></span>
                                        <span><%# Eval("SuppliersName") %></span>
                                        <span><%# Eval("OrderAmount") %></span>
                                        <span><%# Eval("AddTime") %></span>
                                        <span><%# Eval("ReConnection") %></span>
                                        <span><%# Eval("ReBackAddress") %></span>
                                        <span><%# Eval("Logistical") %></span>
                                        <span><%# Eval("LogisticalNumber") %></span>
                                        <span><%# Eval("ReBackDesc") %></span>
                                        <span><%# Eval("ReBackRemark") %></span>
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
                                            <p>身份证号：<span name="pConsigneeCardNo"></span></p>
                                            <p>收货地址：<span name="pAddress"></span></p>
                                            <p>联系方式：<span name="pPhone"></span></p>
                                            <p>运费价格：<span name="pShippingFee"></span></p>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12 ">
                                    <div class="thumbnail">
                                        <div class="caption">
                                            <h3>退货信息</h3>
                                            <p>订单编号：<span name="pOrderNo"></span></p>
                                            <p>订单状态：<span name="pOrderstatus"></span></p>
                                            <p>供货商家：<span name="pSuppliers"></span></p>
                                            <p>订单总额：<span name="pOrderAmount"></span></p>
                                            <p>下单时间：<span name="pCreatTime"></span></p>
                                            <p>退换货类型：<span name="pReBackType"></span></p>
                                            <p>审核状态：<span name="pAuthType"></span></p>
                                            <p>联系方式：<span name="pConnection"></span></p>
                                            <p>退货地址：<span name="pBackAddress"></span></p>
                                            <p>退回物流公司：<span name="pBackLogistical"></span></p>
                                            <p>退回物流单号：<span name="pBackLogisticalNumber"></span></p>
                                            <p>退货原因：<span name="pRemark"></span></p>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="thumbnail">
                                        <div class="caption" id="orderProductList">
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="thumbnail">
                                        <div class="caption" id="operation">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label for="reBackIsPass">是否通过：</label>
                                                        <select id="reBackIsPass" class="form-control input-sm" required="required">
                                                            <option value="20">通过</option>
                                                            <option value="90">不通过</option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label for="reBackAddress">退回地址：</label>
                                                        <input type="text" class="form-control" id="reBackAddress" placeholder="请输入退回的地址..." required="required" value="浙江宁波高新区软件园区A座410室" />
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label for="reBackRemark">意见备注</label>
                                                        <textarea id="reBackRemark" rows="3" class="form-control"></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-primary btn-sm" onclick="ConfirmRebackInfo(this)">确认</button>
                            <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="followUpModal" tabindex="-1" role="dialog" aria-labelledby="followUpModal" aria-hidden="true">
                <div class="modal-dialog" style="min-width: 100px">
                    <div class="modal-content">
                        <form onsubmit="return false;">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                <h4 class="modal-title">退换货后续处理</h4>
                            </div>
                            <div class="modal-body">
                                <h4>提示：<span>请确认寄回的商品无误后再进行确认操作</span></h4>
                                <hr />
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="reBackGoodsIsOK">是否通过：</label>
                                            <select id="reBackGoodsIsOK" class="form-control input-sm" required="required">
                                                <option value="30">商品无误申请通过</option>
                                                <option value="90">商品存在问题申请不允通过</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label for="reBackGoodsRemark">备注信息</label>
                                            <textarea id="reBackGoodsRemark" rows="3" class="form-control" required></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button class="btn btn-primary btn-sm" onclick="ConfirmReBackGoods(this)">确认</button>
                                <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                            </div>
                        </form>
                    </div>

                </div>
            </div>

            <div class="modal fade" id="refundModal" tabindex="-1" role="dialog" aria-labelledby="refundModal" aria-hidden="true">
                <div class="modal-dialog" style="min-width: 100px">
                    <div class="modal-content">
                        <form onsubmit="return false;">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                <h4 class="modal-title">退款处理</h4>
                            </div>
                            <div class="modal-body">
                                <h4>提示：<span>请谨慎操作退款处理 当前订单总金额：<em id="emGooodsAount"></em></span></h4>
                                <hr />
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <input type="datetime" class="form-control" id="rebackMoney" placeholder="请输入要退回的金额" required="required" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button class="btn btn-primary btn-sm" onclick="ConfirmReBackMoney(this)">确认</button>
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
