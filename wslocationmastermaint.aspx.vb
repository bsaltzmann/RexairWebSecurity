Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Security.Cryptography
Imports System.IO
Public Class locationmastermaint
    Inherits System.Web.UI.Page
    Dim MyConnection As SqlConnection
    Sub AddLocation_Click(Sender As Object, E As EventArgs)
        Dim mycreationdate As String
        Dim mynulldate As String

        mynulldate = "2000-01-01 00:00:00"
        mycreationdate = Format(Now, "yyyy-MM-dd HH:mm:ss")
        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""

        If location_id.Text = "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the Location ID."
            location_id.Focus()
            Exit Sub
            'else
            'location_id.text = location_id.text.ToLower()
        Else
            If SQLValid_Entry(Trim(location_id.Text)) = False Then
                AddMessage.Style("color") = "red"
                AddMessage.InnerHtml = "You may not use a reserved word or special char in the Location ID."
                location_id.Focus()
                Exit Sub
            End If
        End If

        If location_name.Text = "" Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "Please enter the Location Name."
            location_name.Focus()
            Exit Sub
        Else
            If SQLValid_Entry(Trim(location_name.Text)) = False Then
                AddMessage.Style("color") = "red"
                AddMessage.InnerHtml = "You may not use a reserved word or special char in the Location Name."
                location_name.Focus()
                Exit Sub
            End If
        End If

        'Do not allow users and passwords to be entered using reservered words that 
        'could be used in a SQL injection attack.		
        If SQLValid_Entry(Trim(location_id.Text)) = False Then
            AddMessage.Style("color") = "red"
            AddMessage.InnerHtml = "You may not use a reserved word or special char in a Location ID."
            location_id.Focus()
            Exit Sub
        End If

        If (Page.IsValid) Then
            Dim MyCommand As SqlCommand
            Dim mycheckbox As Integer
            Dim InsertCmd As String

            InsertCmd = "INSERT INTO ws_location_master (location_id, location_name, inactive, creation_date, creation_user, modification_date, modification_user) "
            InsertCmd = InsertCmd & "VALUES (@locationid, @locationname, @inactive, @creationdate, @creationuser, @modificationdate, @modificationuser)"

            MyCommand = New SqlCommand(InsertCmd, MyConnection)

            MyCommand.Parameters.Add(New SqlParameter("@locationid", SqlDbType.VarChar, 10))
            MyCommand.Parameters("@locationid").Value = Trim(location_id.Text)

            MyCommand.Parameters.Add(New SqlParameter("@locationname", SqlDbType.VarChar, 100))
            MyCommand.Parameters("@locationname").Value = Trim(Server.HtmlEncode(location_name.Text))

            MyCommand.Parameters.Add(New SqlParameter("@inactive", SqlDbType.VarChar, 1))
            If inactive.Checked Then
                mycheckbox = 1
            Else
                mycheckbox = 0
            End If
            MyCommand.Parameters("@inactive").Value = Server.HtmlEncode(mycheckbox)

            MyCommand.Parameters.Add(New SqlParameter("@creationdate", SqlDbType.DateTime, 8))
            MyCommand.Parameters("@creationdate").Value = Trim(Server.HtmlEncode(mycreationdate))

            MyCommand.Parameters.Add(New SqlParameter("@creationuser", SqlDbType.NVarChar, 50))
            MyCommand.Parameters("@creationuser").Value = Trim(Server.HtmlEncode(Session("UserID")))

            MyCommand.Parameters.Add(New SqlParameter("@modificationdate", SqlDbType.DateTime, 8))
            MyCommand.Parameters("@modificationdate").Value = Trim(Server.HtmlEncode(mynulldate))

            MyCommand.Parameters.Add(New SqlParameter("@modificationuser", SqlDbType.VarChar, 50))
            MyCommand.Parameters("@modificationuser").Value = Trim(Server.HtmlEncode(""))

            MyCommand.Connection.Open()

            Try
                MyCommand.ExecuteNonQuery()
                AddMessage.InnerHtml = location_id.Text.ToString() & "<b> Record Added</b><br>"
                location_id.Focus()
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

        location_id.Text = ""
        location_name.Text = ""
        inactive.Checked = False
        location_id.Focus()

        If sort_id.Text <> "" Then
            BindGrid(sort_id.Text)
        Else
            BindGrid("location_id")
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

        mySQL = "SELECT location_id, location_name, inactive, creation_date, "
        mySQL = mySQL & "creation_user, modification_date, modification_user "
        mySQL = mySQL & "FROM ws_location_master"
        If SortField <> "" Then
            mySQL = mySQL & " ORDER BY " & Trim(SortField)
        End If
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        MyCommand.Fill(DS, "ws_location_master")

        MyGridView.DataSource = DS.Tables("ws_location_master").DefaultView
        MyGridView.DataBind()
    End Sub

    Sub FillGrid(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        mySQL = "SELECT location_id, location_name, inactive, creation_date, "
        mySQL = mySQL & " creation_user, modification_date, modification_user "
        mySQL = mySQL & "FROM ws_location_master"
        If sort_id.Text <> "" Then
            mySQL = mySQL & " ORDER BY " & Trim(sort_id.Text)
        Else
            mySQL = mySQL & " ORDER BY " & "location_id"
        End If
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        MyCommand.Fill(DS, "ws_location_master")

        MyGridView.DataSource = DS.Tables("ws_location_master").DefaultView
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
                linkBtn.OnClientClick = "return confirm('Location Records will be removed in Web Security. Are you sure you want to delete this entry?');"
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
        Dim chkBoxChecked As Integer
        Dim locationname As String

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
        locationname = ""
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
        chkBoxChecked = 0
        'Message.InnerHtml = e.Item.Cells.Count.Tostring
        Dim row As GridViewRow = MyGridView.Rows(e.RowIndex)
        'Response.Write(MyGridView.Columns.Count)
        For i = 2 To MyGridView.Columns.Count - 1
            'Response.Write("i = " & i)
            If i = 3 Then ' Inactive
                chkBoxChecked = (CType(row.Cells(i).FindControl("edit_InactiveCheckBox"), CheckBox).Checked) * -1
            ElseIf i = 2 Then ' Location Name
                locationname = Trim(CType(row.Cells(i).FindControl("locationname"), TextBox).Text)
                If locationname = "" Then
                    Message.Style("color") = "red"
                    Message.InnerHtml = "Please enter the Location Name."
                    blnGo = False
                Else
                    If SQLValid_Entry(Trim(locationname)) = False Then
                        Message.Style("color") = "red"
                        Message.InnerHtml = "You may not use a reserved word or special char in the Lcoation Name."
                        blnGo = False
                    End If
                End If
            End If

        Next

        If blnGo Then
            'Message.InnerHtml = params(0).Tostring & " " & params(1).Tostring & " " & params(2).Tostring
            strdgSQL = "UPDATE ws_location_master SET " &
        "location_name = '" & Trim(locationname) & "', " &
        "modification_date = '" & mymoddate & "', " &
        "modification_user = '" & Session("UserID") & "', " &
        "inactive = '" & Trim(chkBoxChecked) & "'" &
        " WHERE location_id = '" & Trim(MyGridView.DataKeys(MyGridView.EditIndex).Value()) & "'"

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
            location_id.Focus()
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
        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""

        'If DeleteAllLocationInfo(Sender, e) = False Then ' Unable to delete User Area / User Area Group info
        '    Message.InnerHtml = "ERROR: Could not delete the User records."
        '    Message.Style("color") = "red"
        '    Exit Sub
        'End If
        If UserMasterExists(location_id.Text) = True Then
            Message.InnerHtml = "ERROR: User Master records with the Location ID exist. Could not delete the Location records."
            Message.Style("color") = "red"
            Exit Sub
        End If

        If ApplicationMasterExists(location_id.Text) = True Then
            Message.InnerHtml = "ERROR: Appliation Master records with the Location ID exist. Could not delete the Location records."
            Message.Style("color") = "red"
            Exit Sub
        End If

        Dim strdgSQL As String = "DELETE FROM ws_location_master WHERE location_id = " & Chr(39) & MyGridView.DataKeys(e.RowIndex).Value & Chr(39)
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
        location_id.Focus()

    End Sub

    Function DeleteAllApplicationInfo(ByVal Sender As Object, ByVal E As GridViewDeleteEventArgs) As Boolean
        'Dim MyCommand As SqlCommand
        'Dim mySQL As String

        DeleteAllApplicationInfo = False

        ''Delete user area group data
        'mySQL = "DELETE FROM application_master WHERE location_id = "
        'mySQL = mySQL & Chr(39) & MyGridView.DataKeys(E.RowIndex).Value() & Chr(39)
        ''Response.Write("uag = " & mySQL)
        ''Response.End()
        'MyCommand = New SqlCommand(mySQL, MyConnection)
        'MyCommand.Connection.Open()

        'Try
        '    MyCommand.ExecuteNonQuery()
        '    DeleteAllApplicationInfo = True
        'Catch Exp As SqlException
        '    DeleteAllApplicationInfo = False
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

        MyGridView.Rows(E.NewEditIndex).FindControl("locationname").Focus()

    End Sub

    Sub gv_PreRender(ByVal Sender As Object, ByVal e As System.EventArgs)
        ' size the edit textboxes to an appropriate width; otherwise they get really wide 
        If MyGridView.EditIndex > -1 Then
            ' if its in edit mode, get the control, set its width and maxlength properties
            Dim rowindex As Integer = MyGridView.EditIndex
            Dim row As GridViewRow = MyGridView.Rows(rowindex)

            'Location Name
            Dim LocationNameTextBox As TextBox = CType(row.FindControl("locationname"), TextBox)
            LocationNameTextBox.Width = Unit.Parse("200px")
            LocationNameTextBox.MaxLength = 40

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
        Session.Timeout = 30 ' 30 minutes
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
            location_id.Focus()
            If sort_id.Text <> "" Then
                BindGrid(sort_id.Text)
            Else
                BindGrid("location_id")
            End If
        End If
    End Sub

    Sub PageReset_Click(Sender As Object, E As EventArgs)
        location_id.Text = ""
        location_name.Text = ""
        inactive.Checked = False
        MyGridView.SelectedIndex = -1

        BindGrid("location_id")

        Message.InnerHtml = ""
        AddMessage.InnerHtml = ""
        location_id.Focus()
    End Sub
    Sub ReturnToMainMenu(ByVal Sender As Object, ByVal E As EventArgs)
        Server.Transfer("wsmainmenu.aspx")
        '<a href="vmistocksearch.aspx"></a>
    End Sub

    Function UserMasterExists(ByVal strlocation_id As String) As Boolean
        Dim DSum As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer
        Dim dataexists As Boolean

        mySQL = "SELECT * FROM ws_user_master WHERE location_id = "
        mySQL = mySQL & Chr(39) & strlocation_id & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DSum = New DataSet()
        MyCommand.Fill(DSum, "ws_user_master")
        mycount = DSum.Tables("ws_user_master").Rows.Count

        dataexists = False
        If mycount > 0 Then
            dataexists = True
        End If

        Return dataexists
    End Function

    Function ApplicationMasterExists(ByVal strlocation_id As String) As Boolean
        Dim DSam As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer
        Dim dataexists As Boolean

        mySQL = "SELECT * FROM application_master WHERE location_id = "
        mySQL = mySQL & Chr(39) & strlocation_id & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DSam = New DataSet()
        MyCommand.Fill(DSam, "application_master")
        mycount = DSam.Tables("application_master").Rows.Count

        dataexists = False
        If mycount > 0 Then
            dataexists = True
        End If

        Return dataexists
    End Function

    Function RecordChangedSinceRead(ByVal strkey_id As String, ByVal beforemoddate As String) As Boolean
        Dim DSpr As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim myprSQL As String
        Dim mycount As Integer
        Dim recordchanged As Boolean

        myprSQL = "SELECT * FROM ws_location_master WHERE location_id = "
        myprSQL = myprSQL & Chr(39) & strkey_id & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(myprSQL, MyConnection)

        'fill dataset
        DSpr = New DataSet()
        MyCommand.Fill(DSpr, "ws_location_master")
        mycount = DSpr.Tables("ws_location_master").Rows.Count

        'response.write("Window date = " & beforemoddate)
        recordchanged = True
        If mycount > 0 Then
            'response.write("DB Date = " & DSpr.Tables("locationmmaster").Rows(0)("modification_date"))		   

            'Only allow the change if someone has not changed the record inbetween
            If beforemoddate = DSpr.Tables("ws_location_master").Rows(0)("modification_date") Then
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
        s = s & Chr(34) & "wslocationmastermaint.rpt" & Chr(34)
        s = s & ") </"
        s = s & "SCRIPT>"

        If Not (ClientScript.IsStartupScriptRegistered("openprint")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "openprint", s, False)
        End If

        'RegisterStartupScript("openprint", s)
    End Sub

End Class