using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

namespace SweetDelights
{
    public partial class Add_Products : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds;

        string fnm;

        string s = ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString;

        void getCon()
        {
            con = DatabaseHelper.GetOpenConnection();
        }

        void clear()
        {
            txtmodel.Text = string.Empty;
            txtdesc.Text = string.Empty;
            txtweight.Text = string.Empty;
            txtconfig.Text = string.Empty;
            txtprice.Text = string.Empty;
        }

        void fileUpload()
        {
            if (imgUpload.HasFile)
            {
                string folderPath = Server.MapPath("images/");
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }
                fnm = "images/" + imgUpload.FileName;
                imgUpload.SaveAs(Server.MapPath(fnm));
            }
            else
            {
                fnm = "images/default-cake.jpg";
            }
        }

        void fillCombo()
        {
            getCon();

            da = new SqlDataAdapter(
                "SELECT * FROM Add_Company_Tbl",
                con
            );

            ds = new DataSet();

            da.Fill(ds);

            drpcompid.Items.Clear();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                drpcompid.Items.Add(
                    ds.Tables[0].Rows[i]["Comp_Name"].ToString()
                );
            }

            con.Close();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            decimal price;
            if (!decimal.TryParse(txtprice.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out price))
            {
                Response.Write("<script>alert('Please enter a valid product price')</script>");
                return;
            }

            getCon();

            if (drpcompid.SelectedItem == null)
            {
                con.Close();
                Response.Write("<script>alert('Please add a company before adding a product')</script>");
                return;
            }

            cmd = new SqlCommand(
                "SELECT Comp_Id FROM Add_Company_Tbl WHERE Comp_Name = @Comp_Name",
                con);
            cmd.Parameters.Add("@Comp_Name", SqlDbType.VarChar, 150).Value = drpcompid.SelectedItem.Text;
            object companyId = cmd.ExecuteScalar();
            if (companyId == null || companyId == DBNull.Value)
            {
                con.Close();
                Response.Write("<script>alert('The selected company could not be found')</script>");
                return;
            }

            fileUpload();

            cmd = new SqlCommand(
                "INSERT INTO Add_Products_Tbl " +
                "(Prod_Comp_Id, Prod_Name, Prod_Model, Prod_Desc, Prod_Weight, Prod_Config, Prod_Img, Prod_Price) " +
                "VALUES (@Prod_Comp_Id, @Prod_Name, @Prod_Model, @Prod_Desc, @Prod_Weight, @Prod_Config, @Prod_Img, @Prod_Price)",
                con);
            cmd.Parameters.Add("@Prod_Comp_Id", SqlDbType.Int).Value = Convert.ToInt32(companyId);
            cmd.Parameters.Add("@Prod_Name", SqlDbType.VarChar, 150).Value = drpcompid.SelectedItem.Text;
            cmd.Parameters.Add("@Prod_Model", SqlDbType.VarChar, 150).Value = txtmodel.Text;
            cmd.Parameters.Add("@Prod_Desc", SqlDbType.VarChar, -1).Value = txtdesc.Text;
            cmd.Parameters.Add("@Prod_Weight", SqlDbType.VarChar, 50).Value = txtweight.Text;
            cmd.Parameters.Add("@Prod_Config", SqlDbType.VarChar, 255).Value = txtconfig.Text;
            cmd.Parameters.Add("@Prod_Img", SqlDbType.VarChar, 255).Value = fnm;
            SqlParameter priceParameter = cmd.Parameters.Add("@Prod_Price", SqlDbType.Decimal);
            priceParameter.Precision = 18;
            priceParameter.Scale = 2;
            priceParameter.Value = price;

            cmd.ExecuteNonQuery();

            Response.Write(
                "<script>alert('Product Added Successfully')</script>"
            );

            clear();

            con.Close();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DatabaseHelper.IsAdminEmail(Convert.ToString(Session["admin"])))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                fillCombo();

                lblWelcome.Text =
                    "Welcome, " + Session["admin"].ToString();
            }
        }
    }
}
