<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_eve_mantto.aspx.cs" Inherits="WSIC2010.w_eve_mantto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">   
    <style type="text/css">
        .style1
        {
            width: 130px;
        }
        .styled-select {
           /*background-color: #465C71;
           color: White;
           width: 268px;
           padding: 5px;
           font-size: 10px;
           line-height: 1;
           border: 0;
           border-radius: 0;
           height: 26px;
           -webkit-appearance: none;*/
            height: 24px;
            margin-right:20px;
            margin-bottom:15px;
            border:solid 1px #aacfe4;
            margin:0px 0px 10px 10px;
            font-size:11px;
         }
    </style>
    <link href="../Styles/marco_root.css" type="text/css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<br />   
<asp:Label ID="LabelMensaje" runat="server" Text="REGISTROS"></asp:Label>

    <table>
    <tr>
        <th colspan="2" align="left">
        <label>Seleccione tipo programa:</label>
            <asp:DropDownList ID="dl_TipoPrograma" runat="server" AutoPostBack="true"  CssClass="styled-select"
                onselectedindexchanged="dl_TipoPrograma_SelectedIndexChanged">
                <asp:ListItem Text="TODOS" Value="0"/>
                <asp:ListItem Text="EJECUTADO"  Value="1"/>
                <asp:ListItem Text="PROGRAMADO DIARIO"  Value="2" Selected="True"/>
                <asp:ListItem Text="PROGRAMADO SEMANAL"  Value="3"/>
                <asp:ListItem Text="PROGRAMADO MENSUAL"  Value="4"/>
                <asp:ListItem Text="PROGRAMADO ANUAL"  Value="5"/>
            </asp:DropDownList>
        </th>
        <th>
            <asp:HyperLink NavigateUrl="http://contenido.coes.org.pe/alfrescostruts/download.do?nodeId=d74862fd-979a-49a4-a238-0c00c31ef53b"
                           Text="Descargue Manual Usuario" Target="_blank"
                           runat="server"
                           ID="h1">Manual Usuario (Descargar)
            </asp:HyperLink>
        </th>
    </tr>
    <tr>
        <th>
            <asp:ListBox ID="ListBoxRegistros" runat="server" Height="500px" Width="700px" 
                Font-Names="Arial" Font-Overline="False" ></asp:ListBox>
        </th>
        <th>
            &nbsp;
        </th>
        <th class="style1">   
         <asp:CheckBox ID="CheckBoxEmpresas" runat="server" Text="Filtrar equipos solo de su empresa." 
        Checked="True" ForeColor="#0066CC" />
        <br />
            <asp:Button ID="ButtonEdit" runat="server" Text="Mantenimientos" Width="100%"  Height="30px"
                onclick="ButtonEdit_Click"/>
            <br />   
            <br />   
             <asp:Button ID="ButtonEditManttos" runat="server" Text="Editar Manttos" Width="100%"  Height="30px"
                 onclick="ButtonEditManttos_Click" Enabled="false" Visible="False"/>            
            <br />
            <br />
            <asp:Button ID="ButtonNew" runat="server" Text="Crear" Width="100%"  Height="30px"
                onclick="ButtonNew_Click" />
        </th>
    </tr>
    </table>
</asp:Content>
