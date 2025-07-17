const CONTROLADOR_URL = siteRoot + 'IND/CargaHistorica/';
var contenedorHt, tblCargarHistorico, listaErrores = [], tblErroresdatos;

$(function () {
    cargarHansonWeb(0);

    $("#btnExportar").click(function () {
        exportarCargaHistorica();
    });

    $("#btnGuardar").click(function () {
        GuardarCargaHistorica();
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
        openPopup("historial");
    });

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

    importarCargaHistorica();

    //guardar fecha max
    $('#fechaMaxMes').Zebra_DatePicker({
        format: 'm Y',
        readonly_element: false,
    });
    $("#btnGuardarFechaMax").click(function () {
        guardarFechaMaxCargaHistorica();
    });
});


function cargarHansonWeb(irptcodi) {

    $.ajax({
        type: 'POST',
        url: CONTROLADOR_URL + "HandsonCargaHistoricoHidro",
        data: { irptcodi: irptcodi },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                inicializarHansonTable(evt.HandsonCargaHistorica);

                $("#textPeriodo").text("Edición manual: " + `${evt.Cuadro.PeriodoIniHistoricoDesc} - ${evt.Cuadro.PeriodoFinHistoricoDesc}`)
                $("#textPeriodo").width($(".search-content").width() - 400);

                $('#listadoH').html(evt.Resultado2);

                $("#listadoH").css("width", "820px");

                $('.tabla_version_x_recalculo').dataTable({
                    sPaginationType: "full_numbers",
                    destroy: "true",
                    ordering: false,
                    searching: true,
                    iDisplayLength: -1,
                    info: false,
                    paging: false,
                    scrollX: false,
                    scrollY: "250px",
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function verPorVersion(irptcodi) {
    cargarHansonWeb(irptcodi);
}

function exportarCargaHistorica() {
    var dataArray = getDataHandsonTable();;

    $.ajax({
        type: 'POST',
        url: CONTROLADOR_URL + "ExportarCargaHistoricoHidro",
        data: { stringJson: JSON.stringify(dataArray) },
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {

                if (evt.Resultado == "0")
                    alert(evt.Mensaje);
                else
                    window.location = CONTROLADOR_URL + "Exportar?file_name=" + evt.Resultado;

            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

}

function GuardarCargaHistorica() {

    if (listaErrores.length > 0) {

        alert('Existen errores en las celdas, favor de corregir y vuelva a envíar.');
        $("#btnMostrarErrores").trigger("click");

        return;
    }

    var dataArray = getDataHandsonTable();

    $.ajax({
        type: 'POST',
        url: CONTROLADOR_URL + "GuardarCargaHistoricoHidro",
        data: { stringJson: JSON.stringify(dataArray) },
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                alert("Se guardó correctamente la información histórica");
                cargarHansonWeb(evt.Resultado);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

}


function inicializarHansonTable(hot) {
    contenedorHt = document.getElementById('htCargaHistorica');

    tblCargarHistorico = new Handsontable(contenedorHt, {
        data: JSON.parse(hot.ListaExcelData2),
        colHeaders: hot.Headers,
        columns: hot.Columnas,
        width: '100%',
        height: 400,
        rowHeights: 23,
        rowHeaders: true,
        fixedColumnsLeft: 3,
        hiddenColumns: {
            indicators: false,
            columns: [3, 4, 5, 6],
        },
        colWidths: hot.ListaColWidth
    });

    tblCargarHistorico.addHook('afterRender', function () {
        tblCargarHistorico.validateCells();
    });

    tblCargarHistorico.addHook('beforeValidate', function (value, row, prop, source) {
        listaErrores = [];
    });

    tblCargarHistorico.addHook('afterValidate', function (isValid, value, row, prop, source) {

        if (!isValid) {

            var error = [];
            var periodo = moment(prop, "YYYYMM").format("MMM YYYY");

            error = {
                address: `[<b>${periodo}</b>] [<b>${row + 1}</b>]`,
                valor: value,
                message: "El dato no es numerico",
                className: "htInvalid"
            };

            listaErrores.push(error);
        }

    });

}

function getDataHandsonTable() {

    var data = tblCargarHistorico.getSourceData();

    var lstdata = [];

    data.forEach(function (items) {

        Object.entries(items).forEach(item => {
            var property = item[0];//Nombre propiedad

            //if (item[1]) { //Valor propiedad

            var column = parseInt(property);

            if (Number.isInteger(column)) {//Periodo

                var indreporte = {
                    Emprcodi: items.Emprcodi,
                    Equipadre: items.Equipadre,
                    Equicodi: items.Equicodi,
                    Grupocodi: items.Grupocodi,
                    Central: items.Central,
                    Emprnomb: items.Emprnomb,
                    Equinomb: items.Equinomb
                };

                var program = items[`${column}`];

                indreporte.Periodo = column;
                indreporte.Tothorasp = program;

                lstdata.push(indreporte);
            }
            //}
        });
    });

    return lstdata;
}


function importarCargaHistorica() {

    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnImportar",
        url: CONTROLADOR_URL + "UploadCargaHistoricoHidro",
        multi_selection: false,
        filters: {
            max_file_size: '10mb',
            mime_types: [
                { title: "Archivos xls", extensions: "xlsx" }
            ]
        },
        init: {
            FilesAdded: function (up, files) {
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                //mostrarMensaje('mensajeInterr', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },

            FileUploaded: function (up, file, info) {
                var result = JSON.parse(info.response);
                tblCargarHistorico.loadData(JSON.parse(result.Resultado2));
                alert("Se importó correctamente el archivo excel");
            },
            UploadComplete: function (up, file, info) {
            },
            Error: function (up, err) {
                alert("Ha ocurrido un error");
            }
        }
    });

    uploader.init();
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

    // Register an alias
    Handsontable.validators.registerValidator('validatorDecimal', customValidator);

})(Handsontable);

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

function guardarFechaMaxCargaHistorica() {
    $.ajax({
        type: 'POST',
        url: CONTROLADOR_URL + "GuardarFechaMaxHidro",
        data: {
            mes: $('#fechaMaxMes').val()
        },
        datatype: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                alert("Se guardó correctamente la información.");
                cargarHansonWeb(0);
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

}