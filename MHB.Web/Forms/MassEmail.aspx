<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MassEmail.aspx.vb" Inherits="MHB.Web.MassEmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="HTMLEditor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">

        function EnableAddressBar(ctrlID) {

            var checked = document.getElementById(ctrlID).checked;

            if (checked) {
                document.getElementById('trSingleRecepient').style.display = '';
            }
            else {
                document.getElementById('trSingleRecepient').style.display = 'none';
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" />
    <div>
        <table>
            <tr>
                <td class="PlainTextBoldLarge">
                    Subject:
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtSubject" runat="server" Width="400px" CssClass="PlainTextBox"></asp:TextBox>
                    <asp:CheckBox ID="chkSendToSingleRecepeint" runat="server" Text="Send to single recepient"
                        CssClass="PlainText" />
                </td>
            </tr>
            <tr id="trSingleRecepient" style="display: none;">
                <td>
                    <span class="PlainTextBoldLarge">Recepients:</span><br />
                    <asp:TextBox ID="txtEmailAddress" runat="server" TextMode="MultiLine" Rows="3" CssClass="PlainTextBox"
                        Width="500px">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="PlainTextBoldLarge">
                    Message:
                </td>
            </tr>
            <tr>
                <td>
                    <HTMLEditor:Editor ID="Editor1" runat="server" Height="300px" Width="500px" AutoFocus="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSendMassMail" runat="server" Text="Send" CssClass="FancyButton"
                        OnClientClick="return confirm('Really send?');" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCounter" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>