var controlador = siteRoot + 'DirectorioImpugnaciones/AdminAgenda/'

$(function () {
    $('#btnBuscar').click(function () {
        mostrarListaEventos();
    });

    mostrarListaEventos();

    $('#btnAgregarEvento').click(function () {
        editarEvento('');
    });
});

mostrarListaEventos = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaAdministrar',
        data: {
            tipo: tipoEventoAgenda,
            fecAnio: $('#fecAnio').val()
        },
        success: function (result) {
            $('#listaEventosAgenda').html(result);
        },
        error: function () {
            alert('Hubo un error');
        }
    });
}

editarEvento = function (id) {
    var redirectUrl = controlador + "Editar?tipo=" + tipoEventoAgenda;
    var form = $('<form action="' + redirectUrl + '" method="post">'+
        '<input type="hidden" name="id" value="' + id + '" />');
    $('body').append(form);
    $(form).submit();
}

eliminarEvento = function (id) {
    if (confirm('¿Está seguro de eliminar el evento?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'Eliminar',
            data: {
                id: id
            },
            success: function (result) {
                if (result == 1) {
                    mostrarListaEventos();
                } else {
                    alert('Hubo un error');
                }
            },
            error: function () {
                alert('Hubo un error');
            }
        });
    }
}

downloadBlob = function (id) {
    document.location.href = controlador + "Download?id=" + id;
}