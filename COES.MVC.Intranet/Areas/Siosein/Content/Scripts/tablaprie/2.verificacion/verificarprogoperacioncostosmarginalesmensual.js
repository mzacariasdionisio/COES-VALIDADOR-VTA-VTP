var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    var chart;
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        sendListaCostosMarginales();
    });
    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion
    cargarListaVerificarProgOperacionCostosMarginalesMensual();

    $('#btnIrCargar').click(function () {
        var periodo = $("#txtFecha").val();
        document.location.href = controlador + 'ProgOperacionCostosMarginalesMensual?periodo=' + periodo;
    });
});

function sendListaCostosMarginales() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaProgOperCostosMarginalesMensual',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionProgOperacionCostosMarginalesMensual?periodo=' + $('#txtFecha').val();    //SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaVerificarProgOperacionCostosMarginalesMensual() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVerificarProgOperacionCostosMarginalesMensual',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            if (aData.NRegistros > 0) {
                $('#tabla_costosmarginales_mesual').DataTable({
                    rowsGroup: [0],
                    scrollY: 500,
                    scrollCollapse: true,
                    paging: false
                });
            }
            else alert(aData.Mensaje);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function viewGrafico(val) {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoVerificarProgOperacionCostosMarginalesMensual',
        data: { mesAnio: mesAnio, ptomedicodi: val },
        dataType: 'json',
        success: function (aData) {
            if (aData.NRegistros > 0) {
                GraficoLinea(aData.Grafico, 'idGrafico1');
            } else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}


// #region SIOSEIN-PRIE-2021
function exportarExcel() {
    var dt = $("#tabla_costosmarginales_mesual").DataTable().rows().data();
    var datosTabla = GetDataDataTable2(dt);
    console.log("ss", datosTabla);
    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }
    var nombreArchivo = nombreArchivoTablaPrie("27_POCM", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 20, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 10, "alinea": "right", "tipo": "string", "omitir": "no" };

    var listaExcelHoja = [];
    var objeto = {
        "idTabla": "#tabla_costosmarginales_mesual",
        "datosTabla": datosTabla,
        "titulo": "Verificar Programa Operación Costos Marginales Mensual - Tabla 27",
        "nombreHoja": "TABLA N° 27",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));
    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}


function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "POCM";
    var tpriecodi = 27;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}

function GetDataDataTable2(data) {
    var dataList = [];
    $.each(data, function (index, value) {
        value[1] = value[1].split("<")[0];
        dataList.push(value);
    });
    return dataList;
}

//#endregion