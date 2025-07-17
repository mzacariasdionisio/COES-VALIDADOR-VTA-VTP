$.message = "";
var controler = siteRoot + "resarcimientos/reporteentrega/";

$(document).ready(function () {

    $('#btnBuscar').click(function () {
        $('.content-hijo .search-content').toggle();
    });
    $('#btnExportarReporte').click(function () {
        var url = "exportarpuntoentrega";

        $.ajax({
            type: "POST",
            url: controler + url,
            data: {
                empresa: $('#RCboEmpresasGeneradoras').val(),
                periodo: $('#CboPeriodo').val(),
                cliente: $('#CboCliente').val(),
                pentrega: $('#CboPuntoEntrega').val(),
                ntension: $('#CboTension').val()
            },
            cache: false,
            success: function (resultado) {
                window.location = controler + 'descargar';
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });

    });

    $('#CboCliente').change(function () {
        var idCliente = $('#CboCliente').val();
        $.ajax({
            type: "POST",
            url: controler + "obtenerPuntoEntregas",

            data: {
                idCliente: idCliente
            },
            cache: false,
            success: function (resultado) {

                $('#CboPuntoEntrega > option').remove();

                if (idCliente == 0) {
                    $('#CboPuntoEntrega').append($('<option>', { value: "0", text: "(TODOS)" }));
                }
                for (var i = 0; i < resultado.length; i++) {
                    $('#CboPuntoEntrega').append($('<option>', { value: resultado[i].Barrcodi, text: resultado[i].BarrNombre }));
                }

            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });
    });

    $('#btnRefrescarReporte').click(function () {
        $('#RCboEmpresasGeneradoras').change();
    });
    $('#CboCliente').change(function () {
        $('#RCboEmpresasGeneradoras').change();
    });
    $('#CboPuntoEntrega').change(function () {
        $('#RCboEmpresasGeneradoras').change();
    });
    $('#CboTension').change(function () {
        $('#RCboEmpresasGeneradoras').change();
    });
    $('#RCboPeriodo').change(function () {
        $('#RCboEmpresasGeneradoras').change();
    });
    $('#RCboEmpresasGeneradoras').change(function () {
    });
    $('#btnConsultar').click(function () {

        buscarReporte();
    });

    //----paginado---
    buscarReporte();

});







