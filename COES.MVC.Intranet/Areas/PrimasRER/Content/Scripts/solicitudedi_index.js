var controlador = siteRoot + 'PrimasRER/insumo/';
var ancho = 1000;

$(function () {
    $('#cntMenu').css("display", "none");

    $("#btnConsultar").click(function () {
        listarSolicitudesEdi();
    });

    listarSolicitudesEdi();

});

function listarSolicitudesEdi() {
    $('#listado').html('');
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;
    var ipericodi = parseInt($("#cbPeriodo").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ListarSolicitudesEdi",
        data: {
            ipericodi: ipericodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listado').html(evt.Resultado);
                $("#listado").css("width", (ancho) + "px");
                refreshDatatable();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

}

function descargarArchivoExcel(rersedcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ExportaraExcelEnergiaUnidad', 
        contentType: 'application/json;',
        data: JSON.stringify({
            rersedcodi: rersedcodi,
        }),
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado == "-1") {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
            else {
                window.location = controlador + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + evt.Resultado;
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error.");
        }
    });
}

function descargarArchivo(nombreArchivo) {
    if (nombreArchivo == null || nombreArchivo.trim() == "") {
        alert("No existe un nombre de archivo.");
        return;
    }

    window.location = controlador + 'ExportarSustentoSolicitudEdi?nombreArchivo=' + nombreArchivo;
}

function refreshDatatable() {
    var altotabla = parseInt($('#listado').height()) || 0;
    $('#tabla_solicitudes_edi').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": true,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": altotabla > 355 || altotabla == 0 ? 355 + "px" : "100%"
    });
}
