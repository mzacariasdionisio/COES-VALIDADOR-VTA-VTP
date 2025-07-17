var controlador = siteRoot + 'Transferencias/EvaluacionParticipante/'
let listaCambios = [];
$(function () {

    $('#FechaDesde').Zebra_DatePicker({
    });

    $('#FechaHasta').Zebra_DatePicker({
    });

    $('#btnConsultar').click(function () {
        mostrarGrillaPtoMax();
    });

    $('#btnExportar').click(function () {
        descargarFormato();
    });

    $('#btnGuardar').click(function () {
        grabarPotenciaMax();
        grabarCna();
    });

    $('#btnProcesarCna').click(function () {
        ProcesarCna();
    });
    $('#btnProcesarCnaTest').click(function () {
        ProcesarCnaTest();
    });

    $("#btnConfiguracionDia").click(function () {
        NuevaConfiguracionDias();
    });

    $('#btnlogProceso').click(function () {
        ListadoCNAProcesado();
    });

    

    $('#btnCorreo').click(function () {
        NotificacionCNA();
    });

    mostrarGrillaPtoMax();
    
});

function mostrarGrillaPtoMax() {

    var fechaInicio = $("#FechaDesde").val().trim();
    var fechaFin = $("#FechaHasta").val().trim();
    var tipoEmpresa = $("#tipoEmpresa").val();
    var nombreEmpresa = $("#empresa").val();

    $.ajax({
        type: 'POST',
        url: controlador + "MostrarGrillaPtoMax",
        dataType: 'json',
        data: {
            fechaInicio: fechaInicio,
            fechaFin: fechaFin,
            tipoEmpresa: tipoEmpresa,
            nombreEmpresa: nombreEmpresa
        },
        success: function (evt) {
            if (evt != -1) {
                $('#listadoPotMax').html('');
                if (evt.Resultado != '-1') {
                    crearHandsonTable(evt, true);
                    mostrarGrillaCna();
                } 
            }
            else {
                alert("La empresa no tiene puntos de medición para cargar.");
            }
        },
        error: function (err) {
            alert("Error al cargar Excel Web");
        }
    });
}

