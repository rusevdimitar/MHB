Imports System.Data.SqlClient
Imports System.Net
Imports System.Xml
Imports MHB.IPGeoLocation
Imports System.IO
Imports MHB.BL.Enums
Imports MHB.DAL

Partial Public Class MasterPage
    Inherits System.Web.UI.MasterPage

    Public _en As Environment = New Environment()

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Me._en.IsInEditMode Then
            Me.Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "Global", Me.ResolveClientUrl("~/Javascript/EditModeScripts.js"))
        Else
            Me.Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "Global", Me.ResolveClientUrl("~/Javascript/JQuery_functions.min.js"))
        End If

        If Not Page.IsPostBack Then

            If Not Request.IsLocal AndAlso UserManager.User.IsInBlackList(Request.UserHostAddress, String.Empty, Me._en.GetConnectionString, 0) Then
                Response.Redirect("AccessDenied.htm")
            End If

            If Me._en.UserID = 0 Then
                Me.SetUserLanguageByGeoIP()
            End If

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me._en.UserID = 0 Then
            PanelStripMenuLinks.Visible = False
        Else
            PanelStripMenuLinks.Visible = True
            LabelUser.Text = String.Format("({0})", Me._en.GetUserName)
            Try
                LabelLastUserInfo.Text = Me._en.userLastIPAddress
            Catch ex As Exception
                Logging.Logger.Log(ex, "MasterPage.master.vb > Page_Load: UserDetails.vb: ", "", Me._en.UserID, Me._en.GetConnectionString)
            End Try
        End If

        Page.ClientScript.RegisterStartupScript(Me.GetType, "Clock", "<script language='javascript'>StartTheClock()</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType, "PageLoadingTime", "<script language='javascript'>calculateloadgingtime()</script>")

        If Not IsPostBack Then

            If Me._en.UserID = 1 Then
                PanelAdminLinks.Visible = True
            Else
                PanelAdminLinks.Visible = False
            End If

            If Me._en.CurrentLanguage = Language.Bulgarian Then
                LabelExchangeRates.Text = Me._en.ExchangeRates
            End If

            If Me._en.IsInEditMode Then
                LinkButtonEditMode.Text = "Turn edit mode off"
            Else
                LinkButtonEditMode.Text = "Turn edit mode on"
            End If

            Me._en.TranslateControls(Me.Page.Controls)

        End If

    End Sub


    Protected Sub SetUserLanguageByGeoIP()

        Try

            Dim languageCode As String = String.Empty

            Dim geo As GeoLocation = New GeoLocation()

            If Request.IsLocal Then
                languageCode = "bg"
            Else
                'languageCode = geo.GetUserLanguageCode(Request.UserHostAddress)
                languageCode = Me._en.CurrentUser.GetVisitorLanguageCodeByAddress(Request.UserHostAddress)
            End If

            Select Case languageCode.ToLower()
                Case "bg", "mk"
                    Me._en.CurrentLanguage = Language.Bulgarian
                    Exit Select
                Case "us", "gb", "au"
                    Me._en.CurrentLanguage = Language.English
                    Exit Select
                Case "de", "at"
                    Me._en.CurrentLanguage = Language.German
                    Exit Select
                Case Else
                    Me._en.CurrentLanguage = Language.English
                    Exit Select
            End Select

        Catch ex As Exception
            Logging.Logger.Log(ex, "SetUserLanguageByGeoIP ", "", _en.UserID, _en.GetConnectionString)
        End Try

    End Sub

#Region "[ LinkButton.Click: LinkButtonSignOut_Click ]"

    Protected Sub LinkButtonSignOut_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonSignOut.Click

        For Each t As MHB.BL.Transaction In _en.Transactions
            t.Add()
        Next

        Logging.Logger.LogAction(Logging.Logger.HistoryAction.LogOut, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

        Dim qry As String =
"UPDATE dbo.tbUsers SET lastlogintime = GETDATE(), lastipaddress = @lastIp, useragent = @userAgent WHERE userID = @userID"

        Dim parLastIPAddress As SqlParameter = New SqlParameter("@lastIp", Request.UserHostAddress)
        Dim parUserAgent As SqlParameter = New SqlParameter("@userAgent", Request.UserAgent)
        Dim parUserID As SqlParameter = New SqlParameter("@userID", _en.UserID)

        Try
            DataBaseConnector.ExecuteQuery(qry, _en.GetConnectionString, parLastIPAddress, parUserAgent, parUserID)
        Catch ex As Exception
            Logging.Logger.Log(ex, "NONFATAL! MasterPage.master.vb > LinkButtonSignOut_Click: ", qry, _en.UserID, _en.GetConnectionString)
        End Try

        Session.Clear()

        Response.Redirect(MHB.BL.URLRewriter.GetLink("Login"))

    End Sub
#End Region

#Region "[ LinkButton.Click: LinkButtonHome_Click ]"
    Protected Sub LinkButtonHome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonHome.Click
        Response.Redirect(MHB.BL.URLRewriter.GetLink("MainForm"))
    End Sub
#End Region

#Region "[ LinkButton.Click: LinkButtonAccountSettings_Click ]"
    Protected Sub LinkButtonAccountSettings_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButtonAccountSettings.Click
        Response.Redirect(MHB.BL.URLRewriter.GetLink("Settings"))
    End Sub
#End Region

    Protected Sub ImageButtonBG_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonBG.Click

        Logging.Logger.LogAction(Logging.Logger.HistoryAction.ChangeCurrentLanguageBulgarian, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)

        Try
            Me._en.CurrentLanguage = Language.Bulgarian

            If Me._en.UserID <> 0 Then

                UserManager.User.SetUsersCurrentLanguage(UserManager.User.Language.Bulgarian, _en.UserID, _en.GetConnectionString)

                Me._en.TranslateControls(Page.Controls)
                'Page.ClientScript.RegisterStartupScript(Me.GetType, "LangAlertBG", "<script language='javascript'>alert('Настройте български като език по подразбиране в секцията настройки (линкът горе вдясно). По този начин, езиковите промени ще бъдат приложени изцяло върху всички елемент и българският ще бъде езика по подразбиране при следващите Ви посещения.')</script>")

                Response.Redirect(Request.Url.AbsoluteUri)

            Else

                Me._en.TranslateControls(Page.Controls)
                ' intro text show/hide
                'ContentPlaceHolder1.FindControl("PanelIntroBG").Visible = True
                'ContentPlaceHolder1.FindControl("PanelIntroEN").Visible = False
                ' intro logo show/hide
                'ContentPlaceHolder1.FindControl("IntroImageBG").Visible = True
                'ContentPlaceHolder1.FindControl("IntroImageEN").Visible = False

            End If

            Session("CostCategoriesKeywords") = Nothing

        Catch ex As Threading.ThreadAbortException
        Catch ex As Exception
            Logging.Logger.Log(ex, "MasterPage.master.vb > ImageButtonBG_Click()", "", _en.UserID, _en.GetConnectionString)
        End Try

    End Sub

    Protected Sub ImageButtonEN_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonEN.Click
        Try

            Logging.Logger.LogAction(Logging.Logger.HistoryAction.ChangeCurrentLanguageEnglish, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)

            _en.CurrentLanguage = Language.English

            If _en.UserID <> 0 Then
                UserManager.User.SetUsersCurrentLanguage(UserManager.User.Language.English, _en.UserID, _en.GetConnectionString)

                _en.TranslateControls(Page.Controls)
                'Page.ClientScript.RegisterStartupScript(Me.GetType, "LangAlertBG", "<script language='javascript'>alert('Tip: Set english as your default language in the account settings page. That way it would be stored as your language of choise for your next visits to the page.')</script>")
                Response.Redirect(Request.Url.AbsoluteUri)

            Else

                _en.TranslateControls(Page.Controls)

                ' intro text show/hide
                'ContentPlaceHolder1.FindControl("PanelIntroBG").Visible = False
                'ContentPlaceHolder1.FindControl("PanelIntroEN").Visible = True

                ' intro logo show/hide
                'ContentPlaceHolder1.FindControl("IntroImageBG").Visible = False
                'ContentPlaceHolder1.FindControl("IntroImageEN").Visible = True
            End If

            Session("CostCategoriesKeywords") = Nothing
        Catch ex As Threading.ThreadAbortException
        Catch ex As Exception
            Logging.Logger.Log(ex, "MasterPage.master.vb > ImageButtonEN_Click()", "", Me._en.UserID, Me._en.GetConnectionString)
        End Try

    End Sub

    Protected Sub ImageButtonDE_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonDE.Click

        Logging.Logger.LogAction(Logging.Logger.HistoryAction.ChangeCurrentLanguageDeutsch, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)
        Me._en.CurrentLanguage = Language.German

        Try
            If Me._en.UserID <> 0 Then
                UserManager.User.SetUsersCurrentLanguage(UserManager.User.Language.German, Me._en.UserID, Me._en.GetConnectionString)

                Me._en.TranslateControls(Page.Controls)
                'Page.ClientScript.RegisterStartupScript(Me.GetType, "LangAlertDE", "<script language='javascript'>alert('Tip: Ändern Sie die Einstellung für die Standardsprache. Klicken Sie auf Einstellungen (Link oben rechts). So wird jedesmal, wenn Sie MyHomeBills laden, Deutsch verwendet. ')</script>")
                Response.Redirect(Request.Url.AbsoluteUri)
            Else

                Me._en.TranslateControls(Page.Controls)

                ' intro text show/hide
                'ContentPlaceHolder1.FindControl("PanelIntroBG").Visible = False
                'ContentPlaceHolder1.FindControl("PanelIntroEN").Visible = True

                ' intro logo show/hide
                'ContentPlaceHolder1.FindControl("IntroImageBG").Visible = False
                'ContentPlaceHolder1.FindControl("IntroImageEN").Visible = True
            End If

            Session("CostCategoriesKeywords") = Nothing

        Catch ex As Threading.ThreadAbortException
        Catch ex As Exception
            Logging.Logger.Log(ex, "MasterPage.master.vb > ImageButtonDE_Click()", "", _en.UserID, _en.GetConnectionString)
        End Try
    End Sub

    Protected Sub LinkButtonPhorum_Click(sender As Object, e As EventArgs) Handles LinkButtonPhorum.Click
        Response.Redirect(MHB.BL.URLRewriter.GetLink("Phorum"))
    End Sub

    Protected Sub LinkButtonFAQ_Click(sender As Object, e As EventArgs) Handles LinkButtonFAQ.Click, LinkButtonHelp.Click
        Response.Redirect(MHB.BL.URLRewriter.GetLink("Help"))
        Logging.Logger.LogAction(Logging.Logger.HistoryAction.Help, _en.UserID, _en.GetConnectionString, Request.UserHostAddress)
    End Sub

    Protected Sub LinkButtonEditMode_Click(sender As Object, e As EventArgs) Handles LinkButtonEditMode.Click

        If Me._en.IsInEditMode = True Then
            Me._en.IsInEditMode = False
        Else
            Me._en.IsInEditMode = True
        End If

        Response.Redirect(Request.RawUrl)

    End Sub

End Class