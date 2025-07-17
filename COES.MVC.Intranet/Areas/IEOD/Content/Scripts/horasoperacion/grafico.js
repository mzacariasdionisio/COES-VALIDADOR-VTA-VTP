var WIDTH_COL_ESTADO_GRUPO = 40;
var WIDTH_COL_EMP = 150;
var WIDTH_COL_CENTRAL = 100;
var WIDTH_COL_GRUPO = 140;
var WIDTH_COL_HORA = 39;
var WIDTH_COL_CI = 46;
var WIDTH_COL_CV = 46;
var WIDTH_COL_PMIN = 38;
var WIDTH_COL_PE = 38;
var WIDTH_COL_TMIN_O = 36;
var WIDTH_COL_TENTREARRAQ = 36;
var WIDTH_COL_PUEDEOFF = 56;
var WIDTH_COL_PUEDEON = 50;
var WIDTH_COL_CAMPANA = 70;
var HEIGTH_BORDER_CELDA_TD = 1;

var IMG_MODO_ON = `<img src='${siteRoot}Areas/IEOD/Content/Images/btn_on16x16.jpg' title='E/S actualmente' />`;
var IMG_MODO_OFF = `<img src='${siteRoot}Areas/IEOD/Content/Images/btn_off16x16.jpg' title='F/S actualmente' />`;

//////////////////////////////////////////////////////////////////////////////////////////
/// Gráfico 24h
//////////////////////////////////////////////////////////////////////////////////////////

function dibujarTablaHorasOperacionEms() {

    $("#td_leygraf_tipo_1").css("background-color", DESGLOSE_COLOR_PLENA_CARGA);
    $("#td_leygraf_tipo_2").css("background-color", DESGLOSE_COLOR_POTENCIA_FIJA);
    $("#td_leygraf_tipo_3").css("background-color", DESGLOSE_COLOR_POTENCIA_MAXIMA);
    $("#td_leygraf_tipo_4").css("background-color", DESGLOSE_COLOR_POTENCIA_MINIMA);
    $("#td_leygraf_tipo_5").css("background-color", CONGESTION_COLOR);

    var strHtml = generaViewListaTermicasEms(getFechaEms());
    strHtml += "<div style='clear:both; height:5px'></div>";
    strHtml += "<div class='form-title_intranet'>";
    return strHtml;
}

var ORDEN_GRAFICO_CAMPO = 0;
var ORDEN_GRAFICO_CAMPO_CI = 1;
var ORDEN_GRAFICO_CAMPO_CV = 2;

var ORDEN_GRAFICO_TIPO = 'asc';

