// ASSETEC 2019-11
var controler = siteRoot + "transferencias/potenciacontratada/";
var error = [];
var hotConsulta;

$(document).ready(function ($) {

    // Evento change del combo cboEmpresa del formulario potencias contratadas que valida si este combo esta vacio o no
    $('#cboEmpresa').on("change", function () {
        if ($('#cboEmpresa').val() != "0") {
            MostrarGrillaExcelConsulta();
            rspta = false;
        }
    });

    // Evento change del combo cboPeriodo del formulario potencias contratadas que valida si este combo esta vacio o no
    $('#cboPeriodo').on("change", function () {
        if ($('#cboPeriodo').val() != "0") {
            MostrarGrillaExcelConsulta();
            rspta = false;
        }
    });

    $('#btnDescargarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        DescargarPotenciaContratada();
    });

    $('#btnValidarGrillaExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        if (parseInt(error.length) > 0) {
            abrirPopupErrores();
        } else {
            mostrarExito("La hoja de cálculo no tiene errores...!");
        }
    });

    // Evento click del boton btnProcesar del formulario potencias contratadas que procesa los registros del archivo
    // Excel que previamente fue descargado y editado para ser subido nuevamente y ser leido y obtener dichos registros
    // editados para finalmente ser insertados en la tabla potencias contratadas
    $('#btnGrabarExcel').click(function () {
        mostrarAlerta("Espere un momento, se esta validando la información");
        if (parseInt(error.length) > 0) {
            abrirPopupErrores();
        } else {
            grabarExcel();
        }
    });
    
    // Evento click del boton btnConsultar del formulario potencias contratadas que obtiene los registros de potencias
    // contratadas en base a las tablas de codigos de solicitud de retiro y de potencias contratadas
    $('#btnConsultar').click(function () {        
        MostrarGrillaExcelConsulta();        
    });

    // Procedimento que adjunta archivo que previamente ha sido modificado para ser leido y procesado para insertar sus
    // registros en la tabla de potencias contratadas
    SeleccionarArchivo();

    $('#btnSolicitudRetiro').click(function () {
        SolicitudCodigoRetiro();
    });
});

// Procedimento que descarga la plantilla con la cual contiene registros de solicitudes de retiro obtenidas por id de
// empresa de la base de datos
function DescargarPotenciaContratada() {    
    var emprcodi = $('#cboEmpresa').val();
    //var emprnomb = $('#cboEmpresa option:selected').html()
    var pericodi = $('#cboPeriodo').val();
    //var perinombre = $('#cboPeriodo option:selected').html()
    $.ajax({
        type: 'POST',
        url: controler + 'DescargarPotenciaContratada',
        data: { pericodi: pericodi, emprcodi: emprcodi },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                var paramList = [
                    { tipo: 'input', nombre: 'file', value: result }
                ];
                var form = CreateForm(controler + 'AbrirArchivo', paramList);
                document.body.appendChild(form);
                form.submit();
                mostrarExito("Ya puede consultar el documento");
                return true;
            }
            else {
                mostrarError('Ha ocurrido un error');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //console.log(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText)                
            mostrarError('Ha ocurrido un error');
        }
    });     
}

// Procedimento que adjunta archivo que previamente ha sido modificado para ser leido y procesado para insertar sus
// registros en la tabla de potencias contratadas
function SeleccionarArchivo() {
    var fullDate = new Date();
    var twoDigitMonth = ((fullDate.getMonth().length) == 1) ? '0' + (fullDate.getMonth() + 1) : (fullDate.getMonth() + 1);
    var fecha = fullDate.getFullYear().toString() + twoDigitMonth.toString() + fullDate.getDate().toString() + fullDate.getHours().toString() + fullDate.getMinutes().toString() + fullDate.getSeconds().toString();
    var comb = fecha;
    //console.log(comb);
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcel',
        url: controler + 'UploadArchivo?fecha=' + comb,
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
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
                // Envia la fecha y el nombre del archivo seleccionado
                //MostrarArchivo(comb, file[file.length - 1].name);
                $('#sfile').val(comb + "_" + file[file.length - 1].name);
                mostrarExcelWeb();
            },
            Error: function (up, err) {
                $('#container').css('display', 'true');
                if (err.code == -601) {
                    document.getElementById('filelist').innerHTML = '<div class="action-alert">' + 'La extensión del archivo no es válido' + '</div>';
                }
            }
        }
    });
    uploader.init();
}

