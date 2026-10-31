using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetDelights
{
    public partial class Add_To_Cart : System.Web.UI.Page
    {
        private int UserId
        {
            get { return Convert.ToInt32(Session["uid"]); }
        }

        private void FillGrid()
        {
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT Cart_Tbl.Cart_Id, Add_Products_Tbl.Prod_Img, " +
                "Add_Products_Tbl.Prod_Model, Add_Products_Tbl.Prod_Price, Cart_Tbl.Quentity " +
                "FROM Cart_Tbl " +
                "INNER JOIN Add_Products_Tbl ON Cart_Tbl.Cart_Prod_Id = Add_Products_Tbl.Prod_Id " +
                "WHERE Cart_Tbl.Cart_User_Id = @User_Id",
                con))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@User_Id", SqlDbType.Int).Value = UserId;

                DataTable cartItems = new DataTable();
                adapter.Fill(cartItems);
                GridView1.DataSource = cartItems;
                GridView1.DataBind();

                decimal total = 0;
                foreach (DataRow row in cartItems.Rows)
                {
                    total += Convert.ToDecimal(row["Prod_Price"]) *
                             Convert.ToInt32(row["Quentity"]);
                }

                grand_total.Text = "Grand Total: ₹" + total.ToString("N2");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                Label5.Text = "Welcome, " +
                    (Session["admin"] == null ? "Customer" : Session["admin"].ToString());
                FillGrid();
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmd_remove")
            {
                int cartId;
                if (!int.TryParse(Convert.ToString(e.CommandArgument), out cartId))
                {
                    throw new InvalidOperationException("The selected cart item has an invalid ID.");
                }

                using (SqlConnection con = DatabaseHelper.GetOpenConnection())
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Cart_Tbl WHERE Cart_Id = @Cart_Id AND Cart_User_Id = @User_Id",
                    con))
                {
                    cmd.Parameters.Add("@Cart_Id", SqlDbType.Int).Value = cartId;
                    cmd.Parameters.Add("@User_Id", SqlDbType.Int).Value = UserId;
                    cmd.ExecuteNonQuery();
                }

                FillGrid();
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            FillGrid();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int cartId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            DropDownList quantityList = (DropDownList)GridView1.Rows[e.RowIndex]
                .FindControl("DropDownList1");
            int quantity;

            if (quantityList == null ||
                !int.TryParse(quantityList.SelectedValue, out quantity) ||
                quantity < 1)
            {
                throw new InvalidOperationException("Select a valid cart quantity.");
            }

            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Cart_Tbl SET Quentity = @Quantity " +
                "WHERE Cart_Id = @Cart_Id AND Cart_User_Id = @User_Id",
                con))
            {
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity;
                cmd.Parameters.Add("@Cart_Id", SqlDbType.Int).Value = cartId;
                cmd.Parameters.Add("@User_Id", SqlDbType.Int).Value = UserId;
                cmd.ExecuteNonQuery();
            }

            GridView1.EditIndex = -1;
            FillGrid();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            FillGrid();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
                e.Row.RowIndex == GridView1.EditIndex)
            {
                DropDownList quantityList = (DropDownList)e.Row.FindControl("DropDownList1");
                int currentQuantity = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Quentity"));
                if (currentQuantity > 10)
                {
                    quantityList.Items.Add(new ListItem(currentQuantity.ToString(), currentQuantity.ToString()));
                }

                quantityList.SelectedValue = currentQuantity.ToString();
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            bool hasItems;
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Cart_Tbl WHERE Cart_User_Id = @User_Id",
                con))
            {
                cmd.Parameters.Add("@User_Id", SqlDbType.Int).Value = UserId;
                hasItems = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }

            if (!hasItems)
            {
                Label5.Text = "Your cart is empty.";
                return;
            }

            Response.Redirect("My_Orders.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
