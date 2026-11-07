using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetDelights
{
    public partial class Admin_Orders : Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;

        string s = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        void getCon()
        {
            con = new SqlConnection(s);
            con.Open();
        }

        void fillGrid()
        {
            getCon();

            string query =
                "SELECT o.Order_Id, o.Order_No, " +
                "ISNULL(e.Name, 'Unknown') AS Name, " +
                "ISNULL(p.Prod_Name, 'Unknown') AS Prod_Name, " +
                "o.Quentity, " +
                "(o.Quentity * ISNULL(p.Prod_Price, 0)) AS Amount, " +
                "o.Order_Date, o.Ship_Address, " +
                "o.Pay_Method, o.Pay_Status, " +
                "ISNULL(o.Order_Status, 'Pending') AS Order_Status " +
                "FROM Order_Tbl o " +
                "LEFT JOIN emp_tbl e " +
                "ON o.Order_User_Id = e.Id " +
                "LEFT JOIN Add_Products_Tbl p " +
                "ON o.Order_Prod_Id = p.Prod_Id ";

            int orderNo;

            if (int.TryParse(Request.QueryString["orderno"], out orderNo)
                && orderNo > 0)
            {
                query += "WHERE o.Order_No = " + orderNo + " ";
            }

            query += "ORDER BY o.Order_Date DESC, o.Order_Id DESC";

            da = new SqlDataAdapter(query, con);

            ds = new DataSet();
            da.Fill(ds);

            GridView1.DataSource = ds;
            GridView1.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblMessage.Text = "Order Details Loaded Successfully";
            }
            else
            {
                lblMessage.Text = "No Orders Found";
            }

            con.Close();
        

     
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                fillGrid();
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            fillGrid();
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            DropDownList ddlPay =
                (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlPayStatus");

            DropDownList ddlOrder =
                (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlOrderStatus");

            string payStatus = ddlPay.SelectedValue;
            string orderStatus = ddlOrder.SelectedValue;

            getCon();

            string query =
                "UPDATE Order_Tbl SET " +
                "Pay_Status = '" + payStatus + "', " +
                "Order_Status = '" + orderStatus + "' " +
                "WHERE Order_Id = " + id;

            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();

            con.Close();

            GridView1.EditIndex = -1;
            fillGrid();

            lblMessage.Text = "Payment and Order Status Updated Successfully";
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            fillGrid();
        }
    }

}
