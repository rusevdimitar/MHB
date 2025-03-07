Imports MHB.BL

Imports MHB.Logging

Public Class Mobile
    Inherits Environment

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If UserID = 0 Then
                Response.Redirect(URLRewriter.GetLink("MobileLogin"))
            End If

            If Not IsPostBack Then
                RebindGrid(Month)
                FillYearDropDown()
            End If

        Catch ex As Exception
            Logger.Log(ex, "Mobile.Page_Load", String.Empty, UserID, GetConnectionString)
        End Try
    End Sub

    Protected Sub RebindGrid(ByVal month As Integer)

        Try

            Dim dataSource As List(Of Expenditure) = New List(Of Expenditure)()

            dataSource = Me.ExpenseManager.GetUserExpenditures(DisplayAllFlaggedSums, False, PaidExpensesHidden)

            GridView1.DataSource = dataSource
            GridView1.DataBind()

        Catch ex As Exception
            Logger.Log(ex, "Mobile.RebindGrid", String.Empty, UserID, GetConnectionString)
        End Try
    End Sub

    Protected Sub FillYearDropDown()

        Try

            For year As Integer = Date.Now.Year - 5 To Date.Now.Year + 2
                DropDownListYears.Items.Add(year)
            Next

            Dim itemYear As ListItem = (From itm As ListItem In DropDownListYears.Items Where itm.Text = Date.Now.Year Select itm).FirstOrDefault()
            Dim itemMonth As ListItem = (From itm As ListItem In DropDownListMonths.Items Where itm.Value = Date.Now.Month Select itm).FirstOrDefault()

            itemYear.Selected = True
            itemMonth.Selected = True

        Catch ex As Exception
            Logger.Log(ex, "Mobile.FillYearDropDown", String.Empty, UserID, GetConnectionString)
        End Try
    End Sub

    Protected Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAdd.Click
        Try

            Dim qry As String = String.Empty

            ExpenseManager.AddNewParentExpense(TextBoxBillValue.Text, False, False, String.Empty, TextBoxBillName.Text, String.Empty, 0, False, qry)

            RebindGrid(Month)

        Catch ex As Exception
            Logger.Log(ex, "Mobile.ButtonAdd_Click", String.Empty, UserID, GetConnectionString)
        End Try

    End Sub

    Protected Sub DropDownListMonths_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListMonths.SelectedIndexChanged
        RebindGrid(DropDownListMonths.SelectedValue)
    End Sub

    Protected Sub DropDownListYears_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListYears.SelectedIndexChanged
        Year = DropDownListYears.SelectedValue

        RebindGrid(Month)
    End Sub
End Class