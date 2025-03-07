Public Class GeoData

#Region "[ Properties ]"




    Private _ipAddress As String = String.Empty
    Public Property IpAddress As String
        Get
            If String.IsNullOrEmpty(_ipAddress) Then
                _ipAddress = String.Empty
            End If
            Return _ipAddress
        End Get
        Set(value As String)
            _ipAddress = value
        End Set
    End Property

    Private _countryCode As String = String.Empty
    Public Property CountryCode As String
        Get
            If String.IsNullOrEmpty(_countryCode) Then
                _countryCode = String.Empty
            End If
            Return _countryCode
        End Get
        Set(value As String)
            _countryCode = value
        End Set
    End Property

    Private _countryName As String = String.Empty
    Public Property CountryName As String
        Get
            If String.IsNullOrEmpty(_countryName) Then
                _countryName = String.Empty
            End If
            Return _countryName
        End Get
        Set(value As String)
            _countryName = value
        End Set
    End Property

    Private _regionCode As String = String.Empty
    Public Property RegionCode As String
        Get
            If String.IsNullOrEmpty(_regionCode) Then
                _regionCode = String.Empty
            End If
            Return _regionCode
        End Get
        Set(value As String)
            _regionCode = value
        End Set
    End Property


    Private _regionName As String = String.Empty
    Public Property RegionName As String
        Get
            If String.IsNullOrEmpty(_regionName) Then
                _regionName = String.Empty
            End If
            Return _regionName
        End Get
        Set(value As String)
            _regionName = value
        End Set
    End Property

    Private _city As String = String.Empty
    Public Property City As String
        Get
            If String.IsNullOrEmpty(_city) Then
                _city = String.Empty
            End If
            Return _city
        End Get
        Set(value As String)
            _city = value
        End Set
    End Property

    Private _zipCode As String = String.Empty
    Public Property ZipCode As String
        Get
            If String.IsNullOrEmpty(_zipCode) Then
                _zipCode = String.Empty
            End If
            Return _zipCode
        End Get
        Set(value As String)
            _zipCode = value
        End Set
    End Property

    Private _latitude As Double = 0.0
    Public Property Latitude As Double
        Get
            If Not IsNumeric(_latitude) Then
                _latitude = 0.0
            End If
            Return _latitude
        End Get
        Set(value As Double)
            _latitude = value
        End Set
    End Property

    Private _longitude As Double = 0.0
    Public Property Longitude As Double
        Get
            If Not IsNumeric(_longitude) Then
                _longitude = 0.0
            End If
            Return _longitude
        End Get
        Set(value As Double)
            _longitude = value
        End Set
    End Property

#End Region

    Public Sub New()

    End Sub

End Class
