var controlador = siteRoot + 'Combustibles/gestionGas/';

const ESTADO_SOLICITADO = 1;
const ESTADO_PROCESADO = 2;
const ESTADO_APROBADO = 3;
const ESTADO_DESAPROBADO = 4;
const ESTADO_FUERA_PLAZO = 5;
const ESTADO_OBSERVADO = 6;
const ESTADO_SUBSANADO = 7;
const ESTADO_CANCELADO = 8;
const ESTADO_APROBADO_PARCIAL = 10;
const ESTADO_SOLICITUD_ASIGNACION = 11;
const ESTADO_ASIGNADO = 12;

const APROBAR = 1;
const APROBAR_PARCIALMENTE = 2;
const DESAPROBAR = 3;
const OBSERVAR = 4;
const ASIGNAR = 5;

const FORMATO3 = 0;
const ARCHIVOS = 1;

var centrales_aprobadas = "";
var centrales_desaprobadas = "";

$(function () {


    $('#btnInicio').click(function () {
        regresarListaPrincipal();
    });

    /******* Observar *******/
    $("#obs_fecMaxRpta").Zebra_DatePicker({
        format: 'd/m/Y',
        direction: [diaActual(), false]
    });

    $("#horaSistema").Zebra_DatePicker({
        format: 'd/m/Y H:i:s',
        direction: [$("#hdFecSistema").val(), false]
    });


    $('#btnPopupObservar').click(function () {
        //validar handsons
        mostrarVentanaObservar();
    });

    $('#btnObservar').click(function () {
        guardarObservacion();
    });

    /******* Aprobar *******/
    $('#btnPopupAprobar').click(function () {
        //validar handsons
        mostrarVentanaAprobar();
    });

    $('#btnAprobar').click(function () {
        guardarAprobacion();
    });

    /******* Desaprobar *******/
    $('#btnPopupDesaprobar').click(function () {
        mostrarVentanaDesaprobar();
    });

    $('#btnDesaprobar').click(function () {
        guardarDesaprobacion();
    });

    /******* Aprobar Parcialmente *******/
    $('#btnPopupAprobarParcialmente').click(function () {
        mostrarVentanaAprobarParcialmente();
    });

    $('#btnAprobarP').click(function () {
        guardarAprobacionParcial();
    });

    /******* Asignar Costo *******/
    $('#btnPopupAsignar').click(function () {
        mostrarVentanaAsignar();
    });

    $('#btnAsignar').click(function () {
        guardarAsignacion();
    });

    $('#btnVerCostos').click(function () {
        var url = siteRoot + 'Migraciones/Parametro/Index';
        window.open(url).focus();
    });

    /******* Descargar Detalles *******/
    $('#btnDescargarFormato').click(function () {
        exportarFormato();
    });
});

function regresarListaPrincipal(estado) {
    var estadoEnvio = parseInt($("#hdIdEstado").val()) || 0;

    if (estado != null) {
        estadoEnvio = estado;
    }

    if (estadoEnvio == 11) estadoEnvio = 1;

    document.location.href = controlador + "Index?carpeta=" + estadoEnvio;
}

