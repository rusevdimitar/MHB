<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Notes.ascx.vb" Inherits="MHB.Web.Notes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="HTMLEditor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<input id="HiddenDatesWithNotes" runat="server" value="" type="hidden" />

<asp:UpdatePanel ID="UpdatePanelNotes" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnInitInsert" runat="server" Text="Add" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FormView ID="FormView1" runat="server" Width="900px" AllowPaging="True" OnPageIndexChanged="FormView1_PageIndexChanged">
                        <ItemTemplate>
                            <table width="100%">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblNoteContent" runat="server" Text='<%# Eval("Note") %>'></asp:Label>

                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Edit"
                                            Text="Edit"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# Eval("ID") %>'
                                            CommandName="Delete" Text="Delete" OnClientClick="javascript:return confirm('Delete?')"></asp:LinkButton>
                                        <asp:Label ID="lblDateEdited" runat="server" Text='<%# Eval("DateCreated") %>' CssClass="PlainText"></asp:Label>
                                    </td>

                                </tr>
                                <tr>
                                    <td align="left" style="width:98%;">
                                        <asp:TextBox ID="TextBoxSharedNoteLink" runat="server" CssClass="PlainTextBox" Width="100%" ReadOnly="true" Visible='<%# Eval("IsShared") %>'
                                            Text='<%# String.Format("{0}?NoteID={1}", MHB.BL.URLRewriter.GetLink("SharedNotesFullLink"), Eval("ID")) %>'
                                            onclick="javascript:this.select();"></asp:TextBox>
                                    </td>
                                    <td align="right">                                        
                                        <asp:ImageButton
                                            ID="ImageButtonShareNote"
                                            runat="server"
                                            ToolTip="Share note"
                                            IsShared='<%# Eval("IsShared")%>'
                                            onmouseout='<%# IIf(Eval("IsShared"), "this.src=""../Images/sharethis-24.png""", "this.src=""../Images/sharethis-24_faded.png""")%>'
                                            ImageUrl='<%# IIf(Eval("IsShared"), "../Images/sharethis-24.png", "../Images/sharethis-24_faded.png")%>'
                                            CommandName="ShareNote"
                                            CommandArgument='<%# Eval("ID")%>' />
                                    </td>
                                </tr>
                            </table>


                        </ItemTemplate>
                        <EditItemTemplate>
                            <HTMLEditor:Editor ID="Editor1" runat="server" Height="400px" Width="900px" Content='<%# Eval("Note") %>'
                                AutoFocus="true" />
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CommandArgument='<%# Eval("ID") %>'
                                OnClick="btnUpdate_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <HTMLEditor:Editor ID="Editor2" runat="server" Height="400px" Width="900px" Content='<%# Eval("Note") %>'
                                AutoFocus="true" />
                            <asp:Button ID="btnInsert" runat="server" Text="Save" OnClick="btnInsert_Click" />
                            <asp:Button ID="btnCancelInsert" runat="server" Text="Cancel" OnClick="btnCancelInsert_Click" />
                        </InsertItemTemplate>
                        <PagerStyle CssClass="GridPager" />
                    </asp:FormView>
                </td>
            </tr>
            <%--<tr>
                <td>
                    <asp:TextBox ID="TextBoxSharedNoteLink" runat="server" CssClass="PlainTextBox" Width="100%"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td>
                    <div id="NotesPreview">
                        <h3>Notes preview</h3>
                        <div>
                            <p>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <p class="PlainTextBold">
                                            Date:&nbsp;<input type="text" id="notesDatePicker" runat="server">
                                            <a href="#" onclick="javascript:__doPostBack('<%= DataGridNotesPreviews.ClientID%>', 'Clear');">clear</a>
                                        </p>
                                        <script type="text/javascript" language="javascript">

                                            function <%= notesDatePicker.ClientID%>()
                                            {
                                                $("[id$=notesDatePicker]").datepicker(
                                                {
                                                    onSelect: function (date) {
                                                        __doPostBack('<%= DataGridNotesPreviews.ClientID%>', date);
                                                    },
                                                    dateFormat: 'yy-mm-dd',
                                                    beforeShowDay: function(date) {

                                                        var noteDates = $("[id$=HiddenDatesWithNotes]").val().split(",");

                                                        var m = date.getMonth(), d = date.getDate(), y = date.getFullYear();

                                                        var currentDate = y + '-' + (m + 1)  + '-' + d;

                                                        if($.inArray(currentDate, noteDates) != -1)
                                                        {
                                                            return [true, 'ui-state-default ui-state-highlight ui-state-active', ''];
                                                        }

                                                        return [true];
                                                    }
                                                });
                                                }

                                                <%= notesDatePicker.ClientID%>();
                                        </script>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="notesDatePicker" />
                                    </Triggers>
                                </asp:UpdatePanel>

                                <asp:DataGrid ID="DataGridNotesPreviews" runat="server" PageSize="5" OnPageIndexChanged="DataGridNotesPreviews_PageIndexChanged" Width="100%" AllowPaging="true" ShowHeader="false" AutoGenerateColumns="false" BackColor="White" BorderStyle="None" BorderWidth="0">
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <div class="PlainTextPale">
                                                    <%# Eval("DateCreated")%>
                                                </div>
                                                <note style="padding: 0 10%;">
                                                    <%# Eval("Note")%>
                                                </note>
                                                <br />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="DataGridNotesPreviewsItemStyle" />
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <AlternatingItemStyle BackColor="#f2f5f7" />
                                    <PagerStyle CssClass="GridPager" />
                                </asp:DataGrid>
                            </p>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
