var controlador = siteRoot + 'Migraciones/AnexoA/'
$(function () {
    cargarLista();
});

function cargarLista() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCostoMarginalesCP',
        data: { fechaInicio: $("#txtFechaInicio").val(), fechaFin: $("#txtFechaFin").val() },
        success: function (aData) {
            //$('#tab-container').html(aData.Resultado);
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(aData);
            $('#listado').css("overflow-x", "auto");
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}