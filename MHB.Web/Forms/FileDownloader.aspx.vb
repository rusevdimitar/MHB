Imports MHB.Web.Environment
Imports System.IO

Public Class FileDownloader
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim filePath As String = String.Empty
        Dim contentType As String = String.Empty

        If Not String.IsNullOrWhiteSpace(Request.QueryString(GlobalVariableNames.FILE_NAME)) Then
            filePath = Request.QueryString(GlobalVariableNames.FILE_NAME)
        ElseIf Not String.IsNullOrWhiteSpace(Session(GlobalVariableNames.FILE_NAME)) Then
            filePath = Session(GlobalVariableNames.FILE_NAME)
        End If

        If Not String.IsNullOrWhiteSpace(Request.QueryString(GlobalVariableNames.CONTENT_TYPE)) Then
            contentType = Request.QueryString(GlobalVariableNames.CONTENT_TYPE)
        ElseIf Not String.IsNullOrWhiteSpace(Session(GlobalVariableNames.CONTENT_TYPE)) Then
            contentType = Request.QueryString(GlobalVariableNames.CONTENT_TYPE)
        End If

        If Not String.IsNullOrEmpty(filePath) Then
            If File.Exists(filePath) Then
                Dim environment As Environment = New Environment()

                If String.IsNullOrEmpty(contentType) Then
                    contentType = HttpHeadersContentType.OCTET
                End If

                environment.SaveFileAs(filePath, Path.GetFileName(filePath), contentType)
            End If
        End If

    End Sub

End Class