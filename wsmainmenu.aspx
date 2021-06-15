<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="wsmainmenu.aspx.vb" Inherits="RexairWebSecurity.wsmainmenu" EnableSessionState = "True"%>

<!--#include virtual="/ForceSSL.inc"-->

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rexair Web Security Main Menu</title>
  <link rel="stylesheet" href="../Content/wsSite.css"/>
  <script src="../Scripts/JavaScript.js" type="text/javascript"></script>

    <style type="text/css" media="screen">

.style2
    {
        width: 501px;
    }
    
	.style3
    {
        width: 31%;
    }
	</style>
</head>

<body class="font8veranda" oncontextmenu="return false">

  <form id="wsmainmenu" method="post" runat="server">
    <div id="container">
          <div class="row">
               <div class="column left" > <h1>Web Security Main Menu</h1>
               </div>
               <div class="column right" > <img src="../images/logo_small.png" alt="Rexair LLC"  width="200" height="100" />
               </div>
         </div>
    <hr class="hrheight2"/>
   	<% If Session("Master") = "true" Then ' Master account so this is for users only%>
         <table style="background-color:#CCCCCC; width: 50%;">
        <tr>
        <td style="width: 20%;">
	    <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" TabIndex="0" Width="180" Height="22" 
             BackColor="#CCCCCC" ForeColor="#000000" BorderStyle="Outset" BorderWidth="2" Font-Bold="True" target="_blank" staticsubmenuindent="10" 
             Font-Size="Small" Font-Underline="False" BorderColor="#FFFFFF" StaticDisplayLevels="1" StaticMenuItemStyle-HorizontalPadding="10" 
             disappearafter="2000" DynamicMenuItemStyle-HorizontalPadding = "10" 
             DynamicEnableDefaultPopOutImage="false" DynamicPopOutImageUrl="../images/application_view_icons.png"
             StaticEnableDefaultPopOutImage="false" StaticPopOutImageUrl="../images/application_view_icons.png">
              <StaticHoverStyle BackColor="#000000" ForeColor="#FFFFFF" BorderColor="Aqua"/>
              <dynamicmenuitemstyle backcolor="#FFFAF0" forecolor="Black" BorderStyle="Outset" BorderWidth="2" />
              <dynamichoverstyle backcolor="#2876CD" forecolor="White"/>
              <DynamicMenuStyle CssClass="adjustedZIndex"/>
            <Items>
            <asp:MenuItem Text="View" Value="View"></asp:MenuItem>
            </Items>
        </asp:Menu>
        </td>
        <td style="width: 20%;">
          <asp:Menu ID="Menu2" runat="server" Orientation="Horizontal" TabIndex="0" Width="180" Height="22"
             BackColor="#CCCCCC" ForeColor="#000000" BorderStyle="Outset" BorderWidth="2" Font-Bold="True" target="_blank" 
             Font-Size="Small" Font-Underline="False" BorderColor="#FFFFFF" StaticDisplayLevels="1" StaticMenuItemStyle-HorizontalPadding="10"
             disappearafter="2000" DynamicMenuItemStyle-HorizontalPadding = "10"
             DynamicEnableDefaultPopOutImage="false" DynamicPopOutImageUrl="../images/direction.png"
             StaticEnableDefaultPopOutImage="false" StaticPopOutImageUrl="../images/direction.png">
              <StaticHoverStyle BackColor="#000000" ForeColor="#FFFFFF" BorderColor="Aqua" />
               <dynamicmenuitemstyle backcolor="#FFFAF0" forecolor="Black" BorderStyle="Outset" BorderWidth="2" />
              <dynamichoverstyle backcolor="#2876CD" forecolor="White"/>
              <DynamicMenuStyle CssClass="adjustedZIndex" />
           <Items>
            <asp:MenuItem Text="Setup" Value="Setup"></asp:MenuItem>
          </Items>
          </asp:Menu>
        </td>
        
        <td class="style3">
	        <asp:Menu ID="Menu3" runat="server" Orientation="Horizontal" TabIndex="0" Width="180" Height="22"
              BackColor="#CCCCCC" ForeColor="#000000" StaticMenuItemStyle-HorizontalPadding="10" BorderStyle="Outset" BorderWidth="2" 
              Font-Bold="True" target="_blank" 
              Font-Size="Small" Font-Underline="False" BorderColor="#FFFFFF" 
              StaticDisplayLevels="1">
              <StaticHoverStyle BackColor="#000000" ForeColor="#FFFFFF" BorderColor="Aqua" />
            <Items>
             <asp:MenuItem Text="Logout" NavigateUrl="wslogin.aspx" Target="_self" Value=""></asp:MenuItem>
            </Items>
            </asp:Menu>
         </td>       
       </tr>
	  </table> 

      <table style="width: 37%; position:absolute; left:950px; top:170px" >
      <tr>
         <td style="vertical-align:top;" class="style2" >
          <div class="frame" >
          <table class="font8veranda">
            <tr>
              <td style="vertical-align:middle;"><strong>User ID:</strong></td>
              <td>
                <asp:TextBox ID="UserIDtb" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="20" Width="183px" BackColor="#CCCCCC"></asp:TextBox> 
              </td>
            </tr>
            <tr>
              <td class="aligntext"><strong>Date:</strong></td>
              <td>
                <asp:TextBox ID="Datetb" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="10" Width="100" BackColor="#CCCCCC" ></asp:TextBox> 
              </td>
            </tr>
            <tr>
              <td class="aligntext"><strong>Time:</strong></td>
              <td>
                <asp:TextBox ID="Timetb" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="10" Width="100" BackColor="#CCCCCC" ></asp:TextBox> 
              </td>
            </tr>
 	      </table>
		  </div>
        </td>
      </tr>
    </table>
   	<%      End If %>
        </div>
  </form>

</body>
</html>
