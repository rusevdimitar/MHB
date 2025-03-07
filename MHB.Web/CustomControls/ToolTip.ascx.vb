Public Partial Class ToolTip
    Inherits System.Web.UI.UserControl

    Dim ctl As String = ""
    Dim msg As String = ""


    Public Property ControlToToolTip() As String
        Get
            Return ctl
        End Get
        Set(ByVal value As String)
            ctl = value
        End Set
    End Property

    Public Property ToolTipMessage() As String
        Get
            Return msg
        End Get
        Set(ByVal value As String)
            msg = value
        End Set
    End Property

   





    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If TypeOf (Page.Master.FindControl("ContentPlaceHolder1").FindControl(ctl)) Is Button Then
                CType(Page.Master.FindControl("ContentPlaceHolder1").FindControl(ctl), Button).Attributes.Add("onmouseover", "ShowToolTip(event, '" & msg & "')")
                CType(Page.Master.FindControl("ContentPlaceHolder1").FindControl(ctl), Button).Attributes.Add("onmouseout", "HideToolTip()")
            ElseIf TypeOf (Page.Master.FindControl("ContentPlaceHolder1").FindControl(ctl)) Is TextBox Then
                CType(Page.Master.FindControl("ContentPlaceHolder1").FindControl(ctl), TextBox).Attributes.Add("onmouseover", "ShowToolTip(event, '" & msg & "')")
                CType(Page.Master.FindControl("ContentPlaceHolder1").FindControl(ctl), TextBox).Attributes.Add("onmouseout", "HideToolTip()")
            End If
        End If
    End Sub

   



End Class