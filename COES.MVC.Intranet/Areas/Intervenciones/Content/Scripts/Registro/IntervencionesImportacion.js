var controler = siteRoot + "Intervenciones/Registro/";

$(document).ready(function ($) {

    $('.txtFecha').Zebra_DatePicker({

    });

    $('.btnok').click(function () { // Reemplazar

        if (document.getElementById('ExcelBuscar')) {
            validarReporte(document.getElementById('ExcelBuscar').getAttribute("name"), '1');
        }
        else {
            alert('Seleccione un documento en formato Excel');
        }
    });

    $('#btnOkAdicionar').click(function () { // Adicionar

        if (document.getElementById('ExcelBuscar')) {
            validarReporte(document.getElementById('ExcelBuscar').getAttribute("name"), '2');
        }
        else {
            alert('Seleccione un documento en formato Excel');
        }
    });

    $('#btnOkAdicionar2').click(function () {

        if (document.getElementById('ExcelBuscar')) {
            validarReporte(document.getElementById('ExcelBuscar').getAttribute("name"), '2');
        }
        else {
            alert('Seleccione un documento en formato Excel');
        }
    });


    $('#btnValidarFile').click(function () {
        var tipoProgramacion = document.getElementById('TipoProgramacion').value;

        if (document.getElementById('ExcelBuscar')) {

            abrirPopup(tipoProgramacion);
        }
        else {
            alert('Seleccione un documento en formato Excel');
        }
    });

    $('#btnLimpiarFile').click(function () {
        location.reload();
    });

    $('#btnRegresar').click(function () {
        var progrCodi = parseInt(document.getElementById('idProgramacion').value) || 0;
        window.location.href = controler + 'IntervencionesRegistro?progCodi=' + progrCodi;
    });

    $('#btnCancel').click(function () {
        $('#popup2').bPopup().close();
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
        url: controler + 'UploadIntervencion?sFecha=' + comb,
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '100mb',
            mime_types: [
                { title: "Archivos Excel con macros .xlsm", extensions: "xlsm" },
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
            },
            FileUploaded: function (up, file, result) {
                var autoid = "ExcelBuscar";
                var value = JSON.parse(result.response);
                document.getElementById('filelist').innerHTML = '<div name="' + value.nuevonombre + '" class="file-name" id="' + autoid + '">' + file.name + ' (' + (file.size / 1000) + 'KB) <a class="remove-item" href="JavaScript:eliminarFile(\'' + autoid + "@" + value.nuevonombre + '\');">X</a> <b></b></div>';

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
    var idinter = string[0];

    uploaderN.removeFile(idinter);

    limpiarInterfaz();

    $('#' + idinter).remove();
}

function validarReporte(nameFilem, sAccion) {
    var tipoProgramacion = document.getElementById('TipoProgramacion').value;
    var progrCodi = document.getElementById('idProgramacion').value;

    $.ajax({
        type: 'POST',
        url: controler + "ImportarIntervencionesExcel",
        datatype: 'json',
        data: JSON.stringify({ progrCodi: progrCodi, tipoProgramacion: tipoProgramacion, filename: nameFilem, accion: sAccion }),
        contentType: "application/json",
        success: function (evt) {
            if (evt.Resultado != '-1') {
                $('#Message').html("<div class='action-exito mensajes'>" + evt.StrMensaje + "</div>");
                $("#listado").html("");
            } else {
                alert(evt.StrMensaje);

                if (evt.ListaIntervencionesErrores.length > 0) {
                    var html = '';
                    html += returnTable();
                    $.each(evt.ListaIntervencionesErrores, function (i, val) {
                        html += returnBody(val);
                    });
                    html += "</tbody>";
                    html += "</table>";

                    $('#Message').html("<div class='action-error mensajes'>" + evt.StrMensaje + "</div>" +
                        "<div class='action-message mensajes'>" + 'Se generó un archivo csv con los datos no validos' + "</div>");
                    $("#listado").html("");
                    $(html).appendTo('#listado');

                    //Observación parametros en la URL - Modificado 15/05/2019
                    var paramList = [
                        { tipo: 'input', nombre: 'file', value: evt.FileName }
                    ];
                    var form = CreateForm(controler + 'AbrirArchivoCSV', paramList);
                    document.body.appendChild(form);
                    form.submit();
                    //:fin

                }
                else {
                    if (evt.ListaIntervencionesCorrectas != 0) {
                        $('#Message').html("<div class='action-message mensajes'>" + evt.StrMensaje + "</div>");
                    } else {
                        $('#Message').html("<div class='action-error mensajes'>" + evt.StrMensaje + "</div>");
                    }
                }

                $('#tabla').dataTable({
                    scrollY: 330,
                    destroy: true,
                    "order": [[0, "asc"]],
                    "iDisplayLength": 50
                });
            }

            $('#popup').bPopup().close();

        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error");
        }
    });
}

//Construir tabla html con header
function returnTable() {
    var html = '';
    html += "<table border='0' class='pretty tabla-icono' id='tabla'>";
    html += "<thead>";
    html += "<tr>";

    html += "<th style='text-align:left'>Fila</th>";
    html += "<th style='text-align:left;'>Observacion</th>";
    html += "<th style='text-align:left'></th>";
    html += "<th style='text-align:left'></th>";
    html += "<th style='text-align:left'></th>";

    html += "<th style='text-align:left'></th>";
    html += "<th style='text-align:left'></th>";
    html += "<th style='text-align:left'></th>";
    html += "<th style='text-align:left'></th>";
    html += "<th style='text-align:left'></th>";

    html += "<th style='text-align:left'></th>";
    html += "<th style='text-align:left'></th>";
    html += "<th style='text-align:left'></th>";
    html += "<th style='text-align:left'></th>";
    html += "<th style='text-align:left'></th>";

    html += "</tr>";
    html += "</thead>";
    html += "<tbody>";

    return html;
}

//Construir body de la tabla html
function returnBody(model) {
    var html = '';
    if (model.ChkMensaje) {
        html += '<tr id="number_' + model.Intercodi + '">';
        html += '<td>' + model.NroItem + '</td>';
        html += '<td colspan=14 style="text-align:left">' + model.Observaciones + '</td>';

        html += '<td style="display: none;"></td>';
        html += '<td style="display: none;"></td>';
        html += '<td style="display: none;"></td>';
        html += '<td style="display: none;"></td>';
        html += '<td style="display: none;"></td>';

        html += '<td style="display: none;"></td>';
        html += '<td style="display: none;"></td>';
        html += '<td style="display: none;"></td>';
        html += '<td style="display: none;"></td>';
        html += '<td style="display: none;"></td>';

        html += '<td style="display: none;"></td>';
        html += '<td style="display: none;"></td>';
        html += '<td style="display: none;"></td>';

        html += '</tr>';

    } else {
        html += '<tr id="number_' + model.Intercodi + '">';
        html += '<td>' + model.NroItem + '</td>';
        html += '<td>' + model.Observaciones + '</td>';
        html += '<td>' + model.Intercodsegempr + '</td>';
        html += '<td>' + model.EmprNomb + '</td>';
        html += '<td>' + model.AreaNomb + '</td>';

        html += '<td>' + model.EquiNomb + '</td>';
        html += '<td>' + model.Equicodi + '</td>';
        html += '<td>' + model.InterfechainiDesc + '</td>';
        html += '<td>' + model.InterfechafinDesc + '</td>';
        html += '<td>' + model.Interdescrip + '</td>';

        html += '<td>' + model.Intermwindispo + '</td>';
        html += '<td>' + model.Interindispo + '</td>';
        html += '<td>' + model.Interinterrup + '</td>';
        html += '<td>' + model.IntNombTipoIntervencion + '</td>';
        html += '<td>' + model.IntNombClaseProgramacion + '</td>';
        html += '</tr>';
    }

    return html;
}

function abrirPopup(tipoProgramacion) {
    $('#popup').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
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