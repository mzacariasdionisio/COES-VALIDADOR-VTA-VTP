var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

var tablaCostoMarginal;

var widthLayout;
$(function () {


    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnEjecutar').click(function () {
        cargarListaCostosMarginales();
    });

    $('#btnValidar').click(function () {
        sendListaCostosMarginales();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaCostosMarginales();
});

function sendListaCostosMarginales() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        /*  var mesAnio = $('#txtFecha').val();
        */
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaCostosMarginales',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionCostosMarginales?periodo=' + $('#txtFecha').val();        //SIOSEIN-PRIE-2021
                }
                else if (d.ResultadoInt === -1) {
                    mostrarMensaje('mensaje', 'error', `Existen <b>${d.CantidadErrores}</b> costos marginales promedio diario marcados en <b>rojo</b> que exceden el limite 1.00 S/./kWh`);
                }
                else { alert("Error!"); }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaCostosMarginales() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCostosMarginales',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            widthLayout = $('#mainLayout').width();
            $('#listado1').html(aData.Resultado).css("width", `${widthLayout-20}px`);
            if (aData.Resultado !== "") {
                tablaCostoMarginal = $('#tabla_costosmarginales').dataTable({
                    filter: true,
                    info: true,
                    scrollX: true,
                    scrollY: "500px",
                    scrollCollapse: true,
                    paging: false,
                    order: [[1, 'asc']],
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
        url: controlador + 'CargarViewGraficoBarraCostoMarg',
        async: false,
        data: { mesAnio: mesAnio, barracodi: id },
        dataType: 'json',
        success: function (aData) {
            GraficoLinea(aData.Grafico, "idGrafico1");
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
    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }
    var nombreArchivo = nombreArchivoTablaPrie("03_CMAR", $("#txtFecha").val());

    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "omitir": "si" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "ancho": 25, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 10, "alinea": "right", "tipo": "string", "omitir": "no" };

    var listaExcelHoja = [];
    var objeto = {
        "idTabla": "#tabla_costosmarginales",
        "datosTabla": datosTabla,
        "titulo": "Verificacion de Costos Marginales Tabla Prie 03",
        "nombreHoja": "TABLA N° 03",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));
    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
    
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "CMAR";
    var tpriecodi = 3;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion



