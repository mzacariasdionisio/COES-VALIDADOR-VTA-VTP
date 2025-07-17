var objHt, objHc;

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

    $('.filtroSimple').each(function () {
        let element = this;
        $(element).multipleSelect({
            name: element.id,
            filter: true,
            single: true,
            placeholder: 'Seleccione',
            onClose: function () {
                const tipo = $('#idTipo').val();
                const valTipo = (tipo) ? tipo : -1;
                const grafico = $('#idGrafico').val();
                const valGrafico = (grafico) ? grafico : -1;
                const barra = $('#idBarra').val();
                const valBarra = (document.getElementById('idTodasBarras').checked) ? -2 : barra;
                if (valTipo != -1 && valGrafico != -1 && (valBarra == -2 || valBarra > 0)) {
                    obtenerMediciones();
                }                
            }
        });
    });

    $('#idTodasBarras').on('click', function () {
        $('#idBarra').multipleSelect('refresh');
        if (document.getElementById('idTodasBarras').checked) {
            $('#idBarra').multipleSelect('disable');
            obtenerMediciones();
        } else {
            $('#idBarra').multipleSelect('enable');
        }
    });

    $('#btnExportar').on('click', function () {
        const myData = objHt.getData();
        const myHeaders = objHt.getColHeader();

        Exportar(myData, myHeaders);
    });

    obtenerMediciones();
});

function Exportar(datos, headers) {
    let val = nombreBarra();
    if (val == true) {
        SetMessage('#message', 'Falta seleccionar el Registro o Barra...', 'error', true);
    } else {
        $.ajax({
            type: 'POST',
            url: controller + 'ExportarDesviacion',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                form: datos,
                header: headers,
                nombre: val
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                if (result != -1) {
                    window.location = controller + 'abrirArchivoDesviacion?formato=' + 3 + '&file=' + result;
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

function obtenerMediciones() {
    const tipo = $('#idTipo').val();
    const grafico = $('#idGrafico').val();
    const barra = $('#idBarra').val();
    const val = (document.getElementById('idTodasBarras').checked) ? -2 : barra;
    $.ajax({
        type: 'POST',
        url: controller + 'ObtenerMediciones',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            tipo: (tipo) ? tipo : -1,
            grafico: (grafico) ? grafico : -1,
            val: (val) ? val : -1
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            console.log(result, '_result');
            SetMessage('#message', result.dataMsg, result.typeMsg, true);
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

function HoraColumnRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.fontWeight = 'bold';
    td.style.background = '#F2F2F2';
}

function actualizarMedicion(medicion) {
    const tipo = $('#idTipo').val();
    const grafico = $('#idGrafico').val();
    const barra = $('#idBarra').val();
    const val = (document.getElementById('idTodasBarras').checked) ? -2 : barra;

    $.ajax({
        type: 'POST',
        url: controller + 'ActualizarMedicion',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            tipo: (tipo) ? tipo : -1,
            grafico: (grafico) ? grafico : -1,
            val: (val) ? val : -1,
            fecha: medicion.fecha
        }),
        datatype: 'json',
        traditional: true,
        success: function (modelo) {
            console.log(modelo, 'modelo');
            let filas = modelo.length;
            console.log(filas, 'filas');
            let indice = (((medicion.id).substring(3, 4)) - 1) * filas + 1;
            console.log(indice, 'indice');
            modelo.forEach((e, index) => {
                const test = e.dArray;
                let med = "med" + (indice + index);
                console.log(med, 'med');
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
            SetMessage('#message', modelo[0].Mensaje, modelo[0].TipoMensaje, true);
        },
        error: function () {
            alert('Ha ocurrido un error...');
        }
    });
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

function nombreBarra() {
    let nombre;
    if (document.getElementById('idTodasBarras').checked) {
        nombre = 'Todas';
    } else {
        nombre = $("#idBarra :selected").text().trim();
    }
    console.log(nombre);
    return nombre;
}