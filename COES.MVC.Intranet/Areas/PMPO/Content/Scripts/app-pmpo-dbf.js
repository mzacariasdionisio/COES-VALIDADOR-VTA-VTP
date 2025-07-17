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
    var url = $("#ListDbf").val();
    var periodo = $("#hddPeriodo").val();
    var nombarra = $("#txtBuscar").val();
    $.ajax({
        type: 'Get',
        url: url,
        dataType: 'json',
        cache: false,
        data: {
            periodo: periodo,
            nombarra: nombarra
        },
        success: function (result) {
            armarHandsontable(result);
        },
        error: function () {
            mostrarError();
        }
    });
}


function armarHandsontable(result) {
    var container = document.getElementById('handson');
    //NET 2019-03-07 - Corrección del guardado de la grilla
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
        //fixedColumnsLeft: 9,
        //manualColumnFreeze: true,
        columns: result.columnas,
        //mergeCells: [{ row: 0, col: 5, rowspan: 1, colspan: 53 }],
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0) {
                cellProperties.renderer = firstRowRendererCabecera;
            }
            //else if (col == 2 || col == 3 || col == 4) {
            //    cellProperties.renderer = firstRowRendererCeleste;
            //}
            return cellProperties;
        },
        hiddenColumns: {
            columns: [7, 8],            
            indicators: false
        }
    };

    hot = new Handsontable(container, hotSettings);
}

function grabar() {
    var url = $("#GrabarDbf").val();
    var periodo = $("#hddPeriodo").val();

    $.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            periodo: periodo, datos: hot.getData()
        }),
        beforeSend: function () {
            mostrarExitoOperacion("Grabando datos...")
        },
        success: function (result) {
            cargarHandosntable();

            mostrarExitoOperacion("Se guardaron " + result.filasafectadas + " Registros, " + result.filasNoGuardadas+ " no se pudieron guardar.")
        },
        error: function (response) {
            mostrarError(response.status + " " + response.statusText);
        }
    });
}

function Exportar() {
    var url = $("#Exportar").val();
    var periodo = $("#hddPeriodo").val();
    var nombarra = $("#txtBuscar").val();
    location.href = url + "/?Periodo=" + periodo + "&nombarra=" + nombarra;
}

function Importar() {
    var url = $("#Importar").val();

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
                //procesarArchivoPR21(file[0].name);
            },
            FileUploaded: function (uploader, file, result) {
                borrarData();
                var resultado = JSON.parse(result.response);
                armarHandsontable(resultado);
            },
            Error: function (up, err) {                
                mostrarError(err.code + "-" + err.message);
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


function GenerarRepDemandaPorBloque() {

    var periodo = $("#hddPeriodo").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarRepDemandaPorBloque',
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

function GenerarRepGrupoRelaso() {

    var idDef = '1';

    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarRepGrupoRelaso',
        dataType: 'json',
        data: {
            strGrrdefcodi: idDef
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