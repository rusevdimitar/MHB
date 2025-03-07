<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    CodeBehind="ExchangeRatesCharts.aspx.vb" Inherits="MHB.Web.ExchangeRatesCharts" %>

<%@ Register Src="../CustomControls/ExchangeRates.ascx" TagName="ExchangeRates" TagPrefix="er" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="LoginPageMainTable">
        <er:ExchangeRates ID="ExchangeRates" runat="server"></er:ExchangeRates>
    </div>
</asp:Content>