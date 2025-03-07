<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CategoriesManagement.ascx.vb"
    Inherits="MHB.Web.CategoriesManagement" %>
<%@ Import Namespace="MHB.BL" %>
<%@ Import Namespace="System.IO" %>
<%@ Register Src="CategoryIcons.ascx" TagName="CategoryIcons" TagPrefix="cat" %>
<div class="divDefaultBody">
    <fieldset>
        <legend>
            <asp:Label ID="LabelAddNewCategoryHeaderText" runat="server" Text="Add new user-defined category"
                CssClass="PlainTextExtraLarge"></asp:Label></legend>
        <table>
            <tr>
                <td valign="top">
                    <table width="550" style="background-color: #FBFDFF; border-style: solid; border-width: 5px; border-color: #F9FCFF;">
                        <tr align="left">
                            <td>
                                <asp:Label ID="LabelCategoryName" runat="server" Text="Name:" CssClass="PlainTextBold"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxCategoryName" runat="server" CssClass="PlainTextBox" Width="260px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr align="left">
                            <td valign="top">
                                <asp:Label ID="LabelCategoryKeywords" runat="server" Text="Keywords:" CssClass="PlainTextBold"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxCategoryKeywords" runat="server" TextMode="MultiLine" Rows="4"
                                    CssClass="PlainTextBox" Width="260px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr align="left" runat="server" id="trUploadIcon">
                            <td valign="top">
                                <asp:Label ID="LabelUploadIcon" runat="server" Text="Upload icon:" CssClass="PlainTextBold"></asp:Label>
                            </td>
                            <td>
                                <asp:FileUpload ID="FileUploadIcon" runat="server" CssClass="PlainTextBox" Width="264px" /><br />
                                <asp:Label ID="LabelCategoryAllowedIconFormats" runat="server" Text="max. size: 22x22 px; allowed formats: *.jpeg|*.png|*.bmp|*.gif"
                                    CssClass="PlainTextSmall"></asp:Label><br />
                            </td>
                        </tr>
                        <tr runat="server" id="trSelectIcon" visible="false">
                            <td align="right">
                                <div style="float: left;">
                                    <asp:Label ID="LabelSelectedIconText" runat="server" Text="Selected icon:" CssClass="PlainTextBold"></asp:Label>
                                </div>
                                <asp:Image ID="ImageSelectedIcon" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="LabelSelectedIconFileName" runat="server" Text=""></asp:Label>
                                <asp:LinkButton ID="LinkButtonUploadIcon" runat="server" Text="Upload new icon"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr align="left">
                            <td>
                                <asp:Label ID="LabelIsPayIconVisible" runat="server" Text="Online Payment?" CssClass="PlainTextBold"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBoxIsPayIconVisible" runat="server" />
                            </td>
                        </tr>
                        <tr align="left">
                            <td>
                                <asp:Label ID="LabelShareNewCategory" runat="server" Text="Share?" CssClass="PlainTextBold"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBoxIsShared" runat="server" />
                            </td>
                        </tr>
                        <tr align="left">
                            <td></td>
                            <td>
                                <asp:Button ID="ButtonUserDefinedCategorySave" runat="server" CssClass="ButtonAddSmall"
                                    Text="Add" />
                                <asp:Button ID="ButtonUserDefinedCategoryClear" runat="server" CssClass="ButtonAddSmall"
                                    Text="Clear" />
                                <asp:Button ID="ButtonUserDefinedCategoryBack" runat="server" CssClass="ButtonAddSmall"
                                    Text="Back" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <cat:CategoryIcons ID="CategoryIcons1" runat="server"></cat:CategoryIcons>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>
            <asp:Label ID="LabelCategoriesList" runat="server" Text="Categories list" CssClass="PlainTextExtraLarge"></asp:Label></legend>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:GridView ID="GridViewCategories" runat="server" AutoGenerateColumns="false"
                    Width="100%" BorderWidth="0" GridLines="None">
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LabelIsShared" runat="server" Text='<%# CType(Container.DataItem, Category).IsShared %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="LabelUserCategoryName" runat="server" Text='<%# CType(Container.DataItem, Category).Name %>'
                                    CssClass="PlainTextBold"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Icon">
                            <ItemTemplate>
                                <img alt="Category Icon Path" src='<%# CType(Container.DataItem, Category).IconPath %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File name">
                            <ItemTemplate>
                                <asp:Label ID="LabelFileName" runat="server" Text='<%# Path.GetFileName(CType(Container.DataItem, Category).IconPath) %>'
                                    CssClass="PlainText"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Keywords">
                            <ItemTemplate>
                                <asp:Label ID="LabelKeywords" runat="server" Text='<%# CType(Container.DataItem, Category).CategoryKeywords %>'
                                    CssClass="PlainTextSmall" Visible='<%# CType(Container.DataItem, Category).ID> 10 %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBoxEditCategoryKeyWords" runat="server" Text='<%# CType(Container.DataItem, Category).CategoryKeywords %>'
                                    Width="80%"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButtonSetShare" runat="server" CommandName="SetShare" CommandArgument='<%# CType(Container.DataItem, Category).ID %>'
                                    Enabled='<%# CType(Container.DataItem, Category).ID> 10 %>' Text=""></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="Edit" CommandName="Edit"
                                    CommandArgument='<%# CType(Container.DataItem, Category).ID %>' Enabled='<%# CType(Container.DataItem, Category).ID> 10 %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkButtonUpdate" runat="server" Text="Save" CommandName="Update"
                                    CommandArgument='<%# CType(Container.DataItem, Category).ID%>'></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButtonDelete" runat="server" Text="Delete" CommandName="Delete"
                                    CommandArgument='<%# CType(Container.DataItem, Category).ID %>' Enabled='<%# CType(Container.DataItem, Category).ID> 10 %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkButtonCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                    CommandArgument='<%# CType(Container.DataItem, Category).ID %>'></asp:LinkButton>
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
        </asp:UpdatePanel>
    </fieldset>
    <br />
    <fieldset>
        <legend>
            <asp:Label ID="LabelUsersCategoriesList" runat="server" Text="Other users' shared categories list"
                CssClass="PlainTextExtraLarge"></asp:Label></legend>
        <asp:UpdatePanel ID="UpdatePanelUsersCategories" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:GridView ID="GridViewUsersCategories" runat="server" AutoGenerateColumns="false"
                    Width="100%" BorderWidth="0" GridLines="None">
                    <Columns>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="LabelUserCategoryName" runat="server" Text='<%# CType(Container.DataItem, Category).Name %>'
                                    CssClass="PlainTextBold"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="120px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Icon">
                            <ItemTemplate>
                                <img alt="x" src='<%# CType(Container.DataItem, Category).IconPath %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File name">
                            <ItemTemplate>
                                <asp:Label ID="LabelFileName" runat="server" Text='<%# Path.GetFileName(CType(Container.DataItem, Category).IconPath) %>'
                                    CssClass="PlainText"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Keywords">
                            <ItemTemplate>
                                <asp:Label ID="LabelKeywords" runat="server" Text='<%# CType(Container.DataItem, Category).CategoryKeywords %>'
                                    CssClass="PlainTextSmall" Visible='<%# CType(Container.DataItem, Category).ID> 10 %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBoxEditCategoryKeyWords" runat="server" Text='<%# CType(Container.DataItem, Category).CategoryKeywords %>'
                                    Width="80%"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="ImageCommentsNotification" runat="server" ImageUrl="~/Images/CalloutIcon.png" Visible='<%# CType(Container.DataItem, Category).CommentsCount > 0 %>' />
                                <asp:Label ID="LabelNotificationsCount" runat="server" CssClass="PlainTextTiny" Text='<%# CType(Container.DataItem, Category).CommentsCount %>' Visible='<%# CType(Container.DataItem, Category).CommentsCount > 0 %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButtonUse" runat="server" Text="Use" CommandName="Use" CommandArgument='<%# CType(Container.DataItem, Category).ID %>'
                                    Enabled='<%# CType(Container.DataItem, Category).ID> 10 %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButtonCategoryComments" runat="server" Text="Comments" UserID='<%# CType(Container.DataItem, Category).UserID %>' CommandName="Comments" CommandArgument='<%# CType(Container.DataItem, Category).ID %>'
                                    OnClientClick='<%# String.Format("javascript:ShowCategoriesCommentsTable(""{0}"");", CType(Container.DataItem, Category).Name)%>'></asp:LinkButton>

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass="AlternatingRowBudgets" />
                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                    <RowStyle Height="25px" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="PanelCategoriesCommentsTable" style="display: none;">
            <asp:UpdatePanel ID="UpdatePanelComments" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:GridView ID="GridViewComments" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
                        ShowHeader="false" BorderWidth="0" CssClass="GridDetails" Width="500px">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>

                                    <asp:LinkButton ID="LinkButtonVoteUp" runat="server"
                                        CommandName="VoteUp" CommandArgument='<%# CType(Container.DataItem, CategoryComment).ID%>'>
                                        <asp:Image ID="ImageCommentVoteUp" ImageUrl="~/Images/thumbs_up.gif" ToolTip="" runat="server" />
                                    </asp:LinkButton>

                                    <asp:Label ID="LabelPositiveVotes" runat="server" CssClass="PlainTextTiny" Text='<%# CType(Container.DataItem, CategoryComment).PositiveVotesCount%>'></asp:Label>

                                    <asp:LinkButton ID="LinkButtonCommentVoteDown" runat="server"
                                        CommandName="VoteDown" CommandArgument='<%# CType(Container.DataItem, CategoryComment).ID%>'>
                                        <asp:Image ID="ImageCommentVoteDown" ImageUrl="~/Images/thumbs_down.gif" ToolTip="" runat="server" />
                                    </asp:LinkButton>

                                    <asp:Label ID="LabelNegativeVotes" runat="server" CssClass="PlainTextTiny" Text='<%# CType(Container.DataItem, CategoryComment).NegativeVotesCount%>'></asp:Label>
                                    <asp:Label ID="LabelCommentPoster" runat="server" Font-Bold="true" Text='<%# CType(Container.DataItem, CategoryComment).Poster%>'></asp:Label>
                                    <%= Me._en.GetTranslatedValue("Says", Me._en.CurrentLanguage) %>
                                    <br />
                                    <asp:Label ID="LabelComment" runat="server" Text='<%# CType(Container.DataItem, CategoryComment).Comment%>' />
                                    <br />
                                    <br />
                                    <asp:LinkButton ID="LinkButtonDeleteComment" runat="server" Visible='<%# Me._en.UserID = 1  %>' Text="Delete" CommandName="DeleteComment" CommandArgument='<%# CType(Container.DataItem, CategoryComment).ID %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ButtonAddField" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <br />
            <asp:Label ID="LabelName" runat="server"></asp:Label>
            <asp:TextBox ID="TextBoxAddNewCategoryCommentName" runat="server" Width="100%"></asp:TextBox>
            <br />
            <asp:Label ID="LabelNewCategoryComment" runat="server"></asp:Label>
            <asp:TextBox ID="TextBoxAddNewCategoryComment" runat="server" TextMode="MultiLine" Rows="3" Width="100%"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="ButtonAddField" runat="server" Text="Add comment" CssClass="ButtonAddSmall" />
        </div>
    </fieldset>
</div>
