var hot;
var MODELO_GRID = null;

// Variables para almacenar la posición de las celdas seleccionadas
var objPosSelecTmp = _inicializarObjetoPosicion();
var objPosSelecOrigen = _inicializarObjetoPosicion();
var objPosSelecDestino = _inicializarObjetoPosicion();

var arrayObjCeldaSelec = [];

// Variables para controlar la habilitación de las opciones del menú
var opcionCopiarActivado = false;
var opcionMoverActivado = false;

var opcionEliminar = false;
var opcionEliminarPorDia = false;

var opcionMarcarRelevante = false;
var opcionMarcarNoRelevante = false;

var opcionVerMensaje = false;
var opcionCambiarEstado = false;
var opcionVerInforme = false;

var OPCION_POPUP_VENTANA = "";

// btn expandir / contraer;
var ocultar = 0;

$(document).ready(function ($) {

    $(document).on('click', '.intervencion', function (e) {
        // your function here
        e.preventDefault();
        var interCodi = $(this).attr("class").split(" ")[1].split("_")[1];
        editarCeldaIntervencion(interCodi);
    });

});

function generarHoTweb() {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }

    var nuevoTamanioTabla = getHeightTablaListado();
    $("#grillaExcel").show();
    $("#TabalaSearch").show();
    nuevoTamanioTabla = nuevoTamanioTabla > 250 ? nuevoTamanioTabla : 250;

    var container = document.getElementById('grillaExcel');
    hot = new Handsontable(container, {
        data: MODELO_GRID.Data,
        maxCols: MODELO_GRID.Columnas.length,
        colHeaders: MODELO_GRID.Headers,
        colWidths: MODELO_GRID.Widths,
        height: nuevoTamanioTabla,
        fixedColumnsLeft: MODELO_GRID.FixedColumnsLeft,
        columnSorting: true,
        manualColumnResize: true,
        sortIndicator: true,
        rowHeaders: false,
        columns: MODELO_GRID.Columnas,
        //hiddenColumns: { //no ocultar columnas porque luego no se renderizan bien algunas celdas
        //    // specify columns hidden by default
        //    columns: [4, 5, 6]
        //},
        outsideClickDeselects: false,
        copyPaste: false,
        autoRowSize: { syncLimit: 30000 },
        cells: function (row, col, prop) {
            var cellProperties = {};

            var columnsColor = MODELO_GRID.ListaColumnasColor;
            for (var i = 0; i < columnsColor.length; i++) {
                if (col == columnsColor[i].indexcabecera && columnsColor[i].isendofweek) {
                    if (columnsColor[i].itypeendofweek == 1) //sabado
                        cellProperties.renderer = firstRowRendererColorSabadoAndSafeHtml;
                    if (columnsColor[i].itypeendofweek == 2) //domingo / feriado
                        cellProperties.renderer = firstRowRendererColorDomingoAndSafeHtml;
                } else if (col == columnsColor[i].indexcabecera && columnsColor[i].isendofweek == false) {
                    cellProperties.renderer = safeHtmlRenderer;
                }
            }

            if (col < MODELO_GRID.FixedColumnsLeft) {
                cellProperties.renderer = titleHtmlRenderer;
            }

            return cellProperties;
        },
        afterSelection: function (actualFila, actualColumna, finalFila, finalColumna) {
            //1. Primero se ejecuta el evento afterSelection

            //obtener valores seleccionados
            //var actualFila = hot.getSelected()[0], actualColumna = hot.getSelected()[1], finalFila = hot.getSelected()[2], finalColumna = hot.getSelected()[3];

            //obtener filas y columnas consistentes
            var vFila = actualFila > finalFila ? finalFila : actualFila;
            var vFilaFinal = finalFila > actualFila ? finalFila : actualFila;
            var vColumna = actualColumna > finalColumna ? finalColumna : actualColumna;
            var vColumnaFinal = finalColumna > actualColumna ? finalColumna : actualColumna;

            //vColumna = vColumna > MODELO_GRID.FixedColumnsLeft ? vColumna : MODELO_GRID.FixedColumnsLeft;

            var vFechaInicio = "";
            var equicodi = 0;

            if (vColumna >= MODELO_GRID.FixedColumnsLeft) {
                var posCol = vColumna - MODELO_GRID.FixedColumnsLeft;
                vFechaInicio = MODELO_GRID.ListaFecha[posCol].DiaDesc;

                //equicodi = parseInt(MODELO_GRID.DataCodigo[vFila][2]) || 0;
                equicodi = parseInt(_getEquicodiFila(vFila)) || 0;
            }

            //generar array temporal
            objPosSelecTmp = {};
            objPosSelecTmp.rowIni = vFila;
            objPosSelecTmp.colIni = vColumna;
            objPosSelecTmp.rowFin = vFilaFinal;
            objPosSelecTmp.colFin = vColumnaFinal;
            objPosSelecTmp.fechaIni = vFechaInicio;
            objPosSelecTmp.equicodi = equicodi;

            ////vData = hot.getDataAtCell(r, c); //data de la celda seleccionada
            //console.log("afterSelection");
            //console.log(r);
            //console.log(c);
            //console.log(r2);
            //console.log(c2);
        },
        contextMenu: {
            //2. Se ejecuta la validación de DISABLED del item
            items: {
                "nuevo": {
                    name: 'Nuevo',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada
                        return bol;
                    }
                },
                //copiar - pegar
                "copiar": {
                    name: 'Copiar',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada
                        return bol;
                    }
                },
                "pegar_agregar": {
                    name: 'Pegar y agregar',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                        if (!bol) {
                            //deshabilitar cuando el destino sea la misma celda de origen
                            if (objPosSelecTmp.rowIni != objPosSelecOrigen.rowIni //diferente fila. No permitido entre distintos equipos
                                //|| objPosSelecTmp.colIni == objPosSelecOrigen.colIni //igual columna. No permitido por no cambiar respecto al origen 
                                || opcionMoverActivado || !opcionCopiarActivado) {
                                bol = true;
                            }
                        }

                        return bol;
                    }
                },
                "pegar_sobrescribir": {
                    name: 'Pegar y sobrescribir',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                        if (!bol) {
                            //deshabilitar cuando el destino sea la misma celda de origen
                            if (objPosSelecTmp.rowIni != objPosSelecOrigen.rowIni //diferente fila. No permitido entre distintos equipos
                                || objPosSelecTmp.colIni == objPosSelecOrigen.colIni //igual columna. No permitido por no cambiar respecto al origen 
                                || opcionMoverActivado || !opcionCopiarActivado) {
                                bol = true;
                            }
                        }

                        return bol;
                    }
                },
                //mover - dejar
                "mover": {
                    name: 'Mover',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada
                        return bol;
                    }
                },
                "dejar_agregar": {
                    name: 'Dejar y agregar',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                        if (!bol) {
                            //deshabilitar cuando el destino sea la misma celda de origen
                            if (objPosSelecTmp.rowIni != objPosSelecOrigen.rowIni //diferente fila. No permitido entre distintos equipos
                                //|| objPosSelecTmp.colIni == objPosSelecOrigen.colIni //igual columna. No permitido por no cambiar respecto al origen 
                                || opcionCopiarActivado || !opcionMoverActivado) {
                                bol = true;
                            }
                        }

                        return bol;
                    }
                },
                "dejar_sobrescribir": {
                    name: 'Dejar y sobrescribir',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                        if (!bol) {
                            //deshabilitar cuando el destino sea la misma celda de origen
                            if (objPosSelecTmp.rowIni != objPosSelecOrigen.rowIni //diferente fila. No permitido entre distintos equipos
                                || objPosSelecTmp.colIni == objPosSelecOrigen.colIni //igual columna. No permitido por no cambiar respecto al origen 
                                || opcionCopiarActivado || !opcionMoverActivado) {
                                bol = true;
                            }
                        }

                        return bol;
                    }
                },
                "eliminar": {
                    name: 'Eliminar',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada
                        return bol;
                    }
                },
                "eliminarPorDia": {
                    name: 'Eliminar día',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                        if (!bol) {
                            //deshabilitar si selecciona varias columnas
                            if (objPosSelecTmp.colIni != objPosSelecTmp.colFin) {
                                bol = true;
                            }
                        }
                        return bol;
                    }
                },
                "agrupar": {
                    name: 'Agrupar',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                        if (!bol) {
                            //deshabilitar cuando no selecciona varias columnas
                            if (objPosSelecTmp.colIni == objPosSelecTmp.colFin) {
                                bol = true;
                            }
                        }
                        return bol;
                    }
                },
                "desagrupar": {
                    name: 'Desagrupar',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                        return bol;
                    }
                },
                "desagruparPorHora": {
                    name: 'Desagrupar por horas',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                        return bol;
                    }
                },
                "relevante": {
                    name: 'Marcar como relevante',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada
                        return bol;
                    }
                },
                "norelevante": {
                    name: 'Marcar como no relevante',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada
                        return bol;
                    }
                },
                "cambiarestado": {
                    name: 'Cambiar Estado',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada
                        return bol;
                    }

                },
                "verMensaje": {
                    name: 'Mensaje',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                        if (!bol) {

                            //habilitar solo si se selecciona una celda
                            if (objPosSelecTmp.colIni != objPosSelecTmp.colFin) {
                                bol = true;
                            }
                            else {
                                if (objPosSelecTmp.rowIni != objPosSelecTmp.rowFin) {
                                    bol = true;
                                }
                            }

                        }
                        return bol;
                    }
                },
                "descargarmsj": {
                    name: 'Descargar mensajes masivos',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada
                        return bol;
                    }
                },
                "verinforme": {
                    name: 'Ver informe',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                        if (!bol) {

                            //habilitar solo si se selecciona una celda
                            if (objPosSelecTmp.colIni != objPosSelecTmp.colFin) {
                                bol = true;
                            }
                            else {
                                if (objPosSelecTmp.rowIni != objPosSelecTmp.rowFin) {
                                    bol = true;
                                }
                            }

                        }
                        return bol;
                    }
                },
                "vermodificacion": {
                    name: 'Ver modificaciones',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                        if (!bol) {
                            //deshabilitar si selecciona varias columnas
                            if (objPosSelecTmp.colIni != objPosSelecTmp.colFin) {
                                bol = true;
                            }
                        }
                        return bol;
                    }
                },
                "verhistoria": {
                    name: 'Ver Historia',
                    disabled: function () {
                        var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                        //habilitar solo si se selecciona una celda o varias de la misma fila
                        if (objPosSelecTmp.rowIni != objPosSelecTmp.rowFin) {
                            bol = true;
                        }

                        return bol;
                    }
                }
            },
            //3. Último se realiza el evento de Callback
            callback: function (key, options) {
                //console.log("callback");
                OPCION_POPUP_VENTANA = key;

                if (key === 'nuevo') {
                    objPosSelecOrigen = objPosSelecTmp;
                    nuevoCeldaIntervencion();
                }

                //copiar - pegar
                if (key === 'copiar') {
                    opcionCopiarActivado = true;
                    opcionMoverActivado = false;
                    objPosSelecOrigen = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                    }, 100);
                }
                if (key === 'pegar_agregar' || key === 'pegar_sobrescribir') {
                    opcionMoverActivado = false;
                    objPosSelecDestino = objPosSelecTmp;

                    setTimeout(function () {
                        ejecutarOpcionSeleccionadaCruzada();
                    }, 100);
                }

                //mover - dejar
                if (key === 'mover') {
                    opcionCopiarActivado = false;
                    opcionMoverActivado = true;
                    objPosSelecOrigen = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                    }, 100);
                }
                if (key === 'dejar_agregar' || key === 'dejar_sobrescribir') {
                    opcionCopiarActivado = false;
                    objPosSelecDestino = objPosSelecTmp;

                    setTimeout(function () {
                        ejecutarOpcionSeleccionadaCruzada();
                    }, 100);
                }

                //
                if (key === 'eliminar') {
                    opcionEliminar = true;
                    objPosSelecOrigen = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                        verVistaPreviaSeleccion();
                    }, 100);
                }
                if (key === 'eliminarPorDia') {
                    opcionEliminarPorDia = true;
                    objPosSelecOrigen = objPosSelecTmp;
                    objPosSelecDestino = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                        verVistaPreviaSeleccion();
                    }, 100);
                }

                if (key == 'agrupar') {
                    objPosSelecOrigen = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                        ejecutarOpcionSeleccionadaCruzada();
                    }, 100);
                }

                if (key == 'desagrupar') {
                    objPosSelecOrigen = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                        ejecutarOpcionSeleccionadaCruzada();
                    }, 100);
                }

                if (key == 'desagruparPorHora') {
                    objPosSelecOrigen = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                        popupDesagregarHoras();
                    }, 100);
                }

                if (key == 'relevante' || key == 'norelevante') {
                    opcionMarcarRelevante = key == 'relevante';
                    opcionMarcarNoRelevante = key == 'norelevante';
                    objPosSelecOrigen = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                        ejecutarOpcionSeleccionadaCruzada();
                    }, 100);
                }
                if (key == 'cambiarestado') {
                    objPosSelecOrigen = objPosSelecTmp;
                    var bol = objPosSelecTmp.colIni < MODELO_GRID.FixedColumnsLeft; //si la columna no tiene intervenciones entonces la opción está deshabilitada

                    if (!bol) {
                        setTimeout(function () {
                            _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                            cambiarestado();
                        }, 100);
                    }

                }
                if (key == 'verMensaje') {
                    opcionVerMensaje = true;
                    objPosSelecOrigen = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                        cru_mostrarMensaje();
                    }, 100);
                }
                if (key == 'descargarmsj') {
                    objPosSelecOrigen = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                        descargarMsjMasivos();
                    }, 100);
                }
                if (key == 'verinforme') {
                    opcionVerInforme = true;
                    objPosSelecOrigen = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                        descargarPdfSustento();
                    }, 100);
                }
                if (key == 'vermodificacion') {
                    opcionVerInforme = true;
                    objPosSelecOrigen = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                        obtenerModificaciones();
                    }, 100);
                }
                if (key == 'verhistoria') {
                    opcionVerInforme = true;
                    objPosSelecOrigen = objPosSelecTmp;

                    setTimeout(function () {
                        _seleccionarCeldaCruzadas(); //seleccionar celdas origen
                        abrirPopupHistoriaEquipo();
                    }, 100);
                }
            },
        }
    });

    hot.addHook('afterOnCellMouseDown', function (event, coords, TD) {
        //console.log(event);
        //console.log(coords);
        //console.log(TD);
        if (coords.row == -1 && (coords.col >= MODELO_GRID.FixedColumnsLeft && coords.col < MODELO_GRID.FixedColumnsLeft + MODELO_GRID.ListaFecha.length)) {
            var posCol = coords.col - MODELO_GRID.FixedColumnsLeft;
            generarVentanaFlotanteDetalle(MODELO_GRID.ListaFecha[posCol]);
        }
    });

    container.addEventListener('dblclick', function (e) {
        hot.getActiveEditor().close();
    });

    hot.render();

    $('#searchgrid').on('keyup', function (event) {
        var value = ('' + this.value).toLowerCase(), row, col, r_len, c_len, td;
        var data = MODELO_GRID.Data;
        var columnas = MODELO_GRID.Columnas;
        var searcharray = [];
        if (value) {
            for (row = 0, r_len = data.length; row < r_len; row++) {
                for (col = 0; col < columnas.length; col++) {
                    if (data[row][col] != null) {
                        if (('' + data[row][col]).toLowerCase().indexOf(value) > -1) {
                            searcharray.push(data[row]);
                            break;
                        }
                    }
                }
            }
            hot.loadData(searcharray);
        }
        else {
            hot.loadData(MODELO_GRID.Data);
        }
    });

    $('input[type="search"]').bind("mouseup", function (e) {
        if (typeof hot != 'undefined') {
            hot.deselectCell();
        }

        var $input = $(this);
        setTimeout(function () {
            var newValue = $input.val();

            if (newValue == "") {
                hot.loadData(MODELO_GRID.Data);
            }
        }, 1);
    });


    updateDimensionHandson(hot);
}

//#endregion

//#region HandsonTable

//#region HandsonTable Style

function safeHtmlRenderer(instance, td, row, col, prop, value, cellProperties) {
    var escaped = Handsontable.helper.stringify(value);

    escaped = strip_tags(escaped, '<em><b><strong><a><big>'); // be sure you only allow certain HTML tags to avoid XSS threats (you should also remove unwanted HTML attributes)
    td.innerHTML = escaped;
    $(td).css({
        "text-align": "center",
        "white-space": "break-spaces"
    });

    return td;
}

function titleHtmlRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.innerHTML = value;
    $(td).attr('title', value);

    return td;
}

//sabado
function firstRowRendererColorSabadoAndSafeHtml(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#B4FFFF';
    td.style.whiteSpace = 'break-spaces';

    var escaped = Handsontable.helper.stringify(value);
    escaped = strip_tags(escaped, '<em><b><strong><a><big>'); // be sure you only allow certain HTML tags to avoid XSS threats (you should also remove unwanted HTML attributes)
    td.innerHTML = escaped;
}

//domingo
function firstRowRendererColorDomingoAndSafeHtml(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#00FFFF';
    td.style.whiteSpace = 'break-spaces';

    var escaped = Handsontable.helper.stringify(value);
    escaped = strip_tags(escaped, '<em><b><strong><a><big>'); // be sure you only allow certain HTML tags to avoid XSS threats (you should also remove unwanted HTML attributes)
    td.innerHTML = escaped;
}

