<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PurchaseHistoryDateSelectorControl.ascx.vb" Inherits="MHB.Web.PurchaseHistoryDateSelectorControl" %>
<asp:Repeater ID="RepeaterPurchaseDates" runat="server">
    <ItemTemplate>
        <asp:LinkButton ID="LinkButtonPurchaseDate" runat="server" Text='<%# String.Format("{0} (<strong>{1:0.0#}</strong>)", CDate(Eval("key")).ToShortDateString, Eval("value")) %>' CssClass="LinkButtonPlainLarge" CommandName="DateSelected" CommandArgument='<%# Eval("key")%>' OnClientClick="javascript:ShowPurchaseHistoryList();"></asp:LinkButton>
        <hr />
    </ItemTemplate>
</asp:Repeater>

<asp:Label ID="LabelTotalSum" runat="server" CssClass="PlainTextBoldLarge"></asp:Label>