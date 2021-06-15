Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Random
Imports System.Security.Cryptography
Imports System.IO
Imports System.Security
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Security.Principal
Imports System.Runtime.InteropServices

Public Class wslogin
    Inherits System.Web.UI.Page
    Public MyConnection As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Response.Cache.SetCacheability(HttpCacheability.NoCache)
        ' Response.ExpiresAbsolute = DateTime.Now.AddMonths(-1)
        ' When Enter is pressed on the User field the get button is run
        Session("MaxLoginAttempts") = 2
        Session("LoggedIn") = "No"
        ClientScript.RegisterHiddenField("_EVENTTARGET", "btnLogin")

        'Message.InnerHtml = ""

        MyConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("wsConnString").ToString)

        If Not (IsPostBack) Then
            'Response.Write("Not ispostback")
            'GetRandomAlphaNumeric()
            user_id.Focus()
            'Login()
        Else
            If Session("RXWS") = "" Then
                Message.Style("color") = "red"
                Message.InnerHtml = "An Error ocurred.  Please close window and try again."
            End If
        End If
    End Sub
    Sub Session_Start()
        ' Administrator will only be allowed a certain number of login attempts
        Session("MaxLoginAttempts") = 2 ' Actually Allows 3 times
        Session("LoginCount") = 0

        'Track whether they're logged in or not
        Session("LoggedIn") = "No"
        Session.Timeout = 525600 ' Minutes (1 year)
    End Sub
    Sub PageReset_Click(Sender As Object, E As EventArgs)
        user_id.Text = ""
        password.Text = ""
        ' imagevalidation.Text = ""

        'user_name.value= ""
        Session("LoginCount") = 0
        Session("LoggedIn") = "No"

        user_id.Focus()
        Message.InnerHtml = ""
        'Message.InnerHtml = "LoginCount= " & Session("LoginCount")
    End Sub
    Sub Page_Init(Sender As Object, E As EventArgs)
        Dim blnCanConnect As Boolean

        blnCanConnect = IsSQLServerOnline(System.Configuration.ConfigurationManager.AppSettings("REXAIRDatabaseName").ToString) ' the SQL server Address

        If blnCanConnect = False Then ' Can not connect to the SQL Server
            Message.Style("color") = "red"
            Message.InnerHtml = "The Rexair Data Warehouse SQL Server is unavailable.  Try again later."
        Else
            'Message.InnerHtml = "The SQL Server is available."	 
            'Response.Write("PI Session = " & Session("WSRC"))
        End If
    End Sub
    Sub Page_UnLoad(ByVal Sender As Object, ByVal E As EventArgs)
        If Not (MyConnection Is DBNull.Value) Then
            If MyConnection.State = ConnectionState.Open Then
                MyConnection.Close()
                MyConnection.Dispose()
            End If
        End If

        'Session.Abandon
    End Sub

    Sub Login_Click(ByVal Sender As Object, ByVal E As EventArgs)
        Message.InnerHtml = ""
        Dim hold_input As String = ""
        Dim DS As DataSet
        Dim mycount As Integer = 0
        Dim maxLoginAttempts As Integer = 0
        Dim loginCount As Integer = 0

        'If GetKeyState(System.Windows.Forms.Keys.CapsLock) = -127 Then
        ' caps lock is on
        'ElseIf GetKeyState(System.Windows.Forms.Keys.CapsLock) = -128 Then
        ' caps lock is off
        'End If

        If user_id.Text = "" Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Please enter the User ID."
            user_id.Focus()
            Exit Sub
        End If

        If password.Text = "" Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Please enter the Password ID."
            password.Focus()
            Exit Sub
        End If

        'Check for SQL injection attack		
        hold_input = Trim(user_id.Text)
        hold_input = stripQuotes(hold_input)

        If SQLValid_Entry(Trim(hold_input)) = False Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Value is not valid."
            user_id.Focus()
            Exit Sub
        End If

        'Check for SQL injection attack		
        hold_input = Trim(password.Text)
        hold_input = stripQuotes(hold_input)

        If SQLValid_Entry(Trim(hold_input)) = False Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Value is not valid."
            password.Focus()
            Exit Sub
        End If

        'If imagevalidation.Text = "" Then
        '    Message.Style("color") = "red"
        '    Message.InnerHtml = "Please enter the Image Validation using the charachters displayed in the graphic below."
        '    password.Focus()
        '    Exit Sub
        'End If

        'If random_number.Text = "" Or (Trim(imagevalidation.Text) <> Trim(random_number.Text)) Then
        '    Message.Style("color") = "red"
        '    Message.InnerHtml = "Image validation does not match the Graphic, please try again."
        '    imagevalidation.Text = ""
        '    password.Focus()
        '    Exit Sub
        'End If

        If (Page.IsValid) Then
            Dim MyCommand As SqlDataAdapter
            Dim myuserSQL As String
            Dim decryptpassword As String

            myuserSQL = "SELECT * FROM vw_ws_user_master WHERE user_id = "
            myuserSQL = myuserSQL & Chr(39) & user_id.Text & Chr(39)

            MyCommand = New SqlDataAdapter(myuserSQL, MyConnection)

            'fill dataset
            DS = New DataSet()
            MyCommand.Fill(DS, "vw_ws_user_master")
            mycount = DS.Tables("vw_ws_user_master").Rows.Count

            maxLoginAttempts = Session("MaxLoginAttempts")
            If mycount <= 0 Then 'No Records returned, Invalid User
                'maxLoginAttempts = Session("MaxLoginAttempts")
                'Response.Write(maxLoginAttempts)
                'Response.Write(Session("LoginCount"))
                If Session("LoginCount") = maxLoginAttempts Then
                    'Message.InnerHtml = "Maximum number of tries has been reached. " & Session("LoginCount") & " " & maxLoginAttempts
                    loginCount = Session("LoginCount")
                    loginCount = loginCount + 1
                    Session("LoginCount") = loginCount

                    Message.InnerHtml = "Maximum number of tries has been reached. Next try, and the login window will close."

                    Message.Style("color") = "red"
                    user_id.Text = ""
                    password.Text = ""
                    'user_name.value = ""
                    user_id.Focus()
                    ' DS.Dispose()
                    DS = Nothing

                    Exit Sub
                ElseIf Session("LoginCount") >= (maxLoginAttempts + 1) Then ' Close window
                    'Response.Write("user_id.Text = " & user_id.Text)
                    'Response.End()
                    ' 03/08/10 BAN  Write pls_login_error_detail to track who is trying to hit the server with invalid information after 4 times
                    Insert_ws_login_error_detail(user_id.Text)
                    'imagevalidation.Text = ""

                    'Added these because Firefox does not honor window.close
                    user_id.Text = ""
                    user_id.Visible = "false"
                    password.Visible = "false"
                    'imagevalidation.Visible = "False"
                    ' btnRegenerateImage.Visible = "false"
                    btnLogin.Visible = "false"
                    btnClear.Visible = "false"
                    ' random_number.Text = ""
                    'Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    'Response.Cache.SetExpires(DateTime.Now) 'or a date much earlier than current time

                    Session.Abandon()
                    Dim s As String

                    'This works in IE but does not close in FireFox because Javascript did not open the window
                    'IE
                    's = "<SCRIPT language='javascript'>window.open('','_self','');window.close();</"
                    ''s = "<SCRIPT language='javascript'>window.open('','_parent','');window.close();</"
                    s = "<SCRIPT language='javascript'>closeWindow();</"
                    ''s = "<SCRIPT language='javascript'>self.close();</"
                    s = s & "SCRIPT>"
                    'FireFox
                    's = "<SCRIPT language='javascript'>window.open('wsloginclose.html','_self');</"
                    ' s = s & "SCRIPT>"

                    If Not (ClientScript.IsStartupScriptRegistered("closewindow")) Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "closewindow", s.ToString(), False)
                    End If
                    'DS.Dispose()
                    DS = Nothing

                    Exit Sub
                Else
                    loginCount = Session("LoginCount")
                    loginCount = loginCount + 1
                    Session("LoginCount") = loginCount

                    Message.InnerHtml = "Invalid User. " '& cstr(loginCount)
                    Message.Style("color") = "red"
                    user_id.Text = ""
                    'accept_confidentiality.checked = false
                    'user_name.value = ""
                    ' DS.Dispose()
                    DS = Nothing

                    user_id.Focus()
                    Exit Sub
                End If
            Else ' Valid User check for inactive user
                If DS.Tables("vw_ws_user_master").Rows(0)("inactive") = 1 Or DS.Tables("vw_ws_user_master").Rows(0)("master_account") = 0 Then

                    If Session("LoginCount") = maxLoginAttempts Then
                        loginCount = Session("LoginCount")
                        loginCount = loginCount + 1
                        Session("LoginCount") = loginCount
                        If DS.Tables("vw_ws_user_master").Rows(0)("master_account") = 0 Then
                            Message.InnerHtml = "To perform this function you need Master user capability. " & user_id.Text
                        Else ' Inactive
                            Message.InnerHtml = "User is inactive. " & user_id.Text
                        End If

                        Message.Style("color") = "red"
                        user_id.Text = ""
                        'accept_confidentiality.checked = false
                        'user_name.value = ""
                        ' DS.Dispose()
                        DS = Nothing

                        user_id.Focus()
                        Exit Sub
                    ElseIf Session("LoginCount") >= (maxLoginAttempts + 1) Then ' Close window
                        Insert_ws_login_error_detail(user_id.Text)
                        'imagevalidation.Text = ""

                        'Added these because Firefox does not honor window.close
                        user_id.Text = ""
                        user_id.Visible = "false"
                        password.Visible = "false"
                        ' imagevalidation.Visible = "False"
                        'btnRegenerateImage.Visible = "false"
                        btnLogin.Visible = "false"
                        btnClear.Visible = "false"
                        ' random_number.Text = ""
                        'Response.Cache.SetCacheability(HttpCacheability.NoCache)
                        'Response.Cache.SetExpires(DateTime.Now) 'or a date much earlier than current time

                        Session.Abandon()
                        Dim s As String

                        'This works in IE but does not close in FireFox because Javascript did not open the window
                        'IE
                        's = "<SCRIPT language='javascript'>window.open('','_self','');window.close();</"
                        ''s = "<SCRIPT language='javascript'>window.open('','_parent','');window.close();</"
                        s = "<SCRIPT language='javascript'>closeWindow();</"
                        ''s = "<SCRIPT language='javascript'>self.close();</"
                        s = s & "SCRIPT>"
                        'FireFox
                        's = "<SCRIPT language='javascript'>window.open('wsloginclose.html','_self');</"
                        ' s = s & "SCRIPT>"

                        If Not (ClientScript.IsStartupScriptRegistered("closewindow")) Then
                            ClientScript.RegisterStartupScript(Me.GetType(), "closewindow", s.ToString(), False)
                        End If
                        'DS.Dispose()
                        DS = Nothing

                        Exit Sub
                    Else
                        loginCount = Session("LoginCount")
                        loginCount = loginCount + 1
                        Session("LoginCount") = loginCount
                        If DS.Tables("vw_ws_user_master").Rows(0)("master_account") = 0 Then
                            Message.InnerHtml = "To perform this function you need Master user capability. " & user_id.Text
                        Else ' Inactive
                            Message.InnerHtml = "User is inactive. " & user_id.Text
                        End If

                        Message.Style("color") = "red"
                        user_id.Text = ""
                        'accept_confidentiality.checked = false
                        'user_name.value = ""
                        'DS.Dispose()
                        DS = Nothing

                        user_id.Focus()
                        Exit Sub
                    End If
                End If ' Inactive user
            End If

            decryptpassword = DecryptText(Trim(DS.Tables("vw_ws_user_master").Rows(0)("user_password")))
            If Trim(password.Text) = decryptpassword Then
                'If Trim(password.Text) = Trim(DS.Tables("vw_ws_user_master").Rows(0)("user_password")) Then
                Message.InnerHtml = "Valid User Login. " & user_id.Text
                'user_name.value = DS.Tables("vw_ws_user_master").Rows(0)("user_name")
                Session("LoggedIn") = "Yes"
                Session("LoginCount") = 0
                Session("UserID") = CStr(user_id.Text)

                ' Response.Write("A " & Session("UserID"))

                If DS.Tables("vw_ws_user_master").Rows(0)("master_account") = 1 Then
                    Session("Master") = "true"
                Else
                    Session("Master") = "false"
                End If

                'Response.Redirect("wsmainmenu.aspx")
                'DS.Dispose()
                DS = Nothing

                'Write Login Information to pls_last_login for informational purposes
                Create_Update_Last_Login()

                RedirectPage(Sender, E)

            Else ' Invalid Password

                If Session("LoginCount") = maxLoginAttempts Then
                    'Message.InnerHtml = "Maximum number of tries has been reached. " & Session("LoginCount") & " " & maxLoginAttempts
                    loginCount = Session("LoginCount")
                    loginCount = loginCount + 1
                    Session("LoginCount") = loginCount

                    Message.InnerHtml = "Maximum number of tries has been reached. Next try, and the login window will close."
                    Message.Style("color") = "red"
                    user_id.Text = ""
                    password.Text = ""
                    'accept_confidentiality.checked = false
                    'DS.Dispose()
                    DS = Nothing

                    password.Focus()
                    Exit Sub
                ElseIf Session("LoginCount") >= (maxLoginAttempts + 1) Then ' Close window
                    'Response.Write("user_id.Text = " & user_id.Text)
                    'Response.End()
                    ' 03/08/10 BAN  Write pls_login_error_detail to track who is trying to hit the server with invalid information after 4 times
                    Insert_ws_login_error_detail(user_id.Text)
                    'imagevalidation.Text = ""

                    'Added these because Firefox does not honor window.close
                    user_id.Text = ""
                    user_id.Visible = "false"
                    password.Visible = "false"
                    ' imagevalidation.Visible = "False"
                    'btnRegenerateImage.Visible = "false"
                    btnLogin.Visible = "false"
                    btnClear.Visible = "false"
                    '  random_number.Text = ""
                    'Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    'Response.Cache.SetExpires(DateTime.Now) 'or a date much earlier than current time

                    Session.Abandon()
                    Dim s As String

                    'This works in IE but does not close in FireFox because Javascript did not open the window
                    'IE
                    's = "<SCRIPT language='javascript'>window.open('','_self','');window.close();</"
                    ''s = "<SCRIPT language='javascript'>window.open('','_parent','');window.close();</"
                    s = "<SCRIPT language='javascript'>closeWindow();</"
                    ''s = "<SCRIPT language='javascript'>self.close();</"
                    s = s & "SCRIPT>"
                    'FireFox
                    's = "<SCRIPT language='javascript'>window.open('wsloginclose.html','_self');</"
                    ' s = s & "SCRIPT>"

                    If Not (ClientScript.IsStartupScriptRegistered("closewindow")) Then
                        ClientScript.RegisterStartupScript(Me.GetType(), "closewindow", s.ToString(), False)
                    End If
                    'DS.Dispose()
                    DS = Nothing

                    Exit Sub
                Else
                    Message.InnerHtml = "Invalid Password. "
                    Message.Style("color") = "red"
                    loginCount = Session("LoginCount")
                    loginCount = loginCount + 1
                    Session("LoginCount") = loginCount
                    password.Text = ""
                    ' DS.Dispose()
                    DS = Nothing
                    'accept_confidentiality.checked = false
                    password.Focus()
                End If
            End If

        End If ' Page Is valid
    End Sub
    Sub Insert_ws_login_error_detail(ByVal userid As String)
        Dim MyInsertCommand As SqlCommand
        Dim mydate As String
        Dim MyInsertCmd As String
        Dim remoteaddr As String = ""
        Dim remotehost As String = ""
        Dim servername As String = ""
        Dim serverport As String = ""
        Dim serversoftware As String = ""
        Dim httpuseragent As String = ""
        Dim url As String = ""

        'mydate = Format(Now, "yyyy-MM-dd hh:mm:ss")
        mydate = Format(Now, "yyyy-MM-dd hh:mm:ss tt")
        ' Response.Write("http_user_agent = " & Request.ServerVariables("http_user_agent"))
        httpuseragent = Mid(Trim(Request.ServerVariables("http_user_agent")), 1, 255)
        'returns Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET CLR 1.1.4322; .NET CLR 2.0.50727; InfoPath.2; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)
        'Response.Write("remote_addr = " & Request.ServerVariables("remote_addr"))
        remoteaddr = Mid(Trim(Request.ServerVariables("remote_addr")), 1, 20)
        'returns 192.168.10.210 for me and 76.235.199.204 for Tony (from outside dsl)
        'Response.Write("remote_host = " & Request.ServerVariables("remote_host"))
        remotehost = Mid(Trim(Request.ServerVariables("remote_host")), 1, 20)
        'returns 192.168.10.210 for me and 76.235.199.204 for Tony (from outside dsl)
        'Response.Write("request_method = " & Request.ServerVariables("request_method"))
        'returns POST
        'Response.Write("server_name = " & Request.ServerVariables("server_name"))
        servername = Mid(Trim(Request.ServerVariables("server_name")), 1, 50)
        'returns www.u-s-c-co.com
        'Response.Write("server_port = " & Request.ServerVariables("server_port"))
        serverport = Mid(Trim(Request.ServerVariables("server_port")), 1, 8)
        'returns 443
        ' Response.Write("server_software = " & Request.ServerVariables("server_software"))
        serversoftware = Mid(Trim(Request.ServerVariables("server_software")), 1, 100)
        'returns Microsoft-IIS/6.0
        'Response.Write("url = " & Request.ServerVariables("url"))
        url = Mid(Trim(Request.ServerVariables("url")), 1, 100)

        ' Response.Write("https_server_issuer = " & Request.ServerVariables("https_server_issuer"))
        'returns C=ZA, S=Western Cape, L=Cape Town, O=Thawte Consulting cc, OU=Certification Services Division, CN=Thawte Premium Server CA, E=premium-server@thawte.com
        'does not return anything
        ' Response.Write("http_host = " & Request.ServerVariables("http_host"))
        'Response.Write("local_addr = " & Request.ServerVariables("local_addr"))
        'Response.Write("http_user_agent = " & Request.ServerVariables("http_user_agent"))

        MyInsertCmd = "INSERT INTO ws_login_error_detail(user_id, creation_date, login_type, error_message,  "
        MyInsertCmd = MyInsertCmd & "remote_addr, remote_host, server_name, server_port, server_software, "
        MyInsertCmd = MyInsertCmd & "http_user_agent, url) "
        MyInsertCmd = MyInsertCmd & "VALUES (@userid, @creationdate, @logintype, @errormessage, "
        MyInsertCmd = MyInsertCmd & "@remoteaddr, @remotehost, @servername, @serverport, @serversoftware, "
        MyInsertCmd = MyInsertCmd & "@httpuseragent, @url) "

        MyInsertCommand = New SqlCommand(MyInsertCmd, MyConnection)

        MyInsertCommand.Parameters.Add(New SqlParameter("@userid", SqlDbType.VarChar, 50))
        MyInsertCommand.Parameters("@userid").Value = Trim(Server.HtmlEncode(userid))

        MyInsertCommand.Parameters.Add(New SqlParameter("@creationdate", SqlDbType.DateTime, 8))
        MyInsertCommand.Parameters("@creationdate").Value = Trim(Server.HtmlEncode(mydate))

        MyInsertCommand.Parameters.Add(New SqlParameter("@logintype", SqlDbType.VarChar, 10))
        MyInsertCommand.Parameters("@logintype").Value = Trim(Server.HtmlEncode("WS"))

        MyInsertCommand.Parameters.Add(New SqlParameter("@errormessage", SqlDbType.VarChar, 255))
        MyInsertCommand.Parameters("@errormessage").Value = Trim(Server.HtmlEncode("Maxinum number of tries exceeded."))

        MyInsertCommand.Parameters.Add(New SqlParameter("@remoteaddr", SqlDbType.VarChar, 20))
        MyInsertCommand.Parameters("@remoteaddr").Value = Trim(remoteaddr)

        MyInsertCommand.Parameters.Add(New SqlParameter("@remotehost", SqlDbType.VarChar, 20))
        MyInsertCommand.Parameters("@remotehost").Value = Trim(remotehost)

        MyInsertCommand.Parameters.Add(New SqlParameter("@servername", SqlDbType.VarChar, 50))
        MyInsertCommand.Parameters("@servername").Value = Trim(servername)

        MyInsertCommand.Parameters.Add(New SqlParameter("@serverport", SqlDbType.VarChar, 8))
        MyInsertCommand.Parameters("@serverport").Value = Trim(serverport)

        MyInsertCommand.Parameters.Add(New SqlParameter("@serversoftware", SqlDbType.VarChar, 100))
        MyInsertCommand.Parameters("@serversoftware").Value = Trim(serversoftware)

        MyInsertCommand.Parameters.Add(New SqlParameter("@httpuseragent", SqlDbType.VarChar, 255))
        MyInsertCommand.Parameters("@httpuseragent").Value = Trim(httpuseragent)

        MyInsertCommand.Parameters.Add(New SqlParameter("@url", SqlDbType.VarChar, 100))
        MyInsertCommand.Parameters("@url").Value = Trim(url)

        MyInsertCommand.Connection.Open()
        Try
            MyInsertCommand.ExecuteNonQuery()
            'response.write("Data Inserted")
            MyInsertCommand.Connection.Close()
            MyInsertCommand.Dispose()
        Catch Exp As SqlException
            'Message.InnerHtml = "Could not add the ws_last_login. Error: " & Exp.Number.Tostring & " " & Exp.Message 
            'Message.Style("color") = "red"
            'DS = nothing
            Exit Sub
        End Try

        MyInsertCommand.Connection.Close()
        MyInsertCommand.Dispose()
        MyInsertCommand = Nothing
    End Sub
    Sub Create_Update_Last_Login()
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim MyInsertCommand As SqlCommand
        Dim mydate As String
        Dim mycount As Integer
        Dim strSQL As String
        Dim MyInsertCmd As String
        Dim MyUpdateCommand As SqlCommand
        Dim numlogins As Long

        'mydate = Format(Now, "yyyy-MM-dd hh:mm:ss")
        mydate = Format(Now, "yyyy-MM-dd hh:mm:ss tt")

        'WS is Web Security
        mySQL = "SELECT * FROM ws_last_login WHERE login_type = "
        mySQL = mySQL & Chr(39) & "WS" & Chr(39)
        mySQL = mySQL & " AND user_id = "
        mySQL = mySQL & Chr(39) & Session("UserID") & Chr(39)

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)
        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "ws_last_login")
        mycount = DS.Tables("ws_last_login").Rows.Count

        If mycount <= 0 Then 'No Record returned, insert new record

            MyInsertCmd = "INSERT INTO ws_last_login(login_type, user_id, creation_date, last_login_date, num_logins) "
            MyInsertCmd = MyInsertCmd & "VALUES (@logintype, @userid, @creationdate, @lastlogindate, @numlogins) "

            MyInsertCommand = New SqlCommand(MyInsertCmd, MyConnection)

            MyInsertCommand.Parameters.Add(New SqlParameter("@logintype", SqlDbType.VarChar, 10))
            MyInsertCommand.Parameters("@logintype").Value = Trim(Server.HtmlEncode("WS"))

            MyInsertCommand.Parameters.Add(New SqlParameter("@userid", SqlDbType.VarChar, 50))
            MyInsertCommand.Parameters("@userid").Value = Trim(Server.HtmlEncode(Session("UserID")))

            MyInsertCommand.Parameters.Add(New SqlParameter("@creationdate", SqlDbType.DateTime, 8))
            MyInsertCommand.Parameters("@creationdate").Value = Trim(Server.HtmlEncode(mydate))

            MyInsertCommand.Parameters.Add(New SqlParameter("@lastlogindate", SqlDbType.DateTime, 8))
            MyInsertCommand.Parameters("@lastlogindate").Value = Trim(Server.HtmlEncode(mydate))

            MyInsertCommand.Parameters.Add(New SqlParameter("@numlogins", SqlDbType.Int, 9))
            MyInsertCommand.Parameters("@numlogins").Value = Int(1)

            MyInsertCommand.Connection.Open()
            Try
                MyInsertCommand.ExecuteNonQuery()
                'response.write("Data Inserted")
                MyInsertCommand.Connection.Close()
                MyInsertCommand.Dispose()
            Catch Exp As SqlException
                'Message.InnerHtml = "Could not add the ws_last_login. Error: " & Exp.Number.Tostring & " " & Exp.Message 
                'Message.Style("color") = "red"
                'DS = nothing
                Exit Sub
            End Try

            MyInsertCommand.Connection.Close()
            MyInsertCommand.Dispose()
        Else ' records exists Update last login datetime for User and WS
            numlogins = DS.Tables(0).Rows(0)("num_logins") + 1

            strSQL = "UPDATE ws_last_login SET "
            strSQL = strSQL & "last_login_date = '" & mydate & "', "
            strSQL = strSQL & " num_logins = " & numlogins
            strSQL = strSQL & " WHERE login_type = '" & Trim("WS") & "'"
            strSQL = strSQL & " AND user_id = '" & Trim(Session("UserID")) & "'"

            'response.write(strSQL)
            'response.end
            MyUpdateCommand = New SqlCommand(strSQL, MyConnection)
            MyUpdateCommand.Connection.Open()

            Try
                MyUpdateCommand.ExecuteNonQuery()
                'Message.InnerHtml = hpartnumber.text & "<b> Record Updated</b><br>"  
                MyUpdateCommand.Connection.Close()
                MyUpdateCommand.Dispose()
                'exit Sub		   
            Catch Exp As SqlException
                'If Exp.Number = 2627
                ' Message.InnerHtml = "ERROR: A record already exists with the same primary key. Error: " & Exp.Number.Tostring
                'Else
                '    Message.InnerHtml = "ERROR: Could not update record. Error: " & Exp.Number.Tostring
                ' End If
                '  Message.Style("color") = "red"
            End Try

            MyUpdateCommand.Connection.Close()
            MyUpdateCommand.Dispose()
        End If
        ' DS.Dispose()
        DS = Nothing

    End Sub

    'Checks for SQL injection attack
    Function stripQuotes(ByVal inputstr As String) As String
        stripQuotes = Replace(inputstr, "'", "''")
    End Function

    Function SQLValid_Entry(ByVal inputstr As String) As Boolean
        Dim valuecompare As Integer
        SQLValid_Entry = True

        inputstr = Trim(inputstr)
        inputstr = inputstr.ToUpper

        valuecompare = InStr(inputstr, "UNION")
        'response.write(valuecompare)
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If

        valuecompare = InStr(inputstr, "JOIN")
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If

        valuecompare = InStr(inputstr, "SELECT")
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If

        valuecompare = InStr(inputstr, "DROP")
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If

        valuecompare = InStr(inputstr, "INSERT")
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If

        valuecompare = InStr(inputstr, "DELETE")
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If

        valuecompare = InStr(inputstr, ";")
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If

        valuecompare = InStr(inputstr, "--")
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If

        valuecompare = InStr(inputstr, "XP_")
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If

        valuecompare = InStr(inputstr, "'")
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If

        valuecompare = InStr(inputstr, Chr(34))
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If
    End Function

    Sub RedirectPage(ByVal Sender As Object, ByVal E As EventArgs)
        Dim DS As DataSet
        'DS.Dispose()
        DS = Nothing
        MyConnection.Close()
        ' Response.Write(String.Format("User name is :{0}", Session("UserID")))
        ' Response.Write(String.Format("Master Account Is : {0}", Session("Master")))

        Response.Redirect("wsmainmenu.aspx", False)
        'Server.Transfer("wsmainmenu.aspx")
        'Context.RewritePath("wsmainmenu.aspx")
    End Sub

    'Decrypt the text 
    Public Shared Function DecryptText(ByVal strText As String) As String
        Return Decrypt(strText, "&%#@?,:*")
    End Function

    Public Shared Function Decrypt(ByVal strText As String, ByVal sDecrKey As String) As String
        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim inputByteArray(strText.Length) As Byte

        Try
            byKey = System.Text.Encoding.UTF8.GetBytes(Left(sDecrKey, 8))
            Dim des As New DESCryptoServiceProvider()
            inputByteArray = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8

            Return encoding.GetString(ms.ToArray())

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function
    Public Function IsSQLServerOnline(ByVal ServerAddress As String) As Boolean
        ' Tests a SQL Server connection by name or IP Address
        Try
            'Attempt to get server address
            Dim objIPHost As New System.Net.IPHostEntry()
            objIPHost = Dns.GetHostEntry(ServerAddress)

            Dim objAddress As System.Net.IPAddress
            objAddress = objIPHost.AddressList(0)

            'Connect to port 1433, most common SQL Server
            ' port. If your target is different, change here.
            Dim objTCP As System.Net.Sockets.TcpClient = New System.Net.Sockets.TcpClient()
            objTCP.Connect(objAddress, 1433)
            'No Problems. Close and Cleanup
            objTCP.Close()
            objTCP = Nothing
            objAddress = Nothing
            objIPHost = Nothing
            'Return Success
            Return True

        Catch ex As Exception
            'Server is unavailable, return fail value
            Return False
        End Try
    End Function
    Public Function Get_SQLUserInformationSetGlobalVariables(useridkey As String) As Boolean
        'From the Page_Init

        'guseridkey = rxdwprog
        Dim strSQL As String = ""
        Dim MySQLCommand As SqlCommand
        Dim MySQLDataReader As SqlDataReader
        Dim mycount As Integer = 0
        Dim MySQLConnection As SqlConnection

        'Get the  user_information from the Rexair Data Warehouse

        Get_SQLUserInformationSetGlobalVariables = False

        '---Open Connection to the Rexair Data Warehouse----------------------------------------------------
        'MySQLConnection = New SqlConnection("server=MFG41-SQL;database=RexairDataWarehouse;UID=RXDW;PWD=RXDW$2020;MultipleActiveResultSets=True") ' with Reader Writer
        MySQLConnection = New SqlConnection("server=MFG41-SQL;database=RexairDataWarehouse;UID=RXDWRO;PWD=RXDWRO$2020;MultipleActiveResultSets=True") ' Read Only Setup to be used for Reporting
        Try
            MySQLConnection.Open()
            '           tbStatus.Text = tbStatus.Text & "Rexair Data Warehouse database opened MFG41-SQL. Get_SQLUserInformationSetGlobalVariables" & Chr(13) & Chr(10)
        Catch ex As Exception
            '            tbStatus.Text = tbStatus.Text & "*Error Could Not open Rexair Data Warehouse database MFG41-SQL. Get_SQLUserInformationSetGlobalVariables" & ex.Message & Chr(13) & Chr(10)

            MySQLConnection.Dispose()
            MySQLConnection = Nothing
            Exit Function
        End Try

        ' strSQL = "Select user_password, user_email "
        strSQL = "Select user_email, user_display_name, Convert(varchar(50), DecryptByPassPhrase('rexairvbnetpearl',user_password)) AS DecryptedPassword "
        strSQL = strSQL & "FROM User_Information " ' 
        strSQL = strSQL & "WHERE user_id='" & useridkey & "'"
        ' MessageBox.Show("strCAL = " & strCAL)

        MySQLCommand = New SqlCommand(strSQL, MySQLConnection)

        Try
            MySQLDataReader = MySQLCommand.ExecuteReader()
            'MessageBox.Show("G")
            'Loop until a valid 
            While MySQLDataReader.Read()

                'mycount = mycount + 1
                Dim myConfiguration As Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")
                'myConfiguration.ConnectionStrings.ConnectionStrings("myDatabaseName").ConnectionString = txtConnectionString.Text
                myConfiguration.AppSettings.Settings.Item("gEmailPass").Value = MySQLDataReader("DecryptedPassword")
                myConfiguration.AppSettings.Settings.Item("gSharePointPwd").Value = MySQLDataReader("DecryptedPassword")
                myConfiguration.AppSettings.Settings.Item("gSharePointUserName").Value = MySQLDataReader("user_email")

                myConfiguration.Save()

                Get_SQLUserInformationSetGlobalVariables = True

                Exit While

            End While
            'Close the Connection to SQL Rexair_Data_Warehouse
            MySQLConnection.Close()
            MySQLConnection = Nothing

            MySQLCommand.Dispose()
            MySQLDataReader.Close()
            MySQLCommand = Nothing
            MySQLDataReader = Nothing

        Catch ex As SqlException
            Response.Write("<B><FONT COLOR=red>*ERROR Get_SQLUserInformationSetGlobalVariables SQL Exception Message  & " & CStr(ex.Message) & "</FONT></B>")
            'Close the Connection to SQL Rexair_Data_Warehouse
            MySQLConnection.Close()
            MySQLConnection = Nothing

            MySQLCommand.Dispose()
            MySQLCommand = Nothing

            Exit Function
        Catch ex As DataException
            Response.Write("<B><FONT COLOR=red>*ERROR Get_SQLUserInformationSetGlobalVariables Data Exception Message  & " & CStr(ex.Message) & "</FONT></B>")
            'Close the Connection to SQL Rexair_Data_Warehouse
            MySQLConnection.Close()
            MySQLConnection = Nothing

            MySQLCommand.Dispose()
            MySQLCommand = Nothing

            Exit Function
        Catch ex As Exception
            Response.Write("<B><FONT COLOR=red>*ERROR Get_SQLUserInformationSetGlobalVariables Exception Message & " & CStr(ex.Message) & "</FONT></B>")
            'Close the Connection to SQL Rexair_Data_Warehouse
            MySQLConnection.Close()
            MySQLConnection = Nothing

            MySQLCommand.Dispose()
            'MySQLDataReader.Close()
            MySQLCommand = Nothing
            ' MySQLDataReader = Nothing

            Exit Function

        End Try

    End Function

End Class