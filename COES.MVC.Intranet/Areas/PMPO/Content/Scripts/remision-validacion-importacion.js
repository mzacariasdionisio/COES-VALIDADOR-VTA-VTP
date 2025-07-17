var controler = siteRoot + 'PMPO/ReportSubmission/';

var flagActivarRefresco = "";
var SEG_REFRESCA_LOG = 5;

var gifcargando = '<img src="' + siteRoot + 'Content/Images/loading1.gif" alt="" width="17" height="17" style="padding-left: 17px;">';
var imgOK = '<img src="' + siteRoot + 'Content/Images/ico-done.gif" alt="" width="17" height="17" style="">';
var imgERROR = '<img src="' + siteRoot + 'Content/Images/error.png" alt="" width="17" height="17" style="">';
var imgALERTA = '<img src="' + siteRoot + 'Content/Images/ico-info.gif" alt="" width="17" height="17" style="">';

$(document).ready(function ($) {

    $('#btnGrabar').click(function () {
        validarReporte();

    });

    //Explorar archivos
    adjuntarArchivo();
});

function adjuntarArchivo() {

    uploaderN = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelectFile',
        container: document.getElementById('container'),
        url: controler + 'UploadValidacion',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: true,
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
                mostrarListaArchivos();
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
                alert("Ha ocurrido un error.");
            }
        },
        error: function (err) {
        }
    });
}

function mostrarListaArchivos(opcion) {
    var autoId = "ExcelBuscar";
    document.getElementById('filelist').innerHTML = "";

    $.ajax({
        type: 'POST',
        url: controler + 'MostrarArchivoImportacion',
        data: {
            opcion: opcion
        },
        success: function (result) {
            for (var i = 0; i < result.ListaDocumentos.length; i++) {
                var archivo = result.ListaDocumentos[i];
                document.getElementById('filelist').innerHTML += '<div name="' + archivo.FileName + '" class="file-name" id="' + autoId + i + '">' + archivo.FileName + ' (' + archivo.FileSize + ') <a class="remove-item" href="JavaScript:eliminarFile(\'' + autoId + i + "@" + archivo.FileName + '\');">X</a> <b></b></div>';
            }
        },
        error: function (err) {
        }
    });
}

function validarReporte() {
    var mesElaboracion = $("#txtMesElaboracion").val();

    $('#logProceso').val('');
    flagActivarRefresco = 'N';
    $("#mensaje_log").hide();
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controler + "ImportarValidacionFormatosExcel",
        data: {
            fechaProceso: mesElaboracion,
        },
        success: function (evt) {
            if (evt.Resultado != '-1' && evt.Resultado != '-5') {

                if (evt.Resultado == "0") {
                    flagActivarRefresco = 'N';
                } else {
                    flagActivarRefresco = 'S';
                    mostrarLogProceso();
                }
            } else {
                if (evt.Resultado == '-5') {
                    alert(evt.Mensaje);
                    $('#Message').html("<div class='action-error mensajes'>" + evt.Mensaje + "</div>" +
                        "<div class='action-message mensajes'>" + 'Se generó un archivo archivo Excel con los datos no validos' + "</div>");

                    window.location.href = controler + 'DescargarExcelLog?archivo=' + evt.PathLog + "&nombre=" + evt.NameFileLog;
                }
                else {
                    alert(evt.Mensaje);
                }

                flagActivarRefresco = 'N';
            }

        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error");
        }
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
    $('#listado_mensaje').html('');
}

//

function mostrarLoadingValidacion() {
    setInterval(function () {
        if (flagActivarRefresco == "S") {
            mostrarLogProceso();
        } else {
            $("#mensaje_log").hide();
        }
    }, SEG_REFRESCA_LOG * 1000);
}

function mostrarLogProceso() {
    var mesElaboracion = $("#txtMesElaboracion").val();


    $.ajax({
        type: 'POST',
        url: controlador + 'ListaLogxMes',
        dataType: 'json',
        data: {
            mes: mesElaboracion,
        },
        success: function (model) {
            if (model.Resultado != "-1") {
                if (model.Envio != null) {
                    $("#div_log").show();
                    $("#log_cod_envio").html(model.Envio.Enviocodi);
                    $("#log_usu_envio").html(model.Envio.Userlogin);
                    $("#log_fecha_envio").html(model.Envio.EnviofechaDesc);

                    $('#div_log_detalle').html(dibujarTablaLog(model.ListaLog));
                }

                if (flagActivarRefresco == 'S' && model.Resultado == "1") { //si estaba procesando y termina entonces actualizar el listado de archivos
                    mostrarListaArchivos();
                }

                flagActivarRefresco = model.Resultado == "0" ? 'S' : 'N';

                if (model.Resultado == "0") {

                    var porcentaje = model.Envio != null ? model.Envio.Enviodesc : '';
                    if (porcentaje != '-1') {
                        $("#mensaje_log").show();
                        var htmlMsj = `Procesamiento de Resultados de Carga masiva en ejecución. Porcentaje de avance: ${porcentaje}%. ${gifcargando}`;
                        mostrarMensaje("mensaje_log", htmlMsj, $tipoMensajeAlerta, $modoMensajeCuadro);
                    }
                }
                else {
                }
            } else {
                flagActivarRefresco = 'N';
                mostrarMensaje("mensaje_log", model.Mensaje, $tipoMensajeAlerta, $modoMensajeCuadro);
                $("#mensaje_log").show();
            }
        },
        error: function (err) {
            flagActivarRefresco = 'N';
            mostrarMensaje("mensaje", 'Ha ocurrido un error', $tipoMensajeError, $modoMensajeCuadro);
        }
    });

}

function dibujarTablaLog(lista) {
    if (lista.length == 0)
        return "<b></b>";

    var cadena = '';
    cadena += `
    <table border="0" class="pretty tabla-icono dataTable" cellspacing="0" width="100%" id="reporteLog">
        <thead>
            <tr>
                <th style='width: 40px'>Estado</th>
                <th style='width: 70px'>Fecha</th>
                <th style='width: 70px'>Hora</th>
                <th style='width: 400px'>Descripción</th>
            </tr>
        </thead>
        <tbody>
    `;

    for (key in lista) {
        var item = lista[key];
        var strIEstado = '';
        switch (item.Pmologtipo) {
            case 3:
                strIEstado = '<a title="">' + imgOK + '</a>';
                break;
            case 4:
                strIEstado = '<a title="">' + imgERROR + '</a>';
                break;
            case 5:
                strIEstado = '<a title="">' + imgALERTA + '</a>';
                break;
            default:
                strIEstado = item.PmologtipoDesc;
                break;
        }

        cadena += `
            <tr>
                <td style="height: 24px">${strIEstado}</td>
                <td style="height: 24px">${item.FechaDesc}</td>
                <td style="height: 24px">${item.HoraDesc}</td>
                <td style="height: 24px; text-align: left;padding-left: 10px;">${item.LogDesc}</td>
            </tr>
        `;
    }
    cadena += "</tbody></table>";

    return cadena;
}