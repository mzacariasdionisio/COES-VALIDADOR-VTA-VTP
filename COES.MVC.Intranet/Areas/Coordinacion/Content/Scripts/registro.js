var controlador = siteRoot + 'coordinacion/cambioturno/'
var hot = null;
var titulos = null;
var subTitulos = null;
var mantenimientos = null;
var agrupaciones = null;
var comentarios = null;
var adicionales = null;
var finales = null;
var merge = null;
var intervalo = null;
var errors = null;
var fechaConsulta = "";

$(document).ready(function () {

    $('#txtFecha').Zebra_DatePicker({
        pair: $('#txtFechaTo'),
        direction: false
    });
    $('#txtFechaTo').Zebra_DatePicker({

    });

    $('#btnConsultar').click(function () {
        if (hot != null) {
            hot.destroy();
        }

        cargarGrilla();
        $('#mensaje').removeClass();
        $('#mensaje').addClass('action-message');
        $('#mensaje').text('Verifique los datos y luego presione Grabar');
    });

    $('#btnGrabar').click(function () {
        grabar();
    });

    $('#btnExcel').click(function () {
        exportar(1);
    });

    $('#btnPDF').click(function () {
        exportar(2);
    });

    $('#btnAuditoria').click(function () {
        auditoria();
    });

});

