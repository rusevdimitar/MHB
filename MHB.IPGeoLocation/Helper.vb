Imports System.Xml
Imports System.Configuration

Public Class Helper

    Private nodeValue As String = String.Empty

    Public Function GetXmlNodeValue(ByVal nodes As XmlNodeList, ByVal nodeName As String) As String

        For Each node As XmlNode In nodes

            If node.HasChildNodes Then
                Call GetXmlNodeValue(node.ChildNodes, nodeName)
            End If

            If node.Name = nodeName Then

                If Not String.IsNullOrEmpty(node.InnerText) Then
                    nodeValue = node.InnerText
                    Exit For
                End If

            End If
        Next

        If String.IsNullOrEmpty(nodeValue) Then
            Return String.Empty
        Else
            Return nodeValue
        End If

    End Function


End Class
