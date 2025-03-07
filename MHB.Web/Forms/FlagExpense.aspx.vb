Public Class FlagExpense
    Inherits Environment

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim id As Integer = Request.QueryString("id")

        Dim result As String = String.Empty

        result = "<table>"

        For i As Integer = 0 To id Step 1

            result &= String.Format( _
                "<tr><td> This is row number #{0}</td></tr>", i)


        Next

        result &= "</table>"

        Response.Write(result)

    End Sub


    'Protected Function FlagExpense() As String

    '    Dim qry As String = String.Empty
    '    Dim ID As Integer = 0

    '    If IsNumeric(Request.QueryString("id")) Then
    '        ID = Request.QueryString("id")
    '    End If


    '    qry = String.Format("UPDATE tbMainTable01 SET Flagged = 0 WHERE ID = 521", ID)

    '    DatabaseConnector.ExecuteQuery(qry, GetConnectionString)

    '    Dim result As String = String.Empty

    '    result = "<table>"

    '    For i As Integer = 0 To 10 Step 1

    '        result &= String.Format( _
    '            "<tr><td> This is row number #{0}</td></tr>", i)


    '    Next

    '    result &= "</table>"

    'End Function
End Class