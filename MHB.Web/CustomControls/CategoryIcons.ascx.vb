Imports System.IO
Imports MHB.BL

Public Class CategoryIcons
    Inherits System.Web.UI.UserControl

    Dim _en As Environment = New Environment()
    Private ReadOnly IconSelectedCommand As String = "IconSelected"
    Private ReadOnly IconDeleteCommand As String = "DeleteIcon"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            LoadIcons()
            LoadUserDefinedIcons()
        End If

    End Sub

    Private Sub LoadIcons()

        Try

            With RepeaterIcons
                .DataSource = _en.CategoryIcons
                .DataBind()
            End With

        Catch ex As Exception
            Logging.Logger.Log(ex, "CategoryIcons.ascx.vb.LoadIcons", String.Empty, _en.UserID, _en.GetConnectionString)
        End Try

    End Sub

    Private Sub LoadUserDefinedIcons()

        Try

            Dim imagesPath As String = Server.MapPath(String.Format("~/Images/UserCategoryIcons/{0}/", _en.UserID))

            If Directory.Exists(imagesPath) Then

                Dim files As List(Of String) = Directory.GetFiles(imagesPath).ToList()
                Dim dataSource As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)()

                Dim categories As IEnumerable(Of Category) = Me._en.Categories

                For Each img As String In files
                    img = img.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    img = img.Substring(img.IndexOf("Images"))
                    img = String.Format("../{0}", img)

                    Dim file As String = img

                    Dim usesCount As Integer = (From category As MHB.BL.Category In categories Where Path.GetFileName(category.IconPath) = Path.GetFileName(file) AndAlso category.UserID <> _en.UserID Select category).Count()

                    dataSource.Add(file, usesCount)
                Next

                With RepeaterUserIcons
                    .DataSource = dataSource
                    .DataBind()
                End With

            End If
        Catch ex As Exception
            Logging.Logger.Log(ex, "LoadUserDefinedIcons.ascx.vb.LoadIcons", String.Empty, _en.UserID, _en.GetConnectionString)
        End Try

    End Sub

    Protected Sub RepeaterIcons_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RepeaterIcons.ItemCommand, RepeaterUserIcons.ItemCommand

        Dim iconPath As String = String.Empty

        If Not String.IsNullOrEmpty(e.CommandArgument) Then
            iconPath = e.CommandArgument
        End If

        Select Case e.CommandName
            Case IconSelectedCommand
                RaiseEvent IconSelected(iconPath, EventArgs.Empty)
                Exit Select

            Case IconDeleteCommand

                Dim fileName As String = Server.MapPath(iconPath)

                File.Delete(fileName)

                ReloadIcons()

                Exit Select
        End Select

    End Sub

    Public Sub ReloadIcons()
        LoadIcons()
        LoadUserDefinedIcons()
    End Sub

    Public Event IconSelected As EventHandler

End Class