Imports System.Data.SqlClient
Imports System.Net
Imports System.Xml
Imports MHB.BL
Imports System.Threading.Tasks
Imports MHB.DAL
Imports System.IO
Imports System.Linq.Expressions

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Dim _connectionString As String = ConfigurationManager.ConnectionStrings("connectionString").ConnectionString

    Dim _environment As Environment = New Environment()

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

        Try
            ' Load translations in cache
            Task.Factory.StartNew(AddressOf Me._environment.SetTranslationInCache) _
                .ContinueWith(Sub()
                                  If DateTime.Now.Hour <= 3 Then
                                      ' Send due date notification messages
                                      Me.SendNotificationEmails()
                                  End If
                              End Sub) _
                .ContinueWith(Sub()
                                  ' Rebuild Db indexes every saturday or sunday
                                  If DateTime.Now.Hour <= 3 AndAlso (DateTime.Now.DayOfWeek = DayOfWeek.Saturday OrElse DateTime.Now.DayOfWeek = DayOfWeek.Sunday) Then
                                      Me.RebuildDbIndexes()
                                  End If
                              End Sub) _
                .ContinueWith(Sub()
                                  Me.LoadUrlRewriterAddressesIntoCache()
                              End Sub)

        Catch ex As Exception
            Logging.Logger.Log(ex, "Global.asax.Application_Start", String.Empty, 0, Me._connectionString)
        Finally
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.ApplicationStart, 0, Me._connectionString, String.Empty)
        End Try

    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)

        If Not Debugger.IsAttached Then
            Me._environment.ValidateLicense()
        End If

        Dim ipAddress As String = String.Empty

        Try

            ipAddress = Request.UserHostAddress

            Me._environment.SetTranslationInCache(ipAddress)

            Me.LoadUrlRewriterAddressesIntoCache(ipAddress)

            Task.Factory.StartNew(Sub()
                                      UserManager.User.UpdateUserGeoLocationInfo(0, Me._connectionString, ipAddress) ' Get GEO IP data
                                  End Sub) _
                .ContinueWith(Sub()
                                  Me.SetCurrentExchangeRates(ipAddress) ' Set exchange rates from BNB rss feed
                              End Sub) _
                .ContinueWith(Sub()
                                  Me.LoadExchangeRatesHistoryIntoCache(ipAddress) ' Get exchange rates history
                              End Sub) _
                .ContinueWith(Sub()
                                  If DateTime.Now.Hour.IsEven() Then
                                      Me.DeleteDemoEntries(ipAddress) ' Delete demo entries
                                  End If
                              End Sub) _
                .ContinueWith(Sub()
                                  Me.HandleImportEmails()
                              End Sub)

        Catch ex As Exception
            Logging.Logger.Log(ex, "Global.asax.Session_Start", String.Empty, 0, Me._connectionString)
        Finally
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.SessionStart, 0, Me._connectionString, ipAddress)
        End Try

    End Sub

    Protected Sub RebuildDbIndexes()
        Try
            DataBaseConnector.ExecuteQuery("spMaintainDbIndexes", Me._connectionString)
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.RebuildDbIndexes, 0, Me._connectionString, String.Empty)
        Catch ex As Exception
            Logging.Logger.Log(ex, "Global.asax.RebuildDbIndexes", "spMaintainDbIndexes", 0, Me._connectionString)
        End Try

    End Sub

    Protected Sub SetCurrentExchangeRates(ByVal ipAddress As String)

        Try

            If HttpRuntime.Cache("ExchangeRates") Is Nothing OrElse DateTime.Now.Hour <= 3 Then

                Dim url As String = "http://www.bnb.bg/Statistics/StExternalSector/StExchangeRates/StERForeignCurrencies/index.htm?download=xml&search=&lang=BG"

                Dim resultXml As String = String.Empty, html As String = String.Empty, rateUSD As String = String.Empty, rateGBP As String = String.Empty, rateCHF As String = String.Empty, currencyRateDate As String = String.Empty

                html = File.ReadAllText(Path.Combine(HttpRuntime.BinDirectory, "CustomControls\ExchangeRatesBar.html"))

                Using webClient As WebClient = New WebClient()

                    resultXml = webClient.DownloadString(url)

                End Using

                Dim doc As XmlDocument = New XmlDocument()

                doc.LoadXml(resultXml)

                Dim nodes As XmlNodeList = doc.SelectNodes("//ROWSET/ROW/CODE[text()='USD' or text()='CHF' or text()='GBP']/..")

                For Each node As XmlNode In nodes

                    currencyRateDate = node("CURR_DATE").InnerText

                    Select Case node("CODE").InnerText.ToUpper()

                        Case "USD"
                            rateUSD = node("RATE").InnerText
                            Exit Select
                        Case "CHF"
                            rateCHF = node("RATE").InnerText
                            Exit Select
                        Case "GBP"
                            rateGBP = node("RATE").InnerText
                            Exit Select

                    End Select

                Next

                html = String.Format(html, currencyRateDate, rateUSD, rateCHF, rateGBP)

                HttpRuntime.Cache.Insert("ExchangeRates", html, Nothing, DateTime.Now.AddHours(8), System.Web.Caching.Cache.NoSlidingExpiration)

                Dim qry As String =
                   "INSERT INTO tbCurrencyExchangeRates (ExchangeRateXml, ExchangeRateHtml, ExchangeRateBGNEUR, ExchangeRateBGNUSD, ExchangeRateBGNGBP, ExchangeRateDate, IsVisible, IsDeleted, ExchangeRateBGNCHF)" &
    "VALUES (@ExchangeRateXml, @ExchangeRateHtml, @ExchangeRateBGNEUR, @ExchangeRateBGNUSD, @ExchangeRateBGNGBP, GETDATE(), 0, 0, @ExchangeRateBGNCHF)"

                Dim parameters() As SqlParameter =
                {
                    New SqlParameter("@ExchangeRateXml", resultXml),
                    New SqlParameter("@ExchangeRateHtml", html),
                    New SqlParameter("@ExchangeRateBGNEUR", String.Empty),
                    New SqlParameter("@ExchangeRateBGNUSD", rateUSD),
                    New SqlParameter("@ExchangeRateBGNCHF", rateCHF),
                    New SqlParameter("@ExchangeRateBGNGBP", rateGBP)
                }

                DataBaseConnector.ExecuteQuery(qry, Me._connectionString, parameters)

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.GetExchangeRates, 0, Me._connectionString, ipAddress)

            End If
        Catch ex As Exception
            Logging.Logger.Log(ex, "SetCurrentExchangeRates", String.Empty, 0, Me._connectionString)

        End Try
    End Sub

    Protected Sub DeleteDemoEntries(ByVal ipAddress As String)

        Dim qry As String = String.Empty

        Try
            qry = "EXECUTE spDeleteDemoEntries"

            Dim result As Integer = DataBaseConnector.GetSingleValue(Of Integer)(qry, Me._connectionString)

            If result <> 0 Then
                Logging.Logger.LogAction(Logging.Logger.HistoryAction.DeleteDemoEntries, 0, Me._connectionString, ipAddress)
            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "DeleteDemoEntries", qry, 0, Me._connectionString)

        End Try

    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request

    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        'Me.LogLastLoginTime()

        Logging.Logger.LogAction(Logging.Logger.HistoryAction.ApplicationStart, 0, Me._connectionString, String.Empty)
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        'Me.LogLastLoginTime()

        Logging.Logger.LogAction(Logging.Logger.HistoryAction.ApplicationEnd, 0, Me._connectionString, String.Empty)
    End Sub

    Protected Sub LogLastLoginTime()

        Dim qry As String = String.Empty

        Try
            qry =
"UPDATE sbo.tbUsers SET lastlogintime = GETDATE(), lastipaddress = @userIPAddress, useragent = @userBrowser WHERE userID = @userID"

            Dim parUserIPAddress As SqlParameter = New SqlParameter("@userIPAddress", Request.UserHostAddress)
            Dim parUserBrowser As SqlParameter = New SqlParameter("@userBrowser", Request.UserAgent)
            Dim parUserID As SqlParameter = New SqlParameter("@userID", Me._environment.UserID)

            DataBaseConnector.ExecuteQuery(qry, Me._connectionString, parUserIPAddress, parUserBrowser, parUserID)

        Catch ex As Exception
            Logging.Logger.Log(ex, "Global.asax.LogLastLoginTime: ", qry, Me._environment.UserID, Me._connectionString)
        End Try

    End Sub

    Protected Sub SendNotificationEmails()

        Dim qry As String = String.Empty
        Dim qry0 As String = String.Empty

        Dim email As String = String.Empty
        Dim billID As String = String.Empty
        Dim billName As String = String.Empty
        Dim billDescription As String = String.Empty
        Dim billValue As String = String.Empty
        Dim dueDate As String = String.Empty
        Dim currency As String = String.Empty
        Dim userID As Integer = 0

        Dim senderEmailAddress As String = System.Web.Configuration.WebConfigurationManager.AppSettings("senderEmailAddress")

        qry = "EXECUTE [spGetDueDateEmailReminderMessage]"

        Try
            Dim reader As IDataReader = DataBaseConnector.GetDataReader(qry, Me._connectionString)

            While reader.Read()

                email = reader("email")
                billID = reader("ID")
                billName = reader("FieldName")
                billDescription = reader("FieldDescription")
                billValue = reader("FieldValue")
                dueDate = reader("DueDate")
                currency = reader("currency")
                userID = CInt(reader("userID"))

                Dim message As String =
