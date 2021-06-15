Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Imports Microsoft.VisualBasic
Imports System.Web.UI.WebControls
Imports System.Globalization
Imports System.Text.RegularExpressions
Public Class rxviewcustomerdata
    Inherits System.Web.UI.Page
    Public MyConnection As SqlConnection
    Public sFormat As System.Globalization.DateTimeFormatInfo = New System.Globalization.DateTimeFormatInfo()

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

        'ViewState("SortExpression") = "customer_number ASC"

        'Message.InnerHtml = ""
        'GetMessage.InnerHtml = ""

        'MyConnection = New SqlConnection(ConfigurationSettings.AppSettings("wsConnString"))
        MyConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("dwConnString").ToString)

        If Not (IsPostBack) Then
            ' starting_date.Text = Format(Convert.ToDateTime(Now().AddDays(-60), sFormat), "MM/dd/yyyy")
            ' ending_date.Text = Format(Convert.ToDateTime(Now().AddDays(-0), sFormat), "MM/dd/yyyy")

            starting_date.Text = Format(Convert.ToDateTime(Now().AddDays(-60), sFormat), "yyyy-MM-dd")
            ending_date.Text = Format(Convert.ToDateTime(Now().AddDays(-0), sFormat), "yyyy-MM-dd")

            'Response.Write("Not Postback")
            customer_number.Text = ""
            customer_name.Text = ""

            getsource.Value = "Dist Sales"
            customer_number.Focus()
            'If getsource.Value = "Dist Sales" Then
            If sortdistsales_id.Text <> "" Then
                BindGridDistSales(sortdistsales_id.Text)
            Else
                BindGridDistSales("reference_date")
            End If
            'ElseIf getsource.Value = "Warranty" Then
            If sortwarranty_id.Text <> "" Then
                BindGridWarranty(sortwarranty_id.Text)
            Else
                BindGridWarranty("[Entry_____ Start Date]")
            End If
            ' ElseIf getsource.Value = "Consumer Sales" Then
            If sortconssales_id.Text <> "" Then
                BindGridConsSale(sortconssales_id.Text)
            Else
                BindGridConsSale("reporting_entry_Date")
            End If
            'Else
            'If sortdistsales_id.Text <> "" Then
            '        BindGrid(sortdistsales_id.Text)
            '    Else
            '        BindGrid("reference_date")
            ' End If
            'End If

            BindGridCust()
            BindGridRespTo()
        Else
            Response.Write("Postback")
            Response.Write(getsource.Value)
            If getsource.Value = "Dist Sales" Then
                'MyGridViewDistSales.Focus()
                Dim sd As String
                sd = "<SCRIPT language='javascript'>$('#myid li:nth-child(1) a').tab('show') // Select first tab </"
                sd = sd & "SCRIPT>"
                'RegisterStartupScript("closewindow", s)
                If Not (ClientScript.IsStartupScriptRegistered("selectDistSalesTab")) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "selectDistSalesTab", sd.ToString(), False)
                End If
            ElseIf getsource.Value = "Warranty" Then
                ' MyGridViewWarranty.Focus()
                Dim w As String
                w = "<SCRIPT language='javascript'>$('#myid li:nth-child(2) a').tab('show') // Select second tab </"
                w = w & "SCRIPT>"
                'RegisterStartupScript("closewindow", s)
                If Not (ClientScript.IsStartupScriptRegistered("selectWarrantyTab")) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "selectWarrantyTab", w.ToString(), False)
                End If
            ElseIf getsource.Value = "Consumer Sales" Then
                ' myid.Items[0].Focus()
                'Response.Write("B$ Focus")
                'MyGridViewConsSale.Focus()

                Dim s As String
                s = "<SCRIPT language='javascript'>$('#myid li:nth-child(3) a').tab('show') // Select third tab </"
                s = s & "SCRIPT>"
                'RegisterStartupScript("closewindow", s)
                If Not (ClientScript.IsStartupScriptRegistered("selectConsSalesTab")) Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "selectConsSalesTab", s.ToString(), False)
                End If
                '  myid.SelectedIndex = 0
                '  myid.Items(0).Focus()
                ' Response.Write("A$ Focus")
                'Response.End()
            End If
        End If

    End Sub
    'Sub CalendarStart_SelectionChanged(Sender As Object, E As EventArgs)
    '    starting_date.Text = CalendarStart.TodaysDate.ToShortDateString()
    'End Sub
    'Sub CalendarEnd_SelectionChanged(Sender As Object, E As EventArgs)
    '    ending_date.Text = CalendarEnd.TodaysDate.ToShortDateString()

    'End Sub

    Sub PageReset_Click(Sender As Object, E As EventArgs)
        Response.Write("PageReset_Click")
        starting_date.Text = Format(Convert.ToDateTime(Now().AddDays(-60), sFormat), "yyyy-MM-dd")
        ending_date.Text = Format(Convert.ToDateTime(Now().AddDays(-0), sFormat), "yyyy-MM-dd")

        'Response.Write("Not Postback")
        customer_number.Text = ""
        customer_name.Text = ""

        getsource.Value = "Dist Sales"
        customer_number.Focus()
        'If getsource.Value = "Dist Sales" Then
        If sortdistsales_id.Text <> "" Then
            BindGridDistSales(sortdistsales_id.Text)
        Else
            BindGridDistSales("reference_date")
        End If
        'ElseIf getsource.Value = "Warranty" Then
        If sortwarranty_id.Text <> "" Then
            BindGridWarranty(sortwarranty_id.Text)
        Else
            BindGridWarranty("[Entry_____ Start Date]")
        End If
        ' ElseIf getsource.Value = "Consumer Sales" Then
        If sortconssales_id.Text <> "" Then
            BindGridConsSale(sortconssales_id.Text)
        Else
            BindGridConsSale("reporting_entry_Date")
        End If

        If getsource.Value = "Dist Sales" Then
            'MyGridViewDistSales.Focus()
            Dim sd As String
            sd = "<SCRIPT language='javascript'>$('#myid li:nth-child(1) a').tab('show') // Select first tab </"
            sd = sd & "SCRIPT>"
            'RegisterStartupScript("closewindow", s)
            If Not (ClientScript.IsStartupScriptRegistered("selectDistSalesTab")) Then
                ClientScript.RegisterStartupScript(Me.GetType(), "selectDistSalesTab", sd.ToString(), False)
            End If
        ElseIf getsource.Value = "Warranty" Then
            ' MyGridViewWarranty.Focus()
            Dim w As String
            w = "<SCRIPT language='javascript'>$('#myid li:nth-child(2) a').tab('show') // Select second tab </"
            w = w & "SCRIPT>"
            'RegisterStartupScript("closewindow", s)
            If Not (ClientScript.IsStartupScriptRegistered("selectWarrantyTab")) Then
                ClientScript.RegisterStartupScript(Me.GetType(), "selectWarrantyTab", w.ToString(), False)
            End If
        ElseIf getsource.Value = "Consumer Sales" Then
            ' myid.Items[0].Focus()
            'Response.Write("B$ Focus")
            'MyGridViewConsSale.Focus()

            Dim s As String
            s = "<SCRIPT language='javascript'>$('#myid li:nth-child(3) a').tab('show') // Select third tab </"
            s = s & "SCRIPT>"
            'RegisterStartupScript("closewindow", s)
            If Not (ClientScript.IsStartupScriptRegistered("selectConsSalesTab")) Then
                ClientScript.RegisterStartupScript(Me.GetType(), "selectConsSalesTab", s.ToString(), False)
            End If
            '  myid.SelectedIndex = 0
            '  myid.Items(0).Focus()
            ' Response.Write("A$ Focus")
            'Response.End()
        End If

        BindGridCust()
        BindGridRespTo()

        MyGridView.SelectedIndex = -1
        MyGridViewRespTo.SelectedIndex = -1
        MyGridViewDistSales.SelectedIndex = -1
        MyGridViewWarranty.SelectedIndex = -1
        MyGridViewConsSales.SelectedIndex = -1

        GetMessage.InnerHtml = ""

    End Sub
    Sub GetData_Click(Sender As Object, E As EventArgs)
        'Response.Write("GetData_Click")
        GetMessage.InnerHtml = ""
        'customer_number.Text = ""

        If customer_number.Text = "" Then
            PageReset_Click(Sender, E)
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter a Customer Number."
            customer_name.Text = ""
            customer_number.Focus()
            Exit Sub
        End If

        If SQLValid_Entry(Trim(customer_number.Text)) = False Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "You may not use a reserved word or special char in the Customer Number."
            customer_number.Focus()
            customer_name.Text = ""
            Exit Sub
        End If

        If ValidDateFormat(starting_date.Text.ToString) = False Then
            PageReset_Click(Sender, E)
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter a the Starting Date in YYYY-MM-DD format."
            starting_date.Focus()
            Exit Sub
        End If

        If ValidDateFormat(ending_date.Text.ToString) = False Then
            PageReset_Click(Sender, E)
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter a the Ending Date in YYYY-MM-DD format."
            ending_date.Focus()
            Exit Sub
        End If

        If IsDate(starting_date.Text) = False Then
            PageReset_Click(Sender, E)
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter a Valid Starting Date."
            starting_date.Focus()
            Exit Sub
        End If

        If IsDate(ending_date.Text) = False Then
            PageReset_Click(Sender, E)
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter a Valid Ending Date."
            ending_date.Focus()
            Exit Sub
        End If

        If starting_date.Text > ending_date.Text Then
            PageReset_Click(Sender, E)
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "The Starting Date must be less than the Ending Date."
            starting_date.Focus()
            Exit Sub
        End If

        If (Page.IsValid) Then
            'getsource.Value = "Dist Sales"
            Dim DS As DataSet
            Dim MyCommand As SqlDataAdapter
            Dim mySQL As String
            Dim mycount As Integer

            mySQL = "SELECT customer_name FROM vw_AS400_Customer_Master_All WHERE company_number = 'RX' AND cust_deliv_seq = '000' AND customer_number = "
            mySQL = mySQL & Chr(39) & customer_number.Text & Chr(39)

            'Open Connection
            MyCommand = New SqlDataAdapter(mySQL, MyConnection)

            'fill dataset
            DS = New DataSet()
            MyCommand.Fill(DS, "vw_AS400_Customer_Master_All")
            mycount = DS.Tables("vw_AS400_Customer_Master_All").Rows.Count
            If mycount > 0 Then
                customer_name.Text = DS.Tables("vw_AS400_Customer_Master_All").Rows(0)("customer_name")
            Else
                GetMessage.InnerHtml = "Invalid Customer Number. " & customer_number.Text
                'getgroup_id.disabled = "false"
                'btnGet.disabled = "false"
                customer_number.Text = ""
                customer_name.Text = ""
                GetMessage.Style("color") = "red"
                customer_number.Focus()
            End If

            'Response.Write(getsource.Value)
            ' If getsource.Value = "Dist Sales" Then
            BindGridDistSales("reference_date")
            ' ElseIf getsource.Value = "Warranty" Then
            BindGridWarranty("[Entry_____ Start Date]")
            '  ElseIf getsource.Value = "Consumer Sales" Then
            BindGridConsSale("reporting_entry_Date")
            'Else
            '    BindGrid("reference_date")
            'End If

            BindGridCust()
            BindGridRespTo()
        End If

    End Sub
    Function ValidDateFormat(mydate As String) As Boolean
        ValidDateFormat = True
        'dd/MM/yyyy
        'Dim regex As Regex = New Regex("(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$")
        'yyyy-MM-dd
        'Dim regex As Regex = New Regex("(((19|20)\d\d)-(0[1-9]|1[0-2])-((0|1)[0-9]|2[0-9]|3[0-1]))$")
        Dim regex As Regex = New Regex("^\d{4}[\-\/\s]?((((0[13578])|(1[02]))[\-\/\s]?(([0-2][0-9])|(3[01])))|(((0[469])|(11))[\-\/\s]?(([0-2][0-9])|(30)))|(02[\-\/\s]?[0-2][0-9]))$")

        'Verify whether date entered in dd/MM/yyyy format.
        Dim isValid As Boolean = regex.IsMatch(mydate.Trim)
        If isValid = False Then
            ValidDateFormat = False
        End If

        ''Verify whether entered date is Valid date.
        'Dim dt As DateTime
        'isValid = DateTime.TryParseExact(mydate, "yyyy/MM/dd", New CultureInfo("en-GB"), DateTimeStyles.None, dt)
        'If Not isValid Then
        '    ValidDateFormat = False
        'End If
    End Function
    Sub oReturnFromCustomer_Click(Sender As Object, E As EventArgs)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim mycount As Integer
        Dim mycustomer_number As String

        GetMessage.InnerHtml = ""

        customer_name.Text = ""
        btnGet.Disabled = "true"

        If customer_number.Text = "" Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please Choose a Customer."
            customer_name.Text = ""
            customer_number.Focus()
            Exit Sub
        Else
            mycustomer_number = customer_number.Text
            'to_user_id.focus
        End If

        'response.write("to " & mysupportid)		   
        mySQL = "SELECT customer_name FROM vw_AS400_Customer_Master_All WHERE company_number = 'RX' AND cust_deliv_seq = '000' AND customer_number = "
        mySQL = mySQL & Chr(39) & mycustomer_number & Chr(39)
        'mySQL = mySQL & " AND inactive = "
        'mySQL = mySQL & chr(39) & "0" & chr(39)		   
        'response.write(mySQL)
        'response.end

        'Open Connection
        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        'fill dataset
        DS = New DataSet()
        MyCommand.Fill(DS, "vw_AS400_Customer_Master_All")
        mycount = DS.Tables("vw_AS400_Customer_Master_All").Rows.Count

        If mycount <= 0 Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Invalid Customer: " & customer_number.Text
            customer_number.Text = ""
            customer_name.Text = ""
            DS.Dispose()
            DS = Nothing
            customer_number.Focus()
            Exit Sub
        Else
            customer_name.Text = DS.Tables("vw_AS400_Customer_Master_All").Rows(0)("customer_name")
            btnGet.Focus()
        End If

        DS.Dispose()
        DS = Nothing
    End Sub
    Sub BindGridDistSales(ByVal SortField As String)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        'Response.Write("BindGridDistSales")

        'This line was added to stop the Invalid CurrentPageIndex value issue.  
        ' I commented this out because if I clicked the sort header I would get moved to the first page 
        ' instead of staying on the page I was on.
        'MyDataGrid.CurrentPageIndex=0	

        'If getsource.Value = "Dist Sales" Then
        mySQL = "SELECT reference_date, reference_number, item_group_major_class, from_cust_wh, from_cust_wh_name, serial_number, rgd_number, rgd_name "
        mySQL = mySQL & "FROM vw_AS400_Serial_Numbers_Sold_All "
        mySQL = mySQL & "WHERE company_number = 'RX' AND to_cust_wh = "
        If customer_number.Text <> "" Then
            mySQL = mySQL & Chr(39) & customer_number.Text & Chr(39)
        Else
            mySQL = mySQL & Chr(39) & "-1" & Chr(39)
        End If
        mySQL = mySQL & " AND reference_date >= "
        mySQL = mySQL & Chr(39) & starting_date.Text & Chr(39)
        mySQL = mySQL & " AND reference_date <= "
        mySQL = mySQL & Chr(39) & ending_date.Text & Chr(39)
        If cbrainbows_only.Checked = True Then
            mySQL = mySQL & " AND item_group_major_class = "
            mySQL = mySQL & Chr(39) & "RB" & Chr(39)
        End If

        If SortField <> "" Then
            If Trim(SortField) = "reference_date" Then
                '    mySQL = mySQL & " ORDER BY " & Trim(SortField) & " DESC"
                'Else
                mySQL = mySQL & " ORDER BY " & Trim(SortField)
            End If

            ' ElseIf Trim(SortField) = "item_group_major_class" Then
            '   mySQL = mySQL & " ORDER BY " & Trim(SortField)
            'End If
        Else
            mySQL = mySQL & " ORDER BY reference_date DESC"
        End If

        'Response.Write(mySQL)

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)
        DS = New DataSet()

        'MyCommand.Fill(DS, "ws_group_user)
        MyCommand.Fill(DS)
        'Step 7 Bind the data grid to the default view of the Table
        'MyDataGrid.DataSource=DS.Tables("ws_group_user").DefaultView
        MyGridViewDistSales.DataSource = DS
        MyGridViewDistSales.DataBind()
    End Sub
    Sub BindGridWarranty(ByVal SortField As String)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        'Response.Write("BindGridWarranty")

        'This line was added to stop the Invalid CurrentPageIndex value issue.  
        ' I commented this out because if I clicked the sort header I would get moved to the first page 
        ' instead of staying on the page I was on.
        'MyDataGrid.CurrentPageIndex=0	

        mySQL = "SELECT [Entry_____ Start Date] AS entry_start_date, [Control Closing Date] AS control_closing_date, [Serial Number] AS serial_number, "
        mySQL = mySQL & "[Country Code] AS country_code, [Return Control Number] AS return_control_number, [TAG Number] AS tag_number, [Part Number] AS part_number, [Part Description] AS part_description, "
        mySQL = mySQL & "[Line Quantity] AS line_qty, [Disposition Description] AS disposition_desc, [Warranty Denial Desc] AS denial_desc, [DaysOnMarket] AS days_onmarket, "
        mySQL = mySQL & "[MonthsOnMarket] AS months_onmarket, [MACHINE SERIES] AS machine_series, [Machine Type] AS machine_type "
        mySQL = mySQL & "FROM vw_AS400_Daily_Closed_Warranty_SRX_Pivot_Table "
        mySQL = mySQL & "WHERE [Customer Number] = "
        'mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        If customer_number.Text <> "" Then
            mySQL = mySQL & Chr(39) & customer_number.Text & Chr(39)
        Else
            mySQL = mySQL & Chr(39) & "-1" & Chr(39)
        End If
        mySQL = mySQL & " And [Entry_____ Start Date] >= "
        mySQL = mySQL & Chr(39) & starting_date.Text & Chr(39)
        mySQL = mySQL & " And [Entry_____ Start Date] <= "
        mySQL = mySQL & Chr(39) & ending_date.Text & Chr(39)

        If cbrainbows_only.Checked = True Then
            mySQL = mySQL & " And [Machine Type] = "
            mySQL = mySQL & Chr(39) & "RB" & Chr(39)
            mySQL = mySQL & " And [MACHINE SERIES] = "
            mySQL = mySQL & Chr(39) & "S4" & Chr(39)
        End If

        If SortField <> "" Then
            ' If Trim(SortField) = "[Entry_____ Start Date]" Then
            mySQL = mySQL & " ORDER BY " & Trim(SortField)
            ' End If
        Else
            mySQL = mySQL & " ORDER BY [Entry_____ Start Date] DESC"
        End If

        'Response.Write(mySQL)

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)
            DS = New DataSet()

            'MyCommand.Fill(DS, "ws_group_user)
            MyCommand.Fill(DS)
            'Step 7 Bind the data grid to the default view of the Table
            'MyDataGrid.DataSource=DS.Tables("ws_group_user").DefaultView
            MyGridViewWarranty.DataSource = DS
            MyGridViewWarranty.DataBind()

    End Sub
    Sub BindGridConsSale(ByVal SortField As String)
        'Response.Write("BindGridconsSale")
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        ' Dim validCRFICLtype As String = ""

        'validCRFICLtype = CRFandICLExists() ' Added this so that if there is a serial number that exists in both CRF and ICL that one that matches the country sold into will be valid

        'This line was added to stop the Invalid CurrentPageIndex value issue.  
        ' I commented this out because if I clicked the sort header I would get moved to the first page 
        ' instead of staying on the page I was on.
        'MyDataGrid.CurrentPageIndex=0	

        mySQL = "SELECT reporting_entry_date, sequence_number, batch_number, sale_type, power_nozzle_serial_number, country_code, country_code_name, city, "
        mySQL = mySQL & "state_prov_code, zip_code, canada_postal_code, rainbow_serial_number, region_number, region_desc, duplicate_remorse_flag "
        mySQL = mySQL & "FROM vw_AS400_CRF_And_ICL_Sales_Data_All "
        mySQL = mySQL & "WHERE company_number = 'RX' AND rgd_number = "
        'mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        If customer_number.Text <> "" Then
            mySQL = mySQL & Chr(39) & customer_number.Text & Chr(39)
        Else
            mySQL = mySQL & Chr(39) & "-1" & Chr(39)
        End If

        mySQL = mySQL & " AND reporting_entry_date >= "
        mySQL = mySQL & Chr(39) & starting_date.Text & Chr(39)
        mySQL = mySQL & " AND reporting_entry_date <= "
        mySQL = mySQL & Chr(39) & ending_date.Text & Chr(39)

        'If validCRFICLtype <> "" Then
        '    mySQL = mySQL & " AND sale_type = "
        '    mySQL = mySQL & Chr(39) & validCRFICLtype & Chr(39)
        'End If

        If SortField <> "" Then
            'If Trim(SortField) = "reporting_entry_date" Then
            mySQL = mySQL & " ORDER BY " & Trim(SortField)
            'End If
        Else
            mySQL = mySQL & " ORDER BY reporting_entry_date DESC"
        End If

        'Response.Write(mySQL)

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)
        DS = New DataSet()

        'MyCommand.Fill(DS, "ws_group_user)
        MyCommand.Fill(DS)
        'Step 7 Bind the data grid to the default view of the Table
        'MyDataGrid.DataSource=DS.Tables("ws_group_user").DefaultView
        MyGridViewConsSales.DataSource = DS
        MyGridViewConsSales.DataBind()

    End Sub

    Sub FillGridDistSales(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        'Response.Write("FillGridDistSales")

        ' If getsource.Value = "Dist Sales" Then
        mySQL = "SELECT reference_date, reference_number, item_group_major_class, from_cust_wh, from_cust_wh_name, serial_number, rgd_number, rgd_name "
        mySQL = mySQL & "FROM vw_AS400_Serial_Numbers_Sold_All "
        mySQL = mySQL & "WHERE company_number = 'RX' AND to_cust_wh = "
        mySQL = mySQL & Chr(39) & customer_number.Text & Chr(39)
        mySQL = mySQL & " AND reference_date >= "
        mySQL = mySQL & Chr(39) & starting_date.Text & Chr(39)
        mySQL = mySQL & " AND reference_date <= "
        mySQL = mySQL & Chr(39) & ending_date.Text & Chr(39)

        If cbrainbows_only.Checked = True Then
            mySQL = mySQL & " AND item_group_major_class = "
            mySQL = mySQL & Chr(39) & "RB" & Chr(39)
        End If

        If sortdistsales_id.Text <> "" Then
            ' If Trim(sortdistsales_id.Text) = "reference_date" Then
            mySQL = mySQL & " ORDER BY " & Trim(sortdistsales_id.Text)
            '  End If
        Else
            mySQL = mySQL & " ORDER BY " & "reference_date"
        End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        'MyCommand.Fill(DS, "ws_group_user")
        MyCommand.Fill(DS)

        'MyDataGrid.DataSource=DS.Tables("ws_group_user").DefaultView
        MyGridViewDistSales.DataSource = DS
        If Not EditIndex.Equals(Nothing) Then
            MyGridViewDistSales.EditIndex = EditIndex
        End If

        MyGridViewDistSales.DataBind()

    End Sub
    Sub FillGridWarranty(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        'Response.Write("FillGridWarranty")

        mySQL = "SELECT [Entry_____ Start Date] AS entry_start_date, [Control Closing Date] AS control_closing_date, [Serial Number] AS serial_number, "
        mySQL = mySQL & "[Country Code] AS country_code, [Return Control Number] AS return_control_number, [TAG Number] AS tag_number, [Part Number] AS part_number, [Part Description] AS part_description, "
        mySQL = mySQL & "[Line Quantity] AS line_qty, [Disposition Description] AS disposition_desc, [Warranty Denial Desc] AS denial_desc, [DaysOnMarket] AS days_onmarket, "
        mySQL = mySQL & "[MonthsOnMarket] AS months_onmarket, [MACHINE SERIES] AS machine_series, [Machine Type] AS machine_type "
        mySQL = mySQL & "FROM vw_AS400_Daily_Closed_Warranty_SRX_Pivot_Table "
        mySQL = mySQL & "WHERE [Customer Number] = "
        mySQL = mySQL & Chr(39) & customer_number.Text & Chr(39)
        mySQL = mySQL & " And [Entry_____ Start Date] >= "
        mySQL = mySQL & Chr(39) & starting_date.Text & Chr(39)
        mySQL = mySQL & " And [Entry_____ Start Date] <= "
        mySQL = mySQL & Chr(39) & ending_date.Text & Chr(39)

        If cbrainbows_only.Checked = True Then
            mySQL = mySQL & " And [Machine Type] = "
            mySQL = mySQL & Chr(39) & "RB" & Chr(39)
            mySQL = mySQL & " And [MACHINE SERIES] = "
            mySQL = mySQL & Chr(39) & "S4" & Chr(39)
        End If

        If sortwarranty_id.Text <> "" Then
            'If Trim(sortwarranty_id.Text) = "[Entry_____ Start Date]" Then
            mySQL = mySQL & " ORDER BY " & Trim(sortwarranty_id.Text)
            'End If
        Else
            mySQL = mySQL & " ORDER BY " & "[Entry_____ Start Date]"
        End If

            MyCommand = New SqlDataAdapter(mySQL, MyConnection)

            DS = New DataSet()
            'MyCommand.Fill(DS, "ws_group_user")
            MyCommand.Fill(DS)

            'MyDataGrid.DataSource=DS.Tables("ws_group_user").DefaultView
            MyGridViewWarranty.DataSource = DS
            If Not EditIndex.Equals(Nothing) Then
                MyGridViewWarranty.EditIndex = EditIndex
            End If

            MyGridViewWarranty.DataBind()

    End Sub
    Sub FillGridConsSale(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        ' Dim validCRFICLtype As String = ""

        'validCRFICLtype = CRFandICLExists() ' Added this so that if there is a serial number that exists in both CRF and ICL that one that matches the country sold into will be valid
        ' Response.Write("FillGridConsSales")

        mySQL = "SELECT reporting_entry_date, sequence_number, batch_number, sale_type, power_nozzle_serial_number, country_code, country_code_name, city, "
        mySQL = mySQL & "state_prov_code, zip_code, canada_postal_code, rainbow_serial_number, region_number, region_desc, duplicate_remorse_flag "
        mySQL = mySQL & "FROM vw_AS400_CRF_And_ICL_Sales_Data_All "
        mySQL = mySQL & "WHERE company_number = 'RX' AND rgd_number = "
        mySQL = mySQL & Chr(39) & customer_number.Text & Chr(39)
        mySQL = mySQL & " AND reporting_entry_date >= "
        mySQL = mySQL & Chr(39) & starting_date.Text & Chr(39)
        mySQL = mySQL & " AND reporting_entry_date <= "
        mySQL = mySQL & Chr(39) & ending_date.Text & Chr(39)

        'If validCRFICLtype <> "" Then
        '    mySQL = mySQL & " AND sale_type = "
        '    mySQL = mySQL & Chr(39) & validCRFICLtype & Chr(39)
        'End If

        If sortconssales_id.Text <> "" Then
            'If Trim(sortconssales_id.Text) = "reporting_entry_date" Then
            mySQL = mySQL & " ORDER BY " & Trim(sortconssales_id.Text)
            'End If
        Else
            mySQL = mySQL & " ORDER BY " & "reporting_entry_date"
        End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        'MyCommand.Fill(DS, "ws_group_user")
        MyCommand.Fill(DS)

        'MyDataGrid.DataSource=DS.Tables("ws_group_user").DefaultView
        MyGridViewConsSales.DataSource = DS
        If Not EditIndex.Equals(Nothing) Then
            MyGridViewConsSales.EditIndex = EditIndex
        End If

        MyGridViewConsSales.DataBind()

    End Sub
    Sub BindGridCust()
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        'Response.Write("BindGridCust")

        'This line was added to stop the Invalid CurrentPageIndex value issue.  
        ' I commented this out because if I clicked the sort header I would get moved to the first page 
        ' instead of staying on the page I was on.
        'MyDataGrid.CurrentPageIndex=0	

        mySQL = "SELECT curr_status_effective_date, customer_status, termination_date, addressline3, state, addressing_country, cust_deliv_seq, customer_class, last_purchase_date, "
        mySQL = mySQL & "responsible_to_number, responsible_to_name, rgd_number, rgd_name FROM vw_AS400_Customer_Master_All "
        mySQL = mySQL & "WHERE company_number = 'RX' AND cust_deliv_seq = '000' AND customer_number = "
        If customer_number.Text <> "" Then
            mySQL = mySQL & Chr(39) & customer_number.Text & Chr(39)
        Else
            mySQL = mySQL & Chr(39) & "-1" & Chr(39)
        End If
        mySQL = mySQL & " ORDER BY curr_status_effective_date"

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)
        DS = New DataSet()

        'MyCommand.Fill(DS, "ws_group_user)
        MyCommand.Fill(DS)
        'Step 7 Bind the data grid to the default view of the Table
        'MyDataGrid.DataSource=DS.Tables("ws_group_user").DefaultView
        MyGridView.DataSource = DS
        MyGridView.DataBind()
    End Sub
    Sub BindGridRespTo()
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        'Response.Write("BindGridRespTo")

        'This line was added to stop the Invalid CurrentPageIndex value issue.  
        ' I commented this out because if I clicked the sort header I would get moved to the first page 
        ' instead of staying on the page I was on.
        'MyDataGrid.CurrentPageIndex=0	

        mySQL = "SELECT curr_status_effective_date, customer_status, termination_date, addressline3, state, addressing_country, cust_deliv_seq, customer_class, last_purchase_date, "
        mySQL = mySQL & "customer_number, customer_name, rgd_number, rgd_name FROM vw_AS400_Customer_Master_All "
        mySQL = mySQL & "WHERE company_number = 'RX' AND cust_deliv_seq = '000' "
        mySQL = mySQL & "AND responsible_to_number = "
        If customer_number.Text <> "" Then
            mySQL = mySQL & Chr(39) & customer_number.Text & Chr(39)
        Else
            mySQL = mySQL & Chr(39) & "-1" & Chr(39)
        End If
        mySQL = mySQL & " AND (termination_date = '1900-01-01' "
        mySQL = mySQL & "AND customer_status <> 'C') OR "
        mySQL = mySQL & "(customer_status = 'C' AND termination_date >= "
        If starting_date.Text <> "" Then
            mySQL = mySQL & Chr(39) & starting_date.Text & Chr(39)
        Else
            mySQL = mySQL & Chr(39) & "1900-01-01" & Chr(39)
        End If
        mySQL = mySQL & ") "

        mySQL = mySQL & "ORDER BY customer_number"

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)
        DS = New DataSet()

        'MyCommand.Fill(DS, "ws_group_user)
        MyCommand.Fill(DS)
        'Step 7 Bind the data grid to the default view of the Table
        'MyDataGrid.DataSource=DS.Tables("ws_group_user").DefaultView
        MyGridViewRespTo.DataSource = DS
        MyGridViewRespTo.DataBind()
    End Sub
    Sub FillGridCust(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        'Response.Write("FillGridProd")

        mySQL = "SELECT curr_status_effective_date, customer_status, termination_date, addressline3, state, addressing_country, cust_deliv_seq, customer_class, last_purchase_date, "
        mySQL = mySQL & "responsible_to_number, responsible_to_name, rgd_number, rgd_name FROM vw_AS400_Customer_Master_All "
        mySQL = mySQL & "WHERE company_number = 'RX' AND cust_deliv_seq = '000' AND customer_number = "
        mySQL = mySQL & Chr(39) & customer_number.Text & Chr(39)
        mySQL = mySQL & " ORDER BY curr_status_effective_date"
        'If sort_id.Text <> "" Then
        '    If Trim(sort_id.Text) = "curr_status_effective_date" Then
        '        mySQL = mySQL & " ORDER BY " & Trim(sort_id.Text)
        '    Else
        '        mySQL = mySQL & " ORDER BY " & Trim(sort_id.Text)
        '    End If
        'Else
        '    mySQL = mySQL & " ORDER BY " & "curr_status_effective_date"
        'End If

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
    Sub FillGridRespTo(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        'Response.Write("FillGridProd")

        mySQL = "SELECT curr_status_effective_date, customer_status, termination_date, addressline3, state, addressing_country, cust_deliv_seq, customer_class, last_purchase_date, "
        mySQL = mySQL & "customer_number, customer_name, rgd_number, rgd_name FROM vw_AS400_Customer_Master_All "
        mySQL = mySQL & "WHERE company_number = 'RX' AND cust_deliv_seq = '000' "
        mySQL = mySQL & "AND responsible_to_number = "
        mySQL = mySQL & Chr(39) & customer_number.Text & Chr(39)

        mySQL = mySQL & " AND (termination_date = '1900-01-01' "
        mySQL = mySQL & "AND customer_status <> 'C') OR "
        mySQL = mySQL & "(customer_status = 'C' AND termination_date >= "
        If starting_date.Text <> "" Then
            mySQL = mySQL & Chr(39) & starting_date.Text & Chr(39)
        Else
            mySQL = mySQL & Chr(39) & "1900-01-01" & Chr(39)
        End If
        mySQL = mySQL & ") "

        mySQL = mySQL & "ORDER BY custoemr_number"
        'If sort_id.Text <> "" Then
        '    If Trim(sort_id.Text) = "curr_status_effective_date" Then
        '        mySQL = mySQL & " ORDER BY " & Trim(sort_id.Text)
        '    Else
        '        mySQL = mySQL & " ORDER BY " & Trim(sort_id.Text)
        '    End If
        'Else
        '    mySQL = mySQL & " ORDER BY " & "curr_status_effective_date"
        'End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        'MyCommand.Fill(DS, "ws_group_user")
        MyCommand.Fill(DS)

        'MyDataGrid.DataSource=DS.Tables("ws_group_user").DefaultView
        MyGridViewRespTo.DataSource = DS
        If Not EditIndex.Equals(Nothing) Then
            MyGridViewRespTo.EditIndex = EditIndex
        End If

        MyGridViewRespTo.DataBind()
    End Sub
    Function CRFandICLExists() As String
        Dim strSQL As String = ""
        Dim MySQLCommand As SqlCommand
        Dim MySQLDataReader As SqlDataReader
        Dim mycount As Integer = 0
        Dim MySQLConnection As SqlConnection

        '04/05/2021 Not needed because pulled by customer.
        'There are serial numbers that have been claimed as both CRF And ICL to consumers for Warranty purposes. Serial Number Inquiry does Not always display the correct
        'record. Spoke to Jane and this is a bug.  The following code will call a view will will pull all theh same CRF and ICL per serial number, look at country sold to and 
        'determine proper record to display

        CRFandICLExists = ""

        '---Open Connection to the Rexair Data Warehouse----------------------------------------------------
        'MySQLConnection = New SqlConnection("server=mfg41-sql;database=RexairDataWarehouse;UID=RXDW;PWD=RXDW$2020;MultipleActiveResultSets=true") ' with Reader Writer
        MySQLConnection = New SqlConnection("server=mfg41-sql;database=RexairDataWarehouse;UID=RXDWRO;PWD=RXDWRO$2020;MultipleActiveResultSets=true") ' Read Only Setup to be used for Reporting
        Try
            MySQLConnection.Open()
            'tbStatus.Text = tbStatus.Text & "Rexair Data Warehouse database opened mfg41-sql. SQLDW_DescriptionsFileDesc" & Chr(13) & Chr(10)

        Catch ex As Exception

            CRFandICLExists = ""

            MySQLConnection.Dispose()
            MySQLConnection = Nothing
            Exit Function
        End Try

        strSQL = "SELECT sold_to_country_type "
        strSQL = strSQL & "FROM vw_AS400_CRF_And_ICL_Sales_Exclude_SN_To_CRFandICL "
        strSQL = strSQL & "WHERE rainbow_serial_number = "
        'strSQL = strSQL & Chr(39) & serial_number.Text & Chr(39)

        MySQLCommand = New SqlCommand(strSQL, MySQLConnection)

        MySQLDataReader = MySQLCommand.ExecuteReader()

        Try
            'MessageBox.Show("G")
            'Loop until a valid 
            While MySQLDataReader.Read()

                'mycount = mycount + 1

                CRFandICLExists = Trim(MySQLDataReader("sold_to_country_type").ToString)
                Exit While

            End While

            'Close the Connection to SQL Rexair_Data_Warehouse
            MySQLConnection.Close()
            MySQLConnection = Nothing

            MySQLCommand.Dispose()
            MySQLDataReader.Close()
            MySQLCommand = Nothing
            MySQLDataReader = Nothing

        Catch ex As DataException
            ' tbStatus.Text = tbStatus.Text & "*ERROR SQLDW_DescriptionsFileDesc DataException Message " & CStr(ex.Message) & Chr(13) & Chr(10)
            'Close the Connection to SQL Rexair_Data_Warehouse
            MySQLConnection.Close()
            MySQLConnection = Nothing

            MySQLCommand.Dispose()
            MySQLDataReader.Close()
            MySQLCommand = Nothing
            MySQLDataReader = Nothing

            Exit Function
        Catch ex As Exception
            ' tbStatus.Text = "*ERROR SQLDW_DescriptionsFileDesc Exception Message " & CStr(ex.Message) & Chr(13) & Chr(10)
            'Close the Connection to SQL Rexair_Data_Warehouse
            MySQLConnection.Close()
            MySQLConnection = Nothing

            MySQLCommand.Dispose()
            MySQLDataReader.Close()
            MySQLCommand = Nothing
            MySQLDataReader = Nothing

            Exit Function
        End Try
    End Function
    Sub gv_RowCommand(ByVal Sender As Object, ByVal e As GridViewCommandEventArgs)
        ' Fires when any button in a button column is clicked. Replaced DataGrids ItemCommand event
        Dim i As Integer = Convert.ToInt32(e.CommandArgument)

        'Response.Write("gv_RowCommand")

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

    Sub gv_RowDataBoundDistSales(ByVal Sender As Object, ByVal e As GridViewRowEventArgs)
        'If MyGridViewDistSales.Rows.Count = 0 Then
        '    If e.Row.RowType = DataControlRowType.Footer Then

        '        Dim lbl As Label = DirectCast(e.Row.Cells(0).FindControl("Label1"), Label)

        '        lbl.Text = MyGridViewDistSales.Rows.Count.ToString()
        '    End If
        'End If


        ' Response.Write("gv_RowDataBoundDistSales")
        ' Fires when a data row is bound to data. Replaced the DataGrid ItemDataBound event
        'Dim i As Integer = e.Row.RowType()
        'If e.Row.RowType = DataControlType.DataRow Then
        'I = Convert.ToInt32(DataBinder.Eval(e.RowDataItem, "ColumnName"))
        'End If
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim linkBtn As LinkButton = CType(e.Row.Cells.Item(0).Controls.Item(2), LinkButton)
        '    If linkBtn.CommandName = "Delete" Then
        '        linkBtn.OnClientClick = "return confirm('Are you sure you want to delete this group/user?');"
        '        'ElseIf linkBtn.CommandName = "Cancel" Then
        '        ' linkBtn.OnClientClick = "return confirm('Are you sure you want to cancel your changes?');"
        '    End If
        'End If
    End Sub
    Sub gv_RowDataBoundWarranty(ByVal Sender As Object, ByVal e As GridViewRowEventArgs)

        ' Response.Write("gv_RowDataBoundWarranty")
        ' Fires when a data row is bound to data. Replaced the DataGrid ItemDataBound event
        'Dim i As Integer = e.Row.RowType()
        'If e.Row.RowType = DataControlType.DataRow Then
        'I = Convert.ToInt32(DataBinder.Eval(e.RowDataItem, "ColumnName"))
        'End If
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim linkBtn As LinkButton = CType(e.Row.Cells.Item(0).Controls.Item(2), LinkButton)
        '    If linkBtn.CommandName = "Delete" Then
        '        linkBtn.OnClientClick = "return confirm('Are you sure you want to delete this group/user?');"
        '        'ElseIf linkBtn.CommandName = "Cancel" Then
        '        ' linkBtn.OnClientClick = "return confirm('Are you sure you want to cancel your changes?');"
        '    End If
        'End If
    End Sub
    Sub gv_RowDataBoundConsSale(ByVal Sender As Object, ByVal e As GridViewRowEventArgs)

        ' Response.Write("gv_RowDataBoundConsSale")
        ' Fires when a data row is bound to data. Replaced the DataGrid ItemDataBound event
        'Dim i As Integer = e.Row.RowType()
        'If e.Row.RowType = DataControlType.DataRow Then
        'I = Convert.ToInt32(DataBinder.Eval(e.RowDataItem, "ColumnName"))
        'End If
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim linkBtn As LinkButton = CType(e.Row.Cells.Item(0).Controls.Item(2), LinkButton)
        '    If linkBtn.CommandName = "Delete" Then
        '        linkBtn.OnClientClick = "return confirm('Are you sure you want to delete this group/user?');"
        '        'ElseIf linkBtn.CommandName = "Cancel" Then
        '        ' linkBtn.OnClientClick = "return confirm('Are you sure you want to cancel your changes?');"
        '    End If
        'End If
    End Sub
    Sub gvp_RowDataBound(ByVal Sender As Object, ByVal e As GridViewRowEventArgs)
        '  Response.Write("gvp_RowDataBound")
        ' Fires when a data row is bound to data. Replaced the DataGrid ItemDataBound event
        'Dim i As Integer = e.Row.RowType()
        'If e.Row.RowType = DataControlType.DataRow Then
        'I = Convert.ToInt32(DataBinder.Eval(e.RowDataItem, "ColumnName"))
        'End If
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim linkBtn As LinkButton = CType(e.Row.Cells.Item(0).Controls.Item(2), LinkButton)
        '    If linkBtn.CommandName = "Delete" Then
        '        linkBtn.OnClientClick = "return confirm('Are you sure you want to delete this group/user?');"
        '        'ElseIf linkBtn.CommandName = "Cancel" Then
        '        ' linkBtn.OnClientClick = "return confirm('Are you sure you want to cancel your changes?');"
        '    End If
        'End If
    End Sub
    Sub gvr_RowDataBound(ByVal Sender As Object, ByVal e As GridViewRowEventArgs)
        '  Response.Write("gvp_RowDataBound")
        ' Fires when a data row is bound to data. Replaced the DataGrid ItemDataBound event
        'Dim i As Integer = e.Row.RowType()
        'If e.Row.RowType = DataControlType.DataRow Then
        'I = Convert.ToInt32(DataBinder.Eval(e.RowDataItem, "ColumnName"))
        'End If
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim linkBtn As LinkButton = CType(e.Row.Cells.Item(0).Controls.Item(2), LinkButton)
        '    If linkBtn.CommandName = "Delete" Then
        '        linkBtn.OnClientClick = "return confirm('Are you sure you want to delete this group/user?');"
        '        'ElseIf linkBtn.CommandName = "Cancel" Then
        '        ' linkBtn.OnClientClick = "return confirm('Are you sure you want to cancel your changes?');"
        '    End If
        'End If
    End Sub

    Sub gv_RowCreated(ByVal Sender As Object, ByVal e As GridViewRowEventArgs)
        'Response.Write("gv_RowCreated")
        ' Fires when a Updated Button is clicked. Fires After
        'If e.Row.RowType = DataControlRowType.Header Then
        'AddGlyph(MyGridView, e.Row)
        ' End If

    End Sub
    Sub gvp_RowCreated(ByVal Sender As Object, ByVal e As GridViewRowEventArgs)

        ' Response.Write("gvp_RowCreated")
        ' Fires when a Updated Button is clicked. Fires After
        'If e.Row.RowType = DataControlRowType.Header Then
        'AddGlyph(MyGridView, e.Row)
        ' End If

    End Sub
    Sub gvr_RowCreated(ByVal Sender As Object, ByVal e As GridViewRowEventArgs)

        ' Response.Write("gvp_RowCreated")
        ' Fires when a Updated Button is clicked. Fires After
        'If e.Row.RowType = DataControlRowType.Header Then
        'AddGlyph(MyGridView, e.Row)
        ' End If

    End Sub

    'Sub AddGlyph(ByVal grid As GridView, ByVal item As GridViewRow)
    '    'Response.Write("AddGlyph")
    '    Dim glyph As Label = New Label()
    '    Dim I As Integer

    '    glyph.EnableTheming = False
    '    glyph.Font.Name = "webdings"
    '    glyph.Font.Size = FontUnit.XSmall
    '    'glyph.Text = (grid.SortDirection = SortDirection.Ascending)
    '    glyph.Text = IIf(grid.SortDirection = SortDirection.Ascending, " 5", " 6").ToString

    '    'find the column sorted by
    '    For I = 1 To (grid.Columns.Count - 1)
    '        Dim colexpr As String = grid.Columns(I).SortExpression
    '        'Response.Write("colexpr= " & colexpr)
    '        'Response.Write("gridsort= " & grid.SortExpression)
    '        If grid.SortExpression = "" Then
    '            item.Cells(1).Controls.Add(glyph)
    '        End If
    '        If colexpr > "" And colexpr = grid.SortExpression Then
    '            'Response.Write(colexpr)
    '            item.Cells(I).Controls.Add(glyph)
    '        End If
    '    Next

    'End Sub
    Sub gv_PreRender(ByVal Sender As Object, ByVal e As System.EventArgs)
        ' Response.Write("gv_PreRender")
        ' size the edit textboxes to an appropriate width; otherwise they get really wide 
        ' If MyGridView.EditIndex > -1 Then
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

        ' End If
    End Sub
    Sub gvp_PreRender(ByVal Sender As Object, ByVal e As System.EventArgs)
        ' Response.Write("gvp_PreRender")
        ' size the edit textboxes to an appropriate width; otherwise they get really wide 
        ' If MyGridView.EditIndex > -1 Then
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

        ' End If
    End Sub
    Sub gvr_PreRender(ByVal Sender As Object, ByVal e As System.EventArgs)
        ' Response.Write("gvp_PreRender")
        ' size the edit textboxes to an appropriate width; otherwise they get really wide 
        ' If MyGridView.EditIndex > -1 Then
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

        ' End If
    End Sub

    Sub gv_PageIndexChangingDistSales(ByVal Sender As Object, ByVal E As GridViewPageEventArgs)
        'Response.Write("gv_PageIndexChangingDistSales")
        'Cancel the paging operation if the user attempts to navigate
        ' to another page while the GridView control Is in edit mode. 
        ' If getsource.Value = "Dist Sales" Then
        ' Clear the error message.
        GetMessage.InnerHtml = ""
        MyGridViewDistSales.PageIndex = E.NewPageIndex
        FillGridDistSales()


    End Sub

    Sub gv_PageIndexChangedDistSales(ByVal sender As Object, ByVal e As EventArgs)
        ' Response.Write("gv_PageIndexChangedDistSales")
        ' Call a helper method to display the current page number 
        ' when the user navigates to a different page.
        DisplayCurrentPageDistSales()

    End Sub

    Sub DisplayCurrentPageDistSales()
        Dim currentPage As Integer
        '  Response.Write("DisplayCurrentPage")

        'If getsource.Value = "Dist Sales" Then
        currentPage = MyGridViewDistSales.PageIndex + 1
        ' Display the current page number. 
        GetMessage.InnerHtml = "Page " & currentPage.ToString() & " of " &
        MyGridViewDistSales.PageCount.ToString() & "."

    End Sub
    Sub gv_PageIndexChangingWarranty(ByVal Sender As Object, ByVal E As GridViewPageEventArgs)
        'Response.Write("gv_PageIndexChangingWarranty")
        'Cancel the paging operation if the user attempts to navigate
        ' to another page while the GridView control Is in edit mode. 
        ' Clear the error message.
        GetMessage.InnerHtml = ""
        MyGridViewWarranty.PageIndex = E.NewPageIndex
        FillGridWarranty()

    End Sub

    Sub gv_PageIndexChangedWarranty(ByVal sender As Object, ByVal e As EventArgs)
        '  Response.Write("gv_PageIndexChangedWarranty")
        ' Call a helper method to display the current page number 
        ' when the user navigates to a different page.
        DisplayCurrentPageWarranty()
    End Sub

    Sub DisplayCurrentPageWarranty()
        Dim currentPage As Integer
        ' Response.Write("DisplayCurrentPageWarranty")


        currentPage = MyGridViewWarranty.PageIndex + 1
        ' Display the current page number. 
        GetMessage.InnerHtml = "Page " & currentPage.ToString() & " of " &
        MyGridViewWarranty.PageCount.ToString() & "."

    End Sub
    Sub gv_PageIndexChangingConsSale(ByVal Sender As Object, ByVal E As GridViewPageEventArgs)
        '  Response.Write("gv_PageIndexChangingConsSale")
        'Cancel the paging operation if the user attempts to navigate
        ' to another page while the GridView control Is in edit mode. 

        ' Clear the error message.
        GetMessage.InnerHtml = ""
        MyGridViewConsSales.PageIndex = E.NewPageIndex
        FillGridConsSale()

    End Sub

    Sub gv_PageIndexChangedConsSale(ByVal sender As Object, ByVal e As EventArgs)
        ' Response.Write("gv_PageIndexChangedConsSale")
        ' Call a helper method to display the current page number 
        ' when the user navigates to a different page.
        DisplayCurrentPageConsSale()
    End Sub

    Sub DisplayCurrentPageConsSale()
        Dim currentPage As Integer
        'Response.Write("DisplayCurrentPageConsSale")

        currentPage = MyGridViewConsSales.PageIndex + 1
        ' Display the current page number. 
        GetMessage.InnerHtml = "Page " & currentPage.ToString() & " of " &
        MyGridViewConsSales.PageCount.ToString() & "."

    End Sub

    Sub gv_SortingDistSales(ByVal Sender As Object, ByVal e As GridViewSortEventArgs)
        'Response.Write("gv_SortingDistSales")

        sortdistsales_id.Text = Trim(e.SortExpression)
        BindGridDistSales(e.SortExpression)

    End Sub
    Sub gv_SortingWarranty(ByVal Sender As Object, ByVal e As GridViewSortEventArgs)
        ' Response.Write("gv_SortingWarranty")

        sortwarranty_id.Text = Trim(e.SortExpression)
        BindGridWarranty(e.SortExpression)
    End Sub
    Sub gv_SortingConsSale(ByVal Sender As Object, ByVal e As GridViewSortEventArgs)
        ' Response.Write("gv_SortingConsSale")

        sortconssales_id.Text = Trim(e.SortExpression)
        BindGridConsSale(e.SortExpression)

    End Sub

    Sub gvp_PageIndexChanging(ByVal Sender As Object, ByVal E As GridViewPageEventArgs)
        ' Cancel the paging operation if the user attempts to navigate
        ' to another page while the GridView control is in edit mode. 

        ' Clear the error message.
        GetMessage.InnerHtml = ""
        MyGridView.PageIndex = E.NewPageIndex
        FillGridCust()

    End Sub

    Sub gvp_PageIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Call a helper method to display the current page number 
        ' when the user navigates to a different page.
        DisplayCurrentPageCust()
    End Sub

    Sub DisplayCurrentPageCust()
        ' Calculate the current page number.
        Dim currentPage As Integer = MyGridView.PageIndex + 1

        ' Display the current page number. 
        GetMessage.InnerHtml = "Page " & currentPage.ToString() & " of " &
        MyGridView.PageCount.ToString() & "."

    End Sub

    Sub gvp_Sorting(ByVal Sender As Object, ByVal e As GridViewSortEventArgs)
        ' sort_id.Text = Trim(e.SortExpression)
        BindGridCust()
    End Sub
    Sub gvr_PageIndexChanging(ByVal Sender As Object, ByVal E As GridViewPageEventArgs)
        ' Cancel the paging operation if the user attempts to navigate
        ' to another page while the GridView control is in edit mode. 

        ' Clear the error message.
        GetMessage.InnerHtml = ""
        MyGridViewRespTo.PageIndex = E.NewPageIndex
        FillGridRespTo()

    End Sub

    Sub gvr_PageIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Call a helper method to display the current page number 
        ' when the user navigates to a different page.
        DisplayCurrentPageRespTo()
    End Sub

    Sub DisplayCurrentPageRespTo()
        ' Calculate the current page number.
        Dim currentPage As Integer = MyGridView.PageIndex + 1

        ' Display the current page number. 
        GetMessage.InnerHtml = "Page " & currentPage.ToString() & " of " &
        MyGridViewRespTo.PageCount.ToString() & "."

    End Sub

    Sub gvr_Sorting(ByVal Sender As Object, ByVal e As GridViewSortEventArgs)
        ' sort_id.Text = Trim(e.SortExpression)
        BindGridRespTo()
    End Sub

    Sub ReturnToMainMenu(ByVal Sender As Object, ByVal E As EventArgs)
        Server.Transfer("wsmainmenu.aspx")
    End Sub
    Sub WindowExcelDistSales(ByVal Sender As Object, ByVal E As EventArgs)
        ' Excel rows limit is 1048576
        gridcountDistSales.Value = MyGridViewDistSales.Rows.Count

        If gridcountDistSales.Value <= 0 Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "No records to export. Please Search for data first. "
            Exit Sub
        End If

        If (gridcountDistSales.Value + 1) <= 1048576 Then
            Response.Clear()
            Response.Buffer = True
            ' Response.ContentType = "application/vnd.ms-excel"
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=" & "rxviewcustomerdataDistSales" & customer_number.Text.ToString & ".xls")
            'Response.AddHeader("content-disposition", String.Format("attachment; filename=rxviewcustomerdata.xls"))

            'Response.AppendHeader("content-disposition", "attachment; filename=myfile.xlsx");

            Response.Charset = ""
            'Response.Charset = "UTF-8"

            MyGridViewDistSales.EnableViewState = False
            MyGridViewDistSales.AllowSorting = False
            MyGridViewDistSales.AllowPaging = False
            MyGridViewDistSales.ShowFooter = False
            MyGridViewDistSales.RowStyle.BackColor = System.Drawing.Color.Transparent
            MyGridViewDistSales.HeaderStyle.ForeColor = System.Drawing.Color.Black
            MyGridViewDistSales.HeaderStyle.BackColor = System.Drawing.Color.Transparent
            MyGridViewDistSales.PagerStyle.BackColor = System.Drawing.Color.Transparent
            'MyGridViewDistSales.Alternating.Itemstyle.Backcolor = System.Drawing.Color.White
            'MyGridViewDistSales.BorderWidth = Unit.Pixel(0)
            MyGridViewDistSales.BackColor = System.Drawing.Color.White
            'MyGridViewDistSales.BorderColor = System.Drawing.Color.Transparent
            MyGridViewDistSales.GridLines = GridLines.Both 'gridlines in spreadsheet
            ' MyGridViewDistSales.BorderStyle = GridLines.Solid

            Dim oStringWriter As New System.IO.StringWriter()
            Dim oHtmlTextWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)

            ClearControls(MyGridViewDistSales)
            'response.end

            ' Dim tw As New System.IO.StringWriter
            ' Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            MyGridViewDistSales.RenderControl(oHtmlTextWriter)

            'Style is added dynamically
            Dim textstyle As String = "<style>.ToText { mso-number-format:\@; } </style>"
            HttpContext.Current.Response.Write(textstyle)

            ' Write the HTML back to the browser.
            HttpContext.Current.Response.Write(oStringWriter.ToString())
            ' End the response.
            HttpContext.Current.Response.End()

        Else
            HttpContext.Current.Response.Write("Too many rows - Export to Excel not possible")
            GetMessage.InnerHtml = "Too many rows - Export to Excel not possible. " & gridcountDistSales.Value.ToString
        End If

    End Sub
    Sub WindowExcelWarranty(ByVal Sender As Object, ByVal E As EventArgs)
        ' Excel rows limit is 1048576
        gridcountWarranty.Value = MyGridViewWarranty.Rows.Count

        If gridcountWarranty.Value <= 0 Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "No records to export. Please Search for data first. "
            Exit Sub
        End If

        If (gridcountWarranty.Value + 1) <= 1048576 Then
            Response.Clear()
            Response.Buffer = True
            ' Response.ContentType = "application/vnd.ms-excel"
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=" & "rxviewcustomerdataWarranty" & customer_number.Text.ToString & ".xls")
            'Response.AddHeader("content-disposition", String.Format("attachment; filename=rxviewcustomerdata.xls"))

            'Response.AppendHeader("content-disposition", "attachment; filename=myfile.xlsx");

            Response.Charset = ""
            'Response.Charset = "UTF-8"

            MyGridViewWarranty.EnableViewState = False
            MyGridViewWarranty.AllowSorting = False
            MyGridViewWarranty.AllowPaging = False
            MyGridViewWarranty.ShowFooter = False
            MyGridViewWarranty.RowStyle.BackColor = System.Drawing.Color.Transparent
            MyGridViewWarranty.HeaderStyle.ForeColor = System.Drawing.Color.Black
            MyGridViewWarranty.HeaderStyle.BackColor = System.Drawing.Color.Transparent
            MyGridViewWarranty.PagerStyle.BackColor = System.Drawing.Color.Transparent
            'MyGridViewWarranty.Alternating.Itemstyle.Backcolor = System.Drawing.Color.White
            'MyGridViewWarranty.BorderWidth = Unit.Pixel(0)
            MyGridViewWarranty.BackColor = System.Drawing.Color.White
            'MyGridViewWarranty.BorderColor = System.Drawing.Color.Transparent
            ' MyGridViewWarranty.GridLines = GridLines.Both 'gridlines in spreadsheet
            ' MyGridViewWarranty.BorderStyle = GridLines.Solid

            Dim oStringWriter As New System.IO.StringWriter()
            Dim oHtmlTextWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)

            ClearControls(MyGridViewWarranty)
            'response.end

            ' Dim tw As New System.IO.StringWriter
            ' Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            MyGridViewWarranty.RenderControl(oHtmlTextWriter)

            'Style is added dynamically
            Dim textstyle As String = "<style>.ToText { mso-number-format:\@; } </style>"
            HttpContext.Current.Response.Write(textstyle)

            ' Write the HTML back to the browser.
            HttpContext.Current.Response.Write(oStringWriter.ToString())
            ' End the response.
            HttpContext.Current.Response.End()

        Else
            HttpContext.Current.Response.Write("Too many rows - Export to Excel not possible")
            GetMessage.InnerHtml = "Too many rows - Export to Excel not possible. " & gridcountWarranty.Value.ToString
        End If

    End Sub
    Sub WindowExcelConsSales(ByVal Sender As Object, ByVal E As EventArgs)
        ' Excel rows limit is 1048576
        gridcountConsSales.Value = MyGridViewConsSales.Rows.Count

        If gridcountConsSales.Value <= 0 Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "No records to export. Please Search for data first. "
            Exit Sub
        End If

        If (gridcountConsSales.Value + 1) <= 1048576 Then
            Response.Clear()
            Response.Buffer = True
            ' Response.ContentType = "application/vnd.ms-excel"
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=" & "rxviewcustomerdataDistSales" & customer_number.Text.ToString & ".xls")
            'Response.AddHeader("content-disposition", String.Format("attachment; filename=rxviewcustomerdata.xls"))

            'Response.AppendHeader("content-disposition", "attachment; filename=myfile.xlsx");

            Response.Charset = ""
            'Response.Charset = "UTF-8"

            MyGridViewConsSales.EnableViewState = False
            MyGridViewConsSales.AllowSorting = False
            MyGridViewConsSales.AllowPaging = False
            MyGridViewConsSales.ShowFooter = False
            MyGridViewConsSales.RowStyle.BackColor = System.Drawing.Color.Transparent
            MyGridViewConsSales.HeaderStyle.ForeColor = System.Drawing.Color.Black
            MyGridViewConsSales.HeaderStyle.BackColor = System.Drawing.Color.Transparent
            MyGridViewConsSales.PagerStyle.BackColor = System.Drawing.Color.Transparent
            'MyGridViewConsSales.Alternating.Itemstyle.Backcolor = System.Drawing.Color.White
            '  MyGridViewConsSales.BorderWidth = Unit.Pixel(0)
            MyGridViewConsSales.BackColor = System.Drawing.Color.White
            'MyGridViewConsSales.BorderColor = System.Drawing.Color.Transparent
            MyGridViewConsSales.GridLines = GridLines.Both 'gridlines in spreadsheet
            ' MyGridViewConsSales.BorderStyle = GridLines.Solid

            Dim oStringWriter As New System.IO.StringWriter()
            Dim oHtmlTextWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)

            ClearControls(MyGridViewConsSales)
            'response.end

            ' Dim tw As New System.IO.StringWriter
            ' Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            MyGridViewConsSales.RenderControl(oHtmlTextWriter)

            'Style is added dynamically
            Dim textstyle As String = "<style>.ToText { mso-number-format:\@; } </style>"
            HttpContext.Current.Response.Write(textstyle)

            ' Write the HTML back to the browser.
            HttpContext.Current.Response.Write(oStringWriter.ToString())
            ' End the response.
            HttpContext.Current.Response.End()

        Else
            HttpContext.Current.Response.Write("Too many rows - Export to Excel not possible")
            GetMessage.InnerHtml = "Too many rows - Export to Excel not possible. " & gridcountConsSales.Value.ToString
        End If

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Confirms that an HtmlForm control Is rendered for the specified ASP.NET Server control at run time. 
    End Sub
    Private Sub ClearControls(ByVal control As Control)
        Dim i As Integer
        'Dim current as Control
        'response.write(" controls count = " & control.Controls.Count - 1)
        For i = control.Controls.Count - 1 To 0 Step -1
            ClearControls(control.Controls(i))
        Next i

        'current = control.Controls(i)
        If control.GetType().Name = "ImageButton" Then
            'response.write(" Imagebutton " & i)
            Dim literal As New LiteralControl()
            control.Parent.Controls.Add(literal)
            literal.Text = CStr("")
            'response.write(" Image Text = " & literal.Text)
            control.Parent.Controls.Remove(control)
        Else

            If Not TypeOf control Is TableCell Then
                If Not (control.GetType().GetProperty("SelectedItem") Is Nothing) Then
                    Dim literal As New LiteralControl()
                    control.Parent.Controls.Add(literal)
                    Try
                        literal.Text = CStr(control.GetType().GetProperty("SelectedItem").GetValue(control, Nothing))
                        'response.write(" selected Item " & literal.Text)
                    Catch
                    End Try
                    control.Parent.Controls.Remove(control)
                Else
                    If Not (control.GetType().GetProperty("Text") Is Nothing) Then
                        Dim literal As New LiteralControl()
                        control.Parent.Controls.Add(literal)
                        literal.Text = CStr(control.GetType().GetProperty("Text").GetValue(control, Nothing))
                        'response.write(" Text = " & literal.Text)
                        control.Parent.Controls.Remove(control)
                    End If
                End If
            End If
        End If
        Return
    End Sub 'ClearControls

End Class