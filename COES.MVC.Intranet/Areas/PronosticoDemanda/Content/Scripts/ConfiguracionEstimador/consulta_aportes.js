var objHt, objHc, objPopup;
var objDtItems, objDtGrupos;

$(document).ready(function () {
    $('.toolHistorico').Zebra_DatePicker({
        onSelect: function () {
            const m = {
                id: this.attr('id'),
                fecha: this.val()
            };
            actualizarMedicion(m);
        }
    });

    $('#btnExportar').on('click', function () {
        const myData = objHt.getData();
        const myHeaders = objHt.getColHeader();

        Exportar(myData, myHeaders);
    });

    $('#idRegistro').multipleSelect({
        filter: true,
        single: true,
        placeholder: 'Seleccione',
        onClose: function () {
            const registro = $('#idRegistro').val();
            obtenerBarrasFiltradas(registro);
        }
    });

    $('#idBarra').multipleSelect({
        filter: true,
        single: false,
        placeholder: 'Seleccione',
        selectAll: true,
        onClose: function () {
            ObtenerMedicionesFactorAporte();
        }
    });

    ObtenerMedicionesFactorAporte();
});

function Exportar(datos, headers) {
    let val = nombreUnidadAsociacion();
    if (val == true) {
        SetMessage('#message', 'Falta seleccionar el Registro o Barra...', 'error', true);
    } else {
        $.ajax({
            type: 'POST',
            url: controller + 'Exportar',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                form: datos,
                header: headers,
                modulo: 3,
                nombre: val
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                if (result != -1) {
                    window.location = controller + 'abrirarchivo?formato=' + 3 + '&file=' + result;
                    SetMessage('#message', 'Felicidades, el archivo se descargo correctamente...!', 'success', true);
                }
                else {
                    SetMessage('#message', 'Lo sentimos, ha ocurrido un error inesperado', 'error', true);
                }
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    }
}

function ObtenerMedicionesFactorAporte() {
    const registro = $('#idRegistro').val();
    const barra = $('#idBarra').val();
    $.ajax({
        type: 'POST',
        url: controller + 'ObtenerMedicionesFactorAporte',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idRegistro: (registro) ? registro : -1,
            idBarra: (barra) ? barra : -1
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {

            //Crea el handsontable
            const ht_model = FormatHandson(result.data);
            iniciarHandson(ht_model);

            //Crea el highchart
            const hc_model = FormatHighchart(result.data);
            iniciarHighchart(hc_model);

            //Llena calendarios
            iniciarCalendarios(result.data);
        },
        error: function () {
            alert('Ha ocurrido un error...');
        }
    });
}

function obtenerBarrasFiltradas(registro) {
    $.ajax({
        type: 'POST',
        url: controller + 'FiltraListaBarras',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            codigo: (registro) ? registro : -1,
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDownList($('#idBarra'), result, 'Barracodi', 'Barranom');
            $('#idBarra').multipleSelect('checkAll');
            ObtenerMedicionesFactorAporte();
        },
        error: function () {
            alert('Ha ocurrido un error...');
        }
    });
}

function actualizarMedicion(medicion) {
    const registro = $('#idRegistro').val();
    const barra = $('#idBarra').val();

    $.ajax({
        type: 'POST',
        url: controller + 'ActualizarMedicion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            idRegistro: (registro) ? registro : -1,
            idBarra: (barra) ? barra : -1,
            fecha: medicion.fecha
        }),
        datatype: 'json',
        traditional: true,
        success: function (modelo) {
            let filas = modelo.length;
            let indice = (((medicion.id).substring(3, 4)) - 1)*filas + 1;

            modelo.forEach((e, index) => {
                const test = e.dArray;
                let med = "med" + (indice + index);
                const nombre = (e.barraNombre).trim() + " - " + medicion.fecha;

                objHc.get(med).
                    setData(test);
                objHc.get(med)
                    .update({ name: nombre });

                const htData = HtGetDataAsProp();
                htData.forEach((e, index) => {
                    e[med] = test[index];
                });

                let htHeader = objHt.getColHeader();
                const colIndex = objHt.propToCol(med);
                htHeader[colIndex] = nombre;
                objHt.updateSettings({
                    data: htData,
                    colHeaders: htHeader
                });
            }); 
        },
        error: function () {
            alert('Ha ocurrido un error...');
        }
    });
}