function updateDimensionHandson(hot) {
    if (typeof (hot) != "undefined" && hot !== undefined && hot != null) {
        var offset = Handsontable.Dom.offset(document.getElementById('grillaExcel'));
        var widthHT;
        var heightHT;

        if (offset.top == 222) {
            heightHT = $(window).height() - offset.top - 90;
        }
        else {
            heightHT = $(window).height() - offset.top - 20;
        }

        if (ocultar == 1) {
            heightHT = $(window).height() - 10;
        }

        widthHT = $(window).width() - offset.left - 30;

        hot.updateSettings({
            width: widthHT
        });
    }
}

//#endregion

//#region HandsonTable option contextMenu

function refrescarCeldasModificadas(nuevoModel) {
    //hot.loadData(result.GridExcel.Data);
    //hot.render();

    var colIni = MODELO_GRID.FixedColumnsLeft; //MODELO_GRID.FixedColumnsLeft
    var colFin = MODELO_GRID.FixedColumnsLeft + MODELO_GRID.ListaFecha.length; //actualiza todas las celdas de las filas seleccionadas

    for (var conti = objPosSelecOrigen.rowIni; conti <= objPosSelecOrigen.rowFin; conti++) {

        //obtener nuevos registros/actualización
        var equicodiFila = parseInt(_getEquicodiFila(conti)) || 0;
        var arrayRefresh = _obtenerArrayData(nuevoModel.Data, nuevoModel.DataCodigo, equicodiFila);

        var arrayCambios = [];
        for (var cont = colIni; cont <= colFin; cont++) {
            var arrayCeldaCambio = [];
            arrayCeldaCambio.push(conti);
            arrayCeldaCambio.push(cont);

            var value = "";
            if (arrayRefresh != null) value = arrayRefresh[cont];
            arrayCeldaCambio.push(value);

            //agregar array
            arrayCambios.push(arrayCeldaCambio);
        }

        //Set new value to a cell. To change many cells at once (recommended way), pass an array of changes in format [[row, col, value],...] as the first argument.
        hot.setDataAtCell(arrayCambios);
    }

    MODELO_GRID.ListaFecha = nuevoModel.ListaFecha;

    _inicializarObjetosCruz();
}

function _obtenerArrayData(dataHtml, dataCodigo, equipo) {
    var i = _getFilaEquipoDataHtml(dataCodigo, equipo);
    if (i >= 0)
        return dataHtml[i];
    return null;
}

function _getFilaEquipoDataHtml(dataCodigo, equipo) {
    for (var i = 0; i < dataCodigo.length; i++) {
        if (dataCodigo[i][2] == equipo)
            return i;
    }

    return -1;
}

function _getEquicodiFila(fila) {
    //obtener el equipo de la celda seleccionada (cuando se ordena la columna no coincide con el orden original)

    var rowDataSelec = hot.getData()[fila];

    for (var i = 0; i < MODELO_GRID.Data.length; i++) {
        var filaDataOrig = MODELO_GRID.Data[i];

        if (rowDataSelec[0] == filaDataOrig[0] && rowDataSelec[1] == filaDataOrig[1] && rowDataSelec[3] == filaDataOrig[3]) {
            var equicodi = parseInt(MODELO_GRID.DataCodigo[i][2]) || 0;
            return equicodi;
        }
    }

    return 0;
}

