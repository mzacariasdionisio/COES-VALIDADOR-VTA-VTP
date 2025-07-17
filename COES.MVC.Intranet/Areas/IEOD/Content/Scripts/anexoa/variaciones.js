$(function () {

    $('#btnBuscar').click(function () {
        cargarLista();
    });

    cargarValoresIniciales();

    $('#btnConfigurarGPS').on('click', function () {
        configurarGPS();
    });

});

function cargarValoresIniciales() {
    $('#cbEmpresa').multipleSelect('checkAll');
    cargarGPS();
}

function mostrarReporteByFiltros() {
    cargarLista();
}

function cargarGPS() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGPS',
        data: {
        },
        success: function (aData) {
            $('#gps').html(aData);
            $('#cbGPS').multipleSelect({
                width: '150px',
                filter: true,
                onClose: function (view) {
                    cargarLista();
                }
            });
            $('#cbGPS').multipleSelect('checkAll');

            cargarLista();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarLista() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVariacionesSostenidasSubitas',
        data: {
            fechaInicio: getFechaInicio(),
            fechaFin: getFechaFin(),
            gps: getGPS()
        },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
            $('#listado').html(aData.Resultado);
            $('#idGraficoContainer').html('');
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function fnClickFrecuencia(x) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoVariacionesSostenidasSubitas',
        data: {
            gps: x,
            fechaInicio: getFechaInicio()
        },
        dataType: 'json',
        success: function (aData) {
            if (aData.NRegistros > 0) {
                GraficoDistribucion(aData);
                var html = `
                ${aData.Resultado} <br/>
                ${aData.Resultado2} <br/>
                ${aData.Resultado3}
                `;

                $('#idVistaGrafica2').html(html);
            }
            else { alert('Sin informacion!'); }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function GraficoDistribucion(result) {

    //generar series
    var series = [];
    for (var i = 0; i < result.Grafico.Series.length; i++) {
        var serie = result.Grafico.Series[i];

        var dataSerie = [];
        for (var j = 0; j < result.Grafico.SerieDataS[i].length; j++) {
            var auxSerie = result.Grafico.SerieDataS[i][j];
            var aux = {
                y: auxSerie.Y,
                name: auxSerie.Name
            }
            dataSerie.push(aux);
        }

        var obj = {
            name: serie.Name,
            type: serie.Type,
            yAxis: serie.YAxis,
            data: dataSerie,
            color: serie.Color,
            dataLabels: {
                enabled: false,
            }
        };

        if (i == 3) {
            obj.zIndex = -1;
            obj.showInLegend = false;
        }

        series.push(obj);
    }

    var opcion = {
        chart: {
            type: 'spline'
        },
        title: {
            text: result.Grafico.TitleText
        },
        xAxis: {
            title: {
                text: ''
            },
            categories: result.Grafico.XAxisCategories,
            labels: {
                step: 2
            }
        },
        credits: {
            enabled: false
        },
        yAxis: {
            title: { text: '' },
            min: result.Grafico.YaxixMin,
            max: result.Grafico.YaxixMax,
        },
        legend: {
        },
        plotOptions: {
        },
        series: series
    };

    $('#idVistaGrafica').highcharts(opcion);

    mostrarGrafico();
}

function mostrarGrafico() {

    setTimeout(function () {
        $('#idGrafico2').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

//GPS solo para el IEOD
function configurarGPS() {
    $.ajax({
        type: 'POST',
        url: controlador + 'ConfigurarGPS',
        success: function (evt) {
            $('#contenidoGPS').html(evt);

            //primero generar datatable
            setTimeout(function () {
                $('#tablaGPS').dataTable({
                    "destroy": "true",
                    "scrollX": true,
                    scrollY: 450,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1,
                    "language": {
                        "emptyTable": "¡No existen gps!"
                    },
                });
            }, 150);

            //luego abrir popup
            setTimeout(function () {
                $('#popupGPS').bPopup({
                    autoClose: false
                });
            }, 50);


            $('#cbSelectAll').click(function (e) {
                var table = $(e.target).closest('table');
                $('td input:checkbox', table).prop('checked', this.checked);
            });

            $('#btnGrabarGPS').on('click', function () {
                grabarConfiguracionGPS();
            });

            $('#btnCancelarGPS').on('click', function () {
                $('#popupGPS').bPopup().close();
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function grabarConfiguracionGPS() {
    var gps = "";
    countgps = 0;
    $('#tablaGPS tbody input').each(function () {
        var ind = $(this).is(':checked') ? "S" : "N";
        var item = $(this).val() + "@" + ind;
        gps = gps + item + "#";
        if ($(this).is(':checked')) {
            countgps++;
        }
    });

    if (countgps > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'GrabarConfiguracionGPS',
            data: {
                id: gps
            },
            dataType: 'json',
            success: function (result) {
                if (result == 1) {
                    $('#popupGPS').bPopup().close();
                    cargarGPS();
                }
                else {
                    alert("Ha ocurrido un error");
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
    else {
        alert("Seleccione al menos una GPS");
    }
}