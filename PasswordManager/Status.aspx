<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Status.aspx.cs" Inherits="PasswordManager.Status" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <p>UserAdmin: <asp:Literal ID="IsUserAdmin" runat="server" Text="" Visible="true"></asp:Literal></p>
     <p>CategoryAdmin: <asp:Literal ID="IsCategoryAdmin" runat="server" Text="" Visible="true"></asp:Literal></p>
</asp:Content>
