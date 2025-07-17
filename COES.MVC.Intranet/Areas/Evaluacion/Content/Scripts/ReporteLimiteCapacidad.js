//Declarando las variables 
let controlador = siteRoot + 'ReporteLimiteCapacidad/';
let hot;
let hotOptions;
let evtHot;

$(function () {
    consultar();

    $("#popUpEditar").addClass("general-popup");
    $("#popupImportarPlantilla").addClass("general-popup");
    $("#popupImportarArchivo").addClass("general-popup");

    $('#btnConsultar').click(function () {
        buscarReporte();
    });
    $('#btnCrear').click(function () {
        editar(0);
    });
    $('#btnImportarPlantilla').click(function () {
        abrirImportarPlantilla();
    });

    importarPlantilla();
    importarArchivoReporte();

    $('#btnDescargarPlantilla').click(function () {
        descargarPlantilla();
    });

    let permisoEdicion = $("#hdnNuevo").val();
    if (permisoEdicion == '1') {
        document.getElementById('btnCrear').style.display = 'unset';
    }

    let permisoExportar = $("#hdnExportar").val();
    if (permisoExportar == '1') {
        document.getElementById('btnDescargarPlantilla').style.display = 'unset';
    }

    let permisoImportar = $("#hdnImportar").val();
    if (permisoImportar == '1') {
        document.getElementById('btnImportarPlantilla').style.display = 'unset';        
    }

});

function buscarReporte() {
    consultar();
}

function descargarReporteWord() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GenerarReporteDesdePlantilla',
        data: {},
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = controlador + 'DescargarArchivo?fileName=' + result;
                return true;
            }
            else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}
$.fn.dataTable.ext.type.order['date-dd-mm-yyyy-pre'] = function (date) {

    if (!date) return 0;

    let parts = date.trim().split(' ');
    let dateParts = parts[0].split('/');
    let timeParts = parts[1] ? parts[1].split(':') : ['00', '00'];

    let hour = parseInt(timeParts[0], 10);
    let minute = parseInt(timeParts[1], 10);
    console.log(new Date(dateParts[2], dateParts[1] - 1, dateParts[0], hour, minute).getTime())
    return new Date(dateParts[2], dateParts[1] - 1, dateParts[0], hour, minute).getTime();

};

let consultar = function () {

    const revision = $("#fRevision").val();
    const descripcion = $("#fDescripcion").val();
    setTimeout(function () {
        $.ajax({
            type: 'POST',
            url: controlador + 'ListaReporteLimiteCapacidad?revision=' + revision + "&descripcion=" + descripcion,
            success: function (evt) {
                $('#lista').html(evt);
                $('#tablaListado').dataTable({
                    "iDisplayLength": 25,
                    columnDefs: [
                        { type: 'date-dd-mm-yyyy', targets: 6 }
                    ],
                    language: {
                        info: 'Mostrando página _PAGE_ de _PAGES_',
                        infoEmpty: 'No hay registros disponibles',
                        infoFiltered: '(filtrado de _MAX_ registros totales)',
                        lengthMenu: 'Mostrar _MENU_ registros por página',
                        zeroRecords: 'No se encontró nada'
                    }, order: [[1, 'desc']]
                });

                $('.dataTables_filter input').attr('maxLength', 50);
            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.', 'error');
            }
        });
    }, 100);
};

