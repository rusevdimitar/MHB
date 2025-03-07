Imports System.Data.SqlClient
Imports System.IO
Imports MHB.DAL

Partial Public Class DownloadAttachment
    Inherits Environment

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim qry As String = String.Empty

        Try

            If IsNumeric(Request.QueryString("id")) Then

                Dim attachingToDetails As Boolean = False

                Boolean.TryParse(Request.QueryString("attachingToDetails"), attachingToDetails)

                Dim recordId As Integer = CInt(Request.QueryString("id"))

                qry = String.Format( _
"SELECT Attachment, AttachmentFileType, IsShared FROM dbo.{0} WHERE ID = @id", IIf(attachingToDetails, Me.DetailsTable, Me.MainTable))

                Dim parID As SqlParameter = New SqlParameter("@id", recordId)

                Using reader As IDataReader = DataBaseConnector.GetDataReader(qry, Me.GetConnectionString, parID)

                    While reader.Read()

                        Dim isShared As Boolean

                        If Not reader.IsDBNull(reader.GetOrdinal("IsShared")) AndAlso Not Boolean.TryParse(reader("IsShared"), isShared) Then
                            Me.CheckAccess()
                        End If

                        If Not reader.IsDBNull(reader.GetOrdinal("AttachmentFileType")) AndAlso Not reader.IsDBNull(reader.GetOrdinal("Attachment")) Then

                            Dim attachmentFileType As String = reader("AttachmentFileType")

                            Dim fileBytes As Byte() = CType(reader("Attachment"), Byte())

                            With Response

                                .AddHeader("Content-Disposition", String.Format("inline; filename=Attachment{0}", attachmentFileType))
                                .ContentType = attachmentFileType.Replace(".", String.Empty)
                                .BinaryWrite(fileBytes)

                            End With
                        End If

                    End While

                End Using

            End If

        Catch ex As Exception
            Logging.Logger.Log(ex, "DownloadAttachment.aspx > Page_Load()", qry, Me.UserID, Me.GetConnectionString)
        End Try
    End Sub

End Class