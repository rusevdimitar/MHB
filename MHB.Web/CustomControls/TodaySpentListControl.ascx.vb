Imports MHB.BL


Public Class TodaySpentListControl
    Inherits System.Web.UI.UserControl

    Dim _en As Environment = New Environment()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.IsPostBack Then
            Me.RebindGrid()
        End If

    End Sub

    Public Sub RebindGrid()

        Try
            Dim dataSource As IEnumerable(Of Tuple(Of Integer, String, Decimal)) = Enumerable.Empty(Of Tuple(Of Integer, String, Decimal))()

            Dim detailsDataSource As IEnumerable(Of Tuple(Of Integer, String, Decimal)) = _
                Me._en.MainTableDataSource.SelectMany(Function(m) m.Details).Where(Function(d) d.DetailValue > 0 _
                                                                                       AndAlso (d.DetailValue <> d.DetailInitialValue OrElse d.DetailDateCreated.Date = d.DetailDate.Date) _
                                                                                       AndAlso (d.DetailDateCreated.Date = Date.Today OrElse d.DetailDate.Date = Date.Today)).Select(Function(d) New Tuple(Of Integer, String, Decimal)(d.ID, d.DetailName, IIf(d.DetailValue < d.DetailInitialValue AndAlso d.DetailDateCreated.Date <> Date.Today, d.DetailValue - d.DetailInitialValue, d.DetailValue))).ToArray()

            Dim mainDataSource As IEnumerable(Of Tuple(Of Integer, String, Decimal)) = _
                Me._en.MainTableDataSource.Where(Function(m) m.FieldValue > 0 _
                                                     AndAlso m.HasDetails = False _
                                                     AndAlso (m.DateRecordCreated.Date = Date.Today OrElse m.DateRecordUpdated.Date = Date.Today) _
                                                     AndAlso (m.FieldValue <> m.FieldOldValue OrElse m.FieldValue <> m.FieldInitialValue) _
                                                     AndAlso Me.CheckForPreviousTransactionModifications(m)).Select(Function(m) New Tuple(Of Integer, String, Decimal)(m.ID, m.FieldName, IIf(m.FieldValue < m.FieldOldValue AndAlso m.DateRecordCreated.Date <> Date.Today, m.FieldValue - m.FieldOldValue, m.FieldValue))).ToArray()

            dataSource = detailsDataSource.Union(mainDataSource)

            With GridViewTodaySpentListControl
                .DataSource = dataSource
                .DataBind()
            End With

        Catch ex As Exception
            Logging.Logger.Log(ex, "TodaySpentListControl.RebindGrid", String.Empty, Me._en.UserID, Me._en.GetConnectionString)
        End Try

    End Sub

    Private Function CheckForPreviousTransactionModifications(ByVal expenditure As Expenditure) As Boolean

        Dim result As Boolean = False

        If expenditure.Transactions.All(Function(t) t.DateModified.Date = Date.Today) Then
            result = True
        Else
            Dim transaction As Transaction = expenditure.Transactions.LastOrDefault(Function(t) t.DateModified.Date < Date.Today)

            If transaction IsNot Nothing Then

                If transaction.NewValue <> expenditure.FieldValue Then
                    result = True
                End If

            End If

        End If

        Return result

    End Function

End Class