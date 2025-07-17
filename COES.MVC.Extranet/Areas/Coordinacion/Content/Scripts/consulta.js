var controlador = siteRoot + 'coordinacion/registroobservacion/';
var hot = null;

$(function () {   
    cargarGrilla();

    $('#btnConsultar').on('click', function () {
        cargarGrilla2();
    });
});


cargarGrilla = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'cargargrillareporte',
        data: {
            idEmpresa: -2
        },
        success: function (evt) {
            $('#cntReporte').html(evt);
            $('#listaSeniales').dataTable({
                "iDisplayLength": 50,
                "scrollX": true
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    })
}

cargarGrilla2 = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'cargargrillareporte',
        data: {
            idEmpresa: $('#cbEmpresa').val()
        },
        success: function (evt) {
            $('#cntReporte').html(evt);
            $('#listaSeniales').dataTable({
                "iDisplayLength": 100,
                "scrollX": true
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    })
}


mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.textAlign = 'center';
    td.style.color = '#fff';
    td.style.backgroundColor = '#2980B9';
};

var disabledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = '#3B83C1';
    td.style.backgroundColor = '#F5F5F5';
};