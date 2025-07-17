var controlador = siteRoot + 'Eventos/AnalisisFallas/';

$(function () {
    $('#tab-container').easytabs(
        { animate: false }
    );

    $('#txtAnio').Zebra_DatePicker({
        format: 'Y',
        onSelect: function (date) {
            mostrarListado();
        }
    });

    $("#btnExportar").click(function () {
        exportar();
    });
    
    mostrarListado();
});

mostrarListado = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "ListaIndicadores",
        data: { anio: $('#txtAnio').val() },
        success: function (resultado) {
            $('#listadoIndicadores').html(resultado);
            //grafica
            cargarIndicadoresCtaf();
        },
        error: function () {
            //mostrarError();
        }
    });
}

//carga grafico Indicadores Ctaf
cargarIndicadoresCtaf = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "ListaGraficoIndicadores",
        data: { anio: $('#txtAnio').val() },
        success: function (result) {
            graficoIndicadoresCtaf(result);
            cargarIndicadoresIT();
        },
        error: function () {

        }
    });
}

graficoIndicadoresCtaf = function (result) {
    debugger;
    var json = result;
    var _anio;
    var jsonmes = []; //empresa
    var jsoncriticidadcolor = []; //color criticidad
    jsoncriticidadcolor.push("#FFCC00");

    var listMes = json.listaAfIndicadores;
    var jsonIndCtaf = json.listaAfIndicadores;
    for (var i in listMes) {
        _anio = listMes[i].Anio;
        if (jsonmes.indexOf(listMes[i].MesNombre) < 0) {
            jsonmes.push((listMes[i].MesNombre + "").trim());
        }
    }

    var jsonmanto = [];
    var k;
    var j;
    for (var i in jsonIndCtaf) {

        k = jsonmes.indexOf((jsonIndCtaf[i].MesNombre + "").trim());
        var reg = jsonIndCtaf[i].Indctaf;
        jsonmanto[k] = reg;
    }

    var opcion = {
        chart: {
            type: 'column',
            options3d: {
                enabled: true,
                alpha: 10,
                beta: 25,
                depth: 70
            }
        },
        title: {
            text: 'PROCESO DE INFORME CTAF (días) - ' + _anio,
            style: {
                fontWeight: 'bold',
                color: 'black'
            }
        },
        xAxis: {
            categories: jsonmes,
            style: {
                fontSize: '5'
            },
            title: {
                text: 'Meses',
                style: {
                    fontWeight: 'bold',
                    color: 'gray'
                }
            },
        },
        yAxis: {
            min: 0,
            max: 30,
            tickInterval: 5,
            endOnTick: false,
            allowDecimals: false,
            title: {
                text: 'Número de días hábiles',
                style: {
                    fontWeight: 'bold',
                    color: 'gray'
                }
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: 'gray'
                }
            },
            plotLines: [{
                color: 'red',                
                width: 2,
                value: 20,
                zIndex: 5,
                dashStyle: 'shortdash',
                label: {
                    text: 'Plazo fijado por la NTCSE',
                    style: {
                        color: '#686A3B',
                        fontWeight: 'bold'
                    }
                }
            }]
        },
        legend: {
            enabled: false
            //reversed: true
            //layout: 'vertical',
            //align: 'right',
            //verticalAlign: 'middle',
            //borderWidth: 0
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.x + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>' //+
                    //'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {

            series: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true
                }
            }
        },
        colors: jsoncriticidadcolor,
        series: [{
            name: 'Indicador CTAF',
            data: jsonmanto
        }
        ]
    };

    //for (var i in jsonmanto) {
    //    opcion.series.push({
    //        //name: jsoncriticidad[i],
    //        data: jsonmanto[i]
    //    });
    //}


    $('#GrafIndicadorCtaf').highcharts(opcion);
}

cargarIndicadoresIT = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "ListaGraficoIndicadores",
        data: { anio: $('#txtAnio').val() },
        success: function (result) {
            graficoIndicadoresIT(result);
            cargarIndicadoresNroEventos();
        },
        error: function () {

        }
    });
}

