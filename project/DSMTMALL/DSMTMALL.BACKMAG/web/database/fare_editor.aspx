<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fare_editor.aspx.cs" Inherits="DSMTMALL.BACKMAG.web.database.fare_editor" %>

<%@ Register Src="~/web/uc/uc_title.ascx" TagPrefix="uc1" TagName="uc_title" %>
<%@ Register Src="~/web/uc/uc_headers.ascx" TagPrefix="uc1" TagName="uc_headers" %>
<%@ Register Src="~/web/uc/uc_menus.ascx" TagPrefix="uc1" TagName="uc_menus" %>
<%@ Register Src="~/web/uc/uc_footer.ascx" TagPrefix="uc1" TagName="uc_footer" %>



<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>后台管理</title>
    <uc1:uc_title runat="server" ID="uc_title" />
    <script src="../../js/web/faredelieryList.js"></script>
</head>
<body>
    <section>
        <uc1:uc_menus runat="server" ID="uc_menus" />
        <div class="main-content">
            <uc1:uc_headers runat="server" ID="uc_headers" />
            <div class="page-heading">
            </div>
            <div class="wrapper">
                <div style="margin: 0 20% 0 20%">
                    <h4 style="text-align: center; margin-bottom: 30px;" runat="server" id="fare_id">新增运费模板</h4>
                    <form class="form-horizontal" role="form">
                        <div class="form-group">
                            <label for="fareName" class="col-sm-2 control-label">模板名称</label>
                            <div class="col-sm-10">
                                <input type="text" class="form-control" id="addfareName" placeholder="请输入模板名称" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="fareAddress" class="col-sm-2 control-label">发货地址</label>
                            <div class="col-sm-10">
                                <input type="text" class="form-control" id="addfareAddress" placeholder="请输入发货地址" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="fareTime" class="col-sm-2 control-label">发货时间</label>
                            <div class="col-sm-10">
                                <input type="text" class="form-control" id="addfareTime" placeholder="请输入多少小时内发货" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="" class="col-sm-2 control-label">计价方式</label>
                            <div class="col-sm-10">
                                <label class="radio-inline">
                                    <input type="radio" name="inlineRadioOptions" value="20" checked="checked" />
                                    按重量
                                </label>
                                <label class="radio-inline">
                                    <input type="radio" name="inlineRadioOptions" value="10" />
                                    按件数
                                </label>
                                <label class="radio-inline">
                                    <input type="radio" name="inlineRadioOptions" value="30" />
                                    按体积
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="" class="col-sm-2 control-label">运送方式</label>
                            <div class="col-sm-10">
                                <table class="table table-striped table-condensed table-hover table-bordered fare-table editor-table" id="editorFareCarry">
                                    <caption>除指定地区外，其余地区的运费采用"默认运费"</caption>
                                    <tr>
                                        <td colspan="6" class="editor-default-td">默认运费：
                                            <input class="form-control" name="default-count" type="text" />
                                            KG（件）内，
                                            <input class="form-control" name="default-money" type="text" />
                                            元，每增加
                                            <input class="form-control" name="default-addCount" type="text" />
                                            KG（件），增加运费
                                            <input class="form-control" name="default-addFee" type="text" />
                                            元</td>
                                    </tr>
                                    <tr>
                                        <th>运送到</th>
                                        <th>首（件/重）</th>
                                        <th>首费</th>
                                        <th>续（件/重）</th>
                                        <th>续费</th>
                                        <th>操作</th>
                                    </tr>
                                    <tr name="addFareList">
                                        <td><span>未添加地区</span><a data-toggle="modal" data-target="#chooseAddressModel" id="btnAssign" onclick="ChooseAddress(this)">编辑</a></td>
                                        <td>
                                            <input class="form-control" type="text" name="count" /></td>
                                        <td>
                                            <input class="form-control" type="text" name="money" /></td>
                                        <td>
                                            <input class="form-control" type="text" name="addCount" /></td>
                                        <td>
                                            <input class="form-control" type="text" name="addFee" /></td>
                                        <td><a onclick="DeleteFareAddress(this)">删除</a></td>
                                    </tr>
                                </table>
                                <div><a style="text-decoration: none" onclick="AddNewFareAddress(this)">为指定地区设置运费</a></div>


                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-2 col-sm-10">
                                <button type="button" class="btn btn-default" onclick="SaveAddress(this)">保存</button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
            <uc1:uc_footer runat="server" ID="uc_footer" />
        </div>
        <div class="modal fade" id="chooseAddressModel" tabindex="-1" role="dialog" aria-labelledby="chooseAddressModel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title">选择地区</h4>
                    </div>
                    <div class="modal-body">
                        <div class="row" id="chooseAddressInfo">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <input type="button" class="btn btn-primary btn-sm" onclick="ConfirmAddress(obj)" value="确认" data-dismiss="modal" />
                        <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
                    </div>
                </div>
            </div>
        </div>
    </section>
</body>

</html>
