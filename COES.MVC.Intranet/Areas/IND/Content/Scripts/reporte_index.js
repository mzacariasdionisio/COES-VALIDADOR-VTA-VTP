var controlador = siteRoot + 'IND/Reporte/';
var ancho = 1000;
var CUADRO_FACTOR_K = 3;
var CUADRO_CUADRO5 = 5;
var CUADRO_CUADRO7 = 7;

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
    //Inicio: IND.PR25.2022
    if (cuadro == 3)
    {
        $("#numero_dias").html('');
    }
    //Fin: IND.PR25.2022

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

                    //Inicio: IND.PR25.2022
                    if (cuadro == 3)
                    {
                        $("#numero_dias").html(evt.NumeroDias); 
                    }
                    //Fin: IND.PR25.2022
                    
                    //cuadro 5
                    if (evt.IndRecalculo.Irecatipo == "PQ")
                        $("#span_msg_cuadro5").html("Despacho Ejecutado");
                    else
                        $("#span_msg_cuadro5").html("Medidores de Generación");

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
        case CUADRO_FACTOR_K:
            window.location.href = controlador + "ViewVersionCuadro3?irptcodi=" + id;
            break;
        case CUADRO_CUADRO5:
            window.location.href = controlador + "ViewVersionCuadro5?irptcodi=" + id;
            break;
        case CUADRO_CUADRO7:
            window.location.href = controlador + "ViewVersionCuadro7?irptcodi=" + id;
            break;
        default:
            window.location.href = controlador + "ViewVersionCuadro?irptcodi=" + id;
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
    $("#div_verificacion_proceso").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'CuadroAplicativoVerificarData',
        data: {
            cuadro: obj.cuadro,
            irecacodi: obj.irecacodi,
            tipoReporte: obj.tipoReporte,
            tiempo: obj.tiempo,
            medicionorigen: obj.medicionorigen,
            famcodi: obj.famcodi,
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                $(".div_verificacion_2").show();
                $("#div_verificacion_proceso").html(result.Resultado);

                setTimeout(function () {

                    $('#tabla_verif').dataTable({
                        "sPaginationType": "full_numbers",
                        "destroy": "true",
                        "ordering": false,
                        "searching": false,
                        "iDisplayLength": 15,
                        "info": false,
                        "paging": false,
                        "scrollX": true,
                        "scrollY": $('#div_verificacion_proceso').height() > 400 ? 400 + "px" : "100%"
                    });
                }, 50);

                //Inicio: IND.PR25.2022
                if (obj.cuadro == 3 && result.ListaVerificacion != null && result.ListaVerificacion.length > 0) {
                    $("#btnProcesarCuadro").hide();
                }
                else {
                    $("#btnProcesarCuadro").show();
                }
                //Fin: IND.PR25.2022
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
        url: controlador + 'CuadroAplicativoGuardar',
        data: {
            cuadro: obj.cuadro,
            irecacodi: obj.irecacodi,
            tipoReporte: obj.tipoReporte,
            tiempo: obj.tiempo,
            medicionorigen: obj.medicionorigen,
            famcodi: obj.famcodi
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error: ' + result.Mensaje);
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
    obj.tipoReporte = "A";
    obj.famcodi = parseInt($("#famcodi_cuadro").val()) || 0;
    obj.tiempo = $('input[name=form_tiempo]:checked').val();
    obj.medicionorigen = null;

    return obj;
}
