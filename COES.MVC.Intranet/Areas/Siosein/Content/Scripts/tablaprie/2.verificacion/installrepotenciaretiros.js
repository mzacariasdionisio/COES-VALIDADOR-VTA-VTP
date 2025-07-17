var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

$(function () {
    $('#txtFecha').Zebra_DatePicker({
        format: 'm Y'
    });

    $('#btnValidar').click(function () {
        validarEquipRepotRegisRetir();
    });

    // #region SIOSEIN-PRIE-2021
    $('#btnExportarExcel').on('click', function () {
        exportarExcel();
    });

    $('#btnExportarTexto').on('click', function () {
        exportarTexto();
    });
    //#endregion


    cargarListaInstallRepotenciaRetiros_Ing();
});

function validarEquipRepotRegisRetir() {
    var mesAnio = $("#txtFecha").val();

    if (confirm("Se enviará la información a tablas PRIE. ¿Desea continuar?")) {
        $.ajax({
            type: 'POST',
            url: controlador + 'validarEquipRepotRegisRetir',
            data: { mesAnio: mesAnio },
            success: function (result) {
                if (result.ResultadoInt === 1) {
                    alert("Se enviaron los Datos Correctamente!..");
                } else if (result.ResultadoInt === 2) {
                    alert("No exite datos para este periodo");
                } else {
                    alert("Ha ocurrido un error");
                }
            },
            error: function () {
                alert("Ha ocurrido un error");
            }
        })
    }
}


function cargarListaInstallRepotenciaRetiros_Ing() {
    var mesAnio = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaInstallRepotencia',
        data: { mesAnio: mesAnio },
        success: function (aData) {
            if (aData.NRegistros >= 0) {
                //ingreso
                $('#listado1').html(aData.Resultado);
                // #region SIOSEIN-PRIE-2021
                $('#tabla25Ingreso').dataTable({
                    filter: true,
                    info: true,
                    paging: false
                });
                //#endregion
                $('#msj_listado1').show();

                //retiro
                $('#listado2').html(aData.Resultado2);
                // #region SIOSEIN-PRIE-2021
                $('#tabla25Retiro').dataTable({
                    filter: true,
                    info: true,
                    paging: false
                });
                //#endregion
                $('#msj_listado2').show();
            } else {
                alert(aData.Mensaje);
            }
        },
        error: function () {
            alert("Ha ocurrido un error");
        }
    });
}

// #region SIOSEIN-PRIE-2021
function exportarExcel() {

    var dtIngreso = $('#tabla25Ingreso').DataTable().rows().data();
    var dtRetiro = $('#tabla25Retiro').DataTable().rows().data();
    var datosTablaIngreso = GetDataDataTable(dtIngreso);
    var datosTablaRetiro = GetDataDataTable(dtRetiro);

    if ((datosTablaIngreso == null || datosTablaIngreso.length < 1) &&
        (datosTablaRetiro == null || datosTablaRetiro.length < 1)) {
        alert("No existen registros para exportar");
        return;
    }

    var nombreArchivo = nombreArchivoTablaPrie("25_REPO", $("#txtFecha").val());
    var titulo = "Nuevas Instalaciones, Repotenciaciones y/o Retiros - Tabla 25";
    var listaColumnaAtributos = [];
    listaColumnaAtributos.push({ "col": 0, "ancho": 10, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 1, "ancho": 50, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 2, "ancho": 40, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 3, "ancho": 15, "alinea": "left" });
    listaColumnaAtributos.push({ "col": 4, "ancho": 40, "alinea": "left" });
    var defaultColumnaAtributos = { "ancho": 25, "alinea": "right", "tipo": "string", "omitir": "no" };


    var objeto1 = {
        "idTabla": "#tabla25Ingreso",
        "datosTabla": datosTablaIngreso,
        "titulo": titulo,
        "nombreHoja": "TABLA N° 25 INGRESO",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    }

    var objeto2 = {
        "idTabla": "#tabla25Retiro",
        "datosTabla": datosTablaRetiro,
        "titulo": titulo,
        "nombreHoja": "TABLA N° 25 RETIRO",
        "defaultColumnaAtributos": defaultColumnaAtributos,
        "listaColumnaAtributos": listaColumnaAtributos
    }

    var listaExcelHoja = [];
    listaExcelHoja.push(generarExcelHoja(objeto1));
    listaExcelHoja.push(generarExcelHoja(objeto2));

    ExportarTablasPrieExcel(listaExcelHoja, nombreArchivo);
}

function exportarTexto() {
    var periodo = $("#txtFecha").val();
    var nombreArchivo = "REPO";
    var tpriecodi = 25;

    if (periodo == null || periodo == "") {
        alert("No existe un periodo para exportar.");
    } else {
        ExportarTablasPrieTexto(tpriecodi, periodo, nombreArchivo);
    }
}
//#endregion