Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Web.UI.DataVisualization.Charting
Imports System.Data.Common
Imports MHB.BL
Imports System.Threading
Imports MHB.BL.Enums
Imports System.Drawing.Imaging
Imports MHB.DAL
Imports AjaxControlToolkit
Imports System.Web.Script.Serialization

Partial Public Class TestGrid
    Inherits Environment

#Region "[ Constants ]"

    Private Const ADD_PRODUCT_COMMAND_NAME As String = "AddProduct"
    Private Const HOTKEY_SAVE As String = "HotKeySave"
    Private Const BATCH_SET_DETAILS_SUPPLIER As String = "BatchSetDetailsSupplier"
    Private Const REBIND_DETAILS_GRID_IN_EDIT_MODE As String = "RebindDetailsGridInEditMode"
    Private Const SHOW_DETAILS_TABLE_AFTER_AJAX_UPLOAD_COMPLETED As String = "AjaxFileUploadReceipt"
    Private Const PRODUCT_EXPENDITURE_DETAILS_POPUP = "ProductExpenditureDetailsPopup"

#End Region

#Region "[ Page_PreRender ]"
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        ' Reset sum of selected grid cells
        Environment.ExecuteScript(sender, "ResetGridCellSum();")

    End Sub
#End Region

#Region "[ Page_Init ]"
    Protected Overloads Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init

        UpdatePanelButtonHidePaidExpenses.Attributes.Add("style", "display:inline")

    End Sub
#End Region

#Region "[ Page_Load ]"
    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Me.InitContextMenu()

            Me.CheckAccess()

            If Not IsPostBack Then

                Me.RebindDivCategory()

                Me.FillYearsDropDownList()

                DivError.Visible = False

                ' Shows a message reminding the user to set his/hers preffered language in the account-settings page
                ' once set the HasSetCurrentLanguage flag is set to true and the message won't reapear
                'If Not HasSetCurrentLanguage Then
                '    ShowSetCurrentLanguageReminder()
                'End If

                DivError.Attributes.Add("onmouseover", "getElementById('" & DivError.ClientID & "').style.visibility= 'hidden'")

                UpdateProgress1.DisplayAfter = 0

                Me.RebindGrid()

                If Me.Page.Request.UrlReferrer IsNot Nothing AndAlso Me.Page.Request.UrlReferrer.AbsolutePath = URLRewriter.GetLink("ProductsManagement") Then
                    GridViewDetails.PageIndex = Integer.MaxValue
                    Environment.ExecuteScript(sender, "ShowDetailsTable('', '', false);", True)
                End If

                Me.RebindDetailsGrid(Me.ExpenditureID)

                ButtonDeleteSelectedRow.OnClientClick = String.Format("javascript:return confirm('{0}')", Me.GetTranslatedValue("reallydelete", Me.CurrentLanguage))
                LinkButtonContextMenuDelete.OnClientClick = String.Format("javascript:return confirm('{0}')", Me.GetTranslatedValue("reallydelete", Me.CurrentLanguage))
                ImageButtonOpenPieChartSuppliersInNewWindow.OnClientClick = String.Format("javascript:ShowPieChartSuppliersPopup('{0}');return false;", Me.GetTranslatedValue("SuppliersPieChartTitle", Me.CurrentLanguage))
                ImageButtonOpenPieChartProductsInNewWindow.OnClientClick = String.Format("javascript:ShowPieChartProductsPopup('{0}');return false;", Me.GetTranslatedValue("ProductsPieChartTitle", Me.CurrentLanguage))
                ImageButtonOpenPieChartCategoriesInNewWindow.OnClientClick = String.Format("javascript:ShowPieChartCategoriesPopup('{0}');return false;", Me.GetTranslatedValue("CategoriesPieChartTitle", Me.CurrentLanguage))

                ' each tabbutton has an ID based on the month number (ex. Button1)
                ' we get the month number from current date and set the selected button to correspond
                ' to the current month
                Me.SetSelectedTabButton(String.Format("Button{0}", Me.Month))

                ' Forces numeric input
                'TextBoxMonthlyBudget.Attributes.Add("onkeydown", "ForceNumericInput(this, true, true)")

                UserManager.User.SetUsersCurrentLanguage(Me.CurrentLanguage, Me.UserID, Me.GetConnectionString)

                LinkButtonSumFlaggedBillsText.ToolTip = Me.GetTranslatedValue("getflaggedsum", Me.CurrentLanguage)

                Dim item As ListItem = (From itm As ListItem In DropDownListYear.Items Where itm.Value = Year Select itm).FirstOrDefault()
                item.Selected = True

                Me.DrawPieChart()

                Me.drpDestinationMonth.SelectedValue = Me.Month + 1
                Me.drpYear.SelectedValue = Me.Year

                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "scrollBottom", "<script language='javascript'>document.getElementById('DivAddNewRecord').style.display = '';document.getElementById('PanelAddNewFields').style.display = '';document.getElementById('DivModal').style.display = '';</script>", False)

                LabelExpectedCostTotalSumDifferenceText.ToolTip = Me.GetTranslatedValue("LabelExpectedCostTotalSumDifferenceTooltip", Me.CurrentLanguage)
                LabelExpectedCostTotalSumDifference.ToolTip = Me.GetTranslatedValue("LabelExpectedCostTotalSumDifferenceTooltip", Me.CurrentLanguage)
            End If

            ToolTip1.ToolTipMessage = Me.GetTranslatedValue("ToolTip1", Me.CurrentLanguage)
            'ToolTip2.ToolTipMessage = GetTranslatedValue("ToolTip2", CurrentLanguage)

            LabelLastUpdated.Text = String.Format("{0}{1}", Me.GetTranslatedValue("Lastupdated", Me.CurrentLanguage), Me.LastUpdate)

            'TabPanelProductsPieChart.HeaderText = Me.GetTranslatedValue("Products", Me.CurrentLanguage)
            'TabPanelCategoriesPieChart.HeaderText = Me.GetTranslatedValue("Categories", Me.CurrentLanguage)
            'TabPanelSuppliersPieChart.HeaderText = Me.GetTranslatedValue("Suppliers", Me.CurrentLanguage)
            'TabPanelMostFrequentlyPurchasedProducts.HeaderText = Me.GetTranslatedValue("MostFrequentItemsTabTitle", Me.CurrentLanguage)

            If Not IsPostBack AndAlso UserID = 25 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "editdirectlyincellsofgrid", "alert('" & GetTranslatedValue("editdirectgridcells", CurrentLanguage) & "');", True)
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "temp", "<script type='text/javascript'>FadeOutUndoIcons();</script>", False)

            Me.HandlePostBacks(sender, e)

            If Me.IsInEditMode Then
                If Me.DetailsDataSource.Count() > 0 Then
                    Environment.ExecuteScript(sender, "ShowDetailsTableInEditMode();", True)
                End If
            End If

        Catch te As Threading.ThreadAbortException
        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("Page_Load(): {0}", ex.Message)
            Logging.Logger.Log(ex, "Page_Load()", "", Me.UserID, Me.GetConnectionString)
        End Try
    End Sub

#End Region

    Private Sub HandlePostBacks(ByVal sender As Object, ByVal e As System.EventArgs)

        If IsPostBack Then

            If Request.Form("__EVENTTARGET") = ButtonAddExpenditureDetails.ClientID AndAlso IsNumeric(Request.Form("__EVENTARGUMENT")) Then

                Dim productID As Integer = CInt(Request.Form("__EVENTARGUMENT"))

                Dim textBoxNewDetailValue As TextBox =
                    DirectCast(GridViewDetails.FooterRow.FindControl("TextBoxDetailValueNew"), TextBox)

                Dim product As Product = Me.Products.FirstOrDefault(Function(p) p.ID = productID)

                With product

                    Dim averagePrice As Decimal = Me.GetSuggestedProductAveragePrice(productID)

                    If averagePrice = 0 Then
                        textBoxNewDetailValue.Text = .StandardCost
                    Else
                        textBoxNewDetailValue.Text = averagePrice.ToString("0.00")
                    End If

                    DirectCast(GridViewDetails.FooterRow.FindControl("TextBoxDetailNameNew"), TextBox).Text = .Name

                    DirectCast(GridViewDetails.FooterRow.FindControl("TextBoxDetailDescriptionNew"), TextBox).Text = .Description

                    Environment.ExecuteScript(sender, String.Format("$(""[id$=HiddenDetailsMeasureTypeID]"").val(""{0}"");", CInt(.PrevailingMeasureType)), True)

                    Me.HiddenDetailsMeasureTypeID.Value = CInt(.PrevailingMeasureType)

                End With

                Environment.ExecuteScript(sender, "SetAutoComplete()", True)

                Environment.ExecuteScript(GridViewDetails, "$('[id$=TextBoxDetailAmountNew]').spinner();", True)

                Me.ButtonAddExpenditureDetails_Click(sender, e)

                UpdatePanelDetails.Update()

            ElseIf Request.Form("__EVENTTARGET") = HOTKEY_SAVE Then

                Me.ButtonUpdate_Click(sender, e)
                Me.ButtonUpdateDetailsTable_Click(sender, e)

            ElseIf Request.Form("__EVENTTARGET") = ButtonDetailsPrintShoppingList.ClientID Then

                Me.ButtonDetailsPrintShoppingList_Click(sender, e)

            ElseIf Request.Form("__EVENTTARGET") = BATCH_SET_DETAILS_SUPPLIER Then

                If Not String.IsNullOrWhiteSpace(Request.Form("__EVENTARGUMENT")) Then

                    Dim selectedDetailsIDs As String() = Request.Form("__EVENTARGUMENT").Split(",")

                    Dim selectedDetails = Me.DetailsDataSource.Where(Function(d) selectedDetailsIDs.Contains(d.ID)).ToList()

                    For Each detail As ExpenditureDetail In selectedDetails
                        detail.SupplierID = Me.HiddenSelectedSupplier.Value
                    Next

                    Me.ExpenseManager.UpdateChildExpenses(selectedDetails)

                    Me.RebindGrid()

                    Me.RebindDetailsGrid()

                    Dim message As String =
                       String.Format(Me.GetTranslatedValue("BatchDetailsSupplierChangedMessage", Me.CurrentLanguage), String.Join(",", selectedDetails.Select(Function(d) d.DetailName).ToArray()), Me.Suppliers.FirstOrDefault(Function(s) s.ID = Me.HiddenSelectedSupplier.Value).Name)

                    Environment.ExecuteScript(sender, String.Format("alert('{0}'); ShowDetailsTable('', '', false);", message), True)
                End If

            ElseIf Request.Form("__EVENTTARGET") = REBIND_DETAILS_GRID_IN_EDIT_MODE Then
                Me.RebindDetailsGrid(Me.MainTableDataSource.FirstOrDefault(Function(parent) parent.CategoryID = Category.Fuel).ID)
                Environment.ExecuteScript(sender, "ShowDetailsTableInEditMode();", True)

                'ElseIf Request.Form("__EVENTTARGET") = PRODUCT_EXPENDITURE_DETAILS_POPUP Then

                '    Dim selectedDetail As String = Request.Form("__EVENTARGUMENT")

                '    If IsNumeric(selectedDetail) Then

                '        Me.ProductDetailsControl.ExpenseDetail = Me.DetailsDataSource.FirstOrDefault(Function(ed) ed.ID = CInt(selectedDetail))
                '        Environment.ExecuteScript(sender, "ShowDetailsTable('', '', false);ShowProductDetailsPanel();", True)

                '    End If
            ElseIf Request.Form("__EVENTTARGET") = SHOW_DETAILS_TABLE_AFTER_AJAX_UPLOAD_COMPLETED Then

                Me.RebindGrid()

                Me.RebindDetailsGrid()

                Dim script As String = String.Format("javascript:ShowDetailsTable(""{0}"", ""{1}"", {2});", Me.MainTableDataSource.FirstOrDefault(Function(exp) exp.ID = Me.ExpenditureID).FieldName, Me.GetTranslatedValue("PickASupplierTitle", Me.CurrentLanguage), IIf(Me.Suppliers.Count > 0, "true", "false"))

                Environment.ExecuteScript(sender, script)

            ElseIf Request.Form("__EVENTTARGET") = UpdatePanelDetails.ClientID AndAlso IsNumeric(Request.Form("__EVENTARGUMENT")) Then

                Dim expenditureID As Integer = CInt(Request.Form("__EVENTARGUMENT"))

                Me.ExpenditureID = expenditureID

                Dim expenditure As Expenditure = Me.MainTableDataSource.FirstOrDefault(Function(exp) exp.ID = expenditureID)

                Environment.ExecuteScript(sender, String.Format("ShowDetailsTable('{0}', '', false);", expenditure.FieldName))

                GridViewDetails.PageIndex = Integer.MaxValue

                Me.RebindDetailsGrid(expenditureID)

                Dim tabControls As Control() = New Control(DetailsGridEditedCategoriesTabPanel.Controls.Count - 1) {}

                DetailsGridEditedCategoriesTabPanel.Controls.CopyTo(tabControls, 0)

                Dim tabIndex As Integer = Array.FindIndex(tabControls, Function(control) control.ID = CStr(expenditureID))

                HiddenSelectedLastEditedParentExpenditureTabIndex.Value = CStr(tabIndex - 1)

            ElseIf Request.Form("__EVENTTARGET") = TextBoxDetailsGridSearch.ClientID Then

                If (IsNumeric(Request.Form("__EVENTARGUMENT"))) Then

                    Dim expenditureID As Integer = CInt(Request.Form("__EVENTARGUMENT"))

                    If expenditureID > 0 Then
                        Me.DetailsDataSource = Me.DetailsDataSource.Where(Function(d) d.ID = expenditureID).ToList()
                    Else
                        Dim detailsGridSearch As DetailsGridSearch = New DetailsGridSearch()

                        Dim jsonResult As String = detailsGridSearch.SearchDetails(TextBoxDetailsGridSearch.Text)

                        Dim serializer As JavaScriptSerializer = New JavaScriptSerializer()

                        Dim matchingDetails As IEnumerable(Of ExpenditureDetailInfo) = CType(serializer.Deserialize(jsonResult, GetType(IEnumerable(Of ExpenditureDetailInfo))), IEnumerable(Of ExpenditureDetailInfo))

                        If matchingDetails.Any() Then
                            Me.DetailsDataSource = Me.DetailsDataSource.Where(Function(d) matchingDetails.Select(Function(m) m.ID).Contains(d.ID)).ToList()
                        End If

                    End If

                    Me.RebindDetailsGridInternal()

                    UpdatePanelDetails.Update()

                End If

            End If

        End If

    End Sub

#Region "[ Sub: RebindGrid ]"

    Protected Sub RebindGrid(Optional ByVal sortEventArgs As GridViewSortEventArgs = Nothing)

        Dim qry As String = String.Empty

        Try

            'Dim args As ExpressionQueryArgs = Me.ExpenseManager.GetExpressionQueryArgs()

            'args.Add("Flagged", Me.DisplayAllFlaggedSums)
            'args.Add("IsPaid", Me.PaidExpensesHidden)

            'reloadedExpenses = Me.ExpenseManager.GetUserExpenditures(Function(exp) _
            '                                                exp.UserID = ExpressionQueryArgs.Parameter(Of Integer)("UserID") AndAlso _
            '                                                exp.Year = ExpressionQueryArgs.Parameter(Of Integer)("Year") AndAlso _
            '                                                exp.Month = ExpressionQueryArgs.Parameter(Of Integer)("Month") AndAlso _
            '                                                exp.IsPaid = ExpressionQueryArgs.Parameter(Of Integer)("IsPaid") AndAlso _
            '                                                exp.Flagged = ExpressionQueryArgs.Parameter(Of Integer)("Flagged") AndAlso _
            '                                                exp.IsDeleted = False, args)

            Dim reloadedExpenses As IEnumerable(Of Expenditure) = Enumerable.Empty(Of Expenditure)()

            If Me.DisplayAllFlaggedSums Then
                reloadedExpenses = Me.ExpenseManager.GetUserExpenditures(True)
            Else
                reloadedExpenses = Me.ExpenseManager.GetUserExpenditures(False, True, Me.PaidExpensesHidden)
            End If

            If Me.MainTableDataSource IsNot Nothing Then

                Dim modifiedExpenses As IEnumerable(Of Expenditure) =
                    reloadedExpenses.Where(Function(m) Me.MainTableDataSource.Any(Function(re) re.ID = m.ID) AndAlso m.FieldValue <> Me.MainTableDataSource.FirstOrDefault(Function(re) re.ID = m.ID).FieldValue).ToArray()

                For Each expense As Expenditure In modifiedExpenses
                    Me.AddTransaction(expense.ID, expense.FieldValue, Me.MainTableDataSource.FirstOrDefault(Function(re) re.ID = expense.ID).FieldValue, expense.FieldName)
                Next

                Me.RecordTransactions()

            End If

            Me.MainTableDataSource = reloadedExpenses

            If sortEventArgs IsNot Nothing Then

                If ViewState("SortDirectionGridView1") Is Nothing Then
                    ViewState("SortDirectionGridView1") = sortEventArgs.SortDirection
                End If

                If ViewState("SortDirectionGridView1").ToString() = sortEventArgs.SortDirection.ToString() Then

                    sortEventArgs.SortDirection = IIf(CType(ViewState("SortColumnDirectionGridView1"), SortDirection) = SortDirection.ASC, SortDirection.DESC, SortDirection.ASC)

                End If

                Dim sortExpression As String = String.Format("{0} {1}", sortEventArgs.SortExpression, sortEventArgs.SortDirection.ToString())

                Me.MainTableDataSource = Me.MainTableDataSource.Sort(sortExpression)

                ViewState("SortDirectionGridView1") = sortEventArgs.SortDirection

            End If

            GridView1.DataSource = Me.MainTableDataSource
            GridView1.DataBind()

            ' If removed Details link is not translated
            Me.TranslateGridViewControls(GridView1)

            If Me.IsInEditMode Then
                Me.PutGridViewHeaderInEditMode(GridView1)
            End If

            Me.CalculateAndDisplaySum()

            Me.IsInSearchMode = False

            With GridViewMostFrequentlyPurchasedProducts
                .DataSource = Me.ExpenseManager.GetMostFrequentlyPurchasedItems(Me.Month, Me.Year, 15)
                .DataBind()
            End With

            With GridViewSurplusItems
                .DataSource = Me.MainTableDataSource.Where(Function(m) m.HasDetails = True).SelectMany(Function(m) m.Details).Where(Function(d) d.IsSurplus = True)
                .DataBind()
            End With

            With GridViewLastEditedItems
                .DataSource = Me.MainTableDataSource.Where(Function(m) m.HasDetails = True).SelectMany(Function(m) m.Details).OrderByDescending(Function(d) d.DetailDate).Take(25)
                .DataBind()
            End With

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("RebindGrid(): {0}", ex.Message)
            Logging.Logger.Log(ex, "RebindGrid", qry, Me.UserID, Me.GetConnectionString)
        End Try

    End Sub

    Protected Sub RebindGrid(ByVal items As List(Of Expenditure))

        Dim qry As String = String.Empty

        Try

            Me.MainTableDataSource = items

            GridView1.DataSource = Me.MainTableDataSource
            GridView1.DataBind()

            Me.CalculateAndDisplaySum()

            Me.IsInSearchMode = False

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("RebindGrid(items): {0}", ex.Message)
            Logging.Logger.Log(ex, "RebindGrid(items)", qry, Me.UserID, Me.GetConnectionString)
        End Try

    End Sub

#End Region

