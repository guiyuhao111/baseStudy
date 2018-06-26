<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_address_editor.aspx.cs" Inherits="DSMTMALL.web.user_address_editor" %>
<%@ Register Src="~/web/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc_show.ascx" TagPrefix="uc1" TagName="uc_show" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>51IPC购物平台</title>
    <uc1:uc_title runat="server" ID="uc_title" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="divAddressEditorPage" data-role="page" style="font-family: 'Microsoft YaHei'" >
            <script src="/js/html/user_address_editor.js"></script>
            <div data-role="header" data-theme="j" >
                <div class="header-back-g">
                    <a data-rel="back">
                        <img src="../../images/web/hpage/rightarr.png" />
                    </a>
                    <span>编辑收货地址</span>
                </div>
                <uc1:uc_show runat="server" ID="uc_show" />
            </div>
            <div data-role="content" >
                <span class="delete_address" id="btnDeleteAddress">删除</span>
                <div class="address-separate">
                    <h2>收货信息<span></span></h2>
                </div>
                <div class="row-item">
                    <dl>
                        <dt>&nbsp;收&nbsp;货&nbsp;地&nbsp;区:</dt>
                        <dd>
                            <div data-role="controlgroup" data-type="horizontal" id="addressManage" >
                                <select id="ddlUserProvince" data-mini="true" runat="server" onchange="ProvinceChange($('#addressManage'),$(this).val(),'')">
                                </select>
                                <select id="ddlUserCity" data-mini="true" onchange="CityChange($('#addressManage'),$('#ddlUserProvince').val(),$(this).val(),'')">
                                    <option value="">城市</option>
                                </select>
                                <select id="ddlUserDistrict" data-mini="true">
                                    <option value="">地区</option>
                                </select>
                            </div>
                        </dd>

                    </dl>
                </div>
                <div class="row-item">
                    <dl>
                        <dt>&nbsp;详&nbsp;细&nbsp;地&nbsp;址:</dt>
                        <dd>
                            <input type="text" id="tbUserAddress" placeholder="地址在25字以内(必填)" maxlength="50" /></dd>
                    </dl>
                </div>
                <div class="row-item">
                    <dl>
                        <dt>&nbsp;收&nbsp;货&nbsp;姓&nbsp;名:</dt>
                        <dd>
                            <input type="text" id="tbUserNickname" placeholder="收货人姓名(必填)" maxlength="10" /></dd>
                    </dl>
                </div>
                <div class="row-item">
                    <dl>
                        <dt>&nbsp;联&nbsp;系&nbsp;方&nbsp;式:</dt>
                        <dd>
                            <input type="tel" id="tbUserPhone" placeholder="请填写手机号码(必填)" maxlength="11" /></dd>
                    </dl>
                </div>
                <div class="row-item">
                    <dl>
                        <dt>&nbsp;身&nbsp;份&nbsp;证&nbsp;号:</dt>
                        <dd>
                            <input type="text" id="tbUserCardNo" placeholder="必须与收货人一致(必填)" maxlength="18" /></dd>
                    </dl>
                </div>
                <%--<input type="button" id="btnDeleteAddress" value="删除" data-theme="c" />--%>
            </div>

            <div data-role="footer" id="btnFooter" data-position="fixed"   data-tap-toggle="false">
                <div style="margin-right: auto; margin-left: auto"></div>
                <div style="margin-right: auto; margin-left: auto">
                    <a href="#" id="btnAddAddress" data-role="button" style="width: 70%; margin-right: auto; margin-left: auto; display: block; background-color: #2b90f1">
                        <span style="line-height: 25px; color: #ffffff; text-shadow: none; font-weight: 100;">保存地址</span>
                    </a>
                </div>
                <div style="margin-right: auto; margin-left: auto"></div>
            </div>
        </div>
    </form>
</body>
</html>
