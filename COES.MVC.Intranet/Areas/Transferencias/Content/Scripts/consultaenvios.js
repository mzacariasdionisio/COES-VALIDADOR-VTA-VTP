var sControlador = siteRoot + "transferencias/consultaenvios/";

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
    var recacodi = $('#recacodi').val();
    var emprcodi = $('#emprcodi').val();
    var trnenvtipinf = $('#trnenvtipinf').val();
    var trnenvplazo = $('#trnenvplazo').val();
    var trnenvliqvt = $('#trnenvliqvt').val();
    $.ajax({
        type: 'POST',
        url: sControlador + "lista",
        data: { pericodi: pericodi, recacodi: recacodi, emprcodi: emprcodi, trnenvtipinf: trnenvtipinf, trnenvplazo: trnenvplazo, trnenvliqvt: trnenvliqvt},
        success: function (evt) {
            $('#listado').html(evt);
            viewEvent();
            exportEvent();
            //oTable = $('#tabla').dataTable({
            //    "sPaginationType": "full_numbers",
            //    "destroy": "true",
            //    "aaSorting": [[2, "asc"], [1, "desc"]]
            //});
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
};

viewEvent = function () {
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
            mostrarMensajeDiv("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
};

exportEvent = function () {
    $('.export').click(function () {
        trnenvcodi = $(this).attr("id").split("_")[1];
        $.ajax({
            type: 'POST',
            url: sControlador + 'exportardata',
            data: { trnenvcodi: trnenvcodi, formato:1 },
            dataType: 'json',
            success: function (result) {
                if (result != -1) {
                    window.location = sControlador + 'abrirarchivo?formato=1&file=' + result;
                    mostrarExito("Felicidades, el archivo de envío: " + trnenvcodi+ " se descargo correctamente...!");
                }
                else {
                    mostrarErrorDiv(result.error);
                }
            },
            error: function (response) {
                mostrarErrorDiv(response.status + " " + response.statusText);
            }
        });
    });
};

mostrarExcelWeb = function (Trnenvcodi) {
    $.ajax({
        type: 'POST',
        url: sControlador + 'mostrarexcelweb',
        data: { trnenvcodi: Trnenvcodi },
        dataType: 'json',
        success: function (result) {
            configurarExcelWeb(result);
            //console.log(result.NroColumnas);
            if (result.MensajeError) {
                alert("Lo sentimos, se ha producido un error: <br>" + result.MensajeError);
            }
            if ($('#trnenvcodi').val() > 0)
                mostrarExito('Código del envío consultado: ' + $('#trnenvcodi').val() + ", Fecha de envío: " + result.TrnEnvFecIns + ", Nro. Códigos reportados: " + result.NroColumnas);
        },
        error: function () {
            mostrarErrorDiv('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

configurarExcelWeb = function (result) {

    if (typeof hot != 'undefined') {
        hot.destroy();
    }
    var container = document.getElementById('grillaExcel');
    var limiteMaxEnergia = result.LimiteMaxEnergia; //Maximo Limite de energia
    $('#trnenvcodi').val(result.Trnenvcodi);
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
        cells: function (row, col, prop) {
            //console.log("col:" + col + " row:" + row + " prop" + prop);
            var cellProperties = {};
            if (row == 0 || row == 1 || row == 2 || row == 3) {
                cellProperties.renderer = firstRowRendererCabecera;
            }
            else if (col == 0) {
                cellProperties.renderer = firstRowRendererCeleste;
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
            var recacodi = $('#recacodi').val()
            $.ajax({
                type: 'POST',
                url: sControlador + 'listarseleccionados',
                data: { pericodi: pericodi, recacodi: recacodi, items: items },
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
                    mostrarErrorDiv("Lo sentimos, ha ocurrido un error inesperado");
                }
            });
        }
        else {
            mostrarErrorDiv("No se ha seleccionado ningun registro");
        }
    }
    else {
        mostrarErrorDiv("No se ha seleccionado ningun registro");
    }
}

siLiquidacion = function () {
    var items = checkMark();
    var pericodi = $('#pericodi').val()
    var recacodi = $('#recacodi').val()
    $.ajax({
        type: 'POST',
        url: sControlador + 'siliquidacion',
        data: { pericodi: pericodi, recacodi: recacodi, items: items },
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

mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

mostrarErrorDiv = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

mostrarMensajeDiv = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}