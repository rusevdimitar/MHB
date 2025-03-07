<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeBehind="UserCategoriesManagement.aspx.vb" Inherits="MHB.Web.UserCategoriesManagement"
    EnableViewStateMac="false" %>

<%@ Register Src="../CustomControls/CategoriesManagement.ascx" TagName="CategoryManager"
    TagPrefix="cat" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <cat:CategoryManager ID="CategoryManager1" runat="server"></cat:CategoryManager>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head">
</asp:Content>