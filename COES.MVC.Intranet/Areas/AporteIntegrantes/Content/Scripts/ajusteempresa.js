var sControlador = siteRoot + "AporteIntegrantes/ajusteempresa/";

//Funciones de busqueda
$(document).ready(function () {

    $('#respuestaImport').hide();
    $('#respuestaImport').removeClass('mensajes');
    var caiajcodi = document.getElementById('EntidadAjuste_Caiajcodi').value;
    if (caiajcodi) {
        buscar();
        uploadExcelAE();
    }

    $('.txtFecha').Zebra_DatePicker({
    });

    $('#btnNuevo').click(function () {
        nuevo();
    });

    $('.txtFecha').Zebra_DatePicker({

    });

    $('#btnRefrescar').click(function () {
        refrescar();
    });

    $('#btnBuscar').click(function () {
        buscar();
    });

    $('#btnDescargarExcelAE').click(function () {
        mostrarAlerta("Espere un momento, se esta procesando su solicitud");
        descargarArchivoAE(1);
    });

});

buscar = function () {
    var caiajcodi = document.getElementById("EntidadAjuste_Caiajcodi").value;
    var caiajetipoinfo = document.getElementById("Caiajetipoinfo").value;
    console.log(caiajetipoinfo);
    $.ajax({
        type: 'POST',
        url: sControlador + "Lista",
        data: { caiajcodi: caiajcodi, caiajetipoinfo: caiajetipoinfo },
        success: function (evt) {
            $('#listado').html(evt);
            addDeleteEvent();
            viewEvent();
            oTable = $('#tabla').dataTable({
                "sPaginationType": "full_numbers",
                "destroy": "true",
                "aaSorting": [[0, "asc"], [1, "asc"]]
            });
        },
        error: function () {
            alert("Lo sentimos, se ha producido un error");
        }
    });
};

//Funciones de eliminado de registro
addDeleteEvent = function () {
    $(".delete").click(function (e) {
        e.preventDefault();
        if (confirm("¿Desea eliminar la información seleccionada?")) {
            id = $(this).attr("id").split("_")[1];
            $.ajax({
                type: "post",
                dataType: "text",
                url: sControlador + "Delete/" + id,
                data: addAntiForgeryToken({ id: id }),
                success: function (resultado) {
                    if (resultado == "true")
                        $("#fila_" + id).remove();
                    else
                        alert("No se ha logrado eliminar el registro");
                }
            });
        }
    });
};

addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    return data;
};

//Funciones de vista detalle
viewEvent = function () {
    $('.view').click(function () {
        id = $(this).attr("id").split("_")[1];
        abrirPopup(id);
    });
};

abrirPopup = function (id) {
    $.ajax({
        type: 'POST',
        url: sControlador + "View/" + id,
        success: function (evt) {
            $('#popup').html(evt);

            setTimeout(function () {
                $('#popup').bPopup({
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown'
                });
            }, 50);
        },
        error: function () {
            alert("Lo sentimos, ha ocurrido un error inesperado");
        }
    });
}

nuevo = function () {
    var caiajcodi = document.getElementById("EntidadAjuste_Caiajcodi").value;
    var caiajetipoinfo = document.getElementById("Caiajetipoinfo").value;
    window.location.href = sControlador + "new?caiajcodi=" + caiajcodi + "&caiajetipoinfo=" + caiajetipoinfo;
}

refrescar = function () {
    window.location.href = sControlador + "index";
}

descargarArchivoAE = function (formato) {
    $.ajax({
        type: 'POST',
        url: sControlador + "ExportarDataAE",
        data: { caiajcodi: $('#EntidadAjuste_Caiajcodi').val(), caiajetipoinfo: $('#Caiajetipoinfo').val(), formato: formato },
        dataType: 'json',
        success: function (result) {
            if (result != -1) {
                window.location = sControlador + "AbrirArchivo?formato=" + formato + "&file=" + result;
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

uploadExcelAE = function () {
    uploaderAE = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSelecionarExcelAE',
        url: sControlador + 'uploadexcel',
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
                if (uploaderAE.files.length == 2) {
                    uploaderAE.removeFile(uploaderDE.files[0]);
                }
                uploaderAE.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada <strong> procesando el archivo...</strong>");
                procesarArchivoAE(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploaderAE.init();
}

procesarArchivoAE = function (sFile) {
    var caiajcodi = document.getElementById('EntidadAjuste_Caiajcodi').value;
    var caiajetipoinfo = document.getElementById('Caiajetipoinfo').value;

    $.ajax({
        type: 'POST',
        url: sControlador + 'procesararchivoae',
        data: { caiajcodi: caiajcodi, caiajetipoinfo: caiajetipoinfo, sarchivo: sFile },
        dataType: 'json',
        cache: false,
        success: function (result) {
            if (result.sError == "") {
                $('#respuestaRegistro').hide();
                $('#respuestaRegistro').removeClass('mensajes');
                //para que actualize la lista con los datos importados
                buscar();
                $('#respuestaImport').html(result.sMensaje);
                $('#respuestaImport').addClass('mensajes');
                //$('#prueba').addClass('exito');
                $('#respuestaImport').show();
                setTimeout(function ()
                { $(".mensajes").fadeOut(800).fadeIn(800).fadeOut(500).fadeIn(500).fadeOut(300); }, 3000);
                //actualizo la pag porque si le doy nuevamente a importar el excel solo lee el ultimo import
                location.reload();


            } else {
                //$('#respuestaRegistro').hide();
                //$('#respuestaRegistro').removeClass('mensajes');
                //$('#respuestaImport').html(result.sError);
                //$('#respuestaImport').addClass('mensajes');
                //$('#respuestaImport').show();
                //setTimeout(function ()
                //{ $(".mensajes").fadeOut(800).fadeIn(800).fadeOut(500).fadeIn(500).fadeOut(300); }, 3000);
                alert(result.sError);
                //actualizo la pag porque si le doy nuevamente a importar el excel solo lee el ultimo import
                location.reload();

            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puedo grabar la información ingresada');
            //actualizo la pag porque si le doy nuevamente a importar el excel solo lee el ultimo import
            location.reload();
        }
    });
}