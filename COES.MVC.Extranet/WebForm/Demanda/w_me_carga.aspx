<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_noFooter.Master" AutoEventWireup="true" CodeBehind="w_me_carga.aspx.cs" Inherits="WSIC2010.Demanda.w_me_carga" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FileUpload ID="FileUpload1" runat="server" /><br />
    <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" /><br />
    <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
</asp:Content>
