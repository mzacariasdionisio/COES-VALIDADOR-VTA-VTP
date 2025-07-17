var controlador = siteRoot + 'Equipamiento/equipo/';

var IMG_INFO_CHECK_CERO = '<img src="' + siteRoot + 'Content/Images/ico-info.gif" alt="" width="16" height="16" >';

var hot, LISTA_DATA, LISTA_FLAG_VIGENCIA_CORRECTA;
var HABILITADO_EDITAR = false;

$(function () {
    $('#cntMenu').css("display", "none");

    $('#btnRegresar').click(function () {
        history.back(1);
    });

    $('#btnEnable').click(function () {
        $("#btnEnable").hide();
        $("#btnSave").show();
        HABILITADO_EDITAR = true;

        mostrarGrillaExcel();
    });
    $('#btnSave').click(function () {
        grabarDatos();
    });


    $('#fecha_consulta').Zebra_DatePicker({
        onSelect: function (date) {
            mostrarGrillaExcel();
        }
    });
    $('input[type=radio][name=rbFiltro]').change(function () {
        mostrarGrillaExcel();
    });
    //$('input[type=radio][name=rbOrden]').change(function () {
    //    mostrarGrillaExcel();
    //});

    mostrarGrillaExcel();

});

mostrarGrillaExcel = function () {
    $(".leyenda_vig").show();

    var habilitarEdicion = HABILITADO_EDITAR;

    $("#propiedadesEquipo").html('');
    if (typeof hot != 'undefined') {
        hot.destroy();
    }

    var iCodEquipo = $("#hfEquipo").val();

    var fecha = $("#fecha_consulta").val();
    var filtroFicha = $('input[name=rbFiltro]:checked').val();
    //var orden = $('input[name=rbOrden]:checked').val();

    $.ajax({
        type: 'POST',
        url: controlador + "PropiedadesHoja",
        data: {
            iEquipo: iCodEquipo,
            fecha: fecha,
            filtroFicha: filtroFicha,
            //orden: orden,
            habilitarEdicion: habilitarEdicion
        },
        success: function (result) {
            if (result.Resultado != '-1') {
                crearGrilla(result, habilitarEdicion);
            }
            else
                mostrarError("Ha ocurrido un error: " + result.Mensaje);
        },
        error: function (err) {
            mostrarError('Ha ocurrido un error');
        }
    });
};

mostrarHistoricoPropiedad = function (propcodi) {
    var iCodEquipo = $("#hfEquipo").val();
    $.ajax({
        type: 'POST',
        url: controlador + "HistoricoPropiedadEquipo",
        data: {
            iEquipo: iCodEquipo,
            iPropiedad: propcodi
        },
        success: function (evt) {
            $('#historicoPropiedad').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popupHistorico').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

        },
        error: function (err) {
            mostrarError('Ha ocurrido un error.');
        }
    });
};

grabarDatos = function () {
    if (confirm("¿Desea guardar los cambios efectuados?")) {

        var fecha = $("#fecha_consulta").val();
        var filtroFicha = $('input[name=rbFiltro]:checked').val();
        //var orden = $('input[name=rbOrden]:checked').val();

        var dataJson = {
            fecha: fecha,
            listaPropequi: hot.getSourceData(),
            iEquipo: $("#hfEquipo").val(),
            filtroFicha: filtroFicha
            //orden: orden
        };

        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            url: controlador + "GrabarDatosPropiedades",
            data: JSON.stringify(dataJson),
            beforeSend: function () {
                mostrarAlerta("Enviando Información ..");
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    $('#mensaje').hide();
                    mostrarGrillaExcel();
                } else {
                    mostrarError("Error al Grabar: " + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarError('Ha ocurrido un error');
            }
        });
    }
};
function mostrarExito(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

function mostrarError(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}

function mostrarMensaje(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

//////////////////////////////////////////////////////////////
function crearGrilla(result, habilitarEdicion) {
    var container = document.getElementById('propiedadesEquipo');

    var handson = result.Handson;
    var columns = handson.Columnas;
    var headers = handson.Headers;
    var widths = handson.ListaColWidth;
    var nestedHeader = obtenerCabeceraPersonalizada(handson.NestedHeader.ListCellNestedHeaders);

    LISTA_DATA = Array.from(result.ListaPropiedad);
    LISTA_FLAG_VIGENCIA_CORRECTA = result.ListaFlagVigenciaCorrecta;

    var readOnly = !habilitarEdicion;

    hot = new Handsontable(container, {
        data: result.ListaPropiedad,
        autoRowSize: true,
        dataSchema: {
            Propcodi: null,
            Propnomb: null,
            Propnombficha: null,
            Propunidad: null,
            FechapropequiDesc: null,
            Valor: null,
            Propequicheckcero: null,
            Propequicomentario: null,
            Equiabrev: null,
            Propequisustento: null,
            //Propequiobservacion: null,
            UltimaModificacionUsuarioDesc: null,
            UltimaModificacionFechaDesc: null,
            Equinomb: null,
        },
        filters: true,
        stretchH: "all",
        colHeaders: headers,
        nestedHeaders: nestedHeader,
        colWidths: widths,
        columns: columns,
        fillHandle: true,
        rowHeaders: true,
        height: 500,
        mergeCells: [],
        readOnly: readOnly,
        //afterGetColHeader: addInput,
        //beforeOnCellMouseDown: doNotSelectColumn,
        cells: function (row, col, prop) {
            var cellProperties = {
            };

            if (row >= 0) {
                //celdas de solo lectura cuando el excel web es editable:
                var tieneCeldaNoEditable = handson.MatrizTipoEstado[row][col] < 0;

                if (col < 4 || col == 5 || col == 7 || col == 9) {
                    if (HABILITADO_EDITAR) {
                        if (tieneCeldaNoEditable) {
                            cellProperties.renderer = renderer_celda_no_editable;
                        }
                    }
                }

                if (HABILITADO_EDITAR) {
                    if (tieneCeldaNoEditable) {
                        cellProperties.readOnly = true;
                    }
                }

                //filas cuerpo
                if (col == 4) {
                    if (HABILITADO_EDITAR) {
                        cellProperties.renderer = !tieneCeldaNoEditable ? vigencia_renderer : renderer_celda_no_editable;
                    }
                }

                //check
                if (col == 6) {
                    if (HABILITADO_EDITAR) {
                        cellProperties.renderer = !tieneCeldaNoEditable ? check_renderer : check_renderer_no_editable;
                    } else {
                        cellProperties.renderer = check_renderer_solo_lectura;
                    }
                }

                //columna descargar archivo
                if (col == 8) {
                    if (HABILITADO_EDITAR) {
                        cellProperties.renderer = !tieneCeldaNoEditable ? row_descargar_sustento_renderer : row_descargar_sustento_renderer_no_editable;
                    } else {
                        cellProperties.renderer = row_descargar_sustento_renderer_solo_lectura
                    }
                }

                //columna ver historial (10)
                if (col == columns.length - 1) {
                    if (HABILITADO_EDITAR) {
                        cellProperties.renderer = !tieneCeldaNoEditable ? row_popup_hist_renderer : row_popup_hist_renderer_no_editable
                    } else {
                        cellProperties.renderer = row_popup_hist_renderer_solo_lectura
                    }
                }
            }

            return cellProperties;
        }
    });

    hot.render();

    $('#searchgrid').on('keyup', function (event) {
        var value = ('' + this.value).toLowerCase(), row, col, r_len, c_len, td;
        var example = $('#propiedadesEquipo');
        var data = LISTA_DATA;
        var searcharray = [];
        if (value) {
            for (row = 0, r_len = data.length; row < r_len; row++) {
                if (data[row].Propcodi != null) {
                    if (('' + data[row].Propcodi).toLowerCase().indexOf(value) > -1) {
                        searcharray.push(data[row]);
                        continue;
                    }
                }
                if (data[row].Propnomb != null) {
                    if (('' + data[row].Propnomb).toLowerCase().indexOf(value) > -1) {
                        searcharray.push(data[row]);
                        continue;
                    }
                }
                if (data[row].Propnombficha != null) {
                    if (('' + data[row].Propnombficha).toLowerCase().indexOf(value) > -1) {
                        searcharray.push(data[row]);
                        continue;
                    }
                }
                if (data[row].Propunidad != null) {
                    if (('' + data[row].Propunidad).toLowerCase().indexOf(value) > -1) {
                        searcharray.push(data[row]);
                        continue;
                    }
                }
                if (data[row].FechapropequiDesc != null) {
                    if (('' + data[row].FechapropequiDesc).toLowerCase().indexOf(value) > -1) {
                        searcharray.push(data[row]);
                        continue;
                    }
                }
                if (data[row].Valor != null) {
                    if (('' + data[row].Valor).toLowerCase().indexOf(value) > -1) {
                        searcharray.push(data[row]);
                        continue;
                    }
                }
                if (data[row].Propequicomentario != null) {
                    if (('' + data[row].Propequicomentario).toLowerCase().indexOf(value) > -1) {
                        searcharray.push(data[row]);
                        continue;
                    }
                }
                if (data[row].Propequisustento != null) {
                    if (('' + data[row].Propequisustento).toLowerCase().indexOf(value) > -1) {
                        searcharray.push(data[row]);
                        continue;
                    }
                }
                if (data[row].UltimaModificacionUsuarioDesc != null) {
                    if (('' + data[row].UltimaModificacionUsuarioDesc).toLowerCase().indexOf(value) > -1) {
                        searcharray.push(data[row]);
                        continue;
                    }
                }
                if (data[row].UltimaModificacionFechaDesc != null) {
                    if (('' + data[row].UltimaModificacionFechaDesc).toLowerCase().indexOf(value) > -1) {
                        searcharray.push(data[row]);
                        continue;
                    }
                }

            }
            hot.loadData(searcharray);
        }
        else {
            hot.loadData(LISTA_DATA);
        }
    });

}

function check_renderer(instance, td, row, col, prop, value, cellPropertie) {
    Handsontable.renderers.CheckboxRenderer.apply(this, arguments);

    td = instance.getCell(row, col);
    var valorNuevo = instance.getDataAtCell(row, col - 1);
    if (valorNuevo == "0") {
        $(`input[class="htCheckboxRendererInput"][data-row="${row}"][data-col="${col}"]`).prop("disabled", false);
    }
    else {
        $(`input[class="htCheckboxRendererInput"][data-row="${row}"][data-col="${col}"]`).prop("disabled", true);
    }
}

function check_renderer_solo_lectura(instance, td, row, col, prop, value, cellPropertie) {
    Handsontable.renderers.CheckboxRenderer.apply(this, arguments);

    td.style.background = '#FFE4C4';
    $(`input[class="htCheckboxRendererInput"][data-row="${row}"][data-col="${col}"]`).prop("disabled", true);
}

function check_renderer_no_editable(instance, td, row, col, prop, value, cellPropertie) {
    Handsontable.renderers.CheckboxRenderer.apply(this, arguments);

    $(td).addClass(" htDisabled ");
    $(`input[class="htCheckboxRendererInput"][data-row="${row}"][data-col="${col}"]`).prop("disabled", true);
}

function _row_popup_hist_renderer(instance, td, row, col, prop, value, cellProperties) {
    $(td).children('.btn').remove();
    var div = document.createElement('div');
    div.className = 'btn';
    div.appendChild(document.createTextNode(" "));
    td.appendChild(div);

    $(div).parent().unbind();

    $(div).parent().on('mouseup', function () {
        var propcodi = instance.getDataAtCell(row, 0);
        mostrarHistoricoPropiedad(propcodi);
    });
    $(td).addClass("estiloHistorialProp");
    $(td).prop('title', 'Ver Historial');
}
function row_popup_hist_renderer(instance, td, row, col, prop, value, cellProperties) {
    _row_popup_hist_renderer(instance, td, row, col, prop, value, cellProperties);

    return td;
}
function row_popup_hist_renderer_no_editable(instance, td, row, col, prop, value, cellProperties) {
    _row_popup_hist_renderer(instance, td, row, col, prop, value, cellProperties);
    td.style.backgroundColor = '#e0e0e0';

    return td;
}
function row_popup_hist_renderer_solo_lectura(instance, td, row, col, prop, value, cellProperties) {
    _row_popup_hist_renderer(instance, td, row, col, prop, value, cellProperties);
    $(td).addClass(" celda_sololectura ");

    return td;
}

function _row_descargar_sustento_renderer(instance, td, row, col, prop, value, cellProperties) {
    $(td).children('.btn').remove();
    $(td).css('line-height', '10px');
    $(td).html('');
    //$(div).parent().unbind();

    //mostrar icono cuando exista url de descarga
    var urlSustento = instance.getDataAtCell(row, 9);
    if (urlSustento != "" && urlSustento != null) {

        var arrayLink = getListaEnlaceXTexto(urlSustento);
        var htmlDiv = '';
        for (var i = 0; i < arrayLink.length; i++) {
            var link = arrayLink[i];
            var esConfidencial = (link.toLocaleUpperCase()).includes('DescargarSustentoConfidencial?'.toLocaleUpperCase());
            var textoTitle = esConfidencial ? 'Descargar archivo de sustento (CONFIDENCIAL)' : 'Descargar archivo de sustento';

            htmlDiv += `
                <div class='estiloSustentoProp' title='${textoTitle} - ${link}' onclick='descargarArchivoSustento("${link}")'>
                    
                </div>
            `;
        }
        $(td).html(htmlDiv);
    }
}
function row_descargar_sustento_renderer(instance, td, row, col, prop, value, cellProperties) {
    _row_descargar_sustento_renderer(instance, td, row, col, prop, value, cellProperties)
    return td
}
function row_descargar_sustento_renderer_no_editable(instance, td, row, col, prop, value, cellProperties) {
    _row_descargar_sustento_renderer(instance, td, row, col, prop, value, cellProperties);
    td.style.backgroundColor = '#e0e0e0';

    return td;
}
function row_descargar_sustento_renderer_solo_lectura(instance, td, row, col, prop, value, cellProperties) {
    _row_descargar_sustento_renderer(instance, td, row, col, prop, value, cellProperties);
    $(td).addClass(" celda_sololectura ");

    return td;
}

function vigencia_renderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    if (value != null) {
        if (LISTA_FLAG_VIGENCIA_CORRECTA != null && LISTA_FLAG_VIGENCIA_CORRECTA.length > 0) {
            var fechaExistente = LISTA_DATA[row].FechapropequiDesc;

            var colorTexto = '';
            if (value == fechaExistente) {//si el usuario no modifica la fecha de vigencia mantener los colores
                if (row <= LISTA_FLAG_VIGENCIA_CORRECTA.length) {
                    colorTexto = LISTA_FLAG_VIGENCIA_CORRECTA[row] == 1 ? 'green' : '#DADA20';
                }
            } else {
                colorTexto = "#245C86";
            }

            var objCss = {
                "color": colorTexto,
            };

            $(td).css(objCss);
        }
    }

    return td;
}

function renderer_celda_no_editable(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    $(td).addClass(" htDisabled ");
}
function renderer_celda_solo_lectura(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    $(td).addClass(" celda_sololectura ");
}

function obtenerCabeceraPersonalizada(lstNestedHeaders) {

    var nestedHeaders = [];

    lstNestedHeaders.forEach(function (currentValue, index, array) {
        var nestedHeader = [];
        currentValue.forEach(function (item) {
            if (item.Colspan == 0) {
                var title = item.Title != undefined && item.Title != null ? item.Title : '';
                var label = item.Label;
                if (item.Class == "icono_check_cero") label = IMG_INFO_CHECK_CERO;

                nestedHeader.push(`<span class='${item.Class}' title='${title}'> ${label} </span>`);
                //nestedHeader.push(item.Label);
            } else {
                //nestedHeader.push({ label: "<div class='prueba'>" + item.Label + " </div>", colspan: item.Colspan });
                nestedHeader.push({ label: item.Label, colspan: item.Colspan });
            }
        });

        nestedHeaders.push(nestedHeader);
    });

    return nestedHeaders;
}

function getListaEnlaceXTexto(texto) {
    if (texto == null || texto == undefined) texto = "";
    texto = texto.trim();

    texto = texto.replace(/(?:\r\n|\r|\n)/g, ' ');
    texto = texto.replace(/(\n)+/g, ' ');

    var arrayLink = [];
    var arraySep = texto.split(' ');
    for (var i = 0; i < arraySep.length; i++) {
        var posibleLink = arraySep[i].trim();
        if (posibleLink.length > 0 && (
            posibleLink.toLowerCase().startsWith('http') || posibleLink.toLowerCase().startsWith('www'))) {
            arrayLink.push(posibleLink);
        }
    }

    return arrayLink;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Archivos de sustento
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function descargarArchivoSustento(urlSustento) {
    if (urlSustento != "" && urlSustento != null) {

        window.open(urlSustento, '_blank').focus();

        //var esFileApp = (urlSustento.toLocaleUpperCase()).includes('DescargarSustentoConfidencial?'.toLocaleUpperCase())
        //    || (urlSustento.toLocaleUpperCase()).includes('DescargarSustento?'.toLocaleUpperCase());

        //if (esFileApp) {
        //    $.ajax({
        //        type: 'POST',
        //        dataType: 'json',
        //        traditional: true,
        //        url: controlador + 'DescargarArchivoSustento',
        //        data: {
        //        },
        //        cache: false,
        //        success: function (result) {
        //            if (result.Resultado == '-1') {
        //                alert('Ha ocurrido un error:' + result.Mensaje);
        //            } else {
        //                var urlFileApp = urlSustento.split('?')[0];
        //                var paramUrlArchivo = (urlSustento.split('?')[1]).split('=')[1];
        //                paramUrlArchivo = decodeURIComponent(paramUrlArchivo.replace(/\+/g, " "));

        //                var paramList = [
        //                    { tipo: 'input', nombre: 'url', value: paramUrlArchivo },
        //                    { tipo: 'input', nombre: 'user', value: result.Resultado }
        //                ];
        //                var form = CreateFormArchivo(urlFileApp, paramList);
        //                document.body.appendChild(form);
        //                form.submit();
        //            }
        //        },
        //        error: function (err) {
        //            alert('Ha ocurrido un error.');
        //        }
        //    });
        //} else {
        //    //var element = document.createElement('a');
        //    //element.setAttribute('href', ' ' + urlSustento);
        //    //element.setAttribute('download', "archivo");
        //    //document.body.appendChild(element);
        //    //element.click();
        //    window.open(urlSustento, '_blank').focus();
        //}
    }
}
