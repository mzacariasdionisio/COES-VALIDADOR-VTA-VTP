var controlador = siteRoot + 'ReportePotencia/Configuracion/';
$(function () {
    
    buscarConfiguracion();
    $('#btnConsultar').click(function () {
        buscarConfiguracion();
    });
    
    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});
buscarConfiguracion = function () {
    
    $.ajax({
        type: 'POST',
        url: controlador + "Listado",
        data: {
            iEmpresa: $('#cbEmpresa').val(),
            sEstado: $('#cbEstado').val()
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError();
        }
    });
};