var MSJ_VAL_CRUCE_HO = "Existen cruces en horas de operación. Debe eliminar y/o actualizar horas de operación.";
var MSJ_VAL_CRUCE_UNID = "¡Existe Cruce de Horas en las unidades!";
var MSJ_VAL_CONTINUIDAD_UNID = "¡Existe continuidad en las unidades, elimine o actualice!";
var MSJ_VAL_MODO_VACIO = 'No existe continuidad de la operación';
var MSJ_VAL_MODO_SIN_UNIDAD = "Debe asignar al menos una unidad correspondiente a la hora de operación";
var MSJ_VAL_HO_INVALIDA = 'Existe Hora de operación no válida';
var DESG_VAL_INCOMPLETO = 'El desglose tiene campos incompletos';
var DESG_VAL_INCLUIDO = 'El desglose de la Hora de Operación no es válido. No esta incluido completamente en la Hora de Operación';
var MSJ_VAL_UNID_ESP_ADMIN = 'Los intervalos de las unidades de generación no estan dentro del Intervalo Horario definido por el COES';

//======================================================================
//Funciones que analizan cruces de horas de operacion, unidades
//======================================================================
//funcion que verifica si hay cruces de horas de operacion
// 1: si no hay cruces de horas de operación
// 2: si hay cruces de horas de operacion 
function verificarCruceModoHO(listaHo, tipoCentral, idEquipoOrIdModo, fecha, fechaFin, enParalelo, fueraParalelo, tipoHO, pos) {
    var horaIni = convertStringToDate(fecha, enParalelo);
    var horaFin = convertStringToDate(fechaFin, fueraParalelo);
    var itipocentral = parseInt(tipoCentral);

    if (tipoHO == FLAG_HO_NO_ESPECIAL) { // tipo de hora de operacion normal
        if (listaHo.length > 0) {
            for (var i = 0; i < listaHo.length; i++) {
                //verificar que el registro no tenga eliminado lógico si vino de BD propiedad opCdrud = -1            
                switch (itipocentral) {
                    case 4: //Hidraulicas
                    case 37: //Solares
                    case 39: //Eolicas                    
                        if (listaHo[i].Equicodi == idEquipoOrIdModo && listaHo[i].OpcionCrud != -1 && i != pos) { // si hay cruce de horas con el rango de horas ingresadas
                            var dateFrom = new Date(moment(listaHo[i].Hophorini));
                            var dateTo = new Date(moment(listaHo[i].Hophorfin));

                            if (moment(horaIni).isBetween(dateFrom, dateTo))
                                return 2;
                            if (moment(horaFin).isBetween(dateFrom, dateTo))
                                return 2;
                            if (moment(dateFrom).isBetween(moment(horaIni), moment(horaFin)))
                                return 2;
                            if (moment(dateTo).isBetween(moment(horaIni), moment(horaFin)))
                                return 2;
                            if (moment(dateFrom).isSame(moment(horaIni)) && moment(dateTo).isSame(moment(horaFin)))
                                return 2;
                        }
                        break
                    case 5: //Térmicas
                        if (listaHo[i].Grupocodi == idEquipoOrIdModo && listaHo[i].OpcionCrud != -1 && i != pos) { // si hay cruce de horas con el rango de horas ingresadas                        
                            var dateFrom = new Date(moment(listaHo[i].Hophorini));
                            var dateTo = new Date(moment(listaHo[i].Hophorfin));

                            if (moment(horaIni).isBetween(dateFrom, dateTo))
                                return 2;
                            if (moment(horaFin).isBetween(dateFrom, dateTo))
                                return 2;
                            if (moment(dateFrom).isBetween(moment(horaIni), moment(horaFin)))
                                return 2;
                            if (moment(dateTo).isBetween(moment(horaIni), moment(horaFin)))
                                return 2;
                            if (moment(dateFrom).isSame(moment(horaIni)) && moment(dateTo).isSame(moment(horaFin)))
                                return 2;
                        }
                        break;
                }
            }
        }
    }

    return 1;
}
//funcion que verifica el cruce de las unidades respectivas del modo de operacion ya sea normal o extra segun el "tipoHO", para el caso de un nuevo registro
function verificarCruceUnidadesHO_CREATE(listaHo, grupocodiModo, horaIni, horaFin, tipoHO, listaUnidad) {

    if (tipoHO == FLAG_HO_NO_ESPECIAL) { // tipo de hora de operacion normal
        if (evtHot.ListaUnidXModoOP.length > 0)
            for (var j = 0; j < evtHot.ListaUnidXModoOP.length; j++)
                if (evtHot.ListaUnidXModoOP[j].Grupocodi == grupocodiModo) {
                    if (listaHo.length > 0) {
                        for (var zz = 0; zz < listaHo.length; zz++) {
                            //verificar que el registro no tenga eliminado lógico si vino de BD porpiedad opCdrud != -1  y no sea una actualizacion                               
                            if (listaHo[zz].Equicodi == evtHot.ListaUnidXModoOP[j].Equicodi && listaHo[zz].OpcionCrud != -1) {
                                var dateFrom = new Date(moment(listaHo[zz].Hophorini));
                                var dateTo = new Date(moment(listaHo[zz].Hophorfin));

                                if (moment(horaIni).isBetween(dateFrom, dateTo))
                                    return MSJ_VAL_CRUCE_UNID;
                                if (moment(horaFin).isBetween(dateFrom, dateTo))
                                    return MSJ_VAL_CRUCE_UNID;
                                if (moment(dateFrom).isBetween(moment(horaIni), moment(horaFin)))
                                    return MSJ_VAL_CRUCE_UNID;
                                if (moment(dateTo).isBetween(moment(horaIni), moment(horaFin)))
                                    return MSJ_VAL_CRUCE_UNID;
                                if (moment(dateFrom).isSame(moment(horaIni)) && moment(dateTo).isSame(moment(horaFin)))
                                    return MSJ_VAL_CRUCE_UNID;
                            }
                        }
                    }
                }

    }

    if (tipoHO == FLAG_HO_ESPECIAL) { //tipo de hora de operacion especial
        if (listaUnidad.length > 0) {
            var listaHOByModo = [];
            var numUnidadConHO = 0;
            for (var m = 0; m < listaUnidad.length; m++) {
                var objUnidad = listaUnidad[m];
                listaHOByModo = listaHOByModo.concat(objUnidad.ListaHo);
                numUnidadConHO += (objUnidad.ListaHo.length > 0 ? 1 : 0);

                //Verificar si hay cruce entre las Ho del formulario y las que ya estan registradas
                var listaHoUnidadDelDia = listarHorasOperacionByUnidad(listaHo, objUnidad.Equicodi);
                listaHoUnidadDelDia = listaHoUnidadDelDia.concat(objUnidad.ListaHo);

                var msjVal = val_verificarCruceHOUnidadEspecial(listaHoUnidadDelDia);
                if (msjVal != '') {
                    return msjVal;
                }

                var msjVal3 = val_verificarContinuidadHOUnidadEspecial(objUnidad.ListaHo);
                if (msjVal3 != '') {
                    return msjVal3;
                }
            }

            var msjVal2 = verificarContinuidadHOModoEspecial(numUnidadConHO, listaHOByModo, horaIni, horaFin);
            if (msjVal2 != '') {
                return msjVal2;
            }
        }
    }

    return '';
}

// funcion que verifica el cruce de horas de operación para las unidades de un modo de operacion para el caso de una actualización
function verificarCruceUnidadesHO_UPDATE(listaHo, grupocodiModo, pos, inputHoraIni, inputHoraFin, tipoHO, listaUnidad) {
    var checkdateFrom = null;
    var checkdateTo = null;
    if (pos != -1) {
        var checkdateFrom = new Date(moment(listaHo[pos].Hophorini));
        var checkdateTo = new Date(moment(listaHo[pos].Hophorfin));
    }
    var horaIni = moment(inputHoraIni);
    var horaFin = moment(inputHoraFin);

    if (tipoHO == FLAG_HO_NO_ESPECIAL) { // tipo de hora de operacion normal
        if (evtHot.ListaUnidXModoOP.length > 0 && listaHo.length > 0)
            for (var j = 0; j < evtHot.ListaUnidXModoOP.length; j++)
                if (evtHot.ListaUnidXModoOP[j].Grupocodi == grupocodiModo) { // si es una unidad del modo 
                    for (var zz = 0; zz < listaHo.length; zz++) {
                        //verificar que el registro no tenga eliminado lógico si vino de BD prOpiedad opCdrud != -1  y no sea una actualizacion                               
                        if (listaHo[zz].Equicodi == evtHot.ListaUnidXModoOP[j].Equicodi //Hora de operación de la unidad
                            && listaHo[zz].OpcionCrud != -1) {

                            var dateFrom = moment(listaHo[zz].Hophorini);
                            var dateTo = moment(listaHo[zz].Hophorfin);

                            if (!dateFrom.isSame(checkdateFrom) && !dateTo.isSame(checkdateTo)) { //si la horaIni y horaFin de la HO unidad no coincide con HO del modo, mostrar por cual motivo
                                if (horaIni.isBetween(dateFrom, dateTo))
                                    return MSJ_VAL_CRUCE_UNID;
                                if (horaFin.isBetween(dateFrom, dateTo))
                                    return MSJ_VAL_CRUCE_UNID;

                                if (dateFrom.isBetween(horaIni, horaFin))
                                    return MSJ_VAL_CRUCE_UNID;
                                if (dateTo.isBetween(horaIni, horaFin))
                                    return MSJ_VAL_CRUCE_UNID;

                                if (dateFrom.isSame(horaIni) && dateTo.isSame(horaFin))
                                    return MSJ_VAL_CRUCE_UNID;
                            }
                        }
                    }
                }
    }

    if (tipoHO == FLAG_HO_ESPECIAL) { //tipo de hora de operacion especial
        if (listaUnidad.length > 0) {
            var listaHOByModo = [];
            var numUnidadConHO = 0;
            for (var m = 0; m < listaUnidad.length; m++) {
                var objUnidad = listaUnidad[m];
                listaHOByModo = listaHOByModo.concat(objUnidad.ListaHo);
                numUnidadConHO += (objUnidad.ListaHo.length > 0 ? 1 : 0);

                //Verificar si hay cruce entre las Ho del formulario y las que ya estan registradas
                var listaHoUnidadDelDia = listarHorasOperacionByUnidadFiltro(listaHo, objUnidad.Equicodi, checkdateFrom, checkdateTo);
                listaHoUnidadDelDia = listaHoUnidadDelDia.concat(objUnidad.ListaHo);

                var msjVal = val_verificarCruceHOUnidadEspecial(listaHoUnidadDelDia);
                if (msjVal != '') {
                    return msjVal;
                }

                var msjVal3 = val_verificarContinuidadHOUnidadEspecial(objUnidad.ListaHo);
                if (msjVal3 != '') {
                    return msjVal3;
                }
            }

            var msjVal2 = verificarContinuidadHOModoEspecial(numUnidadConHO, listaHOByModo, horaIni, horaFin);
            if (msjVal2 != '') {
                return msjVal2;
            }
        }
    }

    return '';
}

//Verifica si existe cruce entre las horas de las unidades especiales de un modo
//También verifica si hay continuidad que no es correcto(Deberia estar unidas)
function val_verificarCruceHOUnidadEspecial(listaHo) {
    ordenarListaHorasOperacion(listaHo);

    for (var pos = 0; pos < listaHo.length; pos++) {
        var horaIni = moment(listaHo[pos].Hophorini);
        var horaFin = moment(listaHo[pos].Hophorfin);

        for (var i = 0; i < listaHo.length; i++) {
            if (i != pos) { // si hay cruce de horas con el rango de horas ingresadas                        
                var dateFrom = moment(listaHo[i].Hophorini);
                var dateTo = moment(listaHo[i].Hophorfin);

                if (horaIni.isBetween(dateFrom, dateTo))
                    return MSJ_VAL_CRUCE_UNID;
                if (horaFin.isBetween(dateFrom, dateTo))
                    return MSJ_VAL_CRUCE_UNID;

                if (dateFrom.isBetween(horaIni, horaFin))
                    return MSJ_VAL_CRUCE_UNID;
                if (dateTo.isBetween(horaIni, horaFin))
                    return MSJ_VAL_CRUCE_UNID;
            }
        }
    }

    return '';
}

//Verifica si existe cruce entre las horas de las unidades especiales de un modo
//También verifica si hay continuidad que no es correcto(Deberia estar unidas)
function val_verificarContinuidadHOUnidadEspecial(listaHo) {
    ordenarListaHorasOperacion(listaHo);

    for (var pos = 0; pos < listaHo.length; pos++) {
        var horaIni = moment(listaHo[pos].Hophorini);
        var horaFin = moment(listaHo[pos].Hophorfin);

        for (var i = 0; i < listaHo.length; i++) {
            if (i != pos) { // si hay cruce de horas con el rango de horas ingresadas                        
                var dateFrom = moment(listaHo[i].Hophorini);
                var dateTo = moment(listaHo[i].Hophorfin);

                if (dateFrom.isSame(horaIni) || dateTo.isSame(horaFin))
                    return MSJ_VAL_CONTINUIDAD_UNID;
                if (dateTo.isSame(horaIni) || dateFrom.isSame(horaFin))
                    return MSJ_VAL_CONTINUIDAD_UNID;
            }
        }
    }

    return '';
}

//Verifica si no hay un vacio entre todas las horas de unidades especiales de un modo
function verificarContinuidadHOModoEspecial(numUnidad, listaHo, checkdateFrom, checkdateTo) {
    ordenarListaHorasOperacion(listaHo);

    var arrDate = [];
    for (var i = 0; i < listaHo.length; i++) {
        arrDate.push(listaHo[i].Hophorini);
        arrDate.push(listaHo[i].Hophorfin);
    }

    arrDate = arrDate.sort(function (a, b) { return a.getTime() - b.getTime() });

    arrDate.filter((date, i, self) =>
        self.findIndex(d => d.getTime() === date.getTime()) === i
    )

    if (arrDate.length > 0) {
        if (arrDate.length >= 2) {
            var dateIni = arrDate[0];
            var dateFin = arrDate[arrDate.length - 1];

            if (moment(checkdateFrom).isSame(dateIni) && moment(checkdateTo).isSame(dateFin)) {// las horas(menor de las de En Paralelo y mayor en Fuera de Paralelo) de la unidades estan dentro del modo
                var listaHoCorte = cortarListaHoSinInterseccion(listaHo);

                if (listaHoCorte.length >= 2) {
                    for (var j = 0; j < listaHoCorte.length - 1; j++) {
                        var fechaFinActual = moment(listaHoCorte[j].Hophorfin);
                        var fechaIniSig = moment(listaHoCorte[j + 1].Hophorini);
                        if (!moment(fechaFinActual).isSame(fechaIniSig)) {
                            return MSJ_VAL_MODO_VACIO;
                        }
                    }
                }
                return '';
            } else {
                return MSJ_VAL_HO_INVALIDA;
            }
        }

        return MSJ_VAL_HO_INVALIDA;
    }

    return '';
}

