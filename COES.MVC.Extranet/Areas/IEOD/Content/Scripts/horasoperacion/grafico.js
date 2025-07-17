
function dibujarTablaHorasOperacion(evt, opcion) {
    var tipoCentral = evt.IdTipoCentral;
    var fecha = $('#txtFecha').val();
    var strHtml = "";

    switch (tipoCentral) {
        case 4: //Hidraulicas  
            strHtml += generaViewListaHidraulicas(evt, fecha);
            break;
        case 5: //Térmicas
            strHtml += generaViewListaTermicas(evt, fecha);
            break;
        case 37: //Solares
        case 39: //Eolicas
            strHtml += generaViewListaSolarEolica(evt, fecha);
            break;
    }

    strHtml += "<div style='clear:both; height:30px'></div>";
    strHtml += "<div class='form-title_intranet'>";
    strHtml += "<div class='popup-title'><span>Reporte de Envío Horas de Operación</span></div></div>";
    strHtml += generaEnvioHorasOperacion(opcion);

    return strHtml;
}

//////////////////////////////////////////////////////////////////////////////////////////
/// Gráfico 24h
//////////////////////////////////////////////////////////////////////////////////////////

function generaViewListaHidraulicas(evt, fecha) {
    var esEditable = parseInt($("#hfIdEnvio").val()) == 0 && evtHot.PlazoEnvio.TipoPlazo != "D";

    var alturaCol = 12;
    var padding = 5 * 2;
    var tamanio = TAMANIO_GRAFICO;
    var tamanioCol = 32; //h0,h23
    var tamanioDescripcion = tamanio - (tamanioCol * (24));


    var cadena = '';
    cadena += "<table  class='pretty tabla-horas' style='width: " + (TAMANIO_TABLA_GRAFICO) + "px' >";

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Cabecera

    cadena += "<thead><tr> ";
    var tamanioCentral = parseInt(tamanioDescripcion / 3);
    tamanioDescripcion -= tamanioCentral;
    cadena += "<th class='' style='width:" + (tamanioCentral - padding) + "px'>CENTRAL</th> ";
    cadena += "<th class='' style='width:" + (tamanioCol - padding) + "px; height: " + alturaCol + "px;'></th> ";
    tamanioDescripcion -= (tamanioCentral - tamanioCol);
    cadena += "<th class='' style='width:" + (tamanioDescripcion - padding) + "px'>GRUPO</th> ";

    segmento = tamanioCentral + tamanioCol + tamanioDescripcion + padding / 3;
    for (var i = 0; i < 24; i++) {
        cadena += "<th class=''  style='width:" + (tamanioCol - padding) + "px'>" + i + "</th>";
    }

    cadena += "</tr></thead>";

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Linea Fecha Actual
    var alturaTD = 23;
    var alturaTDbtn = 20;
    var paddingTop = 5;
    var alturaHOP = 18;
    var alturaTh = 23;
    var anchoTh = tamanioCol;
    var paddingRight = 3;

    var fechaActual = new Date();
    var sFecha = convertStringToDate(fecha, "00:00:00");

    var tamvertical = alturaTh;
    tamvertical += alturaTD * (evt.ListaGrupo.length) + (paddingTop - 2);

    if (moment(moment(fechaActual).format('YYYY-MM-DD')).isSame(moment(sFecha).format('YYYY-MM-DD'))) {
        var horaIni = fechaActual.getHours();
        var minIni = fechaActual.getMinutes();
        var desplazamiento = horaIni + horaIni * anchoTh + Math.round((minIni / 60) * anchoTh) + segmento;
        cadena += "<div title='" + fechaActual.toLocaleString('en-ES') + "' class='vertical-line' style='height:" + tamvertical + "px;left:" + desplazamiento + "px; position: absolute;' />";
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Cuerpo

    cadena += "<tbody>";

    if (evt.ListaCentrales.length > 0) {//(1*)
        for (var i = 0; i < evt.ListaCentrales.length; i++) { // for (2*)
            var icentral = evt.ListaCentrales[i].Equicodi;
            var flag = 0;
            if (evt.ListaGrupo.length > 0) { //   if (3*)
                for (var j = 0; j < evt.ListaGrupo.length; j++) {// for (4*)

                    if (evt.ListaGrupo[j].Equipadre == icentral) {// if (5*)
                        grupo = evt.ListaGrupo[j].Equinomb;
                        cadena += "<tr>";
                        if (flag == 0) {
                            cadena += "<td rowspan=" + nrofilasCentral(icentral, evt.ListaGrupo) + " style='white-space: initial;'><b>" + evt.ListaCentrales[i].Equinomb + "</b></td>";
                        }
                        flag = 1;
                        //** Imprime boton on-off
                        cadena += "<td style='background-color: white; padding-left: 5px;padding-right: 5px; padding-top: 3px;'>";
                        var equipoencendido = verificaEquipoencendido(evt.ListaGrupo[j].Equicodi);
                        if (equipoencendido == 1) // encendido
                        { cadena += "<img src='" + siteRoot + "Areas/IEOD/Content/Images/btn_on16x16.jpg'/>"; }
                        else // apagado
                        { cadena += "<img src='" + siteRoot + "Areas/IEOD/Content/Images/btn_off16x16.jpg'/>"; }

                        cadena += "</td>";
                        //*****
                        cadena += "<td>" + evt.ListaGrupo[j].Equinomb + "</td>";

                        // ***Impresion de horas de operacion


                        if (evt.ListaHorasOperacion.length > 0) { //si hay horas de operacion   if (6*)                            
                            for (var z = 0; z < evt.ListaHorasOperacion.length; z++) {
                                //***convertimos los datos tipo /Date(99999999999)/ a Date
                                evt.ListaHorasOperacion[z].Hophorini = moment(evt.ListaHorasOperacion[z].Hophorini);
                                evt.ListaHorasOperacion[z].Hophorfin = moment(evt.ListaHorasOperacion[z].Hophorfin);

                                //********
                                if (evt.ListaHorasOperacion[z].Equicodi == evt.ListaGrupo[j].Equicodi && evt.ListaHorasOperacion[z].OpcionCrud != -1) {

                                    var dateFrom = new Date(moment(evtHot.ListaHorasOperacion[z].Hophorini));
                                    var dateTo = new Date(moment(evtHot.ListaHorasOperacion[z].Hophorfin));
                                    isDateNow = false;
                                    var fechaActual = new Date();

                                    // comprobar si una fecha actual es anterior a la barra de hora de operación
                                    if (!moment(fechaActual).isBetween(dateFrom, dateTo) && moment(fechaActual).isBefore(dateFrom)) {
                                        isDateNow = true;
                                    }

                                    var horaIni = (new Date(evt.ListaHorasOperacion[z].Hophorini)).getHours();// en formato 24H
                                    var horaFin = (new Date(evt.ListaHorasOperacion[z].Hophorfin)).getHours();// en formato 24H
                                    var minIni = (new Date(evt.ListaHorasOperacion[z].Hophorini)).getMinutes();
                                    var minFin = (new Date(evt.ListaHorasOperacion[z].Hophorfin)).getMinutes();

                                    ///********* genera div para la hora de operacion********************************
                                    if (horaFin == 0 && minFin == 0) {//artificio para mostrar la hora fin 00:00 como 23:59
                                        horaFin = 23;
                                        minFin = 59;
                                    }
                                    var anchoDiv = Math.round(((horaFin + minFin / 60) - (horaIni + minIni / 60)) * anchoTh) + (horaFin - horaIni) - 3;
                                    var posXDiv = horaIni + horaIni * anchoTh + Math.round((minIni / 60) * anchoTh) + segmento;  // 31px por cada celda de horas
                                    var posYDiv = alturaTD * (j) + alturaTh + paddingTop;

                                    var id = evt.ListaHorasOperacion[z].Hopcodi;

                                    var color_ho = getColorHOP(evt.ListaHorasOperacion[z]);

                                    if (isDateNow) {// si hora de operacion es posterior a la fecha actual cambiar color 
                                        color_ho = "#A9A9A9"; //DarkGray 
                                    }

                                    enparalelo = moment(dateFrom).format('HH:mm:ss');
                                    fueraparalelo = moment(dateTo).format('HH:mm:ss');
                                    var claseEditDelHop = esEditable ? "context-menu-one" : "";
                                    cadena += "<div  onmouseover='resaltarFilareporte(" + evt.ListaHorasOperacion[z].Equicodi + "," + evt.ListaHorasOperacion[z].Hopcodi + ");' onMouseOut='changeBgcolor(" + evt.ListaHorasOperacion[z].Equicodi + "," + evt.ListaHorasOperacion[z].Hopcodi;
                                    cadena += ");' id='" + id + "' title='En Paralelo: " + enparalelo + " - Fuera Paralelo: " + fueraparalelo;
                                    cadena += "' class='" + claseEditDelHop + " menu-1 horasoperacion' style='width:" + anchoDiv + "px;left:" + posXDiv + "px;  background-color:" + color_ho + "; top:" + posYDiv + "px; height: " + alturaHOP + "px;' />";
                                    //*******************************************************************************

                                }
                            }
                        }

                        for (var k = 0; k < 24; k++) {
                            cadena += "<td></td>"; //pinta vacio                            
                        }
                        cadena += "</tr>";
                    }
                }
            }
        }
    }
    else {
        cadena += "<td  style='text-align:center'>No existen registros.</td>";
    }
    cadena += "</tbody></table>";

    return cadena;
}

function generaViewListaTermicas(evt, fecha) {
    var esEditable = parseInt($("#hfIdEnvio").val()) == 0 && evtHot.PlazoEnvio.TipoPlazo != "D";

    var cadena = "";

    cadena += "<table  class='pretty tabla-horas' style='width: " + (TAMANIO_TABLA_GRAFICO) + "px' >";

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Cabecera

    cadena += "<thead>";
    var alturaCol = 12;
    var padding = 5 * 2;
    var tamanio = TAMANIO_GRAFICO;
    var tamanioCol = 32; //h0,h23
    var tamanioDescripcion = tamanio - (tamanioCol * (24));
    cadena += "<tr> ";

    var tamanioModo = parseInt(tamanioDescripcion / 3);
    tamanioDescripcion -= tamanioModo;
    cadena += "<th class='' style='width:" + (tamanioModo - padding) + "px'></th> ";
    cadena += "<th class='' style='width:" + (tamanioCol - padding) + "px; height: " + alturaCol + "px;'></th> ";
    tamanioDescripcion -= (tamanioModo - tamanioCol);
    cadena += "<th class='' style='width:" + (tamanioDescripcion - padding) + "px'></th> ";

    segmento = tamanioModo + tamanioCol + tamanioDescripcion + padding / 3;
    for (var i = 0; i < 24; i++) {
        cadena += "<th class=''  style='width:" + (tamanioCol - padding) + "px !important'>" + i + "</th>";
    }

    cadena += "</tr></thead>";

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Cuerpo

    var alturaTD = 23;
    var alturaTDbtn = 20;
    var paddingTop = 5;
    var alturaHOP = 18;
    var alturaTh = 23;
    var anchoTh = tamanioCol;
    var paddingRight = 3;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Modos de Operación

    var flag = 0;
    var contadorModo = 0;
    if (evt.ListaModosOperacion.length > 0) {
        for (var i = 0; i < evt.ListaModosOperacion.length; i++) {
            cadena += "<tr>";
            if (flag == 0) {
                cadena += "<td rowspan=" + evt.ListaModosOperacion.length + " style='background-color:  #ebedef;'><b>MODO OP</b></td>";
            }
            flag = 1;
            //** Imprime boton on-off
            cadena += "<td style='background-color: #ebedef; padding-left: 5px; padding-right: 5px; padding-top: 3px;'>";
            var grupoencendido = verificaEquipoencendidoTermico(evt.ListaModosOperacion[i].Grupocodi);
            if (grupoencendido == 1) // encendido
            { cadena += "<img src='" + siteRoot + "Areas/IEOD/Content/Images/btn_on16x16.jpg'/>"; }
            else // apagado
            { cadena += "<img src='" + siteRoot + "Areas/IEOD/Content/Images/btn_off16x16.jpg'/>"; }
            cadena += "</td>";

            cadena += "<td style='background-color:  #ebedef; height: " + alturaTDbtn + "px!important;'>" + evt.ListaModosOperacion[i].Grupoabrev + "</td>";
            var modo = evt.ListaModosOperacion[i].Grupoabrev;
            //**** if lista modos de operacion
            // ***aca se debe imprimir las horas de operacion            
            if (evt.ListaHorasOperacion.length > 0) { //si hay horas de operacion                              
                for (var z = 0; z < evt.ListaHorasOperacion.length; z++) {
                    //***convertimos los datos tipo /Date(99999999999)/ a Date
                    evt.ListaHorasOperacion[z].Hophorini = moment(evt.ListaHorasOperacion[z].Hophorini);
                    evt.ListaHorasOperacion[z].Hophorfin = moment(evt.ListaHorasOperacion[z].Hophorfin);

                    //********
                    if (evt.ListaHorasOperacion[z].Grupocodi == evt.ListaModosOperacion[i].Grupocodi && evt.ListaHorasOperacion[z].OpcionCrud != -1 && evt.ListaHorasOperacion[z].Equipadre == evt.ListaModosOperacion[i].Equipadre) {
                        var dateFrom = new Date(moment(evt.ListaHorasOperacion[z].Hophorini));
                        var dateTo = new Date(moment(evt.ListaHorasOperacion[z].Hophorfin));
                        var isDateNow = false;
                        var fechaActual = new Date();
                        // comprobar si una fecha actual es anterior a la barra de hora de operación
                        if (!moment(fechaActual).isBetween(dateFrom, dateTo) && moment(fechaActual).isBefore(dateFrom)) {
                            isDateNow = true;
                        }
                        var horaIni = (new Date(evt.ListaHorasOperacion[z].Hophorini)).getHours();
                        var horaFin = (new Date(evt.ListaHorasOperacion[z].Hophorfin)).getHours();
                        var minIni = (new Date(evt.ListaHorasOperacion[z].Hophorini)).getMinutes();
                        var minFin = (new Date(evt.ListaHorasOperacion[z].Hophorfin)).getMinutes();

                        ///********* genera div para la hora de operacion********************************
                        if (horaFin == 0 && minFin == 0) {//artificio para mostrar la hora fin 00:00 como 23:59
                            horaFin = 23;
                            minFin = 59;
                        }

                        var anchoDiv = Math.round(((horaFin + minFin / 60) - (horaIni + minIni / 60)) * anchoTh) + (horaFin - horaIni) - 2;
                        var posXDiv = horaIni + horaIni * anchoTh + Math.round((minIni / 60) * anchoTh) + segmento;  // 31px por cada celda de horas
                        var posYDiv = alturaTD * (contadorModo) + alturaTh + paddingTop;
                        var id = evt.ListaHorasOperacion[z].Hopcodi;

                        var color_ho = getColorHOP(evt.ListaHorasOperacion[z]);
                        if (isDateNow) {// si hora de operacion es posterior a la fecha actual cambiar color 
                            color_ho = "#A9A9A9"; //DarkGray 
                        }

                        enparalelo = moment(dateFrom).format('HH:mm:ss');
                        fueraparalelo = moment(dateTo).format('HH:mm:ss');
                        var claseEditDelHop = esEditableRegHO(esEditable, evt.ListaHorasOperacion[z]) ? "context-menu-one" : "";
                        if (evt.ListaModosOperacion[i].FlagModoEspecial == 'S') {
                            // imprime horas de operacion para modo de operacion extra

                            var codiPadre = evt.ListaHorasOperacion[z].Hopcodi;
                            var posPadre = getPosHop(codiPadre, evt.ListaHorasOperacion);
                            color_ho = getColorHOP(evt.ListaHorasOperacion[posPadre]);

                            var unidadesExtra = listarUnidadesXModo(evt, evt.ListaModosOperacion[i].Grupocodi, evt.ListaModosOperacion[i].Equipadre);

                            var nUnidadesOp = parseInt(unidadesExtra != null && unidadesExtra.length) || 0;
                            nUnidadesOp = nUnidadesOp == 0 ? 1 : nUnidadesOp;

                            var altura = alturaHOP / nUnidadesOp;
                            for (var nh = 0; nh < nUnidadesOp; nh++) {
                                if (isDateNow) {// si hora de operacion es posterior a la fecha actual cambiar color 
                                    color_ho = "#A9A9A9"; //DarkGray 
                                }
                                posYDiv += (nh > 0 ? altura : 0);

                                var lHopByUnidad = listarHorasOperacionByHopcodipadre(evt.ListaHorasOperacion, codiPadre, unidadesExtra[nh].Equicodi);

                                for (var ihop = 0; ihop < lHopByUnidad.length; ihop++) {
                                    lHopByUnidad[ihop].Hophorini = moment(lHopByUnidad[ihop].Hophorini);
                                    lHopByUnidad[ihop].Hophorfin = moment(lHopByUnidad[ihop].Hophorfin);

                                    dateFrom = new Date(moment(lHopByUnidad[ihop].Hophorini));
                                    dateTo = new Date(moment(lHopByUnidad[ihop].Hophorfin));

                                    horaIni = (new Date(lHopByUnidad[ihop].Hophorini)).getHours();
                                    horaFin = (new Date(lHopByUnidad[ihop].Hophorfin)).getHours();
                                    minIni = (new Date(lHopByUnidad[ihop].Hophorini)).getMinutes();
                                    minFin = (new Date(lHopByUnidad[ihop].Hophorfin)).getMinutes();

                                    ///********* genera div para la hora de operacion********************************
                                    if (horaFin == 0 && minFin == 0) {//artificio para mostrar la hora fin 00:00 como 23:59
                                        horaFin = 23;
                                        minFin = 59;
                                    }
                                    anchoDiv = Math.round(((horaFin + minFin / 60) - (horaIni + minIni / 60)) * anchoTh) + (horaFin - horaIni) - 2;
                                    posXDiv = horaIni + horaIni * anchoTh + Math.round((minIni / 60) * anchoTh) + segmento;  // 31px por cada celda de horas

                                    enparalelo = moment(dateFrom).format('HH:mm:ss');
                                    fueraparalelo = moment(dateTo).format('HH:mm:ss');

                                    cadena += "<div ";
                                    cadena += "id='" + id + "' title='En Paralelo: " + enparalelo + " - Fuera Paralelo: " + fueraparalelo + "' class='" + claseEditDelHop +" menu-1 horasoperacion' style='width:";
                                    cadena += anchoDiv + "px;left:" + posXDiv + "px;  background-color:" + color_ho + "; top:" + posYDiv + "px; height: " + altura + "px;' />";
                                }
                            }
                        }
                        else {
                            cadena += "<div  onmouseover='resaltarFilareporte(" + evt.ListaHorasOperacion[z].Grupocodi + "," + evt.ListaHorasOperacion[z].Hopcodi + ");' onMouseOut='changeBgcolor(" + evt.ListaHorasOperacion[z].Grupocodi + "," + evt.ListaHorasOperacion[z].Hopcodi + ");'";
                            cadena += "id='" + id + "' title='En Paralelo: " + enparalelo + " - Fuera Paralelo: " + fueraparalelo + "' class='" + claseEditDelHop + " menu-1 horasoperacion' style='height:" + alturaHOP + "px;  width:";
                            cadena += anchoDiv + "px;left:" + posXDiv + "px;  background-color:" + color_ho + "; top:" + posYDiv + "px' />";
                        }

                        //*******************************************************************************                                                
                    }
                }
            }

            for (var k = 0; k < 24; k++) {
                cadena += "<td style='background-color: #ebedef;'> </td>"; //pinta vacio                
            }
            cadena += "</tr>";
            contadorModo++;
        }
    }
    else {
        cadena += "<tr><td  style='text-align:center'>No existen registros.</td></tr>";
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Unidades

    flag = 0;
    if (evt.ListaUnidades.length > 0 && evt.ListaUnidXModoOP.length > 0) {
        for (var kk = 0; kk < evt.ListaUnidades.length; kk++) {
            cadena += "<tr>";
            if (flag == 0) {
                cadena += "<td rowspan=" + evt.ListaUnidades.length + "><b> UNIDADES </b></td>";
            }
            flag = 1;
            //** Imprime boton on-off
            cadena += "<td style='padding-left: 5px; padding-right: 5px; padding-top: 3px;'>";
            var unidadcendida = verificaEquipoencendido(evt.ListaUnidades[kk].Equicodi);
            if (unidadcendida == 1) // encendido
            { cadena += "<img src='" + siteRoot + "Areas/IEOD/Content/Images/btn_on16x16.jpg'/>"; }
            else // apagado
            { cadena += "<img src='" + siteRoot + "Areas/IEOD/Content/Images/btn_off16x16.jpg'/>"; }
            cadena += "</td>";
            //*****

            cadena += "<td style='height: " + alturaTDbtn + "px!important;'>" + evt.ListaUnidades[kk].Equinomb + "</td>";
            if (evt.ListaHorasOperacion.length > 0) {
                for (var kfil = 0; kfil < evt.ListaHorasOperacion.length; kfil++) {
                    if (evt.ListaHorasOperacion[kfil].Equicodi == evt.ListaUnidades[kk].Equicodi && evt.ListaHorasOperacion[kfil].OpcionCrud != -1) {
                        var dateFrom = new Date(moment(evt.ListaHorasOperacion[kfil].Hophorini));
                        var dateTo = new Date(moment(evt.ListaHorasOperacion[kfil].Hophorfin));
                        var horaIni = (new Date(evt.ListaHorasOperacion[kfil].Hophorini)).getHours();// en formato 24H
                        var horaFin = (new Date(evt.ListaHorasOperacion[kfil].Hophorfin)).getHours();//en formato 24H
                        var minIni = (new Date(evt.ListaHorasOperacion[kfil].Hophorini)).getMinutes();
                        var minFin = (new Date(evt.ListaHorasOperacion[kfil].Hophorfin)).getMinutes();


                        ///********* genera div para la hora de operacion unidades ********************************
                        if (horaFin == 0 && minFin == 0) {//artificio para mostrar la hora fin 00:00 como 23:59
                            horaFin = 23;
                            minFin = 59;
                        }

                        var anchoDiv = Math.round(((horaFin + minFin / 60) - (horaIni + minIni / 60)) * anchoTh) + (horaFin - horaIni) - 2;
                        var posXDiv = horaIni + horaIni * anchoTh + Math.round((minIni / 60) * anchoTh) + segmento;  // 31px por cada celda de horas
                        var posYDiv = alturaTD * (contadorModo) + alturaTh + paddingTop;
                        var id = evt.ListaHorasOperacion[kfil].Hopcodi;

                        var color_ho = "#46b3d6";
                        enparalelo = moment(dateFrom).format('HH:mm:ss');
                        fueraparalelo = moment(dateTo).format('HH:mm:ss');
                        cadena += "<div  id='" + id + "' title='En Paralelo: " + enparalelo + " - Fuera Paralelo: " + fueraparalelo + "' class='horasoperacion' style='height:" + alturaHOP + "px;  width:" + anchoDiv + "px;left:" + posXDiv + "px;  background-color:" + color_ho + "; top:" + posYDiv + "px' />";

                        //*******************************************************************************                        
                    }
                }
            }
            for (var hfil = 0; hfil < 24; hfil++) {
                cadena += "<td> </td>"; //pinta vacio
            }
            cadena += "</tr>";
            contadorModo++;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Linea Fecha Actual

        var fechaActual = new Date();
        var sFecha = convertStringToDate(fecha, "00:00:00");

        var tamvertical = alturaTh;
        tamvertical += alturaTD * (contadorModo) + (paddingTop - 2);

        if (moment(moment(fechaActual).format('YYYY-MM-DD')).isSame(moment(sFecha).format('YYYY-MM-DD'))) {
            var horaIni = fechaActual.getHours();
            var minIni = fechaActual.getMinutes();
            var desplazamiento = horaIni + horaIni * anchoTh + Math.round((minIni / 60) * anchoTh) + segmento;
            cadena += "<div title='" + fechaActual.toLocaleString('en-ES') + "' class='vertical-line' style='height:" + tamvertical + "px;left:" + desplazamiento + "px; position: absolute;' />";
        }
    }
    else {
        cadena += "<tr><td  style='text-align:center'>No existen registros.</td></tr>";
    }
    cadena += "</tbody>";
    cadena += "</table>";
    return cadena;
}

function generaViewListaSolarEolica(evt, fecha) {
    var esEditable = parseInt($("#hfIdEnvio").val()) == 0 && evtHot.PlazoEnvio.TipoPlazo != "D";

    var alturaCol = 12;
    var padding = 5 * 2;
    var tamanio = TAMANIO_GRAFICO;
    var tamanioCol = 32; //h0,h23
    var tamanioDescripcion = tamanio - (tamanioCol * (24));


    var cadena = '';
    cadena += "<table  class='pretty tabla-horas' style='width: " + (TAMANIO_TABLA_GRAFICO) + "px' >";

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Cabecera

    cadena += "<thead><tr> ";
    cadena += "<th class='' style='width:" + (tamanioCol - padding) + "px; height: " + alturaCol + "px;'></th> ";
    tamanioDescripcion -= (tamanioCol);
    var tamanioCentral = parseInt(tamanioDescripcion / 1.1);
    tamanioDescripcion -= tamanioCentral;
    cadena += "<th class='' style='width:" + (tamanioCentral - padding) + "px'>CENTRAL</th> ";

    segmento = tamanioCol + tamanioCentral - 1;
    for (var i = 0; i < 24; i++) {
        cadena += "<th class=''  style='width:" + (tamanioCol - padding) + "px'>" + i + "</th>";
    }

    cadena += "</tr></thead>";

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Linea Fecha Actual
    var alturaTD = 23;
    var alturaTDbtn = 20;
    var paddingTop = 5;
    var alturaHOP = 18;
    var alturaTh = 23;
    var anchoTh = tamanioCol;
    var paddingRight = 3;

    var fechaActual = new Date();
    var sFecha = convertStringToDate(fecha, "00:00:00");

    var tamvertical = alturaTh;
    tamvertical += alturaTD * (evt.ListaCentrales.length) + (paddingTop - 2);

    if (moment(moment(fechaActual).format('YYYY-MM-DD')).isSame(moment(sFecha).format('YYYY-MM-DD'))) {
        var horaIni = fechaActual.getHours();
        var minIni = fechaActual.getMinutes();
        var desplazamiento = horaIni + horaIni * anchoTh + Math.round((minIni / 60) * anchoTh) + segmento;
        cadena += "<div title='" + fechaActual.toLocaleString('en-ES') + "' class='vertical-line' style='height:" + tamvertical + "px;left:" + desplazamiento + "px; position: absolute;' />";
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Cuerpo

    cadena += "<tbody>";

    if (evt.ListaCentrales.length > 0) {
        for (var i = 0; i < evt.ListaCentrales.length; i++) {
            cadena += "<tr>";
            //** Imprime boton on-off
            cadena += "<td style='background-color: white; padding-left: 5px;padding-right: 5px; padding-top: 3px;'>";

            var equipoendido = verificaEquipoencendido(evt.ListaCentrales[i].Equicodi);
            if (equipoendido == 1) // encendido
            { cadena += "<img src='" + siteRoot + "Areas/IEOD/Content/Images/btn_on16x16.jpg'/>"; }
            else // apagado
            { cadena += "<img src='" + siteRoot + "Areas/IEOD/Content/Images/btn_off16x16.jpg'/>"; }
            cadena += "</td>";
            //*****
            cadena += "<td style='white-space: initial;'>" + evt.ListaCentrales[i].Equinomb + "</td>";
            if (evt.ListaHorasOperacion.length > 0) { //si hay horas de operacion                              
                for (var z = 0; z < evt.ListaHorasOperacion.length; z++) {
                    //***convertimos los datos tipo /Date(99999999999)/ a Date
                    evt.ListaHorasOperacion[z].Hophorini = moment(evt.ListaHorasOperacion[z].Hophorini);
                    evt.ListaHorasOperacion[z].Hophorfin = moment(evt.ListaHorasOperacion[z].Hophorfin);
                    if (evt.ListaHorasOperacion[z].Equicodi == evt.ListaCentrales[i].Equicodi && evt.ListaHorasOperacion[z].OpcionCrud != -1) {
                        var dateFrom = new Date(moment(evtHot.ListaHorasOperacion[z].Hophorini));
                        var dateTo = new Date(moment(evtHot.ListaHorasOperacion[z].Hophorfin));
                        var isDateNow = false;
                        var fechaActual = new Date();
                        // comprobar si una fecha actual es anterior a la barra de hora de operación
                        if (!moment(fechaActual).isBetween(dateFrom, dateTo) && moment(fechaActual).isBefore(dateFrom)) {
                            isDateNow = true;
                        }

                        var horaIni = (new Date(evt.ListaHorasOperacion[z].Hophorini)).getHours();// en formato 24H
                        var horaFin = (new Date(evt.ListaHorasOperacion[z].Hophorfin)).getHours();//en formato 24H
                        var minIni = (new Date(evt.ListaHorasOperacion[z].Hophorini)).getMinutes();
                        var minFin = (new Date(evt.ListaHorasOperacion[z].Hophorfin)).getMinutes();


                        ///********* genera div para la hora de operacion ********************************
                        if (horaFin == 0 && minFin == 0) {//artificio para mostrar la hora fin 00:00 como 23:59
                            horaFin = 23;
                            minFin = 59;
                        }
                        var anchoDiv = Math.round(((horaFin + minFin / 60) - (horaIni + minIni / 60)) * anchoTh) + (horaFin - horaIni) - 3;
                        var posXDiv = horaIni + horaIni * anchoTh + Math.round((minIni / 60) * anchoTh) + segmento;  // 31px por cada celda de horas
                        var posYDiv = alturaTD * (i) + alturaTh + paddingTop;
                        var id = evt.ListaHorasOperacion[z].Hopcodi;

                        var color_ho = getColorHOP(evt.ListaHorasOperacion[z]);
                        if (isDateNow) {// si hora de operacion es posterior a la fecha actual cambiar color 
                            color_ho = "#A9A9A9"; //DarkGray 
                        }
                        enparalelo = moment(dateFrom).format('HH:mm:ss');
                        fueraparalelo = moment(dateTo).format('HH:mm:ss');
                        var claseEditDelHop = esEditable ? "context-menu-one" : "";
                        cadena += "<div onmouseover='resaltarFilareporte(" + evt.ListaHorasOperacion[z].Equicodi + "," + evt.ListaHorasOperacion[z].Hopcodi + ");' onMouseOut='changeBgcolor(" + evt.ListaHorasOperacion[z].Equicodi + "," + evt.ListaHorasOperacion[z].Hopcodi + ");'";
                        cadena += "id='" + id + "' title='En Paralelo: " + enparalelo + " - Fuera Paralelo: " + fueraparalelo + "' class='" + claseEditDelHop + " menu-1 horasoperacion' style='width:" + anchoDiv;
                        cadena += "px;left:" + posXDiv + "px;  background-color:" + color_ho + "; top:" + posYDiv + "px; height: " + alturaHOP + "px;' />";

                        //*******************************************************************************
                    }
                }

                for (var k = 0; k < 24; k++) {
                    cadena += "<td> </td>"; //pinta vacio                    
                }
            }
            else { //no hay registros de horas de operación
                for (var col = 0; col < 24; col++) {
                    cadena += "<td> </td>";
                }
            }
        }
        cadena += "</tr>";
    }
    else {
        cadena += "<tr><td  style='text-align:center'>No existen registros.</td></tr>";
    }
    cadena += "</tbody>";
    cadena += "</table>";
    return cadena;
}

//////////////////////////////////////////////////////////////////////////////////////////
/// Reporte de Envío
//////////////////////////////////////////////////////////////////////////////////////////

function generaEnvioHorasOperacion(opcion) {
    var tipoCentral = evtHot.IdTipoCentral;
    var strHtml = "<div style='clear:both; height:30px'></div>";

    //tabla de horas de operacion
    strHtml = "<table border='1' class='pretty tabla-horas' cellspacing='0' width='100%' id='tablaReporte'>";
    strHtml += "<thead>" + cabeceraEnvioHOP(tipoCentral, opcion)
    strHtml += "<tbody>";

    ordenarListaHorasOperacion(evtHot.ListaHorasOperacion);

    switch (tipoCentral) {
        case 4: //Hidraulicas  
            strHtml += generaEnvioHOPHidraulicas(opcion);
            break;
        case 5: //Térmicas
            strHtml += generaEnvioHOPTermicas(opcion);
            break;
        case 37: //Solares
        case 39: //Eolicas
            strHtml += generaEnvioHOPSolarEolica(opcion);
            break;

    }

    strHtml += "</tbody></table>";

    //Boton enviar
    switch (opcion) {
        case 1:
            strHtml += ` 
                <input type="hidden" id="hfConfirmarValInterv" value="0" />
                <table>
                    <tr class="fila_val_intervenciones" style="display: none">
                        <td colspan="2">
                            <table style="margin-top: 0px; margin-bottom: 0px;">
                                <tr>
                                    <td>
                                        <div id="div_msj_val_intervenciones" style="text-indent: 0px;"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <i>Presione el botón <b>Confirmar</b> para guardar la hora de operación, esta acción enviará un correo electrónico al Administrador y usuario que registró la intervención.</i>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table class="btnAcciones">
                                <tr>
                                    <td>
                                        <input type='button' id='btnAcep' value='Enviar' onclick='grabarEnvioHorasOperacion();'/>
                                        <input type="button" id="btnAceptar3" value="Confirmar" onclick='grabarConfirmacionHorasOperacion();' style="display: none" />
                                    </td>
                                    <td>
                                        <input type='button' id='btnCancel' value='Salir' onclick='offshowView();'/>
                                        <input type="button" id="btnCancelar3" value="Cancelar" onclick='offshowView2();' style="display: none" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            `;
            break;
        case 2:
            strHtml += "<table>";
            strHtml += "<tr><td></td></tr>";
            strHtml += '</table>';
            break;
        case 3:
            strHtml += "<table>";
            strHtml += "<tr><td></td></tr>";
            strHtml += '</table>';
            break;
    }

    return strHtml;
}

function cabeceraEnvioHOP(tipoCentral, op) {
    var tipoCabecera = " ";
    var cadena = "";
    switch (tipoCentral) {
        case 4:
            tipoCabecera = "CENTRAL - GRUPO";
            break;
        case 5:
            tipoCabecera = "MODOS OPERACIÓN - UNIDADES ";
            break;
        default:
            tipoCabecera = "CENTRAL";
            break;

    }
    cadena += "<tr><th>" + tipoCabecera + "</th>";
    //cadena += "<th>UNIDAD</th>";
    cadena += "<th>ORDEN <BR>DE ARRANQUE</th>";
    cadena += "<th>EN PARALELO</th>";
    cadena += "<th>ORDEN <BR>DE PARADA</th>";
    cadena += "<th>F/S<BR> POR FALLA</th>";
    cadena += "<th>FIN REGISTRO</th>";
    if (op === 2) { //1: tabla para enviar, 2: actualización, 3: ya enviado o solo lectura
        cadena += "<th style='width: 31px;'></th>";
    }
    cadena += "</tr></thead>";
    return cadena;
}

function generaEnvioHOPHidraulicas(op) {
    var esEditable = parseInt($("#hfIdEnvio").val()) == 0 && evtHot.PlazoEnvio.TipoPlazo != "D";

    var cadenaHidro = "";
    if (evtHot.ListaCentrales.length > 0) {
        for (var i = 0; i < evtHot.ListaCentrales.length; i++) {
            cadenaHidro += "<tr>";
            cadenaHidro += "<td style='background-color:DarkGray;'><b>" + evtHot.ListaCentrales[i].Equinomb + "</b></td>";
            for (var s = 0; s < 5 + (op === 2 ? 1 : 0); s++) {
                cadenaHidro += "<td style='background-color:DarkGray;'></td>";
            }
            cadenaHidro += "</tr>";
            var icentral = evtHot.ListaCentrales[i].Equicodi;
            if (evtHot.ListaGrupo.length > 0) {
                for (var j = 0; j < evtHot.ListaGrupo.length; j++) {
                    if (evtHot.ListaGrupo[j].Equipadre == icentral) {

                        var vacio = 0;
                        var esPrimer = true;
                        for (var z = 0; z < evtHot.ListaHorasOperacion.length; z++) {
                            if (evtHot.ListaHorasOperacion[z].Equicodi == evtHot.ListaGrupo[j].Equicodi && evtHot.ListaHorasOperacion[z].OpcionCrud != -1) {
                                if (esPrimer) {
                                    cadenaHidro += "<tr id ='" + evtHot.ListaGrupo[j].Equicodi + "_" + evtHot.ListaHorasOperacion[z].Hopcodi + "' style='height: 24px;'>";
                                    cadenaHidro += "<td>" + evtHot.ListaGrupo[j].Equinomb + "</td>";
                                    esPrimer = false;
                                }
                                var horaIni = moment(evtHot.ListaHorasOperacion[z].Hophorini).format('HH:mm:ss');
                                var horaFin = moment(evtHot.ListaHorasOperacion[z].Hophorfin).format('HH:mm:ss');
                                var iniHora = (new Date(evtHot.ListaHorasOperacion[z].Hophorini)).getHours();// en formato 24H
                                var minIni = (new Date(evtHot.ListaHorasOperacion[z].Hophorini)).getMinutes();
                                var minFin = (new Date(evtHot.ListaHorasOperacion[z].Hophorfin)).getMinutes();

                                Hophorordarranq = "";
                                Hophorparada = "";

                                if (evtHot.ListaHorasOperacion[z].Hophorordarranq != null && evtHot.ListaHorasOperacion[z].Hophorordarranq != "") {
                                    Hophorordarranq = moment(evtHot.ListaHorasOperacion[z].Hophorordarranq).format('HH:mm:ss');
                                }
                                if (evtHot.ListaHorasOperacion[z].Hophorparada != null && evtHot.ListaHorasOperacion[z].Hophorparada != "") {
                                    Hophorparada = moment(evtHot.ListaHorasOperacion[z].Hophorparada).format('HH:mm:ss');
                                }
                                if (vacio == 1) {
                                    cadenaHidro += "</tr><tr id ='" + evtHot.ListaGrupo[j].Equicodi + "_" + evtHot.ListaHorasOperacion[z].Hopcodi + "' style='height: 24px;'><td></td>";
                                }
                                cadenaHidro += "<td>" + Hophorordarranq + "</td>"; //ORDEN DE ARRANQUE
                                cadenaHidro += "<td>" + horaIni + "</td>"; //EN PARALELO
                                cadenaHidro += "<td>" + Hophorparada + "</td>"; //ORDEN PARADA
                                cadenaHidro += "<td></td>"; //FUERA DE SERVICIO F/S
                                cadenaHidro += "<td>" + horaFin + "</td>"; //FIN REGISTRO         
                                if (op === 2) {
                                    if (esEditable) {
                                        cadenaHidro += "<td><img onclick='editarHOP(" + evtHot.ListaHorasOperacion[z].Hopcodi + ");' src='" + siteRoot + "Content/Images/btn-edit.png' title='Editar Hora de Operación' alt='Editar HoraOperacion'/>";
                                        cadenaHidro += "<img onclick='eliminarHOP(" + evtHot.ListaHorasOperacion[z].Hopcodi + ");' src='" + siteRoot + "Content/Images/btn-cancel.png' title='Eliminar Hora de Operación' alt='Eliminar HoraOperacion'/></td>";
                                    } else {
                                        cadenaHidro += "<td><img onclick='verHOP(" + evtHot.ListaHorasOperacion[z].Hopcodi + ");' src='" + siteRoot + "Content/Images/btn-open.png' title='Ver registro'/></td>";
                                    }
                                }
                                cadenaHidro += "</tr>";
                                vacio = 1;
                            }
                        }
                    }
                }
            }
        }
    }
    return cadenaHidro;
}

function generaEnvioHOPTermicas(op) {
    var esEditable = parseInt($("#hfIdEnvio").val()) == 0 && evtHot.PlazoEnvio.TipoPlazo != "D";

    var cadenaTermo = "";
    cadenaTermo += "<tr><td style='background-color:DarkGray;'>MODOS DE OPERACIÓN</td>";
    for (var s = 0; s < 5 + (op === 2 ? 1 : 0); s++) {
        cadenaTermo += "<td style='background-color:DarkGray;'></td>";
    }
    cadenaTermo += "</tr>";
    if (evtHot.ListaModosOperacion.length > 0) {
        for (var i = 0; i < evtHot.ListaModosOperacion.length; i++) {

            var vacio = 0;
            var esPrimer = true;
            for (var z = 0; z < evtHot.ListaHorasOperacion.length; z++) {
                if (evtHot.ListaHorasOperacion[z].FlagTipoHo == TIPO_HO_MODO && evtHot.ListaHorasOperacion[z].Grupocodi == evtHot.ListaModosOperacion[i].Grupocodi && evtHot.ListaHorasOperacion[z].OpcionCrud != -1) {
                    if (esPrimer) {
                        cadenaTermo += "<tr id ='" + evtHot.ListaModosOperacion[i].Grupocodi + "_" + evtHot.ListaHorasOperacion[z].Hopcodi + "' style='height: 24px;'>";
                        cadenaTermo += "<td>" + evtHot.ListaModosOperacion[i].Gruponomb + "</td>";
                        esPrimer = false;
                    }

                    var horaIni = moment(evtHot.ListaHorasOperacion[z].Hophorini).format('HH:mm:ss');
                    var horaFin = moment(evtHot.ListaHorasOperacion[z].Hophorfin).format('HH:mm:ss');
                    var iniHora = (new Date(evtHot.ListaHorasOperacion[z].Hophorini)).getHours();// en formato 24H
                    var minIni = (new Date(evtHot.ListaHorasOperacion[z].Hophorini)).getMinutes();
                    var minFin = (new Date(evtHot.ListaHorasOperacion[z].Hophorfin)).getMinutes();
                    Hophorordarranq = "";
                    Hophorparada = "";

                    if (evtHot.ListaHorasOperacion[z].Hophorordarranq != null && evtHot.ListaHorasOperacion[z].Hophorordarranq != "") {
                        Hophorordarranq = moment(evtHot.ListaHorasOperacion[z].Hophorordarranq).format('HH:mm:ss');
                    }
                    if (evtHot.ListaHorasOperacion[z].Hophorparada != null && evtHot.ListaHorasOperacion[z].Hophorparada != "") {
                        Hophorparada = moment(evtHot.ListaHorasOperacion[z].Hophorparada).format('HH:mm:ss');
                    }
                    if (vacio == 1) {
                        cadenaTermo += "</tr><tr id ='" + evtHot.ListaModosOperacion[i].Grupocodi + "_" + evtHot.ListaHorasOperacion[z].Hopcodi + "' style='height: 24px;'><td></td>";
                    }
                    var fsFallaDesc = evtHot.ListaHorasOperacion[z].Hopfalla == "F" ? "SÍ" : "";

                    cadenaTermo += "<td>" + Hophorordarranq + "</td>"; //ORDEN DE ARRANQUE
                    cadenaTermo += "<td>" + horaIni + "</td>"; //EN PARALELO
                    cadenaTermo += "<td>" + Hophorparada + "</td>"; //ORDEN PARADA
                    cadenaTermo += "<td>" + fsFallaDesc + "</td>"; //FUERA DE SERVICIOF/S
                    cadenaTermo += "<td>" + horaFin + "</td>"; //FIN REGISTRO
                    if (op === 2) {
                        if (esEditableRegHO(esEditable, evtHot.ListaHorasOperacion[z])) {
                            cadenaTermo += "<td><img onclick='editarHOP(" + evtHot.ListaHorasOperacion[z].Hopcodi + ");' src='" + siteRoot + "Content/Images/btn-edit.png' title='Editar registro'/>";
                            cadenaTermo += "<img onclick='eliminarHOP(" + evtHot.ListaHorasOperacion[z].Hopcodi + ");' src='" + siteRoot + "Content/Images/btn-cancel.png' title='Eliminar Hora de Operación' alt='Eliminar HoraOperacion'/></td>";
                        } else {
                            cadenaTermo += "<td><img onclick='verHOP(" + evtHot.ListaHorasOperacion[z].Hopcodi + ");' src='" + siteRoot + "Content/Images/btn-open.png' title='Ver registro'/></td>";
                        }
                    }
                    cadenaTermo += "</tr>";
                    vacio = 1;
                }
            }
        }
    }

    //****Imprime unidades

    cadenaTermo += "<tr>";
    cadenaTermo += "<td style='background-color:DarkGray;'><b> UNIDADES </b></td>";
    for (var s = 0; s < 5 + (op === 2 ? 1 : 0); s++) {
        cadenaTermo += "<td style='background-color:DarkGray;'> </td>";
    }
    cadenaTermo += "</tr>";
    if (evtHot.ListaUnidades.length > 0 && evtHot.ListaUnidXModoOP.length > 0) {
        for (var kk = 0; kk < evtHot.ListaUnidades.length; kk++) {
            vacio = 0;
            for (var kfil = 0; kfil < evtHot.ListaHorasOperacion.length; kfil++) {
                if (evtHot.ListaHorasOperacion[kfil].FlagTipoHo == TIPO_HO_UNIDAD && evtHot.ListaHorasOperacion[kfil].Equicodi == evtHot.ListaUnidades[kk].Equicodi && evtHot.ListaHorasOperacion[kfil].OpcionCrud != -1) {
                    if (vacio == 0) {
                        cadenaTermo += "<tr>";
                        cadenaTermo += "<td>" + evtHot.ListaUnidades[kk].Equinomb + "</td>";
                    }

                    if (vacio == 1) {
                        cadenaTermo += "<tr><td></td>";
                    }

                    var horaIni = moment(evtHot.ListaHorasOperacion[kfil].Hophorini).format('HH:mm:ss');
                    var horaFin = moment(evtHot.ListaHorasOperacion[kfil].Hophorfin).format('HH:mm:ss');
                    Hophorordarranq = "";
                    Hophorparada = "";
                    if (evtHot.ListaHorasOperacion[kfil].Hophorordarranq != null && evtHot.ListaHorasOperacion[kfil].Hophorordarranq != "") {
                        Hophorordarranq = moment(evtHot.ListaHorasOperacion[kfil].Hophorordarranq).format('HH:mm:ss');
                    }
                    if (evtHot.ListaHorasOperacion[kfil].Hophorparada != null && evtHot.ListaHorasOperacion[kfil].Hophorparada != "") {
                        Hophorparada = moment(evtHot.ListaHorasOperacion[kfil].Hophorparada).format('HH:mm:ss');
                    }
                    var fsFallaDescUnidad = evtHot.ListaHorasOperacion[kfil].Hopfalla == "F" ? "SÍ" : "";

                    cadenaTermo += "<td>" + Hophorordarranq + "</td>"; //ORDEN DE ARRANQUE
                    cadenaTermo += "<td>" + horaIni + "</td>"; //EN PARALELO
                    cadenaTermo += "<td>" + Hophorparada + "</td>"; //ORDEN PARADA
                    cadenaTermo += "<td>" + fsFallaDescUnidad + "</td>"; //FUERA DE SERVICIOF/S
                    cadenaTermo += "<td>" + horaFin + "</td>"; //FIN REGISTRO
                    if (op === 2) cadenaTermo += "<td></td>";
                    cadenaTermo += "</tr>";

                    vacio = 1;
                }
            }
        }
    }
    return cadenaTermo;
}

function generaEnvioHOPSolarEolica(op) {
    var esEditable = parseInt($("#hfIdEnvio").val()) == 0 && evtHot.PlazoEnvio.TipoPlazo != "D";

    var cadenaSolTer = "";
    if (evtHot.ListaCentrales.length > 0) {
        for (var i = 0; i < evtHot.ListaCentrales.length; i++) {
            var vacio = 0;
            var esPrimer = true;
            for (var z = 0; z < evtHot.ListaHorasOperacion.length; z++) {
                if (evtHot.ListaHorasOperacion[z].Equicodi == evtHot.ListaCentrales[i].Equicodi && evtHot.ListaHorasOperacion[z].OpcionCrud != -1) {

                    if (esPrimer) { // imprime primera fila
                        cadenaSolTer += "<tr id= '" + evtHot.ListaCentrales[i].Equicodi + "_" + evtHot.ListaHorasOperacion[z].Hopcodi + "' style='height: 24px;'>";
                        cadenaSolTer += "<td>" + evtHot.ListaCentrales[i].Equinomb + "</td>";
                        esPrimer = false;
                    }

                    var horaIni = moment(evtHot.ListaHorasOperacion[z].Hophorini).format('HH:mm:ss');
                    var horaFin = moment(evtHot.ListaHorasOperacion[z].Hophorfin).format('HH:mm:ss');
                    var iniHora = (new Date(evtHot.ListaHorasOperacion[z].Hophorini)).getHours();// en formato 24H
                    var minIni = (new Date(evtHot.ListaHorasOperacion[z].Hophorini)).getMinutes();
                    var minFin = (new Date(evtHot.ListaHorasOperacion[z].Hophorfin)).getMinutes();

                    Hophorordarranq = "";
                    Hophorparada = "";
                    if (evtHot.ListaHorasOperacion[z].Hophorordarranq != null && evtHot.ListaHorasOperacion[z].Hophorordarranq != "") {
                        Hophorordarranq = moment(evtHot.ListaHorasOperacion[z].Hophorordarranq).format('HH:mm:ss');
                    }
                    if (evtHot.ListaHorasOperacion[z].Hophorparada != null && evtHot.ListaHorasOperacion[z].Hophorparada != "") {
                        Hophorparada = moment(evtHot.ListaHorasOperacion[z].Hophorparada).format('HH:mm:ss');
                    }
                    if (vacio == 1) {
                        cadenaSolTer += "</tr><tr id= '" + evtHot.ListaCentrales[i].Equicodi + "_" + evtHot.ListaHorasOperacion[z].Hopcodi + "' style='height: 24px;'><td></td>";
                    }
                    cadenaSolTer += "<td>" + Hophorordarranq + "</td>"; //ORDEN DE ARRANQUE
                    cadenaSolTer += "<td>" + horaIni + "</td>"; //EN PARALELO
                    cadenaSolTer += "<td>" + Hophorparada + "</td>"; //ORDEN PARADA
                    cadenaSolTer += "<td></td>"; //FUERA DE SERVICIOF/S
                    cadenaSolTer += "<td>" + horaFin + "</td>"; //FIN REGISTRO
                    if (op === 2) {
                        if (esEditable) {
                            cadenaSolTer += "<td><img onclick='editarHOP(" + evtHot.ListaHorasOperacion[z].Hopcodi + ");' src='" + siteRoot + "Content/Images/btn-edit.png' title='Editar Hora de Operación' alt='Editar HoraOperacion'/>";
                            cadenaSolTer += "<img onclick='eliminarHOP(" + evtHot.ListaHorasOperacion[z].Hopcodi + ");' src='" + siteRoot + "Content/Images/btn-cancel.png' title='Eliminar Hora de Operación' alt='Eliminar HoraOperacion'/></td>";
                        } else {
                            cadenaSolTer += "<td><img onclick='verHOP(" + evtHot.ListaHorasOperacion[z].Hopcodi + ");' src='" + siteRoot + "Content/Images/btn-open.png' title='Ver registro'/></td>";
                        }
                    }
                    cadenaSolTer += "</tr>";
                    vacio = 1;
                }
            }
        }
    }
    return cadenaSolTer;
}

/// Muestra los envios anteriores
function dibujarTablaEnviosHO(lista) {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablalenvio' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Id Envío</th><th>Fecha Hora</th><th>Usuario</th></tr></thead>";
    cadena += "<tbody>";

    for (key in lista) {
        var javaScriptDate = new Date(parseInt(lista[key].Enviofecha.substr(6)));
        cadena += "<tr onclick='mostrarEnvioHorasOperacion(" + lista[key].Enviocodi + ");' style='cursor:pointer'><td>" + lista[key].Enviocodi + "</td>";
        cadena += "<td>" + getFormattedDate2(javaScriptDate) + "</td>";
        cadena += "<td>" + lista[key].Lastuser + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;

}

function getColorHOP(hop) {

    var colorHop = "";
    /*if (hop != null && hop.Fenercolor != null)
        colorHop = hop.Fenercolor;*/

    return colorHop
}

//0: apagado, 1: encendido
function verificaEquipoencendido(equicodi) {
    var fechaListado = $("#txtFecha").val();
    var fechaActualFormato = obtenerDiaActualFormato();

    //solo verificar para el día actual
    if (fechaActualFormato == fechaListado) {
        var fechaAct = new Date();
        var hayHoraOpXEquicodi = false;

        if (evtHot.ListaHorasOperacion.length > 0) {
            for (var j = 0; j < evtHot.ListaHorasOperacion.length; j++) {
                if (evtHot.ListaHorasOperacion[j].Equicodi == equicodi && evtHot.ListaHorasOperacion[j].OpcionCrud != -1) {
                    var dateFrom = new Date(moment(evtHot.ListaHorasOperacion[j].Hophorini));
                    var dateTo = new Date(moment(evtHot.ListaHorasOperacion[j].Hophorfin));
                    if (moment(fechaAct).isBetween(dateFrom, dateTo))
                        return 1;

                    hayHoraOpXEquicodi = true;
                }
            }

            if (!hayHoraOpXEquicodi) {//si no existe horas de operacion para tal equicodi buscar en las horas de operacion del día anterior
                if (evtHot.ListaHorasOperacionAnt.length > 0) { //si hay horas de operacion el dia anterior
                    for (var i = 0; i < evtHot.ListaHorasOperacionAnt.length; i++) {
                        if (evtHot.ListaHorasOperacionAnt[i].Equicodi == equicodi && evtHot.ListaHorasOperacionAnt[i].OpcionCrud != -1) {
                            var horaFin = moment(evtHot.ListaHorasOperacionAnt[i].Hophorfin).format('HH:mm');
                            if (horaFin == '00:00') { //if (horaFin == '23:59') {
                                return 1;
                            }
                        }
                    }
                }
            }
        }
        else {
            if (evtHot.ListaHorasOperacionAnt.length > 0) { //si hay horas de operacion el dia anterior
                for (var i = 0; i < evtHot.ListaHorasOperacionAnt.length; i++) {
                    if (evtHot.ListaHorasOperacionAnt[i].Equicodi == equicodi && evtHot.ListaHorasOperacionAnt[i].OpcionCrud != -1) {
                        var horaFin = moment(evtHot.ListaHorasOperacionAnt[i].Hophorfin).format('HH:mm');
                        if (horaFin == '00:00') { //if (horaFin == '23:59') {
                            return 1;
                        }
                    }
                }
            }
        }
    }

    return 0;
}

function verificaEquipoencendidoTermico(Grupocodi) {
    var fechaListado = $("#txtFecha").val();
    var fechaActualFormato = obtenerDiaActualFormato();

    //solo verificar para el día actual
    if (fechaActualFormato == fechaListado) {
        var fechaActual = new Date();
        var hayHoraOpXGrupocodi = false;



        if (evtHot.ListaHorasOperacion.length > 0) {
            for (var j = 0; j < evtHot.ListaHorasOperacion.length; j++) {
                if (evtHot.ListaHorasOperacion[j].Grupocodi == Grupocodi && evtHot.ListaHorasOperacion[j].OpcionCrud != -1) {
                    var dateFrom = new Date(moment(evtHot.ListaHorasOperacion[j].Hophorini));
                    var dateTo = new Date(moment(evtHot.ListaHorasOperacion[j].Hophorfin));
                    if (moment(fechaActual).isBetween(dateFrom, dateTo))
                        return 1;

                    hayHoraOpXGrupocodi = true;
                }
            }


            if (!hayHoraOpXGrupocodi) {//si no existe horas de operacion para tal equicodi buscar en las horas de operacion del día anterior
                if (evtHot.ListaHorasOperacionAnt.length > 0) { //si hay horas de operacion el dia anterior
                    for (var i = 0; i < evtHot.ListaHorasOperacionAnt.length; i++) {
                        if (evtHot.ListaHorasOperacionAnt[i].Grupocodi == Grupocodi && evtHot.ListaHorasOperacionAnt[i].OpcionCrud != -1) {
                            var horaFin = moment(evtHot.ListaHorasOperacionAnt[i].Hophorfin).format('HH:mm');
                            if (horaFin == '00:00') {//if (horaFin == '23:59') {
                                return 1;
                            }
                        }
                    }
                }
            }
        }
        else {
            if (evtHot.ListaHorasOperacionAnt.length > 0) { //si hay horas de operacion el dia anterior
                for (var i = 0; i < evtHot.ListaHorasOperacionAnt.length; i++) {
                    if (evtHot.ListaHorasOperacionAnt[i].Grupocodi == Grupocodi && evtHot.ListaHorasOperacionAnt[i].OpcionCrud != -1) {
                        var horaFin = moment(evtHot.ListaHorasOperacionAnt[i].Hophorfin).format('HH:mm');
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

function resaltarFilareporte(codiEquipo, hopcodi) {
    var idEqui = codiEquipo + "_" + hopcodi
    var fila = document.getElementById(idEqui);
    fila.className = "resaltar";
}

function changeBgcolor(codiEquipo, hopcodi) {
    var idEqui = codiEquipo + "_" + hopcodi;
    var fila = document.getElementById(idEqui);
    fila.className = null;
}
