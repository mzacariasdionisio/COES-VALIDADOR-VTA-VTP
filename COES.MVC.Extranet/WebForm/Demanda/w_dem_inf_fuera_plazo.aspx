<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_noFooter.Master" AutoEventWireup="true" CodeBehind="w_dem_inf_fuera_plazo.aspx.cs" Inherits="WSIC2010.Demanda.w_dem_inf_fuera_plazo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.4.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.8.custom.min.js"></script>
    <link rel="Stylesheet" type="text/css" href="../Styles/jquery-ui-1.8.custom.css"/>
    <style type="text/css">
        h1 
        {
            font: 1.5em "Helvetica Neue","Lucida Grande","Segoe UI",Arial,Helvetica,Verdana,sans-serif;
        }
        .seleccion
        {
           /*font:0.9em/1.1em "Century Gothic","Trebuchet MS",Arial,Helvetica,sans-serif;*/
           font: 1em "Helvetica Neue","Lucida Grande","Segoe UI",Arial,Helvetica,Verdana,sans-serif;
           /*background-color: #0066CC;*/
           /*background-color: #006699;*/
           background-color: #FFF;
           width: 700px; 
           margin: 10px;
           padding: 10px;
           border: 1px solid #BBB;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 style="color:#000;"><center>ENVIO DE INFORMACIÓN - FUERA DE PLAZO</center></h1>
    <div class="seleccion">
        <table>
            <tr>
                <td colspan="2">
                    La información fue cargada fuera de plazo. Detalles a continuación:<br />
                    <asp:ListBox ID="lBox" runat="server" Visible="false"></asp:ListBox><br />
                    La información fue cargada fuera de plazo. Agradeceremos llenar el formulario indicando los motivos por los cuales está reemplazando la información cargada previamente
                </td>
            </tr>
            <tr>
            <td>
            <asp:RadioButtonList ID="rb_list" runat="server" Width="229px">
                <asp:ListItem Text="OMISIÓN EN CARGA INICIAL" Value="2" Selected="True" />
                <asp:ListItem Text="MANTENIMIENTO CORRECTIVO" Value="3" />
                <asp:ListItem Text="FALLA EN EQUIPO (TIEMPO REAL)" Value="4" />
            </asp:RadioButtonList>
            </td>
            <td>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            A&ntilde;ada una breve descipci&oacute;n (150 caracteres)
                <br />
            <asp:TextBox ID="tbox_descipcion" TextMode="MultiLine" runat="server" Height="52px" 
                    Width="404px"></asp:TextBox>
            <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "tbox_descipcion" 
                                            ID="RegularExpressionValidator1" ValidationExpression = "^[\s\S]{0,150}$" runat="server" 
                                            ErrorMessage="Máximo 150 caracteres permitidos."></asp:RegularExpressionValidator>
            </td>
            </tr>
            <tr>
            <td>
            <asp:Button ID="btn_enviar" runat="server" Text="Enviar" 
                    onclick="btn_enviar_Click" />
            </td>
            <td></td>
            </tr>
        </table>
    </div>
</asp:Content>
