var controler = siteRoot + "Equipamiento/Equipo/";

$(document).ready(function ($) {

    $('#btnGrabar').click(function () {
        validarReporte(document.getElementById('ExcelBuscar').getAttribute("name"));

    });

    $('#btnRegresar').click(function () {
        window.location.href = controler + "Index";
    });

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
        url: controler + 'UploadEquipo?sFecha=' + comb,
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
    var idEquipo = string[0];
    var nombreArchivo = string[1];

    uploaderN.removeFile(idEquipo);

    limpiarInterfaz();

    $.ajax({
        type: 'POST',
        url: controler + 'EliminarArchivosImportacionNuevo',
        data: { nombreArchivo: nombreArchivo },
        success: function (result) {
            if (result == -1) {
                $('#' + idEquipo).remove();
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
        url: controler + "ImportarRelEquipoProyectoExcel",
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

                if (evt.ListaReleqpryErrores.length > 0) {
                    var html = '';
                    html += returnTable();
                    $.each(evt.ListaReleqpryErrores, function (i, val) {
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
                    if (evt.ListaReleqpryCorrectos != 0) {
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
    html += "<th style='text-align:left;'>OBSERVACIONES</th>";
    html += "<th style='text-align:left'>CÓDIGO PROYECTO</th>";
    html += "<th style='text-align:left'>Empresa PROYECTO</th>";
    html += "<th style='text-align:left'>Código Estudio (EO)</th>";

    html += "<th style='text-align:left'>CÓDIGO DE EQUIPO</th>";
    html += "<th style='text-align:left'>NOMBRE DE EQUIPO</th>";
    html += "<th style='text-align:left'>ABREVIATURA</th>";
    html += "<th style='text-align:left'>ESTADO</th>";
    html += "<th style='text-align:left'>EMPRESA</th>";
    html += "<th style='text-align:left'>TIPO DE EQUIPO</th>";
    html += "<th style='text-align:left'>UBICACIÓN</th>";

    html += "</tr>";
    html += "</thead>";
    html += "<tbody>";

    return html;
}

//Construir body de la tabla html
function returnBody(model) {
    var html = '';
    html += '<tr id="number_' + model.Equicodi + '">';
    html += '<td>' + model.NroItem + '</td>';
    html += '<td>' + model.Observaciones + '</td>';
    html += '<td>' + (model.Ftprycodi) + '</td>';
    html += '<td>' + (model.Emprnomb) + '</td>';
    html += '<td>' + (model.Ftpryeocodigo) + '</td>';
    html += '<td>' + (model.Equicodi) + '</td>';

    html += '<td>' + (model.Equinomb ?? "") + '</td>';
    html += '<td>' + (model.Equiabrev ?? "") + '</td>';
    html += '<td>' + (model.Equiestadodesc ?? "") + '</td>';
    html += '<td>' + (model.Emprnomb2 ?? "") + '</td>';
    html += '<td>' + (model.Famnomb ?? "") + '</td>';
    html += '<td>' + (model.Areanomb ?? "") + '</td>';

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