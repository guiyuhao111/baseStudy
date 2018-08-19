<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_headers.ascx.cs" Inherits="DSMTMALL.BACKMAG.web.uc.uc_headers" %>
<div class="header-section">

    <!--toggle button start 开关按钮用来缩放目录栏-->
    <a class="toggle-btn"><i class="fa fa-bars"></i></a>

    <!--search start 搜索栏按钮--先隐藏去掉-->
    <%--<form class="searchform" action="index.html" method="post">
                <input type="text" class="form-control" name="keyword" placeholder="Search here..." />
            </form>--%>

    <!--notification menu start 头部右边的导航栏内容 -->
    <div class="menu-right">
        <ul class="notification-menu">
            <%-- <li>
                        <a href="#" class="btn btn-default dropdown-toggle info-number" data-toggle="dropdown">
                            <i class="fa fa-tasks"></i>
                            <span class="badge">8</span>
                        </a>
                        <div class="dropdown-menu dropdown-menu-head pull-right">
                            <h5 class="title">You have 8 pending task</h5>
                            <ul class="dropdown-list user-list">
                               <li class="new">
                                    <a href="#">
                                        <div class="task-info">
                                            <div>Database update</div>
                                        </div>
                                        <div class="progress progress-striped">
                                            <div style="width: 40%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="40" role="progressbar" class="progress-bar progress-bar-warning">
                                                <span class="">40%</span>
                                            </div>
                                        </div>
                                    </a>
                                </li>
                                <li class="new"><a href="">See All Pending Task</a></li>
                            </ul>
                        </div>
                    </li>
                    <li>
                        <a href="#" class="btn btn-default dropdown-toggle info-number" data-toggle="dropdown">
                            <i class="fa fa-envelope-o"></i>
                            <span class="badge">5</span>
                        </a>
                        <div class="dropdown-menu dropdown-menu-head pull-right">
                            <h5 class="title">You have 5 Mails </h5>
                            <ul class="dropdown-list normal-list">
                                <li class="new">
                                    <a href="">
                                        <span class="thumb"><img src="images/photos/user1.png" alt="" /></span>
                                        <span class="desc">
                                          <span class="name">John Doe <span class="badge badge-success">new</span></span>
                                          <span class="msg">Lorem ipsum dolor sit amet...</span>
                                        </span>
                                    </a>
                                </li>
                                <li class="new"><a href="">Read All Mails</a></li>
                            </ul>
                        </div>
                    </li>--%>
            <li>
                <a href="javascript:;" class="btn btn-default dropdown-toggle info-number" data-toggle="dropdown">
                    <i class="fa fa-bell-o"></i>
                    <span class="badge"  id="msgCount" runat="server"></span>
                </a>
                <div class="dropdown-menu dropdown-menu-head pull-right">
                    <h5 class="title">消息通告栏</h5>
                    <ul class="dropdown-list normal-list">
                        <li class="new">
                            <a href="javascript:;" runat="server" id="deliveryOrder">
                                <span class="label label-danger"><i class="fa fa-bolt"></i></span>
                                <span class="name">您有尚未发货的订单 </span>&nbsp---
                                <em class="small" id="orderCount"  runat="server"></em>
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
