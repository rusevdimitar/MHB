<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    CodeBehind="APIManagement.aspx.vb" Inherits="MHB.Web.APIManagement" %>

<%@ Register Src="../CustomControls/ApiManagementControl.ascx" TagName="ApiManagementControl"
    TagPrefix="api" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="LoginPageMainTable">
        <api:ApiManagementControl ID="ApiManagementControl1" runat="server">
        </api:ApiManagementControl>
    </div>
</asp:Content>