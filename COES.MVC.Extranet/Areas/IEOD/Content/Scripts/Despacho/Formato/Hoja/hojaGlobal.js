var modoLectura = false;
//opciones
var OPCION_CONSULTAR = 1;
var OPCION_ENVIAR_DATOS = 2;
var OPCION_ENVIO_ANTERIOR = 3;
var OPCION_IMPORTAR_DATOS = 5;
var CANTIDAD_CLICK_IMPORTAR = 0;
var CANTIDAD_CLICK_TIPO_FORMATO = 0;
var VER_ULTIMO_ENVIO = 1;
var VER_ENVIO = 2;
var NO_VER_ULTIMO_ENVIO = 0;
var OPCION_CONSULTAR_HOY = 6;

//constantes error
var ERROR_BLANCO = 0;
var ERROR_NUMERO = 1;
var ERROR_NO_NUMERO = 2;
var ERROR_LIM_INFERIOR = 3;
var ERROR_LIM_SUPERIOR = 4;
var ERROR_RANGO_FECHA = 5;
var ERROR_CRUCE_PERIODO = 6;
var ERROR_TIME = 7;
var ERROR_EXTREMO_TIME = 8;
var ERROR_UNIDAD = 9;
var ERROR_NUMERO_NEGATIVO = 10;
var ERROR_DATA_CENTRAL_SOLAR = 11;
var ERROR_DATA_INTERCONEXION = 12;
var ERROR_DATA_DESPACHO = 13;

var ERROR_GLOBAL = [
    { tipo: 'BLANCO', Descripcion: 'BLANCO', total: 0, codigo: ERROR_BLANCO, BackgroundColor: '', Color: '', validar: false },
    { tipo: 'NUMERO', Descripcion: 'NÚMERO', total: 0, codigo: ERROR_NUMERO, BackgroundColor: 'white', Color: '', validar: false },
    { tipo: 'NONUMERO', Descripcion: 'El dato no es númerico', total: 0, codigo: ERROR_NO_NUMERO, BackgroundColor: 'red', Color: '', validar: false },
    { tipo: 'LIMINF', Descripcion: "El dato es menor que el límite inferior", total: 0, codigo: ERROR_LIM_INFERIOR, BackgroundColor: 'orange', Color: '', validar: false },
    { tipo: 'LIMSUP', Descripcion: 'El dato es mayor que el límite superior', total: 0, codigo: ERROR_LIM_SUPERIOR, BackgroundColor: 'yellow', Color: '', validar: false },
    { tipo: 'RANFEC', Descripcion: 'RANGO DE FECHA INVALIDO', total: 0, codigo: ERROR_RANGO_FECHA, BackgroundColor: '#FF4C42', Color: 'white', validar: false },
    { tipo: 'CRUPER', Descripcion: 'CRUCE EN PERIODOS', total: 0, codigo: ERROR_CRUCE_PERIODO, BackgroundColor: 'Wheat', Color: 'black', validar: false },
    { tipo: 'ERRTIME', Descripcion: 'TIME INVALIDO', total: 0, codigo: ERROR_TIME, BackgroundColor: 'SandyBrown', Color: 'black', validar: false },
    { tipo: 'ERREXTREMOTIME', Descripcion: 'FECHA FUERA DE RANGO', total: 0, codigo: ERROR_EXTREMO_TIME, BackgroundColor: 'MediumTurquoise', Color: 'black', validar: false },
    { tipo: 'ERRUNIDAD', Descripcion: "Unidades de MW y MVar no estan completadas", total: 0, codigo: ERROR_UNIDAD, BackgroundColor: '#86c2ad', Color: '', validar: false },
    { tipo: 'NUMERONEGATIVO', Descripcion: "El dato debe mayor o igual a 0", total: 0, codigo: ERROR_NUMERO_NEGATIVO, BackgroundColor: 'orange', Color: '', validar: false },
    { tipo: 'ERROR_DATA_CENTRAL_SOLAR', Descripcion: "El dato debe ser 0", total: 0, codigo: ERROR_DATA_CENTRAL_SOLAR, BackgroundColor: 'white', Color: '', validar: false },
    { tipo: 'ERROR_DATA_INTERCONEXION', Descripcion: "VALOR DE EXPORTACIÓN/IMPORTACIÓN MWh DEBE TENER VALOR", total: 0, codigo: ERROR_DATA_INTERCONEXION, BackgroundColor: '#B8E2FB', Color: '', validar: false },
    { tipo: 'ERRDESPACHO', Descripcion: "El dato no esta completado", total: 0, codigo: ERROR_DATA_DESPACHO, BackgroundColor: '#bfdfe8', Color: '', validar: false },
];

imagenOk = "<img src='" + siteRoot + "Content/Images/ico-done.gif'/>";
imagenError = "<img src='" + siteRoot + "Content/Images/ico-delete.gif'/>";

//Tipo Informacion
var TIPOINFOCODI_KV = 5;
var TIPOINFOCODI_A = 9;

//Tipo Ptoinformacion
var TIPO_PTOINFOCODI_EXPORTACION_MWh = 20;
var TIPO_PTOINFOCODI_IMPORTACION_MWh = 21;

//app
var APP_GENERACION_RER = 1;

//////////////////////////////////////////////////////////
//// btnSelectExcel
//////////////////////////////////////////////////////////
function limpiarError(numHoja) {
    setListaErrores([], numHoja);
}

//////////////////////////////////////////////////////////
//// btnMostrarErrores
//////////////////////////////////////////////////////////
function mostrarDetalleErrores(numHoja) {
    $('#validaciones').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false
    });

    setTimeout(function () {
        $('#idTerrores').html(dibujarTablaError(numHoja));

        $('#tablaError').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 200);
}

function dibujarTablaError(numHoja) {
    var htmlTabla = '';

    if (!getTieneHojaView(numHoja)) {
        htmlTabla = dibujarTablaErrorEstandar(numHoja);
    } else {
        htmlTabla = dibujarTablaErrorHojaFormato(numHoja);
    }

    return htmlTabla;
}

function dibujarTablaErrorEstandar(numHoja) {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Celda</th><th>Valor</th><th>Tipo Error</th></tr></thead>";
    cadena += "<tbody>";

    generarListaErroresFromData(numHoja);
    var listErrores = getListaErrores(numHoja);

    var errores = getErrores(numHoja);
    var len = listErrores.length;
    for (var i = 0; i < len; i++) {
        cadena += "<tr><td>" + listErrores[i].Celda + "</td>";
        cadena += "<td style='background: " + errores[listErrores[i].Tipo].BackgroundColor + "'>" + listErrores[i].Valor + "</td>";
        cadena += "<td>" + errores[listErrores[i].Tipo].Descripcion + (listErrores[i].Mensaje != null ? ". " + listErrores[i].Mensaje : '') + "</td></tr>";
    }
    cadena += "</tbody></table>";

    return cadena;
}

function dibujarTablaErrorHojaFormato(numHojaPrincipal) {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaError' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead> <tr><th>Hoja</th> <th>Celda</th> <th>Valor</th> <th>Tipo Error</th> </tr></thead>";
    cadena += "<tbody>";

    var lista = getListaHoja(numHojaPrincipal);
    for (var nh = 0; nh < lista.length; nh++) {
        var numHoja = lista[nh].name;
        var nombreHoja = getNombreHoja(numHoja);

        generarListaErroresFromData(numHoja);
        var listErrores = getListaErrores(numHoja);

        var errores = getErrores(numHoja);
        var len = listErrores.length;
        for (var i = 0; i < len; i++) {
            cadena += "<tr>";
            cadena += "<td>" + nombreHoja + "</td>";
            cadena += "<td>" + listErrores[i].Celda + "</td>";
            cadena += "<td>" + listErrores[i].Valor + "</td>";
            cadena += "<td>" + errores[listErrores[i].Tipo].Descripcion + "</td>";
            cadena += "</tr>";
        }
    }

    cadena += "</tbody></table>";

    return cadena;
}

function eliminarError(celda, tipoError, numHoja) {
    var index = indexOfError(celda, tipoError, numHoja);

    if (index != -1) {
        getListaErrores(numHoja).splice(index, 1);

        //considerar si serán para todas las hojas o solo para la hoja actual
        switch (tipoError) {
            case ERROR_NO_NUMERO:
                getErrores(numHoja)[ERROR_NO_NUMERO].total--;
                break;
            case ERROR_LIM_INFERIOR:
                getErrores(numHoja)[ERROR_LIM_INFERIOR].total--;
                break;
            case ERROR_LIM_SUPERIOR:
                getErrores(numHoja)[ERROR_LIM_SUPERIOR].total--;
                break;
            case ERROR_RANGO_FECHA:
                getErrores(numHoja)[ERROR_RANGO_FECHA].total--;
                break;
            case ERROR_CRUCE_PERIODO:
                getErrores(numHoja)[ERROR_CRUCE_PERIODO].total--;
                break;
            case ERROR_TIME:
                getErrores(numHoja)[ERROR_TIME].total--;
                break;
        }
    }
}

function obtenerError(celda, valor, tipo, numHoja, mensajeAdicional) {
    var regError = null;
    //if (validarError(celda, tipo, numHoja)) {
    regError = {
        Celda: celda,
        Valor: valor,
        Tipo: tipo,
        Mensaje: mensajeAdicional
    };
    //}
    return regError;
}

function indexOfError(celda, tipo, numHoja) {
    var listErrores = getListaErrores(numHoja);
    var index = -1;
    for (var i = 0; i < listErrores.length; i++) {
        if (listErrores[i].Celda == celda && listErrores[i].Tipo == tipo) {
            index = i;
            break;
        }
    }

    return index;
}

function validarError(celda, tipo, numHoja) {
    var listErrores = getListaErrores(numHoja);
    for (var j in listErrores) {
        if (listErrores[j].Celda == celda && listErrores[j].Tipo == tipo) {
            return false;
        }
    }
    return true;
}

function generarListaErroresFromData(numHojaPrincipal) {
    if (!getTieneHojaView(numHojaPrincipal)) {

        generarListaErroresFromDataEstandar(numHojaPrincipal);
    } else {

        var lista = getListaHoja(numHojaPrincipal);
        for (var nh = 0; nh < lista.length; nh++) {
            var numHoja = lista[nh].name;
            generarListaErroresFromDataEstandar(numHoja);
        }
    }
}

function generarListaErroresFromDataEstandar(numHoja) {
    var evt = getVariableEvt(numHoja);
    var readOnly;

    var numCol = evt.Handson.ListaExcelData[0].length;
    var numFil = evt.Handson.ListaExcelData.length;
    var numFilCabecera = evt.FilasCabecera;
    var listErrores = [];
    var totalError = 0;
    var errores = getErrores(numHoja);
    var listaPtos = evt.ListaHojaPto;
    var matrizTipoEstado = evt.Handson.MatrizTipoEstado;
    var paramSolar = evt.ParamSolar;
    var fin = false;

    for (var col = 0; col < numCol && !fin; col++) {
        for (var row = 0; row < numFil && !fin; row++) {
            readOnly = true;

            if (row >= numFilCabecera && row <= numFil && col >= 1 && col <= numCol) {
                if (evt.Handson.ReadOnly) {
                    readOnly = true;
                }
                else {
                    if (matrizTipoEstado[row][col] == 1 || matrizTipoEstado[row][col] == 0) { // Tiene Hora de Operacion
                        readOnly = false;
                    }
                    else {
                        readOnly = true;
                    }
                }
            }

            if (!readOnly) {
                var value = getVariableHot(numHoja).view.instance.getDataAtCell(row, col);
                var regError = obtenerErrorGlobal(value, row, col, listaPtos, errores, numFilCabecera, numHoja, paramSolar);
                if (regError != null) {
                    totalError++;
                    listErrores.push(regError);
                }
            }

            if ((numCol <= col && numFil <= row) || totalError == 100) {
                fin = true;
            }
        }
    }

    setListaErrores(listErrores, numHoja);
}

