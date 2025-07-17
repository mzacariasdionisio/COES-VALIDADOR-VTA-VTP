var controlador = siteRoot + 'RegistroIntegrante/Revision/'

$(function () {

    $("input[type=text]").keyup(function () {
        $("#" + this.id).val($("#" + this.id).val().toUpperCase());
    });

    $('#btnConsultar').click(function () {
        buscarRevision();
    });

    buscarRevision();
});

ExportarConstancia = function (id) {
    var url = controlador + "ExportarConstanciaPDF/" + id;
    var link = document.createElement("a");
    link.href = url;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
}

ExportarRegistro = function (id) {
    var url = controlador + "ExportarRegistroPDF/" + id;
    var link = document.createElement("a");
    link.href = url;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
}

buscarRevision = function () {
    pintarPaginado();
    mostrarListado(1);
}

nuevaRevision = function (view) {
    window.location = controlador + "nuevo?view=" + view;
}

pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            estado: $('#cbEstado').val(),
            tipemprcodi: $('#cbTipo').val(),
            nombre: $('#txtNombreFiltro').val()
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
            estado: $('#cbEstado').val(),
            tipemprcodi: $('#cbTipo').val(),
            nombre: $('#txtNombreFiltro').val(),
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

verDetalleDJR = function (revicodi) {
    window.location = controlador + "revisionDJR?id=" + revicodi;
}

verDetalleSGI = function (revicodi) {
    window.location = controlador + "revisionSGI?id=" + revicodi;
}


darNotificar = function (emprcodi) {

    mensajeOperacion("Este proceso notificará la respuesta de la revisión vía e-mail al responsable del trámite y a las personas de contacto registrados.", null
        , {
            showCancel: true,
            onOk: function () {
                $.ajax({
                    type: 'POST',
                    url: controlador + "darnotificar",
                    data: {
                        emprcodi: emprcodi
                    },
                    success: function (evt) {
                        mostrarMensaje("Se notificó la Revisión.");
                        buscarRevision();
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




darTerminar = function (emprcodi) {

    mensajeOperacion("Este proceso establece la revisión como pre-aprobado, para proceder a su aprobación por la Dirección Ejecutiva.", null
        , {
            showCancel: true,
            onOk: function () {
                $.ajax({
                    type: 'POST',
                    url: controlador + "darterminar",
                    data: {
                        emprcodi: emprcodi
                    },
                    success: function (evt) {
                        if (evt == -1) {
                            mostrarMensaje("Solo se puede pre-aprobar si SGI y DJR no tienen observaciones.");
                        } else {
                            mostrarMensaje("Se proceso la acción solicitada.");
                        }
                        buscarRevision();
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


darConformidad = function (emprcodi) {

    mensajeOperacion("Este proceso enviará un correo electrónico a cada representante legal con la respuesta del proceso y sus credenciales para la Extranet.", null
        , {
            showCancel: true,
            onOk: function () {
                $.ajax({
                    type: 'POST',
                    url: controlador + "darconformidad",
                    data: {
                        emprcodi: emprcodi
                    },
                    success: function (evt) {
                        mostrarMensaje("Se aprobó la Revisión.");
                        buscarRevision();
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


ReEnviarCredenciales = function (emprcodi) {

    mensajeOperacion("Este proceso reenvía las credenciales a la extranet a cada representante legal.", null
        , {
            showCancel: true,
            onOk: function () {
                $.ajax({
                    type: 'POST',
                    url: controlador + "ReEnviarCredenciales",
                    data: {
                        emprcodi: emprcodi
                    },
                    success: function (evt) {
                        mostrarMensaje("Se proceso la acción solicitada.");
                        buscarRevision();
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


BajarCarta = function (id) {
    var url = controlador + "BajarCarta/" + id;
    var link = document.createElement("a");
    link.href = url;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
}


verFlujo = function (emprcodi) {

    $.ajax({
        type: 'POST',
        data: {
            emprcodi: emprcodi
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
                window.open(controlador + 'verDocumentoFlujo?url=' + DocumentoAdjunto, "_blank", 'fullscreen=yes');
            }

            descargarDocumentoFlujo = function (DocumentoAdjunto, DocumentoAdjuntoFileName) {
                document.location.href = controlador + 'DownloadDocumentoFlujo?url=' + DocumentoAdjunto + '&nombre=' + DocumentoAdjuntoFileName;
            }

            $('#btnAceptar').click(function () {
                $('#popupFlujo').bPopup().close();
            });
        }
    });

}

