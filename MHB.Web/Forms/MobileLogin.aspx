<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MobileLogin.aspx.vb" Inherits="MHB.Web.MobileLogin" %>

<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.2//EN" "http://www.openmobilealliance.org/tech/DTD/xhtml-mobile12.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/MobileStyles.css" rel="stylesheet" type="text/css" />
    <meta name="HandheldFriendly" content="true" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="main">
        <img src="../Images/logo_tiny.gif" alt="MyHomeBills.mobi" />
        <br />
        <br />
        <table cellpadding="3" cellspacing="0" width="100%" style="background-color: #dfefff;">
            <tr>
                <td class="PlainTextBold">
                    Username:
                </td>
                <td>
                    <asp:TextBox ID="TextBoxUserName" runat="server" CssClass="PlainTextBox"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="PlainTextBold">
                    Password:
                </td>
                <td>
                    <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password" CssClass="PlainTextBox"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="ButtonLogin" runat="server" Text="Login" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Label ID="LabelError" runat="server" CssClass="ErrorText"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>