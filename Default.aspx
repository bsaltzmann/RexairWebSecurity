<%@ Page Title="Home Page" Language="VB"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="RexairWebSecurity._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="frame">
      <table class="table_input" >
 
		   <tr class="data_row">
			  <td class="data_button" colspan="3">
	             <asp:Button id="btnLogin" Text="Login" OnClick="Login_Click" runat="server"  ToolTip="Click to go to the Login Page. ALT+L" TabIndex="3" AccessKey="L"/>
 			  </td>
           </tr>
 
      </table>
     
    </div>

</asp:Content>


