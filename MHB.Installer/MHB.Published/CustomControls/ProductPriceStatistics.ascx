<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProductPriceStatistics.ascx.vb" Inherits="MHB.Web.ProductPriceStatistics" %>

<%@ Register Src="../CustomControls/ProductDetails.ascx" TagName="ProductDetails" TagPrefix="uc1" %>

<asp:Chart ID="ChartProductPriceStatistics" runat="server" BackImageAlignment="Top" BackSecondaryColor="White"
    BorderColor="#1A3B69" BorderDashStyle="Solid" BorderWidth="2px" Height="320px"
    ImageLocation="../WebCharts/ChartPic_#SEQ(300,3)" Palette="Excel" Width="1200px">
    <Titles>
        <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" ForeColor="26, 59, 105" Name="Title1"
            ShadowColor="32, 0, 0, 0" ShadowOffset="3">
        </asp:Title>
    </Titles>
    <%-- <Legends>
        <asp:Legend Alignment="Center" BackColor="Transparent" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold"
            IsTextAutoFit="False" LegendStyle="Column" Name="Default">
        </asp:Legend>
    </Legends>--%>
    <Series>
        <asp:Series BorderColor="180, 26, 59, 105" ChartType="Line" Color="220, 65, 140, 240"
            Name="Default">
        </asp:Series>
    </Series>
    <ChartAreas>
        <asp:ChartArea BackColor="Transparent" BackSecondaryColor="Transparent" BorderColor="64, 64, 64, 64"
            BorderWidth="0" Name="ChartArea1" ShadowColor="Transparent">
            <Area3DStyle Rotation="0" />
            <AxisY LineColor="64, 64, 64, 64">
                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                <MajorGrid LineColor="64, 64, 64, 64" />
            </AxisY>
            <AxisX LineColor="64, 64, 64, 64">
                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                <MajorGrid LineColor="64, 64, 64, 64" />
            </AxisX>
        </asp:ChartArea>
    </ChartAreas>
</asp:Chart>
<uc1:ProductDetails ID="ProductDetailsControl" runat="server" />
