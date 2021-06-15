<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="wsviewuserapppermission.aspx.vb" Inherits="RexairWebSecurity.wsviewuserapppermission" EnableSessionState = "True"%>

<!--#include virtual="/ForceSSL.inc"-->

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WS View User App Permissions</title>
	   <link rel="stylesheet" href="../Content/wsSite.css"/>
    <script src="../Scripts/JavaScript.js" type="text/javascript"></script>
</head>
<body oncontextmenu="return false">
     <form runat="server" id="wsuserapppermissions" >

    <div id="container">
          <div class="row">
               <div class="column left" > <h1>View User App Permissions</h1>
               </div>
               <div class="column right" > <img src="../images/logo_small.png" alt="Rexair LLC"  width="200" height="100" />
               </div>
         </div>

        <div id="frame">

 <table width:"30%" class="font8veranda">
            <tr>
              <td class="aligntext">
			      <asp:HyperLink runat="server" Text="User ID:" NavigateURL="wsusermastermaint.aspx" Target="_blank"/>
              </td>
              <td>
	              <input type="button" id="btnSearch" onclick='pickUser("getuser_id");' value="S" runat="server" />
              </td>
              <td>
                <asp:TextBox ID="getuser_id" AutoPostBack="false" ReadOnly="false" runat="server" MaxLength="50" Width="100" CssClass="requiredfield" ToolTip="Enter a valid User ID." TabIndex="1"></asp:TextBox> 
              </td>
		      <td>
                <asp:TextBox ID="getuser_name" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="100" Width="300" BackColor="#CCCCCC"></asp:TextBox> 
              </td>
			  <td>
			     <input type="submit" id="btnGet" OnServerClick="GetUser_Click" value="Get" runat="server" tabindex="2"/>
			  </td>  
             </tr>
             <tr>
              <td colspan="4" class="paddingtop15; textaligncenter">
                <span id="GetMessage" EnableViewState="false" class="font11arial" runat="server"/>
              </td>
            </tr>
    </table>
    <hr class="hrheight2"/>
   <table width:"70%">
		<tr>
          <td align:"left" valign:"middle">
               <asp:Button ID="btnUserApp" Text="Get Data" OnClick="GetData" runat="server" ToolTip="Click to request data." TabIndex="3"/>                
           </td>
		</tr>
		<tr>
           <td colspan="2" class="paddingtop15; textalignleft">
              <span id="Message" EnableViewState="false" class="font11arial" runat="server"/>
           </td>
        </tr>	
	</table>
	<hr/> 	
 	<table width:"100%">
		<tr>
            <td >
                  <ASP:GridView id="MyGridView" runat="server"
                   Width="1100"
                   BackColor="#CCCCCC"
                   BorderColor="black"
                   ShowFooter="false"
                   CellPadding="3"
                   CellSpacing="0"
                   Font-Names="Verdana"
                   Font-Size="8pt"
                   HeaderStyle-BackColor="#aaaadd"
			       PageSize="4000"
                   EnableViewState="true"
			       DataKeyNames="group_id"
			       AutoGenerateColumns="false"
				   OnPageIndexChanged = "gv_PageIndexChanged" 
			       OnPageIndexChanging = "gv_PageIndexChanging" 
			       OnRowDataBound = "gv_RowDataBound"
			       OnSorting="gv_Sorting" 
			       AllowSorting="True"
			       AllowPaging="True" 
				   ToolTip="UserAppPermissions.">
								
		           <FooterStyle forecolor="White" backcolor="Black"></FooterStyle>
                   <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
                   <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999" 
                         Font-Names="Verdana" Font-Size="Small"></PagerStyle>
                   <PagerSettings Mode="NumericFirstLast" Position="Top"/>
		           <EditRowStyle BackColor="#AFEEEE" />
                   <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />

				   <Columns>
				          <asp:BoundField HeaderText="Group ID" DataField="group_id" SortExpression="group_id" ReadOnly="true" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Group Name" DataField="group_name" SortExpression="group_name" ReadOnly="true" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="App ID" DataField="application_id" SortExpression="application_id" ReadOnly="true" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				   	      <asp:BoundField HeaderText="App Name" DataField="application_name" SortExpression="application_name" ReadOnly="true" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="Area ID" DataField="area_id" SortExpression="area_id" ReadOnly="true" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="Create" DataField="permission_create" SortExpression="permission_create" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Read" DataField="permission_read" SortExpression="permission_read" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="Update" DataField="permission_update" SortExpression="permission_update" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right"/> 
					      <asp:BoundField HeaderText="Delete" DataField="permission_delete" SortExpression="permission_delete" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
			       </Columns>
		          </ASP:GridView>
		    </td>  
		</tr>
	</table>	
	
	<table class="floatright">
		<tr>
		   <td>
                <asp:TextBox ID="sort_id" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="40" Width="40" Visible="false"></asp:TextBox> 
                <asp:TextBox ID="clearpressed" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="40" Width="40" Visible="false"></asp:TextBox> 
 	            <input type="hidden" name="gridcount" id="gridcount" runat="server" />
		   </td>
		</tr>
		<tr>
            <td align:"right">
               <asp:Button id="btnPrint" Text="Print" OnClick="WindowPrint" runat="server" ToolTip="Click to print the window." TabIndex="3"/>
		   </td>
           <td align:"right">
               <asp:Button id="btnClear" Text="Clear" OnClick="ClearRefresh" runat="server" ToolTip="Click to clear the window." TabIndex="4"/>
		   </td>
           <td align:"right">
               <asp:Button id="btnReturn" Text="Return to Main Menu" OnClick="ReturnToMainMenu" runat="server" ToolTip="Click to return to the WS Main Menu." TabIndex="5"/>
		   </td>
		</tr>
	</table>
   </div>
  </div>
  </form>

</body>
</html>
