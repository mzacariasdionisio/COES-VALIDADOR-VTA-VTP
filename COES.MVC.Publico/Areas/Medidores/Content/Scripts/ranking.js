var controlador = siteRoot + 'Medidores/Ranking/'

$(function () {

    //$('#tab-container').easytabs({
    //    animate: false
    //});

    $('#FechaConsulta').change({
        format: 'Y-m',
        direction: false,
        onSelect: function (date) {
            $('#mensajeGeneral').removeClass();
            $('#mensajeGeneral').text("");
            if (date == $('#hfFechaActual').val()){
                $('#FechaConsulta').val($('#hfFechaConsulta').val());
                $('#mensajeGeneral').addClass("action-alert");
                $('#mensajeGeneral').text("Solo puede seleccionar meses anteriores al actual.");
            }
        }
    });

    $('#cbTipoGeneracion').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#btnConsultar').click(function () {
        consultar();
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    $('#cbTipoEmpresa').multipleSelect({
        width: '100%',
        filter: true,
        onClick: function (view) {
            cargarEmpresas();
        },
        onClose: function (view) {
            cargarEmpresas();
        }
    });

    cargarPrevio();
    cargarEmpresas();
    consultar();
});

cargarPrevio = function () {
    $('#cbTipoGeneracion').multipleSelect('checkAll');
    $('#cbTipoEmpresa').multipleSelect('checkAll');
}

cargarEmpresas = function ()
{
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    $('#hfTipoEmpresa').val(tipoEmpresa);

    $.ajax({
        type: 'POST',
        url: controlador + "empresas",
        data: {
            tiposEmpresa: $('#hfTipoEmpresa').val()
        },
        success: function (evt) {
            $('#empresas').html(evt);
        },
        error: function () {
            mostrarError();
        }
    });
}



