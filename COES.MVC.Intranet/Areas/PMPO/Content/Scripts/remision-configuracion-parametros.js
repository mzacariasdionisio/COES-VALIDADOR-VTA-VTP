var controlador = siteRoot + 'PMPO/ConfiguracionParametros/';


$(function () {
    $("#frmFormato").validate({
        rules: {
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
        },
        messages: {
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
        }
    });

    $("#btnGuardar").click(function () {
        grabarConfiguracion();
    });

    $('#Periodo').change(function () {
        configurarVistaPrevia();
    });

    $('#txtMesPeriodo').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () {
            actualizarVistaPrevia();
        }
    });

    //cajas de texto
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

    consultarConfiguracion();
});

function consultarConfiguracion() {
    $("#mainLayout").hide();

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerConfParametroPlazo",
        dataType: 'json',
        data: {
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#Mesplazo").val(evt.Mesplazo);
                $("#Mesfinplazo").val(evt.Mesfinplazo);
                $("#Mesfinfueraplazo").val(evt.Mesfinfueraplazo);

                $("#DiaPlazo").val(evt.DiaPlazo);
                $("#DiaFinPlazo").val(evt.DiaFinPlazo);
                $("#DiaFinFueraPlazo").val(evt.DiaFinFueraPlazo);

                var confIni = obtenerDatoHoraMin(evt.MinutoPlazo);
                $('#txtMinPlazo').val(confIni[0] + ":" + confIni[1]);
                $('#txtMinPlazo').mask('00:00');

                var confFin = obtenerDatoHoraMin(evt.MinutoFinPlazo);
                $('#txtMinFinPlazo').val(confFin[0] + ":" + confFin[1]);
                $('#txtMinFinPlazo').mask('00:00');

                var confFinFuera = obtenerDatoHoraMin(evt.MinutoFinFueraPlazo);
                $('#txtMinFinFueraPlazo').val(confFinFuera[0] + ":" + confFinFuera[1]);
                $('#txtMinFinFueraPlazo').mask('00:00');

                actualizarVistaPrevia();

                $("#mainLayout").show();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function grabarConfiguracion() {
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

    $.ajax({
        type: 'POST',
        url: controlador + "GuardarConfParametroPlazo",
        dataType: 'json',
        data: $('#frmFormato').serialize(),
        success: function (evt) {
            if (evt.Resultado != "-1") {
                alert("Se actualizó correctamente");
                consultarConfiguracion();
            } else {
                alert("Ocurrió un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error en guardar el formato");
        }
    });
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

    var strFecha = $("#txtMesPeriodo").val();
    strFecha = "01" + "/" + strFecha.substr(0, 2) + "/" + strFecha.substr(3, 4);

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