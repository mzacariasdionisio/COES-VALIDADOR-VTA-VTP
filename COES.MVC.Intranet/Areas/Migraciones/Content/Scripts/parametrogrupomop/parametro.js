var controlador = siteRoot + 'migraciones/parametro/';
var IMG_EDITAR = '<img src="' + siteRoot + 'Content/Images/btn-edit.png" alt="" width="19" height="19" style="">';
var IMG_ELIMINAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="" width="19" height="19" style="">';
var IMG_AGREGAR = '<img src="' + siteRoot + 'Content/Images/btn-add.png" alt="" width="19" height="19" style="">';
var IMG_INFORMATIVO = '<img src="' + siteRoot + 'Content/Images/ico-info.gif" alt="" width="16" height="16" style="">';

var ancho = 900;
var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ACTUAL = 0;
var FECHA_LIMITE = "";
var REPCODI = "";

var CORRELATIVO_AGRUP = 0;
var LISTA_EQUIPO_TERMOELECTRICO = [];
var LISTA_RELACION_EQUIPO_MODO = [];
var CATECODI_MODO_OPERACION = 2;

const COLOR_BAJA = "#FFDDDD";
const IMG_VER = '<img src="' + siteRoot + 'Content/Images/btn-open.png" alt="Ver Envío" width="19" height="19" style="">';
const IMG_ACTIVAR = '<img src="' + siteRoot + 'Content/Images/btn-ok.png" alt="Activar Envío" width="19" height="19" style="">';
var listaProyectosGrupoMemoria = [];

var GRUPOPADRE = -2;

