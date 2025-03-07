Imports MHB.DAL

Partial Public Class CustomPage
    Inherits System.Web.UI.UserControl

    Dim en As Environment = New Environment()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ViewState("sum") = 0

        If ViewState("reloadControl") IsNot Nothing AndAlso ViewState("reloadControl") = True Then
            GetControls()
        End If

        If Not IsPostBack Then
            UpdateSumOfCalcFields(MainContent.Controls)
            BindFieldTypesDropDown()
            BindListControlsDropDown()
        End If

    End Sub

    Protected Enum ControlTypes
        ListBox = 1
        TextBox = 2
        DropDownList = 3
        CheckBox = 4
        DateTime = 5
        NumericTextBox = 6

    End Enum

    Public Sub GetControls()

        Dim qry As String = String.Empty

        Try

            MainContent.Controls.Clear()

            Dim mainTable As HtmlTable = New HtmlTable()
            mainTable.Attributes("class") = "CustomControlsHolderTable"

            qry = String.Format( _
"SELECT * FROM [dbo].[tbCustomPageControls] WHERE [UserID] = {0}", en.UserID)

            Dim reader As IDataReader = DataBaseConnector.GetDataReader(qry, en.GetConnectionString)

            While reader.Read()

                Dim controlTypeID As Integer = reader("ControlTypeID")

                Dim ctl As Control = New Control()
                Dim label As Label = New Label()
                Dim validator As RegularExpressionValidator = New RegularExpressionValidator()
                Dim chkDeleteSelected As CheckBox = New CheckBox()

                Select Case controlTypeID
                    Case ControlTypes.TextBox

                        ctl = New TextBox()

                        label = New Label()
                        label.ID = "Label" & reader("ControlID")
                        label.Text = reader("Label").ToString.Trim

                        chkDeleteSelected = New CheckBox()
                        chkDeleteSelected.ID = "chkDeleteSelected" & reader("ControlID")
                        chkDeleteSelected.Attributes("DistinctDeleteID") = reader("ID")

                        CType(ctl, TextBox).ID = reader("ControlID")
                        CType(ctl, TextBox).Text = reader("ControlValue").ToString().Trim()
                        CType(ctl, TextBox).Attributes("DistinctID") = reader("ID")
                        CType(ctl, TextBox).Attributes("Calculable") = reader("Calculable")
                        CType(ctl, TextBox).Width = 250

                        Exit Select
                    Case ControlTypes.CheckBox

                        ctl = New CheckBox()

                        label = New Label()
                        label.ID = "Label" & reader("ControlID")
                        label.Text = reader("Label").ToString.Trim

                        chkDeleteSelected = New CheckBox()
                        chkDeleteSelected.ID = "chkDeleteSelected" & reader("ControlID")
                        chkDeleteSelected.Attributes("DistinctDeleteID") = reader("ID")

                        CType(ctl, CheckBox).ID = reader("ControlID")
                        CType(ctl, CheckBox).Checked = reader("Selected").ToString().Trim()
                        CType(ctl, CheckBox).Text = reader("ControlValue").ToString().Trim()
                        CType(ctl, CheckBox).Attributes("DistinctID") = reader("ID")

                        Exit Select
                    Case ControlTypes.ListBox

                        ctl = New ListBox()

                        label = New Label()
                        label.ID = "Label" & reader("ControlID")
                        label.Text = reader("Label").ToString.Trim

                        chkDeleteSelected = New CheckBox()
                        chkDeleteSelected.ID = "chkDeleteSelected" & reader("ControlID")
                        chkDeleteSelected.Attributes("DistinctDeleteID") = reader("ID")

                        qry = String.Format( _
"SELECT * FROM [dbo].[tbCustomListItems] WHERE [ControlID] = {0}", reader("ID"))

                        Dim table As DataTable = DataBaseConnector.GetDataTable(qry, en.GetConnectionString)

                        CType(ctl, ListBox).DataValueField = "ListItemValue"
                        CType(ctl, ListBox).DataTextField = "ListItemText"
                        CType(ctl, ListBox).DataSource = table
                        CType(ctl, ListBox).DataBind()
                        CType(ctl, ListBox).Width = 250

                        CType(ctl, ListBox).ID = reader("ControlID")
                        CType(ctl, ListBox).Text = reader("ControlValue").ToString().Trim()
                        CType(ctl, ListBox).Attributes("DistinctID") = reader("ID")

                        For Each item As ListItem In CType(ctl, ListBox).Items
                            For Each row As DataRow In table.Rows
                                If Not IsDBNull(row("Selected")) Then
                                    If row("ListItemValue") = item.Value AndAlso CBool(row("Selected")) Then
                                        item.Selected = True
                                        Exit For
                                    End If
                                End If
                            Next
                            For Each row As DataRow In table.Rows
                                If row("ListItemValue") = item.Value Then
                                    item.Attributes("DistinctID") = row("ID")
                                    Exit For
                                End If
                            Next
                        Next

                        Exit Select
                    Case ControlTypes.DropDownList

                        label = New Label()
                        label.ID = "Label" & reader("ControlID")
                        label.Text = reader("Label").ToString.Trim

                        chkDeleteSelected = New CheckBox()
                        chkDeleteSelected.ID = "chkDeleteSelected" & reader("ControlID")
                        chkDeleteSelected.Attributes("DistinctDeleteID") = reader("ID")

                        ctl = New DropDownList()

                        qry = String.Format( _
"SELECT * FROM [dbo].[tbCustomListItems] WHERE [ControlID] = {0}", reader("ID"))

                        Dim table As DataTable = DataBaseConnector.GetDataTable(qry, en.GetConnectionString)

                        CType(ctl, DropDownList).DataValueField = "ListItemValue"
                        CType(ctl, DropDownList).DataTextField = "ListItemText"
                        CType(ctl, DropDownList).DataSource = table
                        CType(ctl, DropDownList).DataBind()
                        CType(ctl, DropDownList).Width = 250

                        CType(ctl, DropDownList).ID = reader("ControlID")
                        CType(ctl, DropDownList).Text = reader("ControlValue").ToString().Trim()
                        CType(ctl, DropDownList).Attributes("DistinctID") = reader("ID")

                        For Each item As ListItem In CType(ctl, DropDownList).Items
                            For Each row As DataRow In table.Rows
                                If Not IsDBNull(row("Selected")) Then
                                    If row("ListItemValue") = item.Value AndAlso CBool(row("Selected")) Then
                                        item.Selected = True
                                        Exit For
                                    End If
                                End If
                            Next
                            For Each row As DataRow In table.Rows
                                If row("ListItemValue") = item.Value Then
                                    item.Attributes("DistinctID") = row("ID")
                                    Exit For
                                End If
                            Next
                        Next

                        Exit Select

                    Case ControlTypes.DateTime

                        label = New Label()
                        label.ID = "Label" & reader("ControlID")
                        label.Text = reader("Label").ToString.Trim

                        chkDeleteSelected = New CheckBox()
                        chkDeleteSelected.ID = "chkDeleteSelected" & reader("ControlID")
                        chkDeleteSelected.Attributes("DistinctDeleteID") = reader("ID")

                        ctl = New TextBox()

                        CType(ctl, TextBox).ID = reader("ControlID")
                        CType(ctl, TextBox).Text = reader("ControlValue").ToString().Trim()
                        CType(ctl, TextBox).Attributes("DistinctID") = reader("ID")
                        CType(ctl, TextBox).Width = 250

                        Dim calendar As AjaxControlToolkit.CalendarExtender = New AjaxControlToolkit.CalendarExtender()
                        calendar.Animated = True
                        calendar.TargetControlID = CType(ctl, TextBox).ID

                        MainContent.Controls.Add(calendar)

                        Exit Select
                    Case ControlTypes.NumericTextBox

                        ctl = New TextBox()

                        label = New Label()
                        label.ID = "Label" & reader("ControlID")
                        label.Text = reader("Label").ToString.Trim

                        chkDeleteSelected = New CheckBox()
                        chkDeleteSelected.ID = "chkDeleteSelected" & reader("ControlID")
                        chkDeleteSelected.Attributes("DistinctDeleteID") = reader("ID")

                        CType(ctl, TextBox).ID = reader("ControlID")
                        CType(ctl, TextBox).Text = reader("ControlValue").ToString().Trim()
                        CType(ctl, TextBox).Attributes("DistinctID") = reader("ID")
                        CType(ctl, TextBox).Attributes("Calculable") = reader("Calculable")
                        CType(ctl, TextBox).Width = 250

                        validator = New RegularExpressionValidator()
                        validator.ID = "RegularExpressionValidator" & reader("ControlID")
                        validator.ControlToValidate = CType(ctl, TextBox).ID
                        validator.CssClass = "Validator"
                        validator.ErrorMessage = "Numeric values only!"
                        validator.ValidationExpression = "^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"

                        Exit Select

                End Select

                Dim tableRow As HtmlTableRow = New HtmlTableRow()
                Dim rowCell0 As HtmlTableCell = New HtmlTableCell()
                Dim rowCell1 As HtmlTableCell = New HtmlTableCell()
                Dim rowCell2 As HtmlTableCell = New HtmlTableCell()

                rowCell0.Controls.Add(label)
                rowCell0.VAlign = "top"
                rowCell2.VAlign = "top"
                rowCell1.Controls.Add(ctl)
                If validator.ControlToValidate.Length > 0 Then
                    rowCell1.Controls.Add(validator)
                End If

                rowCell2.Controls.Add(chkDeleteSelected)
                tableRow.Controls.Add(rowCell0)
                tableRow.Controls.Add(rowCell1)
                tableRow.Controls.Add(rowCell2)

                mainTable.Controls.Add(tableRow)

            End While

            MainContent.Controls.Add(mainTable)

        Catch ex As Exception

            Logging.Logger.Log(ex, "CustomPage.ascx.vb\GetControls", qry, en.UserID, en.GetConnectionString)
        End Try

    End Sub

    Protected Sub UpdateControlValues(ByVal container As ControlCollection)

        Dim qry As String = String.Empty

        Try

            Dim newValue As String = String.Empty
            Dim id As String = String.Empty

            For Each ctl As Control In container

                If ctl.HasControls Then
                    UpdateControlValues(ctl.Controls)
                End If

                Select Case ctl.GetType.Name

                    Case GetType(TextBox).Name

                        newValue = CType(ctl, TextBox).Text
                        id = CType(ctl, TextBox).Attributes("DistinctID")

                        qry &= String.Format( _
"UPDATE [dbo].[tbCustomPageControls] SET [ControlValue] = '{0}' WHERE [ID] = {1} ", newValue, id)

                        Exit Select
                    Case GetType(CheckBox).Name

                        newValue = CType(ctl, CheckBox).Checked
                        id = CType(ctl, CheckBox).Attributes("DistinctID")

                        qry &= String.Format( _
"UPDATE [dbo].[tbCustomPageControls] SET [Selected] = '{0}' WHERE [ID] = {1} ", newValue, id)

                        Exit Select
                    Case GetType(ListBox).Name

                        id = CType(ctl, ListBox).SelectedItem.Attributes("DistinctID")

                        qry &= String.Format( _
"UPDATE [dbo].[tbCustomListItems] SET [Selected] = 0 WHERE [ControlID] = {0} ", CType(ctl, ListBox).Attributes("DistinctID"))

                        qry &= String.Format( _
"UPDATE [dbo].[tbCustomListItems] SET [Selected] = 1 WHERE [ID] = {0} ", id)

                        Exit Select
                    Case (GetType(DropDownList).Name)

                        id = CType(ctl, DropDownList).SelectedItem.Attributes("DistinctID")

                        qry &= String.Format( _
"UPDATE [dbo].[tbCustomListItems] SET [Selected] = 0 WHERE [ControlID] = {0} ", CType(ctl, DropDownList).Attributes("DistinctID"))

                        qry &= String.Format( _
"UPDATE [dbo].[tbCustomListItems] SET [Selected] = 1 WHERE [ID] = {0} ", id)

                        Exit Select

                End Select

            Next

            DataBaseConnector.ExecuteQuery(qry, en.GetConnectionString)

        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomPage.ascx.vb\UpdateControlValues", qry, en.UserID, en.GetConnectionString)
        End Try

    End Sub

    Protected Sub UpdateSumOfCalcFields(ByVal container As ControlCollection)

        Try

            For Each ctl As Control In container

                If ctl.HasControls Then
                    UpdateSumOfCalcFields(ctl.Controls)
                End If

                If TypeOf (ctl) Is TextBox Then

                    If Boolean.TryParse(CType(ctl, TextBox).Attributes("Calculable"), 0) AndAlso CType(ctl, TextBox).Attributes("Calculable") = True Then

                        If IsNumeric(CType(ctl, TextBox).Text) Then
                            ViewState("sum") = CDbl(ViewState("sum")) + CDbl(CType(ctl, TextBox).Text)
                        End If

                    End If

                End If

            Next

            lblSumCalcFields.Text = ViewState("sum") & en.Currency

        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomPage.ascx.vb\UpdateSumOfCalcFields", String.Empty, en.UserID, en.GetConnectionString)
        End Try

    End Sub

    Protected Sub BindFieldTypesDropDown()

        Dim qry As String = String.Empty

        Try
            qry = _
