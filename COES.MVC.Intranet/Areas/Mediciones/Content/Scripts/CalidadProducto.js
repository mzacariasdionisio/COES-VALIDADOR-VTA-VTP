var controler = siteRoot + "mediciones/CalidadProducto/";
$(function () {
    $('#txtFechaInicial').Zebra_DatePicker({
    });
    $('#txtFechaFinal').Zebra_DatePicker({
    });

    $('#cbGps').multipleSelect();

    //$('#cbGps').val("1");

    $('#cbIntervalo').val("15");

    verificar();

    $('#btnBuscar').click(function () {
        verificar();
    });

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });
});
CrearGrafico = function(fechaInicio, fechaFinal, equipos, intervalo) {
    $.ajax({
        type: "POST",
        url: controlador + "Grafico",
        data: {
            fechaInicial: fechaInicio,
            fechaFinal: fechaFinal,
            equipos: equipos,
            intervalo: intervalo
        },
        success: function (evt) {
            $('#divGrafico').html(evt);
        },
        error: function () {
            //mostrarError();
        }
    });
};
CrearCuadros = function (fechaInicio, fechaFinal, equipos, intervalo) {
    $.ajax({
        type: "POST",
        url: controlador + "CuadrosResumen",
        data: {
            fechaInicial: fechaInicio,
            fechaFinal: fechaFinal,
            equipos: equipos,
            intervalo: intervalo
        },
        success: function (evt) {
            $('#divCuadros').html(evt);
        },
        error: function () {
            //mostrarError();
        }
    });
};
verificar = function () {
    var equipos = $('#cbGps').multipleSelect('getSelects');
    var fechaInicio = $('#txtFechaInicial').val();
    var fechaFinal = $('#txtFechaFinal').val();
    var intervalo = $('#cbIntervalo').val();
    if (fechaInicio != "" && fechaFinal != "" && equipos != "" ) {

        if (new Date(fechaFinal).getTime() >= new Date(fechaInicio).getTime()) {
            CrearGrafico(fechaInicio, fechaFinal, equipos, intervalo);
            CrearCuadros(fechaInicio, fechaFinal, equipos, intervalo);
        }
        else {
            mostrarMensaje("La fecha final debe ser mayor o igual a la fecha inicial.");
        }
    }
    else {
        mostrarMensaje("Ingrese fecha de inicio, fin y equipo Gps.");
    }
}