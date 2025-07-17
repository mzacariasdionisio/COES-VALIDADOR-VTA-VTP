<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_me_log.aspx.cs" Inherits="WSIC2010.w_me_log" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>Listar detalle de cargas de información</p>

    <div>

        &nbsp;
       <br />

       

        <br />

       

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Table ID="Table1" runat="server">
        </asp:Table>

       <hr />
       <asp:Label id="UploadStatusLabel"
           runat="server">
       </asp:Label>        
        <asp:GridView ID="gv" runat="server" 
            onrowdatabound="gv_RowDataBound">
        </asp:GridView>
        <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
    </div>
</asp:Content>
