<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BillDetails.aspx.vb" Inherits="MHB.Web.BillDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Details</title>
    <link href="../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body leftmargin="0px" topmargin="0px" marginwidth="0px" marginheight="0px">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div id="DivLoading" class="LoadingDiv">
                <div style="border: solid 1px #C1DAFF">
                    <table style="width: 100%;">
                        <tr>
                            <td width="33px">
                                <img src="../Images/loading_dollar.gif" />
                            </td>
                            <td align="center">
                                Loading...
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div runat="server" id="DivModal">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <%--======================================= Attach div ==================================================--%>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div id="DivAttach" runat="server" style="visibility: hidden">
                    <table style="width: 100%;">
                        <tr>
                            <td class="PlainButton">
                                Choose a file ot attach:
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                <br />
                                <asp:Label ID="LabelFileMaxSize" CssClass="PlainTextError" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            </td>
                            <caption>
                                &nbsp;</td>
                            </caption>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="ButtonFileAttachConfirm" runat="server" CssClass="PlainButton" Text="OK"
                                    Width="70px" />
                                <asp:Button ID="ButtonFileAttachCancelDialog" runat="server" CssClass="PlainButton"
                                    Text="Cancel" Width="70px" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="ButtonFileAttachConfirm" />
                <asp:AsyncPostBackTrigger ControlID="ButtonFileAttachCancelDialog" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <%--=========================================================================================--%>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Panel ID="PanelDetailsTable" runat="server" DefaultButton="ButtonAddExpenditureDetails"
                    Width="100%">
                    <table style="width: 100%;">
                        <tr>
                            <td colspan="3" style="font-family: Arial, Helvetica, sans-serif; font-size: 18px;
                                font-weight: bold;">
                                <asp:Label ID="LabelDetailedExpenditure" runat="server" Text="Detailed expenditure:"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="border-width: 1px; border-color: #0066FF; font-family: Arial, Helvetica, sans-serif;
                                font-size: 12px; font-weight: bold; background-color: #D5E9FD; border-top-style: solid;">
                                <asp:Button ID="ButtonAddExpenditureDetails" runat="server" CssClass="PlainButton"
                                    Text="Add" />
                                <asp:Button ID="ButtonUpdateDetailsTable" runat="server" CssClass="PlainButton" Text="Save changes" />
                                <asp:Button ID="ButtonDeleteFromDetailsTable" runat="server" CssClass="PlainButton"
                                    Text="Delete selected" />
                                <asp:Button ID="ButtonDetailsTableAttach" runat="server" CssClass="PlainButton" Text="Attach" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:GridView ID="GridViewDetails" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
                                    Width="100%" BackColor="White">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBoxDetailsTableSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBoxDetailsTableID" runat="server" CssClass="GridCells" Text='<%# Bind("ID") %>'
                                                    Visible="false"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBoxDetTblHasAttachment" runat="server" Checked='<%# IIf(IsDBNull(Eval("HasAttachment")),"False",Eval("HasAttachment")) %>'
                                                    Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageDetTblHasAttachment" runat="server" CommandName="Edit"
                                                    ImageUrl="../Images/attach.png" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageDetTblDeleteAttachment" runat="server" CommandName="Delete"
                                                    ImageUrl="../Images/delete_attachment.gif" Visible="false" OnClientClick="javascript:return confirm('really delete?')" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBoxUserDetailsTableExpenditureID" runat="server" CssClass="GridCells"
                                                    Text='<%# Bind("ExpenditureID") %>' Visible="false"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Expenditure">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBoxDetailsTableFieldName" runat="server" CssClass="GridCells"
                                                    Text='<%# Bind("DetailName") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBoxDetailsTableFieldDescription" runat="server" CssClass="GridCells"
                                                    Text='<%# Bind("DetailDescription") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="TextBoxDetailsTableFieldValue" runat="server" CssClass="GridCells"
                                                                Text='<%# String.Format("{0:f}", Eval("DetailValue"))  %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxDetailsTableFieldValue"
                                                                CssClass="Validator" ErrorMessage="*" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBoxFieldDetailsTableDate" runat="server" CssClass="GridCells"
                                                    Text='<%# Bind("DetailDate") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="GridViewLastRowToEdit" />
                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                </asp:GridView>
                                <div id="DivError" runat="server" align="center" class="ErrorDiv">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonAddExpenditureDetails" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ButtonUpdateDetailsTable" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ButtonDeleteFromDetailsTable" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <asp:Button ID="ButtonCloseAndSave" runat="server" Text="Close and save" CssClass="PlainButton" />
    </form>
</body>
</html>