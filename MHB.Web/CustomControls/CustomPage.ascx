<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CustomPage.ascx.vb"
    Inherits="MHB.Web.CustomPage" %>
<asp:UpdatePanel ID="UpdatePanelCustomFields" runat="server" ChildrenAsTriggers="true"
    UpdateMode="Always">
    <ContentTemplate>
        <table>
            <tr>
                <td colspan="2">
                    <asp:PlaceHolder ID="MainContent" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="42px" />
                    <asp:Button ID="btnAddNewCustomField" runat="server" Text="Add" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" />
                    <asp:Button ID="btnLoadControls" runat="server" Text="Load Controls" />
                </td>
                <td>
                    <asp:Label ID="lblSumCalcFields" runat="server" CssClass="PlainTextBoldLarge"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblNewFieldType" runat="server" Text="Type of the field:" CssClass="PlainTextBold"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpFieldType" Width="250px" AutoPostBack="true" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNewFieldName" runat="server" Text="Name of the field:" CssClass="PlainTextBold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNewFieldName" Width="250px" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCalculable" runat="server" Text="Calculable?" CssClass="PlainTextBold"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkCalculable" runat="server" Text="" Enabled="false" CssClass="PlainTextBold" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnAddNewField" runat="server" Width="250px" Text="Add" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnlEditListItems" runat="server" Visible="false">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCustomListFields" runat="server" Text="Custom list fields:" CssClass="PlainTextBold"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drpListControls" runat="server" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblListItemName" runat="server" Text="Item Name:" CssClass="PlainTextBold"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtListItemName" runat="server"></asp:TextBox>
                                                    <asp:Button ID="btnAddNewListItem" runat="server" Text="Add item" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="GridViewListItems" runat="server" AutoGenerateColumns="false">
                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <EditItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="List item name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblListItemName" runat="server" Text='<%# Eval("ListItemText") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtListItemName" runat="server" Text='<%# Eval("ListItemText") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                                        CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                                                        CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"></asp:LinkButton>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
    </Triggers>
</asp:UpdatePanel>