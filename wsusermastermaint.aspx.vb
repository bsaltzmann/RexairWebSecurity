Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Security.Cryptography
Imports System.IO
Public Class wsusermastermaint
    Inherits System.Web.UI.Page

    'Public bookmarkIndex As Integer = 0 ' The index of the bookmarked row
    'Public bookMark As Boolean = True 'Controls is bookmarked
    Dim MyConnection As SqlConnection

    '02/09/2021 BAN Created Form

    Function ValidPhone(ByVal strNumber As String) As String
        Dim strInput As String     ' String to hold our entered number
        Dim strTemp As String      ' Temporary string to hold our working text
        Dim strCurrentChar As String ' Var for storing each character for eval.
        Dim I As Integer           ' Looping var
        Dim strreplace As String

        ValidPhone = ""
        strTemp = ""

        ' Uppercase all characters for consistency
        strInput = UCase(strNumber)
        strreplace = Replace(strInput, "(", "")
        strInput = Replace(strreplace, ")", "")
        strreplace = Replace(strInput, "-", "")
        strInput = strreplace

        ' To be able to handle some pretty bad formatting we strip out
        ' all characters except for chars A to Z and digits 0 to 9
        ' before proceeding.  I left in the chars for stupid slogan
        ' numbers like 1-800-GET-CASH etc...

        For I = 1 To Len(strInput)
            strCurrentChar = Mid(strInput, I, 1)
            ' Numbers (0 to 9)
            If Asc("0") <= Asc(strCurrentChar) And Asc(strCurrentChar) <= Asc("9") Then
                strTemp = strTemp & strCurrentChar
            End If
            ' Upper Case Chars (A to Z)
            If Asc("A") <= Asc(strCurrentChar) And Asc(strCurrentChar) <= Asc("Z") Then
                strTemp = strTemp & strCurrentChar
            End If
        Next 'I

        ' Swap strTemp back to strInput for next set of validation
        ' I also clear strTemp just for good measure!
        strInput = strTemp
        strTemp = ""

        ' Remove leading 1 if applicable
        If Len(strInput) = 11 And Left(strInput, 1) = "1" Then
            strInput = Right(strInput, 10)
        End If

        ' Error catch to make sure strInput is proper length now that
        ' we've finished manipulating it.
        'response.write("strInput = " & strInput & " " & cstr(len(strInput)))
        If Not Len(strInput) = 10 Then
            ' Handle errors as you see fit.  This script raises a real
            ' error so you can handle it like any other runtime error,
            ' but you could also pass an error back via the function's
            ' return value or just display a message... your choice!
            ValidPhone = ""
            Exit Function
            'Err.Raise 1, "FormatPhoneNumber function", _
            '"The phone number to be formatted must be a valid 10 digit US phone number!"

            ' Two alternative error techniques!
            'Response.Write "<B>The phone number to be formatted must be a valid phone number!</B>"
            'Response.End

            ' Note if you use this you'll also need to check for
            ' this below so you don't overwrite it!
            'strTemp = "<B>The phone number to be formatted must be a valid phone number!</B>"
        Else
            strTemp = "("                             ' "("
            strTemp = strTemp & Left(strInput, 3)     ' Area code
            strTemp = strTemp & ") "                  ' ") "
            strTemp = strTemp & Mid(strInput, 4, 3)   ' Exchange
            strTemp = strTemp & "-"                   ' "-"
            strTemp = strTemp & Right(strInput, 4)    ' 4 digit part

            ' Set return value
            'default_phone.value = trim(strTemp)
            ValidPhone = Trim(strTemp)
        End If
    End Function

    Function ValidEmail(ByVal sCheckEmail As String) As Boolean
        Dim sEmail As String
        Dim nAtLoc As Integer
        ValidEmail = True
        sEmail = Trim(sCheckEmail)
        nAtLoc = InStr(1, sEmail, "@")

        If Not (nAtLoc > 1 And (InStrRev(sEmail, ".") > nAtLoc + 1)) Then
            ValidEmail = False
        ElseIf InStr(nAtLoc + 1, sEmail, "@") > nAtLoc Then
            ValidEmail = False
        ElseIf Mid(sEmail, nAtLoc + 1, 1) = "." Then
            ValidEmail = False
        ElseIf InStr(1, Right(sEmail, 2), ".") > 0 Then
            ValidEmail = False
        End If
    End Function

    Function ValidPassword(ByVal strPass As String) As String
        Dim strInput As String    ' String to hold our entered number
        Dim strCurrentChar As String ' Var for storing each character for eval.
        Dim I As Integer        ' Looping var
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

    Sub AddUser_Click(Sender As Object, E As EventArgs)
        Dim mycreationdate As String
        Dim mynulldate As String

        mynulldate = "2000-01-01 00:00:00"
        mycreationdate = Format(Now, "yyyy-MM-dd HH:mm:ss")
        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""

        If user_id.Text = "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the User ID."
            user_id.Focus()
            Exit Sub
            'else
            'customer_id.text = customer_id.text.ToLower()
        Else
            If SQLValid_Entry(Trim(user_id.Text)) = False Then
                AddMessage.Style("color") = "red"
                AddMessage.InnerHtml = "You may not use a reserved word or special char in the User ID."
                user_id.Focus()
                Exit Sub
            End If
        End If

        If user_name.Text = "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the User Name."
            user_name.Focus()
            Exit Sub
        Else
            If SQLValid_Entry(Trim(user_name.Text)) = False Then
                AddMessage.Style("color") = "red"
                AddMessage.InnerHtml = "You may not use a reserved word or special char in the User Name."
                user_name.Focus()
                Exit Sub
            End If
        End If

        If user_password.Text = "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the Password."
            user_password.Focus()
            Exit Sub
        Else ' Force Password at least 8 char with at least 1 number and 1 alpha
            '10/23/08
            If ValidPassword(user_password.Text) = "" Then
                AddMessage.Style("color") = "red"
                AddMessage.InnerHtml = "Please enter a valid Password. (Must be 8 char long with at least 1 alpha or 1 numeric.)"
                user_password.Focus()
                Exit Sub
            End If
        End If

        If verify_password.Text = "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the Verification Password."
            verify_password.Focus()
            Exit Sub
        Else ' Force Password at least 8 char with at least 1 number and 1 alpha
            If ValidPassword(verify_password.Text) = "" Then
                AddMessage.Style("color") = "red"
                AddMessage.InnerHtml = "Please enter a valid Verification Password. (Must be 8 char long with at least 1 alpha or 1 numeric.)"
                verify_password.Focus()
                Exit Sub
            End If
        End If

        If user_password.Text <> verify_password.Text Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "The Password and the Verification Password must be the same."
            verify_password.Focus()
            Exit Sub
        End If

        'Do not allow users and passwords to be entered using reservered words that 
        'could be used in a SQL injection attack.		
        If SQLValid_Entry(Trim(user_id.Text)) = False Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "You may not use a reserved word or special char in a User ID."
            user_id.Focus()
            Exit Sub
        End If

        If SQLValid_Entry(Trim(user_password.Text)) = False Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "You may not use a reserved word or a special char in a Password."
            user_password.Focus()
            Exit Sub
        End If

        If user_email.Text = "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the Email Address."
            user_email.Focus()
            Exit Sub
        Else
            If Not ValidEmail(user_email.Text) Then
                AddMessage.Style("color") = "red"
                AddMessage.InnerHtml = "Please enter a valid Email Address."
                user_email.Focus()
                Exit Sub
            End If
        End If

        If loc_cadillac.Checked = True And loc_troy.Checked = True Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "A user can not be physically in the Troy and Cadillac locations. Check one location."
            loc_cadillac.Focus()
            Exit Sub
        End If

        If loc_cadillac.Checked = False And loc_troy.Checked = False Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "A user must be physically in the Troy or Cadillac locations. Check one location."
            loc_cadillac.Focus()
            Exit Sub
        End If

        'If getlocation_id.Text = "" Then
        '    AddMessage.Style("color") = "red"
        '    AddMessage.InnerHtml = "Please enter the Location ID."
        '    getlocation_id.Focus()
        '    Exit Sub
        'Else
        '    UCase(getlocation_id.Text)
        '    If ValidLocation(getlocation_id.Text) = False Then
        '        AddMessage.Style("color") = "red"
        '        AddMessage.InnerHtml = "Enter a valid Location ID!"
        '        getlocation_id.Focus()
        '        Exit Sub
        '    Else
        '        ' location_id.Text = ValidPhone(default_phone.Text)
        '    End If
        'End If

        If (Page.IsValid) Then
            Dim MyCommand As SqlCommand
            Dim mycheckbox As Integer
            Dim mastercheckbox As Integer
            Dim encryptpassword As String
            Dim InsertCmd As String

            InsertCmd = "INSERT INTO ws_user_master (user_id, user_name, user_password, user_email, loc_cadillac, loc_troy, inactive, master_account, last_password_change, user_p_one, user_p_two, user_p_three, never_expire, which_pass, creation_date, creation_user, modification_date, modification_user) "
            InsertCmd = InsertCmd & "values (@userid, @username, @userpassword, @useremail, @loccadillac, @loctroy, @inactive, @masteraccount, @lastpasswordchange, @userpone, @userptwo, @userpthree, @neverexpire, @whichpass, @creationdate, @creationuser, @modificationdate, @modificationuser)"

            MyCommand = New SqlCommand(InsertCmd, MyConnection)

            MyCommand.Parameters.Add(New SqlParameter("@userid", SqlDbType.VarChar, 50))
            MyCommand.Parameters("@userid").Value = Trim(user_id.Text)

            MyCommand.Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 100))
            MyCommand.Parameters("@username").Value = Trim(Server.HtmlEncode(user_name.Text))

            MyCommand.Parameters.Add(New SqlParameter("@userpassword", SqlDbType.VarChar, 30))

            'response.write(user_password.Value & " ")
            encryptpassword = EncryptText(user_password.Text)
            'response.write("encrypass - " & encryptpassword & " ")
            'decryptpassword = DecryptText(encryptpassword)
            'response.write("decrypass - " & decryptpassword & " ")

            'MyCommand.Parameters("@userpassword").Value = Server.HtmlEncode(user_password.Value)
            MyCommand.Parameters("@userpassword").Value = Server.HtmlEncode(encryptpassword)

            'MyCommand.Parameters.Add(New SqlParameter("@locationid", SqlDbType.VarChar, 10))
            'MyCommand.Parameters("@locationid").Value = Server.HtmlEncode(getlocation_id.Text)

            MyCommand.Parameters.Add(New SqlParameter("@useremail", SqlDbType.VarChar, 50))
            MyCommand.Parameters("@useremail").Value = Server.HtmlEncode(user_email.Text)

            'MyCommand.Parameters.Add(New SqlParameter("@defaultphone", SqlDbType.VarChar, 20))
            'MyCommand.Parameters("@defaultphone").Value = Trim(Server.HtmlEncode(default_phone.Text))

            MyCommand.Parameters.Add(New SqlParameter("@loccadillac", SqlDbType.VarChar, 1))
            If loc_cadillac.Checked Then
                mycheckbox = 1
            Else
                mycheckbox = 0
            End If
            MyCommand.Parameters("@loccadillac").Value = Server.HtmlEncode(mycheckbox)

            MyCommand.Parameters.Add(New SqlParameter("@loctroy", SqlDbType.VarChar, 1))
            If loc_troy.Checked Then
                mycheckbox = 1
            Else
                mycheckbox = 0
            End If
            MyCommand.Parameters("@loctroy").Value = Server.HtmlEncode(mycheckbox)

            MyCommand.Parameters.Add(New SqlParameter("@inactive", SqlDbType.VarChar, 1))
            If inactive.Checked Then
                mycheckbox = 1
            Else
                mycheckbox = 0
            End If
            MyCommand.Parameters("@inactive").Value = Server.HtmlEncode(mycheckbox)

            MyCommand.Parameters.Add(New SqlParameter("@masteraccount", SqlDbType.VarChar, 1))
            If master_account.Checked Then
                mastercheckbox = 1
            Else
                mastercheckbox = 0
            End If
            MyCommand.Parameters("@masteraccount").Value = Server.HtmlEncode(mastercheckbox)

            'MyCommand.Parameters.Add(New SqlParameter("@defaultext", SqlDbType.VarChar, 10))
            'MyCommand.Parameters("@defaultext").Value = Trim(Server.HtmlEncode(default_ext.Text))

            MyCommand.Parameters.Add(New SqlParameter("@lastpasswordchange", SqlDbType.DateTime, 8))
            MyCommand.Parameters("@lastpasswordchange").Value = Trim(Server.HtmlEncode(mycreationdate))

            MyCommand.Parameters.Add(New SqlParameter("@userpone", SqlDbType.VarChar, 30))
            MyCommand.Parameters("@userpone").Value = Server.HtmlEncode(encryptpassword)

            MyCommand.Parameters.Add(New SqlParameter("@userptwo", SqlDbType.VarChar, 30))
            MyCommand.Parameters("@userptwo").Value = Trim(Server.HtmlEncode(""))

            MyCommand.Parameters.Add(New SqlParameter("@userpthree", SqlDbType.VarChar, 30))
            MyCommand.Parameters("@userpthree").Value = Trim(Server.HtmlEncode(""))

            MyCommand.Parameters.Add(New SqlParameter("@neverexpire", SqlDbType.VarChar, 1))
            If never_expire.Checked Then
                mycheckbox = 1
            Else
                mycheckbox = 0
            End If
            'MyCommand.Parameters("@inactive").Value = Server.HtmlEncode(inactive.Value) 
            MyCommand.Parameters("@neverexpire").Value = Server.HtmlEncode(mycheckbox)

            MyCommand.Parameters.Add(New SqlParameter("@whichpass", SqlDbType.Int, 4))
            MyCommand.Parameters("@whichpass").Value = CInt(Trim(Server.HtmlEncode("1")))

            MyCommand.Parameters.Add(New SqlParameter("@creationdate", SqlDbType.DateTime, 8))
            MyCommand.Parameters("@creationdate").Value = Trim(Server.HtmlEncode(mycreationdate))

            MyCommand.Parameters.Add(New SqlParameter("@creationuser", SqlDbType.VarChar, 50))
            MyCommand.Parameters("@creationuser").Value = Trim(Server.HtmlEncode(Session("UserID")))

            MyCommand.Parameters.Add(New SqlParameter("@modificationdate", SqlDbType.DateTime, 8))
            MyCommand.Parameters("@modificationdate").Value = Trim(Server.HtmlEncode(mynulldate))

            MyCommand.Parameters.Add(New SqlParameter("@modificationuser", SqlDbType.VarChar, 50))
            MyCommand.Parameters("@modificationuser").Value = Trim(Server.HtmlEncode(""))

            MyCommand.Connection.Open()

            Try
                MyCommand.ExecuteNonQuery()
                AddMessage.InnerHtml = user_id.Text.ToString() & "<b> Record Added</b><br>"
                user_id.Focus()
            Catch Exp As SqlException
                If Exp.Number = 2601 Then
                    AddMessage.InnerHtml = "ERROR: A record already exists with the same primary key. Error: " & Exp.Number.ToString
                Else
                    AddMessage.InnerHtml = "ERROR: Could not add record, please ensure the fields are correctly filled out. Error: " & Exp.Number.ToString
                End If
                AddMessage.Style("color") = "red"
            End Try

            MyCommand.Connection.Close()

        End If

        user_id.Text = ""
        user_name.Text = ""
        user_password.Text = ""
        verify_password.Text = ""
        user_email.Text = ""
        'getlocation_id.Text = ""
        'getlocation_name.Text = ""
        loc_cadillac.Checked = False
        loc_troy.Checked = False
        inactive.Checked = False
        master_account.Checked = False
        never_expire.Checked = False
        user_id.Focus()

        If sort_id.Text <> "" Then
            BindGrid(sort_id.Text)
        Else
            BindGrid("user_id")
        End If
    End Sub

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

    Sub BindGrid(ByVal SortField As String)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        'This line was added to stop the Invalid CurrentPageIndex value issue.  
        ' I commented this out because if I clicked the sort header I would get moved to the first page 
        ' instead of staying on the page I was on.
        'MyDataGrid.CurrentPageIndex=0	

        mySQL = "SELECT user_id, user_name, user_password, loc_cadillac, loc_troy, inactive, master_account, user_email, creation_date, "
        mySQL = mySQL & " creation_user, modification_date, modification_user, "
        mySQL = mySQL & " last_password_change, user_p_one, user_p_two, user_p_three, never_expire, which_pass "
        mySQL = mySQL & "FROM vw_ws_user_master"
        If SortField <> "" Then
            mySQL = mySQL & " ORDER BY " & Trim(SortField)
        End If
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_user_master")

        MyGridView.DataSource = DS.Tables("vw_ws_user_master").DefaultView
        MyGridView.DataBind()
    End Sub

    Sub FillGrid(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        mySQL = "SELECT user_id, user_name, user_password, loc_cadillac, loc_troy, inactive, master_account, user_email, creation_date, "
        mySQL = mySQL & " creation_user, modification_date, modification_user, "
        mySQL = mySQL & " last_password_change, user_p_one, user_p_two, user_p_three, never_expire, which_pass "
        mySQL = mySQL & "FROM vw_ws_user_master"
        If sort_id.Text <> "" Then
            mySQL = mySQL & " ORDER BY " & Trim(sort_id.Text)
        Else
            mySQL = mySQL & " ORDER BY " & "user_id"
        End If
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_user_master")

        MyGridView.DataSource = DS.Tables("vw_ws_user_master").DefaultView
        If Not EditIndex.Equals(Nothing) Then
            MyGridView.EditIndex = EditIndex
        End If

        MyGridView.DataBind()
    End Sub

    Sub gv_RowCommand(ByVal Sender As Object, ByVal e As GridViewCommandEventArgs)
        ' Fires when any button in a button column is clicked. Replaced DataGrids ItemCommand event
        Dim i As Integer = Convert.ToInt32(e.CommandArgument)

        'response.write("In ItemCommand")
        ' this event fires prior to all of the other commands
        ' use it to provide a more graceful transition out of edit mode
        ' Runs when any button is clicked on the grid
        'Dim scriptJs As String
        'Dim editbutton As LinkButton

        ' Added in case the next page number is clicked
        ' If e.Item.ItemIndex < 0 Then
        'Exit Sub
        '  End If

        'CType(e.Item.FindControl("imagepath"), Label)
        ' editbutton = MyDataGrid.Items(e.Item.ItemIndex).Cells(0).Controls(0)

        ' scriptJs = "<script language=javascript>" & vbCrLf
        '  scriptJs &= "document.getElementById('" & editbutton.UniqueID & "').focus();" & vbCrLf
        '  scriptJs &= "document.getElementById('" & editbutton.UniqueID & "').select();" & vbCrLf
        '  scriptJs &= "<" & "/script>"
        ' If (Not ClientScript.IsStartupScriptRegistered("Startup")) Then
        'ClientScript.RegisterStartupScript(Me.GetType(), "Startup", scriptJs, True)
        ' End If

        ' If (bookMark) Then
        'bookmarkIndex = e.Item.ItemIndex
        '  Me.InsertScriptBlock(e.Item.ClientID)
        ' End If

    End Sub

    Sub gv_RowDataBound(ByVal Sender As Object, ByVal e As GridViewRowEventArgs)
        ' Fires when a data row is bound to data. Replaced the DataGrid ItemDataBound event
        'Dim i As Integer = e.Row.RowType()
        'If e.Row.RowType = DataControlType.DataRow Then
        'I = Convert.ToInt32(DataBinder.Eval(e.RowDataItem, "ColumnName"))
        'End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim linkBtn As LinkButton = CType(e.Row.Cells.Item(0).Controls.Item(2), LinkButton)
            If linkBtn.CommandName = "Delete" Then
                linkBtn.OnClientClick = "return confirm('User and Group User Records will be removed in Web Security. Are you sure you want to delete this entry?');"
                'ElseIf linkBtn.CommandName = "Cancel" Then
                ' linkBtn.OnClientClick = "return confirm('Are you sure you want to cancel your changes?');"
            End If
        End If
    End Sub

    Sub gv_RowCancelingEdit(ByVal Sender As Object, ByVal e As GridViewCancelEditEventArgs)
        'Fires when the Cancel button of a row in edit mode is clicked, but before the row exists edit mode.
        ' The update operation was canceled. Clear the message label.
        Message.InnerHtml = ""
        FillGrid(-1)
    End Sub

    Sub gv_RowUpdating(ByVal Sender As Object, ByVal e As GridViewUpdateEventArgs)
        ' Fires when a Update Button is clicked. Fires Before
        Dim i, j As Integer
        Dim params(6) As String
        Dim blnGo As Boolean = True
        Dim strdgSQL As String
        Dim mymoddate As String
        Dim beforemoddate As String
        Dim chkInactiveBoxChecked As Integer
        Dim masterchkBoxChecked As Integer
        Dim nechkBoxChecked As Integer
        Dim chkCadBoxChecked As Integer
        Dim chkTroyBoxChecked As Integer
        Dim username As String
        Dim useremail As String
        'Dim locationid As String

        ' Use the CopyTo method to copy the DictionaryEntry objects in the 
        ' NewValues collection to an array.
        'Dim records(e.NewValues.Count - 1) As DictionaryEntry
        'e.NewValues.CopyTo(records, 0)

        ' Iterate through the array and HTML encode all user-provided values 
        ' before updating the data source.
        ''Dim entry As DictionaryEntry
        'For Each entry In records
        'e.NewValues(entry.Key) = Server.HtmlEncode(entry.Value.ToString())
        'Next

        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""
        username = ""
        useremail = ""
        'locationid = ""
        beforemoddate = ""

        mymoddate = Format(Now, "yyyy/MM/dd HH:mm:ss")

        If MyGridView.EditIndex < 0 Then
            Exit Sub
        End If

        Dim dgItem As GridViewRow
        'response.write(e.Item.ItemIndex)
        dgItem = MyGridView.Rows(e.RowIndex)
        'For Each dgItem In MyDataGrid.Items
        beforemoddate = CType(dgItem.FindControl("moddate"), Label).Text
        'Next
        'response.write("beforerc = " & beforemoddate)		
        'Response.Write(Trim(MyGridView.DataKeys(MyGridView.EditIndex).Value()))
        'Response.End()
        If RecordChangedSinceRead(Trim(MyGridView.DataKeys(MyGridView.EditIndex).Value()), beforemoddate) Then
            Message.Style("color") = "red"
            Message.InnerHtml = "The Record has been changed since it was read. Click Cancel and change the record again."
            Exit Sub
        End If

        j = 0
        chkInactiveBoxChecked = 0
        chkInactiveBoxChecked = 0
        masterchkBoxChecked = 0
        nechkBoxChecked = 0
        chkCadBoxChecked = 0
        chkTroyBoxChecked = 0

        'Message.InnerHtml = e.Item.Cells.Count.Tostring
        Dim row As GridViewRow = MyGridView.Rows(e.RowIndex)
        'Response.Write(MyGridView.Columns.Count)
        For i = 2 To MyGridView.Columns.Count - 1
            'Response.Write("i = " & i)
            If i = 8 Then ' Never Expire
                nechkBoxChecked = (CType(row.Cells(i).FindControl("edit_InactiveNECheckBox"), CheckBox).Checked) * -1
            ElseIf i = 7 Then ' Master
                masterchkBoxChecked = (CType(row.Cells(i).FindControl("edit_InactiveMCheckBox"), CheckBox).Checked) * -1
            ElseIf i = 4 Then ' Loc Cadillac
                chkCadBoxChecked = (CType(row.Cells(i).FindControl("edit_LocCadCheckBox"), CheckBox).Checked) * -1
            ElseIf i = 5 Then ' Loc Troy
                chkTroyBoxChecked = (CType(row.Cells(i).FindControl("edit_LocTroyCheckBox"), CheckBox).Checked) * -1
            ElseIf i = 6 Then ' Inactive
                chkInactiveBoxChecked = (CType(row.Cells(i).FindControl("edit_InactiveCheckBox"), CheckBox).Checked) * -1
            ElseIf i = 2 Then ' User Name
                username = Trim(CType(row.Cells(i).FindControl("username"), TextBox).Text)
                If username = "" Then
                    Message.Style("color") = "red"
                    Message.InnerHtml = "Please enter the User Name."
                    blnGo = False
                Else
                    If SQLValid_Entry(Trim(username)) = False Then
                        Message.Style("color") = "red"
                        Message.InnerHtml = "You may not use a reserved word or special char in the User Name."
                        blnGo = False
                    End If
                End If
            ElseIf i = 3 Then ' User Email
                useremail = Trim(CType(row.Cells(i).FindControl("useremail"), TextBox).Text)
                If useremail = "" Then
                    Message.Style("color") = "red"
                    Message.InnerHtml = "Please enter the Email Account."
                    blnGo = False
                End If
                If Not ValidEmail(useremail) Then
                    Message.Style("color") = "red"
                    Message.InnerHtml = "Please enter a valid Email Address."
                    blnGo = False
                    Exit For
                End If
                'ElseIf i = 4 Then ' Location ID
                '    locationid = UCase(Trim(CType(row.Cells(i).FindControl("locationid"), TextBox).Text))
                '    If locationid = "" Then
                '        Message.Style("color") = "red"
                '        Message.InnerHtml = "Please enter the Location ID."
                '        blnGo = False
                '    Else
                '        'UCase(getlocation_id.Text)
                '        If ValidLocation(locationid) = False Then
                '            Message.Style("color") = "red"
                '            Message.InnerHtml = "Enter a valid Location ID."
                '            blnGo = False
                '        Else
                '            ' location_id.Text = ValidPhone(default_phone.Text)
                '        End If

            End If
        Next

        If chkCadBoxChecked = 1 And chkTroyBoxChecked = 1 Then
            Message.Style("color") = "red"
            Message.InnerHtml = "A user can not be physically in Cadillac and Troy. Please choose one location."
            blnGo = False
        End If

        If chkCadBoxChecked = 0 And chkTroyBoxChecked = 0 Then
            Message.Style("color") = "red"
            Message.InnerHtml = "A user must be physically in Cadillac or Troy. Please choose one location."
            blnGo = False
        End If

        If blnGo Then
            'Message.InnerHtml = params(0).Tostring & " " & params(1).Tostring & " " & params(2).Tostring
            strdgSQL = "UPDATE ws_user_master SET " &
        "user_name = '" & Trim(username) & "', " &
        "user_email = '" & Trim(useremail) & "', " &
        "modification_date = '" & mymoddate & "', " &
        "modification_user = '" & Session("UserID") & "', " &
        "loc_cadillac = '" & Trim(chkCadBoxChecked) & "', " &
        "loc_troy = '" & Trim(chkTroyBoxChecked) & "', " &
        "inactive = '" & Trim(chkInactiveBoxChecked) & "', " &
        "master_account = '" & Trim(masterchkBoxChecked) & "'," &
        "never_expire = '" & Trim(nechkBoxChecked) & "'" &
        " WHERE user_id = '" & Trim(MyGridView.DataKeys(MyGridView.EditIndex).Value()) & "'"

            ' Response.Write(strdgSQL)
            'Response.End()

            'Message.InnerHtml = strdgSQL
            'Dim DS As DataSet
            Dim MyCommand As SqlCommand

            MyCommand = New SqlCommand(strdgSQL, MyConnection)
            MyCommand.Connection.Open()

            Try
                MyCommand.ExecuteNonQuery()
                Message.InnerHtml = MyGridView.DataKeys(MyGridView.EditIndex).Value() & "<b> Record Updated</b><br>"
            Catch Exp As SqlException
                If Exp.Number = 2627 Then
                    Message.InnerHtml = "ERROR: A record already exists with the same primary key. Error: " & Exp.Number.ToString
                Else
                    Message.InnerHtml = "ERROR: Could not update record, please ensure the fields are correctly filled out. Error: " & Exp.Number.ToString
                End If
                Message.Style("color") = "red"
            End Try

            MyCommand.Connection.Close()

            FillGrid(-1)
            user_id.Focus()
        End If
    End Sub

    Sub gv_RowUpdated(ByVal Sender As Object, ByVal e As GridViewUpdatedEventArgs)
        ' Fires when a Updated Button is clicked. Fires After
        ' Indicate whether the update operation succeeded.
        If e.Exception Is Nothing Then
            Message.InnerHtml = "Row updated successfully."
        Else
            e.ExceptionHandled = True
            Message.InnerHtml = "An error occurred while attempting to update the row."
            Message.Style("color") = "red"
        End If

    End Sub

    Sub gv_RowCreated(ByVal Sender As Object, ByVal e As GridViewRowEventArgs)
        ' Fires when a Updated Button is clicked. Fires After
        'If e.Row.RowType = DataControlRowType.Header Then
        'AddGlyph(MyGridView, e.Row)
        ' End If

    End Sub

    Sub AddGlyph(ByVal grid As GridView, ByVal item As GridViewRow)
        'Response.Write("AddGlyph")
        Dim glyph As Label = New Label()
        Dim I As Integer
        glyph.EnableTheming = False
        glyph.Font.Name = "webdings"
        glyph.Font.Size = FontUnit.XSmall
        'glyph.Text = (grid.SortDirection = SortDirection.Ascending)
        glyph.Text = IIf(grid.SortDirection = SortDirection.Ascending, " 5", " 6").ToString

        'find the column sorted by
        For I = 1 To (grid.Columns.Count - 1)
            Dim colexpr As String = grid.Columns(I).SortExpression
            'Response.Write("colexpr= " & colexpr)
            'Response.Write("gridsort= " & grid.SortExpression)
            If grid.SortExpression = "" Then
                item.Cells(1).Controls.Add(glyph)
            End If
            If colexpr > "" And colexpr = grid.SortExpression Then
                'Response.Write(colexpr)
                item.Cells(I).Controls.Add(glyph)
            End If
        Next

    End Sub

    Sub gv_RowDeleted(ByVal Sender As Object, ByVal e As GridViewDeletedEventArgs)
        ' Fires when a Delete Button is clicked. Fires After
        If e.Exception Is Nothing Then
            Message.InnerHtml = "Row deleted successfully."
        Else
            Message.Style("color") = "red"
            Message.InnerHtml = "An error occurred while attempting to delete the row."
            e.ExceptionHandled = True
        End If

    End Sub

    Sub gv_RowDeleting(ByVal Sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim struser_id As String = ""
        ' Fires when a Delete Button is clicked. Fires Before
        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""

        struser_id = MyGridView.DataKeys(e.RowIndex).Value

        If UserExists_WS_Application_Master(struser_id) = True Then ' user_id found as super_user_id
            Message.InnerHtml = "Error: Super User exists in Application Master. Can Not delete the User record. Inactivate the User ID."
            Message.Style("color") = "red"
            Exit Sub
        End If

        If UserExists_WS_Group_User(struser_id) = True Then ' user_id found as user_id
            Message.InnerHtml = "Error: User exists in Group User. Can Not delete the User record. Inactivate. Inactivate the User ID."
            Message.Style("color") = "red"
            Exit Sub
        End If

        If UserExists_WS_Last_Login(struser_id) = True Then ' user_id found as user_id
            Message.InnerHtml = "ERROR: User Id exists in the Last Logins.  Inactivate the User ID."
            Message.Style("color") = "red"
            Exit Sub
        End If

        If UserExists_WS_Login_Error(struser_id) = True Then ' user_id found as user_id
            Message.InnerHtml = "ERROR: User ID exists in the Login Errors.  Inactivate the User ID."
            Message.Style("color") = "red"
            Exit Sub
        End If

        Dim strdgSQL As String = "DELETE FROM ws_user_master WHERE user_id = " & Chr(39) & MyGridView.DataKeys(e.RowIndex).Value & Chr(39)
        ' Response.Write(strdgSQL)
        'Response.Write(MyGridView.DataKeys(e.RowIndex).Value)
        ' Response.End()
        Dim MyCommand As SqlCommand

        MyCommand = New SqlCommand(strdgSQL, MyConnection)
        MyCommand.Connection.Open()

        Try
            MyCommand.ExecuteNonQuery()
            Message.InnerHtml = "<b>Record Deleted</b><br>" & MyGridView.DataKeys(e.RowIndex).Value()
        Catch Exp As SqlException
            If Exp.Number = 2627 Then
                Message.InnerHtml = "ERROR: A record already exists with the same primary key. Error: " & Exp.Number.ToString
            Else
                Message.InnerHtml = "ERROR: Could not Delete record, please ensure the fields are correctly filled out. Error: " & Exp.Number.ToString
            End If
            Message.Style("color") = "red"
        End Try

        MyCommand.Connection.Close()

        'Added this code to stop the CurrentPageIndex value PageCount error when a record is deleted
        'and its the only item left on a page other than the first one
        If MyGridView.PageIndex >= (MyGridView.PageCount - 1) Then
            MyGridView.PageIndex = 0
            BindGrid(sort_id.Text)
            MyGridView.PageIndex = (MyGridView.PageCount - 1)
        End If

        FillGrid()
        user_id.Focus()

    End Sub

    Function UserExists_WS_Application_Master(struser_id) As Boolean
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer

        UserExists_WS_Application_Master = False

        mySQL = "SELECT application_id, super_user_id FROM vw_ws_application_master WHERE super_user_id = "
        mySQL = mySQL & Chr(39) & struser_id & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill DataSet
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_application_master")
        mycount = DS.Tables("vw_ws_application_master").Rows.Count

        If mycount > 0 Then
            UserExists_WS_Application_Master = True
        End If
    End Function
    Function UserExists_WS_Group_User(struser_id) As Boolean
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer

        UserExists_WS_Group_User = False

        mySQL = "SELECT group_id, user_id FROM vw_ws_group_user WHERE user_id = "
        mySQL = mySQL & Chr(39) & struser_id & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill DataSet
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_group_user")
        mycount = DS.Tables("vw_ws_group_user").Rows.Count

        If mycount > 0 Then
            UserExists_WS_Group_User = True
        End If
    End Function
    Function UserExists_WS_Last_Login(struser_id) As Boolean
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer

        UserExists_WS_Last_Login = False

        mySQL = "SELECT login_type, user_id FROM vw_ws_last_login WHERE user_id = "
        mySQL = mySQL & Chr(39) & struser_id & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill DataSet
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_last_login")
        mycount = DS.Tables("vw_ws_last_login").Rows.Count

        If mycount > 0 Then
            UserExists_WS_Last_Login = True
        End If
    End Function
    Function UserExists_WS_Login_Error(struser_id) As Boolean
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer

        UserExists_WS_Login_Error = False

        mySQL = "SELECT user_id, login_type FROM vw_ws_login_Error WHERE user_id = "
        mySQL = mySQL & Chr(39) & struser_id & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill DataSet
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_login_Error")
        mycount = DS.Tables("vw_ws_login_Error").Rows.Count

        If mycount > 0 Then
            UserExists_WS_Login_Error = True
        End If
    End Function
    Function DeleteAllUserInfo(ByVal Sender As Object, ByVal E As GridViewDeleteEventArgs) As Boolean
        'Dim MyCommand As SqlCommand
        'Dim mySQL As String

        DeleteAllUserInfo = False

        ''Delete user area group data
        'mySQL = "DELETE FROM group_user WHERE user_id = "
        'mySQL = mySQL & Chr(39) & MyGridView.DataKeys(E.RowIndex).Value() & Chr(39)
        ''Response.Write("uag = " & mySQL)
        ''Response.End()
        'MyCommand = New SqlCommand(mySQL, MyConnection)
        'MyCommand.Connection.Open()

        'Try
        '    MyCommand.ExecuteNonQuery()
        '    DeleteAllUserInfo = True
        'Catch Exp As SqlException
        '    DeleteAllUserInfo = False
        '    Exit Function
        'End Try
        'MyCommand.Connection.Close()

    End Function

    Sub gv_RowEditing(ByVal Sender As Object, ByVal E As GridViewEditEventArgs)
        'for i = 5 to e.Item.Cells.Count - 1
        'If i = 5 Then  
        'linkbutton.Attributes.Add("onclick", "javascript: pickDate(" & e.Item.Cells(i).Controls(0) & "');")
        'End If
        'exit for
        'Next

        ' The GridView control is entering edit mode. Clear the message label.
        Message.InnerHtml = ""
        'Set the edit index.
        MyGridView.EditIndex = E.NewEditIndex
        'MyGridView.EditIndex = E.row.dataitemindex
        'Response.Write(MyGridView.EditIndex)
        'Bind data to the GridView control.

        'Dim i As Integer = Convert.ToInt32(E.CommandArgument)

        FillGrid(MyGridView.EditIndex)
        'BindGrid("support_id")
        'Set the focus to control on the edited row
        'Dim rowindex As Integer = MyGridView.EditIndex
        'Dim row As GridViewRow = MyGridView.Rows(rowindex)

        MyGridView.Rows(E.NewEditIndex).FindControl("username").Focus()

    End Sub

    Sub gv_PreRender(ByVal Sender As Object, ByVal e As System.EventArgs)
        ' size the edit textboxes to an appropriate width; otherwise they get really wide 
        If MyGridView.EditIndex > -1 Then
            ' if its in edit mode, get the control, set its width and maxlength properties
            Dim rowindex As Integer = MyGridView.EditIndex
            Dim row As GridViewRow = MyGridView.Rows(rowindex)

            'User Name
            Dim UserNameTextBox As TextBox = CType(row.FindControl("username"), TextBox)
            UserNameTextBox.Width = Unit.Parse("200px")
            UserNameTextBox.MaxLength = 40

            'User Password
            'Dim UserPassTextBox As TextBox = CType(MyDataGrid.Items(MyDataGrid.EditItemIndex).Cells(4).Controls(0), TextBox)
            'UserPassTextBox.Width = Unit.Parse("60px")
            'UserPassTextBox.MaxLength = 10

            'User Email
            Dim UserEmailTextBox As TextBox = CType(row.FindControl("useremail"), TextBox)
            UserEmailTextBox.Width = Unit.Parse("200px")
            UserEmailTextBox.MaxLength = 50

            ''Location
            'Dim LocationIDTextBox As TextBox = CType(row.FindControl("locationid"), TextBox)
            'LocationIDTextBox.Width = Unit.Parse("100px")
            'LocationIDTextBox.MaxLength = 20

        End If
    End Sub

    Sub gv_PageIndexChanging(ByVal Sender As Object, ByVal E As GridViewPageEventArgs)
        ' Cancel the paging operation if the user attempts to navigate
        ' to another page while the GridView control is in edit mode. 
        If MyGridView.EditIndex <> -1 Then

            ' Use the Cancel property to cancel the paging operation.
            E.Cancel = True

            ' Display an error message.
            Dim newPageNumber As Integer = E.NewPageIndex + 1
            Message.Style("color") = "red"
            Message.InnerHtml = "Please update the record before moving to page " &
              newPageNumber.ToString() & "."
        Else
            ' Clear the error message.
            Message.InnerHtml = ""
            MyGridView.PageIndex = E.NewPageIndex
            FillGrid()
        End If

    End Sub

    Sub gv_PageIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Call a helper method to display the current page number 
        ' when the user navigates to a different page.
        DisplayCurrentPage()
    End Sub

    Sub DisplayCurrentPage()
        ' Calculate the current page number.
        Dim currentPage As Integer = MyGridView.PageIndex + 1

        ' Display the current page number. 
        Message.InnerHtml = "Page " & currentPage.ToString() & " of " &
        MyGridView.PageCount.ToString() & "."

    End Sub

    Sub gv_Sorting(ByVal Sender As Object, ByVal e As GridViewSortEventArgs)
        sort_id.Text = Trim(e.SortExpression)
        BindGrid(e.SortExpression)
    End Sub
    Function ValidLocation(strlocation As String) As Boolean
        'Message.InnerHtml = ""
        '' GetMessage.InnerHtml = ""
        'AddMessage.InnerHtml = ""
        '' Dim intgroup_id As Integer

        ValidLocation = False

        'If strlocation Is "" Then
        '    AddMessage.Style("color") = "red"
        '    AddMessage.InnerHtml = "Please enter the Location ID."
        '    getlocation_id.Focus()
        '    Exit Function
        'End If

        ''If (Page.IsValid) then
        'Dim DS As DataSet
        'Dim MyCommand As SqlDataAdapter
        'Dim mySQL As String
        'Dim mycount As Integer

        'mySQL = "SELECT * FROM vw_ws_location_master WHERE location_id = "
        'mySQL = mySQL & Chr(39) & strlocation & Chr(39)
        ''response.write(mySQL)	   
        ''Open Connection
        'MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        ''fill dataset
        'DS = New DataSet()
        'MyCommand.Fill(DS, "vw_ws_location_master")
        'mycount = DS.Tables("vw_ws_location_master").Rows.Count
        'If mycount > 0 Then
        '    getlocation_name.Text = DS.Tables("vw_ws_location_master").Rows(0)("location_name")
        '    ValidLocation = True
        'Else
        '    AddMessage.InnerHtml = "Invalid Location ID: " & getlocation_id.Text
        '    getlocation_id.Text = "*"
        '    getlocation_name.Text = ""
        '    getlocation_id.Focus()
        '    AddMessage.Style("color") = "red"
        'End If

        ''End If
        'DS.Dispose()
        'DS = Nothing

    End Function

    Sub ValidateLocation(ByVal Sender As Object, ByVal E As EventArgs)
        'Message.InnerHtml = ""
        '' GetMessage.InnerHtml = ""
        'AddMessage.InnerHtml = ""
        '' Dim intgroup_id As Integer

        'If getlocation_id.Text Is "" Then
        '    AddMessage.Style("color") = "red"
        '    AddMessage.InnerHtml = "Please enter the Location ID."
        '    getlocation_id.Focus()
        '    Exit Sub
        'Else
        '    UCase(getlocation_id.Text)
        'End If

        ''If Not IsNumeric(location_id.Text) Then
        ''    AddMessage.Style("color") = "red"
        ''    AddMessage.InnerHtml = "Please enter a number in the Location ID."
        ''    location_id.Focus()
        ''    Exit Sub
        ''End If

        ''intgroup_id = Int(group_id.Text)

        ''If (Page.IsValid) then
        'Dim DS As DataSet
        'Dim MyCommand As SqlDataAdapter
        'Dim mySQL As String
        'Dim mycount As Integer

        'mySQL = "SELECT * FROM vw_ws_location_master WHERE location_id = "
        'mySQL = mySQL & Chr(39) & getlocation_id.Text & Chr(39)
        ''response.write(mySQL)	   
        ''Open Connection
        'MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        ''fill dataset
        'DS = New DataSet()
        'MyCommand.Fill(DS, "vw_ws_location_master")
        'mycount = DS.Tables("vw_ws_location_master").Rows.Count
        'If mycount > 0 Then
        '    getlocation_name.Text = DS.Tables("vw_ws_location_master").Rows(0)("location_name")
        'Else
        '    AddMessage.InnerHtml = "Invalid Location ID: " & getlocation_id.Text
        '    getlocation_id.Text = "*"
        '    getlocation_name.Text = ""
        '    getlocation_id.Focus()
        '    AddMessage.Style("color") = "red"
        'End If

        ''End If
        'DS.Dispose()
        'DS = Nothing

    End Sub
    Sub ReturnLocation_Click(ByVal Sender As Object, ByVal E As EventArgs)
        'Dim DS As DataSet
        'Dim MyCommand As SqlDataAdapter
        'Dim mySQL As String
        'Dim mycount As Integer
        'Dim mylocationid As String

        'Message.InnerHtml = ""

        'getlocation_name.Text = ""
        'btnGet.Disabled = "true"

        'If getlocation_id.Text = "" Then
        '    Message.Style("color") = "red"
        '    Message.InnerHtml = "Please Choose Location ID."
        '    getlocation_name.Text = ""
        '    getlocation_id.Focus()
        '    Exit Sub
        'Else
        '    mylocationid = getlocation_id.Text
        'End If

        ''response.write("to " & mysupportid)		   
        'mySQL = "SELECT location_id, location_name FROM vw_ws_location_master WHERE location_id = "
        'mySQL = mySQL & Chr(39) & mylocationid & Chr(39)
        ''mySQL = mySQL & " AND inactive = "
        ''mySQL = mySQL & chr(39) & "0" & chr(39)		   
        ''response.write(mySQL)
        ''response.end

        ''Open Connection
        'MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        ''fill dataset
        'DS = New DataSet()
        'MyCommand.Fill(DS, "vw_ws_location_master")
        'mycount = DS.Tables("vw_ws_location_master").Rows.Count
        ''response.write(mycount)
        'If mycount <= 0 Then
        '    DS.Dispose()
        '    DS = Nothing
        '    Exit Sub
        'Else
        '    getlocation_name.Text = DS.Tables("vw_ws_location_master").Rows(0)("location_name")
        'End If

        ''response.write(group_name.value)
        ''location_id.visible = false
        'DS.Dispose()
        'DS = Nothing

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
    Sub Session_Start()
        Session.Timeout = 525600 ' 30 minutes
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
            'response.write(ConfigurationSettings.AppSettings("RCPLSDatabaseName"))
            'response.end
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
        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""

        'response.write(ConfigurationSettings.AppSettings("wsConnString"))
        MyConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("wsConnString").ToString)
        'response.end

        If Not (IsPostBack) Then
            user_id.Focus()
            If sort_id.Text <> "" Then
                BindGrid(sort_id.Text)
            Else
                BindGrid("user_id")
            End If
        End If
    End Sub

    Sub PageReset_Click(Sender As Object, E As EventArgs)
        user_id.Text = ""
        user_name.Text = ""
        user_password.Text = ""
        verify_password.Text = ""
        'getlocation_id.Text = ""
        'getlocation_name.Text = ""
        user_email.Text = ""
        loc_cadillac.Checked = False
        loc_troy.Checked = False
        inactive.Checked = False
        master_account.Checked = False
        never_expire.Checked = False
        MyGridView.SelectedIndex = -1

        BindGrid("user_id")

        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""
        user_id.Focus()
    End Sub
    Sub ReturnToMainMenu(ByVal Sender As Object, ByVal E As EventArgs)
        'Close report window
        CloseReportWindow()

        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), “ClosePopup”, “window.close();window.opener.location.href=window.opener.location.href;”, True)

        'Server.Transfer("wsmainmenu.aspx")
        '<a href="vmistocksearch.aspx"></a>
    End Sub

    Function GroupUserExists(ByVal struser_id As String) As Boolean
        'Dim DSuag As DataSet
        'Dim MyCommand As SqlDataAdapter
        'Dim mySQL As String
        'Dim mycount As Integer
        Dim dataexists As Boolean

        'mySQL = "SELECT * FROM group_user WHERE user_id = "
        'mySQL = mySQL & Chr(39) & struser_id & Chr(39)

        ''Open Connection
        'MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        ''fill dataset
        'DSuag = New DataSet()
        'MyCommand.Fill(DSuag, "group_user")
        'mycount = DSuag.Tables("group_user").Rows.Count

        dataexists = False
        'If mycount > 0 Then
        '    dataexists = True
        'End If

        Return dataexists
    End Function

    Function RecordChangedSinceRead(ByVal strkey_id As String, ByVal beforemoddate As String) As Boolean
        Dim DSpr As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim myprSQL As String
        Dim mycount As Integer
        Dim recordchanged As Boolean

        myprSQL = "SELECT * FROM ws_user_master WHERE user_id = "
        myprSQL = myprSQL & Chr(39) & strkey_id & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(myprSQL, MyConnection)

        'fill dataset
        DSpr = New DataSet()
        MyCommand.Fill(DSpr, "ws_user_master")
        mycount = DSpr.Tables("ws_user_master").Rows.Count

        'response.write("Window date = " & beforemoddate)
        recordchanged = True
        If mycount > 0 Then
            'response.write("DB Date = " & DSpr.Tables("ws_user_master").Rows(0)("modification_date"))		   

            'Only allow the change if someone has not changed the record inbetween
            If beforemoddate = DSpr.Tables("ws_user_master").Rows(0)("modification_date") Then
                'response.write("Dates equal")
                recordchanged = False
            End If
        End If
        DSpr.Dispose()
        DSpr = Nothing
        Return recordchanged
    End Function

    Sub CloseWindow()
        Dim s As String
        s = "<SCRIPT language='javascript'>window.close() </"
        s = s & "SCRIPT>"
        If Not (ClientScript.IsStartupScriptRegistered("closewindow")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "closewindow", s.ToString(), False)
        End If

    End Sub
    Sub CloseReportWindow()
        ' Response.Write("Before")
        Dim s As String
        s = "<SCRIPT language='javascript'>closeReportWin()</SCRIPT>"

        'Dim s As String
        's = "<SCRIPT language='javascript'>wsusermasterrpt.close() </"
        's = s & "SCRIPT>"
        If Not (ClientScript.IsStartupScriptRegistered("closereportwindow")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "closereportwindow", s, False)
        End If

        'Response.Write("After")
        'Response.End()

    End Sub

    Sub PrintData(ByVal Sender As Object, ByVal E As EventArgs)
        'Dim s As String
        's = "<SCRIPT language='javascript'>PrintData("
        's = s & Chr(34) & "wsusermastermaint.rpt" & Chr(34)
        's = s & ") </"
        's = s & "SCRIPT>"

        'If Not (ClientScript.IsStartupScriptRegistered("openprint")) Then
        '    ClientScript.RegisterStartupScript(Me.GetType(), "openprint", s, False)
        'End If

        Dim s As String
        s = "<SCRIPT language='javascript'>openReportWin()"
        s = s & "</SCRIPT>"

        If Not (ClientScript.IsStartupScriptRegistered("openprint")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "openprint", s, False)
        End If

        ''Dim url As String = "wsviewreport.aspx?formname=wsusermastermaint"
        'Dim url As String = "wsusermasterrpt.aspx"
        'Dim sr As String = "wsusermasterrpt = window.open('" & url + "', 'wsviewreport_window', 'width=1200,height=700,left=100,top=100,resizable=yes', 'true');"

        'If Not (ClientScript.IsStartupScriptRegistered("openprint")) Then
        '    ClientScript.RegisterStartupScript(Me.GetType(), "openprint", sr, True)
        'End If

    End Sub

End Class