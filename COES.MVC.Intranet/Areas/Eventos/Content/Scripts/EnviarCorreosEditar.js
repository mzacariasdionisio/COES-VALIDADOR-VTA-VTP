var controlador = siteRoot + 'eventos/enviarcorreos/';


$(function () {       

    $('#txtFechaIni').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtFechaIni').val(date);
            cargarReprogramas();
        }
    });

    $('#txtHora').mask("99:99:99");

    $('#btnCancelar').click(function () {
        document.location.href = siteRoot + 'eventos/enviarcorreos/';
    });

    $('#btnCancelar2').click(function () {
        document.location.href = siteRoot + 'eventos/enviarcorreos/';
    });
    
    $('#cbTipoEnvio').change(function () {        
        cargarTipoEnvio($('#cbTipoEnvio').val());
    });
    
    setTextoInvisible();
    
    $(document).ready(function () {
                
        $('#cbTipoEnvio').val($('#hfSubcausacodi').val());

        cargarTipoEnvio($('#cbTipoEnvio').val());

        $('#cbxProg').val($('#hfPernomb').val());
        $('#cbxEsp').val($('#hfPernombEspec').val());
        $('#cbxReProgMcp').val($('#hfReprograma').val());
        $('#cbxEspSME').val($('#hfPernombEspecSME').val());
        
        if ($('#hfMailCheck1').val() == "S") { $('#cbxMailCheck1').prop('checked', true); }
        
        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
        }
    });
    
    $('#btnGrabar').click(function () {
        grabar();
    });

    $('#btnGrabar2').click(function () {
        grabar();
    });

    $('#btnSeleccionarEquipo').click(function () {
        
        openBusquedaEquipo();
    });

    cargarBusquedaEquipo();
    //cargarReprogramas();
});

cargarReprogramas = function () {
    var controlador2 = siteRoot + 'eventos/enviarcorreos/';

    $.ajax({
        type: 'GET',
        url: controlador2 + 'CargarReprogramas',
        dataType: 'json',
        data: { fecha: $('#txtFechaIni').val()},
        cache: false,
        success: function (aData) {
            $('#cbxReProgMcp').get(0).options.length = 0;
            $('#cbxReProgMcp').get(0).options[0] = new Option("", "0");
            $.each(aData, function (i, item) {
                $('#cbxReProgMcp').get(0).options[$('#cbxReProgMcp').get(0).options.length] = new Option(item.Text, item.Value);
            });
        //    $('#hfReprograma').val($('#cbxReProgMcp').val());
        },
        error: function (er) {
            mostrarError();
        }
    });
};


openBusquedaEquipo = function() {
    $('#busquedaEquipo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#txtFiltro').focus();
}


cargarBusquedaEquipo = function() {
    $.ajax({
        type: "POST",
        url: siteRoot + "eventos/busqueda/index",
        data: {},
        success: function(evt) {
            $('#busquedaEquipo').html(evt);

        },
        error: function(req, status, error) {
            mostrarError();
        }
    });
}

