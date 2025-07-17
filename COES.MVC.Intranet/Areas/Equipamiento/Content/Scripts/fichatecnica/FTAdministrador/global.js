var IMG_EDITAR_FICHA = `<img src="${siteRoot}Content/Images/btn-properties.png" title="Ver Ficha Técnica o Completar" width="19" height="19"/>`;
var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" alt="Editar Envío" title="Editar Comentario" width="19" height="19" style="">`;
var IMG_VER = `<img src="${siteRoot}Content/Images/btn-open.png" alt="Ver Envío"  title="Ver Comentario"width="19" height="19" style="">`;

var HEIGHT_FORMULARIO = 600;
var WIDTH_FORMULARIO = 1000;

var OPCION_GLOBAL_EDITAR = false;
var OPCION_IMPORTACION = false;

var CELDA_REV_EDITAR = 1;
var CELDA_REV_VER = 2;

const COLOR_BLOQUEADO = "#f9ede4";
const COLOR_NUMERAL = "#92CDDC";
const COLOR_CAB_REQUISITO = "#4682B4";
const COLOR_BORDE = "#DDDDDD";

const INTRANET = 1;
const EXTRANET = 2;

const TIPO_FORMATO_CONEXINTMODIF = 1;
const TIPO_FORMATO_DARBAJA = 3;

const ESTADO_SOLICITADO = 1;
const ESTADO_APROBADO = 3;
const ESTADO_DESAPROBADO = 4;
const ESTADO_OBSERVADO = 6;
const ESTADO_SUBSANADO = 7;
const ESTADO_CANCELADO = 8;
const ESTADO_APROBADO_PARCIAL = 10;

var FECHA_SISTEMA;

const ETAPA_CONEXION = 1;
const ETAPA_INTEGRACION = 2;
const ETAPA_OPERACION_COMERCIAL = 3;
const ETAPA_MODIFICACION = 4;

const OpcionConforme = "OK";
const OpcionNoSubsanado = "NS";
const OpcionSubsanado = "S";
const OpcionObservado = "O";
const OpcionDenegado = "D";

const ColorRojo = "#F90505";
const ColorAzul = "#0544F9";
const ColorVerde = "#2BA205";
const ColorNaranja = "#FC9D02";
const ColorVioleta = "#1502FC";

var numColumnasResizables = 4; //solo las 3 primeras son resizables

const TODOS = 1;
const BASTA1 = 2;

var listaDataRevisionFT = [];
var listaDataRevisionArea = [];
var LISTA_REQUISITO_ARCHIVO = [];
var TIPO_ARCHIVO_REVISION_OBSCOES = "REV_OC";
var TIPO_ARCHIVO_REVISION_RPTAAGENTE = "REV_RA";
var TIPO_ARCHIVO_REVISION_RPTACOES = "REV_RC";

var listaArchivoRevTemp = [];
var TIPO_ARCHIVO_AREA_REVISION = "REV_AREA";
const COLUMNA_REV_SOLICITADO = 2;
const COLUMNA_REV_SUBSANADO = 4;

var ULTIMO_MENSAJE_AUTOGUARDADO = '';

var OBJ_VALIDACION_ENVIO = {};

function validarDatosFiltroDerivacion(datos) {
    var msj = "";

    var strFechaHoy = diaActualSistema(); // en dd/mm/yyyy

    if (esFechaValida(datos.fecMaxRpta)) {
        if (convertirFecha(strFechaHoy) > convertirFecha(datos.fecMaxRpta)) {
            msj += "<p>Debe escoger una fecha máxima de respuesta correcta.	La Fecha Máxima de Respuesta esta fuera del rango permitido (" + strFechaHoy + " para adelante).</p>";
        }
    } else {
        msj += "<p>Debe ingresar fecha máxima de respuesta correcta (en formato dd//mm/yyyy).</p>";
    }

    return msj;
}

function validarDirecccionCorreo(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}


const DATE_REGEX = /^(0[1-9]|[1-2]\d|3[01])(\/)(0[1-9]|1[012])\2(\d{4})$/

function esFechaValida(strFecha) {  //para tipo dd/mm/yyyy

    /* Comprobar formato dd/mm/yyyy, que el no sea mayor de 12 y los días mayores de 31 */
    if (!strFecha.match(DATE_REGEX)) {
        return false
    }

    /* Comprobar los días del mes */
    const dia = parseInt(strFecha.split('/')[0])
    const mes = parseInt(strFecha.split('/')[1])
    const anio = parseInt(strFecha.split('/')[2])
    const diasPorMes = new Date(anio, mes, 0).getDate()
    if (dia > diasPorMes) {
        return false
    }


    return true
}

function AddZero(num) {
    return (num >= 0 && num < 10) ? "0" + num : num + "";
}

function diaActualSistema() { //devuelve strFecha en formato dd/mm/yyyy
    var strDateTime = "";
    if (FECHA_SISTEMA == "") {
        var now = new Date();
        strDateTime = [[AddZero(now.getDate()), AddZero(now.getMonth() + 1), now.getFullYear()].join("/")].join(" ");
    } else {
        strDateTime = FECHA_SISTEMA;
    }

    return strDateTime;
}

function convertirFecha(fecha) {  //para dd/mm/yyyy
    var arrayFecha = fecha.split('/');
    var dia = arrayFecha[0];
    var mes = arrayFecha[1];
    var anio = arrayFecha[2];

    var salida = anio + mes + dia;
    return salida;
}

function esString(valor) {
    if (typeof valor == "string")
        return true;
    else
        return false;
}

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function mostrarMensaje(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

function _irAFooterPantalla() {
    setTimeout(function () {
        $('html, body').scrollTop($("#container").offset().top);
    }, 50);
}