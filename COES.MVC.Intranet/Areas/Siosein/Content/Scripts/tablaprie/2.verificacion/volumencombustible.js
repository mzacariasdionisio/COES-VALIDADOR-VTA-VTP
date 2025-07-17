var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'
var layoutWidth = 0;
$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });
    $('#btnValidar').click(function () {
        validarVolumenCombustible();
    });
    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion
    layoutWidth = $('#mainLayout').width();
    cargarListaVolumenCombustible();
});

function validarVolumenCombustible() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'ValidarVolumenCombustible',
            data: { mesAnio: mesAnio }, //SIOSEIN-PRIE-2021
            dataType: 'json',
            success: function (result) {
                if (result.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionVolumenCombustible?periodo=' + $('#txtFecha').val(); //SIOSEIN-PRIE-2021
                } else if (result.ResultadoInt === 2) {
                    alert("No existen registros a validar");
                }
                else { alert("Error!"); }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaVolumenCombustible() {
    var mesAnio = $('#txtFecha').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVolumenCombustible',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            // #region SIOSEIN-PRIE-2021
            if (aData.Resultado != "-1") {
                $('#listado1').html(aData.Resultado);
                $('#tabla23').DataTable({
                    filter: true,
                    info: true,
                    processing: true,
                    scrollY: "400px",
                    scrollCollapse: true,
                    paging: false,
                    order: [[7, 'asc']],
                });
            } else { alert("Error!"); }
            //#endregion
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var idTabla = "#tabla23";
    var dt = $(idTabla).DataTable();
    var datosTabla = GetDataDataTable(dt.rows().data());

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("23_VCOM", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 45, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 45, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "ancho": 45, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 3, "ancho": 15, "alinea": "center" });
    listaColumnaAtributos.push({ "col": 4, "ancho": 15, "alinea": "center" });
    listaColumnaAtributos.push({ "col": 5, "ancho": 15, "alinea": "center" });
    listaColumnaAtributos.push({ "col": 6, "ancho": 10, "alinea": "center" });
    var defaultColumnaAtributos = { "ancho": 15, "alinea": "center", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": idTabla,
        "datosTabla": datosTabla,
        "titulo": "Volumen Combustible - Tabla 23",
        "nombreHoja": "TABLA N° 23",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "VCOM";
    var tpriecodi = 23;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion