<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Admin.aspx.vb" Inherits="MHB.Web.Admin"
    EnableViewStateMac="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin page</title>
    <link href="../Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:LinkButton ID="LinkButtonMainPage" runat="server">Main page</asp:LinkButton>
    <table>
        <tr align="center">
            <td>
                <asp:ImageButton ID="ImageButtonAdminUsers" runat="server" ImageUrl="~/Images/admin_users_icon.png" />
            </td>
            <td>
                <asp:ImageButton ID="ImageButtonAdminExceptionsLog" runat="server" ImageUrl="~/Images/admin_exceptions_icon.png" />
            </td>
            <td>
                <asp:ImageButton ID="ImageButtonAdminMainTable" runat="server" ImageUrl="~/Images/admin_database_icon.png" />
            </td>
            <td>
                <asp:ImageButton ID="ImageButtonAdminControlsTranslation" runat="server" ImageUrl="~/Images/admin_translations_icon.png" />
            </td>
            <td>
                <asp:ImageButton ID="ImageButtonAdminCostCategories" runat="server" ImageUrl="~/Images/admin_categories_icon.png" />
            </td>
            <td>
                <asp:ImageButton ID="ImageButtonAdminMassEmail" runat="server" ImageUrl="~/Images/admin_mass_email.png" />
            </td>
            <td>
                <asp:ImageButton ID="ImageButtonAdminActionLog" runat="server" ImageUrl="~/Images/admin_action_log.png" />
            </td>
        </tr>
        <tr class="PlainTextBoldLarge" align="center">
            <td>
                Manage Users
            </td>
            <td>
                Exceptions Log
            </td>
            <td>
                Main Table
            </td>
            <td>
                Translations
            </td>
            <td>
                Cost Categories
            </td>
            <td>
                Mass Email
            </td>
            <td>
                Action Log
            </td>
        </tr>
    </table>
    <div id="DivManageUsers" runat="server" style="visibility: hidden;">
        <div class="AdminPageHeader">
            Manage users</div>
        <asp:LinkButton ID="LinkButtonSelectAll" runat="server">Select all</asp:LinkButton>
        &nbsp;|
        <asp:LinkButton ID="LinkButtonDeselectAll" runat="server">Deselect all</asp:LinkButton>
        <asp:Button ID="ButtonShowPassword" runat="server" Text="Show password" CssClass="PlainButton" />
        <asp:Button ID="ButtonHidePassword" runat="server" Text="Hide password" CssClass="PlainButton" />
        <asp:Button ID="ButtonSaveUserChanges" runat="server" Text="Save" CssClass="PlainButton" />
        <asp:Button ID="ButtonDelete" runat="server" Text="Delete" CssClass="PlainButton" />
        <asp:Label ID="LabelError" CssClass="PlainTextError" runat="server"></asp:Label>
        <asp:GridView ID="GridViewUsers" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
            Width="800px" BackColor="White">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxUsersTableSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="User ID">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxUserID" runat="server" TextMode="MultiLine" Rows="4" CssClass="GridCellsLang"
                            Text='<%# Bind("userID") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Password">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxUserPassword" runat="server" TextMode="MultiLine" Rows="4"
                            CssClass="GridCellsLang" Text='<%# Bind("password") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="E-Mail">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxUserEmail" runat="server" TextMode="MultiLine" Rows="4" CssClass="GridCellsLang"
                            Text='<%# Bind("email") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Currency">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxUserCurrency" runat="server" TextMode="MultiLine" Rows="4"
                            CssClass="GridCellsLang" Text='<%# Bind("currency") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CurrentLanguage">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxUserCurrentLanguage" runat="server" TextMode="MultiLine"
                            Rows="4" CssClass="GridCellsLang" Text='<%# Bind("language") %>' Visible="false"></asp:TextBox>
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="DropDownListLang" runat="server" Width="100px">
                                        <asp:ListItem>en</asp:ListItem>
                                        <asp:ListItem>de</asp:ListItem>
                                        <asp:ListItem>bg</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Image ID="ImageEn" runat="server" Visible="false" CssClass="DropDownListCurrentLanguage"
                                        ImageUrl="../Images/flag_en_tiny.JPG" />
                                    <asp:Image ID="ImageDe" runat="server" Visible="false" CssClass="DropDownListCurrentLanguage"
                                        ImageUrl="../Images/flag_de_tiny.JPG" />
                                    <asp:Image ID="ImageBg" runat="server" Visible="false" CssClass="DropDownListCurrentLanguage"
                                        ImageUrl="../Images/flag_bg_tiny.JPG" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="language set?">
                    <ItemTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="CheckBoxUserHasSetLang" Width="150px" CssClass="PlainTextBold"
                                        runat="server" Text='<%# IIf(IsDBNull(Eval("hassetlang")), "False", Eval("hassetlang")) %>'
                                        Checked='<%# IIf(IsDBNull(Eval("hassetlang")), 0, Eval("hassetlang")) %>' />
                                </td>
                                <td>
                                    <asp:Image ID="ImageTrue" runat="server" Visible="false" CssClass="DropDownListCurrentLanguage"
                                        ImageUrl="../Images/button_ok.png" />
                                    <asp:Image ID="ImageFalse" runat="server" Visible="false" CssClass="DropDownListCurrentLanguage"
                                        ImageUrl="../Images/button_cancel.png" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Registration date">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxUserRegistrationDate" runat="server" TextMode="MultiLine"
                            Rows="4" CssClass="GridCells" Text='<%# Bind("registrationdate") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Attachment size (bytes)">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxAttachmentSize" runat="server" TextMode="MultiLine" Rows="4"
                            CssClass="GridCellsLang" Text='<%# Bind("attachmentsize") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Last login">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxlastlogintime" runat="server" TextMode="MultiLine" Rows="4"
                            CssClass="GridCellsLang" Text='<%# Bind("lastlogintime") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Last IP">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxLastIP" runat="server" TextMode="MultiLine" Rows="4" CssClass="GridCellsLang"
                            Text='<%# Bind("lastipaddress") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Browser info">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxBrowserInfo" runat="server" TextMode="MultiLine" Rows="4"
                            CssClass="GridCellsLang" Text='<%# Bind("useragent") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="GridViewHeaderStyle" />
        </asp:GridView>
    </div>
    <div id="DivExceptionsLog" runat="server" style="visibility: hidden;">
        <div class="AdminPageHeader">
            Exceptions log</div>
        <asp:ImageButton ID="ImageButtonPickRecurrenDate" runat="server" ImageUrl="../Images/calendar.jpg" /><asp:Label
            ID="LabelLogDate" CssClass="PlainTextBold" runat="server"></asp:Label>
        <div style="position: absolute; background-color: #999999; font-family: Arial, Helvetica, sans-serif;
            font-size: 12px">
            <asp:Calendar ID="CalendarExceptions" Visible="false" runat="server"></asp:Calendar>
        </div>
        <asp:GridView ID="GridViewLog" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
            Width="800px" BackColor="White">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxExceptionsTableSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Method name">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxExceptionMethod" TextMode="MultiLine" Width="200px" Rows="4"
                            runat="server" CssClass="GridCellsEx" Text='<%# Bind("logMethod") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Message">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxExceptionMessage" TextMode="MultiLine" Rows="4" runat="server"
                            CssClass="GridCellsEx" Text='<%# Bind("logExceptionMessage") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="InnerException">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxInnerExceptionMessage" TextMode="MultiLine" Rows="4" runat="server"
                            CssClass="GridCellsEx" Text='<%# Bind("logInnerExceptionMessage") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Source">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxExceptionSource" TextMode="MultiLine" Rows="4" runat="server"
                            CssClass="GridCellsEx" Text='<%# Bind("logExceptionSource") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="StackTrace">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxExceptionStackTrace" TextMode="MultiLine" Rows="4" runat="server"
                            CssClass="GridCellsEx" Text='<%# Bind("logStackTrace") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="User">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxUserID" TextMode="MultiLine" Width="60px" Rows="4" runat="server"
                            CssClass="GridCellsEx" Text='<%# Bind("logUserID") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxExceptionDate" TextMode="MultiLine" Width="145px" Rows="4"
                            runat="server" CssClass="GridCellsEx" Text='<%# Bind("logDateTime") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SQL query">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxExceptionQuery" TextMode="MultiLine" Width="450px" Rows="4"
                            runat="server" CssClass="GridCellsEx" Text='<%# Bind("logSqlQuery") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="GridViewHeaderStyle" />
        </asp:GridView>
    </div>
    <div id="DivMainTable" runat="server" style="visibility: hidden;">
        <div class="AdminPageHeader">
            Main table</div>
        <asp:LinkButton ID="LinkButtonMainTablesGridSelectAll" runat="server">Select all</asp:LinkButton>&nbsp;|
        <asp:LinkButton ID="LinkButtonMainTablesGridDeselectAll" runat="server">Deselect all</asp:LinkButton>&nbsp;
        <asp:DropDownList ID="DropDownListMainTableIndex" runat="server" AutoPostBack="True">
            <asp:ListItem>- Select table</asp:ListItem>
            <asp:ListItem>tbMainTable01</asp:ListItem>
            <asp:ListItem>tbMainTable02</asp:ListItem>
            <asp:ListItem>tbMainTable03</asp:ListItem>
            <asp:ListItem>tbMainTable04</asp:ListItem>
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownListIDRange" runat="server" AutoPostBack="True">
            <asp:ListItem Value="0">- ID Range</asp:ListItem>
            <asp:ListItem Value="100">0 - 100</asp:ListItem>
            <asp:ListItem Value="200">100 - 200</asp:ListItem>
            <asp:ListItem Value="300">200 - 300</asp:ListItem>
            <asp:ListItem Value="400">300 - 400</asp:ListItem>
            <asp:ListItem Value="500">400 - 500</asp:ListItem>
            <asp:ListItem Value="600">500 - 600</asp:ListItem>
            <asp:ListItem Value="700">600 - 700</asp:ListItem>
            <asp:ListItem Value="800">700 - 800</asp:ListItem>
            <asp:ListItem Value="900">800 - 900</asp:ListItem>
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownListUsers" runat="server" AutoPostBack="True">
        </asp:DropDownList>
        <asp:DropDownList ID="DropDownListHasAttachment" runat="server" AutoPostBack="True">
            <asp:ListItem Value="-1">- Has attachment</asp:ListItem>
            <asp:ListItem Value="1">True</asp:ListItem>
            <asp:ListItem Value="0">False</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="ButtonMainTableSave" runat="server" CssClass="PlainButton" Text="Save" />
        <asp:GridView ID="GridViewMainTable" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
            Width="800px" BackColor="White">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxMainTableSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ID">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxID" runat="server" CssClass="GridCells" Text='<%# Bind("ID") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UserID">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxUserID" runat="server" CssClass="GridCells" Text='<%# Bind("UserID") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Month">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxMonth" runat="server" CssClass="GridCells" Text='<%# Bind("Month") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Year">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxYear" runat="server" CssClass="GridCells" Text='<%# Bind("Year") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FieldName">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxFieldName" runat="server" CssClass="GridCells" Text='<%# Bind("FieldName") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FieldDescription">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxFieldDescription" runat="server" CssClass="GridCells" Text='<%# Bind("FieldDescription") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FieldValue">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxFieldValue" runat="server" CssClass="GridCells" Text='<%# Bind("FieldValue") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DueDate">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxDueDate" runat="server" CssClass="GridCells" Text='<%# Bind("DueDate") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DateRecordUpdated">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxDateRecordUpdated" runat="server" CssClass="GridCells" Text='<%# Bind("DateRecordUpdated") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IsPaid">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxIsPaid" runat="server" Checked='<%# IIf(IsDBNull(Eval("IsPaid")),False,Eval("IsPaid")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="HasDetails">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxHasDetails" runat="server" Checked='<%# IIf(IsDBNull(Eval("HasDetails")),False,Eval("HasDetails")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Attachment">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxAttachment" runat="server" CssClass="GridCells" Text='<%# Bind("Attachment") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AttachmentFileType">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxAttachmentFileType" runat="server" CssClass="GridCells" Text='<%# Bind("AttachmentFileType") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="HasAttachment">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxHasAttachment" runat="server" Checked='<%# IIf(IsDBNull(Eval("HasAttachment")),False,Eval("HasAttachment")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OrderID">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxOrderID" runat="server" CssClass="GridCells" Text='<%# Bind("OrderID") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="GridViewHeaderStyle" />
        </asp:GridView>
    </div>
    <div id="DivControlsTranslation" runat="server" style="visibility: hidden;">
        <div class="AdminPageHeader">
            Control translations</div>
        <asp:Button ID="ButtonLangSave" runat="server" Text="Save" CssClass="PlainButton" />
        <asp:GridView ID="GridViewCurrentLanguage" runat="server" AutoGenerateColumns="False"
            GridLines="Horizontal" Width="800px" BackColor="White">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxCurrentLanguageTableSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Control ID">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxControlID" TextMode="MultiLine" Width="200px" Rows="4" runat="server"
                            CssClass="GridCellsLang" Text='<%# Bind("ControlID") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="English">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxControlTextEN" TextMode="MultiLine" Rows="4" runat="server"
                            CssClass="GridCellsLang" Text='<%# Bind("ControlTextEN") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Български">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxControlTextBG" TextMode="MultiLine" Rows="4" runat="server"
                            CssClass="GridCellsLang" Text='<%# Bind("ControlTextBG") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Deutsch">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxControlTextDE" TextMode="MultiLine" Rows="4" runat="server"
                            CssClass="GridCellsLang" Text='<%# Bind("ControlTextDE") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="GridViewHeaderStyle" />
        </asp:GridView>
        <table>
            <tr>
                <td>
                    <p class="PlainTextBold">
                        Control ID:</p>
                    <asp:TextBox ID="TextBoxControlID" runat="server" CssClass="PlainTextBox" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <p class="PlainTextBold">
                        English:</p>
                    <asp:TextBox ID="TextBoxEN" runat="server" CssClass="PlainTextBox" Width="200px"
                        TextMode="MultiLine" Rows="4"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <p class="PlainTextBold">
                        Deutsch:</p>
                    <asp:TextBox ID="TextBoxDE" runat="server" CssClass="PlainTextBox" Width="200px"
                        TextMode="MultiLine" Rows="4"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <p class="PlainTextBold">
                        Български:</p>
                    <asp:TextBox ID="TextBoxBG" runat="server" CssClass="PlainTextBox" Width="200px"
                        TextMode="MultiLine" Rows="4"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="AddNewTranlation" runat="server" CssClass="PlainButton" Text="Add record" />
                </td>
            </tr>
        </table>
    </div>
    <div id="DivCostCategories" runat="server" style="visibility: hidden;">
        <div class="AdminPageHeader">
            Cost categories automatic recognition - keywords management</div>
        <table>
            <tr>
                <td>
                    <span class="PlainTextBold">Category id:</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxCategoryID" runat="server" CssClass="PlainTextBox"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="PlainTextBold">Keyword:</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxKeyword" runat="server" CssClass="PlainTextBox"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="ButtonInsertCostCategory" runat="server" Text="Insert" CssClass="PlainButton" />
                </td>
                <td>
                    <asp:Button ID="ButtonSaveCostCategory" runat="server" Text="Save" CssClass="PlainButton" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="GridViewCostCategories" runat="server" AutoGenerateColumns="False"
            GridLines="Horizontal" Width="400px" BackColor="White">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxCostcategoriesTableSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxCostCategoriesID" TextMode="MultiLine" Rows="4" runat="server"
                            CssClass="GridCellsLang" Text='<%# Bind("ID") %>' Visible="false"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Category ID">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxCostCategoryID" TextMode="MultiLine" Rows="4" runat="server"
                            CssClass="GridCellsLang" Text='<%# Bind("CostCategoryID") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Keywords">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBoxCostNames" TextMode="MultiLine" Rows="4" runat="server" CssClass="GridCellsLang"
                            Text='<%# Bind("CostNames") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="GridViewHeaderStyle" />
        </asp:GridView>
    </div>
    <div id="DivActionLog" runat="server" style="visibility: hidden;">
        <table>
            <tr>
                <td>
                    <asp:Calendar ID="CalendarActionLogStartDate" runat="server" BackColor="White" Font-Names="Arial"
                        Font-Size="9"></asp:Calendar>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridViewUniqueVisitors" runat="server" BackColor="White" GridLines="Horizontal"
                        AutoGenerateColumns="false" ShowFooter="true">
                        <Columns>
                            <asp:TemplateField HeaderText="User Email:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxUserEmail" runat="server" Text='<%# Eval("UserEmail") %>'
                                        CssClass="GridCellsLang" TextMode="MultiLine" Rows="2" Width="200"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="LabelVisitorsCount" runat="server" Text='<%# String.Format("Visitors since {0} - {1}", CalendarActionLogStartDate.SelectedDate.ToShortDateString(), GridViewUniqueVisitors.Rows.Count) %>'
                                        CssClass="PlainTextBoldLarge"></asp:Label>
                                </FooterTemplate>
                                <FooterStyle BackColor="GhostWhite" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="User Password:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxUserPassword" runat="server" Text='<%# Eval("UserPassword") %>'
                                        CssClass="GridCellsLang" TextMode="MultiLine" Rows="2" Width="200"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <FooterStyle BackColor="GhostWhite" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Registration Date:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxUserRegistrationDate" runat="server" Text='<%# Eval("UserRegistrationDate") %>'
                                        CssClass="GridCellsLang" TextMode="MultiLine" Rows="2" Width="200"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <FooterStyle BackColor="GhostWhite" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last IP:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxUserLastIPAddress" runat="server" Text='<%# Eval("UserLastIPAddress") %>'
                                        CssClass="GridCellsLang" TextMode="MultiLine" Rows="2" Width="200"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <FooterStyle BackColor="GhostWhite" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                    </asp:GridView>
                    <asp:GridView ID="GridViewActionLog" runat="server" BackColor="White" GridLines="Horizontal"
                        AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Transaction:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxTransaction" runat="server" Text='<%# Eval("TransactionMessage") %>'
                                        CssClass="GridCellsLang" TextMode="MultiLine" Rows="2" Width="200"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="User ID:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxUserID" runat="server" Text='<%# Eval("UserID") %>' CssClass="GridCellsLang"
                                        TextMode="MultiLine" Rows="2" Width="50"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxUserEmail" runat="server" Text='<%# Eval("UserEmail") %>'
                                        CssClass="GridCellsLang" TextMode="MultiLine" Rows="2" Width="200"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Password:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxUserPassword" runat="server" Text='<%# Eval("UserPassword") %>'
                                        CssClass="GridCellsLang" TextMode="MultiLine" Rows="2" Width="100"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Time:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxLogDate" runat="server" Text='<%# Eval("LogDate") %>' CssClass="GridCellsLang"
                                        TextMode="MultiLine" Rows="2" Width="150"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Message:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxMessage" runat="server" Text='<%# Eval("Message") %>' CssClass="GridCellsLang"
                                        TextMode="MultiLine" Rows="2" Width="150"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IP:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxIP" runat="server" Text='<%# Eval("IP") %>' CssClass="GridCellsLang"
                                        TextMode="MultiLine" Rows="2" Width="120"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Country Code:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxCountryCode" runat="server" Text='<%# Eval("CountryCode") %>'
                                        CssClass="GridCellsLang" TextMode="MultiLine" Rows="2" Width="100"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="City:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxCity" runat="server" Text='<%# Eval("City") %>' CssClass="GridCellsLang"
                                        TextMode="MultiLine" Rows="2" Width="120"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Region:">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxRegion" runat="server" Text='<%# Eval("Region") %>' CssClass="GridCellsLang"
                                        TextMode="MultiLine" Rows="2" Width="120"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>