function visibilidadBotones() {

    var estadoEnvio = parseInt($("#hdIdEstado").val()) || 0;
    var esEditable = $("#hdEditable").val();

    if (esEditable == 1) {
        if (estadoEnvio == ESTADO_SOLICITADO) {
            $("#bq_aprobar").css("display", "inline-table");

            //si es cental nueva muestra botones de aprobar parcialmente y de desaprobar
            if ($("#hdTipoCentral").val() == "N") {
                if (MODELO_LISTA_CENTRAL.length > 1) $("#bq_aprobarparcialmente").css("display", "inline-table");
                $("#bq_desaprobar").css("display", "inline-table");
            }

            $("#bq_observar").css("display", "inline-table");
            $("#bq_descargar").css("display", "inline-table");

            $("#bq_descargar").css("float", "right");
            $("#bq_observar").css("float", "right");

            //si es cental nueva muestra botones de aprobar parcialmente y de desaprobar
            if ($("#hdTipoCentral").val() == "N") {
                $("#bq_desaprobar").css("float", "right");
                $("#bq_aprobarparcialmente").css("float", "right");
            }
            $("#bq_aprobar").css("float", "right");
        } else {

            if (estadoEnvio == ESTADO_SUBSANADO) {
                $("#bq_aprobar").css("display", "inline-table");
                if (MODELO_LISTA_CENTRAL.length > 1) $("#bq_aprobarparcialmente").css("display", "inline-table");
                $("#bq_desaprobar").css("display", "inline-table");
                $("#bq_descargar").css("display", "inline-table");

                $("#bq_aprobar").css("float", "right");
                $("#bq_aprobarparcialmente").css("float", "right");
                $("#bq_desaprobar").css("float", "right");
                $("#bq_descargar").css("float", "right");
            } else {

                if (estadoEnvio == ESTADO_SOLICITUD_ASIGNACION) {
                    $("#bq_asignar").css("display", "inline-table");

                    $("#bq_asignar").css("float", "right");
                }
                else {
                    //para los VER DETALLES
                    $("#bq_descargar").css("display", "inline-table");
                    $("#bq_descargar").css("float", "right");
                }
            }
        }
    } else {
        //para los VER DETALLES
        $("#bq_descargar").css("display", "inline-table");
        $("#bq_descargar").css("float", "right");
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Observar
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function mostrarVentanaObservar() {

    limpiarBarraMensaje("mensaje");
    var msg = "";
    msg = validarSecciones(OBSERVAR);

    if (msg == "") {
        limpiarPopupObservar();
        abrirPopup("popupObservar");
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function limpiarPopupObservar() {
    limpiarBarraMensaje("mensaje_popupObservar");
    $("#obs_fecMaxRpta").val($("#obs_hffecMaxRpta").val());
    $("#obs_ccAgente").val($("#obs_hfccAgente").val());
}

function guardarObservacion() {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupObservar");

    var correo = getPlantillaCorreo(OBSERVAR);

    var msg = validarCamposAGuardar(correo, OBSERVAR);

    if (msg == "") {
        if (confirm('¿Desea observar el envío de costo de combustible gaseoso?')) {

            var tipoCentral = $("#hdTipoCentral").val();
            var tipoOpcion = $("#hdTipoOpcion").val();

            //actualizar objeto MODELO_LISTA_CENTRAL, MODELO_DOCUMENTO
            if (tipoOpcion != "SA") {
                actualizarModeloMemoria();
                actualizarModeloSustentoMemoria();
            }

            var formulario = {
                IdEnvio: parseInt($('#hfIdEnvio').val()),
                FechaMaxRpta: $('#obs_fecMaxRpta').val(),
                CorreosCc: correo.PlanticorreosCc,
                ListaFormularioCentral: MODELO_LISTA_CENTRAL_JSON,
                FormularioSustento: MODELO_DOCUMENTO_JSON,
            };

            var dataJson = {
                data: JSON.stringify(formulario)
            };

            $.ajax({
                type: 'POST',
                url: controlador + 'GrabarObservacion',
                dataType: 'json',
                contentType: 'application/json; charset=UTF-8',
                data: JSON.stringify(dataJson),
                cache: false,
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        alert('Se envío la observacion a el/los Agente(s).');
                        regresarListaPrincipal(ESTADO_OBSERVADO);
                    } else {
                        mostrarMensaje('mensaje_popupObservar', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);

                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupObservar', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    } else {
        mostrarMensaje('mensaje_popupObservar', 'error', msg);
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Aprobar
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function mostrarVentanaAprobar() {
    limpiarBarraMensaje("mensaje");
    var msg = "";
    msg = validarSecciones(APROBAR);

    if (msg == "") {
        limpiarPopupAprobar();
        abrirPopup("popupAprobar");
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function limpiarPopupAprobar() {
    limpiarBarraMensaje("mensaje_popupAprobar");
    $("#aprob_ccAgente").val($("#obs_hfccAgente").val());
}

function guardarAprobacion() {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupAprobar");

    var correo = getPlantillaCorreo(APROBAR);

    var msg = validarCamposAGuardar(correo, APROBAR);

    if (msg == "") {
        if (confirm('¿Desea aprobar el envío de costo de combustible gaseoso?')) {

            var tipoCentral = $("#hdTipoCentral").val();
            var tipoOpcion = $("#hdTipoOpcion").val();

            //actualizar objeto MODELO_LISTA_CENTRAL, MODELO_DOCUMENTO
            if (tipoOpcion != "SA") {
                actualizarModeloMemoria();
                actualizarModeloSustentoMemoria();
            }

            var formulario = {
                IdEnvio: parseInt($('#hfIdEnvio').val()),
                Correo: correo,
                ListaFormularioCentral: MODELO_LISTA_CENTRAL_JSON,
                FormularioSustento: MODELO_DOCUMENTO_JSON,
            };

            var dataJson = {
                data: JSON.stringify(formulario)
            };

            $.ajax({
                type: 'POST',
                url: controlador + 'GrabarAprobacion',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify(dataJson),
                cache: false,
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        alert('Se efectuó la aprobación correctamente.');
                        regresarListaPrincipal(ESTADO_APROBADO);
                    } else {
                        mostrarMensaje('mensaje_popupAprobar', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);

                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupAprobar', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    } else {
        mostrarMensaje('mensaje_popupAprobar', 'error', msg);
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Desaprobar
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function mostrarVentanaDesaprobar() {
    limpiarBarraMensaje("mensaje");
    var msg = "";
    msg = validarSecciones(DESAPROBAR);

    if (msg == "") {
        limpiarPopupDesaprobar();
        abrirPopup("popupDesaprobar");
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function limpiarPopupDesaprobar() {
    limpiarBarraMensaje("mensaje_popupDesaprobar");
    $("#desaprob_mensajeCoes").val("");
    $("#desaprob_ccAgente").val($("#desaprob_hfccAgente").val());
}

function guardarDesaprobacion() {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupDesaprobar");

    var correo = getPlantillaCorreo(DESAPROBAR);

    var msg = validarCamposAGuardar(correo, DESAPROBAR);

    if (msg == "") {

        if (confirm('¿Desea desaprobar el envío de costo de combustible gaseoso?')) {

            var tipoCentral = $("#hdTipoCentral").val();
            var tipoOpcion = $("#hdTipoOpcion").val();

            //actualizar objeto MODELO_LISTA_CENTRAL, MODELO_DOCUMENTO
            if (tipoOpcion != "SA") {
                actualizarModeloMemoria();
                actualizarModeloSustentoMemoria();
            }

            var formulario = {
                IdEnvio: parseInt($('#hfIdEnvio').val()),
                Correo: correo,
                ListaFormularioCentral: MODELO_LISTA_CENTRAL_JSON,
                FormularioSustento: MODELO_DOCUMENTO_JSON,
            };

            var dataJson = {
                data: JSON.stringify(formulario)
            };

            $.ajax({
                type: 'POST',
                url: controlador + 'GrabarDesaprobacion',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify(dataJson),
                cache: false,
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        alert('Se efectuó la desaprobación correctamente.');
                        regresarListaPrincipal(ESTADO_DESAPROBADO);
                    } else {
                        mostrarMensaje('mensaje_popupDesaprobar', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupDesaprobar', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    }
    else {
        mostrarMensaje('mensaje_popupDesaprobar', 'error', msg);
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Aprobar parcialmente
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function mostrarVentanaAprobarParcialmente() {
    limpiarBarraMensaje("mensaje");
    var msg = "";
    msg = validarSecciones(APROBAR_PARCIALMENTE);

    if (msg == "") {
        $("#aprobp_centralAprob").html(centrales_aprobadas);
        $("#aprobp_centralDesaprob").html(centrales_desaprobadas);
        limpiarPopupAprobarParcialmente();
        abrirPopup("popupAprobarParcialmente");
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function limpiarPopupAprobarParcialmente() {
    limpiarBarraMensaje("mensaje_popupAprobarParcialmente");
    $("#aprobp_mensajeCoes").val("");
    $("#aprobp_ccAgente").val($("#aprobp_hfccAgente").val());
}

function guardarAprobacionParcial() {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupAprobarParcialmente");

    var correo = {};
    correo = getPlantillaCorreo(APROBAR_PARCIALMENTE);

    var msg = validarCamposAGuardar(correo, APROBAR_PARCIALMENTE);

    if (msg == "") {

        var equicodisCAprobadas = $("#cbEquicodisCentralesAprobadasF3").val();
        var equicodisCDesaprobadas = $("#cbEquicodisCentralesDesaprobadasF3").val();

        if (confirm('¿Desea aprobar parcialmente el envío de costo de combustible gaseoso?')) {

            var tipoCentral = $("#hdTipoCentral").val();
            var tipoOpcion = $("#hdTipoOpcion").val();

            //actualizar objeto MODELO_LISTA_CENTRAL, MODELO_DOCUMENTO
            if (tipoOpcion != "SA") {
                actualizarModeloMemoria();
                actualizarModeloSustentoMemoria();
            }

            var formulario = {
                IdEnvio: parseInt($("#hfIdEnvio").val()),
                LstCentralesAprob: equicodisCAprobadas,
                LstCentralesDesaprob: equicodisCDesaprobadas,
                CorreosCc: correo.PlanticorreosCc,
                Plantcontenido: correo.Plantcontenido,
                ListaFormularioCentral: MODELO_LISTA_CENTRAL_JSON,
                FormularioSustento: MODELO_DOCUMENTO_JSON,
            };

            var dataJson = {
                data: JSON.stringify(formulario)
            };

            $.ajax({
                type: 'POST',
                url: controlador + 'GrabarAprobacionParcial',
                dataType: 'json',
                contentType: 'application/json; charset=UTF-8',
                data: JSON.stringify(dataJson),
                cache: false,
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        alert('Se efectuó la aprobación parcial correctamente.');
                        regresarListaPrincipal(ESTADO_APROBADO_PARCIAL);
                    } else {
                        mostrarMensaje('mensaje_popupAprobarParcialmente', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);
                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupAprobarParcialmente', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    }
    else {
        mostrarMensaje('mensaje_popupAprobarParcialmente', 'error', msg);
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Asignación
////////////////////////////////////////////////////////////////////////////////////////////////////////////
function mostrarVentanaAsignar() {
    limpiarBarraMensaje("mensaje");
    var msg = validarExistenciaCosto();

    if (msg == "") {
        limpiarPopupAsignar();
        abrirPopup("popupAsignar");
    } else {
        mostrarMensaje('mensaje', 'error', msg);
    }
}

function validarExistenciaCosto() {
    var msj = "";
    var costoCmbActual = $("#asig_hdCostoCombActual").val();

    //verifica que actualmente haya un costo de combustible para hoy
    if (costoCmbActual == null || costoCmbActual == "") {
        msj += "<p>No existe un costo de combustible gaseoso vigente para hoy.</p>";
    }

    return msj;
}

function limpiarPopupAsignar() {
    limpiarBarraMensaje("mensaje_popupAsignar");
    $("#asig_ccAgente").val($("#asig_hfccAgente").val());
}

function guardarAsignacion() {
    limpiarBarraMensaje("mensaje");
    limpiarBarraMensaje("mensaje_popupAsignar");

    var correo = {};
    correo = getPlantillaCorreo(ASIGNAR);

    var msg = validarCamposAGuardar(correo, ASIGNAR);

    if (msg == "") {
        if (confirm('¿Desea asignar el costo de combustible gaseoso?')) {
            var modeloCloneCentral = JSON.parse(JSON.stringify(MODELO_LISTA_CENTRAL));

            var formulario = {
                IdEnvio: parseInt($('#hfIdEnvio').val()),
                Correo: correo,
                ListaFormularioCentral: modeloCloneCentral,
            };

            var dataJson = {
                data: JSON.stringify(formulario)
            };

            $.ajax({
                type: 'POST',
                url: controlador + 'GrabarAsignacion',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify(dataJson),
                cache: false,
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        alert('Se efectuó la asignación correctamente.');
                        regresarListaPrincipal(ESTADO_ASIGNADO);
                    } else {
                        mostrarMensaje('mensaje_popupAsignar', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);

                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje_popupAsignar', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    } else {
        mostrarMensaje('mensaje_popupAsignar', 'error', msg);
    }
}

function validarCamposAGuardar(correo, accion) {
    var msj = "";
    var validaCc;

    if (accion == OBSERVAR) {
        /*validacion del campo Fecha Maxima*/
        if ($('#obs_fecMaxRpta').val() == "") {
            msj += "<p>Error encontrado en el campo Fecha Máxima. Ingrese un dato válido.</p>";
        }

        /*validacion del campo CC*/
        validaCc = validarCorreo($('#obs_ccAgente').val(), 0, -1);
        if (validaCc < 0) {
            msj += "<p>Error encontrado en el campo CC Agentes. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
        }
    }

    if (accion == APROBAR) {
        /*validacion del campo CC*/
        validaCc = validarCorreo($('#aprob_ccAgente').val(), 0, -1);
        if (validaCc < 0) {
            msj += "<p>Error encontrado en el campo CC Agentes. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
        }
    }

    if (accion == DESAPROBAR) {
        /*validacion del campo CC*/
        validaCc = validarCorreo($('#desaprob_ccAgente').val(), 0, -1);
        if (validaCc < 0) {
            msj += "<p>Error encontrado en el campo CC Agentes. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
        }
    }

    if (accion == APROBAR_PARCIALMENTE) {
        /*validacion del campo CC*/
        validaCc = validarCorreo($('#aprobp_ccAgente').val(), 0, -1);
        if (validaCc < 0) {
            msj += "<p>Error encontrado en el campo CC Agentes. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
        }
    }

    if (accion == ASIGNAR) {
        /*validacion del campo CC*/
        validaCc = validarCorreo($('#asig_ccAgente').val(), 0, -1);
        if (validaCc < 0) {
            msj += "<p>Error encontrado en el campo CC Agentes. Revisar que el/los correo(s) sea(n) válido(s) y separados por punto y coma (;).</p>";
        }
    }




    return msj;
}

function getPlantillaCorreo(accion) {
    var obj = {};
    obj.PlanticorreosCc = "";

    if (accion == OBSERVAR)
        obj.PlanticorreosCc = $("#obs_ccAgente").val();

    if (accion == APROBAR)
        obj.PlanticorreosCc = $("#aprob_ccAgente").val();

    if (accion == DESAPROBAR) {
        obj.PlanticorreosCc = $("#desaprob_ccAgente").val();
        obj.Plantcontenido = $('#desaprob_mensajeCoes').val();
    }

    if (accion == APROBAR_PARCIALMENTE) {
        obj.PlanticorreosCc = $("#aprobp_ccAgente").val();
        obj.Plantcontenido = $('#aprobp_mensajeCoes').val();
    }

    if (accion == ASIGNAR)
        obj.PlanticorreosCc = $("#asig_ccAgente").val();



    return obj;
}

function exportarFormato() {
    var idEnvio = $("#hfIdEnvio").val();
    var mesVigencia = $("#campoMesVigencia").val();

    $.ajax({
        type: 'POST',
        url: controlador + 'ExportarFormato',
        data: {
            idEnvio: idEnvio,
            mesVigencia: mesVigencia
        },
        dataType: 'json',
        success: function (evt) {
            if (evt.Resultado != "-1") {
                window.location = controlador + "ExportarZip?file_name=" + evt.Resultado;
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
        }
    });
}

//VALIDAR SECCION
const SIN_ESTADO_SELEC = 0;
const CONFORME = 1;
const OBSERVADO = 2;
const OBSERVADO_INCOMPLETO = 12;
const SUBSANADO = 3;
const SUBSANADO_INCOMPLETO = 13;
const NOSUBSANADO = 4;
const NOSUBSANADO_INCOMPLETO = 14;
const CONFORME_PARCIALMENTE = 5;

function validarSecciones(accion) {
    var msj = "";

    var lstDatosCentrales = obtenerDatosCentrales(accion);
    var listaArchivo = obtenerDataHandsonArchivo(accion);

    //validar
    var msj2 = "";
    var msjVal1 = validarTabFormato3(lstDatosCentrales, accion);
    var msjVal2 = validarTabSustento(listaArchivo, accion);

    if (msjVal1 != "") msj2 += msjVal1 + "<br>";
    if (msjVal2 != "") msj2 += msjVal2 + "<br/>";

    var numCentralesTotales = lstDatosCentrales.length;
    var lstStrCentralesAprobadas = obtenerStrListaCentralesAprobadas(lstDatosCentrales);
    var lstStrCentralesDesaprobadas = obtenerStrListaCentralesDesaprobadas(lstDatosCentrales);
    var numCentralesAprobadas = obtenerNumeroCentralesAprobadasEnFormato3(lstDatosCentrales);  //todas secciones en CONFORME
    var numCentralesDesaprobadas = obtenerNumeroCentralesDesaprobadasEnFormato3(lstDatosCentrales); //Al menos en una seccion se elige: NO SUBSANADO (eXISTENTES) u Observado (para C Nuevas)
    var numCentralesEnRevision = obtenerNumeroCentralesEnRevisionEnFormato3(lstDatosCentrales);

    var numCentralesObservadas = obtenerNumeroCentralesObservadasEnHandson(lstDatosCentrales); //Al menos en una seccion se elige: NO SUBSANADO (eXISTENTES) u Observado (para C Nuevas)

    //Valida que todas las secciones esten en Conforme (en ambas pestañas: f3 y archivos) 
    if (accion == APROBAR) {
        if (numCentralesAprobadas != numCentralesTotales) {
            msj += "<p>Hay centrales (con el Formato 3 / Informe sustentatorio) que aún no han sido aprobadas.</p>";
        }

        msj += msj2;
    }

    //Valida que todas las secciones haya como minimo un NO SUBSANADO (en ambas pestañas: f3 y archivos) 
    if (accion == DESAPROBAR) {
        if (numCentralesDesaprobadas == 0) {
            msj += "<p>No existe centrales desaprobadas (donde, para centrales nuevas y existentes se debe elegir al menos una opción: No Subsanado).</p>";
        }
    }

    //Valida que exista secciones Observadas, que se haya ingresado comentario (en ambas pestañas: f3 y archivos)
    if (accion == OBSERVAR) {
        if (numCentralesObservadas == 0) {
            msj += "<p>No existe centrales Observadas (centrales con al menos una sección elegida la opción: Observado).</p>";
        }

        msj += msj2;
    }

    //Valida que
    centrales_aprobadas = lstStrCentralesAprobadas.trim();
    centrales_desaprobadas = lstStrCentralesDesaprobadas.trim();
    if (accion == APROBAR_PARCIALMENTE) {
        if (numCentralesEnRevision > 0) {
            msj += "<p>Hay centrales (con el Formato 3 / Informe sustentatorio) que aún no han sido revisadas. Para aprobar parcialmente debe revisar todas las centrales.</p>";
        } else {
            if (numCentralesAprobadas == 0) {
                msj += "<p>No hay centrales aprobadas. Para aprobar parcialmente un envío, debe haber como mínimo una central aprobada.</p>";
            }
            if (numCentralesDesaprobadas == 0) {
                msj += "<p>No hay centrales desaprobadas. Para aprobar parcialmente un envío, debe haber como mínimo una central desaprobada.</p>";
            }
        }

        msj += msj2;
    }


    return msj;
}

function obtenerDatosCentrales(accion) {

    var listaCentrales = MODELO_LISTA_CENTRAL; //variable global de solicitudExcelWeb.js

    var listaDataCentrales = [];
    for (var i = 0; i < listaCentrales.length; i++) {

        var central = listaCentrales[i];

        var objCentralX = {
            Equicodi: central.Equicodi, //codigo
            NombCentral: central.Central, //nombre central
            Tipo: $("#hdTipoCentral").val(), //'N':Nueva, 'E':Existente
            DataFormularioF3: obtenerDataHandson(i, central.Equicodi, central.ArrayItemObs, accion),
        };

        listaDataCentrales.push(objCentralX);
    }

    return listaDataCentrales;
}

function obtenerDataHandson(posTab, equicodi, listaItemObs, accion) {
    var listaDataSeccion = [];

    //
    var objTabCentral = MODELO_LISTA_CENTRAL[posTab];
    var dataHandson = LISTA_OBJETO_HOJA[equicodi].hot.getData();
    var posColSegObs = OBSERVAR == accion ? -3 : -1;

    for (var i = 0; i < listaItemObs.length; i++) {
        var objItemObs = listaItemObs[i];
        if (objItemObs.EsColEstado) {

            var codigoEstado = SIN_ESTADO_SELEC;
            var cadenaEstado = (dataHandson[objItemObs.PosRow][objItemObs.PosCol] ?? "").toUpperCase();

            var objItemObsColSub = _getItemObsByPos(objTabCentral.ArrayItemObs, objItemObs.PosRow, objItemObs.PosCol + posColSegObs);
            var observacionHtml = (objItemObsColSub.Obs.Cbobshtml ?? "").trim();

            switch (cadenaEstado) {
                case "CONFORME":
                    codigoEstado = CONFORME;
                    break;
                case "OBSERVADO":
                    codigoEstado = OBSERVADO;
                    if (observacionHtml == "") codigoEstado = OBSERVADO_INCOMPLETO;
                    break;
                case "SUBSANADO":
                    codigoEstado = SUBSANADO;
                    if (observacionHtml == "") codigoEstado = SUBSANADO_INCOMPLETO;
                    break;
                case "NO SUBSANADO":
                    codigoEstado = NOSUBSANADO;
                    break;
            }

            var objSeccion = {
                NumSeccion: objItemObs.NumeralSeccion, //devuelve num de la seccion
                ValorObs: observacionHtml, //devuelve valor (no es necesario)
                Estado: codigoEstado, //devuelve estado  (entero)  // 1:Conforme, 2:Observado, 3:Subsanado y 4:No subsanado
            };

            listaDataSeccion.push(objSeccion);
        }
    }

    return listaDataSeccion;
}

function obtenerDataHandsonArchivo(accion) {
    var listaDataSeccion = [];

    var tieneObs = MODELO_DOCUMENTO.IncluirObservacion;
    var filaIni = tieneObs ? 2 : 1;

    //actualizar lista de archivo
    var colBtnEliminar = 0;
    var colNombreArchivo = colBtnEliminar + 1;
    var colConf = colNombreArchivo + 1;
    var colObsCOES = colConf + 1;
    var colSubsGen = colObsCOES + 1;
    var colRptaCOES = colSubsGen + 1;
    var colEstado = colRptaCOES + 1;

    if (tieneObs) {
        var posColObsHtml = OBSERVAR == accion ? 0 : 2;
        var objArchivo = MODELO_DOCUMENTO.SeccionCombustible.ListaObs[posColObsHtml];
        var estado = HOT_SUSTENTO.getData()[0 + filaIni][colEstado];
        objArchivo.Cbevdavalor = estado;

        var codigoEstado = SIN_ESTADO_SELEC;

        var cadenaEstado = (estado ?? "").toUpperCase();
        var observacionHtml = (objArchivo.Cbobshtml ?? "").trim();
        switch (cadenaEstado) {
            case "CONFORME":
                codigoEstado = CONFORME;
                break;
            case "CONFORME PARCIALMENTE":
                codigoEstado = CONFORME_PARCIALMENTE;
                break;
            case "OBSERVADO":
                codigoEstado = OBSERVADO;
                if (observacionHtml == "") codigoEstado = OBSERVADO_INCOMPLETO;
                break;
            case "SUBSANADO":
                codigoEstado = SUBSANADO;
                if (observacionHtml == "") codigoEstado = SUBSANADO_INCOMPLETO;
                break;
            case "NO SUBSANADO":
                codigoEstado = NOSUBSANADO;
                break;
        }

        var objSeccion = {
            Archivo: objArchivo.Cbarchnombreenvio, //devuelve num de la seccion
            ValorObs: observacionHtml, //devuelve valor (no es necesario)
            Estado: codigoEstado, //devuelve estado  (entero)  // 1:Conforme, 2:Observado, 3:Subsanado y 4:No subsanado
        };

        listaDataSeccion.push(objSeccion);
    }

    return listaDataSeccion;
}

//
function validarTabFormato3(lstDatosCentrales, accion) {
    var listaMsj = [];

    for (var i = 0; i < lstDatosCentrales.length; i++) {

        var numSeccionXcentral = 0;

        var numSinEstado = 0;
        var numConformes = 0;
        var numObservados = 0;
        var numObservadosIncompleto = 0;
        var numSubsanados = 0;
        var numSubsanadosIncompleto = 0;
        var numNoSubsanados = 0;
        var numNoSubsanadosIncompleto = 0;

        var centralX = lstDatosCentrales[i];
        var lstSeccionesF3 = centralX.DataFormularioF3;

        for (var y = 0; y < lstSeccionesF3.length; y++) {
            var seccionF3 = lstSeccionesF3[y];
            var estadoSeccion = seccionF3.Estado; // 1:Conforme, 2:Observado, 3:Subsanado y 4:No subsanado

            numSeccionXcentral++;

            if (estadoSeccion == 0)
                numSinEstado++;
            if (estadoSeccion == CONFORME)
                numConformes++;
            if (estadoSeccion == OBSERVADO)
                numObservados++;
            if (estadoSeccion == OBSERVADO_INCOMPLETO)
                numObservadosIncompleto++;
            if (estadoSeccion == SUBSANADO)
                numSubsanados++;
            if (estadoSeccion == SUBSANADO_INCOMPLETO)
                numSubsanadosIncompleto++;
            if (estadoSeccion == NOSUBSANADO)
                numNoSubsanados++;
            if (estadoSeccion == NOSUBSANADO_INCOMPLETO)
                numNoSubsanadosIncompleto++;
        }

        centralX.NumSinEstado = numSinEstado;
        centralX.NumSeccionTotales = numSeccionXcentral;
        centralX.NumConformes = numConformes;
        centralX.NumObservados = numObservados;
        centralX.NumObservadosIncompleto = numObservadosIncompleto;
        centralX.NumSubsanados = numSubsanados;
        centralX.NumSubsanadosIncompleto = numSubsanadosIncompleto;
        centralX.NumNoSubsanados = numNoSubsanados;
        centralX.NumNoSubsanadosIncompleto = numNoSubsanadosIncompleto;

        if (OBSERVAR == accion) {
            if (centralX.NumSinEstado > 0) listaMsj.push("Central " + centralX.NombCentral + ": Existen secciones sin seleccionar estado.");
            if (centralX.NumObservadosIncompleto > 0) listaMsj.push("Central " + centralX.NombCentral + ": Existen secciones con estado OBSERVADO sin detalle de observación.");
        }
        if (APROBAR_PARCIALMENTE == accion) {
            if (centralX.NumSinEstado > 0) listaMsj.push("Central " + centralX.NombCentral + ": Existen secciones sin seleccionar estado.");
            if (centralX.NumNoSubsanadosIncompleto > 0) listaMsj.push("Central " + centralX.NombCentral + ": Existen secciones con estado NO SUBSANADO sin detalle de respuesta.");
        }
    }

    return listaMsj.join('<br/>');
}

function validarTabSustento(lstSeccionesArchivos, accion) {
    var listaMsj = [];

    var numSinEstado = 0;
    var numConformes = 0;
    var numConformesParcialmente = 0;
    var numObservados = 0;
    var numObservadosIncompleto = 0;
    var numSubsanados = 0;
    var numSubsanadosIncompleto = 0;
    var numNoSubsanados = 0;
    var numNoSubsanadosIncompleto = 0;

    for (var z = 0; z < lstSeccionesArchivos.length; z++) {
        var seccionA = lstSeccionesArchivos[z];
        var estadoSeccion = seccionA.Estado; // 1:Conforme, 2:Observado, 3:Subsanado y 4:No subsanado

        if (estadoSeccion == SIN_ESTADO_SELEC)
            numSinEstado++;
        if (estadoSeccion == CONFORME)
            numConformes++;
        if (estadoSeccion == CONFORME_PARCIALMENTE)
            numConformesParcialmente++;
        if (estadoSeccion == OBSERVADO)
            numObservados++;
        if (estadoSeccion == OBSERVADO_INCOMPLETO)
            numObservadosIncompleto++;
        if (estadoSeccion == SUBSANADO)
            numSubsanados++;
        if (estadoSeccion == SUBSANADO_INCOMPLETO)
            numSubsanadosIncompleto++;
        if (estadoSeccion == NOSUBSANADO)
            numNoSubsanados++;
        if (estadoSeccion == NOSUBSANADO_INCOMPLETO)
            numNoSubsanadosIncompleto++;
    }

    if (OBSERVAR == accion) {
        if (numSinEstado > 0) listaMsj.push("Existen archivos sin seleccionar estado.");
        if (numObservadosIncompleto > 0) listaMsj.push("Existen archivos con estado OBSERVADO sin detalle de observación.");
    }
    if (APROBAR == accion) {
        if (numConformes < 1) listaMsj.push("Existen archivos sin estado CONFORME.");
    }
    if (APROBAR_PARCIALMENTE == accion) {
        if (numSinEstado > 0) listaMsj.push("Existen archivos sin seleccionar estado.");
        if (numNoSubsanadosIncompleto > 0) listaMsj.push("Existen archivos con estado NO SUBSANADO sin detalle de respuesta.");
    }

    return listaMsj.join('<br>');
}

//
function obtenerNumeroCentralesEnFormato3(infoHandson) {
    $("#cbNumCentralesTotalesF3").val(infoHandson.length);

    var numCentralesEnF3 = parseInt($("#cbNumCentralesTotalesF3").val()) || -1;
    return numCentralesEnF3;
}

function obtenerStrListaCentralesAprobadas(infoHandson) { //todas las secciones en CONFORME
    var strCentralesAprobadas = "";
    var lstAprobadasNombres = [];
    var lstAprobadasEquicodis = [];
    for (var i = 0; i < infoHandson.length; i++) {
        var esAprobada = true;
        var infoCentralX = infoHandson[i];

        var numSeccionesTotalesXCentral = infoCentralX.NumSeccionTotales;
        var numSeccionesAprobadasXCentral = infoCentralX.NumConformes;

        if (numSeccionesAprobadasXCentral == numSeccionesTotalesXCentral) {
            esAprobada = true;

            lstAprobadasNombres.push(infoCentralX.NombCentral);
            lstAprobadasEquicodis.push(infoCentralX.Equicodi);
        } else {
            esAprobada = false;
        }
    }
    strCentralesAprobadas = lstAprobadasNombres.join(',');
    $("#cbEquicodisCentralesAprobadasF3").val(lstAprobadasEquicodis.join(','));


    return strCentralesAprobadas.trim();
}

function obtenerStrListaCentralesDesaprobadas(infoHandson) {
    var strCentralesDesaprobadas = "";
    var lstDesaprobadas = [];
    var lstDesaprobadasEquicodis = [];
    var estadoEnvio = parseInt($("#hdIdEstado").val()) || 0;

    for (var i = 0; i < infoHandson.length; i++) {
        var esDesaprobada = true;
        var infoCentralX = infoHandson[i];

        //Existentes no pueden desaprobarse desde Solicitud
        if (infoCentralX.Tipo == 'E') {

            var numSeccionesTotalesXCentral = infoCentralX.NumSeccionTotales;
            var numSeccionesNoSubsanadosXCentral = infoCentralX.NumNoSubsanados;

            if (numSeccionesTotalesXCentral > 0) {
                if (numSeccionesNoSubsanadosXCentral > 0) {
                    esDesaprobada = true;
                    lstDesaprobadas.push(infoCentralX.NombCentral);
                    lstDesaprobadasEquicodis.push(infoCentralX.Equicodi);
                } else {
                    esDesaprobada = false;
                }
            }
        }

        //Las nuevas pueden desaprobarse desde solicitud
        if (infoCentralX.Tipo == 'N') {

            if (estadoEnvio == ESTADO_SOLICITADO) {
                var numSeccionesTotalesXCentral = infoCentralX.NumSeccionTotales;
                var numSeccionesObservadosXCentral = infoCentralX.NumObservados;

                if (numSeccionesTotalesXCentral > 0) {
                    if (numSeccionesObservadosXCentral > 0) {
                        esDesaprobada = true;
                        lstDesaprobadas.push(infoCentralX.NombCentral);
                        lstDesaprobadasEquicodis.push(infoCentralX.Equicodi);
                    } else {
                        esDesaprobada = false;
                    }
                }
            }

            if (estadoEnvio == ESTADO_SUBSANADO) {
                var numSeccionesTotalesXCentral = infoCentralX.NumSeccionTotales;
                var numSeccionesNoSubsanadosXCentral = infoCentralX.NumNoSubsanados;

                if (numSeccionesTotalesXCentral > 0) {
                    if (numSeccionesNoSubsanadosXCentral > 0) {
                        esDesaprobada = true;
                        lstDesaprobadas.push(infoCentralX.NombCentral);
                        lstDesaprobadasEquicodis.push(infoCentralX.Equicodi);
                    } else {
                        esDesaprobada = false;
                    }
                }
            }
        }
    }

    strCentralesDesaprobadas = lstDesaprobadas.join(',');
    $("#cbEquicodisCentralesDesaprobadasF3").val(lstDesaprobadasEquicodis.join(','));


    return strCentralesDesaprobadas.trim();
}

function obtenerNumeroCentralesAprobadasEnFormato3(infoHandson) {
    var lstStrCentralesAprobadas = obtenerStrListaCentralesAprobadas(infoHandson);
    var numC = 0;
    if (lstStrCentralesAprobadas.trim() != "") {
        const arraylstCentralesAprobadas = lstStrCentralesAprobadas.split(",");
        numC = arraylstCentralesAprobadas.length;
    }

    return numC;
}

function obtenerNumeroCentralesDesaprobadasEnFormato3(infoHandson) {
    var lstStrCentralesDesaprobadas = obtenerStrListaCentralesDesaprobadas(infoHandson);
    var numC = 0;
    if (lstStrCentralesDesaprobadas.trim() != "") {
        const arraylstCentralesDesaprobadas = lstStrCentralesDesaprobadas.split(",");
        numC = arraylstCentralesDesaprobadas.length;
    }

    return numC;
}

function obtenerNumeroCentralesEnRevisionEnFormato3(infoHandson) {
    var numCentralesEnF3 = obtenerNumeroCentralesEnFormato3(infoHandson);
    var numCentralesAprob = obtenerNumeroCentralesAprobadasEnFormato3(infoHandson);
    var numCentralesDesaprob = obtenerNumeroCentralesDesaprobadasEnFormato3(infoHandson);

    const numCentralesEnRevision = numCentralesEnF3 - numCentralesAprob - numCentralesDesaprob;

    return numCentralesEnRevision;
}

function obtenerNumeroCentralesObservadasEnHandson(infoHandson) {
    var lstStrCentralesObservadas = obtenerStrListaCentralesObservadas(infoHandson);
    var numC = 0;
    if (lstStrCentralesObservadas.trim() != "") {
        const arraylstCentralesObservadas = lstStrCentralesObservadas.split(",");
        numC = arraylstCentralesObservadas.length;
    }

    return numC;
}


function obtenerStrListaCentralesObservadas(infoHandson) { //todas las secciones en CONFORME
    var strCentralesObservadas = "";
    var lstObservadas = [];
    for (var i = 0; i < infoHandson.length; i++) {
        var esObservada = true;
        var infoCentralX = infoHandson[i];

        var numSeccionesTotalesXCentral = infoCentralX.NumSeccionTotales;
        var numSeccionesObservadasXCentral = infoCentralX.NumObservados;

        if (numSeccionesObservadasXCentral > 0) {
            esObservada = true;

            lstObservadas.push(infoCentralX.NombCentral);
        } else {
            esObservada = false;
        }
    }
    strCentralesObservadas = lstObservadas.join(',');

    return strCentralesObservadas.trim();
}

function guardarFechaSistema(cbenvcodi) {
    limpiarBarraMensaje("mensaje");
    var fecSistema = $("#horaSistema").val();
    if (fecSistema != "") {
        if (confirm('¿Desea guardar la fecha del sistema para el envío?')) {
            var data = {
                idEnvio: cbenvcodi,
                fechaSistema: fecSistema
            };
            $.ajax({
                type: 'POST',
                url: controlador + 'GuardarFechaSistema',
                contentType: "application/json",
                dataType: 'json',
                data: JSON.stringify(data),
                cache: false,
                success: function (evt) {
                    if (evt.Resultado != "-1") {
                        mostrarMensaje('mensaje', 'exito', "Fecha del sistema para el envío guardado de manera correcta.");
                        window.location.reload();

                    } else {
                        mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error: ' + evt.Mensaje);

                    }
                },
                error: function (err) {
                    mostrarMensaje('mensaje', 'error', 'Ha ocurrido un error.');
                }
            });
        }
    } else {
        mostrarMensaje('mensaje', 'error', "ingresa una fecha del sistema correcto.");
    }
}

function cerrarPopup(id) {
    $("#" + id).bPopup().close()
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


function mostrarMensaje(id, tipo, mensaje) {
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

function diaActual() { //devuelve strFecha en formato dd/mm/yyyy
    var now = new Date();
    var strDateTime = [[AddZero(now.getDate()), AddZero(now.getMonth() + 1), now.getFullYear()].join("/")].join(" ");

    return strDateTime;
}

function AddZero(num) {
    return (num >= 0 && num < 10) ? "0" + num : num + "";
}

function validarCorreo(cadena, minimo, maximo) {
    var arreglo = cadena.split(';');
    var nroCorreo = 0;
    var longitud = arreglo.length;

    if (longitud == 0) {
        arreglo = cadena;
        longitud = 1;
    }

    for (var i = 0; i < longitud; i++) {

        var email = arreglo[i].trim();
        var validacion = validarDirecccionCorreo(email);

        if (validacion) {
            nroCorreo++;
        } else {
            if (email != "")
                return -1;
        }
    }

    if (minimo > nroCorreo)
        return -1;

    if (maximo > 0 && nroCorreo > maximo)
        return -1;

    return 1;
}

function validarDirecccionCorreo(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}
