var controlador = siteRoot + 'eventos/enviarcorreos/';

$(function () {

    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#btnNuevo').click(function () {
        editar(0, 1, $('#cbSubcausa').val());
    });

    //buscar();
    $("#btnBuscar").persisteBusqueda(".content-tabla-search", buscar);

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

});


exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarCorreo",
        dataType: 'json',
        cache: false,
        data: {
            subCausacodi: $('#cbSubcausa').val(),
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val()

        },
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarCorreo";
            }
            else {
                mostrarError();
                alert("NILTON NO");
            }
        },
        error: function () {
            mostrarError();
            ALERT("SI");
        }
    });
}

buscar = function () {
    pintarPaginado();
    mostrarListado(1);
}

pintarPaginado = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            subCausacodi: $('#cbSubcausa').val(),
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val()
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


pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}


mostrarListado = function (nroPagina) {

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            subCausacodi: $('#cbSubcausa').val(),
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val(),
            nroPage: nroPagina
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

mostrarError = function () {
    alert('Ha ocurrido un error.');
}

iniciarControlTexto = function (id) {

    var result = $('#' + id).val();
    tinymce.remove();

    tinymce.init({
        selector: '#' + id,
        plugins: [
            'paste textcolor colorpicker textpattern link preview'
        ],
        convert_urls: false,
        toolbar1:
            'insertfile undo redo | bold italic | alignleft aligncenter alignright alignjustify |  bullist numlist outdent indent| forecolor backcolor link preview',
        menubar: false,
        language: 'es',
        statusbar: false,
        setup: function (editor) {
            editor.on('change',
                function () {
                    editor.save();
                });
        }
    });

}


eliminar = function (id) {

    if (confirm('¿Está seguro de eliminar este registro?')) {
        $.ajax({
            type: 'POST',
            url: controlador + "eliminar",
            dataType: 'json',
            cache: false,
            data: { id: id },
            success: function (resultado) {
                if (resultado == 1) {
                    buscar();
                } else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });


    }
}


editarRegistro = function (id) {
    $.ajax({
        type: 'POST',
        data: { iccodi: id },
        url: controlador + 'editar',
        success: function (evt) {

            $('#contenidoEdicion').html(evt);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });

            }, 50);
            $('#btnGrabar').click(function () {
                grabarRegistro();
            });

            $('#btnCancelar').click(function () {
                $('#popupEdicion').bPopup().close();
            });


        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


grabarRegistro = function () {
    return;
}


editar = function (id, accion, top) {
    document.location.href = controlador + "editar?id=" + id + "&accion=" + accion + "&top=" + top;
}

validarFormatoCorreo = function () {
    var mensaje = "";

    var validaFrom = validarCorreo($('#From').val(), 1, 1);
    if (validaFrom < 0) mensaje = mensaje + " " + "Revisar campo From.\n";

    var validaTo = validarCorreo($('#To').val(), 1, 1);
    if (validaTo < 0) mensaje = mensaje + " " + "Revisar campo To.\n";

    var validaCc = validarCorreo($('#CC').val(), 0, -1);
    if (validaCc < 0) mensaje = mensaje + " " + "Revisar campo CC.\n";

    var validaBcc = validarCorreo($('#BCC').val(), 0, -1);
    if (validaBcc < 0) mensaje = mensaje + " " + "Revisar campo BCC.\n";

    var asunto = $('#Asunto').val().trim() + "";

    if (asunto == "")
        mensaje = mensaje + " " + "Revisar asunto del correo.\n";

    var contenido = $('#Contenido').val().trim() + "";
    var spans = contenido.indexOf('">');
    var spans2 = contenido.indexOf('</');

    if (spans > 0 && spans2 > 0) {
        if ((spans2 - spans) < 10) {
            mensaje = mensaje + " " + "Revisar contenido del correo.\n";
        }
    }


    if (mensaje == "") {
        return 1;
    } else {
        alert(mensaje);
        return -1;
    }

}

validarCorreo = function (cadena, minimo, maximo) {
    var arreglo = cadena.split(';');
    var nroCorreo = 0;
    var longitud = arreglo.length;

    if (longitud == 0) {
        arreglo = cadena;
        longitud = 1;
    }

    for (var i = 0; i < longitud; i++) {

        var palabra = arreglo[i].trim();
        var validacion = validarDirecccionCorreo(palabra);

        if (validacion)
            nroCorreo++;

    }

    if (minimo > nroCorreo)
        return -1;

    if (maximo > 0 && nroCorreo > maximo)
        return -1;

    return 1;
}

function validarDirecccionCorreo(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

enviar = function (id, accion) {

    var controlador2 = siteRoot + 'eventos/enviarcorreos/';

    $.ajax({
        type: 'POST',
        url: controlador2 + "formatocorreo",
        data: {
            id: id,
            plantilla: 'enviarcorreo'
        },
        success: function (result) {
            $('#contenidoEdicion').html(result);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });
            },
                50);

            cargaDeArchivo();


            $.ajax({
                success: function () {
                    iniciarControlTexto('Contenido');
                }
            });

            $('#btnEnviar').click(function () {

                var $boton = $(this);

                // Evita que el botón sea clickeado nuevamente
                if ($boton.prop('disabled')) {
                    return;
                }

                $boton.prop('disabled', true);

                setTimeout(function () {

                    var correoValido = validarFormatoCorreo();
                    if (correoValido < 0)
                        return;

                    $.ajax({
                        type: 'POST',
                        url: controlador2 + "/enviarCorreo",
                        dataType: 'json',
                        data: $('#formFormatoCorreo').serialize(),
                        success: function (result) {
                            if (result >= 1) {

                                //estado a enviado
                                if (accion == 1) {
                                    $.ajax({
                                        type: 'POST',
                                        url: controlador2 + "estadoCorreoEnviado",
                                        dataType: 'json',
                                        cache: false,
                                        data: {
                                            mailcodi: id
                                        },
                                        success: function (result) {
                                            if (result >= 1) {
                                                $('#popupEdicion').bPopup().close();

                                                //ver ventana buscar()
                                                document.location.href = siteRoot + 'eventos/enviarcorreos/';


                                            }
                                            if (result == -1) {
                                                mostrarError();
                                            }
                                        },
                                        error: function () {
                                            mostrarError();
                                        }
                                    });

                                } else {
                                    $('#popupEdicion').bPopup().close();
                                    //ver ventana buscar()
                                    document.location.href = siteRoot + 'eventos/enviarcorreos/';
                                }
                            }
                            if (result == -1) {
                                mostrarError();
                            }
                        },
                        error: function () {
                            mostrarError();
                        }
                    });

                    $boton.prop('disabled', false);

                }, 1000);

            });
        },
        error: function () {

        }
    });
}













