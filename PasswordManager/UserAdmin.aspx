<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserAdmin.aspx.cs" Inherits="PasswordManager.UserAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script language="javascript" type="text/javascript">
    var rsaenckey;
    var rsadeckey;
    var enc;
    var enckey;
    var deckey;
    var dec;
    var rsatestkey;
    var modulu;

    function createEncKey(ek) {
        setMaxDigits(19);
        // Put this statement in your code to create a new RSA key with these parameters
        rsaenckey = new RSAKeyPair(
        ek, 
        "", 
        modulu
        );
    }

    function creatDecKey(dk) {
        setMaxDigits(19);
        // Put this statement in your code to create a new RSA key with these parameters
        rsadeckey = new RSAKeyPair(
        "", 
        dk, 
        modulu
        );
    }

    function testsessionstorage() {
        sessionStorage.setItem('fullname', 'John Paul');                  // defining the session variable
        alert("Your name is: " + sessionStorage.getItem('fullname'));     // accessing it
        alert("Hello " + sessionStorage.fullname);                        // another way of accessing the variable
        sessionStorage.removeItem('fullname');                            // finally unset it 
    }

    function checkfileapi() {
        // Check for the various File API support.
        if (window.File && window.FileReader && window.FileList && window.Blob) {
            // Great success! All the File APIs are supported.
            alert('The File APIs is fully supported in this browser.');
        } else {
            alert('The File APIs are not fully supported in this browser.');
        }
    }

    function CreateTestKey() {
        setMaxDigits(19);
        // Put this statement in your code to create a new RSA key with these parameters
        rsatestkey = new RSAKeyPair(
        "8819ac9951cdcba37e00e372180389b", //enc 8819ac9951cdcba37e00e372180389b
        "177da19d7d7f3a2a4241245ea02a1f33", //dec 177da19d7d7f3a2a4241245ea02a1f33
        "1d418d5752244eaad6f1b06da0ed8f65" //modulu
        );
        alert(encryptedString(rsatestkey, 'sevenload'));
    }

    function RSAEncrypt(encsymkey, mo) {
        modulu = mo;
        //Decrypt SymKey
        deckey = sessionStorage.privdeckey;
        creatDecKey(deckey);
        dec = decryptedString(rsadeckey, encsymkey);

        //Encrypt SymKey
        enckey = document.getElementById("<%=TextBox_PSK.ClientID%>").value; 
        createEncKey(enckey);
        enc = encryptedString(rsaenckey, dec);
        __doPostBack('callPostBack', enc);
    }

</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent"  runat="server">
    <asp:ListBox ID="ListBox_Users" OnSelectedIndexChanged="UsersSelectedIndexChanged" Width="300px" AutoPostBack="true" runat="server"></asp:ListBox>
    <br />
    <asp:Label ID="Label_PSK" runat="server" Text="EncKey:"></asp:Label><br />
    <asp:TextBox ID="TextBox_PSK" runat="server" Width="300px" Enabled="false" TextMode="MultiLine"></asp:TextBox>
    <p>Member Of Category:</p>
    <asp:ListBox ID="ListBox_CategoryMembership" runat="server" Width="300px" ></asp:ListBox>
    <p>Add To Category:</p>
    <asp:DropDownList ID="DropDown_AddToCategory" Width="300px" runat="server"></asp:DropDownList>
    <asp:Button ID="Button_AddToCategory" runat="server" Text="Add" Enabled="false" 
        onclick="Button_AddToCategory_Click" />
</asp:Content>
