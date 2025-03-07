Imports System.Drawing

Partial Public Class GetCaptcha
    Inherits Environment

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Dim b As Bitmap = CaptchaOLD.DrawText(Request.QueryString("text"))

            Dim tempStream As System.IO.MemoryStream = New System.IO.MemoryStream()
            b.Save(tempStream, System.Drawing.Imaging.ImageFormat.Gif)

            Response.AddHeader("Content-Disposition", "inline; filename=captcha.gif")
            Response.ContentType = "gif"
            Response.BinaryWrite(tempStream.ToArray())
        Catch ex As Exception
            Logging.Logger.Log(ex, "GetCaptcha.aspx.vb > Page_Load()", "Request.QueryString(text) = " & Request.QueryString("text"), UserID, GetConnectionString)
            Throw New Exception("GetCaptcha.aspx.vb > Page_Load()", ex)
        End Try
    End Sub

End Class