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
    public partial class UserAdminNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx");
            }
            if (!Session["IsUserAdmin"].ToString().ToLower().Equals("true"))
            {
                Master.FindControl("MainContent").Visible = false;
                Master.FindControl("NotAuthorizedMessage").Visible = true;
                return;
            }
        }

        protected void ButtonCreateNewUserOnClick(object sender, EventArgs e)
        {
            //check if all textboxes filles
            if (TextBox_Password.Text.Length <= 3)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alertpasstooshort", "<script>alert('Password too short.')</script>");
                return;
            }
            if (TextBox_UserName.Text.Length <= 3)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alertusernametooshort", "<script>alert('Username too short.')</script>");
                return;
            }
            
            //Create SQL Datarow
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO PassKeeper_Member (LoginName,LoginPassword,IsUserAdmin,IsCategoryAdmin,PublicKey) VALUES ('" + TextBox_UserName.Text +"','" + TextBox_Password.Text +"','" + CheckBox_IsUserAdmin.Checked +"','" + CheckBox_IsCategoryAdmin.Checked +"','PublicKeyDummy')";
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            TextBox_Password.Text = "";
            TextBox_UserName.Text = "";
            CheckBox_IsCategoryAdmin.Checked = false;
            CheckBox_IsUserAdmin.Checked = false;

            ClientScript.RegisterClientScriptBlock(GetType(), "alertsuccess", "<script>alert('User successfull created.')</script>");
            return;
        }
    }
}