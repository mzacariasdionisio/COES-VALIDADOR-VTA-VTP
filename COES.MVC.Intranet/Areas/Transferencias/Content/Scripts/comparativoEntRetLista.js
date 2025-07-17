var controler = siteRoot + "transferencias/ComparativoEntregaRetiro/";

//Funciones de busqueda
$(document).ready(function () {
    var $chart = null;
    oTable = $('#tabla2').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true"
    });

    $("#tabla_length").css("display", "none");

    $('#grafico3').click(function () {
        $('#grafico2').removeClass('active');
        $('#grafico1').addClass('active');

        metodo.dibujarGrafica();

    });

    $('#grafico4').click(function () {
        $('#grafico1').removeClass('active');
        $('#grafico2').addClass('active');

        metodo.dibujarGrafica();

    });

    //metodo.cargarDatosIniciales();

});

var metodo = {
    cargarDatosIniciales: function () {

        const timezone = new Date().getTimezoneOffset()
        Highcharts.setOptions({
            lang: {
                loading: 'Cargando...',
                months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                weekdays: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                shortMonths: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                exportButtonTitle: "Exportar",
                printButtonTitle: "Importar",
                rangeSelectorFrom: "Desde",
                rangeSelectorTo: "Hasta",
                rangeSelectorZoom: "Período",
                downloadPNG: 'Descargar imagen PNG',
                downloadJPEG: 'Descargar imagen JPEG',
                downloadPDF: 'Descargar imagen PDF',
                downloadSVG: 'Descargar imagen SVG',
                printChart: 'Imprimir',
                resetZoom: 'Reiniciar zoom',
                resetZoomTitle: 'Reiniciar zoom',
                thousandsSep: ",",
                decimalPoint: '.'
            },
            global: {
                timezoneOffset: timezone
            },
            exporting: {
                enabled: true
            }
        });
        metodo.chartJS.init(null);
    },
    chartJS: {
        init: (series) => {

            var id = $('#tab-container li.active a').attr('data-id')

            $chart = Highcharts.chart('contenedor2', {
                chart: {
                    zoomType: 'x'
                },
                title: {
                    text: 'Comparación de Entregas y Retiros'
                },
                xAxis: {
                    type: 'decimal'
                },
                 yAxis: {
                        title: {
                            text: 'MWh'
                        }
                    },
                legend: {
                    enabled: true
                },
                plotOptions: {
                    area: {
                        fillColor: {

                            stops: [
                                [0, Highcharts.getOptions().colors[0]],
                                [1, Highcharts.color(Highcharts.getOptions().colors[0]).setOpacity(0).get('rgba')]
                            ]
                        },
                        marker: {
                            radius: 2
                        },
                        lineWidth: 1,
                        states: {
                            hover: {
                                lineWidth: 1
                            }
                        },
                        threshold: null
                    }
                },
                series: series
            });

        },
        update: (series) => {
            chart.series[0].update({ series: series });
        }
    }
    ,
    dibujarGrafica: () => {
        var pericodi = $("#pericodi1 option:selected").val();
        var version = $("#recacodi1 option:selected").val();
        var pericodi2 = $("#pericodi2 option:selected").val();
        var version2 = $("#recacodi2 option:selected").val();
        if (pericodi == "" || version == "") {
            alert('Por favor, seleccione el Periodo y la versión');
        } else if (pericodi2 == "" || version2 == "") {
            alert('Por favor, seleccione el Periodo y la versión');
        }
        else {
            var trnenvtipinf = $("#trnenvtipinf option:selected").val();
            var empcodi = $("#emprcodi1 option:selected").val();
            var cliemprcodi = $("#cliemprcodi1 option:selected").val();
            var barrcodi = $("#barrcodi1 option:selected").val();
            var flag = $("#flag1 option:selected").val();
            if (empcodi == '')
                empcodi = null;
            if (barrcodi == '')
                barrcodi = null;
            if (pericodi == '')
                pericodi = null;
            if (cliemprcodi == '')
                cliemprcodi = null;
            if (flag == 'T')
                flag = null;

            $.ajax({
                type: 'POST',
                url: controler + "MostrarComparativos",
                data: {
                    trnenvtipinf: trnenvtipinf, pericodi: pericodi, version: version, pericodi2: pericodi2, version2: version2,
                    empcodi: empcodi, cliemprcodi: cliemprcodi, barrcodi: barrcodi, flagEntrReti: flag,
                    dias: "", codigos: ""
                },
                success: function (evt) {

                    armarData(evt, pericodi, pericodi2);

                },
                error: function () {
                    //mostrarError();
                }
            });
        }
    }

}

var arrayData = [];

armarData = function (dataER, periodo1,periodo2) {
    var lista = [];

    var obj1 = {
        name: periodo1,
        data:[]
    };

    var obj2 = {
        name: periodo2,
        data: []
    };

    $.each(dataER, function (i, el) {
        obj1.data.push([el.tiempo, el.valorInicial]);
        obj2.data.push([el.tiempo, el.valorFinal]);
    });

    lista.push(obj1);
    lista.push(obj2);

    console.log(lista);

    metodo.chartJS.init(lista);
    return lista;
}


listarComparativo = function () {
    var pericodi = $("#pericodi1 option:selected").val();
    var version = $("#recacodi1 option:selected").val();
    var pericodi2 = $("#pericodi2 option:selected").val();
    var version2 = $("#recacodi2 option:selected").val();
    if (pericodi == "" || version == "") {
        alert('Por favor, seleccione el Periodo y la versión');
    } else if (pericodi2 == "" || version2 == "") {
        alert('Por favor, seleccione el Periodo y la versión');
    }
    else {
        var trnenvtipinf = $("#trnenvtipinf option:selected").val();
        var empcodi = $("#emprcodi1 option:selected").val();
        var cliemprcodi = $("#cliemprcodi1 option:selected").val();
        var barrcodi = $("#barrcodi1 option:selected").val();
        var flag = $("#flag1 option:selected").val();
        if (empcodi == '')
            empcodi = null;
        if (barrcodi == '')
            barrcodi = null;
        if (pericodi == '')
            pericodi = null;
        if (cliemprcodi == '')
            cliemprcodi = null;
        if (flag == 'T')
            flag = null;

        $.ajax({
            type: 'POST',
            url: controler + "MostrarComparativos",
            data: {
                trnenvtipinf: trnenvtipinf, pericodi: pericodi, version: version, pericodi2: pericodi2, version2: version2,
                empcodi: empcodi, cliemprcodi: cliemprcodi, barrcodi: barrcodi, flagEntrReti: flag,
                dias: "", codigos: ""
            },
            success: function (evt) {

                armarData(evt, pericodi, pericodi2);

            },
            error: function () {
                mostrarError();
            }
        });
    }
}