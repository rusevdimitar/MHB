Imports MHB.BL
Imports MHB.DAL
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports AjaxControlToolkit
Imports MHB.Web.Environment

Public Class ProductsManagement
    Inherits System.Web.UI.UserControl

    Protected _en As Environment = New Environment()

    Private ReadOnly DeleteCommand As String = "Delete"
    Private ReadOnly EditCommand As String = "Edit"
    Private ReadOnly CancelCommand As String = "Cancel"
    Private ReadOnly UpdateCommand As String = "Update"
    Private ReadOnly MergeCommand As String = "Merge"
    Private ReadOnly RemoveFromListCommand As String = "RemoveFromShoppingList"

    Public Property SelectedProductID As Integer
        Get
            If Session("SelectedProductID") Is Nothing Then
                Session("SelectedProductID") = Product.PRODUCT_DEFAULT_ID
            End If
            Return CInt(Session("SelectedProductID"))
        End Get
        Set(value As Integer)
            Session("SelectedProductID") = value
        End Set
    End Property

    Public Property Products() As List(Of Product)
        Get
            If Session("ProductsDataSource") Is Nothing Then
                Session("ProductsDataSource") = New List(Of Product)()
            End If
            Return CType(Session("ProductsDataSource"), List(Of Product))
        End Get
        Set(ByVal value As List(Of Product))
            Session("ProductsDataSource") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then

            Me.RebindProductsGrid(Function(p) p.UserID = Me._en.UserID)
            Me.RebindCategoriesDropDown()
            Me.RebindSuppliersDropDown()
            Me.ButtonAddToShoppingList.Text = Me._en.GetTranslatedValue("btnInitInsert", Me._en.CurrentLanguage)

            If IsNumeric(Request.QueryString("pid")) Then

                Dim productID As Integer = CInt(Request.QueryString("pid"))

                Me.RebindProductsGrid(Function(p) p.UserID = Me._en.UserID AndAlso p.ID = productID)

                Dim editedRow As GridViewRow = (From row As GridViewRow In GridViewProducts.Rows
                                                Where CInt(CType(row.FindControl("LabelProductID"), Label).Text) = productID
                                                Select row).FirstOrDefault()

                If editedRow IsNot Nothing Then

                    GridViewProducts.SetEditRow(editedRow.RowIndex)

                    'Environment.ExecuteScript(sender, String.Format("ScrollToEditedProduct({0});", productID), True)

                End If

            End If

        End If

        If Me.IsPostBack Then

            If Request.Form("__EVENTTARGET") = ButtonFilter.ClientID Then
                Me.ButtonFilter_Click(sender, EventArgs.Empty)
            End If

        End If

        Me._en.ApplyDropDownSkin(UpdatePanelFunctionalControls)

    End Sub

    Protected Sub RebindProductsGrid(Optional ByVal filter As Func(Of Product, Boolean) = Nothing)

        Try

            If filter IsNot Nothing Then

                Me.Products = Me._en.Products.Where(filter).ToList()

            End If

            With GridViewProducts

                .PageSize = IIf(DropDownListPageSize.SelectedValue = -1, Me.Products.Count, DropDownListPageSize.SelectedValue)
                .DataSource = Me.Products
                .DataBind()

                Me._en.TranslateGridViewControls(GridViewProducts)

            End With

        Catch ex As Exception
            Logging.Logger.Log(ex, "ProductsManagement.ascx.vb.RebindProductsGrid", String.Empty, Me._en.UserID, Me._en.GetConnectionString)
        End Try

    End Sub

    Protected Sub RebindCategoriesDropDown()

        With DropDownListFilterByCategory

            .DataTextField = "Name"
            .DataValueField = "ID"

            .DataSource = Me._en.Categories.OrderBy(Function(c) c.Name)
            .DataBind()

        End With

        Environment.InsertDropDownEmptyItem(DropDownListFilterByCategory, Product.PRODUCT_DEFAULT_ID)

    End Sub

    Protected Sub RebindSuppliersDropDown()

        With DropDownListFilterBySupplier

            .DataTextField = "Name"
            .DataValueField = "ID"

            .DataSource = Me._en.Suppliers.OrderBy(Function(s) s.Name)
            .DataBind()

        End With

        Environment.InsertDropDownEmptyItem(DropDownListFilterBySupplier, Supplier.SUPPLIER_DEFAULT_ID)

    End Sub

    Protected Sub DropDownListFilterByCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListFilterByCategory.SelectedIndexChanged

        Me.ButtonFilter_Click(sender, e)

    End Sub

    Protected Sub DropDownListFilterBySupplier_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListFilterBySupplier.SelectedIndexChanged

        Me.ButtonFilter_Click(sender, e)

    End Sub

    Protected Sub DropDownListPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListPageSize.SelectedIndexChanged

        Me.ButtonFilter_Click(sender, e)

    End Sub

    Protected Sub GridViewProducts_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewProducts.RowCommand

        Dim productID As Integer = 0

        If IsNumeric(e.CommandArgument) Then
            productID = CInt(e.CommandArgument)
        End If

        Select Case e.CommandName

            Case DeleteCommand

                Product.Delete(productID, Me._en.GetConnectionString, Me._en.UserID)

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.DeleteProduct, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case CancelCommand

                GridViewProducts.EditIndex = -1

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.CancelEditProduct, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case UpdateCommand

                GridViewProducts.Columns(Me._en.GetCellIndexByName(GridViewProducts.Rows(GridViewProducts.EditIndex), Me._en.GetTranslatedValue("DateModified", Me._en.CurrentLanguage))).Visible = True

                Dim name As String = CType(GridViewProducts.Rows(GridViewProducts.EditIndex).FindControl("TextBoxEditProductName"), TextBox).Text
                Dim description As String = CType(GridViewProducts.Rows(GridViewProducts.EditIndex).FindControl("TextBoxEditProductDescription"), TextBox).Text
                Dim keyWords As String() = CType(GridViewProducts.Rows(GridViewProducts.EditIndex).FindControl("TextBoxEditProductKeywords"), TextBox).Text.Split(",")
                Dim standardCost As String = CType(GridViewProducts.Rows(GridViewProducts.EditIndex).FindControl("TextBoxEditProductStandardCost"), TextBox).Text.Replace(",", ".")
                Dim listPrice As String = CType(GridViewProducts.Rows(GridViewProducts.EditIndex).FindControl("TextBoxEditProductListPrice"), TextBox).Text.Replace(",", ".")
                Dim color As String = CType(GridViewProducts.Rows(GridViewProducts.EditIndex).FindControl("TextBoxEditProductColor"), TextBox).Text
                Dim weight As String = CType(GridViewProducts.Rows(GridViewProducts.EditIndex).FindControl("TextBoxEditProductWeight"), TextBox).Text.Replace(",", ".")
                Dim volume As String = CType(GridViewProducts.Rows(GridViewProducts.EditIndex).FindControl("TextBoxEditProductVolume"), TextBox).Text.Replace(",", ".")
                Dim packageUnitsCount As String = CType(GridViewProducts.Rows(GridViewProducts.EditIndex).FindControl("TextBoxEditProductPackageUnitsCount"), TextBox).Text.Replace(",", ".")
                Dim suppplierID As String = CType(GridViewProducts.Rows(GridViewProducts.EditIndex).FindControl("DropDownListProductVendors"), DropDownList).SelectedValue
                Dim categoryID As String = CType(GridViewProducts.Rows(GridViewProducts.EditIndex).FindControl("DropDownListProductCategories"), DropDownList).SelectedValue

                Dim product As Product = Me.Products.FirstOrDefault(Function(p) p.ID = productID)

                With product
                    .ID = productID
                    .ConnectionString = Me._en.GetConnectionString
                    .UserID = Me._en.UserID
                    .Name = name
                    .Description = description
                    .KeyWords = keyWords
                    .StandardCost = IIf(IsNumeric(standardCost), standardCost, 0.0)
                    .ListPrice = IIf(IsNumeric(listPrice), listPrice, 0.0)
                    .Color = color
                    .Picture = New Byte() {}
                    .Weight = IIf(IsNumeric(weight), weight, 0)
                    .Volume = IIf(IsNumeric(volume), volume, 0)
                    .PackageUnitsCount = IIf(IsNumeric(packageUnitsCount), packageUnitsCount, 0)
                    .VendorID = IIf(IsNumeric(suppplierID), suppplierID, 0)
                    .CategoryID = IIf(IsNumeric(categoryID), categoryID, 0)

                    .Connection = New SqlClient.SqlConnection(Me._en.GetConnectionString)

                End With

                Me._en.ExpenseManager.UpdateProduct(product)

                GridViewProducts.EditIndex = -1

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.UpdateProduct, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case EditCommand

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.EditProduct, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)

                Exit Select

            Case MergeCommand

                With GridViewProducts

                    Dim selectedRowsProductIDs As Integer() = (From row As GridViewRow
                                                                In .Rows
                                                               Where DirectCast(row.FindControl("CheckBoxSelectedProductID"), CheckBox).Checked = True
                                                               Select CInt(DirectCast(row.FindControl("CheckBoxSelectedProductID"), CheckBox).Attributes("ProductID"))).ToArray()

                    If selectedRowsProductIDs.Length > 1 OrElse selectedRowsProductIDs.Length = 0 Then
                        Environment.DisplayWebPageMessage(sender, "Please select one product to merge!")
                        Return
                    End If

                    Product.Merge(productID, selectedRowsProductIDs(0), Me._en.UserID, Me._en.GetConnectionString)

                    Me._en.ExpenseManager.GetProducts()

                    Me.ButtonFilter_Click(sender, e)

                End With

                Exit Select

            Case RemoveFromListCommand

                Dim itemToRemove = Me._en.ShoppingList.Where(Function(sl) sl.Item2.ID = productID)

                If itemToRemove IsNot Nothing Then
                    Me._en.ShoppingList.RemoveRange(itemToRemove)
                End If

                Exit Select

        End Select

        If Not String.IsNullOrEmpty(e.CommandName) Then

            If IsNumeric(Request.QueryString("pid")) Then
                Me.RebindProductsGrid(Function(p) p.UserID = Me._en.UserID AndAlso p.ID = productID)
            Else
                Me.RebindProductsGrid(Function(p) p.UserID = Me._en.UserID _
                                       AndAlso IIf(DropDownListFilterByCategory.SelectedValue = Product.PRODUCT_DEFAULT_ID, True, p.CategoryID = DropDownListFilterByCategory.SelectedValue) _
                                       AndAlso IIf(DropDownListFilterBySupplier.SelectedValue = Supplier.SUPPLIER_DEFAULT_ID, True, p.VendorID = DropDownListFilterBySupplier.SelectedValue) _
                                       AndAlso IIf(String.IsNullOrWhiteSpace(TextBoxFilterByName.Text), True, p.Name.ToUpper().Contains(TextBoxFilterByName.Text.ToUpper())))
            End If

        End If
    End Sub

    Protected Sub GridViewProducts_RowCreated(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridViewProducts.RowCreated

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

    Protected Sub ButtonAddToShoppingList_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddToShoppingList.Click

        Dim selectedProduct As Product = Me.Products.FirstOrDefault(Function(p) p.ID = Me.SelectedProductID)

        Dim amount As Decimal = 1

        If IsNumeric(TextBoxAddToShoppingListAmount.Text) AndAlso CDec(TextBoxAddToShoppingListAmount.Text) > 0 Then
            amount = CDec(TextBoxAddToShoppingListAmount.Text.Replace(",", ".")) ' TODO: Use decimal.Parse and specify culture
        End If

        Dim averagePrice As Decimal = selectedProduct.GetProductPriceStatistics().Average(Function(pps) pps.Item1)

        Me._en.AddToShoppingList(amount, Me.SelectedProductID, averagePrice * amount)

        ButtonPrintShoppingList.Visible = True

        Dim editedRow As GridViewRow = (From row As GridViewRow In GridViewProducts.Rows
                                        Where CInt(CType(row.FindControl("LabelProductID"), Label).Text) = selectedProduct.ID
                                        Select row).FirstOrDefault()

        editedRow.FindControl("ImageItemIsInShoppingList").Visible = True

        editedRow.FindControl("LinkButtonRemoveFromShoppingList").Visible = True

        editedRow.FindControl("LinkButtonShowShoppingListDialog").Visible = False

        ' Get how many times we have added this product to our list
        'Dim duplicates = Me.ShoppingList.Where(Function(p) p.Item2.ID = productID).GroupBy(Function(p) p.Item2.ID).SelectMany(Function(p) p.Skip(0))

        'Dim message As String = String.Format("Added:\n\n{0}\n\nx{1}", Me.ShoppingList.LastOrDefault().Item1, duplicates.Count)

        'Environment.DisplayWebPageMessage(GridViewProducts, message)

    End Sub

    Protected Sub ButtonPrintShoppingList_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonPrintShoppingList.Click

        Dim filePath As String = Me._en.GenerateShoppingListFile(False, AddressOf Me.RebindProductsGrid)

        Session(GlobalVariableNames.FILE_NAME) = filePath

        Response.Redirect(URLRewriter.GetLink("FileDownloader"))

    End Sub

    Protected Sub GridViewProducts_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewProducts.RowDataBound

        If GridViewProducts.EditIndex <> -1 AndAlso e.Row.RowIndex = GridViewProducts.EditIndex Then

            Dim productCategoriesDropDown As DropDownList = CType(e.Row.FindControl("DropDownListProductCategories"), DropDownList)

            With productCategoriesDropDown
                .DataTextField = "Name"
                .DataValueField = "ID"
                .DataSource = Me._en.Categories.OrderBy(Function(c) c.Name)
                .DataBind()
            End With

            Dim productVendorsDropDown As DropDownList = CType(e.Row.FindControl("DropDownListProductVendors"), DropDownList)

            With productVendorsDropDown
                .DataTextField = "Name"
                .DataValueField = "ID"
                .DataSource = Me._en.Suppliers.OrderBy(Function(s) s.Name)
                .DataBind()
            End With

            Environment.InsertDropDownEmptyItem(productCategoriesDropDown, Product.PRODUCT_DEFAULT_ID)
            Environment.InsertDropDownEmptyItem(productVendorsDropDown, Supplier.SUPPLIER_DEFAULT_ID)

            Environment.SetDropDownSelectedValue(productCategoriesDropDown, CType(e.Row.FindControl("LabelEditProductCategoryID"), Label).Text)
            Environment.SetDropDownSelectedValue(productVendorsDropDown, CType(e.Row.FindControl("LabelEditProductVendorID"), Label).Text)

        End If

    End Sub

    Protected Sub GridViewProducts_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridViewProducts.RowEditing
        GridViewProducts.EditIndex = e.NewEditIndex
        GridViewProducts.Columns(Me._en.GetCellIndexByName(GridViewProducts.Rows(e.NewEditIndex), Me._en.GetTranslatedValue("DateModified", Me._en.CurrentLanguage))).Visible = False
        Me.RebindProductsGrid()
    End Sub

    Protected Sub GridViewProducts_RowCancelingEdit(sender As Object, e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridViewProducts.RowCancelingEdit
        GridViewProducts.Columns(Me._en.GetCellIndexByName(GridViewProducts.Rows(e.RowIndex), Me._en.GetTranslatedValue("DateModified", Me._en.CurrentLanguage))).Visible = True
    End Sub

    Protected Sub GridViewProducts_RowUpdating(sender As Object, e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridViewProducts.RowUpdating

    End Sub

    Protected Sub GridViewProducts_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridViewProducts.RowDeleting

    End Sub

    Protected Sub GridViewProducts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridViewProducts.SelectedIndexChanged

    End Sub

    Protected Sub ButtonFilter_Click(sender As Object, e As EventArgs)

        Me.RebindProductsGrid(Function(p) p.UserID = Me._en.UserID _
                                  AndAlso IIf(DropDownListFilterByCategory.SelectedValue = Product.PRODUCT_DEFAULT_ID, True, p.CategoryID = DropDownListFilterByCategory.SelectedValue) _
                                  AndAlso IIf(DropDownListFilterBySupplier.SelectedValue = Supplier.SUPPLIER_DEFAULT_ID, True, p.VendorID = DropDownListFilterBySupplier.SelectedValue) _
                                  AndAlso IIf(String.IsNullOrWhiteSpace(TextBoxFilterByName.Text), True, p.Name.ToUpper().Contains(TextBoxFilterByName.Text.ToUpper())))
    End Sub

    Protected Sub LinkButtonShowShoppingListDialog_Click(sender As Object, e As EventArgs)

        Dim currentRow As GridViewRow = CType(CType(sender, LinkButton).NamingContainer, GridViewRow)

        Me.SelectedProductID = CInt(CType(currentRow.FindControl("LabelProductID"), Label).Text)

    End Sub

    Public Sub SaveFileAs(ByVal fullFilePath As String)

        With HttpContext.Current.Response

            .Clear()
            .ContentType = HttpHeadersContentType.PDF
            .AppendHeader("Content-Disposition", String.Format("attachment; filename={0}", Path.GetFileName(fullFilePath)))
            .TransmitFile(fullFilePath)
            .Flush()

        End With

    End Sub

    Protected Sub GridViewProducts_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)

        GridViewProducts.PageIndex = e.NewPageIndex

        Me.RebindProductsGrid()

    End Sub
End Class