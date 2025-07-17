<%@ Page Title="" Language="C#" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/WebForm/Master/master_base.Master" AutoEventWireup="true"
    CodeBehind="w_eve_mantto_list.aspx.cs" Inherits="WSIC2010.w_eve_mantto_list" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register src="~/WebForm/SeleccionarEquipo.ascx" tagname="SeleccionarEquipo" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function startTime() {
            var ld_fechahora = document.getElementById('<%=hdfecha.ClientID %>').getAttribute('value');
            now = new Date();
            y2k = new Date(ld_fechahora);
            days = (y2k - now) / 1000 / 60 / 60 / 24;
            daysRound = Math.floor(days);
            hours = (y2k - now) / 1000 / 60 / 60 - (24 * daysRound);
            hoursRound = Math.floor(hours);
            minutes = (y2k - now) / 1000 / 60 - (24 * 60 * daysRound) - (60 * hoursRound);
            minutesRound = Math.floor(minutes);
            seconds = (y2k - now) / 1000 - (24 * 60 * 60 * daysRound) - (60 * 60 * hoursRound) - (60 * minutesRound);
            secondsRound = Math.round(seconds);
            sec = (secondsRound == 1) ? " segundo, " : " segundos";
            min = (minutesRound == 1) ? " minuto, " : " minutos, ";
            hr = (hoursRound == 1) ? " hora, " : " horas, ";
            dy = (daysRound == 1) ? " d&iacute;a, " : " d&iacute;as, ";
            diferencia = (y2k - now) / 1000;
            if (diferencia > 0) {
                document.getElementById('<%=LabelTiempoRestante.ClientID %>').innerHTML = "ABIERTO (restan : " + checkTime(daysRound) + dy + checkTime(hoursRound) + hr + checkTime(minutesRound) + min +
                                                                    checkTime(secondsRound) + sec + ")";
            }
            else {
                document.getElementById('<%=LabelTiempoRestante.ClientID %>').innerHTML = "CERRADO";
            }

            t = setTimeout('startTime()', 1000);
        }
        function checkTime(i) {
            if (i < 10) {
                i = "0" + i;
            }
            return i;
        }
        window.onload = function () { startTime(); }
    </script>
    <style type="text/css">
        .wrapper1, .wrapper2{width: 950px; overflow-x: scroll; overflow-y:hidden;}
        .wrapper1{height: 20px; }
        
        .div1 {width:1580px; height: 20px; }
        .div2 {width:1580px; background-color: transparent;overflow: auto;}
    </style>
    <script src="http://code.jquery.com/jquery-1.4.2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(window).load(function () {
            $(function () {
                $(".wrapper1").scroll(function () {
                    $(".wrapper2")
    .scrollLeft($(".wrapper1").scrollLeft());
                });
                $(".wrapper2").scroll(function () {
                    $(".wrapper1")
    .scrollLeft($(".wrapper2").scrollLeft());
                });
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">    
    <div id="div1" style="overflow: auto; width:100%; font-family: Verdana; font-size: 0.8em; " >
   
    <table width="950px">
        <tr>
            <th align="left">
            <asp:Label ID="LabelTituloMantto" runat="server" 
                Text="Mantenimiento Programado Mensual" Font-Bold="True" 
                Font-Size="14px" ForeColor="#0066CC" style="font-family: Calibri"></asp:Label>        
            </th>
            <th align="right">
                <asp:Label ID="LabelTiempoRestante" runat="server" Font-Bold="True" 
                    Font-Size="14px" ForeColor="#FF6666" style="font-family: Calibri"></asp:Label>
                <asp:HiddenField ID="hdfecha" runat="server" value="" />
            </th>
        </tr>
    </table>
           
    <asp:RadioButtonList ID="RadioButtonListDias" runat="server" 
            RepeatDirection="Horizontal" BorderStyle="None" AutoPostBack="True" 
            BackColor="#FAFEE9" 
            onselectedindexchanged="RadioButtonListDias_SelectedIndexChanged" 
            ForeColor="#0000CC" 
            style="font-family: Calibri; font-size: medium; font-weight: 700"  ></asp:RadioButtonList>    
    
    <div class="wrapper1">
        <div class="div1">
        </div>
    </div>
    
    <div class="wrapper2">
    <div class="div2">


    <asp:ListView ID="ListViewManttoList" runat="server" 
        onitemcommand="ListViewManttoList_ItemCommand"
        onitemcanceling="ListViewManttoList_ItemCanceling" 
        onitemdeleting="ListViewManttoList_ItemDeleting" 
        onitemediting="ListViewManttoList_ItemEditing" 
        oniteminserting="ListViewManttoList_ItemInserting" 
        onitemupdating="ListViewManttoList_ItemUpdating"
        ViewStateMode="Enabled" onitemdatabound="ListViewManttoList_ItemDataBound"
        >
                <%--DataKeyNames="mancodi" --%>
        <LayoutTemplate>   
               
            <table id="itemPlaceholderContainer" runat="server" > 
                <%--<tr>
                <th colspan="8"  align="left" >
                    <asp:DataPager ID="pagerTop" runat="server" PageSize="12" PagedControlID="ListViewManttoList" >
                        <Fields>
                            <asp:NextPreviousPagerField ButtonCssClass="command" PreviousPageText="‹" RenderDisabledButtonsAsLabels="true"
                                ShowPreviousPageButton="true" ShowNextPageButton="false" />
                            <asp:NumericPagerField ButtonCount="5" NumericButtonCssClass="command" CurrentPageLabelCssClass="current"
                                NextPreviousButtonCssClass="command" />
                            <asp:NextPreviousPagerField ButtonCssClass="command" NextPageText="›" RenderDisabledButtonsAsLabels="true"
                                ShowNextPageButton="true" ShowPreviousPageButton="false" />
                        </Fields>
                    </asp:DataPager>
                    </th>
                </tr>--%>
                <tr>
                    <th colspan="8"  align="left" >
                        <asp:Button ID="NewButton" runat="server" CommandName="New" Text="Nuevo" />                        
                        <%--<asp:Button ID="CopyEveManttoButton" runat="server"  CommandName="CopyEveMantto" Text="Pegar Mantto Aprob."/>--%>    
                        <%--<asp:Button ID="ImportButton" runat="server"  CommandName="Import" Text="Importar Formato XLS"/>--%>                               
                        <asp:Button ID="GenerateReport" runat="server" CommandName="ReportXLS" Text="Generar Reporte XLS"/>                       
                    </th>

                </tr>   

                <tr align="left" bgcolor="#003399" style="color: #FFFFFF; font-family: Arial, Helvetica, sans-serif; font-size: small;">
                    <th>Opciones</th>
                    <th>TM</th>
                    <th>Empresa</th>
                    <th>Ubicación</th>
                    <th>Fam</th>
                    <th>Equipo</th>
                    <th>Inicio</th>
                    <th>Final</th>
                    <th>MWI.</th>
                    <th>Ind.</th>
                    <th>Int.</th>
                    <th>Descripción</th>
                    <th>Ultima Act.</th>
                    <th>Fec. Creaci&oacute;n</th>
                    <th>Usuario.</th>                    
                </tr>
            <tr id="itemPlaceholder" runat="server">
            </tr>  
           <%-- <tr>
                <td colspan="8" align="center">
                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListViewManttoList"  PageSize="20" class="NavegationBar">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Image" FirstPageImageUrl="~/webform/images/Pager/First_Hover.gif" ShowFirstPageButton="true" 
                            PreviousPageImageUrl="~/webform/images/Pager/Prev_Hover.gif" ShowLastPageButton="false" ShowNextPageButton="false" />
                        <asp:NumericPagerField ButtonCount="5" />
                        <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="true" ShowNextPageButton="true"
                            ShowPreviousPageButton="false" LastPageImageUrl="~/webform/images/Pager/Last_Hover.gif" 
                            NextPageImageUrl="~/webform/images/Pager/Next_Hover.gif" />
                    </Fields>
                </asp:DataPager>
                </td>
             </tr>--%>
                   
            </table>            
        </LayoutTemplate>

        <ItemTemplate>
            <tr valign="middle" style="color: #000000; "0>
                <%-- <td>                    
                    <asp:Label ID="mancodiLabel" runat="server" Text='<%# Eval("mancodi") %>' />
                </td>--%>
               <td  nowrap="nowrap" align="right" valign="top">                                      
                   <asp:ImageButton ID="ObsButton" runat="server" ImageUrl="~/webform/images/flag_red.gif" ToolTip="Contiene observaciones" Enabled="False" CausesValidation="False" />
                   <asp:ImageButton ID="AttachButton" runat="server" CommandName="Attachment"  ImageUrl="~/webform/images/attachment.gif"  ToolTip="Contiene documentos adjuntos" />
                   
                   <asp:ImageButton ID="DeleteButton" runat="server" CommandName="Delete"  ImageUrl="~/webform/images/wdelete.png" Height="16" ToolTip="Cancelar Mantenimiento" />
                   <asp:ImageButton ID="EditButton" runat="server" CommandName="Edit"  ImageUrl="~/webform/images/wedit.png" Height="16" ToolTip="Modificar Registro !" />                   
                   <asp:ImageButton ID="UndoButton" runat="server" CommandName="UndoDelete"  ImageUrl="~/webform/images/undo.gif"  ToolTip="Recuperar Registro Borrado !" />
                                      
                   <asp:Label ID="dlLabelItem" runat="server" ForeColor="#0066CC"></asp:Label>
                   <asp:ImageButton ID="ViewButton" runat="server" CommandName="Edit"  ImageUrl="~/webform/images/WLIST16x16.png" Height="16" ToolTip="Ver Mantenimiento"  CommandArgument="View"/>
               </td>
                    
               <td>
                <asp:Label ID="TIPOEVENABREVLabel" runat="server" Text='<%# Eval("TIPOEVENABREV") %>' />
               </td>
               <td nowrap="nowrap">               
                    <asp:Label ID="mancodiLabel" runat="server" Text='<%# Eval("mancodi") %>' Visible="False" />
                    <asp:Label ID="emprnombLabel" runat="server" Text='<%# Eval("emprnomb").ToString().PadRight(13).Substring(0,13).Trim() %>' />
                    <%--<asp:Label ID="emprcodiLabel" runat="server" Text='<%# Eval("emprcodi") %>' Visible ="False" />--%> 
                </td>
                <td nowrap="nowrap">
                    <asp:Label ID="areanombLabel" runat="server" Text='<%# Eval("areanomb").ToString().PadRight(20).Substring(0,20).Trim() %>' />
                </td>
                <td>
                    <asp:Label ID="famabrevLabel" runat="server" Text='<%# Eval("famabrev") %>' />
                </td>
                <td  nowrap="nowrap" >
                    <asp:Label ID="equiabrevLabel" runat="server" Text='<%# Eval("equiabrev") %>' />
                </td>               
                <td nowrap="nowrap">
                    <asp:Label ID="eveniniLabel" runat="server" Text='<%# Eval("evenini","{0:dd/MM/yyyy HH:mm}") %>' />                  
                </td>
                <td nowrap="nowrap">
                    <asp:Label ID="evenfinLabel" runat="server" Text='<%#  Eval("evenfin", "{0:dd/MM/yyyy HH:mm}") %>' />
                </td>
                <td>
                    <asp:Label ID="evenmwindispLabel" runat="server" Text='<%# Eval("evenmwindisp","{0:0.#}") %>' />
                </td>               
                <td>
                    <asp:Label ID="evenindispoLabel" runat="server" Text='<%# Eval("evenindispo") %>' />
                </td>
                 <td>
                    <asp:Label ID="eveninterrupLabel" runat="server" Text='<%# Eval("eveninterrup") %>' />
                </td>
                <td nowrap="nowrap">
                    <asp:Label ID="evendescripLabel" runat="server" Text='<%# Eval("evendescrip").ToString().PadRight(35).Substring(0,35).Trim() %>' />
                </td>
                <td nowrap="nowrap">
                    <asp:Label ID="lastdateLabel" runat="server" Text='<%# Eval("lastdate","{0:dd/MM HH:mm}") %>' ForeColor="#999999" />
                </td>
                <td nowrap="nowrap">
                    <asp:Label ID="createdLabel" runat="server" Text='<%# Eval("created","{0:dd/MM HH:mm}") %>' ForeColor="#999999" />
                </td>
                <td nowrap="nowrap">
                    <asp:Label ID="lastuserLabel" runat="server" Text='<%# Eval("lastuser")%>' ForeColor="#999999"/>
                </td>
            </tr>
        </ItemTemplate>

        <AlternatingItemTemplate>
            <tr valign="middle" style="color: #000000; background-color: #F0F0F0; ">
           
               <%-- <td>                    
                    <asp:Label ID="mancodiLabel" runat="server" Text='<%# Eval("mancodi") %>' />
                </td>--%>

               <td  nowrap="nowrap" align="right">                   
                   <asp:ImageButton ID="ObsButton" runat="server" ImageUrl="~/webform/images/flag_red.gif" ToolTip="Contiene observaciones" Enabled="False" CausesValidation="False"  />
                   <asp:ImageButton ID="AttachButton" runat="server" CommandName="Attachment"  ImageUrl="~/webform/images/attachment.gif" ToolTip="Contiene documentos adjuntos" />
                   
                   <asp:ImageButton ID="DeleteButton" runat="server" CommandName="Delete"  ImageUrl="~/webform/images/wdelete.png" Height="16" ToolTip="Cancelar Mantenimiento" />
                   <asp:ImageButton ID="EditButton" runat="server" CommandName="Edit"  ImageUrl="~/webform/images/wedit.png" Height="16" ToolTip="Modificar Registro !" />
                   <asp:ImageButton ID="UndoButton" runat="server" CommandName="UndoDelete"  ImageUrl="~/webform/images/undo.gif"  ToolTip="Recuperar Registro Borrado !" />
                   <asp:Label ID="dlLabelItem" runat="server" ForeColor="#0066CC"></asp:Label>
                   <asp:ImageButton ID="ViewButton" runat="server" CommandName="Edit"  ImageUrl="~/webform/images/WLIST16x16.png" Height="16" ToolTip="Ver Mantenimiento" CommandArgument="View" />
               </td>
               <td>
                    <asp:Label ID="TIPOEVENABREVLabel" runat="server" Text='<%# Eval("TIPOEVENABREV") %>' />
               </td>
               <td nowrap="nowrap" width="30">               
                    <asp:Label ID="mancodiLabel" runat="server" Text='<%# Eval("mancodi") %>' Visible="False" />
                    <asp:Label ID="emprnombLabel" runat="server" Text='<%# Eval("emprnomb").ToString().PadRight(13).Substring(0,13).Trim() %>' />
                    <%--<asp:Label ID="emprcodiLabel" runat="server" Text='<%# Eval("emprcodi") %>' Visible="False" /> --%>
                </td>
                <td nowrap="nowrap">
                    <asp:Label ID="areanombLabel" runat="server" Text='<%# Eval("areanomb").ToString().PadRight(20).Substring(0,20).Trim() %>' />
                </td>
                <td>
                    <asp:Label ID="famabrevLabel" runat="server" Text='<%# Eval("famabrev") %>' />
                </td>
                <td >
                    <asp:Label ID="equiabrevLabel" runat="server" Text='<%# Eval("equiabrev") %>' />
                </td>               
                <td nowrap="nowrap">
                    <asp:Label ID="eveniniLabel" runat="server" Text='<%# Eval("evenini","{0:dd/MM/yyyy HH:mm}") %>' />
                </td>
                <td nowrap="nowrap">
                    <asp:Label ID="evenfinLabel" runat="server" Text='<%#  Eval("evenfin", "{0:dd/MM/yyyy HH:mm}") %>' />
                </td>
                <td>
                    <asp:Label ID="evenmwindispLabel" runat="server" Text='<%# Eval("evenmwindisp","{0:0.#}") %>' />
                </td>
                <td>
                    <asp:Label ID="evenindispoLabel" runat="server" Text='<%# Eval("evenindispo") %>' />
                </td>
                 <td>
                    <asp:Label ID="eveninterrupLabel" runat="server" Text='<%# Eval("eveninterrup") %>' />
                </td>
                <td nowrap="nowrap">
                    <asp:Label ID="evendescripLabel" runat="server" Text='<%# Eval("evendescrip").ToString().PadRight(35).Substring(0,35).Trim() %>' />
                </td>
                <td>
                    <asp:Label ID="lastdateLabel" runat="server" Text='<%# Eval("lastdate","{0:dd/MM HH:mm}") %>' ForeColor="#999999"/>
                </td>
                <td>
                    <asp:Label ID="createdLabel" runat="server" Text='<%# Eval("created","{0:dd/MM HH:mm}") %>' ForeColor="#999999"/>
                </td>
                <td>
                    <asp:Label ID="lastuserLabel" runat="server" Text='<%# Eval("lastuser") %>' ForeColor="#999999"/>
                </td>
            </tr>
       </AlternatingItemTemplate>

        <EmptyDataTemplate>
            <table id="Table2" runat="server" style="">
             <tr>
                    <th colspan="5"  align="left">
                        <asp:Button ID="NewButton" runat="server" CommandName="New" Text="Nuevo"/>
                        <asp:Button ID="ImportButton" runat="server"  CommandName="Import" Text="Importar Formato XLS"/>
                        <asp:Button ID="CopyEveManttoButton" runat="server"  CommandName="CopyEveMantto" Text="Copiar de Manttos Aprobados"/>                                     
                    </th>                   
                </tr>
                <tr>
                    <td style="color: #000000; font-weight: bold">
                        <h1>No existe registros.</h1>
                        <br />
                    </td>
                </tr>                
            </table>
        </EmptyDataTemplate>

        <EditItemTemplate>
        <tr>
            <td colspan="9">
                <table bgcolor="#FFFFCC">
                    <tbody>
                        <tr>
                            <th valign="top" align="right">EQUIPO: </th>
                            <td style="color: #000080">   
                                <asp:Label ID="mancodiLabel" runat="server" Text='<%# Eval("mancodi") %>' Visible="False" />                         
                                <asp:Label ID="emprnombLabel" runat="server" Text='<%# Eval("emprnomb") %>' /> 
                                <asp:Label ID="emprcodiLabel" runat="server" Text='<%# Eval("emprcodi") %>' Visible ="False" /> 
                                <asp:Label ID="areanombLabel" runat="server" Text='<%# Eval("areanomb") %>' />:
                                <asp:Label ID="famabrevLabel" runat="server" Text='<%# Eval("famabrev") %>' />
                                <asp:Label ID="equiabrevLabel" runat="server" Text='<%# Eval("equiabrev") %>' />
                            </td>

                        </tr>  
                        <tr>
                          <th align="right" nowrap="nowrap">Tipo:</th>
                            <td>
                              <asp:Label ID="LabelTipoEvenCodi" runat="server" Text='<%# Eval("TipoEvenCodi") %>' Visible="False" />
                              <asp:DropDownList ID="TipoEvenCodiDropDownList" runat="server"/>                                                     
                            </td>
                        </tr>
                         </tr> 
                        <tr>
                         <th align="right" nowrap="nowrap">></th>
                            <td style="color: #993300">
                                 rangos de fechas mayores a 1 dia seran DIVIDIDOS en mantenimientos por dias.
                            </td>
                        </tr>   
                        <tr>
                            <th align="right">Inicio:</th>
                            <td>
                                <asp:TextBox ID="eveniniTextBox" runat="server" Text='<%# Eval("evenini","{0:dd/MM/yyyy HH:mm}")%>' />                                 
                                <asp:Image ID="eveniniImage" runat="server" ImageUrl="~/webform/images/Calendar_scheduleHS.png" /> 
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="eveniniTextBox" PopupButtonID="eveniniImage" Format="dd/MM/yyyy HH:mm" > </asp:CalendarExtender>                              
                                <asp:Label ID="LabeleveniniWarning" runat="server" Text="." ForeColor="#FF3300"></asp:Label>
                                 <asp:MaskedEditExtender ID="MaskedEditExtender1" 
                                    runat="server"
                                    TargetControlID="eveniniTextBox" 
                                    Mask="99/99/9999 99:99"                                 
                                    MaskType="DateTime"
                                    autocomplete = "false"
                                    />
                                 <%-- <asp:MaskedEditValidator ID="MaskedEditValidator1" runat="server"
                                     ControlExtender="MaskedEditExtender1"
                                     ControlToValidate="eveniniTextBox"
                                     EmptyValueMessage="Es requerido la fecha"
                                     InvalidValueMessage="Fecha invalida"
                                     Display="Dynamic"                                    
                                     EmptyValueBlurredText="Es requerido la fecha"
                                     InvalidValueBlurredMessage="Fecha invalida"/>--%>
                            </td>
                        </tr>
                        <tr>
                           <th align="right">Final:</th>
                            <td> 
                                <asp:TextBox ID="evenfinTextBox" runat="server" Text='<%# Eval("evenfin","{0:dd/MM/yyyy HH:mm}") %>'  />                                
                                <asp:Image ID="evenfinImage" runat="server" ImageUrl="~/webform/images/Calendar_scheduleHS.png" /> 
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="evenfinTextBox" PopupButtonID="evenfinImage" Format="dd/MM/yyyy HH:mm"> </asp:CalendarExtender>                              
                                <asp:Label ID="LabelevenfinWarning" runat="server" Text="." ForeColor="#FF3300"></asp:Label>
                                
                                <asp:MaskedEditExtender ID="MaskedEditExtender2" 
                                    runat="server"
                                    TargetControlID="evenfinTextBox" 
                                    Mask="99/99/9999 99:99"                                 
                                    MaskType="DateTime"
                                    autocomplete = "false"
                                    />
                                <%-- <asp:MaskedEditValidator ID="MaskedEditValidator2" runat="server"
                                     ControlExtender="MaskedEditExtender2"
                                     ControlToValidate="evenfinTextBox"
                                     EmptyValueMessage="Es requerido la fecha"
                                     InvalidValueMessage="Fecha invalida"
                                     Display="Dynamic"                                      
                                     EmptyValueBlurredText="Es requerido la fecha"
                                     InvalidValueBlurredMessage="Fecha invalida"/>--%>
                            </td>                            
                        </tr>
                        <tr>
                          <th align="right">MW Indisponibles:</th>
                            <td> 
                                <asp:TextBox ID="evenmwindispTextBox" runat="server" Text='<%# Eval("evenmwindisp","{0:0.#}") %>'  />
                            </td>
                        </tr>
                        <tr>
                          <th align="right">Indisponibilidad (E/F):</th>
                            <td>  
                              <asp:Label ID="Labelevenindispo" runat="server" Text='<%# Eval("evenindispo") %>' Visible="False" />
                              <asp:DropDownList ID="evenindispoDropDownList" runat="server" />                       
                            </td>
                        </tr>
                         <tr>
                          <th align="right" nowrap="nowrap">Genera Interrupción (S/N):</th>
                            <td>                                
                              <asp:Label ID="Labeleveninterrup" runat="server" Text='<%# Eval("eveninterrup") %>' Visible="False" />
                              <asp:DropDownList ID="eveninterrupDropDownList" runat="server"/>                       
                            </td>
                        </tr>
                        <tr>
                          <th align="right" nowrap="nowrap">Situación:</th>
                            <td>
                              <asp:Label ID="LabelEvenTipoProg" runat="server" Text='<%# Eval("EvenTipoProg") %>' Visible="False" />
                              <asp:DropDownList ID="EvenTipoProgDropDownList" runat="server"/>                                                     
                            </td>
                        </tr>
                        <tr>
                           <th valign="top" align="right">Descripción:</th>
                           <td> 
                                <asp:TextBox ID="evendescripTextBox" runat="server" Text='<%# Bind("evendescrip") %>' Rows="4" TextMode="MultiLine" Width="600" />
                           </td>
                        </tr>
                        <tr>
                         <th align="right" nowrap="nowrap">></th>
                            <td style="color: #993300">
                                 longitud m&aacutexima de 300 caracteres.
                            </td>
                        </tr>   
                        <!--<tr>-->
                           <!--<th valign="top" align="right">Observaciones:</th>-->
                           <!--<td>-->
                                <!--<asp:TextBox ID="evenobsrvTextBox" runat="server" Text='<%# Bind("evenobsrv") %>' Rows="4" TextMode="MultiLine" Width="600" />-->
                           <!--</td>-->
                        <!--</tr>-->                                                                 
                        <tr>
                         <th></th>
                            <td>
                                <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Guardar" />
                                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancelar" />
                            </td>
                        </tr>
                        <tr bgcolor="#CCFFFF">
                           <th valign="top" align="right"><h3>Documentos Adjuntos</h3></th>
                           <td>                               
                              <asp:ListBox ID="ListBoxArchivosCargados" runat="server" Rows="4" Width="600"></asp:ListBox>                                
                              <div style="vertical-align: top">                                 
                                <asp:Button id="ButtonAbrirArchivo" Text="Abrir Documento" OnClick="ButtonAbrirArchivo_Click" runat="server" />
                                <asp:Button id="ButtonBorrarArchivo" Text="Borrar Documento" OnClick="ButtonBorrarArchivo_Click" runat="server"/>
                              </div>
                           </td>
                        </tr>
                        <tr bgcolor="#CCFFFF">
                           <th valign="top" align="right">
                               <asp:Label ID="Label1" runat="server" Text="Para adjuntar =>"></asp:Label></th>
                           <td>  
                               <asp:Label ID="LabelUploadText" runat="server" Text="Descripción que se mostrará:"></asp:Label><br />
                               <asp:TextBox ID="TextBoxDescArchivo" runat="server" Width="500"/> 
                               <asp:Label ID="Label2" runat="server" Text="Llenar este campo"></asp:Label>        
                               <asp:FileUpload id="FileUpload1" runat="server" Width="500"></asp:FileUpload>                               
                               <asp:Button id="UploadButton" Text="Cargar Archivo" OnClick="UploadButton_Click" runat="server"> </asp:Button>                                                                 
                           </td> 
                        </tr>        
                        <tr  bgcolor="#CCFFFF">
                            <td>
                            <asp:Button ID="ButtonRegresarListado" CommandName="Cancel" Text="Regresar al listado"  runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        </EditItemTemplate>

        <InsertItemTemplate>
        <tr >
            <td colspan="10">           
                <table bgcolor="#FFFFCC">
                    <tbody>
                        <tr>
                            <th valign="top" align="right">NUEVO MANTENIMIENTO: </th>
                            <th></th>
                        </tr>
                        <tr>
                            <td></td>
                            <td style="color: #000080">   
                                <uc1:SeleccionarEquipo ID="SeleccionarEquipo1" runat="server"  />
                                <asp:Label ID="equicodiLabel" runat="server" Text='<%# Eval("equicodi") %>' Visible="False" />                         
                                <asp:Label ID="emprnombLabel" runat="server" Text='<%# Eval("emprnomb") %>' /> 
                                <asp:Label ID="emprcodiLabel" runat="server" Text='<%# Eval("emprcodi") %>' Visible ="False" /> 
                                <asp:Label ID="areanombLabel" runat="server" Text='<%# Eval("areanomb") %>' />:
                                <asp:Label ID="famabrevLabel" runat="server" Text='<%# Eval("famabrev") %>' />
                                <asp:Label ID="equiabrevLabel" runat="server" Text='<%# Eval("equiabrev") %>' />
                            </td>
                        </tr>                   
                         <tr>
                          <th align="right" nowrap="nowrap">Tipo:</th>
                            <td>
                              <asp:DropDownList ID="TipoEvenCodiDropDownList" runat="server"/>                                                     
                            </td>                            
                        </tr> 
                         <tr>
                         <th align="right" nowrap="nowrap">></th>
                            <td style="color: #993300">
                                 rangos de fechas mayores a 1 dia seran DIVIDIDOS en mantenimientos por dias.
                            </td>
                        </tr>                        
                         <tr>
                            <th align="right">Inicio:</th>
                            <td> 
                                <asp:TextBox ID="eveniniTextBox" runat="server" Text='<%# Eval("evenini","{0:dd/MM/yyyy HH:mm}")%>' /> 
                                <asp:Image ID="eveniniImage" runat="server" ImageUrl="~/webform/images/Calendar_scheduleHS.png" /> 
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="eveniniTextBox" PopupButtonID="eveniniImage" Format="dd/MM/yyyy HH:mm"> </asp:CalendarExtender>                              
                                <asp:Label ID="LabeleveniniWarning" runat="server" Text="." ForeColor="#FF3300"></asp:Label>
                                <asp:MaskedEditExtender ID="MaskedEditExtender1" 
                                    runat="server"
                                    TargetControlID="eveniniTextBox" 
                                    Mask="99/99/9999 99:99"                                 
                                    MaskType="DateTime"
                                    autocomplete = "false"
                                    />
                                 <%-- <asp:MaskedEditValidator ID="MaskedEditValidator1" runat="server"
                                     ControlExtender="MaskedEditExtender1"
                                     ControlToValidate="eveniniTextBox"
                                     EmptyValueMessage="Es requerido la fecha"
                                     InvalidValueMessage="Fecha invalida"
                                     Display="Dynamic"                                    
                                     EmptyValueBlurredText="Es requerido la fecha"
                                     InvalidValueBlurredMessage="Fecha invalida"/>--%>
                            </td>
                        </tr>                    
                        <tr>
                            <th align="right">Final:</th>
                            <td> 
                                <asp:TextBox ID="evenfinTextBox" runat="server" Text='<%# Bind("evenfin") %>' /> <%--//,"{0:dd/MM/yyyy HH:mm}"--%>
                                <asp:Image ID="evenfinImage" runat="server" ImageUrl="~/webform/images/Calendar_scheduleHS.png" /> 
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="evenfinTextBox" PopupButtonID="evenfinImage" Format="dd/MM/yyyy HH:mm"> </asp:CalendarExtender>                              
                                <asp:Label ID="LabelevenfinWarning" runat="server" Text="." ForeColor="#FF3300"></asp:Label> 
                                <asp:MaskedEditExtender ID="MaskedEditExtender2" 
                                    runat="server"
                                    TargetControlID="evenfinTextBox" 
                                    Mask="99/99/9999 99:99"                                 
                                    MaskType="DateTime"
                                    autocomplete = "false"
                                    />
                                 <%--<asp:MaskedEditValidator ID="MaskedEditValidator2" runat="server"
                                     ControlExtender="MaskedEditExtender2"
                                     ControlToValidate="evenfinTextBox"
                                     EmptyValueMessage="Es requerido la fecha"
                                     InvalidValueMessage="Fecha invalida"
                                     Display="Dynamic"                                      
                                     EmptyValueBlurredText="Es requerido la fecha"
                                     InvalidValueBlurredMessage="Fecha invalida"/>--%>
                            </td>
                        </tr>
                        <tr>
                          <th align="right">MW Indisponibles:</th>
                            <td> 
                                <asp:TextBox ID="evenmwindispTextBox" runat="server" Text='<%# Bind("evenmwindisp","{0:0.#}") %>' />
                            </td>
                        </tr>
                        <tr>
                          <th align="right">Indisponibilidad (E/F):</th>
                            <td>                            
                                <asp:DropDownList  ID="evenindispoDropDownList" runat="server"  />                            
                            </td>
                        </tr>                        
                         <tr>
                          <th align="right" nowrap="nowrap">Genera Interrupción (S/N):</th>
                            <td>
                              <asp:DropDownList ID="eveninterrupDropDownList" runat="server"/>                                                     
                            </td>
                        </tr>
                         <tr>
                          <th align="right" nowrap="nowrap">Situación:</th>
                            <td>
                              <asp:DropDownList ID="EvenTipoProgDropDownList" runat="server"/>                                                     
                            </td>
                        </tr>
                        <tr>
                           <th valign="top" align="right">Descripción:</th>
                           <td> 
                                <asp:TextBox ID="evendescripTextBox" runat="server" Text='<%# Bind("evendescrip") %>' Rows="4" TextMode="MultiLine" Width="600" />
                           </td>
                           <td> 
                           </td> 
                        </tr>
                        <tr>
                         <th align="right" nowrap="nowrap">></th>
                            <td style="color: #993300">
                                 longitud máxima de 300 caracteres.
                            </td>
                        </tr>
                        <!--<tr>-->
                           <!--<th valign="top" align="right">Observaciones:</th>-->
                           <!--<td>--> 
                                <!--<asp:TextBox ID="evenobsrvTextBox" runat="server" Text='<%# Bind("evenobsrv") %>' Rows="4" TextMode="MultiLine" Width="600" />-->
                           <!--</td>-->
                           <!--<td>--> 
                           <!--</td> -->
                        <!--</tr>-->
                        <tr>
                         <th></th>
                            <td>
                                <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="Guardar" />
                                <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancelar" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        </InsertItemTemplate>

        </asp:ListView>

        </div>
    </div>
       
        </div>
        
         <%--<asp:DataPager ID="DataPager" runat="server" PagedControlID="ListViewManttoList" PageSize="20" >
            <Fields>
            <asp:TemplatePagerField>
                <PagerTemplate>
                    <b>
                    Total(<asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" /> registros)
                    <br />
                    </b>            
                </PagerTemplate>
            </asp:TemplatePagerField>
            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="False" ></asp:NextPreviousPagerField>
            <asp:NumericPagerField ButtonCount="15"></asp:NumericPagerField>
            <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
            </Fields>
        </asp:DataPager>--%>
        
       
       <br />
    <asp:Label ID="LabelMensaje" runat="server" Text="Registros" 
        ForeColor="#006666"/>


    <asp:ScriptManager ID="ScriptManager1" runat="server"/>        
</asp:Content>
