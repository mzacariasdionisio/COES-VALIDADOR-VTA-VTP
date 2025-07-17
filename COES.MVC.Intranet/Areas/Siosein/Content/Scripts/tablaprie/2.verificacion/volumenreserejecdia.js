var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'
var widthLayout;

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        sendVolumenReserEjecDia();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    widthLayout = $("#mainLayout").width() - 30;

    cargarListaVolumenReserEjecDia();
    //cargarListaVolumenReserEjecDiaDesviacion();
    //cargarGraficoVolumenReserEjecDia();
});

function sendVolumenReserEjecDia() {
        if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
            var mesAnio = $('#txtFecha').val();

            $.ajax({
                type: 'POST',
                url: controlador + 'SendVolumenReserEjecDia',
                data: { mesAnio: mesAnio },
                dataType: 'json',
                success: function (d) {
                    if (d.IdEnvio == 1) {
                        alert("Se enviaron los Datos Correctamente!..");
                       // document.location.href = controlador + 'DifusionVolumenReserEjecDia?periodo=' + $('#txtFecha').val();
                    }
                    else alert("Error!");
                },
                error: function () {
                    alert("Ha ocurrido un error");
                }
            });
        }
}

function cargarListaVolumenReserEjecDia() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVolumenReserEjecDia',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#hfIdEnvio').val(aData.nRegistros);
            var widthList = `${widthLayout}px`;
            $('#listado1').html(aData.Resultado).css("width", widthList);
            $('#tabla19Dia').DataTable({
                rowsGroup: [0],
                info: true,
                scrollX: true,
                scrollCollapse: true,
                paging: false

            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaVolumenReserEjecDiaDesviacion() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVolumenReserEjecDiaDesviacion',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            var widthList = `${widthLayout}px`;
            $('#listado2').html(aData.Resultado).css("width", widthList);
            $('#tabla19DiaDesviacion').DataTable({
                rowsGroup: [0],
                info: true,
                scrollX: true,
                scrollCollapse: true,
                paging: false

            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function viewGrafico(id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarViewGraficoVolumenReserEjecDia',
        data: { id: id, mesAnio: $('#txtFecha').val() },
        dataType: 'json',
        success: function (data) {
            if (data.Grafico.Series.length > 0) {
                disenioGrafico(data, 'idGrafico1', 1);
            } else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
    mostrarGrafico();
}

function disenioGrafico(result, idGrafico, tip) {
    var opcion;
    if (tip == 1) {
        opcion = {
            title: {
                text: result.Grafico.TitleText,
                style: {
                    fontSize: '14'
                }
            },
            xAxis: {
                categories: result.Grafico.XAxisCategories
            },
            yAxis: { //Primary Axes
                title: {
                    text: result.Grafico.YaxixTitle
                }
            },
            tooltip: {
                headerFormat: '<b>{series.name}</b><br>',
                pointFormat: '{point.y:.3f}'
            },
            plotOptions: {
                area: {
                    fillOpacity: 0.5
                }
            },

            series: []
        };

        for (var i in result.Grafico.Series) {
            opcion.series.push({
                name: result.Grafico.Series[i].Name,
                data: result.Grafico.SeriesData[i],
                type: result.Grafico.Series[i].Type,
                //color: result.Grafico.Series[i].Color,
                yAxis: result.Grafico.Series[i].YAxis
            });
        }
    }
    $('#' + idGrafico).highcharts(opcion);
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
    var idTabla = "#tabla19Dia";
    var dt = $(idTabla).DataTable();
    var datosTabla = GetDataDataTable(dt.rows().data());
    var numCols = $(idTabla).dataTable().fnSettings().aoColumns.length;

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("19_VRES", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 25, "alinea": "center" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 15, "alinea": "left" });
    //listaColumnaAtributos.push({ "col": (numCols - 1), "omitir": "no" });
    var defaultColumnaAtributos = { "ancho": 10, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": idTabla,
        "datosTabla": datosTabla,
        "titulo": "Volumen Reservorio Ejecutada Diaria - Tabla 19",
        "nombreHoja": "TABLA N° 19",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "VRES";
    var tpriecodi = 19;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion




