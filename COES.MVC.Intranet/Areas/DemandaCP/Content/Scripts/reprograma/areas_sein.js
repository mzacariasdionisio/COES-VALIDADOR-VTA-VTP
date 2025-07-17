var ht, hc;
$(document).ready(function () {
    $('#selAreaOperativa').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione...',
        onClose: function () {
            obtenerDatos();
        }
    });
    //$('#btnConsultar').click(function () {
    //    obtenerDatos();
    //});
    $('#btnGuardar').click(function () {
        const res = ht.getDataAtProp('ajuste');
        registrarDatos(res);
    });

    obtenerDatos();
});

function obtenerDatos() {
    const [_id] = $('#repSelVersion')
        .multipleSelect('getSelects');
    const [_name] = $('#repSelVersion')
        .multipleSelect('getSelects', 'text');
    const _fecha = $('#repSelFecha').val();
    const [_area] = $('#selAreaOperativa')
        .multipleSelect('getSelects');
    const [_areaNomb] = $('#selAreaOperativa')
        .multipleSelect('getSelects', 'text');
    const [_idVersionComp] = $('#repSelVersionComp')
        .multipleSelect('getSelects');
    const _fechaComp = $('#repSelFechaComp').val();

    $.ajax({
        type: 'POST',
        url: `${controlador}ObtenerDatosAreaSein`,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            selArea: _area,
            selVersion: (_id) ? _id : -1,
            nomVersion: _name,
            selFecha: _fecha,
            selVersionComp: (_idVersionComp) ? _idVersionComp : -1,
            selFechaComp: _fechaComp,
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            //Crea el handsontable
            var ht_model = FormatHandson(result.data);
            GetHanson(ht_model);

            //Crea el highchart
            var hc_title = `Área SEIN: ${_areaNomb}`;
            var hc_model = FormatHighchart(result.data);
            GetHighchart(hc_model, hc_title);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function registrarDatos(_datos) {
    const [_id] = $('#repSelVersion')
        .multipleSelect('getSelects');
    const [_name] = $('#repSelVersion')
        .multipleSelect('getSelects', 'text');
    const _fecha = $('#repSelFecha').val();
    const [_area] = $('#selAreaOperativa')
        .multipleSelect('getSelects');

    $.ajax({
        type: 'POST',
        url: `${controlador}RegistrarDatosAreaSein`,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            selArea: _area,
            selVersion: (_id) ? _id : -1,
            nomVersion: _name,
            selFecha: _fecha,
            datos: _datos,
        }),
        datatype: 'json',
        traditional: true,
        success: function (res) {
            alert(res.dataMsg);
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

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
                col['renderer'] = IntervaloColumnRenderer;
                break;
            case 'text':
                col['type'] = 'text';
                col['className'] = 'htCenter';
                col['readOnly'] = true;
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

function GetHanson(model) {
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

                    htSuma = parseFloat(changes[i][3]);//Agrega el ajuste realizado

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
    });
}

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
            case 'defecto':
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

function ValidarEventosHandsontable(evento) {
    var valid = false;
    var ListEventos = ['edit', 'autofill', 'paste', 'grafico'];

    for (var i = 0; i < ListEventos.length; i++) {
        if (evento == ListEventos[i]) valid = true;
    }
    return valid;
}

function IntervaloColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.background = '#F2F2F2';
}

function PatronColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.NumericRenderer.apply(this, arguments);
    td.style.background = '#edf5fc';
}