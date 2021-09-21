<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="wslogin.aspx.vb" Inherits="RexairWebSecurity.wslogin" EnableSessionState = "True"%>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rexair Web Security Login</title>
    <link rel="stylesheet" href="../Content/wsSite.css"/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jquery-footable/0.1.0/css/footable.min.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-footable/0.1.0/js/footable.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MyGridView').footable();
        });
    </script>
</head>
<body oncontextmenu="return false">

  <form id="wslogin" runat="server" defaultbutton="btnLogin" defaultfocus="user_id">
      <div id="container">
          <div class="row">
               <div class="column left" > <h1>Web Security Login</h1>
               </div>
               <div class="column right" > <img src="../images/logo_small.png" alt="Rexair LLC"  width="100%" height="140%" />
               </div>
         </div>
 
        <div id="frame">
        <table width:"30%" class="font12veranda">
          <tr>
              <td class="aligntext"><strong>User ID: </strong></td>
              <td>
				<asp:TextBox ID="user_id" runat="server" MaxLength="50" 
                      CssClass="requiredfield" 
                      ToolTip="Enter the User ID." tabindex="1" Width="200px"></asp:TextBox>
              </td>
	      </tr>
		  <tr>
	          <td class="aligntext"><strong>Password: </strong></td>
		      <td>
                <asp:TextBox id="password" TextMode="password" runat="server" maxlength="12" 
                CssClass="requiredfield" 
                ToolTip="Enter the User Password." tabindex="2" width="100"/>
              </td>
          </tr>
        </table>

         <div class="buttonArea">
               <asp:Button ID="btnLogin" Font-Size="Large" Text="Login" OnClick="Login_Click" runat="server"  ToolTip="Click to Login. ALT+L" TabIndex="3" AccessKey="L"  />
                <div class="divider"/>
               <asp:Button ID="btnClear" Font-Size="Large" Text="Clear" OnClick="PageReset_Click" runat="server" UseSubmitBehavior="false" ToolTip="Click to reset the window. ALT+C" TabIndex="4" AccessKey="C"  />
         </div>

         <div>
             <span id="Message" EnableViewState="false"  runat="server"/>
         </div>
    </div>
    </div>
  </form>
         
</body>
</html>
