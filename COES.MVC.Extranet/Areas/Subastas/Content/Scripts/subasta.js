var strControlador = siteRoot + 'subastas/subasta/';
var TIPO_OFERTA_DEFECTO = 0;
var TIPO_OFERTA_DIARIO = 1;

var TIPO_ACCION_CONSULTAR = 0;
var TIPO_ACCION_CREAR_NUEVO = 1;
var TIPO_ACCION_EXITO_GRABAR = 2;
var TIPO_ACCION_VER_ENVIO = 3;
var TIPO_ACCION_VER_COPIAR_ENVIO = 4;
var NUM_HOJAS_VACIAS = 0;

$(document).ready(function () {

    //$('#tbs-subasta').easytabs();
    $('#tab-container').easytabs({
        animate: false
    });

    // Disabled Button Enviar, Errors, and Copiar

    //btn seleccionar
    var formatoFecha = TIPO_OFERTA_GLOBAL == TIPO_OFERTA_DIARIO ? 'd/m/Y' : 'm Y';
    $('#dte-subasta-fecha').Zebra_DatePicker({
        format: formatoFecha,
        onSelect: function () {
            btnSeleccionar(TIPO_ACCION_CONSULTAR);
        }
    });

    //btn crear nuevo
    $('#crear-nuevo').click(function () {
        btnCrearNuevoInicializar();
    });
    $('#fecha-inicio-select').Zebra_DatePicker({});
    $('#fecha-fin-select').Zebra_DatePicker({});

    //btn Enviar
    $('#btn-subasta-enviar').click(function () {
        NUM_HOJAS_VACIAS = 0;
        if (isEnIntervaloTemporal == 0)
            btnEnviar();
        else
            btnEnviarTemporal();
    });

    //btn Errores
    $('#btn-subasta-errors').click(function () {
        NUM_HOJAS_VACIAS = 0;
        limpiarError();

        validarOfertasInternal(TIPO_OFERTA_GLOBAL, hstSubastaGlobalSubir, hojaSubir);
        //hstSubastaGlobalSubir.render();
        validarOfertasInternal(TIPO_OFERTA_GLOBAL, hstSubastaGlobalBajar, hojaBajar);
        //hstSubastaGlobalBajar.render();
        mostrarErrores();
    });

    //btn Envios
    $('#btn-subasta-envios').click(function () {
        btnVerEnvios();
    });

    //btn Copiar   
    $('#btn-subasta-copiar').click(function () {
        btnCopiarInicializar();
    });

    //btn Duplicar
    $('#btn-subasta-duplicar').click(function () {
        var val = confirm("¿Esta seguro que desea copiar la información de la OFERTA SUBIR a la OFERTA BAJAR?");
        if (val == true) {
            $("#tituloTabBajar").click()

            setTimeout(function () {
                duplicarHandsonTable();
            }, 250);
        }


    });

    //btn Manttos
    if (TIPO_OFERTA_GLOBAL == TIPO_OFERTA_DIARIO) {
        $('#tbs-subasta').easytabs();
        $("#btn-ocultar-subasta-mantenimiento").parent().css("display", "none");
        $("#btn-ver-subasta-mantenimiento").parent().css("display", "table-cell");

        $('#btn-ver-subasta-mantenimiento').click(function () {
            $("#pnl-subasta-mantenimiento").show();
            $("#btn-ocultar-subasta-mantenimiento").parent().css("display", "table-cell");
            $("#btn-ver-subasta-mantenimiento").parent().css("display", "none");

            $('#tbs-subasta').easytabs({
                animate: false
            });

            mostrarMantenimiento($('#dte-subasta-fecha').val());
        });
        $('#btn-ocultar-subasta-mantenimiento').click(function () {
            $("#pnl-subasta-mantenimiento").hide();
            $("#btn-ocultar-subasta-mantenimiento").parent().css("display", "none");
            $("#btn-ver-subasta-mantenimiento").parent().css("display", "table-cell");

        });
    }

    //btn Nota
    $('#btn-mostrar-nota').click(function () {
        btnMostrarNotaImportante();
    });
    $('#btn-cancelar-mensaje-nota-importante').click(function () {
        popupNotaImportante.close();
    });

    //
    $('#btn-cancelar-mensaje-traslape').click(function () {
        popupTraslape.close();
    });

    $('#btn-cancelar-mensaje-magnitud').click(function () {
        popupMagnitudes.close();
    });

    $('#btn-aceptar-mensaje').click(function () {
        btnConfirmarEnviar();
    });

    $('#btn-cancelar-mensaje').click(function () {
        popup.close();
    });

    btnSeleccionar(TIPO_ACCION_CONSULTAR);
})

function openManual() {
    window.location = strControlador + 'MostrarManualSubastas';
};

function btnMostrarNotaImportante() {
    $('#mensajeNota').html(`
        
        <ul style="font-size: 11px">
            <li>El modelo Yupana calcula la potencia de Reserva Secundaria de Frecuencia (RSF) y Reserva Primaria de Frecuencia (RPF) de los generadores de tal forma que la suma de la RSF y RPF estén dentro de la banda calificada de RSF de los generadores.</li>
            <li>Para mayores detalles acerca del cálculo de la RSF de cada generador que realiza el modelo Yupana, referirse al manual de metodología del modelo Yupana.</li>

        </ul>
    `);
    var t = setTimeout(function () {
        popupNotaImportante = $('#Notificacion-Nota-Importante').bPopup({ modalclose: true, escclose: false });

        clearTimeout(t);
    }, 100);
}

// #region btnSeleccionar

async function btnSeleccionar(accion, inCodigoEnvio) {
    var fechaOferta = '';
    var codigoEnvio = -1;
    var estadoEnvio = 'A';
    $("#tituloTabSubir").click();
    $("#mensajeValOfDiarioYSuOfDefecto").hide();
    var flagContinuarMostrarOferta = true;

    switch (accion) {
        case TIPO_ACCION_CREAR_NUEVO:
            fechaOferta = $('#dte-fecha-inicio').val();

            //validar que las URS calificadas tengan oferta por defecto
            flagContinuarMostrarOferta = await verificarOfertaDiariaYSuOfDefecto();

            break;
        case TIPO_ACCION_CONSULTAR:
            $('#fecha-inicio-select').val($('#dte-subasta-fecha').val());
            $('#fecha-fin-select').val($('#dte-subasta-fecha').val());
            $('#txt-num-dias').val(1);
            fechaOferta = $('#dte-subasta-fecha').val();

            break;
        case TIPO_ACCION_EXITO_GRABAR:
            fechaOferta = $('#dte-subasta-fecha').val();
            break;

        case TIPO_ACCION_VER_ENVIO:
            fechaOferta = null;
            codigoEnvio = parseInt(inCodigoEnvio);
            break;

        case TIPO_ACCION_VER_COPIAR_ENVIO:
            fechaOferta = $('#dte-subasta-fecha').val();
            codigoEnvio = parseInt(inCodigoEnvio);

            break;
    }

    if (flagContinuarMostrarOferta) {
        mostrarOfertas(accion, TIPO_OFERTA_GLOBAL, fechaOferta, codigoEnvio, estadoEnvio);
    }
}

function mostrarOfertas(accion, tipo, fechaOferta, codigo, estado) {
    habilitarMenu(false);
    $('#mensajeRegistro').hide();
    $('#mensajeRegistro').html('');
    $("#mensajeCierreOferta").hide();
    $('#fechas-nuevo').hide();
    clearTiempoRestante();
    if (TIPO_OFERTA_GLOBAL == TIPO_OFERTA_DIARIO) {
        $("#pnl-subasta-mantenimiento").hide();
        $("#btn-ocultar-subasta-mantenimiento").parent().css("display", "none");
        $("#btn-ver-subasta-mantenimiento").parent().css("display", "table-cell");
    }

    $.ajax({
        type: 'POST',
        url: strControlador + "OfertaListar",
        data: {
            accion: accion,
            tipo: tipo,
            strfechaoferta: fechaOferta,
            codigo: codigo,
            estado: estado
        },
        success: function (model) {

            if (model.Resultado != -1) {
                //
                var excelWebHabilitado = true;//model.ObjParametros.TieneExcelWebHabilitado;
                var lectura = true; //se habilita cuando se hace clic en "Crear Nuevo"
                var agregarFila = 0;
                var actualizarView = false;

                switch (accion) {
                    case TIPO_ACCION_CREAR_NUEVO:
                        if (excelWebHabilitado) {
                            lectura = false;
                            agregarFila = 1;
                            actualizarView = true;

                            $('#dte-subasta-fecha').val($('#dte-fecha-inicio').val());

                            $('#fecha-inicio-select').val($('#dte-fecha-inicio').val());
                            $('#fecha-fin-select').val($('#dte-fecha-fin').val());
                            $('#txt-num-dias').val($('#txt-numero-dias').val());

                            //
                            habilitarMenu(true); //se habilita cuando se hace clic en "Crear Nuevo"
                            $('#fechas-nuevo').show();
                            $('#fechas-seleccion').hide();


                            $('#div-nuevo-popup').bPopup().close();
                        } else {
                            alert("Existen día cerrados para las ofertas. Por favor seleccionar otra fecha.");
                        }

                        break;
                    case TIPO_ACCION_CONSULTAR:
                        actualizarView = true;
                        habilitarMenu(false); //se habilita cuando se hace clic en "Crear Nuevo"
                        $('#fechas-seleccion').show();
                        $('#fechas-nuevo').hide();

                        break;

                    case TIPO_ACCION_EXITO_GRABAR:
                        actualizarView = true;
                        habilitarMenu(false); //se habilita cuando se hace clic en "Crear Nuevo"
                        $('#fechas-seleccion').show();
                        $('#fechas-nuevo').hide();

                        $('#mensajeRegistro').show();
                        mostrarMensaje('mensajeRegistro', 'message', '<span>Registro Satisfactorio</span>');
                        break;

                    case TIPO_ACCION_VER_ENVIO:
                        actualizarView = true;
                        $('#dte-subasta-fecha').val(model.FechaOferta);
                        $('#fechas-seleccion').show();

                        $("#mensajeRegistro").show();
                        var estadoHtml = "<strong style='" + (model.OferEstado == "H" ? 'color:gray' : 'color:green') + "'>" + (model.OferEstado == "H" ? 'Histórico' : 'Activo') + "</strong>";
                        var htmlEnvio = "<strong>Código de envío</strong>: " + model.OferCodenvio
                            + ", <strong>Fecha de envío: </strong>" + model.OferfechaenvioDesc
                            + ", <strong>Estado de envío: </strong>" + estadoHtml;
                        mostrarMensaje('mensajeRegistro', 'message', htmlEnvio);
                        break;

                    case TIPO_ACCION_VER_COPIAR_ENVIO:
                        if (excelWebHabilitado) {
                            lectura = false;
                            agregarFila = 1;
                            actualizarView = true;

                            //
                            habilitarMenu(true); //se habilita cuando se hace clic en "Crear Nuevo"
                            $('#fechas-nuevo').show();
                            $('#fechas-seleccion').hide();

                            $("#mensajeRegistro").show();
                            var htmlEnvio = "Selección de envío a copiar correcto. " + "<strong>Código de envío</strong>: " + model.OferCodenvio
                                + ", <strong>Fecha de envío: </strong>" + model.OferfechaenvioDesc;

                            mostrarMensaje('mensajeRegistro', 'message', htmlEnvio);
                        } else {
                            alert("Existen día cerrados para las ofertas. Por favor seleccionar otra fecha.");
                        }

                        break;
                }

                if (actualizarView) {
                    $("#tab-container").show();

                    if (!model.ObjParametros.TieneOfertaPorDefecto && TIPO_OFERTA_GLOBAL == TIPO_OFERTA_DIARIO) {
                        habilitarMenu(false);
                        lectura = false;

                        $("#tab-container").hide();
                        $('#mensajeRegistro').show();
                        mostrarMensaje('mensajeRegistro', 'alert', '<span>Para registrar Mercado de Ajuste Diario primero debe registrar Oferta por Defecto.</span>');
                    }

                    cargarHandson(accion, model, lectura, agregarFila);

                    if (excelWebHabilitado) {
                        mostrarMensaje('mensajeCierreOferta', '1', '');
                        $("#mensajeCierreOferta").show();
                        tiempoRestante();
                    } else {
                        if (!excelWebHabilitado) {
                            $("#mensajeCierreOferta").show();
                            if (TIPO_OFERTA_GLOBAL == TIPO_OFERTA_DIARIO)
                                mostrarMensaje('mensajeCierreOferta', 'alert', '<b>Cerrado</b> las ofertas para el día ' + model.FechaOferta);
                            else
                                mostrarMensaje('mensajeCierreOferta', 'alert', '<b>Cerrado</b>');
                        }
                    }
                }

            } else {
                $('#mensajeRegistro').show();
                mostrarMensaje('mensajeRegistro', 'alert', model.Mensaje);
            }
        },
        error: function (req, status, error) {
            alert("Ha ocurrido un error");
            console.log(error);
        }
    });
}

function cargarHandson(accion, model, lectura, indicadorRegistrar) {
    //cargar la lista urs en la acción nuevo
    if (accion != TIPO_ACCION_VER_COPIAR_ENVIO) {
        cargarParametrosGlobal(model.ObjParametros);
    }

    //
    var arrOfferSubir = [];
    var arrOfferBajar = [];

    for (var tipoCarga = 1; tipoCarga <= 2; tipoCarga++) {
        var dataHanson = [];
        var arrOfertas = tipoCarga == 1 ? model.ListaTabSubir : model.ListaTabBajar;

        if (arrOfertas != null && arrOfertas.length > 0) {
            for (var i = 0, l = arrOfertas.length; i < l; i++) {
                var arrOferta = arrOfertas[i];

                //si la data es de solo lectura, mostrar tal como está en BD
                if (lectura) {
                    dataHanson.push([arrOferta.URS, arrOferta.HoraInicio, arrOferta.HoraFin, arrOferta.PotenciaOfertada, arrOferta.Precio, arrOferta.Grupocodi, arrOferta.BandaCalificada, '', arrOferta.Indice, arrOferta.Cantidad]);
                } else {
                    //si la data es para editar, solo considerar las URS y modo de operación vigente del periodo a cargar
                    var objModo = ARR_MODO_OPERACION_GLOBAL.find(x => x.UrsID == arrOferta.URS && x.ID == arrOferta.Grupocodi);
                    if (objModo !== undefined) {

                        var colDataUrsBandaCalificada = objModo.BndCalificada; //arrOferta.BandaCalificada

                        dataHanson.push([arrOferta.URS, arrOferta.HoraInicio, arrOferta.HoraFin, arrOferta.PotenciaOfertada, arrOferta.Precio, arrOferta.Grupocodi, colDataUrsBandaCalificada, '', arrOferta.Indice, arrOferta.Cantidad]);
                    }
                }
            }
        }

        if (tipoCarga == 1)
            arrOfferSubir = dataHanson;
        if (tipoCarga == 2)
            arrOfferBajar = dataHanson;
    }

    mostrarOfertasInternalSubir(TIPO_OFERTA_GLOBAL, ARR_URS_GLOBAL, ARR_MODO_OPERACION_GLOBAL, ARR_HORAS_GLOBAL, arrOfferSubir, lectura, indicadorRegistrar);

    if (isEnIntervaloTemporal == 0)
        mostrarOfertasInternalBajar(TIPO_OFERTA_GLOBAL, ARR_URS_GLOBAL, ARR_MODO_OPERACION_GLOBAL, ARR_HORAS_GLOBAL, arrOfferBajar, lectura, indicadorRegistrar);

    //
    updateDimensionHandson('hst-subasta-ingreso-subir', hstSubastaGlobalSubir);

    if (isEnIntervaloTemporal == 0)
        updateDimensionHandson('hst-subasta-ingreso-bajar', hstSubastaGlobalBajar);

}

function cargarParametrosGlobal(objParametros) {
    ARR_URS_GLOBAL = [];
    ARR_MODO_OPERACION_GLOBAL = [];

    precioMaximo = objParametros.PrecioMaximo;
    precioMinimo = objParametros.PrecioMinimo;
    tieneOfertaPorDefecto = objParametros.TieneOfertaPorDefecto;
    potenciaMinimoMan = objParametros.PotenciaMinimaMan;

    tmeHoraActual = objParametros.HoraActual;
    tmeHoraInicio = objParametros.HoraInicioParaOfertar;
    tmeHoraFin = objParametros.HoraFinParaOfertar;

    if (objParametros.TieneOfertaPorDefecto || TIPO_OFERTA_GLOBAL == TIPO_OFERTA_DEFECTO) {

        var arrURS = objParametros.URSs;

        if (arrURS != null && arrURS.length > 0) {
            for (var i = 0, j = arrURS.length; i < j; i++) {
                var objUrs = arrURS[i];

                if (objUrs != null) {
                    ARR_URS_GLOBAL.push({ ID: objUrs.ID, Text: objUrs.Text });

                    var arrModoOperacion = objUrs.OperationModes;

                    if (arrModoOperacion != null && arrModoOperacion.length > 0) {
                        for (var m = 0, n = arrModoOperacion.length; m < n; m++) {
                            var objModoOperacion = arrModoOperacion[m];

                            if (objModoOperacion != null) {
                                ARR_MODO_OPERACION_GLOBAL.push({
                                    ID: objModoOperacion.ID,
                                    UrsID: objUrs.ID,
                                    Text: objModoOperacion.Text,
                                    Pot: objModoOperacion.Pot,
                                    IntvMant: objModoOperacion.IntvMant,
                                    Indice: objModoOperacion.Indice,
                                    Cantidad: objModoOperacion.Cantidad,
                                    EsReservaFirme: objModoOperacion.EsReservaFirme,
                                    //BndDisponible: objModoOperacion.BandaDisponible,
                                    BndCalificada: objModoOperacion.BandaCalificada,
                                    BndAdjudicada: objModoOperacion.BandaAdjudicada,
                                });
                            }
                        }
                    }
                }
            }
        }
    }
}

function tiempoRestante() {
    tmeTimeElapsed = tmeHoraActual;

    sitOferta = setInterval(function () {
        msec = tmeHoraFin - tmeTimeElapsed;
        if (msec <= 0) {
            $("#mensajeCierreOferta").hide();
            clearTiempoRestante();
        } else {
            msec = tmeHoraFin - tmeTimeElapsed;

            var dd = Math.floor(msec / 1000 / 60 / 60 / 24);
            msec -= dd * 1000 * 60 * 60 * 24;
            dd = (dd > 0 ? ((dd < 10 ? "0" : "") + dd) : ((dd > -10 ? "0" : "") + (-1 * dd)));

            var hh = Math.floor(msec / 1000 / 60 / 60);
            msec -= hh * 1000 * 60 * 60;
            //hh = (hh < 10 ? "0" : "") + hh;
            hh = (hh > 0 ? ((hh < 10 ? "0" : "") + hh) : ((hh > -10 ? "0" : "") + (-1 * hh)));

            var mm = Math.floor(msec / 1000 / 60);
            msec -= mm * 1000 * 60;
            //mm = (mm < 10 ? "0" : "") + mm;
            mm = (mm > 0 ? ((mm < 10 ? "0" : "") + mm) : ((mm > -10 ? "0" : "") + (-1 * mm)));

            var ss = Math.floor(msec / 1000);
            msec -= ss * 1000;
            //ss = (ss < 10 ? "0" : "") + ss;
            ss = (ss > 0 ? ((ss < 10 ? "0" : "") + ss) : ((ss > -10 ? "0" : "") + (-1 * ss)));


            $('#lbl-time-elapsed').html(hh + ':' + mm + ':' + ss);

            tmeTimeElapsed += 1000;
            tmeHoraActual += 1000;

            //Mostramos mensaje de tiempo restante              
            mostrarMensaje('mensajeCierreOferta', '1', '<b>Abierto</b> (Tiempo Restante: ' + (dd != '00' ? dd + 'd ' : '') + hh + ':' + mm + ':' + ss + ')');

        }
    }, 1000);
}

function clearTiempoRestante() {
    //detener evento set interval
    clearInterval(sitOferta);
    $("#mensajeCierreOferta").hide();
}

function habilitarMenu(flag) {
    if (flag) {
        $("#btn-subasta-enviar").parent().css("display", "table-cell");
        $("#btn-subasta-errors").parent().css("display", "table-cell");
        $("#btn-subasta-copiar").parent().css("display", "table-cell");
        $("#btn-subasta-duplicar").parent().css("display", "table-cell");
    } else {
        $("#btn-subasta-enviar").parent().css("display", "none");
        $("#btn-subasta-errors").parent().css("display", "none");
        $("#btn-subasta-copiar").parent().css("display", "none");
        $("#btn-subasta-duplicar").parent().css("display", "none");
    }
}

async function verificarOfertaDiariaYSuOfDefecto() {
    ES_VALIDO_PERIODO_OF_DIARIA = false;

    if (TIPO_OFERTA_GLOBAL == TIPO_OFERTA_DIARIO) {

        await _validarOfertaDiariaYSuOfDefecto();

        return ES_VALIDO_PERIODO_OF_DIARIA;
    }
    else {
        return true;
    }
}

var ES_VALIDO_PERIODO_OF_DIARIA = false;
async function _validarOfertaDiariaYSuOfDefecto() {

    return $.ajax({
        type: 'POST',
        url: strControlador + "ValidarOfertaDiariaYSuOfDefecto",
        dataType: 'json',
        data: {
            strfechaoferta: $('#dte-fecha-inicio').val(),
            numDia: $('#txt-num-dias').val(),
        },
        success: function (model) {
            if (model.Resultado == 0 || model.Resultado == -1) {
                ES_VALIDO_PERIODO_OF_DIARIA = false;
                var msjVal = model.Mensaje;
                if (model.Resultado == -1) msjVal = "Ha ocurrido un error.";

                $("#mensajeValOfDiarioYSuOfDefecto").show();
                mostrarMensaje('mensajeValOfDiarioYSuOfDefecto', 'error', msjVal);
            } else {
                ES_VALIDO_PERIODO_OF_DIARIA = true;
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

// #endregion

// #region Crear Nuevo

function btnCrearNuevoInicializar() {
    $('#fechas-nuevo').hide();

    var fechaBase = maximoFechaOferta();

    $('#dte-fecha-inicio').val($('#dte-subasta-fecha').val());

    var formatoFecha = TIPO_OFERTA_GLOBAL == TIPO_OFERTA_DIARIO ? 'd/m/Y' : 'm Y';
    $('#dte-fecha-inicio').Zebra_DatePicker({
        format: formatoFecha,
        direction: [1, fechaBase],
        onSelect: function () {
            var sfech = GetFecha($('#dte-fecha-inicio').val(), 1),
                fech2 = (sfech.getDate() < 10 ? '0' + sfech.getDate() : sfech.getDate()) + '/' +
                    (sfech.getMonth() + 1 < 10 ? '0' + (sfech.getMonth() + 1) : sfech.getMonth() + 1) + '/' +
                    sfech.getFullYear();

            //console.log("fecha", fech2);
            $('#dte-fecha-fin').val(fech2);
            $('#dte-fecha-fin').Zebra_DatePicker({
                format: 'd/m/Y',
                direction: [fech2, fechaBase],
                onSelect: function () {
                    var fechaIni = GetFecha($('#dte-fecha-inicio').val(), 1),
                        fechaFin = GetFecha($('#dte-fecha-fin').val(), 1),
                        diferencia = Math.floor((fechaFin - fechaIni) / 86400000) + 1;

                    //console.log('diferencia1', diferencia);
                    $('#txt-numero-dias').val(diferencia);
                }
            });
            var fechaIni = GetFecha($('#dte-fecha-inicio').val(), 1),
                fechaFin = GetFecha($('#dte-fecha-fin').val(), 1),
                diferencia = Math.floor((fechaFin - fechaIni) / 86400000) + 1;

            //console.log('diferencia2', diferencia);
            $('#txt-numero-dias').val(diferencia);
        }
    });

    $('#dte-fecha-fin').val($('#dte-subasta-fecha').val());
    $('#dte-fecha-fin').Zebra_DatePicker({
        format: 'd/m/Y',
        direction: [1, fechaBase],
        onSelect: function () {
            var fechaIni = GetFecha($('#dte-fecha-inicio').val(), 1),
                fechaFin = GetFecha($('#dte-fecha-fin').val(), 1),
                diferencia = Math.floor((fechaFin - fechaIni) / 86400000) + 1;

            //console.log('diferencia1', diferencia);
            $('#txt-numero-dias').val(diferencia);
        }
    });
    $('#txt-numero-dias').val(1);

    $('#btn-subasta-crear-nuevo').unbind();
    $('#btn-subasta-crear-nuevo').click(function () {
        btnSeleccionar(TIPO_ACCION_CREAR_NUEVO);
    });

    var t = setTimeout(function () {
        popup = $('#div-nuevo-popup').bPopup({ modalclose: false, escclose: false });

        clearTimeout(t);
    }, 100);
}

function maximoFechaOferta() {
    var maxNumDias = $('#hdd-maximo-dias').val(),
        sfech = GetFecha($('#hdd-fecha-inicio-oferta').val(), maxNumDias * 1),
        fech = (sfech.getDate() < 10 ? '0' + sfech.getDate() : sfech.getDate()) + '/' +
            (sfech.getMonth() + 1 < 10 ? '0' + (sfech.getMonth() + 1) : sfech.getMonth() + 1) + '/' +
            sfech.getFullYear();

    //console.log("maxNumDias", maxNumDias);
    //console.log("maximoFechaOferta fech2", fech);
    return fech;
}

// #endregion

// #region btn-subasta-enviar

function btnEnviar() {
    limpiarError();

    var blnSatisfactorioSubir = validarOfertasInternal(TIPO_OFERTA_GLOBAL, hstSubastaGlobalSubir, hojaSubir);
    var blnSatisfactorioBajar = validarOfertasInternal(TIPO_OFERTA_GLOBAL, hstSubastaGlobalBajar, hojaBajar);
    var existenErrores = listErrores != null && listErrores.length > 0;

    if (blnSatisfactorioSubir && blnSatisfactorioBajar && !existenErrores) {

        var lstHandson = obtenerDataHansontablesTotal();

        var excelSubir = lstHandson[0];
        var excelBajar = lstHandson[1];

        //console.log("btn-subasta-enviar:excelSubir", excelSubir);
        //console.log("btn-subasta-enviar:excelBajar", excelBajar);

        $.ajax({
            type: 'POST',
            url: strControlador + "ValidarTraslape",
            data: {
                tipo: TIPO_OFERTA_GLOBAL,
                fechaini: $('#fecha-inicio-select').val(),
                codigo: -1,
                estado: 'A',
                numofertas: $('#txt-num-dias').val(),
                ofertasSubir: JSON.stringify(excelSubir),
                ofertasBajar: JSON.stringify(excelBajar)
            },
            success: function (result) {
                console.log('btn-subasta-enviar:ValidarTraslape result:', result);
                //var blnSatisfactorioSubir = validarOfertasInternal(TIPO_OFERTA_GLOBAL, hstSubastaGlobalSubir, hojaSubir);
                //var blnSatisfactorioBajar = validarOfertasInternal(TIPO_OFERTA_GLOBAL, hstSubastaGlobalBajar, hojaBajar);

                hstSubastaGlobalSubir.render();
                hstSubastaGlobalBajar.render();

                console.log('blnSatisfactorioSubir:', blnSatisfactorioSubir);
                console.log('blnSatisfactorioBajar:', blnSatisfactorioBajar);

                //Si no tiene traslapes
                if (result.indice != -1) {

                    $.ajax({
                        type: 'POST',
                        url: strControlador + "ValidarCondicionMagnitudes",
                        data: {
                            tipo: TIPO_OFERTA_GLOBAL,
                            fechaini: $('#fecha-inicio-select').val(),
                            ofertasSubir: JSON.stringify(excelSubir),
                            ofertasBajar: JSON.stringify(excelBajar)
                        },
                        success: function (salida) {
                            if (salida.Resultado != -1) {

                                if (salida.Resultado == 1) {//Cumple la condicion

                                    //verifico si alguno (subir o bajar), ya tiene data almacenada
                                    ////NO EXISTE almacenamiento anterior
                                    if (result.indice == 1) {

                                        grabarHandson(false);
                                    } else {
                                        ////EXISTE data almacenada
                                        if (result.indice == 2 && listErrores.length == 0) {

                                            $('#mensaje').html("Ya existen ofertas en el rango de fechas seleccionado, ¿Desea sobreescribir las ofertas?" + result.mensaje);
                                            var t = setTimeout(function () {
                                                popup = $('#Messages').bPopup({ modalclose: false, escclose: false });

                                                clearTimeout(t);
                                            }, 100);

                                        }
                                    }
                                } else {
                                    if (salida.Resultado == 2) {
                                        $('#mensajeMagnitudes').html(salida.Mensaje);
                                        var t = setTimeout(function () {
                                            popupMagnitudes = $('#Notificacion-Magnitudes').bPopup({ modalclose: false, escclose: false });

                                            clearTimeout(t);
                                        }, 100);
                                    }
                                }

                            } else {
                                alert(salida.Mensaje)
                            }
                        },
                        error: function (req, status, error) {
                            console.log(error);
                        }
                    });



                }
                else  //tiene traslapes
                {
                    //alert(result.mensaje)
                    $('#mensajeTraslapes').html(result.mensaje);
                    var t = setTimeout(function () {
                        popupTraslape = $('#Notificacion-Traslapes').bPopup({ modalclose: false, escclose: false });

                        clearTimeout(t);
                    }, 100);
                }
                console.log("listErrores:", listErrores);


            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    } else {
        mostrarErrores();
    }

}

function btnEnviarTemporal() {
    console.log("temporal");
    limpiarError();

    var blnSatisfactorioSubir = validarOfertasInternalTemporal(TIPO_OFERTA_GLOBAL, hstSubastaGlobalSubir, hojaSubir);
    var existenErrores = listErrores != null && listErrores.length > 0;

    if (blnSatisfactorioSubir && !existenErrores) {

        var lstHandson = obtenerDataHansontablesTotalTemporal();

        var excelSubir = lstHandson[0];

        //console.log("btn-subasta-enviar:excelSubir", excelSubir);
        //console.log("btn-subasta-enviar:excelBajar", excelBajar);

        $.ajax({
            type: 'POST',
            url: strControlador + "ValidarTraslapeTemporal",
            data: {
                tipo: TIPO_OFERTA_GLOBAL,
                fechaini: $('#fecha-inicio-select').val(),
                codigo: -1,
                estado: 'A',
                numofertas: $('#txt-num-dias').val(),
                ofertasSubir: JSON.stringify(excelSubir)
            },
            success: function (result) {
                console.log('btn-subasta-enviar:ValidarTraslape result:', result);
                //var blnSatisfactorioSubir = validarOfertasInternal(TIPO_OFERTA_GLOBAL, hstSubastaGlobalSubir, hojaSubir);
                //var blnSatisfactorioBajar = validarOfertasInternal(TIPO_OFERTA_GLOBAL, hstSubastaGlobalBajar, hojaBajar);

                hstSubastaGlobalSubir.render();
                //hstSubastaGlobalBajar.render();

                console.log('blnSatisfactorioSubir:', blnSatisfactorioSubir);
                //console.log('blnSatisfactorioBajar:', blnSatisfactorioBajar);

                //Si no tiene traslapes
                if (result.indice != -1) {

                    $.ajax({
                        type: 'POST',
                        url: strControlador + "ValidarCondicionMagnitudesTemporal",
                        data: {
                            tipo: TIPO_OFERTA_GLOBAL,
                            fechaini: $('#fecha-inicio-select').val(),
                            ofertasSubir: JSON.stringify(excelSubir)
                        },
                        success: function (salida) {
                            if (salida.Resultado != -1) {

                                if (salida.Resultado == 1) {//Cumple la condicion

                                    //verifico si alguno (subir o bajar), ya tiene data almacenada
                                    ////NO EXISTE almacenamiento anterior
                                    if (result.indice == 1) {

                                        grabarHandsonTemporal(false);
                                    } else {
                                        ////EXISTE data almacenada
                                        if (result.indice == 2 && listErrores.length == 0) {

                                            $('#mensaje').html("Ya existen ofertas en el rango de fechas seleccionado, ¿Desea sobreescribir las ofertas?" + result.mensaje);
                                            var t = setTimeout(function () {
                                                popup = $('#Messages').bPopup({ modalclose: false, escclose: false });

                                                clearTimeout(t);
                                            }, 100);

                                        }
                                    }
                                } else {
                                    if (salida.Resultado == 2) {
                                        $('#mensajeMagnitudes').html(salida.Mensaje);
                                        var t = setTimeout(function () {
                                            popupMagnitudes = $('#Notificacion-Magnitudes').bPopup({ modalclose: false, escclose: false });

                                            clearTimeout(t);
                                        }, 100);
                                    }
                                }

                            } else {
                                alert(salida.Mensaje)
                            }
                        },
                        error: function (req, status, error) {
                            console.log(error);
                        }
                    });



                }
                else  //tiene traslapes
                {
                    //alert(result.mensaje)
                    $('#mensajeTraslapes').html(result.mensaje);
                    var t = setTimeout(function () {
                        popupTraslape = $('#Notificacion-Traslapes').bPopup({ modalclose: false, escclose: false });

                        clearTimeout(t);
                    }, 100);
                }
                console.log("listErrores:", listErrores);


            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    } else {
        mostrarErrores();
    }

}

function btnConfirmarEnviar() {

    if (isEnIntervaloTemporal == 0)
        grabarHandson(true);
    else
        grabarHandsonTemporal(true);
}

function grabarHandson(cerrarPopup) {

    var lstHandson = obtenerDataHansontablesTotal();

    var excelSubir = lstHandson[0];
    var excelBajar = lstHandson[1];

    //console.log("btn-subasta-enviar:excelSubir", excelSubir);
    //console.log("btn-subasta-enviar:excelBajar", excelBajar);

    //console.log("btn-aceptar-mensaje:hstSubastaGlobalSubir.getData()", hstSubastaGlobalSubir.getData());
    //console.log("btn-aceptar-mensaje:hstSubastaGlobalBajar.getData()", hstSubastaGlobalBajar.getData());

    $.ajax({
        type: 'POST',
        url: strControlador + "grabar",
        data: {
            tipo: TIPO_OFERTA_GLOBAL,
            ofertasSubir: JSON.stringify(excelSubir),
            ofertasBajar: JSON.stringify(excelBajar),
            fechaini: $('#fecha-inicio-select').val(),
            numofertas: $('#txt-num-dias').val()
        },
        success: function (model) {
            if (model.Resultado != -1) {
                btnSeleccionar(TIPO_ACCION_EXITO_GRABAR);


            } else {
                $('#mensajeRegistro').show();
                mostrarMensaje('mensajeRegistro', 'alert', model.Mensaje);
            }
            if (cerrarPopup)
                $('#Messages').bPopup().close();
        },
        error: function (req, status, error) {
            $('#mensajeRegistro').show();
            $('#mensajeRegistro').html('El envio tiene un error ' + status + ' - ' + error + '.');
        }
    });
}


function grabarHandsonTemporal(cerrarPopup) {

    var lstHandson = obtenerDataHansontablesTotalTemporal();

    var excelSubir = lstHandson[0];
    //var excelBajar = lstHandson[1];

    //console.log("btn-subasta-enviar:excelSubir", excelSubir);
    //console.log("btn-subasta-enviar:excelBajar", excelBajar);

    //console.log("btn-aceptar-mensaje:hstSubastaGlobalSubir.getData()", hstSubastaGlobalSubir.getData());
    //console.log("btn-aceptar-mensaje:hstSubastaGlobalBajar.getData()", hstSubastaGlobalBajar.getData());

    $.ajax({
        type: 'POST',
        url: strControlador + "GrabarTemporal",
        data: {
            tipo: TIPO_OFERTA_GLOBAL,
            ofertasSubir: JSON.stringify(excelSubir),
            ofertasBajar: null,
            fechaini: $('#fecha-inicio-select').val(),
            numofertas: $('#txt-num-dias').val()
        },
        success: function (model) {
            if (model.Resultado != -1) {
                btnSeleccionar(TIPO_ACCION_EXITO_GRABAR);


            } else {
                $('#mensajeRegistro').show();
                mostrarMensaje('mensajeRegistro', 'alert', model.Mensaje);
            }
            if (cerrarPopup)
                $('#Messages').bPopup().close();
        },
        error: function (req, status, error) {
            $('#mensajeRegistro').show();
            $('#mensajeRegistro').html('El envio tiene un error ' + status + ' - ' + error + '.');
        }
    });
}

// #endregion

function btnCopiarInicializar() {

    $('#dte-fecha-copiar-popup').unbind();

    var formatoFecha = TIPO_OFERTA_GLOBAL == TIPO_OFERTA_DIARIO ? 'd/m/Y' : 'm Y';
    $('#dte-fecha-copiar-popup').Zebra_DatePicker({
        format: formatoFecha,
        always_visible: $('#container'),
        days: ["Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado"],
        months: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
        onSelect: function () {
            $.ajax({
                type: 'POST',
                url: strControlador + "EnvioListar",
                data: {
                    tipo: TIPO_OFERTA_GLOBAL,
                    strfechaoferta: $('#dte-fecha-copiar-popup').val()
                },
                global: false,
                cache: false,
                success: function (model) {

                    if (model.Resultado != -1) {
                        var arrEnvios = model.ListaEnvio;

                        generarHtmlTablaEnvio(TIPO_ACCION_VER_COPIAR_ENVIO, arrEnvios, '#div-copiar-popup', '#div-copiar-popup');
                    } else {
                        alert("Ha ocurrido un error");
                    }
                },
                error: function (req, status, error) {
                    console.log(error);
                }

            });
        }
    });

    var t = setTimeout(function () {
        popup = $('#div-copiar-popup').bPopup({ modalclose: false, escclose: false });

        clearTimeout(t);
    }, 1000);

}

// #region Listar handsontable

function duplicarHandsonTable() {
    var handsonTableACopiar = hstSubastaGlobalSubir;
    var handsonTableCopiada = hstSubastaGlobalBajar;

    var data1 = hstSubastaGlobalSubir.getData();
    var data2 = hstSubastaGlobalBajar.getData();

    var numFilasDataACopiar = data1.length;
    var numFilasDataCopiada = data2.length;

    //var dataAntigua = hstSubastaGlobalBajar.getSourceData();

    for (var fila = 1; fila < numFilasDataACopiar; fila++) {
        for (var col = 0; col < 14; col++) {

            var valorACopiar = data1[fila][col];
            hstSubastaGlobalBajar.setDataAtCell(fila, col, valorACopiar);
        }
    }

    //var dataInetrmedia = hstSubastaGlobalBajar.getSourceData();

    if (numFilasDataACopiar < numFilasDataCopiada) {

        var numFilasEliminar = numFilasDataCopiada - numFilasDataACopiar;
        hstSubastaGlobalBajar.alter('remove_row', numFilasDataACopiar - 1, numFilasEliminar);

    }

    hstSubastaGlobalBajar.render();

    //var dataNueva3 = hstSubastaGlobalBajar.getData();
}

function obtenerDataHansontablesTotal() {
    var lstHandson = [];

    var temp1 = -1;
    var temp2 = -1;

    var data1 = hstSubastaGlobalSubir.getData();
    var data2 = hstSubastaGlobalBajar.getData();

    var excelSubir = [];
    var excelBajar = [];
    //Revisar si afecta la posición de almacenamiento.
    for (var i = intFilaInicioGlobal, j = hstSubastaGlobalSubir.getData().length - 1; i < j; i++) {
        temp1 = -1;
        var intIndiceModoOperacion1 = hstSubastaGlobalSubir.getDataAtCell(i, posIndice);

        //if ($.isNumeric(hstSubastaGlobalSubir.getDataAtCell(i, 7))) {  //bandaDisponible existe
        if (hstSubastaGlobalSubir.getDataAtCell(i, posBandaAdjudicada) != null) temp1 = 1; //Para identificar cabeceras de URS, usado para traslapes (controller)
        excelSubir.push({
            urs: hstSubastaGlobalSubir.getDataAtCell(i - intIndiceModoOperacion1, intUrsColumnaGlobal),
            horainicio: hstSubastaGlobalSubir.getDataAtCell(i - intIndiceModoOperacion1, intHoraInicioColumnaGlobal),
            horaFin: hstSubastaGlobalSubir.getDataAtCell(i - intIndiceModoOperacion1, intHoraFinColumnaGlobal),
            potenciaOfertada: hstSubastaGlobalSubir.getDataAtCell(i - intIndiceModoOperacion1, 3),
            precio: hstSubastaGlobalSubir.getDataAtCell(i - intIndiceModoOperacion1, 4),
            modoOperacion: hstSubastaGlobalSubir.getDataAtCell(i, 5),
            bandaCalificada: hstSubastaGlobalSubir.getDataAtCell(i, 6),
            //bandaDisponible: hstSubastaGlobalSubir.getDataAtCell(i, 7),
            datoVisible: temp1,
        });
        //}

    }

    for (var i = intFilaInicioGlobal, j = hstSubastaGlobalBajar.getData().length - 1; i < j; i++) {
        temp2 = -1;
        var intIndiceModoOperacion2 = hstSubastaGlobalBajar.getDataAtCell(i, posIndice);

        //if ($.isNumeric(hstSubastaGlobalBajar.getDataAtCell(i, 7))) {  //bandaDisponible existe
        if (hstSubastaGlobalBajar.getDataAtCell(i, posBandaAdjudicada) != null) temp2 = 1; //Para identificar cabeceras de URS, usado para traslapes (controller)
        excelBajar.push({
            urs: hstSubastaGlobalBajar.getDataAtCell(i - intIndiceModoOperacion2, intUrsColumnaGlobal),
            horainicio: hstSubastaGlobalBajar.getDataAtCell(i - intIndiceModoOperacion2, intHoraInicioColumnaGlobal),
            horaFin: hstSubastaGlobalBajar.getDataAtCell(i - intIndiceModoOperacion2, intHoraFinColumnaGlobal),
            potenciaOfertada: hstSubastaGlobalBajar.getDataAtCell(i - intIndiceModoOperacion2, 3),
            precio: hstSubastaGlobalBajar.getDataAtCell(i - intIndiceModoOperacion2, 4),
            modoOperacion: hstSubastaGlobalBajar.getDataAtCell(i, 5),
            bandaCalificada: hstSubastaGlobalBajar.getDataAtCell(i, 6),
            //bandaDisponible: hstSubastaGlobalBajar.getDataAtCell(i, 7),
            datoVisible: temp2,
        });
        //}

    }

    lstHandson.push(excelSubir);
    lstHandson.push(excelBajar);

    return lstHandson;
}

function obtenerDataHansontablesTotalTemporal() {
    var lstHandson = [];

    var temp1 = -1;
    var temp2 = -1;

    var data1 = hstSubastaGlobalSubir.getData();

    var excelSubir = [];
    var excelBajar = [];
    //Revisar si afecta la posición de almacenamiento.
    for (var i = intFilaInicioGlobal, j = hstSubastaGlobalSubir.getData().length - 1; i < j; i++) {
        temp1 = -1;
        var intIndiceModoOperacion1 = hstSubastaGlobalSubir.getDataAtCell(i, posIndice);

        //if ($.isNumeric(hstSubastaGlobalSubir.getDataAtCell(i, 7))) {  //bandaDisponible existe
        if (hstSubastaGlobalSubir.getDataAtCell(i, posBandaAdjudicada) != null) temp1 = 1; //Para identificar cabeceras de URS, usado para traslapes (controller)
        excelSubir.push({
            urs: hstSubastaGlobalSubir.getDataAtCell(i - intIndiceModoOperacion1, intUrsColumnaGlobal),
            horainicio: hstSubastaGlobalSubir.getDataAtCell(i - intIndiceModoOperacion1, intHoraInicioColumnaGlobal),
            horaFin: hstSubastaGlobalSubir.getDataAtCell(i - intIndiceModoOperacion1, intHoraFinColumnaGlobal),
            potenciaOfertada: hstSubastaGlobalSubir.getDataAtCell(i - intIndiceModoOperacion1, 3),
            precio: hstSubastaGlobalSubir.getDataAtCell(i - intIndiceModoOperacion1, 4),
            modoOperacion: hstSubastaGlobalSubir.getDataAtCell(i, 5),
            bandaCalificada: hstSubastaGlobalSubir.getDataAtCell(i, 6),
            //bandaDisponible: hstSubastaGlobalSubir.getDataAtCell(i, 7),
            datoVisible: temp1,
        });
        //}

    }

    lstHandson.push(excelSubir);

    return lstHandson;
}



// #endregion

// #region Funciones utiles

function GetFecha(fecha, dias) {
    var fechSelect = fecha.replace(' ', '').replace(' ', '').replace(' ', '').replace(' ', ''),
        aux = fechSelect.split('/'),
        fechAux = aux[2] + '-' + aux[1] + '-' + aux[0],
        fech = new Date(fechAux),
        sfech = fech;
    sfech.setDate(fech.getDate() + dias);

    return sfech;
}

function mostrarMensaje(id, tipo, mensaje) {
    $('#' + id).removeClass();
    $('#' + id).addClass('action-' + tipo);
    $('#' + id).html(mensaje);
};

// #endregion

// #region Validaciones Handsontable

function validarOfertasInternal(tipoOferta, hstSubasta, tipoHoja) {

    var blnEnviar = true,
        blnFormatoHorasCorrecto = true,
        strMensaje = '';

    var arrOfertas = hstSubasta.getData();
    console.log("validarOfertasInternal arrOfertas>>", arrOfertas);
    //ini1
    if (arrOfertas.length > 4) {
        var arrUrsSeleccionadas = [];

        //ini2
        for (var i = intFilaInicioGlobal, j = arrOfertas.length - 1; i < j; i++) {
            var intIndiceModoOperacion = arrOfertas[i][posIndice];

            //ini3
            for (var m = 0, n = 7; m < n; m++) {
                var blnIsValid = true,
                    tdCell = hstSubasta.getCell(i, m),
                    strValue = arrOfertas[i][m];

                //validacion Campos "Hora"
                if (intIndiceModoOperacion === 0 && (m === intHoraInicioColumnaGlobal || m === intHoraFinColumnaGlobal)) {
                    if (strValue === null || strValue.length === 0 || !isNaN(strValue)) {
                        blnFormatoHorasCorrecto =
                            blnIsValid = false;
                        tipoError = "Celda Vacia";
                        codiError = 1;
                    } else if (ARR_HORAS_GLOBAL.indexOf(strValue) === -1) {
                        blnFormatoHorasCorrecto =
                            blnIsValid = false;
                        tipoError = "Hora no válida.";
                        codiError = 5;
                    } else {
                        strValue = parseInt(strValue.replace(':', ''));

                        if (isNaN(strValue) || (strValue < 0 || strValue > 2359)) {
                            blnFormatoHorasCorrecto = blnIsValid = false;
                            tipoError = "Hora no válida.";
                            codiError = 5;
                        }
                    }
                } else {
                    //validacion campo : URS
                    if (intIndiceModoOperacion === 0 && m === intUrsColumnaGlobal) {
                        var arrUrs = ARR_URS_GLOBAL;
                        strValue = strValue.toString();
                        if (strValue && strValue.length > 0 && parseInt(strValue) > 0) {
                            var found = $.map(arrUrs, function (val) {
                                if (val.ID == strValue) return val;
                            });

                            if (found.length === 1) {
                                if (arrUrsSeleccionadas.indexOf(found[0]) === -1) {
                                    arrUrsSeleccionadas.push(found[0]);
                                }

                                blnIsValid = true;
                            } else {
                                blnIsValid = false;
                            }
                        } else {
                            blnIsValid = false;
                        }
                        if (!blnIsValid) {
                            tipoError = "Urs invalido";
                            codiError = 5;
                        }
                    } else {
                        //validacion campo: PRECIO
                        if (intIndiceModoOperacion === 0 && m === 4) {
                            blnIsValid = true;
                            if (strValue === null || strValue.length === 0) {
                                blnIsValid = false;
                                tipoError = "Celda Vacia";
                                codiError = 1;
                            } else if (!$.isNumeric(strValue)) {
                                blnIsValid = false;
                                tipoError = "Valor no es un número";
                                codiError = 2;
                                //} else if (parseFloat(strValue) < 0) {
                                //    blnIsValid = false;
                                //    tipoError = "Valor menor a cero"; //Editado FIT - 8 JUN 2018
                            } else if (parseFloat(strValue) > parseFloat(precioMaximo)) {
                                blnIsValid = false;
                                tipoError = "Precio Máximo: " + precioMaximo;
                                codiError = 4;
                            } else if (parseFloat(strValue) < parseFloat(precioMinimo)) {
                                blnIsValid = false;
                                tipoError = "Precio Mínimo: " + precioMinimo;
                                codiError = 3;
                            } //Agregado FIT - 8 JUN 2018

                        } else {
                            //validacion campo: POTENCIA OFERTADA
                            if (intIndiceModoOperacion === 0 && m === 3) {
                                blnIsValid = true;
                                if (strValue === null || strValue.length === 0) {
                                    blnIsValid = false;
                                    tipoError = "Celda Vacia";
                                    codiError = 1;
                                } else {
                                    if (!$.isNumeric(strValue)) {
                                        blnIsValid = false;
                                        tipoError = "Valor no es un número";
                                        codiError = 2;
                                    } else {
                                        if (parseInt(strValue) < 0) {
                                            blnIsValid = false;
                                            tipoError = "Valor menor a cero";
                                            codiError = 3;
                                        } else if (parseFloat(strValue) < potenciaMinimoMan) {
                                            //Validacion nuevas

                                            blnIsValid = false;
                                            tipoError = "Potencia mínima ofertada: " + potenciaMinimoMan;
                                            codiError = 4;

                                        } else {
                                            //Validacion nuevas
                                            var bndCalificada = arrOfertas[i][6];

                                            if (parseFloat(strValue) > bndCalificada) {
                                                blnIsValid = false;
                                                tipoError = "Potencia superior a la permitida: " + bndCalificada;
                                                codiError = 4;
                                            }

                                        }
                                    }
                                }




                                //} else if (m === 5) {
                                //    blnIsValid = true;

                                //    if (intIndiceModoOperacion == 0) {
                                //        var strMOs = '',
                                //            strDelimiter = '';
                                //        // 20 Jul- Wilfredo solicita no validar Combinaciones de Modos de Operacion
                                //        //if (tipoOferta == 1) {

                                //        //    for (var x = 0, y = arrOfertas[i][10]; x < y; x++) {
                                //        //        if ($.isNumeric(arrOfertas[i + x][7]) && hstSubasta.getCellMeta(i + x, 7).readOnly == false) {
                                //        //            strMOs += strDelimiter + arrOfertas[i + x][6];
                                //        //            strDelimiter = ',';
                                //        //        }
                                //        //    }
                                //        //    if (strMOs.length > 0) {
                                //        //        $.ajax({
                                //        //            type: 'POST',
                                //        //            url: strControlador + "validarmodosoperacion",
                                //        //            async: false,
                                //        //            data: { modosOperacion: strMOs },
                                //        //            success: function (response) {
                                //        //                blnIsValid = (response == "True");
                                //        //            }
                                //        //        });
                                //        //    }
                                //        //}


                                //        for (var x = 0, y = arrOfertas[i][10]; x < y; x++) {
                                //            hstSubasta.setCellMeta(i + x, m, 'valid', blnIsValid);
                                //        }


                                //        //if (!blnIsValid) {
                                //        //    tipoError = "Combinacion Modo Operacion No valida";
                                //        //    strMensaje = "Combinacion Modo Operacion No valida";
                                //        //}
                                //    }

                            } else {

                                //validacion campo: BANDA CALIFICADA
                                if (m === 6) {
                                    blnIsValid = true;
                                    if (strValue !== null && (strValue.length > 0 || $.isNumeric(strValue))) {
                                        if (!$.isNumeric(strValue)) {
                                            blnIsValid = false;
                                            tipoError = "Valor no es un número";
                                            codiError = 1;
                                        }
                                    }
                                } else {


                                }
                            }

                        }
                    }
                }

                //console.log(i + ', ' + m + ' : ' + strValue + ' (' + blnIsValid + ')');
                if (!blnIsValid) {
                    var celda = getExcelColumnName(m + 1) + (i + 1).toString();

                    agregarError(tipoHoja, celda, strValue, tipoError, codiError);
                    blnEnviar = false;
                }

                if (m !== 5) {
                    hstSubasta.setCellMeta(i, m, 'valid', blnIsValid);
                }


            }
            //fin3
        }
        //fin2

        if (blnFormatoHorasCorrecto) {
            blnEnviar = validarTraslapeHorasInternal(tipoHoja, hstSubasta, arrOfertas);
            console.log('Traslape horas' + blnEnviar);
        }


        if (blnEnviar && tipoOferta === TIPO_OFERTA_DEFECTO) {
            if (arrUrsSeleccionadas.length !== ARR_URS_GLOBAL.length) {
                blnEnviar = false;
                strMensaje = 'Debe seleccionar todos los Urs.';

                agregarError(tipoHoja, '', '', strMensaje, 101);
            }

            if (blnEnviar) {
                blnEnviar = validar24HorasInternal(arrOfertas);

                if (blnEnviar == false) {
                    strMensaje = 'Debe ingresar las 24 horas para todas las Urs.';
                    agregarError(tipoHoja, '', '', strMensaje, 102);
                }
            }
        }
    } else {
        NUM_HOJAS_VACIAS++;

        //Si en ninguna de las dos hojas hay data a enviar, muestra error
        if (NUM_HOJAS_VACIAS > 1 && tipoOferta != TIPO_OFERTA_DEFECTO) {
            //strMensaje = 'No ha ingresado ofertas en la hoja "' + hojaSubir + '"';
            //agregarError(tipoHoja, '', '', strMensaje, 103);
            strMensaje = 'No ha ingresado ninguna oferta';
            agregarError('', '', '', strMensaje, 103);
            blnEnviar = false;
        }

        if (NUM_HOJAS_VACIAS >= 1 && tipoOferta === TIPO_OFERTA_DEFECTO) {
            strMensaje = 'La hoja "Oferta ' + tipoHoja + '" debe estar llena.';
            agregarError(tipoHoja, '', '', strMensaje, 103);
            blnEnviar = false;
        }
    }

    //$('#mensajeRegistro').hide();
    ////fin1

    //if (!blnEnviar) {
    //    $('#mensajeRegistro').show();
    //    mostrarMensaje('mensajeRegistro', 'alert', 'Hay errores en el Registro. ' + strMensaje);
    //}

    //console.log(blnEnviar);
    return blnEnviar;
}

function validarOfertasInternalTemporal(tipoOferta, hstSubasta, tipoHoja) {

    var blnEnviar = true,
        blnFormatoHorasCorrecto = true,
        strMensaje = '';

    var arrOfertas = hstSubasta.getData();
    console.log("validarOfertasInternalTemporal arrOfertas>>", arrOfertas);
    //ini1
    if (arrOfertas.length > 4) {
        var arrUrsSeleccionadas = [];

        //ini2
        for (var i = intFilaInicioGlobal, j = arrOfertas.length - 1; i < j; i++) {
            var intIndiceModoOperacion = arrOfertas[i][posIndice];

            //ini3
            for (var m = 0, n = 7; m < n; m++) {
                var blnIsValid = true,
                    tdCell = hstSubasta.getCell(i, m),
                    strValue = arrOfertas[i][m];

                //validacion Campos "Hora"
                if (intIndiceModoOperacion === 0 && (m === intHoraInicioColumnaGlobal || m === intHoraFinColumnaGlobal)) {
                    if (strValue === null || strValue.length === 0 || !isNaN(strValue)) {
                        blnFormatoHorasCorrecto =
                            blnIsValid = false;
                        tipoError = "Celda Vacia";
                        codiError = 1;
                    } else if (ARR_HORAS_GLOBAL.indexOf(strValue) === -1) {
                        blnFormatoHorasCorrecto =
                            blnIsValid = false;
                        tipoError = "Hora no válida.";
                        codiError = 5;
                    } else {
                        strValue = parseInt(strValue.replace(':', ''));

                        if (isNaN(strValue) || (strValue < 0 || strValue > 2359)) {
                            blnFormatoHorasCorrecto = blnIsValid = false;
                            tipoError = "Hora no válida.";
                            codiError = 5;
                        }
                    }
                } else {
                    //validacion campo : URS
                    if (intIndiceModoOperacion === 0 && m === intUrsColumnaGlobal) {
                        var arrUrs = ARR_URS_GLOBAL;
                        strValue = strValue.toString();
                        if (strValue && strValue.length > 0 && parseInt(strValue) > 0) {
                            var found = $.map(arrUrs, function (val) {
                                if (val.ID == strValue) return val;
                            });

                            if (found.length === 1) {
                                if (arrUrsSeleccionadas.indexOf(found[0]) === -1) {
                                    arrUrsSeleccionadas.push(found[0]);
                                }

                                blnIsValid = true;
                            } else {
                                blnIsValid = false;
                            }
                        } else {
                            blnIsValid = false;
                        }
                        if (!blnIsValid) {
                            tipoError = "Urs invalido";
                            codiError = 5;
                        }
                    } else {
                        //validacion campo: PRECIO
                        if (intIndiceModoOperacion === 0 && m === 4) {
                            blnIsValid = true;
                            if (strValue === null || strValue.length === 0) {
                                blnIsValid = false;
                                tipoError = "Celda Vacia";
                                codiError = 1;
                            } else if (!$.isNumeric(strValue)) {
                                blnIsValid = false;
                                tipoError = "Valor no es un número";
                                codiError = 2;
                                //} else if (parseFloat(strValue) < 0) {
                                //    blnIsValid = false;
                                //    tipoError = "Valor menor a cero"; //Editado FIT - 8 JUN 2018
                            } else if (parseFloat(strValue) > parseFloat(precioMaximo)) {
                                blnIsValid = false;
                                tipoError = "Precio Máximo: " + precioMaximo;
                                codiError = 4;
                            } else if (parseFloat(strValue) < parseFloat(precioMinimo)) {
                                blnIsValid = false;
                                tipoError = "Precio Mínimo: " + precioMinimo;
                                codiError = 3;
                            } //Agregado FIT - 8 JUN 2018

                        } else {
                            //validacion campo: POTENCIA OFERTADA
                            if (intIndiceModoOperacion === 0 && m === 3) {
                                blnIsValid = true;
                                if (strValue === null || strValue.length === 0) {
                                    blnIsValid = false;
                                    tipoError = "Celda Vacia";
                                    codiError = 1;
                                } else {
                                    if (!$.isNumeric(strValue)) {
                                        blnIsValid = false;
                                        tipoError = "Valor no es un número";
                                        codiError = 2;
                                    } else {
                                        if (parseInt(strValue) < 0) {
                                            blnIsValid = false;
                                            tipoError = "Valor menor a cero";
                                            codiError = 3;
                                        } else if (parseFloat(strValue) < potenciaMinimoMan) {
                                            //Validacion nuevas

                                            blnIsValid = false;
                                            tipoError = "Potencia mínima ofertada: " + potenciaMinimoMan;
                                            codiError = 4;

                                        } else {
                                            //Validacion nuevas
                                            var bndCalificada = arrOfertas[i][6] / 2;

                                            if (parseFloat(strValue) > bndCalificada) {
                                                blnIsValid = false;
                                                tipoError = "Potencia superior a la permitida: " + bndCalificada;
                                                codiError = 4;
                                            }

                                        }
                                    }
                                }


                            } else {

                                //validacion campo: BANDA CALIFICADA
                                if (m === 6) {
                                    blnIsValid = true;
                                    if (strValue !== null && (strValue.length > 0 || $.isNumeric(strValue))) {
                                        if (!$.isNumeric(strValue)) {
                                            blnIsValid = false;
                                            tipoError = "Valor no es un número";
                                            codiError = 1;
                                        }
                                    }
                                } else {


                                }
                            }

                        }
                    }
                }

                //console.log(i + ', ' + m + ' : ' + strValue + ' (' + blnIsValid + ')');
                if (!blnIsValid) {
                    var celda = getExcelColumnName(m + 1) + (i + 1).toString();

                    agregarError(tipoHoja, celda, strValue, tipoError, codiError);
                    blnEnviar = false;
                }

                if (m !== 5) {
                    hstSubasta.setCellMeta(i, m, 'valid', blnIsValid);
                }


            }
            //fin3
        }
        //fin2

        if (blnFormatoHorasCorrecto) {
            blnEnviar = validarTraslapeHorasInternal(tipoHoja, hstSubasta, arrOfertas);
            console.log('Traslape horas' + blnEnviar);
        }


        if (blnEnviar && tipoOferta === TIPO_OFERTA_DEFECTO) {
            if (arrUrsSeleccionadas.length !== ARR_URS_GLOBAL.length) {
                blnEnviar = false;
                strMensaje = 'Debe seleccionar todos los Urs.';

                agregarError(tipoHoja, '', '', strMensaje, 101);
            }

            if (blnEnviar) {
                blnEnviar = validar24HorasInternal(arrOfertas);

                if (blnEnviar == false) {
                    strMensaje = 'Debe ingresar las 24 horas para todas las Urs.';
                    agregarError(tipoHoja, '', '', strMensaje, 102);
                }
            }
        }
    } else {
        NUM_HOJAS_VACIAS++;

        //Si en ninguna de las dos hojas hay data a enviar, muestra error
        if (NUM_HOJAS_VACIAS > 1 && tipoOferta != TIPO_OFERTA_DEFECTO) {
            //strMensaje = 'No ha ingresado ofertas en la hoja "' + hojaSubir + '"';
            //agregarError(tipoHoja, '', '', strMensaje, 103);
            strMensaje = 'No ha ingresado ninguna oferta';
            agregarError('', '', '', strMensaje, 103);
            blnEnviar = false;
        }

        if (NUM_HOJAS_VACIAS >= 1 && tipoOferta === TIPO_OFERTA_DEFECTO) {
            strMensaje = 'La hoja "Oferta ' + tipoHoja + '" debe estar llena.';
            agregarError(tipoHoja, '', '', strMensaje, 103);
            blnEnviar = false;
        }
    }

    //$('#mensajeRegistro').hide();
    ////fin1

    //if (!blnEnviar) {
    //    $('#mensajeRegistro').show();
    //    mostrarMensaje('mensajeRegistro', 'alert', 'Hay errores en el Registro. ' + strMensaje);
    //}

    //console.log(blnEnviar);
    return blnEnviar;
}

function validarTraslapeHorasInternal(hoja, hstSubasta, ofertas) {
    var blnSatisfactorio = true;

    for (var i = ofertas.length - 2, j = intFilaInicioGlobal; i >= j; i--) {
        var intIndiceModoOperacion = ofertas[i][posIndice];

        if (intIndiceModoOperacion == 0) {
            if (ofertas[i][intHoraInicioColumnaGlobal] == null) continue;

            var intUrs = parseInt(ofertas[i][intUrsColumnaGlobal]),
                strHoraInicio = ofertas[i][intHoraInicioColumnaGlobal].replace(':', ''),
                strHoraFin = ofertas[i][intHoraFinColumnaGlobal].replace(':', ''),
                intHoraInicio = parseInt(strHoraInicio),
                intHoraFin = parseInt(strHoraFin);


            if (intHoraFin <= intHoraInicio) {
                hstSubasta.setCellMeta(i, intHoraInicioColumnaGlobal, 'valid', false);
                hstSubasta.setCellMeta(i, intHoraFinColumnaGlobal, 'valid', false);
                blnSatisfactorio = false;

                console.log('A - ' + i + ' - ' + blnSatisfactorio);

                var celda = getExcelColumnName(1) + (i + 1).toString();
                //agregarError('miTabX1', celda, (intHoraInicio + ' - ' + intHoraFin), 'Hora de Inicio mayor o igual que la hora de fin', 3);
                agregarError(hoja, celda, (intHoraInicio + ' - ' + intHoraFin), 'Hora de Inicio mayor o igual que la hora de fin', 3);
            } else if (i > j) {
                for (var m = ofertas.length - 2, n = intFilaInicioGlobal; m >= n; m--) {
                    var intIndiceModoOperacion = ofertas[m][posIndice]

                    if (intIndiceModoOperacion == 0) {
                        var intUrsAnterior = parseInt(ofertas[m][intUrsColumnaGlobal]);

                        if (m != i && intUrs === intUrsAnterior) {
                            var intHoraInicioAnterior = parseInt(ofertas[m][intHoraInicioColumnaGlobal].replace(':', '')),
                                intHoraFinAnterior = parseInt(ofertas[m][intHoraFinColumnaGlobal].replace(':', ''));

                            if ((intHoraInicio >= intHoraInicioAnterior && intHoraInicio < intHoraFinAnterior)
                                || (intHoraFin < intHoraFinAnterior && intHoraFin > intHoraInicioAnterior)
                                || (intHoraInicio <= intHoraInicioAnterior && intHoraFin >= intHoraFinAnterior)) {

                                hstSubasta.setCellMeta(i, intHoraInicioColumnaGlobal, 'valid', false);
                                hstSubasta.setCellMeta(i, intHoraFinColumnaGlobal, 'valid', false);
                                blnSatisfactorio = false;

                                var celda = getExcelColumnName(1) + (i + 1).toString();
                                //agregarError('miTabX2', celda, (strHoraInicio + ' - ' + strHoraFin), 'Traslape de hora', 5);
                                agregarError(hoja, celda, (strHoraInicio + ' - ' + strHoraFin), 'Traslape de hora', 5);

                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    return blnSatisfactorio;
}

function validar24HorasInternal(ofertas) {
    var blnCorrecto = true,
        arrUrsSeleccionada = [];

    for (var i = intFilaInicioGlobal, j = ofertas.length - 1; i < j; i++) {

        if (ofertas[i][intUrsColumnaGlobal] == null) continue;
        var objUrs = parseInt(ofertas[i][intUrsColumnaGlobal]);

        if (arrUrsSeleccionada.indexOf(objUrs) === -1) {
            arrUrsSeleccionada.push(objUrs);
        }
    }

    for (var i = 0, j = arrUrsSeleccionada.length; i < j; i++) {
        var objUrs = parseInt(arrUrsSeleccionada[i]),
            intTotalHoras = 0;

        for (var m = intFilaInicioGlobal, n = ofertas.length - 1; m < n; m++) {
            var arrFilaSeleccionada = ofertas[m],
                objUrsSeleccionada = parseInt(arrFilaSeleccionada[intUrsColumnaGlobal]);

            if (objUrs === objUrsSeleccionada) {
                intTotalHoras += (parseInt(arrFilaSeleccionada[intHoraFinColumnaGlobal].replace(':', '')) - parseInt(arrFilaSeleccionada[intHoraInicioColumnaGlobal].replace(':', '')));
            }
        }

        if (intTotalHoras < 2359) {
            blnCorrecto = false

            break;
        }
    }

    return blnCorrecto;
}

// #endregion

// #region Ver Errores

function agregarError(hoja, celda, valor, tipo, codiError) {
    if (validarError(hoja, celda)) {
        var regError = {
            Hoja: hoja,
            Celda: celda,
            Valor: valor,
            Tipo: tipo,
            Codigo: codiError
        };
        listErrores.push(regError);
        switch (tipo) {
            case 2:
                totNoNumero++;
                break;
            case 3:
                totLimInf++;
                break;
            case 4:
                totLimSup++;
                break;
        }
    }
}

function validarError(hoja, celda) {
    for (var j in listErrores) {
        if (listErrores[j]['Hoja'] == hoja && listErrores[j]['Celda'] == celda) {
            return false;
        }
    }
    return true;
}

function limpiarError() {
    totLimInf = 0;
    totLimSup = 0;
    totNoNumero = 0;
    listErrores = [];
}

function getExcelColumnName(pi_columnNumber) {
    var li_dividend = pi_columnNumber;
    var ls_columnName = "";
    var li_modulo;

    while (li_dividend > 0) {
        li_modulo = (li_dividend - 1) % 26;
        ls_columnName = String.fromCharCode(65 + li_modulo) + ls_columnName;
        li_dividend = Math.floor((li_dividend - li_modulo) / 26);
    }

    return ls_columnName;
}

function mostrarErrores() {
    $('#div-error-popup > table > tbody > tr').remove();

    if (listErrores != null && listErrores.length > 0) {
        for (var i = 0; i < listErrores.length; i++) {
            var tdHoja = $("<td class='coloreado" + i + "'>").html(listErrores[i].Hoja),
                tdCelda = $("<td class='coloreado" + i + "'>").html(listErrores[i].Celda),
                tdValor = $("<td class='coloreado" + i + "'>").html(listErrores[i].Valor),
                tdTipo = $('<td>').html(listErrores[i].Tipo),

                //$('#tab').css({ "background": "black", "color": "white" });

                tr = $('<tr>')
                    .data('hoja', listErrores[i].Hoja)
                    .data('celda', listErrores[i].Celda)
                    .data('valor', listErrores[i].Valor)
                    .data('tipo', listErrores[i].tipo)
                    .append(tdHoja)
                    .append(tdCelda)
                    .append(tdValor)
                    .append(tdTipo)
            $('#div-error-popup > table > tbody').append(tr);

            //coloreamos las celdas segun tipo error
            var miCodError = listErrores[i].Codigo;
            if (miCodError == 2)
                $('.coloreado' + i).css({ "background": "red", "color": "white", "font-weight": "bold" });
            if (miCodError == 3)
                $('.coloreado' + i).css({ "background": "orange", "color": "white", "font-weight": "bold" });
            if (miCodError == 4)
                $('.coloreado' + i).css({ "background": "yellow", "color": "black", "font-weight": "bold" });
        }

        var t = setTimeout(function () {
            popup = $('#div-error-popup').bPopup({ modalclose: false, escclose: false });
            clearTimeout(t);
        }, 1000);

    } else {
        var td = $('<td>').html('No se encontraron errores.')
            .attr('colSpan', 4),
            tr = $('<tr>')
                .append(td);

        $('#div-error-popup > table > tbody').append(tr);
    }

}

// #endregion

// #region Ver Envíos

function btnVerEnvios() {

    $.ajax({
        type: 'POST',
        url: strControlador + "EnvioListar",
        data: {
            tipo: TIPO_OFERTA_GLOBAL,
            strfechaoferta: $('#dte-subasta-fecha').val()
        },
        success: function (model) {

            if (model.Resultado != -1) {
                var arrEnvios = model.ListaEnvio;

                generarHtmlTablaEnvio(TIPO_ACCION_VER_ENVIO, arrEnvios, '#div-envios-popup', '#div-envios-popup');
            } else {
                alert("Ha ocurrido un error");
            }
        },
        error: function (req, status, error) {
            alert("Ha ocurrido un error");
            console.log(error);
        }
    });
}

function generarHtmlTablaEnvio(accion, arrEnvios, idTablaEnvio, idPopup) {
    $(idTablaEnvio + ' > table > tbody > tr').remove();

    if (arrEnvios != null && arrEnvios.length > 0) {
        for (var i = 0, j = arrEnvios.length; i < j; i++) {
            var objEnvio = arrEnvios[i];

            var tdCodigo = $('<td>').html(objEnvio.CodigoEnvio),
                tdFechaEnvio = $('<td>').html(objEnvio.FechaEnvio),
                tdFechaOferta = $('<td>').html(objEnvio.FechaOferta),
                tr = $('<tr>')
                    .data('fechaoferta', objEnvio.FechaOferta)
                    .data('codigo', objEnvio.Codigo)
                    .data('fechaenvio', objEnvio.FechaEnvio)
                    .append(tdFechaOferta)
                    .append(tdCodigo)
                    .append(tdFechaEnvio)
                    .click(function () {
                        $(idPopup).bPopup().close();

                        btnSeleccionar(accion, $(this).data('codigo'));

                    });

            $(idTablaEnvio + ' > table > tbody').append(tr);
        }
    } else {
        var td = $('<td>').html('No se encontrarón envios.')
            .attr('colSpan', 3),
            tr = $('<tr>')
                .append(td);

        $(idTablaEnvio + ' > table > tbody').append(tr);
    }

    var t = setTimeout(function () {
        $(idPopup).bPopup();

        clearTimeout(t);
    }, 1000);
}

// #endregion

// #region Handontable

var listErrores = [];
var totNoNumero = 0;
var totLimSup = 0;
var totLimInf = 0;
var tipoError = 0;

var colorRojo = "red";
var colorNaranja = "orange";
var colorAmarillo = "yellow";

var codiError = 0;   // 1: vacio,  2: no numerico,   3: inferior al limite inferior, 4: superior al limite superior, 5: otros
var hojaNeutro = "MiTtab";
var hojaSubir = "Subir";
var hojaBajar = "Bajar";

var hstSubastaGlobal = null,
    hstSubastaGlobalSubir = null,
    hstSubastaGlobalBajar = null,
    intHoraInicioColumnaGlobal = 1,
    intHoraFinColumnaGlobal = 2,
    intUrsColumnaGlobal = 0,
    intFilaInicioGlobal = 3,
    posIndice = 8,
    posCantidad = 9,
    posReservaFirme = 11,
    posLibre1 = 12,
    posBandaAdjudicada = 13,
    ARR_URS_GLOBAL = [],
    ARR_MODO_OPERACION_GLOBAL = [],
    ARR_HORAS_GLOBAL = ['00:00', '00:30', '01:00', '01:30', '02:00', '02:30', '03:00', '03:30', '04:00', '04:30', '05:00', '05:30'
        , '06:00', '06:30', '07:00', '07:30', '08:00', '08:30', '09:00', '09:30', '10:00', '10:30', '11:00', '11:30'
        , '12:00', '12:30', '13:00', '13:30', '14:00', '14:30', '15:00', '15:30', '16:00', '16:30', '17:00', '17:30'
        , '18:00', '18:30', '19:00', '19:30', '20:00', '20:30', '21:00', '21:30', '22:00', '22:30', '23:00', '23:30', '23:59'],
    HEIGHT_MINIMO = 500,
    tmeHoraInicio,
    tmeHoraFin,
    tmeTimeElapsed,
    tmeHoraActual,
    precioMaximo = 0,
    precioMinimo = 0,
    potenciaMinimoMan = 0,
    sitOferta = null,
    minSpareRows = 0,
    aperturaPorFecha = false,
    restaTamanio = 1;

var contexMenuConfig = {
    items: {
        'row_below': { name: "Insertar fila abajo" },
        'row_above': { name: "Insertar fila arriba" },
        'remove_row': { name: "Eliminar fila" },
        'undo': { name: "Deshacer" },
        'copy': { name: "Copiar" }
    }
};

function mostrarOfertasInternalSubir(tipoOferta, urss, modosOperacion, horas, ofertas, lectura, minRow) {
    ofertas = (ofertas === null ? [] : ofertas);
    lectura = (lectura === undefined ? false : lectura);

    //cuando sea Oferta por defecto, las columnas de "Hora inicio" y "Hora fin" son solo lectura
    var lecturaColHora = lectura;
    if (TIPO_OFERTA_GLOBAL == TIPO_OFERTA_DEFECTO) lecturaColHora = true;

    var tamanioColumnaEliminar = lectura ? 0.1 : 42;
    var tamanioColumnaPotOfertada = tipoOferta == TIPO_OFERTA_DEFECTO ? 0.1 : 95;
    var container = document.getElementById('hst-subasta-ingreso-subir'),
        //arrWidths = [180, 100, 100, 95, 85, 210, 130, 36, 0.1, 0.1, 0.1, 0.1];
        arrWidths = [125, 95, 95, tamanioColumnaPotOfertada, 115, 270, 90, tamanioColumnaEliminar, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1];

    if (!!hstSubastaGlobalSubir) {
        hstSubastaGlobalSubir.destroy();
    }

    let tituloTabla = "Ingreso de Oferta de la Reserva para Subir";
    try {
        if (isEnIntervaloTemporal == 1) {
            tituloTabla = "Ingreso de Oferta Simétrica";
        }

    } catch {

    }

    ofertas.unshift([tituloTabla, '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
        ['URS', 'Horario', '', (tipoOferta == TIPO_OFERTA_DEFECTO ? 'Potencia de Referencia' : 'Potencia Ofertada'), 'Precio\n (S/. / MW-Mes)', 'Modo de Operación', 'Banda Calificada', '', '', '', '', '', '', ''],
        ['', 'Hora Inicio', 'Hora Fin', '', '', '', '', '', '', '', '', '', '', '']);

    var blnAbierto = (tipoOferta == TIPO_OFERTA_DEFECTO ? true : (tmeHoraActual >= tmeHoraInicio && tmeHoraActual <= tmeHoraFin));

    for (var i = 0, j = ofertas.length; i < j; i++) {
        for (var m = 0, n = urss.length; m < n; m++) {
            if (ofertas[i][intUrsColumnaGlobal] == urss[m].ID) {
                for (var x = 0, y = modosOperacion.length; x < y; x++) {
                    if (modosOperacion[x].UrsID == urss[m].ID && modosOperacion[x].ID == ofertas[i][5]) {
                        ofertas[i][posReservaFirme] = modosOperacion[x].EsReservaFirme;
                    }
                }
            }
        }
    }

    //console.log("mostrarOfertasInternal aperturaPorFecha", aperturaPorFecha);
    //console.log("mostrarOfertasInternal ofertas", ofertas);

    function calculateSizeSubir() {
        var offset = Handsontable.Dom.offset(container);

        //Verificar en que ventana esta, expandida -> 2 o extranet normal -> 1
        if (offset.top == 222) {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }
        else {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }

        if (offset.left > 0)
            availableWidth = $(window).width() - 2 * offset.left; //$("#divGeneral").width() - 50; //
        if (offset.top > 0) {
            availableHeight = HEIGHT_MINIMO; //availableHeight < HEIGHT_MINIMO ? HEIGHT_MINIMO : availableHeight;
            container.style.height = availableHeight + 'px';
        }
        if (hstSubastaGlobalSubir != undefined) {
            hstSubastaGlobalSubir.render();
        }
    }

    hstSubastaGlobalSubir = new Handsontable(container, {
        data: ofertas,
        //maxCols: 10,
        colHeaders: true,
        rowHeaders: true,
        addRow: false,
        colWidths: arrWidths,
        columnSorting: false,
        //contextMenu: contexMenuConfig,
        minSpareRows: minRow,
        height: HEIGHT_MINIMO,
        readOnly: lectura,// (blnAbierto ? lectura : true),

        columns: [
            { className: 'htCenter' },
            { className: 'htCenter' },
            { className: 'htCenter' },
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {}
        ],
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row == 0) {
                cellProperties.renderer = titleRendererIternal;
            }

            if (row == 1 || row == 2) {
                cellProperties.renderer = headerRendererInternal;
            }

            if (row >= intFilaInicioGlobal) {

                var arrData = this.instance.getData(),
                    intModoOperacionIndiceActual = (arrData[row][posIndice] !== null ? arrData[row][posIndice] : 0),
                    strUrsActual = arrData[row - intModoOperacionIndiceActual][intUrsColumnaGlobal],
                    intUrsActual = (strUrsActual !== null && ($.isNumeric(strUrsActual) || strUrsActual.length > 0) ? parseInt(strUrsActual) : null),
                    intModoOperacionActual = (arrData[row][5] !== null ? arrData[row][5] : null),
                    strHoraInicio = arrData[row - intModoOperacionIndiceActual][intHoraInicioColumnaGlobal],
                    strHoraFin = arrData[row - intModoOperacionIndiceActual][intHoraFinColumnaGlobal];

                switch (col) {
                    case 0: //URS
                        cellProperties.readOnly = lectura;
                        cellProperties.renderer = ursRendererInternalSubir;
                        cellProperties.editor = false;
                        cellProperties.source = urss;
                        break;
                    case 1: //HORA INICIO
                        cellProperties.readOnly = lecturaColHora;
                        cellProperties.renderer = horaRendererInternal;
                        cellProperties.editor = HoraInicioEditor;
                        cellProperties.source = horas;
                        break;
                    case 2: //HORA FIN
                        cellProperties.readOnly = lecturaColHora;
                        cellProperties.renderer = horaRendererInternal;
                        cellProperties.editor = HoraFinEditor;
                        cellProperties.source = horas;
                        break;
                    case 3: //POTENCIA OFERTADA
                        //var ocultar = TIPO_OFERTA_GLOBAL == 0 ? true : lectura
                        cellProperties.readOnly = lectura;
                        cellProperties.renderer = potenciaOfertadaRendererInternal;

                        //if (intUrsActual != null) {
                        //    var blnEsReservaFirme = arrData[row - intModoOperacionIndiceActual][11];

                        //    cellProperties.editor = NumericEditor;
                        //    cellProperties.readOnly = (TIPO_OFERTA_GLOBAL == 0 ? true : (blnEsReservaFirme ? true : aperturaPorFecha));
                        //} else {
                        //    cellProperties.editor = false;
                        //    cellProperties.readOnly = true;
                        //}

                        //cellProperties.type = "numeric";
                        cellProperties.type = "text";
                        break;
                    case 4: //PRECIO
                        cellProperties.readOnly = lectura;
                        cellProperties.renderer = precioRendererInternal;

                        //if (intUrsActual != null) {
                        //    cellProperties.editor = NumericEditor;
                        //    cellProperties.readOnly = !blnAbierto;
                        //} else {
                        //    cellProperties.editor = false;
                        //    cellProperties.readOnly = true;
                        //}

                        //cellProperties.type = "numeric";
                        cellProperties.type = "text";
                        break;
                    case 5: //MODO OPERACION
                        cellProperties.readOnly = true;
                        cellProperties.renderer = modoOperacionRendererInternal;
                        cellProperties.editor = false;
                        cellProperties.source = ARR_MODO_OPERACION_GLOBAL;
                        break;
                    case 6: //BANDA CALIFICADA POTENCIA MAXIMA

                        var blnReadOnly = true;

                        if (intUrsActual != null &&
                            (strHoraInicio !== null && strHoraInicio.length > 0) && (strHoraFin !== null && strHoraFin.length > 0)) {

                            var intHoraInicio = parseInt(strHoraInicio.replace(':', '')),
                                intHoraFin = parseInt(strHoraFin.replace(':', '')),
                                arrModoOperacion = this.instance.getCellMeta(row - intModoOperacionIndiceActual, 5).source;

                            if (TIPO_OFERTA_GLOBAL == 1) // Tipo Oferta
                                for (var i = 0, j = arrModoOperacion.length; i < j; i++) {
                                    var objModoOperacion = arrModoOperacion[i];


                                    if (objModoOperacion.UrsID === intUrsActual && objModoOperacion.ID == intModoOperacionActual) {

                                        if (objModoOperacion.IntvMant != null) {

                                            for (var k = 0, l = objModoOperacion.IntvMant.length / 2; k < l; k++) {
                                                var intInicioMan = parseInt(objModoOperacion.IntvMant[k, 0].replace(':', '')),
                                                    intFinMan = parseInt(objModoOperacion.IntvMant[k, 1].replace(':', ''));

                                                if (
                                                    (intHoraInicio >= intInicioMan && intHoraFin < intFinMan)
                                                    || (intHoraInicio < intInicioMan && intHoraFin > intInicioMan)
                                                    || (intHoraInicio < intFinMan && intHoraFin > intFinMan)
                                                ) {
                                                    blnReadOnly = true;
                                                    break;
                                                }
                                            }
                                        }

                                        break;
                                    }
                                }


                        } else {
                            blnReadOnly = true;
                        }

                        if (!blnReadOnly) {
                            cellProperties.editor = NumericEditor;
                            cellProperties.readOnly = !blnAbierto;
                        } else {
                            cellProperties.editor = false;
                            cellProperties.readOnly = true;
                        }

                        cellProperties.readOnly = true;
                        cellProperties.renderer = bandaCalificadaRendererInternal;

                        cellProperties.type = "numeric";
                        cellProperties.format = '0,0.00';
                        break;

                    case 7: //BOTON
                        cellProperties.editor = false;
                        cellProperties.renderer = eliminarRendererInternal;

                        break;
                }
            }
            else {
                cellProperties.editor = false;
            }

            return cellProperties;
        },
        mergeCells: [
            { row: 0, col: 0, rowspan: 1, colspan: 7 },
            //{ row: 0, col: 6, rowspan: 1, colspan: 1 },
            { row: 1, col: 0, rowspan: 2, colspan: 1 },
            { row: 1, col: 1, rowspan: 1, colspan: 2 },
            { row: 1, col: 3, rowspan: 2, colspan: 1 },//POTENCIA
            { row: 1, col: 4, rowspan: 2, colspan: 1 },//PRECIO
            { row: 1, col: 5, rowspan: 2, colspan: 1 },//MO
            { row: 1, col: 6, rowspan: 2, colspan: 1 },//BANDA CALIFICADA
            //{ row: 1, col: 7, rowspan: 2, colspan: 1 }
        ]
    });
    calculateSizeSubir();
    hstSubastaGlobalSubir.addHook('afterChange', function (changes, source) {
        if (changes && source == 'edit') {
            for (var i = 0, j = changes.length; i < j; i++) {
                //alert('afterChange - sincondicional ');
                if (changes[i][1] == intHoraInicioColumnaGlobal || changes[i][1] == intHoraFinColumnaGlobal) {
                    //alert('afterChange - !!! ');
                    hstSubastaGlobalSubir.setCellMeta(changes[i][0], changes[i][1], 'valid', true);
                    $(hstSubastaGlobalSubir.getCell(changes[i][0], changes[i][1])).removeClass('htInvalid');
                }
            }
            //hstSubastaGlobalSubir.render();
        }
    });

    hstSubastaGlobalSubir.addHook('afterCreateRow', function (index, amount, source) {
        updateDimensionHandson('hst-subasta-ingreso-subir', hstSubastaGlobalSubir);
    });
    hstSubastaGlobalSubir.addHook('afterRemoveRow', function (index, amount, source) {
        updateDimensionHandson('hst-subasta-ingreso-subir', hstSubastaGlobalSubir);
    });
}

function mostrarOfertasInternalBajar(tipoOferta, urss, modosOperacion, horas, ofertas, lectura, minRow) {
    ofertas = (ofertas === null ? [] : ofertas);
    lectura = (lectura === undefined ? false : lectura);

    //cuando sea Oferta por defecto, las columnas de "Hora inicio" y "Hora fin" son solo lectura
    var lecturaColHora = lectura;
    if (TIPO_OFERTA_GLOBAL == TIPO_OFERTA_DEFECTO) lecturaColHora = true;

    var tamanioColumnaEliminar = lectura ? 0.1 : 42;
    var tamanioColumnaPotOfertada = tipoOferta == TIPO_OFERTA_DEFECTO ? 0.1 : 95;
    var container = document.getElementById('hst-subasta-ingreso-bajar'),
        //arrWidths = [180, 100, 100, 95, 85, 210, 130, 36, 0.1, 0.1, 0.1, 0.1];
        arrWidths = [125, 95, 95, tamanioColumnaPotOfertada, 115, 270, 90, tamanioColumnaEliminar, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1];

    if (!!hstSubastaGlobalBajar) {
        hstSubastaGlobalBajar.destroy();
    }


    ofertas.unshift(['Ingreso de Oferta de la Reserva para Bajar', '', '', '', '', '', '', '', '', '', '', '', '', ''],
        ['URS', 'Horario', '', (tipoOferta == TIPO_OFERTA_DEFECTO ? 'Potencia de Referencia' : 'Potencia Ofertada'), 'Precio\n (S/. / MW-Mes)', 'Modo de Operación', 'Banda Calificada', '', '', '', '', '', '', ''],
        ['', 'Hora Inicio', 'Hora Fin', '', '', '', '', '', '', '', '', '', '', '']);

    var blnAbierto = (tmeHoraActual >= tmeHoraInicio && tmeHoraActual <= tmeHoraFin);

    for (var i = 0, j = ofertas.length; i < j; i++) {
        for (var m = 0, n = urss.length; m < n; m++) {
            if (ofertas[i][intUrsColumnaGlobal] == urss[m].ID) {
                for (var x = 0, y = modosOperacion.length; x < y; x++) {
                    if (modosOperacion[x].UrsID == urss[m].ID && modosOperacion[x].ID == ofertas[i][5]) {
                        ofertas[i][posReservaFirme] = modosOperacion[x].EsReservaFirme;
                    }
                }
            }
        }
    }
    //console.log("mostrarOfertasInternal aperturaPorFecha", aperturaPorFecha);
    //console.log("mostrarOfertasInternal ofertas", ofertas);

    function calculateSizeBajar() {
        var offset = Handsontable.Dom.offset(container);

        //Verificar en que ventana esta, expandida -> 2 o extranet normal -> 1
        if (offset.top == 222) {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }
        else {
            availableHeight = $(window).height() - 140 - offset.top - 20;
        }

        if (offset.left > 0)
            availableWidth = $(window).width() - 2 * offset.left; //$("#divGeneral").width() - 50; //
        if (offset.top > 0) {
            availableHeight = availableHeight < HEIGHT_MINIMO ? HEIGHT_MINIMO : availableHeight;
            container.style.height = availableHeight + 'px';
        }
        if (hstSubastaGlobalBajar != undefined) {
            hstSubastaGlobalBajar.render();
        }
    }

    hstSubastaGlobalBajar = new Handsontable(container, {
        data: ofertas,
        //maxCols: 10,
        colHeaders: true,
        rowHeaders: true,
        addRow: false,
        colWidths: arrWidths,
        columnSorting: false,
        //contextMenu: contexMenuConfig,
        minSpareRows: minRow,
        height: HEIGHT_MINIMO,
        columns: [
            { className: 'htCenter' },
            { className: 'htCenter' },
            { className: 'htCenter' },
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {},
            {}
        ],

        readOnly: lectura,// (blnAbierto ? lectura : true),
        cells: function (row, col, prop) {
            var cellProperties = {};

            if (row == 0) {
                cellProperties.renderer = titleRendererIternal;
            }

            if (row == 1 || row == 2) {
                cellProperties.renderer = headerRendererInternal;
            }

            if (row >= intFilaInicioGlobal) {

                var arrData = this.instance.getData(),
                    intModoOperacionIndiceActual = (arrData[row][posIndice] !== null ? arrData[row][posIndice] : 0),
                    strUrsActual = arrData[row - intModoOperacionIndiceActual][intUrsColumnaGlobal],
                    intUrsActual = (strUrsActual !== null && ($.isNumeric(strUrsActual) || strUrsActual.length > 0) ? parseInt(strUrsActual) : null),
                    intModoOperacionActual = (arrData[row][5] !== null ? arrData[row][5] : null),
                    strHoraInicio = arrData[row - intModoOperacionIndiceActual][intHoraInicioColumnaGlobal],
                    strHoraFin = arrData[row - intModoOperacionIndiceActual][intHoraFinColumnaGlobal];

                switch (col) {
                    case 0: //URS
                        cellProperties.readOnly = lectura;
                        cellProperties.renderer = ursRendererInternalSubir;
                        cellProperties.editor = false;
                        cellProperties.source = urss;
                        break;
                    case 1: //HORA INICIO
                        cellProperties.readOnly = lecturaColHora;
                        cellProperties.renderer = horaRendererInternal;
                        cellProperties.editor = HoraInicioEditor;
                        cellProperties.source = horas;
                        break;
                    case 2: //HORA FIN
                        cellProperties.readOnly = lecturaColHora;
                        cellProperties.renderer = horaRendererInternal;
                        cellProperties.editor = HoraFinEditor;
                        cellProperties.source = horas;
                        break;
                    case 3: //POTENCIA OFERTADA
                        //var ocultar = TIPO_OFERTA_GLOBAL == 0 ? true : lectura
                        cellProperties.readOnly = lectura;
                        cellProperties.renderer = potenciaOfertadaRendererInternal;

                        //if (intUrsActual != null) {
                        //    var blnEsReservaFirme = arrData[row - intModoOperacionIndiceActual][11];

                        //    cellProperties.editor = NumericEditor;
                        //    cellProperties.readOnly = (TIPO_OFERTA_GLOBAL == 0 ? true : (blnEsReservaFirme ? true : aperturaPorFecha));
                        //} else {
                        //    cellProperties.editor = false;
                        //    cellProperties.readOnly = true;
                        //}

                        //cellProperties.type = "numeric";
                        cellProperties.type = "text";
                        break;
                    case 4: //PRECIO
                        cellProperties.readOnly = lectura;
                        cellProperties.renderer = precioRendererInternal;

                        //if (intUrsActual != null) {
                        //    cellProperties.editor = NumericEditor;
                        //    cellProperties.readOnly = !blnAbierto;
                        //} else {
                        //    cellProperties.editor = false;
                        //    cellProperties.readOnly = true;
                        //}

                        //cellProperties.type = "numeric";
                        cellProperties.type = "text";
                        break;
                    case 5: //MODO OPERACION
                        cellProperties.readOnly = true;
                        cellProperties.renderer = modoOperacionRendererInternal;
                        cellProperties.editor = false;
                        cellProperties.source = ARR_MODO_OPERACION_GLOBAL;
                        break;
                    case 6: //OPTENCIA MAXIMA
                        var blnReadOnly = true;

                        if (intUrsActual != null &&
                            (strHoraInicio !== null && strHoraInicio.length > 0) && (strHoraFin !== null && strHoraFin.length > 0)) {

                            var intHoraInicio = parseInt(strHoraInicio.replace(':', '')),
                                intHoraFin = parseInt(strHoraFin.replace(':', '')),
                                arrModoOperacion = this.instance.getCellMeta(row - intModoOperacionIndiceActual, 5).source;

                            if (TIPO_OFERTA_GLOBAL == 1) // Tipo Oferta
                                for (var i = 0, j = arrModoOperacion.length; i < j; i++) {
                                    var objModoOperacion = arrModoOperacion[i];


                                    if (objModoOperacion.UrsID === intUrsActual && objModoOperacion.ID == intModoOperacionActual) {

                                        if (objModoOperacion.IntvMant != null) {

                                            for (var k = 0, l = objModoOperacion.IntvMant.length / 2; k < l; k++) {
                                                var intInicioMan = parseInt(objModoOperacion.IntvMant[k, 0].replace(':', '')),
                                                    intFinMan = parseInt(objModoOperacion.IntvMant[k, 1].replace(':', ''));

                                                if (
                                                    (intHoraInicio >= intInicioMan && intHoraFin < intFinMan)
                                                    || (intHoraInicio < intInicioMan && intHoraFin > intInicioMan)
                                                    || (intHoraInicio < intFinMan && intHoraFin > intFinMan)
                                                ) {
                                                    blnReadOnly = true;
                                                    break;
                                                }
                                            }
                                        }

                                        break;
                                    }
                                }


                        } else {
                            blnReadOnly = true;
                        }

                        if (!blnReadOnly) {
                            cellProperties.editor = NumericEditor;
                            cellProperties.readOnly = !blnAbierto;
                        } else {
                            cellProperties.editor = false;
                            cellProperties.readOnly = true;
                        }

                        cellProperties.readOnly = true;
                        cellProperties.renderer = bandaCalificadaRendererInternal;

                        cellProperties.type = "numeric";
                        cellProperties.format = '0,0.00';
                        break;

                    case 7: //BOTON
                        cellProperties.editor = false;
                        cellProperties.renderer = eliminarRendererInternal;

                        break;
                }

            }
            else {
                cellProperties.editor = false;
            }

            return cellProperties;
        },
        mergeCells: [
            { row: 0, col: 0, rowspan: 1, colspan: 7 },
            //{ row: 0, col: 6, rowspan: 1, colspan: 1 },
            { row: 1, col: 0, rowspan: 2, colspan: 1 },
            { row: 1, col: 1, rowspan: 1, colspan: 2 },
            { row: 1, col: 3, rowspan: 2, colspan: 1 },
            { row: 1, col: 4, rowspan: 2, colspan: 1 },
            { row: 1, col: 5, rowspan: 2, colspan: 1 },
            { row: 1, col: 6, rowspan: 2, colspan: 1 },
            //{ row: 1, col: 7, rowspan: 2, colspan: 1 }
        ]
    });
    calculateSizeBajar();
    hstSubastaGlobalBajar.addHook('afterChange', function (changes, source) {
        if (changes && source == 'edit') {
            for (var i = 0, j = changes.length; i < j; i++) {
                //alert('afterChange - sincondicional ');
                if (changes[i][1] == intHoraInicioColumnaGlobal || changes[i][1] == intHoraFinColumnaGlobal) {
                    //alert('afterChange - !!! ');
                    hstSubastaGlobalBajar.setCellMeta(changes[i][0], changes[i][1], 'valid', true);
                    $(hstSubastaGlobalBajar.getCell(changes[i][0], changes[i][1])).removeClass('htInvalid');
                }
            }
            //hstSubastaGlobalBajar.render();
        }
    });

    hstSubastaGlobalBajar.addHook('afterCreateRow', function (index, amount, source) {
        updateDimensionHandson('hst-subasta-ingreso-bajar', hstSubastaGlobalBajar);
    });
    hstSubastaGlobalBajar.addHook('afterRemoveRow', function (index, amount, source) {
        updateDimensionHandson('hst-subasta-ingreso-bajar', hstSubastaGlobalBajar);
    });
}

var HoraEditor = Handsontable.editors.AutocompleteEditor.prototype.extend();

HoraEditor.prototype.init = function () {
    Handsontable.editors.AutocompleteEditor.prototype.init.apply(this, arguments);

    this.options = [];
}

HoraEditor.prototype.open = function () {
    var intUrs = this.instance.getDataAtCell(this.cellProperties.row, intUrsColumnaGlobal);

    if (intUrs !== null && ($.isNumeric(intUrs) || intUrs.length > 0)) {
        this.prepareOptions();

        var options = (intUrs !== null ? this.options : [[]]);

        if (options.length > 1) {
            Handsontable.editors.AutocompleteEditor.prototype.open.apply(this, arguments);

            $(this.TEXTAREA)
                .attr('data-inputmask', '\'mask\':\'99:99\', \'placeholder\':\'\'')
                .inputmask();
        }
    }
}

HoraEditor.prototype.queryChoices = function (query) {
    this.query = query;
    var options = this.options;

    if (typeof this.cellProperties.source == 'function') {
        var that = this;
        this.cellProperties.source(query, function (choices) {
            that.updateChoicesList(choices);
        });
    } else if (Array.isArray(this.cellProperties.source)) {
        var choices;
        if (!query || this.cellProperties.filter === false) {
            choices = options;
        } else {
            var filteringCaseSensitive = this.cellProperties.filteringCaseSensitive === true;
            var lowerCaseQuery = query.toLowerCase();
            choices = options.filter(function (choice) {
                if (filteringCaseSensitive) {
                    return choice.indexOf(query) != -1;
                } else {
                    return choice.toLowerCase().indexOf(lowerCaseQuery) != -1;
                }
            });
        }
        this.updateChoicesList(choices);
    } else {
        this.updateChoicesList([]);
    }
};

Handsontable.editors.HoraEditor = HoraEditor;
Handsontable.editors.registerEditor('horeEditor', HoraEditor);

var HoraInicioEditor = Handsontable.editors.HoraEditor.prototype.extend();

HoraInicioEditor.prototype.prepareOptions = function () {
    var options = [],
        instance = this.instance,
        row = this.cellProperties.row,
        arrDataSeleccionada = instance.getData(),
        strHoraInicioActual = arrDataSeleccionada[row][intHoraInicioColumnaGlobal],
        strHoraFinAnterior = (row > intFilaInicioGlobal ? arrDataSeleccionada[row - 1][intHoraFinColumnaGlobal] : null),
        intHoraInicioActual = (strHoraInicioActual === '' || strHoraInicioActual === null ? null : parseInt(strHoraInicioActual.replace(':', ''))),
        intHoraFinAnterior = (strHoraFinAnterior === '' || strHoraFinAnterior === null ? null : parseInt(strHoraFinAnterior.replace(':', '')));

    var strUrsActual = arrDataSeleccionada[row][intUrsColumnaGlobal],
        strUrsAnterior = (row > intFilaInicioGlobal ? arrDataSeleccionada[row - 1][intUrsColumnaGlobal] : null),
        intUrsActual = (strUrsActual !== null ? parseInt(strUrsActual) : null),
        intUrsAnterior = (strUrsAnterior !== null && ($.isNumeric(strUrsAnterior) || strUrsAnterior.length > 0) ? parseInt(strUrsAnterior) : null);

    if (intHoraInicioActual == null) {
        options.push('');
    }

    var blnTieneHoraAnterior = (row === intFilaInicioGlobal || intHoraFinAnterior != null || intUrsAnterior !== intUrsActual);

    if (blnTieneHoraAnterior) {
        var strHoraFinalActual = instance.getDataAtCell(row, intHoraFinColumnaGlobal),
            intHoraFinActual = (strHoraFinalActual === '' || strHoraFinalActual === null ? null : parseInt(strHoraFinalActual.replace(':', ''))),
            arrHoraInicioSeleccionadas = [],
            arrHoraFinSeleccionadas = [],
            arrUrsSeleccionadas = [];

        for (var i = 0, j = arrDataSeleccionada.length; i < j; i++) {
            var rowValues = arrDataSeleccionada[i],
                strHoraInicio = rowValues[intHoraInicioColumnaGlobal],
                intUrs = parseInt(rowValues[intUrsColumnaGlobal]);

            if (strHoraInicio != null && intUrs === intUrsActual) {
                arrHoraInicioSeleccionadas.push(parseInt(strHoraInicio.replace(':', '')));

                var strHoraFin = rowValues[intHoraFinColumnaGlobal];

                if (strHoraFin != null) {
                    arrHoraFinSeleccionadas.push(parseInt(strHoraFin.replace(':', '')));
                } else {
                    arrHoraFinSeleccionadas.push(null);
                }
            }
        }

        var intHoraDesde = 0;

        if (intHoraFinActual != null) {
            var arrHoraFinSeleccionadasOrdenDescendiente = arrHoraFinSeleccionadas.sort().reverse();

            for (var i = 0, j = arrHoraFinSeleccionadasOrdenDescendiente.length; i < j; i++) {
                var intHoraFinSeleccionada = arrHoraFinSeleccionadasOrdenDescendiente[i];

                if (intHoraFinSeleccionada < intHoraFinActual) {
                    intHoraDesde = intHoraFinSeleccionada;
                    break;
                }
            }
        }

        var arrHoras = this.cellProperties.source;

        for (var i = 0, j = arrHoras.length - 1; i < j; i++) {
            var strHora = arrHoras[i],
                blnMostrarEnCombo = true;

            if (arrHoraFinSeleccionadas.length > 0) {
                var intHora = parseInt(strHora.replace(':', ''));

                for (var m = 0, n = arrHoraInicioSeleccionadas.length; m < n; m++) {
                    var intHoraInicio = arrHoraInicioSeleccionadas[m],
                        intHoraFin = arrHoraFinSeleccionadas[m];

                    if (
                        (intHoraFinActual !== null && intHora >= intHoraFinActual)
                        || (intHoraFinActual === null && intHoraDesde <= intHoraInicio && intHora >= intHoraInicio && intHora < intHoraFin)
                        || (intHora < intHoraDesde)
                    ) {
                        blnMostrarEnCombo = false;
                        break;
                    }
                }
            }

            if (blnMostrarEnCombo) {
                options.push(strHora);
            }
        }
    }

    this.options = options;
}

Handsontable.editors.HoraEditor = HoraEditor;
Handsontable.editors.registerEditor('horaInicioEditor', HoraInicioEditor);

var HoraFinEditor = Handsontable.editors.HoraEditor.prototype.extend();

HoraFinEditor.prototype.prepareOptions = function () {
    var options = [],
        instance = this.instance,
        row = this.cellProperties.row,
        arrDataSeleccionada = instance.getData(),
        strHoraInicioActual = arrDataSeleccionada[row][intHoraInicioColumnaGlobal],
        strHoraFinActual = arrDataSeleccionada[row][intHoraFinColumnaGlobal],
        intHoraInicioActual = (strHoraInicioActual === '' || strHoraInicioActual === null ? null : parseInt(strHoraInicioActual.replace(':', ''))),
        intHoraFinActual = (strHoraFinActual === '' || strHoraFinActual === null ? null : parseInt(strHoraFinActual.replace(':', '')));

    var strUrsActual = arrDataSeleccionada[row][intUrsColumnaGlobal],
        intUrsActual = (strUrsActual !== null ? parseInt(strUrsActual) : null);

    if (intHoraFinActual == null) {
        options.push('');
    }

    if (intHoraInicioActual != null) {
        var arrHoraInicioSeleccionadas = [],
            arrDataSeleccionada = instance.getData();

        for (var i = 0, j = arrDataSeleccionada.length; i < j; i++) {
            var rowValues = arrDataSeleccionada[i],
                intUrs = parseInt(rowValues[intUrsColumnaGlobal]);

            if (i != row && intUrs === intUrsActual) {
                var strHoraInicio = rowValues[intHoraInicioColumnaGlobal];

                if (strHoraInicio != null) {
                    arrHoraInicioSeleccionadas.push(parseInt(strHoraInicio.replace(':', '')));
                }
            }
        }

        var intHoraHasta = 2359,
            arrHoraInicioSeleccionadasOrdenAscendente = arrHoraInicioSeleccionadas.sort();

        for (var i = 0, j = arrHoraInicioSeleccionadasOrdenAscendente.length; i < j; i++) {
            var intHoraInicioSeleccionada = arrHoraInicioSeleccionadasOrdenAscendente[i];

            if (intHoraInicioSeleccionada >= intHoraInicioActual) {
                intHoraHasta = intHoraInicioSeleccionada;
                break;
            }
        }

        var arrHoras = this.cellProperties.source;

        for (var i = 0, j = arrHoras.length; i < j; i++) {
            var strHora = arrHoras[i],
                intHora = parseInt(strHora.replace(':', ''));

            if (intHora > intHoraInicioActual && intHora <= intHoraHasta) {
                options.push(strHora);
            }
        }
    }

    this.options = options;
}

Handsontable.editors.registerEditor('horaFinEditor', HoraFinEditor);

var NumericEditor = Handsontable.editors.HandsontableEditor.prototype.extend();

NumericEditor.prototype.open = function () {
    Handsontable.editors.HandsontableEditor.prototype.open.apply(this, arguments);

    $(this.TEXTAREA)
        .attr('data-inputmask', '\'alias\': \'decimal\'')
        .inputmask();
}

Handsontable.editors.registerEditor('numericEditor', NumericEditor);

function titleRendererIternal(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    $(td).addClass('title');
}

function headerRendererInternal(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    $(td).addClass('header');
}

function ursRendererInternalSubir(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    var arrDataSeleccionada = instance.getData(),
        intModoOperacionIndiceActual = (arrDataSeleccionada[row][posIndice] !== null ? arrDataSeleccionada[row][posIndice] : 0);

    //Para evitar descuadres, las celdas precio y potencia deben estar bloquedas si no es elegido una URS 
    if (value !== null) {
        instance.getCellMeta(row, col + 3).readOnly = false;
        instance.getCellMeta(row, col + 4).readOnly = false;
    } else {
        instance.getCellMeta(row, col + 3).readOnly = true;
        instance.getCellMeta(row, col + 4).readOnly = true;
        //$(td).css('background', '#95180B');
    }

    if (intModoOperacionIndiceActual == 0 && row >= intFilaInicioGlobal) {
        var intModoOperacionIndiceAnterior = (row > intFilaInicioGlobal ? arrDataSeleccionada[row - 1][posIndice] + 1 : 0),
            strHoraInicioAnterior = (row > intFilaInicioGlobal ? arrDataSeleccionada[row - intModoOperacionIndiceAnterior][intHoraInicioColumnaGlobal] : null),
            strHoraFinAnterior = (row > intFilaInicioGlobal ? arrDataSeleccionada[row - intModoOperacionIndiceAnterior][intHoraFinColumnaGlobal] : null),
            intHoraInicioAnterior = (strHoraInicioAnterior !== null && strHoraInicioAnterior.length > 0 ? parseInt(strHoraInicioAnterior.replace(':', '')) : null),
            intHoraFinAnterior = (strHoraFinAnterior !== null && strHoraFinAnterior.length > 0 ? parseInt(strHoraFinAnterior.replace(':', '')) : null);

        if (row === intFilaInicioGlobal || (intHoraInicioAnterior !== null && intHoraFinAnterior !== null)) {
            var arrUrs = cellProperties.source,
                options = [];

            if (value !== null || instance.getSettings().readOnly) {
                ////celdas precio y potencia deben estar bloquedas si no se elegido URS
                //instance.getCellMeta(row, col + 3).readOnly = false;
                //instance.getCellMeta(row, col + 4).readOnly = false;

                $(instance.rootElement).find('> select').remove();

                for (var i = 0, j = arrUrs.length; i < j; i++) {
                    var objUrs = arrUrs[i];

                    if (objUrs.ID === parseInt(value)) {
                        $(td).html(objUrs.Text);

                        var t = setTimeout(function () {
                            if (arrDataSeleccionada[row] !== undefined) {
                                $(td).attr('rowspan', arrDataSeleccionada[row][posCantidad]);
                                clearTimeout(t);
                            }
                        }, 0);
                    }
                }
            } else {
                var tmrUrs = setTimeout(function () {
                    //celdas precio y potencia deben estar bloquedas si no se elegido URS
                    //instance.getCellMeta(row, col + 3).readOnly = true;
                    //instance.getCellMeta(row, col + 4).readOnly = true;

                    var width = Handsontable.Dom.outerWidth(td);
                    var height = Handsontable.Dom.outerHeight(td);
                    var rootOffset = Handsontable.Dom.offset(instance.rootElement);
                    var tdOffset = Handsontable.Dom.offset(td);

                    var ddlUrs = $('<select>');

                    ddlUrs
                        .css("width", "125px")
                        .css("height", "22px")
                        .css('position', 'absolute')
                        .css('top', tdOffset.top - rootOffset.top)
                        .css('left', tdOffset.left - rootOffset.left);

                    ddlUrs.append(new Option('', '', false, false));

                    //Listar urs disponibles (aquellos que tienen al menos una media hora disponible)
                    for (var i = 0, j = arrUrs.length; i < j; i++) {
                        var objUrs = arrUrs[i],
                            intTotalHorasPorUrs = 0;
                        for (var m = intFilaInicioGlobal, n = arrDataSeleccionada.length - 1; m < n; m++) {
                            if (objUrs.ID === parseInt(arrDataSeleccionada[m][intUrsColumnaGlobal])) {

                                var strHoraInicioSeleccionada = arrDataSeleccionada[m][intHoraInicioColumnaGlobal],
                                    strHoraFinalSeleccionada = arrDataSeleccionada[m][intHoraFinColumnaGlobal];

                                if ((strHoraInicioSeleccionada !== null && strHoraInicioSeleccionada.length > 0)
                                    && (strHoraFinalSeleccionada !== null && strHoraFinalSeleccionada.length > 0)) {

                                    intTotalHorasPorUrs += (parseInt(strHoraFinalSeleccionada.replace(':', '')) - parseInt(strHoraInicioSeleccionada.replace(':', '')));
                                } else {
                                    intTotalHorasPorUrs = 2359; //el día ya está completo para la URS
                                }
                            }
                        }

                        if (intTotalHorasPorUrs < 2359) {
                            ddlUrs.append(new Option(objUrs.Text, objUrs.ID, false, false));
                        }
                    }

                    $(td).empty();

                    var rootOffset = Handsontable.Dom.offset(instance.rootElement);
                    var tdOffset = Handsontable.Dom.offset(td);

                    ddlUrs
                        .css("width", "125px")
                        .css("height", "22px")
                        .css('position', 'absolute')
                        .css('top', tdOffset.top - rootOffset.top)
                        .css('left', tdOffset.left - rootOffset.left);

                    $(instance.rootElement).find('> select').remove();
                    $(instance.rootElement).append(ddlUrs);

                    ddlUrs.change(function () {
                        var strUrsSelected = $(this).find('option:selected').val(),
                            arrModoOperacion = instance.getCellMeta(row, col + 5).source,
                            arrUrsModoOperacion = [];

                        instance.setDataAtCell(row, col, strUrsSelected);

                        for (var i = 0, j = arrModoOperacion.length; i < j; i++) {
                            var objModoOperacion = arrModoOperacion[i];

                            if (objModoOperacion.UrsID == strUrsSelected) {
                                arrUrsModoOperacion.push(objModoOperacion);
                            }
                        }

                        if (arrUrsModoOperacion.length > 0) {

                            var bndDisponibleMax = null; //()
                            var bndAdjudicadaXURS = 0; //para conocer el total de banda adjudicada, para validar intervalo de potencia ofertada
                            var bndCalificadaXURS = 0;


                            for (var i = 0, j = arrUrsModoOperacion.length; i < j; i++) {
                                var objModoOperacion = arrUrsModoOperacion[i];

                                if (objModoOperacion.Indice > 0) {
                                    instance.alter('insert_row', null);
                                }

                                bndCalificadaXURS = objModoOperacion.BndCalificada; //todos los MO tienen la misma BCal

                                bndAdjudicadaXURS = objModoOperacion.BndAdjudicada;

                                instance.setDataAtCell(row + objModoOperacion.Indice, 5, objModoOperacion.ID);
                                //instance.setDataAtCell(row + objModoOperacion.Indice, 6, objModoOperacion.Pot);
                                instance.setDataAtCell(row + objModoOperacion.Indice, 6, objModoOperacion.BndCalificada);
                                //instance.setDataAtCell(row + objModoOperacion.Indice, 7, objModoOperacion.BndDisponible);
                                instance.setDataAtCell(row + objModoOperacion.Indice, posIndice, objModoOperacion.Indice);
                                instance.setDataAtCell(row + objModoOperacion.Indice, posCantidad, objModoOperacion.Cantidad);
                                instance.setDataAtCell(row + objModoOperacion.Indice, 10, objModoOperacion.Pot);
                                instance.setDataAtCell(row + objModoOperacion.Indice, posReservaFirme, objModoOperacion.EsReservaFirme);

                                //Potencia Referencia para OFERTAS DEFECTO 
                                //(va dentro del for, y no fuera, ya que cuando la URS tiene varios MO, el dato mostrado es de la ultima iteracion)
                                if (TIPO_OFERTA_GLOBAL == 0) {
                                    var bdaAdjudicada_ = bndAdjudicadaXURS != null ? bndAdjudicadaXURS : 0;
                                    var bdaCalificada_ = bndCalificadaXURS != null ? bndCalificadaXURS : 0;
                                    var valorPotencia = (bdaCalificada_ - bdaAdjudicada_) / 2;
                                    instance.setDataAtCell(row + objModoOperacion.Indice, 3, valorPotencia);
                                }
                            }

                            instance.setDataAtCell(row, posLibre1, bndDisponibleMax);
                            instance.setDataAtCell(row, posBandaAdjudicada, 1); //fila dato visible

                            //Asignar 00:00 y 23:59 cuando sea oferta por defecto
                            var arrData = instance.getData();
                            var strUrsActual = arrData[row][intUrsColumnaGlobal];
                            if (TIPO_OFERTA_GLOBAL == TIPO_OFERTA_DEFECTO && strUrsActual != null) {
                                instance.setDataAtCell(row, intHoraInicioColumnaGlobal, "00:00");
                                instance.setDataAtCell(row, intHoraFinColumnaGlobal, "23:59");
                            }
                        }

                        $(this).remove();
                    });

                    clearTimeout(tmrUrs);
                }, 0);
            }

        }
    } else {
        setTimeout(function () {
            $(td).css('display', 'none');
        }, 0);
    }

    if (cellProperties.readOnly) {
        $(td).css('background', '#eeeeee');
    }
    
}

function horaRendererInternal(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    var arrDataSeleccionada = instance.getData(),
        intModoOperacionIndiceActual = (arrDataSeleccionada[row][posIndice] !== null ? arrDataSeleccionada[row][posIndice] : 0);

    if (intModoOperacionIndiceActual == 0) {
        setTimeout(function () {
            if (arrDataSeleccionada[row] !== undefined) {
                $(td).attr('rowspan', arrDataSeleccionada[row][posCantidad]);
            }
        }, 0);

        var strUrsActual = instance.getDataAtCell(row, intUrsColumnaGlobal),
            intUrsActual = (strUrsActual !== null && ($.isNumeric(strUrsActual) || strUrsActual.length > 0) ? parseInt(strUrsActual) : null);

        if (intUrsActual !== null) {
            var blnMostrar = true;

            if (col === intHoraFinColumnaGlobal) {
                var strHoraInicio = instance.getDataAtCell(row, intHoraInicioColumnaGlobal);

                blnMostrar = (strHoraInicio !== null && strHoraInicio.length > 0);
            }

            //alert('mirar aqui');
            if (blnMostrar) {
                //alert('mostrar-....');
                var divArrow = $('<div>')
                    .addClass('htAutocompleteArrow htHoraInicio')
                    .html(String.fromCharCode(9660))
                    .append(divArrow);

                $(td)
                    .html(value ? value : '')
                    .addClass('htHoraInicio')
                    .append(divArrow);

                if (!instance.acArrowListener) {
                    var eventManager = Handsontable.eventManager(instance);

                    instance.acArrowListener = function (event) {
                        if ($(event.target).hasClass('htHoraInicio')) {
                            instance.view.wt.getSetting('onCellDblClick', null, new WalkontableCellCoords(row, col), td);
                        }
                    }

                    eventManager.addEventListener(instance.rootElement, 'mousedown', instance.acArrowListener);
                }
            }
        }

        //validamos el color de la celda
        if (row != instance.getData().length - 1) { //menos la ultima fila
            if (value == null) {
                $(td).css('border', '1px solid gray');
                $('#error-vacio').css('display', 'block');
            } else {
                $('#error-vacio').css('display', 'none');
            }
        }
    } else {
        setTimeout(function () {
            $(td).css('display', 'none');
        }, 0);
    }

    if (cellProperties.readOnly) {
        $(td).css('background', '#eeeeee');
    }
}

function precioRendererInternal(instance, td, row, col, prop, value, cellProperties) {
    //Como quitamos el numericRender para colorear las celdas, agregamos css para posicionarse a la derecha 
    $(td).addClass('htRight');

    var arrDataSeleccionada = instance.getData(),
        intModoOperacionIndiceActual = (arrDataSeleccionada[row][posIndice] !== null ? arrDataSeleccionada[row][posIndice] : 0);

    if (intModoOperacionIndiceActual == 0) {
        setTimeout(function () {
            if (arrDataSeleccionada[row] !== undefined) {
                $(td).attr('rowspan', arrDataSeleccionada[row][posCantidad]);
            }
        }, 0);

        //Handsontable.renderers.NumericRenderer.apply(this, arguments); //quito para que no renderice de tipo numerico

        value = (value == null || value === "" ? null : parseFloat(value).toFixed(2));

        $(td).html(value);

        //validamos el color de la celda
        if (row != instance.getData().length - 1) { //menos la ultima fila
            if (value == null) {
                $(td).css('border', '1px solid gray');
                $('#error-vacio').css('display', 'block');
            } else {
                $('#error-vacio').css('display', 'none');
            }

            if (isNaN(value)) {
                $(td).css('background', colorRojo);
                $('#error-no-numerico').css('display', 'block');
            } else {
                $('#error-no-numerico').css('display', 'none');
            }

            if (parseFloat(value) < 0) {
                $(td).css('background', colorNaranja);
                $('#error-inf-minimo').css('display', 'block');
            } else {
                $('#error-inf-minimo').css('display', 'none');
            }

            if (parseFloat(value) > precioMaximo) {
                $(td).css('background', colorAmarillo);
                $('#error-sup-maximo').css('display', 'block');
            } else {
                $('#error-sup-maximo').css('display', 'none');
            }
        }
    } else {
        setTimeout(function () {
            $(td).css('display', 'none');
        }, 0);
    }

    if (cellProperties.readOnly) {
        $(td).css('background', '#eeeeee');
    }
}

function potenciaOfertadaRendererInternal(instance, td, row, col, prop, value, cellProperties) {
    //Como quitamos el numericRender para colorear las celdas, agregamos css para posicionarse a la derecha 
    $(td).addClass('htRight');

    var arrDataSeleccionada = instance.getData(),
        intModoOperacionIndiceActual = (arrDataSeleccionada[row][posIndice] !== null ? arrDataSeleccionada[row][posIndice] : 0);

    if (intModoOperacionIndiceActual == 0) {
        if (arrDataSeleccionada[row] !== undefined && !!arrDataSeleccionada[row][posCantidad]) {
            setTimeout(function () {

                $(td).attr('rowspan', arrDataSeleccionada[row][posCantidad]);

            }, 0);

            if (cellProperties.readOnly) {
                td.style.background = '#EEE';
            }

            var blnEsReservaFirme = arrDataSeleccionada[row][posReservaFirme];

            //Handsontable.renderers.NumericRenderer.apply(this, arguments); //quito para que no renderice de tipo numerico

            value = (value == null || value === "" ? null : parseFloat(value).toFixed(2));

            $(td).html(value);
            if (parseInt(value, 10) < 0) {
                $(td).css('background', colorNaranja);
            }
        }

        //validamos el color de la celda
        if (row != instance.getData().length - 1) { //menos la ultima fila
            if (value == null) {
                $(td).css('border', '1px solid gray');
                $('#error-vacio').css('display', 'block');
            } else {
                $('#error-vacio').css('display', 'none');
            }

            if (isNaN(value)) {
                $(td).css('background', colorRojo);
                $('#error-no-numerico').css('display', 'block');
            } else {
                $('#error-no-numerico').css('display', 'none');
            }

            if (parseFloat(value) < 0) {
                $(td).css('background', colorNaranja);
                $('#error-inf-minimo').css('display', 'block');
            } else {
                $('#error-inf-minimo').css('display', 'none');
            }

            var bndCalificada = arrDataSeleccionada[row][6];

            //if (parseFloat(value) > bndCalificada) {
            if (parseFloat(value) < potenciaMinimoMan) {
                $(td).css('background', colorAmarillo);
                $('#error-sup-maximo').css('display', 'block');
            } else {
                $('#error-sup-maximo').css('display', 'none');
            }

        }
    } else {
        setTimeout(function () {
            $(td).css('display', 'none');
        }, 0);
    }
    //var fila = arrDataSeleccionada.length - 1;

    //if (row == fila) {
    //    $(td).html('');
    //}

    if (cellProperties.readOnly) {
        $(td).css('background', '#eeeeee');
    }
}

function modoOperacionRendererInternal(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    var arrDataSeleccionada = instance.getData(),
        intModoOperacionIndiceActual = (arrDataSeleccionada[row][posIndice] !== null ? arrDataSeleccionada[row][posIndice] : 0),
        strUrsActual = instance.getDataAtCell(row - intModoOperacionIndiceActual, intUrsColumnaGlobal),
        intUrsActual = (strUrsActual !== null ? parseInt(strUrsActual) : null);

    if (intUrsActual !== null) {
        var arrModoOperacion = cellProperties.source;

        if (value !== null) {
            for (var m = 0, n = arrModoOperacion.length; m < n; m++) {
                var objModoOperacion = arrModoOperacion[m];

                if (objModoOperacion.UrsID === intUrsActual && value === objModoOperacion.ID) {
                    $(td).html(objModoOperacion.Text);
                    break;
                }
            }
        }
    }

    if (cellProperties.readOnly) {
        $(td).css('background', '#eeeeee');
    }
}

function bandaCalificadaRendererInternal(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.NumericRenderer.apply(this, arguments);

    //Agrupar CELDAS
    var arrDataSeleccionada = instance.getData(),
        intModoOperacionIndiceActual = (arrDataSeleccionada[row][posIndice] !== null ? arrDataSeleccionada[row][posIndice] : 0);

    if (intModoOperacionIndiceActual == 0) {
        setTimeout(function () {
            if (arrDataSeleccionada[row] !== undefined) {
                $(td).attr('rowspan', arrDataSeleccionada[row][posCantidad]);
            }
        }, 0);
    }
    else {
        setTimeout(function () {
            $(td).css('display', 'none');
        }, 0);
    }

    //Fin Agrupar Celdas

    if (cellProperties.readOnly) {
        var arrDataSeleccionada = instance.getData(),
            intModoOperacionIndiceActual = (arrDataSeleccionada[row][posIndice] !== null ? arrDataSeleccionada[row][posIndice] : 0),
            strUrsActual = instance.getDataAtCell(row - intModoOperacionIndiceActual, intUrsColumnaGlobal),
            intUrsActual = (strUrsActual !== null ? parseInt(strUrsActual) : null);

        if (intUrsActual != null) {
            td.style.background = '#EEE';
        }
    }

    value = (value == null || value === "" ? null : parseFloat(value).toFixed(2));

    if (value === "-996.00" || value === "-997.00" || value === "-998.00" || value === "-999.00")
        value = "Error Potencia Maxima.Contactar Programacion-COES!";

    $(td).html(value);

    if (cellProperties.readOnly) {
        $(td).css('background', '#eeeeee');
    }
}



function eliminarRendererInternal(instance, td, row, col, prop, value, cellProperties) {
    Handsontable.renderers.TextRenderer.apply(this, arguments);

    var arrDataSeleccionada = instance.getData(),
        intModoOperacionIndiceActual = (arrDataSeleccionada[row][posIndice] !== null ? arrDataSeleccionada[row][posIndice] : 0);

    if (intModoOperacionIndiceActual == 0) {
        setTimeout(function () {
            if (arrDataSeleccionada[row] !== undefined) {
                $(td).attr('rowspan', arrDataSeleccionada[row][posCantidad]);
            }
        }, 0);

        if (!instance.getSettings().readOnly) {
            var btnEliminar = $('<input type=\'button\'>')
                .val('X');

            btnEliminar
                .css('width', '11px')
                .css('text-align', 'center')
                .css('height', '16px')
                .css('margin-left', '0px');

            $(td)
                .html('')
                .append(btnEliminar);

            if (row !== instance.countRows() - 1) {
                btnEliminar.click(function () {
                    $(instance.rootElement).find('> select').remove();

                    for (i = arrDataSeleccionada[row][posCantidad] - 1, j = 0; i >= j; i--) {
                        instance.alter('remove_row', row + i);
                    }

                    setTimeout(function () {
                        instance.render();
                    }, 1);
                });
            }
        }
    }

    if (cellProperties.readOnly) {
        $(td).css('background', '#eeeeee');
    }
}

function updateDimensionHandson(idHandsontable, objHot) {
    var container = document.getElementById(idHandsontable)
    var hot = objHot;
    var offset = {};
    try {
        offset = Handsontable.Dom.offset(container);
    }
    catch (err) {
        console.log(err);
    }

    if (offset.length != 0) {
        var widthHT;
        var heightHT;

        if (offset.top == 222) {
            heightHT = $(window).height() - 140 - offset.top - 20;
        }
        else {
            heightHT = $(window).height() - 140 - offset.top - 20;
        }

        heightHT = HEIGHT_MINIMO; //heightHT < HEIGHT_MINIMO ? heightHT : HEIGHT_MINIMO;
        if (offset.left > 0 && offset.top > 0) {
            var numFilas = hot.getData().length;
            if (numFilas > 10)
                heightHT += ((numFilas - 10) * 30);

            widthHT = $(window).width() - 2 * offset.left; // $("#divGeneral").width() - 50; //
            hot.updateSettings({
                width: widthHT,
                height: heightHT
            });
        }
    }
}

// #endregion


// #region Mantenimientos

function mostrarMantenimiento(fecha) {
    $.ajax({
        type: 'POST',
        url: strControlador + "OfertaMantenimimentoListar",
        data: { fecha: fecha },
        success: function (arrMantenimiento) {
            mostrarMantenimientoInternal(arrMantenimiento);
        },
        error: function (req, status, error) {
        }
    });

    $.ajax({
        type: 'POST',
        url: strControlador + "OfertaMantenimimentoGrafico",
        data: { fecha: fecha },
        success: function (arrMantenimiento) {
            var arrSerie = [];
            for (var i = 0, j = arrMantenimiento.length; i < j; i++) {
                var dbvalor = arrMantenimiento[i];
                //console.log('i = ' + i + ' dbvalor = ' + dbvalor);
                arrSerie.push(dbvalor);
            }

            //console.log('pinta Grafico:' + ARR_HORAS_GLOBAL);
            //console.log('pinta Grafico:' + arrSerie);
            mostrarGraficoInternal(ARR_HORAS_GLOBAL, arrSerie);

        },
        error: function (req, status, error) {
        }
    });
}

function mostrarMantenimientoInternal(mantenimiento) {
    $('table#tbl-subasta-mantenimiento > tbody > tr').remove();
    var dblCapacidadMaximaRSF = 0.00;

    for (var i = 0, j = mantenimiento.length; i < j; i++) {
        var objMantenimiento = mantenimiento[i];

        var tdFecha = $('<td>').html(objMantenimiento.Fecha),
            tdURS = $('<td>').html(objMantenimiento.URS),
            tdModoOperacion = $('<td>').html(objMantenimiento.ModoOperacion),
            tdCapacidadMaximaRSF = $('<td>').html(objMantenimiento.CapacidadMaximaRSF.toFixed(2)),
            tdMantenimientoProgramado = $('<td>').html(objMantenimiento.MantenimientoProgramado == true ? 'SI' : 'NO'),
            tr = $('<tr>')
                .append(tdFecha)
                .append(tdURS)
                .append(tdModoOperacion)
                .append(tdCapacidadMaximaRSF)
                .append(tdMantenimientoProgramado);

        var tdIntervalo = $('<td>');

        if (objMantenimiento.IntervaloMantenimieto != null) {
            for (var m = 0, n = objMantenimiento.IntervaloMantenimieto.length / 2; m < n; m++) {
                tdIntervalo.append('<div>' + objMantenimiento.IntervaloMantenimieto[m, 0] + ' - ' + objMantenimiento.IntervaloMantenimieto[m, 1] + '</div>');
            }
        }

        tr.append(tdIntervalo);

        $('table#tbl-subasta-mantenimiento > tbody').append(tr);

        dblCapacidadMaximaRSF += objMantenimiento.CapacidadMaximaRSF;

    }
}

function mostrarGraficoInternal(horas, serie) {
    $('#gfx-subasta-potenciamax').highcharts({
        credits: {
            enabled: false
        },
        chart: {
            type: 'line'
        },
        title: {
            text: 'Capacidad Máxima para RSF'
        },
        xAxis: {
            categories: horas,
            labels: {
                rotation: 90
            }
        },
        yAxis: {
            title: {
                text: 'Capacidad Máxima'
            },
            labels: {
                format: '{value:.2f}'
            }

        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: false
                },
                marker: {
                    enabled: false
                },
                enableMouseTracking: false,
                showInLegend: false
            }
        },
        series: [{
            data: serie
        }]
    });
}

// #endregion


// #region Funciones antiguas

function foo() {


    //$('#btnExportar')
    //    .attr('disabled', 'disabled')
    //    .css("color", 'darkgray')
    //    .click(function () {
    //        $('#mensajeRegistro').html('');
    //        exportar();
    //    });

    //var uploader = new plupload.Uploader({
    //    runtimes: 'html5,flash,silverlight,html4',
    //    browse_button: 'btn-excel-cargar',
    //    url: strControlador + 'cargarexceloferta',
    //    flash_swf_url: '../js/Moxie.swf',
    //    silverlight_xap_url: '../js/Moxie.xap',
    //    filters: {
    //        max_file_size: '10mb',
    //        mime_types: [
    //            { title: "Archivos de Excel", extensions: "xls,xlsm,xlsx" }
    //        ]
    //    },
    //    init: {
    //        FilesAdded: function (up, files) {
    //            plupload.each(files, function (file) {
    //                uploader.start();
    //            });
    //        },
    //        FileUploaded: function (up, file, info) {
    //            var arrOfertas = JSON.parse(info.response);

    //            var arrOfferSubir = arrOfertas.slice(0);
    //            var arrOfferBajar = arrOfertas.slice(0);

    //            //mostrarOfertasInternal(TIPO_OFERTA_GLOBAL, ARR_URS_GLOBAL, ARR_MODO_OPERACION_GLOBAL, ARR_HORAS_GLOBAL, arrOfertas, false, 0); 
    //            mostrarOfertasInternalSubir(TIPO_OFERTA_GLOBAL, ARR_URS_GLOBAL, ARR_MODO_OPERACION_GLOBAL, ARR_HORAS_GLOBAL, arrOfferSubir, false, 0);
    //            mostrarOfertasInternalBajar(TIPO_OFERTA_GLOBAL, ARR_URS_GLOBAL, ARR_MODO_OPERACION_GLOBAL, ARR_HORAS_GLOBAL, arrOfferBajar, false, 0);

    //            //$('#mensajeRegistro').html('El archivo ha subio satisfactoriamente.');

    //            limpiarError();
    //            validarOfertasInternal(TIPO_OFERTA_GLOBAL, hstSubastaGlobal, hojaNeutro);
    //            hstSubastaGlobal.render();
    //            //mostrarErrores();
    //        },
    //        Error: function (up, err) {
    //            $('#mensajeRegistro').html('El archivo \'<b>' + up.files[0].name + '</b>\' no tiene el formato correcto.');
    //        }
    //    }
    //});
    //uploader.init();

    //function mostrarOfertasInternal(tipoOferta, urss, modosOperacion, horas, ofertas, lectura,minRow) {  
    //    ofertas = (ofertas === null ? [] : ofertas);  
    //    lectura = (lectura === undefined ? false : lectura);  

    //    var container = document.getElementById('hst-subasta-ingreso'),  
    //        arrWidths = [180, 100, 100, 100, 100, 210, 160, 36, 0.1, 0.1, 0.1, 0.1];  

    //    if (!!hstSubastaGlobal) { 
    //        hstSubastaGlobal.destroy(); 
    //    }

    //    ofertas.unshift(['Ingreso de Oferta para Subasta RSF', '', '', '', '', '', '', '', '', '', '', ''],
    //        ['URS', 'Horario', '', 'Precio (S/. / MW-Mes)', (tipoOferta == 0 ? 'Potencia de Referencia' : 'Potencia Ofertada'), 'Modo de Operación', 'Potencia Maxima', '', '', '', '', ''],
    //        ['', 'Hora Inicio', 'Hora Fin', '', '', '', '', '', '', '', '', '']);

    //    var blnAbierto = (tipoOferta == 0 ? true : isNow && (tmeHoraActual >= tmeHoraInicio && tmeHoraActual <= tmeHoraFin));

    //    for (var i = 0, j = ofertas.length; i < j; i++) {
    //        for (var m = 0, n = urss.length; m < n; m++) {
    //            if (ofertas[i][intUrsColumnaGlobal] == urss[m].ID) {
    //                for (var x = 0, y = modosOperacion.length; x < y; x++) {
    //                    if (modosOperacion[x].UrsID == urss[m].ID && modosOperacion[x].ID == ofertas[i][5]) {
    //                        ofertas[i][11] = modosOperacion[x].EsReservaFirme;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    console.log("mostrarOfertasInternal aperturaPorFecha", aperturaPorFecha);
    //    console.log("mostrarOfertasInternal ofertas", ofertas);
    //    hstSubastaGlobal = new Handsontable(container, {
    //        data: ofertas,
    //        maxCols: 10,
    //        colHeaders: true,
    //        rowHeaders: true,
    //        addRow: false,
    //        colWidths: arrWidths,
    //        columnSorting: false,
    //        contextMenu: false,
    //        minSpareRows: minRow,

    //        readOnly: lectura,// (blnAbierto ? lectura : true),
    //        cells: function (row, col, prop) {
    //            var cellProperties = {};

    //            if (row == 0) {
    //                cellProperties.renderer = titleRendererIternal;
    //            }

    //            if (row == 1 || row == 2) {
    //                cellProperties.renderer = headerRendererInternal;
    //            }

    //            if (row >= intFilaInicioGlobal) {

    //                var arrData = this.instance.getData(),
    //                    intModoOperacionIndiceActual = (arrData[row][8] !== null ? arrData[row][8] : 0),
    //                    strUrsActual = arrData[row - intModoOperacionIndiceActual][intUrsColumnaGlobal],
    //                    intUrsActual = (strUrsActual !== null && ($.isNumeric(strUrsActual) || strUrsActual.length > 0) ? parseInt(strUrsActual) : null),
    //                    intModoOperacionActual = (arrData[row][5] !== null ? arrData[row][5] : null),
    //                    strHoraInicio = arrData[row - intModoOperacionIndiceActual][intHoraInicioColumnaGlobal],
    //                    strHoraFin = arrData[row - intModoOperacionIndiceActual][intHoraFinColumnaGlobal];

    //                switch (col) {
    //                    case 0:
    //                        cellProperties.renderer = ursRendererInternal;
    //                        cellProperties.editor = false;
    //                        cellProperties.source = urss;
    //                        break;
    //                    case 1:
    //                        cellProperties.renderer = horaRendererInternal;
    //                        cellProperties.editor = HoraInicioEditor;
    //                        cellProperties.source = horas;
    //                        break;
    //                    case 2:
    //                        cellProperties.renderer = horaRendererInternal;
    //                        cellProperties.editor = HoraFinEditor;
    //                        cellProperties.source = horas;
    //                        break;
    //                    case 3:
    //                        cellProperties.renderer = precioRendererInternal;

    //                        //if (intUrsActual != null) {
    //                        //    cellProperties.editor = NumericEditor;
    //                        //    cellProperties.readOnly = !blnAbierto;
    //                        //} else {
    //                        //    cellProperties.editor = false;
    //                        //    cellProperties.readOnly = true;
    //                        //}

    //                        cellProperties.type = "numeric";
    //                        break;
    //                    case 4:
    //                        cellProperties.renderer = potenciaOfertadaRendererInternal;

    //                        //if (intUrsActual != null) {
    //                        //    var blnEsReservaFirme = arrData[row - intModoOperacionIndiceActual][11];

    //                        //    cellProperties.editor = NumericEditor;
    //                        //    cellProperties.readOnly = (TIPO_OFERTA_GLOBAL == 0 ? true : (blnEsReservaFirme ? true : aperturaPorFecha));
    //                        //} else {
    //                        //    cellProperties.editor = false;
    //                        //    cellProperties.readOnly = true;
    //                        //}

    //                        cellProperties.type = "numeric";
    //                        break;
    //                    case 5:
    //                        cellProperties.renderer = modoOperacionRendererInternal;
    //                        cellProperties.editor = false;
    //                        cellProperties.readOnly = !blnAbierto;
    //                        cellProperties.source = ARR_MODO_OPERACION_GLOBAL;
    //                        break;
    //                    case 6:
    //                        cellProperties.renderer = bandaCalificadaRendererInternal;

    //                        var blnReadOnly = true;

    //                        if (intUrsActual != null &&
    //                            (strHoraInicio !== null && strHoraInicio.length > 0) && (strHoraFin !== null && strHoraFin.length > 0)) {

    //                            var intHoraInicio = parseInt(strHoraInicio.replace(':', '')),
    //                                intHoraFin = parseInt(strHoraFin.replace(':', '')),
    //                                arrModoOperacion = this.instance.getCellMeta(row - intModoOperacionIndiceActual, 5).source;

    //                            if (TIPO_OFERTA_GLOBAL == 1) // Tipo Oferta
    //                                for (var i = 0, j = arrModoOperacion.length; i < j; i++) {
    //                                    var objModoOperacion = arrModoOperacion[i];


    //                                    if (objModoOperacion.UrsID === intUrsActual && objModoOperacion.ID == intModoOperacionActual) {

    //                                        if (objModoOperacion.IntvMant != null) {

    //                                            for (var k = 0, l = objModoOperacion.IntvMant.length / 2; k < l; k++) {
    //                                                var intInicioMan = parseInt(objModoOperacion.IntvMant[k, 0].replace(':', '')),
    //                                                    intFinMan = parseInt(objModoOperacion.IntvMant[k, 1].replace(':', ''));

    //                                                if (
    //                                                    (intHoraInicio >= intInicioMan && intHoraFin < intFinMan)
    //                                                    || (intHoraInicio < intInicioMan && intHoraFin > intInicioMan)
    //                                                    || (intHoraInicio < intFinMan && intHoraFin > intFinMan)
    //                                                ) {
    //                                                    blnReadOnly = true;
    //                                                    break;
    //                                                }
    //                                            }
    //                                        }

    //                                        break;
    //                                    }
    //                                }


    //                        } else {
    //                            blnReadOnly = true;
    //                        }

    //                        if (!blnReadOnly) {
    //                            cellProperties.editor = NumericEditor;
    //                            cellProperties.readOnly = !blnAbierto;
    //                        } else {
    //                            cellProperties.editor = false;
    //                            cellProperties.readOnly = true;
    //                        }

    //                        cellProperties.type = "numeric";
    //                        cellProperties.format = '0,0.00';
    //                        break;
    //                    case 7:
    //                        cellProperties.editor = false;
    //                        cellProperties.renderer = eliminarRendererInternal;

    //                        break;
    //                }
    //            }
    //            else {
    //                cellProperties.editor = false;
    //            }

    //            return cellProperties;
    //        },
    //        mergeCells: [
    //            { row: 0, col: 0, rowspan: 1, colspan: 7 },
    //            { row: 0, col: 6, rowspan: 1, colspan: 1 },
    //            { row: 1, col: 0, rowspan: 2, colspan: 1 },
    //            { row: 1, col: 1, rowspan: 1, colspan: 2 },
    //            { row: 1, col: 3, rowspan: 2, colspan: 1 },
    //            { row: 1, col: 4, rowspan: 2, colspan: 1 },
    //            { row: 1, col: 5, rowspan: 2, colspan: 1 },
    //            { row: 1, col: 6, rowspan: 2, colspan: 1 },
    //            { row: 1, col: 7, rowspan: 2, colspan: 1 }
    //        ]
    //    });

    //    hstSubastaGlobal.addHook('afterChange', function (changes, source) {
    //        if (changes && source == 'edit') {
    //            for (var i = 0, j = changes.length; i < j; i++) {
    //                //alert('afterChange - sincondicional ');
    //                if (changes[i][1] == intHoraInicioColumnaGlobal || changes[i][1] == intHoraFinColumnaGlobal) {
    //                    //alert('afterChange - !!! ');
    //                    hstSubastaGlobal.setCellMeta(changes[i][0], changes[i][1], 'valid', true);
    //                    $(hstSubastaGlobal.getCell(changes[i][0], changes[i][1])).removeClass('htInvalid');
    //                }
    //            }
    //            //hstSubastaGlobal.render();
    //        } 
    //    });  
    //}  
}

// #endregion