var container;
function crearHandsonTable(evtHot, flag) {

    debugger;
    function errorRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        td.style.fontSize = '12px';
        td.style.color = 'black';
        td.style.background = '#93C47D';
    }

    function fechaRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#93C47D';
    }

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function firstRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#93C47D';
    }

    function descripRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#EAF7D9';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#E8F6FF';
        td.style.textAlign = 'center';
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.textAlign = 'center';
        td.style.color = 'MidnightBlue';
        td.style.background = '#FFFFD7';
        //$(td).html(formatFloat(parseFloat(value), 3, '.', ','));
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function calculateSize() {
    }

    container = document.getElementById('listadoPotMax');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        height: 100,
        maxRows: evtHot.Handson.ListaExcelData.length,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: 1,
        fixedColumnsLeft: evtHot.ColumnasCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        afterLoadData: function () {
            this.render();
        },
        afterChange(registrosmodificados, accionesHandsontable) {
            if (accionesHandsontable != 'loadData') {
                registrosmodificados.forEach(function (elemento) {
                    //evtHot.Handson.ListaMerge.push(elemento);
                    listaCambios.push(elemento);
                })
            }
        },
        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;

            if (row == 0) {
                render = firstRowRenderer;
                readOnly = true;
            }
            if (row >= evtHot.FilasCabecera && row <= evtHot.Handson.ListaExcelData.length) {
                render = descripRowRenderer2;
                formato = '0,0.000';
                tipo = 'numeric';
                readOnly = false;
            }

            for (var i in evtHot.Handson.ListaCambios) {
                if ((row == evtHot.Handson.ListaCambios[i].Row) && (col == evtHot.Handson.ListaCambios[i].Col)) {
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
    calculateSize(1);
}

function mostrarGrillaCna() {

    var fechaInicio = $("#FechaDesde").val().trim();
    var fechaFin = $("#FechaHasta").val().trim();
    var tipoEmpresa = $("#tipoEmpresa").val();
    var nombreEmpresa = $("#empresa").val();

    var $container = $('#listadoPotMax');
    //$('#hfDataExcel').val((hot.getData()));
    var data = hot.getData();
    $.ajax({
        type: 'POST',
        url: controlador + "MostrarGrillaCna",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({
            dataExcel: data,
            fechaInicio: fechaInicio,
            fechaFin: fechaFin,
            tipoEmpresa: tipoEmpresa,
            nombreEmpresa: nombreEmpresa
        }),
        success: function (evt) {
            if (evt != -1) {
                $('#listadoCNA').html('');
                if (evt.Resultado != '-1') {
                    crearHandsonTableCna(evt, true);
                }
            }
            else {
                alert("No registros con CNA.");
            }
        },
        error: function (err) {
            alert("Error al cargar Excel Web");
        }
    });
}

var containerCna;
function crearHandsonTableCna(evtHot, flag) {

    function errorRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        td.style.fontSize = '12px';
        td.style.color = 'black';
        td.style.background = '#93C47D';
    }

    function fechaRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#93C47D';
    }

    function firstRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#2980B9';
        td.style.textAlign = 'center';
    }

    function firstRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontWeight = 'bold';
        td.style.fontSize = '10px';
        td.style.color = 'White';
        td.style.background = '#93C47D';
    }

    function descripRowRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#EAF7D9';
    }

    function descripRowRenderer2(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '10px';
        td.style.color = 'MidnightBlue';
        td.style.background = '#E8F6FF';
        td.style.textAlign = 'center';
    }

    function cambiosCellRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        td.style.background = '#FFFFD7';
        $(td).html(formatFloat(parseFloat(value), 3, '.', ','));
    }

    function negativeValueRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);

        if (parseInt(value, 10) < 0) {
            td.style.color = 'orange';
            td.style.fontStyle = 'italic';
        }
    }

    function limitesCellRenderer(instance, td, row, col, prop, value, cellProperties) {

        Handsontable.renderers.TextRenderer.apply(this, arguments);
        td.style.fontSize = '12px';
        td.style.textAlign = 'right';
        if (Number(value) && value != "") {
            if (Number(value) < evtHot.ListaHojaPto[col - 1].Hojaptoliminf) {
                td.style.background = 'orange';
                $(td).html(formatFloat(Number(value), 3, '.', ','));
                var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                agregarError(celda, value, 3);

            }
            else {
                if (Number(value) > evtHot.ListaHojaPto[col - 1].Hojaptolimsup) {
                    td.style.background = 'yellow';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 4);
                }
                else {
                    td.style.background = 'white';
                    $(td).html(formatFloat(Number(value), 3, '.', ','));
                }
            }
        }
        else {
            if (value == "0")
                $(td).html("0.000");
            else if (value != "") {
                if (!Number(value)) {
                    td.style.background = 'red';
                    var celda = getExcelColumnName(parseInt(col) + 1) + (parseInt(row) + 1).toString();
                    agregarError(celda, value, 2);
                }
            }
        }

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

    function calculateSize() {
    }

    containerCna = document.getElementById('listadoCNA');
    Handsontable.renderers.registerRenderer('negativeValueRenderer', negativeValueRenderer);

    hotOptions = {
        data: evtHot.Handson.ListaExcelData,
        height: 500,
        maxRows: evtHot.Handson.ListaExcelData.length,
        maxCols: evtHot.Handson.ListaExcelData[0].length,
        colHeaders: true,
        rowHeaders: true,
        fillHandle: true,
        columnSorting: false,
        className: "",
        readOnly: evtHot.Handson.ReadOnly,
        fixedRowsTop: 1,
        fixedColumnsLeft: evtHot.ColumnasCabecera,
        mergeCells: evtHot.Handson.ListaMerge,
        colWidths: evtHot.Handson.ListaColWidth,
        afterLoadData: function () {
            this.render();
        },

        cells: function (row, col, prop) {
            var cellProperties = {};
            var formato = "";
            var render;
            var readOnly = true;
            var tipo;

            if (row == 0) {
                render = firstRowRenderer;
                readOnly = true;
            }
            if (row >= evtHot.FilasCabecera && row <= evtHot.Handson.ListaExcelData.length) {
                render = descripRowRenderer2;
                formato = '0,0.000';
                tipo = 'numeric';
                readOnly = false;
            }

            for (var i in evtHot.ListaCambios) {
                if ((row == evtHot.ListaCambios[i].Row) && (col == evtHot.ListaCambios[i].Col)) {
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

    hotCna = new Handsontable(containerCna, hotOptions);
    calculateSize(1);
}

function descargarFormato() {
    debugger;
    var fechaInicio = $("#FechaDesde").val().trim();
    var fechaFin = $("#FechaHasta").val().trim();
    var tipoEmpresa = $("#tipoEmpresa").val();
    var nombreEmpresa = $("#empresa").val();
    $('#hfDataExcel').val((hot.getData()));
    var data = hot.getData();
    $.ajax({
        type: 'POST',
        url: controlador + 'generarformato',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({
            dataExcel: data,
            fechaInicio: fechaInicio,
            fechaFin: fechaFin,
            tipoEmpresa: tipoEmpresa,
            nombreEmpresa: nombreEmpresa
        }),
        success: function (result) {
            if (result == 1) {
                window.location = controlador + 'descargarformato';
            }
            else {
                alert(result);
            }
        },
        error: function () {
            alert("Error");
        }
    });
}

function grabarPotenciaMax() {
    debugger;
    console.log(listaCambios);
    var fechaInicio = $("#FechaDesde").val().trim();
    var fechaFin = $("#FechaHasta").val().trim();
    var tipoEmpresa = $("#tipoEmpresa").val();
    var nombreEmpresa = $("#empresa").val();
    $('#hfDataExcel').val((hot.getData()));
    var data = hot.getData();
    console.log(data);
    $.ajax({
        type: 'POST',
        url: controlador + 'grabarPotenciaMax',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({
            dataExcel: data,
            dataExcelCambios: listaCambios,
            fechaInicio: fechaInicio,
            fechaFin: fechaFin,
            tipoEmpresa: tipoEmpresa,
            nombreEmpresa: nombreEmpresa
        }),
        success: function (result) {
            if (result.Resultado == "1") {
                listaCambios = [];
                mostrarMensajeEval('mensaje', 'exito', "Los datos se guardaron correctamente.");
            }
            else if (result.Resultado == "-1"){
                mostrarMensajeEval('mensaje', 'alert', result.StrMensaje);
            }
        },
        error: function () {
            alert("Error");
        }
    });
}

function mostrarMensajeEval(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

function NuevaConfiguracionDias() {

    $.ajax({
        type: 'POST',
        url: controlador + 'ConfigurarDias',
        success: function (evtDias) {
            $('#contenidoDetalleDias').html(evtDias);

            setTimeout(function () {
                $('#popupNuevaConfiguracionDias').bPopup({
                    autoClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });
            }, 500);            
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function ProcesarCna() {
    var fechaInicio = $("#FechaDesde").val().trim();
    var fechaFin = $("#FechaHasta").val().trim();
    $.ajax({
        type: 'POST',
        url: controlador + 'ProcesarCna',
        dataType: 'json',
        data: {
            fechaInicio: fechaInicio,
            fechaFin: fechaFin
        },
        success: function (result) {
            if (result.Resultado == "1") {
                mostrarMensajeEval('mensaje', 'exito', "El cálculo de Cna se procesó con éxito.");
            }
            else if (result.Resultado == "-1") {
                mostrarMensajeEval('mensaje', 'alert', result.StrMensaje);
            }
        },
        error: function () {
            alert("Error");
        }
    });
}

function grabarCna() {
    $('#hfDataExcelCna').val((hotCna.getData()));
    var dataCna = hotCna.getData();
    $.ajax({
        type: 'POST',
        url: controlador + 'grabarCna',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({
            dataExcel: dataCna
        }),
        success: function (result) {
            if (result.Resultado == "1") {
                mostrarMensajeEval('mensaje', 'exito', "Los datos se guardaron correctamente.");
            }
            else if (result.Resultado == "-1") {
                mostrarMensajeEval('mensaje', 'alert', result.StrMensaje);
            }
        },
        error: function () {
            alert("Error");
        }
    });
}

function ProcesarCnaTest() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ProcesarCnaTest',
        dataType: 'json',
        success: function (result) {
            if (result.Resultado == "1") {
                mostrarMensajeEval('mensaje', 'exito', "El cálculo de Cna automático se procesó con éxito.");
            }
            else if (result.Resultado == "-1") {
                mostrarMensajeEval('mensaje', 'alert', result.StrMensaje);
            }
        },
        error: function () {
            alert("Error");
        }
    });
}

function ListadoCNAProcesado() {

    var fechadesde = $("#FechaDesdeLog").val();
    var fechahasta = $("#FechaHastaLog").val();
    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoProcesosCNA',
        data: {
            fechadesde: fechadesde,
            fechahasta: fechahasta
        },
        success: function (evtLog) {
            $('#contenidoDetalleLogCna').html(evtLog);
            setTimeout(function () {
                $('#popupLogCna').bPopup({
                    autoClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });
            }, 500);
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

function NotificacionCNA() {
    $.ajax({
        type: 'POST',
        url: controlador + 'NotificacionCNA',
        dataType: 'json',
        success: function (result) {
            if (result.Resultado == "1") {
                mostrarMensajeEval('mensaje', 'exito', "Se notificó con éxito.");
            }
            else if (result.Resultado == "-1") {
                mostrarMensajeEval('mensaje', 'alert', result.StrMensaje);
            }
        },
        error: function () {
            alert("Error");
        }
    });
}