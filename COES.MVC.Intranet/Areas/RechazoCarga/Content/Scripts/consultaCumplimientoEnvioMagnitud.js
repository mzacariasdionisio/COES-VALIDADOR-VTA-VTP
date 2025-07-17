var controlador = siteRoot + "rechazocarga/CumplimientoEnvioMagnitud/";

$(function () {
    $('#btnConsultar').click(function () {
        obtenerCumplimientoEnvioMagnitud();
    });

    $('#programa').change(function () {
        obtenerCuadros();
    });

    $('#cuadro').change(function () {
        obtenerSuministrador();
    });

    $('#btnExportar').click(function () {
        exportarExcel();
    });
});

function obtenerCuadros() {
    $("#cuadro").empty();
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerCuadrosPorPrograma',
        dataType: 'json',
        data: {
            programa: $("#programa").val()
        },
        success: function (cuadros) {
            $.each(cuadros, function (i, cuadro) {
                $("#cuadro").append('<option value="' + cuadro.Value + '">' +
                     cuadro.Text + '</option>');
            });
            obtenerSuministrador();
        },
        error: function (ex) {
            mostrarError('Consultar cuadros por programa: Ha ocurrido un error');
        }
    });
};

function obtenerSuministrador() {
    $("#suministrador").empty();
    var codigoCuadro = $("#cuadro").val();
    if (codigoCuadro == '') {
        codigoCuadro = 0;
    }
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerSuministrador',
        dataType: 'json',
        data: {
            codigoCuadro: codigoCuadro
        },
        success: function (cuadros) {
            $.each(cuadros, function (i, cuadro) {
                $("#suministrador").append('<option value="' + cuadro.Value + '">' +
                     cuadro.Text + '</option>');
            });
        },
        error: function (ex) {
            mostrarError('Consultar cuadros por programa: Ha ocurrido un error');
        }
    });
};

function obtenerCumplimientoEnvioMagnitud() {

    var programaSeleccionado = $("#programa").val();
    if (programaSeleccionado == '' || programaSeleccionado == '0') {
        mostrarAlerta('Debe seleccionar un programa.');
        return;
    }

    var cuadroSeleccionado = $("#cuadro").val();
    if (cuadroSeleccionado == '' || cuadroSeleccionado == '0') {
        mostrarAlerta('Debe seleccionar un cuadro de programa.');
        return;
    }

    $.ajax({
        type: "POST",
        url: controlador + "listarCumplimientoEnvioMagnitud",
        data: {
            programa: programaSeleccionado,
            cuadro: cuadroSeleccionado,
            suministrador: $("#suministrador").val(),
            cumplio: $("#cumplio").val(),
        },
        success: function (respuesta) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(respuesta);
            $('#tabla').dataTable({
                "ordering": true,
                "paging": false,
                "scrollY": 340,
                "scrollX": true,
                "bDestroy": true
            });
            mostrarMensaje("Consulta generada");
        },
        error: function () {
            mostrarError('Opcion Consultar: Ha ocurrido un error');
        }
    });
};

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}
function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function exportarExcel() {

    var programaSeleccionado = $("#programa").val();
    if (programaSeleccionado == '' || programaSeleccionado == '0') {
        mostrarAlerta('Debe seleccionar un programa.');
        return;
    }

    var cuadroSeleccionado = $("#cuadro").val();
    if (cuadroSeleccionado == '' || cuadroSeleccionado == '0') {
        mostrarAlerta('Debe seleccionar un cuadro de programa.');
        return;
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controlador + 'GenerarReporte',
        data: {
            programa: programaSeleccionado,
            cuadro: cuadroSeleccionado,
            suministrador: $("#suministrador").val(),
            cumplio: $("#cumplio").val(),
        },
        success: function (result) {
            if (result != "-1") {
                window.location.href = controlador + 'DescargarFormato?file=' + result;
                //mostrarExito("Se ha eliminado el registro correctamente.");
                //pintarBusqueda();
            }
            else {
                alert("Error al generar el archivo.");
            }
        },
        error: function () {
            mostrarAlerta("Error al generar el archivo");
        }
    });
}