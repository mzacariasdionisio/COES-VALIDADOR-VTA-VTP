var controlador = siteRoot + 'PotenciaFirme/Reporte/';

var tablaPotenciaFirme;
var contenedorPotenciaFirme;
var tblErroresdatos;
var listaErrorPF = [];
var misCabeceras = [];
var miDataColumnas = [];
var misTamColumnas = [];


$(function () {

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodo').change(function () {
        cargarRevisiones();
    });

    $('#cbRevision').change(function () {
        mostrarTablaPotenciaFirme();
    });

    $("#btnConsultar").click(function () {
        mostrarTablaPotenciaFirme();
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

        $.each(listaErrorPF, function (key, value) {
            var result = listaErrorUnic.filter(x => x.address === value.address);
            if (result.length === 0) {
                listaErrorUnic.push(value);
            }
        });

        tblErroresdatos.clear().draw();
        tblErroresdatos.rows.add(listaErrorUnic).draw();
    });

    $("#btnVerHistorial").click(function () {
        openPopup("historialPF");
    });

    $('#btnExportExcel').click(function () {
        exportarExcel();
    });

    ///////////////////////////
    /// HandsonTable
    ///////////////////////////

    // #region Handsontable Factor de Presencia
    contenedorPotenciaFirme = document.getElementById('tblPotenciaFirme');


    // #endregion


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

    mostrarTablaPotenciaFirme();

});





function armarEstructuraTablaWeb(lstEscenarios) {

    numEscenarios = lstEscenarios.length;
    misCabeceras = ['Empresa', 'Central', 'Unidad', 'Potencia<br> Efectiva', 'Factor de<br> Indisponibilidad', 'Potencia<br> Garantizada', 'Factor<br> Presencia'];
    misTamColumnas = [320, 300, 220, 80, 100, 90, 90];
    miDataColumnas = [
        { data: 'Emprnomb', readOnly: true, editor: false, renderer: repintadoRenderer },
        { data: 'Central', readOnly: true, editor: false, renderer: repintadoRenderer },
        { data: 'Equinomb', readOnly: true, editor: false, renderer: repintadoRenderer },
        { data: 'Pftotpe', readOnly: true, editor: false, type: 'numeric', className: 'htCenter', validator: numeroDecimalValidator, allowInvalid: true, format: '0.00' },
        { data: 'Pftotfi', readOnly: true, editor: false, type: 'numeric', className: 'htCenter', validator: numeroDecimalValidator, allowInvalid: true, format: '0.00%' },
        { data: 'Pftotpg', readOnly: true, editor: false, type: 'numeric', className: 'htCenter', validator: numeroDecimalValidator, allowInvalid: true, format: '0.00' },
        { data: 'Pftotfp', readOnly: true, editor: false, type: 'numeric', className: 'htCenter', validator: numeroDecimalValidator, allowInvalid: true, format: '0.00' },


    ];

    for (var i = 0; i < numEscenarios; i++) {
        var escenario = lstEscenarios[i];
        this.misCabeceras.push("Potencia Firme<br>" + escenario.FechaDesc + "<br>(MW)");
        this.misTamColumnas.push(120);
        this.miDataColumnas.push({ data: 'Pftotpf' + (i + 1), type: 'numeric', className: 'htCenter', validator: numeroDecimalValidator, allowInvalid: true, format: '0.00' });

    }

    $('#tblPotenciaFirme').text(""); //limpia el contenedor de la tabla

    this.tablaPotenciaFirme = new Handsontable(this.contenedorPotenciaFirme, {
        dataSchema: {
            Emprcodi: null,
            Equipadre: null,
            Equicodi: null,
            Pftotpe: null,
            Pftotfi: null,
            Pftotpg: null,
            Pftotfp: null,
            Pftotpf: null
        },

        colHeaders: misCabeceras,
        columns: miDataColumnas,
        colWidths: misTamColumnas,  //1210

        columnSorting: true,
        minSpareRows: 0,
        rowHeaders: true,
        autoWrapRow: true,
        startRows: 0,
        manualColumnResize: false,
        hiddenColumns: {
            columns: [],
            indicators: false
        },
        formulas: false,
        fillHandle: { //numFilas = num registros (no se incrementa al arrastrar)
            direction: 'vertical',
            autoInsertRow: false
        }
    });

    this.tablaPotenciaFirme.addHook('afterRender', function () {
        tablaPotenciaFirme.validateCells();
    });

    this.tablaPotenciaFirme.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrorPF = [];
    });

    this.tablaPotenciaFirme.addHook('afterValidate', function (isValid, value, row, prop, source) {
        if (
            prop === "Pftotpf1" || prop === "Pftotpf2" || prop === "Pftotpf3" || prop === "Pftotpf4" || prop === "Pftotpf5" ||
            prop === "Pftotpf6" || prop === "Pftotpf7" || prop === "Pftotpf8" || prop === "Pftotpf9") {
            var result = valorespfValidator(this, isValid, value, row, prop, 0, 999);
            return registrarErrores(result);
        }
    });
}


