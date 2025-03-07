Partial Public Class Reports
    Inherits Environment

    Protected Overloads Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    End Sub

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("userid") <> UserID Then
            Response.Redirect(MHB.BL.URLRewriter.GetLink("MainForm"))
        End If


        Me.CheckAccess()

        FillDropDownListMonth()
        FillDropDownListYear()
        If Not IsPostBack Then
            LabelReportMonth.Text = GetTranslatedValue("month", CurrentLanguage)
            LabelReportYear.Text = GetTranslatedValue("year", CurrentLanguage)
            DropDownListReportYear.SelectedValue = Request.QueryString("year")
            DropDownListReportMonth.SelectedValue = Request.QueryString("month")
        End If

        Me.AddNavigationLink(Me, MHB.BL.URLRewriter.GetLink("Reports"), GetTranslatedValue("reports", CurrentLanguage))

        If Not IsPostBack Then
            SqlDataSource1.SelectCommand = _
"SELECT * FROM [" & MainTable & "] WHERE (([UserID] = @UserID) AND ([Month] = @Month) AND ([Year] = @Year))"
        End If


    End Sub

    Protected Sub DropDownListReportMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DropDownListReportMonth.SelectedIndexChanged
        Response.Redirect(String.Format("{0}?month={1}&year={2}&userid={3}", MHB.BL.URLRewriter.GetLink("Reports"), DropDownListReportMonth.SelectedValue, DropDownListReportYear.SelectedValue, UserID))

    End Sub

    Protected Sub FillDropDownListMonth()
        For i As Integer = 1 To 12
            Dim item As ListItem = New ListItem(GetTranslatedValue("Button" & i.ToString(), CurrentLanguage), i.ToString())
            DropDownListReportMonth.Items.Add(item)
        Next
    End Sub

    Protected Sub FillDropDownListYear()
        For i As Integer = 2000 To 2020
            Dim item As ListItem = New ListItem(i.ToString(), i.ToString())
            DropDownListReportYear.Items.Add(item)
        Next
    End Sub
End Class