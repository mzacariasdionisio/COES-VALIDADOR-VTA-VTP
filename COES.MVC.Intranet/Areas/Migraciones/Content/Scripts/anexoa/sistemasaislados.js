var controlador = siteRoot + 'Migraciones/AnexoA/';

$(function () {

    cargarLista();
});

function cargarLista() {
    $.ajax({
        type: 'POST',
        data: {
            fechaInicio: getFechaInicio(),
            fechaFin: getFechaFin()
        },
        url: controlador + 'CargarListaSistemasAisladosTemporales',
        success: function (aData) {
            $('#listado').html(aData.Resultado);

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


