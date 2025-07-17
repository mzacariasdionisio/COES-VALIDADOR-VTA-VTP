var controlador = siteRoot + 'TransfSeniales/ReporteSeniales/';
$(function () {


    $('#btnBuscar').click(function () {
        cargarCMRSeniales();
    });


    cargarCMRSeniales();
});


cargarCMRSeniales = function () {
    $.ajax({
        type: "POST",
        url: controlador + "ListaSeniales",
        data: {
            emprcodi: $('#cbEmpresa').val()
        },

        success: function (evt) {
            $('#vistaCMRSeniales').html(evt);

        },
        error: function () {
            mostrarError();
        }
    });
}


function mostrarError() {
    alert("Error");
}