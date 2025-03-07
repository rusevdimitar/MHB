<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DetailsProductPriceStatistics.aspx.vb"
    Inherits="MHB.Web.DetailsProductPriceStatistics" %>

<%@ Register Src="../CustomControls/ProductPriceStatistics.ascx" TagName="ProductPriceStatistics" TagPrefix="pps" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin-top: 0px; margin-left: 0px;">
    <form id="form1" runat="server">
        <pps:ProductPriceStatistics ID="ProductPriceStatistics2" runat="server" />
    </form>
</body>
</html>
