Imports System.Net
Imports System.Text
Imports System.Xml
Imports System.Configuration
Imports System.Text.RegularExpressions

Public Class GeoLocation

    Private _helper As Helper

    Private _geoIpServiceUrls As String() = New String() {}
    Public Property GeoIpServiceUrls() As String()
        Get
            Return _geoIpServiceUrls
        End Get
        Set(ByVal value As String())
            _geoIpServiceUrls = value
        End Set
    End Property

    Private _geoIpServiceNodeName As String = String.Empty
    Public Property GeoIpServiceNodeName() As String
        Get
            If String.IsNullOrEmpty(_geoIpServiceNodeName) Then
                _geoIpServiceNodeName = String.Empty
            End If
            Return _geoIpServiceNodeName
        End Get
        Set(ByVal value As String)
            _geoIpServiceNodeName = value
        End Set
    End Property

    Public Sub New()

        _helper = New Helper()

        If String.IsNullOrEmpty(ConfigurationManager.AppSettings("GeoIPServiceFormattedUrls")) Then
            _geoIpServiceUrls = New String() {"http://ip-api.com/xml/{0}", "http://freegeoip.net/xml/{0}"}
        Else
            _geoIpServiceUrls = ConfigurationManager.AppSettings("GeoIPServiceFormattedUrls").Split(",".ToCharArray())
        End If

        If String.IsNullOrEmpty(ConfigurationManager.AppSettings("GeoIPServiceNodeName")) Then
            _geoIpServiceNodeName = "CountryCode"
        Else
            _geoIpServiceNodeName = ConfigurationManager.AppSettings("GeoIPServiceNodeName")
        End If
    End Sub

    Public Sub New(ByVal geoIpServiceUrls As String(), ByVal geoIpServiceNodeName As String)

        _helper = New Helper()

        _geoIpServiceUrls = geoIpServiceUrls

        _geoIpServiceNodeName = geoIpServiceNodeName

    End Sub

    Public Function GetUserLanguageCode(ByVal ipAddress As String) As String

        Dim languageCode As String = String.Empty

        Try

            Dim result As String = String.Empty

            For Each serviceUrl As String In Me._geoIpServiceUrls

                Dim url As String = String.Format(serviceUrl, ipAddress)

                Dim data() As Byte = New WebClient().DownloadData(url)

                result = Encoding.ASCII.GetString(data)

                Dim doc As XmlDocument = New XmlDocument()

                doc.LoadXml(result)

                languageCode = _helper.GetXmlNodeValue(doc.ChildNodes, _geoIpServiceNodeName)

                If Not String.IsNullOrEmpty(languageCode) Then
                    Return languageCode
                End If

            Next

        Catch ex As Exception
            Throw New Exception(String.Format("GetUserLanguageCode:{0}", ex.Message), ex)
        End Try

        Return languageCode

    End Function

    Public Function GetGeoData(ByVal ipAddress As String) As GeoData

        Try
            Dim geoData As GeoData = New GeoData()

            Dim result As String = String.Empty

            Dim languageCode As String = String.Empty

            For Each serviceUrl As String In Me._geoIpServiceUrls

                Dim url As String = String.Format(serviceUrl, ipAddress)

                Dim data() As Byte = New WebClient().DownloadData(url)

                result = Encoding.ASCII.GetString(data)

                Dim doc As XmlDocument = New XmlDocument()

                doc.LoadXml(result)

                If doc.DocumentElement.SelectSingleNode("Ip | query") IsNot Nothing Then
                    geoData.IpAddress = doc.DocumentElement.SelectSingleNode("Ip | query").InnerText
                End If

                If doc.DocumentElement.SelectSingleNode("CountryCode | countryCode") IsNot Nothing Then
                    geoData.CountryCode = doc.DocumentElement.SelectSingleNode("CountryCode | countryCode").InnerText
                End If

                If doc.DocumentElement.SelectSingleNode("CountryName | country") IsNot Nothing Then
                    geoData.CountryName = doc.DocumentElement.SelectSingleNode("CountryName | country").InnerText
                End If

                If doc.DocumentElement.SelectSingleNode("RegionCode | region") IsNot Nothing Then
                    geoData.RegionCode = doc.DocumentElement.SelectSingleNode("RegionCode | region").InnerText
                End If

                If doc.DocumentElement.SelectSingleNode("RegionName | regionName") IsNot Nothing Then
                    geoData.RegionName = doc.DocumentElement.SelectSingleNode("RegionName | regionName").InnerText
                End If

                If doc.DocumentElement.SelectSingleNode("City | city") IsNot Nothing Then
                    geoData.City = doc.DocumentElement.SelectSingleNode("City | city").InnerText
                End If

                If doc.DocumentElement.SelectSingleNode("ZipCode | zip") IsNot Nothing Then
                    geoData.ZipCode = doc.DocumentElement.SelectSingleNode("ZipCode | zip").InnerText
                End If

                If doc.DocumentElement.SelectSingleNode("Latitude | lat") IsNot Nothing Then
                    geoData.Latitude = CDec(doc.DocumentElement.SelectSingleNode("Latitude | lat").InnerText)
                End If

                If doc.DocumentElement.SelectSingleNode("Longitude | lon") IsNot Nothing Then
                    geoData.Longitude = CDec(doc.DocumentElement.SelectSingleNode("Longitude | lon").InnerText)
                End If

                If String.IsNullOrEmpty(geoData.City) OrElse String.IsNullOrEmpty(geoData.CountryCode) Then
                    Continue For
                End If

                Return geoData

            Next

            Return geoData

        Catch ex As Exception
            'Throw New Exception(String.Format("GetGeoData:{0}", ex.Message), ex)
        End Try

    End Function

End Class