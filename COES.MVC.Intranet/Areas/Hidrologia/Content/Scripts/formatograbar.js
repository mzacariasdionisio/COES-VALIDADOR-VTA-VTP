var controlador = siteRoot + 'hidrologia/';

$(function () {
    $("#frmFormato").validate({
        submitHandler: function () {
            grabarFormato();
        },
        rules: {
            Nombre: {
                required: true,
                minlength: 9
            },
            Descripcion: {
                required: true,
                minlength: 9
            },
            //Inicio Plazo
            txtMinPlazo: {
                required: true,
                minlength: 5

            },
            DiaPlazo: {
                required: true,
                min: -30,
                max: 30
            },
            Mesplazo: {
                required: true,
                min: -12,
                max: 12
            },
            //Fin Plazo
            txtMinFinPlazo: {
                required: true,
                minlength: 5

            },
            DiaFinPlazo: {
                required: true,
                min: -30,
                max: 30
            },
            Mesfinplazo: {
                required: true,
                min: -12,
                max: 12
            },
            //Fuera de Plazo
            txtMinFinFueraPlazo: {
                required: true,
                minlength: 5

            },
            DiaFinFueraPlazo: {
                required: true,
                min: -30,
                max: 30
            },
            Mesfinfueraplazo: {
                required: true,
                min: -12,
                max: 12
            },
            IdCabecera: {
                min: 1
            },
            IdModulo: {
                min: 1
            },
            IdArea: {
                min: 1
            },
            IdPeriodo: {
                min: 1
            },
            Horizonte: {
                min: 1
            },
            Resolucion: {
                min: 1
            },
            AllEmpresa: {
                min: 0
            },
            IdFormato2: {
                min: 1
            }
        },
        messages: {
            Nombre: {
                required: "Ingrese nombre de formato",
                minlength: "Mínino 9 caracteres"
            },
            Descripcion: {
                required: "Ingrese descripción de formato",
                minlength: "Mínino 9 caracteres"
            },
            txtMinPlazo: {
                required: "Ingrese la hora de plazo",
                minlength: "Mínino 4 caracteres"
            },
            DiaPlazo: {
                min: "Dia mayor que -30",
                required: "Ingrese el día de plazo",
                max: "Dia menor que 30"
            },
            MesPlazo: {
                min: "Mes mayor o igual a 0",
                required: "Ingrese el mes de plazo",
                max: "Mes menor que 12"
            },
            txtMinFinPlazo: {
                required: "Ingrese la hora de plazo",
                minlength: "Mínino 4 caracteres"
            },
            DiaFinPlazo: {
                min: "Dia mayor que -30",
                required: "Ingrese el día de plazo",
                max: "Dia menor que 30"
            },
            Mesfinplazo: {
                min: "Mes mayor o igual a 0",
                required: "Ingrese el mes de plazo",
                max: "Mes menor que 12"
            },
            txtMinFinFueraPlazo: {
                required: "Ingrese la hora de plazo",
                minlength: "Mínino 4 caracteres"
            },
            DiaFinFueraPlazo: {
                min: "Dia mayor que -30",
                required: "Ingrese el día de plazo",
                max: "Dia menor que 30"
            },
            Mesfinfueraplazo: {
                min: "Mes mayor o igual a 0",
                required: "Ingrese el mes de plazo",
                max: "Mes menor que 12"
            },
            IdCabecera: {
                min: " Seleccionar Opción"
            },
            IdModulo: {
                min: " Seleccionar Opción"
            },
            IdArea: {
                min: " Seleccionar Opción"
            },
            Periodo: {
                min: " Seleccionar Opción"
            },
            Horizonte: {
                min: " Seleccionar Opción"
            },
            Resolucion: {
                min: " Seleccionar Opción"
            },
            AllEmpresa: {
                min: " Seleccionar Opción"
            },
            IdFormato2: {
                min: " Seleccionar Opción"
            }
        }
    });



    $('#btnCancelar').click(function () {
        $('#popupFormato').bPopup().close();
        recargar();
    });

    $("#IdLectura").val($("#hfLectura").val());
    $("#IdModulo").val($("#hfModulo").val());
    $("#IdCabecera").val($("#hfCabecera").val());
    $("#IdArea").val($("#hfArea").val());
    $("#Periodo").val($("#hfPeriodo").val());
    $("#Horizonte").val($("#hfHorizonte").val());
    $("#Resolucion").val($("#hfResolucion").val());
    $("#AllEmpresa").val($("#hfUsuario").val());
    $("#IdFormato2").val($("#hfFormato2").val());
    if ($("#hfCheckBlanco").val() == 1) {
        $("#chbBlanco").prop("checked", true);
    }
    else {
        $("#chbBlanco").prop("checked", false);
    }
    if ($("#hfCheckPlazo").val() == 1) {
        $("#chbPlazo").prop("checked", true);
    }
    else {
        $("#chbPlazo").prop("checked", false);
    }

    $(".dia_config").hide();
    $(".semana_config").hide();
    $(".mes_config").hide();
    $('#Periodo').change(function () {
        configurarVistaPrevia();
    });

    var confIni = obtenerDatoHoraMin($('#hfMinPlazo').val());
    $('#txtMinPlazo').val(confIni[0] + ":" + confIni[1]);
    $('#txtMinPlazo').mask('00:00');

    var confFin = obtenerDatoHoraMin($('#hfMinFinPlazo').val());
    $('#txtMinFinPlazo').val(confFin[0] + ":" + confFin[1]);
    $('#txtMinFinPlazo').mask('00:00');

    var confFinFuera = obtenerDatoHoraMin($('#hfMinFinFueraPlazo').val());
    $('#txtMinFinFueraPlazo').val(confFinFuera[0] + ":" + confFinFuera[1]);
    $('#txtMinFinFueraPlazo').mask('00:00');

    $('#txtFechaPeriodo').Zebra_DatePicker({
        onSelect: function () {
            actualizarVistaPrevia();
        }
    });
    $('#txtMesPeriodo').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            actualizarVistaPrevia();
        }
    });
    $('#cbSemana').change(function () {
        actualizarVistaPrevia();
    });

    $('#Mesplazo').change(function () {
        actualizarVistaPrevia();
    });
    $('#DiaPlazo').change(function () {
        actualizarVistaPrevia();
    });
    $('#txtMinPlazo').change(function () {
        actualizarVistaPrevia();
    });
    $('#Mesfinplazo').change(function () {
        actualizarVistaPrevia();
    });
    $('#DiaFinPlazo').change(function () {
        actualizarVistaPrevia();
    });
    $('#txtMinFinPlazo').change(function () {
        actualizarVistaPrevia();
    });
    $('#Mesfinfueraplazo').change(function () {
        actualizarVistaPrevia();
    });
    $('#DiaFinFueraPlazo').change(function () {
        actualizarVistaPrevia();
    });
    $('#txtMinFinFueraPlazo').change(function () {
        actualizarVistaPrevia();
    });


    $('#Mesplazo').keyup(function () {
        actualizarVistaPrevia();
    });
    $('#DiaPlazo').keyup(function () {
        actualizarVistaPrevia();
    });
    $('#txtMinPlazo').keyup(function () {
        actualizarVistaPrevia();
    });
    $('#Mesfinplazo').keyup(function () {
        actualizarVistaPrevia();
    });
    $('#DiaFinPlazo').keyup(function () {
        actualizarVistaPrevia();
    });
    $('#txtMinFinPlazo').keyup(function () {
        actualizarVistaPrevia();
    });
    $('#Mesfinfueraplazo').keyup(function () {
        actualizarVistaPrevia();
    });
    $('#DiaFinFueraPlazo').keyup(function () {
        actualizarVistaPrevia();
    });
    $('#txtMinFinFueraPlazo').keyup(function () {
        actualizarVistaPrevia();
    });


    $('#Mesplazo').blur(function () {
        actualizarVistaPrevia();
    });
    $('#DiaPlazo').blur(function () {
        actualizarVistaPrevia();
    });
    $('#txtMinPlazo').blur(function () {
        actualizarVistaPrevia();
    });
    $('#Mesfinplazo').blur(function () {
        actualizarVistaPrevia();
    });
    $('#DiaFinPlazo').blur(function () {
        actualizarVistaPrevia();
    });
    $('#txtMinFinPlazo').blur(function () {
        actualizarVistaPrevia();
    });
    $('#Mesfinfueraplazo').blur(function () {
        actualizarVistaPrevia();
    });
    $('#DiaFinFueraPlazo').blur(function () {
        actualizarVistaPrevia();
    });
    $('#txtMinFinFueraPlazo').blur(function () {
        actualizarVistaPrevia();
    });

    var codigoApp = parseInt($("#hdCodigoApp").val()) || 0;
    var idFormato = parseInt($("#hfIdFormato").val()) || 0;
    //cuando sean formatos del aplicativo GENERACION RER
    if (idFormato > 0 && codigoApp == 1) {        
        $(".fila_modulo").hide();

        //$("#Periodo").prop('disabled', 'disabled');
        //$("#Resolucion").prop('disabled', 'disabled');
        //$("#Horizonte").prop('disabled', 'disabled');
        //$("#IdCabecera").prop('disabled', 'disabled');
    }

    configurarVistaPrevia();
});

function configurarVistaPrevia() {
    $("#txtFechaPeriodo").prop('disabled', 'disabled');
    $("#txtMesPeriodo").prop('disabled', 'disabled');
    $("#txtAnioPeriodo").prop('disabled', 'disabled');
    $("#cbSemana").prop('disabled', 'disabled');

    var valorperiodo = parseInt($('#Periodo').val()) || 0;
    if (valorperiodo == 3 || valorperiodo == 5 || valorperiodo == 4) {//mensual, Semanal x Mes, ANUAL
        $(".mes_config").show();

        $(".dia_config").hide();
        $(".semana_config").hide();
        $('#txtMesPeriodo').removeAttr("disabled");
    } else {
        if (valorperiodo == 2) { //semanal
            $(".semana_config").show();
            $(".dia_config").hide();
            $(".mes_config").hide();

            $('#cbSemana').removeAttr("disabled");
        } else {
            $(".dia_config").show();
            $(".semana_config").hide();
            $(".mes_config").hide();

            $('#txtFechaPeriodo').removeAttr("disabled");
        }
    }
    $("#cbSemana").val($("#semanaActual").val());
    actualizarVistaPrevia();
}

function obtenerDatoHoraMin(hfMinutos) {
    var arrayHM = [];

    var txtHoras = "00" + Math.floor(hfMinutos / 60).toString();
    var horas = txtHoras.substr(txtHoras.length - 2, 2);
    var txtMinutos = "00" + (hfMinutos % 60).toString();
    var minutos = txtMinutos.substr(txtMinutos.length - 2, 2);

    arrayHM.push(horas);
    arrayHM.push(minutos);

    return arrayHM;
}

