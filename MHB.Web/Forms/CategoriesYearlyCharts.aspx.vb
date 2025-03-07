Imports WebChart
Imports System.Data.SqlClient
Imports System.Drawing
Imports MHB.DAL

Partial Public Class CategoriesYearlyCharts
    Inherits Environment

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.CheckAccess()

        Me.AddNavigationLink(Me, MHB.BL.URLRewriter.GetLink("AnnualCategoryCharts"), GetTranslatedValue("charts", CurrentLanguage))

        DivError.Attributes.Add("onmouseover", "getElementById('" & DivError.ClientID & "').style.visibility= 'hidden'")
        DivError.Visible = False

        If Not IsPostBack Then
            FillCategoriesDdl()
            FillYearsDropDdl()
        End If

    End Sub

#Region "[ PlotChart ]"
    Protected Sub PlotChart(ByVal data As DataTable, ByVal title As String)
        Try

            Dim chart As New ColumnChart()

            chart.Fill.Color = Color.FromArgb(50, Color.SteelBlue)

            chart.Line.Color = Color.SteelBlue
            chart.Line.Width = 2

            For i As Integer = 0 To data.Rows.Count - 1 Step 1
                chart.Data.Add(New ChartPoint(String.Format("{0} ({1}{2})", data.Rows(i)("Year"), CDbl(data.Rows(i)("CostSum")), Currency), data.Rows(i)("CostSum")))
            Next

            ChartControl1.ChartTitle.Text = title

            ChartControl1.Charts.Add(chart)
            ChartControl1.RedrawChart()
        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("PlotChart(): {0}", ex.Message)
            Logging.Logger.Log(ex, "PlotChart()", String.Empty, UserID, GetConnectionString)
        End Try
    End Sub
#End Region

#Region "[ FillCategoriesDdl ]"
    Private Sub FillCategoriesDdl()

        Try

            With DropDownListCategories
                .DataTextField = "Name"
                .DataValueField = "ID"
                .DataSource = Me.Categories
                .DataBind()
            End With

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("FillCategoriesDdl(): {0}", ex.Message)
            Logging.Logger.Log(ex, "FillCategoriesDdl()", String.Empty, UserID, GetConnectionString)
        End Try

    End Sub
#End Region

#Region "[ DropDownListCategories_SelectedIndexChanged ]"
    Protected Sub DropDownListCategories_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListCategories.SelectedIndexChanged
        RebindChart()
    End Sub
#End Region

#Region "[ DropDownListStartYear_SelectedIndexChanged ]"
    Protected Sub DropDownListStartYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListStartYear.SelectedIndexChanged
        RebindChart()
    End Sub
#End Region

    Protected Sub FillYearsDropDdl()
        For year As Integer = 2000 To Date.Now.Year + 4
            DropDownListStartYear.Items.Add(year)
        Next
    End Sub

    Protected Sub RebindChart()
        Try

            Dim qry As String = "EXECUTE spGetSumsForCategoryPerYear @UserID, @SelectedYear, @CurrentLanguage, @SelectedCategory"

            Dim sqlParameters As SqlParameter() = New SqlParameter() {
                New SqlParameter("UserID", Me.UserID),
                New SqlParameter("SelectedYear", DropDownListStartYear.SelectedValue),
                New SqlParameter("CurrentLanguage", Me.CurrentLanguage),
                New SqlParameter("SelectedCategory", DropDownListCategories.SelectedValue)}

            Dim table As DataTable = DataBaseConnector.GetDataTable(qry, Me.GetConnectionString, sqlParameters)

            ChartControl1.Visible = True

            PlotChart(table, String.Format("{0} - ({1}-{2})", DropDownListCategories.SelectedItem.Text, DropDownListStartYear.SelectedValue, Date.Now.Year))

        Catch ex As Exception
            DivError.Visible = True
            DivError.InnerText = String.Format("RebindChart(): {0}", ex.Message)
            Logging.Logger.Log(ex, "RebindChart()", String.Empty, UserID, GetConnectionString)
        End Try
    End Sub
End Class