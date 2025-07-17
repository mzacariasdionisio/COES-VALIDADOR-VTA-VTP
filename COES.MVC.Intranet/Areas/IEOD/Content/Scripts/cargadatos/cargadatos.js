var controlador = siteRoot + 'ieod/cargadatos/';
var uploader;
var errores = [];

const INFO_MED48 = 0;
const INFO_SCADA = 1;
let tipoInfoMostrada = INFO_MED48;

var LISTA_COLOR_PTO = [];
var LISTA_INFO_CELDA = [];

$(function () {

    $('#tab-container').easytabs({
        animate: false
    });

    $('#txtFechaConsulta').Zebra_DatePicker({
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $("#btnManualUsuario").click(function () {
        window.location = controlador + 'DescargarManualUsuario';
    });

    $('#btnDescargarFormato').on('click', function () {
        descargarFormato();
    });

    $('#btnConsultarScada').on('click', function () {
        consultarScada();
    });

    $('#btnSubirFormato').on('click', function () {
        subirFormato();
    });

    $('#btnEnviarDatos').on('click', function () {
        enviarDatos();
    });

    $('#btnMostrarErrores').on('click', function () {
        mostrarErrores();
    });

    $('#btnBusqueda').on('click', function () {
        mostrarReporte();
    });


    $('#txtFechaInicio').Zebra_DatePicker({
        onSelect: function (date) {
            $('#txtFechaFin').val($('#txtFechaInicio').val());
        }
    });

    $('#txtFechaFin').Zebra_DatePicker({

    });

    $('#btnExportarConsulta').on('click', function () {
        exportarConsulta();
    });

    $('#btnConfigurarReporte').on('click', function () {
        abrirVentanaConfigReporte();
    });

    habilitarEnvio(false);
    crearPupload();
});

function abrirVentanaConfigReporte() {
    var idReporte = $('#hfIdReporte').val();
    var url = siteRoot + 'ReportesMedicion/formatoreporte/IndexDetalle?id=' + idReporte;
    window.open(url, '_blank');
}

//Pestaña Carga de datos
function consultar() {

    limpiarMensaje('mensaje');
    if ($('#cbFormato').val() != "") {
        if ($('#txtFechaConsulta').val() != "") {
            $.ajax({
                type: 'POST',
                url: controlador + 'consultarBD',
                data: {
                    idReporte: $('#hfIdReporte').val(),
                    tipoinfocodi: $('#cbFormato').val(),
                    fecha: $('#txtFechaConsulta').val(),
                    tipo: $('#hfTipoCarga').val()
                },
                dataType: 'json',
                success: function (result) {
                    if (result.Result == 1) {
                        let hayDatos = result.ExisteDatos;

                        if (hayDatos == 0) mostrarMensaje('mensaje', 'alert', 'No hay información guardada en Base de Datos.');
                        if (hayDatos == 1) mostrarMensaje('mensaje', 'message', 'Se muestra los datos guardados en Base de Datos.');

                        tipoInfoMostrada = INFO_MED48;

                        LISTA_INFO_CELDA = result.ListaInformacionCelda;
                        LISTA_COLOR_PTO = result.ListaColorColumna;

                        cargarGrillaConsulta(result);
                        habilitarEnvio(true);
                        errores = [];
                    }
                    else {
                        mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            });
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Debe seleccionar fecha de consulta.');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione formato.');
    }
}

function consultarScada() {
    limpiarMensaje('mensaje');
    if ($('#cbFormato').val() != "") {
        if ($('#txtFechaConsulta').val() != "") {
            $.ajax({
                type: 'POST',
                url: controlador + 'consultarScada',
                data: {
                    idReporte: $('#hfIdReporte').val(),
                    tipoinfocodi: $('#cbFormato').val(),
                    fecha: $('#txtFechaConsulta').val(),
                    tipo: $('#hfTipoCarga').val()
                },
                dataType: 'json',
                success: function (result) {
                    if (result.Result == 1) {
                        let hayDatos = result.ExisteDatos;

                        if (hayDatos == 0) mostrarMensaje('mensaje', 'alert', 'No hay información guardada en el Scada.');
                        if (hayDatos == 1) mostrarMensaje('mensaje', 'message', 'Se muestran datos del Scada');

                        tipoInfoMostrada = INFO_SCADA;

                        LISTA_INFO_CELDA = result.ListaInformacionCelda;
                        LISTA_COLOR_PTO = result.ListaColorColumna;

                        cargarGrillaConsulta(result);
                        habilitarEnvio(true);
                        errores = [];
                    }
                    else {
                        mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                    }
                },
                error: function () {
                    mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
                }
            });
        }
        else {
            mostrarMensaje('mensaje', 'alert', 'Debe seleccionar fecha de consulta.');
        }
    }
    else {
        mostrarMensaje('mensaje', 'alert', 'Seleccione formato.');
    }
}

function habilitarEnvio(flag) {

    if (!flag) {
        $('#divAcciones').hide();
        $('#ctnBusqueda').hide();
        $('#detalleFormato').html('');
    }
    else {
        $('#divAcciones').show();
        $('#ctnBusqueda').show();
    }
}

function descargarFormato() {
    limpiarMensaje('mensaje');
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarFormato",
        data: {
            idReporte: $('#hfIdReporte').val(),
            tipoinfocodi: $('#cbFormato').val(),
            fecha: $('#txtFechaConsulta').val(),
            tipo: $('#hfTipoCarga').val(),
            tipoDato: tipoInfoMostrada
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                let msg = "";
                if (tipoInfoMostrada == INFO_MED48) msg = "Se ha generado el formato con la información guardada en Base de Datos.";
                if (tipoInfoMostrada == INFO_SCADA) msg = "Se ha generado el formato con la información del Scada.";

                mostrarMensaje('mensaje', 'message', msg);
                location.href = controlador + "DescargarFormato?tipo=" + $('#hfTipoCarga').val();
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function subirFormato() {

}

function enviarDatos() {
    grabarDatos();
}

function mostrarErrores() {
    if (errores.length > 0) {
        var html = obtenerErrores(errores);
        $('#contenidoError').html(html);
        $('#popupErrores').bPopup({});
    }
}

function grabarDatos() {
    $.ajax({
        type: 'POST',
        url: controlador + 'GrabarDatos',
        contentType: 'application/json',
        data: JSON.stringify({
            idReporte: $('#hfIdReporte').val(),
            data: getData(),
            listaInfoCelda: LISTA_INFO_CELDA,
            tipoinfocodi: $('#cbFormato').val(),
            fecha: $('#txtFechaConsulta').val(),
            tipo: $('#hfTipoCarga').val()
        }),
        dataType: 'json',
        success: function (result) {
            if (result == 1) {
                consultar();
                mostrarMensaje('mensaje', 'exito', 'Los datos fueron grabados correctamente.');
            }
            else
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function crearPupload() {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnSubirFormato',
        url: controlador + 'Upload',
        flash_swf_url: 'Scripts/Moxie.swf',
        silverlight_xap_url: 'Scripts/Moxie.xap',
        multi_selection: false,
        filters: {
            max_file_size: '5mb',
            mime_types: [
                { title: "Archivos Excel .xlsx", extensions: "xlsx" }
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
                mostrarMensaje('mensaje', 'alert', "Por favor espere ...(<strong>" + file.percent + "%</strong>)");
            },
            UploadComplete: function (up, file) {
                mostrarMensaje('mensaje', 'alert', "Subida completada. <strong>Por favor espere...</strong>");
                procesarArchivo();

            },
            Error: function (up, err) {
                mostrarMensaje('mensaje', 'error', err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

function procesarArchivo() {
    errores = [];
    $.ajax({
        type: 'POST',
        url: controlador + 'Importar',
        dataType: 'json',
        data: {
            idReporte: $('#hfIdReporte').val(),
            tipoinfocodi: $('#cbFormato').val(),
            fecha: $('#txtFechaConsulta').val(),
            tipo: $('#hfTipoCarga').val()
        },
        success: function (result) {

            if (result.Result == 1) {
                mostrarMensaje('mensaje', 'exito', 'Los datos se muestran correctamente en el Excel web, presione el botón <strong>Enviar</strong> para guardar en la Base de Datos.');

                LISTA_INFO_CELDA = result.Data.ListaInformacionCelda;

                cargarGrillaConsulta(result.Data);
            }
            else if (result.Result == 2) {
                var html = "No se realizó la carga por que se encontraron los siguientes errores: <ul>";
                for (var i in result.Errores) {
                    html = html + "<li>" + result.Errores[i] + "</li>";
                }
                html = html + "</ul>";
                mostrarMensaje('mensaje', 'alert', html);
            }
            else if (result.Result == 3) {
                var html = obtenerErrores(result.Errores);
                errores = result.Errores;
                $('#contenidoError').html(html);
                $('#popupErrores').bPopup({});
                mostrarMensaje('mensaje', 'alert', "No se importaron los datos, dado que encontraron errores de validación de datos.");
            }
            else if (result.Result == -1) {
                mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

//Pestaña Consulta
function mostrarReporte() {
    limpiarMensaje('mensajeConsulta');
    if ($('#cbFormatoConsulta').val() != "") {
        if ($('#txtFechaInicio').val() != "" && $('#txtFechaFin').val() != "") {

            if (getFecha($('#txtFechaInicio').val()) <= getFecha($('#txtFechaFin').val())) {

                $.ajax({
                    type: 'POST',
                    url: controlador + 'ObtenerConsulta',
                    data: {
                        idReporte: $('#hfIdReporte').val(),
                        dia: $('#cbDiaConsulta').val(),
                        tipoinfocodi: $('#cbFormatoConsulta').val(),
                        fechaInicio: $('#txtFechaInicio').val(),
                        fechaFin: $('#txtFechaFin').val(),
                        tipo: $('#hfTipoCarga').val()
                    },
                    dataType: 'json',
                    success: function (result) {
                        if (result.Result == 1) {
                            LISTA_COLOR_PTO = result.ListaColorColumna;
                            cargarGrillaReporte(result);
                        }
                        else {
                            mostrarMensaje('mensajeConsulta', 'error', 'Se ha producido un error.');
                        }
                    },
                    error: function () {
                        mostrarMensaje('mensajeConsulta', 'error', 'Se ha producido un error.');
                    }
                });
            }
            else {
                mostrarMensaje('mensajeConsulta', 'alert', 'Fecha final debe ser mayor a la inicial.');
            }
        }
        else {
            mostrarMensaje('mensajeConsulta', 'alert', 'Debe seleccionar fecha de consulta.');
        }
    }
    else {
        mostrarMensaje('mensajeConsulta', 'alert', 'Seleccione formato.');
    }
}

function exportarConsulta() {
    $.ajax({
        type: 'POST',
        url: controlador + "ExportarConsulta",
        data: {
            idReporte: $('#hfIdReporte').val(),
            dia: $('#cbDiaConsulta').val(),
            tipoinfocodi: $('#cbFormatoConsulta').val(),
            fechaInicio: $('#txtFechaInicio').val(),
            fechaFin: $('#txtFechaFin').val(),
            tipo: $('#hfTipoCarga').val()
        },
        dataType: 'json',
        cache: false,
        success: function (resultado) {
            if (resultado == 1) {
                location.href = controlador + "DescargarConsulta?tipo=" + $('#hfTipoCarga').val();
            }
            else {
                mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
            }
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

//util
function getFecha(date) {
    var parts = date.split("/");
    var date1 = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
    return date1.getTime();
}

function limpiarMensaje(id) {
    $('#' + id).removeClass();
    $('#' + id).html('');
}