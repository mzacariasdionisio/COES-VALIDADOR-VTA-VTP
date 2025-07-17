var controlador = siteRoot + 'PotenciaFirme/Insumos/';

$(function () {

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodo').change(function () {
    });

    $("#btnHistorico").click(function () {
        generarHistorico();
    });
});

function descargarReporteRER(numReporte) {

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoRER",
        data: {
            pfpericodi: $("#cbPeriodo").val(),
            tipoReporte: numReporte
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "0")
                    alert(evt.Mensaje);
                else
                    window.location = controlador + "ExportarRER?file_name=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function generarHistorico() {

    var f1 = $("#hfFIRer").val();
    var f2 = $("#hfFFRer").val();

    if (confirm("¿Desea generar el histórico RER (" + f1 + " - " + f2 + ")?")) {
        $.ajax({
            type: 'POST',
            url: controlador + "GenerarHistoricoRER",
            data: {
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    alert("Se ha generado correctamente el histórico de RER");
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