<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeBehind="Reports.aspx.vb" Inherits="MHB.Web.Reports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <table style="width: 100%;">
            <tr>
                <td align="left">
                    <asp:Label ID="LabelReportMonth" runat="server" CssClass="PlainTextBold"></asp:Label>
                    <asp:DropDownList ID="DropDownListReportMonth" runat="server" CssClass="PlainTextBold"
                        AutoPostBack="true" OnSelectedIndexChanged="DropDownListReportMonth_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="LabelReportYear" runat="server" CssClass="PlainTextBold"></asp:Label>
                    <asp:DropDownList ID="DropDownListReportYear" runat="server" CssClass="PlainTextBold">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                        Height="500px" Width="100%">
                        <LocalReport ReportPath="Report1.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="Test01DbDataSet_tbMainTable01" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ReportsConnectionString %>"
            SelectCommand="">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="1" Name="UserID" QueryStringField="userid"
                    Type="Int32" />
                <asp:QueryStringParameter DefaultValue="1" Name="Month" QueryStringField="month"
                    Type="Int32" />
                <asp:QueryStringParameter DefaultValue="2009" Name="Year" QueryStringField="year"
                    Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <div style="color: #FFFFFF; font-size: 1px;">
        лични финанси, управление лични финанси, финанси, лични, програма лични финанси,
        домашни разходи, домашен, разход, разходи, домашни, домакинство, планиране домакинство,
        разходи домакинство, месечен бюджет, планиране бюджет, програма бюджет, програма
        семеен бюджет, програма месечен бюджет, нещо семеен бюджет, планиране пари, планиране
        пари месец, заплата, планиране заплата, разпределяне заплата mese4en biudjet, mesechen
        budget, budget, programa mese4en biudjet, planirane bjudjet, semeen biudget, planirane
        zaplata, zaplata, planirane, bjudjet, biudget, budget, monthly budget, family budget,
        family bills, myhomebills, my home bills, home expenses, household, expenditure,
        cost
    </div>
</asp:Content>