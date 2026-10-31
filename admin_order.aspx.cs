using System;
using System.Web.UI;

namespace SweetDelights
{
    public partial class admin_order : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string orderNo = Request.QueryString["orderno"];
            int parsedOrderNo;
            string targetUrl = "Admin_Orders.aspx";

            if (int.TryParse(orderNo, out parsedOrderNo) && parsedOrderNo > 0)
            {
                targetUrl += "?orderno=" + parsedOrderNo;
            }

            Response.Redirect(targetUrl, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
