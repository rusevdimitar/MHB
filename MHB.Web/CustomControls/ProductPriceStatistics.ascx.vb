Imports MHB.Logging.Logger
Imports MHB.BL

Public Class ProductPriceStatistics
    Inherits System.Web.UI.UserControl

    Dim _en As Environment

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me._en = New Environment()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.Page.IsPostBack Then

            Try

                Dim productDetails As IEnumerable(Of ExpenditureDetail) = Enumerable.Empty(Of ExpenditureDetail)()

                If IsNumeric(Request.QueryString("ProductID")) Then

                    productDetails = Me._en.ExpenseManager.GetExpenditureDetailsForProduct(CInt(Request.QueryString("ProductID")))

                ElseIf IsNumeric(Request.QueryString("CategoryID")) Then

                    Me.ProductDetailsControl.IsCategoryStatistic = True

                    productDetails = Me._en.ExpenseManager.GetExpenditureDetailsForCategory(CInt(Request.QueryString("CategoryID")))

                End If

                Me.ProductDetailsControl.ExpenseDetail = productDetails.FirstOrDefault()

                Me.DrawChart(productDetails)

            Catch ex As Exception
                Logging.Logger.Log(ex, "Page_Load.ProductPriceStatistics.ascx.vb", String.Empty, Me._en.UserID, Me._en.GetConnectionString)
            Finally
                Logging.Logger.LogAction(HistoryAction.ProductPriceStatistics, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)
            End Try

        End If

    End Sub

    Private Sub DrawChart(ByVal dataSource As IEnumerable(Of ExpenditureDetail))

        If dataSource.Count > 1 Then

            Dim maxValue As Decimal = Math.Ceiling(dataSource.Select(Function(d) Me.GetDetailProductPrice(d)).Max())

            Dim minValue As Decimal = Math.Floor(dataSource.Select(Function(d) Me.GetDetailProductPrice(d)).Min())

            With ChartProductPriceStatistics
                .Series("Default").Points.DataBindXY(dataSource.Select(Function(d) d.DetailDate).ToArray(), dataSource.Select(Function(d) Me.GetDetailProductPrice(d)).ToArray())
                .ChartAreas.First().AxisY.Minimum = minValue
                .ChartAreas.First().AxisY.Maximum = maxValue
            End With

        End If
    End Sub

    Private Function GetDetailProductPrice(ByVal detail As ExpenditureDetail) As Decimal

        Dim price As Decimal = 0.0

        Select Case detail.MeasureType

            Case Enums.MeasureType.Count
                price = detail.DetailValue
                Exit Select
            Case Enums.MeasureType.Volume, Enums.MeasureType.Weight
                price = detail.DetailValue / detail.Amount
                Exit Select

        End Select

        Return Decimal.Round(price, 2)

    End Function

End Class