$(function () {
    $('#cntMenu').css("display", "none");

    REPCODI = $('#hfRepCodi').val();
    FECHA_LIMITE = $('#hfRepFecLimite').val() || '';

    $('#cbEmpresa').change(function () {
        buscarElementos();
    });

    $('#cbCategoria').change(function () {
        $('#check_rsrvfria').prop('checked', false);
        $('#check_nodo').prop('checked', false);
        buscarElementos();
    });

    $('#check_estado_todos').change(function () {
        buscarElementos();
    });

    $('#btnConsultar').on('click', function () {
        buscarElementos();
    });
    $('#btnNuevo').on('click', function () {
        var objGrupo = obtenerPrGrupoDefault();
        cargarFormularioGrupo(1, objGrupo, []);
    });
    $('select[name^="cbFormNE"]').multipleSelect({
        width: '440px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });
    $('#btnGuardarGrupo').on('click', function () {
        guardarFormulario();
    });

    ancho = $('#mainLayout').width() - 50;

    $('#tab-container').easytabs({
        animate: false
    });

    inicializarWeb();

    listaEquipoTermoelectrica();

    $('#btnCrearAgrup').on('click', function () {
        nuevaAgrupacion();
    });

    $('#btnRegresar').on('click', function () {
        regresar(REPCODI);
    });
});

function regresar(repCodi) {
    location.href = siteRoot + 'Despacho/' + "CostosVariables/ViewCostoVariable?repCodi=" + repCodi;

}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Grupo
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function cargarFormularioGrupo(accion, objGrupo, listaRelacionEquipo) {
    $("#tr_fila_lbl_codigo").hide();
    $("#tr_datos_auditoria").hide();
    $('#trFormGrupoPadre').hide();
    $("#cbFormNECatecodi").removeAttr('disabled');
    if (objGrupo.Grupocodi > 0) {
        $("#tr_fila_lbl_codigo").show();
        $("#lbl_codigo_NEGrupo").html(objGrupo.Grupocodi);
        $("#cbFormNECatecodi").prop('disabled', 'disabled');

        $("#tr_datos_auditoria").show();
        $("#txt_usuario_creacion").html(objGrupo.Grupousucreacion);
        $("#txt_fecha_creacion").html(objGrupo.GrupofeccreacionDesc);
        $("#txt_usuario_modificacion").html(objGrupo.Grupousumodificacion);
        $("#txt_fecha_modificacion").html(objGrupo.GrupofecmodificacionDesc);
    }
    $("#hdFormNEGrupocodi").val(objGrupo.Grupocodi);
    $("#txtFormNEGruponomb").val(objGrupo.Gruponomb);
    $("#txtFormNEGrupoabrev").val(objGrupo.Grupoabrev);

    $('#cbFormNEEmprcodi').multipleSelect("setSelects", [objGrupo.Emprcodi]);
    $('#cbFormNEAreacodi').multipleSelect("setSelects", [objGrupo.Areacodi]);

    $('#cbFormNECatecodi').unbind();
    $("#cbFormNECatecodi").val(objGrupo.Catecodi);
    $('#cbFormNECatecodi').change(function () {
        var catecodi = parseInt($("#cbFormNECatecodi").val());

        $(".tr_combo_tipo_modo_op").hide();
        $("#div_unidades_relacionadas").html('');

        if (CATECODI_MODO_OPERACION == catecodi) {
            $(".tr_combo_tipo_modo_op").show();
            var htmlForm = dibujarHtmlFormularioRelacion(0, []);
            $("#div_unidades_relacionadas").html(htmlForm);

            //combo para agregar filas de configuracion
            $("#cbFormRecurso").unbind();
            $('select[name^="cbFormFuente"]').multipleSelect({
                width: '330px',
                filter: true,
                single: true,
                onClose: function () {
                }
            });
            $('select[name^="cbFormFuente"]').multipleSelect("setSelects", [0]);
        }

        cargarGrupos();
    });

    objGrupo.Grupopadre != null ? $('#cbFormNEGrupoPadreCodi').multipleSelect("setSelects", [objGrupo.Grupopadre]) : $('#cbFormNEGrupoPadreCodi').multipleSelect("setSelects", [GRUPOPADRE]);
    if (objGrupo.Grupopadre != undefined && objGrupo.Grupopadre != null) GRUPOPADRE = objGrupo.Grupopadre;
    else GRUPOPADRE = -2;

    cargarGrupos();

    $(".tr_combo_tipo_modo_op").hide();
    $("#cbFormNEGrupotipomodo").val("NN")
    if (CATECODI_MODO_OPERACION == objGrupo.Catecodi) {
        $(".tr_combo_tipo_modo_op").show();
        $("#cbFormNEGrupotipomodo").val(objGrupo.Grupotipomodo ?? "NN")
    }

    $("#checkFormNEGrupointegrante").prop('checked', objGrupo.Grupointegrante == 'S');
    $("#checkFormNETipoGenerRer").prop('checked', objGrupo.TipoGenerRer == 'S');
    $("#checkFormNEGrupotipocogen").prop('checked', objGrupo.Grupotipocogen == 'S');
    $("#checkFormNEGruponodoenergetico").prop('checked', objGrupo.Gruponodoenergetico == 1);
    $("#checkFormNEGruporeservafria").prop('checked', objGrupo.Gruporeservafria == 1);

    $("#cbFormNEFenergcodi").val(objGrupo.Fenergcodi);
    $("#txtFormNEOsinergcodi").val(objGrupo.Osinergcodi);
    $("#cbFormNEGrupoEstado").val(objGrupo.GrupoEstado);
    $("#cbFormNEGrupoactivo").val(objGrupo.Grupoactivo);

    $("#div_unidades_relacionadas").html('');
    if (CATECODI_MODO_OPERACION == objGrupo.Catecodi) {
        var htmlForm = dibujarHtmlFormularioRelacion(objGrupo.Grupocodi, listaRelacionEquipo);
        $("#div_unidades_relacionadas").html(htmlForm);
    }

    //combo para agregar filas de configuracion
    $("#cbFormRecurso").unbind();
    $('select[name^="cbFormFuente"]').multipleSelect({
        width: '330px',
        filter: true,
        single: true,
        onClose: function () {
        }
    });
    $('select[name^="cbFormFuente"]').multipleSelect("setSelects", [0]);

    if (accion == 1) //Si es nuevo
        $("#cbFormNEGrupoEstado").val("P"); //Por defecto Proyecto
    setTimeout(function () {
        $('#popupFormGrupo').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 250);
}

function generarJsonPrGrupo() {

    var grupocodi = parseInt($("#hdFormNEGrupocodi").val());
    var gruponomb = $("#txtFormNEGruponomb").val();
    var grupoabrev = $("#txtFormNEGrupoabrev").val();

    var emprcodi = parseInt($("#cbFormNEEmprcodi").val());
    var grupopadre = parseInt($("#cbFormNEGrupoPadreCodi").val());
    grupopadre = (grupopadre === -2) ? null : grupopadre;

    var areacodi = parseInt($("#cbFormNEAreacodi").val());

    var catecodi = parseInt($("#cbFormNECatecodi").val());
    var grupotipomodo = $("#cbFormNEGrupotipomodo").val();

    var grupointegrante = 'N';
    if ($("#checkFormNEGrupointegrante").is(':checked')) grupointegrante = 'S';

    var tipogenerrer = 'N';
    if ($("#checkFormNETipoGenerRer").is(':checked')) tipogenerrer = 'S';

    var grupotipocogen = 'N';
    if ($("#checkFormNEGrupotipocogen").is(':checked')) grupotipocogen = 'S';

    var gruponodoenergetico = 0;
    if ($("#checkFormNEGruponodoenergetico").is(':checked')) gruponodoenergetico = 1;

    var gruporeservafria = 0;
    if ($("#checkFormNEGruporeservafria").is(':checked')) gruporeservafria = 1;

    var fenergcodi = parseInt($("#cbFormNEFenergcodi").val());
    var osinergcodi = $("#txtFormNEOsinergcodi").val();

    var grupoestado = $("#cbFormNEGrupoEstado").val();
    var grupoactivo = $("#cbFormNEGrupoactivo").val();

    var obj = {
        Grupocodi: grupocodi,
        Gruponomb: gruponomb.trim(),
        Grupoabrev: grupoabrev.trim(),

        Emprcodi: emprcodi,
        Grupopadre: grupopadre,
        Areacodi: areacodi,

        Catecodi: catecodi,

        Grupointegrante: grupointegrante,
        TipoGenerRer: tipogenerrer,
        Grupotipocogen: grupotipocogen,
        Gruponodoenergetico: gruponodoenergetico,
        Gruporeservafria: gruporeservafria,

        Fenergcodi: fenergcodi,
        Osinergcodi: osinergcodi,

        GrupoEstado: grupoestado,
        Grupoactivo: grupoactivo,
        Grupotipomodo: grupotipomodo
    };

    return obj;
}

function generarJsonConfiguracion() {
    var listaConf = [];

    $(`tr[id^="tr_agrup_conf_"]`).each(function (i, obj) {
        var idHtml = $(obj).attr('id');

        var equicodi = parseInt(idHtml.split("_")[5]) || 0;

        listaConf.push(equicodi);
    });

    return listaConf;
}

function obtenerPrGrupoDefault() {
    var obj = {
        Grupocodi: 0,
        Gruponomb: '',
        Grupoabrev: '',

        Emprcodi: -1,
        Areacodi: -1,

        Catecodi: -1,

        Grupointegrante: 'N',
        TipoGenerRer: 'N',
        Grupotipocogen: 'N',
        Gruponodoenergetico: 0,
        Gruporeservafria: 0,

        Fenergcodi: -1,
        Osinergcodi: '',

        GrupoEstado: 'A',
        Grupoactivo: 'S',
    };

    return obj;
}

function guardarFormulario() {
    var objGrupo = generarJsonPrGrupo();
    var listaEquipoRel = generarJsonConfiguracion();

    var msj = validarJsonPrGrupo(objGrupo);

    if (msj == '') {
        $.ajax({
            type: 'POST',
            url: controlador + "RegistrarPrGrupo",
            data: {
                stringJson: JSON.stringify(objGrupo),
                equicodis: listaEquipoRel.join(','),
            },
            cache: false,
            success: function (model) {
                if (model.Resultado != "-1") {
                    alert("Se guardó correctamente el registro");
                    $('#popupFormGrupo').bPopup().close();
                    buscarElementos();
                } else {
                    alert('Ha ocurrido un error:' + model.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    } else {
        alert(msj);
    }
}

function validarJsonPrGrupo(obj) {
    var msj = '';

    if (obj.Catecodi <= 0)
        msj += "No ha seleccionado categoría. " + "\n";

    if (obj.Gruponomb == null || obj.Gruponomb.length <= 0)
        msj += "No ha ingresado nombre de grupo. " + "\n";

    return msj;
}

function editarPrGrupo(grupocodi) {

    $.ajax({
        type: "POST",
        url: controlador + "ObtenerPrGrupo",
        data: {
            grupocodi: grupocodi
        },
        global: false,
        success: function (evt) {
            if (evt.Resultado == '-1') {
                alert('Ha ocurrido un error:' + evt.Mensaje);
            } else {
                cargarFormularioGrupo(2, evt.Grupo, evt.ListaRelacionEquipo);
            }
        },
        error: function (req, status, error) {
            alert("Ha ocurrido un error.");
        }
    });
}


function listaEquipoTermoelectrica() {

    $.ajax({
        type: "POST",
        url: controlador + "ListaEquipoTermoelectrico",
        data: {
        },
        global: false,
        success: function (evt) {
            if (evt.Resultado == '-1') {
            } else {
                LISTA_EQUIPO_TERMOELECTRICO = evt.ListaEquipo;
                LISTA_RELACION_EQUIPO_MODO = evt.ListaRelacionEquipo;
            }
        },
        error: function (req, status, error) {
            alert("Ha ocurrido un error.");
        }
    });
}

function dibujarHtmlFormularioRelacion(grupocodi, listaRelacionEquipo) {
    //html tabla Caudal
    var htmlTablaAgrup = generarHtmlTablaAgrupCentral(grupocodi, listaRelacionEquipo);

    //Div general
    var html = `
<t
        <table cellpadding="8" style="width:auto">
            <tbody>
                <tr>
                    <td colspan="2" class='td02'>
                        ${htmlTablaAgrup}
                    </td>
                </tr>
            </tbody>
        </table>
`;

    return html;
}

function generarHtmlTablaAgrupCentral(grupocodi, listaRelacionEquipo) {
    //crear tabla con la primera fila default
    var htmlTrAgrupDefault = generarHtmlTrAgrupacionCentral(grupocodi, CORRELATIVO_AGRUP, listaRelacionEquipo);

    var html = `
    <table id='table_central_agrup_${grupocodi}' class='table_central_agrup' style="width: auto;">
        <thead>
            <tr>
                <th class='th1'>Unidades de generación relacionadas</th>
            </tr>
        </thead>
        <tbody class='tbody_table_central_agrup'>
            ${htmlTrAgrupDefault}
        </tbody>
    </table>
    `;

    return html;
}

function generarHtmlTrAgrupacionCentral(grupocodi, correlativoAgrup, listaConfig) {

    //punto de medicion
    var htmlcbFormFuente = '';
    var listaEquipo = LISTA_EQUIPO_TERMOELECTRICO;
    for (var i = 0; i < listaEquipo.length; i++) {
        var v = listaEquipo[i];

        htmlcbFormFuente += `<option value='${v.Equicodi}'>${v.Tareaabrev} ${v.Areanomb} - ${v.Equiabrev} (${v.EstadoDesc})</option>`;
    }

    //html trs
    var htmlTr = '';
    var listaConfXAgrup = listaConfig;
    for (var i = 0; i < listaConfXAgrup.length; i++) {
        var objConfig = listaConfXAgrup[i];
        htmlTr += generarHtmlTrAgrupConf(grupocodi, correlativoAgrup, objConfig.Equicodi);
    }

    //
    var html = `
            <tr id='tr_table_agrup_${grupocodi}_${correlativoAgrup}' class='tr_table_central_agrup'>
                <td class='td_agrup' style="padding: 10px;">

                    <table style="width: 100%;">
                        <tbody id='tbody_fuente_${grupocodi}_${correlativoAgrup}'>
                            <tr>
                                <td colspan="3" style="padding: 10px;text-align: left;">
                                    <select name='cbFormFuente'>
                                        <option value='0'>--SELECCIONE--</option>
                                        ${htmlcbFormFuente}
                                    </select>
                                </td>
                                <td>
                                    <a title="Agregar registro" href="JavaScript:addFuenteToAgrupacionTrCentral(${grupocodi}, ${correlativoAgrup});">  ${IMG_AGREGAR} </a>
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

function addFuenteToAgrupacionTrCentral(grupocodi, correlativoAgrup) {
    var idTbody = `tbody_fuente_${grupocodi}_${correlativoAgrup}`;
    var idSelect = `#${idTbody} select[name='cbFormFuente']`;
    var codigoFila = parseInt($(idSelect).val()) || 0;

    if (codigoFila > 0) {
        var htmlTr = generarHtmlTrAgrupConf(grupocodi, correlativoAgrup, codigoFila);

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
    }
}

function generarHtmlTrAgrupConf(grupocodi, correlativoAgrup, codigoFila) {

    var desc = '';
    var v = obtenerObjEq(codigoFila, LISTA_EQUIPO_TERMOELECTRICO);
    if (v != null) desc = `${v.Tareaabrev} ${v.Areanomb} - ${v.Equiabrev} (${v.EstadoDesc})`;

    var htmlTr = `
    <tr id='tr_agrup_conf_${grupocodi}_${correlativoAgrup}_${codigoFila}'>
        <td style='width: 10px;' class='tr_conf_td01'>
            <a title="Eliminar registro" href="JavaScript:quitFuenteToAgrupacionTrCentral(${grupocodi},${correlativoAgrup}, ${codigoFila});">  ${IMG_ELIMINAR} </a>
        </td>
        <td style='' class='tr_conf_td02'>${desc}</td>

    </tr>
    `;

    return htmlTr;
}

function quitFuenteToAgrupacionTrCentral(grupocodi, correlativoAgrup, codigoFila) {
    $(`#tr_agrup_conf_${grupocodi}_${correlativoAgrup}_${codigoFila}`).remove();
}

//
function obtenerObjEq(equicodi, listaObjEq) {

    for (var i = 0; i < listaObjEq.length; i++) {
        if (listaObjEq[i].Equicodi == equicodi)
            return listaObjEq[i];
    }

    return null;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Modos de operacion
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function buscarElementos() {
    $('#textoMensaje').css("display", "none");
    $("#paginado").html('');
    $("#listado").html('');
    $("#listadoParam").html('');

    var msj = "";

    if (msj == "") {
        //$("#tab-container").easytabs({ updateHash: true, defaultTab: "li:eq(1)" });
        $('#tab-container').easytabs('select', '#vistaModoOperacion');
        cargarParametrosGenerales(FECHA_LIMITE);

        pintarPaginado();
        pintarBusqueda(1);
    } else {
        $('#textoMensaje').css("display", "block");
        $('#textoMensaje').removeClass();
        $('#textoMensaje').addClass('action-alert');
        $('#textoMensaje').text(msj);
    }
}

function pintarPaginado() {
    $.ajax({
        type: 'POST',
        url: controlador + "GrupoPaginado",
        data: {
            emprcodi: getCodigoEmpresa(),
            catecodi: getCodigoCategoria(),
            nombre: getNombre(),
            estado: getEstado(),
            esReservaFria: getEsReservaFria(),
            esNodoEnergetico: getEsNodoEnergetico()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function (err) {
            mostrarError();
        }
    });
}

function pintarBusqueda(nroPagina) {
    ancho = $('#mainLayout').width() - 50;

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "GrupoListado",
        data: {
            emprcodi: getCodigoEmpresa(),
            catecodi: getCodigoCategoria(),
            estado: getEstado(),
            nombre: getNombre(),
            nroPagina: $("#hfNroPagina").val(),
            esReservaFria: getEsReservaFria(),
            esNodoEnergetico: getEsNodoEnergetico()
        },
        success: function (evt) {
            $('#listado').css("width", ancho + "px");
            $('#listado').html(evt);

            $('#tabla_modo').dataTable({
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": -1
            });
        },
        error: function (err) {
            mostrarError();
        }
    });
}

function inicializarWeb() {

    var grupocodi = parseInt($("#hdCCGrupocodi").val()) || 0;
    var fechaConsulta = $("#hdCCFecha").val();
    var agrupcodi = parseInt($("#hdCCAgrupcodi").val()) || 0;

    if (grupocodi > 0 && agrupcodi > 0) {
        cargarParametrosGenerales(fechaConsulta);
        verDetalle(grupocodi);
    } else {
        buscarElementos();
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Detalle del modo
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function verDetalle(grupocodi, tipoFiltro) {

    var fechaConsulta = null;
    var agrupcodi = 0;
    var esConsultado = parseInt($("#hdCCEsConsultado").val()) || 0;

    if (esConsultado == 0) {
        fechaConsulta = $("#hdCCFecha").val();
        agrupcodi = parseInt($("#hdCCAgrupcodi").val()) || 0;

        $("#hdCCEsConsultado").val(1);
    }

    $.ajax({
        type: 'POST',
        url: controlador + "GrupoMopDetalle",
        data: {
            grupocodi: grupocodi,
            repcodi: REPCODI,
            idAgrup: agrupcodi,
            fechaConsulta: fechaConsulta
        },
        success: function (evt) {
            $('#textoMensaje').css("display", "none");
            $('#listadoParam').css("width", ancho + "px");
            $('#listadoParam').html(evt);

            $('#txtFechaData').Zebra_DatePicker({
                direction: [FECHA_LIMITE, FECHA_LIMITE],
                onSelect: function () {
                    cargarListaDataXModo();
                }
            });
            $("#cbUnidad").unbind();
            $('#cbUnidad').change(function () {
                cargarListaDataXModo();
            });
            $("#cbAgrupacion").unbind();
            $('#cbAgrupacion').change(function () {
                cargarListaDataXModo();
            });

            $('input[type=radio][name=rbFiltro]').change(function () {
                cargarListaDataXModo();
            });

            //>>>>>>>>> Eventos exportacion importacion parametrosMop >>>>>>>>>>>>>>>>>>>>
            $("#btnExportarReporte").unbind();
            $("#btnImportarReporte").unbind();
            $("#btnCancelarExportacion").unbind();

            $('#btnExportarReporte').click(function () {

                setTimeout(function () {
                    $("#popupExportarReporte").bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);
            });

            $('#btnExportarReporte2').unbind();
            $('#btnExportarReporte2').click(function () {
                var opcion = $("#tipoExportar").val();
                if (opcion > 0) {
                    $('#exportacionTipo').val(opcion);
                    exportarReporte(opcion);
                }
                else {
                    alert("Debe seleccionar un tipo de exportación.");
                }
            });

            $('#btnCancelarExportacion').click(function () {
                $('#popupExportarReporte').bPopup().close();
            });

            $('#btnImportarReporte').click(function () {
                var repcodi = parseInt($("#hfRepCodi").val()) || 0;
                window.location.href = controlador + 'ParametrosMopMasivoImportacion' + (repcodi > 0 ? "?repcodi=" + repcodi : "");
            });
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

            $("#cbUnidadesEspeciales").unbind();
            $("#cbUnidadesEspeciales").empty();
            $("#cbUnidadesEspeciales").append('<option value="-1" selected="selected">[No Seleccionado]</option>');

            //$("#tab-container").easytabs({ updateHash: true, defaultTab: "li:eq(2)" });
            $('#tab-container').easytabs('select', '#vistaDetalle');

            cargarListaDataXModo(tipoFiltro);
        },
        error: function (err) {
            mostrarError();
        }
    });
}

exportarReporte = function (opcion) {
    $('#textoMensaje').css("display", "none");

    var grupocodi = parseInt($("#hfGrupocodi").val()) || 0;
    var fecha = $("#txtFechaData").val();
    var unidad = $("#cbUnidad").val();
    var agrp = $("#cbAgrupacion").val();
    //var filtroFicha = $('input[name=rbFiltro]:checked').val() || -1;

    $.ajax({
        type: 'POST',
        url: controlador + "ExportarReporteGrupo",
        //data: $('#frmBusquedaEquipo').serialize(),
        data: {
            grupocodi: grupocodi,
            strfecha: fecha,
            unidad: unidad,
            idAgrup: agrp,
            filtroFicha: opcion,
            repcodi: REPCODI
        },
        success: function (resultado) {
            if (resultado.Resultado != "-1") {
                $('#popupExportarReporte').bPopup().close();
                window.location = controlador + "AbrirArchivo?file=" + resultado.NombreArchivo;
            } else {
                $('#popupExportarReporte').bPopup().close();
                //mostrarError();
                $('#textoMensaje').css("display", "block");
                $('#textoMensaje').removeClass();
                $('#textoMensaje').addClass('action-alert');
                $('#textoMensaje').text(resultado.Mensaje);
            }
        },
        error: function () {
            $('#popupExportarReporte').bPopup().close();
            mostrarError();
        }
    });
};


function cargarListaDataXModo(tipoFiltro) {
    var grupocodi = parseInt($("#hfGrupocodi").val()) || 0;
    var fecha = $("#txtFechaData").val();
    var unidad = $("#cbUnidad").val();
    var agrp = $("#cbAgrupacion").val();
    var filtroFicha = $('input[name=rbFiltro]:checked').val() || -1;

    $.ajax({
        type: 'POST',
        url: controlador + "GrupoMopData",
        data: {
            grupocodi: grupocodi,
            strfecha: fecha,
            unidad: unidad,
            idAgrup: agrp,
            filtroFicha: filtroFicha,
            tipoFiltro: tipoFiltro
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#div_tabla_data').html('');
                var html = _dibujarTablaListadoGrupoMopData(evt);

                $('#div_tabla_data').css("width", ancho + "px");
                $('#div_tabla_data').html(html);

                $('#tabla_data').dataTable({
                    "sPaginationType": "full_numbers",
                    //"sDom": 'ftp',
                    "stripeClasses": [],
                    "ordering": true,
                    "iDisplayLength": 50
                });
            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarError();
        }
    });
}

function _dibujarTablaListadoGrupoMopData(model) {
    var lista = model.ListaGrupodat;

    var cadena = '';
    cadena += `
    <table id="tabla_data" border="0" class="pretty tabla-icono" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th style="width: 50px">Acciones</th>
                <th style="width: 150px">Código</th>
                <th style="width: 150px">Concepto</th>
                <th style="width: 150px">Nombre Ficha Técnica</th>
                <th style="width: 15px">Abreviatura </th>
                <th style="width: 50px">Unidad</th>
                <th style="width: 50px">Fecha de Vigencia</th>
                <th style="width: 50px">Fórmula</th>
                <th style="width: 30px; background: #d9f7dd;" title="Valor cero(0) correcto">
                    ${IMG_INFORMATIVO}
                </th>
                <th style="width: 150px">Comentario </th>
                <th style="width: 30px">Sustento</th>
                <th style="width: 120px">Usuario modificación</th>
                <th style="width: 120px">Fecha modificación</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in lista) {
        var item = lista[key];

        var htmlDescarga = getHtmlCeldaSustento(item.Gdatsustento);
        var htmlBtn = `
            <a onclick="verHistoricoConcepto('N',${item.Grupocodi},${item.Concepcodi}, '${item.GrupoNomb}', '${item.ConcepDesc}', ${item.Conceppropeq});" 
                    title="Ver Histórico" style="cursor: pointer;">
                    ${IMG_VER}
            </a>
        `;
        if (model.AccesoEditar) {
            htmlBtn += `
                <a onclick="editarDataConcepto('N',${item.Grupocodi},${item.Concepcodi}, '${item.GrupoNomb}', '${item.ConcepDesc}', ${item.Conceppropeq});" 
                                title="Modificación" style="cursor: pointer;">
                                ${IMG_EDITAR}
                </a>`;
        }
        var htmlChecked = item.Gdatcheckcero == 1 ? " checked " : "";

        cadena += `

            <tr>
                <td style="background-color: ${item.Color}">
                    ${htmlBtn}
                </td>
                <td style="color: #777; background: #e0e0e0;border-right: 1px solid #CCC;border-bottom: 1px solid #CCC;" class="categoria">${item.Concepcodi}</td>
                <td style="color: #777; background: #e0e0e0;border-right: 1px solid #CCC;border-bottom: 1px solid #CCC;" class="empresa">${item.ConcepDesc}</td>
                <td style="color: #777; background: #e0e0e0;border-right: 1px solid #CCC;border-bottom: 1px solid #CCC;" class="empresa">${item.Concepnombficha}</td>
                <td style="color: #777; background: #e0e0e0;border-right: 1px solid #CCC;border-bottom: 1px solid #CCC;" class="nombre">${item.Concepabrev}</td>
                <td style="color: #777; background: #e0e0e0;border-right: 1px solid #CCC;border-bottom: 1px solid #CCC;" class="abrev">${item.ConcepUni}</td>
                <td style="background-color: ${item.Color}" class="codigo">${item.FechadatDesc}</td>
                <td style="background-color: ${item.Color}" class="tipo">${item.Formuladat}</td>
                <td style="background-color: ${item.Color}">
                    <input type="checkbox" class="ChkSeleccion1" disabled="disabled" ${htmlChecked} value="${item.Gdatcheckcero}" />
                </td>
                <td style="background-color: ${item.Color}" class="tipo">${item.Gdatcomentario}</td>
                <td style="background-color: ${item.Color}">${htmlDescarga}</td>
                <td style="color: #777; background: #e0e0e0;border-right: 1px solid #CCC;border-bottom: 1px solid #CCC;" class="tipo">${item.Lastuser}</td>
                <td style="color: #777; background: #e0e0e0;border-right: 1px solid #CCC;border-bottom: 1px solid #CCC;" class="tipo">${item.FechaactDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}


//
function verHistoricoConcepto(tipoParametro, grupocodi, concepcodi, gruponomb, concepdesc, conceppropeq) {
    OPCION_ACTUAL = OPCION_VER;
    inicializarGrupoDat(OPCION_VER, tipoParametro, grupocodi, concepcodi, gruponomb, concepdesc, conceppropeq);
}

function editarDataConcepto(tipoParametro, grupocodi, concepcodi, gruponomb, concepdesc, conceppropeq) {
    OPCION_ACTUAL = OPCION_EDITAR;
    inicializarGrupoDat(OPCION_EDITAR, tipoParametro, grupocodi, concepcodi, gruponomb, concepdesc, conceppropeq);
}

function inicializarGrupoDat(opcion, tipoParametro, grupocodi, concepcodi, gruponomb, concepdesc, conceppropeq) {
    $(".unidades_especiales").hide();
    $("#hfGrupocodiDat").val(grupocodi);
    $("#hfConcepcodiDat").val(concepcodi);
    $("#formGrupoHistCnp").html(gruponomb);
    $("#formconceptoHistCnp").html(concepdesc);
    $('#listadoGrupoDat').html('');

    configurarFormularioGrupodatNuevo();

    if (tipoParametro == 'N') { //
        $(".tr_grupo_unidades").show();
    } else { //Parametros generales
        $(".tr_grupo_unidades").hide();
    }

    var arrayVisible = [];

    switch (opcion) {
        case OPCION_VER:
            $("#popupHistoricoConcepto .content-botonera").hide();
            $("#popupHistoricoConcepto .titulo_listado").hide();
            arrayVisible = [{ "targets": [0], "visible": false, }];
            if (conceppropeq != 0) {
                mostrarListaUnidades(arrayVisible, grupocodi, concepcodi, conceppropeq);
            }
            break;
        case OPCION_EDITAR:
            $("#popupHistoricoConcepto .content-botonera").show();
            if (conceppropeq != 0) {
                mostrarListaUnidades(arrayVisible, grupocodi, concepcodi, conceppropeq);
            }
            break;
        case OPCION_NUEVO:
            $("#popupHistoricoConcepto .content-botonera").hide()
            break;
    }

    $("#btnGrupodatNuevo").unbind();
    $('#btnGrupodatNuevo').click(function () {
        $("#btnGrupodatNuevo").hide();
        configurarFormularioGrupodatNuevo();

        if (FECHA_LIMITE != "") {//deshabilitar el input de fecha y setearlo con la fecha del reporte CV
            $("#fechaData").val(FECHA_LIMITE + ' 00:00:00');
            $("#fechaData").prop('disabled', 'disabled');
            $("#fechaData").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Disabled Zebra_DatePicker_Icon_Inside");
        }
    });

    $("#btnGrupodatGuardar").unbind();
    $('#btnGrupodatGuardar').click(function () {
        if (FECHA_LIMITE != "") {
            var fec = $('#fechaData').val().substring(0, 10);
            if (fec == FECHA_LIMITE) {
                guardarGrupodat(arrayVisible, tipoParametro);
            } else {
                alert("Error: La Fecha de Vigencia ingresada no es válida");
            }
        } else {
            guardarGrupodat(arrayVisible, tipoParametro);
        }
    });

    $("#btnGrupodatConsultar").unbind();
    $('#btnGrupodatConsultar').click(function () {
        $("#cbUnidadesEspeciales").val("-1");
        listarGrupodat(arrayVisible, grupocodi, concepcodi);
    });

    listarGrupodat(arrayVisible, grupocodi, concepcodi, conceppropeq);

    $('#fechaData').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#fechaData').Zebra_DatePicker({
        direction: [FECHA_LIMITE, FECHA_LIMITE],
        readonly_element: false,
        onSelect: function (date) {
            $('#fechaData').val(date + " 00:00:00");
        }
    });
}

function mostrarListaUnidades(arrayVisible, grupocodi, concepcodi, conceppropeq) {
    $("#cbUnidadesEspeciales").unbind();
    $("#cbUnidadesEspeciales").empty();
    $("#cbUnidadesEspeciales").append('<option value="-1" selected="selected">[No Seleccionado]</option>');

    var listaEquipo = obtenerListaEqModoBD(grupocodi, LISTA_RELACION_EQUIPO_MODO);

    if (listaEquipo.length > 0) {

        for (var c = 0; c < listaEquipo.length; c++) {
            $("#cbUnidadesEspeciales").append('<option value=' + listaEquipo[c].Equicodi + '>' + listaEquipo[c].Equinomb + '</option>');
        }

        $('#cbUnidadesEspeciales').change(function () {
            listarGrupodat(arrayVisible, grupocodi, concepcodi, conceppropeq, $('#cbUnidadesEspeciales').val());
        });

        $(".unidades_especiales").show();
    }
}
function listarGrupodat(arrayVisible, grupocodi, concepcodi, conceppropeq, equicodi) {
    $("#btnGrupodatNuevo").show();
    $("#formularioGrupodat").hide();
    equicodi = parseInt(equicodi) || -1;

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarHistorico',
        data: {
            grupocodi: grupocodi,
            concepcodi: concepcodi,
            equicodi: equicodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listadoGrupoDat').html('');

                var html = "";
                if (equicodi > 0)
                    html = _dibujarTablaListadoHistoricoGrupoequival(evt);
                else
                    html = _dibujarTablaListadoHistoricoGrupodat(evt);

                $('#listadoGrupoDat').html(html);

                $('#tablaListadoGrupodat').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "bInfo": false,
                    //"bLengthChange": false,
                    //"sDom": 't',
                    "ordering": false,
                    "order": [[2, "asc"]],
                    "iDisplayLength": 15,
                    "columnDefs": arrayVisible
                });

                setTimeout(function () {
                    $('#popupHistoricoConcepto').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 250);
            } else {
                alert(evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error: ' + err);
        }
    });
}

function _dibujarTablaListadoHistoricoGrupodat(model) {
    var lista = model.ListaGrupodat;

    var cadena = '';
    cadena += `
    <table id="tablaListadoGrupodat" border="0" class="pretty tabla-icono" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th style="width: 70px">Acciones</th>
                <th style="width: 80px">Fecha de Vigencia</th>
                <th style="width: 80px">Fórmula</th>
                <th style="width: 50px; background: #d9f7dd;" title="Valor cero(0) correcto">
                    ${IMG_INFORMATIVO}
                </th>
                <th style="width: 120px">Comentario</th>
                <th style="width: 50px">Sustento</th>
                <th style="width: 120px">Usuario modificación</th>
                <th style="width: 120px">Fecha modificación</th>
                <th style="width: 50px">Estado</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in lista) {
        var item = lista[key];

        var claseFila = "";
        if (item.Deleted != 0) { claseFila = "clase_eliminado"; }

        var htmlDescarga = getHtmlCeldaSustento(item.Gdatsustento);
        var htmlBtn = '';
        if (model.AccesoEditar) {
            htmlBtn = `
                <a onclick="editarGrupodat('${item.FechadatDesc}', '${item.Formuladat}', '${item.FechaactDesc}', '${item.Deleted}', '${item.Gdatcomentario}','${item.Gdatsustento}','${item.Gdatcheckcero}');" 
                        title="Editar Registro" style="cursor: pointer;">
                        ${IMG_EDITAR}
                </a>
            `;
            if (item.Deleted == 0) {
                htmlBtn += `
                <a onclick="eliminarGrupodat(${item.Grupocodi},${item.Concepcodi}, '${item.FechadatDesc}', '${item.Formuladat}', '${item.FechaactDesc}', '${item.Deleted}');" 
                                title="Eliminar Registro" style="cursor: pointer;">
                                ${IMG_ELIMINAR}
                </a>`;
            }
        }
        var htmlChecked = item.Gdatcheckcero == 1 ? " checked " : "";

        cadena += `

            <tr class="${claseFila}">
                <td>${htmlBtn}</td>
                <td>${item.FechadatDesc}</td>
                <td>${item.Formuladat}</td>
                <td><input type="checkbox" disabled="disabled" ${htmlChecked} value="${item.Gdatcheckcero}" /></td>
                <td>${item.Gdatcomentario}</td>
                <td>${htmlDescarga}</td>
                <td>${item.Lastuser}</td>
                <td>${item.FechaactDesc}</td>
                <td>${item.EstadoDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function _dibujarTablaListadoHistoricoGrupoequival(model) {
    var lista = model.ListaGrupoEquipoVal;

    var cadena = '';
    cadena += `
    <table id="tablaListadoGrupodat" border="0" class="pretty tabla-icono" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th style="width: 70px">Acciones</th>
                <th style="width: 80px">Fecha de Vigencia</th>
                <th style="width: 80px">Fórmula</th>
                <th style="width: 50px; background: #d9f7dd;" title="Valor cero(0) correcto">
                    ${IMG_INFORMATIVO}
                </th>
                <th style="width: 120px">Comentario</th>
                <th style="width: 50px">Sustento</th>
                <th style="width: 120px">Usuario modificación</th>
                <th style="width: 120px">Fecha modificación</th>
                <th style="width: 50px">Estado</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (var key in lista) {
        var item = lista[key];

        var claseFila = "";
        if (item.Greqvadeleted != 0) { claseFila = "clase_eliminado"; }

        var htmlDescarga = getHtmlCeldaSustento(item.Greqvasustento);
        var htmlBtn = '';
        if (model.AccesoEditar) {
            htmlBtn = `
                <a onclick="editarGrupodat('${item.GreqvafechadatDesc}', '${item.Greqvaformuladat}', '${item.FechaactDesc}', '${item.Greqvadeleted}', '${item.Greqvacomentario}','${item.Greqvasustento}','${item.Greqvacheckcero}');" 
                        title="Editar Registro" style="cursor: pointer;">
                        ${IMG_EDITAR}
                </a>
            `;
            if (item.Greqvadeleted == 0) {
                htmlBtn += `
                <a onclick="eliminarGrupodat(${item.Equicodi}, ${item.Grupocodi},${item.Concepcodi}, '${item.GreqvafechadatDesc}', '${item.Greqvaformuladat}', ${item.FechaactDesc}, '${item.Greqvadeleted}');" 
                                title="Eliminar Registro" style="cursor: pointer;">
                                ${IMG_ELIMINAR}
                </a>`;
            }
        }
        var htmlChecked = item.Gdatcheckcero == 1 ? " checked " : "";

        cadena += `

            <tr class="${claseFila}">
                <td>${htmlBtn}</td>
                <td>${item.GreqvafechadatDesc}</td>
                <td>${item.Greqvaformuladat}</td>
                <td><input type="checkbox" disabled="disabled" ${htmlChecked} value="${item.Greqvacheckcero}" /></td>
                <td>${item.Greqvacomentario}</td>
                <td>${htmlDescarga}</td>
                <td>${item.Lastuser}</td>
                <td>${item.FechaactDesc}</td>
                <td>${item.EstadoDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

function configurarFormularioGrupodatNuevo() {
    OPCION_ACTUAL = OPCION_NUEVO;
    $("#formularioGrupodat").show();
    $("#fechaAct").val($("#hfFechaAct").val());
    $("#fechaData").val($("#hfFechaAct").val());
    $("#valorData").val('');
    $("#hfDeleted").val(0);

    $("#trValorcero").hide();
    $("#Gdatcomentario").val('');
    $("#Gdatsustento").val('');

    $("#btnGrupodatGuardar").val("Agregar");
    $("#formularioGrupodat .popup-title span").html("Nuevo Registro");
    $("#fechaData").removeAttr('disabled');
    $("#fechaData").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Inside");

    CargarEvent();
}

function CargarEvent() {

    $('#valorData').change(function () {
        var codEquipo = parseInt($("#cbUnidadesEspeciales").val()) || 0;
        if ($("#valorData").val() == "0") {
            $("#trValorcero").show();
            $("#ChkValorCeroCorrecto").prop('checked', false);
        }
        else {
            $("#ChkValorCeroCorrecto").prop('checked', false);
            $("#trValorcero").hide();
        }
    });
};

function editarGrupodat(FechadatDesc, Formuladat, FechaactDesc, deleted, comentario, sustento, checkcero) {
    OPCION_ACTUAL = OPCION_EDITAR;

    $("#formularioGrupodat").show();
    $("#fechaAct").val(FechaactDesc);
    $("#fechaData").val(FechadatDesc);
    $("#valorData").val(Formuladat);

    codequipo = parseInt($("#cbUnidadesEspeciales").val());
    if ($("#valorData").val() == "0") {
        $("#trValorcero").show();
        if (checkcero == "1")
            $("#ChkValorCeroCorrecto").prop('checked', true);
        else
            $("#ChkValorCeroCorrecto").prop('checked', false);
    } else {
        $("#trValorcero").hide();
        $("#ChkValorCeroCorrecto").prop('checked', false);
    }
    $("#Gdatcomentario").val(comentario);
    $("#Gdatsustento").val(sustento);

    $("#hfDeleted").val(deleted);
    $("#btnGrupodatGuardar").val("Actualizar");
    $("#formularioGrupodat .popup-title span").html("Modificación de Registro");
    $("#fechaData").prop('disabled', 'disabled');
    $("#fechaData").parent().find("button").prop("class", "Zebra_DatePicker_Icon Zebra_DatePicker_Icon_Disabled Zebra_DatePicker_Icon_Inside");

    CargarEvent();
}

function eliminarGrupodat(grupocodi, concepcodi, FechadatDesc, Formuladat, FechaactDesc, deleted) {
    if (confirm('¿Desea eliminar el registro?')) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'GrupodatEliminar',
            data: {
                grupocodi: grupocodi,
                concepcodi: concepcodi,
                strfechaDat: FechadatDesc,
                deleted: deleted
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se eliminó correctamente el registro");
                    listarGrupodat([], grupocodi, concepcodi);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function eliminaGrupoEquipoVal(equicodi, grupocodi, concepcodi, FechadatDesc, Formuladat, FechaactDesc, deleted) {
    if (confirm('¿Desea eliminar el registro?')) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            traditional: true,
            url: controlador + 'GrupoEquipoValEliminar',
            data: {
                grupocodi: grupocodi,
                equicodi: equicodi,
                concepcodi: concepcodi,
                strfechaDat: FechadatDesc,
                deleted: deleted
            },
            cache: false,
            success: function (result) {
                if (result.Resultado == '-1') {
                    alert('Ha ocurrido un error:' + result.Mensaje);
                } else {
                    alert("Se eliminó correctamente el registro");
                    listarGrupodat([], grupocodi, concepcodi, 0, equicodi);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function guardarGrupodat(arrayVisible, tipoParametro) {
    var entity = getObjetoGrupodatJson();

    var msge = validarDatos(entity);
    if (msge == "") {
        if (confirm('¿Desea guardar el registro?')) {
            var msj = validarGrupodat(entity, tipoParametro);
            var jsonResult = entity.equicodi > 0 ? "GrupoEquipoValGuardar" : "GrupodatGuardar";

            if (msj == "") {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    traditional: true,
                    url: controlador + jsonResult,
                    data: {
                        tipoAccion: OPCION_ACTUAL,
                        grupocodi: entity.grupocodi,
                        concepcodi: entity.concepcodi,
                        strfechaDat: entity.fechaData,
                        formuladat: entity.valorData,
                        deleted: entity.deleted,
                        equicodi: entity.equicodi,
                        tipoParametro: tipoParametro,
                        gdatcheckcero: entity.gdatcheckcero,
                        gdatcomentario: entity.gdatcomentario,
                        gdatsustento: entity.gdatsustento,
                        repcodi: REPCODI
                    },
                    cache: false,
                    success: function (result) {
                        if (result.Resultado == '-1') {
                            alert('Ha ocurrido un error:' + result.Mensaje);
                        } else {
                            alert("Se guardó correctamente el registro");
                            listarGrupodat(arrayVisible, entity.grupocodi, entity.concepcodi, 0, entity.equicodi);

                            if (entity.grupocodi > 0)
                                cargarListaDataXModo();
                            else
                                cargarListaParamGral();
                        }
                    },
                    error: function (err) {
                        alert('Ha ocurrido un error.');
                    }
                });
            } else {
                alert(msj);
            }
        }
    } else {
        alert(msge);
    }
}

function validarDatos(datos) {
    var cad = "";

    var fecha = datos.fechaData;

    if (fecha.includes("d") || fecha.includes("m") || fecha.includes("y") || fecha.includes("h") || fecha.includes("s"))
        cad = cad + "Error: El formato de fecha es incorrecto."
    return cad;
}

function validarGrupodat(obj, tipoParametro) {
    var msj = "";

    if (tipoParametro == 'N' && obj.grupocodi <= 0) {
        msj += "Debe seleccionar un grupo";
    }
    if (obj.concepcodi <= 0) {
        msj += "Debe seleccionar un concepto";
    }
    if (obj.fechaData == null || obj.fechaData == '' || obj.fechaData.length != 19) {
        msj += "Debe seleccionar una fecha";
    }
    if (obj.valorData == null || obj.valorData.length > 100) {
        msj += "El Valor debe ser menor de 100 caracteres";
    }

    if (obj.gdatsustento != null && obj.gdatsustento != "") {
        if (obj.gdatsustento.length > 400) {
            msj += "El Sustento no debe exceder los 400 caracteres, actualmente contiene " + obj.gdatsustento.length + " caracteres.";
        }
    }

    return msj;
}

function getObjetoGrupodatJson() {
    var obj = {};
    obj.fechaData = obtenerHoraValida($("#fechaData").val());
    obj.valorData = $("#valorData").val();
    obj.grupocodi = parseInt($("#hfGrupocodiDat").val()) || 0;
    obj.concepcodi = parseInt($("#hfConcepcodiDat").val()) || 0;
    obj.deleted = parseInt($("#hfDeleted").val()) || 0;
    obj.equicodi = parseInt($("#cbUnidadesEspeciales").val()) || 0;

    var checkCero = $("#ChkValorCeroCorrecto").is(':checked');
    if (checkCero) {
        obj.gdatcheckcero = 1;
    }
    else {
        obj.gdatcheckcero = 0;
    }
    if (obj.valorData != "0") obj.gdatcheckcero = 0;

    obj.gdatcomentario = $("#Gdatcomentario").val();
    obj.gdatsustento = $("#Gdatsustento").val();

    return obj;
}

function obtenerListaEqModoBD(grupocodi, listaObjRel) {
    var lista = [];

    for (var i = 0; i < listaObjRel.length; i++) {
        if (listaObjRel[i].Grupocodi == grupocodi && listaObjRel[i].Grupotipomodo == "E")
            lista.push(listaObjRel[i]);
    }

    return lista;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Archivos de sustento
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function descargarArchivoSustento(urlSustento) {
    if (urlSustento != "" && urlSustento != null) {

        window.open(urlSustento, '_blank').focus();

        //var esFileApp = (urlSustento.toLocaleUpperCase()).includes('DescargarSustentoConfidencial?'.toLocaleUpperCase())
        //    || (urlSustento.toLocaleUpperCase()).includes('DescargarSustento?'.toLocaleUpperCase());

        //if (esFileApp) {
        //    $.ajax({
        //        type: 'POST',
        //        dataType: 'json',
        //        traditional: true,
        //        url: controlador + 'DescargarArchivoSustento',
        //        data: {
        //        },
        //        cache: false,
        //        success: function (result) {
        //            if (result.Resultado == '-1') {
        //                alert('Ha ocurrido un error:' + result.Mensaje);
        //            } else {
        //                var urlFileApp = urlSustento.split('?')[0];
        //                var paramUrlArchivo = (urlSustento.split('?')[1]).split('=')[1];
        //                paramUrlArchivo = decodeURIComponent(paramUrlArchivo.replace(/\+/g, " "));

        //                var paramList = [
        //                    { tipo: 'input', nombre: 'url', value: paramUrlArchivo },
        //                    { tipo: 'input', nombre: 'user', value: result.Resultado }
        //                ];
        //                var form = CreateFormArchivo(urlFileApp, paramList);
        //                document.body.appendChild(form);
        //                form.submit();
        //            }
        //        },
        //        error: function (err) {
        //            alert('Ha ocurrido un error.');
        //        }
        //    });
        //} else {
        //    //var element = document.createElement('a');
        //    //element.setAttribute('href', ' ' + urlSustento);
        //    //element.setAttribute('download', "archivo");
        //    //document.body.appendChild(element);
        //    //element.click();
        //    window.open(urlSustento, '_blank').focus();
        //}
    }
}

//function CreateFormArchivo(path, params, method = 'post') {
//    var form = document.createElement('form');
//    form.method = method;
//    form.action = path;

//    $.each(params, function (index, obj) {
//        var hiddenInput = document.createElement(obj.tipo);
//        hiddenInput.type = 'hidden';
//        hiddenInput.name = obj.nombre;
//        hiddenInput.value = obj.value;
//        form.appendChild(hiddenInput);
//    });
//    return form;
//}

function getHtmlCeldaSustento(urlSustento) {
    var arrayLink = getListaEnlaceXTexto(urlSustento);
    var htmlDiv = '';
    for (var i = 0; i < arrayLink.length; i++) {
        var link = arrayLink[i];
        var esConfidencial = (link.toLocaleUpperCase()).includes('DescargarSustentoConfidencial?'.toLocaleUpperCase());
        var textoTitle = esConfidencial ? 'Descargar archivo de sustento (CONFIDENCIAL)' : 'Descargar archivo de sustento';

        htmlDiv += `
                <div class='estiloSustentoProp' title='${textoTitle} - ${link}' onclick='descargarArchivoSustento("${link}")' style='height: 20px;'>
                    
                </div>
            `;
    }

    return htmlDiv;
}

function getListaEnlaceXTexto(texto) {
    if (texto == null || texto == undefined) texto = "";
    texto = texto.trim();

    texto = texto.replace(/(?:\r\n|\r|\n)/g, ' ');
    texto = texto.replace(/(\n)+/g, ' ');

    var arrayLink = [];
    var arraySep = texto.split(' ');
    for (var i = 0; i < arraySep.length; i++) {
        var posibleLink = arraySep[i].trim();
        if (posibleLink.length > 0 && (
            posibleLink.toLowerCase().startsWith('http') || posibleLink.toLowerCase().startsWith('www'))) {
            arrayLink.push(posibleLink);
        }
    }

    return arrayLink;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Parametros generales
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function cargarParametrosGenerales(fechaConsulta) {
    $.ajax({
        type: 'POST',
        url: controlador + "ParametrosGenerales",
        data: {
            fechaConsulta: fechaConsulta
        },
        success: function (evt) {
            $('#listadoParamGenerales').css("width", ancho + "px");
            $('#listadoParamGenerales').html(evt);

            cargarListaParamGral();

            var fechita = evt.FechaFull;

            if (fechita != "") {
                $('#txtFechaDataParamGral').Zebra_DatePicker({
                    direction: [FECHA_LIMITE, FECHA_LIMITE],
                    onSelect: function () {
                        cargarListaParamGral();
                    }
                });
            } else {
                $('#txtFechaDataParamGral').Zebra_DatePicker({

                    onSelect: function () {
                        cargarListaParamGral();
                    }
                });
            }


        },
        error: function (err) {
            mostrarError();
        }
    });
}

function cargarListaParamGral() {
    var fechaC = $("#txtFechaDataParamGral").val();
    $.ajax({
        type: 'POST',
        url: controlador + "ParametrosGeneralesData",
        data: {
            fechaConsulta: fechaC
        },
        success: function (evt) {
            $('#div_tabla_data_param_grales').css("width", ancho + "px");
            $('#div_tabla_data_param_grales').html(evt);

            $('#tabla_parametro_general').dataTable({
                "sDom": 'ft',
                "ordering": false,
                "iDisplayLength": -1
            });


        },
        error: function (err) {
            mostrarError();
        }
    });
}


////////////////////////////////////  Cambios Ficha Tecnica 2023 ///////////////////////////////////////////

function asignarGrupoExtranetFT(idGrupoMO) {
    limpiarBarraMensaje("textoMensaje");
    limpiarBarraMensaje("mensaje_popupAsignarGrupoFT");

    $("#hdGrupo").val(idGrupoMO);

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerDatosAsignacion',
        data: {
            grupocodi: idGrupoMO
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado == "1") {
                //Muestro los datos del grupo
                completarDatosGrupo(evt.Grupo);

                //muestro lista de proyectos
                var arrayProyectos = obtenerArrayDeProyectos(evt.ListaProyectosGrupo);
                mostrarProyectosExtranet(arrayProyectos);

                $('#btnConfirmarPy').unbind();
                $('#btnConfirmarPy').click(function () {
                    confirmarProyecto();
                });

                $('#btnGuardarAsignacion').unbind();
                $('#btnGuardarAsignacion').click(function () {
                    guardarDatosAsignacion();
                });

                abrirPopup("popupAsignarGrupoFT");
            }
            else {
                mostrarMensaje('textoMensaje', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje('textoMensaje', 'error', 'Se ha producido un error.');
        }
    });

}

function completarDatosGrupo(objGrupo) {
    $("#codigoVal").html(objGrupo.Grupocodi);
    $("#nombreVal").html(objGrupo.Gruponomb.trim());
    $("#categoriaVal").html(objGrupo.Catenomb.trim());
    $("#empresaVal").html(objGrupo.Emprnomb.trim());
    $("#ubicacionVal").html(objGrupo.Areadesc.trim());

}

function obtenerArrayDeProyectos(listaProyectos) {
    var array = [];

    for (key in listaProyectos) {
        var item = listaProyectos[key];

        var estadoDesc = "";
        if (item.Ftreqpestado == 0)
            estadoDesc = "Baja";

        if (item.Ftreqpestado == 1)
            estadoDesc = "Activo";

        let proy = {
            "idGrupoProyecto": item.Ftreqpcodi,
            "codigoProyecto": item.Ftprycodi,
            "nombreEmpresa": item.Emprnomb,
            "codigo": item.Ftpryeocodigo,
            "nombProy": item.Ftpryeonombre,
            "nombProyExt": item.Ftprynombre,
            "estado": item.Ftreqpestado,
            "estadoDesc": estadoDesc,
            "esAgregado": 0,
            "esEditado": 0
        }

        array.push(proy);
    }
    listaProyectosGrupoMemoria = array;

    return array;
}

function mostrarProyectosExtranet(arrayProyectos) {
    $("#listadoProyectosExtranet").html("");

    var htmlTabla = dibujarTablaListadoProyectos(arrayProyectos);
    $("#listadoProyectosExtranet").html(htmlTabla);

    var tamAlturaTablaPyE = 350;

    $('#tablaProyectos').dataTable({
        "scrollY": tamAlturaTablaPyE,
        "scrollX": false,
        "sDom": 't',
        "ordering": false,
        "iDisplayLength": -1
    });
}

function dibujarTablaListadoProyectos(arrayProyectos) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaProyectos"  >
       <thead>
           <tr style="height: 22px;">
               <th style='width:60px;'>Acción</th>
               <th style='width:180px;'>Empresa</th>
               <th style='width:100px;'>Código Estudio (EO)</th>
               <th style='width:180px;'>Nombre del Proyecto</th>
               <th style='width:180px;'>Nombre del Proyecto (Extranet)</th>
               <th style='width:50px;'>Estado</th>
               <th style='width:60px;'>Auditoria</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in arrayProyectos) {
        var item = arrayProyectos[key];
        var colorFila = "";

        if (item.estado == 0)
            colorFila = COLOR_BAJA;

        cadena += `
            <tr>
                <td style='width:60px; background: ${colorFila}'>                   
        `;

        if (item.estado == 0) {
            cadena += `
                    <a href="JavaScript:activarProyecto(${item.idGrupoProyecto},${item.codigoProyecto});">${IMG_ACTIVAR}</a>
            `;
        } else {
            if (item.estado == 1) {
                cadena += `                    
                    <a href="JavaScript:eliminarProyecto(${item.idGrupoProyecto},${item.codigoProyecto});">${IMG_ELIMINAR}</a>
                `;
            }
        }
        cadena += `                    
                </td>
                <td style="background: ${colorFila}; width:180px; ">${item.nombreEmpresa}</td>
                <td style="background: ${colorFila}; width:100px; ">${item.codigo}</td>
                <td style="background: ${colorFila}; width:180px; ">${item.nombProy}</td>
                <td style="background: ${colorFila}; width:180px; ">${item.nombProyExt}</td>
                <td style="background: ${colorFila}; width:50px; ">${item.estadoDesc}</td>
        `;
        if (item.esAgregado == 0) {
            cadena += `
                <td style="background: ${colorFila}; width:60px; "><a href="JavaScript:mostrarAuditoriaPE(${item.idGrupoProyecto});">${IMG_VER}</a></td>
            `;
        } else {
            cadena += `
                <td style="background: ${colorFila}"></td>
            `;
        }

        cadena += `
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}


function agregarProyectoExtranet() {
    limpiarBarraMensaje("mensaje_popupAsignarGrupoFT");
    limpiarBarraMensaje("mensaje_popupBusquedaPy");

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerProyectosExistentes',
        data: {
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado == "1") {
                var htmlPy = dibujarTablaListadoPyE(evt.ListadoProyectos);
                $("#listadoProyectos").html(htmlPy);

                $('#tablaPyE').dataTable({
                    "scrollY": 250,
                    "scrollX": false,
                    "sDom": 'ft',
                    "ordering": false,
                    "iDisplayLength": -1
                });

                abrirPopup("popupBusquedaPy");
            }
            else {
                mostrarMensaje('mensaje_popupAsignarGrupoFT', 'error', 'Error: ' + evt.Mensaje);
            }
        },
        error: function () {
            mostrarMensaje('mensaje_popupAsignarGrupoFT', 'error', 'Se ha producido un error.');
        }
    });
}


function dibujarTablaListadoPyE(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono" id="tablaPyE" >
       <thead>
           <tr style="height: 22px;">
               <th style='width:40px;'>Sel.</th>
               <th style='width:200px;'>Empresa</th>
               <th style='width:130px;'>Código Estudio (EO)</th>
               <th style='width:260px;'>Nombre del Proyecto</th>
               <th style='width:260px;'>Nombre del Proyecto (Extranet)</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        cadena += `
            <tr>
                <td style='width:40px;'>        
                    <input type="radio" id="rdEstudio_${item.Ftprycodi}" name="rdPy" value="${item.Ftprycodi}">
                </td>
                <td style='width:200px;'>${item.Emprnomb}</td>
                <td style='width:130px;'>${item.Ftpryeocodigo}</td>
                <td style='width:260px;'>${item.Ftpryeonombre}</td>
                <td style='width:260px;'>${item.Ftprynombre}</td>
               
           </tr >           
        `;
    }

    cadena += `                   
        </tbody >
    </table >
    `;

    return cadena;
}


function confirmarProyecto() {
    limpiarBarraMensaje("mensaje_popupBusquedaPy");

    var filtro = datosConfirmar();
    var msg = validarDatosConfirmar(filtro);

    if (msg == "") {
        var idProyecto = parseInt(filtro.seleccionado.value) || 0;
        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatosProySel",
            data: {
                ftprycodi: idProyecto
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    agregarProyectoAArrayProyectos(evt.Proyecto);
                    mostrarProyectosExtranet(listaProyectosGrupoMemoria);

                    cerrarPopup('popupBusquedaPy');


                } else {
                    mostrarMensaje('mensaje_popupBusquedaPy', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupBusquedaPy', 'error', 'Ha ocurrido un error al agregar proyecto.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupBusquedaPy', 'alert', msg);
    }
}

function agregarProyectoAArrayProyectos(objProyecto) {
    let proy = {
        "idGrupoProyecto": -1,

        "codigoProyecto": objProyecto.Ftprycodi,
        "nombreEmpresa": objProyecto.Emprnomb,
        "codigo": objProyecto.Ftpryeocodigo,
        "nombProy": objProyecto.Ftpryeonombre,
        "nombProyExt": objProyecto.Ftprynombre,
        "estado": 1,
        "estadoDesc": "Activo",
        "esAgregado": 1,
        "esEditado": 0
    }

    listaProyectosGrupoMemoria.unshift(proy);

}

function datosConfirmar() {
    var filtro = {};

    var radioSel = document.querySelector('input[name="rdPy"]:checked');

    filtro.seleccionado = radioSel;

    return filtro;
}

function validarDatosConfirmar(datos) {

    var msj = "";


    if (datos.seleccionado == null) {
        msj += "<p>Debe seleccionar un Proyecto.</p>";
    } else {
        var idProyecto = parseInt(datos.seleccionado.value) || 0;

        //busco repitencias en los activos
        let lstIguales = listaProyectosGrupoMemoria.filter(x => x.codigoProyecto === idProyecto && x.estado === 1);

        if (lstIguales.length > 0) {
            msj += "<p>El proyecto seleccionado ya se encuentra en la tabla. No se permite duplicados.</p>";

        }
    }

    return msj;
}


function eliminarProyecto(idReg, idProy) {
    limpiarBarraMensaje("mensaje_popupAsignarGrupoFT");
    if (confirm('¿Desea eliminar el registro?')) {

        const registro = listaProyectosGrupoMemoria.find(x => x.idGrupoProyecto === idReg && x.codigoProyecto === idProy);

        if (registro != null) {
            registro.estado = 0;
            registro.estadoDesc = "Baja";
            registro.esEditado = 1;

            mostrarMensaje('mensaje_popupAsignarGrupoFT', 'exito', 'Registro dado de baja satisfactoriamente.');
            mostrarProyectosExtranet(listaProyectosGrupoMemoria);
        } else {
            mostrarMensaje('mensaje_popupAsignarGrupoFT', 'error', 'Ha ocurrido un error al eliminar registro.');
        }

    }
}


function activarProyecto(idReg, idProy) {
    limpiarBarraMensaje("mensaje_popupAsignarGrupoFT");
    if (confirm('¿Desea activar el registro?')) {

        const registro = listaProyectosGrupoMemoria.find(x => x.idGrupoProyecto === idReg && x.codigoProyecto === idProy);

        //valido que no hay otro registro activo con el mismo codigoProyecto
        const regActivoConMismoProyecto = listaProyectosGrupoMemoria.find(x => x.estado === 1 && x.codigoProyecto === idProy);

        if (regActivoConMismoProyecto != null) {
            mostrarMensaje('mensaje_popupAsignarGrupoFT', 'error', 'Existe otro registro activo con el mismo Proyecto.');
        } else {
            if (registro != null) {
                registro.estado = 1;
                registro.estadoDesc = "Activo";
                registro.esEditado = 1;
                mostrarMensaje('mensaje_popupAsignarGrupoFT', 'exito', 'Registro activado satisfactoriamente.');
                mostrarProyectosExtranet(listaProyectosGrupoMemoria);
            } else {
                mostrarMensaje('mensaje_popupAsignarGrupoFT', 'error', 'Ha ocurrido un error al activar registro.');
            }
        }
    }
}

function mostrarAuditoriaPE(ftreqpcodi) {
    limpiarBarraMensaje("mensaje_popupAsignarGrupoFT");
    limpiarDatosPopupAuditoria();

    var filtro = datosAuditoria(ftreqpcodi);
    var msg = validarDatosAuditoria(filtro);

    if (msg == "") {

        var idReg = parseInt(ftreqpcodi) || 0;

        $.ajax({
            type: 'POST',
            url: controlador + "obtenerDatosPEAuditoria",
            data: {
                ftreqpcodi: idReg
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {

                    mostrarDatosPEAuditoria(evt.RelEquipoProyecto);

                } else {
                    mostrarMensaje('mensaje_popupAsignarGrupoFT', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupAsignarGrupoFT', 'error', 'Ha ocurrido un error al mostrar estudios EO.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupAsignarGrupoFT', 'alert', msg);
    }
}

function datosAuditoria(ftreqpcodi) {
    var filtro = {};

    filtro.id = ftreqpcodi;

    return filtro;
}

function validarDatosAuditoria(datos) {

    var msj = "";


    if (datos.id == -1) {
        msj += "<p>Debe seleccionar un registro ya guardado en base de datos.</p>";
    }

    return msj;
}

function limpiarDatosPopupAuditoria() {
    $("#usuCreacionVal").html("");
    $("#fecCreacionVal").html("");
    $("#usuModificacionVal").html("");
    $("#fecModificacionVal").html("");
}

function mostrarDatosPEAuditoria(objReleqpry) {
    $("#usuCreacionVal").html(objReleqpry.Ftreqpusucreacion);
    $("#fecCreacionVal").html(objReleqpry.FechaCreacionDesc);
    $("#usuModificacionVal").html(objReleqpry.Ftreqpusumodificacion);
    $("#fecModificacionVal").html(objReleqpry.FechaModificacionDesc);

    abrirPopup('popupAuditoria');
}


function guardarDatosAsignacion() {
    limpiarBarraMensaje("mensaje_popupAsignarGrupoFT");
    var idGrupo = $("#hdGrupo").val();

    var filtro = datosAsignacion();
    var msg = validarDatosAsignacion(filtro);

    if (msg == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "guardarInfoAsignacion",
            data: {
                grupocodi: idGrupo,
                strCambiosPE: filtro.strCambiosProyExt,
                //strCambiosELT: filtro.strCambiosEmpresasCo,
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    mostrarMensaje('textoMensaje', 'exito', "La información se guardó correctamente.");
                    cerrarPopup('popupAsignarGrupoFT');

                } else {
                    mostrarMensaje('mensaje_popupAsignarGrupoFT', 'error', evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje_popupAsignarGrupoFT', 'error', 'Ha ocurrido un error al mostrar estudios EO.');
            }
        });
    } else {
        mostrarMensaje('mensaje_popupAsignarGrupoFT', 'alert', msg);
    }
}


function datosAsignacion() {
    var filtro = {};

    //Obtengo listado de cambios en proyectos extranet
    let arrayCambiosEP = listaProyectosGrupoMemoria.filter(x => x.esAgregado === 1 || x.esEditado === 1);

    filtro.arrCambiosProyectosExtranet = arrayCambiosEP;
    filtro.strCambiosProyExt = obtenerCadenaCambiosProyExt(arrayCambiosEP);

    return filtro;
}

function validarDatosAsignacion(datos) {

    var msj = "";
    var numCambios = datos.arrCambiosProyectosExtranet.length;

    if (numCambios == 0) {
        msj += "<p>No se detectó cambios, ingrese o edite información.</p>";
    }

    return msj;

}


function obtenerCadenaCambiosProyExt(arrayCambios) {
    var salida = "";

    var lstCambios = [];
    for (key in arrayCambios) {
        var item = arrayCambios[key];
        var reg = item.idGrupoProyecto + "$$" + item.codigoProyecto + "$$" + item.estado + "$$" + item.esAgregado + "$$" + item.esEditado;
        lstCambios.push(reg);
    }

    salida = lstCambios.join("??");
    return salida;
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

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Util
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function mostrarError() {
    $('#textoMensaje').css("display", "block");
    $('#textoMensaje').removeClass();
    $('#textoMensaje').addClass('action-alert');
    $('#textoMensaje').text("Ha ocurrido un error");
}

function getCodigoEmpresa() {
    return parseInt($("#cbEmpresa").val()) || 0;
}

function getCodigoCategoria() {
    return parseInt($("#cbCategoria").val()) || 0;
}

function getEstado() {
    var estado = "S";
    if ($('#check_estado_todos').is(':checked')) {
        estado = '-1';
    }
    return estado;
}

function getEsReservaFria() {
    var estado = -1;
    if ($('#check_rsrvfria').is(':checked')) {
        estado = 1;
    }
    return estado;
}

function getEsNodoEnergetico() {
    var estado = -1;
    if ($('#check_nodo').is(':checked')) {
        estado = 1;
    }
    return estado;
}

function getNombre() {
    return $("#txtNombregrupo").val();
}

function convertirStringToHtml(str) {
    if (str != undefined && str != null) {
        //return str.replace(/&/g, "&amp;").replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
        return str.replace(/>/g, "&gt;").replace(/</g, "&lt;").replace(/"/g, "&quot;");
    }

    return "";
}

function obtenerHoraValida(hora) {
    if (hora !== undefined && hora != null) {
        hora = hora.replace('h', '0');
        hora = hora.replace('h', '0');

        hora = hora.replace('m', '0');
        hora = hora.replace('m', '0');

        hora = hora.replace('s', '0');
        hora = hora.replace('s', '0');
        return hora;
    }

    return '';
}


function validNumber(value) {
    return !isNaN(value) && value != undefined && value != null && value != -1 && value != 0
}


function cargarGrupos() {

    var emprcodiSeleccionado = parseInt($("#cbFormNEEmprcodi").val());
    var catecodiSeleccionado = parseInt($("#cbFormNECatecodi").val());

    const convertidor = {
        9: 5,
        2: 3,
        3: 4,
        4: 4,
        5: 6,
        6: 6,
        15: 16,
        17: 18
    };


    if (validNumber(emprcodiSeleccionado) && validNumber(catecodiSeleccionado)) {
        if (catecodiSeleccionado == 4 || catecodiSeleccionado == 6 || catecodiSeleccionado == 16 || catecodiSeleccionado == 18) {
            $('#trFormGrupoPadre').hide();
            return;
        } else $('#trFormGrupoPadre').show();


        $.ajax({
            type: "POST",
            url: controlador + "ListGroupByEmprcodiAndCatecodi?emprcodi=" + emprcodiSeleccionado + "&catecodi=" + convertidor[catecodiSeleccionado],
            success: function (data) {
                // Limpiar las opciones actuales del segundo select
                $('#cbFormNEGrupoPadreCodi').empty();
                // Agregar las nuevas opciones al segundo select
                $('#cbFormNEGrupoPadreCodi').append('<option value="-2">SIN GRUPO PADRE</option>');
                $.each(data, function (index, item) {
                    $('#cbFormNEGrupoPadreCodi').append('<option value="' + item.Grupocodi + '">' + item.Gruponomb + '</option>');
                });
                $('select[name^="cbFormNEGropoPadre"]').multipleSelect({
                    width: '440px',
                    filter: true,
                    single: true,
                    onClose: function () {
                    }
                });
                $('#cbFormNEGrupoPadreCodi').multipleSelect("setSelects", [GRUPOPADRE]);
            },
            error: function () {
                console.log("Error al cargar grupos.");
            }
        });
    }
}

$(document).ready(function () {
    $('#cbFormNEEmprcodi').change(cargarGrupos);
});