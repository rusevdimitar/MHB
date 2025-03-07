<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Transactions.aspx.vb"
    Inherits="MHB.Web.Transactions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="PlainTextLarge">
        <asp:Repeater ID="RepeaterTransactions" runat="server">
            <ItemTemplate>
                <asp:Label ID="LabelTransaction" runat="server" Text='<%# Eval("TransactionText") %>'></asp:Label>&nbsp;
                <asp:Label ID="LabelDIfference" runat="server" Font-Bold="true"></asp:Label>
                <br />
                <hr />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>