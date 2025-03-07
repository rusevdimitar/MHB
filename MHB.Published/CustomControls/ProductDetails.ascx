<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProductDetails.ascx.vb" Inherits="MHB.Web.ProductDetails" %>

<%@ Import Namespace="MHB.BL" %>

<table border="0" cellpadding="0" cellspacing="0" style="background-color: #F0FCFF;">
    <tr>
        <td>
            <table width="500px" cellspacing="0">
                <tr style="background-color: #CEECF5;">
                    <td>
                        <asp:Label ID="LabelPDCProductNameText" runat="server" CssClass="PlainTextBoldLarge" Text="Product name:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCProductName" runat="server" CssClass="PlainTextBoldExtraLarge" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPDCProductValueText" runat="server" CssClass="PlainTextBold" Text="Product current value:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCProductValue" runat="server" CssClass="PlainTextBoldLarge" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPDCProductMinValueText" runat="server" CssClass="PlainTextBold" Text="Product min value:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCProductMinValue" runat="server" CssClass="PlainTextBoldLarge" ForeColor="Green" Text=""></asp:Label>
                        <asp:LinkButton ID="LinkButtonPDCProductMinValueDate" runat="server" OnClick="LinkButtonPDCProductMinMaxValueDate_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPDCProductMaxValueText" runat="server" CssClass="PlainTextBold" Text="Product max value:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCProductMaxValue" runat="server" CssClass="PlainTextBoldLarge" ForeColor="Red" Text=""></asp:Label>
                        <asp:LinkButton ID="LinkButtonPDCProductMaxValueDate" runat="server" OnClick="LinkButtonPDCProductMinMaxValueDate_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPDCProductAvgValueText" runat="server" CssClass="PlainTextBold" Text="Product avg. value:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCProductAvgValue" runat="server" CssClass="PlainTextBoldLarge" ForeColor="Goldenrod" Text=""></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="trAverageConsumption">
                    <td>
                        <asp:Label ID="LabelPDCAverageConsumptionText" runat="server" CssClass="PlainTextBoldLarge" Text="Product avg. cons.:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCAverageConsumption" runat="server" CssClass="PlainTextBoldLarge" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPDCTotalQuantityText" runat="server" CssClass="PlainTextBold" Text="Total qty. this month:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCTotalQuantityValue" runat="server" CssClass="PlainTextBoldLarge" ForeColor="CornflowerBlue" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPDCTotalQuantityPerMonthText" runat="server" CssClass="PlainTextBold" Text="Qty. per month:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCTotalQuantityPerMonthValue" runat="server" CssClass="PlainTextBoldLarge" ForeColor="Goldenrod" Text=""></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="trPackageUnitsCountMonthly">
                    <td>
                        <asp:Label ID="LabelPDCTotalPackageUnitsCountText" runat="server" CssClass="PlainTextBold" Text="Total units qty. this month:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCTotalPackageUnitsCountValue" runat="server" CssClass="PlainTextBoldLarge" ForeColor="CornflowerBlue" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPDCTotalSumText" runat="server" CssClass="PlainTextBold" Text="Total sum this month:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCTotalSumValue" runat="server" CssClass="PlainTextBoldLarge" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPDCTotalQuantityYearlyText" runat="server" CssClass="PlainTextBold" Text="Total qty. this year:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCTotalQuantityYearlyValue" runat="server" CssClass="PlainTextBoldLarge" ForeColor="CornflowerBlue" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPDCTotalQuantityPerYearText" runat="server" CssClass="PlainTextBold" Text="Qty. per year:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCTotalQuantityPerYearValue" runat="server" CssClass="PlainTextBoldLarge" ForeColor="Goldenrod" Text=""></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="trPackageUnitsCountYearly">
                    <td>
                        <asp:Label ID="LabelPDCTotalPackageUnitsCountYearlyText" runat="server" CssClass="PlainTextBold" Text="Total unit qty. this year:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCTotalPackageUnitsCountYearlyValue" runat="server" CssClass="PlainTextBoldLarge" ForeColor="CornflowerBlue" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPDCTotalSumYearlyText" runat="server" CssClass="PlainTextBold" Text="Total sum this year:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCTotalSumYearlyValue" runat="server" CssClass="PlainTextBoldLarge" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPDCAllTimeTotalQtyText" runat="server" CssClass="PlainTextBold" Text="All time total qty.:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCAllTimeTotalQtyValue" runat="server" CssClass="PlainTextBoldLarge" ForeColor="CornflowerBlue" Text=""></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="trPackageUnitsCountAllTime">
                    <td>
                        <asp:Label ID="LabelPDCAllTimeTotalPackageUnitsCountText" runat="server" CssClass="PlainTextBold" Text="All time total unit qty.:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCAllTimeTotalPackageUnitsCountValue" runat="server" CssClass="PlainTextBoldLarge" ForeColor="CornflowerBlue" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelPDCAllTimeTotalSumText" runat="server" CssClass="PlainTextBold" Text="All time total sum:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LabelPDCAllTimeTotalSumValue" runat="server" CssClass="PlainTextBoldLarge" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
        <td valign="top">
            <asp:GridView ID="GridViewProductSupplierInfo" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
                ShowFooter="true" BorderWidth="0" CssClass="GridDetails" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="PlainTextBold" Width="700px">
                <Columns>

                    <asp:TemplateField HeaderText="Supplier name">
                        <ItemTemplate>
                            <div
                                <%# IIf(CType(Container.DataItem, ProductInfo).HasLowestPrice, "style='background-color:palegreen;font-weight:bold;'", String.Empty)%>
                                <%#IIf(CType(Container.DataItem, ProductInfo).HasHighestPrice, "style='background-color:#FA5858;font-weight:bold;'", String.Empty)%>>
                                <asp:Label ID="LabelProductInfoSupplierName" runat="server" CssClass="PlainText" Text='<%# CType(Container.DataItem, ProductInfo).SupplierName%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataFormatString="{0:0.00}" DataField="Amount" HeaderText="Total count" ItemStyle-CssClass="PlainText" ItemStyle-Width="100" />
                    <asp:BoundField DataFormatString="{0:0.00}" DataField="Total" HeaderText="Total sum" ItemStyle-CssClass="PlainText" ItemStyle-Width="100" />
                    <asp:BoundField DataFormatString="{0:0.00}" DataField="Average" HeaderText="Average sum" ItemStyle-CssClass="PlainText" ItemStyle-Width="100" />

                    <asp:TemplateField HeaderText="Max sum">
                        <ItemTemplate>
                            <div
                                <%#IIf(CType(Container.DataItem, ProductInfo).HasHighestPrice, "style='background-color:#FA5858;font-weight:bold;'", String.Empty)%>>
                                <asp:Label ID="LabelProductInfoSupplierMaxPrice" runat="server" CssClass="PlainText" Text='<%# CType(Container.DataItem, ProductInfo).Max.ToString("0.00")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Min sum">
                        <ItemTemplate>
                            <div
                                <%# IIf(CType(Container.DataItem, ProductInfo).HasLowestPrice, "style='background-color:palegreen;font-weight:bold;'", String.Empty)%>>
                                <asp:Label ID="LabelProductInfoSupplierMinPrice" runat="server" CssClass="PlainText" Text='<%# CType(Container.DataItem, ProductInfo).Min.ToString("0.00")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                    </asp:TemplateField>
                </Columns>
                <RowStyle HorizontalAlign="Center" Height="20px" />
                <HeaderStyle HorizontalAlign="Center" />
            </asp:GridView>
        </td>
    </tr>
</table>