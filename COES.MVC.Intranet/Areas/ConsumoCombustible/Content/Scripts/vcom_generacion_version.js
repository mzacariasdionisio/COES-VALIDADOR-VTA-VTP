var controlador = siteRoot + 'ConsumoCombustible/VCOM/';
$(function () {

    $('#fechaPeriodo').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            listadoVersion();
        }
    });

    $("#btnConsultar").click(function () {
        listadoVersion();
    });

    $("#btnEjecutar").click(function () {
        ejecutarReporteDiario();
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

                    listadoVersion();
                    //verReporte(result.Resultado);
                }
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function ejecutarReporteDiario() {
    var strFecha = $("#fechaPeriodo").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'EjecutarReporteDiario',
        data: {
            strFecha: strFecha
        },
        cache: false,
        success: function (result) {
            if (result.Resultado == '-1') {
                alert('Ha ocurrido un error:' + result.Mensaje);
            } else {
                if (result.Resultado != "") {
                    alert(result.Resultado);
                } else {
                    alert('No se generó nuevas versiones. No existen cambios.');
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
    window.location.href = siteRoot + "ConsumoCombustible/VCOM/Reporte?vercodi=" + id;
}

function guardarRepVcom(id) {
    if (confirm('¿Desea guardar los datos en la Tabla PRIE REP_VCOM?')) {
        $.ajax({
            type: 'POST',
            url: controlador + "GuardarRepVcom",
            data: {
                vercodi: id,
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert('Se procesó correctamente.');

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