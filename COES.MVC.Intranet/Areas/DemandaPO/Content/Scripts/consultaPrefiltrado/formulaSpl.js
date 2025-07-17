
$(document).ready(function () {

    $('#opcionNuevo').click(function () {
        document.getElementById('s-version').style.display = 'none';
        document.getElementById('s-nombre').style.display = 'block';
    });

    $('#opcionExistente').click(function () {
        document.getElementById('s-version').style.display = 'block';
        document.getElementById('s-nombre').style.display = 'none';
    });

    $('#selMetodo').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione...',
    });

    $('#selVariable').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione...',
    });
    $('#selVersion').multipleSelect({
        single: true,
        filter: true,
    });
    $('#selPopVersion').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione...',
    });
    $('#selAreaDemanda').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione...',
    });
    $('#selFuente').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione...',
    });
    $('#selFormula').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione...',
    });
    $('#selCarga').multipleSelect({
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

    $('#selFormulasDpo').multipleSelect({
        single: true,
        filter: true,
        placeholder: 'Seleccione...',
    });

    document.getElementById('ckbFormula').addEventListener('change', function () {

        var chartObjects = {
            object_Enero: window.object_Enero,
            object_Febrero: window.object_Febrero,
            object_Marzo: window.object_Marzo,
            object_Abril: window.object_Abril,
            object_Mayo: window.object_Mayo,
            object_Junio: window.object_Junio,
            object_Julio: window.object_Julio,
            object_Agosto: window.object_Agosto,
            object_Septiembre: window.object_Setiembre,
            object_Octubre: window.object_Octubre,
            object_Noviembre: window.object_Noviembre,
            object_Diciembre: window.object_Diciembre
        };
        for (var prop in chartObjects) {
            if (chartObjects.hasOwnProperty(prop) && chartObjects[prop] === undefined) {
                delete chartObjects[prop];
            }
        }
        if (document.getElementById('ckbFormula').checked) {
            console.log(chartObjects, 'co');
            $.each(chartObjects, function (i, item) {
                item.series[1].show();
            });
        } else {
            console.log(chartObjects, 'co');
            $.each(chartObjects, function (i, item) {
                item.series[1].hide();
            });
        }
    });

    $('#btnGrabar').click(function () {
        const anio = ($('#selAnio').val()) ? $('#selAnio').val() : -1;
        const mes = ($('#selMes').val()) ? $('#selMes').val() : -1;
        const verOrigen = ($('#selVersion').val()) ? $('#selVersion').val() : -1;
        const fuente = ($('#selFuente').val()) ? $('#selFuente').val() : -1;
        const variable = ($('#selVariable').val()) ? $('#selVariable').val() : -1;
        const formula = ($('#selFormula').val()) ? $('#selFormula').val() : -1;
        const carga = ($('#selCarga').val()) ? $('#selCarga').val() : -1;
        if (anio == -1 || mes == -1 || fuente == -1 || variable == -1 || formula == -1 || carga == -1) {
            SetMessage('#message', 'Seleccione los filtros necesarios...', 'warning', true);
        } else {
            const metodo = $('#txtMetodo').val();
            if (metodo == 0) {
                SetMessage('#message', 'Se debe corregir los datos antes de grabarlos...', 'warning', true);
            } else {
                objPopupGrabar = $('#popup-grabar').bPopup({
                    easing: 'easeOutBack',
                    speed: 350,
                    transition: 'fadeIn',
                    modalClose: false,
                    onOpen: function () {
                        SetMessage('#pop-mensaje-grabar',
                            '• El proceso de grabar se realizará con los datos seleccionados corregidos <br>' +
                            '  actualmente en los filtros de la interfaz principal.<br>' +
                            '• En caso de no seleccionar una versión se utilizará los datos de orígen <br>' +
                            '• El método que se utilizará es: ' + $('#txtMetodoNombre').val() + ' (último seleccionado)',
                            'info');
                    },
                    onClose: function () {
                        //$('#txtPopNombre').val('');
                        //htItems.updateSettings({
                        //    data: [],
                        //    columns: ColumnsHandsonBarras()
                        //});
                        //document.getElementById('selMetodo').selectedIndex = 0;
                    }
                }, function () {
                    //listarVersiones();
                });
            }
        }
    });

    $('#btnCorregir').click(function () {
        const anio = ($('#selAnio').val()) ? $('#selAnio').val() : -1;
        const mes = ($('#selMes').val()) ? $('#selMes').val() : -1;
        const version = ($('#selVersion').val()) ? $('#selVersion').val() : -1;
        const fuente = ($('#selFuente').val()) ? $('#selFuente').val() : -1;
        const variable = ($('#selVariable').val()) ? $('#selVariable').val() : -1;
        const formula = ($('#selFormula').val()) ? $('#selFormula').val() : -1;
        const carga = ($('#selCarga').val()) ? $('#selCarga').val() : -1;
        if (anio == -1 || mes == -1 || fuente == -1 || variable == -1 || formula == -1 || carga == -1) {
            SetMessage('#message', 'Seleccione los filtros necesarios...', 'warning', true);
        } else {
            objPopupCorregir = $('#popup-corregir').bPopup({
                easing: 'easeOutBack',
                speed: 350,
                transition: 'fadeIn',
                modalClose: false,
                onOpen: function () {
                    SetMessage('#pop-mensaje-corregir',
                                '• El proceso de corrección se realizará con los datos seleccionados <br>' +
                                '  actualmente en los filtros de la interfaz principal.<br>' +
                                '• En caso de no seleccionar una versión se utilizará los datos de orígen.',
                                'info');
                },
                onClose: function () {
                    document.getElementById('selMetodo').selectedIndex = 0;
                }
            }, function () {
                //generarRelacion();
            });
        }
    });

    $('#selFuente').change(function () {
        const relacion = $('#selFormula').val();
        recargaFiltroAreaDemanda(relacion);
    });

    $('#selFormula').change(function () {
        const relacion = $('#selFormula').val();
        recargaFiltroAreaDemanda(relacion);
    });

    $('#selFormulasDpo').change(function () {
        const formulaDpo = $('#selFormulasDpo').val();
        const fuente = $('#selFuente').val();
        recargaFiltroCargas(formulaDpo, fuente);
    });

    $('#selAreaDemanda').change(function () {
        //const fuente = $('#selFuente').val();
        const relacion = $('#selFormula').val();
        const area = $('#selAreaDemanda').val();
        //if (fuente && relacion && area) {
        //    //recargaFiltroCarga(fuente, version);
        //}
        if (relacion && area)
        {
            recargaFiltroFormulas(relacion, area);
        }
    });

    $('#btnConsultar').click(function () {
        consultarData();
    });

    $('#btnExportar').click(function () {
        exportarData();
    });

    $('#btnPopGrabar').click(function () {
        const selChk = document.querySelector('input[name="opcion"]:checked').value;
        let version = null;

        if (selChk == 1) {
            //version = $('#txtPopNombreVersion').val();
            version = $('#selPopVersion').val();
        } else {
            //version = $('#selPopVersion').val();
            version = $('#txtPopNombreVersion').val();
        }

        if (!version) {
            SetMessage('#message', 'Se debe ingresar o seleccionar una versión...', 'warning', true);
            $("#popup-grabar").bPopup().close();
        } else {
            grabarData(version, selChk);
        }
    });

    $('#btnPopCorregir').click(function () {
        const metodo = $('#selMetodo').val();
        const nombre = $('#selMetodo').find('option[value="' + metodo + '"]').text()
        if (metodo == 0) {
            SetMessage('#message', 'Se debe seleccionar el método de correción...', 'warning', true);
        } else {
            corregirData(metodo, nombre);
        }   
    });

    document.getElementById('s-version').style.display = 'block';
    document.getElementById('s-nombre').style.display = 'none';
    document.getElementById('txtMetodo').value = 0;
    $('#selVersion').multipleSelect('setSelects', ["-1"]);
});

