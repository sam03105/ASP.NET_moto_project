<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="project_motorcycle._default" %>

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

            $("#datetimepickerS").datepicker({
                altField: "#datepicker",
                altFormat: "yy-mm-dd",
                dateFormat: "yy-mm-dd"
            });
            $("#datetimepickerE").datepicker({
                altField: "#datepicker",
                altFormat: "yy-mm-dd",
                dateFormat: "yy-mm-dd"
            });
            
            LoadVendorList();
            LoadMoto_NameList();
            LoadCC_Type();
            MotoGrid_MotoGrid();
            Load_CC_Type_MotoGrid();
            $("#CbVendor").hide();
            $("#CbMoto_Name").hide();
            $("#CbCC").hide();
            $("#datetimepickerS").hide();
            $("#datetimepickerE").hide();
            LoadSelect();
           
            //timelimit_Vendor_confidence_MotoGrid();
            //Load_timelimit_Vendor_confidence_MotoGrid();
        });//網頁準備好時直接跑這些程式

        //function GetStockInfo() {
        //    var stock = getUrlParameter('stock');
        //    $.ajax({
        //        type: 'post',
        //        url: 'DataHandler1.ashx',
        //        data: {
        //            action: 'GetStockInfo',
        //            stock: stock,
        //        },
        //        success: function (data) {
        //            var obj = jQuery.parseJSON(data);
        //            alert(obj[0].Result);
        //        },
        //        error: function () { alert('資料取得失敗'); }
        //    })
        //}

        //function getUrlParameter(sParam) {
        //    var sPageURL = window.location.search.substring(1),
        //        sURLVariables = sPageURL.split('&'),
        //        sParameterName,
        //        i;

        //    for (i = 0; i < sURLVariables.length; i++) {
        //        sParameterName = sURLVariables[i].split('=');

        //        if (sParameterName[0] == sParam) {
        //            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        //        }
        //    }
        //}

        function LoadVendorList() {
            $.ajax({
                type: 'post',
                url: 'DataHandler.ashx',
                async: false,
                data: {
                    action: 'GetVendorList',
                },
                success: function (data) {
                    var obj = jQuery.parseJSON(data);
                    $('#CbVendor').append(new Option("全廠商", ""));
                    $.each(obj, function (i, el) {
                        $('#CbVendor').append(new Option(el.Vendor_Name, el.Vendor_Id));
                    }
                    )
                },
                error: function () { alert('資料取得失敗1'); }
            })
        }//設定廠商下拉選單

        function LoadMoto_NameList() {
            $.ajax({
                type: 'post',
                url: 'DataHandler.ashx',
                async: false,
                data: {
                    action: 'GetMoto_NameList',
                    Vendor_Id: $('#CbVendor').find('option:selected').val()
                },
                success: function (data) {
                    $('#CbMoto_Name').empty();
                    var obj = jQuery.parseJSON(data);
                    $('#CbMoto_Name').append(new Option("全車型", ""));
                    $.each(obj, function (i, el) {
                        $('#CbMoto_Name').append(new Option(el.Moto_Name, el.Moto_Id));
                    });
                },
                error: function () { alert('資料取得失敗2'); }
            })
        }//設定車型下拉選單

        function LoadCC_Type() {
            $.ajax({
                type: 'post',
                url: 'DataHandler.ashx',
                async: false,
                data: {
                    action: 'GetCC_TypeList',
                    Vendor_Id: $('#CbVendor').find('option:selected').val(),
                    Moto_Id: $('#CbMoto_Name').find('option:selected').val(),
                },
                success: function (data) {
                    $('#CbCC').empty();
                    var obj = jQuery.parseJSON(data);
                    $('#CbCC').append(new Option("全CC", ""));
                    $.each(obj, function (i, el) {
                        $('#CbCC').append(new Option(el.CC_Type, el.CC_Type));
                    });
                },
                error: function () { alert('資料取得失敗2'); }
            })
        }//偵測廠商和車型選單的選取值,並設定CC數下拉選單
        
        function MotoGrid_MotoGrid() {
            $("#MotoGrid1").jqGrid(
                {
                    url: 'DataHandler.ashx',
                    data: {
                        action: 'GetMotoGrid_MotoGridData',
                        Vendor_Id: $('#CbVendor').find('option:selected').val(),
                        Moto_Name: $('#CbMoto_Name').find('option:selected').val(),
                        CC_Type: $('#CbCC').find('option:selected').val(),
                        Time1: $("#datetimepickerS").val(),
                        Time2: $("#datetimepickerE").val()
                    },
                    datatype: "json",
                    jsonReader: {
                        repeatitems: false
                    },
                    colNames: ['車ID', '剎車型態', '車型', '價格', '廠商ID', '廠商名稱', '銷量', '發行日期', '引擎形式', '前懸吊', '後懸吊', '最高馬力', '最大扭力', '傳動方式', '供油方式', 'USB', '長', '寬', '高', '油耗', '油箱容量', '座高', '重量', '軸距','CC數','照片'],
                    colModel: [
                        { name: 'Moto_Id', index: 'Moto_Id', width: 60, stype: 'text' },
                        { name: 'Brake_Type', index: 'Brake_Type', width: 70, stype: 'text' },
                        { name: 'Moto_Name', index: 'Moto_Name', width: 60, stype: 'text' },
                        { name: 'Price', index: 'Price', width: 50, stype: 'text' },
                        { name: 'Vendor_Id', index: 'Vendor_Id', width: 50, stype: 'text' },
                        { name: 'Vendor_Name', index: 'Vendor_Name', width: 70, stype: 'text' },
                        { name: 'Sales_Volume', index: 'Sales_Volume', width: 60, stype: 'text' },
                        { name: 'Sales_Date', index: 'Sales_Date', width: 70, stype: 'text' },
                        { name: 'Engine_Form', index: 'Engine_Form', width: 70, stype: 'text' },
                        { name: 'Front_Suspension', index: 'Front_Suspension', width: 60, stype: 'text' },
                        { name: 'Back_Suspension', index: 'Back_Suspension', width: 60, stype: 'text' },
                        { name: 'Max_Power', index: 'Max_Power', width: 60, stype: 'text' },
                        { name: 'Max_Torque', index: 'Max_Torque', width: 60, stype: 'text' },
                        { name: 'Transfer_Method', index: 'Transfer_Method', width: 65, stype: 'text' },
                        { name: 'Supply_Oil', index: 'Supply_Oil', width: 60, stype: 'text' },
                        { name: 'Usb', index: 'Usb', width: 45, stype: 'text' },
                        { name: 'Size_L', index: 'Size_L', width: 50, stype: 'text' },
                        { name: 'Size_W', index: 'Size_W', width: 50, stype: 'text' },
                        { name: 'Size_H', index: 'Size_H', width: 50, stype: 'text' },
                        { name: 'Oil_Consumption', index: 'Oil_Consumption', width: 65, stype: 'text' },
                        { name: 'Oil_Capacity', index: 'Oil_Capacity', width: 65, stype: 'text' },
                        { name: 'Sit_Height', index: 'Sit_Height', width: 60, stype: 'text' },
                        { name: 'Weight', index: 'Weight', width: 60, stype: 'text' },
                        { name: 'Wheel_Base', index: 'Wheel_Base', width: 70, stype: 'text' },
                        { name: 'CC_Type', index: 'CC_Type', width: 60, stype: 'text' },
                        {
                            name: 'picture', index: 'picture', width: 75, stype: 'text',
                            formatter: function (cellvalue, options, rowObject) {
                                var obMember = rowObject["Moto_Id"];
                                return "<input type='button' value='刪除' onclick=DelMember('" + obMember + "')\>";
                            }
                        }
                    ],
                    rowNum: 25,
                    height: 500,
                    loadonce: true,
                    width: 2000,
                    pager: '#MotoGridPager1',
                    sortname: 'FunctionNo',
                    viewrecords: true,
                    sortorder: '',
                    caption: "機車細項列表",
                    onSelectRow: function (id) {
                    }
                }).navGrid('#MotoGridPager1', { edit: false, add: false, del: false, search: false });
        }//設定全資訊表格

        function DelMember(data) {
            /*$.ajax({
                type: 'post',
                url: 'DataHandler1.ashx',
                async: false,
                data: {
                    action: 'DelMember',
                    member_id: data
                },
                success: function (data) {
                    var Jdata = jQuery.parseJSON(data);
                    if (Jdata.result == 'true') {
                        alert('刪除成功');
                    }
                    else {
                        alert('刪除失敗');
                    }
                    LoadMemberGrid();
                },
                error: function () { alert('資料取得失敗1'); }
            })*/
            var url =""+data+".jpg";
            document.write("<img src=images/" + url + ">");
        }//接取ID來收尋圖片資訊

        function Load_CC_Type_MotoGrid() {
            $("#MotoGrid1").jqGrid('setGridParam',
                {
                    url: 'DataHandler.ashx',
                    postData: {
                        action: 'GetMotoGrid_MotoGridData',
                        Vendor_Id: $('#CbVendor').find('option:selected').val(),
                        Moto_Id: $('#CbMoto_Name').find('option:selected').val(),
                        CC_Type: $('#CbCC').find('option:selected').val(),
                        Time1: $("#datetimepickerS").val(),
                        Time2: $("#datetimepickerE").val()
                    },
                    datatype: 'json',
                    page: 1
                }).trigger('reloadGrid');
        }//全機車資訊表格建立

        function LoadSelect() {
            $('#CbType').empty();
            $('#CbType').append(new Option("請選擇", "0"));
            $('#CbType').append(new Option("全資訊", "1"));
            $('#CbType').append(new Option("最高銷量", "2"));
            $('#CbType').append(new Option("分類", "3"));
            $('#CbType').append(new Option("轉址", "5"));
        }//設定模式下拉式選單

        function Select() {
            //alert($('#CbType').find('option:selected').val());
            if ($('#CbType').find('option:selected').val() == 3) {
                $("#divMoto2").hide();
                $("#divMoto1").show();
                $("#CbVendor").show();
                $("#CbMoto_Name").show();
                $("#CbCC").show();
                $("#datetimepickerS").hide();
                $("#datetimepickerE").hide();
                LoadVendorList();
                LoadMoto_NameList();
                LoadCC_Type();
                LoadSelect();

                MotoGrid_MotoGrid();
                Load_CC_Type_MotoGrid();
            }
            else if ($('#CbType').find('option:selected').val() == 2) {
                $("#divMoto1").hide();
                $("#divMoto2").show();
                $("#CbVendor").hide();
                $("#CbMoto_Name").hide();
                $("#CbCC").hide();
                $("#datetimepickerS").show();
                $("#datetimepickerE").show();

                //LoadVendorList();
                //LoadMoto_NameList();
                //LoadCC_Type();
                //LoadSelect();
                timelimit_Vendor_confidence_MotoGrid();
                Load_timelimit_Vendor_confidence_MotoGrid();
            }
            else if ($('#CbType').find('option:selected').val() == 1) {
                $("#divMoto2").hide();
                $("#divMoto1").show();
                $("#CbVendor").hide();
                $("#CbMoto_Name").hide();
                $("#CbCC").hide();
                $("#datetimepickerS").hide();
                $("#datetimepickerE").hide();
                LoadVendorList();
                LoadMoto_NameList();
                LoadCC_Type();
                LoadSelect();

                MotoGrid_MotoGrid();
                Load_CC_Type_MotoGrid();
                
            }
            else if ($('#CbType').find('option:selected').val() == 5) {
                document.location.href = "http://localhost/Select1.aspx";
            }
            else if ($('#CbType').find('option:selected').val() == 0) {
                $("#divMoto1").hide();
                $("#divMoto2").hide();
                $("#CbVendor").hide();
                $("#CbMoto_Name").hide();
                $("#CbCC").hide();
                $("#datetimepickerS").hide();
                $("#datetimepickerE").hide();
                LoadVendorList();
                LoadMoto_NameList();
                LoadCC_Type();
                LoadSelect();

            }
        }//看選取哪個模式來執行不同的模式

        function timelimit_Vendor_confidence_MotoGrid() {
            $("#MotoGrid2").jqGrid(
                {
                    data: {
                        action: 'GetVendor_confidence_GridData',
                        Time1: $("#datetimepickerS").val(),
                        Time2: $("#datetimepickerE").val()
                    },
                    jsonReader: {
                        root: "rows",
                        page: "page",
                        total: "total",
                        records: "record",
                        repeatitems: false
                    },
                    datatpe: "jsonstring",
                    colNames: ['廠商', '車型', '車Id', '最高銷量'],
                    colModel: [
                        { name: 'Vendor_Name', index: 'Vendor_Name', width: 60, stype: 'text' },
                        { name: 'Moto_Name', index: 'Moto_Name', width: 60, stype: 'text' },
                        { name: 'Moto_Id', index: 'Moto_Id', width: 75, stype: 'text' },
                        { name: 'sales_volume', index: 'sales_volume', width: 75, stype: 'text' }
                    ],
                    rowNum: 5, height: 350, width: 950, scroll: true, pager: '#MotoGridPager2', viewrecords: true,
                    caption: " 人員列表",
                    onSelectRow: function (id) {
                    },
                }).navGrid('#MotoGridPager2', { edit: false, add: false, del: false, search: false });
        }//最高銷量

        function Load_timelimit_Vendor_confidence_MotoGrid() {
            $("#MotoGrid2").jqGrid('setGridParam',
                {
                    url: 'DataHandler.ashx',
                    postData: {
                        action: 'GetVendor_confidence_GridData',
                        Time1: $("#datetimepickerS").val(),
                        Time2: $("#datetimepickerE").val()
                    },
                    datatype: 'json',
                    page: 1
                }).trigger('reloadGrid');
        }//最高銷量的偵測時間

    </script>
        <div>
            <%--廠商:--%>
        <select id ="CbVendor" onchange="LoadMoto_NameList();Load_CC_Type_MotoGrid();LoadCC_Type();">
            </select>
            <%--車型:--%>
    <select id ="CbMoto_Name" onchange="LoadCC_Type();Load_CC_Type_MotoGrid();LoadCC_Type();">
        </select>
        <input id="datetimepickerS" type="text" onchange="Load_timelimit_Vendor_confidence_MotoGrid();"/>
        <input id="datetimepickerE" type="text" onchange="Load_timelimit_Vendor_confidence_MotoGrid();"/>
            <%--CC數選擇:--%>
            <select id ="CbCC" onchange="Load_CC_Type_MotoGrid();">
            </select>
            模式:
            <select id ="CbType" onchange="Select();">
            </select>
            <img id="pic /">
        </div><%--新增3個下拉式選單還有觸發事件--%>
    </form>
     
    <div id="divMoto1">
        <table id="MotoGrid1" style="text-align:center"></table>
        <div id="MotoGridPager1"></div>
    </div>
    <div id="divMoto2">
        <table id="MotoGrid2" style="text-align:center"></table>
        <div id="MotoGridPager2"></div>
    </div>
</body>
</html>
