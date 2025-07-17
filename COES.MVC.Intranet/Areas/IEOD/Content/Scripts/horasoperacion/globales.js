var controlador = siteRoot + 'IEOD/HorasOperacion/';

var GLOBAL_HO = null;
var TIENE_PERMISO_ADMIN = false;

var LISTA_TIPO_OPERACION;
var LISTA_MODO_OPERACION;
var LISTA_CENTRALES;

//constantes
var MODO_GRUPORESERVAFRIA = 1;
var FLAG_GRUPORESERVAFRIA_TO_REGISTRAR_UNIDADES = 1;

var FLAG_MODO_OP_ESPECIAL = "S";
var FLAG_HO_ESPECIAL = 1;
var FLAG_HO_NO_ESPECIAL = 0;
var TIPO_HO_MODO = 100;
var TIPO_HO_UNIDAD = 200;
var TIPO_HO_MODO_BD = 1;
var TIPO_HO_UNIDAD_BD = 2;

var LISTA_CALIFICACION = [];
var SUBCAUSACODI_DEFAULT = -1;
var SUBCAUSACODI_PRUEBA_ALEATORIA_PR25 = 114;
var SUBCAUSACODI_POR_PRUEBAS = 106;

//gráfico y dimensiones de listado
var segmento = 0;

var TIPO_LINEA_ACTUAL = 1;
var TIPO_LINEA_COSTO_INCREMENTAL = 2;
var TIPO_LINEA_FLECHA = 3;
var VALOR_CHECK_HORAMIN_TIEMPOREAL = 1;
var VALOR_CHECK_HORAMIN_INPUT = 2;
var FLAG_MODO_OPERO = 1;

var VALOR_ALERTA_EMS_SI = 1;
var VALOR_ALERTA_EMS_NO = 0;
var VALOR_ALERTA_SCADA_SI = 1;
var VALOR_ALERTA_SCADA_NO = 0;
var VALOR_ALERTA_INTERVENCION_SI = 1;
var VALOR_ALERTA_INTERVENCION_NO = 0;
var VALOR_ALERTA_COSTO_INCREMENTAL_SI = 1;
var VALOR_ALERTA_COSTO_INCREMENTAL_NO = 0;

var TITLE_ALERTA_SCADA = 'Alerta Scada';
var TITLE_ALERTA_EMS = 'Alerta Ems';
var TITLE_ALERTA_CI = 'Alerta Costo Incremental';
var TITLE_ALERTA_INTERVENCIONES = 'Alerta Intervención';
var TITLE_FLECHA_CI = 'Ordenamiento de costo Incremental del más barato al más caro';
var TITLE_CI = 'Costo incremental en el tramo ';

var ANCHO_LISTADO_EMS = 1200; //default para laptop
var ANCHO_VISIBLE_GRAFICO_DEFAULT = 1000;//default para laptop
var ANCHO_VISIBLE_GRAFICO = 1200;//default para laptop

var ALTO_VISIBLE_LISTADO_EMS_DEFAULT = 500; //default para laptop
var ALTO_VISIBLE_LISTADO_EMS = 520;

var TAMANIO_TABLA_GRAFICO = 1838;
var TAMANIO_TABLA_EMS_SCADA = 1500;

const CONGESTION_COLOR = "#000000";
const CENTRAL_COLOR_DEFAULT = '#C6F5FB';

//acciones sobre formulario
var APP_OPCION = -1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ELIMINAR = 4;
var OPCION_VER = 5;

var OPCION_DIV = -1;
var NUEVO_DIV_INICIAL = 1;
var NUEVO_DIV_AGREGAR = 2;

var APP_GRAFICO_OPCION = 0;
var APP_GRAFICO_MUESTRA_TODO_CENTRAL_GRUPOMODO = 1;

var APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;

// Se utiliza para registrar una nueva hora de operación desde las tablas EMS y Scada
var CENTRAL_NUEVA = "";
var ORIGEN_DETALLE = "";