billName & " - " & billDescription & " " & billValue & " " & currency & vbCrLf &
dueDate & vbCrLf &
"http://www.myhomebills.info"

                If MHB.Mail.Mail.SendMail(message, "[MyHomeBills] Notification message/Известие за краен срок за заплащане на сметка", email, senderEmailAddress) Then

                    qry0 = String.Format("UPDATE {0} SET Notified = 1, NotificationDate = GETDATE() WHERE ID = @id", MHB.BL.ExpenditureManager.GetUserMainTableNames(userID).Item1)

                    DataBaseConnector.ExecuteQuery(qry0, _connectionString, New SqlParameter("@id", billID))

                End If

            End While

            Logging.Logger.LogAction(Logging.Logger.HistoryAction.SendNotificationEmails, 0, _connectionString, String.Empty)

        Catch ex As Exception
            Logging.Logger.Log(ex, "Global.asax.Session_Start.SendNotificationEmails", qry, 0, _connectionString)
        End Try

    End Sub

    Public Async Sub LoadExchangeRatesHistoryIntoCache(ByVal ipAddress As String)

        Try

            If HttpRuntime.Cache("AllExchangeRatesHistory") Is Nothing OrElse DateTime.Now.Hour <= 3 Then

                Dim interval As Integer = New Random().Next(6000, 20000)

                Await Task.Delay(interval)

                Dim reader As IDataReader = DataBaseConnector.GetDataReader("SELECT * FROM tbCurrencyExchangeRates", Me._connectionString)

                Dim currencyExchangeRates As List(Of CurrencyExchangeRate) = New List(Of CurrencyExchangeRate)()

                While reader.Read()

                    Dim exchangeRate As CurrencyExchangeRate = New CurrencyExchangeRate()

                    With exchangeRate

                        .ID = reader.Get(Of Integer)("ID")
                        .EURtoBGN = reader.Get(Of Decimal)("ExchangeRateBGNEUR")
                        .USDtoBGN = reader.Get(Of Decimal)("ExchangeRateBGNUSD")
                        .GBPtoBGN = reader.Get(Of Decimal)("ExchangeRateBGNGBP")
                        .CHFtoBGN = reader.Get(Of Decimal)("ExchangeRateBGNCHF")
                        .Date = reader.Get(Of DateTime)("ExchangeRateDate")

                    End With

                    currencyExchangeRates.Add(exchangeRate)

                End While

                HttpRuntime.Cache.Insert("AllExchangeRatesHistory", currencyExchangeRates, Nothing, DateTime.Now.AddHours(8), System.Web.Caching.Cache.NoSlidingExpiration)

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.GetExchangeRatesHistory, 0, Me._connectionString, ipAddress)

            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "Global.asax.vb.LoadExchangeRatesHistoryIntoCache", String.Empty, 0, Me._connectionString)
        End Try

    End Sub

    Private Sub LoadUrlRewriterAddressesIntoCache(Optional ByVal ipAddress As String = "")

        Try

            If HttpRuntime.Cache("UrlRewriteAddressesList") Is Nothing Then

                Dim rewriter As URLRewriter = New URLRewriter()

                Dim addresses As List(Of RewriteAddress) = rewriter.GetDeserializedAddresses()

                HttpRuntime.Cache.Insert("UrlRewriteAddressesList", addresses.ToArray(), Nothing, DateTime.Now.AddHours(8), System.Web.Caching.Cache.NoSlidingExpiration)

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.LoadUrlRewriterAddressesListIntoCache, 0, Me._connectionString, ipAddress)

            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "Global.asax.vb.LoadUrlRewriterAddressesIntoCache", String.Empty, 0, Me._connectionString)
        End Try

    End Sub

    Public Sub HandleImportEmails()

        Try
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.BeginHandleImportEmails, 0, Me._connectionString, String.Empty)

            Dim addedItems As List(Of String) = New List(Of String)()
            Dim failedItems As List(Of String) = New List(Of String)()

            Dim importedMessages As IEnumerable(Of Mail.ImportMessage) = Mail.Mail.GetImportEmail()

            For Each importedMessage As Mail.ImportMessage In importedMessages

                If importedMessage.MessageItems.Count() > 20 Then
                    Mail.Mail.SendMail("You can only import up to 20 items per email", "[MyHomeBills] You can only import up to 20 items per email", importedMessage.SenderEmailAddress)
                    Continue For
                End If

                Dim apiKey As UserManager.APIKey = UserManager.User.GetAPIKey(importedMessage.SenderEmailAddress, Me._connectionString)

                If apiKey.Key = importedMessage.ApiKey Then

                    Dim expenseManager As ExpenditureManager = New ExpenditureManager(Me._connectionString, apiKey.UserID, DateTime.Now.Month, DateTime.Now.Year, Enums.Language.English)

                    Dim queryArgs = expenseManager.GetExpressionQueryArgs()
                    queryArgs.Add("IsDeleted", False)

                    Dim queryWhereClause As Expression(Of Func(Of Expenditure, Boolean)) = Function(exp) exp.UserID = ExpressionQueryArgs.Parameter(Of Integer)("UserID") AndAlso
                            exp.IsDeleted = ExpressionQueryArgs.Parameter(Of Boolean)("IsDeleted") AndAlso
                            exp.Year = ExpressionQueryArgs.Parameter(Of Integer)("Year") AndAlso
                            exp.Month = ExpressionQueryArgs.Parameter(Of Integer)("Month")

                    Dim expenses As IEnumerable(Of Expenditure) = expenseManager.GetUserExpenditures(queryWhereClause, queryArgs)

                    For Each messageItem As Mail.ImportMessageItem In importedMessage.MessageItems

                        Dim matchingCategoryID As Integer = Me._environment.GetCategoryID(messageItem.CategoryName, expenseManager.Categories)

                        Dim matchingParentExpense As Expenditure = expenses.FirstOrDefault(Function(exp) _
                                                                (exp.CategoryID <> Category.CATEGORY_DEFAULT_ID AndAlso exp.CategoryID = matchingCategoryID) _
                                                                OrElse exp.FieldName.ToUpper() = messageItem.CategoryName.ToUpper())

                        If matchingParentExpense IsNot Nothing Then

                            If Not matchingParentExpense.Details.Any(Function(existingDetail) _
                                                                         existingDetail.DetailName = messageItem.ProductName AndAlso
                                                                         existingDetail.DetailDateCreated = importedMessage.DateSent) Then

                                ' TODO: HANDLE COUNT/WEIGHT/VOLUME
                                Dim newExpenseDetail As ExpenditureDetail = New ExpenditureDetail()

                                With newExpenseDetail
                                    .Parent = matchingParentExpense
                                    .DetailName = messageItem.ProductName
                                    .DetailDescription = Mail.Mail.MHB_DEFAULT_IMPORT_RESPONSE_EMAIL_SUBJECT
                                    .DetailValue = messageItem.Value
                                    .DetailDateCreated = importedMessage.DateSent
                                    .Amount = messageItem.Quantity
                                End With

                                expenseManager.AddNewChildExpense(newExpenseDetail)

                                addedItems.Add(String.Format("Added Detail - Name: {0}; Value: {1}; Amount: {2}", newExpenseDetail.DetailName, newExpenseDetail.DetailValue, newExpenseDetail.Amount))

                                Logging.Logger.LogAction(Logging.Logger.HistoryAction.HandleImportEmailsAddedImportedChildExpense, 0, Me._connectionString, String.Empty)

                                Continue For
                            End If
                        Else
                            expenseManager.AddNewParentExpense(messageItem.ProductName, False, False, Nothing, messageItem.CategoryName, Mail.Mail.MHB_DEFAULT_IMPORT_RESPONSE_EMAIL_SUBJECT, Product.PRODUCT_DEFAULT_ID, Nothing, Nothing)
                            addedItems.Add(String.Format("Added Parent {0} - {1}", messageItem.CategoryName, messageItem.ProductName))
                            Continue For
                        End If

                        failedItems.Add(String.Format("Failed to import:", messageItem.ProductName))
                    Next

                End If

                Try
                    Dim subject As String = "[MyHomeBills] Import successful!"

                    If failedItems.Any() Then

                        subject = "[MyHomeBills] Import failed!"

                        If addedItems.Any() Then
                            subject = "[MyHomeBills] Some items failed to import!"
                        End If

                    End If

                    Mail.Mail.SendMail(String.Format("{0}<br />{1}", String.Join("<br />", addedItems), String.Join("<br />", failedItems)), subject, importedMessage.SenderEmailAddress)

                Catch ex As Exception
                    Logging.Logger.Log(ex, "Environment.HandleImportEmails send response email", String.Empty, 0, Me._connectionString)
                End Try

            Next
        Catch ex As Exception
            Logging.Logger.Log(ex, "Environment.HandleImportEmails", String.Empty, 0, Me._connectionString)
        Finally
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.EndHandleImportEmails, 0, Me._connectionString, String.Empty)
        End Try

    End Sub
End Class