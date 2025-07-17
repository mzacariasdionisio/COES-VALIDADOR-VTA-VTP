var controlador = siteRoot + 'eventos/informefallan2/';

$(function () {

    $('#btnCancelar').click(function () {
        document.location.href = controlador;
    });

    $('#btnCancelar2').click(function () {
        document.location.href = controlador;
    });

    $(document).ready(function () {

        if ($('#hfEvenIPIEN2Emitido').val() == 'S') { $('#rbEvenIPIEN2EmitidoS').prop('checked', true); }
        if ($('#hfEvenIPIEN2Emitido').val() == 'N') { $('#rbEvenIPIEN2EmitidoN').prop('checked', true); }
        if ($('#hfEvenInfPIN2Emitido').val() == 'S') { $('#rbEvenInfPIN2EmitidoS').prop('checked', true); }
        if ($('#hfEvenInfPIN2Emitido').val() == 'N') { $('#rbEvenInfPIN2EmitidoN').prop('checked', true); }
        if ($('#hfEvenIFEN2Emitido').val() == 'S') { $('#rbEvenIFEN2EmitidoS').prop('checked', true); }
        if ($('#hfEvenIFEN2Emitido').val() == 'N') { $('#rbEvenIFEN2EmitidoN').prop('checked', true); }
        if ($('#hfEvenInfFN2Emitido').val() == 'S') { $('#rbEvenInfFN2EmitidoS').prop('checked', true); }
        if ($('#hfEvenInfFN2Emitido').val() == 'N') { $('#rbEvenInfFN2EmitidoN').prop('checked', true); }

        //informe preliminar inicial - Sin informe de empresa
        $('#cbEvenIPIEN2Elab').val($('#hfEvenIPIEN2Elab').val());

        //informe preliminar inicial - Informe
        $('#cbEvenInfPIN2Elab').val($('#hfEvenInfPIN2Elab').val());

        //informe final - Sin informe de empresa
        $('#cbEvenIFEN2Elab').val($('#hfEvenIFEN2Elab').val());

        //informe final - Informe
        $('#cbEvenInfFN2Elab').val($('#hfEvenInfFN2Elab').val());

        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
            $('#btnEnviarIPI').hide();
            $('#btnEnviarIF').hide();
        }
    });

    $('#btnGrabar').click(function () {
        grabar(-1);
    });

    $('#btnGrabar2').click(function () {
        grabar(-1);
    });


    $('#btnEnviarIPISI').click(function () {
        //enviarIPISI();
        grabar(0);
    });

    $('#btnEnviarIPI').click(function () {
        //enviarIPI();
        grabar(1);
    });

    $('#btnEnviarIFSI').click(function () {
        //enviarIFSI();
        grabar(2);
    });

    $('#btnEnviarIF').click(function () {
        //enviarIF();
        grabar(3);
    });

});


mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').html('La operación se realizó con éxito.');
}


completarFecha = function (id) {

    var valor = $('#' + id).val() + " ";

    if (valor.trim() == "")
        return;

    var date = valor.split(' ')[0].split('/');
    var time = valor.split(' ')[1].split(':');
    var date0 = (validarNumero(date[0])) ? date[0] : "00";
    var date1 = (validarNumero(date[1])) ? date[1] : "00";
    var date2 = (validarNumero(date[2])) ? date[2] : "00";
    var time0 = (validarNumero(time[0])) ? time[0] : "00";
    var time1 = (validarNumero(time[1])) ? time[1] : "00";
    var time2 = (validarNumero(time[2])) ? time[2] : "00";

    $('#' + id).val(date0 + "/" + date1 + "/" + date2 + " " + time0 + ":" + time1 + ":" + time2);
}


validarNumero = function (valor) {
    return !isNaN(parseFloat(valor)) && isFinite(valor);
}


copiarValorComboACampoOculto = function (id) {
    $('#hf' + id).val($('#cb' + id).val());
}


validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    //informe preliminar inicial
    //sin informe empresa
    if ($('#rbEvenIPIEN2EmitidoS').is(':checked')) {
        $('#hfEvenIPIEN2Emitido').val('S');
    }

    if ($('#rbEvenIPIEN2EmitidoN').is(':checked')) {
        $('#hfEvenIPIEN2Emitido').val('N');
    }
    //informe
    if ($('#rbEvenInfPIN2EmitidoS').is(':checked')) {
        $('#hfEvenInfPIN2Emitido').val('S');
    }
    if ($('#rbEvenInfPIN2EmitidoN').is(':checked')) {
        $('#hfEvenInfPIN2Emitido').val('N');
    }

    //informe final
    //sin informe empresa
    if ($('#rbEvenIFEN2EmitidoS').is(':checked')) {
        $('#hfEvenIFEN2Emitido').val('S');
    }
    if ($('#rbEvenIFEN2EmitidoN').is(':checked')) {
        $('#hfEvenIFEN2Emitido').val('N');
    }
    //informe
    if ($('#rbEvenInfFN2EmitidoS').is(':checked')) {
        $('#hfEvenInfFN2Emitido').val('S');
    }

    if ($('#rbEvenInfFN2EmitidoN').is(':checked')) {
        $('#hfEvenInfFN2Emitido').val('N');
    }

    var valorAnio = $('#txtEvenAnio').val();

    if (valorAnio != "") {
        if (!validarNumero(valorAnio)) {
            mensaje = mensaje + "<li>Año no es numérico</li>";
            flag = false;
        }
    }

    var valorEvenN2Corr = $('#txtEvenN2Corr').val();
    if (valorEvenN2Corr != "") {
        if (!validarNumero(valorEvenN2Corr)) {
            mensaje = mensaje + "<li>Correlativo COES no es numérico</li>";
            flag = false;
        }
    }

    copiarValorComboACampoOculto("EvenIPIEN2Elab");
    copiarValorComboACampoOculto("EvenInfPIN2Elab");
    copiarValorComboACampoOculto("EvenIFEN2Elab");
    copiarValorComboACampoOculto("EvenInfFN2Elab");
    completarFecha("txtEvenIPIEN2FechEm");
    completarFecha("txtEveninfPFechEmis");
    completarFecha("txtEvenIFEN2FechEm");
    completarFecha("txtEvenInfFN2FechEmis");

    if (flag) mensaje = "";
    return mensaje;
}


grabar = function (indicador) {
    var mensaje = validarRegistro();

    if (mensaje == "") {
        controlador = siteRoot + 'eventos/informefallan2/';
        $.ajax({
            type: 'POST',
            url: controlador + "grabar",
            dataType: 'json',
            data: $('#form').serialize(),
            success: function (result) {
                if (result != "-1") {

                    mostrarExito();
                    $('#hfEvenInfN2Codi').val(result);

                    if (indicador == 0) {
                        enviarIPISI();
                    }
                    else if (indicador == 1) {
                        enviarIPI();
                    }
                    else if (indicador == 2) {
                        enviarIFSI();
                    }
                    else if (indicador == 3) {
                        enviarIF();
                    }

                } else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    } else {
        mostrarAlerta(mensaje);
    }
}


mostrarError = function () {
    alert("Ha ocurrido un error");
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


// Indicador = 0;
enviarIPISI = function () {
    var id = $('#hfEvenInfN2Codi').val();
    enviarInformeFallaN2(id, 1, 0);
}

// Indicador = 1
enviarIPI = function () {
    var id = $('#hfEvenInfN2Codi').val();
    enviarInformeFallaN2(id, 1, 1);
}

// Indicador = 2
enviarIFSI = function () {
    var id = $('#hfEvenInfN2Codi').val();
    enviarInformeFallaN2(id, 1, 2);
}

// Indicador = 3
enviarIF = function () {
    var id = $('#hfEvenInfN2Codi').val();
    enviarInformeFallaN2(id, 1, 3);
}


//informe preliminar inicial, informe final
enviarInformeFallaN2 = function (id, enviar, tipoInforme) {

    var idFormato = id + "," + enviar + "," + tipoInforme;
    var controlador2 = siteRoot + 'eventos/enviarcorreos/';

    $.ajax({
        type: 'POST',
        url: controlador2 + "formatocorreo",
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
                        url: controlador2 + "/enviarCorreo",
                        dataType: 'json',
                        data: $('#formFormatoCorreo').serialize(),
                        success: function (result) {
                            if (result >= 1) {

                                if (enviar == 1) {

                                    //estado a enviado
                                    $.ajax({
                                        type: 'POST',
                                        url: controlador2 + "estadocorreoenviadoinformefallan2",
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





