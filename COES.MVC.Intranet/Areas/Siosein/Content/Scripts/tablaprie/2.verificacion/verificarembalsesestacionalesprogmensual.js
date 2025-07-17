var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    var chart;
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        sendListaEmbalsesEstaProgMensual();
    });

    $('#titulo2').hide();

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaVerificarEmbalsesEstacionalesProgMensual();
});

function sendListaEmbalsesEstaProgMensual() {
    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'sendListaEmbalsesEstacionalesProgMensual',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionEmbalsesEstacionalesProgMensual?periodo=' + $('#txtFecha').val();     // SIOSEIN-PRIE-2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaVerificarEmbalsesEstacionalesProgMensual() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaVerificarEmbalsesEstacionalesProgMensual',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            if (aData.NRegistros > 0) {
                $('#listado1').html(aData.Resultado);
                $('#listado2').html(aData.Resultado2);

                $('#EmbalProgMensual,#EmbalProgMensual2').DataTable({
                    scrollY: 500,
                    scrollCollapse: true,
                    paging: false,
                    rowsGroup: [0],
                    order: [[0, 'asc']],
                });
            } else {
                alert(aData.Mensaje);
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}


function viewGrafico(val) {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarViewGraficoVerificarEmbalsesEstacionalesProgMensual',
        data: { mesAnio: mesAnio, ptomedicodi: val },
        dataType: 'json',
        success: function (aData) {
            if (aData.NRegistros > 0) {
                GraficoLinea(aData.Grafico, 'idGrafico1');
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
    var dt = $("#EmbalProgMensual").DataTable().rows().data();
    var datosTabla = GetDataDataTable2(dt);

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }
    var nombreArchivo = nombreArchivoTablaPrie("29_POLJ", $("#txtFecha").val());

    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 15, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 15, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": "#EmbalProgMensual",
        "datosTabla": datosTabla,
        "titulo": "Verificar Embalses Estacionales Programa Mensual - Tabla 29",
        "nombreHoja": "TABLA N° 29",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));
    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);

}

function GetDataDataTable2(data) {
    var dataList = [];
    $.each(data, function (index, value) {
        value[0] = (value[0].split(";")[2]).split("<")[0];
        dataList.push(value);  
    });
    return dataList;
}


function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "POLJ";
    var tpriecodi = 29;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion