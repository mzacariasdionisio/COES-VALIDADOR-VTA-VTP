var controlador = siteRoot + 'PrimasRER/cuadro/';
var ancho = 1000;

$(function () {
    $('#cntMenu').css("display", "none");

    $('#fecha_inicio').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "date"
    });

    $('#fecha_inicio').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {

        }
    });

    $('#hora_inicio').timepicker({
        timeFormat: 'HH:mm',
        interval: 15,
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });

    $('#fecha_fin').inputmask({
        mask: "1/2/y",
        placeholder: "dd/mm/yyyy",
        alias: "date"
    });

    $('#fecha_fin').Zebra_DatePicker({
        readonly_element: false,
        onSelect: function (date) {

        }
    });

    $('#hora_fin').timepicker({
        timeFormat: 'HH:mm',
        interval: 15,
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });

    $('#cbSolicitud').change(function () {
        cargarUnidadesGeneracion();
    });

    $("#btnConsultar").click(function () {
        consultar();
    });

    $("#btnGuardar").click(function () {
        guardar();
    });

    $("#btnDescargar").click(function () {
        descargar();
    });

    $("#btnCalcular").click(function () {
        calcular();
    });

    uploadExcel();
});

function cargarUnidadesGeneracion() {
    var reresecodi = parseInt($("#cbSolicitud").val()) || 0;
    var nombre_solicitud_central = $("#cbSolicitud option:selected").text();
    var rerevacodi = parseInt($("#rerevacodi").val()) || 0;
    if (reresecodi < 1) {
        alert("Debe seleccionar una solicitud - central.");
        return;
    }
    if (rerevacodi < 1) {
        alert("Una versión es requerida.");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "CargarUnidadesGeneracion",
        data: {
            reresecodi: reresecodi,
            rerevacodi: rerevacodi,
        },
        success: function (evt) {
            if (evt.Resultado == "1") { 
                $("#comparativo_texto").attr('style', 'font-size: 12px; display: none;');
                $("#comparativo_boton").attr('style', 'display: inline-block; display: none;');
                $("#comparativo_tr1").attr('style', 'display: none;');
                $("#comparativo_tr2").attr('style', 'display: none;');
                $("#comparativo_tr3").attr('style', 'display: none;');
                $("#comparativo_tr4").attr('style', 'display: none;');
                $("#comparativo_tr5").attr('style', 'display: none;');
                $("#comparativo_tr6").attr('style', 'display: none;');
                $("#comparativo_tr7").attr('style', 'display: none;');

                $("#reresecodi").val(reresecodi);
                $("#rereeucodi").val(0);
                $("#rerccboridatos").val("");
                $("#nombre_solicitud_central").val(nombre_solicitud_central);
                $("#label_nombre_solicitud_central").text("Solicitud - Central: " + nombre_solicitud_central);
                $("#nombre_unidad_generadora").val("");
                $("#label_nombre_unidad_generadora").text("");
                $("#fecha_inicio").val("");
                $("#hora_inicio").val("");
                $("#fecha_fin").val("");
                $("#hora_fin").val("");
                $("#energia_estimada_central").val(evt.EnergiaEstimadaCentral); 
                $("#energia_estimada_15_min").val(evt.EnergiaEstimada15Min);
                $("#energia_estimada_unidad").val(evt.EnergiaEstimadaUnidad);
                $("#energia_solicitada_unidad").val(evt.EnergiaSolicitadaUnidad);
                $('#listado').html("");
                $('#idGrafico1').html("");

                $('#cbUnidadGeneracion').empty();
                if (evt.ListaEvaluacionEnergiaUnidad.length > 0) {
                    $('#cbUnidadGeneracion').get(0).options[0] = new Option("", "0");
                    $.each(evt.ListaEvaluacionEnergiaUnidad, function (i, item) {
                        $('#cbUnidadGeneracion').get(0).options[$('#cbUnidadGeneracion').get(0).options.length] = new Option(item.Equinomb, item.Rereeucodi);
                    });
                }
            }
            else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function consultar() {
    var rerevacodi = parseInt($("#rerevacodi").val()) || 0;
    var reresecodi = parseInt($("#reresecodi").val()) || 0;
    var rereeucodi = parseInt($("#cbUnidadGeneracion").val()) || 0;
    var nombre_unidad_generadora = $("#cbUnidadGeneracion option:selected").text();

    if (rerevacodi < 1) {
        alert("Una versión es requerida.");
        return;
    }
    if (reresecodi < 1) {
        alert("Debe seleccionar una solicitud - central.");
        return;
    }
    if (rereeucodi < 1) {
        alert("Debe seleccionar una unidad de generación.");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ConsultarEvaluacionEnergiaUnidad",
        data: {
            rerevacodi: rerevacodi,
            reresecodi: reresecodi,
            rereeucodi: rereeucodi,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $("#comparativo_texto").attr('style', 'font-size: 12px;');
                $("#comparativo_boton").attr('style', 'display: inline-block;');
                $("#comparativo_tr1").removeAttr('style');
                $("#comparativo_tr2").removeAttr('style');
                $("#comparativo_tr3").removeAttr('style');
                $("#comparativo_tr4").removeAttr('style');
                $("#comparativo_tr5").removeAttr('style');
                $("#comparativo_tr6").removeAttr('style');
                $("#comparativo_tr7").removeAttr('style');

                $("#rereeucodi").val(rereeucodi);
                $("#nombre_unidad_generadora").val(nombre_unidad_generadora);
                $("#label_nombre_unidad_generadora").text("Unidad de Generación: " + nombre_unidad_generadora);
                $("#rerccbcodi").val(evt.IdComparativoCab);
                $("#rerccboridatos").val("");
                $("#fecha_inicio").val("");
                $("#hora_inicio").val("");
                $("#fecha_fin").val("");
                $("#hora_fin").val("");
                $("#energia_estimada_central").val(evt.EnergiaEstimadaCentral);
                $("#energia_estimada_15_min").val(evt.EnergiaEstimada15Min);
                $("#energia_estimada_unidad").val(evt.EnergiaEstimadaUnidad);
                $("#energia_solicitada_unidad").val(evt.EnergiaSolicitadaUnidad);
                $('#listado').html(evt.Resultado);
                refreshDatatable();
                GraficoLinea(evt.Grafico, 'idGrafico1');
            }
            else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function descargar() {
    var rerevacodi = parseInt($("#rerevacodi").val()) || 0;
    var reresecodi = parseInt($("#reresecodi").val()) || 0;
    var rereeucodi = parseInt($("#rereeucodi").val()) || 0;

    if (rerevacodi < 1) {
        alert("Una versión es requerida.");
        return;
    }
    if (reresecodi < 1) {
        alert("Debe seleccionar una solicitud - central.");
        return;
    }
    if (rereeucodi < 1) {
        alert("Debe seleccionar y consultar una unidad de generación.");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ExportaraExcelTablaEvaluacionEnergiaUnidad",
        data: {
            rerevacodi: rerevacodi,
            reresecodi: reresecodi,
            rereeucodi: rereeucodi,
        },
        success: function (evt) {
            if (evt.Resultado == "-1") {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
            else {
                window.location = controlador + 'abrirarchivo?tipo=' + 1 + '&nombreArchivo=' + evt.Resultado;
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

mostrarExito = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-exito");
};
mostrarError = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-error");
};
mostrarAlerta = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass("action-alert");
};
mostrarMensaje = function (msg) {
    $('#mensaje').removeClass();
    $('#mensaje').html(msg);
    $('#mensaje').addClass('action-message');
}

/* Paso 2: Procedimiento para la lectura de un archivos */
uploadExcel = function () {
    uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: 'btnImportar',                                   /* SE ASIGNA EL BTN QUE EJECUTARA EL EVENTO */
        url: controlador + 'ImportarArchivoExcelParaTablaEvaluacionEnergiaUnidad',   /* Función en el controlador que ejecutara el evento  */
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
                if (uploader.files.length == 2) {
                    uploader.removeFile(uploader.files[0]);
                }
                uploader.start();
                up.refresh();
            },
            UploadProgress: function (up, file) {
                mostrarAlerta("Por favor espere ...(" + file.percent + "%)");
            },
            UploadComplete: function (up, file) {
                mostrarAlerta("Subida completada, procesando el archivo...");
                procesarArchivo(file[0].name);
            },
            Error: function (up, err) {
                mostrarError(err.code + "-" + err.message);
            }
        }
    });
    uploader.init();
}

procesarArchivo = function (nombreArchivo) {
    var rerevacodi = parseInt($("#rerevacodi").val()) || 0;
    var reresecodi = parseInt($("#reresecodi").val()) || 0;
    var rereeucodi = parseInt($("#rereeucodi").val()) || 0;
    var rerccbcodi = parseInt($("#rerccbcodi").val()) || 0;

    if (rerevacodi < 1) {
        alert("Una versión es requerida.");
        return;
    }
    if (reresecodi < 1) {
        alert("Debe seleccionar una solicitud - central.");
        return;
    }
    if (rereeucodi < 1) {
        alert("Debe seleccionar y consultar una unidad de generación.");
        return;
    }
    if (rerccbcodi < 0) { //rerccbcodi = 0: No existe en BD. rerccbcodi > 0: Existe en BD.
        alert("Un comparativo de cabecera es requerido.");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + 'ProcesarArchivoExcelParaTablaEvaluacionEnergiaUnidad',
        data: {
            rerevacodi: rerevacodi,
            reresecodi: reresecodi,
            rereeucodi: rereeucodi,
            rerccbcodi: rerccbcodi, 
            nombreArchivo: nombreArchivo,
        },
        dataType: 'json',
        cache: false,
        success: function (evt) {
            if (evt.Resultado == "-1") {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
            else {
                if (evt.RegError > 0) {
                    var mensajeError = "Lo sentimos, " + evt.RegError + " registro(s) no ha(n) sido leido(s) por presentar los siguientes errores: " + evt.MensajeError;
                    mostrarError(mensajeError);
                    alert(mensajeError.replaceAll('<br>', ''));
                }
                else {
                    $("#rerccboridatos").val("AE");
                    $("#energia_estimada_central").val(evt.EnergiaEstimadaCentral);
                    $("#energia_estimada_15_min").val(evt.EnergiaEstimada15Min);
                    $("#energia_estimada_unidad").val(evt.EnergiaEstimadaUnidad);
                    $("#energia_solicitada_unidad").val(evt.EnergiaSolicitadaUnidad);
                    $('#listado').html(evt.Resultado);
                    refreshDatatable();
                    GraficoLinea(evt.Grafico, 'idGrafico1');
                    var mensajeExitoso = "Se leyeron los datos de la columna 'Energía Estimada' del archivo Excel y se cargaron los datos sobre la tabla de la unidad generadora";
                    mostrarExito(mensajeExitoso);
                    alert(mensajeExitoso);
                }
            }
        },
        error: function () {
            mostrarError('Lo sentimos no se puede mostrar la hoja de cálculo')
        }
    });
}

function calcular() {
    var rerevacodi = parseInt($("#rerevacodi").val()) || 0;
    var reresecodi = parseInt($("#reresecodi").val()) || 0;
    var rereeucodi = parseInt($("#rereeucodi").val()) || 0;
    var rerccbcodi = parseInt($("#rerccbcodi").val()) || 0;

    var fecha_inicio = $("#fecha_inicio").val();
    var hora_inicio = $("#hora_inicio").val();
    var fecha_fin = $("#fecha_fin").val();
    var hora_fin = $("#hora_fin").val();

    if (rerevacodi < 1) {
        alert("Una versión es requerida.");
        return;
    }
    if (reresecodi < 1) {
        alert("Debe seleccionar una solicitud - central.");
        return;
    }
    if (rereeucodi < 1) {
        alert("Debe seleccionar y consultar una unidad de generación.");
        return;
    }
    if (rerccbcodi < 0) { //rerccbcodi = 0: No existe en BD. rerccbcodi > 0: Existe en BD.
        alert("Un comparativo de cabecera es requerido.");
        return;
    }

    if (fecha_inicio == null || fecha_inicio.trim() == "") {
        alert("Una fecha de inicio es requerida.");
        return;
    }
    if (hora_inicio == null || hora_inicio.trim() == "") {
        alert("Una hora de inicio es requerida.");
        return;
    }
    if (fecha_fin == null || fecha_fin.trim() == "") {
        alert("Una fecha fin es requerida.");
        return;
    }
    if (hora_fin == null || hora_fin.trim() == "") {
        alert("Una hora fin es requerida.");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "ProcesarValorTipicoParaTablaEvaluacionEnergiaUnidad",
        data: {
            rerevacodi: rerevacodi,
            reresecodi: reresecodi,
            rereeucodi: rereeucodi,
            rerccbcodi: rerccbcodi,
            fecha_inicio: fecha_inicio,
            hora_inicio: hora_inicio,
            fecha_fin: fecha_fin,
            hora_fin: hora_fin,
        },
        success: function (evt) {
            if (evt.Resultado == "-1") {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
            else {
                $("#rerccboridatos").val("VT");
                $("#energia_estimada_central").val(evt.EnergiaEstimadaCentral);
                $("#energia_estimada_15_min").val(evt.EnergiaEstimada15Min);
                $("#energia_estimada_unidad").val(evt.EnergiaEstimadaUnidad);
                $("#energia_solicitada_unidad").val(evt.EnergiaSolicitadaUnidad);
                $('#listado').html(evt.Resultado);
                refreshDatatable();
                GraficoLinea(evt.Grafico, 'idGrafico1');
                var mensajeExitoso = "Se cargaron los datos sobre la tabla de la unidad generadora";
                alert(mensajeExitoso);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function guardar() {

    var answer = confirm("¿Está seguro que desea guardar los datos?");
    if (!answer)
    {
        return;
    }

    var energia_estimada_central = $("#energia_estimada_central").val();
    var energia_estimada_unidad = $("#energia_estimada_unidad").val();
    var energia_solicitada_unidad = $("#energia_solicitada_unidad").val();

    if (energia_estimada_central == null || energia_estimada_central.trim() == "")
    {
        alert("La energía estimada de la central es requerida.");
        return;
    }
    if (energia_estimada_unidad == null || energia_estimada_unidad.trim() == "") {
        alert("La energía estimada de la unidad es requerida.");
        return;
    }
    if (energia_solicitada_unidad == null || energia_solicitada_unidad.trim() == "") {
        alert("La energía solicitada de la unidad es requerida.");
        return;
    }

    var rerevacodi = parseInt($("#rerevacodi").val()) || 0;
    var reresecodi = parseInt($("#reresecodi").val()) || 0;
    var rereeucodi = parseInt($("#rereeucodi").val()) || 0;
    var rerccbcodi = parseInt($("#rerccbcodi").val()) || 0;
    var reresetotenergiaestimada = parseFloat(energia_estimada_central) || 0;
    var rerccbtoteneestimada = parseFloat(energia_estimada_unidad) || 0;
    var rerccbtotenesolicitada = parseFloat(energia_solicitada_unidad) || 0;
    var dt = $('#tabla_evaluacion_energia_unidad').DataTable().rows().data();
    var datosTabla = GetDataDataTable(dt);
    var rerccboridatos = $("#rerccboridatos").val();

    if (rerevacodi < 1) {
        alert("Una versión es requerida.");
        return;
    }
    if (reresecodi < 1) {
        alert("Debe seleccionar una solicitud - central.");
        return;
    }
    if (rereeucodi < 1) {
        alert("Debe seleccionar y consultar una unidad de generación.");
        return;
    }
    if (rerccbcodi < 0) { //rerccbcodi = 0: Nuevo registro. rerccbcodi > 0: Registro existente.
        alert("Un id de comparativo es requerido.");
        return;
    }
    if (datosTabla == null || datosTabla.length < 1) {
        alert("No existen registros para guardar.");
        return;
    }
    if (rerccboridatos == null || rerccboridatos.trim() == "")
    {
        alert("Primero, debe realizar una carga de datos. Para ello, debe ejecutar el botón 'Importar' o el botón 'Calcular'.");
        return;
    }

    $.ajax({
        type: 'POST',
        url: controlador + "GuardarComparativo",
        dataType: 'json',
        contentType: 'application/json',
        traditional: true,
        data: JSON.stringify({
            rerevacodi: rerevacodi,
            reresecodi: reresecodi,
            rereeucodi: rereeucodi,
            rerccbcodi: rerccbcodi,
            reresetotenergiaestimada: reresetotenergiaestimada,
            rerccbtoteneestimada: rerccbtoteneestimada,
            rerccbtotenesolicitada: rerccbtotenesolicitada,
            datosTabla: datosTabla,
            rerccboridatos: rerccboridatos,
        }),
        success: function (evt) {
            if (evt.Resultado == "1") {
                $("#rerccboridatos").val("");
                $("#rerccbcodi").val(evt.IdComparativoCab);
                alert("Se guardaron los datos satisfactoriamente");
            }
            else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

function refreshDatatable() {
    var altotabla = parseInt($('#listado').height()) || 0;
    $('#tabla_evaluacion_energia_unidad').dataTable({
        "sPaginationType": "full_numbers",
        "destroy": "true",
        "ordering": false,
        "searching": false,
        "iDisplayLength": 15,
        "info": false,
        "paging": false,
        "scrollX": true,
        "scrollY": altotabla > 355 || altotabla == 0 ? 355 + "px" : "100%"
    });
}

/**
 * Permite obtener Object Array de un DataTable
 * @param {any} data DataTable rows data
 * @returns {any} array
 */
function GetDataDataTable(data) {
    var dataList = [];

    $.each(data, function (index, value) {
        dataList.push(value);
    });

    return dataList;
}