function existeListaErrores(numHojaPrincipal) {
    if (!getTieneHojaView(numHojaPrincipal)) {

        return getListaErrores(numHojaPrincipal).length > 0;
    } else {

        var lista = getListaHoja(numHojaPrincipal);
        for (var nh = 0; nh < lista.length; nh++) {
            var numHoja = lista[nh].name;
            var numErrores = getListaErrores(numHoja).length;

            if (numErrores > 0) {
                return true;
            }
        }
    }

    return false;
}

function obtenerErrorGlobal(value, row, col, listaPtos, errores, numFilCabecera, numHoja, paramSolar) {
    var errorReg = null;
    var limiteInf = listaPtos[col - 1].Hojaptoliminf;
    var limiteSup = listaPtos[col - 1].Hojaptolimsup;
    var tipoCentral = listaPtos[col - 1].Famcodi;

    //validacion RDO
    var esPtoTieneCheck = listaPtos[col - 1].TieneCheckExtranet && listaPtos[col - 1].Hptoindcheck == null;
    var esSolar = esPtoTieneCheck && (tipoCentral == 36 || tipoCentral == 37);
    var esEolico = esPtoTieneCheck && (tipoCentral == 38 || tipoCentral == 39);
    var hCelda = row - numFilCabecera + 1;

    if (value != null) {
        celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();

        if (Number(value)) {
            var valorNumerico = Number(value);

            if (errores[ERROR_LIM_INFERIOR].validar) {
                if (valorNumerico < limiteInf) {
                    errorReg = obtenerError(celda, value, ERROR_LIM_INFERIOR, numHoja, 'El valor mínimo es ' + limiteInf);
                }
            }
            if (errores[ERROR_LIM_SUPERIOR].validar) {
                if (valorNumerico > limiteSup) {
                    errorReg = obtenerError(celda, value, ERROR_LIM_SUPERIOR, numHoja, 'El valor máximo es ' + limiteSup);
                }
            }

            //número menor que 0
            if (errores[ERROR_NUMERO_NEGATIVO].validar) {
                if (valorNumerico < 0) {
                    errorReg = obtenerError(celda, value, ERROR_NUMERO_NEGATIVO, numHoja, '');
                }
            }

            //if (errores[ERROR_DATA_CENTRAL_SOLAR].validar) {
            //    if (tipoCentral == 37) { //tipo central
            //        var k = (row - numFilCabecera + 1) % 96;
            //        if (((k >= 1 && k <= 16) || (k >= 78 && k <= 96))) {
            //            errorReg = obtenerError(celda, value, ERROR_DATA_CENTRAL_SOLAR, numHoja, '');
            //        }
            //    }
            //}

            if (errores[ERROR_DATA_INTERCONEXION].validar) {
                if (!esValidoInterconexion(valorNumerico, row, col, listaPtos, numHoja)) {
                    errorReg = obtenerError(celda, value, ERROR_DATA_INTERCONEXION, numHoja, '');
                }
            }
        }
        else {
            if (value == "0") {
                if (errores[ERROR_DATA_DESPACHO].validar) {
                    errorReg = obtenerError(celda, value, ERROR_DATA_DESPACHO, numHoja, '');
                }
            }
            else {
                if (value != "") {
                    if (!Number(value)) {
                        errorReg = obtenerError(celda, value, ERROR_NO_NUMERO, numHoja, '');
                    }
                } else if (esEolico && value == "") {
                    errorReg = obtenerError(celda, value, ERROR_BLANCO, numHoja, '');
                }
                else if (esSolar && value == "" && (paramSolar.HInicio <= hCelda && hCelda <= paramSolar.HFin)) {
                    errorReg = obtenerError(celda, value, ERROR_BLANCO, numHoja, '');
                }
                else {
                    if (errores[ERROR_BLANCO].validar) {
                        errorReg = obtenerError(celda, value, ERROR_BLANCO, numHoja, '');
                    }
                    if (errores[ERROR_DATA_DESPACHO].validar) {
                        errorReg = obtenerError(celda, value, ERROR_DATA_DESPACHO, numHoja, '');
                    }
                }
            }
        }
    }

    if (errores[ERROR_UNIDAD].validar && errorReg == null) {
        //if (!esValidoDespachoUnidadGeneracion(value, row, col, listaPtos, numHoja)) {
        //    errorReg = obtenerError(celda, value, ERROR_UNIDAD, numHoja);
        //}
    }

    return errorReg;
}

// Interconexiones Internacionales
function esValidoInterconexion(valorNumerico, row, col, listaPtos, numHoja) {
    var tipoinfocodi = listaPtos[col - 1].Tipoinfocodi;

    if (getVariableHot(numHoja) != undefined && getVariableHot(numHoja).view != null && valorNumerico != null && (tipoinfocodi == TIPOINFOCODI_A)) {
        var colexportMW = _getPosPtoFromTipoPtomedicodi(listaPtos, TIPO_PTOINFOCODI_EXPORTACION_MWh, numHoja) + 1;
        var colimportMW = _getPosPtoFromTipoPtomedicodi(listaPtos, TIPO_PTOINFOCODI_IMPORTACION_MWh, numHoja) + 1;
        var valexportMW = getVariableHot(numHoja).view.instance.getDataAtCell(row, colexportMW);
        var valimportMW = getVariableHot(numHoja).view.instance.getDataAtCell(row, colimportMW);

        if ((valexportMW == null || valexportMW == 0) && (valimportMW == null || valimportMW == 0)) {
            return false;
        }
    }

    return true;
}

function _getPosPtoFromTipoPtomedicodi(listaPtos, tptomedicodi, numHoja) {
    for (var i = 0; i < listaPtos.length; i++) {
        if (listaPtos[i].Tptomedicodi == tptomedicodi) {
            return i;
        }
    }
}

// Despacho
function esValidoDespachoUnidadGeneracion(ht, row, col) {
    if (ht.instance !== undefined) {
        data = ht.instance.getData();
        dataInvalidaByUnidad = [];
        dataPosGrupo = DATA_POSICION_PTO_MEDICION;

        //obtener todos los errores
        for (i = 0; i < dataPosGrupo.length; i++) {
            var listaPos = dataPosGrupo[i].Pto.split(",");
            for (k = 0; k < 48; k++) {//recorrer las siguientes 48 filas del grupo

                if (filasCab + k == row) {//buscamos la fila actual
                    var valores = [];
                    for (var j = 0; j < listaPos.length; j++) {//recorrer por unidad

                        var valor = data[filasCab + k][parseInt(listaPos[j]) + 1];
                        valores.push({ 'valor': valor, 'fila': filasCab + k, 'col': parseInt(listaPos[j]) + 1 });
                    }

                    //si todos los valores son vacios, hacer nada
                    var noHayNumero = true;
                    for (var m = 0; m < valores.length; m++) {
                        noHayNumero = noHayNumero && !Number(valores[m].valor);
                    }

                    //si hay un numero, entonces agregar coordenados que faltan data
                    if (!noHayNumero) {
                        for (var m = 0; m < valores.length; m++) {
                            if (!Number(valores[m].valor)) {
                                dataInvalidaByUnidad.push(
                                    [parseInt(valores[m].fila), parseInt(valores[m].col)]
                                );
                            }
                        }
                    }
                }
            }
        }

        //pintar los errores
        for (var i = 0; i < dataInvalidaByUnidad.length; i++) {
            row = dataInvalidaByUnidad[i][0];
            col = dataInvalidaByUnidad[i][1];

            value = ht.instance.getDataAtCell(row, col);
            td = ht.instance.getCell(row, col);

            if (value != null && td !== undefined) {
                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();

                eliminarError(celda, errorUnidad);
                td.style.background = errores[errorUnidad].BackgroundColor;
                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                agregarError(celda, value, errorUnidad);
            }
        }
    }
}

