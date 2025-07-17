var ht, pop_ht, hc, dataParametros;
var isActive = false;
var arrayBloques = [];
var tempData = {};

$(document).ready(function () {
    //Aplica la lib. Zebra_Datepicker
    $('.f-fecha').Zebra_DatePicker({
        onSelect: function () {//Evento onChange
            var e = $('#id-punto').val();
            var s = $('#id-byid').val();
            var f = $('#id-fecha').val();
            if (s) {
                LoadData(s, f);
            }
            else {
                if (e) {
                    LoadData(e, f);
                }
                else {
                    SetMessage('#message',
                        'Es necesario seleccionar o ingresar un punto de medición para empezar',
                        'warning', true);
                }
            }
        }
    });

    //Aplica la lib. MultiSelect
    $('.f-select-s').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: true,
            placeholder: 'Seleccione',
            onClose: function () {
                var e = $('#id-punto').val();
                var s = $('#id-byid').val();
                var f = $('#id-fecha').val();

                if (s) {
                    LoadData(s, f);
                }
                else {
                    if (e) {
                        LoadData(e, f);
                    }
                    else {
                        SetMessage('#message',
                            'Es necesario seleccionar o ingresar un punto de medición para empezar',
                            'warning', true);
                    }
                }
            }
        });
    });

    $('.f-select-m').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: false,
            placeholder: 'Seleccione',
            onClose: function () {
                var e = document.getElementById(this.name);
                UpdateFilters(e.id);
            },
            onCheckAll: function () {
                var e = document.getElementById(this.name);
                $(e).val(null);
            }
        });
    });

    $('#id-byid').on('change', function () {
        var s = $(this).val();
        var e = $('#id-punto').val();
        var f = $('#id-fecha').val();
        if (s) {
            $('[class="f-select-m"]').each(function () {
                $(this).multipleSelect('disable');
            });
            $('#id-punto.f-select-s').each(function () {
                $(this).multipleSelect('disable');
            });
            LoadData(s, f);
        }
        else {
            $('[class="f-select-m"]').each(function () {
                $(this).multipleSelect('enable');
            });
            $('#id-punto.f-select-s').each(function () {
                $(this).multipleSelect('enable');
            });

            if (e) {
                LoadData(e, f);
            }
            else {
                SetMessage('#message',
                    'Es necesario seleccionar o ingresar un punto de medición para empezar',
                    'warning', true);
            }
        }
    });

    $('#btn-guardar').on('click', function () {
        //Obtiene datos necesarios para las validaciones
        var id = $('#id-punto').val();

        //Valida que el Handsontable este activo
        if (!isActive) {
            SetMessage('#message',
                'Debe seleccionar un punto de medición!',
                'warning', true);
            return false;
        }

        //Valida que se haya seleccionado un punto de medición
        if (!id) {
            SetMessage('#message',
                'Debe seleccionar un punto de medición!',
                'warning', true);
            return false;
        }

        //Obtiene datos para el registro
        var date = $('#id-fecha').val();
        var data = FormatMedicion(ht.getDataAtProp('final'));

        //Función para el registro
        Save(id, date, data);
    });

    $('#btn-reemplazar').on('click', function () {
        if (!isActive) return false;

        //Calcula el nuevo valor de la columna y linea 'Defecto'
        var d_patron = ht.getDataAtProp('patron');
        var d_ajuste = ht.getDataAtProp('ajuste');

        var n_defecto = [];
        $.each(d_patron, function (i, value) {
            var s = value + d_ajuste[i];
            n_defecto.push(s);
        });

        //Reemplaza la linea 'Defecto' del Highchart 
        hc.get('final').setData(n_defecto);//patrón
        
        //Obtiene el indice de la columna 'Defecto' del Handsontable
        var idx_defecto = ht.propToCol('final');

        //Obtiene los datos mostrados por hansontable
        var ht_data = HtGetDataAsProp();

        //Construye el nuevo juego de datos
        $.each(ht_data, function (i, item) {
            item['final'] = n_defecto[i];
        });

        //Actualiza el handsontable
        ht.updateSettings({ data: ht_data });

        //Mensaje
        SetMessage('#message',
            'Se reemplazaron los valores de la columna "Defecto", ' +
            'recuerde que aún es necesario guardar los cambios',
            'warning',
            true);
    });

    $('.cls-bloque').on('change', function () {
        var id = this.id;
        var valor = parseFloat(this.value);
        if (!(isNaN(valor))) {
            EditarPorBloque(id, valor);
        }
    });

    $('#id-fecha-inicio').Zebra_DatePicker();
    $('#id-fecha-fin').Zebra_DatePicker();

    $('#id-fecha-inicio-por-fecha').Zebra_DatePicker();
    $('#id-fecha-fin-por-fecha').Zebra_DatePicker();

    $('#btn-export').on('click', function () {
        pop = $('#popupExport').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                SetMessage('#pop-mensaje-export',
                    '• Seleccione el rango de fechas de los perfiles patrones antes de "Exportar".<br>' +
                    '• Se iniciará la exportación a partir de la "Fecha de inicio", esta si sera incluida.<br>' +
                    '• Se finalizará la exportación con la "Fecha de fin", esta si sera incluida.',
                    'info');
            }
        });
    });

    $('#btn-pop-exportar').on('click', function () {
        $.ajax({
            type: 'POST',
            url: controller + 'PerfilesPatronExportar',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                fechaInicio: $('#id-fecha-inicio').val(),
                fechaFin: $('#id-fecha-fin').val()
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                $("#popupExport").bPopup().close();
                // SetMessage('#message', result.dataMsg, result.typeMsg);
                if (result != -1) {
                    window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
                    mostrarMensaje('mensaje', 'exito', "Felicidades, el archivo se descargo correctamente...!");
                }
                else {
                    mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
                }
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    });

    LoadData(-1, '31/12/9999');

    //btn-export-por-fecha

    $('#btn-export-por-fecha').on('click', function () {
        pop = $('#popupExportPorFecha').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                SetMessage('#pop-mensaje-export-por-fecha',
                    '• Seleccione el rango de fechas de los perfiles patrones antes de "Exportar".<br>' +
                    '• Se iniciará la exportación a partir de la "Fecha de inicio", esta si sera incluida.<br>' +
                    '• Se finalizará la exportación con la "Fecha de fin", esta si sera incluida.',
                    'info');
            }
        });
    });



    $('#btn-pop-exportar-por-fecha').on('click', function () {
        $.ajax({
            type: 'POST',
            url: controller + 'PerfilesPatronExportarporFecha',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                fechaInicio: $('#id-fecha-inicio-por-fecha').val(),
                fechaFin: $('#id-fecha-fin-por-fecha').val()
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                $("#popupExportPorFecha").bPopup().close();
                // SetMessage('#message', result.dataMsg, result.typeMsg);
                if (result != -1) {
                    window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
                    mostrarMensaje('mensaje', 'exito', "Felicidades, el archivo se descargo correctamente...!");
                }
                else {
                    mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
                }
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    });

    /*
    $('#btn-export-por-fecha').on('click', function () {
        $.ajax({
            type: 'POST',
            url: controller + 'PerfilesPatronExportarporFecha',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                fecha: $('#id-fecha').val()
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                $("#popupExport").bPopup().close();
                // SetMessage('#message', result.dataMsg, result.typeMsg);
                if (result != -1) {
                    window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
                    console.log(controller + 'abrirarchivo?formato=' + 1 + '&file=' + result);
                    mostrarMensaje('mensaje', 'exito', "Felicidades, el archivo se descargo correctamente...!");
                } else {
                    mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
                }
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    });*/
});


//Envia la información modificada para su registro
function Save(pto, dte, dta) {
    $.ajax({
        type: 'POST',
        url: controller + 'PerfilesSave',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPunto: pto,
            regFecha: dte,
            dataMedicion: dta
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            SetMessage('#message', result.dataMsg, result.typeMsg, true);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Carga los datos del modulo
function LoadData(s_id, s_date) {
    $.ajax({
        type: 'POST',
        url: controller + 'PerfilesDatos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPunto: s_id,
            regFecha: s_date
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (Reload(result.valid)) {
                //Setea los parámetros de configuración
                dataParametros = result.config;

                //Crea el handsontable
                var ht_model = FormatHandson(result.data);
                GetHanson(ht_model);

                //Crea el highchart
                var hc_model = FormatHighchart(result.data);
                GetHighchart(hc_model, result.title);

                //Crea los calendarios
                GetCalendars(s_id, result.data);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Actualiza los filtros de busqueda
function UpdateFilters(id) {
    //Validación
    if (id == 'id-tipoempresa') $('#id-empresa').val(null);

    $.ajax({
        type: 'POST',
        url: controller + 'PerfilesUpdateFiltros',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            dataFiltros: GetFiltersValues()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            switch (id) {
                case 'main':

                    break;
                case 'id-area':
                    //Actualiza las listas subestación, empresa y puntos de medición
                    //RefillDropDowList($('#id-subestacion'), dataset_prueba, 'id', 'nombre');
                    //RefillDropDowList($('#id-empresa'), dataset_prueba, 'id', 'nombre');
                    //RefillDropDowList($('#id-punto'), dataset_prueba, 'id', 'nombre');
                    break;
                case 'id-ubicacion':
                    //Actualiza la lista de puntos de medición
                    RefillDropDowList($('#id-punto'), result.ListPtomedicion, 'Ptomedicodi', 'Ptomedidesc');
                    break;
                case 'id-tipoempresa':
                    //Actualiza la lista de empresas
                    RefillDropDowList($('#id-empresa'), result.ListEmpresa, 'Emprcodi', 'Emprnomb');
                    //Actualiza la lista de puntos de medición
                    RefillDropDowList($('#id-punto'), result.ListPtomedicion, 'Ptomedicodi', 'Ptomedidesc');
                    break;
                case 'id-empresa':
                    //Actualiza la lista de puntos de medición
                    RefillDropDowList($('#id-punto'), result.ListPtomedicion, 'Ptomedicodi', 'Ptomedidesc');
                    break;
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function UpdatePatron(s_id, s_date_a, s_date_b, slunes, meds) {
    var e = $('#id-punto').val();
    var s = $('#id-byid').val();

    var s_pto = (s) ? s : e;

    $.ajax({
        type: 'POST',
        url: controller + 'PerfilesUpdatePatron',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPunto: s_pto,
            regFechaA: s_date_a,
            regFechaB: s_date_b,
            esLunes: slunes,
            tipoPatron: dataParametros.Prncfgtipopatron,
            dsvPatron: dataParametros.Prncfgporcdsvptrn,
            dataMediciones: meds
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            //Actualiza las series del hc
            hc.get(s_id).setData(result.medicion);//medición
            hc.get('patron').setData(result.patron);//patrón
            hc.get('rmin').setData(result.emin);//emin
            hc.get('rmax').setData(result.emax);//emax

            //Actualiza la leyenda de la medición en el hc
            hc.get(s_id).update({ name: s_date_a });

            //Actualiza las columnas del ht
            var idx_med = ht.propToCol(s_id);
            
            var ht_data = HtGetDataAsProp();
            $.each(ht_data, function (i, item) {
                item[s_id] = result.medicion[i];
                item['patron'] = result.patron[i];
            });

            //Actualiza el titulo de la columna en el ht
            var ht_headers = ht.getColHeader();
            ht_headers[idx_med] = s_date_a;

            ht.updateSettings({ colHeaders: ht_headers, data: ht_data });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Formatea el modelo de datos para el Handsontable
function FormatHandson(model) {
    var handson = {};
    var data = [], columns = [], headers = [];
    var rule = ['no'];

    $.each(model, function (i, item) {
        //crea la propiedad "colHeaders"
        if (!(rule.includes(item.htrender))) {
            headers.push(item.label);
        }

        //Crea la propiedad "columns"
        var col = {};
        col['data'] = item.id;
        //col['title'] = item.label;

        switch (item.htrender) {
            case 'hora':
                col['type'] = 'text';
                col['className'] = 'htCenter';
                col['readOnly'] = true;
                col['renderer'] = HoraColumnRenderer;
                break;
            case 'normal':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = true;
                break;
            case 'patron':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = true;
                col['renderer'] = PatronColumnRenderer;
                break;
            case 'edit':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = false;
                col['allowInvalid'] = false;
                col['allowEmpty'] = false;
                break;
            case 'final':
                col['type'] = 'numeric';
                col['format'] = '0.00';
                col['readOnly'] = true;
                col['renderer'] = PatronColumnRenderer;
        }

        if (!(rule.includes(item.htrender))) {
            columns.push(col);

            //Crea la propiedad "data"
            $.each(item.data, function (j, value) {
                var row = {};
                if (data[j]) {
                    data[j][item.id] = value;
                }
                else {
                    row[item.id] = value;
                    data.push(row);
                }
            });
        }
    });

    handson['colHeaders'] = headers;
    handson['columns'] = columns;
    handson['data'] = data;
    handson['maxCols'] = columns.length;
    handson['maxRows'] = data.length;
    return handson;
}

//Crea los calendarios correspondientes a las mediciones historicas
//que conforman el perfil patrón
function GetCalendars(id, model) {
    //valida la carga inicial
    if (id == -1) return false;

    //Limpia los remanentes del Zebra_DatePicker
    var zd_m = $('.med-m');
    $.each(zd_m, function (i, item) {
        var x = $(item).data('Zebra_DatePicker');
        x.destroy();
    });

    var zd_t = $('.med-t');
    $.each(zd_t, function (i, item) {
        var x = $(item).data('Zebra_DatePicker');
        x.destroy();
    });

    //Limpia el contenedor
    $('#zd').html('');

    $.each(model, function (i, item) {
        if (item.id.includes('med')) {
            //Contenedor del grupo
            var ctn = $('<div class="simple-box"><div>');
            ctn.css('margin-bottom', '10px');
            ctn.css('padding', '5px 0px 5px 9px');
            ctn.css('border-radius', '4px');

            //Crea el calendario "intervalo completo" o "parcial mañana"
            var itp_m = $('<input type="text" class="med-m"/>');
            itp_m.data('id', item.id);//Id correspondiente a la medición

            itp_m.css('width', '100px');
            itp_m.css('margin-bottom', '5px');
            itp_m.val(item.label);
            ctn.append(itp_m);
            
            //Crea el calendario "parcial tarde"
            var itp_t = $('<input type="text" class="med-t"/>');
            itp_t.data('id', item.id);//Id correspondiente a la medición

            itp_t.css('width', '100px');
            if (!item.slunes) itp_t.css('display', 'none');
            itp_t.val(item.label2);
            ctn.append(itp_t);

            $('#zd').append(ctn);
        }
    });

    //Agrega Zebra_Datepicker a los calendarios y sus eventos
    $('.med-m').Zebra_DatePicker({
        onSelect: function () {
            var s_lunes = false;
            var s_dia = GetDia(this.val());
            var elem_t = this.parents('.simple-box').find('.med-t');

            //Valida si se debe o no mostrar el calendario "parcial tarde"
            if (s_dia == 0) {
                s_lunes = true;
                var s_date = $('#id-fecha').val();
                if (GetDia(s_date) != 0) elem_t.val(this.val());
                elem_t.css('display', 'inline-block');
                elem_t.parent().css('display', 'inline-block');
            }
            else {
                elem_t.css('display', 'none');
                elem_t.parent().css('display', 'none');
            }

            //Actualiza el perfil patrón
            var meds = GetMediciones(this.data('id'));
            UpdatePatron(this.data('id'), this.val(), elem_t.val(), s_lunes, meds);
        }
    });

    $('.med-t').Zebra_DatePicker({
        onSelect: function () {
            var elem_m = this.parents('.simple-box').find('.med-m');
            var meds = GetMediciones(this.data('id'));
            UpdatePatron(this.data('id'), elem_m.val(), this.val(), true, meds);
        }
    });
}

//Crea el Handsontable
function GetHanson(model) {
    var container = document.getElementById('ht');
    ht = new Handsontable(container, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        colHeaders: model.colHeaders,
        afterChange: function (changes, source) {
            if (ValidarEventosHandsontable(source)) {
                htBaseIndex = ht.propToCol('patron');//Setea la columna base (Handsontable)
                htDestinoIndex = ht.propToCol('final');//Setea la columna final (Handsontable)

                hcSerieIndex = hc.get('final').index;//Setea el indice de la linea que se modificara (Highchart)

                for (var i = 0; i < changes.length; i++) {
                    htRowIndex = changes[i][0]; htRowData = ht.getDataAtRow(htRowIndex);

                    htSuma = parseFloat(changes[i][3]);//Agrega el ajuste realizado

                    if (arrayBloques[htRowIndex] != null) {
                        htSuma += parseFloat(arrayBloques[htRowIndex]);//Suma el valor de los bloques
                    }

                    if (htRowData[htBaseIndex] != null) {
                        htSuma += parseFloat(htRowData[htBaseIndex]);//Suma el valor de "Entrada"
                    }

                    ht.setDataAtCell(htRowIndex, htDestinoIndex, htSuma, 'sum');//Realiza la modificación

                    if (source != 'grafico') {//El evento "grafico" modifica la grafica previamente
                        hc.series[hcSerieIndex].data[htRowIndex].update(htSuma);
                    }
                }
            }
        },
        afterInit: function () {
            isActive = true;
        }
    });
}

//Formatea el modelo de datos para el Highchart
function FormatHighchart(model) {
    var highchart = {};
    var categories;
    var series = [];
    var rule = ['categoria', 'no'];
    $.each(model, function (i, item) {
        var serie = {};
        switch (item.hcrender) {
            case 'categoria':
                categories = item.data;
                break;
            case 'normal':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['marker'] = { enabled: true, symbol: 'circle' };
                break;
            case 'patron':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['lineWidth'] = 6;
                serie['marker'] = { enabled: true, symbol: 'circle' };
                break;
            case 'margen':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['color'] = '#000000';
                serie['dashStyle'] = 'dot';
                serie['marker'] = { enabled: false, symbol: 'circle' };
                break;
            case 'final':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['lineWidth'] = 6;
                serie['marker'] = { enabled: true, symbol: 'circle' };
                serie['draggableY'] = true;
                break;
        }

        if (!(rule.includes(item.hcrender))) series.push(serie);
    });

    highchart['categories'] = categories;
    highchart['series'] = series;
    return highchart;
}

//Crea el Highchart
function GetHighchart(model, title) {
    hc = Highcharts.chart('hc', {
        chart: {
            type: 'spline',
            backgroundColor: 'transparent',
            plotShadow: false,
            zoomType: 'xy',
        },
        title: {
            text: title
        },
        credits: {
            enabled: false
        },
        line: {
            cursor: 'ns-resize'
        },
        legend: {
            itemDistance: 15,
            verticalAlign: 'top',
            symbolWidth: 10,
            itemStyle: {
                fontWeight: 'normal'
            },
            y: 0
        },
        tooltip: { enabled: true },
        plotOptions: {
            series: {
                stickyTracking: false,
                marker: { enabled: true, radius: 3 },
                point: {
                    events: {
                        drop: function () {//Actualiza la columna Ajuste del Handsontable                            
                            var a = ht.getDataAtRowProp(this.x, 'ajuste');
                            var hc_ajuste = Highcharts.numberFormat(this.y - this.dragStart.y + a, 2);
                            ht.setDataAtRowProp(this.x, 'ajuste', hc_ajuste, 'grafico');
                        }
                    }
                }
            }
        },
        xAxis: {
            tickInterval: 1,
            categories: model.categories,
            labels: { rotation: 90 }
        },
        yAxis: {
            title: ''
        },
        tooltip: {
            crosshairs: true,
            shared: true
        },
        series: model.series
    });
}

//Setea el modelo de envio de información
function GetFiltersValues() {
    var model = {};
    model['SelById'] = $('#id-byid').val();
    model['SelUbicacion'] = $('#id-ubicacion').val();
    model['SelTipoEmpresa'] = $('#id-tipoempresa').val();
    model['SelEmpresa'] = $('#id-empresa').val();
    return model;
}

//Convierte una fecha al formato YYYY-MM-DD
function GetDia(f) {
    var f_dia;

    var f_array = f.split('/');
    var f_convert = f_array[2] + '-' + f_array[1] + '-' + f_array[0];

    var f_date = new Date(Date.parse(f_convert));
    f_dia = f_date.getDay();

    return f_dia;
}

//Obtiene las mediciones historicas que forman el perfil patrón
function GetMediciones(s_med) {
    if (!hc) return false;

    var res = [];
    var meds = hc.userOptions.series;
    $.each(meds, function (i, item) {
        if (item.id.includes('med')) {
            if (item.id != s_med) res.push(item.data);
        }
    });

    return res;
}

//Llena el contenido de una lista desplegable
function RefillDropDowList(element, data, data_id, data_name) {
    //Vacia el elemento
    element.empty();
    //Carga el elemento
    $.each(data, function (i, item) {
        var n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];
        element.append($('<option></option>').val(n_value).html(n_html));
    });
    //Actualiza la lib.multipleselect
    element.multipleSelect('refresh');
}

//Permite ajustar los intervalos definidos en los rangos bloques
function EditarPorBloque(id, valor) {
    var limIni, limFin;
    var blqSuma, blqFila;
    var blqCambios = [];
    var htAjusteIndex = ht.propToCol('ajuste');
    var factor = 1;

    switch (id) {
        case 'base':
            limIni = 0 * factor;
            limFin = 15 * factor;
            break;
        case 'media':
            limIni = 15 * factor;
            limFin = 35 * factor;
            break;
        case 'punta':
            limIni = 35 * factor;
            limFin = 48 * factor;
            break;
    }

    for (var i = limIni; i < limFin; i++) {
        arrayBloques[i] = valor;
        dataAjuste = ht.getDataAtRowProp(i, 'ajuste');
        blqFila = [];
        blqFila = [i, 'ajuste', dataAjuste];
        blqCambios.push(blqFila);
    }
    ht.setDataAtRowProp(blqCambios);
}

//Valida los eventos permitidos en el Handsontable
function ValidarEventosHandsontable(evento) {
    var valid = false;
    var ListEventos = ['edit', 'autofill', 'paste', 'grafico'];

    for (var i = 0; i < ListEventos.length; i++) {
        if (evento == ListEventos[i]) {
            valid = true;
        }
    }

    return valid;
}

//Estilos de las columnas del Handsontable
function HoraColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.background = '#F2F2F2';
}

function PatronColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.NumericRenderer.apply(this, arguments);
    td.style.background = '#edf5fc';
}

//Desactiva el flag "isActive" del handsontable
//Limpia el contenedor "ht"
//Limpia los bloques
//muestra u oculta el panel "workspace"
function Reload(valid) {
    isActive = false;
    $('#ht').html('');
    arrayBloques = [];
    $('.cls-bloque').val('');
    if (valid) $('#workspace').css('display', 'flex');
    else $('#workspace').css('display', 'none');
    return valid;
}

//Formatea los datos de un arreglo a un modelo
//tipo Medición por intervalos(H1, H2, ...)
function FormatMedicion(data) {
    var res = {};
    $.each(data, function (i, item) {
        var s = i + 1;
        var prop = 'H' + s;
        res[prop] = item;
    });
    
    return res;
}

function HtGetDataAsProp() {
    if (!isActive) return false;

    var res = [];
    var s_max = ht.countRows();
    var s_schema = ht.getSchema();
    for (var i = 0; i < s_max; i++) {
        var s_row = {};
        $.each(s_schema, function (prop, value) {
            s_row[prop] = ht.getDataAtRowProp(i, prop);
        });
        res.push(s_row);
    }
    return res;
}
