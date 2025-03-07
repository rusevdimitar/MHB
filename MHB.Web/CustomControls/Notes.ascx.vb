Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports MHB.BL
Imports MHB.DAL

Partial Public Class Notes
    Inherits System.Web.UI.UserControl

    Private Const COMMAND_NAME_SHARE_NOTE As String = "ShareNote"

    Dim _en As Environment = New Environment()

    Public Property NoteID As Integer
        Get
            If Not IsNumeric(Session("NoteID")) Then
                Session("NoteID") = -1
            End If

            Return Session("NoteID")
        End Get
        Set(value As Integer)
            Session("NoteID") = value
        End Set
    End Property

    Public ReadOnly Property IsInSharedMode As Boolean
        Get
            Return Path.GetFileNameWithoutExtension(Me.Request.Path) = "SharedNotes"
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then

            If Request.QueryString("NoteID") IsNot Nothing AndAlso IsNumeric(Request.QueryString("NoteID")) Then
                Me.NoteID = CInt(Request.QueryString("NoteID"))
            End If

            Me.BindFormView()

        End If

        If Me.IsPostBack Then

            Environment.ExecuteScript(Me.FormView1, "$('note').readmore({ maxHeight: 50 });$('#NotesPreview').accordion({ collapsible: true });", True)

            Environment.ExecuteScript(Me.notesDatePicker, String.Format("{0}();", notesDatePicker.ClientID), True)

            If Request.Form("__EVENTTARGET") = DataGridNotesPreviews.ClientID Then

                If IsDate(Request.Form("__EVENTARGUMENT")) Then

                    Dim noteDate As Date = CDate(Request.Form("__EVENTARGUMENT"))

                    Me.BindFormView(noteDate)

                ElseIf Request.Form("__EVENTARGUMENT") = Environment.COMMAND_CLEAR_DEFAULT Then

                    notesDatePicker.Value = String.Empty

                    Me.BindFormView()

                End If

            End If

        End If

    End Sub

    Public Sub BindFormView(Optional ByVal noteDate As Date = Nothing)

        Dim qry As String = String.Empty

        If IsDate(notesDatePicker.Value) Then
            noteDate = CDate(notesDatePicker.Value)
        End If

        Try

            Dim table As DataTable = New DataTable()

            Dim parameters As SqlParameter()

            If Me.NoteID <> -1 Then
                qry = "SELECT ID, UserID, Note, DateCreated, IsShared FROM tbNotes WHERE ID = @noteID AND IsShared = 1"
                parameters = {New SqlParameter("noteID", Me.NoteID)}
            Else
                If noteDate <> Nothing Then
                    qry = "SELECT ID, UserID, Note, DateCreated, IsShared FROM tbNotes WHERE UserID = @userID AND CONVERT(DATE, DateCreated) = @noteDate ORDER BY DateCreated DESC"
                    parameters = {New SqlParameter("userID", Me._en.UserID), New SqlParameter("noteDate", noteDate)}
                Else
                    qry = "SELECT ID, UserID, Note, DateCreated, IsShared FROM tbNotes WHERE UserID = @userID ORDER BY DateCreated DESC"
                    parameters = {New SqlParameter("userID", Me._en.UserID)}
                End If
            End If

            table = DataBaseConnector.GetDataTable(qry, Me._en.GetConnectionString, parameters)

            'Dim datesWithNotes = table.AsEnumerable().Select(Function(r) r.Field(Of DateTime)("DateCreated")).ToArray()

            Dim datesWithNotes = table.Rows.Cast(Of DataRow).Where(Function(row) IsDate(row("DateCreated"))).Select(Function(row) CDate(row("DateCreated")).ToString("yyyy-M-d"))

            Me.HiddenDatesWithNotes.Value = String.Join(",", datesWithNotes)

            With FormView1
                .DataSource = table
                .DataBind()
            End With

            With DataGridNotesPreviews
                .PagerStyle.Mode = PagerMode.NumericPages
                .DataSource = table
                .DataBind()
            End With

        Catch ex As Exception
            Logging.Logger.Log(ex, "Notes.ascx.vb > BindFormView()", qry, Me._en.UserID, Me._en.GetConnectionString)
        End Try

    End Sub

    Protected Sub FormView1_ModeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles FormView1.ModeChanged

    End Sub

    Protected Sub FormView1_ModeChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewModeEventArgs) Handles FormView1.ModeChanging

        FormView1.ChangeMode(FormViewMode.Edit)

        Me.BindFormView()

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim qry As String = String.Empty

        Try

            Dim id As String = CType(FormView1.FindControl("btnUpdate"), Button).CommandArgument

            Dim text As String = CType(FormView1.FindControl("Editor1"), AjaxControlToolkit.HTMLEditor.Editor).Content

            qry = "UPDATE tbNotes SET Note = @text WHERE ID = @id"

            DataBaseConnector.ExecuteQuery(qry, Me._en.GetConnectionString, New SqlParameter("@text", text), New SqlParameter("@id", id))

            FormView1.ChangeMode(FormViewMode.ReadOnly)

            Me.BindFormView()

        Catch ex As Exception
            Logging.Logger.Log(ex, "Notes.ascx.vb > btnUpdate_Click()", qry, Me._en.UserID, Me._en.GetConnectionString)
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)

        FormView1.ChangeMode(FormViewMode.ReadOnly)

        Me.BindFormView()

    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs)

        If Me.IsInSharedMode Then
            Return
        End If

        Dim qry As String = String.Empty

        Try

            Dim text As String = CType(FormView1.FindControl("Editor2"), AjaxControlToolkit.HTMLEditor.Editor).Content

            qry = "INSERT INTO tbNotes (UserID, Note, DateCreated) VALUES (@userID, @text, GETDATE())"

            DataBaseConnector.ExecuteQuery(qry, Me._en.GetConnectionString, New SqlParameter("userID", Me._en.UserID), New SqlParameter("text", text))

            FormView1.ChangeMode(FormViewMode.ReadOnly)

            Me.BindFormView()

        Catch ex As Exception
            Logging.Logger.Log(ex, "Notes.ascx.vb > btnInsert_Click()", qry, _en.UserID, _en.GetConnectionString)
        End Try

    End Sub

    Protected Sub btnInitInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInitInsert.Click
        FormView1.ChangeMode(FormViewMode.Insert)
        Me.BindFormView()
    End Sub

    Protected Sub FormView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewPageEventArgs) Handles FormView1.PageIndexChanging
        FormView1.PageIndex = e.NewPageIndex
        Me.BindFormView()
    End Sub

    Protected Sub btnCancelInsert_Click(ByVal sender As Object, ByVal e As EventArgs)
        FormView1.ChangeMode(FormViewMode.ReadOnly)
        Me.BindFormView()
    End Sub

    Protected Sub FormView1_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewCommandEventArgs) Handles FormView1.ItemCommand

        Dim qry As String = String.Empty

        If Me.IsInSharedMode Then
            Return
        End If

        If Not IsNumeric(e.CommandArgument) Then
            Return
        End If

        Dim noteID As Integer = CInt(e.CommandArgument)

        Try

            Select Case e.CommandName

                Case Environment.COMMAND_DELETE_DEFAULT

                    qry = "DELETE FROM tbNotes WHERE ID = @noteID"

                    DataBaseConnector.ExecuteQuery(qry, Me._en.GetConnectionString, New SqlParameter("noteID", noteID))

                    Exit Select

                Case Notes.COMMAND_NAME_SHARE_NOTE

                    Dim imageButtonShareNote As ImageButton = CType(e.CommandSource, ImageButton)

                    Dim isShared As Boolean = CBool(imageButtonShareNote.Attributes("IsShared"))

                    qry = "UPDATE tbNotes SET IsShared = @isShared WHERE ID = @noteID"

                    DataBaseConnector.ExecuteQuery(qry, Me._en.GetConnectionString, New SqlParameter("isShared", Not isShared), New SqlParameter("noteID", noteID))

                    Exit Select

            End Select

            Me.BindFormView()

        Catch ex As Exception
            Logging.Logger.Log(ex, "Notes.ascx.vb > FormView1_ItemCommand()", qry, Me._en.UserID, Me._en.GetConnectionString)
        End Try
    End Sub

    Protected Sub FormView1_ItemDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewDeleteEventArgs) Handles FormView1.ItemDeleting

    End Sub

    Protected Sub DataGridNotesPreviews_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs)
        DataGridNotesPreviews.CurrentPageIndex = e.NewPageIndex
        Me.BindFormView()
    End Sub

    Protected Sub FormView1_PageIndexChanged(sender As Object, e As EventArgs)

    End Sub
End Class