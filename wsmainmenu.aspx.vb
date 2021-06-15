Imports System.Data
Imports System.IO
Imports System.Net
Imports System.DateTime
Public Class wsmainmenu
    Inherits System.Web.UI.Page

    '02/11/2021 BAN Created
    Public Function ZeroPad(ByVal datain As String) As String
        Dim dataout As String
        If Len(datain) < 2 Then
            dataout = "0" & datain
        Else
            dataout = datain
        End If

        Return dataout
    End Function

    Public Function getDate() As String
        Dim mm As String
        Dim dd As String
        Dim yyyy As String
        Dim mydata As String
        Dim returndate As String

        mydata = Date.Now.Month
        mm = ZeroPad(mydata)

        mydata = Date.Now.Day
        dd = ZeroPad(mydata)

        yyyy = Date.Now.Year

        returndate = mm & "/" & dd & "/" & yyyy
        Return returndate
    End Function

    Public Function getTime() As String
        Dim hour As String
        Dim minute As String
        Dim seconds As String
        Dim mydata As String
        Dim returntime As String

        mydata = Date.Now.Hour
        hour = ZeroPad(mydata)

        mydata = Date.Now.Minute
        minute = ZeroPad(mydata)

        mydata = Date.Now.Second
        seconds = ZeroPad(mydata)

        returntime = hour & ":" & minute & ":" & seconds
        Return returntime
    End Function

    Sub GetDateTime()
        Datetb.Text = getDate()
        'Response.Write(getDate())

        Timetb.Text = getTime()
        ' Response.Write(getDate())
    End Sub

    'Sub Session_Start()
    '    Session.Timeout = 525500 ' Minutes (1 year)
    'End Sub

    Sub Page_Init(Sender As Object, E As EventArgs)
        'Response.Write("Page_Init " & Session("UserID"))
        ' Response.Write("Page_Init " & Session("LoggedIn"))

        If Session("LoggedIn") = "No" Or Session("UserID") = "" Then
            Response.Write("<B><FONT COLOR=red>Your session timed out due to inactivity or you have not logged in originally. You must re-login or login.</FONT></B>")
            Session.Abandon()
            Server.Transfer("wslogin.aspx")
            'CloseWindow()
            Exit Sub
        End If
    End Sub

    Sub Page_Load(Sender As Object, E As EventArgs)
        ' Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'Response.ExpiresAbsolute = DateTime.Now.AddMonths(-1)
        'Try
        '    UserIDtb.Text = Session("UserID")
        '    ' Response.Write("Page_Load " & UserIDtb.Text)
        '    ' Response.Write("Page_Load " & Session("LoggedIn"))
        'Catch ex As NullReferenceException
        '    UserIDtb.Text = "ERROR"
        '    ' Response.Write(ex.Message + " Session doesn't exist")
        'End Try
        UserIDtb.Text = Session("UserID")
        'UserIDtb.Text = "TESTING"
        'Response.Write("Page_Load " & UserIDtb.Text)
        'Response.Write("Page_Load " & Session("LoggedIn"))

        ' Response.Write("B " & Session("UserID"))

        If Not (IsPostBack) Then
            ' Response.Write("C " & Session("UserID"))
            GetDateTime()

            CreateMenus()
        Else
            'Response.Write("D " & Session("UserID"))
        End If
    End Sub

    Sub CreateMenus()
        Dim j As Integer

        j = 0
        '-------------- View Menu1 -----------------------------------
        'Response.Write("1")
        Menu1.Items(0).Enabled = True
        ' Menu1.Items.Add(mipartinfo)

        Dim miviewuserappperm As New MenuItem
        miviewuserappperm.Text = "View User App Perms"
        miviewuserappperm.Value = "View User App Perms"
        miviewuserappperm.NavigateUrl = "wsviewuserapppermission.aspx"
        miviewuserappperm.Target = "_self"
        Menu1.Items(0).ChildItems.Add(miviewuserappperm)
        j = j + 1

        Dim miviewsndata As New MenuItem
        miviewsndata.Text = "View RX SN Data"
        miviewsndata.Value = "View RX SN Data"
        miviewsndata.NavigateUrl = "rxviewserialnumberdata.aspx"
        miviewsndata.Target = "_self"
        Menu1.Items(0).ChildItems.Add(miviewsndata)
        j = j + 1

        Dim miviewcustdata As New MenuItem
        miviewcustdata.Text = "View RX Cust Data"
        miviewcustdata.Value = "View RX Cust Data"
        miviewcustdata.NavigateUrl = "rxviewcustomerdata.aspx"
        miviewcustdata.Target = "_self"
        Menu1.Items(0).ChildItems.Add(miviewcustdata)
        j = j + 1

        '----------Setup Menu2 ---------------------------
        Menu2.Items(0).Enabled = True
        Menu2.Visible = True

        'Dim milm As New MenuItem
        'milm.Text = "Location Master"
        'milm.Value = "Location Master"
        'milm.NavigateUrl = "wslocationmastermaint.aspx"
        'milm.Target = "_self"
        'Menu2.Items(0).ChildItems.Add(milm)
        'j = j + 1

        Dim mium As New MenuItem
        mium.Text = "User Master"
        mium.Value = "User Master"
        mium.NavigateUrl = "wsusermastermaint.aspx"
        mium.Target = "_self"
        Menu2.Items(0).ChildItems.Add(mium)
        j = j + 1

        Dim miam As New MenuItem
        miam.Text = "Application Master"
        miam.Value = "Application Master"
        miam.NavigateUrl = "wsapplicationmastermaint.aspx"
        miam.Target = "_self"
        Menu2.Items(0).ChildItems.Add(miam)
        j = j + 1

        Dim migm As New MenuItem
        migm.Text = "Group Master"
        migm.Value = "Group Master"
        migm.NavigateUrl = "wsgroupmastermaint.aspx"
        migm.Target = "_self"
        Menu2.Items(0).ChildItems.Add(migm)
        j = j + 1

        Dim migu As New MenuItem
        migu.Text = "Group User"
        migu.Value = "Group User"
        migu.NavigateUrl = "wsgroupusermaint.aspx"
        migu.Target = "_self"
        Menu2.Items(0).ChildItems.Add(migu)
        j = j + 1

        Dim miaa As New MenuItem
        miaa.Text = "Application Area"
        miaa.Value = "Application Area"
        miaa.NavigateUrl = "wsapplicationareamaint.aspx"
        miaa.Target = "_self"
        Menu2.Items(0).ChildItems.Add(miaa)
        j = j + 1

        Dim miga As New MenuItem
        miga.Text = "Group Application Area"
        miga.Value = "Group Application Area"
        miga.NavigateUrl = "wsgroupapplicationarea.aspx"
        miga.Target = "_self"
        Menu2.Items(0).ChildItems.Add(miga)
        j = j + 1

        Dim micopy As New MenuItem
        micopy.Text = "Copy User Security"
        micopy.Value = "Copy User Security"
        micopy.NavigateUrl = "wscopyusersecurity.aspx"
        micopy.Target = "_self"
        Menu2.Items(0).ChildItems.Add(micopy)
        j = j + 1

        Dim michange As New MenuItem
        michange.Text = "Change User Password"
        michange.Value = "Change User Password"
        michange.NavigateUrl = "wschangeuserpassword.aspx"
        michange.Target = "_self"
        Menu2.Items(0).ChildItems.Add(michange)
        j = j + 1

    End Sub

    Sub PageReset_Click(Sender As Object, E As EventArgs)
        'GetDateTime
    End Sub

    Sub CloseWindow()
        Dim s As String
        s = "<SCRIPT language='javascript'>window.close() </"
        s = s & "SCRIPT>"
        'RegisterStartupScript("closewindow", s)
        If Not (ClientScript.IsStartupScriptRegistered("closewindow")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "closewindow", s.ToString(), False)
        End If
    End Sub

    Sub Logout(Sender As Object, E As EventArgs)
        Session.Abandon()
        Server.Transfer("wslogin.aspx")
        'Dim s as string
        's = "<SCRIPT language='javascript'>window.close() </" 
        's = s & "SCRIPT>"
        'RegisterStartupScript("closewindow", s)
    End Sub

End Class