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
    public partial class UserAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.RegisterClientScriptInclude("BigInt", "JScript/BigInt.js");
            ClientScript.RegisterClientScriptInclude("Barrett", "JScript/Barrett.js");
            ClientScript.RegisterClientScriptInclude("RSA", "JScript/RSA.js");

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
            else
            {
                this.ClientScript.GetPostBackEventReference(this, "arg");
                if (!IsPostBack)
                {
                    Load_User();
                    Load_Categorys();
                }
                else
                {
                    string eventTarget = this.Request["__EVENTTARGET"];
                    string eventArgument = this.Request["__EVENTARGUMENT"];

                    if (eventTarget != String.Empty && eventTarget == "callPostBack")
                    {
                        if (eventArgument != String.Empty)
                        {
                            SQLInsertNewSymkey(eventArgument, Session["CategoryToAdd"].ToString(), Session["UserToAdd"].ToString());
                            UpdateCategoryMembership();
                        }

                    }
                }
            }
        }

        protected void SQLInsertNewSymkey(string encsymkey, string cat, string member)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO PassKeeper_Category_Members (ENC_SYMKEY,CATEGORY_UID,MEMBER_UID) VALUES ('"+ encsymkey +"','"+ cat +"','"+ member +"')";                    
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void UpdateCategoryMembership()
        {
            ListBox_CategoryMembership.Items.Clear();
            SqlDataReader myDataReader;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select [MEMBER_UID],[CATEGORY_UID],[CATEGORY_NAME],[PublicKey] From PassKeeper_Category_Members INNER JOIN [PassKeeper_Member] ON PassKeeper_Category_Members.MEMBER_UID=PassKeeper_Member.ID INNER JOIN [PassKeeper_Categorys] ON PassKeeper_Category_Members.CATEGORY_UID=PassKeeper_Categorys.CATEGORY_ID where MEMBER_UID=@USERID";
                    cmd.Parameters.AddWithValue("@USERID", ListBox_Users.SelectedValue);
                    connection.Open();

                    myDataReader = cmd.ExecuteReader();

                    while (myDataReader.Read())
                    {
                        ListBox_CategoryMembership.Items.Add(new ListItem(myDataReader["Category_NAME"].ToString(), myDataReader["Category_UID"].ToString()));

                    }

                    myDataReader.Close();
                    connection.Close();
                }
            }
        }

        protected void UsersSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCategoryMembership();
            TextBox_PSK.Text = "";

            SqlDataReader myDataReader;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select [PublicKey] From [PassKeeper_Member] where ID=@USERID";
                    cmd.Parameters.AddWithValue("@USERID", ListBox_Users.SelectedValue);
                    connection.Open();

                    myDataReader = cmd.ExecuteReader();

                    if (myDataReader.Read())
                    {
                        TextBox_PSK.Text = myDataReader["PublicKey"].ToString();
                    }

                    myDataReader.Close();
                }
                
            }
            Button_AddToCategory.Enabled = true;
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
                        DropDown_AddToCategory.Items.Add(new ListItem(myDataReader["CATEGORY_NAME"].ToString(), myDataReader["CATEGORY_ID"].ToString()));
                    }

                    myDataReader.Close();
                }
            }
        }

        protected void Load_User()
        {
            SqlDataReader myDataReader;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select * From PassKeeper_Member";
                    connection.Open();

                    myDataReader = cmd.ExecuteReader();
                  
                    while (myDataReader.Read())
                    {
                        ListBox_Users.Items.Add(new ListItem(myDataReader["LoginName"].ToString(), myDataReader["ID"].ToString()));
                    }   

                    myDataReader.Close();
                }
            }
        }

        protected void Button_AddToCategory_Click(object sender, EventArgs e)
        {
            
            //check if selected user has already a PublicKey
            if (TextBox_PSK.Text.Length <= 5)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alertpsktooshort","<script>alert('PSK too short.')</script>");
                return;
            }
            //Check if selected user is not already member
            if (ListBox_CategoryMembership.Items.FindByValue(DropDown_AddToCategory.SelectedValue) != null)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alertnothingtodo", "<script>alert('Already member, nothing to do.')</script>");
                return;
            }
            //check if Admin has encrypted symkey for selected category

            SqlDataReader myDataReader;
            string encSymKeyAdmin;
            string modulu = "";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select [MEMBER_UID],[CATEGORY_UID],[ENC_SYMKEY],[KeyModulu] From PassKeeper_Category_Members INNER JOIN [PassKeeper_Member] ON PassKeeper_Category_Members.MEMBER_UID=PassKeeper_Member.ID WHERE MEMBER_UID=@USERID AND CATEGORY_UID=@CATID";
                    cmd.Parameters.AddWithValue("@USERID", Session["UserID"].ToString());
                    cmd.Parameters.AddWithValue("@CATID", DropDown_AddToCategory.SelectedValue.ToString());
                    connection.Open();

                    myDataReader = cmd.ExecuteReader();

                    if (myDataReader.Read())
                    {
                        encSymKeyAdmin = myDataReader["ENC_SYMKEY"].ToString();
                        modulu = myDataReader["KeyModulu"].ToString();
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(GetType(), "alertmisssymkey", "<script>alert('You don't have a SymKey for this Category!')</script>");
                        return;
                    }

                    myDataReader.Close();
                    connection.Close();
                }
            }
            Session["CategoryToAdd"] = DropDown_AddToCategory.SelectedValue;
            Session["UserToAdd"] =ListBox_Users.SelectedValue;

            //Decrypt symkey with AdminPrivKey and RSA encrypt with PublicKey from selected user
            ClientScript.RegisterStartupScript(GetType(), "generatenewsymkey", "javascript: RSAEncrypt('" + encSymKeyAdmin + "','"+ modulu +"'); ", true);
        }
    }
}
