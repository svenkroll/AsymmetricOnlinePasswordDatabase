<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserAdminNew.aspx.cs" Inherits="PasswordManager.UserAdminNew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:Table ID="Table1" runat="server" Height="90%" Width="90%" BorderColor="#333333" BorderStyle="Solid" GridLines="none">
    <asp:TableRow ID="TableRow1" runat="server" Height="100%" Width="100%">
        <asp:TableCell ID="TableCell1" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="20%" Height="100%">
            <asp:Label ID="Label1" runat="server" Text="Username:" Height="20px"></asp:Label><br />            
        </asp:TableCell>
        <asp:TableCell ID="TableCell2" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="80%" Height="100%">
            <asp:TextBox ID="TextBox_UserName" runat="server" Height="20px"></asp:TextBox><br />            
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow2" runat="server" Height="100%" Width="100%">
        <asp:TableCell ID="TableCell3" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="20%" Height="100%">
            <asp:Label ID="Label7" runat="server" Text="Logon password:" Height="20px"></asp:Label><br />
        </asp:TableCell>
        <asp:TableCell ID="TableCell4" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="80%" Height="100%">
            <asp:TextBox ID="TextBox_Password" runat="server" Height="20px"></asp:TextBox><br />
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow4" runat="server" Height="100%" Width="100%">
        <asp:TableCell ID="TableCell7" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="20%" Height="100%">
            <asp:Label ID="Label4" runat="server" Text="IsUserAdmin:" Height="20px"></asp:Label><br />
        </asp:TableCell>
        <asp:TableCell ID="TableCell8" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="80%" Height="100%">
            <asp:CheckBox ID="CheckBox_IsUserAdmin" runat="server"  Height="20px"/><br />
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="TableRow5" runat="server" Height="100%" Width="100%">
        <asp:TableCell ID="TableCell9" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="20%" Height="100%">
            <asp:Label ID="Label5" runat="server" Text="IsCategoryAdmin:" Height="20px"></asp:Label><br />
        </asp:TableCell>
        <asp:TableCell ID="TableCell10" runat="server" BackColor="White" BorderColor="#333333" BorderStyle="Solid" Width="80%" Height="100%">
            <asp:CheckBox ID="CheckBox_IsCategoryAdmin" runat="server"  Height="20px"/><br />
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
    <asp:Button ID="Button_CreateNewUser" runat="server" Text="Create" OnClick="ButtonCreateNewUserOnClick" />
</asp:Content>
