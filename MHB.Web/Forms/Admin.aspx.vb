Imports System.Data.SqlClient
Imports MHB.Logging
Imports MHB.DAL

Partial Public Class Admin
    Inherits Environment

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If UserID <> 1 Then
            Response.Redirect(MHB.BL.URLRewriter.GetLink("Login"))
        End If

        If Not IsPostBack Then
            'BindGrid(False)
            'BindLogGrid(Today)
            'BindCurrentLanguageGrid()
            'BindCostCategoriesGrid()

            Using cn As New SqlConnection(GetConnectionString)
                Using adapter As New SqlDataAdapter("SELECT CAST([userID] AS CHAR(4))+' - '+[email] AS login, [userID] FROM [dbo].[tbUsers] UNION SELECT '- All users' AS login,0 AS userID", cn)
                    Dim table As New DataTable()
                    adapter.Fill(table)
                    DropDownListUsers.DataSource = table
                    DropDownListUsers.DataTextField = "login"
                    DropDownListUsers.DataValueField = "userID"
                    DropDownListUsers.DataBind()
                    table.Dispose()
                End Using
            End Using
        End If

    End Sub

    Protected Sub BindActionLog()

        Try

            Dim actionLog As ActionLog = New ActionLog(GetConnectionString)

            Dim logs As List(Of ActionLog) = actionLog.LoadAll(CalendarActionLogStartDate.SelectedDate)

            With GridViewActionLog
                .DataSource = logs
                .DataBind()
            End With

        Catch ex As Exception
            LabelError.Text = ex.Message
        End Try

    End Sub

    Protected Sub BindUniqueVisitors()

        Try

            Dim startDate = String.Empty

            With CalendarActionLogStartDate.SelectedDate
                startDate = String.Format("{0}-{1}-{2}", .Year, .Month, .Day)
            End With

            Dim dataSource As DataTable = DataBaseConnector.GetDataTable(String.Format("EXECUTE spGetUniqueVisitors '{0}'", startDate), GetConnectionString)

            GridViewUniqueVisitors.DataSource = dataSource
            GridViewUniqueVisitors.DataBind()

        Catch ex As Exception
            LabelError.Text = ex.Message
        End Try

    End Sub

    Protected Sub BindCostCategoriesGrid()
        Dim qry As String = ""
        Try
            qry = _
"SELECT * FROM [dbo].[tbCostCategories]"

            Using cn As New SqlConnection(GetConnectionString)
                Using cmd As New SqlCommand(qry, cn)
                    Using adapter As New SqlDataAdapter(cmd)
                        Dim table As DataTable = New DataTable()
                        adapter.Fill(table)
                        GridViewCostCategories.DataSource = table
                        GridViewCostCategories.DataBind()
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LabelError.Text = ex.Message & qry
        End Try
    End Sub

    Protected Sub BindGrid(ByVal showPassword As Boolean)
        Try

            Dim table As New DataTable
            Dim qry As String = ""

            If showPassword Then
                qry = _
"SELECT CAST([password] AS VARCHAR(50)) AS password,* FROM [dbo].[tbUsers]"
            Else
                qry = "SELECT * FROM [dbo].[tbUsers]"
            End If

            Using cn As New SqlConnection(GetConnectionString)
                Using adapter As New SqlDataAdapter(qry, cn)
                    adapter.Fill(table)
                End Using
            End Using

            GridViewUsers.DataSource = table
            GridViewUsers.DataBind()

        Catch ex As Exception
            LabelError.Text = "BindGrid: " & ex.Message
        End Try
    End Sub

    Protected Sub BindLogGrid(ByVal logDate As DateTime)
        Try

            Dim table As New DataTable()

            Using cn As New SqlConnection(GetConnectionString)
                Using adapter As New SqlDataAdapter("SELECT * FROM [dbo].[tbLog] WHERE logDateTime >= '" & logDate.ToString() & "'", cn)
                    adapter.Fill(table)
                End Using
            End Using

            GridViewLog.DataSource = table
            GridViewLog.DataBind()
        Catch ex As Exception
            LabelError.Text = "BindLogGrid: " & ex.Message
        End Try
    End Sub

    Protected Sub BindCurrentLanguageGrid()
        Try

            Dim table As New DataTable()

            Using cn As New SqlConnection(GetConnectionString)
                Using adapter As New SqlDataAdapter("SELECT * FROM [dbo].[tbCurrentLanguage]", cn)
                    adapter.Fill(table)
                End Using
            End Using

            GridViewCurrentLanguage.DataSource = table
            GridViewCurrentLanguage.DataBind()
        Catch ex As Exception
            LabelError.Text = "BindCurrentLanguageGrid: " & ex.Message
        End Try
    End Sub

    Protected Sub BindMainTableGrid(ByVal mainTableName As String, ByVal idRange As Integer, ByVal userID As Integer, ByVal hasAttachment As Integer)
        Dim qry As String = ""
        Try

            Dim hasAttach As String = ""

            If hasAttachment = 1 Then
                hasAttach = "HasAttachment = 1 AND "
            ElseIf hasAttachment = 0 Then
                hasAttach = "(HasAttachment = 0 OR HasAttachment IS NULL) AND "
            Else
                hasAttach = ""
            End If

            If userID = 0 Then
                qry = "SELECT * FROM [dbo].[" & mainTableName & "] WHERE " & hasAttach & " ID BETWEEN " & (idRange - 100).ToString() & " AND " & idRange.ToString()
            End If

            If idRange = 0 Then
                qry = "SELECT * FROM [dbo].[" & mainTableName & "] WHERE " & hasAttach & " UserID = " & userID.ToString()
            End If

            If userID = 0 AndAlso idRange = 0 Then
                qry = "SELECT * FROM [dbo].[" & mainTableName & "] WHERE " & hasAttach.Replace("AND", "")
                If hasAttachment = -1 Then
                    qry = qry.Replace("WHERE", "")
                End If
            End If

            If idRange <> 0 AndAlso userID <> 0 Then
                qry = "SELECT * FROM [dbo].[" & mainTableName & "] WHERE " & hasAttach & " UserID = " & userID.ToString() & " AND ID BETWEEN " & (idRange - 100).ToString() & " AND " & idRange.ToString()
            End If
            Dim table As DataTable = New DataTable()

            Using cn As New SqlConnection(GetConnectionString)
                Using adapter As New SqlDataAdapter(qry, cn)
                    adapter.Fill(table)
                End Using
            End Using

            GridViewMainTable.DataSource = table
            GridViewMainTable.DataBind()

        Catch ex As Exception
            LabelError.Text = "BindMainTableGrid: " & ex.Message & qry
        End Try
    End Sub

    Protected Sub ImageButtonPickRecurrenDate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonPickRecurrenDate.Click
        If CalendarExceptions.Visible Then
            CalendarExceptions.Visible = False
        Else
            CalendarExceptions.Visible = True
        End If
    End Sub

    Protected Sub CalendarExceptions_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CalendarExceptions.SelectionChanged
        BindLogGrid(CalendarExceptions.SelectedDate)
        CalendarExceptions.Visible = False
        LabelLogDate.Text = "All exceptions from: " & CalendarExceptions.SelectedDate.ToShortDateString() & " to: " & Today.ToShortDateString()
    End Sub

    Protected Sub ButtonShowPassword_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonShowPassword.Click
        BindGrid(True)
    End Sub

    Protected Sub ButtonHidePassword_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonHidePassword.Click
        BindGrid(False)
    End Sub

    Protected Sub ButtonSaveUserChanges_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveUserChanges.Click
        Dim qry As String = ""
        Try

            For i As Integer = 0 To GridViewUsers.Rows.Count - 1
                If CType(GridViewUsers.Rows(i).FindControl("CheckBoxUsersTableSelect"), CheckBox).Checked Then
                    qry &= _
                    " UPDATE [dbo].[tbUsers] SET " & _
                          "[userID] = " & CType(GridViewUsers.Rows(i).FindControl("TextBoxUserID"), TextBox).Text & " " & _
                          ",[email] = '" & CType(GridViewUsers.Rows(i).FindControl("TextBoxUserEmail"), TextBox).Text & "' " & _
                          ",[currency] = '" & CType(GridViewUsers.Rows(i).FindControl("TextBoxUserCurrency"), TextBox).Text & "' " & _
                          ",[language] = '" & CType(GridViewUsers.Rows(i).FindControl("DropDownListLang"), DropDownList).SelectedValue & "' " & _
                          ",[hassetlang] = '" & CType(GridViewUsers.Rows(i).FindControl("CheckBoxUserHasSetLang"), CheckBox).Checked.ToString() & "'" & _
                          ",[registrationdate] = '" & CType(GridViewUsers.Rows(i).FindControl("TextBoxUserRegistrationDate"), TextBox).Text & "'" & _
                          ",[attachmentsize] = " & IIf(CType(GridViewUsers.Rows(i).FindControl("TextBoxAttachmentSize"), TextBox).Text.Trim().Length = 0, "500000", CType(GridViewUsers.Rows(i).FindControl("TextBoxAttachmentSize"), TextBox).Text) & _
                    " WHERE [UserID] = " & CType(GridViewUsers.Rows(i).FindControl("TextBoxUserID"), TextBox).Text
                End If

            Next
            Using cn As New SqlConnection(GetConnectionString)
                Using cmd As New SqlCommand(qry, cn)
                    cn.Open()
                    cmd.ExecuteNonQuery()
                    cn.Close()
                End Using
            End Using

            BindGrid(False)

        Catch ex As Exception
            LabelError.Text = "ButtonSaveUserChanges_Click: " & ex.Message & "<br/>" & qry
        End Try
    End Sub

    Protected Sub LinkButtonSelectAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonSelectAll.Click
        For i As Integer = 0 To GridViewUsers.Rows.Count - 1
            If GridViewUsers.Rows(i).RowType = DataControlRowType.DataRow Then
                If Not CType(GridViewUsers.Rows(i).FindControl("CheckBoxUsersTableSelect"), CheckBox).Checked Then
                    CType(GridViewUsers.Rows(i).FindControl("CheckBoxUsersTableSelect"), CheckBox).Checked = True
                End If
            End If
        Next
    End Sub

    Protected Sub LinkButtonDeselectAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonDeselectAll.Click
        For i As Integer = 0 To GridViewUsers.Rows.Count - 1
            If GridViewUsers.Rows(i).RowType = DataControlRowType.DataRow Then
                If CType(GridViewUsers.Rows(i).FindControl("CheckBoxUsersTableSelect"), CheckBox).Checked Then
                    CType(GridViewUsers.Rows(i).FindControl("CheckBoxUsersTableSelect"), CheckBox).Checked = False
                End If
            End If
        Next
    End Sub

    Protected Sub ButtonDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDelete.Click
        Try

            Dim qry As String = ""

            For i As Integer = 0 To GridViewUsers.Rows.Count - 1
                If GridViewUsers.Rows(i).RowType = DataControlRowType.DataRow Then
                    If CType(GridViewUsers.Rows(i).FindControl("CheckBoxUsersTableSelect"), CheckBox).Checked Then
                        qry &= " DELETE FROM [dbo].[tbUsers] WHERE [userID] = " & CType(GridViewUsers.Rows(i).FindControl("TextBoxUserID"), TextBox).Text
                    End If
                End If
            Next

            If qry.Length > 0 Then
                Using cn As New SqlConnection(GetConnectionString)
                    Using cmd As New SqlCommand(qry, cn)
                        cn.Open()
                        cmd.ExecuteNonQuery()
                        cn.Close()
                    End Using
                End Using
            End If

            BindGrid(False)
        Catch ex As Exception
            LabelError.Text = "ButtonDelete_Click: " & ex.Message
        End Try
    End Sub

    Protected Sub LinkButtonMainPage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonMainPage.Click
        Response.Redirect(MHB.BL.URLRewriter.GetLink("MainForm"))
    End Sub

    Protected Sub GridViewUsers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewUsers.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Select Case CType(e.Row.FindControl("TextBoxUserCurrentLanguage"), TextBox).Text
                    Case "1"
                        CType(e.Row.FindControl("DropDownListLang"), DropDownList).SelectedIndex = 0
                        CType(e.Row.FindControl("ImageEn"), Image).Visible = True
                        Exit Select
                    Case "2"
                        CType(e.Row.FindControl("DropDownListLang"), DropDownList).SelectedIndex = 1
                        CType(e.Row.FindControl("ImageDe"), Image).Visible = True
                        Exit Select
                    Case "0"
                        CType(e.Row.FindControl("DropDownListLang"), DropDownList).SelectedIndex = 2
                        CType(e.Row.FindControl("ImageBg"), Image).Visible = True
                        Exit Select
                End Select

                If CType(e.Row.FindControl("CheckBoxUserHasSetLang"), CheckBox).Checked Then
                    CType(e.Row.FindControl("ImageTrue"), Image).Visible = True
                Else
                    CType(e.Row.FindControl("ImageFalse"), Image).Visible = True
                End If

            End If

        Catch ex As Exception
            LabelError.Text = "GridViewUsers_RowDataBound: " & ex.Message
        End Try
    End Sub

    Protected Sub LinkButtonMainTablesGridSelectAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonMainTablesGridSelectAll.Click
        For i As Integer = 0 To GridViewMainTable.Rows.Count - 1
            If GridViewMainTable.Rows(i).RowType = DataControlRowType.DataRow Then
                If Not CType(GridViewMainTable.Rows(i).FindControl("CheckBoxMainTableSelect"), CheckBox).Checked Then
                    CType(GridViewMainTable.Rows(i).FindControl("CheckBoxMainTableSelect"), CheckBox).Checked = True
                End If
            End If
        Next
    End Sub

    Protected Sub LinkButtonMainTablesGridDeselectAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonMainTablesGridDeselectAll.Click
        For i As Integer = 0 To GridViewMainTable.Rows.Count - 1
            If GridViewMainTable.Rows(i).RowType = DataControlRowType.DataRow Then
                If CType(GridViewMainTable.Rows(i).FindControl("CheckBoxMainTableSelect"), CheckBox).Checked Then
                    CType(GridViewMainTable.Rows(i).FindControl("CheckBoxMainTableSelect"), CheckBox).Checked = False
                End If
            End If
        Next
    End Sub

    Protected Sub ButtonMainTableSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonMainTableSave.Click
        Dim qry As String = ""
        Try

            For i As Integer = 0 To GridViewMainTable.Rows.Count - 1
                If CType(GridViewMainTable.Rows(i).FindControl("CheckBoxMainTableSelect"), CheckBox).Checked Then
                    qry &= _
                    "  UPDATE [dbo].[" & DropDownListMainTableIndex.SelectedValue & "] " & _
                    "  SET [UserID] = " & CType(GridViewMainTable.Rows(i).FindControl("TextBoxUserID"), TextBox).Text & _
                    "     ,[Month] =  " & CType(GridViewMainTable.Rows(i).FindControl("TextBoxMonth"), TextBox).Text & _
                    "     ,[Year] = " & CType(GridViewMainTable.Rows(i).FindControl("TextBoxYear"), TextBox).Text & _
                    "     ,[FieldName] = '" & CType(GridViewMainTable.Rows(i).FindControl("TextBoxFieldName"), TextBox).Text & "'" & _
                    "     ,[FieldDescription] = '" & CType(GridViewMainTable.Rows(i).FindControl("TextBoxFieldDescription"), TextBox).Text & "'" & _
                    "     ,[FieldValue] = " & CType(GridViewMainTable.Rows(i).FindControl("TextBoxFieldValue"), TextBox).Text & _
                    "     ,[DueDate] = '" & CType(GridViewMainTable.Rows(i).FindControl("TextBoxDueDate"), TextBox).Text & "'" & _
                    "     ,[DateRecordUpdated] = '" & CType(GridViewMainTable.Rows(i).FindControl("TextBoxDueDate"), TextBox).Text & "'" & _
                    "     ,[IsPaid] = '" & CType(GridViewMainTable.Rows(i).FindControl("CheckBoxIsPaid"), CheckBox).Checked.ToString() & "'" & _
                    "     ,[HasDetails] = '" & CType(GridViewMainTable.Rows(i).FindControl("CheckBoxHasDetails"), CheckBox).Checked.ToString() & "'" & _
                    "     ,[AttachmentFileType] = '" & CType(GridViewMainTable.Rows(i).FindControl("TextBoxAttachmentFileType"), TextBox).Text & "'" & _
                    "     ,[HasAttachment] = '" & CType(GridViewMainTable.Rows(i).FindControl("CheckBoxHasAttachment"), CheckBox).Checked.ToString() & "'" & _
                    "     ,[OrderID] = " & IIf(CType(GridViewMainTable.Rows(i).FindControl("TextBoxAttachment"), TextBox).Text.Trim().Length = 0, "NULL", CType(GridViewMainTable.Rows(i).FindControl("TextBoxAttachment"), TextBox).Text) & _
                    " WHERE ID = " & CType(GridViewMainTable.Rows(i).FindControl("TextBoxOrderID"), TextBox).Text
                End If
            Next
            If qry.Length > 0 Then
                Using cn As New SqlConnection(GetConnectionString)
                    Using cmd As New SqlCommand(qry, cn)
                        cn.Open()
                        cmd.ExecuteNonQuery()
                        cn.Close()
                    End Using
                End Using
            End If

            BindMainTableGrid(DropDownListMainTableIndex.SelectedValue, CInt(DropDownListIDRange.SelectedValue), CInt(DropDownListUsers.SelectedValue), CBool(DropDownListHasAttachment.SelectedValue))

        Catch ex As Exception
            LabelError.Text = "ButtonMainTableSave_Click: " & ex.Message & qry
        End Try
    End Sub

    Protected Sub DropDownListMainTableIndex_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListMainTableIndex.SelectedIndexChanged
        BindMainTableGrid(DropDownListMainTableIndex.SelectedValue, CInt(DropDownListIDRange.SelectedValue), CInt(DropDownListUsers.SelectedValue), CInt(DropDownListHasAttachment.SelectedValue))
    End Sub

    Protected Sub DropDownListIDRange_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListIDRange.SelectedIndexChanged
        BindMainTableGrid(DropDownListMainTableIndex.SelectedValue, CInt(DropDownListIDRange.SelectedValue), CInt(DropDownListUsers.SelectedValue), CInt(DropDownListHasAttachment.SelectedValue))
    End Sub

    Protected Sub DropDownListUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListUsers.SelectedIndexChanged
        BindMainTableGrid(DropDownListMainTableIndex.SelectedValue, CInt(DropDownListIDRange.SelectedValue), CInt(DropDownListUsers.SelectedValue), CInt(DropDownListHasAttachment.SelectedValue))
    End Sub

    Protected Sub DropDownListHasAttachment_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListHasAttachment.SelectedIndexChanged
        BindMainTableGrid(DropDownListMainTableIndex.SelectedValue, CInt(DropDownListIDRange.SelectedValue), CInt(DropDownListUsers.SelectedValue), CInt(DropDownListHasAttachment.SelectedValue))
    End Sub

    Protected Sub ButtonLangSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonLangSave.Click

        Try

            Dim qry As String = ""

            For i As Integer = 0 To GridViewCurrentLanguage.Rows.Count - 1
                If CType(GridViewCurrentLanguage.Rows(i).FindControl("CheckBoxCurrentLanguageTableSelect"), CheckBox).Checked Then
                    qry &= _
        " UPDATE [dbo].[tbCurrentLanguage] SET " & _
        "[ControlTextEN] = '" & CType(GridViewCurrentLanguage.Rows(i).FindControl("TextBoxControlTextEN"), TextBox).Text & "'" & _
        ",[ControlTextBG] = '" & CType(GridViewCurrentLanguage.Rows(i).FindControl("TextBoxControlTextBG"), TextBox).Text & "'" & _
        ",[ControlTextDE] = '" & CType(GridViewCurrentLanguage.Rows(i).FindControl("TextBoxControlTextDE"), TextBox).Text & "'" & _
        " WHERE [ControlID] = '" & CType(GridViewCurrentLanguage.Rows(i).FindControl("TextBoxControlID"), TextBox).Text & "'"
                End If
            Next

            Using cn As New SqlConnection(GetConnectionString)
                Using cmd As New SqlCommand(qry, cn)
                    cn.Open()
                    cmd.ExecuteNonQuery()
                    cn.Close()
                End Using
            End Using

            BindCurrentLanguageGrid()

        Catch ex As Exception
            LabelError.Text = "ButtonLangSave_Click: " & ex.Message
        End Try

    End Sub

    Protected Sub AddNewTranlation_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AddNewTranlation.Click
        Dim qry As String = _
