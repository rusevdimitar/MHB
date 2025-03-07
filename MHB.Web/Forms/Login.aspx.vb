Imports System.Data.SqlClient
Imports System.Net
Imports System.Xml
Imports MHB.BL
Imports MHB.DAL

Partial Public Class Login
    Inherits Environment

    Protected Overloads Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    End Sub

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not IsPostBack Then

                If Me.SnowFallEnabled Then

                    Page.ClientScript.RegisterStartupScript(Me.GetType, "Snow", "<script src='../Javascript/snowstorm.js' type='text/javascript'></script>")

                    Dim scriptSettings As String =
                        "<script>" &
                            "snowStorm.flakesMaxActive = 96" &
                        "</script>"

                    Page.ClientScript.RegisterStartupScript(Me.GetType, "SnowSettings", scriptSettings)

                End If

                ImageButtonScreenShots.Attributes.Add("onmouseout", String.Format("javascript:getElementById('{0}').src ='../Images/scrnsht_button_normal_1.png';", ImageButtonScreenShots.ClientID))
                ImageButtonScreenShots.Attributes.Add("onmouseover", String.Format("javascript:getElementById('{0}').src ='../Images/scrnsht_button_hover_1.png';", ImageButtonScreenShots.ClientID))

                ImageButtonDemo.Attributes.Add("onmouseout", String.Format("javascript:getElementById('{0}').src ='../Images/demo_button_normal_1.png';", ImageButtonDemo.ClientID))
                ImageButtonDemo.Attributes.Add("onmouseover", String.Format("javascript:getElementById('{0}').src ='../Images/demo_button_hover_1.png';", ImageButtonDemo.ClientID))

                ImageButtonDownloadInstaller.Attributes.Add("onmouseout", String.Format("javascript:getElementById('{0}').src ='../Images/btn_download_standalone.png';", ImageButtonDownloadInstaller.ClientID))
                ImageButtonDownloadInstaller.Attributes.Add("onmouseover", String.Format("javascript:getElementById('{0}').src ='../Images/btn_download_standalone_hover.png';", ImageButtonDownloadInstaller.ClientID))

                DivRecoverPass.Attributes("class") = "CenteredDivRecoverPassword"

                Me.SetWaterMarks()

                Me.DisplayCaptcha()

                If Request.QueryString("UserID") <> Nothing AndAlso IsNumeric(Request.QueryString("UserID")) Then

                    Session.Clear()

                    If Request.IsLocal OrElse (Me.CurrentUser.AutoLoginIsAllowed = True _
                        AndAlso ((DateTime.Now.TimeOfDay < Me.CurrentUser.AutoLoginStartTime OrElse DateTime.Now.TimeOfDay > Me.CurrentUser.AutoLoginEndTime) OrElse Not DateTime.Now.IsWeekDay()) _
                        AndAlso (Me.CurrentUser.AutoLoginHomeAddress.Equals(IPAddress.Parse(Request.UserHostAddress)) OrElse HttpContext.Current.Request.IsLocal)) Then

                        Me.UserID = CInt(Request.QueryString("UserID"))

                        Response.Redirect(URLRewriter.GetLink("MainForm"))
                    Else
                        Session.Clear()
                        Me.UserID = 0
                    End If

                End If
            End If
        Catch tax As Threading.ThreadAbortException
        Catch ex As Exception
            Logging.Logger.Log(ex, "Login.aspx.vb>Page_Load:", String.Empty, UserID, GetConnectionString)
        End Try

    End Sub

    Protected Sub SetWaterMarks()

        Try

            Dim textStrengthDescriptions As StringBuilder = New StringBuilder()
            textStrengthDescriptions.Append(GetTranslatedValue("passverypoor", CurrentLanguage))
            textStrengthDescriptions.Append(";")
            textStrengthDescriptions.Append(GetTranslatedValue("passweak", CurrentLanguage))
            textStrengthDescriptions.Append(";")
            textStrengthDescriptions.Append(GetTranslatedValue("passaverage", CurrentLanguage))
            textStrengthDescriptions.Append(";")
            textStrengthDescriptions.Append(GetTranslatedValue("passstrong", CurrentLanguage))
            textStrengthDescriptions.Append(";")
            textStrengthDescriptions.Append(GetTranslatedValue("passexcellent", CurrentLanguage))
            textStrengthDescriptions.Append(";")

            PasswordStrength1.TextStrengthDescriptions = textStrengthDescriptions.ToString()
            PasswordStrength1.PrefixText = GetTranslatedValue("passprefixtext", CurrentLanguage)

            TextBoxWatermarkExtender1.WatermarkText = GetTranslatedValue("typemailwatermark", CurrentLanguage)
            TextBoxWatermarkExtender2.WatermarkText = GetTranslatedValue("typepasswatermark", CurrentLanguage)
            TextBoxWatermarkExtender3.WatermarkText = GetTranslatedValue("typemailwatermark", CurrentLanguage)
            TextBoxWatermarkExtender4.WatermarkText = GetTranslatedValue("typepasswatermark", CurrentLanguage)
        Catch

        End Try

    End Sub

    Protected Sub DisplayCaptcha()

        Try
            Dim i As Integer = 0

            ImageCaptcha.ImageUrl = "~" & Captcha.Captcha.GetCaptchaBitmap(Server.MapPath(".") & "\..", i)
            ViewState("captcha") = i
        Catch ex As Exception
            Throw New Exception("Login.aspx.vb.DisplayCaptcha():" & ex.Message, ex)
        End Try

    End Sub

    Protected Sub ButtonLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonLogin.Click

        Session.Clear()

        Dim qryForLogFile As String = String.Empty

        Try

            Me.CurrentLanguage = Nothing

            If TextBoxEmail.Text.Trim().Length = 0 Then
                Environment.DisplayWebPageMessage(sender, "Please enter an email.")
                TextBoxEmail.Focus()
                Return
            End If

            Dim result As Integer = 0

            result = UserManager.User.GetUserID(TextBoxEmail.Text.Trim(), TextBoxPassword.Text.Trim(), Me.GetConnectionString)

            System.Threading.Tasks.Task.Factory.StartNew(Sub()
                                                             UserManager.User.UpdateUserGeoLocationInfo(result, Me.GetConnectionString, Request.UserHostAddress)
                                                         End Sub)

            If result <> 0 Then
                Me.UserID = result
                Response.Redirect(MHB.BL.URLRewriter.GetLink("MainForm"))
            Else
                Environment.DisplayWebPageMessage(sender, Me.GetTranslatedValue("InvalidUserPass", Me.CurrentLanguage))
                LabelError.Text = Me.GetTranslatedValue("InvalidUserPass", Me.CurrentLanguage)
                TextBoxPassword.Focus()
            End If

        Catch te As Threading.ThreadAbortException
        Catch ex As Exception
            Logging.Logger.Log(ex, "Login.aspx.vb.ButtonLogin_Click()", qryForLogFile, Me.UserID, Me.GetConnectionString)
        Finally
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.Login, Me.UserID, Me.GetConnectionString, Request.UserHostAddress)
        End Try

    End Sub

    Protected Sub ButtonRegister_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRegister.Click

        Dim qry As String = String.Empty

        Session.Clear()

        Try

            If Not Me.CheckUserInput(sender) Then
                Return
            End If

            If Me.CheckIfUserNameTaken() Then
                Return
            End If

            Dim userID As Integer = Me.GetNewUserID()

            If userID > 0 Then
                Me.UserID = userID
            End If

            qry =
"INSERT INTO tbUsers ([Password], UserID ,Email, Currency, [Language], HasSetLang, RegistrationDate, AttachmentSize)" & vbCrLf &
"VALUES(CAST(@Password AS VARBINARY), @UserID, @Email, 'BGN', @Language, 0, GETDATE(), 500000)" & vbCrLf

            Dim sqlParameters As List(Of SqlParameter) = New List(Of SqlParameter)()

            Dim paramEmail As SqlParameter = New SqlParameter("Email", SqlDbType.VarChar, 100)
            paramEmail.Value = TextBoxRegisterEmail.Text.Trim()

            Dim paramPassword As SqlParameter = New SqlParameter("Password", SqlDbType.VarChar, 50)
            paramPassword.Value = TextBoxRegisterPass.Text.Trim()

            sqlParameters.Add(paramEmail)
            sqlParameters.Add(paramPassword)

            sqlParameters.Add(New SqlParameter("UserID", Me.UserID))
            sqlParameters.Add(New SqlParameter("Language", Me.CurrentLanguage))

            For i As Integer = 0 To 5

                qry &= String.Format("INSERT INTO tbMonthlyBudget (UserID, [Year]) VALUES (@UserID, @Year{0})", i) & vbCrLf

                qry &= String.Format("INSERT INTO {0} (UserID) VALUES (@UserID)", Me.MainTable) & vbCrLf

                qry &= String.Format("INSERT INTO tbMonthlyExpenses (UserID, [Year]) VALUES (@UserID, @Year{0})", i) & vbCrLf

                qry &= String.Format("INSERT INTO tbMonthlySavings (UserID, [Year]) VALUES (@UserID, @Year{0})", i) & vbCrLf

                sqlParameters.Add(New SqlParameter(String.Format("Year{0}", i), CStr(Year + i)))

            Next

            DataBaseConnector.ExecuteQuery(qry, Me.GetConnectionString, sqlParameters.ToArray())

            Try
                MHB.Mail.Mail.SendWelcomeMail(TextBoxRegisterEmail.Text, Server.MapPath("\Forms\") & "RegisterMail.htm")
                MHB.Mail.Mail.SendMail("Нов потребител! <br/> User ID: " & userID & "<br/>E-Mail: " & TextBoxRegisterEmail.Text & "<br/>Password: " & TextBoxRegisterPass.Text, "Нов потребител!", "rusev.dimitar@gmail.com", SenderEmailAddress)
            Catch
            End Try

            Response.Redirect(MHB.BL.URLRewriter.GetLink("MainForm"))

        Catch x As Threading.ThreadAbortException
        Catch ex As Exception
            LabelError.Text = ex.Message
            Logging.Logger.Log(ex, "Login.aspx.vb > ButtonRegister_Click()", qry, Me.UserID, Me.GetConnectionString)
        Finally
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.Register, Me.UserID, Me.GetConnectionString, Me.Request.UserHostAddress)
        End Try
    End Sub

    Private Function CheckUserInput(ByVal sender As Object) As Boolean

        If String.IsNullOrWhiteSpace(TextBoxRegisterEmail.Text) Then

            Environment.DisplayWebPageMessage(sender, Me.GetTranslatedValue("TextBoxRegisterEmailAlert", Me.CurrentLanguage))
            TextBoxRegisterEmail.Focus()

            Return False

        ElseIf String.IsNullOrWhiteSpace(TextBoxRegisterPass.Text) Then

            Environment.DisplayWebPageMessage(sender, Me.GetTranslatedValue("TextBoxRegisterPassAlert", Me.CurrentLanguage))
            TextBoxRegisterPass.Focus()

            Return False

        ElseIf TextBoxConfirmCaptchaText.Text <> CStr(ViewState("captcha")) Then

            LabelError.Text = Me.GetTranslatedValue("captchanomatch", Me.CurrentLanguage)
            Me.DisplayCaptcha()

            Return False

        Else
            Return True
        End If

    End Function

    Private Function CheckIfUserNameTaken()

        Dim qry As String = "SELECT Email FROM tbUsers WHERE Email = @Email"

        Dim result As Object = DataBaseConnector.GetSingleValue(qry, Me.GetConnectionString, New SqlParameter("Email", TextBoxRegisterEmail.Text))

        If Not String.IsNullOrEmpty(result) Then
            LabelError.Text = Me.GetTranslatedValue("UserNameTaken", Me.CurrentLanguage)
            Return True
        Else
            Return False
        End If

    End Function

    Private Function GetNewUserID() As Integer
        Dim qry As String = "SELECT MAX(UserID) FROM tbUsers"

        Dim result As Object = DataBaseConnector.GetSingleValue(qry, Me.GetConnectionString)

        If IsNumeric(result) Then
            Return result + 1
        Else
            Return -1
        End If
    End Function

    Protected Sub LinkButtonForgottenPasswod_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonForgottenPasswod.Click
        DivRecoverPass.Style("visibility") = "visible"
    End Sub

#Region "[ ButtonRecoverPassword_Click ]"
    Protected Sub ButtonRecoverPassword_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonRecoverPassword.Click

        Logging.Logger.LogAction(Logging.Logger.HistoryAction.RecoverPassword, UserID, GetConnectionString, Request.UserHostAddress)

        Dim qry As String = String.Format(
"SELECT CAST([password] AS VARCHAR) FROM [dbo].[tbUsers] WHERE [email] = '{0}'",
 TextBoxEnterEmailToRecover.Text)

        Try

            Dim pass As String = DataBaseConnector.GetSingleValue(qry, GetConnectionString)

            If pass.Length = 0 Then
                ClientScript.RegisterStartupScript(Me.GetType, "nosuchemailregistered", "<script language='javascript'>alert('" & GetTranslatedValue("nosuchemailregistered", CurrentLanguage) & "')</script>")
                Exit Sub
            End If

            Dim sendSuccess As Boolean = False

            sendSuccess = MHB.Mail.Mail.SendMail("MyHomeBills Password Recovery: <br/> User: " & TextBoxEnterEmailToRecover.Text & "<br/> Password: " & pass, "[MyHomeBills] Password recovery/Възстановяване парола", TextBoxEnterEmailToRecover.Text, SenderEmailAddress)

            If sendSuccess = False Then
                ClientScript.RegisterStartupScript(Me.GetType, "norecoveremailalert", String.Format("<script language='javascript'>alert('{0}')</script>", GetTranslatedValue("norecoveremailalert", CurrentLanguage)))
            End If

            DivRecoverPass.Style("visibility") = "hidden"

            ClientScript.RegisterStartupScript(Me.GetType(), "passwordrecover", String.Format("<script language='javascript'>alert('{0}')</script>", String.Format(GetTranslatedValue("RecoverPasswordEmailSentMsg", CurrentLanguage), TextBoxEnterEmailToRecover.Text)))

        Catch ex As Exception
            Logging.Logger.Log(ex, "ButtonRecoverPassword_Click()", qry, UserID, GetConnectionString)
        End Try

    End Sub
#End Region

    Protected Sub ButtonCloseRecoverPassDiv_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonCloseRecoverPassDiv.Click
        DivRecoverPass.Style("visibility") = "hidden"
    End Sub

    'Protected Sub ImageButtonVideoTutorial_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonVideoTutorial.Click
    '    Logging.Logger.LogAction(Logging.Logger.HistoryAction.VideoTutorial, UserID, GetConnectionString, Request.UserHostAddress)
    'End Sub

    Protected Sub ImageButtonScreenshots_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonScreenShots.Click
        Logging.Logger.LogAction(Logging.Logger.HistoryAction.ScreenShots, UserID, GetConnectionString, Request.UserHostAddress)
    End Sub

    Protected Sub ImageButtonStartDemo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonDemo.Click
        UserID = 25
        Logging.Logger.LogAction(Logging.Logger.HistoryAction.StartDemo, UserID, GetConnectionString, Request.UserHostAddress)
        Response.Redirect(MHB.BL.URLRewriter.GetLink("MainForm"))
    End Sub
End Class