var controlador = siteRoot + 'PrimasRER/cuadro/';
var ancho = 1000;
var ALTURA_HANDSON = 600;
var LISTA_HoT = [null];
var error = [];
var headerht = [];

$(function () {
    $('#cntMenu').css("display", "none");

    $("#btnRegresar").click(function () {
        history.back();
    });

    $("#btnGuardar").click(function () {
        guardarEvaluacion();
    });

    cargarHandsonTable();

});

function cargarHandsonTable() {

    ALTURA_HANDSON = parseInt($(".panel-container").height());
    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 800;

    $("#listado1").html('');
    var container1 = document.getElementById('listado1');

    if (typeof LISTA_HoT[0] != 'undefined' && LISTA_HoT[0] !== null) {
        LISTA_HoT[0].destroy();
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerHandsonTableListadoEvaluacionSolicitudEdi",
        data: {
            rerevacodi: $("#eva_rerevacodi").val(),
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                crearGrillaExcel(0, container1, evt.HandsonTable, ALTURA_HANDSON);
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

    if (parseInt(error.length) > 0) {
        abrirPopupErrores();
        return;
    }
    
    var rerevacodi = $("#eva_rerevacodi").val();
    var dataht = LISTA_HoT[0].getData();

    $.ajax({
        type: 'POST',
        url: controlador + "ActualizarEvaluacionSolicitudEdi",
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            rerevacodi: rerevacodi,
            dataht: dataht
        }),
        success: function (evt) {
            if (evt.Resultado == "1") {
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

function crearGrillaExcel(tab, container, handson, heightHansonTab) {
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
    var ImagenDescargaArchivo = function (instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.innerHTML = "<a href='javascript:descargarArchivo(\"" + value.toString() + "\")'><img style='margin-top: 4px; margin-bottom: 4px;' src='../../Content/Images/bajar.png' alt='Descargar archivo' title='Descargar archivo' /></a>";
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

            if ((col == 1 || col == 2 || col == 3 || col == 6 || col == 10 || col == 11 || col == 12 || col == 13 || col == 14) && row >= 0) {
                cellProperties.renderer = LateralIzq;
            }

            if (col == 8 && row >= 0) {
                cellProperties.renderer = ImagenDescargaArchivoExcel;
            }

            if (col == 9 && row >= 0) {
                cellProperties.renderer = ImagenDescargaArchivo;
            }

            if (row >= 0) {
                switch (true) {
                    case (col == 0): //DEBE COINCIDIR CON LA CONDICION PARA LIMPIAR LA LISTA DE ERRORES
                    case (col == 4):
                    case (col == 5):
                    case (col == 7):
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

    if ((col == 4 || col == 5 || col == 7) && (value == undefined || value == null || value.trim() == "")) {
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
    var rerevacodi = $("#eva_rerevacodi").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportaraExcelEvaluacionEnergiaUnidad',
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

function descargarArchivo(nombreArchivo) {
    if (nombreArchivo == null || nombreArchivo.trim() == "")
    {
        alert("No existe un nombre de archivo.");
        return;
    } 

    window.location = controlador + 'ExportarSustentoEvaluacionSolicitudEdi?nombreArchivo=' + nombreArchivo;
}