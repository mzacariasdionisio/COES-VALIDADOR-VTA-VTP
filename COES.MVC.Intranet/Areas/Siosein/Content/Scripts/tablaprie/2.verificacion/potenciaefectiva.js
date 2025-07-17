var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/';
var tblPotenciaEfectiva;

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnEjecutar').click(function () {
        cargarListaPotenciaEfectiva();
    });

    $('#btnValidar').click(function () {
        //validar 

        var dataTable = tblPotenciaEfectiva.rows().data();

        var data = GetDataDataTable(dataTable);

        sendListaPotenciaEfectiva(data);
    });
    
    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion


    //#region Datatable

    tblPotenciaEfectiva = $('#tblPotenciaEfectiva').DataTable({
        data: [],
        columns: [
            { data: "Osinergcodi3", width: "8%", className: "text" },   // SIOSEIN-PRIE-2021
            { data: "Emprnomb", width: "16%", className: "text" },
            { data: "Osinergcodi2", width: "8%", className: "text" },   // SIOSEIN-PRIE-2021
            { data: "Central", width: "15%", className: "text" },
            { data: "Osinergcodi", width: "8%", className: "text" },    // SIOSEIN-PRIE-2021
            { data: "GrupoNomb", width: "15%", className: "text" },
            { data: "ValorPeAct", width: "10%", className: "number" },
            { data: "ValorPeAnt", width: "10%", className: "number" },
            { data: "Variacion", width: "10%", className: "number" },
        ],
        rowCallback: function (row, data) { },
        filter: true,
        info: true,
        processing: true,
        scrollY: "400px",
        scrollCollapse: true,
        paging: false
    });

    //#endregion

    cargarListaPotenciaEfectiva();
});

function sendListaPotenciaEfectiva(data) {

    if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {

        var dataJson = {
            mesAnio: $('#txtFecha').val(),
            listaData: data
        };

        $.ajax({
            type: 'POST',
            url: controlador + 'SendListaPotenciaEfectiva',
            data: JSON.stringify(dataJson),
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            success: function (d) {

                if (d.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                    //document.location.href = controlador + 'DifusionPotenciaEfectiva?periodo=' + $('#txtFecha').val(); //SIOSEIN-PRIE-2021
                }
                else {
                    alert("Error!");
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function cargarListaPotenciaEfectiva() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaPotenciaEfectiva',
        data: { mesAnio: mesAnio },
        success: function (aData) {

            //#region Cambios SIOSEIN
            if (aData.Estado === false) {
                alert("Ha ocurrido un error en la consulta");
                return;
            }

            $.each(aData.listaPEfectiva, function (index, value) {
                value.FechaHoraInicio = moment(value.FechaHoraInicio).toISOString();
                value.FechaHoraFin = moment(value.FechaHoraFin).toISOString();
            });

            tblPotenciaEfectiva.clear().draw();
            tblPotenciaEfectiva.rows.add(aData.listaPEfectiva).draw();

            setTimeout(function () {
                $($.fn.dataTable.tables(true)).DataTable().columns.adjust().draw();
            }, 5);

            //#endregion

            //$('#hfIdEnvio').val(aData.nRegistros);
            //$('#listado1').html(aData.Resultado);
            //if (aData.nRegistros > 0) {
            //    $('#tabla_potenciaefectiva').dataTable();
            //}
            //$('#msj_listado1').show();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var dt = $('#tblPotenciaEfectiva').DataTable().rows().data();
    var dataList = {};

    // Leer sólo los campos necesarios para la exportación.
    $.each(dt, function (index, value) {
        dataList[index.toString()] = [value.Osinergcodi3, value.Emprnomb, value.Osinergcodi2,
                                       value.Central, value.Osinergcodi, value.GrupoNomb,
                                       value.ValorPeAct, value.ValorPeAnt, value.Variacion];
    });
    //

    var datosTabla = GetDataDataTable(dataList);

    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("02_PEFEC", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 1, "ancho": 50, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 3, "ancho": 50, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 5, "ancho": 50, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 15, "alinea": "right", "tipo": "string" };

    var objeto = {
        "idTabla": "#tblPotenciaEfectiva",
        "datosTabla": datosTabla,
        "titulo": "Envío Declaración Mensual Tabla 02 Potencia Efectiva",
        "nombreHoja": "TABLA N° 02",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    }

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "PEFEC";
    var tpriecodi = 2;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion