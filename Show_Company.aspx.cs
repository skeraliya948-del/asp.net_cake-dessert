using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetDelights
{
    public partial class Show_Company : Page
    {
        private void FillDataList()
        {
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT Comp_Id, Comp_Name, Comp_Image, Comp_Desc FROM Add_Company_Tbl",
                con))
            {
                DataTable companies = new DataTable();
                adapter.Fill(companies);
                DataList1.DataSource = companies;
                DataList1.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                Label1.Text = "Welcome, " +
                    HttpUtility.HtmlEncode(Convert.ToString(Session["admin"]));
                FillDataList();
            }
        }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "cmd_cid")
            {
                int companyId;
                if (!int.TryParse(Convert.ToString(e.CommandArgument), out companyId))
                {
                    throw new InvalidOperationException("The selected company has an invalid ID.");
                }

                Response.Redirect("Show_Products.aspx?cid=" + companyId);
            }
        }
    }
}
