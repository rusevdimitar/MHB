<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeBehind="Charts.aspx.vb" Inherits="MHB.Web.Charts" %>

<%@ Register Assembly="WebChart" Namespace="WebChart" TagPrefix="Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div align="center">
        <table style="width: 100%; background-color: #FFFFFF;">
            <tr>
                <td align="center">
                    <Web:ChartControl ID="ChartControl1" runat="server" BorderStyle="Solid" BorderWidth="1px"
                        Height="500px" Width="1200px" BorderColor="Gainsboro" Visible="False" HasChartLegend="False">
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
            <tr>
                <td align="left">
                    <asp:Label ID="LabelSum" runat="server" Text="" CssClass="PlainTextBoldLarge" ForeColor="Navy"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="DivError" runat="server" align="center" class="ErrorDiv">
                    </div>
                </td>
            </tr>
            <tr>
                <td style="border-width: 1px; border-color: #0066FF; font-family: Arial, Helvetica, sans-serif;
                    font-size: 13px; font-weight: bold; background-color: #D5E9FD; border-top-style: solid;">
                    <asp:Label ID="LabelExpenses" runat="server" Text="Expenses:"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="GridViewFirefoxBorders">
                        <asp:GridView ID="GridViewAnnualReport" runat="server" AutoGenerateColumns="False"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Jan">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumJan" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumJanuary") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Feb">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumFeb" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumFebruary") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mar">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumMar" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumMarch") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Apr">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumApr" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumApril") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="May">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumMay" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumMay") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="June">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumJune" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumJune") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="July">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumJuly" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumJuly") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Aug">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumAug" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumAugust") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sept">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumSept" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumSeptember") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Oct">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumOct" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumOctober") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nov">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumNov" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumNovember") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dec">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumDec" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumDecember") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="ButtonAnnuExpChart" runat="server" CssClass="PlainButton" Text="Plot chart" />
                </td>
            </tr>
            <tr>
                <td style="border-width: 1px; border-color: #0066FF; font-family: Arial, Helvetica, sans-serif;
                    font-size: 13px; font-weight: bold; background-color: #D5E9FD; border-top-style: solid;">
                    <asp:Label ID="LabelBudget" runat="server" Text="Budget:"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="GridViewFirefoxBorders" align="center">
                        <asp:GridView ID="GridViewAnnualReport0" runat="server" AutoGenerateColumns="False"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Jan">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumJan" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumJanuary") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Feb">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumFeb" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumFebruary") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mar">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumMar" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumMarch") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Apr">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumApr" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumApril") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="May">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumMay" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumMay") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="June">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumJune" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumJune") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="July">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumJuly" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumJuly") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Aug">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumAug" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumAugust") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sept">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumSept" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumSeptember") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Oct">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumOct" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumOctober") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nov">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumNov" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumNovember") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dec">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumDec" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumDecember") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="ButtonAnnuBudgetChart" runat="server" CssClass="PlainButton" Text="Plot chart" />
                </td>
            </tr>
            <tr>
                <td style="border-width: 1px; border-color: #0066FF; font-family: Arial, Helvetica, sans-serif;
                    font-size: 13px; font-weight: bold; background-color: #D5E9FD; border-top-style: solid;">
                    <asp:Label ID="LabelSavings" runat="server" Text="Savings:"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="GridViewFirefoxBorders" align="center">
                        <asp:GridView ID="GridViewAnnualReport1" runat="server" AutoGenerateColumns="False"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Jan">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumJan" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumJanuary") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Feb">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumFeb" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumFebruary") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mar">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumMar" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumMarch") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Apr">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumApr" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumApril") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="May">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumMay" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumMay") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="June">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumJune" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumJune") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="July">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumJuly" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumJuly") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Aug">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumAug" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumAugust") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sept">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumSept" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumSeptember") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Oct">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumOct" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumOctober") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nov">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumNov" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumNovember") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dec">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBoxSumDec" CssClass="GridTextBoxesSmall" runat="server" Text='<%# Bind("SumDecember") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="ButtonAnnuSavingsChart" runat="server" CssClass="PlainButton" Text="Plot chart" />
                </td>
            </tr>
        </table>
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
    </div>
</asp:Content>