function consultarData() {
    const anio = ($('#selAnio').val()) ? $('#selAnio').val() : -1;
    const mes = ($('#selMes').val()) ? $('#selMes').val() : -1;
    const version = ($('#selVersion').val()) ? $('#selVersion').val() : -1;
    const fuente = ($('#selFuente').val()) ? $('#selFuente').val() : -1;
    const variable = ($('#selVariable').val()) ? $('#selVariable').val() : -1;
    const relacion = ($('#selFormula').val()) ? $('#selFormula').val() : -1;
    const carga = ($('#selCarga').val()) ? $('#selCarga').val() : -1;
    const formula = ($('#selFormulasDpo').val()) ? $('#selFormulasDpo').val() : -1;

    if (anio == -1 || mes == -1 || fuente == -1 || variable == -1 || relacion == -1 || carga == -1) {
        SetMessage('#message', 'Seleccione los filtros necesarios...', 'warning', true);
    } else {
        $.ajax({
            type: 'POST',
            url: controller + 'ConsultarData',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                anio: anio,
                mes: mes,
                version: version,
                fuente: fuente,
                variable: variable,
                relacion: relacion,
                carga: carga,
                formula: formula
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                document.getElementById('txtMetodo').value = 0;
                construirGraficas(result);
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    }
}
function construirGraficas(result) {
    $('#div-graficos').html('');
    ///
    const _wraperMaxMin = $('#div-graficos');
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
                type: 'line',
                step: 'left'
            },
            {
                id: -2,
                name: 'Minimo',
                data: result.ListaMinimo.data,
                marker: { symbol: 'circle' },
                type: 'line',
                step: 'left'
            }
        ],
    };
    crearHighchartMaximoMinimo(modelHighchartMaxMin, result.ListaPosiciones);
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
        const _wraper = $('#div-graficos');
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
                    id: result.ListaData[e.pos].id,//e.id,
                    name: e.nombre,
                    data: result.ListaData[e.pos].data,
                    marker: { symbol: 'circle' },
                },
                {
                    id: result.ListaAlgoritmo[e.pos].id,//e.id,
                    name: 'Formula',
                    data: result.ListaAlgoritmo[e.pos].data,
                    marker: { symbol: 'circle' },
                }
            ],
        };
        crearHighchartMeses(modelHighchart);
    });
}

function corregirData(metodo, nombre) {
    const anio = ($('#selAnio').val()) ? $('#selAnio').val() : -1;
    const mes = ($('#selMes').val()) ? $('#selMes').val() : -1;
    const version = ($('#selVersion').val()) ? $('#selVersion').val() : -1;
    const fuente = ($('#selFuente').val()) ? $('#selFuente').val() : -1;
    const variable = ($('#selVariable').val()) ? $('#selVariable').val() : -1;
    const relacion = ($('#selFormula').val()) ? $('#selFormula').val() : -1;
    const carga = ($('#selCarga').val()) ? $('#selCarga').val() : -1;
    const area = ($('#selAreaDemanda').val()) ? $('#selAreaDemanda').val() : -1;
    const formulaDpo = ($('#selFormulasDpo').val()) ? $('#selFormulasDpo').val() : -1;

    if (fuente == 1 || fuente == 2 || fuente == 4) {
        $.ajax({
            type: 'POST',
            url: controller + 'CorregirData',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                metodo: metodo,
                anio: anio,
                mes: mes,
                fuente: fuente,
                variable: variable,
                relacion: relacion,
                carga: carga,
                version: version,
                area: area,
                formulaDpo: formulaDpo
            }),
            datatype: 'json',
            traditional: true,
            success: function (result) {
                $("#popup-corregir").bPopup().close();
                $('#selMetodo').multipleSelect('setSelects', ["0"]);
                document.getElementById('txtMetodo').value = metodo;
                document.getElementById('txtMetodoNombre').value = nombre;
                SetMessage('#message', result.Mensaje, result.TipoMensaje, true);
                construirGraficas(result);
            },
            error: function () {
                alert("Ha ocurrido un problema...");
            }
        });
    } else {
        SetMessage('#message', 'Solo se puede corregir las fuentes SIRPIT y SICLI...', 'warning', true);
    }
}

function grabarData(verDestino, opcion) {
    const anio = ($('#selAnio').val()) ? $('#selAnio').val() : -1;
    const mes = ($('#selMes').val()) ? $('#selMes').val() : -1;
    const verOrigen = ($('#selVersion').val()) ? $('#selVersion').val() : -1;
    const fuente = ($('#selFuente').val()) ? $('#selFuente').val() : -1;
    const variable = ($('#selVariable').val()) ? $('#selVariable').val() : -1;
    const relacion = ($('#selFormula').val()) ? $('#selFormula').val() : -1;
    const carga = ($('#selCarga').val()) ? $('#selCarga').val() : -1;
    const metodo = $('#txtMetodo').val();
    const area = ($('#selAreaDemanda').val()) ? $('#selAreaDemanda').val() : -1;
    const formulaDpo = ($('#selFormulasDpo').val()) ? $('#selFormulasDpo').val() : -1;

    $.ajax({
        type: 'POST',
        url: controller + 'GrabarData',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            anio: anio,
            mes: mes,
            fuente: fuente,
            variable: variable,
            relacion: relacion,
            carga: carga,
            metodo: metodo,
            versionOrigen: verOrigen,
            versionDestino: verDestino,
            opcion: opcion,
            area: area,
            formulaDpo: formulaDpo
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            $("#popup-grabar").bPopup().close();
            RefillDropDowListConcat($('#selVersion'), result.ListaVersiones, 'Vergrpcodi', 'Vergrpnomb');
            $('#selVersion').multipleSelect('setSelects', [verOrigen]);
            RefillDropDowListConcat($('#selPopVersion'), result.ListaVersiones, 'Vergrpcodi', 'Vergrpnomb');
            document.getElementById('txtPopNombreVersion').value = "";
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function recargaFiltroCargas(formulaDpo, fuente) {
    $.ajax({
        type: 'POST',
        url: controller + 'recargaCarga',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            formulaDpo: formulaDpo,
            fuente: fuente
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDowList($('#selCarga'), result.ListaCargas, 'filtroCodigo', 'filtroNombre');
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function recargaFiltroAreaDemanda(relacion) {
    $.ajax({
        type: 'POST',
        url: controller + 'recargaAreaDemanda',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            relacion: relacion
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDowList($('#selAreaDemanda'), result.ListaAreaDemanda, 'Splfrmarea', 'Splareanombre');
            RefillDropDowList($('#selFormulasDpo'), result.ListaFormulasDpo, 'Prrucodi', 'Prruabrev');
            RefillDropDowList($('#selCarga'), result.ListaCargas, 'filtroCodigo', 'filtroNombre');
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}

function recargaFiltroFormulas(relacion, area) {
    $.ajax({
        type: 'POST',
        url: controller + 'recargaFormulas',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            relacion: relacion,
            area: area
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            RefillDropDowList($('#selFormulasDpo'), result.ListaFormulasDpo, 'Prrucodi', 'Prruabrev');
            RefillDropDowList($('#selCarga'), result.ListaCargas, 'filtroCodigo', 'filtroNombre');
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
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

function RefillDropDowListConcat(element, data, data_id, data_name) {
    //Vacia el elemento
    element.empty();
    //Carga el elemento
    $.each(data, function (i, item) {
        var n_value = i, n_html = item;
        if (data_id) n_value = item[data_id];
        if (data_name) n_html = item[data_name];
        element.append($('<option></option>').val(n_value).html(n_html + " [" + n_value +"]"));
    });
    //Actualiza la lib.multipleselect
    element.multipleSelect('refresh');
}

function agregarGrafica() {
    $('#div-graficos').html('');
    const _idMeses = $('#selMes').multipleSelect('getSelects');
    const _textMeses = $('#selMes').multipleSelect('getSelects', 'text');
    const meses = _idMeses
        .map((id, index) => {
            return {
                id: id,
                nombre: _textMeses[index].trim(),
            };
        });

    meses.forEach(e => {
        const _wraper = $('#div-graficos');
        const _container = `<div id="panel-${e.nombre}" class="highchart-panel"></div>`;
        const _title = `<div class="highchart-panel-title">${e.nombre}</div>`;
        const _highchart = `<div id="highchart-${e.nombre}" class="highchart-panel-body">grafico</div>`;

        _wraper.append(_container);
        _wraper.find(`#panel-${e.nombre}`).append(_title);
        _wraper.find(`#panel-${e.nombre}`).append(_highchart);

        const modelHighchart = {          
            objectName: `object_${e.nombre}`,
            elementName: `highchart-${e.nombre}`,
            categories: [...Array(30).keys()], //Días del mes
            series: [
                {
                    id: e.id,
                    name: e.nombre,
                    data: Array(30).fill(5),
                    marker: { symbol: 'circle' },
                }
            ],            
        };
        crearHighchart(modelHighchart);
    });
}

function crearHighchartMaximoMinimo(model, posiciones) {
    window[model.objectName] = Highcharts.chart(model.elementName, {
        chart: {
            height: '400px',
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
            enabled: false
            //itemDistance: 15,
            //verticalAlign: 'top',
            //symbolWidth: 10,
            //itemStyle: {
            //    fontWeight: 'normal'
            //},
            //y: 0
        },
        tooltip: { enabled: true },
        plotOptions: {
            series: {
                stickyTracking: false,
                marker: { enabled: false, radius: 3 }
            }
        },
        xAxis: {
            //tickInterval: (tipo == 1) ? 1 : 96,
            //tickInterval: 1,
            categories: model.categories,//.map(str => str.replace(/\d+/g, '').trim()),
            labels: {
                formatter: function () {
                    var label = this.axis.defaultLabelFormatter.call(this);
                    return label.replace(/\d+/g, '').trim();
                },
                rotation: 0
            },
            tickPositions: posiciones
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

function crearHighchartMeses(model) {
    window[model.objectName] = Highcharts.chart(model.elementName, {
        chart: {
            height: '400px',
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
            enabled: true,
            itemDistance: 5,
            verticalAlign: 'top',
            symbolWidth: 5,
            itemStyle: {
                fontWeight: 'normal'
            },
            y: 0
        },
        tooltip: { enabled: true },
        plotOptions: {
            series: {
                stickyTracking: false,
                marker: { enabled: false, radius: 3 },
            }
        },
        xAxis: {
            //tickInterval: (tipo == 1) ? 1 : 96,
            tickInterval: 96,
            categories: model.categories,
            labels: {
                formatter: function () {
                    var label = this.axis.defaultLabelFormatter.call(this);
                    return label.split('/')[0];
                },
                rotation: 90
            }
            //tickPositions: [14, 45]
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

function exportarData() {
    var chartObjects = {
        object_Enero: window.object_Enero,
        object_Febrero: window.object_Febrero,
        object_Marzo: window.object_Marzo,
        object_Abril: window.object_Abril,
        object_Mayo: window.object_Mayo,
        object_Junio: window.object_Junio,
        object_Julio: window.object_Julio,
        object_Agosto: window.object_Agosto,
        object_Septiembre: window.object_Setiembre,
        object_Octubre: window.object_Octubre,
        object_Noviembre: window.object_Noviembre,
        object_Diciembre: window.object_Diciembre
    };

    var data = [];

    for (var chartName in chartObjects) {

        var chartObject = chartObjects[chartName];
        if (chartObject) {
            var seriesData = chartObject.series;
            for (var i = 0; i < seriesData.length; i++) {
                var series = seriesData[i];
                var seriesNombre = series.name;
                var seriesValores = series.data;

                var obj = {
                    Serie: seriesNombre,
                    Valores: [],
                    Fechas: []
                };

                for (var j = 0; j < seriesValores.length; j++) {
                    var datos = seriesValores[j];
                    var datosValores = datos.y;
                    var datosFechas = datos.category;
                    obj.Valores.push(datosValores);
                    obj.Fechas.push(datosFechas);
                }
                data.push(obj);
            }
        }
    }

    const version = $('#selVersion option:selected').text();//($('#selVersion').val()) ? $('#selVersion').val() : -1;
    const variable = $('#selVariable option:selected').text();//($('#selVariable').val()) ? $('#selVariable').val() : -1;
    const formula = $('#selFormulasDpo option:selected').text();//($('#selFormulasDpo').val()) ? $('#selFormulasDpo').val() : -1;

    $.ajax({
        type: 'POST',
        url: controller + 'exportarData',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            data: data,
            version: version,
            variable: variable,
            formula: formula
        }),
        datatype: 'json',
        traditional: true,
        success: function (result) {
            if (result != -1) {
                window.location = controller + 'abrirarchivo?formato=' + 1 + '&file=' + result;
                //mostrarMensaje('mensaje', 'exito', "Felicidades, el archivo se descargo correctamente...!");
                SetMessage('#message', 'El archivo se descargo correctamente...', 'success', false);
            }
            else {
                //mostrarMensaje('mensaje', 'error', "Lo sentimos, ha ocurrido un error inesperado");
                SetMessage('#message', 'Lo sentimos, ha ocurrido un error inesperado...', 'warning', false);
            }
        },
        error: function () {
            alert("Ha ocurrido un problema...");
        }
    });
}