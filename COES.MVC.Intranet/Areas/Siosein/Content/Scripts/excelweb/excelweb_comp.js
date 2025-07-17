var controlador = siteRoot + 'Siosein/TablasPrieDeclaracionMen/'

var HOT_TABLA_WEB;
var CONTAINER_TABLA_WEB;
var tblErroresdatos;
var HEIGHT_MINIMO = 650;
var FLAG_MOSTRAR_EXITO_ENVIO = 1;
var widthLayout;

var LISTA_ERROR = [];

$(function () {
    $('#fecha_proceso').Zebra_DatePicker({
        format: 'm Y'
    });

    $("#btnVolverListado").click(function () {
        var periodo = $("#fecha_proceso").val();
        window.location.href = siteRoot + "Siosein/RemisionesOsinergmin/ListaTablasPrie?periodo=" + periodo;
    });

    $('#btnDescargarFormato').click(function () {
        descargarFormato();
    });

    $("#btnEnviarDatos").click(function () {
        guardarDatos();
    });

    $("#btnVerEnvios").click(function () {
        openPopupCrear("enviosInterrupcion");
    });

    $("#btnImportarAppTransferencia").click(function () {
        listarTablaCOMP(1);
    });

    $("#btnMostrarErrores").click(function () {

        openPopupCrear("erroresDatos");

        var listaErrorUnic = [];

        $.each(LISTA_ERROR, function (key, value) {
            var result = listaErrorUnic.filter(x => x.address === value.address);
            if (result.length === 0) {
                listaErrorUnic.push(value);
            }
        });

        tblErroresdatos.clear().draw();
        tblErroresdatos.rows.add(listaErrorUnic).draw();

    });

    widthLayout = $("#mainLayout").width() - 50;

    valoresInciales();
    crearPupload();

    configurarHandontable();

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

    listarTablaCOMP(0);
});

function listarTablaCOMP(esDefault) {
    esDefault = parseInt(esDefault) || 0;
    mostrarMensaje('mensajeInterr', '', '');

    LISTA_ERROR = [];
    $("#barraHerramientas").hide();
    $(".leyenda_excel").hide();
    $("#div_ultimo_envio").hide();

    $.ajax({
        url: controlador + "ListarTablaCOMP",
        data: {
            fechaPeriodo: $("#fecha_proceso").val(),
            esDefault: esDefault
        },
        type: 'POST',
        success: function (result) {

            var widthList = `${widthLayout}px`;
            $('#excelweb').css("width", widthList);

            if (result.NRegistros === -1) {
                mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
            } else {
                mostrarMensajeEnvioExcelWeb(esDefault, result.UsuarioEnvio, result.FechaEnvio);
                cargarHansonTablePorFuenteDato(result);
            }
        },
        error: function (xhr, status) {
            mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
        }
    });
}

function mostrarMensajeEnvioExcelWeb(esDefault, usuarioEnvio, fechaEnvio) {
    if (esDefault == 0) {
        var msj = `
            <b>No se ha realizado envío para el periodo seleccionado.</b>
        `;
        if (usuarioEnvio != '') {
            msj = `
            Datos de envío. Usuario: <b>${usuarioEnvio}</b>, fecha y hora: <b>${fechaEnvio}</b>.
        `;
        }

        $("#div_ultimo_envio").html(msj);

        $("#div_ultimo_envio").show();
    }
}

function configurarHandontable() {

    // #region Handsontable Interrupción Eracmf
    CONTAINER_TABLA_WEB = document.getElementById('tblExcelWeb');

    HOT_TABLA_WEB = new Handsontable(CONTAINER_TABLA_WEB, {
        dataSchema: {
            Emprnomb: null,
            Emprcodosinergmin: null,
            Emprcodi: null,
            Tbcompte: null,
            Tbcomppsr: null,
            Tbcomprscd: null,
            Tbcomprscul: null,
            Tbcompcbec: null,
            Tbcompcrf: null,
            Tbcompcio: null,
            Tbcompcpa: null,
            Tbcompsma: null,
            Tbcompoc: null,
        },
        colHeaders: ['Empresa', 'Código', 'Código',
            'Transferencias <br/>de Energía ', 'Prorrateo del <br/>Saldo Resultante ', 'Retiros <br/>Sin Contratos <br/>de Distribuidores',
            'Retiros <br/>Sin Contratos <br/>de Usuarios Libres', 'Compensación por <br/>Baja Eficiencia <br/>del Combustible ', 'Compensación por <br/>Regulación de Frecuencia ',
            'Compensación por <br/>Inflexibilidad Operativa', 'Compensación Por <br/>Pruebas Aleatorias', 'Saldo del <br/>Meses Anteriores ',
            'Otras Compensaciones '],
        columns: [
            { data: 'Emprnomb', editor: false, className: 'soloLectura' },
            { data: 'Emprcodosinergmin', editor: false, className: 'soloLectura' },
            { data: 'Emprcodi', editor: false, className: 'soloLectura' },
            { data: 'Tbcompte', type: 'numeric', numericFormat: { pattern: '0.000', culture: 'es-PE' }, },
            { data: 'Tbcomppsr', type: 'numeric', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true, },
            { data: 'Tbcomprscd', type: 'numeric', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true },
            { data: 'Tbcomprscul', type: 'numeric', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true },
            { data: 'Tbcompcbec', type: 'numeric', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true },
            { data: 'Tbcompcrf', type: 'numeric', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true },
            { data: 'Tbcompcio', type: 'numeric', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true },
            { data: 'Tbcompcpa', type: 'numeric', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true },
            { data: 'Tbcompsma', type: 'numeric', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true },
            { data: 'Tbcompoc', type: 'numeric', className: 'htRight', validator: numeroDecimalValidator, allowInvalid: true },
        ],
        colWidths: [340, 50, 1, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150],
        columnSorting: true,
        minSpareRows: 0,
        rowHeaders: true,
        autoWrapRow: false,
        manualColumnResize: true,
    });

    // #endregion

}

function descargarFormato() {
    var dataHandson, dataValido = [];

    var dataHandson = HOT_TABLA_WEB.getSourceData();
    var dataValido = obtenerDataValidoExcelWeb(dataHandson, false);

    var data = {
        fechaPeriodo: $("#fecha_proceso").val(),
        listaData: dataValido
    };

    $.ajax({
        url: controlador + "DescargarPlantillaTablaCOMP",
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
}

function guardarDatos() {
    if (LISTA_ERROR.length === 0) {
        var dataHandson = HOT_TABLA_WEB.getSourceData();
        var dataValido = obtenerDataValidoExcelWeb(dataHandson, true);

        if (dataValido.length <= 0) {
            mostrarMensaje('mensajeInterr', 'alert', 'No existe como mínino un registro completo.');
            return;
        }

        var dataJson = {
            fechaPeriodo: $("#fecha_proceso").val(),
            listaData: dataValido
        };

        $.ajax({
            url: controlador + "GuardarTablaCOMP",
            type: 'POST',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify(dataJson),
            success: function (result) {
                mostrarMensaje('mensajeInterr', 'exito', 'Los datos se guardaron correctamente');

                if (result.NRegistros === 1) {
                    LISTA_ERROR = [];
                    listarTablaCOMP(0);
                }
            },
            error: function (xhr, status) {
                mostrarMensaje('mensajeInterr', 'error', 'Se ha producido un error.');
            }
        });
    } else {
        mostrarMensaje('mensajeInterr', 'error', 'Existen errores en las celdas, favor de corregir y vuelva a envíar.');
        $("#btnMostrarErrores").trigger("click");
    }
}

// #region Metodo

function registrarErrores(result) {
    if (!result.valid)
        LISTA_ERROR.push(result.error);
    return result.valid;
}

function crearPupload() {

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnImportar",
        url: controlador + "UploadTablaCOMP",
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
            afecodi: $('#txtAfecodi').val(),
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
    HOT_TABLA_WEB.getPlugin('AutoColumnSize').clearCache().recalculateAllColumnsWidth();
    HOT_TABLA_WEB.render();
}

function cargarHansonTablePorFuenteDato(result) {
    hansonTableClear();

    $("#barraHerramientas").css('display', 'inline-block');
    $("#excelweb").show();
    $("#contentTbl09").show();
    cargarHansonTable(HOT_TABLA_WEB, result.ListaData);
    updateDimensionHandson(HOT_TABLA_WEB, CONTAINER_TABLA_WEB);
}

function cargarHansonTable(tblHot, inputJson) {
    var lstJson = inputJson && inputJson.length > 0 ? inputJson : [];

    var lstData = [];
    tblHot.loadData(lstData);

    for (var index in lstJson) {

        var item = lstJson[index];

        var data = {
            Emprnomb: item.Emprnomb,
            Emprcodosinergmin: item.Emprcodosinergmin,
            Emprcodi: item.Emprcodi,
            Tbcompte: item.Tbcompte,
            Tbcomppsr: item.Tbcomppsr,
            Tbcomprscd: item.Tbcomprscd,
            Tbcomprscul: item.Tbcomprscul,
            Tbcompcbec: item.Tbcompcbec,
            Tbcompcrf: item.Tbcompcrf,
            Tbcompcio: item.Tbcompcio,
            Tbcompsma: item.Tbcompsma,
            Tbcompoc: item.Tbcompoc,
            Tbcompcpa: item.Tbcompcpa,
        };

        lstData.push(data);
    }
    tblHot.loadData(lstData);
    tblHot.render();

}

function obtenerDataValidoExcelWeb(dataHandson, soloFilaValida) {
    var lstData = [];
    for (var index in dataHandson) {
        var item = dataHandson[index];

        //item.Intsummw = parseFloat(item.Intsummw) || 0.0; //el campo es texto, para enviar al controller se castea a float

        //if (soloFilaValida) {
        //    if (item.Intsummw >= 0 && item.Intsumfechainterrini2 && item.Intsumfechainterrfin2 && item.Intsumfuncion) {
        //        lstData.push(item);
        //    }
        //} else {
        //    lstData.push(item);
        //}

        lstData.push(item);
    }
    return lstData;
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
    HOT_TABLA_WEB.loadData([]);
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
                widthHT = $(window).width() - 1 * offset.left; // $("#divGeneral").width() - 50; //
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

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

// #endregion

