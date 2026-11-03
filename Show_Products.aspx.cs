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
        SqlConnection con;
        SqlDataAdapter da;
        DataSet ds;

        string s = ConfigurationManager
                    .ConnectionStrings["dbcon"]
                    .ConnectionString;

        void getCon()
        {
            con = new SqlConnection(s);
            con.Open();
        }


        void fillAllProducts()
        {
            getCon();

            da = new SqlDataAdapter(
                "SELECT * FROM Add_Products_Tbl",
                con
            );

            ds = new DataSet();

            da.Fill(ds);

            DataList1.DataSource = ds;
            DataList1.DataBind();

            if (ds.Tables[0].Rows.Count == 0)
            {
                lblEmpty.Visible = true;
                lblEmpty.Text = "No products found.";
            }
            else
            {
                lblEmpty.Visible = false;
            }


        }


        void fillCompanyProducts(int cid)
        {
            getCon();

            da = new SqlDataAdapter(
                "SELECT * FROM Add_Products_Tbl WHERE Prod_Comp_Id = " + cid,
                con
            );

            ds = new DataSet();

            da.Fill(ds);

            DataList1.DataSource = ds;
            DataList1.DataBind();

            if (ds.Tables[0].Rows.Count == 0)
            {
                lblEmpty.Visible = true;
                lblEmpty.Text = "No products found for this company.";
            }
            else
            {
                lblEmpty.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                Label3.Text =
                    "Welcome, " + Session["admin"].ToString();

                string cid = Request.QueryString["cid"];

                if (string.IsNullOrEmpty(cid))
                {

                    fillAllProducts();
                }
                else
                {

                    int companyId = Convert.ToInt32(cid);

                    fillCompanyProducts(companyId);
                }

            }

        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "cmd_view")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                Response.Redirect(
                    "ViewDetails.aspx?pid=" + id
                );
            }

            if (e.CommandName == "cmd_cart")
            {
                int productId = Convert.ToInt32(e.CommandArgument);

                if (Session["uid"] == null)
                {
                    Response.Redirect("login.aspx");
                    return;
                }

                int userId = Convert.ToInt32(Session["uid"]);

                getCon();


                SqlCommand checkCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Cart_Tbl WHERE Cart_User_Id = "
                    + userId +
                    " AND Cart_Prod_Id = "
                    + productId,
                    con
                );

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {

                    SqlCommand updateCmd = new SqlCommand(
                        "UPDATE Cart_Tbl SET Quentity = Quentity + 1 " +
                        "WHERE Cart_User_Id = " + userId +
                        " AND Cart_Prod_Id = " + productId,
                        con
                    );

                    updateCmd.ExecuteNonQuery();
                }
                else
                {

                    SqlCommand insertCmd = new SqlCommand(
                        "INSERT INTO Cart_Tbl " +
                        "(Cart_User_Id, Cart_Prod_Id, Quentity, AddedDate) " +
                        "VALUES (" +
                        userId + ", " +
                        productId + ", " +
                        "1, GETDATE())",
                        con
                    );

                    insertCmd.ExecuteNonQuery();
                }




                Response.Redirect("Add_To_Cart.aspx");
            }
        }
    }
}
