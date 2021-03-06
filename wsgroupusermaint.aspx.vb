Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Public Class wsgroupusermaint
    Inherits System.Web.UI.Page
    Public MyConnection As SqlConnection
    Sub AddGroupUser_Click(Sender As Object, E As EventArgs)
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

        If group_id.Text = "" Then
            Exit Sub
        End If

        If user_id Is "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the User ID."
            user_id.Focus()
            Exit Sub
        Else
            If SQLValid_Entry(Trim(user_id.Text)) = False Then
                GetMessage.Style("color") = "red"
                GetMessage.InnerHtml = "You may not use a reserved word or special char in the User ID."
                user_id.Focus()
                Exit Sub
            End If
        End If

        If (Page.IsValid) Then
            Dim MyCommand As SqlCommand
            Dim mycheckbox As Integer

            Dim InsertCmd As String = "INSERT INTO ws_group_user (group_id, user_id, inactive, creation_date, creation_user, modification_date, modification_user) VALUES (@groupid, @userid, @inactive, @creationdate, @creationuser, @modificationdate, @modificationuser)"

            MyCommand = New SqlCommand(InsertCmd, MyConnection)

            MyCommand.Parameters.Add(New SqlParameter("@groupid", SqlDbType.VarChar, 20))
            MyCommand.Parameters("@groupid").Value = Trim(Server.HtmlEncode(group_id.Text))

            MyCommand.Parameters.Add(New SqlParameter("@userid", SqlDbType.VarChar, 50))
            MyCommand.Parameters("@userid").Value = Server.HtmlEncode(user_id.Text)

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

            'AddMessage.InnerHtml = group_id.Value & " " & user_id.Value & " " & mycheckbox

            Try
                MyCommand.ExecuteNonQuery()
                AddMessage.InnerHtml = "<b> Record Added:</b><br>" & "Group " & group_id.Text.ToString() & "  User " & user_id.Text
                'AddMessage.InnerHtml = "<b> Record Added:</b><br>" & group_id.Value.ToString() & " " & series_id.Value.ToString()
                'getgroup_id.disabled = "false"
                'btnGet.disabled = "false"
                getgroup_id.Focus()
            Catch Exp As SqlException
                If Exp.Number = 2601 Or Exp.Number = 2627 Then
                    AddMessage.InnerHtml = "ERROR: A record already exists with the same primary key. Error: " & Exp.Number.ToString
                Else
                    AddMessage.InnerHtml = "ERROR: Could not Add record, please ensure the fields are correctly filled out. Error: " & Exp.Number.ToString
                End If
                AddMessage.Style("color") = "red"
            End Try

            MyCommand.Connection.Close()

        End If

        user_id.Text = "*"
        user_name.Text = ""
        inactive.Checked = False
        getgroup_id.Focus()

        If sort_id.Text <> "" Then
            BindGrid(sort_id.Text)
        Else
            BindGrid("user_id")
        End If
    End Sub

    Sub GetGroup_Click(Sender As Object, E As EventArgs)
        Message.InnerHtml = ""
        GetMessage.InnerHtml = ""
        user_id.Text = "*"
        user_name.Text = ""

        If getgroup_id.text = "" Then
            PageReset_Click(Sender, E)
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter the Group ID."
            getgroup_id.Focus()
            Exit Sub
        End If

        'If Not IsNumeric(getgroup_id.text) Then
        '    GetMessage.Style("color") = "red"
        '    GetMessage.InnerHtml = "Please enter a number in the Group ID."
        '    user_id.Focus()
        '    Exit Sub
        'End If

        If (Page.IsValid) Then
            Dim DS As DataSet
            Dim MyCommand As SqlDataAdapter
            Dim myuserSQL As String
            Dim mycount As Integer

            myuserSQL = "SELECT * FROM vw_ws_group_master WHERE group_id = "
            myuserSQL = myuserSQL & Chr(39) & getgroup_id.text & Chr(39)

            'Open Connection
            MyCommand = New SqlDataAdapter(myuserSQL, MyConnection)

            'fill dataset
            DS = New DataSet()
            MyCommand.Fill(DS, "vw_ws_group_master")
            mycount = DS.Tables("vw_ws_group_master").Rows.Count
            If mycount > 0 Then
                getgroup_name.Text = DS.Tables("vw_ws_group_master").Rows(0)("group_name")
                'getgroup_id.disabled = "true"
                'btnGet.disabled = "true"
                group_id.Text = getgroup_id.Text
                user_id.Focus()
            Else
                GetMessage.InnerHtml = "Invalid Group ID. " & getgroup_id.Text
                'getgroup_id.disabled = "false"
                'btnGet.disabled = "false"
                getgroup_name.text = ""
                getgroup_id.text = ""
                group_id.Text = ""
                GetMessage.Style("color") = "red"
                getgroup_id.Focus()
            End If

        End If

        BindGrid("user_id")
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

    Sub BindGrid(ByVal SortField As String)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        'This line was added to stop the Invalid CurrentPageIndex value issue.  
        ' I commented this out because if I clicked the sort header I would get moved to the first page 
        ' instead of staying on the page I was on.
        'MyDataGrid.CurrentPageIndex=0	

        mySQL = "SELECT gu.group_id, gu.user_id, gu.inactive, gu.modification_date, u.user_name AS UserName FROM ws_group_user AS gu INNER JOIN vw_ws_user_master AS u ON gu.user_ID = u.user_id WHERE gu.group_id = "
        If getgroup_id.text <> "" Then
            mySQL = mySQL & Chr(39) & getgroup_id.text & Chr(39)
        Else
            mySQL = mySQL & Chr(39) & "-1" & Chr(39)
        End If
        If SortField <> "" Then
            If Trim(SortField) = "user_id" Then
                mySQL = mySQL & " ORDER BY " & "gu." & Trim(SortField)
            Else
                mySQL = mySQL & " ORDER BY " & Trim(SortField)
            End If
        End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)
        DS = New DataSet()

        'MyCommand.Fill(DS, "ws_group_user)
        MyCommand.Fill(DS)
        'Step 7 Bind the data grid to the default view of the Table
        'MyDataGrid.DataSource=DS.Tables("ws_group_user").DefaultView
        MyGridView.DataSource = DS
        MyGridView.DataBind()
    End Sub

    Sub FillGrid(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        mySQL = "SELECT gu.group_id, gu.user_id, gu.inactive, gu.modification_date, u.user_name AS UserName FROM ws_group_user AS gu INNER JOIN vw_ws_user_master AS u ON gu.user_ID = u.user_id WHERE gu.group_id = "
        mySQL = mySQL & Chr(39) & getgroup_id.Text & Chr(39)
        If sort_id.Text <> "" Then
            If Trim(sort_id.Text) = "user_id" Then
                mySQL = mySQL & " ORDER BY " & "gu." & Trim(sort_id.Text)
            Else
                mySQL = mySQL & " ORDER BY " & Trim(sort_id.Text)
            End If
        Else
            mySQL = mySQL & " ORDER BY " & "gu.user_id"
        End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        'MyCommand.Fill(DS, "ws_group_user")
        MyCommand.Fill(DS)

        'MyDataGrid.DataSource=DS.Tables("ws_group_user").DefaultView
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
                linkBtn.OnClientClick = "return confirm('Are you sure you want to delete this group/user?');"
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
        'Dim intseries_id As Integer
        Dim blnGo As Boolean = True
        Dim strdgSQL As String
        Dim mymoddate As String
        Dim beforemoddate As String
        Dim chkBoxChecked As Integer
        Dim struser_id As Integer
        Dim currentRow As Integer

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
        'intseries_id = CStr(row.Cells(2).Text)
        struser_id = CStr(row.Cells(2).Text)

        Dim dgItem As GridViewRow
        'response.write(e.Item.ItemIndex)
        dgItem = MyGridView.Rows(e.RowIndex)
        'For Each dgItem In MyDataGrid.Items
        beforemoddate = CType(dgItem.FindControl("moddate"), Label).Text
        'Next
        'response.write("beforerc = " & beforemoddate)		
        'Response.Write(Trim(MyGridView.DataKeys(MyGridView.EditIndex).Value()))
        'Response.End()

        If RecordChangedSinceRead(Trim(MyGridView.DataKeys(MyGridView.EditIndex).Value()), beforemoddate, struser_id) Then
            Message.Style("color") = "red"
            Message.InnerHtml = "The Record has been changed since it was read. Click Cancel and change the record again."
            Exit Sub
        End If

        chkBoxChecked = 0

        'Dim row As GridViewRow = MyGridView.Rows(e.RowIndex)
        'Response.Write(MyGridView.Columns.Count)

        chkBoxChecked = (CType(row.Cells(4).FindControl("edit_InactiveCheckBox"), CheckBox).Checked) * -1

        'Message.InnerHtml = params(0).Tostring & " " & params(1).Tostring & " " & params(2).Tostring
        strdgSQL = "UPDATE ws_group_user SET " &
    "modification_date = '" & mymoddate & "', " &
    "modification_user = '" & Session("UserID") & "', " &
    "inactive = '" & Trim(chkBoxChecked) & "'" &
    " WHERE group_id = '" & Trim(MyGridView.DataKeys(MyGridView.EditIndex).Value()) & "'" &
    " AND user_id = " & Chr(39) & user_id & Chr(39)

        ' Response.Write(strdgSQL)
        ' Response.End()

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
        getgroup_id.Focus()
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
        Dim user_id As String
        Dim currentRow As Integer
        Dim strdgSQL As String

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
        user_id = CStr(row.Cells(2).Text)

        strdgSQL = "DELETE FROM ws_group_user where group_id = " & Chr(39) & MyGridView.DataKeys(e.RowIndex).Value & Chr(39)
        strdgSQL = strdgSQL & " AND user_id = " & Chr(39) & user_id & Chr(39)

        'Response.Write(strdgSQL)
        'Response.Write(MyGridView.DataKeys(e.RowIndex).Value)
        'Response.End()
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
        'BindGrid("support_id")
        'Set the focus to control on the edited row
        'Dim rowindex As Integer = MyGridView.EditIndex
        'Dim row As GridViewRow = MyGridView.Rows(rowindex)
        'Support Person Email
        'Dim SupportPersonEmailTextBox As TextBox = CType(row.FindControl("supportpersonemail"), TextBox)
        'SupportPersonEmailTextBox.Width = Unit.Parse("200px")
        'SupportPersonEmailTextBox.MaxLength = 100
        'SupportPersonEmailTextBox.Focus()
        MyGridView.Rows(E.NewEditIndex).FindControl("edit_InactiveCheckBox").Focus()

    End Sub

    Sub gv_PreRender(ByVal Sender As Object, ByVal e As System.EventArgs)
        ' size the edit textboxes to an appropriate width; otherwise they get really wide 
        If MyGridView.EditIndex > -1 Then
            ' if its in edit mode, get the control, set its width and maxlength properties
            'Dim rowindex As Integer = MyGridView.EditIndex
            ' Dim row As GridViewRow = MyGridView.Rows(rowindex)

            'Series Name
            'Dim SeriesNameTextBox As TextBox = CType(row.FindControl("seriesname"), TextBox)
            ' SeriesNameTextBox.Width = Unit.Parse("300px")
            ' SeriesNameTextBox.MaxLength = 50

            ' Dim SeriesADTextBox As TextBox = CType(row.FindControl("addtldata"), TextBox)
            ' SeriesADTextBox.Width = Unit.Parse("300px")
            ' SeriesADTextBox.MaxLength = 50

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
        Session.Timeout = 525600 '
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
            'objIPHost = System.Net.Dns.Resolve(ServerAddress)
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
        ' When Enter is pressed on the User field the get button is run
        ' RegisterHiddenField("_EVENTTARGET", "btnGet")
        ClientScript.RegisterHiddenField("_EVENTTARGET", "btnGet")

        ViewState("SortExpression") = "group_id ASC"

        Message.InnerHtml = ""
        GetMessage.InnerHtml = ""

        'MyConnection = New SqlConnection(ConfigurationSettings.AppSettings("wsConnString"))
        MyConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("wsConnString").ToString)

        If Not (IsPostBack) Then
            user_id.Text = "*"
            getgroup_id.Focus()
            If sort_id.Text <> "" Then
                BindGrid(sort_id.Text)
            Else
                BindGrid("user_id")
            End If
        End If

    End Sub

    Sub PageReset_Click(Sender As Object, E As EventArgs)
        getgroup_id.text = ""
        getgroup_name.text = ""
        group_id.Text = ""
        user_id.Text = "*"
        user_name.Text = ""
        'getgroup_id.disabled = "false"
        'btnGet.disabled = "false"
        inactive.Checked = False
        MyGridView.SelectedIndex = -1

        BindGrid("user_id")

        Message.InnerHtml = ""
        GetMessage.InnerHtml = ""
        getgroup_id.Focus()
    End Sub

    Sub ReturnToMainMenu(ByVal Sender As Object, ByVal E As EventArgs)
        Server.Transfer("wsmainmenu.aspx")
        '<a href="vmistocksearch.aspx"></a>
    End Sub

    Sub ValidateUser(Sender As Object, E As EventArgs)
        Message.InnerHtml = ""
        GetMessage.InnerHtml = ""
        AddMessage.InnerHtml = ""
        'Dim user_id As String = ""

        If user_id.text = "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the User ID."
            user_id.Focus()
            Exit Sub
        End If

        'If Not IsNumeric(series_id.text) Then
        '    AddMessage.Style("color") = "red"
        '    AddMessage.InnerHtml = "Please enter a number in the Series ID."
        '    series_id.Focus()
        '    Exit Sub
        'End If

        'intseries_id = Int(series_id.text)

        'If (Page.IsValid) then
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim myuserSQL As String
        Dim mycount As Integer

        myuserSQL = "SELECT * FROM vw_ws_user_master WHERE user_id = "
        myuserSQL = myuserSQL & Chr(39) & user_id.Text & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(myuserSQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_user_master")
        mycount = DS.Tables("vw_ws_user_master").Rows.Count
        If mycount > 0 Then
            user_name.Text = DS.Tables("vw_ws_user_master").Rows(0)("user_name")
        Else
            AddMessage.InnerHtml = "Invalid User ID: " & user_id.Text
            user_id.Text = "*"
            user_name.Text = ""
            user_id.Focus()
            AddMessage.Style("color") = "red"
        End If

        'DS.Dispose()
        DS = Nothing

        'End If

        'BindGrid()
    End Sub

    Function RecordChangedSinceRead(strkey_id As String, beforemoddate As String, struser_id As String) As Boolean
        Dim DSpr As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim myprSQL As String
        Dim mycount As Integer
        Dim recordchanged As Boolean

        myprSQL = "SELECT * FROM ws_group_user WHERE group_id = "
        myprSQL = myprSQL & Chr(39) & strkey_id & Chr(39)
        myprSQL = myprSQL & " AND user_id = "
        myprSQL = myprSQL & Chr(39) & struser_id & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(myprSQL, MyConnection)

        'fill dataset
        DSpr = New DataSet()
        MyCommand.Fill(DSpr, "ws_group_user")
        mycount = DSpr.Tables("ws_group_user").Rows.Count

        'response.write("Window date = " & beforemoddate)
        recordchanged = True
        If mycount > 0 Then
            'response.write("DB Date = " & DSpr.Tables("group_series").Rows(0)("modification_date"))		   

            'Only allow the change if someone has not changed the record inbetween
            If beforemoddate = DSpr.Tables("ws_group_user").Rows(0)("modification_date") Then
                'response.write("Dates equal")
                recordchanged = False
            End If
        End If

        'DSpr.Dispose()
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

    Sub ReturnUser_Click(Sender As Object, E As EventArgs)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer
        Dim myuserid As String

        Message.InnerHtml = ""

        user_name.Text = ""
        btnSubmit.Disabled = "true"

        If user_id.Text = "" Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Please Choose User ID."
            user_name.Text = ""
            user_id.Focus()
            Exit Sub
        Else
            myuserid = user_id.Text
        End If

        'response.write("to " & mysupportid)		   
        mySQL = "SELECT user_id, user_name FROM vw_ws_user_master WHERE user_id = "
        mySQL = mySQL & Chr(39) & myuserid & Chr(39)
        mySQL = mySQL & " AND inactive = "
        mySQL = mySQL & Chr(39) & "0" & Chr(39)
        'response.write(mySQL)
        'response.end

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_user_master")
        mycount = DS.Tables("vw_ws_user_master").Rows.Count

        If mycount <= 0 Then
            Exit Sub
        Else
            user_name.Text = DS.Tables("vw_ws_user_master").Rows(0)("user_name")
        End If

        ' DS.Dispose()
        DS = Nothing
    End Sub

    Sub PrintData(ByVal Sender As Object, ByVal E As EventArgs)
        Dim s As String
        s = "<SCRIPT language='javascript'>PrintData("
        s = s & Chr(34) & "wsgroupusermaint.rpt" & Chr(34)
        s = s & ") </"
        s = s & "SCRIPT>"
        ' RegisterStartupScript("openprint", s)
        If Not (ClientScript.IsStartupScriptRegistered("openprint")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "openprint", s, False)
        End If
    End Sub
End Class