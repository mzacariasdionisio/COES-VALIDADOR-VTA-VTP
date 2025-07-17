<%@ Page Title="" Language="C#" MasterPageFile="~/WebForm/Master/master_login.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WSIC2010.Account.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
    .lb_error
    {
        color:#F00;
        font-size: 10px;
        font-weight:bold;
    }

        .separador {
            border-right:1px dashed #3a8dcb;
        }

        .texto {
                
    font-size: 12px;
    font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
    margin: 0px;
    padding: 0px;
    color: #696969;
    line-height:20px
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td valign="top" style="width:50%">
                  <br />                

                 <strong>Estimados Integrantes:</strong>
                        <br />
                        <br />
                        <div class="texto">
                            Se comunica que a través del Procedimiento Administrativo “Envío de Información por parte de los Integrantes mediante la Plataforma Extranet” 
                            que entró en vigencia el 01 de enero del 2016, establece lo siguiente: 

                            "Cada vez que algún Integrante requiera <strong>la asignación y/o cancelación de usuarios y contraseñas </strong> para el acceso a la Plataforma Extranet, 
                            podrá solicitarlo al COES. Para dichos efectos, deberá remitir una solicitud, mediante su Representante Legal, señalando el (los) nombre(s) de la(s) 
                            persona(s) a quien(es) se le(s) creará o cancelará el acceso, esta solicitud se podrá formular vía la Plataforma Extranet o de acuerdo a los formatos 
                            establecidos en el Anexo 1." 
                            <a href='https://www.coes.org.pe/portal/browser/download?url=Marco%20Normativo%2FProcedimientos%2FAdministrativo%2FEnv%C3%ADo%20de%20Informaci%C3%B3n%20por%20parte%20de%20los%20Integrantes%20mediante%20la%20Plataforma%20Extranet.pdf'>Para mayor detalle dar clic aquí</a>
                            
                            <br />
                            <br />
                            Para temas relacionados con el Soporte Técnico como problemas o inconvenientes que se presenten en su manejo escribir un correo electrónico a <a href="mailTo:extranet@coes.org.pe">extranet@coes.org.pe</a>
                       
                            <br />
						    <br />
						
						    De conformidad con la Ley N°29733, Ley de Protección de Datos Personales, el titular da su consentimiento para el tratamiento
                            de los datos personales que por cualquier medio desde el momento de su ingreso o utilización del extranet del Comité de Operaciones
                            Económicas del Sistema Interconectado Nacional (COES).

                            <br />
                            <br />

                            Nota: Para poder acceder con normalidad a los módulos de la plataforma extranet del COES, es necesario que la navegación se realice 
                            a través de Edge, Chrome o Firefox; cabe indicar que , en lo posible no utilice el navegador "Internet Explorer", por lo que presenta 
                            problemas en su funcionamiento (browser no soportado). 
                        </div>
                <br />                
                
            </td>
            <td class="separador" style="width:4%"></td>
            <td valign="top" style="width:46%">
                <div class="marco_basico_main">
                        <div class="marco_login" style="background-color:#c8d9eb; margin-top:50px">
                            <div class="marco_login_titulo">
                                <div class="login_titulo">
                                    <h2>Acceso al SGO-COES</h2>
                                </div>
                            </div>
                            <div class="login_login_logo">
                                <img alt="" src="../images/UserConfig.png" />
                            </div>
                            <div class="marco_login_main">
                                <div class="fila_general">
                                    <label>Usuario:</label>
                                    <asp:TextBox ID="TextBoxUserLogin" runat="server" ></asp:TextBox>
                                </div>
                                <div class="fila_general">
                                    <label>Contrase&ntilde;a:</label>
                                    <asp:TextBox ID="TextBoxUserPassword" runat="server" TextMode="Password" ></asp:TextBox>
                                </div>
                                <div class="fila_general" style="text-align:center; padding-top:15px">
                                    <asp:Button ID="ButtonLogin" runat="server" Text="Ingresar" 
                                        onclick="ButtonLogin_Click"/>
                                </div>
                                <div class="fila_general" style="display:none">
                                    ¿No está registrado? 
                                    <asp:HyperLink ID="HyperLink1" Visible="false" runat="server" NavigateUrl="~/account/registro/index">Obtenga su cuenta de acceso</asp:HyperLink>
                                </div>
                                <div class="fila_general" style="text-align:left;margin-left:0px;padding-left:0px;">
                                    <asp:Label ID="labelmessage" runat="server" Text="" CssClass="lb_error"></asp:Label>
                                </div>
                                <div class="fila_general">
                                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/WebForm/Account/RestorePassword.aspx" Enabled="true" Visible="true">¿Olvido su contrase&ntilde;a? (clic aqu&iacute;)</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
            </td>
        </tr>
    </table>
                    
</asp:Content>
