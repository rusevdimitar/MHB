Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports MHB.BL
Imports System.Linq

Imports System.Web.Script.Services
Imports System.Web.Script.Serialization

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class ProductAutoComplete
    Inherits System.Web.Services.WebService

    Dim _environment As Environment

    Public ReadOnly Property ConnectionString As String
        Get
            Return ConfigurationManager.ConnectionStrings("connectionString").ToString()
        End Get
    End Property

    Public Sub New()

        Me._environment = New Environment()
    End Sub

    <WebMethod(EnableSession:=True)>
    Public Function GetProducts(ByVal prefixText As String, ByVal count As Integer) As List(Of String)

        Dim items As List(Of String) = New List(Of String)()

        Dim products As Product()

        Try

            Dim transliteratedText As Tuple(Of String, String) = Me._environment.TransliterateText(prefixText)

            Dim originalPRefixText As String = transliteratedText.Item1
            Dim transliteratedPrefixText As String = transliteratedText.Item2

            products = Me._environment.Products.Where(Function(product) product.KeyWords.Any(Function(keyWord) _
                                                                                   keyWord.ToLower().Contains(originalPRefixText.ToLower()) OrElse
                                                                                   keyWord.ToLower().Contains(transliteratedPrefixText.ToLower())
                                                                                   )).OrderByDescending(Function(product) product.Priority).ToArray()

            For Each p As Product In products
                For Each k As String In p.KeyWords
                    items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(k.Trim(), p.ID))
                Next
            Next

        Catch ex As Exception
            Logging.Logger.Log(ex, "MHB.Web.Forms.ProductAutoComplete.asmx.vb.GetProducts", String.Empty, 0, Me.ConnectionString)
        Finally
            products = Nothing
        End Try

        Return items

    End Function

    <WebMethod(EnableSession:=True)>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function GetProductsJSON(ByVal prefixText As String) As String

        Dim products As ProductInfo() = New ProductInfo() {}

        Try

            Dim transliteratedText As Tuple(Of String, String) = Me._environment.TransliterateText(prefixText)

            Dim originalPRefixText As String = transliteratedText.Item1
            Dim transliteratedPrefixText As String = transliteratedText.Item2

            products = Me._environment.Products.Where(Function(product) product.KeyWords.Any(Function(keyWord) _
                                                                                   keyWord.ToLower().Contains(originalPRefixText.ToLower()) OrElse
                                                                                   keyWord.ToLower().Contains(transliteratedPrefixText.ToLower())
                                                                                   )).OrderByDescending(Function(product) product.Priority) _
                                                                           .Select(Function(p) New ProductInfo() With {.ID = p.ID, .Name = p.Name}).ToArray()

        Catch ex As Exception
            Logging.Logger.Log(ex, "MHB.Web.Forms.ProductAutoComplete.asmx.vb.GetProducts", String.Empty, 0, Me.ConnectionString)
        End Try

        Return New JavaScriptSerializer().Serialize(products)

    End Function

End Class