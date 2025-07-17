var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'
var widthLayout;
$(function () {
    widthLayout = $('#mainLayout').width();
    
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnEjecutar').click(function () {
        cargarListaHidrologiaCuencas();
    });

    $('#btnValidar').click(function () {
        sendHidrologiaCuencas();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaHidrologiaCuencas();
});

function sendHidrologiaCuencas() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendHidrologiaCuencas',
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

function cargarListaHidrologiaCuencas() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaHidrologiaCuencas',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            if (aData.NRegistros) {
                var widthList = `${widthLayout}px`;
                $('#listado1').html(aData.Resultado).css("width", widthList);
                $('#listado1 table').DataTable({
                    scrollY: 500,
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

    var idTabla = "#tabla18";
    var dt = $(idTabla).DataTable();
    var datosTabla = GetDataDataTable(dt.rows().data());

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("22_HCUE", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 40, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 10, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": idTabla,
        "datosTabla": datosTabla,
        "titulo": "Hidrología Cuencas - Tabla 22",
        "nombreHoja": "TABLA N° 22",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "HCUE";
    var tpriecodi = 22;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion