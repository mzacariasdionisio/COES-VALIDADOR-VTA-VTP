var controlador = siteRoot + 'Eventos/AnalisisFallas/';

$(function () {
    $('#tab-container').easytabs();

    $('#txtHoraEnPlazo').keyup(function () {
        actualizarVistaPrevia();
        $('#btnGuardarPlazo').removeAttr("disabled");
    });

    $('#txtHoraEnPlazo').keyup(function (event) {        
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/([0-9])([0-9]{2})$/, '$1:$2')
                ;
        });
    });

    $('#txtHoraFinPlazo').keyup(function (event) {
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/([0-9])([0-9]{2})$/, '$1:$2')
                ;
        });
    });

    $('#txtHoraEjecucion').keyup(function (event) {
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/([0-9])([0-9]{2})$/, '$1:$2')
                ;
        });
    });

    $('#txtHoraEnPlazo').change(function () {
        actualizarVistaPrevia();
        $('#btnGuardarPlazo').removeAttr("disabled");
    });
    $('#txtHoraFinPlazo').change(function () {
        actualizarVistaPrevia();
        $('#btnGuardarPlazo').removeAttr("disabled");
    });

    $('#btnGuardarPlazo').on("click", function () {
        var msj = ValidarConfiguracionPlazo();

        if (msj === "") {
            GuardarHoraPlazo();
        }
        else {
            $('#msjConfiguracionPlazo').show();
            $('#msjConfiguracionPlazo').removeClass();
            $('#msjConfiguracionPlazo').html(ValidarConfiguracionPlazo());
            $('#msjConfiguracionPlazo').addClass('action-alert');
        }
    });

    
    $('#btnGuardarHoraE').on("click", function () {
        var msj = ValidarConfiguracionProceso();

        if (msj === "") {
            GuardarHoraProceso();
        }
        else {
            $('#msjConfiguracionProceso').show();
            $('#msjConfiguracionProceso').removeClass();
            $('#msjConfiguracionProceso').html(ValidarConfiguracionProceso());
            $('#msjConfiguracionProceso').addClass('action-alert');
        }
    });


    $('#txtFechaPeriodo').inputmask({
        mask: "1/2/y h:s:s",
        placeholder: "dd/mm/yyyy hh:mm:ss",
        alias: "datetime",
        hourFormat: "24"
    });
    $('#txtFechaPeriodo').Zebra_DatePicker({
        readonly_element: false,
        format: 'd/m/Y',
        onSelect: function (date) {
            $('#txtFechaPeriodo').val(date + " 00:00:00");
            actualizarVistaPrevia();
        }
    });
    $('#txtFechaPeriodo').change(function () {
        actualizarVistaPrevia();
    });

    actualizarVistaPrevia();
});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Configuración de plazos Extranet
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function GuardarHoraPlazo() {
    var horasEnPlazo = $('#txtHoraEnPlazo').val();
    var horasFueraPlazo = $('#txtHoraFinPlazo').val();

    $.ajax({
        type: 'POST',
        url: controlador + "GuardarConfiguracionPlazoExtranetCTAF",
        data: {
            horasEnPlazo: horasEnPlazo,
            horasFueraPlazo: horasFueraPlazo
        },
        dataType: 'json',
        success: function (model) {
            if (model.Resultado !== '-1') {
                $('#msjConfiguracionPlazo').removeClass();
                $('#msjConfiguracionPlazo').addClass('action-exito');
                $('#msjConfiguracionPlazo').text('La configuración se guardó Correctamente!');
            } else {
                $('#msjConfiguracionPlazo').removeClass();
                $('#msjConfiguracionPlazo').addClass('action-error');
                $('#msjConfiguracionPlazo').text('Error al Guardar configuración: ' + model.StrMensaje);
            }
        },
        error: function (result) {
            alert("Ha ocurrido un error.");
        }
    });
}

function ValidarConfiguracionPlazo() {
    var msj1 = ValidarHora('txtHoraEnPlazo', 'En Plazo');
    var msj2 = ValidarHora('txtHoraFinPlazo', 'Fin Plazo');

    if (msj1 != '') {
        //return msj1 + "<br/>" + msj2;
        if (msj2 != '')
            return msj1 + "<br/> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; " + msj2;
        else
            return msj1;
    } else {
        if (msj2 != '')
            return msj2;
        else
            return '';
    }
}

function GuardarHoraProceso() {
    var horaEjecucion = $('#txtHoraEjecucion').val();

    $.ajax({
        type: 'POST',
        url: controlador + "GuardarHoraEjecucionAlertaCTAF",
        data: {
            horaEjecucion: horaEjecucion
        },
        dataType: 'json',
        success: function (model) {
            if (model.Resultado !== '-1') {
                $('#msjConfiguracionProceso').removeClass();
                $('#msjConfiguracionProceso').addClass('action-exito');
                $('#msjConfiguracionProceso').text('La configuración se guardó Correctamente!');
            } else {
                $('#msjConfiguracionProceso').removeClass();
                $('#msjConfiguracionProceso').addClass('action-error');
                $('#msjConfiguracionProceso').text('Error al Guardar configuración: ' + model.StrMensaje);
            }
        },
        error: function (result) {
            alert("Ha ocurrido un error.");
        }
    });
}

function ValidarConfiguracionProceso() {
    var msj1 = ValidarHora('txtHoraEjecucion', 'Hora Ejecución');

    if (msj1 != '') {
        
            return msj1;
    } else {        
            return '';
    }
}

function ValidarHora(idInputHora, campo) {
    var inputHora = $("#" + idInputHora).val();
    var resultado = inputHora.split(":");
    var hora = resultado[0];
    var minuto = resultado[1];

    var msj = '';

    if (Number.isInteger(parseInt(hora),10)) {
        if (Number.isInteger(parseInt(minuto),10)) {
            if (inputHora.replace(/\s/g, '') === "") {
                msj = "Ingrese un Valor";
            }
            else {
                //if (hora > 24) {
                //    HoraEje = "Ingrese Hora Correcta";
                //}

                if (minuto >= 60) {
                    msj = "Ingrese Minuto Correcto";
                }
            }
            if (inputHora.length !== 5) {
                msj = "Ingrese una hora válida con el siguiente formato. 'hh:mm'";
            }

            if (msj != '') {
                msj = campo + ": " + msj;
                $('#' + idInputHora).focus();
                flagHoraEje = false;
            }
        } else {
            msj = "Ingrese una Hora Válida";
            msj = campo + ": " + msj;
        }
    } else {
        msj = "Ingrese una Hora Válida";
        msj = campo + ": " + msj;
    }
    

    return msj;
}

function actualizarVistaPrevia() {
    var inputHora1 = $("#txtHoraEnPlazo").val();
    var inputHora2 = $("#txtHoraFinPlazo").val();
    var strFecha = $("#txtFechaPeriodo").val();

    var dIniPlazo = moment(convertStringToDate(strFecha));
    var dFinPlazo = moment(convertStringToDate(strFecha));
    var dFueraPlazo = moment(convertStringToDate(strFecha));

    var resultado1 = inputHora1.split(":");
    var hora1 = resultado1[0];
    var minuto1 = resultado1[1];

    var resultado2 = inputHora2.split(":");
    var hora2 = resultado2[0];
    var minuto2 = resultado2[1];

    dIniPlazo = dIniPlazo;
    dFinPlazo = dFinPlazo.add(hora1, 'hours').add(minuto1, 'minutes');
    dFueraPlazo = dFueraPlazo.add(hora2, 'hours').add(minuto2, 'minutes');

    $("#txtFechaEnPlazo1").text(dIniPlazo.format('DD/MM/YYYY HH:mm:ss'));
    $("#txtFechaEnPlazo2").text(dFinPlazo.format('DD/MM/YYYY HH:mm:ss'));
    $("#txtFechaFueraPlazo1").text(dFinPlazo.format('DD/MM/YYYY HH:mm:ss'));
    $("#txtFechaFueraPlazo2").text(dFueraPlazo.format('DD/MM/YYYY HH:mm:ss'));
}

//convierte 2 cadenas de texto fecha(dd/mm/yyyy) y horas(hh:mm:ss) a tipo Date
function convertStringToDate(strfecha) {
    var fecha = strfecha.substring(0, 10);
    var horas = strfecha.substring(10, 19);

    var partsFecha = fecha.split('/');
    if (horas == "") {
        return "";
    }
    var partsHoras = horas.split(':');
    //new Date(yyyy, mm-1, dd, hh, mm, ss);
    return new Date(partsFecha[2], partsFecha[1] - 1, partsFecha[0], partsHoras[0], partsHoras[1], partsHoras[2]);
}