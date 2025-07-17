var ht, pop_ht, hc, dataParametros;
var isActive = false;
var arrayBloques = [];

$(document).ready(function () {
    //Aplica la lib. Zebra_Datepicker
    $('.f-fecha').Zebra_DatePicker({
        onSelect: function () {//Evento onChange
            LoadData();
        }
    });

    //Aplica la lib. MultiSelect
    $('.f-select').each(function () {
        var element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: false,
            single: true,
            placeholder: 'Seleccione',
            onClose: function () {
                LoadData();
            }
        });
    });
    
    $('.cls-bloque').on('change', function () {
        var id = this.id;
        var valor = parseFloat(this.value);
        if (!(isNaN(valor))) {
            EditarPorBloque(id, valor);
        }
    });

    $('#btn-guardar').on('click', function () {
        x = $('#id-area').val();
        if (x == $('#main').data('sein')) return false;

        //Obtiene datos para el registro
        var data = FormatMedicion(ht.getDataAtProp('ajuste'));

        //Función para el registro
        Save(data);
    });
    
    LoadData();
});

//Carga los datos del modulo
function LoadData() {
    $.ajax({
        type: 'POST',
        url: controller + 'VegetativaDatos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idArea: $('#id-area').val(),
            regFecha: $('#id-fecha').val()
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            //Recarga los componentes
            Reload();

            //Setea los parámetros de configuración
            dataParametros = result.cfg;

            //Crea el handsontable
            var ht_model = FormatHandson(result.data);
            GetHanson(ht_model);

            //Crea el highchart
            var hc_title = $("#id-area option:selected").html();
            var hc_model = FormatHighchart(result.data);
            GetHighchart(hc_model, hc_title);

            //Crea los calendarios
            x = $('#id-area').val();
            if (x != $('#main').data('sein')) {
                GetCalendars(result.data, false);
            }
            else {
                GetCalendars(result.data, true);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

//Envia la información modificada para su registro
function Save(data) {
    $.ajax({
        type: 'POST',
        url: controller + 'VegetativaSave',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPunto: $('#id-area').val(),
            regFecha: $('#id-fecha').val(),
            dataMedicion: data
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

function UpdatePatron(s_id, s_date_a, s_date_b, slunes, meds) {
    $.ajax({
        type: 'POST',
        url: controller + 'VegetativaUpdatePatron',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idPunto: $('#id-area').val(),
            regFechaA: s_date_a,
            regFechaB: s_date_b,
            esLunes: slunes,
            tipoPatron: dataParametros.Prncfgtipopatron,
            dataMediciones: meds
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            //Actualiza las series del hc
            hc.get(s_id).setData(result.medicion);//medición
            hc.get('patron').setData(result.patron);//patrón

            //Actualiza la leyenda de la medición en el hc
            hc.get(s_id).update({ name: s_date_a });
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
function GetCalendars(model, esSein) {
    //valida la carga inicial
    //if (id == -1) return false;

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

    //Validación
    if (esSein) return false;

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
                htBaseIndex = ht.propToCol('demanda');//Setea la columna base (Handsontable)
                htDestinoIndex = ht.propToCol('vegetativa');//Setea la columna final (Handsontable)
                htULibresIndex = ht.propToCol('usulib');
                htSurIndex = ht.propToCol('sur');
                htNorteIndex = ht.propToCol('norte');
                htSCentroIndex = ht.propToCol('scentro');

                hcSerieIndex = hc.get('vegetativa').index;//Setea el indice de la linea que se modificara (Highchart)

                for (var i = 0; i < changes.length; i++) {
                    htRowIndex = changes[i][0]; htRowData = ht.getDataAtRow(htRowIndex);

                    //Agrega el ajuste realizado
                    htSuma = parseFloat(changes[i][3]);

                    //Suma el valor de los bloques
                    if (arrayBloques[htRowIndex] != null) {
                        htSuma += parseFloat(arrayBloques[htRowIndex]);
                    }

                    //Suma el valor de "Entrada"
                    if (htRowData[htBaseIndex] != null) {
                        htSuma += parseFloat(htRowData[htBaseIndex]);
                    }

                    //Suma el valor de "Usuarios libres"
                    if (htRowData[htULibresIndex] != null) {
                        htSuma -= parseFloat(htRowData[htULibresIndex]);
                    }
                    
                    //Suma el valor de "Sur"
                    if (!isNaN(htSurIndex)) {
                        if (htRowData[htSurIndex] != null) {
                            htSuma += parseFloat(htRowData[htSurIndex]);
                        }
                    }

                    //Suma el valor de "Norte"
                    if (!isNaN(htNorteIndex)) {
                        if (htRowData[htNorteIndex] != null) {
                            htSuma += parseFloat(htRowData[htNorteIndex]);
                        }
                    }

                    //Suma el valor de "SCentro"
                    if (!isNaN(htSCentroIndex)) {
                        if (htRowData[htSCentroIndex] != null) {
                            htSuma += parseFloat(htRowData[htSCentroIndex]);
                        }
                    }

                    //Realiza la modificación
                    ht.setDataAtCell(htRowIndex, htDestinoIndex, htSuma, 'sum');

                    //El evento "grafico" modifica la grafica previamente
                    if (source != 'grafico') {
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
            case 'final_noedit':
                serie['id'] = item.id;
                serie['name'] = item.label;
                serie['data'] = item.data;
                serie['lineWidth'] = 6;
                serie['marker'] = { enabled: true, symbol: 'circle' };
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
        if (evento == ListEventos[i]) valid = true;
    }
    return valid;
}

//Desactiva el flag "isActive" del handsontable
//Limpia el contenedor "ht"
//Limpia los bloques
//muestra u oculta el panel "workspace"
function Reload() {
    $('#ht').html('');
    arrayBloques = [];
    $('.cls-bloque').val('');

    //Habilita o deshabilita la edición por bloques
    x = $('#id-area').val();
    if (x != $('#main').data('sein')) {
        $('.cls-bloque').prop('disabled', false);
    }
    else {
        $('.cls-bloque').prop('disabled', true);
    }
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

//Formatea los datos de un arreglo a un modelo
//tipo Medición por intervalos(H1, H2, ...)
function FormatMedicion(data) {
    var res = {};
    $.each(data, function (i, item) {
        var s = i + 1;
        var prop = 'H' + s;
        res[prop] = item;
    });

    //Agrega los valores de los bloques al ajuste
    $.each(arrayBloques, function (i, item) {
        var s = i + 1;
        var prop = 'H' + s;
        res[prop] += item;
    });

    return res;
}

//**
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
//**