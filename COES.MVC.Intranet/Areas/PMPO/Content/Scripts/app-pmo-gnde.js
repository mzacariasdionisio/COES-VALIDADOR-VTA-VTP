var controlador = siteRoot + 'PMPO/GeneracionArchivosDAT/';

var hot;
$(document).ready(function () {
    cargarHandosntable();
    Importar();
});

function filtrarData() {
    cargarHandosntable();
}

function cargarHandosntable() {
    var url = $("#ListGndse").val();
    var periodo = $("#hddPeriodo").val();
    //var central = $("#txtBuscar").val();
    var central = $("#cboCentral").val();
    $.ajax({
        type: 'Get',
        url: url,
        dataType: 'json',
        cache: false,
        data: {
            periodo: periodo,
            central: central
        },
        success: function (result) {
            armarHandsontable(result);
        },
        error: function () {
            mostrarError();
        }
    });
}

function grabar() {
    var url = $("#GrabarGnde").val();
    var periodo = $("#hddPeriodo").val();
    //var central = $("#txtBuscar").val();
    var central = $("#cboCentral").val();

    $.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            periodo: periodo, datos: hot.getData(),central: central
        }),
        beforeSend: function () {
            mostrarExitoOperacion("Grabando datos...")
        },
        success: function (result) {
            cargarHandosntable();

            mostrarExitoOperacion("Se guardaron " + result.filasafectadas + " Registros, " + result.filasNoGuardadas + " no se pudieron guardar.")
        },
        error: function (response) {
            mostrarError(response.status + " " + response.statusText);
        }
    });
}

function armarHandsontable(result) {
    var container = document.getElementById('handson');

    var hotSettings = {
        data: result.data,
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
        //fixedColumnsLeft: 5,
        //manualColumnFreeze: true,
        columns: result.columnas,
        //mergeCells: [{ row: 0, col: 5, rowspan: 1, colspan: 53 }],
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0 ) {
                cellProperties.renderer = firstRowRendererCabecera;
            }
            //else if (col == 1 || col == 2 || col == 3) {
            //    cellProperties.renderer = firstRowRendererCeleste;
            //}
            return cellProperties;
        }
    };

    hot = new Handsontable(container, hotSettings);
}

function Exportar() {

    var periodo = $("#hddPeriodo").val();
    //var central = $("#txtBuscar").val();
    var central = $("#cboCentral").val();
    var url = $("#ExportarDataGndse").val();

    location.href = url + "/?periodo=" + periodo + "&central=" + central;

}


function GenerarReporteExcel() {

    var periodo = $("#hddPeriodo").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteExcelTotalGndse',
        dataType: 'json',
        data: {
            periodo: periodo
        },
        success: function (result) {

            if (result != -1) {
                document.location.href = controlador + 'descargar?formato=' + 1 + '&file=' + result
                mostrarExitoOperacion("Exportación realizada.");
            }
            else {
                mostrarError('Opcion Exportar: Ha ocurrido un error');
            }

        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error');
        }
    });
}

function Importar() {
    var url = $("#ImportarDataGndse").val();

    var uploaderPR21 = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelDT',
        url: url,
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
                borrarData();
                var resultado = JSON.parse(result.response);
                armarHandsontable(resultado);
            }
        }
    });
    uploaderPR21.init();
}

function borrarData() {
    hot.updateSettings({
        data: []
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

firstRowRendererAmarillo = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#FFFFD7';
}