<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="wsusermasterrpt.aspx.vb" Inherits="RexairWebSecurity.wsusermasterrpt" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!--#include virtual="/ForceSSL.inc"-->

<!DOCTYPE HTML>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
     <title>WS User Master Report</title>
    <link rel="stylesheet" href="../Content/wsSite.css"/>
    <style type="text/css">
        html,body,form,#div1 {
            height: 100%; 
        }
    </style>
   </head>

<body oncontextmenu="return false">

   <form id="form1" runat="server">
    <div style="height:100vh;" >
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" ProcessingMode="Remote" Height="100%" Width="100%" visibility="true" ShowBackButton="True" ShowExportControls="True" ShowFindControls="True" ShowPageNavigationControls="True" ShowPrintButton="true" ShowRefreshButton="true" ShowZoomControl="true">
        </rsweb:ReportViewer>
    </div>

    </form>
    <input />
</body>
</html>
