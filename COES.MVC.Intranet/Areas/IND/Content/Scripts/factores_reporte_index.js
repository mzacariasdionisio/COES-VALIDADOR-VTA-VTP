var controlador = siteRoot + 'IND/ReporteFactores/';
var ancho = 1000;

var FACTOR_FORT_TERMICO = 8;
var FACTOR_PROG_TERMICO = 9;
var FACTOR_PROG_HIDRO = 10;
const FACTOR_PRESENCIA = 11;

$(function () {
    $('#cntMenu').css("display", "none");

    $('#tab-container').easytabs({
        animate: false
    });

    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });

    $('#tab-container').easytabs('select', '#vistaVersion');

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodo').change(function () {
        listadoRecalculo();
    });

    $('#cbRecalculo').change(function () {
        listadoVersion();
    });

    $('#desc_fecha_ini').Zebra_DatePicker({});

    $('#desc_fecha_fin').Zebra_DatePicker({});

    $("#btnBuscar").click(function () {
        mostrarListado();
    });

    $("#btnActualizarParam").click(function () {
        getRecalculo();
    });

    $("#btnVerificarData").click(function () {
        verificarCuadroApp();
    });

    $("#btnProcesarCuadro").click(function () {
        guardarCuadroApp();
    });

    listadoVersion();

});

///////////////////////////
/// Formulario 
///////////////////////////

