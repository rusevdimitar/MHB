Imports MHB.BL

Public Class PurchaseHistoryControl
    Inherits System.Web.UI.UserControl

    Private _en As Environment = New Environment()

    Private _selectedDate As Date
    Public Property SelectedDate() As Date
        Get
            Return Me._selectedDate
        End Get
        Set(ByVal value As Date)
            Me._selectedDate = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub RebindGrid()

        Dim detailsDataSource = Me._en.MainTableDataSource _
                            .Where(Function(exp) exp.HasDetails = True) _
                            .SelectMany(Function(exp) exp.Details) _
                            .Where(Function(det) det.DetailDate.Date = Me._selectedDate) _
                            .Select(Function(det) _
                            New With
                            {
                                .Name = det.DetailName,
                                .Value = det.DetailValue,
                                .Date = det.DetailDate,
                                .Supplier = det.Supplier.Name
                            })

        Dim parentDataSource = Me._en.MainTableDataSource.Where(Function(exp) exp.DateRecordUpdated.Date = Me._selectedDate AndAlso exp.HasDetails = False) _
                            .Select(Function(exp) _
                            New With
                            {
                                .Name = exp.FieldName,
                                .Value = exp.FieldValue,
                                .Date = exp.DateRecordUpdated,
                                .Supplier = String.Empty
                            })

        Dim dataSource = detailsDataSource.Union(parentDataSource)

        With GridViewPurchaseHistoryList
            .DataSource = dataSource
            .DataBind()

            If .FooterRow IsNot Nothing Then

                Dim indexFooterRowCell As Integer = Me._en.GetCellIndexByName(.FooterRow, "Amount")

                Dim labelTotalAmount As Label = CType(.FooterRow.Cells(indexFooterRowCell).FindControl("LabelTotalAmount"), Label)

                labelTotalAmount.Text = dataSource.Sum(Function(exp) exp.Value).ToString("0.0#")

            End If

        End With

    End Sub

End Class