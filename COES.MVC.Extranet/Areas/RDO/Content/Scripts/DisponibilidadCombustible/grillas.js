/// Crea El objeto Handson en la pagina web
var container;
var previoChange = 0;

function crearGrillaExcelConsumo(evt) {
    nfilConsumo = evt.Handson.Width;
    listaPtos = evt.Formato.ListaHoja[0].ListaPtos;
    listaPtosStock = evt.Formato.ListaHoja[1].ListaPtos;
    nfilStock = listaPtosStock.length / 2;
    grillaBD = evt.Handson.ListaExcelData;
    enabledStockIni = evt.EnabledStockInicio;
    recuperaStockInicio(grillaBD);
    mostrarmensajes(evt.EnPlazo, evt.IdEnvio, evt.FechaEnvio);

    function headerRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.textAlign = 'center';
        td.style.color = 'White';
        td.style.background = '#2980B9';
    }

    function descripRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.fontWeight = 'bold';
        td.style.color = 'MidnightBlue';
        td.style.background = 'LightSkyBlue';
    }

    function descrip2RowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.textAlign = 'left';
        td.style.fontWeight = 'bold';
        td.style.background = '#E0FFFF';
    }

    function descrip3RowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.textAlign = 'left';
        td.style.background = '#DCDCDC';
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.background = '#FFFFD7';
        if (value != "" && col != columnas[0].observacion) {
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        }
    }

    var container = document.getElementById('detalleFormato');
    hotOptions = {
        data: grillaBD,
        mergeCells: evt.Handson.ListaMerge,
        maxRows: nfilConsumo + nfilStock + 5,
        height: 500,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        colWidths: [150, 80, 100, 100, 100, 100, 120, 100, 220],
        className: "htCenter",
        readOnly: evt.Handson.ReadOnly,
        formulas: true,
        afterChange: function (changes, source) {
            if (changes != null) {
                for (var i = changes.length - 1; i >= 0; i--) {
                    row = changes[i][0];
                    if ((changes[i][1] == 4) || (changes[i][1] == 3)) {
                        var valorOld = changes[i][2];
                        var valorNew = changes[i][3];
                        for (var z = 0; z < nfilStock; z++) {
                            fila = z + nfilConsumo + 6;
                            valor = "";
                            try {
                                valor = this.plugin.helper.cellValue(getExcelColumnName(columnas[0].final + 1) + (fila).toString());
                                if (row < nfilConsumo + 5) {
                                    var cadena = "Sum(E" + (row + 1).toString() + ")";
                                    var formula = this.getDataAtCell(fila - 1, columnas[0].total);
                                    var n = formula.indexOf(cadena);
                                    if (n > -1) {
                                        this.setDataAtCell(nfilConsumo + 5 + z, columnas[0].declarado, valor);
                                    }
                                }
                                else {
                                    if (row == fila - 1) {
                                        this.setDataAtCell(nfilConsumo + 5 + z, columnas[0].declarado, valor);
                                    }
                                }
                            }
                            catch (err) {
                                console.log("Error: " + err.message);
                            }

                        }
                    }
                }
            }
        },
        cells: function (row, col, prop) {
            var cellProperties = {};

            //configuracion de titulo
            if (row == 0 && col < columnas[1].fin1) {
                cellProperties = {
                    renderer: headerRenderer,
                    readOnly: true
                }
            }
            if (col == columnas[1].fin1) {
                cellProperties = {
                    readOnly: true
                }
            }

            if (row == (nfilConsumo + 3)) {
                cellProperties = {
                    renderer: headerRenderer,
                    readOnly: true
                }
            }
            if (row == 1 || row == (nfilConsumo + 4)) {
                cellProperties = {
                    renderer: descripRowRenderer,
                    readOnly: true
                }
            }
            if (row >= 2 && row < 2 + nfilConsumo) {
                switch (col) {
                    case columnas[1].central:
                    case columnas[1].tipo:
                    case columnas[1].unidad:
                    case columnas[1].medidor:
                        render_merge(this, row, col);
                        cellProperties = {
                            readOnly: true
                        }
                        break;
                    case columnas[1].consumo:
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        else {
                            render_celda_color_stock(this, row, col);
                        }
                        cellProperties = {
                            format: '0,0.000',
                            type: 'numeric'
                        }
                        break;
                    case columnas[1].total:
                        render_total(this, row, col);
                        cellProperties = {
                            readOnly: true,
                            format: '0,0.000',
                            type: 'numeric'
                        }
                        break;
                }
            }
            if (row == nfilConsumo + 2) {
                cellProperties = {
                    readOnly: true
                }
            }
            if (row >= nfilConsumo + 5) {
                switch (col) {
                    case columnas[0].central:
                    case columnas[0].tipo:
                    case columnas[0].unidad:
                        render_vertical_align(this, row, col);
                        cellProperties = {
                            renderer: descrip2RowRenderer,
                            readOnly: true
                        }
                        break;
                    case columnas[0].inicial:
                        var readOnly = true;
                        if (enabledStockIni && !evt.Handson.ReadOnly) {
                            readOnly = false;
                            render_celda_color_stock(this, evt, row, col);
                        }
                        else {
                            render_readonly(this, row, col);
                        }
                        render_vertical_align(this, row, col);
                        cellProperties = {
                            format: '0,0.000',
                            type: 'numeric',
                            readOnly: readOnly
                        }
                        break;

                    case columnas[0].recepcion:
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        else {
                            render_celda_color_stock(this, row, col);
                        }
                        cellProperties = {
                            format: '0,0.000',
                            type: 'numeric'
                        }
                        break;
                    case columnas[0].total:
                        render_total(this, evtHot, row, col);
                        render_vertical_align(this, row, col);
                        cellProperties = {
                            readOnly: true,
                            format: '0,0.000',
                            type: 'numeric'
                        }
                        break;
                    case columnas[0].final:
                        render_readonly(this, row, col);
                        cellProperties = {
                            readOnly: true,
                            format: '0,0.000',
                            type: 'numeric'
                        }
                        break;
                    case columnas[0].declarado:
                        render_total_declarado(this, row, col);
                        render_vertical_align(this, row, col);
                        cellProperties = {
                            format: '0,0.000',
                            type: 'numeric'
                        }
                        break;
                    case columnas[0].observacion:
                        this.maxLength = maxCadena;
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        cellProperties = {
                            type: 'text'
                        }
                        break;
                }

            }

            for (var i in evt.ListaCambios) {

                if ((row == evt.ListaCambios[i].Row) && (col == evt.ListaCambios[i].Col)) {
                    switch (col) {
                        case columnas[0].observacion:
                            cellProperties = {
                                renderer: cambiosCellRenderer,
                                type: 'text'
                            }
                            break;
                        default:
                            cellProperties = {
                                renderer: cambiosCellRenderer,
                                format: '0,0.000',
                                type: 'numeric'
                            }
                            break;
                    }

                }
            }
            return cellProperties;
        }
    };
    hot = new Handsontable(container, hotOptions);
}

function crearGrillaExcelPresion(evt) {

    var nColumnas = evt.Handson.ListaExcelData[0].length;
    var listaPtos = evt.ListaHojaPto;
    mostrarmensajes(evt.EnPlazo, evt.IdEnvio, evt.FechaEnvio);

    function descripRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        if (col > 0)
            if (parseInt(listaPtos[col - 1].Tptomedicodi) == 40) {
                td.style.background = '#E8F6FF';
                td.style.color = 'MidnightBlue';
            }
    }

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
    }

    function fechaRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#E8F6FF';
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.color = 'black';
        td.style.background = '#FFFFD7';
        $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
    }

    function readOnlyValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.color = 'DimGray';
        td.style.background = 'Gainsboro';
        if (parseFloat(value))
            $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
        else {
            if (value == "0")
                $(td).html("0.000");
        }
    }

    var container = document.getElementById('detalleFormato');
    hotOptions = {
        data: evt.Handson.ListaExcelData,
        mergeCells: evt.Handson.ListaMerge,
        maxRows: 27,
        height: 500,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        colWidths: evt.Handson.ListaColWidth,
        readOnly: evt.Handson.ReadOnly,

        cells: function (row, col, prop) {
            var cellProperties = {};
            var render;
            var formato = "";
            var tipo;
            readOnly = true;
            if (row == 0) {
                render = firstRowRenderer;
            }
            if (row > 0 && row <= 2) {
                render = descripRowRenderer;
            }
            if (row >= 3 && row <= 27 && col == 0) {
                render = fechaRenderer;
            }
            if (row >= 3 && row <= 27 && col >= 1 && col <= nColumnas) {
                if (evt.Handson.ReadOnly) {
                    render_readonly(this, row, col);
                }
                else {
                    readOnly = evt.Handson.ListaFilaReadOnly[row];
                    if (evt.Handson.ListaFilaReadOnly[row]) {
                        render = readOnlyValueRenderer;
                    }
                    else {
                        render_celda_color(this, evt, row, col);
                        readOnly = false;
                    }
                }
                formato = '0,0.000';
                tipo = 'numeric';
            }

            for (var i in evt.ListaCambios) {
                if ((row == evt.ListaCambios[i].Row) && (col == evt.ListaCambios[i].Col)) {
                    render = cambiosCellRenderer;
                    formato = '0,0.000';
                    tipo = 'numeric';
                }
            }
            cellProperties = {
                renderer: render,
                format: formato,
                type: tipo,
                readOnly: readOnly
            }

            return cellProperties;
        }
    };
    hot = new Handsontable(container, hotOptions);

}

function crearGrillaExcelDisponibilidad(evt) {
    combo = evt.ListaEmbalses;
    fechaDia = $("#hfFecha").val();
    modoLectura = evt.Handson.ReadOnly;
    ptosmedicion = evt.ListaPtosSelect;
    grillaBD = evt.Handson.ListaExcelData;
    agregarColumna();
    nFilasFor2 = grillaBD.length - 1;
    listaFecha = evt.Handson.ListaSourceDropDown[0];
    var date_validator_regexp = /^(0[0-9]|1[0-9]|2[0123])[:]([0-5][0-9])[:]([0-5][0-9]$)/;
    var optionsList = evt.ListaPtoMedicion;
    listaCentrales = evt.ListaPtoMedicion;
    listaPtos = evt.ListaHojaPto;
    container = document.getElementById('detalleFormato');
    mostrarmensajes(evt.EnPlazo, evt.IdEnvio, evt.FechaEnvio);

    var datosBDRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
    };

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4682B4';
        td.style.fontSize = '11px';
        td.style.color = '#FFFFFF'
    };

    var dropdownCentralRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var selectedId;

        for (var index = 0; index < optionsList.length; index++) {
            if (parseInt(value) === optionsList[index].id) {
                selectedId = optionsList[index].id;
                value = optionsList[index].text;
            }
        }
        $(td).addClass("estilocombo");
        Handsontable.TextCell.renderer.apply(this, arguments);
        $('#selectedId').text(selectedId);
    }

    var dropdownEstadoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var selectedId;

        for (var index = 0; index < listaEstado.length; index++) {
            if (parseInt(value) === listaEstado[index].id) {
                selectedId = listaEstado[index].id;
                value = listaEstado[index].text;
            }
        }
        $(td).addClass("estilocombo");
        Handsontable.TextCell.renderer.apply(this, arguments);
        $('#selectedId').text(selectedId);
    }

    function delete_row_renderer(instance, td, row, col, prop, value, cellProperties) {
        var div;
        $(td).children('.btn').remove();
        div = document.createElement('div');
        div.className = 'btn';
        div.appendChild(document.createTextNode("."));
        td.appendChild(div);

        $(div).on('mouseup', function () {
            if (confirm("Desea eliminar?")) {
                eliminarFilaHandson();
                return instance.alter("remove_row", row);
            }
        });
        $(td).addClass("estilodelete");
        return td;
    }

    hotOptions = {
        data: grillaBD,
        height: 500,
        maxRows: evt.Handson.ListaExcelData.length,
        maxCols: 7,
        //width: evt.Handson.Width,
        colHeaders: true,
        rowHeaders: true,
        //stretchH: 'all',
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        colWidths: [350, 130, 200, 150, 0, 50, 0],//evt.Handson.ListaColWidth,
        hiddenColumns: {
            columns: [4, 6]
        },
        //readOnly: modoLectura,
        contextMenu: ['row_below', 'remove_row'],
        columns: [
            {},
            {},
            {},
            {},
            {},
            {},
            {}
        ],
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            if (row > 0) {
                switch (col) {
                    case 0:
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        render_vertical_align(this, row, col);
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownCentralRenderer;
                        cellProperties.width = "200px";
                        cellProperties.select2Options = {
                            data: optionsList,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false,
                        };
                        break;
                    //case 1:
                    //    //render_readonly(this, row, col);
                    //    //if (evt.Handson.ReadOnly) {
                    //    //    render_readonly(this, row, col);
                    //    //}
                    //    //render_vertical_align(this, row, col);
                    //    //cellProperties.readOnly = true;
                    //    break;
                    case 1:
                        cellProperties.readOnly = true;
                        break;
                    case 2:
                        cellProperties.format = '0,0.000';
                        cellProperties.type = 'numeric';
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        else {
                            render_celda_error(this, row, col);
                        }
                        render_vertical_align(this, row, col);
                        break;
                    case 3:
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        render_vertical_align(this, row, col);
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownEstadoRenderer;
                        cellProperties.width = "100px";
                        cellProperties.select2Options = {
                            data: listaEstado,
                            dropdownAutoWidth: true,
                            width: 'resolve'
                        };
                        break;
                    case 4:
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        cellProperties.readOnly = true;
                        break;
                    case 5:
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        if (!modoLectura) {
                            cellProperties.renderer = delete_row_renderer;
                        }
                        render_vertical_align(this, row, col);
                        break;
                }

                // cellProperties.readOnly = true;
            }

            return cellProperties;
        },
    };
    hot = new Handsontable(container, hotOptions);

    hot.updateSettings({
        contextMenu: {
            callback: function (key, options) {
                if (key === 'about') {
                    setTimeout(function () {
                        // timeout is used to make sure the menu collapsed before alert is shown
                        alert("This is a context menu with default and custom options mixed");
                    }, 100);
                }
            },
            items: {
                "row_below": {
                    name: 'Agregar Registro',
                    disabled: function () {
                        // if first row, disable this option
                        return ((hot.getDataAtCol(1).length - nFilasFor2 - 1) == optionsList.length)

                    }
                },
                "remove_row": {
                    name: 'Borrar Registro?',
                    disabled: function () {
                        // if first row, disable this option
                        return hot.getSelected()[0] <= nFilasFor2
                    }
                }
            }
        }
    })
}

