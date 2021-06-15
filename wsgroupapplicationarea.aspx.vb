Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Public Class wsgroupapplicationarea
    Inherits System.Web.UI.Page
    Public MyConnection As SqlConnection
    Sub AddGroupApplicationArea_Click(ByVal Sender As Object, ByVal E As EventArgs)
        Dim mycreationdate As String
        Dim mynulldate As String

        mynulldate = "2001-01-01 00:00:00"
        mycreationdate = Format(Now, "yyyy-MM-dd HH:mm:ss")

        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""
        GetMessage.InnerHtml = ""

        If getgroup_id.Text = "" Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter the Group ID."
            getgroup_id.Focus()
            Exit Sub
        Else
            If SQLValid_Entry(Trim(getgroup_id.Text)) = False Then
                GetMessage.Style("color") = "red"
                GetMessage.InnerHtml = "You may not use a reserved word or special char in the Group ID."
                getgroup_id.Focus()
                Exit Sub
            End If
        End If

        If getapplication_id.Text = "" Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter the Application ID."
            getapplication_id.Focus()
            Exit Sub
        Else
            If SQLValid_Entry(Trim(getapplication_id.Text)) = False Then
                GetMessage.Style("color") = "red"
                GetMessage.InnerHtml = "You may not use a reserved word or special char in the Application ID."
                getapplication_id.Focus()
                Exit Sub
            End If
        End If

        If getarea_id.Text = "" Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter the Application ID."
            getapplication_id.Focus()
            Exit Sub
        Else
            If SQLValid_Entry(Trim(getapplication_id.Text)) = False Then
                GetMessage.Style("color") = "red"
                GetMessage.InnerHtml = "You may not use a reserved word or special char in the Application ID."
                getapplication_id.Focus()
                Exit Sub
            End If
        End If

        'If Not IsNumeric(getapplication_id.Text) Then
        '    AddMessage.Style("color") = "red"
        '    AddMessage.InnerHtml = "Please enter a number in the Area ID."
        '    getapplication_id.Focus()
        '    Exit Sub
        'End If

        If group_id.Text = "" Then
            Exit Sub
        End If

        If application_id.Text = "" Then
            Exit Sub
        End If

        If area_id.Text = "" Then
            Exit Sub
        End If

        'If area_id Is "" Then
        '    AddMessage.Style("color") = "red"
        '    AddMessage.InnerHtml = "Please enter the Area ID."
        '    area_id.Focus()
        '    Exit Sub
        'Else
        '    If SQLValid_Entry(Trim(area_id.Text)) = False Then
        '        GetMessage.Style("color") = "red"
        '        GetMessage.InnerHtml = "You may not use a reserved word or special char in the Area ID."
        '        group_id.Focus()
        '        Exit Sub
        '    End If
        'End If

        If permission_create.Checked = False And permission_read.Checked = False And permission_update.Checked = False And permission_delete.Checked = False Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Check Application Permissions."
            permission_create.Focus()
            Exit Sub
        End If

        If (Page.IsValid) Then
            Dim MyCommand As SqlCommand
            Dim mycheckbox As Integer

            Dim InsertCmd As String = "INSERT INTO ws_group_application_area (group_id, application_id, area_id, permission_create, permission_read, permission_update, permission_delete, inactive, creation_date, creation_user, modification_date, modification_user) "
            InsertCmd = InsertCmd & "VALUES (@groupid, @applicationid, @areaid, @permissioncreate, @permissionread, @permissionupdate, @permissiondelete, @inactive, @creationdate, @creationuser, @modificationdate, @modificationuser)"

            MyCommand = New SqlCommand(InsertCmd, MyConnection)

            MyCommand.Parameters.Add(New SqlParameter("@groupid", SqlDbType.VarChar, 20))
            MyCommand.Parameters("@groupid").Value = Trim(Server.HtmlEncode(group_id.Text))

            MyCommand.Parameters.Add(New SqlParameter("@applicationid", SqlDbType.VarChar, 20))
            MyCommand.Parameters("@applicationid").Value = Server.HtmlEncode(application_id.Text)

            MyCommand.Parameters.Add(New SqlParameter("@areaid", SqlDbType.VarChar, 50))
            MyCommand.Parameters("@areaid").Value = Server.HtmlEncode(area_id.Text)

            MyCommand.Parameters.Add(New SqlParameter("@permissioncreate", SqlDbType.VarChar, 1))
            If permission_create.Checked Then
                mycheckbox = 1
            Else
                mycheckbox = 0
            End If
            MyCommand.Parameters("@permissioncreate").Value = Server.HtmlEncode(mycheckbox)

            MyCommand.Parameters.Add(New SqlParameter("@permissionread", SqlDbType.VarChar, 1))
            If permission_read.Checked Then
                mycheckbox = 1
            Else
                mycheckbox = 0
            End If
            MyCommand.Parameters("@permissionread").Value = Server.HtmlEncode(mycheckbox)

            MyCommand.Parameters.Add(New SqlParameter("@permissionupdate", SqlDbType.VarChar, 1))
            If permission_update.Checked Then
                mycheckbox = 1
            Else
                mycheckbox = 0
            End If
            MyCommand.Parameters("@permissionupdate").Value = Server.HtmlEncode(mycheckbox)

            MyCommand.Parameters.Add(New SqlParameter("@permissiondelete", SqlDbType.VarChar, 1))
            If permission_delete.Checked Then
                mycheckbox = 1
            Else
                mycheckbox = 0
            End If
            MyCommand.Parameters("@permissiondelete").Value = Server.HtmlEncode(mycheckbox)

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

            AddMessage.InnerHtml = group_id.Text & " " & application_id.Text & " " & area_id.Text

            'Response.Write(MyCommand.ToString)

            Try
                MyCommand.ExecuteNonQuery()
                AddMessage.InnerHtml = "<b> Record Added. Group:" & group_id.Text.ToString() & " Application: " & application_id.Text.ToString & " Area: " & area_id.Text & "</b><br>"
                getgroup_id.ReadOnly = "false"
                getapplication_id.ReadOnly = "false"
                'btnSubmit.disabled = "false"
                getgroup_id.Focus()
            Catch Exp As SqlException
                If Exp.Number = 2601 Then
                    AddMessage.InnerHtml = "ERROR: A record already exists with the same primary key. Error: " & Exp.Number.ToString
                Else
                    AddMessage.InnerHtml = "ERROR: Could not add record, please ensure the fields are correctly filled out. Error: " & Exp.Number.ToString & " " & Exp.ErrorCode.ToString & " " & Exp.Message
                End If
                AddMessage.Style("color") = "red"
            End Try

            MyCommand.Connection.Close()

        End If

        area_id.Text = ""
        application_id.Text = ""

        permission_create.Checked = False
        permission_read.Checked = False
        permission_update.Checked = False
        permission_delete.Checked = False
        inactive.Checked = False
        getgroup_id.Focus()

        If sort_id.Text <> "" Then
            BindGrid(sort_id.Text)
        Else
            BindGrid("area_id")
        End If
    End Sub

    Function GetApplication() As Boolean
        Dim DSapp As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim myappSQL As String
        Dim mycount As Integer
        Dim myreturn As Boolean

        myappSQL = "SELECT * FROM vw_ws_application_master WHERE application_id = "
        myappSQL = myappSQL & Chr(39) & getapplication_id.Text & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(myappSQL, MyConnection)

        'fill dataset
        DSapp = New DataSet()
        MyCommand.Fill(DSapp, "vw_ws_application_master")
        mycount = DSapp.Tables("vw_ws_application_master").Rows.Count
        If mycount > 0 Then
            getapplication_name.Text = DSapp.Tables("vw_ws_application_master").Rows(0)("application_name")
            'getapplication_id.readonly = "true"
            getapplication_name.ReadOnly = "true"
            application_id.Text = getapplication_id.Text
            myreturn = True
        Else
            GetMessage.InnerHtml = "Invalid Application ID. " & getapplication_id.Text
            'getarea_id.readonly = "false"
            getapplication_name.ReadOnly = "false"
            getapplication_name.Text = ""
            getapplication_id.Text = ""
            group_id.Text = ""
            application_id.Text = "*"
            area_id.Text = "*"
            GetMessage.Style("color") = "red"
            myreturn = False
        End If
        DSapp.Dispose()
        DSapp = Nothing

        Return myreturn
    End Function

    Function GetGroup() As Boolean
        Dim DSgroup As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mygroupSQL As String
        Dim mycount As Integer
        Dim myreturn As Boolean

        mygroupSQL = "SELECT * FROM vw_ws_group_master WHERE group_id = "
        mygroupSQL = mygroupSQL & Chr(39) & getgroup_id.Text & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(mygroupSQL, MyConnection)

        'fill dataset
        DSgroup = New DataSet()
        MyCommand.Fill(DSgroup, " vw_ws_group_master")
        mycount = DSgroup.Tables(" vw_ws_group_master").Rows.Count
        If mycount > 0 Then
            getgroup_name.Text = DSgroup.Tables("vw_ws_group_master").Rows(0)("group_name")
            getgroup_id.ReadOnly = "true"
            getgroup_name.ReadOnly = "true"
            'btnSubmit.disabled = "true"
            group_id.Text = getgroup_id.Text
            myreturn = True
        Else
            GetMessage.InnerHtml = "Invalid Group ID. " & getgroup_id.Text
            getgroup_id.ReadOnly = "false"
            getgroup_name.ReadOnly = "false"
            'btnSubmit.disabled = "false"
            getgroup_name.Text = ""
            getgroup_id.Text = ""
            group_id.Text = ""
            GetMessage.Style("color") = "red"
            myreturn = False
        End If

        DSgroup.Dispose()
        DSgroup = Nothing

        Return myreturn
    End Function

    Sub GetGroupApplicationArea_Click(ByVal Sender As Object, ByVal E As EventArgs)
        Dim j As Integer

        Message.InnerHtml = ""
        GetMessage.InnerHtml = ""

        If getgroup_id.Text = "" Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter the Group ID."
            getgroup_id.Focus()
            Exit Sub
        Else
            If SQLValid_Entry(Trim(getgroup_id.Text)) = False Then
                GetMessage.Style("color") = "red"
                GetMessage.InnerHtml = "You may not use a reserved word or special char in the Group ID."
                getgroup_id.Focus()
                Exit Sub
            End If
        End If

        If getapplication_id.Text = "" Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter the Application ID."
            getapplication_id.Focus()
            Exit Sub
        Else

            Dim fields As String() = Split(getapplication_id.Text, "^")

            Dim field As String
            j = 0
            For Each field In fields
                Console.WriteLine(field)
                If j = 0 Then
                    getapplication_id.Text = Trim(field)
                End If
                If j = 1 Then
                    getarea_id.Text = Trim(field)
                End If
                j = j + 1
            Next

            If SQLValid_Entry(Trim(getapplication_id.Text)) = False Then
                GetMessage.Style("color") = "red"
                GetMessage.InnerHtml = "You may not use a reserved word or special char in the Application ID."
                getapplication_id.Focus()
                Exit Sub
            End If
        End If

        If getarea_id.Text = "" Then
            'AreaReset_Click(Sender, E)
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter the Area ID."
            getarea_id.Focus()
            Exit Sub
        Else
            If SQLValid_Entry(Trim(getarea_id.Text)) = False Then
                GetMessage.Style("color") = "red"
                GetMessage.InnerHtml = "You may not use a reserved word or special char in the Area ID."
                getarea_id.Focus()
                Exit Sub
            End If
        End If

        'If Not IsNumeric(getarea_id.Text) Then
        '    GetMessage.Style("color") = "red"
        '    GetMessage.InnerHtml = "Please enter a number in the Area ID."
        '    getarea_id.Focus()
        '    Exit Sub
        'End If

        If (Page.IsValid) Then
            Dim DSapparea As DataSet
            Dim MyCommand As SqlDataAdapter
            Dim myappareaSQL As String
            Dim mycount As Integer

            myappareaSQL = "SELECT * FROM vw_ws_application_area WHERE application_id = "
            myappareaSQL = myappareaSQL & Chr(39) & getapplication_id.Text & Chr(39)
            myappareaSQL = myappareaSQL & "AND area_id = "
            myappareaSQL = myappareaSQL & Chr(39) & getarea_id.Text & Chr(39)

            'Open Connection
            MyCommand = New SqlDataAdapter(myappareaSQL, MyConnection)

            'fill dataset
            DSapparea = New DataSet()
            MyCommand.Fill(DSapparea, "vw_ws_application_area")
            mycount = DSapparea.Tables("vw_ws_application_area").Rows.Count

            If mycount > 0 Then
                '  If GetApplication() Then
                'If Not GetArea() Then
                '    Exit Sub
                'End If
                ' If
                getapplication_name.Text = DSapparea.Tables("vw_ws_application_area").Rows(0)("application_name")
                'getgroup_id.disabled = "true"
                'btnGet.disabled = "true"
                application_id.Text = getapplication_id.Text
                area_id.Text = getarea_id.Text
                permission_create.Focus()
            Else
                GetMessage.InnerHtml = "Invalid Application Area. " & getapplication_id.Text & " " & getarea_id.Text
                getapplication_id.ReadOnly = "false"
                getapplication_name.Text = ""
                getapplication_id.Text = ""
                'getarea_id.readonly = "false"
                'btnSubmit.disabled = "false"   
                'getarea_name.Text = ""
                getarea_id.Text = ""
                application_id.Text = "*"
                area_id.Text = "*"
                GetMessage.Style("color") = "red"
            End If

        End If

        BindGrid("area_id")
    End Sub

    Sub BindGrid(ByVal SortField As String)
        'if userid.value <> "" then
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        mySQL = "SELECT gaa.group_id, gaa.application_id, gaa.area_id, gaa.permission_create, gaa.permission_read, gaa.permission_update, gaa.permission_delete, gaa.inactive, gaa.modification_date, g.group_name AS GroupName FROM ws_group_application_area as gaa INNER JOIN vw_ws_group_master g ON gaa.group_ID = g.group_id where gaa.group_id = "
        mySQL = mySQL & Chr(39) & getgroup_id.Text & Chr(39)
        mySQL = mySQL & "AND application_id = "
        mySQL = mySQL & Chr(39) & getapplication_id.Text & Chr(39)
        'mySQL = mySQL & "AND area_id = "
        'mySQL = mySQL & Chr(39) & getarea_id.Text & Chr(39)
        If SortField <> "" Then
            If Trim(SortField) = "area_id" Then
                mySQL = mySQL & " ORDER BY " & "gaa." & Trim(SortField)
            Else
                mySQL = mySQL & " ORDER BY " & Trim(SortField)
            End If
        End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()

        'MyCommand.Fill(DS, "group_application_area")
        MyCommand.Fill(DS)

        'Bind the data grid to the default view of the Table
        'MyDataGrid.DataSource=DS.Tables("user_area_group").DefaultView
        MyGridView.DataSource = DS
        MyGridView.DataBind()

        'end if   
    End Sub

    Sub FillGrid(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        mySQL = "SELECT gaa.group_id, gaa.application_id, gaa.area_id, gaa.permission_create, gaa.permission_read, gaa.permission_update, gaa.permission_delete, gaa.inactive, gaa.modification_date, g.group_name AS GroupName FROM ws_group_application_area as gaa INNER JOIN vw_ws_group_master g ON gaa.group_ID = g.group_id where gaa.group_id = "
        mySQL = mySQL & Chr(39) & getgroup_id.Text & Chr(39)
        mySQL = mySQL & "AND application_id = "
        mySQL = mySQL & Chr(39) & getapplication_id.Text & Chr(39)
        'mySQL = mySQL & "AND area_id = "
        'mySQL = mySQL & Chr(39) & getarea_id.Text & Chr(39)
        If sort_id.Text <> "" Then
            If Trim(sort_id.Text) = "area_id" Then
                mySQL = mySQL & " ORDER BY " & "gaa." & Trim(sort_id.Text)
            Else
                mySQL = mySQL & " ORDER BY " & Trim(sort_id.Text)
            End If
        Else
            mySQL = mySQL & " ORDER BY " & "gaa.area_id"
        End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        'MyCommand.Fill(DS, "user_area_group")
        MyCommand.Fill(DS)

        'MyDataGrid.DataSource=DS.Tables("user_area_group").DefaultView
        MyGridView.DataSource = DS

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
                linkBtn.OnClientClick = "return confirm('Are you sure you want to delete this Group, Application, Area?');"
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
        Dim blnGo As Boolean = True
        Dim strdgSQL As String
        Dim mymoddate As String
        Dim beforemoddate As String
        Dim chkInactiveBoxChecked As Integer
        Dim chkpermissioncreateBoxChecked As Integer
        Dim chkpermissionReadoxChecked As Integer
        Dim chkpermissionUpdateBoxChecked As Integer
        Dim chkpermissionDeleteBoxChecked As Integer
        Dim currentRow As Integer
        Dim strapplication_id As String
        Dim strarea_id As String

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
        beforemoddate = ""

        mymoddate = Format(Now, "yyyy/MM/dd HH:mm:ss")

        If MyGridView.EditIndex < 0 Then
            Exit Sub
        End If

        Dim row As GridViewRow = MyGridView.Rows(e.RowIndex)
        'Response.Write(MyGridView.Columns.Count)

        currentRow = row.DataItemIndex

        strapplication_id = CStr(row.Cells(2).Text)
        strarea_id = CStr(row.Cells(3).Text)

        Dim dgItem As GridViewRow
        'response.write(e.Item.ItemIndex)
        dgItem = MyGridView.Rows(e.RowIndex)
        'For Each dgItem In MyDataGrid.Items
        beforemoddate = CType(dgItem.FindControl("moddate"), Label).Text
        'Next
        'response.write("beforerc = " & beforemoddate)		
        'Response.Write(Trim(MyGridView.DataKeys(MyGridView.EditIndex).Value()))
        'Response.End()

        If RecordChangedSinceRead(Trim(MyGridView.DataKeys(MyGridView.EditIndex).Value()), beforemoddate, strapplication_id, strarea_id) Then
            Message.Style("color") = "red"
            Message.InnerHtml = "The Record has been changed since it was read. Click Cancel and change the record again."
            Exit Sub
        End If

        chkInactiveBoxChecked = 0
        chkpermissioncreateBoxChecked = 0
        chkpermissionReadoxChecked = 0
        chkpermissionUpdateBoxChecked = 0
        chkpermissionDeleteBoxChecked = 0

        'Dim row As GridViewRow = MyGridView.Rows(e.RowIndex)
        'Response.Write(MyGridView.Columns.Count)

        blnGo = True
        For i = 2 To MyGridView.Columns.Count - 1
            'Response.Write("i = " & i)
            If i = 4 Then ' Create
                chkpermissioncreateBoxChecked = (CType(row.Cells(i).FindControl("edit_PermissionCreateCheckBox"), CheckBox).Checked) * -1
            ElseIf i = 5 Then ' Read
                chkpermissionReadoxChecked = (CType(row.Cells(i).FindControl("edit_PermissionReadCheckBox"), CheckBox).Checked) * -1
            ElseIf i = 6 Then ' Update
                chkpermissionUpdateBoxChecked = (CType(row.Cells(i).FindControl("edit_PermissionUpdateCheckBox"), CheckBox).Checked) * -1
            ElseIf i = 7 Then ' Delete
                chkpermissionDeleteBoxChecked = (CType(row.Cells(i).FindControl("edit_PermissionDeleteCheckBox"), CheckBox).Checked) * -1
            ElseIf i = 8 Then ' Inactive
                chkInactiveBoxChecked = (CType(row.Cells(i).FindControl("edit_InactiveCheckBox"), CheckBox).Checked) * -1
                'ElseIf i = 2 Then ' group Name
                '    groupname = Trim(CType(row.Cells(i).FindControl("groupname"), TextBox).Text)
                '    If groupname = "" Then
                '        Message.Style("color") = "red"
                '        Message.InnerHtml = "Please enter the group Name."
                '        blnGo = False
                '    Else
                '        If SQLValid_Entry(Trim(groupname)) = False Then
                '            Message.Style("color") = "red"
                '            Message.InnerHtml = "You may not use a reserved word or special char in the Lcoation Name."
                '            blnGo = False
                '        End If
                '    End If
            End If

            'chkBoxChecked = (CType(row.Cells(4).FindControl("edit_InactiveCheckBox"), CheckBox).Checked) * -1
        Next

        If chkpermissioncreateBoxChecked = 0 And chkpermissionReadoxChecked = 0 And chkpermissionUpdateBoxChecked = 0 And chkpermissionDeleteBoxChecked = 0 Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Check At least one Application Permission."
            blnGo = False
        End If

        If blnGo Then
            'Message.InnerHtml = params(0).Tostring & " " & params(1).Tostring & " " & params(2).Tostring
            strdgSQL = "UPDATE ws_group_application_area SET " &
            "permission_create = '" & Trim(chkpermissioncreateBoxChecked) & "', " &
            "permission_read = '" & Trim(chkpermissionReadoxChecked) & "', " &
            "permission_update = '" & Trim(chkpermissionUpdateBoxChecked) & "', " &
            "permission_delete = '" & Trim(chkpermissionDeleteBoxChecked) & "', " &
           "inactive = '" & Trim(chkInactiveBoxChecked) & "', " &
            "modification_date = '" & mymoddate & "', " &
            "modification_user = '" & Session("UserID") & "'" &
           " WHERE group_id = '" & Trim(MyGridView.DataKeys(MyGridView.EditIndex).Value()) & "'" &
           " AND application_id = " & Chr(39) & strapplication_id & Chr(39) &
           " AND area_id = " & Chr(39) & strarea_id & Chr(39)

            Response.Write(strdgSQL)
            'Response.End()

            'Message.InnerHtml = strdgSQL
            'Dim DS As DataSet
            Dim MyCommand As SqlCommand

            MyCommand = New SqlCommand(strdgSQL, MyConnection)
            MyCommand.Connection.Open()

            Try
                MyCommand.ExecuteNonQuery()
                Message.InnerHtml = " Group ID: " & MyGridView.DataKeys(MyGridView.EditIndex).Value() & " Application: " & strapplication_id.ToString & " Area: " & strarea_id.ToString & "<b> Record Updated</b><br>"

            Catch Exp As SqlException
                If Exp.Number = 2627 Then
                    Message.InnerHtml = "ERROR: A record already exists with the same primary key. Error: " & Exp.Number.ToString
                Else
                    Message.InnerHtml = "ERROR: Could not Update record, please ensure the fields are correctly filled out. Error: " & Exp.Number.ToString & " " & Exp.Message
                End If
                Message.Style("color") = "red"
            End Try

            MyCommand.Connection.Close()

            FillGrid(-1)
            getgroup_id.Focus()
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
        ' Fires when a Delete Button is clicked. Fires Before
        Dim currentRow As Integer
        Dim strdgSQL As String
        Dim strarea_id, strapplication_id As String

        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""

        Dim row As GridViewRow = MyGridView.Rows(e.RowIndex)
        'Response.Write(MyGridView.Columns.Count)

        Message.InnerHtml = CStr(MyGridView.Columns.Count)
        currentRow = row.DataItemIndex
        Message.InnerHtml = CStr(currentRow)

        'Trace.Write("Custom Category", cstr(e.Item.Cells(3).Text))	
        'Response.Write(row.Cells(3).Text)
        'Response.End()

        strapplication_id = CStr(row.Cells(2).Text)
        strarea_id = CStr(row.Cells(3).Text)

        strdgSQL = "DELETE FROM ws_group_application_area WHERE group_id = " & Chr(39) & MyGridView.DataKeys(e.RowIndex).Value & Chr(39)
        strdgSQL = strdgSQL & " AND application_id = " & Chr(39) & strapplication_id & Chr(39)
        strdgSQL = strdgSQL & " AND area_id = " & Chr(39) & strarea_id & Chr(39)

        ' Response.Write(strdgSQL)
        'Response.Write(MyGridView.DataKeys(e.RowIndex).Value)
        'Response.End()
        Dim MyCommand As SqlCommand

        MyCommand = New SqlCommand(strdgSQL, MyConnection)
        MyCommand.Connection.Open()

        Try
            MyCommand.ExecuteNonQuery()
            'Message.InnerHtml = "<b>Record Deleted</b><br>" & MyGridView.DataKeys(e.RowIndex).Value()
            Message.InnerHtml = " Group ID: " & MyGridView.DataKeys(e.RowIndex).Value() & " Application: " & strapplication_id.ToString & " Area: " & strarea_id.ToString & "<b> Record Deleted</b><br>"

        Catch Exp As SqlException
            'If Exp.Number = 2627 Then
            '    Message.InnerHtml = "ERROR: A record already exists with the same primary key. Error: " & Exp.Number.ToString
            'Else
            Message.InnerHtml = "ERROR: Could not Delete record, please ensure the fields are correctly filled out. Error: " & Exp.Number.ToString & " " & Exp.Message
            'End If
            Message.Style("color") = "red"
        End Try

        MyCommand.Connection.Close()

        'Added this code to stop the CurrentPageIndex value PageCount error when a record is deleted
        'and its the only item left on a page other than the first one
        If MyGridView.PageIndex >= (MyGridView.PageCount - 1) Then
            MyGridView.PageIndex = 0
            BindGrid(sort_id.Text)
            'MyGridView.PageIndex = (MyGridView.PageCount - 1)
            If MyGridView.PageCount > 0 Then
                MyGridView.PageIndex = (MyGridView.PageCount - 1)
            End If
        End If

        FillGrid()
        getgroup_id.Focus()

    End Sub

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
        'BindGrid("email_id")
        'Set the focus to control on the edited row
        'Dim rowindex As Integer = MyGridView.EditIndex
        'Dim row As GridViewRow = MyGridView.Rows(rowindex)
        ' Email
        'Dim EmailTextBox As TextBox = CType(row.FindControl("email"), TextBox)
        'EmailTextBox.Width = Unit.Parse("200px")
        'EmailTextBox.MaxLength = 100
        'EmailTextBox.Focus()
        MyGridView.Rows(E.NewEditIndex).FindControl("edit_PermissionCreateCheckBox").Focus()

    End Sub

    Sub gv_PreRender(ByVal Sender As Object, ByVal e As System.EventArgs)
        ' size the edit textboxes to an appropriate width; otherwise they get really wide 
        If MyGridView.EditIndex > -1 Then
            ' if its in edit mode, get the control, set its width and maxlength properties
            'Dim rowindex As Integer = MyGridView.EditIndex
            ' Dim row As GridViewRow = MyGridView.Rows(rowindex)

            'Area Name
            'Dim AreaNameTextBox As TextBox = CType(row.FindControl("areaname"), TextBox)
            ' AreaNameTextBox.Width = Unit.Parse("300px")
            ' AreaNameTextBox.MaxLength = 50

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

    Sub Session_Start()
        'Session.Timeout = 30 ' 30 minutes
        Session.Timeout = 525600 ' 30 minutes
    End Sub

    Sub Page_Init(ByVal Sender As Object, ByVal E As EventArgs)
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

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        ' When Enter is pressed on the User field the get button is run
        'RegisterHiddenField("_EVENTTARGET", "btnSubmit")
        ClientScript.RegisterHiddenField("_EVENTTARGET", "btnSubmit")
        ' MyGridView.Attributes.Add("bordercolor", "Black")

        ViewState("SortExpression") = "group_id ASC"

        Message.InnerHtml = ""
        GetMessage.InnerHtml = ""

        MyConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("wsConnString").ToString)

        If Not (IsPostBack) Then
            application_id.Text = "*"
            area_id.Text = "*"
            getgroup_id.Focus()
            If sort_id.Text <> "" Then
                BindGrid(sort_id.Text)
            Else
                BindGrid("area_id")
            End If
        End If

    End Sub

    Sub PageReset_Click(ByVal Sender As Object, ByVal E As EventArgs)
        getgroup_id.Text = ""
        getgroup_name.Text = ""
        getapplication_id.Text = ""
        getapplication_name.Text = ""
        group_id.Text = ""
        application_id.Text = "*"
        area_id.Text = "*"
        'area_name.Text = ""
        getgroup_id.ReadOnly = "false"
        getapplication_id.ReadOnly = "false"
        ' MyGridView.SelectedIndex = -1

        'datagrid.currentpageindex=1
        'datagrid.pagecount=2

        'btnSubmit.disabled = "false"
        permission_create.Checked = False
        permission_read.Checked = False
        permission_update.Checked = False
        permission_delete.Checked = False
        inactive.Checked = False
        'Response.Write(MyGridView.PageCount)
        'Response.Write(MyGridView.PageIndex)
        'Response.End()
        If MyGridView.PageIndex >= (MyGridView.PageCount - 1) Then
            MyGridView.PageIndex = 0
            BindGrid("area_id")
            'MyGridView.PageIndex = (MyGridView.PageCount - 1)
        End If

        getgroup_id.Focus()
        Message.InnerHtml = ""
        GetMessage.InnerHtml = ""
    End Sub

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

    Sub ApplicationReset_Click(ByVal Sender As Object, ByVal E As EventArgs)
        getapplication_id.Text = ""
        getapplication_name.Text = ""
        group_id.Text = ""
        application_id.Text = "*"
        area_id.Text = "*"
        ' area_name.Text = ""
        getgroup_id.ReadOnly = "false"
        getapplication_id.ReadOnly = "false"
        inactive.Checked = False
        MyGridView.SelectedIndex = -1

        BindGrid("area_id")

        getarea_id.Focus()
        Message.InnerHtml = ""
        GetMessage.InnerHtml = ""
    End Sub

    Sub ReturnToMainMenu(ByVal Sender As Object, ByVal E As EventArgs)
        Server.Transfer("wsmainmenu.aspx")
        '<a href="vmistocksearch.aspx"></a>
    End Sub

    Sub ValidateApplicationArea(ByVal Sender As Object, ByVal E As EventArgs)
        Message.InnerHtml = ""
        GetMessage.InnerHtml = ""
        AddMessage.InnerHtml = ""

        'if getuser_id.text = "" then
        'AddMessage.Style("color") = "red"
        'AddMessage.InnerHtml = "Please enter the User ID."
        'getuser_id.Focus()
        'exit sub
        'end if

        'if getarea_id.text = "" then
        'AddMessage.Style("color") = "red"
        'AddMessage.InnerHtml = "Please enter the Area ID."
        'getarea_id.focus
        'exit sub
        'end if

        If application_id.Text Is "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the Application ID."
            application_id.Focus()
            Exit Sub
        End If

        If area_id.Text Is "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the Area ID."
            application_id.Focus()
            Exit Sub
        End If

        'If (Page.IsValid) then
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer

        mySQL = "SELECT * FROM vw_ws_application_area WHERE application_id = "
        mySQL = mySQL & Chr(39) & application_id.Text & Chr(39)
        mySQL = mySQL & "AND area_id = " & Chr(39) & area_id.Text & Chr(39)
        'response.write(mySQL)	   
        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, " vw_ws_application_area")
        mycount = DS.Tables(" vw_ws_application_area").Rows.Count
        If mycount > 0 Then
            'application_name.Text = DS.Tables(" vw_ws_application_area").Rows(0)("application_name")
        Else
            AddMessage.InnerHtml = "Invalid Application ID / Area ID: " & application_id.Text & " " & area_id.Text
            application_id.Text = "*"
            area_id.Text = "*"
            ' appliction_name.Text = ""
            group_id.Focus()
            AddMessage.Style("color") = "red"
        End If

        'End If
        DS.Dispose()
        DS = Nothing

    End Sub

    Function RecordChangedSinceRead(ByVal strkey_id As String, ByVal beforemoddate As String, ByVal strapplication_id As String, ByVal strarea_id As String) As Boolean
        Dim DSpr As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim myprSQL As String
        Dim mycount As Integer
        Dim recordchanged As Boolean

        myprSQL = "SELECT * FROM vw_ws_group_application_area WHERE group_id = "
        myprSQL = myprSQL & Chr(39) & strkey_id & Chr(39)
        myprSQL = myprSQL & " AND application_id = "
        myprSQL = myprSQL & Chr(39) & strapplication_id & Chr(39)
        myprSQL = myprSQL & " AND area_id = "
        myprSQL = myprSQL & Chr(39) & strarea_id & Chr(39)

        'Response.Write("myprSQL = " & myprSQL)

        'Open Connection
        MyCommand = New SqlDataAdapter(myprSQL, MyConnection)

        'fill dataset
        DSpr = New DataSet()
        MyCommand.Fill(DSpr, "vw_ws_group_application_area")
        mycount = DSpr.Tables("vw_ws_group_application_area").Rows.Count

        ' Response.Write("Window date = " & beforemoddate)
        recordchanged = True
        If mycount > 0 Then
            'response.write("DB Date = " & DSpr.Tables("vw_ws_group_application_area").Rows(0)("modification_date"))		   
            'Response.Write("current date = " & DSpr.Tables("vw_ws_group_application_area").Rows(0)("modification_date"))
            'Only allow the change if someone has not changed the record inbetween
            If beforemoddate = DSpr.Tables("vw_ws_group_application_area").Rows(0)("modification_date") Then
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
        'RegisterStartupScript("closewindow", s)
        If Not (ClientScript.IsStartupScriptRegistered("closewindow")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "closewindow", s.ToString(), False)
        End If
    End Sub

    'sub OpenGroupSearch(Sender As Object, E As EventArgs)
    'area_id.visible = true

    'if getuser_id.text = "" then
    'AddMessage.Style("color") = "red"
    'AddMessage.InnerHtml = "Please enter the User ID."
    'getuser_id.focus
    'exit sub
    'end if

    'if getarea_id.text = "" then
    'AddMessage.Style("color") = "red"
    'AddMessage.InnerHtml = "Please enter the Area ID."
    'getarea_id.focus
    'exit sub
    'end if

    'btnGSubmit.disabled = "false"
    'Dim s as string
    's = "<SCRIPT language='javascript'>pickGroup("
    's = s & chr(34) & "group_id" & chr(34)
    's = s & ") </" 
    's = s & "SCRIPT>"
    'RegisterStartupScript("opengroupsearch", s)
    'javascript:pickSupport("getsupport_id");'
    'end sub

    'Sub ReturnGroup_Click(ByVal Sender As Object, ByVal E As EventArgs)
    '    Dim DS As DataSet
    '    Dim MyCommand As SqlDataAdapter
    '    Dim mySQL As String
    '    Dim mycount As Integer
    '    Dim mygroupid As String

    '    Message.InnerHtml = ""

    '    group_name.Text = ""
    '    btnGSubmit.Disabled = "true"

    '    If group_id.Text = "" Then
    '        Message.Style("color") = "red"
    '        Message.InnerHtml = "Please Choose Group ID."
    '        group_name.Text = ""
    '        group_id.Focus()
    '        Exit Sub
    '    Else
    '        mygroupid = group_id.Text
    '    End If

    '    'response.write("to " & mysupportid)		   
    '    mySQL = "SELECT group_id, group_name FROM view_group_master WHERE group_id = "
    '    mySQL = mySQL & Chr(39) & mygroupid & Chr(39)
    '    'mySQL = mySQL & " AND inactive = "
    '    'mySQL = mySQL & chr(39) & "0" & chr(39)		   
    '    'response.write(mySQL)
    '    'response.end

    '    'Open Connection
    '    MyCommand = New SqlDataAdapter(mySQL, MyConnection)

    '    'fill dataset
    '    DS = New DataSet()
    '    MyCommand.Fill(DS, "view_group_master")
    '    mycount = DS.Tables("view_group_master").Rows.Count
    '    'response.write(mycount)
    '    If mycount <= 0 Then
    '        DS.Dispose()
    '        DS = Nothing
    '        Exit Sub
    '    Else
    '        group_name.Text = DS.Tables("view_group_master").Rows(0)("group_name")
    '    End If

    '    'response.write(group_name.value)
    '    'group_id.visible = false
    '    DS.Dispose()
    '    DS = Nothing

    'End Sub

    'sub OpenUserSearch(Sender As Object, E As EventArgs)
    'btnGet.disabled = "false"
    'Dim s as string
    's = "<SCRIPT language='javascript'>pickUser("
    's = s & chr(34) & "getuser_id" & chr(34)
    's = s & ") </" 
    's = s & "SCRIPT>"
    'RegisterStartupScript("openusersearch", s)
    'javascript:pickSupport("getsupport_id");'
    'end sub

    Sub ReturnGroup_Click(ByVal Sender As Object, ByVal E As EventArgs)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer
        Dim mygroupid As String

        Message.InnerHtml = ""

        getgroup_name.Text = ""
        group_id.Text = ""
        btnGet.Disabled = "true"

        If getgroup_id.Text = "" Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Please Choose Group ID."
            getgroup_name.Text = ""
            getgroup_id.Focus()
            Exit Sub
        Else
            mygroupid = getgroup_id.Text
        End If

        'response.write("to " & mysupportid)		   
        mySQL = "SELECT group_id, group_name FROM vw_ws_group_master WHERE group_id = "
        mySQL = mySQL & Chr(39) & mygroupid & Chr(39)
        mySQL = mySQL & " AND inactive = "
        mySQL = mySQL & Chr(39) & "0" & Chr(39)
        'response.write(mySQL)
        'response.end

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_group_master")
        mycount = DS.Tables("vw_ws_group_master").Rows.Count

        If mycount <= 0 Then
            DS.Dispose()
            DS = Nothing
            Exit Sub
        Else
            getgroup_name.Text = DS.Tables("vw_ws_group_master").Rows(0)("group_name")
            getapplication_id.Focus()

            group_id.Text = getgroup_id.Text
        End If
        'getuser_id.visible = false
        DS.Dispose()
        DS = Nothing

    End Sub
    Sub PrintData(ByVal Sender As Object, ByVal E As EventArgs)
        Dim s As String
        s = "<SCRIPT language='javascript'>PrintData("
        s = s & Chr(34) & "wsuserareagroupmaint.rpt" & Chr(34)
        s = s & ") </"
        s = s & "SCRIPT>"
        'RegisterStartupScript("openprint", s)
        If Not (ClientScript.IsStartupScriptRegistered("openprint")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "openprint", s, False)
        End If
    End Sub

End Class