Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Security.Cryptography
Imports System.IO
Public Class wschangeuserpassword
    Inherits System.Web.UI.Page
    Public MyConnection As SqlConnection

    Public explabel As String = "( Password expires every 60 days. )"

    '02/09/2021 BAN Created
    Function ValidPassword(ByVal strPass As String) As String
        Dim strInput As String      ' String to hold our entered number
        Dim strCurrentChar As String ' Var for storing each character for eval.
        Dim I As Integer           ' Looping var
        Dim numcount As Integer
        Dim alphacount As Integer

        ValidPassword = ""
        numcount = 0
        alphacount = 0

        ' Uppercase all characters for consistency
        strInput = Trim(UCase(strPass))
        'response.write(strInput)
        If Len(strInput) < 8 Then
            'response.write("Less than 8")
            ValidPassword = ""
            Exit Function
        End If

        For I = 1 To Len(strInput)
            strCurrentChar = Mid(strInput, I, 1)
            ' Numbers (0 to 9)
            If Asc(strCurrentChar) >= Asc("0") And Asc(strCurrentChar) <= Asc("9") Then
                numcount = numcount + 1
                'strTemp = strTemp & strCurrentChar
            Else
                alphacount = alphacount + 1
            End If
            ' Upper Case Chars (A to Z)
            'If Asc("A") <= Asc(strCurrentChar) And Asc(strCurrentChar) <= Asc("Z") Then
            '    alphacount = alphacount + 1
            'strTemp = strTemp & strCurrentChar
            'End If 
        Next 'I
        'response.write(alphacount)
        'response.write(numcount)
        If (alphacount = 0) Or (numcount = 0) Then
            ValidPassword = ""
            Exit Function
        Else
            ' Set return value
            ValidPassword = Trim(strInput)
        End If
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

    Sub ChangeUser_Click(Sender As Object, E As EventArgs)
        Dim mycreationdate As String
        Dim mynulldate As String

        mynulldate = "2001-01-01 00:00:00"
        mycreationdate = Format(Now, "yyyy-MM-dd hh:mm:ss")
        Message.InnerHtml = ""

        If user_id.text = "" Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Please enter the User ID."
            user_id.Focus()
            Exit Sub
        Else
            If user_name.text = "" Then
                Message.Style("color") = "red"
                Message.InnerHtml = "Invalid User ID."
                user_id.Focus()
                Exit Sub
            End If
        End If

        If passunknown.checked = True And from_user_password.text <> "" Then
            Message.Style("color") = "red"
            Message.InnerHtml = "You may not enter data into the From User Password and check the password unknown checkbox."
            from_user_password.Focus()
            Exit Sub
        End If

        If passunknown.checked = False Then
            If from_user_password.text = "" Then
                Message.Style("color") = "red"
                Message.InnerHtml = "Please enter the From User Password."
                from_user_password.Focus()
                Exit Sub
            End If
        End If

        If to_user_password.text = "" Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Please enter the To User Password."
            to_user_password.Focus()
            Exit Sub
        Else ' Force Password at least 8 char with at least 1 number and 1 alpha
            If ValidPassword(to_user_password.Text) = "" Then
                Message.Style("color") = "red"
                Message.InnerHtml = "Please enter a valid Password. (Must be 8 char long with at least 1 alpha or 1 numeric.)"
                to_user_password.Focus()
                Exit Sub
            End If
        End If

        If confirm_user_password.text = "" Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Please enter the Confirm User Password."
            confirm_user_password.Focus()
            Exit Sub
        End If

        If SQLValid_Entry(Trim(user_id.text)) = False Then
            Message.Style("color") = "red"
            Message.InnerHtml = "You may not use a reserved word or special char in a User ID."
            user_id.Focus()
            Exit Sub
        End If

        If SQLValid_Entry(Trim(from_user_password.text)) = False Then
            Message.Style("color") = "red"
            Message.InnerHtml = "You may not use a reserved word or special char in Current Password."
            from_user_password.Focus()
            Exit Sub
        End If

        If SQLValid_Entry(Trim(to_user_password.text)) = False Then
            Message.Style("color") = "red"
            Message.InnerHtml = "You may not use a reserved word or special char in New Password."
            to_user_password.Focus()
            Exit Sub
        End If

        If SQLValid_Entry(Trim(confirm_user_password.text)) = False Then
            Message.Style("color") = "red"
            Message.InnerHtml = "You may not use a reserved word or special char in Confirmation New Password."
            confirm_user_password.Focus()
            Exit Sub
        End If

        If from_user_password.text = to_user_password.text Then
            Message.Style("color") = "red"
            Message.InnerHtml = "The From User Password must not equal the To User Password."
            from_user_password.Focus()
            Exit Sub
        End If

        'response.write(user_p_one.text)
        'response.write(user_p_two.text)
        'response.write(user_p_three.text)
        'response.write(to_user_password.text)
        If user_p_one.text = "" And user_p_two.text = "" And user_p_three.text = "" Then ' no check for past passwords
        Else
            If user_p_one.text <> "" Then
                If Trim(to_user_password.text) = user_p_one.text Then
                    Message.Style("color") = "red"
                    Message.InnerHtml = "The New User Password must not equal one of the last three passwords used."
                    to_user_password.Focus()
                    Exit Sub
                End If
            End If

            If user_p_two.text <> "" Then
                If Trim(to_user_password.text) = user_p_two.text Then
                    Message.Style("color") = "red"
                    Message.InnerHtml = "The New User Password must not equal one of the last three passwords used."
                    to_user_password.Focus()
                    Exit Sub
                End If
            End If

            If user_p_three.text <> "" Then
                If Trim(to_user_password.text) = user_p_three.text Then
                    Message.Style("color") = "red"
                    Message.InnerHtml = "The New User Password must not equal one of the last three passwords used."
                    to_user_password.Focus()
                    Exit Sub
                End If
            End If
        End If

        If to_user_password.text <> confirm_user_password.text Then
            Message.Style("color") = "red"
            Message.InnerHtml = "The To User Password must equal the Confirm User Password."
            to_user_password.Focus()
            Exit Sub
        End If

        'Response.Write("A")

        If (Page.IsValid) Then
            ' Response.Write("A0")
            If ConfirmFromUserPassword() = True Then ' Verify that the From User password is correct.
                'Response.Write("A1")
                If UpdateUserPassword() = True Then 'Update the User Password
                    'Response.Write("A2")
                    Message.InnerHtml = "User Password changed."
                    Message.Style("color") = "red"
                Else
                    'Message.InnerHtml = "ERROR: Could not add the To User records."
                    Message.Style("color") = "red"
                    Exit Sub
                End If
            Else
                Message.Style("color") = "red"
                Exit Sub
            End If
        Else
            'Response.Write("B")
        End If
        ' Response.Write("A3")

        user_id.text = ""
        user_name.text = ""
        from_user_password.text = ""
        to_user_password.text = ""
        confirm_user_password.text = ""
        last_password_change.Text = ""
        last_login_date.Text = ""
        login_type.Text = ""
        expiration_date.Text = ""
        expireslabel.Text = explabel
        user_p_one.text = ""
        user_p_two.text = ""
        user_p_three.text = ""
        never_expire.text = ""
        which_pass.text = 1

    End Sub

    Function ConfirmFromUserPassword() As Boolean
        Message.InnerHtml = ""
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim myuserSQL As String
        Dim mycount As Integer
        Dim decryptpassword As String

        myuserSQL = "SELECT user_password FROM vw_ws_user_master WHERE user_id = "
        myuserSQL = myuserSQL & Chr(39) & user_id.Text & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(myuserSQL, MyConnection)
        ConfirmFromUserPassword = True

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_user_master")
        mycount = DS.Tables("vw_ws_user_master").Rows.Count
        If mycount > 0 Then
            decryptpassword = DecryptText(Trim(DS.Tables("vw_ws_user_master").Rows(0)("user_password")))
            'response.write("decrypass - " & decryptpassword & " ")
            If passunknown.Checked = False Then
                If Trim(from_user_password.Text) <> decryptpassword Then
                    Message.InnerHtml = "Invalid From User Password."
                    from_user_password.Text = ""
                    Message.Style("color") = "red"
                    ConfirmFromUserPassword = False
                    from_user_password.Focus()
                End If
            Else ' password is unknown.  Therefore do not check for it to be able to set new password
                ConfirmFromUserPassword = True
            End If
        Else
            Message.InnerHtml = "Invalid User ID. " & user_id.Text
            user_name.Text = ""
            user_id.Text = ""
            from_user_password.Text = ""
            to_user_password.Text = ""
            confirm_user_password.Text = ""
            last_password_change.Text = ""
            last_login_date.Text = ""
            login_type.Text = ""
            expiration_date.Text = ""
            expireslabel.Text = explabel
            user_p_one.Text = ""
            user_p_two.Text = ""
            user_p_three.Text = ""
            never_expire.Text = ""
            which_pass.Text = 1
            Message.Style("color") = "red"
            ConfirmFromUserPassword = False
        End If
        DS.Dispose()
        DS = Nothing

    End Function
    Function UpdateUserPassword() As Boolean
        Dim strdgSQL As String
        Dim mymoddate As String
        Dim encryptpassword As String
        Dim newwhichpass As Integer

        UpdateUserPassword = False

        Message.InnerHtml = ""
        mymoddate = Format(Now, "yyyy-MM-dd HH:mm:ss")

        'response.write("touserpass - " & to_user_password.Value & " ")
        encryptpassword = EncryptText(to_user_password.Text)
        If which_pass.Text = 1 Then
            newwhichpass = 2
        ElseIf which_pass.Text = 2 Then
            newwhichpass = 3
        ElseIf which_pass.Text = 3 Then
            newwhichpass = 1
        End If

        strdgSQL = "UPDATE ws_user_master SET "
        strdgSQL = strdgSQL & "user_password = '" & Trim(encryptpassword) & "', "
        strdgSQL = strdgSQL & "modification_date = '" & mymoddate & "', "
        strdgSQL = strdgSQL & "last_password_change = '" & mymoddate & "', "
        strdgSQL = strdgSQL & "modification_user = '" & Session("UserID") & "', "
        strdgSQL = strdgSQL & "which_pass = " & newwhichpass & ", "
        If which_pass.Text = 1 Then
            strdgSQL = strdgSQL & "user_p_one = '" & Trim(encryptpassword) & "' "
        ElseIf which_pass.Text = 2 Then
            strdgSQL = strdgSQL & "user_p_two = '" & Trim(encryptpassword) & "' "
        ElseIf which_pass.Text = 3 Then
            strdgSQL = strdgSQL & "user_p_three = '" & Trim(encryptpassword) & "' "
        End If
        strdgSQL = strdgSQL & "WHERE user_id = '" & Trim(user_id.Text) & "'"

        Dim MyCommand As SqlCommand

        MyCommand = New SqlCommand(strdgSQL, MyConnection)
        MyCommand.Connection.Open()

        'Response.Write(strdgSQL)
        'Response.End()

        Try
            MyCommand.ExecuteNonQuery()

            Message.InnerHtml = user_id.Text & "<b> Record Updated</b><br>"

            user_id.Text = ""
            user_name.Text = ""
            from_user_password.Text = ""
            to_user_password.Text = ""
            confirm_user_password.Text = ""
            last_password_change.Text = ""
            last_login_date.Text = ""
            login_type.Text = ""
            expiration_date.Text = ""
            expireslabel.Text = explabel
            user_p_one.Text = ""
            user_p_two.Text = ""
            user_p_three.Text = ""
            never_expire.Text = ""
            which_pass.Text = 1

        Catch Exp As SqlException
            If Exp.Number = 2627 Then
                Message.InnerHtml = "ERROR: A record already exists with the same primary key. Error: " & Exp.Number.ToString
            Else
                Message.InnerHtml = "ERROR: Could not update record, please ensure the fields are correctly filled out. Error: " & Exp.Number.ToString
            End If
            Message.Style("color") = "red"
            MyCommand.Connection.Close()
            Exit Function
        End Try

        MyCommand.Connection.Close()

        'Return UpdateUserPassword()
        UpdateUserPassword = True
    End Function

    Sub Session_Start()
        Session.Timeout = 525600
    End Sub

    Sub Page_Init(Sender As Object, E As EventArgs)
        'Response.Write("PageINIT")
        'Response.End()
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
                'response.write("SQL Server Is available.")	  
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
        'Response.Write("PageLoad")
        '  Response.End()
        Message.InnerHtml = ""
        'RegisterHiddenField("_EVENTTARGET", "btnGet")
        ClientScript.RegisterHiddenField("_EVENTTARGET", "btnGet")

        MyConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("wsConnString").ToString)

        If Not (IsPostBack) Then
            user_id.Focus()
        End If
    End Sub
    Sub Page_UnLoad(Sender As Object, E As EventArgs)
        'If Not IsDBNull(MyConnection) Or MyConnection.State = ConnectionState.Open Then
        '    MyConnection.Close()
        'End If
    End Sub

    Sub PageReset_Click(Sender As Object, E As EventArgs)
        user_id.Text = ""
        user_name.Text = ""
        from_user_password.Text = ""
        to_user_password.Text = ""
        confirm_user_password.Text = ""
        last_password_change.Text = ""
        last_login_date.Text = ""
        login_type.Text = ""
        expireslabel.Text = explabel
        expiration_date.Text = ""
        user_p_one.Text = ""
        user_p_two.Text = ""
        user_p_three.Text = ""
        never_expire.Text = ""
        which_pass.Text = 1
        user_id.Focus()

        Message.InnerHtml = ""
    End Sub

    Sub ReturnToMainMenu(ByVal Sender As Object, ByVal E As EventArgs)
        Server.Transfer("wsmainmenu.aspx")

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

    'sub OpenUserSearch(Sender As Object, E As EventArgs)
    'btnGet.disabled = "false"
    'Dim s as string
    's = "<SCRIPT language='javascript'>pickUser("
    's = s & chr(34) & "user_id" & chr(34)
    's = s & ") </" 
    's = s & "SCRIPT>"
    'RegisterStartupScript("openuusersearch", s)
    'javascript:pickSupport("getsupport_id");'
    'end sub

    Sub ReturnUser_Click(Sender As Object, E As EventArgs)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer
        Dim myuserid As String
        Dim todaysdate As DateTime
        Dim datedifference As Integer

        todaysdate = Format(Now, "MM/dd/yyyy hh:mm:ss")

        Message.InnerHtml = ""

        user_name.Text = ""
        last_password_change.Text = ""
        last_login_date.Text = ""
        login_type.Text = ""
        expiration_date.Text = ""
        expireslabel.Text = explabel
        user_p_one.Text = ""
        user_p_two.Text = ""
        user_p_three.Text = ""
        never_expire.Text = ""
        which_pass.Text = 1
        btnGet.Disabled = "true"

        If user_id.Text = "" Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Please Enter a User ID."
            user_name.Text = ""
            user_id.Focus()
            Exit Sub
        Else
            myuserid = Trim(user_id.Text)
        End If

        'response.write("to " & mysupportid)		   
        mySQL = "SELECT user_id, user_name, user_password, last_password_change, user_p_one, user_p_two, user_p_three, never_expire, which_pass FROM vw_ws_user_master WHERE user_id = "
        mySQL = mySQL & Chr(39) & myuserid & Chr(39)
        mySQL = mySQL & " AND inactive = "
        mySQL = mySQL & Chr(39) & "0" & Chr(39)
        'Response.Write(mySQL)
        'Response.End()

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_user_master")
        mycount = DS.Tables("vw_ws_user_master").Rows.Count

        If mycount <= 0 Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Invalid User ID."
            user_id.Focus()
            Exit Sub
        Else
            user_name.Text = DS.Tables("vw_ws_user_master").Rows(0)("user_name")
            If Trim(DS.Tables("vw_ws_user_master").Rows(0)("user_p_one")) <> "" Then
                user_p_one.Text = DecryptText(DS.Tables("vw_ws_user_master").Rows(0)("user_p_one"))
            Else
                user_p_one.Text = ""
            End If
            If Trim(DS.Tables("vw_ws_user_master").Rows(0)("user_p_two")) <> "" Then
                user_p_two.Text = DecryptText(DS.Tables("vw_ws_user_master").Rows(0)("user_p_two"))
            Else
                user_p_two.Text = ""
            End If
            If Trim(DS.Tables("vw_ws_user_master").Rows(0)("user_p_three")) <> "" Then
                user_p_three.Text = DecryptText(DS.Tables("vw_ws_user_master").Rows(0)("user_p_three"))
            Else
                user_p_three.Text = ""
            End If
            last_password_change.Text = DS.Tables("vw_ws_user_master").Rows(0)("last_password_change")
            never_expire.Text = DS.Tables("vw_ws_user_master").Rows(0)("never_expire")
            which_pass.Text = DS.Tables("vw_ws_user_master").Rows(0)("which_pass")
            If never_expire.Text = "1" Then ' Password Never Expires
                expiration_date.Text = "NEVER"
                expireslabel.Text = ""
            Else
                Dim ts As TimeSpan = todaysdate.Subtract(last_password_change.Text)
                datedifference = ts.Days
                'Response.Write(datedifference)
                expiration_date.Text = DateTime.Now.AddDays((60 - datedifference))
                expireslabel.Text = explabel
            End If

            GetLastLoggedIn(myuserid)

            from_user_password.Focus()
        End If
        'to_user_id.visible = false
        DS.Dispose()
        DS = Nothing
    End Sub

    Sub GetLastLoggedIn(ByVal userid As String)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer
        Dim myuserid As String

        myuserid = userid

        'response.write("to " & mysupportid)		   
        mySQL = "SELECT login_type, user_id, creation_date, last_login_date FROM vw_ws_last_login WHERE user_id = "
        mySQL = mySQL & Chr(39) & myuserid & Chr(39)
        mySQL = mySQL & " ORDER BY last_login_date DESC"
        ' response.write(mySQL)
        'response.end

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_last_login")
        mycount = DS.Tables("vw_ws_last_login").Rows.Count

        If mycount <= 0 Then
            last_login_date.Text = ""
            login_type.Text = ""
            DS.Dispose()
            DS = Nothing
            Exit Sub
        Else
            last_login_date.Text = DS.Tables("vw_ws_last_login").Rows(0)("last_login_date")
            login_type.Text = DS.Tables("vw_ws_last_login").Rows(0)("login_type")
        End If

        DS.Dispose()
        DS = Nothing
    End Sub

    'Encrypt the text
    Public Shared Function EncryptText(ByVal strText As String) As String
        Return Encrypt(strText, "&%#@?,:*")
    End Function

    'Decrypt the text 
    Public Shared Function DecryptText(ByVal strText As String) As String
        Return Decrypt(strText, "&%#@?,:*")
    End Function

    'The function used to encrypt the text
    Private Shared Function Encrypt(ByVal strText As String, ByVal strEncrKey As String) As String
        'Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

        Try
            'byKey() = System.Text.Encoding.UTF8.GetBytes(Left(strEncrKey, 8))
            Dim byKey() As Byte = System.Text.Encoding.UTF8.GetBytes(Left(strEncrKey, 8))
            Dim des As New DESCryptoServiceProvider()
            Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(strText)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    'The function used to decrypt the text
    Private Shared Function Decrypt(ByVal strText As String, ByVal sDecrKey As String) As String
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
End Class