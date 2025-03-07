Imports MHB.BL
Imports System.Web.UI.DataVisualization.Charting
Imports System.Data.SqlClient
Imports MHB.DAL

Public Class Statistics
    Inherits Environment

    Public Property ChartDataSource As DataTable
        Get
            If Session("ChartDataSource") Is Nothing Then
                Session("ChartDataSource") = GetDataSource(DropDownListStartYear.SelectedValue, DropDownListEndYear.SelectedValue)
            End If

            Return Session("ChartDataSource")
        End Get
        Set(value As DataTable)
            Session("ChartDataSource") = value
        End Set
    End Property

    Public Property ChartRotation As Integer
        Get
            Return CInt(ViewState("ChartRotation"))
        End Get
        Set(value As Integer)
            ViewState("ChartRotation") = value
        End Set
    End Property

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.CheckAccess()

        Me.AddNavigationLink(Me, MHB.BL.URLRewriter.GetLink("Statistics"), GetTranslatedValue("ButtonStatistics", CurrentLanguage))

        If Not IsPostBack Then
            Me.FillYearsDropDowns()
            Me.FillCategoriesCheckBoxList()
            Me.DrawChart(False)
        End If

    End Sub

    Protected Sub FillCategoriesCheckBoxList()

        With Me.CheckBoxListCategories
            .DataTextField = "Name"
            .DataValueField = "ID"
            .DataSource = Me.Categories
            .DataBind()
        End With

    End Sub

    Protected Sub DrawChart(ByVal persistPalette As Boolean)

        Try

            Dim table As DataTable = ChartDataSource
            Dim query As EnumerableRowCollection(Of DataRow)
            Dim year As Integer = DropDownListStartYear.SelectedValue
            Dim endYear As Integer = DropDownListEndYear.SelectedValue

            While year <= endYear

                query = From row In table.AsEnumerable()
                     Where (row.Field(Of Integer)("Year") = year)
                     Select row

                Dim seriesName As String = String.Format("DefaultSeries{0}", year)

                Dim series As Series = New Series(seriesName)

                With series

                    If Not persistPalette Then
                        Dim colors As Array = [Enum].GetValues(GetType(ChartColorPalette))
                        Dim random As Random = New Random()
                        .Palette = colors.GetValue(random.Next(colors.Length))
                    End If

                    .ChartType = SeriesChartType.Bar
                    .ChartArea = "ChartArea1"
                End With

                Chart1.Series.Add(series)

                Chart1.Series(seriesName).Points.DataBind(query.AsEnumerable(), "CategoryName", "CategorySum", "Label=CategorySum")

                year = year + 1

            End While

        Catch ex As Exception
            Logging.Logger.Log(ex, "DrawChart", String.Empty, UserID, GetConnectionString)
        End Try

    End Sub

    Protected Function GetDataSource(ByVal startYear As Integer, ByVal endYear As Integer) As DataTable

        Dim qry As String = String.Empty
        Dim table As DataTable = New DataTable()

        Try

            qry = "DECLARE @selectedCategories CategorySelectionType" & vbCr

            Dim selectedCategoryIDs As String() = Me.CheckBoxListCategories.Items.Cast(Of ListItem).Where(Function(item) item.Selected = True).Select(Function(item) item.Value).ToArray()

            For Each selectedCategoryID In selectedCategoryIDs
                qry &= String.Format("INSERT INTO @selectedCategories VALUES ({0})", selectedCategoryID) & vbCr
            Next

            qry &= "EXECUTE spGetAllUsersCategorySums  @selectedCategories, @language, @startYear, @endYear, @userID"

            Dim parLanguage As SqlParameter = New SqlParameter("@language", CurrentLanguage)
            Dim parStartYear As SqlParameter = New SqlParameter("@startYear", startYear)
            Dim parEndYear As SqlParameter = New SqlParameter("@endYear", endYear)
            Dim parUserID As SqlParameter = New SqlParameter("@userID", UserID)

            table = DataBaseConnector.GetDataTable(qry, GetConnectionString, parLanguage, parStartYear, parEndYear, parUserID)

        Catch ex As Exception
            Logging.Logger.Log(ex, "DrawChart", qry, UserID, GetConnectionString)
        End Try

        Return table

    End Function

    Protected Sub FillYearsDropDowns()

        DropDownListStartYear.DataSource = Enumerable.Range(2008, DateTime.Now.Year - 2005 + 1)
        DropDownListStartYear.DataBind()

        With DropDownListEndYear
            .DataSource = Enumerable.Range(2008, DateTime.Now.Year - 2008 + 1)
            .DataBind()

            .SelectedIndex = .Items.Count - 1

        End With
    End Sub

    Protected Sub ButtonPlotChart_Click(sender As Object, e As EventArgs) Handles ButtonPlotChart.Click
        ChartDataSource = Nothing
        DrawChart(False)
    End Sub

    Protected Sub ButtonRotateLeft_Click(sender As Object, e As EventArgs) Handles ButtonRotateLeft.Click, ButtonRotateRight.Click

        Select Case CType(sender, Button).ID

            Case ButtonRotateLeft.ID
                ChartRotation -= 10
                Exit Select
            Case ButtonRotateRight.ID
                ChartRotation += 10
                Exit Select

        End Select

        Chart1.ChartAreas(0).Area3DStyle.Rotation = ChartRotation

        DrawChart(True)

    End Sub

    Protected Sub DropDownListPeriodYears_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownListStartYear.SelectedIndexChanged, DropDownListEndYear.SelectedIndexChanged
        ChartDataSource = Nothing
        DrawChart(False)
    End Sub
End Class