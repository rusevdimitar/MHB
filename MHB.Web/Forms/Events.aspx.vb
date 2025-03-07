Imports MHB.BL

Public Class Events
    Inherits Environment

    Protected Overloads Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init

        If Not IsPostBack Then
            CalendarEvents.VisibleDate = New DateTime(Year, Month, 1)
        End If

    End Sub

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.CheckAccess()

        Me.AddNavigationLink(Me, MHB.BL.URLRewriter.GetLink("Events"), GetTranslatedValue("ButtonEventsCalendar", CurrentLanguage))

    End Sub

    Protected Sub CalendarEvents_DayRender(sender As Object, e As System.Web.UI.WebControls.DayRenderEventArgs) Handles CalendarEvents.DayRender

        Try

            'Dim dueDates = dataSource.Select(Function(e) New Date(e.DueDate.Ticks)).Where(Function(d) d.Year > 1900)

            If Not e.Day.IsOtherMonth Then

                Dim scheduledExpenses As List(Of Expenditure) = (From exp As Expenditure In MainTableDataSource Where exp.DueDate.Day = e.Day.DayNumberText AndAlso exp.DueDate.Year > 1900 Select exp).ToList()

                For Each bill As Expenditure In scheduledExpenses

                    With bill

                        Dim name As HtmlGenericControl = New HtmlGenericControl("p")

                        If Not String.IsNullOrEmpty(.FieldDescription) Then
                            name.InnerHtml = String.Format("<code>-{0}┐<br />&nbsp;&nbsp;({1})</code>", .FieldName, .FieldDescription)
                        Else
                            name.InnerHtml = String.Format("<code>-{0} {1}</code>", .FieldName, .FieldDescription)
                        End If

                        If bill.IsPaid Then
                            name.Attributes.Add("style", "color:DarkGreen;")
                            name.InnerHtml += "✓"
                        Else
                            name.Attributes.Add("style", "color:#FF0000;")
                        End If

                        e.Cell.Controls.Add(name)

                        e.Cell.BackColor = Drawing.Color.FromName("#FFF2BF")

                    End With

                Next

                Dim userIncome As List(Of Income) = (From i As Income In IncomeDataSource Where i.Date.Day = e.Day.DayNumberText AndAlso i.Date.Year > 1900 Select i).ToList()

                For Each Income As Income In userIncome

                    With Income

                        Dim name As HtmlGenericControl = New HtmlGenericControl("p")
                        name.InnerHtml = String.Format("<code>{0}<strong>+</strong>{1}┐<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;({2})</code>", GetTranslatedValue("lblNewIncomeName", CurrentLanguage), .Name, Math.Round(.Value, 2))

                        name.Attributes.Add("style", "color:DarkGreen;")

                        e.Cell.Controls.Add(name)

                        e.Cell.BackColor = Drawing.Color.FromName("#abffb1")
                        e.Cell.BorderColor = Drawing.Color.DarkGreen
                        e.Cell.BorderStyle = BorderStyle.Solid
                        e.Cell.BorderWidth = 1

                    End With

                Next

                If scheduledExpenses.Count() > 0 Then

                    Dim currentlyAvailableIncome = IncomeDataSource.Where(Function(i) i.Date <= e.Day.Date).Sum(Function(inc_sum) inc_sum.Value)

                    Dim currentExpenses = MainTableDataSource.Where(Function(exp) exp.DueDate <= e.Day.Date AndAlso exp.DueDate.Year > 1900 AndAlso exp.IsPaid = False).Sum(Function(exp_sum) exp_sum.FieldExpectedValue)

                    Dim sumPaidExpenses = MainTableDataSource.Where(Function(exp) exp.DueDate <= e.Day.Date AndAlso exp.DueDate.Year > 1900 AndAlso exp.IsPaid = True).Sum(Function(exp_sum) exp_sum.FieldValue)

                    Dim availIncomeExpenseDiff As Decimal = currentlyAvailableIncome - currentExpenses - sumPaidExpenses

                    Dim availIncomeDescr As String = String.Empty

                    If availIncomeExpenseDiff <= 0 Then
                        availIncomeDescr = String.Format("<br /><strong style='color:DarkRed'>{0} {1:f2} !!!</strong>", GetTranslatedValue("RestIncomeText", CurrentLanguage), availIncomeExpenseDiff)
                    Else
                        availIncomeDescr = String.Format("<br /><strong style='color:DarkGreen'>{0} +{1:f2}</strong>", GetTranslatedValue("RestIncomeText", CurrentLanguage), availIncomeExpenseDiff)
                    End If

                    Dim plan As HtmlGenericControl = New HtmlGenericControl("p")

                    With plan
                        .InnerHtml += availIncomeDescr
                        .Attributes.Add("style", "vertical-alignment:bottom;")
                        e.Cell.Controls.Add(plan)
                    End With
                End If

            End If
        Catch ex As Exception
            Logging.Logger.Log(ex, "CalendarEvents_DayRender()", String.Empty, UserID, GetConnectionString)
        End Try
    End Sub

    Protected Sub CalendarEvents_VisibleMonthChanged(ByVal sender As Object, ByVal e As MonthChangedEventArgs) Handles CalendarEvents.VisibleMonthChanged

        Me.Month = e.NewDate.Month

        Me.MainTableDataSource = Me.ExpenseManager.GetUserExpenditures(Me.DisplayAllFlaggedSums, False, Me.PaidExpensesHidden)

        Me.IncomeDataSource = Me.ExpenseManager.GetUserIncome(Me.Month)

    End Sub


End Class