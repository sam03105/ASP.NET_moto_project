<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Select1.aspx.cs" Inherits="project_motorcycle.Select1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
     <script src="Scripts/jquery-3.5.1.js"></script>
    <script src="Scripts/jquery.jqGrid.js"></script>
    <script src="Scripts/i18n/grid.locale-tw.js"></script>
    <script src="Scripts/jquery-ui-1.12.1.js"></script>
    <link href="Content/jquery.jqGrid/ui.jqgrid.css" rel="stylesheet" />
    <link href="Content/themes/base/jquery-ui.css" rel="stylesheet" />
    <form id="form1" runat="server">
         <script type="text/javascript">
             $(document).ready(function () {
                 GetStockInfo();
             });//網頁準備好時直接跑這些程式

             function GetStockInfo() {
                 var stock = getUrlParameter('stock');
                 $.ajax({
                     type: 'post',
                     url: 'DataHandler.ashx',
                     data: {
                         action: 'GetStockInfo',
                         stock: stock,
                     },
                     success: function (data) {
                         var obj = jQuery.parseJSON(data);
                         var print = "煞車系統:" + obj[0].Brake_Type +'\n'+ "價錢:" + obj[0].Price +'\n'+ "廠牌:" + obj[0].Vendor_Name;
                         //alert(print);
                         var url = "images/" + obj[0].Moto_Id + ".jpg";
                         document.getElementById("pic").src = url;
                     },
                     error: function () { alert('資料取得失敗'); }
                 })
             }/*抓取網址傳入的值*/

             function getUrlParameter(sParam) {
                 var sPageURL = window.location.search.substring(1),
                     sURLVariables = sPageURL.split('&'),
                     sParameterName,
                     i;

                 for (i = 0; i < sURLVariables.length; i++) {
                     sParameterName = sURLVariables[i].split('=');

                     if (sParameterName[0] == sParam) {
                         return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
                     }
                 }
             }/*分析抓取到的值*/

             function turn() {
                 document.location.href = "http://192.168.50.187/default.aspx";
             }

             

</script>
        <div>
            <img id="pic" onclick="turn();" />
            <button type="button" id='change' onclick='turn();'>按鈕</button>
        </div><%--建立按鈕和圖片的位子--%>
    </form>
</body>
</html>
