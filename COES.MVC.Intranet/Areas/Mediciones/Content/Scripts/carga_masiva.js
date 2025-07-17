var controlador = siteRoot + 'mediciones/cargamasiva/'

$(function () {
    $('#tab-container').easytabs({
        animate: false
    });

    //////////////////////////////////////////
    $('#txtFechaIniDespachoGen').Zebra_DatePicker({
        onSelect: function () { }
    });
    $('#txtFechaFinDespachoGen').Zebra_DatePicker({
        onSelect: function () { }
    });
    $('#btnNuevoDespachoGen').click(function () {
        setTimeout(function () {
            $('#popupNuevoDespachoGen').bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown'
            });
        }, 50);
    });
    $('#btnDespachoGen').click(function () {
        procesarGeneracionDespacho();
    });

    //////////////////////////////////////////
    $('#txtFechaIni').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () { }
    });
    $('#txtFechaFin').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () { }
    });

    $('#btnNuevoMD').click(function () {
        nuevoMD();
    });

    $('#btnProcesarMD').click(function () {
        procesarMD();
    });

    //////////////////////////////////////////
    $('#txtFechaIniFp').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () { }
    });
    $('#txtFechaFinFp').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () { }
    });

    $('#btnNuevoFp').click(function () {
        nuevoFp();
    });

    $('#btnProcesarFp').click(function () {
        procesarFp();
    });

    //////////////////////////////////////////
    $('#txtFechaIniProdGen').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () { }
    });
    $('#txtFechaFinProdGen').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () { }
    });

    $('#btnNuevoProdGen').click(function () {
        nuevoProdGen();
    });

    $('#btnProcesarProdGen').click(function () {
        procesarProdGen();
    });

    //////////////////////////////////////////
    $('#txtFechaIniPePinst').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () { }
    });
    $('#txtFechaFinPePinst').Zebra_DatePicker({
        format: 'm Y',
        onSelect: function () { }
    });

    $('#btnNuevoPePinst').click(function () {
        nuevoPePinst();
    });

    $('#btnProcesarPePinst').click(function () {
        procesarPePinst();
    });
});

////////////////////////////////////////////////////////////////////////////////////////////////
/// Producción de los Despachos de Generació
function procesarGeneracionDespacho() {
    var fechaIni = $("#txtFechaIniDespachoGen").val();
    var fechaFin = $("#txtFechaFinDespachoGen").val();

    if (confirm('¿Está seguro que desea procesar "Producción de los Despachos de Generación" para el periodo seleccionado?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarDespachoDiario',
            data: {
                fechaIni: fechaIni,
                fechafin: fechaFin,
            },
            cache: false,
            success: function (data) {
                if (data.Resultado == 1) {
                    alert("Se procesó correctamente.");
                    $('#popupNuevoDespachoGen').bPopup().close();
                } else {
                    alert('Ha ocurrido un error: ' + data.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////
/// Generar Maxima Demanda
function nuevoMD() {
    setTimeout(function () {
        $('#popupGenerarMD').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function procesarMD() {
    var fechaIni = $("#txtFechaIni").val();
    var fechaFin = $("#txtFechaFin").val();

    var mesIni = fechaIni;
    var mesFin = fechaFin;

    if (confirm('¿Está seguro que desea procesar la Máxima Demanda para el periodo seleccionado?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarMaximaDemanda',
            data: {
                mesIni: mesIni,
                mesFin: mesFin,
            },
            cache: false,
            success: function (data) {
                if (data.Resultado == 1) {
                    alert("Se generó correctamente la Máxima Demanda");
                    $('#popupGenerarMD').bPopup().close();
                } else {
                    alert('Ha ocurrido un error: ' + data.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////
/// Factor Planta
function nuevoFp() {
    setTimeout(function () {
        $('#popupFp').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function procesarFp() {
    var fechaIni = $("#txtFechaIniFp").val();
    var fechaFin = $("#txtFechaFinFp").val();

    var mesIni = fechaIni;
    var mesFin = fechaFin;

    if (confirm('¿Está seguro que desea procesar el Factor Planta en el periodo seleccionado?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarFactorPlanta',
            data: {
                mesIni: mesIni,
                mesFin: mesFin,
            },
            cache: false,
            success: function (data) {
                if (data.Resultado == 1) {
                    alert("Se generó correctamente la información");
                    $('#popupFp').bPopup().close();
                } else {
                    alert('Ha ocurrido un error: ' + data.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////
/// Generación de las unidades
function nuevoProdGen() {
    setTimeout(function () {
        $('#popupProdGen').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function procesarProdGen() {
    var fechaIni = $("#txtFechaIniProdGen").val();
    var fechaFin = $("#txtFechaFinProdGen").val();

    var mesIni = fechaIni;
    var mesFin = fechaFin;

    if (confirm('¿Está seguro que desea procesar la Producción de las unidades en el periodo seleccionado?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarProduccionGeneracion',
            data: {
                mesIni: mesIni,
                mesFin: mesFin,
            },
            cache: false,
            success: function (data) {
                if (data.Resultado == 1) {
                    alert("Se generó correctamente la información");
                    $('#popupProdGen').bPopup().close();
                } else {
                    alert('Ha ocurrido un error: ' + data.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////
/// Potencia efectiva e instalada
function nuevoPePinst() {
    setTimeout(function () {
        $('#popupPePinst').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown'
        });
    }, 50);
}

function procesarPePinst() {
    var fechaIni = $("#txtFechaIniPePinst").val();
    var fechaFin = $("#txtFechaFinPePinst").val();

    var mesIni = fechaIni;
    var mesFin = fechaFin;

    if (confirm('¿Está seguro que desea procesar la Potencia efectiva e instalada en el periodo seleccionado?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GuardarPotPEInst',
            data: {
                mesIni: mesIni,
                mesFin: mesFin,
            },
            cache: false,
            success: function (data) {
                if (data.Resultado == 1) {
                    alert("Se generó correctamente la información");
                    $('#popupPePinst').bPopup().close();
                } else {
                    alert('Ha ocurrido un error: ' + data.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error');
            }
        });
    }
}

