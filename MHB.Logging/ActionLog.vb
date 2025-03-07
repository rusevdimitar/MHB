Imports System.Data.SqlClient
Imports System.Xml.Serialization
Imports System.Runtime.Serialization

Public Class ActionLog

#Region "[ Public Constants ]"

    Public Const CITY_SOFIA As String = "Sofia"
    Public Const CITY_PLOVDIV As String = "Plovdiv"
    Public Const CITY_VARNA As String = "Varna"
    Public Const CITY_VIDIN As String = "Vidin"
    Public Const CITY_MONTANA As String = "Montana"
    Public Const CITY_LOM As String = "Lom"
    Public Const CITY_RUSE As String = "Rousse"
    Public Const CITY_SVISHTOV As String = "Svishtov"
    Public Const CITY_SILISTRA As String = "Silistra"
    Public Const CITY_SLIVEN As String = "Sliven"
    Public Const CITY_DOBRICH As String = "Dobrich"
    Public Const CITY_SHUMEN As String = "Shumen"
    Public Const CITY_RAZGRAD As String = "Razgrad"
    Public Const CITY_PLEVEN As String = "Pleven"
    Public Const CITY_LOVECH As String = "Lovech"
    Public Const CITY_VRATSA As String = "Vratsa"
    Public Const CITY_TURNOVO As String = "Turnovo"
    Public Const CITY_BURGAS As String = "Burgas"
    Public Const CITY_STARA_ZAGORA As String = "Stara Zagora"
    Public Const CITY_YAMBOL As String = "Yambol"
    Public Const CITY_HASKOVO As String = "Haskovo"
    Public Const CITY_KARDJALI As String = "Kardjali"
    Public Const CITY_ASENOVGRAD As String = "Asenovgrad"
    Public Const CITY_SMOLYAN As String = "Smolyan"
    Public Const CITY_PERNIK As String = "Pernik"
    Public Const CITY_BLAGOEVGRAD As String = "Blagoevgrad"
    Public Const CITY_KYUSTENDIL As String = "Kyustendil"
    Public Const CITY_PAZARDJIK As String = "Pazardzik"
    Public Const CITY_DIMITROVGRAD As String = "Dimitrovgrad"
    Public Const VILLAGE_TSARATSOVO As String = "Tsaratsovo"
    Public Const CITY_GABROVO As String = "Gabrovo"

#End Region

