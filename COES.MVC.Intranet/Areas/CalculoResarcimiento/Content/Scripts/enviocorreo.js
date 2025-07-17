var controlador = siteRoot + 'calculoresarcimiento/Correo/';

var IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Mensaje" width="19" height="19" style="">';
var IMG_FILE = '<img src="' + siteRoot + 'Content/Images/file.png" alt="Archivo" width="16" height="16" style="float: right;">';
var IMG_LOADING = '<img src="' + siteRoot + 'Content/Images/loadingtree.gif" alt="" width="19" height="19" style="">';

const VER = 1;
const NUEVO = 2;

const SolEnvioInterrupcionesTrimestral = "174";
const SolEnvioInterrupcionesSemestral = "175";
const SolEnvioObservacionesAInterrupciones = "176";
const SolEnvioRespuestasAInterrupciones = "177";
const SolDecisionesControversia = "178";
const SolEnvíoCompensacionesMalaCalidadProducto = "179";
const SolEnvioInterrupcionesPendientesReportar = "320";

var repIntPendienteAdjuntado = 1;
var repIntPendienteArchOriginal = 1;

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').easytabs('select', '#tabLista');    

    $('#txtAnioDesde').Zebra_DatePicker({
        format: 'Y',
        pair: $('#txtAnioHasta'),
        direction: -1,
    });

    $('#txtAnioHasta').Zebra_DatePicker({
        format: 'Y',
        pair: $('#txtAnioDesde'),
        direction: 1,
    });

    

    $('#btnConsultar').on("click", function () {
        cargarListaCorreos();
    });

    $('#btnNuevoMensaje').click(function () {
        mostrarDetalle(null, NUEVO);
    });

    cargarListaCorreos();

});

