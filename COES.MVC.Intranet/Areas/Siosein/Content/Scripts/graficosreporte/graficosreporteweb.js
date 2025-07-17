$(function () {    
    $('#btnPopupGraficos').click(function () {
        hojaListado();
    });

    $('#btnGuardarVisibleHoja').click(function () {
        guardarVisibleHoja();
    });
});


function GraficoLineaH(data, content) {

    var series = [];

    for (var d in data.SerieData) {
        var item = data.SerieData[d];
        if (item === null) {
            continue;
        }
        series.push({ name: item.Name, data: item.Data, type: item.Type, color: item.Color });
    }

    Highcharts.chart(content, {
        chart: {
            type: 'line',
            shadow: true
        },
        title: {
            text: data.TitleText
        },
        tooltip: {
            valueDecimals: 2,
            shared: true,
            valueSuffix: ' ' + data.YaxixLabelsFormat
        },
        xAxis: {
            categories: data.XAxisCategories,
            crosshair: true,
            title: {
                text: data.XAxisTitle
            }
        },
        yAxis: {
            title: {
                text: data.YAxixTitle
            },
            labels: {
                format: '{value} ' + data.YaxixLabelsFormat
            },
            max: data.YaxixMax
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: false
                },
                marker: {
                    enabled: true
                },
                enableMouseTracking: true
            }
        },
        subtitle: {
            text: data.Subtitle,
            align: 'left',
            verticalAlign: 'bottom',
            floating: false,
            x: 10,
            y: 9
        },
        legend: {
            layout: data.LegendLayout,
            align: data.LegendAlign,
            verticalAlign: data.LegendVerticalAlign
        },
        series: series,
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        }
    });
}

/**
 * Graficos Reporte Web Hidrología
 * */

function hojaListado() {
    $("#div_hoja_graficos").html('');
    var ireporcodi = parseInt($("#hdReportecodi").val());
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarGraficosReporte',
        dataType: 'json',
        data: {
            reporcodi: ireporcodi,
        },
        cache: false,
        success: function (data) {
            if (data.Resultado != "-1") {
                var htmlExcel = dibujarTablaListadoGraficos(data.ListaGraficosReporte);
                $("#div_hoja_graficos").html(htmlExcel);

                //primero generar datatable
                setTimeout(function () {
                    $('#tbl_hoja').dataTable({
                        "destroy": "true",
                        "scrollX": true,
                        scrollY: 300,
                        "sDom": 'ft',
                        "ordering": false,
                        "bPaginate": false,
                        "iDisplayLength": -1,
                        "language": {
                            "emptyTable": "¡No existen hojas!"
                        },
                    });
                }, 150);

                //luego abrir popup
                setTimeout(function () {
                    $('#idPopupGraficosReporte').bPopup({
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown',
                        modalClose: false
                    });
                }, 50);

            } else {
                alert("Ha ocurrido un error: " + data.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function dibujarTablaListadoGraficos(lista) {

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="tbl_hoja">
        <thead>
            <tr>
                <th style='width: 40px'>Mostrar</th>
                <th style='width: 200px'>Cód Reporte</th>
                <th style='width: 900px'>Gráfico</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        var schecked = item.IsCheck == 1 ? " checked " : "";

        cadena += `
            <tr>
                <td style="height: 24px;text-align: center; ">
                    <input type='checkbox' id='hoja_${item.Reporcodi}' name='hoja_excel' ${schecked} value='${item.Reporcodi}' />
                </td>
                <td style="height: 24px;text-align: center; ">${item.Reporcodi}</td>
                <td style="height: 24px;text-align: left; ">${item.Repornombre}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}


function guardarVisibleHoja() {
    var reporcodi = parseInt($("#hdReportecodi").val());
    $.ajax({
        type: "POST",
        url: controlador + "GuardarVisibleGraficos",
        traditional: true,
        data: {
            reporcodisVisible: _listarSeleccion(),
            ireporcodi: reporcodi,
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                alert("Se ha actualizado correctamente.");
                $('#idPopupGraficosReporte').bPopup().close();
                if (reporcodi == 317) // Inf. Mensual 4.2. Evolucion de volumenes de embalses y lagunas (Mm3)
                { mostrarVolUtilListadoGrafico(2); }
                if (reporcodi == 319) //Inf. Mensual 4.4. Evolucion mensual de los caudales (m3/s)
                { mostrarPromedioCaudales(2); }
                if (reporcodi == 335) //Inf. Anual 5.1. EVOLUCIÓN DE LOS VOLÚMENES ALMACENADOS
                { mostrarVolUtilListadoGrafico(); }
                if (reporcodi == 336) //Inf. Anual 5.2. EVOLUCIÓN PROMEDIO MENSUAL DE CAUDALES (m3/s)
                { mostrarPromedioCaudales(); }

            } else {
                alert(model.Mensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });

}

function _listarSeleccion() {
    var selected = [];
    $('input[type=checkbox][name=hoja_excel]').each(function () {
        if ($(this).is(":checked")) {
            selected.push($(this).attr('value'));
        }
    });

    return selected.join(",");
}