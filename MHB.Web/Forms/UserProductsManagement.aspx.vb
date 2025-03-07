Public Class UserProductsManagement
    Inherits Environment

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.CheckAccess()

        Me.AddNavigationLink(Me, MHB.BL.URLRewriter.GetLink("ProductsManagement"), Me.GetTranslatedValue("productsmanagement", Me.CurrentLanguage))

        If Not IsPostBack Then
            Me.TabPanelCategories.HeaderText = Me.GetTranslatedValue("Categories", Me.CurrentLanguage)
            Me.TabPanelProducts.HeaderText = Me.GetTranslatedValue("Products", Me.CurrentLanguage)
            Me.TabPanelSuppliers.HeaderText = Me.GetTranslatedValue("Suppliers", Me.CurrentLanguage)
        End If

    End Sub

End Class