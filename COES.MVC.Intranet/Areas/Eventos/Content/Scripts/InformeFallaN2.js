var controlador = siteRoot + 'eventos/informefallan2/';

$(function () {

    //$("#cbEmitido").val("N");
    var fecha = new Date();
    var anio = fecha.getFullYear();
    var fechaInicialAnio = "01/01/" + anio;

    //$('#FechaDesde').val(fechaInicialAnio);


    $('#FechaDesde').Zebra_DatePicker({

    });

    $('#FechaHasta').Zebra_DatePicker({

    });

    //buscar();
    $("#btnBuscar").persisteBusqueda("#frmBusqueda", buscar);

    $('#btnBuscar').click(function () {
        buscar();
    });


    $('#btnDescargarLog').click(function () {
        exportarLog();
    });

});

function convertirFecha(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

buscar = function () {
    var fechaini = convertirFecha($('#FechaDesde').val());
    var fechafin = convertirFecha($('#FechaHasta').val());

    if (fechaini <= fechafin) {
        var infEmitido = $("#cbEmitido").val();
        controlador = siteRoot + 'eventos/informefallan2/';
        pintarPaginado(infEmitido);
        mostrarListado(infEmitido, 1);
    } else {
        alert("Fecha inicial supera a la final");
    }
}

pintarPaginado = function (infEmitido, nroPagina) {

    $.ajax({
        type: 'POST',
        url: controlador + "paginado",
        data: {
            infEmitido: infEmitido,
            emprCodi: $('#cbEmpresa').val(),
            equiAbrev: $('#EquiAbrev').val(),
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val(),
            nroPage: nroPagina
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
    var infEmitido = $("#cbEmitido").val();
    mostrarListado(infEmitido, nroPagina);
}


mostrarListado = function (infEmitido, nroPagina) {

    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "lista",
        data: {
            infEmitido: infEmitido,
            emprCodi: $('#cbEmpresa').val(),
            equiAbrev: $('#EquiAbrev').val(),
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


editar = function (id, accion) {
    controlador = siteRoot + 'eventos/informefallan2/';
    document.location.href = controlador + "editar?id=" + id + "&accion=" + accion;
}

eliminar = function (id) {
    controlador = siteRoot + 'eventos/informefallan2/';
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


mostrarError = function () {
    alert('Ha ocurrido un error.');
}


enviar = function (id, enviar, tipoInforme) {
    enviarInformeFallaN2(id, enviar, tipoInforme);
}


iniciarControlTexto = function (id) {

    var result = $('#' + id).val();
    tinymce.remove();

    tinymce.init({
        selector: '#' + id,
        plugins: [
            'paste textcolor colorpicker textpattern'
        ],
        toolbar1:
            'insertfile undo redo | bold italic | alignleft aligncenter alignright alignjustify |  bullist numlist outdent indent| forecolor backcolor',
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


verArchivos = function (id) {

    document.location.href = siteRoot + 'eventos/enviarcorreos/' + "vercarpeta?id=" + id + "&tipoplantilla=" + "informefallan2";

}


//informe preliminar inicial, informe final
enviarInformeFallaN2 = function (id, enviar, tipoInforme) {

    var idFormato = id + "," + enviar + "," + tipoInforme;
    var controladorifn2 = siteRoot + 'eventos/enviarcorreos/';

    $.ajax({
        type: 'POST',
        url: controladorifn2 + "formatocorreo",
        data: {
            id: idFormato,
            plantilla: 'informefallan2'
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
                        url: controladorifn2 + "/enviarCorreo",
                        dataType: 'json',
                        data: $('#formFormatoCorreo').serialize(),
                        success: function (result) {
                            if (result >= 1) {

                                if (enviar == 1) {
                                    //estado a enviado
                                    $.ajax({
                                        type: 'POST',
                                        url: controladorifn2 + "estadocorreoenviadoinformefallan2",
                                        dataType: 'json',
                                        cache: false,
                                        data: {
                                            id: $('#hfEvenInfN2Codi').val(),
                                            tipo: tipoInforme
                                        },
                                        success: function (result) {
                                            if (result >= 1) {
                                                $('#popupEdicion').bPopup().close();

                                                //ver ventana buscar()
                                                document.location.href = siteRoot + 'eventos/informefallan2/';

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

                                    document.location.href = siteRoot + 'eventos/informefallan2/';

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


    controlador = siteRoot + 'eventos/informefallan2/';

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



verPopupListaEnviosEventos = function (idEvento) {
    $.ajax({
        type: "POST",
        url: siteRoot + "eventos/informefallan2/listaEnviosEventos",
        data: { idEvento: idEvento },
        success: function (evt) {
            $('#popupListadoEnviosEventos').css("width", "700px")
            $('#popupListadoEnviosEventos').html(evt);
            pintarPaginadoEnvios(idEvento);
            setTimeout(function () {
                $('#popupListadoEnviosEventos').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });
            }, 1);
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

pintarPaginadoEnvios = function (idEvento) {
    $.ajax({
        type: "POST",
        url: siteRoot + "eventos/informefallan2/paginadoEnvios",
        data: { idEvento: idEvento },
        success: function (evt) {
            $('#paginadoEnvios').html(evt);


            mostrarPaginado();
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });

}



function exportarLog() {

    $.ajax({
        type: 'POST',
        url: controlador + "ExportarLog",
        data: {
            fechaIni: $('#FechaDesde').val(),
            fechaFin: $('#FechaHasta').val()
        },
        success: function (evt) {
            descargarLog(evt, 1);
        },
        error: function () {
            //mostrarError('mensaje');
        }
    });
}

function descargarLog(nombrearchivo, tipo) {
    window.location = controlador + "Descargar?nombreArchivo=" + nombrearchivo + "|" + tipo;
}

