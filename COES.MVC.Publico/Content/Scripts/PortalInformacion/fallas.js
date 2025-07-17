var controlador = siteRoot + 'portalinformacion/';
var vistaFalla = true;
$(document).ready(function () {
       

    $('#txtFechaInicial').Zebra_DatePicker({
        pair: $('#txtFechaFinal'),
        onSelect: function (date) {
            var date1 = getFecha(date);
            var date2 = getFecha($('#txtFechaFinal').val());
            if (date1 > date2) {
                $('#txtFechaFinal').val(date);
            }
        }
    });

    $('#txtFechaFinal').Zebra_DatePicker({
        direction: true
    });        

    $('#cbTipoEmpresa').change(function () {
        cargarEmpresas();
    });
       
    $('#btnConsultarEventoFalla').click(function () {
        buscarEvento();
    });

    $('#btnGraficaFallas').click(function () {
        graficarFalla();
    });  

    $('#btnExportarFallas').click(function () {
        exportar();
    });

    $('#cbTipoEquipo').val("0");
    $('#cbEmpresa').val("0");

    buscarEvento();
});



mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "EventoFallas",
        data: $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#contenidoEventoFallas').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            alert('Ha ocurrido un Error');
        }
    });
}

buscarEvento = function () {
    pintarPaginado();
    mostrarListado(1);
}

pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "Paginado",
        data: $('#frmBusqueda').serialize(),
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            alert('Ha ocurrido un Error');
        }
    });
}

pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
}

cargarEmpresas = function () {
    $.ajax({
        type: 'POST',
        url: controlador + 'cargarempresas',
        dataType: 'json',
        data: { idTipoEmpresa: $('#cbTipoEmpresa').val() },
        cache: false,
        success: function (aData) {
            $('#cbEmpresa').get(0).options.length = 0;
            $('#cbEmpresa').get(0).options[0] = new Option("( TODOS )", "");
            $.each(aData, function (i, item) {
                $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            alert('Ha ocurrido un Error');
        }
    });
}

exportar = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "exportarevento",
        dataType: 'json',
        cache: false,
        data: $('#frmBusqueda').serialize(),
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "descargarevento";
            }
            else {
                alert('Ha ocurrido un Error');
            }
        },
        error: function () {
            alert('Ha ocurrido un Error');
        }
    });
}

graficarFalla = function () {
    $('#paginado').html("");
    $('#contenidoEventoFallas').html(
        '<div class="bg-dashboard-info" style="margin-bottom:10px">PARTICIPACIÓN DEL NÚMERO DE FALLAS POR TIPO DE CAUSA DE FALLA CIER</div>' +
        '<div id="graficoFallaCier"></div>' +
        '<div class="bg-dashboard-info" style="margin-bottom:10px">DURACIÓN DE FALLA POR TIPO DE EQUIPO Y CAUSA DE FALLA CIER</div>' +
        '<div id="graficoEquipoCier"></div>' +
        '<div class="bg-dashboard-info" style="margin-bottom:10px">DURACIÓN DE FALLA POR TIPO DE CAUSA DE FALLA CIER Y NIVEL DE TENSIÓN</div>' +
        '<div id="graficoCierTension"></div>' +
        '<div class="bg-dashboard-info" style="margin-bottom:10px">ENERGÍA INTERRUMPIDA APROXIMADA POR TIPO DE EQUIPO (MWh)</div>' +
        '<div id="graficoEnInterrumpidaEquipo"></div>');
    $.ajax({
        type: 'POST',
        url: controlador + 'DatosGraficoFalla',
        dataType: 'json',
        data: $('#frmBusqueda').serialize(),
        success: function (result) {
            $('#graficoFallaCier').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: null
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.y}</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '{point.percentage:.1f} %'
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    name: 'Numero de fallas',
                    data: result["FallaCier"]
                }]
            });

            $('#graficoEnInterrumpidaEquipo').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: result["NombresEnergiaInterrumpidaxEquipo"],
                    title: {
                        text: 'TIPO DE EQUIPO'
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'MWh'
                    },
                    stackLabels: {
                        enabled: false
                    }
                },
                legend: {
                    enabled: false
                },
                tooltip: {
                    headerFormat: '<b>{point.x}</b><br/>',
                    pointFormat: 'Energía Interrumpida: <b>{point.stackTotal} MWh</b>'
                },
                plotOptions: {
                    column: {
                        stacking: 'normal',
                        dataLabels: {
                            enabled: true,
                            color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                            style: {
                                textShadow: '0 0 3px black'
                            }
                        }
                    }
                },
                series: [result["EnergiaInterrumpidaxEquipo"]]
            });

            $('#graficoEquipoCier').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: result["NombresDuracionxTipoEquipoYCier"]
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Duración minutos'
                    },
                    stackLabels: {
                        enabled: true,
                        style: {
                            fontWeight: 'bold',
                            color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                        }
                    }
                },
                legend: {
                    align: 'right',
                    x: -30,
                    verticalAlign: 'top',
                    y: 25,
                    floating: true,
                    backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                    borderColor: '#CCC',
                    borderWidth: 1,
                    shadow: false
                },
                tooltip: {
                    headerFormat: '<b>{point.x}</b><br/>',
                    pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
                },
                plotOptions: {
                    column: {
                        stacking: 'normal',
                        dataLabels: {
                            enabled: true,
                            color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                            style: {
                                textShadow: '0 0 3px black'
                            }
                        }
                    }
                },
                series: result["DuracionxTipoEquipoYCier"]
            });
            $('#graficoCierTension').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: null
                },
                xAxis: {
                    categories: result["NombresDuracionxCierYTension"]
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Duración minutos'
                    },
                    stackLabels: {
                        enabled: true,
                        style: {
                            fontWeight: 'bold',
                            color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                        }
                    }
                },
                legend: {
                    align: 'right',
                    x: -30,
                    verticalAlign: 'top',
                    y: 25,
                    floating: true,
                    backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                    borderColor: '#CCC',
                    borderWidth: 1,
                    shadow: false
                },
                tooltip: {
                    headerFormat: '<b>{point.x}</b><br/>',
                    pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
                },
                plotOptions: {
                    column: {
                        stacking: 'normal',
                        dataLabels: {
                            enabled: true,
                            color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                            style: {
                                textShadow: '0 0 3px black'
                            }
                        }
                    }
                },
                series: result["DuracionxCierYTension"]
            });
        },
        error: function () {

        }
    });
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}