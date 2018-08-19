<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_confirm.ascx.cs" Inherits="DSMTMALL.BACKMAG.web.uc.uc_confirm" %>

<div class="modal fade" id="confirmModal" tabindex="-1" role="dialog" aria-labelledby="confirmModal" aria-hidden="true">
    <div class="modal-dialog modal-sm">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                <h4 class="modal-title">提示</h4>
            </div>
            <div class="modal-body">
                <strong id="sgConfirm"></strong>
            </div>
            <div class="modal-footer">
                <input type="button" id="aConfirm"  class ="btn btn-primary btn-sm" />
                <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">取消</button>
            </div>
        </div>
    </div>
</div>