// Crea un objeto para almacenar los innerHTMLs iniciales de los divs
var divsInitialState = {};
var LISTA_HORAOP_FORM_EDITABLE = [];
var LISTA_HORAOP_ORIGINAL = 0;

var PLANTILLA_DESC_PRUEBAS_ALEATORIAS = "A las HH:mm h, alcanzó su máxima generación";

var LISTA_HORAOP_DTO_EDITABLE = []; //tiene en memoria los objetos de horas de operación que estan siendo editados de forma múltiple

var MODO_NUEVO = null;
var SELECCION_POR_MODO = false;
var SELECCION_MANUAL = true;

var TIPO_VENTANA_YUPANA = 100;
var TIPO_VENTANA_EMS = 101;
var TIPO_VENTANA_SCADA = 102;

//desglose
var LISTA_TIPO_DESGLOSE = [];
var DESG_CODI_POT_FIJA = 3;
var DESG_CODI_POT_MAX = 4;
var DESG_CODI_POT_MIN = 5;
var DESG_CODI_PLENA_CARGA = 6;

const DESGLOSE_TIPO_PLENA_CARGA = 6;
const DESGLOSE_TIPO_POTENCIA_FIJA = 3;
const DESGLOSE_TIPO_POTENCIA_MAXIMA = 4;
const DESGLOSE_TIPO_POTENCIA_MINIMA = 5;

const DESGLOSE_COLOR_PLENA_CARGA = "#0070C0";
const DESGLOSE_COLOR_POTENCIA_FIJA = "#EB701D";
const DESGLOSE_COLOR_POTENCIA_MAXIMA = "#A365D1";
const DESGLOSE_COLOR_POTENCIA_MINIMA = "#FFE59B";

var OBJ_REFERENCIA = {
    id_ref_txtOrdenArranqueH: "#txtOrdenArranqueH",
    id_ref_txtEnParaleloH: "#txtEnParaleloH",
    id_ref_txtOrdenParadaH: "#txtOrdenParadaH",
    id_ref_txtFueraParaleloH: "#txtFueraParaleloH",
    id_ref_txtOrdenParadaF: '#txtOrdenParadaF',
    id_ref_txtFueraParaleloF: '#txtFueraParaleloF',
    id_ref_txtOrdenArranqueF: '#txtOrdenArranqueF',
    id_ref_txtEnParaleloF: '#txtEnParaleloF',
};

//======================================================================
//Objeto json
//======================================================================
var EMPRESA_DEFAULT_SELECT = -3;
var ID_CENTRAL_DEFAULT_SELECT = -3;
var ID_MODO_DEFAULT_SELECT = -1;

var OBJ_DATA_HORA_OPERACION = {

    Hopcodi: 0,
    CodiPadre: '',

    IdPos: -1,

    IdEmpresa: EMPRESA_DEFAULT_SELECT,
    IdCentralSelect: ID_CENTRAL_DEFAULT_SELECT,
    IdGrupoModo: ID_MODO_DEFAULT_SELECT,
    TipoModOp: TIPO_HO_MODO,

    FechaIni: '',
    HoraIni: '',
    FechaFin: '',
    HoraFin: '',
    Fechahorordarranq: '',
    Hophorordarranq: '',
    FechaHophorparada: '',
    Hophorparada: '',

    IdTipoOperSelect: SUBCAUSACODI_DEFAULT,
    MatrizunidadesExtra: [],
    Hopdesc: '',
    Hopobs: '',

    OpfueraServ: 0,
    OpCompOrdArranq: 0,
    OpCompOrdParad: 0,
    OpSistAislado: 0,
    OpLimTransm: 0,
    OpArranqBlackStart: 0,
    OpEnsayope: 0,
    IdMotOpForzadaSelect: 0,
    HopPruebaExitosa: 0,

    UsuarioModificacion: '',
    FechaModificacion: '',

    entityBitacora: {
    },
    BitacoraHophoriniFecha: '',
    BitacoraHophoriniHora: '',
    BitacoraHophorfinFecha: '',
    BitacoraHophorfinHora: '',
    BitacoraDescripcion: '',
    BitacoraComentario: '',
    BitacoraDetalle: '',
    BitacoraIdSubCausaEvento: '',
    BitacoraIdEvento: '',
    BitacoraIdTipoEvento: '',
    BitacoraIdEquipo: '',
    BitacoraIdEmpresa: '',
    BitacoraIdTipoOperacion: '',
    HoBitacoraJson: null,

    entityHoDetalle: {},
    ListaDesglose: [],

};

//======================================================================
//Funciones sobre la lista de Horas de Operación
//======================================================================

//Obtener codigo de la unidad TV
function getCodigoTV(grupocodi) {
    var idtv = -1;

    for (var i in GLOBAL_HO.ListaUnidXModoOP) {
        if (GLOBAL_HO.ListaUnidXModoOP[i].Grupocodi == grupocodi && GLOBAL_HO.ListaUnidXModoOP[i].Idtv != 0) {
            idtv = GLOBAL_HO.ListaUnidXModoOP[i].Idtv; //grupocodi despacho de la TV
        }
    }

    if (idtv > 0) {
        for (var j in GLOBAL_HO.ListaUnidades) {
            if (GLOBAL_HO.ListaUnidades[j].Grupocodi == idtv) {
                return GLOBAL_HO.ListaUnidades[j].Equicodi; //equipo del grupo despacho
            }
        }
    }

    return idtv;
}

//Obtener objeto modo de operacion
function getModoFromListaModo(listaModo, grupocodi) {
    if (listaModo != null && listaModo.length > 0) {
        for (var i = 0; i < listaModo.length; i++) {
            if (listaModo[i].Grupocodi == grupocodi) {
                return listaModo[i];
            }
        }
    }

    return null;
}

// Valida si el modo de operación tiene Horas de Operacion
function tieneHorasOperacionXModo(evt, modo) {
    if (evt.ListaHorasOperacion.length > 0) { //si hay horas de operacion                              
        for (var z = 0; z < evt.ListaHorasOperacion.length; z++) {
            if (evt.ListaHorasOperacion[z].Grupocodi == modo.Grupocodi && evt.ListaHorasOperacion[z].OpcionCrud != -1
                && evt.ListaHorasOperacion[z].Equipadre == modo.Equipadre) {
                return true;
            }
        }
    }

    return false;
}

// Valida si el modo de operación tiene Horas Programadas
function tieneHorasProgramadasXModo(evt, modo) {
    if (evt.ListaHorasProgramadas.length > 0) { //si hay horas de operacion                              
        for (var z = 0; z < evt.ListaHorasProgramadas.length; z++) {
            if ((evt.ListaHorasProgramadas[z].Grupocodi == modo.Grupocodi)
                && (evt.ListaHorasProgramadas[z].EsValido)) {
                return true;
            }
        }
    }

    return false;
}

//Listar unidades por Código de modo de operación
function listarUnidadesXModo(modo) {
    var listaUnidadXModo = [];
    if (modo > 0) {
        var objModo = getModoFromListaModo(GLOBAL_HO.ListaModosOperacion, modo);

        var listaUnidades = GLOBAL_HO.ListaUnidXModoOP;
        if (GLOBAL_HO.ListaUnidades.length > 0 && listaUnidades.length > 0) {
            for (var kk = 0; kk < listaUnidades.length; kk++) {
                if (listaUnidades[kk].Grupocodi == modo && listaUnidades[kk].Equipadre == objModo.Equipadre) {
                    listaUnidadXModo.push(listaUnidades[kk]);
                }
            }
        }
    }

    return listaUnidadXModo;
}

//Obtener objeto equipo
function getEquipoFromListaUnidades(listaUnidades, equicodi) {
    if (listaUnidades != null && listaUnidades.length > 0) {
        for (var i = 0; i < listaUnidades.length; i++) {
            if (listaUnidades[i].Equicodi == equicodi) {
                return listaUnidades[i];
            }
        }
    }

    return null;
}

//Listar Horas Operacion por Padre
function listarHorasOperacionByHopcodipadre(listaHorasOperacion, Hopcodipadre, equicodi) {
    var listaData = [];
    if (listaHorasOperacion.length > 0) {
        for (var i = 0; i < listaHorasOperacion.length; i++) {
            //filtrar la hora de operación
            if (listaHorasOperacion[i].Hopcodi == Hopcodipadre
                && listaHorasOperacion[i].OpcionCrud != -1) {
                var listaHoXUnidad = listaHorasOperacion[i].ListaHoUnidad;

                if (listaHoXUnidad) {
                    //de la lista detalle solo obtener el equipo que se esta buscando
                    for (var j = 0; j < listaHoXUnidad.length; j++) {
                        if (listaHoXUnidad[j].Equicodi == equicodi) {
                            listaHoXUnidad[j].FlagTipoHo = TIPO_HO_UNIDAD_BD;

                            listaData.push(listaHoXUnidad[j]);
                        }
                    }
                }

            }
        }
    }

    return listaData;
}

//Listar Horas Operacion por Codigo de Modo de operación
function listarHorasOperacionByModoOp(listaHo, grupocodi) {
    grupocodi = parseInt(grupocodi);
    var listaData = [];
    if (listaHo.length > 0) {
        for (var i = 0; i < listaHo.length; i++) {
            if (listaHo[i].Grupocodi == grupocodi && listaHo[i].OpcionCrud != -1) {
                listaData.push(listaHo[i]);
            }
        }
    }

    return listaData;
}

//Listar Horas Operacion por Codigo de Unidad Especial
function listarHorasOperacionByUnidad(listaHo, equicodi) {
    var listaData = [];
    if (listaHo.length > 0) {
         //lista de horas de operación a nivel de modo
        for (var i = 0; i < listaHo.length; i++) {
            var objEsp = listaHo[i];
            if (objEsp.OpcionCrud != -1) {
                //verificar las unidades de cada modo
                for (var j = 0; j < objEsp.ListaHoUnidad.length; j++) {
                    var objUnidad = objEsp.ListaHoUnidad[j];
                    if (objUnidad.Equicodi == equicodi)
                        listaData.push(objUnidad);
                }
            }
        }
    }

    return listaData;
}

//Listar Horas Operacion por Codigo de Unidad Especial que no esten en un rango
function listarHorasOperacionByUnidadFiltro(listaHo, equicodi, checkdateFrom, checkdateTo) {
    var listaDataIni = listarHorasOperacionByUnidad(listaHo, equicodi);
    var listaFinal = [];
    if (listaDataIni.length > 0) {
        for (var i = 0; i < listaDataIni.length; i++) {
            var dateIni = moment(listaDataIni[i].Hophorini);
            var dateFin = moment(listaDataIni[i].Hophorfin);

            if (!(dateIni.isSameOrAfter(moment(checkdateFrom)) && dateFin.isSameOrBefore(moment(checkdateTo)))) {
                listaFinal.push(listaDataIni[i]);
            }
        }
    }

    return listaFinal;
}

//-- Obtiene posición del codigo de la Hora de operación dentro de la lista
function getPosHop(hopcodi, listaHO) {
    for (var i = 0; i < listaHO.length; i++) {
        if (listaHO[i].Hopcodi == hopcodi)
            return i;
    }

    return -1;
}
function getPosHounidad(hopunicodi, listaHO) {
    for (var i = 0; i < listaHO.length; i++) {
        if (listaHO[i].Hopunicodi == hopunicodi)
            return i;
    }

    return -1;
}

function getHoraoperacionYGrupoByHopcodi(listaHO, listaModo, hopcodi) {
    var arrayObj = [];
    if (listaHO != undefined && listaHO != null) {
        for (var i = 0; i < listaHO.length; i++) {
            if (listaHO[i].Hopcodi == hopcodi) {
                var regHo = listaHO[i];
                var regModoOp = getModoFromListaModo(listaModo, regHo.Grupocodi);

                arrayObj.push(regHo);
                arrayObj.push(regModoOp);

                return arrayObj;
            }
        }
    }

    return arrayObj;
}

//Funcion que indica si las horas de operación del modo tiene el Subcausacodi
function tieneCalificacionFromListaHO(listaHo, subcausacodi) {
    if (listaHo.length > 0) {
        for (var i = 0; i < listaHo.length; i++) {
            if (listaHo[i].Subcausacodi == subcausacodi && listaHo[i].OpcionCrud != -1) {
                return true;
            }
        }
    }

    return false;
}

// Dividir las horas de operacion en rangos que no se intersectan
function cortarListaHoSinInterseccion(listaHo) {
    ordenarListaHorasOperacion(listaHo);

    //generar fechas distintas del listado
    var arrDate = [];
    for (var i = 0; i < listaHo.length; i++) {
        arrDate.push(listaHo[i].Hophorini);
        arrDate.push(listaHo[i].Hophorfin);
    }

    arrDate = arrDate.sort(function (a, b) { return a.valueOf() - b.valueOf() });

    arrDate.filter((date, i, self) =>
        self.findIndex(d => d.valueOf() === date.valueOf()) === i
    );

    //cortar las horas de operación
    var listaHoCorte = [];
    for (var i = 0; i < listaHo.length; i++) {
        var objHo = listaHo[i];
        var fechaIni = moment(objHo.Hophorini);
        var fechaFin = moment(objHo.Hophorfin);

        for (var j = 0; j < arrDate.length - 1; j++) {
            var fechaCorteIni = moment(arrDate[j]);
            var fechaCorteFin = moment(arrDate[j + 1]);

            if (fechaCorteIni.isSameOrAfter(fechaIni) && fechaCorteFin.isSameOrBefore(fechaFin)) {
                var objCorte = {
                    Hophorini: fechaCorteIni,
                    Hophorfin: fechaCorteFin
                };
                listaHoCorte.push(objCorte);
            }
        }
    }

    //lista de unicos
    var listaHoUnico = [];
    for (var i = 0; i < listaHoCorte.length; i++) {
        var fechaCorteIni = moment(listaHoCorte[i].Hophorini);
        var fechaCorteFin = moment(listaHoCorte[i].Hophorfin);

        var existeEnLista = false;
        for (var j = 0; j < listaHoUnico.length && !existeEnLista; j++) {
            if (fechaCorteIni.isSame(listaHoUnico[j].Hophorini) && fechaCorteFin.isSame(listaHoUnico[j].Hophorfin)) {
                existeEnLista = true;
            }
        }

        if (!existeEnLista) {
            listaHoUnico.push(listaHoCorte[i]);
        }
    }

    return listaHoUnico;
}

// Ordenar lista de Horas de Operacion por Fecha Ini
function ordenarListaHorasOperacion(listaHO) {
    for (var i = 0; i < listaHO.length - 1; i++) {
        for (var j = 0; j < listaHO.length - 1; j++) {
            var timeIni = moment(listaHO[j].Hophorini).toDate().getTime();
            var timeSig = moment(listaHO[j + 1].Hophorini).toDate().getTime();

            if (timeIni > timeSig) {
                var tmp = listaHO[j + 1];
                listaHO[j + 1] = listaHO[j];
                listaHO[j] = tmp;
            }
        }
    }
}

//Flag si es Modo de operación especial
// 1: Es unidad especial
// 0: No es unidad especial
function glb_FlagModoEspecial(evt, grupocodi) {
    for (var i = 0; i < evt.ListaModosOperacion.length; i++) {
        var modo = evt.ListaModosOperacion[i];
        if (modo.Grupocodi == grupocodi
            && modo.FlagModoEspecial == FLAG_MODO_OP_ESPECIAL) {
            return FLAG_HO_ESPECIAL;
        }
    }

    return FLAG_HO_NO_ESPECIAL;
}

function desgl_GetObjByTipo(tipo) {
    for (var i = 0; i < LISTA_TIPO_DESGLOSE.length; i++) {
        var obj = LISTA_TIPO_DESGLOSE[i];
        if (obj.TipoDesglose == tipo)
            return obj;
    }
    return null;
}

function desgl_NombreTipoDesglose(tipo) {
    var obj = desgl_GetObjByTipo(tipo);
    if (obj != null)
        return obj.Subcausadesc;

    return '';
}

// Devuelve el nombre del tipo de operacion encontrado por el codigo("subcausacodi")
function getTipoOperacion(subcausacodi) {
    var tipoOperac = "";
    if (GLOBAL_HO.ListaTipoOperacion.length > 0) {
        for (var i = 0; i < GLOBAL_HO.ListaTipoOperacion.length; i++) {
            if (GLOBAL_HO.ListaTipoOperacion[i].Subcausacodi == parseInt(subcausacodi)) {
                tipoOperac = GLOBAL_HO.ListaTipoOperacion[i].Subcausadesc;
            }
        }
    }
    return tipoOperac
}

function getTipoOperacionManual(hopcodi) {
    listTipoOperacion = [];
    strTipoOperacion = "";
    if (GLOBAL_HO.ListaHorasOperacion.length > 0) {
        for (i = 0; i < GLOBAL_HO.ListaHorasOperacion.length; i++) {
            if (GLOBAL_HO.ListaHorasOperacion[i].Hopcodipadre == hopcodi) {
                // lenamos el arreglo con los nombres de tipo de operacion
                listTipoOperacion.push(getTipoOperacion(GLOBAL_HO.ListaHorasOperacion[i].Subcausacodi));
            }
        }
        if (listTipoOperacion.length > 0) {
            var unique = {};
            var distinct = [];
            listTipoOperacion.forEach(function (x) {
                if (!unique[x]) {
                    distinct.push(x);
                    unique[x] = true;
                }
            });

            if (distinct.length == 1) {
                strTipoOperacion = distinct[0];
            }
            else {
                strTipoOperacion = distinct.join('-');
            }
        }
    }
    return strTipoOperacion;
}

function getFindEquipoxCodUnidadesExtra(equicodi, MatrizunidadesExtra) {
    if (MatrizunidadesExtra.length > 0) {
        for (var j = 0; j < MatrizunidadesExtra.length; j++) {
            if (MatrizunidadesExtra[j].EquiCodi == equicodi)
                return j;
        }
    }
    return -1
}

function getFindEquipoxCodUnidadesCentral(idcodi, codGrupos) {
    if (codGrupos.length > 0) {
        for (var j = 0; j < codGrupos.length; j++) {
            if (codGrupos[j].EquiCodi == idcodi)
                return j;
        }
    }
    return -1
}

function getFindUnidadxHoraOp(EquiCodi, ListaHorasOperacion, checkdateFrom, checkdateTo) {
    try {
        if (ListaHorasOperacion.length > 0) {
            for (var i = 0; i < ListaHorasOperacion.length; i++) {
                if (ListaHorasOperacion[i].Equicodi == EquiCodi && ListaHorasOperacion[i].OpcionCrud != -1) {
                    var dateFrom = new Date(moment(ListaHorasOperacion[i].Hophorini));
                    var dateTo = new Date(moment(ListaHorasOperacion[i].Hophorfin));
                    if (moment(dateFrom).isSame(checkdateFrom) && moment(dateTo).isSame(checkdateTo)) {
                        return i;
                    }
                }
            }
        }
    }
    catch (err) {
        alert(err.message);
    }
    return -1;
}

//======================================================================
//Funciones UTIL...
//======================================================================

function obtenerFechaByCampoHO(campo) {
    return campo != null && campo != "" ? moment(campo).format("DD/MM/YYYY") : "";
}

function obtenerHoraByCampoHO(campo) {
    return campo != null && campo != "" ? moment(campo).format("HH:mm:ss") : "";
}

//Obtener date valido para enviar al servidor
function obtenerDateToJson(fechaHora) {
    return fechaHora != null && fechaHora != "" ? moment(fechaHora) : null;
}

function offshowView() {
    $('#enviosHorasOperacion').bPopup().close();
}

function getFormattedDate(date) {
    if (date instanceof Date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        hours = hours < 10 ? '0' + hours : hours;
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;

        return year + '/' + month + '/' + day + " " + strTime;
    }
    else {
        return "No es fecha";
    }
}

// oculta o muestra toda una fila <tr>
function mostrarOcultarCentral(opc) {
    if (opc == 1) {
        $('#trCentral').show();
    }
    if (opc == 0) {
        $('#central').html("");
        $('#trCentral').hide();
    }
}

function limpiarBarra() {
    $('#barraHorasOperacion').css("display", "none");
    $('#detalleHorasOperacion').html("");
}

//convierte 2 cadenas de texto fecha(dd/mm/yyyy) y horas(hh:mm:ss) a tipo Date
function convertStringToDate(fecha, horas) {
    if (fecha) {
        var partsFecha = fecha.split('/');
        horas = obtenerHoraValida(horas);
        if (horas == "") {
            return "";
        }
        var partsHoras = horas.split(':');
        //new Date(yyyy, mm-1, dd, hh, mm, ss);
        return new Date(partsFecha[2], partsFecha[1] - 1, partsFecha[0], partsHoras[0], partsHoras[1], partsHoras[2]);
    } else {
        return null;
    }

}

function diferenciaFechas(fecha01, fecha02) {
    var sFecha = $('#txtFecha').val()
    var HoraIni = convertStringToDate(sFecha, fecha01);
    var HoraFin = convertStringToDate(sFecha, fecha02);
    // comprobar si una fecha es anterior a otra
    return moment(HoraIni).isBefore(HoraFin);
}

