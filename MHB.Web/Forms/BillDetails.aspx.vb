Imports System.Data.SqlClient
Imports MHB.DAL

Partial Public Class BillDetails
    Inherits Environment

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Me.CheckAccess()

            RebindDetailsGrid(CInt(Request.QueryString("id")))

            DivModal.Visible = False
            DivModal.Attributes.Add("class", "ModalDiv")

            DivError.Visible = False
            DivAttach.Attributes.Add("class", "AttachDiv")

        End If

    End Sub

#Region "[ Sub: RebindDetailsGrid ]"

    Protected Sub RebindDetailsGrid(ByVal expenditureID As Integer)

        Dim qry As String = "SELECT * FROM [dbo].[" & DetailsTable & "] WHERE [ExpenditureID] = " & expenditureID
        Try
            Dim table As New DataTable()

            table = DataBaseConnector.GetDataTable(qry, GetConnectionString)

            Dim newRow As DataRow = table.NewRow()
            newRow.Item("DetailName") = GetTranslatedValue("enternewname", CurrentLanguage)
            newRow.Item("DetailDescription") = GetTranslatedValue("enternewdescription", CurrentLanguage)
            newRow.Item("DetailValue") = "0.0"
            table.Rows.Add(newRow)

            GridViewDetails.DataSource = table
            GridViewDetails.DataBind()

            ' we hide the columns with template fields, where visible=false, to get rid of nasty borders
            'GridViewDetails.Columns(1).Visible = False
            'GridViewDetails.Columns(2).Visible = False

            GridViewDetails.Rows(GridViewDetails.Rows.Count - 1).FindControl("CheckBoxDetailsTableSelect").Visible = False

            Dim detailsSum As Double = 0.0

            For i As Integer = 0 To GridViewDetails.Rows.Count - 1
                If IsNumeric(CType(GridViewDetails.Rows(i).FindControl("TextBoxDetailsTableFieldValue"), TextBox).Text) Then
                    detailsSum += CDbl(CType(GridViewDetails.Rows(i).FindControl("TextBoxDetailsTableFieldValue"), TextBox).Text)
                End If

            Next

            Dim TextBoxFieldName As TextBox = CType(GridViewDetails.Rows(GridViewDetails.Rows.Count - 1).FindControl("TextBoxDetailsTableFieldName"), TextBox)
            TextBoxFieldName.CssClass = "GridViewLastRowToEdit"
            TextBoxFieldName.Attributes.Add("onmouseover", "if(getElementById('" & TextBoxFieldName.ClientID & "').value == '" & GetTranslatedValue("enternewname", CurrentLanguage) & "'){getElementById('" & TextBoxFieldName.ClientID & "').value = ''; getElementById('" & TextBoxFieldName.ClientID & "').style.font = '12px arial,helvetica,sans-serif'}")

            Dim TextBoxFieldDescription As TextBox = CType(GridViewDetails.Rows(GridViewDetails.Rows.Count - 1).FindControl("TextBoxDetailsTableFieldDescription"), TextBox)
            TextBoxFieldDescription.CssClass = "GridViewLastRowToEdit"
            TextBoxFieldDescription.Attributes.Add("onmouseover", "if(getElementById('" & TextBoxFieldDescription.ClientID & "').value == '" & GetTranslatedValue("enternewdescription", CurrentLanguage) & "'){getElementById('" & TextBoxFieldDescription.ClientID & "').value = ''; getElementById('" & TextBoxFieldDescription.ClientID & "').style.font = '12px arial,helvetica,sans-serif'}")
            TextBoxFieldDescription.Attributes.Add("onfocus", "if(getElementById('" & TextBoxFieldDescription.ClientID & "').value == " & GetTranslatedValue("enternewdescription", CurrentLanguage) & "'){getElementById('" & TextBoxFieldDescription.ClientID & "').value = ''; getElementById('" & TextBoxFieldDescription.ClientID & "').style.font = '12px arial,helvetica,sans-serif'}")

            Dim TextBoxValue As TextBox = CType(GridViewDetails.Rows(GridViewDetails.Rows.Count - 1).FindControl("TextBoxDetailsTableFieldValue"), TextBox)
            TextBoxValue.CssClass = "GridViewLastRowToEdit"
            TextBoxValue.Attributes.Add("onmouseover", "if(getElementById('" & TextBoxValue.ClientID & "').value == '0.0'){getElementById('" & TextBoxValue.ClientID & "').value = ''; getElementById('" & TextBoxValue.ClientID & "').style.font = '12px arial,helvetica,sans-serif'}")
            TextBoxValue.Attributes.Add("onfocus", "if(getElementById('" & TextBoxValue.ClientID & "').value == '0.0'){getElementById('" & TextBoxValue.ClientID & "').value = ''; getElementById('" & TextBoxValue.ClientID & "').style.font = '12px arial,helvetica,sans-serif'}")

        Catch ex As Exception
            'DivError.Visible = True
            'DivError.InnerText = "RebindDetailsGrid(): " & ex.Message
            Logging.Logger.Log(ex, "RebindDetailsGrid()", qry, UserID, GetConnectionString)
        End Try

    End Sub

#End Region

    '============================================================================================
    '           GridViewDetails functional controls
    '============================================================================================

    ' ADD
#Region "[ Button.Click: ButtonAddExpenditureDetails_Click ]"

    Protected Sub ButtonAddExpenditureDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddExpenditureDetails.Click
        Dim qry As String = ""
        Try

            ' checks if there is a row selected to add details to
            'If GridView1.SelectedRow Is Nothing Then
            '    'ClientScript.RegisterStartupScript(Me.GetType(), "alert", "<script language=javascript>alert('No rows selected! Click the details link on a row you would like to edit.')</script>")
            '    ScriptManager.RegisterClientScriptBlock(Me, ButtonAddExpenditureDetails.GetType, "msg", "alert('No rows selected! Click the details link on a row you would like to edit.');", True)
            '    Return
            'End If

            qry = _
    "INSERT INTO [dbo].[" & DetailsTable & "] ([ExpenditureID], [DetailName] ,[DetailDescription] ,[DetailValue] ,[DetailDate]) VALUES " & _
    "( " & Request.QueryString("id") & _
    ",'" & CType(GridViewDetails.Rows(GridViewDetails.Rows.Count - 1).FindControl("TextBoxDetailsTableFieldName"), TextBox).Text.Replace("'", "") & "' " & _
    ",'" & CType(GridViewDetails.Rows(GridViewDetails.Rows.Count - 1).FindControl("TextBoxDetailsTableFieldDescription"), TextBox).Text.Replace("'", "") & "' " & _
    "," & IIf(IsNumeric(CType(GridViewDetails.Rows(GridViewDetails.Rows.Count - 1).FindControl("TextBoxDetailsTableFieldValue"), TextBox).Text.Replace(" ", "")), CType(GridViewDetails.Rows(GridViewDetails.Rows.Count - 1).FindControl("TextBoxDetailsTableFieldValue"), TextBox).Text.Replace(",", ".").Replace(" ", ""), "0") & _
    ",GETDATE())"

            DataBaseConnector.ExecuteQuery(qry, GetConnectionString)

            RebindDetailsGrid(CInt(Request.QueryString("id")))

            'ButtonUpdate_Click(sender, e)
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.AddDetails, UserID, GetConnectionString, Request.UserHostAddress)

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "ButtonAddExpenditureDetails_Click(): " & ex.Message
            Logging.Logger.Log(ex, "ButtonAddExpenditureDetails_Click()", qry, UserID, GetConnectionString)
        End Try
    End Sub

#End Region

    ' SAVE
#Region "[ Button.Click: ButtonUpdateDetailsTable_Click ]"
    Protected Sub ButtonUpdateDetailsTable_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonUpdateDetailsTable.Click
        Dim qry As String = ""
        Try

            ' checks if there is a row selected to add details to
            'If GridView1.SelectedRow Is Nothing Then
            '    'ClientScript.RegisterStartupScript(Me.GetType(), "alert", "<script language=javascript>alert('No rows selected! Click the details link on a row you would like to edit.')</script>")
            '    ScriptManager.RegisterClientScriptBlock(Me, ButtonUpdateDetailsTable.GetType, "msg", "alert('No rows selected! Click the details link on a row you would like to edit.');", True)
            '    Return
            If GridViewDetails.Rows.Count = 1 Then
                'ClientScript.RegisterStartupScript(Me.GetType(), "alert", "<script language=javascript>alert('Press the add button to add the new entry. There are yet no entries to update.')</script>")
                ScriptManager.RegisterClientScriptBlock(Me, ButtonUpdateDetailsTable.GetType, "msg", "alert('" & GetTranslatedValue("PressAddButtonToAddNewEntry", CurrentLanguage) & "');", True)
                Return
            End If

            For i As Integer = 0 To GridViewDetails.Rows.Count - 1

                qry &= _
" UPDATE [dbo].[" & DetailsTable & "] SET " & _
      "[DetailName] = '" & CType(GridViewDetails.Rows(i).FindControl("TextBoxDetailsTableFieldName"), TextBox).Text.Replace("'", "") & "'" & _
      ",[DetailDescription] = '" & CType(GridViewDetails.Rows(i).FindControl("TextBoxDetailsTableFieldDescription"), TextBox).Text.Replace("'", "") & "'" & _
      ",[DetailValue] = '" & CType(GridViewDetails.Rows(i).FindControl("TextBoxDetailsTableFieldValue"), TextBox).Text.Replace(",", ".").Replace(" ", "") & "'" & _
"WHERE ID = " & IIf(CType(GridViewDetails.Rows(i).FindControl("TextBoxDetailsTableID"), TextBox).Text.Equals(""), "0", CType(GridViewDetails.Rows(i).FindControl("TextBoxDetailsTableID"), TextBox).Text)

            Next

            DataBaseConnector.ExecuteQuery(qry, GetConnectionString)

            RebindDetailsGrid(CInt(Request.QueryString("id")))
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.UpdateDetails, UserID, GetConnectionString, Request.UserHostAddress)
            'ButtonUpdate_Click(sender, e)
        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "ButtonUpdateDetailsTable_Click(): " & ex.Message
            Logging.Logger.Log(ex, "ButtonUpdateDetailsTable_Click()", qry, UserID, GetConnectionString)
        End Try
    End Sub

#End Region

    ' DELETE
#Region "[ Button.Click: ButtonDeleteFromDetailsTable_Click ]"
    Protected Sub ButtonDeleteFromDetailsTable_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteFromDetailsTable.Click

        Dim qry As String = ""

        Try

            Dim hasSelectedRows As Boolean = False
            ' checks if there is a row selected to add details to
            For i As Integer = 0 To GridViewDetails.Rows.Count - 1
                If CType(GridViewDetails.Rows(i).FindControl("CheckBoxDetailsTableSelect"), CheckBox).Checked Then
                    hasSelectedRows = True
                    Exit For
                End If
            Next

            If Not hasSelectedRows Then
                'ClientScript.RegisterStartupScript(Me.GetType(), "alert", "<script language=javascript>alert('No rows selected! Mark a row you would like to delete.')</script>")
                ScriptManager.RegisterClientScriptBlock(Me, ButtonDeleteFromDetailsTable.GetType, "msg", "alert('" & GetTranslatedValue("DetailsMarkARowToDelete", CurrentLanguage) & "');", True)
                Return
            End If

            For i As Integer = 0 To GridViewDetails.Rows.Count - 1

                If CType(GridViewDetails.Rows(i).FindControl("CheckBoxDetailsTableSelect"), CheckBox).Checked Then
                    qry &= String.Format( _
" DELETE FROM [dbo].[{0}] WHERE ID = {1}", DetailsTable, CType(GridViewDetails.Rows(i).FindControl("TextBoxDetailsTableID"), TextBox).Text)

                End If

            Next

            DataBaseConnector.ExecuteQuery(qry, GetConnectionString)

            Logging.Logger.LogAction(Logging.Logger.HistoryAction.DeleteDetails, UserID, GetConnectionString, Request.UserHostAddress)
            RebindDetailsGrid(CInt(Request.QueryString("id")))

            'ButtonUpdate_Click(sender, e)
        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "ButtonDeleteFromDetailsTable_Click(): " & ex.Message
            Logging.Logger.Log(ex, "ButtonDeleteFromDetailsTable_Click()", qry, UserID, GetConnectionString)
        End Try
    End Sub

