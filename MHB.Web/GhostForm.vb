Imports System
Imports System.Web.UI
Imports System.Web.UI.HtmlControls


''' <summary>
''' This is a special form that can _not_ render the actual form tag, but always render the contents
''' </summary>
''' <remarks></remarks>
Public Class GhostForm
    Inherits System.Web.UI.HtmlControls.HtmlForm

    Private _render As Boolean
    Public Property RenderFormTag() As Boolean
        Get
            Return _render
        End Get
        Set(ByVal value As Boolean)
            _render = value
        End Set
    End Property

    Public Sub New()
        _render = True
    End Sub

    Protected Overrides Sub RenderEndTag(ByVal writer As System.Web.UI.HtmlTextWriter)
        If (_render) Then
            MyBase.RenderEndTag(writer)
        End If
    End Sub

    Protected Overrides Sub RenderBeginTag(ByVal writer As System.Web.UI.HtmlTextWriter)
        If (_render) Then
            MyBase.RenderBeginTag(writer)
        End If
    End Sub

End Class