//////////////////////////////////////////////////////////
//// btnMostrarJustificaciones
//////////////////////////////////////////////////////////
function popUpListaJustificaciones(numHoja) {
    $('#idJustificaciones').html(dibujarTablaJustificaciones(getVariableEvt(numHoja).ListaCongeladoByEnvio, numHoja));
    setTimeout(function () {
        $('#justificaciones').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaJustificaciones').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);
}

function dibujarTablaJustificaciones(listaData, numHoja) {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaJustificaciones' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><th>Periodo</th><th>Empresa</th><th>Subestación</th><th>Equipo</th><th>Justificación</th><th style='width:260px'>Descripción de Justificación</th></tr></thead>";
    cadena += "<tbody>";
    var len = listaData.length;
    for (var i = 0; i < len; i++) {
        cadena += "<tr>";
        cadena += "<td>" + listaData[i].Periodo + "</td>";
        cadena += "<td>" + listaData[i].Empresa + "</td>";
        cadena += "<td>" + listaData[i].Subestacion + "</td>";
        cadena += "<td>" + listaData[i].Equipo + "</td>";
        cadena += "<td>" + listaData[i].SubcausacodiDesc + "</td>";
        if (listaData[i].Justificacion == -1) {
            cadena += '<td><input style="width:250px;" disabled="disabled" type="text" id="texto' + i + '" value="' + listaData[i].Texto + '"/></td>';
        } else {
            cadena += "</select></td>";
            cadena += '<td></td>';
        }
    }
    cadena += "</tbody></table>";

    return cadena;

}

function mostrarDetalleDataCongelada(numHoja) {
    $('#congelados').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false
    });

    setTimeout(function () {
        $('#idCongelados').html(dibujarTablaDataCongelada(numHoja));

        $('#tablaCongelados').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 200);
}

function cerrarDetalleDataCongelada(numHoja) {
    $('#congelados').bPopup().close();
}

function dibujarTablaDataCongelada(numHoja) {
    var htmlTabla = '';

    if (!getTieneHojaView(numHoja)) {
        htmlTabla = dibujarTablaDataCongeladaEstandar(numHoja);
    } else {
        htmlTabla = dibujarTablaDataCongeladaHojaFormato(numHoja);
    }

    return htmlTabla;
}

function dibujarTablaDataCongeladaEstandar(numHoja) {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaCongelados' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Rango</th><th>Periodo</th><th>Empresa</th><th>Subestación</th><th>Equipo</th><th>Dato Congelado</th><th>Unidad</th><th>Justificación</th><th style='width:260px'>Descripción de Justificación</th><th>Estado</th></tr></thead>";
    cadena += "<tbody>";

    generarListaDataCongelada(numHoja);
    var listaCongelados = getListaDataCongelada(numHoja);

    var justificaciones = getCausaJustificacion(numHoja);
    var len = listaCongelados.length;
    for (var i = 0; i < len; i++) {
        cadena += "<tr>";
        cadena += "<td>" + listaCongelados[i].Rango + "</td>";
        cadena += "<td>" + listaCongelados[i].Periodo + "</td>";
        cadena += "<td>" + listaCongelados[i].Empresa + "</td>";
        cadena += "<td>" + listaCongelados[i].Subestacion + "</td>";
        cadena += "<td>" + listaCongelados[i].Equipo + "</td>";
        cadena += "<td>" + number_format(listaCongelados[i].Valor, 3) + "</td>";
        cadena += "<td>" + listaCongelados[i].Unidad + "</td>";
        cadena += "<td><select id='justCmb" + i + "' style='width:200px;' onChange=\"justificarCongelada('" + numHoja + "'," + i + ",this.options[this.selectedIndex].value)\">";
        cadena += "<OPTION VALUE='-1'>  - Seleccionar - </OPTION>";
        for (var z = 0; z < justificaciones.length; z++) {
            cadena += "<OPTION VALUE='" + justificaciones[z].Subcausacodi + "'>" + justificaciones[z].Subcausadesc + "</OPTION>";
        }
        cadena += "<OPTION VALUE='0'>Otro</OPTION>";
        cadena += "</select></td>";
        cadena += "<td><input id='texto" + i + " type='text' style='width:250px;display: none' disabled='disabled' onchange=\"justificarCongelada2('" + numHoja + "'," + i + ")\"/></td>";
        cadena += "<td id='seljust" + i + "'>" + imagenError + "</td>";
        cadena += "</tr>";
    }
    cadena += "</tbody></table>";
    cadena += "<div><input type='button' id='btnEnviar' value='Envíar' onclick=\"enviarCongelados('" + numHoja + "')\" />";
    cadena += "<input type='button' id='btnCancelar' onclick='cerrarDetalleDataCongelada();'  value='Cancelar' /></div>";

    return cadena;
}
//ASSETEC 201909 - se agrego la columna hoja
function dibujarTablaDataCongeladaHojaFormato(numHojaPrincipal) {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaCongelados' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Rango</th> <th>Periodo</th> <th>Empresa</th> <th>Subestación</th> <th>Equipo</th> <th>Dato Congelado</th><th>Unidad</th><th>Justificación</th><th style='width:260px'>Descripción de Justificación</th><th>Estado</th></tr></thead>";
    cadena += "<tbody>";

    var lista = getListaHoja(numHojaPrincipal);
    for (var nh = 0; nh < lista.length; nh++) {
        var numHoja = lista[nh].name;
        var nombreHoja = getNombreHoja(numHoja);

        generarListaDataCongelada(numHoja);
        var listaCongelados = getListaDataCongelada(numHoja);

        var justificaciones = getCausaJustificacion(numHoja);
        var len = listaCongelados.length;
        for (var i = 0; i < len; i++) {
            cadena += "<tr>";
            cadena += "<td>" + nombreHoja + "</td>"; //ASSETEC
            cadena += "<td>" + listaCongelados[i].Rango + "</td>";
            cadena += "<td>" + listaCongelados[i].Periodo + "</td>";
            cadena += "<td>" + listaCongelados[i].Empresa + "</td>";
            cadena += "<td>" + listaCongelados[i].Subestacion + "</td>";
            cadena += "<td>" + listaCongelados[i].Equipo + "</td>";
            cadena += "<td>" + number_format(listaCongelados[i].Valor, 3) + "</td>";
            cadena += "<td>" + listaCongelados[i].Unidad + "</td>";
            cadena += "<td><select id='justCmb" + i + "' style='width:200px;' onChange=\"justificarCongelada('" + numHoja + "'," + i + ",this.options[this.selectedIndex].value)\">";
            cadena += "<OPTION VALUE='-2'>  - Seleccionar - </OPTION>";
            for (var z = 0; z < justificaciones.length; z++) {
                cadena += "<OPTION VALUE='" + justificaciones[z].Subcausacodi + "'>" + justificaciones[z].Subcausadesc + "</OPTION>";
            }
            cadena += "</select></td>";
            cadena += "<td><input id='texto" + i + "' type='text' style='width:250px;display: none' disabled='disabled' onchange=\"justificarCongelada2('" + numHoja + "'," + i + ")\"/></td>";
            cadena += "<td id='seljust" + i + "'>" + imagenError + "</td>";
            cadena += "</tr>";
        }
    }
    cadena += "</tbody></table>";
    cadena += "<div><input type='button' id='btnEnviar' value='Envíar' onclick=\"enviarCongelados('" + numHojaPrincipal + "')\" />";
    cadena += "<input type='button' id='btnCancelar' onclick='cerrarDetalleDataCongelada();'  value='Cancelar' /></div>";

    return cadena;
}

function justificarCongelada(numHoja, fila, tipoJustif) {
    textoJust = '';
    $("#texto" + fila).hide();
    $("#texto" + fila).attr('disabled', 'disabled');
    if (tipoJustif == -2) {
        $("#seljust" + fila).html(imagenError);
    }
    else {
        if (tipoJustif == -1) {
            $("#texto" + fila).show();
            $("#texto" + fila).removeAttr('disabled');
            textoJust = $("#texto" + fila).val();
            textoJust = textoJust === undefined ? '' : textoJust.trim();
            if (textoJust == '') {
                $("#seljust" + fila).html(imagenError);
            } else {
                $("#seljust" + fila).html(imagenOk);
            }
        } else {
            $("#seljust" + fila).html(imagenOk);
        }
    }
    getListaDataCongelada(numHoja)[fila].Justificacion = tipoJustif;
    getListaDataCongelada(numHoja)[fila].Texto = textoJust;
}

function justificarCongelada2(numHoja, fila) {
    textoJust = '';
    tipoJustif = $("#justCmb" + fila).val();
    $("#texto" + fila).hide();
    $("#texto" + fila).attr('disabled', 'disabled');

    if (tipoJustif == -1) {
        $("#texto" + fila).show();
        $("#texto" + fila).removeAttr('disabled');
        textoJust = $("#texto" + fila).val();
        textoJust = textoJust === undefined ? '' : textoJust.trim();
        if (textoJust == '') {
            $("#seljust" + fila).html(imagenError);
        } else {
            $("#seljust" + fila).html(imagenOk);
        }
    } else {
        $("#seljust" + fila).html(imagenOk);
    }
    getListaDataCongelada(numHoja)[fila].Justificacion = tipoJustif;
    getListaDataCongelada(numHoja)[fila].Texto = textoJust;
}

function existeCongeladoSinJustificar(numHoja) {
    var listaCongelados = getListaDataJustificacion(numHoja);
    var totalCongelados = listaCongelados.length;
    var justificados = 0;
    for (var i = 0; i < totalCongelados; i++) {
        if (listaCongelados[i].Justificacion >= -1) {
            justificados++;
        }
    }
    return justificados != totalCongelados
}

function generarListaDataCongelada(numHojaPrincipal) {
    if (!getTieneHojaView(numHojaPrincipal)) {

        generarListaDataCongeladaEstandar(numHojaPrincipal);
    } else {

        var lista = getListaHoja(numHojaPrincipal);
        for (var nh = 0; nh < lista.length; nh++) {
            var numHoja = lista[nh].name;
            generarListaDataCongeladaEstandar(numHoja);
        }
    }
}

function generarListaDataCongeladaEstandar(numHoja) {
    var evt = getVariableEvt(numHoja);
    var listaPtos = evt.ListaHojaPto;
    var data = getData(numHoja);

    var columnas = listaPtos.length;
    var filas = evt.Handson.ListaExcelData.length;
    var filasCab = evt.FilasCabecera;
    var valorAnterior = -100;
    var datosCongelados = 0;
    var inicioCongelado = 0;
    var finCongeladoFila = 0;
    var finCongeladoCol = 0;
    var listaCongelados = [];

    for (var i = 1; i <= columnas; i++) {
        for (var j = filasCab; j < filas; j++) {
            valor = data[j][i];
            if (valor == valorAnterior && valor != "") {
                datosCongelados++;
                if (datosCongelados == 1) {
                    inicioCongelado = j;
                    var congelado = {
                        Ptomedicodi: listaPtos[i - 1].Ptomedicodi,
                        Tipoinfocodi: listaPtos[i - 1].Tipoinfocodi,
                        Rango: getExcelColumnName(i + 1) + j + ":",
                        Periodo: data[j][0] + " hasta ",
                        Empresa: data[1][i],
                        Subestacion: data[3][i],
                        Equipo: data[4][i],
                        Unidad: data[6][i],
                        Justificacion: -1,
                        Texto: "",
                        Valor: valor,
                        FechaInicio: getFechaFila(data[j][0], filas, j)
                    };
                }
            }
            else {
                if (datosCongelados > 3) {
                    finCongeladoCol = i;
                    finCongeladoFila = j - 1;
                    valorAnterior = -100;
                    congelado.Rango += getExcelColumnName(finCongeladoCol + 1) + finCongeladoFila;
                    congelado.Periodo += data[finCongeladoFila][0];
                    congelado.FechaFin = getFechaFila(data[finCongeladoFila][0], filas, finCongeladoFila);
                    listaCongelados.push(congelado);
                }
                datosCongelados = 0;
            }
            valorAnterior = valor;
        }

        //si se termina el pto de medicion entonces guardar los congelados que hubiesen
        if (datosCongelados > 3) {
            finCongeladoCol = i;
            finCongeladoFila = j - 1;
            valorAnterior = -100;
            congelado.Rango += getExcelColumnName(finCongeladoCol + 1) + finCongeladoFila;
            congelado.Periodo += data[finCongeladoFila][0];
            congelado.FechaFin = getFechaFila(data[finCongeladoFila][0], filas, finCongeladoFila);
            listaCongelados.push(congelado);
        }
        datosCongelados = 0;
    }

    setListaDataCongelada(listaCongelados, numHoja);
}

function existeListaDataCongelada(numHojaPrincipal) {
    if (!getTieneHojaView(numHojaPrincipal)) {

        return getListaDataCongelada(numHojaPrincipal).length > 0;
    } else {

        var lista = getListaHoja(numHojaPrincipal);
        for (var nh = 0; nh < lista.length; nh++) {
            var numHoja = lista[nh].name;
            var numErrores = getListaDataCongelada(numHoja).length;

            if (numErrores > 0) {
                return true;
            }
        }
    }

    return false;
}

//////////////////////////////////////////////////////////
//// btnVerEnvios
//// popup para todas las hojas
//////////////////////////////////////////////////////////
function popUpListaEnvios(numHoja) {
    $('#idEnviosAnteriores').html(dibujarTablaEnvios(getVariableEvt(numHoja).ListaEnvios, numHoja));
    setTimeout(function () {
        $('#enviosanteriores').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablalenvio').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);
}

function dibujarTablaEnvios(lista, numHoja) {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablalenvio' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Id Envío</th><th>Fecha Hora</th><th>Usuario</th></tr></thead>";
    cadena += "<tbody>";

    for (key in lista) {
        var javaScriptDate = new Date(parseInt(lista[key].Enviofecha.substr(6)));
        cadena += "<tr onclick='mostrarEnvioExcelWeb(" + lista[key].Enviocodi + "," + numHoja + " );' style='cursor:pointer'><td>" + lista[key].Enviocodi + "</td>";
        cadena += "<td>" + getFormattedDate(javaScriptDate) + "</td>";
        cadena += "<td>" + lista[key].Lastuser + "</td></tr>";
    }
    cadena += "</tbody></table>";
    return cadena;

}

//////////////////////////////////////////////////////////
//// btnManttos
//////////////////////////////////////////////////////////
function popupManttos(numHoja) {
    $('#idMantenimiento').html(dibujarTablaManttos(numHoja));
    setTimeout(function () {
        $('#mantenimientos').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaManttos').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 50);
}

function dibujarTablaManttos(numHoja) {
    var htmlTabla = '';

    if (!getTieneHojaView(numHoja)) {
        htmlTabla = dibujarTablaManttosEstandar(numHoja);
    } else {
        htmlTabla = dibujarTablaManttosHojaFormato(numHoja);
    }

    return htmlTabla;
}

function dibujarTablaManttosEstandar(numHoja) {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaManttos' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Equipo Afectado</th><th>Equipo en Mantto</th><th>Tipo Equipo</th><th>Descripción</th><th>Fecha Inicio</th><th>Fecha Inicio</th><th>Estado</th></tr></thead>";
    cadena += "<tbody>";

    var listaResultado = getManttosFromHoja(numHoja);

    var len = listaResultado.length;
    for (var i = 0; i < len; i++) {
        var obj = listaResultado[i];
        cadena += dibujarManto(obj.Gruponomb, obj.Mantto);
    }
    cadena += "</tbody></table>";
    return cadena;
}

function dibujarTablaManttosHojaFormato(numHojaPrincipal) {//TO DO Falta implementar
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaManttos' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr> <th>Hoja</th> <th>Celda</th><th>Descripción</th></tr></thead>";
    cadena += "<tbody>";

    cadena += "</tbody></table>";

    return cadena;
}

function getManttosFromHoja(numHoja) {
    var listaResul = [];

    var listaGrupo = listarGrupos(numHoja);
    var totEquipos = listaGrupo.length;
    var manttos = getVariableEvt(numHoja).ListaMantenimiento;
    var listaBloqueMantos = getVariableEvt(numHoja).ListaBloqueMantos;

    for (var j = 0; j < totEquipos; j++) {
        var listamanto = findMantos(listaGrupo[j].Grupocodi, listaBloqueMantos);
        for (var k = 0; k < listamanto.length; k++) {
            indice = buscarManto(listamanto[k], manttos);
            if (indice != -1) {
                var obj = { Gruponomb: listaGrupo[j].Gruponomb, Mantto: manttos[indice] };
                listaResul.push(obj);
            }
        }
    }
    return listaResul;
}

function findMantos(equicodi, listaBloqueMantos) {
    var lista = [];
    for (var i = 0; i < listaBloqueMantos.length; i++) {
        if (listaBloqueMantos[i].Equicodi == equicodi) {
            for (var j = 0; j < listaBloqueMantos[i].ListaManto.length; j++) {
                indice = lista.indexOf(listaBloqueMantos[i].ListaManto[j]);
                if (indice == -1) {
                    lista.push(listaBloqueMantos[i].ListaManto[j]);
                }
            }
        }
    }
    return lista;
}

function buscarManto(manto, manttos) {
    for (var i = 0; i < manttos.length; i++) {
        if (manto == manttos[i].Manttocodi) {
            return i;
        }
    }
    return -1;
}

function dibujarManto(equipo, objMantto) {
    var tamano = (objMantto.Evendescrip.length > 20) ? 20 : objMantto.Evendescrip.length;
    var descripcion = objMantto.Evendescrip.substr(0, tamano);
    var eveini = parseJsonDate(objMantto.Evenini);
    var evefin = parseJsonDate(objMantto.Evenfin);

    var fechaIni = eveini.getFullYear().toString() + "-" + ("0" + (eveini.getMonth() + 1).toString()).slice(-2) + "-" + ("0" + eveini.getDate().toString()).slice(-2);
    fechaIni += " " + ("0" + eveini.getHours().toString()).slice(-2) + ":" + ("0" + eveini.getMinutes().toString()).slice(-2) + ":" + ("0" + eveini.getSeconds().toString()).slice(-2);

    var fechaFin = evefin.getFullYear().toString() + "-" + ("0" + (evefin.getMonth() + 1).toString()).slice(-2) + "-" + ("0" + evefin.getDate().toString()).slice(-2);
    fechaFin += " " + ("0" + evefin.getHours().toString()).slice(-2) + ":" + ("0" + evefin.getMinutes().toString()).slice(-2) + ":" + ("0" + evefin.getSeconds().toString()).slice(-2);

    var cadena = "<tr><td>" + equipo + "</td><td>" + (objMantto.Equinomb != null ? objMantto.Equinomb : "") + "</td><td>" + (objMantto.Famnomb != null ? objMantto.Famnomb : "") + "</td>" +
        "<td><p title='" + objMantto.Evendescrip + "'>" + descripcion + "</p></td>" + "<td>" + fechaIni + "</td><td>" + fechaFin + "</td><td style='text-align: center;'>" + objMantto.Evenindispo + "</td></tr>";
    return cadena;
}

//////////////////////////////////////////////////////////
//// btnEventos
//////////////////////////////////////////////////////////
function popupEventos(numHoja) {
    $('#idEvento').html(dibujarTablaEventos(numHoja));
    setTimeout(function () {
        $('#eventos').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaEventos').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 50);
}

function dibujarTablaEventos(numHoja) {
    var htmlTabla = '';

    if (!getTieneHojaView(numHoja)) {
        htmlTabla = dibujarTablaEventosEstandar(numHoja);
    } else {
        htmlTabla = dibujarTablaEventosHojaFormato(numHoja);
    }

    return htmlTabla;
}

function dibujarTablaEventosEstandar(numHoja) {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaEventos' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Equipo en Evento</th><th>Tipo Equipo</th><th>Descripción</th><th>Fecha Inicio</th><th>Fecha Fin</th></tr></thead>";
    cadena += "<tbody>";

    var listaResultado = getEventosFromHoja(numHoja);

    var len = listaResultado.length;
    for (var i = 0; i < len; i++) {
        var obj = listaResultado[i];
        cadena += "<tr>";
        cadena += dibujarEvento(obj);
        cadena += "</tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

function dibujarTablaEventosHojaFormato(numHojaPrincipal) {//TO DO Falta implementar
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaManttos' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr> <th>Hoja</th> <th>Celda</th><th>Descripción</th></tr></thead>";
    cadena += "<tbody>";

    cadena += "</tbody></table>";

    return cadena;
}

function dibujarEvento(objEvento) {
    var tamano = (objEvento.Evenasunto.length > 20) ? 20 : objEvento.Evenasunto.length;
    var descripcion = objEvento.Evenasunto.substr(0, tamano);
    var eveini = parseJsonDate(objEvento.Evenini);
    var fechaIni = eveini.getFullYear().toString() + "-" + ("0" + (eveini.getMonth() + 1).toString()).slice(-2) + "-" + ("0" + eveini.getDate().toString()).slice(-2);
    fechaIni += " " + ("0" + eveini.getHours().toString()).slice(-2) + ":" + ("0" + eveini.getMinutes().toString()).slice(-2) + ":" + ("0" + eveini.getSeconds().toString()).slice(-2);
    var evefin = parseJsonDate(objEvento.Evenfin);
    var fechaFin = evefin.getFullYear().toString() + "-" + ("0" + (evefin.getMonth() + 1).toString()).slice(-2) + "-" + ("0" + evefin.getDate().toString()).slice(-2);
    fechaFin += " " + ("0" + evefin.getHours().toString()).slice(-2) + ":" + ("0" + evefin.getMinutes().toString()).slice(-2) + ":" + ("0" + evefin.getSeconds().toString()).slice(-2);
    var cadena = "<td>" + objEvento.Equinomb + "</td><td>" + objEvento.Famnomb + "</td>" +
        "<td><p title='" + objEvento.Evenasunto + "'>" + descripcion + "</p></td>" + "<td>" + fechaIni + "</td><td>" + fechaFin + "</td>";
    return cadena;
}

function getEventosFromHoja(numHoja) {
    var eventos = getVariableEvt(numHoja).ListaEvento;
    return eventos;
}

//////////////////////////////////////////////////////////
//// btnVerLeyenda
//////////////////////////////////////////////////////////
function popupLeyenda(numHoja) {
    $('#idLeyenda').html(dibujarTablaLeyenda(numHoja));
    setTimeout(function () {
        $('#leyenda').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaLeyenda').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

    }, 50);
}

function dibujarTablaLeyenda(numHoja) {
    var htmlTabla = '';

    if (!getTieneHojaView(numHoja)) {
        htmlTabla = dibujarTablaLeyendaEstandar(numHoja);
    } else {
        htmlTabla = dibujarTablaLeyendaHojaFormato(numHoja);
    }

    return htmlTabla;
}

function dibujarTablaLeyendaEstandar(numHoja) {

    //var cadena = "<div style='clear:both; height:5px'></div> ";
    //cadena += "<table id='tablaLeyenda' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    //cadena += "<thead><tr><th>Celda</th><th>Descripción</th></tr></thead>";
    //cadena += "<tbody>";

    //var validacionApp = getLeyenda(numHoja);

    //var len = validacionApp.length;
    //for (var i = 0; i < len; i++) {
    //    cadena += "<tr>";
    //    cadena += '     <td style="background-color: ' + validacionApp[i].BackgroundColor + ' !important; color: white;"></td>';
    //    cadena += "<td style='text-align: left;'>" + (validacionApp[i].codigo > 0 ? 'Error: ' : ' ') + validacionApp[i].Descripcion + "</td></tr>";
    //}
    //cadena += "</tbody></table>";
    //return cadena;
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<div style='height:115px'>";
    cadena += "<table id='tablaLeyenda' border='1' class='pretty tabla-adicional' cellspacing='0' >";
    cadena += "<thead><tr><th>Celda</th><th>Descripción</th></tr></thead>";
    cadena += "<div>";

    var validacionApp = getLeyenda(numHoja);

    var len = validacionApp.length;
    for (var i = 0; i < len; i++) {
        cadena += "<tr>";
        cadena += '     <td style="background-color: ' + validacionApp[i].BackgroundColor + ' !important; color: white;"></td>';
        cadena += "<td style='text-align: left;'>" + (validacionApp[i].codigo > 0 ? 'Error: ' : ' ') + validacionApp[i].Descripcion + "</td></tr>";
    }
    cadena += "</div></table></div>";

    cadena += "<div style='clear:both; height:5px'></div> ";
    cadena += "<div style='height:95px'>";
    cadena += "<table id='tablaLeyendaCheck' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th style='width: 43px;'>Celda</th><th>Descripción</th></tr></thead>";
    cadena += "<tbody>";
    cadena += "<tr>";
    cadena += '<td style="background-color: #E0E3E6 !important; color: white;"><input type="checkbox" id="cbox1" checked="true" readonly="readonly" onclick="javascript: return false;"></td>';
    cadena += "<td style='text-align: left;'>Representa las horas Ejecutadas.</td></tr>";
    cadena += "<tr>";
    cadena += '<td style="background-color: #E0E3E6 !important; color: white;"><input type="checkbox" id="cbox1" readonly="readonly" onclick="javascript: return false;"></td>';
    cadena += "<td style='text-align: left;'>Representa las horas Proyectadas.</td></tr>";
    cadena += "</tbody></table></div>";
    return cadena;
}

function dibujarTablaLeyendaHojaFormato(numHojaPrincipal) {
    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaLeyenda' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr> <th>Hoja</th> <th>Celda</th><th>Descripción</th></tr></thead>";
    cadena += "<tbody>";

    var lista = getListaHoja(numHojaPrincipal);
    for (var nh = 0; nh < lista.length; nh++) {
        var numHoja = lista[nh].name;
        var nombreHoja = getNombreHoja(numHoja);

        var validacionApp = getLeyenda(numHoja);

        var len = validacionApp.length;
        for (var i = 0; i < len; i++) {
            cadena += "<tr>";
            cadena += "<td>" + nombreHoja + "</td>";
            cadena += '     <td style="background-color: ' + validacionApp[i].BackgroundColor + ' !important; color: white;"></td>';
            cadena += "<td style='text-align: left;'>" + (validacionApp[i].codigo > 0 ? 'Error: ' : ' ') + validacionApp[i].Descripcion + "</td></tr>";
        }
    }

    cadena += "</tbody></table>";

    return cadena;
}

function getLeyenda(numHoja) {
    var listaErroresApp = [];
    var evtHot = getVariableEvt(numHoja);
    var errores = getErrores(numHoja);
    for (var i = 0; i < errores.length; i++) {
        if (errores[i].validar) {
            listaErroresApp.push(errores[i]);
        }
    }

    if (evtHot != undefined && evtHot != null) {
        if ((evtHot.ValidaEventos != undefined && evtHot.ValidaEventos != null && evtHot.ValidaEventos)
            || (evtHot.ValidaRestricOperativa != undefined && evtHot.ValidaRestricOperativa != null && evtHot.ValidaRestricOperativa)) {
            var validacion = {
                Descripcion: 'VALIDACION DE EVENTOS: En este rango existe un evento que indispone la unidad de generación.',
                BackgroundColor: 'Salmon'
            };
            listaErroresApp.push(validacion);
        }

        if ((evtHot.ValidaMantenimiento != undefined && evtHot.ValidaMantenimiento != null && evtHot.ValidaMantenimiento)) {
            var validacion = {
                Descripcion: 'VALIDACION DE MANTENIMIENTOS: En este rango el equipo se encuentra en mantenimiento. ',
                BackgroundColor: 'MediumPurple'
            };
            listaErroresApp.push(validacion);
        }

        if (evtHot.ValidaHorasOperacion != undefined && evtHot.ValidaHorasOperacion != null && evtHot.ValidaHorasOperacion) {
            var validacion = {
                Descripcion: 'HORAS DE OPERACION: En este rango El equipo no tiene horas de operación registradas.',
                BackgroundColor: 'Silver'
            };
            listaErroresApp.push(validacion);
        }
    }

    return listaErroresApp;
}

//////////////////////////////////////////////////////////
//// btnDescargarManualUsuario
//////////////////////////////////////////////////////////

function descargarManualUsuario(numHoja) {
    window.location = controlador + 'MostrarManualUsuario' + "?app=" + getApp(numHoja);
};

//////////////////////////////////////////////////////////
//// btnGrafico
//////////////////////////////////////////////////////////

function generarFiltroGraficoFormatoIEOD(numHoja) {
    /// Lista de Centrales
    $(getIdElementoGrafico(numHoja, '.div_unidad2 select')).html("").fadeIn();
    var central = getVariableEvt(numHoja).ListaEquipo;
    $(getIdElementoGrafico(numHoja, '#cbCentral2')).empty();
    for (var i = 0; i < central.length; i++) {
        $(getIdElementoGrafico(numHoja, '#cbCentral2')).append('<option value=' + central[i].Equicodi + ' selected="selected">' + central[i].Equinomb + '</option>');
    }

    //Lista Punto
    var listaPunto = getListaPtos(numHoja);
    $(getIdElementoGrafico(numHoja, '#cbPunto')).empty();
    for (var i = 0; i < listaPunto.length; i++) {
        $(getIdElementoGrafico(numHoja, '#cbPunto')).append('<option value=' + listaPunto[i].Ptomedicodi + '>' + listaPunto[i].PtoMediEleNomb + '</option>');
    }

    //Lista Grupo
    var listaGrupo = listarGrupoXListaPto(getListaPtos(numHoja));
    $(getIdElementoGrafico(numHoja, '#cbGrupo')).empty();
    for (var i = 0; i < listaGrupo.length; i++) {
        $(getIdElementoGrafico(numHoja, '#cbGrupo')).append('<option value=' + listaGrupo[i].Grupocodi + '>' + listaGrupo[i].Gruponomb + '</option>');
    }

    // Lista Medida
    var listaMedida = getVariableEvt(numHoja).ListaTipoInformacion;
    filtroMedidaHtml = "";
    for (var i = 0; i < listaMedida.length; i++) {
        filtroMedidaHtml += "<option value='" + listaMedida[i].Tipoinfocodi + "'> " + listaMedida[i].Tipoinfoabrev + "</option>";
    }
    $(getIdElementoGrafico(numHoja, '.div_unidad2 select')).html("<select id='cbUnidad2'><option value='0' default> -TODOS- </option>" + filtroMedidaHtml + "</select>");
    $(getIdElementoGrafico(numHoja, '.div_ejeder select')).html("<select id='cbEjeder' multiple='multiple'>" + filtroMedidaHtml + "</select>");

    //eventos
    $(getIdElementoGrafico(numHoja, '#cbCentral2')).multipleSelect({
        width: '170px',
        filter: true,
        color: '#4C97C3',
        onClose: function (view) {
            filtrarGrupoXCentral(numHoja);
            graficoFormatoCentralUnidadMedidaEje(numHoja);
        }
    });
    $(getIdElementoGrafico(numHoja, '#cbCentral2')).multipleSelect('checkAll');

    $(getIdElementoGrafico(numHoja, '#cbPunto')).multipleSelect({
        width: '250px',
        filter: true,
        color: '#4C97C3',
        onClose: function (view) {
            graficoFormatoCentralUnidadMedidaEje(numHoja);
        }
    });
    $(getIdElementoGrafico(numHoja, '#cbPunto')).multipleSelect('checkAll');

    $(getIdElementoGrafico(numHoja, '#cbGrupo')).multipleSelect({
        width: '250px',
        filter: true,
        color: '#4C97C3',
        onClose: function (view) {
            graficoFormatoCentralUnidadMedidaEje(numHoja);
        }
    });
    $(getIdElementoGrafico(numHoja, '#cbGrupo')).multipleSelect('checkAll');

    $(getIdElementoGrafico(numHoja, '#cbEjeder')).multipleSelect({
        width: '100px',
        filter: true,
        color: '#4C97C3',
        onClose: function (view) {
        }
    });
    $(getIdElementoGrafico(numHoja, '#cbEjeder')).multipleSelect('checkAll');

    $(getIdElementoGrafico(numHoja, '#cbUnidad2')).change(function () {
        graficoFormatoCentralUnidadMedidaEje(numHoja);
    });

    $(getIdElementoGrafico(numHoja, '#chkAgrupar')).prop("checked", true);
    $(getIdElementoGrafico(numHoja, '#cbGrupo')).multipleSelect("disable");
    $(getIdElementoGrafico(numHoja, '#chkAgrupar')).click(function () {
        if ($(getIdElementoGrafico(numHoja, '#chkAgrupar')).prop('checked')) {
            $(getIdElementoGrafico(numHoja, '#cbGrupo')).multipleSelect("disable");
            $(getIdElementoGrafico(numHoja, '#cbGrupo')).multipleSelect("checkAll");
        }
        else {
            $(getIdElementoGrafico(numHoja, '#cbGrupo')).multipleSelect("enable");
        }
        graficoFormatoCentralUnidadMedidaEje(numHoja);
    });
}

function graficoFormatoCentralUnidadMedidaEje(numHoja) {

    var objSalida = getApp(numHoja) == APP_GENERACION_RER ? getDataToGraficoAppGeneracionRER(numHoja) : getDataToGraficoAppGeneral(numHoja);
    var arrayCategorias = objSalida.arrayCategorias;
    var series = objSalida.series;
    var serieName = objSalida.serieName;
    var titulo = objSalida.titulo;

    var opcion = {
        rangeSelector: {
            selected: 1
        },

        title: {
            text: titulo,
            style: {
                fontSize: '18'
            }
        },
        xAxis: {
            //tickInterval: 48, // one week
            //tickWidth: 1,
            title: {
                text: "Horas"
            },
            gridLineWidth: 1,
            categories: arrayCategorias
        },
        yAxis: [{
            title: {
                text: ""
            },
            min: 0
        },

        {
            title: {
                text: ""
            },
            opposite: false

        }],
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            borderWidth: 0,
            enabled: true
        },
        series: []
    };
    for (i = 0; i < series.length; i++) {
        opcion.series.push({
            name: serieName[i],
            data: series[i],
        });
    }
    $(getIdElementoGrafico(numHoja, '#panelGrafico')).highcharts(opcion);
    return serieName.length;
}

function getDataToGraficoAppGeneral(numHoja) {

    series = [];
    serieName = [];
    serieIdCentral = [];
    z = 0;
    agrupado = 0;

    //Filtros de la vista grafica
    medida = $(getIdElementoGrafico(numHoja, '#cbUnidad2')).val();
    central = $(getIdElementoGrafico(numHoja, '#cbCentral2')).multipleSelect('getSelects').toString();
    grupo = $(getIdElementoGrafico(numHoja, '#cbGrupo')).multipleSelect('getSelects').toString();
    if ($(getIdElementoGrafico(numHoja, '#chkAgrupar')).prop('checked')) {
        agrupado = 1;
    }
    listaCentral = central.split(",");
    listaGrupoFiltro = grupo.split(",");

    var filasCab = getVariableEvt(numHoja).FilasCabecera;
    var titulo = getVariableEvt(numHoja).TituloGrafico;
    var listaPtos = getListaPtos(numHoja);
    var data = getVariableHot(numHoja).getData();
    var numFilasData = getVariableEvt(numHoja).Handson.ListaExcelData.length - getVariableEvt(numHoja).FilasCabecera;

    posicion = 0;
    for (i = 0; i < listaPtos.length; i++) {
        posicion = listaCentral.indexOf(listaPtos[i].Equipadre.toString());
        posGrupo = listaGrupoFiltro.indexOf(listaPtos[i].Equicodi.toString());
        //console.log("Posicion:" + posGrupo);
        if ((posicion != -1) &&
            (listaPtos[i].Tipoinfocodi == medida || medida == 0) &&
            (posGrupo != -1)) {
            if (agrupado == 1) {
                var posCentral = serieIdCentral.indexOf(listaPtos[i].Equipadre * 10000 + listaPtos[i].Tipoinfocodi);
                if (posCentral == -1) {
                    series[z] = [];
                    serieName[z] = listaPtos[i].Equipopadre + "-" + listaPtos[i].Tipoinfoabrev;
                    serieIdCentral.push(listaPtos[i].Equipadre * 10000 + listaPtos[i].Tipoinfocodi);
                    for (k = 0; k < numFilasData; k++) {
                        var valor = data[filasCab + k][i + 1];
                        if (!Number(valor)) {
                            series[z].push(null);
                        }
                        else {
                            series[z].push(Number(valor));
                        }
                    }
                    z++;
                }
                else {
                    for (k = 0; k < numFilasData; k++) {
                        var valor = data[filasCab + k][i + 1];
                        if (Number(valor)) {
                            series[posCentral][k] += Number(valor);
                        }
                    }
                }
            }
            else {
                series[z] = [];
                serieName[z] = listaPtos[i].Equinomb + "-" + listaPtos[i].Tipoinfoabrev;
                for (k = 0; k < numFilasData; k++) {
                    var valor = data[filasCab + k][i + 1];
                    if (!Number(valor)) {
                        series[z].push(null);
                    }
                    else {
                        series[z].push(Number(valor));
                    }
                }
                z++;
            }
        }
    }

    //obtener categoria (medias horas)
    var arrayCategorias = [];
    for (k = 0; k < numFilasData; k++) {
        var valor = data[filasCab + k][0];
        arrayCategorias.push(valor);
    }

    var objSalida = {};
    objSalida.arrayCategorias = arrayCategorias;
    objSalida.series = series;
    objSalida.serieName = serieName;
    objSalida.titulo = titulo;

    return objSalida;
}

function getDataToGraficoAppGeneracionRER(numHoja) {

    series = [];
    serieName = [];
    serieIdCentral = [];
    z = 0;
    agrupado = 0;

    //Filtros de la vista grafica
    medida = $(getIdElementoGrafico(numHoja, '#cbUnidad2')).val();
    central = $(getIdElementoGrafico(numHoja, '#cbCentral2')).multipleSelect('getSelects').toString();
    punto = $(getIdElementoGrafico(numHoja, '#cbPunto')).multipleSelect('getSelects').toString();
    if ($(getIdElementoGrafico(numHoja, '#chkAgrupar')).prop('checked')) {
        agrupado = 1;
    }
    listaCentral = central.split(",");
    listaPuntoFiltro = punto.split(",");

    var filasCab = getVariableEvt(numHoja).FilasCabecera;
    var titulo = getVariableEvt(numHoja).TituloGrafico;
    var listaPtos = getListaPtos(numHoja);
    var listaTipopto = getVariableEvt(numHoja).ListaTipopuntotomedicion;
    var data = getVariableHot(numHoja).getData();
    var numFilasData = getVariableEvt(numHoja).Handson.ListaExcelData.length - getVariableEvt(numHoja).FilasCabecera;

    posicion = 0;

    if (listaTipopto != null) {
        for (var t = 0; t < listaTipopto.length; t++) {
            var tipoPto = listaTipopto[t].Tipoptomedicodi;

            for (i = 0; i < listaPtos.length; i++) {
                posicion = listaCentral.indexOf(listaPtos[i].Equipadre.toString());
                //posPto = i;
                posPto = listaPuntoFiltro.indexOf(listaPtos[i].Ptomedicodi.toString());

                //console.log("Posicion:" + posGrupo);
                if ((posicion != -1) &&
                    (listaPtos[i].Tipoinfocodi == medida || medida == 0) &&
                    (listaPtos[i].Tptomedicodi == tipoPto) &&
                    (posPto != -1)) {
                    if (agrupado == 1) {
                        var posCentral = serieIdCentral.indexOf(listaPtos[i].Equipadre * 100000 + listaPtos[i].Tipoinfocodi * 1000 + listaPtos[i].Tptomedicodi);
                        if (posCentral == -1) {
                            series[z] = [];
                            serieName[z] = listaPtos[i].Equipopadre + "-" + listaPtos[i].Tipoinfoabrev + (tipoPto != 15 ? "(" + listaTipopto[t].Tipoptomedinomb + ")" : '');

                            serieIdCentral.push(listaPtos[i].Equipadre * 100000 + listaPtos[i].Tipoinfocodi * 1000 + listaPtos[i].Tptomedicodi);
                            for (k = 0; k < numFilasData; k++) {
                                var valor = data[filasCab + k][i + 1];
                                if (!Number(valor)) {
                                    series[z].push(null);
                                }
                                else {
                                    series[z].push(Number(valor));
                                }
                            }
                            z++;
                        }
                        else {
                            for (k = 0; k < numFilasData; k++) {
                                var valor = data[filasCab + k][i + 1];
                                if (Number(valor)) {
                                    series[posCentral][k] += Number(valor);
                                }
                            }
                        }
                    }
                    else {
                        series[z] = [];
                        serieName[z] = listaPtos[i].PtoMediEleNomb;
                        for (k = 0; k < numFilasData; k++) {
                            var valor = data[filasCab + k][i + 1];
                            if (!Number(valor)) {
                                series[z].push(null);
                            }
                            else {
                                series[z].push(Number(valor));
                            }
                        }
                        z++;
                    }
                }
            }
        }
    }

    //obtener categoria (medias horas)
    var arrayCategorias = [];
    for (k = 0; k < numFilasData; k++) {
        var valor = data[filasCab + k][0];
        arrayCategorias.push(valor);
    }

    var objSalida = {};
    objSalida.arrayCategorias = arrayCategorias;
    objSalida.series = series;
    objSalida.serieName = serieName;
    objSalida.titulo = titulo;

    return objSalida;
}

function filtrarGrupoXCentral(numHoja) {
    var listaPtos = getListaPtos(numHoja);
    var listaGrupoAux = listarGrupos(numHoja);
    var total = listaGrupoAux.length;

    var central = $(getIdElementoGrafico(numHoja, '#cbCentral2')).multipleSelect('getSelects').toString();
    var listacentral = central.split(",");

    var filtroGrupoHtml = "<select id='cbGrupo' multiple= 'multiple'>";
    for (var j = 0; j < listacentral.length; j++) {
        for (var i = 0; i < total; i++) {
            grupo = {
                Centralcodi: listaPtos[i].Equipadre,
                Gruponomb: listaPtos[i].Equipopadre + "_" + listaPtos[i].Equinomb,
                Grupocodi: listaPtos[i].Equicodi
            }
            if (listaGrupoAux[i].Centralcodi == listacentral[j]) {
                filtroGrupoHtml += "<option value='" + listaGrupoAux[i].Grupocodi + "'> " + listaGrupoAux[i].Gruponomb + "</option>";
            }
        }
    }
    filtroGrupoHtml += "</select>";
    $(getIdElementoGrafico(numHoja, '.div_grupo select')).html(filtroGrupoHtml);

    $(getIdElementoGrafico(numHoja, '#cbGrupo')).multipleSelect('refresh');
    $(getIdElementoGrafico(numHoja, '#cbGrupo')).multipleSelect('checkAll');
}

function listarGrupoXListaPto(listaPtos) {
    var listaGrupo = [];
    for (var i = 0; i < listaPtos.length; i++) {
        find = buscarGrupo(listaGrupo, listaPtos[i].Equicodi);
        if (find == -1) {
            grupo = {
                Centralcodi: listaPtos[i].Equipadre,
                Gruponomb: (listaPtos[i].Equipopadre != null ? listaPtos[i].Equipopadre + "_" : "") + listaPtos[i].Equinomb,
                Grupocodi: listaPtos[i].Equicodi
            }
            listaGrupo.push(grupo);
        }
    }

    return listaGrupo;
}

function buscarGrupo(lista, grupo) {
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].Grupocodi == grupo) {
            return i;
        }
    }
    return -1;
}

function listarGrupos(numHoja) {
    var lista = [];
    var listaPtos = getListaPtos(numHoja);
    for (var i = 0; i < listaPtos.length; i++) {
        find = buscarGrupo(lista, listaPtos[i].Equicodi);
        if (find == -1) {
            grupo = {
                Centralcodi: listaPtos[i].Equipadre,
                Gruponomb: listaPtos[i].Equipopadre + "_" + listaPtos[i].Equinomb,
                Grupocodi: listaPtos[i].Equicodi
            }
            lista.push(grupo);
        }
    }

    return lista;
}

//////////////////////////////////////////////////////////
//// btnExpandirRestaurar
//////////////////////////////////////////////////////////
function expandirRestaurar(numHoja) {
    if ($(getIdElemento(numHoja, '#hfExpandirContraer')).val() == "E") {
        expandirExcel(numHoja);

        $(getIdElemento(numHoja, '#hfExpandirContraer')).val("C");
        $(getIdElemento(numHoja, '#spanExpandirContraer')).text('Restaurar');

        var img = $(getIdElemento(numHoja, '#imgExpandirContraer')).attr('src');
        var newImg = img.replace('expandir.png', 'contraer.png');
        $(getIdElemento(numHoja, '#imgExpandirContraer')).attr('src', newImg);

    }
    else {
        restaurarExcel(numHoja);

        $(getIdElemento(numHoja, '#hfExpandirContraer')).val("E");
        $(getIdElemento(numHoja, '#spanExpandirContraer')).text('Expandir');

        var img = $(getIdElemento(numHoja, '#imgExpandirContraer')).attr('src');
        var newImg = img.replace('contraer.png', 'expandir.png');
        $(getIdElemento(numHoja, '#imgExpandirContraer')).attr('src', newImg);
    }

    updateDimensionHandson(numHoja);
}

function expandirExcel(numHojaPrincipal) {
    $(getIdElemento(numHojaPrincipal, '#mainLayout')).addClass("divexcel");

    if (!getTieneHojaView(numHojaPrincipal)) {

        getVariableHot(numHojaPrincipal).render();
    } else {

        var lista = getListaHoja(numHojaPrincipal);
        for (var nh = 0; nh < lista.length; nh++) {
            var numHoja = lista[nh].name;
            getVariableHot(numHoja).render();
        }
    }
}

function restaurarExcel(numHoja) {
    $('#tophead').css("display", "none");
    $('#detExcel').css("display", "block");
    $(getIdElemento(numHoja, '#mainLayout')).removeClass("divexcel");
    $(getIdElemento(numHoja, '#itemExpandir')).css("display", "block");
    $(getIdElemento(numHoja, '#itemRestaurar')).css("display", "none");
}

//////////////////////////////////////////////////////////
//// btnHOP
//////////////////////////////////////////////////////////
function popupListaHOP(numHoja) {
    $('#idListaHOP').html(dibujarTablaListaHOP(getVariableEvt(numHoja).ListaHOP, numHoja));
    setTimeout(function () {
        $('#listaHOP').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablaListaHOP').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });

        $('#btnExportarHOP').unbind('click');
        $('#btnExportarHOP').click(function () {
            exportarReporteHOP(numHoja);
        });
    }, 50);
}

function dibujarTablaListaHOP(lista, numHoja) {

    var cadena = "<div style='clear:both; height:5px'></div> ";
    cadena += "<table id='tablaListaHOP' border='1' class='pretty tabla-adicional' cellspacing='0'>";
    cadena += "<thead><tr><th>Central</th><th>Unidad</th><th>Modo de Operación - Grupo</th><th>Fecha Inicio</th><th>Fecha Fin</th></tr></thead>";
    cadena += "<tbody>";

    for (key in lista) {
        cadena += "<tr>";
        cadena += "<td>" + lista[key].Central + "</td>";
        cadena += "<td>" + lista[key].Unidad + "</td>";
        cadena += "<td>" + lista[key].ModoOpGrupo + "</td>";
        cadena += "<td>" + lista[key].FechaIni + "</td>";
        cadena += "<td>" + lista[key].FechaFin + "</td>";
        cadena += "</tr>";
    }
    cadena += "</tbody></table>";
    return cadena;
}

//////////////////////////////////////////////////////////
//// btnExportarHOP
//////////////////////////////////////////////////////////
function exportarReporteHOP(numHoja) {
    var mes = $(getIdElemento(numHoja, '#txtMes')).val();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        traditional: true,
        url: controlador + 'ExportarExcelReporteHOP',
        data: JSON.stringify({
            idEmpresa: getIdEmpresa(numHoja),
            mes: mes
        }),
        beforeSend: function () {
            mostrarExito(numHoja, "Descargando información ...");
        },
        success: function (result) {
            if (result.length > 0 && result[0] != "-1") {
                mostrarExito(numHoja, "<strong>Los datos se descargaron correctamente</strong>");
                window.location.href = controlador + 'DescargarExcelReporteHOP?archivo=' + result[0] + '&nombre=' + result[1];
            }
            else {
                alert("Error en descargar el archivo");
            }
        },
        error: function (result) {
            alert('ha ocurrido un error al descargar el archivo excel. ' + result.status + ' - ' + result.statusText + '.');
        }
    });
}

//////////////////////////////////////////////////////////
//// Funciones para obtener los distintos campos
//////////////////////////////////////////////////////////
function getHoja(numHoja) {
    return LISTA_OBJETO_HOJA[numHoja];
}

function getIdHoja(numHoja) {
    return $(getIdElemento(numHoja, "#hfHoja")).val();
}
function getHojaNombre(numHoja) {
    return $(getIdElemento(numHoja, "#hfHojaNombre")).val();
}

function getApp(numHoja) {
    return parseInt($(getIdElemento(numHoja, "#hfApp")).val()) || 0;
}

function getSemana(numHoja) {
    $(getIdElemento(numHoja, '#hfSemana')).val($(getIdElemento(numHoja, '#cbSemana')).val());
    $(getIdElemento(numHoja, '#hfAnho')).val($(getIdElemento(numHoja, '#Anho')).val());
    semana = $(getIdElemento(numHoja, "#hfSemana")).val();
    anho = $(getIdElemento(numHoja, '#Anho')).val();

    if (semana !== undefined && anho !== undefined) {
        semana = anho.toString() + semana;
    } else {
        semana = '';
    }

    return semana;
}

function getFecha(numHoja) {
    var fecha = $(getIdElemento(numHoja, '#txtFecha')).val();
    $(getIdElemento(numHoja, '#hfFecha')).val(fecha);
    fecha = $(getIdElemento(numHoja, "#hfFecha")).val();
    fecha = fecha !== undefined ? fecha : '';
    return fecha;
}

function getMes(numHoja) {
    var mes = $(getIdElemento(numHoja, "#txtMes")).val();
    $(getIdElemento(numHoja, "#hfMes")).val(mes);
    mes = $(getIdElemento(numHoja, "#hfMes")).val();
    mes = mes !== undefined ? mes : '';
    return mes;
}

function getAnho(numHoja) {
    var mes = $(getIdElemento(numHoja, "#Anho")).val();
    $(getIdElemento(numHoja, "#hfAnho")).val(mes);
    mes = $(getIdElemento(numHoja, "#hfAnho")).val();
    mes = mes !== undefined ? mes : '';
    return mes;
}

function getIdFormato(numHoja) {
    formato = $(getIdElemento(numHoja, "#cbTipoFormato")).val();
    formato = formato !== undefined ? formato : 0;
    if (formato == 0) {
        formato2 = $(getIdElemento(numHoja, "#hfFormato")).val();
        formato = formato2 !== undefined ? formato2 : 0;
    }
    return formato;
}

function setIdEnvio(numHoja, idEnvio) {
    $(getIdElemento(numHoja, "#hfIdEnvio")).val(idEnvio);
}

function getIdEmpresa(numHoja) {
    return $(getIdElemento(numHoja, "#cbEmpresa")).val();
}
function setIdEmpresa(numHoja, empr) {
    $(getIdElemento(numHoja, "#hfEmpresa")).val(empr);
}

function getHorizonte(numHoja) {
    return $(getIdElemento(numHoja, "#cbHorizonte")).val();
}
function setHorizonte(numHoja, hor) {
    $(getIdElemento(numHoja, "#hfHorizonte")).val(hor);
}

function getNumeroHoja(numHoja) {
    var hoja = numHoja;
    hoja = hoja !== undefined ? hoja : '';
    return hoja + '';
}

function getTieneHojaView(numHoja) {
    return getListaHoja(numHoja) != undefined && getListaHoja(numHoja) != null && getListaHoja(numHoja).length > 0;
}

function getData(numHoja) {
    var data = null;
    if (!getTieneHojaView(numHoja)) {
        data = getVariableHot(numHoja).getData();
    }
    return data;
}

function getListaDataNumHoja(numHoja) {
    var listaHoja = null;

    if (getTieneHojaView(numHoja)) {
        listaHoja = [];
        var lista = getListaHoja(numHoja);
        for (var nh = 0; nh < lista.length; nh++) {
            listaHoja.push(lista[nh].name);
        }
    }

    return listaHoja;
}

function getListaData(numHoja) {
    var listaData = null;

    if (getTieneHojaView(numHoja)) {
        listaData = [];

        var lista = getListaHoja(numHoja);
        for (var nh = 0; nh < lista.length; nh++) {
            listaData.push(getVariableHot(lista[nh].name).getData());
        }
    }

    return listaData;
}

function getListaDataJustificacion(numHojaPrincipal) {
    var listaCongelados = [];
    if (!getTieneHojaView(numHojaPrincipal)) {
        return null;
    } else {
        var lista = getListaHoja(numHojaPrincipal);
        for (var nh = 0; nh < lista.length; nh++) {
            var numHoja = lista[nh].name;
            var nombreHoja = getNombreHoja(numHoja);

            listaCongelados = listaCongelados.concat(getListaDataCongelada(numHoja));
        }
    }

    return listaCongelados.length > 0 ? listaCongelados : null;
}

//evtHot
function setVariableEvt(evt, hoja) {
    var numHoja = getNumeroHoja(hoja);

    LISTA_OBJETO_HOJA[numHoja].evtHot = evt;

    LISTA_OBJETO_HOJA[numHoja].listaSize = evt.Handson.ListaColWidth;
    LISTA_OBJETO_HOJA[numHoja].filasCab = evt.FilasCabecera;
    LISTA_OBJETO_HOJA[numHoja].listaPtos = evt.ListaHojaPto;
    LISTA_OBJETO_HOJA[numHoja].manttos = evt.ListaMantenimiento;
    LISTA_OBJETO_HOJA[numHoja].eventos = evt.ListaEvento;
    LISTA_OBJETO_HOJA[numHoja].listaBloqueMantos = evt.ListaBloqueMantos;
    LISTA_OBJETO_HOJA[numHoja].causaJustificacion = evt.ListaCausaJustificacion;
    LISTA_OBJETO_HOJA[numHoja].HorarioCodi = evt.IdHorario;

}
function getVariableEvt(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].evtHot;
}

//hot
function setVariableHot(hotVar, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].hot = hotVar;
}
function getVariableHot(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].hot;
}

