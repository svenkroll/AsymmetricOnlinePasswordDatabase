<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfileChangeKey.aspx.cs" Inherits="PasswordManager.ProfileChangeKey" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
    var enc;
    var dec;
    var ret;
    var deckey;
    var modulu;
    var enckey;
    var rsadeckey;
    var rsaenckey;

    function ReEncodeSymKeys(EncSymKeyArray, CatSymArray) {
        //Decrypt SymKey
        deckey = sessionStorage.privdeckey;
        enckey = document.getElementById("<%=TextBox_enc_expo.ClientID%>").value;
        modulu = document.getElementById("<%=TextBox_modulu.ClientID%>").value;
        creatDecKey(deckey, modulu);
        createEncKey(enckey, modulu);
        var Keys = EncSymKeyArray.split(",");
        var Cats = CatSymArray.split(",");       
        for (var i = 1; i < Keys.length; ++i) {
            dec = decryptedString(rsadeckey, Keys[i]);
            enc = encryptedString(rsaenckey, dec);
            if (i == 1) {
                ret = enc + "|" + Cats[i];
            }
            else {
                ret += "," + enc + "|" + Cats[i];
            }
        }
        __doPostBack('callPostBack', ret);
    }

    function createEncKey(ek, m) {
        setMaxDigits(19);
        rsaenckey = new RSAKeyPair(
        ek,
        "",
        m 
        );
    }

    function creatDecKey(dk, m) {
        setMaxDigits(19);
        rsadeckey = new RSAKeyPair(
        "",
        dk,
        m
        );
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
    Please take care you have loaded your old private Key, if not you can do it <a href="ProfileLoadKey.aspx">here</a>.<br />
    <br />
        <asp:Table ID="Table1" runat="server" Height="90%" Width="90%" BorderColor="#333333" BorderStyle="Solid" GridLines="Vertical">
            <asp:TableRow ID="TableRow1" runat="server" Height="100%" Width="100%">
                <asp:TableCell ID="TableCell1" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="30%" Height="100%">
                    <asp:Label ID="Label1" runat="server" Text="New PublicKey"></asp:Label><br />
                    <asp:Label ID="Label2" runat="server" Text="New Modulu"></asp:Label><br />
                </asp:TableCell>
            <asp:TableCell ID="TableCell2" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="70%" Height="100%">
                <asp:TextBox ID="TextBox_enc_expo" runat="server" Width="350px"></asp:TextBox><br />
                <asp:TextBox ID="TextBox_modulu" runat="server" Width="350px"></asp:TextBox><br />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Button ID="Button_save" runat="server" Text="Change" OnClick="Button_ChangeOnClick" />
</asp:Content>
