using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;

namespace SweetDelights
{
    public partial class My_Orders : Page
    {
        private int UserId
        {
            get { return Convert.ToInt32(Session["uid"]); }
        }

        private void FillCustomer()
        {
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT Name, Email, Mobile, Address FROM emp_tbl WHERE Id = @User_Id",
                con))
            {
                cmd.Parameters.Add("@User_Id", SqlDbType.Int).Value = UserId;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("The signed-in customer record could not be found.");
                    }

                    txtcustname.Text = Convert.ToString(reader["Name"]);
                    txtemail.Text = Convert.ToString(reader["Email"]);
                    txtmobile.Text = Convert.ToString(reader["Mobile"]);
                    if (reader["Address"] != DBNull.Value)
                    {
                        txtaddress.Text = Convert.ToString(reader["Address"]);
                    }
                }
            }
        }

        private void FillOrderPreview()
        {
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT p.Prod_Model, c.Quentity, p.Prod_Price, " +
                "(c.Quentity * p.Prod_Price) AS Line_Total " +
                "FROM Cart_Tbl c " +
                "INNER JOIN Add_Products_Tbl p ON c.Cart_Prod_Id = p.Prod_Id " +
                "WHERE c.Cart_User_Id = @User_Id ORDER BY c.Cart_Id",
                con))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@User_Id", SqlDbType.Int).Value = UserId;
                DataTable cart = new DataTable();
                adapter.Fill(cart);
                GridView1.DataSource = cart;
                GridView1.DataBind();

                decimal subtotal = 0;
                foreach (DataRow row in cart.Rows)
                {
                    subtotal += Convert.ToDecimal(row["Line_Total"]);
                }

                decimal deliveryCharge = subtotal == 0 || subtotal >= 1000 ? 0 : 50;
                txtsubtotal.Text = subtotal.ToString("N2", CultureInfo.CurrentCulture);
                txtdelcharge.Text = deliveryCharge.ToString("N2", CultureInfo.CurrentCulture);
                txtgrandtotal.Text = (subtotal + deliveryCharge).ToString("N2", CultureInfo.CurrentCulture);
                btnPlaceOrder.Enabled = cart.Rows.Count > 0;
            }
        }

        private void ShowOrder(int orderNo)
        {
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT Order_No, Order_Date, Ship_Address, Pay_Method, Pay_Status, " +
                "Order_Status, Subtotal, Delivery_Charge, Grand_Total " +
                "FROM Order_Tbl WHERE Order_No = @Order_No AND Order_User_Id = @User_Id",
                con))
            {
                cmd.Parameters.Add("@Order_No", SqlDbType.Int).Value = orderNo;
                cmd.Parameters.Add("@User_Id", SqlDbType.Int).Value = UserId;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        lblMessage.Text = "That order could not be found.";
                        pnlCheckout.Visible = false;
                        pnlConfirmation.Visible = true;
                        return;
                    }

                    lblOrderNo.Text = Server.HtmlEncode(Convert.ToString(reader["Order_No"]));
                    lblOrderDate.Text = Server.HtmlEncode(
                        Convert.ToDateTime(reader["Order_Date"]).ToString("dd MMM yyyy, hh:mm tt"));
                    lblShipAddress.Text = Server.HtmlEncode(Convert.ToString(reader["Ship_Address"]));
                    lblPayMethod.Text = Server.HtmlEncode(Convert.ToString(reader["Pay_Method"]));
                    lblPayStatus.Text = Server.HtmlEncode(Convert.ToString(reader["Pay_Status"]));
                    lblOrderStatus.Text = Server.HtmlEncode(Convert.ToString(reader["Order_Status"]));
                    lblSubtotal.Text = Convert.ToDecimal(reader["Subtotal"]).ToString("N2", CultureInfo.CurrentCulture);
                    lblDelivery.Text = Convert.ToDecimal(reader["Delivery_Charge"]).ToString("N2", CultureInfo.CurrentCulture);
                    lblGrandTotal.Text = Convert.ToDecimal(reader["Grand_Total"]).ToString("N2", CultureInfo.CurrentCulture);
                }
            }

            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT Product_Name, Quentity, Unit_Price, " +
                "(Quentity * Unit_Price) AS Line_Total " +
                "FROM Order_Details_Tbl WHERE Order_No = @Order_No " +
                "ORDER BY Order_Detail_Id",
                con))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@Order_No", SqlDbType.Int).Value = orderNo;
                DataTable details = new DataTable();
                adapter.Fill(details);
                GridViewConfirmation.DataSource = details;
                GridViewConfirmation.DataBind();
            }

            pnlCheckout.Visible = false;
            pnlConfirmation.Visible = true;
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
                int orderNo;
                if (int.TryParse(Request.QueryString["orderno"], out orderNo))
                {
                    ShowOrder(orderNo);
                    return;
                }

                FillCustomer();
                FillOrderPreview();
            }
        }

        protected void rblpay_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillOrderPreview();
        }

        protected void btn_place_Click(object sender, EventArgs e)
        {
            string shippingAddress = txtaddress.Text.Trim();
            if (string.IsNullOrWhiteSpace(shippingAddress))
            {
                lblMessage.Text = "Please enter a shipping address.";
                return;
            }

            if (shippingAddress.Length > 500)
            {
                lblMessage.Text = "The shipping address must be 500 characters or fewer.";
                return;
            }

            string paymentMethod = rblpay.SelectedValue;
            if (paymentMethod != "Cash on Delivery" && paymentMethod != "UPI")
            {
                lblMessage.Text = "Please select a valid payment method.";
                return;
            }

            int orderNo;
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    DataTable cart = new DataTable();
                    using (SqlCommand cartCommand = new SqlCommand(
                        "SELECT c.Cart_Prod_Id, c.Quentity, p.Prod_Model, p.Prod_Price " +
                        "FROM Cart_Tbl c WITH (UPDLOCK, HOLDLOCK) " +
                        "INNER JOIN Add_Products_Tbl p ON c.Cart_Prod_Id = p.Prod_Id " +
                        "WHERE c.Cart_User_Id = @User_Id ORDER BY c.Cart_Id",
                        con, transaction))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cartCommand))
                    {
                        cartCommand.Parameters.Add("@User_Id", SqlDbType.Int).Value = UserId;
                        adapter.Fill(cart);
                    }

                    if (cart.Rows.Count == 0)
                    {
                        transaction.Rollback();
                        lblMessage.Text = "Your cart is empty.";
                        GridView1.DataSource = cart;
                        GridView1.DataBind();
                        txtsubtotal.Text = "0.00";
                        txtdelcharge.Text = "0.00";
                        txtgrandtotal.Text = "0.00";
                        btnPlaceOrder.Enabled = false;
                        return;
                    }

                    decimal subtotal = 0;
                    foreach (DataRow row in cart.Rows)
                    {
                        subtotal += Convert.ToDecimal(row["Prod_Price"]) *
                                    Convert.ToInt32(row["Quentity"]);
                    }

                    decimal deliveryCharge = subtotal == 0 || subtotal >= 1000 ? 0 : 50;
                    using (SqlCommand orderCommand = new SqlCommand(
                        "INSERT INTO Order_Tbl " +
                        "(Order_User_Id, Ship_Address, Pay_Method, Pay_Status, Order_Status, " +
                        "Subtotal, Delivery_Charge, Grand_Total) " +
                        "OUTPUT INSERTED.Order_No " +
                        "VALUES (@User_Id, @Ship_Address, @Pay_Method, 'Pending', 'Pending', " +
                        "@Subtotal, @Delivery_Charge, @Grand_Total)",
                        con, transaction))
                    {
                        orderCommand.Parameters.Add("@User_Id", SqlDbType.Int).Value = UserId;
                        orderCommand.Parameters.Add("@Ship_Address", SqlDbType.VarChar, 500).Value = shippingAddress;
                        orderCommand.Parameters.Add("@Pay_Method", SqlDbType.VarChar, 50).Value = paymentMethod;

                        SqlParameter subtotalParameter = orderCommand.Parameters.Add("@Subtotal", SqlDbType.Decimal);
                        subtotalParameter.Precision = 18;
                        subtotalParameter.Scale = 2;
                        subtotalParameter.Value = subtotal;

                        SqlParameter deliveryParameter = orderCommand.Parameters.Add("@Delivery_Charge", SqlDbType.Decimal);
                        deliveryParameter.Precision = 18;
                        deliveryParameter.Scale = 2;
                        deliveryParameter.Value = deliveryCharge;

                        SqlParameter totalParameter = orderCommand.Parameters.Add("@Grand_Total", SqlDbType.Decimal);
                        totalParameter.Precision = 18;
                        totalParameter.Scale = 2;
                        totalParameter.Value = subtotal + deliveryCharge;

                        orderNo = Convert.ToInt32(orderCommand.ExecuteScalar());
                    }

                    foreach (DataRow row in cart.Rows)
                    {
                        using (SqlCommand detailCommand = new SqlCommand(
                            "INSERT INTO Order_Details_Tbl " +
                            "(Order_No, Order_Prod_Id, Product_Name, Quentity, Unit_Price) " +
                            "VALUES (@Order_No, @Product_Id, @Product_Name, @Quantity, @Unit_Price)",
                            con, transaction))
                        {
                            detailCommand.Parameters.Add("@Order_No", SqlDbType.Int).Value = orderNo;
                            detailCommand.Parameters.Add("@Product_Id", SqlDbType.Int).Value =
                                Convert.ToInt32(row["Cart_Prod_Id"]);
                            detailCommand.Parameters.Add("@Product_Name", SqlDbType.VarChar, 150).Value =
                                Convert.ToString(row["Prod_Model"]);
                            detailCommand.Parameters.Add("@Quantity", SqlDbType.Int).Value =
                                Convert.ToInt32(row["Quentity"]);

                            SqlParameter unitPrice = detailCommand.Parameters.Add("@Unit_Price", SqlDbType.Decimal);
                            unitPrice.Precision = 18;
                            unitPrice.Scale = 2;
                            unitPrice.Value = Convert.ToDecimal(row["Prod_Price"]);

                            detailCommand.ExecuteNonQuery();
                        }
                    }

                    using (SqlCommand clearCart = new SqlCommand(
                        "DELETE FROM Cart_Tbl WHERE Cart_User_Id = @User_Id",
                        con, transaction))
                    {
                        clearCart.Parameters.Add("@User_Id", SqlDbType.Int).Value = UserId;
                        clearCart.ExecuteNonQuery();
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

            Response.Redirect("My_Orders.aspx?orderno=" + orderNo, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
