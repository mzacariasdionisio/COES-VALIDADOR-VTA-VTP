var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $("#btnValidar").click(function () {
        sendProduccionEnergia();
    });

    //#region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaProduccionEnergia();
});

function sendProduccionEnergia() {

    var periodo = $('#txtFecha').val();

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        $.ajax({
            type: 'POST',
            url: controlador + "SendProduccionEnergia",
            data: { periodo: periodo },
            success: function (result) {
                if (result.ResultadoInt > 0) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionProduccionEnergia?periodo=' + $('#txtFecha').val(); //SIOSEIN-PRIE-2021                    
                } else {
                    alert("Ha ocurrido un error");
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function verDetalleGraficoDiferenciaPresentada(equicodi) {
    var periodo = $('#txtFecha').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoDiferenciasPresentadasDelPeriodo',
        data: { periodo: periodo, equicodi: equicodi },
        success: function (data) {

            disegnGrafico(data, 'idGrafico1', 'EVOLUCION DE DIFERENCIAS PRESENTADAS ENTRE FUENTE DE DATOS (TABLAS TXT vs BD SGOCOES)', 1);
            mostrarGrafico();

        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    })
}

function cargarListaProduccionEnergia() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaProduccionEnergia',
        data: { mesAnio: mesAnio },
        success: function (aData) {

            $('#listado1').html(aData.Resultado);

            $('#tabla_prodenergia').dataTable({
                filter: true,
                info: true,
                scrollY: "500px",
                scrollCollapse: true,
                paging: false,
                order: [[2, 'asc']],
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}
function disegnGrafico(data, grafico, texto, tip) {
    if (tip == 2) {

        var categorias = [];
        var series = [];

        for (var i = 0; i < data.Grafico.XAxisCategories.length; i++) {
            categorias.push(data.Grafico.XAxisCategories[i]);
        }
        for (var i = 0; i < data.Grafico.Series.length; i++) {
            var valores = [];
            for (var j = 0; j < data.Grafico.Series[i].Data.length; j++) {
                valores.push(data.Grafico.Series[i].Data[j].Y);
            }
            series.push({
                type: data.Grafico.Series[i].Type,
                name: data.Grafico.Series[i].Name,
                data: valores
            });

        }

        Highcharts.chart(grafico, {
            title: {
                text: texto
            },
            credits: {
                enabled: false
            },
            xAxis: {
                categories: categorias
            },
            series: series
        });
    }

    if (tip == 1) {
        var series = [];

        var jsonDate = data.Grafico.Series[0].Data[0].X;
        var re = /-?\d+/;
        var value = re.exec(jsonDate);
        var date = new Date(parseInt(value[0]));

        for (var i = 0; i < data.Grafico.Series.length; i++) {
            var obj = { name: data.Grafico.Series[i].Name };

            var valores = [];
            for (var j = 0; j < data.Grafico.Series[i].Data.length; j++) {
                valores.push(data.Grafico.Series[i].Data[j].Y);
            }
            var serie = { name: obj.name, data: valores };
            series.push(serie);
        }


        Highcharts.stockChart(grafico, {
            chart: {
                type: 'spline'
            },
            title: {
                text: texto
            },
            xAxis: {
                type: 'datetime',
                labels: {
                    overflow: 'justify'
                }
            },
            credits: {
                enabled: false
            },
            yAxis: {
                title: {
                    text: 'Wind speed (m/s)'
                },
            },
            tooltip: {
                valueSuffix: ' m/s'
            },
            plotOptions: {
                spline: {
                    lineWidth: 4,
                    states: {
                        hover: {
                            lineWidth: 5
                        }
                    },
                    marker: {
                        enabled: false
                    },
                    pointInterval: 900000, // 15 minutos
                    pointStart: Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), 0, 0, 0)
                }
            },
            series: series

        });
    }


}
function mostrarGrafico() {
    setTimeout(function () {
        $('#idGraficoPopup').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var dt = $('#tabla_prodenergia').DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);
    var numCols = $('#tabla_prodenergia').dataTable().fnSettings().aoColumns.length;

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("05_PROD", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 50, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 40, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 3, "ancho": 40, "alinea": "left" });
    listaColumnaAtributos.push({ "col": numCols - 1, "omitir": "si" });
    var defaultColumnaAtributos = { "ancho": 15, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": "#tabla_prodenergia",
        "datosTabla": datosTabla,
        "titulo": "Verificacion de Produccion de Energia Tabla Prie 05",
        "nombreHoja": "TABLA N° 05",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    }

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "PROD";
    var tpriecodi = 5;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion
