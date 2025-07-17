var controlador = siteRoot + 'PotenciaFirme/Insumos/';

var tablaFactorPresencia;
var contenedorFactorPresencia;
var tblErroresdatos;
var listaErrorFP = [];
var RECURSO_FP = 6;
var VIENE_DE_CONSULTA = 1;


$(function () {
    SetearFiltradoDefault();

    opcionImportarFP();

    listadoVersion();

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodo').change(function () {
        cargarRevisiones();
    });

    $('#cbRevision').change(function () {
        mostrarTablaFactorPresencia();
    });

    $('#cbEmpresa').change(function () {
        cargarCentrales();
    });

    $("#btnConsultar").click(function () {
        mostrarTablaFactorPresencia(VIENE_DE_CONSULTA);
    });

    $("#btnGuardarData").click(function () {
        GuardarDataTabla();

    });

    $("#btnEditarData").click(function () {
        activarEdicionDeLaTabla(true)
    });

    $("#btnCargarBD").click(function () {
        verPorVersion(-3);
    });

    $("#btnMostrarErrores").click(function () {
        var listaErrorUnic = [];
        openPopup("erroresDatos");

        $.each(listaErrorFP, function (key, value) {
            var result = listaErrorUnic.filter(x => x.address === value.address);
            if (result.length === 0) {
                listaErrorUnic.push(value);
            }
        });

        tblErroresdatos.clear().draw();
        tblErroresdatos.rows.add(listaErrorUnic).draw();
    });

    $("#btnVerHistorial").click(function () {
        openPopup("historialFP");
    });

    $('#btnExportExcel').click(function () {
        exportarExcel();
    });

    ///////////////////////////
    /// HandsonTable
    ///////////////////////////

    // #region Handsontable Factor de Presencia
    contenedorFactorPresencia = document.getElementById('tblFactorPresencia');

    tablaFactorPresencia = new Handsontable(contenedorFactorPresencia, {
        dataSchema: {
            Emprcodi: null,
            Equipadre: null,
            Equicodi: null,
            Pffactfp: null
        },

        colHeaders: ['Empresa', 'Central', 'Unidad', 'Factor Presencia'],
        columns: [
            { data: 'Emprnomb', readOnly: true, editor: false, renderer: repintadoRenderer },
            { data: 'Central', readOnly: true, editor: false, renderer: repintadoRenderer },
            { data: 'Pffactunidadnomb', readOnly: true, editor: false, renderer: repintadoRenderer },
            { data: 'Pffactfp', className: 'htRight', type: 'numeric', validator: numeroDecimalValidator, allowInvalid: true, format: '0.00' },
        ],
        colWidths: [370, 220, 360, 180],  //1210
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

    tablaFactorPresencia.addHook('afterRender', function () {
        tablaFactorPresencia.validateCells();
    });

    tablaFactorPresencia.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrorFP = [];
    });

    tablaFactorPresencia.addHook('afterValidate', function (isValid, value, row, prop, source) {
        if (prop === "Pffactfp") {
            var result = factoresValidator(this, isValid, value, row, prop, 0, 1);
            return registrarErrores(result);
        }
    });

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

    mostrarTablaFactorPresencia();

});

function mostrarTablaFactorPresencia(origen) {
    $("#mensaje").css("display", "none");
    cargarListaFactorPresencia(origen);
}

/**
 * Muestra el listado de factores presencia en la tabla handson
 */
function cargarListaFactorPresencia(origen) {
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

        $("#contenidoFactorPresencia").hide();

        $.ajax({
            url: controlador + "CargarLstFactorPresencia",
            data: {
                revision: revision,
                emprcodi: $("#cbEmpresa").val(),
                central: $("#cbCentral").val(),
                verscodi: -2
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    //mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de factor presencia.');
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de factor presencia. Error: ' + result.Mensaje);
                } else {
                    activarEdicionDeLaTabla(!result.TieneRegistroPrevio);
                    listadoVersion();
                    cargarTablaHandson(result);
                    $('#hversion').val(result.Version);
                    if (result.NumVersion > 0) {
                        $('#versnumero').text("Versión: " + result.NumVersion);
                    } else {
                        $('#versnumero').text("");
                    }

                    if (!result.TieneRegistroPrevio)
                        $("#msj_cargar_bd").show();
                }
            },
            error: function (xhr, status) {
                //mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de factor presencia.');
            }
        });
    } else {
        alert(msj);
    }
}

/**
 * Muestra el listado de factores de presencia en la tabla handson de cierta versión
 */
function verPorVersion(verscodi) {
    $("#contenidoFactorPresencia").hide();

    var revision = parseInt($('#cbRevision').val()) || 0;

    $.ajax({
        url: controlador + "CargarLstFactorPresencia",
        data: {
            revision: revision,
            emprcodi: -2,
            central: -2,
            verscodi: verscodi
        },
        type: 'POST',
        success: function (result) {
            if (result.Resultado === "-1") {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de factor presencia por versión. Error: ' + result.Mensaje);
            } else {
                $('#historialFP').bPopup().close();
                activarEdicionDeLaTabla(verscodi < 0);
                cargarTablaHandson(result);
                $('#hversion').val(verscodi);
                if (result.NumVersion > 0) {
                    $('#versnumero').text("Versión: " + result.NumVersion);
                } else {
                    $('#versnumero').text("");
                }
            }
        },
        error: function (xhr, status) {
            //mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de factor presencia por versión.');

        }
    });
}

/**
 * Exporta la información de la tabla handson a un archivo excel
 * */
function exportarExcel() {
    var dataHandson, dataValido = [];
    dataHandson = tablaFactorPresencia.getSourceData();
    dataValido = obtenerDataFPValido(dataHandson, true);
    var datos = {
        listaFP: dataValido
    };

    $.ajax({
        url: controlador + "DescargarFormatoFP",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(datos),
        success: function (result) {
            switch (result.NRegistros) {
                case 1: {// si hay elementos
                    window.location = controlador + "Exportar?fi=" + result.Resultado; break;
                }
                case 2: {// sino hay elementos
                    mostrarMensaje('mensaje', 'error', 'No existen registros !');
                    break;
                }
                case -1: {// Error en C#
                    mostrarMensaje('mensaje', 'error', 'Error en reporte result');
                    break;
                }
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

    $("#msj_cargar_bd").hide();
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
        $("#btnImportar").show();
        $("#btnCargarBD").show();
        $("#btnMostrarErrores").show();
        habilitarModoEdicion(tablaFactorPresencia);

    } else {
        $("#btnEditarData").show();
        $("#btnGuardarData").hide();
        $("#btnImportar").hide();
        $("#btnCargarBD").hide();
        $("#btnMostrarErrores").hide();
        habilitarModoSoloLectura(tablaFactorPresencia);
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
    $("#contenidoFactorPresencia").show();
    RellenarTablaHandson(tablaFactorPresencia, result.ListaFactorPresencia);
    //updateDimensionHandson(tablaFactorPresencia, contenedorFactorPresencia);                   
}

/**
 * Limpia la tabla handson
 * */
function hansonTableClear() {

    tablaFactorPresencia.loadData([]);
}

/**
 * Rellena y muestra la información en la tabla handson
 */
function RellenarTablaHandson(tablaExcelWeb, inputJson) {
    var lstFactorPresencia = inputJson && inputJson.length > 0 ? inputJson : [];

    var lstData = [];
    tablaExcelWeb.loadData(lstData);

    for (var index in lstFactorPresencia) {
        var item = lstFactorPresencia[index];

        var data = {
            Emprcodi: item.Emprcodi,
            Emprnomb: item.Emprnomb,
            Equipadre: item.Equipadre,
            Central: item.Central,
            Equicodi: item.Equicodi,
            Pffactunidadnomb: item.Pffactunidadnomb,
            Grupocodi: item.Grupocodi,
            Pffactfp: item.Pffactfp
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
        listaErrorFP.push(result.error);
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
 * Metodo que valida el rango de los factores presencia
 */
function factoresValidator(instance, isValid, value2, row, prop, valMin, valMax) {
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


        if (value < 1 && value != null) {
            var td = instance.getCell(row, 3);
            $(td).addClass("celdaRoja");
        }
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
    if (listaErrorFP.length === 0) {
        var dataHandson, dataValido = [];
        dataHandson = tablaFactorPresencia.getSourceData();
        dataValido = obtenerDataFPValido(dataHandson, true);

        var dataJson = {
            periodo: $('#txtPeriodo').val(),
            recacodi: $('#cbRevision').val(),
            verscodi: $('#hversion').val(),
            lstFPresencia: dataValido
        };

        $.ajax({
            url: controlador + "GuardarListadoFactorPresencia",

            data: JSON.stringify(dataJson),
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error al momento de guardar la información.');
                    //mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al momento de guardar la información.');
                } else {
                    if (result.Resultado === "1") {
                        mostrarMensaje('mensaje', 'message', 'Los Factores presencia fueron guardados exitosamente.');
                        listaErrorFP = [];
                        mostrarTablaFactorPresencia();
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
function obtenerDataFPValido(dataHandson, soloFilaValida) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

        //Si se desea filtrar data por alguna condición, agregar aqui
        lstData.push(item);
    }
    return lstData;
}

/**
 * Parametros de filtro por defecto
 */
function SetearFiltradoDefault() {
    $('#cbEmpresa').val(-2);
    $('#cbCentral').val(-2);
}

/**
 * Rellena el listado de centrales
 * */
function cargarCentrales() {
    var periodo = parseInt($('#cbPeriodo').val()) || 0;
    var empresa = $('#cbEmpresa').val();

    if (empresa == "-2") empresa = "-1";
    $('#hempresa').val(empresa);
    $.ajax({
        type: 'POST',
        url: controlador + '/CargarCentrales',

        data: {
            idEmpresa: $('#hempresa').val(),
            periodo: periodo,
            tiporecurso: RECURSO_FP
        },

        success: function (aData) {

            $('#centrales').html(aData);
        },
        error: function (err) {
            //mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de centrales.');
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de centrales.');
        }
    });
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
                if (evt.ListaRecalculo.length > 0) {
                    $.each(evt.ListaRecalculo, function (i, item) {
                        $('#cbRevision').get(0).options[$('#cbRevision').get(0).options.length] = new Option(item.Pfrecanombre, item.Pfrecacodi);
                    });
                } else {
                    $('#cbRevision').get(0).options[0] = new Option("--", "0");
                }

                mostrarTablaFactorPresencia();
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
 * Importa informacion del insumo desde un archivo excel
 * */
function opcionImportarFP() {

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnImportar",
        url: controlador + "ImportarInfoFP",
        multi_selection: false,
        filters: {
            max_file_size: 0,
            mime_types: [
                { title: "Archivos xls", extensions: "xls,xlsx" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                //if (uploader.files.length === 2) {
                //    uploader.removeFile(uploader.files[0]);
                //}
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                //mostrarMensaje('mensajeInterr', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },

            FileUploaded: function (up, file, info) {
                var result = JSON.parse(info.response);
                if (result.Resultado != "-1") {
                    mostrarMensaje('mensaje', 'exito', "Los datos se cargaron correctamente, presione el botón Guardar para grabar.");
                    //mostrarMensaje_('mensaje', 'exito', "Los datos se cargaron correctamente, presione el botón Guardar para grabar.");
                    cargarTablaHandson(result);
                } else {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                    //mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
                }
            },
            UploadComplete: function (up, file, info) {
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
                if (err.code === -600) {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                    //mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
                    return;

                }
            }
        }
    });

    uploader.init();
}

/**
 * Lista en un popup las versiones existentes para el insumo segun tipo de insumo y revisión
 * */
function listadoVersion() {

    $('#listadoHFP').html('');

    var recurso = RECURSO_FP;     //CAMBIAR SEGUN INSUMO LA TABLA RECURSO 
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
                $('#listadoHFP').html(evt.Resultado);

                $("#listadoHFP").css("width", (820) + "px");

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
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error al listar las versiones. Error:' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            //mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
};

/**
 * Validar que existan todos los parametros al momento de hacer la consulta
 */
function validarConsulta(obj) {
    var msj = "";

    if (obj.consulta_empr == 0) {
        msj += "Debe ingresar una empresa correcta para realizar la consulta." + "\n";
    } else {
        if (obj.consulta_cent == 0) {
            msj += "Debe ingresar una central correcta para realizar la consulta." + "\n";
        } else {

        }
    }

    return msj;
}

/**
 * Parametros para consulta
 * */
function getObjetoJsonConsulta() {
    var obj = {};

    obj.consulta_revi = $("#cbRevision").val() || 0;
    obj.consulta_empr = $("#cbEmpresa").val() || 0;
    obj.consulta_cent = $("#cbCentral").val() || 0;

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
