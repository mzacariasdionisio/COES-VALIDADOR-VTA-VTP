controlador = siteRoot + 'calculoresarcimiento/configuracion/';

$(function () {

    $('#btnNuevo').on('click', function () {
        editar(0);
    });

    $('#btnRegresar').on('click', function () {
        document.location.href = controlador + 'tipointerrupcion';
    });
    consultar();
});


function consultar(anio) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ListaCausaInterrupcion',
        data: {
            idTipo: $('#hfIdTipoInterrupcion').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
            $('#tablaTipoInterrupcion').dataTable({
                "iDisplayLength": 25
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function editar(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'editarCausaInterrupcion',
        data: {
            id: id,
            idTipo: $('#hfIdTipoInterrupcion').val()
        },
        global: false,
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false,
                    modalClose: false,
                    escClose: false,
                    follow: [false, false]
                });
            }, 50);

            $('#btnGrabar').on("click", function () {
                grabar();
            });

            $('#btnCancelar').on("click", function () {
                $('#popupEdicion').bPopup().close();
            });
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function eliminar(id) {
    if (confirm('¿Está seguro de realizar esta operación?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarCausainterrupcion',
            data: {
                id: id
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'La operación se realizó correctamente.');
                    consultar();
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function verCausas(id) {
    document.location.href = controlador + 'causainterrupcion?id=' + id;
}

function grabar() {
    if ($('#txtCausaInterrupcion').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + 'grabarCausaInterrupcion',
            data: {
                id: $('#hfIdCausaInterrupcion').val(),
                nombre: $('#txtCausaInterrupcion').val(),
                estado: $('#cbEstado').val(),
                idTipo: $('#hfIdTipoInterrupcion').val()
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos de la causa de interrrupcíón se grabaron correctamente.');
                    $('#popupEdicion').bPopup().close();
                    consultar();
                }
                else {
                    mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensajeEdicion', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensajeEdicion', 'alert', 'Por favor ingrese el nombre de la causa de interrupción.');
    }
}