//colsToHide
function setColsToHide(colsToHide, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].colsToHide = colsToHide;
}
function getColsToHide(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].colsToHide;
}

//Validacion Data Congelada
function setValidacionDataCongelada(valor, hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].validaDataCongelada = valor;
}
function getValidacionDataCongelada(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].validaDataCongelada;
}

//listaDataCongelada
function setListaDataCongelada(lista, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].listDataCongelada = lista;
}
function getListaDataCongelada(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].listDataCongelada;
}

//causaJustificacion
function setCausaJustificacion(lista, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].causaJustificacion = lista;
}
function getCausaJustificacion(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].causaJustificacion;
}

//listaErrores
function setListaErrores(listErrores, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].listErrores = listErrores;
}
function getListaErrores(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].listErrores;
}

//errores
function setErrores(errores, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].errores = errores;
}
function getErrores(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].errores;
}
//tieneGrafico
function setTieneGrafico(valor, hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].tieneGrafico = valor;
}
function getTieneGrafico(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].tieneGrafico;
}

//tieneFiltro
function setTieneFiltro(valor, hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].tieneFiltro = valor;
}
function getTieneFiltro(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].tieneFiltro;
}

//tieneHOP
function setTieneHOP(valor, hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].tieneHOP = valor;
}
function getTieneHOP(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].tieneHOP;
}

