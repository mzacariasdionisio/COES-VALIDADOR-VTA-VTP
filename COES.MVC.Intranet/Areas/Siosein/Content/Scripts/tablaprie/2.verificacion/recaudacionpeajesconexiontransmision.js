var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {        
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        SendListaRecaudacionPeajesConexionTransmision();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });
    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion

    cargarListaRecaudacionPeajesConexionTransmision();
});

function SendListaRecaudacionPeajesConexionTransmision() {
    //if ($('#hfIdEnvio').val() == 0) { alert("Debe Consultar un periodo..."); return; }    // SIOSEIN-PRIE-2021
    
    //if ($('#hfIdEnvio').val() > 0) {                // SIOSEIN-PRIE-2021
        if (confirm("Se enviara la informacion a tablas PRIE. ¿Desea continuar?")) {
            var mesAnio = $('#txtFecha').val();

            $.ajax({
                type: 'POST',
                url: controlador + 'SendListaRecaudacionPeajesConexionTransmision',
                data: { mesAnio: mesAnio },
                dataType: 'json',
                success: function (d) {
                    if (d.IdEnvio == 1) {
                        alert("Se enviaron los Datos Correctamente!..");
                        //document.location.href = controlador + 'DifusionRecaudacionPeajesConexionTransmision?periodo=' + $('#txtFecha').val();
                    }
                    else alert("Error!");
                },
                error: function () {
                    alert("Ha ocurrido un error");
                }
            });
        }
    //} else { alert("Sin Informacion para enviar a COES..."); }                // SIOSEIN-PRIE-2021
}

function cargarListaRecaudacionPeajesConexionTransmision() {

    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaRecaudacionPeajesConexionTransmision',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            $('#listado1').css("width", $('#mainLayout').width() - 20 + "px");
            $('#listado1').html(aData.Resultado);

            $('#tabla_peajes').dataTable({
                scrollY: 500,
                scrollCollapse: true,
                paging: false
            });

          //  cargarListaRecaudacionPeajesConexionTransmisionConsolidado(aData[1]);
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaRecaudacionPeajesConexionTransmisionConsolidado(aData) {
    $('#listado2').html(aData.Resultado);
    $('#msj_listado2').show();
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {
    var dt = $("#tabla_peajes").DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);
    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para exportar");
        return;
    }
    var nombreArchivo = nombreArchivoTablaPrie("13_RECA", $("#txtFecha").val());
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 35, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 15, "alinea": "right", "tipo": "string", "omitir": "no" };

    var listaExcelHoja = [];
    var objeto = {
        "idTabla": "#tabla_peajes",
        "datosTabla": datosTabla,
        "titulo": "Recaudación Peajes Calculados por Conexión y Transmisión - Tabla 13",
        "nombreHoja": "TABLA N° 13",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    };

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto));
    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}


function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "RECA";
    var tpriecodi = 13;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion