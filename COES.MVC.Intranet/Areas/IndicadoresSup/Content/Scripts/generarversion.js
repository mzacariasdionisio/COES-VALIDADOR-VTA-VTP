var controlador = siteRoot + 'IndicadoresSup/numeral/';
$(function () {

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y',
        direction: -1,
        onSelect: function () {
            consultar();
        }
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnGenerar').on('click', function () {
        var numeral = parseInt($('#cbNumeral').val()) || 0;
        if (numeral == 11) {
            cargarPrecalculoEnergiaForzada(0);
        }
        else
            generarVersion();
    });

    $('#cbNumeral').on('change', function () {
        consultar();
    });

    $("#btnGuardarGenForzada").click(function () {
        guardarGenForzada();
    });

    consultar();
});

function consultar() {
    var numeral = $('#cbNumeral').val();
    if (numeral === "1" || numeral === "11" || numeral === "6") {
        $("#btnExportDetail").css("display", "block");
    } else {
        $("#btnExportDetail").css("display", "none");
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'listagenerarversion',
        data: {
            fecha: $('#txtFecha').val(),
            numeral: $('#cbNumeral').val()
        },
        success: function (evt) {
            $('#listado').html(evt);
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function generarVersion() {

    $.ajax({
        type: 'POST',
        url: controlador + 'generaversion',
        dataType: 'json',
        data: {
            periodo: $('#txtFecha').val(),
            numeral: $('#cbNumeral').val()
        },
        success: function (evt) {

            if (evt.Resultado === "1") {
                consultar();
                mostrarMensaje('mensaje', 'exito', evt.Mensaje);
            } else {

                mostrarMensaje('mensaje', 'error', 'Se ha producido un error. ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });

}

function cargarPrecalculoEnergiaForzada(version) {
    mostrarMensaje('mensaje', '', '');
    dataNumeral511 = [];
    listaErrorTblEnergForz = [];

    //$("#excelwebNumeral").hide();
    $(".leyenda_excel").hide();

    $.ajax({
        url: controlador + "GenerarExcelwebNumeral511",
        data: {
            periodo: $('#txtFecha').val(),
            numeral: $('#cbNumeral').val(),
            verncodi: version
        },
        type: 'POST',
        success: function (result) {
            if (result.Resultado == "-1") {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            } else {
                setTimeout(function () {
                    $('#popupExcelWeb').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 50);

                dataNumeral511 = result.ListaPrecalculoEnergiaForzada;

                generarUIExcelWeb();
                setTimeout(function () {
                    if (tblEnergForz !== undefined) 
                        tblEnergForz.loadData([]);
                    if (version > 0) {
                        $("#btnGuardarGenForzada").hide();
                        desabilitarHansontables(tblEnergForz);
                    }
                    else {
                        $("#btnGuardarGenForzada").show();
                        habilitarHansontables(tblEnergForz);
                    }

                    cargarHansonTable(tblEnergForz, result.ListaPrecalculoEnergiaForzada);
                    updateDimensionHandson(tblEnergForz, containerEnergForz);
                }, 450);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function guardarGenForzada() {
    if (dataNumeral511.length > 0) {
        if (listaErrorTblEnergForz.length === 0) {
            var dataHandson, dataValido = [];
            dataHandson = tblEnergForz.getSourceData();
            dataValido = obtenerDataValido(dataHandson);

            if (dataValido.length <= 0) {
                mostrarMensaje('mensajePopup', 'alert', 'No existe como mínino un registro completo.');
                return;
            }

            var dataJson = {
                periodo: $('#txtFecha').val(),
                numeral: $('#cbNumeral').val(),
                listaEnergiaForzada: dataValido
            };

            $.ajax({
                url: controlador + "EnvioEnergiaForzada",
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify(dataJson),
                success: function (result) {

                    if (result.Resultado == "-1") {
                        mostrarMensaje('mensajePopup', result.Resultado, result.Mensaje);
                    } else {
                        $('#popupExcelWeb').bPopup().close();

                        consultar();
                        mostrarMensaje('mensaje', 'exito', result.Mensaje);
                    }
                },
                error: function (xhr, status) {
                    mostrarMensaje('mensajePopup', 'error', 'Se ha producido un error.');
                }
            });

        } else {
            mostrarMensaje('mensajePopup', 'error', 'Existen errores en las celdas, favor de corregir y vuelva a envíar.');
        }
    } else {
        mostrarMensaje('mensajePopup', 'error', 'No existen registros.');
    }
}

function generarDetalleVersion() {

    $.ajax({
        type: 'POST',
        url: controlador + 'Generadetalleversion',
        dataType: 'json',
        data: {
            periodo: $('#txtFecha').val(),
            numeral: $('#cbNumeral').val()
        },
        success: function (evt) {
            switch (evt.NroEstadoNum) {
                case 1: window.location = controlador + "Exportar?fi=" + evt.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert("Error en reporte result"); break;// Error en C#
            }
        },
        error: function (err) { mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');; }
    });

}

function mostrarNumeral(verncodi, numeral, fecha) {

    $.ajax({
        type: 'POST',
        url: controlador + 'listanumeral',
        data: {
            verncodi: verncodi,
            fecha: fecha,
            numeral: numeral
        },
        success: function (evt) {
            $('#listado').html(evt);
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function validarNumeral(verncodi) {

    if (confirm('¿Confirmar validación de versión?')) {

        $.ajax({
            type: 'POST',
            url: controlador + 'ValidaVersionNumeral',
            data: { verncodi: verncodi },
            success: function (evt) {

                if (evt.Resultado === "1") {
                    consultar();
                    mostrarMensaje('mensaje', 'exito', evt.Mensaje);
                } else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            error: function (err) {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function exportaVersionNumerales(verncodi, numeral, fecha) {
    $.ajax({
        type: 'POST',
        url: controlador + 'ExportaVersionNumeral',
        data: {
            verncodi: verncodi,
            fecha: fecha,
            numeral: numeral
        },
        success: function (evt) {
            switch (evt.NroEstadoNum) {
                case 1: window.location = controlador + "Exportar?fi=" + evt.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert("Error en exportación del reporte. " + evt.Mensaje); break;// Error en C#
            }
        },
        error: function (err) { mostrarMensaje('mensaje', 'error', 'Se ha producido un error.'); }
    });
}

mostrarMensaje = function (id, tipo, mensaje) {
    $('#' + id).show("slow");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

    setTimeout(function () {
        $('#' + id).hide(2000);
    }, 5000);
};

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//

var tblEnergForz;
var containerEnergForz;
var listaErrorTblEnergForz = [];
var dataNumeral511 = [];
var HEIGHT_MINIMO = 250;
var HEIGHT_MAXIMO = 550;

function generarUIExcelWeb() {
    containerEnergForz = document.getElementById('tblEnergForz');

    tblEnergForz = new Handsontable(containerEnergForz, {
        dataSchema: {
            Ptomedidesc: null,
            Tipo: null,
            Gruponomb: null,
            Genforhorini2: null,
            Genforhorfin2: null,
            Numhoras: null,
            PotenciaPromedio: null,
            Energiaforzada: null
        },
        colHeaders: ['Zona', 'Tipo', 'Modo de operación', 'Fecha Inicio', 'Fecha Fin', '# Horas', 'Pot. prom(MW)', 'Energ. Forzada(MWh)'],
        columns: [
            { data: 'Ptomedidesc', editor: false, className: 'soloLectura' },
            { data: 'Tipo', editor: false, className: 'soloLectura' },
            { data: 'Gruponomb', editor: false, className: 'soloLectura' },
            { data: 'Genforhorini2', editor: false, type: 'date', dateFormat: 'DD/MM/YYYY HH:mm:ss', correctFormat: true, className: 'soloLectura' },
            { data: 'Genforhorfin2', editor: false, type: 'date', dateFormat: 'DD/MM/YYYY HH:mm:ss', correctFormat: true, className: 'soloLectura' },
            { data: 'Numhoras', type: 'numeric', format: '0.00', editor: false, className: 'soloLectura htRight' },
            { data: 'PotenciaPromedio', type: 'numeric', format: '0.00000', className: 'htRight', renderer: potenciaRenderer },
            { data: 'Energiaforzada', type: 'numeric', editor: false, format: '0.00000', className: 'htRight', className: 'soloLectura', renderer: energiaRenderer }
        ],
        colWidths: [60, 150, 200, 140, 140, 70, 120, 120],
        columnSorting: true,
        minSpareRows: 0,
        rowHeaders: true,
        autoWrapRow: true,
        startRows: 0,
        manualColumnResize: false
    });

    tblEnergForz.addHook('afterRender', function () {
        tblEnergForz.validateCells();
    });

    tblEnergForz.addHook('afterRenderer', function (TD, row, column, prop, value, cellProperties) {
    });

    tblEnergForz.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrorTblEnergForz = [];
    });

    tblEnergForz.addHook('afterValidate', function (isValid, value, row, prop, source) {
        if (prop === "PotenciaPromedio") {
            var result = potenciaValidator(this, isValid, value, row, prop, 0, 1000);
            return registrarErrores(result);
        }
    });
}

function potenciaValidator(instance, isValid, value, row, prop, valMin, valMax) {
    var error = [];

    var columnName = instance.getColHeader(instance.propToCol(prop));

    var className, mensaje;

    if ($.isNumeric(value) && isValid) {

        if (value < valMin) {
            className = "errorLimitInferior";
            mensaje = "El dato es menor que el límite inferior.";
            isValid = false;
        }

        if (value >= valMax) {
            className = "errorLimitSuperior";
            mensaje = "El dato es mayor que el límite superior.";
            isValid = false;
        }

        error = {
            address: `[<b>${columnName}</b>] [<b>${row + 1}</b>]`,
            valor: value,
            className: className,
            message: mensaje
        };
        instance.setCellMeta(row, instance.propToCol(prop), "invalidCellClassName", className);

    } else if (value && !$.isNumeric(value)) {

        className = "htInvalid";
        mensaje = "El dato no es numérico";

        //quitar <br>
        var regex = /<br\s*[\/]?>/gi;
        columnName = columnName != null ? columnName.replace(regex, "") : "";

        error = {
            address: `[<b>${columnName}</b>] [<b>${row + 1}</b>]`,
            valor: value,
            className: className,
            message: mensaje
        };
        instance.setCellMeta(row, instance.propToCol(prop), "invalidCellClassName", className);
    }
    return { valid: isValid, error: error };
}

function potenciaRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.TextCell.renderer.apply(this, arguments);

    //Handsontable.renderers.TextRenderer.apply(this, arguments);
    //var stfechaIni = instance.getDataAtCell(row, col - 2);
    //var stfechaFin = instance.getDataAtCell(row, col - 1);

    //if (moment(stfechaIni, 'DD/MM/YYYY HH:mm:ss').isValid() && moment(stfechaFin, 'DD/MM/YYYY HH:mm:ss').isValid()) {

    //    var fechaIni = moment(stfechaIni, 'DD/MM/YYYY HH:mm:ss');
    //    var fechaFin = moment(stfechaFin, 'DD/MM/YYYY HH:mm:ss');
    //    var duracionMinuto = fechaFin.diff(fechaIni, 'minutes', true);

    //    td.innerHTML = duracionMinuto.toFixed(2);
    //}

    var potProm = parseFloat(instance.getDataAtCell(row, col)) || 0;
    td.innerHTML = potProm.toFixed(5);

    if (dataNumeral511.length > 0) {
        if (dataNumeral511[row].PotenciaPromedio > 0) {
            cellProperties.readOnly = true;
            cellProperties.className = 'soloLectura htRight';
        }
    }
}

function energiaRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    var numHoras = parseFloat(instance.getDataAtCell(row, col - 2)) || 0;
    var potProm = parseFloat(instance.getDataAtCell(row, col - 1)) || 0;

    td.innerHTML = 0;
    if (numHoras > 0 && potProm > 0) {
        var energia = numHoras * potProm;

        td.innerHTML = energia.toFixed(5);
    }
    cellProperties.readOnly = true;
    cellProperties.className = 'soloLectura htRight';
}

function registrarErrores(result) {
    if (!result.valid)
        listaErrorTblEnergForz.push(result.error);
    return result.valid;
}

function updateDimensionHandson(hot, container) {
    if (hot !== undefined && hot != null) {
        var offset = {};
        try {
            offset = Handsontable.Dom.offset(container);
        }
        catch (err) {
            console.log(err);
        }

        if (offset.length != 0) {
            var widthHT;
            var heightHT;
            var offsetTop = parseInt(offset.top) || 0;

            if (offset.top == 222) {
                heightHT = $(window).height() - 140 - offset.top - 20;
            }
            else {
                heightHT = $(window).height() - 140 - offset.top - 20;
            }

            console.log(heightHT);
            heightHT = heightHT > HEIGHT_MAXIMO ? HEIGHT_MAXIMO : heightHT;
            heightHT = heightHT < HEIGHT_MINIMO ? HEIGHT_MINIMO : heightHT;
            heightHT += 20;
            if (offset.left > 0 && offset.top > 0) {
                widthHT = $(window).width() - 2 * offset.left; // $("#divGeneral").width() - 50; //
                hot.updateSettings({
                    width: widthHT,
                    height: heightHT
                });
            } else {
                console.log('updateDimensionHandson');
            }
        }
    }
}

function desabilitarHansontables(hanson) {
    hanson.updateSettings({
        readOnly: true
    });
}

function habilitarHansontables(hanson) {
    hanson.updateSettings({
        readOnly: false
    });
}

function cargarHansonTable(tbl, inputJson) {
    var lst = inputJson && inputJson.length > 0 ? inputJson : [];

    var lstData = [];
    tbl.loadData(lstData);

    for (var index in lst) {
        var item = lst[index];

        var data = {
            Ptomedidesc: item.Ptomedidesc,
            Tipo: item.Tipo,
            Gruponomb: item.Gruponomb,
            Genforhorini2: item.Genforhorini2,
            Genforhorfin2: item.Genforhorfin2,
            Numhoras: item.Numhoras,
            PotenciaPromedio: item.PotenciaPromedio,
            Energiaforzada: item.Energiaforzada
        };

        lstData.push(data);
    }

    tbl.loadData(lstData);
}

function obtenerDataValido(tbl) {

    for (var i = 0; i < dataNumeral511.length; i++) {
        var regFila = tbl[i];
        if (dataNumeral511[i].PotenciaPromedio > 0) { }
        else {
            dataNumeral511[i].PotenciaPromedio = regFila.PotenciaPromedio;
        }
    }

    return dataNumeral511;
}