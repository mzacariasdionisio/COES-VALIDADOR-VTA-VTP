var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        sendListaBalanceEmpresas();
    });
    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion
    cargarListaBalanceEmpresas();
});

function cargarListaBalanceEmpresas() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaBalanceEmpresas',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#listado1').css("width", $('#mainLayout').width() - 20 + "px");
            $('#listado1').html(aData.Resultado);
            $('#tabla_Balances').dataTable({
                scrollY: 400,
                scrollX: true,
                scrollCollapse: true,
                paging: false,
                fixedColumns: true
            });
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaCalculoPotenciaEfectivaXEmpresaXCentral() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaCalculoPotenciaEfectivaXEmpresaXCentral',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#listado2').html(aData.Resultado);
            $('#msj_listado2').show();
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function sendListaBalanceEmpresas() {
    //if ($('#hfIdEnvio').val() == 0) { alert("Debe Consultar un periodo..."); return; }        

    //if ($('#hfIdEnvio').val() > 0) {                                                          
        if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
            var mesAnio = $('#txtFecha').val();

            $.ajax({
                type: 'POST',
                url: controlador + 'SendListaBalanceEmpresas',
                data: { mesAnio: mesAnio },
                dataType: 'json',
                success: function (d) {
                    if (d.IdEnvio == 1) {
                        alert("Se enviaron los Datos Correctamente!..");
                        //document.location.href = controlador + 'DifusionBalanceEmpresas?periodo=' + $('#txtFecha').val();     //SIOSEIN-PRIE-2021
                    }
                    else alert("Error!");
                },
                error: function () {
                    alert("Ha ocurrido un error");
                }
            });
        }
    //} else { alert("Sin Informacion para enviar a COES..."); }                
}


// #region SIOSEIN-PRIE-2021
function exportarExcel() {
    var dt = $("#tabla_Balances").DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);
    console.log("ss", datosTabla);
    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }
    var nombreArchivo = nombreArchivoTablaPrie("09_COMP", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 55, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "ancho": 20, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 3, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 6, "ancho": 30, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 7, "ancho": 30, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 9, "ancho": 30, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 20, "alinea": "right", "tipo": "string", "omitir": "no" };

    var objeto = {
        "idTabla": "#tabla_Balances",
        "datosTabla": datosTabla,
        "titulo": "Balance de Empresas Tablas Prie 09",
        "nombreHoja": "TABLA N° 09",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));
    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}


function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "COMP";
    var tpriecodi = 9;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion

