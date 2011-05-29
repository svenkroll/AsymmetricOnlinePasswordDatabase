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
    public partial class ProfileChangeKey : System.Web.UI.Page
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
                            //Update reEncoded SymKeys
                            string[] pairs = eventArgument.Split(',');
                            foreach (string s in pairs)
                            {
                                string[] single = s.Split('|');
                                string key = single[0];
                                string cat = single[1];
                                SQLUpdateCategoryMemberEnySymKey(Session["id"].ToString(), key, cat);
                            }
                            SQLInsertNewPubKey(TextBox_enc_expo.Text, TextBox_modulu.Text);
                            
                            TextBox_modulu.Text = "";
                            TextBox_enc_expo.Text = "";
                            ClientScript.RegisterClientScriptBlock(GetType(), "alertReadyPostback", "<script>alert('Update successfull.')</script>");
                            return;
                        }

                    }
                }
            }
        }

        private void SQLUpdateCategoryMemberEnySymKey(string p, string key, string cat)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE PassKeeper_Category_Members SET (ENC_SYMKEY) VALUES ('" + key + "') WHERE CATEGORY_UID='"+cat+"' AND MEMBER_UID='"+p+"'";
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void Button_ChangeOnClick(object sender, EventArgs e)
        {
            if (TextBox_enc_expo.Text.Length < 1)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alertenctooshort", "<script>alert('PublicKey too short.')</script>");
                return;
            }

            if (TextBox_modulu.Text.Length < 10)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "alertModulutooshort", "<script>alert('Modulu too short.')</script>");
                return;
            }
            
            //ReEnc SymKeys
            ReEncryptSymKeys(Session["UserID"].ToString(), TextBox_enc_expo.Text, TextBox_modulu.Text);
            
        }

        private void ReEncryptSymKeys(string id, string e, string m)
        {
            string EncSymKey = "";
            string CatID = "";

            SqlDataReader myDataReader;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select [ENC_SYMKEY], [CATEGORY_UID] From PassKeeper_Category_Members where Member_UID='" + id + "'";
                    connection.Open();

                    myDataReader = cmd.ExecuteReader();

                    while (myDataReader.Read())
                    {
                        EncSymKey += "," + myDataReader["ENC_SYMKEY"].ToString();
                        CatID += "," + myDataReader["CATEGORY_UID"].ToString();

                    }
                    myDataReader.Close();

                }
            }
            if (EncSymKey.Equals(""))
            {//Nothing to do then change the key, no category symkeys to reencode.
                SQLInsertNewPubKey(TextBox_enc_expo.Text, TextBox_modulu.Text);
                TextBox_modulu.Text = "";
                TextBox_enc_expo.Text = "";
                ClientScript.RegisterClientScriptBlock(GetType(), "alertReady", "<script>alert('Update successfull.')</script>");
                return;
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "reencode", "javascript: ReEncodeSymKeys('" + EncSymKey + "','" + CatID + "');", true);
            }
        }

        private void SQLInsertNewPubKey(string m, string p)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE PassKeeper_Member SET PublicKey='" + p + "',KeyModulu='" + m + "' WHERE ID='" + Session["UserID"] + "'";
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}