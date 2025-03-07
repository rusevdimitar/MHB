<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Mobile.aspx.vb" Inherits="MHB.Web.Mobile" %>

<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.2//EN" "http://www.openmobilealliance.org/tech/DTD/xhtml-mobile12.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MyHomeBills Mobile</title>
    <link href="../Styles/MobileStyles.css" rel="stylesheet" type="text/css" />
    <meta name="HandheldFriendly" content="true" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="main">
        <img src="../Images/logo_tiny.gif" alt="MyHomeBills.mobi" />
        <table style="background-color: #dfefff; width: 100%;">
            <tr>
                <td>
                    <asp:DropDownList ID="DropDownListMonths" runat="server" CssClass="PlainTextBold"
                        AutoPostBack="true">
                        <asp:ListItem Value="1" Text="January"></asp:ListItem>
                        <asp:ListItem Value="2" Text="February"></asp:ListItem>
                        <asp:ListItem Value="3" Text="March"></asp:ListItem>
                        <asp:ListItem Value="4" Text="April"></asp:ListItem>
                        <asp:ListItem Value="5" Text="May"></asp:ListItem>
                        <asp:ListItem Value="6" Text="June"></asp:ListItem>
                        <asp:ListItem Value="7" Text="July"></asp:ListItem>
                        <asp:ListItem Value="8" Text="August"></asp:ListItem>
                        <asp:ListItem Value="9" Text="September"></asp:ListItem>
                        <asp:ListItem Value="10" Text="October"></asp:ListItem>
                        <asp:ListItem Value="11" Text="November"></asp:ListItem>
                        <asp:ListItem Value="12" Text="December"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="DropDownListYears" runat="server" CssClass="PlainTextBold"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="PlainTextBold">
                    New Bill Name:
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TextBoxBillName" runat="server" CssClass="PlainTextBox"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="PlainTextBold">
                    Expected Value:
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TextBoxBillValue" runat="server" CssClass="PlainTextBox"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server" ControlToValidate="TextBoxBillValue"
                        CssClass="ErrorText" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="ButtonAdd" runat="server" Text="Add" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                        GridLines="Horizontal" BackColor="White" BorderWidth="0">
                        <Columns>
                            <asp:TemplateField HeaderText="Bill" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxFieldName" runat="server" CssClass="GridCells" Text='<%# Bind("FieldName") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Expected sum" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxFieldExpectedValue" runat="server" CssClass="GridCells" Width="70px"
                                        Text='<%# IIf(IsDBNull(Eval("FieldExpectedValue")), "0", String.Format("{0:f}", Eval("FieldExpectedValue")))%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sum" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxFieldValue" runat="server" CssClass="GridCells" Width="70px"
                                        Text='<%# String.Format("{0:f}", Eval("FieldValue")) %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <div>
        </div>
    </div>
    </form>
</body>
</html>