function iniciarHandson() {
    const contenedor = document.getElementById('ht');
    objHt = new Handsontable(contenedor, {
        data: ObtenerDatos(),
        fillHandle: true,
        stretchH: 'all',
        columns: [
            {
                type: 'text',
                title: 'Hora',
                readOnly: true,
                className: 'htCenter',
                renderer: HoraColumnRenderer
            },
            {
                type: 'numeric',
                title: 'Día 1',
                format: '0.00'
            },
            {
                type: 'numeric',
                title: 'Día 2',
                format: '0.00'
            },
            {
                type: 'numeric',
                title: 'Día 3',
                format: '0.00'
            },
            {
                type: 'numeric',
                title: 'Día 4',
                format: '0.00'
            },
            {
                type: 'numeric',
                title: 'Día 5',
                format: '0.00'
            },
            {
                type: 'numeric',
                title: 'Día 6',
                format: '0.00'
            },
            {
                type: 'numeric',
                title: 'Día 7',
                format: '0.00'
            },
        ]
    });
}

function iniciarHighchart() {
    objHc = Highcharts.chart('hc', {
        chart: {
            type: 'spline',
            backgroundColor: 'transparent',
            plotShadow: false,
            zoomType: 'xy',
        },
        title: {
            text: ''
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
        tooltip: {
            enabled: true
        },
        xAxis: {
            tickInterval: 1,
            categories: objHt.getDataAtCol(0),
            labels: {
                rotation: 90
            }
        },
        yAxis: {
            title: ''
        },
        tooltip: {
            crosshairs: true,
            shared: true
        },
        series: [
            {
                id: 'd1',
                name: 'Día 1',
                data: objHt.getDataAtCol(1)
            },
            {
                id: 'd2',
                name: 'Día 2',
                data: objHt.getDataAtCol(2)
            },
            {
                id: 'd3',
                name: 'Día 3',
                data: objHt.getDataAtCol(3)
            },
            {
                id: 'd4',
                name: 'Día 4',
                data: objHt.getDataAtCol(4)
            },
            {
                id: 'd5',
                name: 'Día 5',
                data: objHt.getDataAtCol(5)
            },
            {
                id: 'd6',
                name: 'Día 6',
                data: objHt.getDataAtCol(6)
            },
            {
                id: 'd7',
                name: 'Día 7',
                data: objHt.getDataAtCol(7)
            }
        ]
    });
}

function ObtenerDatos() {
    let datos = [];
    for (var i = 0; i < 48; i++) {
        datos.push(new Array(8).fill(0));
    }

    let baseTime = new Date();
    baseTime.setHours(0, 0, 0, 0);
    datos.forEach((row, index) => {
        const minutes = 30 * (index + 1);
        let t = new Date(baseTime);
        t.setMinutes(baseTime.getMinutes() + minutes);
        const hh = (t.getHours().toString().length == 1)
            ? `0${t.getHours()}` : t.getHours().toString();
        const mm = (t.getMinutes().toString().length == 1)
            ? `0${t.getMinutes()}` : t.getMinutes().toString();

        row[0] = `${hh}:${mm}`;
    });
    return datos;
}

function HoraColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.background = '#F2F2F2';
}

function RefillDropDownList(element, data, data_id, data_name) {
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

function HtGetDataAsProp() {
    var res = [];
    var s_max = objHt.countRows();
    var s_schema = objHt.getSchema();
    for (var i = 0; i < s_max; i++) {
        var s_row = {};
        $.each(s_schema, function (prop, value) {
            s_row[prop] = objHt.getDataAtRowProp(i, prop);
        });
        res.push(s_row);
    }
    return res;
}

function iniciarHandson(model) {
    const contenedor = document.getElementById('ht');
    contenedor.innerHTML = '';
    objHt = new Handsontable(contenedor, {
        data: model.data,
        fillHandle: true,
        stretchH: 'all',
        maxCols: model.maxCols,
        maxRows: model.maxRows,
        minSpareCols: 0,
        minSpareRows: 0,
        columns: model.columns,
        colHeaders: model.colHeaders
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

function iniciarHighchart(model) {
    objHc = Highcharts.chart('hc', {
        chart: {
            type: 'spline',
            backgroundColor: 'transparent',
            plotShadow: false,
            zoomType: 'xy',
        },
        title: {
            text: ''
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
                marker: { enabled: true, radius: 3 }
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

function iniciarCalendarios(model) {
    let fechas = [];
    model.forEach(e => {
        if (e.id == 'intervalos') return;
        fechas = e.labelFecha
        return;
    });
    let i = 0;
    let med = '';
    fechas.forEach(e => {
        med = "med" + (i + 1);
        const dateValue = (e == 'No encontrada')
            ? '' : e;
        $(`#${med}`).val(dateValue);
        i += 1;
    });
}

function nombreUnidadAsociacion() {
    let nRegistro;
    $("#idRegistro :selected").each(function (i, sel) {
        nRegistro = $(sel).text().trim();
    });

    let val = (nRegistro) ? nRegistro : true;
   
    return val;
}