#End Region

    ' SHOW attach div for details table
#Region "[ Button.Click: ButtonDetailsTableAttach_Click ]"
    Protected Sub ButtonDetailsTableAttach_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDetailsTableAttach.Click

        Dim selectedRowsCount As Integer = 0
        For i As Integer = 0 To GridViewDetails.Rows.Count - 1
            If CType(GridViewDetails.Rows(i).FindControl("CheckBoxDetailsTableSelect"), CheckBox).Checked Then
                'DetailsTblEditIndex = CInt(CType(GridViewDetails.Rows(i).FindControl("TextBoxDetailsTableID"), TextBox).Text)
                selectedRowsCount = selectedRowsCount + 1
            End If
        Next

        If selectedRowsCount > 1 Then
            'ClientScript.RegisterStartupScript(Me.GetType(), "morethanonerowselected", "<script language='javascript'>alert('Please select only one row. Namely the bill you want to attach a document to.')</script>")
            ScriptManager.RegisterClientScriptBlock(Me, ButtonDetailsTableAttach.GetType, "msg", "alert('" & GetTranslatedValue("PleaseSelectOnlyOneRow", CurrentLanguage) & "');", True)
            DivModal.Visible = False
            DivAttach.Attributes("style") = "visibility: hidden;"
            Return
        ElseIf selectedRowsCount = 0 Then
            'ClientScript.RegisterStartupScript(Me.GetType(), "selectonerow", "<script language='javascript'>alert('Please select one row. Namely the bill you want to attach a document to.')</script>")
            ScriptManager.RegisterClientScriptBlock(Me, ButtonDetailsTableAttach.GetType, "msg", "alert('" & GetTranslatedValue("PleaseSelectRow", CurrentLanguage) & "');", True)
            DivModal.Visible = False
            DivAttach.Attributes("style") = "visibility: hidden;"
            Return
        Else
            DivModal.Visible = True
            'DivAttach.Visible = True
            'ScriptManager.RegisterClientScriptBlock(Me, ButtonAttach.GetType, "showAttachDiv", "<script language='javascript'>getElementById('" & DivAttach.ClientID & "').style.visibility = 'visible';</script>", True)
            DivAttach.Attributes("style") = "visibility: visible;"

        End If

        Logging.Logger.LogAction(Logging.Logger.HistoryAction.AttachToDetails, UserID, GetConnectionString, Request.UserHostAddress)

    End Sub

#End Region

    ' ATTACH
#Region "[ Button.Click: ButtonFileAttachConfirm_Click ]"
    Protected Sub ButtonFileAttachConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonFileAttachConfirm.Click

        Dim qry As String = ""
        Try

            'we check if the user has picked a file
            If Not FileUpload1.HasFile Then
                'ClientScript.RegisterStartupScript(Me.GetType(), "nofilepicked", "<script language='javascript'>alert('Please browse the file you would like to attach')</script>")
                ScriptManager.RegisterClientScriptBlock(Me, ButtonFileAttachConfirm.GetType, "msg", "alert('" & GetTranslatedValue("BrowseFileAlert", CurrentLanguage) & "');", True)
                DivModal.Visible = False
                DivAttach.Visible = False
                Return
            ElseIf FileUpload1.FileBytes.Length > AttachmentMaxSize Then
                ScriptManager.RegisterClientScriptBlock(Me, ButtonFileAttachConfirm.GetType, "msg", "alert('" & GetTranslatedValue("AttachFileSizeAlert", CurrentLanguage) & "');", True)
                DivModal.Visible = False
                DivAttach.Visible = False
                Return
            End If

            Using cn As New SqlConnection(GetConnectionString)

                Dim param As New SqlParameter("@attachment", SqlDbType.VarBinary)

                param.Value = FileUpload1.FileBytes

                qry = _
