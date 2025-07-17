
var controlador = siteRoot + 'PotenciaFirme/Insumos/';

var tablaContratosCV;
var contenedorContratosCV;
var tblErroresdatos;
var listaErrorContratosCV = [];
var RECURSO_CCV = 3;
var lstEmpresas = [];
var diccionario = {};
var VIENE_DE_CONSULTA = 1;

const CALENDARIO_ESP = {
    previousMonth: 'Mes anterior',
    nextMonth: 'Mes siguiente',
    months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
    weekdays: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
    weekdaysShort: ['Dom', 'Lun', 'Mar', 'Mier', 'Jue', 'Vie', 'Sab']
}

const COL_FECHA_INI = 5;
const COL_FECHA_FIN = 6;
var fechaIni, fechaFin;
var fechaIniDesc, fechaFinDesc;

$(function () {          
   
    listadoVersion();

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodo').change(function () {
        cargarRevisiones();
    });

    $('#cbRevision').change(function () {
        mostrarTablaContratosCV();
    });

    obtenerEmpresas();
       
    $("#btnConsultar").click(function () {
        mostrarTablaContratosCV(VIENE_DE_CONSULTA);
    });

    $("#btnGuardarData").click(function () {
        GuardarDataTabla();
    });

    $("#btnEditarData").click(function () {
        activarEdicionDeLaTabla(true)
    });

    $("#btnMostrarErrores").click(function () {
        var listaErrorUnic = [];
        openPopup("erroresDatos");

        $.each(listaErrorContratosCV, function (key, value) {
            var result = listaErrorUnic.filter(x => x.address === value.address);
            if (result.length === 0) {
                listaErrorUnic.push(value);
            }
        });

        tblErroresdatos.clear().draw();
        tblErroresdatos.rows.add(listaErrorUnic).draw();
    });

    $("#btnVerHistorial").click(function () {
        openPopup("historialCCV");
    });

    $('#btnExportExcel').click(function () {
        exportarExcel();
    });

    ///////////////////////////
    /// HandsonTable
    ///////////////////////////

    // #region Handsontable contratos de compra y venta de Potencia Firme
    InicializarHansonTable();
    // #endrregion

    // #region Tabla errores
    tblErroresdatos = $('#tblListaerrores').DataTable({
        data: [],
        columns: [
            { data: "className", visible: false },
            { data: "address", width: "50" },
            { data: "valor", width: "100" },
            { data: "message", width: "180" }
        ],
        rowCallback: function (row, data) { },
        filter: false,
        info: false,
        processing: true,
        scrollCollapse: true,
        paging: false,
        autoWidth: true,
        createdRow: function (row, data, dataIndex) {
            $(row).find('td').eq(1).addClass(data.className);
        }
    });
    //#Endregion   

    mostrarTablaContratosCV();
});