function cargarPeriodos(anio) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ObtenerPeriodos',
        data: {
            anio: anio
        },
        dataType: 'json',
        global: false,
        success: function (result) { 
            if (result != -1) {
                $('#cbPeriodo').get(0).options.length = 0;
                $('#cbPeriodo').get(0).options[0] = new Option("--SELECCIONE--", "-1");
                $.each(result, function (i, item) {
                    $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Repernombre, item.Repercodi);
                });                
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function cargarListaCorreos() {
    limpiarBarraMensaje("mensaje");

    var datosC = {};
    datosC = getDatosConsulta();

    var msg = validarCamposConsulta(datosC);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + "listarCorreos",
            data: {
                strTipoCorreo: datosC.Tipo,
                anioDesde: datosC.Desde,
                anioHasta: datosC.Hasta
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    var htmlListadoCorreos = dibujarTablaCorreos(evt.ListadoCorreosEnviados);
                    $("#div_listado").html(htmlListadoCorreos);

                    $('#tablaCorreos').dataTable({
                        "scrollY": 430,
                        "scrollX": false,
                        "sDom": 'ft',
                        "ordering": true,
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
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }

}

function getDatosConsulta() {
    var obj = {};

    obj.Tipo = $("#cbTipoCorreo").val();
    obj.Desde = parseInt($("#txtAnioDesde").val()) || 0;
    obj.Hasta = parseInt($("#txtAnioHasta").val()) || 0;

    return obj;
}


function validarCamposConsulta(datos) {
    var msj = "";

    if (datos.Desde == 0) {
        msj += "<p>Debe escoger un año inicial correcto.</p>";
    }

    if (datos.Hasta == 0) {
        msj += "<p>Debe escoger un año final correcto.</p>";
    }

    if (datos.Hasta < datos.Desde) {
        msj += "<p>Debe escoger un rango correcto. El año final debe ser mayor o igual al año inicial.</p>";
    }

    return msj;
}

function dibujarTablaCorreos(listaCorreos) {
    var num = 0;

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaCorreos" >
       <thead>
           <tr style="height: 22px;">
                <th style='width:50px;'>Acciones</th>
                <th>Id Correo</th>
                <th>Periodo</th>
                <th>Tipo de Correo</th>
                <th>Dirigido a</th>
                <th>Usuario quien envío</th>
                <th>Fecha de Envío</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in listaCorreos) {
        var item = listaCorreos[key];
        num++;
        cadena += `
            <tr>
                <td style='width:50px;'>        
                    <a href="JavaScript:mostrarDetalle(${item.Relcorcodi},${VER});">${IMG_VER}</a>
                </td>
                <td>${item.Relcorcodi}</td>
                <td>${item.PeriodoDesc}</td>
                <td>${item.Tipo}</td>
                <td>${item.Emprnomb}</td>
                <td>${item.Relcorusucreacion}</td>
                <td>${item.RelcorfeccreacionDesc}</td>
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}

function mostrarDetalle(correoId, accion) {
    limpiarBarraMensaje("mensaje");

    //limpio el formulario de correo
    $("#div_detalle").html("");

    var id = correoId || 0;

        
    $('#tab-container').easytabs('select', '#tabDetalle');
    
    $.ajax({
        type: 'POST',
        url: controlador + "CargarDatosGeneralesCorreo",
        data: {
            accion: accion,
            relcorcodi: id            
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlFiltro = dibujarSeccionFiltro(evt, accion);
                $("#div_filtro").html(htmlFiltro);


                $('#txtAnio').Zebra_DatePicker({
                    format: 'Y',
                    onSelect: function (date) {
                        cargarPeriodos(date)

                        restablecerComboTipo();
                    }
                });

                

                $('#cbPeriodo').change(function () {
                    var seleccionado = $('#cbPeriodo').val();
                    if (seleccionado != "-1") {
                        verificarPeriodo(seleccionado);
                    }

                    restablecerComboTipo();
                    
                });


                $('#cbTipoCorreoEnvio').change(function () {                    
                    llenarEmpresas();
                    $("#div_detalle").html("");
                });


                $('#btnPrevisualizar').click(function () {
                    $("#div_detalle").html("");
                    var data = getDataFiltro();
                    var msg = validarCamposDatosFiltro(data);
                    if (msg == "") {
                        mostrarEstructuraDetalleCorreo(null, NUEVO);
                        
                        

                        //$.ajax({
                        //    success: function () {
                        //        //habilito/deshabilito edicion de contenido
                        //        var readonly = 1; //No será editable

                        //        //muestro editor de contenido
                        //        iniciarControlTexto('Contenido', evt.ListaVariables, readonly);
                        //    }
                        //});
                        
                    } else {
                        mostrarMensaje('mensaje', 'alert', msg);
                    }
                });

                if (accion == VER) {
                    cargarDataCabeceraYDeshabilitar(evt);
                    mostrarEstructuraDetalleCorreo(correoId, VER);
                }
                

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function cargarDataCabeceraYDeshabilitar(evento) {
    
    //Para algunos muestro los ocultados
    if (evento.IdPlantilla == parseInt(SolEnvioInterrupcionesTrimestral))
        $("#cmbTrim").css("display", "block");
    else
        $("#cmbTrim").css("display", "none");

    if (evento.IdPlantilla == parseInt(SolEnvioInterrupcionesSemestral))
        $("#cmbSem").css("display", "block");
    else
        $("#cmbSem").css("display", "none");

    //Muestro combo empresa
    $(".empresaBloque").css("display", "table-cell");
    
    //TituloEmpresa
    if (evento.IdPlantilla == parseInt(SolEnvioInterrupcionesTrimestral) || evento.IdPlantilla == parseInt(SolEnvioInterrupcionesSemestral) ||
        evento.IdPlantilla == parseInt(SolEnvioRespuestasAInterrupciones) || evento.IdPlantilla == parseInt(SolDecisionesControversia) ||
        evento.IdPlantilla == parseInt(SolEnvioInterrupcionesPendientesReportar)
    )
        $("#tituloEmpresa").html("Suministrador");
    if (evento.IdPlantilla == parseInt(SolEnvioObservacionesAInterrupciones))
        $("#tituloEmpresa").html("Responsable");
        

    document.getElementById("txtAnio").disabled = true;
    document.getElementById("cbPeriodo").disabled = true;
    document.getElementById("cbTipoCorreoEnvio").disabled = true;
    document.getElementById("cbEmpresa").disabled = true;

    $("#btnPrevisualizar").css("display", "none");

    $("#txtAnio").val(evento.Anio);
    $("#cbPeriodo").val(evento.Repercodi);
    $("#cbTipoCorreoEnvio").val(evento.IdPlantilla + "");
    //$("#cbEmpresa").val(evento.EmpresaId);
    $('#cbEmpresa').get(0).options.length = 0;
    $('#cbEmpresa').get(0).options[0] = new Option(evento.NombreEmpresa, "");

    //if(evento)
}

function restablecerComboTipo() {
    $("#div_detalle").html("");;
    $('#cbTipoCorreoEnvio').val("-1");    
    $(".empresaBloque").css("display", "none");    
}

function verificarPeriodo(val) {
    limpiarBarraMensaje("mensaje");
    $.ajax({
        type: 'POST',
        url: controlador + 'VerificarPeriodo',
        data: {
            repercodi: val
        },
        //dataType: 'json',
        //global: false,
        success: function (evt) {
            if (evt != "-1") {
                var esSemestral = evt.EsSemestral;

                if (esSemestral) {
                    $("#cmbSem").css("display", "block");
                    $("#cmbTrim").css("display", "none");
                }
                else {
                    $("#cmbSem").css("display", "none");
                    $("#cmbTrim").css("display", "block");
                }

            }
            else {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.' + evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function llenarEmpresas() {
    limpiarBarraMensaje("mensaje");

    var data = getDatosLlenado();
    var msg = validarCamposDatosLlenado(data);

    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'llenarEmpresas',
            data: {
                repercodi: data.PeriodoId,
                tipoCorreo: data.TipoCorreo
            },
            //dataType: 'json',
            //global: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    var empresas = evt.ListaEmpresas;
                    $("#hdNumEmpresas").val(empresas.length);
                    if (evt.MuestraEmpresa) {
                        $("#tituloEmpresa").html(evt.CampoEmpresa);
                        $(".empresaBloque").css("display", "table-cell");
                        $("#cbEmpresa").val("-1");
                    } else {
                        $(".empresaBloque").css("display", "none");
                    }
                    
                    //lleno las empresas
                    $('#cbEmpresa').get(0).options.length = 0;
                    $('#cbEmpresa').get(0).options[0] = new Option("--SELECCIONE--", "-1");
                    $.each(empresas, function (i, item) {
                        $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Emprnomb, item.Emprcodi);
                    });
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.' + evt.Mensaje);
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar empresas. ' + msg);
    }
}

function getDatosLlenado() {
    var obj = {};

    obj.PeriodoId = parseInt($("#cbPeriodo").val());
    obj.TipoCorreo = parseInt($("#cbTipoCorreoEnvio").val());

    return obj;
}


function validarCamposDatosLlenado(datos) {
    var msj = "";

    if (datos.PeriodoId == -1) {
        msj += "<p>Debe seleccionar un periodo correcto.</p>";
    }

    

    return msj;
}

function getDataFiltro() {
    var obj = {};

    //otros
    obj.Anio = parseInt($("#txtAnio").val());
    obj.PeriodoId = $("#cbPeriodo").val();
    obj.TipoId = $("#cbTipoCorreoEnvio").val();
    obj.EmpresaId = $("#cbEmpresa").val();    

    return obj;
}


function validarCamposDatosFiltro(data) {
    var msj = "";

    var numEmpresas = $("#hdNumEmpresas").val();
    if (data.PeriodoId == "-1") {
        msj += "<p>Seleccione un periodo correcto.</p>";
    }

    if (data.TipoId == "-1") {
        msj += "<p>Seleccione un tipo de correo correcto.</p>";
    }

    if (data.TipoId == SolEnvioObservacionesAInterrupciones) {
        if (numEmpresas > 0) {
            if (data.EmpresaId == "-1") {
                msj += "<p>Seleccione un responsable correcto.</p>";
            }
        } else {
            if (data.EmpresaId == "-1") {
                msj += "<p>No hay empresas responsables de interrupciones para el periodo seleccionado.</p>";
            }
        }
        
    } else {
        if (data.TipoId == SolEnvioRespuestasAInterrupciones) {
            if (numEmpresas > 0) {
                if (data.EmpresaId == "-1") {
                    msj += "<p>Seleccione un suministrador correcto.</p>";
                }
            } else {
                if (data.EmpresaId == "-1") {
                    msj += "<p>No hay empresas suministradoras que tengan observaciones de interrupciones de suministros por atender para el periodo seleccionado.</p>";
                }
            }
        } else {
            if (data.TipoId == SolDecisionesControversia) {
                if (numEmpresas > 0) {
                    if (data.EmpresaId == "-1") {
                        msj += "<p>Seleccione algún suministrador correcto.</p>";
                    }
                } else {
                    if (data.EmpresaId == "-1") {
                        msj += "<p>No hay empresas suministradoras con interrupciones donde la conformidad del responsable y suministrador tengan el valor de “No” para el periodo seleccionado.</p>";
                    }
                }
            } else {
                if (data.TipoId == SolEnvioInterrupcionesPendientesReportar) {
                    var suministrador = $("#cbEmpresa").val();
                    if (suministrador == "-1") {
                       
                            msj += "<p>Seleccione algún suministrador correcto.</p>";
                        
                    } 
                }
            }
        }
    }    


    return msj;
}

function dibujarSeccionFiltro(evento, accion) {

    var lstPeriodos = evento.ListaPeriodo;
    var lstResponsables = evento.ListaEmpresas;
    var cadena = '';

    //Agrego nombre del correo

    cadena += `
    `;
    cadena += `           
            <div class="search-content">
                    <table class="content-tabla-search" style="width:auto">
                        <tr>
                            <td>Año:</td>
                            <td>
                                <input type="text" id="txtAnio" style="width: 60px;" value="${evento.Anio}" />
                            </td>
                            <td style="padding: 3px 10px 3px 12px;">Seleccione periodo:</td>
                            <td>
                                <select id="cbPeriodo" style="width: 125px;">
                                    <option value="-1">--SELECCIONE--</option>
    `;

    Object.values(lstPeriodos).forEach(val => {
            cadena += `
                                    <option value="${val.Repercodi}">${val.Repernombre}</option>
            `;
    });

    cadena += `
                                    
                                </select>
                            </td>
                            <td style="padding: 3px 10px 3px 12px;">Tipo de correo:</td>
                            <td>
                                <select id="cbTipoCorreoEnvio" style="width:330px; ">
                                    <option value="-1">--SELECCIONE--</option>
                                    <option value="${SolDecisionesControversia}">Solicitud de envío de decisiones de controversias</option>
                                    <option value="${SolEnvioInterrupcionesPendientesReportar}">Solicitud de envío de interrupciones pendientes de reportar</option>
                                    <option id="cmbTrim" style="display: none;" value="${SolEnvioInterrupcionesTrimestral}">Solicitud de envío de interrupciones</option>
                                    <option id="cmbSem" style="display: none;" value="${SolEnvioInterrupcionesSemestral}">Solicitud de envío de interrupciones</option>
                                    <option value="${SolEnvioObservacionesAInterrupciones}">Solicitud de envío de observaciones a las interrupciones</option>
                                    <option value="${SolEnvioRespuestasAInterrupciones}">Solicitud de envío de respuestas a las observaciones</option>
                                    
                                </select>
                            </td>

                            <td class="empresaBloque" id="tituloEmpresa" style="padding: 3px 10px 3px 12px; display:none;">:</td>
                            <td class="empresaBloque" style="display:none;">
                                <select id="cbEmpresa" style="width: 200px;">
                                    <option value="-1" selected>-SELECCIONE-</option>
                                </select>
                            </td>

                            <td style="padding: 0px 10px 3px 20px;">
                                <input type="button" id="btnPrevisualizar" value="Previsualizar" />

                            </td>
                        </tr>
                    </table>
                </div>
    `;





    return cadena;
}


function mostrarEstructuraDetalleCorreo(correoId, accion) {
    limpiarBarraMensaje("mensaje");
    

    var id = correoId || 0;
    
    var data = getDataFiltro();

    $.ajax({
        type: 'POST',        
        url: controlador + "detallarCorreo",
        data: {
            idCorreo: id,
            anio: data.Anio,
            repercodi: data.PeriodoId,
            tipoCorreo: data.TipoId,
            strEmpresaId: data.EmpresaId,           
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                var htmlPlantilla = dibujarPlantilla(evt.Correo, accion, data.TipoId, data.EmpresaId, data.PeriodoId);
                $("#div_detalle").html(htmlPlantilla);


                $('#btnEnviarCorreo').click(function () {
                    enviarMensaje();
                });

                $.ajax({
                    success: function () {
                        //habilito/deshabilito edicion de contenido
                        var readonly = 1; //No será editable

                        //muestro editor de contenido
                        iniciarControlTexto('Contenido', evt.ListaVariables, readonly);                       
                        
                    }
                });

                var tipoCorreo = $("#cbTipoCorreoEnvio").val();
                if (tipoCorreo == SolEnvioInterrupcionesPendientesReportar) { 
                    repIntPendienteArchOriginal = 1;
                    cargaDeArchivo();
                    var msj = ""; 
                    var validaPara = validarCorreo($('#To').val(), 1, -1);
                    if (validaPara < 0) {
                        msj += "<p>Este suministrador no tiene correos registrados.</p>";
                    }

                    if (msj != "") {
                        setTimeout(function () {
                            mostrarMensaje('mensaje', 'alert', msj);
                        }, 1000);
                        
                    } 
                }
                

            } else {
                mostrarMensaje('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}



function dibujarPlantilla(objCorreo, accion, tipoCorreo, idEmpresa, idPeriodo) {

    var esEditable = true;
    var habilitacion = "";
    var habilitacionCcBcc = "";
    var colorFondo = "";

    if (accion == VER) {
        esEditable = false;
        habilitacion = "disabled";
        habilitacionCcBcc = "disabled";
    }

    if (accion == NUEVO) {
        esEditable = true;
        habilitacion = "disabled";
        habilitacionCcBcc = "";
        colorFondo = "white";
    }

    //Formato 
    
    var val_To = objCorreo.Corrto != null ? objCorreo.Corrto : "";
    var val_CC = objCorreo.Corrcc != null ? objCorreo.Corrcc : "";
    var val_BCC = objCorreo.Corrbcc != null ? objCorreo.Corrbcc : "";
    var val_Asunto = objCorreo.Corrasunto != null ? objCorreo.Corrasunto : "";
    var val_Contenido = objCorreo.Corrcontenido != null ? objCorreo.Corrcontenido : "";
    var val_Archivos = objCorreo.Archivos != null ? objCorreo.Archivos : "";

    var cadena = '';

    //Agrego nombre del correo
    if (accion == VER) {
        cadena += `
            <div id="informacionPlantilla" style="height:20px; padding-top: 8px; padding-bottom: 12px;" >
                <div class="tbform-label" style="width:120px; float: left; font-size:18px;">
                    Id correo:
                </div>
                <div id="valor" style="float: left; font-size:18px;">
                    ${objCorreo.Corrcodi}
                </div>
            </div>
    `;
    }
    cadena += `
            <table style="width:100%">    
                <tr>
                    <td style="width:120px" class="registro-label"><input type="hidden" id="hfIdPlantillaCorreo" value="${objCorreo.Corrcodi}"/></td>
                </tr>
                
                <tr>
                    <td class="tbform-label">Para (*):</td>
                    <td class="registro-control" style="width:1100px;"><input type="text" name="To" id="To" value="${val_To}" maxlength="100" style="width:1090px;" ${habilitacion}/></td>
                </tr>                 
            
                <tr>
                    <td class="tbform-label">CC:</td>
                    <td class="registro-control" style="width:1100px;"><input name="CC" id="CC" type="text" value="${val_CC}" maxlength="120" style="width:1090px; background:${colorFondo}"   ${habilitacionCcBcc}/></td>
                    
                </tr>
                <tr>
                    <td class="tbform-label"> BCC:</td>
                    <td class="registro-control" style="width:1100px;"><input name="BCC" id="BCC" type="text" value="${val_BCC}" maxlength="120" style="width:1090px; background:${colorFondo}" ${habilitacionCcBcc}/></td>
                </tr>
                <tr>
                    <td class="tbform-label">Asunto (*):</td>
                    <td class="registro-control" style="width:1100px;">
                        <textarea maxlength="450" name="Asunto" id="Asuntos" value=""  cols="" rows="3" style="resize: none;width:1090px;"  ${habilitacion}>${val_Asunto}</textarea>
                    </td>
                    
                </tr>
                <tr>
                    <td class="tbform-label"> Contenido (*):</td>
                    <td class="registro-control" style="width:1100px;">
                        <textarea name="Contenido" id="Contenido" maxlength="2000" cols="180" rows="22"  ${habilitacion}>
                            ${val_Contenido}
                        </textarea>
    `;

    if (accion == NUEVO) {
        cadena += `
                        (*): Campos obligatorios.  <br> <br>                
        `;
        if (tipoCorreo == SolEnvioInterrupcionesSemestral || tipoCorreo == SolEnvioInterrupcionesTrimestral) {
            cadena += `
                        <b>Nota:</b> Al presionar <b>Enviar</b> el sistema enviará mensaje a todas los emails del campo <b>Para, CC y BCC</b>.                   
            `;
        } else {
            if (tipoCorreo == SolEnvioObservacionesAInterrupciones ) {
                cadena += `
                        <b>Nota:</b> Al presionar <b>Enviar</b> el sistema solo enviará mensaje a aquellas empresas responsables que tengan registrado emails.                   
            `;
            } else {
                if (tipoCorreo == SolEnvioRespuestasAInterrupciones || tipoCorreo == SolDecisionesControversia) {
                    cadena += `
                        <b>Nota:</b> Al presionar <b>Enviar</b> el sistema solo enviará mensaje a aquellas empresas suministradoras que tengan registrado emails.                   
            `;
                } else {

                }
            }
        }
    } else {

    }
    
    cadena += `
                        
                    </td>
                    <td class="registro-label">
                    
                    </td>
                </tr>
            </table>
    `;

    if (accion == NUEVO) {        

        if (esEditable) {
            if (tipoCorreo == SolEnvioInterrupcionesPendientesReportar) {

                cadena += `

                    <div style="clear:both; height:10px"></div>
                    <table class="" style="width:100%">
                        <tr>
                            <td class="tbform-label" style="width:120px">Archivo Adjuntado:</td>
                            <td>
                                <table id="tabRepIntPendientes">

                                    <tr>
                                            <td style="width:30px;">
                                                ${IMG_FILE}
                                            </td>
                                            <td style="text-align:left;">
                                                <h4 onclick="descargarArchivoInterrupcionesPendientesPorSuministrador(${idEmpresa}, ${idPeriodo});" title= "Descargar Archivo" style="cursor:pointer;margin: 3px;width:fit-content;">Interrupciones_Pendientes_Reportar.xlsx</h4>
                                            </td>
                                     </tr>                                           
                              </table>
                           </td>
                        </tr>
                    </table>
                `;

                cadena += `

                    
                    <table class="" style="width:95%">
                        <tr>
                            <td class="tbform-label" style="width:120px"></td>
                            <td class="registro-control" style ="">
                                <div style="padding:5px">
                                     <div id="container" class="file-carga" style="padding: 0px 0px 0px 0px; margin-top: -10px; width: 85%; min-height: 0px;">
                                        <div id="loadingcarga" class="estado-carga">
                                            <div class="estado-image">${IMG_LOADING}</div>
                                            <div class="estado-text">Cargando...</div>
                                        </div>
                                        <div id="filelist">No soportado por el navegador.</div>
                                    </div>
                                    <input id="fileList" type="hidden" name="Archivo" value=""/>
                                </div>
                                <div>
                                     <input type="button" id="btnSelectFile2" value="Seleccionar un archivo" />
                                     <div id="filelist" style="padding-top: 5px;padding-left: 5px;font-size: 11px;">Al adjuntar un nuevo archivo, el anterior quedará descartado.</div>
                                </div>
                            </td>                            
                        </tr>
                    </table>
                    
                `; 
            }
            cadena += `
        <table style="width:100%">
            <tr>
                
                <td colspan="3" style="padding-top: 10px; text-align: center;">
                    <input type="button" id="btnEnviarCorreo" value="Enviar" />
                    
                </td>
            </tr>
        </table>
        `;
        }
    } else {
        if (accion == VER) {
            cadena += `

            <div style="clear:both; height:10px"></div>
            <table class="" style="width:100%">
                <tr>
                    <td class="tbform-label" style="width:120px">Archivos Adjuntados:</td>
                    <td>
                        <table>
                           
            `;

            if (val_Archivos.trim() != "") {
                var listaArchivo = val_Archivos.split('/');

                for (var i = 0; i < listaArchivo.length; i++) {
                    var item = listaArchivo[i];
                    cadena += `
                                <tr>

                                    <td style="width:30px;">
                                        ${IMG_FILE}
                                    </td>
                                    <td style="text-align:left;">                                        
                                        <h4 onclick="descargarArchivo('${item}',${objCorreo.Corrcodi});" style="cursor:pointer;margin: 3px;width:fit-content;">${item}</h4>
                                    </td>
                                </tr>
                    `;
                }
            }

            cadena += `
                       
                      </table>
                   </td>
                </tr>
            </table>
            `;

        }
    }




    return cadena;
}


function cargaDeArchivo() {
    limpiarBarraMensaje("mensaje");
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile2',
        container: document.getElementById('container'),
        url: controlador + "Upload",
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '20mb',
            mime_types: [                
                { title: "Documentos", extensions: "xls,xlsx" }
            ]
        },
        init: {
            PostInit: function () {
                $('#filelist').html('');
                $('#container').css('display', 'none');
                limpiarBarraMensaje("mensaje");
            },
            FilesAdded: function (up, files) {
                limpiarBarraMensaje("mensaje");
                
                gestionarArchivoCargado(up);                                           
            },
            UploadProgress: function (up, file) {
                document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = '<span>' + file.percent + "%</span>";
                $('#' + file.id).css('background-color', '#FFFFB7');
            },
            UploadComplete: function (up, file) {
                $('#loadingcarga').css('display', 'none');
                validarArchivo(up, uploader);                
            },
            FileUploaded: function (up, file, result) {
                if (file.size <= 0)
                    alert("Error: El archivo adjuntado no tiene ningun tamaño valido, vuelva cargar");
            },
            Error: function (up, err) {

                if (err.message == "File extension error.") {
                    mostrarMensaje('mensaje', 'alert', "El archivo seleccionado no cumple con las extensiones permitidas (xls,xlsx). Procure adjuntar archivos de estos tipos.");
                } else {
                    if (err.message == "File size error.") {
                        mostrarMensaje('mensaje', 'alert', "El archivo adjuntado sobrepasa los 20MB permitidos.");
                    } else {
                        mostrarMensaje('mensaje', 'alert', err.message);
                    }
                }

            }
        }
    });
    uploader.init();

    eliminarFile = function (id, val) {
        uploader.removeFile(id);
        $('#' + id).remove();
        repIntPendienteAdjuntado = val;
        $('#fileList').val("");
    }

    gestionarArchivoCargado = function (up) {
        $('#filelist').html('');
        $('#container').css('display', 'block');

        //Si hay mas de 2 archivos adjuntados elimino 1
        var numArchivosAdjuntados = uploader.files.length;
        if (numArchivosAdjuntados > 1) {
            if (confirm("Ya existe un archivo adjuntado, ¿Desea reemplazar dicho archivo?")) {
                //elimina el archivo anterior
                var file = uploader.files[0];
                eliminarFile(file.id, 0);

                //carga el unico archivo
                for (i = 0; i < uploader.files.length; i++) {
                    var file = uploader.files[i];

                    $('#filelist').html($('#filelist').html() + '<div class="file-name" id="' + file.id + '">' + file.name +
                        ' (' + plupload.formatSize(file.size) + ') <a class="remove-item" title="Eliminar archivoo adjuntado" href="JavaScript:eliminarFile(' +
                        '\'' + file.id + '\',0);">X</a> <b></b></div>');

                    $('#fileList').val($('#fileList').val() + "/" + file.name);
                }
                up.refresh();
                uploader.start();
                repIntPendienteAdjuntado = 1;
            } else {
                //elimina el ultimo archivo cargado
                var file = uploader.files[1];
                eliminarFile(file.id, 1);

                for (i = 0; i < uploader.files.length; i++) {
                    var file = uploader.files[i];

                    $('#filelist').html($('#filelist').html() + '<div class="file-name" id="' + file.id + '">' + file.name +
                        ' (' + plupload.formatSize(file.size) + ') <a class="remove-item" title="Eliminar archivo adjuntado" href="JavaScript:eliminarFile(' +
                        '\'' + file.id + '\',0);">X</a> <b></b></div>');

                    $('#fileList').val($('#fileList').val() + "/" + file.name);
                }
                //up.refresh();//
            }
        } else {
            if (repIntPendienteAdjuntado == 1) {
                if (confirm("Al adjuntar este archivo reemplazará al archivo original, ¿Desea reemplazar dicho archivo?")) {
                    //carga el primer archivo y oculta el archivo por defecto
                    for (i = 0; i < uploader.files.length; i++) {
                        var file = uploader.files[i];

                        $('#filelist').html($('#filelist').html() + '<div class="file-name" id="' + file.id + '">' + file.name +
                            ' (' + plupload.formatSize(file.size) + ') <a class="remove-item" title="Eliminar archivo adjuntado" href="JavaScript:eliminarFile(' +
                            '\'' + file.id + '\',0);">X</a> <b></b></div>');

                        $('#fileList').val($('#fileList').val() + "/" + file.name);
                    }
                    up.refresh();
                    uploader.start();

                    //oculta archivo defecto
                    $("#tabRepIntPendientes").css("display", "none");

                    repIntPendienteAdjuntado = 1;
                    repIntPendienteArchOriginal = 0;
                } else {
                    //elimina el ultimo archivo cargado
                    var file = uploader.files[0];
                    eliminarFile(file.id, 1);
                }
            } else {
                //carga el unico archivo
                for (i = 0; i < uploader.files.length; i++) {
                    var file = uploader.files[i];

                    $('#filelist').html($('#filelist').html() + '<div class="file-name" id="' + file.id + '">' + file.name +
                        ' (' + plupload.formatSize(file.size) + ') <a class="remove-item" title="Eliminar archivo adjuntado" href="JavaScript:eliminarFile(' +
                        '\'' + file.id + '\',0);">X</a> <b></b></div>');

                    $('#fileList').val($('#fileList').val() + "/" + file.name);
                }
                up.refresh();
                uploader.start();
                repIntPendienteAdjuntado = 1;
            }
        }  
    }

    //return uploader;validarReporteIPR
}

function validarArchivo(up, uploader) {
    $.ajax({
        type: 'POST',
        url: controlador + 'validarReporteIPR',
        dataType: 'json',
        data: {
            
        },
        success: function (result) {
            if (result.Result == 1) {    }
            else if (result.Result == 2) {
                var file = uploader.files[0];
                eliminarFile(file.id, 0);

                var html = "No se realizó la carga por que se encontraron los siguientes errores: <ul>";
                for (var i in result.Errores) {
                    html = html + "<li>" + result.Errores[i] + "</li>";
                }
                html = html + "</ul>";
                mostrarMensaje('mensaje', 'alert', html);
            }
            else if (result.Result == -1) {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}
function getCorreo() {
    var obj = {};

    obj.Corrcc = $("#CC").val();
    obj.Corrbcc = $("#BCC").val();
    obj.Archivos = "";
    var tipoCorreo = $("#cbTipoCorreoEnvio").val();
    if (tipoCorreo == SolEnvioInterrupcionesPendientesReportar) {
        obj.Archivos = $("#fileList").val();
    }

    return obj;
}

function validarCamposCorreoAGuardar(correo) {
    var msj = "";  

    /*********************************************** validacion del campo CC ***********************************************/
    var validaCc = validarCorreo($('#CC').val(), 0, -1);
    if (validaCc < 0) {
        msj += "<p>Error encontrado en el campo 'CC'. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }



    /*********************************************** validacion del campo BCC ***********************************************/
    var validaBcc = validarCorreo($('#BCC').val(), 0, -1);
    if (validaBcc < 0) {
        msj += "<p>Error encontrado en el campo 'BCC'. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
    }

    /*********************************************** validacion del campo PARA (solo para Interrupciones Pendientes) ***********************************************/
    var tipoCorreo = $("#cbTipoCorreoEnvio").val();
    if (tipoCorreo == SolEnvioInterrupcionesPendientesReportar) {
        //valido que existan correos registrados de suministradors
        var validaPara = validarCorreo($('#To').val(), 1, -1);
        if (validaPara < 0) {
            msj += "<p>Error encontrado en el campo 'Para'. Este suministrador no tiene correos registrados.</p>";
        }

        //valido que exista 1 archivo adjuntado
        if (repIntPendienteAdjuntado == 0) {
            msj += "<p>Se debe adjuntar un archivo.</p>";
        }
    }

    return msj;
}

function validarCorreo(valCadena, minimo, maximo) {
    var cadena = valCadena;

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
        return -2;

    return 1;
}

function validarDirecccionCorreo(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}


function enviarMensaje() {
    limpiarBarraMensaje("mensaje");

    var periodoId = parseInt($("#cbPeriodo").val());
    var tipoCorreo = parseInt($("#cbTipoCorreoEnvio").val());

    var correo = {};
    correo = getCorreo();

    var msg = validarCamposCorreoAGuardar(correo);
    var empresaSel = parseInt($("#cbEmpresa").val()) || 0; 
     
    if (msg == "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'EnviarGrupoCorreo',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({
                correo: correo,
                repercodi: periodoId,
                tipoCorreo: tipoCorreo, 
                idEmpresa: empresaSel,
                idArchivoOriginal: repIntPendienteArchOriginal
            }
            ),
            cache: false,
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    $('#tab-container').easytabs('select', '#tabLista');
                    
                    cargarListaCorreos();
                    mostrarMensaje('mensaje', 'exito', "El correo fue enviado exitosamente...");
                    $("#div_filtro").html("");
                    $("#div_detalle").html("");

                } else {
                    mostrarMensaje('mensaje', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}




function iniciarControlTexto(id, listaVariables, sololectura) {

    tinymce.remove();

    tinymce.init({
        selector: '#' + id,
        plugins: [
            //'paste textcolor colorpicker textpattern link table image imagetools preview'
            'wordcount advlist anchor autolink codesample colorpicker contextmenu fullscreen image imagetools lists link media noneditable preview  searchreplace table template textcolor visualblocks wordcount'
        ],
        toolbar1:
            //'insertfile undo redo | fontsizeselect bold italic underline strikethrough | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link | table | image | mybutton | preview',
            'insertfile undo redo | styleselect fontsizeselect | forecolor backcolor | bullist numlist outdent indent | link | table | image | preview',

        menubar: false,
        readonly: sololectura,
        language: 'es',
        statusbar: false,
        convert_urls: false,
        plugin_preview_width: 1000,
        setup: function (editor) {
            editor.on('change',
                function () {
                    editor.save();
                });
            editor.on('init',
                function () {
                    
                });
        },
        
    });
    

}


function descargarArchivo(nomArchivo, relcorcodi) {
    window.location = controlador + 'DescargarArchivoEnvio?fileName=' + nomArchivo + '&relcorcodi=' + relcorcodi;

}
function descargarArchivoInterrupcionesPendientesPorSuministrador(idEmpSuministrador, idPeriodo) { 
    
        $.ajax({            
            type: 'POST',
            url: controlador + "GenerarYDescargarReporteIntPendientesReportar",
            data: {
                idSuministrador: idEmpSuministrador,
                idPeriodo: idPeriodo
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controlador + "ExportarDesdeTemporales?file_name=" + evt.Resultado;
                } else {
                    mostrarMensaje('mensaje', 'error', 'Error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
   
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