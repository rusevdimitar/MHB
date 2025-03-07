Imports System.Data.SqlClient
Imports MHB.DAL

Partial Public Class PreviewAttachment
    Inherits Environment

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Me.CheckAccess()

            Dim qry As String = String.Empty

            If CBool(Request.QueryString("attachingToDetails")) Then
                qry = _
"SELECT [Attachment], [AttachmentFileType], [HasAttachment] FROM [dbo].[" & DetailsTable & "] WHERE ID = " & Request.QueryString("id")
            Else
                qry = _
"SELECT [Attachment], [AttachmentFileType], [HasAttachment] FROM [dbo].[" & MainTable & "] WHERE ID = " & Request.QueryString("id")
            End If

            Dim reader As IDataReader

            reader = DataBaseConnector.GetDataReader(qry, GetConnectionString)
            reader.Read()

            If Not reader.IsDBNull(0) AndAlso Not reader.IsDBNull(1) AndAlso Not reader.IsDBNull(2) Then

                Dim bmp As System.Drawing.Bitmap = New System.Drawing.Bitmap(Context.Server.MapPath("..\Images\NoImage.gif"))
                Dim tempStream As System.IO.MemoryStream = New System.IO.MemoryStream()
                bmp.Save(tempStream, System.Drawing.Imaging.ImageFormat.Jpeg)

                Dim fileTypes(4) As String
                fileTypes(0) = "JPG"
                fileTypes(1) = "GIF"
                fileTypes(2) = "PNG"
                fileTypes(3) = "BMP"
                fileTypes(4) = "JPEG"

                Dim isImgFile As Boolean = False

                For i As Integer = 0 To fileTypes.Length - 1
                    If reader("AttachmentFileType").ToString().ToUpper().Contains(fileTypes(i)) Then
                        isImgFile = True
                        Exit For
                    End If
                Next

                If isImgFile Then
                    Response.AddHeader("Content-Disposition", "inline; filename=Doc1" & reader("AttachmentFileType"))
                    Response.ContentType = reader("AttachmentFileType").ToString().Replace(".", "")
                    Response.BinaryWrite(reader("Attachment"))
                Else
                    Response.AddHeader("Content-Disposition", "inline; filename=Doc1.jpg")
                    Response.ContentType = "jpg"
                    Response.BinaryWrite(tempStream.ToArray())
                End If

            End If
            reader.Close()

        Catch ex As Exception
            Throw New Exception("PreviewAttachment.aspx: " & ex.Message, ex)
        End Try
    End Sub

End Class