using System;
using System.Data;
using System.Data.SqlClient;

namespace SweetDelights
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uid"] != null)
            {
                Response.Redirect(DatabaseHelper.IsAdminEmail(Convert.ToString(Session["admin"]))
                    ? "Admin_Dashboard.aspx"
                    : "Default.aspx");
            }
        }

        protected void login_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtemail.Text) ||
                string.IsNullOrWhiteSpace(txtpassword.Text))
            {
                Response.Write("<script>alert('Please enter both email and password.')</script>");
                return;
            }

            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT Id, Name, Email, Mobile FROM emp_tbl " +
                "WHERE Email = @Email AND Password = @Password",
                con))
            {
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = txtemail.Text.Trim();
                cmd.Parameters.Add("@Password", SqlDbType.VarChar, 100).Value = txtpassword.Text;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Response.Write("<script>alert('Invalid email or password.')</script>");
                        return;
                    }

                    Session["uid"] = Convert.ToInt32(reader["Id"]);
                    Session["admin"] = Convert.ToString(reader["Email"]);
                    Session["User"] = new User
                    {
                        FullName = Convert.ToString(reader["Name"]),
                        Email = Convert.ToString(reader["Email"]),
                        Phone = Convert.ToString(reader["Mobile"]),
                        IsLoggedIn = true
                    };
                }
            }

            Response.Redirect(DatabaseHelper.IsAdminEmail(Convert.ToString(Session["admin"]))
                ? "Admin_Dashboard.aspx"
                : "Default.aspx");
        }
    }
}
