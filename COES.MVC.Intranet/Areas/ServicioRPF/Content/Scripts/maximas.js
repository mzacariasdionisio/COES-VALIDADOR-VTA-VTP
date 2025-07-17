var controlador = siteRoot + 'serviciorpf/'

$(function () {

    $('#FechaConsulta').Zebra_DatePicker({

    });

    $('#btnConsultar').click(function () {
        mostrar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });   

    mostrar();
});

function mostrar() {
    $.ajax({
        type: "POST",
        url: controlador + "consulta/consultapotencia",
        data: { fecha: $('#FechaConsulta').val() },
        success: function (evt) {
            $('#listado').html(evt);
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "aaSorting": [[0, "asc"]],
                "destroy": "true"
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

function exportar() {   

    $.ajax({
        type: 'POST',
        url: controlador + 'consulta/generararchivopotencia',
        data: { fecha: $('#FechaConsulta').val() },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                window.location = controlador + "consulta/exportarpotencia";
            }
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function mostrarError() {
    alert("Ha ocurrido un error.");
}