// Procedimiento que lista un archivo subido al sistema
function MostrarArchivo(fechaUp, fileName) {
    var autoid = "ExcelBuscar";

    $.ajax({
        type: 'POST',
        url: controler + 'MostrarArchivo',
        data: { sFecha: fechaUp, sFilename: fileName },
        success: function (result) {
            var listaArchivos = result.ListaDocumentosFiltrado;
            $.each(listaArchivos, function (index, value) {
                document.getElementById('filelist').innerHTML = '<div name="' + value.FileName + '" class="file-name" id="' + autoid + '">' + value.FileName + ' (' + value.FileSize + ') <a class="remove-item" href="JavaScript:EliminarFile(\'' + autoid + "@" + value.FileName + '\');">X</a> <b></b></div>';
                $('#sfile').val(value.FileName);
                mostrarExito("El archivo ha sido copiada al portapapeles, ahora puede Procesar Archivo");
            })
        },
        error: function () {
            mostrarError('Lo sentimos, se ha producido un error.');
        }
    });
}

// Procedimiento que elimina un archivo excel subido al sistema
function EliminarFile(id) {
    var string = id.split("@");
    var idinter = string[0];
    var nombreArchivo = string[1];
    uploaderN.removeFile(idinter);
    $.ajax({
        type: 'POST',
        url: controler + 'EliminarArchivos',
        data: { nombreArchivo: nombreArchivo },
        success: function (result) {
            if (result == -1) {
                $('#' + idinter).remove();
                $('#sfile').val("");
                mostrarExito("Se ha eliminado el archivo del portapapeles");
            } else {
                mostrarError("Lo sentimos, algo salio mal");
            }
        },
        error: function () {
            mostrarError('Ha ocurrido un error');
        }
    });
}

grabarExcel = function () {
    var emprcodi = $('#cboEmpresa').val();
    //var emprnomb = $('#cboEmpresa option:selected').html()
    var pericodi = $('#cboPeriodo').val();
    //var perinombre = $('#cboPeriodo option:selected').html()
    //console.log(hotConsulta.getData());
    $.ajax({
        type: "POST",
        url: controler + 'procesarpotenciacontratada',
        dataType: "json",
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({ pericodi: pericodi, emprcodi: emprcodi, datos: hotConsulta.getData() }),
        success: function (result) {
            if (isNaN(parseInt(result, 10))) {
                mostrarError('Lo sentimos, ocurrio un error al grabar la información:' + result);
            }
            if (result == -1) {
                mostrarError('Lo sentimos, ocurrio un error al grabar la información');
            }
            else {
                mostrarExito('Se ha registrado ' + result + ' registros correctamente.');
                return true;
            }
        },
        error: function (response) {
            mostrarError(response.status + " " + response.statusText);
        }
    });
} 

