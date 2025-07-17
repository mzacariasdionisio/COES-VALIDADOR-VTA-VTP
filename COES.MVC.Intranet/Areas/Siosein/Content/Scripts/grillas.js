/// Crea El objeto Handson en la pagina web
var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

var container;
var previoChange = 0;
var optionsListCentralesOsinerg;
var optionsListUnidadesOsinerg;
var listaunidadesTemp;
var listaunidades;
var unidadesLista = [];
var unidadesListaTemp = [];

function validaUnidadConValue(list_, valor) {
    var res = 0;
    if (valor.indexOf("+") > -1 || valor.indexOf("/") > -1) {
        var splitValor = valor.split('+');
        for (var a = 0; a < splitValor.length; a++) {
            splitValor[a] = splitValor[a].trim();
            if (list_.indexOf(splitValor[a].substring(splitValor[a].length - 1, splitValor[a].length)) > -1) {
                res++;
            }
        }
    } else {
        //if (list_.indexOf(valor.substring(valor.length - 1, valor.length)) > -1) {
        if (list_.trim().indexOf(valor.trim().replace('-', '')) > -1) {
            res = 1;
        }
    }
    return res;
}
function formatThousands(n, dp) {
    var s = '' + (Math.floor(n)), d = n % 1, i = s.length, r = '';
    while ((i -= 3) > 0) { r = ',' + s.substr(i, 3) + r; }
    return s.substr(0, i + 3) + r +
      (d ? '.' + Math.round(d * Math.pow(10, dp || 2)) : '');
};
function crearGrillaExcelPotenciaFirme(evt) {
    //fechaDia = $("#hfFecha").val();
    //modoLectura = evt.Handson.ReadOnly;
    var grillaBD = evt.Handson.ListaExcelData;
    optionsListCentralesOsinerg = evt.ListaCentralesOsinergCodi;
    optionsListUnidadesOsinerg = evt.ListaUnidadesOsinergCodi;
    unidadesLista = evt.ListaUnidadesOsinergCodi;

    nFilasFor2 = grillaBD.length - 2;

    container = document.getElementById('detalleFormato');

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4682B4';
        td.style.fontSize = '11px';
        td.style.color = '#FFFFFF'
    };

    var dropdownCentralRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var selectedId;
        var hecho = 0;
        var celda = getExcelColumnName(1).toString();
        var valor = parseInt(value);

        if (isNaN(valor)) {

            var dat = "";
            if (value != null) {
                if (value.trim() != "") {

                    var celda = getExcelColumnName(1).toString();
                    var insertado = 0;
                    for (var i = 0; i < optionsListCentralesOsinerg.length; i++) {


                        ////
                        if (insertado == 0) {



                            value = value.toUpperCase();
                            var nomCentral = (optionsListCentralesOsinerg[i].text).toUpperCase();
                            var nomCodigo = optionsListCentralesOsinerg[i].codigo;


                            if (value.indexOf("C.H. ") != -1) {
                                if (value.indexOf("C.H. ") == 0) {
                                    value = value.replace("C.H. ", "");
                                    hecho = 1;
                                }
                            }
                            if (value.indexOf("CH. ") != -1) {
                                if (value.indexOf("CH. ") == 0) {
                                    value = value.replace("CH. ", "");
                                    hecho = 1;
                                }
                            }
                            if (value.indexOf("CH ") != -1) {
                                if (value.indexOf("CH ") == 0) {
                                    value = value.replace("CH ", "");
                                    hecho = 1;
                                }
                            }
                            if (value.indexOf("CH-") != -1) {
                                if (value.indexOf("CH-") == 0) {
                                    value = value.replace("CH-", "");
                                    hecho = 1;
                                }
                            }
                            if (nomCentral.indexOf("C.H. ") != -1) {
                                if (nomCentral.indexOf("C.H. ") == 0) {
                                    nomCentral = nomCentral.replace("C.H. ", "");
                                    hecho = 1;
                                }
                            }
                            if (nomCentral.indexOf("C.H ") != -1) {
                                if (nomCentral.indexOf("C.H ") == 0) {
                                    nomCentral = nomCentral.replace("C.H ", "");
                                    hecho = 1;
                                }
                            }
                            if (nomCentral.indexOf("CH. ") != -1) {
                                if (nomCentral.indexOf("CH. ") == 0) {
                                    nomCentral = nomCentral.replace("CH. ", "");
                                    hecho = 1;
                                }
                            }
                            if (nomCentral.indexOf("CH ") != -1) {
                                if (nomCentral.indexOf("CH ") == 0) {
                                    nomCentral.replace("CH ", "");
                                    hecho = 1;
                                }
                            }
                            if (nomCentral.indexOf("CH-") != -1) {
                                if (nomCentral.indexOf("CH-") == 0) {
                                    nomCentral = nomCentral.replace("CH-", "");
                                    hecho = 1;
                                }
                            }
                            /*************TERMOELECTRICAS*************/
                            if (hecho == 0) {
                                if (value.indexOf("C.T. ") != -1) {
                                    if (value.indexOf("C.T. ") == 0) {
                                        value = value.replace("C.T. ", "");
                                    }
                                }
                                if (value.indexOf("CT. ") != -1) {
                                    if (value.indexOf("CT. ") == 0) {
                                        value = value.replace("CT. ", "");
                                    }
                                }
                                if (value.indexOf("CT ") != -1) {
                                    if (value.indexOf("CT ") == 0) {
                                        value = value.replace("CT ", "");
                                    }
                                }
                                if (value.indexOf("CT-") != -1) {
                                    if (value.indexOf("CT-") == 0) {
                                        value = value.replace("CT-", "");
                                    }
                                }
                                if (nomCentral.indexOf("C.T. ") != -1) {
                                    if (nomCentral.indexOf("C.T. ") == 0) {
                                        nomCentral = nomCentral.replace("C.T. ", "");
                                    }
                                }
                                if (nomCentral.indexOf("C.T ") != -1) {
                                    if (nomCentral.indexOf("C.T ") == 0) {
                                        nomCentral = nomCentral.replace("C.T ", "");
                                    }
                                }
                                if (nomCentral.indexOf("CT. ") != -1) {
                                    if (nomCentral.indexOf("CT. ") == 0) {
                                        nomCentral = nomCentral.replace("CT. ", "");
                                    }
                                }
                                if (nomCentral.indexOf("CT ") != -1) {
                                    if (nomCentral.indexOf("CT ") == 0) {
                                        nomCentral.replace("CT ", "");
                                    }
                                }
                                if (nomCentral.indexOf("CT-") != -1) {
                                    if (nomCentral.indexOf("CT-") == 0) {
                                        nomCentral = nomCentral.replace("CT-", "");
                                    }
                                }
                            }
                            /************************************************/
                            /*************RER, COGENERACION, NO INTEGRANTES*************/
                            if (hecho == 0) {
                                if (value.indexOf("C.E. ") != -1) {
                                    if (value.indexOf("C.E. ") == 0) {
                                        value = value.replace("C.E. ", "");
                                    }
                                }
                                if (value.indexOf("CE. ") != -1) {
                                    if (value.indexOf("CE. ") == 0) {
                                        value = value.replace("CE. ", "");
                                    }
                                }
                                if (value.indexOf("CE ") != -1) {
                                    if (value.indexOf("CE ") == 0) {
                                        value = value.replace("CE ", "");
                                    }
                                }
                                if (value.indexOf("CE-") != -1) {
                                    if (value.indexOf("CE-") == 0) {
                                        value = value.replace("CE-", "");
                                    }
                                }

                                if (nomCentral.indexOf("C.E. ") != -1) {
                                    if (nomCentral.indexOf("C.E. ") == 0) {
                                        nomCentral = nomCentral.replace("C.E. ", "");
                                    }
                                }
                                if (nomCentral.indexOf("C.E ") != -1) {
                                    if (nomCentral.indexOf("C.E ") == 0) {
                                        nomCentral = nomCentral.replace("C.E ", "");
                                    }
                                }
                                if (nomCentral.indexOf("CE. ") != -1) {
                                    if (nomCentral.indexOf("CE. ") == 0) {
                                        nomCentral = nomCentral.replace("CE. ", "");
                                    }
                                }
                                if (nomCentral.indexOf("CE ") != -1) {
                                    if (nomCentral.indexOf("CE ") == 0) {
                                        nomCentral.replace("CE ", "");
                                    }
                                }
                                if (nomCentral.indexOf("CE-") != -1) {
                                    if (nomCentral.indexOf("CE-") == 0) {
                                        nomCentral = nomCentral.replace("CE-", "");
                                    }
                                }
                            }
                            if (hecho == 0) {
                                if (value.indexOf("C.S. ") != -1) {
                                    if (value.indexOf("C.S. ") == 0) {
                                        value = value.replace("C.S. ", "");
                                    }
                                }
                                if (value.indexOf("CS. ") != -1) {
                                    if (value.indexOf("CS. ") == 0) {
                                        value = value.replace("CS. ", "");
                                    }
                                }
                                if (value.indexOf("CS ") != -1) {
                                    if (value.indexOf("CS ") == 0) {
                                        value = value.replace("CS ", "");
                                    }
                                }
                                if (value.indexOf("CS-") != -1) {
                                    if (value.indexOf("CS-") == 0) {
                                        value = value.replace("CS-", "");
                                    }
                                }

                                if (nomCentral.indexOf("C.S. ") != -1) {
                                    if (nomCentral.indexOf("C.S. ") == 0) {
                                        nomCentral = nomCentral.replace("C.S. ", "");
                                    }
                                }
                                if (nomCentral.indexOf("C.S ") != -1) {
                                    if (nomCentral.indexOf("C.S ") == 0) {
                                        nomCentral = nomCentral.replace("C.S ", "");
                                    }
                                }
                                if (nomCentral.indexOf("CS. ") != -1) {
                                    if (nomCentral.indexOf("CS. ") == 0) {
                                        nomCentral = nomCentral.replace("CS. ", "");
                                    }
                                }
                                if (nomCentral.indexOf("CS ") != -1) {
                                    if (nomCentral.indexOf("CS ") == 0) {
                                        nomCentral.replace("CS ", "");
                                    }
                                }
                                if (nomCentral.indexOf("CS-") != -1) {
                                    if (nomCentral.indexOf("CS-") == 0) {
                                        nomCentral = nomCentral.replace("CS-", "");
                                    }
                                }
                            }
                            /************************************************/
                            if (value.trim() === nomCentral.trim()) {
                                selectedId = optionsListCentralesOsinerg[i].id;
                                value = optionsListCentralesOsinerg[i].text;
                                dat = optionsListCentralesOsinerg[i].id;
                                // $('#selectedId').text(value);
                                evt.Handson.ListaExcelData[row]["0"] = nomCodigo;
                                evt.Handson.ListaExcelData[row]["1"] = selectedId;
                                grillaBD = evt.Handson.ListaExcelData;
                                insertado = 1;
                                break;
                            }

                        }
                        /////
                    }

                    eliminarError(celda + (row + 1), errorNoOsinergCodi);
                    if (dat == "") {
                        td.style.color = errores[errorNoOsinergCodi].Color;
                        td.style.background = errores[errorNoOsinergCodi].BackgroundColor;
                        agregarError(celda + (row + 1), value, errorNoOsinergCodi);
                    }
                    $(td).addClass("estilocombo");
                    $('#selectedId').text(selectedId);
                    Handsontable.TextCell.renderer.apply(this, arguments);
                    $('#selectedId').text(selectedId);



                }
            }


        } else {
            for (var index = 0; index < optionsListCentralesOsinerg.length; index++) {
                if (parseInt(value) === optionsListCentralesOsinerg[index].id) {
                    selectedId = optionsListCentralesOsinerg[index].id;
                    value = optionsListCentralesOsinerg[index].text;
                    dat = optionsListCentralesOsinerg[index].id;

                    evt.Handson.ListaExcelData[row]["0"] = optionsListCentralesOsinerg[index].codigo;
                    evt.Handson.ListaExcelData[row]["1"] = selectedId;
                    grillaBD = evt.Handson.ListaExcelData;
                    break;
                }
            }
            eliminarError(celda + (row + 1), errorNoOsinergCodi);
            if (dat == "") {
                td.style.color = errores[errorNoOsinergCodi].Color;
                td.style.background = errores[errorNoOsinergCodi].BackgroundColor;
                agregarError(celda + (row + 1), value, errorNoOsinergCodi);
            }

            $(td).addClass("estilocombo");
            Handsontable.TextCell.renderer.apply(this, arguments);
            $('#selectedId').text(selectedId);
            //$('#selectedId').text(dat);
        }

        /******************/
    }

    var dropdownUnidadRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var selectedId;
        var hecho = 0;
        var celda = getExcelColumnName(1).toString();
        var valor = parseInt(value);
        var codigoosi = hot.getData()[row][0];
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'ObtenerUnidades',
            data: { osicodi: codigoosi },
            success: function (aData) {
                var filterlist = [];

                /*var activeEditor = instance.getActiveEditor();*/
                //  var activeEditor2 = hot.getCellMeta(row, 2);


                for (var index = 0; index < aData.length; index++) {

                    filterlist.push(aData[index]);

                }

                unidadesListaTemp = filterlist;
                /* activeEditor.options.data = filterlist;*/
                /*hot.getCellMeta(row, 2).data = unidadesLista;
                hot.getCellMeta(row, 2).select2Options.data = filterlist;
                hot.getDataAtCell(row, 2, filterlist);

                cellProperties.select2Options.data = filterlist

                 Handsontable.editors.TextEditor.apply(this, arguments);
                 Handsontable.editors.DropdownEditor.apply(this, arguments);
                 Handsontable.renderers.AutocompleteRenderer.apply(this, arguments);*/

                /*hot.render();*/
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });


        listaunidadesTemp = unidadesListaTemp;

        if (isNaN(valor)) {

            var dat = "";
            if (value != null) {
                if (value.trim() != "") {

                    var celda = getExcelColumnName(1).toString();

                    for (var i = 0; i < listaunidadesTemp.length; i++) {
                        value = value.toUpperCase();
                        var nomCentral = (listaunidadesTemp[i].text).toUpperCase();


                        if (value.trim() === nomCentral.trim()) {
                            selectedId = listaunidadesTemp[i].id;
                            value = listaunidadesTemp[i].text;
                            dat = listaunidadesTemp[i].id;
                            // $('#selectedId').text(value);
                            evt.Handson.ListaExcelData[row]["2"] = selectedId;



                            grillaBD = evt.Handson.ListaExcelData;
                            break;
                        }
                    }

                    eliminarError(celda + (row + 1), errorNoOsinergCodi);
                    if (dat == "") {
                        td.style.color = errores[errorNoOsinergCodi].Color;
                        td.style.background = errores[errorNoOsinergCodi].BackgroundColor;
                        agregarError(celda + (row + 1), value, errorNoOsinergCodi);
                    }
                    $(td).addClass("estilocombo");
                    $('#selectedId').text(selectedId);
                    Handsontable.TextCell.renderer.apply(this, arguments);
                    $('#selectedId').text(selectedId);



                }
            }


        } else {
            for (var index = 0; index < listaunidadesTemp.length; index++) {
                if (parseInt(value) === listaunidadesTemp[index].id) {
                    selectedId = listaunidadesTemp[index].id;
                    value = listaunidadesTemp[index].text;
                    dat = listaunidadesTemp[index].id;

                    evt.Handson.ListaExcelData[row]["2"] = selectedId;



                    grillaBD = evt.Handson.ListaExcelData;
                    break;
                }
            }
            eliminarError(celda + (row + 1), errorNoOsinergCodi);
            if (dat == "") {
                td.style.color = errores[errorNoOsinergCodi].Color;
                td.style.background = errores[errorNoOsinergCodi].BackgroundColor;
                agregarError(celda + (row + 1), value, errorNoOsinergCodi);
            }

            $(td).addClass("estilocombo");
            Handsontable.TextCell.renderer.apply(this, arguments);
            $('#selectedId').text(selectedId);
            //$('#selectedId').text(dat);
        }
        //  Handsontable.TextCell.editor.apply(this, arguments);

        //   Handsontable.TextCell.renderer.apply(this, arguments);
    }

    function customDropdownRenderer(instance, td, row, col, prop, value, cellProperties) {

        if (instance.getCell(row, col)) {
            $(instance.getCell(row, col)).addClass('estilocombo');
        }

        var selectedId;



        var codigoosi = hot.getData()[row][0];
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'ObtenerUnidades',
            data: { osicodi: codigoosi },
            success: function (aData) {
                var filterlist = [];


                var activeEditor2 = hot.getCellMeta(row, 2);

                for (var index = 0; index < aData.length; index++) {

                    filterlist.push(aData[index]);

                }

                unidadesListaTemp = filterlist;
                hot.getCellMeta(row, 2).data = filterlist;/**/
                hot.getCellMeta(row, 2).select2Options.data = filterlist;
                /*hot.getDataAtCell(row, 2, filterlist);*/

                cellProperties.select2Options.data = filterlist;


            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });




        var colOptions = cellProperties.select2Options.data;


        for (var index = 0; index < colOptions.length; index++) {

            if (parseInt(value) === parseInt(colOptions[index].id)) {
                selectedId = colOptions[index].id;
                value = colOptions[index].text;

                break;
            }
        }

        Handsontable.TextCell.renderer.apply(this, arguments);
        Handsontable.AutocompleteCell.renderer.apply(this, arguments);
        /*Handsontable.AutocompleteCell.editor.apply(this, arguments);*/

        /*Handsontable.TextCell.renderer.apply(this, arguments);*/
        /*Handsontable.TextCell.editor.apply(this, arguments);*/
    }

    function unidades(instance, row) {

        var listaunidadesTemp0 = evt.ListaUnidadesOsinergCodi;

        var dao = grillaBD;

        var codigoosi = dao[row]["0"];
        $.ajax({
            type: 'POST',
            async: false,
            url: controlador + 'ObtenerUnidades',
            data: { osicodi: codigoosi },
            success: function (aData) {
                var filterlist = [];

                for (var index = 0; index < aData.length; index++) {

                    filterlist.push(aData[index]);

                }

                unidadesListaTemp0 = filterlist;
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
        /*  listaunidadesTemp = listaunidades;*/
        /**/
        return { results: unidadesListaTemp0 };
    };


    /*  function getQuery(options) {
          options.callback({ results: unidades(this) });
      };*/
    var flagRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var currencyCode = value;
        //alert(currencyCode);
        //while (td.firstChild) {
        //    td.removeChild(td.firstChild);
        //}
        //alert(currencyCodes.indexOf(currencyCode));
        /*
        if (currencyCodes.indexOf(currencyCode) > -1) {
            var flagElement = document.createElement('DIV');
            flagElement.className = 'flag ' + currencyCode.toLowerCase();
            td.appendChild(flagElement);
            alert(1);
        } else {
            alert(2);
            var textNode = document.createTextNode(value === null ? '' : value);
            td.appendChild(textNode);
        }*/
    };

    var validarUnidad = function (instance, td, row, col, prop, value, cellProperties) {
        if (value != null) {
            if (value.trim() != "") {
                var result = 0, tot = 0;
                var celda = getExcelColumnName(3).toString();
                //alert(hot.getDataAtCell(1, 0));
                for (var i = 0; i < optionsListUnidadesOsinerg.length; i++) {
                    if (hot.getData()[row][0] == optionsListUnidadesOsinerg[i].codigo) {
                        result = validaUnidadConValue(optionsListUnidadesOsinerg[i].text, value);
                        if (result > 0) {
                            tot++;
                        }
                    }
                }
                if (tot > 0) {
                    eliminarError(celda + (row + 1), errorNoUnidadOsinergCodi);
                } else {
                    td.style.color = errores[errorNoUnidadOsinergCodi].Color;
                    td.style.background = errores[errorNoUnidadOsinergCodi].BackgroundColor;
                    agregarError(celda + (row + 1), value, errorNoUnidadOsinergCodi);
                }
            }
        }
        Handsontable.renderers.TextRenderer.apply(this, arguments);
    };

    var validarOsinergCodi = function (instance, td, row, col, prop, value, cellProperties) {
        var dat = "";
        if (value != null) {
            if (value.trim() != "") {
                var celda = getExcelColumnName(1).toString();
                for (var i = 0; i < optionsListCentralesOsinerg.length; i++) {
                    value = value.toUpperCase();
                    if (value == optionsListCentralesOsinerg[i].codigo) {
                        dat = optionsListCentralesOsinerg[i].codigo;
                        break;
                    }
                }
                eliminarError(celda + (row + 1), errorNoOsinergCodi);
                if (dat == "") {
                    td.style.color = errores[errorNoOsinergCodi].Color;
                    td.style.background = errores[errorNoOsinergCodi].BackgroundColor;
                    agregarError(celda + (row + 1), value, errorNoOsinergCodi);
                }
            }
        }
        Handsontable.renderers.TextRenderer.apply(this, arguments);
    };

    var validarMonto = function (instance, td, row, col, prop, value, cellProperties) {
        var returnTipError;
        if (value != "") {
            var celda = getExcelColumnName(4).toString();
            returnTipError = getTipoError(value, 0, 0);
            eliminarError(celda + (row + 1), errorNoNumero);

            if (returnTipError == 3 || returnTipError == 2) {
                td.style.color = errores[errorNoNumero].Color;
                td.style.backgroundColor = errores[errorNoNumero].BackgroundColor;
                agregarError(celda + (row + 1), value, errorNoNumero);
            }
        }
        if (!isNaN(value)) { value = formatThousands(value); }

        if (value.trim() == '-') { value = value.trim().replace('-', '0'); eliminarError(celda + (row + 1), errorNoNumero); evt.Handson.ListaExcelData[row][col] = value; td.style.color = 'black'; }
        if (value.trim() == 0) { td.style.backgroundColor = 'white'; td.style.background = 'white'; }
        Handsontable.TextCell.renderer.apply(this, arguments);
        //Handsontable.renderers.TextRenderer.apply(this, arguments);
    };

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
        maxCols: 5,
        width: 1200,
        height: 3000,
        //width: evt.Handson.Width,
        colHeaders: true,
        rowHeaders: true,
        hiddenColumns: {
            columns: [4], indicators: true
        },
        //stretchH: 'all',
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        colWidths: evt.Handson.ListaColWidth,
        //readOnly: modoLectura,
        contextMenu: [/*'row_below', */'remove_row'],
        afterGetColHeader: function (col, th) {
            //if (col > -1) {
            //currentCoulmn = grillaBD.headers[col];
            /*if (hiddenColumns.indexOf(currentCoulmn.fieldName) > -1) {
                th.style.display = 'none';
            }*/
            //alert(currentCoulmn);
            //}
            //th.style.display = 'none';
        },
        columns: [
            {},
            {},
            {
                /*  data: unidadesLista,
                  editor: 'select2',
                  renderer: customDropdownRenderer,
                  select2Options: {
                      data: [],
                      dropdownAutoWidth: true,
                  }*/
            },
            {},
            {}],
        // afterChange: cambio,
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            if (row > 0) {
                switch (col) {
                    case 0:
                        //render_vertical_align(this, row, col);
                        //cellProperties.editor = 'select2';
                        // cellProperties.renderer = validarOsinergCodi;
                        //cellProperties.width = "200px";
                        //cellProperties.select2Options = {
                        //    data: optionsListEmp,
                        //    dropdownAutoWidth: true,
                        //    width: 'resolve',
                        //    allowClear: false,
                        //};
                        //cellProperties.readOnly = (evt.Handson.ReadOnly) ? true : false;
                        break;
                    case 1:
                        //render_readonly(this, row, col);
                        //cellProperties.readOnly = (evt.Handson.ReadOnly) ? true : false;
                        /*if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }*/
                        render_vertical_align(this, row, col);
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownCentralRenderer;
                        cellProperties.width = "200px";
                        cellProperties.select2Options = {
                            data: optionsListCentralesOsinerg,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false
                            //id: 'central'+row
                        };
                        break;
                    case 2:
                        //render_readonly(this, row, col);
                        //  render_vertical_align(this, row, col);
                        //cellProperties.readOnly = (evt.Handson.ReadOnly) ? true : false;
                        //   cellProperties.renderer = validarUnidad;

                        /*  if (evt.Handson.ReadOnly) {
                              render_readonly(this, row, col);
                          }*/
                        render_vertical_align(this, row, col);
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownUnidadRenderer;
                        cellProperties.width = "200px";
                        //cellProperties.data = unidadesLista;
                        cellProperties.select2Options = {
                            // data: [],
                            // data: unidades,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false
                            //id: 'drop' + row
                            , query: function (query) {
                                var data = { results: [] }, i, j, s;
                                var dao = grillaBD;
                                var codigoosi = dao[row]["0"];
                                $.ajax({
                                    type: 'POST',
                                    async: false,
                                    url: controlador + 'ObtenerUnidades',
                                    data: { osicodi: codigoosi },
                                    success: function (aData) {
                                        for (var index = 0; index < aData.length; index++) {
                                            data.results.push({ id: aData[index].id, text: aData[index].text });
                                        }
                                    },
                                    error: function () {
                                        alert("Ha ocurrido un error");
                                    }
                                });

                                query.callback(data);
                            }
                        };



                        break;
                    case 3:
                        cellProperties.format = '0,000.000';
                        cellProperties.type = 'numeric';
                        //render_vertical_align(this, row, col);
                        //cellProperties.readOnly = (evt.Handson.ReadOnly) ? true : false;
                        cellProperties.className = "htRight";
                        cellProperties.renderer = validarMonto;
                        break;
                    case 4:
                        //cellProperties.renderer = puntosmedicion;
                        //cellProperties.type = { renderer: hiddenRowRender };
                        break;
                }
            }

            return cellProperties;
        }
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
                /*"row_below": {
                    name: 'Agregar Registro',
                    disabled: function () {
                        // if first row, disable this option
                        return ((hot.getDataAtCol(1).length - nFilasFor2 - 1) == optionsListEmp.length)
                    }
                },*/
                "remove_row": {
                    name: 'Borrar Registro?',
                    disabled: function () {
                        // if first row, disable this option
                        return hot.getSelected()[0] <= nFilasFor2
                    }
                }
            }
        }
    });
}
function excelWebPotenciaFirme(evt, heightHansonTab) {
    var grillaBD = evt.Handson.ListaExcelData;
    var mergeBD = evt.Handson.ListaMerge;
    container = document.getElementById('detalleFormato');
    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#16365C';
        td.style.fontSize = '11px';
        td.style.color = '#FFFFFF';
        td.style.verticalAlign = 'middle';
    };
    var LateralIzq = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#DCE6F1';
        td.style.fontSize = '11px';
        td.style.color = 'black';
    };
    var ColorEmpresa = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4F81BD';
        td.style.fontSize = '11px';
        td.style.color = 'white'
    };
    var ColorContent = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#D9D9D9';
        td.style.fontSize = '11px';
        td.style.color = 'black'
    };
    var validarHora = function (instance, td, row, col, prop, value, cellProperties) {
        var valido;
        var noNumero;
        if (value != "") {
            var celda = getExcelColumnName(col + 1).toString();
            valido = validaTimeShort(value);
            eliminarError(celda + (row + 1), errorTime);
            if (valido == false) {
                var resto = row % 3;
                if (resto != 0) {
                    td.style.color = errores[errorTime].Color;
                    td.style.background = errores[errorTime].BackgroundColor;
                    agregarError(celda + (row + 1), value, errorTime);
                } else {
                    noNumeroEntero = getTipoError(value, 0, 0);
                    if (noNumeroEntero == 3 || noNumeroEntero == 2) {
                        td.style.color = errores[errorNoNumero].Color;
                        td.style.background = errores[errorNoNumero].BackgroundColor;
                        agregarError(celda + (row + 1), value, errorNoNumero);
                    } else {
                        //calculo resta de horas en minutos
                        var tiempo;
                        var horaIni = instance.getDataAtCell(row - 2, prop);
                        var horaFin = instance.getDataAtCell(row - 1, prop);
                        tiempo = restarHoras(horaIni, horaFin);
                        arguments[5] = tiempo;
                    }
                }
            }
            if (hot.getData()[row][5] === 'Hora Inicio') {
                var horaIni1 = instance.getDataAtCell(row, prop);
                var horaFin1 = instance.getDataAtCell(row + 1, prop);
                var sa1 = horaIni1.split(":");
                var sa2 = horaFin1.split(":");
                t11 = new Date();
                t11.setHours(sa1[0], sa1[1]);
                t22 = new Date();
                t22.setHours(sa2[0], sa2[1]);
                var timpoini = t11.getHours() * 60 + t11.getMinutes();
                var timpofin = t22.getHours() * 60 + t22.getMinutes();
                if (timpofin < timpoini) {
                    td.style.color = errores[errorHoraIniMayFin].Color;
                    td.style.background = errores[errorHoraIniMayFin].BackgroundColor;
                    agregarError(celda + (row + 1), value, errorHoraIniMayFin);
                }
            } else if (hot.getData()[row][5] === 'Hora Fin') {
                var hora = instance.getDataAtCell(row, prop);
                var sa = hora.split(":");
                t11 = new Date();
                t11.setHours(sa[0], sa[1]);
                var timpofin = t11.getHours() * 60 + t11.getMinutes();
            }
        } else {
            //calculo para nuevos registro
            if (row >= 3) {
                var horafin = instance.getDataAtCell(row - 1, prop);
                var horaini = instance.getDataAtCell(row - 2, prop);
                var tiempo = 0;
                var res = row % 3
                if (res == 0) {
                    if (horaini != "" || horafin != "") {
                        if (horaini != "" && horafin == "") {
                            arguments[5] = 0;
                        } else if (horafin != 0 && horaini == "") {
                            arguments[5] = 0;
                        } else if (horafin != 0 && horaini != "") {
                            tiempo = restarHoras(horaini, horafin);
                            arguments[5] = tiempo;
                        }
                    }
                }
            }
        }
        Handsontable.TextCell.renderer.apply(this, arguments);
    };
    var validarPeTermo = function (instance, td, row, col, prop, value, cellProperties) {
        var result_ = 0;
        if (value == "") {
            if (col == 4 && hot.getData()[row][2] != "" && hot.getData()[row][5] == "") {
                arguments[5] = 0;
            }
        }
        Handsontable.TextCell.renderer.apply(this, arguments);
    };
    var validarPeHidro = function (instance, td, row, col, prop, value, cellProperties) {
        var result_ = 0;
        if (value == "") {
            if (col == 6 && hot.getData()[row][5] != "") {
                arguments[5] = 0;
            }
        }
        Handsontable.TextCell.renderer.apply(this, arguments);
    };

    hotOptions = {
        data: grillaBD,
        height: heightHansonTab,
        colHeaders: true,
        rowHeaders: true,
        hiddenColumns: {
            //columns: [1],
            //indicators: true
        },
        fillHandle: true,
        columnSorting: false,
        colWidths: evt.Handson.ListaColWidth,
        fixedRowsTop: 1,
        fixedColumnsLeft: 3,
        afterChange: changesCalculo,
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            if (col == 0 && row > 0) {
                cellProperties.renderer = ColorEmpresa;
            }
            if (col > 0 && col < 3 && row > 0) {
                cellProperties.renderer = LateralIzq;
            }
            if ((col == 3 || col == 5 || col == 7) && row > 0) {
                cellProperties.readOnly = true;
                cellProperties.type = 'numeric';
                cellProperties.format = "#.00";
            }
            if (col == 4 && row > 0) {
                if (grillaBD[row][2] == null) {
                    cellProperties.readOnly = true;
                }
            }
            if (col == 6 && row > 0) {
                if (grillaBD[row][2] != null) {
                    cellProperties.readOnly = true;
                }
            }
            if (row > 0) {
                switch (col) {
                    case 0: cellProperties.className = "htLeft htMiddle"; cellProperties.readOnly = true; break;
                    case 1: cellProperties.className = "htLeft htMiddle"; cellProperties.readOnly = true; break;
                    case 2: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 4: cellProperties.className = "htRight htMiddle"; cellProperties.renderer = validarPeTermo, render_ColorResult(this, row, col, 1, 1); break;
                    case 6: cellProperties.className = "htRight htMiddle"; cellProperties.renderer = validarPeHidro, render_ColorResult(this, row, col, 2, 1); break;
                }
            }
            return cellProperties;
        },
        mergeCells: mergeBD
    };
    hot = new Handsontable(container, hotOptions);
    //hot.render();
}

function changesCalculo(changes, source) {
    if (!changes) {
        return;
    }
    var instance = this;
    var result = 0;
    changes.forEach(function (change) {
        row = change[0];
        col = change[1];
        newValue = change[3];
        if (col == 4 && hot.getData()[row][2] != "") {
            if (hot.getData()[row][3] != null) {
                if (newValue == "") { newValue = "0"; }
                if (newValue != "") {
                    var pe_ = hot.getData()[row][3];
                    result = (1 - (newValue / 100)) * pe_;
                    instance.setDataAtCell(row, 7, result);
                }
            }
        }
        if (col == 6 && hot.getData()[row][5] != "") {
            if (hot.getData()[row][3] != null) {
                if (newValue == "") { newValue = "0"; }
                if (newValue != "") {
                    var pg_ = hot.getData()[row][5];
                    result = newValue * pg_;
                    instance.setDataAtCell(row, 7, result);
                }
            }
        }
    });
}

function render_ColorResult(ht, row, col, tipo, x) {
    if (tipo == 1) {
        if (col == 4 && row > 0) {
            if (ht.instance.getDataAtCell(row, col) == "") {
                $(ht.instance.getCell(row, col)).css(
                    {
                        "background-color": "#D9D9D9"
                    });
            }
        }
    }
    if (tipo == 2) {
        if (col == 6 && row > 0) {
            if (ht.instance.getDataAtCell(row, col) == "") {
                $(ht.instance.getCell(row, col)).css(
                    {
                        "background-color": "#D9D9D9"
                    });
            }
        }
    }
}

function crearGrillaExcelTablaPrie03(evt) {
    var grillaBD = evt.Handson.ListaExcelData;
    optionsListUnidadesOsinerg = evt.ListaUnidadesOsinergCodi;

    container = document.getElementById('detalleFormato');

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4682B4';
        td.style.fontSize = '11px';
        td.style.color = '#FFFFFF'
    };

    var validarOsinergCodi = function (instance, td, row, col, prop, value, cellProperties) {
        var dat = "";
        if (value != null) {
            if (value.trim() != "") {
                var celda = getExcelColumnName(1).toString();
                for (var i = 0; i < optionsListUnidadesOsinerg.length; i++) {
                    value = value.toUpperCase();
                    if (value == optionsListUnidadesOsinerg[i].codigo) {
                        dat = optionsListUnidadesOsinerg[i].codigo;
                        break;
                    }
                }
                eliminarError(celda + (row + 1), errorNoOsinergCodi);
                if (dat == "") {
                    td.style.color = errores[errorNoOsinergCodi].Color;
                    td.style.background = errores[errorNoOsinergCodi].BackgroundColor;
                    agregarError(celda + (row + 1), value, errorNoOsinergCodi);
                }
            }
        }
        Handsontable.renderers.TextRenderer.apply(this, arguments);
    };

    hotOptions = {
        data: grillaBD,
        maxCols: 32,
        width: 1200,
        height: 600,
        colHeaders: true,
        rowHeaders: true,
        //hiddenColumns: {columns: [4], indicators: true},
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        //colWidths: evt.Handson.ListaColWidth,
        //columns: [{}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}],
        //afterChange: changess,
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            if (row > 0) {
                switch (col) {
                    case 0:
                        //cellProperties.renderer = validarOsinergCodi;
                        break;
                    case 1:
                        break;
                    case 2:
                        //render_vertical_align(this, row, col);
                        //cellProperties.renderer = validarUnidad;
                        break;
                    case 3:
                        //cellProperties.format = '0,000.000';
                        //cellProperties.type = 'numeric';
                        //cellProperties.className = "htRight";
                        //cellProperties.renderer = validarMonto;
                        break;
                    case 4:
                        break;
                }
            }

            return cellProperties;
        }
    };
    hot = new Handsontable(container, hotOptions);
}
function crearGrillaExcelProgOperacionMensual(evt, tipo) {
    var grillaBD = evt.Handson.ListaExcelData;
    /* optionsListUnidadesOsinerg = evt.ListaUnidadesOsinergCodi;*/
    var Centrales = evt.CentralesLista;
    var nombre = 'detalleFormato' + tipo;
    nFilasFor2 = grillaBD.length - 1;
    container = document.getElementById(nombre);

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

    var activeStateRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.CheckboxCell.renderer.apply(this, arguments);

        /* if (value === true) {
             instance.rootElement.find('table').first().find('tbody tr').eq(row).find('td').css('background', '#d4e9c2');
         }
         */
        /*td.style.textAlign = 'center';*/

    }

    var dropdownCentralRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var selectedId;
        var hecho = 0;
        var celda = getExcelColumnName(1).toString();
        var valor = parseInt(value);

        if (isNaN(valor)) {

            var dat = "";
            if (value != null) {
                if (value.trim() != "") {

                    var celda = getExcelColumnName(1).toString();

                    for (var i = 0; i < Centrales.length; i++) {
                        value = value.toUpperCase();
                        var nomCentral = (Centrales[i].text).toUpperCase();


                        if (value.indexOf("C.H. ") != -1) {
                            if (value.indexOf("C.H. ") == 0) {
                                value = value.replace("C.H. ", "");
                                hecho = 1;
                            }
                        }
                        if (value.indexOf("CH. ") != -1) {
                            if (value.indexOf("CH. ") == 0) {
                                value = value.replace("CH. ", "");
                                hecho = 1;
                            }
                        }
                        if (value.indexOf("CH ") != -1) {
                            if (value.indexOf("CH ") == 0) {
                                value = value.replace("CH ", "");
                                hecho = 1;
                            }
                        }
                        if (value.indexOf("CH-") != -1) {
                            if (value.indexOf("CH-") == 0) {
                                value = value.replace("CH-", "");
                                hecho = 1;
                            }
                        }
                        if (nomCentral.indexOf("C.H. ") != -1) {
                            if (nomCentral.indexOf("C.H. ") == 0) {
                                nomCentral = nomCentral.replace("C.H. ", "");
                                hecho = 1;
                            }
                        }
                        if (nomCentral.indexOf("C.H ") != -1) {
                            if (nomCentral.indexOf("C.H ") == 0) {
                                nomCentral = nomCentral.replace("C.H ", "");
                                hecho = 1;
                            }
                        }
                        if (nomCentral.indexOf("CH. ") != -1) {
                            if (nomCentral.indexOf("CH. ") == 0) {
                                nomCentral = nomCentral.replace("CH. ", "");
                                hecho = 1;
                            }
                        }
                        if (nomCentral.indexOf("CH ") != -1) {
                            if (nomCentral.indexOf("CH ") == 0) {
                                nomCentral.replace("CH ", "");
                                hecho = 1;
                            }
                        }
                        if (nomCentral.indexOf("CH-") != -1) {
                            if (nomCentral.indexOf("CH-") == 0) {
                                nomCentral = nomCentral.replace("CH-", "");
                                hecho = 1;
                            }
                        }
                        /*************TERMOELECTRICAS*************/
                        if (hecho == 0) {
                            if (value.indexOf("C.T. ") != -1) {
                                if (value.indexOf("C.T. ") == 0) {
                                    value = value.replace("C.T. ", "");
                                }
                            }
                            if (value.indexOf("CT. ") != -1) {
                                if (value.indexOf("CT. ") == 0) {
                                    value = value.replace("CT. ", "");
                                }
                            }
                            if (value.indexOf("CT ") != -1) {
                                if (value.indexOf("CT ") == 0) {
                                    value = value.replace("CT ", "");
                                }
                            }
                            if (value.indexOf("CT-") != -1) {
                                if (value.indexOf("CT-") == 0) {
                                    value = value.replace("CT-", "");
                                }
                            }
                            if (nomCentral.indexOf("C.T. ") != -1) {
                                if (nomCentral.indexOf("C.T. ") == 0) {
                                    nomCentral = nomCentral.replace("C.T. ", "");
                                }
                            }
                            if (nomCentral.indexOf("C.T ") != -1) {
                                if (nomCentral.indexOf("C.T ") == 0) {
                                    nomCentral = nomCentral.replace("C.T ", "");
                                }
                            }
                            if (nomCentral.indexOf("CT. ") != -1) {
                                if (nomCentral.indexOf("CT. ") == 0) {
                                    nomCentral = nomCentral.replace("CT. ", "");
                                }
                            }
                            if (nomCentral.indexOf("CT ") != -1) {
                                if (nomCentral.indexOf("CT ") == 0) {
                                    nomCentral.replace("CT ", "");
                                }
                            }
                            if (nomCentral.indexOf("CT-") != -1) {
                                if (nomCentral.indexOf("CT-") == 0) {
                                    nomCentral = nomCentral.replace("CT-", "");
                                }
                            }
                        }
                        /************************************************/
                        /*************RER, COGENERACION, NO INTEGRANTES*************/
                        if (hecho == 0) {
                            if (value.indexOf("C.E. ") != -1) {
                                if (value.indexOf("C.E. ") == 0) {
                                    value = value.replace("C.E. ", "");
                                }
                            }
                            if (value.indexOf("CE. ") != -1) {
                                if (value.indexOf("CE. ") == 0) {
                                    value = value.replace("CE. ", "");
                                }
                            }
                            if (value.indexOf("CE ") != -1) {
                                if (value.indexOf("CE ") == 0) {
                                    value = value.replace("CE ", "");
                                }
                            }
                            if (value.indexOf("CE-") != -1) {
                                if (value.indexOf("CE-") == 0) {
                                    value = value.replace("CE-", "");
                                }
                            }

                            if (nomCentral.indexOf("C.E. ") != -1) {
                                if (nomCentral.indexOf("C.E. ") == 0) {
                                    nomCentral = nomCentral.replace("C.E. ", "");
                                }
                            }
                            if (nomCentral.indexOf("C.E ") != -1) {
                                if (nomCentral.indexOf("C.E ") == 0) {
                                    nomCentral = nomCentral.replace("C.E ", "");
                                }
                            }
                            if (nomCentral.indexOf("CE. ") != -1) {
                                if (nomCentral.indexOf("CE. ") == 0) {
                                    nomCentral = nomCentral.replace("CE. ", "");
                                }
                            }
                            if (nomCentral.indexOf("CE ") != -1) {
                                if (nomCentral.indexOf("CE ") == 0) {
                                    nomCentral.replace("CE ", "");
                                }
                            }
                            if (nomCentral.indexOf("CE-") != -1) {
                                if (nomCentral.indexOf("CE-") == 0) {
                                    nomCentral = nomCentral.replace("CE-", "");
                                }
                            }
                        }
                        if (hecho == 0) {
                            if (value.indexOf("C.S. ") != -1) {
                                if (value.indexOf("C.S. ") == 0) {
                                    value = value.replace("C.S. ", "");
                                }
                            }
                            if (value.indexOf("CS. ") != -1) {
                                if (value.indexOf("CS. ") == 0) {
                                    value = value.replace("CS. ", "");
                                }
                            }
                            if (value.indexOf("CS ") != -1) {
                                if (value.indexOf("CS ") == 0) {
                                    value = value.replace("CS ", "");
                                }
                            }
                            if (value.indexOf("CS-") != -1) {
                                if (value.indexOf("CS-") == 0) {
                                    value = value.replace("CS-", "");
                                }
                            }

                            if (nomCentral.indexOf("C.S. ") != -1) {
                                if (nomCentral.indexOf("C.S. ") == 0) {
                                    nomCentral = nomCentral.replace("C.S. ", "");
                                }
                            }
                            if (nomCentral.indexOf("C.S ") != -1) {
                                if (nomCentral.indexOf("C.S ") == 0) {
                                    nomCentral = nomCentral.replace("C.S ", "");
                                }
                            }
                            if (nomCentral.indexOf("CS. ") != -1) {
                                if (nomCentral.indexOf("CS. ") == 0) {
                                    nomCentral = nomCentral.replace("CS. ", "");
                                }
                            }
                            if (nomCentral.indexOf("CS ") != -1) {
                                if (nomCentral.indexOf("CS ") == 0) {
                                    nomCentral.replace("CS ", "");
                                }
                            }
                            if (nomCentral.indexOf("CS-") != -1) {
                                if (nomCentral.indexOf("CS-") == 0) {
                                    nomCentral = nomCentral.replace("CS-", "");
                                }
                            }
                        }
                        /************************************************/
                        if (value.trim() === nomCentral.trim()) {
                            selectedId = Centrales[i].id;
                            value = Centrales[i].text;
                            dat = Centrales[i].id;
                            // $('#selectedId').text(value);
                            evt.Handson.ListaExcelData[row]["1"] = selectedId;



                            grillaBD = evt.Handson.ListaExcelData;
                            break;
                        }
                    }
                    if (evt.Handson.ListaExcelData[row]["0"] == true) {
                        dat = "true";
                        td.style.color = errores[errorNoOsinergCodi].Color;
                        td.style.background = errores[errorNoOsinergCodi].BackgroundColor;
                    }




                    NeweliminarError(tipo, celda + (row + 1), errorNoOsinergCodi);
                    if (dat == "") {
                        td.style.color = errores[errorNoOsinergCodi].Color;
                        td.style.background = errores[errorNoOsinergCodi].BackgroundColor;
                        NewagregarError(tipo, celda + (row + 1), value, errorNoOsinergCodi);
                    }
                    $(td).addClass("estilocombo");
                    $('#selectedId').text(selectedId);
                    Handsontable.TextCell.renderer.apply(this, arguments);
                    $('#selectedId').text(selectedId);



                }
            }
            // if (selectedId == 195){alert(1);}
            // $(td).addClass("estilocombo");
            //  Handsontable.TextCell.renderer.apply(this, arguments);
            // if (selectedId == 195) { alert(2); }
            //$('#selectedId').text(selectedId);
            //$('#selectedId').text(dat);
            // if (dat == 195) { alert(3); }



        } else {
            for (var index = 0; index < Centrales.length; index++) {
                if (parseInt(value) === Centrales[index].id) {
                    selectedId = Centrales[index].id;
                    value = Centrales[index].text;
                    dat = Centrales[index].id;
                    break;
                }
            }
            NeweliminarError(tipo, celda + (row + 1), errorNoOsinergCodi);
            if (dat == "") {
                td.style.color = errores[errorNoOsinergCodi].Color;
                td.style.background = errores[errorNoOsinergCodi].BackgroundColor;
                NewagregarError(tipo, celda + (row + 1), value, errorNoOsinergCodi);
            }

            $(td).addClass("estilocombo");
            Handsontable.TextCell.renderer.apply(this, arguments);
            $('#selectedId').text(selectedId);
            //$('#selectedId').text(dat);
        }

        /******************/
    }


    var validarOsinergCodi = function (instance, td, row, col, prop, value, cellProperties) {
        var dat = "";
        if (value != null) {
            if (value.trim() != "") {
                var celda = getExcelColumnName(1).toString();
                for (var i = 0; i < optionsListUnidadesOsinerg.length; i++) {
                    value = value.toUpperCase();
                    if (value == optionsListUnidadesOsinerg[i].codigo) {
                        dat = optionsListUnidadesOsinerg[i].codigo;
                        break;
                    }
                }
                NeweliminarError(celda + (row + 1), errorNoOsinergCodi);
                if (dat == "") {
                    td.style.color = errores[errorNoOsinergCodi].Color;
                    td.style.background = errores[errorNoOsinergCodi].BackgroundColor;
                    agregarError(celda + (row + 1), value, errorNoOsinergCodi);
                }
            }
        }
        Handsontable.renderers.TextRenderer.apply(this, arguments);
    };

    var validarMonto = function (instance, td, row, col, prop, value, cellProperties) {
        var returnTipError;
        if (value != "") {
            var celda = getExcelColumnName(col + 1).toString();
            returnTipError = getTipoError(value, 0, 0);
            NeweliminarError(tipo, celda + (row + 1), errorNoNumero);

            if (returnTipError == 3 || returnTipError == 2) {
                td.style.color = errores[errorNoNumero].Color;
                td.style.background = errores[errorNoNumero].BackgroundColor;
                NewagregarError(tipo, celda + (row + 1), value, errorNoNumero);
            }
        }

        if (!isNaN(value)) { value = formatThousands(value); }
        Handsontable.TextCell.renderer.apply(this, arguments);
    };

    function delete_row_renderer(instance, td, row, col, prop, value, cellProperties) {
        var div;
        $(td).children('.btn').remove();
        div = document.createElement('div');
        div.className = 'btn';
        div.appendChild(document.createTextNode("."));
        td.appendChild(div);

        $(div).on('mouseup', function () {
            if (confirm("Desea eliminar?")) {
                NeweliminarFilaHandson(tipo);
                return instance.alter("remove_row", row);
            }
        });
        $(td).addClass("estilodelete");
        return td;
    }

    hotOptions = {
        data: grillaBD,
        //maxCols: 5,
        width: 1280,
        height: 1000,
        colHeaders: true,
        rowHeaders: true,
        hiddenColumns: {
            //columns: [4], indicators: true
        },
        //stretchH: 'all',
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        colWidths: evt.Handson.ListaColWidth,
        //readOnly: modoLectura,
        contextMenu: ['row_below', 'remove_row'],
        /* afterGetColHeader: function (col, th) {
 
         },*/
        columns: [
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {}

        ],
        // afterChange: changess,
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            if (row > 0) {
                switch (col) {
                    /* case 0:                        
                    //    cellProperties.type = 'checkbox';
                    //    cellProperties.checkedTemplate = true;
                    //    cellProperties.uncheckedTemplate = false;
                    //    break;*/
                    case 0:
                        /*cellProperties.className = "htLeft";
                        cellProperties.renderer = validarOsinergCodi;*/
                        if (evt.Handson.ReadOnly) {
                            render_readonly(this, row, col);
                        }
                        render_vertical_align(this, row, col);
                        cellProperties.editor = 'select2';
                        cellProperties.renderer = dropdownCentralRenderer;
                        cellProperties.width = "200px";
                        cellProperties.select2Options = {
                            data: Centrales,
                            dropdownAutoWidth: true,
                            width: 'resolve',
                            allowClear: false
                        };
                        break;
                    case 1: cellProperties.format = '0,000.000'; cellProperties.type = 'numeric'; cellProperties.renderer = validarMonto; break;
                    case 2: cellProperties.format = '0,000.000'; cellProperties.type = 'numeric'; cellProperties.renderer = validarMonto; break;
                    case 3: cellProperties.format = '0,000.000'; cellProperties.type = 'numeric'; cellProperties.renderer = validarMonto; break;
                    case 4: cellProperties.format = '0,000.000'; cellProperties.type = 'numeric'; cellProperties.renderer = validarMonto; break;
                    case 5: cellProperties.format = '0,000.000'; cellProperties.type = 'numeric'; cellProperties.renderer = validarMonto; break;
                    case 6: cellProperties.format = '0,000.000'; cellProperties.type = 'numeric'; cellProperties.renderer = validarMonto; break;
                    case 7: cellProperties.format = '0,000.000'; cellProperties.type = 'numeric'; cellProperties.renderer = validarMonto; break;
                    case 8: cellProperties.format = '0,000.000'; cellProperties.type = 'numeric'; cellProperties.renderer = validarMonto; break;
                    case 9: cellProperties.format = '0,000.000'; cellProperties.type = 'numeric'; cellProperties.renderer = validarMonto; break;
                    case 10: cellProperties.format = '0,000.000'; cellProperties.type = 'numeric'; cellProperties.renderer = validarMonto; break;
                    case 11: cellProperties.format = '0,000.000'; cellProperties.type = 'numeric'; cellProperties.renderer = validarMonto; break;
                    case 12: cellProperties.format = '0,000.000'; cellProperties.type = 'numeric'; cellProperties.renderer = validarMonto; break;
                }
            }

            return cellProperties;
        }
    };
    if (tipo == 1) {
        hot1 = new Handsontable(container, hotOptions);
        hot1.updateSettings({
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
                            return ((hot1.getDataAtCol(1).length - nFilasFor2 - 1) == Centrales.length)

                        }
                    },
                    "remove_row": {
                        name: 'Borrar Registro?',
                        disabled: function () {
                            // if first row, disable this option
                            return hot1.getSelected()[0] <= nFilasFor2
                        }
                    }
                }
            }
        })
    }
    if (tipo == 2) {
        hot2 = new Handsontable(container, hotOptions);
        hot2.updateSettings({
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
                            return ((hot2.getDataAtCol(1).length - nFilasFor2 - 1) == Centrales.length)

                        }
                    },
                    "remove_row": {
                        name: 'Borrar Registro?',
                        disabled: function () {
                            // if first row, disable this option
                            return hot2.getSelected()[0] <= nFilasFor2
                        }
                    }
                }
            }
        })
    }
    if (tipo == 3) {
        hot3 = new Handsontable(container, hotOptions);
        hot3.updateSettings({
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
                            return ((hot3.getDataAtCol(1).length - nFilasFor2 - 1) == Centrales.length)

                        }
                    },
                    "remove_row": {
                        name: 'Borrar Registro?',
                        disabled: function () {
                            // if first row, disable this option
                            return hot3.getSelected()[0] <= nFilasFor2
                        }
                    }
                }
            }
        })
    }

    /*  hot = new Handsontable(container, hotOptions);*/
}
function crearGrillaExcelProgOperacionCostosMarginalesMensual(evt) {
    var grillaBD = evt.Handson.ListaExcelData;
    optionsListUnidadesOsinerg = evt.ListaUnidadesOsinergCodi;

    container = document.getElementById('detalleFormato');

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4682B4';
        td.style.fontSize = '11px';
        td.style.color = '#FFFFFF'
    };

    var flagRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var currencyCode = value;
    };
    /*
    var validarOsinergCodi = function (instance, td, row, col, prop, value, cellProperties) {
        var dat = "";
        if (value != null) {
            if (value.trim() != "") {
                var celda = getExcelColumnName(1).toString();
                for (var i = 0; i < optionsListUnidadesOsinerg.length; i++) {
                    value = value.toUpperCase();
                    if (value == optionsListUnidadesOsinerg[i].codigo) {
                        dat = optionsListUnidadesOsinerg[i].codigo;
                        break;
                    }
                }
                eliminarError(celda + (row + 1), errorNoOsinergCodi);
                if (dat == "") {
                    td.style.color = errores[errorNoOsinergCodi].Color;
                    td.style.background = errores[errorNoOsinergCodi].BackgroundColor;
                    agregarError(celda + (row + 1), value, errorNoOsinergCodi);
                }
            }
        }
        Handsontable.renderers.TextRenderer.apply(this, arguments);
    };*/

    var validarMonto = function (instance, td, row, col, prop, value, cellProperties) {
        var returnTipError;
        if (value != "") {
            var celda = getExcelColumnName(4).toString();
            returnTipError = getTipoError(value, 0, 0);
            eliminarError(celda + (row + 1), errorNoNumero);

            if (returnTipError == 3 || returnTipError == 2) {
                td.style.color = errores[errorNoNumero].Color;
                td.style.background = errores[errorNoNumero].BackgroundColor;
                agregarError(celda + (row + 1), value, errorNoNumero);
            }
        }

        if (!isNaN(value)) { value = formatThousands(value); }
        Handsontable.TextCell.renderer.apply(this, arguments);
    };

    hotOptions = {
        data: grillaBD,
        //maxCols: 5,
        width: 1420,
        height: 1000,
        colHeaders: true,
        rowHeaders: true,
        hiddenColumns: {
            columns: [2]
        },
        //stretchH: 'all',
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        colWidths: evt.Handson.ListaColWidth,
        //readOnly: modoLectura,
        //contextMenu: ['row_below', 'remove_row'],
        afterGetColHeader: function (col, th) {

        },
        columns:
            [{}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}],
        afterChange: changess,
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            if (row > 0) {
                switch (col) {
                    case 0: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 1: cellProperties.className = "htLeft"; cellProperties.readOnly = true; break;
                    case 2: cellProperties.className = "htRight"; cellProperties.readOnly = true; break;
                    case 3: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 4: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 5: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 6: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 7: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 8: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 9: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 10: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 11: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 12: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 13: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 14: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                }
            }

            return cellProperties;
        },
        mergeCells: [
            { row: 1, col: 0, rowspan: 5, colspan: 1 },
            { row: 6, col: 0, rowspan: 5, colspan: 1 },
            { row: 11, col: 0, rowspan: 5, colspan: 1 }
        ]
    };
    hot = new Handsontable(container, hotOptions);
}
function crearGrillaExcelCostosOperacionProgMensual(evt) {
    var grillaBD = evt.Handson.ListaExcelData;
    optionsListUnidadesOsinerg = evt.ListaUnidadesOsinergCodi;

    container = document.getElementById('detalleFormato');

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4682B4';
        td.style.fontSize = '11px';
        td.style.color = '#FFFFFF'
    };

    var flagRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var currencyCode = value;
    };
    /*
    var validarOsinergCodi = function (instance, td, row, col, prop, value, cellProperties) {
        var dat = "";
        if (value != null) {
            if (value.trim() != "") {
                var celda = getExcelColumnName(1).toString();
                for (var i = 0; i < optionsListUnidadesOsinerg.length; i++) {
                    value = value.toUpperCase();
                    if (value == optionsListUnidadesOsinerg[i].codigo) {
                        dat = optionsListUnidadesOsinerg[i].codigo;
                        break;
                    }
                }
                eliminarError(celda + (row + 1), errorNoOsinergCodi);
                if (dat == "") {
                    td.style.color = errores[errorNoOsinergCodi].Color;
                    td.style.background = errores[errorNoOsinergCodi].BackgroundColor;
                    agregarError(celda + (row + 1), value, errorNoOsinergCodi);
                }
            }
        }
        Handsontable.renderers.TextRenderer.apply(this, arguments);
    };*/

    var validarMonto = function (instance, td, row, col, prop, value, cellProperties) {
        var returnTipError;
        if (value != "") {
            var celda = getExcelColumnName(4).toString();
            returnTipError = getTipoError(value, 0, 0);
            eliminarError(celda + (row + 1), errorNoNumero);

            if (returnTipError == 3 || returnTipError == 2) {
                td.style.color = errores[errorNoNumero].Color;
                td.style.background = errores[errorNoNumero].BackgroundColor;
                agregarError(celda + (row + 1), value, errorNoNumero);
            }
        }

        if (!isNaN(value)) { value = formatThousands(value); }
        Handsontable.TextCell.renderer.apply(this, arguments);
    };

    hotOptions = {
        data: grillaBD,
        //maxCols: 5,
        width: 1340,
        height: 100,
        colHeaders: true,
        rowHeaders: true,
        hiddenColumns: {
            columns: [1]//, indicators: true
        },
        //stretchH: 'all',
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        colWidths: evt.Handson.ListaColWidth,
        //readOnly: modoLectura,
        //contextMenu: ['row_below', 'remove_row'],
        afterGetColHeader: function (col, th) {

        },
        columns: [
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {}
        ],
        //afterChange: changess,
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            if (row > 0) {
                switch (col) {
                    case 0: cellProperties.className = "htLeft"; cellProperties.readOnly = true; break;
                    case 1: cellProperties.className = "htRight"; cellProperties.readOnly = true; break;
                    case 2: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 3: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 4: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 5: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 6: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 7: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 8: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 9: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 10: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 11: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 12: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 13: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                }
            }

            return cellProperties;
        }
    };
    hot = new Handsontable(container, hotOptions);
}

function crearGrillaExcelEmbalsesEstacionalesProgMensual(evt) {
    var grillaBD = evt.Handson.ListaExcelData;
    optionsListUnidadesOsinerg = evt.ListaUnidadesOsinergCodi;

    container = document.getElementById('detalleFormato');

    var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4682B4';
        td.style.fontSize = '11px';
        td.style.color = '#FFFFFF'
    };

    var flagRenderer = function (instance, td, row, col, prop, value, cellProperties) {
        var currencyCode = value;
    };

    var validarMonto = function (instance, td, row, col, prop, value, cellProperties) {
        var returnTipError;
        if (value != "") {
            var celda = getExcelColumnName(col).toString();
            returnTipError = getTipoError(value, 0, 0);
            eliminarError(celda + (row + 1), errorNoNumero);

            if (returnTipError == 3 || returnTipError == 2) {
                td.style.color = errores[errorNoNumero].Color;
                td.style.background = errores[errorNoNumero].BackgroundColor;
                agregarError(celda + (row + 1), value, errorNoNumero);
            }
        }

        if (!isNaN(value)) { value = formatThousands(value); }
        Handsontable.TextCell.renderer.apply(this, arguments);
    };

    hotOptions = {
        data: grillaBD,
        //maxCols: 5,
        width: 1420,
        height: 1000,
        colHeaders: true,
        rowHeaders: true,
        hiddenColumns: {
            //columns: [2]
        },
        //stretchH: 'all',
        fillHandle: true,
        columnSorting: false,
        className: "htCenter",
        colWidths: evt.Handson.ListaColWidth,
        //readOnly: modoLectura,
        //contextMenu: ['row_below', 'remove_row'],
        afterGetColHeader: function (col, th) {

        },
        columns:
            [{}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}],
        afterChange: changess,
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0) {
                cellProperties.renderer = tituloRenderer;
                cellProperties.readOnly = true;
            }
            if (row > 0) {
                switch (col) {
                    case 0: cellProperties.className = "htCenter htMiddle"; cellProperties.readOnly = true; break;
                    case 1: cellProperties.className = "htLeft"; cellProperties.readOnly = true; break;
                    case 2: cellProperties.className = "htRight"; cellProperties.readOnly = true; break;
                    case 3: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 4: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 5: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 6: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 7: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 8: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 9: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 10: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 11: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 12: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 13: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                    case 14: cellProperties.format = '0,000.000'; cellProperties.className = "htRight"; cellProperties.renderer = validarMonto; break;
                }
            }

            return cellProperties;
        },
        mergeCells: [
            { row: 1, col: 0, rowspan: 8, colspan: 1 },
            { row: 9, col: 0, rowspan: 5, colspan: 1 },
            { row: 14, col: 0, rowspan: 5, colspan: 1 },
            { row: 19, col: 0, rowspan: 5, colspan: 1 },
            { row: 24, col: 0, rowspan: 5, colspan: 1 },
            { row: 29, col: 0, rowspan: 5, colspan: 1 },
            { row: 34, col: 0, rowspan: 5, colspan: 1 },
            { row: 39, col: 0, rowspan: 5, colspan: 1 },
            { row: 44, col: 0, rowspan: 5, colspan: 1 },
            { row: 49, col: 0, rowspan: 5, colspan: 1 },
            { row: 54, col: 0, rowspan: 5, colspan: 1 },
            { row: 59, col: 0, rowspan: 5, colspan: 1 },
            { row: 64, col: 0, rowspan: 5, colspan: 1 },
            { row: 69, col: 0, rowspan: 5, colspan: 1 },
            { row: 74, col: 0, rowspan: 5, colspan: 1 },
            { row: 79, col: 0, rowspan: 5, colspan: 1 },
            { row: 84, col: 0, rowspan: 5, colspan: 1 },
            { row: 89, col: 0, rowspan: 5, colspan: 1 },
            { row: 94, col: 0, rowspan: 5, colspan: 1 },
            { row: 99, col: 0, rowspan: 5, colspan: 1 },
            { row: 104, col: 0, rowspan: 5, colspan: 1 },
            { row: 109, col: 0, rowspan: 5, colspan: 1 },
            { row: 114, col: 0, rowspan: 5, colspan: 1 }
        ]
    };
    hot = new Handsontable(container, hotOptions);
}

function changess(changes, source) {
    if (!changes) {
        return;
    }
    var instance = this;
    var result = 0;
    changes.forEach(function (change) {
        row = change[0];
        col = change[1];
        newValue = change[3];

        if (col == 3) {
            if (hot.getData()[row][2] != null) {
                if (hot.getData()[row][2].trim() != "") {
                    for (var i = 0; i < optionsListUnidadesOsinerg.length; i++) {
                        if (hot.getData()[row][0] == optionsListUnidadesOsinerg[i].codigo) {
                            result = validaUnidadConValue(optionsListUnidadesOsinerg[i].text, hot.getData()[row][2]);
                            if (result > 0) instance.setDataAtCell(row, 4, optionsListUnidadesOsinerg[i].id);
                        }
                    }
                } else {
                    if (hot.getData()[row][0].trim() != "") {
                        for (var i = 0; i < optionsListCentralesOsinerg.length; i++) {
                            if (hot.getData()[row][0] == optionsListCentralesOsinerg[i].codigo) {
                                instance.setDataAtCell(row, 4, optionsListCentralesOsinerg[i].id);
                            }
                        }
                    }
                }
            }
        }
    });
    /*
    for (var i = changes.length - 1; i >= 0; i--) {
        var valorOld = changes[i][2];
        var valorNew = changes[i][3];
        var tipoOld = isNaN(changes[i][2]) ? true : false;//getTipoError(valorOld, liminf, limsup);
        var tipoNew = isNaN(changes[i][3]) ? true : false;//getTipoError(valorNew, liminf, limsup); 
        var celda = getExcelColumnName(parseInt(changes[i][1]) + 1) + (parseInt(changes[i][0]) + 1).toString();
        if (tipoOld > 1) {
            eliminarError(celda, tipoOld);
            if (tipoNew > 1) {agregarError(celda, changes[i][3], tipoNew);}
        }
        else {if (tipoNew > 1) {agregarError(celda, changes[i][3], tipoNew);}}
    }*/
}

