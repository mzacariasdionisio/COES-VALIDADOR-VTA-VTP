var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'
var widthLayout;
$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        sendListaTransferenciaEnergia();
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

    cargarListaPagoTransferenciaEnergia();
});

function sendListaTransferenciaEnergia() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaValorTransferenciaEnergia',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionPagoTransferenciaEnergia?periodo=' + $('#txtFecha').val();    //SIOSEIN - PRIE - 2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaPagoTransferenciaEnergia() {

    var mesAnio = $('#txtFecha').val();
    var widthList = `${widthLayout - 20}px`;

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaPagoTransferenciaEnergia',
        data: { mesAnio: mesAnio },
        success: function (aData) {

            $('#listado1,#listado2').css("width", widthList);

            $('#listado1').html(aData.Resultado);
            $('#listado2').html(aData.Resultado2);

            $('#tabla10').dataTable({
                scrollY: 600,
                scrollX: true,
                scrollCollapse: true,
                paging: false,
                fixedColumns: {
                    leftColumns: 1
                }
            });

            if (aData.NRegistros > 0) {
                $('#tablaConsolidado').dataTable({
                    scrollY: 600,
                    scrollX: true,
                    scrollCollapse: true,
                    paging: false,
                    fixedColumns: {
                        leftColumns: 1
                    }
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

    var idTabla = "#tabla10";
    var dt = $(idTabla).DataTable();
    var datosTabla = GetDataDataTable(dt.rows().data());

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("10_PTRA", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 50, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 20, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": idTabla,
        "datosTabla": datosTabla,
        "titulo": "Pago Transferencia de Energia - Tabla Prie 10",
        "nombreHoja": "TABLA N° 10",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "PTRA";
    var tpriecodi = 10;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
// #endregion