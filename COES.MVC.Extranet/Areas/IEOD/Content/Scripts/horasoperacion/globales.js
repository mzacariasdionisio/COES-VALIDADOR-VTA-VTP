var controlador = siteRoot + 'IEOD/HorasOperacion/';

var ID_CENTRAL_DEFAULT = -2;
var TIPO_CENTRAL_HIDROELECTRICA = 4;
var TIPO_CENTRAL_TERMOELECTRICA = 5;
var TIPO_CENTRAL_SOLAR = 37;
var TIPO_CENTRAL_EOLICA = 39;

var MODO_GRUPORESERVAFRIA = 1;
var FLAG_GRUPORESERVAFRIA_TO_REGISTRAR_UNIDADES = 1;

var SUBCAUSACODI_POR_PRUEBAS = 106;

var FLAG_MODO_OP_ESPECIAL = "S";
var FLAG_HO_ESPECIAL = 1;
var FLAG_HO_NO_ESPECIAL = 0;
var TIPO_HO_MODO = 1;
var TIPO_HO_UNIDAD = 2;
var FLAG_UNIDAD_INDEPENDIZADA = true;
var FLAG_UNIDAD_NO_INDEPENDIZADA = false;
var TIPO_HO_MODO_BD = 1;
var TIPO_HO_UNIDAD_BD = 2;

var FLAG_UNIDAD_ESPECIAL_ADMIN_CREACION = 0;
var FLAG_UNIDAD_ESPECIAL_ADMIN_MODIFICACION_PROPIO = 1;
var FLAG_UNIDAD_ESPECIAL_ADMIN_MODIFICACION_FROM_AGENTE = 2;

var FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION = 5;
var FLAG_UNIDAD_ESPECIAL_AGENTE_MODIFICACION_PROPIO = 6;
var FLAG_UNIDAD_ESPECIAL_AGENTE_MODIFICACION_FROM_ADMIN = 7;

var TAMANIO_GRAFICO = 1040;
var TAMANIO_TABLA_GRAFICO = TAMANIO_GRAFICO - 0;

var APP_OPCION = -1;
var OPCION_COPIAR = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ELIMINAR = 4;
var OPCION_VER = 5;
var OPCION_ENVIO_DATOS = 6;

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

var OBJ_DATA_HORA_OPERACION = {
    IdPos: -1,

    IdEmpresa: -1,
    IdTipoCentral: 0,
    IdCentralSelect: -3,
    IdEquipoOrIdModo: -1,
    IdEquipo: 0,
    TipoModOp: TIPO_HO_MODO,

    FechaIni: '',
    HoraIni: '',
    FechaFin: '',
    HoraFin: '',
    Fechahorordarranq: '',
    Hophorordarranq: '',
    FechaHophorparada: '',
    Hophorparada: '',

    IdTipoOperSelect: -1,
    MatrizunidadesExtra: [],
    Hopobs: '',
    Hopnotifuniesp: FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION,

    OpfueraServ: 0,
    OpCompOrdArranq: 0,
    OpCompOrdParad: 0,
    OpSistAislado: 0,
    OpLimTransm: 0,
    OpArranqBlackStart: 0,
    OpEnsayope: 0,
    IdMotOpForzadaSelect: 0,

    //dataListaLineasCongestion: dataListaLineasCongestion,

    UsuarioModificacion: '',
    FechaModificacion: '',

    entityBitacora: {},
    HoBitacoraJson: null,

    entityHoDetalle: {},
    HoDetalleJson: null,
    ListaDesglose: [],
};

//======================================================================
//Funciones sobre la lista de Horas de Operación
//======================================================================

