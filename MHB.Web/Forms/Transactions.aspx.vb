Imports MHB.BL

Public Class Transactions
    Inherits Environment

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsNumeric(Request.QueryString("ID")) Then
            RepeaterTransactions.DataSource = Transaction.GetAll(Request.QueryString("ID"), GetConnectionString)
            RepeaterTransactions.DataBind()

            If RepeaterTransactions.Items.Count = 0 Then

                Dim emptyDataSource As List(Of Transaction) = New List(Of Transaction)()
                emptyDataSource.Add(New Transaction With {.ID = 0, .TransactionText = "No transactions have been recorded for this item."})
                RepeaterTransactions.DataSource = emptyDataSource
                RepeaterTransactions.DataBind()
            End If
        End If

    End Sub

    Protected Sub RepeaterTransactions_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RepeaterTransactions.ItemDataBound

        Dim labelDifference As Label = CType(e.Item.FindControl("LabelDifference"), Label)

        Dim newValue As Decimal = DataBinder.Eval(e.Item.DataItem, "NewValue")
        Dim oldValue As Decimal = DataBinder.Eval(e.Item.DataItem, "OldValue")

        If (newValue > oldValue) Then
            labelDifference.ForeColor = Drawing.Color.Green
            labelDifference.Text = String.Format("+{0}", newValue - oldValue)
        Else
            labelDifference.ForeColor = Drawing.Color.Red
            labelDifference.Text = String.Format("{0}", newValue - oldValue)
        End If

    End Sub
End Class