//cargado
function setEstaCargado(valor, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].estaCargado = valor;
}
function getEstaCargado(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].estaCargado;
}

//Mostrar ultimo envio
function setVerUltimoEnvio(valor, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].verUltimoEnvio = valor;
}
function getVerUltimoEnvio(hoja) {
    var configHoja = getConfigHoja(hoja);

    var opcionActual = parseInt(LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].verUltimoEnvio) || NO_VER_ULTIMO_ENVIO;

    if (configHoja != undefined && configHoja != null && !configHoja.verUltimoEnvio) {
        opcionActual = NO_VER_ULTIMO_ENVIO;
    }
    return opcionActual;
}

//Mostrar Panel IEOD
function getEsActivoTab(hoja) {
    var idTab = LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].idTab;
    var idCont = LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].idTabContainer;
    var hojaPadre = LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].numHojaPadre;

    var idHojaViewActivo = $(getIdElemento(hojaPadre, idCont) + " .panel-container .active").attr("id"); //tab activo

    if (idHojaViewActivo == idTab) {
        return true;
    }
    return false;
}

//cantidad click importar (solo una vez debe inicializarse)
function setCantidadClickImportar(valor, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].cantidadClickImportar = valor;
}
function getCantidadClickImportar(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].cantidadClickImportar;
}

//listaSize
function getListaSize(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].listaSize;
}

//listaPtos
function getListaPtos(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].listaPtos;
}

//hoja padre
function setEsHojaPadre(valor, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].esHojaPadre = valor;
}
function getEsHojaPadre(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].esHojaPadre;
}

//listaHoja
function setListaHoja(listaHoja, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].listaHoja = listaHoja;
}
function getListaHoja(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].listaHoja;
}

//configHoja
function setConfigHoja(configHoja, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].configHoja = configHoja;
}
function getConfigHoja(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].configHoja;
}

//// Operaciones sobre Hoja
//nombreHoja
function setNombreHoja(valor, hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].nombreHoja = valor;
}
function getNombreHoja(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].nombreHoja;
}

function getHorarioCodi(hoja) {
    return LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].HorarioCodi;
}

function setHorarioCodi(valor, hoja) {
    LISTA_OBJETO_HOJA[getNumeroHoja(hoja)].HorarioCodi = valor;
}

function crearObjetoHoja(numHojaPadre, idTabView, idTabViewContainer) {
    var hot;
    var evtHot;
    var listaPtos = null;
    var listaFecha = null;
    var listaSize = [];
    var colsToHide = [];
    var listaGrupo = [];
    var listaGrupoAux = [];
    var listaBloqueMantos = [];
    var errores = JSON.parse(JSON.stringify(ERROR_GLOBAL));
    var listErrores = [];
    var validaDataCongelada = false;
    var listDataCongelada = [];
    var causaJustificacion = [];
    var manttos = [];
    var eventos = [];
    var filasCab = 0;
    var grillaBD;
    var tieneGrafico = false;
    var tieneHOP = false;
    var tieneFiltro = true;
    var estaCargado = false;
    var esHojaPadre = false;
    var cantidadClickImportar = 0;
    var verUltimoEnvio = VER_ULTIMO_ENVIO;

    var listaHoja = [];
    var nombreHoja = '';
    var HorarioCodi = '';
    var obj = {
        hot: hot,
        evtHot: evtHot,
        validaDataCongelada: validaDataCongelada,
        listDataCongelada: listDataCongelada,
        causaJustificacion: causaJustificacion,
        listaPtos: listaPtos,
        listaFecha: listaFecha,
        listaSize: listaSize,
        colsToHide: colsToHide,
        listaGrupo: listaGrupo,
        listaGrupoAux: listaGrupoAux,
        listaBloqueMantos: listaBloqueMantos,
        errores: errores,
        listErrores: listErrores,
        manttos: manttos,
        eventos: eventos,
        filasCab: filasCab,
        grillaBD: grillaBD,
        tieneGrafico: tieneGrafico,
        tieneHOP: tieneHOP,
        tieneFiltro: tieneFiltro,
        listaHoja: listaHoja,
        estaCargado: estaCargado,
        nombreHoja: nombreHoja,
        esHojaPadre: esHojaPadre,
        cantidadClickImportar: cantidadClickImportar,
        configHoja: crearConfigHoja(),
        verUltimoEnvio: verUltimoEnvio,
        idTab: idTabView,
        idTabContainer: idTabViewContainer,
        numHojaPadre: numHojaPadre,
        HorarioCodi: HorarioCodi
    };

    return obj;
}

function crearConfigHoja() {
    var config = {
        tieneFiltroFecha: false,
        valorFiltroFecha: 0,
        tieneFiltroCentral: false,
        tieneFiltroArea: false,
        tieneFiltroSubestacion: false,
        tieneFiltroFamilia: false,
        tieneFiltroFormato: false,
        tienePanelIEOD: false,
        verUltimoEnvio: true
    };

    return config;
}

function crearHojaView(name, id, hoja) {
    var obj = {
        name: name,
        id: id,
        hoja: hoja
    };

    return obj;
}

function getIdElemento(numHoja, idElemento) {
    var numHoja = getNumeroHoja(numHoja);
    var idForm = '';
    if (numHoja != '') {
        idForm = "#formHoja" + numHoja;

        if (getEsHojaPadre(numHoja) && (idElemento == "#hfHoja" || idElemento == "#mainLayout")) { return idForm + " " + idElemento + "Main"; }

        return idForm + " " + idElemento;
    }

    //var id = idElemento.startsWith("#hf") ? idElemento + "Main" : idElemento;
    var id = idElemento;

    return id;
}

function getIdElementoGrafico(numHoja, idElemento) {
    var numHoja = getNumeroHoja(numHoja);
    var idForm = '';
    if (numHoja != '') {
        idForm = "#idGraficoHoja" + numHoja;
        return idForm + " " + idElemento;
    }

    var id = idElemento;
    return idElemento;
}

function getIdGrafico(numHoja) {
    return getIdElementoGrafico(numHoja, '');
}

//configuracion


//////////////////////////////////////////////////////////
//// Funciones para mostrar mensajes
//////////////////////////////////////////////////////////
function mostrarMensajeInformativo(numHoja, mensaje) {
    $(getIdElemento(numHoja, '#mensaje')).removeClass();
    $(getIdElemento(numHoja, '#mensaje')).addClass("action-message");
    $(getIdElemento(numHoja, '#mensaje')).html(mensaje);
    $(getIdElemento(numHoja, '#mensaje')).css("display", "block");
}
function mostrarMensajeExito(numHoja, mensaje) {
    $(getIdElemento(numHoja, '#mensaje')).removeClass();
    $(getIdElemento(numHoja, '#mensaje')).addClass("action-exito");
    $(getIdElemento(numHoja, '#mensaje')).html(mensaje);
    $(getIdElemento(numHoja, '#mensaje')).css("display", "block");
}

function hideAllMensaje(numHoja) {
    hideMensaje(numHoja);
    hideMensajeEvento(numHoja);
}
function hideMensaje(numHoja) {
    $(getIdElemento(numHoja, '#mensaje')).css("display", "none");
}
function hideMensajeEvento(numHoja) {
    $(getIdElemento(numHoja, '#mensajeEvento')).css("display", "none");
}

function mostrarEventoError(numHoja, alerta) {
    $(getIdElemento(numHoja, '#mensajeEvento')).removeClass();
    $(getIdElemento(numHoja, '#mensajeEvento')).addClass("action-error");
    $(getIdElemento(numHoja, '#mensajeEvento')).html(alerta);
    $(getIdElemento(numHoja, '#mensajeEvento')).css("display", "block");
}

function mostrarEventoCorrecto(numHoja, mensaje) {
    $(getIdElemento(numHoja, '#mensajeEvento')).removeClass();
    $(getIdElemento(numHoja, '#mensajeEvento')).addClass("action-exito");
    $(getIdElemento(numHoja, '#mensajeEvento')).html(mensaje);
    $(getIdElemento(numHoja, '#mensajeEvento')).css("display", "block");
}

function mostrarEventoAlerta(numHoja, mensaje) {
    if (mensaje != '') {
        $(getIdElemento(numHoja, '#mensajeEvento')).removeClass();
        $(getIdElemento(numHoja, '#mensajeEvento')).addClass('action-alert');
        $(getIdElemento(numHoja, '#mensajeEvento')).html(mensaje);
        $(getIdElemento(numHoja, '#mensajeEvento')).css("display", "block");
    }
}

function obtenerMensajeEnvio(numHoja, idEnvio, accionActual, val_envio, horarioenvio, evt) {
    var envio = $(getIdElemento(numHoja, "#hfIdEnvio")).val();
    let myDate = new Date();
    let hours = myDate.getHours();
    let minutes = myDate.getMinutes();
    let horario = parseInt(val_envio) + 1;

    if (envio > 0 && accionActual == 1) {
        var plazo = (getVariableEvt(numHoja).EnPlazo) ? "<strong style='color:green'>En plazo</strong>" : "<strong style='color:red'>Fuera de plazo</strong>";
        var mensaje = "<strong>Código de envío</strong> : " + idEnvio
            + ", <strong>Fecha de envío: </strong>" + getVariableEvt(numHoja).FechaEnvio
            + ", <strong>Enviado </strong>" + plazo;
        return mensaje;
    }
    else {
        var esEmpresaVigente = getVariableEvt(numHoja).EsEmpresaVigente;
        if (esEmpresaVigente) {
            var tipoPlazo = getVariableEvt(numHoja).TipoPlazo;
            var mensajePlazo = getVariableEvt(numHoja).MensajePlazo;

            if (horario != null && accionActual != 3) {
                if (hours > horario) {
                    tipoPlazo = "F";
                }
                else if (hours == horario && minutes > 0) {
                    tipoPlazo = "F";
                }
                else {
                    tipoPlazo = "P";
                }
            }


            if (idEnvio <= 0 && accionActual == 2) {
                mensajePlazo = "Envio hasta las " + horario.toString().padStart(2, '0') + "hrs. con tolerancia de 1 hora de acuerdo al horario seleccionado.";
            }
            else if (idEnvio > 0 && accionActual == 2) {
                var plazo = (getVariableEvt(numHoja).EnPlazo) ? "<strong style='color:green'>En plazo</strong>" : "<strong style='color:red'>Fuera de plazo</strong>";

                mensajePlazo = "<strong>Código de envío</strong> : " + idEnvio
                    + ", <strong>Fecha de envío: </strong>" + getVariableEvt(numHoja).FechaEnvio
                    + ", <strong>Enviado </strong>" + plazo;
                tipoPlazo = (getVariableEvt(numHoja).EnPlazo) ? "P" : "F";
            }

            if (tipoPlazo != null || tipoPlazo != "") {
                if (tipoPlazo != "D") {
                    var mensaje = "<strong style='color:green'>En plazo</strong>";;
                    if (tipoPlazo != "P") mensaje = "<strong style='color:red'>Fuera de plazo</strong>";
                    return "Por favor complete los datos. <strong>Plazo del Envio: </strong>" + mensaje + (mensajePlazo != null ? ", " + mensajePlazo : "");
                } else {
                    var mensaje = "<strong style='color:blue'>Deshabilitado</strong>";
                    return "No está permitido el envió de información, solo para consulta. <strong>Plazo del Envio: </strong>" + mensaje;
                }

            } else {

                if (!getVariableEvt(numHoja).EnPlazo) {
                    return "<strong style='color:red'>Fuera de plazo</strong>";
                }
                else return "<strong style='color:green'>En plazo</strong>";
            }
        } else {
            var msjNoVigente = "La empresa se encuentra <strong style='color:blue'>No Vigente</strong>.";
            var mensaje = "<strong style='color:blue'>Deshabilitado</strong>";
            return msjNoVigente + " No está permitido el envió de información, solo para consulta.";
        }
    }
}

function obtenerMensajeAlertaScada(numHoja) {
    var evt = getVariableEvt(numHoja);
    var mensaje = '';

    var filasCab = evt.FilasCabecera - 1;

    //Validar mensaje
    if (evt.UtilizaScada && evt.ValidaTiempoReal && evt.TieneDataScada) {

        //obtiene el número del "H" que tiene  el ultimo valor 
        var hInicio = 1;
        var h48maximo = evt.Handson.HMaximoData48Enviado - 1;
        var hScadaMaximo = evt.Handson.HMaximoDataScadaDisponible;

        //obtiene el número de fila  con estado de valor "-1" en tiempo real
        var valHInicio = evt.Handson.ListaExcelData[hInicio + filasCab][0];
        var valHAnterior = evt.Handson.ListaExcelData[h48maximo + filasCab][0];
        var valMaximoH = evt.Handson.ListaExcelData[h48maximo + filasCab][0];
        var valMaximoScada = evt.Handson.ListaExcelData[hScadaMaximo + filasCab][0];

        if (hScadaMaximo > h48maximo) {
            //valor anterior del ultimo H envío
            var fechaUltEnvio = evt.FechaEnvioLast;

            if (fechaUltEnvio != null && fechaUltEnvio != '' && h48maximo > 0) {
                mensaje = 'Los datos de las filas [' + valHInicio + ', ' + valMaximoH + "] corresponden al último envío" +
                    ". <b>Los datos restantes a las señales Scada</b>.";
            } else {
                if (hScadaMaximo > 0)
                    mensaje = 'Los datos de las filas [' + valHInicio + ', ' + valMaximoScada + "] corresponden a las señales Scada.";
            }
        }
    }

    return mensaje;
}

function mostrarErrorPrincipal(alerta) {
    $('#mensajePrincipal').removeClass("action-message");
    $('#mensajePrincipal').removeClass("action-alert");
    $('#mensajePrincipal').removeClass("action-exito");
    $('#mensajePrincipal').addClass("action-error");
    $('#mensajePrincipal').html(alerta);
    $('#mensajePrincipal').css("display", "block");
}
///////////////////////////////

function validarSeleccionDatos(numHoja) {
    if (!($(getIdElemento(numHoja, '#hfEmpresa')).val() == $(getIdElemento(numHoja, '#cbEmpresa')).val()
        && $(getIdElemento(numHoja, '#txtFecha')).val() == $(getIdElemento(numHoja, '#hfFecha')).val())) {
        return false;
    }
    return true;
}