function mostrarTablaPotenciaFirme() {
    $("#mensaje").css("display", "none");
    cargarListaPotenciaFirme();
}


/**
 * Muestra el listado de Potencia Firme en la tabla handson
 */
function cargarListaPotenciaFirme() {

    var msj = "";
    var obj = {};

    obj = getObjetoJsonConsulta();
    msj = validarConsulta(obj);


    if (msj == "") {
        mostrarTodosBotonesMantenimiento();

        var revision = parseInt($('#cbRevision').val()) || 0;

        $("#contenidoPotenciaFirme").hide();

        $.ajax({
            url: controlador + "cargarLstPotenciaFirme",
            data: {
                recacodi: revision
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    //mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de Potencia Firme.');
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de Potencia Firme. Error: ' + result.Mensaje);
                } else {

                    armarEstructuraTablaWeb(result.ListaEscenario);
                    activarEdicionDeLaTabla(!result.TieneRegistroPrevio);

                    cargarTablaHandson(result);


                    $('#hfIdReporte').val(result.IdReporte);
                    $('#hrevision').val(result.IdRecalculo);

                    ListadoDeCambios(result.IdRecalculo);
                    if (result.ListaEscenario.length == 0) {
                        ocultarTodosBotonesMantenimiento();
                    }

                }
            },
            error: function (xhr, status) {
                //mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de Potencia Firme.');
            }
        });
    } else {
        alert(msj);
        mostrar_tabla_vacia();
    }
}


/**
 * Muestra el listado de PF en la tabla handson de cierto reporte
 */
