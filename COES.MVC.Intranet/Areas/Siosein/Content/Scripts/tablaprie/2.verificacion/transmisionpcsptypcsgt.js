var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'
var widthLayout;
$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        SendListaTransmisionPCSPTyPCSGT();
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
    cargarListaTransmisionPCSPTyPCSGT();
});

function SendListaTransmisionPCSPTyPCSGT() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaTransmisionPCSPTyPCSGT',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt == 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionTransmisionPCSPTyPCSGT?periodo=' + $('#txtFecha').val(); //SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }

}

function cargarListaTransmisionPCSPTyPCSGT() {

    var mesAnio = $('#txtFecha').val();
    var widthTable = widthLayout - 20;
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaTransmisionPCSPTyPCSGT',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            if (aData.NRegistros === -1) {
                alert(aData.Mensaje);
                return;
            }

            //Compensación por transmisión PCSPT y PCSGT
            $('#lstCompensacion').css("width", `${widthTable}px`);
            $('#lstCompensacion').html(aData.Resultado);
            $('#tabla_TransSPT').dataTable({
                scrollY: 600,
                scrollX: true,
                scrollCollapse: true,
                paging: false
            });

            //reporte consolidado
            //$('#lstConsolidado').html(aData.Resultado2);
            //$('#msj_listado2').show();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var dt = $('#tabla_TransSPT').DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("12_PEAJ", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 12, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 50, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 15, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": "#tabla_TransSPT",
        "datosTabla": datosTabla,
        "titulo": "Compensación Transmisión PCSPT y PCSGT - Tabla 12",
        "nombreHoja": "TABLA N° 12",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    }

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "PEAJ";
    var tpriecodi = 12;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion