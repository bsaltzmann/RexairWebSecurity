Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Imports Microsoft.VisualBasic
Public Class rxviewserialnumberdata
    Inherits System.Web.UI.Page
    Public MyConnection As SqlConnection
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

        ' ViewState("SortExpression") = "serial_number ASC"

        'Message.InnerHtml = ""
        'GetMessage.InnerHtml = ""

        'MyConnection = New SqlConnection(ConfigurationSettings.AppSettings("wsConnString"))
        MyConnection = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("dwConnString").ToString)

        If Not (IsPostBack) Then ' Page Just Opened
            'Response.Write("Not Postback")

            serial_number.Text = ""
            serial_number.Focus()

            BindGridDistSales("reference_date")

            BindGridWarranty("[Entry_____ Start Date]")

            BindGridConsSales("reporting_entry_date")

            BindGridProd("production_date")

        Else

        End If

    End Sub

    Sub PageReset_Click(Sender As Object, E As EventArgs)
        serial_number.Text = ""
        MyGridView.SelectedIndex = -1
        MyGridViewDistSales.SelectedIndex = -1
        MyGridViewWarranty.SelectedIndex = -1
        MyGridViewConsSales.SelectedIndex = -1

        'BindGridDistSales("reference_date")

        'BindGridWarranty("[Entry_____ Start Date]")

        'BindGridConsSales("reference_date")

        'BindGridProd("production_date")

        'GetMessage.InnerHtml = ""
        'serial_number_id.Focus()
    End Sub
    Sub GetData_Click(Sender As Object, E As EventArgs)
        GetMessage.InnerHtml = ""
        'serial_number.Text = ""

        If serial_number.Text = "" Then
            PageReset_Click(Sender, E)
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter a Serial Number."
            serial_number.Focus()
            Exit Sub
        End If

        If Not IsNumeric(serial_number.Text) Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter only numbers in the Serial Number field."
            serial_number.Focus()
            Exit Sub
        End If

        If Len(serial_number.Text) <> 9 Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter a number with a length of 9 in the Serial Number field."
            serial_number.Focus()
            Exit Sub
        End If

        If serial_number.Text = "000000000" Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "Please enter valid Serial Number."
            serial_number.Focus()
            Exit Sub
        End If

        If SQLValid_Entry(Trim(serial_number.Text)) = False Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "You may not use a reserved word or special char in the Serial Number."
            serial_number.Focus()
            Exit Sub
        End If

        'If Mid(serial_number.Text, 1, 3) = "022" Then
        '    GetMessage.Style("color") = "red"
        '    GetMessage.InnerHtml = "SRX Serial numbers must start with '022'."
        '    serial_number.Focus()
        '    Exit Sub
        'End If

        If (Page.IsValid) Then
            'getsource.Value = "Dist Sales"
            'Dim DS As DataSet
            'Dim MyCommand As SqlDataAdapter
            'Dim myuserSQL As String
            'Dim mycount As Integer

            'myuserSQL = "SELECT * FROM vw_ws_group_master WHERE group_id = "
            'myuserSQL = myuserSQL & Chr(39) & getgroup_id.text & Chr(39)

            ''Open Connection
            'MyCommand = New SqlDataAdapter(myuserSQL, MyConnection)

            ''fill dataset
            'DS = New DataSet()
            'MyCommand.Fill(DS, "vw_ws_group_master")
            'mycount = DS.Tables("vw_ws_group_master").Rows.Count
            'If mycount > 0 Then
            '    getgroup_name.Text = DS.Tables("vw_ws_group_master").Rows(0)("group_name")
            '    'getgroup_id.disabled = "true"
            '    'btnGet.disabled = "true"
            '    group_id.Text = getgroup_id.Text
            '    user_id.Focus()
            'Else
            '    GetMessage.InnerHtml = "Invalid Group ID. " & getgroup_id.Text
            '    'getgroup_id.disabled = "false"
            '    'btnGet.disabled = "false"
            '    getgroup_name.text = ""
            '    getgroup_id.text = ""
            '    group_id.Text = ""
            '    GetMessage.Style("color") = "red"
            '    getgroup_id.Focus()
            'End If

            BindGridDistSales("reference_date")

            BindGridWarranty("[Entry_____ Start Date]")

            BindGridConsSales("reporting_entry_Date")

            BindGridProd("production_date")
        End If


    End Sub
    Sub BindGridDistSales(ByVal SortField As String)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        mySQL = "SELECT reference_date, reference_number, item_group_major_class, from_cust_wh, from_cust_wh_name, to_cust_wh, to_cust_wh_name, rgd_number, rgd_name FROM vw_AS400_Serial_Numbers_Sold_All "
        mySQL = mySQL & "WHERE company_number = 'RX' AND serial_number = "
        'mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        If serial_number.Text <> "" Then
            mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        Else
            mySQL = mySQL & Chr(39) & "-1" & Chr(39)
        End If
        If SortField <> "" Then
            If Trim(SortField) = "reference_date" Then
                mySQL = mySQL & " ORDER BY " & Trim(SortField)
            End If
        End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)
        DS = New DataSet()

        'MyCommand.Fill(DS, "vw_AS400_Serial_Numbers_Sold_All")
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

        mySQL = "SELECT [Entry_____ Start Date] AS entry_start_date, [Control Closing Date] as control_closing_date, [Customer Number] as customer_number, [Customer Name] as customer_name, "
        mySQL = mySQL & "[Country Code] as country_code, [Return Control Number] as return_control_number, [Tag Number] AS tag_number, [Part Number] as part_number, [Part Description] as part_description, "
        mySQL = mySQL & "[Line Quantity] as line_qty, [Disposition Description] as disposition_desc, [Warranty Denial Desc] as denial_desc, [DaysOnMarket] as days_onmarket, "
        mySQL = mySQL & "[MonthsOnMarket] as months_onmarket FROM vw_AS400_Daily_Closed_Warranty_SRX_Pivot_Table "
        mySQL = mySQL & "WHERE [Serial Number] = "
        'mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        If serial_number.Text <> "" Then
            mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        Else
            mySQL = mySQL & Chr(39) & "-1" & Chr(39)
        End If
        If SortField <> "" Then
            If Trim(SortField) = "[Entry_____ Start Date]" Then
                mySQL = mySQL & " ORDER BY " & Trim(SortField)
            End If
        End If

        ' Response.Write(mySQL)

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)
        DS = New DataSet()

        'MyCommand.Fill(DS, "vw_AS400_Daily_Closed_Warranty_SRX_Pivot_Table")
        MyCommand.Fill(DS)
        'Step 7 Bind the data grid to the default view of the Table
        'MyDataGrid.DataSource=DS.Tables("vw_AS400_Daily_Closed_Warranty_SRX_Pivot_Table").DefaultView
        MyGridViewWarranty.DataSource = DS
        MyGridViewWarranty.DataBind()
    End Sub
    Sub BindGridConsSales(ByVal SortField As String)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim validCRFICLtype As String = ""

        validCRFICLtype = CRFandICLExists() ' Added this so that if there is a serial number that exists in both CRF and ICL that one that matches the country sold into will be valid

        mySQL = "SELECT reporting_entry_date, sequence_number, batch_number, sale_type, power_nozzle_serial_number, country_code, country_code_name, city, "
        mySQL = mySQL & "state_prov_code, zip_code, canada_postal_code, rgd_number, rgd_name, region_number, region_desc, duplicate_remorse_flag "
        mySQL = mySQL & "FROM vw_AS400_CRF_And_ICL_Sales_Data_All "
        mySQL = mySQL & "WHERE company_number = 'RX' "

        'mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        If serial_number.Text <> "" Then
            If Mid(serial_number.Text, 1, 3) = "042" Then ' This is a Power Nozzle SN
                mySQL = mySQL & "AND power_nozzle_serial_number = "
                mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
            Else
                mySQL = mySQL & "AND rainbow_serial_number = "
                mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
            End If
        Else
            mySQL = mySQL & "AND rainbow_serial_number = "
            mySQL = mySQL & Chr(39) & "-1" & Chr(39)
        End If
        If validCRFICLtype <> "" Then
            mySQL = mySQL & " AND sale_type = "
            mySQL = mySQL & Chr(39) & validCRFICLtype & Chr(39)
        End If
        mySQL = mySQL & " ORDER BY " & "reporting_entry_date"
        'If SortField <> "" Then
        '    If Trim(SortField) = "reporting_entry_date" Then
        '        mySQL = mySQL & " ORDER BY " & Trim(SortField)
        '    End If
        'End If

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)
        DS = New DataSet()

        'MyCommand.Fill(DS, "vw_AS400_CRF_And_ICL_Sales_Data_All")
        MyCommand.Fill(DS)
        'Step 7 Bind the data grid to the default view of the Table
        'MyDataGrid.DataSource=DS.Tables("vw_AS400_CRF_And_ICL_Sales_Data_All").DefaultView
        MyGridViewConsSales.DataSource = DS
        MyGridViewConsSales.DataBind()
    End Sub

    Sub FillGridDistSales(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        mySQL = "SELECT reference_date, reference_number, item_group_major_class, from_cust_wh, from_cust_wh_name, to_cust_wh, to_cust_wh_name, rgd_number, rgd_name FROM vw_AS400_Serial_Numbers_Sold_All "
        mySQL = mySQL & "WHERE company_number = 'RX' AND serial_number = "
        mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        mySQL = mySQL & " ORDER BY " & "reference_date"

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        'MyCommand.Fill(DS, "vw_AS400_Serial_Numbers_Sold_All")
        MyCommand.Fill(DS)

        'MyDataGrid.DataSource=DS.Tables("vw_AS400_Serial_Numbers_Sold_All").DefaultView
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

        mySQL = "SELECT [Entry_____ Start Date] AS entry_start_date, [Control Closing Date] as control_closing_date, [Customer Number] as customer_number, [Customer Name] as customer_name, "
        mySQL = mySQL & "[Country Code] as country_code, [Return Control Number] as return_control_number, [Tag Number] AS tag_number, [Part Number] as part_number, [Part Description] as part_description, "
        mySQL = mySQL & "[Line Quantity] as line_qty, [Disposition Description] as disposition_desc, [Warranty Denial Desc] as denial_desc, [DaysOnMarket] as days_onmarket, "
        mySQL = mySQL & "[MonthsOnMarket] as months_onmarket FROM vw_AS400_Daily_Closed_Warranty_SRX_Pivot_Table "
        mySQL = mySQL & "WHERE [Serial Number] = "
        mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        mySQL = mySQL & " ORDER BY " & "[Entry_____ Start Date]"

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        'MyCommand.Fill(DS, "vw_AS400_Daily_Closed_Warranty_SRX_Pivot_Table")
        MyCommand.Fill(DS)

        'MyDataGrid.DataSource=DS.Tables("vw_AS400_Daily_Closed_Warranty_SRX_Pivot_Table").DefaultView
        MyGridViewWarranty.DataSource = DS
        If Not EditIndex.Equals(Nothing) Then
            MyGridViewWarranty.EditIndex = EditIndex
        End If

        MyGridViewWarranty.DataBind()

    End Sub
    Sub FillGridConsSales(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String
        Dim validCRFICLtype As String = ""

        validCRFICLtype = CRFandICLExists() ' Added this so that if there is a serial number that exists in both CRF and ICL that one that matches the country sold into will be valid

        mySQL = "SELECT reporting_entry_date, sequence_number, batch_number, sale_type, power_nozzle_serial_number, country_code, country_code_name, city, "
        mySQL = mySQL & "state_prov_code, zip_code, canada_postal_code, rgd_number, rgd_name, region_number, region_desc, duplicate_remorse_flag "
        mySQL = mySQL & "FROM vw_AS400_CRF_And_ICL_Sales_Data_All "
        mySQL = mySQL & "WHERE company_number = 'RX' "
        'mySQL = mySQL & "AND rainbow_serial_number = "

        If Mid(serial_number.Text, 1, 3) = "042" Then ' This is a Power Nozzle SN
            mySQL = mySQL & "AND power_nozzle_serial_number = "
            mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        Else
            mySQL = mySQL & "AND rainbow_serial_number = "
            mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        End If

        If validCRFICLtype <> "" Then
            mySQL = mySQL & " AND sale_type = "
            mySQL = mySQL & Chr(39) & validCRFICLtype & Chr(39)
        End If
        ' mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        mySQL = mySQL & " ORDER BY " & "reporting_entry_date"

        MyCommand = New SqlDataAdapter(mySQL, MyConnection)

        DS = New DataSet()
        'MyCommand.Fill(DS, "vw_AS400_CRF_And_ICL_Sales_Data_All")
        MyCommand.Fill(DS)

        'MyDataGrid.DataSource=DS.Tables("vw_AS400_CRF_And_ICL_Sales_Data_All").DefaultView
        MyGridViewConsSales.DataSource = DS
        If Not EditIndex.Equals(Nothing) Then
            MyGridViewConsSales.EditIndex = EditIndex
        End If

        MyGridViewConsSales.DataBind()
    End Sub
    Sub BindGridProd(ByVal SortField As String)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        mySQL = "SELECT production_date, part_number, item_description, item_class, country, CBSN, CBSN_date, PMSN, PMSN_date FROM vw_AS400_Production_Date_Serial_Numbers WHERE serial_number = "
        If serial_number.Text <> "" Then
            mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        Else
            mySQL = mySQL & Chr(39) & "-1" & Chr(39)
        End If
        If SortField <> "" Then
            If Trim(SortField) = "production_date" Then
                mySQL = mySQL & " ORDER BY " & Trim(SortField)
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
    Sub FillGridProd(Optional ByVal EditIndex As Integer = -1)
        Dim DS As DataSet
        Dim MyCommand As SqlDataAdapter
        Dim mySQL As String

        mySQL = "SELECT production_date, part_number, item_description, item_class, country, CBSN, CBSN_date, PMSN, PMSN_date FROM vw_AS400_Production_Date_Serial_Numbers WHERE serial_number = "
        mySQL = mySQL & Chr(39) & serial_number.Text & Chr(39)
        mySQL = mySQL & " ORDER BY " & "production_date"

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
    Function CRFandICLExists() As String
        Dim strSQL As String = ""
        Dim MySQLCommand As SqlCommand
        Dim MySQLDataReader As SqlDataReader
        Dim mycount As Integer = 0
        Dim MySQLConnection As SqlConnection

        '04/05/2021 There are serial numbers that have been claimed as both CRF and ICL to consumers for Warranty purposes. Serial Number Inquiry does not always display the correct
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
        strSQL = strSQL & Chr(39) & serial_number.Text & Chr(39)

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
        ' Fires when a Updated Button is clicked. Fires After
        'If e.Row.RowType = DataControlRowType.Header Then
        'AddGlyph(MyGridView, e.Row)
        ' End If

    End Sub
    Sub gvp_RowCreated(ByVal Sender As Object, ByVal e As GridViewRowEventArgs)
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
    Sub ReturnToMainMenu(ByVal Sender As Object, ByVal E As EventArgs)
        Server.Transfer("wsmainmenu.aspx")
    End Sub
    Sub WindowExcelDistSales(ByVal Sender As Object, ByVal E As EventArgs)
        ' Excel rows limit is 1048576
        gridcountDistSales.Value = MyGridViewDistSales.Rows.Count

        If gridcountDistSales.Value <= 0 Then
            GetMessage.Style("color") = "red"
            GetMessage.InnerHtml = "No Distributor Sales records to export. Please Search for data first. "
            Exit Sub
        End If

        If (gridcountDistSales.Value + 1) <= 1048576 Then
            Response.Clear()
            Response.Buffer = True
            ' Response.ContentType = "application/vnd.ms-excel"
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=" & "rxviewserialnumberdataDistSales" & serial_number.Text.ToString & ".xls")
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
            GetMessage.InnerHtml = "No Closed Warranty records to export. Please Search for data first. "
            Exit Sub
        End If

        If (gridcountWarranty.Value + 1) <= 1048576 Then
            Response.Clear()
            Response.Buffer = True
            ' Response.ContentType = "application/vnd.ms-excel"
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=" & "rxviewserialnumberdataWarranty" & serial_number.Text.ToString & ".xls")
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
            GetMessage.InnerHtml = "No Consumer Sales records to export. Please Search for data first. "
            Exit Sub
        End If

        If (gridcountConsSales.Value + 1) <= 1048576 Then
            Response.Clear()
            Response.Buffer = True
            ' Response.ContentType = "application/vnd.ms-excel"
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=" & "rxviewserialnumberdataDistSales" & serial_number.Text.ToString & ".xls")
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