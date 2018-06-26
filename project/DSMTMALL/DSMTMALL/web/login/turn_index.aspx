<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="turn_index.aspx.cs" Inherits="DSMTMALL.web.login.turn_index" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>51IPC购物平台</title>
    <script src="../../js/jquery.js"></script>
    <style>
        canvas {
            display: block;
            position: absolute;
            top: 52%;
            width: 100%;
            height: 100px;
        }
        span{
            font-size: 45px; font-family: 'Microsoft YaHei'; position: absolute; top: 60%; left: 60%;
        }
    </style>
</head>
<body style="max-height: 90%;">
    <iframe id="showIframe" hidden="hidden"></iframe>
    <div id="showBgImg" runat="server" style="margin: -8px">
        <img src="../../css/images/welcome.jpg" style="width: 100%;" />
        <script src="../../js/handler/index.js"></script>
    </div>
    <script>
        window.onload = function () {
            var url = "/web/ajax.aspx";
            var args = { "jsonType": "trun_index", "createTime": new Date() };
            var times = 0;
            var iframeOnload = 0;
            setInterval(function () {
                if (times >= 2 && iframeOnload == 1) {
                    iframeOnload = 0;
                    $.post(url, args, function (data) {
                        if (data == "success") {
                            window.location.href = '/web/mall_Index.aspx?';
                        } else if (data == "nofinduser") {
                            window.location.href = "/web/login/binding.aspx?";
                        } else {
                            window.location.href = '/web/user_login.aspx?';
                        }
                    });
                }
                times++;
            }, 1000);
            var iframe = document.getElementById("showIframe");
            iframe.src = "/web/index.aspx";
            if (iframe.attachEvent) {
                iframe.attachEvent("onload", function () {
                    iframeOnload = 1;
                });
            } else {
                iframe.onload = function () {
                    iframeOnload = 1;
                };
            }
        };
    </script>
</body>
</html>
