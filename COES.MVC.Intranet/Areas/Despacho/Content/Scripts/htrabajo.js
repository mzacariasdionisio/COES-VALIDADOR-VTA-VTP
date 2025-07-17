var controladorConfig = siteRoot + 'Despacho/HTrabajo/';

var CONFIGURACION_WEB = null;
var CONFIGURACION_GLOBAL = null;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ACTUAL = 0;
var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="" width="19" height="19" style="">';
var IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" width="19" height="19" style="">';
var IMG_VERSIONES = '<img src="' + siteRoot + 'Content/Images/btn-properties.png" alt="" width="19" height="19" style="">';
//var IMG_AGREGAR = '<img src="' + siteRoot + 'Content/Images/btn-add.png" alt="" width="19" height="19" style="">';
var IMG_AGREGAR = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="" width="19" height="19" style="">';

var CORRELATIVO_AGRUP = 0;
var CORRELATIVO_AGRUP2 = 0;

var opcionPto = 1;
var opcionCanal = 2;

$(function () {
    $("#btnNuevo").click(function () {
        inicializarFormulario_(0, true);
    });

    cargarListadoConfiguracion();
});

///////////////////////////
/// Listado de centrales
///////////////////////////

function cargarListadoConfiguracion() {
    $("#div_reporte").html('');
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controladorConfig + 'ListadoConfiguracionRER',
        dataType: 'json',
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                //LISTA_REPORTE_CONFIGURACION = data.ListaConfiguracion;
                var html = dibujarTablaReporte(data.ListaConfiguracionRER);
                $("#div_reporte").html(html);

                $('#reporteConfiguracion').dataTable({
                    "destroy": "true",
                    "scrollY": 550,
                    "scrollX": true,
                    "sDom": 'ft',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1
                });

            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaReporte(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="reporteConfiguracion">
        <thead>
            <tr>
                <th style='width: 70px'>Acción</th>
                <th style='width: 100px'>Tipo Central</th>
                <th style='width: 100px'>Nombre Central</th>
                <th style='width: 80px'>Fuente de Datos</th>
                <th style='width: 310px'>Elemento</th>
                <th style='width: 70px'>Factor</th>
                <th style='width: 70px'>Usuario modificación</th>
                <th style='width: 70px'>Fecha modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        var nombreElemento = item.NombreElemento.replaceAll("|", "<br />");
        var factorDesc = item.FactorDesc.replaceAll("|", "<br />");

        cadena += `
            <tr>
                <td style="height: 24px">
                    <a title="Editar registro" href="JavaScript:editarConfiguracion(${item.Htcentcodi});">${IMG_EDITAR} </a>
                    <a title="Eliminar registro" href="JavaScript:eliminarConfiguracion(${item.Htcentcodi});">${IMG_ELIMINAR} </a>
                </td>
                <td style="height: 24px;text-align: center; ">${item.Famnomb}</td>
                <td style="height: 24px;text-align: center; ">${item.Equinomb}</td>
                <td style="height: 24px;text-align: center; ">${item.FuenteDesc}</td>
                <td style="height: 24px;text-align: left; ">${nombreElemento}</td>
                <td style="height: 24px;text-align: center;">${factorDesc}</td>
                <td style="height: 24px">${item.UltimaModificacionUsuarioDesc}</td>
                <td style="height: 24px">${item.UltimaModificacionFechaDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function guardarFormulario() {
    var objCentralCfg = generarJsonModelo();
    var msj = validarJsonModelo(objCentralCfg);

    if (msj == '') {
        $.ajax({
            type: 'POST',
            url: controladorConfig + "GuardarConfiguracion",
            data: objCentralCfg,
            cache: false,
            success: function (model) {
                if (model.Resultado != "-1") {
                    $('#popup_formulario').bPopup().close();

                    cargarListadoConfiguracion();
                    _mostrarMensajeAlertaTemporal(true, "El registro se guardó correctamente.");
                } else {
                    _mostrarMensajeAlertaTemporal(false, model.Mensaje);
                }
            },
            error: function () {
                $("#mensaje").show();
                mostrarMensaje("mensaje", msj, $tipoMensajeError, $modoMensajeCuadro);
            }
        });
    } else {
        _mostrarMensajeAlertaTemporal(false, msj);
    }
}

function validarJsonModelo(obj) {

    var fuente = obj.tipo == opcionPto ? "I" : "S";

    var msj = '';
    if (obj.equicodi <= 0)
        msj += `No ha seleccionado Central` + "<br/>";
    if (obj.tipo == opcionPto) {
        if (obj.listaCfg.length == 0)
            msj += `No ingresó puntos de medición IDCC. ` + "<br/>";
    }
    else
        if (obj.listaCfg.length == 0)
            msj += `No ingresó canales Scada. ` + "<br/>";


    if (obj.listaCfgNovalida.length > 0) {
        msj += `Tipo de Fuente Incompatible con CC.` + "<br/>";
    }
    //if (obj.idCentralCfg > 0) {
    //    if (CONFIGURACION_WEB.Htcentfuente != fuente)
    //        msj += `Tipo de Fuente Incompatible con CC”.` + "<br/>";
    //}

    return msj;
}

function editarConfiguracion(htcentcodi) {
    $("#mensaje").hide();
    inicializarFormulario_(htcentcodi, true);
}

function eliminarConfiguracion(htcentcodi) {
        $.ajax({
            type: 'POST',
            url: controladorConfig + "EliminarConfiguracion",
            data: {
                htcentcodi: htcentcodi
            },
            cache: false,
            success: function (model) {
                if (model.Resultado != "-1") {

                    cargarListadoConfiguracion();
                    _mostrarMensajeAlertaTemporal(true, "El registro se eliminó correctamente.");
                } else {
                    _mostrarMensajeAlertaTemporal(false, model.Mensaje);
                }
            },
            error: function () {
                $("#mensaje").show();
                _mostrarMensajeAlertaTemporal(false, "Ha ocurrido un error");
            }
        });
}

///////////////////////////
/// Formulario web
///////////////////////////

function generarJsonModelo() {
    var idCentralCfg = parseInt($("#idHtCentralCfg").val()) || 0;
    var equicodi = parseInt($("#cbFormRecurso").val()) || 0;
    var chkReal = $("#chkTReal").is(':checked');
    var chkHTrabajo = $("#chkHtrabajo").is(':checked');
    var tipo = 0;
    var tipoSinCheck = 0;
    if (chkReal) {
        tipo = 2;
        tipoSinCheck = 1;
        // = "I";
    }
    if (chkHTrabajo) {
        tipo = 1;
        tipoSinCheck = 2;
        //"S";
    }

    var listaCfg = generarJsonConfiguracion(tipo);
    var listaCfgNovalida = generarJsonConfiguracion(tipoSinCheck);

    var obj = {
        equicodi: equicodi,
        idCentralCfg: idCentralCfg,
        tipo: tipo,
        listaCfg: listaCfg,
        listaCfgNovalida: listaCfgNovalida,
        strConf: JSON.stringify(listaCfg)
    };

    return obj;
}

function generarJsonConfiguracion(tipo) {
    var listaConf = [];

    $(`tr[id^="tr_agrup_conf_${tipo}"]`).each(function (i, obj) {
        var idHtml = $(obj).attr('id');

        var codigo = parseInt(idHtml.split("_")[6]) || 0;
        var factor = parseFloat($(`#${idHtml} input[name^="factor"]`).val()) || 1.0;

        var objConf = {
            Ptomedicodi: tipo == 1 ? codigo : null,
            Canalcodi: tipo == 2 ? codigo : null,
            Htcdetfactor: factor,
        };

        listaConf.push(objConf);
    });

    return listaConf;
}

