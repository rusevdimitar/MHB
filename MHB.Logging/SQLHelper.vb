Imports System.Data.SqlClient
Imports MHB.DAL

Public Class SQLHelper

    Public Shared Function LoadActionLogs(ByVal connectionString As String, ByVal startDate As DateTime) As IDataReader
        Return LoadActionLogsInternal(connectionString, Nothing, startDate)
    End Function

    Public Shared Function LoadActionLogs(ByVal connection As SqlConnection, ByVal startDate As DateTime) As IDataReader
        Return LoadActionLogsInternal(Nothing, connection, startDate)
    End Function

    Private Shared Function LoadActionLogsInternal(ByVal connectionString As String, ByVal connection As SqlConnection, ByVal startDate As DateTime) As IDataReader

        Dim qry As String = String.Empty

        If connection Is Nothing AndAlso String.IsNullOrEmpty(connectionString) Then
            Throw New ArgumentNullException("MHB.Logging.Logger.LogAction: connectionString parameter is null or empty! Either provide a valid connection string or pass an existing SqlConnection!")
        End If

        If connection Is Nothing Then
            connection = New SqlConnection(connectionString)
        End If

        Dim reader As IDataReader

        Try

            qry = "EXECUTE spGetActionLogData @startDate"

            Dim parStartDate As SqlParameter = New SqlParameter("@startDate", startDate)

            reader = DataBaseConnector.GetDataReader(qry, connection, parStartDate)

            Return reader

        Catch ex As Exception
            Logger.Log(ex, "MHB.Logging.SQLHelper.LoadActionLogs", qry, 0, connection.ConnectionString)
            Return Nothing
        End Try

    End Function

    Public Shared Function LoadExceptionLogs(ByVal connectionString As String, ByVal startDate As DateTime) As IDataReader
        Return LoadExceptionLogsInternal(connectionString, Nothing, startDate)
    End Function

    Public Shared Function LoadExceptionLogs(ByVal connection As SqlConnection, ByVal startDate As DateTime) As IDataReader
        Return LoadExceptionLogsInternal(Nothing, connection, startDate)
    End Function

    Public Shared Function LoadExceptionLogsInternal(ByVal connectionString As String, ByVal connection As SqlConnection, ByVal startDate As DateTime) As IDataReader

        Dim qry As String = String.Empty

        If connection Is Nothing AndAlso String.IsNullOrEmpty(connectionString) Then
            Throw New ArgumentNullException("MHB.Logging.Logger.LoadExceptionLogs: connectionString parameter is null or empty! Either provide a valid connection string or pass an existing SqlConnection!")
        End If

        If connection Is Nothing Then
            connection = New SqlConnection(connectionString)
        End If

        Dim reader As IDataReader

        Try

            qry = "SELECT ID, logMethod, logExceptionMessage, logInnerExceptionMessage, logExceptionSource, logStackTrace, logUserID, logDateTime, logSqlQuery " & _
                  "FROM dbo.tbLog WHERE logDateTime > @logDateTime"

            Dim parStartDate As SqlParameter = New SqlParameter("@logDateTime", startDate)

            reader = DataBaseConnector.GetDataReader(qry, connection, parStartDate)

            Return reader

        Catch ex As Exception
            Logger.Log(ex, "MHB.Logging.SQLHelper.LoadExceptionLogs", qry, 0, connection.ConnectionString)
            Return Nothing
        End Try

    End Function

End Class