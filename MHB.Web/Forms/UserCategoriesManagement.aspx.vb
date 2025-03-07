Imports MHB.BL
Imports System.IO


Partial Public Class UserCategoriesManagement
    Inherits Environment



    Protected Overloads Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init




    End Sub

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.CheckAccess()

        Me.AddNavigationLink(Me, MHB.BL.URLRewriter.GetLink("CategoriesManagement"), Me.GetTranslatedValue("categoriesmanagement", Me.CurrentLanguage))

    End Sub

End Class