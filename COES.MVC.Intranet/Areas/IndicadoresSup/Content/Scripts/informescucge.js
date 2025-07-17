var controlador = siteRoot + 'IndicadoresSup/InformesCUCGE/'

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });
    $('.pickermes').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            CargarInformesCUCGE();
        }
    });

    CargarInformesCUCGE();
});

function CargarInformesCUCGE() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarInformesCUCGE',
        data: { mesanio: $("#txtMesAnio").val() },
        success: function (aData) {
            $('#listado1').html(aData.Resultados[0]);
            $('#listado2').html(aData.Resultados[1]);

        },
        error: function (err) { alert("Ha ocurrido un error"); }
    });
}