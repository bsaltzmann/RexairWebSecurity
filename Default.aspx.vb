Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Random
Imports System.Security.Cryptography
Imports System.IO
Imports System.Security
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Security.Principal
Imports System.Runtime.InteropServices

Public Class _Default
    Inherits Page

    Sub Page_Init(Sender As Object, E As EventArgs)

    End Sub

    Sub Login_Click(ByVal Sender As Object, ByVal E As EventArgs)
        'Response.Redirect("wslogin.aspx")
        Server.Transfer("wslogin.aspx")
    End Sub
    Public Function Get_SQLUserInformationSetGlobalVariables(useridkey As String) As Boolean
        'From the Page_Init

        'guseridkey = rxdwprog
        Dim strSQL As String = ""
        Dim MySQLCommand As SqlCommand
        Dim MySQLDataReader As SqlDataReader
        Dim mycount As Integer = 0
        Dim MySQLConnection As SqlConnection

        'Get the  user_information from the Rexair Data Warehouse

        Get_SQLUserInformationSetGlobalVariables = False

        '---Open Connection to the Rexair Data Warehouse----------------------------------------------------
        'MySQLConnection = New SqlConnection("server=MFG41-SQL;database=RexairDataWarehouse;UID=RXDW;PWD=RXDW$2020;MultipleActiveResultSets=True") ' with Reader Writer
        MySQLConnection = New SqlConnection("server=MFG41-SQL;database=RexairDataWarehouse;UID=RXDWRO;PWD=RXDWRO$2020;MultipleActiveResultSets=True") ' Read Only Setup to be used for Reporting
        Try
            MySQLConnection.Open()
            '           tbStatus.Text = tbStatus.Text & "Rexair Data Warehouse database opened MFG41-SQL. Get_SQLUserInformationSetGlobalVariables" & Chr(13) & Chr(10)
        Catch ex As Exception
            '            tbStatus.Text = tbStatus.Text & "*Error Could Not open Rexair Data Warehouse database MFG41-SQL. Get_SQLUserInformationSetGlobalVariables" & ex.Message & Chr(13) & Chr(10)

            MySQLConnection.Dispose()
            MySQLConnection = Nothing
            Exit Function
        End Try

        ' strSQL = "Select user_password, user_email "
        strSQL = "Select user_email, user_display_name, Convert(varchar(50), DecryptByPassPhrase('rexairvbnetpearl',user_password)) AS DecryptedPassword "
        strSQL = strSQL & "FROM User_Information " ' 
        strSQL = strSQL & "WHERE user_id='" & useridkey & "'"
        ' MessageBox.Show("strCAL = " & strCAL)

        MySQLCommand = New SqlCommand(strSQL, MySQLConnection)

        Try
            MySQLDataReader = MySQLCommand.ExecuteReader()
            'MessageBox.Show("G")
            'Loop until a valid 
            While MySQLDataReader.Read()

                'mycount = mycount + 1
                Dim myConfiguration As Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~")
                'myConfiguration.ConnectionStrings.ConnectionStrings("myDatabaseName").ConnectionString = txtConnectionString.Text
                myConfiguration.AppSettings.Settings.Item("gEmailPass").Value = MySQLDataReader("DecryptedPassword")
                myConfiguration.AppSettings.Settings.Item("gSharePointPwd").Value = MySQLDataReader("DecryptedPassword")
                myConfiguration.AppSettings.Settings.Item("gSharePointUserName").Value = MySQLDataReader("user_email")

                myConfiguration.Save()

                Get_SQLUserInformationSetGlobalVariables = True

                Exit While

            End While
            'Close the Connection to SQL Rexair_Data_Warehouse
            MySQLConnection.Close()
            MySQLConnection = Nothing

            MySQLCommand.Dispose()
            MySQLDataReader.Close()
            MySQLCommand = Nothing
            MySQLDataReader = Nothing

        Catch ex As SqlException
            Response.Write("<B><FONT COLOR=red>*ERROR Get_SQLUserInformationSetGlobalVariables SQL Exception Message  & " & CStr(ex.Message) & "</FONT></B>")
            'Close the Connection to SQL Rexair_Data_Warehouse
            MySQLConnection.Close()
            MySQLConnection = Nothing

            MySQLCommand.Dispose()
            MySQLCommand = Nothing

            Exit Function
        Catch ex As DataException
            Response.Write("<B><FONT COLOR=red>*ERROR Get_SQLUserInformationSetGlobalVariables Data Exception Message  & " & CStr(ex.Message) & "</FONT></B>")
            'Close the Connection to SQL Rexair_Data_Warehouse
            MySQLConnection.Close()
            MySQLConnection = Nothing

            MySQLCommand.Dispose()
            MySQLCommand = Nothing

            Exit Function
        Catch ex As Exception
            Response.Write("<B><FONT COLOR=red>*ERROR Get_SQLUserInformationSetGlobalVariables Exception Message & " & CStr(ex.Message) & "</FONT></B>")
            'Close the Connection to SQL Rexair_Data_Warehouse
            MySQLConnection.Close()
            MySQLConnection = Nothing

            MySQLCommand.Dispose()
            'MySQLDataReader.Close()
            MySQLCommand = Nothing
            ' MySQLDataReader = Nothing

            Exit Function

        End Try

    End Function
End Class