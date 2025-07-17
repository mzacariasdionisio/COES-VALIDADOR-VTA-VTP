
var controlador = siteRoot + 'eventos/AnalisisFallas/';
var tblInterrupEracmf;
var tblInterrupciones;
var tblSuministros;
var containerInterrupEracmf;
var containerInterrupciones;
var containerSuministros;
var tblErroresdatos, tblEnvio;
var HEIGHT_MINIMO = 250;
var FLAG_MOSTRAR_EXITO_ENVIO = 1;

var listaErrorInterrupcion = [];

const TipoInformacion = Object.freeze({ InterrupcionActivacionERACMF: "13", Interrupcion: "14", ReduccionSuministros: "15" });

$(function () {

    $('#tab-container').easytabs({
        animate: true
    });

    $('#btnIrListado').click(function () {
        window.location.href = controlador + 'Index';
    });

    $('#btnEditarEnvio').click(function () {
        cargarInterrupcionSuministro(0);
    });

    $('#btnDescargarFormato').click(function () {
        var idTipoInformacion = $('#CboReportar').val();
        var afecodi =$("#cbFechaHoraInicio").val() ;
        var dataHandson, dataValido = [];
        switch (idTipoInformacion) {
            case TipoInformacion.InterrupcionActivacionERACMF:
                dataHandson = tblInterrupEracmf.getSourceData();
                dataValido = obtenerDataInterrupSuminValidoEracmf(dataHandson, false);
                break;
            case TipoInformacion.Interrupcion:
                dataHandson = tblInterrupciones.getSourceData();
                dataValido = obtenerDataInterrupcionValido(dataHandson, false);
                break;
            case TipoInformacion.ReduccionSuministros:
                dataHandson = tblSuministros.getSourceData();
                dataValido = obtenerDataReduccionSuministroValido(dataHandson, false);
                break;
        }

        var data = {
            emprcodi: $("#cbEmpresa").val(),
            idtipoinformacion: parseInt(idTipoInformacion),
            afecodi: afecodi,
            listaInterrupcionSuministro: dataValido
        };
        $.ajax({
            url: controlador + "DescargarFormatoInterrupcionSuministro",
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify(data),
            success: function (result) {
                switch (result.NRegistros) {
                    case 1: window.location = controlador + "Exportar?fi=" + result.Resultado; break;// si hay elementos
                    case 2: alert("No existen registros !"); break;// sino hay elementos
                    case -1: alert("Error en reporte result"); break;// Error en C#
                }
            },
            error: function (xhr, status) {
                mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
            }
        });
    });

    $("#btnEnviarDatos").click(function () {
        if (listaErrorInterrupcion.length === 0) {


            var dataHandson, dataValido = [];
            var fuentedato = $("#CboReportar").val();
            switch (fuentedato) {
                case TipoInformacion.InterrupcionActivacionERACMF:
                    dataHandson = tblInterrupEracmf.getSourceData();
                    dataValido = obtenerDataInterrupSuminValidoEracmf(dataHandson, true);
                    break;
                case TipoInformacion.Interrupcion:
                    dataHandson = tblInterrupciones.getSourceData();
                    dataValido = obtenerDataInterrupcionValido(dataHandson, true);
                    break;
                case TipoInformacion.ReduccionSuministros:
                    dataHandson = tblSuministros.getSourceData();
                    dataValido = obtenerDataReduccionSuministroValido(dataHandson, true);
                    break;
            }


            if (dataValido.length <= 0) {
                mostrarMensaje('mensajeInterr', 'alert', 'No existe como mínino un registro completo.');
                return;
            }

            var dataJson = {
                emprcodi: $("#cbEmpresa").val(),
                idtipoinformacion: fuentedato,
                afecodi: $("#cbFechaHoraInicio").val(),
                listaInterrupcionSuministro: dataValido
            };

            $.ajax({
                url: controlador + "EnvioInterrupcionSuministro",
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify(dataJson),
                success: function (result) {
                    mostrarMensaje('mensajeInterr', result.Resultado, result.StrMensaje);

                    if (result.NRegistros === 1) {
                        listaErrorInterrupcion = [];
                        cargarInterrupcionSuministro(result.Enviocodi, FLAG_MOSTRAR_EXITO_ENVIO);
                    }

                },
                error: function (xhr, status) {
                    console.log(xhr);
                    console.log(status);
                    mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
                }
            });
        } else {
            mostrarMensaje('mensajeInterr', 'error', 'Existen errores en las celdas, favor de corregir y vuelva a envíar.');
            $("#btnMostrarErrores").trigger("click");
        }
    });

    $("#btnVerEnvios").click(function () {
        openPopupCrear("enviosInterrupcion");
    });

    $("#btnMostrarErrores").click(function () {

        openPopupCrear("erroresDatos");

        var listaErrorUnic = [];

        $.each(listaErrorInterrupcion, function (key, value) {
            var result = listaErrorUnic.filter(x => x.address === value.address);
            if (result.length === 0) {
                listaErrorUnic.push(value);
            }
        });

        tblErroresdatos.clear().draw();
        tblErroresdatos.rows.add(listaErrorUnic).draw();

    });
    $("#btnEliminarEnvios").click(function () {
        $.ajax({
            url: controlador + "EliminarInterrupcionSuministro",
            data: {
                emprcodi: $("#cbEmpresa").val(),
                idtipoinformacion: $("#CboReportar").val(),
                afecodi: $("#cbFechaHoraInicio").val()
            },
            type: 'POST',
            success: function (result) {
                mostrarMensaje('mensajeInterr', result.Resultado, result.StrMensaje);
                cargarInterrupcionSuministro(0);
            },
            error: function (xhr, status) {
                mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
            }
        });

    });

    valoresInciales();
    crearPupload();

    // #region Config Handsontable

    var dtpConfig = {
        firstDay: 1,
        showWeekNumber: true,
        i18n: {
            previousMonth: 'Mes anterior',
            nextMonth: 'Mes siguiente',
            months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            weekdays: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
            weekdaysShort: ['Dom', 'Lun', 'Mar', 'Mier', 'Jue', 'Vie', 'Sab']
        }
    };

    var contexMenuConfig = {
        items: {
            'row_below': { name: "Insertar fila abajo" },
            'row_above': { name: "Insertar fila arriba" },
            'remove_row': { name: "Eliminar fila" },
            'undo': { name: "Deshacer" },
            'copy': { name: "Copiar" }
        }
    };

    // #endregion

    // #region Handsontable Interrupción Eracmf
    containerInterrupEracmf = document.getElementById('tblInterrupcionEracmf');

    tblInterrupEracmf = new Handsontable(containerInterrupEracmf, {
        dataSchema: {
            Intsumzona: null,
            Intsumempresa: null,
            Intsumsuministro: null,
            Intsumsubestacion: null,
            Intsummw: null,
            Intsumfechainterrini2: null,
            Intsumfechainterrfin2: null,
            Intsumduracion: null,
            Intsumfuncion: null,
            Intsumnumetapa: null,
            Intsumobs: null
        },
        colHeaders: ['Zona', 'Empresa', 'Circuito alimentador', 'S.E.', 'Potencia <br>(MW)', 'Fecha Inicio<br>(DD/MM/YYYY HH:mm:ss)', 'Fecha Fin<br>(DD/MM/YYYY HH:mm:ss)', 'Duración<br> (Min)', 'Función', 'Etapa', 'Observaciones'],
        columns: [
            { data: 'Intsumzona', editor: false, className: 'soloLectura' },
            { data: 'Intsumempresa', editor: false, className: 'soloLectura' },
            { data: 'Intsumsuministro', editor: false, className: 'soloLectura' },
            { data: 'Intsumsubestacion', editor: false, className: 'soloLectura' },
            { data: 'Intsummw', type: 'text', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true },
            { data: 'Intsumfechainterrini2', type: 'date', dateFormat: 'DD/MM/YYYY HH:mm:ss', correctFormat: true, datePickerConfig: dtpConfig },
            { data: 'Intsumfechainterrfin2', type: 'date', dateFormat: 'DD/MM/YYYY HH:mm:ss', correctFormat: true, datePickerConfig: dtpConfig },
            { data: 'Intsumduracion', type: 'numeric', editor: false, className: 'soloLectura htRight', renderer: duracionRenderer },
            { data: 'Intsumfuncion', type: 'dropdown', source: ['f', 'Df'], className: 'htCenter' },
            { data: 'Intsumnumetapa', editor: false, className: 'soloLectura htCenter' },
            {
                data: 'Intsumobs',
                validator: function (value, callback) {
                    if (value) {
                        if (value.length > 140) {
                            callback(false);
                        }
                    }
                    callback(true);
                }
            }
        ],
        colWidths: [60, 200, 150, 150, 60, 150, 150, 60, 70, 60, 300],
        columnSorting: true,
        minSpareRows: 0,
        rowHeaders: true,
        autoWrapRow: true,
        startRows: 0,
        manualColumnResize: true,
    });

    tblInterrupEracmf.addHook('afterRender', function () {
        tblInterrupEracmf.validateCells();
    });

    tblInterrupEracmf.addHook('afterRenderer', function (TD, row, column, prop, value, cellProperties) {
        if (prop === "Intsumnumetapa") {
            if (value > 3) {
                tblInterrupEracmf.setCellMeta(row, column - 1, 'source', ['f']);
            }
        }
    });

    tblInterrupEracmf.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrorInterrupcion = [];
    });

    tblInterrupEracmf.addHook('afterValidate', function (isValid, value, row, prop, source) {

        if (prop === "Intsummw" || prop === "Intsummwfin") {
            var result = potenciaValidator(this, isValid, value, row, prop, 0, 1000, 1);
            return registrarErrores(result);
        }

        if (prop === "Intsumfechainterrini2") {
            var result2 = fechaInicioValidator(this, isValid, value, row, prop);
            return registrarErrores(result2);
        }

        if (prop === "Intsumfechainterrfin2") {
            var result3 = fechaFinValidator(this, isValid, value, row, prop);
            return registrarErrores(result3);
        }

        if (prop === "Intsumobs") {
            var result4 = observacionValidator(this, isValid, value, row, prop);
            return registrarErrores(result4);
        }

        if (prop == "Intsumfuncion") {
            var result5 = funcionEramcfValidator(this, isValid, value, row, prop);
            return registrarErrores(result5);
        }
    });

    // #endregion

    // #region Handsontable Interrupciones

    containerInterrupciones = document.getElementById('tblInterrupciones');

    tblInterrupciones = new Handsontable(containerInterrupciones, {
        dataSchema: {
            Intsumsuministro: null,
            Intsumsubestacion: null,
            Intsummw: null,
            Intsumfechainterrini2: null,
            Intsumfechainterrfin2: null,
            Intsumduracion: null,
            Intsumobs: null
        },
        colHeaders: ['Suministro / Alimentador', 'S.E.', 'Potencia<br> (MW)', 'Fecha Inicio<br>(DD/MM/YYYY HH:mm:ss)', 'Fecha Fin<br>(DD/MM/YYYY HH:mm:ss)', 'Duración<br> (Min)', 'Observación'],
        columns: [
            { data: 'Intsumsuministro' },
            { data: 'Intsumsubestacion' },
            { data: 'Intsummw', type: 'text', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true},
            { data: 'Intsumfechainterrini2', type: 'date', dateFormat: 'DD/MM/YYYY HH:mm:ss', correctFormat: true, datePickerConfig: dtpConfig },
            { data: 'Intsumfechainterrfin2', type: 'date', dateFormat: 'DD/MM/YYYY HH:mm:ss', correctFormat: true, datePickerConfig: dtpConfig },
            { data: 'Intsumduracion', type: 'numeric', editor: false, className: 'soloLectura htCenter', renderer: duracionRenderer },
            {
                data: 'Intsumobs',
                validator: function (value, callback) {
                    if (value != null) {
                        if (value.length > 140) {                            
                            callback(false);
                        }
                    }
                    callback(true);
                }
            }
        ],
        colWidths: [200, 200, 60, 150, 150, 60, 300],
        contextMenu: contexMenuConfig,
        columnSorting: true,
        rowHeaders: true,
        autoWrapRow: true,
        startRows: 0,
        manualColumnResize: true
    });

    tblInterrupciones.addHook('afterRender', function () {
        tblInterrupciones.validateCells();
    });

    tblInterrupciones.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrorInterrupcion = [];
    });

    tblInterrupciones.addHook('afterValidate', function (isValid, value, row, prop, source) {
        if (prop === "Intsummw") {
            var result = potenciaValidator(this, isValid, value, row, prop, 0, 1000, 0);
            return registrarErrores(result);
        }

        if (prop === "Intsumfechainterrini2") {
            var result2 = fechaInicioValidator(this, isValid, value, row, prop);
            return registrarErrores(result2);
        }

        if (prop === "Intsumfechainterrfin2") {
            var result3 = fechaFinValidator(this, isValid, value, row, prop);
            return registrarErrores(result3);
        }

        if (prop === "Intsumobs") {
            var result4 = observacionValidator(this, isValid, value, row, prop);
            return registrarErrores(result4);
        }
    });

    // #endregion

    // #region Handsontable Suministros

    containerSuministros = document.getElementById('tblSuministros');

    tblSuministros = new Handsontable(containerSuministros, {
        dataSchema: {
            Intsumsuministro: null,
            Intsumsubestacion: null,
            Intsummw: null,
            Intsummwfin: null,
            Intsummwred: null,
            Intsumfechainterrini2: null,
            Intsumfechainterrfin2: null,
            Intsumduracion: null
        },
        colHeaders: ['Suministro / Circuito', 'S.E.', 'De<br> (MW)', 'A<br> (MW)', 'Reducción <br>(MW)', 'Fecha Inicio<br>(DD/MM/YYYY HH:mm:ss)', 'Fecha Fin<br>(DD/MM/YYYY HH:mm:ss)', 'Duración<br> (Min)'],
        columns: [
            { data: 'Intsumsuministro' },
            { data: 'Intsumsubestacion' },
            { data: 'Intsummw', type: 'text', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true},
            { data: 'Intsummwfin', type: 'text', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true },
            { data: 'Intsummwred', editor: false, className: 'soloLectura htRight', renderer: reduccionRenderer },
            { data: 'Intsumfechainterrini2', type: 'date', dateFormat: 'DD/MM/YYYY HH:mm:ss', correctFormat: true, datePickerConfig: dtpConfig },
            { data: 'Intsumfechainterrfin2', type: 'date', dateFormat: 'DD/MM/YYYY HH:mm:ss', correctFormat: true, datePickerConfig: dtpConfig },
            { data: 'Intsumduracion', type: 'numeric', editor: false, className: 'soloLectura htCenter', renderer: duracionRenderer }
        ],
        colWidths: [200, 200, 60, 60, 60, 150, 150, 60],
        contextMenu: contexMenuConfig,
        columnSorting: true,
        rowHeaders: true,
        autoWrapRow: true,
        startRows: 0,
        manualColumnResize: true,
    });

    tblSuministros.addHook('afterRender', function () {
        tblSuministros.validateCells();
    });

    tblSuministros.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrorInterrupcion = [];
    });

    tblSuministros.addHook('afterValidate', function (isValid, value, row, prop, source) {

        if (prop === "Intsummw") {
            var result = potenciaValidator(this, isValid, value, row, prop, 0, 280, 0);
            return registrarErrores(result);
        }

        if (prop === "Intsumfechainterrini2") {
            var result2 = fechaInicioValidator(this, isValid, value, row, prop);
            return registrarErrores(result2);
        }

        if (prop === "Intsumfechainterrfin2") {
            var result3 = fechaFinValidator(this, isValid, value, row, prop);
            return registrarErrores(result3);
        }

        if (prop === "Intsummwfin") {
            var result4 = intsummwfinValidator(this, isValid, value, row, prop);
            return registrarErrores(result4);
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

    // #endregion

    // #region Tabla envio

    tblEnvio = $('#tablalenvio').DataTable({
        data: [],
        columns: [
            { data: "Enviocodi", width: "30%" },
            { data: "Enviofecha2", width: "40%" },
            { data: "Lastuser", width: "30%" },
            { data: "Envioplazo", width: "30%" }
        ],
        rowCallback: function (row, data) { },
        filter: false,
        info: false,
        processing: true,
        scrollCollapse: true,
        paging: false,
        autoWidth: true,
        ordering: false
    });

    $('#tablalenvio tbody').on('click', 'tr', function () {
        var data = tblEnvio.row(this).data();
        var idEnvio = data.Enviocodi;
        $('#enviosInterrupcion').bPopup().close();
        cargarInterrupcionSuministro(idEnvio);
    });

    // #endregion

    $("#CboReportar").val(TipoInformacion.Interrupcion);
    $("#CboReportar").change(function () {      
        cargarInterrupcionSuministro(0);
    });

    $("#cbEmpresa").change(function () {
        cargarInterrupcionSuministro(0);
    });

    $("#cbFechaHoraInicio").change(function () {
        mostrarTipo();
    });

    mostrarTipo()
    mostrarTabInterrupcion();

  
});


function mostrarTipo() {
    var select = document.getElementById("CboReportar"); //Seleccionamos el select
    $.ajax({
        type: 'POST',
        url: controlador + 'ListarTipoInterrupcion',
        data: {
            afecodi: $("#cbFechaHoraInicio").val()
        },
        success: function (evtResult) {
            $("#CboReportar").empty();
            evt = evtResult;
            for (var x = 0; x < evt.ListaTipoInformacion.length; x++) {
                var option = document.createElement("option"); //Creamos la opcion
                option.value = evt.ListaTipoInformacion[x].Fdatcodi;
                option.text = evt.ListaTipoInformacion[x].Fdatnombre
                select.appendChild(option); //Metemos la opción en el select
            }

            cargarInterrupcionSuministro(0);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

// #region Metodo

function registrarErrores(result) {
    if (!result.valid)
        listaErrorInterrupcion.push(result.error);
    return result.valid;
}

function crearPupload() {

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnImportar",
        url: controlador + "UploadInterrupcionSuministro",
        multi_selection: false,
        filters: {
            max_file_size: 0,
            mime_types: [
                { title: "Archivos xls", extensions: "xls,xlsx" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length === 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensajeInterr', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },

            FileUploaded: function (up, file, info) {
                var result = JSON.parse(info.response);
                if (result.NRegistros != -1) {
                    mostrarMensaje('mensajeInterr', 'exito', "<strong>Los datos se cargaron correctamente en el excel web, presione el botón enviar para grabar.</strong>");
                    cargarHansonTablePorFuenteDato(result);
                } else {
                    mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
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

    uploader.bind('BeforeUpload', function (up, file) {
        up.settings.multipart_params = {
            emprcodi: $("#cbEmpresa").val(),
            afecodi: $("#cbFechaHoraInicio").val(),
            idtipoinformacion: $('#CboReportar').val()
        };
    });

    uploader.init();
}

function valoresInciales() {

}

function hiddenColumnsHandsontable(hot, columns) {
    var plugin = hot.getPlugin('hiddenColumns');
    plugin.hideColumns(columns);
    hot.render();
}

function showColumnsHandsontable(hot, columns) {
    var plugin = hot.getPlugin('hiddenColumns');
    plugin.showColumns(columns);
    hot.render();
}

function recalcularColumnas() {
    tblInterrupEracmf.getPlugin('AutoColumnSize').clearCache().recalculateAllColumnsWidth();
    tblInterrupEracmf.render();
}

function cargarInterrupcionSuministro(idEnvio, flagMostrarExitoEnvio) {
    idEnvio = parseInt(idEnvio) || 0;
    flagMostrarExitoEnvio = parseInt(flagMostrarExitoEnvio) || 0;
    mostrarMensaje('mensajeInterr', '', '');
    listaErrorInterrupcion = [];
    $("#barraHerramientas").hide();
    $("#excelwebSuministro").hide();
    $("#contentEracmf,#contentInterrupciones,#contentSuministros").hide();
    $(".leyenda_excel").hide();

    $.ajax({
        url: controlador + "CargarInterrupcionSuministro",
        data: {
            emprcodi: $("#cbEmpresa").val(),
            //afecodi: $('#txtAfecodi').val(),
            fdatcodi: $("#CboReportar").val(),
            afecodi: $("#cbFechaHoraInicio").val(),
            enviocodi: idEnvio
        },
        type: 'POST',
        success: function (result) {
            if (result.NRegistros === -1) {
                mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
            } else {
                $("#txtFechaInterrupcion").val(result.oEventoDTO.FechaInterrupcion)
                validarPlazoHtml(result, idEnvio, flagMostrarExitoEnvio);
                cargarHansonTablePorFuenteDato(result);

                var lstEnvio = result.ListaMeEnvio;
                tblEnvio.clear().draw();
                tblEnvio.rows.add(lstEnvio).draw();
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
        }
    }); 
}

function mostrarTabInterrupcion() {
    var data = $("#cbEmpresa").val();

    if (data) {
        if (parseInt(data) > 0) {
            $("#tabInterrup").show();
            $("#tabs2").show();
            $('#tab-container').easytabs('select', '#tabs2');
            cargarInterrupcionSuministro();
        } else {
            $('#tab-container').easytabs('select', '#tabs1');
        }
    } else {
        $('#tab-container').easytabs('select', '#tabs1');
    }
}

function validarPlazoHtml(result, idEnvio, flagMostrarExitoEnvio) {

    var evento = result.oEventoDTO;
    var mensaje = "Por favor seleccione empresa y tipo información a reportar.";
    var tipoMensaje = 'message';
    if (evento) {
        $("#barraHerramientas").css('display', 'inline-block');
        $("#excelwebSuministro").show();

        var mensajePlazo = "";
        if (idEnvio > 0) {
            var plazo = evento.EnPlazo ? "<strong style='color:green'>En plazo</strong>" : "<strong style='color:red'>Fuera de plazo</strong>";
            if (flagMostrarExitoEnvio == 0) {
                mensaje = "<strong>Código de envío</strong> : " + idEnvio
                    + ", <strong>Fecha de envío: </strong>" + result.FechaEnvio
                    + ", <strong>Enviado </strong>" + plazo + ".";
            } else {
                tipoMensaje = 'exito';
                mensaje = "Los datos se enviaron correctamente. <strong>Código de envío</strong>: " + idEnvio
                    + ", <strong>Fecha de envío: </strong>" + result.FechaEnvio
                    + ", <strong>Enviado </strong>" + plazo + ".";
            }
        } else {
            if (!evento.EmpresaActivo) {
                mensajePlazo = " La empresa se encuentra <strong style = 'color:blue' > No Vigente</strong>.";
            } else if (evento.Deshabilidado) {
                mensajePlazo = " <strong>Plazo del Envio: </strong> <strong style='color:blue'>Deshabilitado.</strong>";
            } else if (evento.EnPlazo) {
                mensajePlazo = " <strong>Plazo del Envio: </strong> <strong style='color:green'>En plazo.</strong>";
            } else if (!evento.EnPlazo) {
                mensajePlazo = " <strong>Plazo del Envio: </strong> <strong style='color:red'>Fuera de plazo.</strong>";
            }
        }

        $("#btnEditarEnvio,#btnEnviarDatos,#btnDescargarFormat,#btnImportar,#btnMostrarErrores,#btnEliminarEnvios").parent().parent().hide();
        desabilitarHansontables(tblInterrupEracmf);
        desabilitarHansontables(tblInterrupciones);
        desabilitarHansontables(tblSuministros);
        if (!evento.EmpresaActivo || evento.Deshabilidado) {
        } else {
            if (idEnvio > 0) {
                $("#btnEditarEnvio,#btnDescargarFormato,#btnEliminarEnvios").parent().parent().show();
            }
            else {
                $("#btnEnviarDatos,#btnImportar,#btnMostrarErrores,#btnEliminarEnvios").parent().parent().show();
                $(".leyenda_excel").show();
                habilitarHansontables(tblInterrupEracmf);
                habilitarHansontables(tblInterrupciones);
                habilitarHansontables(tblSuministros);
            }
        }

        mostrarMensaje('mensajeInterrPlazo', tipoMensaje, mensaje + mensajePlazo);
    }
}

function cargarHansonTable(tblInterrSumin, inputJson) {
    var lstInterrSumin = inputJson && inputJson.length > 0 ? inputJson : [];

    var fechaInterrupcion = $("#txtFechaInterrupcion").val();

    var lstData = [];
    tblInterrSumin.loadData(lstData);

    for (var index in lstInterrSumin) {

        var item = lstInterrSumin[index];
        var fechaDefault = item.Intsumfechainterrini2 != null && item.Intsumfechainterrini2 != '' ? item.Intsumfechainterrini2 : fechaInterrupcion;

        var data = {
            Intsumzona: item.Intsumzona,
            Intsumempresa: item.Intsumempresa,
            Intsumsuministro: item.Intsumsuministro,
            Intsumsubestacion: item.Intsumsubestacion,
            Intsummw: item.Intsummw,
            Intsummwfin: item.Intsummwfin,
            Intsummwred: item.Intsummwred,
            Intsumfechainterrini2: fechaDefault,
            Intsumfechainterrfin2: item.Intsumfechainterrfin2,
            Intsumduracion: item.Intsumduracion,
            Intsumfuncion: item.Intsumfuncion,
            Intsumnumetapa: item.Intsumnumetapa,
            Intsumobs: item.Intsumobs
        };

        lstData.push(data);
    }
    tblInterrSumin.loadData(lstData);
}

function cargarHansonTable2(tblInterrSumin, inputJson) {
    var lstInterrSumin = inputJson && inputJson.length > 0 ? inputJson : [];

    var fechaInterrupcion = $("#txtFechaInterrupcion").val();

    var lstData = [];
    tblInterrSumin.loadData(lstData);

    for (var index in lstInterrSumin) {

        var item = lstInterrSumin[index];

        var data = {
            Intsumsuministro: item.Intsumsuministro,
            Intsumsubestacion: item.Intsumsubestacion,
            Intsummw: item.Intsummw2,
            Intsummwfin: item.Intsummwfin2,
            Intsumfechainterrini2: item.Intsumfechainterrini2,
            Intsumfechainterrfin2: item.Intsumfechainterrfin2,
            Intsumobs: item.Intsumobs
        };

        lstData.push(data);
    }

    if (lstData.length < 5) {
        for (var i = lstData.length; i <= 5; i++) {
            lstData.push({ Intsumfechainterrini2: fechaInterrupcion });
        }
    }

    tblInterrSumin.loadData(lstData);
}

function obtenerDataInterrupSuminValidoEracmf(dataHandson, soloFilaValida) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

        item.Intsummw = parseFloat(item.Intsummw) || 0.0; //el campo es texto, para enviar al controller se castea a float

        if (soloFilaValida) {
            if (item.Intsummw >= 0 && item.Intsumfechainterrini2 && item.Intsumfechainterrfin2 && item.Intsumfuncion) {
                lstData.push(item);
            }
        } else {
            lstData.push(item);
        }
    }
    return lstData;
}

function obtenerDataInterrupcionValido(dataHandson, soloFilaValida) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

        item.Intsummw = parseFloat(item.Intsummw) || 0.0; //el campo es texto, para enviar al controller se castea a float

        if (soloFilaValida) {
            if (item.Intsumsuministro && item.Intsumsubestacion && item.Intsummw >= 0 && item.Intsumfechainterrini2 && item.Intsumfechainterrfin2) {
                lstData.push(item);
            }
        } else {
            lstData.push(item);
        }
    }
    return lstData;
}

function obtenerDataReduccionSuministroValido(dataHandson, soloFilaValida) {

    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

        item.Intsummw = parseFloat(item.Intsummw) || 0.0; //el campo es texto, para enviar al controller se castea a float
        item.Intsummwfin = parseFloat(item.Intsummwfin) || 0.0; //el campo es texto, para enviar al controller se castea a float

        if (soloFilaValida) {
            if (item.Intsumsuministro && item.Intsumsubestacion && item.Intsummw >= 0 && item.Intsummwfin >= 0 && item.Intsumfechainterrini2 && item.Intsumfechainterrfin2) {
                lstData.push(item);
            }
        } else {
            lstData.push(item);
        }
    }
    return lstData;
}

function cargarHansonTablePorFuenteDato(result) {
    hansonTableClear();
    switch ($("#CboReportar").val()) {
        case TipoInformacion.InterrupcionActivacionERACMF:
            $("#contentEracmf").show();
            cargarHansonTable(tblInterrupEracmf, result.ListaInterrupSuministro);
            updateDimensionHandson(tblInterrupEracmf, containerInterrupEracmf);
            break;
        case TipoInformacion.Interrupcion:
            $("#contentInterrupciones").show();
            cargarHansonTable2(tblInterrupciones, result.ListaInterrupSuministro);
            updateDimensionHandson(tblInterrupciones, containerInterrupciones);
            break;
        case TipoInformacion.ReduccionSuministros:
            $("#contentSuministros").show();
            cargarHansonTable2(tblSuministros, result.ListaInterrupSuministro);
            updateDimensionHandson(tblSuministros, containerSuministros);
            break;
    }
}

function openPopupCrear(contentPopup) {

    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);

}

function hansonTableClear() {

    var fechaInterrupcion = $("#txtFechaInterrupcion").val();

    switch ($("#CboReportar").val()) {
        case TipoInformacion.InterrupcionActivacionERACMF:
            tblInterrupEracmf.loadData([]);
            break;
        case TipoInformacion.Interrupcion:
            tblInterrupciones.loadData([{ Intsumfechainterrini2: fechaInterrupcion }, { Intsumfechainterrini2: fechaInterrupcion }, { Intsumfechainterrini2: fechaInterrupcion }, { Intsumfechainterrini2: fechaInterrupcion }, { Intsumfechainterrini2: fechaInterrupcion }]);
            break;
        case TipoInformacion.ReduccionSuministros:
            tblSuministros.loadData([{ Intsumfechainterrini2: fechaInterrupcion }, { Intsumfechainterrini2: fechaInterrupcion }, { Intsumfechainterrini2: fechaInterrupcion }, { Intsumfechainterrini2: fechaInterrupcion }, { Intsumfechainterrini2: fechaInterrupcion }]);
            break;
    }
}

function desabilitarHansontables(hanson) {
    hanson.updateSettings({
        readOnly: true
    });
}

function habilitarHansontables(hanson) {
    hanson.updateSettings({
        readOnly: false
    });
}

function updateDimensionHandson(hot, container) {
    if (hot !== undefined && hot != null) {
        var offset = {};
        try {
            offset = Handsontable.Dom.offset(container);
        }
        catch (err) {
            console.log(err);
        }

        if (offset.length != 0) {
            var widthHT;
            var heightHT;
            var offsetTop = parseInt(offset.top) || 0;

            if (offset.top == 222) {
                heightHT = $(window).height() - 140 - offset.top - 20;
            }
            else {
                heightHT = $(window).height() - 140 - offset.top - 20;
            }

            heightHT = HEIGHT_MINIMO;
            if (offset.left > 0 && offset.top > 0) {
                widthHT = $(window).width() - 2 * offset.left; // $("#divGeneral").width() - 50; //
                hot.updateSettings({
                    width: widthHT,
                    height: heightHT
                });
            } else {
                console.log('updateDimensionHandson');
            }
        }
    }
}

function duracionRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    var stfechaIni = instance.getDataAtCell(row, col - 2);
    var stfechaFin = instance.getDataAtCell(row, col - 1);

    if (moment(stfechaIni, 'DD/MM/YYYY HH:mm:ss').isValid() && moment(stfechaFin, 'DD/MM/YYYY HH:mm:ss').isValid()) {

        var fechaIni = moment(stfechaIni, 'DD/MM/YYYY HH:mm:ss');
        var fechaFin = moment(stfechaFin, 'DD/MM/YYYY HH:mm:ss');
        var duracionMinuto = fechaFin.diff(fechaIni, 'minutes', true);

        td.innerHTML = duracionMinuto.toFixed(2);
    }
}

function reduccionRenderer(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    var valDe = instance.getDataAtCell(row, col - 2);
    var valA = instance.getDataAtCell(row, col - 1);

    if (valDe && valA) {

        var valReduccion = valDe - valA;
        td.innerHTML = valReduccion.toFixed(3);
    }
}

function potenciaValidator(instance, isValid, value2, row, prop, valMin, valMax, tipoInterrup) {
    var error = [];

    var columnName = instance.getColHeader(instance.propToCol(prop));

    var className, mensaje;

    value = value2;
    if (value2 != undefined && value2 != null)
        value = value + '';

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

        if (tipoInterrup == 1) {
            var observ = instance.getDataAtCell(row, instance.propToCol(prop) + 6);
            if (value == 0 && !observ) {
                className = "htInvalid";
                mensaje = "Debe ingresar almenos una observación";
                isValid = false;
            }
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

            //quitar <br>
            var regex = /<br\s*[\/]?>/gi;
            columnName = columnName != null ? columnName.replace(regex, "") : "";

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

function fechaInicioValidator(instance, isValid, value, row, prop) {

    var error = [];

    if (value) {
        var fechaIni = moment(value, 'DD/MM/YYYY HH:mm:ss');
        if (fechaIni.isValid()) {
            var fechaInterrupcion = moment($("#txtFechaInterrupcion").val(), 'DD/MM/YYYY HH:mm:ss');
            var numMaxSeg = parseInt($("#txtMaxSegFechaIni").val()) || 0;
            var fechaMaxInicio = moment(fechaInterrupcion).add(numMaxSeg, 'seconds');

            if (fechaInterrupcion.isValid()) {
                var isValid1 = fechaIni >= fechaInterrupcion;
                var isValid2 = fechaIni <= fechaMaxInicio;
                //var columnName = instance.getColHeader(instance.propToCol(prop));
                if (!isValid1) {
                    error = {
                        address: `[<b>Fecha inicio</b>] [<b>${row + 1}</b>]`,
                        valor: value,
                        className: "htInvalid",
                        message: "El dato es anterior a la fecha y hora de inicio de interrupción"
                    };
                }
                if (!isValid2) {
                    error = {
                        address: `[<b>Fecha inicio</b>] [<b>${row + 1}</b>]`,
                        valor: value,
                        className: "htInvalid",
                        message: "El dato no puede ser mayor en " + numMaxSeg + " segundos a la fecha y hora de inicio de interrupción"
                    };
                }

                isValid = isValid1 && isValid2;
            }
        } else {
            isValid = false;
            error = {
                address: `[<b>Fecha inicio</b>] [<b>${row + 1}</b>]`,
                valor: value,
                className: "htInvalid",
                message: "El dato no es de tipo fecha"
            };
        }
    }
    return { valid: isValid, error: error };
}

function fechaFinValidator(instance, isValid, value, row, prop) {

    var error = [];

    if (!isValid) {
        error = {
            address: `[<b>Fecha fin</b>] [<b>${row + 1}</b>]`,
            valor: value,
            className: "htInvalid",
            message: "El dato no es de tipo fecha"
        };
        return { valid: isValid, error: error };
    }

    if (value) {
        var fechaIni = moment(instance.getDataAtCell(row, instance.propToCol(prop) - 1), 'DD/MM/YYYY HH:mm:ss');
        if (fechaIni.isValid()) {
            var fechaFin = moment(value, 'DD/MM/YYYY HH:mm:ss');
            if (fechaFin.isValid()) {
                isValid = fechaIni < fechaFin;

                //var columnName = instance.getColHeader(instance.propToCol(prop));
                error = {
                    address: `[<b>Fecha fin</b>] [<b>${row + 1}</b>]`,
                    valor: value,
                    className: "htInvalid",
                    message: "El dato debe ser mayor a la fecha inicio"
                };
            }
        }
    }
    return { valid: isValid, error: error };
}

function observacionValidator(instance, isValid, value, row, prop) {
    var error = [];

    if (value) {
        if (value.length > 140) {
            isValid = false;
            error = {
                address: `[<b>Observación</b>] [<b>${row + 1}</b>]`,
                valor: value,
                className: "htInvalid",
                message: "Solamente permite hasta 140 caracteres"
            };
        }
    }
    return { valid: isValid, error: error };
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


function intsummwfinValidator(instance, isValid, value, row, prop) {
    var error = [];

    if (value) {

        var valorDe = instance.getDataAtCell(row, instance.propToCol(prop) - 1);
        if (valorDe) {
            var valorDeFloat = parseFloat(valorDe) || 0;
            var valorAFloat = parseFloat(value) || 0;
            if (valorDeFloat <= valorAFloat) {
                isValid = false;
                error = {
                    address: `[<b>A (MW)</b>] [<b>${row + 1}</b>]`,
                    valor: value,
                    className: "htInvalid",
                    message: "El dato debe ser menor que “De (MW)”"
                };
            }
        }

    }
    return { valid: isValid, error: error };
}

function funcionEramcfValidator(instance, isValid, value, row, prop) {
    var error = [];

    isValid = true;
    if (value) {
        if (value != 'f' && value != 'Df') {
            isValid = false;
            error = {
                address: `[<b>Función</b>] [<b>${row + 1}</b>]`,
                valor: value,
                className: "htInvalid",
                message: "La función debe ser f o Df"
            };
        }
    }
    return { valid: isValid, error: error };
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

// #endregion