function crearGrillaExcelQuemaGas(evt) {
    fechaDia = $("#hfFecha").val();
    modoLectura = evt.Handson.ReadOnly;
    mostrarmensajes(evt.EnPlazo, evt.IdEnvio, evt.FechaEnvio);
    ptosmedicion = evt.ListaPtosSelect;
    grillaBD = evt.Handson.ListaExcelData;
    agregarColumna();
    nFilasFor2 = grillaBD.length - 1;
    var optionsList = evt.ListaPtoMedicion;
    listaPtos = evt.ListaHojaPto;
    container = document.getElementById('detalleFormato');
    mostrarmensajes(evt.EnPlazo, evt.IdEnvio, evt.FechaEnvio);

    var datosBDRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '11px';
    };

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4682B4';
        td.style.fontSize = '11px';
        td.style.color = '#FFFFFF'
    };

    var dropdownCentralRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var selectedId;

        for (var index = 0; index < optionsList.length; index++) {
            if (parseInt(value) === optionsList[index].id) {
                selectedId = optionsList[index].id;
                value = optionsList[index].text;
            }
        }
        $(td).addClass("estilocombo");
        Handsontable.TextCell.renderer.apply(this, arguments);
        $('#selectedId').text(selectedId);
    }

    var dropdownTipoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var selectedId;

        for (var index = 0; index < listaTipo.length; index++) {
            if (parseInt(value) === listaTipo[index].id) {
                selectedId = listaTipo[index].id;
                value = listaTipo[index].text;
            }
        }
        $(td).addClass("estilocombo");
        Handsontable.TextCell.renderer.apply(this, arguments);
        $('#selectedId').text(selectedId);
    }

    function delete_row_renderer(instance, td, row, col, prop, value, cellProperties) {
        var div;
        $(td).children('.btn').remove();
        div = document.createElement('div');
        div.className = 'btn';
        div.appendChild(document.createTextNode("."));
        td.appendChild(div);
        $(div).on('mouseup', function () {
            if (confirm("Desea eliminar?")) {
                eliminarFilaHandson();
                return instance.alter("remove_row", row);
            }
        });
        $(td).addClass("estilodelete");
        return td;
    }

    hotOptions = {
        data: grillaBD,
        height: 500,
        maxRows: evt.Handson.ListaExcelData.length,
        maxCols: 7,
        //width: evt.Handson.Width,
        colHeaders: true,//['FECHA', 'EMBALSE', 'HORA INICIO','HORA FIN','CAUDAL m/3','DESCRIPCIÓN'],
        rowHeaders: true,
        //stretchH: 'all',
        fillHandle: true,
        columnSorting: false,
        //contextMenu: true,
        className: "htCenter",
        colWidths: evt.Handson.ListaColWidth,
        readOnly: modoLectura,
        contextMenu: ['row_below', 'remove_row'],
        hiddenColumns: {
            columns: [6]
        },
        columns: [
            {},
            {},
            {
                type: 'time',
                timeFormat: 'HH:mm:ss',
                correctFormat: true
            },
            {},
            {
                maxLength: maxCadena
            },
            {},
            {}
        ],
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            if (row > 0) {
                switch (col) {
                    case 0:
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        render_vertical_align(this, row, col);
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownCentralRenderer;
                        cellProperties.width = "200px";
                        cellProperties.select2Options = {
                            data: optionsList,
                            dropdownAutoWidth: true,
                            width: 'resolve'
                        };
                        break;
                    case 1:
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        render_vertical_align(this, row, col);
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownTipoRenderer;
                        cellProperties.width = "100px";
                        cellProperties.select2Options = {
                            data: listaTipo,
                            dropdownAutoWidth: true,
                            width: 'resolve'
                        };

                        break;
                    case 2:
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        else {
                            render_celda_time_quema(this, row, col);
                        }
                        render_vertical_align(this, row, col);
                        break;
                    case 3:
                        cellProperties.format = '0,0.000';
                        cellProperties.type = 'numeric';
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        else {
                            render_celda_error(this, row, col);
                        }
                        render_vertical_align(this, row, col);
                        break;
                    case 4:
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        render_vertical_align(this, row, col);
                        break;
                    case 5:
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        if (!modoLectura) {
                            cellProperties.renderer = delete_row_renderer;
                        }
                        render_vertical_align(this, row, col);
                        break;
                }

                // cellProperties.readOnly = true;
            }

            return cellProperties;
        },
    };
    hot = new Handsontable(container, hotOptions);

    hot.updateSettings({
        contextMenu: {
            callback: function (key, options) {
                if (key === 'about') {
                    setTimeout(function () {
                        // timeout is used to make sure the menu collapsed before alert is shown
                        alert("This is a context menu with default and custom options mixed");
                    }, 100);
                }
            },
            items: {
                "row_below": {
                    name: 'Agregar Registro',
                    disabled: function () {
                        // if first row, disable this option
                        return ((hot.getDataAtCol(1).length - nFilasFor2 - 1) == optionsList.length)

                    }
                },
                "remove_row": {
                    name: 'Borrar Registro?',
                    disabled: function () {
                        // if first row, disable this option
                        return hot.getSelected()[0] <= nFilasFor2
                    }
                }
            }
        }
    })
}