cargarGrilla = function () {

    var fecha = $('#txtFecha').val();
    var turno = $('#cbTurno').val();

    if (fecha != "" && turno != "") {

        $('#btnGrabar').css('display', 'block');

        $.ajax({
            type: 'POST',
            url: controlador + 'obtenerdatagrilla',
            datatype: 'json',
            async: false,
            data: {
                fecha: fecha,
                turno: turno
            },
            success: function (result) {
                errors = [];
                clearInterval(intervalo);
                fechaConsulta = result.Fecha;
                $('#hfIdCambioTurno').val(result.Indicador);
                $('#hfIdNumReprogramas').val(result.NumReprogramas);
                if (result.Indicador > 0) {
                    $('#btnExcel').css('display', 'block');
                    $('#btnPDF').css('display', 'block');
                    $('#btnAuditoria').css('display', 'block');
                }
                else {
                    $('#btnExcel').css('display', 'none');
                    $('#btnPDF').css('display', 'none');
                    $('#btnAuditoria').css('display', 'none');
                }

                var container = document.getElementById('contenedor');

                var subTituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.backgroundColor = '#D7EFEF';
                    td.style.textAlign = 'center';
                    td.style.fontWeight = 'bold';
                    td.style.color = '#1C91AE';
                    td.style.verticalAlign = 'middle';
                };

                var tituloRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.fontWeight = 'bold';
                    td.style.textAlign = 'left';
                    td.style.color = '#fff';
                    td.style.backgroundColor = '#1991B5';
                };

                var reprogramasRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.fontWeight = 'bold';
                    td.style.textAlign = 'center';
                    //td.style.color = '#fff';
                    //td.style.backgroundColor = '#1991B5';
                };

                var agrupacionRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.fontWeight = 'bold';
                    td.style.textAlign = 'left';
                    td.style.color = '#AD6500';
                    td.style.backgroundColor = '#FFEB9C';
                };

                var comentarioRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.textAlign = 'left';
                    td.style.color = '#EA9140';
                };

                var finalRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.backgroundColor = '#F2F2F2';
                    td.style.textAlign = 'center';
                    td.style.fontWeight = 'bold';
                    td.style.color = '#FA7D00';
                    td.style.verticalAlign = 'middle';
                };

                var defaultRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.backgroundColor = '#FFF';
                    td.style.fontSize = '11px';
                };

                var defaultRendererDerecho = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.backgroundColor = '#FFF';
                    td.style.fontSize = '11px';
                    td.style.textAlign = 'right';
                };

                var defaultRendererCentro = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.backgroundColor = '#FFF';
                    td.style.fontSize = '11px';
                    td.style.textAlign = 'center';
                };

                var errorRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.backgroundColor = 'red';
                };

                var mantenimientoRenderer = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.backgroundColor = '#EAFFEA';
                };

                var mantenimientoRendererCentro = function (instance, td, row, col, prop, value, cellProperties) {
                    Handsontable.renderers.TextRenderer.apply(this, arguments);
                    td.style.backgroundColor = '#EAFFEA';
                    td.style.textAlign = 'center';
                };

                var data = result.Datos;
                titulos = result.IndicesTitulo;
                subTitulos = result.IndicesSubtitulo;
                agrupaciones = result.IndicesAgrupacion;
                comentarios = result.IndicesComentario;
                adicionales = result.IndicesAdicional;
                mantenimientos = result.IndiceMantenimiento;
                finales = result.IndicesFinal;
                merge = result.Merge;
                validaciones = result.Validaciones;
                longitudes = result.Longitudes;
                centros = result.Centros;
                derechos = result.Derechos;
                reprogr = result.Reprog;


                $('#cbCoordinador').val(result.IdPersona);

                var hotOptions = {
                    data: data,
                    colHeaders: false,
                    rowHeaders: true,
                    comments: true,
                    colWidths: [260, 110, 110, 110, 110, 110, 110, 110, 110, 110],
                    cells: function (row, col, prop) {

                        var cellProperties = {};

                        cellProperties.renderer = defaultRenderer;

                        if (titulos.indexOf(row) != -1) {
                            cellProperties.renderer = tituloRenderer;
                            cellProperties.readOnly = true;
                        }
                        if (subTitulos.indexOf(row) != -1) {
                            cellProperties.renderer = subTituloRenderer;
                            cellProperties.readOnly = true;
                        }
                        if (agrupaciones.indexOf(row) != -1) {
                            cellProperties.renderer = agrupacionRenderer;
                            cellProperties.readOnly = true;
                        }
                        if (comentarios.indexOf(row) != -1) {
                            cellProperties.renderer = comentarioRenderer;
                        }
                        if (adicionales.indexOf(row) != -1) {
                            cellProperties.fillHandle = false;
                        }
                        if (finales.indexOf(row) != -1) {
                            cellProperties.renderer = finalRenderer;
                            cellProperties.readOnly = true;
                        }
                        if (mantenimientos.indexOf(row) != -1) {
                            cellProperties.renderer = mantenimientoRenderer;
                            cellProperties.readOnly = false;
                        }

                        for (var i in validaciones) {
                            if (validaciones[i]['Row'] == row && validaciones[i]['Column'] == col) {
                                cellProperties.type = 'dropdown';
                                cellProperties.source = validaciones[i].Elementos;
                                break;
                            }
                        }

                        for (var i in reprogr) {
                            if (reprogr[i]['Row'] == row && reprogr[i]['Column'] == col) {
                                cellProperties.renderer = reprogramasRenderer;
                                cellProperties.readOnly = true;

                            }
                        }

                        for (var i in longitudes) {
                            if (row == longitudes[i]['Row'] && col == longitudes[i]['Column']) {
                                this.maxLength = longitudes[i]['Longitud'];
                            }
                        }

                        for (var i in centros) {
                            if (row == centros[i]['Row'] && col == centros[i]['Column']) {

                                if (mantenimientos.indexOf(row) != -1) {
                                    cellProperties.renderer = mantenimientoRendererCentro;
                                    cellProperties.readOnly = false;
                                }
                                else {
                                    cellProperties.renderer = defaultRendererCentro;
                                }
                            }
                        }

                        for (var i in derechos) {
                            if (row == derechos[i]['Row'] && col == derechos[i]['Column']) {
                                cellProperties.renderer = defaultRendererDerecho;
                            }
                        }

                        for (var i in errors) {
                            if (row == errors[i]['row'] && col == errors[i]['column']) {
                                cellProperties.renderer = errorRenderer;
                            }
                        }

                        return cellProperties;

                    },
                    mergeCells: merge,
                    afterCreateRow: function (index, amount) {

                        for (var i in titulos) {
                            if (index <= titulos[i]) {
                                titulos[i] = titulos[i] + 1;
                            }
                        }
                        for (var i in subTitulos) {
                            if (index <= subTitulos[i]) {
                                subTitulos[i] = subTitulos[i] + 1;
                            }
                        }
                        for (var i in agrupaciones) {
                            if (index <= agrupaciones[i]) {
                                agrupaciones[i] = agrupaciones[i] + 1;
                            }
                        }
                        for (var i in comentarios) {
                            if (index <= comentarios[i]) {
                                comentarios[i] = comentarios[i] + 1;
                            }
                        }
                        for (var i in adicionales) {
                            if (index <= adicionales[i]) {
                                adicionales[i] = adicionales[i] + 1;
                            }
                        }
                        for (var i in finales) {
                            if (index <= finales[i]) {
                                finales[i] = finales[i] + 1;
                            }
                        }

                        for (var i in mantenimientos) {
                            if (index <= mantenimientos[i]) {
                                mantenimientos[i] = mantenimientos[i] + 1;
                            }
                        }

                        var newMerge = [];
                        for (var i in merge) {
                            if (merge[i]['row'] == index - 1) {
                                newMerge.push({ row: index, col: merge[i]['col'], rowspan: merge[i]['rowspan'], colspan: merge[i]['colspan'] });
                            }
                        }

                        for (var j in newMerge) {
                            merge.push(newMerge[j]);
                        }

                        var newValidaciones = [];
                        for (var i in validaciones) {
                            if (validaciones[i]['Row'] == index - 1) {
                                newValidaciones.push({ Row: index, Column: validaciones[i]['Column'], Elementos: validaciones[i]['Elementos'] });
                            }
                            if (validaciones[i]['Row'] > index - 1) {
                                validaciones[i]['Row'] = validaciones[i]['Row'] + 1;
                            }
                        }

                        for (var j in newValidaciones) {
                            validaciones.push(newValidaciones[j]);
                        }

                        var newLongitudes = [];
                        for (var i in longitudes) {
                            if (longitudes[i]['Row'] == index - 1) {
                                newLongitudes.push({ Row: index, Column: longitudes[i]['Column'], Longitud: longitudes[i]['Longitud'] });
                            }
                            if (longitudes[i]['Row'] > index - 1) {
                                longitudes[i]['Row'] = longitudes[i]['Row'] + 1;
                            }
                        }

                        for (var j in newLongitudes) {
                            longitudes.push(newLongitudes[j]);
                        }


                        var newCentros = [];
                        for (var i in centros) {
                            if (centros[i]['Row'] == index - 1) {
                                newCentros.push({ Row: index, Column: centros[i]['Column'] });
                            }
                            if (centros[i]['Row'] > index - 1) {
                                centros[i]['Row'] = centros[i]['Row'] + 1;
                            }
                        }

                        for (var j in newCentros) {
                            centros.push(newCentros[j]);
                        }


                        var newDerechos = [];
                        for (var i in derechos) {
                            if (derechos[i]['Row'] == index - 1) {
                                newDerechos.push({ Row: index, Column: derechos[i]['Column'] });
                            }
                            if (derechos[i]['Row'] > index - 1) {
                                derechos[i]['Row'] = derechos[i]['Row'] + 1;
                            }
                        }

                        for (var j in newDerechos) {
                            derechos.push(newDerechos[j]);
                        }


                        for (var i in errors) {
                            if (errors[i]['row'] > index - 1) {
                                errors[i]['row'] = errors[i]['row'] + 1;
                            }
                        }

                        hotOptions.mergeCells = merge;
                        hot.mergeCells = new Handsontable.MergeCells(hotOptions.mergeCells);
                        hot.updateSettings(hotOptions);
                    },
                    afterRemoveRow: function (index, amount) {

                        for (var i in titulos) {
                            if (index < titulos[i] && index > 2) {
                                titulos[i] = titulos[i] - 1;
                            }
                        }
                        for (var i in subTitulos) {
                            if (index < subTitulos[i] && index > 2) {
                                subTitulos[i] = subTitulos[i] - 1;
                            }
                        }
                        for (var i in agrupaciones) {
                            if (index < agrupaciones[i] && index > 2) {
                                agrupaciones[i] = agrupaciones[i] - 1;
                            }
                        }
                        for (var i in comentarios) {
                            if (index < comentarios[i] && index > 2) {
                                comentarios[i] = comentarios[i] - 1;
                            }
                        }
                        for (var i in adicionales) {
                            if (index < adicionales[i] && index > 2) {
                                adicionales[i] = adicionales[i] - 1;
                            }
                        }
                        for (var i in finales) {
                            if (index < finales[i] && index > 2) {
                                finales[i] = finales[i] - 1;
                            }
                        }

                        for (var i in mantenimientos) {
                            if (index < mantenimientos[i] && index > 2) {
                                mantenimientos[i] = mantenimientos[i] - 1;
                            }
                        }

                        for (var i in validaciones) {
                            if (validaciones[i]['Row'] == index - 1) {
                                validaciones.splice(i, 1);
                            }
                            else if (validaciones[i]['Row'] > index - 1) {
                                validaciones[i]['Row'] = validaciones[i]['Row'] - 1;
                            }
                        }


                        for (var i in longitudes) {
                            if (longitudes[i]['Row'] == index - 1) {
                                longitudes.splice(i, 1);
                            }
                            else if (longitudes[i]['Row'] > index - 1) {
                                longitudes[i]['Row'] = longitudes[i]['Row'] - 1;
                            }
                        }

                        for (var i in derechos) {
                            if (derechos[i]['Row'] == index - 1) {
                                derechos.splice(i, 1);
                            }
                            else if (derechos[i]['Row'] > index - 1) {
                                derechos[i]['Row'] = derechos[i]['Row'] - 1;
                            }
                        }

                        for (var i in centros) {
                            if (centros[i]['Row'] == index - 1) {
                                centros.splice(i, 1);
                            }
                            else if (centros[i]['Row'] > index - 1) {
                                centros[i]['Row'] = centros[i]['Row'] - 1;
                            }
                        }

                        for (var i in errors) {
                            if (errors[i]['row'] == index - 1) {
                                errors.splice(i, 1);
                            }
                            else if (errors[i]['row'] > index - 1) {
                                errors[i]['row'] = errors[i]['row'] - 1;
                            }
                        }
                    },
                    afterLoadData: function () {
                        this.render();
                    },
                    afterValidate: function (isValid, value, row, prop, source) {
                        if (value != "") {
                            if (!isValid) {
                                errors.push({ row: row, column: prop });
                            }
                            else {
                                for (var j in errors) {
                                    if (errors[j]['row'] == row && errors[j]['column'] == prop) {
                                        errors.splice(j, 1);
                                    }
                                }
                            }
                        }
                        else {
                            for (var j in errors) {
                                if (errors[j]['row'] == row && errors[j]['column'] == prop) {
                                    errors.splice(j, 1);
                                }
                            }
                            return true;
                        }
                        ////Valida si existe archivo ATR
                        //if (value.includes('Reprograma')) {  
                        //    var suma = 0;
                        //    var valor = value + "";
                        //    var fecha0 = fechaConsulta;
                        //    var tipoValidacion = 1; //valida cada celda
                        //    llenarCeldaArchivoATR(fechaConsulta, valor, tipoValidacion);
                        //}
                    }
                };

                hot = new Handsontable(container, hotOptions);

                hot.updateSettings({
                    contextMenu: {
                        callback: function (key, options) {
                            if (key === 'about') {
                                setTimeout(function () {
                                    alert("This is a context menu with default and custom options mixed");
                                }, 100);
                            }
                        },
                        items: {
                            "row_below": {
                                name: 'Agregar fila',
                                disabled: function () {
                                    return (subTitulos.indexOf(hot.getSelected()[0]) > -1 || titulos.indexOf(hot.getSelected()[0]) > -1
                                        || agrupaciones.indexOf(hot.getSelected()[0]) > -1 || comentarios.indexOf(hot.getSelected()[0]) > -1
                                        || adicionales.indexOf(hot.getSelected()[0]) > -1);
                                }
                            },
                            "remove_row": {
                                name: 'Eliminar fila',
                                disabled: function () {
                                    return (subTitulos.indexOf(hot.getSelected()[0]) > -1 || titulos.indexOf(hot.getSelected()[0]) > -1
                                        || agrupaciones.indexOf(hot.getSelected()[0]) > -1 || comentarios.indexOf(hot.getSelected()[0]) > -1
                                        || adicionales.indexOf(hot.getSelected()[0]) > -1
                                        || hot.getSelected().length == 1);
                                }
                            }
                        }
                    }
                });

                if (result.Indicador == 0) {
                    hot.setDataAtCell(hot.countRows() - 4, 8, obtenerHora());
                    intervalo = setInterval(function () {
                        if (hot != null) {
                            hot.setDataAtCell(hot.countRows() - 4, 8, obtenerHora());
                        }
                    }, 1000);
                }

            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta("Seleccione fecha y turno")
    }
}

