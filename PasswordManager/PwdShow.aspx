<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PwdShow.aspx.cs" Inherits="PasswordManager.PwdShow" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
    var encpass;
    var dec;
    var deckey;
    var rsadeckey;
    var password;
    var modulu;

    function AESdecrypt(EncSymKey, mo) {
        //Decrypt SymKey
        deckey = sessionStorage.privdeckey;
        modulu = mo;
        creatDecKey(deckey);
        dec = decryptedString(rsadeckey, EncSymKey);
        //dec password
        password = document.getElementById("<%=HiddenField_Password.ClientID%>").value;
        decpass = GibberishAES.dec(password, dec);
        __doPostBack('callPostBack', decpass);
    }

    function creatDecKey(dk) {
        setMaxDigits(19);
        rsadeckey = new RSAKeyPair(
        "",
        dk,
        modulu
        );
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Table ID="Table1" runat="server" Height="90%" Width="90%" 
        BorderColor="#333333" BorderStyle="Solid" GridLines="Vertical">
    <asp:TableRow ID="TableRow1" runat="server" Height="100%" Width="100%">
        <asp:TableCell ID="TableCell1" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="30%" Height="100%">
            <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" OnSelectedNodeChanged="SelectedNodeChanged" >
                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                <ParentNodeStyle Font-Bold="False" />
                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
            </asp:TreeView>
        </asp:TableCell>
        <asp:TableCell ID="TableCell2" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="70%" Height="100%">
            <asp:Label ID="Label1" runat="server" Text="Title:" Width="100px" Height="20px"></asp:Label><asp:TextBox ID="TextBox_Title" runat="server" Width="200px" Height="20px" Enabled="False"></asp:TextBox><br />
            <asp:Label ID="Label5" runat="server" Text="User name:" Width="100px" Height="20px"></asp:Label><asp:TextBox ID="TextBox_Username" runat="server" Width="200px" Height="20px" Enabled="False"></asp:TextBox><br />
            <asp:Label ID="Label2" runat="server" Text="Description:" Width="100px" Height="20px"></asp:Label><asp:TextBox ID="TextBox_Description" runat="server" Width="200px" Height="20px" Enabled="False"></asp:TextBox><br />
            <asp:Label ID="Label6" runat="server" Text="URL:" Width="100px" Height="20px"></asp:Label><asp:TextBox ID="TextBox_URL" runat="server" Width="200px" Height="20px" Enabled="False"></asp:TextBox><br />
            <asp:Label ID="Label3" runat="server" Text="Category:" Width="100px" Height="20px"></asp:Label><asp:TextBox ID="TextBox_Category" runat="server" Width="200px" Height="20px" Enabled="False"></asp:TextBox><asp:Button ID="Button_ChangeCategory" runat="server" Text="Change" Width="100px" Height="20px" /><br />
            <asp:Label ID="Label4" runat="server" Text="Password:" Width="100px" Height="20px"></asp:Label><asp:TextBox ID="TextBox_Password" runat="server" Width="200px" Height="20px" Enabled="False"></asp:TextBox><asp:Button ID="Button_Password" runat="server" Text="Show" Width="100px" Height="20px" OnClick="Button_ShowPasswordOnClick" /><br />
            
        </asp:TableCell>
    </asp:TableRow>

    </asp:Table>
    <asp:HiddenField ID="HiddenField_Category_ID" runat="server" />
    <asp:HiddenField ID="HiddenField_Password" runat="server" />
</asp:Content>
