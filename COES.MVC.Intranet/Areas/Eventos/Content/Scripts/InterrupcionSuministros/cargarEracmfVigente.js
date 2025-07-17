var controlador = siteRoot + 'Eventos/AnalisisFallas/';
var uploader;

$(function () {
    var idEvento = $("#hCodiEvento").val();

    $('#btnSeleccionarArchivo').click(function () {
        mostrarMensaje('mensaje', 'message', "Por favor seleccione el archivo ERACMF.");
    });

    $('#btnProcesarArchivo').click(function () {
        uploader.start();
    });

    $("#btnRegresar").click(function () {        
        window.location.href = controlador + 'InterrupcionSuministros';
    });

    AdjuntarERACMF(idEvento);

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });

    listarArchivoEracmf(idEvento);
});

function AdjuntarERACMF(evencodi) {

    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: "btnSeleccionarArchivo",

        url: controlador + "CargarArchivoERACMF",
        multi_selection: false,
        filters: {
            max_file_size: 0,
            mime_types: [
                { title: "Archivos xls", extensions: "xls,xlsx" },
            ]
        },
        multipart_params: {
            codigoEvento: evencodi
        },
        init: {
            FilesAdded: function (up, files) {
                if (uploader.files.length === 2) {
                    uploader.removeFile(uploader.files[0]);
                }

                $("#txtNombreArchivo").css('display', 'inline-block');
                for (i = 0; i < files.length; i++) {
                    var file = files[i];
                    $("#txtNombreArchivo").html(file.name);
                }
                $('#btnProcesarArchivo').removeAttr("disabled");
            },
            UploadProgress: function (up, file) {
                mostrarMensaje('mensaje', 'alert', "Procesando Archivo, por favor espere ...(<strong>" + (file.percent - 1) + "%</strong>)");
                $('#listado').html('');
                $('#listado2').html('');
                $("#txtAuditoria").html('');
                $('#btnSeleccionarArchivo').prop('disabled', 'disabled');
                $('#btnProcesarArchivo').prop('disabled', 'disabled');
            },

            FileUploaded: function (up, file, info) {
                $("#txtNombreArchivo").hide();
                $("#txtNombreArchivo").html('');
                
                var aData = JSON.parse(info.response);
                if (aData.Resultado != '-1') {
                    mostrarListaEracmf(aData);
                    mostrarListaErrores(aData);

                    mostrarMensaje('mensaje', 'exito', "El Archivo ERACMF fue procesado exitosamente.");
                    if (aData.Resultado2 != null && aData.Resultado2 != '') {
                        mostrarMensaje('mensaje', 'exito', "El Archivo ERACMF fue procesado exitosamente. Las celda que tuvieron datos erróneos se guardaron con su valor por defecto(Para campos numéricos el 0).");
                    }
                } else {
                    mostrarMensaje('mensaje', 'message', "Por favor seleccione el archivo ERACMF.");
                    mostrarMensaje('mensaje', 'alert', aData.StrMensaje);
                    //alert(aData.StrMensaje);
                }
                $('#btnSeleccionarArchivo').removeAttr("disabled");
                $('#btnProcesarArchivo').prop('disabled', 'disabled');
            },
            UploadComplete: function (up, file) {
            },
            Error: function (up, err) {
                $('#btnProcesarArchivo').prop('disabled', 'disabled');
                mostrarMensaje('mensaje', 'alert', err.message);
                //alert(err.code + "-" + err.message);
                //if (err.code === -600) {
                //    alert("La capacidad máxima de superada..... "); return;
                //}
            }
        }
    });

    uploader.init();
}

function listarArchivoEracmf(id) {
    $('#listado').html('');

    $.ajax({
        type: 'POST',
        url: controlador + 'ListarArchivoEracmf',
        data: {
            id: id
        },
        success: function (aData) {
            if (aData.Resultado != '-1') {
                mostrarListaEracmf(aData);
            } else {
                alert(aData.StrMensaje);
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function mostrarListaEracmf(aData) {
    $('#listado').html('');
    $("#txtAuditoria").html('');
    var usuario = aData.UltimaModificacionUsuarioDesc;
    var fechaAct = aData.UltimaModificacionFechaDesc;

    if (usuario != '') {
        $("#txtAuditoria").html(`Usuario actualización: <b>${usuario}</b>, Fecha actualización: <b>${fechaAct}</b>`);

        $('#listado').css("width", $('#mainLayout').width() + "px");
        $('#listado').html(aData.Resultado);

        $('#tabla').dataTable({
            bJQueryUI: true,
            "scrollY": 340,
            "scrollX": true,
            "sDom": 'ft',
            "ordering": true,
            "iDisplayLength": -1
        });
    }
}

function mostrarListaErrores(aData) {
    $('#listado2').html('');

    //Tabla de errores
    if (aData.Resultado2 != null && aData.Resultado2 != '') {
        $('#listado2').html(aData.Resultado2);

        $('#tabla2').dataTable({
            bJQueryUI: true,
            "scrollY": $('#listado2').height() > 200 ? 200 + "px" : "100%",
            "scrollX": false,
            "sDom": 'ft',
            "ordering": true,
            "iDisplayLength": -1
        });
    }
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};