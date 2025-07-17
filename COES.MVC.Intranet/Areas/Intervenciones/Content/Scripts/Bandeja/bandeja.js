var controlador = siteRoot + "intervenciones/registro/";
var controladorBandeja = siteRoot + "intervenciones/bandeja/";
var controladorRegistro = siteRoot + "intervenciones/registro/";
var EVENCLASECODI_ANUAL = 5;
var ARREGLO_INTERVENCION = [];

$(function () {

    $('#txtInicio').Zebra_DatePicker({
        pair: $('#txtFin')
    });

    $('#txtFin').Zebra_DatePicker({
       direction:true
    });

    $('#cbTipoProgramacion').on('change', function () {
        programaciones(0);
    });

    $('#cbProgramacion').on('change', function () {
        obtenerFechaOperacion(0);
    });

    $('#btnConsultar').on('click', function () {
        consultar();
    });

    $('#btnNuevaComunicacion').on('click', function () {
        nuevaComunicacion();
    });

    $('#btnReportePDF').on('click', function () {
        reportePDF();
    });

    $('#btnDescargarAdjunto').on('click', function () {
        descargarZipArchivos();
    });

    $('#btnDescargarMsj').on('click', function () {
        descargarZipComunicacion();
    });

    programaciones(1);
});

function programaciones(accion) {
    var tipoProgramacion = parseInt($('#cbTipoProgramacion').val()) || 0;

    if (tipoProgramacion > 0) {
        $.ajax({
            type: 'POST',
            url: controladorBandeja + "ListarProgramaciones",
            datatype: "json",
            contentType: 'application/json',
            data: JSON.stringify({ idTipoProgramacion: tipoProgramacion }),
            success: function (evt) {
                $('#cbProgramacion').empty();
                evt.ProgramacionRegistro;
                var option = '<option value="0">----- Seleccione ----- </option>';
                if (EVENCLASECODI_ANUAL == tipoProgramacion) option = '<option value="0">----- Todos (más reciente) ----- </option>';
                $.each(evt.ListaProgramaciones, function (k, v) {
                    if (v.Progrcodi == evt.Entidad.Progrcodi) {
                        option += '<option value =' + v.Progrcodi + ' selected>' + v.ProgrnombYPlazoCruzado + '</option>';
                    } else {
                        option += '<option value =' + v.Progrcodi + '>' + v.ProgrnombYPlazoCruzado + '</option>';
                    }
                })
                $('#cbProgramacion').append(option);
                obtenerFechaOperacion(accion);                
            },
            error: function (err) {                
                mostrarMensaje('mensaje', 'error', 'Lo sentimos, se ha producido un error');
            }
        });
    } else {
        $("#cbProgramacion").empty();
        var option = '<option value="0">----- Seleccione un tipo de programación ----- </option>';
        $('#cbProgramacion').append(option);
    }
}

function obtenerFechaOperacion(accion) {
    var tipoProgramacion = parseInt($('#cbTipoProgramacion').val()) || 0;
    var programacion = parseInt($('#cbProgramacion').val()) || 0;

    if (tipoProgramacion == 0 || programacion == 0 || tipoProgramacion == "" || programacion == "") {
        return;
    }

    $.ajax({
        type: 'POST',
        url: controladorBandeja + "ObtenerFechaProgramacion",
        datatype: "json",
        contentType: 'application/json',
        data: JSON.stringify({ progCodi: programacion }),
        success: function (model) {
            $("#txtInicio").val(model.Progrfechaini);
            $("#txtFin").val(model.Progrfechafin);

            if (accion == 1) {
                consultar();
            }
        },
        error: function (err) {            
            mostrarMensaje('mensaje', 'error', 'Lo sentimos, se ha producido un error al obtener las fechas de operacion');
        }
    });
}

