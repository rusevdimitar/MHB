<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    CodeBehind="Statistics.aspx.vb" Inherits="MHB.Web.Statistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="divDefaultBody">
        <table width="100%" cellpadding="5">
            <tr>
                <td align="center" valign="middle" class="BrightBar">
                    <asp:Label ID="LabelStartYear" runat="server" CssClass="PlainTextBold" Text="Start year:"></asp:Label>&nbsp;
                    <asp:DropDownList ID="DropDownListStartYear" runat="server" AutoPostBack="true" Width="150px">
                    </asp:DropDownList>
                    <asp:Label ID="LabelEndYear" runat="server" CssClass="PlainTextBold" Text="End year:"></asp:Label>&nbsp;
                    <asp:DropDownList ID="DropDownListEndYear" runat="server" AutoPostBack="true" Width="150px">
                    </asp:DropDownList>
                    <asp:Button ID="ButtonPlotChart" runat="server" Text="Plot" CssClass="ButtonAddSmall" />
                    <asp:Button ID="ButtonRotateLeft" runat="server" Text="Rotate left" CssClass="ButtonAddMedium" />
                    <asp:Button ID="ButtonRotateRight" runat="server" Text="Rotate right" CssClass="ButtonAddMedium" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBoxList ID="CheckBoxListCategories" runat="server" CssClass="CheckBoxListVertival"></asp:CheckBoxList>

                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanelStatisticsChart" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:Label ID="LabelGeneralStatisticsInfo" runat="server" Text="Info: Starting year bars are located in the innermost part of the chart"
                                CssClass="PlainText"></asp:Label>
                            <br />
                            <asp:Chart ID="Chart1" runat="server" Width="1280px" Height="960px" ImageStorageMode="UseImageLocation"
                                ImageLocation="../WebCharts/ChartPic_#SEQ(300,3)">
                                <Series>
                                    <%-- <asp:Series ChartType="Bar" Palette="EarthTones" ChartArea="ChartArea1" Name="Default">
                            </asp:Series>
                            <asp:Series ChartType="Bar" Palette="Pastel" ChartArea="ChartArea1" Name="Default1">
                            </asp:Series>
                            <asp:Series ChartType="Bar" Palette="Light" ChartArea="ChartArea1" Name="Default2">
                            </asp:Series>--%>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BackColor="Transparent">
                                        <AxisX Title="Category" IntervalAutoMode="VariableCount" InterlacedColor="Transparent">
                                        </AxisX>
                                        <AxisY Title="Sum">
                                        </AxisY>
                                        <Area3DStyle Perspective="10" Enable3D="True" Inclination="30" IsRightAngleAxes="False"
                                            WallWidth="0" IsClustered="False" />
                                        <%-- Additional markup here --%>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ButtonRotateLeft" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="ButtonRotateRight" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListStartYear" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListEndYear" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
