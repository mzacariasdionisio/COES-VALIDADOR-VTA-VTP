var controlador = siteRoot + 'Combustibles/ConfiguracionGas/';

//variables de correos, ayudaran a validarlos campos de Asuntos y Contenido (igual a lo de ConstantesCombustibles)
const ValMesActual = "{MES_ACTUAL}";
const ValMesActualOSiguiente = "{MES_ACTUAL_O_SIGUIENTE}";
const ValMesSiguiente = "{MES_SIGUIENTE}";
const ValAnio = "{ANIO}";
const ValListaCentrales = "{LISTA_CENTRALES}";
const ValDestinatariosPara = "{DESTINATARIO}";
const ValEmpresa = "{EMPRESA}";
const ValMesVigencia = "{MES_VIGENCIA}";
const ValCostoCombAsignado = "{COSTO_COMB_ASIGNADO}";
const ValIdEnvio = "{ID_ENVIO}";
const ValTipoCombustible = "{TIPO_COMBUSTIBLE}";
const ValFechaSolicitud = "{FECHA_SOLICITUD}";
const ValEstado = "{ESTADO}";
const ValFechaCancelacion = "{FECHA_CANCELACION}";
const ValFechaRevision = "{FECHA_REVISION}";
const ValFechaMaxRpta = "{FECHA_MAX_RESPUESTA}";
const ValFechaSubsanacion = "{FECHA_DE_SUBSANACION}";
const ValFechaPlazoMaxSubsanacion = "{FECHA_PLAZO_MAX_DE_SUBSANACION}";
const ValFechaAprobacion = "{FECHA_DE_APROBACION}";
const ValFechaDesaprobacion = "{FECHA_DE_DESAPROBACION}";
const ValRespuestaCoes = "{RESPUESTA_COES}";
const ValFechaAsignacion = "{FECHA_DE_ASIGNACION}";
const ValFechaInicioPlazo = "{FECHA_INICIO_DE_PLAZO}";
const ValFechaFinPlazo = "{FECHA_FIN_DE_PLAZO}";
const ValListaCentralesAprobadas = "{LISTA_CENTRALES_APROBADAS}";
const ValListaCentralesDesaprobadas = "{LISTA_CENTRALES_DESAPROBADAS}";
const ValHorasCulminacion = "{VALOR_DE_HORAS_CULMINACION}";
const ValDiasRecepcion = "{VALOR_DE_DIAS_RECEPCION}";

//campo que ayudará para validar campo CC y Para
const ValOtrosAgentes = "{OTROS_AGENTES_DE_EMPRESA}";
const ValTodosAgentes = "{TODOS_AGENTES_DE_EMPRESA}";
const ValAgenteUltimo = "{AGENTE_ULTIMO_EVENTO}";

var lstVaraiablesCorreo = [
    ValMesActual, ValMesActualOSiguiente, ValMesSiguiente, ValAnio, ValListaCentrales, ValDestinatariosPara, ValEmpresa, ValMesVigencia, ValCostoCombAsignado,
    ValIdEnvio, ValTipoCombustible, ValFechaSolicitud, ValEstado, ValFechaCancelacion, ValFechaRevision, ValFechaMaxRpta, ValFechaSubsanacion, ValFechaPlazoMaxSubsanacion,
    ValFechaAprobacion, ValFechaDesaprobacion, ValRespuestaCoes, ValFechaAsignacion, ValFechaInicioPlazo, ValFechaFinPlazo,  ValListaCentralesAprobadas,
    ValListaCentralesDesaprobadas, ValHorasCulminacion, ValDiasRecepcion
];

var lstVariablesCC = [
    ValOtrosAgentes, ValTodosAgentes, ValAgenteUltimo
];

var lstVariablesPara = [
    ValAgenteUltimo, ValTodosAgentes
];

const NOTIFICACION = 1;
const RECORDATORIO = 2;
const NOTIFICACION_IMCUMPLIMIENTO_ENTREGA_INFO_CE = 133;
//const NOTIFICACION_IMCUMPLIMIENTO_ENTREGA_INFO_CN = 134;
const NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_CE = 143;
const NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_CN = 144;
const RECORDATORIO_REVISAR_INFORMACION_RECIBIDA_CE = 156;
const RECORDATORIO_REVISAR_INFORMACION_RECIBIDA_CN = 157;
const RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANAR_CE = 158;
const RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANAR_CN = 159;
const RECORDATORIO_REVISAR_SUBSANACIONES_PRESENTADAS_CE = 160;
const RECORDATORIO_REVISAR_SUBSANACIONES_PRESENTADAS_CN = 161;

const VER = 1;
const EDITAR = 2;

const ENVIO_AUTOMATICO = 0;
const ENVIO_MANUAL = 1;

const VARIABLE_ASUNTO = 0;
const VARIABLE_CC = 2;
const VARIABLE_PARA = 3;

const SEPARADOR_INI_VARIABLE = "{";// Debe coincidir con separadores de los valores en ConstantesCombustibles
const SEPARADOR_FIN_VARIABLE = "}";// Debe coincidir con separadores de los valores en ConstantesCombustibles

var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="Editar Envío" width="19" height="19" style="">';
var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Envío" width="19" height="19" style="">';
var tipoPlantillaEnListado = -1; //Notificacion o Recordatorio
var tipoCentralEnListado = -1; //Existente o Nueva
var tipoEnvioEnDetalles = -1; // Automatico o Manual
var respondeATodos = "";


$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');

   
    $('#cbTipoPlantilla').change(function () {        
        mostrarListadoPlantilla();
    });
    $('#cbTipoCentral').change(function () {        
        mostrarListadoPlantilla();
    });

    mostrarListadoPlantilla();
});