/************************************************************************************************************************************************/
// procedimiento que procesa el que procesa los registros del archivo Excel que previamente fue descargado y editado 
// para ser subido nuevamente y ser leido y obtener dichos registros editados para finalmente ser insertados en la 
// tabla potencias contratadas
mostrarExcelWeb = function () {
    var emprcodi = $('#cboEmpresa').val();
    var pericodi = $('#cboPeriodo').val();
    var sfile = $('#sfile').val();
    if (sfile != "") {
        $.ajax({
            type: 'POST',
            url: controler + 'mostrarexcelweb',
            data: { pericodi: pericodi, emprcodi: emprcodi, sfile: sfile },
            dataType: 'json',
            success: function (result) {
                if (result == -1) {
                    mostrarError('No se ha encontrado registros de la empresa seleccionada');
                }
                else {
                    configurarExcelWeb(result);
                    var bGrabar = result.bGrabar;
                    if (bGrabar) {
                        $('#btnDescargarExcel').css('display', 'block');
                        $('#btnSelecionarExcel').css('display', 'block');
                        $('#btnGrabarExcel').css('display', 'block');
                        $('#btnValidarGrillaExcel').css('display', 'block');
                    }
                    else {
                        $('#btnDescargarExcel').css('display', 'block');
                        $('#btnSelecionarExcel').css('display', 'none');
                        $('#btnGrabarExcel').css('display', 'none');
                        $('#btnValidarGrillaExcel').css('display', 'none');
                    }
                    $('#divAcciones').css('display', 'block');
                    var sMensaje = 'Se ha importado ' + result.NumRegistros + ' registros';
                    if (result.MensajeError != "") {
                        mostrarAlerta(sMensaje + ', pero los siguientes códigos no estan presentes en el Excel: ' + result.MensajeError);
                    }
                    else {
                        mostrarExito(sMensaje + ', por favor verificar la información y proceder a grabar en la opción <b>Enviar datos</b>.');
                    }
                    return true;
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //console.log(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText)
                mostrarError('Ha ocurrido un error');
            }
        });
    }
    else {
        mostrarError('Lo sentimos, no ha seleccionado ningun archivo');
    }
}
/************************************************************************************************************************************************/



configurarExcelWeb = function (result) {
    if (typeof hotConsulta != 'undefined') {
        hotConsulta.destroy();
    }
    var container = document.getElementById('grillaExcelConsulta');
    calculateSizeHandsontable(container);
    hotConsulta = new Handsontable(container, {
        data: result.Data,
        maxCols: result.Columnas.length,
        colWidths: result.Widths,
        columns: result.Columnas,
        fixedRowsTop: result.FixedRowsTop,
        //fixedColumnsLeft: result.FixedColumnsLeft,
        currentRowClassName: 'currentRow',
        mergeCells: [
            { row: 0, col: 0, rowspan: 1, colspan: 7 },
            { row: 0, col: 7, rowspan: 1, colspan: 2 },
            { row: 0, col: 9, rowspan: 2, colspan: 1 },
            { row: 0, col: 10, rowspan: 1, colspan: 3 },
            { row: 0, col: 13, rowspan: 1, colspan: 3 },
            { row: 0, col: 16, rowspan: 2, colspan: 1 }
        ],
        cells: function (row, col, prop) {
            var cellProperties = {};
            if (row == 0 || row == 1) {
                if (col == 0 || col == 1 || col == 2 || col == 3 || col == 4 || col == 5 || col == 6 || col == 7 || col == 8) {
                    cellProperties.renderer = FirstRowRendererCabecerasAzul;
                }
                else if (col == 9 || col == 10 || col == 11 || col == 12 || col == 13 || col == 14 || col == 15 || col == 16) {
                    cellProperties.renderer = FirstRowRendererCabecerasAnaranjadas;
                }
            }
            else if (col == 0 || col == 1 || col == 2 || col == 3 || col == 4 || col == 5 || col == 6 || col == 7 || col == 8) {
                cellProperties.renderer = firstRowRendererCeleste;
            }
            if (col > 9 && col < 16 && row > 1) {
                cellProperties.renderer = negativeValueRenderer;
            }
            //else if (col == 9) {
            //    cellProperties.renderer = validarTextoValueRenderer;
            //}
            return cellProperties;
        },
    });
    hotConsulta.render();
}

// Procedimento que formatea y muestra los registros de la consulta de potencias contratadas en base del id de empresa
// y id de periodo
MostrarGrillaExcelConsulta = function () {
    var idEmpresa = $('#cboEmpresa').val();
    var idPeriodo = $('#cboPeriodo').val();
    $.ajax({
        type: 'POST',
        url: controler + "GrillaExcelConsultar",
        data: { idEmpresa: idEmpresa, idPeriodo: idPeriodo },
        dataType: 'json',
        success: function (result) {
            //console.log(result.Data);
            configurarExcelWeb(result);
            if (result.NumRegistros > 0) {
                mostrarExito('Ya puede verificar la información de ' + result.NumRegistros + ' registros y completar los datos faltantes');
                var bGrabar = result.bGrabar;
                if (bGrabar) {
                    $('#btnDescargarExcel').css('display', 'block');
                    $('#btnSelecionarExcel').css('display', 'block');
                    $('#btnGrabarExcel').css('display', 'block');
                    $('#btnValidarGrillaExcel').css('display', 'block');
                }
                else {
                    $('#btnDescargarExcel').css('display', 'block');
                    $('#btnSelecionarExcel').css('display', 'none');
                    $('#btnGrabarExcel').css('display', 'none');
                    $('#btnValidarGrillaExcel').css('display', 'none');
                }
                $('#divAcciones').css('display', 'block');
            }
            else {
                mostrarAlerta('Lo sentimos, no encontramos información para la consulta');                
                //$('#divAcciones').css('display', 'none');
                $('#divAcciones').css('display', 'block');
                $('#btnDescargarExcel').css('display', 'none');
                $('#btnSelecionarExcel').css('display', 'none');
                $('#btnGrabarExcel').css('display', 'none');
                $('#btnValidarGrillaExcel').css('display', 'none');
                $('#btnSolicitudRetiro').css('display', 'block');
                if (typeof hotConsulta != 'undefined') {
                    hotConsulta.destroy();
                }
                var container = document.getElementById('grillaExcelConsulta');
                hotConsulta = new Handsontable(container, {
                    width: 1,
                    height: 1
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //console.log(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText)
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo');
        }
    });   
}

function calculateSizeHandsontable(container) {
    var offset = Handsontable.Dom.offset(container);
    var iAltura = $(window).height() - offset.top - 50;
    //console.log($(window).height());
    //console.log(offset.top);
    //console.log(iAltura);
    container.style.height = iAltura + 'px';
    container.style.overflow = 'hidden';
    container.style.width = '1040px';
}

negativeValueRenderer = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    var sHeader = $(instance.getCell(1, col)).html();
    var sFila = $(instance.getCell(row, 0)).html();
    var sMensaje;
    if (row == 2 && col == 10) {
        //Limpiamos la lista de errores
        error = [];
        //console.log('Valor: ' + value);
    }
    if (value) {
        if (isNaN(parseInt(value, 10))) {
            //console.log('Basura ' + value); //NO ES NUMERO
            td.style.backgroundColor = '#F02211';
            td.style.color = '#FFFFFF';
            td.style.fontWeight = 'bold';
            sMensaje = "[1]Valor " + value + " no es válido";
        }
        //else if (parseInt(value, 10) < 0) {
        //    // add class "negativ0o" pinta  Naranja
        //    td.style.backgroundColor = '#FFCC33';
        //    //console.log('Negativo ' + value);
        //}
        else if (parseInt(value, 10) > 9999 || parseInt(value, 10) <= -1) {
            //console.log('>350 ' + value); //"Maximo" pinta amarillo
            td.style.background = '#F3F554';
            sMensaje = "[2]El valor " + value.toString() + " supera el Limite permitido: +/- 10,000";
        }
    }
    else if (value != "0"){
        td.style.background = '#ECAFF0'; //lila
        sMensaje = "[3]Tiene un valor vacio ";
    }
    if (sMensaje) {
        //console.log(value);
        if (!isNaN(value)) value = "";
        error.push(value.toString().concat("_-_" + sFila + "_-_" + sHeader + "_-_" + sMensaje));
        //console.log(error);
    }
}

// Formatea y colorea los encabezados del hansontable de azul
FirstRowRendererCabecerasAzul = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'white';
    td.style.background = '#3D8AB8';
    td.style.fontWeight = 'bold';
    cellProperties.className = "htCenter",
    cellProperties.readOnly = true;
}

// Formatea y colorea los encabezados del hansontable de anaranjado
FirstRowRendererCabecerasAnaranjadas = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'white';
    td.style.background = '#FF8000';
    td.style.fontWeight = 'bold';
    cellProperties.className = "htCenter",
    cellProperties.readOnly = true;
}

firstRowRendererCeleste = function (instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);
    td.style.color = 'black';
    td.style.background = '#E8F6FF';
}

