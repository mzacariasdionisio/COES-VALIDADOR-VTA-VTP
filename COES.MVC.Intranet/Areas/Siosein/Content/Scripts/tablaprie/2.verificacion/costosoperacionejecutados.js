var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

var model;

const TIPO_VISUALIZACION = Object.freeze({
    GRUPO_DESP: "1",
    MODO_OPE: "2"
});

$(function () {

    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        SendListaCostosOperacionEjecutados();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    $('input:radio[name="tipoVisualizacion"]').change(function () {
        if (!$(this).is(':checked')) return;

        // SIOSEIN-PRIE-2021
        //if ($(this).val() === TIPO_VISUALIZACION.GRUPO_DESP) {
        //    $('#listado1').html(model.Resultado);
        //} else {
        //    $('#listado1').html(model.Resultado2);
        //}
        //

        $('#tblCostOperaEjecut').DataTable({
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            fixedColumns: {
                leftColumns: 1,
                rightColumns: 1
            }
        });
    });

    //SIOSEIN-PRIE-2021
    //$('#listado1').css("width", `${$('#mainLayout').width()}px`); 
    //cargarListaCostosOperacionEjecutados();
    //
    cargarListaCostosOperacionEjecutadosConsolidado();
});

function SendListaCostosOperacionEjecutados() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaCostosOperacionEjecutados',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionCostosOperacionEjecutados?periodo=' + $('#txtFecha').val(); //SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaCostosOperacionEjecutados() {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCostosOperacionEjecutados',
        data: { mesAnio: $('#txtFecha').val() },
        success: function (aData) {
            model = aData;

            var tipoVisuali = $("input[name='tipoVisualizacion']:checked").val();
            
            if (tipoVisuali === TIPO_VISUALIZACION.GRUPO_DESP) {
                $('#listado1').html(aData.Resultado);
            } else {
                $('#listado1').html(aData.Resultado2);
            }

            GraficoColumnas(aData.Grafico, "idGrafico1");

            $('#tblCostOperaEjecut').DataTable({
                scrollX: true,
                scrollCollapse: true,
                paging: false,
                fixedColumns: {
                    leftColumns: 1,
                    rightColumns: 1
                }
            });

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaCostosOperacionEjecutadosConsolidado() {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCostosOperacionEjecutadosConsolidado',
        data: { mesAnio: $('#txtFecha').val() },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);
            //SIOSEIN-PRIE-2021
            $('#costosOperacionEjecutados').DataTable({
                filter: true,
                info: true,
                processing: true,
                scrollY: "500px",
                scrollCollapse: true,
                paging: false,
                searching: false
            });
            //
            GraficoCombinadoDual(aData.Grafico, "idGrafico2");
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var idTabla = "#costosOperacionEjecutados";
    var dt = $(idTabla).DataTable();
    var datosTabla = GetDataDataTable(dt.rows().data());

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("14_COST", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 5, "alinea": "center" });
    var defaultColumnaAtributos = { "ancho": 20, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": idTabla,
        "datosTabla": datosTabla,
        "titulo": "Costos Operacion Ejecutados - Tabla Prie 14",
        "nombreHoja": "TABLA N° 14",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "COST";
    var tpriecodi = 14;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
// #endregion