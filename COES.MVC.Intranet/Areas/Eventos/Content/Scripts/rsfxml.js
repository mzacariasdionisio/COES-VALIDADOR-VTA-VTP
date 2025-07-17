var controlador = siteRoot + 'eventos/rsf/';
var hot = null;
var limites = null;
var cellEdit = [];
var xmlBkp = null;

$(function () {

    $('#btnExportar').on('click', function () {
        generarXML();
    });

    $('#btnExportarAGC').on('click', function () {
        generarXMLAGC();
    });

    $('#btnExportarUnitMaxGeneration').on('click', function () {
        generarXMLUnitMaxGeneration();
    });

    $('#btnGrabar').on('click', function () {
        grabar2();
    });

    consultar();
});


consultar = function () {
    if (hot != null) {
        hot.destroy();
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerestructuraxml',
        data: {
            fecha: $('#hfFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            xmlBkp = result.DatosBkp;
            cargarGrilla(result);
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error');
        }
    });
};

cargarGrilla = function (result) {

    limites = result.ListaLimite;
    

    var container = document.getElementById('contenedor');
    var data = result.Datos;
    var countGrupos = result.Columnas;

    calculateSize = function () {
        var offset;
        offset = Handsontable.Dom.offset(container);
        availableHeight = $(window).height() - offset.top - 10;
        availableWidth = $(window).width() - 2 * offset.left;
        container.style.height = availableHeight + 'px';
        container.style.width = availableWidth + 'px';
        hot.render();
    };

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
        td.style.color = '#fff';
        td.style.backgroundColor = '#4C97C3';
    };

    var tituloRendererAdicional = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '11px';
        td.style.textAlign = 'center';
        td.style.color = '#fff';
        td.style.backgroundColor = '#FF9900';
    };

    var subTituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#D7EFEF';
        td.style.textAlign = 'left';
        td.style.fontWeight = 'normal';
        td.style.color = '#1C91AE';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var totalRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.NumericRenderer.apply(this, arguments);
        td.style.backgroundColor = '#70AD47';
        td.style.textAlign = 'right';
        td.style.fontWeight = 'bold';
        td.style.color = '#ffffff';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };


    indisponibilidadRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        cellProperties.type = 'dropdown';
        cellProperties.source = ['0', '1'];
        Handsontable.renderers.TextRenderer.apply(this, arguments);
    };

    var comentarioRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.textAlign = 'right';
        td.style.fontWeight = 'bold';
        td.style.height = "40px";
        td.style.color = '#AD6500';
        td.style.backgroundColor = '#FFEB9C';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var textoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.height = "40px";
        td.style.backgroundColor = '#EBEBEB';
        td.style.verticalAlign = 'middle';
    };

    var disbledRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#FFDBA4';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
    };

    var disbledRenderer1 = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.NumericRenderer.apply(this, arguments);
        td.style.backgroundColor = '#EBEBEB';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
        cellProperties.format = '0,0.000';
        cellProperties.type = 'numeric';
    };


    var habilitado = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.NumericRenderer.apply(this, arguments);
        td.style.backgroundColor = '#FFFFFF';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
        cellProperties.format = '0,0.000';
        cellProperties.type = 'numeric';
    };

    var disbledDerechaRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.NumericRenderer.apply(this, arguments);
        td.style.backgroundColor = '#FFDBA4';
        td.style.fontSize = '11px';
        td.style.verticalAlign = 'middle';
        cellProperties.format = '0,0.000';
        cellProperties.type = 'numeric';
    };




    var merges = [
        { row: 0, col: 0, rowspan: 1, colspan: 5 }
    ];

    var widths = [75, 1, 160, 140, 110];
    
    for (var i = 0; i <= countGrupos; i++) {
        var columna = 5 + i * 7;
        merges.push({ row: 0, col: columna, rowspan: 1, colspan: 7 });
        merges.push({ row: result.Longitud + 2, col: columna, rowspan: 1, colspan: 7 });

        widths.push(35);
        widths.push(60);
        widths.push(60);
        widths.push(60);
        widths.push(60);
        widths.push(60);
        widths.push(60);
    }
    

    hot = new Handsontable(container, {
        data: data,
        rowHeaders: false,
        colHeaders: false,
        comments: true,
        height: 600,
        fixedRowsTop: 2,
        fixedColumnsLeft: 5,
        maxRows: result.Longitud + 3,
        colWidths: widths,
        afterChange: onChange,
        afterInit: function () {
            /*
            var dataRowSimulateStart = 6;
            for (let i = 0; i < 48; i++) {
                let celdaValor = hot.getDataAtCell(68, dataRowSimulateStart);
                hot.setDataAtCell(68, dataRowSimulateStart, celdaValor);
                dataRowSimulateStart = dataRowSimulateStart + 7;
            }*/
            /*
            setTimeout(function () {
                $('#loading').bPopup({
                    fadeSpeed: 'fast',
                    opacity: 0.4,
                    followSpeed: 500,
                    modalColor: '#000000',
                    modalClose: false
                });
                var dataRowSimulateStart = 6;
                for (let i = 0; i < 48; i++) {
                    let celdaValor = hot.getDataAtCell(68, dataRowSimulateStart);
                    hot.setDataAtCell(68, dataRowSimulateStart, celdaValor);
                    dataRowSimulateStart = dataRowSimulateStart + 7;
                }
                setTimeout(function () { $('#loading').bPopup().close(); }, 500);
                
            }, 500);
            console.log("Calculando Fenix");
            */
            
        },
        cells: function (row, col, prop) {
            /*
            console.log(row + " - " + col);
            console.log(prop);*/

            var cellProperties = {};

            //- Headers
            if (row == 0 || row == 1 || row == result.Longitud + 2) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }

            //- Filas normales
            if (row > 1 && row <= result.Longitud + 1 && col < 5) {
                cellProperties.renderer = subTituloRenderer;
            }

            if (col < 5) {
                cellProperties.readOnly = true;
            }


            if (result.Indices.indexOf(row) != -1 && col > 4) {
                cellProperties.renderer = disbledDerechaRenderer;
                cellProperties.readOnly = true;
            }

            if (row > 1 && row < result.Longitud + 2 && col >= 5) {

                //- Aquí debemos hacer la jugada para los totales

                //console.log(countGrupos);

                for (var i = 0; i <= countGrupos; i++) {

                    if (col == 5 + i * 7) {
                        cellProperties.format = '0';
                        cellProperties.type = 'numeric';
                        cellProperties.readOnly = true;
                    }
                    if (col == 6 + i * 7 || col == 7 + i * 7 || col == 8 + i * 7 || col == 9 + i * 7 || col == 10 + i * 7 || col == 11 + i * 7) {
                        cellProperties.format = '0,0.000';
                        cellProperties.type = 'numeric';
                    }

                    //if ((col == 5 + i * 7 || col == 7 + i * 7 || col == 8 + i * 7 || col == 9 + i * 7 || col == 10 + i * 7 || col == 11 + i * 7) && row <= result.Longitud + 1) {
                    //     cellProperties.renderer = disbledRenderer1;
                    //}

                    var limite = limites[row - 2];

                    //- Primera casuística
                    if (limite.Tipo == 1 || limite.Tipo == 4 || limite.Tipo == 5) {
                        if (limite.TipoGrupo == 1) {

                            if (col === 6 + i * 7) {
                                cellProperties.renderer = habilitado;
                                cellProperties.readOnly = false;
                                cellProperties.format = '0,0.000';
                                cellProperties.type = 'numeric';
                            }

                        }
                        else {
                            cellProperties.renderer = disbledRenderer1;
                            cellProperties.readOnly = true;
                        }

                    }
                    if (limite.Tipo == 2) {

                        if (limite.TipoGrupo != 1) {
                            if (limite.Asignacion == 1) {
                                if (col == 9 + i * 7) {
                                    cellProperties.renderer = disbledRenderer1;
                                }
                                else {
                                    if ((col == 5 + i * 7 || col == 7 + i * 7 || col == 8 + i * 7 || col == 9 + i * 7 || col == 10 + i * 7 || col == 11 + i * 7) && row <= result.Longitud + 1) {
                                        cellProperties.renderer = disbledRenderer1;
                                    }
                                }
                            }
                            else {
                                cellProperties.renderer = disbledRenderer1;
                            }
                        }

                    }

                    if (limite.Tipo == 3) {
                        if (limite.TipoGrupo != 1) {
                            if ((col == 5 + i * 7 || col == 7 + i * 7 || col == 8 + i * 7 || col == 9 + i * 7 || col == 10 + i * 7 || col == 11 + i * 7) && row <= result.Longitud + 1) {
                                cellProperties.renderer = disbledRenderer1;
                            }
                        }
                    }

                }
            }

            //- centrales

            if (result.Indices.indexOf(row) != -1 && col <= 4) {
                cellProperties.renderer = disbledRenderer;
                cellProperties.readOnly = true;
            }



            return cellProperties;
        },
        mergeCells: merges,
    });


    var dataRowSimulateStart = 6;
    for (let i = 0; i < 48; i++) {
        let celdaValor = hot.getDataAtCell(68, dataRowSimulateStart);
        hot.setDataAtCell(68, dataRowSimulateStart, celdaValor);
        dataRowSimulateStart = dataRowSimulateStart + 7;
        console.log("Calculando Fenix");
    }

    function onChange(changes, source) {
       // return;
        
        console.log("cambios");
        console.log("CHANGES " + changes);
        console.log("SOURCE " + source);
        

        if (!changes) {
            return;
        }
        var instance = this;
        changes.forEach(function (change) {
            var row = change[0];
            var col = change[1];
            var newValue = change[3];
            var elemento = limites[row - 2];

            if (change[3] != change[2]) {
                cellEdit.push(hot.getDataAtCell(row, col));
                console.log(cellEdit);
            }

            var rsfUpTxt = "";
            var rsfDownTxt = "";
            var rsfUp = 0;
            var rsfDown = 0;
            var despacho = 0;

            if ((col - 6) % 7 == 0) {

                if (elemento.Tipo == 1) {
                    console.log(elemento.Tipo);
                    hot.setDataAtCell(row, col + 3, newValue);
                    var indexValue = row + elemento.Contador - 1;
                    rsfUpTxt = hot.getDataAtCell(indexValue, col + 1);
                    rsfDownTxt = hot.getDataAtCell(indexValue, col + 2);
                    rsfUp = 0;
                    rsfDown = 0;
                    despacho = 0;

                    if (rsfUpTxt != "" && rsfUpTxt != null) {
                        rsfUp = parseFloat(rsfUpTxt);
                    }
                    if (rsfDownTxt != "" && rsfDownTxt != null) {
                        rsfDown = parseFloat(rsfDownTxt);
                    }
                    if (newValue != "" && newValue != null) {
                        despacho = parseFloat(newValue);
                    }
                    
                    hot.setDataAtCell(indexValue, col + 4, despacho - rsfDown);
                    hot.setDataAtCell(indexValue, col + 5, despacho + rsfUp);
                }
                else if (elemento.Tipo == 2 || elemento.Tipo == 3) {
                    console.log(elemento.Tipo);
                    hot.setDataAtCell(row, col + 3, newValue);

                    rsfUpTxt = hot.getDataAtCell(row, col + 1);
                    rsfDownTxt = hot.getDataAtCell(row, col + 2);
                    rsfUp = 0;
                    rsfDown = 0;
                    despacho = 0;

                    if (rsfUpTxt != "" && rsfUpTxt != null) {
                        rsfUp = parseFloat(rsfUpTxt);
                    }
                    if (rsfDownTxt != "" && rsfDownTxt != null) {
                        rsfDown = parseFloat(rsfDownTxt);
                    }
                    if (newValue != "" && newValue != null) {
                        despacho = parseFloat(newValue);
                    }
                    hot.setDataAtCell(row, col + 4, despacho - rsfDown);
                    hot.setDataAtCell(row, col + 5, despacho + rsfUp);

                    //- Nuevo cálculo 18012020 - Movisoft

                    if (elemento.Tipo == 3) {

                        var limMax = limites[row - 2].LimiteMax;

                        if (despacho == 0) {
                            hot.setDataAtCell(row, col + 4, limMax);
                            hot.setDataAtCell(row, col + 5, limMax);
                        }
                    }

                    //- Fin nuevo cálculo 18012020

                }
                else if (elemento.Tipo == 4) {
                    console.log(elemento.Tipo);
                    hot.setDataAtCell(row, col + 3, newValue);
                    var contador = 0;
                    rsfUpTxt = "";
                    rsfDownTxt = "";
                    rsfUp = 0;
                    rsfDown = 0;
                    despacho = 0;

                    if (newValue != "" && newValue != null) {
                        despacho = parseFloat(newValue);
                    }


                    for (var i = elemento.Indice + 1; i <= elemento.Indice + elemento.Contador - 1; i++) {
                        var dato = hot.getDataAtCell(i, col - 1);

                        if (dato == "1") {
                            contador++;
                            rsfUpTxt = hot.getDataAtCell(i, col + 1);
                            rsfDownTxt = hot.getDataAtCell(i, col + 2);

                            if (rsfUpTxt != "" && rsfUpTxt != null) {
                                rsfUp = rsfUp + parseFloat(rsfUpTxt);
                            }
                            if (rsfDownTxt != "" && rsfDownTxt != null) {
                                rsfDown = rsfDown + parseFloat(rsfDownTxt);
                            }
                        }
                    }

                    if (hot.getDataAtCell(row, col + 1) != "" && hot.getDataAtCell(row, col + 1) != null &&
                        hot.getDataAtCell(row, col + 2) != "" && hot.getDataAtCell(row, col + 2) != null) {
                        rsfUpTxt = hot.getDataAtCell(row, col + 1);
                        rsfDownTxt = hot.getDataAtCell(row, col + 2);
                        rsfUp = parseFloat(rsfUpTxt);
                        rsfDown = parseFloat(rsfDownTxt);
                    }

                    if (contador > 0) {
                        for (var j = elemento.Indice + 1; j <= elemento.Indice + elemento.Contador - 1; j++) {
                            dato = hot.getDataAtCell(j, col - 1);

                            if (dato == "1") {

                                //- Cambio movisoft 20012021

                                if (despacho != 0) {
                                    hot.setDataAtCell(j, col + 4, (despacho - rsfDown) / contador);
                                    hot.setDataAtCell(j, col + 5, (despacho + rsfUp) / contador);
                                }
                                else {
                                    var limMax = limites[j - 2].LimiteMax;
                                    var opeTxt = hot.getDataAtCell(j, col - 1);

                                    hot.setDataAtCell(j, col + 4, parseFloat(limMax) * parseFloat(opeTxt));
                                    hot.setDataAtCell(j, col + 5, parseFloat(limMax) * parseFloat(opeTxt));
                                }

                                //- Fin cambio movisoft 20012021
                            }
                            else {
                                hot.setDataAtCell(j, col + 4, 0);
                                hot.setDataAtCell(j, col + 5, 0);
                            }
                        }
                    }
                }
                else if (elemento.Tipo == 5) {
                    console.log(elemento.Tipo);
                    //- Modificación de formulas para cálculo 16.01.2021

                    hot.setDataAtCell(row, col + 3, newValue);
                    var countMax = 0;
                    rsfUpTxt = "";
                    rsfDownTxt = "";
                    var rsfUpTot = 0;
                    var rsfDownTot = 0;
                    despacho = 0;

                    if (newValue != "" && newValue != null) {
                        despacho = parseFloat(newValue);
                    }

                    for (var i = elemento.Indice + 1; i <= elemento.Indice + elemento.Contador - 1; i++) {
                        var dato = hot.getDataAtCell(i, col - 1);

                        if (dato == "1") {
                            rsfUpTxt = hot.getDataAtCell(i, col + 1);
                            rsfDownTxt = hot.getDataAtCell(i, col + 2);

                            if (rsfUpTxt != "" && rsfUpTxt != null) {
                                rsfUpTot = rsfUpTot + parseFloat(rsfUpTxt);
                            }
                            if (rsfDownTxt != "" && rsfDownTxt != null) {
                                rsfDownTot = rsfDownTot + parseFloat(rsfDownTxt);
                            }
                        }
                    }

                    if (hot.getDataAtCell(row, col + 1) != "" && hot.getDataAtCell(row, col + 1) != null &&
                        hot.getDataAtCell(row, col + 2) != "" && hot.getDataAtCell(row, col + 2) != null) {
                        rsfUpTxt = hot.getDataAtCell(row, col + 1);
                        rsfDownTxt = hot.getDataAtCell(row, col + 2);
                        rsfUpTot = parseFloat(rsfUpTxt);
                        rsfDownTot = parseFloat(rsfDownTxt);
                    }

                    //- Fin de calculo de totales

                    var opeAnt = 0;
                    var scheludedLoad = 0;
                    var scheludedLoadAnt = 0;

                    for (var m = elemento.Indice + 1; m <= elemento.Indice + elemento.Contador - 1; m++) {
                        rsfUpTxt = hot.getDataAtCell(m, col + 1);
                        rsfDownTxt = hot.getDataAtCell(m, col + 2);
                        var opeTxt = hot.getDataAtCell(m, col - 1);
                        rsfUp = 0;
                        rsfDown = 0;
                        despacho = 0;
                        var limMax = limites[m - 2].LimiteMax;
                        var limMin = limites[m - 2].LimiteMin;

                        console.log("Min:" + limMin);
                        console.log("Max:" + limMax);
                        var ope = 0;

                        if (opeTxt != "" && opeTxt != null) {
                            ope = parseFloat(opeTxt);
                        }
                        if (rsfUpTxt != "" && rsfUpTxt != null) {
                            rsfUp = parseFloat(rsfUpTxt);
                        }
                        if (rsfDownTxt != "" && rsfDownTxt != null) {
                            rsfDown = parseFloat(rsfDownTxt);
                        }
                        if (newValue != "" && newValue != null) {
                            despacho = parseFloat(newValue);
                        }

                        if (limites[m - 2].Asignacion != "1") {

                            if (rsfUpTxt != "" && rsfUpTxt != null && rsfDownTxt != "" && rsfDownTxt != null) {
                                if (rsfUp + rsfDown == 0) {
                                    hot.setDataAtCell(m, col + 3, (parseFloat(limMax) + parseFloat(limMin)) / 2);

                                    //- Cambio Movisoft 20012021
                                    //hot.setDataAtCell(m, col + 4, (parseFloat(limMax) + parseFloat(limMin)) / 2);
                                    //hot.setDataAtCell(m, col + 5, (parseFloat(limMax) + parseFloat(limMin)) / 2);
                                    hot.setDataAtCell(m, col + 4, (parseFloat(limMax) * ope));
                                    hot.setDataAtCell(m, col + 5, (parseFloat(limMax) * ope));
                                    //- Fin cambio Movisoft 20012021
                                }
                                else {
                                    hot.setDataAtCell(m, col + 3, (parseFloat(limMax) - rsfUp));
                                    hot.setDataAtCell(m, col + 4, (parseFloat(limMax) * ope - rsfUp - rsfDown));
                                    hot.setDataAtCell(m, col + 5, (parseFloat(limMax) * ope));
                                }
                            }
                        }
                        else {
                            countMax++;

                            if (countMax == 1) {

                                if (ope == 0) {
                                    //- Cambio Movisoft 25012021
                                    //scheludedLoad = (parseFloat(limMax) + parseFloat(limMin)) / 2;
                                    scheludedLoad = parseFloat(limMax);
                                    //- Fin cambio Movisoft 25012021
                                }
                                else {
                                    if (ope * parseFloat(limMax) - despacho >= rsfUp && despacho - ope * parseFloat(limMin) >= rsfDown) {
                                        scheludedLoad = despacho;
                                    }
                                    else {
                                        if (rsfDownTot + rsfUpTot == 0) {
                                            if (despacho > ope * parseFloat(limMax)) {
                                                scheludedLoad = ope * parseFloat(limMax);
                                            }
                                            else {
                                                scheludedLoad = despacho;
                                            }
                                        }
                                        else {
                                            scheludedLoad = ope * parseFloat(limMax) - rsfUp;
                                        }
                                    }
                                }
                                opeAnt = ope;
                                scheludedLoadAnt = scheludedLoad;
                            }
                            else if (countMax == 2) {
                                if (ope == 0) {

                                    //- Cambio Movisoft 25012021
                                    //scheludedLoad = (parseFloat(limMax) + parseFloat(limMin)) / 2;
                                    scheludedLoad = parseFloat(limMax);
                                    //- Fin cambio Movisoft 25012021
                                }
                                else {
                                    if (opeAnt == 0) {

                                        if (ope * parseFloat(limMax) - despacho >= rsfUp && despacho - ope * parseFloat(limMin) >= rsfDown) {
                                            scheludedLoad = despacho;
                                        }

                                        else {
                                            if (rsfDownTot + rsfUpTot == 0) {
                                                if (despacho > ope * parseFloat(limMax)) {
                                                    scheludedLoad = ope * parseFloat(limMax);
                                                }
                                                else {
                                                    scheludedLoad = despacho;
                                                }
                                            }
                                            else {
                                                scheludedLoad = ope * parseFloat(limMax) - rsfUp;
                                            }
                                        }
                                    }
                                    else {

                                        var cal = (despacho - scheludedLoadAnt <= ope * parseFloat(limMin)) ?
                                            ope * parseFloat(limMin) + rsfDown : despacho - scheludedLoadAnt;

                                        if (cal > ope * parseFloat(limMax)) {
                                            scheludedLoad = ope * parseFloat(limMax);
                                        }
                                        else {
                                            if (despacho - scheludedLoadAnt <= ope * parseFloat(limMin)) {
                                                scheludedLoad = ope * parseFloat(limMin) + rsfDown;
                                            }
                                            else {
                                                scheludedLoad = despacho - scheludedLoadAnt;
                                            }
                                        }
                                    }
                                }
                            }

                            hot.setDataAtCell(m, col + 3, scheludedLoad);

                            //- Correción Movisoft 25012021

                            if (despacho != 0) {

                                hot.setDataAtCell(m, col + 4, scheludedLoad - rsfDown);
                                hot.setDataAtCell(m, col + 5, scheludedLoad + rsfUp);
                            }
                            else {
                                hot.setDataAtCell(m, col + 4, parseFloat(limMax));
                                hot.setDataAtCell(m, col + 5, parseFloat(limMax));
                            }
                            //- Fin corrección Movisoft 25012021
                        }
                    }

                    //- Fin Modificación de formulas para cálculo 16.01.2021
                }
            }
        });
    }

};

