Imports System.Data.SqlClient
Imports System.Xml.Serialization
Imports System.Runtime.Serialization

Public Class ExceptionLog


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
            Return Me._id
        End Get
        Set(value As Integer)
            Me._id = value
        End Set
    End Property

    Private _methodName As String = String.Empty
    Public Property MethodName As String
        Get
            Return Me._methodName
        End Get
        Set(value As String)
            Me._methodName = value
        End Set
    End Property

    Private _userID As Integer = 0
    Public Property UserID As Integer
        Get
            Return Me._userID
        End Get
        Set(value As Integer)
            Me._userID = value
        End Set
    End Property

    Private _qry As String = String.Empty
    Public Property SqlQuery As String
        Get
            Return Me._qry
        End Get
        Set(value As String)
            Me._qry = value
        End Set
    End Property

    Private _message As String = String.Empty
    Public Property LogMessage As String
        Get
            Return Me._message
        End Get
        Set(value As String)
            Me._message = value
        End Set
    End Property

    Private _innerExceptionMessage As String = String.Empty
    Public Property LogInnerExceptionMessage As String
        Get
            Return Me._innerExceptionMessage
        End Get
        Set(value As String)
            Me._innerExceptionMessage = value
        End Set
    End Property

    Private _logSource As String = String.Empty
    Public Property LogSource As String
        Get
            Return Me._logSource
        End Get
        Set(value As String)
            Me._logSource = value
        End Set
    End Property

    Private _logStackTrace As String = String.Empty
    Public Property LogStackTrace As String
        Get
            Return Me._logStackTrace
        End Get
        Set(value As String)
            Me._logStackTrace = value
        End Set
    End Property

    Private _logDate As DateTime
    Public Property LogDate As DateTime
        Get
            Return Me._logDate
        End Get
        Set(value As DateTime)
            Me._logDate = value
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

    Public Function LoadAll(ByVal startDate As Date) As List(Of ExceptionLog)

        Dim exceptionLogs As List(Of ExceptionLog) = New List(Of ExceptionLog)()

        If Me._connection Is Nothing AndAlso String.IsNullOrEmpty(Me._connectionString) Then
            Throw New ArgumentNullException("MHB.Logging.ExceptionLog.LoadAll: connectionString parameter is null or empty! Either provide a valid connection string or pass an existing SqlConnection!")
        End If

        If Me._connection Is Nothing Then
            Me._connection = New SqlConnection(Me._connectionString)
        End If

        Dim reader As IDataReader = SQLHelper.LoadExceptionLogs(Me._connection, startDate)

        While reader.Read()

            Dim log As ExceptionLog = New ExceptionLog()

            If Not reader.IsDBNull(reader.GetOrdinal("ID")) Then
                log.ID = DirectCast(reader("ID"), Integer)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logUserID")) Then
                log.UserID = DirectCast(reader("logUserID"), Integer)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logMethod")) Then
                log.MethodName = DirectCast(reader("logMethod"), String)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logExceptionMessage")) Then
                log.LogMessage = DirectCast(reader("logExceptionMessage"), String)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logInnerExceptionMessage")) Then
                log.LogInnerExceptionMessage = DirectCast(reader("logInnerExceptionMessage"), String)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logExceptionSource")) Then
                log.LogSource = DirectCast(reader("logExceptionSource"), String)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logStackTrace")) Then
                log.LogStackTrace = DirectCast(reader("logStackTrace"), String)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logDateTime")) Then
                log.LogDate = DirectCast(reader("logDateTime"), DateTime)
            End If

            If Not reader.IsDBNull(reader.GetOrdinal("logSqlQuery")) Then
                log.SqlQuery = DirectCast(reader("logSqlQuery"), String)
            End If

            exceptionLogs.Add(log)

        End While

        If Not reader.IsClosed Then
            reader.Close()
        End If

        Return exceptionLogs

    End Function

#End Region

End Class
