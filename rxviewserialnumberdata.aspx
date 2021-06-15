﻿<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="rxviewserialnumberdata.aspx.vb" Inherits="RexairWebSecurity.rxviewserialnumberdata" EnableSessionState = "True"%>

<!--#include virtual="/ForceSSL.inc"-->

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RX View Serial Number Data</title>
        <link rel="stylesheet" href="../Content/wsSite.css"/>
        <script src="../Scripts/JavaScript.js" type="text/javascript"></script>
      <meta charset="utf-8"/>
 
</head>
<body oncontextmenu="return false">

    <form id="rxviewserialnumber" runat="server" >
       <div id="container">
          <div class="row">
               <div class="column left" > <h2>View Serial Number Data</h2>
               </div>
               <div class="column right" > <img src="../images/logo_small.png" alt="Rexair LLC"  width="200" height="100" />
               </div>
         </div>

        <div id="frame">
                <table width:"30%" class="font8veranda">
            <tr>
              <td class="aligntext">
			      <asp:label runat="server" Text="Serial Number:" />
              </td>
              <td>
                <asp:TextBox ID="serial_number" AutoPostBack="false" ReadOnly="false" runat="server" MaxLength="20" Width="150" CssClass="requiredfield" ToolTip="Enter an SRX Serial Number." TabIndex="1"></asp:TextBox> 
              </td>
               <td></td>
			  <td>
			     <input type="submit" id="btnGet" OnServerClick="GetData_Click" value="Get" runat="server" tabindex="2"/>
			  </td>  
             </tr>

    </table>
    <table class="table_input" >
	<tr>
        <td >
          <ASP:GridView id="MyGridView" runat="server"
            Width="950"
            BackColor="#CCCCCC"
            BorderColor="black"
            ShowFooter="true"
            CellPadding="3"
            CellSpacing="0"
            Font-Names="Verdana"
            Font-Size="8pt"
            HeaderStyle-BackColor="#aaaadd"
            EnableViewState="true"
			OnRowDataBound = "gvp_RowDataBound"
			OnRowCreated = "gvp_RowCreated"
			OnPreRender="gvp_PreRender"
			DataKeyNames="production_date"
			AutoGenerateColumns="false"
			AutoGenerateEditButton = "false"
			AutoGenerateDeleteButton = "false" 
			PageSize="3"
			AllowSorting="False"
			AllowPaging="False"
            ToolTip="Serial Number Production Information">
				
		    <FooterStyle forecolor="White" backcolor="Black"></FooterStyle>
            <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
            <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999" Font-Names="Verdana" Font-Size="Small"></PagerStyle>
            <PagerSettings Mode="NumericFirstLast" Position="Top"/>
			<EditRowStyle BackColor="#E6E8FA" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <EmptyDataTemplate>
               <asp:Label ID="Label1" runat="server">
                   There is no Serial Number data to display. Enter a valid Serial Number and Click Get.
               </asp:Label>
            </EmptyDataTemplate>
            					
			<Columns>
			   <asp:BoundField HeaderText="Prod Date" DataField="production_date" DataFormatString ="{0:yyyy/MM/dd}" ItemStyle-Width="70px" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
			   <asp:BoundField HeaderText="Part #" DataField="part_number" ItemStyle-Width="50px" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
			   <asp:BoundField HeaderText="Part Desc" DataField="item_description" ItemStyle-Width="250px" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
               <asp:BoundField HeaderText="Item Class" DataField="item_class" ItemStyle-Width="50px" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
               <asp:BoundField HeaderText="Country" DataField="country" ItemStyle-Width="70px" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
	           <asp:BoundField HeaderText="CB SN" DataField="CBSN" ItemStyle-Width="70px" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
               <asp:BoundField HeaderText="CB Date" DataField="CBSN_date" DataFormatString ="{0:yyyy/MM/dd}" ItemStyle-Width="100px" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
   	           <asp:BoundField HeaderText="PM SN" DataField="PMSN" ItemStyle-Width="70px" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
               <asp:BoundField HeaderText="PM Date" DataField="PMSN_date" DataFormatString ="{0:yyyy/MM/dd}" ItemStyle-Width="100px" ReadOnly="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
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
  <div >
    <div >
      <h3>Distributor Sale(s)</h3>
       <table width:"100%">
		<tr>
            <td >
                <ASP:GridView id="MyGridViewDistSales" runat="server"
                   Width="1000"
                   BackColor="#CCCCCC"
                   BorderColor="black"
                   ShowFooter="true"
                   CellPadding="3"
                   CellSpacing="0"
                   Font-Names="Verdana"
                   Font-Size="8pt"
                   HeaderStyle-BackColor="#aaaadd"
			       PageSize="10"
                   EnableViewState="true"
			       DataKeyNames="reference_date"
			       AutoGenerateColumns="false"
			       OnRowDataBound = "gv_RowDataBound"
			       AllowSorting="False"
			       AllowPaging="False" 
				   ToolTip="Distributor Sales">
								
		           <FooterStyle forecolor="White" backcolor="Black"></FooterStyle>
                   <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
                   <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999" 
                         Font-Names="Verdana" Font-Size="Small"></PagerStyle>
                   <PagerSettings Mode="NumericFirstLast" Position="Top"/>
		           <EditRowStyle BackColor="#AFEEEE" />
                   <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />

				   <Columns>
				          <asp:BoundField HeaderText="Ref Date" DataField="reference_date" ReadOnly="true" DataFormatString ="{0:yyyy/MM/dd}" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Ref Num" DataField="reference_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Item Class" DataField="item_group_major_class" ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				   	      <asp:BoundField HeaderText="Frm WH" DataField="from_cust_wh" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="Frm WH Name" DataField="from_cust_wh_name" ReadOnly="true" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="To Cust" DataField="to_cust_wh" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="To Cust Name" DataField="to_cust_wh_name" ReadOnly="true" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="RGD" DataField="rgd_number"  ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="RGD Name" DataField="rgd_name" ReadOnly="true" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
			       </Columns>
		          </ASP:GridView>
		    </td>  
		</tr>
	</table>	
    </div>
    <div >
      <h3>Closed Warranty</h3>
      <table width:"100%">
		<tr>
            <td >
                <ASP:GridView id="MyGridViewWarranty" runat="server"
                   Width="1150"
                   BackColor="#CCCCCC"
                   BorderColor="black"
                   ShowFooter="true"
                   CellPadding="3"
                   CellSpacing="0"
                   Font-Names="Verdana"
                   Font-Size="8pt"
                   HeaderStyle-BackColor="#aaaadd"
			       PageSize="15"
                   EnableViewState="true"
			       DataKeyNames="entry_start_date"
			       AutoGenerateColumns="false"
			       OnRowDataBound = "gv_RowDataBound"
			       AllowSorting="False"
			       AllowPaging="False" 
				   ToolTip="SRX Closed Warranty">
								
		           <FooterStyle forecolor="White" backcolor="Black"></FooterStyle>
                   <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
                   <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999" 
                         Font-Names="Verdana" Font-Size="Small"></PagerStyle>
                   <PagerSettings Mode="NumericFirstLast" Position="Top"/>
		           <EditRowStyle BackColor="#AFEEEE" />
                   <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />

				   <Columns>
				          <asp:BoundField HeaderText="Entry/Start Dt" DataField="entry_start_date" DataFormatString ="{0:yyyy/MM/dd}" ReadOnly="true" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Closed Dt" DataField="control_closing_date" DataFormatString ="{0:yyyy/MM/dd}" ReadOnly="true" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Cust" DataField="customer_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				   	      <asp:BoundField HeaderText="Cust Name" DataField="customer_name"  ReadOnly="true" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				   	      <asp:BoundField HeaderText="Cntry" DataField="country_code"  ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Ctrl #" DataField="return_control_number"  ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Tag" DataField="tag_number"  ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Part" DataField="part_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="Part Desc" DataField="part_description"  ReadOnly="true" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
					      <asp:BoundField HeaderText="Ln Qty" DataField="line_qty"  ReadOnly="true" ItemStyle-Width="30px" DataFormatString ="{0:n0}" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Right"/> 
            		      <asp:BoundField HeaderText="Disp Desc" DataField="disposition_desc"  ReadOnly="true" ItemStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
            		      <asp:BoundField HeaderText="Denial Desc" DataField="denial_desc"  ReadOnly="true"  ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
            		      <asp:BoundField HeaderText="Days On Mkt" DataField="days_onmarket"  ReadOnly="true"  ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/> 
           		          <asp:BoundField HeaderText="Mths On Mrkt" DataField="months_onmarket"  ReadOnly="true"  ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/> 

			       </Columns>
		          </ASP:GridView>
		    </td>  
		</tr>
	</table>	
    </div>
    <div >
      <h3>Consumer Sale(s)</h3>
      <table width:"100%">
		<tr>
            <td >
                <ASP:GridView id="MyGridViewConsSales" runat="server"
                   Width="1100"
                   BackColor="#CCCCCC"
                   BorderColor="black"
                   ShowFooter="true"
                   CellPadding="3"
                   CellSpacing="1"
                   Font-Names="Verdana"
                   Font-Size="8pt"
                   HeaderStyle-BackColor="#aaaadd"
			       PageSize="10"
                   EnableViewState="true"
			       DataKeyNames="reporting_entry_Date"
			       AutoGenerateColumns="false"
			       OnRowDataBound = "gv_RowDataBound"
			       AllowSorting="False"
			       AllowPaging="False" 
				   ToolTip="Sales to the Consumer">
								
		           <FooterStyle forecolor="White" backcolor="Black"></FooterStyle>
                   <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
                   <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999" 
                         Font-Names="Verdana" Font-Size="Small"></PagerStyle>
                   <PagerSettings Mode="NumericFirstLast" Position="Top"/>
		           <EditRowStyle BackColor="#AFEEEE" />
                   <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />

				   <Columns>
				          <asp:BoundField HeaderText="Rpt/Entry Dt" DataField="reporting_entry_date"  DataFormatString ="{0:yyyy/MM/dd}" ReadOnly="true" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Seq #" DataField="sequence_number"  ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Batch #" DataField="batch_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Type" DataField="sale_type"  ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="PZ SN" DataField="power_nozzle_serial_number"  ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="C Cd" DataField="country_code" ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				   	      <asp:BoundField HeaderText="Country" DataField="country_code_name"  ReadOnly="true" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="City" DataField="city"  ReadOnly="true" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="ST" DataField="state_prov_code"  ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
				          <asp:BoundField HeaderText="Postal" DataField="zip_code" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="CA Postal" DataField="canada_postal_code"  ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
				          <asp:BoundField HeaderText="RGD" DataField="rgd_number" ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="RGD Name" DataField="rgd_name"  ReadOnly="true" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
				          <asp:BoundField HeaderText="Region" DataField="region_number"  ReadOnly="true" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
					      <asp:BoundField HeaderText="Region Desc" DataField="region_desc"  ReadOnly="true" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 
   					      <asp:BoundField HeaderText="Dup/Rms" DataField="duplicate_remorse_flag"  ReadOnly="true" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/> 

			       </Columns>
		          </ASP:GridView>
		    </td>  
		</tr>
	</table>	
    </div>
   </div>