function getCodigoTV() {
    idtv = -1;

    for (var i in evtHot.ListaUnidXModoOP) {

        if (evtHot.ListaUnidXModoOP[i].Idtv != 0) {
            idtv = evtHot.ListaUnidXModoOP[i].Idtv;
        }
    }

    for (var j in evtHot.ListaUnidades) {
        if (evtHot.ListaUnidades[j].Grupocodi == idtv) {
            return evtHot.ListaUnidades[j].Equicodi;
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
//Listar unidades por Código de modo de operación
function listarUnidadesXModo(evtHot, modo, equipadre) {
    var listaUnidadXModo = [];
    var listaUnidades = evtHot.ListaUnidXModoOP;
    if (evtHot.ListaUnidades.length > 0 && listaUnidades.length > 0) {
        for (var kk = 0; kk < listaUnidades.length; kk++) {
            if (listaUnidades[kk].Grupocodi == modo && listaUnidades[kk].Equipadre == equipadre) {
                listaUnidadXModo.push(listaUnidades[kk]);
            }
        }
    }

    return listaUnidadXModo;
}

//Listar unidades por Hora de Operación
function listarUnidadesXModoByHo(pos) {
    var hoppadre = evtHot.ListaHorasOperacion[pos];

    if (hoppadre.FlagModoEspecial != FLAG_MODO_OP_ESPECIAL) {
        return listarUnidadesXModo(evtHot, hoppadre.Grupocodi, hoppadre.Equipadre);
    }

    var listaUnidades = []
    if (evtHot.ListaHorasOperacion.length > 0) {
        for (var zz = 0; zz < evtHot.ListaHorasOperacion.length; zz++) {
            if (evtHot.ListaHorasOperacion[zz].Hopcodipadre == hoppadre.Hopcodi) {
                listaUnidades.push({ Equicodi: evtHot.ListaHorasOperacion[zz].Equicodi, Gruponomb: hoppadre.Gruponomb, Emprnomb: hoppadre.Emprnomb });
            }
        }
    }

    if (evtHot.ListaUnidades.length > 0 && evtHot.ListaUnidXModoOP.length > 0) {
        for (var kk = 0; kk < evtHot.ListaUnidades.length; kk++) {
            for (var nn = 0; nn < listaUnidades.length; nn++) {
                if (listaUnidades[nn].Equicodi == evtHot.ListaUnidades[kk].Equicodi) {
                    listaUnidades[nn].Equinomb = evtHot.ListaUnidades[kk].Equinomb;
                }
            }
        }
    }

    var obj = {};
    for (var i = 0, len = listaUnidades.length; i < len; i++)
        obj[listaUnidades[i]['Equinomb']] = listaUnidades[i];

    var listaFinal = new Array();
    for (var key in obj)
        listaFinal.push(obj[key]);

    return listaFinal;
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
            if (listaHorasOperacion[i].Hopcodipadre == Hopcodipadre
                && listaHorasOperacion[i].Equicodi == equicodi
                && listaHorasOperacion[i].OpcionCrud != -1) {
                listaHorasOperacion[i].FlagTipoHo = TIPO_HO_UNIDAD_BD;
                listaData.push(listaHorasOperacion[i]);
            }
        }
    }

    return listaData;
}

//Listar Horas Operacion por Codigo de Unidad Especial
function listarHorasOperacionByUnidad(listaHo, equicodi) {
    var listaData = [];
    if (listaHo.length > 0) {
        for (var i = 0; i < listaHo.length; i++) {
            if (listaHo[i].Equicodi == equicodi && listaHo[i].OpcionCrud != -1) {
                listaData.push(listaHo[i]);
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

// Dividir las horas de operacion en rangos que no se intersectan
function cortarListaHoSinInterseccion(listaHo) {
    ordenarListaHorasOperacion(listaHo);

    //generar fechas distintas del listado
    var arrDate = [];
    for (var i = 0; i < listaHo.length; i++) {
        arrDate.push(listaHo[i].Hophorini);
        arrDate.push(listaHo[i].Hophorfin);
    }

    arrDate = arrDate.sort(function (a, b) { return a.getTime() - b.getTime() });

    arrDate.filter((date, i, self) =>
        self.findIndex(d => d.getTime() === date.getTime()) === i
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

function getFindEquipoxCodUnidadesExtra(idcodi, MatrizunidadesExtra) {
    if (MatrizunidadesExtra.length > 0) {
        for (var j = 0; j < MatrizunidadesExtra.length; j++) {
            if (MatrizunidadesExtra[j].EquiCodi == idcodi)
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

//indica si existe hora de operacion con hora de parada mayor o igual a las 23:00:00 hs.
function obtenerHoraOperacionDiaAnterior(codEquipoActual) {
    sFecha = $('#txtFecha').val();
    checkdateFrom = convertStringToDate(sFecha, "23:59:59");

    var tipoCentral = parseInt(evtHot.IdTipoCentral);
    switch (tipoCentral) {
        case 4: //Hidro
            for (var i in evtHot.ListaCentrales) {
                for (var j in evtHot.ListaGrupo) {
                    if (evtHot.ListaGrupo[j].Equipadre == evtHot.ListaCentrales[i].Equicodi) {
                        if (evtHot.ListaHorasOperacionAnt.length > 0) { //si hay horas de operacion el dia anterior
                            for (var z in evtHot.ListaHorasOperacionAnt) {
                                if (evtHot.ListaHorasOperacionAnt[z].Equicodi == codEquipoActual) {
                                    horaFinAnt = moment(evtHot.ListaHorasOperacionAnt[z].Hophorfin);
                                    horaFinAntFormato = horaFinAnt.format('HH:mm:ss');
                                    horaParAnt = moment(evtHot.ListaHorasOperacionAnt[z].Hophorparada);
                                    if (horaFinAntFormato == "00:00:00") {//if (horaFinAnt == "23:59:59") {
                                        horaFinAntMaxLim01 = moment(checkdateFrom).set('date', moment(checkdateFrom).get('date') - 1);

                                        horaFinAntMaxLim02 = horaFinAntMaxLim01;
                                        horaFinAntMaxLim02 = moment(horaFinAntMaxLim02).set('hour', 23);
                                        horaFinAntMaxLim02 = moment(horaFinAntMaxLim02).set('minute', 0);
                                        horaFinAntMaxLim02 = moment(horaFinAntMaxLim02).set('second', 0);
                                        if (moment(horaParAnt).isBetween(moment(horaFinAntMaxLim02), moment(horaFinAnt))) {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            break;
        case 5: //Térmo
            for (var i in evtHot.ListaModosOperacion) {
                for (var z in evtHot.ListaHorasOperacionAnt) {
                    if (evtHot.ListaHorasOperacionAnt[z].Grupocodi == codEquipoActual) {
                        horaFinAnt = moment(evtHot.ListaHorasOperacionAnt[z].Hophorfin);
                        horaFinAntFormato = horaFinAnt.format('HH:mm:ss');
                        horaParAnt = moment(evtHot.ListaHorasOperacionAnt[z].Hophorparada);
                        if (horaFinAntFormato == "00:00:00") {
                            horaFinAntMaxLim01 = moment(checkdateFrom).set('date', moment(checkdateFrom).get('date') - 1);

                            horaFinAntMaxLim02 = horaFinAntMaxLim01;
                            horaFinAntMaxLim02 = moment(horaFinAntMaxLim02).set('hour', 23);
                            horaFinAntMaxLim02 = moment(horaFinAntMaxLim02).set('minute', 0);
                            horaFinAntMaxLim02 = moment(horaFinAntMaxLim02).set('second', 0);
                            if (moment(horaParAnt).isBetween(moment(horaFinAntMaxLim02), moment(horaFinAnt))) {
                                return true;
                            }
                        }
                    }
                }
            }

            break;
        case 37: //Solares
        case 39: //Eolicas
            for (var i in evtHot.ListaCentrales) {
                for (var z in evtHot.ListaHorasOperacionAnt) {
                    if (evtHot.ListaHorasOperacionAnt[z].Equicodi == codEquipoActual) {
                        horaFinAnt = moment(evtHot.ListaHorasOperacionAnt[z].Hophorfin);
                        horaFinAntFormato = horaFinAnt.format('HH:mm:ss');
                        horaParAnt = moment(evtHot.ListaHorasOperacionAnt[z].Hophorparada);
                        if (horaFinAntFormato == "00:00:00") {
                            horaFinAntMaxLim01 = moment(checkdateFrom).set('date', moment(checkdateFrom).get('date') - 1);

                            horaFinAntMaxLim02 = horaFinAntMaxLim01;
                            horaFinAntMaxLim02 = moment(horaFinAntMaxLim02).set('hour', 23);
                            horaFinAntMaxLim02 = moment(horaFinAntMaxLim02).set('minute', 0);
                            horaFinAntMaxLim02 = moment(horaFinAntMaxLim02).set('second', 0);
                            if (moment(horaParAnt).isBetween(moment(horaFinAntMaxLim02), moment(horaFinAnt))) {
                                return true;
                            }
                        }
                    }
                }
            }
            break;
    }

    return false;
}

function esEditableRegHO(esEditable, regHo) {
    if (esEditable) {
        if (glb_FlagModoEspecial(evtHot, regHo.Grupocodi) == FLAG_HO_ESPECIAL) {
            return true;
        } else {
            var notifUniEsp = regHo.Hopnotifuniesp;
            if (notifUniEsp == FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION || notifUniEsp == FLAG_UNIDAD_ESPECIAL_AGENTE_MODIFICACION_PROPIO) {
                return true;
            }
        }
    }

    return false;
}

function getTextoNotificacion(opcionVista, grupocodi, notifUniEsp) {
    var flagEsp = glb_FlagModoEspecial(evtHot, grupocodi);
    if (opcionVista != OPCION_VER) {
        if (FLAG_HO_ESPECIAL == flagEsp) {
            if (notifUniEsp < FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION || notifUniEsp == FLAG_UNIDAD_ESPECIAL_AGENTE_MODIFICACION_FROM_ADMIN) {
                return "El usuario Administrador realizó la última actualización en el sistema, favor de verificar.";
            }
        }
    } else {
        if (FLAG_HO_ESPECIAL != flagEsp) {
            if (notifUniEsp < FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION || notifUniEsp == FLAG_UNIDAD_ESPECIAL_AGENTE_MODIFICACION_FROM_ADMIN) {
                return "El usuario Administrador realizó la última actualización en el sistema, no está permitido la modificación de este registro.";
            }
        }
    }

    return "";
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

function getFormattedDate2(date) {
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

function getFormattedDate3(date) {
    if (date instanceof Date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var segundos = date.getSeconds();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        hours = hours < 10 ? '0' + hours : hours;
        minutes = minutes < 10 ? '0' + minutes : minutes;
        segundos = segundos < 10 ? '0' + segundos : segundos;
        var strTime = hours + ':' + minutes + ':' + segundos + ' ' + ampm;

        return year + '/' + month + '/' + day + " " + strTime;
    }
    else {
        return "No es fecha";
    }
}

// oculta o muestra toda una fila <tr> Central
function mostrarOcultarCentral(opc) {
    if (opc == 1) {
        $('#trCentral').show();
    }
    if (opc == 0) {
        $('#central').html("");
        $('#trCentral').hide();
    }
}

//Oculta la barra de menu grafico
function limpiarBarra() {
    $('#barraHorasOperacion').css("display", "none");
    $('#detalleHorasOperacion').html("");
    $(".msjNotificacionUniEsp").hide();
}

// cierra la vista de envio de horas de operación
function offshowView() {
    $('#enviosHorasOperacion').bPopup().close();
}
function offshowView2() {
    $('#enviosHorasOperacion').bPopup().close();
    $('.fila_val_intervenciones').hide();

    $("#btnAceptar3").hide();
    $("#btnCancelar3").hide();

    $("#btnAcep").show();
    $("#btnCancel").show();

    $("#mensajeEvento").hide();
}

//convierte 2 cadenas de texto fecha(dd/mm/yyyy) y horas(hh:mm:ss) a tipo Date
function convertStringToDate(fecha, horas) {
    var partsFecha = fecha.split('/');
    horas = obtenerHoraValida(horas);
    if (horas == "") {
        return "";
    }
    var partsHoras = horas.split(':');
    //new Date(yyyy, mm-1, dd, hh, mm, ss);
    return new Date(partsFecha[2], partsFecha[1] - 1, partsFecha[0], partsHoras[0], partsHoras[1], partsHoras[2]);
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

function obtenerDiaSiguiente(sFecha) { //sFecha, formato YYYY-MM-DD STRING
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

function ui_setInputmaskHora(ref_element) {
    $(ref_element).inputmask({
        mask: "h:s:s",
        placeholder: "hh:mm:ss",
        alias: "datetime",
        hourformat: 24
    });
}

function val_esValidoCadena(str) {
    return str != undefined && str != null && str != '' && str.trim() != '';
}

function val_esValidoDate(datetime) {
    return datetime != undefined && datetime != null && (datetime instanceof Date) && !isNaN(datetime.getTime());
}

function validarFecha(fecha) {
    fechaProcesada = evtHot.FechaListado;

    if (fecha == fechaProcesada) {
        return true;
    }
    return false;
}