abrirPopupErrores = function () {
    var html = '<span class="button b-close"><span>X</span></span>';
    html += '<p><b>Corregir los siguientes errores</b><p>';
    html += '<table border="0" class="pretty tabla-icono" id="tabla">'
    html += '<thead>'
    html += '<tr>'
    html += '<th> </th>'
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
        var sTipError = SplitError[3].substring(0, 3);
        if (sTipError === "[1]") {
            sBackground = " background-color: #F02211;";
        }
        else if (sTipError === "[2]") {
            sBackground = " background-color: #F3F554;";
        }
        else if (sTipError === "[3]") {
            sBackground = " background-color: #ECAFF0;";
        }
        var sMsgError = SplitError[3].substring(3);
        html += '<tr id="Fila_' + i + '">';
        html += '<td style="text-align:right;' + sBackground + '"> </td>';
        html += '<td style="text-align:right;' + sStyle + '">' + SplitError[1] + '</td>';
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[2] + '</td>';
        html += '<td style="text-align:left;' + sStyle + '">' + SplitError[0] + '</td>';
        html += '<td style="text-align:left;' + sStyle + '">' + sMsgError + '</td>';
        html += '</tr>'
    }
    html += '</tbody>'
    html += '</table>'

    $('#popupErrores').html(html);
    $('#popupErrores').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    mostrarError("Lo sentimos, la hoja del cálculo tiene errores");
}

// Procedimento que enmascara una URL por motivos de seguridad que contiene parametros sensibles
function CreateForm(path, params, method = 'post') {
    var form = document.createElement('form');
    form.method = method;
    form.action = path;

    $.each(params, function (index, obj) {
        var hiddenInput = document.createElement(obj.tipo);
        hiddenInput.type = 'hidden';
        hiddenInput.name = obj.nombre;
        hiddenInput.value = obj.value;
        form.appendChild(hiddenInput);
    });

    return form;
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

mostrarMensaje = function (mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-message');
}

SolicitudCodigoRetiro = function () {
    var controlador = siteRoot + "transferencias/codigoretiro/index/";
    window.location.href = controlador;
}