"UPDATE [dbo].[" & DetailsTable & "] SET [Attachment] = @attachment, [AttachmentFileType] = '" & IIf(FileUpload1.FileName.Length > 0, FileUpload1.FileName.Substring(FileUpload1.FileName.LastIndexOf(".")), "''") & "', [HasAttachment] = 1 WHERE ID = " & 0

                Using cmd As New SqlCommand(qry, cn)
                    cmd.Parameters.Add(param)
                    cn.Open()
                    cmd.ExecuteNonQuery()
                    cn.Close()
                End Using

            End Using

            'RebindGrid(Month)
            RebindDetailsGrid(CInt(Request.QueryString("id")))

            DivAttach.Attributes("style") = "visibility:hidden"
            DivModal.Visible = False

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "ButtonAttach_Click(): " & ex.Message
            Logging.Logger.Log(ex, "ButtonAttach_Click()", qry, UserID, GetConnectionString)
        End Try
    End Sub
#End Region

    ' DELETE ATTACHMENT
#Region "[ GridViewDetails_RowDeleting ]"
    Protected Sub GridViewDetails_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridViewDetails.RowDeleting
        Dim qry As String = ""
        Try

            qry = String.Format( _
    "UPDATE [dbo].[{0}] SET [HasAttachment] = 0 WHERE ID = {1}", DetailsTable, CType(GridViewDetails.Rows(e.RowIndex).FindControl("TextBoxDetailsTableID"), TextBox).Text)

            DataBaseConnector.ExecuteQuery(qry, GetConnectionString)

            RebindDetailsGrid(CInt(Request.QueryString("id")))

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "GridViewDetails_RowDeleting" & ex.Message
            Logging.Logger.Log(ex, "GridViewDetails_RowDeleting", qry, UserID, GetConnectionString)
        End Try
    End Sub

#End Region

    ' DOWNLOAD ATTACHMENT
#Region "[ GridViewDetails_RowEditing ]"

    Protected Sub GridViewDetails_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridViewDetails.RowEditing
        Try
            'AttachingToDetailsTable = True
            Response.Redirect("DownloadAttachment.aspx?id=" & CType(GridViewDetails.Rows(e.NewEditIndex).FindControl("TextBoxDetailsTableID"), TextBox).Text)
        Catch ex As Exception
            'DivError.Visible = True
            'DivError.InnerText = "GridViewDetails_RowEditing" & ex.Message
            Logging.Logger.Log(ex, "GridViewDetails_RowEditing", "none", UserID, GetConnectionString)
        End Try
    End Sub

#End Region

    'CLOSE attachment div
#Region "[ Button.Click: ButtonFileAttachCancelDialog_Click ]"

    Protected Sub ButtonFileAttachCancelDialog_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonFileAttachCancelDialog.Click
        DivModal.Visible = False
        DivAttach.Attributes("style") = "visibility:hidden"
    End Sub

#End Region

    '==================================================================
    '           GridViewDetails EventHandlers
    '==================================================================

#Region "[ GridViewDetails_RowDataBound ]"

    Protected Sub GridViewDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewDetails.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                ' we check if the row has an attachment
                If CType(e.Row.FindControl("CheckBoxDetTblHasAttachment"), CheckBox).Checked Then
                    CType(e.Row.FindControl("ImageDetTblHasAttachment"), Image).Visible = True
                    CType(e.Row.FindControl("ImageDetTblDeleteAttachment"), Image).Visible = True
                End If
            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "GridViewDetails_RowDataBound", "none", UserID, GetConnectionString)
        End Try

    End Sub

#End Region

    Protected Sub ButtonCloseAndSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCloseAndSave.Click
        ButtonUpdateDetailsTable_Click(sender, e)
        ExpenditureID = CInt(Request.QueryString("id"))
        Response.Write("<script type='text/javascript'>window.opener.document.getElementById('ctl00_ContentPlaceHolder1_HiddenTextBoxRefreshGrid').value=1;window.opener.document.getElementById('ctl00_ContentPlaceHolder1_HiddenTextBoxSelectedRow').value=" & Request.QueryString("id") & ";window.opener.__doPostBack();window.close();</script>")

    End Sub
End Class