function convertirFecha(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

function obtenerDiaSiguiente(sFecha) { //sFecha, formato dd/mm/yyyy
    checkdateFrom = convertStringToDate(sFecha, "00:00:00");
    horaFin01 = moment(checkdateFrom).set('date', moment(checkdateFrom).get('date') + 1);

    return moment(horaFin01).format('DD/MM/YYYY');
}

function obtenerDiaActualFormato() {
    var fechaAct = new Date();
    return moment(fechaAct).format('DD/MM/YYYY');
}

function getDirectionZebra(fechaForm) {
    var fechaActual = moment();
    fechaActual = moment(fechaActual, "DD-MM-YYYY");
    fechaForm = moment(fechaForm, "DD-MM-YYYY");
    var diff = fechaForm.diff(fechaActual, 'days');

    return diff;
}

function formatJavaScriptSerializer(listaHO) {
    if (listaHO != null) {
        var total = listaHO.length;
        for (i = 0; i < total; i++) {
            var reg = listaHO[i];

            reg.Hophorini = reg.Hophorini == undefined || reg.Hophorini == null || reg.Hophorini == "" ? null : moment(reg.Hophorini).toISOString();
            reg.Hophorfin = reg.Hophorfin == undefined || reg.Hophorfin == null || reg.Hophorfin == "" ? null : moment(reg.Hophorfin).toISOString();
            reg.Hophorordarranq = reg.Hophorordarranq == undefined || reg.Hophorordarranq == null || reg.Hophorordarranq == "" ? null : moment(reg.Hophorordarranq).toISOString();
            reg.Hophorparada = reg.Hophorparada == undefined || reg.Hophorparada == null || reg.Hophorparada == "" ? null : moment(reg.Hophorparada).toISOString();
            reg.Lastdate = reg.Lastdate == undefined || reg.Lastdate == null || reg.Lastdate == "" ? null : moment(reg.Lastdate).toISOString();

            reg.FechaProceso = reg.FechaProceso == undefined || reg.FechaProceso == null || reg.FechaProceso == "" ? null : moment(reg.FechaProceso).toISOString();

            reg.FechaProgIni = reg.FechaProgIni == undefined || reg.FechaProgIni == null || reg.FechaProgIni == "" ? null : moment(reg.FechaProgIni).toISOString();
            reg.FechaProgFin = reg.FechaProgFin == undefined || reg.FechaProgFin == null || reg.FechaProgFin == "" ? null : moment(reg.FechaProgFin).toISOString();
            reg.BitacoraHophorini = reg.BitacoraHophorini == undefined || reg.BitacoraHophorini == null || reg.BitacoraHophorini == "" ? null : moment(reg.BitacoraHophorini).toISOString();
            reg.BitacoraHophorfin = reg.BitacoraHophorfin == undefined || reg.BitacoraHophorfin == null || reg.BitacoraHophorfin == "" ? null : moment(reg.BitacoraHophorfin).toISOString();

            if (reg.ListaHoUnidad != null) {
                for (y = 0; y < reg.ListaHoUnidad.length; y++) {
                    var luni = reg.ListaHoUnidad[y];
                    luni.Hopunihorini = luni.Hopunihorini == undefined || luni.Hopunihorini == null || luni.Hopunihorini == "" ? null : moment(luni.Hopunihorini).toISOString();
                    luni.Hopunihorfin = luni.Hopunihorfin == undefined || luni.Hopunihorfin == null || luni.Hopunihorfin == "" ? null : moment(luni.Hopunihorfin).toISOString();
                    luni.Hopunihorarranq = luni.Hopunihorarranq == undefined || luni.Hopunihorarranq == null || luni.Hopunihorarranq == "" ? null : moment(luni.Hopunihorarranq).toISOString();
                    luni.Hophorfin = luni.Hophorfin == undefined || luni.Hophorfin == null || luni.Hophorfin == "" ? null : moment(luni.Hophorfin).toISOString();
                    luni.Hophorini = luni.Hophorini == undefined || luni.Hophorini == null || luni.Hophorini == "" ? null : moment(luni.Hophorini).toISOString();
                    luni.Hopunihorordarranq = luni.Hopunihorordarranq == undefined || luni.Hopunihorordarranq == null || luni.Hopunihorordarranq == "" ? null : moment(luni.Hopunihorordarranq).toISOString();
                    luni.Hophorordarranq = luni.Hophorordarranq == undefined || luni.Hophorordarranq == null || luni.Hophorordarranq == "" ? null : moment(luni.Hophorordarranq).toISOString();
                    luni.Hopunihorparada = luni.Hopunihorparada == undefined || luni.Hopunihorparada == null || luni.Hopunihorparada == "" ? null : moment(luni.Hopunihorparada).toISOString();
                    luni.Hophorparada = luni.Hophorparada == undefined || luni.Hophorparada == null || luni.Hophorparada == "" ? null : moment(luni.Hophorparada).toISOString();
                }
            }

            if (reg.ListaDesglose != null) {
                reg.ListaDesglose.forEach(function (objDesg) {
                    objDesg = formatObjDesglose(objDesg);
                });
            }
        }

        return listaHO;
    }

    return {};
}

function formatObjDesglose(objDesg) {

    var Ichorini = convertStringToDate(objDesg.FechaIni, objDesg.HoraIni);
    var Ichorfin = convertStringToDate(objDesg.FechaFin, objDesg.HoraFin);

    objDesg.Ichorini = getTimeValidoToJson(Ichorini);
    objDesg.Ichorfin = getTimeValidoToJson(Ichorfin);

    return objDesg;
}

function getTimeValidoToJson(datetime) {
    if (datetime == undefined || datetime == null || datetime == "")
        return null;
    return moment(datetime).toISOString();
}

function obtenerHoraValida(hora) {
    if (hora !== undefined && hora != null) {
        hora = hora.replace('h', '0');
        hora = hora.replace('h', '0');

        hora = hora.replace('m', '0');
        hora = hora.replace('m', '0');

        hora = hora.replace('s', '0');
        hora = hora.replace('s', '0');
        return hora;
    }

    return '';
}

function obtenerHoraFinDefecto() {
    return $("#hfHoraFinDefecto").val() + ":00";
}

function val_esValidoCadena(str) {
    return str != undefined && str != null && str != '' && str.trim() != '';
}

function val_esValidoDate(datetime) {
    return datetime != undefined && datetime != null && (datetime instanceof Date) && !isNaN(datetime.getTime());
}

function validarFecha(fecha) {
    fechaProcesada = GLOBAL_HO.FechaListado;

    if (fecha == fechaProcesada) {
        return true;
    }
    return false;
}