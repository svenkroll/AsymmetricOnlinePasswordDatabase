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
    public partial class CategoryAdminNew : System.Web.UI.Page
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
            if (!Session["IsCategoryAdmin"].ToString().ToLower().Equals("true"))
            {
                Master.FindControl("MainContent").Visible = false;
                Master.FindControl("NotAuthorizedMessage").Visible = true;
                return;
            }
            else
            {
                this.ClientScript.GetPostBackEventReference(this, "arg");
                if (IsPostBack)
                {
                    string eventTarget = this.Request["__EVENTTARGET"];
                    string eventArgument = this.Request["__EVENTARGUMENT"];

                    if (eventTarget != String.Empty && eventTarget == "callPostBack")
                    {
                        if (eventArgument != String.Empty)
                        {
                            SQLCreateCategoryMembership(Session["UserID"].ToString(), Session["CategoryIdToCreate"].ToString(), eventArgument);
                        }

                    }
                }
            }
        }

        private void SQLCreateCategoryMembership(string userId, string catId, string encSymKey)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO PassKeeper_Category_Members (ENC_SYMKEY,CATEGORY_UID,MEMBER_UID) VALUES ('" + encSymKey + "','" + catId + "','" + userId + "')";
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            ClientScript.RegisterClientScriptBlock(GetType(), "alertinfoready", "<script>alert('Category successfully created.')</script>");
            return;

        }

        protected void ButtonAddCategoryOnClick(object sender, EventArgs e)
        {
            if (TextBox_CategoryName.Text.Length <= 3)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alertcategorynameshort", "<script>alert('Category name too short, 3 or more signs needed.')</script>");
                return;
            }
            if (CheckCategoryExists(TextBox_CategoryName.Text))
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alertcategoryexist", "<script>alert('Category already exists.')</script>");
                return;
            }
            if (TextBox_SymKey.Text.Length <= 3)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alertsymkeytooshort", "<script>alert('SymKey is too short, 3 or more signs needed.')</script>");
                return;
            }
            string newCatId = "";
            newCatId = SQLCreateCategory(TextBox_CategoryName.Text);
            if (!newCatId.Equals("-1"))
            {
                //Save CatId for later
                Session["CategoryIdToCreate"] = newCatId;

                //get PublicKey
                SqlDataReader myDataReader;
                string pubEncKey = "";
                string modulu ="";
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Select [PublicKey],[KeyModulu] From [PassKeeper_Member] where ID=@USERID";
                        cmd.Parameters.AddWithValue("@USERID", Session["UserID"]);
                        connection.Open();

                        myDataReader = cmd.ExecuteReader();

                        if (myDataReader.Read())
                        {
                            pubEncKey = myDataReader["PublicKey"].ToString();
                            modulu = myDataReader["KeyModulu"].ToString();
                        }

                        myDataReader.Close();
                    }

                }
                //run jscript and let it encrypt the clear string
                ClientScript.RegisterStartupScript(GetType(), "generatenewsymkey", "javascript: RSAEncrypt('" + TextBox_SymKey.Text + "','" + pubEncKey + "','" + modulu + "'); ", true);
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alerterrorinsertsqlcat", "<script>alert('An SQL error occured during category creation!')</script>");
                return;
            }

        }

        private string SQLCreateCategory(string name)
        {
            string newid = "-1";
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO PassKeeper_Categorys (CATEGORY_NAME) VALUES ('" + name + "')";
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    SqlDataReader myDataReader;
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select [CATEGORY_ID] From PassKeeper_Categorys WHERE CATEGORY_NAME=@CATID";
                    cmd.Parameters.AddWithValue("@CATID", name);
                    connection.Open();

                    myDataReader = cmd.ExecuteReader();

                    if (myDataReader.Read())
                    {
                        newid = myDataReader["CATEGORY_ID"].ToString();
                    }

                    myDataReader.Close();
                    connection.Close();
                }
            }
            return newid;
        }

        private bool CheckCategoryExists(string name)
        {
            SqlDataReader myDataReader;
            bool ret = false;

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
                        if (myDataReader["Category_Name"].ToString().Equals(name))
                        {
                            ret = true;
                        }
                    }

                    myDataReader.Close();
                }
            }
            return ret;
        }
    }
}