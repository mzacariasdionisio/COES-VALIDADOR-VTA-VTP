var controlador = siteRoot + 'Migraciones/AnexoA/'
$(function () {

        cargarLista();

});

function cargarLista() {

    var fechaInicio = $('#txtFechaInicio').val();
    var fechaFin = $('#txtFechaFin').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaRegistroCongestionesST',
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin },
        success: function (aData) {
            $('#listado').html(aData.Resultado);
            $('#idGraficoContainer').html('');
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });



}