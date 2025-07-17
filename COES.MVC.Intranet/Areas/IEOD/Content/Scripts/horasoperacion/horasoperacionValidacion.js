var MSJ_VAL_CRUCE_HO = "Existen cruces en horas de operación. Debe eliminar y/o actualizar horas de operación.";
var MSJ_VAL_CRUCE_UNID = "¡Existe Cruce de Horas en las unidades!";
var MSJ_VAL_CONTINUIDAD_UNID = "¡Existe continuidad en las unidades, elimine o actualice!";
var MSJ_VAL_MODO_VACIO = 'No existe continuidad de la operación';
var MSJ_VAL_MODO_SIN_UNIDAD = "Debe asignar al menos una unidad correspondiente a la hora de operación";
var MSJ_VAL_HO_INVALIDA = 'Existe Hora de operación no válida';
var DESG_VAL_INCOMPLETO = 'El desglose tiene campos incompletos';
var DESG_VAL_INCLUIDO = 'El desglose esta fuera del intervalo de la Hora de Operación';
var DESG_VAL_RANGO_NO_VALIDO = ': La Fecha de Inicio no debe ser mayor o igual a la Fecha de Fin';

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Funcionalidad para la Orden de Arranque, En Paralelo, Orden de Parada, Fuera Paralelo
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//verifica horas de operacion antes de enviar a grabar
function validarHorasOperacionArranqueYParada(listaHo) {
    var valor = true;
    var grupocodiModo = 0;
    var nombEquipo = "";

    //validacion por Modos de Operacion
    for (var j in listaHo) {
        grupocodiModo = listaHo[j].Grupocodi;

        var objModo = getModoFromListaModo(GLOBAL_HO.ListaModosOperacion, grupocodiModo);
        if (objModo != null) nombEquipo = objModo.Gruponomb;

        if (listaHo[j].OpcionCrud != -1) {//Horas de operacion válidas (no eliminados)

            horaFin = moment(listaHo[j].Hophorfin).format('HH:mm:ss');
            if (horaFin != "00:00:00") {//if (horaFin != "23:59:59") {

                //verificacion Orden de parada
                //ORDEN DE PARADA VACIO
                if (listaHo[j].Hophorparada == null || listaHo[j].Hophorparada == "") {
                    //no validar
                }
                else {
                    //ORDEN DE PARADA NO VACIO
                    //valida si tiene adyacente a la derecha
                    objAux = buscaHopDerecha(listaHo[j], listaHo, grupocodiModo);
                    if (objAux) { //si existe hora de operación derecha 

                        alert("Debe Borrar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                        return false;
                    } else {
                        horaInicio = moment(listaHo[j].Hophorini).format('HH:mm:ss');
                        if (horaInicio == "00:00:00") {
                            //verificar día anterior
                            existeHoraOperAnterior = obtenerHoraOperacionDiaAnterior(grupocodiModo);
                            if (existeHoraOperAnterior) {
                                alert("Existe Hora de Operación del día anterior con Hora de parada\n" +
                                    "Debe Borrar Orden de Parada para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                                return false;
                            }
                        } else {
                            flagVerifUnid = verificaContinuidadUnidades(grupocodiModo, listaHo[j].Hophorfin, listaHo); // verificamos continuidad en sus unidades realcionadas

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
                //no validar
            }
            else {
                //ORDEN DE ARRANQUE NO VACIO
                objAux = buscaHopIzquierda(listaHo[j], listaHo, grupocodiModo);
                if (objAux) {
                    //si hay unidades especiales a validar
                    if (listaHo[j].MatrizunidadesExtra != null && listaHo[j].MatrizunidadesExtra.length > 0) {
                        horaInicio = listaHo[j].Hophorini;
                        existeUnidadIzq = buscaUnidadesEspecialesIzquierda(listaHo, horaInicio, listaHo[j].MatrizunidadesExtra);
                        if (existeUnidadIzq) {
                            listaHo[j].Hophorordarranq = "";
                        }
                    } else {
                        alert("Debe Borrar Orden de Arranque para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                        return false;
                    }
                } else {
                    horaInicio = listaHo[j].Hophorini;
                    existeUnidadIzq = buscaUnidadesIzquierda(listaHo, grupocodiModo, horaInicio);
                    if (existeUnidadIzq) {
                        alert("Debe Borrar Orden de Arranque para HO: " + nombEquipo + ": " + moment(listaHo[j].Hophorini).format('HH:mm:ss') + " - " + moment(listaHo[j].Hophorfin).format('HH:mm:ss'));
                        return false;
                    }
                }
            }
        }
    }

    return valor;
}

//verifica si existe hora de operacion adyacente al lado derecho
function buscaHopDerecha(objHoraOperacion, listaHo, grupocodiModo) {
    horaFinHop = new Date(moment(objHoraOperacion.Hophorfin)); // hora fin o fuera de paralelo de la hora de operacion a comparar
    for (var i in listaHo) {
        if (listaHo[i].Hopcodi != objHoraOperacion.Hopcodi && listaHo[i].Grupocodi == grupocodiModo) {

            horaIni = new Date(moment(listaHo[i].Hophorini));
            if (moment(horaFinHop).isSame(horaIni)) { // si existe continuidad
                return true;
            }
        }

    }
    return false;
}

//verifica si existe hora de operacion adyacente al lado izquierdo
function buscaHopIzquierda(objHoraOperacion, listaHo, grupocodiModo) {
    horaIniHop = new Date(moment(objHoraOperacion.Hophorini)); // hora ini o paralelo de la hora de operacion a comparar
    for (var i in listaHo) {
        if (listaHo[i].Hopcodi != objHoraOperacion.Hopcodi && listaHo[i].Grupocodi == grupocodiModo) {

            horaFin = new Date(moment(listaHo[i].Hophorfin));
            if (moment(horaIniHop).isSame(horaFin)) { // si existe continuidad
                return true;
            }
        }

    }
    return false;
}

function buscaUnidadesIzquierda(listaHo, grupocodiModo, horaInicio) {

    var idUnidTV = getCodigoTV(grupocodiModo);
    var horaIni = moment(horaInicio);

    if (listaHo.length > 0) {
        for (var zz = 0; zz < listaHo.length; zz++) {
            //verificar que el registro no tenga eliminado lógico si vino de BD prOpiedad opCdrud != -1  y no sea una actualizacion
            if (listaHo[zz].OpcionCrud != -1 && listaHo[zz].Grupocodi == grupocodiModo) {

                for (var i = 0; i < listaHo[zz].ListaHoUnidad.length; i++) {
                    var equicodi = listaHo[zz].ListaHoUnidad[i].Equicodi;

                    if (equicodi != idUnidTV) {

                        var dateTo = new Date(moment(listaHo[zz].ListaHoUnidad[i].Hophorfin));
                        if (moment(horaIni).isSame(dateTo)) {
                            return true;
                        }
                    }
                }
            }
        }
    }
    return false;
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
function verificaContinuidadUnidades(grupocodiModo, horfin, listaHo) {
    var idUnidTV = getCodigoTV(grupocodiModo);
    var hraFinModoOP = new Date(moment(horfin));

    if (listaHo.length > 0) {
        for (var zz = 0; zz < listaHo.length; zz++) {
            //verificar que el registro no tenga eliminado lógico si vino de BD prOpiedad opCdrud != -1     
            if (listaHo[zz].OpcionCrud != -1 && listaHo[zz].Grupocodi == grupocodiModo) {

                for (var i = 0; i < listaHo[zz].ListaHoUnidad.length; i++) {
                    var equicodi = listaHo[zz].ListaHoUnidad[i].Equicodi;
                    if (equicodi != idUnidTV) {

                        var dateIni = new Date(moment(listaHo[zz].ListaHoUnidad[i].Hophorini));
                        if (moment(dateIni).isSame(hraFinModoOP)) {
                            return true;
                        }
                    }
                }
            }
        }
    }

    return false;
}

//indica si existe hora de operacion con hora de parada mayor o igual a las 23:00:00 hs.
function obtenerHoraOperacionDiaAnterior(grupocodiModo) {
    var sFecha = $('#txtFecha').val();
    var checkdateFrom = convertStringToDate(sFecha, "23:59:59");

    for (var z in GLOBAL_HO.ListaHorasOperacionAnt) {
        if (GLOBAL_HO.ListaHorasOperacionAnt[z].Grupocodi == grupocodiModo) {
            horaFinAnt = moment(GLOBAL_HO.ListaHorasOperacionAnt[z].Hophorfin);
            horaFinAntFormato = horaFinAnt.format('HH:mm:ss');
            horaParAnt = moment(GLOBAL_HO.ListaHorasOperacionAnt[z].Hophorparada);
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

    return false;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Funcionalidad para la Orden de Arranque
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function val_OrdenArranqueCentralReservaFria(listaHorasOperacion) {
    //considerar modos que tienen reserva fria    
    var listaHIniHFin = [];
    for (var i = 0; i < listaHorasOperacion.length; i++) {
        var reg = listaHorasOperacion[i];
        var regModoOp = getModoFromListaModo(GLOBAL_HO.ListaModosOperacion, reg.Grupocodi);
        if (regModoOp.Gruporeservafria == MODO_GRUPORESERVAFRIA) {

            var obj = {
                Pos: i,
                Grupocodi: reg.Grupocodi,
                Hophorini: reg.Hophorini,
                Hophorfin: reg.Hophorfin,
                Hophorordarranq: reg.Hophorordarranq,
                Hophorparada: reg.Hophorparada
            };
            listaHIniHFin.push(obj);
        }
    }

    //ordenar ho
    ordenarListaHorasOperacion(listaHIniHFin);

    if (listaHIniHFin.length > 0) {
        //la primera hora de operación debe tener orden de arranque cuando es reserva fria
        var objMenorHoraIni = listaHIniHFin[0];
        if ((objMenorHoraIni.Hophorordarranq == null || objMenorHoraIni.Hophorordarranq == "")) {
            return "Debe Ingresar Orden de Arranque a la hora de operación inicial.";
        }
    }

    return '';
}

function val_OrdenArranquePruebaAleatoria(listaHorasOperacion) {
    //considerar todos los modos visibles en la pestaña "Detalle"
    var listaHIniHFin = [];
    for (var i = 0; i < listaHorasOperacion.length; i++) {
        var reg = listaHorasOperacion[i];

        var obj = {
            Pos: i,
            Grupocodi: reg.Grupocodi,
            Hophorini: reg.Hophorini,
            Hophorfin: reg.Hophorfin,
            Hophorordarranq: reg.Hophorordarranq,
            Hophorparada: reg.Hophorparada
        };
        listaHIniHFin.push(obj);
    }

    //si algun formulario tiene prueba aleatoria
    if (!tieneCalificacionFromListaHO(listaHIniHFin, SUBCAUSACODI_PRUEBA_ALEATORIA_PR25)) {
        return '';
    }

    //ordenar ho
    ordenarListaHorasOperacion(listaHIniHFin);

    if (listaHIniHFin.length > 0) {
        //la primera hora de operación debe tener orden de arranque cuando es reserva fria
        var objMenorHoraIni = listaHIniHFin[0];
        if ((objMenorHoraIni.Hophorordarranq == null || objMenorHoraIni.Hophorordarranq == "")) {
            return "Debe Ingresar Orden de Arranque a la hora de operación inicial.";
        }
    }

    return '';
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Validación de la descripción
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function val_Descripcion(descripcion, modo, flagRFGeneradores, chkArranqBlackStart) {
    if (descripcion != null && descripcion.length > 600) {
        return 'La descripción no debe exceder los 600 caracteres';
    }

    var regModoOp = getModoFromListaModo(GLOBAL_HO.ListaModosOperacion, modo);
    if (regModoOp == null || regModoOp.Gruporeservafria != MODO_GRUPORESERVAFRIA) {
        return '';
    }

    //Validar que la central de RF debe indicar black start y 
    if (regModoOp != null && regModoOp.Gruporeservafria == MODO_GRUPORESERVAFRIA) {
        var descVal = descripcion.toUpperCase();
        if (chkArranqBlackStart == 1) {
            if (!(descVal.includes("BLACK") && descVal.includes("START"))) {
                return "Debe indicar en el campo 'Descripción' si el arranque fue con Black Start";
            }
        }

        if (flagRFGeneradores == FLAG_GRUPORESERVAFRIA_TO_REGISTRAR_UNIDADES) {
            if (!(descVal.includes("OPER") || descVal.includes("UNIDAD") || descVal.includes("GENERADOR") || descVal.includes("GRUPO"))) {
                return "Debe indicar en el campo 'Descripción' las unidades que operaron. \nPor ejemplo: \"... operaron las unidades G1 ...\"";
            }
        }
    }

    return '';
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Funciones que validan la UI
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ho_validarHoraOperacion(tipoHo, objRef, idTipoModo, tipoCentral, idPos, numero) {

    var modo = $('#detalle' + numero + ' #cbModoOpGrupo').val();
    idTipoModo = parseInt(idTipoModo) || 0;

    $('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenArranqueH).val(obtenerHoraValida($('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenArranqueH).val()));
    $('#detalle' + numero + ' ' + objRef.id_ref_txtEnParaleloH).val(obtenerHoraValida($('#detalle' + numero + ' ' + objRef.id_ref_txtEnParaleloH).val()));
    $('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenParadaH).val(obtenerHoraValida($('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenParadaH).val()));
    $('#detalle' + numero + ' ' + objRef.id_ref_txtFueraParaleloH).val(obtenerHoraValida($('#detalle' + numero + ' ' + objRef.id_ref_txtFueraParaleloH).val()));

    actualizartxtFueraParaleloF('#detalle' + numero + ' ' + objRef.id_ref_txtFueraParaleloH, '#detalle' + numero + ' ' + objRef.id_ref_txtFueraParaleloF, numero);

    var enParalelo = $('#detalle' + numero + ' ' + objRef.id_ref_txtEnParaleloH).val();
    var fueraParalelo = $('#detalle' + numero + ' ' + objRef.id_ref_txtFueraParaleloH).val();
    var ordenArranque = $('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenArranqueH).val();
    var ordenParada = $('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenParadaH).val();

    var fOrdenParada = convertStringToDate($('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenParadaF).val(), $('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenParadaH).val());
    var fFueraParalelo = convertStringToDate($('#detalle' + numero + ' ' + objRef.id_ref_txtFueraParaleloF).val(), $('#detalle' + numero + ' ' + objRef.id_ref_txtFueraParaleloH).val());
    var fOrdenArranque = convertStringToDate($('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenArranqueF).val(), $('#detalle' + numero + ' ' + objRef.id_ref_txtOrdenArranqueH).val());
    var fEnParalelo = convertStringToDate($('#detalle' + numero + ' ' + objRef.id_ref_txtEnParaleloF).val(), $('#detalle' + numero + ' ' + objRef.id_ref_txtEnParaleloH).val());

    // Verifica Orden de Arranque
    // si enParalelo = "00:00:00" -> ya no es necesario ordenArranque
    if (ordenArranque != "" && enParalelo != "" && enParalelo != "00:00:00") {
        var difHoras1 = moment(fOrdenArranque).isSameOrAfter(fEnParalelo);
        if (difHoras1) { // verifica que ambas horas sean ascendentes
            return "La fecha 'En paralelo' es menor o igual que 'Orden de Arranque'";
        }
    }

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
            return "No debe tener valor 'Orden de Arranque'";
        }
    }

    //Verifica orden de parada si existe
    if (ordenParada != "" && fueraParalelo != "") {
        var difHoras2 = moment(fOrdenParada).isSameOrAfter(fFueraParalelo);
        if (difHoras2) {
            return "La fecha 'Fuera de Paralelo' es menor o igual que 'Orden de Parada'";
        }
    }

    // verifica fuera de paralelo
    if (fueraParalelo == "") {
        return "Debe ingresar valor para la fecha 'Fuera de Paralelo'";
    }

    // valida la hora fin a las 24:00:00 hs
    if ($('#detalle' + numero + ' ' + objRef.id_ref_txtEnParaleloF).val() != $('#detalle' + numero + ' ' + objRef.id_ref_txtFueraParaleloF).val()) {
        if ($('#detalle' + numero + ' ' + objRef.id_ref_txtFueraParaleloH).val() != "00:00:00") {
            return "La fecha 'Fuera de Paralelo' debe ser '00:00:00'";
        }
    }

    var difHoras = moment(fEnParalelo).isSameOrAfter(fFueraParalelo);
    if (difHoras) {
        return "La fecha 'Fuera de Paralelo' es menor o igual que 'En Paralelo'";
    }

    if (TIPO_HO_MODO == tipoHo) {
        //Validar descripción
        var descripcion = $('#detalle' + numero + ' #TxtDescripcion').val();
        var flagRFGeneradores = parseInt($('#detalle' + numero + ' #hfFlagCentralRsvFriaToRegistrarUnidad').val());
        var chkArranqBlackStart = $('#detalle' + numero + ' #chkArranqueBlackStart').prop("checked");
        var msjDescripcion = val_Descripcion(descripcion, modo, flagRFGeneradores, chkArranqBlackStart);
        if (msjDescripcion != '') {
            return msjDescripcion;
        }
    }

    return '';
}

function ho_validarHoraOperacionEnGrid(tipoHo, objRef, idTipoModo) {
    var modo = objRef.cbModoOpGrupo;
    idTipoModo = parseInt(idTipoModo) || 0;

    objRef.id_ref_txtOrdenArranqueH = obtenerHoraValida(objRef.id_ref_txtOrdenArranqueH);
    objRef.id_ref_txtEnParaleloH = obtenerHoraValida(objRef.id_ref_txtEnParaleloH);
    objRef.id_ref_txtOrdenParadaH = obtenerHoraValida(objRef.id_ref_txtOrdenParadaH);
    objRef.id_ref_txtFueraParaleloH = obtenerHoraValida(objRef.id_ref_txtFueraParaleloH);

    var enParalelo = objRef.id_ref_txtEnParaleloH;
    var fueraParalelo = objRef.id_ref_txtFueraParaleloH;
    var ordenArranque = objRef.id_ref_txtOrdenArranqueH;
    var ordenParada = objRef.id_ref_txtOrdenParadaH;

    var fOrdenParada = convertStringToDate(objRef.id_ref_txtOrdenParadaF, objRef.id_ref_txtOrdenParadaH);
    var fFueraParalelo = convertStringToDate(objRef.id_ref_txtFueraParaleloF, objRef.id_ref_txtFueraParaleloH);
    var fOrdenArranque = convertStringToDate(objRef.id_ref_txtOrdenArranqueF, objRef.id_ref_txtOrdenArranqueH);
    var fEnParalelo = convertStringToDate(objRef.id_ref_txtEnParaleloF, objRef.id_ref_txtEnParaleloH);

    if (objRef.id_ref_txtFueraParaleloH == "00:00:00") {
        fFueraParalelo.setDate(fFueraParalelo.getDate() + 1);
    }

    // Verifica Orden de Arranque
    // si enParalelo = "00:00:00" -> ya no es necesario ordenArranque
    if (ordenArranque != "" && enParalelo != "" && enParalelo != "00:00:00") {
        var difHoras1 = moment(fOrdenArranque).isSameOrAfter(fEnParalelo);
        if (difHoras1) { // verifica que ambas horas sean ascendentes
            return "La fecha 'En paralelo' es menor o igual que 'Orden de Arranque'";
        }
    }

    //verifica hora enParalelo
    // si enParalelo = "00:00:00" -> ya no es necesario ordenArranque

    if (enParalelo != "" && ordenParada != "") {
        var difHoras3 = moment(fEnParalelo).isSameOrAfter(fOrdenParada);
        if (difHoras3) {
            return "La fecha 'Orden de Parada' es menor o igual que 'En Paralelo'";
        }
    }

    if (enParalelo == "") {
        return "Debe ingresar valor para la fecha 'En Paralelo'";
    }

    if (fOrdenArranque == fEnParalelo) {
        if (enParalelo == "00:00:00" && ordenArranque != "") {
            return "No debe tener valor 'Orden de Arranque'";
        }
    }

    //Verifica orden de parada si existe
    if (ordenParada != "" && fueraParalelo != "") {
        var difHoras2 = moment(fOrdenParada).isSameOrAfter(fFueraParalelo);
        if (difHoras2) {
            return "La fecha 'Fuera de Paralelo' es menor o igual que 'Orden de Parada'";
        }
    }

    // verifica fuera de paralelo
    if (fueraParalelo == "") {
        return "Debe ingresar valor para la fecha 'Fuera de Paralelo'";
    }

    // valida la hora fin a las 24:00:00 hs
    if (objRef.id_ref_txtEnParaleloF != objRef.id_ref_txtFueraParaleloF) {
        if (objRef.id_ref_txtFueraParaleloH != "00:00:00") {
            return "La fecha 'Fuera de Paralelo' debe ser '00:00:00'";
        }
    }

    var difHoras = moment(fEnParalelo).isSameOrAfter(fFueraParalelo);
    if (difHoras) {
        return "La fecha 'Fuera de Paralelo' es menor o igual que 'En Paralelo'";
    }

    return '';
}

function desg_validarCruceDesglose(listaHoraDesg, listaHo) {
    var arrayObj = [];
    var horaDesc = '';
    if (listaHoraDesg.length > 0) {
        arrayObj = getHoraoperacionYGrupoByHopcodi(listaHo, GLOBAL_HO.ListaModosOperacion, listaHoraDesg[0].Hopcodi);
        if (arrayObj.length == 2) {
            horaDesc = arrayObj[1] != null ? arrayObj[1].Gruponomb : '';
            horaDesc += arrayObj[0] != null ? ', EN PARALELO [' + moment(arrayObj[0].Hophorini).format('DD/MM/YYYY') + ' ' + moment(arrayObj[0].Hophorini).format('HH:mm:ss')
                + '], FUERA DE PARALELO [' + moment(arrayObj[0].Hophorfin).format('DD/MM/YYYY') + ' ' + moment(arrayObj[0].Hophorfin).format('HH:mm:ss') + ']' + ':\n' : '';
        }
    }

    for (var mm = 0; mm < listaHoraDesg.length; mm++) {
        var horaIni = new Date(moment(listaHoraDesg[mm].Ichorini));
        var horaFin = new Date(moment(listaHoraDesg[mm].Ichorfin));
        for (var zz = 0; zz < listaHoraDesg.length; zz++) {
            if (zz != mm) {
                var msjVal = desg_mensajeCruceDesglose(listaHoraDesg[mm].TipoDesglose, listaHoraDesg[zz].TipoDesglose);
                var dateFrom = new Date(moment(listaHoraDesg[zz].Ichorini));
                var dateTo = new Date(moment(listaHoraDesg[zz].Ichorfin));

                if (moment(horaIni).isBetween(dateFrom, dateTo)
                    || moment(horaFin).isBetween(dateFrom, dateTo)
                    || moment(dateFrom).isBetween(moment(horaIni), moment(horaFin))
                    || moment(dateTo).isBetween(moment(horaIni), moment(horaFin))
                    || moment(dateFrom).isSame(moment(horaIni)) && moment(dateTo).isSame(moment(horaFin))
                )
                    return horaDesc + msjVal;
            }
        }
    }

    return '';
}

function desg_mensajeCruceDesglose(tipoDesgA, tipoB) {
    return desgl_NombreTipoDesglose(tipoDesgA) + " tiene cruce con " + desgl_NombreTipoDesglose(tipoB);
}

function validarDesgloseXHorasOperacion(listaHo) {
    var valor = true;

    //validacion por Modos de Operacion
    for (var j in listaHo) {
        var grupocodiModo = listaHo[j].Grupocodi;
        if (listaHo[j].OpcionCrud != -1) {//Horas de operacion válidas (no eliminados)

            if (APP_OPCION != OPCION_ELIMINAR) {
                var msjValDesg = desg_validarData(listaHo[j].ListaDesglose, j, listaHo, j + 1);
                if (msjValDesg != '') {
                    alert(msjValDesg);
                    return false;
                }
            }
        }
    }

    return valor;
}