generarXML = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarxml',
        data: {
            fecha: $('#hfFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                document.location.href = controlador + "descargarxml";
            }
            if (result == -1) {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

generarXMLAGC = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarxml',
        data: {
            fecha: $('#hfFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            $.post(controlador + "generarXMLAGC", { fecha: $('#hfFecha').val() }, function (resultadoCopia) {
                if (resultadoCopia.toUpperCase() == "TRUE") {
                    alert("Los archivos XML's se generaron correctamente y fueron transferidos a la ruta compartida.");
                } else {
                    alert("Error!, se presentó un problema durante el copiado de los archivos XML a la ruta compartida. Contacte a DTI para solucionar el inconveniente.");
                }
            });
        },
        error: function () {
            alert('Error!, se presentó un problema durante la generación de los archivos XML. Contacte a DTI para solucionar el inconveniente.');
        }
    });
};

generarXMLUnitMaxGeneration = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'generarxml',
        data: {
            fecha: $('#hfFecha').val()
        },
        dataType: 'json',
        success: function (result) {
            $.post(controlador + "generarxmlunitmaxgeneration", { fecha: $('#hfFecha').val() }, function (resultadoCopia) {
                if (resultadoCopia.toUpperCase() == "TRUE") {
                    alert("El archivo XML se generó correctamente y fue transferido a la ruta compartida.");
                } else {
                    alert("Error!, se presentó un problema durante el copiado del archivo XML a la ruta compartida. Contacte a DTI para solucionar el inconveniente.");
                }
            });
            //generarXMLAGC
        },
        error: function () {
            alert('Error!, se presentó un problema durante la generación del archivo XML. Contacte a DTI para solucionar el inconveniente.');
        }
    });
};

grabar = function () {
    var datos = hot.getData(0, 0, hot.countRows() - 1, hot.countCols() - 1);
    var validacion = validarDatos(datos);

    if (validacion == "") {
        $.ajax({
            type: "POST",
            url: controlador + 'grabarxml',
            dataType: "json",
            contentType: 'application/json',
            traditional: true,
            data: JSON.stringify({
                fecha: $('#hfFecha').val(),
                datos: datos
            }),
            success: function (result) {
                if (result == 1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente.');
                    consultar(0);
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', validacion);
    }
};

grabar2 = function () {
    var datos = hot.getData(0, 0, hot.countRows() - 1, hot.countCols() - 1);
    var validacion = validarDatos(datos);

    if (validacion == "") {
        $.ajax({
            type: "POST",
            url: controlador + 'grabarxml2',
            dataType: "json",
            contentType: 'application/json',
            traditional: true,
            data: JSON.stringify({
                fecha: $('#hfFecha').val(),
                datos: datos,
                datosBkp: xmlBkp
            }),
            success: function (result) {
                if (result > -1) {
                    mostrarMensaje('mensaje', 'exito', 'Los datos se grabaron correctamente. Se modificaron ' + result + ' datos.');
                    consultar(0);
                }
                else {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            },
            error: function () {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'alert', validacion);
    }
};

validarDatos = function (data) {
    var mensaje = "<ul>";
    var validacionHeader = true;
    var validacionColumnas = true;
    var validacionFormato = true;
    for (var i = 5; i < data[0].length; i++) {
        //if (data[0][i] == null || data[0][i] == "") {
        //    validacionHeader = false;
        //}
        var validacionItem = false;
        for (var j = 2; j < data.length - 2; j++) {
            if (data[j][i] != null && data[j][i] != "") {
                validacionItem = true;

                if (!$.isNumeric(data[j][i])) {
                    validacionFormato = false;
                }
            }
        }
        validacionColumnas = validacionColumnas && validacionItem;
    }

    if (!validacionHeader) {
        mensaje = mensaje + "<li>Ingrese todos los rangos de horas.</li>";
    }
    if (!validacionColumnas) {
        mensaje = mensaje + "<li>Ingrese al menos un valor correcto de cada columna.</li>";
    }
    if (!validacionFormato) {
        mensaje = mensaje + "<li>Ingrese valores correctos.</li>";
    }

    mensaje = mensaje + "</ul>";

    if (validacionHeader && validacionColumnas && validacionFormato) {
        mensaje = "";
    }
    mensaje = "";
    return mensaje;
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

limpiarMensaje = function () {
    $('#mensaje').removeClass();
    $('#mensaje').html("Completa los datos");
    $('#mensaje').addClass('action-message');
};