#Region "[ Properties ]"

    Private _connectionString As String = String.Empty
    Public Property ConnectionString As String
        Get
            Return Me._connectionString
        End Get
        Set(value As String)
            Me._connectionString = value
        End Set
    End Property

    Private _connection As SqlConnection = Nothing

    <IgnoreDataMemberAttribute()>
    Public Property Connection As SqlConnection
        Get
            Return Me._connection
        End Get
        Set(value As SqlConnection)
            Me._connection = value
        End Set
    End Property

    Private _id As Integer = 0
    Public Property ID As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    Private _transactionMessage As String = String.Empty
    Public Property TransactionMessage As String
        Get
            Return _transactionMessage
        End Get
        Set(value As String)
            _transactionMessage = value
        End Set
    End Property

    Private _action As Logger.HistoryAction
    Public Property Action As Logger.HistoryAction
        Get
            Return _action
        End Get
        Set(value As Logger.HistoryAction)
            _action = value
        End Set
    End Property

    Private _userID As Integer = 0
    Public Property UserID As Integer
        Get
            Return _userID
        End Get
        Set(value As Integer)
            _userID = value
        End Set
    End Property

    Private _userEmail As String = String.Empty
    Public Property UserEmail As String
        Get
            Return _userEmail
        End Get
        Set(value As String)
            _userEmail = value
        End Set
    End Property

    Private _userPassword As String = String.Empty
    Public Property UserPassword As String
        Get
            Return _userPassword
        End Get
        Set(value As String)
            _userPassword = value
        End Set
    End Property

    Private _logDate As DateTime
    Public Property LogDate As DateTime
        Get
            Return _logDate
        End Get
        Set(value As DateTime)
            _logDate = value
        End Set
    End Property

    Private _message As String = String.Empty
    Public Property Message As String
        Get
            Return _message
        End Get
        Set(value As String)
            _message = value
        End Set
    End Property

    Private _ip As String = String.Empty
    Public Property IP As String
        Get
            Return _ip
        End Get
        Set(value As String)
            _ip = value
        End Set
    End Property

    Private _countryCode As String = String.Empty
    Public Property CountryCode As String
        Get
            Return _countryCode
        End Get
        Set(value As String)
            _countryCode = value
        End Set
    End Property

    Private _city As String = String.Empty
    Public Property City As String
        Get
            Return _city
        End Get
        Set(value As String)
            _city = value
        End Set
    End Property

    Private _region As String = String.Empty
    Public Property Region As String
        Get
            Return _region
        End Get
        Set(value As String)
            _region = value
        End Set
    End Property

    Public Property Color() As String
        Get
            Select Case Me._action

                Case Logger.HistoryAction.Delete
                    Return "#FF6666"
                Case Logger.HistoryAction.AddCategory
                    Return "#FFCC99"
                Case Logger.HistoryAction.AddNew
                    Return "#0080FF"
                Case Logger.HistoryAction.AddIncome
                    Return "#FBCF37"
                Case Logger.HistoryAction.EditIncome
                    Return "#FFE27F"
                Case Logger.HistoryAction.DeleteIncome
                    Return "#FFC600"
                Case Logger.HistoryAction.DeleteCategory
                    Return "#FF9999"
                Case Logger.HistoryAction.Events
                    Return "#E5CCFF"
                Case Logger.HistoryAction.Login
                    Return "#80FF00"
                Case Logger.HistoryAction.LogOut
                    Return "#CC0000"
                Case Logger.HistoryAction.AddDetails
                    Return "#3399FF"
                Case Logger.HistoryAction.AttachDetails
                    Return "#88BAFE"
                Case Logger.HistoryAction.AttachToDetails
                    Return "#67A7FF"
                Case Logger.HistoryAction.RebuildDbIndexes
                    Return "#4C9900"
                Case Logger.HistoryAction.LoadTranslationsInCache
                    Return "#FFFFCC"
                Case Logger.HistoryAction.UpdateDetails
                    Return "#F0FFF0"
                Case Logger.HistoryAction.Update
                    Return "#C1FFC1"
                Case Logger.HistoryAction.StartDemo
                    Return "#FFB5C5"
                Case Logger.HistoryAction.ScreenShots
                    Return "#FFBBFF"
                Case Logger.HistoryAction.CurrencyExchangeRatesCharts
                    Return "#67FFDD"
                Case Logger.HistoryAction.GetExchangeRatesHistory
                    Return "#D4FDF4"
                Case Logger.HistoryAction.GetExchangeRates
                    Return "#BDFBED"
                Case Logger.HistoryAction.DeleteDemoEntries
                    Return "#FDD4DD"
                Case Logger.HistoryAction.DownloadMyHomeBillsInstaller
                    Return "#d8ff00"
                Case Logger.HistoryAction.ValidateLicenseSuccessful
                    Return "#bdf6ff"
                Case Logger.HistoryAction.API_DeleteParentExpenditure _
                , Logger.HistoryAction.API_DeleteChildExpenditures _
                , Logger.HistoryAction.API_AddParentExpenditure _
                , Logger.HistoryAction.API_SearchUserExpenditures _
                , Logger.HistoryAction.API_GetExpenditureDetails _
                , Logger.HistoryAction.API_UpdateParentExpenses _
                , Logger.HistoryAction.API_GetUsersAverageSumForCategory _
                , Logger.HistoryAction.API_CopyParentExpense _
                , Logger.HistoryAction.API_DeleteAttachment _
                , Logger.HistoryAction.API_GetMaximumExpenditureForCategory _
                , Logger.HistoryAction.API_GetUserIncome _
                , Logger.HistoryAction.API_DuplicateExpenditures _
                , Logger.HistoryAction.API_GetYearlyExpensesProMonth _
                , Logger.HistoryAction.API_GetYearlyBudgetsProMonth _
                , Logger.HistoryAction.API_GetYearlySavingsProMonth _
                , Logger.HistoryAction.API_GetUserExpenditures _
                , Logger.HistoryAction.API_GetExpenditures _
                , Logger.HistoryAction.API_GetActionLogs _
                , Logger.HistoryAction.API_GetExceptionLogs _
                , Logger.HistoryAction.API_BlockUser _
                , Logger.HistoryAction.API_BanIP _
                , Logger.HistoryAction.API_GetSingleValue _
                , Logger.HistoryAction.API_ExecuteQuery _
                , Logger.HistoryAction.API_GetDataReader _
                , Logger.HistoryAction.API_GetDataTable
                    Return "#FFD83C"
                Case Logger.HistoryAction.SessionStart
                    Return "#A9BCF5"
                Case Logger.HistoryAction.BlackListCheckCheck_BANNED
                    Return "#ff0000"
                Case Else
                    Return "#FFFFFF"

            End Select
        End Get
        Set(value As String)
            Throw New Exception("Color Property is read-only!")
        End Set
    End Property

    Public Property RelativeCoordinates As Tuple(Of Double, Double)
        Get
            Select Case Me._city

                Case CITY_SOFIA
                    Return New Tuple(Of Double, Double)(70, 170)
                Case CITY_PLOVDIV
                    Return New Tuple(Of Double, Double)(150, 210)
                Case CITY_VARNA
                    Return New Tuple(Of Double, Double)(340, 130)
                Case CITY_VIDIN
                    Return New Tuple(Of Double, Double)(42, 82)
                Case CITY_MONTANA
                    Return New Tuple(Of Double, Double)(72, 118)
                Case CITY_LOM
                    Return New Tuple(Of Double, Double)(79, 95)
                Case CITY_RUSE
                    Return New Tuple(Of Double, Double)(229, 91)
                Case CITY_SVISHTOV
                    Return New Tuple(Of Double, Double)(195, 108)
                Case CITY_SILISTRA
                    Return New Tuple(Of Double, Double)(307, 71)
                Case CITY_SLIVEN
                    Return New Tuple(Of Double, Double)(250, 180)
                Case CITY_DOBRICH
                    Return New Tuple(Of Double, Double)(328, 113)
                Case CITY_SHUMEN
                    Return New Tuple(Of Double, Double)(285, 134)
                Case CITY_RAZGRAD
                    Return New Tuple(Of Double, Double)(282, 113)
                Case CITY_PLEVEN
                    Return New Tuple(Of Double, Double)(150, 126)
                Case CITY_LOVECH
                    Return New Tuple(Of Double, Double)(157, 142)
                Case CITY_VRATSA
                    Return New Tuple(Of Double, Double)(98, 139)
                Case CITY_TURNOVO
                    Return New Tuple(Of Double, Double)(214, 147)
                Case CITY_BURGAS
                    Return New Tuple(Of Double, Double)(315, 193)
                Case CITY_STARA_ZAGORA
                    Return New Tuple(Of Double, Double)(200, 200)
                Case CITY_YAMBOL
                    Return New Tuple(Of Double, Double)(267, 194)
                Case CITY_HASKOVO
                    Return New Tuple(Of Double, Double)(200, 230)
                Case CITY_KARDJALI
                    Return New Tuple(Of Double, Double)(190, 260)
                Case CITY_ASENOVGRAD
                    Return New Tuple(Of Double, Double)(171, 229)
                Case CITY_SMOLYAN
                    Return New Tuple(Of Double, Double)(-20, -20)
                Case CITY_PERNIK
                    Return New Tuple(Of Double, Double)(-20, -20)
                Case CITY_BLAGOEVGRAD
                    Return New Tuple(Of Double, Double)(-20, -20)
                Case CITY_KYUSTENDIL
                    Return New Tuple(Of Double, Double)(-20, -20)
                Case CITY_PAZARDJIK
                    Return New Tuple(Of Double, Double)(-20, -20)
                Case CITY_DIMITROVGRAD
                    Return New Tuple(Of Double, Double)(205, 215)
                Case CITY_GABROVO
                    Return New Tuple(Of Double, Double)(199, 155)
                Case VILLAGE_TSARATSOVO
                    Return New Tuple(Of Double, Double)(140, 200)

                Case Else
                    Return New Tuple(Of Double, Double)(-20, -20)

            End Select
        End Get
        Set(value As Tuple(Of Double, Double))
            Throw New NotImplementedException("RelativeCoordinates Property is read-only! [ WCF Service hack :( ]")
        End Set
    End Property

#End Region

#Region "[ Constructors ]"

    Public Sub New()

    End Sub

    Public Sub New(ByVal connectionString As String)
        Me._connectionString = connectionString
    End Sub

    Public Sub New(ByVal connection As SqlConnection)
        Me._connection = connection
    End Sub

#End Region

#Region "[ Public Members ]"

    Public Function LoadAll(ByVal startDate As Date) As List(Of ActionLog)

        Dim actionLogs As List(Of ActionLog) = New List(Of ActionLog)()

        If Me._connection Is Nothing AndAlso String.IsNullOrEmpty(Me._connectionString) Then
            Throw New ArgumentNullException("MHB.Logging.ActionLog.LoadAll: connectionString parameter is null or empty! Either provide a valid connection string or pass an existing SqlConnection!")
        End If

        If Me._connection Is Nothing Then
            Me._connection = New SqlConnection(Me._connectionString)
        End If

        Dim reader As IDataReader = SQLHelper.LoadActionLogs(Me._connection, startDate)

        While reader.Read()

            Dim log As ActionLog = New ActionLog()

            If Not reader.IsDBNull(reader.GetOrdinal("ID")) Then
                log.ID = DirectCast(reader("ID"), Integer)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logAction")) Then
                log.Action = DirectCast(reader("logAction"), Logger.HistoryAction)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("TransactionText")) Then
                log.TransactionMessage = DirectCast(reader("TransactionText"), String)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logUserID")) Then
                log.UserID = DirectCast(reader("logUserID"), Integer)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("UserEmail")) Then
                log.UserEmail = DirectCast(reader("UserEmail"), String)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("UserPassword")) Then
                log.UserPassword = DirectCast(reader("UserPassword"), String)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logDate")) AndAlso IsDate(reader("logDate")) Then
                log.LogDate = DirectCast(reader("logDate"), DateTime)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logMessage")) Then
                log.Message = DirectCast(reader("logMessage"), String)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logIP")) Then
                log.IP = DirectCast(reader("logIP"), String)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("CountryCode")) Then
                log.CountryCode = DirectCast(reader("CountryCode"), String)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("City")) Then
                log.City = DirectCast(reader("City"), String)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("RegionName")) Then
                log.Region = DirectCast(reader("RegionName"), String)
            End If

            actionLogs.Add(log)

        End While

        If Not reader.IsClosed Then
            reader.Close()
        End If

        Return actionLogs

    End Function

#End Region

End Class