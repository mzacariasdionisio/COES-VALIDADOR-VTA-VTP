$(function () {
    cargarTensionBarraSemanal();
});

function mostrarReporteByFiltros() {
    cargarTensionBarraSemanal();
}

function cargarTensionBarraSemanal() {
    var codigoVersion = getCodigoVersion();
    var tension = $("#hdRed").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarTensionBarraSemanal',
        data: {
            codigoVersion: codigoVersion,
            red: tension 
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
            $('#listado').html(aData.Resultado);

            if (aData.ListaGrafico != null) {// si existen registros
                $('#grafico1').css("display", "block");
                TensionBarraSemanal(aData, "grafico1", 1, tension
                );
            }
            else {// No existen registros
                $('#' + grafic).css("display", "none");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function TensionBarraSemanal(result, idGrafico, tipografico, tension) {
    var opcion;
    
    if (tipografico == 1) {
        var json = result.ListaGrafico;
        //var json = result.Series;
        var jsondata = [];
        var indice = 3;
        for (var i in json) {
            var jsonValor = [];
            var jsonLista = json[i].ListaVal;
            for (var j in jsonLista) {
                jsonValor.push(jsonLista[j]);
            }
            jsondata.push({
                name: json[i].SerieName,
                data: jsonValor,
                color: json[i].SerieColor,
                //index: indice
            });
            indice--;
        }

        opcion = {
            chart: {
                type: 'scatter'
            },
            title: {
                text: result.Grafico.TitleText
            },
            xAxis: {
                title: {
                    text: ''
                },
                categories: result.Grafico.XAxisCategories
            },
            credits: {
                enabled: false
            },
            yAxis: {
                title: { text: '' },
                plotLines: [{
                    color: '#AA0000',
                    width: 3,
                    value: tension
                }]
            },
            //legend: {
            //    reversed: true
            //},
            subtitle: {
                text: result.Grafico.Subtitle,
                align: 'left',
                verticalAlign: 'bottom',
                floating: true,
                x: 10,
                y: 9
            },
            plotOptions: {
                //bar: {
                //    dataLabels: {
                //        enabled: false
                //    }
                //}
            },
            series: jsondata
        };
    }

    $('#' + idGrafico).highcharts(opcion);
}