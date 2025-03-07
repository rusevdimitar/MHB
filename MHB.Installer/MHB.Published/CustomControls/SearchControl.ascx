<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SearchControl.ascx.vb"
    Inherits="MHB.Web.SearchControl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

 <script type="text/javascript" language="javascript">
     
        function SearchAutoCompleteItemSelected(source, eventArgs) {
           
            __doPostBack('<%= ButtonPerformSearch.ClientID%>', eventArgs.get_value());
        }
     </script>

<table>
    <tr>
        <td nowrap="nowrap" align="right">
            <asp:Label ID="LabelSearchString" runat="server" CssClass="PlainTextLarge" Text="Bill name or description:"></asp:Label>
        </td>
        <td colspan="2">
            <asp:TextBox ID="TextBoxSearchString" runat="server" Width="205px"></asp:TextBox>
            <asp:AutoCompleteExtender
                runat="server"
                BehaviorID="AutoCompleteSearchEx"
                ID="AutoCompleteSearch"
                TargetControlID="TextBoxSearchString"
                ServicePath="~/Forms/SearchAutoComplete.asmx"
                ServiceMethod="GetSearchKeywords"
                MinimumPrefixLength="2"
                CompletionInterval="10"
                EnableCaching="true"
                CompletionSetCount="20"
                DelimiterCharacters=";, :"
                OnClientItemSelected="SearchAutoCompleteItemSelected"
                ShowOnlyCurrentWordInCompletionListItem="true"
                CompletionListCssClass="autocomplete_completionListElement"
                CompletionListItemCssClass="autocomplete_listItem"
                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                <animations>
                    <OnShow>
                        <Sequence>
                            <%-- Make the completion list transparent and then show it --%>
                            <OpacityAction Opacity="0" />
                            <HideAction Visible="true" />
                            
                            <%--Cache the original size of the completion list the first time
                                the animation is played and then set it to zero --%>
                            <ScriptAction Script="
                                // Cache the size and setup the initial size
                                var behavior = $find('AutoCompleteSearchEx');
                                if (!behavior._height) {
                                    var target = behavior.get_completionList();
                                    behavior._height = target.offsetHeight - 2;
                                    target.style.height = '0px';
                                }" />
                            
                            <%-- Expand from 0px to the appropriate size while fading in --%>
                            <Parallel Duration=".4">
                                <FadeIn />
                                <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteSearchEx')._height" />
                            </Parallel>
                        </Sequence>
                    </OnShow>
                    <OnHide>
                        <%-- Collapse down to 0px and fade out --%>
                        <Parallel Duration=".4">
                            <FadeOut />
                            <Length PropertyKey="height" StartValueScript="$find('AutoCompleteSearchEx')._height" EndValue="0" />
                        </Parallel>
                    </OnHide>
                </animations>
            </asp:AutoCompleteExtender>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="LabelSearchBySum" runat="server" CssClass="PlainTextLarge" Text="Search By Sum:"></asp:Label>
        </td>
        <td align="left" colspan="2">
            <asp:DropDownList ID="DropDownListHigherOrLowerThan" Width="40px" runat="server">
                <asp:ListItem Text=">" Selected="true" Value=">"></asp:ListItem>
                <asp:ListItem Text="<" Value="<"></asp:ListItem>
                <asp:ListItem Text="=" Value="="></asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="TextBoxSum" runat="server" Width="160px"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server" ControlToValidate="TextBoxSum"
                CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="LabelSelectYear" CssClass="PlainTextLarge" runat="server" Text="Select Year To Search Through:"></asp:Label>
        </td>
        <td colspan="2">
            <asp:DropDownList ID="DropDownListYear" runat="server">
            </asp:DropDownList>
            <asp:CheckBox ID="CheckBoxSeachByYearToo" CssClass="PlainTextLarge" runat="server"
                Text="Search By Year?" Checked="false" />
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="LabelSearchByCategory" runat="server" Text="Category:"></asp:Label>
        </td>
        <td colspan="2">
            <asp:DropDownList ID="DropDownListCategory" runat="server">
            </asp:DropDownList>
            <asp:CheckBox ID="CheckBoxSearchByCategory" runat="server" Text="Search By Category?" />
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="LabelSortBy" runat="server" Text="Sort By:"></asp:Label>
        </td>
        <td colspan="2">
            <asp:DropDownList ID="DropDownListSortBy" runat="server">
            </asp:DropDownList>
            <asp:DropDownList ID="DropDownListSortDirection" runat="server">
                <asp:ListItem Text="ASC" Value="0"></asp:ListItem>
                <asp:ListItem Text="DESC" Value="1"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:RadioButtonList ID="RadioButtonListSearchOptions" runat="server" CssClass="PlainTextLarge"
                RepeatDirection="Horizontal">
                <asp:ListItem Text="Search By Sum" Value="0" id="rliSearchBySum"></asp:ListItem>
                <asp:ListItem Text="Search By Text" Value="1" id="rliSearchByText" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Search By Details" Value="2" id="rliSearchByDetails"></asp:ListItem>
                <asp:ListItem Text="Both" Value="3" id="rliBoth"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:Label ID="LabelStatus" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:Button ID="ButtonPerformSearch" runat="server" CssClass="ButtonSearch" Text="Search" />
        </td>
    </tr>
</table>