function generaViewListaTermicasEms(fecha, flagDesglose, flagCongestion) {
    var cadena = "";
    var cadenaHP = "";
    var styleDesglose = flagDesglose ? `display: block;` : `display: none;`;
    var styleCongestion = flagCongestion ? `display: block;` : `display: none;`;

    cadena += `<table class='pretty tabla-horas' style='width: ${TAMANIO_TABLA_GRAFICO}px'>`;

    // Cabecera

    var alturaTh = 12 + HEIGTH_BORDER_CELDA_TD;
    cadena += "<thead>";
    var segmento = WIDTH_COL_ESTADO_GRUPO + WIDTH_COL_EMP + WIDTH_COL_CENTRAL + WIDTH_COL_GRUPO + HEIGTH_BORDER_CELDA_TD * 4; //ancho anterior al inicio de la primera hora , incluye border 1 de cada celda

    cadena += "<tr> ";
    cadena += "<th class='alerta_encendido' style='width:" + WIDTH_COL_ESTADO_GRUPO + "px; height: " + alturaTh + "px;'></th> ";
    cadena += "<th class='' style='width:" + WIDTH_COL_EMP + "px'>EMPRESA</th> ";
    cadena += "<th class='' style='width:" + WIDTH_COL_CENTRAL + "px'>CENTRAL</th> ";
    cadena += "<th class='' style='width:" + WIDTH_COL_GRUPO + "px'>MODOS DE OPERACIÓN</th> ";
    for (var i = 0; i < 24; i++) {
        cadena += "<th class=''  style='width:" + WIDTH_COL_HORA + "px'>" + i + "</th>";
    }

    cadena += "<th class='' style='width:" + WIDTH_COL_CI + "px' title='Costo Incremental' >C.I.</th> "; //
    cadena += "<th class='' style='width:" + WIDTH_COL_CV + "px' title='Costo Variable' >C.V.</th> "; //

    //nuevas columnas
    cadena += "<th class='' style='width:" + WIDTH_COL_PMIN + "px' title='Potencia mínima'>Pmin</th> "; //Pmin
    cadena += "<th class='' style='width:" + WIDTH_COL_PE + "px' title='Potencia efectiva'>Pef</th> "; //Pef
    cadena += "<th class='' style='width:" + WIDTH_COL_TMIN_O + "px' title='Tiempo minimo de operación'>TminO</th> "; //TminO
    cadena += "<th class='' style='width:" + WIDTH_COL_TENTREARRAQ + "px' title='Tiempo entre arranques'>T ÷ Arr</th> "; //TminA
    cadena += "<th class='' style='width:" + WIDTH_COL_PUEDEON + "px' title='Puede parar'>PuedeOFF</th> "; //TParada
    cadena += "<th class='' style='width:" + WIDTH_COL_PUEDEOFF + "px' title='Puede arrancar'>PuedeON</th> "; //TArranque

    cadena += "<th class='alerta_ems' style='width:" + WIDTH_COL_CAMPANA + "px'></th> "; //alerta scada o ems, costo incremental, intervencion
    cadena += "<th class='alerta_ems' style=''></th> "; //ultima celda para ocupe lo que sobre
    cadena += "</tr></thead>";

    // Cuerpo

    //declaramos la lista de centrales termicas para traer la data
    if (GLOBAL_HO.ListaHorasOperacion.length > 0) {
        lstHorasOperacion = GLOBAL_HO.ListaHorasOperacion;
    }

    if (GLOBAL_HO.ListaHorasProgramadas.length > 0) {
        lstHorasProgramadas = GLOBAL_HO.ListaHorasProgramadas;
    }

    var alturaTD = 22 + HEIGTH_BORDER_CELDA_TD;
    var alturaTDbtn = 20;
    var paddingTop = 2;
    var alturaHOP = 19;
    var anchoTh = WIDTH_COL_HORA + HEIGTH_BORDER_CELDA_TD;

    var posYDiv = 0;

    var contadorModo = 0;
    var contadorModoEncendido = 0;
    if (GLOBAL_HO.ListaModosOperacionCI.length > 0) {

        for (var i = 0; i < GLOBAL_HO.ListaModosOperacionCI.length; i++) {
            if (tieneHorasOperacionXModo(GLOBAL_HO, GLOBAL_HO.ListaModosOperacionCI[i])) {
                var objModo = GLOBAL_HO.ListaModosOperacionCI[i];

                objModo.ListaHopcodiEms = [];
                objModo.ListaHopcodiScada = [];
                objModo.ListaHopcodiIntervencion = [];
                objModo.ListaHopcodiCostoIncremental = [];
                var tipoOPEncendido = "";
                var enparaleloEncendido = "";
                var fueraparaleloEncendido = "";
                var obsModoEncendido = '';

                //solo para fecha Hoy
                var grupoencendido = verificaEquipoencendidoTermico(GLOBAL_HO.ListaModosOperacionCI[i].Grupocodi);

                //Color azul para los modos que tienen la flecha verde
                var claseFilaCIencendido = '';
                if (objModo.FlagEncendido == FLAG_MODO_OPERO) {
                    claseFilaCIencendido = 'ci_encendido';
                }

                //Columna 1: ON / OFF
                cadena += "<tr>";
                cadena += "<td class='alerta_encendido' style='background-color: white; padding-left: 5px;padding-right: 5px; padding-top: 2px; height: " + alturaTDbtn + "px!important;'>";
                cadena += (grupoencendido == 1) ? IMG_MODO_ON : IMG_MODO_OFF;
                cadena += "</td>";

                var titlePrefijoFila = `Empresa: ${objModo.EmprNomb} \nCentral T: ${objModo.Central} \nModo Op: ${objModo.Gruponomb} (${objModo.Grupoabrev}) \n`;
                //Columna 2: EMPRESA
                cadena += `<td class='${claseFilaCIencendido}' style='text-align: left;padding-left: 3px;' title='${titlePrefijoFila}' >${_textoTdGrafico(objModo.EmprNomb, WIDTH_COL_EMP)}</td>`;
                //Columna 3: CENTRAL
                cadena += `<td class='${claseFilaCIencendido}' style='text-align: left;padding-left: 3px;' title='${titlePrefijoFila}' >${_textoTdGrafico(objModo.Central, WIDTH_COL_CENTRAL)}</td>`;
                //Columna 4: MODOS DE OPERACIÓN
                cadena += `<td class='${claseFilaCIencendido}' style='' title='${titlePrefijoFila}' >${_textoTdGrafico(objModo.Grupoabrev, WIDTH_COL_GRUPO)}</td>`;

                var cadenaDes = "";
                var colorDes = "";

                //html flotante
                if (GLOBAL_HO.ListaHorasOperacion.length > 0) { //si hay horas de operacion                              
                    for (var z = 0; z < GLOBAL_HO.ListaHorasOperacion.length; z++) {
                        var regHO = GLOBAL_HO.ListaHorasOperacion[z];

                        cadenaDes = "";

                        //***convertimos los datos tipo /Date(99999999999)/ a Date
                        regHO.Hophorini = moment(regHO.Hophorini);
                        regHO.Hophorfin = moment(regHO.Hophorfin);

                        //Todas las horas de operación del día por modo de operacion
                        if (regHO.Grupocodi == objModo.Grupocodi && regHO.OpcionCrud != -1) {
                            var dateFrom = new Date(moment(regHO.Hophorini));
                            var dateTo = new Date(moment(regHO.Hophorfin));
                            var isDateNow = false;
                            var fechaActual = new Date();
                            // comprobar si una fecha actual es anterior a la barra de hora de operación
                            if (!moment(fechaActual).isBetween(dateFrom, dateTo) && moment(fechaActual).isBefore(dateFrom)) {
                                isDateNow = true;
                            }
                            var horaIni = (new Date(regHO.Hophorini)).getHours();
                            var horaFin = (new Date(regHO.Hophorfin)).getHours();
                            var minIni = (new Date(regHO.Hophorini)).getMinutes();
                            var minFin = (new Date(regHO.Hophorfin)).getMinutes();

                            var anchoDiv = _getAnchoDivBloqueHora(horaIni, minIni, horaFin, minFin);

                            var posXDiv = segmento + _getAnchoDivBloqueHora(0, 0, horaIni, minIni, true);
                            posYDiv = (alturaTD * contadorModo) + alturaTh + paddingTop;
                            var id = "hopGraf" + regHO.Hopcodi;

                            //
                            var anchoDivDesglose = 0;
                            var posXDivDes = posXDiv;
                            var posYDivDes = posYDiv + 21;

                            // Congestión 
                            var titleCongestion = '';
                            if (regHO.Hoplimtrans == "S") {

                                titleCongestion = "Límite de Transmisión (Congestión)\n";

                                var fecIni = new Date(regHO.Hophorini);
                                var fecFin = new Date(regHO.Hophorfin);

                                var horaIniDes = fecIni.getHours();
                                var horaFinDes = fecFin.getHours();
                                var minIniDes = fecIni.getMinutes();
                                var minFinDes = fecFin.getMinutes();

                                anchoDivDesglose = _getAnchoDivBloqueHora(horaIniDes, minIniDes, horaFinDes, minFinDes);

                                if (anchoDivDesglose > 0) {

                                    colorDes = CONGESTION_COLOR;

                                    posXDivDes = segmento + _getAnchoDivBloqueHora(0, 0, horaIniDes, minIniDes, true);

                                    cadenaDes += `<div class='dv-congestion horasoperacion ${claseBorderHoAislado}' `;
                                    cadenaDes += `id='${id}-s' style='${styleCongestion};width:`
                                    cadenaDes += anchoDivDesglose + "px;left:" + posXDivDes + "px;background-color:" + colorDes + ";border-color:" + colorDes + ";top:" + (posYDivDes) + "px; height: " + "1" + "px;' />";
                                }
                            }

                            posYDivDes = posYDiv + 17;

                            // Desglose
                            var titleDesglose = '';
                            for (var indiceDes = 0; indiceDes < regHO.ListaDesglose.length; indiceDes++) {

                                var desglose = regHO.ListaDesglose[indiceDes];

                                var fechaIniAux = desglose.FechaIni.split("/");
                                var fechaIni = fechaIniAux[2] + "-" + fechaIniAux[1] + "-" + fechaIniAux[0];

                                var fechaFinAux = desglose.FechaFin.split("/");
                                var fechaFin = fechaFinAux[2] + "-" + fechaFinAux[1] + "-" + fechaFinAux[0];

                                titleDesglose += `${desgl_NombreTipoDesglose(desglose.TipoDesglose)}. Hora Inicio: ${desglose.HoraIni}. Hora Final: ${desglose.HoraFin}. \n`;

                                var horaIniDes = (new Date(fechaIni + " " + desglose.HoraIni)).getHours();
                                var horaFinDes = (new Date(fechaFin + " " + desglose.HoraFin)).getHours();
                                var minIniDes = (new Date(fechaIni + " " + desglose.HoraIni)).getMinutes();
                                var minFinDes = (new Date(fechaFin + " " + desglose.HoraFin)).getMinutes();

                                anchoDivDesglose = _getAnchoDivBloqueHora(horaIniDes, minIniDes, horaFinDes, minFinDes);

                                if (anchoDivDesglose > 0) {
                                    if (desglose.TipoDesglose == DESGLOSE_TIPO_PLENA_CARGA) { colorDes = DESGLOSE_COLOR_PLENA_CARGA; }
                                    if (desglose.TipoDesglose == DESGLOSE_TIPO_POTENCIA_FIJA) { colorDes = DESGLOSE_COLOR_POTENCIA_FIJA; }
                                    if (desglose.TipoDesglose == DESGLOSE_TIPO_POTENCIA_MAXIMA) { colorDes = DESGLOSE_COLOR_POTENCIA_MAXIMA; }
                                    if (desglose.TipoDesglose == DESGLOSE_TIPO_POTENCIA_MINIMA) { colorDes = DESGLOSE_COLOR_POTENCIA_MINIMA; }

                                    posXDivDes = segmento + _getAnchoDivBloqueHora(0, 0, horaIniDes, minIniDes, true);

                                    cadenaDes += `<div class='dv-desglose horasoperacion' `;
                                    cadenaDes += `id='${id}-s' title='${tituloDes}' style='${styleDesglose};width:`
                                    cadenaDes += anchoDivDesglose + "px;left:" + posXDivDes + "px;background-color:" + colorDes + ";border-color:" + colorDes + ";top:" + (posYDivDes) + "px; height: " + "1" + "px;' />";
                                }
                            }

                            //

                            var color_ho = getColorHOP(regHO.Subcausacodi);
                            if (isDateNow) {// si hora de operacion es posterior a la fecha actual cambiar color 
                                color_ho = "#A9A9A9"; //DarkGray 
                            }

                            //verificar si la hora de operacion esta en Sistema aislado
                            var saislado = regHO.Hopsaislado;
                            var claseBorderHoAislado = '';
                            if (saislado == 1)
                                claseBorderHoAislado = 'sistema_aislado';

                            var tipoOp = `Calificación: ${getTipoOperacion(regHO.Subcausacodi)} \n`;
                            var enparalelo = `En paralelo: ${moment(dateFrom).format('HH:mm:ss')} \n`;
                            var fueraparalelo = `Fuera de paralelo: ${moment(dateTo).format('HH:mm:ss')} \n`;
                            var sistemaAisladoDesc = saislado == 1 ? "SISTEMA AISLADO \n" : "";
                            var obsModo = regHO.Hopdesc ?? '';
                            if (obsModo != "") obsModo = `Observación: ${obsModo} \n`;

                            //verificar que calificacion segun el filtro de hora y minuto
                            if (objModo.FlagEncendido == FLAG_MODO_OPERO && objModo.Hopcodi == regHO.Hopcodi) {
                                tipoOPEncendido = tipoOp;
                                enparaleloEncendido = enparalelo;
                                fueraparaleloEncendido = fueraparalelo;
                                obsModoEncendido = obsModo;
                                sisAisladoEncendido = sistemaAisladoDesc;
                            }

                            if (objModo.FlagModoEspecial == 'S') {
                                // imprime horas de operacion para modo de operacion extra

                                var unidadesExtra = listarUnidadesXModo(objModo.Grupocodi);

                                var nUnidadesOp = parseInt(unidadesExtra != null && unidadesExtra.length) || 0;
                                nUnidadesOp = nUnidadesOp == 0 ? 1 : nUnidadesOp;

                                var altura = alturaHOP / nUnidadesOp;
                                var posYDivEspecial = posYDiv;
                                for (var nh = 0; nh < nUnidadesOp; nh++) {
                                    if (isDateNow) {// si hora de operacion es posterior a la fecha actual cambiar color 
                                        color_ho = "#A9A9A9"; //DarkGray 
                                    }
                                    posYDivEspecial += (nh > 0 ? altura : 0);

                                    var lHopByUnidad = [];
                                    if (unidadesExtra != null && unidadesExtra.length) lHopByUnidad = listarHorasOperacionByHopcodipadre(GLOBAL_HO.ListaHorasOperacion, regHO.Hopcodi, unidadesExtra[nh].Equicodi);

                                    for (var ihop = 0; ihop < lHopByUnidad.length; ihop++) {
                                        lHopByUnidad[ihop].Hophorini = moment(lHopByUnidad[ihop].Hophorini);
                                        lHopByUnidad[ihop].Hophorfin = moment(lHopByUnidad[ihop].Hophorfin);

                                        dateFrom = new Date(moment(lHopByUnidad[ihop].Hophorini));
                                        dateTo = new Date(moment(lHopByUnidad[ihop].Hophorfin));

                                        horaIni = (new Date(lHopByUnidad[ihop].Hophorini)).getHours();
                                        horaFin = (new Date(lHopByUnidad[ihop].Hophorfin)).getHours();
                                        minIni = (new Date(lHopByUnidad[ihop].Hophorini)).getMinutes();
                                        minFin = (new Date(lHopByUnidad[ihop].Hophorfin)).getMinutes();

                                        anchoDiv = _getAnchoDivBloqueHora(horaIni, minIni, horaFin, minFin);
                                        posXDiv = segmento + _getAnchoDivBloqueHora(0, 0, horaIni, minIni, true);  // 31px por cada celda de horas

                                        var enparalelo = `En paralelo: ${moment(dateFrom).format('HH:mm:ss')} \n`;
                                        var fueraparalelo = `Fuera de paralelo: ${moment(dateTo).format('HH:mm:ss')} \n`;

                                        var titleRangoUnidad = `${titlePrefijoFila}Unidad: ${lHopByUnidad[ihop].Equiabrev} \n\n${tipoOp}${enparalelo}${fueraparalelo}${sistemaAisladoDesc}${obsModo}\n${titleCongestion}${titleDesglose}`;

                                        cadena += `<div class='context-menu-one menu-1 horasoperacion ${claseBorderHoAislado}' `;
                                        cadena += `id='${id}' grupocodi="${objModo.Grupocodi}" title='${titleRangoUnidad}' style='width:`;
                                        cadena += anchoDiv + "px;left:" + posXDiv + "px;  background-color:" + color_ho + "; top:" + (posYDivEspecial) + "px; height: " + altura + "px;' />";
                                    }
                                }
                            }
                            else {
                                var titleRangoModo = `${titlePrefijoFila} \n${tipoOp}${enparalelo}${fueraparalelo}${sistemaAisladoDesc}${obsModo}\n${titleCongestion}${titleDesglose}`;

                                cadena += `<div class='context-menu-one menu-1 horasoperacion ${claseBorderHoAislado}' `;
                                cadena += `id='${id}' grupocodi="${objModo.Grupocodi}" title='${titleRangoModo}' style='width:`;
                                cadena += anchoDiv + "px;left:" + posXDiv + "px;  background-color:" + color_ho + "; top:" + (posYDiv) + "px; height: " + alturaHOP + "px;' />";
                            }

                            //agregar bloques de desglose / congestión
                            cadena = cadena + cadenaDes;

                            //Verificar si las HOP del modo tienen alerta scada
                            if (VALOR_ALERTA_SCADA_SI == regHO.TieneAlertaScada) {
                                objModo.ListaHopcodiScada.push(regHO.Hopcodi);
                            }
                            //Verificar si las HOP del modo tienen alerta ems
                            if (VALOR_ALERTA_EMS_SI == regHO.TieneAlertaEms) {
                                objModo.ListaHopcodiEms.push(regHO.Hopcodi);
                            }
                            //Verificar si las HOP del modo tienen alerta intervención
                            if (VALOR_ALERTA_INTERVENCION_SI == regHO.TieneAlertaIntervencion) {
                                objModo.ListaHopcodiIntervencion.push(regHO.Hopcodi);
                            }
                            //Verificar si las HOP del modo tienen alerta costo incremental
                            if (VALOR_ALERTA_COSTO_INCREMENTAL_SI == regHO.TieneAlertaCostoIncremental) {
                                objModo.ListaHopcodiCostoIncremental.push(regHO.Hopcodi);
                            }
                        }
                    }
                }

                //Columna 5 a 28: 24 horas
                for (var k = 0; k < 24; k++) {
                    cadena += "<td style='background-color: #ebedef;'> </td>"; //pinta vacio luego se superpone los divs de las lineas de codigo anteriores
                }

                //campos adicionales
                var comentarioCI = objModo.Comentario ?? '';
                if (comentarioCI != "") comentarioCI = `(${comentarioCI})`;
                var titleCI = titlePrefijoFila;
                if (objModo.FlagEncendido == FLAG_MODO_OPERO) {
                    titleCI = `${titlePrefijoFila} \n${tipoOPEncendido}${enparaleloEncendido}${fueraparaleloEncendido}${sisAisladoEncendido}${obsModoEncendido}`;
                }
                titleCI += `\nC.I.${objModo.NumTramo} (S/. / MWh): ${objModo.CIncremental} ${comentarioCI}`;

                //td ci
                cadena += `<td class='${claseFilaCIencendido}' title='${titleCI}' style='text-align: right !important;'>${objModo.CIncrementalFormateado ?? ''}</td>`;
                //td cv
                cadena += `<td class='${claseFilaCIencendido}' title='${titlePrefijoFila} \nC.V. (S/. / MWh): ${objModo.CVariable}' style='text-align: right !important;'>${objModo.CVariableFormateado ?? ''}</td>`;

                //agregados
                var HOPMin = objModo.PMin !== null ? objModo.PMin : '';
                var HOPEfe = objModo.PEfe !== null ? objModo.PEfe : '';
                var HOTMinO = objModo.TMinO !== null ? objModo.TMinO : '';
                var HOTMinA = objModo.TMinA !== null ? objModo.TMinA : '';
                var HOTParada = objModo.TParada !== null ? objModo.TParada : '';
                var HOTArranque = objModo.TArranque !== null ? objModo.TArranque : '';
                var titlePuedeOff = HOTParada != "" ? ` \nPuede parar: ${HOTParada}` : "";
                var titlePuedeOn = HOTArranque != "" ? ` \nPuede arrancar: ${HOTArranque}` : "";

                cadena += `<td class='${claseFilaCIencendido}' style='text-align: center;' title='${titlePrefijoFila} \nPmin (MW): ${HOPMin}' >${HOPMin}</td>`;
                cadena += `<td class='${claseFilaCIencendido}' style='text-align: center;' title='${titlePrefijoFila} \nPef (MW): ${HOPEfe}' >${HOPEfe}</td>`;
                cadena += `<td class='${claseFilaCIencendido}' style='text-align: center;' title='${titlePrefijoFila} \nTminO (h): ${HOTMinO}' >${HOTMinO}</td>`;
                cadena += `<td class='${claseFilaCIencendido}' style='text-align: center;' title='${titlePrefijoFila} \nT ÷ Arr (h): ${HOTMinA}' >${HOTMinA}</td>`;
                cadena += `<td class='${claseFilaCIencendido}' style='text-align: center;' title='${titlePrefijoFila} ${titlePuedeOff}' >${HOTParada}</td>`;
                cadena += `<td class='${claseFilaCIencendido}' style='text-align: center;' title='${titlePrefijoFila} ${titlePuedeOn}' >${HOTArranque}</td>`;

                var strAlerta = '';
                var titleAlerta = `${titlePrefijoFila} \n`;
                //campana scada o ems
                if (objModo.ListaHopcodiScada.length > 0 || objModo.ListaHopcodiEms.length > 0) {
                    if (objModo.ListaHopcodiEms.length > 0) {
                        var listaHop = objModo.ListaHopcodiEms.join(',');
                        strAlerta += `<div class='bellImgEms' onclick='verAlertaEmsXModo("${listaHop}")' />`;
                        titleAlerta += "Alerta EMS\n";
                    } else {
                        var listaHop = objModo.ListaHopcodiScada.join(',');
                        strAlerta += `<div class='bellImgScada' onclick='verAlertaScadaXModo("${listaHop}")' />`;
                        titleAlerta += "Alerta SCADA\n";
                    }
                }

                //campana costo incremental
                if (objModo.ListaHopcodiCostoIncremental.length > 0) {
                    var listaHop = objModo.ListaHopcodiCostoIncremental.join(',');
                    strAlerta += `<div class='bellImgCostoIncremental' onclick='verAlertaCostoIncrementalXModo("${listaHop}")' />`;
                    titleAlerta += "Alerta Costo incremental\n";
                }

                //campana intervencion
                if (objModo.ListaHopcodiIntervencion.length > 0) {
                    var listaHop = objModo.ListaHopcodiIntervencion.join(',');
                    strAlerta += `<div class='bellImgIntervencion' onclick='verAlertaIntervencionXModo("${listaHop}")' />`;
                    titleAlerta += "Alerta Intervención\n";
                }

                cadena += `<td class='alerta_ems' title='${titleAlerta}' >${strAlerta}</td>`;
                cadena += "</tr>";

                contadorModo++;
                if (objModo.FlagEncendido == FLAG_MODO_OPERO)
                    contadorModoEncendido++;
            }
        }

    }
    else {

        if (GLOBAL_HO.ListaHorasProgramadas.length == 0) {
            cadena += "<tr>";
            cadena += "<td class='alerta_encendido' style='background-color: white; padding-left: 5px;padding-right: 5px; padding-top: 2px; height: " + alturaTDbtn + "px!important;'></td>";
            cadena += "<td colspan='34'>&nbsp;</td>";
            cadena += "<td class='alerta_ems'></td>";
            cadena += "</tr>";
        }
    }

    //YUPANA (barras amarillas)
    if ($('#chkConsultarOtros').is(':checked')) {

        for (var i = 0; i < GLOBAL_HO.ListaModosOperacionProgramada.length; i++) {

            var objModo = GLOBAL_HO.ListaModosOperacionProgramada[i];

            claseFilaCIencendido = "";

            //Columna 1: ON / OFF
            cadenaHP += "<tr>";
            cadenaHP += "<td class='alerta_encendido' style='background-color: white; padding-left: 5px;padding-right: 5px; padding-top: 2px; height: " + alturaTDbtn + "px!important;'>";
            cadenaHP += "</td>";

            var titlePrefijoFila = `Horas programadas (Yupana) \n\nEmpresa: ${objModo.EmprNomb} \nCentral T: ${objModo.Central} \nModo Op: ${objModo.Gruponomb} (${objModo.Grupoabrev}) \n`;
            //Columna 2: EMPRESA
            cadenaHP += `<td class='${claseFilaCIencendido}' style='text-align: left;padding-left: 3px;' title='${titlePrefijoFila}' >${_textoTdGrafico(objModo.EmprNomb, WIDTH_COL_EMP)}</td>`;
            //Columna 3: CENTRAL
            cadenaHP += `<td class='${claseFilaCIencendido}' style='text-align: left;padding-left: 3px;' title='${titlePrefijoFila}' >${_textoTdGrafico(objModo.Central, WIDTH_COL_CENTRAL)}</td>`;
            //Columna 4: MODOS DE OPERACIÓN
            cadenaHP += `<td class='${claseFilaCIencendido}' style='' title='${titlePrefijoFila}' >${_textoTdGrafico(objModo.Grupoabrev, WIDTH_COL_GRUPO)}</td>`;

            var colorHP = "yellow";
            for (var z = 0; z < GLOBAL_HO.ListaHorasProgramadas.length; z++) {
                var horaProgramada = GLOBAL_HO.ListaHorasProgramadas[z];

                if (horaProgramada.Grupocodi == objModo.Grupocodi) {
                    /// Horas Programadas
                    var id = "hopGraf" + horaProgramada.Hopcodi;
                    if ((horaProgramada.HoraInicio != null) && (horaProgramada.HoraFin != "")) {

                        var fechaIni = new Date(horaProgramada.HoraInicio);
                        var fechaFin = new Date(horaProgramada.HoraFin);

                        var enparalelo = `Hora Inicio: ${moment(fechaIni).format('HH:mm:ss')} \n`;
                        var fueraparalelo = `Hora Final: ${moment(fechaFin).format('HH:mm:ss')} \n`;
                        var tituloDes = `${titlePrefijoFila} \n${enparalelo}${fueraparalelo}`;

                        var horaIniDes = fechaIni.getHours();
                        var horaFinDes = fechaFin.getHours();
                        var minIniDes = fechaIni.getMinutes();
                        var minFinDes = fechaFin.getMinutes();

                        anchoDivDesglose = _getAnchoDivBloqueHora(horaIniDes, minIniDes, horaFinDes, minFinDes);

                        if (anchoDivDesglose > 0) {

                            posXDivDes = segmento + _getAnchoDivBloqueHora(0, 0, horaIniDes, minIniDes, true);  // 31px por cada celda de horas
                            posYDiv = (alturaTD * contadorModo) + alturaTh + paddingTop;

                            cadenaHP += `<div class='context-menu-hp dv-horasprog horasoperacion'`;
                            cadenaHP += `id="${id}-hp" editable="${horaProgramada.EsEdicion}" grupocodi="${horaProgramada.Grupocodi}" data-inicio="${horaProgramada.HoraInicio.substring(11)}" data-fin="23:58:00" title="${tituloDes}" style="z-index:;width:`;
                            cadenaHP += anchoDivDesglose + `px;left:${posXDivDes}px;background-color:${colorHP};top:${posYDiv}px; height:${alturaHOP}px;" />`;
                        }
                    }
                }
            }

            contadorModo++;

            //las 24 horas
            for (var k = 0; k < 24; k++) {
                cadenaHP += "<td style='background-color: #ebedef;'> </td>"; //pinta vacio luego se superpone los divs de las lineas de codigo anteriores
            }

            //campos adicionales
            var comentarioCI = objModo.Comentario ?? '';
            if (comentarioCI != "") comentarioCI = `(${comentarioCI})`;
            var titleCI = titlePrefijoFila;
            titleCI += `\nC.I.${objModo.NumTramo} (S/. / MWh): ${objModo.CIncremental} ${comentarioCI}`;

            //td ci
            cadenaHP += `<td class='${claseFilaCIencendido}' title='${titleCI}' style='text-align: right !important;'>${objModo.CIncrementalFormateado ?? ''}</td>`;
            //td cv
            cadenaHP += `<td class='${claseFilaCIencendido}' title='${titlePrefijoFila} \nC.V. (S/. / MWh): ${objModo.CVariable}' style='text-align: right !important;'>${objModo.CVariableFormateado ?? ''}</td>`;

            //agregados
            var HOPMin = objModo.PMin !== null ? objModo.PMin : '';
            var HOPEfe = objModo.PEfe !== null ? objModo.PEfe : '';
            var HOTMinO = objModo.TMinO !== null ? objModo.TMinO : '';
            var HOTMinA = objModo.TMinA !== null ? objModo.TMinA : '';
            var HOTParada = '';
            var HOTArranque = '';

            cadenaHP += `<td class='${claseFilaCIencendido}' style='text-align: center;' title='${titlePrefijoFila} \nPmin (MW): ${HOPMin}' >${HOPMin}</td>`;
            cadenaHP += `<td class='${claseFilaCIencendido}' style='text-align: center;' title='${titlePrefijoFila} \nPef (MW): ${HOPEfe}' >${HOPEfe}</td>`;
            cadenaHP += `<td class='${claseFilaCIencendido}' style='text-align: center;' title='${titlePrefijoFila} \nTminO (h): ${HOTMinO}' >${HOTMinO}</td>`;
            cadenaHP += `<td class='${claseFilaCIencendido}' style='text-align: center;' title='${titlePrefijoFila} \nT ÷ Arr (h): ${HOTMinA}' >${HOTMinA}</td>`;
            cadenaHP += `<td class='${claseFilaCIencendido}' style='text-align: center;' title='${titlePrefijoFila} \n' >${HOTParada}</td>`;
            cadenaHP += `<td class='${claseFilaCIencendido}' style='text-align: center;' title='${titlePrefijoFila} \n' >${HOTArranque}</td>`;

            cadenaHP += `<td class='alerta_ems'></td>`;
            cadenaHP += "</tr>";
        }
    }

    cadena += cadenaHP;

    cadena += "</tbody>";
    cadena += "</table>";

    // Linea Fecha Actual (Verde)
    cadena += generarLineaVertical(TIPO_LINEA_ACTUAL, fecha, contadorModo, alturaTh, anchoTh, alturaTD, segmento);

    // Linea Fecha de consulta para Costos incrementales (Azul)
    cadena += generarLineaVertical(TIPO_LINEA_COSTO_INCREMENTAL, fecha, contadorModoEncendido, alturaTh, anchoTh, alturaTD, segmento);

    // Linea Flecha (Rojo)
    cadena += generarLineaVertical(TIPO_LINEA_FLECHA, fecha, contadorModoEncendido, alturaTh, anchoTh, alturaTD, segmento);

    return cadena;
}

function generaViewListaUnidadesNoRegistradasEms(fechaEms) {
    return _generaViewListaUnidadesNoRegistradasEquipos(fechaEms, '#tablaUnidNoRegEMS', GLOBAL_HO.ListaUnidadesNoRegistradasEMS, GLOBAL_HO.ListaHOPUnidadesNoRegistradasEMS);
}

function generaViewListaUnidadesNoRegistradasScada(fechaEms) {
    return _generaViewListaUnidadesNoRegistradasEquipos(fechaEms, '#tablaUnidNoRegScada', GLOBAL_HO.ListaUnidadesNoRegistradasScada, GLOBAL_HO.ListaHOPUnidadesNoRegistradasScada);
}

function _generaViewListaUnidadesNoRegistradasEquipos(fecha, idTabla, listaUnidadesNoRegistradas, listaHorasOperacionUnidadesNoRegistradas) {
    var strHtml = "";
    strHtml += `<table id='${idTabla}' class='pretty tabla-horas' style='width: ${TAMANIO_TABLA_EMS_SCADA}px'>`;
    strHtml += "<thead>";

    //INICIO CABECERA
    var alturaTh = 12 + HEIGTH_BORDER_CELDA_TD;
    var segmento = WIDTH_COL_ESTADO_GRUPO + WIDTH_COL_EMP + WIDTH_COL_CENTRAL + WIDTH_COL_GRUPO + HEIGTH_BORDER_CELDA_TD * 5; //ancho anterior al inicio de la primera hora , incluye border 1 de cada celda

    var cadena = "<tr> ";

    //
    cadena += "<th class='alerta_encendido' style='width:" + WIDTH_COL_ESTADO_GRUPO + "px; height: " + alturaTh + "px;'></th> ";
    cadena += "<th class='' style='width:" + WIDTH_COL_EMP + "px'>EMPRESA</th> ";
    cadena += "<th class='' style='width:" + WIDTH_COL_CENTRAL + "px'>CENTRAL</th> ";
    cadena += "<th class='' style='width:" + WIDTH_COL_GRUPO + "px'>UNIDAD</th> ";
    for (var i = 0; i < 24; i++) {
        cadena += "<th class='' style='width:" + WIDTH_COL_HORA + "px'>" + i + "</th>";
    }

    cadena += "<th class='alerta_ems' style=''></th> ";
    cadena += "</tr></thead>";
    strHtml += cadena;

    //INICIO CUERPO
    var cadenaTermo = "<tbody>";

    var alturaTD = 22 + HEIGTH_BORDER_CELDA_TD;
    var alturaTDbtn = 20;
    var paddingTop = 2;
    var alturaHOP = 19;
    var anchoTh = WIDTH_COL_HORA + HEIGTH_BORDER_CELDA_TD;

    //        
    if (listaUnidadesNoRegistradas.length > 0) {
        for (var i = 0; i < listaUnidadesNoRegistradas.length; i++) {
            var unidadActual = listaUnidadesNoRegistradas[i];

            var titlePrefijoFila = `Empresa: ${unidadActual.Emprnomb} \nCentral T: ${unidadActual.Central} \nUnidad: ${unidadActual.Equiabrev} \n`;

            cadenaTermo += "<tr>";
            cadenaTermo += `<td title='${titlePrefijoFila}' class='alerta_encendido' style='background-color: white; padding-left: 5px;padding-right: 5px; padding-top: 2px; height: ${alturaTDbtn}px!important;'></td>`;
            cadenaTermo += `<td title='${titlePrefijoFila}' style='text-align: left;padding-left: 3px; ' >${_textoTdGrafico(unidadActual.Emprnomb, WIDTH_COL_EMP)}</td>`;
            cadenaTermo += `<td title='${titlePrefijoFila}' style='text-align: left;padding-left: 3px; ' >${_textoTdGrafico(unidadActual.Central, WIDTH_COL_CENTRAL)}</td>`;
            cadenaTermo += `<td title='${titlePrefijoFila}' style='' >${_textoTdGrafico(unidadActual.Equiabrev, WIDTH_COL_GRUPO)}</td>`;

            if (listaHorasOperacionUnidadesNoRegistradas.length > 0) { //si hay horas de operacion                              
                for (var z = 0; z < listaHorasOperacionUnidadesNoRegistradas.length; z++) {
                    var hopActual = listaHorasOperacionUnidadesNoRegistradas[z];

                    //***convertimos los datos tipo /Date(99999999999)/ a Date
                    hopActual.FechaIni = moment(hopActual.FechaIni);
                    hopActual.FechaFin = moment(hopActual.FechaFin);

                    //Todas las horas de operación del día por modo de operacion
                    if (hopActual.Equicodi == unidadActual.Equicodi) {
                        var dateFrom = new Date(moment(hopActual.FechaIni));
                        var dateTo = new Date(moment(hopActual.FechaFin));

                        var isDateNow = false;
                        var fechaActual = new Date();
                        // comprobar si una fecha actual es anterior a la barra de hora de operación
                        if (!moment(fechaActual).isBetween(dateFrom, dateTo) && moment(fechaActual).isBefore(dateFrom)) {
                            isDateNow = true;
                        }
                        var horaIni = (new Date(hopActual.FechaIni)).getHours();
                        var horaFin = (new Date(hopActual.FechaFin)).getHours();
                        var minIni = (new Date(hopActual.FechaIni)).getMinutes();
                        var minFin = (new Date(hopActual.FechaFin)).getMinutes();

                        var anchoDiv = _getAnchoDivBloqueHora(horaIni, minIni, horaFin, minFin);
                        var posXDiv = segmento + _getAnchoDivBloqueHora(0, 0, horaIni, minIni, true);  // 31px por cada celda de horas
                        var posYDiv = alturaTD * (i) + alturaTh + paddingTop;
                        var id = hopActual.Equicodi;

                        var color_ho = "#A9A9A9";
                        if (isDateNow) {// si hora de operacion es posterior a la fecha actual cambiar color 
                            color_ho = "#A9A9A9"; //DarkGray 
                        }

                        var horaInicio = moment(dateFrom).format('HH:mm:ss');
                        var horaFin = moment(dateTo).format('HH:mm:ss');

                        var titleHora = `${titlePrefijoFila} \nHora Inicio: ${horaInicio} \nHora Final: ${horaFin}`;

                        var eventos2 = "";
                        cadenaTermo += "<div " + eventos2;
                        cadenaTermo += "id='horGraf" + id + "' title='" + titleHora + "' class='context-menu-unr horasoperacion'";

                        cadenaTermo += " data-inicio=\"" + horaInicio + "\" data-fin=\"" + horaFin + "\"";
                        cadenaTermo += ` data-empresa='${listaUnidadesNoRegistradas[i].Emprcodi}' data-central='${listaUnidadesNoRegistradas[i].Equipadre}' `;

                        cadenaTermo += " style='width:" + anchoDiv + "px;left:" + (posXDiv) + "px;  background-color:" + color_ho + "; top:" + (posYDiv) + "px; height: " + alturaHOP + "px;' />";
                    }
                }
            }

            //las 24 horas
            for (var k = 0; k < 24; k++) {
                cadenaTermo += "<td style='background-color: #ebedef;'> </td>"; //pinta vacio                
            }

            cadenaTermo += "<td class='alerta_ems'></td>";
            cadenaTermo += "</tr>";
        }
    } else {
        cadenaTermo += "<tr>";
        cadenaTermo += "<td class='alerta_encendido' style='background-color: white; padding-left: 5px;padding-right: 5px; padding-top: 2px; height: " + alturaTDbtn + "px!important;'></td>";
        cadenaTermo += "<td colspan='27'>&nbsp;</td>";
        cadenaTermo += "<td class='alerta_ems'></td>";
        cadenaTermo += "</tr>";
    }

    cadenaTermo += "</tbody>";
    cadenaTermo += "</table>";
    cadenaTermo += generarLineaVertical(TIPO_LINEA_ACTUAL, fecha, listaUnidadesNoRegistradas.length, alturaTh, anchoTh, alturaTD, segmento);

    strHtml += cadenaTermo;

    //
    strHtml += "<div style='clear:both; height:5px'></div>";
    strHtml += "<div class='form-title_intranet'>";
    return strHtml;
}

function generarLineaVertical(tipoLinea, fecha, numeroFilas, alturaTh, anchoTh, alturaTD, segmento) {
    if (numeroFilas == 0)
        return '';

    var str = '';
    var tamvertical = alturaTh;
    tamvertical += (alturaTD * numeroFilas);

    var fechaActual;
    var sFecha;

    switch (tipoLinea) {
        case TIPO_LINEA_ACTUAL: //fecha y hora actual TR. Linea roja
        case TIPO_LINEA_COSTO_INCREMENTAL: //fecha y hora actual TR. Linea verde de modos de operación encendidos

            var claseLinea = tipoLinea == TIPO_LINEA_ACTUAL ? 'vertical-line-tr' : 'vertical-line-ci';

            var horaMin = getValorHoraMinToConsulta();
            var fechaConsulta = convertStringToDate(fecha, horaMin + ":00");

            var horaIni = fechaConsulta.getHours();
            var minIni = fechaConsulta.getMinutes();
            var desplazamiento = segmento + _getAnchoDivBloqueHora(0, 0, horaIni, minIni, true);
            var sFechaConsultaFormato = moment(fechaConsulta).format('DD/MM/YYYY') + " " + moment(fechaConsulta).format('HH:mm:ss');

            str += "<div title='Fecha de consulta: " + sFechaConsultaFormato + "' class='" + claseLinea + "' style='height:" + (tamvertical) + "px;left:" + (desplazamiento) + "px; position: absolute;' />";

            break;

        case TIPO_LINEA_FLECHA: //flecha

            var tamvertical = alturaTD * numeroFilas + 5;
            var desplazamiento = segmento + _getAnchoDivBloqueHora(0, 0, 23, 59, true) + WIDTH_COL_CI + HEIGTH_BORDER_CELDA_TD * 4;

            if (ORDEN_GRAFICO_CAMPO == ORDEN_GRAFICO_CAMPO_CV) {
                desplazamiento += WIDTH_COL_CV;
            }

            var classFlecha = "flecha_abajo";
            var topFlecha = alturaTh + tamvertical - 8;

            if (ORDEN_GRAFICO_TIPO == 'desc') {
                classFlecha = "flecha_arriba";
                topFlecha = 16;
            }

            var desplFlecha = desplazamiento - 6;
            str += "<div title='" + TITLE_FLECHA_CI + "' class='" + classFlecha + "' style='top:" + (topFlecha) + "px;left:" + (desplFlecha) + "px; position: absolute;' />";
            str += "<div title='" + TITLE_FLECHA_CI + "' class='vertical-line-flecha' style='top:" + alturaTh + "px;height:" + (tamvertical - 8) + "px;left:" + (desplazamiento) + "px; position: absolute;' />";

            break;

    }

    return str;
}

function _textoTdGrafico(texto, anchoTd) {
    var anchoVisible = anchoTd - 7;

    var html = `<div style="width: ${anchoVisible}px;overflow:hidden;white-space:nowrap;">${texto}</div>`;
    return html;
}

function _getAnchoDivBloqueHora(horaIni, minIni, horaFin, minFin, flagPosicionDespuesSegmento) {

    if (flagPosicionDespuesSegmento) {
        //no hacer nada
    }
    else {
        if (horaFin == 0 && minFin == 0) {//artificio para mostrar la hora fin 00:00 como 23:59
            horaFin = 23;
            minFin = 59;
        }
    }

    var anchoDiv = Math.round(((horaFin + minFin / 60) - (horaIni + minIni / 60)) * (WIDTH_COL_HORA)); //ancho sin el border

    if (flagPosicionDespuesSegmento) {
        //agregar el border de las horas anteriores
        anchoDiv += HEIGTH_BORDER_CELDA_TD * (horaFin);
    } else {
        if ((horaFin - horaIni) > 1) {
            //bloque de hora de operación ejecutada
            anchoDiv += HEIGTH_BORDER_CELDA_TD * (horaFin - horaIni + 1); //adicionar el width de los borders que estan adentro del bloque
        }

        //quitar el border de la izquierda y derecha para que no aumente el ancho del bloque
        anchoDiv -= HEIGTH_BORDER_CELDA_TD * 2;
    }

    return anchoDiv;
}