function mostrarListadoPlantilla(tipoPlantilla) {
    limpiarBarraMensaje("mensaje");

    var filtro = datosFiltro();

    tipoCentralEnListado = filtro.tipoCentral;
    tipoPlantillaEnListado = filtro.tipoPlantilla;
     
    $.ajax({
        type: 'POST',
        url: controlador + "listarPlantillasCorreos",
        data: {
            tipoPlantilla: filtro.tipoPlantilla,
            tipoCentral: filtro.tipoCentral,

        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                
                var htmlPlantillas = dibujarTablaPlantillas(evt.ListadoPlantillasCorreo, filtro.tipoPlantilla, filtro.tipoCentral, evt.TienePermisoAdmin);
                $("#div_listado").html(htmlPlantillas);

                $('#tablaPlantillas').dataTable({                    
                    "scrollY": 480,
                    "scrollX": false,
                    "sDom": 't',
                    "ordering": false,
                    "iDisplayLength": -1
                });

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
    
}

function datosFiltro() {
    var filtro = {};

    var tipoPlantilla = $("#cbTipoPlantilla").val();
    var tipoCentral = $('#cbTipoCentral').val();

    filtro.tipoPlantilla = tipoPlantilla;
    filtro.tipoCentral = tipoCentral;

    return filtro;
}

function dibujarTablaPlantillas(listaPlantillas, tipoPlantilla, tipoCentral, esAdmin) {
    var num = 0;
    var tipo_plantilla = "";
    if (tipoPlantilla == NOTIFICACION)
        tipo_plantilla = "Notificación";
    else
        tipo_plantilla = "Recordatorio";

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaPlantillas" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:90px;'>Acciones</th>
               <th>N°</th>
               <th>Código de Plantilla</th>
               <th>${tipo_plantilla}</th>
               <th>Usuario Modificación</th>
               <th>Fecha de Modificación</th>        
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaPlantillas) {
        var item = listaPlantillas[key];
        num++;

        var htmlTdAccion = "";
        if (esAdmin) {
            htmlTdAccion = `
                    <a href="JavaScript:mostrarDetalle(${item.Plantcodi},'${tipoCentral}', ${VER});">${IMG_VER}</a>
                    <a href="JavaScript:mostrarDetalle(${item.Plantcodi},'${tipoCentral}', ${EDITAR});">${IMG_EDITAR}</a>
            `;
        } else {
            htmlTdAccion = `
                    <a href="JavaScript:mostrarDetalle(${item.Plantcodi},'${tipoCentral}', ${VER});">${IMG_VER}</a>
            `;
        }

        cadena += `
            <tr>
                <td style='width:90px;'>   
                    ${htmlTdAccion}
                </td>
                <td>${num}</td>
                <td>${item.Plantcodi}</td>
                <td style="text-align:left;">${item.Plantnomb}</td>
                <td>${item.Plantusumodificacion}</td>
                <td>${item.PlantfecmodificacionDesc}</td>
           </tr >           
        `;
    }


    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function mostrarDetalle(plantillacodi, tipoCentral, accion) {
    limpiarBarraMensaje("mensaje");
    $('#tab-container').easytabs('select', '#tabDetalle');

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerDetalleCorreo",
        data: {
            plantillacodi: plantillacodi
            
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                respondeATodos = evt.PlantillaCorreo.RespondeAAgente;

                var htmlPlantilla = dibujarPlantilla(evt.PlantillaCorreo, plantillacodi, tipoCentral, accion, ENVIO_AUTOMATICO, evt.TipoCorreo);
                $("#div_detalle").html(htmlPlantilla);
                
                $.ajax({
                    success: function () {
                        //habilito/deshabilito edicion de contenido
                        var readonly = 0;
                        if (accion == VER) readonly = 1;

                        //muestro editor de contenido
                        iniciarControlTexto('Contenido', evt.ListaVariables, readonly);
                    }
                });

                $('#HoraR').Zebra_DatePicker({
                    format: "H:i",
                });                

                $('#btnGuardarPC').click(function () {
                    guardarPlantilla();                    
                });

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarPlantilla(objPlantilla, plantillacodi, tipoCentral, accion, tipoEnvio, tipoCorreo) {

    var esEditable = true;
    var habilitacion = "";

    if (accion == VER) {
        esEditable = false;
        habilitacion = "disabled";
    }
    tipoEnvioEnDetalles = tipoEnvio;

    //horas y dias
    var val_DiasRecepcion = objPlantilla.ParametroDiaHora;  //Paso el mismo valor xq al pintarse solo uno lo hace (solo un plantcodi se muestra)
    var val_HoraCulminacion = objPlantilla.ParametroDiaHora;  //Paso el mismo valor xq al pintarse solo uno lo hace (solo un plantcodi se muestra)

    //Formato 
    var val_To = objPlantilla.Planticorreos != null ? objPlantilla.Planticorreos : "";
    var val_CC = objPlantilla.PlanticorreosCc != null ? objPlantilla.PlanticorreosCc : "";
    var val_BCC = objPlantilla.PlanticorreosBcc != null ? objPlantilla.PlanticorreosBcc : "";
    var val_Asunto = objPlantilla.Plantasunto != null ? objPlantilla.Plantasunto : "";
    var val_Contenido = objPlantilla.Plantcontenido != null ? objPlantilla.Plantcontenido : "";

    //campo hora y estado
    var val_Hora = objPlantilla.Hora != null ? objPlantilla.Hora : "";
    var val_Estado = objPlantilla.EstadoRecordatorio != null ? objPlantilla.EstadoRecordatorio : "";
    var seleccionA = "";
    var seleccionI = "";
    if (val_Estado == "A")
        seleccionA = "selected";
    if (val_Estado == "I")
        seleccionI = "selected";

    //campo De
    var val_De = "";
    if (tipoEnvio == ENVIO_AUTOMATICO)
        val_De = "webapp@coes.org.pe";
    if (tipoEnvio == ENVIO_MANUAL)
        val_De = objPlantilla.PlanticorreoFrom != null ? objPlantilla.PlanticorreoFrom : "";



    var cadena = '';

    //Agrego boton para ejecutar recordatorios
    if (tipoCorreo == RECORDATORIO) {
        if (plantillacodi == RECORDATORIO_REVISAR_INFORMACION_RECIBIDA_CE || plantillacodi == RECORDATORIO_REVISAR_INFORMACION_RECIBIDA_CN ||
            plantillacodi == RECORDATORIO_REVISAR_SUBSANACIONES_PRESENTADAS_CE || plantillacodi == RECORDATORIO_REVISAR_SUBSANACIONES_PRESENTADAS_CN) {
            cadena += `
            <fieldset style="margin-bottom: 10px; padding-bottom: revert;">
                <legend>Notificación Manual</legend>
                Recordatorio automático: Se ejecuta cada dia a las ${val_Hora} horas. <b>Presionar botón ejecutar para enviar recordatorio de forma manual. </b>
                <input style="float: right;" type="button" id="btnEjecutar" value="Ejecutar"  onclick="ejecutarRecordatorio( ${plantillacodi});"/>
            
            </fieldset>
            `;
        }
        else { //Recordatorio para informar del vencimiento de plazo para subsanar 
            cadena += `
            <fieldset style="margin-bottom: 10px; padding-bottom: revert;">
                <legend>Notificación Manual</legend>
                Recordatorio automático: Se ejecuta cada hora. <b>Presionar botón ejecutar para enviar recordatorio de forma manual. </b>
                <input style="float: right;" type="button" id="btnEjecutar" value="Ejecutar"  onclick="ejecutarRecordatorio( ${plantillacodi});"/>
            
            </fieldset>
            `;
        }
    } else {

        //Tambien agrego boton para ejecutar recordatorios de alguns notificaciones
        if (plantillacodi == NOTIFICACION_IMCUMPLIMIENTO_ENTREGA_INFO_CE || 
            plantillacodi == NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_CE || plantillacodi == NOTIFICACION_CULMINACION_PLAZO_SUBSANAR_CN) {
            cadena += `
        <fieldset style="margin-bottom: 10px; padding-bottom: revert;">
            <legend>Notificación Manual</legend>
            Notificación automática. <b>Presionar botón ejecutar para enviar recordatorio de forma manual. </b>
            <input style="float: right;" type="button" id="btnEjecutar" value="Ejecutar"  onclick="ejecutarRecordatorio( ${plantillacodi});"/>
            
        </fieldset>
        `;
        }
    }


    //Agrego nombre del correo
    cadena += `
    <div id="informacionPlantilla" style="height:20px; padding-top: 8px; padding-bottom: 12px;" >
        <div class="tbform-label" style="width:120px; float: left; font-size:18px;">
            Plantilla:
        </div>
        <div id="valor" style="float: left; font-size:18px;">
            ${objPlantilla.Plantcodi} - ${objPlantilla.Plantnomb}
        </div>
    </div>
    <table style="width:100%">
    `;

    //Agrego un fila para gestionar hora de envio de recordatorios
    if (tipoCorreo == RECORDATORIO) {
        if (objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANAR_CE || objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANAR_CN) {
            cadena += `
                    <tr> 
                        <td class="tbform-label">Habilitación:</td>
                        <td class="registro-control" style="width:1100px;">                            
                            <select id="cbEstadoR" style="width: 115px;" ${habilitacion} value="" >
                                <option value="A" ${seleccionA}>Activo</option>
                                <option value="I" ${seleccionI}>Inactivo</option>
                            </select>
                        </td>
                    </tr>
            `;
        } else {
            cadena += `
                    <tr> 
                        <td class="tbform-label">Hora de Ejecución y habilitación:</td>
                        <td class="registro-control" style="width:1100px;">
                            <input type="text" name="Hora" id="HoraR" value="${val_Hora}"  style="width:65px;" ${habilitacion} title="Permite editar hora de los correos automáticos de recordatorios."/>
                            (hh:mm)
                            <select id="cbEstadoR" style="width: 115px; margin-left: 25px;" ${habilitacion} value="I" >
                                <option value="A" ${seleccionA}>Activo</option>
                                <option value="I" ${seleccionI}>Inactivo</option>
                            </select>
                        </td>
                    </tr>
            `;
        }
    }

    //Agrego un fila para gestionar parametros (horas/dias)
    if (tipoCorreo == RECORDATORIO) {
        if (objPlantilla.Plantcodi == RECORDATORIO_REVISAR_INFORMACION_RECIBIDA_CE || objPlantilla.Plantcodi == RECORDATORIO_REVISAR_INFORMACION_RECIBIDA_CN ||
            objPlantilla.Plantcodi == RECORDATORIO_REVISAR_SUBSANACIONES_PRESENTADAS_CE || objPlantilla.Plantcodi == RECORDATORIO_REVISAR_SUBSANACIONES_PRESENTADAS_CN ) {
            cadena += `
                    <tr> 
                        <td class="tbform-label">Días Recepción:</td>
                        <td class="registro-control" style="width:1100px;">
                            <input type="number" name="Hora" id="parametroDH" value="${val_DiasRecepcion}"  style="width:35px;" ${habilitacion} title="Permite editar la cantidad de días recidbidos." onkeypress="return validarNumeroEntero(this,event)" />
                            (días)                           
                        </td>
                    </tr>
            `;
        } else {
            if (objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANAR_CE || objPlantilla.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANAR_CN ) {
                cadena += `
                    <tr> 
                        <td class="tbform-label">Horas culminación:</td>
                        <td class="registro-control" style="width:1100px;">
                            <input type="number" name="Hora" id="parametroDH" value="${val_HoraCulminacion}"  style="width:35px;" ${habilitacion} title="Permite editar la cantidad de horas del plazo de culminación." onkeypress="return validarNumeroEntero(this,event)" />
                            (horas)
                        </td>
                    </tr>
                `;
            }
        }
    }

    cadena += `
                    <tr>
                        <td style="width:120px" class="registro-label"><input type="hidden" id="hfIdPlantillaCorreo" value="${objPlantilla.Plantcodi}"/></td>
                    </tr>
                    <tr>
                        <td style="width:120px" class="tbform-label">De:</td>
    `;
 
    

    //Habilito y deshabilito el campo De y Para
    if (tipoEnvio == ENVIO_AUTOMATICO) {
        cadena += `
                        <td class="registro-control" style="width:1100px;"><input type="text" name="From" id="From" value="${val_De}" maxlength="900" style="width:1090px;" disabled /></td>
                    </tr>
        `;        
         cadena += `
                    <tr>
                        <td class="tbform-label">Para (*):</td>
                        <td class="registro-control" style="width:1100px;"><input type="text" name="To" id="To" value="${val_To}" maxlength="100" style="width:1090px;" onfocus="activarBoton('botonAddPropPara');"  onblur="desactivarBoton('botonAddPropPara','To');" ${habilitacion}/></td>
                        <td class="registro-label"><input type="button" id="botonAddPropPara" value="Agregar variable" onclick="abrirListadoVariables( ${plantillacodi}, '${VARIABLE_PARA}');" style="visibility:hidden"  ></td>
                    </tr>
        `;
        

    } else {
        if (tipoEnvio == ENVIO_MANUAL) {
            cadena += `
                        <td class="registro-control" style="width:1100px;"><input type="text" name="From" id="From" value="${val_De}" maxlength="900" style="width:1090px;" ${habilitacion} /></td>
                    </tr>
                    <tr>
                        <td class="tbform-label">Para (*):</td>
                        <td class="registro-control" style="width:1100px;"><input type="text" name="To" id="To" value="${val_To}" maxlength="100" style="width:1090px;" ${habilitacion}/></td>
                    </tr>
        `;
        }
    }

    //Otros (CC, BCC, ASUNTO y CONTENIDO)
    cadena += `                
            
            <tr>
                <td class="tbform-label">CC:</td>
                <td class="registro-control" style="width:1100px;"><input name="CC" id="CC" type="text" value="${val_CC}" maxlength="120" style="width:1090px;"  onfocus="activarBoton('botonAddPropCC');"  onblur="desactivarBoton('botonAddPropCC','CC');" ${habilitacion}/></td>
                <td class="registro-label"><input type="button" id="botonAddPropCC" value="Agregar variable" onclick="abrirListadoVariables( ${plantillacodi}, '${VARIABLE_CC}');" style="visibility:hidden"  ></td>
            </tr>
            <tr>
                <td class="tbform-label"> BCC:</td>
                <td class="registro-control" style="width:1100px;"><input name="BCC" id="BCC" type="text" value="${val_BCC}" maxlength="120" style="width:1090px;" ${habilitacion}/></td>
            </tr>
            <tr>
                <td class="tbform-label">Asunto (*):</td>
                <td class="registro-control" style="width:1100px;">
                    <textarea maxlength="450" name="Asunto" id="Asuntos" value=""  cols="" rows="3" style="resize: none;width:1090px;" onfocus="activarBoton('botonAddPropAsunto');"  onblur="desactivarBoton('botonAddPropAsunto','Asuntos');" ${habilitacion}>${val_Asunto}</textarea>
                </td>
                <td class="registro-label"><input type="button" id="botonAddPropAsunto" value="Agregar variable" onclick="abrirListadoVariables( ${plantillacodi}, '${VARIABLE_ASUNTO}');" style="visibility:hidden"  ></td>
            </tr>
            <tr>
                <td class="tbform-label"> Contenido (*):</td>
                <td class="registro-control" style="width:1100px;">
                    <textarea name="Contenido" id="Contenido" maxlength="2000" cols="180" rows="22"  ${habilitacion}>
                        ${val_Contenido}
                    </textarea>
                    (*): Campos obligatorios.
                </td>
                <td class="registro-label">
                    
                </td>
            </tr>
    `;

    if (esEditable) {
        cadena += `

            <tr>
                
                <td colspan="3" style="padding-top: 2px; text-align: center;">
                    <input type="button" id="btnGuardarPC" value="Guardar" />
                    
                </td>
            </tr>

        `;
    }

    cadena += `
    </table>    
    `;   

    return cadena;
}

function iniciarControlTexto (id, listaVariables, sololectura) {

    tinymce.remove();

    tinymce.init({
        selector: '#' + id,
        plugins: [
            //'paste textcolor colorpicker textpattern link table image imagetools preview'
            'wordcount advlist anchor autolink codesample colorpicker contextmenu fullscreen image imagetools lists link media noneditable preview  searchreplace table template textcolor visualblocks wordcount'
        ],
        toolbar1:
            //'insertfile undo redo | fontsizeselect bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link | table | image | mybutton | preview',
            'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | image | mybutton | preview',
        
        menubar: false,
        readonly: sololectura ? 1 : 0,
        language: 'es',
        statusbar: false,
        convert_urls: false,
        plugin_preview_width: 1000,        
        setup: function (editor) {
            editor.on('change',
                function () {
                    editor.save();
                });            
            editor.addButton('mybutton', {
                type: 'menubutton',
                text: 'Agregar Variables',
                tooltip: "Ingrese una variable",
                icon: false,
                menu: llenarMenus(editor, listaVariables)                
            });
            
        }
    });

}

function llenarMenus(e, lista) {
    var listaObj = [];
    var numVariables = lista.length;

    if (numVariables >= 1) {
        var regObj = {};
        var regVariable = lista[0];
        regObj.text = regVariable.Nombre;
        regObj.onclick = function () { e.insertContent(regVariable.Valor); };
        listaObj.push(regObj);

        if (numVariables >= 2) {
            regObj = {};
            var regVariable1 = lista[1];
            regObj.text = regVariable1.Nombre;
            regObj.onclick = function () { e.insertContent(regVariable1.Valor); };
            listaObj.push(regObj);

            if (numVariables >= 3) {
                regObj = {};
                var regVariable2 = lista[2];
                regObj.text = regVariable2.Nombre;
                regObj.onclick = function () { e.insertContent(regVariable2.Valor); };
                listaObj.push(regObj);

                if (numVariables >= 4) {
                    regObj = {};
                    var regVariable3 = lista[3];
                    regObj.text = regVariable3.Nombre;
                    regObj.onclick = function () { e.insertContent(regVariable3.Valor); };
                    listaObj.push(regObj);

                    if (numVariables >= 5) {
                        regObj = {};
                        var regVariable4 = lista[4];
                        regObj.text = regVariable4.Nombre;
                        regObj.onclick = function () { e.insertContent(regVariable4.Valor); };
                        listaObj.push(regObj);

                        if (numVariables >= 6) {
                            regObj = {};
                            var regVariable5 = lista[5];
                            regObj.text = regVariable5.Nombre;
                            regObj.onclick = function () { e.insertContent(regVariable5.Valor); };
                            listaObj.push(regObj);

                            if (numVariables >= 7) {
                                regObj = {};
                                var regVariable6 = lista[6];
                                regObj.text = regVariable6.Nombre;
                                regObj.onclick = function () { e.insertContent(regVariable6.Valor); };
                                listaObj.push(regObj);

                                if (numVariables >= 8) {
                                    regObj = {};
                                    var regVariable7 = lista[7];
                                    regObj.text = regVariable7.Nombre;
                                    regObj.onclick = function () { e.insertContent(regVariable7.Valor); };
                                    listaObj.push(regObj);

                                    if (numVariables >= 9) {
                                        regObj = {};
                                        var regVariable8 = lista[8];
                                        regObj.text = regVariable8.Nombre;
                                        regObj.onclick = function () { e.insertContent(regVariable8.Valor); };
                                        listaObj.push(regObj);

                                        if (numVariables >= 10) {
                                            regObj = {};
                                            var regVariable9 = lista[9];
                                            regObj.text = regVariable9.Nombre;
                                            regObj.onclick = function () { e.insertContent(regVariable9.Valor); };
                                            listaObj.push(regObj);

                                            if (numVariables >= 11) {
                                                regObj = {};
                                                var regVariable10 = lista[10];
                                                regObj.text = regVariable10.Nombre;
                                                regObj.onclick = function () { e.insertContent(regVariable10.Valor); };
                                                listaObj.push(regObj);

                                                if (numVariables >= 12) {
                                                    regObj = {};
                                                    var regVariable11 = lista[11];
                                                    regObj.text = regVariable11.Nombre;
                                                    regObj.onclick = function () { e.insertContent(regVariable11.Valor); };
                                                    listaObj.push(regObj);

                                                    if (numVariables >= 13) {
                                                        regObj = {};
                                                        var regVariable12 = lista[12];
                                                        regObj.text = regVariable12.Nombre;
                                                        regObj.onclick = function () { e.insertContent(regVariable12.Valor); };
                                                        listaObj.push(regObj);

                                                        if (numVariables >= 14) {
                                                            regObj = {};
                                                            var regVariable13 = lista[13];
                                                            regObj.text = regVariable13.Nombre;
                                                            regObj.onclick = function () { e.insertContent(regVariable13.Valor); };
                                                            listaObj.push(regObj);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    return listaObj;
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function activarBoton(idBoton) {
    $("#" + idBoton).css("visibility", "visible");    
}

async function desactivarBoton(idBoton, idCaja) {
    await sleep(800);
        
    var caja = document.getElementById(idCaja);

    if (document.activeElement != caja) {
        $("#" + idBoton).css("visibility", "hidden");       
    }            
}


function abrirListadoVariables(idPlantilla, tipoCampo) { 
    
    $.ajax({
        type: 'POST',
        url: controlador + 'listarVariables',
        data: {
            idPlantilla: idPlantilla,
            campo: tipoCampo
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlVariables = dibujarListadoVariables(evt.ListaVariables);
                $("#seccionListadoVariables").html(htmlVariables);

                abrirPopup("popupAgregarVariable");

                $('#btnAceptarVar').click(function () {
                    if (tipoCampo == VARIABLE_ASUNTO)
                        agregarTextoA($("#cbVariable").val(), "Asuntos");
                    if (tipoCampo == VARIABLE_CC)
                        agregarTextoA($("#cbVariable").val(), "CC");
                    if (tipoCampo == VARIABLE_PARA)
                        agregarTextoA($("#cbVariable").val(), "To");

                    cerrarPopup('popupAgregarVariable');
                });

                $('#btnCancelarVar').click(function () {
                    cerrarPopup('popupAgregarVariable');
                });
                
            }
            else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function dibujarListadoVariables(listaVariables) {

    var cadena = '';
    cadena += `
    <table border="0"  id="tablaCargos">       
        <tr>
            <td>Variable :</td>
            <td>
                <select id="cbVariable" >
    `;

    for (key in listaVariables) {
        var item = listaVariables[key];
        cadena += `            
                    <option value="${item.Valor}">${item.Nombre}</option>
        `;
    }

    cadena += `            
                </select>
            </td>
         </tr >
         <tr style="text-align: center;">
                <td colspan="2" style="padding-top: 35px;">
                    <input type="button" id="btnAceptarVar" value="Agregar" />
                    <input type="button" id="btnCancelarVar" value="Cancelar" />
                </td>
            </tr>
    </table >
    `;

    return cadena;
}

function agregarTextoA(textoAIngresar, idCaja) {
    var caja = document.getElementById(idCaja);
    var inicio = caja.selectionStart;
    var fin = caja.selectionEnd;
    var textoAgregado = textoAIngresar;

    var texto = caja.value;
    var textIni = texto.substring(0, inicio);
    var textFin = texto.substring(inicio);
    var nuevoTexto = textIni + textoAgregado + textFin;
    $('#' + idCaja).val(nuevoTexto);

    caja.focus();
}

function guardarPlantilla() {
    limpiarBarraMensaje("mensaje");
    var correo = {};
    correo = getPlantillaCorreo();

    var msg = validarCamposCorreoAGuardar(correo);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarPlantillaCorreo',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({
                plantillaCorreo: correo,
            }
            ),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    cerrarPopup("popupEditarCentral");

                    //muestra el listado respectivo
                    $("#cbTipoPlantilla").val(tipoPlantillaEnListado);
                    $('#cbTipoCentral').val(tipoCentralEnListado);
                    $('#tab-container').easytabs('select', '#tabLista');
                    mostrarMensaje('mensaje', 'exito', "Los cambios fueron guardados exitosamente...");
                    mostrarListadoPlantilla();
                    $("#div_detalle").html("");

                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function getPlantillaCorreo() {
    var obj = {};

    

    if (tipoEnvioEnDetalles == ENVIO_AUTOMATICO) {        
        obj.Planticorreos = $("#To").val();
    } else {
        if (tipoEnvioEnDetalles == ENVIO_MANUAL) {
            obj.Planticorreos = $("#To").val();
        }
    }

    //Hora y Estado
    obj.Hora = "";
    obj.Estado = "";
    if (tipoPlantillaEnListado == RECORDATORIO) {
        obj.Hora = $("#HoraR").val();
        obj.EstadoRecordatorio = $("#cbEstadoR").val();
    }

    //Parametros Dia y hora (recordatorios)
    obj.ParametroDiaHora = "";
    if (tipoPlantillaEnListado == RECORDATORIO) {
        obj.ParametroDiaHora = $("#parametroDH").val();
    }

    //otros
    obj.Plantcodi = $("#hfIdPlantillaCorreo").val();
    obj.Plantcontenido = $("#Contenido").val();
    obj.Plantasunto = $("#Asuntos").val();        
    obj.PlanticorreosCc = $("#CC").val();
    obj.PlanticorreosBcc = $("#BCC").val();
    obj.PlanticorreoFrom = $("#From").val();        

    return obj;
}

function validarCamposCorreoAGuardar(correo) {
    var msj = "";

    /*********************************************** validacion del campo CC ***********************************************/    
    var validaTo = 0;
    
    if ($('#To').val().trim() == "") {
        msj += "<p>Error encontrado en el campo Para. Debe ingresar remitente.</p>";
    }

    validaTo = validarCorreo($('#To').val(), 0, -1, "To");
    if (validaTo < 0) {
        msj += "<p>Error encontrado en el campo Para. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }
    var para_ = correo.Planticorreos.trim();
    var tienenMismaCantidadPara = validarCantidadDeSeparadores(para_);
    if (!tienenMismaCantidadPara) {
        msj += "<p>Revisar campo Para, la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
    }
    //valido que las palabras dentro de un {} sean variables admitidas
    msj = validarVariablesCorrectas(para_, "To", lstVariablesPara, msj);

    //valido los (;) antes de los ({)
    msj = validarSeparacionConectores(para_, "To", msj);

    //valido cantidad de (;)
    msj = validarNumeroPuntoYComas(para_, "To", msj);

    

    /*********************************************** validacion del campo CC ***********************************************/
    var validaCc = validarCorreo($('#CC').val(), 0, -1, "Cc");
    if (validaCc < 0) {
        msj += "<p>Error encontrado en el campo CC. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }
    var micc_ = correo.PlanticorreosCc.trim();
    var tienenMismaCantidadCc = validarCantidadDeSeparadores(micc_);
    if (!tienenMismaCantidadCc) {
        msj += "<p>Revisar campo CC, la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
    }
    //valido que las palabras dentro de un {} sean variables admitidas
    msj = validarVariablesCorrectas(micc_, "CC", lstVariablesCC, msj);

    //valido los (;) antes de los ({)
    msj = validarSeparacionConectores(micc_, "CC", msj);

    //valido cantidad de (;)
    msj = validarNumeroPuntoYComas(micc_, "CC", msj);


    /*********************************************** validacion del campo BCC ***********************************************/
    var validaBcc = validarCorreo($('#BCC').val(), 0, -1, "Bcc");
    if (validaBcc < 0) {
        msj += "<p>Error encontrado en el campo BCC. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }


    /********************************************** validacion del campo Asunto ***********************************************/
    var asunto_ = correo.Plantasunto.trim();
    if (asunto_ == "") {
        msj += "<p>Error encontrado en el campo Asunto. Debe ingresar Asunto.</p>";
    }
    
    var tienenMismaCantidadAsunto = validarCantidadDeSeparadores(asunto_);
       
    if (!tienenMismaCantidadAsunto) {
        msj += "<p>Revisar campo Asunto, la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
    }

    //valido que las palabras dentro de un {} sean variables admitidas
    msj = validarVariablesCorrectas(asunto_, "Asunto", lstVaraiablesCorreo, msj);  




    /*********************************************** validacion del campo Contenido ***********************************************/

    var contenido_ = correo.Plantcontenido.trim();
    if (contenido_ == "") {
        msj += "<p>Ingrese Contenido.</p>";
    }  
    var tienenMismaCantidadContenido = validarCantidadDeSeparadores(contenido_);

    
    if (!tienenMismaCantidadContenido) {
        msj += "<p>Revisar campo Contenido, la cantidad de caracteres '{' no coincide con la cantidad de caracteres '}'. </p>";
    }

    //valido que las palabras dentro de un {} sean variables admitidas   
    msj = validarVariablesCorrectas(contenido_, "Contenido", lstVaraiablesCorreo, msj);
    


    /**************************************** validacion del campo Hora y Estado para recordatorios ****************************************/    
    if (tipoPlantillaEnListado == RECORDATORIO) {
        if (correo.Plantcodi == RECORDATORIO_REVISAR_INFORMACION_RECIBIDA_CE || correo.Plantcodi == RECORDATORIO_REVISAR_INFORMACION_RECIBIDA_CN ||
            correo.Plantcodi == RECORDATORIO_REVISAR_SUBSANACIONES_PRESENTADAS_CE || correo.Plantcodi == RECORDATORIO_REVISAR_SUBSANACIONES_PRESENTADAS_CN) {
        
            if (correo.Hora.trim() == "") {
                msj += "<p>Revisar campo Hora. Ingreseve valor.</p>";
            }
            
        }
        if (correo.EstadoRecordatorio.trim() == "") {
            msj += "<p>Revisar campo Estado. Escoga un valor.</p>";
        }        
    }



    /*********************************************** validacion parametros (dias/horas) ***********************************************/    
    if (tipoPlantillaEnListado == RECORDATORIO) {
       
            //verifico si se esta usando la variable
        let resultadoHora1 = (contenido_).includes(ValHorasCulminacion);
        let resultadoHora2 = (asunto_).includes(ValHorasCulminacion);
        let resultadoDia1 = (contenido_).includes(ValDiasRecepcion);
        let resultadoDia2 = (asunto_).includes(ValDiasRecepcion);

        if (resultadoHora1 || resultadoHora2 || resultadoDia1 || resultadoDia2) {
        
            var mival = "";
            if (correo.Plantcodi == RECORDATORIO_REVISAR_INFORMACION_RECIBIDA_CE || correo.Plantcodi == RECORDATORIO_REVISAR_INFORMACION_RECIBIDA_CN ||
                correo.Plantcodi == RECORDATORIO_REVISAR_SUBSANACIONES_PRESENTADAS_CE || correo.Plantcodi == RECORDATORIO_REVISAR_SUBSANACIONES_PRESENTADAS_CN) {
                mival = "Dias Recepción";
            }

            if (correo.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANAR_CE || correo.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANAR_CN) {
                mival = "Horas Culminación";
            }

            if (correo.ParametroDiaHora.trim() == "") {
                msj += "<p>Revisar campo " + mival + ". Ingrese valor.</p>";
            }

            
        } else {
            if (parseInt(correo.ParametroDiaHora.trim()) < 0) {
                if (correo.Plantcodi == RECORDATORIO_REVISAR_INFORMACION_RECIBIDA_CE || correo.Plantcodi == RECORDATORIO_REVISAR_INFORMACION_RECIBIDA_CN ||
                    correo.Plantcodi == RECORDATORIO_REVISAR_SUBSANACIONES_PRESENTADAS_CE || correo.Plantcodi == RECORDATORIO_REVISAR_SUBSANACIONES_PRESENTADAS_CN) {
                    msj += "<p>El valor de Dias de repección debe ser positivo.</p>";
                }

                if (correo.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANAR_CE || correo.Plantcodi == RECORDATORIO_VENCIMIENTO_PLAZO_SUBSANAR_CN) {
                    msj += "<p>El valor de Horas de culminación debe ser positivo.</p>";
                }
            }
        }
    }
        
    return msj;
}

function borrarVacios(textoConVacios) {
    let textoSinVacios = textoConVacios.replace(/ /g, "");
    return textoSinVacios;
}

function validarCantidadDeSeparadores(campo) {
    
    //valido que la cantidad de { sea igual al de }
    let regexIni = new RegExp(SEPARADOR_INI_VARIABLE, 'g')
    let regexFin = new RegExp(SEPARADOR_FIN_VARIABLE, 'g')
    /*const regex = /{/g;*/
    const lstSeparadorIniCampo = campo.match(regexIni);
    const lstSeparadorFinCampo = campo.match(regexFin);

    var tienenMismaCantidad = false;
    if (lstSeparadorIniCampo != null) {
        if (lstSeparadorFinCampo != null) {
            if (lstSeparadorIniCampo.length == lstSeparadorFinCampo.length)
                tienenMismaCantidad = true;
        } else {
            tienenMismaCantidad = false;
        }
    } else {
        if (lstSeparadorFinCampo != null) {
            tienenMismaCantidad = false;
        } else {
            tienenMismaCantidad = true;
        }
    }

    return tienenMismaCantidad;
}

function validarNumeroPuntoYComas(valorCampo, campo, msj) {
    var cadena = valorCampo;

    if (cadena != "") {

        //para el campo Para quito la variable (ValTodosAgentes = "{TODOS_AGENTES_DE_EMPRESA}", ValAgenteUltimo = "{AGENTE_ULTIMO_EVENTO}") si hubiese   
        if (campo == "To") {
            cadena = cadena.replace(ValTodosAgentes, "").replace(ValAgenteUltimo, "");
        }

        //para el campo Cc quito la variable (ValOtrosAgentes = "{OTROS_AGENTES_DE_EMPRESA}", ValTodosAgentes = "{TODOS_AGENTES_DE_EMPRESA}", ValAgenteUltimo = "{AGENTE_ULTIMO_EVENTO}") si hubiese   
        if (campo == "Cc") {
            cadena = cadena.replace(ValOtrosAgentes, "").replace(ValTodosAgentes, "").replace(ValAgenteUltimo, "");
        }

        var arreglo = cadena.split(';');
        var nroCorreo = 0;
        var longitud = arreglo.length;

        if (longitud == 0) {
            arreglo = cadena;
            longitud = 1;
        }

        for (var i = 0; i < longitud; i++) {

            var email = arreglo[i].trim();
            var validacion = validarDirecccionCorreo(email);

            if (validacion) {
                nroCorreo++;
            } 

        }

        //si hay correos valido la cantidad de  ;
        if (nroCorreo > 0) {
            //num de punto y comas
            const regex = /;/g;
            const lstTotalPuntoYComas = valorCampo.match(regex);

            //num de llaves
            const regex2 = /{/g;
            const lstTotalInicioConectores = valorCampo.match(regex2);

            //solo valido si hay variables
            if (lstTotalInicioConectores != null) {
                if (lstTotalPuntoYComas != null) {
                    if (lstTotalInicioConectores.length != lstTotalPuntoYComas.length) {
                        if (campo == "To")
                            campo = "Para";
                        msj += "<p>Revisar campo " + campo + ". Se detectó que el número de punto y comas (;) es incorrecto. </p>";
                    }
                } else {
                    if (campo == "To")
                        campo = "Para";
                    msj += "<p>Revisar campo " + campo + ". Se detectó que el número de punto y comas (;) es incorrectoo. </p>";
                }
            } 
        }                        

    }
    return msj;
}


function validarSeparacionConectores(valorCampo, campo, msj) { 
    //validando que no haya {var1}{var2} (sin ; entre ellos)

    //quito los vacios
    var valorSinVacios = borrarVacios(valorCampo);
    const regex = /}\w*{/g;
    const lstTotalLlavesSinPuntoYComa = valorSinVacios.match(regex);
    if (lstTotalLlavesSinPuntoYComa != null) {
        if (lstTotalLlavesSinPuntoYComa.length > 0) {
            if (campo == "To")
                campo = "Para";
            msj += "<p>Revisar campo " + campo + ". Se detectó la omisión de (;) entre dos variables continuas. </p>";
        }
    }    

    return msj;
}

function validarVariablesCorrectas(valorCampo, campo, lstVariables, msj) {
    //validando textos tipo: {texto}
    const regex = /{\w+}/g;
    const lstPalabrasDentroParentesis = valorCampo.match(regex);
    if (lstPalabrasDentroParentesis != null) {
        let lstDiferentes = lstPalabrasDentroParentesis.filter(x => !lstVariables.includes(x));
        if (lstDiferentes.length > 0) {
            if (campo == "To")
                campo = "Para";
            msj += "<p>Revisar campo " + campo + ". Se detectó variables ( texto dentro de {} ) no reconocidas. </p>";
        }
    }

    //validando textos tipo: {}
    const regex2 = /{}/g;
    const lstPalabrasSoloParentesis = valorCampo.match(regex2);
    if (lstPalabrasSoloParentesis != null) {        
        if (lstPalabrasSoloParentesis.length > 0) {
            if (campo == "To")
                campo = "Para";
            msj += "<p>Revisar campo " + campo + ". Se detectó texto ( {} ) no permitido. </p>";
        }
    }

    return msj;
}

function validarCorreo(valCadena, minimo, maximo, campo) {
    var cadena = valCadena;

    //para el campo Para quito la variable (ValTodosAgentes = "{TODOS_AGENTES_DE_EMPRESA}", ValAgenteUltimo = "{AGENTE_ULTIMO_EVENTO}") si hubiese   
    if (campo == "To") {
        cadena = valCadena.replace(ValTodosAgentes, "").replace(ValAgenteUltimo, "");
    }

    //para el campo Cc quito la variable (ValOtrosAgentes = "{OTROS_AGENTES_DE_EMPRESA}", ValTodosAgentes = "{TODOS_AGENTES_DE_EMPRESA}", ValAgenteUltimo = "{AGENTE_ULTIMO_EVENTO}") si hubiese   
    if (campo == "Cc") {
        cadena = valCadena.replace(ValOtrosAgentes, "").replace(ValTodosAgentes, "").replace(ValAgenteUltimo, "");
    }

    var arreglo = cadena.split(';');
    var nroCorreo = 0;
    var longitud = arreglo.length;

    if (longitud == 0) {
        arreglo = cadena;
        longitud = 1;
    }

    for (var i = 0; i < longitud; i++) {

        var email = arreglo[i].trim();
        var validacion = validarDirecccionCorreo(email);

        if (validacion) {
            nroCorreo++;
        } else {
            if (email != "")
                return -1;
        }

    }

    if (minimo > nroCorreo)
        return -1;

    if (maximo > 0 && nroCorreo > maximo)
        return -1;

    return 1;
}

function validarDirecccionCorreo(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function ejecutarRecordatorio(plantcodi) {
    limpiarBarraMensaje("mensaje");
    $.ajax({
        type: 'POST',
        url: controlador + 'EjecutarRecordatorio',
        data: {
            plantcodi: plantcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                mostrarMensaje('mensaje', 'exito', 'El proceso se ejecutó correctamente');
                
            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error...');
            
        }
    });
}

function validarNumeroEntero (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        return false;
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}


function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}


function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}