"SELECT [ID], [ControlType], [ControlTypeText] FROM [dbo].[tbControlTypes]"

            Dim table As DataTable = DataBaseConnector.GetDataTable(qry, en.GetConnectionString)

            drpFieldType.DataTextField = "ControlTypeText"
            drpFieldType.DataValueField = "ID"

            drpFieldType.DataSource = table
            drpFieldType.DataBind()

        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomPage.ascx.vb\BindFieldTypesDropDown", qry, en.UserID, en.GetConnectionString)
        End Try

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        UpdateControlValues(MainContent.Controls)
        UpdateSumOfCalcFields(MainContent.Controls)
        GetControls()
    End Sub

    Protected Sub btnAddNewCustomField_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddNewCustomField.Click

        pnlEdit.Visible = True

    End Sub

    Protected Sub DeleteSelectedFields(ByVal container As ControlCollection)

        Dim qry As String = String.Empty

        Try

            For Each ctl As Control In container

                If ctl.HasControls Then
                    DeleteSelectedFields(ctl.Controls)
                End If

                Dim id As String = String.Empty

                If TypeOf ctl Is CheckBox Then
                    If CType(ctl, CheckBox).Attributes("DistinctDeleteID") IsNot Nothing AndAlso CType(ctl, CheckBox).Checked Then
                        id = CType(ctl, CheckBox).Attributes("DistinctDeleteID")
                        qry &= String.Format( _
"DELETE FROM [dbo].[tbCustomListItems] WHERE [ControlID] = {0}" & vbCrLf & _
"DELETE FROM [dbo].[tbCustomPageControls] WHERE [ID] = {0}" & vbCrLf, id)
                    End If
                End If

            Next

            DataBaseConnector.ExecuteQuery(qry, en.GetConnectionString)

            GetControls()

        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomPage.ascx.vb\DeleteSelectedFields", qry, en.UserID, en.GetConnectionString)
        End Try

    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        DeleteSelectedFields(MainContent.Controls)
        GetControls()
    End Sub

    Protected Sub BindListControlsDropDown()

        Dim qry As String = String.Empty

        Try

            qry = String.Format( _
"SELECT * FROM [dbo].[tbCustomPageControls] WHERE ([ControlTypeID] = 1 OR [ControlTypeID] = 3) AND [UserID] = {0}", en.UserID)

            drpListControls.DataTextField = "Label"
            drpListControls.DataValueField = "ID"

            drpListControls.DataSource = DataBaseConnector.GetDataTable(qry, en.GetConnectionString)
            drpListControls.DataBind()

            drpListControls.Items.Insert(0, "[Please select]")

        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomPage.ascx.vb\BindListControlsDropDown", qry, en.UserID, en.GetConnectionString)
        End Try

    End Sub

    Protected Sub BindListItemsGrid(ByVal controlID As Integer)

        Dim qry As String = String.Empty

        Try

            qry = String.Format( _
"SELECT [ID], [ControlID], [ListItemValue], [ListItemText], [Selected] FROM [dbo].[tbCustomListItems] WHERE [ControlID] = {0}", controlID)

            GridViewListItems.DataSource = DataBaseConnector.GetDataTable(qry, en.GetConnectionString)
            GridViewListItems.DataBind()

        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomPage.ascx.vb\BindListItemsGrid", qry, en.UserID, en.GetConnectionString)
        End Try

    End Sub

    Protected Sub drpListControls_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpListControls.SelectedIndexChanged
        If IsNumeric(drpListControls.SelectedValue) Then
            BindListItemsGrid(drpListControls.SelectedValue)
        End If
    End Sub

    Protected Sub btnAddNewListItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddNewListItem.Click
        Dim qry As String = String.Empty

        Try

            qry = String.Format( _
"INSERT INTO [dbo].[tbCustomListItems]([ControlID],[ListItemValue],[ListItemText],[Selected]) VALUES " & _
"({0},'{1}','{2}',0)", drpListControls.SelectedValue, txtListItemName.Text, txtListItemName.Text)

            DataBaseConnector.ExecuteQuery(qry, en.GetConnectionString)

            GetControls()

            If IsNumeric(drpListControls.SelectedValue) Then
                BindListItemsGrid(drpListControls.SelectedValue)
            End If
        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomPage.ascx.vb\btnAddNewField_Click", qry, en.UserID, en.GetConnectionString)
        End Try

    End Sub

    Protected Sub GridViewListItems_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewListItems.RowCommand

        Dim qry As String = String.Empty

        Try
            Select Case e.CommandName.ToUpper
                Case "UPDATE"

                    Dim newValue As String = CType(CType(e.CommandSource, LinkButton).NamingContainer.FindControl("txtListItemName"), TextBox).Text

                    qry = String.Format( _
    "UPDATE [dbo].[tbCustomListItems] SET [ListItemValue] = '{0}', [ListItemText] = '{1}' WHERE [ID] = {2}", newValue, newValue, e.CommandArgument)

                    Exit Select

                Case "DELETE"

                    qry = String.Format( _
    "DELETE FROM [dbo].[tbCustomListItems] WHERE [ID] = {0}", e.CommandArgument)

                    Exit Select

            End Select

            DataBaseConnector.ExecuteQuery(qry, en.GetConnectionString)

            GridViewListItems.EditIndex = -1
            If IsNumeric(drpListControls.SelectedValue) Then
                BindListItemsGrid(drpListControls.SelectedValue)
            End If

            GetControls()
        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomPage.ascx.vb\GridViewListItems_RowCommand", qry, en.UserID, en.GetConnectionString)
        End Try
    End Sub

    Protected Sub GridViewListItems_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridViewListItems.RowEditing
        GridViewListItems.EditIndex = e.NewEditIndex
        If IsNumeric(drpListControls.SelectedValue) Then
            BindListItemsGrid(drpListControls.SelectedValue)
        End If
    End Sub

    Protected Sub GridViewListItems_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridViewListItems.RowUpdating

    End Sub

    Protected Sub GridViewListItems_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridViewListItems.RowDeleting

    End Sub

    Protected Sub GridViewListItems_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridViewListItems.RowCancelingEdit

    End Sub

    Protected Sub btnAddNewField_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddNewField.Click
        Dim qry As String = String.Empty

        Try

            qry = String.Format( _
"INSERT INTO [dbo].[tbCustomPageControls] ([UserID], [ControlID], [ControlTypeID], [ControlValue], [Calculable], [Selected], [Label])" & _
"VALUES ({0},'{1}',{2},'',{3},0,'{4}')", _
en.UserID, Guid.NewGuid(), drpFieldType.SelectedValue, IIf(chkCalculable.Checked, 1, 0), txtNewFieldName.Text)
            DataBaseConnector.ExecuteQuery(qry, en.GetConnectionString)

            GetControls()
            BindListControlsDropDown()

        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomPage.ascx.vb\btnAddNewField_Click", qry, en.UserID, en.GetConnectionString)
        End Try

    End Sub

    Protected Sub drpFieldType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpFieldType.SelectedIndexChanged

        Select Case drpFieldType.SelectedValue

            Case 1, 3
                pnlEditListItems.Visible = True
                Exit Select
            Case 6
                chkCalculable.Enabled = True
                Exit Select
            Case Else
                pnlEditListItems.Visible = False
                chkCalculable.Enabled = False
                Exit Select

        End Select

        btnAddNewField.Text = "Add new " & drpFieldType.SelectedItem.Text
        chkCalculable.Checked = False
    End Sub

    Protected Sub btnLoadControls_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLoadControls.Click
        GetControls()
        ViewState("reloadControl") = True
    End Sub
End Class