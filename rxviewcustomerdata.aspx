<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="rxviewcustomerdata.aspx.vb" Inherits="RexairWebSecurity.rxviewcustomerdata" EnableSessionState = "True" EnableEventValidation ="false"%>

<!--#include virtual="/ForceSSL.inc"-->

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RX View Customer Data</title>
        <link rel="stylesheet" href="../Content/wsSite.css"/>
        <script src="../Scripts/JavaScript.js" type="text/javascript"></script>
      <meta charset="utf-8"/>
  <meta name="viewport" content="width=device-width, initial-scale=1"/>
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css"/>
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
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

    <form id="rxviewcustomerdata" runat="server" >
       <div id="container">
          <div class="row">
               <div class="column left" > <h3>View Customer Data</h3> </div>
               <div class="column right" > <img src="../images/logo_small.png" alt="Rexair LLC"  width="100%" height="140%" />  </div>
         </div>

        <div id="frame">
         <table width:"50%" class="font8veranda">
            <tr>
              <td class="aligntext">
			      <asp:label runat="server" Text="Customer:" />
              </td>
              <td>
                <input type="button" id="btnSearch" onclick='pickCust("customer_number");' value="S" runat="server" />
                <asp:TextBox ID="customer_number" AutoPostBack="false" ReadOnly="false" runat="server" MaxLength="20" Width="150" CssClass="requiredfield" ToolTip="Enter an Custoemr Number." TabIndex="1"></asp:TextBox> 
              </td>
              <td>
                 <asp:TextBox ID="customer_name" AutoPostBack="false" ReadOnly="false" runat="server" MaxLength="100" Width="250" TabIndex="0"></asp:TextBox> 
               </td>
               <td></td>
               <td>
                  <input type="submit" id="btnGet" OnServerClick="GetData_Click" value="Get Customer Info" runat="server" tabindex="2"/>
              </td>
                <td></td>
              <td>
                 <asp:Button Text="Clear" OnClick="PageReset_Click" runat="server" ToolTip="Click to reset the window. ALT+C" TabIndex="4" AccessKey="C"/>
              </td>
             </tr>
             <tr>
              <td class="aligntext; font8veranda">
			      <asp:label runat="server" Text="Starting Date (yyyy-mm-dd):" />
              </td>
              <td>
                <asp:TextBox ID="starting_date" AutoPostBack="false" ReadOnly="false" runat="server" MaxLength="20" Width="150" CssClass="requiredfield" ToolTip="Enter a Starting Date." TabIndex="2"></asp:TextBox> 
              </td>
             </tr>
             <tr>
              <td class="aligntext; font8veranda">
			      <asp:label runat="server" Text="Ending Date (yyyy-mm-dd):" />
              </td>
              <td>
                <asp:TextBox ID="ending_date" AutoPostBack="false" ReadOnly="false" runat="server" MaxLength="20" Width="150" CssClass="requiredfield" ToolTip="Enter an Ending Date." TabIndex="3"></asp:TextBox> 
               </td>
             </tr>
             <tr>
              <td class="aligntext; font8veranda">
			      <asp:label runat="server" Text="SRX Rainbows Only:" />
              </td>
              <td>
                 <asp:CheckBox ID="cbrainbows_only" Checked="True" AutoPostBack="false" Enabled="true" runat="server" ToolTip="Checked to pull data only for SRX Rainbows." TabIndex="4"></asp:CheckBox> 
               </td>
             </tr>
 

    </table>
    <table class="table_input">
	<tr>
        <td >
          <ASP:GridView id="MyGridView" runat="server"
            Width="1000"
            BackColor="#CCCCCC"
            BorderColor="black"
            ShowFooter="false"
            CellPadding="10"
            CellSpacing="5"
            Font-Names="Verdana"
            Font-Size="8pt"
            HeaderStyle-BackColor="#aaaadd"
            EnableViewState="true"
			OnPageIndexChanged = "gvp_PageIndexChanged" 
			OnPageIndexChanging = "gvp_PageIndexChanging" 
			OnRowDataBound = "gvp_RowDataBound"
			OnRowCreated = "gvp_RowCreated"
			OnSorting="gvp_Sorting" 
			OnPreRender="gvp_PreRender"
			DataKeyNames="curr_status_effective_date"
			AutoGenerateColumns="false"
			AutoGenerateEditButton = "false"
			AutoGenerateDeleteButton = "false" 
			PageSize="3"
			AllowSorting="False"
			AllowPaging="False">
				
		    <FooterStyle forecolor="Black" backcolor="Black"></FooterStyle>
            <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
            <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999" Font-Names="Verdana" Font-Size="Small"></PagerStyle>
            <PagerSettings Mode="NumericFirstLast" Position="Top"/>
			<EditRowStyle BackColor="#E6E8FA" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <EmptyDataTemplate>
               <asp:Label ID="Label1" runat="server">
                   There is no Customer data to display. Enter a valid Customer Number and Click Get.
               </asp:Label>
            </EmptyDataTemplate>
            					
			<Columns>
			   <asp:BoundField HeaderText="Stat Eff Dt" DataField="curr_status_effective_date" DataFormatString ="{0:yyyy/MM/dd}" ItemStyle-Width="70px" ReadOnly="true"/> 
               <asp:BoundField HeaderText="Status" DataField="customer_status" ItemStyle-Width="30px" ReadOnly="true" />
               <asp:BoundField HeaderText="Term Dt" DataField="termination_date" DataFormatString ="{0:MM/dd/yyyy}" ItemStyle-Width="70px" ReadOnly="true"/> 
			   <asp:BoundField HeaderText="City" DataField="addressline3" ItemStyle-Width="70px" ReadOnly="true" />
			   <asp:BoundField HeaderText="ST" DataField="state" ItemStyle-Width="30px" ReadOnly="true" />
               <asp:BoundField HeaderText="Ctry" DataField="addressing_country" ItemStyle-Width="30px" ReadOnly="true" />
 	           <asp:BoundField HeaderText="Seq" DataField="cust_deliv_seq" ItemStyle-Width="30px" ReadOnly="true" />
               <asp:BoundField HeaderText="Class" DataField="customer_class" ItemStyle-Width="30px" ReadOnly="true" />
               <asp:BoundField HeaderText="Lst Pur Dt" DataField="last_purchase_date" DataFormatString ="{0:yyyy/MM/dd}" ItemStyle-Width="70px" ReadOnly="true"/> 
               <asp:BoundField HeaderText="Resp To" DataField="responsible_to_number" ItemStyle-Width="50px" ReadOnly="true" />
               <asp:BoundField HeaderText="Resp To Name" DataField="responsible_to_name" ItemStyle-Width="150px" ReadOnly="true" />
               <asp:BoundField HeaderText="RGD" DataField="rgd_number" ItemStyle-Width="50px" ReadOnly="true" />
               <asp:BoundField HeaderText="RGD Name" DataField="rgd_name" ItemStyle-Width="150px" ReadOnly="true" />
			</Columns>
		  </ASP:GridView>
		</td>  
        </tr>
        <tr>
            <td>
       <h4>Customers Responsible To</h4>
            </td>
        </tr>
        <tr>

        <td >
          <ASP:GridView id="MyGridViewRespTo" runat="server"
            Width="1100"
            BackColor="#CCCCCC"
            BorderColor="black"
            ShowFooter="true"
            CellPadding="10"
            CellSpacing="5"
            Font-Names="Verdana"
            Font-Size="8pt"
            HeaderStyle-BackColor="#aaaadd"
            EnableViewState="true"
			OnPageIndexChanged = "gvr_PageIndexChanged" 
			OnPageIndexChanging = "gvr_PageIndexChanging" 
			OnRowDataBound = "gvr_RowDataBound"
			OnRowCreated = "gvr_RowCreated"
			OnSorting="gvr_Sorting" 
			OnPreRender="gvr_PreRender"
			DataKeyNames="curr_status_effective_date"
			AutoGenerateColumns="false"
			AutoGenerateEditButton = "false"
			AutoGenerateDeleteButton = "false" 
			PageSize="10"
			AllowSorting="False"
			AllowPaging="False">
				
		    <FooterStyle forecolor="Black" backcolor="Black"></FooterStyle>
            <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
            <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999" Font-Names="Verdana" Font-Size="Small"></PagerStyle>
            <PagerSettings Mode="NumericFirstLast" Position="Top"/>
			<EditRowStyle BackColor="#E6E8FA" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <EmptyDataTemplate>
               <asp:Label ID="Label1" runat="server">
                   There are no Responsible To Customers to display.
               </asp:Label>
            </EmptyDataTemplate>
            					
			<Columns>
               <asp:BoundField HeaderText="Cust" DataField="customer_number" ItemStyle-Width="50px" ReadOnly="true" />
               <asp:BoundField HeaderText="Cust Name" DataField="customer_name" ItemStyle-Width="200px" ReadOnly="true" />
			   <asp:BoundField HeaderText="Stat Eff Dt" DataField="curr_status_effective_date" DataFormatString ="{0:yyyy/MM/dd}" ItemStyle-Width="70px" ReadOnly="true"/> 
               <asp:BoundField HeaderText="Status" DataField="customer_status" ItemStyle-Width="30px" ReadOnly="true" />
               <asp:BoundField HeaderText="Term Dt" DataField="termination_date" DataFormatString ="{0:yyyy/MM/dd}" ItemStyle-Width="70px" ReadOnly="true"/> 
			   <asp:BoundField HeaderText="City" DataField="addressline3" ItemStyle-Width="200px" ReadOnly="true" />
			   <asp:BoundField HeaderText="ST" DataField="state" ItemStyle-Width="30px" ReadOnly="true" />
               <asp:BoundField HeaderText="Ctry" DataField="addressing_country" ItemStyle-Width="30px" ReadOnly="true" />
 	           <asp:BoundField HeaderText="Seq" DataField="cust_deliv_seq" ItemStyle-Width="30px" ReadOnly="true" />
               <asp:BoundField HeaderText="Class" DataField="customer_class" ItemStyle-Width="30px" ReadOnly="true" />
               <asp:BoundField HeaderText="Lst Pur Dt" DataField="last_purchase_date" DataFormatString ="{0:yyyy/MM/dd}" ItemStyle-Width="70px" ReadOnly="true"/> 
               <asp:BoundField HeaderText="RGD" DataField="rgd_number" ItemStyle-Width="30px" ReadOnly="true" />
               <asp:BoundField HeaderText="RGD Name" DataField="rgd_name" ItemStyle-Width="200px" ReadOnly="true" />
			</Columns>
		  </ASP:GridView>
		</td>  
        </tr>
        <tr>
        <td colspan="1" class="paddingtop15; textaligncenter">
           <span id="GetMessage" EnableViewState="false" class="font11arial" runat="server"/>
        </td>
       </tr>
</table>
    <hr class="hrheight2"/>

 <div class="container">
  <h4>Customer Data</h4>
  <hr style="height:2px;border-width:0;color:gray;background-color:gray"/>
  <ul class="nav nav-pills" id="myid">
    <li class="active"><a data-toggle="pill" href="#home">Dist Sales</a></li>
    <li><a data-toggle="pill" href="#menu1">Warranty</a></li>
    <li><a data-toggle="pill" href="#menu2">Consumer Sales</a></li>
    <li><a data-toggle="pill" href="#menu3">Open</a></li>
  </ul>
  
  <div class="tab-content">
    <div id="home" class="tab-pane fade in active">
      <h3>Dist Sales</h3>
      <p>Sales to the Distributor.</p>
       <table width:"100%">
		<tr>
            <td >
                <ASP:GridView id="MyGridViewDistSales" runat="server"
                   Width="1000"
                   BackColor="#CCCCCC"
                   BorderColor="black"
                   ShowFooter="true"
                   CellPadding="10"
                   CellSpacing="5"
                   Font-Names="Verdana"
                   Font-Size="8pt"
                   HeaderStyle-BackColor="#aaaadd"
			       PageSize="1000"
                   EnableViewState="true"
			       DataKeyNames="reference_date"
			       AutoGenerateColumns="false"
				   OnPageIndexChanged = "gv_PageIndexChangedDistSales" 
			       OnPageIndexChanging = "gv_PageIndexChangingDistSales" 
			       OnRowDataBound = "gv_RowDataBoundDistSales"
			       OnSorting="gv_SortingDistSales" 
			       AllowSorting="True"
			       AllowPaging="True" 
				   ToolTip="Dist Sales">
								
		           <FooterStyle forecolor="White" backcolor="Black"></FooterStyle>
                   <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
                   <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999" Font-Names="Verdana" Font-Size="Small"></PagerStyle>
                   <PagerSettings Mode="NumericFirstLast" Position="Top"/>
		           <EditRowStyle BackColor="#AFEEEE" />
                   <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />

				   <Columns>
				          <asp:BoundField HeaderText="Ref Date" DataField="reference_date" SortExpression="reference_date" ReadOnly="true" DataFormatString ="{0:yyyy/MM/dd}" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Ref Number" DataField="reference_number" SortExpression="reference_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Item Class" DataField="item_group_major_class" SortExpression="item_group_major_class" ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				   	      <asp:BoundField HeaderText="From WH" DataField="from_cust_wh" SortExpression="from_cust_wh" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="Fr Cust Name" DataField="from_cust_wh_name" SortExpression="from_cust_wh_name" ReadOnly="true" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="Serial Number" DataField="serial_number" SortExpression="serial_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="RGD #" DataField="rgd_number" SortExpression="rgd_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="RGD Name" DataField="rgd_name" SortExpression="rgd_name" ReadOnly="true" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
                          <asp:TemplateField ItemStyle-Width="3">
                             <FooterTemplate >
                               <%# MyGridViewDistSales.Rows.Count %>
                            </FooterTemplate>
                        </asp:TemplateField>
			       </Columns>
		          </ASP:GridView>
		    </td>  
		</tr>
	</table>	
    </div>
    <div id="menu1" class="tab-pane fade">
      <h3>Warranty</h3>
      <p>Closed Warranty.</p>
      <table width:"100%">
		<tr>
            <td >
                <ASP:GridView id="MyGridViewWarranty" runat="server"
                   Width="1150"
                   BackColor="#CCCCCC"
                   BorderColor="black"
                   ShowFooter="true"
                   CellPadding="10"
                   CellSpacing="5"
                   Font-Names="Verdana"
                   Font-Size="8pt"
                   HeaderStyle-BackColor="#aaaadd"
			       PageSize="1000"
                   EnableViewState="true"
			       DataKeyNames="entry_start_date"
			       AutoGenerateColumns="false"
				   OnPageIndexChanged = "gv_PageIndexChangedWarranty" 
			       OnPageIndexChanging = "gv_PageIndexChangingWarranty" 
			       OnRowDataBound = "gv_RowDataBoundWarranty"
			       OnSorting="gv_SortingWarranty" 
			       AllowSorting="True"
			       AllowPaging="True" 
				   ToolTip="Closed Warranty">
								
		           <FooterStyle forecolor="White" backcolor="Black"></FooterStyle>
                   <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
                   <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999" 
                         Font-Names="Verdana" Font-Size="Small"></PagerStyle>
                   <PagerSettings Mode="NumericFirstLast" Position="Top"/>
		           <EditRowStyle BackColor="#AFEEEE" />
                   <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />

				   <Columns>
				          <asp:BoundField HeaderText="Ent/Start Dt" DataField="entry_start_date" SortExpression="entry_start_date" DataFormatString ="{0:yyyy/MM/dd}" ReadOnly="true" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Closed Date" DataField="control_closing_date" SortExpression="control_closing_date" DataFormatString ="{0:yyyy/MM/dd}" ReadOnly="true" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Serial #" DataField="serial_number" SortExpression="serial_number" ReadOnly="true" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				   	      <asp:BoundField HeaderText="CC" DataField="country_code" SortExpression="country_code" ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Ctrl #" DataField="return_control_number" SortExpression="return_control_number" ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Tag" DataField="tag_number" SortExpression="tag_number" ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Part #" DataField="part_number" SortExpression="part_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="Part Desc" DataField="part_description" SortExpression="part_description" ReadOnly="true" ItemStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
					      <asp:BoundField HeaderText="Ln Qty" DataField="line_qty" ReadOnly="true" ItemStyle-Width="30px" DataFormatString ="{0:n0}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right"/> 
            		      <asp:BoundField HeaderText="Disp Desc" DataField="disposition_desc" ReadOnly="true" SortExpression="disposition_desc" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
            		      <asp:BoundField HeaderText="Denial Desc" DataField="denial_desc" ReadOnly="true" SortExpression="denial_desc" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
              		      <asp:BoundField HeaderText="Type" DataField="machine_type" ReadOnly="true" SortExpression="machine_type" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
              		      <asp:BoundField HeaderText="MS" DataField="machine_series" ReadOnly="true" SortExpression="machine_series" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
                          <asp:BoundField HeaderText="DOM" DataField="days_onmarket" ReadOnly="true" SortExpression="days_onmarket" ItemStyle-Width="20px" DataFormatString ="{0:n0}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/> 
           		          <asp:BoundField HeaderText="MOM" DataField="months_onmarket"  ReadOnly="true" SortExpression="months_onmarket" ItemStyle-Width="20px" DataFormatString ="{0:n0}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/> 
                           <asp:TemplateField ItemStyle-Width="3">
                             <FooterTemplate >
                               <%# MyGridViewWarranty.Rows.Count %>
                            </FooterTemplate>
                        </asp:TemplateField>
			       </Columns>
		          </ASP:GridView>
		    </td>  
		</tr>
	</table>	
    </div>
    <div id="menu2" class="tab-pane fade">
      <h3>Consumer Sales</h3>
      <p>Sale to the Consumer.</p>
              <table width:"100%">
		<tr>
            <td >
                <ASP:GridView id="MyGridViewConsSales" runat="server"
                   Width="1100"
                   BackColor="#CCCCCC"
                   BorderColor="black"
                   ShowFooter="true"
                   CellPadding="10"
                   CellSpacing="5"
                   Font-Names="Verdana"
                   Font-Size="8pt"
                   HeaderStyle-BackColor="#aaaadd"
			       PageSize="1000"
                   EnableViewState="true"
			       DataKeyNames="reporting_entry_Date"
			       AutoGenerateColumns="false"
				   OnPageIndexChanged = "gv_PageIndexChangedConsSale" 
			       OnPageIndexChanging = "gv_PageIndexChangingConsSale" 
			       OnRowDataBound = "gv_RowDataBoundConsSale"
			       OnSorting="gv_SortingConsSale" 
			       AllowSorting="True"
			       AllowPaging="True" 
				   ToolTip="Sales to the Consumer">
								
		           <FooterStyle forecolor="White" backcolor="Black"></FooterStyle>
                   <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
                   <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999" 
                         Font-Names="Verdana" Font-Size="Small"></PagerStyle>
                   <PagerSettings Mode="NumericFirstLast" Position="Top"/>
		           <EditRowStyle BackColor="#AFEEEE" />
                   <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />

				   <Columns>
				          <asp:BoundField HeaderText="Rpt/Entry Dt" DataField="reporting_entry_date" SortExpression="reporting_entry_date" DataFormatString ="{0:yyyy/MM/dd}" ReadOnly="true" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Seq #" DataField="sequence_number" SortExpression="sequence_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Batch #" DataField="batch_number" SortExpression="batch_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Type" DataField="sale_type" SortExpression="sale_type" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="RB SN" DataField="rainbow_serial_number" SortExpression="rainbow_serial_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="PZ SN" DataField="power_nozzle_serial_number" SortExpression="power_nozzle_serial_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="C Cd" DataField="country_code" SortExpression="country_code" ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				   	      <asp:BoundField HeaderText="Country" DataField="country_code_name" SortExpression="country_code_name" ReadOnly="true" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="City" DataField="city" SortExpression="city" ReadOnly="true" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="ST" DataField="state_prov_code" SortExpression="state_prov_code" ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Postal" DataField="zip_code" SortExpression="zip_code" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="CA Postal" DataField="canada_postal_code" SortExpression="canada_postal_code" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
				          <asp:BoundField HeaderText="Region #" DataField="region_number" SortExpression="region_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="Region Desc" DataField="region_desc" SortExpression="region_desc" ReadOnly="true" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
					      <asp:BoundField HeaderText="Dup/Rms" DataField="duplicate_remorse_flag" SortExpression="duplicate_remorse_flag" ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
                          <asp:TemplateField ItemStyle-Width="3">
                             <FooterTemplate >
                               <%# MyGridViewConsSales.Rows.Count %>
                            </FooterTemplate>
                         </asp:TemplateField>
			       </Columns>

		          </ASP:GridView>
		    </td>  
		</tr>
	</table>	
    </div>
    <div id="menu3" class="tab-pane fade">
      <h3>Open</h3>
      <p>TBD.</p>
    </div>
  </div>
