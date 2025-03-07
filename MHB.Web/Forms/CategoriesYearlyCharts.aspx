<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeBehind="CategoriesYearlyCharts.aspx.vb" Inherits="MHB.Web.CategoriesYearlyCharts" %>

<%@ Register Assembly="WebChart" Namespace="WebChart" TagPrefix="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div align="center" style="background-color: White;">
        <div id="DivError" runat="server" align="center" class="ErrorDiv">
        </div>
        <table cellspacing="0" cellpadding="10" style="background-color: WhiteSmoke;" width="100%">
            <tr>
                <td align="left" valign="middle">
                    <asp:Label ID="LabelPickCategory" runat="server" CssClass="PlainTextBold" Text="Pick category:"></asp:Label>&nbsp;
                    <asp:DropDownList ID="DropDownListCategories" runat="server" AutoPostBack="true"
                        Width="150px">
                    </asp:DropDownList>
                    <asp:Label ID="LabelStartYear" runat="server" CssClass="PlainTextBold" Text="Start year:"></asp:Label>&nbsp;
                    <asp:DropDownList ID="DropDownListStartYear" runat="server" AutoPostBack="true" Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <Web:ChartControl ID="ChartControl1" runat="server" BorderStyle="Solid" BorderWidth="1px"
                        Height="500px" Width="1280px" BorderColor="Gainsboro" HasChartLegend="False"
                        Visible="false">
                        <YAxisFont StringFormat="Far,Near,Character,LineLimit" />
                        <XTitle StringFormat="Near,Near,None,NoFontFallback" />
                        <XAxisFont StringFormat="Center,Center,Character,LineLimit" />
                        <Background Color="WhiteSmoke" ForeColor="DarkGray" HatchStyle="WideDownwardDiagonal" />
                        <ChartTitle StringFormat="Center,Near,Character,LineLimit" Font="Verdana, 10pt, style=Bold"
                            ForeColor="8, 73, 140" />
                        <Charts>
                            <Web:ColumnChart ShowLineMarkers="False">
                                <DataLabels>
                                    <Border Color="Transparent" />
                                    <Background Color="Transparent" />
                                </DataLabels>
                            </Web:ColumnChart>
                        </Charts>
                        <YTitle StringFormat="Center,Near,Character,LineLimit" />
                        <Border Color="DarkGray" />
                        <PlotBackground CenterColor="GhostWhite" />
                    </Web:ChartControl>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>