<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_upload.ascx.cs" Inherits="DSMTMALL.BACKMAG.web.uc.uc_upload" %>
<div class="modal fade bs-example-modal-lg text-center" id="uploadPicture" tabindex="-1" role="dialog" aria-labelledby="showPicture" aria-hidden="true">
    <div class="modal-dialog" style="display: inline-block; width: auto;">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                <h4 class="modal-title">上传图片</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="input-group">
                            <div class="input-group">
                                <input type="text" id="importFileName" class="form-control input-sm pointer" onclick="return $('#importFile').click();" placeholder="请选择图片文件" readonly="readonly" />
                                <input type="file" id="importFile" name="newSoftFile" class="form-control input-sm" style="display: none;" onchange="SelectFile(this)" />
                                <span class="input-group-btn">
                                    <input type="button" id="upLoadPicture" class="form-control input-sm" value="上传图片" onclick="UploadPicture(this)" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="modal fade bs-example-modal-lg text-center" id="uploadExcelModal" tabindex="-1" role="dialog" aria-labelledby="showExcel" aria-hidden="true">
    <div class="modal-dialog" style="display: inline-block; width: auto;">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                <h4 class="modal-title">上传EXCEL</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="input-group">
                            <div class="input-group">
                                <input type="text" id="importFileExcel" class="form-control input-sm pointer" onclick="return $('#importExcel').click();" placeholder="请选择EXCEL文件" readonly="readonly" />
                                <input type="file" id="importExcel" name="newSoftFile" class="form-control input-sm" style="display: none;" onchange="SelectFile(this)" />
                                <span class="input-group-btn">
                                    <input type="button" id="uploadExcel" class="form-control input-sm" value="上传EXCEL" onclick="UploadExcelFile(this)" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
