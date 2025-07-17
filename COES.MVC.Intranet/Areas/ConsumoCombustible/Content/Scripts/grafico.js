var controlador = siteRoot + 'ConsumoCombustible/Version/';
$(function () {

    $('#desc_fecha_ini').Zebra_DatePicker({
    });

    $('#cbEmpresa').change(function () {
        cargarListaCentral();
    });

    $('#cbCentral').change(function () {
        listado();
    });

    $("#btnRegresar").click(function () {
        var fecha = $("#hfFechaPeriodo").val();
        fecha = fecha.replace("/", "-");
        fecha = fecha.replace("/", "-");

        window.location.href = siteRoot + "ConsumoCombustible/Version/Index?fechaConsulta=" + fecha;
    });

    //
    listado();
});

///////////////////////////
/// Listar filtros 
///////////////////////////

function cargarListaCentral() {
    $('#cbCentral').empty();
    $('#cbCentral').append("<option value='-1'>--SELECCIONE--</option>");

    if (getEmpresa() > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + 'ViewCargarFiltros',
            dataType: 'json',
            data: {
                vercodi: $("#hfVersion").val(),
                empresa: getEmpresa(),
            },
            cache: false,
            success: function (data) {

                if (data.ListaCentral.length > 0) {
                    $.each(data.ListaCentral, function (i, item) {
                        $('#cbCentral').get(0).options[$('#cbCentral').get(0).options.length] = new Option(item.Central, item.Equipadre);
                    });
                }

                listado();
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function getEmpresa() {
    var idEmpresa = $('#cbEmpresa').val();

    return idEmpresa;
}

function getCentral() {
    var central = $('#cbCentral').val();

    return central;
}

///////////////////////////
/// web 
///////////////////////////

function listado() {
    $('#listado').html('');

    ancho = $("#mainLayout").width() > 1100 ? $("#mainLayout").width() - 30 : 1100;
    if (getCentral() > 0) {
        $.ajax({
            type: 'POST',
            url: controlador + "ListaGrafico",
            data: {
                vercodi: $("#hfVersion").val(),
                empresa: getEmpresa(),
                central: getCentral()
            },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    var htmlGraf = '';
                    for (var i = 0; i < evt.ListaGrafico.length; i++) {
                        htmlGraf += `
                        <div style='height: 650px; float: left;'>
                            <div class="action-alert" id="mensaje_alert_${i}" style="margin-bottom: 5px; margin-top: 0px; display: none;width: 500px;"></div>

                            <div id="graficos_${i}" style="height: 400px; width: 550px; "></div>
                            <div style=" height:10px"></div>
                        </div>
                        `;

                    }

                    $('#listado').html(htmlGraf);

                    for (var i = 0; i < evt.ListaGrafico.length; i++) {
                        generarAlertaHo(evt.ListaGrafico[i].ListaNota, "mensaje_alert_" + i);

                        graficoReporte(evt.ListaGrafico[i], "graficos_" + i);
                    }
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert('Ha ocurrido un error.');
            }
        });
    }
}

function graficoReporte(grafico, idGrafico) {
    var series = [];
    var series1 = [];

    //despacho
    if (grafico.SerieDataS[0][0] != null) {
        for (k = 0; k < grafico.SerieDataS[0].length; k++) {
            var now = parseJsonDate(grafico.SerieDataS[0][k].X);
            var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());

            var p1 = {
                y: grafico.SerieDataS[0][k].Y,
                x: nowUTC,
                color: '#3498DB'
            };

            series.push(p1);
        }
    }

    //horas de operacion
    if (grafico.SerieDataS[1][0] != null) {
        for (k = 0; k < grafico.SerieDataS[1].length; k++) {
            var now = parseJsonDate(grafico.SerieDataS[1][k].X);
            var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());

            var p2 = {
                calif: grafico.SerieDataS[1][k].Type,
                y: grafico.SerieDataS[1][k].Y,
                x: nowUTC,
                color: '#DC143C'
            };

            series1.push(p2);
        }
    }

    var opcion = {
        chart: {
            zoomType: 'xy'
        },
        rangeSelector: {
            selected: 1,
            inputEnabled: false,
            buttonTheme: {
                visibility: 'hidden'
            },
            labelStyle: {
                visibility: 'hidden'
            },
        },
        title: {
            text: grafico.TitleText,

        },
        xAxis: {
            type: 'datetime'
        },
        yAxis: [{
            labels: {
                format: '{value}'
            },
            title: {
                text: grafico.YAxixTitle[0]
            },
            min: 0
        },
        {
            title: {
                text: grafico.YAxixTitle[1]
            },
            opposite: false
        }],
        tooltip: {
            //pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y:,.2f}</b> <br/>',
            formatter: function () {
                var pointDesp = this.points[0];
                var pointHo = this.points[1];
                var descNo = pointHo.point.calif != "" ? "(" + pointHo.point.calif + ")" : "";

                return Highcharts.dateFormat('%H:%M', this.x) + '<br>' +
                    Highcharts.numberFormat(pointDesp.y, 2) + ' MW' + ' <span style="color:' + pointDesp.color + '">Despacho</span>' + '<br/>' +
                    Highcharts.numberFormat(pointHo.y, 2) + ' MW' + ' <span style="color:' + pointHo.color + '">Horas de Operación ' + descNo + '</span>' + '<br/>'
                    ;
            },
            shared: true
        },
        legend: {
            layout: 'horizontal',
            align: 'center',
            verticalAlign: 'bottom',
            borderWidth: 0,
            enabled: true
        },
        series: []
    };
    opcion.series.push({
        name: grafico.Series[0].Name,
        color: grafico.Series[0].Color,
        data: series,
        type: grafico.Series[0].Type
    });
    if (grafico.SerieDataS[1]) {
        opcion.series.push({
            name: grafico.Series[1].Name,
            color: grafico.Series[1].Color,
            data: series1,
            type: grafico.Series[1].Type
        });
    }

    $('#' + idGrafico).highcharts('StockChart', opcion);
    //Highcharts.chart(idGrafico, opcion);
}

function generarAlertaHo(listaNota, idNota) {
    if (listaNota != null && listaNota.length > 0) {
        var htmlNota = 'Existen horas de operación con las siguientes calificaciones:';
        htmlNota += "<ul>";
        for (var i = 0; i < listaNota.length; i++) {
            htmlNota += `
                    <li>
                ${listaNota[i]}
                    </li>
                `;
        }
        htmlNota += "</ul>";

        $("#" + idNota).html(htmlNota);
        $("#" + idNota).show();
    }
}