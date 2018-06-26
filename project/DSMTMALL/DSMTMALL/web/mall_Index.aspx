<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mall_Index.aspx.cs" Inherits="DSMTMALL.web.mall_Index" %>

<%@ Register Src="~/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>51IPC购物平台</title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body>
    <div id="divPageIndex" data-role="page" style="border: none;">
        <script src="/js/html/mallIndex.js"></script>
        <div data-role="header" style="background-color: #F2F2F2;" >
            <div class="copyright" >
               <img src="../css/images/company.png"/>
            </div>
            <div class="input-search" data-theme="z">
                <form id="myform">
                    <span>
                        <input id="filter-listview" type="search" data-type="search" placeholder="关键词检索..." />
                    </span>
                </form>
            </div>
        </div>
        <%--Header-End--%>
        <div data-role="content" style="padding: 0px;" data-theme="j" class="my-content-color">
            <%--轮播图--%>
            <div class="carousel-pic" style="margin: 0 auto; display: none">
                <div class="flexslider">
                    <ul id="carousel-slider" class="slides">
                        <asp:Repeater ID="repeaterNoticeList" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="<%# Eval("TargetUrl") %>" data-transition="slide">
                                        <img alt="" src="<%# Eval("NPicture") %>" style="width: 100%" onerror="nofindRoll()" />
                                    </a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
            <%--轮播图结束--%>
            <div id="goodsTopListShow" style="display: none">
                <div id="divCpyGoodsList" runat="server" hidden="hidden">
                    <div class="mallIndex-nav">
                        <span style="background-image: url(../images/web/hpage/cpychoose1.png); background-repeat: no-repeat; background-size: contain; width: 100%" id="cpyNameSpan" runat="server">店长推荐</span>
                    </div>
                    <div id="cpyTopGoodsList" class="ui-grid-a  myTopGoodsList" style="margin-left: auto; margin-right: auto" runat="server"></div>

                </div>
                <div id="foodsGoodsList">
                    <div class="mallIndex-nav">
                        <span style="background-image: url(../images/web/hpage/cpychoose.png); background-repeat: no-repeat; background-size: contain;">爱尚生活</span>
                    </div>
                    <div id="gridTopGoodsList" class="ui-grid-a  myTopGoodsList" style="margin-left: auto; margin-right: auto"></div>
                    <div id="divNoRecord" class="divNoRecord toHide">
                        <img alt="norecord" src="/images/web/icon_norecord.png" />
                        <p id="pNoRecord"></p>
                    </div>
                </div>
            </div>
            <div style="width: 100%"><span style="display: inline-block; color: #333; width: 100%; font-size: 12px; text-align: center; height: 50px; line-height: 50px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">Copyright ©2017 宁波粮仓在线电子商务有限公司 版权所有</span></div>
        </div>

        <uc1:uc_footer runat="server" ID="uc_footer" />
    </div>

</body>
</html>
