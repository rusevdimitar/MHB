Imports System.Drawing

Public Class CaptchaOLD
    Inherits Environment





    ''' <summary>
    ''' Generates an image showing the given text
    ''' </summary>
    ''' <param name="text">The text to be shown in the image</param>
    ''' <returns></returns>
    Public Shared Function DrawText(ByVal text As String) As Bitmap

        Try

            ' we create a new bitmap that will hold the captcha image
            Dim b As Bitmap = New Bitmap(1, 1)

            ' declare a new font with a size of 24
            Dim f As Font = New Font("Verdana", 24)

            Dim graphics As Graphics = graphics.FromImage(b)

            ' we measure the length and height of the captcha text and set the Bitmap width and heigth attributes
            Dim width As Integer = CInt(graphics.MeasureString(text, f).Width)
            Dim height As Integer = CInt(graphics.MeasureString(text, f).Height)

            b = New Bitmap(b, New Size(width, height))

            graphics = graphics.FromImage(b)
            graphics.Clear(Color.DarkBlue)

            Dim rnd As Random = New Random()

            Dim points(60) As Point

            ' we create a random array of Point objects,
            'we will later use to draw some Beziers to blur things a bit
            For i As Integer = 0 To points.Length - 1
                points(i) = New Point(CInt(rnd.Next(width)), CInt(rnd.Next(height)))
            Next


            ' we now declare several pens to draw some lines accross, that will mask the captcha text
            Dim pen0 As Pen = New Pen(Color.PaleVioletRed)
            pen0.Width = 2
            pen0.DashStyle = Drawing2D.DashStyle.Dot

            Dim pen1 As Pen = New Pen(Color.OldLace)
            pen1.Width = 3
            pen1.DashStyle = Drawing2D.DashStyle.Dash

            Dim pen2 As Pen = New Pen(Color.PaleGreen)
            pen2.Width = 4
            pen2.DashStyle = Drawing2D.DashStyle.Dash

            Dim pen3 As Pen = New Pen(Color.PaleGreen)
            pen3.Width = 4
            pen3.DashStyle = Drawing2D.DashStyle.Dash

            Dim pen4 As Pen = New Pen(Color.PaleGreen)
            pen4.Width = 4
            pen4.DashStyle = Drawing2D.DashStyle.DashDotDot

            graphics.DrawLine(pen1, 0, 1, height, width)
            graphics.DrawLine(pen2, 0, 1, width, height)
            graphics.DrawLine(pen3, 5, 1, rnd.Next(height), rnd.Next(width))
            graphics.DrawLine(pen4, 1, 5, rnd.Next(height), rnd.Next(width))
            graphics.DrawBeziers(pen0, points)
            graphics.DrawString(text, f, New SolidBrush(Color.White), 0, 0)
            graphics.Flush()

            Return b
        Catch ex As Exception
            Throw (New Exception("Captcha.vb > DrawText()", ex))
        End Try

    End Function

End Class
