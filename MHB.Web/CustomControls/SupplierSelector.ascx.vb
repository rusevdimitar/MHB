Imports MHB.BL

Public Class SupplierSelector
    Inherits System.Web.UI.UserControl

    Dim _environment As Environment = New Environment()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim suppliers As List(Of Supplier) = Me._environment.Suppliers.Where(Function(s) s.ID <> Supplier.SUPPLIER_DEFAULT_ID AndAlso s.ActiveFlag = True).OrderBy(Function(s) s.Name).ToList()

        suppliers.ForEach(Sub(s)

                              Dim buttonSupplier As Button = New Button() With
                                {
                                    .Text = s.Name,
                                    .ID = String.Format("ButtonSelectedDetailsSupplier_{0}", s.ID),
                                    .OnClientClick = String.Format("javascript:SetSupplierIdHiddenField({0}, '{1}', '{2}'); return false;", s.ID, s.Name, .ID),
                                    .CssClass = "ButtonAddMedium"
                                }

                              buttonSupplier.Attributes.Add("style", String.Format("margin: 5px 5px 5px 5px; opacity: {0:0.0} !important;", s.Opacity))

                              buttonSupplier.Attributes.Add("onmouseover", "this.style='opacity: 1.0; margin: 5px 5px 5px 5px;'")
                              buttonSupplier.Attributes.Add("onmouseout", String.Format("this.style='opacity: {0}; margin: 5px 5px 5px 5px;'", s.Opacity))

                              PanelSuppliersList.Controls.Add(buttonSupplier)
                          End Sub)

    End Sub

End Class