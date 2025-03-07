Imports MHB.BL
Imports MHB.Logging

Namespace Forms
    Public Class BuyStandalone
        Inherits Environment

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not Me.IsPostBack Then

                Logger.LogAction(Logger.HistoryAction.DownloadPageOpened, 0, Me.GetConnectionString, Request.UserHostAddress)

                If Me.CurrentLanguage <> Enums.Language.Bulgarian Then
                    Logger.LogAction(Logger.HistoryAction.DownloadMyHomeBillsInstaller, 0, Me.GetConnectionString, Request.UserHostAddress)
                    Response.Redirect(URLRewriter.GetLink("DownloadLatestInstallerLink"))
                End If

            End If
        End Sub

        Protected Sub ImageButtonDownload_OnClick(sender As Object, e As ImageClickEventArgs)
            Logger.LogAction(Logger.HistoryAction.DownloadMyHomeBillsInstaller, 0, Me.GetConnectionString, Request.UserHostAddress)
            Response.Redirect(URLRewriter.GetLink("DownloadLatestInstallerLink"))
        End Sub
    End Class
End Namespace