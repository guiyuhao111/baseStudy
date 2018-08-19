<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="un_theader.ascx.cs" Inherits="DSMTMALL.BACKMAG.web.uc.un_theader" %>
<div class="header-section">
    <a class="toggle-btn"><i class="fa fa-bars"></i></a>
    <div class="menu-right">
        <ul class="notification-menu">
            <li>
                <a href="javascript:;" class="btn btn-default dropdown-toggle info-number" data-toggle="dropdown">
                    <i class="fa fa-bell-o"></i>
                    <span class="badge" id="msgCount" runat="server"></span>
                </a>
                <div class="dropdown-menu dropdown-menu-head pull-right">
                    <h5 class="title">消息通告栏</h5>
                    <ul class="dropdown-list normal-list">
                        <li class="new">
                            <a href="javascript:;" runat="server" id="deliveryOrder">
                                <span class="label label-danger"><i class="fa fa-bolt"></i></span>
                                <span class="name">您有未发货的订单需处理 </span>&nbsp---
                                <em class="small" id="orderCount" runat="server"></em>
                            </a>
                        </li>
                        <li class="new">
                            <a href="javascript:;" runat="server" id="deliverReBack">
                                <span class="label label-danger"><i class="fa fa-bolt"></i></span>
                                <span class="name">您有尚未处理的售后订单 </span>&nbsp---
                                <em class="small" id="changeGoods" runat="server"></em>
                            </a>
                        </li>
                    </ul>
                </div>
            </li>
            <li>
                <a href="javascript:;" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                    <img src="../../images/photos/user-avatar.png" />
                    <span runat="server" id="hMainTitle">管理员</span>
                    <span class="caret"></span>
                </a>
                <ul class="dropdown-menu dropdown-menu-usermenu pull-right">
                    <li><a runat="server" id="aExit" class="pull-right"><i class="fa fa-sign-out"></i>Log Out</a></li>
                </ul>
            </li>

        </ul>
    </div>
</div>
