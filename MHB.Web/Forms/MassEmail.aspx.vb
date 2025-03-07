Imports MHB.BL

Public Class MassEmail
    Inherits Environment

    Protected Overloads Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        chkSendToSingleRecepeint.Attributes.Add("onclick", String.Format("EnableAddressBar('{0}')", chkSendToSingleRecepeint.ClientID))

    End Sub

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If UserID <> 1 Then
            Response.Redirect("Login.aspx")
        End If


    End Sub



    Protected Sub btnSendMassMail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendMassMail.Click


        Dim message As String = EmailTemplateLetter

        message = message.Replace(EmailTemplatePlaceHolderCode, Editor1.Content)

        If chkSendToSingleRecepeint.Checked Then

            MHB.Mail.Mail.SendMail(message, txtSubject.Text, txtEmailAddress.Text, SenderEmailAddress)
            lblCounter.Text = String.Format("Email sent to: {0}", txtEmailAddress.Text)

        Else
            Dim reader As IDataReader = UserManager.User.GetUserEmails(GetConnectionString, UserID)
            Dim counter As Integer = 0

            While reader.Read()

                Try

                    MHB.Mail.Mail.SendMail(message, txtSubject.Text, reader("email"), SenderEmailAddress)

                    counter += 1

                Catch ex As Exception
                    Logging.Logger.Log(ex, "btnSendMassMail_Click", String.Empty, UserID, GetConnectionString)
                End Try

            End While

            lblCounter.Text = String.Format("{0} emails have been sent.", counter.ToString())
        End If

       

    End Sub
End Class