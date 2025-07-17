var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        sendListaTransferenciaPotencia();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaPagoTransferenciaPotencia();
});

function sendListaTransferenciaPotencia() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaTransferenciaPotencia',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionPagoTransferenciaPotencia?periodo=' + $('#txtFecha').val(); //SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaPagoTransferenciaPotencia() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaPagoTransferenciaPotencia',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            var ancho = $('#mainLayout').width() - 20;
            $('#listado1').css("width", ancho  + "px");
            $('#listado1').html(aData.Resultado);
            $('#tabla08').dataTable({
                scrollY: 600,
                scrollX: true,
                scrollCollapse: true,
                paging: false
            });

            if (aData.NRegistros > 0) {
                $('#listado2').css("width", ancho + "px");
                $('#listado2').html(aData.Resultado2);
                $('#tabla08_nuevo').dataTable({
                    scrollY: 600,
                    scrollX: true,
                    scrollCollapse: true,
                    paging: false
                });
            }
            $('#msj_listado2').show();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var dt = $('#tabla08').DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("08_TRPP", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 50, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 30, "alinea": "right", "tipo": "string" };

    var objeto = {
        "idTabla": "#listado1 #tabla08",
        "datosTabla": datosTabla,
        "titulo": "Pago Transferencia de Potencia - Tabla Prie 08",
        "nombreHoja": "TABLA N° 08",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    }

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "TRPP";
    var tpriecodi = 8;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion

