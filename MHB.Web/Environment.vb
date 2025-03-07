Imports System.Data.SqlClient
Imports MHB.BL
Imports System.IO
Imports System.Net
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports MHB.BL.Enums
Imports System.CodeDom.Compiler
Imports System.CodeDom
Imports MHB.DAL
Imports MHB.Licensing
Imports MHB.Logging
Imports AjaxControlToolkit
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Linq.Expressions
Imports System.Threading.Tasks

Public Class Environment
    Inherits System.Web.UI.Page

    Public Const EMPTY_API_KEY As String = "00000000000000000000000000000000"

    Public Const COMMAND_SELECT As String = "Select"
    Public Const COMMAND_COPY_PASTE_DETAIL As String = "CopyPasteDetail"
    Public Const COMMAND_PRINT_SHOPPING_LIST_DETAIL As String = "AddToShoppingListDetails"
    Public Const COMMAND_EDIT_DETAIL_PRODUCT As String = "EditDetailProduct"
    Public Const COMMAND_DETAIL_PRODUCT_PRICE_STATISTICS As String = "DetailProductPriceStatistics"
    Public Const COMMAND_DETAIL_MERGE As String = "Merge"
    Public Const COMMAND_DETAIL_APPROVE_SUGGESTED_PRODUCT As String = "DetailProductApproveSuggestedProduct"
    Public Const COMMAND_DETAIL_REJECT_SUGGESTED_PRODUCT As String = "DetailProductRejectSuggestedProduct"
    Public Const COMMAND_DETAIL_CHANGE_MEASURE_TYPE As String = "ChangeMeasureTypeDetail"
    Public Const COMMAND_NAVIGATE_TO_BILL_DATE = "NavigateToBillDate"
    Public Const COMMAND_SHARE_MAIN_ATTACHMENT = "ShareMainAttachment"
    Public Const COMMAND_SHARE_DETAIL_ATTACHMENT = "ShareDetailAttachment"
    Public Const DEFAULT_DETAILS_GRID_PAGE_SIZE = 20
    Public Const COMMAND_DELETE_DEFAULT = "Delete"
    Public Const COMMAND_CLEAR_DEFAULT = "Clear"
    Public Const COMMAND_MARK_SURPLUS = "MarkSurplus"

    Public ReadOnly BULGARIAN_ALPHABET_LOWERCASE As Char() = New Char() {"а", "б", "в", "г", "д", "е", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ь", "ю", "я"}

    Public ReadOnly BULGARIAN_ALPHABET_TRANSLITERATED_LOWERCASE As Char() = New Char() {"a", "b", "w", "g", "d", "e", "j", "z", "i", "i", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "h", "c", "`", "[", "]", "y", "y", "ju", "q"}

    Public ReadOnly vbSpace As Char = CType(Space(1), Char)

    Public Enum Months
        January = 1
        February = 2
        March = 3
        April = 4
        May = 5
        June = 6
        July = 7
        August = 8
        September = 9
        October = 10
        November = 11
        December = 12
    End Enum

#Region "[ Properties ]"

    Public ReadOnly Property Translations As IEnumerable(Of Translation)
        Get
            If Session("Translations") Is Nothing Then
                Session("Translations") = DirectCast(HttpRuntime.Cache("translations"), IEnumerable(Of Translation))
            End If
            Return CType(Session("Translations"), IEnumerable(Of Translation))
        End Get
    End Property

    Public Property LastEditedParentExpenditures() As List(Of Expenditure)
        Get

            If Session("LastEditedParentExpenditures") Is Nothing Then
                Session("LastEditedParentExpenditures") = New List(Of Expenditure)
            End If

            Return CType(Session("LastEditedParentExpenditures"), List(Of Expenditure))
        End Get
        Set(ByVal value As List(Of Expenditure))
            Session("LastEditedParentExpenditures") = value
        End Set
    End Property
    Public Property CurrentUser As UserManager.User
        Get
            If Session("CurrentUser") Is Nothing Then
                Dim user As UserManager.User = New UserManager.User(New SqlConnection(Me.GetConnectionString), Me.UserID)
                Session("CurrentUser") = user
                Me.CurrentLanguage = CType(user.SelectedLanguage, Enums.Language)
            End If
            Return CType(Session("CurrentUser"), UserManager.User)
        End Get
        Protected Set(ByVal value As UserManager.User)
            Session("LastSelectedParentWithDetails") = value
        End Set
    End Property

    Public Property LastAddedMultipleDetailsIDs() As List(Of Integer)
        Get

            If Session("LastAddedMultipleDetails") Is Nothing Then
                Session("LastAddedMultipleDetails") = New List(Of Integer)
            End If

            Return CType(Session("LastAddedMultipleDetails"), List(Of Integer))
        End Get
        Set(ByVal value As List(Of Integer))
            Session("LastAddedMultipleDetails") = value
        End Set
    End Property

    Public Property LastSelectedParentWithDetails() As Integer
        Get
            If Not IsNumeric(Session("LastSelectedParentWithDetails")) Then
                Session("LastSelectedParentWithDetails") = -1
            End If
            Return CInt(Session("LastSelectedParentWithDetails"))
        End Get
        Set(ByVal value As Integer)
            Session("LastSelectedParentWithDetails") = value
        End Set
    End Property

    Public Property PageSizeDetails() As Integer
        Get
            If Not IsNumeric(Session("PageSizeDetails")) Then
                Session("PageSizeDetails") = Environment.DEFAULT_DETAILS_GRID_PAGE_SIZE
            End If
            Return CInt(Session("PageSizeDetails"))
        End Get
        Set(ByVal value As Integer)
            Session("PageSizeDetails") = value
        End Set
    End Property

    Public Property SelectedDetails As ExpenditureDetail()
        Get
            If Session("SelectedDetails") Is Nothing Then
                Session("SelectedDetails") = New ExpenditureDetail() {}
            End If
            Return CType(Session("SelectedDetails"), ExpenditureDetail())
        End Get
        Set(value As ExpenditureDetail())
            Session("SelectedDetails") = value
        End Set
    End Property

    Public Property IsInEditMode() As Boolean
        Get
            If Session("IsInEditMode") Is Nothing Then
                Session("IsInEditMode") = False
            End If
            Return CBool(Session("IsInEditMode"))
        End Get
        Set(ByVal value As Boolean)
            Session("IsInEditMode") = value
        End Set
    End Property

    Public ReadOnly Property ShoppingListsRootPath As String
        Get
            Dim path As String = ConfigurationManager.AppSettings("ShoppingListsRootPath")

            If String.IsNullOrEmpty(path) Then
                path = "\ShoppingLists\"
            End If

            path = Server.MapPath(path)

            If Not System.IO.Directory.Exists(path) Then
                System.IO.Directory.CreateDirectory(path)
            End If

            Return path

        End Get
    End Property

    Public Property ShoppingList As List(Of Tuple(Of String, Product, Decimal))
        Get
            If Session("ShoppingList") Is Nothing Then
                Session("ShoppingList") = New List(Of Tuple(Of String, Product, Decimal))
            End If
            Return CType(Session("ShoppingList"), List(Of Tuple(Of String, Product, Decimal)))
        End Get
        Set(value As List(Of Tuple(Of String, Product, Decimal)))
            Session("ShoppingList") = value
        End Set
    End Property

    Public ReadOnly Property HomeIPAddresses As IPAddress()
        Get
            Return Regex.Replace(ConfigurationManager.AppSettings("HomeIP"), "\s", String.Empty).Split(",").Select(Function(a) IPAddress.Parse(a)).ToArray()
        End Get
    End Property

    Public ReadOnly Property ExchangeRatesHistory As IEnumerable(Of CurrencyExchangeRate)
        Get
            If HttpRuntime.Cache("AllExchangeRatesHistory") IsNot Nothing Then
                Return CType(HttpRuntime.Cache("AllExchangeRatesHistory"), IEnumerable(Of CurrencyExchangeRate))
            Else
                Return Enumerable.Empty(Of CurrencyExchangeRate)()
            End If

        End Get
    End Property

    Public ReadOnly Property SMTPServerAddress As String
        Get
            Return System.Web.Configuration.WebConfigurationManager.AppSettings("smtp")
        End Get
    End Property

    Public ReadOnly Property SnowFallEnabled() As Boolean
        Get
            Return CBool(System.Web.Configuration.WebConfigurationManager.AppSettings("SnowFallEnabled"))
        End Get
    End Property

    Public ReadOnly Property SenderEmailAddress As String
        Get
            Return System.Web.Configuration.WebConfigurationManager.AppSettings("senderEmailAddress")
        End Get
    End Property

    Public ReadOnly Property EmailTemplatePlaceHolderCode As String
        Get
            Return System.Web.Configuration.WebConfigurationManager.AppSettings("emailTemplatePlaceHolderCode")
        End Get
    End Property

    Public Property CategoryIcons As List(Of String)
        Get
            If Session("CategoryIcons") Is Nothing Then
                Session("CategoryIcons") = LoadIcons()
            End If
            Return CType(Session("CategoryIcons"), List(Of String))
        End Get
        Set(value As List(Of String))
            Session("CategoryIcons") = value
        End Set
    End Property

    ''' <summary>
    ''' Returns the whole Email template HTML page as a string.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property EmailTemplateLetter As String
        Get
            Dim message As String =
            System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(
                                       System.Web.Configuration.WebConfigurationManager.AppSettings("emailTemplatePath")))

            If message IsNot Nothing Then
                Return message
            Else : Return String.Empty
            End If

        End Get
    End Property

    Public ReadOnly Property MainTable() As String
        Get
            If Session("MainTable") Is Nothing Then

                Session("MainTable") = ExpenditureManager.GetUserMainTableNames(Me.UserID).Item1

            End If

            Return Session("MainTable").ToString()
        End Get

    End Property

    Public ReadOnly Property DetailsTable() As String
        Get
            If Session("DetailsTable") Is Nothing Then

                Session("DetailsTable") = ExpenditureManager.GetUserMainTableNames(Me.UserID).Item2

            End If

            Return Session("DetailsTable").ToString()
        End Get

    End Property

    Public ReadOnly Property LastUpdate() As Date
        Get
            If Session("DateLastUpdate") Is Nothing Then

                Dim qry As String = String.Empty

                Try
                    qry = String.Format("SELECT MAX(DateRecordUpdated) FROM {0} WHERE UserID = @UserID AND [Month] = @Month AND [Year] = @Year", Me.MainTable)

                    Session("DateLastUpdate") = DataBaseConnector.GetSingleValue(Of DateTime)(qry, Me.GetConnectionString,
                                                                                              New SqlParameter("UserID", Me.UserID),
                                                                                              New SqlParameter("Month", Me.Month),
                                                                                             New SqlParameter("Year", Me.Year))
                Catch ex As Exception
                    Logging.Logger.Log(ex, "Environment.LastUpdate", qry, Me.UserID, Me.GetConnectionString)
                End Try

            End If

            Return CDate(Session("DateLastUpdate"))

        End Get
    End Property

    Public ReadOnly Property Categories As IEnumerable(Of Category)
        Get
            Return Me.ExpenseManager.Categories
        End Get
    End Property

    Public ReadOnly Property Suppliers As IEnumerable(Of Supplier)
        Get
            Return Me.ExpenseManager.Suppliers
        End Get
    End Property

    Public ReadOnly Property Products As IEnumerable(Of Product)
        Get
            Return Me.ExpenseManager.Products
        End Get
    End Property

    Public Property userLastlogintime() As DateTime
        Get
            Return CDate(IIf(IsDBNull(Session("userLastlogintime")), Today().ToString(), Session("userLastlogintime")))
        End Get
        Set(ByVal value As DateTime)
            Session("userLastlogintime") = value
        End Set
    End Property

    Public Property userUseragent() As String
        Get
            Return CStr(IIf(IsDBNull(Session("userUseragent")), "", Session("userUseragent")))
        End Get
        Set(ByVal value As String)
            Session("userUseragent") = value
        End Set
    End Property

    Public ReadOnly Property userLastIPAddress() As String
        Get
            If Session("lastSession") Is Nothing Then

                Dim lastIPAddress As String = String.Empty
                Dim lastLoginTime As String = String.Empty
                Dim userAgent As String = String.Empty

                UserManager.User.GetLastIPAddress(UserID, GetConnectionString, lastLoginTime, lastIPAddress, userAgent)

                Session("lastSession") = GetTranslatedValue("lastlogintime", CurrentLanguage) & lastLoginTime & GetTranslatedValue("fromlastip", CurrentLanguage) & lastIPAddress & " Browser: " & userAgent
            End If

            Return Session("lastSession").ToString()

        End Get

    End Property

    Private _transactionManager As TransactionManager
    Public ReadOnly Property TransactManager() As TransactionManager
        Get
            If Me._transactionManager Is Nothing Then
                Me._transactionManager = New TransactionManager()
            End If
            Return Me._transactionManager
        End Get
    End Property

    Public ReadOnly Property ExpenseManager() As ExpenditureManager
        Get
            If Session("ExpenditureManager") Is Nothing Then
                Me.InitExpenditureManager()

            End If
            Return CType(Session("ExpenditureManager"), ExpenditureManager)
        End Get
    End Property

    Public Property CurrentLanguage() As Language
        Get
            If Session("lang") Is Nothing Then

                If Me.UserID = 0 Then
                    Return Language.English
                End If

            End If

            Return CType(Session("lang"), Language)
        End Get
        Set(ByVal value As Language)
            Session("lang") = value
        End Set
    End Property

    Public Property IsInSearchMode() As Boolean
        Get
            If Session("IsInSearchMode") Is Nothing Then
                Session("IsInSearchMode") = False
            End If
            Return CBool(Session("IsInSearchMode"))
        End Get
        Set(ByVal value As Boolean)
            Session("IsInSearchMode") = value
        End Set
    End Property

    Public Property HasSetCurrentLanguage() As Boolean
        Get
            If Session("hasSetLang") Is Nothing Then

                Session("hasSetLang") = DataBaseConnector.GetSingleValue(Of Boolean)(
"SELECT [hassetlang] FROM [dbo].[tbUsers] WHERE [userID] = @UserID", Me.GetConnectionString, New SqlParameter("UserID", Me.UserID))

                If IsDBNull(Session("hasSetLang")) Then
                    Session("hasSetLang") = False
                End If
            End If
            Return CBool(Session("hasSetLang"))
        End Get
        Set(ByVal value As Boolean)
            Session("hasSetLang") = value
        End Set
    End Property

    Public ReadOnly Property AttachmentMaxSize() As Integer
        Get
            If Session("AttachmentMaxSize") Is Nothing Then

                Session("AttachmentMaxSize") =
                DataBaseConnector.GetSingleValue(Of Integer)("SELECT [attachmentsize] FROM [dbo].[tbUsers] WHERE [userID] = @UserID", Me.GetConnectionString, New SqlParameter("UserID", Me.UserID))

                If Not IsDBNull(Session("AttachmentMaxSize")) Then
                    Return CInt(Session("AttachmentMaxSize"))
                Else
                    Session("AttachmentMaxSize") = 500000
                    Return CInt(Session("AttachmentMaxSize"))
                End If

            End If
            Return CInt(Session("AttachmentMaxSize"))
        End Get
    End Property

    Public ReadOnly Property GetConnectionString() As String
        Get
            If Session("connectionString") Is Nothing Then
                Session("connectionString") = ConfigurationManager.ConnectionStrings("connectionString").ToString()
            End If
            Return Session("connectionString").ToString()
        End Get
    End Property

    Public Property UserID() As Integer
        Get
            If Session("UserID") Is Nothing Then
                Session("UserID") = 0
            End If
            Return CInt(Session("UserID"))
        End Get
        Set(ByVal value As Integer)
            Session("UserID") = value
        End Set
    End Property

    Public Property EditIndex() As Integer
        Get
            If Session("EditIndex") Is Nothing Then
                Session("EditIndex") = 0
            End If
            Return CInt(Session("EditIndex"))
        End Get
        Set(ByVal value As Integer)
            Session("EditIndex") = value
        End Set
    End Property

    Public Property Sum() As Decimal
        Get
            If Session("Sum") Is Nothing Then
                Session("Sum") = 0
            End If
            Return CDec(Session("Sum"))
        End Get
        Set(ByVal value As Decimal)
            Session("Sum") = value
        End Set
    End Property

    Public Property ExpectedCostsSum() As Decimal
        Get
            If Session("ExpectedCostsSum") Is Nothing Then
                Session("ExpectedCostsSum") = 0
            End If
            Return CDec(Session("ExpectedCostsSum"))
        End Get
        Set(ByVal value As Decimal)
            Session("ExpectedCostsSum") = value
        End Set
    End Property

    Public Property Savings() As Decimal
        Get
            If Session("Savings") Is Nothing Then
                Session("Savings") = 0
            End If
            Return CDec(Session("Savings"))
        End Get
        Set(ByVal value As Decimal)
            Session("Savings") = value
        End Set
    End Property

    Public Property MonthBudget() As Decimal
        Get
            If Session("MonthBudget") Is Nothing Then
                Session("MonthBudget") = 0
            End If
            Return CDec(Session("MonthBudget"))
        End Get
        Set(ByVal value As Decimal)
            Session("MonthBudget") = value
        End Set
    End Property

    Public Property MonthBudgetTotal() As Decimal
        Get
            If Session("MonthBudgetTotal") Is Nothing Then
                Session("MonthBudgetTotal") = 0
            End If
            Return CDec(Session("MonthBudgetTotal"))
        End Get
        Set(ByVal value As Decimal)
            Session("MonthBudgetTotal") = value
        End Set
    End Property

    Public Property Month() As Integer
        Get
            If Session("Month") Is Nothing Then
                Session("Month") = DateTime.Now.Month
            End If
            Return CInt(Session("Month"))
        End Get
        Set(ByVal value As Integer)
            Me.ExpenseManager.Month = value
            Session("Month") = value
        End Set
    End Property

    Public Property Year() As Integer
        Get
            If Session("Year") Is Nothing Then
                Session("Year") = DateTime.Now.Year
            End If
            Return CInt(Session("Year"))
        End Get
        Set(ByVal value As Integer)
            Me.ExpenseManager.Year = value
            Session("Year") = value
        End Set
    End Property

    'Public ReadOnly Property CostCategoriesKeywords As String()
    '    Get
    '        If Session("CostCategoriesKeywords") Is Nothing Then
    '            Session("CostCategoriesKeywords") = Category.GetCategoryKeyWords(CurrentLanguage, UserID, GetConnectionString)
    '        End If
    '        Return CType(Session("CostCategoriesKeywords"), String())
    '    End Get

    'End Property

    Public Property ExpenditureID() As Integer
        Get
            If Session("ExpenditureID") Is Nothing Then
                Session("ExpenditureID") = 0
            End If
            Return CInt(Session("ExpenditureID"))
        End Get

        Set(ByVal value As Integer)
            Session("ExpenditureID") = value
        End Set
    End Property

    Public Property Transactions As List(Of Transaction)
        Get
            If Session("Transactions") Is Nothing Then
                Session("Transactions") = New List(Of Transaction)()
            End If
            Return CType(Session("Transactions"), List(Of Transaction))
        End Get
        Set(ByVal value As List(Of Transaction))
            Session("Transactions") = value
        End Set
    End Property

    Public Property PopUpAlertIsShown() As Boolean
        Get
            If Session("popupalert") Is Nothing Then
                Session("popupalert") = False
            End If
            Return CBool(Session("popupalert"))
        End Get
        Set(ByVal value As Boolean)
            Session("popupalert") = value
        End Set
    End Property

    Public Property Currency() As String
        Get
            If Session("currency") Is Nothing Then
                Try

                    Session("currency") = DataBaseConnector.GetSingleValue(Of String)(
"SELECT currency FROM tbUsers WHERE userID = @UserID", Me.GetConnectionString, New SqlParameter("UserID", Me.UserID))

                    Return Session("currency").ToString()

                Catch
                    Session("currency") = "USD"
                    Return Session("currency").ToString()
                End Try
            Else
                Return Session("currency").ToString()
            End If
        End Get
        Set(ByVal value As String)
            Session("currency") = value
        End Set
    End Property

    Public Property DisplayAllFlaggedSums() As Boolean
        Get
            If Session("DisplayAllFlaggedSums") Is Nothing Then
                Session("DisplayAllFlaggedSums") = False
            End If
            Return CBool(Session("DisplayAllFlaggedSums"))
        End Get
        Set(ByVal value As Boolean)
            Session("DisplayAllFlaggedSums") = value
        End Set
    End Property

    Public Property PaidExpensesHidden() As Boolean
        Get
            If Session("HidePaid") Is Nothing Then
                Session("HidePaid") = False
            End If
            Return CBool(Session("HidePaid"))
        End Get
        Set(value As Boolean)
            Session("HidePaid") = value
        End Set
    End Property

    Public Property MainTableDataSource As IEnumerable(Of Expenditure)
        Get
            Return CType(Session("MainTableDataSource"), IEnumerable(Of Expenditure))
        End Get
        Set(value As IEnumerable(Of Expenditure))
            Session("MainTableDataSource") = value
        End Set
    End Property

    Public Property DetailsDataSource As IEnumerable(Of ExpenditureDetail)
        Get
            Return CType(Session("DetailsDataSource"), IEnumerable(Of ExpenditureDetail))
        End Get
        Set(value As IEnumerable(Of ExpenditureDetail))
            Session("DetailsDataSource") = value
        End Set
    End Property

    Public Property IncomeDataSource As IEnumerable(Of Income)
        Get
            Return CType(Session("IncomeDataSource"), IEnumerable(Of Income))
        End Get
        Set(value As IEnumerable(Of Income))
            Session("IncomeDataSource") = value
        End Set
    End Property

    Public ReadOnly Property SearchKeywords As Lazy(Of List(Of String))
        Get
            If Session("SearchKeywords") Is Nothing Then
                Session("SearchKeywords") = Me.ExpenseManager.GetSearchKeywords()
            End If
            Return New Lazy(Of List(Of String))(Function() CType(Session("SearchKeywords"), List(Of String)))
        End Get
    End Property

    Public ReadOnly Property ExchangeRates As String
        Get

            If HttpRuntime.Cache("ExchangeRates") IsNot Nothing Then

                Session("LastExchangeRates") = CStr(HttpRuntime.Cache("ExchangeRates"))

            End If

            Return CStr(Session("LastExchangeRates"))

        End Get
    End Property

#End Region

#Region "[ Constructors ]"

    Public Sub New()

        Me._transactionManager = New TransactionManager()

    End Sub

#End Region

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init

        Me._transactionManager.ConnectionString = Me.GetConnectionString

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    Private Sub InitExpenditureManager()

        If HttpContext.Current.Session IsNot Nothing Then

            Session("ExpenditureManager") = New ExpenditureManager(Me.GetConnectionString, Me.UserID, Me.Month, Me.Year, Me.CurrentLanguage)

        End If

    End Sub

    Private Function LoadIcons() As List(Of String)

        Dim icons As List(Of String) = New List(Of String)

        Try
            Dim imagesPath As String = Server.MapPath("~/Images/UserCategoryIcons/")

            Dim files As List(Of String) = GetFilesRecursive(imagesPath)

            For Each img As String In files
                img = img.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                img = img.Substring(img.IndexOf("Images"))
                icons.Add(String.Format("../{0}", img))
            Next
        Catch ex As Exception
            Logging.Logger.Log(ex, "Environment.vb.LoadIcons", String.Empty, UserID, GetConnectionString)
        End Try

        Return icons
    End Function

    Protected Function GetParentExpendituresForDate(ByVal d As Date) As KeyValuePair(Of Date, Decimal)

        Dim kv As KeyValuePair(Of Date, Decimal)

        Dim recordsForDate As IEnumerable(Of Expenditure) = Me.MainTableDataSource.Where(Function(exp) exp.HasDetails = False AndAlso exp.DateRecordUpdated.Date = d)

        If recordsForDate.Any() Then

            Dim totalSumChanges As Decimal, sumTransactions As Decimal, sumNoTransactions As Decimal

            Dim recordsWithTransactions As IEnumerable(Of Expenditure) = recordsForDate.Where(Function(exp) exp.Transactions.Any())

            If recordsWithTransactions.Any() Then
                Dim recordsTransactionsForDate As IEnumerable(Of Transaction) = recordsWithTransactions.SelectMany(Function(exp) exp.Transactions).Where(Function(t) t.DateModified.Date = d)
                sumTransactions = recordsTransactionsForDate.Sum(Function(t) t.NewValue - t.OldValue)
            End If

            Dim recordsWithoutTransactions As IEnumerable(Of Expenditure) = recordsForDate.Except(recordsWithTransactions)

            If recordsWithoutTransactions.Any() Then
                sumNoTransactions = recordsWithoutTransactions.Sum(Function(exp) exp.FieldValue)
            End If

            totalSumChanges = sumTransactions + sumNoTransactions

            kv = New KeyValuePair(Of Date, Decimal)(d, totalSumChanges)

        End If

        Return kv

    End Function

#Region "[ Public Members ]"

    'Public Function GetSpentPerDay() As IEnumerable(Of KeyValuePair(Of Date, Decimal))

    '    Dim parentDates As IEnumerable(Of KeyValuePair(Of Date, Decimal)) = Me.MainTableDataSource.Where(Function(exp) exp.HasDetails = False).OrderBy(Function(exp) exp.DateRecordUpdated) _
    '            .Select(Function(exp) exp.DateRecordUpdated.Date).Distinct() _
    '            .Select(Function(d) Me.GetParentExpendituresForDate(d))

    '    Dim details As IEnumerable(Of ExpenditureDetail) = Me.MainTableDataSource.SelectMany(Function(exp) exp.Details)

    '    Dim detailDates As IEnumerable(Of KeyValuePair(Of Date, Decimal)) = details _
    '        .Select(Function(det) det.DetailDate.Date).Distinct() _
    '        .Select(Function(d) New KeyValuePair(Of Date, Decimal)(d, details.Where(Function(det) det.DetailDate.Date = d).Sum(Function(det) det.DetailValue)))

    '    Dim dataSource As IEnumerable(Of KeyValuePair(Of Date, Decimal)) = parentDates.Union(detailDates).OrderBy(Function(kv) kv.Key)

    '    Dim duplicates = dataSource.GroupBy(Function(kv) kv.Key).Where(Function(g) g.Skip(1).Any()).SelectMany(Function(g) g).GroupBy(Function(kv) kv.Key)

    '    Dim mergedDuplicates As IEnumerable(Of KeyValuePair(Of Date, Decimal)) = duplicates.Select(Function(group) New KeyValuePair(Of Date, Decimal)(group.Key, group.Sum(Function(g) g.Value)))

    '    Dim mergedDuplicatesDates As Date() = mergedDuplicates.Select(Function(mkv) mkv.Key).ToArray()

    '    Dim dataSourceWithoutDuplicates = dataSource.Where(Function(kv) Not mergedDuplicatesDates.Contains(kv.Key))

    '    Dim result = dataSourceWithoutDuplicates.Union(mergedDuplicates).OrderBy(Function(kv) kv.Key)

    '    Return result

    'End Function

    Public Function BytesToBitmap(ByVal byteArray As Byte()) As Drawing.Bitmap

        Dim img As Drawing.Bitmap

        Using ms As MemoryStream = New MemoryStream(byteArray)

            img = CType(Drawing.Image.FromStream(ms), Drawing.Bitmap)

        End Using

        Return img

    End Function

    Protected Sub RecordTransactions()
        If Me.Transactions.Count() > 0 Then
            Me.TransactManager.RecordTransactions(Me.Transactions)
            Me.Transactions.Clear()
        End If
    End Sub

    Protected Sub AddTransaction(ByVal expenditureID As Integer, ByVal newValue As Decimal, ByVal oldValue As Decimal, ByVal name As String)

        Dim transaction As Transaction = New Transaction()

        transaction.UserID = Me.UserID
        transaction.ExpenseID = expenditureID
        transaction.ConnectionString = Me.GetConnectionString
        transaction.NewValue = newValue
        transaction.OldValue = oldValue
        transaction.DateModified = DateTime.Now
        transaction.TransactionText = String.Format("[{0}] changed from {1} to {2} on {3};", name, oldValue, newValue, DateTime.Now)

        Me.Transactions.Add(transaction)

    End Sub

    Public Shared Sub ClearFieldsContent(ByVal source As Control)

        For Each ctl As Control In source.Controls

            If ctl.HasControls Then
                ClearFieldsContent(ctl)
            End If

            If TypeOf (ctl) Is TextBox Then
                DirectCast(ctl, TextBox).Text = String.Empty
            ElseIf TypeOf (ctl) Is CheckBox Then
                DirectCast(ctl, CheckBox).Checked = False
            End If

        Next

    End Sub

    Public Shared Sub SetDropDownSelectedValue(ByVal dropDownList As DropDownList, ByVal value As Object)

        For Each item As System.Web.UI.WebControls.ListItem In dropDownList.Items
            If item.Value = value Then
                item.Selected = True
                Exit Sub
            End If
        Next

    End Sub

    Public Shared Sub InsertDropDownEmptyItem(ByVal dropDownList As DropDownList, ByVal defaultValue As Object)

        With dropDownList
            .Items.Insert(0, New System.Web.UI.WebControls.ListItem(String.Empty, defaultValue))
        End With

    End Sub

    Public Shared Sub DisplayWebPageMessage(ByVal sender As Object, ByVal message As String)
        ScriptManager.RegisterStartupScript(sender, sender.GetType, sender.GetHashCode().ToString(), String.Format("alert('{0}');", message), True)
    End Sub

    Public Shared Sub ExecuteScript(ByVal sender As Object, ByVal script As String, Optional ByVal addScriptTags As Boolean = True)
        ScriptManager.RegisterStartupScript(sender, sender.GetType, sender.GetHashCode().ToString(), script, addScriptTags)
    End Sub

    '    private static string ToLiteral(string input)
    '{
    '    using (var writer = new StringWriter())
    '    {
    '        using (var provider = CodeDomProvider.CreateProvider("CSharp"))
    '        {
    '            provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
    '            return writer.ToString();
    '        }
    '    }
    '}

    Private Shared Function ToLiteral(ByVal input As String) As String

        Using writer As StringWriter = New StringWriter()

            Using provider As CodeDomProvider = CodeDomProvider.CreateProvider("CSharp")

                provider.GenerateCodeFromExpression(New CodePrimitiveExpression(input), writer, Nothing)

                Return writer.ToString()

            End Using

        End Using

    End Function

    Public Shared Function GetOpenInCustomWindowScript(ByVal url As String, Optional ByVal location As Integer = 0, Optional ByVal status As Integer = 0, Optional ByVal scrollbars As Boolean = 0, Optional ByVal resizable As Boolean = True, Optional ByVal width As Integer = 0, Optional ByVal height As Integer = 0, Optional ByVal title As String = "MyHomeBills") As String

        If String.IsNullOrWhiteSpace(url) Then
            Throw New ArgumentNullException("Environment.GetOpenInCustomWindowScript: Expected parameter url!")
        End If

        Dim script As StringBuilder = New StringBuilder()

        With script

            .AppendFormat("javascript:window.open('{0}',", url)
            .AppendFormat("'{0}',", title)
            .Append("'")
            .AppendFormat("location={0},", location)
            .AppendFormat("status={0},", status)
            .AppendFormat("scrollbars={0},", IIf(scrollbars, 1, 0))
            .AppendFormat("resizable={0},", IIf(resizable, 1, 0))

            If width > 0 Then
                .AppendFormat("width={0},", width)
            End If

            If height > 0 Then
                .AppendFormat("height={0}", height)
            End If

            .Append("'")
            .Append(");")
            '.Append("return false;")

        End With

        Return script.ToString()
    End Function

    Public Shared Function GetFilesRecursive(ByVal initial As String) As List(Of String)
        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(initial)

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string
            Dim dir As String = stack.Pop
            Try
                ' Add all immediate file paths
                result.AddRange(Directory.GetFiles(dir, "*.*"))

                ' Loop through all subdirectories and add them to the stack.
                Dim directoryName As String
                For Each directoryName In Directory.GetDirectories(dir)
                    stack.Push(directoryName)
                Next

            Catch ex As Exception
            End Try
        Loop

        ' Return the list
        Return result
    End Function

    Private selectedControls As Control() = New Control() {}
    Private selectedControlsCount As Integer = 0
    ''' <summary>
    ''' Gets all controls in the specified container control recursively which match the target type;
    ''' </summary>
    ''' <param name="container">Control</param>
    ''' <param name="targetType">Type</param>
    ''' <returns>Control()</returns>
    ''' <remarks></remarks>
    Public Function GetControlsByTypeRecursively(ByVal container As ControlCollection, ByVal targetType As Type) As Control()

        Try

            For Each control As Control In container

                If control.HasControls Then
                    Me.GetControlsByTypeRecursively(control.Controls, targetType)
                End If

                If control.GetType() Is targetType Then
                    ReDim Preserve selectedControls(selectedControlsCount)
                    selectedControls(selectedControlsCount) = control
                    selectedControlsCount = selectedControlsCount + 1
                End If
            Next

        Catch ex As Exception
            Logging.Logger.Log(ex, "Environment.GetControlsByTypeRecursively", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

        Return selectedControls

    End Function

    Public Sub ApplyDropDownSkin(ByVal container As Control)

        Try

            Dim names As String() = Me.GetControlsByTypeRecursively(container.Controls, GetType(DropDownList)).Select(Function(ddl) ddl.ID).ToArray()

            ScriptManager.RegisterStartupScript(container, container.GetType(), "select2style", String.Format("ApplySelect2Style('{0}');", String.Join(",", names)), True)

        Catch ex As Exception
            Logging.Logger.Log(ex, "Environment.ApplyDropDownSkin", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

    End Sub

    Public Sub SetLoadingDivDown()
        Try
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "setLoadingDivUP", "<script language='javascript'>document.getElementById('DivLoading').style.top = (document.body.clientHeight - 450) + 'px'</script>", False)
        Catch ex As Exception
            Logging.Logger.Log(ex, "SetLoadingDivUp", String.Empty, UserID, GetConnectionString)
        End Try
    End Sub

    Public Sub RestoreLoadingDivPosition()
        Try
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "restoreLoadingDivStyle", "<script language='javascript'>document.getElementById('DivLoading').style.top ='40%'</script>", False)
        Catch ex As Exception
            Logging.Logger.Log(ex, "RestoreLoadingDivPosition", String.Empty, UserID, GetConnectionString)
        End Try
    End Sub

    ''' <summary>
    ''' Sets the grid cells style.
    ''' </summary>
    ''' <param name="grid">The GridView</param>
    Public Sub SetGridCellsStyle(ByVal grid As GridView)

        For i As Integer = 0 To grid.Rows.Count - 1
            For j As Integer = 0 To grid.Rows(i).Cells.Count - 1
                For Each ctl As Control In grid.Rows(i).Cells(j).Controls
                    If TypeOf (ctl) Is TextBox Then
                        CType(ctl, TextBox).CssClass = "GridCells"
                        'CType(ctl, TextBox).Attributes.Add("onmouseover", "getElementById('" & CType(ctl, TextBox).ClientID & "').style.backgroundColor = '#FFFFCC'")
                        'CType(ctl, TextBox).Attributes.Add("onmouseout", "getElementById('" & CType(ctl, TextBox).ClientID & "').style.backgroundColor = 'white'")
                        CType(ctl, TextBox).Attributes.Add("onblur", "getElementById('" & CType(ctl, TextBox).ClientID & "').value = getElementById('" & CType(ctl, TextBox).ClientID & "').value.replace(/^\s+|\s+$/g,'')")
                        CType(ctl, TextBox).Width = New Unit("98%")
                    End If
                Next
            Next
        Next

    End Sub

    ''' <summary>
    ''' Gets the name of the user.
    ''' </summary>
    ''' <returns>String</returns>
    Public Function GetUserName() As String
        Try

            If Session("UserName") Is Nothing Then
                Session("UserName") = DataBaseConnector.GetSingleValue(
String.Format("SELECT [email] FROM [dbo].[tbUsers] WHERE userID = {0}", UserID), GetConnectionString)
            End If

            Return Session("UserName")

        Catch ex As Exception
            Throw New Exception("Environment.GetUserName(): " & ex.Message, ex)
        End Try
    End Function

    Private ReadOnly _translatedControlsToExclude As String() = New String() {"LinkButtonMainTableDetails", "lnkEdit", "lnkDelete"}

    Private ReadOnly _controlsTranslated As List(Of String) = New List(Of String)()

    Public Sub TranslateControls(ByVal container As ControlCollection)

        Dim controlName As String = String.Empty

        Try

            If Me.IsInEditMode Then
                Me.SetTranslationInCache()
            End If

            Dim containerControls As IEnumerable(Of Control) = container.Cast(Of Control)()

            ' We scroll through all controls on the page
            For Each ctl As Control In containerControls

                controlName = ctl.ID

                Select Case ctl.GetType.Name

                    Case GetType(GridView).Name
                        Continue For
                        Exit Select

                    Case GetType(RadioButtonList).Name

                        Dim radioButtonList As RadioButtonList = DirectCast(ctl, RadioButtonList)

                        For Each itm As System.Web.UI.WebControls.ListItem In radioButtonList.Items

                            Dim id As String = itm.Attributes("id")

                            Dim controlID As String = IIf(id Is Nothing, itm.Text, id).ToString()

                            Dim translation As Translation = Me.Translations.FirstOrDefault(Function(kv) kv.ControlID = controlID)

                            If translation IsNot Nothing Then

                                translation.SelectedLanguage = Me.CurrentLanguage

                                itm.Text = translation.CurrentTranslation

                            End If

                        Next

                        Continue For

                        Exit Select

                    Case GetType(TabContainer).Name

                        Dim tabContainer As TabContainer = DirectCast(ctl, TabContainer)

                        For Each tabPanel As TabPanel In tabContainer.Controls.OfType(Of TabPanel)

                            Dim translation As Translation = Me.Translations.FirstOrDefault(Function(kv) kv.ControlID = tabPanel.ID)

                            If translation IsNot Nothing Then

                                translation.SelectedLanguage = Me.CurrentLanguage

                                tabPanel.HeaderText = translation.CurrentTranslation

                            End If
                        Next

                        Exit Select
                End Select

                If ctl.HasControls Then
                    Me.TranslateControls(ctl.Controls)
                ElseIf ctl.ID Is Nothing Then
                    Continue For
                End If

                If ctl.ID Is Nothing Then
                    Continue For
                End If

                If ctl.Parent IsNot Nothing AndAlso Not TypeOf ctl.Parent Is Panel _
                    AndAlso Not Me._translatedControlsToExclude.Contains(ctl.ID) Then

                    If Not Me._controlsTranslated.Contains(ctl.ID) Then
                        Me._controlsTranslated.Add(ctl.ID)
                    Else
                        Continue For
                    End If

                End If

                Dim matchingTranslation As Translation = Me.Translations.FirstOrDefault(Function(kv) kv.ControlID = ctl.ID)

                If matchingTranslation IsNot Nothing Then

                    matchingTranslation.SelectedLanguage = Me.CurrentLanguage

                    Dim translatedValue As String = matchingTranslation.CurrentTranslation

                    ' TODO: Optimize checks
                    If Me.IsInEditMode Then

                        If Not TypeOf ctl Is ImageButton AndAlso TypeOf ctl Is WebControl Then

                            If TypeOf ctl Is Button Then
                                DirectCast(ctl, Button).OnClientClick = String.Empty
                                'ElseIf TypeOf ctl Is LinkButton Then
                                '    DirectCast(ctl, LinkButton).OnClientClick = String.Empty
                            End If

                            If Not TypeOf ctl Is CompositeDataBoundControl Then
                                Dim webControl As WebControl = CType(ctl, WebControl)

                                With webControl
                                    .Attributes.Remove("onclick")
                                    .Attributes.Add("onclick", String.Format("javascript:ShowEditControlTranslationPopup('{0}', '{1}'); return false;", ctl.ID, translatedValue))
                                End With
                            End If

                        End If
                    End If

                    Select Case ctl.GetType.Name

                        Case GetType(Button).Name
                            DirectCast(ctl, Button).Text = translatedValue
                            Exit Select
                        Case GetType(CheckBox).Name
                            DirectCast(ctl, CheckBox).Text = translatedValue
                            Exit Select
                        Case GetType(LinkButton).Name
                            DirectCast(ctl, LinkButton).Text = translatedValue
                            Exit Select
                        Case GetType(Label).Name
                            DirectCast(ctl, Label).Text = translatedValue
                            Exit Select
                        Case GetType(ImageButton).Name
                            DirectCast(ctl, ImageButton).ToolTip = translatedValue
                            Exit Select

                    End Select

                Else

                End If

            Next

        Catch ex As Exception

            If ex.Message.Contains("Cache is not available") Then Return

            Logging.Logger.Log(ex, "TranslateControls", String.Empty, Me.UserID, Me.GetConnectionString)

        Finally
            Debug.WriteLine(String.Format("### TranslateControls: {0}", controlName))
        End Try
    End Sub

    Public Sub TranslateGridViewControls(ByVal gridView As GridView)

        For Each col As DataControlField In gridView.Columns

            If String.IsNullOrEmpty(col.HeaderText) Then
                Continue For
            End If

            Dim translation As Translation = Me.Translations.FirstOrDefault(Function(kv) kv.ControlID = col.HeaderText)

            If translation IsNot Nothing Then

                translation.SelectedLanguage = Me.CurrentLanguage

                If String.IsNullOrEmpty(col.SortExpression) Then
                    col.SortExpression = col.HeaderText
                End If

                col.HeaderText = translation.CurrentTranslation

            End If

        Next

        gridView.DataBind()

        For Each row As GridViewRow In gridView.Rows
            Me.TranslateRowControls(row.Controls)
        Next

    End Sub

    Private Sub TranslateRowControls(rowControls As ControlCollection)

        For Each ctrl As Control In rowControls

            If ctrl.HasControls Then
                TranslateRowControls(ctrl.Controls)
            End If

            If ctrl.ID Is Nothing Then
                Continue For
            End If

            Dim translation As Translation = Me.Translations.FirstOrDefault(Function(kv) kv.ControlID = ctrl.ID)

            If translation IsNot Nothing Then

                translation.SelectedLanguage = Me.CurrentLanguage

                If TypeOf ctrl Is ImageButton Then

                    DirectCast(ctrl, ImageButton).ToolTip = translation.CurrentTranslation

                    Continue For

                ElseIf TypeOf ctrl Is LinkButton Then

                    DirectCast(ctrl, LinkButton).Text = translation.CurrentTranslation

                    Continue For

                End If

            End If
        Next

    End Sub

    Public Sub PutGridViewHeaderInEditMode(ByVal gridView As GridView)

        If gridView.HeaderRow IsNot Nothing Then

            For Each cell As TableCell In gridView.HeaderRow.Cells
                If Not String.IsNullOrWhiteSpace(Server.HtmlDecode(cell.Text)) Then

                    Dim parentColumn As DataControlField = (From col As DataControlField
                                                             In gridView.Columns
                                                            Where col.HeaderText = cell.Text
                                                            Select col).FirstOrDefault()

                    Dim controlID As String = String.Empty

                    If parentColumn IsNot Nothing Then
                        controlID = parentColumn.SortExpression
                    Else
                        controlID = cell.Text
                    End If

                    cell.Attributes.Add("onclick", String.Format("javascript:ShowEditControlTranslationPopup('{0}', '{1}'); return false;", controlID, cell.Text))
                End If
            Next

        End If

    End Sub

    'Public Function GetCategoryID(ByVal input As String) As Integer

    '    Try

    '        input = input.Replace(",", " ").Replace(".", " ")
    '        Dim keyWords As String() = input.ToUpper.Split(" ")
    '        Dim filter As String = String.Empty
    '        Dim costCategoryID As Integer = 0

    '        For Each key As String In keyWords
    '            If key.Trim.Length > 0 Then
    '                filter = key.ToUpper & "*"
    '                Dim matchingCategory As IEnumerable(Of DataRow) =
    '                    From cat In CostCategoriesKeywords Where cat("CostNames") Like filter Select cat

    '                If matchingCategory.Count > 0 AndAlso IsNumeric(matchingCategory(0)("CostCategoryID")) Then
    '                    costCategoryID = matchingCategory(0)("CostCategoryID")
    '                    Exit For
    '                End If
    '            End If
    '        Next

    '        Return costCategoryID

    '    Catch ex As Exception
    '        Throw New Exception(String.Format("GetCategoryID:{0}", ex.Message), ex)
    '    End Try

    'End Function

    Private Function GetWildCardSearchString(ByVal keyword As String) As String

        If keyword.Length > 4 Then

            Dim categoryKeywordCharArray As Char() = keyword.ToCharArray()

            categoryKeywordCharArray(0) = "*"c
            categoryKeywordCharArray(categoryKeywordCharArray.Length - 1) = "*"c

            Return New String(categoryKeywordCharArray)

        End If

        Return keyword

    End Function

    Public Function GetCategoryID(ByVal input As String, Optional ByVal userCategories As IEnumerable(Of Category) = Nothing) As Integer

        Dim secondMatchingCategory As Integer = Category.CATEGORY_DEFAULT_ID

        Try

            Dim categories As IEnumerable(Of Category) = Enumerable.Empty(Of Category)

            If userCategories IsNot Nothing Then
                categories = userCategories
            Else
                categories = Me.Categories
            End If

            input = input.Replace(",", vbSpace).Replace(".", vbSpace).Trim().ToUpper()

            ' Start with entire entered text
            Dim enteredKeywords As IEnumerable(Of String) = New String() {input}

            ' Split input further to check each word
            enteredKeywords = enteredKeywords.Union(input.ToUpper.Split(vbSpace))

            For Each enteredKeyword As String In enteredKeywords

                For Each category As Category In categories

                    For Each categoryKeyword As String In category.Keywords

                        If enteredKeyword = categoryKeyword Then
                            Return category.ID ' If we find the best match right away - we return
                        End If

                        ' we keep on searching for the second best match
                        If enteredKeyword.Contains(categoryKeyword) OrElse LikeOperator.LikeString(enteredKeyword, Me.GetWildCardSearchString(categoryKeyword), CompareMethod.Text) Then
                            secondMatchingCategory = category.ID
                            Exit For
                        End If

                    Next
                Next

            Next

        Catch ex As Exception
            Throw New Exception(String.Format("GetCategoryID:{0}", ex.Message), ex)
        End Try

        Return secondMatchingCategory

    End Function

    ''' <summary>
    ''' Returns the translation for a corresponding key in the DataBase
    ''' </summary>
    ''' <param name="key">Example: 'TextBoxValue.ToolTip'</param>
    ''' <param name="lang">'en' for English, 'de' for German/Deutsch, 'bg' for Bulgarian...</param>
    ''' <returns>Translated value for the given key.</returns>
    ''' <remarks></remarks>
    Public Function GetTranslatedValue(ByVal key As String, ByVal lang As Language) As String
        Try

            Dim matchingTranslation As Translation = Me.Translations.FirstOrDefault(Function(t) t.ControlID = key)

            If matchingTranslation IsNot Nothing Then
                matchingTranslation.SelectedLanguage = Me.CurrentLanguage
                Return matchingTranslation.CurrentTranslation
            Else
                Return key
            End If

        Catch ex As Exception
            If ex.Message.Contains("Cache is not available") Then Return key

            Throw New Exception(String.Format("GetTranslatedValue():{0}", ex.Message), ex)
        End Try
    End Function

    '    ''' <summary>
    '    ''' Logs the specified Exception.
    '    ''' </summary>
    '    ''' <param name="ex">The Exception object.</param>
    '    ''' <param name="method">The method.</param>
    '    ''' <returns>Boolean</returns>
    '    Public Function Logging.Logger.Log(ByVal ex As Exception, ByVal method As String, ByVal qry As String) As Boolean
    '        Try

    '            Dim InnerException As String = ""
    '            If ex.InnerException IsNot Nothing Then
    '                If ex.InnerException.Message.Length > 500 Then
    '                    InnerException = ex.InnerException.Message.Substring(0, 499).Replace("'", String.Empty)
    '                Else
    '                    InnerException = ex.InnerException.Message.Replace("'", String.Empty)
    '                End If
    '            Else
    '                InnerException = "No InnerException information provided"
    '            End If

    '            Dim StackTrace As String = ""
    '            If ex.StackTrace IsNot Nothing Then
    '                If ex.StackTrace.Length > 500 Then
    '                    StackTrace = ex.StackTrace.Substring(0, 499).Replace("'", String.Empty)
    '                Else
    '                    StackTrace = ex.StackTrace.Replace("'", String.Empty)
    '                End If
    '            Else
    '                StackTrace = "No StackTrace information provided"
    '            End If

    '            Dim Source As String = ""
    '            If ex.Source IsNot Nothing Then
    '                If ex.Source.Length > 500 Then
    '                    Source = ex.Source.Substring(0, 499).Replace("'", String.Empty)
    '                Else
    '                    Source = ex.Source.Replace("'", String.Empty)
    '                End If

    '            Else
    '                Source = "No Source information provided"
    '            End If

    '            Dim qryToLog As String = ""
    '            If qry.Length > 500 Then
    '                qry = qry.Replace("'", "''")
    '                qryToLog = qry.Substring(0, 499)
    '            Else
    '                qryToLog = qry.Replace("'", "''")
    '            End If

    '            Dim msgToLog As String = ""
    '            If ex.Message.Length > 500 Then
    '                msgToLog = ex.Message.Replace("'", String.Empty).Substring(0, 499)
    '            Else
    '                msgToLog = ex.Message.Replace("'", String.Empty)
    '            End If

    '            Dim sql As String = _
    '"INSERT INTO [dbo].[tbLog] VALUES ('" & method & "','" & msgToLog & "','" & InnerException & "','" & Source & "','" & StackTrace & "'," & UserID.ToString() & ",'" & DateTime.Now.ToString() & "', '" & qryToLog & "')"

    '            DatabaseConnector.ExecuteQuery(sql, GetConnectionString)

    '            Return True
    '        Catch
    '            Return False
    '        End Try
    '    End Function

    Public Function FilterNumericString(ByVal value As String) As Decimal

        Dim result As Decimal = 0D

        Try
            Dim resultString As String = String.Empty
            resultString = Regex.Replace(value, "[A-Za-z;'%^!@#$%&*()_+-]", String.Empty)
            resultString = resultString.Replace(",", ".")

            If IsNumeric(resultString) Then
                result = CDec(resultString)
            End If

        Catch ex As Exception
            Throw New Exception(String.Format("Environment.FilterNumericString", ex.Message), ex)
        End Try

        Return result

    End Function

    Public Function GetCellIndexByName(ByVal row As GridViewRow, ByVal columnName As String) As Integer

        Dim translatedColumnName As String = Me.GetTranslatedValue(columnName, Me.CurrentLanguage)

        Dim columnIndex As Integer = -1

        For index = 0 To row.Cells.Count Step 1

            Dim cell As DataControlFieldCell = CType(row.Cells(index), DataControlFieldCell)

            If TypeOf cell.ContainingField Is BoundField Then

                Dim boundField As BoundField = CType(cell.ContainingField, BoundField)

                If boundField.DataField.Equals(columnName) OrElse boundField.DataField.Equals(translatedColumnName) Then
                    columnIndex = index
                    Exit For
                End If

            ElseIf TypeOf cell.ContainingField Is TemplateField Then

                Dim templateField As TemplateField = CType(cell.ContainingField, TemplateField)

                If templateField.HeaderText.Equals(columnName) OrElse templateField.HeaderText.Equals(translatedColumnName) Then
                    columnIndex = index
                    Exit For
                End If

            End If
        Next

        Return columnIndex

    End Function

    Public Function GetColumnByHeaderText(ByVal source As GridView, ByVal headerText As String) As DataControlField

        Return (From column As DataControlField
                                         In source.Columns
                Where Not String.IsNullOrWhiteSpace(column.HeaderText) AndAlso column.HeaderText = headerText
                Select column).FirstOrDefault()
    End Function

    Public Sub AddNavigationLink(ByVal sender As Object, ByVal navigateUrl As String, ByVal linkText As String)

        Try
            Dim separator As Literal = New Literal()
            separator.Text = " » "

            Dim navLink As LinkButton = New LinkButton()
            navLink.CssClass = "NavigationBar"
            navLink.OnClientClick = String.Format("<%= Response.Redirect(""{0}"")%>", navigateUrl)
            navLink.Text = linkText

            CType(sender.Master.FindControl("PanelNavigation"), Panel).Controls.Add(separator)
            CType(sender.Master.FindControl("PanelNavigation"), Panel).Controls.Add(navLink)

        Catch ex As Exception
            Logging.Logger.Log(ex, "AddNavigationLink", String.Empty, UserID, GetConnectionString)
        End Try

    End Sub

    Public Sub CheckAccess()
        If Me.UserID = 0 Then
            Response.Redirect(MHB.BL.URLRewriter.GetLink("Login"))
        End If
    End Sub

    Public Shared Function IsAssemblyDebugBuild(ByVal assembly As System.Reflection.Assembly) As Boolean

        Return assembly.GetCustomAttributes(False).OfType(Of DebuggableAttribute).Select(Function(att) att.IsJITTrackingEnabled).Any()

    End Function

    Public Sub AddToShoppingList(ByVal quantity As Decimal, ByVal productID As Integer, Optional ByVal value As Decimal = -1)

        Dim selectedProduct As Product = Me.Products.FirstOrDefault(Function(p) p.ID = productID)

        With selectedProduct

            Dim sum As Decimal = 0.0

            If value <> -1 Then
                sum = value
            Else
                sum = .ListPrice * quantity
            End If

            Me.ShoppingList.Add(New Tuple(Of String, Product, Decimal)(String.Format("{0} (x{1}) - {2:0.00}{3}", .Name, quantity, sum, Me.Currency), selectedProduct, sum))

        End With

    End Sub

    Public Function GenerateShoppingListFile(ByVal printList As Boolean, Optional ByVal callback As Action = Nothing) As String

        Dim fullPath As String = String.Empty

        Dim fileName As String = String.Empty

        If (Me.ShoppingList.Count > 0) Then

            Dim doc As Document = New Document()

            fileName = String.Format("Shopping_List_{0}_{1}.pdf", DateTime.Now.ToISODateFileName(), Me.UserID)

            fullPath = Path.Combine(Me.ShoppingListsRootPath, Me.UserID)

            If Not System.IO.Directory.Exists(fullPath) Then
                System.IO.Directory.CreateDirectory(fullPath)
            End If

            Dim fileNamePath As String = Path.Combine(fullPath, fileName)

            PdfWriter.GetInstance(doc, New FileStream(fileNamePath, FileMode.Create))

            doc.Open()

            Dim base As BaseFont = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\COMIC.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED)

            Dim normalFont As Font = New Font(base, 12, Font.NORMAL, BaseColor.BLACK)
            Dim boldFont As Font = New Font(base, 14, Font.BOLD, BaseColor.BLACK)

            doc.Add(New Paragraph(String.Format("{0} - {1}", Me.GetTranslatedValue("ShoppingList", Me.CurrentLanguage), DateTime.Now.ToISODateFileName()), boldFont))

            doc.Add(New Paragraph("----------------------------------------"))

            Me.ShoppingList.ForEach(Function(p) doc.Add(New Paragraph(p.Item1, normalFont)))

            doc.Add(New Paragraph("----------------------------------------"))

            Dim sum As Decimal = Me.ShoppingList.Sum(Function(p) p.Item3)

            doc.Add(New Paragraph(String.Format("{0} {1:0.00}{2}", Me.GetTranslatedValue("LabelExpectedCost", Me.CurrentLanguage), sum, Me.Currency), normalFont))

            doc.Close()

            Me.ShoppingList.Clear()

            If callback IsNot Nothing Then
                callback()
            End If

            If printList Then
                Me.SaveFileAs(fileNamePath, fileName, HttpHeadersContentType.PDF)
            End If

        Else
            Environment.DisplayWebPageMessage(Me, Me.GetTranslatedValue("NotifyAddProductsToShoppingList", Me.CurrentLanguage))
        End If

        Return Path.Combine(fullPath, fileName)

    End Function

    Public Sub SaveFileAs(ByVal fullFilePath As String, ByVal fileName As String, ByVal contentType As String)

        With HttpContext.Current.Response
            .ClearHeaders()
            .ClearContent()
            .AddHeader("Content-Disposition", String.Format("attachment; filename={0}", fileName))
            .ContentType = contentType
            .AddHeader("Content-Length", New FileInfo(fullFilePath).Length.ToString())
            .TransmitFile(fullFilePath)
            .Flush()
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        End With

    End Sub

    Public Class HttpHeadersContentType

        Public Const OCTET As String = "application/octet-stream"
        Public Const PDF As String = "application/pdf"
        Public Const GIF As String = "image/gif"
        Public Const JPEG As String = "image/jpeg"
        Public Const PJPEG As String = "image/pjpeg"
        Public Const PNG As String = "image/png"
        Public Const SVG As String = "image/svg+xml"
        Public Const TIFF As String = "image/tiff"

    End Class

    Public Class GlobalVariableNames

        Public Const FILE_NAME As String = "fileName"
        Public Const CONTENT_TYPE As String = "contentType"

    End Class

    Public Sub SetTranslationInCache(Optional ByVal ipAddress As String = "")

        Dim connectionString As String = String.Empty

        Try
            If HttpRuntime.Cache("translations") Is Nothing Then

                connectionString = ConfigurationManager.ConnectionStrings("connectionString").ConnectionString

                Dim translations As IEnumerable(Of Translation) = Translation.GetTranslations(connectionString)

                HttpRuntime.Cache.Insert("translations", translations, Nothing, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration)

                Dim userID As Integer = 0

                If HttpContext.Current IsNot Nothing Then
                    userID = Me.UserID
                End If

                Logging.Logger.LogAction(Logging.Logger.HistoryAction.SetTranslationInCache, userID, connectionString, ipAddress)

            End If
        Catch ex As Exception
            Logging.Logger.Log(ex, "Global.asax.SetTranslationInCache", String.Empty, 0, connectionString)
        End Try
    End Sub

    Public Function IsLatinLetter(ByVal c As Char) As Boolean

        Return (c >= "a" AndAlso c <= "z") OrElse (c >= "A" AndAlso c <= "Z")

    End Function

    Public Function TransliterateText(ByVal inputText As String) As Tuple(Of String, String)

        Dim originalPrefixTextArray As Char() = inputText.ToLower().ToCharArray()

        Dim transiliteratedPrefixTextArray As Char() = originalPrefixTextArray.Clone()

        If Me.CurrentLanguage = MHB.BL.Enums.Language.Bulgarian Then

            For i As Integer = 0 To originalPrefixTextArray.Length - 1

                Dim ch As Char = originalPrefixTextArray(i)

                If Me.IsLatinLetter(ch) Then

                    Dim index As Integer = Array.IndexOf(Me.BULGARIAN_ALPHABET_TRANSLITERATED_LOWERCASE, ch)

                    transiliteratedPrefixTextArray(i) = Me.BULGARIAN_ALPHABET_LOWERCASE(index)

                End If

            Next

        End If

        Dim originalPrefixText As String = String.Join(String.Empty, originalPrefixTextArray)

        Dim transliteratedPrefixText As String = String.Join(String.Empty, transiliteratedPrefixTextArray)

        Return New Tuple(Of String, String)(originalPrefixText, transliteratedPrefixText)

    End Function

#End Region

    Protected Friend Sub ValidateLicense()

        Const licenseFile As String = "~/License/License.lic"

        Dim denyAccess = Sub(ByVal message As String)

                             Logging.Logger.Log(New Exception(message), "Environment.ValidateLicense", String.Empty, Me.UserID, Me.GetConnectionString)

                             Session.Abandon()
                             Session.Clear()

                             System.Environment.Exit(-1)

                         End Sub

        Try

            Dim licenseManager As MHB.Licensing.LicenseManager = New MHB.Licensing.LicenseManager(Server.MapPath(licenseFile))

            Dim output As String = String.Empty

            If Not licenseManager.ValidateLicense(output) Then
                denyAccess(output)
            Else
                Logging.Logger.LogAction(Logger.HistoryAction.ValidateLicenseSuccessful, Me.UserID, Me.GetConnectionString, String.Empty)
            End If

        Catch ex As Exception
            denyAccess(ex.Message)
        Finally
            Logging.Logger.LogAction(Logger.HistoryAction.EndValidateLicense, Me.UserID, Me.GetConnectionString, String.Empty)
        End Try

    End Sub

End Class