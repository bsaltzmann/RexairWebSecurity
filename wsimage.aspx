<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wsimage.aspx.vb" Inherits="RexairWebSecurity.wscaimage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link rel="stylesheet" href="Site.css"/>
</head>
<body class="font8veranda" oncontextmenu="return false">
  <form id="wslogin" runat="server">
 
    <table style="width:80%;" >
        <tr>
           <td>
               <asp:TextBox ID="random_number" AutoPostBack="false" ReadOnly="true" runat="server" Width="200" Visible="true"></asp:TextBox> 
           </td>
        </tr>
     </table>
     
   </form>

  
</body>
</html>
