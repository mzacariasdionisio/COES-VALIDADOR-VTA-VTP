var controler = siteRoot + "transfpotencia/peajeegresominfo/";
var error = [];
$(document).ready(function () {
    $('#emprcodi').multipleSelect({
        width: '220px',
        filter: true,
        selectAll: false,
        single: true
    });

    $('#cliemprcodi').multipleSelect({
        width: '220px',
        filter: true,
        selectAll: false,
        single: true
    });

    $('#barrcodi').multipleSelect({
        width: '220px',
        filter: true,
        selectAll: false,
        single: true
    });

    $('#barrcodifco').multipleSelect({
        width: '220px',
        filter: true,
        selectAll: false,
        single: true
    });

    $('#btnConsultarVista').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        mostrarGrillaExcelConsulta();
    });

    $('#btnDescargarExcelConsulta').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoConsulta(1);
    });

    $('#btnDescargarPdfConsulta').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoConsulta(2);
    });

    //UploadExcel();

});

firstRowRendererCabecera = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'white';
    td.style.background = '#3D8AB8';
    td.style.fontWeight = 'bold';
    cellProperties.className = "htCenter",
    cellProperties.readOnly = true;
    //console.log(cellProperties);
}

firstRowRendererCeleste = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#E8F6FF';
}

firstRowRendererAmarillo = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#FFFFD7';
}

negativeValueRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    // if row contains negative number
    if (parseInt(value, 10) < 0) {
        // add class "negative"
        td.style.color = '#FA5858';
        td.style.fontWeight = 'bold';
    }
    if (!value || value === '') {
        td.style.background = '#E6E6E6';
    }
    else {
        td.style.background = '#FFFFFF';
    }
}

getCustomRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = '#2980B9';
    td.style.background = '#2980B9';
    //console.log(value);
}

Recargar = function () {
    var cmbPericodi = document.getElementById('pericodi');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value;
}

RecargarConsulta = function () {
    var cmbPericodi = document.getElementById('pericodiConsulta');
    window.location.href = controler + "index?pericodi=" + cmbPericodi.value;
}

