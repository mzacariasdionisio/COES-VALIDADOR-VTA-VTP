var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'
var widthLayout;

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnEjecutar').click(function () {
        CargarCostoMarginal();
    });

    $('#btnValidar').click(function () {
        sendListaDesviaciones();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    widthLayout = $('#mainLayout').width();

    cargarListaDesviaciones();
});

function sendListaDesviaciones() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaDesviaciones',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionDesviaciones?periodo=' + $('#txtFecha').val();    // SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaDesviaciones() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaDesviaciones',
        data: { mesAnio: $('#txtFecha').val() },
        success: function (aData) {
            var widthList = `${widthLayout}px`;
            $('#listado1').html(aData.Resultado).css("width", widthList);

            $('#tabla6').dataTable({
                scrollY: 500,
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

// #region SIOSEIN-PRIE-2021
function exportarExcel() {
    var dt = $("#tabla6").DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);
    var numCols = $('#tabla6').dataTable().fnSettings().aoColumns.length - 1;
    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }
    var nombreArchivo = nombreArchivoTablaPrie("06_DESV", $("#txtFecha").val());

    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 50, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 30, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 3, "ancho": 25, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 4, "ancho": 20, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 5, "ancho": 20, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 9, "ancho": 20, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 10, "ancho": 20, "alinea": "left" });
    listaColumnaAtributos.push({ "col": numCols, "omitir": "si" });
    var defaultColumnaAtributos = { "ancho": 15, "alinea": "right", "tipo": "string", "omitir": "no" };

    var listaExcelHoja = [];
    var objeto = {
        "idTabla": "#tabla6",
        "datosTabla": datosTabla,
        "titulo": "Verificacion de Desviaciones Tabla Prie 06",
        "nombreHoja": "TABLA N° 06",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));
    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}


function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "DESV";
    var tpriecodi = 6;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion
