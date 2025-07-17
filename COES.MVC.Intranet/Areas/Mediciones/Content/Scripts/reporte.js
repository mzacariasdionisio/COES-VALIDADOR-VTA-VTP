var controlador = siteRoot + 'mediciones/reportemedidores/'

$(function () {

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

    $('#tab-container').easytabs({
        animate: false
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

    $('#cbTipoGeneracion').multipleSelect({
        width: '100%',
        filter: true
    });

    $('#cbRecursoEnergetico').multipleSelect({
        width: '100%',
        filter: true
    });
     
    $('#btnConsultar').click(function(){
        mostrarReporte();       
    });

    $('#btnExportar').click(function () {
        exportar();
    });

    cargarPrevio();
    cargarEmpresas();
});


cargarPrevio = function () {
    $('#cbTipoGeneracion').multipleSelect('checkAll');
    $('#cbTipoEmpresa').multipleSelect('checkAll');
    $('#cbRecursoEnergetico').multipleSelect('checkAll');
}

formatearTabla = function () {
    $('#listadoRecurso').css("width", ($('#mainLayout').width() - 35) + "px");
    $('#listadoTipo').css("width", ($('#mainLayout').width() - 35) + "px");

    /*$("#tbRecurso").freezeHeader({ 'height': '480px' });
    $("#tbTipoEnergetica").freezeHeader({ 'height': '480px' });*/
    
}

cargarEmpresas = function () {
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


mostrarReporte = function ()
{
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var fuenteEnergia = $('#cbRecursoEnergetico').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');

    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfFuenteEnergia').val(fuenteEnergia);
    $('#hfTipoEmpresa').val(tipoEmpresa);

    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "")
    {
        $.ajax({
            type: 'POST',
            url: controlador + "reporte",
            data: {
                fechaInicial: $('#txtFechaInicial').val(), fechaFinal: $('#txtFechaFinal').val(),
                tiposEmpresa : $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                fuentesEnergia: $('#hfFuenteEnergia').val(), central: $('#cbCentral').val()
            },
            success: function (evt) {               
                $('#reporte').html(evt);
                //mostrarGraficos();
            },
            error: function () {
                mostrarError();
            }
        });
    }
    else
    {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}

cargarReporteResumen = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "grafico",
        dataType: 'json',      
        success: function (result) {            
            graficoMaxima(result);
            graficoEnergia(result);     
        },
        error: function () {
            mostrarError();
        }
    });
}

cargarReporteTipoGeneracion = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "graficotipogeneracion",
        dataType: 'json',
        success: function (result) {
            graficoTipoGeneracion(result);         
        },
        error: function () {
            mostrarError();
        }
    });
}


cargarReporteEmpresa = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "graficoempresas",
        dataType: 'json',
        data: {
            tipoGeneracion: $('#cbReporteGeneracion').val()
        },
        success: function (result) {
            graficoEmpresa(result);
        },
        error: function () {
            mostrarError();
        }
    });
}

graficoMaxima = function (result){

    var json = result;
    var jsondata = [];
    for (var i in json) {
        jsondata.push([json[i].Fenergnomb, json[i].MDFuenteEnergia]);
    }

    $('#graficoMaxima').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: ''
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage}%</b>',
            percentageDecimals: 1
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,                               
                    formatter: function () {
                        return '<b>' + this.point.name + '</b>: ' + $.number(this.percentage, 3, ',', ' ') + ' %';
                    }
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Participación',
            data: jsondata
        }]
    }, function (chart) {
        var txt = "";

        txt = txt + '<table class="tabla-formulario">';
        txt = txt + '<thead>';
        txt = txt + '<tr>';
        txt = txt + '<th>Tipo de Recurso Energético</th><th>Máxima Demanda (MW)</th><th>Participación (%)</th>';
        txt = txt + '</tr>';
        txt = txt + '</thead>';
        txt = txt + '<tbody>';
        var total = 0;
        $.each(chart.series[0].data, function (j, data) {
            txt = txt + '<tr class="itemcolor">';
            txt = txt + '<td><div class="symbol" style="background-color:' + data.color + '"></div><div class="serieName" id="">' + data.name + '</div></td><td style="text-align:right">' + $.number(data.y, 3, ',', ' ') + '</td><td style="text-align:right">' + $.number(data.percentage, 3, ',', ' ') + '</td>';
            txt = txt + '</tr>';

            total = total + data.y;
        });
        txt = txt + '</tbody>';
        txt = txt + '<tfoot>';
        txt = txt + '<tr class="table-total1">';
        txt = txt + '<td style="text-align:right">TOTAL</td><td style="text-align:right">' + $.number(total, 3, ',', ' ') + '</td><td style="text-align:right">100,000</td>';
        txt = txt + '</tr>';
        txt = txt + '</tfoot>';
        txt = txt + '</table>';
        $('#legendMaxima').html(txt);

        $('#legendMaxima .itemcolor').click(function () {

            var inx = $(this).index(),
                point = chart.series[0].data[inx];
            if (point.visible) {
                point.setVisible(false);
                $(this).css("background-color", '#EAF7D9');
            }
            else {
                point.setVisible(true);
                $(this).css("background-color", '#fff');
            }
        });
    });
}

graficoEnergia = function (result) {
    var json = result;
    var jsondata = [];
    for (var i in json) {
        jsondata.push([json[i].Fenergnomb, json[i].EnergiaFuenteEnergia]);
    }

    $('#graficoEnergia').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: ''
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage}%</b>',
            percentageDecimals: 1
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,                   
                    formatter: function () {
                        return '<b>' + this.point.name + '</b>: ' + $.number(this.percentage, 3, ',', ' ') + ' %';
                    }
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Participación',
            data: jsondata
        }]
    }, function (chart) {
        var txt = "";

        txt = txt + '<table class="tabla-formulario">';
        txt = txt + '<thead>';
        txt = txt + '<tr>';
        txt = txt + '<th>Tipo de Recurso Energético</th><th>Energía (MWh)</th><th>Participación (%)</th>';
        txt = txt + '</tr>';
        txt = txt + '</thead>';
        txt = txt + '<tbody>';
        var total = 0;
        $.each(chart.series[0].data, function (j, data) {
            txt = txt + '<tr class="itemcolor">';
            txt = txt + '<td><div class="symbol" style="background-color:' + data.color + '"></div><div class="serieName" id="">' + data.name + '</div></td><td style="text-align:right">' + $.number(data.y, 3, ',', ' ') + '</td><td style="text-align:right">' + $.number(data.percentage, 3, ',', ' ') + '</td>';
            txt = txt + '</tr>';
            total = total + data.y;
        });
        txt = txt + '</tbody>';
        txt = txt + '<tfoot>';
        txt = txt + '<tr class="table-total1">';
        txt = txt + '<td style="text-align:right">TOTAL</td><td style="text-align:right">' + $.number(total, 3, ',', ' ') + '</td><td style="text-align:right">100,000</td>';
        txt = txt + '</tr>';
        txt = txt + '</tfoot>';
        txt = txt + '</table>';
        $('#legendEnergia').html(txt);

        $('#legendEnergia .itemcolor').click(function () {

            var inx = $(this).index(),
                point = chart.series[0].data[inx];
            if (point.visible) {
                point.setVisible(false);
                $(this).css("background-color", '#EAF7D9');
            }
            else {
                point.setVisible(true);
                $(this).css("background-color", '#fff');
            }
        });
    });
}

graficoEmpresa = function (result)
{    
    var json = result;
    var jsondata = [];
    for (var i in json) {       
        jsondata.push([json[i].Emprnomb, json[i].Meditotal]);
    }    

    $('#graficoEmpresa').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: ''
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage}%</b>',
            percentageDecimals: 1
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,                   
                    formatter: function () {
                        return '<b>' + this.point.name + '</b>: ' + $.number(this.percentage, 3, ',', ' ') + ' %';
                    }
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Participación',
            data: jsondata
        }]
    }, function (chart) {

        var txt = "";
               
        txt = txt + '<table class="tabla-formulario">';
        txt = txt + '<thead>';
        txt = txt + '<tr>';
        txt = txt + '<th>Empresas</th><th>Energía (MWh)</th><th>Participación (%)</th>';
        txt = txt + '</tr>';
        txt = txt + '</thead>';
        txt = txt + '<tbody>';
        var total = 0;
        $.each(chart.series[0].data, function (j, data) {
            txt = txt + '<tr class="itemcolor">';
            txt = txt + '<td><div class="symbol" style="background-color:' + data.color + '"></div><div class="serieName" id="">' + data.name + '</div></td><td style="text-align:right">' + $.number(data.y, 3, ',', ' ') + '</td><td style="text-align:right">' + $.number(data.percentage, 3, ',', ' ') + '</td>';
            txt = txt + '</tr>';

            total = total + data.y;
        });
        txt = txt + '</tbody>';
        txt = txt + '<tfoot>';
        txt = txt + '<tr class="table-total1">';
        txt = txt + '<td style="text-align:right">TOTAL</td><td style="text-align:right">' + $.number(total, 3, ',', ' ') + '</td><td style="text-align:right">100,000</td>';
        txt = txt + '</tr>';
        txt = txt + '</tfoot>';
        txt = txt + '</table>';
        $('#legendEmpresa').html(txt);

        $('#legendEmpresa .itemcolor').click(function () {
                       
            var inx = $(this).index(),
                point = chart.series[0].data[inx];
            if (point.visible) {
                point.setVisible(false);
                $(this).css("background-color", '#EAF7D9');
            }
            else {
                point.setVisible(true);
                $(this).css("background-color", '#fff');
            }
        });

    });
}

graficoTipoGeneracion = function (result) {
    var json = result;
    var jsondata = [];
    for (var i in json) {
        jsondata.push([json[i].Tgenernomb, json[i].MDFuenteEnergia]);
    }

    $('#graficaGeneracion').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: ''
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage}%</b>',
            percentageDecimals: 1
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,                    
                    formatter: function () {
                        return '<b>' + this.point.name + '</b>: ' + $.number(this.percentage, 3, ',', ' ') + ' %';
                    }
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Participación',
            data: jsondata
        }]
    }, function (chart) {
        var txt = "";

        txt = txt + '<table class="tabla-formulario">';
        txt = txt + '<thead>';
        txt = txt + '<tr>';
        txt = txt + '<th>Tipo de Generación</th><th>Máxima Demanda (MW)</th><th>Participación (%)</th>';
        txt = txt + '</tr>';
        txt = txt + '</thead>';
        txt = txt + '<tbody>';
        var total = 0;
        $.each(chart.series[0].data, function (j, data) {
            txt = txt + '<tr class="itemcolor">';
            txt = txt + '<td><div class="symbol" style="background-color:' + data.color + '"></div><div class="serieName" id="">' + data.name + '</div></td><td style="text-align:right">' + $.number(data.y, 3, ',', ' ') + '</td><td style="text-align:right">' + $.number(data.percentage, 3, ',', ' ')  + '</td>';
            txt = txt + '</tr>';
            total = total + data.y;
        });
        txt = txt + '</tbody>';
        txt = txt + '<tfoot class="table-total1">';
        txt = txt + '<tr>';
        txt = txt + '<td style="text-align:right">TOTAL</td><td style="text-align:right">' + $.number(total, 3, ',', ' ') + '</td><td style="text-align:right">100,000</td>';
        txt = txt + '</tr>';
        txt = txt + '</tfoot>';
        txt = txt + '</table>';
        $('#legendGeneracion').html(txt);

        $('#legendGeneracion .itemcolor').click(function () {

            var inx = $(this).index(),
                point = chart.series[0].data[inx];
            if (point.visible) {
                point.setVisible(false);
                $(this).css("background-color", '#EAF7D9');
            }
            else {
                point.setVisible(true);
                $(this).css("background-color", '#fff');
            }
        });
    });

}

exportar = function () {
    
    var empresa = $('#cbEmpresas').multipleSelect('getSelects');
    var tipoGeneracion = $('#cbTipoGeneracion').multipleSelect('getSelects');
    var fuenteEnergia = $('#cbRecursoEnergetico').multipleSelect('getSelects');
    var tipoEmpresa = $('#cbTipoEmpresa').multipleSelect('getSelects');

    $('#hfEmpresa').val(empresa);
    $('#hfTipoGeneracion').val(tipoGeneracion);
    $('#hfFuenteEnergia').val(fuenteEnergia);
    $('#hfTipoEmpresa').val(tipoEmpresa);


    if ($('#txtFechaInicial').val() != "" && $('#txtFechaFinal').val() != "") {
        $.ajax({
            type: 'POST',
            url: controlador + "exportar",
            dataType: 'json',
            data: {
                fechaInicial: $('#txtFechaInicial').val(), fechaFinal: $('#txtFechaFinal').val(),
                tiposEmpresa : $('#hfTipoEmpresa').val(),
                empresas: $('#hfEmpresa').val(), tiposGeneracion: $('#hfTipoGeneracion').val(),
                fuentesEnergia: $('#hfFuenteEnergia').val(), central: $('#cbCentral').val()
            },
            success: function (result) {
                if (result == 1) {
                    window.location = controlador + 'descargar';
                }
                if (result == -1) {
                    mostrarError();
                }
            },
            error: function () {
                mostrarError()
            }
        });
    }
    else {
        mostrarAlerta("Seleccione rango de fechas.");
    }
}

mostrarAlerta = function (mensaje) {
    alert(mensaje);
}

mostrarError = function () {
    alert("Ha ocurrido un error.");
}

getFecha = function (date) {
    var parts = date.split("/");
    var date = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date.getTime();
}

