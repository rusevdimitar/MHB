<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CostsCompare.aspx.vb"
    Inherits="MHB.Web.CostsCompare" %>

<%@ Register Assembly="WebChart" Namespace="WebChart" TagPrefix="Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Compare</title>
    <link href="../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin-top: 0px; margin-left: 0px;">
    <form id="form1" runat="server">
    <div>
        <Web:ChartControl ID="ChartControl1" runat="server" BorderStyle="Solid" BorderWidth="1px"
            Height="350px" Width="1000px" BorderColor="Gainsboro" HasChartLegend="False">
            <YAxisFont StringFormat="Far,Near,Character,LineLimit" />
            <XTitle StringFormat="Near,Near,None,NoFontFallback" />
            <XAxisFont StringFormat="Center,Center,Character,LineLimit" />
            <Background Color="WhiteSmoke" ForeColor="DarkGray" HatchStyle="WideDownwardDiagonal" />
            <ChartTitle StringFormat="Center,Near,Character,LineLimit" Font="Verdana, 10pt, style=Bold"
                ForeColor="8, 73, 140" />
            <Charts>
                <Web:ColumnChart ShowLineMarkers="False">
                    <DataLabels Position="Bottom">
                        <Border Color="Transparent" />
                        <Background Color="Transparent" />
                    </DataLabels>
                </Web:ColumnChart>
            </Charts>
            <YTitle StringFormat="Center,Near,Character,LineLimit" />
            <Border Color="DarkGray" />
            <PlotBackground CenterColor="GhostWhite" />
        </Web:ChartControl>
    </div>
    </form>
</body>
</html>