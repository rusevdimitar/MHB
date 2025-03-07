Imports MHB.DAL
Imports MHB.Logging.Logger

Public Class ExchangeRates
    Inherits System.Web.UI.UserControl

    Dim _en As Environment

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me._en = New Environment()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Try

                Dim code As String = IIf(String.IsNullOrEmpty(Request.QueryString("code")), "UsdToBgn", Request.QueryString("code"))

                Dim min, max As Decimal

                Select Case code

                    Case "UsdToBgn"
                        max = Me._en.ExchangeRatesHistory().Max(Function(rate) rate.USDtoBGN)
                        min = Me._en.ExchangeRatesHistory().Min(Function(rate) rate.USDtoBGN)
                        Exit Select
                    Case "GbpToBgn"
                        max = Me._en.ExchangeRatesHistory().Max(Function(rate) rate.GBPtoBGN)
                        min = Me._en.ExchangeRatesHistory().Min(Function(rate) rate.GBPtoBGN)
                        Exit Select
                    Case "ChfToBgn"
                        max = Me._en.ExchangeRatesHistory().Max(Function(rate) rate.CHFtoBGN)
                        min = Me._en.ExchangeRatesHistory().Min(Function(rate) rate.CHFtoBGN)
                        Exit Select

                End Select

                With Chart1
                    .Series("Default").Points.DataBindXY(Me._en.ExchangeRatesHistory, "Date", Me._en.ExchangeRatesHistory, code)
                    .ChartAreas.First().AxisY.Minimum = min
                    .ChartAreas.First().AxisY.Maximum = max
                End With

                Logging.Logger.LogAction(HistoryAction.CurrencyExchangeRatesCharts, Me._en.UserID, Me._en.GetConnectionString, Request.UserHostAddress)

            Catch ex As Exception
                Logging.Logger.Log(ex, "Page_Load.ExchangeRates.ascx.vb", String.Empty, Me._en.UserID, Me._en.GetConnectionString)
            End Try

        End If

    End Sub

End Class