var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnEjecutar').click(function () {
        cargarListaProgOperacionDiario();
    });

    $('#btnValidar').click(function () {
        validarProgramaOperacionDiario();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaProgOperacionDiario();
});

function cargarListaProgOperacionDiario() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaProgOperacionDiario',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#listado1').html(aData.Resultado);
            $('#listado3').html(aData.Resultado2);
            if (aData.NRegistros > 0) {
                $('#tabla_progDiaria').dataTable({
                    scrollY: 500,
                    scrollX: false,
                    paging: false,
                    order: [[1, 'asc']],
                });
                GraficoCombinadoDual(aData.Grafico, "idGrafico2");
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarGraficoPorCentral(emprecodi, equipadre) {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaProgOperacionDiarioPorCentral',
        data: { periodo: mesAnio, emprcodi: emprecodi, equipadre: equipadre },
        dataType: 'json',
        success: function (data) {

            GraficoCombinadoDual(data.Grafico, "idGrafico1");
            mostrarGrafico();
        },
        error: function () {
            alert('Ha ocurrido un error');
        }
    });
}

function validarProgramaOperacionDiario() {

    var periodo = $('#txtFecha').val();

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        $.ajax({
            type: 'POST',
            url: controlador + "ValidarProgramaOperacionDiario",
            data: { periodo: periodo },
            success: function (result) {
                if (result.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionProgOperacionDiario?periodo=' + $('#txtFecha').val(); //SIOSEIN - PRIE - 2021
                } else {
                    alert("Ha ocurrido un error");
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
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

    var idTabla = "#tabla_progDiaria";
    var dt = $(idTabla).DataTable();
    var datosTabla = GetDataDataTable(dt.rows().data());

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("33_PD01", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 40, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "ancho": 40, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 8, "omitir": "si", });
    var defaultColumnaAtributos = { "ancho": 20, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": idTabla,
        "datosTabla": datosTabla,
        "titulo": "Programa de Operación Diario - Tabla 33",
        "nombreHoja": "TABLA N° 33",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "PD01";
    var tpriecodi = 33;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion

