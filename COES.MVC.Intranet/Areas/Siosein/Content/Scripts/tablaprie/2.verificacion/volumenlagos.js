var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'
var widthLayout;
$(function () {
    widthLayout = $('#mainLayout').width();
    
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        sendVolumenLagos();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion
    cargarListaVolumenLagos();
});

function sendVolumenLagos() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendVolumenLagos',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionVolumenLagos?periodo=' + $('#txtFecha').val();//SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaVolumenLagos() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVolumenLagos',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            if (aData.NRegistros) {
                var widthList = `${widthLayout}px`;
                $('#listado1').html(aData.Resultado).css("width", widthList);
                $('#tblEmbalse').DataTable({
                    scrollY: 500,
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

function viewGrafico(id) {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarViewGraficoVolumenLagos',
        data: { id: id, mesAnio: $('#txtFecha').val() },
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
    var idTabla = "#tblEmbalse";
    var dt = $(idTabla).DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);
    var numCols = $(idTabla).dataTable().fnSettings().aoColumns.length;

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("20_HLAG", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 25, "alinea": "left" });
    listaColumnaAtributos.push({ "col": (numCols - 1), "ancho": 15, "alinea": "left", "omitir": "si" });
    var defaultColumnaAtributos = { "ancho": 10, "alinea": "right", "tipo": "string", "omitir": "no" };

     var objeto = {
        "idTabla": idTabla,
        "datosTabla": datosTabla,
        "titulo": "Volumen de Lagos (HLAG) - Tabla 20",
        "nombreHoja": "TABLA N° 20",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));
    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "HLAG";
    var tpriecodi = 20;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion