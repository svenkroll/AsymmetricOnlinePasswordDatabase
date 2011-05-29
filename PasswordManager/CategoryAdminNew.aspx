<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategoryAdminNew.aspx.cs" Inherits="PasswordManager.CategoryAdminNew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script language="javascript" type="text/javascript">
    var rsaenckey;
    var enc;
    var modulu;

    function createEncKey(ek) {
        setMaxDigits(19);
        rsaenckey = new RSAKeyPair(
        ek,
        "",
        modulu
        );
    }

    function RSAEncrypt(symkey, enckey, mo) {
        modulu = mo;
        createEncKey(enckey);
        enc = encryptedString(rsaenckey, symkey);
        __doPostBack('callPostBack', enc);
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="Label1" runat="server" Text="Category Name:"></asp:Label>
    <asp:TextBox ID="TextBox_CategoryName" runat="server"></asp:TextBox>
    <asp:Label ID="Label2" runat="server" Text="Category SymKey:"></asp:Label>
    <asp:TextBox ID="TextBox_SymKey" runat="server"></asp:TextBox>
    <asp:Button ID="Button_AddCategory" runat="server" Text="add" OnClick="ButtonAddCategoryOnClick" />
</asp:Content>
