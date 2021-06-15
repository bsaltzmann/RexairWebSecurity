Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Public Class wsviewuserapppermission
    Inherits System.Web.UI.Page
    Public MyConnection As SqlConnection
    Sub BindGrid(ByVal SortField As String)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        'This line was added to stop the Invalid CurrentPageIndex value issue.  
        ' I commented this out because if I clicked the sort header I would get moved to the first page 
        ' instead of staying on the page I was on.
        'MyDataGrid.CurrentPageIndex=0	

        mySQL = "SELECT group_id, group_name, application_id, application_name, area_id, permission_create, permission_read, permission_update, permission_delete FROM vw_ws_user_application_permissions "
        mySQL = mySQL & "WHERE user_master_inactive = 0 AND group_master_inactive = 0 AND group_user_inactive = 0 and group_app_area_inactive = 0 AND "
        mySQL = mySQL & "NOT (permission_create = 'false' AND permission_read = 'false' AND permission_update = 'false' AND permission_delete = 'false') AND "
        mySQL = mySQL & "user_id = "
        If getuser_id.Text <> "" Then
            mySQL = mySQL & Chr(39) & getuser_id.Text & Chr(39)
        Else
            mySQL = mySQL & Chr(39) & "-1" & Chr(39)
        End If
        If SortField <> "" Then
            If Trim(SortField) = "group_id" Then
                mySQL = mySQL & " ORDER BY " & Trim(SortField)
            Else
                mySQL = mySQL & " ORDER BY " & Trim(SortField)
            End If
        End If
        'Response.Write(mySQL)
        'Response.End()

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

        mySQL = "SELECT group_id, group_name, application_id, application_name, area_id, permission_create, permission_read, permission_update, permission_delete FROM vw_ws_user_application_permissions "
        mySQL = mySQL & "WHERE user_master_inactive = 0 AND group_master_inactive = 0 AND group_user_inactive = 0 and group_app_area_inactive = 0 AND "
        mySQL = mySQL & "NOT (permission_create = 'false' AND permission_read = 'false' AND permission_update = 'false' AND permission_delete = 'false') ANd "
        mySQL = mySQL & "user_id = "
        mySQL = mySQL & Chr(39) & getuser_id.Text & Chr(39)
        If sort_id.Text <> "" Then
            If Trim(sort_id.Text) = "group_id" Then
                mySQL = mySQL & " ORDER BY " & Trim(sort_id.Text)
            Else
                mySQL = mySQL & " ORDER BY " & Trim(sort_id.Text)
            End If
        Else
            mySQL = mySQL & " ORDER BY " & "group_id"
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
        '10/31/07 BAN
        valuecompare = InStr(inputstr, "'")
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If

        '08/13/09 BAN "
        valuecompare = InStr(inputstr, Chr(34))
        If valuecompare <> 0 Then
            SQLValid_Entry = False
            Exit Function
        End If
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
            getuser_id.Text = ""
            getuser_id.Focus()
            If sort_id.Text <> "" Then
                BindGrid(sort_id.Text)
            Else
                BindGrid("group_id")
            End If
        End If

    End Sub

    Sub PageReset_Click(Sender As Object, E As EventArgs)
        getuser_id.Text = ""
        getuser_name.Text = ""

        MyGridView.SelectedIndex = -1

        BindGrid("group_id")

        Message.InnerHtml = ""
        GetMessage.InnerHtml = ""
        getuser_id.Focus()
    End Sub

    Sub ReturnToMainMenu(ByVal Sender As Object, ByVal E As EventArgs)
        Server.Transfer("wsmainmenu.aspx")
    End Sub

    Sub CloseWindow(ByVal Sender As Object, ByVal E As EventArgs)
        Dim s As String
        s = "<SCRIPT language='javascript'>window.close() </"
        s = s & "SCRIPT>"
        'RegisterStartupScript("closewindow", s)
        If Not (ClientScript.IsStartupScriptRegistered("closewindow")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "closewindow", s.ToString(), False)
        End If
    End Sub
    Sub ValidateUser(Sender As Object, E As EventArgs)
        Message.InnerHtml = ""
        GetMessage.InnerHtml = ""
        'Dim user_id As String = ""

        If getuser_id.Text = "" Then
            Message.Style("color") = "red"
            Message.InnerHtml = "Please enter the User ID."
            getuser_id.Focus()
            Exit Sub
        End If

        'If (Page.IsValid) then
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim myuserSQL As String
        Dim mycount As Integer

        myuserSQL = "SELECT user_id, user_name FROM vw_ws_user_master WHERE user_id = "
        myuserSQL = myuserSQL & Chr(39) & getuser_id.Text & Chr(39)

        'Open Connection
        MyCommand = New SqlDataAdapter(myuserSQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_ws_user_master")
        mycount = DS.Tables("vw_ws_user_master").Rows.Count
        If mycount > 0 Then
            getuser_name.Text = DS.Tables("vw_ws_user_master").Rows(0)("user_name")
        Else
            Message.InnerHtml = "Invalid User ID: " & getuser_id.Text
            getuser_id.Text = ""
            getuser_name.Text = ""
            getuser_id.Focus()
            Message.Style("color") = "red"
        End If

        'DS.Dispose()
        DS = Nothing

        'End If

        'BindGrid()
    End Sub
    Sub GetUser_Click(Sender As Object, E As EventArgs)
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
            getuser_name.Text = DS.Tables("vw_ws_user_master").Rows(0)("user_name")
        End If

        ' DS.Dispose()
        DS = Nothing
    End Sub

    Sub ClearRefresh(ByVal Sender As Object, ByVal E As EventArgs)
        getuser_id.Text = ""
        getuser_name.Text = ""
        Message.InnerHtml = ""
        BindGrid("group_id")
        getuser_id.Focus()
    End Sub

    Sub GetData(ByVal sender As Object, ByVal e As EventArgs)
        Message.InnerHtml = ""

        BindGrid("group_id")

    End Sub

    Sub WindowPrint(ByVal Sender As Object, ByVal E As EventArgs)
        Dim s As String
        gridcount.Value = 0

        gridcount.Value = MyGridView.Rows.Count

        If gridcount.Value <= 0 Then
            Message.Style("color") = "red"
            Message.InnerHtml = "No records to print. Please retrieve data first. "
            Exit Sub
        End If

        s = "<SCRIPT language='javascript'>window.print() </"
        s = s & "SCRIPT>"
        ' RegisterStartupScript("mmwindow", s)
        If Not (ClientScript.IsStartupScriptRegistered("printwindow")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "printwindow", s.ToString(), False)
        End If
    End Sub

    Sub gv_RowDataBound(ByVal Sender As Object, ByVal e As GridViewRowEventArgs)
        ' Fires when a data row is bound to data. Replaced the DataGrid ItemDataBound event
        'Dim i As Integer = e.Row.RowType()
        'If e.Row.RowType = DataControlType.DataRow Then
        'I = Convert.ToInt32(DataBinder.Eval(e.RowDataItem, "ColumnName"))
        'End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Dim linkBtn As LinkButton = CType(e.Row.Cells.Item(0).Controls.Item(2), LinkButton)
            'If linkBtn.CommandName = "Delete" Then
            '    linkBtn.OnClientClick = "return confirm('Are you sure you want to delete this group/user?');"
            '    'ElseIf linkBtn.CommandName = "Cancel" Then
            '    ' linkBtn.OnClientClick = "return confirm('Are you sure you want to cancel your changes?');"
            'End If
        End If
    End Sub

End Class