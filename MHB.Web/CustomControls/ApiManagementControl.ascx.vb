Public Class ApiManagementControl
    Inherits System.Web.UI.UserControl

    Dim en As Environment = New Environment()



    Protected Sub Page_Init() Handles Me.Init

        If Not IsPostBack Then

            Dim key As String = Me.GetUserApiKey()

            If String.IsNullOrWhiteSpace(key) Then
                LabelApiKey.Text = Environment.EMPTY_API_KEY
            Else
                LabelApiKey.Text = key
            End If

            LabelApiServiceUrl.Text = ConfigurationManager.AppSettings.Get("APIServiceUrl")

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not IsPostBack Then
        '    en.TranslateControls(Page.Controls)
        'End If
    End Sub

    Protected Sub ButtonLogin_Click(sender As Object, e As EventArgs) Handles ButtonLogin.Click

        Try

            If en.UserID = UserManager.User.GetUserID(TextBoxUserName.Text, TextBoxPassword.Text, en.GetConnectionString) Then
                LabelApiKey.Text = UserManager.User.GenerateAPIKey(TextBoxUserName.Text, TextBoxPassword.Text, False, en.GetConnectionString)
                Logging.Logger.LogAction(Logging.Logger.HistoryAction.GenerateAPIKeySuccess, en.UserID, en.GetConnectionString, Request.UserHostAddress)
            Else
                Environment.DisplayWebPageMessage(Me, en.GetTranslatedValue("InvalidUserPass", en.CurrentLanguage))
                Logging.Logger.LogAction(Logging.Logger.HistoryAction.GenerateAPIKeyInvalidCredentials, en.UserID, en.GetConnectionString, Request.UserHostAddress)
            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "ApiManagementControl.ascx.vb.ButtonLogin_Click", String.Empty, en.UserID, en.GetConnectionString)
        End Try

    End Sub

    Protected Function GetUserApiKey() As String

        Dim key As String = Environment.EMPTY_API_KEY

        Try

            key = UserManager.User.GetAPIKey(en.UserID, en.GetConnectionString).Key

        Catch ex As Exception
            Logging.Logger.Log(ex, "ApiManagementControl.ascx.vb.GetUserApiKey", String.Empty, en.UserID, en.GetConnectionString)
        End Try

        Return key
    End Function
End Class