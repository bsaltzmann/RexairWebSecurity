Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Security.Cryptography
Imports System.IO
Public Class wsapplicationmastermaint
    Inherits System.Web.UI.Page
    Dim MyConnection As SqlConnection

    '02/15/2021 BAN Created Form

    Sub AddApp_Click(Sender As Object, E As EventArgs)
        Dim mycreationdate As String
        Dim mynulldate As String

        mynulldate = "2000-01-01 00:00:00"
        mycreationdate = Format(Now, "yyyy-MM-dd HH:mm:ss")
        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""

        If application_id.Text = "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the Application ID."
            application_id.Focus()
            Exit Sub
            'else
            'customer_id.text = customer_id.text.ToLower()
        Else
            If SQLValid_Entry(Trim(application_id.Text)) = False Then
                AddMessage.Style("color") = "red"
                AddMessage.InnerHtml = "You may not use a reserved word or special char in the Application ID."
                application_id.Focus()
                Exit Sub
            End If
        End If

        If application_name.Text = "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the Application Name."
            application_name.Focus()
            Exit Sub
        Else
            If SQLValid_Entry(Trim(application_name.Text)) = False Then
                AddMessage.Style("color") = "red"
                AddMessage.InnerHtml = "You may not use a reserved word or special char in the Application Name."
                application_name.Focus()
                Exit Sub
            End If
        End If

        If SQLValid_Entry(Trim(application_id.Text)) = False Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "You may not use a reserved word or special char in a Application ID."
            application_id.Focus()
            Exit Sub
        End If

        If getuser_id.Text = "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the Super User ID."
            getuser_id.Focus()
            Exit Sub
        Else
            UCase(getuser_id.Text)
            If ValidUser(getuser_id.Text) = False Then
                AddMessage.Style("color") = "red"
                AddMessage.InnerHtml = "Enter a valid Super User ID!"
                getuser_id.Focus()
                Exit Sub
            Else
                ' location_id.Text = ValidPhone(default_phone.Text)
            End If
        End If

        If loc_cadillac.Checked = False And loc_troy.Checked = False Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "A user must be physically in the Troy or Cadillac locations. Check one location."
            loc_cadillac.Focus()
            Exit Sub
        End If

        If (Page.IsValid) Then
            Dim MyCommand As SqlCommand
            Dim mycheckbox As Integer
            Dim InsertCmd As String

            InsertCmd = "INSERT INTO ws_application_master (application_id, application_name, super_user_id, loc_cadillac, loc_troy, app_notes, inactive, creation_date, creation_user, modification_date, modification_user) "
            InsertCmd = InsertCmd & "values (@applicationid, @applicationname, @userid, @loccadillac, @loctroy, @appnotes, @inactive, @creationdate, @creationuser, @modificationdate, @modificationuser)"

            MyCommand = New SqlCommand(InsertCmd, MyConnection)

            MyCommand.Parameters.Add(New SqlParameter("@applicationid", SqlDbType.VarChar, 20))
            MyCommand.Parameters("@applicationid").Value = Trim(application_id.Text)

            MyCommand.Parameters.Add(New SqlParameter("@applicationname", SqlDbType.VarChar, 100))
            MyCommand.Parameters("@applicationname").Value = Trim(Server.HtmlEncode(application_name.Text))

            MyCommand.Parameters.Add(New SqlParameter("@userid", SqlDbType.VarChar, 50))
            MyCommand.Parameters("@userid").Value = Server.HtmlEncode(getuser_id.Text)

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

            MyCommand.Parameters.Add(New SqlParameter("@appnotes", SqlDbType.VarChar, 255))
            MyCommand.Parameters("@appnotes").Value = Server.HtmlEncode(app_notes.Text)

            MyCommand.Parameters.Add(New SqlParameter("@inactive", SqlDbType.VarChar, 1))
            If inactive.Checked Then
                mycheckbox = 1
            Else
                mycheckbox = 0
            End If
            MyCommand.Parameters("@inactive").Value = Server.HtmlEncode(mycheckbox)

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
                AddMessage.InnerHtml = application_id.Text.ToString() & "<b> Record Added</b><br>"
                application_id.Focus()
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

        application_id.Text = ""
        application_name.Text = ""
        getuser_id.Text = ""
        getuser_name.Text = ""
        app_notes.Text = ""
        loc_cadillac.Checked = False
        loc_troy.Checked = False
        inactive.Checked = False
        application_id.Focus()

        If sort_id.Text <> "" Then
            BindGrid(sort_id.Text)
        Else
            BindGrid("application_id")
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

        mySQL = "SELECT application_id, application_name, super_user_id, loc_cadillac, loc_troy, app_notes, inactive, creation_date, "
        mySQL = mySQL & " creation_user, modification_date, modification_user "
        mySQL = mySQL & "FROM ws_application_master"
        If SortField <> "" Then
            mySQL = mySQL & " ORDER BY " & Trim(SortField)
        End If
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        MyCommand.Fill(DS, "ws_application_master")

        MyGridView.DataSource = DS.Tables("ws_application_master").DefaultView
        MyGridView.DataBind()
    End Sub

    Sub FillGrid(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        mySQL = "SELECT application_id, application_name, super_user_id, loc_cadillac, loc_troy, app_notes, inactive, creation_date, "
        mySQL = mySQL & " creation_user, modification_date, modification_user "
        mySQL = mySQL & "FROM ws_application_master"
        If sort_id.Text <> "" Then
            mySQL = mySQL & " ORDER BY " & Trim(sort_id.Text)
        Else
            mySQL = mySQL & " ORDER BY " & "application_id"
        End If
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        MyCommand.Fill(DS, "ws_application_master")

        MyGridView.DataSource = DS.Tables("ws_application_master").DefaultView
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
                linkBtn.OnClientClick = "return confirm('Application Records will be removed in Web Security. Are you sure you want to delete this entry?');"
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
        Dim chkLocCadBoxChecked As Integer
        Dim chkLocTroyBoxChecked As Integer
        Dim applicationname As String
        Dim superuserid As String
        Dim appnotes As String

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
        applicationname = ""
        superuserid = ""
        appnotes = ""
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
        chkLocCadBoxChecked = 0
        chkLocTroyBoxChecked = 0
        'Message.InnerHtml = e.Item.Cells.Count.Tostring
        Dim row As GridViewRow = MyGridView.Rows(e.RowIndex)
        'Response.Write(MyGridView.Columns.Count)
        For i = 2 To MyGridView.Columns.Count - 1
            'Response.Write("i = " & i)
            If i = 5 Then ' Loc Cad
                chkLocCadBoxChecked = (CType(row.Cells(i).FindControl("edit_LocCadCheckBox"), CheckBox).Checked) * -1
            ElseIf i = 6 Then 'Loc Troy
                chkLocTroyBoxChecked = (CType(row.Cells(i).FindControl("edit_LocTroyCheckBox"), CheckBox).Checked) * -1
            ElseIf i = 7 Then ' Inactive
                chkInactiveBoxChecked = (CType(row.Cells(i).FindControl("edit_InactiveCheckBox"), CheckBox).Checked) * -1
            ElseIf i = 2 Then ' User Name
                applicationname = Trim(CType(row.Cells(i).FindControl("applicationname"), TextBox).Text)
                If applicationname = "" Then
                    Message.Style("color") = "red"
                    Message.InnerHtml = "Please enter the Application Name."
                    blnGo = False
                Else
                    If SQLValid_Entry(Trim(applicationname)) = False Then
                        Message.Style("color") = "red"
                        Message.InnerHtml = "You may not use a reserved word or special char in the Application Name."
                        blnGo = False
                    End If
                End If
            ElseIf i = 3 Then ' User ID
                superuserid = UCase(Trim(CType(row.Cells(i).FindControl("superuserid"), TextBox).Text))
                If superuserid = "" Then
                    Message.Style("color") = "red"
                    Message.InnerHtml = "Please enter the Super User ID."
                    blnGo = False
                Else
                    If ValidGridUser(superuserid) = False Then
                        Message.Style("color") = "red"
                        Message.InnerHtml = "Enter a valid User ID."
                        blnGo = False
                    Else
                        ' location_id.Text = ValidPhone(default_phone.Text)
                    End If
                End If
            ElseIf i = 4 Then ' Notes
                appnotes = UCase(Trim(CType(row.Cells(i).FindControl("appnotes"), TextBox).Text))
                'If appnotes = "" Then
                '    Message.Style("color") = "red"
                '    Message.InnerHtml = "Please enter the Application Notes."
                '    blnGo = False
                'End If
            End If

        Next

        If chkLocCadBoxChecked = 0 And chkLocTroyBoxChecked = 0 Then
            Message.Style("color") = "red"
            Message.InnerHtml = "An Application must be at Cadillac or Troy. Please choose one or both locations."
            blnGo = False
        End If

        If blnGo = True Then
            'Message.InnerHtml = params(0).Tostring & " " & params(1).Tostring & " " & params(2).Tostring
            strdgSQL = "UPDATE ws_application_master SET " &
        "application_name = '" & Trim(applicationname) & "', " &
        "super_user_id = '" & Trim(superuserid) & "', " &
        "loc_cadillac = '" & Trim(chkLocCadBoxChecked) & "', " &
        "loc_troy = '" & Trim(chkLocTroyBoxChecked) & "', " &
        "app_notes = '" & Trim(appnotes) & "', " &
        "modification_date = '" & mymoddate & "', " &
        "modification_user = '" & Session("UserID") & "', " &
        "inactive = '" & Trim(chkInactiveBoxChecked) & "' " &
        " WHERE application_id = '" & Trim(MyGridView.DataKeys(MyGridView.EditIndex).Value()) & "'"

            ' Response.Write(strdgSQL)
            '  Response.End()

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
            application_id.Focus()
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
        Dim strapplication_id As String = ""
        ' Fires when a Delete Button is clicked. Fires Before
        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""

        strapplication_id = MyGridView.DataKeys(e.RowIndex).Value

        If ApplicationExists_WS_Application_Area(strapplication_id) = True Then ' strapplication_id 
            Message.InnerHtml = "Error: Application exists in Application Area. Can Not delete the Application record. Inactivate the Application ID."
            Message.Style("color") = "red"
            Exit Sub
        End If

        If ApplicationExists_WS_Group_Application_Area(strapplication_id) = True Then ' strapplication_id 
            Message.InnerHtml = "Error: Application exists in Group Application Area. Can Not delete the Application record. Inactivate the Application ID."
            Message.Style("color") = "red"
            Exit Sub
        End If

        Dim strdgSQL As String = "DELETE FROM ws_application_master WHERE application_id = " & Chr(39) & MyGridView.DataKeys(e.RowIndex).Value & Chr(39)
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
        application_id.Focus()

    End Sub
    Function ApplicationExists_WS_Application_Area(strapplication_id) As Boolean
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer

        ApplicationExists_WS_Application_Area = False

        mySQL = "SELECT application_id, area_id FROM vw_ws_application_area WHERE application_id = "
        mySQL = mySQL & Chr(39) & strapplication_id & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill DataSet
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_application_area")
        mycount = DS.Tables("vw_ws_application_area").Rows.Count

        If mycount > 0 Then
            ApplicationExists_WS_Application_Area = True
        End If
    End Function
    Function ApplicationExists_WS_Group_Application_Area(strapplication_id) As Boolean
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer

        ApplicationExists_WS_Group_Application_Area = False

        mySQL = "SELECT application_id, area_id FROM vw_ws_group_application_area WHERE application_id = "
        mySQL = mySQL & Chr(39) & strapplication_id & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill DataSet
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_group_application_area")
        mycount = DS.Tables("vw_ws_group_application_area").Rows.Count

        If mycount > 0 Then
            ApplicationExists_WS_Group_Application_Area = True
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

        MyGridView.Rows(E.NewEditIndex).FindControl("applicationname").Focus()

    End Sub

    Sub gv_PreRender(ByVal Sender As Object, ByVal e As System.EventArgs)
        ' size the edit textboxes to an appropriate width; otherwise they get really wide 
        If MyGridView.EditIndex > -1 Then
            ' if its in edit mode, get the control, set its width and maxlength properties
            Dim rowindex As Integer = MyGridView.EditIndex
            Dim row As GridViewRow = MyGridView.Rows(rowindex)

            'App Name
            Dim AppNameTextBox As TextBox = CType(row.FindControl("applicationname"), TextBox)
            AppNameTextBox.Width = Unit.Parse("200px")
            AppNameTextBox.MaxLength = 40

            'Super User ID
            Dim UserIDTextBox As TextBox = CType(row.FindControl("superuserid"), TextBox)
            UserIDTextBox.Width = Unit.Parse("200px")
            UserIDTextBox.MaxLength = 50

            'App notes
            Dim AppNotesTextBox As TextBox = CType(row.FindControl("appnotes"), TextBox)
            AppNotesTextBox.Width = Unit.Parse("200px")
            AppNotesTextBox.MaxLength = 255

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
    Function ValidUser(struser As String) As Boolean
        Message.InnerHtml = ""
        ' GetMessage.InnerHtml = ""
        AddMessage.InnerHtml = ""
        ' Dim intgroup_id As Integer

        ValidUser = False

        If struser Is "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the User ID."
            getuser_id.Focus()
            Exit Function
        End If

        'If (Page.IsValid) then
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer

        mySQL = "SELECT * FROM vw_ws_user_master WHERE user_id = "
        mySQL = mySQL & Chr(39) & struser & Chr(39)
        'response.write(mySQL)	   
        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_user_master")
        mycount = DS.Tables("vw_ws_user_master").Rows.Count
        If mycount > 0 Then
            getuser_name.Text = DS.Tables("vw_ws_user_master").Rows(0)("user_name")
            ValidUser = True
        Else
            AddMessage.InnerHtml = "Invalid Super User ID: " & getuser_id.Text
            getuser_id.Text = "*"
            getuser_name.Text = ""
            getuser_id.Focus()
            AddMessage.Style("color") = "red"
        End If

        'End If
        DS.Dispose()
        DS = Nothing

    End Function

    Sub ValidateUser(ByVal Sender As Object, ByVal E As EventArgs)
        Message.InnerHtml = ""
        ' GetMessage.InnerHtml = ""
        AddMessage.InnerHtml = ""
        ' Dim intgroup_id As Integer

        If getuser_id.Text Is "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the Super User ID."
            getuser_id.Focus()
            Exit Sub
        Else
            'UCase(getlocation_id.Text)
        End If

        'If Not IsNumeric(location_id.Text) Then
        '    AddMessage.Style("color") = "red"
        '    AddMessage.InnerHtml = "Please enter a number in the Location ID."
        '    location_id.Focus()
        '    Exit Sub
        'End If

        'intgroup_id = Int(group_id.Text)

        'If (Page.IsValid) then
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer

        mySQL = "SELECT * FROM vw_ws_user_master WHERE user_id = "
        mySQL = mySQL & Chr(39) & getuser_id.Text & Chr(39)
        'response.write(mySQL)	   
        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_user_master")
        mycount = DS.Tables("vw_ws_user_master").Rows.Count
        If mycount > 0 Then
            getuser_name.Text = DS.Tables("vw_ws_user_master").Rows(0)("user_name")
        Else
            AddMessage.InnerHtml = "Invalid Super User ID: " & getuser_id.Text
            getuser_id.Text = "*"
            getuser_name.Text = ""
            getuser_id.Focus()
            AddMessage.Style("color") = "red"
        End If

        'End If
        DS.Dispose()
        DS = Nothing

    End Sub
    Sub ReturnUser_Click(ByVal Sender As Object, ByVal E As EventArgs)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer
        Dim myuserid As String

        Message.InnerHtml = ""

        getuser_name.Text = ""
        btnGet.Disabled = "true"

        If getuser_id.Text = "" Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Please Choose User ID."
            getuser_name.Text = ""
            getuser_id.Focus()
            Exit Sub
        Else
            myuserid = getuser_id.Text
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
        'response.write(mycount)
        If mycount <= 0 Then
            DS.Dispose()
            DS = Nothing
            Exit Sub
        Else
            getuser_name.Text = DS.Tables("vw_ws_user_master").Rows(0)("user_name")
        End If

        'response.write(user_name.value)
        'location_id.visible = false
        DS.Dispose()
        DS = Nothing

    End Sub
    Function ValidGridUser(struser As String) As Boolean
        Message.InnerHtml = ""
        ' GetMessage.InnerHtml = ""
        AddMessage.InnerHtml = ""
        ' Dim intgroup_id As Integer

        ValidGridUser = False

        If struser Is "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the Super User ID."
            getuser_id.Focus()
            Exit Function
        End If

        'If (Page.IsValid) then
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer

        mySQL = "SELECT * FROM vw_ws_user_master WHERE user_id = "
        mySQL = mySQL & Chr(39) & struser & Chr(39)
        'response.write(mySQL)	   
        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_user_master")
        mycount = DS.Tables("vw_ws_user_master").Rows.Count
        If mycount > 0 Then
            ' getuser_name.Text = DS.Tables("vw_ws_user_master").Rows(0)("user_name")
            ValidGridUser = True
        Else
            AddMessage.InnerHtml = "Invalid Super User ID: " & getuser_id.Text
            ' getuser_id.Text = "*"
            '  getuser_name.Text = ""
            '  getuser_id.Focus()
            AddMessage.Style("color") = "red"
        End If

        'End If
        DS.Dispose()
        DS = Nothing

    End Function

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
                'response.write("<B><FONT COLOR=red>The Rexair SQL Server is unavailable at this time.</FONT></B>")
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
            application_id.Focus()
            If sort_id.Text <> "" Then
                BindGrid(sort_id.Text)
            Else
                BindGrid("application_id")
            End If
        End If
    End Sub

    Sub PageReset_Click(Sender As Object, E As EventArgs)
        application_id.Text = ""
        application_name.Text = ""
        getuser_id.Text = ""
        getuser_name.Text = ""
        loc_cadillac.Checked = False
        loc_troy.Checked = False
        app_notes.Text = ""
        inactive.Checked = False
        MyGridView.SelectedIndex = -1

        BindGrid("application_id")

        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""
        application_id.Focus()
    End Sub
    Sub ReturnToMainMenu(ByVal Sender As Object, ByVal E As EventArgs)
        Server.Transfer("wsmainmenu.aspx")
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

        myprSQL = "SELECT * FROM ws_application_master WHERE application_id = "
        myprSQL = myprSQL & Chr(39) & strkey_id & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(myprSQL, MyConnection)

        'fill dataset
        DSpr = New DataSet()
        MyCommand.Fill(DSpr, "ws_application_master")
        mycount = DSpr.Tables("ws_application_master").Rows.Count

        'response.write("Window date = " & beforemoddate)
        recordchanged = True
        If mycount > 0 Then
            'response.write("DB Date = " & DSpr.Tables("ws_user_master").Rows(0)("modification_date"))		   

            'Only allow the change if someone has not changed the record inbetween
            If beforemoddate = DSpr.Tables("ws_application_master").Rows(0)("modification_date") Then
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

    Sub PrintData(ByVal Sender As Object, ByVal E As EventArgs)
        Dim s As String
        s = "<SCRIPT language='javascript'>PrintData("
        s = s & Chr(34) & "wsusermastermaint.rpt" & Chr(34)
        s = s & ") </"
        s = s & "SCRIPT>"

        If Not (ClientScript.IsStartupScriptRegistered("openprint")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "openprint", s, False)
        End If

        'RegisterStartupScript("openprint", s)
    End Sub

End Class