cargarTipoEnvio = function (tipo) {    

    $('#trHora').hide();
    $('#trConsecuencia').hide();
    $('#trProgramador').show();
    $('#trCoordinador').show();
    setTexto('#lblFecha', 'Fecha');
    $('#trEspecialistaSME').hide();
    setTextoInvisible();

    var tipo1 = parseInt(tipo);

    $('#txtFechaIni').Zebra_DatePicker({
        format: 'd/m/Y', 
    });

    if (!($('#txtFechaIni').val().includes("/"))) {
        if ($('#txtFechaIni').val() != "")
            cambiarFechaADiario($('#txtFechaIni').val());
    }

    switch (tipo1) {

        case 321: //Programa Diario            
            setTexto('#lblturno', 'Turno');
            setTextoVisible('#gbmop', true);
            setTextoVisible('#cbxMailCheck1', true);
            setTexto('#lblcv', 'Costo variable');
            setTexto('#lblesp', 'Especialista');
            setTextoVisible('#cbxEsp', true);
            break;

        case 322: //RePrograma
            setTexto('#lblCausa', 'Causa reprograma');
            setTextoVisible('#txtCausa', true);
            setTexto('#lblbloque', 'Bloque horario');
            setTextoVisible('#cbbloque', true);
            setTexto('#lblcv', 'Reprograma');
            setTextoVisible('#gbhoja', true);
            setTexto('#lblreprogMcp', 'Reprograma YUPANA');
            setTextoVisible('#cbxReProgMcp', true);
            break;

        case 323: //Costo Variable
            setTexto('#lblcv', 'Costo variable');
            setTextoVisible('#gbhoja', true);
            break;

        case 326: //Análisis de la Operación Diaria ATR
            setTexto('#lblturno', 'Turno');
            setTextoVisible('#gbmop', true);
            break;

        case 328: //Racionamiento de carga
            setTexto('#lblEquipo', 'Equipo');
            setTextoVisible('#btnSeleccionarEquipo', true);
            setTexto('#txtEquipo', '.');
            break;

        case 333: //Programa Semanal Final
            setTexto('#lblturno', 'Turno');
            setTextoVisible('#gbmop', true);
            setTexto('#lblesp', 'Especialista');
            setTextoVisible('#cbxEsp', true);
            break;

        case 335: //Término Elab.Programa            
            setTextoVisible('#gbpmop', true);
            setTexto('#lblesp', 'Especialista');
            setTextoVisible('#cbxEsp', true);
            $('#trCoordinador').hide();
            break;

        case 351:
            setTexto('#lblEquipo', 'Equipo');
            setTextoVisible('#btnSeleccionarEquipo', true);
            setTexto('#txtEquipo', '.');
            $('#trProgramador').hide();
            $('#trHora').show();
            $('#trConsecuencia').show();
            break;

        case 403:
        case 410:
            cargarTipoReportes(); 
            document.getElementById('cbxEspSME').value = "0"; //resetea el combo
            break;
        case 404:
        case 405:
        case 406:
        case 407:
        case 408:
        case 409:
            $('#txtFechaIni').Zebra_DatePicker({
                format: 'm Y',
            });
            if ($('#txtFechaIni').val() != "")
                cambiarFechaAMensual($('#txtFechaIni').val());

            cargarTipoReportes();
            document.getElementById('cbxEspSME').value = "0"; //resetea el combo
            setTexto('#lblFecha', 'Mes');
            
            break;

        default:
    }
};
function cargarTipoReportes() {    
    $('#trProgramador').hide();
    $('#trCoordinador').hide();
    $('#trEspecialistaSME').show();
}

function setTextoInvisible() {

    setTexto('#lblturno', '');
    setTextoVisible('#gbmop', false);
    setTextoVisible('#gbpmop', false);
    setTexto('#lblCausa', '');
    setTextoVisible('#txtCausa', false);
    setTexto('#lblEquipo', '');
    setTextoVisible('#btnSeleccionarEquipo', false);
    setTexto('#txtEquipo', '');
    setTexto('#lblbloque', '');
    setTextoVisible('#cbbloque', false);
    setTextoVisible('#cbxMailCheck1', false);
    setTexto('#lblcv', '');
    setTextoVisible('#gbhoja', false);
    setTexto('#lblesp', '');
    setTextoVisible('#cbxEsp', false);
    setTexto('#lblreprogMcp', '');
    setTextoVisible('#cbxReProgMcp', false);
}


function setTextoVisible(id, visible) {

    if (visible)
        $(id).show();
    else
        $(id).hide();

}


function setTexto(id, texto) {

    if (texto != '') {
        $(id).text(texto);
        $(id).show();
    }
    else {
        $(id).hide();
    }
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


validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;

    $('#hfPernomb').val($('#cbxProg').val());
    $('#hfPernombEspec').val($('#cbxEsp').val());
    $('#hfReprograma').val($('#cbxReProgMcp').val());

    var flagDatos = true;
    var tipo = $('#cbTipoEnvio').val();
    if (tipo == "351" || tipo == "403" || tipo == "404" || tipo == "405" || tipo == "406" || tipo == "407" || tipo == "408" || tipo == "409" || tipo == "410") {
        flagDatos = false;
    }

    if (flagDatos) {
        if ($('#hfPernomb').val() == "0" || $('#hfPernomb').val() == "" || $('#hfPernomb').val() == null) {
            mensaje = mensaje + "<li>" + "Debe seleccionar programador" + "</li>";
            flag = false;
        }
    }

    if (!flagDatos) {
        if (tipo == "351") {
            if ($('#txtHora').val() == "" || $('#txtConsecuencia').val() == "") {
                mensaje = mensaje + "<li>Ingrese la hora y la consecuencia.</li>";
                flag = false;
            }
        } else {
            if ($('#cbxEspSME').val() == "0" || $('#cbxEspSME').val() == "" || $('#cbxEspSME').val() == null) {
                mensaje = mensaje + "<li>Ingrese un especialista.</li>";
                flag = false;
            }
        }
    }
    
    var contenido = $('#contenido').val();

    if (contenido == "") {
        mensaje = mensaje + "<li>" + "Debe ingresar contenido de correo" + "</li>";
        flag = false;
    }

    var fechaX = $('#txtFechaIni').val();
    if (fechaX == "") {
        mensaje = mensaje + "<li>" + "Debe ingresar una fecha" + "</li>";
        flag = false;
    }
    if (flag) mensaje = "";
    return mensaje;
}


grabar = function() {
    var mensaje = validarRegistro();

    if (mensaje == "") {

        $('#hfSubcausacodi').val($('#cbTipoEnvio').val());

        //Análisis de la Operación Diaria ATR / Programa Diario
        if ($('#Mantto').is(':checked') || $('#ManttoyOperac').is(':checked') || $('#Operac').is(':checked')) {
            if ($('#Mantto').is(':checked')) $('#hfMailturnonum').val(2);
            if ($('#Operac').is(':checked')) $('#hfMailturnonum').val(3);
            if ($('#ManttoyOperac').is(':checked')) $('#hfMailturnonum').val(4);
        }

        //Término Elab.Programa
        if ($('#PDM')
            .is(':checked') ||
            $('#PSM').is(':checked') ||
            $('#PDO').is(':checked') ||
            $('#PSO').is(':checked')) {
            if ($('#PDM').is(':checked')) $('#hfMailtipoprograma').val(0);
            if ($('#PSM').is(':checked')) $('#hfMailtipoprograma').val(2);
            if ($('#PDO').is(':checked')) $('#hfMailtipoprograma').val(1);
            if ($('#PSO').is(':checked')) $('#hfMailtipoprograma').val(3);
        }

        //RePrograma / Costo Variable
        if ($('#HojaA').is(':checked') ||
            $('#HojaB').is(':checked') ||
            $('#HojaC').is(':checked') ||
            $('#HojaD').is(':checked') ||
            $('#HojaE').is(':checked') ||
            $('#HojaF').is(':checked') ||
            $('#HojaG').is(':checked') ||
            $('#HojaH').is(':checked')
        ) {
            if ($('#HojaA').is(':checked')) $('#hfMailhoja').val('A');
            if ($('#HojaB').is(':checked')) $('#hfMailhoja').val('B');
            if ($('#HojaC').is(':checked')) $('#hfMailhoja').val('C');
            if ($('#HojaD').is(':checked')) $('#hfMailhoja').val('D');
            if ($('#HojaE').is(':checked')) $('#hfMailhoja').val('E');
            if ($('#HojaF').is(':checked')) $('#hfMailhoja').val('F');
            if ($('#HojaG').is(':checked')) $('#hfMailhoja').val('G');
            if ($('#HojaH').is(':checked')) $('#hfMailhoja').val('H');
        }

        //Término Elab.Programa
        if ($('#cbxMailCheck1').is(':checked')) {
            $('#hfMailCheck1').val('S');
        } else {
            $('#hfMailCheck1').val('N');
        }

        //bloque horario
        $('#hfMailbloquehorario').val($('#EveMail_Mailbloquehorario').val());


        var controlador2 = siteRoot + 'eventos/enviarcorreos/';

        $.ajax({
            type: 'POST',
            url: controlador2 + "grabar",
            dataType: 'json',
            data: $('#formEvento').serialize(),
            success: function(result) {
                if (result != "-1") {

                    var idemitido = result.split(',');
                    var id = idemitido[0];
                    var emitido = idemitido[1];

                    mostrarExito();

                    $('#hfMailcodi').val(id);

                    if (emitido == 'N') {
                        //mostrar correo
                        $.ajax({
                            type: 'POST',
                            url: controlador2 + "formatocorreo",
                            data: {
                                id: $('#hfMailcodi').val(),
                                plantilla: 'enviarcorreo'
                            },
                            success: function(result) {

                                $('#contenidoEdicion').html(result);
                                setTimeout(function() {
                                        $('#popupEdicion').bPopup({
                                            autoClose: false
                                        });

                                    },
                                    50);

                                cargaDeArchivo();

                                $.ajax({
                                    success: function() {

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
                                                    $.ajax({
                                                        type: 'POST',
                                                        url: controlador2 + "estadoCorreoEnviado",
                                                        dataType: 'json',
                                                        cache: false,
                                                        data: {
                                                            mailcodi: $('#hfMailcodi').val()
                                                        },
                                                        success: function (result) {
                                                            if (result >= 1) {
                                                                $('#popupEdicion').bPopup().close();
                                                                //ver ventana buscar()
                                                                document.location
                                                                    .href = siteRoot + 'eventos/enviarcorreos/';
                                                            }
                                                            if (result == -1) {
                                                                mostrarError();
                                                            }
                                                        },
                                                        error: function () {
                                                            mostrarError();
                                                        }
                                                    });

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
                            error: function() {

                            }


                        });
                    } else {
                        //ver ventana buscar()
                        document.location.href = siteRoot + 'eventos/enviarcorreos/';
                    }
                }
                if (result == -1) {
                    mostrarError();
                }
            },
            error: function() {
                mostrarError();
            }
        });
    } else {
        mostrarAlerta(mensaje);
    }
}


mostrarError = function() {
    alert("Ha ocurrido un error");
}


seleccionarEquipo = function (idEquipo, substacion, equipo, empresa, idEmpresa) {

    var controlador2 = siteRoot + 'eventos/enviarcorreos/';

    var tipoEquipo = '';

    $.ajax({
        type: 'POST',
        url: controlador2 + 'seleccEquipo',
        dataType: 'json',
        data: { idEquipo: idEquipo },
        cache: false,
        success: function (result) {
            $('#hfEquicodi').val(idEquipo);
            //tipoEquipo = result;
            $('#txtEquipo').val(result);
        },
        error: function () {
            mostrarError();
        },
    });

    
    $('#busquedaEquipo').bPopup().close();
}


iniciarControlTexto = function (id) {
    
    var result = $('#' + id).val();
    
    tinymce.remove();
    
    tinymce.init({
        selector: '#' + id,
        plugins: [
            'paste textcolor colorpicker textpattern link preview'
        ],
        toolbar1:
            'insertfile undo redo | bold italic | alignleft aligncenter alignright alignjustify |  bullist numlist outdent indent| forecolor backcolor link preview',
        menubar: false,
        language: 'es',
        statusbar: false,
        convert_urls: false,
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


function cambiarFechaAMensual(fechaDiaria) {
    var arrayDatos = fechaDiaria.split('/');
    var dia = arrayDatos[0];
    var mes = arrayDatos[1];
    var anio = arrayDatos[2];
    var valor = mes + ' ' + anio;

    $('#txtFechaIni').val(valor);
}

function cambiarFechaADiario(fechaMensual) {
    var arrayDatos = fechaMensual.split(' ');
    var dia = '01';
    var mes = arrayDatos[0];
    var anio = arrayDatos[1];
    var valor = dia + '/' + mes + '/' + anio;

    $('#txtFechaIni').val(valor);
}






