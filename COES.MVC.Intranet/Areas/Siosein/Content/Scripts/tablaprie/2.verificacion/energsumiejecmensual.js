var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        validarEnergiaNoSuministrada();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaEnergSumiEjecMensual();
    cargarListaEnergSumiEjecMensualDetalle();
});

function validarEnergiaNoSuministrada() {
    var mesAnio = $('#txtFecha').val();

    if (confirm("Se enviará la información a tablas PRIE. ¿Desea continuar?")) {

        $.ajax({
            type: 'POST',
            url: controlador + 'ValidarEnergiaNoSuministrada',
            data: { mesAnio: mesAnio },
            success: function (result) {
                if (result.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionEnergSumiEjecMensual?periodo=' + $('#txtFecha').val();        //SIOSEIN-PRIE-2021
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

function cargarListaEnergSumiEjecMensual() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaEnergSumiEjecMensual',
        data: { mesAnio: mesAnio },
        success: function (aData) {

            $('#listado1').html(aData.Resultado);

            if (aData.NRegistros > 0) {
                $('#tabla16').dataTable();
                GraficoColumnas(aData.Grafico, "idGrafico1");
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaEnergSumiEjecMensualDetalle() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaEnergSumiEjecMensualDetalle',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);
            if (aData.NRegistros > 0) {
                $('#tabla162').dataTable({
                    scrollY: 400,
                    scrollX: true,
                    scrollCollapse: true,
                    paging: false
                });
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


// #region SIOSEIN-PRIE-2021
function exportarExcel() {
    var dtDetalle = $("#tabla162").DataTable().rows().data();
    var datosTablaDetalle = GetDataDataTable(dtDetalle);
    if (datosTablaDetalle == null || datosTablaDetalle.length < 1) {
        alert("No existen registros para exportar");
        return;
    }
    var nombreArchivo = nombreArchivoTablaPrie("16_ENSE", $("#txtFecha").val());

    var listaColumnaAtributos1 = [];
    listaColumnaAtributos1.push({ "col": 0, "ancho": 20, "alinea": "left" });
    listaColumnaAtributos1.push({ "col": 1, "ancho": 30, "alinea": "left" });
    listaColumnaAtributos1.push({ "col": 2, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos1.push({ "col": 3, "ancho": 20, "alinea": "left" });
    listaColumnaAtributos1.push({ "col": 4, "ancho": 75, "alinea": "left" });
    var defaultColumnaAtributos1 = { "ancho": 20, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto1 = {
        "idTabla": "#tabla162",
        "datosTabla": datosTablaDetalle,
        "titulo": "Energía no Suministrada Ejecutada Mensual - Tabla 16",
        "nombreHoja": "TABLA N° 16",
        "defaultColumnaAtributos": defaultColumnaAtributos1,
        "listaColumnaAtributos": listaColumnaAtributos1
    };
    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto1));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "ENSE";
    var tpriecodi = 16;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion
