using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace PasswordManager
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("Status.aspx");
            }
        }

        protected void OnLoggedIn(object sender, EventArgs e)
        {
            SqlDataReader myDataReader;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    Session["UserName"] = Login1.UserName;
                    string username = Session["UserName"].ToString();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select * From PassKeeper_Member Where LoginName = @username";
                    cmd.Parameters.AddWithValue("@username", username);
                    connection.Open();

                    myDataReader = cmd.ExecuteReader();

                    if (myDataReader.Read())
                    {
                        Session["UserID"] = myDataReader["ID"].ToString();
                        Session["IsUserAdmin"] = myDataReader["IsUserAdmin"].ToString();
                        Session["IsCategoryAdmin"] = myDataReader["IsCategoryAdmin"].ToString();
                    }

                    myDataReader.Close();
                }
            }
        }
    }
}