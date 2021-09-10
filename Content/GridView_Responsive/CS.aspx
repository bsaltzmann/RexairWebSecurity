<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CS.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width = device-width, initial-scale = 1.0, minimum-scale = 1.0, maximum-scale = 1.0, user-scalable = no" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:GridView ID="GridView1" CssClass="footable" runat="server" AutoGenerateColumns="false"
        Style="max-width: 500px">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Customer Id" />
            <asp:BoundField DataField="Name" HeaderText="Customer Name" />
            <asp:BoundField DataField="Country" HeaderText="Country" />
            <asp:BoundField DataField="Salary" HeaderText="Salary" />
        </Columns>
    </asp:GridView>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jquery-footable/0.1.0/css/footable.min.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-footable/0.1.0/js/footable.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=GridView1]').footable();
        });
    </script>
    </form>
</body>
</html>
