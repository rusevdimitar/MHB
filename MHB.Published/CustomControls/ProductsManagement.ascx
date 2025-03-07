<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProductsManagement.ascx.vb" Inherits="MHB.Web.ProductsManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="MHB.BL" %>

<script type="text/javascript">
    function colorChanged(sender) {
        sender.get_element().style.color =
             "#" + sender.get_selectedColor();
    }

    function ProductAutoCompleteItemSelected(source, eventArgs) {
        __doPostBack('<%= ButtonFilter.ClientID%>', eventArgs.get_value());
    }
</script>
<div id="divProductsManagementToolbar" class="divDefaultBody">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div id="DivLoading" class="LoadingDiv">
                <div style="border: solid 1px #C1DAFF">
                    <table style="width: 100%;">
                        <tr>
                            <td width="33px">
                                <img src="../Images/loading_dollar.gif" alt="Loading..." />
                            </td>
                            <td align="center">Loading...
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanelFunctionalControls" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelFunctionalControls" runat="server" DefaultButton="ButtonFilter" Style="padding: 10px;">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="ButtonPrintShoppingList" runat="server" CssClass="FancyButton" Text="Print shopping list" Visible="false" />
                        </td>
                        <td>
                            <asp:Label ID="LabelFilterByNameText" runat="server" CssClass="PlainTextBold" Text="Search by name:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxFilterByName" runat="server" CssClass="PlainTextBox" Width="200px"></asp:TextBox>
                            <asp:AutoCompleteExtender
                                runat="server"
                                BehaviorID="AutoCompleteEx"
                                ID="AutoCompleteProducts"
                                TargetControlID="TextBoxFilterByName"
                                ServicePath="~/Forms/ProductAutoComplete.asmx"
                                ServiceMethod="GetProducts"
                                MinimumPrefixLength="2"
                                CompletionInterval="10"
                                EnableCaching="true"
                                CompletionSetCount="20"
                                DelimiterCharacters=";, :"
                                OnClientItemSelected="ProductAutoCompleteItemSelected"
                                ShowOnlyCurrentWordInCompletionListItem="true"
                                CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListItemCssClass="autocomplete_listItem"
                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                <Animations>
                            <OnShow>
                                <Sequence>
                                    <%-- Make the completion list transparent and then show it --%>
                                    <OpacityAction Opacity="0" />
                                    <HideAction Visible="true" />

                                    <%--Cache the original size of the completion list the first time
                                        the animation is played and then set it to zero --%>
                                    <ScriptAction Script="
                                        // Cache the size and setup the initial size
                                        var behavior = $find('AutoCompleteExDetails');
                                        if (!behavior._height) {
                                            var target = behavior.get_completionList();
                                            behavior._height = target.offsetHeight - 2;
                                            target.style.height = '0px';
                                        }" />

                                    <%-- Expand from 0px to the appropriate size while fading in --%>
                                    <Parallel Duration=".4">
                                        <FadeIn />
                                        <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx')._height" />
                                    </Parallel>
                                </Sequence>
                            </OnShow>
                            <OnHide>
                                <%-- Collapse down to 0px and fade out --%>
                                <Parallel Duration=".4">
                                    <FadeOut />
                                    <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx')._height" EndValue="0" />
                                </Parallel>
                            </OnHide>
                                </Animations>
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label ID="LabelFilterByCategoryText" runat="server" CssClass="PlainTextBold" Text="Filter by category:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListFilterByCategory" runat="server" Width="170px" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="LabelFilterBySupplierText" runat="server" CssClass="PlainTextBold" Text="Filter by supplier:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListFilterBySupplier" runat="server" Width="170px" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="LabelPageSizeText" runat="server" CssClass="PlainTextBold" Text="Page size:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListPageSize" runat="server" Width="50px" AutoPostBack="true">
                                <asp:ListItem Text="20" Value="20" Selected="True" />
                                <asp:ListItem Text="50" Value="50" />
                                <asp:ListItem Text="100" Value="100" />
                                <asp:ListItem Text="150" Value="150" />
                                <asp:ListItem Text="200" Value="200" />
                                <asp:ListItem Text="All" Value="-1" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="ButtonFilter" runat="server" CssClass="ButtonAddMedium" Text="Filter" OnClick="ButtonFilter_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelProductsList" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <asp:GridView ID="GridViewProducts" runat="server" AutoGenerateColumns="false"
                Width="100%" BorderWidth="0" ShowFooter="true" GridLines="None" AllowPaging="true" PageSize="20" OnPageIndexChanging="GridViewProducts_PageIndexChanging">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBoxSelectedProductID" runat="server" ProductID='<%# CType(Container.DataItem, Product).ID%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href='<%# String.Format("#{0}", CType(Container.DataItem, Product).ID)%>'></a>
                            <asp:Label ID="LabelProductID" runat="server" Text='<%# CType(Container.DataItem, Product).ID%>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Image ID="ImageItemIsInShoppingList" runat="server" ImageUrl="~/Images/tick_circle.png" Visible='<%# Me._en.ShoppingList.Any(Function(sl) CType(sl.Item2, Product).ID = CType(Container.DataItem, Product).ID)%>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="LabelProductName" runat="server" Text='<%# CType(Container.DataItem, Product).Name%>'
                                CssClass="PlainTextBold"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditProductName" runat="server" Text='<%# CType(Container.DataItem, Product).Name%>' Width="90%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="LabelProductDescription" runat="server" Text='<%# CType(Container.DataItem, Product).Description%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditProductDescription" runat="server" Text='<%# CType(Container.DataItem, Product).Description%>' Width="90%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Keywords">
                        <ItemTemplate>
                            <asp:Label ID="LabelProductKeywords" runat="server" Text='<%# String.Join(",", CType(Container.DataItem, Product).KeyWords)%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditProductKeywords" runat="server" Text='<%# String.Join(",", CType(Container.DataItem, Product).KeyWords)%>' Width="90%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="StandardCost">
                        <ItemTemplate>
                            <asp:Label ID="LabelProductStandardCost" runat="server" Text='<%# String.Format("{0:f}", CType(Container.DataItem, Product).StandardCost)%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditProductStandardCost" runat="server" Text='<%# String.Format("{0:f}", CType(Container.DataItem, Product).StandardCost)%>' Width="60%"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorEditStandardCost" runat="server" ControlToValidate="TextBoxEditProductStandardCost" CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="ListPrice">
                        <ItemTemplate>
                            <asp:Label ID="LabelProductListPrice" runat="server" Text='<%# String.Format("{0:f}", CType(Container.DataItem, Product).ListPrice)%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditProductListPrice" runat="server" Text='<%# String.Format("{0:f}", CType(Container.DataItem, Product).ListPrice)%>' Width="60%"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorEditProductListPrice" runat="server" ControlToValidate="TextBoxEditProductListPrice" CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Color">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxProductColor" runat="server" Text='<%# CType(Container.DataItem, Product).Color%>' BackColor='<%# System.Drawing.ColorTranslator.FromHtml(String.Format("#{0}", CType(Container.DataItem, Product).Color))%>'
                                CssClass="PlainTextBox" ReadOnly="true" Width="44px"></asp:TextBox>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditProductColor" runat="server" Text='<%# CType(Container.DataItem, Product).Color%>' MaxLength="6" Width="60%"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorEditProductColor" runat="server" ControlToValidate="TextBoxEditProductColor" CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$"></asp:RegularExpressionValidator>
                            <asp:ColorPickerExtender runat="server"
                                ID="ColorPickerExtender1"
                                TargetControlID="TextBoxEditProductColor"
                                OnClientColorSelectionChanged="colorChanged" />
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Weight">
                        <ItemTemplate>
                            <asp:Label ID="LabelProductWeight" runat="server" Text='<%# CType(Container.DataItem, Product).Weight%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditProductWeight" runat="server" Text='<%# CType(Container.DataItem, Product).Weight%>'
                                Width="60%"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorEditProductWeight" runat="server" ControlToValidate="TextBoxEditProductWeight" CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Volume">
                        <ItemTemplate>
                            <asp:Label ID="LabelProductVolume" runat="server" Text='<%# CType(Container.DataItem, Product).Volume%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditProductVolume" runat="server" Text='<%# CType(Container.DataItem, Product).Volume%>'
                                Width="60%"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorEditProductVolume" runat="server" ControlToValidate="TextBoxEditProductVolume" CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="PackageUnitsCount">
                        <ItemTemplate>
                            <asp:Label ID="LabelProductPackageUnitsCount" runat="server" Text='<%# CType(Container.DataItem, Product).PackageUnitsCount%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBoxEditProductPackageUnitsCount" runat="server" Text='<%# CType(Container.DataItem, Product).PackageUnitsCount%>'
                                Width="60%"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorEditProductPackageUnitsCount" runat="server" ControlToValidate="TextBoxEditProductPackageUnitsCount" CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*[1-9][0-9]*[\s]*$"></asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Icon">
                        <ItemTemplate>
                            <img alt="Category Icon" src='<%# CType(Container.DataItem, Product).Category.IconPath%>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="CategoryName">
                        <ItemTemplate>
                            <asp:Label ID="LabelProductCategoryName" runat="server" Text='<%# CType(Container.DataItem, Product).Category.Name%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="LabelEditProductCategoryID" runat="server" Text='<%# CType(Container.DataItem, Product).Category.ID%>' Visible="false"></asp:Label>
                            <asp:DropDownList ID="DropDownListProductCategories" runat="server" Width="100px"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Vendor">
                        <ItemTemplate>
                            <asp:Label ID="LabelProductSupplierName" runat="server" Text='<%# CType(Container.DataItem, Product).Supplier.Name%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="LabelEditProductVendorID" runat="server" Text='<%# CType(Container.DataItem, Product).Supplier.ID%>' Visible="false"></asp:Label>
                            <asp:DropDownList ID="DropDownListProductVendors" runat="server" Width="100px"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="DateModified">
                        <ItemTemplate>
                            <asp:Label ID="LabelProductDateModified" runat="server" Text='<%# CType(Container.DataItem, Product).DateModified.ToString("dd-MM-yyyy hh:mm")%>'
                                CssClass="PlainText"></asp:Label>
                        </ItemTemplate>

                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Panel ID="PanelPopupItemTemplate" runat="server" CssClass="HoverMenuStyle">
                                <%--Text='<%# Me._en.GetTranslatedValue("LinkButtonShowShoppingListDialog", Me._en.CurrentLanguage)%>'--%>
                                <asp:LinkButton ID="LinkButtonShowShoppingListDialog" runat="server" Text="Add to shop.list"
                                    CommandArgument='<%# CType(Container.DataItem, Product).ID %>' OnClientClick='<%# String.Format("DisplayShoppingListDialog(""{0}"");", CType(Container.DataItem, Product).Name)%>' OnClick="LinkButtonShowShoppingListDialog_Click" Visible='<%# Me._en.ShoppingList.Any(Function(sl) CType(sl.Item2, Product).ID = CType(Container.DataItem, Product).ID) = False%>'></asp:LinkButton>

                                <asp:LinkButton ID="LinkButtonRemoveFromShoppingList" runat="server" Text="Remove from shop.list"
                                    CommandArgument='<%# CType(Container.DataItem, Product).ID %>' CommandName="RemoveFromShoppingList" OnClick="LinkButtonShowShoppingListDialog_Click" Visible='<%# Me._en.ShoppingList.Any(Function(sl) CType(sl.Item2, Product).ID = CType(Container.DataItem, Product).ID)%>'></asp:LinkButton>
                                <br />
                                <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="Edit" CommandName="Edit"
                                    CommandArgument='<%# CType(Container.DataItem, Product).ID %>'></asp:LinkButton>
                                <br />
                                <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="Delete" CommandName="Delete"
                                    CommandArgument='<%# CType(Container.DataItem, Product).ID %>'></asp:LinkButton>
                                <br />
                                <asp:LinkButton ID="LinkButtonMerge" runat="server" Text="Merge" CommandName="Merge"
                                    CommandArgument='<%# CType(Container.DataItem, Product).ID %>'></asp:LinkButton>
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
                                    CommandArgument='<%# CType(Container.DataItem, Product).ID%>'></asp:LinkButton>
                                <br />
                                <asp:LinkButton ID="LinkButtonCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                    CommandArgument='<%# CType(Container.DataItem, Product).ID %>'></asp:LinkButton>
                            </asp:Panel>
                        </EditItemTemplate>

                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButtonDetailProductStatistics" runat="server"
                                ToolTip='<%# Me._en.GetTranslatedValue("DetailProductStatistics", Me._en.CurrentLanguage) %>'
                                AlternateText='<%# Me._en.GetTranslatedValue("DetailProductStatistics", Me._en.CurrentLanguage)%>'
                                ImageUrl="../Images/product_prices_chart_faded.gif"
                                onmouseover="this.src='../Images/product_prices_chart.gif'"
                                onmouseout="this.src='../Images/product_prices_chart_faded.gif'"
                                OnClick='<%#
 MHB.Web.Environment.GetOpenInCustomWindowScript(URLRewriter.GetLink("DetailsProductPriceStatistics", _
                                                          New KeyValuePair(Of String, String)("ProductID", CType(Container.DataItem, Product).ID)), 0, 0, False, True, 1200, 585)%>'></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="AlternatingRowBudgets" />
                <HeaderStyle CssClass="GridViewHeaderStyle" />
                <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                <RowStyle Height="25px" />
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonFilter" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ButtonAddToShoppingList" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ButtonCancel" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <div id="ShoppingListAmount" style="display: none;">
        <asp:Panel ID="PanelShoppingListAmount" runat="server" DefaultButton="ButtonAddToShoppingList">
            <table>
                <tr>
                    <td colspan="4" align="center">
                        <span class="PlainTextBold" id="LabelShoppingListProductName"></span>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelAddToShoppingListAmount" runat="server" CssClass="PlainTextBold" Text="Amount:"></asp:Label>
                    </td>
                    <td>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server" ControlToValidate="TextBoxAddToShoppingListAmount"
                            CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                        <asp:TextBox ID="TextBoxAddToShoppingListAmount" runat="server" Width="30px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="ButtonAddToShoppingList" runat="server" CssClass="ButtonAddSmall" Text="Add" OnClientClick="CloseFancyBoxAndSubmit(this, event); return false;" />
                    </td>
                    <td>
                        <asp:Button ID="ButtonCancel" runat="server" CssClass="ButtonAddSmall" Text="Cancel" OnClientClick="CloseFancyBoxAndSubmit(this, event); return false;" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</div>