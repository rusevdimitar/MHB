<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SuppliersManagement.ascx.vb" Inherits="MHB.Web.SuppliersManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="MHB.BL" %>
<div class="divDefaultBody">

    <div id="PanelAddNewSupplier" runat="server" style="display: none;">
        <table>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="LabelNewSupplierName" runat="server" Text="Name:" CssClass="PlainTextBold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxNewSupplierName" runat="server" CssClass="PlainTextBox" Width="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <asp:Label ID="LabelNewSupplierDescription" runat="server" Text="Description:" CssClass="PlainTextBold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxNewSupplierDescription" runat="server" CssClass="PlainTextBox" Width="200" TextMode="MultiLine" Rows="3"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="LabelNewSupplierAddress" runat="server" Text="Address:" CssClass="PlainTextBold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxNewSupplierAddress" runat="server" CssClass="PlainTextBox" Width="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="LabelNewSupplierPreffered" runat="server" Text="Preffered:" CssClass="PlainTextBold"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="CheckBoxNewSupplierPreffered" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="LabelNewSupplierActive" runat="server" Text="Active:" CssClass="PlainTextBold"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="CheckBoxNewSupplierActive" runat="server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="LabelNewSupplierWebsite" runat="server" Text="Website:" CssClass="PlainTextBold"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxNewSupplierWebsite" runat="server" CssClass="PlainTextBox" Width="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="ButtonCreateNewSupplier" runat="server" Text="Create" CssClass="ButtonAddSmall" />
                    <asp:Button ID="ButtonCreateNewSupplierCancel" runat="server" Text="Cancel" CssClass="ButtonAddSmall" OnClientClick="javascript:CloseCreateNewSupplierDialog();" />
                </td>
            </tr>
        </table>
    </div>

    <asp:UpdatePanel ID="UpdatePanelSuppliersList" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <div style="padding: 10px;">
                <asp:Button ID="ButtonAddNewSupplier" runat="server" Text="Add new supplier" CssClass="ButtonAddInsert" OnClientClick="javascript:ShowNewSupplierTable('Add new supplier');" />
            </div>
            <asp:GridView ID="GridViewSuppliers" runat="server" AutoGenerateColumns="false"
                Width="100%" BorderWidth="0" GridLines="None">
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="LabelSupplierID" runat="server" Text='<%# CType(Container.DataItem, Supplier).ID%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="LabelSupplierUserID" runat="server" Text='<%# CType(Container.DataItem, Supplier).UserID%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%-- <asp:TemplateField HeaderText="SupplierAccountNumber">
                        <ItemTemplate>
                            <asp:Label ID="LabelSupplierAccountNumber" runat="server" Text='<%# CType(Container.DataItem, Supplier).AccountNumber%>'
                                CssClass="PlainTextBold"></asp:Label>
                        </ItemTemplate>
                       <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditSupplierAccountNumber" runat="server" Text='<%# CType(Container.DataItem, Supplier).AccountNumber%>' MaxLength="15"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>--%>

                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="LabelSupplierName" runat="server" Text='<%# CType(Container.DataItem, Supplier).Name%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditSupplierName" runat="server" Text='<%# CType(Container.DataItem, Supplier).Name%>' Width="90%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="LabelSupplierDescription" runat="server" Text='<%# CType(Container.DataItem, Supplier).Description%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditSupplierDescription" runat="server" Text='<%# CType(Container.DataItem, Supplier).Description%>' Width="90%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="SupplierAddress">
                        <ItemTemplate>
                            <asp:Label ID="LabelSupplierAddress" runat="server" Text='<%# CType(Container.DataItem, Supplier).Address%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditSupplierAddress" runat="server" Text='<%# CType(Container.DataItem, Supplier).Address%>' Width="90%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <%-- <asp:TemplateField HeaderText="SupplierCreditRating">
                    <ItemTemplate>
                        <asp:Label ID="LabelSupplierStandardCost" runat="server" Text='<%# String.Format("{0:f}", CType(Container.DataItem, Supplier).CreditRating)%>'
                            CssClass="PlainText"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxEditSupplierCreditRating" runat="server" Text='<%# String.Format("{0:f}", CType(Container.DataItem, Supplier).CreditRating)%>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>--%>

                    <asp:TemplateField HeaderText="SupplierPreferredVendorStatus">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBoxSupplierPreferredVendorStatus" runat="server" Enabled="false" Checked='<%# CType(Container.DataItem, Supplier).PreferredVendorStatus%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBoxEditSupplierPreferredVendorStatus" runat="server" Checked='<%# CType(Container.DataItem, Supplier).PreferredVendorStatus%>' />
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="SupplierActiveFlag">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBoxSupplierActiveFlag" runat="server" Enabled="false" Checked='<%# CType(Container.DataItem, Supplier).ActiveFlag%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="CheckBoxEditSupplierActiveFlag" runat="server" Checked='<%# CType(Container.DataItem, Supplier).ActiveFlag%>' />
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="SupplierPurchasingWebServiceURL">
                        <ItemTemplate>
                            <asp:Label ID="LabelSupplierPurchasingWebServiceURL" runat="server" Text='<%# CType(Container.DataItem, Supplier).PurchasingWebServiceURL%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditSupplierPurchasingWebServiceURL" runat="server" Text='<%# CType(Container.DataItem, Supplier).PurchasingWebServiceURL%>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="SupplierWebsiteURL">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButtonSupplierWebsiteURL" runat="server" PostBackUrl='<%# CType(Container.DataItem, Supplier).WebSiteURL%>' Text='<%# CType(Container.DataItem, Supplier).WebSiteURL%>'></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditSupplierWebsiteURL" runat="server" Text='<%# CType(Container.DataItem, Supplier).WebSiteURL%>' Width="90%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <%--   <asp:TemplateField HeaderText="DateModified">
                        <ItemTemplate>
                            <asp:Label ID="LabelSupplierDateModified" runat="server" Text='<%# CType(Container.DataItem, Supplier).DateModified%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>--%>

                    <asp:TemplateField>

                        <ItemTemplate>

                            <asp:Panel ID="PanelPopupItemTemplate" runat="server" CssClass="HoverMenuStyle">
                                <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="Edit" CommandName="Edit"
                                    CommandArgument='<%# CType(Container.DataItem, Supplier).ID %>'></asp:LinkButton>
                                <br />
                                <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="Delete" CommandName="Delete"
                                    CommandArgument='<%# CType(Container.DataItem, Supplier).ID %>'></asp:LinkButton>
                            </asp:Panel>

                            <asp:HoverMenuExtender ID="HoverMenuExtenderItemTemplate" runat="Server"
                                TargetControlID="PanelPopupItemTemplate"
                                PopupControlID="PanelPopupItemTemplate"
                                HoverCssClass="HoverMenuSelectedRowStyle"
                                PopupPosition="Right"
                                OffsetX="0"
                                OffsetY="0"
                                PopDelay="50" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Panel ID="PanelPopupEditTemplate" runat="server">
                                <asp:LinkButton ID="LinkButtonUpdate" runat="server" Text="Save" CommandName="Update"
                                    CommandArgument='<%# CType(Container.DataItem, Supplier).ID%>'></asp:LinkButton>
                                <br />
                                <asp:LinkButton ID="LinkButtonCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                    CommandArgument='<%# CType(Container.DataItem, Supplier).ID %>'></asp:LinkButton>
                            </asp:Panel>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="AlternatingRowBudgets" />
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <RowStyle Height="25px" />
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonCreateNewSupplier" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</div>