graficoIndicadoresIT = function (result) {
    debugger;
    var json = result;
    var _anio;
    var jsonmes = []; //empresa
    var jsoncriticidadcolor = []; //color criticidad
    jsoncriticidadcolor.push("#90ee7e");

    var listMes = json.listaAfIndicadores;
    var jsonIndCtaf = json.listaAfIndicadores;
    for (var i in listMes) {
        _anio = listMes[i].Anio;
        if (jsonmes.indexOf(listMes[i].MesNombre) < 0) {
            jsonmes.push((listMes[i].MesNombre + "").trim());
        }
    }

    var jsonmanto = [];
    var k;
    var j;
    for (var i in jsonIndCtaf) {

        k = jsonmes.indexOf((jsonIndCtaf[i].MesNombre + "").trim());
        var reg = jsonIndCtaf[i].Indit;
        jsonmanto[k] = reg;
    }

    var opcion = {
        chart: {
            type: 'column',
            options3d: {
                enabled: true,
                alpha: 10,
                beta: 25,
                depth: 70
            }
        },
        title: {
            text: 'PROCESO DE ASIGNACION DE RESPONSABILIDAD PROMEDIO (días) - ' + _anio,
            align: 'center',
            style: {
                fontWeight: 'bold',
                color: 'black'
            }
        },
        xAxis: {
            categories: jsonmes,
            style: {
                fontSize: '5'
            },
            title: {
                text: 'Meses',
                style: {
                    fontWeight: 'bold',
                    color: 'gray'
                }
            },
        },
        yAxis: {
            min: 0,
            max: 40,
            tickInterval: 5,
            endOnTick: false,
            allowDecimals: false,
            title: {
                text: 'Número de días hábiles',
                style: {
                    fontWeight: 'bold',
                    color: 'gray'
                }
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: 'gray'
                }
            },
            plotLines: [{
                color: 'red',
                width: 2,
                value: 30,
                zIndex: 5,
                dashStyle: 'shortdash',
                label: {
                    text: 'Plazo fijado por la NTCSE',
                    style: {
                        color: '#686A3B',
                        fontWeight: 'bold'
                    }
                }
            }]
        },
        legend: {
            //reversed: true
            enabled: false
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.x + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {

            series: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true


                }
            }
        },
        colors: jsoncriticidadcolor,
        series: [{
            name: 'Indicador ITD',
            data: jsonmanto
        }
        ]
    };

    $('#GrafIndicadorIt').highcharts(opcion);
}

cargarIndicadoresNroEventos = function () {

    $.ajax({
        type: 'POST',
        url: controlador + "ListaGraficoIndicadores",
        data: { anio: $('#txtAnio').val() },
        success: function (result) {
            graficoIndicadoresNroEventos(result);
            //cargarEmpresaEstado();
        },
        error: function () {

        }
    });
}

graficoIndicadoresNroEventos = function (result) {
    debugger;
    var json = result;
    var _anio;
    var jsonmes = []; //empresa
    var jsoncriticidadcolor = []; //color criticidad
    jsoncriticidadcolor.push("#F45B5B");

    var listMes = json.listaAfIndicadores;
    var jsonIndCtaf = json.listaAfIndicadores;
    for (var i in listMes) {
        _anio = listMes[i].Anio;
        if (jsonmes.indexOf(listMes[i].MesNombre) < 0) {
            jsonmes.push((listMes[i].MesNombre + "").trim());

        }
    }

    var jsonmanto = [];
    var k;
    var j;
    for (var i in jsonIndCtaf) {

        k = jsonmes.indexOf((jsonIndCtaf[i].MesNombre + "").trim());
        var reg = jsonIndCtaf[i].TotalEventosMes;
        jsonmanto[k] = reg;
    }

    var opcion = {
        chart: {
            type: 'column',
            options3d: {
                enabled: true,
                alpha: 10,
                beta: 25,
                depth: 70
            }
        },
        title: {
            text: 'FRECUENCIA DE EVENTOS OCURRIDOS ' + _anio,
            style: {
                fontWeight: 'bold',
                color: 'black'
            }
        },
        xAxis: {
            categories: jsonmes,
            style: {
                fontSize: '5'
            },
            title: {
                text: 'Meses',
                style: {
                    fontWeight: 'bold',
                    color: 'gray'
                }
            },
        },
        yAxis: {
            min: 0,
            max: 30,
            tickInterval: 5,
            endOnTick: false,
            allowDecimals: false,
            title: {
                text: 'Número de Eventos',
                style: {
                    fontWeight: 'bold',
                    color: 'gray'
                }
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: 'gray'
                }
            }
        },
        legend: {
            //reversed: true
            enabled: false
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.x + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            }
        },
        plotOptions: {

            series: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true


                }
            }
        },
        colors: jsoncriticidadcolor,
        series: [{
            name: 'Número de Eventos',
            data: jsonmanto
        }
        ]
    };

    $('#GrafFrecuenciaEve').highcharts(opcion);
}

exportar = function () {
    
    $.ajax({
        type: 'POST',
        url: controlador + "ReporteIndicadores",
        data: { anio: $('#txtAnio').val() },
        success: function (resultado) {
            if (resultado.result == 1) {
                location.href = controlador + "DescargarReporteIndicadores";
            }
            else {
                //mostrarError();
            }
        },
        error: function () {
            //mostrarError();
        }
    });
}