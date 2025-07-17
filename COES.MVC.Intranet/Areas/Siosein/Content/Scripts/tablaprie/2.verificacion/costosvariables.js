var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnEjecutar').click(function () {
        cargarListaCostosVariables();
    });

    $('#btnValidar').click(function () {
        sendListaCostosVariables();
    });

    $('#cboCombustible').change(function (e) {
        cargarGraficoCvarPorTComustible($(this).val());
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaCostosVariables();
});

function sendListaCostosVariables() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaCostosVariables',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionCostosVariables?periodo=' + $('#txtFecha').val(); //SIOSEIN - PRIE - 2021
                }
                else { alert("Error!"); }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }

}

function cargarListaCostosVariables() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCostosVariables',
        data: { mesAnio: mesAnio },
        success: function (aData) {

            $('#listado1').html(aData.Resultado);
            if (aData.NRegistros > 0) {
                $('#tabla_costovariable').dataTable({
                    filter: true,
                    info: true,
                    scrollY: "400px",
                    scrollCollapse: true,
                    paging: false
                });
            }
            GraficoLinea(aData.Grafico, "idGrafico1");

            $('#cboCombustible').find('option:not(:first)').remove();
            $.each(aData.Lista1, function (key, value) {
                $('#cboCombustible').append(new Option(value.text, value.id));
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarGraficoCvarPorTComustible(idCombustivle) {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoCostosVariablesXTComb',
        data: { mesAnio: mesAnio, tipComb: idCombustivle },
        success: function (aData) {
            GraficoColumnas(aData.Grafico, "idGrafico2");
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var idTabla = "#tabla_costovariable";
    var dt = $(idTabla).DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("04_CVAR", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 60, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 9, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 20, "alinea": "right", "tipo": "string", "omitir": "no"};

    var objeto = {
        "idTabla": idTabla,
        "datosTabla": datosTabla,
        "titulo": "Verificación de Costos Variables Tabla Prie 04",
        "nombreHoja": "TABLA N° 04",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "CVAR";
    var tpriecodi = 4;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion