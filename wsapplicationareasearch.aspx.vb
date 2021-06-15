Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Public Class wsapplicationareasearch
    Inherits System.Web.UI.Page
    Public MyConnection As SqlConnection
    Sub Search(ByVal Sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim s As String
        Dim applicationid As String
        Dim areaid As String
        Dim apparea As String
        Dim currentRow As Integer

        Dim row As GridViewRow = MyGridView.Rows(e.RowIndex)
        'Response.Write(MyGridView.Columns.Count)

        currentRow = row.DataItemIndex
        areaid = CStr(row.Cells(3).Text)
        applicationid = MyGridView.DataKeys(e.RowIndex).Value()

        apparea = applicationid & "^" & areaid

        s = "<SCRIPT language='javascript'>"
        s = s & "window.opener.document.all['" 'Reference to original window
        s = s & Request.QueryString("src") 'Reference to original window
        s = s & "'].value ='" + apparea + "';"
        s = s & "if (window.opener.document.title == 'WS Group Application Area Maintenance') {"
        s = s & "var btn = window.opener.document.getElementById('btnSubmit');"
        s = s & "}"
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS Copy User Security') {"
        If Trim(getsource.Value) = "from_application_id" Then
            s = s & "var btn = window.opener.document.getElementById('btnGet');"
        Else
            s = s & "var btn = window.opener.document.getElementById('btnTGet');"
        End If
        s = s & "}" ' end of Copy user security
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS User Area Group Maintenance') {"
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        s = s & "}"
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS User Password Change') {"
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        s = s & "}"
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS Specific User UAG View') {"
        'if trim(getsource.value) = "support_id" then
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        'else	  
        's = s & "var btn = window.opener.document.getElementById('btnTSubmit');"
        'end if
        s = s & "}"
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS Specific User UA View') {"
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        s = s & "}"
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS View User Password') {"
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        s = s & "}"
        s = s & "}"
        s = s & "}"
        s = s & "}"
        s = s & "}"
        s = s & "}"
        s = s & "}" ' end of problem report add if
        s = s & "btn.click();" 'The button name
        s = s & "window.close();"
        s = s & "</"
        s = s & "SCRIPT>"

        If Not (ClientScript.IsClientScriptBlockRegistered("search")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "search", s.ToString())
        End If

    End Sub

    Sub Close_Click(Sender As Object, E As EventArgs)
        Dim s As String
        Dim applicationid As String
        ' Dim areaid As String

        applicationid = ""
        'areaid = ""

        s = "<SCRIPT language='javascript'>"
        s = s & "window.opener.document.all['" 'Reference to original window
        s = s & Request.QueryString("src") 'Reference to original window
        s = s & "'].value ='" + applicationid + "';"
        s = s & "if (window.opener.document.title == 'WS Group Application Area Maintenance') {"
        s = s & "var btn = window.opener.document.getElementById('btnSubmit');"
        s = s & "}"
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS Copy User Security') {"
        If Trim(getsource.Value) = "from_application_id" Then
            s = s & "var btn = window.opener.document.getElementById('btnGet');"
        Else
            s = s & "var btn = window.opener.document.getElementById('btnTGet');"
        End If
        s = s & "}" ' end of Copy user security
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS User Area Group Maintenance') {"
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        s = s & "}"
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS User Password Change') {"
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        s = s & "}"
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS Specific User UAG View') {"
        'if trim(getsource.value) = "support_id" then
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        'else	  
        's = s & "var btn = window.opener.document.getElementById('btnTSubmit');"
        'end if
        s = s & "}"
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS Specific User UA View') {"
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        s = s & "}"
        s = s & "else {"
        s = s & "if (window.opener.document.title == 'WS View User Password') {"
        s = s & "var btn = window.opener.document.getElementById('btnGet');"
        s = s & "}"
        s = s & "}"
        s = s & "}"
        s = s & "}"
        s = s & "}"
        s = s & "}"
        s = s & "}" ' end of problem report add if
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
        'if applicationid.value <> "" then
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String = ""

        'This line was added to stop the Invalid CurrentPageIndex value issue
        '  MyGridView.CurrentPageIndex=0	

        If getapplicationid.Value = "" And getapplicationname.Value = "" Then
            mySQL = "SELECT application_id, application_name, area_id, inactive FROM vw_ws_application_area"
            mySQL = mySQL & " WHERE inactive = "
            mySQL = mySQL & Chr(39) & "0" & Chr(39)
            mySQL = mySQL & " ORDER BY application_id, area_id"
        Else
            If getapplicationid.Value <> "" Then
                mySQL = "SELECT application_id, application_name, area_id, inactive FROM vw_ws_application_area WHERE application_id like "
                mySQL = mySQL & Chr(39) & "%" & getapplicationid.Value & "%" & Chr(39)
                'mySQL = mySQL & " AND inactive = "
                'mySQL = mySQL & chr(39) & "0" & chr(39)
                mySQL = mySQL & " ORDER BY application_id, area_id"
            ElseIf getapplicationname.Value <> "" Then
                mySQL = "SELECT application_id, application_name, area_id, inactive FROM vw_ws_application_area WHERE application_name like "
                mySQL = mySQL & Chr(39) & "%" & getapplicationname.Value & "%" & Chr(39)
                'mySQL = mySQL & " AND inactive = "
                'mySQL = mySQL & chr(39) & "0" & chr(39)
                mySQL = mySQL & " ORDER BY application_name, area_id"
            End If
            'response.write(mysql)
            'response.end
        End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        MyCommand.Fill(DS)
        'MyCommand.Fill(DS, "wsinfo")

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

        If getapplicationid.Value = "" And getapplicationname.Value = "" Then
            mySQL = "SELECT application_id, application_name, area_id, inactive from vw_ws_application_area"
            mySQL = mySQL & " WHERE inactive = "
            mySQL = mySQL & Chr(39) & "0" & Chr(39)
            mySQL = mySQL & " ORDER BY application_id, area_id"
        Else
            If getapplicationid.Value <> "" Then
                mySQL = "SELECT application_id, application_name, area_id, inactive from vw_ws_application_area WHERE application_id like "
                mySQL = mySQL & Chr(39) & "%" & getapplicationid.Value & "%" & Chr(39)
                'mySQL = mySQL & " AND inactive = "
                'mySQL = mySQL & chr(39) & "0" & chr(39)
                mySQL = mySQL & " ORDER BY application_id, area_id"
            ElseIf getapplicationname.Value <> "" Then
                mySQL = "SELECT application_id, application_name, area_id, inactive from vw_ws_application_area WHERE application_name like "
                mySQL = mySQL & Chr(39) & "%" & getapplicationname.Value & "%" & Chr(39)
                'mySQL = mySQL & " AND inactive = "
                'mySQL = mySQL & chr(39) & "0" & chr(39)
                mySQL = mySQL & " ORDER BY application_name, area_id"
            End If
            'response.write(mysql)
            'response.end
        End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        MyCommand.Fill(DS)

        MyGridView.DataSource = DS
        If Not EditIndex.Equals(Nothing) Then
            MyGridView.EditIndex = EditIndex
        End If

        MyGridView.DataBind()
        DS = Nothing
    End Sub

    Sub Session_Start()
        Session.Timeout = 525600 ' 
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
            '    blnCanConnect = IsSQLServerOnline(ConfigurationSettings.AppSettings("REXAIRDatabaseName"))
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
        'RegisterHiddenField("_EVENTTARGET", "btnGet")
        ClientScript.RegisterHiddenField("_EVENTTARGET", "btnGet")
        'ViewState("SortExpression")="cust_part ASC"

        Message.InnerHtml = ""

        Dim s As String
        s = "<SCRIPT language='javascript'>"
        s = s & "document.wsapplicationmastersearch.getsource.value = '"
        s = s & Request.QueryString("src")
        s = s & "' </"
        s = s & "SCRIPT>"
        ' RegisterStartupScript("PageLoad", s)
        If Not (ClientScript.IsStartupScriptRegistered("PageLoad")) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "PageLoad", s.ToString(), False)
        End If

        ' MyConnection = New SqlConnection(ConfigurationSettings.AppSettings("wsConnString"))
        MyConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("wsConnString").ToString)

        If Not (IsPostBack) Then
            ApplicationSelect.SelectedIndex = 0 ' Application ID
            getapplicationid.Disabled = False
            getapplicationname.Disabled = True
            getapplicationid.Focus()

            BindGrid()
        End If

    End Sub

    Sub DropChange_Click(Sender As Object, E As EventArgs)
        'Message.InnerHtml = " I am here selected" & stockSelect.SelectedIndex
        getapplicationid.Value = ""
        getapplicationname.Value = ""

        If ApplicationSelect.SelectedIndex = 0 Then 'Application ID
            getapplicationid.Disabled = False
            getapplicationname.Disabled = True
            getapplicationid.Focus()
        Else
            getapplicationname.Disabled = False
            getapplicationid.Disabled = True
            getapplicationname.Focus()
        End If

        BindGrid()

        'Message.InnerHtml = "here selected" & SupportSelect.SelectedIndex & SupportSelect.id
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

    'Sub PageReset_Click(Sender As Object, E As EventArgs)
    '    getapplicationid.value = ""
    '	getapplicationname.value= ""

    '	UserSelect.SelectedIndex = 0 ' User ID
    '	getapplicationid.disabled = FALSE
    '    getapplicationname.Disabled = True
    '    MyGridView.SelectedIndex = -1
    '	
    'getapplicationid.focus()

    '	BindGrid()

    '	Message.InnerHtml = ""
    ' End Sub

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