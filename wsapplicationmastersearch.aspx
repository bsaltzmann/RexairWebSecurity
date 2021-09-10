<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="wsapplicationmastersearch.aspx.vb" Inherits="RexairWebSecurity.wsapplicationmastersearch" EnableSessionState = "True"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WS Application Master Search</title>
      <link rel="stylesheet" href="../Content/wsSite.css"/>

    <style type="text/css" media="screen">
        #getapplicationid
    {
        width: 156px;
    }
    #getapplicationname
    {
        width: 275px;
    }

</style>
</head>
<body class="font8veranda" oncontextmenu="return false">

  <form runat="server" id="wsapplicationmastersearch">
     <div id="container">
          <div class="row">
               <div class="column left" > <h1>Application Master Search</h1>
               </div>
               <div class="column right" > <img src="../images/logo_small.png" alt="Rexair LLC"  width="100" height="50" />
               </div>
         </div>
   </div>
	
   <hr class="hrheight2"/>
   
     <table style="width:60%;" class="font8veranda">
             <tr>
			  <td>
			  <strong>Search Type: </strong><asp:ListBox id="ApplicationSelect" runat="server" Rows="1" OnSelectedIndexChanged="DropChange_Click" AutoPostBack="true" TabIndex="1">
			     <asp:ListItem>Application ID</asp:ListItem>
			     <asp:ListItem>Application Name</asp:ListItem>	
			  </asp:ListBox>
			  </td>
			  <td></td>
			 </tr>  
             <tr>
  			  <td>
                <strong>Application ID: </strong><input type="text" id="getapplicationid" value="" runat="server" maxlength="50"  tabindex="1" />
              </td>
			  <td>
                  <strong>Application Name: </strong> <input type="text" id="getapplicationname" value="" runat="server" maxlength="100" disabled="disabled" />
              </td>
			  <td>
			     <input type="submit" id="btnGet" OnServerClick="GetData_Click" value="Search" runat="server" title="Click to Search based on criteria entered. ALT+S" accesskey="S"/>
			  </td>  
			  <td>
			     <input type="button" id="btnClose" OnServerClick="Close_Click" value="Close" runat="server" title="Click to Close the window without returning a Application. ALT+C" accesskey="C"/>
			  </td>  
             </tr>
             <tr>
              <td colspan="2" class="paddingtop15; textaligncenter">
                <span id="Message" EnableViewState="false" class="font11arial" runat="server"/>
              </td>
            </tr>
     </table>
     <hr class="hrheight2"/>
	<br/>
    <table style="width:30%;" >
	<tr>
        <td >
          <ASP:GridView id="MyGridView" runat="server"
            Width="600"
            BackColor="#CCCCCC"
            BorderColor="black"
            ShowFooter="false"
            CellPadding="3"
            CellSpacing="0"
            Font-Names="Verdana"
            Font-Size="8pt"
            HeaderStyle-BackColor="#aaaadd"
            EnableViewState="true"
			OnPageIndexChanged="gv_PageIndexChanged"
			OnPageIndexChanging = "gv_PageIndexChanging" 
			AutoGenerateColumns="false"
			OnRowDeleting="Search"
			DataKeyNames="application_id"
			PageSize="200"
			AllowPaging="True">
				
		    <FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
            <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
            <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999"></PagerStyle>
            <PagerSettings Mode="NumericFirstLast" Position="Top" />
			<EditRowStyle BackColor="#E6E8FA" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <EmptyDataTemplate>
               <asp:Label ID="Label2" runat="server">
                   There is no data to display in this view. Get a valid Application ID.
               </asp:Label>
            </EmptyDataTemplate>
            					
			<Columns>
 		       <asp:ButtonField HeaderText="Select?" Text="Select" CommandName="Delete" ButtonType="Button" />
			   <asp:BoundField HeaderText="Application ID" DataField="application_id" ReadOnly="true" ItemStyle-Width="400px"/>
			   <asp:BoundField HeaderText="Application Name" DataField="application_name" ReadOnly="true" ItemStyle-Width="300px"/>

	           <asp:TemplateField HeaderText="Inactive">
		         <ItemTemplate>
                   <asp:CheckBox ID="InactiveCheckBox" Enabled="False"
                      Checked='<%# Databinder.Eval(Container.DataItem, "inactive") %>'
                      runat="server"/>
                 </ItemTemplate>          
               </asp:TemplateField>
			   
			</Columns>
		  </ASP:GridView>
		</td> 
		</tr> 
		<tr> 
		   <td>
             <input type="hidden" id="getsource" value="" runat="server" visible="false" />
           </td>
        </tr>
	</table>
  </form>

</body>
</html>