</div>
       <table  >
           <tr>
           <td >
               <asp:Button id="btnExcelDistSales" Text="Dist Sales To Excel" OnClick="WindowExcelDistSales" runat="server" ToolTip="Click to export the Distributor Sales data to Excel." TabIndex="5"/>
		   </td>
           <td >
               <asp:Button id="btnExcelWarranty" Text="Warranty To Excel" OnClick="WindowExcelWarranty" runat="server" ToolTip="Click to export the Closed Warranty data to Excel." TabIndex="6"/>
		   </td>
           <td >
               <asp:Button id="btnExcelConsSales" Text="Cons Sales To Excel" OnClick="WindowExcelConsSales" runat="server" ToolTip="Click to export the Consumer Sales data to Excel." TabIndex="7"/>
		   </td>
             <td>
   	           <asp:Button id="btnReturn" Text="Return to Main Menu" OnClick="ReturnToMainMenu" runat="server" ToolTip="Click to return to the Web Security Main Menu. ALT+R" AccessKey="R" TabIndex="8"/>
		     </td>
		   </tr>
         </table>
          <table >
           <tr>
            <td>
                <input type="hidden" name="gridcountDistSales" id="gridcountDistSales" runat="server" />
                <input type="hidden" name="gridcountWarranty" id="gridcountWarranty" runat="server" />
                <input type="hidden" name="gridcountConsSales" id="gridcountConsSales" runat="server" />
            </td>
           </tr>

         </table>     
        </div>
        </div>
    </form>
</body>
</html>
