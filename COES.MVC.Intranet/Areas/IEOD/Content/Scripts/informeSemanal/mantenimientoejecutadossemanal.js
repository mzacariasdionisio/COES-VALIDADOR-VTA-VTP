$(function () {
    //cargarMantenimientoEjecutadosSemanal();
    //cargargraficoMantenimientoEjecutadosSemanal(2);
    //cargarGraficoMantenimientoProgramadoSemanal();
});

function mostrarReporteByFiltros() {
    //cargarMantenimientoEjecutadosSemanal();
    //cargargraficoMantenimientoEjecutadosSemanal(2);
    //cargarGraficoMantenimientoProgramadoSemanal();
}

function cargarGraficoMantenimientoProgramadoSemanal() {
    var codigoVersion = getCodigoVersion();

    var grafic = "grafico1"; 
    //var grafic;
    //switch (tipografico) {
    //    case 1: grafic = "grafico1"; break;
    //    case 2: grafic = "grafico2"; break;
    //}
    $.ajax({
        type: 'POST',
        url: controlador + "CargarGraficoMantenimientoProgramadoSemanal",
        data: {
            codigoVersion: codigoVersion
        },
        dataType: 'json',
        success: function (result) {

            if (result.ListaGrafico[0]) {// si existen registros
                $('#' + grafic).css("display", "block");
                MantenimientoEjecutadosSemanal(result, grafic, 2);
            }
            else {// No existen registros
                $('#' + grafic).css("display", "none");
            }

            //if (data.Series.length > 0) {// si existen registros
            //    //$('#' + grafic).css("display", "block");
            //    disenioGrafico(data,'grafico1', '', 1);
            //}
            //else {// No existen registros
            //    //$('#' + grafic).css("display", "none");
            //}
        },
        error: function (err) {
            alert("Ha ocurrido un error en generar grafico");
        }
    });
}

function cargarMantenimientoEjecutadosSemanal() {
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarMantenimientoEjecutadosSemanal',
        data: { fechaInicio: fechaInicio, fechaFin: fechaFin },
        success: function (aData) {
            $('.filtro_fecha_desc').html(aData.FiltroFechaDesc);
            $('#listado').html(aData.Resultado);
            $('#mantEje').dataTable();
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargargraficoMantenimientoEjecutadosSemanal(tipografico) {
    var fechaInicio = getFechaInicio();
    var fechaFin = getFechaFin();
    var grafic;
    switch (tipografico) {
        case 1: grafic = "grafico1"; break;
        case 2: grafic = "grafico2"; break;
    }
    $.ajax({
        type: 'POST',
        url: controlador + "CargarGraficoMantenimientoEjecutadosSemanal",
        data: {
            fechaInicio: fechaInicio,
            fechaFin: fechaFin,
            param: tipografico
        },
        dataType: 'json',
        success: function (result) {
            if (result.ListaGrafico[0]) {// si existen registros
                $('#' + grafic).css("display", "block");
                MantenimientoEjecutadosSemanal(result, grafic, tipografico);
            }
            else {// No existen registros
                $('#' + grafic).css("display", "none");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error en generar grafico");
        }
    });
}

function MantenimientoEjecutadosSemanal(result, idGrafico, tipografico) {
    var opcion;
    if (tipografico == 2) {
        var json = result.ListaGrafico;
        var jsondata = [];
        for (var i in json) {
            var jsonValor = [];
            var jsonLista = json[i];
            jsondata.push({
                name: jsonLista.SerieName,
                y: jsonLista.Valor
            });
        }

        opcion = {
            chart: {
                type: 'pie'
            },
            title: {
                text: result.Titulo
            },
            credits: {
                enabled: false
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            series: [{
                colorByPoint: true,
                name: 'Total',
                data: jsondata
            }]
        };
    }

    $('#' + idGrafico).highcharts(opcion);
}

function disenioGrafico(data, grafico, titulo, tip) {
    if (tip == 1) {

    }
}