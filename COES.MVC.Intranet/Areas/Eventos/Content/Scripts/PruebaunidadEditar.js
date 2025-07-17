var controlador = siteRoot + 'eventos/pruebaunidad/';


$(function () {

    $('#txtPrundFecha').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "datetime"
    });

    $('#txtPrundFecha').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtPrundFecha').val(date);
            cargarUnidadSorteada();
        }
    });

    $('#txtPrundHoraordenarranque').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtPrundHoraordenarranque').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtPrundHoraordenarranque').val(date);
        }
    });

    $('#txtPrundHorasincronizacion').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtPrundHorasincronizacion').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtPrundHorasincronizacion').val(date);
        }
    });

    $('#txtPrundHorainiplenacarga').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtPrundHorainiplenacarga').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtPrundHorainiplenacarga').val(date);
        }
    });

    $('#txtPrundHorafalla').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtPrundHorafalla').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtPrundHorafalla').val(date);
        }
    });

    $('#txtPrundHoraordenarranque2').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtPrundHoraordenarranque2').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtPrundHoraordenarranque2').val(date);
        }
    });

    $('#txtPrundHorasincronizacion2').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtPrundHorasincronizacion2').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtPrundHorasincronizacion2').val(date);
        }
    });

    $('#txtPrundHorainiplenacarga2').inputmask({
        mask: "1/2/y h:s",
        placeholder: "dd/mm/yyyy hh:mm",
        alias: "datetime",
        hourFormat: "24"
    });

    $('#txtPrundHorainiplenacarga2').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {
            $('#txtPrundHorainiplenacarga2').val(date);
        }
    });


    $('#rbPrundEscenario1').click(function () {
        activarEscenario(1);
    });


    $('#rbPrundEscenario2').click(function () {
        activarEscenario(2);
    });

    $('#rbPrundEscenario3').click(function () {
        activarEscenario(3);
    });

    $('#rbPrundEscenario4').click(function () {
        activarEscenario(4);
    });


    $('#btnCancelar').click(function () {
        document.location.href = controlador;
    });


    $('#cbModo').change(function () {
        cargarParametro($('#cbModo').val());
    });


    $(document).ready(function () {

        $('#cbModo').val("0");

        $('#rbPrundEscenario1').prop('checked', true);
        if ($('#hfPrundEscenario').val() == '1') { $('#rbPrundEscenario1').prop('checked', true); }
        if ($('#hfPrundEscenario').val() == '2') { $('#rbPrundEscenario2').prop('checked', true); }
        if ($('#hfPrundEscenario').val() == '3') { $('#rbPrundEscenario3').prop('checked', true); }
        if ($('#hfPrundEscenario').val() == '4') { $('#rbPrundEscenario4').prop('checked', true); }
        $('#rbPrundSegundadesconxN').prop('checked', true);
        if ($('#hfPrundSegundadesconx').val() == 'S') { $('#rbPrundSegundadesconxS').prop('checked', true); }
        if ($('#hfPrundSegundadesconx').val() == 'N') { $('#rbPrundSegundadesconxN').prop('checked', true); }
        $('#rbPrundFallaotranosincronzN').prop('checked', true);
        if ($('#hfPrundFallaotranosincronz').val() == 'S') { $('#rbPrundFallaotranosincronzS').prop('checked', true); }
        if ($('#hfPrundFallaotranosincronz').val() == 'N') { $('#rbPrundFallaotranosincronzN').prop('checked', true); }
        $('#rbPrundFallaotraunidsincronzN').prop('checked', true);
        if ($('#hfPrundFallaotraunidsincronz').val() == 'S') { $('#rbPrundFallaotraunidsincronzS').prop('checked', true); }
        if ($('#hfPrundFallaotraunidsincronz').val() == 'N') { $('#rbPrundFallaotraunidsincronzN').prop('checked', true); }
        $('#rbPrundFallaequiposinreingresoN').prop('checked', true);
        if ($('#hfPrundFallaequiposinreingreso').val() == 'S') { $('#rbPrundFallaequiposinreingresoS').prop('checked', true); }
        if ($('#hfPrundFallaequiposinreingreso').val() == 'N') { $('#rbPrundFallaequiposinreingresoN').prop('checked', true); }
        $('#rbPrundCalchayregmedidX').prop('checked', true);
        if ($('#hfPrundCalchayregmedid').val() == 'S') { $('#rbPrundCalchayregmedidS').prop('checked', true); }
        if ($('#hfPrundCalchayregmedid').val() == 'N') { $('#rbPrundCalchayregmedidN').prop('checked', true); }
        if ($('#hfPrundCalchayregmedid').val() == 'X') { $('#rbPrundCalchayregmedidX').prop('checked', true); }
        $('#rbPrundCalhayindispX').prop('checked', true);
        if ($('#hfPrundCalhayindisp').val() == 'S') { $('#rbPrundCalhayindispS').prop('checked', true); }
        if ($('#hfPrundCalhayindisp').val() == 'N') { $('#rbPrundCalhayindispN').prop('checked', true); }
        if ($('#hfPrundCalhayindisp').val() == 'X') { $('#rbPrundCalhayindispX').prop('checked', true); }
        $('#rbPrundCalcpruebaexitosaX').prop('checked', true);
        if ($('#hfPrundCalcpruebaexitosa').val() == 'S') { $('#rbPrundCalcpruebaexitosaS').prop('checked', true); $('#lblResult').val(""); $('#lblResult').css("display", "none"); }
        if ($('#hfPrundCalcpruebaexitosa').val() == 'N') { $('#rbPrundCalcpruebaexitosaN').prop('checked', true); $('#lblResult').val("Unidad Regresa a Sorteo"); $('#lblResult').css("display", "inline"); }
        if ($('#hfPrundCalcpruebaexitosa').val() == 'X') { $('#rbPrundCalcpruebaexitosaX').prop('checked', true); $('#lblResult').val(""); $('#lblResult').css("display", "none"); }
        $('#rbPrundCalccondhoratarrX').prop('checked', true);
        if ($('#hfPrundCalccondhoratarr').val() == 'S') { $('#rbPrundCalccondhoratarrS').prop('checked', true); }
        if ($('#hfPrundCalccondhoratarr').val() == 'N') { $('#rbPrundCalccondhoratarrN').prop('checked', true); }
        if ($('#hfPrundCalccondhoratarr').val() == 'X') { $('#rbPrundCalccondhoratarrX').prop('checked', true); }
        $('#rbPrundCalccondhoraprogtarrX').prop('checked', true);
        if ($('#hfPrundCalccondhoraprogtarr').val() == 'S') { $('#rbPrundCalccondhoraprogtarrS').prop('checked', true); }
        if ($('#hfPrundCalccondhoraprogtarr').val() == 'N') { $('#rbPrundCalccondhoraprogtarrN').prop('checked', true); }
        if ($('#hfPrundCalccondhoraprogtarr').val() == 'X') { $('#rbPrundCalccondhoraprogtarrX').prop('checked', true); }
        $('#rbPrundCalcindispprimtramoX').prop('checked', true);
        if ($('#hfPrundCalcindispprimtramo').val() == 'S') { $('#rbPrundCalcindispprimtramoS').prop('checked', true); }
        if ($('#hfPrundCalcindispprimtramo').val() == 'N') { $('#rbPrundCalcindispprimtramoN').prop('checked', true); }
        if ($('#hfPrundCalcindispprimtramo').val() == 'X') { $('#rbPrundCalcindispprimtramoX').prop('checked', true); }
        $('#rbPrundCalcindispsegtramoX').prop('checked', true);
        if ($('#hfPrundCalcindispsegtramo').val() == 'S') { $('#rbPrundCalcindispsegtramoS').prop('checked', true); }
        if ($('#hfPrundCalcindispsegtramo').val() == 'N') { $('#rbPrundCalcindispsegtramoN').prop('checked', true); }
        if ($('#hfPrundCalcindispsegtramo').val() == 'X') { $('#rbPrundCalcindispsegtramoX').prop('checked', true); }

        $('#rbPrundEliminadoN').prop('checked', true);

        if ($('#hfPrundEliminado').val() == 'S') { $('#rbPrundEliminadoS').prop('checked', true); }
        if ($('#hfPrundEliminado').val() == 'N') { $('#rbPrundEliminadoN').prop('checked', true); }


        if ($('#hfAccion').val() == 0) {
            $('#btnGrabar').hide();
        }


        activarEscenario($('#hfPrundEscenario').val());

        cargarUnidadSorteada();

    });

    $('#btnGrabar').click(function () {
        grabar();
    });

});


