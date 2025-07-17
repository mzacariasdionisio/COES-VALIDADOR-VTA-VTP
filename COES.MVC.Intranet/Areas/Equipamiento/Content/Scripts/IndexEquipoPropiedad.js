var controlador = siteRoot + 'Equipamiento/EquipoPropiedad/';
$(function () {
    $('#cbTipoEmpresa').val(-2);
    $('#cbEmpresa').val(-2);
    $('#cbTipoEquipo').val(17);
    $('#cbEstado').val('A');
    cargarEmpresas();
    buscarEquipos();

    $('#btnBuscar').click(function () {
        buscarEquipos();
    });

    $('#cbTipoEmpresa').change(function () {
        cargarEmpresas();
        return false;
    });

    $('#btnExportar').click(function () {
        exportarPropiedades();
    });
    $('#btnExportarReporte').click(function () {

        setTimeout(function () {
            $("#popupExportarReporte").bPopup({
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                modalClose: false
            });
        }, 50);

        //exportarReporte();
    });

    $('#btnExportarReporte2').click(function () {
        var opcion = $("#tipoExportar").val();
        if (opcion > 0) {
            $('#exportacionTipo').val(opcion);
            exportarReporte(opcion);
        }
        else {
            alert("Debe seleccionar un tipo de exportación.");
        }
    });

    $('#btnCancelarExportacion').click(function () {
        $('#popupExportarReporte').bPopup().close();
    });

    $('#btnImportarReporte').click(function () {
        window.location.href = controlador + 'PropiedadesMasivoImportacion';
    });

    //$('#btnImportar').click(function () {
    //    importarPropiedades();
    //});

    $(window).resize(function () {
        $('#listado').css("width", $('#mainLayout').width() + "px");
    });

    crearPupload();
});
mostrarExitoOperacion = function () {
    $('#mensaje').removeClass("action-error");
    $('#mensaje').addClass("action-exito");
    $('#mensaje').text("La operación se realizó con exito...!");
    $('#mensaje').css("display", "block");
};

mostrarError = function () {
    $('#mensaje').removeClass("action-exito");
    $('#mensaje').addClass("action-error");
    $('#mensaje').text("Ha ocurrido un error");
    $('#mensaje').css("display", "block");
};
cargarEmpresas = function () {
    $.ajax({
        type: 'GET',
        url: controlador + '/CargarEmpresas',
        dataType: 'json',
        data: { idTipoEmpresa: $('#cbTipoEmpresa').val() },
        cache: false,
        success: function (aData) {
            $('#cbEmpresa').get(0).options.length = 0;
            $('#cbEmpresa').get(0).options[0] = new Option("TODOS", "-2");
            $.each(aData, function (i, item) {
                $('#cbEmpresa').get(0).options[$('#cbEmpresa').get(0).options.length] = new Option(item.Text, item.Value);
            });
        },
        error: function () {
            mostrarError();
        }
    });
};
buscarEquipos = function () {
    $('#mensaje').css("display", "none");
    pintarPaginado();
    mostrarListado(1);
};
pintarPaginado = function () {
    $.ajax({
        type: 'POST',
        url: controlador + "PaginadoEquipos",
        data: $('#frmBusquedaEquipo').serialize(),
        success: function (evt) {
            $('#paginado').html(evt);
            mostrarPaginado();
        },
        error: function () {
            mostrarError();
        }
    });
};
mostrarListado = function (nroPagina) {
    $('#hfNroPagina').val(nroPagina);
    $.ajax({
        type: 'POST',
        url: controlador + "ListadoEquipos",
        data: $('#frmBusquedaEquipo').serialize(),
        success: function (evt) {
            $('#listado').css("width", $('#mainLayout').width() + "px");

            $('#listado').html(evt);
            $('#tabla').dataTable({
                "scrollY": 430,
                "scrollX": true,
                "sDom": 't',
                "ordering": false,
                "iDisplayLength": 50
            });
        },
        error: function () {
            mostrarError();
        }
    });
};
pintarBusqueda = function (nroPagina) {
    mostrarListado(nroPagina);
};
ocultarMensaje = function () {
    $('#mensaje').css("display", "none");
};


exportarReporte = function (opcion) {
    $('#mensaje').css("display", "none");
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarReporte",
        data: $('#frmBusquedaEquipo').serialize(),
        //data: {
        //    model: $('#frmBusquedaEquipo').serialize(),
        //    opcion: opcion
        //},
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado.Resultado != "-1") {
                $('#popupExportarReporte').bPopup().close();
                window.location = controlador + "AbrirArchivo?file=" + resultado.NombreArchivo;
                //location.href = controlador + "DescargarReporte";
            } else {
                $('#popupExportarReporte').bPopup().close();
                mostrarError();
            }
        },
        error: function () {
            $('#popupExportarReporte').bPopup().close();
            mostrarError();
        }
    });
};


exportarPropiedades = function () {
    $('#mensaje').css("display", "none");
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarPropiedades",
        data: $('#frmBusquedaEquipo').serialize(),
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarPlantilla";
            } else {
                mostrarError();
            }
        },
        error: function () {
            mostrarError();
        }
    });
};
function crearPupload() {
    ocultarMensaje();
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnImportar',
        url: siteRoot + 'Equipamiento/EquipoPropiedad/Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {

            max_file_size: '10mb',
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

                //if ($("#hfIdEnvio").val() == 0) {
                //    mostrarAlerta("Por favor muestre la información del periodo actual.");
                //    return false;
                //} else {
                mostrarAlerta("Subida completada <strong>Por favor espere</strong>");
                //}


                var retorno = leerFileUpExcel();
                if (retorno == 1) {
                    limpiarError();
                    mostrarExitoOperacion();

                }
                else {
                    mostrarError("Error: Formato desconocido.");
                }

            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function leerFileUpExcel() {
    var retorno = 0;
    $.ajax({
        type: 'POST',
        url: controlador + 'ImportarDataPropiedades',
        dataType: 'json',
        async: false,
        data: $('#frmBusquedaEquipo').serialize(),
        success: function (res) {
            retorno = res;
            mostrarExitoOperacion();
        },
        error: function () {
            mostrarError("Ha ocurrido un error");
        }
    });
    return retorno;
}

function mostrarAlerta(mensaje) {
    $('#mensaje').removeClass();
    $('#mensaje').html(mensaje);
    $('#mensaje').addClass('action-alert');
}