obtenerHora = function () {
    var d = new Date();
    var h = ("0" + d.getHours()).slice(-2);
    var m = ("0" + d.getMinutes()).slice(-2);
    var s = ("0" + d.getSeconds()).slice(-2);

    return h + ":" + m + ":" + s;
}

grabar = function () {

    var fecha = $('#txtFecha').val();
    var turno = $('#cbTurno').val();
    var responsable = $('#cbCoordinador').val();

    if (fecha != "" && turno != "" && responsable != "") {

        validacionGrabarATR(fecha, turno, responsable);

    }
    else {
        mostrarAlerta("Seleccione fecha, turno y responsable.");
    }
}

function validacionGrabarATR(fecha, turno, responsable) {

    var esNuevo = esNuevoCambioTurno();
    var lstAgrupados = obtenerAgrupaciones();

    verificarExistenciaDeArchivosATR(fechaConsulta, lstAgrupados, esNuevo);

}


function verificarExistenciaDeArchivosATR(fechaConsulta, values, esNuevo) {

    var numReprogramas = $('#hfIdNumReprogramas').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'VerificarSoloExistenciaDeArchivosATR',
        datatype: 'json',
        //async: false,
        data: {
            esNuevoHT: esNuevo,
            numReprogramas: numReprogramas,
            fechaConsulta: fechaConsulta,
            agrupados: values
        },
        success: function (evt) {
            var excep_mensaje = evt.StrMensaje;
            var opcion = evt.Indicador;

            if (opcion != -1) {

                if (opcion == 1) { // Todas tienen archivo ATR

                    if (esNuevo) {

                        grabarHojaCambio(evt.DataEnvio, esNuevo);

                        var nuevo = esNuevoCambioTurno();
                        var lstAgrupados = obtenerAgrupaciones();
                        llenarCeldaArchivoATR(fechaConsulta, lstAgrupados, esNuevo)
                    } else {

                        var nuevo = esNuevoCambioTurno();
                        var lstAgrupados = obtenerAgrupaciones();
                        llenarCeldaArchivoATR(fechaConsulta, lstAgrupados, esNuevo)
                    }

                } else {
                    if (opcion == 0) { //Hay al menos uno sin archivo ATR

                        $('#popupSaveReprog').bPopup({
                            easing: 'easeOutBack',
                            speed: 450,
                            transition: 'slideDown',
                            opacity: 0.8,
                        });


                        $('#btnSaveNo').click(function () {
                            cerrarPopUpSave();
                        });

                        $('#btnSaveSi').click(function () {
                            if (esNuevo) {

                                grabarHojaCambio(evt.DataEnvio, esNuevo);

                                var nuevo = esNuevoCambioTurno();
                                var lstAgrupados = obtenerAgrupaciones();
                                llenarCeldaArchivoATR(fechaConsulta, lstAgrupados, esNuevo)
                            } else {

                                var nuevo = esNuevoCambioTurno();
                                var lstAgrupados = obtenerAgrupaciones();
                                llenarCeldaArchivoATR(fechaConsulta, lstAgrupados, esNuevo)
                            }

                        });
                    } else {
                        if (opcion == 2) { // No tiene reprogramas (0 reprogramas)
                            grabarHojaCambio("", false);
                        } else {
                            if (opcion == 3) { // SiCambioTurno existente pero no se puede verificar su tipo
                                alert("No se puede identificar los tipos de Reprograma.");
                                grabarHojaCambio("", false);
                            }
                        }
                    }
                }

            } else {
                alert(excep_mensaje);
                //alert("Lo sentimos, ha ocurrido un error inesperado al verificar existencia de archivos ATR");

            }
        },
        error: function () {
            mostrarError();
        }
    });
}

function llenarCeldaArchivoATR(fechaConsulta, values, nuevo) {


    $.ajax({
        type: 'POST',
        url: controlador + 'CompletarCeldaATR',
        datatype: 'json',
        async: false,
        data: {
            fechaConsulta: fechaConsulta,
            agrupados: values
        },
        success: function (evt) {
            var excep_mensaje = evt.StrMensaje;
            var opcion = evt.Indicador;

            if (opcion != -1) {
                if (opcion == 1) {

                    grabarHojaCambio(evt.DataEnvio, nuevo);
                } else {
                    if (opcion == 0) {

                        grabarHojaCambio(evt.DataEnvio, nuevo);

                        cerrarPopUpSave();

                    }
                }
            }

        },
        error: function () {
            mostrarError();
        }
    });
}

function grabarHojaCambio(dataEnvio, esNuevo) {

    var fecha = $('#txtFecha').val();
    var turno = $('#cbTurno').val();
    var responsable = $('#cbCoordinador').val();

    var mensaje = validacion();

    if (mensaje == "") {

        $.ajax({
            type: "POST",
            url: controlador + 'grabar',
            dataType: "json",
            async: false,
            contentType: 'application/json',
            traditional: true,
            data: JSON.stringify({
                data: hot.getData(),
                dataAdicional: dataEnvio,
                subtitulos: subTitulos,
                agrupaciones: agrupaciones,
                comentarios: comentarios,
                adicionales: adicionales,
                fecha: fecha,
                turno: turno,
                responsable: responsable
            }),

            success: function (data) {
                if (data == 1) {
                    mostrarExito();
                    $('#btnExcel').css('display', 'block');
                    $('#btnPDF').css('display', 'block');
                    $('#btnAuditoria').css('display', 'block');
                    if (!esNuevo) {
                        if (hot != null) {
                            hot.destroy();
                        }
                    }

                    cargarGrilla();
                }
                else {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        mostrarAlerta(mensaje);
    }

}

function esNuevoCambioTurno() {

    var esNuevo = false;
    var datos = hot.getData();
    var tipoReprograma = ""; // A,B,C,D,E,F,G,...
    var subSeccs = ""; //subseccionCodi


    for (var j = subTitulos[2] + 2; j <= subTitulos[3] - 3; j++) {
        tipoReprograma = datos[j - 1][9];
        subSeccs = datos[j - 1][8];
        if (subSeccs == 0)
            esNuevo = true;

    }

    return esNuevo;
}

function obtenerAgrupaciones() {
    var datos = hot.getData();
    var tipoReprograma = ""; // A,B,C,D,E,F,G,...
    var subSeccs = ""; //subseccionCodi
    var reg = ""; //'tipo/subseccioncodi'
    var lstGrupos = new Array();
    var num = 0;

    for (var j = subTitulos[2] + 2; j <= subTitulos[3] - 3; j++) {
        tipoReprograma = datos[j - 1][9];
        subSeccs = datos[j - 1][8];

        reg = tipoReprograma + "/" + subSeccs;
        //Si se guardo el tipo del reprograma
        if (tipoReprograma != null) {
            lstGrupos[num] = reg;
            num++;
        }
        else {
            //falta verificar que pasa si no tienen guardado el tipo de reprograma
        }
    }
    var lstAgrupados = lstGrupos.join(',');

    return lstAgrupados;
}


function cerrarPopUpSave() {
    $('#popupSaveReprog').bPopup().close();
}



exportar = function (formato) {
    var fecha = $('#txtFecha').val();
    var turno = $('#cbTurno').val();

    if (fecha != "" && turno != "") {

        $.ajax({
            type: 'POST',
            url: controlador + 'exportar',
            data: {
                fecha: fecha,
                turno: turno,
                formato: formato
            },
            dataType: 'json',
            success: function (result) {
                if (result.Indicador == 1) {
                    document.location.href = controlador + 'descargar?formato=' + formato;
                }
                else {

                    //mostrarError();
                    mostrarMsgError(result.StrMensaje);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        alert("Seleccione fecha, turno.");
    }
}

auditoria = function () {

    $.ajax({
        type: 'POST',
        url: controlador + 'auditoria',
        data: {
            id: $('#hfIdCambioTurno').val()
        },
        success: function (evt) {
            $('#contenidoAuditoria').html(evt);
            $('#tablaAuditoria').dataTable({
            });
            setTimeout(function () {
                $('#popupAuditoria').bPopup({
                });
            }, 50);


        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarExito = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-exito');
    $('#mensaje').text('La operación se ejecutó exitosamente, puede exportar a Excel o PDF');
}

mostrarError = function () {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').text('Ha ocurrido un error por favor intentar nuevamente');
}

mostrarMsgError = function (msgError) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-error');
    $('#mensaje').text(msgError);
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-alert');
    $('#mensaje').html(mensaje);
}

mostrarMensaje = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').addClass('action-message');
    $('#mensaje').html(mensaje);
}

validacion = function () {
    var datos = hot.getData();
    var mensaje = "";

    var flagNumero = true;
    for (var j = subTitulos[1] + 1; j < subTitulos[2]; j++) {

        if (hot.getDataAtCell(j, 3) != null) {
            if (hot.getDataAtCell(j, 3) != "") {
                if (isDecimal(hot.getDataAtCell(j, 3)) == false) {
                    flagNumero = false;
                }
            }
        }
        if (hot.getDataAtCell(j, 6) != null) {
            if (hot.getDataAtCell(j, 6) != "") {
                if (isDecimal(hot.getDataAtCell(j, 6)) == false) {
                    flagNumero = false;
                }
            }
        }
        if (hot.getDataAtCell(j, 9) != null) {
            if (hot.getDataAtCell(j, 9) != "") {
                if (isDecimal(hot.getDataAtCell(j, 9)) == false) {
                    flagNumero = false;
                }
            }
        }
    }

    if (!flagNumero) {
        mensaje = mensaje + '<li>Las magnitudes deben ser cantidades numéricas.</li>';
    }

    var indice = hot.countRows() - 2

    if (hot.getDataAtCell(indice, 0) == null) {
        mensaje = mensaje + '<li>Seleccione coordinador que recibe el turno.</li>';
    }
    else {
        if (hot.getDataAtCell(indice, 0) == "") {
            mensaje = mensaje + '<li>Seleccione coordinador que recibe el turno.</li>';
        }
    }
    if (hot.getDataAtCell(indice, 2) == null) {
        mensaje = mensaje + '<li>Seleccione especialista que recibe el turno.</li>';
    }
    else {
        if (hot.getDataAtCell(indice, 2) == "") {
            mensaje = mensaje + '<li>Seleccione especialista que recibe el turno.</li>';
        }
    }
    if (hot.getDataAtCell(indice, 6) == null) {
        mensaje = mensaje + '<li>Seleccione analista que recibe el turno.</li>';
    }
    else {
        if (hot.getDataAtCell(indice, 6) == "") {
            mensaje = mensaje + '<li>Seleccione analista que recibe el turno.</li>';
        }
    }

    if (hot.getDataAtCell(indice - 2, 8) == null) {
        mensaje = mensaje + '<li>Ingrese la hora de cambio del turno</li>';
    }
    else if (hot.getDataAtCell(indice - 2, 8) == "") {
        mensaje = mensaje + '<li>Ingrese la hora de cambio del turno</li>';
    }
    else {
        if (validateTime(hot.getDataAtCell(indice - 2, 8)) == false) {
            mensaje = mensaje + '<li>El formato de la hora de cambio de turno no es correcto.</li>';
        }
    }

    if (hot.getDataAtCell(indice - 2, 0) != "SI") {
        mensaje = mensaje + '<li>Por favor, verificar que se entregan los CASOS SIN RESERVA de cada reprograma</li>';
    }

    if (errors.length > 0) {
        mensaje = mensaje + '<li>Por favor verifique las celdas de color rojo</li>';
    }


    return mensaje;
}

isDecimal = function (n) {
    if (n == "")
        return false;

    var count = 0;
    var strCheck = "0123456789.";
    var i;

    for (i in n) {
        if (strCheck.indexOf(n[i]) == -1)
            return false;
        else {
            if (n[i] == '.') {
                count = count + 1;
            }
        }
    }
    if (count > 1) return false;
    return true;
}

validateTime = function (inputStr) {
    if (!inputStr || inputStr.length < 1) { return false; }
    var time = inputStr.split(':');
    return (time.length === 2
        && parseInt(time[0], 10) >= 0
        && parseInt(time[0], 10) <= 23
        && parseInt(time[1], 10) >= 0
        && parseInt(time[1], 10) <= 59) ||
        (time.length === 3
            && parseInt(time[0], 10) >= 0
            && parseInt(time[0], 10) <= 23
            && parseInt(time[1], 10) >= 0
            && parseInt(time[1], 10) <= 59
            && parseInt(time[2], 10) >= 0
            && parseInt(time[2], 10) <= 59)
}