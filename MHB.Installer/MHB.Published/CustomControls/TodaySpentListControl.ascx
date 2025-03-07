<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TodaySpentListControl.ascx.vb" Inherits="MHB.Web.TodaySpentListControl" %>

<asp:GridView ID="GridViewTodaySpentListControl" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
    ShowFooter="true" BorderWidth="0" CssClass="GridDetails" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="PlainTextBold">
    <Columns>
        <asp:BoundField DataField="Item2" HeaderText="Name" ItemStyle-CssClass="PlainText" />
        <asp:BoundField DataFormatString="{0:0.00}" DataField="Item3" HeaderText="Amount" ItemStyle-CssClass="PlainText" />
    </Columns>
</asp:GridView>