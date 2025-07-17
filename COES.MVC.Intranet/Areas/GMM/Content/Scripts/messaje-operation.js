$(document).ready(function () {
    $('#btnAceptarMessages').click(function () {
        $('#MessagesClose').bPopup().close();
        var x = parseInt($('#MessagesClose #btnAceptarMessages').attr('data-option'));
        if (x == 1) {
            location.reload();
        }
        if (x == 2) {
            window.location.href = siteRoot;
        }
    });
});
function mensajeOperacion(resultado, x, options) {
    $('#MessagesClose .content-messajes-text').text(resultado);

    $('#MessagesClose').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#btnAceptarMessages').attr('data-option', x);

    if (!!options && !!options.onOk) {
        $('#btnAceptarMessages').unbind("click");
        $('#btnAceptarMessages').click(function () {
            options.onOk();

            $('#btnCancelarMessages').hide();
            $('#btnAceptarMessages').unbind("click");
            $('#btnAceptarMessages').click(function () {
                $('#MessagesClose').bPopup().close();
                var x = parseInt($('#MessagesClose #btnAceptarMessages').attr('data-option'));
                if (x == 1) {
                    location.reload();
                }
            });
        });
    }

    if (options && options.showCancel) {
        $('#btnCancelarMessages').show();

        $('#btnCancelarMessages').click(function () {
            if (!!options && !!options.onCancel) {
                options.onCancel()
            }

            $('#MessagesClose').bPopup().close();
        });
    }
}

function mensajesError(resultado) {
    $(".mensaje-content-error").html(resultado);
    $(".mensaje-content-error").fadeOut(3000).fadeIn(2000).fadeOut(3000).fadeIn(2000).fadeOut(500);
}

function AsigdataTable() {
    $('#divGeneral > .tabla-formulario').dataTable({
        "iDisplayLength": 25
    });
}
function validaErrorOperation(status) {
    if (status == 0 || status == 403) {
        $('#MessagesClose .content-messajes-text').text("Session Caducada, Actualice nuevamente la pagina e Inicie sesion...");
        $('#MessagesClose').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false,
            escClose: false
        });
        $('#btnAceptarMessages').attr('data-option', 2);
    }
}