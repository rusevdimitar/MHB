Imports MHB.BL

Public Class APIManagement
    Inherits Environment

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CheckAccess()
    End Sub

End Class