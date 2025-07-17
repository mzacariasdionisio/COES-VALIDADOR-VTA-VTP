var controlador = siteRoot + 'Migraciones/Despacho/';

var CARGA_DATOS_DESDE_EXCEL = 1;
var CARGA_DATOS_DESDE_SICOES = 2;

var ACCION_CONSULTA = 1;
var ACCION_EXPORTAR = 199945;

var LECTCODI_PROGSEMANAL = 3;
var LECTCODI_PROGDIARIO = 4;
var LECTCODI_REPROGDIARIO = 5;
var LECTCODI_EJECUTADOHISTO = 6;
var LECTCODI_EJECUTADO = 93;
var LECTCODI_AJUSTEDIARIO = 7;

var CONF_ANTERIOR_CARGA_DATOS = 0;
var CONF_ANTERIOR_FECHA_INI = '';
var CONF_ANTERIOR_FECHA_FIN = '';
var CONF_ANTERIOR_LECTCODI = 0;

var DIFF_ANTERIOR_MILISEGUNDOS = 0;

var valide = false;
var EXISTE_HTRABAJO_EN_MEMORIA = false;
var ARCHIVO_HTRABAJO = '';
var LISTA_TABLA_WEB = [];
var LISTA_TIPO_INFO = [];

$(function () {
    $('#cntMenu').css("display", "none");

    $('#tab-container').easytabs({ animate: false });

    $('input:radio[name="cbCargaDatos"]').change(function () {
        EXISTE_HTRABAJO_EN_MEMORIA = false;

        f_mostrarElementos(getCheckCargaDatos());
    });
    $("#cbProg1").val(6);
    $("#cbProg3").val(6);
    f_mostrarElementos(getCheckCargaDatos());

    //Fila 1: Desde Excel
    $('#cbProg3').change(function (e) {
        EXISTE_HTRABAJO_EN_MEMORIA = false;

        f_mostrarElementos(getCheckCargaDatos());
    });
    crearPupload("Archivos XLSM .", "xlsm");

    //Fila 2: Desde SICOES
    $('#txtFechaini').Zebra_DatePicker({
        pair: $('#txtFechafin'),
        onSelect: function (date) {
            $('#txtFechafin').val(date);
        }
    });
    $('#txtFechafin').Zebra_DatePicker({
        direction: true
    });

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

    $('#cbProg1').change(function (e) {
        valide = false;
        f_mostrarElementos(getCheckCargaDatos());
    });

    $('#cbTipoinfo').change(function (e) {
        _mostrarTablaWebXTipoinfo(getTipoinfocodi());
    });

    $("#btnConsultar").click(function () {
        f_mostrarResultado(ACCION_CONSULTA);
    });
    $("#btnExportar").click(function () {
        f_mostrarResultado(ACCION_EXPORTAR);
    });

    //subida de htrabajo
    $("#btnSaveTmp").click(function () {
        guardarExcelCargado();
    });

    //recálculo costo operación
    $("#btnRecalcularCO").click(function () {
        costo_consultarRecalculoCostoOpEjecutado();
    });
    $("#btnSaveTmpRecalcularCO").click(function () {
        costo_guardarRecalculoCostoOpEjecutado();
    });

    ////////////////////////////////
    $(window).resize(function () {
        updateContainer();
    });

    $("#tab-container").hide();

    //f_mostrarResultado(ACCION_CONSULTA);
});

//////////////////////////////////////////////////////////
//// filtros
//////////////////////////////////////////////////////////

function f_mostrarElementos(tipo) {
    $("#tab-container").hide();

    $("#idCombo1").hide();
    $("#btnSaveTmp").hide();

    $("#listado").html(' ');
    $("#mensaje").hide();
    $("#mensaje2").hide();

    $(".fila_cargaDatos_1").hide();
    $(".fila_cargaDatos_2").hide();

    $("#btnRecalcularCO").hide();
    $("#txtCostoTotal").html('');
    $("#btnSaveTmpRecalcularCO").hide();

    switch (tipo) {
        case CARGA_DATOS_DESDE_EXCEL:
            $(".fila_cargaDatos_1").show();
            mostrarMensajeCargaArchivo();

            break;
        case CARGA_DATOS_DESDE_SICOES:
            $("#btnConsultar").show();
            $(".fila_cargaDatos_2").show();
            break;
    }
};

function getLectcodi(p) {
    var pp = "";
    switch (p) {
        case CARGA_DATOS_DESDE_EXCEL:
            pp = $('#cbProg3').val(); break;
        case CARGA_DATOS_DESDE_SICOES:
            pp = $('#cbProg1').val(); break;
    }
    return parseInt(pp) || 0;
}

function getCheckCargaDatos() {
    var tipoCargaDatos = $('input[name=cbCargaDatos]:checked').val();

    return parseInt(tipoCargaDatos) || 0;
}
function getTieneDetalleAdicional() {
    var estado = false;
    if ($('#check_detalleAdicional').is(':checked')) {
        estado = true;
    }
    return estado;
}

function getTipoinfocodi() {
    return parseInt($("#cbTipoinfo").val()) || 0;
}

//////////////////////////////////////////////////////////
//// Desde SICOES
//////////////////////////////////////////////////////////

function consultarCdispatch(esConsultaBD, lectcodi, tipoinfocodi, flagRecalcularCosto) {

    $("#listaObservaciones").html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ConsultarCdispatch",
        dataType: 'json',
        data: {
            fecha1: $('#txtFechaini').val(),
            fecha2: $('#txtFechafin').val(),
            lectcodi: lectcodi,
            esConsultaBD: esConsultaBD,
            nombreFile: ARCHIVO_HTRABAJO,
            flagRecalcularCosto: flagRecalcularCosto
        },
        success: function (evt) {
            $("#tab-container").hide();
            $("#mensaje").hide();
            $("#mensaje2").hide();

            $("#leyenda").hide();
            $("#idCombo1").hide();

            if (evt.nRegistros > 0) {

                $("#tab-container").show();

                $("#leyenda").show();
                $("#idCombo1").show();

                //tabla web
                LISTA_TIPO_INFO = evt.ListaTipoInfo;
                LISTA_TABLA_WEB = evt.ListaResultado;
                _mostrarTablaWebXTipoinfo(tipoinfocodi);

                $("#listaObservaciones").html(evt.ResultadoObservaciones);
                $("#txtCostoTotal").html(evt.CostoTotalOperacion);

                //carga de archivo
                if (EXISTE_HTRABAJO_EN_MEMORIA) {
                    $("#btnSaveTmp").show();
                    $("#mensaje2").html('Los datos se procesaron correctamente, presione el botón <b>Guardar</b> para grabar en el sistema.');
                    $("#mensaje2").show();
                }

                //Recálculo de costo de la operación
                if (flagRecalcularCosto) {
                    costo_VisibleGuardarRecalculo(evt);
                }

            }
            else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function _mostrarTablaWebXTipoinfo(tipoinfocodi) {
    $("#listado").html('');
    var scrollTablaY = getHeightTablaListado();

    //tabla web
    var posTablaWeb = 0;
    for (var i = 0; i < LISTA_TIPO_INFO.length; i++) {
        if (tipoinfocodi == LISTA_TIPO_INFO[i].Tipoinfocodi)
            posTablaWeb = i;
    }

    $("#listado").html(LISTA_TABLA_WEB[posTablaWeb]);

    $('#tb_info').dataTable({
        "scrollY": scrollTablaY,
        "scrollX": true,
        "scrollCollapse": false,
        "sDom": 't',
        "ordering": false,
        paging: false,
        "iDisplayLength": -1,
        "bAutoWidth": false,
        "destroy": "true",
        fixedColumns: {
            leftColumns: 1
        },
        stripeClasses: []
    });

    $("#tb_info").DataTable().draw();
    updateContainer();
    $("#tb_info").DataTable().draw();
}

function exportarCdispatch(esConsultaBD, lectcodi, tipoinfocodi) {

    var detalleAdicional = getTieneDetalleAdicional();

    $.ajax({
        type: 'POST',
        url: controlador + "ExportarCdispatch",
        dataType: 'json',
        data: {
            fecha1: $('#txtFechaini').val(),
            fecha2: $('#txtFechafin').val(),
            lectcodi: lectcodi,
            tipoinfocodi: tipoinfocodi,
            esConsultaBD: esConsultaBD,
            tieneMostrarDetallaAdicional: detalleAdicional,
            nombreFile: ARCHIVO_HTRABAJO
        },
        success: function (evt) {

            if (evt.nRegistros >= 0) {
                if (evt.nRegistros > 0) {
                    window.location = controlador + "Exportar?fi=" + evt.Resultado;
                } else {
                    alert("No existen registros !");
                }
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

async function f_mostrarResultado(tipoAccion) {
    var tipoCargaDatos = EXISTE_HTRABAJO_EN_MEMORIA ? CARGA_DATOS_DESDE_EXCEL : CARGA_DATOS_DESDE_SICOES;
    var lectcodi = getLectcodi(tipoCargaDatos);

    var consultarBD = true;
    if (EXISTE_HTRABAJO_EN_MEMORIA) {
        consultarBD = false;

        $("#btnConsultar").hide();
        $("#btnRecalcularCO").hide();
    } else {
        consultarBD = true;

        $("#btnConsultar").show();
        if (lectcodi == LECTCODI_EJECUTADOHISTO)
            $("#btnRecalcularCO").show();
    }


    //web
    if (ACCION_CONSULTA == tipoAccion) {
        if (consultarBD) {
            VISIBLE_BOTON_RECALCULO = false;
            await costo_VerificarPermisoRecalculo(lectcodi);

            costo_VisibleBoton();
        }

        consultarCdispatch(consultarBD, lectcodi, getTipoinfocodi(), false);
    }

    //excel
    if (ACCION_EXPORTAR == tipoAccion) {
        exportarCdispatch(consultarBD, lectcodi, getTipoinfocodi());
    }
}

//funcion que calcula el ancho disponible para la tabla reporte
function getHeightTablaListado() {
    var h = $(window).height()
        - $("header").height()
        - $("#cntTitulo").height()
        - 15
        - $("#filtros_cdispatch").height() + 16 //Filtros
        - 40 //tab
        - 170 //primera fila
        - 100
        ;

    //return h + "px";
    return 738 + "px";
}

function updateContainer() {
    var $containerWidth = $(window).width();

    $('#listado').css("width", $containerWidth - 65 + "px");
}

//////////////////////////////////////////////////////////
//// Cargar Htrabajo y guardar
//////////////////////////////////////////////////////////
async function crearPupload(descripcionExtensionPermitido, extensionPermitido) {

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnSelect1",
        url: controlador + 'Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: 0,
            mime_types: [
                {
                    title: descripcionExtensionPermitido + extensionPermitido,
                    extensions: extensionPermitido
                },
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
                $("#mensaje2").hide();
            },
            UploadComplete: function (up, file) {
                if (file[0].percent == 100) {
                    mostrarAlerta("Subida completada. <strong>Por favor espere</strong>");
                    uploader.splice();
                    uploader.refresh();
                } else {
                    mostrarError("No se ha cargado el archivo.");
                }
            },
            FileUploaded: function (up, file, result) {
                var model = JSON.parse(result.response);

                if (model.success) {
                    ARCHIVO_HTRABAJO = model.nuevonombre;

                    validarFileUpxls();
                } else {
                    ARCHIVO_HTRABAJO = '';
                }
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });

    uploader.init();
}

async function validarFileUpxls() {
    $.ajax({
        type: 'POST',
        url: controlador + "ValidarFileUpxls",
        dataType: 'json',
        data: {
            lectcodi: $('#cbProg3').val(),
            nombreFile: ARCHIVO_HTRABAJO
        },
        success: function (evt) {
            if (evt.MensajeError == null || evt.MensajeError == '') {
                EXISTE_HTRABAJO_EN_MEMORIA = true;

                mostrarExito(evt.MensajeOK);
                f_mostrarResultado(ACCION_CONSULTA);
            } else {
                mostrarError(evt.MensajeError);
            }
        },
        error: function (err) {
            mostrarError("Ha ocurrido un error");
        }
    });
}

async function guardarExcelCargado() {
    var lectcodi = getLectcodi(CARGA_DATOS_DESDE_EXCEL);
    if (EXISTE_HTRABAJO_EN_MEMORIA) {
        $.ajax({
            type: 'POST',
            url: controlador + "SaveLoadXls",
            dataType: 'json',
            data: {
                lectcodi: lectcodi,
                nombreFile: ARCHIVO_HTRABAJO,
            },
            success: function (evt) {
                if (evt.nRegistros > 0) {
                    $('input:radio[name=cbCargaDatos]').val(['2']);
                    f_mostrarElementos(getCheckCargaDatos());

                    //setear datos para consulta
                    EXISTE_HTRABAJO_EN_MEMORIA = false;
                    ARCHIVO_HTRABAJO = '';
                    $("#txtFechaini").val(evt.FechaIni);
                    $("#txtFechafin").val(evt.FechaFin);
                    $("#cbProg1").val(lectcodi);

                    mostrarExito("Los datos se guardaron correctamente.");

                    f_mostrarResultado(ACCION_CONSULTA);
                } else {
                    mostrarError("Ha ocurrido un error. " + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarError("Ha ocurrido un error");
            }
        });
    } else {
        alert("No existe Htrabajo seleccionado.");
    }
}

//////////////////////////////////////////////////////////
//// Recálculo de costo operación
//////////////////////////////////////////////////////////

function costo_consultarRecalculoCostoOpEjecutado() {
    var lectcodi = getLectcodi(CARGA_DATOS_DESDE_SICOES);
    consultarCdispatch(true, lectcodi, getTipoinfocodi(), true);
}

function costo_guardarRecalculoCostoOpEjecutado() {
    if (confirm("¿Desea guardar el recálculo del Costo de la Operación?")) {

        var lectcodi = getLectcodi(CARGA_DATOS_DESDE_SICOES);
        $.ajax({
            type: 'POST',
            url: controlador + "GuardarRecalculoCostoOperacion",
            dataType: 'json',
            data: {
                fecha: $('#txtFechaini').val(),
                lectcodi: lectcodi,
            },
            success: function (model) {
                if (model.nRegistros > 0) {
                    mostrarExito("La operación se realizó correctamente.");

                    f_mostrarResultado(ACCION_CONSULTA);
                } else {
                    mostrarError("Ha ocurrido un error");
                }
            },
            error: function (err) {
                mostrarError("Ha ocurrido un error");
            }
        });
    }
}

var VISIBLE_BOTON_RECALCULO = false;
async function costo_VerificarPermisoRecalculo(lectcodi) {
    var fecha = $('#txtFechaini').val();

    return $.ajax({
        type: 'POST',
        url: controlador + 'VerificarPermisoRecalculo',
        data: {
            fecha: fecha,
            lectcodi: lectcodi,
        },
        dataType: 'json',
        success: function (model) {
            VISIBLE_BOTON_RECALCULO = (model.nRegistros > 0 && model.TienePermisoRecalculo);
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function costo_VisibleBoton() {
    $("#btnRecalcularCO").hide();
    $("#btnSaveTmpRecalcularCO").hide();
    $("#txtCostoTotal").html("");

    if (VISIBLE_BOTON_RECALCULO) {
        $("#btnRecalcularCO").show();
    }
}

function costo_VisibleGuardarRecalculo(model) {
    if (model.CostoTotalOperacion != model.CostoTotalOperacionNuevo) {
        $("#txtCostoTotal").html(model.CostoTotalOperacion + "<br/>" + model.CostoTotalOperacionNuevo);
        $("#btnSaveTmpRecalcularCO").show();
    } else {
        $("#txtCostoTotal").html("No existe diferencia.");
    }
}

//////////////////////////////////////////////////////////
//// Útil
//////////////////////////////////////////////////////////
function mostrarExito(mensaje) {
    if (mensaje !== null && mensaje != '') {
        $('#mensaje').removeClass();
        $('#mensaje').html(mensaje);
        $('#mensaje').addClass('action-exito');
        $('#mensaje').show();
    }
}

function mostrarError(mensaje) {
    if (mensaje !== null && mensaje != '') {
        $('#mensaje').removeClass();
        $('#mensaje').html(mensaje);
        $('#mensaje').addClass('action-error');
        $('#mensaje').show();
    }
}

function mostrarAlerta(mensaje) {
    if (mensaje !== null && mensaje != '') {
        $('#mensaje').removeClass();
        $('#mensaje').html(mensaje);
        $('#mensaje').addClass('action-alert');
        $('#mensaje').show();
    }
}

function mostrarMensajeDefault(mensaje) {
    if (mensaje !== null && mensaje != '') {
        $('#mensaje').removeClass();
        $('#mensaje').html(mensaje);
        $('#mensaje').addClass('action-message');
        $('#mensaje').show();
    }
}

function mostrarMensajeCargaArchivo() {
    $("#mensaje2").hide();
    var strMensaje = 'Cargar la <b>Hoja de trabajo</b>, el nombre del archivo debe tener la siguiente estructura: <b>Htrabajo_generación_YYYYMMDD</b>.xlsm';

    if (strMensaje !== null && strMensaje != '') {
        $("#mensaje2").html(strMensaje);
        $("#mensaje2").show();
    }
}