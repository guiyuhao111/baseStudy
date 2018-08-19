<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_menus.ascx.cs" Inherits="DSMTMALL.BACKMAG.web.uc.uc_menus" %>

<div class="left-side sticky-left-side" >
    <!--logo and iconic logo start logo图标-->
    <div class="logo">
        <a href="javascript:;">
            <img src="../../images/logo.png" alt=""></a>
    </div>
    <div class="logo-icon text-center">
        <a href="javascript:;">
            <img src="../../images/logo_icon.png" alt=""></a>
    </div>

    <div class="left-side-inner">
        <!--visible to small devices only--> 
            <div class="visible-xs hidden-sm hidden-md hidden-lg">
                <div class="media logged-user">
                    <img alt="" src="../../images/photos/user-avatar.png" class="media-object">
                    <div class="media-body">
                        <h4><a href="#">John Doe</a></h4>
                        <span>"Hello There..."</span>
                    </div>
                </div>

                <h5 class="left-nav-title">Account Information</h5>
                <ul class="nav nav-pills nav-stacked custom-nav">
                  <li><a href="#"><i class="fa fa-user"></i> <span>Profile</span></a></li>
                  <li><a href="#"><i class="fa fa-cog"></i> <span>Settings</span></a></li>
                  <li><a href="#"><i class="fa fa-sign-out"></i> <span>Sign Out</span></a></li>
                </ul>
            </div>
        <!--左边导航栏开始-->
        <ul class="nav nav-pills nav-stacked custom-nav" runat="server" id="ulMenu">
        </ul>

    </div>
</div>
