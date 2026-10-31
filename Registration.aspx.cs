using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace CakeShop
{
    public partial class Registration : System.Web.UI.Page
    {
        SqlConnection con;
        SqlDataAdapter da;
        DataSet ds;
        SqlCommand cmd;
        string fnm;

        string s = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\asp.net\\asp.net\\CakeShop\\App_Data\\CakeShopDB.mdf;Integrated Security=True";

        // ── Open Connection ──────────────────────────────────
        void getcon()
        {
            con = new SqlConnection(s);
            con.Open();
        }

        // ── Image Upload ─────────────────────────────────────
        void imgupload()
        {
            if (flpimg.HasFile)
            {
                string folder = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                fnm = "Uploads/" + flpimg.FileName;
                flpimg.SaveAs(Server.MapPath(fnm));
            }
        }

        // ── Fill GridView ─────────────────────────────────────
        void fillgrid()
        {
            getcon();
            da = new SqlDataAdapter("SELECT * FROM Registration", con);
            ds = new DataSet();
            da.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            con.Close();
        }

        // ── Fill Data for Edit ────────────────────────────────
        void filldata()
        {
            getcon();
            da = new SqlDataAdapter("SELECT * FROM Registration WHERE Id='" + ViewState["id"] + "'", con);
            ds = new DataSet();
            da.Fill(ds);

            txtnm.Text  = ds.Tables[0].Rows[0][1].ToString();
            txteml.Text = ds.Tables[0].Rows[0][3].ToString();
            drpct.SelectedValue = ds.Tables[0].Rows[0][4].ToString();
            txtadd.Text = ds.Tables[0].Rows[0][5].ToString();
            txtmbl.Text = ds.Tables[0].Rows[0][6].ToString();

            if (ds.Tables[0].Rows[0]["Gender"].ToString() == "Male")
                rdbgen.SelectedValue = "Male";
            else
                rdbgen.SelectedValue = "Female";

            con.Close();
        }

        // ── Clear Form ────────────────────────────────────────
        void clear()
        {
            txtnm.Text  = "";
            txteml.Text = "";
            txtadd.Text = "";
            txtmbl.Text = "";
            rdbgen.SelectedIndex = -1;
            drpct.SelectedIndex  = 0;
        }

        // ── Page Load ─────────────────────────────────────────
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillgrid();
            }
        }

        // ── Save / Update ─────────────────────────────────────
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "Save")
            {
                // INSERT
                getcon();
                imgupload();
                cmd = new SqlCommand(
                    "INSERT INTO Registration(Name,Gender,Email,City,Address,Mobile,Image) " +
                    "VALUES('" + txtnm.Text + "','" +
                    rdbgen.SelectedValue + "','" +
                    txteml.Text + "','" +
                    drpct.SelectedValue + "','" +
                    txtadd.Text + "','" +
                    txtmbl.Text + "','" +
                    fnm + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();
                fillgrid();
                lblMessage.Text = "✅ Registration saved successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                // UPDATE
                getcon();
                cmd = new SqlCommand(
                    "UPDATE Registration SET " +
                    "Name='"    + txtnm.Text + "'," +
                    "Gender='"  + rdbgen.SelectedValue + "'," +
                    "Email='"   + txteml.Text + "'," +
                    "City='"    + drpct.SelectedValue + "'," +
                    "Address='" + txtadd.Text + "'," +
                    "Mobile='"  + txtmbl.Text + "' " +
                    "WHERE Id='" + ViewState["id"] + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                fillgrid();
                clear();
                btnSave.Text = "Save";
                lblMessage.Text = "✅ Record updated successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
        }

        // ── GridView Edit / Delete ────────────────────────────
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmd_edt")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                ViewState["id"] = id;
                btnSave.Text = "Update";
                filldata();
            }
            else if (e.CommandName == "cmd_dlt")
            {
                getcon();
                cmd = new SqlCommand(
                    "DELETE FROM Registration WHERE Id='" + e.CommandArgument + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                fillgrid();
                lblMessage.Text = "🗑 Record deleted.";
                lblMessage.ForeColor = System.Drawing.Color.OrangeRed;
            }
        }
    }
}