function consultar() {
    $("#mensaje").hide();

    $.ajax({
        type: 'POST',
        url: controladorBandeja + 'Mensajes',
        data: {
            tipoProgramacion: $('#cbTipoProgramacion').val(),
            indicadorFecha: 1,
            programa: $('#cbProgramacion').val(),
            fechaInicio: $('#txtInicio').val(),
            fechaFin: $('#txtFin').val()
        },
        success: function (evt) {
            ARREGLO_INTERVENCION = [];
            $('#listadoMensaje').html(evt);            
            $('.mensaje-item').on('click', function () {
                mostrarDetalle($(this).attr('data-id'), 0, $(this).attr('data-update'), $(this).attr('data-msg'));
            });

            $('.cb-intervencion').on('click', function () {
                if ($(this).is(':checked')) agregarIntervencion($(this).attr('data-id'), $(this).attr('data-update'), $(this).attr('data-msg'));
                else removerIntervencion($(this).attr('data-id'));
            });
            $('#detalleMensaje').html("");
            $('#detalleMensaje').css('display', 'block');
            $('#frmRegistro').attr('src', '');
            $('#contenedorAcciones').css('display', 'none');
            $('#btnNuevaComunicacion').css('display', 'none');
            $('#btnDescargarAdjunto').css('display', 'none');
            $('#btnReportePDF').css('display', 'none');
            $('#hfIntervencion').val("");
            
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function mostrarDetalle(intercodi, option, indUpdate, msgcodi) {
    $('.mensaje-content').removeClass("mensaje-select");
    $('#mensaje-content-' + intercodi).addClass("mensaje-select");
    $.ajax({
        type: 'POST',
        url: controladorBandeja + 'Detalle',
        data: {
            intercodi: intercodi,
            indUpdate: indUpdate,
            msgcodi: msgcodi
        },
        success: function (evt) {
            $('#detalleMensaje').html(evt);
            $('.detalle-header').on('click', function () {
                $('.detalle-mensaje-total').removeClass('detalle-open');
                $('#detalle-intervencion-' + $(this).attr('data-id')).addClass('detalle-open');
            });
            $('#contenedorAcciones').css('display', 'block');
            $('#btnNuevaComunicacion').css('display', 'block');
            $('#btnDescargarAdjunto').css('display', 'block');
            $('#btnReportePDF').css('display', 'block');
            $('#detalleMensaje').css('display', 'block');
            $('#nuevoMensaje').css('display', 'none');
            ARREGLO_INTERVENCION = [];
            $('.cb-intervencion').prop('checked', false);
            $('#hfIntervencion').val(intercodi);
            if (option == 1) {
                $('#cb-intervencio-' + intercodi).prop('checked', true);
                ARREGLO_INTERVENCION.push(intercodi);
            }
            if (indUpdate == 1) {
                $('#mensaje-content-' + intercodi).removeClass("bolder");
                $('#cb-intervencio-' + intercodi).attr("data-update", "");
                $('#item-mensaje-' + intercodi).attr("data-update", "");
            }

            $('.adjunto').on('click', function () {
                descargarArchivoMensaje($(this).attr('data-archivo-fisico'), $(this).attr('data-file-name'), $('#hfIdProgramacion').val(), $(this).attr('data-msg-id'));                
            });
           
        },
        error: function () {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function nuevaComunicacion() {

    if ($('#hfIntervencion').val() != "") {
        ARREGLO_INTERVENCION = [];
        ARREGLO_INTERVENCION.push($('#hfIntervencion').val());
    }

    var intercodis = ARREGLO_INTERVENCION;

    if (intercodis == "") {
        alert("Debe seleccionar una o varias intervenciones.");
    } else {
        $('#detalleMensaje').html("");
        $('#detalleMensaje').css('display', 'none');
        $('#nuevoMensaje').css('display', 'block');

        var url = controladorRegistro + 'IntervencionesMensajeRegistro' +
            '?intercodis=' + intercodis +
            '&origen=' + 'bandeja';

        $('#frmRegistro').attr('src', url);
    }
}

function descargarArchivoMensaje(filename, filenameoriginal, progcodi, carpetafiles) {    
    window.location = controladorRegistro + 'DescargarArchivoDePrograma' +
        '?modulo=' + 'mensajes' +
        '&fileName=' + filename +
        '&fileNameOriginal=' + filenameoriginal +
        '&progrcodi=' + progcodi +
        '&carpetafiles=' + carpetafiles +
        '&subcarpetafiles=' + 0;
}

function reportePDF() {
    if ($('#hfIntervencion').val() != "")
        window.location.href = controladorBandeja + "GenerarPDF?intercodi=" + $('#hfIntervencion').val();
}

function descargarZipArchivos() {
    if ($('#hfIntervencion').val() != "") {
        var interCodi = ($("#hfIntervencion").val()).trim();
        $.ajax({
            type: 'POST',
            url: controladorRegistro + 'DescargarArchivosSeleccionados',
            data: { tipo: interCodi },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controladorRegistro + "ExportarZip?file_name=" + evt.Resultado;
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Error al descargar archivos");
            }
        });
    }
}

function descargarZipComunicacion() {
    if($('#hfIntervencion').val() != ""){
        var interCodi = ($("#hfIntervencion").val()).trim();
        $.ajax({
            type: 'POST',
            url: controladorBandeja + 'DescargarMensajes',
            data: { intercodi: interCodi },
            success: function (evt) {
                if (evt.Resultado != "-1") {
                    window.location = controladorRegistro + "ExportarZipMsjMasivos?file_name=" + evt.Resultado;
                } else {
                    alert("Ha ocurrido un error: " + evt.Mensaje);
                }
            },
            error: function (err) {
                alert("Error al descargar archivos");
            }
        });
    }
}

function agregarIntervencion(id, indUpdate, msgcodi) {
    if (!ARREGLO_INTERVENCION.includes(id)) ARREGLO_INTERVENCION.push(id);
    if (ARREGLO_INTERVENCION.length > 1) {
        $('#detalleMensaje').html("");
        $('#btnDescargarAdjunto').css('display', 'none');
        $('#btnReportePDF').css('display', 'none');
        $('#hfIntervencion').val("");
        $('.mensaje-content').removeClass("mensaje-select");
    }
       
    if (ARREGLO_INTERVENCION.length == 1) {
        mostrarDetalle(id, 1, indUpdate, msgcodi);
    }

    $('#contenedorAcciones').css('display', 'block');
    $('#btnNuevaComunicacion').css('display', 'block');
}

function removerIntervencion(id) {
    if (ARREGLO_INTERVENCION.includes(id)) {
        const index = ARREGLO_INTERVENCION.indexOf(id);
        if (index > -1) {
            ARREGLO_INTERVENCION.splice(index, 1);
        }
    }
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).show();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
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

function CreateFormArchivo(path, params, method = 'post') {
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