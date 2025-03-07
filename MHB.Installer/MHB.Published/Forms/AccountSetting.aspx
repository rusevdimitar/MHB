<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeBehind="AccountSetting.aspx.vb" Inherits="MHB.Web.AccountSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divContentBody" align="center" runat="server" id="PanelAccountSettingsMain">
        <table style="width: 100%;">
            <tr>
                <td colspan="3">
                    <asp:Label ID="LabelSelectCurrency" runat="server" Text="Select currency" CssClass="PlainTextBoldLarge"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelCurrency" runat="server" Text="Currency:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DropDownListCurrencies" runat="server" Width="204px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="ButtonSaveChanges" runat="server" CssClass="ButtonAddMedium" Text="Save changes" />
                </td>
            </tr>
            <!-- Currency -->
            <tr>
                <td colspan="3">
                    <hr />
                </td>
            </tr>
            <!-- *NewLine -->
            <tr>
                <td colspan="3">
                    <asp:Label ID="LabelChooseNewPassword" runat="server" Text="Choose a new password" CssClass="PlainTextBoldLarge"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelOldPassword" runat="server" Text="Old password:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxOldPassword" runat="server" CssClass="PlainTextBox" TextMode="Password" Width="200px"></asp:TextBox>
                </td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelNewPassword" runat="server" Text="New password:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxNewPassword" runat="server" CssClass="PlainTextBox" TextMode="Password"
                        Width="200px"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="ButtonSavePassword" runat="server" CssClass="ButtonAddMedium" Text="Save changes" />
                </td>
            </tr>
            <!-- Password -->
            <tr>
                <td colspan="3">
                    <hr />
                </td>
            </tr>
            <!-- *NewLine -->
            <tr>
                <td colspan="3">
                    <asp:Label ID="LabelCurrentLanguage" runat="server" Text="Current Language" CssClass="PlainTextBoldLarge"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:DropDownList ID="DropDownListCurrentLanguage" runat="server" Width="204px">
                        <asp:ListItem Value="1">English</asp:ListItem>
                        <asp:ListItem Value="2">Deutsch</asp:ListItem>
                        <asp:ListItem Value="0">Български</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="ButtonSaveCurrentLanguage" runat="server" CssClass="ButtonAddMedium"
                        Text="Save changes" />
                </td>
            </tr>
            <!-- Language -->
            <tr>
                <td colspan="3">
                    <hr />
                </td>
            </tr>
            <!-- *NewLine -->
            <tr>
                <td colspan="3">
                    <asp:Label ID="LabelRestrictAutoLoginTimeFrame" runat="server" Text="Auto login configuration" CssClass="PlainTextBoldLarge"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="CheckBoxAutoLoginEnabled" runat="server" Text="Enable auto login?" />
                </td>
                <td>
                    <asp:Label ID="LabelBoxDateTimePickerAutoAccessTimeLimitsStart" runat="server" Text="Start time:"></asp:Label>
                    <asp:TextBox ID="TextBoxDateTimePickerAutoAccessTimeLimitsStart" runat="server" CssClass="PlainTextBox" TextMode="Time"
                        Width="60px"></asp:TextBox>
                    <asp:Label ID="LabelBoxDateTimePickerAutoAccessTimeLimitsEnd" runat="server" Text="End time:"></asp:Label>
                    <asp:TextBox ID="TextBoxDateTimePickerAutoAccessTimeLimitsEnd" runat="server" CssClass="PlainTextBox" TextMode="Time"
                        Width="60px"></asp:TextBox>
                    <script type="text/javascript">

                        $('[id$=TextBoxDateTimePickerAutoAccessTimeLimitsStart]').datetimepicker({
                            datepicker: false,
                            format: 'H:i',
                            step: 5
                        });

                        $('[id$=TextBoxDateTimePickerAutoAccessTimeLimitsEnd]').datetimepicker({
                            datepicker: false,
                            format: 'H:i',
                            step: 5
                        });
                    </script>
                </td>
                <td>
                    <asp:Button ID="ButtonSaveAutoLoginSettings" runat="server" CssClass="ButtonAddMedium"
                        Text="Set auto login" /></td>
            </tr>
            <!-- Access restriction -->
        </table>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" CssClass="ValidatorSmall"
            ErrorMessage="Password length: 6-15 and no special characters  (ex.: &quot;@#$%^&amp;()/?.;,{}_[] etc.) ). Password must be alphanumeric (contain both numbers and letters)."
            ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,15})$" ControlToValidate="TextBoxNewPassword"></asp:RegularExpressionValidator>
        <div id="DivError" runat="server" align="center" class="ErrorDiv">
        </div>
        <div style="color: #FFFFFF; font-size: 1px;">
            лични финанси, управление лични финанси, финанси, лични, програма лични финанси,
            домашни разходи, домашен, разход, разходи, домашни, домакинство, планиране домакинство,
            разходи домакинство, месечен бюджет, планиране бюджет, програма бюджет, програма
            семеен бюджет, програма месечен бюджет, нещо семеен бюджет, планиране пари, планиране
            пари месец, заплата, планиране заплата, разпределяне заплата mese4en biudjet, mesechen
            budget, budget, programa mese4en biudjet, planirane bjudjet, semeen biudget, planirane
            zaplata, zaplata, planirane, bjudjet, biudget, budget, monthly budget, family budget,
            family bills, myhomebills, my home bills, home expenses, household, expenditure,
            cost
        </div>
    </div>
</asp:Content>