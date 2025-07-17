const controller = siteRoot + "CPPA/Transmisores/";
const imageRoot = siteRoot + "Content/Images/";

var hot;
var data;
var error = [];

$(document).ready(function () {
    // Oculta botones de comando
    $('#btnDescargarFormato').hide();
    $('#btnSelectExcel').hide();
    $('#btnEnviarDatos').hide();
    $('#btnMostrarErrores').hide();
    $('#btnVerEnvios').hide();

    // Ocultar el cuadro de mensajes
    $('#mensaje').hide();

    $('#cbAnio').change(function () {
        cargarAjustes($(this).val());
    });

    $('#cbAjuste').change(function () {
        cargarRevisiones($('#cbAnio').val(), $(this).val());
    });

    $('#btnConsultar').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");

        var cpanio = $('#cbAnio').val();
        var cpajuste = $('#cbAjuste').val();
        var cparcodi = $('#cbRevision').val();

        if (cpanio == 0) {
            mostrarAlerta("No selecciono el año");
            return;
        }

        if (cpajuste == '') {
            mostrarAlerta("No selecciono el ajuste");
            return;
        }

        if (cparcodi == 0) {
            mostrarAlerta("No selecciono la revisión");
            return;
        }

        mostrarGrillaExcel();
    });

    $('#btnDescargarFormato').click(function () {
        $('#mensaje').show();
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");

        var cpanio = $('#cbAnio').val();
        var cpajuste = $('#cbAjuste').val();
        var cparcodi = $('#cbRevision').val();

        if (cpanio == 0) {
            mostrarAlerta("No selecciono el año");
            return;
        }

        if (cpajuste == '') {
            mostrarAlerta("No selecciono el ajuste");
            return;
        }

        if (cparcodi == 0) {
            mostrarAlerta("No selecciono la revisión");
            return;
        }

        descargarArchivo(1);
    });

    $('#btnEnviarDatos').click(function () {
        $('#mensaje').show();
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");

        var cpanio = $('#cbAnio').val();
        var cpajuste = $('#cbAjuste').val();
        var cparcodi = $('#cbRevision').val();

        if (cpanio == 0) {
            mostrarAlerta("No selecciono el año");
            return;
        }

        if (cpajuste == '') {
            mostrarAlerta("No selecciono el ajuste");
            return;
        }

        if (cparcodi == 0) {
            mostrarAlerta("No selecciono la revisión");
            return;
        }
        

        if (parseInt(error.length) > 0) {
            abrirPopupErrores();
        } else {
            grabarExcel();
        }
    });

    $('#btnMostrarErrores').click(function () {
        $('#mensaje').show();
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        
        if (parseInt(error.length) > 0) {
            abrirPopupErrores();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    $('#btnVerEnvios').click(function () {
        var cpanio = $('#cbAnio').val();
        var cpajuste = $('#cbAjuste').val();
        var cparcodi = $('#cbRevision').val();

        if (cpanio == 0) {
            mostrarAlerta("No selecciono el año");
            return;
        }

        if (cpajuste == '') {
            mostrarAlerta("No selecciono el ajuste");
            return;
        }

        if (cparcodi == 0) {
            mostrarAlerta("No selecciono la revisión");
            return;
        }

        popUpListaEnvios();
    });

    uploadExcel();

    // Muestra mensaje inicial
    mostrarExito('Por favor selecciones los filtros para poder realizar las consultas y operaciones correspondientes');
    $('#mensaje').show();
});

function cargarAjustes(year) {
    if (year) {
        $('#cbAjuste').prop('disabled', false).empty().append('<option value="">Seleccione un ajuste</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">Seleccione una revisión</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year))
            .map(x => x.Cpaapajuste)
            .filter((value, index, self) => self.indexOf(value) === index) // Eliminar duplicados
            .forEach((cpaapajuste) => { $('#cbAjuste').append('<option value="' + cpaapajuste + '">' + cpaapajuste + '</option>'); });
    }
    else {
        $('#cbAjuste').prop('disabled', true).empty().append('<option value="">Seleccione un ajuste</option>');
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">Seleccione una revisión</option>');
    }
}

function cargarRevisiones(year, fit) {
    if (year && fit) {
        $('#cbRevision').prop('disabled', false).empty().append('<option value="">Seleccione una revisión</option>');

        listRevision
            .filter(x => x.Cpaapanio === parseInt(year) && x.Cpaapajuste === fit)
            .forEach((revision) => {
                let estado = revision.Cparestado == 'A' ? '' : ' [' + revision.Cparestado + ']';
                $('#cbRevision').append('<option value="' + revision.Cparcodi + '">' + revision.Cparrevision + estado + '</option>');
            });
    }
    else {
        $('#cbRevision').prop('disabled', true).empty().append('<option value="">Seleccione una revisión</option>');
    }
}

mostrarGrillaExcel = function () {
    if (typeof hot != 'undefined') {
        hot.destroy();
    }

    var container = document.getElementById('grillaExcel');

    $.ajax({
        type: 'POST',
        url: controller + "GrillaExcel",
        data: {
            cpatdanio: $('#cbAnio').val(),
            cpatdajuste: $('#cbAjuste').val(),
            cparcodi: $('#cbRevision').val()
        },
        dataType: 'json',
        success: function (result) {
            data = result.Data;
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var visible = result.Visible;
            var codigo = result.Codigo;
            var fechaRegistro = result.FechaRegistro;
            var anio = result.Anio;
            var Mensaje = result.sMensaje;
            var EstadoRevision = result.EstadoRevision;
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                maxRows: result.Data.countRows - 1,
                colHeaders: true,
                colWidths: widths,
                columnSorting: false,
                contextMenu: false,
                minSpareRows: 1,
                rowHeaders: true,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                currentRowClassName: 'currentRow',
                cells: function (row, col, prop) {
                    var cellProperties = {};

                    if (row >= 0 && col == 0) {
                        cellProperties.readOnly = true;
                    }

                    if (row == 0) {
                        if (col == 0) {
                            cellProperties.renderer = firstRowRendererCabecerasAzul;
                        }
                        else if (col >= 1 && col <= 12) {
                            cellProperties.renderer = firstRowRendererCabecerasAzul;
                        }
                    }
                    else {
                        if (col == 0) {
                            cellProperties.renderer = rowsRendererCabecerasAzul;
                        }
                        else if (col >= 1 && col <= 12) {
                            cellProperties.renderer = negativeValueRenderer;
                        }
                    }

                    return cellProperties;
                }
            });

            hot.render();
          
          
            if (visible == 0) {
                if (EstadoRevision == 'C' || EstadoRevision == 'X') {
                    $('#grillaExcel').show();
                    $('#btnDescargarFormato').show();
                    $('#btnSelectExcel').hide();
                    $('#btnEnviarDatos').hide();
                    $('#btnMostrarErrores').hide();
                    $('#btnVerEnvios').show();
                }
                else {
                    $('#grillaExcel').show();
                    $('#btnDescargarFormato').hide();
                    $('#btnSelectExcel').hide();
                    $('#btnEnviarDatos').hide();
                    $('#btnMostrarErrores').hide();
                    $('#btnVerEnvios').hide();
                }

                mostrarAlerta(Mensaje);
            }
            else if (visible == 1) {

                if (EstadoRevision == 'C' || EstadoRevision == 'X') {
                    $('#grillaExcel').show();
                    $('#btnDescargarFormato').show();
                    $('#btnSelectExcel').hide();
                    $('#btnEnviarDatos').hide();
                    $('#btnMostrarErrores').hide();
                    $('#btnVerEnvios').show();
                }
                else {
                    $('#grillaExcel').show();
                    $('#btnDescargarFormato').show();
                    $('#btnSelectExcel').show();
                    $('#btnEnviarDatos').show();
                    $('#btnMostrarErrores').show();
                    $('#btnVerEnvios').show();
                }

                mostrarExito(Mensaje);
            }
            else if (visible == 2) {

                $('#grillaExcel').show();
                $('#btnDescargarFormato').show();
                $('#btnSelectExcel').show();
                $('#btnEnviarDatos').show();
                $('#btnMostrarErrores').show();
                $('#btnVerEnvios').show();

                mostrarAlerta(Mensaje);
            }
            else if (visible == 3) {

                if (EstadoRevision == 'C' || EstadoRevision == 'X') {
                    $('#grillaExcel').show();
                    $('#btnDescargarFormato').show();
                    $('#btnSelectExcel').hide();
                    $('#btnEnviarDatos').hide();
                    $('#btnMostrarErrores').hide();
                    $('#btnVerEnvios').show();
                }
                else {
                    $('#grillaExcel').show();
                    $('#btnDescargarFormato').show();
                    $('#btnSelectExcel').show();
                    $('#btnEnviarDatos').show();
                    $('#btnMostrarErrores').show();
                    $('#btnVerEnvios').show();
                }

                mostrarAlerta(Mensaje);
            }
            else if (visible == 4) {
                if (EstadoRevision == 'C' || EstadoRevision == 'X') {
                    $('#grillaExcel').show();
                    $('#btnDescargarFormato').show();
                    $('#btnSelectExcel').hide();
                    $('#btnEnviarDatos').hide();
                    $('#btnMostrarErrores').hide();
                    $('#btnVerEnvios').show();
                }
                else {
                    $('#grillaExcel').show();
                    $('#btnDescargarFormato').show();
                    $('#btnSelectExcel').show();
                    $('#btnEnviarDatos').show();
                    $('#btnMostrarErrores').show();
                    $('#btnVerEnvios').show();
                }

                mostrarAlerta(Mensaje);
            }
            else if (visible == 5) {

                if (EstadoRevision == 'C' || EstadoRevision == 'X') {
                    $('#grillaExcel').show();
                    $('#btnDescargarFormato').show();
                    $('#btnSelectExcel').hide();
                    $('#btnEnviarDatos').hide();
                    $('#btnMostrarErrores').hide();
                    $('#btnVerEnvios').show();
                }
                else {
                    $('#grillaExcel').show();
                    $('#btnDescargarFormato').show();
                    $('#btnSelectExcel').show();
                    $('#btnEnviarDatos').show();
                    $('#btnMostrarErrores').show();
                    $('#btnVerEnvios').show();
                }

                mostrarAlerta(Mensaje);
            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

grabarExcel = function () {
    $.ajax({
        type: "POST",
        url: controller + 'GrabarGrillaExcel',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            Anio: $('#cbAnio').val(),
            IdAjuste: $('#cbAjuste').val(),
            IdRevision: $('#cbRevision').val(),
            datos: hot.getData()
        }),
        success: function (result) {
            if (result == "1") {
                mostrarExito('La información se ha guardo correctamente');
            }
            else {
                mostrarError('Ha ocurrido un error: ' + result);
            }
        },
        error: function (response) {
            mostrarError(response.status + " " + response.statusText);
        }
    });
}

uploadExcel = function () {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectExcel',
        url: controller + 'uploadExcel',
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
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong>Por favor espere ...</strong>");
                procesarArchivo(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });

    uploader.init();
}

procesarArchivo = function (sFile) {
    mostrarAlerta("Espere un momento, se esta procesando su solicitud");

    if (typeof hot != 'undefined') {
        hot.destroy();
    }

    var container = document.getElementById('grillaExcel');

    $.ajax({
        type: 'POST',
        url: controller + 'procesararchivo',
        data: {
            sarchivo: sFile,
            cpatdanio: $('#cbAnio').val(),
            cpatdajuste: $('#cbAjuste').val(),
            cparcodi: $('#cbRevision').val()
        },
        dataType: 'json',
        success: function (result) {
            data = result.Data;

            var numMaxColumnas;
            if (result.Columnas == null)
                numMaxColumnas = null;
            else
                numMaxColumnas = result.Columnas.length;

            var numMaxFilas;
            if (result.Data == null)
                numMaxFilas = 0;
            else
                numMaxFilas = result.Data.countRows - 1;

            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var iRegError = result.RegError;
            var sMensajeError = result.sMensaje;
            var anio = result.Anio;
            var EstadoRevision = result.EstadoRevision;
            hot = new Handsontable(container, {
                data: data,
                maxCols: numMaxColumnas,
                maxRows: numMaxFilas,
                colHeaders: true,
                colWidths: widths,
                columnSorting: false,
                contextMenu: false,
                minSpareRows: 1,
                rowHeaders: true,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                currentRowClassName: 'currentRow',
                cells: function (row, col, prop) {
                    var cellProperties = {};

                    if (row >= 0 && col == 0) {
                        cellProperties.readOnly = true;
                    }

                    if (row == 0) {
                        if (col == 0) {
                            cellProperties.renderer = firstRowRendererCabecerasAzul;
                        }
                        else if (col >= 1 && col <= 12) {
                            cellProperties.renderer = firstRowRendererCabecerasAzul;
                        }
                    }
                    else {
                        if (col == 0) {
                            cellProperties.renderer = rowsRendererCabecerasAzul;
                        }
                        else if (col >= 1 && col <= 12) {
                            cellProperties.renderer = negativeValueRenderer;
                        }
                    }

                    return cellProperties;
                }
            });

            hot.render();
          

            if (iRegError > 0) {
                $('#grillaExcel').hide();
                $('#btnDescargarFormato').hide();
                $('#btnSelectExcel').hide();
                $('#btnEnviarDatos').hide();
                $('#btnMostrarErrores').hide();
                $('#btnVerEnvios').hide();
                mostrarError("Lo sentimos, no se ha podido realizar la operación por presentar <b>errores.</b>" + sMensajeError);
            }
            else {
                if (EstadoRevision == 'C' || EstadoRevision == 'X') {
                    $('#grillaExcel').hide();
                    $('#btnDescargarFormato').show();
                    $('#btnSelectExcel').hide();
                    $('#btnEnviarDatos').hide();
                    $('#btnMostrarErrores').hide();
                    $('#btnVerEnvios').show();
                    mostrarAlerta(sMensajeError);
                }
                else {
                    $('#grillaExcel').show();
                    $('#btnDescargarFormato').show();
                    $('#btnSelectExcel').show();
                    $('#btnEnviarDatos').show();
                    $('#btnMostrarErrores').show();
                    $('#btnVerEnvios').show();
                    mostrarMensaje("Por favor verifique los datos y luego proceda a <b>Grabar</b>");
                }
            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

abrirPopupErrores = function () {
    var html = '<span class="button b-close"><span>X</span></span>';
    html += '<p style="padding: 10px;"><b>Corregir los siguientes errores</b><p>';
    html += '<table border="0" class="pretty tabla-icono" id="tabla" cellspacing="2" cellpadding="5">'
    html += '<thead>'
    html += '<tr>'
    html += '<th>Fila</th>'
    html += '<th>Columna</th>'
    html += '<th>Valor</th>'
    html += '<th>Comentario</th>'
    html += '</tr>'
    html += '</thead>'
    html += '<tbody>'
    for (var i = error.length - 1; i >= 0; i--) {
        var sStyle = "background : #ffffff;";
        var sBackground = "";
        if (i % 2)
            var sStyle = "background : #fbf4bf;";

        var SplitError = error[i].split("_-_");
        var sTipError = SplitError[4].substring(0, 3);
        if (sTipError === "[1]") {
            sBackground = " background-color: #F02211;";
        }
        else if (sTipError === "[2]") {
            sBackground = " background-color: #F3F554;";
        }
        else if (sTipError === "[3]") {
            sBackground = " background-color: #ECAFF0;";
        }

        var sMsgError = SplitError[4].substring(3);
        html += '<tr id="Fila_' + i + '">'
        html += '<td style="text-align:center;' + sBackground + '">' + (parseInt(SplitError[1]) + 1) + '</td>'
        html += '<td style="text-align:center;' + sStyle + '">' + SplitError[2] + '</td>'
        html += '<td style="text-align:center;' + sStyle + '">' + SplitError[3] + '</td>'
        html += '<td style="text-align:left;' + sStyle + '">' + sMsgError + '</td>'
        html += '</tr>'
    }
    html += '</tbody>'
    html += '</table>'

    $('#popupErrores').html(html);

    $('#popup').html(html);
    $('#popup').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });

    mostrarError("Lo sentimos, la hoja del cálculo tiene errores");
}

descargarArchivo = function (formato) {
    $.ajax({
        type: 'POST',
        url: controller + 'ExportarData',
        data: {
            Anio: $('#cbAnio').val(),
            IdAjuste: $('#cbAjuste').val(),
            IdRevision: $('#cbRevision').val(),
            NombRevision: $('#cbRevision option:selected').text(),
            formato: formato
        },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = controller + 'AbrirArchivo?formato=' + formato + '&file=' + result + '&anio=' + $('#cbAnio').val() + '&idAjuste=' + $('#cbAjuste').val();
                mostrarExito("Felicidades, el archivo se descargo correctamente...!");
            }
            else {
                mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });
}

function popUpListaEnvios() {
    $.ajax({
        type: 'POST',
        traditional: true,
        url: controller + 'ListarEnvios',
        data: {
            cparcodi: $('#cbRevision').val()
        },
        success: function (result) {
            $('#idEnviosAnteriores').html(result.HtmlList);

            setTimeout(function () {
                $('#enviosanteriores').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    modalClose: false
                });

                $('#tablaenvio').dataTable({
                    "scrollY": 330,
                    "scrollX": true,
                    "sDom": 't',
                    "ordering": false,
                    "bPaginate": false,
                    "iDisplayLength": -1
                });
            }, 50);

        },
        error: function (err) {
            alert('Ha ocurrido un error al generar vista');
        }
    });
}

obtenerTransmisoresTransmisoresDet = function (codigo, anio, ajuste, revision) {
    $('#enviosanteriores').bPopup().close();

    mostrarAlerta("Espere un momento, se esta procesando su solicitud");

    if (typeof hot != 'undefined') {
        hot.destroy();
    }

    $("#cbAnio").val(anio);
    $("#cbAjuste").val(ajuste);
    $("#cbRevision").val(revision);

    var container = document.getElementById('grillaExcel');

    $.ajax({
        type: 'POST',
        url: controller + "CargarGrillaExcel",
        data: {
            cpattcodi: codigo,
            cpatdanio: $('#cbAnio').val(),
            cpatdajuste: $('#cbAjuste').val(),
            cparcodi: $('#cbRevision').val()
        },
        dataType: 'json',
        success: function (result) {
            data = result.Data;
            var columns = result.Columnas;
            var headers = result.Headers;
            var widths = result.Widths;
            var visible = result.Visible;
            var anio = result.Anio;
            var fechaRegistro = "";
            var Mensaje = result.sMensaje;
            var EstadoRevision = result.EstadoRevision;
            hot = new Handsontable(container, {
                data: data,
                maxCols: result.Columnas.length,
                maxRows: result.Data.countRows - 1,
                colHeaders: true,
                colWidths: widths,
                columnSorting: false,
                contextMenu: false,
                minSpareRows: 1,
                rowHeaders: true,
                columns: columns,
                fixedRowsTop: result.FixedRowsTop,
                fixedColumnsLeft: result.FixedColumnsLeft,
                currentRowClassName: 'currentRow',
                cells: function (row, col, prop) {
                    var cellProperties = {};

                    if (row >= 0 && col == 0) {
                        cellProperties.readOnly = true;
                    }

                    if (row == 0) {
                        if (col == 0) {
                            cellProperties.renderer = firstRowRendererCabecerasAzul;
                        }
                        else if (col >= 1 && col <= 12) {
                            cellProperties.renderer = firstRowRendererCabecerasAzul;
                        }
                    }
                    else {
                        if (col == 0) {
                            cellProperties.renderer = rowsRendererCabecerasAzul;
                        }
                        else if (col >= 1 && col <= 12) {
                            cellProperties.renderer = negativeValueRenderer;
                        }
                    }

                    return cellProperties;
                }
            });

            hot.render();


            if (EstadoRevision == 'C' || EstadoRevision == 'X') {
                $('#grillaExcel').hide();
                $('#btnDescargarFormato').show();
                $('#btnSelectExcel').hide();
                $('#btnEnviarDatos').hide();
                $('#btnMostrarErrores').hide();
                $('#btnVerEnvios').show();
                mostrarAlerta(Mensaje);
            }
            else {
                $('#grillaExcel').show();
                $('#btnDescargarFormato').show();
                $('#btnSelectExcel').show();
                $('#btnEnviarDatos').show();
                $('#btnMostrarErrores').show();
                $('#btnVerEnvios').show();
                mostrarMensaje("Podrá modificar los datos y luego podra <b>Grabar</b> los cambios, generando un nuevo registro");
            }

            $('#enviosanteriores').bPopup().close();
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}


firstRowRendererCabecerasAzul = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'white';
    td.style.background = '#3D8AB8';
    td.style.fontWeight = 'bold';
    cellProperties.className = "htCenter"
    cellProperties.readOnly = true;
}

rowsRendererCabecerasAzul = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'white';
    td.style.background = '#3D8AB8';
    td.style.fontWeight = 'bold';
    cellProperties.readOnly = true;
}

negativeValueRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    var sHeader = $(instance.getCell(0, col)).html();
    var sColumn = $(instance.getCell(row, 0)).html();
    var sMensaje;

    if (row == 1 && col == 1) {
        // Limpiamos la lista de errores
        error = [];
    }

    if (value) {
        if (isNaN(parseInt(value, 10))) { // No es numero
            td.style.backgroundColor = '#F02211'; // Establece fondo de la celda de color rojo
            td.style.color = '#FFFFFF'; // Color fuente blanco
            td.style.fontWeight = 'bold';
            sMensaje = "[1]El Valor " + value + " en el día " + sHeader + " para [" + sColumn + "] no es válido";
        }
        else if (parseFloat(value, 10) > 999999999 || parseFloat(value, 10) < 0) { // Numero negativo o fuera de limite maximo
            td.style.background = '#F3F554'; // Establece fondo de la celda de color amarillo
            td.style.fontWeight = 'bold'; // Fuente en negrita
            sMensaje = "[2]El valor " + value + " en el día " + sHeader + " para [" + sColumn + "] supera el Limite Max/Min permitido: 10,000/0";
        }
        // Validar si el valor NO es alfanumérico (usa una expresión regular)
        else if (!/^\d+(\.\d+)?$/.test(value)) {
            td.style.backgroundColor = '#F02211'; // Fondo rojo
            td.style.color = '#FFFFFF';           // Texto blanco
            td.style.fontWeight = 'bold';         // Negrita
            sMensaje = "[1]El valor '" + value + "' en el día " + sHeader + " para [" + sColumn + "] no es un número válido.";
        }
    }
    else if (value != "0") { // Si la celda esta vacia
        td.style.background = '#FFFFFF'; // Establece fondo de la celda de color blanco
    }

    if (sMensaje) {
        /*if (isNaN(value)) value = "";*/
        error.push(value.toString().concat("_-_" + row + "_-_" + sHeader + "_-_" + value + "_-_" + sMensaje));
    }
}


mostrarExito = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-exito');
}

mostrarError = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-error');
}

mostrarAlerta = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}
