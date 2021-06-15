<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="wscopyusersecurity.aspx.vb" Inherits="RexairWebSecurity.wscopyusersecurity" EnableSessionState = "True"%>

<!--#include virtual="/ForceSSL.inc"-->

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WS Copy User Security</title>
    <link rel="stylesheet" href="../Content/wsSite.css"/>
    <script src="../Scripts/JavaScript.js" type="text/javascript"></script>
</head>
<body oncontextmenu="return false">
 
  <form id="wscopyusersecurity" runat="server">
 	
     <div id="container">
          <div class="row">
               <div class="column left" > <h1>Copy User Security</h1>
               </div>
               <div class="column right" > <img src="../images/logo_small.png" alt="Rexair LLC"  width="200" height="100" />
               </div>
         </div>

        <div id="frame">
	
    <table width:"50%" >
      <tr>
        <td valign:"top">
          <div class="frame">
          <table class="font8veranda">
            <tr>
              <td colspan="4"  class="font10veranda">Copy User Security:</td>
            </tr>
            <tr>
 			  <td class="aligntext">
			     <asp:HyperLink runat="server" Text="From User ID:" NavigateURL="wsusermastermaint.aspx" Target="_blank" TabIndex="0"/>
	             <input type="button" id="btnSearch" onclick='javascript:pickUser("from_user_id");' value="S" runat="server" tabindex="0"/>
              </td>
              <td>
 	            <asp:Textbox id="from_user_id" AutoPostBack="true" OnTextChanged="ReturnFromUser_Click" runat="server" MaxLength="50" Width="200" CssClass="requiredfield" ToolTip="Enter a valid User ID." TabIndex="1"/>
              </td>
              <td>
			     <div class="buttonvisbilityhidden"><input type="button" id="btnGet" onServerClick="ReturnFromUser_Click" value="" runat="server" disabled="disabled" tabindex="0"/></div>
	          </td>
            </tr>
            <tr>
              <td colspan="1" class="aligntext"><strong>From User Name: </strong></td>
              <td>
                <asp:TextBox ID="from_user_name" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="100" Width="300" BackColor="#CCCCCC" TabIndex="0"></asp:TextBox> 
               </td>
            </tr>
            <tr>
 			  <td class="aligntext">
			     <asp:HyperLink runat="server" Text="To User ID:" NavigateURL="wsusermastermaint.aspx" Target="_blank" TabIndex="0"/>
	             <input type="button" id="btnTSearch" onclick='javascript:pickTUser("to_user_id");' value="S" runat="server" tabindex="0" />
              </td>
              <td>
 	            <asp:Textbox id="to_user_id" AutoPostBack="true" OnTextChanged="ReturnToUser_Click" runat="server" MaxLength="50" Width="200" CssClass="requiredfield" Tooltip="Enter a valid User ID." TabIndex="2"/>
              </td>
              <td>
			     <div class="buttonvisbilityhidden"><input type="button" id="btnTGet" onServerClick="ReturnToUser_Click" value="" runat="server" disabled="disabled" tabindex="0"/></div>
	          </td>
            </tr>
            <tr>
              <td colspan="1" class="aligntext"><strong>To User Name: </strong></td>
              <td>
                <asp:TextBox ID="to_user_name" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="100" Width="300" BackColor="#CCCCCC" TabIndex="0"></asp:TextBox> 
               </td>
            </tr>
			 
             <tr>
              <td></td>
              <td class="paddingtop15">
                <asp:Button id="btnCopy" Text="Copy" OnClick="CopyUser_Click" runat="server" ToolTip="Click to copy user security to another user. ALT+Y" AccessKey="Y" TabIndex="3"/>
                <asp:Button Text="Clear" OnClick="PageReset_Click" runat="server" ToolTip="Click to reset the window. ALT+C" TabIndex="4" AccessKey="C"/>
		      </td>
            </tr>
            <tr>
              <td colspan="5" class="paddingtop15; textalignleft">
                <span id="CopyMessage" EnableViewState="false" class="font11arial" runat="server"/>
              </td>
            </tr>
          </table>
		  </div>
        </td>
      </tr>
    </table>
	<table width:"100%" class="textalignright">
	    <tr>
		   <td>
		   </td>
		</tr>
		<tr>
           <td>
               <asp:Button id="btnReturn" Text="Return to Main Menu" OnClick="ReturnToMainMenu" runat="server" ToolTip="Click to return to the Web Security Main Menu. ALT+R" AccessKey="R" TabIndex="5"/>
		   </td>
		</tr>
	</table>
             </div>
          </div>
  </form>
</body>
</html>
