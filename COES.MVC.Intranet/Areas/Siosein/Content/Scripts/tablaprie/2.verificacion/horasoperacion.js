var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        sendListaHorasOperacion();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarHorasHoperacion();
    cargarHorasOperacionConsolidado();
});

function sendListaHorasOperacion() {
    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaHorasOperacion',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionHorasOperacion?periodo=' + $('#txtFecha').val(); //SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarHorasHoperacion() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaHorasOperacion',
        data: { mesAnio: mesAnio },
        success: function (aData) {

            if (aData.NRegistros === -1) {
                alert("Ha ocurrido un error");
                return;
            }

            // #region HOP 
            $('#listado1').html(aData.Resultado);
            if (aData.NRegistros > 0) {
                $('#tabla_horasope').dataTable({
                    searching: false,
                    scrollY: 600,
                    scrollX: true,
                    scrollCollapse: true,
                    paging: false,
                    fixedColumns: {
                        leftColumns: 1
                    },
                    columnDefs: [{
                        targets: 5,
                        render: $.fn.dataTable.render.ellipsis(120, true)
                    }]
                });
            }

            if (aData.NRegistros !== 0) {
                GraficoColumnas(aData.Grafico, "idGrafico1");
            } else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');

            // #endregion

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarHorasOperacionConsolidado() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaHorasOperacionConsolidado',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);

            if (aData.NRegistros !== 0) {
                GraficoColumnas(aData.Grafico, "idGrafico2");
            } else $('#idGrafico2').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


// #region SIOSEIN-PRIE-2021
function exportarExcel() {
    var idTabla = "#tabla_horasope";
    var dt = $(idTabla).DataTable();
    var datosTabla = GetDataDataTable(dt.rows().data());

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("15_HOPE", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 12, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 40, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "ancho": 20, "alinea": "right" });
    listaColumnaAtributos.push({ "col": 3, "ancho": 30, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 6, "ancho": 120, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 20, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": idTabla,
        "datosTabla": datosTabla,
        "titulo": "Horas de Operacion - Tabla 15",
        "nombreHoja": "TABLA N° 15",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "HOPE";
    var tpriecodi = 15;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion