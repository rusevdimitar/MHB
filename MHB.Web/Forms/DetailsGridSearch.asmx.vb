Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports MHB.BL
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class DetailsGridSearch
    Inherits System.Web.Services.WebService

    Dim _en As Environment

    Public Sub New()
        Me._en = New Environment()
    End Sub

    Public ReadOnly Property ConnectionString As String
        Get
            Return ConfigurationManager.ConnectionStrings("connectionString").ToString()
        End Get
    End Property

    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function SearchDetails(ByVal input As String) As String

        If Me._en.DetailsDataSource Is Nothing Then
            Return String.Empty
        End If

        Dim results As IEnumerable(Of ExpenditureDetailInfo) = Enumerable.Empty(Of ExpenditureDetailInfo)

        Try

            Dim transliteratedText As Tuple(Of String, String) = Me._en.TransliterateText(input)

            Dim originalPRefixText As String = transliteratedText.Item1
            Dim transliteratedPrefixText As String = transliteratedText.Item2

            results = Me._en.DetailsDataSource.Where(Function(d) d.DetailName.ToLower().Contains(originalPRefixText.ToLower()) _
                                                         OrElse d.DetailName.ToLower().Contains(transliteratedPrefixText.ToLower()) _
                                                         OrElse d.DetailDescription.ToLower().Contains(originalPRefixText.ToLower()) _
                                                         OrElse d.DetailDescription.ToLower().Contains(transliteratedPrefixText.ToLower())) _
                                                         .Select(Function(d) New ExpenditureDetailInfo() With {.ID = d.ID, .Name = d.DetailName})

        Catch ex As Exception
            Logging.Logger.Log(ex, "MHB.Web.Forms.DetailsGridSearch.asmx.vb.SearchDetails", String.Empty, 0, Me.ConnectionString)
        End Try

        Return New JavaScriptSerializer().Serialize(results)

    End Function

End Class