function ejecutarOpcionSeleccionadaCruzada() {
    var msjConfirm = '¿Desea realizar la acción?';

    var flagEjecutarAccion = true;

    var marcarRelevante = '';
    var intercodiRadio = 0;
    switch (OPCION_POPUP_VENTANA) {
        case "pegar_agregar":
            msjConfirm = "¿Desea copiar la(s) intervencion(es)?";
            break;
        case "pegar_sobrescribir":
            msjConfirm = "¿Desea copiar la(s) intervencion(es)? Esta acción eliminará los días de intersección (por día, tipo de intervención y descripción).";
            break;
        case "dejar_agregar":
            msjConfirm = "¿Desea mover la(s) intervencion(es)?";
            break;
        case "dejar_sobrescribir":
            msjConfirm = "¿Desea mover la(s) intervencion(es)? Esta acción eliminará los días de intersección (por día, tipo de intervención y descripción).";
            break;
        case "eliminar":
            msjConfirm = '¿Desea eliminar la(s) intervencion(es)?';
            _listarIntercodiSeleccion();
            break;
        case "eliminarPorDia":
            msjConfirm = '¿Desea eliminar la intervención para el día seleccionado?';
            _listarIntercodiSeleccion();
            break;
        case "relevante":
            marcarRelevante = 'S';
            break;
        case "norelevante":
            marcarRelevante = 'N';
            break;
        case "verMensaje":
            flagEjecutarAccion = false;
            intercodiRadio = _getIntercodiRadioSeleccion();
            break;
    }

    $("#alerta").hide();

    if (flagEjecutarAccion) {
        if (arrayObjCeldaSelec.length > 0) {
            if (!confirm(msjConfirm)) return;

            $.ajax({
                type: "POST",
                url: controler + "EjecutarOpcionIntervencionCruzada",
                traditional: true,
                data: {
                    opcion: OPCION_POPUP_VENTANA,
                    tipoAccion: _obtenerCeldaModifData(),
                    dataArrayCruzada: JSON.stringify(arrayObjCeldaSelec),
                    strFechaDestino: objPosSelecDestino.fechaIni,
                    marcarRelevante: marcarRelevante,
                    strHoraIni: $("#desagregarHoraIni").val(),
                    strHoraFin: $("#desagregarHoraFin").val(),
                },
                success: function (model) {

                    if (OPCION_POPUP_VENTANA == "eliminar" || OPCION_POPUP_VENTANA == "eliminarPorDia" || OPCION_POPUP_VENTANA == "desagruparPorHora") {
                        $('#popupVistaPreviaOpcionCruzada').bPopup().close();
                    }

                    if (model.Resultado != "-1") {
                        if (model.Resultado == "0") {
                            _popupObservaciones(model.ListaIntervencionesErrores);
                        }
                        else {
                            $("#alerta").show();
                            $("#alerta").html("<div class='action-exito ' style='margin: 0; padding-top: 5px; padding-bottom: 5px;'>La acción se ejecutó correctamente.</div>");
                            setTimeout(function () { $("#alerta").fadeOut(1000) }, 2000);

                            mostrarGrillaExcel(true);
                        }
                    } else {
                        alert(model.StrMensaje);
                    }
                },
                error: function (err) {
                    alert("Lo sentimos, ha ocurrido un error inesperado");
                }
            });
        } else {
            $('#popupVistaPreviaOpcionCruzada').bPopup().close();
            alert("No existen intervenciones para realizar la acción.");
        }
    } else {
        //ver mensaje
        $('#popupVistaPreviaOpcionCruzada').bPopup().close();
        abrirPopupComunicaciones(intercodiRadio);
    }
}

function verVistaPreviaSeleccion() {
    $("#btnEjecutarOpcionCruzada").hide();

    var msj = "";
    if (OPCION_POPUP_VENTANA == "desagruparPorHora")
        msj = validarHorasDesagregadas();

    if (msj == "") {
        $.ajax({
            type: 'POST',
            traditional: true,
            dataType: "json",
            url: controler + "ListaXIntercodi",
            data: {
                opcion: OPCION_POPUP_VENTANA,
                tipoAccion: _obtenerCeldaModifData(),
                strFechaDestino: objPosSelecDestino.fechaIni,
                dataArrayCruzada: JSON.stringify(arrayObjCeldaSelec),
                strHoraIni: $("#desagregarHoraIni").val(),
                strHoraFin: $("#desagregarHoraFin").val(),
            },
            success: function (model) {
                if (model.Resultado != "-1") {
                    if (model.Resultado == "0") {
                        _popupObservaciones(model.ListaIntervencionesErrores);
                    } else {
                        _popupSeleccionIntervencion(model.ListaIntervenciones);
                    }
                } else {
                    alert(model.StrMensaje);
                }
            },
            error: function (err) {
                alert("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    } else {
        alert(msj);
    }
}

function popupDesagregarHoras() {
    if (arrayObjCeldaSelec.length > 0) {

        var txtBtn = "Desagrupar Por Hora";
        var tituloPopup = "Desagrupar por Hora";
        $("#popupVistaPreviaOpcionCruzada .popup-title span").html(tituloPopup);
        $("#popupVistaPreviaOpcionCruzada").css({ "width": "300px", "maxWidth": "300px", "height": "300px", "minHeight": "300px" });

        var cadena = '';
        cadena += `
                <div style=''>

                    <div id="selecionHoras" style="width:250px;margin-bottom: 10px;">
                        <div>
                            Hora inicio:
                            <input type="time" class="without_ampm" id="desagregarHoraIni" name="desagregarHoraIni" value="08:00" data-mask="00:00" data-mask-selectonfocus="true" />
                        </div>

                        <div>
                            Hora fin:
                            <input type="time" class="without_ampm" id="desagregarHoraFin" name="desagregarHoraIni" value="17:00" data-mask="00:00" data-mask-selectonfocus="true" style='margin-left: 14px;margin-top: 7px;' />

                        </div>
                    </div>

                </div>
            `;

        cadena += '<div id="alerta_2" class="td_inline" style="margin-bottom: 10px;">    </div>';
        cadena += '<div id="tblVistaPrevia"></div>';

        cadena += `
            <div style='margin-top: 5px;'>
                <input type="button" value="${txtBtn}" id="btnEjecutarOpcionCruzada" >
            </div>
        `;

        $('#idDivVistaPreviaOpcionCruzada').html(cadena);

        $('#popupVistaPreviaOpcionCruzada').bPopup({
            modalClose: false,
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
        });

        $("#btnEjecutarOpcionCruzada").unbind();
        $('#btnEjecutarOpcionCruzada').click(function () {
            ejecutarOpcionSeleccionadaCruzada();
        });

    } else {
        alert("No existen intervenciones para realizar la acción.");
    }
}

function _popupSeleccionIntervencion(listaIntervenciones) {
    if (arrayObjCeldaSelec.length > 0) {
        $("#btnEjecutarOpcionCruzada").unbind();

        var tituloPopup = "";

        var txtBtn = '';
        switch (OPCION_POPUP_VENTANA) {

            case "pegar":
                txtBtn = "Copiar";
                tituloPopup = "Copiar / Pegar" + ' ' + objPosSelecDestino.fechaIni;
                verVistaPrevia = false;
                break;
            case "dejar":
                txtBtn = "Mover";
                tituloPopup = "Mover / Dejar" + ' ' + objPosSelecDestino.fechaIni;
                verVistaPrevia = false;
                break;
            case "eliminar":
                txtBtn = "Eliminar";
                tituloPopup = "Eliminar intervenciones";
                break;
            case "agrupar":
                txtBtn = "Agrupar";
                tituloPopup = "Agrupar";
                break;
            case "desagrupar":
                txtBtn = "Desagrupar";
                tituloPopup = "Desagrupar";
                break;
            case "desagruparPorHora":
                txtBtn = "Desagrupar Por Hora";
                tituloPopup = "Desagrupar por Hora";
                verVistaPrevia = false;
                break;
            case "eliminarPorDia":
                txtBtn = "Eliminar día";
                tituloPopup = "Eliminar intervenciones de día seleccionado" + ' ' + objPosSelecDestino.fechaIni;
                break;
            case "relevante":
                txtBtn = "Marcar relevante";
                tituloPopup = "Marcar relevante";
                break;
            case "norelevante":
                txtBtn = "Marcar No relevante";
                tituloPopup = "Marcar no relevante";
                break;
            case "verMensaje":
                txtBtn = "Ver Mensaje";
                tituloPopup = "Seleccionar intervención";
                break;
        }

        $("#popupVistaPreviaOpcionCruzada .popup-title span").html(tituloPopup);
        $("#popupVistaPreviaOpcionCruzada").css({ "width": "1070px", "maxWidth": "1070px", "height": "600px", "minHeight": "600px" });

        var cadena = '';

        if (OPCION_POPUP_VENTANA == "pegar" || OPCION_POPUP_VENTANA == "dejar") {
            cadena += `
                <div style='margin-bottom: 10px;'>
                    Seleccione opción:
                    <div>
                        <input type="radio" name="opcionCeldaModifData" value="1" checked="checked" style='margin-left: 130px;'>  
                        <span style="font-weight: bold;">Agregar</span> 
                        <span>(si existe intersección entonces se eliminará el registro existente en el destino)<span>
                    </div>
                    <div>
                        <input type="button" value="Vista previa" id="btnVerVistaPrevia" style='margin-top: 7px;'>
                        <input type="radio" name="opcionCeldaModifData" value="2" style='margin-left: 39px;'> 
                        <span style="font-weight: bold;">Sobrescribir</span> 
                        <span>(Se eliminaran los días de intersección)<span>
                    </div>
                </div>
        `;
        }

        cadena += '<div id="alerta_2" class="td_inline" style="margin-bottom: 10px;">    </div>';
        cadena += '<div id="tblVistaPrevia"></div>';

        if (txtBtn != '') {
            cadena += `
            <div style='margin-top: 5px;'>
                <input type="button" value="${txtBtn}" id="btnEjecutarOpcionCruzada" style='display: none'>
            </div>
        `;
        }

        $('#idDivVistaPreviaOpcionCruzada').html(cadena);

        _visualizarPopupListaIntervencion(listaIntervenciones, true, false);

        $('#popupVistaPreviaOpcionCruzada').bPopup({
            modalClose: false,
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
        });

        $("#btnVerVistaPrevia").unbind();
        $('#btnVerVistaPrevia').click(function () {
            ejecutarOpcionSeleccionadaCruzada();
        });

    } else {
        alert("No existen intervenciones para realizar la acción.");
    }
}

function _popupObservaciones(listaIntervencionesErrores) {
    $("#btnEjecutarOpcionCruzada").unbind();

    $("#popupVistaPreviaOpcionCruzada .popup-title span").html("Observaciones");
    $("#popupVistaPreviaOpcionCruzada").css({ "width": "1070px", "maxWidth": "1070px", "height": "600px", "minHeight": "600px" });

    var cadena = '';
    cadena += `<div id="alerta_2" class="td_inline" style="margin-bottom: 10px;">   
            <div class='action-alert ' style='margin: 0; padding-top: 5px; padding-bottom: 5px;'>No existen registros válidos para realizar la acción.</div>
    </div>'`;
    cadena += '<div id="tblVistaPrevia"></div>';

    $('#idDivVistaPreviaOpcionCruzada').html(cadena);

    _visualizarPopupListaIntervencion(listaIntervencionesErrores, false, true);

    $('#popupVistaPreviaOpcionCruzada').bPopup({
        modalClose: false,
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
    });
}

function _visualizarPopupListaIntervencion(lista, mostrarBtn, mostrarColAdicional) {
    var htmlTabla = _dibujarTablaListaIntercodi(lista, mostrarColAdicional);
    $('#tblVistaPrevia').html(htmlTabla);

    if (mostrarBtn) $("#btnEjecutarOpcionCruzada").show();
    $("#btnEjecutarOpcionCruzada").unbind();
    $('#btnEjecutarOpcionCruzada').click(function () {
        ejecutarOpcionSeleccionadaCruzada();
    });

    setTimeout(function () {
        $('#tblReporteVistaPrevia').dataTable({
            scrollY: 450,
            scrollX: true,
            "sDom": 't',
            scrollCollapse: false,
            "destroy": "true",
            "bAutoWidth": false,
            paging: false,
            "ordering": false,
        });

    }, 250);
}

function _dibujarTablaListaIntercodi(lista, mostrarColAdicional) {
    if (lista.length == 0)
        return "<b></b>";

    var incluirColSel = false;
    var flagColSelUnica = false;
    switch (OPCION_POPUP_VENTANA) {
        case "eliminar":
            incluirColSel = true;
            break;
        case "eliminarPorDia":
            incluirColSel = true;
            break;
        case "verMensaje":
            incluirColSel = true;
            flagColSelUnica = true;
            break;
    }

    var cadena = '';
    var thSel = incluirColSel && !mostrarColAdicional ? `<th style='width: 40px'>Seleccionar</th>` : "";
    var thColAdicional = mostrarColAdicional ? `
                <th style='width: 50px;'>Fuente</th>
                <th style='width: 150px;background-color: red;'>Observación</th>
    `: "";
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tblReporteVistaPrevia">
        <thead>
            <tr>
                ${thSel}
                ${thColAdicional}
                <th style='width: 150px'>Horizonte</th>
                <th style='width: 150px'>Fecha</th>
                <th style='width: 100px'>Ubicación</th>
                <th style='width: 50px'>Equipo</th>
                <th style='width: 50px'>Tipo</th>
                <th style='width: 300px'>Descripción</th>
                <th style='width: 50px'>Cod. seg.</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];

        var fuente = item.Comentario != null && item.Comentario != "" ? item.Comentario : "";
        var obs = item.Actividad != null && item.Actividad != "" ? item.Actividad : "";

        var styleFuente = "";
        if (fuente.toUpperCase() == "ORIGEN") styleFuente = 'background-color: #ffe7a4;'
        if (fuente.toUpperCase() == "DESTINO") styleFuente = 'background-color: #fdf1d0;'

        var tdSel = incluirColSel && !mostrarColAdicional ? `<td style="height: 24px"><input type="checkbox" class="chkSeleccion_intercodi" value="${item.Intercodi}" name="chkSeleccion_intercodi" checked></td>` : '';
        if (flagColSelUnica && !mostrarColAdicional) tdSel = `<td style="height: 24px"><input type="radio" class="radioSeleccion_intercodi" value="${item.Intercodi}" name="radioSeleccion_intercodi"></td>`;
        if (incluirColSel) styleFuente = "";
        //if (fuente.toUpperCase() != "ORIGEN" && incluirColSel) tdSel = `<td style="height: 24px"></td>`;

        var tdColAdicional = mostrarColAdicional ? `
                <td style="height: 24px;${styleFuente}">${fuente}</td>
                <td style="height: 24px;${styleFuente}">${obs}</td>
        `
            : "";

        if (incluirColSel || fuente != '' || obs != '') {
            cadena += `
            <tr>
                ${tdSel}
                ${tdColAdicional}
                <td style="height: 24px;${styleFuente}">${item.IntNombTipoProgramacion}</td>
                <td style="height: 24px;width: 150px;${styleFuente}">${item.InterfechainiDesc} <br> ${item.InterfechafinDesc}</td>
                <td style="height: 24px;${styleFuente}">${item.AreaNomb}</td>
                <td style="height: 24px;${styleFuente}">${item.Equiabrev}</td>
                <td style="height: 24px;${styleFuente}">${item.Tipoevenabrev}</td>
                <td style="height: 24px;${styleFuente}">${item.Interdescrip}</td>
                <td style="height: 24px;${styleFuente}">${item.Intercodsegempr}</td>
            </tr>
        `;
        }
    }
    cadena += `</tbody></table>`;

    return cadena;
}

function _listarIntercodiSeleccion() {
    var selected = [];
    $('input[type=checkbox].chkSeleccion_intercodi').each(function () {
        if ($(this).is(":checked")) {
            selected.push($(this).attr('value'));
        }
    });

    if (selected.length > 0) {
        var fechaSel = arrayObjCeldaSelec[0].StrFecha;
        arrayObjCeldaSelec = [];
        arrayObjCeldaSelec.push({
            StrFecha: fechaSel,
            StrListaInterCodi: selected.join(",")
        });
    }
    else
        arrayObjCeldaSelec = [];
}

function _getIntercodiRadioSeleccion() {
    var valor = parseInt($('input[name="radioSeleccion_intercodi"]:checked').val()) || 0;
    return valor;
}

function _obtenerCeldaModifData() {
    var valor = parseInt($('input[name="opcionCeldaModifData"]:checked').val()) || 0;
    return valor;
}

function _seleccionarCeldaCruzadas() {
    arrayObjCeldaSelec = [];

    //recorrer cada DIA
    for (var cont = objPosSelecOrigen.colIni; cont <= objPosSelecOrigen.colFin; cont++) {
        var posCol = cont - MODELO_GRID.FixedColumnsLeft;
        var vFechaInicio = MODELO_GRID.ListaFecha[posCol].DiaDesc;

        var objCelda = { StrFecha: vFechaInicio };
        var listaIntercodi = [];

        //recorrer cada EQUIPO
        for (var conti = objPosSelecOrigen.rowIni; conti <= objPosSelecOrigen.rowFin; conti++) {
            var varDataTmp = hot.getDataAtCell(conti, cont);

            //por cada celda con datos obtener los codigos de intervenciones
            if (varDataTmp !== undefined && varDataTmp != null) {
                listaIntercodi = listaIntercodi.concat(GetArrayHtmlValue(varDataTmp));
            }
        }
        objCelda.StrListaInterCodi = listaIntercodi.join(',')

        //solo agregar si existen intervenciones
        if (objCelda.StrListaInterCodi != '')
            arrayObjCeldaSelec.push(objCelda);
    }
}

function _inicializarObjetosCruz() {
    arrayObjCeldaSelec = [];

    //objPosSelecTmp = _inicializarObjetoPosicion();
    objPosSelecOrigen = _inicializarObjetoPosicion();
    objPosSelecDestino = _inicializarObjetoPosicion();

    opcionCopiarActivado = false;
    opcionMoverActivado = false;

    opcionMarcarRelevante = false;
    opcionMarcarNoRelevante = false;

    opcionEliminar = false;
    opcionEliminarPorDia = false;

    opcionVerMensaje = false;
    opcionCambiarEstado = false;
    opcionVerInforme = false;
}

//validar Horas desagregadas
function validarHorasDesagregadas() {
    var strHoraIni = $("#desagregarHoraIni").val() || "";
    var strHoraFin = $("#desagregarHoraFin").val() || "";

    var validacion = "";
    if (strHoraIni == "" || strHoraFin == '') {
        validacion = "Debe ingresar las horas Inicio y final";
    }
    else {
        //Dividimos la hora y minuto
        var timeInicio = strHoraIni.split(":");
        var timeFin = strHoraFin.split(":");
        var iniHora = timeInicio[0];
        var iniMin = timeInicio[1];
        var finHora = timeFin[0];
        var finMin = timeFin[1];

        var horainicial = iniHora + "/" + iniMin + "/" + "00";
        var horafinal = finHora + "/" + finMin + "/" + "00";

        if (validarFechaRegistro(horainicial, horafinal)) {
            validacion = "Hora Incio debe ser menor a la hora Final.";
        }
    }
    return validacion;
}

function validarFechaRegistro(fecha1, fecha2) {
    var segundosfecha1 = convertirASegundos(fecha1);
    var segundosfecha2 = convertirASegundos(fecha2);

    var diferencia = segundosfecha2 - segundosfecha1;

    if (diferencia <= 0)
        return true;
    else
        return false;
}

function convertirASegundos(tiempo) {

    var x = tiempo.split('/');
    var hor = x[0];
    var min = x[1];
    var sec = x[2];
    return (Number(sec) + (Number(min) * 60) + (Number(hor) * 3600));
}

//#endregion

//#region Detalle de medias horas de Potencia indisponible

function generarVentanaFlotanteDetalle(objColumnaDia) {
    $("#idDivDetalleMediaHora").html('');
    var $containerWidth = $(window).width() - 100;
    $("#popupDetalleMediaHora").css({ "maxWidth": $containerWidth + "px", "width": ($containerWidth - 50) + "px" });

    var tituloPopup = 'Detalle de la Potencia Indisponible - ' + objColumnaDia.DiaDesc;
    $("#popupDetalleMediaHora .popup-title span").html(tituloPopup);

    var html = dibujarTablaDetallePotIndisp(objColumnaDia);
    $('#idDivDetalleMediaHora').html(html);

    $('#tabla_detalle').dataTable({
        scrollY: 550,
        scrollX: true,
        "sDom": 'ft',
        scrollCollapse: false,
        "destroy": "true",
        "bAutoWidth": false,
        paging: false,
        "ordering": false,
        fixedColumns: {
            leftColumns: 3
        },
    });

    $('#popupDetalleMediaHora').bPopup({
        modalClose: false,
        easing: 'easeOutBack',
        speed: 10,
        transition: 'slideDown',
    });

    $("#tabla_detalle").DataTable().draw();
    updateContainerDetalle();
    $("#tabla_detalle").DataTable().draw();
}

function updateContainerDetalle() {
    var $containerWidth = $(window).width();

    $('#idDivDetalleMediaHora').css("width", $containerWidth - 150 + "px");
}

function dibujarTablaDetallePotIndisp(objColumnaDia) {

    var cadena = '';

    var strMediaHora = '';
    for (key in objColumnaDia.ListaMediaHora) {
        var item = objColumnaDia.ListaMediaHora[key];
        strMediaHora += `<th class='truncate_datatable_3' style='font-weight: normal;font-size: 10px;'>${item}</th>`;
    }

    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" style='width: 2500px' id="tabla_detalle">
        <thead>
            <tr>
                <th class='truncate_datatable_1'>Empresa</th>
                <th class='truncate_datatable_1'>Ubicación</th>
                <th class='truncate_datatable_2'>Equipo</th>
                ${strMediaHora}
            </tr>
        </thead>
        <tbody>
    `;

    for (key in objColumnaDia.ListaEquipo) {
        var regEq = objColumnaDia.ListaEquipo[key];

        var strCeldaIndisp = '';
        for (var h = 0; h < 48; h++) {
            var color = !regEq.ListaTieneIndisp[h] ? 'background-color: white;' : 'background-color: #FFE4E1;';
            var valor = regEq.ListaMWIndisp[h] != null ? regEq.ListaMWIndisp[h] : '';
            strCeldaIndisp += `<td class='truncate_datatable_3' style="${color}">${valor}</td>`;
        }

        cadena += `
            <tr>
                <td class='truncate_datatable_1' title='${regEq.EmprNomb}'>${regEq.EmprNomb}</td>
                <td class='truncate_datatable_1' title='${regEq.AreaNomb}'>${regEq.AreaNomb}</td>
                <td class='truncate_datatable_2' title='${regEq.EquiNomb}'>${regEq.EquiNomb}</td>
                ${strCeldaIndisp}
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}

//#endregion

//
function GetArrayHtmlValue(vData) {
    vData = vData != null ? vData : "";

    var listaCadenaCelda = vData.split(' ');

    var arrayIds = []
    for (var i = 0; i < listaCadenaCelda.length; i++) {
        if (listaCadenaCelda[i].startsWith('intercodi_')) {

            var listaCadenaCodigo = listaCadenaCelda[i].split('_');
            arrayIds.push(listaCadenaCodigo[1]);
        }
    }

    return arrayIds;
}

function strip_tags(input, allowed) {
    //var tags = /<\/?([a-z][a-z0-9]*)\b[^>]*>/gi,
    //    commentsAndPhpTags = /<!--[\s\S]*?-->|<\?(?:php)?[\s\S]*?\?>/gi;

    //// making sure the allowed arg is a string containing only tags in lowercase (<a><b><c>)
    //allowed = (((allowed || "") + "").toLowerCase().match(/<[a-z][a-z0-9]*>/g) || []).join('');

    //return input.replace(commentsAndPhpTags, '').replace(tags, function ($0, $1) {
    //    return allowed.indexOf('<' + $1.toLowerCase() + '>') > -1 ? $0 : '';
    //});

    return input;
}

function _inicializarObjetoPosicion() {
    var objPos = { rowIni: 0, colIni: 0, rowFin: 0, colFin: 0, fechaIni: '', equicodi: 0 };
    return objPos;
}

//#endregion

//#region Ventana Nuevo/Edicion Intervenciones Cruzadas

// Carga la ventana de detalle (popup)
function editarCeldaIntervencion(vinterCodi) {
    objPosSelecOrigen = objPosSelecTmp; //guardar en memoria la celda seleccionada

    $('#popupFormIntervencion').html('');

    if (APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO === undefined) { } else {
        APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;
    }

    var objParam = {
        interCodi: vinterCodi,
        progrCodi: 0,
        tipoProgramacion: 0,
        escruzadas: true,
        esFlotante: true
    };

    //IntervencionesFormulario.js
    mostrarIntervencion(objParam);
};

function nuevoCeldaIntervencion() {
    APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;

    objPosSelecOrigen = objPosSelecTmp; //guardar en memoria la celda seleccionada

    $('#popupFormIntervencion').html('');

    var objParam = {
        interCodi: 0,
        progrCodi: parseInt($("#Programacion").val()) || 0,
        tipoProgramacion: parseInt($("#TipoProgramacion").val()) || 0,
        escruzadas: true,
        strFechaCol: objPosSelecOrigen.fechaIni,
        equicodi: objPosSelecOrigen.equicodi
    };

    //IntervencionesFormulario.js
    mostrarIntervencion(objParam);
}

function nuevaIntervencionCruzada() {
    var tipoProgramacion = parseInt($('#TipoProgramacion').val()) || 0;
    if (tipoProgramacion > 0) {
        APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;

        $('#popupFormIntervencion').html('');

        var objParam = {
            interCodi: 0,
            progrCodi: parseInt($("#Programacion").val()) || 0,
            tipoProgramacion: parseInt($("#TipoProgramacion").val()) || 0,
            escruzadas: true,
            strFechaCol: $("#Entidad_Interfechaini").val(),
            tieneCeldaSelec: false
        };

        //IntervencionesFormulario.js
        mostrarIntervencion(objParam);
    } else {
        alert("Seleccione un Tipo de Programación");
    }
}

//#endregion

function cambiarestado() {
    var msj = "";

    if (msj == "") {
        $.ajax({
            type: 'POST',
            url: controler + "IntervencionesCambiarEstado",
            data: {
                stringIntervenciones: JSON.stringify(arrayObjCeldaSelec),
                rep: 0
            },
            success: function (evt) {
                if (evt.length > 2) {
                    $('#popupFormCambiarEstado').html(evt);

                    setTimeout(function () {
                        $('#popupFormCambiarEstado').bPopup({
                            modalClose: false,
                            easing: 'easeOutBack',
                            speed: 450,
                            transition: 'slideDown'
                        });
                    }, 50);
                }
                else {
                    alert("Error: Seleccionar Intervenciones");
                }
            },
            error: function (err) {
                alert("Lo sentimos, ha ocurrido un error inesperado");
            }
        });
    } else {
        alert(msj);
    }
}

//Descargar mensajes masivos en .Rar
function descargarMsjMasivos() {

    var tipo = JSON.stringify(arrayObjCeldaSelec);
    var progrcodi = parseInt($("#Programacion").val()) || 0;

    if (tipo != null && tipo != "") {
        if (confirm("¿Desea descargar los mensajes de la información seleccionada?")) {
            $.ajax({
                type: 'POST',
                url: controler + 'DescargarMensajesMasivos',
                data: {
                    tipo: tipo,
                    progrcodi: progrcodi,
                    modulo: 0
                },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        window.location = controler + "ExportarZipMsjMasivos?file_name=" + evt.Resultado;

                    } else {
                        alert(evt.Mensaje);
                    }
                },
                error: function (err) {
                    alert("Error al descargar mensajes");
                }
            });
        }
    } else {
        alert("Seleccione registros para descargar mensajes.");
    }
}

function descargarPdfSustento() {

    var seleccion = JSON.stringify(arrayObjCeldaSelec);

    if (seleccion != null && seleccion != "[]") {
        $.ajax({
            type: 'POST',
            url: controler + 'ValidarDescargaSustento',
            data: {
                seleccion: seleccion
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    //LLamar poput
                    cargarVistaPreviaSustento(evt.ListaNombreArchivo, evt.Resultado);

                } else {
                    alert(evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Error al descargar mensajes");
            }
        });
    } else {
        alert("Seleccione una intervención para descargar sustento.");
    }
}

function cargarVistaPreviaSustento(listaNombreArchivo, rutaReporte) {

    //generar html
    var txtHtml = dibujarPopupVistaPrevia(listaNombreArchivo, rutaReporte);
    $('#popupVerInforme').html(txtHtml);

    //mostrar popup
    $('#popupVerInforme').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function dibujarPopupVistaPrevia(listaNombreArchivo, rutaReporte) {

    var rutaCompleta = window.location.href;
    var ruraInicial = rutaCompleta.split("/Intervenciones");
    var urlPrincipal = ruraInicial[0];
    if (!urlPrincipal.endsWith('/')) urlPrincipal += '/';

    var txtHtml = `
    <div class="popup-title">
        <span>VER</span>

        <input type="button" id="bSalirSustento" value="Salir" class='b-close' style='float: right; margin-right: 35px;' />                   
    </div>
    
    <div class="content-registro" style="height: 585px; overflow-y: auto">
    
        <div id="mensaje3" class="action-alert" style="margin: 10px 0px 10px; display: none;"></div>
        <div>
    `;

    for (var i = 0; i < listaNombreArchivo.length; i++) {
        var cod = listaNombreArchivo[i];

        var urlFrame = rutaReporte + cod;
        var urlFinal = urlPrincipal + urlFrame;

        var htmlRpta = '';

        htmlRpta += ` <iframe id="vistaprevia_${cod}" src="${urlFinal}" style="width: 100%; height:1200px;" frameborder="0">
                          </iframe>
                    `;

        txtHtml += `${htmlRpta}
                    `;
    }

    txtHtml += `
               </div>
           </div>
    `;

    return txtHtml;
}

//ver mensajes
function cru_mostrarMensaje() {
    var listaCelda = arrayObjCeldaSelec;

    //una celda
    if (listaCelda.length == 1) {

        var listaIntercodi = listaCelda[0].StrListaInterCodi.split(',');
        if (listaIntercodi.length == 1) {
            abrirPopupComunicaciones(listaIntercodi[0]);
        } else {
            //mostrar popup de selección única
            verVistaPreviaSeleccion();
        }
    }
}

//información de intervenciones (modificaciones)
function obtenerModificaciones() {

    var seleccion = JSON.stringify(arrayObjCeldaSelec);

    if (seleccion != null && seleccion != "[]") {
        $.ajax({
            type: 'POST',
            url: controler + 'ObtenerModificaciones',
            data: {
                seleccion: seleccion
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    //LLamar poput
                    cargarModificaciones(evt.ListaModificaciones);

                } else {
                    alert(evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Error al descargar mensajes");
            }
        });
    } else {
        alert("Seleccione una intervención para ver modificaciones.");
    }
}

function cargarModificaciones(lstModificaciones) {

    //generar html
    var txtHtml = dibujarPoputModificaciones(lstModificaciones);
    $('#popupModificaciones').html(txtHtml);

    //mostrar popup
    $('#popupModificaciones').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

function dibujarPoputModificaciones(lstModificaciones) {

    var txtHtml = `
    <div class="popup-title">
        <span>VER</span>

        <input type="button" id="bSalirSustento" value="Salir" class='b-close' style='float: right; margin-right: 35px;' />                   
    </div>
    
    <div class="content-registro" style="height: 585px; overflow-y: auto">
    
        <div id="mensaje3" class="action-alert" style="margin: 10px 0px 10px; display: none;"></div>
        <div>
    `;

    for (var i = 0; i < lstModificaciones.length; i++) {
        var modif = lstModificaciones[i];

        var modificaciones = dibujarModificacionIntervencion(modif.ListaIntervenciones);


        var htmlRpta = '';

        htmlRpta += ` <div id="vistaprevia_${modif.interCodi}" style="width: 100%; height:auto;" >
                      ${modificaciones}
                      </div>
                    `;

        txtHtml += `${htmlRpta}
                    `;
    }

    txtHtml += `
               </div>
           </div>
    `;

    return txtHtml;
}

function dibujarModificacionIntervencion(listaIntervenciones) {

    var html = `
    <div style="overflow-y: auto">

    <div class="form-title">
        <div class="content-titulo">Información de Intervenciónes  </div>
    </div>
    <br />

    <table border="0" class="pretty tabla-icono" id="TablaLogSimple" style="white-space: nowrap;">
        <thead>
            <tr>
                <th style="text-align:left">Fec/Hor.Sistema</th>
                <th style="text-align:left">Usuario</th>
                <th style="text-align:left">Justificacion</th>
                <th style="text-align:left">Clase</th>
                <th style="text-align:left">Estado</th>
                <th style="text-align:left">Rev. Agente</th>
                <th style="text-align:left">Tip.Interv.</th>
                <th style="text-align:left">Ubicación</th>
                <th style="text-align:left">Tipo</th>
                <th style="text-align:left">Equipo</th>
                <th style="text-align:left">Fec.Inicio</th>
                <th style="text-align:left">Fec.Fin</th>
                <th style="text-align:left">MWI</th>
                <th style="text-align:left">Ind</th>
                <th style="text-align:left">Int</th>
                <th style="text-align:left">Descripción</th>
                <th style="text-align:left">Nota</th>
                <th style="text-align:left">Cod. Seguimiento</th>
            </tr>
        </thead>
        <tbody>    `;

    for (var i = 0; i < listaIntervenciones.length; i++) {
        var item = listaIntervenciones[i];

        var sStyle = "";
        var sEstado = "PENDIENTE";
        var revisadoAgente = "Revisado";
        if (item.Interleido == 0) {
            revisadoAgente = "No Revisado";
        }

        if (item.Estadocodi == 2) {   //Aprobado
            if (item.Interprocesado == 2) {
                sEstado = "EN REVERSIÓN";
            }
            else {
                sEstado = "CONFORME";
            }
        }
        if (item.Estadocodi == 3) {   //Rechazado
            sStyle = "background-color:#FFC4C4; text-decoration:line-through";
            sEstado = "RECHAZADO";
        }
        else if (item.Interdeleted == 1) {   //Eliminado
            sStyle = "background-color:#E0DADA; text-decoration:line-through";
            sEstado = "ELIMINADO";
        }

        var fechmodificacion = "";
        var usumodificacion = "";
        if (item.Interusuagrup != null && item.Interusuagrup != "") {

            var fechmodificacion = item.Interfecmodificacion > item.Interfecagrup ? item.UltimaModificacionFechaDesc : item.UltimaModificacionFecAgrupDesc;
            var usumodificacion = item.Interfecmodificacion > item.Interfecagrup ? item.UltimaModificacionUsuarioDesc : item.Interusuagrup;
        }
        else {
            var fechmodificacion = item.UltimaModificacionFechaDesc;
            var usumodificacion = item.UltimaModificacionUsuarioDesc;
        }

        var fechaini = "";
        var fechafin = "";

        //fechaini = Convert.ToDateTime(item.Interfechaini).ToString("dd/MM/yyyy HH:mm")
        //fechafin = Convert.ToDateTime(item.Interfechafin).ToString("dd/MM/yyyy HH:mm")

        html += `<tr id="fila_${item.Intercodi}">
                    <td style="text-align:center; @sStyle">${fechmodificacion}</td>
                    <td style="text-align:center; @sStyle">${usumodificacion}</td>

                    <td style="text-align:left; @sStyle">${item.Interjustifaprobrechaz}</td>
                    <td style="text-align:left; @sStyle">${item.ClaseProgramacion}</td>
                    <td style="text-align:left; @sStyle">${sEstado}</td>
                    <td style="text-align:left; @sStyle">${revisadoAgente}</td>
                    <td style="text-align:left; @sStyle">${item.TipoEvenDesc}</td>
                    <td style="text-align:left; @sStyle">${item.AreaNomb}</td>
                    <td style="text-align:left; @sStyle">${item.FamNomb}</td>
                    <td style="text-align:left; @sStyle">${item.Equiabrev}</td>
                    <td style="text-align:center; @sStyle">${item.InterfechainiDesc}</td>
                    <td style="text-align:center; @sStyle">${item.InterfechafinDesc}</td>
                    <td style="text-align:center; @sStyle">${item.Intermwindispo}</td>
                    <td style="text-align:center; @sStyle">${item.Interindispo}</td>
                    <td style="text-align:center; @sStyle">${item.Interinterrup}</td>
                    <td style="text-align:left; @sStyle">${item.Interdescrip}</td>
                    <td style="text-align:left; @sStyle">${item.Internota}</td>
                    <td style="text-align:left; @sStyle">${item.Intercodsegempr}</td>
                </tr>
                `;
    }


    html += `
            </tbody>
        </table>
    </div>
    `;

    return html;
}

// Cargar pantalla Ver historia (popup)
function abrirPopupHistoriaEquipo() {
    objPosSelecOrigen = objPosSelecTmp; //guardar en memoria la celda seleccionada

    $('#popupFormHistoria').html('');

    var objParam = {
        equicodi: objPosSelecOrigen.equicodi,
        progrCodi: 0,
        tipoProgramacion: 0,
        escruzadas: true
    };

    //IntervencionesHistoria.js
    formularioHistoria(objParam);
};