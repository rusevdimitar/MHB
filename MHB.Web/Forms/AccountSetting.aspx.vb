Imports System.Data.SqlClient
Imports MHB.BL.Enums
Imports System.Net
Imports MHB.BL

Partial Public Class AccountSetting
    Inherits Environment

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Me.CheckAccess()

            If Not IsPostBack Then

                Me.BindCurrenciesDropDownList()

                Environment.SetDropDownSelectedValue(DropDownListCurrencies, Me.Currency)
                Environment.SetDropDownSelectedValue(DropDownListCurrentLanguage, Me.CurrentLanguage)

                TextBoxDateTimePickerAutoAccessTimeLimitsStart.Text = Me.CurrentUser.AutoLoginStartTime.ToString()
                TextBoxDateTimePickerAutoAccessTimeLimitsEnd.Text = Me.CurrentUser.AutoLoginEndTime.ToString()
                CheckBoxAutoLoginEnabled.Checked = Me.CurrentUser.AutoLoginIsAllowed

            End If

            Me.AddNavigationLink(Me, MHB.BL.URLRewriter.GetLink("Settings"), GetTranslatedValue("accountsettings", CurrentLanguage))
            Me.ApplyDropDownSkin(PanelAccountSettingsMain)

        Catch ex As Exception
            Logging.Logger.Log(ex, "AccountSettings.Page_Load", String.Empty, Me.UserID, Me.GetConnectionString)
        End Try

    End Sub

    Private Sub BindCurrenciesDropDownList()
        With DropDownListCurrencies
            .DataSource = Enums.CurrenciesList().Select(Function(dict) dict.Value)
            .DataTextField = "Name"
            .DataValueField = "Code"
            .DataBind()
        End With
    End Sub

    Protected Sub ButtonSaveChanges_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveChanges.Click

        Try
            With Me.CurrentUser

                .Currency = DropDownListCurrencies.SelectedValue

                If .UpdateUser() = True Then
                    Environment.DisplayWebPageMessage(sender, String.Format(Me.GetTranslatedValue("CurrencySetSuccessfully", Me.CurrentLanguage), .Currency))
                End If

            End With
        Catch ex As Exception
            Logging.Logger.Log(ex, "ButtonSaveChanges_Click", String.Empty, Me.UserID, Me.GetConnectionString)
        Finally
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.ChangeCurrency, UserID, GetConnectionString, Request.UserHostAddress)
        End Try

    End Sub

    Protected Sub ButtonSavePassword_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSavePassword.Click

        Try

            If TextBoxOldPassword.Text = Encoding.ASCII.GetString(Me.CurrentUser.Password) Then

                Me.CurrentUser.Password = Encoding.ASCII.GetBytes(TextBoxNewPassword.Text)

                If Me.CurrentUser.UpdateUser() = True Then
                    Environment.DisplayWebPageMessage(sender, Me.GetTranslatedValue("PasswordChangedSuccessfully", Me.CurrentLanguage))
                End If

            Else
                Environment.DisplayWebPageMessage(sender, Me.GetTranslatedValue("OldPasswordDoesNotMatch", Me.CurrentLanguage))
            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "ButtonSavePassword_Click", String.Empty, Me.UserID, Me.GetConnectionString)
        Finally
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.ChangePassword, Me.UserID, Me.GetConnectionString, Request.UserHostAddress)
        End Try

    End Sub

    Protected Sub ButtonSaveCurrentLanguage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveCurrentLanguage.Click

        Try
            With Me.CurrentUser

                Me.CurrentLanguage = DropDownListCurrentLanguage.SelectedValue

                .SelectedLanguage = Me.CurrentLanguage

                If .UpdateUser() = True Then
                    Environment.DisplayWebPageMessage(sender, Me.GetTranslatedValue("UserLanguageSetSuccessfully", Me.CurrentLanguage))
                End If

            End With
        Catch ex As Exception
            Logging.Logger.Log(ex, "ButtonSaveCurrentLanguage_Click", String.Empty, Me.UserID, Me.GetConnectionString)
        Finally
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.ChangeUserLanguage, Me.UserID, Me.GetConnectionString, Request.UserHostAddress)
        End Try

    End Sub

    Protected Sub ButtonSaveAutoLoginSettings_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonSaveAutoLoginSettings.Click

        Try

            Dim ip As IPAddress = Nothing

            If Not IPAddress.TryParse(Me.Page.Request.UserHostAddress, ip) Then
                Throw New Exception(String.Format("Invalid IP Address:{0}", ip.ToString()))
            End If

            With Me.CurrentUser

                .AutoLoginStartTime = TimeSpan.Parse(TextBoxDateTimePickerAutoAccessTimeLimitsStart.Text)
                .AutoLoginEndTime = TimeSpan.Parse(TextBoxDateTimePickerAutoAccessTimeLimitsEnd.Text)
                .AutoLoginIsAllowed = CheckBoxAutoLoginEnabled.Checked

                If .AutoLoginIsAllowed Then
                    .AutoLoginHomeAddress = ip
                Else
                    .AutoLoginHomeAddress = IPAddress.Any
                End If

                Dim result As Boolean = .UpdateUser()

                If result = True Then
                    Environment.DisplayWebPageMessage(sender, Me.GetTranslatedValue("AutoLoginSettingsSetSuccessfully", Me.CurrentLanguage))
                End If

            End With

        Catch ex As Exception
            Logging.Logger.Log(ex, "ButtonSaveAutoLoginSettings_Click", String.Empty, Me.UserID, Me.GetConnectionString)
        Finally
            Logging.Logger.LogAction(Logging.Logger.HistoryAction.ChangeUserAutoLoginSettings, Me.UserID, Me.GetConnectionString, Request.UserHostAddress)
        End Try

    End Sub

End Class