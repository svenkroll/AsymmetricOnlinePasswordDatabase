using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PasswordManager
{
    public partial class Status : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                IsUserAdmin.Text = Session["IsUserAdmin"].ToString();
                IsCategoryAdmin.Text = Session["IsCategoryAdmin"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
}