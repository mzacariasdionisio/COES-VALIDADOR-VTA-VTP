var controlador = siteRoot + 'Migraciones/AnexoA/';

$(function () {
    $('#btnBuscar').click(function () {
        cargarLista();
    });
    
    $('#txtFechaInicio').Zebra_DatePicker({
        //direction: -1
    });

    cargarLista();
});

function cargarLista() {
    var fechaInicio = $("#txtFechaInicio").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaAsignacionRRPFyRRSF',
        data: {
            fechaInicio: fechaInicio
        },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}