function listadoPeriodo() {

    var anio = parseInt($("#cbAnio").val()) || 0;

    $("#cbPeriodo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Iperinombre, item.Ipericodi);
                    });
                } else {
                    $('#cbPeriodo').get(0).options[0] = new Option("--", "0");
                }

                listadoRecalculo();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function listadoRecalculo() {

    var ipericodi = parseInt($("#cbPeriodo").val()) || 0;

    $("#cbRecalculo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "RecalculoListado",
        data: {
            ipericodi: ipericodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaRecalculo.length > 0) {
                    $.each(evt.ListaRecalculo, function (i, item) {
                        $('#cbRecalculo').get(0).options[$('#cbRecalculo').get(0).options.length] = new Option(item.Irecanombre, item.Irecacodi);
                    });
                } else {
                    $('#cbRecalculo').get(0).options[0] = new Option("--", "0");
                }

                listadoVersion();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getRecalculo() {
    var irecacodi = parseInt($("#cbRecalculo").val()) || 0;
    var cuadro = parseInt($("#hfCuadro").val()) || 0;

    $(".div_verificacion_2").hide();
    $("#formulario_excel_historico").hide();
    $(".td_filtro_fecha").hide();

    $("#td_estado_recalculo").html('');
    $("#desc_periodo").html('');
    $("#desc_recalculo").html('');

    $("#codigo_version_cuadro5").val('-1');
    $("#span_msg_cuadro5").html('');

    $("#cbRecPf").empty();
    $("#codigo_version_cuadro1").val('-1');
    $("#codigo_usar_app_cuadro1").val('-1');
    $("#span_msg_cuadro1").html('');

    $("#codigo_version_cuadro2").val('-1');
    $("#codigo_usar_app_cuadro2").val('-1');
    $("#span_msg_cuadro2").html('');

    $("#codigo_version_cuadro14").val('-1');
    $("#codigo_usar_app_cuadro14").val('-1');
    $("#span_msg_cuadro14").html('');

    $("#codigo_version_cuadro4").val('-1');
    $("#codigo_usar_app_cuadro4").val('-1');
    $("#span_msg_cuadro4").html('');

    $("#msjProcesar").hide();
    $("#formulario_aplicativo").hide();

    if (irecacodi > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + "RecalculoDatos",
            data: {
                irecacodi: irecacodi,
                idCuadro: cuadro,
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    $(".td_filtro_fecha").show();

                    $("#formulario_excel_historico").show();

                    $("#desc_fecha_ini").val(evt.FechaIni);
                    $("#desc_fecha_fin").val(evt.FechaFin);

                    var color = evt.IndRecalculo.Estado != "A" ? "red" : "blue";
                    $("#td_estado_recalculo").html('<span style="font-weight: bold;;color: ' + color + '">' + evt.IndRecalculo.IrecaestadoDesc + '</span>');

                    $("#desc_periodo").html(evt.IndPeriodo.Iperinombre);
                    $("#desc_recalculo").html(evt.IndRecalculo.Irecanombre);

                    //factor 1 y 2
                    $("#codigo_version_cuadro1").val(evt.IdReporte);
                    $("#codigo_usar_app_cuadro1").val(evt.UsarAplicativo);

                    $("#codigo_version_cuadro2").val(evt.IdReporte2);
                    $("#codigo_usar_app_cuadro2").val(evt.UsarAplicativo);

                    $("#codigo_version_cuadro14").val(evt.IdReporte3);
                    $("#span_msg_cuadro14").html(evt.Mensaje3);

                    if (evt.UsarAplicativo > 0) {
                        $("#span_msg_cuadro1").html(evt.Mensaje);
                        $("#span_msg_cuadro2").html(evt.Mensaje2);
                    } else {
                        $("#span_msg_cuadro1").html("Se utilizará información de la Carga Histórica.");
                    }

                    //factor 3
                    $("#codigo_version_cuadro4").val(evt.IdReporte);
                    $("#codigo_usar_app_cuadro4").val(evt.UsarAplicativo);
                    if (evt.UsarAplicativo > 0) {
                        $("#span_msg_cuadro4").html(evt.Mensaje);
                    } else {
                        $("#span_msg_cuadro4").html("Se utilizará información de la Carga Histórica.");
                    }

                    //factor 4
                    $("#codigo_version_cuadro5").val(evt.IdReporte);
                    $("#span_msg_cuadro5").html(evt.Mensaje);

                    if (evt.IndRecalculo.Estado == "A")
                        $("#formulario_aplicativo").show();
                    else
                        $("#msjProcesar").show();
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function listadoVersion() {
    getRecalculo();

    $('#listado').html('');
    $("#formulario_aplicativo").hide();
    $('#formulario_excel_historico').hide();

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $('#tab-container').easytabs('select', '#vistaVersion');

    var cuadro = parseInt($("#hfCuadro").val()) || 0;
    var irecacodi = parseInt($("#cbRecalculo").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "VersionListado",
        data: {
            cuadro: cuadro,
            irecacodi: irecacodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').html(evt.Resultado);

                $("#listado").css("width", (ancho) + "px");
                $('.tabla_version_x_recalculo').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "ordering": false,
                    "searching": false,
                    "iDisplayLength": -1,
                    "info": false,
                    "paging": false,
                    "scrollX": true,
                    "scrollY": "100%"
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function editarReporte(id, cuadro) {
    switch (cuadro) {
        case FACTOR_PRESENCIA:
            window.location.href = controlador + "ViewFactorPresencia?irptcodi=" + id;
            break;
        case FACTOR_PROG_HIDRO:
            window.location.href = controlador + "ViewFactorProgramadoHidro?irptcodi=" + id;
            break;
        case FACTOR_FORT_TERMICO:
            window.location.href = controlador + "ViewFactorFortTermico?irptcodi=" + id;
            break;
        case FACTOR_PROG_TERMICO:
            window.location.href = controlador + "ViewFactorProgTermico?irptcodi=" + id;
            break;

    }
}

function verReporte(id, cuadro) {
    switch (cuadro) {
        case FACTOR_PRESENCIA:
            window.location.href = controlador + "ViewFactorPresencia?irptcodi=" + id;
            break;
        case FACTOR_PROG_HIDRO:
            window.location.href = controlador + "ViewFactorProgramadoHidro?irptcodi=" + id;
            break;
        case FACTOR_FORT_TERMICO:
            window.location.href = controlador + "ViewFactorFortTermico?irptcodi=" + id;
            break;
        case FACTOR_PROG_TERMICO:
            window.location.href = controlador + "ViewFactorProgTermico?irptcodi=" + id;
            break;

    }
}

function aprobarVersion(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'AprobarVersion',
        data: {
            irptcodi: id
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                alert("Se actualizó correctamente el registro.");
                listadoVersion();
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function descargarReporte(id) {

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelCuadro",
        data: {
            irptcodi: id,
            flagDescargar: "S",
            famcodi: -1
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "0")
                    alert(evt.Mensaje);
                else
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

}

function refrehDatatable() {
    $('.tabla_version_x_recalculo').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": false,
        "iDisplayLength": -1,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": "100%"
    });
}

///////////////////////////
/// Procesar 
///////////////////////////

function verificarCuadroApp() {
    var obj = getObjetoJsonCuadro();
    $(".div_verificacion_2").hide();

    $.ajax({
        type: 'POST',
        url: controlador + 'FactorAplicativoVerificarData',
        data: {
            cuadro: obj.cuadro,
            irecacodi: obj.irecacodi,
            idReporteCuadro1: obj.idReporteCuadro1,
            idReporteCuadro2: obj.idReporteCuadro2,
            idReporteCuadro14: obj.idReporteCuadro14,
            idReporteCuadro4: obj.idReporteCuadro4,
            idReporteCuadro5: obj.idReporteCuadro5,
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                if (result.FlagContinuar) {
                    $(".div_verificacion_2").show();
                    $("#div_verificacion_proceso").html(result.Resultado);
                } else {
                    alert("Existen validaciones que deben ser absueltas para continuar con el proceso.");
                }
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function guardarCuadroApp() {
    var obj = getObjetoJsonCuadro();

    $.ajax({
        type: 'POST',
        url: controlador + 'FactorAplicativoGuardar',
        data: {
            cuadro: obj.cuadro,
            irecacodi: obj.irecacodi,
            idReporteCuadro1: obj.idReporteCuadro1,
            idReporteCuadro2: obj.idReporteCuadro2,
            idReporteCuadro14: obj.idReporteCuadro14,
            idReporteCuadro4: obj.idReporteCuadro4,
            idReporteCuadro5: obj.idReporteCuadro5,
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                if (result.IdReporte > 0) {
                    alert('Se procesó correctamente.');

                    editarReporte(result.IdReporte, obj.cuadro);
                } else {
                    alert('No se generó nueva versión. No existen cambios.');
                }
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function getObjetoJsonCuadro() {
    var obj = {};

    obj.cuadro = parseInt($("#hfCuadro").val()) || 0;
    obj.irecacodi = parseInt($("#cbRecalculo").val()) || 0;
    obj.pfrecacodi = parseInt($("#cbRecPf").val()) || 0;
    obj.idReporteCuadro1 = parseInt($("#codigo_version_cuadro1").val()) || 0;
    obj.idReporteCuadro2 = parseInt($("#codigo_version_cuadro2").val()) || 0;
    obj.idReporteCuadro14 = parseInt($("#codigo_version_cuadro14").val()) || 0;
    obj.idReporteCuadro4 = parseInt($("#codigo_version_cuadro4").val()) || 0;
    obj.idReporteCuadro5 = parseInt($("#codigo_version_cuadro5").val()) || 0;

    return obj;
}
