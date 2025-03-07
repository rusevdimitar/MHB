'Imports System.Data.SqlClient


'Public Class UserDetailsOLD
'    Inherits Environment

'    Dim _userID As Integer = 0

'#Region "[ Properties ]"

'    Public Property userPassword() As String
'        Get
'            Return CStr(Session("userPassword"))
'        End Get
'        Set(ByVal value As String)
'            Session("userPassword") = value
'        End Set
'    End Property

'    Public Property userEmail() As String
'        Get
'            Return CStr(Session("userEmail"))
'        End Get
'        Set(ByVal value As String)
'            Session("userEmail") = value
'        End Set
'    End Property

'    Public Property userCurrency() As String
'        Get
'            Return CStr(Session("userCurrency"))
'        End Get
'        Set(ByVal value As String)
'            Session("userCurrency") = value
'        End Set
'    End Property

'    Public Property userCurrentLanguage() As String
'        Get
'            Return CStr(Session("userCurrentLanguage"))
'        End Get
'        Set(ByVal value As String)
'            Session("userCurrentLanguage") = value
'        End Set
'    End Property

'    Public Property userHassetlang() As Boolean
'        Get
'            Return CBool(Session("userHassetlang"))
'        End Get
'        Set(ByVal value As Boolean)
'            Session("userHassetlang") = value
'        End Set
'    End Property

'    Public Property userRegistrationdate() As DateTime
'        Get
'            Return CDate(Session("userRegistrationdate"))
'        End Get
'        Set(ByVal value As DateTime)
'            Session("userRegistrationdate") = value
'        End Set
'    End Property

'    Public Property userAttachmentsize() As Integer
'        Get
'            Return CInt(Session("userAttachmentsize"))
'        End Get
'        Set(ByVal value As Integer)
'            Session("userAttachmentsize") = value
'        End Set
'    End Property

'    Public Property userLastlogintime() As DateTime
'        Get
'            Return CDate(IIf(IsDBNull(Session("userLastlogintime")), Today().ToString(), Session("userLastlogintime")))
'        End Get
'        Set(ByVal value As DateTime)
'            Session("userLastlogintime") = value
'        End Set
'    End Property

'    Public Property userUseragent() As String
'        Get
'            Return CStr(IIf(IsDBNull(Session("userUseragent")), "", Session("userUseragent")))
'        End Get
'        Set(ByVal value As String)
'            Session("userUseragent") = value
'        End Set
'    End Property

'    Public Property userLastIPAddress() As String
'        Get
'            Return CStr(IIf(IsDBNull(Session("userLastIPAddress")), "", Session("userLastIPAddress")))
'        End Get
'        Set(ByVal value As String)
'            Session("userLastIPAddress") = value
'        End Set
'    End Property

'#End Region

'#Region "[ Constructors ]"
'    Public Sub New(ByVal userID As Integer)
'        _userID = userID

'        Dim qry As String = _
'"SELECT [password],[email],[currency],[language],[hassetlang],[registrationdate],[attachmentsize],[lastlogintime],[lastipaddress],[useragent] FROM [dbo].[tbUsers] WHERE [userID] = " & userID.ToString()

'        Try

'            DatabaseConnector.GetDataReader(qry, GetConnectionString)

'            Dim reader As IDataReader = DatabaseConnector.GetDataReader(qry, GetConnectionString)

'            While reader.Read()
'                Session("userPassword") = reader("password")
'                Session("userEmail") = reader("email")
'                Session("userCurrency") = reader("currency")
'                Session("userCurrentLanguage") = reader("language")
'                Session("userHassetlang") = reader("hassetlang")
'                Session("userRegistrationdate") = reader("registrationdate")
'                Session("userAttachmentsize") = reader("attachmentsize")
'                Session("userLastlogintime") = reader("lastlogintime")
'                Session("userUseragent") = reader("useragent")
'                Session("userLastIPAddress") = reader("lastipaddress")
'            End While


'        Catch ex As Exception
'            Logging.Logger.Log(ex, "UserDetails.vb New(): ", qry)
'        End Try

'    End Sub

'#End Region

'#Region "[ Function: Update() As Boolean ]"
'    Public Function Update() As Boolean
'        Dim qry As String = _
'"UPDATE [dbo].[tbUsers]" & _
'" SET [password] = CAST('" & userPassword & "' AS VARBINARY) " & _
'" ,[email] = '" & userEmail & "'" & _
'" ,[currency] = '" & userCurrency & "'" & _
'" ,[language] = '" & userCurrentLanguage & "'" & _
'" ,[hassetlang] = '" & userHassetlang & "'" & _
'" ,[attachmentsize] = " & userAttachmentsize & _
'" WHERE [UserID] = " & _userID
'        Try

'            DatabaseConnector.ExecuteQuery(qry, GetConnectionString)

'        Catch ex As Exception
'            Logging.Logger.Log(ex, "UserDetails.vb > Save(): ", qry)
'            Return False
'        End Try
'    End Function

'#End Region

'#Region "[ Function: SetUsersCurrentLanguage() As Boolean ]"
'    ''' <summary>
'    ''' Sets the user's language. (note: does not translate the page controls! Use: SetCurrentLanguage(controlcollection) method!)
'    ''' </summary>
'    ''' <param name="lang">The language abreviation eg.'en','de','bg' etc.</param>
'    ''' <returns></returns>
'    Public Function SetUsersCurrentLanguage(ByVal lang As String) As Boolean
'        Dim qry As String = ""
'        Try
'            CurrentLanguage = lang
'            'hassetlang
'            qry = _
'"UPDATE [dbo].[tbUsers] SET [language] = '" & lang & "' WHERE [userID] = " & UserID & _
'" UPDATE [dbo].[tbUsers] SET [hassetlang] = 1 WHERE [userID] = " & UserID

'            DatabaseConnector.ExecuteQuery(qry, GetConnectionString)

'            If lang = "en" Then
'                'ClientScript.RegisterStartupScript(Me.GetType, "langen", "<script language='javascript'>alert('English set successfully.')</script>")
'                Return True
'            ElseIf lang = "bg" Then
'                'ClientScript.RegisterStartupScript(Me.GetType, "langbg", "<script language='javascript'>alert('Български избран успешно.')</script>")
'                Return True
'            ElseIf lang = "de" Then
'                'ClientScript.RegisterStartupScript(Me.GetType, "langde", "<script language='javascript'>alert('Deutsch wurde als Standardsprache ausgewählt.')</script>")
'                Return True
'            End If

'        Catch ex As Exception
'            Logging.Logger.Log(ex, "UserDetails.vb > SetUsersCurrentLanguage(ByVal lang As String)", qry)
'        End Try

'    End Function
'#End Region

'End Class
