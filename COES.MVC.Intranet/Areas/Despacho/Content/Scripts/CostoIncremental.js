var controlador = siteRoot + 'Despacho/CostoIncremental/'

$(function () {

    $('#Fecha').Zebra_DatePicker({
    });

    $('#btnConsultar').click(function () {
        mostrarListado();
    });

    mostrarListado();
});

mostrarListado = function () {

    var fecha = $('#Fecha').val();  

    $.ajax({
        type: 'POST',
        url: controlador + "listado",
        data: {
            fecha: fecha
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 600,
                "scrollX": true,
                "sDom": 't',
                "ordering": true,
                "iDisplayLength": 200
            });
        },
        error: function () {
            mostrarError();
        }
    });
}


mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

ExportarExcel = function () {
    var fecha = $('#Fecha').val();    

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoReporte",
        data: {
            fecha: fecha
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "ExportarExcel";
            }
            if (result == -1) {
                alert("Error al imprimir.");
            }
        },
        error: function () {
            mostrarError();
        }
    });

}
