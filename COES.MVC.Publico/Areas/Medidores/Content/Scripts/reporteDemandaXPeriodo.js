var controlador = siteRoot + 'Medidores/maximademanda/';
var bloqueHorarioData = {};
var tituloReporte = "";

$(function () {
    $("#cbCentral").val("1");

    $("#cbTipoGen").change(function () {
        consultar();
    });
    $("#cbCentral").change(function () {
        consultar();
    });
    $("#cbPeriodo").change(function () {
        consultar();
        actualizarTitulo();
    });
    $("#cbEmpresa").change(function () {
        consultar();
    });

    $('#mes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            consultar();
        }
    });

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    consultar();
    tituloReporte = $("#hfTitulo").val();
    bloqueHorarioData = JSON.parse($("#hfBloqueHorarioData").val());
    actualizarTitulo();
});

function actualizarTitulo() {
    var periodo = $("#cbPeriodo").val();
    var horario = "";
    for (var i = 0; i < bloqueHorarioData.length ; i++) {
        if (bloqueHorarioData[i].Tipo == periodo) {
            horario = bloqueHorarioData[i].Descripcion;
        }
    }
    var descripcion = tituloReporte + " en " + horario;
    $("#tituloReporte").html(descripcion);
}

function consultar() {
    $("#reporteDemandaPeriodo").html("");
    if (validarConsulta) {
        mostrarResumenMaximaDemanda();
    }
}

function validarConsulta() {
    return true;
}

function mostrarResumenMaximaDemanda() {
    var idEmpresa = $("#cbEmpresa").val();
    var mes = $("#mes").val();
    var tipoGeneracion = $('#cbTipoGen').val();
    var tipoCentral = $('#cbCentral').val();
    var bloqueHorario = $('#cbPeriodo').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ReporteDemandaPeriodo',
        data: {
            tipoCentral: tipoCentral,
            tipoGeneracion: tipoGeneracion,
            idEmpresa: idEmpresa,
            mes: mes,
            bloqueHorario: bloqueHorario
        },
        success: function (data) {
            $('#reporteDemandaPeriodo').html(data);

            $('#tablaConsolidado').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "bPaginate": false,
                "iDisplayLength": -1
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function exportar() {
    var idEmpresa = $("#cbEmpresa").val();
    var mes = $("#mes").val();
    var tipoGeneracion = $('#cbTipoGen').val();
    var tipoCentral = $('#cbCentral').val();
    var bloqueHorario = $('#cbPeriodo').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarDemandaPeriodo',
        data: {
            tipoCentral: tipoCentral,
            tipoGeneracion: tipoGeneracion,
            idEmpresa: idEmpresa,
            mes: mes,
            bloqueHorario: bloqueHorario
        },
        success: function (result) {
            if (result.length > 0) {
                archivo = result[0];
                nombre = result[1];
                if (archivo != '-1') {
                    window.location.href = controlador + 'DescargarArchivo?rutaArchivoTemp=' + archivo + "&nombreArchivo=" + nombre;
                } else {
                    alert("Error en descargar el archivo");
                }
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function () {
            alert('ha ocurrido un error al descargar el archivo excel.');
        }
    });
}