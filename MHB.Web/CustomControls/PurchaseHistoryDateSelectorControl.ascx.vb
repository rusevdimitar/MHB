Public Class PurchaseHistoryDateSelectorControl
    Inherits System.Web.UI.UserControl

    Private _en As Environment = New Environment()

    Public Event DateSelected(ByVal selectedDate As Date)

    Private _dataSource As IEnumerable(Of KeyValuePair(Of Date, Decimal))
    Public Property DataSource() As IEnumerable(Of KeyValuePair(Of Date, Decimal))
        Get
            Return Me._dataSource
        End Get
        Set(ByVal value As IEnumerable(Of KeyValuePair(Of Date, Decimal)))
            Me._dataSource = value
            Me.RebindRepeater(Me._dataSource)
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub RebindRepeater(ByVal dataSource As IEnumerable(Of KeyValuePair(Of Date, Decimal)))

        With RepeaterPurchaseDates
            .DataSource = dataSource
            .DataBind()
        End With

        LabelTotalSum.Text = dataSource.Sum(Function(s) s.Value).ToString("0.0#")

    End Sub

    Protected Sub RepeaterPurchaseDates_ItemCommand(ByVal sender As Object, ByVal e As RepeaterCommandEventArgs) Handles RepeaterPurchaseDates.ItemCommand

        Dim selectedDate As Date = Date.Now

        If e.CommandArgument IsNot Nothing Then
            Date.TryParse(CStr(e.CommandArgument), selectedDate)
        End If

        RaiseEvent DateSelected(selectedDate)

    End Sub

End Class