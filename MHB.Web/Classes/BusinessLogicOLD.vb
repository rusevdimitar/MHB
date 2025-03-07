Imports System.Data.SqlClient
Imports MHB.DAL

Public Class BusinessLogicOLD
    Inherits Environment

    Public Function GetMonthBudget(ByVal year As Integer, ByVal month As Integer, ByVal userID As Integer) As Double
        Dim qry As String = String.Empty
        Try

            Dim result As Object = Nothing

            Select Case month

                Case 1
                    qry = "SELECT [BudgetJan] FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = " & userID & " AND Year = " & year.ToString()
                    Exit Select
                Case 2
                    qry = "SELECT [BudgetFeb] FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = " & userID & " AND Year = " & year.ToString()
                    Exit Select
                Case 3
                    qry = "SELECT [BudgetMar] FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = " & userID & " AND Year = " & year.ToString()
                    Exit Select
                Case 4
                    qry = "SELECT [BudgetApr] FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = " & userID & " AND Year = " & year.ToString()
                    Exit Select
                Case 5
                    qry = "SELECT [BudgetMay] FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = " & userID & " AND Year = " & year.ToString()
                    Exit Select
                Case 6
                    qry = "SELECT [BudgetJune] FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = " & userID & " AND Year = " & year.ToString()
                    Exit Select
                Case 7
                    qry = "SELECT [BudgetJuly] FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = " & userID & " AND Year = " & year.ToString()
                    Exit Select
                Case 8
                    qry = "SELECT [BudgetAug] FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = " & userID & " AND Year = " & year.ToString()
                    Exit Select
                Case 9
                    qry = "SELECT [BudgetSept] FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = " & userID & " AND Year = " & year.ToString()
                    Exit Select
                Case 10
                    qry = "SELECT [BudgetOct] FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = " & userID & " AND Year = " & year.ToString()
                    Exit Select
                Case 11
                    qry = "SELECT [BudgetNov] FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = " & userID & " AND Year = " & year.ToString()
                    Exit Select
                Case 12
                    qry = "SELECT [BudgetDec] FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = " & userID & " AND Year = " & year.ToString()
                    Exit Select

            End Select

            result = DataBaseConnector.GetSingleValue(qry, GetConnectionString)
            If IsNumeric(result) Then
                Return CDbl(result)
            Else : Return 0
            End If

        Catch ex As Exception
            Throw New Exception("GetMonthBudget(): " & ex.Message & "qry=" & qry, ex)
        End Try

    End Function

    Public Function GetMonthlySavings(ByVal year As Integer, ByVal month As Months, ByVal userID As Integer) As Double

        Dim qry As String = String.Empty
        Dim result As Object = Nothing

        Try

            Select Case month

                Case Months.January
                    qry = String.Format( _
    "SELECT [SavingsJan] FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year)
                    Exit Select

                Case Months.February
                    qry = String.Format( _
    "SELECT [SavingsFeb] FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year)
                    Exit Select

                Case Months.March
                    qry = String.Format( _
    "SELECT [SavingsMar] FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year)
                    Exit Select

                Case Months.April
                    qry = String.Format( _
    "SELECT [SavingsApr] FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year)
                    Exit Select

                Case Months.May
                    qry = String.Format( _
    "SELECT [SavingsMay] FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year)
                    Exit Select

                Case Months.June
                    qry = String.Format( _
    "SELECT [SavingsJune] FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year)
                    Exit Select

                Case Months.July
                    qry = String.Format( _
    "SELECT [SavingsJuly] FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year)
                    Exit Select

                Case Months.August
                    qry = String.Format( _
    "SELECT [SavingsAug] FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year)
                    Exit Select

                Case Months.September
                    qry = String.Format( _
    "SELECT [SavingsSept] FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year)
                    Exit Select

                Case Months.October
                    qry = String.Format( _
    "SELECT [SavingsOct] FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year)
                    Exit Select

                Case Months.November
                    qry = String.Format( _
    "SELECT [SavingsNov] FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year)
                    Exit Select

                Case Months.December
                    qry = String.Format( _
    "SELECT [SavingsDec] FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year)
                    Exit Select

            End Select

            result = DataBaseConnector.GetSingleValue(qry, GetConnectionString)

            If IsNumeric(result) Then
                Return CDbl(result)
            Else : Return 0
            End If

        Catch ex As Exception
            Throw New Exception("GetMonthlySaings(): " & ex.Message & "qry=" & qry, ex)
        End Try

    End Function

End Class