function editar(e) {
    if (e == 0) {
        $("#span_pop_title").text("Crear reporte");
    } else {
        $("#span_pop_title").text("Modificar reporte");
    }
    
    $.ajax({
        type: 'POST',
        url: controlador + "Editar?id=" + e,
        success: function (evt) {
            $('#editarArea').html(evt);
            $('#mensaje').css("display", "none");
            setTimeout(function () {
                $('#popUpEditar').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);

            $("#btnCerrar").click(function () {
                $("#popUpEditar").bPopup().close();
            });

        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error.', 'error');
        }
    });
};

let guardarReporte = function () {

    if (validarGuardar()) {
        const codigo = $('#Codigo').val();
        const revision = $('#Revision').val();
        const descripcion = $('#Descripcion').val();
        const emitidoEl = $('#EmitidoEl').val();
        const elaboradoPor = $('#ElaboradoPor').val();
        const revisadoPor = $('#RevisadoPor').val();
        const aprobadoPor = $('#AprobadoPor').val();
        const accion = $('#Accion').val();

        let textoConfirmación = "";
        let mensajeSalida = "";
        if (accion == "Crear") {
            textoConfirmación = "¿Esta seguro de crear el registro?";
            mensajeSalida = "Se ha creado el registro correctamente.";
        } else {
            textoConfirmación = "¿Esta seguro de modificar el registro?";
            mensajeSalida = "Se ha modificar el registro correctamente.";
        }

        if (confirm(textoConfirmación)) {

            let datos = {
                Codigo: codigo,
                Revision: revision,
                Descripcion: descripcion,
                EmitidoEl: emitidoEl,
                ElaboradoPor: elaboradoPor,
                RevisadoPor: revisadoPor,
                AprobadoPor: aprobadoPor
            };

            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarReporteLimiteCapacidad',
                dataType: 'json',
                data: datos,
                cache: false,
                success: function (resultado) {
                    if (resultado == 1) {
                        $('#popUpEditar').bPopup().close();
                        mostrarMensaje(mensajeSalida, 'exito');
                        buscarReporte();

                    } else if (resultado == 2) {
                        alert('No se puede crear un reporte con fecha igual o anterior a la fecha del último reporte');
                    } else if (resultado == 3) {
                        alert('No se puede crear un reporte con fecha posterior a la fecha actual');
                    } else {
                        $('#popUpEditar').bPopup().close();
                        mostrarMensaje('Ha ocurrido un error.', 'error');
                    }
                },
                error: function () {
                    mostrarMensaje('Ha ocurrido un error.', 'error');
                }
            });
        }
    }
};
let mostrarMensaje = function (mensaje, tipo) {
    if (tipo == 'error') {
        $('#mensaje').removeClass("action-exito");
        $('#mensaje').addClass("action-error");
        $('#mensaje').text(mensaje);
        $('#mensaje').css("display", "block");
    } else {
        $('#mensaje').removeClass("action-error");
        $('#mensaje').addClass("action-exito");
        $('#mensaje').text(mensaje);
        $('#mensaje').css("display", "block");
    }
};

function eliminar(e) {
    
    if (confirm("¿Esta seguro de eliminar el registro?")) {
        $.ajax({
            type: 'POST',
            url: controlador + "Eliminar?id=" + e,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarMensaje('Se ha eliminado el registro correctamente.', 'exito');
                    buscarReporte();
                } else
                    mostrarMensaje('Ha ocurrido un error.', 'error');

            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.', 'error');
            }
        });
    }
};

function abrirImportarPlantilla() {

    $("#txtNombreArchivo").val('');

    $('#popupImportarPlantilla').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });

    $('#btnCerrarImportarPlantilla').click(function () {
        $("#popupImportarPlantilla").bPopup().close();
    });

    $('#btnCargar').click(function () {
        cargarPlantilla();
    });
    
};

function importarPlantilla() {

    let fecha = new Date();
    let formatofecha = formatDate(fecha);

    $("#FechaPlantilla").val(formatofecha);

    let uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnCargarArchivo',
        multipart_params: {
            "fecha": formatofecha
        },
        container: document.getElementById('container'),
        url: siteRoot + 'ReporteLimiteCapacidad/Upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '10mb',
            mime_types: [
                { title: "Archivos Word .docx", extensions: "docx,doc" }
            ]
        },

        init: {
            PostInit: function () {
            },
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                
                up.refresh();
                up.start();
            },
            UploadComplete: function (up, files) {
                plupload.each(files, function (file) {
                    $('#txtNombreArchivo').val(file.name);
                });
            },
            Error: function (up, err) {
                alert("El archivo a importar no tiene el formato requerido [.docx]");
            }
        }
    });

    uploader.init();
};

function cargarPlantilla() {

    const nombreArchivo = $('#txtNombreArchivo').val();
    const fechaPlantilla = $("#FechaPlantilla").val();

    if (nombreArchivo == '') {
        alert('Debe seleccionar un archivo');
        return;
    }

    const datos = {
        nombreArchivo: nombreArchivo,
        fechaPlantilla: fechaPlantilla
    };

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarPlantilla',
        data: datos,
        dataType: 'json',
        async: false,
        success: function (respuesta) {

            if (respuesta == 1) {
                $("#popupImportarPlantilla").bPopup().close();
                mostrarMensaje("La plantilla fue cargada correctamente.", 'exito');
            } else {
                mostrarMensaje('Ocurrio un error al cargar la plantilla..', 'error');
            }
        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error.', 'error');
        }
    });
};