function grabarFormato() {
    actualizarVistaPrevia();
    var minPlazo = $('#txtMinPlazo').val();
    var mPlazo = parseInt(minPlazo.substr(0, 2)) * 60 + parseInt(minPlazo.substr(3, 2));
    $('#hfMinPlazo').val(mPlazo);

    minPlazo = $('#txtMinFinPlazo').val();
    mPlazo = parseInt(minPlazo.substr(0, 2)) * 60 + parseInt(minPlazo.substr(3, 2));
    $('#hfMinFinPlazo').val(mPlazo);

    minPlazo = $('#txtMinFinFueraPlazo').val();
    mPlazo = parseInt(minPlazo.substr(0, 2)) * 60 + parseInt(minPlazo.substr(3, 2));
    $('#hfMinFinFueraPlazo').val(mPlazo);

    if ($('#chbPlazo:checked').val()) {
        $('#hfCheckPlazo').val(1);
    }
    else {
        $('#hfCheckPlazo').val(0);
    }

    if ($('#chbBlanco:checked').val()) {
        $('#hfCheckBlanco').val(1);
    }
    else {
        $('#hfCheckBlanco').val(0);
    }

    $.ajax({
        type: 'POST',
        url: controlador + "Formatomedicion/GrabarFormato",
        dataType: 'json',
        data: $('#frmFormato').serialize(),
        success: function (evt) {
            if (evt == 1) {
                alert("Grabó correctamente");
                $('#popupFormato').bPopup().close();
                recargar();
            }
            if (evt == -1) {
                alert("Ocurrio un error");
            }
        },
        error: function () {
            alert("Ha ocurrido un error en guardar el formato");
        }
    });

}

