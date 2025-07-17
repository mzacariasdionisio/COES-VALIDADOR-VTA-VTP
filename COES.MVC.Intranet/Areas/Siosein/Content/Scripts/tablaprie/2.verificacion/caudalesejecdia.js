var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/';
var widthLayout;
$(function () {
    widthLayout = $('#mainLayout').width();
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnEjecutar').click(function () {
        cargarListaCaudalesEjecDia();
    });

    $('#btnValidar').click(function () {
        sendCaudalesEjecutadosDiario();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaCaudalesEjecDia();
});

function sendCaudalesEjecutadosDiario() {
    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendCaudalesEjecutadosDiario',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionCaudalesEjecDia?periodo=' + $('#txtFecha').val(); //SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaCaudalesEjecDia() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCaudalesEjecDia',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            var widthList = `${widthLayout}px`;
            $('#listado1').html(aData.Resultado).css("width", widthList);
            if (aData.NRegistros) {
                $('#tabla18').DataTable({
                    scrollY: 600,
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

    var dt = $('#tabla18').DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("18_CAUD", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 12, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 40, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "ancho": 10, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 12, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": "#tabla18",
        "datosTabla": datosTabla,
        "titulo": "Caudales Ejecutados Diario Tabla 18",
        "nombreHoja": "TABLA N° 18",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    }

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "CAUD";
    var tpriecodi = 18;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion