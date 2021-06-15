Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Public Class wscopyusersecurity
    Inherits System.Web.UI.Page
    Public MyConnection As SqlConnection
    '03/01/2021 BAN Created
    Sub CopyUser_Click(Sender As Object, E As EventArgs)
        Dim mycreationdate As String
        Dim mynulldate As String

        mynulldate = "2001-01-01 00:00:00"
        mycreationdate = Format(Now, "yyyy-MM-dd hh:mm:ss")
        CopyMessage.InnerHtml = ""

        If from_user_id.Text = "" Then
            CopyMessage.Style("color") = "red"
            CopyMessage.InnerHtml = "Please enter the From User ID."
            from_user_id.Focus()
            Exit Sub
        Else
            If SQLValid_Entry(Trim(from_user_id.Text)) = False Then
                CopyMessage.Style("color") = "red"
                CopyMessage.InnerHtml = "You may not use a reserved word or special char in the From User ID."
                from_user_id.Focus()
                Exit Sub
            End If
        End If

        If to_user_id.Text = "" Then
            CopyMessage.Style("color") = "red"
            CopyMessage.InnerHtml = "Please enter the To User ID."
            to_user_id.Focus()
            Exit Sub
        Else
            If SQLValid_Entry(Trim(to_user_id.Text)) = False Then
                CopyMessage.Style("color") = "red"
                CopyMessage.InnerHtml = "You may not use a reserved word or special char in the To User ID."
                to_user_id.Focus()
                Exit Sub
            End If
        End If

        If from_user_id.Text = to_user_id.Text Then
            CopyMessage.Style("color") = "red"
            CopyMessage.InnerHtml = "The From User ID can not equal the To User ID."
            to_user_id.Focus()
            Exit Sub
        End If

        If (Page.IsValid) Then
            If DeleteToUserInfo() = True Then ' Old Data removed or it did not exist originally
                If InsertToUserInfo() = False Then ' Old Data could not be removed
                    CopyMessage.InnerHtml = "ERROR: Could not add the To User records."
                    CopyMessage.Style("color") = "red"
                Else ' Data Inserted
                    CopyMessage.InnerHtml = "Copy Complete."
                    CopyMessage.Style("color") = "red"
                End If
            Else 'Old data could not be removed
                CopyMessage.InnerHtml = "ERROR: Could not delete the To User records."
                CopyMessage.Style("color") = "red"
            End If

        End If

        from_user_id.Text = ""
        from_user_name.Text = ""
        to_user_id.Text = ""
        to_user_name.Text = ""

    End Sub

    Function DeleteToUserInfo() As Boolean
        Dim MyCommand As SqlCommand
        Dim mySQL As String

        DeleteToUserInfo = False

        'Delete user area group data
        mySQL = "DELETE FROM ws_group_user WHERE user_id = "
        mySQL = mySQL & Chr(39) & to_user_id.Text & Chr(39)

        MyCommand = New SqlCommand(mySQL, MyConnection)
        MyCommand.Connection.Open()

        Try
            MyCommand.ExecuteNonQuery()
            DeleteToUserInfo = True
        Catch Exp As SqlException
            DeleteToUserInfo = False
            Exit Function
        End Try
        MyCommand.Connection.Close()

        MyCommand.Dispose()
        MyCommand = Nothing

    End Function

    Function InsertToUserInfo() As Boolean
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim myuserSQL As String
        Dim mycount As Integer
        Dim mycreationdate As String
        Dim mynulldate As String
        Dim j As Integer

        mynulldate = "2001-01-01 00:00:00"
        'CopyMessage.InnerHtml = ""
        InsertToUserInfo = False

        'Get all of the user_area records for the from_user_id
        myuserSQL = "SELECT * FROM ws_group_user WHERE user_id = "
        myuserSQL = myuserSQL & Chr(39) & from_user_id.Text & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(myuserSQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "ws_group_user")
        mycount = DS.Tables("ws_group_user").Rows.Count

        If mycount <= 0 Then
            InsertToUserInfo = True
            Exit Function
        End If

        'Add To User Area
        For j = 0 To (mycount - 1)
            mycreationdate = Format(Now, "yyyy-MM-dd HH:mm:ss")
            'customer_lb.Items.Insert(i, new ListItem(DS.Tables("customer_master").Rows(j)("customer_name")))

            Dim MyInsertCommand As SqlCommand

            Dim InsertCmd As String = "INSERT INTO ws_group_user (group_id, user_id, inactive, creation_date, creation_user, modification_date, modification_user) VALUES (@groupid, @userid, @inactive, @creationdate, @creationuser, @modificationdate, @modificationuser)"

            MyInsertCommand = New SqlCommand(InsertCmd, MyConnection)

            MyInsertCommand.Parameters.Add(New SqlParameter("@groupid", SqlDbType.VarChar, 20))
            MyInsertCommand.Parameters("@groupid").Value = Server.HtmlEncode(DS.Tables("ws_group_user").Rows(j)("group_id"))

            MyInsertCommand.Parameters.Add(New SqlParameter("@userid", SqlDbType.VarChar, 50))
            MyInsertCommand.Parameters("@userid").Value = Server.HtmlEncode(to_user_id.Text)

            MyInsertCommand.Parameters.Add(New SqlParameter("@inactive", SqlDbType.VarChar, 1))
            MyInsertCommand.Parameters("@inactive").Value = Server.HtmlEncode(DS.Tables("ws_group_user").Rows(j)("inactive"))

            MyInsertCommand.Parameters.Add(New SqlParameter("@creationdate", SqlDbType.DateTime, 8))
            MyInsertCommand.Parameters("@creationdate").Value = Trim(Server.HtmlEncode(mycreationdate))

            MyInsertCommand.Parameters.Add(New SqlParameter("@creationuser", SqlDbType.VarChar, 20))
            MyInsertCommand.Parameters("@creationuser").Value = Trim(Server.HtmlEncode(Session("UserID")))

            MyInsertCommand.Parameters.Add(New SqlParameter("@modificationdate", SqlDbType.DateTime, 8))
            MyInsertCommand.Parameters("@modificationdate").Value = Trim(Server.HtmlEncode(mynulldate))

            MyInsertCommand.Parameters.Add(New SqlParameter("@modificationuser", SqlDbType.VarChar, 20))
            MyInsertCommand.Parameters("@modificationuser").Value = Trim(Server.HtmlEncode(""))



            MyInsertCommand.Connection.Open()
            'CopyMessage.InnerHtml = user_id.Value & " " & area_id.Value & " " & mycheckbox

            Try
                MyInsertCommand.ExecuteNonQuery()
                InsertToUserInfo = True
            Catch Exp As SqlException
                InsertToUserInfo = False
                MyInsertCommand.Connection.Close()
                Exit Function
            End Try

            MyInsertCommand.Connection.Close()
        Next

        CopyMessage.Style("color") = "red"
        CopyMessage.InnerHtml = "Security copied from User ID " & from_user_id.Text & " to " & to_user_id.Text

    End Function

    Function SQLValid_Entry(ByVal inputstr As String) As Boolean
        Dim valuecompare As String
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
        valuecompare = InStr(inputstr, " DROP ")
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

    Sub Session_Start()
        Session.Timeout = 525600
    End Sub

    Sub Page_Init(Sender As Object, E As EventArgs)
        Dim blnCanConnect As Boolean

        If Session("LoggedIn") = "No" Or Session("UserID") = "" Then
            Response.Write("<B><FONT COLOR=red>Your session timed out due to inactivity or you have not logged in originally. You must re-login or login.</FONT></B>")
            Session.Abandon()
            Server.Transfer("wslogin.aspx")
            'CloseWindow()
            Exit Sub
        Else
            blnCanConnect = IsSQLServerOnline(System.Configuration.ConfigurationManager.AppSettings("REXAIRDatabaseName").ToString) ' the SQL server Address

            If blnCanConnect = False Then ' Can not connect to the SQL Server
                'response.write("<B><FONT COLOR=red>The Problem Report SQL Server is unavailable at this time.</FONT></B>")
                Session.Abandon()
                Server.Transfer("wslogin.aspx")
                'else
                'response.write("SQL Server is available.")	  
            End If
        End If
    End Sub

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

    Sub Page_Load(Sender As Object, E As EventArgs)
        CopyMessage.InnerHtml = ""

        MyConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("wsConnString").ToString)

        If Not (IsPostBack) Then
            from_user_id.Focus()
        End If
    End Sub

    Sub PageReset_Click(Sender As Object, E As EventArgs)
        from_user_id.Text = ""
        from_user_name.Text = ""
        to_user_id.Text = ""
        to_user_name.Text = ""
        from_user_id.Focus()

        CopyMessage.InnerHtml = ""
    End Sub

    Sub ReturnToMainMenu(ByVal Sender As Object, ByVal E As EventArgs)
        Server.Transfer("wsmainmenu.aspx")
        '<a href="vmistocksearch.aspx"></a>
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

    'sub OpenFromUserSearch(Sender As Object, E As EventArgs)
    'btnGet.disabled = "false"
    'Dim s as string
    's = "<SCRIPT language='javascript'>pickUser("
    's = s & chr(34) & "from_user_id" & chr(34)
    's = s & ") </" 
    's = s & "SCRIPT>"
    'RegisterStartupScript("openfusersearch", s)
    'javascript:pickSupport("getsupport_id");'
    'end sub

    Sub ReturnFromUser_Click(Sender As Object, E As EventArgs)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer
        Dim myuserid As String

        CopyMessage.InnerHtml = ""

        from_user_name.Text = ""
        btnGet.Disabled = "true"

        If from_user_id.Text = "" Then
            CopyMessage.Style("color") = "red"
            CopyMessage.InnerHtml = "Please Choose From User ID."
            from_user_name.Text = ""
            from_user_id.Focus()
            Exit Sub
        Else
            myuserid = from_user_id.Text
            'to_user_id.focus
        End If

        'response.write("to " & mysupportid)		   
        mySQL = "SELECT user_id, user_name FROM vw_ws_user_master WHERE user_id = "
        mySQL = mySQL & Chr(39) & myuserid & Chr(39)
        'mySQL = mySQL & " AND inactive = "
        'mySQL = mySQL & chr(39) & "0" & chr(39)		   
        'response.write(mySQL)
        'response.end

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_user_master")
        mycount = DS.Tables("vw_ws_user_master").Rows.Count

        If mycount <= 0 Then
            CopyMessage.Style("color") = "red"
            CopyMessage.InnerHtml = "Invalid From User ID: " & from_user_id.Text
            from_user_id.Text = ""
            from_user_name.Text = ""
            DS.Dispose()
            DS = Nothing
            from_user_id.Focus()
            Exit Sub
        Else
            from_user_name.Text = DS.Tables("vw_ws_user_master").Rows(0)("user_name")
            to_user_id.Focus()
        End If
        'to_user_id.visible = false
        DS.Dispose()
        DS = Nothing
    End Sub

    'sub OpenToUserSearch(Sender As Object, E As EventArgs)
    'btnTGet.disabled = "false"
    'Dim s as string
    's = "<SCRIPT language='javascript'>pickUser("
    's = s & chr(34) & "to_user_id" & chr(34)
    's = s & ") </" 
    's = s & "SCRIPT>"
    'RegisterStartupScript("opentusersearch", s)
    'javascript:pickSupport("getsupport_id");'
    'end sub

    Sub ReturnToUser_Click(Sender As Object, E As EventArgs)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer
        Dim myuserid As String

        CopyMessage.InnerHtml = ""

        to_user_name.Text = ""
        btnTGet.Disabled = "true"

        If to_user_id.Text = "" Then
            CopyMessage.Style("color") = "red"
            CopyMessage.InnerHtml = "Please Choose To User ID."
            to_user_name.Text = ""
            to_user_id.Focus()
            Exit Sub
        Else
            myuserid = to_user_id.Text
            'to_user_id.focus
        End If

        'response.write("to " & mysupportid)		   
        mySQL = "SELECT user_id, user_name from vw_ws_user_master where user_id = "
        mySQL = mySQL & Chr(39) & myuserid & Chr(39)
        'mySQL = mySQL & " AND inactive = "
        'mySQL = mySQL & chr(39) & "0" & chr(39)		   
        'response.write(mySQL)
        'response.end

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_user_master")
        mycount = DS.Tables("vw_ws_user_master").Rows.Count

        If mycount <= 0 Then
            CopyMessage.Style("color") = "red"
            CopyMessage.InnerHtml = "Invalid To User ID: " & to_user_id.Text
            to_user_id.Text = ""
            to_user_name.Text = ""
            DS.Dispose()
            DS = Nothing
            to_user_id.Focus()
            Exit Sub
        Else
            to_user_name.Text = DS.Tables("vw_ws_user_master").Rows(0)("user_name")
            btnCopy.Focus()
        End If
        'to_user_id.visible = false
        DS.Dispose()
        DS = Nothing
    End Sub



End Class