<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PasswordManager.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

                        <asp:Login ID="Login1" OnLoggedIn="OnLoggedIn" runat="server" DisplayRememberMe="false" DestinationPageUrl="~/ProfileLoadKey.aspx">
                        </asp:Login>

</asp:Content>