consultar = function ()
{
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#FechaConsulta').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "consulta",
            data: {
                fecha: $('#FechaConsulta').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val()
            },
            success: function (evt) {
                $('#divConsulta').html(evt);
                ordenamiento($('#FechaConsulta').val());
                
                diagramaCarga($('#FechaConsulta').val(), $('#hfTipoEmpresa').val(), $('#hfEmpresa').val(), $('#hfTipoGeneracion').val(), $('#cbCentral').val());
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else
    {
        alert("Por favor seleccione mes.");
    }
}

ordenamiento = function(fecha)
{
    $.ajax({
        type: 'POST',
        dataType: 'json',
        cache: false,
        url: controlador + "ordenamiento",
        data: {
            fecha: fecha
        },
        success: function (result) {                  
            plotear(result.ListaDemandaDia);
            pintarResultado(result.ProduccionEnergia, result.FactorCarga);
            pintarPaginado(1);
        },
        error: function () {
            mostrarError();
        }
    });
}

evolucion = function (tiposEmpresa, empresas, tiposGeneracion, central)
{
    $.ajax({
        type: 'POST',
        dataType: 'json',
        cache: false,
        url: controlador + "evolucion",
        data: {
            tiposEmpresa: tiposEmpresa, empresas:empresas, tiposGeneracion:tiposGeneracion, central:central
        },
        success: function (result) {            
            plotearEvolucion(result);
        },
        error: function () {
            mostrarError();
        }
    });
}

diagramaCarga = function (fecha, tiposEmpresa, empresas, tiposGeneracion, central)
{
    $.ajax({
        type: 'POST',
        dataType: 'json',
        cache: false,
        url: controlador + "diagramacarga",
        data: {
            fecha: fecha, tiposEmpresa: tiposEmpresa, empresas: empresas, tiposGeneracion: tiposGeneracion,
            central: central
        },
        success: function (result) {
            plotearDiagramaCarga(result);
        },
        error: function () {
            mostrarError();
        }
    });
}

plotear = function (result)
{
    var json = result;
    var jsondata = [];
    var count = 1;
    for (var i in json) {

        var variable = count / 4;
        jsondata.push([variable, json[i].Valor]);
        count++;
    }

    $('#grafico').highcharts({
        chart: {
            type: 'area'
        },
        title: {
            text: 'Diagrama de duración mensual'
        },
        xAxis: {
            allowDecimals: false,
            title: {
                text: 'Tiempo (h)'
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        yAxis: {
            title: {
                text: 'Potencia (MW)'
            },
            labels: {
                formatter: function () {
                    return this.value / 1000 + 'k';
                }
            }
        },
        tooltip: {
            pointFormat: '{series.name} Potencia <b>{point.y:,.0f}</b><br/> en la hora {point.x}'
        },
        plotOptions: {
            area: {
                pointStart: 1,
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        },
        series: [{
            name: 'Potencia',
            data: jsondata
        }]
    });
}


plotearEvolucion = function (result)
{
    var json = result.ListaSerie;
    var indice = result.IndiceMaximaDemanda;
    var titulo = result.Titulo;
    var valorMaxima = result.ValorMaximaDemanda;

    var jsondata = [];

    for (var i in json) {
        var jsonValor = [];
        var jsonLista = json[i].ListaValores;
        for (var j in jsonLista) {
            jsonValor.push(jsonLista[j]);
        }
        jsondata.push({
            name: json[i].SerieName,
            data: jsonValor,
            color: json[i].SerieColor
        });
    }
   
    var opciones = {
        chart: {
            type: 'area'
        },
        title: {
            text: 'DESPACHO EN EL DÍA DE MÁXIMA DEMANDA POR TIPO DE RECURSO ENERGÉTICO ' + titulo
        },
        xAxis: {
            allowDecimals: false,            
            title: {
                text: 'Tiempo'
            },
            labels: {
                rotation: -90,
                formatter: function () {
                    return this.value;
                }
            },
            categories: [
                '00:15', '00:30', '00:45', '01:00', '01:15', '01:30', '01:45',
                '02:00', '02:15', '02:30', '02:45', '03:00', '03:15', '03:30', '03:45',
                '04:00', '04:15', '04:30', '04:45', '05:00', '05:15', '05:30', '05:45',
                '06:00', '06:15', '06:30', '06:45', '07:00', '07:15', '07:30', '07:45',
                '08:00', '08:15', '08:30', '08:45', '09:00', '09:15', '09:30', '09:45',
                '10:00', '10:15', '10:30', '10:45', '11:00', '11:15', '11:30', '11:45',
                '12:00', '12:15', '12:30', '12:45', '13:00', '13:15', '13:30', '13:45',
                '14:00', '14:15', '14:30', '14:45', '15:00', '15:15', '15:30', '15:45',
                '16:00', '16:15', '16:30', '16:45', '17:00', '17:15', '17:30', '17:45',
                '18:00', '18:15', '18:30', '18:45', '19:00', '19:15', '19:30', '19:45',
                '20:00', '20:15', '20:30', '20:45', '21:00', '21:15', '21:30', '21:45',
                '22:00', '22:15', '22:30', '22:45', '23:00', '23:15', '23:30', '23:45', '00:00'
            ],
            tickInterval: 2,
            plotLines: [{
                color: 'red', 
                dashStyle: 'longdashdot', 
                value: indice, 
                width: 2,
                label: {
                    text: $.number(valorMaxima, 3, ',', ' '),
                    align: 'left'
                }
            }]
        },
        yAxis: {
            title: {
                text: 'Potencia (MW)'
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        tooltip: {
            pointFormat: '{series.name} Potencia <b>{point.y:,.0f}</b><br/> en la hora {point.x}'
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                lineColor: '#666666',
                lineWidth: 1,
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        },
        series: jsondata
    };
     
    $('#graficoRecursoEnergetico').highcharts(opciones);
}

plotearDiagramaCarga = function (result) {
       
    var json = result.ListaSerie;
    var indiceMaxima = result.IndiceMaximaDemanda;
    var indiceMinima = result.IndiceMinimaDemanda;
    var valorMaxima = result.ValorMaximaDemanda;
    var valorMinima = result.ValorMinimaDemanda;

    var jsondata = [];

    for (var i in json) {
        var jsonValor = [];
        var jsonLista = json[i].ListaValores;
        for (var j in jsonLista) {
            jsonValor.push(jsonLista[j]);            
        }
        jsondata.push({
            name: json[i].SerieName,
            data: jsonValor,
            color: json[i].SerieColor
        });
    }    

    var opciones = {
        chart: {
            type: 'area'
        },
        title: {
            text: 'DIAGRAMA DE CARGA DEL SEIN MÁXIMA Y MÍNIMA DEMANDA ACUMULADA A ' + result.Titulo
        },
        xAxis: {
            allowDecimals: false,
            title: {
                text: 'Tiempo'
            },
            labels: {
                rotation: -90,
                formatter: function () {
                    return this.value;
                }
            },
            categories: [
                '00:15', '00:30', '00:45', '01:00', '01:15', '01:30', '01:45',
                '02:00', '02:15', '02:30', '02:45', '03:00', '03:15', '03:30', '03:45',
                '04:00', '04:15', '04:30', '04:45', '05:00', '05:15', '05:30', '05:45',
                '06:00', '06:15', '06:30', '06:45', '07:00', '07:15', '07:30', '07:45',
                '08:00', '08:15', '08:30', '08:45', '09:00', '09:15', '09:30', '09:45',
                '10:00', '10:15', '10:30', '10:45', '11:00', '11:15', '11:30', '11:45',
                '12:00', '12:15', '12:30', '12:45', '13:00', '13:15', '13:30', '13:45',
                '14:00', '14:15', '14:30', '14:45', '15:00', '15:15', '15:30', '15:45',
                '16:00', '16:15', '16:30', '16:45', '17:00', '17:15', '17:30', '17:45',
                '18:00', '18:15', '18:30', '18:45', '19:00', '19:15', '19:30', '19:45',
                '20:00', '20:15', '20:30', '20:45', '21:00', '21:15', '21:30', '21:45',
                '22:00', '22:15', '22:30', '22:45', '23:00', '23:15', '23:30', '23:45', '00:00'
            ],
            tickInterval: 2,
            plotLines: [{
                color: 'red',
                dashStyle: 'longdashdot',
                value: indiceMaxima,
                width: 2,
                label: { 
                    text: $.number(valorMaxima, 3, ',', ' '),
                    align: 'left'                   
                }
            },
            {
                color: 'red',
                dashStyle: 'longdashdot',
                value: indiceMinima,
                width: 2,
                label: { 
                    text: $.number(valorMinima, 3, ',', ' '),
                    align: 'left'                   
                }
            }
            ]
        },
        yAxis: {
            title: {
                text: 'Potencia (MW)'
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        tooltip: {
            pointFormat: '{series.name} Potencia <b>{point.y:,.0f}</b><br/> en la hora {point.x}'
        },
        plotOptions: {
            area: {
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        },
        series: jsondata
    };

    $('#graficoDiagramaCarga').highcharts(opciones);
    
}

pintarResultado = function (produccionEnergia, factorCarga)
{
    $('#produccionEnergia').html($.number(produccionEnergia, 3, ',', ' '));
    $('#factorCarga').html($.number(factorCarga, 3, ',', ' '));
}

pintarPaginado = function (index) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        cache: false,
        url: controlador + "paginado",
        data: {
            index: index
        },
        success: function (result) {
            $('.page-item').removeClass("page-active");
            $('#page-item' + index).addClass("page-active");
            $("#tablaOrdenamiento > tbody").html("");
            var json = result;
            var count = (index - 1) * 300 + 1;
            var valExp = 0;
            var valImp = 0;

            for (var i in json) {
                valExp = 0;
                valImp = 0;
                if (parseFloat(json[i].ValorInter) < 0) {
                    valImp = parseFloat(json[i].ValorInter) * -1;
                }
                else {
                    valExp = parseFloat(json[i].ValorInter);
                }

                $('#tablaOrdenamiento >tbody').append(
                    '<tr>' +
                    '   <td>' + count + '</td>' +
                    '   <td>' + json[i].StrMediFecha + '</td>' +
                    '   <td>' + $.number(json[i].ValorGeneracion, 3, ',', ' ') + '</td>' +
                    '   <td>' + $.number(valImp, 3, ',', ' ') + '</td>' +
                    '   <td>' + $.number(valExp, 3, ',', ' ') + '</td>' +
                    '   <td>' + $.number(json[i].Valor, 3, ',', ' ') + '</td>' +
                    '</tr>'
                );
                count++;
            }
        },
        error: function () {
            mostrarError();
        }
    });
}

exportar = function ()
{
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');
    if (empresa == "[object Object]") empresa = "";
    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#FechaConsulta').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            dataType: 'json',
            data: {
                fecha: $('#FechaConsulta').val(),
                tiposEmpresa: $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                central: $('#cbCentral').val()
            },
            success: function (result) {
                if (result == "1") {
                    window.location = controlador + 'descargar'
                }
                else {
                    alert(result);
                }
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else {
        alert("Por favor seleccione mes.");
    }
}

mostrarAlerta = function (mensaje)
{
    alert(mensaje);
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}