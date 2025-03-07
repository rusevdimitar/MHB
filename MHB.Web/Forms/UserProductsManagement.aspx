<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="UserProductsManagement.aspx.vb" Inherits="MHB.Web.UserProductsManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CustomControls/ProductsManagement.ascx" TagName="ProductsManagement" TagPrefix="pm" %>
<%@ Register Src="../CustomControls/SuppliersManagement.ascx" TagName="SuppliersManagement" TagPrefix="sm" %>
<%@ Register Src="../CustomControls/CategoriesManagement.ascx" TagName="CategoryManager" TagPrefix="cm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

    <asp:TabContainer runat="server" CssClass="ajax__tab_yuitabview-theme">
        <asp:TabPanel ID="TabPanelProducts" runat="server" HeaderText="Products" BackColor="White">
            <ContentTemplate>
                <pm:ProductsManagement ID="ProductsManagement1" runat="server"></pm:ProductsManagement>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanelSuppliers" runat="server" HeaderText="Suppliers" BackColor="White">
            <ContentTemplate>
                <sm:SuppliersManagement ID="SuppliersManagement1" runat="server"></sm:SuppliersManagement>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel ID="TabPanelCategories" runat="server" HeaderText="Categories" BackColor="White">
            <ContentTemplate>
                <cm:CategoryManager ID="CategoryManager1" runat="server"></cm:CategoryManager>
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
