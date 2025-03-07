Imports MHB.BL
Imports System.IO

Public Class CategoriesManagement
    Inherits System.Web.UI.UserControl

    Public _en As Environment = New Environment()
    Private ReadOnly DeleteCommand As String = "Delete"
    Private ReadOnly EditCommand As String = "Edit"
    Private ReadOnly CancelCommand As String = "Cancel"
    Private ReadOnly UpdateCommand As String = "Update"
    Private ReadOnly UseCommand As String = "Use"
    Private ReadOnly CommentsCommand As String = "Comments"
    Private ReadOnly SetShareCommand As String = "SetShare"
    Private ReadOnly CommentVoteDownCommand As String = "VoteDown"
    Private ReadOnly CommentVoteUpCommand As String = "VoteUp"
    Private ReadOnly CommentDeleteCommand As String = "DeleteComment"

    Public Property SelectedCategoryOwnerUserID() As Integer
        Get
            If Not IsNumeric(Session("SelectedCategoryOwnerUserID")) Then
                Session("SelectedCategoryOwnerUserID") = 0
            End If
            Return Session("SelectedCategoryOwnerUserID")
        End Get
        Set(ByVal value As Integer)
            Session("SelectedCategoryOwnerUserID") = value
        End Set
    End Property

    Public Property SelectedCategoryID() As Integer
        Get
            If Not IsNumeric(Session("SelectedCategoryID")) Then
                Session("SelectedCategoryID") = 0
            End If
            Return Session("SelectedCategoryID")
        End Get
        Set(ByVal value As Integer)
            Session("SelectedCategoryID") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.RebindCategoriesGrid()
            Me.RebindUsersCategoriesGrid()
        End If
    End Sub

    Protected Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonUserDefinedCategorySave.Click

        Try

            If Me._en.Categories.Any(Function(c) c.Name.ToLower() = TextBoxCategoryName.Text.ToLower()) Then
                Environment.DisplayWebPageMessage(sender, String.Format("Category with name {0} already exists", TextBoxCategoryName.Text))
                Return
            End If

            Dim physicalFilePath As String = String.Empty
            Dim relativeFilePath As String = String.Empty

            If FileUploadIcon.HasFile Then
                Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(FileUploadIcon.PostedFile.InputStream)

                If image.Width > 22 OrElse image.Height > 22 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "invalidicondimensions", "alert('Maximum allowable icon dimensions - width: 22px, height: 22px');", True)
                    Return
                End If

                Dim iconsPath = Server.MapPath(String.Format("~\Images\UserCategoryIcons\{0}\", _en.UserID))

                If Not Directory.Exists(iconsPath) Then
                    Directory.CreateDirectory(iconsPath)
                End If

                physicalFilePath = String.Format("{0}{1}", iconsPath, FileUploadIcon.FileName)

                FileUploadIcon.PostedFile.SaveAs(physicalFilePath)

                relativeFilePath = String.Format("../Images/UserCategoryIcons/{0}/{1}", _en.UserID, Path.GetFileName(physicalFilePath))

            Else
                relativeFilePath = LabelSelectedIconFileName.Text
            End If

            Dim category As Category = New Category()

            With category
                .Name = TextBoxCategoryName.Text
                .UserID = _en.UserID
                .CategoryKeywords = TextBoxCategoryKeywords.Text
                .IconPath = relativeFilePath
                .IsPayIconVisible = CheckBoxIsPayIconVisible.Checked
                .IsShared = CheckBoxIsShared.Checked
                .ConnectionString = _en.GetConnectionString
                .LanguageID = _en.CurrentLanguage
            End With

            Me._en.ExpenseManager.AddCategory(category)

            Me.RebindCategoriesGrid()

            Me.RebindUsersCategoriesGrid()

            Me.ClearFields()

            CategoryIcons1.ReloadIcons()

            Logging.Logger.LogAction(Logging.Logger.HistoryAction.AddCategory, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

        Catch ex As Exception
            Logging.Logger.Log(ex, "CategoriesManagement.ButtonSave_Click", String.Empty, _en.UserID, _en.GetConnectionString)
        End Try

    End Sub

    Private Sub ClearFields()
        For Each ctl As Control In Me.Controls
            Select Case ctl.GetType().Name
                Case GetType(TextBox).Name
                    CType(ctl, TextBox).Text = String.Empty
                    Exit Select
                Case GetType(CheckBox).Name
                    CType(ctl, CheckBox).Checked = False
                    Exit Select
            End Select
        Next
    End Sub

    Private Sub RebindCommentsGrid(ByVal categoryID As Integer, ByVal ownerUserID As Integer)

        Dim category As Category = New Category(categoryID, _en.CurrentLanguage, ownerUserID, _en.GetConnectionString)

        With category
            .LoadComments()

            GridViewComments.DataSource = .Comments.Where(Function(comment) comment.IsDeleted = False).ToList()
            GridViewComments.DataBind()

            Me._en.TranslateGridViewControls(GridViewComments)

        End With

    End Sub

    Private Sub RebindCategoriesGrid()

        Try

            With GridViewCategories
                .DataSource = Me._en.Categories
                .DataBind()
            End With

        Catch ex As Exception
            Logging.Logger.Log(ex, "CategoriesManagement.RebindCategoriesGrid", String.Empty, _en.UserID, _en.GetConnectionString)
        Finally
            Me._en.TranslateGridViewControls(GridViewCategories)
        End Try

    End Sub

    Private Sub RebindUsersCategoriesGrid()

        Try

            With GridViewUsersCategories
                Dim dataSource As IEnumerable(Of Category) = Me._en.ExpenseManager.GetAllCategories().Where(Function(cat) cat.UserID <> Me._en.UserID AndAlso cat.IsShared = True AndAlso cat.UserID <> 0).ToList()
                .DataSource = dataSource
                .DataBind()
            End With

        Catch ex As Exception
            Logging.Logger.Log(ex, "CategoriesManagement.RebindUsersCategoriesGrid", String.Empty, _en.UserID, _en.GetConnectionString)
        Finally
            Me._en.TranslateGridViewControls(GridViewUsersCategories)
        End Try

    End Sub

    Protected Sub GridViewCategories_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewCategories.RowCommand, GridViewUsersCategories.RowCommand, GridViewComments.RowCommand

        Dim categoryID As Integer = 0

        If IsNumeric(e.CommandArgument) Then
            categoryID = CInt(e.CommandArgument)
        End If

        Select Case e.CommandName
            Case DeleteCommand

                Me._en.ExpenseManager.DeleteCategory(categoryID)

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.DeleteCategory, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case CancelCommand

                GridViewCategories.EditIndex = -1

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.CancelEditCategory, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case UpdateCommand

                Dim c As Category = New Category()

                With c
                    .ConnectionString = Me._en.GetConnectionString
                    .UserID = Me._en.UserID
                    .ID = categoryID
                    .Load()
                End With

                c.CategoryKeywords = CType(GridViewCategories.Rows(GridViewCategories.EditIndex).FindControl("TextBoxEditCategoryKeyWords"), TextBox).Text

                Me._en.ExpenseManager.UpdateCategory(c)

                GridViewCategories.EditIndex = -1

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.UpdateCategory, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case EditCommand

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.EditCategory, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case UseCommand

                Category.CopyCategory(_en.GetConnectionString, categoryID, _en.UserID)

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.CopyUserCategory, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case CommentsCommand

                Dim ownerUserID As Integer = CInt(CType(e.CommandSource, LinkButton).Attributes.Item("UserID"))

                Me.SelectedCategoryOwnerUserID = ownerUserID

                Me.SelectedCategoryID = categoryID

                Me.RebindCommentsGrid(categoryID, ownerUserID)

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.AddCategoryComment, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case SetShareCommand

                Dim c As Category = New Category()

                With c
                    .ConnectionString = _en.GetConnectionString
                    .UserID = _en.UserID
                    .ID = categoryID
                    .Load()
                End With

                c.SetShareFlag(Not c.IsShared)

                Exit Select

            Case CommentVoteDownCommand

                Dim comment As CategoryComment = New CategoryComment(e.CommandArgument, _en.GetConnectionString)

                If Not comment.UsersVoted.Any(Function(item) item = _en.UserID) Then
                    comment.VoteDown(_en.UserID)
                End If

                Me.RebindCommentsGrid(Me.SelectedCategoryID, Me.SelectedCategoryOwnerUserID)

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.VoteDownOnCategoryComment, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case CommentVoteUpCommand

                Dim comment As CategoryComment = New CategoryComment(e.CommandArgument, _en.GetConnectionString)

                If Not comment.UsersVoted.Any(Function(item) item = _en.UserID) Then
                    comment.VoteUp(_en.UserID)
                End If

                Me.RebindCommentsGrid(Me.SelectedCategoryID, Me.SelectedCategoryOwnerUserID)

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.VoteUpOnCategoryComment, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case CommentDeleteCommand

                Dim comment As CategoryComment = New CategoryComment(e.CommandArgument, _en.GetConnectionString)

                comment.Delete()

                Me.RebindCommentsGrid(Me.SelectedCategoryID, Me.SelectedCategoryOwnerUserID)

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.DeleteCategoryComment, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

        End Select

        Me._en.ExpenseManager.GetCategories()

        Me.RebindCategoriesGrid()

        Me.RebindUsersCategoriesGrid()

    End Sub

    Protected Sub GridViewCategories_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridViewCategories.RowDeleting

    End Sub

    Protected Sub ButtonClear_Click(sender As Object, e As EventArgs) Handles ButtonUserDefinedCategoryClear.Click
        Me.ClearFields()
    End Sub

    Protected Sub ButtonUserDefinedCategoryBack_Click(sender As Object, e As EventArgs) Handles ButtonUserDefinedCategoryBack.Click
        Response.Redirect(MHB.BL.URLRewriter.GetLink("MainForm"))
    End Sub

    Protected Sub ButtonAddNewCategoryComment_Click(sender As Object, e As EventArgs) Handles ButtonAddField.Click

        Try

            Dim comment As CategoryComment = New CategoryComment()

            With comment
                .UserID = _en.UserID
                .Comment = TextBoxAddNewCategoryComment.Text.Trim()
                .Poster = TextBoxAddNewCategoryCommentName.Text.Trim()
                .ConnectionString = _en.GetConnectionString
                .CategoryID = Me.SelectedCategoryID
                .Add()
            End With

            Me.RebindCommentsGrid(Me.SelectedCategoryID, Me.SelectedCategoryOwnerUserID)

            TextBoxAddNewCategoryComment.Text = String.Empty
            TextBoxAddNewCategoryCommentName.Text = String.Empty

        Catch ex As Exception
            Logging.Logger.Log(ex, "ButtonAddNewCategoryComment_Click", String.Empty, _en.UserID, _en.GetConnectionString)
        End Try

    End Sub

    Protected Sub GridViewCategories_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridViewCategories.RowEditing

        Try

            GridViewCategories.EditIndex = e.NewEditIndex
            Me.RebindCategoriesGrid()

            'Dim categoryID As Integer = CType(GridViewCategories.Rows(e.NewEditIndex).FindControl("LinkButtonUpdate"), LinkButton).CommandArgument

            'Dim c As Category = New Category(categoryID, _en.CurrentLanguage, _en.UserID, _en.GetConnectionString)

            'Dim TextBoxEditKeywords As TextBox = CType(GridViewCategories.Rows(e.NewEditIndex).FindControl("TextBoxEditCategoryKeyWords"), TextBox)

            'Dim keyWords As String = TextBoxEditKeywords.Text

            'Dim charsToTrim() As Char = {",", " "}

            'TextBoxEditKeywords.Text = keyWords.Substring(keyWords.IndexOf(c.Name.ToUpper()) + c.Name.Length).TrimStart(charsToTrim)

        Catch ex As Exception
            Logging.Logger.Log(ex, "GridViewCategories_RowEditing", String.Empty, _en.UserID, _en.GetConnectionString)
        End Try

    End Sub

    Protected Sub GridViewCategories_RowCancelingEdit(sender As Object, e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridViewCategories.RowCancelingEdit

    End Sub

    Protected Sub GridViewCategories_RowUpdating(sender As Object, e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridViewCategories.RowUpdating

    End Sub

    Protected Sub IconSelected(ByVal sender As Object, ByVal e As EventArgs) Handles CategoryIcons1.IconSelected

        If Not String.IsNullOrEmpty(sender) Then

            Dim iconPath As String = CStr(sender)

            trUploadIcon.Visible = False
            trSelectIcon.Visible = True

            LabelSelectedIconFileName.Text = iconPath
            ImageSelectedIcon.ImageUrl = iconPath

        End If

    End Sub

    Protected Sub LinkButtonUploadIcon_Click(sender As Object, e As EventArgs) Handles LinkButtonUploadIcon.Click
        trUploadIcon.Visible = True
        trSelectIcon.Visible = False
    End Sub

    Protected Sub GridViewCategories_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewCategories.RowDataBound

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim linkButtonSetShare As LinkButton = CType(e.Row.FindControl("LinkButtonSetShare"), LinkButton)

                Dim labelIsShared As Label = CType(e.Row.FindControl("LabelIsShared"), Label)

                If CBool(labelIsShared.Text) Then
                    linkButtonSetShare.Text = _en.GetTranslatedValue("RemoveShareCategory", _en.CurrentLanguage)
                Else
                    linkButtonSetShare.Text = _en.GetTranslatedValue("ShareCategory", _en.CurrentLanguage)
                End If

            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "GridViewCategories_RowDataBound", String.Empty, _en.UserID, _en.GetConnectionString)
        End Try
    End Sub
End Class