function grafico_displayDiv(css) {
    $("." + css).toggle('display');
}

//////////////////////////////////////////////////////////////////////////////////////////
/// Útil
//////////////////////////////////////////////////////////////////////////////////////////
function getColorHOP(subcausacodi) {

    var colorHop = "";
    var lista = GLOBAL_HO.ListaTipoOperacion;
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].Subcausacodi == parseInt(subcausacodi)) {
            colorHop = lista[i].Subcausacolor;
        }
    }

    return colorHop
}

function verificaEquipoencendidoTermico(Grupocodi) {
    var fechaListado = $("#txtFecha").val();
    var fechaActualFormato = obtenerDiaActualFormato();

    //solo verificar para el día actual
    if (fechaActualFormato == fechaListado) {
        var fechaActual = new Date();
        var hayHoraOpXGrupocodi = false;

        if (GLOBAL_HO.ListaHorasOperacion.length > 0) {
            for (var j = 0; j < GLOBAL_HO.ListaHorasOperacion.length; j++) {
                if (GLOBAL_HO.ListaHorasOperacion[j].Grupocodi == Grupocodi && GLOBAL_HO.ListaHorasOperacion[j].OpcionCrud != -1) {
                    var dateFrom = new Date(moment(GLOBAL_HO.ListaHorasOperacion[j].Hophorini));
                    var dateTo = new Date(moment(GLOBAL_HO.ListaHorasOperacion[j].Hophorfin));
                    if (moment(fechaActual).isBetween(dateFrom, dateTo))
                        return 1;

                    hayHoraOpXGrupocodi = true;
                }
            }


            if (!hayHoraOpXGrupocodi) {//si no existe horas de operacion para tal equicodi buscar en las horas de operacion del día anterior
                if (GLOBAL_HO.ListaHorasOperacionAnt.length > 0) { //si hay horas de operacion el dia anterior
                    for (var i = 0; i < GLOBAL_HO.ListaHorasOperacionAnt.length; i++) {
                        if (GLOBAL_HO.ListaHorasOperacionAnt[i].Grupocodi == Grupocodi && GLOBAL_HO.ListaHorasOperacionAnt[i].OpcionCrud != -1) {
                            var horaFin = moment(GLOBAL_HO.ListaHorasOperacionAnt[i].Hophorfin).format('HH:mm');
                            if (horaFin == '00:00') {//if (horaFin == '23:59') {
                                return 1;
                            }
                        }
                    }
                }
            }
        }
        else {
            if (GLOBAL_HO.ListaHorasOperacionAnt.length > 0) { //si hay horas de operacion el dia anterior
                for (var i = 0; i < GLOBAL_HO.ListaHorasOperacionAnt.length; i++) {
                    if (GLOBAL_HO.ListaHorasOperacionAnt[i].Grupocodi == Grupocodi && GLOBAL_HO.ListaHorasOperacionAnt[i].OpcionCrud != -1) {
                        var horaFin = moment(GLOBAL_HO.ListaHorasOperacionAnt[i].Hophorfin).format('HH:mm');
                        if (horaFin == '00:00') {//if (horaFin == '23:59') {
                            return 1;
                        }
                    }
                }
            }
        }
    }
    return 0;
}

function nrofilasCentral(icentral, ListaGrupo) {
    var i = 0;
    if (ListaGrupo.length > 0) {
        for (var j = 0; j < ListaGrupo.length; j++) {

            if (ListaGrupo[j].Equipadre == icentral) {
                i++;
            }
        }
        return i;
    }
    return i++;
}

function changeBgcolor(codiEquipo, hopcodi) {
    var idEqui = codiEquipo + "_" + hopcodi
    var fila = document.getElementById(idEqui);
    fila.className = null;
}

function resaltarFilareporte(codiEquipo, hopcodi) {
    var idEqui = codiEquipo + "_" + hopcodi
    var fila = document.getElementById(idEqui);
    fila.className = "resaltar";
}

function darFormatoNumero(valor) {
    if (!esNumero(valor)) {
        return "";
    } else {
        // Convierte reg.PMin a un número decimal y lo trunca a 2 decimales.
        return parseFloat(valor).toFixed(2);
    }
}

function esNumero(valor) {
    // Utilizamos la funcion parseFloat para intentar convertir la cadena a un número decimal.
    // Si el resultado es falso se devuelve un NaN.
    return !isNaN(parseFloat(valor));
}