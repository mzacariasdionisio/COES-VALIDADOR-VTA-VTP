var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    buscarDatos();

    $('#txtFecha').Zebra_DatePicker();

    $('#txtFecha2').Zebra_DatePicker();


    $('#btnValidar').click(function () {
        ValidarFlujoInterconexionEjec();
    });

    //#region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    $('#tab-container').easytabs({
        animate: false
    });

    $('#cbParametros').change(function () {
        buscarDatos();
    });
});
function buscarDatos() {
    $("#cbInterconexion").val($("#hfInterconexion").val());
    cargarListaFlujoInterconexionEjec();
}

//Inicio Validación Tabla 17
function ValidarFlujoInterconexionEjec() {
    var mesAnio = $('#txtFecha').val();
    var mesAnio2 = $('#txtFecha2').val();

    var parametros = $('#cbParametros').val();

    if (confirm("Se enviara la información a tablas PRIE. ¿Desea continuar?")) {

        $.ajax({
            type: 'POST',
            url: controlador + 'ValidarFlujoInterconexionEjec',
            data: { fecha1: mesAnio, fecha2: mesAnio2, param: parametros },
            success: function (result) {
                if (result.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionFlujoInterconexionEjec?periodo=' + $('#txtFecha').val();//SIOSEIN-PRIE-2021
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


//Inicio Index
function cargarListaFlujoInterconexionEjec() {
    var mesAnio = $('#txtFecha').val();
    var mesAnio2 = $('#txtFecha2').val();
    var interconexiones = $('#cbInterconexiones').val();
    var parametros = $('#cbParametros').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaFlujoInterconexionEjec',
        data: { fecha1: mesAnio, fecha2: mesAnio2, interconex: interconexiones, param: parametros },
        success: function (aData) {
            $('#hfIdEnvio').val(aData.length);
            //alert(aData.length);
            $('#listado1').html(aData);

            $('#msj_listado2').hide();
           
            if ($('#tabla th').length > 1) {
                $('#tabla').dataTable({
                    "bSort": false,
                    "scrollY": 630,
                    "scrollX": true,
                    "sDom": 't',
                    "iDisplayLength": -1
                });
            }
            cargarGraficoFlujoInterconexionEjec();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoFlujoInterconexionEjec() {
    var mesAnio = $('#txtFecha').val();
    var mesAnio2 = $('#txtFecha2').val();
    var interconexiones = $('#cbInterconexiones').val();
    var parametros = $('#cbParametros').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoFlujoInterconexionEjec',
        data: { idPtomedicion: $('#cbInterconexion').val(), fecha1: mesAnio, fecha2: mesAnio2, interconex: interconexiones, param: parametros },
        dataType: 'json',
        success: function (aData) {
            if (aData.Grafico.SerieDataS[0].length > 0) {
                graficoReporteFlujoPotenciaSt(aData);
            }
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

graficoReporteFlujoPotenciaSt = function (result) {
    var series = [];
    var series1 = [];

    for (k = 0; k < result.Grafico.SerieDataS[0].length; k++) {
        var seriePoint = [];
        var now = parseJsonDate(result.Grafico.SerieDataS[0][k].X);
        var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
        seriePoint.push(nowUTC);
        seriePoint.push(result.Grafico.SerieDataS[0][k].Y);
        series.push(seriePoint);
    }
    if (result.Grafico.SerieDataS[1])
        for (k = 0; k < result.Grafico.SerieDataS[1].length; k++) {
            var seriePoint = [];
            var now = parseJsonDate(result.Grafico.SerieDataS[1][k].X);
            var nowUTC = Date.UTC(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds());
            seriePoint.push(nowUTC);
            seriePoint.push(result.Grafico.SerieDataS[1][k].Y);
            series1.push(seriePoint);
        }

    var opcion = {
        rangeSelector: {
            selected: 1
        },

        title: {
            text: result.Grafico.titleText,
            style: {
                fontSize: '8'
            }
        },
        yAxis: [{
            title: {
                text: result.Grafico.YAxixTitle[0]
            },
            min: 0
        },
    {
        title: {
            text: result.Grafico.YAxixTitle[1]
        },
        opposite: false

    }],
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
        name: result.Grafico.Series[0].Name,
        color: result.Grafico.Series[0].Color,
        data: series,
        type: result.Grafico.Series[0].Type
    });
    if (result.Grafico.SerieDataS[1]) {
        opcion.series.push({
            name: result.Grafico.Series[1].Name,
            color: result.Grafico.Series[1].Color,
            data: series1,
            type: result.Grafico.Series[1].Type
        });
    }

    $('#graficos').highcharts('StockChart', opcion);
}
function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}
//Fin Index

//SIOSEIN-PRIE-2021
////Inicio Exportar Excel
//function exportarExcel() {
//    $.ajax({
//        type: 'POST',
//        url: controlador + "reportes/GenerarArchivoGrafFlujoPotencia",
//        data: {
//            idPtomedicion: $('#hfInterconexiones').val(),
//            tipoInterconexion: 1,
//            parametro: $('#cbParametros').val(),
//            fechaInicial: $('#txtFecha').val(),
//            fechaFinal: $('#txtFecha2').val()
//        },
//        dataType: 'json',
//        success: function (result) {
//            if (result == 1) {
//                window.location = controlador + "reportes/ExportarReporte?tipo=3";
//            }
//            if (result == -1) {
//                alert("Error en mostrar documento Excel")
//            }
//            if (result == 0) {
//                alert("No existen registros");
//            }
//        },
//        error: function () {
//            alert("Error en Grafico export a Excel");
//        }
//    });
//
//}
////fin Exportar Excel
//

//Inicio Pestania Listado
function convertirFecha(dateStr) {
    var parts = dateStr.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

//Fin Pestania Listado

// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var dt = $('#tabla').DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("17_FLUJ", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 35, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 20, "alinea": "left", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": "#tabla",
        "datosTabla": datosTabla,
        "titulo": "Flujo Interconexiones Ejecutadas - Tabla 17",
        "nombreHoja": "TABLA N° 17",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    }

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "FLUJ";
    var tpriecodi = 17;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion