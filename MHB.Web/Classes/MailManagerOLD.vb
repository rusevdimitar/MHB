'Imports System.Web.Mail



'Public Class MailManagerOLD
'    Inherits Environment

'    Public Sub New()

'    End Sub



'    ''' <summary>
'    ''' Sends the welcome mail.
'    ''' </summary>
'    ''' <param name="sendMessageTo">The address to send a message to.</param>
'    ''' <returns>Boolean</returns>
'    Public Function SendWelcomeMail(ByVal sendMessageTo As String) As Boolean

'        Try

'            Dim msg As String = System.IO.File.ReadAllText(Server.MapPath("\Forms\") & "RegisterMail.htm")


'            Dim message As MailMessage = New MailMessage

'            message.From = "support@myhomebills.info"
'            message.To = sendMessageTo
'            message.Body = msg
'            message.BodyFormat = MailFormat.Html
'            message.BodyEncoding = Text.Encoding.UTF8
'            message.Subject = "Support MyHomeBills"

'            System.Web.Mail.SmtpMail.SmtpServer = "localhost"
'            System.Web.Mail.SmtpMail.Send(message)

'            Return True

'        Catch ex As Exception
'            Logging.Logger.Log(ex, "MailManager.vb > SendWelcomeMail()", "failed to send mail to: " & sendMessageTo)
'            Return False
'        End Try
'    End Function

'    ''' <summary>
'    ''' Sends an e-mail.
'    ''' </summary>
'    ''' <param name="sendMessageTo">The recepient's address</param>
'    ''' <param name="messageText">The message text.</param>
'    ''' <returns>Boolean</returns>
'    Public Function SendMail(ByVal sendMessageTo As String, ByVal messageText As String) As Boolean
'        Try



'            Dim message As MailMessage = New MailMessage

'            message.From = "support@myhomebills.info"
'            message.To = sendMessageTo
'            message.Body = messageText
'            message.BodyFormat = MailFormat.Html
'            message.BodyEncoding = Text.Encoding.UTF8
'            message.Subject = "Support MyHomeBills"

'            System.Web.Mail.SmtpMail.SmtpServer = "localhost"
'            System.Web.Mail.SmtpMail.Send(message)

'            Return True


'        Catch ex As Exception
'            Logging.Logger.Log(ex, "MailManager.vb > SendMail()", "failed to send mail to: " & sendMessageTo)
'            Return False
'        End Try
'    End Function





'End Class
