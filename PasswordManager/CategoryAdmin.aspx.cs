using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PasswordManager
{
    public partial class CategoryAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx");
            }
            if (!Session["IsCategoryAdmin"].ToString().ToLower().Equals("true"))
            {
                Master.FindControl("MainContent").Visible = false;
                Master.FindControl("NotAuthorizedMessage").Visible = true;
                return;
            }
            else
            {
                Load_Categorys();
            }

        }

        protected void Load_Categorys()
        {
            SqlDataReader myDataReader;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select * From PassKeeper_Categorys";
                    connection.Open();

                    myDataReader = cmd.ExecuteReader();

                    while (myDataReader.Read())
                    {
                      ListBox_Categorys.Items.Add(new ListItem(myDataReader["Category_Name"].ToString(), myDataReader["Category_ID"].ToString()));
                    }

                    myDataReader.Close();
                }
            }
        }
    }
}