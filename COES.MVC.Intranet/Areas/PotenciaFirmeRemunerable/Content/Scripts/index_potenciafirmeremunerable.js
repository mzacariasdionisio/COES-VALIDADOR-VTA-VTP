var controlador = siteRoot + 'PotenciaFirmeRemunerable/Reporte/';
var controlador2 = siteRoot + 'PotenciaFirmeRemunerable/General/';

var tablaPotenciaFirme;
var contenedorPotenciaFirme;
var tblErroresdatos;
var listaErrores = [];
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

        $.each(listaErrores, function (key, value) {
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

        //$("#contenidoPotenciaFirme").hide();

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

                    //armarEstructuraTablaWeb(result.ListaEscenario);
                    inicializarHansonTable(result.HandsonModel);
                    activarEdicionDeLaTabla(!result.TieneRegistroPrevio);

                    $("#contenidoPotenciaFirme").show();


                    $('#hfIdReporte').val(result.IdReporte);
                    $('#hrevision').val(result.IdRecalculo);

                    if (result.IdRecalculo > 0) {
                        ListadoDeCambios(result.IdRecalculo);
                    }

                    //if (!result.ListaEscenario || result.ListaEscenario.length == 0 ) {
                    //    ocultarTodosBotonesMantenimiento();
                    //}


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


function inicializarHansonTable(hot) {
    var contenedorHt = document.getElementById('tblPotenciaFirme');

    var nestedHeader = getNestedHeader(hot.NestedHeader.ListCellNestedHeaders);
    misCabeceras = getHeader(hot.NestedHeader.ListCellNestedHeaders);
    var data = JSON.parse(hot.ListaExcelData2);

    if (tablaPotenciaFirme && data.length == 0) {

        tablaPotenciaFirme.updateSettings({
            nestedHeaders: nestedHeader,
            columns: hot.Columnas,
        });
        tablaPotenciaFirme.loadData([]);
    } else {

        tablaPotenciaFirme = new Handsontable(contenedorHt, {
            data: JSON.parse(hot.ListaExcelData2),
            nestedHeaders: nestedHeader,
            columns: hot.Columnas,
            width: '100%',
            height: 500,
            rowHeights: 23,
            colHeaders: true,
            rowHeaders: true,
            fixedColumnsLeft: 2,
            manualColumnResize: hot.ListaColWidth,
            hiddenColumns: {
                indicators: false,
                columns: [2, 3, 4],
            }
        });

    }




    tablaPotenciaFirme.addHook('afterRender', function () {
        tablaPotenciaFirme.validateCells();
    });

    tablaPotenciaFirme.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrores = [];
    });

    tablaPotenciaFirme.addHook('afterValidate', function (isValid, value, row, prop, source) {

        var col = tablaPotenciaFirme.propToCol(prop);
        var periodoDesc = misCabeceras[0][col];
        var cabecera = misCabeceras[1][col].replaceAll('<br>', ' ');
        var error = [];

        if (!isValid) {

            if ($.isNumeric(value)) {

                if (value < 0) {

                    error = {
                        address: `<b>[</b>${periodoDesc}] [${cabecera}<b>]</b> <b>[</b>${row + 1}<b>]</b>`,
                        valor: value,
                        className: "errorLimitInferior",
                        message: "El dato es menor que el límite inferior."
                    };
                    this.setCellMeta(row, this.propToCol(prop), "invalidCellClassName", error.className);
                    listaErrores.push(error);

                }

            } else if (value) {

                error = {
                    address: `<b>[</b>${periodoDesc}] [${cabecera}<b>]</b> <b>[</b>${row + 1}<b>]</b>`,
                    valor: value,
                    className: "htInvalid",
                    message: "El dato no es numérico."
                };
                this.setCellMeta(row, this.propToCol(prop), "invalidCellClassName", error.className);
                listaErrores.push(error);

            }

        }

        return isValid;

    });

}

function getNestedHeader(lstNestedHeaders) {

    var nestedHeaders = [];

    lstNestedHeaders.forEach(function (currentValue, index, array) {
        var nestedHeader = [];
        currentValue.forEach(function (item) {
            if (item.Colspan == 0) {
                nestedHeader.push(item.Label);
            } else {
                nestedHeader.push({ label: item.Label, colspan: item.Colspan });
            }
        });

        nestedHeaders.push(nestedHeader);
    });

    return nestedHeaders;
}

function getHeader(lstNestedHeaders) {

    var nestedHeaders = [];

    lstNestedHeaders.forEach(function (currentValue, index, array) {
        var nestedHeader = [];
        currentValue.forEach(function (item) {
            if (item.Colspan == 0) {
                nestedHeader.push(item.Label);
            } else {
                for (var i = 0; i < item.Colspan; i++) {
                    nestedHeader.push(item.Label);
                }
            }
        });

        nestedHeaders.push(nestedHeader);
    });

    return nestedHeaders;
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

                inicializarHansonTable(result.HandsonModel);
                activarEdicionDeLaTabla(false);

                $('#hfIdReporte').val(result.IdReporte);
                $("#contenidoPotenciaFirme").show(); 

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
}

/**
 * Limpia la tabla handson
 * */
function hansonTableClear() {

    tablaPotenciaFirme.loadData([]);
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


(function (Handsontable) {
    function customValidator(value, callback) {
        var valueToValidate = value;

        if (valueToValidate === null || valueToValidate === void 0) {
            valueToValidate = '';
        }

        if (this.allowEmpty && valueToValidate === '') {
            callback(true);
        } else if (valueToValidate === '') {
            callback(false);
        } else if ($.isNumeric(value)) {
            callback(value >= 0);
        } else {
            callback(false);
        }
    }

    /**
 * Colorea de azul las celdas Empresa, central y unidad
 */
    function repintadoRenderer(instance, td, row, col, prop, value, cellProperties) {
        Handsontable.renderers.TextRenderer.apply(this, arguments);
        $(td).addClass("celdaAzul");
    }

    // Register an alias
    Handsontable.validators.registerValidator('validatorDecimal', customValidator);
    Handsontable.renderers.registerRenderer('repintadoRenderer', repintadoRenderer);

})(Handsontable);



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

    if (listaErrores.length === 0) {
        var revision = parseInt($('#cbRevision').val()) || 0;

        var reportecodi = parseInt($('#hfIdReporte').val()) || 0;
        var dataHandson, dataValido = [];
        dataHandson = tablaPotenciaFirme.getSourceData();
        var data = getDataHandsonTable(dataHandson);

        if (data.length == 0) {
            alert("No hay registro para Guardar");
            return;
        }

        dataValido = JSON.stringify(data);

        var dataJson = {
            recacodi: revision,
            reportecodi: reportecodi,
            stringJson: dataValido
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
        alert("Existen errores en las celdas, favor de corregir y vuelva a envíar.");
        //mostrarMensaje('mensaje', 'error', 'Existen errores en las celdas, favor de corregir y vuelva a envíar.');
        $("#btnMostrarErrores").trigger("click");
    }
};

function getDataHandsonTable(dataHandson) {

    var lstdata = [];

    dataHandson.forEach(function (items) {

        Object.entries(items).forEach(item => {
            var property = item[0];//Nombre propiedad

            if (item[1]) { //Valor propiedad

                var column = parseInt(property);

                if (Number.isInteger(column)) {//Periodo

                    var indreporte = {
                        Equipadre: items.Equipadre,
                        Equicodi: items.Equicodi,
                        Grupocodi: items.Grupocodi,
                        Central: items.Central,
                        Pfrtotunidadnomb: items.Pfrtotunidadnomb
                    };

                    var escenario = items[`${column}`];

                    indreporte.Pfresccodi = column;
                    indreporte.Pfrtotpd = escenario.Pfrtotpd;
                    indreporte.Pfrtotpdd = escenario.Pfrtotpdd;
                    indreporte.Pfrtotpf = escenario.Pfrtotpf;
                    indreporte.Pfrtotpfr = escenario.Pfrtotpfr;


                    lstdata.push(indreporte);
                }
            }
        });
    });

    return lstdata;
}


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
        url: controlador2 + "PeriodoListado",
        data: {
            anio: anio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodo.length > 0) {
                    $.each(evt.ListaPeriodo, function (i, item) {
                        $('#cbPeriodo').get(0).options[$('#cbPeriodo').get(0).options.length] = new Option(item.Pfrpernombre, item.Pfrpercodi);
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
        url: controlador2 + "CargarRevisiones",
        data: {
            pfpericodi: pfpericodi,
        },
        success: function (evt) {

            if (evt.Resultado != "-1") {

                if (evt.ListaRecalculo.length > 0) {
                    $.each(evt.ListaRecalculo, function (i, item) {
                        $('#cbRevision').get(0).options[$('#cbRevision').get(0).options.length] = new Option(item.Pfrrecnombre, item.Pfrreccodi);
                    });
                } else {
                    $('#cbRevision').get(0).options[0] = new Option("--", "0");
                }
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