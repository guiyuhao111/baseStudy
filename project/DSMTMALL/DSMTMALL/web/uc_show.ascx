<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_show.ascx.cs" Inherits="DSMTMALL.web.uc_show" %>

<div id="divShowboxBack" data-theme="j" class="showbox-back">
    <div id="divShowboxBox" data-theme="j" class="showbox-box">
        <table class="showbox-table">
            <tr>
                <td class="showbox-td-left">
                    <img alt="imgShowbox" id="imgShowbox" src="javascript:;" /></td>
                <td class="showbox-td-right">
                    <p id="pShowbox"></p>
                </td>
            </tr>
        </table>
        <ul class="showbox-box-ul">
            <li><a id="btnShowboxLeft" href="javascript:;"></a></li>
            <li><a id="btnShowboxRight" href="javascript:;" onclick="divShowboxHide_Click()"></a></li>
        </ul>
    </div>
</div>
<div id="divPromptBar" class="divPromptBar toHide" onclick="HidePromptBar()">
    <h2>温馨提示！</h2>
    <p id="pPromptBar"></p>
</div>
<%--<div id="divNoRecord" class="divNoRecord toHide">
    <img alt="norecord" src="/images/web/icon_norecord.png" />
    <p id="pNoRecord"></p>
</div>--%>
