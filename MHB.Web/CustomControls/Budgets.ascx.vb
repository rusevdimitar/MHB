Imports System.Data.SqlClient
Imports MHB.BL
Imports MHB.DAL

Partial Public Class Budgets
    Inherits System.Web.UI.UserControl

    Dim _en As Environment = New Environment()

    Dim _previousMonthIncome As IEnumerable(Of Income) = Enumerable.Empty(Of Income)()

    Public Event BudgetUpdated As EventHandler

    Public ReadOnly Property IncomeSum() As Decimal
        Get
            Return Me._en.IncomeDataSource.Sum(Function(i) i.Value)
        End Get
    End Property

    Public ReadOnly Property PreviousMonthIncomeSum() As Decimal
        Get
            Return Me._previousMonthIncome.Sum(Function(i) i.Value)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Me.BindGrid()
            'Me._en.TranslateControls(Me.Controls)
        End If
    End Sub

    Protected Sub GridViewBudgets_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewBudgets.RowCommand
        Dim qry As String = String.Empty
        Try

            Select Case e.CommandName.ToUpper()
                Case "EDIT"

                    Exit Select
                Case "DELETE"

                    Dim income As Income = New Income()

                    With income
                        .ConnectionString = Me._en.GetConnectionString
                        .Delete(CInt(e.CommandArgument))
                    End With

                    Me.BindGrid()

                    Logging.Logger.LogAction(Logging.Logger.HistoryAction.DeleteIncome, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)

                    Exit Select
                Case "UPDATE"

                    Dim linkButton As LinkButton = DirectCast(e.CommandSource, LinkButton)

                    Dim incomeNameText As String = DirectCast(linkButton.NamingContainer.FindControl("txtIncomeName"), TextBox).Text

                    Dim incomeValueText As String = DirectCast(linkButton.NamingContainer.FindControl("txtIncomeValue"), TextBox).Text

                    Dim incomeDateText As String = DirectCast(linkButton.NamingContainer.FindControl("txtIncomeDate"), TextBox).Text

                    Dim incomeValue As Decimal = Me._en.FilterNumericString(incomeValueText)

                    Dim income As Income = New Income()

                    With income
                        .ID = CInt(e.CommandArgument)
                        .ConnectionString = Me._en.GetConnectionString
                        .Name = incomeNameText
                        .Value = incomeValue
                        .UserID = Me._en.UserID

                        If IsDate(incomeDateText) Then
                            .Date = CDate(incomeDateText)
                        End If

                        .Update()
                    End With

                    GridViewBudgets.EditIndex = -1
                    Me.BindGrid()

                    Logging.Logger.LogAction(Logging.Logger.HistoryAction.EditIncome, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)

                    Exit Select
                Case "CANCEL"

                    GridViewBudgets.EditIndex = -1
                    Me.BindGrid()

                    Exit Select

            End Select
        Catch ex As Exception
            Logging.Logger.Log(ex, String.Format("Budgets.ascx.vb > GridViewBudgets_RowCommand: e.CommandName={0}", e.CommandName), qry, Me._en.UserID, Me._en.GetConnectionString)
        Finally
            RaiseEvent BudgetUpdated(Me, EventArgs.Empty)
        End Try
    End Sub

    Public Sub BindGrid()

        Dim qry As String = String.Empty

        Try

            Me._en.IncomeDataSource = Me._en.ExpenseManager.GetUserIncome().OrderBy(Function(i) i.Date)

            Dim previousMonth As Integer = -1
            Dim previousYear As Integer = -1

            If Me._en.Month = 1 Then
                previousYear = Me._en.Year - 1
                previousMonth = 12
            Else
                previousYear = Me._en.Year
                previousMonth = Me._en.Month - 1
            End If

            Me._previousMonthIncome = Me._en.ExpenseManager.GetUserIncome(previousMonth, previousYear).OrderBy(Function(i) i.Date)

            With GridViewBudgets

                .DataSource = Me._en.IncomeDataSource
                .DataBind()

                Me._en.TranslateGridViewControls(GridViewBudgets)

                If .FooterRow IsNot Nothing Then

                    .FooterRow.Cells(3).ColumnSpan = 4

                    Dim lblSum As Label = CType(.FooterRow.FindControl("lblSum"), Label)

                    If lblSum IsNot Nothing Then
                        lblSum.Text = Me._en.GetTranslatedValue("incomesum", Me._en.CurrentLanguage) & " " & String.Format("{0:0.00} {1}", Me.IncomeSum, Me._en.Currency)
                    End If

                    Dim lblBudgetDifference As Label = DirectCast(.FooterRow.FindControl("lblDiffernce"), Label)

                    If lblBudgetDifference IsNot Nothing Then
                        If Me.PreviousMonthIncomeSum > Me.IncomeSum Then
                            lblBudgetDifference.Text = String.Format("(-{0})", Me.PreviousMonthIncomeSum - Me.IncomeSum)
                            lblBudgetDifference.CssClass = "PlainTextErrorExtraLarge"
                        Else
                            lblBudgetDifference.Text = String.Format("(+{0})", Me.IncomeSum - Me.PreviousMonthIncomeSum)
                            lblBudgetDifference.CssClass = "PlainTextGreenExtraLarge"
                        End If

                    End If

                End If

            End With

            Me.UpdateMonthlyBudget(Me._en.Month, Me._en.Year, Me._en.UserID)

        Catch ex As Exception
            Logging.Logger.Log(ex, "Budgets.ascx.vb > BindGrid():", qry, _en.UserID, _en.GetConnectionString)
        End Try
    End Sub

    Protected Sub GridViewBudgets_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridViewBudgets.RowEditing
        GridViewBudgets.EditIndex = e.NewEditIndex()
        Me.BindGrid()
    End Sub

    Protected Sub GridViewBudgets_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridViewBudgets.RowUpdating

    End Sub

    Protected Sub GridViewBudgets_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridViewBudgets.RowCancelingEdit

    End Sub

    Protected Sub lnkShowAddNewIncomePanel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkShowAddNewIncomePanel.Click
        If pnlAddNewIncome.Visible Then
            pnlAddNewIncome.Visible = False
            txtNewIncomeName.Text = String.Empty
            txtNewIncomeValue.Text = String.Empty
        Else
            pnlAddNewIncome.Visible = True
            txtNewIncomeName.Text = String.Empty
            txtNewIncomeValue.Text = String.Empty
        End If
    End Sub

#Region "[ btnAddNewIncome_Click ]"

    Protected Sub btnAddNewIncome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddNewIncome.Click

        Dim qry As String = String.Empty

        Try

            Dim parUserID As SqlParameter = New SqlParameter("@UserID", _en.UserID)
            Dim parMonth As SqlParameter = New SqlParameter("@Month", _en.Month)
            Dim parYear As SqlParameter = New SqlParameter("@Year", _en.Year)

            qry = "spCheckAndInsertBudgetsSavingsExpensesPlaceHolderRows @Year, @UserID"

            DataBaseConnector.ExecuteQuery(qry, _en.GetConnectionString, parYear, parUserID)

            Dim income As Income = New Income()
            With income
                .ConnectionString = _en.GetConnectionString
                .Name = txtNewIncomeName.Text
                .Value = Me._en.FilterNumericString(txtNewIncomeValue.Text)
                If IsDate(txtNewIncomeDate.Text) Then
                    .Date = CDate(txtNewIncomeDate.Text)
                End If
                .UserID = Me._en.UserID
                .Month = Me._en.Month
                .Year = Me._en.Year
                .Add()
            End With

            BindGrid()
            btnCancelAddNewIncome_Click(sender, e)

            Logging.Logger.LogAction(Logging.Logger.HistoryAction.AddIncome, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

        Catch ex As Exception
            Logging.Logger.Log(ex, "Budgets.ascx.vb.btnAddNewIncome_Click:", qry, _en.UserID, _en.GetConnectionString)
        Finally
            RaiseEvent BudgetUpdated(Me, EventArgs.Empty)
        End Try
    End Sub
#End Region

#Region "[ btnCancelAddNewIncome_Click ]"

    Protected Sub btnCancelAddNewIncome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelAddNewIncome.Click

        txtNewIncomeName.Text = String.Empty
        txtNewIncomeValue.Text = String.Empty
        txtNewIncomeDate.Text = String.Empty

        pnlAddNewIncome.Visible = False
    End Sub
#End Region

    Protected Sub UpdateMonthlyBudget(ByVal month As Integer, ByVal year As Integer, ByVal userID As Integer)

        Dim qry As String = String.Empty

        Try

            Dim parIncomeSum As SqlParameter = New SqlParameter("@IncomeSum", IncomeSum)
            Dim parUserID As SqlParameter = New SqlParameter("@UserID", userID)
            Dim parYear As SqlParameter = New SqlParameter("@Year", year)

            Select Case month

                Case 1
                    qry = "UPDATE [dbo].[tbMonthlyBudget] SET [BudgetJan] = @IncomeSum WHERE [UserID] = @UserID AND Year = @Year"
                    Exit Select
                Case 2
                    qry = "UPDATE [dbo].[tbMonthlyBudget] SET [BudgetFeb] = @IncomeSum WHERE [UserID] = @UserID AND Year = @Year"
                    Exit Select
                Case 3
                    qry = "UPDATE [dbo].[tbMonthlyBudget] SET [BudgetMar] = @IncomeSum WHERE [UserID] = @UserID AND Year = @Year"
                    Exit Select
                Case 4
                    qry = "UPDATE [dbo].[tbMonthlyBudget] SET [BudgetApr] = @IncomeSum WHERE [UserID] = @UserID AND Year = @Year"
                    Exit Select
                Case 5
                    qry = "UPDATE [dbo].[tbMonthlyBudget] SET [BudgetMay] = @IncomeSum WHERE [UserID] = @UserID AND Year = @Year"
                    Exit Select
                Case 6
                    qry = "UPDATE [dbo].[tbMonthlyBudget] SET [BudgetJune] = @IncomeSum WHERE [UserID] = @UserID AND Year = @Year"
                    Exit Select
                Case 7
                    qry = "UPDATE [dbo].[tbMonthlyBudget] SET [BudgetJuly] = @IncomeSum WHERE [UserID] = @UserID AND Year = @Year"
                    Exit Select
                Case 8
                    qry = "UPDATE [dbo].[tbMonthlyBudget] SET [BudgetAug] = @IncomeSum WHERE [UserID] = @UserID AND Year = @Year"
                    Exit Select
                Case 9
                    qry = "UPDATE [dbo].[tbMonthlyBudget] SET [BudgetSept] = @IncomeSum WHERE [UserID] = @UserID AND Year = @Year"
                    Exit Select
                Case 10
                    qry = "UPDATE [dbo].[tbMonthlyBudget] SET [BudgetOct] = @IncomeSum WHERE [UserID] = @UserID AND Year = @Year"
                    Exit Select
                Case 11
                    qry = "UPDATE [dbo].[tbMonthlyBudget] SET [BudgetNov] = @IncomeSum WHERE [UserID] = @UserID AND Year = @Year"
                    Exit Select
                Case 12
                    qry = "UPDATE [dbo].[tbMonthlyBudget] SET [BudgetDec] = @IncomeSum WHERE [UserID] = @UserID AND Year = @Year"
                    Exit Select
            End Select

            DataBaseConnector.ExecuteQuery(qry, _en.GetConnectionString, parIncomeSum, parUserID, parYear)

        Catch ex As Exception
            Logging.Logger.Log(ex, "Budgets.ascx.vb > UpdateMonthlyBudget() ", qry, _en.UserID, _en.GetConnectionString)
        End Try
    End Sub

    Protected Sub GridViewBudgets_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridViewBudgets.RowDeleting

    End Sub

    Protected Sub GridViewBudgets_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridViewBudgets.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lblIncomeDate As Label = CType(e.Row.FindControl("lblIncomeDate"), Label)

            If lblIncomeDate IsNot Nothing AndAlso IsDate(lblIncomeDate.Text) Then
                lblIncomeDate.Text = Convert.ToDateTime(lblIncomeDate.Text).ToShortDateString()
            End If

        End If
    End Sub
End Class