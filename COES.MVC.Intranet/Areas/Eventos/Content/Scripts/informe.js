var controlador = siteRoot + 'eventos/informe/';
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
var tipos = null;
var fechahora = null;
var validaciones = null;
var longitudes = null;
var centros = null;
var derechos = null;

$(function () {
    $('#btnGrabarFile').click(function () {
        grabarFile();
    });

    $('#btnCancelarFile').click(function () {
        $('#popupFile').bPopup().close();
    });

    $('#btnExportarWord').click(function () {
        exportar('WORD');
    });

    $('#btnExportarPDF').click(function () {
        exportar('PDF');
    });

    $('#btnAceptarCopia').click(function () {
        copiar();
    });

    $('#btnCancelarCopia').click(function () {
        $('#popupCopia').bPopup().close();
    });

    $('#btnAceptarFinalizar').click(function () {
        confirmarInforme();
    });

    $('#btnCancelarFinalizar').click(function () {
        $('#popupFinalizar').bPopup().close();
    });

    $('#btnConsultaInforme').click(function () {
        cargarEstadoInforme();
    });

    $('#btnInformeSEV').click(function () {

    });

    $('#btnListado').click(function () {
        document.location.href = siteRoot + 'eventos/evento/index';
    });

    $('#btnAceptarCopiaEmpresa').click(function () {
        copiarEmpresa();
    });

    $('#btnCancelarCopiaEmpresa').click(function () {
        $('#popupCopiaEmpresa').bPopup().close();
    });

    $(document).ready(function () {
        $('#formato-tb-anexo-general').hide();
        $('#btnGrabar').hide();
        $('#btnExportar').hide();
        $('#mensajeInforme').hide();

    });

});


openEvento = function () {
    $('#contenidoEvento').css('display', 'block');
    $('#contenidoInforme').css('display', 'none');

    $('#formato-tb-anexo-general').hide();

    $('#btnGrabar').hide();
    $('#btnExportar').hide();
    $('#mensajeInforme').hide();
    $('#auditoriaInforme').hide();


    setearEstilo("E");

}

openInforme = function (idInforme, indicador) {
    $('#contenidoEvento').css('display', 'none');
    $('#contenidoInforme').css('display', 'block');
    $('#btnExportar').show();
    $('#btnRevisar').show();
    $('#Importar').show();
    $('#mensajeInforme').show();
    $('#auditoriaInforme').show();

    mostrarExito("Datos del informe");

    if (hot != null) {
        hot.destroy();
    }

    setearEstilo(indicador);

    var idEvento = $('#hfCodigoEvento').val();
    cargarGrilla(idEvento, idInforme, indicador);

    //anexo
    cargarDocumentos(idInforme, $('#hfIndicadorEdicion').val());

    //auditoria (plazos)
    cargarAuditoria(idInforme);

    $('#formato-tb-anexo-general').show();

}


//para htable
cargarGrilla = function (idEvento, idInforme, indicador) {

    $('#btnGrabar').css('display', 'block');

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerdatagrilla',
        datatype: 'json',
        data: {
            idEvento: idEvento,
            idInforme: idInforme,
            indicador: indicador
        },
        success: function (result) {
            errors = [];

            var container = document.getElementById('contenidoInforme');

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

            var fechahoraRenderer = function (ht, row, col) {

                var r = /^(((0[1-9]|[12]\d|3[01])[\/\.-](0[13578]|1[02])[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((0[1-9]|[12]\d|30)[\/\.-](0[13456789]|1[012])[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((0[1-9]|1\d|2[0-8])[\/\.-](02)[\/\.-]((19|[2-9]\d)\d{2})\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9]))|((29)[\/\.-](02)[\/\.-]((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))\s(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])))$/g;

                var value = ht.instance.getDataAtCell(row, col);
                var td = ht.instance.getCell(row, col);

                var esfechahora = r.test(value);

                if (!esfechahora) {
                    td.style.backgroundColor = 'red';
                }

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
            tipos = result.Tipos;
            fechahora = result.FechaHora;

            var hotOptions = {
                data: data,
                colHeaders: false,
                rowHeaders: true,
                comments: true,
                width: 1300,
                height: 770,
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

                    for (var i in tipos) {
                        if (row == tipos[i]['Row'] && col == tipos[i]['Column']) {
                            this.type = tipos[i]['Tipo'];
                        }
                    }

                    for (var i in fechahora) {

                        if (row == fechahora[i]['Row'] && col == fechahora[i]['Column']) {
                            fechahoraRenderer(this, row, col);
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

                    var newTipos = [];
                    for (var i in tipos) {
                        if (tipos[i]['Row'] == index - 1) {
                            newTipos.push({ Row: index, Column: tipos[i]['Column'], Tipo: tipos[i]['Tipo'] });
                        }
                        if (tipos[i]['Row'] > index - 1) {
                            tipos[i]['Row'] = tipos[i]['Row'] + 1;
                        }
                    }

                    for (var j in newTipos) {
                        tipos.push(newTipos[j]);
                    }

                    var newFechahora = [];
                    for (var i in fechahora) {
                        if (fechahora[i]['Row'] == index - 1) {
                            newFechahora.push({ Row: index, Column: fechahora[i]['Column'] });
                        }
                        if (fechahora[i]['Row'] > index - 1) {
                            fechahora[i]['Row'] = fechahora[i]['Row'] + 1;
                        }
                    }

                    for (var j in newFechahora) {
                        fechahora.push(newFechahora[j]);
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


                    for (var i in tipos) {
                        if (tipos[i]['Row'] == index - 1) {
                            tipos.splice(i, 1);
                        }
                        else if (tipos[i]['Row'] > index - 1) {
                            tipos[i]['Row'] = tipos[i]['Row'] - 1;
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
                                    || adicionales.indexOf(hot.getSelected()[0]) > -1
                                    );

                            }
                        },
                        "remove_row": {
                            name: 'Eliminar fila',
                            disabled: function () {
                                return (subTitulos.indexOf(hot.getSelected()[0]) > -1 || titulos.indexOf(hot.getSelected()[0]) > -1
                                    || agrupaciones.indexOf(hot.getSelected()[0]) > -1 || comentarios.indexOf(hot.getSelected()[0]) > -1
                                    || adicionales.indexOf(hot.getSelected()[0]) > -1
                                    || hot.getSelected().length == 1
                                    || subTitulos.indexOf(hot.getSelected()[0] + 1) > -1
                                    || adicionales.indexOf(hot.getSelected()[0] + 1) > -1
                                    || agrupaciones.indexOf(hot.getSelected()[0] + 1) > -1
                                    || titulos.indexOf(hot.getSelected()[0] + 1) > -1);
                            }
                        }
                    }
                }
            });




            $("#btnGrabar").off("click");

            $('#btnGrabar').click(function () {
                grabar();
            });


            $("#btnExportar").off("click");
            $('#btnExportar').click(function () {
                openExportar();
            });

        },
        error: function () {
            mostrarError();
        }
    });
}
























grabarItemReporte = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'grabarelemento',
        dataType: 'json',
        global: false,
        data: $('#formElemento').serialize(),
        success: function (result) {
            if (result.Indicador > 0) {
                pintarElemento(result.Indicador, result.Entidad);
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

grabarTexto = function (id, idItemInforme) {
    var comentario = $('#' + id).val();

    $.ajax({
        type: 'POST',
        url: controlador + 'actualizartexto',
        dataType: 'json',
        data: {
            idItemInforme: idItemInforme,
            comentario: comentario
        },
        success: function (result) {
            if (result == -1) {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

obtenerListaEquipo = function (familia) {
    $('#cbEquipo').get(0).options.length = 0;
    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerequipos',
        dataType: 'json',
        data: { indicador: familia },
        cache: false,
        global: false,
        success: function (aData) {
            $('#cbEquipo').get(0).options.length = 0;
            $('#cbEquipo').get(0).options[0] = new Option("-SELECCIONE-", "");
            $.each(aData, function (i, item) {
                $('#cbEquipo').get(0).options[$('#cbEquipo').get(0).options.length] = new Option(item.TAREAABREV + ' ' + item.AREANOMB + ' ' + item.Equiabrev, item.Equicodi);
            });
        },
        error: function () {
            mostrarError();
        }
    });
}


openElemento = function (idInforme, nroItem, nroSubItem, idItemInforme) {
    $.ajax({
        type: "POST",
        url: controlador + "elemento",
        data: {
            idInforme: idInforme,
            itemNumber: nroItem,
            subItemNumber: nroSubItem,
            idItemInforme: idItemInforme
        },
        success: function (evt) {
            $('#contenidoElemento').html(evt);

            setTimeout(function () {
                $('#popupElemento').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}


deleteElemento = function (elemento, idItemInforme) {
    if (confirm('¿Está seguro de quitar este elemento?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarelemento',
            dataType: 'json',
            data: { idItemInforme: idItemInforme },
            success: function (result) {
                if (result == 1) {
                    $('#' + elemento).remove();
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
}


cargarPrevio = function () {
    limpiarFormulario();
    var itemInforme = "";
    var subItemInforme = "";
    var valor = $('#cbItemInforme').val();
    if (valor == "5-1") {
        itemInforme = "5";
        subItemInforme = "1";
    }
    else if (valor == "5-2") {
        itemInforme = "5";
        subItemInforme = "2";
    }
    else if (valor == "5-3") {
        itemInforme = "5";
        subItemInforme = "3";
    }
    else {
        itemInforme = valor;
        subItemInforme = "0";
    }

    $('#hfItemInforme').val(itemInforme);
    $('#hfSubItemInforme').val(subItemInforme);

    cargarFormulario(1);
}

cargarFormulario = function (indicador) {
    $('.tr-elemento').css('display', 'none');
    var item = $('#hfItemInforme').val();

    if (item == "5") {
        var subItem = $('#hfSubItemInforme').val();
        $('#trUnidad').css('display', 'block');
        $('#trPotenciaActiva').css('display', 'block');
        $('#trPotenciaReactiva').css('display', 'block');
        $('#trObservacion').css('display', 'block');
        $('#spanObservacion').text('Observación');

        if (subItem == "1") {
            $('#spanUnidad').text('Unidad');
            if (indicador == 1) {
                cargarEquipamiento('U');
            }
        }
        if (subItem == "2") {
            $('#trSubestacionDe').css('display', 'block');
            $('#trSubestacionhasta').css('display', 'block');
            $('#spanUnidad').text('Línea de Transmisión');
            if (indicador == 1) {
                cargarEquipamiento('L');
            }
        }
        if (subItem == "3") {
            $('#trNivelTension').css('display', 'block');
            $('#spanUnidad').text('Transformador');
            if (indicador == 1) {
                cargarEquipamiento('T');
            }
        }
    }
    if (item == "6") {
        $('#trHora').css('display', 'block');
        $('#trObservacion').css('display', 'block');
        $('#spanObservacion').text('Descripción del evento');
    }
    if (item == "7") {
        $('#trUnidad').css('display', 'block');
        $('#trSenalizacion').css('display', 'block');
        $('#trInterruptor').css('display', 'block');
        $('#trAC').css('display', 'block');
        $('#spanUnidad').text('Equipo');
        if (indicador == 1) {
            cargarEquipamiento('U');
        }
    }
    if (item == "8") {
        $('#trUnidad').css('display', 'block');
        $('#trInterruptor').css('display', 'block');
        $('#trContadorAntes').css('display', 'block');
        $('#trContadorDespues').css('display', 'block');
        $('#spanUnidad').text('Celda');
        if (indicador == 1) {
            cargarEquipamiento('C');
        }
    }

    if (item == "10") {
        $('#trSuministro').css('display', 'block');
        $('#trPotenciaMW').css('display', 'block');
        $('#trHoraInicial').css('display', 'block');
        $('#trHoraFinal').css('display', 'block');
        $('#trProteccion').css('display', 'block');
    }
    if (item == "11") {
        $('#trObservacion').css('display', 'block');
        $('#spanObservacion').text('Conclusión');
    }
    if (item == "12") {
        $('#trObservacion').css('display', 'block');
        $('#spanObservacion').text('Acciones ejecutadas');
    }
    if (item == "13") {
        $('#trObservacion').css('display', 'block');
        $('#spanObservacion').text('Observación / Recomendación');
    }
}

grabarElemento = function () {
    var mensaje = validarElemento();
    if (mensaje == "") {
        grabarItemReporte();
        $('#mensajeElemento').removeClass();
        $('#mensajeElemento').addClass("action-exito");
        $('#mensajeElemento').html("Se agregó el elemento. Puede agregar otro.");

        limpiarFormulario();
        cargarFormulario(0);
    }
    else {
        $('#mensajeElemento').removeClass();
        $('#mensajeElemento').addClass("action-alert");
        $('#mensajeElemento').html(mensaje);
    }
}

cancelarElemento = function () {
    $('#popupElemento').bPopup().close();
}

cargarEquipamiento = function (item) {
    obtenerListaEquipo(item);
}

validarElemento = function () {
    var mensaje = "";

    var item = $('#hfItemInforme').val();
    var subItem = $('#hfSubItemInforme').val();

    if (item == "5") {
        if ($('#cbEquipo').val() == "") {
            mensaje = mensaje + "<li>Selecione una unidad.</li>";
        }

        if (subItem == "2") {
            if ($('#txtSubestacionDe').val() == "") {
                mensaje = mensaje + "<li>Ingrese subestación desde.</li>";
            }
            if ($('#txtSubestacionHasta').val() == "") {
                mensaje = mensaje + "<li>Ingrese subestación hasta.</li>"
            }
        }
        if (subItem == "3") {
            if ($('#txtNivelTension').val() == "") {
                mensaje = mensaje + "<li>Ingrese nivel de tensión.</li>";
            }
        }
    }
    if (item == "6") {
        if ($('#txtHora').val() == "") {
            mensaje = mensaje + "<li>Ingrese la hora.</li>";
        }
        else {
            if (!$('#txtHora').inputmask("isComplete")) {
                mensaje = mensaje + "<li>Ingrese una hora correcta.</li>";
            }
        }
        if ($('#txtObservacion').val() == "") {
            mensaje = mensaje + "<li>Ingrese la descripción del evento.</li>";
        }
    }
    if (item == "7") {
        if ($('#cbEquipo').val() == "") {
            mensaje = mensaje + "<li>Selecione una unidad.</li>";
        }
        if ($('#txtSenializacion').val() == "") {
            mensaje = mensaje + "<li>Ingrese señalización.</li>";
        }
        if ($('#cbInterruptor').val() == "") {
            mensaje = mensaje + "<li>Seleccione un interruptor.</li>";
        }
        if ($('#cbAC').val() == "") {
            mensaje = mensaje + "<li>Seleccione A/C.</li>";
        }
    }
    if (item == "8") {
        if ($('#cbEquipo').val() == "") {
            mensaje = mensaje + "<li>Seleccione celda.</li>";
        }
        if ($('#cbInterruptor').val() == "") {
            mensaje = mensaje + "<li>Seleccione interruptor</li>";
        }
        if ($('#txtRA').val() == "" && $('#txtSA').val() == "" && $('#txtTA')) {
            mensaje = mensaje + "<li>Ingrese contadores inicial.</li>";
        }
        if ($('#txtRD').val() == "" && $('#txtSD').val() == "" && $('#txtTD')) {
            mensaje = mensaje + "<li>Ingrese contadores después.</li>";
        }
    }
    if (item == "10") {

        if ($('#txtSuministro').val() == "") {
            mensaje = mensaje + "<li>Ingrese el suministro.</li>";
        }
        if ($('#txtPotenciaMW').val() == "") {
            mensaje = mensaje + "<li>Ingrese la potencia (MW).</li>";
        }
        if ($('#txtHoraInicial').val() == "") {
            mensaje = mensaje + "<li>Ingrese la hora inicial de la interrupción.</li>";
        }
        else if (!$('#txtHoraInicial').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese una hora correcta.</li>";
        }

        if ($('#txtHoraFinal').val() == "") {
            mensaje = mensaje + "<li>Ingrese la hora final de la interrupción.</li>";
        }
        else if (!$('#txtHoraFinal').inputmask("isComplete")) {
            mensaje = mensaje + "<li>Ingrese una hora correcta.</li>";
        }
        if ($('#txtProteccion').val() == "") {
            mensaje = mensaje + "<li>Ingrese la hora final de la interrupción.</li>";
        }
    }
    if (item == "11") {
        if ($('#txtObservacion').val() == "") {
            mensaje = mensaje + "<li>Ingrese la conclusión.</li>";
        }
    }
    if (item == "12") {
        if ($('#txtObservacion').val() == "") {
            mensaje = mensaje + "<li>Ingrese la acción ejecutada.</li>";
        }
    }
    if (item == "13") {
        if ($('#txtObservacion').val() == "") {
            mensaje = mensaje + "<li>Ingrese la observación / recomendación.</li>";
        }
    }

    var resultado = "";
    if (mensaje != "") {
        resultado = resultado + "<ul>";
        resultado = resultado + mensaje;
        resultado = resultado + "</ul>";
    }

    return resultado;
}

limpiarFormulario = function () {
    $('#cbEquipo').val("");
    $('#txtSubestacionDe').val("");
    $('#txtSubestacionHasta').val("");
    $('#txtPotActiva').val("");
    $('#txtPotReactiva').val("");
    $('#txtNivelTension').val("");
    $('#txtHora').val("");
    $('#txtObservacion').val("");
    $('#txtSenializacion').val("");
    $('#cbInterruptor').val("");
    $('#cbAC').val("");
    $('#txtRA').val("");
    $('#txtSA').val("");
    $('#txtTA').val("");
    $('#txtRD').val("");
    $('#txtSD').val("");
    $('#txtTD').val("");
    $('#txtSuministro').val("");
    $('#txtPotenciaMW').val("");
    $('#txtHoraInicial').val("");
    $('#txtHoraFinal').val("");
    $('#txtProteccion').val("");
}

pintarElemento = function (indicador, model) {
    var id = '#' + model.Eveninfcodi + '_' + model.Itemnumber + '_' + model.Subitemnumber;
    var indice = $(id + ' >tbody >tr').length + 1;
    var html = '';
    var itemInforme = model.Itemnumber;
    var subItemInforme = model.Subitemnumber;
    var idtr = model.Eveninfcodi + '_' + model.Itemnumber + '_' + model.Subitemnumber + '_' + model.Infitemcodi;

    if (model.Desobservacion == null) model.Desobservacion = " ";
    if (model.Potactiva == null) model.Potactiva = " ";
    if (model.Potreactiva == null) model.Potreactiva = " ";

    if (itemInforme == 5) {
        if (subItemInforme == 1) {
            if (indicador == 1) {
                html =
                '<tr id = "' + idtr + '">' +
                '    <td>' + indice + '</td>' +
                '    <td>' + model.Areanomb + '</td>' +
                '    <td>' + model.Equinomb + '</td>' +
                '    <td>' + model.Potactiva + '</td>' +
                '    <td>' + model.Potreactiva + '</td>' +
                '    <td>' + model.Desobservacion + '</td>' +
                '    <td>' +
                '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 5, 1, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
                '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
                '    </td>' +
                '</tr>';
            }
            else {
                $('#' + idtr).find('td').eq(1).text(model.Areanomb);
                $('#' + idtr).find('td').eq(2).text(model.Equinomb);
                $('#' + idtr).find('td').eq(3).text(model.Potactiva);
                $('#' + idtr).find('td').eq(4).text(model.Potreactiva);
                $('#' + idtr).find('td').eq(5).text(model.Desobservacion);
            }
        }
        if (subItemInforme == 2) {
            if (indicador == 1) {
                html =
                '<tr id = "' + idtr + '">' +
                '    <td>' + indice + '</td>' +
                '    <td>' + model.Equinomb + ' - ' + model.Equicodi + '</td>' +
                '    <td>' + model.Subestacionde + '</td>' +
                '    <td>' + model.Subestacionhasta + '</td>' +
                '    <td>' + model.Potactiva + '</td>' +
                '    <td>' + model.Potreactiva + '</td>' +
                '    <td>' + model.Desobservacion + '</td>' +
                '    <td>' +
                '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 5, 2, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
                '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
                '    </td>' +
                '</tr>';
            }
            else {
                $('#' + idtr).find('td').eq(1).text(model.Equinomb + ' - ' + model.Equicodi);
                $('#' + idtr).find('td').eq(2).text(model.Subestacionde);
                $('#' + idtr).find('td').eq(3).text(model.Subestacionhasta);
                $('#' + idtr).find('td').eq(4).text(model.Potactiva);
                $('#' + idtr).find('td').eq(5).text(model.Potreactiva);
                $('#' + idtr).find('td').eq(6).text(model.Desobservacion);
            }
        }
        if (subItemInforme == 3) {
            if (indicador == 1) {
                html =
                '<tr id = "' + idtr + '">' +
                '    <td>' + indice + '</td>' +
                '    <td>' + model.Areanomb + '</td>' +
                '    <td>' + model.Equinomb + ' ' + model.Equicodi + '</td>' +
                '    <td>' + model.Potactiva + '</td>' +
                '    <td>' + model.Potreactiva + '</td>' +
                '    <td>' + model.Niveltension + '</td>' +
                '    <td>' + model.Desobservacion + '</td>' +
                '    <td>' +
                '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 5, 3, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
                '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
                '    </td>' +
                '</tr>';
            }
            else {
                $('#' + idtr).find('td').eq(1).text(model.Areanomb);
                $('#' + idtr).find('td').eq(2).text(model.Equinomb + ' - ' + model.Equicodi);
                $('#' + idtr).find('td').eq(3).text(model.Potactiva);
                $('#' + idtr).find('td').eq(4).text(model.Potreactiva);
                $('#' + idtr).find('td').eq(5).text(model.Niveltension);
                $('#' + idtr).find('td').eq(6).text(model.Desobservacion);
            }
        }
    }
    if (itemInforme == 6) {
        if (indicador == 1) {
            html =
            '<tr id = "' + idtr + '">' +
            '    <td>' + model.Itemhora + '</td>' +
            '    <td>' + model.Desobservacion + '</td>' +
            '    <td>' +
            '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 6, 0, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
            '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
            '    </td>' +
            '</tr>';
        }
        else {
            $('#' + idtr).find('td').eq(0).text(model.Itemhora);
            $('#' + idtr).find('td').eq(1).text(model.Desobservacion);
        }
    }
    if (itemInforme == 7) {
        if (indicador == 1) {
            html =
            '<tr id = "' + idtr + '">' +
            '     <td>' + model.Areanomb + '</td>' +
            '     <td>' + model.Equinomb + '</td>' +
            '     <td>' + model.Senializacion + '</td>' +
            '     <td>' + model.Internomb + '</td>' +
            '     <td>' + model.Ac + '</td>' +
            '    <td>' +
            '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 7, 0, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
            '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
            '    </td>' +
            '</tr>';
        }
        else {
            $('#' + idtr).find('td').eq(0).text(model.Areanomb);
            $('#' + idtr).find('td').eq(1).text(model.Equinomb);
            $('#' + idtr).find('td').eq(2).text(model.Senializacion);
            $('#' + idtr).find('td').eq(3).text(model.Internomb);
            $('#' + idtr).find('td').eq(4).text(model.Ac);
        }
    }
    if (itemInforme == 8) {
        if (indicador == 1) {
            html =
            '<tr id = "' + idtr + '">' +
            '    <td>' + model.Areanomb + '</td>' +
            '    <td>' + model.Equinomb + '</td>' +
            '    <td>' + model.Internomb + '</td>' +
            '    <td>' + model.Ra + '</td>' +
            '    <td>' + model.Sa + '</td>' +
            '    <td>' + model.Ta + '</td>' +
            '    <td>' + model.Rd + '</td>' +
            '    <td>' + model.Sd + '</td>' +
            '    <td>' + model.Td + '</td>' +
            '    <td>' +
            '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 8, 0, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
            '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
            '    </td>' +
            '</tr>';
        }
        else {
            $('#' + idtr).find('td').eq(0).text(model.Areanomb);
            $('#' + idtr).find('td').eq(1).text(model.Equinomb);
            $('#' + idtr).find('td').eq(2).text(model.Internomb);
            $('#' + idtr).find('td').eq(3).text(model.Ra);
            $('#' + idtr).find('td').eq(4).text(model.Sa);
            $('#' + idtr).find('td').eq(5).text(model.Ta);
            $('#' + idtr).find('td').eq(6).text(model.Rd);
            $('#' + idtr).find('td').eq(7).text(model.Sd);
            $('#' + idtr).find('td').eq(8).text(model.Td);
        }
    }
    if (itemInforme == 10) {
        if (indicador == 1) {
            html =
            '<tr id = "' + idtr + '">' +
            '    <td>' + indice + '</td>' +
            '    <td>' + model.Sumininistro + '</td>' +
            '    <td>' + model.Potenciamw + '</td>' +
            '    <td>' + model.DesIntInicio + '</td>' +
            '    <td>' + model.DesIntFin + '</td>' +
            '    <td>' + model.Duracion + '</td>' +
            '    <td>' + model.Proteccion + '</td>' +
            '    <td>' +
            '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , 10, 0, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
            '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
            '    </td>' +
            '</tr>';
        }
        else {
            $('#' + idtr).find('td').eq(1).text(model.Sumininistro);
            $('#' + idtr).find('td').eq(2).text(model.Potenciamw);
            $('#' + idtr).find('td').eq(3).text(model.DesIntInicio);
            $('#' + idtr).find('td').eq(4).text(model.DesIntFin);
            $('#' + idtr).find('td').eq(5).text(model.Duracion);
            $('#' + idtr).find('td').eq(6).text(model.Proteccion);

        }
    }
    if (itemInforme == 11 || itemInforme == 12 || itemInforme == 13) {
        if (indicador == 1) {
            html =
            '<tr id = "' + idtr + '">' +
            '    <td>' + indice + '</td>' +
            '    <td>' + model.Desobservacion + '</td>' +
            '    <td>' +
            '        <a href="JavaScript:openElemento(' + model.Eveninfcodi + ' , ' + itemInforme + ' , 0, ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Pen.png" title="Editar" /></a>' +
            '        <a href="JavaScript:deleteElemento(\'' + idtr + '\', ' + model.Infitemcodi + ');" class="action-informe"><img src="' + siteRoot + 'Content/Images/Trash.png" title="Eliminar" /></a>' +
            '    </td>' +
            '</tr>';
        }
        else {
            $('#' + idtr).find('td').eq(1).text(model.Desobservacion);
        }
    }
    if (indicador == 1) {
        $(id + '> tbody').append(html);
    }
}

cargarDocumentos = function (idInforme, indicador) {

    $.ajax({
        type: "POST",
        url: controlador + "anexo",
        data: {
            idInforme: idInforme,
            indicador: indicador
        },
        success: function (evt) {
            $('#contenedorFile').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}


$('#btnGrabar').click(function () {
    grabar();
});

grabar = function () {

    var idEvento = $('#hfCodigoEvento').val();
    var idInforme = $('#hfCodigoInforme').val();
    var tipo = $('#hfTipoInforme').val();


    $.ajax({
        type: "POST",
        url: controlador + 'grabar',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            data: hot.getData(),
            titulos: titulos,
            subtitulos: subTitulos,
            agrupaciones: agrupaciones,
            comentarios: comentarios,
            adicionales: adicionales,
            idEvento: idEvento,
            idInforme: idInforme,
            tipo: tipo
        }),

        success: function (data) {
            if (data == 1) {
                cargarGrilla(idEvento, idInforme, tipo);

                if (hot != null) {
                    hot.destroy();
                }
                mostrarExito("La operación se realizó con éxito...");

            } else {

                mostrarAlerta("Se presentan errores en los datos ingresados. Revise los campos en rojo y/o celdas vacias");
            }

        },
        error: function () {
            mostrarError();
        }
    });

}


cargarAuditoria = function (idInforme) {
    $.ajax({
        type: "POST",
        url: controlador + "auditoria",
        data: {
            idInforme: idInforme
        },
        success: function (evt) {
            $('#auditoriaInforme').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}

deleteFile = function (idFile) {
    if (confirm('¿Está seguro de quitar este elemento?')) {
        $.ajax({
            type: 'POST',
            url: controlador + 'eliminarfile',
            dataType: 'json',
            data: { idFile: idFile },
            success: function (result) {
                if (result == 1) {
                    cargarDocumentos($('#hfCodigoInforme').val(), $('#hfIndicadorEdicion').val());
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
}

editarFile = function (idFile, descripcion) {
    $('#txtDescripcionFile').val(descripcion);
    $('#hfIdFileReporte').val(idFile);

    $('#popupFile').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

grabarFile = function () {
    $.ajax({
        type: 'POST',
        global: false,
        url: controlador + 'grabarfile',
        dataType: 'json',
        data: {
            idFile: $('#hfIdFileReporte').val(),
            descripcion: $('#txtDescripcionFile').val()
        },
        success: function (result) {
            if (result == 1) {
                cargarDocumentos($('#hfCodigoInforme').val(), $('#hfIndicadorEdicion').val());
                $('#popupFile').bPopup().close();
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

openExportar = function () {
    $('#popupExportar').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}

exportar = function (tipo) {

    var idEvento = $('#hfCodigoEvento').val();
    var idInforme = $('#hfCodigoInforme').val();
    var idEmpresa = $('#hfCodigoEmpresa').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'exportar',
        global: false,
        dataType: 'json',
        data: {
            idEvento: idEvento,
            idInforme: idInforme,
            idEmpresa: idEmpresa,
            tipo: tipo
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                window.location = controlador + "descargar?tipo=" + tipo;
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

openCopia = function () {
    var tipo = $('#hfTipoInforme').val();
    $('#cbOrigenPreliminar').css('display', 'none');
    $('#cbOrigenFinal').css('display', 'none');
    $('#cbOrigenComplementario').css('display', 'none');

    if (tipo == "P") {
        $('#cbOrigenPreliminar').css('display', 'block');
    }
    if (tipo == "F") {
        $('#cbOrigenFinal').css('display', 'block');
    }
    if (tipo == "C") {
        $('#cbOrigenComplementario').css('display', 'block');
    }

    $('#popupCopiar').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
}


openCopiaEmpresa = function () {
    $('#popupCopiaEmpresa').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });

}


copiar = function () {
    var tipo = $('#hfTipoInforme').val();
    var idDestino = $('#hfCodigoInforme').val();
    var idOrigen = "";
    var indicador = $("input:radio[name='OpcionCopia']:checked").val();

    if (tipo == "P") {
        idOrigen = $('#CodigoInformePreliminarInicial').val();
    }

    if (tipo == "F") {

        if ($('#cbOrigenFinal').val() == "I") {
            idOrigen = $('#CodigoInformePreliminarInicial').val();
        }
        if ($('#cbOrigenFinal').val() == "P") {
            idOrigen = $('#CodigoInformePreliminar').val();
        }
    }
    if (tipo == "C") {
        if ($('#cbOrigenComplementario').val() == "I") {
            idOrigen = $('#CodigoInformePreliminarInicial').val();
        }
        if ($('#cbOrigenComplementario').val() == "P") {
            idOrigen = $('#CodigoInformePreliminar').val();
        }
        if ($('#cbOrigenComplementario').val() == "F") {
            idOrigen = $('#CodigoInformeFinal').val();
        }
    }

    if (idOrigen != "" && idDestino != "") {
        if (confirm('¿Está seguro de realizar esta acción?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'copiar',
                global: false,
                dataType: 'json',
                data: {
                    idOrigen: idOrigen,
                    idDestino: idDestino,
                    indicador: indicador
                },
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        openInforme(idDestino, tipo);
                        $('#popupCopia').bPopup().close();
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
    }
}


copiarEmpresa = function () {

    var tipo = $('#hfTipoInforme').val();
    var idEvento = $('#hfCodigoEvento').val();
    var idDestino = $('#hfCodigoInforme').val();
    var idEmpresa = $('#cbEmpresaCopía').val();
    var indicador = $("input:radio[name='OpcionCopiaEmpresa']:checked").val();

    if (idEmpresa != "" && idDestino != "") {

        if (confirm('¿Está seguro de realizar esta acción?')) {
            $.ajax({
                type: 'POST',
                url: controlador + 'copiarempresa',
                global: false,
                dataType: 'json',
                data: {
                    idEvento: idEvento,
                    idEmpresa: idEmpresa,
                    idDestino: idDestino,
                    tipo: tipo,
                    indicador: indicador
                },
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        openInforme(idDestino, tipo);
                        $('#popupCopiaEmpresa').bPopup().close();
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
    }
    else {
        mostrarAlerta('Seleccione empresa');
    }

}


finalizar = function () {
    var tipo = $('#hfTipoInforme').val();
    var idEvento = $('#hfCodigoEvento').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'validarfinalizar',
        global: false,
        dataType: 'json',
        data: {
            idEvento: idEvento,
            indicador: tipo,
            idEmpresa: $('#hfCodigoEmpresa').val()
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                $('#divMensajeFinalizar').css('display', 'none');
            }
            if (resultado == 2) {
                $('#divMensajeFinalizar').css('display', 'block');
            }

            setTimeout(function () {
                $('#popupFinalizar').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}


revisar = function () {
    var tipo = $('#hfTipoInforme').val();
    var idInforme = $('#hfCodigoInforme').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'revisar',
        global: false,
        dataType: 'json',
        data: {
            idEvento: $('#hfCodigoEvento').val(),
            tipo: tipo,
            idInforme: idInforme
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                cargarAuditoria(idInforme);
                $('#mensajeInforme').removeClass();
                $('#mensajeInforme').addClass('action-exito');
                $('#mensajeInforme').html('La operación se realizó correctamente.');
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

confirmarInforme = function () {
    var tipo = $('#hfTipoInforme').val();
    var idInforme = $('#hfCodigoInforme').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'finalizar',
        global: false,
        dataType: 'json',
        data: {
            idEvento: $('#hfCodigoEvento').val(),
            tipo: tipo,
            idInforme: idInforme,
            idEmpresa: $('#hfCodigoEmpresa').val()
        },
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                openInforme(idInforme, tipo);
                $('#popupFinalizar').bPopup().close();
            }
            else if (resultado == 2) {
                mostrarAlerta("Debe agregar datos o adjuntos al informe para poder finalizar");
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

setearEstilo = function (indicador) {
    $('#enlaceInformeP').removeClass();
    $('#enlaceInformeF').removeClass();
    $('#enlaceInformeC').removeClass();
    $('#enlaceInformeA').removeClass();
    $('#enlaceInformeI').removeClass();
    $('#enlaceEvento').removeClass();

    if (indicador == 'P') {
        $('#enlaceEvento').addClass('opcion-informe');
        $('#enlaceInformeF').addClass('opcion-informe');
        $('#enlaceInformeC').addClass('opcion-informe');
        $('#enlaceInformeA').addClass('opcion-informe');
        $('#enlaceInformeI').addClass('opcion-informe');
        $('#enlaceInformeP').addClass('opcion-informe-active');
    }
    if (indicador == 'F') {
        $('#enlaceEvento').addClass('opcion-informe');
        $('#enlaceInformeP').addClass('opcion-informe');
        $('#enlaceInformeC').addClass('opcion-informe');
        $('#enlaceInformeA').addClass('opcion-informe');
        $('#enlaceInformeI').addClass('opcion-informe');
        $('#enlaceInformeF').addClass('opcion-informe-active');
    }
    if (indicador == 'C') {
        $('#enlaceEvento').addClass('opcion-informe');
        $('#enlaceInformeP').addClass('opcion-informe');
        $('#enlaceInformeF').addClass('opcion-informe');
        $('#enlaceInformeA').addClass('opcion-informe');
        $('#enlaceInformeI').addClass('opcion-informe');
        $('#enlaceInformeC').addClass('opcion-informe-active');
    }
    if (indicador == 'A') {
        $('#enlaceEvento').addClass('opcion-informe');
        $('#enlaceInformeP').addClass('opcion-informe');
        $('#enlaceInformeF').addClass('opcion-informe');
        $('#enlaceInformeC').addClass('opcion-informe');
        $('#enlaceInformeI').addClass('opcion-informe');
        $('#enlaceInformeA').addClass('opcion-informe-active');
    }
    if (indicador == 'E') {
        $('#enlaceEvento').addClass('opcion-informe-active');
        $('#enlaceInformeP').addClass('opcion-informe');
        $('#enlaceInformeF').addClass('opcion-informe');
        $('#enlaceInformeC').addClass('opcion-informe');
        $('#enlaceInformeA').addClass('opcion-informe');
        $('#enlaceInformeI').addClass('opcion-informe');
    }
    if (indicador == "I") {
        $('#enlaceEvento').addClass('opcion-informe');
        $('#enlaceInformeP').addClass('opcion-informe');
        $('#enlaceInformeF').addClass('opcion-informe');
        $('#enlaceInformeC').addClass('opcion-informe');
        $('#enlaceInformeI').addClass('opcion-informe-active');
        $('#enlaceInformeA').addClass('opcion-informe');
    }
}

cargarEstadoInforme = function (idEvento, indicador) {
    var idEvento = $('#hfCodigoEvento').val();
    $.ajax({
        type: 'POST',
        url: siteRoot + "eventos/evento/informe",
        data: {
            idEvento: idEvento
        },
        success: function (evt) {
            $('#contenedorInforme').html(evt);

            setTimeout(function () {
                $('#popupInforme').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });

                $('#tablaReporte').dataTable({
                    "sPaginationType": "full_numbers",
                    "aaSorting": [[0, "desc"]],
                    "destroy": "true",
                    "sDom": 'ftp',
                });

            }, 50);
        },
        error: function () {
            mostrarError();
        }
    });
}

showInforme = function (idEmpresa, idEvento) {
    document.location.href = siteRoot + 'eventos/informe?evento=' + idEvento + '&empresa=' + idEmpresa;
}

informeConsolidado = function (idEvento, indicador) {
    document.location.href = siteRoot + 'eventos/informe?evento=' + idEvento + '&empresa=0&indicador=' + indicador;
}



openInterrupcion = function (idInforme) {
    $('#divInterrupcion').css('display', 'block');
}

closeInterrupcion = function () {
    $('#divInterrupcion').css('display', 'none');
}

procesarInterrupcion = function () {
    var idInforme = $('#hfCodigoInforme').val();
    var tipoInforme = $('#hfTipoInforme').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'importarinterrupcion',
        dataType: 'json',
        global: false,
        data: {
            idInforme: idInforme
        },
        success: function (result) {
            if (result == 1) {
                openInforme(idInforme, tipoInforme);
            }
            else if (result == -1) {
                mostrarError();
            }
            else {
                var html = '<div class="action-alert"><ul>';
                for (var error in result) {
                    html = html + '<li>' + result[error] + '</li>';
                }
                html = html + '</ul></div>';

                $('#fileInfoInterrupcion').html(html);
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}


validarNumero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode == 46) {
        var regex = new RegExp(/\./g);
        var count = $(item).val().match(regex).length;
        if (count > 1) {
            return false;
        }
    }

    if (charCode == 45) {
        var regex = new RegExp(/\-/g)
        var count = $(item).val().match(regex).length;
        if (count > 0) {
            return false;
        }
    }

    if (charCode > 31 && charCode != 45 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

validarEntero = function (item, evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    return true;
}

mostrarAlerta = function (mensaje) {
    $('#mensajeInforme').removeClass();
    $('#mensajeInforme').addClass('action-alert');
    $('#mensajeInforme').html(mensaje);
}

mostrarExito = function (mensaje) {
    $('#mensajeInforme').removeClass();
    $('#mensajeInforme').addClass('action-exito');
    $('#mensajeInforme').html(mensaje);
}