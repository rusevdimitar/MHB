Imports MHB.BL

Partial Public Class SearchControl
    Inherits System.Web.UI.UserControl

    Dim _en As Environment = New Environment()

    Dim _searchResult As List(Of MHB.BL.Expenditure) = New List(Of MHB.BL.Expenditure)
    Public ReadOnly Property SearchResult() As List(Of MHB.BL.Expenditure)
        Get
            Return Me._searchResult
        End Get
    End Property

    Public Event SearchPerformed As EventHandler

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then
            Call FillYearDdl()
            Call FillSearchByCategoryDdl()
            Call FillSortByDdl()
        End If

        If Me.IsPostBack Then

            If Request.Form("__EVENTTARGET") = ButtonPerformSearch.ClientID Then

                Me.ButtonPerformSearch_Click(sender, e)

            End If

        End If

    End Sub

    Protected Sub ButtonPerformSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonPerformSearch.Click
        Me.PerformSearch()
    End Sub

    Protected Sub FillYearDdl()
        Try
            For i As Integer = DateTime.Now.Year - 10 To DateTime.Now.Year + 20
                DropDownListYear.Items.Add(i)
            Next

        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomControls\SearchControl.ascx.vb\FillYearDropDownList()", String.Empty, Me._en.UserID, Me._en.GetConnectionString)
        End Try

    End Sub

    Protected Sub FillSortByDdl()
        Try
            With DropDownListSortBy
                .DataTextField = "Name"
                .DataValueField = "ID"
                .DataSource = SortOption.GetAll(Me._en.GetConnectionString, Me._en.CurrentLanguage)
                .DataBind()
            End With
        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomControls\SearchControl.ascx.vb\FillSortByDdl()", String.Empty, Me._en.UserID, Me._en.GetConnectionString)
        End Try
    End Sub

    Public Sub PerformSearch()

        Try

            Me._searchResult = Me._en.ExpenseManager.SearchUserExpenditures(RadioButtonListSearchOptions.SelectedValue, CheckBoxSeachByYearToo.Checked, DropDownListYear.SelectedValue, TextBoxSearchString.Text, TextBoxSum.Text, DropDownListHigherOrLowerThan.SelectedValue, CheckBoxSearchByCategory.Checked, DropDownListCategory.SelectedValue, DropDownListSortBy.SelectedValue, DropDownListSortDirection.SelectedValue)

            Dim sender As Object = New Object()

            RaiseEvent SearchPerformed(sender, EventArgs.Empty)

            Logging.Logger.LogAction(Logging.Logger.HistoryAction.PerformSearch, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomControls\SearchControl.ascx.vb\ButtonPerformSearch_Click()", String.Empty, Me._en.UserID, Me._en.GetConnectionString)
        End Try

    End Sub

    Protected Sub FillSearchByCategoryDdl()

        Try

            With DropDownListCategory
                .DataTextField = "Name"
                .DataValueField = "ID"
                .DataSource = Me._en.Categories
                .DataBind()
            End With

        Catch ex As Exception
            Logging.Logger.Log(ex, "CustomControls\SearchControl.ascx.vb\FillSearchByCategoryDdl()", String.Empty, Me._en.UserID, Me._en.GetConnectionString)
        End Try
    End Sub
End Class