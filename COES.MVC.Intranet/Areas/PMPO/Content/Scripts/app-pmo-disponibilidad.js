var controlador = siteRoot + 'PMPO/GeneracionArchivosDAT/';

var DESCRIPCION_HANDSON = [];
var hot;
$(document).ready(function () {
    cargarHandsontableDisp();
    Importar();

    $('#btnRetornar').click(function () {
        var periodo = $("#hddPeriodo").val();
        location.href = controlador + "Index?periodo=" + periodo;
    });
});

function cargarHandsontableDisp() {

    var periodo = $("#hddPeriodo").val();
    var formato = $("#hddFormato").val();

    $.ajax({
        type: 'Post',
        url: controlador + "ListarDisponibilidad",
        dataType: 'json',
        cache: false,
        data: {
            periodo: periodo,
            tipoFormato: formato
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                armarHandsontable(model);
            } else {
                mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
                $("#mensaje").show();
            }
        },
        error: function (err) {
            mostrarError();
        }
    });
}

function grabar() {
    $("#mensaje").hide();
    var periodo = $("#hddPeriodo").val();
    var formato = $("#hddFormato").val();

    $.ajax({
        type: "POST",
        url: controlador + "GrabarDataDisponibilidad",
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            periodo: periodo,
            tipoFormato: formato,
            datos: hot.getData()
        }),
        beforeSend: function () {
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                mostrarMensaje("mensaje", "Se guardó los datos exitosamente", $tipoMensajeExito, $modoMensajeCuadro);
                cargarHandsontableDisp();
            } else {
                mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
                $("#mensaje").show();
            }
        },
        error: function (err) {
            mostrarMensaje("mensaje", 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            $("#mensaje").show();
        }
    });
}

function openPopupMantto() {
    $('#popupExportarMantto').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown',
        modalClose: false
    });
}

function ExportarMantenimientos() {
    $("#mensaje").hide();

    var periodo = $("#hddPeriodo").val();
    var formato = $("#hddFormato").val();

    var tipoReporteMantto = $('input[name="rbMantto"]:checked').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarMantenimientos',
        dataType: 'json',
        data: {
            periodo: periodo,
            tipo: formato,
            tipoReporteMantto
        },
        success: function (model) {
            $('#popupExportarMantto').bPopup().close();

            if (model.Resultado != "-1") {
                document.location.href = controlador + 'descargar?formato=' + 1 + '&file=' + model.Archivo
                mostrarMensaje("mensaje", "Exportación realizada.", $tipoMensajeExito, $modoMensajeCuadro);
                $("#mensaje").show();
            }
            else {
                mostrarMensaje("mensaje", model.Mensaje, $tipoMensajeError, $modoMensajeCuadro);
                $("#mensaje").show();
            }
        },
        error: function (err) {
            mostrarMensaje("mensaje", 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            $("#mensaje").show();
        }
    });
}

function Exportar() {
    var periodo = $("#hddPeriodo").val();
    var formato = $("#hddFormato").val();

    location.href = controlador + "ExportarDataDisponibilidad?periodo=" + periodo + "&tipoFormato=" + formato;
}

function armarHandsontable(result) {
    var container = document.getElementById('handson');
    DESCRIPCION_HANDSON = null; //result.descripcionHandson;

    var hotSettings = {
        data: result.Data,
        //stretchH: 'all',
        //width: 1050,
        autoWrapRow: true,
        height: 600,
        //maxRows: 22,
        manualRowResize: true,
        manualColumnResize: true,
        rowHeaders: true,
        //manualRowMove: true,
        //manualColumnMove: true,
        contextMenu: true,
        filters: true,
        dropdownMenu: true,
        fixedColumnsLeft: 5,
        fixedRowsTop: 2,
        //manualColumnFreeze: true,
        columns: result.Columnas,
        mergeCells: [{ row: 0, col: 5, rowspan: 1, colspan: 12 }],
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0 || row == 1) {
                cellProperties.renderer = firstRowRendererCabecera;
            }
            else if (col == 2 || col == 3 || col == 4) {
                cellProperties.renderer = firstRowRendererCeleste;
            } else {

            }
            return cellProperties;
        },
        hiddenColumns: {
            columns: [0, 1],
            indicators: false
        }
    };

    hot = new Handsontable(container, hotSettings);
    hot.addHook('afterRenderer', function (TD, row, col, prop, value, cellProperties) {
        if (value != null && value != "" && DESCRIPCION_HANDSON != null) {
            var descrip = DESCRIPCION_HANDSON[row][col];
            $(TD).attr('title', descrip);
        }
    });
}

firstRowRendererCabecera = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'white';
    td.style.background = '#3D8AB8';
    td.style.fontWeight = 'bold';
    cellProperties.className = "htCenter",
        cellProperties.readOnly = true;
    //console.log(cellProperties);
}

firstRowRendererCeleste = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#E8F6FF';
}

function Importar() {
    var uploaderPR21 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelDT',
        url: controlador + "ImportarDataDisponibilidad",
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx,xls" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploaderPR21.files.length == 2) {
                    uploaderPR21.removeFile(uploaderPR21.files[0]);
                }
                uploaderPR21.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Cargando...");
            },
            UploadComplete: function (up, file, response) {
                mostrarExitoOperacion("Se proceso el archivo correctamente.");
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            },
            FileUploaded: function (uploader, file, result) {
                var resultado = JSON.parse(result.response);
                armarHandsontable(resultado);
            }
        }
    });
    uploaderPR21.init();
}
