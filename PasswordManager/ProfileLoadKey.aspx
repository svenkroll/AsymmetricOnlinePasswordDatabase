<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfileLoadKey.aspx.cs" Inherits="PasswordManager.ProfileLoadKey" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript"> 

function handle_files(files) {   
   
        file = files[0]     
        var reader = new FileReader()     
        ret = []
        reader.onload = function (e) {
                sessionStorage.setItem('privdeckey', e.target.result);
                document.getElementById("<%=TextBox_PSK.ClientID%>").value = sessionStorage.privdeckey; 
                            }     
        reader.onerror = function(stuff) {
                            console.log("error", stuff)       
                            console.log (stuff.getMessage())     
                        }
        reader.readAsText(file)
} 
</script> 

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:Label ID="Label1" runat="server" Text="PrivateKey:"></asp:Label><asp:TextBox ID="TextBox_PSK" Width="300px" runat="server"></asp:TextBox>
<br />
<input type="file" onchange="handle_files(this.files)" /> 
</asp:Content>