function _mostrarMensajeAlertaTemporal(esExito, mensaje) {
    $("#mensaje").hide();
    $("#mensaje").show();

    if (esExito)
        $("#mensaje").html(`<div class='action-exito ' style='margin: 0; padding-top: 10px; padding-bottom: 10px;'>${mensaje}</div>`);
    else
        $("#mensaje").html(`<div class='action-error ' style='margin: 0; padding-top: 10px; padding-bottom: 10px;'>${mensaje}</div>`);
    setTimeout(function () { $("#mensaje").fadeOut(1000) }, 2500);
}


//////////////////////////////////final
function inicializarFormulario_(htcentcodi, editable) {
    $("#div_formulario").html('');
    //$('#popupFormConcepto').html('');

    $.ajax({
        type: 'POST',
        url: controladorConfig + "ObtenerHtCentralCfg",
        dataType: 'json',
        data: {
            htcentcodi: htcentcodi,
            editable: editable
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                CONFIGURACION_WEB = evt.Entidad;
                CONFIGURACION_GLOBAL = evt;

                //....................................................
                if (evt.AccionNuevo) {
                    OPCION_ACTUAL = OPCION_NUEVO;
                } else {
                    if (evt.AccionEditar) {
                        OPCION_ACTUAL = OPCION_EDITAR;
                    }
                }

                //Inicializar Formulario
                var tituloPopup = evt.AccionEditar? 'Edición de configuración' : 'Nueva configuración';
                $("#popup_formulario .popup-title span").html(tituloPopup);

                var htmlForm = dibujarHtmlFormulario_(evt);
                $("#div_formulario").html(htmlForm);

                //inicializarVistaFormulario();
                cargarEventosFormulario();

                $('#popup_formulario').bPopup({
                    modalClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar Concepto");
        }
    });
}

function dibujarHtmlFormulario_(model, objEdit) {
    //idConfiguracion;
    var codconfig = model.AccionEditar ? CONFIGURACION_WEB.Htcentcodi : 0;

    var tipo = opcionPto;
    var htmlCheckTR = "";
    var htmlCheckHT = "";
    if (model.AccionEditar) {
        tipo = CONFIGURACION_WEB.Htcentfuente == "I" ? opcionPto : opcionCanal;
        htmlCheckTR = CONFIGURACION_WEB.Htcentfuente == "S" ? 'checked="checked"' : "";
        htmlCheckHT = CONFIGURACION_WEB.Htcentfuente == "I" ? 'checked="checked"': "";
    }
    else
        htmlCheckHT = 'checked="checked"';

    //Central
    var idCentral = model.AccionEditar ? CONFIGURACION_WEB.Equicodi : 0;
    var centralnombre = model.AccionEditar ? CONFIGURACION_WEB.Equinomb : 0;

    var htmlcbFormCentrales = '';
    if (model.AccionNuevo) {
        htmlcbFormCentrales += `<select id='cbFormRecurso'>`;
        htmlcbFormCentrales += `<option value='0'>--SELECCIONE--</option>`;
        for (var i = 0; i < model.ListaCentrales.length; i++) {
            var v = model.ListaCentrales[i];
            var esSelected = v.Equicodi == idCentral ? ' selected ' : '';
            htmlcbFormCentrales += `<option value='${v.Equicodi}' ${esSelected}>${v.Equicodi} - ${v.Equinomb}</option>`;
        }
        htmlcbFormCentrales += `</select>`;
    } else {
        htmlcbFormCentrales += `<input type='text' value='${centralnombre}' style="width: 250px" disabled /> `;
        htmlcbFormCentrales += `<input type='hidden' id='cbFormRecurso' value='${idCentral}' /> `;
    }


    //html tabla Caudal
    //var listaConfig = model.AccionEditar? model.ListaConfiguracion : [];
    var listaConfigHT = model.AccionEditar && tipo == opcionPto ? model.ListaConfiguracion : [];
    var listaConfigTR = model.AccionEditar && tipo == opcionCanal ? model.ListaConfiguracion : [];

    var htmlTablaAgrup = generarHtmlTablaAgrupCentral_(codconfig, model, listaConfigHT, opcionPto, htmlCheckHT);
    var htmlTablaAgrup2 = generarHtmlTablaAgrupCentral_(codconfig, model, listaConfigTR, opcionCanal, htmlCheckTR);

    //Div general
    var html = `
<input type="hidden" id="idHtCentralCfg" value="${codconfig}" />
<input type="hidden" id="idTipoFuente" value="${tipo}" />
    <div class="content-registro" style="width:auto">

        <div style="clear:both; height:10px;"></div>

        <table cellpadding="8" style="width:auto">
            <tbody>
                <tr>
                    <td class="registro-label" style="width:120px;">CENTRAL:</td>
                    <td class="registro-control" style="width:300px;">
                    ${htmlcbFormCentrales}
                    </td>
                </tr>
                <tr>
                    <td class="registro-label" style="width:120px;">Fuente de Datos:</td>
                    <td class="tbform-control">
                        <div>
                            <input type="radio" id="chkTReal" name="Htcentfuente" value="S" ${htmlCheckTR}> <span>Tiempo Real SP7</span>
                        </div>
                        <div>
                            <input type="radio" id="chkHtrabajo" name="Htcentfuente" value="I" ${htmlCheckHT}> <span>Archivo HTrabajo</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class='td02'>
                        ${htmlTablaAgrup}
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class='td02'>
                        ${htmlTablaAgrup2}
                    </td>
                </tr>
            </tbody>
        </table>

        <table class="btnAcciones" style="width: 200px; float: right;">
             <tbody><tr>
                    <td>
                        <input type="button" id="btnGrabarForm" value="Grabar">
                    </td>
                    <td>
                        <input type="button" id="btnCancelarForm" value="Cancelar">
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
`;

    return html;
}

function generarHtmlTablaAgrupCentral_(codconfig, model, listaConfig, tipo, tienecheck) {

    //var visible = tienecheck.length > 0 ? "show": "hidden";

    //crear tabla con la primera fila default
    var htmlNombre = tipo == 1 ? "Punto de medición IDCC" : "Canal Scada";
    var correlativo = tipo == 1 ? CORRELATIVO_AGRUP : CORRELATIVO_AGRUP2;

    var htmlTrAgrupDefault = generarHtmlTrAgrupacionCentral_(codconfig, correlativo, tipo, listaConfig);

    var html = `
    <table id='table_central_agrup_${tipo}_${codconfig}' class='table_central_agrup' style="width: 100%;"}>
        <thead>
            <tr>
                <th class='th1'>${htmlNombre}</th>
            </tr>
        </thead>
        <tbody class='tbody_table_central_agrup'>
            ${htmlTrAgrupDefault}
        </tbody>
    </table>
    `;

    return html;
}

function generarHtmlTrAgrupacionCentral_(codconfig, correlativoAgrup, tipo, listaConfig) {
    //html trs
    var htmlTr = '';
    var listaConfXAgrup = listaConfig;
    for (var i = 0; i < listaConfXAgrup.length; i++) {
        var objConfig = listaConfXAgrup[i];
        var cod_fila = objConfig.Ptomedicodi != null ? objConfig.Ptomedicodi : objConfig.Canalcodi; // obtiene le codigo (ptomedicon o canal)
        htmlTr += generarHtmlTrAgrupConf_(codconfig, correlativoAgrup, cod_fila, objConfig.Htcdetfactor, tipo);
    }

    //
    var html = `
            <tr id='tr_table_agrup_${tipo} _${codconfig}_${correlativoAgrup}' class='tr_table_central_agrup'>
                <td class='td_agrup' style="padding: 10px;">

                    <table style="width: 100%;">
                        <tbody id='tbody_fuente_${tipo}_${codconfig}_${correlativoAgrup}'>
                            <tr>
                                <td colspan="4" style="padding: 10px;text-align: left;">
                                    Buscar Código:
                                    <input type="text" id='txtPtomedicion_${tipo}' name="ptomedicion" value="" style=" width: 80px;" >
                                    <a title="Agregar registro" href="JavaScript:addFuenteToAgrupacionTrCentral(${tipo}, ${codconfig}, ${correlativoAgrup});">  ${IMG_AGREGAR} </a>
                                </td>
                            </tr>
                            ${htmlTr}
                        </tbody>
                    </table>

                </td>
                <td></td>
            </tr>
    `;

    if (tipo == 1) CORRELATIVO_AGRUP++;
    else CORRELATIVO_AGRUP2++;

    //CORRELATIVO_AGRUP++;

    return html;
}

function addFuenteToAgrupacionTrCentral(tipo, codconfig, correlativoAgrup) {
    var idTbody = `tbody_fuente_${tipo}_${codconfig}_${correlativoAgrup}`;
    var idSelect = `#txtPtomedicion_${tipo}`;
    var codigoFila = parseInt($(idSelect).val()) || 0;
    var factor = "1.0";
    var flagValido = true;

    $(`tr[id^="tr_agrup_conf_${tipo}"]`).each(function (i, obj) {
        var idHtml = $(obj).attr('id');

        var codigo = parseInt(idHtml.split("_")[6]) || 0;
        var factor = parseFloat($(`#${idHtml} input[name^="factor"]`).val()) || 1.0;

        if (codigoFila == codigo) {
            alert("el codigo ya ha sido agregado");
            flagValido = false;
        }
    });


    if (codigoFila > 0 && flagValido) {
        //var htmlTr = obtenerPtoMedicion(codconfig, correlativoAgrup, codigoFila, '1.0', tipo);
        var desc = '';
        var htmlTr = '';
        $.ajax({
            type: "POST",
            url: controladorConfig + "ObtenerElemento",
            data: {
                codigo: codigoFila,
                //tipo: $("#idTipoFuente").val()
                tipo: tipo
            },
            global: false,
            success: function (evt) {

                if (evt.Resultado == "1") {
                    var regElemento = evt.Elemento;
                    desc = codigoFila + ' - ' + regElemento.Descripcion;
                    //var regPto = evt.Puntomed;
                    //desc = codigoFila + ' - ' + regPto.Ptomedielenomb;
                    htmlTr = `
                    <tr id='tr_agrup_conf_${tipo}_${codconfig}_${correlativoAgrup}_${codigoFila}'>
                        <td style='width: 10px;' class='tr_conf_td01'>
                            <a title="Eliminar registro" href="JavaScript:quitFuenteToAgrupacionTrCentral_(${tipo}, ${codconfig},${correlativoAgrup}, ${codigoFila});">  ${IMG_ELIMINAR} </a>
                        </td>
                        <td style='' class='tr_conf_td02'>${desc}</td>

                        <td style='width: 40px;' class='tr_conf_td03'>
                            <input type="text" name='factor' value="${factor}" style="width: 30px" >
                        </td>
                    </tr>
                    `;

                    //return htmlTr;
                    $("#" + idTbody).append(htmlTr);

                    $('input[name^="factor"]').unbind();
                    $('input[name^="factor"]').bind('keypress', function (e) {
                        var key = window.Event ? e.which : e.keyCode
                        return (key <= 13 || (key >= 48 && key <= 57) || key == 46 || key == 45);
                    });
                    $('input[name^="factor"]').bind('paste', function (e) {
                        var key = window.Event ? e.which : e.keyCode
                        return (key <= 13 || (key >= 48 && key <= 57) || key == 46 || key == 45);
                    });
                }
                else {
                    alert(evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
        if (codigoFila <= 0)
            alert("EL código debe ser un valor numérico positivo");
    }
}

function generarHtmlTrAgrupConf_(codconfig, correlativoAgrup, codigoFila, factor, tipo) {

    var desc = '';
    var regPto = obtenerObjPto_(codigoFila, tipo);
    if (regPto != null)
    {
        if (tipo == opcionPto)
            desc = codigoFila + ' - ' + regPto.Ptomedielenomb;
        else
            desc = codigoFila + ' - ' + regPto.Canalnomb;
    }

    var htmlTr = `
    <tr id='tr_agrup_conf_${tipo}_${codconfig}_${correlativoAgrup}_${codigoFila}'>
        <td style='width: 10px;' class='tr_conf_td01'>
            <a title="Eliminar registro" href="JavaScript:quitFuenteToAgrupacionTrCentral_(${tipo}, ${codconfig},${correlativoAgrup}, ${codigoFila});">  ${IMG_ELIMINAR} </a>
        </td>
        <td style='' class='tr_conf_td02'>${desc}</td>

        <td style='width: 40px;' class='tr_conf_td03'>
            <input type="text" name='factor' value="${factor}" style="width: 30px" >
        </td>
    </tr>
    `;

    return htmlTr;
}

function quitFuenteToAgrupacionTrCentral_(tipo, codconfig, correlativoAgrup, codigoFila) {
    $(`#tr_agrup_conf_${tipo}_${codconfig}_${correlativoAgrup}_${codigoFila}`).remove();
}

function obtenerObjPto_(ptomedicodi, tipo) {

    //llamar controler para obtener pto o canal

    var listaObjPto = CONFIGURACION_GLOBAL.ListaConfiguracion;
    for (var i = 0; i < listaObjPto.length; i++) {
        if (tipo == opcionPto) {
            if (listaObjPto[i].Ptomedicodi == ptomedicodi)
                return listaObjPto[i];
        }
        else {
            if (listaObjPto[i].Canalcodi == ptomedicodi)
                return listaObjPto[i];
        }
    }

    return null;
}

function cargarEventosFormulario() {

    //$(function () {
    //});


    $("#cbFormRecurso").unbind();

    if (OPCION_ACTUAL == OPCION_NUEVO) {
        $('#cbFormRecurso').multipleSelect({
            width: '220px',
            filter: true,
            single: true,
            onClose: function () {
            }
        });
        $("#cbFormRecurso").multipleSelect("setSelects", [$("#cbFormRecurso").val() || 0]);
    }

    //combo para agregar filas de configuracion
    $('select[name^="cbFormFuente"]').multipleSelect({
        width: '220px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });
    $('select[name^="cbFormFuente"]').multipleSelect("setSelects", [0]);

    //input de factor
    $('input[name^="factor"]').unbind();
    $('input[name^="factor"]').bind('keypress', function (e) {
        var key = window.Event ? e.which : e.keyCode
        return (key <= 13 || (key >= 48 && key <= 57) || key == 46 || key == 45);
    });
    $('input[name^="factor"]').bind('paste', function (e) {
        var key = window.Event ? e.which : e.keyCode
        return (key <= 13 || (key >= 48 && key <= 57) || key == 46 || key == 45);
    });

    $("#btnGrabarForm").unbind();
    $("#btnCancelarForm").unbind();
    $("#btnGrabarForm").click(function () {
        guardarFormulario();
    });
    $("#btnCancelarForm").click(function () {
        $('#popup_formulario').bPopup().close();
    });

    // Comprobar cuando cambia un checkbox
    $('input[type=radio]').on('change', function () {
        if ($(this).is(':checked')) {

            var codcfg = $("#idHtCentralCfg").val();
            var idTable1 = `#table_central_agrup_1_${codcfg}`;
            var idTable2 = `#table_central_agrup_2_${codcfg}`;

            var idCheck = $(this).prop("id");
            if (idCheck.length > 0) {
                if ($(this).prop("id") == "chkTReal") {
                    $("#idTipoFuente").val(opcionCanal);
                }
                else {
                    $("#idTipoFuente").val(opcionPto);
                }
            }
        }
    });

}