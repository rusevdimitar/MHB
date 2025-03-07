Public Class SharedNotes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Me.IsPostBack Then

            Environment.ExecuteScript(sender, "ShowNotesPanel();", True)

        End If

    End Sub

End Class