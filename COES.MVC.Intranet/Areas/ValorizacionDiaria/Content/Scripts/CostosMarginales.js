var controlador = siteRoot + 'ValorizacionDiaria/CostosMarginales/';

$(function () {
    $('#txtFecha').Zebra_DatePicker({

    });
});

//
$('#btnEjecutar').click(function () {    
    EjecutarCostosMarginales();
    
});


var EjecutarCostosMarginales = function () {

    var fecha = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + "EjecutarCostosMarginales",
        data: { fecha: fecha },
        dataType: 'json',
        cache: false,
        async: false,
        success: function (resultado) {
            retorno = resultado;
            if (resultado == 1) {
                alert('Proceso ejecutado con exito!');
            } else {
                alert('No se ha conseguido ejecutar el proceso');
            }
        },
        error: function () {
            alert('Ocurrio un problema al generar el proceso');
        }
    });
};
