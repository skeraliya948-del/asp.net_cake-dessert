using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;

namespace SweetDelights
{
    public partial class Admin_Dashboard : Page
    {
        SqlConnection con;
        SqlCommand cmd;
        string s = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            filltotalorder();
            filltotaluser();
            filltotalproduct();
            filltotalamount();

        }

        void getCon()
        {
            con = new SqlConnection(s);
            con.Open();
        }

        void filltotalorder()
        {
            getCon();
            string query = "SELECT COUNT(*) FROM Order_Tbl";
            cmd = new SqlCommand(query, con);
            int totalOrders = (int)cmd.ExecuteScalar();
            lblOrders.Text = totalOrders.ToString();

        }

        void filltotaluser()
        {
            getCon();
            string query = "SELECT COUNT(*) FROM emp_tbl";
            cmd = new SqlCommand(query, con);
            int totalUsers = (int)cmd.ExecuteScalar();
            lblUsers.Text = totalUsers.ToString();

        }

        void filltotalproduct()
        {
            getCon();
            string query = "SELECT COUNT(*) FROM Add_Products_Tbl";
            cmd = new SqlCommand(query, con);
            int totalProducts = (int)cmd.ExecuteScalar();
            lblProducts.Text = totalProducts.ToString();



        }
        void filltotalamount()
        {
            getCon();

            string query = "SELECT SUM(Quentity * Prod_Price) FROM Order_Tbl o JOIN Add_Products_Tbl p ON o.Order_Prod_Id = p.Prod_Id";
            cmd = new SqlCommand(query, con);
            int totalAmount = Convert.ToInt32(cmd.ExecuteScalar());
            lblRevenue.Text = totalAmount.ToString();
        }
    }
    }
}
