<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ApiManagementControl.ascx.vb"
    Inherits="MHB.Web.ApiManagementControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<table>
    <tr>
        <td>
            <asp:Label ID="LabelApiKeyText" runat="server" CssClass="PlainTextBoldLarge" Text="API Key:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="LabelApiKey" runat="server" CssClass="PlainTextErrorExtraLarge"></asp:Label>
        </td>
        <td>
            <asp:Button ID="ButtonGenerateApiKeyShowPopup" runat="server" CssClass="ButtonAddMedium"
                Text="Generate API Key" OnClientClick="javascript:return false;" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="LabelApiServiceUrlText" runat="server" Text="API Service Url:" CssClass="PlainTextBold"></asp:Label>
        </td>
        <td colspan="2">
            <asp:Label ID="LabelApiServiceUrl" runat="server" Text="" CssClass="PlainText"></asp:Label>
        </td>
    </tr>
</table>
<div style="display: none;">
    <div id="generateApiKey" style="width: 230px; height: 80px; background: GhostWhite;">
        <table>
            <tr>
                <td>
                    <asp:Label ID="LabelUserName" runat="server" Text="Username:" CssClass="PlainTextBold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxUserName" runat="server" CssClass="PlainTextBox"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelPassword" runat="server" Text="Password:" CssClass="PlainTextBold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password" CssClass="PlainTextBox"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="ButtonLogin" runat="server" Text="Ok" CssClass="ButtonAddTiny" OnClientClick="CloseFancyBoxAndSubmit(this); return false;" />                    
                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" CssClass="ButtonAddTiny" />
                </td>
            </tr>
        </table>
    </div>
</div>