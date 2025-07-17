var controlador = siteRoot + 'eventos/informefalla/';

$(function () {

    $('#txtEveninfMemFechEmis').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtEveninfMemFechEmis').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtEveninfMemFechEmis').val(date);
        }
    });


    $('#txtEveninfActFecha').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtEveninfActFecha').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtEveninfActFecha').val(date);
        }
    });


    $('#txtEveninfActElab').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtEveninfActElab').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtEveninfActElab').val(date);
        }
    });


    $('#btnCancelar').click(function () {
        document.location.href = controlador;
    });

    $('#btnCancelar2').click(function () {
        document.location.href = controlador;
    });

    $(document).ready(function () {

        //informe extranet

        $('#cbEveninfActElab').val($('#hfEveninfActElab').val());

        //informe preliminar inicial
        //elaborado por
        $('#cbEveninfPIElab').val($('#hfEveninfPIElab').val());
        //revisado por
        $('#cbEveninfPIRevs').val($('#hfEveninfPIRevs').val());
        //emitido por
        $('#cbEveninfPIEmit').val($('#hfEveninfPIEmit').val());

        //informe preliminar
        //elaborado por
        $('#cbEveninfPElab').val($('#hfEveninfPElab').val());
        //revisado por
        $('#cbEveninfPRevs').val($('#hfEveninfPRevs').val());
        //emitido por
        $('#cbEveninfPEmit').val($('#hfEveninfPEmit').val());

        //informe final
        //elaborado por
        $('#cbEveninfElab').val($('#hfEveninfElab').val());
        //revisado por
        $('#cbEveninfRevs').val($('#hfEveninfRevs').val());
        //emitido por
        $('#cbEveninfEmit').val($('#hfEveninfEmit').val());

        //informe mem        
        //elaborado por
        $('#cbEveninfMemElab').val($('#hfEveninfMemElab').val());
        //revisado por
        $('#cbEveninfMemRevs').val($('#hfEveninfMemRevs').val());
        //emitido por
        $('#cbEveninfMemEmit').val($('#hfEveninfMemEmit').val());

        //extranet osinergmin
        //emitido por
        $('#cbEveninfPIFechEmis').val($('#hfEveninfPIFechEmis').val());


        if ($('#hfEveninfActuacion').val() == 'N') {
            $('#gbEveninfActuacion').hide();
        }

        if ($('#hfEveninfMem').val() == 'N') {
            $('#gbEveninfMem').hide();
        }


        //preliminar inicial        
        if ($('#hfEveninfPIEmitido').val() == 'S') {
            $('#rbEveninfPIEmitidoS').prop('checked', true);
        }
        if ($('#hfEveninfPIEmitido').val() == 'N') {
            $('#rbEveninfPIEmitidoN').prop('checked', true);
        }

        //preliminar
        if ($('#hfEveninfPEmitido').val() == 'S') {
            $('#rbEveninfPEmitidoS').prop('checked', true);
        }
        if ($('#hfEveninfPEmitido').val() == 'N') {
            $('#rbEveninfPEmitidoN').prop('checked', true);
        }

        //final
        if ($('#hfEveninfEmitido').val() == 'S') {
            $('#rbEveninfEmitidoS').prop('checked', true);
        }
        if ($('#hfEveninfEmitido').val() == 'N') {
            $('#rbEveninfEmitidoN').prop('checked', true);
        }

        //mem
        if ($('#hfEveninfMemEmitido').val() == 'S') {
            $('#rbEveninfMemEmitidoS').prop('checked', true);
        }
        if ($('#hfEveninfMemEmitido').val() == 'N') {
            $('#rbEveninfMemEmitidoN').prop('checked', true);
        }

        //extranet osinergmin
        if ($('#hfEveninfActLlamado').val() == 'S') {
            $('#rbEveninfActLlamadoS').prop('checked', true);
        }
        if ($('#hfEveninfActLlamado').val() == 'N') {
            $('#rbEveninfActLlamadoN').prop('checked', true);
        }


        //informe mem
        if ($('#hfEveninfMem').val() == 'S') {
            $('#rbEveninfMemS').prop('checked', true);
        }
        if ($('#hfEveninfMem').val() == 'N') {
            $('#rbEveninfMemN').prop('checked', true);
        }


        if ($('#hfEveninfActuacion').val() == 'S') {
            $('#rbEveninfActuacionS').prop('checked', true);
        }
        if ($('#hfEveninfActuacion').val() == 'N') {
            $('#rbEveninfActuacionN').prop('checked', true);
        }

        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
            $('#btnEnviarIPI').hide();
            $('#btnEnviarIP').hide();
            $('#btnEnviarIF').hide();

        }

    });

    $('#btnGrabar').click(function () {
        grabar();
    });

    $('#btnGrabar2').click(function () {
        grabar();
    });


    $('#btnEnviarIPI').click(function () {
        enviarIPI();
    });

    $('#btnEnviarIP').click(function () {
        enviarIP();
    });

    $('#btnEnviarIF').click(function () {
        enviarIF();
    });

});

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

copiarValorComboACampoOculto = function (id) {

    $('#hf' + id).val($('#cb' + id).val());

}

validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    //extranet osinergmin
    if ($('#rbEveninfActLlamadoS').is(':checked')) {
        $('#hfEveninfActLlamado').val('S');
    }

    if ($('#rbEveninfActLlamadoN').is(':checked')) {
        $('#hfEveninfActLlamado').val('N');
    }

    //informe preliminar inicial
    if ($('#rbEveninfPIEmitidoS').is(':checked')) {
        $('#hfEveninfPIEmitido').val('S');
    }

    if ($('#rbEveninfPIEmitidoN').is(':checked')) {
        $('#hfEveninfPIEmitido').val('N');
    }


    //informe preliminar
    if ($('#rbEveninfPEmitidoS').is(':checked')) {
        $('#hfEveninfPEmitido').val('S');
    }

    if ($('#rbEveninfPEmitidoN').is(':checked')) {
        $('#hfEveninfPEmitido').val('N');
    }

    //informe final
    if ($('#rbEveninfEmitidoS').is(':checked')) {
        $('#hfEveninfEmitido').val('S');
    }

    if ($('#rbEveninfEmitidoN').is(':checked')) {
        $('#hfEveninfEmitido').val('N');
    }

    //informe mem
    if ($('#rbEveninfMemEmitidoS').is(':checked')) {
        $('#hfEveninfMemEmitido').val('S');
    }

    if ($('#rbEveninfMemEmitidoN').is(':checked')) {
        $('#hfEveninfMemEmitido').val('N');
    }

    //es informe mem?
    if ($('#rbEveninfMemS').is(':checked')) {
        $('#hfEveninfMem').val('S');
    }

    if ($('#rbEveninfMemN').is(':checked')) {
        $('#hfEveninfMem').val('N');
    }

    var valorAnio = $('#txtEvenAnio').val();
    if (valorAnio != "") {

        if (!validarNumero(valorAnio)) {
            mensaje = mensaje + "<li>Año no es numérico</li>";
            flag = false;
        }

    }

    var valorEvenCorrSco = $('#txtEvenCorrSco').val();
    if (valorEvenCorrSco != "") {

        if (!validarNumero(valorEvenCorrSco)) {
            mensaje = mensaje + "<li>Correlativo COES no es numérico</li>";
            flag = false;
        }
    }

    var valorEvenCorrmem = $('#txtEvenCorrmem').val();
    if (valorEvenCorrmem != "") {
        if (!validarNumero(valorEvenCorrmem)) {
            mensaje = mensaje + "<li>Correlativo MEM no es numérico</li>";
            flag = false;
        }
    }

    var valorEvenCorr = $('#txtEvenCorr').val();
    if (valorEvenCorr != "") {
        if (!validarNumero(valorEvenCorr)) {
            mensaje = mensaje + "<li>Correlativo COES (anterior) no es numérico</li>";
            flag = false;
        }
    }

    //extranet osingermin
    copiarValorComboACampoOculto("EveninfActElab");

    //preliminar inicial
    copiarValorComboACampoOculto("EveninfPIElab");
    copiarValorComboACampoOculto("EveninfPIRevs");
    copiarValorComboACampoOculto("EveninfPIEmit");

    //preliminar
    copiarValorComboACampoOculto("EveninfPElab");
    copiarValorComboACampoOculto("EveninfPRevs");
    copiarValorComboACampoOculto("EveninfPEmit");

    //final
    copiarValorComboACampoOculto("EveninfElab");
    copiarValorComboACampoOculto("EveninfRevs");
    copiarValorComboACampoOculto("EveninfEmit");

    //mem
    copiarValorComboACampoOculto("EveninfMemElab");
    copiarValorComboACampoOculto("EveninfMemRevs");
    copiarValorComboACampoOculto("EveninfMemEmit");

    //completar fecha
    completarFecha("txtEveninfActFecha");
    completarFecha("txtEveninfPIFechEmis");
    completarFecha("txtEveninfPFechEmis");
    completarFecha("txtEveninfFechEmis");
    completarFecha("txtEveninfMemFechEmis");


    if (flag) mensaje = "";
    return mensaje;
}


grabar = function () {
    var mensaje = validarRegistro();

    if (mensaje == "") {
        controlador = siteRoot + 'eventos/informefalla/';
        $.ajax({
            type: 'POST',
            url: controlador + "grabar",
            dataType: 'json',
            data: $('#form').serialize(),
            success: function (result) {
                if (result != "-1") {
                    mostrarExito();
                    $('#hfEveninfCodi').val(result);
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


enviarIPI = function () {
    var id = $('#hfEveninfCodi').val();
    enviarInformeFalla(id, 1, 0);
}


enviarIP = function () {
    var id = $('#hfEveninfCodi').val();
    enviarInformeFalla(id, 1, 1);
}


enviarIF = function () {
    var id = $('#hfEveninfCodi').val();
    enviarInformeFalla(id, 1, 2);
}


//informe preliminar inicial, informe preliminar, informe final
enviarInformeFalla = function (id, enviar, tipoInforme) {

    var idFormato = id + "," + enviar + "," + tipoInforme;


    var controlador2 = siteRoot + 'eventos/enviarcorreos/';


    $.ajax({
        type: 'POST',
        url: controlador2 + "formatocorreo",
        data: {
            id: idFormato,
            plantilla: 'informefallan1'
        },
        success: function (result) {

            $('#contenidoEdicion').html(result);
            setTimeout(function () {
                $('#popupEdicion').bPopup({
                    autoClose: false
                });

            },
                50);


            //cargaDeArchivo();

            $.ajax({
                success: function () {
                    cargaDeArchivo();
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
                                        url: controlador2 + "estadocorreoenviadoinformefalla",
                                        dataType: 'json',
                                        cache: false,
                                        data: {
                                            id: $('#hfEveninfCodi').val(),
                                            tipo: tipoInforme
                                        },
                                        success: function (result) {
                                            if (result >= 1) {
                                                $('#popupEdicion').bPopup().close();

                                                //ver ventana buscar()
                                                document.location.href = siteRoot + 'eventos/informefalla/';


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

                                    document.location.href = siteRoot + 'eventos/informefalla/';

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
