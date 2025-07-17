var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'
var layoutWidth = 0;
$(function () {        
    layoutWidth = $('#mainLayout').width();

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnEjecutar').click(function () {
        cargarListaHechosRelevantes();
    });

    $('#btnValidar').click(function () {
        sendListaHechosRelevantes();
    });
    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaHechosRelevantes();
});

function sendListaHechosRelevantes() {
    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaHechosRelevantes',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionHechosRelevantes?periodo=' + $('#txtFecha').val();    //SIOSEIN-PRIE-2021
                }
                else { alert("Error!"); }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaHechosRelevantes() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaHechosRelevantes',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            if (aData.NRegistros > 0) {
                $('#listado1').html(aData.Resultado).css("width", `${layoutWidth}px`);
                $('#tabla24').dataTable({
                    scrollY: 500,
                    scrollX: true,
                    scrollCollapse: true,
                    paging: false,
                    order: [[1, 'asc']],
                });
            } else {
                alert(aData.Mensaje);
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


// #region SIOSEIN-PRIE-2021
function exportarExcel() {
    var dt = $("#tabla24").DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);
    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }
    var nombreArchivo = nombreArchivoTablaPrie("24_IEVE", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 30, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "ancho": 20, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 3, "ancho": 10, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 4, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 5, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 6, "ancho": 20, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 7, "ancho": 20, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 8, "ancho": 85, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 35, "alinea": "right", "tipo": "string", "omitir": "no" };

    var listaExcelHoja = [];
    var objeto = {
        "idTabla": "#tabla24",
        "datosTabla": datosTabla,
        "titulo": "Hechos Relevantes - Tabla 24",
        "nombreHoja": "TABLA N° 24",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));
    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);

}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "IEVE";
    var tpriecodi = 24;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion