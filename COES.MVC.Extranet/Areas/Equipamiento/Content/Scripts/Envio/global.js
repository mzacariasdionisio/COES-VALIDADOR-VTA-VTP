//Variables globales del envio
var OPCION_GLOBAL_EDITAR = false;
var OPCION_IMPORTACION = false;
var HEIGHT_MINIMO = 500;
var HEIGHT_FORMULARIO = 600;
var WIDTH_FORMULARIO = 1000;

var CELDA_REV_EDITAR = 1;
var CELDA_REV_VER = 2;
const VER = 4;
const OPCION_FORMULARIO_ACTUAL_BD = 10;
const OPCION_FORMULARIO_LIMPIAR = 11;
const OPCION_FORMULARIO_CAMBIO_ENTRE_VERSION = 12;

const COLOR_BLOQUEADO = "#f9ede4";
const COLOR_NUMERAL = "#92CDDC";
const COLOR_CAB_REQUISITO = "#4682B4";
const COLOR_BORDE = "#DDDDDD";

const INTRANET = 1;
const EXTRANET = 2;

const TIPO_FORMATO_DARBAJA = 3;

const ESTADO_SOLICITADO = 1;
const ESTADO_APROBADO = 3;
const ESTADO_DESAPROBADO = 4;
const ESTADO_OBSERVADO = 6;
const ESTADO_SUBSANADO = 7;
const ESTADO_CANCELADO = 8;
const ESTADO_APROBADO_PARCIAL = 10;

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

//Autoguarado
const ACCION_GUARDADO_SISTEMA = "S";
const ACCION_GUARDADO_MANUAL = "M";

var FLAG_AUTOGUARDADO_HABILITADO = false;
var LISTA_ID_INTERVALO_AUTOGUARDADO = [];
var FLAG_PAUSAR_AUTOGUARDADO = false;
var NUM_AUTOGUARDADO = 0;

var VERSION_TEMPORAL_CREADO_OK = false;
var EXISTE_CAMBIO_SIN_GUARDAR = false;
var TIPO_COOKIE_GUARDAR_SIN_CONEXION = 'G_NO_C';
var TIPO_COOKIE_GUARDAR_CON_CONEXION = 'G_SI_C';
var TIPO_COOKIE_ENVIAR_SIN_CONEXION = 'E_NO_C';
var TIPO_COOKIE_ENVIAR_CON_CONEXION = 'E_SI_C';
var ULTIMO_MENSAJE_AUTOGUARDADO = '';
var HAY_AUTOGUARDADO_Y_USO_INFO = "Usó una información autoguardada";
var HAY_AUTOGUARDADO_Y_NO_USO_INFO = "No usó una información autoguardada (reinició información)";

var CODIGO_ENVIO = 0;
var CODIGO_VERSION = 0;
var LISTA_VERSIONES = [];
var LISTA_AUTOGUARDADO = [];
var LISTA_ERRORES = [];

var LISTA_REQUISITO_ARCHIVO = [];
var listaDataRevisionFT = [];
var listaArchivoRevTemp = [];
var TIPO_ARCHIVO_REVISION_OBSCOES = "REV_OC";
var TIPO_ARCHIVO_REVISION_RPTAAGENTE = "REV_RA";
var TIPO_ARCHIVO_REVISION_RPTACOES = "REV_RC";

//imagenes
var IMG_EDITAR_FICHA = `<img src="${siteRoot}Content/Images/btn-properties.png" title="Ingreso de datos" width="19" height="19"/>`;
var IMG_GRABAR = `<img src="${siteRoot}Content/Images/grabar.png" title="" width="19" height="19"/>`;
var IMG_EDITAR = `<img src="${siteRoot}Content/Images/btn-edit.png" alt="Editar Envío" title="Editar Comentario" width="19" height="19" style="">`;
var IMG_VER = `<img src="${siteRoot}Content/Images/btn-open.png" alt="Ver Envío"  title="Ver Comentario"width="19" height="19" style="">`;
var IMG_CANCELAR = '<img src="' + siteRoot + 'Content/Images/btn-cancel.png" alt="Quitar equipo" width="15" height="19" style="">';


function obtenerFechaHoraActual() {
    var fechahora;

    let date = new Date();
    let fecha = String(date.getDate()).padStart(2, '0') + '/' + String(date.getMonth() + 1).padStart(2, '0') + '/' + date.getFullYear();
    let hora = String(date.getHours()).padStart(2, '0') + ":" + String(date.getMinutes()).padStart(2, '0') + ":" + String(date.getSeconds()).padStart(2, '0');

    fechahora = fecha + " " + hora;

    return fechahora;
}

////////////////////////////////////////////////
// Util
////////////////////////////////////////////////

function _limpiarBarraMensaje_(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
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

function _mostrarMensaje_(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
}

function _limpiarBarraMensaje_(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

function _irAFooterPantalla() {
    setTimeout(function () {
        $('html, body').scrollTop($("#contentFooter").offset().top);
    }, 50);
}