function cambio(changes, source) {

    if (!changes) {
        return;
    }
    var instance = this;
    var result = 0;
    changes.forEach(function (change) {
        row = change[0];
        col = change[1];
        newValue = change[3];

        if (col == 1) {

            var codigoosi = hot.getData()[row][0];

            $.ajax({
                type: 'POST',
                async: false,
                url: controlador + 'ObtenerUnidades',
                data: { osicodi: codigoosi },
                success: function (aData) {

                    var filterlist = [];
                    hot.selectCell(row, 2);
                    var activeEditor = instance.getActiveEditor();
                    var activeEditor2 = hot.getCellMeta(row, 2);


                    for (var index = 0; index < aData.length; index++) {

                        filterlist.push(aData[index]);

                    }

                    unidadesLista = filterlist;
                    activeEditor2.data = unidadesLista;
                    activeEditor2.select2Options.data = filterlist;
                    /* activeEditor.options.data = filterlist;*/

                    /*  var algo = evt.Handson.ListaExcelData[row]["2"];*/



                    /*  grillaBD = evt.Handson.ListaExcelData;*/
                    /* hot.render();*/
                },
                error: function () {
                    alert("Ha ocurrido un error");
                }
            });

        }

        /*   if (col == 3) {
               if (hot.getData()[row][2] != null) {
                   if (hot.getData()[row][2].trim() != "") {
                       for (var i = 0; i < optionsListUnidadesOsinerg.length; i++) {
                           if (hot.getData()[row][0] == optionsListUnidadesOsinerg[i].codigo) {
                               result = validaUnidadConValue(optionsListUnidadesOsinerg[i].text, hot.getData()[row][2]);
                               if (result > 0) instance.setDataAtCell(row, 4, optionsListUnidadesOsinerg[i].id);
                           }
                       }
                   } else {
                       if (hot.getData()[row][0].trim() != "") {
                           for (var i = 0; i < optionsListCentralesOsinerg.length; i++) {
                               if (hot.getData()[row][0] == optionsListCentralesOsinerg[i].codigo) {
                                   instance.setDataAtCell(row, 4, optionsListCentralesOsinerg[i].id);
                               }
                           }
                       }
                   }
               }
           }*/
    });


}

function getExcelColumnName(pi_columnNumber) {
    var li_dividend = pi_columnNumber;
    var ls_columnName = "";
    var li_modulo;

    while (li_dividend > 0) {
        li_modulo = (li_dividend - 1) % 26;
        ls_columnName = String.fromCharCode(65 + li_modulo) + ls_columnName;
        li_dividend = Math.floor((li_dividend - li_modulo) / 26);
    }

    return ls_columnName;
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
            if ((Number(value) <= listaPtos[indexPto].Hojaptoliminf)) {
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
