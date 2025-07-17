var controlador = siteRoot + 'RegistroIntegrante/Solicitud/'

$(function () {

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    $('#btnConsultar').click(function () {
        buscarEvento();
    });

    buscarEvento();
});


buscarEvento = function () {
    pintarPaginado();
    mostrarListado(1);
}

nuevaSolicitud = function (view) {
    window.location = controlador + "nuevo?view=" + view;
}

pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            soliestado: $('#cbEstadoSolicitud').val()
        },
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "listado",
        data: {
            soliestado: $('#cbEstadoSolicitud').val(),
            nroPagina: nroPagina
        },
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError();
        }
    });
}

pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').html("Ha ocurrido un error.");
}

mostrarMensaje = function (mensaje) {
    alert(mensaje);
}

verDetalle = function (solicodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "verificarTipoSoli",
        data: {
            solicodi: solicodi
        },
        success: function (evt) {
            window.location = controlador + "verdetalle?view=" + evt.view + "&solicodi=" + evt.codisoli;
        },
        error: function () {
            mostrarError();
        }
    });
}

darConformidad = function (solicodi) {

    mensajeOperacion("Este proceso aprueba la solicitud, ¿Esta seguro?", null        
    , {
        showCancel: true,
        onOk: function () {
            $.ajax({
                type: 'POST',
                url: controlador + "darconformidad",
                data: {
                    solicodi: solicodi
                },
                success: function (evt) {
                    mostrarMensaje("Se aprobó la solicitud.");
                    buscarEvento();
                },
                error: function () {
                    mostrarError();
                }
            });
        },
        onCancel: function () {

        }
    });
}

darNotificar = function (solicodi) {    
    mensajeOperacion("Este proceso enviará un correo electrónico a cada representante legal con la respuesta del proceso, ¿Esta seguro?", null
        , {
            showCancel: true,
            onOk: function () {
                $.ajax({
                    type: 'POST',
                    url: controlador + "darnotificar",
                    data: {
                        solicodi: solicodi
                    },
                    success: function (evt) {
                        mostrarMensaje("Se proceso la acción solicitada.");
                        buscarEvento();
                    },
                    error: function () {
                        mostrarError();
                    }
                });
            },
            onCancel: function () {

            }
        });
}


verFlujo = function (emprcodi, solicodi) {

    $.ajax({
        type: 'POST',
        data: {
            emprcodi: emprcodi,
            solicodi: solicodi
        },
        url: controlador + 'verFlujo',
        success: function (evt) {
            $('#contenidoFlujo').html(evt);
            setTimeout(function () {
                $('#popupFlujo').bPopup({
                    autoClose: false,
                });
            }, 50);

            verDocumentoFlujo = function (DocumentoAdjunto) {
                window.open(controlador + 'ver?url=' + DocumentoAdjunto, "_blank", 'fullscreen=yes');
            }

            descargarDocumentoFlujo = function (DocumentoAdjunto, DocumentoAdjuntoFileName) {
                document.location.href = controlador + 'Download?url=' + DocumentoAdjunto + '&nombre=' + DocumentoAdjuntoFileName;
            }

            $('#btnAceptar').click(function () {
                $('#popupFlujo').bPopup().close();
            });
        }
    });

}