function recargar() {
    var codigoApp = parseInt($("#hdCodigoApp").val()) || 0;
    document.location.href = controlador + "formatomedicion/Index?app=" + codigoApp;
}

function actualizarVistaPrevia() {
    var obj = getObjPlazo();
    var strFecha = obj.Fecha;
    var dhoy = moment(convertStringToDate(strFecha, '00:00:00'));
    var dIniPlazo = moment(convertStringToDate(strFecha, '00:00:00'));
    var dFinPlazo = moment(convertStringToDate(strFecha, '00:00:00'));
    var dFueraPlazo = moment(convertStringToDate(strFecha, '00:00:00'));

    dIniPlazo = dIniPlazo.add(obj.Plazinimes, 'M').add(obj.Plazinidia, 'days').add(obj.Plazinimin, 'minutes');
    dFinPlazo = dFinPlazo.add(obj.Plazfinmes, 'M').add(obj.Plazfindia, 'days').add(obj.Plazfinmin, 'minutes');
    dFueraPlazo = dFueraPlazo.add(obj.Plazfuerames, 'M').add(obj.Plazfueradia, 'days').add(obj.Plazfueramin, 'minutes');

    $("#txtFechaPeriodo").text(dhoy.format('DD/MM/YYYY'));
    $("#txtFechaEnPlazo1").text(dIniPlazo.format('DD/MM/YYYY HH:mm'));
    $("#txtFechaEnPlazo2").text(dFinPlazo.format('DD/MM/YYYY HH:mm'));
    $("#txtFechaFueraPlazo1").text(dFinPlazo.format('DD/MM/YYYY HH:mm'));
    $("#txtFechaFueraPlazo2").text(dFueraPlazo.format('DD/MM/YYYY HH:mm'));
}

function getObjPlazo() {
    var obj = {};

    var strFecha = $("#txtFechaPeriodo").val();
    var valorperiodo = parseInt($('#Periodo').val()) || 0;
    if (valorperiodo == 3 || valorperiodo == 5 || valorperiodo == 4) {//mensual, Semanal x Mes
        strFecha = $("#txtMesPeriodo").val();
        strFecha = "01" + "/" + strFecha.substr(0, 2) + "/" + strFecha.substr(3, 4);
    } else {
        if (valorperiodo == 2) {
            strFecha = $("#cbSemana").val();
        } else {
        }
    }
    obj.Fecha = strFecha;

    obj.Plazinimes = parseInt($("#Mesplazo").val()) || 0;
    obj.Plazinidia = parseInt($("#DiaPlazo").val()) || 0;
    obj.Plazinimin = convertirHoraAMinutos($("#txtMinPlazo").val());

    obj.Plazfinmes = parseInt($("#Mesfinplazo").val()) || 0;
    obj.Plazfindia = parseInt($("#DiaFinPlazo").val()) || 0;
    obj.Plazfinmin = convertirHoraAMinutos($("#txtMinFinPlazo").val());

    obj.Plazfuerames = parseInt($("#Mesfinfueraplazo").val()) || 0;
    obj.Plazfueradia = parseInt($("#DiaFinFueraPlazo").val()) || 0;
    obj.Plazfueramin = convertirHoraAMinutos($("#txtMinFinFueraPlazo").val());

    return obj;
}

function convertirHoraAMinutos(minPlazo) {
    var mPlazo = parseInt(minPlazo.substr(0, 2)) * 60 + parseInt(minPlazo.substr(3, 2));
    mPlazo = parseInt(mPlazo) || 0;

    return mPlazo;
}

//convierte 2 cadenas de texto fecha(dd/mm/yyyy) y horas(hh:mm:ss) a tipo Date
function convertStringToDate(fecha, horas) {
    var partsFecha = fecha.split('/');
    if (horas == "") {
        return "";
    }
    var partsHoras = horas.split(':');
    //new Date(yyyy, mm-1, dd, hh, mm, ss);
    return new Date(partsFecha[2], partsFecha[1] - 1, partsFecha[0], partsHoras[0], partsHoras[1], partsHoras[2]);
}