#Region "[ Sub: RebindDetailsGrid ]"

    Protected Sub RebindDetailsGrid(ByVal expenditureID As Integer)

        Dim parentExpenditure As Expenditure = Me.MainTableDataSource.FirstOrDefault(Function(e) e.ID = expenditureID)

        If parentExpenditure IsNot Nothing AndAlso Not Me.LastEditedParentExpenditures.Any(Function(exp) exp.ID = expenditureID) Then
            Me.LastEditedParentExpenditures.Add(parentExpenditure)
            HiddenSelectedLastEditedParentExpenditureTabIndex.Value = CStr(Me.LastEditedParentExpenditures.Count - 1) ' that means new detail is opened and a new tab would be added - so select it
        Else
            HiddenSelectedLastEditedParentExpenditureTabIndex.Value = CStr(Me.LastEditedParentExpenditures.FindIndex(Function(exp) exp.ID = expenditureID))
        End If

        'Environment.ExecuteScript(GridViewDetails, "$(""#tabs"").tabs();")

        If Me.MainTableDataSource.Count = 0 Then
            Return
        End If

        Me.DetailsDataSource = Me.MainTableDataSource.Where(Function(m) m.ID = expenditureID).SelectMany(Function(m) m.Details).ToArray()

        Me.RebindDetailsGridInternal()

        With ListBoxParentExpenditures
            .DataSource = Me.MainTableDataSource.Select(Function(p) New With {.ID = p.ID, .Name = p.FieldName})
            .DataTextField = "Name"
            .DataValueField = "ID"
            .DataBind()
        End With

    End Sub

    Protected Sub RebindDetailsGrid(ByVal year As Integer, ByVal categoryID As Integer, Optional ByVal measureType As Enums.MeasureType = MeasureType.NotSet)

        Me.DetailsDataSource = Me.ExpenseManager.GetExpenditureDetails(New SqlConnection(Me.GetConnectionString), year, categoryID, measureType)

        Me.RebindDetailsGridInternal()

    End Sub

    Protected Sub RebindDetailsGrid()

        If Me.ExpenditureID > 0 Then
            Me.RebindDetailsGrid(Me.ExpenditureID)
        End If

    End Sub

    Private Sub FillLastEditedDetailsTabPanel()

        For Each expenditure As Expenditure In Me.LastEditedParentExpenditures

            If DetailsGridEditedCategoriesTabPanel.FindControl(CStr(expenditure.ID)) Is Nothing Then

                Dim tab As HtmlGenericControl = New HtmlGenericControl("li") With {.ID = CStr(expenditure.ID)}

                Dim anchor As HtmlAnchor = New HtmlAnchor() With {.HRef = "foo", .InnerText = expenditure.FieldName}

                anchor.Attributes.Add("onclick", String.Format("__doPostBack('{0}', {1});", UpdatePanelDetails.ClientID, expenditure.ID))

                tab.Controls.Add(anchor)

                DetailsGridEditedCategoriesTabPanel.Controls.Add(tab)
            End If
        Next

    End Sub

    Public Function GetSupplierName(ByVal supplier As Supplier, ByVal displayFullName As Boolean) As String

        If supplier IsNot Nothing AndAlso supplier.ID <> Supplier.SUPPLIER_DEFAULT_ID Then
            If Not displayFullName AndAlso supplier.Name.Length >= 10 Then
                Return String.Format("{0}..", supplier.Name.Substring(0, 10))
            Else
                Return supplier.Name
            End If
        Else
            Return String.Empty
        End If

    End Function

    Protected Sub RebindDetailsGridInternal(Optional ByVal sortEventArgs As GridViewSortEventArgs = Nothing)

        Try

            If Me.DetailsDataSource.Count > 0 Then

                If sortEventArgs IsNot Nothing Then

                    If ViewState("SortDirection") Is Nothing Then
                        ViewState("SortDirection") = sortEventArgs.SortDirection
                    End If

                    If ViewState("SortDirection").ToString() = sortEventArgs.SortDirection.ToString() Then

                        sortEventArgs.SortDirection = IIf(CType(ViewState("SortColumnDirection"), SortDirection) = SortDirection.ASC, SortDirection.DESC, SortDirection.ASC)

                    End If

                    Dim sortExpression As String = String.Format("{0} {1}", sortEventArgs.SortExpression, sortEventArgs.SortDirection.ToString())

                    Me.DetailsDataSource = Me.DetailsDataSource.Sort(sortExpression).ToArray()

                    ViewState("SortDirection") = sortEventArgs.SortDirection

                End If

                GridViewDetails.DataSource = Me.DetailsDataSource

                Dim referenceDetail As ExpenditureDetail = Me.DetailsDataSource.FirstOrDefault(Function(d) d.ProductID <> Product.PRODUCT_DEFAULT_ID)

                If referenceDetail IsNot Nothing Then

                    Dim refereceCategoryID As Integer = referenceDetail.CategoryID

                    If Me.DetailsDataSource.All(Function(d) d.CategoryID = refereceCategoryID) Then
                        ButtonAllDetailsForCategory.Visible = True
                    End If
                Else
                    ButtonAllDetailsForCategory.Visible = False
                End If

            Else
                GridViewDetails.DataSource = New List(Of ExpenditureDetail) From {New ExpenditureDetail()}
            End If

            If Me.PageSizeDetails = -1 Then
                Me.PageSizeDetails = Me.DetailsDataSource.Count()
            End If

            GridViewDetails.PageSize = Me.PageSizeDetails

            GridViewDetails.DataBind()

            Me.TranslateGridViewControls(GridViewDetails)

            If Me.IsInEditMode Then
                Me.PutGridViewHeaderInEditMode(GridViewDetails)
            End If

            Me.RebindDetailsGridProductsInfo()

            Environment.ExecuteScript(GridViewDetails, "$('[id*=TextBoxDetailAmountNew]').spinner();", True)

            Me.DrawDetailsPieChart()

            Me.FillLastEditedDetailsTabPanel()

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("RebindDetailsGridInternal(): {0}", ex.Message)
            Logging.Logger.Log(ex, "RebindDetailsGridInternal", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try
    End Sub

    Private Sub DrawDetailsPieChart()

        Try

            Dim sbDataPoints As StringBuilder = New StringBuilder()

            Dim getAmount As Func(Of IEnumerable(Of ExpenditureDetail), String) = Function(details) As String

                                                                                      Dim result As String = String.Empty

                                                                                      Try

                                                                                          Dim groupedDetails = details.GroupBy(Function(d) d.MeasureType)

                                                                                          result = String.Format("{0:0.0} {1} - ", details.Sum(Function(d) d.DetailValue), Me.Currency)

                                                                                          For Each group In groupedDetails.OrderBy(Function(d) d.Key)

                                                                                              Dim weightText = String.Empty, countText As String = String.Empty

                                                                                              Select Case group.Key

                                                                                                  Case MeasureType.Weight, MeasureType.Volume
                                                                                                      weightText = String.Format("{0:0.00} & ", group.Sum(Function(d) d.Amount))
                                                                                                      Exit Select
                                                                                                  Case Else
                                                                                                      Dim count As Decimal = group.Where(Function(d) d.Product IsNot Nothing).Sum(Function(d) d.Product.PackageUnitsCount)

                                                                                                      count += group.Where(Function(d) d.Product Is Nothing).Count()

                                                                                                      If count = 0 Then
                                                                                                          count = group.Count()
                                                                                                      End If

                                                                                                      countText = String.Format("({0}) & ", count)
                                                                                                      Exit Select

                                                                                              End Select

                                                                                              result &= weightText
                                                                                              result &= countText

                                                                                          Next

                                                                                          result = result.Trim().TrimEnd("&".ToCharArray())

                                                                                      Catch ex As Exception
                                                                                          Debug.WriteLine(String.Format("#DrawDetailsPieChart.getAmount(): {0}", ex.Message))
                                                                                      End Try

                                                                                      Return result

                                                                                  End Function

            Dim categories As IEnumerable(Of Tuple(Of Integer, String, Decimal)) = Me.DetailsDataSource _
                .GroupBy(Function(d) d.CategoryID) _
                .Select(Function(group) _
                            New Tuple(Of Integer, String, Decimal)(group.Key, getAmount(group.ToList()), group.Sum(Function(d) d.DetailValue))) _
                            .OrderByDescending(Function(tuple) tuple.Item3)

            sbDataPoints.Append("[")

            For Each kv As Tuple(Of Integer, String, Decimal) In categories

                Dim category As Category = Me.Categories.FirstOrDefault(Function(c) c.ID = kv.Item1)

                If category IsNot Nothing Then

                    Dim legendText As String = String.Format("[{0}] {1}", category.Name, kv.Item2)

                    sbDataPoints.AppendFormat("{{ label: ""{0}"",  y: {1:0.00}, legendText: ""{0}""}}," & vbCrLf, legendText, kv.Item3)

                End If

            Next

            Dim dataPoints As String = sbDataPoints.ToString().TrimEnd(vbCrLf.ToCharArray()).TrimEnd(",".ToCharArray())

            dataPoints &= "]"

            Environment.ExecuteScript(UpdatePanelDetailsGridChartContainer, String.Format("DrawDetailsPieChart({0});", dataPoints), True)

        Catch ex As Exception
            Logging.Logger.Log(ex, "DrawDetailsPieChart", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

    End Sub

#End Region

#Region "[ Sub: RebindDetailsGridProductsInfo ]"

    Protected Sub RebindDetailsGridProductsInfo()

        Try

            Dim mileageColumn As TemplateField = Me.GetColumnByHeaderText(GridViewDetails, Me.GetTranslatedValue("Mileage", Me.CurrentLanguage))

            Dim avgConsumptionColumn As TemplateField = Me.GetColumnByHeaderText(GridViewDetails, Me.GetTranslatedValue("Avg. Consumption", Me.CurrentLanguage))

            If Me.DetailsDataSource.All(Function(d) d.MeasureType = Enums.MeasureType.Volume) Then

                Dim groupedProducts = Me.DetailsDataSource.GroupBy(Function(d) d.ProductID)

                Dim subTotalProductDetails As List(Of ProductInfo) = New List(Of ProductInfo)()

                For Each p In groupedProducts

                    Dim product As Product = p.FirstOrDefault().Product

                    Dim productInfo As ProductInfo = New ProductInfo()

                    With productInfo

                        .Name = product.Name
                        .Amount = p.Sum(Function(d) d.Amount)
                        .Total = p.Sum(Function(d) d.DetailValue)
                        .Average = p.Average(Function(d) d.DetailValue / d.Amount)
                        .Max = p.Max(Function(d) d.DetailValue / d.Amount)
                        .Min = p.Min(Function(d) d.DetailValue / d.Amount)

                        If p.Any(Function(d) d.Parent IsNot Nothing AndAlso d.Parent.CategoryID = Category.Fuel AndAlso d.Product IsNot Nothing AndAlso d.Product.Parameters.Any()) Then

                            Dim detailsWithValidParameters As ExpenditureDetail() =
p.Where(Function(det) det.Product.Parameters.Any(Function(par) par.ProductParameterTypeID = ProductParameterType.Mileage AndAlso IsNumeric(par.Value) AndAlso par.Value > 0)).ToArray()

                            Dim parameters As ProductParameter() = detailsWithValidParameters.SelectMany(Function(det) det.Product.Parameters).ToArray()

                            Dim distance As Decimal = parameters.Sum(Function(par) par.Value)

                            Dim amount As Decimal = detailsWithValidParameters.Sum(Function(det) det.Amount)

                            If distance > 0 AndAlso amount > 0 Then
                                .Consumption = amount / distance * 100
                                .PricePerHundredKms = .Consumption * .Average
                                .PricePerKm = .PricePerHundredKms / 100
                            End If

                            Dim expenditure As Expenditure = Me.MainTableDataSource.FirstOrDefault(Function(e) e.CategoryID = Category.Fuel)

                            If expenditure IsNot Nothing AndAlso IsNumeric(expenditure.FieldExpectedValue) Then

                                .ExpectedQuantity = expenditure.FieldExpectedValue / .Average

                                Dim remainingQuantity As Decimal = (expenditure.FieldExpectedValue - expenditure.FieldValue) / .Average

                                If .Consumption > 0 Then
                                    .ExpectedRange = (.ExpectedQuantity / .Consumption) * 100
                                    .RemainingRange = (remainingQuantity / .Consumption) * 100
                                End If
                            End If

                        Else
                            .Consumption = 0D
                        End If

                    End With

                    subTotalProductDetails.Add(productInfo)

                Next

                With GridViewDetailsProductsInfo
                    .DataSource = subTotalProductDetails
                    .DataBind()
                End With

                mileageColumn.Visible = True
                avgConsumptionColumn.Visible = True

            Else
                With GridViewDetailsProductsInfo
                    .DataSource = Nothing
                    .DataBind()
                End With

                mileageColumn.Visible = False
                avgConsumptionColumn.Visible = False
            End If

            If Me.IsInEditMode Then
                Me.PutGridViewHeaderInEditMode(GridViewDetailsProductsInfo)
            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "RebindDetailsGridProductsInfo", String.Empty, Me.UserID, Me.GetConnectionString)
        Finally
            Me.TranslateGridViewControls(GridViewDetailsProductsInfo)
        End Try

    End Sub

#End Region

#Region "[ Sub: RebindDivCategory ]"

    Protected Sub RebindDivCategory()

        Try
            ListBoxPickCategory.DataTextField = "Name"
            ListBoxPickCategory.DataValueField = "ID"

            ListBoxPickCategory.DataSource = Me.Categories
            ListBoxPickCategory.DataBind()

        Catch ex As Exception
            Logging.Logger.Log(ex, "RebindDivCategory()", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try
    End Sub

#End Region

#Region "[ Sub: CalculateAndDisplaySum ]"

    Protected Sub CalculateAndDisplaySum()

        Dim qry As String = String.Empty

        Try

            Budget1.BindGrid()

            Me.DrawPieChart()

            Me.Sum = 0
            Me.ExpectedCostsSum = 0
            Me.Savings = 0
            Dim sumYearlySavings As Double = 0

            qry = "EXEC spGetBudgetsSavingsSumExpenses @year, @month, @userID"

            Dim reader As IDataReader = DataBaseConnector.GetDataReader(qry, Me.GetConnectionString, New SqlParameter("year", Me.Year), New SqlParameter("month", Me.Month), New SqlParameter("userID", Me.UserID))

            While reader.Read()

                Me.Sum = reader.Get(Of Decimal)("sumExpenses")
                Me.ExpectedCostsSum = reader.Get(Of Decimal)("sumExpectedExpenses")
                Me.MonthBudget = reader.Get(Of Decimal)("budget")
                Me.MonthBudgetTotal = reader.Get(Of Decimal)("budgetTotal")
                Me.Savings = reader.Get(Of Decimal)("savings")

                lblSumFlaggedBillsValue.Text = String.Format("{0:f}", reader.Get(Of Decimal)("sumFlaggedExpenses"))
                sumYearlySavings = reader.Get(Of Decimal)("sumYearlySavings")
                LinkButtonSpentToday.Text = String.Format("{0:f}", reader("sumSpentToday"))
            End While

            If Me.IncomeDataSource IsNot Nothing AndAlso Me.IncomeDataSource.Any() Then

                Dim nextMonthIncome As IEnumerable(Of Income) = Enumerable.Empty(Of Income)()

                Dim nextIncomeDate As Date = Me.GetNextIncomeDate(nextMonthIncome)

                Dim daysUntilNextPayCheck As Integer = (nextIncomeDate - DateTime.Now).Days

                If daysUntilNextPayCheck > 0 Then

                    Dim sumPerDay = (Me.MonthBudget - Me.Sum) / daysUntilNextPayCheck

                    LabelSumPerDay.Text = String.Format("{0:0.00}", sumPerDay)

                End If

                Me.DrawMonthCompletionProgressBar(nextIncomeDate, nextMonthIncome)

            End If

            'LabelSum.Text = String.Format("{0}{1}", (Me.MonthBudget - Me.Sum).ToString("0.00"), Me.Currency)

            If Me.MonthBudget >= Me.ExpectedCostsSum Then
                LabelExpectedCostSumDifference.CssClass = "PlainTextAllSystemsGoExtraLarge"
            Else
                LabelExpectedCostSumDifference.CssClass = "PlainTextErrorExtraLarge"
            End If

            If Me.MonthBudgetTotal >= Me.ExpectedCostsSum Then
                LabelExpectedCostTotalSumDifference.CssClass = "PlainTextAllSystemsGoExtraLarge"
            Else
                LabelExpectedCostTotalSumDifference.CssClass = "PlainTextErrorExtraLarge"
            End If

            'LabelExpectedCostSum.Text = String.Format("{0:f}", Me.ExpectedCostsSum)

            If GridView1.FooterRow IsNot Nothing Then
                DirectCast(GridView1.FooterRow.FindControl("LabelSumPlanned"), Label).Text = String.Format("{0:f}", Me.ExpectedCostsSum)
                DirectCast(GridView1.FooterRow.FindControl("LabelSumPaid"), Label).Text = String.Format("{0:f}", Me.Sum)
                DirectCast(GridView1.FooterRow.FindControl("LabelSumDifference"), Label).Text = String.Format("{0:f}", Me.MonthBudget - Me.Sum)
            End If

            LabelExpectedSavings.Text = String.Format("{0:f}", Me.Savings)

            LabelAllSavings.Text = String.Format("{0:f}", sumYearlySavings)

            LabelExpectedCostSumDifference.Text = String.Format("{0:f}", (Me.MonthBudget - Me.ExpectedCostsSum))

            LabelExpectedCostTotalSumDifference.Text = String.Format("{0:f}", (Me.MonthBudgetTotal - Me.ExpectedCostsSum))

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("CalculateAndDisplaySum(): {0}", ex.Message)
            Logging.Logger.Log(ex, "CalculateAndDisplaySum()", qry, Me.UserID, Me.GetConnectionString)
        End Try

    End Sub

#End Region

#Region "[ Sub: DrawPieChart ]"

    Private Sub InitializePieChart(ByVal chart As Chart, ByVal xAxisColumnName As String, ByVal yAxisColumnName As String, ByVal dataSource As DataTable)

        Try

            Dim dv As DataView = New DataView(dataSource)

            With chart

                .Series("Default").Points.DataBindXY(dv, xAxisColumnName, dv, yAxisColumnName)

                .Series("Default").ChartType = SeriesChartType.Pie
                .Series("Default")("PieLabelStyle") = "Outside"
                .ChartAreas("ChartArea1").Area3DStyle.Enable3D = True

                .Series("Default").IsValueShownAsLabel = True
                .Legends(0).LegendStyle = LegendStyle.Column

                .ImageStorageMode = System.Web.UI.DataVisualization.Charting.ImageStorageMode.UseImageLocation
                .Series("Default").LabelForeColor = Color.Black

            End With

        Catch ex As Exception
            Logging.Logger.Log(ex, "InitializePieChart()", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

    End Sub

    Private Sub DrawPieChart()

        Try

            ' Categories pie chart
            Dim table As DataTable = Me.GetGroupedCategoriesWithSums()

            Me.InitializePieChart(ChartCategories, "costCategoryName", "costSum", table)

            If table.Columns.Contains("costSum") AndAlso table.Columns.Contains("costCategoryName") Then
                table.Columns.Add("costFullName", GetType(String), "costSum + ' ' + costCategoryName")
            End If

            Me.InitializePieChart(ChartCategoriesPopup, "costFullName", "costSum", table)

            ChartCategoriesPopup.Height = table.Rows.Count * 50

            ' Products pie chart
            Dim productsDataSource As DataTable = Me.GetGroupedProductsWithSums()

            Me.InitializePieChart(ChartProducts, "costCategoryName", "costSum", productsDataSource)

            Me.InitializePieChart(ChartProductsPopup, "costCategoryName", "costSum", productsDataSource)

            ChartProductsPopup.Height = table.Rows.Count * 50

            ' Suppliers pie chart
            Dim suppliersDataSource As DataTable = Me.GetGroupedSuppliersWithSums()

            Me.InitializePieChart(ChartSuppliers, "costCategoryName", "costSum", suppliersDataSource)

            Me.InitializePieChart(ChartSuppliersPopup, "costCategoryName", "costSum", suppliersDataSource)

            ChartSuppliersPopup.Height = table.Rows.Count * 50

        Catch ex As Exception
            Logging.Logger.Log(ex, "DrawPieChart()", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

    End Sub

    Private Function GetGroupedProductsWithSums() As DataTable

        Dim table As DataTable = New DataTable()

        Try

            Dim productsForMonth = Me.MainTableDataSource _
                .SelectMany(Function(m) m.Details) _
                .Where(Function(d) d.ProductID <> Product.PRODUCT_DEFAULT_ID AndAlso Me.Products.Select(Function(p) p.ID).Contains(d.ProductID)) _
                .GroupBy(Function(d) d.ProductID) _
                .ToDictionary(Function(d) Me.Products.FirstOrDefault(Function(p) p.ID = d.Key), Function(d) d.Select(Function(dd) dd.DetailValue).Sum()) _
                .OrderByDescending(Function(d) d.Value)

            table.Columns.Add("costCategoryName", GetType(String))
            table.Columns.Add("costSum", GetType(Decimal))

            For Each kv As KeyValuePair(Of Product, Decimal) In productsForMonth
                table.Rows.Add(String.Format("{0} = {1}", kv.Key.Name, kv.Value), kv.Value)
            Next

        Catch ex As Exception
            Logging.Logger.Log(ex, "GetGroupedProductsWithSums", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

        Return table
    End Function

    Private Function GetGroupedCategoriesWithSums() As DataTable

        Dim table As DataTable = New DataTable()

        Try

            Dim categoriesForMonth = Me.MainTableDataSource _
                .Where(Function(d) d.CategoryID <> Category.CATEGORY_DEFAULT_ID AndAlso Me.Categories.Select(Function(c) c.ID).Contains(d.CategoryID)) _
                .GroupBy(Function(d) d.CategoryID) _
                .ToDictionary(Function(d) Me.Categories.FirstOrDefault(Function(c) c.ID = d.Key), Function(d) d.Select(Function(dd) dd.FieldValue).Sum()) _
                .OrderByDescending(Function(d) d.Value)

            table.Columns.Add("costCategoryName", GetType(String))
            table.Columns.Add("costSum", GetType(Decimal))

            For Each kv As KeyValuePair(Of Category, Decimal) In categoriesForMonth
                table.Rows.Add(String.Format("{0} = {1}", kv.Key.Name, kv.Value), kv.Value)
            Next

        Catch ex As Exception
            Logging.Logger.Log(ex, "GetGroupedCategoriesWithSums", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

        Return table
    End Function

    Private Function GetGroupedSuppliersWithSums() As DataTable

        Dim table As DataTable = New DataTable()

        Try

            Dim suppliersForMonth = Me.MainTableDataSource _
                .SelectMany(Function(m) m.Details) _
                .Where(Function(d) d.SupplierID <> Supplier.SUPPLIER_DEFAULT_ID AndAlso Me.Suppliers.Select(Function(s) s.ID).Contains(d.SupplierID)) _
                .GroupBy(Function(d) d.SupplierID) _
                .ToDictionary(Function(d) Me.Suppliers.FirstOrDefault(Function(s) s.ID = d.Key), Function(d) d.Select(Function(dd) dd.DetailValue).Sum()) _
                .OrderByDescending(Function(d) d.Value)

            table.Columns.Add("costCategoryName", GetType(String))
            table.Columns.Add("costSum", GetType(Decimal))

            For Each kv As KeyValuePair(Of Supplier, Decimal) In suppliersForMonth
                table.Rows.Add(String.Format("{0} = {1}", kv.Key.Name, kv.Value), kv.Value)
            Next

        Catch ex As Exception
            Logging.Logger.Log(ex, "GetGroupedSuppliersWithSums", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

        Return table

    End Function

#End Region

    '============================================================================================
    '           GridView1 functional controls
    '============================================================================================

    ' TAB Button click
#Region "[ Button.Click: TabButton Button_Click ]"
    Protected Sub Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click, Button2.Click, Button3.Click, Button4.Click, Button5.Click, Button6.Click, Button7.Click, Button8.Click, Button9.Click, Button10.Click, Button11.Click, Button12.Click

        '!!! ALWAYS CALL FIRST AS ALL METHODS DEPEND ON THE CURRENT MONTH !!!
        ' we get the selected month and populate the Month Session variable
        Me.Month = CInt(CType(sender, Button).ID.Replace("Button", String.Empty))

        ' we display this months budget in the textbox bellow the grid
        'Budget1.BindGrid()

        Me.RestoreLoadingDivPosition()

        Me.DisplayAllFlaggedSums = False

        ' we set the selected button
        Me.SetSelectedTabButton(CType(sender, Button).ID)

        ' we rebind the main grid
        Me.RebindGrid()

        ' we get this months budget and store it in a session variable
        Me.MonthBudget = UserManager.User.GetMonthlyBudget(Me.UserID, Me.Month, Me.Year, Me.GetConnectionString)

        'RebindDetailsGrid(0)

        Logging.Logger.LogAction(Logging.Logger.HistoryAction.ChangeMonth, Me.UserID, Me.GetConnectionString, Request.UserHostAddress)

    End Sub
#End Region

    ' ADD
#Region "[ Button.Click: ButtonAddField_Click ]"
    Protected Sub ButtonAddField_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddField.Click
        Dim qry As String = String.Empty
        Try

            Dim febException As Boolean = False

            Me.ExpenseManager.AddNewParentExpense(
 TextBoxExpectedValue.Text.Replace(",", "."), CheckBoxEnterForEveryMonth.Checked, CheckBoxRecurrentExpenditure.Checked, TextBoxAddNewReccurentFieldDueDate.Text, TextBoxFieldName.Text, TextBoxFieldDescription.Text, HiddenProductID.Value, febException, qry)

            Call Me.ZeroSelectedProductID()

            If febException Then
                ScriptManager.RegisterClientScriptBlock(Me, ButtonDeleteSelectedRow.GetType, "msg", "alert('" & GetTranslatedValue("February28DaysAlert", CurrentLanguage) & "');", True)
            End If

            Me.RebindGrid()

            'DivAddNewRecord.Visible = False
            ''DivModal.Visible = False

        Catch ex As Exception
            Logging.Logger.Log(ex, "ButtonAddField_Click()", qry, UserID, GetConnectionString)
            DivError.Visible = True
            'DivAddNewRecord.Visible = False
            'DivModal.Visible = False
            DivError.InnerText = "ButtonAddField_Click(): " & ex.Message
        End Try
    End Sub
#End Region

    ' UPDATE
#Region "[ Button.Click: ButtonUpdate_Click ]"

    Protected Sub ButtonUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonUpdate.Click

        Me.RestoreLoadingDivPosition()

        Dim expenses As List(Of Expenditure) = New List(Of Expenditure)()

        Try

            Me.Sum = 0
            Me.Savings = 0
            Me.MonthBudget = Me.Budget1.IncomeSum

            For Each row As GridViewRow In GridView1.Rows

                Dim editedExpenditure As Expenditure = Me.GetExpenditureFromGridViewRow(row)

                Dim referenceExpenditure As Expenditure = Me.MainTableDataSource.FirstOrDefault(Function(expense) expense.ID = editedExpenditure.ID)

                editedExpenditure.Flagged = referenceExpenditure.Flagged ' keep and maintain the flagged flag as this changes when flag icon is clicked and this update is called when 'Update' button is pressed

                With editedExpenditure

                    If referenceExpenditure.FieldName <> .FieldName OrElse referenceExpenditure.FieldDescription <> .FieldDescription OrElse referenceExpenditure.FieldValue <> .FieldValue OrElse referenceExpenditure.FieldExpectedValue <> .FieldExpectedValue OrElse referenceExpenditure.DueDate <> .DueDate OrElse referenceExpenditure.IsPaid <> .IsPaid Then

                        expenses.Add(editedExpenditure)

                    End If

                    Me.Sum += .FieldValue

                    If .CategoryID = 5 Then
                        Me.Savings += editedExpenditure.FieldValue
                    End If

                End With
            Next

            Me.ExpenseManager.UpdateBudgets(Me.Sum, Me.MonthBudget, Me.Savings)
            Me.ExpenseManager.UpdateParentExpenses(expenses, String.Empty)

            If Me.IsInSearchMode Then
                SearchControl.PerformSearch()
            Else
                Me.RebindGrid()
            End If

            Logging.Logger.LogAction(Logging.Logger.HistoryAction.Update, Me.UserID, Me.GetConnectionString, Request.UserHostAddress)

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("MainForm.ButtonUpdate_Click: {0}", ex.Message)
            Logging.Logger.Log(ex, "MainForm.ButtonUpdate_Click: {0}", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

    End Sub

    Private Function GetExpenditureFromGridViewRow(ByVal row As GridViewRow) As Expenditure

        Dim expenditure As Expenditure = New Expenditure()

        Try

            With row

                Dim fieldCurrentStoredValue As String = DirectCast(.FindControl("TextBoxHiddenStoredCurrentValue"), TextBox).Text
                Dim fieldValue As String = DirectCast(.FindControl("TextBoxFieldValue"), TextBox).Text
                Dim fieldExpectedValue As String = DirectCast(.FindControl("TextBoxFieldExpectedValue"), TextBox).Text
                Dim dueDate As String = DirectCast(.FindControl("TextBoxDueDate"), TextBox).Text

                Dim fieldOldValue As String = String.Empty

                If fieldValue.Trim() = fieldCurrentStoredValue.Trim() Then
                    fieldOldValue = DirectCast(.FindControl("TextBoxHiddenFieldValueOld"), TextBox).Text
                Else
                    fieldOldValue = DirectCast(.FindControl("TextBoxHiddenStoredCurrentValue"), TextBox).Text
                End If

                If Not IsNumeric(fieldValue) Then
                    fieldValue = 0
                Else
                    fieldValue = fieldValue.ToString().Replace(",", ".")
                End If

                If Not IsNumeric(fieldExpectedValue) Then
                    fieldExpectedValue = 0
                Else
                    fieldExpectedValue = fieldExpectedValue.ToString().Replace(",", ".")
                End If

                expenditure.ID = CInt(DirectCast(.FindControl("TextBoxID"), TextBox).Text)
                expenditure.FieldName = DirectCast(.FindControl("TextBoxFieldName"), TextBox).Text
                expenditure.CategoryID = Me.GetCategoryID(expenditure.FieldName)
                expenditure.FieldDescription = DirectCast(.FindControl("TextBoxFieldDescription"), TextBox).Text
                expenditure.FieldExpectedValue = fieldExpectedValue
                expenditure.FieldValue = fieldValue
                expenditure.FieldOldValue = fieldOldValue
                expenditure.IsPaid = DirectCast(.FindControl("CheckBoxIsPaid"), CheckBox).Checked
                expenditure.DueDate = IIf(IsDate(dueDate), dueDate, CDate("1900-01-01"))
                expenditure.IsShared = DirectCast(.FindControl("CheckBoxIsShared"), CheckBox).Checked

            End With

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("MainForm.GetExpenditureFromGridViewRow: {0}", ex.Message)
            Logging.Logger.Log(ex, "MainForm.GetExpenditureFromGridViewRow: {0}", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

        Return expenditure

    End Function

#End Region

    ' DELETE
#Region "[ Button.Click: ButtonDeleteSelectedRow_Click ]"
    Protected Sub ButtonDeleteSelectedRow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteSelectedRow.Click

        RestoreLoadingDivPosition()

        Dim qry As String = String.Empty
        Dim parentExpenseIDs(GridView1.Rows.Count) As Integer
        Dim recordsDeleted As Integer = 0

        Try

            Dim rows As IEnumerable(Of GridViewRow) = From row In Me.GridView1.Rows Where CType(CType(row, GridViewRow).FindControl("CheckBoxSelectRow"), CheckBox).Checked Select CType(row, GridViewRow)

            Dim index As Integer = 0

            For Each a In rows

                Dim chkBox As CheckBox = CType(a.FindControl("CheckBoxSelectRow"), CheckBox)

                If chkBox.Checked Then
                    parentExpenseIDs(index) = chkBox.Attributes("rowid")

                    index = index + 1
                End If

            Next

            If index > 0 Then
                recordsDeleted = Me.ExpenseManager.DeleteParentExpense(parentExpenseIDs, qry)
            Else
                recordsDeleted = Me.ExpenseManager.DeleteParentExpense(New Integer() {HiddenRowID.Value}, qry)
            End If

            ScriptManager.RegisterClientScriptBlock(Me, ButtonDeleteSelectedRow.GetType, "msg", "alert('" & recordsDeleted & " " & GetTranslatedValue("rowshavebeendeleted", CurrentLanguage) & "');", True)

            Me.RebindGrid()

            Me.ButtonUpdate_Click(sender, e)
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.Delete, Me.UserID, Me.GetConnectionString, Request.UserHostAddress)

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "ButtonDeleteSelectedRow_Click(): " & ex.Message
            Logging.Logger.Log(ex, "ButtonDeleteSelectedRow_Click()", qry, UserID, GetConnectionString)
        End Try

    End Sub
#End Region

    ' ATTACH (For both GridViews !!!)
#Region "[ Button.Click: ButtonFileAttachConfirm_Click ]"
    Protected Sub ButtonFileAttachConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonFileAttachConfirm.Click

        Dim qry As String = String.Empty

        Try

            If Not FileUpload1.HasFile Then
                Environment.DisplayWebPageMessage(sender, Me.GetTranslatedValue("BrowseFileAlert", Me.CurrentLanguage))
                Return
            ElseIf FileUpload1.FileBytes.Length > AttachmentMaxSize Then
                Environment.DisplayWebPageMessage(sender, Me.GetTranslatedValue("AttachFileSizeAlert", Me.CurrentLanguage))
                Return
            End If

            Dim parAttachment As SqlParameter = New SqlParameter("Attachment", FileUpload1.FileBytes)
            Dim parAttachmentFileType As SqlParameter = New SqlParameter("AttachmentFileType", Path.GetExtension(FileUpload1.FileName))
            Dim parID As SqlParameter = New SqlParameter("ID", 0)

            If Me.HiddenAttachingToDetailsTable.Value Then
                qry = String.Format("UPDATE {0} SET Attachment = @Attachment, AttachmentFileType = @AttachmentFileType, HasAttachment = 1 WHERE ID = @ID", Me.DetailsTable)
                parID.Value = Me.HiddenDetailsRowID.Value
                Environment.ExecuteScript(sender, "ShowDetailsTable('', '', true);")
            Else
                qry = String.Format("UPDATE {0} SET Attachment = @Attachment, AttachmentFileType = @AttachmentFileType, HasAttachment = 1 WHERE ID = @ID", Me.MainTable)
                parID.Value = Me.HiddenRowID.Value
            End If

            DataBaseConnector.ExecuteQuery(qry, Me.GetConnectionString, parAttachment, parAttachmentFileType, parID)

            Me.RebindGrid()
            Me.RebindDetailsGrid(Me.ExpenditureID)

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("ButtonAttach_Click(): {0}", ex.Message)
            Logging.Logger.Log(ex, "ButtonAttach_Click()", qry, Me.UserID, Me.GetConnectionString)
        End Try
    End Sub
#End Region

    ' ADD DETAILS
#Region "[ Button.Click: ButtonAddDetails_Click ]"

    Protected Sub ButtonAddDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddDetails.Click

        Dim qry As String = String.Empty

        Try
            Dim hasSelectedARow As Boolean = False

            For i As Integer = 0 To GridView1.Rows.Count - 1

                If CType(GridView1.Rows(i).FindControl("CheckBoxSelectRow"), CheckBox).Checked Then

                    hasSelectedARow = True

                    qry = String.Format(
"UPDATE [dbo].[{0}] SET [HasDetails] = 1 WHERE ID = {1}", MainTable, CType(GridView1.Rows(i).FindControl("TextBoxID"), TextBox).Text)

                    DataBaseConnector.ExecuteQuery(qry, GetConnectionString)

                    GridView1.SelectedIndex = i
                    GridView1_SelectedIndexChanged(sender, e)
                    Exit For
                End If
            Next

            If Not hasSelectedARow Then
                Return
            End If

            GridViewDetails.PageIndex = Integer.MaxValue

            Me.RebindGrid()
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "scrollBottom", "<script language='javascript'>window.scrollTo(0,99999999)</script>", False)
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.AttachDetails, UserID, GetConnectionString, Request.UserHostAddress)

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "ButtonAddDetails_Click(): " & ex.Message
            Logging.Logger.Log(ex, "ButtonAddDetails_Click()", qry, UserID, GetConnectionString)
        End Try
    End Sub

#End Region

    ' DOWNLOAD ATTACHMENT
#Region "[ GridView1_RowEditing ]"
    Protected Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing
        Try

            Response.Redirect(URLRewriter.GetLink("DownloadAttachment", New KeyValuePair(Of String, String)("id", CType(GridView1.Rows(e.NewEditIndex).FindControl("TextBoxID"), TextBox).Text),
                                                                                         New KeyValuePair(Of String, String)("attachingToDetails", False)))
        Catch ex As ThreadAbortException
        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "GridView1_RowEditing" & ex.Message
            Logging.Logger.Log(ex, "GridView1_RowEditing", "none", UserID, GetConnectionString)
        End Try
    End Sub
#End Region

    ' DELETE ATTACHMENT
#Region "[ GridView1_RowDeleting ]"

    Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting

        Dim qry As String = String.Empty

        Try

            Me.ExpenseManager.DeleteAttachment(CType(GridView1.Rows(e.RowIndex).FindControl("TextBoxID"), TextBox).Text, qry)

            DataBaseConnector.ExecuteQuery(qry, GetConnectionString)

            Me.RebindGrid()
        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "GridView1_RowDeleting" & ex.Message
            Logging.Logger.Log(ex, "GridView1_RowDeleting", qry, UserID, GetConnectionString)
        End Try
    End Sub

#End Region

    ' DUPLICATE
#Region "[ ButtonStartDuplicate_Click ]"
    Protected Sub ButtonStartDuplicate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonStartDuplicate.Click

        Dim qry As String = String.Empty

        Try

            If drpDestinationMonth.SelectedValue <> Me.Month Then

                Me.ExpenseManager.DuplicateExpenditures(CInt(drpDestinationMonth.SelectedValue), CInt(drpYear.SelectedValue), chkDeleteExistingData.Checked, chkCopyFlaggedRecordsOnly.Checked, chkMarkUnpaid.Checked, chkZeroActualSums.Checked, qry)

                Me.Year = CInt(drpYear.SelectedValue)

                Me.Month = CInt(drpDestinationMonth.SelectedValue)

                Me.SetCurrentMonth(Me.Month)

                Me.SetCurrentYear(Me.Year)

            Else
                Environment.DisplayWebPageMessage(Me, Me.GetTranslatedValue("DuplicateMonthRecordsDestMonthMatchesNotify", Me.CurrentLanguage))
            End If

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("ButtonStartDuplicate_Click(): {0}; query: {1}", ex.Message, qry)
            Logging.Logger.Log(ex, "ButtonStartDuplicate_Click()", qry, UserID, GetConnectionString)
        End Try

    End Sub

#End Region

    '============================================================================================
    '           GridViewDetails functional controls
    '============================================================================================

    ' ADD
#Region "[ Button.Click: ButtonAddExpenditureDetails_Click ]"

    Protected Sub ButtonAddExpenditureDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddExpenditureDetails.Click, ButtonDetailsAddBottom.Click

        Try

            If GridViewDetails.FooterRow IsNot Nothing Then

                Dim textBoxDetailNameNew As TextBox = DirectCast(GridViewDetails.FooterRow.FindControl("TextBoxDetailNameNew"), TextBox)
                Dim textBoxDetailDescriptionNew As TextBox = DirectCast(GridViewDetails.FooterRow.FindControl("TextBoxDetailDescriptionNew"), TextBox)
                Dim textBoxDetailValueNew As TextBox = DirectCast(GridViewDetails.FooterRow.FindControl("TextBoxDetailValueNew"), TextBox)
                Dim textBoxDetailAmountNew As TextBox = DirectCast(GridViewDetails.FooterRow.FindControl("TextBoxDetailAmountNew"), TextBox)

                ' If no input just do a SAVE and do not add an empty record
                If String.IsNullOrWhiteSpace(textBoxDetailValueNew.Text) AndAlso String.IsNullOrWhiteSpace(textBoxDetailNameNew.Text) AndAlso String.IsNullOrWhiteSpace(textBoxDetailDescriptionNew.Text) Then
                    Me.ButtonUpdateDetailsTable_Click(sender, e)
                    Return
                End If

                Dim count As Decimal = 1D

                If IsNumeric(textBoxDetailAmountNew.Text) Then
                    count = CDec(textBoxDetailAmountNew.Text.Replace(",", "."))
                End If

                If count > 50 Then
                    Return
                End If

                Dim value As Decimal = CDec(IIf(IsNumeric(textBoxDetailValueNew.Text), textBoxDetailValueNew.Text.Replace(",", "."), 0))

                Dim expenditureDetail As ExpenditureDetail = New ExpenditureDetail(Me.ExpenseManager)

                With expenditureDetail

                    .ExpenditureID = Me.ExpenditureID
                    .DetailName = textBoxDetailNameNew.Text
                    .DetailDescription = textBoxDetailDescriptionNew.Text

                    .ProductID = Me.HiddenProductID.Value
                    .Product = New Product(.ProductID, Me.UserID, Me.GetConnectionString)
                    .SupplierID = Me.HiddenSelectedSupplier.Value
                    .MainTableName = Me.MainTable
                    .DetailsTableName = Me.DetailsTable
                    .ConnectionString = Me.GetConnectionString
                    .Connection = New SqlConnection(Me.GetConnectionString)
                    .UserID = Me.UserID
                    .Amount = count

                    Dim measureType As Enums.MeasureType = Enums.MeasureType.Count

                    If IsNumeric(Me.HiddenDetailsMeasureTypeID.Value) Then
                        measureType = CType(Me.HiddenDetailsMeasureTypeID.Value, Enums.MeasureType)
                    End If

                    .MeasureType = measureType

                    Dim transactionSum As Decimal = 0D

                    Select Case measureType

                        Case Enums.MeasureType.Volume, Enums.MeasureType.Weight
                            .DetailValue = count * value
                            .Add()
                            transactionSum = .DetailValue
                            Exit Select
                        Case Enums.MeasureType.Count
                            .DetailValue = value
                            For index = 1 To CInt(count)
                                Dim newDetailID As Integer = .Add()

                                If count > 1 Then
                                    Me.LastAddedMultipleDetailsIDs.Add(newDetailID)
                                End If
                            Next
                            transactionSum = value * CInt(count)
                            Exit Select
                        Case Else
                            .DetailValue = value
                            .Add()
                            transactionSum = .DetailValue
                            Exit Select

                    End Select

                    'If transactionSum > 0 Then

                    '    Dim parent As Expenditure = Me.MainTableDataSource.FirstOrDefault(Function(p) p.ID = .ExpenditureID)

                    '    Dim sumDetails As Decimal = Me.DetailsDataSource.Where(Function(d) d.ExpenditureID = .ExpenditureID).Sum(Function(d) d.DetailValue) + transactionSum

                    '    Me.AddTransaction(parent.ID, sumDetails, parent.FieldValue, parent.FieldName)

                    '    Me.RecordTransactions()

                    'End If

                End With

                ' Clear textboxes
                textBoxDetailNameNew.Text = String.Empty
                textBoxDetailDescriptionNew.Text = String.Empty
                textBoxDetailValueNew.Text = String.Empty

                ' Clear weight/volume/count radio
                Me.RadioButtonListWeightVolumeSelector.SelectedIndex = 2
                HiddenDetailsMeasureTypeID.Value = MeasureType.Count
                Me.ZeroSelectedProductID()

                Me.RebindGrid()

                GridViewDetails.PageIndex = Integer.MaxValue

                Me.RebindDetailsGrid(Me.ExpenditureID)

            End If
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.AddDetails, UserID, GetConnectionString, Request.UserHostAddress)

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("ButtonAddExpenditureDetails_Click(): {0}", ex.Message)
            Logging.Logger.Log(ex, "ButtonAddExpenditureDetails_Click()", String.Empty, UserID, GetConnectionString)
        Finally
            Environment.ExecuteScript(sender, "$(""[id$=HiddenDetailsMeasureTypeID]"").val(""3"");", True)
        End Try
    End Sub

#End Region

    ' SAVE
#Region "[ Button.Click: ButtonUpdateDetailsTable_Click ]"

    Private Sub SaveGridViewDetailsChanges()

        Try

            Dim expensesToUpdate As List(Of ExpenditureDetail) = New List(Of ExpenditureDetail)()

            For Each row As GridViewRow In GridViewDetails.Rows

                Dim detailName As String = String.Empty
                Dim detailDescription As String = String.Empty
                Dim detailValue As Decimal = 0.0
                Dim detailDate As String = String.Empty
                Dim id As String = String.Empty
                Dim isSelected As Boolean = False
                Dim mileage As Decimal = 0.0
                Dim amount As Decimal = 0.0

                With row

                    detailName = CType(.FindControl("TextBoxDetailsTableFieldName"), TextBox).Text
                    detailDescription = CType(.FindControl("TextBoxDetailsTableFieldDescription"), TextBox).Text

                    If IsNumeric(CType(.FindControl("TextBoxDetailsTableFieldValue"), TextBox).Text) Then
                        detailValue = CDec(CType(.FindControl("TextBoxDetailsTableFieldValue"), TextBox).Text.Replace(",", "."))
                    Else
                        detailValue = 0
                    End If

                    detailDate = CType(.FindControl("TextBoxFieldDetailsTableDate"), TextBox).Text
                    id = CType(.FindControl("TextBoxDetailsTableID"), TextBox).Text

                    isSelected = CType(.FindControl("CheckBoxDetailsTableSelect"), CheckBox).Checked

                    If IsNumeric(CType(.FindControl("TextBoxDetailsTableMileage"), TextBox).Text) Then
                        mileage = CDec(CType(.FindControl("TextBoxDetailsTableMileage"), TextBox).Text.Replace(",", "."))
                    Else
                        mileage = 0
                    End If

                    If IsNumeric(CType(.FindControl("TextBoxDetailsTableFieldAmount"), TextBox).Text) Then
                        amount = CDec(CType(.FindControl("TextBoxDetailsTableFieldAmount"), TextBox).Text.Replace(",", "."))
                    Else
                        amount = 0
                    End If

                End With

                If id = 0 Then Continue For

                Dim originalChildExpense As ExpenditureDetail = Me.DetailsDataSource.FirstOrDefault(Function(d) d.ID = id)

                Dim newChildExpense As ExpenditureDetail = New ExpenditureDetail()

                newChildExpense.ProductID = originalChildExpense.ProductID
                newChildExpense.Product = New Product() With {.Parameters = Enumerable.Empty(Of ProductParameter)()}

                If mileage > 0 Then

                    newChildExpense.Product.Parameters =
                    {
                        New ProductParameter() With
                        {
                            .ProductParameterTypeID = ProductParameterType.Mileage,
                            .Key = "Mileage",
                            .Value = mileage,
                            .ParentID = originalChildExpense.ID,
                            .ProductID = newChildExpense.ProductID
                        }
                    }

                End If

                Dim originalParameterValue As IEnumerable(Of String) = Enumerable.Empty(Of String)()

                If originalChildExpense.Product IsNot Nothing AndAlso originalChildExpense.Product.Parameters.Any() Then
                    originalParameterValue = originalChildExpense.Product.Parameters.Select(Function(para) para.Value)
                End If

                Dim newParameterValue As IEnumerable(Of String) = Enumerable.Empty(Of String)()

                If newChildExpense.Product IsNot Nothing AndAlso newChildExpense.Product.Parameters.Any() Then
                    newParameterValue = newChildExpense.Product.Parameters.Select(Function(para) para.Value)
                End If

                If originalChildExpense.ForceUpdate = True OrElse originalChildExpense.Amount <> amount OrElse originalChildExpense.DetailName <> detailName OrElse originalChildExpense.DetailDescription <> detailDescription OrElse
                    Math.Ceiling(originalChildExpense.DetailValue * 100) / 100 <> Math.Ceiling(detailValue * 100) / 100 OrElse isSelected OrElse Not Enumerable.SequenceEqual(originalParameterValue, newParameterValue) Then

                    With newChildExpense

                        .DetailName = detailName
                        .DetailDescription = detailDescription
                        .DetailValue = CDec(IIf(IsNumeric(detailValue), detailValue, 0))
                        .Amount = amount
                        .DetailDate = DateTime.Now
                        .MeasureType = originalChildExpense.MeasureType

                        If isSelected AndAlso CInt(Me.HiddenSelectedSupplier.Value) <> originalChildExpense.SupplierID Then
                            .SupplierID = CInt(Me.HiddenSelectedSupplier.Value)
                        Else
                            .SupplierID = originalChildExpense.SupplierID
                        End If

                        .ExpenditureID = Me.ExpenditureID
                        .ID = CInt(id)

                    End With

                    expensesToUpdate.Add(newChildExpense)

                End If

            Next

            If expensesToUpdate.Any() Then

                If Me.LastAddedMultipleDetailsIDs.Any() Then

                    Dim autoUpdateDetailSource As ExpenditureDetail =
                        expensesToUpdate.FirstOrDefault(Function(d) Me.LastAddedMultipleDetailsIDs.Contains(d.ID))

                    If autoUpdateDetailSource IsNot Nothing Then

                        Dim autoUpdatedMultipleDetails As IEnumerable(Of ExpenditureDetail) =
                            Me.DetailsDataSource.Where(Function(d) Me.LastAddedMultipleDetailsIDs.Contains(d.ID) AndAlso d.ID <> autoUpdateDetailSource.ID)

                        For Each autoUpdatedDetail As ExpenditureDetail In autoUpdatedMultipleDetails
                            autoUpdatedDetail.DetailValue = autoUpdateDetailSource.DetailValue
                        Next

                        expensesToUpdate.AddRange(autoUpdatedMultipleDetails)

                    End If
                End If

                ' Update child expenses
                Me.ExpenseManager.UpdateChildExpenses(expensesToUpdate)

                Me.RebindGrid()

                ' Rebind the DetailsGrid - sets the calculated value of all fields to the selected row value of the parent GridView1
                Me.RebindDetailsGrid(Me.ExpenditureID)

            End If

            Logging.Logger.LogAction(Logging.Logger.HistoryAction.UpdateDetails, UserID, GetConnectionString, Request.UserHostAddress)
        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("ButtonUpdateDetailsTable_Click(): {0}", ex.Message)
            Logging.Logger.Log(ex, "ButtonUpdateDetailsTable_Click()", String.Empty, Me.UserID, Me.GetConnectionString)
        Finally
            Me.LastAddedMultipleDetailsIDs.Clear()
        End Try

    End Sub

    Protected Sub ButtonUpdateDetailsTable_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonUpdateDetailsTable.Click, ButtonDetailsSaveBottom.Click

        Me.SaveGridViewDetailsChanges()

    End Sub

    Private Function ExpenditureDetailProductParametersAreModified(ByVal originalExpenditureDetail As ExpenditureDetail, ByVal newExpenditureDetail As ExpenditureDetail)

        Dim result = False

        If originalExpenditureDetail.Product IsNot Nothing Then

            If originalExpenditureDetail.Product.Parameters.Any() Then
                result = Me.ProductParametersAreModified(originalExpenditureDetail.Product.Parameters, newExpenditureDetail.Product.Parameters)
            End If

        End If

        Return result

    End Function

    Private Function ProductParametersAreModified(ByVal originalParameters As IEnumerable(Of ProductParameter), ByVal newParameters As IEnumerable(Of ProductParameter))

        Dim result As Boolean = False

        For Each par As ProductParameter In originalParameters

            If newParameters.Any(Function(p) p.ProductParameterTypeID = par.ProductParameterTypeID AndAlso p.Value = par.Value) Then
                Continue For
            Else
                result = True
                Exit For
            End If

        Next

        Return result

    End Function

#End Region

    ' DELETE
#Region "[ Button.Click: ButtonDeleteFromDetailsTable_Click ]"
    Protected Sub ButtonDeleteFromDetailsTable_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDeleteFromDetailsTable.Click, BittonDetailsDeleteBottom.Click

        Dim qry As String = String.Empty

        Try

            Dim expensesToDelete As IEnumerable(Of ExpenditureDetail) =
                  From row As GridViewRow In GridViewDetails.Rows
                  Where row.FindControl("CheckBoxDetailsTableSelect") IsNot Nothing AndAlso
                        row.FindControl("TextBoxDetailsTableID") IsNot Nothing AndAlso
                        CType(row.FindControl("CheckBoxDetailsTableSelect"), CheckBox).Checked
                  Select Me.DetailsDataSource.FirstOrDefault(Function(d) d.ID = CInt(CType(row.FindControl("TextBoxDetailsTableID"), TextBox).Text))

            If expensesToDelete.Count = 0 Then
                Environment.DisplayWebPageMessage(Me, Me.GetTranslatedValue("DetailsMarkARowToDelete", Me.CurrentLanguage))
                Return
            End If

            'If expensesToDelete.Any() Then

            '    Dim parent As Expenditure = Me.MainTableDataSource.FirstOrDefault(Function(p) p.ID = expensesToDelete.FirstOrDefault().ExpenditureID)

            '    Dim sumDetails As Decimal = Me.DetailsDataSource.Except(expensesToDelete).Sum(Function(d) d.DetailValue)

            '    If parent.FieldValue <> sumDetails Then
            '        Me.AddTransaction(parent.ID, sumDetails, parent.FieldValue, parent.FieldName)
            '    End If

            '    Me.RecordTransactions()

            'End If

            Me.ExpenseManager.DeleteChildExpenses(expensesToDelete.ToArray(), qry)

            Me.RebindGrid()

            Me.RebindDetailsGrid(Me.ExpenditureID)

            Logging.Logger.LogAction(Logging.Logger.HistoryAction.DeleteDetails, Me.UserID, Me.GetConnectionString, Request.UserHostAddress)

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("ButtonDeleteFromDetailsTable_Click(): {0}", ex.Message)
            Logging.Logger.Log(ex, "ButtonDeleteFromDetailsTable_Click()", qry, Me.UserID, Me.GetConnectionString)
        End Try
    End Sub

#End Region

    ' DELETE ATTACHMENT
#Region "[ GridViewDetails_RowDeleting ]"
    Protected Sub GridViewDetails_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridViewDetails.RowDeleting
        Dim qry As String = ""
        Try

            qry = String.Format(
    "UPDATE [dbo].[{0}] SET [HasAttachment] = 0 WHERE ID = {1}", DetailsTable, CType(GridViewDetails.Rows(e.RowIndex).FindControl("TextBoxDetailsTableID"), TextBox).Text)

            DataBaseConnector.ExecuteQuery(qry, GetConnectionString)

            Me.RebindDetailsGrid(ExpenditureID)

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
            Response.Redirect(String.Format("DownloadAttachment.aspx?id={0}&attachingToDetails=true", CType(GridViewDetails.Rows(e.NewEditIndex).FindControl("TextBoxDetailsTableID"), TextBox).Text))
        Catch ex As ThreadAbortException
        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "GridViewDetails_RowEditing" & ex.Message
            Logging.Logger.Log(ex, "GridViewDetails_RowEditing", "none", Me.UserID, Me.GetConnectionString)
        End Try
    End Sub

#End Region

    ' CREATE NEW PRODUCT
#Region "[ ButtonAddNewDetailsProduct_Click ]"

    Protected Sub ButtonAddNewDetailsProduct_Click(ByVal sender As Object, e As EventArgs) Handles ButtonAddNewProduct.Click

        Try

            If String.IsNullOrWhiteSpace(TextBoxNewProductName.Text) OrElse
                String.IsNullOrWhiteSpace(TextBoxNewProductKeywords.Text) Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "enternewproductnameandkeys", "alert('Please enter name and/or keywords for your new product.');", True)
                Return
            End If

            Dim listPrice As Decimal = 0.0
            Dim standardCost As Decimal = 0.0
            Dim volume As Decimal = 0.0
            Dim weight As Decimal = 0.0

            If IsNumeric(TextBoxNewProductListPrice.Text) Then
                listPrice = CDec(TextBoxNewProductListPrice.Text.Replace(",", "."))
            End If

            If IsNumeric(TextBoxNewProductStandardCost.Text) Then
                standardCost = CDec(TextBoxNewProductStandardCost.Text.Replace(",", "."))
            End If

            If IsNumeric(TextBoxNewProductVolume.Text) Then
                volume = CDec(TextBoxNewProductVolume.Text.Replace(",", "."))
            End If

            If IsNumeric(TextBoxNewProductWeight.Text) Then
                weight = CDec(TextBoxNewProductWeight.Text.Replace(",", "."))
            End If

            Dim product As Product = New Product()

            With product

                .Name = TextBoxNewProductName.Text.Trim()
                .Description = TextBoxNewProductDescription.Text.Trim()
                .KeyWords = TextBoxNewProductKeywords.Text.Split(",")
                .ListPrice = listPrice
                .StandardCost = standardCost
                .Volume = volume
                .Weight = weight
                .UserID = Me.UserID
                .ConnectionString = Me.GetConnectionString
                .VendorID = DropDownListNewProductSupplier.SelectedValue
                .CategoryID = DropDownListNewProductCategory.SelectedValue

                Dim detail As ExpenditureDetail = Me.DetailsDataSource.FirstOrDefault(Function(d) d.ID = Me.HiddenDetailsRowID.Value)

                ' Adds the newly created product
                detail.ProductID = Me.ExpenseManager.AddProduct(product)

                ' Find similar products and attempt to categorize
                Dim existingProducts As List(Of ExpenditureDetail) = Me.DetailsDataSource.Where(Function(d) d.ID <> detail.ID AndAlso d.DetailName = .Name AndAlso d.DetailDescription = .Description).ToList()

                existingProducts.ForEach(Sub(d) d.ProductID = detail.ProductID)

                existingProducts.Add(detail)

                'Me.ExpenseManager.UpdateChildExpenses(New List(Of ExpenditureDetail) From {detail})
                Me.ExpenseManager.UpdateChildExpenses(existingProducts)

                Me.RebindGrid()

                Me.RebindDetailsGrid(detail.ExpenditureID)

                UpdatePanelDetails.Update()

            End With

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("ButtonAddNewDetailsProduct_Click:{0}", ex.Message)
            Logging.Logger.Log(ex, "ButtonAddNewDetailsProduct_Click", "none", Me.UserID, Me.GetConnectionString)
        Finally
            TextBoxNewProductName.Text = String.Empty
            TextBoxNewProductDescription.Text = String.Empty
            TextBoxNewProductKeywords.Text = String.Empty
            TextBoxNewProductListPrice.Text = String.Empty
            TextBoxNewProductStandardCost.Text = String.Empty
            TextBoxNewProductVolume.Text = String.Empty
            TextBoxNewProductWeight.Text = String.Empty
        End Try

    End Sub

#End Region

    ' PRINT SHOPPING LIST
#Region "[ Button.Click: ButtonDetailsPrintShoppingList ]"
    Protected Sub ButtonDetailsPrintShoppingList_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDetailsPrintShoppingList.Click, ButtonDetailsPrintShoppingListBottom.Click

        ' If shopping list is clear print all detail expenditures that are a product
        If (Me.ShoppingList.Count = 0) Then

            ' Get all checked(selected) details and project as ProductID|DetailID tuple;
            Dim selectedExpenseDetails As Tuple(Of Integer, Integer)() =
                (From row As GridViewRow In Me.GridViewDetails.Rows
                 Where row.RowType = DataControlRowType.DataRow AndAlso CType(row.FindControl("CheckBoxDetailsTableSelect"), CheckBox).Checked = True
                 Select New Tuple(Of Integer, Integer)(CInt(CType(row.FindControl("CheckBoxDetailsTableSelect"), CheckBox).Attributes("ProductID")),
                                                         CInt(CType(row.FindControl("CheckBoxDetailsTableSelect"), CheckBox).Attributes("DetailID")))).ToArray()

            Dim detailExpensesAndQuantities As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)()

            If selectedExpenseDetails.Length > 0 Then

                If HiddenPrintShoppingListDetailsIncludeAllProductEntries.Value = 1 Then
                    detailExpensesAndQuantities = Me.DetailsDataSource.Where(Function(d) selectedExpenseDetails.Select(Function(dp) dp.Item1).Contains(d.ProductID)).GroupBy(Function(d) d.ProductID).ToDictionary(Function(d) d.Key, Function(d) d.Count())
                Else
                    detailExpensesAndQuantities = Me.DetailsDataSource.Where(Function(d) d.ProductID <> Product.PRODUCT_DEFAULT_ID AndAlso selectedExpenseDetails.Select(Function(dp) dp.Item2).Contains(d.ID)).GroupBy(Function(d) d.ProductID).ToDictionary(Function(d) d.Key, Function(d) 1)
                End If
            Else
                detailExpensesAndQuantities = Me.DetailsDataSource.Where(Function(d) d.ProductID <> Product.PRODUCT_DEFAULT_ID).GroupBy(Function(d) d.ProductID).ToDictionary(Function(d) d.Key, Function(d) d.Count())
            End If

            For Each dc In detailExpensesAndQuantities
                Dim sum As Decimal = 0D

                If HiddenPrintShoppingListDetailsIncludeAllProductEntries.Value = 1 Then
                    sum = Me.DetailsDataSource.Where(Function(d) d.ProductID = dc.Key).Sum(Function(d) d.DetailValue)
                Else
                    sum = Me.DetailsDataSource.Where(Function(d) d.ProductID = dc.Key).Average(Function(d) d.DetailValue)
                End If

                Me.AddToShoppingList(dc.Value, dc.Key, sum)
            Next
        End If

        Dim filePath As String = Me.GenerateShoppingListFile(False)

        Response.Redirect(URLRewriter.GetLink("FileDownloader", New KeyValuePair(Of String, String)(GlobalVariableNames.FILE_NAME, filePath),
                                                                New KeyValuePair(Of String, String)(GlobalVariableNames.CONTENT_TYPE, HttpHeadersContentType.PDF)))

    End Sub
#End Region

    '==================================================================
    '           GridView1 EventHandlers
    '==================================================================

#Region "[ GridView1_RowCommand ]"

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        Try

            Select Case e.CommandName

                Case COMMAND_NAVIGATE_TO_BILL_DATE

                    If e.CommandArgument IsNot Nothing Then

                        Dim parameters As String() = e.CommandArgument.ToString().Split(",")

                        If parameters.Length > 0 Then

                            Me.Year = CInt(parameters(0))

                            Me.Month = CInt(parameters(1))

                            Me.SetCurrentMonth(Me.Month)

                            Me.SetCurrentYear(Me.Year)

                            Logging.Logger.LogAction(Logging.Logger.HistoryAction.NavigateToBillDate, Me.UserID, Me.GetConnectionString, Request.UserHostAddress)

                        End If

                    End If

                    Exit Select

                Case COMMAND_SHARE_MAIN_ATTACHMENT

                    If IsNumeric(e.CommandArgument) Then

                        Dim expenditure As Expenditure = Me.MainTableDataSource.FirstOrDefault(Function(exp) exp.ID = CInt(e.CommandArgument))

                        If expenditure IsNot Nothing Then

                            expenditure.IsShared = Not expenditure.IsShared

                            Me.ExpenseManager.UpdateParentExpenses(New List(Of Expenditure)(New Expenditure() {expenditure}), String.Empty)

                            Me.RebindGrid()

                            If expenditure.IsShared Then

                                Response.Redirect(URLRewriter.GetLink("DownloadAttachment", New KeyValuePair(Of String, String)("id", expenditure.ID),
                                                                                            New KeyValuePair(Of String, String)("attachingToDetails", False)))

                            End If

                        End If

                    End If

                    Exit Select

            End Select

        Catch ex As ThreadAbortException
        Catch ex As Exception
            Logging.Logger.Log(ex, "GridView1_RowCommand", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

    End Sub

#End Region

#Region "[ GridView1_RowDataBound ]"

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Try

            If (e.Row.RowType = DataControlRowType.Header) Then

                Dim CheckBoxSelectAllRows As CheckBox = DirectCast(e.Row.FindControl("CheckBoxSelectAllRows"), CheckBox)
                CheckBoxSelectAllRows.Attributes.Add("onclick", String.Format("javascript:SelectAllRows('{0}');", CheckBoxSelectAllRows.ClientID))

                Dim CheckBoxSelectAllPaid As CheckBox = DirectCast(e.Row.FindControl("chkSelectAllPaid"), CheckBox)
                CheckBoxSelectAllPaid.Attributes.Add("onclick", String.Format("javascript:SelectAllIsPaid('{0}');", CheckBoxSelectAllPaid.ClientID))
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim ID As Integer = DirectCast(e.Row.FindControl("TextBoxID"), TextBox).Text

                Dim hoverScript As String = String.Format(
"GridUndoIconHover('{0}');ShowToolTip(event, 'get last saved sum');", e.Row.FindControl("ImmageButtonRevertToLastSavedSum").ClientID)

                Dim blurScript As String = String.Format(
"GridUndoIconBlur('{0}');HideToolTip();", e.Row.FindControl("ImmageButtonRevertToLastSavedSum").ClientID)

                DirectCast(e.Row.FindControl("ImmageButtonRevertToLastSavedSum"), HtmlImage).Attributes.Add("onmouseover", hoverScript)
                DirectCast(e.Row.FindControl("ImmageButtonRevertToLastSavedSum"), HtmlImage).Attributes.Add("onmouseout", blurScript)

                DirectCast(e.Row.FindControl("ImmageButtonShowAddToSumDiv"), HtmlImage).Attributes.Add("onclick",
String.Format("EnableAddToSumDiv('{0}')", e.Row.FindControl("divAddToSum").ClientID))

                DirectCast(e.Row.FindControl("ImageButtonAddToCurrVal"), HtmlImage).Attributes.Add("onclick",
 String.Format("AddToCurrentSum('{0}', '{1}', '{2}')",
               e.Row.FindControl("TextBoxFieldValue").ClientID,
               e.Row.FindControl("TextBoxFieldValueAddition").ClientID,
               e.Row.FindControl("divAddToSum").ClientID))

                Dim HiddenUndoRedo As String = String.Format("HiddenUndoRedo_{0}", ID)

                DirectCast(e.Row.FindControl("ImmageButtonRevertToLastSavedSum"), HtmlImage).Attributes.Add("onclick",
String.Format("RevertToLastSum('{0}', '{1}', '{2}', '{3}')",
              e.Row.FindControl("TextBoxFieldValue").ClientID,
              e.Row.FindControl("TextBoxHiddenFieldValueOld").ClientID,
              e.Row.FindControl("TextBoxHiddenStoredCurrentValue").ClientID,
              HiddenUndoRedo))

                Dim txtDueDate As TextBox = DirectCast(e.Row.FindControl("TextBoxDueDate"), TextBox)
                Dim txtFieldExpectedValue As TextBox = DirectCast(e.Row.FindControl("TextBoxFieldExpectedValue"), TextBox)
                Dim txtDaysLeft As TextBox = DirectCast(e.Row.FindControl("TextBoxDaysLeft"), TextBox)
                Dim txtFieldValue As TextBox = DirectCast(e.Row.FindControl("TextBoxFieldValue"), TextBox)
                Dim txtFieldValueDifference As TextBox = DirectCast(e.Row.FindControl("TextBoxFieldValueDifference"), TextBox)
                Dim txtCategoryID As TextBox = DirectCast(e.Row.FindControl("TextBoxCategoryID"), TextBox)

                Dim imgDueDateWarning As System.Web.UI.WebControls.Image = DirectCast(e.Row.FindControl("ImageDueDateWarning"), System.Web.UI.WebControls.Image)
                Dim imgBillIsPaid As System.Web.UI.WebControls.Image = DirectCast(e.Row.FindControl("ImageBillIsPaid"), System.Web.UI.WebControls.Image)
                Dim imgDeleteAttachment As System.Web.UI.WebControls.Image = DirectCast(e.Row.FindControl("ImageDeleteAttachment"), System.Web.UI.WebControls.Image)
                Dim imgPending As System.Web.UI.WebControls.Image = DirectCast(e.Row.FindControl("ImagePendingIcon"), System.Web.UI.WebControls.Image)
                Dim imgHasAttachment As System.Web.UI.WebControls.Image = DirectCast(e.Row.FindControl("ImageHasAttachment"), System.Web.UI.WebControls.Image)

                Dim chkHasAttachment As CheckBox = DirectCast(e.Row.FindControl("CheckBoxHasAttachment"), CheckBox)
                Dim chkPaid As CheckBox = DirectCast(e.Row.FindControl("CheckBoxIsPaid"), CheckBox)
                Dim chkHasDetails As CheckBox = DirectCast(e.Row.FindControl("CheckBoxHasDetails"), CheckBox)
                Dim chkSelectRow As CheckBox = DirectCast(e.Row.FindControl("CheckBoxSelectRow"), CheckBox)

                Dim imgBtnCategory As ImageButton = DirectCast(e.Row.FindControl("ImageCategory"), ImageButton)
                Dim imgBtnPreviewAttachment As ImageButton = DirectCast(e.Row.FindControl("ImageButtonPreviewAttachment"), ImageButton)

                Dim imageButtonActive As ImageButton = DirectCast(e.Row.FindControl("ImageButtonActive"), ImageButton)
                Dim imageButtonInActive As ImageButton = DirectCast(e.Row.FindControl("ImageButtonInActive"), ImageButton)

                Dim LinkButtonTransactions As LinkButton = DirectCast(e.Row.FindControl("LinkButtonTransactions"), LinkButton)

                Dim flagged As Boolean = CBool(DataBinder.Eval(e.Row.DataItem, "Flagged"))

                'chkSelectRow.Attributes.Add("onclick", String.Format("javascript:SelectRow('{0}', '{1}', '{2}')", e.Row.ClientID, chkSelectRow.ClientID, chkPaid.ClientID))

                ' if TextBoxDueDate has a date entered and also the 'paid' flag is not marked
                If IsDate(txtDueDate.Text) AndAlso CDate(txtDueDate.Text).Year > 1910 Then

                    'if bill is overdue
                    If Convert.ToDateTime(txtDueDate.Text) < DateTime.Now() Then
                        imgDueDateWarning.Visible = True
                        txtDaysLeft.Text =
CInt((Convert.ToDateTime(txtDueDate.Text) - DateTime.Now()).TotalDays) & " " & GetTranslatedValue("daysoverdue", CurrentLanguage)
                        txtDaysLeft.ForeColor = Drawing.Color.Red

                    Else 'if bill is pending
                        imgPending.Visible = True
                        txtDaysLeft.Text =
CInt((Convert.ToDateTime(txtDueDate.Text) - DateTime.Now()).TotalDays) & " " & GetTranslatedValue("daysleft", CurrentLanguage)
                        txtDaysLeft.ForeColor = Drawing.Color.Goldenrod

                    End If
                Else

                    txtDueDate.Text = String.Empty
                End If

                If chkPaid.Checked Then 'if bill is marked paid
                    imgPending.Visible = False
                    imgDueDateWarning.Visible = False
                    imgBillIsPaid.Visible = True
                    txtDaysLeft.Text = GetTranslatedValue("Paid", CurrentLanguage)
                    txtDaysLeft.ForeColor = Drawing.Color.Green

                End If

                ' if expenditure has a details table
                If chkHasDetails.Checked Then
                    e.Row.FindControl("LinkButtonMainTableDetails").Visible = True
                    Dim TextBoxValue As TextBox =
                    txtFieldValue

                    TextBoxValue.ReadOnly = True
                    TextBoxValue.ToolTip = GetTranslatedValue("TextBoxValue.ToolTip", CurrentLanguage)
                    TextBoxValue.Attributes.Add("onclick", "javascript:alert('" & TextBoxValue.ToolTip & "');")
                    'e.Row.Attributes.Add("ondblclick", "javascript:window.open('BillDetails.aspx?id=" & ID & "','DetailsPopUp','location=0,status=0,scrollbars=1,resizable=1,width=680,height=600');return false;")

                    DirectCast(e.Row.FindControl("ImmageButtonShowAddToSumDiv"), HtmlImage).Src = "../Images/edit_add_placeholder.gif"
                    DirectCast(e.Row.FindControl("ImmageButtonShowAddToSumDiv"), HtmlImage).Attributes.Add("onclick", "javascript:alert('" & TextBoxValue.ToolTip & "');")

                Else
                    e.Row.FindControl("LinkButtonMainTableDetails").Visible = False
                End If

                LinkButtonTransactions.Attributes.Add("onclick", "javascript:window.open('Transactions.aspx?id=" & ID & "','TransactionsPopup','location=0,status=0,scrollbars=1,resizable=1,width=680,height=600');return false;")
                LinkButtonTransactions.ToolTip = GetTranslatedValue("LinkButtonTransactionsToolTip", CurrentLanguage)

                ' we strip the annoying 12:00 off the 'TextBoxDueDate'
                If IsDate(txtDueDate.Text) Then
                    txtDueDate.Text = Convert.ToDateTime(txtDueDate.Text).ToShortDateString()
                End If

                ' we check if the row has an attachment
                If chkHasAttachment.Checked Then
                    imgHasAttachment.Visible = True
                    imgDeleteAttachment.Visible = True
                    imgBtnPreviewAttachment.Visible = True
                End If

                ' we calculate the difference between expected cost and actual recorded sum
                If IsNumeric(txtFieldExpectedValue.Text) AndAlso IsNumeric(txtFieldValue.Text) Then
                    If CInt(txtFieldExpectedValue.Text) > 0 AndAlso CInt(txtFieldValue.Text) > 0 Then
                        txtFieldValueDifference.Text = String.Format("{0:f}", (CDbl(txtFieldExpectedValue.Text) - CDbl(txtFieldValue.Text)))
                        ' if difference is less than 0 we change the css (red colour)
                        If CInt(txtFieldValueDifference.Text) < 0 Then
                            txtFieldValueDifference.CssClass = "GridCellsCostDiffNOK"
                            ' if difference is positive we change the css (blue colour)
                        ElseIf CInt(txtFieldValueDifference.Text) > 0 Then
                            txtFieldValueDifference.CssClass = "GridCellsCostDiffOK"
                        Else
                            txtFieldValueDifference.CssClass = "GridCellsCostNoDiff"
                        End If
                    End If
                End If

                imgBtnCategory.Attributes.Add("onclick", "javascript:window.open('CostsCompare.aspx?costCategory=" & txtCategoryID.Text & "','CompareWin','location=0,status=0,scrollbars=0,resizable=1,width=1100,height=350'); ")

                Dim cat As Category =
    (From c As Category In Categories Where c.ID = CInt(txtCategoryID.Text) Select c).FirstOrDefault()

                If cat IsNot Nothing Then
                    imgBtnCategory.Visible = True
                    imgBtnCategory.ImageUrl = cat.IconPath
                End If

                If flagged Then
                    imageButtonActive.Visible = True
                    imageButtonInActive.Visible = False
                Else
                    imageButtonActive.Visible = False
                    imageButtonInActive.Visible = True
                End If

                imageButtonActive.Attributes.Add("ID", ID)
                imageButtonInActive.Attributes.Add("ID", ID)

                imageButtonActive.Attributes.Add("Flagged", flagged)
                imageButtonInActive.Attributes.Add("Flagged", flagged)

                txtFieldExpectedValue.Attributes.Add("onclick", String.Format("javascript:AddCellValue('{0}')", txtFieldExpectedValue.ClientID))
                txtFieldValue.Attributes.Add("onclick", String.Format("javascript:AddCellValue('{0}')", txtFieldValue.ClientID))
                'txtFieldValueDifference.Attributes.Add("onclick", String.Format("javascript:AddCellValue('{0}')", txtFieldValueDifference.ClientID))

                txtFieldExpectedValue.Attributes.Add("isCalc", False)
                txtFieldValue.Attributes.Add("isCalc", False)
                'txtFieldValueDifference.Attributes.Add("isCalc", False)

                txtDueDate.Dispose()
                txtFieldExpectedValue.Dispose()
                txtDaysLeft.Dispose()
                txtFieldValue.Dispose()
                txtFieldValueDifference.Dispose()
                txtCategoryID.Dispose()
                imgDueDateWarning.Dispose()
                imgBillIsPaid.Dispose()
                imgDeleteAttachment.Dispose()
                imgPending.Dispose()
                imgHasAttachment.Dispose()
                chkHasAttachment.Dispose()
                chkPaid.Dispose()
                chkHasDetails.Dispose()
                chkSelectRow.Dispose()
                imgBtnCategory.Dispose()
                imgBtnPreviewAttachment.Dispose()
                imageButtonActive.Dispose()
                imageButtonInActive.Dispose()
                LinkButtonTransactions.Dispose()
            End If

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("GridView1_RowDataBound:{0}", ex.Message)
            Logging.Logger.Log(ex, "GridView1_RowDataBound", "none", UserID, GetConnectionString)
        End Try
    End Sub

#End Region

#Region "[ GridView1_SelectedIndexChanged ]"
    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView1.SelectedIndexChanged

        Dim ID As Integer = CInt(CType(GridView1.SelectedRow.FindControl("TextBoxID"), TextBox).Text)

        GridViewDetails.PageIndex = Int32.MaxValue

        Me.RebindDetailsGrid(ID)

        Me.ExpenditureID = ID

    End Sub
#End Region

    '==================================================================
    '           GridViewDetails EventHandlers
    '==================================================================

#Region "[ GridViewDetails_RowDataBound ]"

    Protected Sub GridViewDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewDetails.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim expenditureDetail = DirectCast(e.Row.DataItem, ExpenditureDetail)

                If expenditureDetail.ID = 0 Then
                    e.Row.Visible = False
                End If

                Dim txtBoxDetailsTableFieldAmount As TextBox = DirectCast(e.Row.FindControl("TextBoxDetailsTableFieldAmount"), TextBox)

                If expenditureDetail.MeasureType = MeasureType.Volume OrElse expenditureDetail.MeasureType = MeasureType.Weight Then
                    txtBoxDetailsTableFieldAmount.Visible = True
                End If

                If expenditureDetail.Product IsNot Nothing AndAlso expenditureDetail.Product.IsFixedMeasureType = True Then
                    txtBoxDetailsTableFieldAmount.ReadOnly = True
                End If

                ' we check if the row has an attachment
                If expenditureDetail.HasAttachment = True Then
                    CType(e.Row.FindControl("ImageDetTblHasAttachment"), System.Web.UI.WebControls.Image).Visible = True
                    CType(e.Row.FindControl("ImageDetTblDeleteAttachment"), System.Web.UI.WebControls.Image).Visible = True
                End If

                Dim txtFieldDetailName As TextBox = DirectCast(e.Row.FindControl("TextBoxDetailsTableFieldName"), TextBox)

                Dim txtFieldDetailValue As TextBox = DirectCast(e.Row.FindControl("TextBoxDetailsTableFieldValue"), TextBox)

                With txtFieldDetailValue
                    .Attributes.Add("onclick", String.Format("javascript:AddCellValue('{0}');", txtFieldDetailValue.ClientID))
                    .Attributes.Add("isCalc", False)
                End With

                Dim txtFieldDetailAmount As TextBox = DirectCast(e.Row.FindControl("TextBoxDetailsTableFieldAmount"), TextBox)

                With txtFieldDetailAmount
                    .Attributes.Add("onclick", String.Format("javascript:AddCellValue('{0}');", txtFieldDetailAmount.ClientID))
                    .Attributes.Add("isCalc", False)
                End With

                Dim txtFieldDetailMileage As TextBox = DirectCast(e.Row.FindControl("TextBoxDetailsTableMileage"), TextBox)

                With txtFieldDetailMileage
                    .Attributes.Add("onclick", String.Format("javascript:AddCellValue('{0}');", txtFieldDetailMileage.ClientID))
                    .Attributes.Add("isCalc", False)
                End With

                Dim textBoxDetailsTableAverageConsumption As TextBox = DirectCast(e.Row.FindControl("TextBoxDetailsTableAverageConsumption"), TextBox)

                If expenditureDetail.CategoryID = Category.Fuel Then

                    If expenditureDetail.ProductID <> Product.PRODUCT_DEFAULT_ID AndAlso expenditureDetail.Product.Parameters IsNot Nothing Then

                        Dim milageParameter As ProductParameter = expenditureDetail.Product.Parameters.FirstOrDefault(Function(p) p.ProductParameterTypeID = ProductParameterType.Mileage)

                        If milageParameter IsNot Nothing Then
                            txtFieldDetailMileage.Text = milageParameter.Value
                            textBoxDetailsTableAverageConsumption.Visible = True
                        End If

                    End If

                End If

                If expenditureDetail.ProductID = Product.PRODUCT_DEFAULT_ID Then

                    Dim dropDownListDetailsGridProductSuggest As DropDownList = DirectCast(e.Row.FindControl("DropDownListDetailsGridProductSuggest"), DropDownList)

                    With dropDownListDetailsGridProductSuggest

                        Dim matchingProducts As Product() =
                            Me.Products.Where(Function(p) p.KeyWords.Union(p.OcrKeyWords).Any(Function(pkw) Me.IsMatchingProduct(txtFieldDetailName.Text, pkw))).OrderBy(Function(p) p.Name).ToArray()

                        If matchingProducts.Length > 0 Then

                            .Visible = True
                            .DataSource = matchingProducts
                            .DataValueField = "ID"
                            .DataTextField = "Name"
                            .DataBind()

                            e.Row.FindControl("ImageButtonDetailApproveSuggestedProduct").Visible = True
                            txtBoxDetailsTableFieldAmount.Visible = True

                        End If

                    End With
                Else
                    e.Row.FindControl("ImageButtonDetailRejectSuggestedProduct").Visible = True

                    If expenditureDetail.Product IsNot Nothing AndAlso expenditureDetail.Product.Category IsNot Nothing Then

                        Dim imgBtnCategory As ImageButton = DirectCast(e.Row.FindControl("ImageCategory"), ImageButton)

                        With imgBtnCategory
                            .Visible = True
                            '.Attributes.Add("onclick", Environment.GetOpenInCustomWindowScript(URLRewriter.GetLink("CostsCompare", New KeyValuePair(Of String, String)("costCategory", expenditureDetail.Product.CategoryID)), 0, 0, 0, 1, 1100, 350))
                            .ImageUrl = expenditureDetail.Product.Category.IconPath
                        End With

                    End If

                End If

            ElseIf e.Row.RowType = DataControlRowType.Footer Then

                If Me.DetailsDataSource.All(Function(d) d.CategoryID = Category.Fuel) Then

                    Dim labelDetailsTableAverageConsumption As Label = DirectCast(e.Row.FindControl("LabelDetailsTableAverageConsumption"), Label)

                    Dim amount As Decimal = Me.DetailsDataSource _
                            .Where(Function(d) d.HasProductParameters AndAlso d.Product.Parameters.Any(Function(p) p.ProductParameterTypeID = ProductParameterType.Mileage)) _
                            .Sum(Function(d) d.Amount)

                    Dim milage As Decimal = Me.DetailsDataSource _
                            .Where(Function(d) d.HasProductParameters = True AndAlso d.ProductID <> Product.PRODUCT_DEFAULT_ID) _
                            .SelectMany(Function(d) d.Product.Parameters) _
                            .Where(Function(p) p.ProductParameterTypeID = ProductParameterType.Mileage AndAlso IsNumeric(p.Value)) _
                            .Sum(Function(p) CDec(p.Value))

                    If milage > 0 AndAlso amount > 0 Then
                        labelDetailsTableAverageConsumption.Text = String.Format("{0:f}", (amount / milage) * 100)
                    End If

                End If
            End If
        Catch ex As Exception
            Logging.Logger.Log(ex, "GridViewDetails_RowDataBound", "none", UserID, GetConnectionString)
        End Try

    End Sub

    Private Function IsMatchingProduct(ByVal newProductName As String, ByVal keyword As String) As Boolean

        Dim enteredProductKeyWords As String() = newProductName.ToLower().Split(" ")

        Dim existingKeywords As String() = keyword.Split(" ")

        For Each k As String In enteredProductKeyWords

            If k.Length > 4 Then

                Dim mask As String = Regex.Replace(k.Trim().ToLower(), "(\w{1})$|^(\w{1})", String.Empty)

                If keyword.Contains(mask) Then
                    Return True
                End If

            Else

                If existingKeywords.Contains(k) Then
                    Return True
                End If

            End If

        Next

        Return False

    End Function

#End Region

#Region "[ GridViewDetails_RowCommand ]"

    Protected Sub GridViewDetails_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridViewDetails.RowCommand

        Try

            Dim detailID As Integer = CInt(e.CommandArgument)

            Dim selectedDetail As ExpenditureDetail = Me.DetailsDataSource.FirstOrDefault(Function(d) d.ID = detailID)

            'selectedDetail.Parent = Me.MainTableDataSource.FirstOrDefault(Function(p) p.ID = selectedDetail.ExpenditureID)

            Select Case e.CommandName

                Case COMMAND_COPY_PASTE_DETAIL

                    If selectedDetail IsNot Nothing Then

                        Me.ExpenseManager.AddNewChildExpense(selectedDetail)

                        Me.RebindGrid()

                        Me.RebindDetailsGrid(Me.ExpenditureID)

                    End If

                    Exit Select

                Case COMMAND_PRINT_SHOPPING_LIST_DETAIL

                    ' Key = ProductID; Value = quantity of that product in DetailsDataSource
                    Dim products As Dictionary(Of Integer, Integer) = Me.DetailsDataSource.Where(Function(d) d.ProductID = selectedDetail.ProductID).GroupBy(Function(d) d.ProductID).ToDictionary(Function(d) d.Key, Function(d) d.Count())

                    For Each kv As KeyValuePair(Of Integer, Integer) In products
                        Me.AddToShoppingList(kv.Value, kv.Key, Me.DetailsDataSource.Where(Function(d) d.ProductID = kv.Key).Sum(Function(d) d.DetailValue))
                    Next

                    Exit Select
                Case COMMAND_EDIT_DETAIL_PRODUCT

                    Response.Redirect(URLRewriter.GetLink("ProductsManagement", New KeyValuePair(Of String, String)("pid", selectedDetail.ProductID)))

                    Exit Select

                Case COMMAND_DETAIL_PRODUCT_PRICE_STATISTICS

                    Exit Select

                Case COMMAND_DETAIL_MERGE

                    Dim selectedRows As GridViewRow() = (From row As GridViewRow In GridViewDetails.Rows
                                                         Where CType(row.FindControl("CheckBoxDetailsTableSelect"), CheckBox).Checked
                                                         Select row).ToArray()

                    Dim selectedDetailIDs As Integer() = selectedRows.Select(Function(row) CInt(CType(row.FindControl("TextBoxDetailsTableID"), TextBox).Text)).ToArray()

                    Me.ExpenseManager.MergeDetails(selectedDetailIDs)

                    Me.RebindGrid()

                    Me.RebindDetailsGrid(Me.ExpenditureID)

                    Exit Select

                Case COMMAND_DETAIL_APPROVE_SUGGESTED_PRODUCT

                    Dim row As Control = DirectCast(e.CommandSource, Control).NamingContainer

                    Dim dropDownListSuggestedProduct As Control = row.FindControl("DropDownListDetailsGridProductSuggest")

                    If dropDownListSuggestedProduct IsNot Nothing Then

                        Dim dropDownListProductSuggestions As DropDownList = DirectCast(dropDownListSuggestedProduct, DropDownList)

                        Dim productId As Integer = dropDownListProductSuggestions.SelectedValue

                        If IsNumeric(productId) Then

                            selectedDetail.ProductID = productId

                            selectedDetail.ForceUpdate = True

                            If row.FindControl("TextBoxDetailsTableFieldName") IsNot Nothing Then

                                Dim textBoxDetailsTableFieldName As TextBox = DirectCast(row.FindControl("TextBoxDetailsTableFieldName"), TextBox)

                                Dim product As Product = Me.Products.FirstOrDefault(Function(p) p.ID = productId)

                                If product IsNot Nothing Then

                                    If selectedDetail.IsOcrScanned Then

                                        If Not product.OcrKeyWords.Contains(textBoxDetailsTableFieldName.Text) Then

                                            product.OcrKeyWords = product.OcrKeyWords.Union(New String() {textBoxDetailsTableFieldName.Text}).ToArray()

                                            Me.ExpenseManager.UpdateProduct(product)

                                        End If
                                    Else

                                        If Not product.KeyWords.Contains(textBoxDetailsTableFieldName.Text) Then

                                            product.KeyWords = product.KeyWords.Union(New String() {textBoxDetailsTableFieldName.Text}).ToArray()

                                            Me.ExpenseManager.UpdateProduct(product)

                                        End If
                                    End If

                                End If

                                If dropDownListProductSuggestions.SelectedItem IsNot Nothing Then
                                    textBoxDetailsTableFieldName.Text = dropDownListProductSuggestions.SelectedItem.Text
                                End If

                            End If

                            Me.SaveGridViewDetailsChanges()

                            Return
                        End If

                    End If

                    Exit Select

                Case COMMAND_DETAIL_REJECT_SUGGESTED_PRODUCT

                    selectedDetail.ProductID = Product.PRODUCT_DEFAULT_ID

                    selectedDetail.ForceUpdate = True

                    Me.SaveGridViewDetailsChanges()

                    Exit Select

                Case COMMAND_SHARE_DETAIL_ATTACHMENT

                    selectedDetail.IsShared = Not selectedDetail.IsShared

                    Me.ExpenseManager.UpdateChildExpenses(New List(Of ExpenditureDetail)(New ExpenditureDetail() {selectedDetail}))

                    Me.RebindDetailsGrid(Me.ExpenditureID)

                    If selectedDetail.IsShared Then

                        Response.Redirect(URLRewriter.GetLink("DownloadAttachment", New KeyValuePair(Of String, String)("id", selectedDetail.ID),
                                                                                    New KeyValuePair(Of String, String)("attachingToDetails", True)))
                    End If

                    Exit Select

                Case COMMAND_DETAIL_CHANGE_MEASURE_TYPE

                    If selectedDetail.MeasureType < [Enum].GetNames(GetType(MeasureType)).Length - 1 Then
                        selectedDetail.MeasureType = selectedDetail.MeasureType + 1
                    Else
                        selectedDetail.MeasureType = 1
                    End If

                    selectedDetail.ForceUpdate = True

                    Me.SaveGridViewDetailsChanges()

                    Exit Select

                Case COMMAND_MARK_SURPLUS

                    selectedDetail.IsSurplus = Not selectedDetail.IsSurplus

                    Me.ExpenseManager.UpdateChildExpenses(New List(Of ExpenditureDetail)(New ExpenditureDetail() {selectedDetail}))

                    Me.RebindGrid()

                    Me.RebindDetailsGrid(Me.ExpenditureID)

                    Exit Select

            End Select

            'Dim newValue As Decimal = Me.MainTableDataSource.FirstOrDefault(Function(m) m.ID = selectedDetail.ExpenditureID).FieldValue

            'If newValue <> selectedDetail.Parent.FieldValue Then
            '    Me.AddTransaction(selectedDetail.ExpenditureID, newValue, selectedDetail.Parent.FieldValue, selectedDetail.Parent.FieldName)
            '    Me.RecordTransactions()
            'End If

        Catch ex As ThreadAbortException
        Catch ex As Exception
            Logging.Logger.Log(ex, "GridViewDetails_RowCommand", String.Format("e.CommandName:{0};e.CommandArgument:{1};e.CommandSource:{2};", e.CommandName, e.CommandArgument, e.CommandSource), Me.UserID, Me.GetConnectionString)
        End Try
    End Sub

#End Region

    ' Show ADD NEW PRODUCT popup
#Region "[ GridViewDetails_SelectedIndexChanged ]"

    Protected Sub GridViewDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridViewDetails.SelectedIndexChanged

        Try

            Dim ID As Integer = CInt(CType(GridViewDetails.SelectedRow.FindControl("TextBoxDetailsTableID"), TextBox).Text)

            Dim detail As ExpenditureDetail = Me.DetailsDataSource.FirstOrDefault(Function(d) d.ID = ID)

            TextBoxNewProductName.Text = detail.DetailName
            TextBoxNewProductDescription.Text = detail.DetailDescription
            TextBoxNewProductKeywords.Text = detail.DetailName
            TextBoxNewProductListPrice.Text = detail.DetailValue
            TextBoxNewProductStandardCost.Text = detail.DetailValue

            Call Me.RebindSuppliersDropDown()
            Call Me.RebindCategoriesDropDown()

            Environment.SetDropDownSelectedValue(Me.DropDownListNewProductSupplier, CInt(Me.HiddenSelectedSupplier.Value))

            Dim detectedCategoryID As Integer = Me.GetCategoryID(String.Join(",", detail.DetailName, detail.DetailDescription))

            If detectedCategoryID = Category.CATEGORY_DEFAULT_ID Then

                Dim kw As String() = detail.DetailName.Split(New Char() {" ", ","}).Union(detail.DetailDescription.Split(New Char() {" ", ","})).Where(Function(k) Not String.IsNullOrWhiteSpace(k)).ToArray()

                Dim kwTranslit As String() = kw.Select(Function(k) Me.TransliterateText(k).Item2).ToArray()

                Dim enteredKeywords As String() = kw.Union(kwTranslit).Select(Function(k) k.ToLower()).ToArray()

                Dim matchingExistingProduct As Product =
                    Me.Products.FirstOrDefault(Function(p) _
                                                   p.KeyWords.Any(Function(k) _
                                                                      enteredKeywords.Any(Function(ek) k.ToLower().Contains(ek) OrElse ek.Contains(k.ToLower()))))

                If matchingExistingProduct IsNot Nothing Then
                    detectedCategoryID = matchingExistingProduct.CategoryID
                End If

            End If

            Environment.SetDropDownSelectedValue(Me.DropDownListNewProductCategory, detectedCategoryID)

        Catch ex As Exception
            Logging.Logger.Log(ex, "GridViewDetails_SelectedIndexChanged", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try
    End Sub

#End Region

    Protected Sub GridViewSurplusAndLastEditedItems_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridViewSurplusItems.RowDataBound, GridViewLastEditedItems.RowDataBound

        If e.Row.RowType = DataControlCellType.Footer Then

            Dim gridView As GridView = CType(sender, GridView)

            Dim dataSource As IEnumerable(Of ExpenditureDetail) = CType(gridView.DataSource, IEnumerable(Of ExpenditureDetail))

            Dim sumText = String.Format("{0:0.00} {1}", dataSource.Sum(Function(d) d.DetailValue), Me.Currency)

            Dim labelTotalSumSurplusItems As Label = e.Row.FindControl("LabelTotalSumSurplusItems")

            If labelTotalSumSurplusItems IsNot Nothing Then
                labelTotalSumSurplusItems.Text = sumText
            End If

            Dim labelTotalSumLastEditedItems As Label = e.Row.FindControl("LabelTotalSumLastEditedItems")

            If labelTotalSumLastEditedItems IsNot Nothing Then
                labelTotalSumLastEditedItems.Text = sumText
            End If

        End If

    End Sub

    '============================================================
    '           Layout functionality (not relevant)
    '============================================================

#Region "[ FillYearsDropDownList ]"
    Protected Sub FillYearsDropDownList()

        For year As Integer = Date.Now.Year - 3 To Date.Now.Year + 4
            drpYear.Items.Add(year)
        Next

    End Sub
#End Region

#Region "[ RebindGridOnSearchPerformed ]"
    Protected Sub RebindGridOnSearchPerformed() Handles SearchControl.SearchPerformed

        If SearchControl.SearchResult.Count > 0 Then
            Me.IsInSearchMode = True
            Me.RebindGrid(Me.SearchControl.SearchResult)
        Else
            Environment.DisplayWebPageMessage(Me.SearchControl, Me.GetTranslatedValue("nosearchrecordsfound", Me.CurrentLanguage))
            Exit Sub
        End If

    End Sub
#End Region

#Region "[ Sub: SetSelectedTabButton ]"

    Protected Sub SetSelectedTabButton(ByVal selectedButtonID As String)
        Try
            For Each ctl As Control In PanelTabMenu.Controls
                If TypeOf (ctl) Is Button Then
                    If CType(ctl, Button).ID.Equals(selectedButtonID) Then
                        CType(ctl, Button).CssClass = "SelectedTabButton"
                    Else
                        CType(ctl, Button).CssClass = "NormalTabButton"
                    End If
                End If
            Next

            Me.Month = CInt(selectedButtonID.Replace("Button", String.Empty))

            ' sets the text of the label under the tabbuttons (Example: 2009, January)
            Me.SetCurrentMonthLabelText(Me.Month)

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "SetSelectedTabButton(): " & ex.Message
            Logging.Logger.Log(ex, "SetSelectedTabButton()", "none", UserID, GetConnectionString)
        End Try

    End Sub

#End Region

#Region "[ GetSuggestedProductAveragePrice ]"

    Private Function GetSuggestedProductAveragePrice(ByVal productID As Integer) As Decimal

        Dim averageResult As Decimal = 0D

        Try

            Dim allSpentOnSelectedProductThisMonth As ProductInfo() =
                Me.MainTableDataSource _
                .SelectMany(Function(m) m.Details).Where(Function(d) d.ProductID = productID) _
                .Select(Function(d) New ProductInfo() With {.ID = d.ProductID, .Amount = d.Amount, .Total = d.DetailValue, .MeasureType = d.MeasureType}).ToArray()

            If allSpentOnSelectedProductThisMonth.Any() Then

                If allSpentOnSelectedProductThisMonth.All(Function(p) p.MeasureType = Enums.MeasureType.Volume OrElse p.MeasureType = Enums.MeasureType.Weight) Then
                    averageResult = allSpentOnSelectedProductThisMonth.Average(Function(p) p.Total / p.Amount)
                Else
                    averageResult = allSpentOnSelectedProductThisMonth.Average(Function(p) p.Total)
                End If

            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "GetSuggestedProductAveragePrice", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

        Return averageResult

    End Function

#End Region

#Region "[ RebindSuppliersDropDown ]"

    Private Sub RebindSuppliersDropDown()
        With DropDownListNewProductSupplier
            .DataTextField = "Name"
            .DataValueField = "ID"
            .DataSource = Me.ExpenseManager.Suppliers.OrderBy(Function(s) s.Name)
            .DataBind()
        End With

    End Sub

#End Region

#Region "[ RebindCategoriesDropDown ]"

    Private Sub RebindCategoriesDropDown()
        With DropDownListNewProductCategory
            .DataTextField = "Name"
            .DataValueField = "ID"
            .DataSource = Me.Categories.OrderBy(Function(c) c.Name)
            .DataBind()
        End With
    End Sub

#End Region

#Region "[ Sub: SetCurrentMonthLabelText() ]"

    Protected Sub SetCurrentMonthLabelText(ByVal month As Integer)
        Select Case month
            Case 1
                LabelCurrentMonth.Text = Year.ToString() & ", " & Button1.Text
                Exit Select
            Case 2
                LabelCurrentMonth.Text = Year.ToString() & ", " & Button2.Text
                Exit Select
            Case 3
                LabelCurrentMonth.Text = Year.ToString() & ", " & Button3.Text
                Exit Select
            Case 4
                LabelCurrentMonth.Text = Year.ToString() & ", " & Button4.Text
                Exit Select
            Case 5
                LabelCurrentMonth.Text = Year.ToString() & ", " & Button5.Text
                Exit Select
            Case 6
                LabelCurrentMonth.Text = Year.ToString() & ", " & Button6.Text
                Exit Select
            Case 7
                LabelCurrentMonth.Text = Year.ToString() & ", " & Button7.Text
                Exit Select
            Case 8
                LabelCurrentMonth.Text = Year.ToString() & ", " & Button8.Text
                Exit Select
            Case 9
                LabelCurrentMonth.Text = Year.ToString() & ", " & Button9.Text
                Exit Select
            Case 10
                LabelCurrentMonth.Text = Year.ToString() & ", " & Button10.Text
                Exit Select
            Case 11
                LabelCurrentMonth.Text = Year.ToString() & ", " & Button11.Text
                Exit Select
            Case 12
                LabelCurrentMonth.Text = Year.ToString() & ", " & Button12.Text
                Exit Select
        End Select

    End Sub
#End Region

#Region "[ Sub: ShowSetCurrentLanguageReminder() ]"

    Public Sub ShowSetCurrentLanguageReminder()
        Select Case CurrentLanguage
            Case "bg"
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "setLangBGFromSettingsPage", "<script language='javascript'>alert('Моля настройте езика си по подразбиране от страницата настройки.')</script>", False)
                'ClientScript.RegisterStartupScript(Me.GetType, "setLangBGFromSettingsPage", "<script language='javascript'>alert('Моля настройте езика си по подразбиране от страницата настройки.')</script>")
                Exit Select
            Case "en"
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "setLangENFromSettingsPage", "<script language='javascript'>alert('Please set your default language settings from the settings page.')</script>", False)
                'ClientScript.RegisterStartupScript(Me.GetType, "setLangBGFromSettingsPage", "<script language='javascript'>alert('Please set your default language settings from the settings page.')</script>")
                Exit Select
            Case "de"
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "setLangENFromSettingsPage", "<script language='javascript'>alert('Bitte wählen Sie Ihre Standard-Sprache in der Einstellungen-Seite.')</script>", False)
                'ClientScript.RegisterStartupScript(Me.GetType, "setLangBGFromSettingsPage", "<script language='javascript'>alert('Please set your default language settings from the settings page.')</script>")
                Exit Select
        End Select
    End Sub
#End Region

    ' Row UP
#Region "[ ImageButton.Click: ImageButtonUp_Click() ]"

    Protected Sub ImageButtonUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonUp.Click

        Dim qry As String = String.Empty

        Dim sqlParameters As List(Of SqlParameter) = New List(Of SqlParameter)()

        Try

            For i As Integer = 1 To GridView1.Rows.Count - 1

                If CType(GridView1.Rows(i).FindControl("CheckBoxSelectRow"), CheckBox).Checked Then

                    Dim firstOrderID As Integer = CInt(CType(GridView1.Rows(i - 1).FindControl("TextBoxOrderID"), TextBox).Text)
                    Dim firstID As Integer = CInt(CType(GridView1.Rows(i).FindControl("TextBoxID"), TextBox).Text)

                    Dim secondOrderID As Integer = CInt(CType(GridView1.Rows(i).FindControl("TextBoxOrderID"), TextBox).Text)
                    Dim secondID As Integer = CInt(CType(GridView1.Rows(i - 1).FindControl("TextBoxID"), TextBox).Text)

                    qry &= String.Format("UPDATE {0} SET OrderID = @FirstOrderID{1} WHERE ID = @FirstID{1}" & vbCrLf, Me.MainTable, i)
                    qry &= String.Format("UPDATE {0} SET OrderID = @SecondOrderID{1} WHERE ID = @SecondID{1}" & vbCrLf, Me.MainTable, i)

                    sqlParameters.Add(New SqlParameter(String.Format("FirstOrderID{0}", i), firstOrderID))
                    sqlParameters.Add(New SqlParameter(String.Format("SecondOrderID{0}", i), secondOrderID))

                    sqlParameters.Add(New SqlParameter(String.Format("FirstID{0}", i), firstID))
                    sqlParameters.Add(New SqlParameter(String.Format("SecondID{0}", i), secondID))

                End If

            Next

            If Not String.IsNullOrEmpty(qry) Then

                DataBaseConnector.ExecuteQuery(qry, Me.GetConnectionString, sqlParameters.ToArray())

                Me.RebindGrid()

            End If

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "ImageButtonUp_Click(): " & ex.Message
            Logging.Logger.Log(ex, "ImageButtonUp_Click()", qry, UserID, GetConnectionString)
        End Try

    End Sub
#End Region

    ' Row DOWN
#Region "[ ImageButton.Click: ImageButtonDown_Click() ]"

    Protected Sub ImageButtonDown_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonDown.Click

        Dim qry As String = String.Empty

        Try

            Dim sqlParameters As List(Of SqlParameter) = New List(Of SqlParameter)()

            For i As Integer = 0 To GridView1.Rows.Count - 2

                If CType(GridView1.Rows(i).FindControl("CheckBoxSelectRow"), CheckBox).Checked Then

                    Dim firstOrderID As Integer = CInt(CType(GridView1.Rows(i + 1).FindControl("TextBoxOrderID"), TextBox).Text)
                    Dim firstID As Integer = CInt(CType(GridView1.Rows(i).FindControl("TextBoxID"), TextBox).Text)

                    Dim secondOrderID As Integer = CInt(CType(GridView1.Rows(i).FindControl("TextBoxOrderID"), TextBox).Text)
                    Dim secondID As Integer = CInt(CType(GridView1.Rows(i + 1).FindControl("TextBoxID"), TextBox).Text)

                    qry &= String.Format("UPDATE {0} SET OrderID = @FirstOrderID{1} WHERE ID = @FirstID{1}" & vbCrLf, Me.MainTable, i)
                    qry &= String.Format("UPDATE {0} SET OrderID = @SecondOrderID{1} WHERE ID = @SecondID{1}" & vbCrLf, Me.MainTable, i)

                    sqlParameters.Add(New SqlParameter(String.Format("FirstOrderID{0}", i), firstOrderID))
                    sqlParameters.Add(New SqlParameter(String.Format("SecondOrderID{0}", i), secondOrderID))

                    sqlParameters.Add(New SqlParameter(String.Format("FirstID{0}", i), firstID))
                    sqlParameters.Add(New SqlParameter(String.Format("SecondID{0}", i), secondID))

                End If
            Next

            If Not String.IsNullOrEmpty(qry) Then

                DataBaseConnector.ExecuteQuery(qry, Me.GetConnectionString, sqlParameters.ToArray())

                Me.RebindGrid()

            End If

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "ImageButtonDown_Click(): " & ex.Message
            Logging.Logger.Log(ex, "ImageButtonDown_Click()", qry, UserID, GetConnectionString)
        End Try
    End Sub
#End Region

    ' Marks the row in GridView1 when you click details
#Region "[ Sub: SetSelectedRowStyle() ]"
    Protected Sub SetSelectedRowStyle(ByVal container As ControlCollection)
        Try

            'For Each ctl As Control In container
            '    If TypeOf ctl Is TextBox Then
            '        CType(ctl, TextBox).CssClass = "GridCellsSelectedRowStyle"
            '    ElseIf ctl.HasControls Then
            '        SetSelectedRowStyle(ctl.Controls)
            '    End If
            'Next
        Catch ex As Exception
            Logging.Logger.Log(ex, "SetSelectedRowStyle()", "", UserID, GetConnectionString)
        End Try
    End Sub
#End Region

    ' redirect Charts.aspx
#Region "[ Button.Click: ButtonAnnualSummary_Click ]"
    Protected Sub ButtonAnnualSummary_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAnnualSummary.Click
        'ClientScript.RegisterStartupScript(Me.GetType(), "summary", "<script language=javascript>window.open ('Charts.aspx','Summary','menubar=0,resizable=1,scrolling=1,width=940,height=600');</script>")
        'If Not PopUpAlertIsShown Then
        '    ClientScript.RegisterStartupScript(Me.GetType(), "alertPopUp", "<script language=javascript>alert('Click the bar above and choose allow popups to show the Annual Summary page.')</script>")
        '    PopUpAlertIsShown = True
        'End If

        RestoreLoadingDivPosition()
        Logging.Logger.LogAction(Logging.Logger.HistoryAction.AnnualReportCharts, UserID, GetConnectionString, Request.UserHostAddress)
        Response.Redirect(URLRewriter.GetLink("Charts"))

    End Sub
#End Region

    ' redirect Statistics.aspx
#Region "[ Button.Click: ButtonStatistics_Click ]"
    Protected Sub ButtonStatistics_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonStatistics.Click
        'ClientScript.RegisterStartupScript(Me.GetType(), "summary", "<script language=javascript>window.open ('Charts.aspx','Summary','menubar=0,resizable=1,scrolling=1,width=940,height=600');</script>")
        'If Not PopUpAlertIsShown Then
        '    ClientScript.RegisterStartupScript(Me.GetType(), "alertPopUp", "<script language=javascript>alert('Click the bar above and choose allow popups to show the Annual Summary page.')</script>")
        '    PopUpAlertIsShown = True
        'End If

        RestoreLoadingDivPosition()
        Logging.Logger.LogAction(Logging.Logger.HistoryAction.Statistics, UserID, GetConnectionString, Request.UserHostAddress)
        Response.Redirect(URLRewriter.GetLink("Statistics"))

    End Sub
#End Region

    ' redirect CategoriesYearlyCharts.aspx
#Region "[ Button.Click: ButtonAnnualSummary_Click ]"
    Protected Sub ButtonAnnualSummaryPerCategory_Click(sender As Object, e As EventArgs) Handles ButtonAnnualSummaryPerCategory.Click
        RestoreLoadingDivPosition()
        Logging.Logger.LogAction(Logging.Logger.HistoryAction.AnnualCategoryCharts, UserID, GetConnectionString, Request.UserHostAddress)
        Response.Redirect(URLRewriter.GetLink("AnnualCategoryCharts"))
    End Sub
#End Region

    ' select all
#Region "[ LinkButton.Click: LinkButtonSelectAll_Click ]"

    Protected Sub LinkButtonSelectAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonSelectAll.Click
        Try

            For i As Integer = 0 To GridView1.Rows.Count - 1
                If GridView1.Rows(i).RowType = DataControlRowType.DataRow Then
                    If Not CType(GridView1.Rows(i).FindControl("CheckBoxSelectRow"), CheckBox).Checked Then
                        CType(GridView1.Rows(i).FindControl("CheckBoxSelectRow"), CheckBox).Checked = True
                    End If
                End If
            Next
        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "LinkButtonSelectAll_Click" & ex.Message
            Logging.Logger.Log(ex, "LinkButtonSelectAll_Click", "none", UserID, GetConnectionString)
        End Try
    End Sub

#End Region

    ' deselect all
#Region "[ LinkButton.Click: LinkButtonDeselectAll_Click ]"

    Protected Sub LinkButtonDeselectAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonDeselectAll.Click

        Try

            For i As Integer = 0 To GridView1.Rows.Count - 1
                If GridView1.Rows(i).RowType = DataControlRowType.DataRow Then
                    If CType(GridView1.Rows(i).FindControl("CheckBoxSelectRow"), CheckBox).Checked Then
                        CType(GridView1.Rows(i).FindControl("CheckBoxSelectRow"), CheckBox).Checked = False
                    End If
                End If
            Next
        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "LinkButtonDeselectAll_Click(): " & ex.Message
            Logging.Logger.Log(ex, "LinkButtonDeselectAll_Click", "none", UserID, GetConnectionString)
        End Try

    End Sub

#End Region

    ' shows the category comparison popup chart
#Region "[ ButtonAnnualReportForExport_Click ]"
    Protected Sub ButtonAnnualReportForExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAnnualReportForExport.Click
        Logging.Logger.LogAction(Logging.Logger.HistoryAction.AnnualReportPDFExcel, UserID, GetConnectionString, Request.UserHostAddress)
        Response.Redirect(String.Format("{0}?month={1}&year={2}&userid={3}", URLRewriter.GetLink("Reports"), Month, Year, UserID))
    End Sub
#End Region

    ' changes the year
#Region "[ DropDownListYear_SelectedIndexChanged ]"
    Protected Sub DropDownListYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListYear.SelectedIndexChanged
        Me.Year = DropDownListYear.SelectedValue

        Me.SetCurrentMonth(Month)

        Me.RebindGrid()
    End Sub
#End Region

    ' shows/hides the recurrency checkbox
#Region "[ CheckBoxRecurrentExpenditure_CheckedChanged ]"
    Protected Sub CheckBoxRecurrentExpenditure_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CheckBoxRecurrentExpenditure.CheckedChanged
        If CheckBoxRecurrentExpenditure.Checked Then
            CheckBoxEnterForEveryMonth.Style("visibility") = "visible"
            LabelRecurrentForFollowingMonthsOnly.Style("visibility") = "visible"
        Else
            CheckBoxEnterForEveryMonth.Style("visibility") = "hidden"
            LabelRecurrentForFollowingMonthsOnly.Style("visibility") = "hidden"
        End If
    End Sub
#End Region

#Region "[ SHOW Panels, Divs ... ]"

    'SHOW add new field div
#Region "[ Button.Click: ButtonShowAddNewFieldsDiv_Click ]"
    'Protected Sub ButtonShowAddNewFieldsDiv_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonShowAddNewFieldsDiv.Click
    '    Try
    '        RestoreLoadingDivPosition()

    '        'If DivAddNewRecord.Visible Then
    '        '    DivAddNewRecord.Visible = False
    '        '    'DivModal.Visible = False
    '        'Else
    '        '    DivAddNewRecord.Visible = True
    '        '    'DivModal.Visible = True
    '        'End If

    '        'DrawPieChart()

    '        'For Each ctl As Control In PanelAddNewFields.Controls
    '        '    If TypeOf (ctl) Is TextBox Then
    '        '        CType(ctl, TextBox).Text = String.Empty
    '        '    End If
    '        'Next
    '        Logging.Logger.LogAction(Logging.Logger.HistoryAction.AddNew, UserID, GetConnectionString, Request.UserHostAddress)
    '    Catch ex As Exception
    '        DivError.Visible = True
    '        DivError.InnerText = "ButtonShowAddNewFieldsDiv_Click(): " & ex.Message
    '        Logging.Logger.Log(ex, "ButtonShowAddNewFieldsDiv_Click", "none", UserID, GetConnectionString)
    '    End Try

    'End Sub
#End Region

    '    'SHOW PREVIEW Div
    '#Region "[ ImageButton.Click: ImageButtonPreviewAttachment_Click ]"
    '    Protected Sub ImageButtonPreviewAttachment_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '        CType(CType(sender, ImageButton).Parent.Parent.FindControl("DivPreviewAttachment"), HtmlGenericControl).Visible = True
    '    End Sub
    '#End Region

    'SHOW calculator
#Region "[ Button.Click: ButtonCalculator_Click ]"
    Protected Sub ButtonCalculator_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCalculator.Click
        ''ClientScript.RegisterStartupScript(Me.GetType(), "calc", "<script language=javascript>window.open ('Calculator.htm','Calculator','menubar=0,resizable=0,width=200,height=150');</script>")
        'If PanelCalculator.Visible Then
        '    ButtonCalculator.Text = GetTranslatedValue("CalculatorButtonOpen", CurrentLanguage)
        '    PanelCalculator.Visible = False
        'Else
        '    ButtonCalculator.Text = GetTranslatedValue("CalculatorButtonClose", CurrentLanguage)
        '    PanelCalculator.Visible = True
        'End If

        Me.DrawPieChart()
    End Sub
#End Region

#End Region

#Region "[ CLOSE Panels, Divs ... ]"

    'CLOSE add new field div
#Region "[ Button.Click: ButtonAddFieldCancel_Click ]"
    'Protected Sub ButtonAddFieldCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAddFieldCancel.Click
    '    'DivAddNewRecord.Visible = False
    '    'DivModal.Visible = False
    '    DrawPieChart()
    'End Sub
#End Region

    'CLOSE PREVIEW Div
#Region "[ LinkButton.Click: LinkButtonClosePreview_Click ]"
    Protected Sub LinkButtonClosePreview_Click(ByVal sender As Object, ByVal e As EventArgs)
        CType(CType(sender, LinkButton).Parent.Parent.FindControl("DivPreviewAttachment"), HtmlGenericControl).Visible = False
        DrawPieChart()
    End Sub
#End Region

    'CLOSE PickCategory Div inside the add new field div
#Region "[ Button.Click: ButtonPickCategoryConfirm_Click ]"
    'Protected Sub ButtonPickCategoryConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonPickCategoryConfirm.Click
    '    TextBoxFieldName.Text = ListBoxPickCategory.SelectedItem.Text
    '    'DivPickCategory.Visible = False
    'End Sub
#End Region

    'CLOSE PickCategory Div inside the add new field div
#Region "[ Button.Click: ButtonPickCategoryCancel_Click ]"
    'Protected Sub ButtonPickCategoryCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonPickCategoryCancel.Click
    '    'DivPickCategory.Visible = False
    'End Sub
#End Region

#End Region

    Private Function GetNextIncomeDate(ByRef nextMonthIncome As IEnumerable(Of Income)) As Date
        Dim nextIncomeDate As Date = Nothing

        Dim nextMonth As Integer = Me.Month

        Dim year As Integer = Me.Year

        If nextMonth = 12 Then
            nextMonth = 1
            year = year + 1
        Else
            nextMonth = nextMonth + 1
        End If

        nextMonthIncome = Me.ExpenseManager.GetUserIncome(nextMonth, year)

        Dim monthIncome As IEnumerable(Of Income) = If(TryCast(nextMonthIncome, Income()), nextMonthIncome.ToArray())

        If nextMonthIncome IsNot Nothing AndAlso monthIncome.Any() Then
            nextIncomeDate = monthIncome.Min(Function(d) d.Date)
        Else
            nextIncomeDate = Me.IncomeDataSource.Min(Function(d) d.Date).AddMonths(1)
        End If

        Return nextIncomeDate
    End Function

    Private Sub DrawMonthCompletionProgressBar(ByVal nextIncomeDate As Date, ByVal nextMonthIncome As IEnumerable(Of Income))

        Try

            Dim firstIncomeDate As Date = Me.IncomeDataSource.Min(Function(i) i.Date)

            Dim daysUntilNextPayCheck As Double = (nextIncomeDate - DateTime.Now.Date).TotalDays

            Dim daysBetweenPayChecks As Double = (nextIncomeDate - firstIncomeDate).TotalDays

            Dim dayWidthPixels As Integer = CInt(1283 / daysBetweenPayChecks)

            Dim table As HtmlTable = New HtmlTable()

            table.Attributes.Add("class", "ProgressBar")

            Dim row As HtmlTableRow = New HtmlTableRow()

            Dim counterRow As HtmlTableRow = New HtmlTableRow()

            If Not (firstIncomeDate.Year >= DateTime.Now.Year AndAlso nextIncomeDate.Year >= DateTime.Now.Year) Then
                Return
            End If

            Dim remainingDaysCounter As Integer = 0

            For Each d As DateTime In DateHelper.EachDay(firstIncomeDate, nextIncomeDate)

                Dim counterRowCell As HtmlTableCell = New HtmlTableCell()

                counterRowCell.Attributes.Add("class", "ProgressBarCounterRowCellStyle")
                counterRowCell.Width = dayWidthPixels.ToString()
                counterRowCell.Align = "center"

                If d.Date > DateTime.Now.Date Then

                    remainingDaysCounter = remainingDaysCounter + 1

                    counterRowCell.InnerText = CStr(remainingDaysCounter)

                End If

                counterRow.Cells.Add(counterRowCell)

                Dim cell As HtmlTableCell = New HtmlTableCell()
                cell.Width = dayWidthPixels.ToString()
                cell.InnerText = d.Day.ToString()
                cell.Align = "center"

                If d.Date < DateTime.Now Then
                    cell.Attributes.Add("class", "ProgressBarPassedDaysStyle")
                End If

                If d.Date = DateTime.Now.Date Then
                    cell.Attributes.Add("class", "ProgressBarCurrentDayStyle")
                End If

                If Not d.Date.IsWeekDay Then
                    cell.Attributes.Add("class", "ProgressBarWeekendStyle")
                End If

                If Me.IncomeDataSource.Any(Function(i) i.Date = d.Date) OrElse nextMonthIncome.Any(Function(i) i.Date = d.Date) Then
                    'cell.Attributes.Add("class", "ProgressBarIncomeDayStyle")
                    cell.InnerText = String.Empty
                    cell.Controls.Add(New HtmlImage() With {.Src = "../Images/Coins.png"})

                End If

                row.Cells.Add(cell)
            Next

            table.Rows.Add(row)
            table.Rows.Add(counterRow)

            PanelProgressBarContainer.Controls.Clear()
            PanelProgressBarContainer.Controls.Add(table)

            'Dim progressText As String = String.Format("{0} days remaining", daysThisMonth)
            'Dim progress As Double = DateTime.Now.Day

            'Dim script As String = "$('#progressbar').progressbar();" & vbCrLf &
            '        String.Format("$('#progressbar').progressbar('option', 'max', {0});", daysThisMonth) & vbCrLf &
            '        String.Format("$('#progressbar').progressbar('value', {0}).children('.ui-progressbar-value').html('{1}').css('display', 'block');", progress, progressText)

            'Environment.ExecuteScript(GridView1, script, True)
        Catch ex As Exception
            Logging.Logger.Log(ex, "DrawMonthCompletionProgressBar", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try
    End Sub

    Protected Sub BudgetUpdated() Handles Budget1.BudgetUpdated
        Me.CalculateAndDisplaySum()

    End Sub

    Protected Sub ButtonNotes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonNotes.Click
        Me.RestoreLoadingDivPosition()
        Me.DrawPieChart()
    End Sub

    Protected Sub ImageButtonFlag_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim qry As String = String.Empty
        Dim id As String = CType(sender, ImageButton).Attributes("ID")
        Dim flagged As Boolean = CType(sender, ImageButton).Attributes("Flagged")

        Try
            If flagged Then
                qry = String.Format(
"UPDATE [dbo].[{0}] Set [Flagged] = 0 WHERE [ID] = {1}", MainTable, id)
            Else
                qry = String.Format(
"UPDATE [dbo].[{0}] Set [Flagged] = 1 WHERE [ID] = {1}", MainTable, id)
            End If

            DataBaseConnector.ExecuteQuery(qry, GetConnectionString)

            Me.RebindGrid()

            Logging.Logger.LogAction(Logging.Logger.HistoryAction.FlagBill, UserID, GetConnectionString, Request.UserHostAddress)

        Catch ex As Exception
            Logging.Logger.Log(ex, "ImageButtonFlag_Click", qry, UserID, GetConnectionString)
        End Try
    End Sub

    Protected Sub LinkButtonSumFlaggedBillsText_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonSumFlaggedBillsText.Click

        If Me.DisplayAllFlaggedSums Then
            Me.DisplayAllFlaggedSums = False
            Me.RebindGrid()
        Else
            Me.DisplayAllFlaggedSums = True
            Dim flaggedExpenditures As List(Of Expenditure) = Me.ExpenseManager.GetUserExpenditures(Me.DisplayAllFlaggedSums)
            Me.RebindGrid(flaggedExpenditures)
        End If

    End Sub

    Protected Sub LinkButtonFlagAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonFlagAll.Click

        Dim id As String = String.Empty
        Dim qry As String = String.Empty

        For i As Integer = 0 To GridView1.Rows.Count - 1

            id = CType(GridView1.Rows(i).FindControl("TextBoxID"), TextBox).Text
            If IsNumeric(id) Then
                qry &= String.Format(
"UPDATE [dbo].[{0}] Set [Flagged] = 1 WHERE [ID] = {1} ", MainTable, id)

            End If
        Next

        DataBaseConnector.ExecuteQuery(qry, GetConnectionString)

        Me.RebindGrid()

    End Sub

    Protected Sub LinkButtonUnflagAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonUnflagAll.Click

        Dim id As String = String.Empty
        Dim qry As String = String.Empty

        For i As Integer = 0 To GridView1.Rows.Count - 1

            id = CType(GridView1.Rows(i).FindControl("TextBoxID"), TextBox).Text
            If IsNumeric(id) Then
                qry &= String.Format(
"UPDATE [dbo].[{0}] Set [Flagged] = 0 WHERE [ID] = {1} ", MainTable, id)

            End If
        Next

        DataBaseConnector.ExecuteQuery(qry, GetConnectionString)

        Me.RebindGrid()

    End Sub

    Protected Sub ButtonUndo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonUndo.Click
        Me.RebindGrid()
    End Sub

    ' BEGIN CONTEXT MENU --->

    ' COPY
#Region "[ LinkButtonContextMenuCopy_Click ]"
    Protected Sub LinkButtonContextMenuCopy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonContextMenuCopy.Click

        Dim qry As String = String.Empty

        Try

            Me.ExpenseManager.CopyParentExpense(HiddenRowID.Value, qry)

            Me.RebindGrid()

            ButtonUpdate_Click(sender, e)
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.CopyParentExpense, UserID, GetConnectionString, Request.UserHostAddress)

        Catch ex As Exception
            Logging.Logger.Log(ex, "LinkButtonContextMenuDelete_Click", qry, UserID, GetConnectionString)
        End Try
    End Sub

    Protected Sub LinkButtonContextMenuDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonContextMenuDelete.Click

        ButtonDeleteSelectedRow_Click(sender, e)
    End Sub
#End Region

    ' FLAG
#Region "[ LinkButtonContextMenuFlag_Click ]"
    Protected Sub LinkButtonContextMenuFlag_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonContextMenuFlag.Click

        Dim qry As String = String.Empty

        Try

            qry = String.Format(
"UPDATE [dbo].[{0}] Set [Flagged] = (Select 1 - [Flagged] FROM [dbo].[{0}] WHERE [ID] = {1}) WHERE [ID] = {1}", MainTable, HiddenRowID.Value)

            DataBaseConnector.ExecuteQuery(qry, GetConnectionString)

            Me.RebindGrid()

            Logging.Logger.LogAction(Logging.Logger.HistoryAction.FlagBill, UserID, GetConnectionString, Request.UserHostAddress)

        Catch ex As Exception
            Logging.Logger.Log(ex, "LinkButtonContextMenuFlag_Click", qry, UserID, GetConnectionString)
        End Try

    End Sub
#End Region

    ' IS PAID
#Region "[ LinkButtonMarkedPaid_Click ]"
    Protected Sub LinkButtonMarkedPaid_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonContextMenuMarkedPaid.Click

        Dim qry As String = String.Empty

        Try

            Dim parentExpenseIDs(GridView1.Rows.Count) As Integer

            Dim rows As IEnumerable(Of GridViewRow) = From row In Me.GridView1.Rows Where CType(CType(row, GridViewRow).FindControl("CheckBoxSelectRow"), CheckBox).Checked Select CType(row, GridViewRow)

            Dim index As Integer = 0

            For Each a In rows

                Dim chkBox As CheckBox = CType(a.FindControl("CheckBoxSelectRow"), CheckBox)

                If chkBox.Checked Then
                    parentExpenseIDs(index) = chkBox.Attributes("rowid")

                    index = index + 1
                End If

            Next

            Dim dataSource As List(Of Expenditure) = CType(MainTableDataSource, List(Of Expenditure))

            Dim expenses As List(Of Expenditure) = New List(Of Expenditure)()

            If index > 0 Then
                expenses = (From exp As Expenditure In dataSource Where parentExpenseIDs.Contains(exp.ID) Select exp).ToList()
            Else
                expenses = (From exp As Expenditure In dataSource Where exp.ID = HiddenRowID.Value Select exp).ToList()
            End If

            For Each expense As Expenditure In expenses
                expense.IsPaid = Not expense.IsPaid
            Next

            Me.ExpenseManager.UpdateParentExpenses(expenses, String.Empty)

            Me.RebindGrid()

        Catch ex As Exception
            Logging.Logger.Log(ex, "LinkButtonMarkedPaid_Click", qry, UserID, GetConnectionString)
        End Try

    End Sub

#End Region

    ' DOWNLOAD ATTACHMENT
#Region "[ LinkButtonContextMenuDownloadAttachment_Click ]"
    Protected Sub LinkButtonContextMenuDownloadAttachment_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonContextMenuDownloadAttachment.Click

        Try
            Response.Redirect(String.Format("DownloadAttachment.aspx?id={0}&attachingToDetails=False", HiddenRowID.Value))
        Catch ex As Exception
            Logging.Logger.Log(ex, "LinkButtonContextMenuDownloadAttachment", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

    End Sub
#End Region

    ' DELETE ATTACHMENT
#Region "[ LinkButtonContextMenuDeleteAttachment_Click ]"
    Protected Sub LinkButtonContextMenuDeleteAttachment_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonContextMenuDeleteAttachment.Click

        Dim qry As String = String.Empty

        Try

            Me.ExpenseManager.DeleteAttachment(HiddenRowID.Value, qry)

        Catch ex As Exception
            Logging.Logger.Log(ex, "LinkButtonContextMenuDownloadAttachment", qry, UserID, GetConnectionString)
        End Try

    End Sub
#End Region

    ' NEXT MONTH
#Region "[ LinkButtonContextMenuNextMonth_Click ]"
    Protected Sub LinkButtonContextMenuNextMonth_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonContextMenuNextMonth.Click

        Dim qry As String = String.Empty

        Try

            If Month < 12 Then

                SetCurrentMonth(Month + 1)

            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "LinkButtonContextMenuNextMonth_Click", qry, UserID, GetConnectionString)
        End Try

    End Sub

#End Region

    ' PREVIOUS MONTH
#Region "[ LinkButtonContextMenuPreviousMonth_Click ]"
    Protected Sub LinkButtonContextMenuPreviousMonth_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonContextMenuPreviousMonth.Click

        Dim qry As String = String.Empty

        Try

            If Month > 1 Then

                SetCurrentMonth(Month - 1)

            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "LinkButtonContextMenuPreviousMonth_Click", qry, UserID, GetConnectionString)
        End Try

    End Sub
#End Region

    ' COPY ALL TO NEXT MONTH
#Region "[ LinkButtonContextMenuCopyToNextMonth_Click ]"
    Protected Sub LinkButtonContextMenuCopyToNextMonth_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonContextMenuCopyToNextMonth.Click

        Dim qry As String = String.Empty

        Try

            Me.Month = Me.Month + 1

            Me.ExpenseManager.DuplicateExpenditures(Me.Month, Me.Year, False, False, True, True, qry)

            Me.SetCurrentMonth(Me.Month + 1)

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("LinkButtonContextMenuCopyToNextMonth_Click() {0}; query: {1}", ex.Message, qry)
            Logging.Logger.Log(ex, "LinkButtonContextMenuCopyToNextMonth_Click()", qry, UserID, GetConnectionString)
        End Try

    End Sub
#End Region

    ' <--- END CONTEXT MENU

    Protected Sub InitContextMenu()

        Dim script As String = String.Format("var myContextMenu = New ContextMenu('{0}', 'contextMenu', '{1}', function (e) {{ }});", GridView1.ClientID, HiddenRowID.ClientID)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "initcontextmenu", script, True)

    End Sub

    Protected Sub ButtonHidePaidExpenses_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonHidePaidExpenses.Click, LinkButtonContextMenuHidePaid.Click

        Me.PaidExpensesHidden = Not Me.PaidExpensesHidden

        If Me.PaidExpensesHidden Then
            ButtonHidePaidExpenses.CssClass = "ButtonAddMediumActive"
            ButtonHidePaidExpenses.Text = Me.GetTranslatedValue("ButtonShowPaidExpenses", Me.CurrentLanguage)
        Else
            ButtonHidePaidExpenses.CssClass = "ButtonAddMediumInactive"
            ButtonHidePaidExpenses.Text = Me.GetTranslatedValue("ButtonHidePaidExpenses", Me.CurrentLanguage)
        End If

        Me.RebindGrid()

    End Sub

    Protected Sub SetCurrentMonth(ByVal month As Integer)
        Dim btn As Button = New Button()

        btn.ID = String.Format("Button{0}", month)

        Me.Button_Click(btn, EventArgs.Empty)
    End Sub

    Protected Sub SetCurrentYear(ByVal year As Integer)

        Me.Year = year

        With DropDownListYear
            .SelectedIndex = -1
            .Items.FindByValue(CStr(year)).Selected = True
        End With

    End Sub

    'Protected Sub ButtonCategoriesManagement_Click(sender As Object, e As EventArgs) Handles ButtonCategoriesManagement.Click
    '    Response.Redirect(URLRewriter.GetLink("CategoriesManagement"))
    'End Sub

    Protected Sub ButtonProductsManagement_Click(sender As Object, e As EventArgs) Handles ButtonProductsManagement.Click
        Response.Redirect(URLRewriter.GetLink("ProductsManagement"))
    End Sub

    Protected Sub ButtonEventsCalendar_Click(sender As Object, e As EventArgs) Handles ButtonEventsCalendar.Click
        Response.Redirect(URLRewriter.GetLink("Events"))
        Logging.Logger.LogAction(Logging.Logger.HistoryAction.Events, UserID, GetConnectionString, Request.UserHostAddress)
    End Sub

    Protected Sub ZeroSelectedProductID()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ZeroProductID", "$(""[id*=HiddenProductID]"").val(1)", True)
    End Sub

    Protected Sub LinkButtonSpentToday_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonSpentToday.Click

        Me.TodaySpentListControl1.RebindGrid()
    End Sub

    Protected Sub ButtonSaveNewTranslation_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveNewTranslation.Click

        Try

            Translation.UpdateControlTranslation(TextBoxControlID.Text, TextBoxNewTranslation.Text, Me.CurrentLanguage, Me.GetConnectionString)

            HttpRuntime.Cache.Remove("translations")

            Response.Redirect(Request.RawUrl)

        Catch tae As ThreadAbortException
        Catch ex As Exception
            Logging.Logger.Log(ex, "ButtonSaveNewTranslation_Click", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

    End Sub

    Protected Sub ButtonAllDetailsForCategory_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAllDetailsForCategory.Click

        Dim categoryID As Integer = Me.DetailsDataSource.FirstOrDefault().CategoryID

        If categoryID = Category.Fuel Then
            Me.RebindDetailsGrid(Me.Year, categoryID, MeasureType.Volume)
        Else
            Me.RebindDetailsGrid(Me.Year, categoryID)
        End If

    End Sub

    Protected Sub ListBoxParentExpenditures_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBoxParentExpenditures.SelectedIndexChanged

        Me.SelectedDetails = (From row As GridViewRow
                                     In GridViewDetails.Rows
                              Where DirectCast(row.FindControl("CheckBoxDetailsTableSelect"), CheckBox).Checked
                              Select Me.DetailsDataSource.FirstOrDefault(Function(d) d.ID = CInt(DirectCast(row.FindControl("CheckBoxDetailsTableSelect"), CheckBox).Attributes("DetailID")))).ToArray()

        If Me.SelectedDetails.Any() Then

            Dim targetParentID As Integer = CInt(ListBoxParentExpenditures.SelectedValue)

            Dim targetExpenditure As Expenditure = Me.MainTableDataSource.FirstOrDefault(Function(exp) exp.ID = targetParentID)

            Me.ExpenditureID = targetParentID ' stay on the targed details

            For Each selectedDetail As ExpenditureDetail In Me.SelectedDetails
                selectedDetail.ExpenditureID = targetParentID
            Next

            GridViewDetails.PageIndex = Integer.MaxValue

            Me.ExpenseManager.UpdateChildExpenses(Me.SelectedDetails.ToList())

            Me.RebindGrid()
            Me.RebindDetailsGrid(targetParentID)

        End If

    End Sub

    Protected Sub GridViewDetail_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridViewDetails.PageIndexChanging

        GridViewDetails.PageIndex = e.NewPageIndex

        Me.RebindDetailsGridInternal()

        'Me.RebindDetailsGrid(Me.ExpenditureID)

    End Sub

    Protected Sub GridViewDetail_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GridViewDetails.Sorting

        Me.RebindDetailsGridInternal(e)

    End Sub

    Protected Sub GridViewMain_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GridView1.Sorting

        Me.RebindDetailsGridInternal(e)

    End Sub

    Protected Sub DropDownListDetailsPageSizeTop_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListDetailsPageSizeTop.SelectedIndexChanged, DropDownListDetailsPageSizeBottom.SelectedIndexChanged

        Dim ddl As DropDownList = CType(sender, DropDownList)

        Me.PageSizeDetails = ddl.SelectedValue

        Me.RebindDetailsGridInternal()

        Dim script As String = String.Format("$('#{0}').val({2}); $('#{1}').val({2});", DropDownListDetailsPageSizeTop.ClientID, DropDownListDetailsPageSizeBottom.ClientID, Me.PageSizeDetails)

        Environment.ExecuteScript(sender, script, True)

    End Sub

    Protected Sub PurchaseHistory_DateSelected(ByVal selectedDate As Date) Handles PurchaseHistoryDateSelectorControl1.DateSelected

        With Me.PurchaseHistoryControl1
            .SelectedDate = selectedDate
            .RebindGrid()
        End With

    End Sub

    Protected Sub AjaxFileUploadReceipt_UploadComplete(sender As Object, e As AjaxControlToolkit.AjaxFileUploadEventArgs)

        Dim physicalFilePath As String = String.Empty

        Dim bitmapReceipt As Bitmap = Nothing

        Dim bitmapToOcr As Bitmap = Nothing

        Try

            Dim receiptsPath = Server.MapPath(String.Format("~\Images\Receipts\{0}\", Me.UserID))

            If Not Directory.Exists(receiptsPath) Then
                Directory.CreateDirectory(receiptsPath)
            End If

            physicalFilePath = Path.Combine(receiptsPath, e.FileName)

            bitmapReceipt = Bitmap.FromStream(New MemoryStream(e.GetContents()))

            bitmapToOcr = New Bitmap(bitmapReceipt)

            bitmapToOcr.Save(physicalFilePath)

            Dim receiptImporter As ReceiptScanner.ReceiptImporter = New ReceiptScanner.ReceiptImporter()

            Dim products() As ReceiptScanner.ReceiptImporter.Product = receiptImporter.Scan(physicalFilePath)

            For Each product As ReceiptScanner.ReceiptImporter.Product In products

                Dim expenditureDetail As ExpenditureDetail = New ExpenditureDetail(Me.ExpenseManager)

                With expenditureDetail

                    .ExpenditureID = Me.ExpenditureID
                    .DetailName = product.Name

                    .ProductID = MHB.BL.Product.PRODUCT_DEFAULT_ID
                    '.Product = New Product(.ProductID, Me.UserID, Me.GetConnectionString)
                    .SupplierID = Me.HiddenSelectedSupplier.Value
                    .MainTableName = Me.MainTable
                    .DetailsTableName = Me.DetailsTable
                    .ConnectionString = Me.GetConnectionString
                    .Connection = New SqlConnection(Me.GetConnectionString)
                    .UserID = Me.UserID
                    .Amount = 1
                    .IsOcrScanned = True

                    Dim transactionSum As Decimal = 0D

                    .MeasureType = MeasureType.Count
                    .DetailValue = product.Price
                    .Add()

                    transactionSum = .DetailValue

                End With

            Next

        Catch ex As Exception
            Logging.Logger.Log(ex, "AjaxFileUploadReceipt_UploadComplete", String.Empty, Me.UserID, Me.GetConnectionString)
        Finally

            Dim encoder As ImageCodecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(Function(enc) enc.MimeType = "image/jpeg")

            Dim encoderParameters As EncoderParameters = New EncoderParameters(1)

            encoderParameters.Param(0) = New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 30)

            bitmapToOcr.Save(Path.ChangeExtension(physicalFilePath, ImageFormat.Jpeg.ToString().ToLower()), encoder, encoderParameters)

            File.Delete(physicalFilePath)

            bitmapToOcr.Dispose()
            bitmapToOcr = Nothing

            bitmapReceipt.Dispose()
            bitmapReceipt = Nothing

            encoder = Nothing
            encoderParameters = Nothing

        End Try

    End Sub

    Private Sub GridView1_Init(sender As Object, e As EventArgs) Handles GridView1.Init

    End Sub

    Protected Sub LinkButtonPurchaseHistory_Click(sender As Object, e As EventArgs)
        Me.PurchaseHistoryDateSelectorControl1.DataSource = Me.ExpenseManager.GetSumSpentPerDay(Me.MainTableDataSource)
    End Sub

End Class