function formatDate(date) {
    return date.getFullYear().toString() +
        String(date.getMonth() + 1).padStart(2, '0') +
        String(date.getDate()).padStart(2, '0') +
        String(date.getHours()).padStart(2, '0') +
        String(date.getMinutes()).padStart(2, '0') +
        String(date.getSeconds()).padStart(2, '0');
}
let loadValidacionFile = function (mensaje) {
    alert(mensaje);
}

function descargarPlantilla() {    
    window.location = controlador + 'DescargarPlantilla';
}

function abrirImportarArchivo(id, revision) {

    $("#IdReporteLimiteCapacidad").val(id);
    $("#RevisionAAR").val(revision);
    $("#txtNombreArchivoAAR").val('');

    $('#popupImportarArchivo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });

    $('#btnCerrarImportarArchivo').click(function () {
        $("#popupImportarArchivo").bPopup().close();
    });

    $('#btnCargarArchivoAAR').click(function () {
        cargarArchivoReporte();
    });

};

function importarArchivoReporte() {

    let fecha = new Date();
    let formatofecha = formatDate(fecha);

    $("#FechaPlantillaAAR").val(formatofecha);

    let uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSeleccionarArchivoAAR',
        multipart_params: {
            "fecha": formatofecha
        },
        container: document.getElementById('containerAAR'),
        url: siteRoot + 'ReporteLimiteCapacidad/Upload',
        flash_swf_url: '~/Content/Scripts/Moxie.swf',
        silverlight_xap_url: '~/Content/Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '10mb',
            mime_types: [
                { title: "Archivos Word .docx", extensions: "docx,doc" }
            ]
        },

        init: {
            PostInit: function () {
            },
            FilesAdded: function (up, files) {
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }

                up.refresh();
                up.start();
            },
            UploadComplete: function (up, files) {
                plupload.each(files, function (file) {
                    $('#txtNombreArchivoAAR').val(file.name);
                });
            },
            Error: function (up, err) {
                alert("El archivo a importar no tiene el formato requerido [.docx]");
            }
        }
    });

    uploader.init();
};
function cargarArchivoReporte() {

    const nombreArchivo = $('#txtNombreArchivoAAR').val();
    const fechaPlantilla = $("#FechaPlantillaAAR").val();
    const id = $("#IdReporteLimiteCapacidad").val();
    const revision = $("#RevisionAAR").val();

    if (nombreArchivo == '') {
        alert('Debe seleccionar un archivo');
        return;
    }

    const datos = {
        nombreArchivo: nombreArchivo,
        fechaPlantilla: fechaPlantilla,
        revision: revision,
        id: id
    };

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarArchivoReporte',
        data: datos,
        dataType: 'json',
        async: false,
        success: function (respuesta) {

            if (respuesta == 1) {
                $("#popupImportarArchivo").bPopup().close();
                mostrarMensaje("El archivo fue cargado correctamente.", 'exito');
                buscarReporte();
            } else {
                mostrarMensaje('Ocurrio un error al cargar el achivo.', 'error');
            }
        },
        error: function () {
            mostrarMensaje('Ha ocurrido un error.', 'error');
        }
    });
};

function descargarArchivoReporte(id) {
    window.location = controlador + 'DescargarArchivoReporte?id=' + id;
}

function eliminarArchivoReporte(id) {

    if (confirm("¿Esta seguro de eliminar el archivo del registro?")) {
        $.ajax({
            type: 'POST',
            url: controlador + "EliminarArchivoReporte?id=" + id,
            success: function (resultado) {
                if (resultado == 1) {
                    mostrarMensaje('Se ha eliminado el archivo correctamente.', 'exito');
                    buscarReporte();
                } else
                    mostrarMensaje('Ha ocurrido un error.', 'error');

            },
            error: function () {
                mostrarMensaje('Ha ocurrido un error.', 'error');
            }
        });
    }
};

function validarGuardar() {
    let campos = [];

    if ($('#Descripcion').val() == '') campos.push('Descripción');
    if ($('#EmitidoEl').val() == '') campos.push('Emitido el');
    if ($('#ElaboradoPor').val() == '') campos.push('Elaborado por');
    if ($('#RevisadoPor').val() == '') campos.push('Revisado por');
    if ($('#AprobadoPor').val() == '') campos.push('Aprobado por');

    if (campos.length > 0) {

        mensajeValidador(campos);
        return false;
    }

    return true;
}

function mensajeValidador(campos) {
    let texto = "Los campos: \n";

    for (const campo of campos) {
        texto += campo + "(*) \n";
    }

    alert(texto + " son requeridos");
}