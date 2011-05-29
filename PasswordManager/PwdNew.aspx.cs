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
    public partial class PwdNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.RegisterClientScriptInclude("BigInt", "JScript/BigInt.js");
            ClientScript.RegisterClientScriptInclude("Barrett", "JScript/Barrett.js");
            ClientScript.RegisterClientScriptInclude("RSA", "JScript/RSA.js");
            ClientScript.RegisterClientScriptInclude("gibberishaes", "JScript/gibberish-aes.js");
            ClientScript.RegisterClientScriptInclude("gibberishaesmin", "JScript/gibberish-aes.min.js");

            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    Load_MemberCategorys();
                }
                else
                {
                    string eventTarget = this.Request["__EVENTTARGET"];
                    string eventArgument = this.Request["__EVENTARGUMENT"];

                    if (eventTarget != String.Empty && eventTarget == "callPostBack")
                    {
                        if (eventArgument != String.Empty)
                        {
                            SQLInsertNewPwd(eventArgument, TextBox_Title.Text, TextBox_Description.Text, DropDownList_Category.SelectedValue, TextBox_URL.Text, TextBox_Username.Text);
                            TextBox_Description.Text = "";
                            TextBox_Password.Text = "";
                            TextBox_Title.Text = "";
                            TextBox_Username.Text = "";
                            TextBox_URL.Text = "";
                        }

                    }
                }
            }
        }

        private void SQLInsertNewPwd(string eventArgument, string title, string description, string catId, string url, string username)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO PassKeeper_Passwords (ENC_PASSWORD,CATEGORY_UID,DESCRIPTION,PASSWORD_TITLE,PASSWORD_URL,PASSWORD_USERNAME) VALUES ('" + eventArgument + "','" + catId + "','" + description + "','"+ title +"','"+ url +"','"+ username +"')";
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void Button_CreateNewPasswordOnClick(object sender, EventArgs e)
        {
            //check if all required fields filled
            if (TextBox_Description.Text.Length <= 5)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alertdesctooshort", "<script>alert('Desrciption too short.')</script>");
                return;
            }
            if (TextBox_Password.Text.Length <= 5)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alertpwdtooshort", "<script>alert('Password too short.')</script>");
                return;
            }
            if (TextBox_Title.Text.Length <= 5)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alerttitletooshort", "<script>alert('Title too short.')</script>");
                return;
            }
            //get encrypted SymKey

            string EncSymKey = "";
            string modulu = "";
            SqlDataReader myDataReader;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select [ENC_SYMKEY],[KeyModulu] From PassKeeper_Category_Members INNER JOIN [PassKeeper_Member] ON PassKeeper_Category_Members.MEMBER_UID=PassKeeper_Member.ID where Member_UID='" + Session["UserID"] + "' AND CATEGORY_UID='" + DropDownList_Category.SelectedValue + "'";
                    connection.Open();

                    myDataReader = cmd.ExecuteReader();

                    if(myDataReader.Read())
                    {
                        EncSymKey = myDataReader["ENC_SYMKEY"].ToString();
                        modulu = myDataReader["KeyModulu"].ToString();
                    }
                    myDataReader.Close();
                }
            }
            //Encrypt new Password
            ClientScript.RegisterStartupScript(GetType(), "generatenewpwd", "javascript: AESencrypt('" + EncSymKey + "','" + TextBox_Password.Text + "','" + modulu +"'); ", true);
            //Create in SQL through postback

        }

        private void Load_MemberCategorys()
        {
            SqlDataReader myDataReader;
            DropDownList_Category.Items.Clear();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select [CATEGORY_NAME], [CATEGORY_ID] From PassKeeper_Category_Members INNER JOIN [Passkeeper_Categorys] ON PassKeeper_Category_Members.CATEGORY_UID=PassKeeper_Categorys.CATEGORY_ID where Member_UID='" + Session["UserID"] + "'";
                    connection.Open();

                    myDataReader = cmd.ExecuteReader();

                    while (myDataReader.Read())
                    {
                        DropDownList_Category.Items.Add(new ListItem(myDataReader["CATEGORY_NAME"].ToString(), myDataReader["CATEGORY_ID"].ToString()));
                    }

                    myDataReader.Close();
                }
            }
        }
    }
}