mostrarGrillaExcelConsulta = function () {
    if (typeof hotConsulta != 'undefined') {
        hotConsulta.destroy();
    }
    var container = document.getElementById('grillaExcelConsulta');
    var pericodi = $('#pericodiConsulta').val();
    var recpotcodi = $('#recpotcodiConsulta').val();
    var emprcodi;
    var cliemprcodi;
    var barrcodi;
    var barrcodifco;
    var pegrmitipousuario = $('#pegrmitipousuario').val();
    var pegrmilicitacion = $('#pegrmilicitacion').val();
    var pegrmicalidad = $('#pegrmicalidad').val();
    //emprcodi
    if ($("#emprcodi").multipleSelect('getSelects') == "")
    { emprcodi = 0; }
    else
    { emprcodi = $("#emprcodi option:selected").val(); }
    //cliemprcodi
    if ($("#cliemprcodi").multipleSelect('getSelects') == "")
    { cliemprcodi = 0; }
    else
    { cliemprcodi = $("#cliemprcodi option:selected").val(); }
    //barrcodi
    if ($("#barrcodi").multipleSelect('getSelects') == "")
    { barrcodi = 0; }
    else
    { barrcodi = $("#barrcodi option:selected").val(); }
    //barrcodifco
    if ($("#barrcodifco").multipleSelect('getSelects') == "")
    { barrcodifco = 0; }
    else
    { barrcodifco = $("#barrcodifco option:selected").val(); }
    //console.log(emprcodi);
    $.ajax({
        type: 'POST',
        url: controler + "grillaexcelconsulta",
        data: { pericodi: pericodi, recpotcodi: recpotcodi, emprcodi: emprcodi, cliemprcodi: cliemprcodi, barrcodi: barrcodi, barrcodifco: barrcodifco, pegrmitipousuario: pegrmitipousuario, pegrmilicitacion: pegrmilicitacion, pegrmicalidad: pegrmicalidad },
        dataType: 'json',
        success: function (result) {
            var columns = result.Columnas;
            var widths = result.Widths;
            hotConsulta = new Handsontable(container, {
                data: result.Data,
                maxCols: result.Columnas.length,
                colHeaders: false,
                rowHeaders: true,
                colWidths: widths,
                contextMenu: false,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                currentRowClassName: 'currentRow',
                mergeCells: [{ row: 0, col: 0, rowspan: 2, colspan: 1 },
                             { row: 0, col: 1, rowspan: 2, colspan: 1 },
                             { row: 0, col: 2, rowspan: 2, colspan: 1 },
                             { row: 0, col: 3, rowspan: 2, colspan: 1 },
                             { row: 0, col: 4, rowspan: 2, colspan: 1 },
                             { row: 0, col: 5, rowspan: 1, colspan: 2 },
                             { row: 0, col: 7, rowspan: 1, colspan: 3 },
                             { row: 0, col: 10, rowspan: 1, colspan: 3 },
                             { row: 0, col: 13, rowspan: 2, colspan: 1 }],
                cells: function (row, col, prop) {
                    //console.log("col:" + col + " row:" + row + " prop" + prop);
                    var cellProperties = {};
                    if (row == 0 || row == 1) {
                        cellProperties.renderer = firstRowRendererCabecera;
                    }
                    else if (col == 0 || col == 1 || col == 2 || col == 3 || col == 4 || col == 13) {
                        cellProperties.renderer = firstRowRendererCeleste;
                    }
                    else if (col == 10 || col == 11 || col == 12) {
                        cellProperties.renderer = firstRowRendererAmarillo;
                    }
                    //else if (col >= 4) {
                    //    //Para el llenado de datos
                    //    cellProperties.renderer = "negativeValueRenderer";
                    //}
                    return cellProperties;
                },
            });
            $('#divAccionesConsulta').css('display', 'block');
            var iNumRegistros = hotConsulta.countRows();
            if (iNumRegistros >= 2) iNumRegistros = iNumRegistros - 3;
            mostrarExito('Se han encontrado ' + iNumRegistros + ' registro(s)');
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

descargarArchivoConsulta = function (formato) {
    var pericodi = $('#pericodiConsulta').val();
    var recpotcodi = $('#recpotcodiConsulta').val();
    var emprcodi;
    var cliemprcodi;
    var barrcodi;
    var barrcodifco;
    var pegrmitipousuario = $('#pegrmitipousuario').val();
    var pegrmilicitacion = $('#pegrmilicitacion').val();
    var pegrmicalidad = $('#pegrmicalidad').val();
    //emprcodi
    if ($("#emprcodi").multipleSelect('getSelects') == "")
    { emprcodi = 0; }
    else
    { emprcodi = $("#emprcodi option:selected").val(); }
    //cliemprcodi
    if ($("#cliemprcodi").multipleSelect('getSelects') == "")
    { cliemprcodi = 0; }
    else
    { cliemprcodi = $("#cliemprcodi option:selected").val(); }
    //barrcodi
    if ($("#barrcodi").multipleSelect('getSelects') == "")
    { barrcodi = 0; }
    else
    { barrcodi = $("#barrcodi option:selected").val(); }
    //barrcodifco
    if ($("#barrcodifco").multipleSelect('getSelects') == "")
    { barrcodifco = 0; }
    else
    { barrcodifco = $("#barrcodifco option:selected").val(); }
    $.ajax({
        type: 'POST',
        url: controler + 'exportardata',
        data: { pericodi: pericodi, recpotcodi: recpotcodi, emprcodi: emprcodi, formato: formato, cliemprcodi: cliemprcodi, barrcodi: barrcodi, barrcodifco: barrcodifco, pegrmitipousuario: pegrmitipousuario, pegrmilicitacion: pegrmilicitacion, pegrmicalidad: pegrmicalidad },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = controler + 'abrirarchivo?formato=' + formato + '&file=' + result;
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}