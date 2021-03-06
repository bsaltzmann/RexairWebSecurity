<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="wsapplicationmastermaint.aspx.vb" Inherits="RexairWebSecurity.wsapplicationmastermaint" EnableSessionState = "True"%>

<!--#include virtual="/ForceSSL.inc"-->

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WS Application Master Maintenance</title>
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

<body oncontextmenu="return false">

  <form id="wsapplicationmastermaint" runat="server">
      <div id="container">
          <div class="row">
               <div class="column left" > <h1>Application Master Maintenance</h1>
               </div>
               <div class="column right" > <img src="../images/logo_small.png" alt="Rexair LLC"  width="100%" height="140%" />
               </div>
         </div>

        <div id="frame">
	
      <table class="table_input" >
        <tr>
            <td style="vertical-align:top;">
                 <asp:GridView ID="MyGridView" runat="server" 
                    Width="850"
                    AllowPaging="True" 
                    AllowSorting="True" 
                    AutoGenerateColumns="false" 
                    BackColor="#CCCCCC" 
                    BorderColor="black" 
                    CellPadding="3" 
                    CellSpacing="0" 
                    EnableViewState="true" 
                    Font-Names="Verdana" 
                    Font-Size="8pt" 
                    HeaderStyle-BackColor="#aaaadd" 
                    DataKeyNames="application_id"
			        AutoGenerateEditButton = "true"
			        AutoGenerateDeleteButton = "true" 
			        OnRowCancelingEdit = "gv_RowCancelingEdit" 
			        OnRowDeleting="gv_RowDeleting" 
			        OnRowDeleted="gv_RowDeleted"
			        OnRowUpdating="gv_RowUpdating" 
			        OnRowUpdated="gv_RowUpdated"
			        OnRowEditing="gv_RowEditing" 
			        OnPageIndexChanged = "gv_PageIndexChanged" 
			        OnPageIndexChanging = "gv_PageIndexChanging" 
			        OnRowDataBound = "gv_RowDataBound"
			        OnRowCreated = "gv_RowCreated"
			        OnSorting="gv_Sorting" 
			        OnPreRender="gv_PreRender"
			        PageSize="200" 
                    ShowFooter="false">
                    
		    <FooterStyle forecolor="Black" backcolor="#CCCCCC"></FooterStyle>
            <HeaderStyle font-bold="True" forecolor="White" backcolor="#2876CD"></HeaderStyle>
            <PagerStyle horizontalalign="Right" forecolor="Black" backcolor="#999999" 
                         Font-Names="Verdana" Font-Size="Small"></PagerStyle>
            <PagerSettings Mode="NumericFirstLast" Position="Top"/>
			<EditRowStyle BackColor="#E6E8FA" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <EmptyDataTemplate>
               <asp:Label ID="Label1" runat="server">
                   There is no data to display in this view.
               </asp:Label>
            </EmptyDataTemplate>
            
                     <Columns>
                          <asp:BoundField DataField="application_id" HeaderText="Application ID" 
                            ItemStyle-Width="100px" ReadOnly="true" SortExpression="application_id" HeaderStyle-HorizontalAlign="Left">
                          <ItemStyle Width="100px"></ItemStyle>
                         </asp:BoundField>

			            <asp:TemplateField HeaderText="Application Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200px" SortExpression="application_name">
                            <ItemTemplate>
                               <%#Eval("application_name")%>
                            </ItemTemplate> 
                            <EditItemTemplate>
                                <asp:TextBox ID="applicationname" runat="server" Text='<%#Eval("application_name") %>'/>
                            </EditItemTemplate> 

                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                            <ItemStyle Width="200px"></ItemStyle>
                        </asp:TemplateField>     

 			            <asp:TemplateField HeaderText="Super User" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200px" SortExpression="super_user_id">
                            <ItemTemplate>
                               <%#Eval("super_user_id")%>
                            </ItemTemplate> 
                            <EditItemTemplate>
                                <asp:TextBox ID="superuserid" runat="server" Text='<%#Eval("super_user_id") %>'/>
                            </EditItemTemplate> 

                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                            <ItemStyle Width="200px"></ItemStyle>
                        </asp:TemplateField>     
                          
                   		<asp:TemplateField HeaderText="Notes" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="240px" SortExpression="app_notes">
                            <ItemTemplate>
                               <%#Eval("app_notes")%>
                            </ItemTemplate> 
                            <EditItemTemplate>
                               <asp:TextBox ID="appnotes" runat="server" Text='<%#Eval("app_notes") %>' TextMode="MultiLine"/>
                            </EditItemTemplate> 

                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                            <ItemStyle Width="240px"></ItemStyle>
                       </asp:TemplateField>

                       <asp:TemplateField HeaderText="Cadillac Loc" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px" SortExpression="loc_cadillac">
                            <ItemTemplate>
                                <asp:CheckBox ID="LocCadCheckBox" runat="server" 
                                    Checked='<%# DataBinder.Eval(Container.DataItem, "loc_cadillac") %>'  Enabled="False" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="edit_LocCadCheckBox" Runat="server" 
                                    Checked='<%# DataBinder.Eval(Container.DataItem, "loc_cadillac") %>' Enabled="True" />
                            </EditItemTemplate>

                            <ItemStyle Width="20px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Troy Loc" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px" SortExpression="loc_troy">
                            <ItemTemplate>
                                <asp:CheckBox ID="LocTroyCheckBox" runat="server" 
                                    Checked='<%# DataBinder.Eval(Container.DataItem, "loc_troy") %>'  Enabled="False" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="edit_LocTroyCheckBox" Runat="server" 
                                    Checked='<%# DataBinder.Eval(Container.DataItem, "loc_troy") %>' Enabled="True" />
                            </EditItemTemplate>

                            <ItemStyle Width="20px"></ItemStyle>
                        </asp:TemplateField>
           
                       <asp:TemplateField HeaderText="Inact" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px" SortExpression="inactive">
                            <ItemTemplate>
                                <asp:CheckBox ID="InactiveCheckBox" runat="server" 
                                    Checked='<%# DataBinder.Eval(Container.DataItem, "inactive") %>'  Enabled="False" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="edit_InactiveCheckBox" Runat="server" 
                                    Checked='<%# Databinder.Eval(Container.DataItem, "inactive") %>' Enabled="True" />
                            </EditItemTemplate>

                            <ItemStyle Width="20px"></ItemStyle>
                        </asp:TemplateField>
                         
                       <asp:TemplateField HeaderText="Lst Mod Dt" ItemStyle-Width="150px" Visible="false">
                          <ItemTemplate>
                              <asp:Label id="moddate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.modification_date") %>' > 
                              </asp:Label>
                         </ItemTemplate>

                         <ItemStyle Width="150px"></ItemStyle>
                       </asp:TemplateField>
                        
                     </Columns>
                </asp:GridView>
            </td>
            <td style="vertical-align:top;">
                <div class="frame">
                    <table class="font8veranda">
                        <tr>
                            <td colspan="2" class="font10veranda">
                                Add a New Application:</td>
                        </tr>
                        <tr>
                            <td class="aligntext">
                                <strong>Application ID: </strong>
                            </td>
                            <td>
                                <asp:TextBox ID="application_id" runat="server" AutoPostBack="false" MaxLength="50" 
                                    ReadOnly="false" CssClass="requiredfield" 
                                    TabIndex="1" ToolTip="Enter a unique Application ID." Width="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="aligntext">
                                <strong>Application Name: </strong>
                            </td>
                            <td>
                                <asp:TextBox ID="application_name" runat="server" AutoPostBack="false" MaxLength="100" 
                                    ReadOnly="false" 
                                    CssClass="requiredfield"  
                                    TabIndex="2" ToolTip="Enter the Application Name." Width="200"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
 			                 <td class="aligntext">
			                    <asp:HyperLink runat="server" Text="Super User ID:" NavigateURL="wsusermastermaint.aspx" Target="_blank" TabIndex="0"/>
	                           <input type="button" id="btnUserSearch" onclick='pickUser("getuser_id")' value="S" runat="server" tabindex="0" />
                              </td>
                              <td>
                                 <asp:TextBox ID="getuser_id" AutoPostBack="true" OnTextChanged="ReturnUser_Click" ReadOnly="false" runat="server" MaxLength="50" Width="150" CssClass="requiredfield" ToolTip="Enter a valid User ID." TabIndex="3"></asp:TextBox> 
                             </td>
                         </tr>
                        <tr>
                            <td>

                            </td>
                           <td>
                                 <asp:TextBox ID="getuser_name" AutoPostBack="false" ReadOnly="true" runat="server" MaxLength="100" Width="200" BackColor="#CCCCCC"></asp:TextBox> 
                            </td>
                            <td>
			                    <input type="button" id="btnGet" onServerClick="ReturnUser_Click" value="" runat="server" disabled="disabled" class="buttonvisbilityhidden" height="0px" tabindex="0"/>
	                        </td>
                        </tr>

                         <tr>
                            <td class="aligntext">
                                <strong>App Loc Cadillac: </strong>
                            </td>
                            <td>
                                <asp:CheckBox ID="loc_cadillac" runat="server" AutoPostBack="false" Enabled="true" 
                                    TabIndex="4" ToolTip="Check if the User is Physically located in Cadillac." />
                            </td>
                        </tr>
                        <tr>
                         <td class="aligntext">
                                <strong>App Loc Troy: </strong>
                            </td>
                            <td>
                                <asp:CheckBox ID="loc_troy" runat="server" AutoPostBack="false" Enabled="true" 
                                    TabIndex="5" ToolTip="Check if the User is Physically located in Troy." />
                            </td>
                        </tr>
                         <tr>
                            <td class="aligntext">
                                <strong>Application Notes: </strong>
                            </td>
                            <td>
                                <asp:TextBox ID="app_notes" runat="server" maxlength="255" TextMode="Multiline" Rows="5"
                                    CssClass="requiredfield" 
                                    tabindex="6" ToolTip="Enter the User Application Notes." width="200" />
                            </td>
                        </tr>
                        <tr>
                            <td class="aligntext">
                                <strong>Inactive: </strong>
                            </td>
                            <td>
                                <asp:CheckBox ID="inactive" runat="server" AutoPostBack="false" Enabled="true" 
                                    TabIndex="7" ToolTip="Enter the status of the Application." />
                            </td>
                        </tr>
  
                        <tr>
                            <td>
                            </td>
                            <td class="paddingtop15">
                                <asp:Button ID="btnAdd" runat="server" OnClick="AddApp_Click" TabIndex="10" Text="Add" ToolTip="Click to save the Application. ALT+D" AccessKey="D"/>
                                <asp:Button ID="btnClear" runat="server" OnClick="PageReset_Click" TabIndex="11" Text="Clear" ToolTip="Click to reset the window. ALT+C" AccessKey="C"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:Center;" colspan="2" class="paddingtop15">
                                <span id="AddMessage" runat="server" enableviewstate="false" class="font11arial"></span>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
 
    </table>
 
         <div class="buttonArea">
                 <asp:Button ID="btnPrint" Font-Size="Large" runat="server" OnClick="PrintData" TabIndex="12" Text="Print" ToolTip="Click to Print the Data. ALT+P" AccessKey="P"/>
                <asp:Button ID="btnReturn" Font-Size="Large" runat="server" OnClick="ReturnToMainMenu" TabIndex="13" Text="Return to Main Menu" ToolTip="Click to return to the Web Security Main Menu. ALT+R" AccessKey="R" />
         </div>

         <div>
             <span id="Message" EnableViewState="false"  runat="server"/>
         </div>

        <table style="width:100%;" class="textalignright">
           <tr>
            <td>
                <asp:TextBox ID="sort_id" runat="server" AutoPostBack="false" MaxLength="40" 
                    ReadOnly="true" 
                    Visible="false" Width="40"></asp:TextBox>
            </td>
           </tr>
         </table>


         </div>
           </div>

  </form>

</body>
</html>