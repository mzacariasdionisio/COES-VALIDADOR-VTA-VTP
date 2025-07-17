var controlador = siteRoot + 'IND/Reporte/';
var ancho = 1000;
var LISTA_TOT_TERMO = [];
var LISTA_TOT_HIDRO = [];
var CUADRO_1 = 1;

$(function () {
    $('#cntMenu').css("display", "none");

    $('#tab-container').easytabs({
        animate: false
    });
    $('#tab-container').bind('easytabs:after', function () {
        refrehDatatable();
    });
    $('#tab-container').easytabs('select', '#tabMsj');

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

    $('#btnGuardarBD').click(function () {
        guardarHistorico();
    });

    crearPupload();

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

    $("#codigo_version_asegurada").val('-1');
    $("#span_msg_asegurada").html('');

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

                    var color = evt.IndRecalculo.Irecaestado != "A" ? "red" : "blue";
                    $("#td_estado_recalculo").html('<span style="font-weight: bold;;color: ' + color + '">' + evt.IndRecalculo.IrecaestadoDesc + '</span>');

                    $("#desc_periodo").html(evt.IndPeriodo.Iperinombre);
                    $("#desc_recalculo").html(evt.IndRecalculo.Irecanombre);

                    //pa
                    $("#codigo_version_asegurada").val(evt.IdReporte);
                    $("#span_msg_asegurada").html(evt.Mensaje);

                    //k
                    $("#codigo_version_factork").val(evt.IdReporte);
                    $("#span_msg_factork").html(evt.Mensaje);

                    //cuadro 5
                    if (evt.IndRecalculo.Irecatipo == "PQ")
                        $("#span_msg_cuadro5").html("Despacho Ejecutado");
                    else
                        $("#span_msg_cuadro5").html("Medidores de Generación");

                    if (evt.IndRecalculo.Irecaestado == "A")
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
    inicializarForm();

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $('#tab-container').easytabs('select', '#tabMsj');

    var irecacodi = parseInt($("#cbRecalculo").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "VersionListadoHistorico",
        data: {
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

function descargarReporte(rptcodi, cuadro) {
    var famcodi = CUADRO_1 == cuadro ? 5 : 4;

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelCuadro",
        data: {
            irptcodi: rptcodi,
            centralIntegrante: "S",
            empresa: "-1",
            central: "-1",
            famcodi: famcodi,
            strFechaIni: $("#desc_fecha_ini").val(),
            strFechaFin: $("#desc_fecha_fin").val()
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
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

///////////////////////////
/// Procesar 
///////////////////////////

function guardarHistorico() {
    var irecacodi = parseInt($("#cbRecalculo").val()) || 0;

    var model = {
        IdRecalculo: irecacodi,
        ListaTotTermo: LISTA_TOT_TERMO,
        ListaTotHidro: LISTA_TOT_HIDRO,
    };

    var mensaje = '';
    if (model.IdRecalculo <= 0) {
        mensaje = mensaje + "<li>Debe seleccionar un recálculo.</li>";
    }

    if (mensaje == '') {
        if (confirm("¿Desea guardar en Base de datos?")) {
            $.ajax({
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                traditional: true,
                url: controlador + "GuardarHistoricoPR25",
                data: JSON.stringify({
                    indModel: model
                }),
                cache: false,
                success: function (data) {
                    if (data.Resultado != "-1") {
                        alert('La carga histórica se guardó correctamente.');

                        listadoVersion();

                    } else {
                        alert('Ha ocurrido un error: ' + data.Mensaje);
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        }
    } else {
        alert(mensaje);
    }
}

function crearPupload() {

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnUpload",
        url: controlador + "UploadExcelHistorico",
        multi_selection: false,
        filters: {
            max_file_size: '10mb',
            max_file_size: 0,
            mime_types: [
                { title: "Archivos xls", extensions: "xlsx" }
            ]
        },
        multipart_params: { recacodi: 0 },
        init: {
            FilesAdded: function (up, files) {
                inicializarForm();

                uploader.settings.multipart_params.recacodi = parseInt($("#cbRecalculo").val()) || 0;
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                //mostrarMensaje('mensajeInterr', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },

            FileUploaded: function (up, file, info) {
                var result = JSON.parse(info.response);
                mostrarMensajeArchivo(result);
            },
            UploadComplete: function (up, file, info) {
            },
            Error: function (up, err) {
                LISTA_TOT_TERMO = [];
                alert("Ha ocurrido un error");
            }
        }
    });

    uploader.init();
}

function mostrarMensajeArchivo(model) {
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 800;

    if (model.Resultado != "-1") {
        LISTA_TOT_TERMO = model.ListaTotTermo;
        LISTA_TOT_HIDRO = model.ListaTotHidro;

        $(".div_guardar_bd").show();

        //nombre de archivo
        $("#txtNombreArchivo").html(model.NombreArchivoUpload);

        //validaciones
        $('#listadoVal').html(model.RptHtml3);
        $("#listadoVal").css("width", (ancho) + "px");

        //Reporte consolidado
        $('#listado1').html(model.RptHtml1);
        $("#listado1").css("width", (ancho) + "px");

        $('#listado3').html(model.RptHtml2);
        $("#listado3").css("width", (ancho) + "px");

        refrehDatatable();
    } else {
        alert(model.Mensaje);
    }
}

function inicializarForm() {
    $("#listadoVal").html('');
    $("#listado1").html('');
    $("#listado3").html('');
    $("#txtNombreArchivo").html('');
    $(".div_guardar_bd").hide();

    LISTA_TOT_TERMO = [];
    LISTA_TOT_HIDRO = [];
}

function refrehDatatable() {
    $('#tabla_verif').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": "400px"
    });

    $('#tabla_reporte_consolidado1').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": "400px"
    });

    $('#tabla_reporte_consolidado4').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": "400px"
    });
}

