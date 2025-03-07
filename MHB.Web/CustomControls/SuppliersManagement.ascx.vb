Imports MHB.BL
Imports AjaxControlToolkit

Public Class SuppliersManagement
    Inherits System.Web.UI.UserControl

    Dim _en As Environment = New Environment()

    Private ReadOnly DeleteCommand As String = "Delete"
    Private ReadOnly EditCommand As String = "Edit"
    Private ReadOnly CancelCommand As String = "Cancel"
    Private ReadOnly UpdateCommand As String = "Update"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.RebindSuppliersGrid()
        End If

    End Sub

    Protected Sub RebindSuppliersGrid()

        With GridViewSuppliers
            .DataSource = Me._en.Suppliers
            .DataBind()

            Me._en.TranslateGridViewControls(GridViewSuppliers)
        End With

    End Sub

    Protected Sub ButtonCreateNewSupplier_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCreateNewSupplier.Click

        If Me.CheckIfSupplierExists(TextBoxNewSupplierName.Text) Then
            Return
        End If

        Dim newSupplier As Supplier = New Supplier()

        With newSupplier

            .AccountNumber = Supplier.GetSupplierID()
            .Name = TextBoxNewSupplierName.Text
            .Description = TextBoxNewSupplierDescription.Text
            .Address = TextBoxNewSupplierAddress.Text
            .PreferredVendorStatus = CheckBoxNewSupplierPreffered.Checked
            .ActiveFlag = CheckBoxNewSupplierActive.Checked
            .WebSiteURL = TextBoxNewSupplierWebsite.Text
            .UserID = Me._en.UserID
        End With

        Me._en.ExpenseManager.AddSupplier(newSupplier)

        Me.RebindSuppliersGrid()

    End Sub

    Protected Sub GridViewSuppliers_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewSuppliers.RowCommand

        Dim supplierID As Integer = 0

        If IsNumeric(e.CommandArgument) Then
            supplierID = CInt(e.CommandArgument)
        End If

        Select Case e.CommandName

            Case DeleteCommand

                Me._en.ExpenseManager.DeleteSupplier(supplierID)

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.DeleteSupplier, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case CancelCommand

                GridViewSuppliers.EditIndex = -1

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.CancelEditSupplier, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case UpdateCommand

                Dim supplier As Supplier = Me._en.Suppliers.FirstOrDefault(Function(s) s.ID = supplierID)

                With supplier

                    .ID = supplierID
                    .Name = CType(GridViewSuppliers.Rows(GridViewSuppliers.EditIndex).FindControl("TextBoxEditSupplierName"), TextBox).Text
                    .Description = CType(GridViewSuppliers.Rows(GridViewSuppliers.EditIndex).FindControl("TextBoxEditSupplierDescription"), TextBox).Text
                    .Address = CType(GridViewSuppliers.Rows(GridViewSuppliers.EditIndex).FindControl("TextBoxEditSupplierAddress"), TextBox).Text
                    .PreferredVendorStatus = CType(GridViewSuppliers.Rows(GridViewSuppliers.EditIndex).FindControl("CheckBoxEditSupplierPreferredVendorStatus"), CheckBox).Checked
                    .ActiveFlag = CType(GridViewSuppliers.Rows(GridViewSuppliers.EditIndex).FindControl("CheckBoxEditSupplierActiveFlag"), CheckBox).Checked
                    .WebSiteURL = CType(GridViewSuppliers.Rows(GridViewSuppliers.EditIndex).FindControl("TextBoxEditSupplierWebsiteURL"), TextBox).Text
                    .UserID = Me._en.UserID
                    .Connection = New SqlClient.SqlConnection(Me._en.GetConnectionString)

                End With

                Me._en.ExpenseManager.UpdateSupplier(supplier)

                GridViewSuppliers.EditIndex = -1

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.UpdateSupplier, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case EditCommand

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.EditSupplier, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)

                Exit Select

        End Select

        Me.RebindSuppliersGrid()

    End Sub

    Protected Sub GridViewSuppliers_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridViewSuppliers.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim hoverMenuItemTempate As HoverMenuExtender = CType(e.Row.FindControl("HoverMenuExtenderItemTemplate"), HoverMenuExtender)

            If hoverMenuItemTempate IsNot Nothing Then
                With hoverMenuItemTempate
                    e.Row.ID = e.Row.RowIndex.ToString()
                    .TargetControlID = e.Row.ID
                End With
            End If
        End If

    End Sub

    Protected Sub GridViewSuppliers_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridViewSuppliers.RowEditing
        GridViewSuppliers.EditIndex = e.NewEditIndex
        Me.RebindSuppliersGrid()
    End Sub

    Private Function CheckIfSupplierExists(ByVal supplierName As String) As Boolean

        Return Me._en.Suppliers.Any(Function(s) s.Name.ToUpper().Equals(supplierName.ToUpper()))

    End Function

End Class