</div>
    <br/><br/>
    <hr/>	
       <table >
           <tr>
             <td >
               <asp:Button id="btnExcelDistSales" Text="Dist Sales To Excel" OnClick="WindowExcelDistSales" runat="server" ToolTip="Click to export the Distributor Sales data to Excel." TabIndex="5"/>
		     </td>
             <td></td>
             <td >
               <asp:Button id="btnExcelWarranty" Text="Warranty To Excel" OnClick="WindowExcelWarranty" runat="server" ToolTip="Click to export the Closed Warranty data to Excel." TabIndex="6"/>
		     </td>
             <td></td>
             <td >
               <asp:Button id="btnExcelConsSales" Text="Cons Sales To Excel" OnClick="WindowExcelConsSales" runat="server" ToolTip="Click to export the Consumer Sales data to Excel." TabIndex="7"/>
		     </td>
            <td></td>
             <td >
               <asp:Button id="btnWindowPrint" Text="Web Page Print" OnClientClick="javascript:window.print();" runat="server" ToolTip="Click to print the Web Page." TabIndex="7"/>
             </td>
             <td></td>
             <td>
   	           <asp:Button id="btnReturn" Text="Return to Main Menu" OnClick="ReturnToMainMenu" runat="server" ToolTip="Click to return to the Web Security Main Menu. ALT+R" AccessKey="R" TabIndex="8"/>
		     </td>
		   </tr>
        </table>
        <table >
           <tr>
            <td>
                <asp:TextBox ID="sortdistsales_id" runat="server" AutoPostBack="false" MaxLength="40"  ReadOnly="true" Visible="false" Width="40"></asp:TextBox>
                <asp:TextBox ID="sortwarranty_id" runat="server" AutoPostBack="false" MaxLength="40"  ReadOnly="true"  Visible="false" Width="40"></asp:TextBox>
                <asp:TextBox ID="sortconssales_id" runat="server" AutoPostBack="false" MaxLength="40"  ReadOnly="true" Visible="false" Width="40"></asp:TextBox>
                <input type="hidden" name="gridcountDistSales" id="gridcountDistSales" runat="server" />
                <input type="hidden" name="gridcountWarranty" id="gridcountWarranty" runat="server" />
                <input type="hidden" name="gridcountConsSales" id="gridcountConsSales" runat="server" />
                <input type="text" id="getsource" name="getsource" runat="server" tvalue="" visible="false" readonly="true"/>
            </td>
           </tr>

         </table>
        
        </div>
        </div>

    </form>
</body>
</html>
