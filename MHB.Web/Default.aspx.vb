Imports MHB.BL

Partial Public Class _Default1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Redirect(URLRewriter.GetLink("Login"))

    End Sub

End Class