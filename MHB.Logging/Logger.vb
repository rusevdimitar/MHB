Imports System.Data.SqlClient
Imports MHB.DAL

Public Class Logger

    Public Enum HistoryAction
        Login = 0
        Register = 1
        Update = 2
        AddNew = 3
        Delete = 4
        Attach = 5
        AttachDetails = 6
        AddDetails = 7
        DeleteDetails = 8
        UpdateDetails = 9
        AttachToDetails = 10
        LogOut = 11
        AnnualReportCharts = 12
        AnnualReportPDFExcel = 13
        RecoverPassword = 14
        VideoTutorial = 15
        ScreenShots = 16
        ChangeCurrentLanguageEnglish = 17
        ChangeCurrentLanguageDeutsch = 18
        ChangeCurrentLanguageBulgarian = 19
        ChangeCurrency = 20
        ChangePassword = 21
        DeleteDemoEntries = 22
        StartDemo = 23
        AddIncome = 24
        DeleteIncome = 25
        EditIncome = 26
        FlagBill = 27
        PerformSearch = 28
        InitiateSearchWindow = 29
        ChangeMonth = 30
        CopyParentExpense = 31
        AnnualCategoryCharts = 32
        AddCategory = 33
        UpdateCategory = 34
        EditCategory = 35
        DeleteCategory = 36
        CancelEditCategory = 37
        Help = 38
        Events = 39
        GetExchangeRates = 40
        CopyUserCategory = 41
        Statistics = 42
        RebuildDbIndexes = 43
        LoadTranslationsInCache = 44
        CurrencyExchangeRatesCharts = 45
        GenerateAPIKeySuccess = 46
        GenerateAPIKeyInvalidCredentials = 47
        API_DeleteParentExpenditure = 48
        API_DeleteChildExpenditures = 49
        API_AddParentExpenditure = 50
        API_SearchUserExpenditures = 51
        API_GetExpenditureDetails = 52
        API_UpdateParentExpenses = 53
        API_GetUsersAverageSumForCategory = 54
        API_CopyParentExpense = 55
        API_DeleteAttachment = 56
        API_GetMaximumExpenditureForCategory = 57
        API_GetUserIncome = 58
        API_DuplicateExpenditures = 59
        API_GetYearlyExpensesProMonth = 60
        API_GetYearlyBudgetsProMonth = 61
        API_GetYearlySavingsProMonth = 62
        API_GetUserExpenditures = 63
        API_GetExpenditures = 64
        API_GetActionLogs = 65
        API_GetExceptionLogs = 66
        API_BlockUser = 67
        API_BanIP = 68
        API_GetSingleValue = 69
        API_ExecuteQuery = 70
        API_GetDataReader = 71
        API_GetDataTable = 72
        SendNotificationEmails = 73
        AddCategoryComment = 74
        VoteUpOnCategoryComment = 75
        VoteDownOnCategoryComment = 76
        DeleteCategoryComment = 77
        DeleteProduct = 78
        CancelEditProduct = 79
        UpdateProduct = 80
        DeleteSupplier = 81
        CancelEditSupplier = 82
        UpdateSupplier = 83
        EditSupplier = 84
        EditProduct = 85
        GetExchangeRatesHistory = 86
        ProductPriceStatistics = 87
        NavigateToBillDate = 88
        LoadUrlRewriterAddressesListIntoCache = 89
        ChangeUserLanguage = 90
        ChangeUserAutoLoginSettings = 91
        SetTranslationInCache = 92
        SessionStart = 93
        ApplicationStart = 94
        ApplicationEnd = 95
        SessionEnd = 96
        EndValidateLicense = 97
        ValidateLicenseSuccessful = 98
        DownloadMyHomeBillsInstaller = 99
        DownloadPageOpened = 100
        BlackListCheckCheckOK = 101
        BlackListCheckCheck_BANNED = 102
        EndHandleImportEmails = 103
        BeginHandleImportEmails = 104
        HandleImportEmailsAddedImportedChildExpense = 105
    End Enum

    ''' <summary>
    ''' Logs the specified Exception.
    ''' </summary>
    ''' <param name="ex">The Exception object.</param>
    ''' <param name="method">The method.</param>
    ''' <returns>Boolean</returns>
    Public Shared Function Log(ByVal ex As Exception, ByVal method As String, ByVal qry As String, ByVal userID As Integer, ByVal connectionString As String) As Boolean
        Try

            Debug.WriteLine(String.Format("[{0}]: {1}", method, ex.Message))

            Dim InnerException As String = ""
            If ex.InnerException IsNot Nothing Then
                If ex.InnerException.Message.Length > 500 Then
                    InnerException = ex.InnerException.Message.Substring(0, 499).Replace("'", String.Empty)
                Else
                    InnerException = ex.InnerException.Message.Replace("'", String.Empty)
                End If
            Else
                InnerException = "No InnerException information provided"
            End If

            Dim StackTrace As String = ""
            If ex.StackTrace IsNot Nothing Then
                If ex.StackTrace.Length > 500 Then
                    StackTrace = ex.StackTrace.Substring(0, 499).Replace("'", String.Empty)
                Else
                    StackTrace = ex.StackTrace.Replace("'", String.Empty)
                End If
            Else
                StackTrace = "No StackTrace information provided"
            End If

            Dim Source As String = ""
            If ex.Source IsNot Nothing Then
                If ex.Source.Length > 500 Then
                    Source = ex.Source.Substring(0, 499).Replace("'", String.Empty)
                Else
                    Source = ex.Source.Replace("'", String.Empty)
                End If

            Else
                Source = "No Source information provided"
            End If

            Dim qryToLog As String = ""
            If qry.Length > 500 Then
                qry = qry.Replace("'", "''")
                qryToLog = qry.Substring(0, 499)
            Else
                qryToLog = qry.Replace("'", "''")
            End If

            Dim msgToLog As String = ""
            If ex.Message.Length > 500 Then
                msgToLog = ex.Message.Replace("'", String.Empty).Substring(0, 499)
            Else
                msgToLog = ex.Message.Replace("'", String.Empty)
            End If

            Dim sql As String =
"INSERT INTO [dbo].[tbLog] VALUES ('" & method & "','" & msgToLog & "','" & InnerException & "','" & Source & "','" & StackTrace & "'," & userID.ToString() & ",'" & DateTime.Now.ToString() & "', '" & qryToLog & "')"

            DataBaseConnector.ExecuteQuery(sql, connectionString)

            Return True
        Catch
            Return False
        Finally

        End Try
    End Function

    Public Shared Function LogAction(ByVal action As HistoryAction, ByVal userID As Integer, ByVal connectionString As String, ByVal hostAddress As String, Optional ByVal connection As SqlConnection = Nothing) As Boolean
        Try

            If connection Is Nothing AndAlso String.IsNullOrEmpty(connectionString) Then
                Throw New ArgumentNullException("MHB.Logging.Logger.LogAction: connectionString parameter is null or empty! Either provide a valid connection string or pass an existing SqlConnection!")
            End If

            If connection Is Nothing Then
                connection = New SqlConnection(connectionString)
            End If

            If hostAddress Is Nothing Then
                hostAddress = String.Empty
            End If

            Dim parLogAction As SqlParameter = New SqlParameter("@LogAction", CInt(action))
            Dim parLogUserID As SqlParameter = New SqlParameter("@LogUserID", userID)
            Dim parLogMessage As SqlParameter = New SqlParameter("@LogMessage", action.ToString())
            Dim parLogIP As SqlParameter = New SqlParameter("@LogIP", hostAddress)

            DataBaseConnector.ExecuteQuery(
"INSERT INTO [dbo].[tbActionLog] ([LogAction], [logUserID], [logDate], [logMessage], [logIP]) VALUES " &
"(@LogAction, @LogUserID, GETDATE(), @LogMessage,@LogIP)", connection, parLogAction, parLogUserID, parLogMessage, parLogIP)

        Catch ex As Exception
            Logging.Logger.Log(ex, "Logging.Logger.LogAction()", action.ToString(), userID, connectionString)
        End Try
    End Function
End Class