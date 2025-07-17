var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });
    $('#btnEjecutar').click(function () {
        cargarListaVerificarCostosOperacionProgMensual();
    });
    $('#btnValidar').click(function () {
        sendListaCostosOperacionProgMensual();
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
        document.location.href = controlador + 'CostosOperacionProgMensual?periodo=' + periodo;
    });

    cargarListaVerificarCostosOperacionProgMensual();
});

function sendListaCostosOperacionProgMensual() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaCostosOperacionProgMensual',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionCostosOperacionProgMensual?periodo=' + $('#txtFecha').val();//SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaVerificarCostosOperacionProgMensual() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVerificarCostosOperacionProgMensual',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            if (aData.NRegistros > 0) {
                $('#listado1').html(aData.Resultado);
                // #region SIOSEIN - PRIE - 2021
                $('#tabla_costosOperacionProgramaMensual').dataTable({
                    scrollY: 200,
                    scrollX: false,
                    paging: false,
                    ordering: false
                });
                // #endregion
                GraficoCombinadoDual(aData.Grafico, "idGrafico1");
            } else {
                alert("Ha ocurrido un error");
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var idTabla = "#tabla_costosOperacionProgramaMensual";
    var dt = $(idTabla).DataTable();
    var datosTabla = GetDataDataTable(dt.rows().data());

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("28_POCV", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 20, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 40, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": idTabla,
        "datosTabla": datosTabla,
        "titulo": "Verificar Costos de Operación Programa Mensual - Tabla 28",
        "nombreHoja": "TABLA N° 28",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "POCV";
    var tpriecodi = 28;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
// #endregion