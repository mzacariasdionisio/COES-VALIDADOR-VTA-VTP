var ht, hc;
var arrayBloques = [];

$(document).ready(function () {
    $('#id-fecha').Zebra_DatePicker({
        onSelect: () => { obtenerDatos(); }
    });
    $('#id-area').multipleSelect({
        single: true,
        placeholder: 'Seleccione',
        onClose: () => { obtenerDatos(); }
    });
    $('.toolHistorico').Zebra_DatePicker({
        onSelect: function () {
            const m = {
                id: this.attr('id'),
                fecha: this.val()
            };
            obtenerMedicionHistorica(m);
        }
    });
    $('.cls-bloque').on('change', function () {
        var id = this.id;
        var valor = parseFloat(this.value);
        if (!(isNaN(valor))) {
            EditarPorBloque(id, valor);
        }
    });
    $('#btn-pronostico').on('click', function () {
        pop = $('#popup').bPopup({
            easing: 'easeOutBack',
            speed: 350,
            transition: 'fadeIn',
            modalClose: false,
            onOpen: function () {
                SetMessage('#pop-mensaje',
                    '• Se calculará el pronóstico para la "Fecha" seleccionada de la vista principal.<br>' +
                    '• Se utilizarán los días selccionados como "Historicos" de la vista principal.<br>' +
                    '• Es necesario seleccionar 5 días "Historicos" para ejecutar el pronóstico.<br>' +
                    '• El proceso puede tardar algunos minutos.',
                    'warning');
            }
        });
    });
    $('#btn-pop-ejecutar').on('click', function () {
        pop.close();
        ejecutarPronostico();
    });

    $('#btn-guardar').on('click', function () {        
        const data = FormatMedicion(ht.getDataAtProp('ajuste'));
        registrarDatos(data);
    });

    obtenerDatos();
});

function ejecutarPronostico() {
    const classHistorico = $('.toolHistorico')
        .toArray()
        .filter(e => (e.value == "") ? false : true);
    const selHistoricos = classHistorico
        .map(e => {
            return `${e.id}:${e.value}`;
        });
    $.ajax({
        type: 'POST',
        url: controller + 'PronosticoPorGeneracionEjecutar',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            regFecha: $('#id-fecha').val(),
            selHistoricos: selHistoricos
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

function registrarDatos(data) {
    $.ajax({
        type: 'POST',
        url: controller + 'PronosticoPorGeneracionSave',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idArea: $('#id-area').val(),
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


function obtenerDatos() {
    const id = $('#id-area').val();
    const classHistorico = $('.toolHistorico')
        .toArray()
        .filter(e => (e.value == "") ? false : true);
    const selHistoricos = classHistorico
        .map(e => {            
            return `${e.id}:${e.value}`;
        });
    $.ajax({
        type: 'POST',
        url: controller + 'PronosticoPorGeneracionDatos',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idArea: (id) ? id : -1,
            regFecha: $('#id-fecha').val(),
            selHistoricos: selHistoricos
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {                        
            //Crea el handsontable
            var ht_model = FormatoHandson(result.data);
            obtenerHandson(ht_model);

            //Crea el highchart
            var hc_title = "";
            var hc_model = FormatoHighchart(result.data);
            obtenerHighchart(hc_model, hc_title);

            //Crea los calendarios
            obtenerCalendarios(result.data);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function obtenerMedicionHistorica(medicion) {
    const id = $('#id-area').val();
    $.ajax({
        type: 'POST',
        url: controller + 'PronosticoPorGeneracionMedicion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idArea: (id) ? id : -1,
            regFecha: medicion.fecha
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            const objHc = hc.get(medicion.id);
            if (objHc) {
                hc.get(medicion.id).
                    setData(result);
                hc.get(medicion.id)
                    .update({ name: medicion.fecha });
            }
            else {
                hc.addSeries({
                    id: medicion.id,
                    name: medicion.fecha,
                    data: result,
                    marker: { enabled: true, symbol: 'circle' }
                });
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function obtenerHandson(model) {
    $('#ht').html('');
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
                htBaseIndex = ht.propToCol('base');//Setea la columna base (Handsontable)
                htDestinoIndex = ht.propToCol('final');//Setea la columna final (Handsontable)

                hcSerieIndex = hc.get('final').index;//Setea el indice de la linea que se modificara (Highchart)

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

                    //Realiza la modificación
                    ht.setDataAtCell(htRowIndex, htDestinoIndex, htSuma, 'sum');

                    //El evento "grafico" modifica la grafica previamente
                    if (source != 'grafico') {
                        hc.series[hcSerieIndex].data[htRowIndex].update(htSuma);
                    }
                }
            }
        },        
    });
}

function obtenerHighchart(model, title) {
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
                        drop: function () {                            
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

function obtenerCalendarios(model) {
    model.forEach(e => {
        const noPermitidos = ['intervalos', 'base', 'ajuste', 'final'];
        if (noPermitidos.includes(e)) return false;        
        $(`#${e.id}`).val(e.label);
    });
}

//Formatea el modelo de datos para el Handsontable
function FormatoHandson(model) {
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

//Formatea el modelo de datos para el Highchart
function FormatoHighchart(model) {
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
