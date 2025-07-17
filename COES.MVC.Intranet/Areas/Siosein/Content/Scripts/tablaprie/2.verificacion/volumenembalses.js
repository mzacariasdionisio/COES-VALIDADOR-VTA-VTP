var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'
var widthLayout;
$(function () {
    widthLayout = $('#mainLayout').width();
    
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        sendVolumenEmbalses();
    });
    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion
    cargarListaVolumenEmbalses();
});

function sendVolumenEmbalses() {
    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendVolumenEmbalses',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionVolumenEmbalses?periodo=' + $('#txtFecha').val();     // SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaVolumenEmbalses() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVolumenEmbalses',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            if (aData.NRegistros) {
                var widthList = `${widthLayout}px`;
                $('#listado1').html(aData.Resultado).css("width", widthList);
                $('#tblEmbalse').DataTable({
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
    var dt = $("#tblEmbalse").DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);
    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }
    var nombreArchivo = nombreArchivoTablaPrie("21_HEMB", $("#txtFecha").val());

    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 50, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 10, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": "#tblEmbalse",
        "datosTabla": datosTabla,
        "titulo": "Volumenes de Embalses - Tabla 21",
        "nombreHoja": "TABLA N° 21",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));
    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);

}


function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "HEMB";
    var tpriecodi = 21;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion

