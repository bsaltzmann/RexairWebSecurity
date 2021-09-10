<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="wsgroupmastersearch.aspx.vb" Inherits="RexairWebSecurity.wsgroupmastersearch" EnableSessionState = "True"%>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WS Group Master Search</title>
     <link rel="stylesheet" href="../Content/wsSite.css"/>

    <style type="text/css" media="screen">
    #getuserid
    {
        width: 317px;
    }
    #getusername
    {
        width: 275px;
    }

        #getgroupname {
            width: 175px;
        }
        #getgroupid {
            width: 146px;
        }

    </style>
</head>
<body class="font8veranda" oncontextmenu="return false">

  <form runat="server" id="wsusermastersearch">
        <div id="container">
          <div class="row">
               <div class="column left" > <h1>Group Master Search</h1>
               </div>
               <div class="column right" > <img src="../images/logo_small.png" alt="Rexair LLC"  width="100" height="50" />
               </div>
         </div>

        <div id="frame">
   
     <table style="width:50%;" class="font8veranda">
             <tr>
			  <td>
			  <strong>Search Type: </strong><asp:ListBox id="GroupSelect" runat="server" Rows="1" OnSelectedIndexChanged="DropChange_Click" AutoPostBack="true" TabIndex="1">
			     <asp:ListItem>Group ID</asp:ListItem>
			     <asp:ListItem>Group Name</asp:ListItem>	
			  </asp:ListBox>
			  </td>
			  <td></td>
			 </tr>  
             <tr>
  			  <td>
                <strong>Group ID: </strong><input type="text" id="getgroupid" value="" 
                      runat="server" maxlength="128" tabindex="1" />
              </td>
			  <td>
                <strong>Group Name: </strong> <input type="text" id="getgroupname" value="" 
                      runat="server" maxlength="100" disabled="disabled" />
              </td>
			  <td>
			     <input type="submit" id="btnGet" OnServerClick="GetData_Click" value="Search" runat="server" title="Click to Search based on criteria entered. ALT+S" accesskey="S"/>
			  </td>  
			  <td>
			     <input type="button" id="btnClose" OnServerClick="Close_Click" value="Close" runat="server" title="Click to Close the window without returning a Group. ALT+C" accesskey="C"/>
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
          <ASP:GridView ID="MyGridView" CssClass="footable" runat="server"
            Width = "600"
            BackColor = "#CCCCCC"
            BorderColor = "black"
            ShowFooter = "false"
            CellPadding = "3"
            CellSpacing = "0"
            Font-Names = "Verdana"
            Font-Size = "8pt"
            HeaderStyle-BackColor = "#aaaadd"
            EnableViewState = "true"
            OnPageIndexChanged = "gv_PageIndexChanged"
            OnPageIndexChanging = "gv_PageIndexChanging"
            AutoGenerateColumns = "false"
            OnRowDeleting = "Search"
            DataKeyNames = "group_id"
            PageSize = "100"
            AllowPaging = "True" >

              
		    <FooterStyle forecolor = "Black" backcolor="#CCCCCC"></FooterStyle>
            <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
            <PagerStyle horizontalalign = "Right" forecolor="Black" backcolor="#999999"></PagerStyle>
            <PagerSettings Mode = "NumericFirstLast" Position="Top" />
			<EditRowStyle BackColor = "#E6E8FA" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333"/>
            <EmptyDataTemplate>
               <asp:Label ID = "Label2" runat="server">
                   There Is no data to display in this view. Get a valid Group ID.
               </asp:Label>
            </EmptyDataTemplate>
            					
			<Columns>
 		       <asp:ButtonField HeaderText = "Select?" Text="Select" CommandName="Delete" ButtonType="Button" />
			   <asp:BoundField HeaderText = "Group ID" DataField="group_id" ReadOnly="true" ItemStyle-Width="400px"/>
			   <asp:BoundField HeaderText = "Group Name" DataField="group_name" ReadOnly="true" ItemStyle-Width="300px"/>

	           <asp:TemplateField HeaderText = "Inactive" >
                 <ItemTemplate>
                     <asp:CheckBox ID="InactiveCheckBox" Enabled="False"
                         Checked='<%# DataBinder.Eval(Container.DataItem, "inactive") %>'
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
    </div>
    </div>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jquery-footable/0.1.0/css/footable.min.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-footable/0.1.0/js/footable.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#MyGridView').footable({
                "breakpoints": {
                    "x-small": 480,
                    "small": 768,
                    "medium": 992,
                    "large": 1200,
                    "x-large": 1400
                },
                "toggleColumn": "last",
		        "expandFirst": true,
            });
        });
    </script>
  </form>

</body>
</html>
