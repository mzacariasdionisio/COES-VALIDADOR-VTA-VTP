var controlador = siteRoot + 'Eventos/AnalisisFallas/';

$(function () {
    $('#dtInicio').Zebra_DatePicker({
    });

    $('#dtFin').Zebra_DatePicker({
    });

    $("#btnConsultar").click(function () {
        Consultar();
    });
    $("#btnExportar").click(function () {
        exportarReporte();
    });

    ObtenerListado();
});

function verDetalleEvento(valor) {
    location.href = controlador + "Interrupcionsuministro?id=" + valor;
};

function ObtenerListado() {
    var miDataM = getObjFiltro();
    $("#listado").html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ListadoSolicitudes',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(miDataM),
        success: function (eData) {
            $('#listado').css("width", $('.form-main').width() + "px");

            $('#listado').html(eData);
            $('#tabla').dataTable({
                bJQueryUI: true,
                "scrollY": 440,
                "scrollX": false,
                "sDom": 'ft',
                "ordering": true,
                "order": [[0, 'asc']],
                "iDisplayLength": -1
            });
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function getObjFiltro() {
    var cboEmpresa = $("#cbEmpresa").val();
    var dtInicio = $("#dtInicio").val();
    var dtFin = $("#dtFin").val();
    var cboEstado = $("#cboEstado").val();

    var Estado = "T";

    if (cboEstado != "TODOS") {
        if (cboEstado == "PENDIENTE") {
            Estado = "P";
        } else if (cboEstado == "ATENDIDO") {
            Estado = "A";
        } 
    }

    var miDataM = {
        Emprcodi: cboEmpresa,
        EstadoSoli: Estado,
        DI: dtInicio,
        DF: dtFin
    };

    return miDataM;
}

function Consultar() {
    ObtenerListado();
}

function VerArchivosAdjuntados(codSoli) {
    abrirPopupArchivos(codSoli);
}

function EditarSolicitud(codSoli) {
    abrirPopupEdicion(codSoli);
}

function abrirPopupArchivos(idtSoli) {
    $.ajax({
        type: 'POST',
        url: controlador + "VerArchivosAdjuntados",
        data: {
            codSoli: idtSoli
        },
        success: function (evt) {
            $('#contenidoDetalle').html(evt);
            var excep_resultado = $("#hdResultado_ED").val();
            var excep_mensaje = $("#hdMensaje_ED").val();

            if (excep_resultado !== "-1") {
                setTimeout(function () {
                    $('#popupArchivosAdjuntados').bPopup({
                        autoClose: false,
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 500);
            } else {
                $('#contenidoDetalle').html('');
                alert(excep_mensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}


function abrirPopupEdicion(idtSoli) {
    $.ajax({
        type: 'POST',
        url: controlador + "EditarSolicitud",
        data: {
            codSoli: idtSoli
        },
        success: function (evt) {
            $('#contenidoEdicion').html(evt);
            var excep_resultado = $("#hdResultado_ED").val();
            var excep_mensaje = $("#hdMensaje_ED").val();

            if (excep_resultado !== "-1") {
                setTimeout(function () {
                    $('#popupEditarSolicitud').bPopup({
                        autoClose: false,
                        easing: 'easeOutBack',
                        speed: 450,
                        transition: 'slideDown'
                    });
                }, 500);

                inicializaEdicion();
            } else {
                $('#contenidoEdicion').html('');
                alert(excep_mensaje);
            }
        },
        error: function (err) {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Exportación
function exportarReporte() {
    var miDataM = getObjFiltro();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarReporteSolicitudes',
        data: JSON.stringify(miDataM),
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.Resultado !== "-1") {
                var paramList = [
                    { tipo: 'input', nombre: 'file1', value: result.NombreArchivo },
                ];
                var form = CreateForm(controlador + 'Exportar', paramList);
                document.body.appendChild(form);
                form.submit();
            }
            else {
                alert('Ha ocurrido un error');
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error');
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

////////////////////////////////////////////////////////////////////////////////////////////////////////
/// edición solicitud
function inicializaEdicion() {
    var estadoEdit = $("#cboEstado2").val();
    if (estadoEdit != "Atendido") {
        $(".fila_comentario").css("display", "none");
        $("#ArchivoObs").css("display", "none");
    }
    else {
        $(".fila_comentario").css("display", "table-cell");
        $("#ArchivoObs").css("display", "block");
    }

    $("#cboEstado2").change(function () {
        var data = $(this).val();
        if (data != "Atendido") {
            $(".fila_comentario").css("display", "none");
            $("#ArchivoObs").css("display", "none");
        }
        else {
            $(".fila_comentario").css("display", "table-cell");
            $("#ArchivoObs").css("display", "block");
        }
    });

    $("#btnCancelarEdicion").click(function () {
        $('#popupEditarSolicitud').bPopup().close();
    });

    $('#btnGrabar').click(function () {
        ActualizarSolicitud();
    });

    adjuntarArchivo();
}

function ActualizarSolicitud() {
    var cboEstado = $("#cboEstado2").val();
    var comentario = $("#txtComentario").val();
    var idEmrpesa = $("#idEmpresa").val();
    var idEnvio = $("#idEnvio").val();
    var idDescEvento = $("#idDescEvento").val();
    
    var Estado;
    var IdSolicitud = $("#idSoliEdit").val();

    if (cboEstado == "Pendiente") {
        Estado = "P";
    } else if (cboEstado == "Atendido") {
        Estado = "A";
    }

    var solicitud = {
        Sorespcodi: IdSolicitud,
        Sorespestadosol: Estado,
        Sorespobs: comentario,
        Enviocodi: idEnvio,
        Emprcodi: idEmrpesa,
        Sorespdesc: idDescEvento
    }
    $.ajax({
        type: 'POST',
        url: controlador + 'SolicitudUpdate',
        contentType: "application/json; charset=utf-8",

        data: JSON.stringify(solicitud),

        success: function (eData) {
            if (eData.Resultado == '-1') {
                alert(eData.StrMensaje);
            }
            else {
                if (eData.Resultado != "") {
                    if (eData.Resultado == "1") {
                        alert(eData.StrMensaje);
                        location.href = controlador + "IndexSolicitudes";
                    }
                    else
                        alert(eData.StrMensaje);
                }
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function adjuntarArchivo() {
    var nombreModulo = document.getElementById('NombreModulo').value;
    var fullDate = new Date();
    var twoDigitMonth = ((fullDate.getMonth().length) == 1) ? '0' + (fullDate.getMonth() + 1) : (fullDate.getMonth() + 1);
    var sFecha = fullDate.getFullYear().toString() + twoDigitMonth.toString() + fullDate.getDate().toString() + fullDate.getHours().toString() + fullDate.getMinutes().toString() + fullDate.getSeconds().toString();

    uploaderN = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnSelectFile",
        url: controlador + 'UploadSolicitudes?sFecha=' + sFecha + '&sModulo=' + nombreModulo,
        multi_selection: false,
        filters: {
            max_file_size: '10mb',
            mime_types: [
                { title: "Archivos Word .doc", extensions: "doc" },
                { title: "Archivos Word .docx", extensions: "docx" },
                { title: "Archivos pdf .pdf", extensions: "pdf" }
            ]
        },
        init: {
            PostInit: function () {
                mostrarListaArchivos();
            },
            FilesAdded: function (up, files) {
                document.getElementById('filelist').innerHTML = '';
                $('#container').css('display', 'block');

                if (uploaderN.files.length === 2) {
                    uploaderN.removeFile(uploaderN.files[0]);
                }
                for (i = 0; i < uploaderN.files.length; i++) {
                    var file = uploaderN.files[i];
                }

                uploaderN.start();
            },
            UploadProgress: function (up, file) {
            },
            UploadComplete: function (up, file) {
                $('#container').css('display', 'block');
                mostrarListaArchivos();
            },
            Error: function (up, err) {
                if (err.code === -600) {
                    alert("La capacidad máxima del archivo es de 10Mb..... \nSeleccionar archivo con el tamaño adecuado."); return;
                }
            }
        }
    });
    uploaderN.init();
}

function mostrarListaArchivos() {
    var autoId = 0;
    var nombreModulo = document.getElementById('NombreModulo').value;

    $.ajax({
        type: 'POST',
        url: controlador + 'ListaArchivosNuevo',
        data: { sModulo: nombreModulo },
        success: function (result) {
            var listaArchivos = result.ListaDocumentosFiltrado;

            $.each(listaArchivos, function (index, value) {
                autoId++;
                document.getElementById('filelist').innerHTML += '<div class="file-name" id="' + autoId + '">' + value.FileName + ' (' + value.FileSize + ') <a class="remove-item" href="JavaScript:eliminarFile(\'' + autoId + "@" + value.FileName + '\');">X</a> <b></b></div>';
            })
        },
        error: function () {
        }
    });
}

function eliminarFile(id) {
    var string = id.split("@");
    var idInter = string[0];
    var nombreArchivo = string[1];

    uploaderN.removeFile(idInter);

    $.ajax({
        type: 'POST',
        url: controlador + 'EliminarArchivosNuevo',
        data: { nombreArchivo: nombreArchivo },
        success: function (result) {
            if (result == -1) {
                $('#' + idInter).remove();
            } else {
                alert("Algo salió mal");
            }
        },
        error: function () {
        }
    });
}


function descargarInforme(idSoli, nombreArchivo, tipoInforme) {
    window.location = controlador + 'DescargarInforme?nameArchivo=' + nombreArchivo + '&idSoli=' + idSoli + '&tipoArchivo=' + tipoInforme;
}