function verPFPorVersion(reportecodi) {
    $("#contenidoPotenciaFirme").hide();

    $.ajax({
        url: controlador + "cargarLstPotenciaFirmePorVersionReporte",
        data: {
            reportecodi: reportecodi
        },
        type: 'POST',
        success: function (result) {
            if (result.Resultado === "-1") {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de Potencia Firme por versión.');
            } else {
                $('#historialPF').bPopup().close();
                armarEstructuraTablaWeb(result.ListaEscenario);
                activarEdicionDeLaTabla(false);

                cargarTablaHandson(result);

                $('#hfIdReporte').val(result.IdReporte);

            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');

        }
    });
}



/**
 * Exporta la información de la tabla handson a un archivo excel
 * */
function exportarExcel() {

    var numReporte = 9;
    var reporteCodi = $("#hfIdReporte").val();
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoPFirme",
        data: {
            pfrtcodi: reporteCodi,
            tipoReporte: numReporte
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.Resultado == "0")
                    alert(evt.Mensaje);
                else
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
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
 * Función que verifica cual de los dos botones (Guardar o Editar) debe mostrarse
 */
function activarEdicionDeLaTabla(esEditable) {
    if (esEditable) {
        $("#versionInsumo").hide();
        $("#btnEditarData").hide();
        $("#btnGuardarData").show();
        $("#btnMostrarErrores").show();
        habilitarModoEdicion(tablaPotenciaFirme);

    } else {
        $("#btnEditarData").show();
        $("#btnGuardarData").hide();
        $("#btnMostrarErrores").hide();
        habilitarModoSoloLectura(tablaPotenciaFirme);
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
    $("#contenidoPotenciaFirme").show();
    RellenarTablaHandson(tablaPotenciaFirme, result.ListaPotenciaFirme, result.ListaEscenario);
    //updateDimensionHandson(tablaPotenciaFirme, contenedorPotenciaFirme);                   
}

/**
 * Limpia la tabla handson
 * */
function hansonTableClear() {

    tablaPotenciaFirme.loadData([]);
}


/**
 * Rellena y muestra la información en la tabla handson
 */
function RellenarTablaHandson(tablaExcelWeb, inputJson, lstEscenarios) {
    var lstPotenciaFirme = inputJson && inputJson.length > 0 ? inputJson : [];
    var numEscenarios = lstEscenarios.length;
    var lstData = [];
    tablaExcelWeb.loadData(lstData);

    for (var index in lstPotenciaFirme) {
        var item = lstPotenciaFirme[index];
        //seteando 0 a los null
        if (item.Pftotpf1 == null) item.Pftotpf1 = 0;
        if (item.Pftotpf2 == null) item.Pftotpf2 = 0;
        if (item.Pftotpf3 == null) item.Pftotpf3 = 0;
        if (item.Pftotpf4 == null) item.Pftotpf4 = 0;
        if (item.Pftotpf5 == null) item.Pftotpf5 = 0;
        if (item.Pftotpf6 == null) item.Pftotpf6 = 0;
        if (item.Pftotpf7 == null) item.Pftotpf7 = 0;
        if (item.Pftotpf8 == null) item.Pftotpf8 = 0;
        if (item.Pftotpf9 == null) item.Pftotpf9 = 0;

        var data = {
            Pftotcodi: item.Pftotcodi,
            Emprnomb: item.Emprnomb,
            Emprcodi: item.Emprcodi,
            Equipadre: item.Equipadre,
            Central: item.Central,
            Equicodi: item.Equicodi,
            Equinomb: item.Pftotunidadnomb,
            Famcodi: item.Famcodi,
            Famnomb: item.Famnomb,
            Grupocodi: item.Grupocodi,
            Pftotpe: item.Pftotpe,
            Pftotfi: item.Pftotfi,
            Pftotpg: item.Pftotpg,
            Pftotfp: item.Pftotfp,
            Pftotalemp: item.Pftotalemp,

            Pftotpf1: item.Pftotpf1,
            Pftotpf2: item.Pftotpf2,
            Pftotpf3: item.Pftotpf3,
            Pftotpf4: item.Pftotpf4,
            Pftotpf5: item.Pftotpf5,
            Pftotpf6: item.Pftotpf6,
            Pftotpf7: item.Pftotpf7,
            Pftotpf8: item.Pftotpf8,
            Pftotpf9: item.Pftotpf9,


        };

        lstData.push(data);
    }
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
        listaErrorPF.push(result.error);
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
function repintadoRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    $(td).addClass("celdaAzul");
}

/**
 * Metodo que valida el rango de los valores de potecia firme
 */
function valorespfValidator(instance, isValid, value2, row, prop, valMin, valMax) {
    var error = [];
    var columnName = instance.getColHeader(instance.propToCol(prop));
    var className, mensaje;

    value = value2;
    if (value2 != undefined && value2 != null) {
        //value = value + '';
    }

    if ($.isNumeric(value) && isValid) {

        if (value < valMin) {
            className = "errorLimitInferior";
            mensaje = "El dato es menor que el límite inferior.";
            isValid = false;
        }

        if (value > valMax) {
            className = "errorLimitSuperior";
            mensaje = "El dato es mayor que el límite superior.";
            isValid = false;
        }

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

    if (listaErrorPF.length === 0) {
        var revision = parseInt($('#cbRevision').val()) || 0;

        var reportecodi = parseInt($('#hfIdReporte').val()) || 0;
        var dataHandson, dataValido = [];
        dataHandson = tablaPotenciaFirme.getSourceData();
        dataValido = obtenerDataPFValido(dataHandson, true);

        var dataJson = {
            recacodi: revision,
            reportecodi: reportecodi,
            lstPFirme: dataValido
        };

        $.ajax({
            url: controlador + "GuardarListadoEditadoPF",

            data: JSON.stringify(dataJson),
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error al momento de guardar la información.');
                } else {
                    if (result.Resultado === "1") {
                        mostrarMensaje('mensaje', 'message', 'La información modificada de Potencia Firme fueron guardados exitosamente.');
                        listaErrorPF = [];
                        mostrarTablaPotenciaFirme();
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
function obtenerDataPFValido(dataHandson, soloFilaValida) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

        //Si se desea filtrar data por alguna condición, agregar aqui
        lstData.push(item);
    }
    return lstData;
};


/**
 * Lista en un popup las versiones existentes para el calculo de PF segun periodo
 * */
function ListadoDeCambios(recalculo) {

    $('#listadoHPF').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoDeCambiosPF",
        data: {
            mirecacodi: recalculo
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listadoHPF').html(evt.Resultado);

                $("#listadoHPF").css("width", (820) + "px");

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
                //mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error al listar las versiones del cálculo de Potencia Firme.');
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error al listar las versiones del cálculo de Potencia Firme. Error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            //mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error al listar las versiones del cálculo de Potencia Firme.');
        }
    });
};

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
                if (evt.ListaRecalculo.length > 0) {
                    $.each(evt.ListaRecalculo, function (i, item) {
                        $('#cbRevision').get(0).options[$('#cbRevision').get(0).options.length] = new Option(item.Pfrecanombre, item.Pfrecacodi);
                    });
                } else {
                    $('#cbRevision').get(0).options[0] = new Option("--", "0");
                }

                mostrarTablaPotenciaFirme();
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
 * Validar que existan todos los parametros al momento de hacer la consulta
 */
function validarConsulta() {
    return "";
}


/**
 * Parametros para consulta
 * */
function getObjetoJsonConsulta() {
    var obj = {};

    obj.consulta_revi = $("#cbRevision").val() || 0;

    return obj;
}


function mostrar_tabla_vacia() {
    armarEstructuraTablaWeb([]);
    ocultarTodosBotonesMantenimiento();
}

function ocultarTodosBotonesMantenimiento() {
    $("#btnEditarData").hide();
    $("#btnGuardarData").hide();
    $("#btnVerHistorial").hide();
    $("#btnMostrarErrores").hide();
    $("#btnExportExcel").hide();
}

function mostrarTodosBotonesMantenimiento() {
    $("#btnEditarData").show();
    $("#btnGuardarData").show();
    $("#btnVerHistorial").show();
    $("#btnMostrarErrores").show();
    $("#btnExportExcel").show();
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