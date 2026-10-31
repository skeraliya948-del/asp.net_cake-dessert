using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetDelights
{
    public partial class Admin_Orders : Page
    {
        private int? SelectedOrderNo
        {
            get
            {
                int orderNo;
                return int.TryParse(Request.QueryString["orderno"], out orderNo) && orderNo > 0
                    ? (int?)orderNo
                    : null;
            }
        }

        private bool RequireAdmin()
        {
            if (DatabaseHelper.IsAdminEmail(Convert.ToString(Session["admin"])))
            {
                return true;
            }

            Response.Redirect("Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return false;
        }

        private void BindOrders()
        {
            string query =
                "SELECT o.Order_No, o.Order_Date, u.Name, u.Email, o.Grand_Total, " +
                "o.Pay_Status, o.Order_Status " +
                "FROM Order_Tbl o INNER JOIN emp_tbl u ON u.Id = o.Order_User_Id ";
            if (SelectedOrderNo.HasValue)
            {
                query += "WHERE o.Order_No = @Order_No ";
            }
            query += "ORDER BY o.Order_Date DESC, o.Order_No DESC";

            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                if (SelectedOrderNo.HasValue)
                {
                    cmd.Parameters.Add("@Order_No", SqlDbType.Int).Value = SelectedOrderNo.Value;
                }

                DataTable orders = new DataTable();
                adapter.Fill(orders);
                GridViewOrders.DataSource = orders;
                GridViewOrders.DataBind();
                lblOrdersMessage.Text = orders.Rows.Count == 0
                    ? "No matching orders were found."
                    : string.Empty;
            }

            if (SelectedOrderNo.HasValue)
            {
                BindOrderDetails(SelectedOrderNo.Value);
            }
            else
            {
                pnlOrderDetails.Visible = false;
            }
        }

        private void BindOrderDetails(int orderNo)
        {
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT Product_Name, Quentity, Unit_Price, " +
                "(Quentity * Unit_Price) AS Line_Total " +
                "FROM Order_Details_Tbl WHERE Order_No = @Order_No ORDER BY Order_Detail_Id",
                con))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@Order_No", SqlDbType.Int).Value = orderNo;
                DataTable details = new DataTable();
                adapter.Fill(details);
                GridViewDetails.DataSource = details;
                GridViewDetails.DataBind();
                pnlOrderDetails.Visible = details.Rows.Count > 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!RequireAdmin())
            {
                return;
            }

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["orderno"]) && !SelectedOrderNo.HasValue)
                {
                    lblOrdersMessage.Text = "The order number is invalid.";
                    return;
                }

                BindOrders();
            }
        }

        protected void GridViewOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            DropDownList orderStatus = (DropDownList)e.Row.FindControl("ddlOrderStatus");
            DropDownList paymentStatus = (DropDownList)e.Row.FindControl("ddlPayStatus");
            string currentOrderStatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Order_Status"));
            string currentPaymentStatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Pay_Status"));

            if (orderStatus.Items.FindByValue(currentOrderStatus) != null)
            {
                orderStatus.SelectedValue = currentOrderStatus;
            }

            if (paymentStatus.Items.FindByValue(currentPaymentStatus) != null)
            {
                paymentStatus.SelectedValue = currentPaymentStatus;
            }
        }

        protected void GridViewOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "save_status")
            {
                return;
            }

            if (!RequireAdmin())
            {
                return;
            }
            int orderNo;
            GridViewRow row = ((Control)e.CommandSource).NamingContainer as GridViewRow;
            if (row == null ||
                !int.TryParse(Convert.ToString(e.CommandArgument), out orderNo) ||
                orderNo <= 0)
            {
                throw new InvalidOperationException("The selected order has an invalid ID.");
            }

            DropDownList orderStatus = (DropDownList)row.FindControl("ddlOrderStatus");
            DropDownList paymentStatus = (DropDownList)row.FindControl("ddlPayStatus");
            string orderStatusValue = orderStatus.SelectedValue;
            string paymentStatusValue = paymentStatus.SelectedValue;

            if ((orderStatusValue != "Pending" &&
                 orderStatusValue != "Processing" &&
                 orderStatusValue != "Completed" &&
                 orderStatusValue != "Cancelled") ||
                (paymentStatusValue != "Pending" &&
                 paymentStatusValue != "Paid" &&
                 paymentStatusValue != "Refunded"))
            {
                throw new InvalidOperationException("Select valid order and payment statuses.");
            }

            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Order_Tbl SET Order_Status = @Order_Status, Pay_Status = @Pay_Status " +
                "WHERE Order_No = @Order_No",
                con))
            {
                cmd.Parameters.Add("@Order_Status", SqlDbType.VarChar, 30).Value = orderStatusValue;
                cmd.Parameters.Add("@Pay_Status", SqlDbType.VarChar, 30).Value = paymentStatusValue;
                cmd.Parameters.Add("@Order_No", SqlDbType.Int).Value = orderNo;
                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException("The selected order could not be updated.");
                }
            }

            BindOrders();
            lblOrdersMessage.Text = "Order #" + orderNo + " was updated.";
        }
    }
}
