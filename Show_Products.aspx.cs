using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetDelights
{
    public partial class Show_Products : Page
    {
        private void FillProducts(int? companyId)
        {
            string query = "SELECT * FROM Add_Products_Tbl";
            if (companyId.HasValue)
            {
                query += " WHERE Prod_Comp_Id = @Company_Id";
            }

            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                if (companyId.HasValue)
                {
                    cmd.Parameters.Add("@Company_Id", SqlDbType.Int).Value = companyId.Value;
                }

                DataTable products = new DataTable();
                adapter.Fill(products);
                DataList1.DataSource = products;
                DataList1.DataBind();

                if (products.Rows.Count == 0)
                {
                    lblEmpty.Visible = true;
                    lblEmpty.Text = companyId.HasValue
                        ? "No products found for this company."
                        : "No products found.";
                }
                else
                {
                    lblEmpty.Visible = false;
                }
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
                Label3.Text = "Welcome, " +
                    HttpUtility.HtmlEncode(Convert.ToString(Session["admin"]));

                string companyIdValue = Request.QueryString["cid"];
                if (string.IsNullOrWhiteSpace(companyIdValue))
                {
                    FillProducts(null);
                    return;
                }

                int companyId;
                if (!int.TryParse(companyIdValue, out companyId) || companyId <= 0)
                {
                    lblEmpty.Visible = true;
                    lblEmpty.Text = "The selected company is invalid.";
                    return;
                }

                FillProducts(companyId);
            }
        }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            int recordId = 0;
            if ((e.CommandName == "cmd_view" || e.CommandName == "cmd_cart") &&
                (!int.TryParse(Convert.ToString(e.CommandArgument), out recordId) || recordId <= 0))
            {
                throw new InvalidOperationException("The selected product has an invalid ID.");
            }

            if (e.CommandName == "cmd_view")
            {
                Response.Redirect("ViewDetails.aspx?pid=" + recordId);
                return;
            }

            if (e.CommandName != "cmd_cart")
            {
                return;
            }

            int userId;
            if (!int.TryParse(Convert.ToString(Session["uid"]), out userId) || userId <= 0)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    using (SqlCommand productCommand = new SqlCommand(
                        "SELECT Prod_Id FROM Add_Products_Tbl WITH (UPDLOCK, HOLDLOCK) WHERE Prod_Id = @Product_Id",
                        con, transaction))
                    {
                        productCommand.Parameters.Add("@Product_Id", SqlDbType.Int).Value = recordId;
                        if (productCommand.ExecuteScalar() == null)
                        {
                            transaction.Rollback();
                            lblEmpty.Visible = true;
                            lblEmpty.Text = "This product is no longer available.";
                            return;
                        }
                    }

                    using (SqlCommand cartCommand = new SqlCommand(
                        "UPDATE Cart_Tbl WITH (UPDLOCK, HOLDLOCK) " +
                        "SET Quentity = Quentity + 1 " +
                        "WHERE Cart_User_Id = @User_Id AND Cart_Prod_Id = @Product_Id; " +
                        "IF @@ROWCOUNT = 0 " +
                        "INSERT INTO Cart_Tbl (Cart_User_Id, Cart_Prod_Id, Quentity, AddedDate) " +
                        "VALUES (@User_Id, @Product_Id, 1, GETDATE())",
                        con, transaction))
                    {
                        cartCommand.Parameters.Add("@User_Id", SqlDbType.Int).Value = userId;
                        cartCommand.Parameters.Add("@Product_Id", SqlDbType.Int).Value = recordId;
                        cartCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    if (transaction.Connection != null)
                    {
                        transaction.Rollback();
                    }

                    throw;
                }
            }

            Response.Redirect("Add_To_Cart.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
