// scrips relacionados => "globales.js"
var controlador = siteRoot + 'IEOD/HorasOperacion/';
var evtHot;

//////////////////////////////////////////////////////////////////////////////////////////////////
/// Listado de horas de operación
//////////////////////////////////////////////////////////////////////////////////////////////////

function generaListado() {
    if ($("#txtFecha").val() != "") {
        sFecha = $('#txtFecha').val();
        fechaActual = new Date();

        hideMensaje();
        hideMensajeEvento();

        $("#hfIdEnvio").val(0);
        mostrarListadoHorasOperacion();
    }
    else {
        alert("Error!.Ingresar fecha correcta");
    }
}

function mostrarListadoHorasOperacion(opcion) {
    var idEmpresa = $('#cbEmpresa').val();
    var idTipoCentral = parseInt($('#cbTipoCentral').val()) || 0;
    var sFecha = $('#txtFecha').val()
    var idCentral = parseInt($('#cbCentral').val()) || 0;
    if (idTipoCentral == 0) {
        $('#listado').html("");
        $('#barraHorasOperacion').css("display", "none");
    }

    if (idTipoCentral != 5) //Hidraulicas -> No hay central
        idCentral = ID_CENTRAL_DEFAULT;
    var idEnvio = $("#hfIdEnvio").val();

    if (idTipoCentral != 0) {
        $.ajax({
            type: 'POST',
            url: controlador + "lista",
            dataType: 'json',
            data: {
                idEmpresa: idEmpresa,
                idTipoCentral: idTipoCentral,
                sFecha: sFecha,
                idCentral: idCentral,
                idEnvio: idEnvio
            },
            success: function (evt) {
                if (evt.error == undefined) {
                    evtHot = evt;

                    if (evt.MensajeNotifuniesp != null || evt.MensajeNotifuniesp != "") {
                        $(".msjNotificacionUniEsp").html(evt.MensajeNotifuniesp);
                        $(".msjNotificacionUniEsp").show();
                    }

                    //
                    switch (opcion) {
                        case OPCION_ENVIO_DATOS:
                            var mensaje = mostrarMensajeEnvioFuenteDatos(evt);
                            mostrarExito("Los datos se enviaron correctamente. " + mensaje);
                            break;
                        default:
                            var mensaje = mostrarMensajeEnvioFuenteDatos(evt);
                            mostrarMensaje(mensaje);
                            break;
                    }

                    //
                    $('#listado').css("width", $('#mainLayout').width() + "px");
                    if (idEnvio > 0 || evtHot.PlazoEnvio.TipoPlazo == "D") {
                        $('#listado').html(dibujarTablaHorasOperacion(evt, 3));
                        $('#btnAgregarHoraOperacion').css("display", "none");
                        $('#btnContinuarHoraOperacion').css("display", "none");
                        $('#btnEnviarDatos').css("display", "none");
                        $('#barraHorasOperacion').css("display", "table");
                    } else {
                        $('#listado').html(dibujarTablaHorasOperacion(evt, 2));
                        $('#barraHorasOperacion').css("display", "table");
                        $('#btnAgregarHoraOperacion').css("display", "table-cell");
                        $('#btnEnviarDatos').css("display", "table-cell");
                    }
                }
                else {
                    mostrarError("Error:" + evt.descripcion);
                }

                dibujarPanelIeod(tipoFormato, 5, -1);
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////
/// Envío de horas de operación
//////////////////////////////////////////////////////////////////////////////////////////////////

function enviarHorasOperacion() {
    if (confirm("¿Desea enviar información a COES?")) {
        hideMensaje();
        var empresa = $('#cbEmpresa').val();
        var fecha = $('#txtFecha').val();
        var idTipoCentral = parseInt($('#cbTipoCentral').val()) || 0;
        var idCentral = parseInt($('#cbCentral').val()) || 0;
        if (idTipoCentral != 5) //Hidraulicas -> No hay central
            idCentral = ID_CENTRAL_DEFAULT;
        var matriz = evtHot.ListaHorasOperacion;
        var confirmarInterv = parseInt($("#hfConfirmarValInterv").val()) || 0;

        $("#btnAcep").hide();
        $("#btnCancel").hide();
        $("#btnAceptar3").hide();
        $("#btnCancelar3").hide();

        $.ajax({
            type: 'POST',
            //async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            traditional: true,
            url: controlador + "RegistrarEnvioHorasOperacion",
            data: JSON.stringify({
                data: matriz,
                idEmpresa: empresa,
                tipoCentral: idTipoCentral,
                fecha: fecha,
                idCentral: idCentral,
                flagConfirmarInterv: confirmarInterv
            }),
            beforeSend: function () {
                mostrarExito("Enviando Información ..");
            },
            success: function (model) {
                if (model.Resultado == 1) {
                    offshowView();
                    $("#btnAcep").show();
                    $("#btnCancel").show();
                    $(".fila_val_intervenciones").hide();

                    $("#hfIdEnvio").val(model.PlazoEnvio.IdEnvio);
                    mostrarListadoHorasOperacion(OPCION_ENVIO_DATOS);
                    dibujarPanelIeod(tipoFormato, 5, -1);
                }
                else {
                    //Validación de intervenciones
                    if (model.Resultado == 2) {
                        $("#btnAceptar3").show();
                        $("#btnCancelar3").show();

                        alert('Se ha encontrado equipos que están en mantenimiento y fuera de servicio');
                        interv_mostrarAdvertencia(model.ListaHorasOperacion, model.ListaValidacionHorasOperacionIntervencion);
                    } else {
                        $("#btnAcep").show();
                        $("#btnCancel").show();
                        alert("Error al Grabar: " + model.Mensaje);
                    }
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

function verEnvioHorasOperacion(opcion) {
    var result = generaEnvioHorasOperacion(opcion, 3);
    $('#idEnvioHorasOperacion').html(result);
    setTimeout(function () {
        $('#enviosHorasOperacion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 50);
}

function grabarEnvioHorasOperacion() {
    $("#hfConfirmarValInterv").val(0);
    enviarHorasOperacion();
}
function grabarConfirmacionHorasOperacion() {
    $("#hfConfirmarValInterv").val(1);
    enviarHorasOperacion();
}

function popUpListaEnvios() {
    $('#idEnviosAnteriores').html(dibujarTablaEnviosHO(evtHot.ListaEnvios));
    setTimeout(function () {
        $('#enviosanteriores').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
        $('#tablalenvio').dataTable({
            "scrollY": 330,
            "scrollX": true,
            "sDom": 't',
            "ordering": false,
            "bPaginate": false,
            "iDisplayLength": -1
        });
    }, 50);
}

function mostrarEnvioHorasOperacion(envio) {
    $('#enviosanteriores').bPopup().close();
    $("#hfIdEnvio").val(envio);

    mostrarListadoHorasOperacion();
}

//////////////////////////////////////////////////////////////////////////////////////////////////
/// Funciones para cargar interfaz de Horas de Operación
//////////////////////////////////////////////////////////////////////////////////////////////////

/// Función que genera la vista para el ingreso de datos para crear o modificar un intervalo de horas de operación
/// <params>
/// name: opt -> 0: modificacion de datos, 1: nuevo registro
/// name: idEquipo -> codigo de equipo (Hidraulicas, Solares, Eolicas), código de Modo de operación (Térmicas)
///</params>
function view_FormularioHO(obj) {
    obj.IdTipoOperSelect = obj.IdTipoOperSelect == null ? -1 : obj.IdTipoOperSelect;

    obj.IdEmpresa = $('#cbEmpresa').val();
    obj.IdTipoCentral = $('#cbTipoCentral').val();
    obj.IdCentralSelect = $('#cbCentral').val();
    if (obj.IdCentralSelect == null)
        obj.IdCentralSelect = -1;
    obj.Fecha = $('#txtFecha').val();

    obj.UsuarioModificacion = obj.UsuarioModificacion != undefined ? obj.UsuarioModificacion : "";
    obj.FechaModificacion = obj.FechaModificacion != undefined ? obj.FechaModificacion : "";

    $('#idForHorasOperacion').html('');
    $.ajax({
        type: 'POST',
        traditional: true,
        async: false,
        url: controlador + 'ViewIngresoHorasOperacion',
        data: {
            objJsonForm: JSON.stringify(obj)
        },
        success: function (result) {
            view_HtmlInterfaz(obj, result);
        },
        error: function (err) {
            alert('ha ocurrido un error al generar vista');
        }
    });
}

function view_HtmlInterfaz(obj, result) {

    $('#idForHorasOperacion').html(result);

    view_ConfInicialRegistro();

    setTimeout(function () {
        $('#newHorasOperacion').bPopup({
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            modalClose: false
        });
    }, 450);
}

function view_ConfInicialRegistro() {
    $(".msjNotificacionEdit").html('');

    var notifUniEsp = parseInt($("#hfHopnotifuniesp").val()) || 0;
    var idModoGrupo = parseInt($('#hfIdModoGrupo').val()) || 0;
    var msjNotif = getTextoNotificacion(APP_OPCION, idModoGrupo, notifUniEsp);

    if (msjNotif != '') {
        $("#txtEnParaleloH").prop('disabled', 'disabled');
        $("#txtFueraParaleloH").prop('disabled', 'disabled');
        $(".msjNotificacionEdit").html(msjNotif);
        $(".msjNotificacionEdit").show();
    } else {
        $(".msjNotificacionEdit").hide();
    }

    ui_setInputmaskHora('#txtOrdenArranqueH');
    ui_setInputmaskHora('#txtOrdenParadaH');
    ui_setInputmaskHora('#txtFueraParaleloH');
    ui_setInputmaskHora('#txtEnParaleloH');

    $(OBJ_REFERENCIA.id_ref_txtOrdenArranqueH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_MODO) });
    $(OBJ_REFERENCIA.id_ref_txtEnParaleloH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_MODO) });
    $(OBJ_REFERENCIA.id_ref_txtOrdenParadaH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_MODO) });
    $(OBJ_REFERENCIA.id_ref_txtFueraParaleloH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_MODO); actualizartxtFueraParaleloF('#txtFueraParaleloH', '#txtFueraParaleloF') });

    document.getElementById("chkFueraServicio").checked = parseInt($('#hfOPFueraServ').val());
    document.getElementById("chkArranqueBlackStart").checked = parseInt($('#hfOpArranqBlackStart').val());
    document.getElementById("chkEnsayoPotenciaEfectiva").checked = parseInt($('#hfOpOpEnsayope').val());
    activarDesactivarCampos(document.getElementById("chkFueraServicio").checked);

    $('#cbCentral2').val($('#hfCentral').val());
    $('#cbTipoOp').val($('#hfTipoOerac').val());

    $("#btnCancelar2").click(function () {
        $('#newHorasOperacion').bPopup().close();
    });

    $("#cbTipoOp").click(function () {
        seteaCombosHijosunidades($(this).val());
    });

    $("#btnAceptar2").click(function () {
        ho_ValidarFormulario();
    });

    $('#cbCentral2').change(function () {
        if (TIPO_CENTRAL_HIDROELECTRICA == $('#hfTipoCentral').val() && APP_OPCION != OPCION_EDITAR) {
            $('.tr-grupo-modo').hide();
            $('.tr-tipo-operacion').hide();
            $('.unidades_modo').hide();
            cargarListadoUnidadesEspeciales($('#hfTipoCentral').val(), $('#cbEmpresa').val(), $("#cbCentral2").val());
        } else {
            cargarListaModo_Grupo($('#hfTipoCentral').val(), $('#hfIdModoGrupo').val(), -1);
        }
        view_EnabledOrDisabledUIHoraOperacion();
    });

    if (TIPO_CENTRAL_HIDROELECTRICA == $('#hfTipoCentral').val() && APP_OPCION != OPCION_EDITAR) {
        $('.tr-grupo-modo').hide();
        $('.tr-tipo-operacion').hide();
        $('.unidades_modo').hide();
        cargarListadoUnidadesEspeciales($('#hfTipoCentral').val(), $('#cbEmpresa').val(), $("#cbCentral2").val());
    } else {
        if ($("#hfIdTipoModOp").val() == "1") {
            $(".tr-grupo-modo").show();
        }
        cargarListaModo_Grupo($('#hfTipoCentral').val(), $('#hfIdModoGrupo').val(), $('#hfIdPos').val());
    }

    if ($('#hfTipoCentral').val() == 37 || $('#hfTipoCentral').val() == 39) { // si es solar o elolica
        setearFechasInicioFin($('#cbCentral2').val(), $('#hfIdPos').val());
    }

    $('#chkArranqueBlackStart').change(function () {
        incluirDescripcionBlackStart();
    });
}

function view_EnabledOrDisabledUIHoraOperacion() {
    var idModoGrupo = parseInt($('#cbModoOpGrupo').val()) || 0;

    if (APP_OPCION != OPCION_VER) {
        var notifUniEsp = parseInt($("#hfHopnotifuniesp").val()) || 0;

        if (idModoGrupo > 0) {
            if (glb_FlagModoEspecial(evtHot, idModoGrupo) == FLAG_HO_ESPECIAL
                || (notifUniEsp < FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION || notifUniEsp == FLAG_UNIDAD_ESPECIAL_AGENTE_MODIFICACION_FROM_ADMIN)) {
                $("#txtOrdenArranqueF").prop('disabled', 'disabled');
                $(OBJ_REFERENCIA.id_ref_txtOrdenArranqueH).prop('disabled', 'disabled');
                $(OBJ_REFERENCIA.id_ref_txtEnParaleloH).prop('disabled', 'disabled');
                $(OBJ_REFERENCIA.id_ref_txtOrdenParadaH).prop('disabled', 'disabled');
                $(OBJ_REFERENCIA.id_ref_txtFueraParaleloH).prop('disabled', 'disabled');
            } else {
                $("#txtOrdenArranqueF").removeAttr("disabled");
                $(OBJ_REFERENCIA.id_ref_txtOrdenArranqueH).removeAttr("disabled");
                $(OBJ_REFERENCIA.id_ref_txtEnParaleloH).removeAttr("disabled");
                $(OBJ_REFERENCIA.id_ref_txtOrdenParadaH).removeAttr("disabled");
                $(OBJ_REFERENCIA.id_ref_txtFueraParaleloH).removeAttr("disabled");
            }
        } else {
            $("#txtOrdenArranqueF").prop('disabled', 'disabled');
        }
        $("#chkArranqueBlackStart").removeAttr("disabled");
        $("#chkEnsayoPotenciaEfectiva").removeAttr("disabled");
    } else {
        $(OBJ_REFERENCIA.id_ref_txtOrdenArranqueH).prop('disabled', 'disabled');
        $(OBJ_REFERENCIA.id_ref_txtEnParaleloH).prop('disabled', 'disabled');
        $(OBJ_REFERENCIA.id_ref_txtOrdenParadaH).prop('disabled', 'disabled');
        $(OBJ_REFERENCIA.id_ref_txtFueraParaleloH).prop('disabled', 'disabled');
        $("#chkArranqueBlackStart").prop('disabled', 'disabled');
        $("#chkEnsayoPotenciaEfectiva").prop('disabled', 'disabled');
    }

    //Ocultar UI para Centrales de Reserva Fría
    $("#leyendaBlackStartCReservFria").hide();
    $("#leyendaGeneradoresCReservFria").hide();
    $("#chkArranqueBlackStart").hide();
    $("#txtArranqueBlackStart").hide();

    var flagRFGeneradores = parseInt($('#hfFlagCentralRsvFriaToRegistrarUnidad').val());

    if (idModoGrupo > 0) {
        var regModoOp = getModoFromListaModo(evtHot.ListaModosOperacion, idModoGrupo);
        if (regModoOp != null && regModoOp.Gruporeservafria == MODO_GRUPORESERVAFRIA) {
            $("#chkArranqueBlackStart").show();
            $("#txtArranqueBlackStart").show();
            if (APP_OPCION != OPCION_VER) {
                $("#leyendaBlackStartCReservFria").show();
                if (flagRFGeneradores == FLAG_GRUPORESERVAFRIA_TO_REGISTRAR_UNIDADES) {
                    $("#leyendaGeneradoresCReservFria").show();
                }
            } else {
            }
        }
    }

    //Ocultar UI para check Ensayo potencia efectiva
    mostrarCheckEnsayoPotenciaEfectiva();
}

function modificaHoraOperacion(idHopcodi) {
    var strObjForm = JSON.stringify(OBJ_DATA_HORA_OPERACION);
    var objForm = JSON.parse(strObjForm);

    var pos = -1; // indice de la posicion del arreglo de horas operacion
    // buscamos la hora de operacion

    if (evtHot.ListaHorasOperacion.length > 0) {
        for (var i = 0; i < evtHot.ListaHorasOperacion.length; i++) {
            if (evtHot.ListaHorasOperacion[i].Hopcodi == idHopcodi) {
                pos = i; // encontrado
            }
        }
    }
    objForm.IdPos = pos;

    //
    objForm.FechaIni = moment(evtHot.ListaHorasOperacion[pos].Hophorini).format('DD/MM/YYYY');
    objForm.HoraIni = moment(evtHot.ListaHorasOperacion[pos].Hophorini).format('HH:mm:ss');

    objForm.FechaFin = moment(evtHot.ListaHorasOperacion[pos].Hophorfin).format('DD/MM/YYYY');
    objForm.HoraFin = moment(evtHot.ListaHorasOperacion[pos].Hophorfin).format('HH:mm:ss');

    objForm.Fechahorordarranq = objForm.FechaIni;
    if (evtHot.ListaHorasOperacion[pos].Hophorordarranq != null && evtHot.ListaHorasOperacion[pos].Hophorordarranq != "") {
        objForm.Fechahorordarranq = moment(evtHot.ListaHorasOperacion[pos].Hophorordarranq).format('DD/MM/YYYY');
        objForm.Hophorordarranq = moment(evtHot.ListaHorasOperacion[pos].Hophorordarranq).format('HH:mm:ss');
    }

    objForm.FechaHophorparada = objForm.FechaIni;
    if (evtHot.ListaHorasOperacion[pos].Hophorparada != null && evtHot.ListaHorasOperacion[pos].Hophorparada != "") {
        objForm.FechaHophorparada = moment(evtHot.ListaHorasOperacion[pos].Hophorparada).format('DD/MM/YYYY');
        objForm.Hophorparada = moment(evtHot.ListaHorasOperacion[pos].Hophorparada).format('HH:mm:ss');
    }

    objForm.IdTipoOperSelect = evtHot.ListaHorasOperacion[pos].Subcausacodi;

    if (evtHot.IdTipoCentral == TIPO_CENTRAL_TERMOELECTRICA) {
        objForm.IdEquipoOrIdModo = evtHot.ListaHorasOperacion[pos].Grupocodi;
    } else {
        objForm.IdEquipoOrIdModo = evtHot.ListaHorasOperacion[pos].Equicodi;
    }

    if (evtHot.ListaHorasOperacion[pos].Hopfalla == "F") {
        objForm.OpFueraServ = 1;
    }

    if (evtHot.ListaHorasOperacion[pos].Hoparrqblackstart == 'S') {
        objForm.OpArranqBlackStart = 1;
    }

    if (evtHot.ListaHorasOperacion[pos].Hopensayope == 'S') {
        objForm.OpEnsayope = 1;
    }

    objForm.Hopnotifuniesp = evtHot.ListaHorasOperacion[pos].Hopnotifuniesp;
    objForm.Hopobs = evtHot.ListaHorasOperacion[pos].Hopobs;

    objForm.MatrizunidadesExtra = [];
    if (evtHot.IdTipoCentral == TIPO_CENTRAL_TERMOELECTRICA) {
        objForm.MatrizunidadesExtra = evtHot.ListaHorasOperacion[pos].MatrizunidadesExtra;
        if (objForm.MatrizunidadesExtra === undefined || objForm.MatrizunidadesExtra == null)
            objForm.MatrizunidadesExtra = [];
    }

    view_FormularioHO(objForm);
}


//elimina un rango de horas para un equipo selecionado
function eliminarHorasDeOperacion(idHopcodi) {
    if (confirm("¿Desea eliminar hora de operación?")) {
        if (evtHot.ListaHorasOperacion.length > 0) {

            switch (evtHot.IdTipoCentral) {
                case 4: //Hidraulicas  
                case 37: //Solares
                case 39: //Eolicas
                    for (var i = 0; i < evtHot.ListaHorasOperacion.length; i++) {
                        if (evtHot.ListaHorasOperacion[i].Hopcodi == idHopcodi) { // encontrado
                            if (evtHot.ListaHorasOperacion[i].Hopcodi > 0) { //si viene de base de datos
                                evtHot.ListaHorasOperacion[i].OpcionCrud = -1; // eliminado lógico 
                            }
                            else {
                                evtHot.ListaHorasOperacion.splice(i, 1); //eliminado fisico de la matriz auxiliar
                            }
                        }
                    }

                    break;
                case 5: //Térmicas                      
                    for (var i = evtHot.ListaHorasOperacion.length - 1; i >= 0; i--) {
                        if (evtHot.ListaHorasOperacion[i].Hopcodi == idHopcodi || evtHot.ListaHorasOperacion[i].Hopcodipadre == idHopcodi) {
                            if (evtHot.ListaHorasOperacion[i].Hopcodi > -1) { // viene de base de datos
                                evtHot.ListaHorasOperacion[i].OpcionCrud = -1; // eliminado lógico
                            }
                            else {
                                evtHot.ListaHorasOperacion.splice(i, 1); //eliminado fisico de la matriz auxiliar                                
                            }
                        }
                    }
                    break;
            }
        }
    }
}

function actualizaListadoHorasDeOperacion(evt) {
    $('#listado').css("width", $('#mainLayout').width() + "px");
    $('#listado').html(dibujarTablaHorasOperacion(evt, 2));

    //verificar si luego de los cambios existe Notificacion a mostrar
    var contador = 0;
    if (evtHot.ListaHorasOperacion.length > 0) {
        for (var zz = 0; zz < evtHot.ListaHorasOperacion.length; zz++) {
            if (evtHot.ListaHorasOperacion[zz].Hopnotifuniesp < FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION) {
                contador++;
            }
        }
    }

    if (contador == 0) {
        $(".msjNotificacionUniEsp").hide();
    } else {
        $(".msjNotificacionUniEsp").show();
    }
}

function cargarTipoCentral() {
    var idEmpresa = $('#cbEmpresa').val();
    var fecha = $('#txtFecha').val();

    $.ajax({
        type: 'POST',
        url: controlador + 'CargarTipoCentral',

        data: {
            idEmpresa: idEmpresa,
            fecha: fecha
        },

        success: function (aData) {
            $('#tipocentral').html(aData);
            if ($('#cbTipoCentral').val() == 5) {
                cargarCentral();
            }
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarCentral() {
    var idTipoCentral = $('#cbTipoCentral').val();
    if (idTipoCentral == 4)
        mostrarOcultarCentral(0);
    if (idTipoCentral == 5)
        mostrarOcultarCentral(1);
    var idEmpresa = $('#cbEmpresa').val();
    var fecha = $('#txtFecha').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarCentral',

        data: {
            tipoCentral: idTipoCentral,
            idEmpresa: idEmpresa,
            fecha: fecha
        },

        success: function (aData) {
            $('#central').html(aData);
        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListaModo_Grupo(tipoCentral, modoGrupoSelect, pos) {
    var idCentral = $('#cbCentral2').val();
    var idEmpresa = $('#cbEmpresa').val();
    $.ajax({
        type: 'POST',
        url: controlador + 'CargarListaGrupoModo',

        data: {
            idCentral: idCentral,
            idEmpresa: idEmpresa,
            idTipoCentral: tipoCentral,
            modoGrupoSelect: modoGrupoSelect,
            pos: pos
        },
        success: function (aData) {
            $('#listaModoGrupo').html(aData);

            $('#cbModoOpGrupo').val($('#hfIdModGrup').val());
            $("#hfFlagCentralRsvFriaToRegistrarUnidad").val($("#hfFlagCentralRsvFriaToRegistrarUnidad2").val());

            $('#cbModoOpGrupo').change(function () {
                $(".unidades_modo").hide();
                view_EnabledOrDisabledUIHoraOperacion();
                cargarListadoUnidadesEspeciales($("#cbTipoCentral").val(), $("#cbEmpresa").val(), $("#cbCentral2").val(), $('#cbModoOpGrupo').val());

                setearFechasInicioFin($(this).val(), -1, idCentral);
            });

            cargarListadoUnidadesEspeciales($("#cbTipoCentral").val(), $("#cbEmpresa").val(), $("#cbCentral2").val(), $('#cbModoOpGrupo').val());
            view_EnabledOrDisabledUIHoraOperacion();
            setearFechasInicioFin($('#cbModoOpGrupo').val(), $('#hfIdPos').val(), idCentral);

        },
        error: function (err) {
            alert("Ha ocurrido un error");
        }
    });
}

function cargarListadoUnidadesEspeciales(idTipoCentral, idEmpresa, idCentral, idModo) {
    if (!(APP_OPCION == OPCION_EDITAR && TIPO_CENTRAL_HIDROELECTRICA == idTipoCentral)) {
        idModo = parseInt(idModo) || 0;
        $(".unidades_modo").hide();
        $('.unidades_no_esp').hide();
        $('#unidades_especiales').html('');
        $('#unidades_no_esp').html('');

        $.ajax({
            type: 'POST',
            url: controlador + 'CargarListaUnidadesModo',
            data: {
                idCentral: idCentral,
                idEmpresa: idEmpresa,
                idTipoCentral: idTipoCentral,
                idModo: idModo
            },
            success: function (aData) {
                if (TIPO_CENTRAL_HIDROELECTRICA == idTipoCentral) {
                    $('#unidades_no_esp').html(aData);
                } else {
                    $('#unidades_especiales').html(aData);
                }

                view_EnabledOrDisabledUIHoraOperacion();

                var tipoModoUnidadEspecial = $("#tipoModoUnidadEspecial").val();
                var tipoOpDefecto = $('#tipoSubcausaDefecto').val();

                $("select[name=cbTipoOperacion]").each(function () {
                    $("#" + this.id).val(tipoOpDefecto);
                });
                if (TIPO_CENTRAL_HIDROELECTRICA == idTipoCentral) {
                    $("input[name=chkUnidades]").each(function () {
                        $("#" + this.id).prop("checked", true);
                    });
                }

                $('#hfIdTipoModOp').val(tipoModoUnidadEspecial);
                if (tipoModoUnidadEspecial == "0") {
                    if (TIPO_CENTRAL_TERMOELECTRICA == idTipoCentral) {
                        $(".unidades_modo").show();
                        $(".unidades_modo.titulo td").text("Seleccione Unidades para el Modo de Operación");
                        unidEsp_mostrarTablaFormularioUnidadesEspeciales($("#hfIdPos").val());
                    }
                    if (TIPO_CENTRAL_HIDROELECTRICA == idTipoCentral) {
                        $(".unidades_no_esp").show();
                        $(".unidades_no_esp.titulo td").text("Seleccione Unidades para la Central");
                    }
                }
            },
            error: function (err) {
                alert("Ha ocurrido un error");
            }
        });
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Operaciones de Registro, Edicion de Hora de Operación Principal
///////////////////////////////////////////////////////////////////////////////////////////////////

function ho_ValidarFormulario() {
    var errors = 0;
    var flagModoEsp = glb_FlagModoEspecial(evtHot, $("#cbModoOpGrupo").val());

    //Validar formulario
    var msjValidacionHo = ho_validarHoraOperacion(TIPO_HO_MODO, OBJ_REFERENCIA, flagModoEsp, $('#hfTipoCentral').val(), $('#hfIdPos').val());
    if (msjValidacionHo != "") {
        alert(msjValidacionHo);
        return false;
    }

    //Validación de formulario a Nivel de Unidades de un Modo de Operación Especial
    if (FLAG_HO_ESPECIAL == flagModoEsp) {
        var numUnid = 0;
        //Verificar que se haya seleccionado al menos una unidad asociada al modo de operación
        $("input[name=chkUnidades]:checked").each(function () {
            numUnid += 1;
        });
        if (numUnid == 0) {
            alert(MSJ_VAL_MODO_SIN_UNIDAD)
            return false;
        }

        var MatrizunidadesExtraHop = unidEsp_listarHop();
        var msjValidEsp = unidEsp_validarTablaUnidEspeciales(MatrizunidadesExtraHop, OBJ_REFERENCIA);
        if (msjValidEsp != '') {
            alert(msjValidEsp);
            return false;
        }
        //Validar nuevamente el formulario debido a actualización por parte de las unidades de las Horas EN PARALELO, FUERA PARALELO del modo
        msjValidacionHo = ho_validarHoraOperacion(TIPO_HO_MODO, OBJ_REFERENCIA, flagModoEsp, $('#hfTipoCentral').val(), $('#hfIdPos').val());
        if (msjValidacionHo != "") {
            alert(msjValidacionHo);
            return false;
        }
    }

    if (errors == 0) {
        ho_CreateOrUpdateHoraOperacion($('#hfIdPos').val(), flagModoEsp);// 0: si es modo de operacion tipo extra 1: normal
    }

    return true;
}

//Registrar o Actualizar Horas de Operación del Formulario
function ho_CreateOrUpdateHoraOperacion(posArray, tipoHO) { // pos: indice de lista de horas de operacion si es modificacion, -1 si es nuevo       
    evtHot.TipoModOp = parseInt(tipoHO);

    var listaHoForm = ho_listarObjFormulario();

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Validación de Cruces
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    var strListaHoTmp = JSON.stringify(evtHot.ListaHorasOperacion);
    var listaHoTmp = JSON.parse(strListaHoTmp);
    for (var i = 0; i < listaHoForm.length; i++) {
        var flag = 0;
        var objHo = listaHoForm[i];
        var pos = i > 0 ? -1 : posArray; //la posición 1 hacia adelante son independizados

        //Verificación de cruces de Horas de Operación
        if (TIPO_CENTRAL_HIDROELECTRICA == evtHot.IdTipoCentral && APP_OPCION == OPCION_NUEVO) { //Hidro cuando se quiere ingresar por central y todos los grupos
            var MatrizunidadesExtraFinal = [];
            if (objHo.MatrizunidadesExtra.length > 0) {
                for (var wz = 0; wz < objHo.MatrizunidadesExtra.length; wz++) {
                    // verifica si hay cruces de horas // 1: nuevo, 2: si hay cruces      
                    flag = verificarCruceModoHO(listaHoTmp, evtHot.IdTipoCentral, objHo.MatrizunidadesExtra[wz].EquiCodi, objHo.fecha, objHo.fechaFin, objHo.enParalelo, objHo.fueraParalelo, tipoHO, pos);

                    if (flag == 1) { // si no hay cruces                 
                        MatrizunidadesExtraFinal.push(objHo.MatrizunidadesExtra[wz]);
                    }
                }
                if (objHo.MatrizunidadesExtra.length != MatrizunidadesExtraFinal.length) {
                    alert(MSJ_VAL_CRUCE_HO);
                    return false;
                }
                objHo.MatrizunidadesExtra = MatrizunidadesExtraFinal;
            }
        } else {
            // verifica si hay cruces de horas // 1: nuevo, 2: si hay cruces      
            flag = verificarCruceModoHO(listaHoTmp, evtHot.IdTipoCentral, objHo.idEquipoOrIdModo, objHo.fecha, objHo.fechaFin, objHo.enParalelo, objHo.fueraParalelo, tipoHO, pos);

            if (flag == 2) { // si hay cruces
                alert(MSJ_VAL_CRUCE_HO);
                return false;
            }
        }

        if (flag == 1) {
            if (pos != -1) { // Actualizar Hora de Operación
                switch (evtHot.IdTipoCentral) {
                    case TIPO_CENTRAL_TERMOELECTRICA:
                        var msjValidacion = verificarCruceUnidadesHO_UPDATE(listaHoTmp, objHo.idEquipoOrIdModo, pos, convertStringToDate(objHo.fecha, objHo.enParalelo), convertStringToDate(objHo.fechaFin, objHo.fueraParalelo), tipoHO, objHo.MatrizunidadesExtraHop);

                        if (msjValidacion != '') {// sin hay cruce horas de operacion en unidades
                            alert(msjValidacion);
                            return false;
                        }
                        break;
                }
                listaHoTmp = ho_updateObjHo(tipoHO, objHo, listaHoTmp, pos);
            } else { // Nueva Hora de Operación
                switch (evtHot.IdTipoCentral) {
                    case TIPO_CENTRAL_TERMOELECTRICA:
                        var msjValidacion = verificarCruceUnidadesHO_CREATE(listaHoTmp, objHo.idEquipoOrIdModo, convertStringToDate(objHo.fecha, objHo.enParalelo), convertStringToDate(objHo.fechaFin, objHo.fueraParalelo), tipoHO, objHo.MatrizunidadesExtraHop);

                        if (msjValidacion != '') {// sin hay cruce horas de operacion en unidades
                            alert(msjValidacion);
                            return false;
                        }
                        break;
                }
                listaHoTmp = ho_createObjHo(tipoHO, objHo, listaHoTmp);
            }
        }
    }

    var isVerif = validarHorasOperacion(listaHoTmp);
    if (!isVerif)
        return false;

    // nuevo o actualiza registro de horas de operacion
    for (var i = 0; i < listaHoForm.length; i++) {
        var objHo = listaHoForm[i];
        var pos = i > 0 ? -1 : posArray; //la posición 1 hacia adelante son independizados

        if (pos != -1) {
            ///////////////////////////////////////////////////////////////////////////////////////////////
            /// Actualizar Hora de Operación
            ///////////////////////////////////////////////////////////////////////////////////////////////
            evtHot.ListaHorasOperacion = ho_updateObjHo(tipoHO, objHo, evtHot.ListaHorasOperacion, pos);
        }
        else {
            ///////////////////////////////////////////////////////////////////////////////////////////////
            /// Registro de Hora de Operación
            ///////////////////////////////////////////////////////////////////////////////////////////////
            evtHot.ListaHorasOperacion = ho_createObjHo(tipoHO, objHo, evtHot.ListaHorasOperacion);
        }
    }

    actualizaListadoHorasDeOperacion(evtHot);
    $('#newHorasOperacion').bPopup().close();
}

//Registrar o Actualizar Horas de Operación de las unidades de un Modo de Operación
function ho_CreateUpdateHoraOperacionUnidEsp(listaHo, pos, idEquipoOrIdModo, MatrizunidadesExtra, MatrizunidadesExtraHop) {
    if (evtHot.ListaUnidXModoOP.length > 0) {
        for (var j = 0; j < evtHot.ListaUnidXModoOP.length; j++) {
            if (evtHot.ListaUnidXModoOP[j].Grupocodi == idEquipoOrIdModo) {
                var flag01 = getFindEquipoxCodUnidadesExtra(evtHot.ListaUnidXModoOP[j].Equicodi, MatrizunidadesExtra);

                if (flag01 > -1) {
                    // existe unidad, Registrar o Actualizar las horas de operacion de la tabla

                    for (var ttt = 0; ttt < MatrizunidadesExtraHop.length; ttt++) {
                        var objUnidEsp = MatrizunidadesExtraHop[ttt];
                        if (objUnidEsp.Equicodi == evtHot.ListaUnidXModoOP[j].Equicodi) {
                            var listaHoTmp = listarHorasOperacionByHopcodipadre(listaHo, listaHo[pos].Hopcodi, objUnidEsp.Equicodi);
                            var listaHoTablaHtml = objUnidEsp.ListaHo;

                            for (var hhh = 0; hhh < listaHoTablaHtml.length; hhh++) {
                                var hopEspTmp = listaHoTablaHtml[hhh];
                                var pos02 = getPosHop(hopEspTmp.Hopcodi, listaHo);

                                if (pos02 > -1) { // existe unidad en hora de operación

                                    // modificamos horas de operacion de la unidad relacionada
                                    listaHo[pos02].Hophorini = hopEspTmp.Hophorini;
                                    listaHo[pos02].Hophorfin = hopEspTmp.Hophorfin;
                                    listaHo[pos02].Hophorordarranq = hopEspTmp.Hophorordarranq;
                                    listaHo[pos02].Hophorparada = hopEspTmp.Hophorparada;
                                    listaHo[pos02].Hopfalla = hopEspTmp.Hopfalla;
                                    listaHo[pos02].FlagTipoHo = TIPO_HO_UNIDAD_BD;
                                    listaHo[pos02].Hoparrqblackstart = hopEspTmp.arranqBlackStart;
                                    listaHo[pos02].Hopensayope = hopEspTmp.ensayoPotenciaEfectiva;
                                    listaHo[pos02].OpcionCrud = 2; // -1: eliminar, 0:lectura, 1:crear, 2:actualizar  
                                }
                                else { // no existe unidad en horas de operacion 
                                    ///****crear unidad extra nueva
                                    newHopcodi = (-1) * (listaHo.length + 1);
                                    var entityUnid =
                                    {
                                        Hopcodi: newHopcodi,
                                        Hophorini: hopEspTmp.Hophorini,
                                        Hophorfin: hopEspTmp.Hophorfin,
                                        Hophorordarranq: hopEspTmp.Hophorordarranq,
                                        Hophorparada: hopEspTmp.Hophorparada,
                                        Equicodi: objUnidEsp.Equicodi,
                                        Equiabrev: objUnidEsp.Equiabrev,
                                        Emprcodi: objUnidEsp.Emprcodi,
                                        Hopfalla: hopEspTmp.Hopfalla,
                                        FlagTipoHo: TIPO_HO_UNIDAD_BD,
                                        OpcionCrud: 1, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
                                        CodiPadre: idEquipoOrIdModo,
                                        Hopcodipadre: listaHo[pos].Hopcodi,
                                        Hoparrqblackstart: hopEspTmp.arranqBlackStart,
                                        Hopensayope: hopEspTmp.ensayoPotenciaEfectiva
                                    };
                                    //registramos la unidad relacionada al modo de operación
                                    listaHo.push(entityUnid);
                                }
                            }

                            //Eliminado Lógico o Físico
                            var listaHopcodiEliminable = unidEsp_listarHopcodiAEliminar(listaHoTmp, listaHoTablaHtml);

                            var listaHOFinal = [];

                            if (listaHoTmp.length > 0) {
                                for (var h2 = 0; h2 < listaHo.length; h2++) {
                                    var deleteFisico = false;
                                    for (var ti = 0; ti < listaHopcodiEliminable.length; ti++) {
                                        if (listaHopcodiEliminable[ti] == listaHo[h2].Hopcodi) {
                                            deleteFisico = true; //eliminado fisico de la matriz auxiliar  
                                            if (listaHo[h2].Hopcodi != 0) {// si unidad viene de Bd -> eliminado lógico
                                                listaHo[h2].OpcionCrud = -1; // eliminado lógico
                                                deleteFisico = false;
                                            }
                                        }
                                    }

                                    if (!deleteFisico)
                                        listaHOFinal.push(listaHo[h2]);
                                    else
                                        console.log('Eliminado:' + h2);
                                }

                                listaHo = listaHOFinal;
                            }

                        }
                    }
                }
                else {
                    // si no existe unidad en lista de codigos de unidades seleccionadas
                    var listaHoTmp = listarHorasOperacionByHopcodipadre(listaHo, listaHo[pos].Hopcodi, evtHot.ListaUnidXModoOP[j].Equicodi);
                    var listaHOFinal = [];

                    if (listaHoTmp.length > 0) {
                        for (var h2 = 0; h2 < listaHo.length; h2++) {
                            var deleteFisico = false;
                            for (var hhh = 0; hhh < listaHoTmp.length; hhh++) {
                                if (listaHoTmp[hhh].Hopcodi == listaHo[h2].Hopcodi) {
                                    deleteFisico = true; //eliminado fisico de la matriz auxiliar  
                                    if (listaHo[h2].Hopcodi != 0) {// si unidad viene de Bd -> eliminado lógico
                                        listaHo[h2].OpcionCrud = -1; // eliminado lógico
                                        deleteFisico = false;
                                    }
                                }
                            }

                            if (!deleteFisico)
                                listaHOFinal.push(listaHo[h2]);
                            else
                                console.log('Eliminado:' + h2);
                        }

                        listaHo = listaHOFinal;
                    }
                }
            }
        }
    }

    return listaHo;
}

//Listar las horas de operación generadas por el formulario
function ho_listarObjFormulario() {
    var listaHoForm = [];

    var objMain = {};
    objMain.emprcodi = $('#cbEmpresa2').val(); //emprcodi
    objMain.fecha = $('#txtFecha').val();
    objMain.fechaFin = $('#txtFueraParaleloF').val(); // fecha de fuera paralelo si es fin del dia 24:00:00 hs
    objMain.central = $('#cbCentral2').val();
    objMain.modoGrupo = $('#cbModoOpGrupo').val(); // modo de operacion ó grupo
    objMain.ordenArranqueF = $('#txtOrdenArranqueF').val();
    objMain.ordenArranque = $('#txtOrdenArranqueH').val();
    objMain.enParaleloF = $('#txtEnParaleloF').val(); // HoraIni (HH:mm:ss)   
    objMain.enParalelo = $('#txtEnParaleloH').val(); // HoraIni (HH:mm:ss)         
    objMain.ordenParadaF = $('#txtOrdenParadaF').val();
    objMain.ordenParada = $('#txtOrdenParadaH').val();
    objMain.fueraParaleloF = $('#txtFueraParaleloF').val(); //HoraFin (HH:mm:ss)
    objMain.fueraParalelo = $('#txtFueraParaleloH').val(); //HoraFin (HH:mm:ss)

    objMain.observacion = $("#txtObservacion").val();
    objMain.tipoOperacion = $('#cbTipoOp').val();

    // valores por defecto de los checkbox en BD
    objMain.fueraServicio = null;
    objMain.compOrdArranq = "N";
    objMain.compOrdParad = "N";
    objMain.sistAislado = 0;
    objMain.limTrans = 'N';
    objMain.arranqBlackStart = 'N';
    objMain.ensayoPotenciaEfectiva = 'N';

    objMain.MatrizunidadesExtra = [];
    $("input[name=chkUnidades]:checked").each(function () {

        idTipoOP = $("#cb" + this.id).val();
        var objEquipo = getEquipoFromListaUnidades(evtHot.ListaUnidades, this.id);
        var equiabrevDesc = objEquipo != null ? objEquipo.Equiabrev : '';

        var entity = {
            Emprcodi: $('#cbEmpresa2').val(),
            EquiCodi: this.id, // unidad para el modo de operacion
            Equiabrev: equiabrevDesc,
            tipoOpId: idTipoOP
        };
        objMain.MatrizunidadesExtra.push(entity);
    });

    objMain.MatrizunidadesExtraHop = unidEsp_listarHop(); //Unidades especiales

    objMain.valCkeckfueraServ = document.getElementById("chkFueraServicio").checked;
    if (objMain.valCkeckfueraServ == 1) {
        objMain.fueraServicio = 'F';
    }

    objMain.valChkArranqBlackStart = document.getElementById("chkArranqueBlackStart").checked;
    if (objMain.valChkArranqBlackStart == 1) {
        objMain.arranqBlackStart = 'S';
    }

    objMain.valChkEnsayoPotenciaEfectiva = document.getElementById("chkEnsayoPotenciaEfectiva").checked;
    if (objMain.valChkEnsayoPotenciaEfectiva == 1) {
        objMain.ensayoPotenciaEfectiva = 'S';
    }

    //////////////////////////////////////////////////////
    objMain.idEquipoOrIdModo = 0;
    switch (evtHot.IdTipoCentral) {
        case 4: //Hidraulicas
        case 5: //Térmicas
            objMain.idEquipoOrIdModo = objMain.modoGrupo;
            break;
        case 37: //Solares
        case 39: //Eolicas
            objMain.idEquipoOrIdModo = objMain.central;
            break;
    }

    listaHoForm.push(objMain);

    return listaHoForm;
}

//Agrega el obj Hora de Operación a lista 
function ho_createObjHo(tipoHO, objForm, listaHo) {

    var codAutoHopcodi = (-1) * (listaHo.length + 1);

    switch (evtHot.IdTipoCentral) {
        case TIPO_CENTRAL_SOLAR:
        case TIPO_CENTRAL_EOLICA:

            var entity =
            {
                Hopcodi: codAutoHopcodi,
                Hophorini: moment(convertStringToDate(objForm.enParaleloF, objForm.enParalelo)),
                Hophorfin: moment(convertStringToDate(objForm.fueraParaleloF, objForm.fueraParalelo)),
                Hophorordarranq: convertStringToDate(objForm.ordenArranqueF, objForm.ordenArranque),
                Hophorparada: convertStringToDate(objForm.ordenParadaF, objForm.ordenParada),
                Emprcodi: objForm.emprcodi,
                Equicodi: objForm.idEquipoOrIdModo,
                Subcausacodi: objForm.tipoOperacion,
                OpcionCrud: 1, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
                Hopfalla: objForm.fueraServicio,
                Hopobs: objForm.observacion,
                Hoparrqblackstart: objForm.arranqBlackStart,
                Hopensayope: objForm.ensayoPotenciaEfectiva,
                Hopnotifuniesp: FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION
            };

            listaHo.push(entity);
            break;
        case TIPO_CENTRAL_HIDROELECTRICA:
            if (objForm.MatrizunidadesExtra.length > 0) {
                for (var wz = 0; wz < objForm.MatrizunidadesExtra.length; wz++) {
                    codAutoHopcodi = (-1) * (listaHo.length + 1);
                    var entity2 =
                    {
                        Hopcodi: codAutoHopcodi,
                        Hophorini: moment(convertStringToDate(objForm.enParaleloF, objForm.enParalelo)),
                        Hophorfin: moment(convertStringToDate(objForm.fueraParaleloF, objForm.fueraParalelo)),
                        Hophorordarranq: convertStringToDate(objForm.ordenArranqueF, objForm.ordenArranque),
                        Hophorparada: convertStringToDate(objForm.ordenParadaF, objForm.ordenParada),
                        Emprcodi: objForm.emprcodi,
                        Equicodi: objForm.MatrizunidadesExtra[wz].EquiCodi,
                        Subcausacodi: objForm.MatrizunidadesExtra[wz].tipoOpId,
                        OpcionCrud: 1, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
                        Hopfalla: objForm.fueraServicio,
                        Hopobs: objForm.observacion,
                        Hoparrqblackstart: objForm.arranqBlackStart,
                        Hopensayope: objForm.ensayoPotenciaEfectiva,
                        Hopnotifuniesp: FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION
                    };

                    //registramos las unidades relacionadas a la central
                    listaHo.push(entity2);
                }
            }
            break;

        case TIPO_CENTRAL_TERMOELECTRICA:// no hay cruces en TV
            var objModo = getModoFromListaModo(evtHot.ListaModosOperacion, objForm.idEquipoOrIdModo);
            var centralDesc = objModo != null ? objModo.Central : '';
            var grupoabrevDesc = objModo != null ? objModo.Grupoabrev : '';
            var strFlagModo = glb_FlagModoEspecial(evtHot, objForm.idEquipoOrIdModo) == FLAG_HO_ESPECIAL ? FLAG_MODO_OP_ESPECIAL : 'N';
            //creamos la hora de operacion para el modo de operacion
            var entity3 =
            {
                Hopcodi: codAutoHopcodi,
                Hophorini: moment(convertStringToDate(objForm.enParaleloF, objForm.enParalelo)),
                Hophorfin: moment(convertStringToDate(objForm.fueraParaleloF, objForm.fueraParalelo)),
                Hophorordarranq: convertStringToDate(objForm.ordenArranqueF, objForm.ordenArranque),
                Hophorparada: convertStringToDate(objForm.ordenParadaF, objForm.ordenParada),
                Emprcodi: objForm.emprcodi,
                Equipadre: objForm.central,
                Grupocodi: objForm.idEquipoOrIdModo,
                FlagTipoHo: TIPO_HO_MODO_BD,
                Central: centralDesc,
                Grupoabrev: grupoabrevDesc,
                Subcausacodi: objForm.tipoOperacion,
                OpcionCrud: 1, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
                UnidadesExtra: objForm.MatrizunidadesExtra,

                Hoparrqblackstart: objForm.arranqBlackStart,
                Hopensayope: objForm.ensayoPotenciaEfectiva,
                Hopfalla: objForm.fueraServicio,
                Hopobs: objForm.observacion,
                Hopnotifuniesp: FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION,
                FlagModoEspecial: strFlagModo
            };
            //registramos las horas de operacion del Modo de operacion y luego las unidades asociadas
            listaHo.push(entity3);

            //registramos unidades asociadas
            switch (tipoHO) {
                case FLAG_HO_ESPECIAL: //Tipo de modo de operacion extra
                    listaHo = ho_CreateUpdateHoraOperacionUnidEsp(listaHo, listaHo.length - 1, objForm.idEquipoOrIdModo, objForm.MatrizunidadesExtra, objForm.MatrizunidadesExtraHop);
                    break;
                case FLAG_HO_NO_ESPECIAL: //Tipo de modo de operacion normal
                    for (var wz = 0; wz < evtHot.ListaUnidades.length; wz++) {
                        if (evtHot.ListaUnidXModoOP.length > 0)
                            for (var j = 0; j < evtHot.ListaUnidXModoOP.length; j++)
                                if (evtHot.ListaUnidXModoOP[j].Equicodi == evtHot.ListaUnidades[wz].Equicodi && evtHot.ListaUnidXModoOP[j].Grupocodi == objForm.idEquipoOrIdModo) {
                                    var entity4 =
                                    {
                                        Hopcodi: codAutoHopcodi - 1,
                                        Hophorini: moment(convertStringToDate(objForm.enParaleloF, objForm.enParalelo)),
                                        Hophorfin: moment(convertStringToDate(objForm.fueraParaleloF, objForm.fueraParalelo)),
                                        Hophorordarranq: convertStringToDate(objForm.ordenArranqueF, objForm.ordenArranque),
                                        Hophorparada: convertStringToDate(objForm.ordenParadaF, objForm.ordenParada),
                                        Emprcodi: objForm.emprcodi,
                                        Equipadre: objForm.central,
                                        Equicodi: evtHot.ListaUnidades[wz].Equicodi,
                                        Equiabrev: evtHot.ListaUnidades[wz].Equiabrev,
                                        FlagTipoHo: TIPO_HO_UNIDAD_BD,
                                        OpcionCrud: 1, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
                                        CodiPadre: objForm.idEquipoOrIdModo,
                                        Subcausacodi: objForm.tipoOperacion,
                                        Hopcodipadre: codAutoHopcodi,

                                        Hoparrqblackstart: objForm.arranqBlackStart,
                                        Hopensayope: objForm.ensayoPotenciaEfectiva,
                                        Hopfalla: objForm.fueraServicio,
                                        Hopobs: objForm.observacion
                                    };
                                    //registramos la unidad relacionada al modo de operación
                                    listaHo.push(entity4);
                                }
                    }
                    break;
            }
            break;
    }

    return listaHo;
}

//Actualiza el obj Hora de Operación de la lista
function ho_updateObjHo(tipoHO, objForm, listaHo, pos) {

    switch (evtHot.IdTipoCentral) {
        case TIPO_CENTRAL_HIDROELECTRICA:
        case TIPO_CENTRAL_SOLAR:
        case TIPO_CENTRAL_EOLICA:
            //recuperamos el rango de fechas a modificar
            var checkdateFrom = new Date(moment(listaHo[pos].Hophorini));
            var checkdateTo = new Date(moment(listaHo[pos].Hophorfin));
            // modificamos horas de operacion del modo de operacion
            listaHo[pos].Hophorini = moment(convertStringToDate(objForm.enParaleloF, objForm.enParalelo));
            listaHo[pos].Hophorfin = moment(convertStringToDate(objForm.fueraParaleloF, objForm.fueraParalelo));
            listaHo[pos].Hophorordarranq = convertStringToDate(objForm.ordenArranqueF, objForm.ordenArranque);
            listaHo[pos].Hophorparada = convertStringToDate(objForm.ordenParadaF, objForm.ordenParada);
            listaHo[pos].FlagTipoHo = TIPO_HO_MODO_BD;
            listaHo[pos].Subcausacodi = objForm.tipoOperacion;
            listaHo[pos].Emprcodi = objForm.emprcodi;
            listaHo[pos].OpcionCrud = 2; // -1: eliminar, 0:lectura, 1:crear, 2:actualizar   
            listaHo[pos].Hopfalla = objForm.fueraServicio;
            listaHo[pos].Hopobs = objForm.observacion;
            listaHo[pos].Hoparrqblackstart = objForm.arranqBlackStart;
            listaHo[pos].Hopensayope = objForm.ensayoPotenciaEfectiva;

            break;
        case TIPO_CENTRAL_TERMOELECTRICA:
            //recuperamos el rango de fechas a modificar
            var checkdateFrom = new Date(moment(listaHo[pos].Hophorini));
            var checkdateTo = new Date(moment(listaHo[pos].Hophorfin));
            // modificamos horas de operacion del modo de operacion
            listaHo[pos].Hophorini = moment(convertStringToDate(objForm.enParaleloF, objForm.enParalelo));
            listaHo[pos].Hophorfin = moment(convertStringToDate(objForm.fueraParaleloF, objForm.fueraParalelo));
            listaHo[pos].Hophorordarranq = convertStringToDate(objForm.ordenArranqueF, objForm.ordenArranque);
            listaHo[pos].Hophorparada = convertStringToDate(objForm.ordenParadaF, objForm.ordenParada);
            listaHo[pos].Subcausacodi = objForm.tipoOperacion;
            listaHo[pos].MatrizunidadesExtra = objForm.MatrizunidadesExtra;
            listaHo[pos].Emprcodi = objForm.emprcodi;
            listaHo[pos].OpcionCrud = 2; // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
            listaHo[pos].Hopfalla = objForm.fueraServicio;
            listaHo[pos].Hoparrqblackstart = objForm.arranqBlackStart;
            listaHo[pos].Hopensayope = objForm.ensayoPotenciaEfectiva;
            listaHo[pos].Hopobs = objForm.observacion;

            /// modificamos horas de operacion de las unidades relacionadas
            switch (tipoHO) {
                case FLAG_HO_ESPECIAL: //Tipo de Modo de Operacion con Unidades Especiales
                    listaHo = ho_CreateUpdateHoraOperacionUnidEsp(listaHo, pos, objForm.idEquipoOrIdModo, objForm.MatrizunidadesExtra, objForm.MatrizunidadesExtraHop);
                    break;
                case FLAG_HO_NO_ESPECIAL:
                    if (evtHot.ListaUnidXModoOP.length > 0)
                        for (var j = 0; j < evtHot.ListaUnidXModoOP.length; j++)
                            if (evtHot.ListaUnidXModoOP[j].Grupocodi == objForm.idEquipoOrIdModo) {
                                if (listaHo.length > 0) {
                                    for (var zz = 0; zz < listaHo.length; zz++) {
                                        //verificar que el registro no tenga eliminado lógico si vino de BD propiedad opCdrud != -1  y no sea una actualizacion                               
                                        if (listaHo[zz].Equicodi == evtHot.ListaUnidXModoOP[j].Equicodi && listaHo[zz].OpcionCrud != -1) {
                                            var dateFrom = new Date(moment(listaHo[zz].Hophorini));
                                            var dateTo = new Date(moment(listaHo[zz].Hophorfin));
                                            if (moment(dateFrom).isSame(checkdateFrom) && moment(dateTo).isSame(checkdateTo)) {
                                                // modificamos horas de operacion de la unidad relacionada
                                                listaHo[zz].Hophorini = moment(convertStringToDate(objForm.enParaleloF, objForm.enParalelo));
                                                listaHo[zz].Hophorfin = moment(convertStringToDate(objForm.fueraParaleloF, objForm.fueraParalelo));
                                                listaHo[zz].Hophorordarranq = convertStringToDate(objForm.ordenArranqueF, objForm.ordenArranque);
                                                listaHo[zz].Hophorparada = convertStringToDate(objForm.ordenParadaF, objForm.ordenParada);
                                                listaHo[zz].FlagTipoHo = TIPO_HO_UNIDAD_BD;
                                                listaHo[zz].Subcausacodi = objForm.tipoOperacion;
                                                listaHo[zz].OpcionCrud = 2; // -1: eliminar, 0:lectura, 1:crear, 2:actualizar
                                                listaHo[zz].Emprcodi = objForm.emprcodi;
                                                listaHo[zz].Hopfalla = objForm.fueraServicio;
                                                listaHo[zz].Hopobs = objForm.observacion;
                                                listaHo[zz].Hoparrqblackstart = objForm.arranqBlackStart;
                                                listaHo[zz].Hopensayope = objForm.ensayoPotenciaEfectiva;
                                            }
                                        }
                                    }
                                }
                            }
                    break;
            }
            break;
    }

    return listaHo;
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Formulario de Unidades Especiales
///////////////////////////////////////////////////////////////////////////////////////////////////

function unidEsp_mostrarTablaFormularioUnidadesEspeciales(pos) {
    pos = parseInt(pos);
    var hoppadre = {};
    if (pos > -1) { //Edición de Hora de Operación
        hoppadre = evtHot.ListaHorasOperacion[pos];
    } else { //Registro de Hora de Operación        
        hoppadre = unidEsp_GetHopDefaultRegistro();
    }

    if (hoppadre.FlagModoEspecial == FLAG_MODO_OP_ESPECIAL) {

        var listaUnidad = listarUnidadesXModo(evtHot, hoppadre.Grupocodi, hoppadre.Equipadre);

        for (var i = 0; i < listaUnidad.length; i++) {
            var objUnidad = listaUnidad[i];
            var arrayTmp = [];
            arrayTmp.push(hoppadre);
            var listaHoXUnidad = pos > -1 ? listarHorasOperacionByHopcodipadre(evtHot.ListaHorasOperacion, hoppadre.Hopcodi, objUnidad.Equicodi) : arrayTmp;

            //Generar Tabla html por cada unidad
            var htmlXUnidad = '';
            htmlXUnidad += `
                <table class='tabla_ho_esp pretty tabla-icono' id='tabla_ho_esp_${objUnidad.Equicodi}' style='width: 744px'>
                    <thead>
                        <tr>
                            <th>Orden de Arranque</th>
                            <th>En Paralelo</th>
                            <th>Orden de Parada</th>
                            <th>Fuera de Paralelo</th>
            `;
            if (APP_OPCION != OPCION_VER) {
                htmlXUnidad += "<th></th>";
            }
            htmlXUnidad += `
                    </tr>
                </thead>
                <tbody> `;

            for (var m = 0; m < listaHoXUnidad.length; m++) {
                htmlXUnidad += unidEsp_visualizarHoraOperacionUnidadEspecial(listaHoXUnidad[m], listaHoXUnidad[m].Hopcodi, objUnidad.Equicodi, m, pos, OPCION_EDITAR);
            }
            htmlXUnidad +=
                `</tbody >
            </table >`;

            $("#unidadEspecial_" + objUnidad.Equicodi).html(htmlXUnidad);

            $("#" + objUnidad.Equicodi).prop("checked", false);
            if (listaHoXUnidad.length > 0) {
                $("#" + objUnidad.Equicodi).prop("checked", true);
            }

            //Asignar eventos 
            for (var m = 0; m < listaHoXUnidad.length; m++) {
                unidEsp_asignarEventoPos(objUnidad.Equicodi, m);
            }

            //Mostrar boton agregar 
            if (APP_OPCION != OPCION_VER) {
                $("#" + objUnidad.Equicodi).parent().find("a").show();
            }
        }

        //Mostrar deshabilitado las fechas de orden parada cuando esta activado el check
        unidEsp_HabilitacionCheckFS(document.getElementById("chkFueraServicio").checked);
    }
}

function unidEsp_visualizarHoraOperacionUnidadEspecial(hoUnidad, hopcodi, equicodi, pos_row, pos_HO, tipoOpcion) {
    var Fecha = obtenerFechaByCampoHO(hoUnidad.Hophorini);
    var Hophorordarranq = OPCION_NUEVO == tipoOpcion ? '' : obtenerHoraByCampoHO(hoUnidad.Hophorordarranq);
    var Fechahorordarranq = OPCION_NUEVO == tipoOpcion ? '' : obtenerFechaByCampoHO(hoUnidad.Hophorordarranq);
    Fechahorordarranq = Fechahorordarranq != '' ? Fechahorordarranq : Fecha;
    var HoraIni = OPCION_NUEVO == tipoOpcion ? '' : obtenerHoraByCampoHO(hoUnidad.Hophorini);
    var HoraFin = OPCION_NUEVO == tipoOpcion ? '' : obtenerHoraByCampoHO(hoUnidad.Hophorfin);
    var FechaFin = obtenerFechaByCampoHO(hoUnidad.Hophorfin);
    var Hophorparada = OPCION_NUEVO == tipoOpcion ? '' : obtenerHoraByCampoHO(hoUnidad.Hophorparada);

    var htmlDiaOrdArranq = unidEsp_listarOrdArranq(Fechahorordarranq);

    var idFila = 'tr_ho_esp_' + pos_row;
    var enabledInput = APP_OPCION == OPCION_VER ? 'disabled' : '';
    var disenioTabla = `
        <tr id='${idFila}'>
            <td>
                <input type="hidden" name="hopcodi" value="${hopcodi}" disabled />
                <select name="txtOrdenArranqueF" style="width:90px;" ${enabledInput}>  
                    ${htmlDiaOrdArranq}
                </select>
                <input type="Text" name="txtOrdenArranqueH" style="width:64px;" value="${Hophorordarranq}" autocomplete="off" ${enabledInput}/>
            </td>
            <td>
                <input type="text" name="txtEnParaleloF" style="width:77px;" value="${Fecha}" disabled />
                <input type="Text" name="txtEnParaleloH" style="width:64px;" value="${HoraIni}" autocomplete="off" ${enabledInput}/>
            </td>
            <td>
                <input type="text" name="txtOrdenParadaF" style="width:77px;" value="${Fecha}" disabled />
                <input type="Text" name="txtOrdenParadaH" style="width:64px;" value="${Hophorparada}" autocomplete="off" ${enabledInput}/>
            </td>
            <td>
                <input type="text" name="txtFueraParaleloF" style="width:77px;" value="${FechaFin}" disabled />
                <input type="Text" name="txtFueraParaleloH" style="width:64px;" value="${HoraFin}" autocomplete="off" ${enabledInput}/>
            </td>
    `;

    if (APP_OPCION != OPCION_VER) {
        disenioTabla += `
            <td style='text-align: center'>
                <a href='JavaScript:unidEsp_quitarHoEspecial(${equicodi},${pos_row})'>
                    <img src='${siteRoot}Content/Images/btn-cancel.png' />
                </a>
            </td>
        </tr>
        `;
    }
    disenioTabla += '</tr>'

    return disenioTabla;
}

function unidEsp_listarOrdArranq(Fechahorordarranq) {
    var fechaHoy = $('#hfFecha').val();
    var fechaAnterior = $('#hfFechaAnterior').val();
    var listaFechaArranque = [];
    listaFechaArranque.push(fechaHoy);
    listaFechaArranque.push(fechaAnterior);

    var disenioTabla = '';
    listaFechaArranque.forEach(function (item) {
        var selected = "";
        if (item == Fechahorordarranq) {
            selected = "selected";
        }
        disenioTabla += `
                <option value="${item}" ${selected}> ${item}</option >
        `;
    });

    return disenioTabla;
}

function unidEsp_quitarHoEspecial(equicodi, pos) {
    $('#tabla_ho_esp_' + equicodi + ' #tr_ho_esp_' + pos).remove();
    var m = parseInt($('#tabla_ho_esp_' + equicodi + ' tbody tr').length) || 0;

    $("#" + equicodi).prop("checked", false);
    if (m > 0) {
        $("#" + equicodi).prop("checked", true);
    }
}

function unidEsp_agregarHoEspecial(equicodi, pos_HO) {
    var pos = parseInt($('#hfIdPos').val());
    if (pos == -1) {
        pos_HO = -1;
    }

    var hoppadre = {};
    /*if (pos_HO > -1) { //Edición de Hora de Operación
        hoppadre = evtHot.ListaHorasOperacion[pos_HO];
    } else { //Registro de Hora de Operación  */
    hoppadre = unidEsp_GetHopDefaultRegistro();
    //}

    var idTable = '#tabla_ho_esp_' + equicodi;
    var m = parseInt($(idTable + ' tbody tr').length) || 0;
    var htmlXUnidad = unidEsp_visualizarHoraOperacionUnidadEspecial(hoppadre, 0, equicodi, m + 1, pos_HO, m == 0 ? OPCION_EDITAR : OPCION_NUEVO);

    $(idTable).find('tbody').append(htmlXUnidad);

    $("#" + equicodi).prop("checked", true);
    unidEsp_asignarEventoPos(equicodi, m + 1);

    //Mostrar deshabilitado las fechas de orden parada cuando esta activado el check
    unidEsp_HabilitacionCheckFS(document.getElementById("chkFueraServicio").checked);
}

function unidEsp_GetHopDefaultRegistro() {
    var fecha = $('#txtFecha').val();
    var fechaFin = $('#txtFueraParaleloF').val(); // fecha de fuera paralello si es fin del dia 24:00:00 hs
    var equipadre = parseInt($("#cbCentral2").val());
    var modoGrupo = $('#cbModoOpGrupo').val(); // modo de operacion ó grupo
    var ordenArranque = $('#txtOrdenArranqueH').val();
    var enParalelo = $('#txtEnParaleloH').val(); // HoraIni (HH:mm:ss)          
    var ordenParada = $('#txtOrdenParadaH').val();
    var fueraParalelo = $('#txtFueraParaleloH').val(); //HoraFin (HH:mm:ss)
    var MatrizunidadesExtra = [];
    var objModo = getModoFromListaModo(evtHot.ListaModosOperacion, parseInt(modoGrupo));
    var flagModo = objModo != null ? objModo.FlagModoEspecial : null;

    var entity =
    {
        Hopcodi: 0,
        Hophorini: moment(convertStringToDate(fecha, enParalelo)),
        Hophorfin: moment(convertStringToDate(fechaFin, fueraParalelo)),
        Hophorordarranq: convertStringToDate(fecha, ordenArranque),
        Hophorparada: convertStringToDate(fecha, ordenParada),
        Grupocodi: modoGrupo,
        Equipadre: equipadre,
        OpcionCrud: 1, // -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
        MatrizunidadesExtra: MatrizunidadesExtra,
        FlagModoEspecial: flagModo
    };

    return entity;
}

function unidEsp_asignarEventoPos(equicodi, pos_row) {
    var idFila = '#unidadEspecial_' + equicodi + " " + '#tr_ho_esp_' + pos_row;

    var id_txtFueraParaleloF = idFila + " " + 'input[name=txtFueraParaleloF]';
    var id_txtOrdenArranqueH = idFila + " " + 'input[name=txtOrdenArranqueH]';
    var id_txtEnParaleloH = idFila + " " + 'input[name=txtEnParaleloH]';
    var id_txtOrdenParadaH = idFila + " " + 'input[name=txtOrdenParadaH]';
    var id_txtFueraParaleloH = idFila + " " + 'input[name=txtFueraParaleloH]';

    ui_setInputmaskHora(id_txtOrdenArranqueH);
    ui_setInputmaskHora(id_txtEnParaleloH);
    ui_setInputmaskHora(id_txtOrdenParadaH);
    ui_setInputmaskHora(id_txtFueraParaleloH);

    //$(id_txtFueraParaleloH).on('blur', function (e) { actualizartxtFueraParaleloF(id_txtFueraParaleloH, id_txtFueraParaleloF) });
    //$(id_txtFueraParaleloH).on('keypress', function (e) { actualizartxtFueraParaleloF(id_txtFueraParaleloH, id_txtFueraParaleloF) });
    //$(id_txtFueraParaleloH).on('keyup', function (e) { actualizartxtFueraParaleloF(id_txtFueraParaleloH, id_txtFueraParaleloF) });

    $(id_txtOrdenArranqueH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_UNIDAD) });
    $(id_txtOrdenParadaH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_UNIDAD) });

    var notifUniEsp = parseInt($("#hfHopnotifuniesp").val()) || 0;
    if (notifUniEsp < FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION || notifUniEsp == FLAG_UNIDAD_ESPECIAL_AGENTE_MODIFICACION_FROM_ADMIN) {
        //no actualizar las Fechas mientras escriben
    } else {
        $(id_txtEnParaleloH).on('focusout', function (e) { actualizarFechasUIHoraOperacion(TIPO_HO_UNIDAD) });
        $(id_txtFueraParaleloH).on('focusout', function (e) { actualizartxtFueraParaleloF(id_txtFueraParaleloH, id_txtFueraParaleloF); actualizarFechasUIHoraOperacion(TIPO_HO_UNIDAD) });
    }
}

function unidEsp_listarHop() {
    var MatrizunidadesExtra = [];
    $("input[name=chkUnidades]").each(function () {
        var entity = {
            Emprcodi: $('#cbEmpresa2').val(),
            Equicodi: this.id, // unidad del modo de operacion
            ListaHo: []
        }
        $('#tabla_ho_esp_' + entity.Equicodi + " tbody").find('tr').each(function () {
            var idFila = '#unidadEspecial_' + entity.Equicodi + " #" + $(this).get()[0].id;
            var id_txtOrdenArranqueH = idFila + " " + 'input[name=txtOrdenArranqueH]';
            var id_txtEnParaleloH = idFila + " " + 'input[name=txtEnParaleloH]';
            var id_txtOrdenParadaH = idFila + " " + 'input[name=txtOrdenParadaH]';
            var id_txtFueraParaleloH = idFila + " " + 'input[name=txtFueraParaleloH]';

            var hopcodi = ($(this).find("input[name='hopcodi']").first()).val();

            $(id_txtOrdenArranqueH).val(obtenerHoraValida($(id_txtOrdenArranqueH).val()));
            $(id_txtEnParaleloH).val(obtenerHoraValida($(id_txtEnParaleloH).val()));
            $(id_txtOrdenParadaH).val(obtenerHoraValida($(id_txtOrdenParadaH).val()));
            $(id_txtFueraParaleloH).val(obtenerHoraValida($(id_txtFueraParaleloH).val()));

            var txtOrdenArranqueF = ($(this).find("select[name='txtOrdenArranqueF']").first()).val();
            var txtOrdenArranqueH = ($(this).find("input[name='txtOrdenArranqueH']").first()).val();

            var txtEnParaleloF = ($(this).find("input[name='txtEnParaleloF']").first()).val();
            var txtEnParaleloH = ($(this).find("input[name='txtEnParaleloH']").first()).val();

            var txtOrdenParadaF = ($(this).find("input[name='txtOrdenParadaF']").first()).val();
            var txtOrdenParadaH = ($(this).find("input[name='txtOrdenParadaH']").first()).val();

            var txtFueraParaleloF = ($(this).find("input[name='txtFueraParaleloF']").first()).val();
            var txtFueraParaleloH = ($(this).find("input[name='txtFueraParaleloH']").first()).val();

            var fOrdenArranque = convertStringToDate(txtOrdenArranqueF, txtOrdenArranqueH);
            var fEnParalelo = convertStringToDate(txtEnParaleloF, txtEnParaleloH);
            var fOrdenParada = convertStringToDate(txtOrdenParadaF, txtOrdenParadaH);
            var fFueraParalelo = convertStringToDate(txtFueraParaleloF, txtFueraParaleloH);

            var hopEsp = {};
            hopEsp.Hopcodi = hopcodi;
            hopEsp.Equicodi = entity.Equicodi;
            hopEsp.Emprcodi = entity.Emprcodi;
            hopEsp.Hophorini = fEnParalelo;
            hopEsp.Hophorfin = fFueraParalelo;
            hopEsp.Hophorordarranq = fOrdenArranque;
            hopEsp.Hophorparada = fOrdenParada;
            hopEsp.RefIdFila = '#unidadEspecial_' + entity.Equicodi + " #" + $(this).get()[0].id;

            hopEsp.ordenArranqueF = txtOrdenArranqueF;
            hopEsp.ordenArranque = fOrdenArranque != '' ? txtOrdenArranqueH : '';

            hopEsp.enParaleloF = txtEnParaleloF; // HoraIni (HH:mm:ss) 
            hopEsp.enParalelo = txtEnParaleloH; // HoraIni (HH:mm:ss)  

            hopEsp.ordenParadaF = txtOrdenParadaF;
            hopEsp.ordenParada = fOrdenParada != '' ? txtOrdenParadaH : fOrdenParada;

            hopEsp.fueraParaleloF = txtFueraParaleloF; //HoraFin (HH:mm:ss)
            hopEsp.fueraParalelo = txtFueraParaleloH; //HoraFin (HH:mm:ss)

            var valCkeckfueraServ = document.getElementById("chkFueraServicio").checked;
            hopEsp.Hopfalla = valCkeckfueraServ == 1 ? 'F' : null;

            entity.ListaHo.push(hopEsp);
        });

        MatrizunidadesExtra.push(entity)
    });

    return MatrizunidadesExtra;
}

function unidEsp_listarHopcodiAEliminar(listaHOMem, listaHOTbl) {
    var listaCodi = [];
    for (var i = 0; i < listaHOMem.length; i++) {
        var bEliminar = true;

        for (var j = 0; j < listaHOTbl.length; j++) {
            var hopcodiTmp = parseInt(listaHOTbl[j].Hopcodi) || 0;
            if (listaHOMem[i].Hopcodi == hopcodiTmp && hopcodiTmp != 0) {
                bEliminar = false;
            }
        }

        if (bEliminar) {
            listaCodi.push(listaHOMem[i].Hopcodi);
        }
    }

    return listaCodi;
}

function unidEsp_validarTablaUnidEspeciales(listaUnidades, objRefPadre) {

    /////////////////////////////////////////////////////////////////////////////////////////////
    //Validar cada fila de Cada Unidad
    /////////////////////////////////////////////////////////////////////////////////////////////
    var msj = unidEsp_validarFilaTabla(listaUnidades);
    if (msj != '') {
        return msj;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////
    //Actualizar campos de Fecha del Formulario de la Hora de Operación modo
    /////////////////////////////////////////////////////////////////////////////////////////////
    unidEsp_actualizarUIFechasModo(listaUnidades, objRefPadre);

    return '';
}

function unidEsp_validarFilaTabla(listaUnidades) {
    var listaHo = [];
    if (listaUnidades.length > 0) {
        for (var i = 0; i < listaUnidades.length; i++) {
            var listaHoByUnid = listaUnidades[i].ListaHo;
            listaHo = listaHo.concat(listaHoByUnid);
            for (var j = 0; j < listaHoByUnid.length; j++) {
                var objHo = listaHoByUnid[j];

                var idFila = objHo.RefIdFila;
                var id_txtOrdenArranqueH = idFila + " " + 'input[name=txtOrdenArranqueH]';
                var id_txtEnParaleloH = idFila + " " + 'input[name=txtEnParaleloH]';
                var id_txtOrdenParadaH = idFila + " " + 'input[name=txtOrdenParadaH]';
                var id_txtFueraParaleloH = idFila + " " + 'input[name=txtFueraParaleloH]';

                var id_txtOrdenArranqueF = idFila + " " + 'select[name=txtOrdenArranqueF]';
                var id_txtEnParaleloF = idFila + " " + 'input[name=txtEnParaleloF]';
                var id_txtOrdenParadaF = idFila + " " + 'input[name=txtOrdenParadaF]';
                var id_txtFueraParaleloF = idFila + " " + 'input[name=txtFueraParaleloF]';

                var objReferencia = {
                    id_ref_txtOrdenArranqueH: id_txtOrdenArranqueH,
                    id_ref_txtEnParaleloH: id_txtEnParaleloH,
                    id_ref_txtOrdenParadaH: id_txtOrdenParadaH,
                    id_ref_txtFueraParaleloH: id_txtFueraParaleloH,
                    id_ref_txtOrdenParadaF: id_txtOrdenParadaF,
                    id_ref_txtFueraParaleloF: id_txtFueraParaleloF,
                    id_ref_txtOrdenArranqueF: id_txtOrdenArranqueF,
                    id_ref_txtEnParaleloF: id_txtEnParaleloF,
                };

                var msjValidacionHoUnidad = ho_validarHoraOperacion(TIPO_HO_UNIDAD, objReferencia);
                if (msjValidacionHoUnidad != '') {
                    return msjValidacionHoUnidad;
                }
            }
        }
        if (listaHo.length == 0) {
            return MSJ_VAL_MODO_SIN_UNIDAD;
        }
    } else {
        return MSJ_VAL_MODO_SIN_UNIDAD;
    }

    return '';
}

function unidEsp_actualizarUIFechasModo(listaUnidades, objRefPadre) {
    var listaHo = [];
    if (listaUnidades.length > 0) {
        for (var i = 0; i < listaUnidades.length; i++) {
            var listaHoByUnid = listaUnidades[i].ListaHo;
            listaHo = listaHo.concat(listaHoByUnid);
        }
    }

    //Actualizar campos de Fecha del Formulario de la Hora de Operación modo
    ordenarListaHorasOperacion(listaHo);

    //
    var arrDateIni = [];
    for (var i = 0; i < listaHo.length; i++) {
        var dt = moment(listaHo[i].Hophorini).toDate();
        if (val_esValidoDate(dt))
            arrDateIni.push(dt);
    }
    arrDateIni = arrDateIni.sort(function (a, b) { return a.getTime() - b.getTime() });
    arrDateIni.filter((date, i, self) =>
        self.findIndex(d => d.getTime() === date.getTime()) === i
    );

    //
    var arrDateFin = [];
    for (var i = 0; i < listaHo.length; i++) {
        var dt = moment(listaHo[i].Hophorfin).toDate();
        if (val_esValidoDate(dt))
            arrDateFin.push(dt);
    }
    arrDateFin = arrDateFin.sort(function (a, b) { return a.getTime() - b.getTime() });
    arrDateFin.filter((date, i, self) =>
        self.findIndex(d => d.getTime() === date.getTime()) === i
    );

    //
    var arrDateArranq = [];
    for (var i = 0; i < listaHo.length; i++) {
        var dt = moment(listaHo[i].Hophorordarranq).toDate();
        if (val_esValidoDate(dt))
            arrDateArranq.push(dt);
    }
    arrDateArranq = arrDateArranq.sort(function (a, b) { return a.getTime() - b.getTime() });
    arrDateArranq.filter((date, i, self) =>
        self.findIndex(d => d.getTime() === date.getTime()) === i
    );

    //
    var arrDateParada = [];
    for (var i = 0; i < listaHo.length; i++) {
        var dt = moment(listaHo[i].Hophorparada).toDate();
        if (val_esValidoDate(dt))
            arrDateParada.push(dt);
    }
    arrDateParada = arrDateParada.sort(function (a, b) { return a.getTime() - b.getTime() });
    arrDateParada.filter((date, i, self) =>
        self.findIndex(d => d.getTime() === date.getTime()) === i
    );

    var strDateIniH = arrDateIni.length > 0 ? moment(arrDateIni[0]).format('HH:mm:ss') : '';
    var strDateFinH = arrDateFin.length > 0 ? moment(arrDateFin[arrDateFin.length - 1]).format('HH:mm:ss') : '';
    var strDateArranqH = arrDateArranq.length > 0 ? moment(arrDateArranq[0]).format('HH:mm:ss') : '';
    var strDateParadaH = arrDateParada.length > 0 ? moment(arrDateParada[arrDateParada.length - 1]).format('HH:mm:ss') : '';

    var objHop = unidEsp_GetHopDefaultRegistro();
    var fechaIni = moment(arrDateIni[0]);
    var fechaFin = moment(arrDateFin[arrDateFin.length - 1]);

    var fechaHoy = $('#hfFecha').val();
    var strDateIniF = strDateIniH != '' ? moment(arrDateIni[0]).format('DD/MM/YYYY') : fechaHoy;
    var strDateFinF = strDateFinH != '' ? moment(arrDateFin[arrDateFin.length - 1]).format('DD/MM/YYYY') : fechaHoy;
    var strDateArranqF = strDateArranqH != '' ? moment(arrDateArranq[0]).format('DD/MM/YYYY') : fechaHoy;
    var strDateParadaF = strDateParadaH != '' ? moment(arrDateParada[arrDateParada.length - 1]).format('DD/MM/YYYY') : fechaHoy;


    $(objRefPadre.id_ref_txtOrdenArranqueH).val(obtenerHoraValida(strDateArranqH));
    $(objRefPadre.id_ref_txtOrdenParadaH).val(obtenerHoraValida(strDateParadaH));

    $(objRefPadre.id_ref_txtOrdenArranqueF).val(obtenerHoraValida(strDateArranqF));
    $(objRefPadre.id_ref_txtOrdenParadaF).val(obtenerHoraValida(strDateParadaF));

    var notifUniEsp = parseInt($("#hfHopnotifuniesp").val()) || 0;
    if (notifUniEsp < FLAG_UNIDAD_ESPECIAL_AGENTE_CREACION || notifUniEsp == FLAG_UNIDAD_ESPECIAL_AGENTE_MODIFICACION_FROM_ADMIN) {
        if (!moment(objHop.Hophorini).isSame(fechaIni) || !moment(objHop.Hophorfin).isSame(fechaFin)) {
            return MSJ_VAL_UNID_ESP_ADMIN;
        }
    } else {
        $(objRefPadre.id_ref_txtEnParaleloH).val(obtenerHoraValida(strDateIniH));
        $(objRefPadre.id_ref_txtFueraParaleloH).val(obtenerHoraValida(strDateFinH));

        $(objRefPadre.id_ref_txtEnParaleloF).val(obtenerHoraValida(strDateIniF));
        $(objRefPadre.id_ref_txtFueraParaleloF).val(obtenerHoraValida(strDateFinF));
    }
}

function unidEsp_HabilitacionCheckFS(value) {
    var listaUnidades = unidEsp_listarHop(FLAG_UNIDAD_NO_INDEPENDIZADA);

    if (listaUnidades != null && listaUnidades.length > 0) {
        for (var i = 0; i < listaUnidades.length; i++) {
            var listaHo = listaUnidades[i].ListaHo;
            if (listaHo.length > 0) {
                for (var j = 0; j < listaHo.length; j++) {
                    if (value) {
                        $(listaHo[j].RefIdFila + ' select[name=txtOrdenArranqueF]').prop('disabled', 'disabled');
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenArranqueH]').prop('disabled', 'disabled');
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenParadaH]').prop('disabled', 'disabled');
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenParadaF]').val('');
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenParadaH]').val('');
                    } else {
                        $(listaHo[j].RefIdFila + ' select[name=txtOrdenArranqueF]').removeAttr("disabled");
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenParadaF]').val($("#txtFecha").val());
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenArranqueH]').removeAttr("disabled");
                        $(listaHo[j].RefIdFila + ' input[name=txtOrdenParadaH]').removeAttr("disabled");
                    }
                }
            }
        }
        if (value) {
            document.getElementById("txtOrdenParadaH").value = "";
            document.getElementById("txtOrdenParadaF").value = '';
        }
        document.getElementById("txtOrdenArranqueH").disabled = true;
        document.getElementById("txtOrdenParadaH").disabled = true;
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// 
///////////////////////////////////////////////////////////////////////////////////////////////////

//Actualizar los inputs de fecha
function actualizarFechasUIHoraOperacion(tipoHo) {
    tipoHo = parseInt(tipoHo) || 0;

    switch (tipoHo) {
        case TIPO_HO_UNIDAD:
            var MatrizunidadesExtraHop = unidEsp_listarHop();
            var listaUnidades = MatrizunidadesExtraHop;

            var listaHo = [];
            if (listaUnidades.length > 0) {
                for (var i = 0; i < listaUnidades.length; i++) {
                    var listaHoByUnid = listaUnidades[i].ListaHo;
                    listaHo = listaHo.concat(listaHoByUnid);
                    for (var j = 0; j < listaHoByUnid.length; j++) {
                        var objHo = listaHoByUnid[j];

                        var idFila = objHo.RefIdFila;
                        var id_txtOrdenArranqueH = idFila + " " + 'input[name=txtOrdenArranqueH]';
                        var id_txtEnParaleloH = idFila + " " + 'input[name=txtEnParaleloH]';
                        var id_txtOrdenParadaH = idFila + " " + 'input[name=txtOrdenParadaH]';
                        var id_txtFueraParaleloH = idFila + " " + 'input[name=txtFueraParaleloH]';

                        var id_txtOrdenArranqueF = idFila + " " + 'select[name=txtOrdenArranqueF]';
                        var id_txtEnParaleloF = idFila + " " + 'input[name=txtEnParaleloF]';
                        var id_txtOrdenParadaF = idFila + " " + 'input[name=txtOrdenParadaF]';
                        var id_txtFueraParaleloF = idFila + " " + 'input[name=txtFueraParaleloF]';

                        var objRef = {
                            id_ref_txtOrdenArranqueH: id_txtOrdenArranqueH,
                            id_ref_txtEnParaleloH: id_txtEnParaleloH,
                            id_ref_txtOrdenParadaH: id_txtOrdenParadaH,
                            id_ref_txtFueraParaleloH: id_txtFueraParaleloH,
                            id_ref_txtOrdenParadaF: id_txtOrdenParadaF,
                            id_ref_txtFueraParaleloF: id_txtFueraParaleloF,
                            id_ref_txtOrdenArranqueF: id_txtOrdenArranqueF,
                            id_ref_txtEnParaleloF: id_txtEnParaleloF,
                        };

                        $(objRef.id_ref_txtOrdenArranqueH).val(obtenerHoraValida($(objRef.id_ref_txtOrdenArranqueH).val()));
                        $(objRef.id_ref_txtEnParaleloH).val(obtenerHoraValida($(objRef.id_ref_txtEnParaleloH).val()));
                        $(objRef.id_ref_txtOrdenParadaH).val(obtenerHoraValida($(objRef.id_ref_txtOrdenParadaH).val()));
                        $(objRef.id_ref_txtFueraParaleloH).val(obtenerHoraValida($(objRef.id_ref_txtFueraParaleloH).val()));
                    }
                }
            }

            unidEsp_actualizarUIFechasModo(MatrizunidadesExtraHop, OBJ_REFERENCIA);
            break;

        case TIPO_HO_MODO:
            $(OBJ_REFERENCIA.id_ref_txtOrdenArranqueH).val(obtenerHoraValida($(OBJ_REFERENCIA.id_ref_txtOrdenArranqueH).val()));
            $(OBJ_REFERENCIA.id_ref_txtEnParaleloH).val(obtenerHoraValida($(OBJ_REFERENCIA.id_ref_txtEnParaleloH).val()));
            $(OBJ_REFERENCIA.id_ref_txtOrdenParadaH).val(obtenerHoraValida($(OBJ_REFERENCIA.id_ref_txtOrdenParadaH).val()));
            $(OBJ_REFERENCIA.id_ref_txtFueraParaleloH).val(obtenerHoraValida($(OBJ_REFERENCIA.id_ref_txtFueraParaleloH).val()));
            break;
    }
}

// Actualizar el Día a mostrar
function actualizartxtFueraParaleloF(ref_txtFueraParaleloH, ref_txtFueraParaleloF) {
    var fechaSiguiente = $("#hfFechaSiguiente").val();
    var fecha = $("#hfFecha").val();
    var hora = obtenerHoraValida($(ref_txtFueraParaleloH).val());
    if (hora == '00:00:00') {
        $(ref_txtFueraParaleloF).val(fechaSiguiente);
    } else {
        $(ref_txtFueraParaleloF).val(fecha);
    }
}

function incluirDescripcionBlackStart() {
    var descripcion = $("#txtObservacion").val();
    descripcion = descripcion != null ? descripcion : '';
    var descVal = descripcion.toUpperCase();

    if ($('#chkArranqueBlackStart').is(":checked")) {
        if (!(descVal.includes("BLACK") && descVal.includes("START"))) {
            descripcion = "Arranque en Black Start, " + descripcion;
            $("#txtObservacion").val(descripcion);
        }

    } else {
        var strInDesc = "Arranque en Black Start,".toUpperCase();
        var posTxt = descVal.indexOf(strInDesc);
        if (posTxt > -1) {
            var descNew = descripcion.substring(0, posTxt) + '' + descripcion.substring(posTxt + strInDesc.length, descripcion.length - posTxt + strInDesc.length);
            $("#txtObservacion").val(descNew.trim());
        }
    }
}

function mostrarCheckEnsayoPotenciaEfectiva() {
    var calif = parseInt($("#cbTipoOp").val()) || 0;

    $("#chkEnsayoPotenciaEfectiva").hide();
    $("#txtEnsayoPotenciaEfectiva").hide();
    if (SUBCAUSACODI_POR_PRUEBAS == calif) {
        //$("#chkEnsayoPotenciaEfectiva").show();
        //$("#txtEnsayoPotenciaEfectiva").show();
    } else {
        //document.getElementById("chkEnsayoPotenciaEfectiva").checked = 0;
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Intervenciones
///////////////////////////////////////////////////////////////////////////////////////////////////
function interv_mostrarAdvertencia(listaHo, listaInterv) {
    if (listaHo.length > 0) {
        var html = '';
        $("#div_msj_val_intervenciones").prop('class', 'action-alert');

        //Horas de operacion
        for (var i = 0; i < listaHo.length; i++) {
            //html += i > 0 ? '<br/>' : '';
            var regHo = listaHo[i];
            var horaIni = moment(regHo.Hophorini).format('DD/MM/YYYY HH:mm:ss');
            var horaFin = moment(regHo.Hophorfin).format('DD/MM/YYYY HH:mm:ss');
            var central = regHo.Central;
            var modo = regHo.Grupoabrev;

            html += `La hora de operación <b>${central}</b>-<b>${modo}</b> ${horaIni} hasta ${horaFin}.<br/>`;
        }

        html += 'Presenta unidades que están en mantenimiento y fuera de servicio: <br/>';
        html += `
                <table class="pretty tabla-icono tabla-ems" style="text-indent: 0px;width: 100%; margin-top: 7px;">
                    <thead>
                        <tr>
                            <th style="">Unidad</th>
                            <th>Tip. Interv.</th>
                            <th style="">Hora Inicio</th>
                            <th style="">Hora Fin</th>
                            <th>Disp.</th>
                            <th>Descripción</th>
                        </tr>
                    </thead>
                    <tbody>
                `;

        //Intervenciones
        for (var j = 0; j < listaInterv.length; j++) {
            var regInterv = listaInterv[j];
            var unidad = regInterv.Equinomb;
            var tipoInter = regInterv.Tipoevenabrev;
            var horaIni = regInterv.FechaIniDesc;
            var horaFin = regInterv.FechaFinDesc;
            var disp = regInterv.Interindispo;
            var descrip = regInterv.Interdescrip;

            html += `
                    <tr>
                        <td>${unidad}</td>
                        <td style="text-align:left;">${tipoInter}</td>
                        <td>${horaIni}</td>
                        <td>${horaFin}</td>
                        <td>${disp}</td>
                        <td style="text-align:left;">${descrip}</td>
                    </tr>
            `;
        }
        html += `
                    </tbody>
                </table>
                `
            ;


        $("#div_msj_val_intervenciones").html(html);
        $("#div_msj_val_intervenciones").show();
        $(".fila_val_intervenciones").show();
    }
}

//*****************************************************************
//***************** UTIL ******************************************
//*****************************************************************

function setearFechasInicioFin(id, posicion, idCentral) {
    var dateMax = convertStringToDate("01/01/1900", "00:00:00");
    var nuevo = false;
    var pos = -1;
    switch (evtHot.IdTipoCentral) {

        case 5: //Térmicas            
            if (evtHot.ListaHorasOperacion.length > 0) {

                for (var i = 0; i < evtHot.ListaHorasOperacion.length; i++) {
                    var regHop = evtHot.ListaHorasOperacion[i];
                    if ((idCentral > 0 && regHop.Equipadre > 0 && regHop.Equipadre == idCentral) || regHop.Grupocodi == id) {
                        var dateTo = new Date(moment(regHop.Hophorfin));
                        if (moment(dateMax).isBefore(dateTo)) {
                            dateMax = dateTo;
                            pos = i;
                        }
                    }
                }
                if (pos > -1) {
                    horaIni = moment(evtHot.ListaHorasOperacion[pos].Hophorfin).format('HH:mm:ss');
                }
                else {
                    horaIni = "00:00:00";
                }
            }
            else {
                nuevo = true;
            }
            break;
        case 4: //Hidraulicas
        case 37: //Solares
        case 39: //Eolicas
            if (evtHot.ListaHorasOperacion.length > 0) {

                for (var i = 0; i < evtHot.ListaHorasOperacion.length; i++) {
                    if (evtHot.ListaHorasOperacion[i].Equicodi == id) {
                        var dateTo = new Date(moment(evtHot.ListaHorasOperacion[i].Hophorfin));
                        if (moment(dateMax).isBefore(dateTo)) {
                            dateMax = dateTo;
                            pos = i;
                        }
                    }
                }
                if (pos > -1) {
                    horaIni = moment(evtHot.ListaHorasOperacion[pos].Hophorfin).format('HH:mm:ss');
                }
                else {
                    horaIni = "00:00:00";
                }
            }
            else {
                nuevo = true;
            }
            break;
    }
    horaFin = "00:00:00";
    if (!nuevo && posicion == -1) {
        $('#txtEnParaleloH').val(horaIni);
        $('#txtFueraParaleloH').val(horaFin);
    }
}

function activarDesactivarCampos(value) {
    //Validacion, si la central es termo, no debe permitir editar el arranque y el orden de parada
    var habilitadoArranqueParada = true;
    switch (evtHot.IdTipoCentral) {
        case 5: //Térmicas
            habilitadoArranqueParada = true;
            break;
        case 4: //Hidraulicas
        case 37: //Solares
        case 39: //Eolicas
            habilitadoArranqueParada = false;
            break;
    }

    if (value) {
        habilitadoArranqueParada = true;
        document.getElementById("txtOrdenParadaH").value = "";
        document.getElementById("txtOrdenParadaF").value = '';
    } else {
        document.getElementById("txtOrdenParadaF").value = $("#hfFecha").val()
    }

    document.getElementById("txtOrdenArranqueH").disabled = habilitadoArranqueParada;
    document.getElementById("txtOrdenParadaH").disabled = habilitadoArranqueParada;

    unidEsp_HabilitacionCheckFS(value);
}

// actualiza el valor del tipo de operacion seleccionado del combobox padre
function seteaCombosHijosunidades(tipoOperacion) {
    for (var i = 0; i < evtHot.ListaUnidades.length; i++) {
        $("#cb" + evtHot.ListaUnidades[i].Equicodi).val(tipoOperacion);
    }
}
