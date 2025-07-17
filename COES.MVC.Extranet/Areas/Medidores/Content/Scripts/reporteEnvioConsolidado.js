var controlador = siteRoot + 'Medidores/Reporte/';

$(function () {
    $('#txtMesIni').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            limpiarReporte();
        }
    });
    $('#txtMesFin').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            limpiarReporte();
        }
    });
    $('#cbEmpresa').change(function () {
        limpiarReporte();
    });

    $('#btnConsultar').click(function () {
        btnConsultar();
    });

    $('#btnExportar').click(function () {
        btnExportar();
    });
});

function limpiarReporte() {
    $('#reporteConsolidado').html('');
    $('#mensajePrincipal').hide();
}

function btnConsultar() {
    limpiarReporte();
    mostrarConsolidadoEnvio();
}

function btnExportar() {
    $('#mensajePrincipal').hide();
    exportarConsolidadoEnvio();
}

function mostrarConsolidadoEnvio() {
    var idEmpresa = $('#cbEmpresa').val();
    var mesIni = $('#txtMesIni').val();
    var mesFin = $('#txtMesFin').val();

    $.ajax({
        type: 'POST',
        async: true,
        data: {
            idEmpresa: idEmpresa,
            mesIni: mesIni,
            mesFin: mesFin
        },
        url: controlador + "ConsolidadoEnvioByEmpresa",
        success: function (evt) {
            $('#reporteConsolidado').html(evt);
        },
        error: function () {
            mostrarErrorPrincipal('Ocurrió un error.');
        }
    });
}

function exportarConsolidadoEnvio() {
    var idEmpresa = $('#cbEmpresa').val();
    var mesIni = $('#txtMesIni').val();
    var mesFin = $('#txtMesFin').val();

    $.ajax({
        type: 'POST',
        async: true,
        url: controlador + 'ExportaExcelConsolidadoEnvio',
        data: {
            idEmpresa: idEmpresa,
            mesIni: mesIni,
            mesFin: mesFin
        },
        beforeSend: function () {    
            mostrarExito('', "Descargando información ...");
        },
        success: function (result) {
            if (result.length > 0 && result != "-1") {               
                mostrarExito('', "<strong>Los datos se descargaron correctamente</strong>");
                window.location.href = controlador + 'DescargarExcelConsolidadoEnvio?archivo=' + result;
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });

    function mostrarExito(mensaje) {
        $('#mensajePrincipal').removeClass();
        $('#mensajePrincipal').html(mensaje);
        $('#mensajePrincipal').addClass('action-exito');
    }

}