function InicializarHansonTable() {
    contenedorContratosCV = document.getElementById('tblContratosCV');

    fechaIniDesc = $("#hfFechaIni").val();
    fechaFinDesc = $("#hfFechaFin").val();

    tablaContratosCV = new Handsontable(contenedorContratosCV, {
        dataSchema: {
            Pfcontnombcedente: null,
            Pfcontcedente: null,
            Pfcontnombcesionario: null,
            Pfcontcesionario: null,
            Pfcontcantidad: null,
            Pfcontvigenciaini: null,
            Pfcontvigenciafin: null,
            Pfcontobservacion: null,
            PfcontvigenciainiDesc: null,
            PfcontvigenciafinDesc: null
        },
        colHeaders: ['Compra', '', 'Vende', '', 'Cantidad (MW)', 'Inicio', 'Fin', 'Observación'],
        columns: [
            { data: 'Pfcontnombcedente', type: 'dropdown', source: lstEmpresas },
            { data: 'Pfcontcedente', type: 'numeric', },
            { data: 'Pfcontnombcesionario', type: 'dropdown', source: lstEmpresas },
            { data: 'Pfcontcesionario', type: 'numeric', },
            { data: 'Pfcontcantidad', type: 'numeric', className: 'htCenter', format: '0.00' },
            {
                data: 'Pfcontvigenciaini', type: 'date', className: 'htCenter', dateFormat: 'DD/MM/YYYY', correctFormat: true,
                defaultDate: fechaIni,
                datePickerConfig: {
                    firstDay: 1,
                    numberOfMonths: 1,
                    showWeekNumber: false,
                    minDate: fechaIni,
                    maxDate: fechaFin,
                    i18n: CALENDARIO_ESP
                }
            },
            {
                data: 'Pfcontvigenciafin', type: 'date', className: 'htCenter', dateFormat: 'DD/MM/YYYY', correctFormat: true,
                defaultDate: fechaFin,
                datePickerConfig: {
                    firstDay: 1,
                    showWeekNumber: false,
                    numberOfMonths: 1,
                    minDate: fechaIni,
                    maxDate: fechaFin,
                    i18n: CALENDARIO_ESP,
                }
            },
            { data: 'Pfcontobservacion', type: 'text' },
            { data: 'PfcontvigenciainiDesc', type: 'text' },
            { data: 'PfcontvigenciafinDesc', type: 'text' },
        ],
        colWidths: [340, 50, 340, 50, 120, 100, 100, 320, 0, 0],



        //contextMenu: true,
        //contextMenu: contexMenuConfig,
        //contextMenu: ['remove_row'],
        contextMenu: {
            items: {
                "remove_row": {
                    name: 'Click para Eliminar Fila' // Set custom text for predefined option
                }
            }
        },
        columnSorting: true,
        minSpareRows: 0,
        rowHeaders: true,
        autoWrapRow: true,
        startRows: 0,
        manualColumnResize: false,
        hiddenColumns: {
            columns: [1, 3, 8, 9],
            indicators: false
        },
        formulas: false,
        fillHandle: {
            direction: 'vertical',
            autoInsertRow: false
        }
    });

    tablaContratosCV.addHook('afterRender', function() {
        tablaContratosCV.validateCells();
    });

    tablaContratosCV.addHook('beforeValidate', function(value, row, prop, source) {
        listaErrorContratosCV = [];
    });

    tablaContratosCV.addHook('afterValidate', function(isValid, value, row, prop, source) {
        if (prop === "Pfcontcantidad") {
            var result = cantidadValidator(this, isValid, value, row, prop, 0, 1000);
            return registrarErrores(result);
        }
        if (prop === "Pfcontvigenciafin") {
            //var data = tablaContratosCV.getSourceData();
            var data2 = tablaContratosCV.getData();
            //var fecIni = data[row][5];
            var fecIni2 = data2[row][5];
            var result = fechaFinValidator(this, isValid, value, row, prop, 0, fecIni2);
            return registrarErrores(result);
        }
        if (prop === "Pfcontvigenciaini") {
            var result = celdasVaciaValidator(this, isValid, value, row, prop, 0, 0);
            return registrarErrores(result);
        }
        if (prop === "Pfcontnombcedente" || prop === "Pfcontnombcesionario") {
            var result = empresasValidator(this, isValid, value, row, prop, 0, 0);
            return registrarErrores(result);
        }

    });

    //valida que una empresa no se venda y compre a si misma
    tablaContratosCV.addHook('afterChange', function(changes, src) {

        if (changes != null) {
            $.each(changes, function(index, element) {
                var change = element;
                if (change != null && change != undefined) {
                    var rowIndex = change[0];
                    var column = change[1];
                    var oldValue = change[2];
                    var newValue = change[3];

                    if (oldValue != newValue && column == "Pfcontnombcedente") {
                        tablaContratosCV.setDataAtCell(rowIndex, tablaContratosCV.propToCol(1), diccionario[newValue]);

                    }
                    if (oldValue != newValue && column == "Pfcontnombcesionario") {
                        tablaContratosCV.setDataAtCell(rowIndex, tablaContratosCV.propToCol(3), diccionario[newValue]);

                    }
                }

            });
        }
    });

    // #endregion
    // #region configuraciones        
    document.querySelector('#btnAgregarFila').addEventListener('click', function() {
        var hot = tablaContratosCV;
        var row = hot.countRows();

        fechaIniDesc = $("#hfFechaIni").val();
        fechaFinDesc = $("#hfFechaFin").val();

        hot.alter('insert_row', row, 1);
        hot.setDataAtCell(row, COL_FECHA_INI, fechaIniDesc);
        hot.setDataAtCell(row, COL_FECHA_FIN, fechaFinDesc);
    });
}

function mostrarTablaContratosCV(origen) {
    $("#mensaje").css("display", "none");

    fechaIniDesc = $("#hfFechaIni").val();
    fechaFinDesc = $("#hfFechaFin").val();

    tablaContratosCV.updateSettings({
        columns: [
            { data: 'Pfcontnombcedente', type: 'dropdown', source: lstEmpresas },
            { data: 'Pfcontcedente', type: 'numeric', },
            { data: 'Pfcontnombcesionario', type: 'dropdown', source: lstEmpresas },
            { data: 'Pfcontcesionario', type: 'numeric', },
            { data: 'Pfcontcantidad', type: 'numeric', className: 'htCenter', format: '0.00' },
            {
                data: 'Pfcontvigenciaini', type: 'date', className: 'htCenter', dateFormat: 'DD/MM/YYYY', correctFormat: true,
                defaultDate: fechaIni,
                datePickerConfig: {
                    firstDay: 1,
                    numberOfMonths: 1,
                    showWeekNumber: false,
                    minDate: fechaIni,
                    maxDate: fechaFin,
                    i18n: CALENDARIO_ESP
                }
            },
            {
                data: 'Pfcontvigenciafin', type: 'date', className: 'htCenter', dateFormat: 'DD/MM/YYYY', correctFormat: true,
                defaultDate: fechaFin,
                datePickerConfig: {
                    firstDay: 1,
                    showWeekNumber: false,
                    numberOfMonths: 1,
                    minDate: fechaIni,
                    maxDate: fechaFin,
                    i18n: CALENDARIO_ESP,
                }
            },
            { data: 'Pfcontobservacion', type: 'text' },
            { data: 'PfcontvigenciainiDesc', type: 'text' },
            { data: 'PfcontvigenciafinDesc', type: 'text' },
        ]
    });

    cargarListaContratosCV(origen);
}

/**
 * Muestra el listado de contratos de compra y venta de Potencia Firme en la tabla handson
 */
function cargarListaContratosCV(origen) {

    origen = parseInt(origen) || 0;
    var msj = "";
    var obj = {};

    if (origen == VIENE_DE_CONSULTA) {
        obj = getObjetoJsonConsulta();
        msj = validarConsulta(obj);
    }

    ocultarBarraHerramienta();
    var revision = parseInt($('#cbRevision').val()) || 0;
    if (revision <= 0)
        msj = "Debe seleccionar Recálculo de Potencia Firme.";

    if (msj == "") {
        $("#contenidoContratosCV").hide();

        $.ajax({
            url: controlador + "CargarLstContratosCV",
            data: {
                revision: revision,
                verscodi: -2
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    //mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de contratos de compra y venta de Potencia Firme.');
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de contratos de compra y venta de Potencia Firme. Error: ' + result.Mensaje);
                } else {
                    activarEdicionDeLaTabla(!result.TieneRegistroPrevio);
                    listadoVersion();
                    cargarTablaHandson(result);
                    $('#hversion').val(result.Version);

                    /*para la nota de version mostrada*/
                    if (result.NumVersion != null) {
                        $('#versnumero').text("Versión: " + result.NumVersion);
                    } else {
                        $('#versnumero').text("");
                    }
                }
            },
            error: function (xhr, status) {
                //mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de contratos de compra y venta de Potencia Firme.');
            }
        });
    } else {
        alert(msj);
    }
}

/**
 * Muestra el listado de contratos de compra y venta de Potencia Firmeen la tabla handson de cierta versión
 */
function verPorVersion(verscodi) {
    $("#contenidoContratosCV").hide();

    var revision = parseInt($('#cbRevision').val()) || 0;

    $.ajax({
        url: controlador + "CargarLstContratosCV",
        data: {
            revision: revision,
            verscodi: verscodi
        },
        type: 'POST',
        success: function (result) {
            if (result.Resultado === "-1") {
                //mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de contratos de compra y venta de Potencia Firme por versión.');
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de contratos de compra y venta de Potencia Firme por versión. Error: ' + result.Mensaje);
            } else {
                $('#historialCCV').bPopup().close();
                activarEdicionDeLaTabla(false);
                cargarTablaHandson(result);
                $('#hversion').val(verscodi);
                if (result.NumVersion != null) {
                    $('#versnumero').text("Versión: " + result.NumVersion);
                } else {
                    $('#versnumero').text("");
                }
            }
        },
        error: function (xhr, status) {
            //mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de contratos de compra y venta de Potencia Firme por versión.');
        }
    });
}


function obtenerEmpresas() {
    //flagMostrarExitoEnvio = parseInt(flagMostrarExitoEnvio) || 0;

    //$("#contenidoContratosCV").hide();

    $.ajax({
        url: controlador + "CargarLstEmpresasCV",
        data: { },
        type: 'POST',
        success: function (result) {
            if (result.Resultado == "1") {                                         
                var empresas = result.ListaEmpresasTotales;
                
                for (var index in empresas) {
                    var item = empresas[index];

                    //llenamos lista empresas
                    lstEmpresas.push(item.Emprnomb);                    

                    //llenamos el diccionario
                    diccionario[item.Emprnomb] = item.Emprcodi;
                }                               
            } else {
                //mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar las empresas');  
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar las empresas. Error: ' + result.Mensaje);  
            }
        },
        error: function (xhr, status) {
            //mostrarMensaje('mensaje', 'error', 'Se ha producido un error');
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar las empresas.'); 
        }
    });
}

/**
 * Exporta la información de la tabla handson a un archivo excel
 * */
function exportarExcel() {
    var dataHandson, dataValido = [];
    dataHandson = tablaContratosCV.getSourceData();
    dataValido = obtenerDataCCVValido(dataHandson, true);
    var datos = {
        listaCCV: dataValido
    };

    $.ajax({
        url: controlador + "DescargarFormatoCCV",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(datos),
        success: function (result) {
            switch (result.NRegistros) {
                case 1: window.location = controlador + "Exportar?fi=" + result.Resultado; break;// si hay elementos
                case 2: mostrarMensaje('mensaje', 'error', 'No existen registros !'); break;// sino hay elementos
                case -1: mostrarMensaje('mensaje', 'error', 'Error en reporte result'); break;// Error en C#
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al momento de exportar');

        }
    });
}

/**
 * Cambia de estado (A:APROBADO) a cierta versión, las demas pasan a estado (G:GENERADO)
 */
function aprobarVersion(verscodi, recursocodi, recacodi) {
    if (confirm('¿Desea aprobar la versión escogida?')) {
        $.ajax({
            url: controlador + "AprobarVersionInsumo",
            data: {
                verscodi: verscodi,
                recursocodi: recursocodi,
                recacodi: recacodi
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error al aprobar una versión');
                } else {
                    listadoVersion();

                    mostrarMensaje('mensaje', 'message', 'Cambios realizados con éxito.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function ocultarBarraHerramienta() {
    $("#versionInsumo").hide();
    $("#btnEditarData").hide();
    $("#btnGuardarData").hide();
    $("#btnImportar").hide();
    $("#btnCargarBD").hide();
    $("#btnMostrarErrores").hide();
    $("#versionInsumo").hide();
    $("#btnExportExcel").hide();
    $("#btnVerHistorial").hide();
}

/**
 * Función que verifica cual de los dos botones (Guardar o Editar) debe mostrarse
 */
function activarEdicionDeLaTabla(esEditable) {
    $("#btnExportExcel").show();
    $("#btnVerHistorial").show();

    if (esEditable) {
        $("#versionInsumo").hide();
        $("#btnEditarData").hide();
        $("#btnGuardarData").show();
        $("#btnMostrarErrores").show(); 
        $("#btnAgregarFila").show(); 
        
        habilitarModoEdicion(tablaContratosCV);

    } else {
        $("#btnEditarData").show();
        $("#btnGuardarData").hide();
        $("#btnMostrarErrores").hide();
        $("#btnAgregarFila").hide();
        habilitarModoSoloLectura(tablaContratosCV);
        $("#versionInsumo").show();
    }
}

function habilitarModoSoloLectura(hanson) {
    hanson.updateSettings({
        readOnly: true
    });
}

function habilitarModoEdicion(hanson) {
    hanson.updateSettings({
        readOnly: false
    });
}

/**
 * Coloca la informacion a la tabla handson
 */
function cargarTablaHandson(result) {
    hansonTableClear();
    $("#contenidoContratosCV").show();
    RellenarTablaHandson(tablaContratosCV, result.ListaContratosCV);
    //updateDimensionHandson(tablaContratosCV, contenedorContratosCV);                   
}

/**
 * Limpia la tabla handson
 * */
function hansonTableClear() {

    tablaContratosCV.loadData([]);
}

/**
 * Rellena y muestra la información en la tabla handson
 */
function RellenarTablaHandson(tablaExcelWeb, inputJson) {
    var lstContratosCV = inputJson && inputJson.length > 0 ? inputJson : [];

    var lstData = [];

    tablaExcelWeb.loadData(lstData);

    for (var index in lstContratosCV) {
        var item = lstContratosCV[index];

        var data = {

            Pfcontnombcedente: item.Pfcontnombcedente,
            Pfcontcedente: item.Pfcontcedente,
            Pfcontnombcesionario: item.Pfcontnombcesionario,
            Pfcontcesionario: item.Pfcontcesionario,
            Pfcontcantidad: item.Pfcontcantidad,
            Pfcontvigenciaini: item.PfcontvigenciainiDesc,
            Pfcontvigenciafin: item.PfcontvigenciafinDesc,
            Pfcontobservacion: item.Pfcontobservacion,
            PfcontvigenciainiDesc: item.PfcontvigenciainiDesc,
            PfcontvigenciafinDesc: item.PfcontvigenciafinDesc,
        };

        lstData.push(data);
    }

    //if (lstContratosCV.length <= 0) {
    //    lstData.push(
    //        {
    //            Pfcontvigenciaini: fechaIniDesc,
    //            Pfcontvigenciafin: fechaFinDesc,
    //            PfcontvigenciainiDesc: fechaIniDesc,
    //            PfcontvigenciafinDesc: fechaFinDesc,
    //        }
    //    );
    //}

    tablaExcelWeb.loadData(lstData);
}

/**
 * Abre los popUps existentes en la vista principal
 */
function openPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

/**
 * Llena la lista de errores
 */
function registrarErrores(result) {
    if (!result.valid)
        listaErrorContratosCV.push(result.error);
    return result.valid;
}

/**
 * Valida que los valores sean decimales 
 */
numeroDecimalValidator = function (value, callback) {
    var isValid = true;
    if ($.isNumeric(value) && isValid) {

    } else if (value && !$.isNumeric(value)) {
        if (value != undefined && value != null && value != '') {
            isValid = false;
        }
    }
    callback(isValid);
};


/**
 * Colorea de azul las celdas Empresa, central y unidad
 */
//function repintadoRenderer(instance, td, row, col, prop, value, cellProperties) {
//    Handsontable.renderers.TextRenderer.apply(this, arguments);
//    $(td).addClass("celdaAzul");
//}


/**
 * Valida las celdas vacias 
 */
function celdasVaciaValidator(instance, isValid, value2, row, prop, valMin, valMax) {
    var error = [];
    var columnName = instance.getColHeader(instance.propToCol(prop));
    var className, mensaje;

    value = value2;

    
    //Valida celda vacia
    if (value == null || value == "") {
        className = "errorCeldaVacia";
        mensaje = "Celda vacía.";
        isValid = false;
    }

    error = {
        address: `[<b>${columnName}</b>] [<b>${row + 1}</b>]`,
        valor: value,
        className: className,
        message: mensaje
    };
    instance.setCellMeta(row, instance.propToCol(prop), "invalidCellClassName", className);   

    
    return { valid: isValid, error: error };
}

/**
 * Válida campos vacios y que una empresa no debe comprar y vender a si misma
 */
function empresasValidator(instance, isValid, value2, row, prop, valMin, valMax) {
    isValid = true;

    var error = [];
    var columnName = instance.getColHeader(instance.propToCol(prop));
    var className, mensaje;

    value = value2;


    //Valida celda vacia
    if (value == null || value == "") {
        className = "errorCeldaVacia";
        mensaje = "Celda vacía.";
        isValid = false;
    } else {
        if (prop == "Pfcontnombcedente") {
            var data2 = tablaContratosCV.getData(); 
            var nombreCedente = value;
            var nombreCesionario = data2[row][2];

            if (nombreCedente == nombreCesionario) {
                className = "htInvalid";
                mensaje = "La empresa vende Potencia Firme a si misma";
                isValid = false;
            }

        } else {
            if (prop == "Pfcontnombcesionario")  {
                var data2 = tablaContratosCV.getData();
                var nombreCesionario = value;
                var nombreCedente = data2[row][0];

                if (nombreCedente == nombreCesionario) {
                    className = "htInvalid";
                    mensaje = "La empresa compra Potencia Firme a si misma";
                    isValid = false;
                }
            }
        }
    }

    error = {
        address: `[<b>${columnName}</b>] [<b>${row + 1}</b>]`,
        valor: value,
        className: className,
        message: mensaje
    };
    instance.setCellMeta(row, instance.propToCol(prop), "invalidCellClassName", className);


    return { valid: isValid, error: error };
}

/**
 * Valida que la fecha fin sea mayor a fecha inicio
 */
function fechaFinValidator(instance, isValid, value2, row, prop, valMin, valMax) {
    isValid = true;
    var error = [];
    var columnName = instance.getColHeader(instance.propToCol(prop));
    var className, mensaje;

    value = value2;

    //Valida celda vacia
    if (value == null || value == "") {
        className = "errorCeldaVacia";
        mensaje = "Celda vacía.";
        isValid = false;
    }

    error = {
        address: `[<b>${columnName}</b>] [<b>${row + 1}</b>]`,
        valor: value,
        className: className,
        message: mensaje
    };
    instance.setCellMeta(row, instance.propToCol(prop), "invalidCellClassName", className);

    //valida otros tipos
    if (valMax != undefined && valMax != null) {
        if (value < valMax) {
            className = "errorLimitInferior";
            mensaje = "La fecha Final es anterior a la fecha Inicial.";
            isValid = false;
        }

        error = {
            address: `[<b>${columnName}</b>] [<b>${row + 1}</b>]`,
            valor: value,
            className: className,
            message: mensaje
        };
        instance.setCellMeta(row, instance.propToCol(prop), "invalidCellClassName", className);

        
    }
    return { valid: isValid, error: error };
}
/**
 * Metodo que valida la cantidad de potencia
 */
function cantidadValidator(instance, isValid, value2, row, prop, valMin, valMax) {
    isValid = true;

    var error = [];
    var columnName = instance.getColHeader(instance.propToCol(prop));
    var className, mensaje;

    value = value2;
    if (value2 != undefined && value2 != null) {
        //value = value + '';
    }

    //Valida celda vacia
    if (value == null || value == "") {
        className = "errorCeldaVacia";
        mensaje = "Celda vacía.";
        isValid = false;
    }

    error = {
        address: `[<b>${columnName}</b>] [<b>${row + 1}</b>]`,
        valor: value,
        className: className,
        message: mensaje
    };
    instance.setCellMeta(row, instance.propToCol(prop), "invalidCellClassName", className);

    //valida otros tipos
    if ($.isNumeric(value) && isValid) {

        if (value < valMin) {
            className = "errorLimitInferior";
            mensaje = "El dato es menor que el límite inferior.";
            isValid = false;
        }

        if (value >= valMax) {
            className = "errorLimitSuperior";
            mensaje = "El dato es mayor que el límite superior.";
            isValid = false;
        }
        

        
        instance.setCellMeta(row, instance.propToCol(prop), "invalidCellClassName", className);

        error = {
            address: `[<b>${columnName}</b>] [<b>${row + 1}</b>]`,
            valor: value,
            className: className,
            message: mensaje
        };
        instance.setCellMeta(row, instance.propToCol(prop), "invalidCellClassName", className);

    } else if (value && !$.isNumeric(value)) {
        if (value != undefined && value != null && value != '') {
            isValid = false;

            className = "htInvalid";
            mensaje = "El dato no es numérico";

            ////quitar <br>
            //var regex = /<br\s*[\/]?>/gi;
            //columnName = columnName != null ? columnName.replace(regex, "") : "";

            error = {
                address: `[<b>${columnName}</b>] [<b>${row + 1}</b>]`,
                valor: value,
                className: className,
                message: mensaje
            };
            instance.setCellMeta(row, instance.propToCol(prop), "invalidCellClassName", className);
        }
    }
    return { valid: isValid, error: error };
}

/**
 * Guarda los datos proporcionados en la tabla handson
 */
function GuardarDataTabla() {
    if (listaErrorContratosCV.length === 0) {
        var dataHandson, dataValido = [];
        dataHandson = tablaContratosCV.getSourceData();
        dataValido = obtenerDataCCVValido(dataHandson, true);

        var dataJson = {
            recacodi: $('#cbRevision').val(),
            verscodi: $('#hversion').val(),
            lstContratosCV: dataValido
        };

        $.ajax({
            url: controlador + "GuardarListadoContratosCV",

            data: JSON.stringify(dataJson),
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error al momento de guardar la información.');
                } else {
                    if (result.Resultado === "1") {
                        mostrarMensaje('mensaje', 'message', 'Los Contratos fueron guardados exitosamente.');
                        listaErrorContratosCV = [];
                        mostrarTablaContratosCV();
                        activarEdicionDeLaTabla(false);
                    }
                }
            },
            error: function (xhr, status) {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
    else {
        mostrarMensaje('mensaje', 'error', 'Existen errores en las celdas, favor de corregir y vuelva a envíar.');
        $("#btnMostrarErrores").trigger("click");
    }
};

/**
 * Obtiene el listado de los registros del handsontable
 */
function obtenerDataCCVValido(dataHandson, soloFilaValida) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

        item.PfcontvigenciainiDesc = item.Pfcontvigenciaini.toString();
        item.PfcontvigenciafinDesc = item.Pfcontvigenciafin.toString();
        //Si se desea filtrar data por alguna condición, agregar aqui
        lstData.push(item);
    }
    return lstData;
}


function obtenerFechaMinSegunPeriodo() {
    return moment().subtract(1, 'months').startOf('month').toDate();
}

function obtenerFechaMaxSegunPeriodo() {
    return moment().subtract(1, 'months').endOf('month').toDate();
}

/**
 * Rellena el listado de revisiones por cada periodo
 * */
function listadoPeriodo() {

    var anio = parseInt($("#cbAnio").val()) || 0;

    $("#cbPeriodo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Pfperinombre, item.Pfpericodi);
                    });
                } else {
                    $('#cbPeriodo').get(0).options[0] = new Option("--", "0");
                }

                cargarRevisiones();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
};

function cargarRevisiones() {

    var pfpericodi = parseInt($("#cbPeriodo").val()) || 0;

    $("#cbRevision").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "CargarRevisiones",
        data: {
            pfpericodi: pfpericodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#hfFechaIni").val(evt.FechaIni);
                $("#hfFechaFin").val(evt.FechaFin);

                if (evt.ListaRevisiones.length > 0) {
                    $.each(evt.ListaRevisiones, function (i, item) {
                        $('#cbRevision').get(0).options[$('#cbRevision').get(0).options.length] = new Option(item.Pfrecanombre, item.Pfrecacodi);
                    });
                } else {
                    $('#cbRevision').get(0).options[0] = new Option("--", "0");
                }

                mostrarTablaContratosCV();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}


/**
 * Lista en un popup las versiones existentes para el insumo segun tipo de insumo y revisión
 * */
function listadoVersion() {

    $('#listadoHCCV').html('');

    var recurso = RECURSO_CCV;     //CAMBIAR SEGUN INSUMO LA TABLA RECURSO 
    var recacodi = parseInt($("#cbRevision").val()) || 0;

    $.ajax({
        type: 'POST',
        url: controlador + "VersionListado",
        data: {
            recurso: recurso,
            recacodi: recacodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listadoHCCV').html(evt.Resultado);

                $("#listadoHCCV").css("width", (820) + "px");

                $('.tabla_version_x_recalculo').dataTable({
                    "sPaginationType": "full_numbers",
                    "destroy": "true",
                    "ordering": false,
                    "searching": true,
                    "iDisplayLength": -1,
                    "info": false,
                    "paging": false,
                    "scrollX": false,
                    "scrollY": "250px",
                });
            } else {
                //mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error al listar las versiones.');
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error al listar las versiones. Error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            //mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};



/**
 * Validar que existan todos los parametros al momento de hacer la consulta
 */
function validarConsulta(obj) {
    var msj = "";

    return msj;
}

/**
 * Parametros para consulta
 * */
function getObjetoJsonConsulta() {
    var obj = {};

    obj.consulta_revi = $("#cbRevision").val() || 0;

    return obj;
}

/**
 * Muestra mensajes de notificación
 */
function mostrarMensaje(id, tipo, mensaje) {
    //$('#' + id).removeClass();
    //$('#' + id).addClass('action-' + tipo);
    //$('#' + id).html(mensaje);

    alert(mensaje);
};

function mostrarMensaje_(id, tipo, mensaje) {
    $("#mensaje").css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};
