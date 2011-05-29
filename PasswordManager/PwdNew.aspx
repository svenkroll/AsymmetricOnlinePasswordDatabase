<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PwdNew.aspx.cs" Inherits="PasswordManager.PwdNew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
    var enc;
    var dec;
    var deckey;
    var rsadeckey;
    var modulu;

    function AESencrypt(EncSymKey, password, mo) {
        modulu = mo;
        //Decrypt SymKey
        deckey = sessionStorage.privdeckey;
        creatDecKey(deckey);
        dec = decryptedString(rsadeckey, EncSymKey);
        //enc password
        enc = GibberishAES.enc(password, dec);
        __doPostBack('callPostBack', enc);
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
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:Table ID="Table1" runat="server" Height="90%" Width="90%" BorderColor="#333333" BorderStyle="Solid" GridLines="none">
    <asp:TableRow ID="TableRow1" runat="server" Height="100%" Width="100%">
        <asp:TableCell ID="TableCell1" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="20%" Height="100%">
            <asp:Label ID="Label1" runat="server" Text="Title:"></asp:Label><br />            
        </asp:TableCell>
        <asp:TableCell ID="TableCell2" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="80%" Height="100%">
            <asp:TextBox ID="TextBox_Title" runat="server"></asp:TextBox><br />        
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow_USERNAME" runat="server" Height="100%" Width="100%">
        <asp:TableCell ID="TableCell_Username_1" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="20%" Height="100%">
            <asp:Label ID="Label_usernmae" runat="server" Text="User name:"></asp:Label><br />            
        </asp:TableCell>
        <asp:TableCell ID="TableCell_Username_2" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="80%" Height="100%">
            <asp:TextBox ID="TextBox_Username" runat="server"></asp:TextBox><br />        
        </asp:TableCell>
    </asp:TableRow>
        <asp:TableRow ID="TableRow_URL" runat="server" Height="100%" Width="100%">
        <asp:TableCell ID="TableCell_URL_1" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="20%" Height="100%">
            <asp:Label ID="Label_url" runat="server" Text="URL:"></asp:Label><br />            
        </asp:TableCell>
        <asp:TableCell ID="TableCell_URL_2" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="80%" Height="100%">
            <asp:TextBox ID="TextBox_URL" runat="server"></asp:TextBox><br />        
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow2" runat="server" Height="100%" Width="100%">
        <asp:TableCell ID="TableCell3" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="20%" Height="100%">
            <asp:Label ID="Label3" runat="server" Text="Description:"></asp:Label><br />        
        </asp:TableCell>
        <asp:TableCell ID="TableCell4" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="80%" Height="100%">
            <asp:TextBox ID="TextBox_Description" runat="server"></asp:TextBox><br />        
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow3" runat="server" Height="100%" Width="100%">
        <asp:TableCell ID="TableCell5" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="20%" Height="100%">
            <asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label><br />
        </asp:TableCell>
        <asp:TableCell ID="TableCell6" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="80%" Height="100%">
            <asp:TextBox ID="TextBox_Password" runat="server"></asp:TextBox><br />        
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow4" runat="server" Height="100%" Width="100%">
        <asp:TableCell ID="TableCell7" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="20%" Height="100%">
            <asp:Label ID="Label4" runat="server" Text="Category:"></asp:Label><br />        
        </asp:TableCell>
        <asp:TableCell ID="TableCell8" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="80%" Height="100%">
            <asp:DropDownList ID="DropDownList_Category" runat="server"></asp:DropDownList><br />        
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
    <asp:Button ID="Button_CreateNewPassword" runat="server" Text="Create" OnClick="Button_CreateNewPasswordOnClick" />
</asp:Content>
