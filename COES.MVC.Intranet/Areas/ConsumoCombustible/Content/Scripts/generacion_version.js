var controlador = siteRoot + 'ConsumoCombustible/Version/';
$(function () {

    $('#fechaPeriodo').Zebra_DatePicker({
        onSelect: function () {
            listadoVersion();
        }
    });

    $("#btnConsultar").click(function () {
        listadoVersion();
    });

    $("#btnGenerar").click(function () {
        generarVersion();
    });

    listadoVersion();

});

///////////////////////////
/// Formulario 
///////////////////////////

function listadoVersion() {
    $('#listado').html('');

    var strFecha = $("#fechaPeriodo").val();
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;

    $.ajax({
        type: 'POST',
        url: controlador + "VersionListado",
        data: {
            strFecha: strFecha
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').html(evt.Resultado);

                $("#listado").css("width", (ancho) + "px");
                $('#tabla_version').dataTable({
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

///////////////////////////
/// Procesar 
///////////////////////////

function generarVersion() {
    var strFecha = $("#fechaPeriodo").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarVersion',
        data: {
            strFecha: strFecha
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                if (result.Resultado == "0") {
                    alert('No se generó nueva versión. No existen cambios.');
                } else {
                    alert('Se procesó correctamente.');

                    verReporte(result.Resultado);
                }
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

//
function verReporte(id) {
    window.location.href = siteRoot + "ConsumoCombustible/Version/Reporte?vercodi=" + id;
}

function verGrafico(id) {
    window.location.href = siteRoot + "ConsumoCombustible/Version/Grafico?vercodi=" + id;
}

function exportarReporte(id) {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoExcelReporte",
        data: {
            vercodi: id,
            empresa: "-1",
            central: "-1",
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