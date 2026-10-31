using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace SweetDelights
{
    public partial class ViewDetails : Page
    {
        private void FillDataList(int productId)
        {
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT * FROM Add_Products_Tbl WHERE Prod_Id = @Product_Id",
                con))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@Product_Id", SqlDbType.Int).Value = productId;

                DataTable product = new DataTable();
                adapter.Fill(product);

                if (product.Rows.Count == 0)
                {
                    lblEmpty.Visible = true;
                    lblEmpty.Text = "The requested product could not be found.";
                    return;
                }

                DataList1.DataSource = product;
                DataList1.DataBind();
                lblEmpty.Visible = false;
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
                int productId;
                if (!int.TryParse(Request.QueryString["pid"], out productId) || productId <= 0)
                {
                    lblEmpty.Visible = true;
                    lblEmpty.Text = "A valid product ID is required.";
                    return;
                }

                FillDataList(productId);
            }
        }
    }
}