"INSERT INTO [dbo].[tbCurrentLanguage] " & _
"VALUES ('" & TextBoxControlID.Text & "','" & TextBoxEN.Text & "','" & TextBoxBG.Text & "','" & TextBoxDE.Text & "') "
        Try
            Using cn As New SqlConnection(GetConnectionString)
                Using cmd As New SqlCommand(qry, cn)
                    cn.Open()
                    cmd.ExecuteNonQuery()
                    cn.Close()
                End Using
            End Using
        Catch ex As Exception
            LabelError.Text = "AddNewTranlation_Click(): " & qry & "<br/><br/><br/>" & ex.Message
        End Try
    End Sub

    Protected Sub ButtonSaveCostCategory_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveCostCategory.Click
        Dim qry As String = ""
        Try
            For i As Integer = 0 To GridViewCostCategories.Rows.Count - 1
                If CType(GridViewCostCategories.Rows(i).FindControl("CheckBoxCostcategoriesTableSelect"), CheckBox).Checked Then
                    qry &= _
" UPDATE [dbo].[tbCostCategories] SET [CostCategoryID] = " & CType(GridViewCostCategories.Rows(i).FindControl("TextBoxCostCategoryID"), TextBox).Text & ", [CostNames] = '" & CType(GridViewCostCategories.Rows(i).FindControl("TextBoxCostNames"), TextBox).Text & "' WHERE [ID] = " & CType(GridViewCostCategories.Rows(i).FindControl("TextBoxCostCategoriesID"), TextBox).Text
                End If
            Next

            Using cn As New SqlConnection(GetConnectionString)
                Using cmd As New SqlCommand(qry, cn)
                    cn.Open()
                    cmd.ExecuteNonQuery()
                    cn.Close()
                End Using
            End Using

            TextBoxCategoryID.Text = ""
            TextBoxKeyword.Text = ""
            BindCostCategoriesGrid()

        Catch ex As Exception
            LabelError.Text = ex.Message
        End Try
    End Sub

    Protected Sub ButtonInsertCostCategory_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonInsertCostCategory.Click
        Dim qry As String = ""
        Try

            qry = _
"INSERT INTO [dbo].[tbCostCategories] VALUES (" & TextBoxCategoryID.Text & ",'" & TextBoxKeyword.Text & "')"

            Using cn As New SqlConnection(GetConnectionString)
                Using cmd As New SqlCommand(qry, cn)
                    cn.Open()
                    cmd.ExecuteNonQuery()
                    cn.Close()
                End Using
            End Using

            TextBoxCategoryID.Text = ""
            TextBoxKeyword.Text = ""
            BindCostCategoriesGrid()

        Catch ex As Exception
            LabelError.Text = ex.Message
        End Try
    End Sub

    Protected Sub ImageButtonAdminUsers_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonAdminUsers.Click

        DivManageUsers.Attributes("style") = "position: absolute; top: 200px; visibility:visible;"
        DivExceptionsLog.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivMainTable.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivControlsTranslation.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivCostCategories.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivActionLog.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"

        BindGrid(False)

    End Sub

    Protected Sub ImageButtonAdminExceptionsLog_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonAdminExceptionsLog.Click
        DivManageUsers.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivExceptionsLog.Attributes("style") = "position: absolute; top: 200px; visibility:visible;"
        DivMainTable.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivControlsTranslation.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivCostCategories.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivActionLog.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"

        BindLogGrid(Today)
    End Sub

    Protected Sub ImageButtonAdminMainTable_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonAdminMainTable.Click
        DivManageUsers.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivExceptionsLog.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivMainTable.Attributes("style") = "position: absolute; top: 200px; visibility:visible;"
        DivControlsTranslation.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivCostCategories.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivActionLog.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"

    End Sub

    Protected Sub ImageButtonAdminControlsTranslation_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonAdminControlsTranslation.Click
        DivManageUsers.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivExceptionsLog.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivMainTable.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivControlsTranslation.Attributes("style") = "position: absolute; top: 200px; visibility:visible;"
        DivCostCategories.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivActionLog.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"

    End Sub

    Protected Sub ImageButtonAdminCostCategories_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonAdminCostCategories.Click
        DivManageUsers.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivExceptionsLog.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivMainTable.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivControlsTranslation.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivCostCategories.Attributes("style") = "position: absolute; top: 200px; visibility:visible;"
        DivActionLog.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"

    End Sub

    Protected Sub ImageButtonAdminActionLog_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonAdminActionLog.Click

        DivManageUsers.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivExceptionsLog.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivMainTable.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivControlsTranslation.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivCostCategories.Attributes("style") = "position: absolute; top: 200px; visibility:hidden;"
        DivActionLog.Attributes("style") = "position: absolute; top: 200px; visibility:visible;"

    End Sub

    Protected Sub ImageButtonAdminMassEmail_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonAdminMassEmail.Click
        Response.Redirect("MassEmail.aspx")
    End Sub

    Protected Sub CalendarActionLogStartDate_SelectionChanged(sender As Object, e As EventArgs) Handles CalendarActionLogStartDate.SelectionChanged
        BindActionLog()
        BindUniqueVisitors()
    End Sub
End Class