var controladorResultado = siteRoot + 'cortoplazo/InformacionOperativa/';

$(function () {

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#txtFecha').Zebra_DatePicker({
    });

    $('#cbSelectAll').click(function (e) {
        var table = $('#tableHorasReproceso');
        $('td input:checkbox', table).prop('checked', this.checked);
    });

    $('#btn-downloadfile').click(function () {
        var urls = validarSeleccion();
        document.getElementById("multiDownload").innerHTML = "";
        if (urls.length >= 1) {
            downloadBlobs(urls);
        }
        else {
            mostrarMensajeSeleccionUnico();
        }
    });

    consultar();
});

function consultar() {

    if ($('#txtFecha').val() != "") {
        $.ajax({
            type: 'POST',
            url: controladorResultado + 'listado',
            data: {
                fecha: $('#txtFecha').val(),
                tipoProceso: 0
            },
            success: function (evt) {
                $('#listado').html(evt);
                $('#tablaListado').dataTable({
                    "iDisplayLength": 25
                });
                $('#folder').hide();
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione fecha.');
    }
}

verArchivo = function (correlativo) {
    if ($('#txtFecha').val() != "") {
        $.ajax({
            type: 'POST',
            url: controladorResultado + 'FileManager',
            data: {
                fecha: $('#txtFecha').val(),
                correlativo: correlativo
            },
            success: function (result) {
                //$('.bread-crumb').css("display", 'none');
                $('#browserDocument').html(result);
                $('#folder').show();
                
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione fecha.');
    }

}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

downloadBlobs = function (urls) {
    var contenedor = $('#multiDownload');
    for (i in urls) {
        contenedor.append('<a href="' + controladorResultado + "download?url=" + urls[i] + '" class="file-download"></a>');
    }
    $('.file-download').multiDownload();
}

validarSeleccion = function () {
    var urls = [];
    $('#tbDocumentLibrary tbody tr').each(function (i, row) {
        var $actualrow = $(row);
        $checkbox = $actualrow.find('input:checked');
        if ($checkbox.is(':checked')) {
            urls.push($checkbox.val());
        }
    });
    return urls;
}

mostrarMensajeSeleccionUnico = function () {
    alert("Seleccion un archivo!");
}