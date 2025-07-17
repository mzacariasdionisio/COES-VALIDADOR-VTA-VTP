var controlador = siteRoot + 'Interconexiones/ampliacion/';
var listaPeriodoMedidor = [];
$(function () {

    $('#idFecha').Zebra_DatePicker({
    });
    $('#btnBuscar').click(function () {
        mostrarListado();
    });
    $('#btnAmpliar').click(function () {
        agregarAmpliacion();
    });
    $("#cbEmpresa").val($("#hfEmpresa").val());
    mostrarListado();
});

function mostrarListado() {
    var empresa = $('#cbEmpresa').val();
    var estado = $('#cbEstado').val();

    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            empresa: empresa, fecha: $('#idFecha').val() },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function agregarAmpliacion() {

    $.ajax({
        type: 'POST',
        url: controlador + "AgregarAmpliacion",
        data: {
            periodoIni: 0
        },
        success: function (evt) {

            $('#detalleAmpliacion').html(evt);

            setTimeout(function () {
                $('#validaciones').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 50);
        },
        error: function () {
            alert("Error en mostrar periodo");
        }
    });
}

function grabarAmpliacion() {

    var hora = $('#cbHora').val();
    
    $('#validaciones').bPopup().close();

    $.ajax({
        type: 'POST',
        url: controlador + "grabarValidacion",
        dataType: 'json',
        data: {
            fecha: $('#idFechaEnvio').val(), hora: hora, empresa: $('#cbEmpresa').val()
        },
        success: function (evt) {
            mostrarListado();
        },
        error: function () {
            alert("Error en Grabar Ampliación");
        }
    });
}