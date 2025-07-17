var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    var chart;
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnEjecutar').click(function () {
        cargarListaCostosMarginalesSemanal();
    });

    $('#btnValidar').click(function () {
        validarCostosMarginalesSemanal();
    });
    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaCostosMarginalesSemanal();
});

function validarCostosMarginalesSemanal() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'ValidarCostosMarginalesSemanal',
            data: { periodo: mesAnio },
            dataType: 'json',
            success: function (result) {
                if (result) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionCostosMarginalesSemanal?periodo=' + $('#txtFecha').val();     // SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaCostosMarginalesSemanal() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCostosMarginalesSemanal',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            if (aData.Resultado !== "") {
                tablaCostoMarginal = $('#tabla_costosmarginales').dataTable({
                    filter: true,
                    info: true,
                    scrollX: true,
                    scrollY: "500px",
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

function viewGraficoBarraCostoMarg(id) {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarViewGraficoBarraCostoMargSemanal',
        data: { mesAnio: mesAnio, ptomedicodi: id },
        dataType: 'json',
        success: function (aData) {
            if (aData.NRegistros > 0) {
                GraficoLinea(aData.Grafico, 'idGrafico1');
                mostrarGrafico();
            } else $('#idGrafico1').html('Sin Grafico - No existen registros a mostrar!');
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
    mostrarGrafico();
}

function mostrarGrafico() {
    setTimeout(function () {
        $('#idGraficoPopup').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {
    var dt = $("#tabla_costosmarginales").DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);
    var numCols = $('#tabla_costosmarginales').dataTable().fnSettings().aoColumns.length;
    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }
    var nombreArchivo = nombreArchivoTablaPrie("32_PS03", $("#txtFecha").val());

    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 20, "alinea": "left" });
    listaColumnaAtributos.push({ "col": numCols-1 , "omitir": "si" });
    var defaultColumnaAtributos = { "ancho": 10, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": "#tabla_costosmarginales",
        "datosTabla": datosTabla,
        "titulo": "Costos Marginales Semanal - Tabla Prie 32",
        "nombreHoja": "TABLA N° 32",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));
    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}


function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "PS03";
    var tpriecodi = 32;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion

