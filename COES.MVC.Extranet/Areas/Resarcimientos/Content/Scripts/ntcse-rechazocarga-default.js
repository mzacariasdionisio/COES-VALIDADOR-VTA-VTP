$.message = "";
var controler = siteRoot + "resarcimientos/rechazocarga/";

$(document).ready(function () {
    //----paginado---
    buscarReporte();
    $('#CboPeriodo').change(function () {
        $.ajax({
            type: "POST",
            url: controler + "Optionselect",
            //dataType: 'json',
            data: {
                empresa: $('#CboEmpresasGeneradoras').val(),
                periodo: $('#CboPeriodo').val(),
                cliente: "",
                pentrega: "",
                ntension: ""
            },
            cache: false,
            success: function (resultado) {
                $('#content_selection').html(resultado);
                buscarReporte();
            },
            error: function (req, status, error) {
                mensajeOperacion(error);
                validaErrorOperation(req.status);
            }
        });
    });

    $('#btnExportar').click(function () {
        var url = "exportarrechazocarga";
        $.ajax({
            type: "POST",
            url: controler + url,
            //dataType: 'json',
            data: {
                empresa: $('#CboEmpresasGeneradoras').val(),
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
});

