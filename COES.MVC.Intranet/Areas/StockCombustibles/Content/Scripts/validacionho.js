var controlador = siteRoot + 'StockCombustibles/Reportes/'
$(function () {
    $('#Fecha').Zebra_DatePicker({

    });
    $('#btnBuscar').click(function () {
        mostrarListado();
    });
    mostrarListado();

});

function mostrarListado() {
    var fecha = $('#Fecha').val();
    $.ajax({
        type: 'POST',
        url: controlador + "listavalidacionho",
        data: {
            fecha: fecha
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(evt);

            $('#tabla').dataTable({
                // "aoColumns": aoColumns(),
                "bAutoWidth": false,
                "bSort": false,
                "scrollY":530,
                "scrollX": true,
                "sDom": 't',
                "iDisplayLength": 100
            });
        },
        error: function () {
            alert("Ha ocurrido un error en lista");
        }
    });

}