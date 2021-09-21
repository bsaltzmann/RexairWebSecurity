<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="rxcustomermastersearch.aspx.vb" Inherits="RexairWebSecurity.rxcustomermastersearch" EnableSessionState = "True"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RX Customer Master Search</title>
      <link rel="stylesheet" href="../Content/wsSite.css"/>

    <style type="text/css" media="screen">
        #getuserid
    {
        width: 160px;
    }
    #getusername
    {
        width: 186px;
    }

</style>
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
<body class="font8veranda" oncontextmenu="return false">

  <form runat="server" id="rxcustomermastersearch">
      <div id="container">
          <div class="row">
               <div class="column left" > <h1>Customer Master Search</h1>
               </div>
               <div class="column right" > <img src="../images/logo_small.png" alt="Rexair LLC"  width="100%" height="140%" />
               </div>
         </div>

        <div id="frame">
   
     <table style="width:40%;" class="font8veranda">
             <tr>
			  <td>
			  <strong>Search Type: </strong><asp:ListBox id="CustSelect" runat="server" Rows="1" OnSelectedIndexChanged="DropChange_Click" AutoPostBack="true" TabIndex="1">
                 <asp:ListItem>Customer Name</asp:ListItem>
			     <asp:ListItem>Customer Number</asp:ListItem>
 	
			  </asp:ListBox>
			  </td>
			  <td></td>
			 </tr>  
             <tr>
  			  <td>
                <strong>Customer Num: </strong><input type="text" id="getcustomerid" value="" runat="server" maxlength="50" tabindex="1" />
              </td>
			  <td>
                <strong>Customer Name: </strong> <input type="text" id="getcustomername" value="" runat="server" maxlength="100" disabled="disabled" />
              </td>
			  <td>
			     <input type="submit" id="btnGet" OnServerClick="GetData_Click" value="Search" runat="server" title="Click to Search based on criteria entered. ALT+S" accesskey="S"/>
			  </td>  
			  <td>
			     <input type="button" id="btnClose" OnServerClick="Close_Click" value="Close" runat="server" title="Click to Close the window without returning a User. ALT+C" accesskey="C"/>
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
          <ASP:GridView ID="MyGridView" runat="server"
            Width="900"
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
			DataKeyNames="customer_number"
			PageSize="500"
			AllowPaging="True">
				
		    <FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
            <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
            <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999"></PagerStyle>
            <PagerSettings Mode="NumericFirstLast" Position="Top" />
			<EditRowStyle BackColor="#E6E8FA" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <EmptyDataTemplate>
               <asp:Label ID="Label2" runat="server">
                   There is no data to display in this view. Get a valid Customer ID.
               </asp:Label>
            </EmptyDataTemplate>
            					
			<Columns>
 		       <asp:ButtonField HeaderText="Select?" Text="Select" CommandName="Delete" ButtonType="Button" />
			   <asp:BoundField HeaderText="Customer" DataField="customer_number" ReadOnly="true" ItemStyle-Width="70px"/>
			   <asp:BoundField HeaderText="Customer Name" DataField="customer_name" ReadOnly="true" ItemStyle-Width="250px"/>
               <asp:BoundField HeaderText="Seq" DataField="cust_deliv_seq" ReadOnly="true" ItemStyle-Width="70px"/>
               <asp:BoundField HeaderText="Status" DataField="customer_status" ReadOnly="true" ItemStyle-Width="30px"/>
               <asp:BoundField HeaderText="ST" DataField="state" ReadOnly="true" ItemStyle-Width="30px"/>
               <asp:BoundField HeaderText="Class" DataField="customer_class" ReadOnly="true" ItemStyle-Width="30px"/>
               <asp:BoundField HeaderText="Cntry" DataField="addressing_country" ReadOnly="true" ItemStyle-Width="30px"/>
               <asp:BoundField HeaderText="Term Dt" DataField="termination_date" DataFormatString ="{0:MM/dd/yyyy}" ItemStyle-Width="70px" ReadOnly="true"/> 
               <asp:BoundField HeaderText="Lst Pur Dt" DataField="last_purchase_date" DataFormatString ="{0:yyyy/MM/dd}" ItemStyle-Width="70px" ReadOnly="true"/> 
               <asp:BoundField HeaderText="Resp To" DataField="responsible_to_number" ReadOnly="true" ItemStyle-Width="70px"/>
			   <asp:BoundField HeaderText="Resp To Name" DataField="responsible_to_name" ReadOnly="true" ItemStyle-Width="250px"/>
               <asp:BoundField HeaderText="RGD" DataField="rgd_number" ReadOnly="true" ItemStyle-Width="70px"/>
			   <asp:BoundField HeaderText="RGD" DataField="rgd_name" ReadOnly="true" ItemStyle-Width="250px"/>
			   
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
  </form>

</body>
</html>
