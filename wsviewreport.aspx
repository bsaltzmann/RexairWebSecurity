<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="wsviewreport.aspx.vb" Inherits="RexairWebSecurity.wsviewreport" EnableSessionState = "True"%>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!--#include virtual="/ForceSSL.inc"-->

<!DOCTYPE HTML>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
     <title>WS Report viewer</title>
    <link rel="stylesheet" href="../Content/wsSite.css"/>
    <style type="text/css">
        html,body,form,#div1 {
            height: 100%; 
        }
    </style>
   </head>
<body oncontextmenu="return false">

       <script type="text/javascript">
           var LocationSearchWin = null;
           function pickLocation(Src) {
               document.getElementById('btnGet').disabled = false;
               if (LocationSearchWin == null) {
                   LocationSearchWin = window.open("wslocationmastersearch.aspx?src=" + Src,
                       "_blank",
                       "height=500, width=700, left=100, top=100, " +
                       "location=no, menubar=no, resizable=no, " +
                       "scrollbars=yes, titlebar=no, toolbar=no, fullscreen=yes", true);
               }
               else {
                   LocationSearchWin.focus()
               }
           }

           function msg() {
               alert("Hello world!");
           }

           function getFormName() {
               alert(window.location.search);
           }

           function showDialogue() {
               var query = window.location.search.substring(1);
               //document.getElementById('getsource').value = query;
               document.getElementById("<%=TextBox1.ClientID%>") = window.location.search.substring(1);
               //document.getElementById('<%= getsource.ClientID %>').value = window.location.search.substring(1);
               //alert(query);
           } 

       </script>
    <form id="form1" runat="server">
    <div style="height:100vh;" >
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" ProcessingMode="Remote" Height="100%" Width="100%" visibility="true" ShowBackButton="True" ShowExportControls="True" ShowFindControls="True" ShowPageNavigationControls="True" ShowPrintButton="true" ShowRefreshButton="true" ShowZoomControl="true">
        </rsweb:ReportViewer>
    </div>

   
        <asp:Label ID="lblJavaScript" runat="server" Text=""></asp:Label> 
        <asp:Button ID="btnShowDialogue" runat="server" Text="Show Dialogue" /> 
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <input type="hidden" id="getsource" value="" runat="server" visible="true" />
    </form>
    <input />
</body>
</html>
