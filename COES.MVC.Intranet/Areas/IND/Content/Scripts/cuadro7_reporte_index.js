var controlador = siteRoot + 'IND/Reporte7/';
var ancho = 1000;
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


    $('#txtAnioIni').Zebra_DatePicker({
        format: 'Y',
        pair: $('#txtAnioFin'),
        onSelect: function (date) {
            $('#txtAnioFin').val(date);
            listadoVersion();
        }
    });
    $('#txtAnioFin').Zebra_DatePicker({
        format: 'Y',
        onSelect: function (date) {
            listadoVersion();
        }
    });

    $('#txtMesIni').Zebra_DatePicker({
        format: 'm Y',
        pair: $('#txtMesFin'),
        onSelect: function (date) {
            $('#txtMesFin').val(date);
        }
    });
    $('#txtMesFin').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function (date) {
            listadoVersion();
        }
    });

    var horDefault = $("#hfHorizonte").val();
    $("input[name=rbRadio][value=" + horDefault + "]").attr('checked', 'checked');
    $('input[type=radio][name=rbRadio]').change(function () {
        mostrarFila();
    });
    mostrarFila();

    $("#btnConsultarAnio").click(function () {
        listadoVersion();
    });
    $("#btnConsultarMes").click(function () {
        listadoVersion();
    });

    $("#btnProcesarCuadro").click(function () {
        guardarCuadroApp();
    });

    listadoVersion();
});

///////////////////////////
/// Formulario 
///////////////////////////

function mostrarFila() {
    $("#bloqueAnio").hide();
    $("#bloqueMes").hide();

    var horizonte = $('input[name="rbRadio"]:checked').val();
    if (horizonte == "VA")
        $("#bloqueAnio").show();

    if (horizonte == "VM")
        $("#bloqueMes").show();
}

function listadoVersion() {

    $('#listado').html('');
    $("#formulario_aplicativo").hide();

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $('#tab-container').easytabs('select', '#vistaVersion');

    var obj = getObjetoJsonCuadro();

    $.ajax({
        type: 'POST',
        url: controlador + "VersionListado",
        data: {
            horizonte: obj.horizonte,
            anioIni: obj.anioIni,
            anioFin: obj.anioFin,
            mesIni: obj.mesIni,
            mesFin: obj.mesFin
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').html(evt.Resultado);

                if (obj.horizonte == "VA") {
                    $("#desc_periodo_ini").html(obj.anioIni);
                    $("#desc_periodo_fin").html(obj.anioFin);
                } else {
                    $("#desc_periodo_ini").html(obj.mesIni);
                    $("#desc_periodo_fin").html(obj.mesFin);
                }
                $("#formulario_aplicativo").show();


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
    window.location.href = controlador + "ViewVersionCuadro7?irptcodi=" + id;
}
function verReporte(id, cuadro) {
    window.location.href = controlador + "ViewVersionCuadro7?irptcodi=" + id;
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

function guardarCuadroApp() {
    var obj = getObjetoJsonCuadro();

    $.ajax({
        type: 'POST',
        url: controlador + 'CuadroAplicativoGuardar',
        data: {
            horizonte: obj.horizonte,
            anioIni: obj.anioIni,
            anioFin: obj.anioFin,
            mesIni: obj.mesIni,
            mesFin: obj.mesFin
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
    obj.horizonte = $('input[name="rbRadio"]:checked').val();
    obj.anioIni = $('#txtAnioIni').val();
    obj.anioFin = $('#txtAnioFin').val();;
    obj.mesIni = $('#txtMesIni').val();
    obj.mesFin = $('#txtMesFin').val();

    return obj;
}
