<%@ Page Title="Stock de Combustible" Language="C#" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true" CodeBehind="w_op_combustible.aspx.cs" Inherits="WSIC2010.w_op_combustible" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <asp:ToolkitScriptManager ID="tscGeneral" runat="server">
    </asp:ToolkitScriptManager>

    <div style="padding:20px">
    
    <asp:UpdatePanel ID="upGeneral" runat="server">
        <ContentTemplate>
        
        <asp:MultiView ID="mvGeneral" runat="server">
        <asp:View runat="server" ID="vRegistro">

           <asp:Label ID="lblMsg" runat="server" Text="" CssClass="errorstock"></asp:Label>

           <div class="tit-page">STOCK DE COMBUSTIBLE</div>
           
           
           <div style="float:right; text-align:right; margin-bottom:10px; width:260px" runat="server" id="divMantenimiento" visible="false">
                <asp:LinkButton ID="lbMantenimiento" runat="server" onclick="lbMantenimiento_Click">Configurar centrales y tipo de combustible</asp:LinkButton>                
           </div>
           <div style="float:right; width:120px; font-weight:bold; text-align:center">
           
           
           <asp:HyperLink NavigateUrl="http://contenido.coes.org.pe/alfrescostruts/download.do?nodeId=0be8af45-b071-4af1-9e7a-2ae52b9a2232"
                           Text="Descargue Manual Usuario" Target="_blank"
                           runat="server"
                           ID="h1">Manual Usuario (Descargar)
            </asp:HyperLink>
           </div>
           <div style="clear:both; margin-bottom:15px"></div>
           <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>Empresa:</td>
                    <td>
                        <asp:DropDownList ID="ddlEmpresaFiltro" runat="server" DataTextField="emprnomb" 
                            DataValueField="emprcodi" CssClass="combonuevo" style="width:180px" 
                            AutoPostBack="True" 
                            onselectedindexchanged="ddlEmpresaFiltro_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>Fecha Inicio:</td>
                    <td>
                        <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="cajanuevo" style="width:70px"></asp:TextBox>
                        <asp:CalendarExtender ID="cFechaInicio" runat="server" PopupButtonID="txtFechaInicio"
                             TargetControlID="txtFechaInicio" Format="dd/MM/yyyy" CssClass="calendar-css">
                        </asp:CalendarExtender>
                    </td>
                    <td>Fin:</td>
                    <td>
                        <asp:TextBox ID="txtFechaFin" runat="server" CssClass="cajanuevo" style="width:70px"></asp:TextBox>
                        <asp:CalendarExtender ID="cFechaFin" runat="server" PopupButtonID="txtFechaFin"
                             TargetControlID="txtFechaFin" Format="dd/MM/yyyy" CssClass="calendar-css">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="botonnuevo" onclick="btnBuscar_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnExportar" runat="server" onclick="btnExportar_Click" Text="Exportar a Excel" CssClass="botonnuevo" />

                        <asp:Panel ID="pnlExportar" runat="server" CssClass="pnlexportar" Visible="false">
                          
                            <table cellpadding="0" cellspacing = "0" border="0" width="100%">
                            <tr>
                                <td>Seleccione fecha:</td>
                                <td>
                                    <asp:TextBox ID="txtFechaExportar" runat="server" CssClass="cajanuevo" style="width:100px"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceFechaExportar" runat="server" PopupButtonID="txtFechaExportar"
                                         TargetControlID="txtFechaExportar" Format="dd/MM/yyyy" CssClass="calendar-css">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align:center; padding-top:15px">
                                    <asp:Button ID="btnAceptarExportar" runat="server" Text="Aceptar" 
                                        onclick="btnAceptarExportar_Click" />    
                                    <asp:Button ID="btnCancelarExportar" runat="server" Text="Cancelar" 
                                        onclick="btnCancelarExportar_Click" />
                                </td>                       
                            </tr>
                            </table>      
                            
                        </asp:Panel>
                    </td>
                    <td style="width:100px"></td>
                    <td>
                        <asp:Button ID="btnCopiar" runat="server" Text="Copiar" OnClick="btnCopiar_Click" CssClass="botonnuevo" />
                    </td>
                    <td>
                        <asp:Button ID="btnAgregar" runat="server" Text="Nuevo" onclick="btnAgregar_Click" CssClass="botonnuevo" />
                    </td>
                    
                </tr>            
           </table>
            
           <div style="clear:both; height:15px"></div>

            <asp:GridView ID="gvStock" runat="server" AutoGenerateColumns="False" GridLines="None" 
                onrowcommand="gvGrid_RowCommand" onrowdatabound="gvGrid_RowDataBound" 
                CssClass= "grid-gral" AllowPaging="True" 
                onpageindexchanging="gvStock_PageIndexChanging" PageSize="40">
                <Columns>
                    <asp:BoundField DataField="EMPRNOMB" HeaderText="Empresa" />
                    <asp:BoundField DataField="GRUPONOMB" HeaderText="Central" />
                    <asp:BoundField DataField="MEDIFECHA" HeaderText="Fecha" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="TIPOINFODESC" HeaderText="Tipo Combustible" />
                    <asp:BoundField DataField="H1" HeaderText="STOCK COMBUSTIBLE" HtmlEncode="false" DataFormatString="{0:#,###.00}" />
                    <asp:BoundField DataField="LASTUSER" HeaderText="Usuario" HtmlEncode="false" DataFormatString="{0:#,###.00}" /> 
                    <asp:BoundField DataField="LASTDATE" HeaderText="Fecha" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy HH:mm}" /> 
                    <asp:TemplateField HeaderText="Modificar">
                        <ItemTemplate>                            
                            <asp:LinkButton ID="lbModificar" runat="server" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' CommandName="Modificar">Modificar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>                
                    <asp:TemplateField HeaderText="Eliminar">
                        <ItemTemplate>
                            <asp:Label ID="lblPtoMedicion" runat="server" Text='<%# Bind("ptomedicodi") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblTipoInformacion" runat="server" Text='<%# Bind("tipoinfocodi") %>' Visible="false"></asp:Label> 
                            <asp:Label ID="lblNota" runat="server" Text='<%# Bind("nota") %>' Visible="false"></asp:Label>
                            <asp:LinkButton ID="lbQuitar" runat="server" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' CommandName="Eliminar">Eliminar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="grid-row"   />
                <HeaderStyle CssClass="grid-header"   />   
                <FooterStyle CssClass = "grid-footer" />
                <SelectedRowStyle CssClass="grid-selected" />
                <EmptyDataRowStyle CssClass="grid-empty" />  
                <AlternatingRowStyle CssClass="grid-alternating" />
            </asp:GridView>          

            <div class="modal-popup" id="divRegistro" runat="server" style="display:none">

                <div class="modal-header">
                    <div class="modal-titulo" runat="server" id="divMove">Registro de Datos</div>
                    <div class="modal-close" runat="server" id="divClose">X</div>
                </div>
                <div class="modal-content">
                    <asp:Label ID="lblTitulo" runat="server" Text=""></asp:Label>

                    <asp:Panel ID="pnlMensaje" runat="server" CssClass="content-message" >
                        <asp:Label ID="lblMensaje" runat="server" Text="Por favor ingrese los datos"></asp:Label>
                    </asp:Panel>

                    <asp:HiddenField ID="hfEdicion" runat="server" />

                    <div style="clear:both; height:15px;"></div>

                    <asp:Panel runat="server" ID="pnlRegistrarNuevo" DefaultButton="btnGrabarStock">

                    <table cellpadding="0" cellspacing="0" border="0"  class="modal-table">                        
                        <tr>
                            <td>Empresas: </td>
                            <td>
                                <asp:DropDownList ID="ddlEmpresa" runat="server" DataTextField="emprnomb" DataValueField="emprcodi" AutoPostBack="True" CssClass="combonuevo" 
                                    onselectedindexchanged="ddlEmpresa_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Central: </td>
                            <td>
                                <asp:DropDownList ID="ddlCentral" runat="server" DataTextField="gruponomb" CssClass="combonuevo"
                                     DataValueField="ptomedicodi" AutoPostBack="True" 
                                    onselectedindexchanged="ddlCentral_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Fecha:</td>
                            <td>
                                <asp:TextBox ID="txtFecha" runat="server" CssClass="cajanuevo"></asp:TextBox>
                                <asp:CalendarExtender ID="cExtender" runat="server" PopupButtonID="txtFecha"
                                    TargetControlID="txtFecha" Format="dd/MM/yyyy" CssClass="calendar-css">
                                </asp:CalendarExtender>
                                <table cellpadding="0"  cellspacing="0" width="auto" border="0" runat="server" id="tablaFecha" visible="false">
                                    <tr>
                                        <td style="padding-left:10px">Desde:</td>
                                        <td>
                                            <asp:TextBox ID="txtLimInferior" runat="server" CssClass="cajanuevo" style="width:70px"></asp:TextBox>
                                            <asp:CalendarExtender ID="ceLimInferior" runat="server" PopupButtonID="txtLimInferior"
                                                TargetControlID="txtLimInferior" Format="dd/MM/yyyy" CssClass="calendar-css">
                                            </asp:CalendarExtender>                                 
                                        </td>
                                        <td style="padding-left:10px">Hasta:(Opcional)</td>
                                        <td>
                                            <asp:TextBox ID="txtLimSuperior" runat="server" CssClass="cajanuevo" style="width:70px"></asp:TextBox>
                                            <asp:CalendarExtender ID="ceLimSuperior" runat="server" PopupButtonID="txtLimSuperior"
                                                TargetControlID="txtLimSuperior" Format="dd/MM/yyyy" CssClass="calendar-css">
                                            </asp:CalendarExtender>                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>Tipo Combustible:</td>
                            <td>
                                <asp:DropDownList ID="ddlTipoInformacion" runat="server" CssClass="combonuevo"
                                 DataTextField="tipoinfodesc" DataValueField="tipoinfocodi">                                                   
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Disponibilidad:</td>
                            <td>
                                <asp:TextBox ID="txtH1" runat="server" CssClass="cajanuevo"></asp:TextBox>
                            </td>
                        </tr>   
                        <tr>
                            <td>Nota:</td>
                            <td>
                                <asp:TextBox ID="textNota" runat="server" CssClass="cajanuevo"></asp:TextBox>
                            </td>
                        </tr> 
                        <tr>  
                            <td colspan="2" style="text-align:center">
                                <div style="clear:both; height:10px"></div>
                                <asp:Button ID="btnGrabarStock" runat="server" Text="Registrar" 
                                    onclick="btnGrabarStock_Click" CssClass="botonnuevo" />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                                    onclick="btnCancelar_Click"  CssClass="botonnuevo"/>
                            </td>
                        </tr>
                    </table>                   

                    </asp:Panel>


                    <asp:Panel runat="server" ID="pnlConfirmarGrabar" DefaultButton="btnConfirmarGrabar" Visible="false">

                        <div style="height:170px; overflow-y:scroll">
                        <asp:GridView ID="gvFechasRepetidas" runat="server" ShowHeader="false" CssClass="grid-gral"  GridLines="None"  AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />                                
                            </Columns>
                            <RowStyle CssClass="grid-row"   />
                            <HeaderStyle CssClass="grid-header"   />   
                            <FooterStyle CssClass = "grid-footer" />
                            <SelectedRowStyle CssClass="grid-selected" />
                            <EmptyDataRowStyle CssClass="grid-empty" />  
                            <AlternatingRowStyle CssClass="grid-alternating" />
                        </asp:GridView>
                        </div>
                        <div style="clear:both"></div>
                        <div id="divMsgConfirmarGrabar" runat="server" visible="true">
                            <strong>¿Desea grabar los datos para los días en los cuales no existen datos?</strong>
                        </div>
                        <div style="width:200px; text-align:center; margin:auto; margin-top:20px">
                            <asp:Button runat="server" ID="btnConfirmarGrabar" OnClick="btnConfirmarGrabar_Click" CssClass="botonnuevo"  Text="Continuar" />
                            <asp:Button runat="server" ID="btnCancelarConfirmar" OnClick="btnCancelarConfirmar_Click" CssClass="botonnuevo"  Text="Cancelar" />
                        </div>
                    
                    </asp:Panel>


                    <div style="clear:both"></div>
                </div>

                <div style="clear:both"></div>
            </div>
            <asp:HiddenField runat="server" ID="hfRegistro" />
            <asp:ModalPopupExtender ID="mpeRegistro" runat="server" TargetControlID="hfRegistro" CancelControlID="divClose"
                 PopupControlID="divRegistro" PopupDragHandleControlID ="divMove" BackgroundCssClass="modal-background">
            </asp:ModalPopupExtender>

            <!--Inicio nuevo -->
                
                <div class="modal-popup" id="divCopiar" runat="server" style="display:none">

                <div class="modal-header">
                    <div class="modal-titulo" runat="server" id="divCopiarMove">Registro de Datos - Copiar</div>
                    <div class="modal-close" runat="server" id="divCopiarClose">X</div>
                </div>
                <div class="modal-content">
                    
                    <asp:Panel ID="pnlMensajeCopiar" runat="server" CssClass="content-message1" >
                        <asp:Label ID="lblMensajeCopiar" runat="server" Text="Ingrese los datos requeridos."></asp:Label>
                    </asp:Panel>                                     

                    <div style="clear:both; height:15px;"></div>

                    <asp:Panel runat="server" ID="pnlCopiarAccion" DefaultButton="btnGrabarCopiar">

                    <table cellpadding="0" cellspacing="0" border="0"  class="modal-table">                        
                        <tr>
                            <td>Empresas: </td>
                            <td>
                                <asp:DropDownList ID="ddlEmpresaCopiar" runat="server" DataTextField="emprnomb" DataValueField="emprcodi" AutoPostBack="True" CssClass="combonuevo" 
                                    onselectedindexchanged="ddlEmpresaCopiar_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Central: </td>
                            <td>
                                <asp:DropDownList ID="ddlCentralCopiar" runat="server" DataTextField="gruponomb" CssClass="combonuevo"
                                     DataValueField="ptomedicodi">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Copiar del día:</td>
                            <td>
                                <asp:TextBox ID="txtFechaCopiar" runat="server" CssClass="cajanuevo" ></asp:TextBox>
                                <asp:CalendarExtender ID="ceFechaCopiar" runat="server" PopupButtonID="txtFechaCopiar"
                                     TargetControlID="txtFechaCopiar" Format="dd/MM/yyyy" CssClass="calendar-css">
                                </asp:CalendarExtender>                               
                            </td>
                        </tr>
                        <tr>
                            <td>A los días:</td>
                            <td>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td style="padding-left:10px">Desde:</td>
                                        <td>
                                            <asp:TextBox ID="txtFechaCopiarDesde" runat="server" CssClass="cajanuevo" style="width:70px"></asp:TextBox>
                                            <asp:CalendarExtender ID="ceCopiarDesde" runat="server" PopupButtonID="txtFechaCopiarDesde"
                                                 TargetControlID="txtFechaCopiarDesde" Format="dd/MM/yyyy" CssClass="calendar-css">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td style="padding-left:10px">Hasta: (Opcional)</td>
                                        <td>
                                            <asp:TextBox ID="txtFechaCopiarHasta" runat="server" CssClass="cajanuevo" style="width:70px"></asp:TextBox>
                                            <asp:CalendarExtender ID="ceCopiarHasta" runat="server" PopupButtonID="txtFechaCopiarHasta"
                                                 TargetControlID="txtFechaCopiarHasta" Format="dd/MM/yyyy" CssClass="calendar-css">
                                            </asp:CalendarExtender>    
                                        </td>
                                    </tr>
                                </table>                               
                            </td>
                        </tr>
                      
                        <tr>  
                            <td colspan="2" style="text-align:center">
                                <div style="clear:both; height:10px"></div>
                                <asp:Button ID="btnGrabarCopiar" runat="server" Text="Registrar" 
                                    onclick="btnGrabarCopiar_Click" CssClass="botonnuevo" />
                                <asp:Button ID="btnCancelarCopiar" runat="server" Text="Cancelar" 
                                    onclick="btnCancelarCopiar_Click"  CssClass="botonnuevo"/>
                            </td>
                        </tr>
                    </table>                   

                    </asp:Panel>


                     <asp:Panel runat="server" ID="pnlConfirmarCopiar" DefaultButton="btnConfirmarGrabar" Visible="false">

                        <div style="height:170px; overflow-y:scroll">
                        <asp:GridView ID="gvRepetidosCopiar" runat="server" ShowHeader="false" CssClass="grid-gral"  GridLines="None"  AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />                                
                            </Columns>
                            <RowStyle CssClass="grid-row"   />
                            <HeaderStyle CssClass="grid-header"   />   
                            <FooterStyle CssClass = "grid-footer" />
                            <SelectedRowStyle CssClass="grid-selected" />
                            <EmptyDataRowStyle CssClass="grid-empty" />  
                            <AlternatingRowStyle CssClass="grid-alternating" />
                        </asp:GridView>
                        </div>
                        <div style="clear:both"></div>

                        <div id="divMsgConfirmarCopiar" runat="server" visible="true">
                            <strong>¿Desea grabar los datos para los días en los cuales no existen datos?</strong>
                        </div>
                        <div style="width:200px; text-align:center; margin:auto; margin-top:20px">
                            <asp:Button runat="server" ID="btnConfirmarCopíar" OnClick="btnConfirmarCopíar_Click" CssClass="botonnuevo"  Text="Continuar" />
                            <asp:Button runat="server" ID="btnCancelarConfirmarCopiar" OnClick="btnCancelarConfirmarCopiar_Click" CssClass="botonnuevo"  Text="Cancelar" />
                        </div>
                    
                    </asp:Panel>

                </div>
            </div>
            <asp:HiddenField runat="server" ID="hfPopupCopiar" />
            <asp:ModalPopupExtender ID="mpeCopiar" runat="server" TargetControlID="hfPopupCopiar" CancelControlID="divCopiarClose"
                 PopupControlID="divCopiar" PopupDragHandleControlID ="divCopiarMove" BackgroundCssClass="modal-background">
            </asp:ModalPopupExtender>

            <!--Inicio Fin -->




        </asp:View>
        <asp:View runat="server" ID="vMantenimiento">

         <div class="tit-page">CONFIGURACIÓN DE CENTRALES Y FUENTES DE ENERGÍA</div>
           <div style="clear:both; margin-bottom:15px"></div>

        <asp:Label ID="lblMsgConfiguracion" runat="server" Text="" CssClass="errorstock"></asp:Label>

        <div style="float:right; text-align:right">
            <asp:Button ID="btnConfiguracion" runat="server" Text="Agregar" onclick="btnConfiguracion_Click" CssClass="botonnuevo" />
            <asp:Button ID="btnCancelConfiguracion" runat="server" Text="Cancelar" 
                onclick="btnCancelConfiguracion_Click" CssClass="botonnuevo" />
        </div>
        <div style="clear:both; height:15px"></div>

        <asp:GridView ID="gvConfiguracion" runat="server" AutoGenerateColumns="False" GridLines="None" 
                onrowcommand="gvConfiguracion_RowCommand" onrowdatabound="gvConfiguracion_RowDataBound" 
                CssClass= "grid-gral" AllowPaging="True" 
                onpageindexchanging="gvConfiguracion_PageIndexChanging" PageSize="40">
                <Columns>
                    <asp:BoundField DataField="EMPRNOMB" HeaderText="Empresa" />
                    <asp:BoundField DataField="GRUPONOMB" HeaderText="Central" />
                    <asp:BoundField DataField="TIPOINFODESC" HeaderText="Tipo Comb." />
                    <asp:BoundField DataField="ESTADO" HeaderText="Estado" />
                    <asp:BoundField DataField="USERCREATE" HeaderText="Usu. Crea." />
                    <asp:BoundField DataField="FECCREATE" HeaderText="Fec. Crea." HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}"  />
                    <asp:BoundField DataField="USERUPDATE" HeaderText="Usu. Actu." />
                    <asp:BoundField DataField="FECUPDATE" HeaderText="Fec. Actu." HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}"  />
                    <asp:TemplateField HeaderText="Modificar">
                        <ItemTemplate>                            
                            <asp:LinkButton ID="lbModificar" runat="server" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' CommandName="Modificar">Modificar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>                
                    <asp:TemplateField HeaderText="Eliminar">
                        <ItemTemplate>
                            <asp:Label ID="lblEmpresa" runat="server" Text='<%# Bind("EMPRCODI") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblGrupo" runat="server" Text='<%# Bind("ptomedicodi") %>' Visible="false"></asp:Label>  
                            <asp:Label ID="lblTipoInformacion" runat="server" Text='<%# Bind("tipoinfocodi") %>' Visible="false"></asp:Label>                                                      
                            <asp:LinkButton ID="lbQuitar" runat="server" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' CommandName="Eliminar">Eliminar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="grid-row"   />
                <HeaderStyle CssClass="grid-header"   />   
                <FooterStyle CssClass = "grid-footer" />
                <SelectedRowStyle CssClass="grid-selected" />
                <EmptyDataRowStyle CssClass="grid-empty" />  
                <AlternatingRowStyle CssClass="grid-alternating" />
            </asp:GridView>
            
             <div class="modal-popup" id="divModalConfiguracion" runat="server" style="display:none">

                <div class="modal-header">
                    <div class="modal-titulo" runat="server" id="divMoveConfiguracion">Registro de Datos</div>
                    <div class="modal-close" runat="server" id="divCloseConfiguracion">X</div>
                </div>
                <div class="modal-content">
                    
                    <asp:Panel ID="pnlMensajeConfiguracion" runat="server" CssClass="content-message" >
                        <asp:Label ID="lblMensajeConfiguracion" runat="server" Text="Por favor ingrese los datos"></asp:Label>
                    </asp:Panel>

                    <div style="clear:both; height:15px;"></div>

                    <asp:Panel runat="server" ID="Panel1" DefaultButton="btnGrabarConfiguracion">
                    <table cellpadding="0" cellspacing="0" border="0"  class="modal-table">                        
                        <tr>
                            <td>Empresas: </td>
                            <td>
                                <asp:DropDownList ID="ddlEmpresaConfiguracion" runat="server" DataTextField="emprnomb" DataValueField="emprcodi" AutoPostBack="True" CssClass="combonuevo" 
                                    onselectedindexchanged="ddlEmpresaConfiguracion_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Central: </td>
                            <td>
                                <asp:DropDownList ID="ddlCentralConfiguracion" runat="server" DataTextField="gruponomb" CssClass="combonuevo"
                                     DataValueField="ptomedicodi">
                                </asp:DropDownList>
                            </td>
                        </tr>                       
                        <tr>
                            <td>Tipo Combustible:</td>
                            <td>
                                <asp:DropDownList ID="ddlCombustibleConfiguracion" runat="server" CssClass="combonuevo">
                                    <asp:ListItem Text="--SELECCIONE--" Value=""></asp:ListItem>
                                    <asp:ListItem Value="42" Text="Carbón (Toneladas)"></asp:ListItem>
                                    <asp:ListItem Value="43" Text="Biodiesel (Galones)"></asp:ListItem>
                                    <asp:ListItem Value="44" Text="R6 (Galones)"></asp:ListItem>
                                    <asp:ListItem Value="45" Text="R500 (Galones)"></asp:ListItem>
                                    <asp:ListItem Value="46" Text="Gas natural (Mm3)"></asp:ListItem>                                    
                                    <asp:ListItem Value="47" Text="Diesel (Galones)"></asp:ListItem>  
                                </asp:DropDownList>
                            </td>
                        </tr>   
                        <tr>
                            <td>Estado:</td>
                            <td>
                                 <asp:DropDownList ID="ddlEstadoConfiguracion" runat="server" CssClass="combonuevo">
                                    <asp:ListItem Value="" Text="--SELECCIONE--"></asp:ListItem>
                                    <asp:ListItem Value="A" Text="Activo"></asp:ListItem>
                                    <asp:ListItem Value="I" Text="Inactivo"></asp:ListItem>
                                 </asp:DropDownList>
                            </td>
                        </tr>
                         <tr>  
                            <td colspan="2" style="text-align:center">
                                <div style="clear:both; height:10px"></div>
                                <asp:Button ID="btnGrabarConfiguracion" runat="server" Text="Registrar" 
                                    onclick="btnGrabarConfiguracion_Click" CssClass="botonnuevo" />
                                <asp:Button ID="btnCancelarConfiguracion" runat="server" Text="Cancelar" 
                                    onclick="btnCancelarConfiguracion_Click"  CssClass="botonnuevo"/>
                                <asp:HiddenField ID="hfIdEmpresaConfig" runat="server" />
                                <asp:HiddenField ID="hfIdCentalConfig" runat="server" />
                                <asp:HiddenField ID="hfIdTipoInfoConfig" runat="server" />
                            </td>
                        </tr>                    
                    </table>       
                    
                    </asp:Panel>           

                </div>
            </div>
            <asp:HiddenField runat="server" ID="hfConfiguracion" />
            <asp:ModalPopupExtender ID="mpeConfiguracion" runat="server" TargetControlID="hfConfiguracion" CancelControlID="divCloseConfiguracion"
                 PopupControlID="divModalConfiguracion" PopupDragHandleControlID ="divMoveConfiguracion" BackgroundCssClass="modal-background">
            </asp:ModalPopupExtender>
            
        </asp:View>
    </asp:MultiView>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAceptarExportar" />
        </Triggers>
    </asp:UpdatePanel>

    </div>
</asp:Content>
