var controlador = siteRoot + 'PMPO/EscenarioSDDP/';

var AGREGAR_ES = 1;
var EDITAR_ES = 2;
var DETALLES_ES = 3;

var DE_LISTADO = 1;
var DE_POPUP = 2;

var validarCambioDePestaña = true;
var contador = 0;

$(function () {

    $('#tab-container').easytabs({
        animate: false,
        select: '#vistaListado'
    });

    $('#tab-container').bind('easytabs:before', function () {
        var esTabDetalle = $("#tab-container .tab.active").html().toLowerCase().includes('detalle');
        var existeHtmlTabDetalle = $("#vistaDetalle").html().trim() != '';
        var esEditarCrear = parseInt($("#hfAccionDetalle").val()) != DETALLES_ES;

        if (validarCambioDePestaña) {
            if (esTabDetalle && existeHtmlTabDetalle && esEditarCrear) {
                if (confirm('¿Desea cambiar de pestaña? Si selecciona "Aceptar" se perderán los cambios. Si selecciona "Cancelar" se mantendrá en la misma pestaña')) {
                    $("#vistaDetalle").html(''); //limpiar tab Detalle                    
                } else {
                    return false;
                }
            }
        }
        validarCambioDePestaña = true;
    });

    mostrarListadoEscenariosSppd();

    $('#btnAgregarEscenario').click(function () {
        limpiarCamposNuevoEscenario();
        abrirPopup("crearEscenario");
    });

    $('#btnCancelarEscenario').click(function () {
        $('#crearEscenario').bPopup().close();
    });

    $('#btnCrearEscenario').click(function () {
        limpiarBarraMensaje('mensajeCrearEscenario');

        var obj = {};
        obj = getObjetoCrearESJson();
        var mensaje = validarDatosPopupCrearEscenario(obj);
        if (mensaje == "") {
            validarYCrearNuevoEscenario(AGREGAR_ES, obj);

        } else {
            mostrarMensaje_('mensajeCrearEscenario', 'error', mensaje);
        }
    });

    $('#cmbResolucion').change(function () {
        var tipoSeleccionado = $('#cmbResolucion').val();
        actualizarMesAnioNuevoEscenario(tipoSeleccionado);
    });

});

