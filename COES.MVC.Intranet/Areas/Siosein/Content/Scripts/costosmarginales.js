var controlador = siteRoot + 'siosein/CalculoCostosMarginales/';

$(function () {

    $('#txtMes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            mostrarReporteCostosMarginales()
    }
    });

    $('#btnBuscar').click(function () {
        mostrarReporteCostosMarginales()
    });

    $('#btnExportar').click(function () {
        exportarExcel();
    });
});

function actualizarPantalla() {
    $(window).on('resize', function () {
        var anchoActual = $(".search-content").width();

        $(".panel-container").css({
            width: anchoActual
        });
    });
}

function mostrarReporteCostosMarginales() {
    $.ajax({
        type: 'POST',
        url: controlador + "ViewReporteCostosMarginales",
        data: {            
            mes: $('#txtMes').val()
        },
        success: function (dataHtml) {
            var anchoActual = $(".search-content").width();
            $('#listado').html(dataHtml.Resultado);
            $(".panel-container").css({
                width: anchoActual
            });

            actualizarPantalla();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