function render_celda_color(ht, evtHot, row, col) {
    fondoLimSup = "yellow";
    fondoLimInf = "orange";
    fondo = "white"
    cell_color = "#000";
    font_color = "#fff";
    limiteInf = 0;
    limiteSup = 0;
    value = ht.instance.getDataAtCell(row, col);
    td = ht.instance.getCell(row, col);
    listaPtos = evtHot.ListaHojaPto;
    if (value != null) {
        celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
        if (Number(value)) {
            limiteInf = listaPtos[col - 1].Hojaptoliminf;
            limiteSup = listaPtos[col - 1].Hojaptolimsup;
            if (Number(value) < limiteInf) {
                td.style.background = errores[errorLimInferior].BackgroundColor;
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                eliminarError(celda, errorLimInferior);
                agregarError(celda, value, errorLimInferior);
            }
            else {
                eliminarError(celda, errorLimInferior);
                if (Number(value) > limiteSup) {
                    td.style.background = errores[errorLimSuperior].BackgroundColor;
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    eliminarError(celda, errorLimSuperior);
                    agregarError(celda, value, errorLimSuperior);
                }
                else {
                    eliminarError(celda, errorLimSuperior);
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            eliminarError(celda, errorNoNumero);
            if (value == "0")
                $(td).html("0.000");
            else if (value != "") {
                if (!Number(value)) {
                    td.style.background = errores[errorNoNumero].BackgroundColor;
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, errorNoNumero);
                }
            }
        }

    }

    //$(ht.instance.getCell(row, col)).css(
    //    {
    //        "color": "black",
    //        "background-color": fondo,
    //        "vertical-align": "middle"
    //    });
}

function render_celda_color_stock(ht, row, col) {
    fondoLimSup = "yellow";
    fondoLimInf = "orange";
    fondo = "white"
    cell_color = "#000";
    font_color = "#fff";
    indexPto = 0;
    limiteInf = 0;
    limiteSup = 0;
    value = ht.instance.getDataAtCell(row, col);
    celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
    td = ht.instance.getCell(row, col);
    if (value != null) {
        var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
        if (Number(value)) {
            if (row >= 2 && row < 2 + nfilConsumo) {
                indexPto = row - 2;
                limiteInf = listaPtos[indexPto].Hojaptoliminf;
                limiteSup = listaPtos[indexPto].Hojaptolimsup;
            }
            else {
                if (row >= nfilConsumo + 5) {
                    switch (col) {
                        case columnas[0].inicial:
                        case columnas[0].final:
                            indexPto = (row - nfilConsumo - 5) * 2;
                            break;
                        case columnas[0].recepcion:
                            indexPto = (row - nfilConsumo - 5) * 2 + 1;
                            break;
                    }
                    limiteInf = listaPtosStock[indexPto].Hojaptoliminf;
                    limiteSup = listaPtosStock[indexPto].Hojaptolimsup;
                }
            }
            // indexPto = indexOfPto(ptoSeleccionado);
            if (Number(value) < limiteInf) {
                td.style.background = errores[errorLimInferior].BackgroundColor;
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                eliminarError(celda, errorLimInferior);
                agregarError(celda, value, errorLimInferior);
            }
            else {
                eliminarError(celda, errorLimInferior);
                if (Number(value) > limiteSup) {
                    td.style.background = errores[errorLimSuperior].BackgroundColor;
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    eliminarError(celda, errorLimSuperior);
                    agregarError(celda, value, errorLimSuperior);
                }
                else {
                    eliminarError(celda, errorLimSuperior);
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            eliminarError(celda, errorNoNumero);
            if (value == "0")
                $(td).html("0.000");
            else if (value != "") {
                if (isNaN(Number(value))) {
                    //console.log("Error:" + Number("PP"));
                    td.style.background = errores[errorNoNumero].BackgroundColor;
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, errorNoNumero);
                }
            }
        }
    }
    $(ht.instance.getCell(row, col)).css(
        {
            "vertical-align": "middle"
        });
}

function render_total(ht, row, col) {
    fondo = "#4682B4"
    font_color = "white";

    value = ht.instance.getDataAtCell(row, col);

    $(ht.instance.getCell(row, col)).css(
        {
            "color": font_color,
            "font-size": "14px",
            "background-color": fondo,
            "vertical-align": "middle",
            "font-weight": "bold"
        })

}

function render_total_declarado(ht, row, col) {
    fondo = "white"
    font_color = "#2E8B57";
    value = ht.instance.getDataAtCell(row, col);//ht.instance.plugin.helper.cellValue(getExcelColumnName(columnas[0].declarado) + (row + 1).toString());
    eserror = false;
    indexPto = 0;
    limiteInf = 0;
    limiteSup = 0;
    celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
    td = ht.instance.getCell(row, col);

    if (value != null) {
        var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
        if (Number(value)) {
            indexPto = (row - nfilConsumo - 5) * 2;
            limiteInf = listaPtosStock[indexPto].Hojaptoliminf;
            limiteSup = listaPtosStock[indexPto].Hojaptolimsup;

            // indexPto = indexOfPto(ptoSeleccionado);
            if (Number(value) < limiteInf) {
                td.style.background = errores[errorLimInferior].BackgroundColor;
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                agregarError(celda, value, errorLimInferior);
                eserror = true;
            }
            else {
                eliminarError(celda, errorLimInferior);
                if (Number(value) > limiteSup) {
                    td.style.background = errores[errorLimSuperior].BackgroundColor;
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, errorLimSuperior);
                    eserror = true;
                }
                else {
                    eliminarError(celda, errorLimSuperior);
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            eliminarError(celda, errorLimInferior);
            if (value == "0")
                $(td).html("0.000");
            else if (value != "") {
                if (isNaN(Number(value))) {
                    td.style.background = errores[errorNoNumero].BackgroundColor;
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, errorNoNumero);
                    eserror = true;
                }
            }
        }
    }

    if (value == null) {
        $(td).html("0.000");
    }


    if (!eserror) {
        $(ht.instance.getCell(row, col)).css(
            {
                "color": font_color,
                "font-size": "14px",
                "background-color": fondo,
                "font-weight": "bold"
            });
    }
}

function render_merge(ht, row, col) {
    fondo = "#E0FFFF"
    font_color = "MidnightBlue";
    value = ht.instance.getDataAtCell(row, col);
    $(ht.instance.getCell(row, col)).css(
        {
            "color": font_color,
            "font-size": "12px",
            "background-color": fondo,
            "vertical-align": "middle"
            //"readOnly": true

        })

}

function render_celda_error(ht, row, col) {
    fondoLimSup = "yellow";
    fondoLimInf = "orange";
    fondo = "white"
    cell_color = "#000";
    font_color = "#fff";

    value = ht.instance.getDataAtCell(row, col);
    ptoSeleccionado = ht.instance.getDataAtCell(row, 0);
    if (ptoSeleccionado == null)
        return;
    if (ptoSeleccionado == '')
        return;
    td = ht.instance.getCell(row, col);
    ///////
    var columnaPtos = ht.instance.getDataAtCol(0);

    if (value != null) {
        var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
        if (!isNaN(Number(value))) {
            indexPto = indexOfPto(ptoSeleccionado);
            if ((Number(value) < listaPtos[indexPto].Hojaptoliminf)) {
                console.log("Menor " + value);
                td.style.background = errores[errorLimInferior].BackgroundColor;
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                eliminarError(celda, errorLimInferior);
                agregarError(celda, value, errorLimInferior);
            }
            else {
                eliminarError(celda, errorLimInferior);
                if (Number(value) > listaPtos[indexPto].Hojaptolimsup) {
                    td.style.background = errores[errorLimSuperior].BackgroundColor;
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, errorLimSuperior);
                }
                else {
                    eliminarError(celda, errorLimSuperior);
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            eliminarError(celda, errorNoNumero);
            if (value == "0")
                $(td).html("0.000");
            else if (value != "") {
                if (isNaN(Number(value))) {
                    td.style.background = errores[errorNoNumero].BackgroundColor;
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, errorNoNumero);
                }
            }
        }
    }
}

function render_celda_time(ht, row, col) {
    value = ht.instance.getDataAtCell(row, col);
    fecha = ht.instance.getDataAtCell(row, 1);
    td = ht.instance.getCell(row, col);
    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
    if (validaTime(value)) {
        eliminarError(celda, errorTime);
        if (validaExtremoTime(value, fecha)) {
            eliminarError(celda, errorExtremoTime);
            td.style.background = 'white';
        }
        else {
            agregarError(celda, value, errorExtremoTime);
            td.style.background = errores[errorExtremoTime].BackgroundColor;
        }
    }
    else {
        agregarError(celda, value, errorTime);
    }

}

function render_vertical_align(ht, row, col) {
    $(ht.instance.getCell(row, col)).css(
        {
            "vertical-align": "middle"
        });
}

function render_readonly(ht, row, col) {
    fondo = "Silver"
    font_color = "DimGray";

    value = ht.instance.getDataAtCell(row, col);

    $(ht.instance.getCell(row, col)).css(
        {
            "color": font_color,
            "background-color": fondo,
            "vertical-align": "middle"
        })

}

function render_celda_time_quema(ht, row, col) {
    value = ht.instance.getDataAtCell(row, col);
    fecha = ht.instance.getDataAtCell(row, 1);
    td = ht.instance.getCell(row, col);
    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
    if (validaTime(value)) {
        eliminarError(celda, errorTime);
        td.style.background = 'white';
    }
    else {
        agregarError(celda, value, errorTime);
    }
}

