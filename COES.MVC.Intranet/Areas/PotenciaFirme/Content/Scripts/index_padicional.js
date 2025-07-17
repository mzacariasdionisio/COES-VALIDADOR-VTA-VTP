var controlador = siteRoot + 'PotenciaFirme/Insumos/';

var tblPotAdicional;
var contenedorPotAdicional;
var tblErroresdatos;
var listaErrorPA = [];
var RECURSO_PA = 2;
var DATO_IMPORTADO = 1;
var DATO_CARGADO = 0;
var EDITAR = 1;
var VIENE_DE_CONSULTA = 2;

$(function () {
    SetearFiltradoDefault();
    crearPupload();
    listadoVersion();

    $('#cbAnio').change(function () {
        listadoPeriodo();
    });

    $('#cbPeriodo').change(function () {
        cargarRevisiones();
    });

    $('#cbRevision').change(function () {
        mostrarTabPotenciaAdicional();
    });

    $('#cbEmpresa').change(function () {
        cargarCentrales();
    });

    $("#btnConsultar").click(function () {
        mostrarTabPotenciaAdicional(VIENE_DE_CONSULTA);
    });

    $("#btnGuardarData").click(function () {
        GuardarDataTabla();

    });

    $("#btnEditarData").click(function () {
        activarEdicionDeLaTabla(true);
    });

    $("#btnCargarBD").click(function () {
        verPorVersion(-3);
    });

    $("#btnMostrarErrores").click(function () {

        openPopup("erroresDatos");

        var listaErrorUnic = [];

        $.each(listaErrorPA, function (key, value) {
            var result = listaErrorUnic.filter(x => x.address === value.address);
            if (result.length === 0) {
                listaErrorUnic.push(value);
            }
        });

        tblErroresdatos.clear().draw();
        tblErroresdatos.rows.add(listaErrorUnic).draw();

    });

    $("#btnVerHistorial").click(function () {    //*************************************************************************
        openPopup("historialPA");
    });

    $('#btnExportExcel').click(function () {
        exportarExcel();
    });

    ///////////////////////////
    /// HandsonTable
    ///////////////////////////

    // #region Handsontable Potencia Garantizada
    contenedorPotAdicional = document.getElementById('tblPotAdicional');

    tblPotAdicional = new Handsontable(contenedorPotAdicional, {
        dataSchema: {

            Emprcodi: null,
            Equipadre: null,
            Equicodi: null,
            Pfpadipe: null,
            Pfpadifi: null,
            Pfpadipf: null,
            Formula: null,
            Pfpadiincremental: null
        },

        colHeaders: ['Empresa', 'Central', 'Unidad', 'P. Efectiva (MW)', 'Factor Indisponibilidad (%)', 'Potencia Firme (MW)', 'Formu', 'Increm'],
        columns: [


            { data: 'Emprnomb', readOnly: true, editor: false, renderer: repintadoAzulRenderer },
            { data: 'Central', readOnly: true, editor: false, renderer: repintadoAzulRenderer },
            { data: 'Pfpadiunidadnomb', readOnly: true, editor: false, className: 'celdaCU', renderer: repintadoRenderer, },
            { data: 'Pfpadipe', className: 'htRight', type: 'numeric', validator: numeroDecimalValidator, allowInvalid: true, format: '0.00' },
            { data: 'Pfpadifi', readOnly: true, className: 'htRight', type: 'numeric', validator: numeroDecimalValidator, allowInvalid: true, format: '0.00%', },
            { data: 'Pfpadipf', className: 'htRight', type: 'text', validator: numeroDecimalValidator, renderer: formulaRenderer, allowInvalid: true, },

            {
                data: 'Formula', className: 'htRight', type: 'text', validator: numeroDecimalValidator, renderer: formulaRenderer, allowInvalid: true,

            },
            { data: 'Pfpadiincremental', readOnly: true, editor: false },

        ],
        colWidths: [350, 280, 220, 100, 160, 160, 40, 40],  //1210
        columnSorting: true,
        minSpareRows: 0,
        rowHeaders: true,
        autoWrapRow: true,
        startRows: 0,
        manualColumnResize: false,
        hiddenColumns: {
            columns: [6, 7],
            indicators: false
        },
        formulas: true,
        fillHandle: { //numFilas = num registros (no se incrementa al arrastrar)
            direction: 'vertical',
            autoInsertRow: false
        }
    });

    tblPotAdicional.addHook('afterRender', function () {
        tblPotAdicional.validateCells();
    });

    tblPotAdicional.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrorPA = [];
    });

    tblPotAdicional.addHook('afterValidate', function (isValid, value, row, prop, source) {

        if (prop === "Pfpadipe" || prop === "Pfpadifi") {
            var result = potenciasValidator(this, isValid, value, row, prop, 0, 1000);
            return registrarErrores(result);
        }
        //console.log(prop);
        if (prop == "Formula" || prop === "Pfpadipf") {
            var result = formulaValidator(this, isValid, value, row, 6, prop, 0, 1000);
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

    mostrarTabPotenciaAdicional();
});

function mostrarTabPotenciaAdicional(accion) {
    $("#mensaje").css("display", "none");
    accion = parseInt(accion) || 0;
    cargarPotenciasAdicionales(accion);
}

function cargarPotenciasAdicionales(accion) {

    accion = parseInt(accion) || 0;
    var msj = "";
    var obj = {};

    if (accion == VIENE_DE_CONSULTA) {
        obj = getObjetoJsonConsulta();
        msj = validarConsulta(obj);
    }

    ocultarBarraHerramienta();
    var revision = parseInt($('#cbRevision').val()) || 0;
    if (revision <= 0)
        msj = "Debe seleccionar Recálculo de Potencia Firme.";

    if (msj == "") {
        $("#contentPAdicional ").hide();

        $.ajax({
            url: controlador + "CargarLstPotenciasAdicionales",
            data: {
                revision: revision,
                emprcodi: $("#cbEmpresa").val(),
                central: $("#cbCentral").val(),
                verscodi: -2,
                accion: accion
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de Potencias Adicionales. Error: ' + result.Mensaje);
                } else {
                    if (accion == 1)
                        activarEdicionDeLaTabla(true);
                    else
                        activarEdicionDeLaTabla(!result.TieneRegistroPrevio);
                    listadoVersion();
                    cargarTablaHandson(result, DATO_CARGADO);

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
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de Potencias Adicionales.');
                //alert('Se ha producido un error.');
            }
        });
    } else {
        alert(msj);
    }
}

function verPorVersion(verscodi) {
    ////mostrarMensaje('mensajeInterr', '', '');

    $("#contentPAdicional ").hide();

    var revision = parseInt($('#cbRevision').val()) || 0;

    $.ajax({
        url: controlador + "CargarLstPotenciasAdicionales",
        data: {
            revision: revision,
            emprcodi: -2,
            central: -2,
            verscodi: verscodi,
            accion: VIENE_DE_CONSULTA
        },
        type: 'POST',
        success: function (result) {
            if (result.Resultado === "-1") {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de Potencias Adicionales por versión. Error: ' + result.Mensaje);
                //alert('Se ha producido un error.');
            } else {
                //validarPlazoHtml(result, idEnvio, flagMostrarExitoEnvio);
                $('#historialPA').bPopup().close();
                activarEdicionDeLaTabla(verscodi < 0);
                cargarTablaHandson(result, DATO_CARGADO);
                $('#hversion').val(verscodi);
                if (result.NumVersion > 0) {
                    $('#versnumero').text("Versión: " + result.NumVersion);
                } else {
                    $('#versnumero').text("");
                }
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de Potencias Adicionales por versión. ');
            //alert('Se ha producido un error.');
        }
    });
}

function exportarExcel() {
    var dataHandson, dataValido = [];
    dataHandson = tblPotAdicional.getSourceData();
    dataValido = obtenerDataPAValido(dataHandson, true, 1);
    var datos = {
        listaPA: dataValido
    };

    $.ajax({
        url: controlador + "DescargarFormatoPA",
        type: 'POST',
        contentType: 'application/json; charset=UTF-8',
        dataType: 'json',
        data: JSON.stringify(datos),
        success: function (result) {
            switch (result.NRegistros) {
                case 1: window.location = controlador + "Exportar?fi=" + result.Resultado; break;// si hay elementos
                case 2: alert("No existen registros !"); break;// sino hay elementos
                case -1: alert("Error en reporte result"); break;// Error en C#
            }
        },
        error: function (xhr, status) {
            //mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
            alert('Se ha producido un error al momento de exportar.');
        }
    });
}

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
                    //mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
                    alert('Se ha producido un error.');
                } else {
                    listadoVersion();
                    alert('Cambios realizados con éxito.');


                }
            },
            error: function (xhr, status) {
                //mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
                alert('Se ha producido un error.');
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
 * @param {any} tieneRegistroPrevio
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
        habilitarModoEdicion(tblPotAdicional);

    } else {
        $("#btnEditarData").show();
        $("#btnGuardarData").hide();
        $("#btnImportar").hide();
        $("#btnCargarBD").hide();
        $("#btnMostrarErrores").hide();
        habilitarModoSoloLectura(tblPotAdicional);
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

function cargarTablaHandson(result, origen) {
    hansonTableClear();
    $("#contentPAdicional ").show();
    RellenarTablaHandson(tblPotAdicional, result.ListaPotenciaAdicional, origen);
    //updateDimensionHandson(tblPotAdicional, contenedorPotAdicional);                   
}

function hansonTableClear() {

    tblPotAdicional.loadData([]);
}

function RellenarTablaHandson(tablaExcelWeb, inputJson, origen) {
    var lstPotenciaAdicional = inputJson && inputJson.length > 0 ? inputJson : [];

    //var fechaInterrupcion = $("#txtFechaInterrupcion").val();

    var lstData = [];
    tablaExcelWeb.loadData(lstData);
    var fila = 0;
    for (var index in lstPotenciaAdicional) {
        fila++;
        var item = lstPotenciaAdicional[index];
        //var fechaDefault = item.Intsumfechainterrini2 != null && item.Intsumfechainterrini2 != '' ? item.Intsumfechainterrini2 : fechaInterrupcion;

        var mifi;
        var mipf;

        //Para columna FI
        if (origen == DATO_CARGADO) {
            if (item.Pfpadifi == null) {
                if (item.Formula != "")
                    mifi = 0.0;
                else
                    mifi = null;
            } else
                mifi = item.Pfpadifi;
        } else {
            if (origen == DATO_IMPORTADO) {
                mifi = item.Pfpadifi;
            }
        }


        //para columna PF
        if (origen == DATO_CARGADO) {
            if (item.Formula == "") {
                mipf = item.Pfpadipf;
            } else {
                mipf = item.Formula;
            }
        } else {
            if (origen == DATO_IMPORTADO) {
                if (item.Pfpadiincremental == 0)
                    mipf = item.Pfpadipf;
                else {
                    if (item.Pfpadiincremental == 1) {
                        mipf = "=+(1-E" + fila + ")*D" + fila;
                    }

                }
            }
        }

        //Para el campo Unidad
        var miunidad = item.Pfpadiunidadnomb;
        if (origen == DATO_CARGADO) {
            if (item.Equicodi == 90000) {
                miunidad = miunidad + "(*)";
            }
        } else {

        }



        var data = {
            Emprcodi: item.Emprcodi,
            Emprnomb: item.Emprnomb,
            Equipadre: item.Equipadre,
            Central: item.Central,
            Equicodi: item.Equicodi,
            Pfpadiunidadnomb: miunidad,
            Grupocodi: item.Grupocodi,
            Pfpadipe: item.Pfpadipe,
            Pfpadifi: mifi,
            Pfpadipf: mipf,
            Formula: item.Formula,
            Pfpadiincremental: item.Pfpadiincremental
        };

        lstData.push(data);
    }
    tablaExcelWeb.loadData(lstData);
}

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

function registrarErrores(result) {
    if (!result.valid)
        listaErrorPA.push(result.error);
    return result.valid;
}

numeroDecimalValidator = function (value, callback) {

    var isValid = true;
    if ($.isNumeric(value) && isValid) {

    } else if (value && !$.isNumeric(value)) {
        if (value != undefined && value != null && value != '' && !(value + "").includes("=+(")) {
            isValid = false;
        }
    }

    callback(isValid);
};

function formulaRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    var colu1 = col;
    var fila1 = row;
    var srtr = value;
    var esNumerico = !isNaN(value) && value != null && value != undefined;

    if ((value + "").includes("=+(")) {
        var svalor = tblPotAdicional.plugin.helper.cellValue('G' + (row + 1));
        var pe = instance.getDataAtCell(row, col - 2);
        var fi = instance.getDataAtCell(row, col - 1);

        if (pe !== null && pe !== '' && fi !== null && fi !== '') {

            //var pf = pe * fi;
            var pf = (1 - fi) * pe;
            td.innerHTML = pf.toFixed(2);
        }

        cellProperties.readOnly = true;
    } else {
        if (value == "") {

        } else {
            if (esNumerico) {
                var val = instance.getDataAtCell(row, col);
                var val1 = val * 1;
                td.innerHTML = val1.toFixed(2);
            } else {
                td.innerHTML = value;
            }
        }
    }
}

function repintadoRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    if ((value + "").toUpperCase().includes("INCREMENTAL") || (value + "").toUpperCase().includes("ADICIONAL")) {
        $(td).addClass("celdaEspecial");
    } else {
        $(td).addClass("celdaAzul");
    }
}

/**
 * Colorea de azul las celdas Empresa, central y unidad
 */
function repintadoAzulRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    $(td).addClass("celdaAzul");
}

/**
 * Metodo que valida el rango de las potencias efectivas y potencias garantizadas
 * @param {any} instance
 * @param {any} isValid
 * @param {any} value2
 * @param {any} row
 * @param {any} prop
 * @param {any} valMin
 * @param {any} valMax
 */
function potenciasValidator(instance, isValid, value2, row, prop, valMin, valMax) {
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

        if (value >= valMax) {
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

function formulaValidator(instance, isValid, value2, row, col, prop, valMin, valMax) {

    var error = [];
    var columnName = instance.getColHeader(instance.propToCol(prop));
    var className, mensaje;

    var valor;
    if (value2 == undefined || value2 == null || value2 == "") {
        valor = 0;
    } else {
        if ((value2 + "").includes("=+(")) {
            valor = tblPotAdicional.plugin.helper.cellValue('G' + (row + 1));

            return {
                valid: true, error: error
            };
        } else {
            valor = value2;
        }
    }

    value = valor;

    var id = 0;
    if ($.isNumeric(value)) {

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

        error = {
            address: `[<b>${columnName}</b>] [<b>${row + 1}</b>]`,
            valor: value,
            className: className,
            message: mensaje
        };
        instance.setCellMeta(row, instance.propToCol(prop), "invalidCellClassName", className);

        //var td = instance.getCell(row, col);
        //formulaRenderer(instance, td, row, col, prop, value, null);

    } else {
        if (!$.isNumeric(value)) {

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

function GuardarDataTabla() {
    if (listaErrorPA.length === 0) {
        //mostrarMensaje('mensajeInterr', '', '');
        var dataHandson, dataValido = [];
        dataHandson = tblPotAdicional.getSourceData();
        dataValido = obtenerDataPAValido(dataHandson, true, 0);

        var dataJson = {
            periodo: $('#txtPeriodo').val(),
            recacodi: $('#cbRevision').val(),
            verscodi: $('#hversion').val(),
            lstPAdicional: dataValido
        };

        $.ajax({
            url: controlador + "GuardarPotenciaAdicional",

            data: JSON.stringify(dataJson),
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            success: function (result) {
                if (result.Resultado === "-1") {
                    alert('Se ha producido un error.');
                } else {


                    if (result.Resultado === "1") {
                        alert('La potencia Adicional fue guardada exitosamente.');
                        listaErrorPA = [];
                        mostrarTabPotenciaAdicional();
                        activarEdicionDeLaTabla(false);
                    }
                }
            },
            error: function (xhr, status) {
                alert('Se ha producido un error.');
            }
        });
    }
    else {
        alert('Existen errores en las celdas, favor de corregir y vuelva a envíar.');
        $("#btnMostrarErrores").trigger("click");
    }
};

function obtenerDataPAValido(dataHandson, soloFilaValida, tipo) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];
        if (tipo == 0) {
            item.Pfpadiunidadnomb = item.Pfpadiunidadnomb.replace('(*)', '');
        }
        //Si se desea filtrar data por alguna condición, agregar aqui
        lstData.push(item);

    }
    return lstData;
}

function SetearFiltradoDefault() {
    $('#cbEmpresa').val(-2);
    $('#cbCentral').val(-2);
}

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
            tiporecurso: RECURSO_PA
        },

        success: function (aData) {

            $('#centrales').html(aData);
        },
        error: function (err) {
            //alert("Ha ocurrido un error");
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de centrales.');
        }
    });
}
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

                mostrarTabPotenciaAdicional();
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function crearPupload() {

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnImportar",
        url: controlador + "UploadInfoPA",
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
                if (result.NRegistros != -1) {
                    alert("Los datos se cargaron correctamente, presione el botón Guardar para grabar.");
                    //cargarHansonTablePorFuenteDato(result);
                    cargarTablaHandson(result, DATO_IMPORTADO);
                } else {
                    alert('Se ha producido un error.');
                }
            },
            UploadComplete: function (up, file, info) {
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
                if (err.code === -600) {
                    alert("Ha ocurrido un error"); return;
                }
            }
        }
    });

    //uploader.bind('BeforeUpload', function (up, file) {
    //    up.settings.multipart_params = {
    //        emprcodi: $("#cbEmpresa").val(),
    //        afecodi: $('#txtAfecodi').val(),
    //        idtipoinformacion: $('#CboReportar').val()
    //    };
    //});

    uploader.init();
}

function listadoVersion() {

    $('#listadoHPA').html('');

    var recurso = RECURSO_PA;     //CAMBIAR SEGUN INSUMO LA TABLA RECURSO 
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
                $('#listadoHPA').html(evt.Resultado);

                $("#listadoHPA").css("width", (820) + "px");

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
                //alert("Ha ocurrido un error: " + evt.Mensaje);
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error al listar las versiones. Error:' + evt.Mensaje);
            }
        },
        error: function (err) {
            //alert('Ha ocurrido un error.');
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
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

function mostrarMensaje_(id, tipo, mensaje) {
    $("#mensaje").css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};