var controlador = siteRoot + 'IEOD/Configuracion/';

$(function () {
    $('#btnConsultar').on('click', function () {
        listarFuenteDatos();
    });

    listarFuenteDatos();
});

///////////////////////////
/// Consulta
///////////////////////////
function listarFuenteDatos() {
    $.ajax({
        type: 'POST',
        url: controlador + 'FuenteDatosPlazoLista',
        data: {
        },
        success: function (result) {
            $('#listado').css("width", $('#mainLayout').width() + "px");
            $('#listado').html(result);

            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 'ft',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function (err) {
            alert('Ha ocurrido un error.' + err.ResponseText);
        }
    });
}

///////////////////////////
/// Edición
///////////////////////////
function inicializarFormPlazo() {
    $("#cbPeriodo").val($("#hfPeriodo").val());
    setMinutos($("#hfIniMin").val(), 'txtIniMin');
    setMinutos($("#hfFinMin").val(), 'txtFinMin');
    setMinutos($("#hfFueraMin").val(), 'txtFueraMin');

    actualizarVistaPrevia();
}

function editarPlazo(idPlazo) {
    $.ajax({
        type: 'POST',
        url: controlador + "FuenteDatosPlazoFormulario",
        data: {
            idPlazo: idPlazo
        },
        success: function (evt) {
            $('#editarPlazo').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupEditarPlazo').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function (err) {
            alert('Ha ocurrido un error.' + err.ResponseText);
        }
    });
}

function actualizarPlazo() {
    if (confirm('¿Está seguro que desea actualizar el plazo?')) {
        var msj = validarPlazo();

        if (msj == "") {
            var entity = getObjPlazo();
            var obj = JSON.stringify(entity);

            $.ajax({
                type: 'POST',
                dataType: 'json',
                traditional: true,
                url: controlador + 'FuenteDatosPlazoGuardar',
                data: {
                    strJson: obj
                },
                cache: false,
                success: function (result) {
                    if (result.Resultado == '-1') {
                        alert('Ha ocurrido un error:' + result.Mensaje);
                    } else {
                        alert("Se actualizó correctamente el Plazo");
                        listarFuenteDatos();
                        cerrarPopupPlazo();
                    }
                },
                error: function (err) {
                    alert('Ha ocurrido un error.');
                }
            });
        } else {
            alert(msj);
        }
    }
}

function cerrarPopupPlazo() {
    $('#popupEditarPlazo').bPopup().close();
}

function getObjPlazo() {
    var obj = {};

    obj.Plazcodi = $("#hfPlazcodi").val()
    obj.Plazperiodo = $("#cbPeriodo").val() || 0;
    obj.Plazinidia = parseInt($("#iniDia").val()) || 0;
    obj.Plazinimin = convertirHoraAMinutos($("#txtIniMin").val());
    obj.Plazfindia = parseInt($("#finDia").val()) || 0;
    obj.Plazfinmin = convertirHoraAMinutos($("#txtFinMin").val());
    obj.Plazfueradia = parseInt($("#fueraDia").val()) || 0;
    obj.Plazfueramin = convertirHoraAMinutos($("#txtFueraMin").val());

    return obj;
}

function validarPlazo() {
    var obj = getObjPlazo();
    var dhoy = moment(convertStringToDate($("#hfFechaPeriodo").val(), '00:00:00'));
    var dIniPlazo = moment(convertStringToDate($("#hfFechaPeriodo").val(), '00:00:00'));
    var dFinPlazo = moment(convertStringToDate($("#hfFechaPeriodo").val(), '00:00:00'));
    var dFueraPlazo = moment(convertStringToDate($("#hfFechaPeriodo").val(), '00:00:00'));

    dIniPlazo = dIniPlazo.add(obj.Plazinidia, 'days').add(obj.Plazinimin, 'minutes');
    dFinPlazo = dFinPlazo.add(obj.Plazfindia, 'days').add(obj.Plazfinmin, 'minutes');
    dFueraPlazo = dFueraPlazo.add(obj.Plazfueradia, 'days').add(obj.Plazfueramin, 'minutes');

    var mensaje = "";

    if (obj.Plazperiodo == '0') {
        mensaje = mensaje + "Seleccione periodo." + "\n";
    }

    if (dFinPlazo.isBefore(dIniPlazo)) {
        mensaje = mensaje + "Fin de Plazo no es correcto" + "\n";
    }

    if (dFueraPlazo.isBefore(dFinPlazo)) {
        mensaje = mensaje + "Fuera de Plazo no es correcto" + "\n";
    }

    return mensaje;
}

function setMinutos(strMinutos, idMinutos) {
    $('#' + idMinutos).val(convertirMinutosAHora(strMinutos));
    $('#' + idMinutos).mask('00:00');
}

function convertirMinutosAHora(strMinutos) {
    var inputMinutos = parseInt(strMinutos) || 0;
    var txtHoras = "00" + Math.floor(inputMinutos / 60).toString();
    var horas = txtHoras.substr(txtHoras.length - 2, 2);
    var txtMinutos = "00" + (inputMinutos % 60).toString();
    var minutos = txtMinutos.substr(txtMinutos.length - 2, 2);

    return horas + ":" + minutos;
}

function convertirHoraAMinutos(minPlazo) {
    var mPlazo = parseInt(minPlazo.substr(0, 2)) * 60 + parseInt(minPlazo.substr(3, 2));
    mPlazo = parseInt(mPlazo) || 0;

    return mPlazo;
}

function actualizarVistaPrevia() {
    var obj = getObjPlazo();
    var dhoy = moment(convertStringToDate($("#hfFechaPeriodo").val(), '00:00:00'));
    var dIniPlazo = moment(convertStringToDate($("#hfFechaPeriodo").val(), '00:00:00'));
    var dFinPlazo = moment(convertStringToDate($("#hfFechaPeriodo").val(), '00:00:00'));
    var dFueraPlazo = moment(convertStringToDate($("#hfFechaPeriodo").val(), '00:00:00'));

    dIniPlazo = dIniPlazo.add(obj.Plazinidia, 'days').add(obj.Plazinimin, 'minutes');
    dFinPlazo = dFinPlazo.add(obj.Plazfindia, 'days').add(obj.Plazfinmin, 'minutes');
    dFueraPlazo = dFueraPlazo.add(obj.Plazfueradia, 'days').add(obj.Plazfueramin, 'minutes');

    $("#txtFechaPeriodo").text(dhoy.format('DD/MM/YYYY'));
    $("#txtFechaEnPlazo1").text(dIniPlazo.format('DD/MM/YYYY HH:mm'));
    $("#txtFechaEnPlazo2").text(dFinPlazo.format('DD/MM/YYYY HH:mm'));
    $("#txtFechaFueraPlazo1").text(dFinPlazo.format('DD/MM/YYYY HH:mm'));
    $("#txtFechaFueraPlazo2").text(dFueraPlazo.format('DD/MM/YYYY HH:mm'));
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