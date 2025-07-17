var controlador = siteRoot + 'PotenciaFirme/Insumos/';
var ancho = 1000;
var tblPotGarantizada;
var contenedorPotGarantizada;
var tblErroresdatos;
var listaErrorPG = [];
var RECURSO_PG = 1;
var VIENE_DE_CONSULTA = 1;

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
        mostrarTabPotenciaGarantizada();
    });

    $('#cbEmpresa').change(function () {
        cargarCentrales();
    });

    $("#btnConsultar").click(function () {
        mostrarTabPotenciaGarantizada(VIENE_DE_CONSULTA);
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

        openPopup("erroresDatos");

        var listaErrorUnic = [];

        $.each(listaErrorPG, function (key, value) {
            var result = listaErrorUnic.filter(x => x.address === value.address);
            if (result.length === 0) {
                listaErrorUnic.push(value);
            }
        });

        tblErroresdatos.clear().draw();
        tblErroresdatos.rows.add(listaErrorUnic).draw();

    });

    $("#btnVerHistorial").click(function () {    //*************************************************************************
        openPopup("historialPG");
    });

    $('#btnExportExcel').click(function () {
        exportarExcel();
    });

    ///////////////////////////
    /// HandsonTable
    ///////////////////////////

    // #region Handsontable Potencia Garantizada
    contenedorPotGarantizada = document.getElementById('tblPotGarantizada');

    tblPotGarantizada = new Handsontable(contenedorPotGarantizada, {
        dataSchema: {

            Emprcodi: null,
            Equipadre: null,
            Equicodi: null,
            Pfpgarpe: null,
            Pfpgarpg: null
        },

        colHeaders: ['Empresa', 'Central', 'Unidad', 'Potencia Efectiva (MW)', 'Potencia Garantizada (MW)'],
        columns: [

            { data: 'Emprnomb', readOnly: true, editor: false, renderer: repintadoRenderer },
            { data: 'Central', readOnly: true, editor: false, renderer: repintadoRenderer },
            { data: 'Equinomb', readOnly: true, editor: false, renderer: repintadoRenderer },
            { data: 'Pfpgarpe', className: 'htRight', type: 'numeric', validator: numeroDecimalValidator, allowInvalid: true, format: '0.000' },
            { data: 'Pfpgarpg', className: 'htRight', type: 'numeric', validator: numeroDecimalValidator, allowInvalid: true, format: '0.000' },

        ],
        colWidths: [370, 220, 360, 160, 160],  //1210
        columnSorting: true,
        minSpareRows: 0,
        rowHeaders: true,
        autoWrapRow: true,
        startRows: 0,
        manualColumnResize: false,
        fillHandle: {
            direction: 'vertical',
            autoInsertRow: false
        }
    });

    tblPotGarantizada.addHook('afterRender', function () {
        tblPotGarantizada.validateCells();
    });

    tblPotGarantizada.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrorPG = [];
    });

    tblPotGarantizada.addHook('afterValidate', function (isValid, value, row, prop, source) {

        if (prop === "Pfpgarpe" || prop === "Pfpgarpg") {
            var result = potenciasValidator(this, isValid, value, row, prop, 0, 1000);
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

    mostrarTabPotenciaGarantizada();
});

function mostrarTabPotenciaGarantizada(origen) {
    $("#mensaje").css("display", "none");
    cargarPotenciasGarantizadas(origen);
}

function cargarPotenciasGarantizadas(origen) {
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
        $("#contentPGarantizada").hide();

        $.ajax({
            url: controlador + "CargarLstPotenciasGarantizadas",
            data: {
                revision: revision,
                emprcodi: $("#cbEmpresa").val(),
                equicodi: $("#cbCentral").val(),
                verscodi: -2
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de Potencias Garantizadas. Error: ' + result.Mensaje);
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
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al cargar el listado de Potencias Garantizadas.');
                //alert('Se ha producido un error.');
            }
        });
    } else {
        alert(msj);
    }
}

function verPorVersion(verscodi) {
    var revision = parseInt($('#cbRevision').val()) || 0;

    $("#contentPGarantizada").hide();

    $.ajax({
        url: controlador + "CargarLstPotenciasGarantizadas",
        data: {
            revision: revision,
            emprcodi: -2,
            equicodi: -2,
            verscodi: verscodi
        },
        type: 'POST',
        success: function (result) {
            if (result.Resultado === "-1") {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de Potencias Garantizadas por versión. Error: ' + result.Mensaje);
            } else {
                $('#historialPG').bPopup().close();
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
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error al cargar el listado de Potencias Garantizadas por versión.');
            //alert('Se ha producido un error.');
        }
    });
}

function exportarExcel() {

    var dataHandson, dataValido = [];
    dataHandson = tblPotGarantizada.getSourceData();
    dataValido = obtenerDataPGValido(dataHandson, true);
    var datos = {
        listaPG: dataValido
    };

    $.ajax({
        url: controlador + "DescargarFormatoPG",
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

                    alert('Se ha producido un error.');
                } else {
                    listadoVersion();
                    alert('Cambios realizados con éxito.');


                }
            },
            error: function (xhr, status) {

                alert('Se ha producido un error.');
            }
        });
    }
}
/**
 * Colorea de azul las celdas Empresa, central y unidad
 */
function repintadoRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    $(td).addClass("celdaAzul");
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

function activarEdicionDeLaTabla(esEditable) {
    $("#btnExportExcel").show();
    $("#btnVerHistorial").show();

    if (esEditable) {
        $("#btnEditarData").hide();
        $("#btnGuardarData").show();
        $("#btnImportar").show();
        $("#btnCargarBD").show();
        $("#btnMostrarErrores").show();
        habilitarModoEdicion(tblPotGarantizada);
        $("#versionInsumo").hide();

    } else {
        $("#btnEditarData").show();
        $("#btnGuardarData").hide();
        $("#btnImportar").hide();
        $("#btnCargarBD").hide();
        $("#btnMostrarErrores").hide();
        habilitarModoSoloLectura(tblPotGarantizada);
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

function cargarTablaHandson(result) {
    hansonTableClear();
    $("#contentPGarantizada").show();
    RellenarTablaHandson(tblPotGarantizada, result.ListaPotenciaGarantizada);
    //updateDimensionHandson(tblPotGarantizada, contenedorPotGarantizada);                   
}

function hansonTableClear() {

    tblPotGarantizada.loadData([]);
}

function RellenarTablaHandson(tablaExcelWeb, inputJson) {
    var lstPotenciaGarantizada = inputJson && inputJson.length > 0 ? inputJson : [];

    //var fechaInterrupcion = $("#txtFechaInterrupcion").val();
    var lstData = [];
    tablaExcelWeb.loadData(lstData);

    for (var index in lstPotenciaGarantizada) {

        var item = lstPotenciaGarantizada[index];
        //var fechaDefault = item.Intsumfechainterrini2 != null && item.Intsumfechainterrini2 != '' ? item.Intsumfechainterrini2 : fechaInterrupcion;

        var data = {
            Emprcodi: item.Emprcodi,
            Emprnomb: item.Emprnomb,
            Equipadre: item.Equipadre,
            Central: item.Central,
            Equicodi: item.Equicodi,
            Equinomb: item.Equinomb,
            Grupocodi: item.Grupocodi,
            Pfpgarpe: item.Pfpgarpe,
            Pfpgarpg: item.Pfpgarpg
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
        listaErrorPG.push(result.error);
    return result.valid;
}

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
    if ($.isNumeric(value) && isValid) {
        if (prop === "Pfpgarpg") {

            if (parseFloat(value) > instance.getDataAtRowProp(row, 'Pfpgarpe')) {
                isValid = false;

                className = "htInvalid";
                mensaje = "La potencia garantizada es mayor que la efectiva";

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

    }


    return { valid: isValid, error: error };
}

function GuardarDataTabla() {
    if (listaErrorPG.length === 0) {


        var dataHandson, dataValido = [];
        dataHandson = tblPotGarantizada.getSourceData();
        dataValido = obtenerDataPGValido(dataHandson, true);

        var dataJson = {
            periodo: $('#txtPeriodo').val(),
            recacodi: $('#cbRevision').val(),
            verscodi: $('#hversion').val(),
            lstPGarantizada: dataValido
        };

        $.ajax({
            url: controlador + "GuardarPotenciaGarantizada",

            data: JSON.stringify(dataJson),
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            success: function (result) {
                if (result.Resultado === "-1") {
                    alert('Se ha producido un error.');
                } else {


                    if (result.Resultado === "1") {
                        alert('La potencia Garantizada fue guardada exitosamente.');
                        listaErrorPG = [];
                        mostrarTabPotenciaGarantizada();
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

function obtenerDataPGValido(dataHandson, soloFilaValida) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

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
    var empresa = $('#cbEmpresa').val();

    var periodo = parseInt($('#cbPeriodo').val()) || 0;

    if (empresa == "-2") empresa = "-1";
    $('#hempresa').val(empresa);
    $.ajax({
        type: 'POST',
        url: controlador + '/CargarCentrales',

        data: {
            idEmpresa: $('#hempresa').val(),
            periodo: periodo,
            tiporecurso: RECURSO_PG
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

                mostrarTabPotenciaGarantizada();
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
        url: controlador + "UploadInfoPG",
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
                    cargarTablaHandson(result);
                } else {
                    alert('Se ha producido un error al importar información.');
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

    $('#listadoHPG').html('');

    var recurso = 1;     //CAMBIAR SEGUN INSUMO LA TABLA RECURSO **************************************************************
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
                $('#listadoHPG').html(evt.Resultado);

                $("#listadoHPG").css("width", (820) + "px");
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
