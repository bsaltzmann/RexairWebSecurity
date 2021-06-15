Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Public Class wsgroupmastersearch
    Inherits System.Web.UI.Page
    Public MyConnection As SqlConnection
    Sub Search(Sender As Object, e As GridViewDeleteEventArgs)
        Dim s As String
        Dim groupid As String

        groupid = MyGridView.DataKeys(e.RowIndex).Value()

        s = "<SCRIPT language='javascript'>"
        s = s & "window.opener.document.all['" 'Reference to original window
        s = s & Request.QueryString("src") 'Reference to original window
        s = s & "'].value ='" + groupid + "';"
        s = s & "if (window.opener.document.title == 'WS Group User Maintenance') {"
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        s = s & "}"
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS Group Application Area Maintenance') {"
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        s = s & "}"
        s = s & "else {"
        s = s & "var btn = window.opener.document.getElementById('btnGSubmit');"
        s = s & "}" ' end of else
        s = s & "}" ' end of else
        s = s & "btn.click();" 'The button name
        s = s & "window.close();"
        s = s & "</"
        s = s & "SCRIPT>"

        If Not (ClientScript.IsClientScriptBlockRegistered("search")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "search", s.ToString())
        End If
    End Sub

    'Close the Search window without choosing data
    Sub Close_Click(Sender As Object, E As EventArgs)
        Dim s As String
        Dim groupid As String

        groupid = ""

        s = "<SCRIPT language='javascript'>"
        s = s & "window.opener.document.all['" 'Reference to original window
        s = s & Request.QueryString("src") 'Reference to original window
        s = s & "'].value ='" + groupid + "';"
        s = s & "if (window.opener.document.title == 'WS Group User Maintenance') {"
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        s = s & "}"
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS Group Application Area Maintenance') {"
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        s = s & "}"
        s = s & "else {"
        s = s & "var btn = window.opener.document.getElementById('btnGSubmit');"
        s = s & "}" ' end of else
        s = s & "}" ' end of else
        s = s & "btn.click();" 'The button name
        s = s & "window.close();"
        s = s & "</"
        s = s & "SCRIPT>"

        If Not (ClientScript.IsClientScriptBlockRegistered("search")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "search", s.ToString())
        End If
    End Sub

    Sub SubmitForm()
        Dim s As String
        s = "<SCRIPT language='javascript'>submitit() </"
        s = s & "SCRIPT>"
        'RegisterStartupScript("StartUp", s)
        If Not (ClientScript.IsStartupScriptRegistered("StartUp")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "StartUp", s.ToString())
        End If
    End Sub

    Sub GetData_Click(Sender As Object, E As EventArgs)
        BindGrid()
    End Sub

    Sub BindGrid()
        'if userid.value <> "" then
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String = ""

        'This line was added to stop the Invalid CurrentPageIndex value issue
        'MyGridView.CurrentPageIndex=0	

        If getgroupid.Value = "" And getgroupname.Value = "" Then
            mySQL = "SELECT group_id, group_name, inactive FROM vw_ws_group_master"
            'mySQL = mySQL & " WHERE inactive = "
            'mySQL = mySQL & chr(39) & "0" & chr(39)
            mySQL = mySQL & " ORDER BY group_id"
        Else
            If getgroupid.Value <> "" Then
                mySQL = "SELECT group_id, group_name, inactive FROM vw_ws_group_master WHERE group_id like "
                mySQL = mySQL & Chr(39) & "%" & getgroupid.Value & "%" & Chr(39)
                'mySQL = mySQL & " AND inactive = "
                'mySQL = mySQL & chr(39) & "0" & chr(39)
                mySQL = mySQL & " ORDER BY group_id"
            ElseIf getgroupname.Value <> "" Then
                mySQL = "SELECT group_id, group_name, inactive FROM vw_ws_group_master WHERE group_name like "
                mySQL = mySQL & Chr(39) & "%" & getgroupname.Value & "%" & Chr(39)
                'mySQL = mySQL & " AND inactive = "
                'mySQL = mySQL & chr(39) & "0" & chr(39)
                mySQL = mySQL & " ORDER BY group_name"
            End If
            'response.write(mysql)
            'response.end
        End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        MyCommand.Fill(DS)

        'Bind the data grid to the default view of the Table
        'MyDataGrid.DataSource=DS.Tables("vmistockinfo").DefaultView
        MyGridView.DataSource = DS
        MyGridView.DataBind()
        DS = Nothing
        'end if   
    End Sub

    Sub FillGrid(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String = ""

        If getgroupid.Value = "" And getgroupname.Value = "" Then
            mySQL = "SELECT group_id, group_name, inactive FROM vw_ws_group_master"
            'mySQL = mySQL & " WHERE inactive = "
            'mySQL = mySQL & chr(39) & "0" & chr(39)
            mySQL = mySQL & " ORDER BY group_id"
        Else
            If getgroupid.Value <> "" Then
                mySQL = "SELECT group_id, group_name, inactive FROM vw_ws_group_master WHERE group_id like "
                mySQL = mySQL & Chr(39) & "%" & getgroupid.Value & "%" & Chr(39)
                'mySQL = mySQL & " AND inactive = "
                'mySQL = mySQL & chr(39) & "0" & chr(39)
                mySQL = mySQL & " ORDER BY group_id"
            ElseIf getgroupname.Value <> "" Then
                mySQL = "SELECT group_id, group_name, inactive FROM vw_ws_group_master WHERE group_name like "
                mySQL = mySQL & Chr(39) & "%" & getgroupname.Value & "%" & Chr(39)
                'mySQL = mySQL & " AND inactive = "
                'mySQL = mySQL & chr(39) & "0" & chr(39)
                mySQL = mySQL & " ORDER BY group_name"
            End If
            'response.write(mysql)
            'response.end
        End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        MyCommand.Fill(DS)
        'MyCommand.Fill(DS, "VMIData")

        'MyDataGrid.DataSource=DS.Tables("VMIDAta").DefaultView
        MyGridView.DataSource = DS
        If Not EditIndex.Equals(Nothing) Then
            MyGridView.EditIndex = EditIndex
        End If

        MyGridView.DataBind()
        DS = Nothing
    End Sub

    Sub Session_Start()
        'Session.Timeout = 30 ' 30 minutes
        Session.Timeout = 525600
    End Sub

    Sub Page_Init(Sender As Object, E As EventArgs)
        Dim blnCanConnect As Boolean

        If Session("LoggedIn") = "No" Or Session("UserID") = "" Then
            'response.write("<B><FONT COLOR=red>Your session timed out due to inactivity or you have not logged in originally. You must re-login or login.</FONT></B>")
            HistoryBack()
            'Session.Abandon 
            'server.transfer("wslogin.aspx")
            'CloseWindow()
            Exit Sub
        Else
            blnCanConnect = IsSQLServerOnline(System.Configuration.ConfigurationManager.AppSettings("REXAIRDatabaseName").ToString) ' the SQL server Address

            If blnCanConnect = False Then ' Can not connect to the SQL Server
                'response.write("<B><FONT COLOR=red>The Problem Report SQL Server is unavailable at this time.</FONT></B>")
                HistoryBack()
                'Session.Abandon 
                'server.transfer("wslogin.aspx")
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

    Sub Page_UnLoad(Sender As Object, E As EventArgs)
        MyConnection.Close()
    End Sub

    Sub Page_Load(Sender As Object, E As EventArgs)
        ' When Enter is pressed on the User field the get button is run
        ' RegisterHiddenField("_EVENTTARGET", "btnGet")
        ClientScript.RegisterHiddenField("_EVENTTARGET", "btnGet")
        'ViewState("SortExpression")="cust_part ASC"

        Message.InnerHtml = ""

        MyConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("wsConnString").ToString)

        If Not (IsPostBack) Then
            GroupSelect.SelectedIndex = 0 ' User ID
            getgroupid.Disabled = False
            getgroupname.Disabled = True
            getgroupid.Focus()

            BindGrid()
        End If

    End Sub

    Sub DropChange_Click(Sender As Object, E As EventArgs)
        'Message.InnerHtml = " I am here selected" & stockSelect.SelectedIndex
        getgroupid.Value = ""
        getgroupname.Value = ""

        If GroupSelect.SelectedIndex = 0 Then 'User ID
            getgroupid.Disabled = False
            getgroupname.Disabled = True
            getgroupid.Focus()
        Else
            getgroupname.Disabled = False
            getgroupid.Disabled = True
            getgroupname.Focus()
        End If

        BindGrid()

        'Message.InnerHtml = "here selected" & SupportSelect.SelectedIndex & SupportSelect.id
    End Sub

    'Sub PageReset_Click(Sender As Object, E As EventArgs)
    'getgroupid.value = ""
    'getgroupname.value= ""

    'GroupSelect.SelectedIndex = 0 ' User ID
    'getgroupid.disabled = FALSE
    'getgroupname.disabled = TRUE
    ' getgroupid.Focus()
    ' MyGridView.SelectedIndex = -1

    '	BindGrid()

    '	Message.InnerHtml = ""
    'End Sub

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

    Sub CloseWindow()
        Dim s As String
        s = "<SCRIPT language='javascript'>window.close() </"
        s = s & "SCRIPT>"
        'RegisterStartupScript("closewindow", s)
        If Not (ClientScript.IsStartupScriptRegistered("closewindow")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "closewindow", s.ToString(), False)
        End If
    End Sub

    Sub HistoryBack()
        Dim s As String
        s = "<SCRIPT language='javascript'>history.back() </"
        s = s & "SCRIPT>"
        ' RegisterStartupScript("historyback", s)
        If Not (ClientScript.IsStartupScriptRegistered("historyback")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "historyback", s.ToString(), False)
        End If
    End Sub

End Class