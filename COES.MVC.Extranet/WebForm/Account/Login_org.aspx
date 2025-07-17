<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login_org.aspx.cs" Inherits="WSIC2010.Account.Login_org" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Content/Css/styles/base.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/login.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/marco_app.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/marco_base.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/marco_basico.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/marco_general.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/marco_menu.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/marco_root.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/marco_sistema.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/marco_web.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/root.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/sistema.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/Site.css" type="text/css" rel="Stylesheet"/>
    <link href="~/Content/Css/styles/wcoes.css" type="text/css" rel="Stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="marco_root">
        <div class="marco_base_top">
            <div class="marco_base_top_head" style="background-color:#465c71">
                <div class="marco_base_top_a">
                    <div class="marco_base_top_logo">
                    <img id="cph_marco_root_cph_base_top_Image1" src="../images/logo_coes_bkh.png">
                    </div>
                    <div class="marco_base_top_titulo">
                    <h1><span id="cph_marco_root_cph_base_top_Label1">EXTRANET SGO-COES</span></h1>
                    <h4><span id="cph_marco_root_cph_base_top_lb_etiqueta"></span></h4>
                </div>
                </div>
                <div class="marco_base_top_qlink">
                    <div class="marco_top_basic_qlink" style="background-color:#a6c9ed;">
                    <a id="cph_marco_root_cph_base_top_qlink_HyperLink1" href="login.aspx" style="color:White;">Ingresar </a>
                </div>
                </div>
            </div>
            <div class="marco_base_main">
                <div class="marco_basico_main">
                    <div class="marco_login" style="background-color:#a6c9ed;">
                        <div class="marco_login_titulo">
                            <div class="login_titulo">
                                <h2>Acceso al Sistema</h2>
                            </div>
                        </div>
                        <div class="login_login_logo">
                            <img alt="" src="../images/UserConfig.png" />
                        </div>
                        <div class="marco_login_main">
                            <div class="fila_general">
                                <label>Usuario:</label>
                                <asp:TextBox ID="TextBoxUserLogin" runat="server" Width="200px" ></asp:TextBox>
                            </div>
                            <div class="fila_general">
                                <label>Contrase&ntilde;a:</label>
                                <asp:TextBox ID="TextBoxUserPassword" runat="server" TextMode="Password" Width="200px" ></asp:TextBox>
                            </div>
                            <div class="fila_general">
                                <asp:Button ID="ButtonLogin" runat="server" Text="Ingresar" onclick="ButtonLogin_Click" />
                            </div>
                            <div class="fila_general">
                                <span id="fila_blanco">¿No está registrado? 
                                <asp:HyperLink ID="HyperLink1" runat="server" 
                                               NavigateUrl="~/WebForm/Account/Register.aspx">Obtenga su cuenta de acceso
                                </asp:HyperLink></span>
                            </div>
                            <div class="fila_general">
                                <asp:Label ID="labelmessage" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="marco_base_pie">
                <div class="marco_base_pie_main">
                    <span>COES © Todos los derechos reservados</span>
                    <br />
                    <span>C. Manuel Roaud y Paz Soldan 364. San Isidro, Lima - PERÚ Teléfono: (511) 611-8585</span>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
