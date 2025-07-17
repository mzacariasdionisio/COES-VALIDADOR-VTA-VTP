$(document).ready(function () {
    $('#selVariable').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione...',
    });
    $('#selVersion').multipleSelect({
        single: true,
        filter: true,
    });
    $('#selPunto').multipleSelect({
        single: false,
        filter: true,
        placeholder: 'Seleccione...',
    });

    $('#selMes').multipleSelect({
        single: false,
        filter: true,
        placeholder: 'Seleccione...',
    });
    $('#selAnio').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione...',
    });
    $('#btnConsultar').click(function () {
        consultarData();
    });

    $('#selVersion').multipleSelect('setSelects', ["-1"]);
});


function consultarData() {
    const anio = ($('#selAnio').val()) ? $('#selAnio').val() : -1;
    const mes = ($('#selMes').val()) ? $('#selMes').val() : -1;
    const version = ($('#selVersion').val()) ? $('#selVersion').val() : -1;
    const carga = ($('#selPunto').val()) ? $('#selPunto').val() : -1;
    const variable = ($('#selVariable').val()) ? $('#selVariable').val() : -1;

    $.ajax({
        type: 'POST',
        url: controller + 'ConsultarData',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            anio: anio,
            mes: mes,
            version: version,
            fuente: 2,
            variable: variable,
            relacion: 'N',
            carga: carga,
            formula: 0           
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            console.log(result, 'result');
            $('#div-graficosicli').html('');
            ///
            const _wraperMaxMin = $('#div-graficosicli');
            const _containerMaxMin = `<div id="panel-Maximo" class="highchart-panel"></div>`;
            const _titleMaxMin = `<div class="highchart-panel-title">Máximos y Mínimos</div>`;
            const _highchartMaxMin = `<div id="highchart-Maximo" class="highchart-panel-body">grafico</div>`;
            _wraperMaxMin.append(_containerMaxMin);
            _wraperMaxMin.find(`#panel-Maximo`).append(_titleMaxMin);
            _wraperMaxMin.find(`#panel-Maximo`).append(_highchartMaxMin);

            const modelHighchartMaxMin = {
                objectName: `object_Maximo`,
                elementName: `highchart-Maximo`,
                categories: result.ListaMaximo.diasHora,//['Enero', 'Febrero'],//[...Array(result[e.pos].dias).keys()], //Días del mes
                series: [
                    {
                        id: -1,
                        name: 'Maximo',
                        data: result.ListaMaximo.data,
                        marker: { symbol: 'circle' },
                    },
                    {
                        id: -2,
                        name: 'Minimo',
                        data: result.ListaMinimo.data,
                        marker: { symbol: 'circle' },
                    }
                ],
            };
            crearHighchart(modelHighchartMaxMin, 1);
            ///
            const _idMeses = $('#selMes').multipleSelect('getSelects');
            const _textMeses = $('#selMes').multipleSelect('getSelects', 'text');
            const meses = _idMeses
                .map((id, index) => {
                    return {
                        id: id,
                        pos: index,
                        nombre: _textMeses[index].trim(),
                    };
                });

            meses.forEach(e => {
                const _wraper = $('#div-graficosicli');
                const _container = `<div id="panel-${e.nombre}" class="highchart-panel"></div>`;
                const _title = `<div class="highchart-panel-title">${e.nombre}</div>`;
                const _highchart = `<div id="highchart-${e.nombre}" class="highchart-panel-body">grafico</div>`;
                _wraper.append(_container);
                _wraper.find(`#panel-${e.nombre}`).append(_title);
                _wraper.find(`#panel-${e.nombre}`).append(_highchart);

                const modelHighchart = {
                    objectName: `object_${e.nombre}`,
                    elementName: `highchart-${e.nombre}`,
                    categories: result.ListaData[e.pos].diasHora,//[...Array(result[e.pos].dias).keys()], //Días del mes
                    series: [
                        {
                            id: e.id,
                            name: e.nombre,
                            data: result.ListaData[e.pos].data,
                            marker: { symbol: 'circle' },
                        }
                    ],
                };
                crearHighchart(modelHighchart, 2);
            });
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function crearHighchart(model, tipo) {
    window[model.objectName] = Highcharts.chart(model.elementName, {
        chart: {
            height: '300px',
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
                marker: { enabled: false, radius: 3 }
            }
        },
        xAxis: {
            tickInterval: (tipo == 1) ? 1 : 96,
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