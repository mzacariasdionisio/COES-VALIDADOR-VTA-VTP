const TIPO_COMB_LIQUIDO = 1;
const TIPO_COMB_CARBON = 2;

const MONEDA_SOLES = "S";
const MONEDA_DOLAR = "D";
const MONEDA_LITRO = "l";
const MONEDA_KILO = "kg";

const TIPO_DETALLE_FACTURA = 1;
const TIPO_DETALLE_DEMURRAGE = 2;
const TIPO_DETALLE_MERMA = 3;
const TIPO_VOLUMEN_COMBUSTIBLE = 4;

const ESTADO_SOLICITUD = 1;
const ESTADO_CANCELADO = 8;
const ESTADO_OBSERVADO = 6;
const ESTADO_APROBADO = 3;
const ESTADO_LEVANTAMIENTO_OBS = 7;
const ESTADO_DESAPROBADO = 4;

const MENSAJE_FALTA_DATOS = 'Debe ingresar la información, favor de completar los datos.';
const MENSAJE_VENTANA_SECCION = 'Clic aquí para ingresar los valores de la sección.';

////////////////////////////////////////////////////////////////////////////////////////////////////////////
function obtenerSeccionXcnp(listaSeccion, concepto) {
    for (var i = 0; i < listaSeccion.length; i++) {
        var seccion = listaSeccion[i];
        if (seccion.Seccion.Ccombcodi == concepto)
            return seccion;
    }

    return null;
}

function obtenerArchivo(listaSeccionDocumento, concepto, pos) {

    var seccion = obtenerSeccionXcnp(listaSeccionDocumento, concepto);

    for (var i = 0; i < seccion.ListaArchivo.length; i++) {
        if (i == pos)
            return seccion.ListaArchivo[i];
    }

    return null;
}

function obtenerItemDato(concepto, codigoItem) {
    var seccion = obtenerSeccionXcnp(MODELO_WEB.ListaSeccion, concepto);

    for (var i = 0; i < seccion.ListaItem.length; i++) {
        var item = seccion.ListaItem[i]
        if (item.Ccombcodi == codigoItem)
            return item.ItemDato;
    }

    return null;
}

function obtenerListaFacturaXItem(concepto, codigoItem) {
    var itemDato = obtenerItemDato(concepto, codigoItem);
    if (itemDato != null) {
        return itemDato.ListaDetalle;
    }

    return null;
}

//////
function objDefaultFactura(monedaSeccion) {
    var tipoMoneda = monedaSeccion;

    var obj = {
        Cbdetcompago: '',
        Cbdettipo: TIPO_DETALLE_FACTURA,
        CbdetfechaemisionDesc: '',
        Cbdettipocambio: '',
        Cbdetmoneda: tipoMoneda,
        Cbdetvolumen: '',
        Cbdetcosto: '',
        Cbdetcosto2: '',
        Cbdetimpuesto: '',
    };

    return obj;
}

function objDefaultVolCombustible() {

    var obj = {
        Cbdettipo: TIPO_VOLUMEN_COMBUSTIBLE,
        CbdetfechaemisionDesc: MODELO_WEB.RangoFinValidoRecepcion,
        Cbdetvolumen: '',
        MensajeValidacion: ''
    };

    return obj;
}

function objDefaultDemurrage(monedaSeccion) {
    var tipoMoneda = monedaSeccion;

    var obj = {
        Cbdetcompago: '',
        Cbdettipo: TIPO_DETALLE_DEMURRAGE,
        CbdetfechaemisionDesc: '',
        Cbdettipocambio: '',
        Cbdetmoneda: tipoMoneda,
        Cbdetvolumen: '',
        Cbdetcosto: '',
        Cbdetcosto2: '',
        Cbdetimpuesto: '',
    };

    return obj;
}

function objDefaultMerma(monedaSeccion) {
    var tipoMoneda = monedaSeccion;

    var obj = {
        Cbdetcompago: '',
        Cbdettipo: TIPO_DETALLE_MERMA,
        CbdetfechaemisionDesc: '',
        Cbdettipocambio: '',
        Cbdetmoneda: tipoMoneda,
        Cbdetvolumen: '',
        Cbdetcosto: '',
        Cbdetcosto2: '',
        Cbdetimpuesto: '',
    };

    return obj;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Util
////////////////////////////////////////////////////////////////////////////////////////////////////////////
//function utilizada en /Content/scripts/formato/hoja/global.js
function formatFloat(num, casasDec, sepDecimal, sepMilhar) {
    //si la cadena es vacia no formatear a numero
    if (!esValidoString(num + ''))
        return '';

    if (num == 0) {
        var cerosDer = '';
        while (cerosDer.length < casasDec)
            cerosDer = '0' + cerosDer;

        return "0" + sepDecimal + cerosDer;
    }

    if (num < 0) {
        num = -num;
        sinal = -1;
    } else
        sinal = 1;
    var resposta = "";
    var part = "";

    if (num != Math.floor(num)) // decimal values present
    {
        part = Math.round((num - Math.floor(num)) * Math.pow(10, casasDec)).toString(); // transforms decimal part into integer (rounded)
        while (part.length < casasDec)
            part = '0' + part;
        if (casasDec > 0) {
            resposta = sepDecimal + part;
            num = Math.floor(num);
        } else
            num = Math.round(num);
    } // end of decimal part
    else {
        while (part.length < casasDec)
            part = '0' + part;
        if (casasDec > 0) {
            resposta = sepDecimal + part;
        }
    }

    var numBefWhile = num; //A veces no aparece el número cero (0) antes del punto decimal
    while (num > 0) // integer part
    {
        part = (num - Math.floor(num / 1000) * 1000).toString(); // part = three less significant digits
        num = Math.floor(num / 1000);
        if (num > 0)
            while (part.length < 3) // 123.023.123  if sepMilhar = '.'
                part = '0' + part; // 023
        resposta = part + resposta;
        if (num > 0)
            resposta = sepMilhar + resposta;
    }
    if (sinal < 0)
        resposta = '-' + ((numBefWhile == 0) ? '0' : '') + resposta;
    else {
        if (numBefWhile == 0)
            resposta = '0' + resposta;
    }
    return resposta;
}

function validaTexto(evt, id) {
    try {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode == 59 || charCode == 92) { return false; }
        return true;
    } catch (w) { alert(w); }
}

function validaNum(evt, id) {
    try {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode == 86) { return false; }
        if (charCode == 46) {

            var txt = document.getElementById(id).value;
            txt += ".";
            var count = txt.split(".").length - 1;

            if ((txt.indexOf(".") > 0) && count <= 1) { return true; }
        }
        if (charCode > 31 && (charCode < 48 || charCode > 57)) { return false; }
        return true;
    } catch (w) { alert(w); }
}

function esValidoString(valor) {
    return valor != null && valor != '' && valor.trim() != '';
}

//////////////////////////////////////////////////////////////////////////////
function mostrarInformativo(alerta) {
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').addClass("action-message");
    $('#mensajeEvento').html(alerta);
    $('#mensajeEvento').css("display", "block");
}

function mostrarAlerta(alerta) {
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').addClass("action-alert");
    $('#mensajeEvento').html(alerta);
    $('#mensajeEvento').css("display", "block");
}

function mostrarError(alerta) {
    $('#mensajeEvento').removeClass("action-message");
    $('#mensajeEvento').removeClass("action-alert");
    $('#mensajeEvento').removeClass("action-exito");
    $('#mensajeEvento').addClass("action-error");
    $('#mensajeEvento').html(alerta);
    $('#mensajeEvento').css("display", "block");
}