cargarUnidadSorteada = function () {
    $('#cbModo').empty();
    limpiarTextoParametro();

    $.ajax({
        type: 'POST',
        url: controlador + "unidadsorteada",
        data: {
            fecha: $('#txtPrundFecha').val()
        },
        dataType: 'json',
        cache: false,
        success: function (evt) {

            var cad = evt;

            $('#cbModo').empty();
            var n = cad.indexOf(",");

            var equicodi = cad.substr(0, n);
            var equinomb = cad.substr(n + 1);

            $('#txtPrundUnidad').val(equinomb);
            $('#txtPrundUnidad2').val(equinomb);

            //recogiendo los modos de operacion
            var select2 = document.getElementById("cbModo");
            select2.options[select2.options.length] = new Option("--SELECCIONE MODO--", "-1");

            if (equicodi != "-1") {
                $('#hfEquicodi').val(equicodi);

                //--------------

                $.ajax({
                    type: 'POST',
                    url: controlador + "modounidadsorteada",
                    data: {
                        equicodi: equicodi
                    },
                    dataType: 'json',
                    cache: false,
                    success: function (evt) {

                        var _len = evt.length;
                        var cad1 = _len + "\r\n";

                        for (i = 0; i < _len; i++) {
                            post = evt[i];
                            var select = document.getElementById("cbModo");
                            select.options[select.options.length] = new Option(post.Gruponomb, post.Grupocodi);
                        }

                        $('#cbModo').val($('#hfModo').val());

                        if ($('#cbModo').val() != "0") {
                            //cargarParametro($('#cbModo').val());
                            cargarParametroSecundario($('#cbModo').val());
                        }

                    },
                    error: function (xhr, textStatus, exceptionThrown) {
                        mostrarError();
                    }
                });

                //--------------
            }
        },
        error: function (xhr, textStatus, exceptionThrown) {
            mostrarError();
        }
    });

}


cargarParametro = function (grupocodi) {
    if (grupocodi == null) {
        $('#cbModo').val("0");
        return;
    }

    limpiarTextoParametro();



    if (grupocodi != "-1") {

        $.ajax({
            type: 'POST',
            url: controlador + "parametrounidadsorteada",
            data: {
                grupocodi: grupocodi,
                fecha: $('#txtPrundFecha').val(),
                equicodi: $('#hfEquicodi').val()
            },
            dataType: 'json',
            cache: false,
            success: function (evt) {

                $('#txtPrundPotencia').val(evt.PotenciaEfectiva);
                $('#txtPrundTiempoarranque').val(evt.TiempoEntreArranques);
                $('#txtPrundTiempoarranqueSinc').val(evt.TiempoArranqueSinc);
                $('#txtPrundTiempoarranqueSincPef').val(evt.TiempoSincPotEf);


                $('#txtPrundRpf').val(evt.Rpf);
                $('#txtPrundTiempoprueba').val(evt.TiempoPrueba);
                $('#txtPrundRatio').val(evt.PrundRatio);


            },
            error: function (xhr, textStatus, exceptionThrown) {
                mostrarError();
            }
        });

    }
}


cargarParametroSecundario = function (grupocodi) {
    if (grupocodi == null) {
        $('#cbModo').val("0");
        return;
    }

    limpiarTextoParametro();



    if (grupocodi != "-1") {

        $.ajax({
            type: 'POST',
            url: controlador + "parametrounidadsorteada",
            data: {
                grupocodi: grupocodi,
                fecha: $('#txtPrundFecha').val(),
                equicodi: $('#hfEquicodi').val()
            },
            dataType: 'json',
            cache: false,
            success: function (evt) {

                $('#txtPrundRpf').val(evt.Rpf);
                $('#txtPrundTiempoprueba').val(evt.TiempoPrueba);
                $('#txtPrundRatio').val(evt.PrundRatio);


            },
            error: function (xhr, textStatus, exceptionThrown) {
                mostrarError();
            }
        });

    }
}


limpiarTextoParametro = function () {

    //$('#txtPrundPotencia').val("");
    //$('#txtPrundTiempoarranque').val("");
    //$('#txtPrundTiempoarranqueSinc').val("");
    //$('#txtPrundTiempoarranqueSincPef').val("");

    $('#txtPrundRpf').val("");
    $('#txtPrundTiempoprueba').val("");
    $('#txtPrundRatio').val("");
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


activarEscenario = function (escenario) {

    var log = false;

    setTextoVisible(txtPrundHorasincronzBase, escenario != 2 || log); //HS
    setTextoVisible(txtPrundHorainiplenacargaBase, (escenario != 2 && escenario != 3) || log); //HPE
    setTextoVisible(txtPrundHorafallaBase, escenario != 1 || log);
    setTextoVisible(txtPrundHo2Base, escenario != 1 || log);
    setTextoVisible(txtPrundHs2Base, escenario != 1 || log);
    setTextoVisible(txtPrundHpe2Base, escenario != 1 || log);
    setTextoVisible(txtPrundSegDescBase, escenario != 1 || log);

    setTextoVisible(txtPrundCalcperiodoprogpruebaBase, escenario != 1 || log);
    setTextoVisible(txtPrundCalccondhoratarrBase, escenario == 2 || log);
    setTextoVisible(txtPrundCalccondhoraprogtarrBase, (escenario != 1 && escenario != 2) || log);

    setTextoVisible(txtPrundCalcindispprimtramoBase, escenario == 4 || log);
    setTextoVisible(txtPrundCalcindispsegtramoBase, escenario == 4 || log);

    setTextoVisible(txtPrundCalhayindispBase, escenario != 4 || log);

}


function setTextoVisible(psId, pbVisible) {
    var str1 = "";
    if (pbVisible) {
        str1 = "inline";
        $(psId).show();
    }
    else {
        str1 = "none";
        $(psId).hide();
    }

}

validarNumero = function (valor) {

    return !isNaN(parseFloat(valor)) && isFinite(valor);
}


validarRegistro = function () {

    var mensaje = "<ul>";
    var flag = true;


    if ($('#rbPrundEscenario1').is(':checked')) {
        $('#hfPrundEscenario').val('1');
    }

    if ($('#rbPrundEscenario2').is(':checked')) {
        $('#hfPrundEscenario').val('2');
    }

    if ($('#rbPrundEscenario3').is(':checked')) {
        $('#hfPrundEscenario').val('3');
    }

    if ($('#rbPrundEscenario4').is(':checked')) {
        $('#hfPrundEscenario').val('4');
    }

    if ($('#rbPrundSegundadesconxS').is(':checked')) {
        $('#hfPrundSegundadesconx').val('S');
    }

    if ($('#rbPrundSegundadesconxN').is(':checked')) {
        $('#hfPrundSegundadesconx').val('N');
    }

    if ($('#rbPrundFallaotranosincronzS').is(':checked')) {
        $('#hfPrundFallaotranosincronz').val('S');
    }

    if ($('#rbPrundFallaotranosincronzN').is(':checked')) {
        $('#hfPrundFallaotranosincronz').val('N');
    }

    if ($('#rbPrundFallaotraunidsincronzS').is(':checked')) {
        $('#hfPrundFallaotraunidsincronz').val('S');
    }

    if ($('#rbPrundFallaotraunidsincronzN').is(':checked')) {
        $('#hfPrundFallaotraunidsincronz').val('N');
    }

    if ($('#rbPrundFallaequiposinreingresoS').is(':checked')) {
        $('#hfPrundFallaequiposinreingreso').val('S');
    }

    if ($('#rbPrundFallaequiposinreingresoN').is(':checked')) {
        $('#hfPrundFallaequiposinreingreso').val('N');
    }

    if ($('#rbPrundCalchayregmedidS').is(':checked')) {
        $('#hfPrundCalchayregmedid').val('S');
    }

    if ($('#rbPrundCalchayregmedidN').is(':checked')) {
        $('#hfPrundCalchayregmedid').val('N');
    }

    if ($('#rbPrundCalchayregmedidX').is(':checked')) {
        $('#hfPrundCalchayregmedid').val('X');
    }

    if ($('#rbPrundCalhayindispS').is(':checked')) {
        $('#hfPrundCalhayindisp').val('S');
    }

    if ($('#rbPrundCalhayindispN').is(':checked')) {
        $('#hfPrundCalhayindisp').val('N');
    }

    if ($('#rbPrundCalhayindispX').is(':checked')) {
        $('#hfPrundCalhayindisp').val('X');
    }

    if ($('#rbPrundCalcpruebaexitosaS').is(':checked')) {
        $('#hfPrundCalcpruebaexitosa').val('S');
        $('#lblResult').val("");
        $('#lblResult').css("display", "none");
    }

    if ($('#rbPrundCalcpruebaexitosaN').is(':checked')) {
        $('#hfPrundCalcpruebaexitosa').val('N');
        $('#lblResult').val("Unidad Regresa a Sorteo");
        $('#lblResult').css("display", "inline");
    }

    if ($('#rbPrundCalcpruebaexitosaX').is(':checked')) {
        $('#hfPrundCalcpruebaexitosa').val('X');
        $('#lblResult').val("");
        $('#lblResult').css("display", "none");
    }

    if ($('#rbPrundCalccondhoratarrS').is(':checked')) {
        $('#hfPrundCalccondhoratarr').val('S');
    }

    if ($('#rbPrundCalccondhoratarrN').is(':checked')) {
        $('#hfPrundCalccondhoratarr').val('N');
    }

    if ($('#rbPrundCalccondhoratarrX').is(':checked')) {
        $('#hfPrundCalccondhoratarr').val('X');
    }

    if ($('#rbPrundCalccondhoraprogtarrS').is(':checked')) {
        $('#hfPrundCalccondhoraprogtarr').val('S');
    }

    if ($('#rbPrundCalccondhoraprogtarrN').is(':checked')) {
        $('#hfPrundCalccondhoraprogtarr').val('N');
    }
    if ($('#rbPrundCalccondhoraprogtarrX').is(':checked')) {
        $('#hfPrundCalccondhoraprogtarr').val('X');
    }

    if ($('#rbPrundCalcindispprimtramoS').is(':checked')) {
        $('#hfPrundCalcindispprimtramo').val('S');
    }

    if ($('#rbPrundCalcindispprimtramoN').is(':checked')) {
        $('#hfPrundCalcindispprimtramo').val('N');
    }

    if ($('#rbPrundCalcindispprimtramoX').is(':checked')) {
        $('#hfPrundCalcindispprimtramo').val('X');
    }

    if ($('#rbPrundCalcindispsegtramoS').is(':checked')) {
        $('#hfPrundCalcindispsegtramo').val('S');
    }

    if ($('#rbPrundCalcindispsegtramoN').is(':checked')) {
        $('#hfPrundCalcindispsegtramo').val('N');
    }

    if ($('#rbPrundEliminadoS').is(':checked')) {
        $('#hfPrundEliminado').val('S');
    }

    if ($('#rbPrundEliminadoN').is(':checked')) {
        $('#hfPrundEliminado').val('N');
    }

    $('#hfModo').val($('#cbModo').val());


    if ($('#hfModo').val() == "0" || $('#hfModo').val() == "-1" || $('#hfModo').val() == '') {
        mensaje = mensaje + "<li>Seleccione el modo de operación.</li>";
        flag = false;
    }

    //Validando números
    if (!validarNumero($('#txtPrundPotencia').val())) {
        mensaje = mensaje + "<li>Potencia Efectiva no es numérico</li>";
        flag = false;
    }

    if (!validarNumero($('#txtPrundTiempoarranque').val())) {
        mensaje = mensaje + "<li>Tiempo entre arranque no es numérico</li>";
        flag = false;
    }

    if (!validarNumero($('#txtPrundTiempoarranqueSinc').val())) {
        mensaje = mensaje + "<li>Tiempo de arranque a sincronización no es numérico</li>";
        flag = false;
    }

    if (!validarNumero($('#txtPrundTiempoarranqueSincPef').val())) {
        mensaje = mensaje + "<li>Tiempo de Sincronización a Potencia Efectiva no es numérico</li>";
        flag = false;
    }


    if (flag) mensaje = "";
    return mensaje;
}



grabar = function () {
    var mensaje = validarRegistro();

    if (mensaje == "") {

        $.ajax({
            type: 'POST',
            url: controlador + "grabar",
            dataType: 'json',
            data: $('#form').serialize(),
            success: function (result) {
                var cod = "-1";
                var mensajeInt = "";
                var cad = result;
                var idx = cad.indexOf(",");

                if (idx < 0) {
                    cod = cad;
                }
                else {
                    cod = cad.substr(0, idx);
                    mensajeInt = cad.substr(idx + 1);
                }

                if (cod != "-1") {

                    mostrarExito();
                    $('#hfPrundCodi').val(cod);
                    document.location.href = controlador + "editar?id=" + $('#hfPrundCodi').val() + "&accion=1";

                }
                else {
                    mostrarAlerta(mensajeInt);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta(mensaje);
    }
}

mostrarError = function () {
    alert("Ha ocurrido un error");
}



