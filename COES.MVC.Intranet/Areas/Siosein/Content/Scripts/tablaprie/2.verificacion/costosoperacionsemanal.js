var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });


    $('#btnValidar').click(function () {
        SendListaCostosOperacionSemanal();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion
    
    cargarListaCostosOperacionSemanal();
});
    

function SendListaCostosOperacionSemanal() {
    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaCostosOperacionSemanal',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionCostosOperacionSemanal?periodo=' + $('#txtFecha').val(); //SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaCostosOperacionSemanal() {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCostosOperacionSemanal',
        data: { mesAnio: $('#txtFecha').val() },
        success: function (aData) {
            if (aData.NRegistros >= 1) {
                $('#listado1').html(aData.Resultado);
                // #region SIOSEIN-PRIE-2021
                $('#costosOperacionEjecutados').dataTable({
                    filter: true,
                    info: true,
                    scrollY: "500px",
                    scrollCollapse: true,
                    paging: false
                });
                //#endregion
                GraficoCombinadoDual(aData.Grafico, "idGrafico1");
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var dt = $('#costosOperacionEjecutados').DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("31_PS02", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 20, "alinea": "center" });
    var defaultColumnaAtributos = { "ancho": 20, "alinea": "right", "tipo": "string" };

    var objeto = {
        "idTabla": "#costosOperacionEjecutados",
        "datosTabla": datosTabla,
        "titulo": "Costo Operacion Semanal - Tabla Prie 31",
        "nombreHoja": "TABLA N° 31",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    }

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "PS02";
    var tpriecodi = 31;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion