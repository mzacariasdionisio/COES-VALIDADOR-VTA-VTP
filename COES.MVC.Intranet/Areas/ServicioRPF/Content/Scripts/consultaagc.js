var controlador = siteRoot + 'serviciorpf/envioagc/'

$(function () {
    $('#txtFechaConsulta').Zebra_DatePicker({
        direction:false
    });

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });
   
    consultar();
});

function consultar() {
    $.ajax({
        type: "POST",
        url: controlador + "listado",
        data: {
            fecha: $('#txtFechaConsulta').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            oTable = $('#tabla').dataTable({
                "aaSorting": [[1, "asc"]],
                "destroy": "true",
                "aoColumnDefs": [
                    { 'bSortable': false, 'aTargets': [5] }
                ],
                "sDom": 'fti',
                "iDisplayLength": 400
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function exportar() {

    $('#mensaje').removeClass();
    $('#mensaje').html("");
    var puntos = "";

    $('#tabla tbody tr').each(function (i, row) {
        var $actualrow = $(row);
        $checkbox = $actualrow.find('input:checked');
        if ($checkbox.is(':checked')) {
            puntos = puntos + $checkbox.val() + ",";
        }
    });

    if (puntos != "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'generarreporte',
            data: {
                puntos: puntos,
                fecha: $('#txtFechaConsulta').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result.Result == 1) {
                    window.location = controlador + "descargarreporte?fileName=" + result.FileName;
                }
                else if (result.Result == -1) {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione al menos un registro.');
    }
}


function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};