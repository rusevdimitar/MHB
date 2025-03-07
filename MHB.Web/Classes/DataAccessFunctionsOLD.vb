'Imports System.Data.SqlClient
'Imports Microsoft.Practices.EnterpriseLibrary.Data
'Imports System.Data.Common

'Public Class MHB.DALOLD
'    Inherits Database


'    Public Sub New(ByVal connectionString As String)
'        MyBase.New(connectionString, SqlClient.SqlClientFactory.Instance)
'    End Sub

'    Protected Overrides Sub DeriveParameters(ByVal discoveryCommand As System.Data.Common.DbCommand)

'    End Sub


'    Public Shared Function GetDataReader(ByVal qry As String) As IDataReader

'        Dim en As Environment = New Environment()

'        Try
'            Dim reader As IDataReader

'            Using cmd As DbCommand = en.db.GetSqlStringCommand(qry)

'                reader = en.db.ExecuteReader(cmd)

'            End Using

'            Return reader


'        Catch ex As Exception
'            Logging.Logger.Log(ex, "MHB.DAL.vb > GetDataReader(ByVal qry As String)", qry)
'            Throw New Exception("MHB.DAL.vb > GetDataReader(ByVal qry As String)" & ex.Message, ex)
'        End Try


'    End Function

'    Public Shared Function GetSingleValue(ByVal qry As String) As Object

'        Dim en As Environment = New Environment()

'        Dim result As Object = New Object()

'        Try
'            Using cmd As DbCommand = en.db.GetSqlStringCommand(qry)
'                Using cn As DbConnection = en.db.CreateConnection()
'                    cn.Open()
'                    cmd.Connection = cn
'                    result = en.db.ExecuteScalar(cmd)
'                    cn.Close()
'                End Using

'            End Using

'            If IsDBNull(result) OrElse result Is Nothing OrElse result Is DBNull.Value Then
'                Return String.Empty
'            Else
'                Return result
'            End If

'        Catch ex As Exception
'            Logging.Logger.Log(ex, "MHB.DAL.vb > GetSingleValue()", qry)
'            Throw New Exception("MHB.DAL.vb > GetSingleValue()" & ex.Message, ex)
'        End Try


'    End Function

'    Public Shared Function ExecuteQuery(ByVal qry As String, ByVal ParamArray args() As Object) As Boolean

'        Dim en As Environment = New Environment()


'        Dim trans As DbTransaction = Nothing

'        Try
'            If qry.Length > 0 Then

'                qry = String.Format(qry, args)

'                Using cn As DbConnection = en.db.CreateConnection

'                    If cn.State <> ConnectionState.Open Then
'                        cn.Open()
'                    End If

'                    trans = cn.BeginTransaction()

'                    Dim cmd As DbCommand = en.db.GetSqlStringCommand(qry)

'                    en.db.ExecuteNonQuery(cmd, trans)
'                    trans.Commit()

'                    If cn.State <> ConnectionState.Closed Then
'                        cn.Close()
'                    End If

'                End Using
'                Return True
'            Else
'                Return False
'            End If


'        Catch ex As Exception
'            Logging.Logger.Log(ex, "MHB.DAL.vb > ExecuteQuery()", qry)
'            Return False
'        End Try


'    End Function



'    Public Shared Function GetDataTable(ByVal qry As String) As DataTable

'        Dim en As Environment = New Environment()

'        Try

'            Dim table As DataTable = New DataTable()
'            Using cmd As DbCommand = en.db.GetSqlStringCommand(qry)
'                Using adapter As DbDataAdapter = en.db.GetDataAdapter()
'                    adapter.SelectCommand = cmd
'                    adapter.SelectCommand.Connection = en.db.CreateConnection()
'                    adapter.SelectCommand.Connection.Open()
'                    adapter.Fill(table)
'                    adapter.SelectCommand.Connection.Close()
'                End Using
'            End Using

'            Return table

'        Catch ex As Exception
'            Logging.Logger.Log(ex, "MHB.DAL.vb > GetDataTable()", qry)
'            Throw New Exception("MHB.DAL.vb > GetDataTable()" & ex.Message, ex)
'        End Try


'    End Function




'End Class