function mostrarListadoEscenariosSppd() {

    $.ajax({
        type: 'POST',
        url: controlador + "ListarEscenariosSddp",
        dataType: 'json',
        data: {},
        success: function (evt) {
            if (evt.Resultado != "-1") {

                $("#cuadroEscenarios").html(evt.HtmlListadoEscenariosSddp);
                refrehDatatable();


            } else {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error: ' + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
        }
    });
}

function refrehDatatable() {
    $('#tabla_EscenariosSddp').dataTable({
        "scrollY": 430,
        "scrollX": true,
        "sDom": 't',
        "ordering": false,
        "iDisplayLength": 50
    });
}


function mantenerEscenarioSddp(accion, topcodi, origen) {
    $('#tab-container').easytabs('select', '#vistaDetalle');

    mostrarVistaDetalles(accion, topcodi);

    if (origen == DE_POPUP)
        $('#historialES').bPopup().close();
}

function mostrarVistaDetalles(accion, topcodi) {
    $("#vistaDetalle").html('');

    $.ajax({
        type: 'POST',
        url: controlador + "CargarDetalles",
        data: {
            accion: accion,
            topcodi: topcodi
        },
        success: function (evt) {

            $('#vistaDetalle').html(evt);

            $('#btnGenerarDat').click(function () {
                generarArchivos(topcodi);
            });

        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error: ' + err);
        }
    });
}


function validarYCrearNuevoEscenario(accion, objDato) {

    $.ajax({
        type: 'POST',
        url: controlador + "CrearNuevoEscenario",
        data: {
            accion: accion,
            resolucion: objDato.resolucion,
            anio: parseInt(objDato.anio),
            mes: parseInt(objDato.mes)
        },
        success: function (resultado) {
            if (resultado.Resultado == "1") {
                limpiarCamposNuevoEscenario();
                $('#crearEscenario').bPopup().close();
                mostrarMensaje_('mensaje', 'exito', 'Escenario SDDP registrado con éxito.');
                mostrarListadoEscenariosSppd();
            }
            if (resultado.Resultado == "-1") {
                mostrarMensaje_('mensajeCrearEscenario', 'error', resultado.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error:' + err);
        }
    });
}


function marcarOficial(mtopcodi) {
    if (confirm('¿Desea asignar como oficial el escenario seleccionado?” ')) {
        $.ajax({
            url: controlador + "MarcarOficial",
            data: {
                mtopcodi: mtopcodi
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al marcar oficial: ' + result.Mensaje);
                } else {
                    //actualizar las fechas popup crear
                    $("#hfNuevoMesAnioSemanal").val(result.MesAnioSemanal);
                    $("#hfNuevoMesAnioMensual").val(result.MesAnioMensual);

                    mostrarListadoEscenariosSppd();
                    mostrarMensaje_('mensaje', 'exito', 'El escenario seleccionado se marcó como Oficial.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}


function quitarOficial(mtopcodi, origen) {
    if (confirm('¿Desea quitar el identificador Oficial al escenario seleccionado?” ')) {
        $.ajax({
            url: controlador + "QuitarOficial",
            data: {
                mtopcodi: mtopcodi
            },
            type: 'POST',
            success: function (result) {
                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al quitar oficial: ' + result.Mensaje);
                } else {
                    //actualizar las fechas popup crear
                    $("#hfNuevoMesAnioSemanal").val(result.MesAnioSemanal);
                    $("#hfNuevoMesAnioMensual").val(result.MesAnioMensual);

                    if (origen == DE_POPUP)
                        $('#historialES').bPopup().close();

                    mostrarListadoEscenariosSppd();
                    mostrarMensaje_('mensaje', 'exito', 'El escenario seleccionado fue desoficializada.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}

function mostrarEscenariosTotales(mtopcodi) {
    $('#listadoHES').html('');

    $.ajax({
        type: 'POST',
        url: controlador + "ListadoEscenarioTotales",
        data: {
            mtopcodi: mtopcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                $('#listadoHES').html(evt.Resultado);

                $("#listadoHES").css("width", (860) + "px");

                abrirPopup("historialES");
            } else {

                mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error al listar los escenarios para el periodo seleccionado: ' + evt.Mensaje);
            }
        },
        error: function (err) {

            mostrarMensaje_('mensaje', 'error', 'Ha ocurrido un error al listar los escenarios totales.');
        }
    });
}

function eliminarEscenario(mtopcodi) {

    var msgConfirmacion = '¿Esta seguro que desea eliminar el escenario seleccionado?';

    if (confirm(msgConfirmacion)) {
        $.ajax({
            url: controlador + "EliminarEscenario",
            data: {
                mtopcodi: mtopcodi
            },
            type: 'POST',
            success: function (result) {

                if (result.Resultado === "-1") {
                    mostrarMensaje_('mensaje', 'error', 'Se ha producido un error al eliminar el escenario: ' + result.Mensaje);
                } else {
                    //actualizar las fechas popup crear
                    $("#hfNuevoMesAnioSemanal").val(result.MesAnioSemanal);
                    $("#hfNuevoMesAnioMensual").val(result.MesAnioMensual);

                    mostrarListadoEscenariosSppd();
                    mostrarMensaje_('mensaje', 'exito', 'Eliminación del escenario realizado con éxito.');
                }
            },
            error: function (xhr, status) {
                mostrarMensaje_('mensaje', 'error', 'Se ha producido un error.');
            }
        });
    }
}



function limpiarCamposNuevoEscenario() {
    $('#cmbResolucion').val("0");
    $("#anioCrear").css("display", "none");
}

function actualizarMesAnioNuevoEscenario(opcion) {
    var mesAnio = "";
    if (opcion == "0") { //seleccione tipo
        $("#anioCrear").css("display", "none");
    } else {
        $("#anioCrear").css("display", "block");

        if (opcion == "S") { //tipo : semanal
            mesAnio = $("#hfNuevoMesAnioSemanal").val();

            $("#txtMesAnio").val(($("#hfNuevoMesAnioSemanal").val()).replace('*', ''));
        }
        if (opcion == "M") { //tipo : mensual
            mesAnio = $("#hfNuevoMesAnioMensual").val();

            $("#txtMesAnio").val(($("#hfNuevoMesAnioMensual").val()).replace('*', ''));
        }
    }

    if (mesAnio.includes("*")) {// cuando no existe registros anteriores
        $('#txtMesAnio').Zebra_DatePicker({
            format: 'm Y',
        });
    } else {// cuando  ya existe registrados
        $('#txtMesAnio').Zebra_DatePicker({
            format: 'm Y',
            direction: [mesAnio.replace('*', ''), "12 2100"],
        });
    }

}

function validarDatosPopupCrearEscenario(obj) {

    var msj = "";
    if (obj.resolucion == "0") {
        msj += "<p>Debe seleccionar una resolución.</p>";
    }

    if (obj.aniomes == "0") {
        msj += "<p>Debe ingresar un periodo.</p>";
    }

    return msj;
}

function getObjetoCrearESJson() {
    var obj = {};
    var txtMesAnio = $("#txtMesAnio").val();
    var arrayNumeros = txtMesAnio.split(' ');
    var anio = "0";
    var mes = "0";

    if (arrayNumeros.length > 0) {
        anio = arrayNumeros[1];
        mes = arrayNumeros[0];
    }

    obj.resolucion = $("#cmbResolucion").val() || "0";
    obj.aniomes = txtMesAnio || "0";
    obj.anio = anio;
    obj.mes = mes;

    return obj;
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

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function mostrarMensaje_(id, tipo, mensaje) {
    $("#" + id).css("display", "block");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);

    await sleep(6000);

    limpiarBarraMensaje(id);
}

function limpiarBarraMensaje(id) {
    $('#' + id).css("display", "none");
    $('#' + id).removeClass();
    $('#' + id).addClass('action-message');
    $('#' + id).html('');
}

function generarArchivos(mtopcodi) {
    $.ajax({
        type: 'POST',
        url: controlador + "GenerarArchivoSalida",
        data: {
            topcodi: mtopcodi
        },
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarZip?file_name=" + evt.Resultado;
            } else {
                mostrarMensaje_('mensaje', 'error', evt.Mensaje);
            }
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });

}



function mostrarCopiaCentralesSddp(mtopcodi, accion) {
    var url = siteRoot + 'PMPO/CentralSDDP/Index';
    cargarFrame(url, mtopcodi, accion, 1550);
}

function mostrarCopiaModificaciones(topcodi, accion) {
    var url = siteRoot + 'PMPO/ModificacionCentralSDDP/Index';
    cargarFrame(url, topcodi, accion, 650);
}

function mostrarCopiaVolumenes(mtopcodi, accion) {
    var url = siteRoot + 'PMPO/volumenembalse/Index';
    cargarFrame(url, mtopcodi, accion, 700);
}

function cargarFrame(url, mtopcodi, accion, alto) {
    var urlFrame = url;

    $('#myiframe').html('');
    $('#myiframe').attr("src", urlFrame + "?mtopcodi=" + mtopcodi + "&acn=" + accion);
    $('#myiframe').attr("height", alto);
    $('#myiframe').attr("width", $(window).width() - 260);

    if (contador != 0)
        navigationFn.goToSection('#myiframe');

    contador++;
}

var navigationFn = {
    goToSection: function (id) {
        $('html, body').animate({
            scrollTop: $(id).offset().top - 30
        }, 150);
    }
}