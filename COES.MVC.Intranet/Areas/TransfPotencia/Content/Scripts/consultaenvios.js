var sControlador = siteRoot + "transfpotencia/consultaenvios/";

var enterprisename = '';

$(document).ready(function () {

    listarEnvios();
    $('#btnListarEnvios').click(function () {
        listarEnvios();
        mostrarExito("Ya puede consultar la información");
    });

    $('#btnListarSeleccionados').click(function () {
        listarSeleccionados();
    });

});

recargar = function () {
    var pericodi = $('#pericodi').val();
    var sUrl = sControlador + "index?pericodi=" + pericodi;
    window.location.href = sUrl;
}

listarEnvios = function () {
    var pericodi = $('#pericodi').val();
    var recpotcodi = $('#recpotcodi').val();
    var emprcodi = $('#emprcodi').val();
    var plazo = $('#plazo').val();
    var liquidacion = $('#liquidacion').val();

    if (recpotcodi == null) { recpotcodi = "0";}

    $.ajax({
        type: 'POST',
        url: sControlador + "lista",
        data: { pericodi: pericodi, recpotcodi: recpotcodi, emprcodi: emprcodi, plazo: plazo, liquidacion: liquidacion },
        success: function (evt) {
            $('#listado').html(evt);
            viewEvent();
            exportEvent();
            exportValidacionesEnvio();
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
};

viewEvent = function () {
    //Aqui
    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopupView(id);
    });
};

abrirPopupView = function (id) {
    $.ajax({
        type: 'POST',
        url: sControlador + "view/" + id,
        success: function (evt) {
            $('#popup').html(evt);
            $('#imgHandsontable').css('display', 'block');
            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarMensaje("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
};

exportEvent = function () {
    $('.export').click(function () {
        var pericodi = $('#pericodi').val()
        var recpotcodi = $('#recpotcodi').val()
        var pegrcodi = $(this).attr("id").split("_")[1];
        $.ajax({
            type: 'POST',
            url: sControlador + 'exportardata',
            data: { pericodi: pericodi, recpotcodi: recpotcodi, pegrcodi: pegrcodi, emprcodi: $('#emprcodi').val(), formato: 1 },
            dataType: 'json',
            success: function (result) {
                if (result != -1) {
                    window.location = sControlador + 'abrirarchivo?formato=1&file=' + result;
                    mostrarExito("Felicidades, el archivo de envío: " + pegrcodi + " se descargo correctamente...!");
                }
                else {
                    mostrarError(result.error);
                }
            },
            error: function (response) {
                mostrarError(response.status + " " + response.statusText);
            }
        });
    });
};

exportValidacionesEnvio = function () {
    $('.exportValidaciones').click(function () {
        var pericodi = $('#pericodi').val()
        var recpotcodi = $('#recpotcodi').val()
        var pegrcodi = $(this).attr("id").split("_")[2];
        $.ajax({
            type: 'POST',
            url: sControlador + 'exportardatavalidaciones',
            data: { pericodi: pericodi, recpotcodi: recpotcodi, pegrcodi: pegrcodi, formato: 1, enterprisename: enterprisename },
            dataType: 'json',
            success: function (result) {
                if (result != -1) {
                    window.location = sControlador + 'abrirarchivo?formato=1&file=' + result;
                    mostrarExito("Felicidades, el archivo de envío: " + pegrcodi + " se descargo correctamente...!");
                }
                else {
                    mostrarError(result.error);
                }
            },
            error: function (response) {
                mostrarError(response.status + " " + response.statusText);
            }
        });
    });
};

selectEnterprise = function (name) {
    enterprisename = name;
}

mostrarExcelWeb = function (pegrcodi) {
    $.ajax({
        type: 'POST',
        url: sControlador + 'mostrarexcelweb',
        data: {
            pegrcodi: pegrcodi,
            pericodi: $('#pericodi').val(),
            emprcodi: $('#emprcodi').val()
        },
        dataType: 'json',
        success: function (result) {

            if (result.esFormatoNuevo == 1)
                configurarExcelWeb(result);
            else
                configurarExcelWebAntiguo(result);

            //console.log(result.NroColumnas);
            if (result.MensajeError) {
                alert("Lo sentimos, se ha producido un error: <br>" + result.MensajeError);
            }
            if ($('#pegrcodi').val() > 0)
                mostrarExito('Código del envío consultado: ' + $('#pegrcodi').val() + ", Fecha de envío: " + result.TrnEnvFecIns + ", Nro. Códigos reportados: " + result.NroColumnas);
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

configurarExcelWebAntiguo = function (result) {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcel');
    $('#pegrcodi').val(result.pegrcodi);
    var columns = result.Columnas;
    var widths = result.Widths;
    var data = result.Data;
    hot = new Handsontable(container, {
        data: data,
        //maxCols: result.Columnas.length,
        colHeaders: false,
        rowHeaders: true,
        colWidths: widths,
        contextMenu: false,
        minSpareRows: 0,
        columns: columns,
        fixedRowsTop: result.FixedRowsTop,
        fixedColumnsLeft: result.FixedColumnsLeft,
        currentRowClassName: 'currentRow',
        mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 },
        { row: 0, col: 1, rowspan: 2, colspan: 1 },
        { row: 0, col: 2, rowspan: 2, colspan: 1 },
        { row: 0, col: 3, rowspan: 2, colspan: 1 },
        { row: 0, col: 4, rowspan: 1, colspan: 2 },
        { row: 0, col: 6, rowspan: 1, colspan: 3 },
        { row: 0, col: 9, rowspan: 1, colspan: 3 },
        { row: 0, col: 12, rowspan: 2, colspan: 1 }],
        cells: function (row, col, prop) {
            //console.log("col:" + col + " row:" + row + " prop" + prop);
            var cellProperties = {};
            if (row == 0 || row == 1) {
                cellProperties.renderer = firstRowRendererCabecera;
            }
            else if (col == 0 || col == 1 || col == 2 || col == 3 || col == 12) {
                cellProperties.renderer = firstRowRendererCeleste;
            }
            else if (col == 9 || col == 10 || col == 11) {
                cellProperties.renderer = firstRowRendererAmarillo;
            }
            return cellProperties;
        },
    });
    hot.render();
    $('#imgHandsontable').css('display', 'none');
}
configurarExcelWeb = function (result) {

    if (typeof hot != 'undefined') {
        hot.destroy();
    }

    console.log(result.Data)
    var container = document.getElementById('grillaExcel');
    $('#pegrcodi').val(result.pegrcodi);
    var columns = result.Columnas;
    var widths = result.Widths;
    hot = new Handsontable(container, {
        data: result.Data,
        //maxCols: result.Columnas.length,
        colHeaders: false,
        rowHeaders: true,
        colWidths: widths,
        contextMenu: true,
        minSpareRows: 1,
        columns: columns,
        fixedRowsTop: result.FixedRowsTop,
        fixedColumnsLeft: result.FixedColumnsLeft,
        currentRowClassName: 'currentRow',
        cells: function (row, col, prop) {
            //console.log("col:" + col + " row:" + row + " prop" + prop);
            var cellProperties = {};
            if (row == 0) {
                cellProperties.renderer = firstRowRendererCabecera;
            }

            return cellProperties;
        },
    });
    hot.render();
    $('#imgHandsontable').css('display', 'none');
}

listarSeleccionados = function () {
    var iNumReg = document.getElementById('Count').value;
    //console.log(iNumReg);
    if (iNumReg > 0) {
        var items = checkMark();
        //console.log(items);
        if (items != "0") {
            var pericodi = $('#pericodi').val()
            var recpotcodi = $('#recpotcodi').val()
            $.ajax({
                type: 'POST',
                url: sControlador + 'listarseleccionados',
                data: { pericodi: pericodi, recpotcodi: recpotcodi, items: items },
                success: function (evt) {
                    $('#popup2').html(evt);
                    setTimeout(function () {
                        $('#popup2').bPopup({
                            easing: 'easeOutBack',
                            speed: 450,
                            transition: 'slideDown'
                        });
                    }, 50);
                    $('#divSiLiquidacion').css('display', '');
                    $('#divNo').css('display', '');
                    $('#divCerrar').css('display', 'none');
                },
                error: function () {
                    mostrarError("Lo sentimos, ha ocurrido un error inesperado");
                }
            });
        }
        else {
            mostrarError("No se ha seleccionado ningun registro");
        }
    }
    else {
        mostrarError("No se ha seleccionado ningun registro");
    }
}

siLiquidacion = function () {
    var items = checkMark();
    var pericodi = $('#pericodi').val()
    var recpotcodi = $('#recpotcodi').val()
    $.ajax({
        type: 'POST',
        url: sControlador + 'siliquidacion',
        data: { pericodi: pericodi, recpotcodi: recpotcodi, items: items },
        dataType: 'json',
        success: function (result) {
            if (result.sError) {
                alert("Lo sentimos, se ha producido un error: <br>" + result.sError);
            }
            else {
                alert('Se han procesado ' + result.iNumReg + ' envios correctamente');
                $('#divSiLiquidacion').css('display', 'none');
                $('#divNo').css('display', 'none');
                $('#divCerrar').css('display', '');
            }
        },
        error: function () {
            alert('Lo sentimos no se puede ejecutar el proceso')
        }
    });
}

cerrarPopup2 = function () {
    $('#popup2').bPopup().close();
    //$('#popup2').empty();
    listarEnvios();
}

firstRowRendererCabecera = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'white';
    td.style.background = '#3D8AB8';
    td.style.fontWeight = 'bold';
    td.style.fontFamily = 'sans - serif';
    td.style.fontSize = '12px';
    cellProperties.className = "htCenter",
        cellProperties.readOnly = true;
}

firstRowRendererCeleste = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#E8F6FF';
    td.style.fontFamily = 'sans - serif';
    td.style.fontSize = '12px';
}

firstRowRendererAmarillo = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#FFFFD7';
}

getCustomRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = '#2980B9';
    td.style.background = '#2980B9';
    //console.log(value);
}