function agregarHOpUnidades(equicodi, unidadcodi, horaIni, horaFin) {
    var flag = 1; //-1:si esta incluido en un intervalo , 0: si hubo cruce y se fusiono, 1: si no hay cruce, 2: no hacer nada   
    if (evtHot.ListaUnidXModoOP.length > 0) {
        for (var mm = 0; mm < evtHot.ListaUnidXModoOP.length; mm++) {
            if (evtHot.ListaUnidXModoOP[mm].Equicodi == unidadcodi && evtHot.ListaUnidXModoOP[mm].Grupocodi == equicodi) {
                posx = -1;
                flag = 1;
                if (evtHot.ListaHorasOperacion.length > 0) {
                    for (var zz = 0; zz < evtHot.ListaHorasOperacion.length; zz++) {
                        if (evtHot.ListaHorasOperacion[zz].Equicodi == unidadcodi) {
                            var dateFrom = new Date(moment(evtHot.ListaHorasOperacion[zz].Hophorini));
                            var dateTo = new Date(moment(evtHot.ListaHorasOperacion[zz].Hophorfin));
                            // Caso 1: 
                            //si las horas de operacion estan dentro de un intervalo
                            //******************************
                            if (moment(horaIni).isBetween(dateFrom, dateTo) && moment(horaFin).isBetween(dateFrom, dateTo)) {
                                return -1;
                            }
                            // Caso 2: 
                            //si hay cruce y existe inervalo a la izquierda
                            //*****
                            if (moment(horaIni).isBetween(dateFrom, dateTo) && !moment(horaFin).isBetween(dateFrom, dateTo)) {
                                //modificamos el intervalo donde hay cruce
                                if (flag == 0) {// si hubo fusion anterior
                                    //modificamos el intervalo 
                                    if (evtHot.ListaHorasOperacion[zz].Hopcodi != 0) { // si viene de BD
                                        evtHot.ListaHorasOperacion[zz].Hophorini = horaIni;
                                        evtHot.ListaHorasOperacion[zz].Hophorfin = horaFin;
                                        evtHot.ListaHorasOperacion[zz].opCrud = 2; //  2:actualizar
                                        flag = 0;
                                        posx = zz;
                                        // eliminamos el intervalo anterior
                                        if (evtHot.ListaHorasOperacion[posx].Hopcodi != 0) // si el intervalo anterior viene de BD
                                            evtHot.ListaHorasOperacion[posx].opCrud = -1; // eliminado lógico  
                                        else
                                            evtHot.ListaHorasOperacion.splice(posx, 1);
                                    }
                                    else { // modificamos el intervalo anterior
                                        evtHot.ListaHorasOperacion[posx].Hophorini = horaIni;
                                        evtHot.ListaHorasOperacion[posx].Hophorfin = horaFin;
                                        evtHot.ListaHorasOperacion[posx] = 2; //  2:actualizar
                                        evtHot.ListaHorasOperacion.splice(zz, 1);
                                    }
                                }
                                else { // si no hubo fusion
                                    evtHot.ListaHorasOperacion[zz].Hophorfin = horaFin;
                                    evtHot.ListaHorasOperacion[zz].opCrud = 2; //  2:actualizar 
                                    horaIni = dateFrom;
                                    flag = 0;
                                    posx = zz;
                                }
                            }
                            // Caso 3: 
                            //si hay cruce y existe inervalo a la derecha
                            //*****
                            if (!moment(horaIni).isBetween(dateFrom, dateTo) && moment(horaFin).isBetween(dateFrom, dateTo)) {
                                //modificamos el intervalo donde hay cruce
                                if (flag == 0) {// si hubo fusion anterior
                                    //modificamos el intervalo 
                                    if (evtHot.ListaHorasOperacion[zz].Hopcodi != 0) { // si viene de BD
                                        evtHot.ListaHorasOperacion[zz].Hophorini = horaIni;
                                        evtHot.ListaHorasOperacion[zz].Hophorfin = horaFin;
                                        evtHot.ListaHorasOperacion[zz].opCrud = 2; //  2:actualizar
                                        flag = 0;
                                        posx = zz;
                                        // eliminamos el intervalo anterior
                                        if (evtHot.ListaHorasOperacion[posx].Hopcodi != 0) // si el intervalo anterior viene de BD
                                            evtHot.ListaHorasOperacion[posx].opCrud = -1; // eliminado lógico  
                                        else
                                            evtHot.ListaHorasOperacion.splice(posx, 1);
                                    }
                                    else { // modificamos el intervalo anterior
                                        evtHot.ListaHorasOperacion[posx].Hophorini = horaIni;
                                        evtHot.ListaHorasOperacion[posx].Hophorfin = horaFin;
                                        evtHot.ListaHorasOperacion[posx].opCrud = 2; //  2:actualizar
                                        evtHot.ListaHorasOperacion.splice(zz, 1);
                                    }
                                }
                                else { // si no hubo fusion
                                    evtHot.ListaHorasOperacion[zz].Hophorini = horaIni;
                                    evtHot.ListaHorasOperacion[zz].opCrud = 2; //  2:actualizar 
                                    horaFin = dateTo;
                                    flag = 0;
                                    posx = zz;
                                }
                            }
                            // Caso 4: 
                            //si esta cubre todo un inervalo previo
                            //*****
                            if (horaIni < dateFrom && horaFin > dateTo) {
                                evtHot.ListaHorasOperacion[posx].Hophorini = horaIni;
                                evtHot.ListaHorasOperacion[posx].Hophorfin = horaFin;
                                evtHot.ListaHorasOperacion[posx].opCrud = 2; //  2:actualizar
                                flag = 0;
                                posx = zz;
                            }
                        }
                    }
                    return flag;
                }

            }
            ///si no coicide
            flag = 2;
        }
    }
    return flag;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Funcionalidad para la Orden de Arranque, En Paralelo, Orden de Parada, Fuera Paralelo
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//verifica horas de operacion
function validarHorasOperacion(listaHo) {
    var valor = true;
    var sFecha = $('#txtFecha').val();
    var tipoCentral = parseInt(evtHot.IdTipoCentral);
    var codEquipo = 0;
    var nombEquipo = "";
    switch (tipoCentral) {
        case 4: //Hidro
            for (var i in evtHot.ListaCentrales) {
                for (var j in evtHot.ListaGrupo) {
                    if (evtHot.ListaGrupo[j].Equipadre == evtHot.ListaCentrales[i].Equicodi) {
                        codEquipo = evtHot.ListaGrupo[j].Equicodi;
                        nombEquipo = evtHot.ListaGrupo[j].Equinomb;
                        for (var z in listaHo) {
                            if (listaHo[z].OpcionCrud != -1 && listaHo[z].Equicodi == codEquipo) {//Horas de operacion válidas

                                horaFin = moment(listaHo[z].Hophorfin).format('HH:mm:ss');
                                if (horaFin != "00:00:00") {//if (horaFin != "23:59:59") {

                                    //verificacion Orden de parada
                                    //ORDEN DE PARADA VACIO
                                    if (listaHo[z].Hophorparada == null || listaHo[z].Hophorparada == "") {
                                        //valida si tiene adyacente a la derecha
                                        objAux = buscaHopDerecha(listaHo[z], listaHo, codEquipo, tipoCentral);
                                        if (objAux) { //si existe hora de operación derecha 
                                            //OK
                                        } else {
                                            if (listaHo[z].Hopfalla != "F") {//No es Fuera de Servicio(F/S) por Falla
                                                horaInicio = moment(listaHo[z].Hophorini).format('HH:mm:ss');
                                                if (horaInicio == "00:00:00") {
                                                    //verificar día anterior
                                                    existeHoraOperAnterior = obtenerHoraOperacionDiaAnterior(codEquipo);
                                                    if (!existeHoraOperAnterior) {
                                                        /*
                                                        * La validación de Orden de Parada esta deshabilitada
                                                        alert("Debe Ingresar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[z].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[z].Hophorfin).format('HH:mm:ss'));
                                                        return false;
                                                        */
                                                    }
                                                } else {
                                                    //verificar hora actual
                                                    fechaAct = new Date();
                                                    min = moment(fechaAct).minute();
                                                    fechaAct = moment(fechaAct).minute(min - 30);
                                                    dateTo = new Date(moment(listaHo[z].Hophorfin));
                                                    if (!moment(fechaAct).isBefore(dateTo)) {
                                                        /*
                                                        * La validación de Orden de Parada esta deshabilitada
                                                        alert("Debe Ingresar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[z].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[z].Hophorfin).format('HH:mm:ss'));
                                                        return false;
                                                        */
                                                    }
                                                }
                                            }
                                        }
                                    } else {
                                        //ORDEN DE PARADA NO VACIO

                                        //valida si tiene adyacente a la derecha
                                        objAux = buscaHopDerecha(listaHo[z], listaHo, codEquipo, tipoCentral);
                                        if (objAux) { //si existe hora de operación derecha 
                                            alert("Debe Borrar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[z].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[z].Hophorfin).format('HH:mm:ss'));
                                            return false;
                                        } else {
                                            horaInicio = moment(listaHo[z].Hophorini).format('HH:mm:ss');
                                            if (horaInicio == "00:00:00") {
                                                //verificar día anterior
                                                existeHoraOperAnterior = obtenerHoraOperacionDiaAnterior(codEquipo);
                                                if (existeHoraOperAnterior) {
                                                    alert("Existe Hora de Operación del día anterior con Hora de parada\n" +
                                                        "Debe Borrar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[z].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[z].Hophorfin).format('HH:mm:ss'));
                                                    return false;
                                                }
                                            }
                                        }
                                    }
                                }

                                //verificacion Orden de Arranque
                                //ORDEN DE ARRANQUE VACIO
                                if (listaHo[z].Hophorordarranq == null || listaHo[z].Hophorordarranq == "") {
                                    horaInicioAct = moment(listaHo[z].Hophorini).format('HH:mm:ss');
                                    if (horaInicioAct != "00:00:00") {
                                        objAux = buscaHopIzquierda(listaHo[z], listaHo, codEquipo, tipoCentral);
                                        if (!objAux) {
                                            /*
                                            * La validación de Orden de Arranque esta deshabilitada
                                            alert("Debe Ingresar Orden de Arraque para HO: " + nombEquipo + ": " + moment(listaHo[z].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[z].Hophorfin).format('HH:mm:ss'));
                                            return false;
                                            */
                                        }
                                    }
                                }

                                //ORDEN DE ARRANQUE NO VACIO
                                else {
                                    objAux = buscaHopIzquierda(listaHo[z], listaHo, codEquipo, tipoCentral);
                                    if (objAux) {
                                        alert("Debe Borrar Orden de Arranque para HO: " + nombEquipo + ": " + moment(listaHo[z].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[z].Hophorfin).format('HH:mm:ss'));
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            break;
        case 5: //Térmo

            //validacion por Modos de Operacion
            for (var i in evtHot.ListaModosOperacion) {
                codEquipo = evtHot.ListaModosOperacion[i].Grupocodi;
                nombEquipo = evtHot.ListaModosOperacion[i].Gruponomb;
                for (var j in listaHo) {
                    if (listaHo[j].OpcionCrud != -1 && listaHo[j].Grupocodi == codEquipo) {//Horas de operacion válidas

                        horaFin = moment(listaHo[j].Hophorfin).format('HH:mm:ss');
                        if (horaFin != "00:00:00") {//if (horaFin != "23:59:59") {

                            //verificacion Orden de parada
                            //ORDEN DE PARADA VACIO
                            if (listaHo[j].Hophorparada == null || listaHo[j].Hophorparada == "") {
                                //valida si tiene adyacente a la derecha
                                objAux = buscaHopDerecha(listaHo[j], listaHo, codEquipo, tipoCentral);
                                if (objAux) { //si existe hora de operación derecha 
                                    //OK

                                    //si hay unidades especiales a validar
                                    if (listaHo[j].MatrizunidadesExtra != null) {
                                        horaFin = listaHo[j].Hophorfin;
                                        existeUnidadDer = buscaUnidadesEspecialesDerecha(listaHo, horaFin, listaHo[j].MatrizunidadesExtra);
                                        if (!existeUnidadDer) {
                                            /*
                                            * La validación de Orden de Parada esta deshabilitada
                                            alert("Debe Ingresar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                            return false;
                                            */
                                        }
                                    }
                                } else {
                                    if (listaHo[j].Hopfalla != "F") {//No es Fuera de Servicio(F/S) por Falla
                                        horaInicio = moment(listaHo[j].Hophorini).format('HH:mm:ss');
                                        if (horaInicio == "00:00:00") {
                                            //verificar día anterior
                                            existeHoraOperAnterior = obtenerHoraOperacionDiaAnterior(codEquipo);
                                            if (!existeHoraOperAnterior) {
                                                /*
                                                * La validación de Orden de Parada esta deshabilitada
                                                alert("Debe Ingresar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                                return false;
                                                */
                                            }
                                        } else {
                                            //verificar hora actual
                                            fechaAct = new Date();
                                            min = moment(fechaAct).minute();
                                            fechaAct = moment(fechaAct).minute(min - 30);
                                            dateTo = new Date(moment(listaHo[j].Hophorfin));
                                            if (!moment(fechaAct).isBefore(dateTo)) {
                                                flagVerifUnid = verificaContinuidadUnidades(codEquipo, listaHo[j].Hophorfin); // verificamos continuidad en sus unidades realcionadas

                                                if (!flagVerifUnid) {
                                                    /*
                                                    * La validación de Orden de Parada esta deshabilitada
                                                    alert("Debe Ingresar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                                    return false;
                                                    */
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else {
                                //ORDEN DE PARADA NO VACIO
                                //valida si tiene adyacente a la derecha
                                objAux = buscaHopDerecha(listaHo[j], listaHo, codEquipo, tipoCentral);
                                if (objAux) { //si existe hora de operación derecha 

                                    //si hay unidades especiales a validar
                                    if (listaHo[j].MatrizunidadesExtra != null) {
                                        horaFin = listaHo[j].Hophorfin;
                                        existeUnidadDer = buscaUnidadesEspecialesDerecha(listaHo, horaFin, listaHo[j].MatrizunidadesExtra);
                                        if (existeUnidadDer) {
                                            alert("Debe Borrar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                            return false;
                                        }
                                    } else {
                                        alert("Debe Borrar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                        return false;
                                    }
                                } else {
                                    horaInicio = moment(listaHo[j].Hophorini).format('HH:mm:ss');
                                    if (horaInicio == "00:00:00") {
                                        //verificar día anterior
                                        existeHoraOperAnterior = obtenerHoraOperacionDiaAnterior(codEquipo);
                                        if (existeHoraOperAnterior) {
                                            alert("Existe Hora de Operación del día anterior con Hora de parada\n" +
                                                "Debe Borrar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                            return false;
                                        }
                                    } else {
                                        flagVerifUnid = verificaContinuidadUnidades(codEquipo, listaHo[j].Hophorfin); // verificamos continuidad en sus unidades realcionadas

                                        if (flagVerifUnid) {
                                            alert("Existe continuidad de Hora de Operación en unidades asociadas\n" +
                                                "Debe Borrar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                            return false;
                                        }
                                    }
                                }
                            }
                        }

                        //verificacion Orden de Arranque
                        //ORDEN DE ARRANQUE VACIO
                        if (listaHo[j].Hophorordarranq == null || listaHo[j].Hophorordarranq == "") {
                            horaInicioAct = moment(listaHo[j].Hophorini).format('HH:mm:ss');
                            if (horaInicioAct != "00:00:00") {
                                objAux = buscaHopIzquierda(listaHo[j], listaHo, codEquipo, tipoCentral);
                                if (!objAux) {
                                    horaInicio = listaHo[j].Hophorini;
                                    existeUnidadIzq = buscaUnidadesIzquierda(listaHo, codEquipo, horaInicio);
                                    if (!existeUnidadIzq) {
                                        /*
                                        * La validación de Orden de Arranque esta deshabilitada
                                        alert("Debe Ingresar Orden de Arraque para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                        return false;
                                        */
                                    }
                                } else {
                                    //si hay unidades especiales a validar
                                    if (listaHo[j].MatrizunidadesExtra != null) {
                                        //si hay horas de operacion a la izquierda, buscar si tienen unidades
                                        horaInicio = listaHo[j].Hophorini;
                                        existeUnidadIzq = buscaUnidadesEspecialesIzquierda(listaHo, horaInicio, listaHo[j].MatrizunidadesExtra);
                                        if (!existeUnidadIzq) {
                                            /*
                                            * La validación de Orden de Arranque esta deshabilitada
                                            alert("Debe Ingresar Orden de Arraque para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                            return false;
                                            */
                                        }
                                    }
                                }
                            }
                        }
                        //ORDEN DE ARRANQUE NO VACIO
                        else {
                            objAux = buscaHopIzquierda(listaHo[j], listaHo, codEquipo, tipoCentral);
                            if (objAux) {
                                //si hay unidades especiales a validar
                                if (listaHo[j].MatrizunidadesExtra != null) {
                                    horaInicio = listaHo[j].Hophorini;
                                    existeUnidadIzq = buscaUnidadesEspecialesIzquierda(listaHo, horaInicio, listaHo[j].MatrizunidadesExtra);
                                    if (existeUnidadIzq) {
                                        //alert("Debe Borrar Orden de Arranque para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                        //return false;
                                        listaHo[j].Hophorordarranq = "";
                                    }
                                } else {
                                    alert("Debe Borrar Orden de Arranque para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                    return false;
                                }
                            } else {
                                horaInicio = listaHo[j].Hophorini;
                                existeUnidadIzq = buscaUnidadesIzquierda(listaHo, codEquipo, horaInicio);
                                if (existeUnidadIzq) {
                                    alert("Debe Borrar Orden de Arranque para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            break;
        case 37: //Solares
        case 39: //Eolicas
            for (var i in evtHot.ListaCentrales) {
                codEquipo = evtHot.ListaCentrales[i].Equicodi;
                nombEquipo = evtHot.ListaCentrales[i].Equinomb;
                for (var j in listaHo) {
                    if (listaHo[j].OpcionCrud != -1 && listaHo[j].Equicodi == codEquipo) {//Horas de operacion válidas

                        //verificacion Orden de parada
                        if (listaHo[j].Hophorparada == null || listaHo[j].Hophorparada == "") {
                            //ORDEN DE PARADA VACIO

                            //no es necesario ingresar orden de parada
                        } else {
                            //ORDEN DE PARADA NO VACIO
                            /*
                            * La validación de Orden de Parada esta deshabilitada
                            alert("Debe Borrar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                            return false;
                            */
                        }

                        //verificacion Orden de Arranque
                        if (listaHo[j].Hophorordarranq == null || listaHo[j].Hophorordarranq == "") {
                            //ORDEN DE ARRANQUE VACIO

                            //no es necesario ingresar orden de arranque
                        }

                        //ORDEN DE ARRANQUE NO VACIO
                        else {
                            /*
                            * La validación de Orden de Arranque esta deshabilitada
                            alert("Debe Borrar Orden de Arranque para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                            return false;
                            */
                        }
                    }
                }
            }
            break;
    }
    return valor;
}

//verifica si existe hora de operacion adyacente al lado derecho
function buscaHopDerecha(objHoraOperacion, listaHo, codEquipo, tipoCentral) {
    horaFinHop = new Date(moment(objHoraOperacion.Hophorfin)); // hora fin o fuera de paralelo de la hora de operacion a comparar
    for (var i in listaHo) {
        switch (tipoCentral) {
            case 4: //Hidro
            case 37: //Solares
            case 39: //Eolicas
                if (listaHo[i].Hopcodi != objHoraOperacion.Hopcodi && listaHo[i].Equicodi == codEquipo && listaHo[i].OpcionCrud != -1) {
                    horaIni = new Date(moment(listaHo[i].Hophorini));
                    if (moment(horaFinHop).isSame(horaIni)) { // si existe continuidad
                        return true;
                    }
                }
                break;
            case 5: //Térmo
                if (listaHo[i].Hopcodi != objHoraOperacion.Hopcodi && listaHo[i].Grupocodi == codEquipo) {

                    horaIni = new Date(moment(listaHo[i].Hophorini));
                    if (moment(horaFinHop).isSame(horaIni)) { // si existe continuidad
                        return true;
                    }
                }
                break;
        }

    }
    return false;
}

//verifica si existe hora de operacion adyacente al lado izquierdo
function buscaHopIzquierda(objHoraOperacion, listaHo, codEquipo, tipoCentral) {
    horaIniHop = new Date(moment(objHoraOperacion.Hophorini)); // hora ini o paralelo de la hora de operacion a comparar
    for (var i in listaHo) {
        switch (tipoCentral) {
            case 4: //Hidro
            case 37: //Solares
            case 39: //Eolicas
                if (listaHo[i].Hopcodi != objHoraOperacion.Hopcodi && listaHo[i].Equicodi == codEquipo && listaHo[i].OpcionCrud != -1) {
                    horaFin = new Date(moment(listaHo[i].Hophorfin));
                    if (moment(horaIniHop).isSame(horaFin)) { // si existe continuidad
                        return true;
                    }
                }
                break;
            case 5: //Térmo
                if (listaHo[i].Hopcodi != objHoraOperacion.Hopcodi && listaHo[i].Grupocodi == codEquipo) {

                    horaFin = new Date(moment(listaHo[i].Hophorfin));
                    if (moment(horaIniHop).isSame(horaFin)) { // si existe continuidad
                        return true;
                    }
                }
                break;
        }

    }
    return false;
}

function buscaUnidadesIzquierda(listaHo, codEquipo, horaInicio) {
    var result = false;

    var idUnidTV = getCodigoTV();

    var horaIni = moment(horaInicio);

    if (evtHot.ListaUnidXModoOP.length > 0)
        for (var j = 0; j < evtHot.ListaUnidXModoOP.length; j++)
            if (evtHot.ListaUnidXModoOP[j].Grupocodi == codEquipo && evtHot.ListaUnidXModoOP[j].Equicodi != idUnidTV) {
                var encontrado = false;
                if (evtHot.ListaHorasOperacion.length > 0) {
                    for (var zz = 0; zz < evtHot.ListaHorasOperacion.length; zz++) {
                        //verificar que el registro no tenga eliminado lógico si vino de BD prOpiedad opCdrud != -1  y no sea una actualizacion                               
                        if (listaHo[zz].Equicodi == evtHot.ListaUnidXModoOP[j].Equicodi && listaHo[zz].OpcionCrud != -1
                            && listaHo[zz].Equicodi != idUnidTV) {

                            dateTo = new Date(moment(listaHo[zz].Hophorfin));
                            if (moment(horaIni).isSame(dateTo)) {
                                encontrado = true;
                                return encontrado;
                            }
                        }
                    }
                }
            }
    return result;
}

function buscaUnidadesEspecialesIzquierda(listaHo, horaInicio, ListaUniExtrasDia) {
    var result = false;

    if (ListaUniExtrasDia.length > 0) {
        for (var m = 0; m < ListaUniExtrasDia.length; m++) {
            var encontrado = false;
            if (listaHo.length > 0) {
                for (var nn = 0; nn < listaHo.length; nn++) {
                    //verificar que el registro no tenga eliminado lógico si vino de BD propiedad opCdrud != -1  y no sea una actualizacion                               
                    if (listaHo[nn].Equicodi == ListaUniExtrasDia[m].EquiCodi && listaHo[nn].OpcionCrud != -1) {
                        dateTo = new Date(moment(listaHo[nn].Hophorfin));
                        if (moment(horaInicio).isSame(dateTo)) {
                            encontrado = true;
                            return encontrado;
                        }
                    }
                }
            }
        }
    }
    return result;
}

function buscaUnidadesEspecialesDerecha(listaHo, horaFin, ListaUniExtrasDia) {
    var result = false;

    if (ListaUniExtrasDia.length > 0) {
        for (var m = 0; m < ListaUniExtrasDia.length; m++) {
            encontrado = false;
            if (listaHo.length > 0) {
                for (var nn = 0; nn < listaHo.length; nn++) {
                    //verificar que el registro no tenga eliminado lógico si vino de BD propiedad opCdrud != -1  y no sea una actualizacion                               
                    if (listaHo[nn].Equicodi == ListaUniExtrasDia[m].EquiCodi && listaHo[nn].OpcionCrud != -1) {
                        dateTo = new Date(moment(listaHo[nn].Hophorini));
                        if (moment(horaFin).isSame(dateTo)) {
                            encontrado = true;
                            return encontrado;
                        }
                    }
                }
            }
        }
    }
    return result;
}

//verifica continuidad lado derecho de horas de operacion de las unidades relacionadas a un modo de operacion 
function verificaContinuidadUnidades(codEquipo, horfin) {
    result = true;

    idUnidTV = getCodigoTV();

    hraFinModoOP = new Date(moment(horfin));
    if (evtHot.ListaUnidXModoOP.length > 0)
        for (var j = 0; j < evtHot.ListaUnidXModoOP.length; j++)
            if (evtHot.ListaUnidXModoOP[j].Grupocodi == codEquipo && evtHot.ListaUnidXModoOP[j].Equicodi != idUnidTV) {
                encontrado = false;
                if (evtHot.ListaHorasOperacion.length > 0) {
                    for (var zz = 0; zz < evtHot.ListaHorasOperacion.length; zz++) {
                        //verificar que el registro no tenga eliminado lógico si vino de BD prOpiedad opCdrud != -1                           
                        if (evtHot.ListaHorasOperacion[zz].Equicodi == evtHot.ListaUnidXModoOP[j].Equicodi && evtHot.ListaHorasOperacion[zz].OpcionCrud != -1
                            && evtHot.ListaHorasOperacion[zz].Equicodi != idUnidTV) {

                            dateIni = new Date(moment(evtHot.ListaHorasOperacion[zz].Hophorini));
                            if (moment(dateIni).isSame(hraFinModoOP)) {
                                encontrado = true;
                            }
                        }
                    }
                }
                result = result && encontrado;
            }
    return result;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Validación de la descripción (Observación)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function val_Descripcion(descripcion, modo, flagRFGeneradores, chkArranqBlackStart) {
    if (descripcion != null && descripcion.length > 600) {
        return 'La observación no debe exceder los 600 caracteres';
    }

    var regModoOp = getModoFromListaModo(evtHot.ListaModosOperacion, modo);
    if (regModoOp == null || regModoOp.Gruporeservafria != MODO_GRUPORESERVAFRIA) {
        return '';
    }

    //Validar que la central de RF debe indicar black start y 
    if (regModoOp != null && regModoOp.Gruporeservafria == MODO_GRUPORESERVAFRIA) {
        var descVal = descripcion.toUpperCase();
        if (chkArranqBlackStart == 1) {
            if (!(descVal.includes("BLACK") && descVal.includes("START"))) {
                return "Debe indicar en el campo 'Observación' si el arranque fue con Black Start";
            }
        }

        if (flagRFGeneradores == FLAG_GRUPORESERVAFRIA_TO_REGISTRAR_UNIDADES) {
            if (!(descVal.includes("OPER") || descVal.includes("UNIDAD") || descVal.includes("GENERADOR") || descVal.includes("GRUPO"))) {
                return "Debe indicar en el campo 'Observación' las unidades que operaron. \nPor ejemplo: \"... operaron las unidades G1 ...\"";
            }
        }
    }

    return '';
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Funciones que validan la UI
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ho_validarHoraOperacion(tipoHo, objRef, idTipoModo, tipoCentral, idPos, idTipoModo) {
    var modo = $("#cbModoOpGrupo").val();
    idTipoModo = parseInt(idTipoModo) || 0;

    $(objRef.id_ref_txtOrdenArranqueH).val(obtenerHoraValida($(objRef.id_ref_txtOrdenArranqueH).val()));
    $(objRef.id_ref_txtEnParaleloH).val(obtenerHoraValida($(objRef.id_ref_txtEnParaleloH).val()));
    $(objRef.id_ref_txtOrdenParadaH).val(obtenerHoraValida($(objRef.id_ref_txtOrdenParadaH).val()));
    $(objRef.id_ref_txtFueraParaleloH).val(obtenerHoraValida($(objRef.id_ref_txtFueraParaleloH).val()));

    actualizartxtFueraParaleloF(objRef.id_ref_txtFueraParaleloH, objRef.id_ref_txtFueraParaleloF);

    var enParalelo = $(objRef.id_ref_txtEnParaleloH).val();
    var fueraParalelo = $(objRef.id_ref_txtFueraParaleloH).val();
    var ordenArranque = $(objRef.id_ref_txtOrdenArranqueH).val();
    var ordenParada = $(objRef.id_ref_txtOrdenParadaH).val();

    var fOrdenParada = convertStringToDate($(objRef.id_ref_txtOrdenParadaF).val(), $(objRef.id_ref_txtOrdenParadaH).val());
    var fFueraParalelo = convertStringToDate($(objRef.id_ref_txtFueraParaleloF).val(), $(objRef.id_ref_txtFueraParaleloH).val());
    var fOrdenArranque = convertStringToDate($(objRef.id_ref_txtOrdenArranqueF).val(), $(objRef.id_ref_txtOrdenArranqueH).val());
    var fEnParalelo = convertStringToDate($(objRef.id_ref_txtEnParaleloF).val(), $(objRef.id_ref_txtEnParaleloH).val());

    // Verifica Orden de Arranque
    // si enParalelo = "00:00:00" -> ya no es necesario ordenArranque
    if (ordenArranque != "" && enParalelo != "" && enParalelo != "00:00:00") {
        var difHoras1 = moment(fOrdenArranque).isSameOrAfter(fEnParalelo);
        if (difHoras1) { // verifica que ambas horas sean ascendentes
            //$(objRef.id_ref_txtOrdenArranqueH).focus();
            return "La fecha 'En paralelo' es menor o igual que 'Orden de Arranque'";
        }
    }

    /*
    * La validación de Orden de Arranque esta deshabilitada
    if (ordenArranque == "" && enParalelo != "00:00:00") {
        tipocentral = tipoCentral;
        if (tipocentral != 5) {//Hidraulicas, solares, eolicas
            verificaOrdenArranque = verificaHorasAlternas($('#hfIdPos').val(), 1); // 1:analiza vecino izquierdo adyacente  
            if (!verificaOrdenArranque) {//no tiene vecino
                if (evtHot.IdTipoCentral != 37 && evtHot.IdTipoCentral != 39) { // si no es solar ni eolica mandar mensaje de validacion
                    alert("Debe ingresar 'Orden de Arranque'");
                    $(objRef.id_ref_txtOrdenArranqueH).focus();
                    return false;
                }
            }
        }
        else {////Térmicas
            flagUnid = verificaUnidPreviasencendidas(idPos, idTipoModo);
            //alert(flagUnid);
            if (!flagUnid) {//sin no existen unidades previas adyaventes en funcionamiento 
                //verificaOrdenArranque = verificaHorasAlternas($('#hfIdPos').val(), 1); // 1:analiza vecino izquierdo adyacente            
                //if (!verificaOrdenArranque) {
                alert("Debe ingresar 'Orden de Arranque'");
                $(objRef.id_ref_txtOrdenArranqueH).focus();
                return false;
                // }
            }
        }
    }
    */

    //verifica hora enParalelo
    // si enParalelo = "00:00:00" -> ya no es necesario ordenArranque

    if (enParalelo != "" && ordenParada != "") {
        var difHoras3 = moment(fEnParalelo).isSameOrAfter(fOrdenParada);
        if (difHoras3) {
            //$(objRef.id_ref_txtEnParaleloH).focus();
            return "La fecha 'Orden de Parada' es menor o igual que 'En Paralelo'";
        }
    }

    if (enParalelo == "") {
        //$(objRef.id_ref_txtEnParaleloH).focus();
        return "Debe ingresar valor para la fecha 'En Paralelo'";
    }

    if (fOrdenArranque == fEnParalelo) {
        if (enParalelo == "00:00:00" && ordenArranque != "") {
            //$(objRef.id_ref_txtOrdenArranqueH).focus();
            return "No debe tener valor 'Orden de Arranque'";
        }
    }

    //Verifica orden de parada si existe
    if (ordenParada != "" && fueraParalelo != "") {
        var difHoras2 = moment(fOrdenParada).isSameOrAfter(fFueraParalelo);
        if (difHoras2) {
            //$(objRef.id_ref_txtOrdenParadaH).focus();
            return "La fecha 'Fuera de Paralelo' es menor o igual que 'Orden de Parada'";
        }
    }

    // verifica fuera de paralelo
    if (fueraParalelo == "") {
        //$(objRef.id_ref_txtFueraParaleloH).focus();
        return "Debe ingresar valor para la fecha 'Fuera de Paralelo'";
    }

    // valida la hora fin a las 24:00:00 hs
    if ($(objRef.id_ref_txtEnParaleloF).val() != $(objRef.id_ref_txtFueraParaleloF).val()) {
        if ($(objRef.id_ref_txtFueraParaleloH).val() != "00:00:00") {
            return "La fecha 'Fuera de Paralelo' debe ser '00:00:00'";
        }
    }

    var difHoras = moment(fEnParalelo).isSameOrAfter(fFueraParalelo);
    if (difHoras) {
        //$(objRef.id_ref_txtEnParaleloH).focus();
        return "La fecha 'Fuera de Paralelo' es menor o igual que 'En Paralelo'";
    }

    if (TIPO_HO_MODO == tipoHo) {

        //Validar Central solar
        if (TIPO_CENTRAL_SOLAR == evtHot.IdTipoCentral) {
            var paramSolarHoraIni = $("#hfParamSolarHoraIni").val() + ":00";
            var paramSolarHoraFin = $("#hfParamSolarHoraFin").val() + ":00";
            // si es central solar la hora en paralelo debe ser mayor o igual a 05:15am
            var fechaValEnParaleloMin = convertStringToDate($('#txtEnParaleloF').val(), paramSolarHoraIni);
            var diff = moment(fEnParalelo).isBefore(moment(fechaValEnParaleloMin));
            if (diff) {
                return "La fecha 'En Paralelo' debe ser mayor o igual a " + paramSolarHoraIni;
                //$('#txtEnParaleloH').focus();
            }

            // si es central solar la hora fuera de paralelo debe ser menor o igual a 19:30am
            var fechaValFueraParaleloMax = convertStringToDate($('#txtEnParaleloF').val(), paramSolarHoraFin);
            var diff = moment(fFueraParalelo).isAfter(fechaValFueraParaleloMax);
            if (diff) {
                return "La fecha 'Fuera de Paralelo' debe ser menor o igual que " + $('#txtEnParaleloF').val() + " " + paramSolarHoraFin;
                //$('#txtFueraParaleloH').focus();
            }
        }

        //Validar descripción
        var observacion = $("#txtObservacion").val();
        var flagRFGeneradores = parseInt($('#hfFlagCentralRsvFriaToRegistrarUnidad').val());
        var chkArranqBlackStart = document.getElementById("chkArranqueBlackStart").checked;
        var msjDescripcion = val_Descripcion(observacion, modo, flagRFGeneradores, chkArranqBlackStart);
        if (msjDescripcion != '') {
            return msjDescripcion;
        }
    }

    return '';
}