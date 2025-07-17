var controlador = siteRoot + 'CostoOportunidad/Reporte/';


$(function () {

    $('#tab-container').easytabs({
        animate: false
    });
    $('#FechaConsulta').Zebra_DatePicker({
    });
    $('#btnBuscar').click(function () {
        buscarDatos();
    });
    $('#btnExpotar').click(function () {
        exportarExcelReporte();
    });
    buscarDatos();
});
function buscarDatos() {
    mostrarListado();
 }

function mostrarListado() {
    
    $.ajax({
        type: 'POST',
        url: controlador + "ListaReporte",
        data: {
            fecha: $('#FechaConsulta').val()
        },
        success: function (evt) {
            $('#divDespachoConReserva').html(evt);
            if ($('#tabla th').length > 1) {
                $('#tabla').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 800,
                    "scrollX": 1200,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }
            mostrarDespachoSinReserva();
        },
        error: function (evt) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarDespachoSinReserva() {
    
    $.ajax({
        type: 'POST',
        url: controlador + "ListaDespachoSinReserva",
        data: {
            fecha: $('#FechaConsulta').val()
        },
        success: function (evt) {
            $('#divDespachoSinReserva').html(evt);
            if ($('#tablaSin th').length > 1) {
                $('#tablaSin').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 800,
                    "scrollX": 1200,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }
            mostrarReservaEjec();
        },
        error: function (evt) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarReservaEjec() {
    
    $.ajax({
        type: 'POST',
        url: controlador + "ReporteReservaEject",
        data: {
            fecha: $('#FechaConsulta').val()
        },
        success: function (evt) {
            $('#divReservaEjecutada').html(evt);
            if ($('#tabla2 th').length > 1) {
                $('#tabla2').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 800,
                    "scrollX": 1200,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }
            mostrarReservaProg();
        },
        error: function (evt) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarReservaProg() {

    $.ajax({
        type: 'POST',
        url: controlador + "ReporteReservaProg",
        data: {
            fecha: $('#FechaConsulta').val()
        },
        success: function (evt) {
            //$('#divReservaProgramada').css("width", "100%");
            $('#divReservaProgramada').html(evt);
            if ($('#tabla3 th').length > 1) {
                $('#tabla3').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 800,
                    "scrollX": 1200,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }
            mostrarCOConReserva();
        },
        error: function (evt) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarCOConReserva() {
   
    $.ajax({
        type: 'POST',
        url: controlador + "ReporteCOConReserva",
        data: {
            fecha: $('#FechaConsulta').val()
        },
        success: function (evt) {
            //$('#divDespachoCOConReserva').css("width", "100%");
            $('#divDespachoCOConReserva').html(evt);
            if ($('#tabla4 th').length > 1) {
                $('#tabla4').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 800,
                    "scrollX": 1200,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }
            mostrarCOSinReserva()
        },
        error: function (evt) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarCOSinReserva() {
    $.ajax({
        type: 'POST',
        url: controlador + "ReporteCOSinReserva",
        data: {
            fecha: $('#FechaConsulta').val()
        },
        success: function (evt) {
            //$('#divDespachoCOSinReserva').css("width", "100%");
            $('#divDespachoCOSinReserva').html(evt);
            if ($('#tabla5 th').length > 1) {
                $('#tabla5').dataTable({
                    "bAutoWidth": false,
                    "bSort": false,
                    "scrollY": 800,
                    "scrollX": 1200,
                    "sDom": 't',
                    "iDisplayLength": 100
                });
            }

        },
        error: function (evt) {
            alert("Ha ocurrido un error");
        }
    });
}

function exportarExcelReporte() {

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteXls',
        data: {
            fecha: $('#FechaConsulta').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {// si hay elementos
                window.location = controlador + "ExportarReporte";
            }
            if (result == 2) { // sino hay elementos
                alert("No existen registros !");
            }
            if (result == -1) {
                alert("Error en reporte result")
            }
        },
        error: function () {
            alert("Error en reporte");;
        }
    });
}