var controladorConfig = siteRoot + 'YupanaContinuo/Configuracion/';

var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="" width="19" height="19" style="">';
var IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" width="19" height="19" style="">';
var IMG_VERSIONES = '<img src="' + siteRoot + 'Content/Images/btn-properties.png" alt="" width="19" height="19" style="">';
var IMG_AGREGAR = '<img src="' + siteRoot + 'Content/Images/btn-add.png" alt="" width="19" height="19" style="">';

var LISTA_REPORTE_CONFIGURACION = [];
var LISTA_RECURSO = [];
var LISTA_PUNTO_MEDICION = [];

var CORRELATIVO_AGRUP = 0;

var CONFIGURACION_TIPO_CAUDAL = 1;
var CONFIGURACION_TIPO_RER = 2;

$(function () {
    cargarConfiguracionDefault();
});

function cargarConfiguracionDefault() {
    //formulario
    $("#btnNuevo").click(function () {
        inicializarFormulario(null);
    });

    var tipoConfiguracion = $("#tipoConfiguracion").val();
    var tyupcodi = parseInt($("#tyupcodi").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controladorConfig + 'ListadoFiltroConfiguracion',
        dataType: 'json',
        cache: false,
        data: {
            tyupcodi: tyupcodi
        },
        success: function (data) {
            if (data.Resultado != "-1") {
                LISTA_RECURSO = data.ListaRecurso;
                LISTA_PUNTO_MEDICION = data.ListaPto;

                if (tipoConfiguracion == "B")
                    cargarListadoConfiguracion();
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

///////////////////////////
/// Listado
///////////////////////////

function cargarListadoConfiguracion() {
    var tyupcodi = parseInt($("#tyupcodi").val()) || 0;
    var yupcfgcodi = parseInt($("#yupcfgcodi").val()) || 0;

    $("#div_reporte").html('');
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controladorConfig + 'ListadoReporteConfiguracion',
        dataType: 'json',
        cache: false,
        data: {
            yupcfgcodi: yupcfgcodi
        },
        success: function (data) {
            if (data.Resultado != "-1") {
                LISTA_REPORTE_CONFIGURACION = data.ListaConfiguracion;

                var html = dibujarTablaReporte(tyupcodi, data.ListaConfiguracion);
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

function _getNombreCampoRecurso(tyupcodi) {
    var nombreCampoRecurso = 'Embalse/Planta H.';
    if (CONFIGURACION_TIPO_RER == tyupcodi) nombreCampoRecurso = 'Planta RER';

    return nombreCampoRecurso;
}

function dibujarTablaReporte(tyupcodi, lista) {
    var nombreCampoRecurso = _getNombreCampoRecurso(tyupcodi);

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="reporteConfiguracion">
        <thead>
            <tr>
                <th style='width: 70px'>Acción</th>
                <th style='width: 110px'>${nombreCampoRecurso}</th>
                <th style='width: 110px'>Punto de medición</th>
                <th style='width: 110px'>Factor</th>
                <th style='width: 70px'>Usuario modificación</th>
                <th style='width: 70px'>Fecha modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        var ptomedielenomb = item.Ptomedielenomb.replaceAll("|", "<br />");
        var factorDesc = item.FactorDesc.replaceAll("|", "<br />");

        cadena += `
            <tr>
                <td style="height: 24px">
                    <a title="Editar registro" href="JavaScript:editarConfiguracion(${item.Recurcodi}, '${item.Recurnombre}');">${IMG_EDITAR} </a>
                    <a title="Editar registro" href="JavaScript:eliminarConfiguracion(${item.Recurcodi}, '${item.Recurnombre}');">${IMG_ELIMINAR} </a>
                </td>
                <td style="height: 24px;text-align: left; ">${item.Recurnombre}</td>
                <td style="height: 24px;text-align: center; ">${ptomedielenomb}</td>
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
    var objRecurso = generarJsonModelo();
    var msj = validarJsonModelo(objRecurso);

    if (msj == '') {
        $.ajax({
            type: 'POST',
            url: controladorConfig + "GuardarConfiguracion",
            data: objRecurso,
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
    var nombreCampoRecurso = _getNombreCampoRecurso(tyupcodi);

    var msj = '';
    if (obj.Recurcodi <= 0)
        msj += `No ha seleccionado ${nombreCampoRecurso}. ` + "<br/>";
    if (obj.listaPto.length == 0)
        msj += `No ha seleccionado puntos de medición. ` + "<br/>";

    return msj;
}

function editarConfiguracion(recurcodi, recurnombre) {
    $("#mensaje").hide();

    var objEdit = {};
    objEdit.Recurcodi = recurcodi;
    objEdit.Recurnombre = recurnombre;
    objEdit.ListaPto = [];

    for (var i = 0; i < LISTA_REPORTE_CONFIGURACION.length; i++) {
        var obj = LISTA_REPORTE_CONFIGURACION[i];
        if (obj.Recurcodi == recurcodi) {

            //columna Punto de medición
            var listaDesc = obj.Ptomedielenomb.split("|");
            for (key in listaDesc) {
                var item = listaDesc[key];

                var ptoDesc = item.split("-")[0];
                var ptomedicodi = parseInt(ptoDesc) || 0;

                objEdit.ListaPto.push({ Ptomedicodi: ptomedicodi, Ycdetfactor: 1.0 });
            }

            //columna Factor
            var listaFactorDesc = obj.FactorDesc.split("|");
            for (var j = 0; j < listaFactorDesc.length; j++) {
                objEdit.ListaPto[j].Ycdetfactor = parseFloat(listaFactorDesc[j]) || 0;
            }
        }
    }

    inicializarFormulario(objEdit);
}

function eliminarConfiguracion(recurcodi) {
    var yupcfgcodi = parseInt($("#yupcfgcodi").val()) || 0;

        $.ajax({
            type: 'POST',
            url: controladorConfig + "GuardarConfiguracion",
            data: {
                recurcodi: recurcodi,
                yupcfgcodi: yupcfgcodi,
                strConf: null
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
    var yupcfgcodi = parseInt($("#yupcfgcodi").val()) || 0;
    var recurcodi = parseInt($("#cbFormRecurso").val()) || 0;
    var listaPto = generarJsonConfiguracion();

    var obj = {
        yupcfgcodi: yupcfgcodi,
        recurcodi: recurcodi,
        listaPto: listaPto,
        strConf: JSON.stringify(listaPto)
    };

    return obj;
}

function generarJsonConfiguracion() {
    var listaConf = [];

    $(`tr[id^="tr_agrup_conf_"]`).each(function (i, obj) {
        var idHtml = $(obj).attr('id');

        var ptomedicodi = parseInt(idHtml.split("_")[5]) || 0;
        var factor = parseFloat($(`#${idHtml} input[name^="factor"]`).val()) || 1.0;

        var objConf = {
            Ptomedicodi: ptomedicodi,
            Ycdetfactor: factor,
        };

        listaConf.push(objConf);
    });

    return listaConf;
}

////////////////////////////////////////////////////////////
function inicializarFormulario(objEdit) {
    $("#div_formulario").html('');

    var tituloPopup = objEdit != null ? 'Edición de configuración' : 'Nueva configuración';
    $("#popup_formulario .popup-title span").html(tituloPopup);

    var htmlForm = dibujarHtmlFormulario(objEdit);
    $("#div_formulario").html(htmlForm);

    $(function () {
        $("#cbFormRecurso").unbind();

        if (objEdit == null) {
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

        setTimeout(function () {
            $("#popup_formulario").bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);
    });

}

function dibujarHtmlFormulario(objEdit) {
    var tyupcodi = parseInt($("#tyupcodi").val()) || 0;
    var nombreCampoRecurso = _getNombreCampoRecurso(tyupcodi);

    //recurso
    var idRecurcodi = objEdit != null ? objEdit.Recurcodi : 0;
    var recurnombre = objEdit != null ? objEdit.Recurnombre : 0;

    var htmlcbFormRecurso = '';
    if (objEdit == null) {
        htmlcbFormRecurso += `<select id='cbFormRecurso'>`;
        htmlcbFormRecurso += `<option value='0'>--SELECCIONE--</option>`;
        for (var i = 0; i < LISTA_RECURSO.length; i++) {
            var v = LISTA_RECURSO[i];
            var esSelected = v.Recurcodi == idRecurcodi ? ' selected ' : '';
            htmlcbFormRecurso += `<option value='${v.Recurcodi}' ${esSelected}>${v.Recurcodi} - ${v.Recurnombre}</option>`;
        }
        htmlcbFormRecurso += `</select>`;
    } else {
        htmlcbFormRecurso += `<input type='text' value='${recurnombre}' style="width: 250px" disabled /> `;
        htmlcbFormRecurso += `<input type='hidden' id='cbFormRecurso' value='${idRecurcodi}' /> `;
    }

    //html tabla Caudal
    var listaConfig = objEdit != null ? objEdit.ListaPto : [];
    var htmlTablaAgrup = generarHtmlTablaAgrupCentral(idRecurcodi, listaConfig);

    //Div general
    var html = `
    <div class="content-registro" style="width:auto">

        <div style="clear:both; height:10px;"></div>

        <table cellpadding="8" style="width:auto">
            <tbody>
                <tr>
                    <td class="registro-label" style="width:120px;">${nombreCampoRecurso}:</td>
                    <td class="registro-control" style="width:300px;">
                        ${htmlcbFormRecurso}
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class='td02'>
                        ${htmlTablaAgrup}
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

function generarHtmlTablaAgrupCentral(recurcodi, listaConfig) {
    //crear tabla con la primera fila default
    var htmlTrAgrupDefault = generarHtmlTrAgrupacionCentral(recurcodi, CORRELATIVO_AGRUP, listaConfig);

    var html = `
    <table id='table_central_agrup_${recurcodi}' class='table_central_agrup' style="width: 100%;">
        <thead>
            <tr>
                <th class='th1'>Punto de medición</th>
            </tr>
        </thead>
        <tbody class='tbody_table_central_agrup'>
            ${htmlTrAgrupDefault}
        </tbody>
    </table>
    `;

    return html;
}

function generarHtmlTrAgrupacionCentral(recurcodi, correlativoAgrup, listaConfig) {

    //punto de medicion
    var htmlcbFormFuente = '';
    var listaPtoIDCC = LISTA_PUNTO_MEDICION;
    for (var i = 0; i < listaPtoIDCC.length; i++) {
        var v = listaPtoIDCC[i];
        htmlcbFormFuente += `<option value='${v.Ptomedicodi}'>${v.Ptomedicodi} - ${v.Ptomedielenomb}</option>`;
    }

    //html trs
    var htmlTr = '';
    var listaConfXAgrup = listaConfig;
    for (var i = 0; i < listaConfXAgrup.length; i++) {
        var objConfig = listaConfXAgrup[i];
        htmlTr += generarHtmlTrAgrupConf(recurcodi, correlativoAgrup, objConfig.Ptomedicodi, objConfig.Ycdetfactor);
    }

    //
    var html = `
            <tr id='tr_table_agrup_${recurcodi}_${correlativoAgrup}' class='tr_table_central_agrup'>
                <td class='td_agrup' style="padding: 10px;">

                    <table style="width: 100%;">
                        <tbody id='tbody_fuente_${recurcodi}_${correlativoAgrup}'>
                            <tr>
                                <td colspan="3" style="padding: 10px;text-align: left;">
                                    <select name='cbFormFuente'>
                                        <option value='0'>--SELECCIONE--</option>
                                        ${htmlcbFormFuente}
                                    </select>
                                    <a title="Agregar registro" href="JavaScript:addFuenteToAgrupacionTrCentral(${recurcodi}, ${correlativoAgrup});">  ${IMG_AGREGAR} </a>
                                </td>
                            </tr>
                            ${htmlTr}
                        </tbody>
                    </table>

                </td>
                <td></td>
            </tr>
    `;

    CORRELATIVO_AGRUP++;

    return html;
}

function addFuenteToAgrupacionTrCentral(recurcodi, correlativoAgrup) {
    var idTbody = `tbody_fuente_${recurcodi}_${correlativoAgrup}`;
    var idSelect = `#${idTbody} select[name='cbFormFuente']`;
    var codigoFila = parseInt($(idSelect).val()) || 0;

    if (codigoFila > 0) {
        var htmlTr = generarHtmlTrAgrupConf(recurcodi, correlativoAgrup, codigoFila, '1.0');

        $("#" + idTbody).append(htmlTr);

        //combo para agregar filas de configuracion
        $('select[name^="cbFormFuente"]').multipleSelect({
            width: '220px',
            filter: true,
            single: true,
            onClose: function () {
            }
        });
        $('select[name^="cbFormFuente"]').multipleSelect("setSelects", [0]);

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
}

function generarHtmlTrAgrupConf(recurcodi, correlativoAgrup, codigoFila, factor) {

    var desc = '';
    var regPto = obtenerObjPto(codigoFila, LISTA_PUNTO_MEDICION);
    if (regPto != null) desc = codigoFila + ' - ' + regPto.Ptomedielenomb;

    var htmlTr = `
    <tr id='tr_agrup_conf_${recurcodi}_${correlativoAgrup}_${codigoFila}'>
        <td style='width: 10px;' class='tr_conf_td01'>
            <a title="Eliminar registro" href="JavaScript:quitFuenteToAgrupacionTrCentral(${recurcodi},${correlativoAgrup}, ${codigoFila});">  ${IMG_ELIMINAR} </a>
        </td>
        <td style='' class='tr_conf_td02'>${desc}</td>

        <td style='width: 40px;' class='tr_conf_td03'>
            <input type="text" name='factor' value="${factor}" style="width: 30px" >
        </td>
    </tr>
    `;

    return htmlTr;
}

function quitFuenteToAgrupacionTrCentral(recurcodi, correlativoAgrup, codigoFila) {
    $(`#tr_agrup_conf_${recurcodi}_${correlativoAgrup}_${codigoFila}`).remove();
}

//
function obtenerObjPto(ptomedicodi, listaObjPto) {

    for (var i = 0; i < listaObjPto.length; i++) {
        if (listaObjPto[i].Ptomedicodi == ptomedicodi)
            return listaObjPto[i];
    }

    return null;
}
function obtenerObjRecurso(recurcodi, listaObjRecurso) {

    for (var i = 0; i < listaObjRecurso.length; i++) {
        if (listaObjRecurso[i].Recurcodi == recurcodi)
            return listaObjRecurso[i];
    }

    return null;
}

function _mostrarMensajeAlertaTemporal(esExito, mensaje) {
    $("#mensaje").hide();
    $("#mensaje").show();

    if (esExito)
        $("#mensaje").html(`<div class='action-exito ' style='margin: 0; padding-top: 10px; padding-bottom: 10px;'>${mensaje}</div>`);
    else
        $("#mensaje").html(`<div class='action-error ' style='margin: 0; padding-top: 10px; padding-bottom: 10px;'>${mensaje}</div>`);
    setTimeout(function () { $("#mensaje").fadeOut(1000) }, 2000);
}