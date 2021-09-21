<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="wschangeuserpassword.aspx.vb" Inherits="RexairWebSecurity.wschangeuserpassword" EnableSessionState = "True"%>

<!--#include virtual="/ForceSSL.inc"-->

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WS User Password Change</title>
       <link rel="stylesheet" href="../Content/wsSite.css"/>
     <script src="../Scripts/JavaScript.js" type="text/javascript"></script>
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
<body class="font8veranda" onkeypress="checkCapsLock(event)" oncontextmenu="return false">

  <form runat="server" id="wschangeuserpassword" defaultbutton="btnChange" defaultfocus="user_id">
      <div id="container">
          <div class="row">
               <div class="column left" > <h1>Change User Password</h1>
               </div>
               <div class="column right" > <img src="../images/logo_small.png" alt="Rexair LLC"  width="100%" height="140%" />
               </div>
         </div>

        <div id="frame">
	
    <table width:"100%" class="font8veranda">
      <tr>
        <td style="vertical-align:middle;">
          <div class="frame">
          <table class="font8veranda">
             <tr>
 			  <td class="aligntext">
			     <asp:HyperLink runat="server" Text="User ID:" NavigateURL="wsusermastermaint.aspx" Target="_blank" TabIndex="0"/>
	             <input type="button" id="btnSearch" onclick='javascript:pickUser("user_id");' value="S" runat="server" accesskey="S" tabindex="0" />
 	             <asp:Textbox id="user_id" AutoPostBack="true" OnTextChanged="ReturnUser_Click" runat="server" CssClass="requiredfield" MaxLength="50" Width="200px" ToolTip="Enter a valid User ID." TabIndex="1"/>
              </td>
		      <td>
                <asp:TextBox ID="user_name" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="100" Width="300" BackColor="#CCCCCC" TabIndex="0"></asp:TextBox> 
              </td>
              <td>
			     <div class="buttonvisbilityhidden" ><input type="submit" id="btnGet" onServerClick="ReturnUser_Click" value="Get" runat="server" disabled="disabled" tabindex="0"/></div>
	          </td>
            </tr>
            </table>
            <table>
            <tr>
              <td colspan="1" class="aligntext"><strong>From User Password: </strong></td>
              <td>
                <asp:TextBox id="from_user_password" TextMode="password" runat="server" maxlength="12" width="100" CssClass="requiredfield" ToolTip="Enter the current User Password." TabIndex="2"/>
                </td>
               <td>
                  <strong>Password Unknown: </strong>
                  <asp:CheckBox ID="passunknown" AutoPostBack="false" Enabled="true" runat="server" ToolTip="Click if you do not remember the current password." TabIndex="0"></asp:CheckBox> 
               </td>
            </tr>
            <tr>
              <td colspan="1" class="aligntext"><strong>To User Password: </strong></td>
              <td>
                <asp:TextBox id="to_user_password" TextMode="password" runat="server" maxlength="12" width="100" CssClass="requiredfield" ToolTip="Enter the new Password." TabIndex="3"/>
              </td>
            </tr>
            <tr>
              <td colspan="1" class="aligntext"><strong>Confirm User Password: </strong></td>
              <td>
                <asp:TextBox id="confirm_user_password" TextMode="password" runat="server" maxlength="12" width="100" CssClass="requiredfield" ToolTip="Enter the new Password again for confirmation." TabIndex="4"/>
              </td>
            </tr>
			<tr>
			  <td colspan="1" class="aligntext"><strong>Next Password To Change: </strong></td>
		      <td>
                <asp:TextBox ID="which_pass" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="4" Width="50" BackColor="#CCCCCC" TabIndex="0"></asp:TextBox> 
              </td>
			</tr>
			<tr>
			  <td colspan="1" class="aligntext"><strong>Never Expire: </strong></td>
		      <td style="vertical-align:top;">
                <asp:TextBox ID="never_expire" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="4" Width="50" BackColor="#CCCCCC" TabIndex="0"></asp:TextBox> 
              </td>
 			</tr>
			<tr>
			  <td></td>
			  <td colspan="3"> 
                  <strong>( 0 = Password Expires / 1 = Password will never Expire )</strong>
              </td>
			</tr>
			<tr>
			  <td colspan="1" class="aligntext"><strong>Password One: </strong></td>
		      <td>
                <asp:TextBox ID="user_p_one" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="12" Width="100" BackColor="#CCCCCC" TabIndex="0"></asp:TextBox> 
              </td>
			</tr>
			<tr>
			  <td colspan="1" class="aligntext"><strong>Password Two: </strong></td>
		      <td>
                <asp:TextBox ID="user_p_two" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="12" Width="100" BackColor="#CCCCCC" TabIndex="0"></asp:TextBox> 
              </td>
			</tr>
			<tr>
			  <td colspan="1" class="aligntext"><strong>Password Three: </strong></td>
		      <td>
                <asp:TextBox ID="user_p_three" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="12" Width="100" BackColor="#CCCCCC" TabIndex="0"></asp:TextBox> 
              </td>
			</tr>
			<tr>
			  <td colspan="1" class="aligntext"><strong>Last Change: </strong></td>
		      <td>
                <asp:TextBox ID="last_password_change" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="40" Width="200" BackColor="#CCCCCC" TabIndex="0"></asp:TextBox> 
              </td>
			</tr>
			<tr>
			  <td colspan="1" class="aligntext"><strong>Last Logged In: </strong></td>
		      <td>
                <asp:TextBox ID="last_login_date" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="100" Width="200" BackColor="#CCCCCC" TabIndex="0"></asp:TextBox> 
              </td>
			</tr>
		    <tr>
			  <td colspan="1" class="aligntext"><strong>Login Type: </strong></td>
		      <td>
                <asp:TextBox ID="login_type" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="40" Width="100" BackColor="#CCCCCC" TabIndex="0"></asp:TextBox> 
              </td>
			</tr>
		    <tr>
			  <td colspan="1" class="aligntext"><strong>Expires: </strong></td>
		      <td>
                <asp:TextBox ID="expiration_date" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="40" Width="200" BackColor="#CCCCCC" TabIndex="0"></asp:TextBox> 
              </td>
			</tr>
			<tr>
			  <td></td>
			  <td colspan="3"> 
			     <asp:Label id="expireslabel" text="" runat="server" Font-Bold="true"/>
               </td>
			</tr>
            <tr>
              <td></td>
            </tr>
            <tr>
              <td></td>
              <td class="paddingtop15; textalignleft">
                <asp:Button id="btnChange" Text="Update Password" OnClick="ChangeUser_Click" runat="server" ToolTip="Click to change the Users Password. ALT+U" AccessKey="U" TabIndex="5"/>
                <asp:Button Text="Clear" OnClick="PageReset_Click" runat="server" ToolTip="Click to reset the window. ALT+C" AccessKey="C" TabIndex="6" />
		      </td>
            </tr>
            <tr>
              <td colspan="5" class="paddingtop15; textalignleft">
                <span id="Message" EnableViewState="false" class="font11arial" runat="server"/>
              </td>
            </tr>
          </table>
		  </div>
        </td>
      </tr>
    </table>
	<table class="textalignright">
	    <tr>
		   <td>
		   </td>
		</tr>
		<tr>
           <td>
               <asp:Button id="btnReturn" Text="Return to Main Menu" OnClick="ReturnToMainMenu" runat="server" ToolTip="Click to return to the Web Security Main Menu. ALT+R" AccessKey="R" TabIndex="7"/>
		   </td>
		</tr>
	</table>
             </div>
           </div>
  </form>
</body>
</html>
