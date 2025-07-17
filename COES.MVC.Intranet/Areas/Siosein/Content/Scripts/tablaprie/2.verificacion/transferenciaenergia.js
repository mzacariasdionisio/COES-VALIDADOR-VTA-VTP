var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnEjecutar').click(function () {
        cargarListaTransferenciaEnergia();
        cargarListaTransferenciaEnergiaConsolidado();
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

    cargarListaTransferenciaEnergia();
    //cargarListaTransferenciaEnergiaConsolidado();
});

function cargarListaTransferenciaEnergia() {
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaTransferenciaEnergia',
        data: { mesAnio: $('#txtFecha').val() },
        success: function (aData) {

            $('#listado1').html(aData.Resultado);
            $('#Tabla_Transferencia').DataTable({
                rowsGroup: [0, 6],
                filter: true,
                info: true,
                processing: true,
                scrollY: "500px",
                scrollCollapse: true,
                paging: false
            });
            $('#listado2').html(aData.Resultado2);


            $('#Tabla_Consolidado').dataTable();
            $('#msj_listado2').show();

        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function viewGraficoBarraTransEnerg(id) {

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarGraficoTransferenciaEnergia',
        data: { mesAnio: $('#txtFecha').val(), idEmpresa: id },
        success: function (aData) {

            if (aData.Grafico !== null) {
                GraficoColumnasBar(aData.Grafico, "idGrafico1");
                mostrarGrafico();
            }

        },
        error: function () {
            alert("Ha ocurrido un error");
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

function sendListaTransferenciaEnergia() {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {

        var mesAnio = $('#txtFecha').val();

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaTransferenciaEnergia',
            data: { mesAnio: mesAnio },
            dataType: 'json',
            success: function (d) {
                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionTransferenciaEnergia?periodo=' + $('#txtFecha').val(); //SIOSEIN - PRIE - 2021
                }
                else alert("Error!");
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var idTabla = "#Tabla_Transferencia";
    var dt = $(idTabla).DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("07_TREN", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 50, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 3, "ancho": 30, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 7, "omitir": "si" });
    var defaultColumnaAtributos = { "ancho": 15, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": idTabla,
        "datosTabla": datosTabla,
        "titulo": "Transferencia de Energias Tabla Prie 07",
        "nombreHoja": "TABLA N° 07",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "TREN";
    var tpriecodi = 7;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion