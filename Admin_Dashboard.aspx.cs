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

        private void LoadSummary()
        {
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT " +
                "(SELECT COALESCE(SUM(Grand_Total), 0) FROM Order_Tbl WHERE Pay_Status = 'Paid') AS Revenue, " +
                "(SELECT COUNT(*) FROM Order_Tbl) AS Orders, " +
                "(SELECT COUNT(*) FROM emp_tbl WHERE Email <> @Admin_Email) AS Users, " +
                "(SELECT COUNT(*) FROM Add_Products_Tbl) AS Products, " +
                "(SELECT COALESCE(SUM(d.Quentity), 0) FROM Order_Details_Tbl d " +
                "INNER JOIN Order_Tbl o ON o.Order_No = d.Order_No WHERE o.Order_Status <> 'Cancelled') AS Items_Sold, " +
                "(SELECT COUNT(*) FROM Order_Tbl WHERE Order_Status = 'Pending') AS Pending",
                con))
            {
                cmd.Parameters.Add("@Admin_Email", SqlDbType.VarChar, 100).Value =
                    DatabaseHelper.AdminEmail;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblRevenue.Text = Convert.ToDecimal(reader["Revenue"]).ToString("C2", CultureInfo.GetCultureInfo("en-IN"));
                        lblOrders.Text = Convert.ToInt32(reader["Orders"]).ToString("N0");
                        lblUsers.Text = Convert.ToInt32(reader["Users"]).ToString("N0");
                        lblProducts.Text = Convert.ToInt32(reader["Products"]).ToString("N0");
                        lblItemsSold.Text = Convert.ToInt32(reader["Items_Sold"]).ToString("N0");
                        lblPending.Text = Convert.ToInt32(reader["Pending"]).ToString("N0");
                    }
                }
            }
        }

        private void LoadRevenueChart()
        {
            DateTime currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime startMonth = currentMonth.AddMonths(-5);
            DateTime endMonth = currentMonth.AddMonths(1);
            Dictionary<DateTime, decimal> monthlyRevenue = new Dictionary<DateTime, decimal>();

            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT YEAR(Order_Date) AS Order_Year, MONTH(Order_Date) AS Order_Month, " +
                "SUM(Grand_Total) AS Revenue FROM Order_Tbl " +
                "WHERE Pay_Status = 'Paid' AND Order_Date >= @Start_Date AND Order_Date < @End_Date " +
                "GROUP BY YEAR(Order_Date), MONTH(Order_Date)",
                con))
            {
                cmd.Parameters.Add("@Start_Date", SqlDbType.DateTime).Value = startMonth;
                cmd.Parameters.Add("@End_Date", SqlDbType.DateTime).Value = endMonth;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime month = new DateTime(
                            Convert.ToInt32(reader["Order_Year"]),
                            Convert.ToInt32(reader["Order_Month"]),
                            1);
                        monthlyRevenue[month] = Convert.ToDecimal(reader["Revenue"]);
                    }
                }
            }

            DataTable chart = new DataTable();
            chart.Columns.Add("Month", typeof(string));
            chart.Columns.Add("Revenue", typeof(decimal));
            chart.Columns.Add("Percentage", typeof(decimal));

            decimal maximum = 0;
            foreach (decimal value in monthlyRevenue.Values)
            {
                maximum = Math.Max(maximum, value);
            }

            for (int i = 0; i < 6; i++)
            {
                DateTime month = startMonth.AddMonths(i);
                decimal revenue;
                if (!monthlyRevenue.TryGetValue(month, out revenue))
                {
                    revenue = 0;
                }

                decimal percentage = maximum == 0 ? 0 : decimal.Round(revenue * 100 / maximum, 2);
                chart.Rows.Add(month.ToString("MMM yy", CultureInfo.CurrentCulture), revenue, percentage);
            }

            rptRevenue.DataSource = chart;
            rptRevenue.DataBind();
        }

        private void LoadOrderStatuses()
        {
            Dictionary<string, int> counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT Order_Status, COUNT(*) AS Status_Count FROM Order_Tbl GROUP BY Order_Status",
                con))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    counts[Convert.ToString(reader["Order_Status"])] =
                        Convert.ToInt32(reader["Status_Count"]);
                }
            }

            string[] statuses = { "Pending", "Processing", "Completed", "Cancelled" };
            int total = 0;
            foreach (string status in statuses)
            {
                int count;
                if (counts.TryGetValue(status, out count))
                {
                    total += count;
                }
            }

            DataTable statusTable = new DataTable();
            statusTable.Columns.Add("Status", typeof(string));
            statusTable.Columns.Add("Count", typeof(int));
            statusTable.Columns.Add("Percentage", typeof(decimal));

            foreach (string status in statuses)
            {
                int count;
                if (!counts.TryGetValue(status, out count))
                {
                    count = 0;
                }

                decimal percentage = total == 0 ? 0 : decimal.Round(count * 100m / total, 2);
                statusTable.Rows.Add(status, count, percentage);
            }

            rptStatus.DataSource = statusTable;
            rptStatus.DataBind();
        }

        private void LoadRecentOrders()
        {
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT TOP 8 o.Order_No, u.Name, o.Grand_Total AS Amount, " +
                "o.Pay_Status, o.Order_Status FROM Order_Tbl o " +
                "INNER JOIN emp_tbl u ON u.Id = o.Order_User_Id " +
                "ORDER BY o.Order_Date DESC, o.Order_No DESC",
                con))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable orders = new DataTable();
                adapter.Fill(orders);
                GridView1.DataSource = orders;
                GridView1.DataBind();
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
                lblDate.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy", CultureInfo.CurrentCulture);
                lblAdminName.Text = "Welcome back, " + DatabaseHelper.AdminEmail;
                LoadSummary();
                LoadRevenueChart();
                LoadOrderStatuses();
                LoadRecentOrders();
            }
        }
    }
}
