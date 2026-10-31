using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetDelights
{
    public partial class insert : System.Web.UI.Page
    {
        private void FillGrid()
        {
            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(
                "SELECT Id, Name, Gender, Email, City, Mobile, Image FROM emp_tbl",
                con))
            {
                DataTable users = new DataTable();
                da.Fill(users);
                GridView1.DataSource = users;
                GridView1.DataBind();
            }
        }

        private void FileUpload()
        {
            if (imgupload.HasFile)
            {
                string folderPath = Server.MapPath("images/");
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }

                fnm = "images/" + System.IO.Path.GetFileName(imgupload.FileName);
                imgupload.SaveAs(Server.MapPath(fnm));
            }
            else
            {
                fnm = "images/default-user.jpg";
            }
        }

        private string fnm;

        private void Clear()
        {
            txtusername.Text = string.Empty;
            txtaddress.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtpassword.Text = string.Empty;
            txtmbl.Text = string.Empty;
            rdogen.ClearSelection();
            drpcity.ClearSelection();
        }

        private bool RequireAdminSession()
        {
            if (DatabaseHelper.IsAdminEmail(Convert.ToString(Session["admin"])))
            {
                return true;
            }

            Response.Redirect("Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool isSignedIn = Session["uid"] != null;
            bool isAdminSession = DatabaseHelper.IsAdminEmail(Convert.ToString(Session["admin"]));
            pnlLoginLink.Visible = !isSignedIn;
            pnlAdminGrid.Visible = true;
            pnlUserRecords.Visible = isAdminSession;

            if (!IsPostBack)
            {
                if (!isSignedIn)
                {
                    lblWelcome.Text = "Welcome to User Registration";
                    return;
                }

                if (!isAdminSession)
                {
                    lblWelcome.Text = "Create your Sweet Delights account";
                    Save_btn.Text = "Register";
                    return;
                }

                using (SqlConnection con = DatabaseHelper.GetOpenConnection())
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT Name FROM emp_tbl WHERE Email = @Email",
                    con))
                {
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value =
                        DatabaseHelper.AdminEmail;
                    object name = cmd.ExecuteScalar();
                    lblWelcome.Text = name == null || name == DBNull.Value
                        ? "Welcome " + DatabaseHelper.AdminEmail
                        : "Welcome " + name.ToString();
                }

                FillGrid();
            }
        }

        private void FillData()
        {
            int id = Convert.ToInt32(ViewState["id"]);

            using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT Name, Gender, Email, City, Address, Mobile FROM emp_tbl WHERE Id = @Id",
                con))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("The selected user record no longer exists.");
                    }

                    txtusername.Text = Convert.ToString(reader["Name"]);
                    string gender = Convert.ToString(reader["Gender"]);
                    if (rdogen.Items.FindByValue(gender) != null)
                    {
                        rdogen.SelectedValue = gender;
                    }

                    txtemail.Text = Convert.ToString(reader["Email"]);
                    string city = Convert.ToString(reader["City"]);
                    if (drpcity.Items.FindByValue(city) != null)
                    {
                        drpcity.SelectedValue = city;
                    }

                    txtaddress.Text = Convert.ToString(reader["Address"]);
                    txtmbl.Text = Convert.ToString(reader["Mobile"]);
                }
            }
        }

        protected void Save_btn_Click(object sender, EventArgs e)
        {
            bool isInsert = Save_btn.Text == "Save";

            if (isInsert)
            {
                bool shouldSignInNewUser = Session["uid"] == null;
                if (!DatabaseHelper.IsAdminEmail(Convert.ToString(Session["admin"])) &&
                    DatabaseHelper.IsAdminEmail(txtemail.Text.Trim()))
                {
                    Response.Write("<script>alert('This email address is reserved for the store administrator.')</script>");
                    return;
                }

                FileUpload();

                using (SqlConnection con = DatabaseHelper.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand(
                "INSERT INTO emp_tbl (Name, Gender, Email, Password, City, Address, Mobile, Image) " +
                "VALUES (@Name, @Gender, @Email, @Password, @City, @Address, @Mobile, @Image); " +
                "SELECT CAST(SCOPE_IDENTITY() AS int);",
                con))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = txtusername.Text;
                    cmd.Parameters.Add("@Gender", SqlDbType.VarChar, 20).Value = rdogen.SelectedValue;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = txtemail.Text;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 100).Value = txtpassword.Text;
                    cmd.Parameters.Add("@City", SqlDbType.VarChar, 50).Value = drpcity.SelectedValue;
                    cmd.Parameters.Add("@Address", SqlDbType.VarChar, 255).Value = txtaddress.Text;
                    cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 20).Value = txtmbl.Text;
                    cmd.Parameters.Add("@Image", SqlDbType.VarChar, 255).Value = fnm;

                    int userId;
                    try
                    {
                        userId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627 || ex.Number == 2601)
                        {
                            Response.Write("<script>alert('This email address is already registered.')</script>");
                            return;
                        }

                        throw;
                    }

                    if (shouldSignInNewUser)
                    {
                        Session["uid"] = userId;
                        Session["admin"] = txtemail.Text;
                        Session["User"] = new User
                        {
                            FullName = txtusername.Text,
                            Email = txtemail.Text,
                            Phone = txtmbl.Text,
                            IsLoggedIn = true
                        };
                        lblWelcome.Text = "Welcome " + txtusername.Text;
                    }
                }

                Response.Write("<script>alert('User registered successfully.')</script>");
            }
            else
            {
                if (!RequireAdminSession())
                {
                    return;
                }

                int id = Convert.ToInt32(ViewState["id"]);

                using (SqlConnection con = DatabaseHelper.GetOpenConnection())
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE emp_tbl SET Name = @Name, Gender = @Gender, Email = @Email, " +
                    "City = @City, Address = @Address, Mobile = @Mobile WHERE Id = @Id",
                    con))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = txtusername.Text;
                    cmd.Parameters.Add("@Gender", SqlDbType.VarChar, 20).Value = rdogen.SelectedValue;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = txtemail.Text;
                    cmd.Parameters.Add("@City", SqlDbType.VarChar, 50).Value = drpcity.SelectedValue;
                    cmd.Parameters.Add("@Address", SqlDbType.VarChar, 255).Value = txtaddress.Text;
                    cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 20).Value = txtmbl.Text;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    cmd.ExecuteNonQuery();
                }

                Response.Write("<script>alert('User details updated successfully.')</script>");
                Save_btn.Text = "Save";
            }

            Clear();
            if (DatabaseHelper.IsAdminEmail(Convert.ToString(Session["admin"])))
            {
                FillGrid();
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!RequireAdminSession())
            {
                return;
            }

            int id;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out id))
            {
                throw new InvalidOperationException("The selected user record has an invalid ID.");
            }

            if (e.CommandName == "cmd_edt")
            {
                ViewState["id"] = id;
                Save_btn.Text = "Update";
                FillData();
            }
            else if (e.CommandName == "cmd_del")
            {
                using (SqlConnection con = DatabaseHelper.GetOpenConnection())
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM emp_tbl WHERE Id = @Id",
                    con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    cmd.ExecuteNonQuery();
                }

                FillGrid();
                Response.Write("<script>alert('User record deleted successfully.')</script>");
            }
        }
    }
}
