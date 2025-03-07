Imports System.Data.SqlClient
Imports WebChart
Imports System.Drawing


Partial Public Class CostsCompare
    Inherits Environment

    Protected ReadOnly Property CostCategory() As String
        Get
            If Request.QueryString("costCategory") IsNot Nothing Then
                Return CStr(Request.QueryString("costCategory"))
            Else
                Return "0"
            End If
        End Get
    End Property


    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.CheckAccess()

        Me.PlotChart()
    End Sub


#Region "[ Sub: PlotChart ]"
    Protected Sub PlotChart()
        Try

            Dim chart As New ColumnChart()
            chart.Fill.Color = Color.FromArgb(50, Color.SteelBlue)
            chart.Line.Color = Color.SteelBlue
            chart.Line.Width = 2

            Dim result As Object = 0

            Using cn As New SqlConnection(GetConnectionString)

                Dim qry As String = "EXECUTE [dbo].[spGetUsersAverageExpensesForCategory] @UserID, @CategoryID, @Year, @Month"

                Dim parUserID As SqlParameter = New SqlParameter("@UserID", UserID)
                Dim parCategoryID As SqlParameter = New SqlParameter("@CategoryID", CostCategory)
                Dim parYear As SqlParameter = New SqlParameter("@Year", Year)
                Dim parMonth As SqlParameter = New SqlParameter("@Month", Month)

                Using cmd As New SqlCommand(qry, cn)

                    cn.Open()

                    cmd.Parameters.Add(parUserID)
                    cmd.Parameters.Add(parCategoryID)
                    cmd.Parameters.Add(parYear)
                    cmd.Parameters.Add(parMonth)

                    Dim reader As IDataReader = cmd.ExecuteReader()

                    While reader.Read()

                        If Not reader.IsDBNull(reader.GetOrdinal("UsersAverage")) Then
                            chart.Data.Add(New ChartPoint(String.Format("Your avg.: {0:0.00}", reader("UsersAverage")), reader("UsersAverage")))
                        End If

                        If Not reader.IsDBNull(reader.GetOrdinal("MaxString")) Then
                            chart.Data.Add(New ChartPoint(String.Format("Your max: {0:0.00}", reader("MaxString")), reader("MaxValue")))
                        End If

                        If Not reader.IsDBNull(reader.GetOrdinal("MinValue")) Then
                            chart.Data.Add(New ChartPoint(String.Format("Your min: {0:0.00}", reader("MinValue")), reader("MinValue")))
                        End If

                        If Not reader.IsDBNull(reader.GetOrdinal("SumThisYear")) Then
                            chart.Data.Add(New ChartPoint(String.Format("Sum {0}: {1:0.00}", Year, reader("SumThisYear")), reader("SumThisYear")))
                        End If

                        If Not reader.IsDBNull(reader.GetOrdinal("SumThisMonth")) Then
                            chart.Data.Add(New ChartPoint(String.Format("Sum {0}.{1}: {2:0.00}", Me.Year, Me.Month, reader("SumThisMonth")), reader("SumThisMonth")))
                        End If

                        If Not reader.IsDBNull(reader.GetOrdinal("SumPreviousMonth")) Then
                            chart.Data.Add(New ChartPoint(String.Format("Sum {0}.{1}: {2:0.00}", IIf(Me.Month = 1, Me.Year - 1, Me.Year), IIf(Me.Month = 1, 12, Me.Month - 1), reader("SumPreviousMonth")), reader("SumPreviousMonth")))
                        End If

                        Dim difference As Decimal = 0.0

                        If Not reader.IsDBNull(reader.GetOrdinal("DifferenceToPrevMonth")) Then
                            difference = reader("DifferenceToPrevMonth")
                        End If

                        If difference < 0 Then
                            chart.Data.Add(New ChartPoint(String.Format("{0}{1:0.00}", GetTranslatedValue("paidlessthismonth", CurrentLanguage), difference * -1), difference * -1))
                        Else
                            chart.Data.Add(New ChartPoint(String.Format("{0}{1:0.00}", GetTranslatedValue("paidmorethismonth", CurrentLanguage), difference), difference))
                        End If

                       

                    End While

                    cn.Close()

                End Using
            End Using

            Select Case Request.QueryString("costCategory")
                Case 1
                    ChartControl1.ChartTitle.Text = GetTranslatedValue("comparegasoline", CurrentLanguage)
                    Exit Select
                Case 2
                    ChartControl1.ChartTitle.Text = GetTranslatedValue("compareelectricity", CurrentLanguage)
                    Exit Select
                Case 3
                    ChartControl1.ChartTitle.Text = GetTranslatedValue("comparephone", CurrentLanguage)
                    Exit Select
                Case 4
                    ChartControl1.ChartTitle.Text = GetTranslatedValue("comparerent", CurrentLanguage)
                    Exit Select
                Case 5
                    ChartControl1.ChartTitle.Text = GetTranslatedValue("comparesavings", CurrentLanguage)
                    Exit Select
                Case 6
                    ChartControl1.ChartTitle.Text = GetTranslatedValue("compareinternet", CurrentLanguage)
                    Exit Select
                Case 7
                    ChartControl1.ChartTitle.Text = GetTranslatedValue("comparefood", CurrentLanguage)
                    Exit Select
                Case 8
                    ChartControl1.ChartTitle.Text = GetTranslatedValue("comparecar", CurrentLanguage)
                    Exit Select
                Case 9
                    ChartControl1.ChartTitle.Text = GetTranslatedValue("compareloan", CurrentLanguage)
                    Exit Select
                Case 10
                    ChartControl1.ChartTitle.Text = GetTranslatedValue("comparemedical", CurrentLanguage)
                    Exit Select



            End Select


            ChartControl1.Charts.Add(chart)
            ChartControl1.RedrawChart()
        Catch ex As Exception
            Logging.Logger.Log(ex, "CostsCompare.aspx.vb > PlotChart()", "none", UserID, GetConnectionString)
        End Try
    End Sub
#End Region


End Class