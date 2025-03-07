Public Class MobileLogin
    Inherits Environment


    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub ButtonLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonLogin.Click

        Dim result As Integer = 0


        result = UserManager.User.GetUserID(TextBoxUserName.Text.Trim(), TextBoxPassword.Text.Trim(), GetConnectionString)

        If result <> 0 Then
            UserID = result
            Response.Redirect(MHB.BL.URLRewriter.GetLink("MainFormMobile"))
        Else
            LabelError.Text = "Invalid email/password entered"
            TextBoxPassword.Focus()
        End If
    End Sub
End Class