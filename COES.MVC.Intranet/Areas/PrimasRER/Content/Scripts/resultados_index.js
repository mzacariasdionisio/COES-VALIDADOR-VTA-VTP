var controlador = siteRoot + 'PrimasRER/cuadro/';
var ancho = 1000;
var ALTURA_HANDSON = 600;
var LISTA_HoT = [null];
var error = [];
var headerht = [];

$(function () {
    $('#cntMenu').css("display", "none");

    $("#btnExportarExcelAprobados").click(function () {
        descargarArchivoExcelEvaluacion(1)
    });

    $("#btnExportarExcelNoAprobados").click(function () {
        descargarArchivoExcelEvaluacion(2)
    });

    $("#btnExportarExcelFuerzaMayor").click(function () {
        descargarArchivoExcelEvaluacion(3)
    });

    $("#btnValidar").click(function () {
        guardarEvaluacion();
    });

    cargarHandsonTable();

});

function cargarHandsonTable() {

    ALTURA_HANDSON = parseInt($(".listado1").height());
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 800;

    $("#listado1").html('');
    let container1 = document.getElementById('listado1');

    if (typeof LISTA_HoT[0] != 'undefined' && LISTA_HoT[0] !== null) {
        LISTA_HoT[0].destroy();
    }

    let rerevacodi = parseInt($("#res_rerevacodi").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerHandsonTableListadoEvaluacionSolicitudEdiParaResultados",
        data: {
            rerevacodi: rerevacodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.EsEditable) {
                    $("#btnValidar").removeAttr('style');
                    $("#btnValidado").attr('style', 'display: none;');
                }
                else {
                    $("#btnValidar").attr('style', 'display: none;');
                    $("#btnValidado").removeAttr('style');
                }
                crearGrillaExcel(0, container1, evt.HandsonTable, ALTURA_HANDSON, evt.EsEditable);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function guardarEvaluacion() {

    var answer = confirm("¿Está seguro que desea validar y cerrar los datos?");
    if (!answer) {
        return;
    }

    if (parseInt(error.length) > 0) {
        abrirPopupErrores();
        return;
    }

    var rerevacodi = $("#res_rerevacodi").val();
    var dataht = LISTA_HoT[0].getData();

    $.ajax({
        type: 'POST',
        url: controlador + "ActualizarEvaluacionSolicitudEdiParaResultados",
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            rerevacodi: rerevacodi,
            dataht: dataht
        }),
        success: function (evt) {
            if (evt.Resultado == "1") {
                $("#btnValidar").attr('style', 'display: none;');
                $("#btnValidado").removeAttr('style');
                alert("Se guardaron los datos exitosamente.");
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function crearGrillaExcel(tab, container, handson, heightHansonTab, esEditable) {
    var ColorEmpresa = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#4F81BD';
        td.style.fontSize = '11px';
        td.style.color = 'white'
    };
    var LateralIzq = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#DCE6F1';
        td.style.fontSize = '11px';
        td.style.color = 'black';
    };
    var ColorContent = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.backgroundColor = '#D9D9D9';
        td.style.fontSize = '11px';
        td.style.color = 'black'
    };
    var ImagenDescargaArchivoExcel = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.innerHTML = "<a href='javascript:descargarArchivoExcel(" + value.toString() + ")'><img style='margin-top: 4px; margin-bottom: 4px;' width='25' height='25' src='../../Content/Images/ExportExcel.png' alt='Descargar archivo Excel' title='Descargar archivo Excel' /></a>";
        td.style.backgroundColor = '#DCE6F1';
        td.style.fontSize = '11px';
        td.style.color = 'black';
    };

    var maxCols = handson.MaxCols;
    var maxRows = handson.MaxRows;
    var columns = handson.Columnas;
    var headers = handson.Headers;
    headerht = handson.Headers;
    var widths = handson.ListaColWidth;
    var data = handson.ListaExcelData;
    var dataDescrip = handson.ListaExcelDescripcion;
    var dataFormato = handson.ListaExcelFormatoHtml;
    var arrMergeCells = handson.ListaMerge;
    var listaCambios = handson.ListaCambios;
    var editar = esEditable;

    LISTA_HoT[tab] = new Handsontable(container, {
        data: data,
        stretchH: "all",
        observeChanges: true,
        colHeaders: headers,
        colWidths: widths,
        rowHeaders: true,
        columnSorting: false,
        minSpareCols: 0,
        minSpareRows: 0,
        maxCols: maxCols,
        maxRows: maxRows,
        readOnly: true,
        columns: columns,
        height: heightHansonTab,
        mergeCells: arrMergeCells,
        fixedColumnsLeft: 0,
        cells: function (row, col, prop) { 
            var cellProperties = {
            };

            if ((col == 1 || col == 4 || col == 5 || col == 6 || col == 8 || col == 10 || col == 11) && row >= 0) {
                cellProperties.renderer = LateralIzq;
            }

            if (col == 11 && row >= 0) {
                cellProperties.renderer = ImagenDescargaArchivoExcel;
            }

            if (editar == false)
            {
                if ((col == 2 || col == 3 || col == 7 || col == 9) && row >= 0) {
                    cellProperties.renderer = LateralIzq;
                }
            }

            if (row >= 0 && esEditable == true) {
                switch (true) {
                    case (col == 0): //DEBE COINCIDIR CON LA CONDICION PARA LIMPIAR LA LISTA DE ERRORES
                    case (col == 2):
                    case (col == 3):
                    case (col == 7):
                    case (col == 9):
                        cellProperties.renderer = ValueRenderer;
                        break;
                }
            }

            return cellProperties;
        }
    });
}

var ValueRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    var sHeader = headerht[col]; //$(instance.getCell(0, col)).html(); //.getColHeader(col)
    //var sColumn = $(instance.getCell(row, 1)).html();
    var valueMaximum = 999999999;
    var valueMinimum = -999999999;
    var sMensaje = "";

    if ((row == 0 && col == 0)) { //DEBE COINCIDIR CON LA ASIGNACION DE VALUERENDERER
        error = []; //Limpiamos la lista de errores, esto sólo se hace una sóla vez
        error.length = 0;
    }

    if ((col == 2 || col == 3 || col == 7) && (value == undefined || value == null || value.trim() == "")) {
        td.style.background = '#ECAFF0';
        sMensaje = "[1]La columna '" + sHeader + "' tiene un valor vacio (fila: " + (row + 1) + ", columna: " + col + ").";
    }

    if (sMensaje != null && sMensaje.trim() != "" && sMensaje.trim().length > 0) {
        if (value == null || value.trim() == "") { value = ""; }
        error.push(value.toString().concat("_-_" + row + "_-_" + sHeader + "_-_" + sMensaje));
        sMensaje = "";
    }
};

abrirPopupErrores = function () {
    var html = '<span class="button b-close"><span>X</span></span>';
    html += '<p><b>Corregir los siguientes errores</b><p>';
    html += '<table border="0" class="pretty tabla-icono" id="tabla">'
    html += '<thead>'
    html += '<tr>'
    html += '<th>Fila</th>'
    html += '<th>Columna</th>'
    html += '<th>Valor</th>'
    html += '<th>Comentario</th>'
    html += '</tr>'
    html += '</thead>'
    html += '<tbody>'
    for (var i = error.length - 1; i >= 0; i--) {
        var sStyle = "background : #ffffff;";
        var sBackground = "";
        if (i % 2) {
            var sStyle = "background : #fbf4bf;";
        }
        var SplitError = error[i].split("_-_");
        var sTipError = SplitError[3].substring(0, 3);
        if (sTipError === "[1]") {
            sBackground = " background-color: #ECAFF0"; //#FAC0BB;"; //#F02211
        }
        else if (sTipError === "[2]") {
            sBackground = " background-color: #F02211"; //#F3F554;";
        }
        else if (sTipError === "[3]") {
            sBackground = " background-color: #F3F554"; //#ECAFF0;";
        }
        var sMsgError = SplitError[3].substring(3);
        html += '<tr id="Fila_' + i + '">'
        html += '<td style="text-align:right;' + sBackground + '">' + (parseInt(SplitError[1]) + 1) + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[2] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[0] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + sMsgError + '</td>'
        html += '</tr>'
    }
    html += '</tbody>'
    html += '</table>'

    $('#popupErrores').html(html);
    $('#popupErrores').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    alert("La hoja del cálculo tiene errores.");
}

function descargarArchivoExcel(reresecodi) {
    var rerevacodi = $("#res_rerevacodi").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportaraExcelEnergiaEstimadaEvaluacionSolicitudEdi',
        contentType: 'application/json;',
        data: JSON.stringify({
            reresecodi: reresecodi,
            rerevacodi: rerevacodi,
        }),
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado == "-1") {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
            else {
                window.location = controlador + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + evt.Resultado;
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error.");
        }
    });
}

function descargarArchivoExcelEvaluacion(tipoReporte) {
    var rerevacodi = $("#res_rerevacodi").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportaraExcelEvaluacionParaResultados',
        contentType: 'application/json;',
        data: JSON.stringify({
            rerevacodi: rerevacodi,
            tipoReporte: tipoReporte
        }),
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado == "-1") {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
            else {
                window.location = controlador + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + evt.Resultado;
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error.");
        }
    });
}
