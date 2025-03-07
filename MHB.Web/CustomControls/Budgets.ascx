<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Budgets.ascx.vb" Inherits="MHB.Web.Budgets" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="MHB.BL" %>
<asp:LinkButton ID="lnkShowAddNewIncomePanel" runat="server" Text="Add New Income"></asp:LinkButton>
<br />
<asp:Panel ID="pnlAddNewIncome" runat="server" Visible="false">
    <table>
        <tr>
            <td>
                <asp:Label ID="lblNewIncomeName" runat="server" Text="Income Name:" CssClass="PlainTextBold"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNewIncomeName" runat="server" CssClass="PlainTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblNewIncomeValue" runat="server" Text="Income Value:" CssClass="PlainTextBold"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNewIncomeValue" runat="server" CssClass="PlainTextBox"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server" ControlToValidate="txtNewIncomeValue"
                    CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblNewIncomeDate" runat="server" Text="Income Date:" CssClass="PlainTextBold"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNewIncomeDate" runat="server" CssClass="PlainTextBox"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtenderIncomeDate" TargetControlID="txtNewIncomeDate"
                    runat="server">
                </asp:CalendarExtender>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDueDateGrid" runat="server"
                    ControlToValidate="txtNewIncomeDate" CssClass="Validator" ErrorMessage="!!!"
                    ValidationExpression="(?=\d)^(?:(?!(?:10\D(?:0?[5-9]|1[0-4])\D(?:1582))|(?:0?9\D(?:0?[3-9]|1[0-3])\D(?:1752)))((?:0?[13578]|1[02])|(?:0?[469]|11)(?!\/31)(?!-31)(?!\.31)|(?:0?2(?=.?(?:(?:29.(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:(?:\d\d)(?:[02468][048]|[13579][26])(?!\x20BC))|(?:00(?:42|3[0369]|2[147]|1[258]|09)\x20BC))))))|(?:0?2(?=.(?:(?:\d\D)|(?:[01]\d)|(?:2[0-8])))))([-.\/])(0?[1-9]|[12]\d|3[01])\2(?!0000)((?=(?:00(?:4[0-5]|[0-3]?\d)\x20BC)|(?:\d{4}(?!\x20BC)))\d{4}(?:\x20BC)?)(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnAddNewIncome" runat="server" Text="Add" CssClass="ButtonAddSmall" />
                <asp:Button ID="btnCancelAddNewIncome" runat="server" Text="Cancel" CssClass="ButtonAddSmall" />
            </td>
        </tr>
    </table>
</asp:Panel>
<br />
<asp:GridView ID="GridViewBudgets" runat="server" AutoGenerateColumns="false" GridLines="None"
    BackColor="GhostWhite" BorderStyle="None" BorderWidth="0px" ShowHeader="false"
    ShowFooter="true" Width="500px" FooterStyle-CssClass="BudgetsGridViewFooter">
    <Columns>
        <asp:TemplateField Visible="False">
            <ItemTemplate>
                <asp:Label ID="lblID" runat="server" Text='<%# CType(Container.DataItem, Income).ID %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField Visible="False">
            <ItemTemplate>
                <asp:Label ID="lblUserID" runat="server" Text='<%# CType(Container.DataItem, Income).UserID %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField Visible="False">
            <ItemTemplate>
                <asp:Label ID="lblMonth" runat="server" Text='<%# CType(Container.DataItem, Income).Month %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="incomename">
            <ItemTemplate>
                <asp:Label ID="lblIncomeName" runat="server" Text='<%# CType(Container.DataItem, Income).Name %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtIncomeName" runat="server" CssClass="PlainTextBox" Text='<%# CType(Container.DataItem, Income).Name %>'></asp:TextBox>
            </EditItemTemplate>
            <FooterTemplate>
                <br />
                <asp:Label ID="lblSum" runat="server" Text="" CssClass="PlainTextExtraLarge"></asp:Label>
                <asp:Label ID="lblDiffernce" runat="server" Text="" CssClass="PlainTextExtraLarge"></asp:Label>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="incomevalue">
            <ItemTemplate>
                <asp:Label ID="lblIncomeValue" runat="server" Width="40%" Text='<%# String.Format("{0:f}", CType(Container.DataItem, Income).Value) %>'
                    CssClass="PlainTextBoldLarge"></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtIncomeValue" runat="server" CssClass="PlainTextBox" Text='<%# CType(Container.DataItem, Income).Value %>'></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server" ControlToValidate="txtIncomeValue"
                    CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="incomedate">
            <ItemTemplate>
                <asp:Label ID="lblIncomeDate" runat="server" Text='<%# CType(Container.DataItem, Income).Date %>'
                    Visible='<%# CDate(CType(Container.DataItem, Income).Date).Year> 1910 %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtIncomeDate" runat="server" CssClass="PlainTextBox" Text='<%# IIf(CDate(CType(Container.DataItem, Income).Date).Year> 1910, String.Format("{0:M/d/yyyy}", CType(Container.DataItem, Income).Date), String.Empty) %>'></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtenderIncomeDate" TargetControlID="txtIncomeDate"
                    runat="server">
                </asp:CalendarExtender>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDueDateGrid" runat="server"
                    ControlToValidate="txtIncomeDate" CssClass="Validator" ErrorMessage="!!!" ValidationExpression="(?=\d)^(?:(?!(?:10\D(?:0?[5-9]|1[0-4])\D(?:1582))|(?:0?9\D(?:0?[3-9]|1[0-3])\D(?:1752)))((?:0?[13578]|1[02])|(?:0?[469]|11)(?!\/31)(?!-31)(?!\.31)|(?:0?2(?=.?(?:(?:29.(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:(?:\d\d)(?:[02468][048]|[13579][26])(?!\x20BC))|(?:00(?:42|3[0369]|2[147]|1[258]|09)\x20BC))))))|(?:0?2(?=.(?:(?:\d\D)|(?:[01]\d)|(?:2[0-8])))))([-.\/])(0?[1-9]|[12]\d|3[01])\2(?!0000)((?=(?:00(?:4[0-5]|[0-3]?\d)\x20BC)|(?:\d{4}(?!\x20BC)))\d{4}(?:\x20BC)?)(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$"></asp:RegularExpressionValidator>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# CType(Container.DataItem, Income).ID %>'></asp:LinkButton>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                    CommandArgument='<%# CType(Container.DataItem, Income).ID %>'></asp:LinkButton>
            </EditItemTemplate>
            <ItemStyle HorizontalAlign="Right" Width="50" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                    CommandArgument='<%# CType(Container.DataItem, Income).ID %>' OnClientClick="javascript:return confirm('Really delete?')"></asp:LinkButton>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                    CommandArgument='<%# CType(Container.DataItem, Income).ID %>'></asp:LinkButton>
            </EditItemTemplate>
            <ItemStyle Width="50" />
        </asp:TemplateField>
    </Columns>
    <AlternatingRowStyle CssClass="AlternatingRowBudgets" />
</asp:GridView>