<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CategoryIcons.ascx.vb"
    Inherits="MHB.Web.CategoryIcons" %>
<asp:Label ID="LabelSelectIconRepeater" runat="server" Text="Select icon:" CssClass="PlainTextBoldLarge"></asp:Label><br />
<asp:Repeater ID="RepeaterIcons" runat="server">
    <ItemTemplate>
        <asp:ImageButton ID="ImageButtonIcon" runat="server" ImageUrl='<%# Container.DataItem %>'
            CommandName="IconSelected" CommandArgument='<%# Container.DataItem %>' />
    </ItemTemplate>
</asp:Repeater>
<br />
<br />
<asp:Label ID="LabelUserIcons" runat="server" Text="Your icons:" CssClass="PlainTextBoldLarge"></asp:Label><br />
<asp:Repeater ID="RepeaterUserIcons" runat="server">
    <ItemTemplate>
        <table width="400px" class='<%# IIf(Container.ItemIndex Mod 2 = 0, "", "RepeaterAlternatingRowStyleIcons") %>'>
            <tr>
                <td style="width: 30px">
                    <asp:ImageButton ID="ImageButtonUserIcon" runat="server" ImageUrl='<%# CType(Container.DataItem, KeyValuePair(Of String, Integer)).Key %>'
                        CommandName="IconSelected" CommandArgument='<%# CType(Container.DataItem, KeyValuePair(Of String, Integer)).Key %>' />
                </td>
                <td>
                    <asp:Label ID="LabelUserIconUsedByCount" runat="server" Text='<%# CType(Container.DataItem, KeyValuePair(Of String, Integer)).Value %>'
                        Visible='<%# CType(Container.DataItem, KeyValuePair(Of String, Integer)).Value> 0 %>'
                        CssClass="PlainTextSmall"></asp:Label>
                </td>
                <td style="width: 250px">
                    <asp:Label ID="LabelUserIconUsedByCountText" runat="server" Text="other user(s) use this icon."
                        CssClass="PlainTextSmall" Visible='<%# CType(Container.DataItem, KeyValuePair(Of String, Integer)).Value> 0 %>'></asp:Label>&nbsp;
                </td>
                <td align="right">
                    <asp:LinkButton ID="LinkButtonRemoveUserDefinedColumn" runat="server" Text="Delete"
                        CommandName="DeleteIcon" CommandArgument='<%# CType(Container.DataItem, KeyValuePair(Of String, Integer)).Key %>'
                        OnClientClick="javascript:return confirm('Really delete?')"></asp:LinkButton>
                </td>
            </tr>
        </table>
    </ItemTemplate>
</asp:Repeater>