Imports System.Data
Imports System.Drawing.Color
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Security.Cryptography
Imports System.Net
Imports System.Drawing.Text
Imports System.Drawing.Drawing2D
Public Class wscaimage
    Inherits System.Web.UI.Page

    Sub PageReset_Click(ByVal Sender As Object, ByVal E As EventArgs)
    End Sub

    Sub Page_Init(Sender As Object, E As EventArgs)

    End Sub

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        If Not (IsPostBack) Then
            ' GetRandomAlphaNumeric()
            'GenerateImage(Session("WSRX"))
            GenerateImage("cowp3nsw")
            'If random_number.Text <> "" Then
            '  GenImage(random_number.Text)
            'End If
        End If

    End Sub

    ' Function generateImage(ByVal sTextToImg As String) As System.Drawing.Bitmap
    Sub OldgenerateImage(ByVal sTextToImg As String)

        Response.Clear()
        Dim height As Integer = 100
        Dim width As Integer = 200
        Dim r As New Random
        Dim x As Integer = r.Next(75)
        Dim RandY1 As Integer
        Dim RandY2 As Integer
        Dim Rand As Random
        Dim iNum As Integer
        Dim hatchbrush As HatchBrush
        Dim rect As Rectangle

        'LockBits
        'Dim bmp As New Bitmap(width, height, PixelFormat.Format24bppRgb)
        Dim bmp As New Bitmap(width, height, PixelFormat.Format32bppRgb)
        Dim g As Graphics = Graphics.FromImage(bmp)

        g.TextRenderingHint = TextRenderingHint.AntiAlias

        g.Clear(Color.LightGray)
        hatchbrush = New HatchBrush(HatchStyle.Percent90, Color.LightGray, Color.White)
        rect = New Rectangle(0, 0, width, height)
        g.FillRectangle(hatchbrush, rect)
        g.DrawRectangle(Pens.White, 1, 1, width - 3, height - 3)
        g.DrawRectangle(Pens.Gray, 2, 2, width - 3, height - 3)
        g.DrawRectangle(Pens.Black, 0, 0, width, height)

        g.DrawString(sTextToImg,
          New Font("Arial", 12, FontStyle.Italic),
          SystemBrushes.WindowText, New PointF(x, 50))

        'Add two Random Lines
        Rand = New Random()
        iNum = Rand.Next(10000, 99999)
        ' Create random numbers for the first line 
        RandY1 = Rand.Next(0, 50)
        RandY2 = Rand.Next(0, 100)
        'Draw the first line 
        g.DrawLine(Pens.Maroon, 0, RandY1, 200, RandY2)
        'Create random numbers for the second lineRand
        RandY1 = Rand.Next(0, 50)
        RandY2 = Rand.Next(0, 100)
        ' Draw the second line
        g.DrawLine(Pens.Black, 0, RandY1, 200, RandY2)

        bmp.Save(Response.OutputStream, ImageFormat.Jpeg)
        g.Dispose()
        bmp.Dispose()
        'Response.End()

        'generateImage = bmpImage

    End Sub

    ' ====================================================================
    ' Creates the bitmap image.
    ' ====================================================================
    Sub GenerateImage(ByVal sTextToImg As String)
        Dim my_height As Integer = 100
        Dim my_width As Integer = 250
        'Dim my_familyName As String = "Century Schoolbook"
        Dim my_familyName As String = "Arial"
        Dim Random As Random
        Dim Rand As Random
        Dim iNum As Integer
        Dim RandY1 As Integer
        Dim RandY2 As Integer
        'Response.Write(sTextToImg)
        Response.Clear()
        ' Create a new 32-bit bitmap image.
        Dim bitmap As Bitmap = New Bitmap(my_width, my_height, PixelFormat.Format32bppRgb)

        ' Create a graphics object for drawing.
        Dim g As Graphics = Graphics.FromImage(bitmap)
        g.TextRenderingHint = TextRenderingHint.AntiAlias

        g.Clear(Color.LightGray)
        g.SmoothingMode = SmoothingMode.AntiAlias
        Dim rect As Rectangle = New Rectangle(0, 0, my_width, my_height)

        ' Fill in the background.
        Dim hatchBrush As HatchBrush = New HatchBrush(HatchStyle.Percent90, Color.LightGray, Color.White)
        g.FillRectangle(hatchBrush, rect)

        'Draw the border
        g.DrawRectangle(SystemPens.ControlDark, rect)

        ' Set up the text font.
        Dim size As SizeF
        Dim fontSize As Single = rect.Height + 1
        Dim font As Font
        ' Adjust the font size until the text fits within the image.
        Do
            fontSize = fontSize - 1
            font = New Font(my_familyName, fontSize, FontStyle.Bold)
            size = g.MeasureString(sTextToImg, font)
        Loop While (size.Width > rect.Width)

        ' Set up the text format.
        Dim format As StringFormat = New StringFormat()
        format.Alignment = StringAlignment.Center
        format.LineAlignment = StringAlignment.Center

        ' Create a path using the text and warp it randomly.
        Random = New Random()
        Dim path As GraphicsPath = New GraphicsPath()
        path.AddString(sTextToImg, font.FontFamily, font.Style, font.Size, rect, format)
        Dim v As Single = 4.0F
        Dim points() As PointF = {
        New PointF(Random.Next(rect.Width) / v, Random.Next(rect.Height) / v), New PointF(rect.Width - Random.Next(rect.Width) / v, Random.Next(rect.Height) / v),
        New PointF(Random.Next(rect.Width) / v, rect.Height - Random.Next(rect.Height) / v),
        New PointF(rect.Width - Random.Next(rect.Width) / v, rect.Height - Random.Next(rect.Height) / v)
        }
        Dim matrix As Matrix = New Matrix
        matrix.Translate(0.0F, 0.0F)
        path.Warp(points, rect, matrix, WarpMode.Perspective, 0.0F)

        ' Draw the text.
        'hatchBrush = New HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray)
        'Foreground, Background
        hatchBrush = New HatchBrush(HatchStyle.LargeConfetti, Color.Black, Color.DarkGray)
        g.FillPath(hatchBrush, path)

        ' Add some random noise.
        Dim m As Integer = Math.Max(rect.Width, rect.Height)
        For i As Integer = 0 To i < (rect.Width * rect.Height / 30.0F) Step 1
            Dim x As Integer = Random.Next(rect.Height)
            Dim y As Integer = Random.Next(rect.Height)
            Dim w As Integer = Random.Next(m / 40)
            Dim h As Integer = Random.Next(m / 40)
            g.FillEllipse(hatchBrush, x, y, w, h)
        Next

        'Add two Random Lines
        Rand = New Random()
        iNum = Rand.Next(10000, 99999)
        ' Create random numbers for the first line 
        RandY1 = Rand.Next(0, 50)
        RandY2 = Rand.Next(0, 100)
        'Draw the first line 
        g.DrawLine(Pens.Maroon, 0, RandY1, 250, RandY2)
        'Create random numbers for the second lineRand
        RandY1 = Rand.Next(0, 50)
        RandY2 = Rand.Next(0, 100)
        ' Draw the second line
        g.DrawLine(Pens.Black, 0, RandY1, 250, RandY2)

        bitmap.Save(Response.OutputStream, ImageFormat.Jpeg)

        ' Clean up
        font.Dispose()
        hatchBrush.Dispose()
        bitmap.Dispose()
        g.Dispose()

        ' Set the image.
        'my_image = bitmap
    End Sub

End Class