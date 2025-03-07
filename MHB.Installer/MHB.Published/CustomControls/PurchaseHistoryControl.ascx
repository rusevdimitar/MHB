<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PurchaseHistoryControl.ascx.vb" Inherits="MHB.Web.PurchaseHistoryControl" %>
<asp:GridView ID="GridViewPurchaseHistoryList" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
    ShowFooter="true" BorderWidth="0" CssClass="GridDetails" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="PlainTextBold">
    <Columns>
        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="PlainText" />

        <asp:TemplateField HeaderText="Amount">
            <ItemTemplate>
                <asp:Label ID="LabelAmount" runat="server" Text='<%# Container.DataItem.Value %>' CssClass="PlainText"></asp:Label>
            </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="LabelTotalAmount" runat="server" CssClass="PlainTextBoldLarge"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>

        <asp:BoundField DataField="Supplier" HeaderText="Supplier" ItemStyle-CssClass="PlainText" />
        <asp:BoundField DataField="Date" HeaderText="Date" ItemStyle-CssClass="PlainText" />
    </Columns>
</asp:GridView>