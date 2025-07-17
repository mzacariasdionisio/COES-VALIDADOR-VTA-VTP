var controlador = siteRoot + 'CostoOportunidad/DatosSP7/';

const MENSUAL_IMP_DIAS_FALTANTES = 0;
const MENSUAL_IMP_TODO = 1;
const DIARIO = 2; //para importacion automatica


$(function () {
    

    $('#cbAnioPeriodo').change(function () {
        cargarPeriodos();        
    });

    $('#cbMesPeriodo').change(function () {        
        cargarVersiones();
    });

    $('#cbVersion').change(function () {
        cargarListadoImportados();
    });

    cargarListadoImportados();

    $("#btnImportarDF").click(function () {
        importarLasSenialesSP7(MENSUAL_IMP_DIAS_FALTANTES);
    });
    
    $("#btnImportarTodo").click(function () {
        importarLasSenialesSP7(MENSUAL_IMP_TODO);
    });
});

/** Carga lista de importados **/
function cargarListadoImportados() {
    var notamsg = "mensaje";

    var obj = obtenerFiltros();

    var msg = validarFiltros(obj);

    if (msg == "") {
        mostrarBotones();
        limpiarBarraMensaje(notamsg);

        $.ajax({
            type: 'POST',
            url: controlador + "ListarImportados",
            data: {
                copercodi: obj.copercodi,
                covercodi: obj.covercodi
            },
            success: function (evt) {                
                if (evt.Resultado != "-1") {

                    if (evt.MostrarBtnDF)
                        $("#btnImportarDF").css("display", "block");
                    else
                        $("#btnImportarDF").css("display", "none");

                    $('#listadoDeImportados').html(evt.Resultado);
                    refrehDatatable();

                } else {
                    mostrarMensaje_(notamsg, 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
                }
            },
            error: function (err) {
                mostrarMensaje_(notamsg, 'error', 'Ha ocurrido un error: ' + err);
            }
        });
    } else {
        mostrarMensaje_(notamsg, 'error', msg);
        ocultarBotones();
    }
}

function ocultarBotones() {
    $("#btnImportarDF").css("display", "none");
    $("#btnImportarTodo").css("display", "none");
    $("#listadoDeImportados").html("");
    
}

function mostrarBotones() {
    $("#btnImportarDF").css("display", "block");
    $("#btnImportarTodo").css("display", "block");
}

function validarFiltros(objCampos) {
    var msj = "";

    if (objCampos.copercodi == "") {
        msj += "<p>Debe escoger un periodo correcto.</p>";
    }

    if (objCampos.covercodi == 0) {
        msj += "<p>Debe escoger una versión correcta.</p>";
    }

    return msj;
}

function cargarPeriodos() {

    var annio = -1;

    annio = parseInt($("#cbAnioPeriodo").val()) || 0;
    $("#cbMesPeriodo").empty();

    $.ajax({
        type: 'POST',
        url: controlador + "PeriodoListado",
        data: {
            anio: annio,
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                if (evt.ListaPeriodos.length > 0) {
                    $.each(evt.ListaPeriodos, function (i, item) {
                        $('#cbMesPeriodo').get(0).options[$('#cbMesPeriodo').get(0).options.length] = new Option(item.Copernomb, item.Copercodi);
                    });            
                    cargarVersiones();
                    
                } else {
                    $('#cbMesPeriodo').get(0).options[0] = new Option("--", "0");
                }

            } else {
                mostrarMensaje_('mensaje', 'error',"Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error','Ha ocurrido un error.');
        }
    });
}

function cargarVersiones() {

    $.ajax({
        type: 'POST',
        url: controlador + 'obtenerversiones',
        data: {
            idPeriodo: $('#cbMesPeriodo').val()
        },
        dataType: 'json',
        global: false,
        success: function (result) {
            if (result != -1) {
                $('#cbVersion').get(0).options.length = 0;
                $('#cbVersion').get(0).options[0] = new Option("--Escoger versión--", "0");
                $.each(result, function (i, item) {
                    $('#cbVersion').get(0).options[$('#cbVersion').get(0).options.length] = new Option(item.Coverdesc, item.Covercodi);
                });
                cargarListadoImportados();
            }
            else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        },
        error: function () {
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}



function obtenerFiltros() {
    var filtro = {};

    filtro.copercodi = $("#cbMesPeriodo").val();
    filtro.covercodi = parseInt($("#cbVersion").val());

    return filtro;
}


function descargarReporteError(prodiacodi) {

    $.ajax({
        type: 'POST',
        url: controlador + "GenerarReporteError",
        data: {
            prodiacodi: prodiacodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {                
                    window.location = controlador + "Exportar?file_name=" + evt.Resultado;
            } else {                
                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

function importarLasSenialesSP7(tipo_Importacion) {
    var notamsg = "mensaje";
    obj = obtenerFiltros();

    var msgConfirmacion = "";
    var msgTodo = "¿Seguro de realizar la importación de señales SP7 para todo el periodo y version seleccionado?";
    var msgdf = "¿Seguro de realizar la importación de señales SP7 para los dias faltantes del periodo y version seleccionado?";

    if (tipo_Importacion == MENSUAL_IMP_TODO) msgConfirmacion = msgTodo;
    if (tipo_Importacion == MENSUAL_IMP_DIAS_FALTANTES) msgConfirmacion = msgdf;

    if (confirm(msgConfirmacion)) {
        var msg = validarFiltros(obj);

        if (msg == "") {
            limpiarBarraMensaje(notamsg);

            $.ajax({
                type: 'POST',
                url: controlador + "importarSenialesSP7",
                data: {
                    tipoImportacion: tipo_Importacion,
                    copercodi: obj.copercodi,
                    covercodi: obj.covercodi
                },
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        cargarListadoImportados();
                    } else {

                        mostrarMensaje_(notamsg, 'error', 'Ha ocurrido un error.' + evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje_(notamsg, 'error', 'Ha ocurrido un error.');
                }
            });
        }
        else {
            mostrarMensaje_(notamsg, 'error', msg);
        }
    }
}

function cerrarPopup(id) {
    $(id).bPopup().close()
}

function abrirPopup(contentPopup) {
    setTimeout(function () {
        $("#" + contentPopup).bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function refrehDatatable() {
    $('#tabla_lstImportados').dataTable({
        "scrollY": 430,
        "scrollX": true,
        "sDom": 't',
        "ordering": false,
        "iDisplayLength": 50
    });
}


function mostrarMensaje_(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}