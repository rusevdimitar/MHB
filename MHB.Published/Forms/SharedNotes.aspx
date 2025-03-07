<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="SharedNotes.aspx.vb" Inherits="MHB.Web.SharedNotes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CustomControls/Notes.ascx" TagName="Note" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="divContentBody" style="height:300px;">
        <div id="PanelNotes" style="display: none;">
            <uc1:Note ID="Note1" runat="server" />
        </div>
    </div>
</asp:Content>
