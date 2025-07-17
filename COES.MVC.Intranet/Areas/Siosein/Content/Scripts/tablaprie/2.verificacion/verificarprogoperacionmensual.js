var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });
    $('#btnEjecutar').click(function () {
        cargarListaVerificarProgOperacionMensual();
    });
    $('#btnValidar').click(function () {
        SendListaProgOperMensual();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    $('#btnIrCargar').click(function () {
        var periodo = $("#txtFecha").val();
        document.location.href = controlador + 'ProgOperacionMensual?periodo=' + periodo;
    });

    cargarListaVerificarProgOperacionMensual();
});

function SendListaProgOperMensual() {
    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaProgOperMensual',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) alert("Se enviaron los Datos Correctamente!..");
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaVerificarProgOperacionMensual() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVerificarProgOperacionMensual',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            if (aData.NRegistros > 0) {
                $('#ProgMensual').DataTable({
                    scrollY: 500,
                    scrollCollapse: true,
                    paging: false,
                    order: [[2, 'asc']],
                });
            } else { $('#btnIrCargar').show(); $('#btnValidar').hide(); }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function disenioGrafico(result, texto, tip) {

    var valor = [[]];
    var serieName = []; //nombre de las centrales
    var fechas = [];
    var serie1 = [];
    var serie2 = [];
    var serie3 = [];
    var titleText = "Programa de Operación Mensual (POPE)";
    var mes = "";
    for (j = 0; j < result.Categoria.length; j++) {
        serieName[j] = result.Categoria[j];
    }
    for (j = 0; j < result.Serie1.length; j++) {
        serie1[j] = result.Serie1[j];
    }
    for (j = 0; j < result.Serie2.length; j++) {
        serie2[j] = result.Serie2[j];
    }
    for (j = 0; j < result.Serie3.length; j++) {
        serie3[j] = result.Serie3[j];
    }






 /*   for (i = 0; i < result.Grafico.SerieDataS[0].length; i++) {
        valor[i] = [];
        var now = parseJsonDate(result.Grafico.SerieDataS[0][i].X);
        diaActual = now.getDate();
        diaActual = (diaActual < 9) ? '0' + diaActual : diaActual;
        mesActual = 'Oct';
        diaActual = diaActual + mesActual + now.getFullYear();
        fechas.push(diaActual);
        //fechas.push(diaActual.concat(mesActual, now.getFullYear()));
    }
    titleText = "RECECPCIÓN" + result.Grafico.TitleText;
    for (k = 0; k < result.Grafico.Series.length; k++) {

        for (z = 0; z < result.Grafico.SerieDataS[k].length; z++) {
            valor[z].push(result.Grafico.SerieDataS[k][z].Z);
        }
    }
    */
/*    var opcion = {
        chart: {
            zoomType: 'xy'
        },
        title: {
            text: titleText
        },
        xAxis: {
            categories: serieName
        },
        yAxis: [{ 
            labels: {
                format: '{value}°C',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            },
            title: {
                text: 'Temperature',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            }
        }, { 
            title: {
                text: 'Rainfall',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            labels: {
                format: '{value} mm',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            opposite: true
        }],
        tooltip: {
            shared: true
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            x: 120,
            verticalAlign: 'top',
            y: 100,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
        },       
         series: [{
            name: 'Rainfall',
            type: 'column',
            yAxis: 1,
            data: result.Serie1,
            tooltip: {
                valueSuffix: ' mm'
            }

        }, {
            name: 'Fall',
            type: 'column',
            yAxis: 1,
            data: result.Serie2,
            tooltip: {
                valueSuffix: ' mm'
            }

        }, {
            name: 'Temperature',
            type: 'spline',
            data: result.Serie3,
            tooltip: {
                valueSuffix: '°C'
            }
        }]
    };

 
    $('#idGrafico1').highcharts(opcion);*/

    /*if (tip == 1) {*/
        Highcharts.chart('idGrafico1', {
            chart: {
                zoomType: 'xy'
            },
            title: {
                text: titleText
            },
            subtitle: {
                text: ''
            },
            xAxis: [{
                categories: serieName,
                crosshair: true
            }],
            yAxis: [ { // Secondary yAxis
                title: {
                    text: '',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                opposite: true
            },
            { // Primary yAxis
                labels: {
                    format: '{value}',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: '',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                }
            }


            ],
            tooltip: {
                shared: true
            },
            legend: {
                /*layout: 'horizontal',*/
                align: 'right',
                x: -30,
                verticalAlign: 'top',
                y: 25,
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
            },
            series: [{
                name: 'ENERGIA ACTIVA INFORMADO',
                type: 'column',
                yAxis: 1,
                data: serie1,
                tooltip: {
                    valueSuffix: ' MWh'
                }

            }, {
                name: 'ENERGIA',
                type: 'column',
                yAxis: 1,
                data: serie2,
                tooltip: {
                    valueSuffix: ' MWh'
                }

            }, {
                name: 'VARIACION',
                type: 'spline',
                data: serie3,
                tooltip: {
                    valueSuffix: '%'
                }
            }]
        });
   /* }*/
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var dt = $('#ProgMensual').DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("26_POPE", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 12, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 50, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "ancho": 12, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 3, "ancho": 35, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 20, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": "#ProgMensual",
        "datosTabla": datosTabla,
        "titulo": "Verificar Programa Operacion Mensual - Tabla 26",
        "nombreHoja": "TABLA N° 26",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    }

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "POPE";
    var tpriecodi = 26;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion