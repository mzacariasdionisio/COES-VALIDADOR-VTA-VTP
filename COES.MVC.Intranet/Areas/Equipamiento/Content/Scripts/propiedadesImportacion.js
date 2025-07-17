var controler = siteRoot + "Equipamiento/Propiedad/";

$(document).ready(function ($) {

    $('#btnGrabar').click(function () {
        validarReporte(document.getElementById('ExcelBuscar').getAttribute("name"));

    });

    $('#btnRegresar').click(function () {
        window.location.href = controler + "Index";
    });

    //$('#btnCancel').click(function () {
    //    $('#popup2').bPopup().close();
    //});

    //Explorar archivos
    adjuntarArchivo();
});

function adjuntarArchivo() {
    var fullDate = new Date();
    var twoDigitMonth = ((fullDate.getMonth().length) == 1) ? '0' + (fullDate.getMonth() + 1) : (fullDate.getMonth() + 1);
    var sFecha = fullDate.getFullYear().toString() + twoDigitMonth.toString() + fullDate.getDate().toString() + fullDate.getHours().toString() + fullDate.getMinutes().toString() + fullDate.getSeconds().toString();
    var comb = sFecha;

    uploaderN = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: controler + 'UploadPropiedad?sFecha=' + comb,
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" },
            ]
        },
        init: {
            PostInit: function () {
                $('#loadingcarga').css('display', 'none');
                document.getElementById('filelist').innerHTML = '';
            },

            FilesAdded: function (up, files) {
                document.getElementById('filelist').innerHTML = '';
                $('#container').css('display', 'block');
                for (i = 0; i < uploaderN.files.length; i++) {
                    var file = uploaderN.files[i];
                }
                uploaderN.start();
            },
            UploadProgress: function (up, file) {
            },
            UploadComplete: function (up, file) {
                limpiarInterfaz();

                $('#container').css('display', 'block');
                $('#loadingcarga').css('display', 'none');

                //Envia la fecha y el nombre del archivo seleccionado
                mostarListaArchivos(comb, file[file.length - 1].name);
            },
            Error: function (up, err) {
                $('#container').css('display', 'true');
                if (err.code == -601) {
                    document.getElementById('filelist').innerHTML = '<div class="action-alert">' + 'La extensión del archivo no es válido' + '</div>';
                }
            }
        }
    });

    uploaderN.init();
}

function eliminarFile(id) {
    var string = id.split("@");
    var idProp = string[0];
    var nombreArchivo = string[1];

    uploaderN.removeFile(idProp);

    limpiarInterfaz();

    $.ajax({
        type: 'POST',
        url: controler + 'EliminarArchivosImportacionNuevo',
        data: { nombreArchivo: nombreArchivo },
        success: function (result) {
            if (result == -1) {
                $('#' + idProp).remove();
            } else {
                alert("Algo salio mal");
            }
        },
        error: function (err) {
        }
    });
}

function mostarListaArchivos(fechaUp, fileName) {
    var autoId = "ExcelBuscar";

    $.ajax({
        type: 'POST',
        url: controler + 'MostrarArchivoImportacion',
        data: { sFecha: fechaUp, sFilename: fileName },
        success: function (result) {
            var archivo = result.Documento;
            document.getElementById('filelist').innerHTML = '<div name="' + archivo.FileName + '" class="file-name" id="' + autoId + '">' + archivo.FileName + ' (' + archivo.FileSize + ') <a class="remove-item" href="JavaScript:eliminarFile(\'' + autoId + "@" + archivo.FileName + '\');">X</a> <b></b></div>';
        },
        error: function (err) {
        }
    });
}

function validarReporte(nameFilem) {
    $.ajax({
        type: 'POST',
        url: controler + "ImportarPropiedadesExcel",
        datatype: 'json',
        data: JSON.stringify({ filename: nameFilem }),
        contentType: "application/json",
        success: function (evt) {
            if (evt.Resultado != '-1') {
                $('#Message').html("<div class='action-exito mensajes'>" + evt.StrMensaje + "</div>");
                $("#listado").html("");
            } else {
                alert(evt.StrMensaje);

                var ancho = parseInt($('#mainLayout').width()) - 30;
                $('#listado').css("width", ancho + "px");

                if (evt.ListaPropiedadesErrores.length > 0) {
                    var html = '';
                    html += returnTable();
                    $.each(evt.ListaPropiedadesErrores, function (i, val) {
                        html += returnBody(val);
                    });
                    html += "</tbody>";
                    html += "</table>";

                    $('#Message').html("<div class='action-error mensajes'>" + evt.StrMensaje + "</div>" +
                        "<div class='action-message mensajes'>" + 'Se generó un archivo csv con los datos no validos' + "</div>");
                    $("#listado").html("");
                    $(html).appendTo('#listado');

                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.FileName }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivoCSV', paramList);
                    document.body.appendChild(form);
                    form.submit();
                }
                else {
                    if (evt.ListaPropiedadesCorrectas != 0) {
                        $('#Message').html("<div class='action-message mensajes'>" + evt.StrMensaje + "</div>");
                    } else {
                        $('#Message').html("<div class='action-error mensajes'>" + evt.StrMensaje + "</div>");
                    }
                }

                $('#tablaReporte').dataTable({
                    "scrollX": true,
                    "bAutoWidth": false,
                    "destroy": "true",
                    "order": [[0, "asc"]],
                    "iDisplayLength": 50,
                });
            }

        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error");
        }
    });
}

//Construir tabla html con header
function returnTable() {
    var html = '';
    html += "<table border='0' class='pretty tabla-icono' id='tablaReporte'>";
    html += "<thead>";
    html += "<tr>";

    html += "<th style='text-align:left'>ÍTEM</th>";
    html += "<th style='text-align:left;width: 500px'>OBSERVACIONES</th>";
    html += "<th style='text-align:left'>CÓDIGO DE <br/>PROPIEDAD</th>";
    html += "<th style='text-align:left'>NOMBRE</th>";
    html += "<th style='text-align:left'>NOMBRE DE <br/>FICHA TÉCNICA</th>";

    html += "<th style='text-align:left'>ABREVIATURA</th>";
    html += "<th style='text-align:left'>DEFINICIÓN</th>";
    html += "<th style='text-align:left'>UNIDAD</th>";
    html += "<th style='text-align:left'>TIPO DE DATO</th>";
    html += "<th style='text-align:left'>LONGITUD 1</th>";

    html += "<th style='text-align:left'>LONGITUD 2</th>";
    html += "<th style='text-align:left'>FICHA TÉCNICA</th>";
    html += "<th style='text-align:left'>CÓDIGO DE TIPO <br/>DE EQUIPO</th>";
    html += "<th style='text-align:left'>NOMBRE DE TIPO <br/>DE EQUIPO</th>";

    html += "</tr>";
    html += "</thead>";
    html += "<tbody>";

    return html;
}

//Construir body de la tabla html
function returnBody(model) {
    var html = '';
    html += '<tr id="number_' + model.Propcodi + '">';
    html += '<td>' + model.NroItem + '</td>';
    html += '<td style="text-align:left;width: 500px">' + model.Observaciones + '</td>';
    html += '<td>' + (model.Propcodi > 0 ? model.Propcodi : "") + '</td>';
    html += '<td>' + model.Propnomb + '</td>';
    html += '<td>' + model.Propnombficha + '</td>';

    html += '<td>' + model.Propabrev + '</td>';
    html += '<td>' + model.Propdefinicion + '</td>';
    html += '<td>' + model.Propunidad + '</td>';
    html += '<td>' + model.Proptipo + '</td>';
    html += '<td>' + (model.Proptipolong1 ?? "") + '</td>';

    html += '<td>' + (model.Proptipolong2 ?? "") + '</td>';
    html += '<td>' + model.Propfichaoficial + '</td>';
    html += '<td>' + model.Famcodi + '</td>';
    html += '<td>' + model.NombreFamilia + '</td>';
    html += '</tr>';

    return html;
}


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

function limpiarInterfaz() {
    $('#Message').html('');
    $('#listado').html('');
}