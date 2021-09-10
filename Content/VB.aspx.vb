Imports System.Data

Partial Class VB
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Dim dt As New DataTable()
            dt.Columns.AddRange(New DataColumn(3) {New DataColumn("Id"), New DataColumn("Name"), New DataColumn("Country"), New DataColumn("Salary")})
            dt.Rows.Add(1, "John Hammond", "United States", 70000)
            dt.Rows.Add(2, "Mudassar Khan", "India", 40000)
            dt.Rows.Add(3, "Suzanne Mathews", "France", 30000)
            dt.Rows.Add(4, "Robert Schidner", "Russia", 50000)
            GridView1.DataSource = dt
            GridView1.DataBind()

            'Attribute to show the Plus Minus Button.
            GridView1.HeaderRow.Cells(0).Attributes("data-class") = "expand"

            'Attribute to hide column in Phone.
            GridView1.HeaderRow.Cells(2).Attributes("data-hide") = "phone"
            GridView1.HeaderRow.Cells(3).Attributes("data-hide") = "phone"

            'Adds THEAD and TBODY to GridView.
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub

End Class
