// ASSETEC 2019-11
var controler = siteRoot + "transferencias/potenciacontratada/";
var error = [];
var hotConsulta;

$(document).ready(function ($) {
    $('#btnConsultar').click(function () {
        mostrarGrillaExcelConsulta();
    });

    $('#btnExportarExcel').click(function () {
        DescargarPotenciaContratada();
    });

    mostrarGrillaExcelConsulta();
});

// Procedimento que formatea y muestra los registros de la consulta de potencias contratadas en base del id de empresa
// y id de periodo
mostrarGrillaExcelConsulta = function () {
    if (ValidarPotenciasContratada()) {
        var idEmpresa = $('#cboEmpresa').val();
        var idCliente = $('#cboCliente').val();
        var idBarra = $('#cboBarra').val();
        var idPeriodo = $('#cboPeriodo').val();
        $.ajax({
            type: 'POST',
            url: controler + "GrillaExcelConsultar",
            data: { idEmpresa: idEmpresa, idPeriodo: idPeriodo, idCliente: idCliente, idBarra: idBarra },
            dataType: 'json',
            success: function (result) {
                if (result.NumRegistros == 0) {
                    var container = document.getElementById('grillaExcelConsulta');
                    container.innerHTML = "";
                    mostrarAlerta('Lo sentimos, no encontramos registros');
                }
                else {
                    if (typeof hotConsulta != 'undefined') {
                        hotConsulta.destroy();
                    }
                    var container = document.getElementById('grillaExcelConsulta');
                    calculateSizeHandsontable(container);
                    hotConsulta = new Handsontable(container, {
                        data: result.Data,
                        maxCols: result.Columnas.length,
                        colWidths: result.Widths,
                        columns: result.Columnas,
                        fixedRowsTop: result.FixedRowsTop,
                        //fixedColumnsLeft: result.FixedColumnsLeft,
                        currentRowClassName: 'currentRow',
                        mergeCells: [
                            { row: 0, col: 0, rowspan: 1, colspan: 7 },
                            { row: 0, col: 7, rowspan: 1, colspan: 2 },
                            { row: 0, col: 9, rowspan: 2, colspan: 1 },
                            { row: 0, col: 10, rowspan: 1, colspan: 3 },
                            { row: 0, col: 13, rowspan: 1, colspan: 3 },
                            { row: 0, col: 16, rowspan: 2, colspan: 1 },
                            { row: 0, col: 17, rowspan: 2, colspan: 1 },
                            { row: 0, col: 18, rowspan: 2, colspan: 1 }
                        ],
                        cells: function (row, col, prop) {
                            var cellProperties = {};
                            if (row == 0 || row == 1) {
                                if (col == 0 || col == 1 || col == 2 || col == 3 || col == 4 || col == 5 || col == 6 || col == 7 || col == 8 || col == 17 || col == 18) {
                                    cellProperties.renderer = FirstRowRendererCabecerasAzul;
                                }
                                else if (col == 9 || col == 10 || col == 11 || col == 12 || col == 13 || col == 14 || col == 15 || col == 16) {
                                    cellProperties.renderer = FirstRowRendererCabecerasAnaranjadas;
                                }
                            }
                            return cellProperties;
                        },
                    });
                    hotConsulta.render();
                    mostrarExito('Ya puede verificar la información de ' + result.NumRegistros + ' registros.');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText)
                mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo');
            }
        });
    }    
}

// Procedimento que descarga la plantilla con la cual contiene registros de solicitudes de retiro obtenidas por id de
// empresa de la base de datos
function DescargarPotenciaContratada() {
    if (ValidarPotenciasContratada()) {
        var emprcodi = $('#cboEmpresa').val();
        //var emprnomb = $('#cboEmpresa option:selected').html()
        var pericodi = $('#cboPeriodo').val();
        //var perinombre = $('#cboPeriodo option:selected').html()
        $.ajax({
            type: 'POST',
            url: controler + 'DescargarPotenciaContratada',
            data: { pericodi: pericodi, emprcodi: emprcodi },
            dataType: 'json',
            success: function (result) {
                if (result != -1) {
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: result }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivo', paramList);
                    document.body.appendChild(form);
                    form.submit();
                    mostrarExito("Ya puede consultar el documento");
                    return true;
                }
                else {
                    mostrarError('Ha ocurrido un error');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText)
                mostrarError('Ha ocurrido un error');
            }
        });
    }
}

// Valida los campos en blanco de la descarga del archivo Excel de portencias contratadas
function ValidarPotenciasContratada() {
    var rspta = true;

    //if ($('#cboEmpresa').val() == "0") {
    //    $('#cboEmpresa').css("border-color", "red");
    //    $('#msjErrEmpresas').css("color", "red");
    //    $('#msjErrEmpresas').html("Seleccione la empresa");
    //    rspta = false;
    //}
    //else {
    //    $('#cboEmpresa').css("border-color", "");
    //    $('#msjErrEmpresas').css("color", "red");
    //    $('#msjErrEmpresas').html("");
    //}
    if ($('#cboPeriodo').val() == "0") {
        $('#cboPeriodo').css("border-color", "red");
        $('#msjErrPeriodo').css("color", "red");
        $('#msjErrPeriodo').html("Seleccione el periodo");
        rspta = false;
    }
    else {
        $('#cboPeriodo').css("border-color", "");
        $('#msjErrPeriodo').css("color", "red");
        $('#msjErrPeriodo').html("");
    }
    return rspta;
}

mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

mostrarError = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

mostrarMensaje = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

// Formatea y colorea los encabezados del hansontable de azul
FirstRowRendererCabecerasAzul = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'white';
    td.style.background = '#3D8AB8';
    td.style.fontWeight = 'bold';
    cellProperties.className = "htCenter",
        cellProperties.readOnly = true;
}

// Formatea y colorea los encabezados del hansontable de anaranjado
FirstRowRendererCabecerasAnaranjadas = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'white';
    td.style.background = '#FF8000';
    td.style.fontWeight = 'bold';
    cellProperties.className = "htCenter",
        cellProperties.readOnly = true;
}

function CreateForm(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });

    return form;
}

function calculateSizeHandsontable(container) {
    var offset = Handsontable.Dom.offset(container);
    var iAltura = $(window).height() - offset.top - 50;
    var iAncho = $(window).width() - 150;
    //console.log($(window).height());
    //console.log(offset.top);
    //console.log(iAltura);
    container.style.height = iAltura + 'px';
    container.style.overflow = 'hidden';
    container.style.width = iAncho;
}