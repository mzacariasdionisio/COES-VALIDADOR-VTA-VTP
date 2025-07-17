<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_noFooter.Master" AutoEventWireup="true" CodeBehind="w_me_demanda.aspx.cs" Inherits="WSIC2010.Demanda.w_me_demanda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        h1 
        {
            font: 1.5em "Helvetica Neue","Lucida Grande","Segoe UI",Arial,Helvetica,Verdana,sans-serif;
        }
        table.sample {
	        /*width:30%;*/
            border-top:1px solid #C8C8C8;/*e5eff8;*/
            border-right:1px solid #C8C8C8;/*e5eff8;*/
            margin:1em auto;
            margin-left: 10px;
            border-collapse:collapse;
            float: left;
        }
        table.sample th {
	        background:#4a70aa;/*f4f9fe;*/
            text-align:center;
            font:bold 0.9em/1.1em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;
            color:#ffffff;/*66a3d3;*/
        }
        table.sample td {
	        color:#678197;
            border-bottom:1px solid #C8C8C8;/*e5eff8;*/
            border-left:1px solid #C8C8C8;/*e5eff8;*/
            padding:.3em 1em;
            text-align:center;
            font:0.8em/1.0em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;
        }
        table.sample td.left {
	        color:#678197;
            border-bottom:1px solid #FFF;/*e5eff8;*/
            border-left:1px solid #FFF;/*e5eff8;*/
            padding:.3em 1em;
            text-align:left;
            font:1.0em/1.2em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;
            font-weight: bold;
            color:#ffffff;
            background:#4a70aa;
        }
        table.sample th strong {
            font:bold 0.7em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;
            margin:.5em .5em .5em 0;
            color:#66a3d3;                    
        }
        table.sample th em {
            color:#f03b58;
            font-weight: bold;
            font-size: .7em;
            font-style: normal;
        }
        .seleccion
        {
           /*font:0.9em/1.1em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;*/
           font: 1em "Helvetica Neue","Lucida Grande","Segoe UI",Arial,Helvetica,Verdana,sans-serif;
           /*background-color: #0066CC;*/
           /*background-color: #006699;*/
           background-color: #FFF;
           width: 650px; 
           margin: 10px;
           padding: 10px;
           border: 1px solid #BBB;
        }
        #FileUpload1, #DropDownList1
        {
            margin-left: 15px;
        }
        #Button1
        {
            margin-left: 15px;
        }
        #UploadStatusLabel
        {
             color: #FF0000;
             font-style: italic;
             margin-left: 40px;    
        }
        .watermarked
        {
            color: #C0C0C0;
            font-style:italic;
            font-size: 1em
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 style="color:#000;"><center>CARGA - DEMANDA EN BARRAS</center></h1>
    <div class="seleccion">
        <table>
            <tr>
                <td style="color:#000;">Empresa:</td>
                <td align="left"><asp:DropDownList ID="DDLEmpresa" runat="server" /></td>
                <td rowspan="5" align="center" style="width:220px;">
                        <asp:Button ID="Button2" runat="server" Text="Grabar en BD" Visible="false" 
                            onclick="Button2_Click" />
                </td>
            </tr>
            <tr>
                    <td style="color:#000;">Tipo de Programa:</td>
                    <td align="left">
                        <asp:DropDownList ID="DDL1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL1_SelectedIndexChanged"/>
                    </td>
                </tr>
            <tr>
                <td style="color:#000;">Fecha:</td>
                <td><asp:TextBox ID="TextBox1"  Enabled="false" runat="server" ></asp:TextBox>&nbsp;dd/mm/aaaa</td>
            </tr>
            <tr>
                <td><asp:Label ID="lblNumSem" runat="server" Text="N° Semana" Visible="false"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="DDLNumSemana" runat="server" Enabled="false" 
                        Visible="false" AutoPostBack="True" 
                        onselectedindexchanged="DDLNumSemana_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="color:#000;">Archivo:</td>
                <td><asp:FileUpload ID="FileUpload1" runat="server" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button id="UploadButon" runat="server" text="Previsualizar la carga" 
                        onclick="UploadButon_Click" /></td>
            </tr>
        </table>
    </div>
        <asp:Label id="StatusLabel" runat="server" text=""/>
</asp:Content>
