Imports Microsoft.Reporting.WebForms
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Net
Imports System.IO
Public Class wsusermasterrpt
    Inherits System.Web.UI.Page

    Sub Session_Start()
        'Session.Timeout = 30 ' 30 minutes
        Session.Timeout = 525600
    End Sub

    Sub Page_Init(Sender As Object, E As EventArgs)
        'Response.Write("Pager_Init")
        ' Response.End()
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
    Sub GetParameter()
        'Dim s As String
        's = "<SCRIPT language='javascript'>PrintData("
        's = s & Chr(34) & "wsusermastermaint.rpt" & Chr(34)
        's = s & ") </"
        's = s & "SCRIPT>"

        's = "<SCRIPT language='javascript'>"
        's = s & "const params = new URLSearchParams(window.location.search);"
        's = s & "var x = params.get('formname');"
        's = s & "getsource.value = x;"
        's = s & "</SCRIPT>"

        's = "<SCRIPT language='javascript'>"
        's = s & "getsource.Value = window.location.search;"
        's = s & "</SCRIPT>"

        's = "<SCRIPT language='javascript'>getFormName("
        's = s & ") </"
        's = s & "SCRIPT>"

        ' getsource.Value = "wsusermastermaint"
        ' getsource.Value = window.location.search

    End Sub
    Sub Page_Load(Sender As Object, E As EventArgs)

        If Not Page.IsPostBack Then

            'Set the processing mode for the ReportViewer to Remote  
            ReportViewer1.ProcessingMode = ProcessingMode.Remote
            ' ReportViewer1.ShowReportBody = True

            Dim serverReport As ServerReport
            serverReport = ReportViewer1.ServerReport

            'Set the report server URL and report path  
            serverReport.ReportServerUrl = New Uri("http://mfg41-sql/ReportServer")

            serverReport.ReportPath = "/Applications/WebSecurity/wsusermaster"

            serverReport.Refresh()

            ReportViewer1.Visible = True

            ''Create the sales order number report parameter  
            'Dim salesOrderNumber As New ReportParameter()
            'salesOrderNumber.Name = "SalesOrderNumber"
            'salesOrderNumber.Values.Add("SO43661")

            ''Set the report parameters for the report  
            'Dim parameters() As ReportParameter = {salesOrderNumber}
            'serverReport.SetParameters(parameters)
        End If

    End Sub

End Class