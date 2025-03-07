Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Linq

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SearchAutoComplete
    Inherits System.Web.Services.WebService

    Dim _environment As Environment

    Public ReadOnly Property ConnectionString
        Get
            Return ConfigurationManager.ConnectionStrings("connectionString").ToString()
        End Get
    End Property


    Public Sub New()
        Me._environment = New Environment()
    End Sub

    <WebMethod(EnableSession:=True)> _
    Public Function GetSearchKeywords(ByVal prefixText As String, ByVal count As Integer) As String()

        Return Me._environment.SearchKeywords.Value.Where(Function(k) k.ToUpper().Contains(prefixText.ToUpper())).Select(Function(k) AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(k.Trim(), k)).ToArray()

    End Function

End Class