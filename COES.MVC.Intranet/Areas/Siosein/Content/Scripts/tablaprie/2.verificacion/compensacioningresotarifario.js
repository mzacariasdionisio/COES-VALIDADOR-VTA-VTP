var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        SendListaCompensacionIngresoTarifario();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaCompensacionIngresoTarifario();
});

function SendListaCompensacionIngresoTarifario() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaCompensacionIngresoTarifario',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionCompensacionIngresoTarifario?periodo=' + $('#txtFecha').val(); //SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaCompensacionIngresoTarifario() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCompensacionIngresoTarifario',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            if (aData.NRegistros !== -1) {
                $('#listado1').css("width", $('#mainLayout').width() - 20 + "px");
                $('#listado1').html(aData.Resultado);
                $('#tabla_CompIngTarif').dataTable({
                    scrollY: 600,
                    scrollX: true,
                    scrollCollapse: true,
                    paging: false
                });
            } else {
                alert(aData.Mensaje);
            }

            //cargarListaCompensacionIngresoTarifarioConsolidado(aData);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaCompensacionIngresoTarifarioConsolidado(aData) {
    $('#listado2').html(aData.Resultado);
    $('#msj_listado2').show();
}



// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var dt = $('#tabla_CompIngTarif').DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("11_CITA", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 50, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 20, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": "#tabla_CompIngTarif",
        "datosTabla": datosTabla,
        "titulo": "Compensacion de Ingreso Tarifario - Tabla Prie 11",
        "nombreHoja": "TABLA N° 11",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    }

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "CITA";
    var tpriecodi = 11;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion