Imports WebChart
Imports System.Data.SqlClient
Imports System.Drawing
Imports MHB.BL

Partial Public Class Charts
    Inherits Environment

#Region "[ Enum: ChartType ]"
    Protected Enum ChartType
        AnnualExpensesChart = 0
        AnnualBudgetChart = 1
        AnnualSavingsChart = 2
    End Enum
#End Region

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.CheckAccess()

        Me.AddNavigationLink(Me, MHB.BL.URLRewriter.GetLink("Charts"), GetTranslatedValue("charts", CurrentLanguage))

        DivError.Attributes.Add("onmouseover", "getElementById('" & DivError.ClientID & "').style.visibility= 'hidden'")
        DivError.Visible = False

        If Not IsPostBack Then
            Me.RebindAnnualReportGrid()
        End If

    End Sub

#Region "[ GetDataToPlot ]"

    Protected Function GetDataToPlot(ByVal chartType As ChartType) As DataTable

        Dim table As DataTable = New DataTable()

        Dim expenses As ExpensesProMonth = New ExpensesProMonth()

        Try

            Select Case chartType

                Case Charts.ChartType.AnnualExpensesChart
                    expenses = ExpenseManager.GetYearlyExpensesProMonth()

                    Exit Select
                Case Charts.ChartType.AnnualBudgetChart
                    expenses = ExpenseManager.GetYearlyBudgetsProMonth()

                    Exit Select
                Case Charts.ChartType.AnnualSavingsChart
                    expenses = ExpenseManager.GetYearlySavingsProMonth()

                    Exit Select

            End Select

            table = ExpenseManager.GetYearlyExpensesProMonthDataTable(expenses)

            Dim sum As Decimal = 0

            With expenses
                sum = .SumJanuary +
                    .SumFebruary +
                    .SumMarch +
                    .SumApril +
                    .SumMay +
                    .SumJune +
                    .SumJuly +
                    .SumAugust +
                    .SumSeptember +
                    .SumOctober +
                    .SumNovember +
                    .SumDecember
            End With

            LabelSum.Text = String.Format("{0} {1} {2}", GetTranslatedValue("lblNewIncomeValue", CurrentLanguage), sum.ToString("0.00"), Currency)

        Catch ex As Exception
            Logging.Logger.Log(ex, "Charts.aspx.vb.GetDataToPlot()", String.Empty, UserID, GetConnectionString)
            DivError.Visible = True
            DivError.InnerText = String.Format("GetDataToPlot(): {0}", ex.Message)
            Return New DataTable()
        End Try

        Return table

    End Function
#End Region

#Region "[ Sub: PlotChart ]"
    Protected Sub PlotChart(ByVal data As DataTable, ByVal title As String)
        Try

            Dim chart As New ColumnChart()

            chart.Fill.Color = Color.FromArgb(50, Color.SteelBlue)

            chart.Line.Color = Color.SteelBlue
            chart.Line.Width = 2

            For i As Integer = 0 To data.Columns.Count - 1 Step 1

                Select Case i

                    Case 0
                        chart.Data.Add(New ChartPoint("Jan " & CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString(), CDec(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString()))
                        Exit Select
                    Case 1
                        chart.Data.Add(New ChartPoint("Feb " & CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString(), CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString()))
                        Exit Select
                    Case 2
                        chart.Data.Add(New ChartPoint("Mar " & CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString(), CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString()))
                        Exit Select
                    Case 3
                        chart.Data.Add(New ChartPoint("Apr " & CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString(), CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString()))
                        Exit Select
                    Case 4
                        chart.Data.Add(New ChartPoint("May " & CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString(), CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString()))
                        Exit Select
                    Case 5
                        chart.Data.Add(New ChartPoint("June " & CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString(), CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString()))
                        Exit Select
                    Case 6
                        chart.Data.Add(New ChartPoint("July " & CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString(), CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString()))
                        Exit Select
                    Case 7
                        chart.Data.Add(New ChartPoint("Aug " & CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString(), CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString()))
                        Exit Select
                    Case 8
                        chart.Data.Add(New ChartPoint("Sept " & CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString(), CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString()))
                        Exit Select
                    Case 9
                        chart.Data.Add(New ChartPoint("Oct " & CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString(), CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString()))
                        Exit Select
                    Case 10
                        chart.Data.Add(New ChartPoint("Nov " & CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString(), CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString()))
                        Exit Select
                    Case 11
                        chart.Data.Add(New ChartPoint("Dec " & CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString(), CDbl(IIf(IsDBNull(data.Rows(0)(i)), 0, data.Rows(0)(i))).ToString()))
                        Exit Select

                End Select

            Next

            '//ChartControl1.Background.Color = Color.FromArgb(75, Color.SteelBlue);
            '//ChartControl1.Background.Type = InteriorType.LinearGradient;
            '//ChartControl1.Background.ForeColor = Color.SteelBlue;
            '//ChartControl1.Background.EndPoint = new Point(1000, 600);
            '//ChartControl1.Legend.Position = LegendPosition.Bottom;
            '//ChartControl1.Legend.Width = 40;

            '//ChartControl1.YAxisFont.ForeColor = Color.SteelBlue;
            '//ChartControl1.XAxisFont.ForeColor = Color.SteelBlue;

            ChartControl1.ChartTitle.Text = title
            '//ChartControl1.ChartTitle.ForeColor = Color.White;

            '//ChartControl1.Border.Color = Color.SteelBlue;
            '//ChartControl1.BorderStyle = BorderStyle.Solid;
            '//ChartControl1.BorderWidth = 1;

            ChartControl1.Charts.Add(chart)
            ChartControl1.RedrawChart()
        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = "PlotChart(): " & ex.Message
        End Try
    End Sub
#End Region

#Region "[ RebindAnnualReportGrid ]"
    Protected Sub RebindAnnualReportGrid()
        Try

            GridViewAnnualReport.DataSource = ExpenseManager.GetYearlyExpensesProMonthDataTable(ExpenseManager.GetYearlyExpensesProMonth())
            GridViewAnnualReport.DataBind()

            GridViewAnnualReport0.DataSource = ExpenseManager.GetYearlyExpensesProMonthDataTable(ExpenseManager.GetYearlyBudgetsProMonth())
            GridViewAnnualReport0.DataBind()

            GridViewAnnualReport1.DataSource = ExpenseManager.GetYearlyExpensesProMonthDataTable(ExpenseManager.GetYearlySavingsProMonth())
            GridViewAnnualReport1.DataBind()

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("RebindAnnualReportGrid(): {0}", ex.Message)
        End Try

    End Sub
#End Region

    Protected Sub ButtonAnnuExpChart_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAnnuExpChart.Click
        ChartControl1.Visible = True
        PlotChart(GetDataToPlot(ChartType.AnnualExpensesChart), GetTranslatedValue("annualexpenses", CurrentLanguage))
    End Sub

    Protected Sub ButtonAnnuBudgetChart_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAnnuBudgetChart.Click
        ChartControl1.Visible = True
        PlotChart(GetDataToPlot(ChartType.AnnualBudgetChart), GetTranslatedValue("annualbudget", CurrentLanguage))
    End Sub

    Protected Sub ButtonAnnuSavingsChart_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAnnuSavingsChart.Click
        ChartControl1.Visible = True
        PlotChart(GetDataToPlot(ChartType.AnnualSavingsChart), GetTranslatedValue("annualsavings", CurrentLanguage))
    End Sub
End Class