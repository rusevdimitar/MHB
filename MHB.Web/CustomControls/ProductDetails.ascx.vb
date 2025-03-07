Imports MHB.BL
Imports MHB.BL.Enums
Imports System.Linq.Expressions

Public Class ProductDetails
    Inherits System.Web.UI.UserControl

    Private _en As Environment = New Environment()

    Private _expenseDetail As ExpenditureDetail

    Public Property ExpenseDetail As ExpenditureDetail
        Get
            Return Me._expenseDetail
        End Get
        Set(value As ExpenditureDetail)
            Me._expenseDetail = value
        End Set
    End Property

    Private _isCategoryStatistic As Boolean
    Public Property IsCategoryStatistic() As Boolean
        Get
            Return _isCategoryStatistic
        End Get
        Set(ByVal value As Boolean)
            _isCategoryStatistic = value
        End Set
    End Property

    Private _productSupplierInfo As ProductInfo()

    Public ReadOnly Property ProductSupplierInfo As ProductInfo()
        Get
            Return Me._productSupplierInfo
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.GetStatistics()
    End Sub

    Private Sub GetStatistics()

        Try

            ' Stupid workaround - for category statistics product and expenseDetail are not needed! refactor!
            If Me._expenseDetail IsNot Nothing AndAlso Me._expenseDetail.Product IsNot Nothing Then

                Dim product As Product = Me._expenseDetail.Product

                Dim queryWhereClauseFunc As Expression(Of Func(Of ExpenditureDetail, Boolean)) = Nothing

                Dim args As ExpressionQueryArgs = Me._en.ExpenseManager.GetExpressionQueryArgs()

                args.Add("IsDeleted", False)

                If Me.IsCategoryStatistic = True Then

                    LabelPDCProductName.Text = product.Category.Name

                    args.Add("CategoryID", product.CategoryID)

                    queryWhereClauseFunc = Function(d) d.CategoryID = ExpressionQueryArgs.Parameter(Of Integer)("CategoryID") _
                                               AndAlso d.IsDeleted = ExpressionQueryArgs.Parameter(Of Boolean)("IsDeleted")
                Else

                    LabelPDCProductName.Text = product.Name

                    args.Add("ProductID", product.ID)

                    queryWhereClauseFunc = Function(d) d.ProductID = ExpressionQueryArgs.Parameter(Of Integer)("ProductID") _
                                             AndAlso d.IsDeleted = ExpressionQueryArgs.Parameter(Of Boolean)("IsDeleted")
                End If

                Dim productEntries As IEnumerable(Of ExpenditureDetail) = Me._en.ExpenseManager.GetExpenditureDetails(queryWhereClauseFunc, args)

                ' get the most frequently occuring measure type
                Dim measureType As MeasureType = productEntries.GroupBy(Function(d) d.MeasureType).OrderByDescending(Function(d) d.Count()).Select(Function(d) d.Key).First()

                ' up we calculated the most frequently occuring measure type (either weight, volume or count) and here we remove the wrong ones which do not match
                Dim expenditureDetailsToRemove As ExpenditureDetail() = productEntries.Where(Function(detail) detail.MeasureType <> measureType).ToArray()

                productEntries = productEntries.Except(expenditureDetailsToRemove)

                Dim funcMaxMinValueDetailPerSupplier = Function(supplierID As Integer) As ProductInfo

                                                           Dim detailsForSupplier = productEntries.Where(Function(detail) detail.SupplierID = supplierID)

                                                           Dim quantityIsInWeightOrVolume As Boolean = measureType = Enums.MeasureType.Volume OrElse measureType = Enums.MeasureType.Weight

                                                           Dim minValue As Decimal = 0D
                                                           Dim maxValue As Decimal = 0D
                                                           Dim avgValue As Decimal = 0D
                                                           Dim totalValue As Decimal = 0D
                                                           Dim totalAmount As Decimal = 0D

                                                           If quantityIsInWeightOrVolume = True Then

                                                               ' We can also use the ExpenditureDetail.UnitPrice property which has already this calculated;
                                                               Dim inceptionWeightVolumeCalcFunc = Function(detail As ExpenditureDetail) As Decimal
                                                                                                       Return Decimal.Round(detail.DetailValue / detail.Amount, 2)
                                                                                                   End Function

                                                               minValue = detailsForSupplier.Min(Function(detail) inceptionWeightVolumeCalcFunc(detail))
                                                               maxValue = detailsForSupplier.Max(Function(detail) inceptionWeightVolumeCalcFunc(detail))
                                                               avgValue = detailsForSupplier.Average(Function(detail) inceptionWeightVolumeCalcFunc(detail))
                                                               totalAmount = detailsForSupplier.Sum(Function(detail) detail.Amount)
                                                           Else

                                                               minValue = detailsForSupplier.Min(Function(detail) detail.DetailValue)
                                                               maxValue = detailsForSupplier.Max(Function(detail) detail.DetailValue)
                                                               avgValue = detailsForSupplier.Average(Function(detail) detail.DetailValue)
                                                               totalAmount = detailsForSupplier.Count
                                                           End If

                                                           totalValue = detailsForSupplier.Sum(Function(detail) detail.DetailValue)

                                                           Return New ProductInfo() With {
                                                                .UniqueID = Guid.NewGuid(),
                                                                .SupplierID = detailsForSupplier.First().SupplierID,
                                                                .SupplierName = detailsForSupplier.First().Supplier.Name,
                                                                .Max = maxValue,
                                                                .Min = minValue,
                                                                .Average = avgValue,
                                                                .Total = totalValue,
                                                                .Amount = totalAmount
                                                            }

                                                       End Function

                Me._productSupplierInfo =
productEntries.GroupBy(Function(detail) detail.SupplierID).Distinct().Select(Function(grouping) funcMaxMinValueDetailPerSupplier(grouping.Key)).OrderBy(Function(pi) pi.Average).ToArray()

                Dim lowestPrice As Decimal = Me._productSupplierInfo.Where(Function(pi) pi.SupplierID <> Supplier.SUPPLIER_DEFAULT_ID).Min(Function(mpi) mpi.Min)

                Dim productWithLowestPrice As ProductInfo() = Me._productSupplierInfo.Where(Function(pi) pi.Min = lowestPrice AndAlso pi.SupplierID <> Supplier.SUPPLIER_DEFAULT_ID).ToArray()

                For Each piLow As ProductInfo In productWithLowestPrice
                    piLow.HasLowestPrice = True
                Next

                Dim highestPrice As Decimal = Me._productSupplierInfo.Where(Function(pi) pi.SupplierID <> Supplier.SUPPLIER_DEFAULT_ID).Max(Function(mpi) mpi.Max)

                Dim productWithHighestPrice As ProductInfo() = Me._productSupplierInfo.Where(Function(pi) pi.Max = highestPrice AndAlso pi.SupplierID <> Supplier.SUPPLIER_DEFAULT_ID).ToArray()

                For Each piHigh As ProductInfo In productWithHighestPrice
                    piHigh.HasHighestPrice = True
                Next

                With GridViewProductSupplierInfo

                    .DataSource = Me.ProductSupplierInfo
                    .DataBind()

                End With

                productEntries = productEntries.Where(Function(d) d.MeasureType = measureType)

                LabelPDCProductValue.Text = Me._expenseDetail.UnitPrice.ToString("0.00")

                Dim minEntry = productEntries.Where(Function(d) d.MeasureType = measureType).OrderBy(Function(d) d.UnitPrice).First()
                LabelPDCProductMinValue.Text = String.Format("{0} - {1} - ", minEntry.UnitPrice.ToString("0.00"), minEntry.Supplier.Name)
                LinkButtonPDCProductMinValueDate.Text = minEntry.DetailDate.ToISODateFileName()

                Dim maxEntry = productEntries.Where(Function(d) d.MeasureType = measureType).OrderByDescending(Function(d) d.UnitPrice).First()
                LabelPDCProductMaxValue.Text = String.Format("{0} - {1} - ", maxEntry.UnitPrice.ToString("0.00"), maxEntry.Supplier.Name)
                LinkButtonPDCProductMaxValueDate.Text = maxEntry.DetailDate.ToISODateFileName()

                If product.Parameters.Any(Function(p) p.ProductParameterTypeID = ProductParameterType.Mileage) AndAlso Me._expenseDetail.MeasureType = Enums.MeasureType.Volume Then
                    trAverageConsumption.Visible = True
                    LabelPDCAverageConsumption.Text = (Me._expenseDetail.Amount / CDbl(product.Parameters.FirstOrDefault(Function(p) p.ProductParameterTypeID = ProductParameterType.Mileage).Value) * 100).ToString("0.00")
                Else
                    trAverageConsumption.Visible = False
                End If

                LabelPDCProductAvgValue.Text = productEntries.Average(Function(d) d.UnitPrice).ToString("0.00")

                Dim productEntriesThisMonth As IEnumerable(Of ExpenditureDetail) = productEntries.Where(Function(d) d.DetailDate.Year = Me._en.Year AndAlso d.DetailDate.Month = Me._en.Month)
                Dim productEntriesThisYear As IEnumerable(Of ExpenditureDetail) = productEntries.Where(Function(d) d.DetailDate.Year = Me._en.Year)

                Dim detailsPerYear = productEntries.GroupBy(Function(d) d.DetailDate.Year).Select(Function(kv) kv)
                Dim detailsPerMonth = detailsPerYear.Select(Function(kv) kv.GroupBy(Function(d) d.DetailDate.Month))

                If measureType = Enums.MeasureType.Volume OrElse measureType = Enums.MeasureType.Weight Then
                    trPackageUnitsCountMonthly.Visible = False
                    trPackageUnitsCountYearly.Visible = False
                    trPackageUnitsCountAllTime.Visible = False

                    Dim unitCountsPerMonth = detailsPerMonth.SelectMany(Function(ykv) ykv.Select(Function(kv) New With {.Month = kv.Key, .Sum = kv.Sum(Function(d) d.Amount)}))
                    Dim unitCountsPerYear = detailsPerYear.Select(Function(kv) New With {.Year = kv.Key, .Sum = kv.Sum(Function(d) d.Amount)})

                    LabelPDCTotalQuantityPerMonthValue.Text = String.Format("{0} - {1}", unitCountsPerMonth.Min(Function(d) d.Sum), unitCountsPerMonth.Max(Function(d) d.Sum))
                    LabelPDCTotalQuantityPerYearValue.Text = String.Format("{0} - {1}", unitCountsPerYear.Min(Function(d) d.Sum), unitCountsPerYear.Max(Function(d) d.Sum))

                    LabelPDCTotalQuantityYearlyValue.Text = productEntriesThisYear.Sum(Function(d) d.Amount).ToString("0.00")
                    LabelPDCTotalQuantityValue.Text = productEntriesThisMonth.Sum(Function(d) d.Amount).ToString("0.00")
                    LabelPDCAllTimeTotalQtyValue.Text = String.Format("{0} - ({1} - {2})", productEntries.Sum(Function(d) d.Amount).ToString("0.00"), productEntries.Min(Function(d) d.DetailDate).ToISODateFileName(), productEntries.Max(Function(d) d.DetailDate).ToISODateFileName())
                Else
                    LabelPDCTotalQuantityYearlyValue.Text = productEntriesThisYear.Count().ToString()
                    LabelPDCTotalQuantityValue.Text = productEntriesThisMonth.Count().ToString()

                    Dim unitCountsPerMonth = detailsPerMonth.SelectMany(Function(ykv) ykv.Select(Function(kv) New With {.Month = kv.Key, .Sum = kv.Where(Function(d) d.Product IsNot Nothing).Sum(Function(d) d.Product.PackageUnitsCount)}))
                    Dim unitCountsPerYear = detailsPerYear.Select(Function(kv) New With {.Year = kv.Key, .Sum = kv.Where(Function(d) d.Product IsNot Nothing).Sum(Function(d) d.Product.PackageUnitsCount)})

                    ' Per month units
                    LabelPDCTotalQuantityPerMonthValue.Text = String.Format("{0} - {1} (avg. {2})", unitCountsPerMonth.Min(Function(d) d.Sum), unitCountsPerMonth.Max(Function(d) d.Sum), unitCountsPerMonth.Average(Function(d) d.Sum).ToString("0.0"))

                    ' Per year units
                    LabelPDCTotalQuantityPerYearValue.Text = String.Format("{0} - {1} (avg. {2})", unitCountsPerYear.Min(Function(d) d.Sum), unitCountsPerYear.Max(Function(d) d.Sum), unitCountsPerYear.Average(Function(d) d.Sum).ToString("0.0"))

                    ' This year units
                    LabelPDCTotalPackageUnitsCountYearlyValue.Text = productEntriesThisYear.Where(Function(d) d.Product IsNot Nothing).Sum(Function(detail) detail.Product.PackageUnitsCount).ToString()

                    ' This month units
                    LabelPDCTotalPackageUnitsCountValue.Text = productEntriesThisMonth.Where(Function(d) d.Product IsNot Nothing).Sum(Function(detail) detail.Product.PackageUnitsCount).ToString()

                    ' All time units
                    LabelPDCAllTimeTotalPackageUnitsCountValue.Text = productEntries.Where(Function(d) d.Product IsNot Nothing).Sum(Function(d) d.Product.PackageUnitsCount).ToString("0.00")

                    LabelPDCAllTimeTotalQtyValue.Text = String.Format("{0} - ({1} - {2})", productEntries.Count(), productEntries.Min(Function(d) d.DetailDate).ToISODateFileName(), productEntries.Max(Function(d) d.DetailDate).ToISODateFileName())
                End If

                LabelPDCTotalSumValue.Text = productEntriesThisMonth.Sum(Function(d) d.DetailValue).ToString("0.00")
                LabelPDCTotalSumYearlyValue.Text = productEntriesThisYear.Sum(Function(d) d.DetailValue).ToString("0.00")

                LabelPDCAllTimeTotalSumValue.Text = String.Format("{0} - ({1} - {2})", productEntries.Sum(Function(d) d.DetailValue).ToString("0.00"), productEntries.Min(Function(d) d.DetailDate).ToISODateFileName(), productEntries.Max(Function(d) d.DetailDate).ToISODateFileName())

            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "ProductDetails.GetStatistics", String.Empty, Me._en.UserID, Me._en.GetConnectionString)
        Finally
            Me._en.TranslateGridViewControls(GridViewProductSupplierInfo)
        End Try
    End Sub

    Protected Sub LinkButtonPDCProductMinMaxValueDate_Click(sender As Object, e As EventArgs)

        Dim linkButton As LinkButton = CType(sender, LinkButton)

        Dim expenseDetailDate As Date

        If Date.TryParse(linkButton.Text, expenseDetailDate) Then
            Me._en.Year = CInt(expenseDetailDate.Year)
            Me._en.Month = CInt(expenseDetailDate.Month)

            Response.Redirect(Request.RawUrl)

        End If

    End Sub
End Class