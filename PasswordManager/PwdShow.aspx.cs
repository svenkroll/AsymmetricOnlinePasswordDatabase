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
    public partial class PwdShow : System.Web.UI.Page
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
                    Load_MemberCategoryPasswords();
                    TreeView1.CollapseAll();
                }
                else
                {
                    string eventTarget = this.Request["__EVENTTARGET"];
                    string eventArgument = this.Request["__EVENTARGUMENT"];

                    if (eventTarget != String.Empty && eventTarget == "callPostBack")
                    {
                        if (eventArgument != String.Empty)
                        {
                            TextBox_Password.Text = eventArgument;
                        }

                    }
                }
            }
        }


        protected void Button_ShowPasswordOnClick(object sender, EventArgs e)
        {
            string EncSymKey = "";
            string modulu = "";
            SqlDataReader myDataReader;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select [ENC_SYMKEY], [KeyModulu] From PassKeeper_Category_Members INNER JOIN [PassKeeper_Member] ON PassKeeper_Category_Members.MEMBER_UID=PassKeeper_Member.ID where CATEGORY_UID='" + HiddenField_Category_ID.Value + "' AND MEMBER_UID='" + Session["UserID"] + "'";
                    connection.Open();

                    myDataReader = cmd.ExecuteReader();

                    while (myDataReader.Read())
                    {
                        EncSymKey = myDataReader["ENC_SYMKEY"].ToString();
                        modulu = myDataReader["KeyModulu"].ToString();
                    }
                    
                    myDataReader.Close();
                }
            }

            ClientScript.RegisterStartupScript(GetType(), "generatenewpwd", "javascript: AESdecrypt('" + EncSymKey + "','"+ modulu +"'); ", true);
        }
        private void Load_MemberCategoryPasswords()
        {
            foreach (TreeNode t in TreeView1.Nodes)
            {
                SqlDataReader myDataReader;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Select [PASSWORD_TITLE], [PASSWORD_ID] From PassKeeper_Passwords where CATEGORY_UID='" + t.Value + "'";
                        connection.Open();

                        myDataReader = cmd.ExecuteReader();

                        while (myDataReader.Read())
                        {
                            TreeNode tn = new TreeNode(myDataReader["PASSWORD_TITLE"].ToString(), myDataReader["PASSWORD_ID"].ToString());
                            tn.SelectAction = TreeNodeSelectAction.Select;
                            t.ChildNodes.Add(tn);
                        }
                        t.Text = t.Text + "(" + t.ChildNodes.Count + ")";
                        myDataReader.Close();
                    }
                }
            }
        }

        protected void SelectedNodeChanged(object sender, EventArgs e)
        {

            SqlDataReader myDataReader;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select [PASSWORD_TITLE],[PASSWORD_URL],[PASSWORD_USERNAME],[DESCRIPTION],[ENC_PASSWORD],[CATEGORY_NAME],[CATEGORY_ID] From PassKeeper_Passwords INNER JOIN [PassKeeper_Categorys] ON PassKeeper_Passwords.CATEGORY_UID=Passkeeper_Categorys.CATEGORY_ID where PASSWORD_ID='" + TreeView1.SelectedNode.Value + "'";
                    connection.Open();

                    myDataReader = cmd.ExecuteReader();

                    while (myDataReader.Read())
                    {
                        TextBox_Title.Text = myDataReader["PASSWORD_TITLE"].ToString();
                        TextBox_Description.Text = myDataReader["DESCRIPTION"].ToString();
                        HiddenField_Password.Value = myDataReader["ENC_PASSWORD"].ToString();
                        TextBox_Category.Text = myDataReader["CATEGORY_NAME"].ToString();
                        HiddenField_Category_ID.Value = myDataReader["CATEGORY_ID"].ToString();
                        TextBox_URL.Text = myDataReader["PASSWORD_URL"].ToString();
                        TextBox_Username.Text = myDataReader["PASSWORD_USERNAME"].ToString();
                    }

                    myDataReader.Close();
                }
            }
        }

        private void Load_MemberCategorys()
        {
            SqlDataReader myDataReader;

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
                        TreeNode t = new TreeNode(myDataReader["CATEGORY_NAME"].ToString(), myDataReader["CATEGORY_ID"].ToString());

                        t.SelectAction = TreeNodeSelectAction.None;
                        TreeView1.Nodes.Add(t);
                    }

                    myDataReader.Close();
                }
            }
        }
    }
}