var controlador = siteRoot + 'DirectorioImpugnaciones/AdminImpugnacion/';

$(function () {
    listaAdministrar();

    $('#btnAgregarDocumento').click(function () {
        var redirectUrl = controlador + "Editar?tipo=" + tipoImpugnacion;
        var form = $('<form action="' + redirectUrl + '" method="post">' +
            '<input type="hidden" name="fecAnio" value="' + $('#fecAnio').val() + '" />' +
            '<input type="hidden" name="fecMes" value="' + $('#fecMes').val() + '" />' +
            '</form>');
        $('body').append(form);
        $(form).submit();
    });

    $('#btnBuscar').click(function () {
        listaAdministrar();
    });
});

listaAdministrar = function () {   
  
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaAdministrar',
        data: {
            tipoImpugnacion: tipoImpugnacion,
            fecAnio: $('#fecAnio').val(),
            fecMes: $('#fecMes').val()
        },
        success: function (evt) {
            $('#listaImpugnaciones').html(evt);
        },
        error: function (evt) {
            alert('error');
        }
    });
}

eliminarDocumento = function (id) {
    if (confirm('¿Está seguro de eliminar este registro?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'Eliminar',
            data: {
                codigo: id
            },
            success: function () {
                listaAdministrar();
            },
            error: function () {
                alert("Hubo un error");
            }
        });
    }
}

editarDocumento = function (id) {
    var urlDestino = controlador + 'Editar?tipo=' + tipoImpugnacion;
    myRedirect(urlDestino, id);
}

mostrarAgregarDocumento = function () {
    $('#popupEdicion').bPopup({
        autoClose: false
    });
}

myRedirect = function (redirectUrl, id) {
    var form = $('<form action="' + redirectUrl + '" method="post">' +
    '<input type="hidden" name="id" value="' + id + '" />' +
    '</form>');
    $('body').append(form);
    $(form).submit();
};

downloadBlob = function (url) {
    document.location.href = controlador + "Download?id=" + url;
}