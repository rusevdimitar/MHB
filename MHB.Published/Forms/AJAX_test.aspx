﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AJAX_test.aspx.vb" Inherits="MHB.Web.AJAX_test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Javascript/AXAX_functions.js" type="text/javascript"></script>
    <script src="../Javascript/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <script src="../Javascript/jquery-1.4.1.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="text" id="inputText" value="" />
        <input type="button" id="btnSearch" value="Search" />
        <div id="statusDiv" style="height: 150px; width: 350px; background-color: Aqua; font-family: Verdana;
            font-size: x-large;">
        </div>
    </div>
    </form>
</body>
</html>