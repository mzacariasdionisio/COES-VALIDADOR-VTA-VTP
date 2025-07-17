var controler = siteRoot + "Intervenciones/Registro/";
var controlador = siteRoot + "Intervenciones/Registro/";
var APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO = 0;
var INTERVENCION_WEB = null;
var INTERVENCION_GLOBAL = null;

var OPCION_VER = 1;
var OPCION_EDITAR = 2;
var OPCION_NUEVO = 3;
var OPCION_ACTUAL = 0;

const GENERADOR_HIDRO = "2", GENERADOR_TERMO = "3", GENERADOR_SOLAR = "36", GENERADOR_EOLICO = "38";
const CENTRAL_HIDRO = "4", CENTRAL_TERMO = "5", CENTRAL_SOLAR = "37", CENTRAL_EOLICA = "39";

var listaGenerCent = [GENERADOR_HIDRO, GENERADOR_TERMO, GENERADOR_SOLAR, GENERADOR_EOLICO, CENTRAL_HIDRO, CENTRAL_TERMO, CENTRAL_SOLAR, CENTRAL_EOLICA];

function cargarEventosJs() {
    $('#inifecha').unbind();
    $('#inifecha').Zebra_DatePicker({
        direction: [document.getElementById('Progrfechaini').value, convertirDateToString(obtenerFechaFinProgramacion24(document.getElementById('Progrfechafin').value))],
    });
    $('#finfecha').unbind();
    $('#finfecha').Zebra_DatePicker({
        direction: [document.getElementById('Progrfechaini').value, convertirDateToString(obtenerFechaFinProgramacion24(document.getElementById('Progrfechafin').value))],
    });

    $("#inihora").unbind();
    $("#inihora").on("change paste keyup", function () {
        if ($(this).val() == "24" && $("#iniminuto").val() == "00") {
            var fechaIni = obtenerFechaFinProgramacion24($("#inifecha").val());
            var fechaFinProg = obtenerFechaFinProgramacion24(document.getElementById('Progrfechafin').value);

            if (fechaIni > fechaFinProg) {
                alert("Debe ser menor o igual que la fecha de Fin del Programa ");
                $("#inihora").val("00");
                $("#iniminuto").val("00");
            }
            else {
                $("#inifecha").val(convertirDateToString(fechaIni));
                $("#inihora").val("00");
                $("#iniminuto").val("00");
            }
        }
    });

    $("#finhora").unbind();
    $("#finhora").on("change paste keyup", function () {
        if ($(this).val() == "24") {
            var fechaFin = obtenerFechaFinProgramacion24($("#finfecha").val());
            var fechaFinProg = obtenerFechaFinProgramacion24(document.getElementById('Progrfechafin').value);

            if (fechaFin > fechaFinProg) {
                alert("Debe ser menor o igual que la fecha de Fin del Programa ");
                $("#finhora").val("00");
                $("#finminuto").val("00");
            }
            else {
                $("#finfecha").val(convertirDateToString(fechaFin));
                $("#finhora").val("00");
                $("#finminuto").val("00");
            }
        }
    });

    $("#inihora, #iniminuto, #finhora, #finminuto").bind('keypress', function (e) {
        if (e.keyCode == '9' || e.keyCode == '16') {
            return;
        }
        var code;
        if (e.keyCode) code = e.keyCode;
        else if (e.which) code = e.which;
        if (e.which == 46)
            return false;
        if (code == 8 || code == 46)
            return true;
        if (code < 48 || code > 57)
            return false;
    });

    $("#iniminuto").unbind();
    $("#finminuto").unbind();
    $('#iniminuto, #inihora, #finhora, #finminuto').blur(function (params) {
        while ($(this).val().length < 2)
            $(this).val('0' + $(this).val());
    });

    $('#MWIndisponibles').unbind();
    $('#MWIndisponibles').bind('keypress', function (e) {
        var key = window.Event ? e.which : e.keyCode
        return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
    });

    $("#registrar").unbind();
    $("#registrar").click(function () {
        $("#hfConfirmarValIntervEjec").val(0);
        $("#hfConfirmarValIntervHo").val(0);

        $("#hfResultadoValIntervEjec").val(0);
        $("#hfResultadoValInterv").val(0);

        guardarIntervencion();
    });

    $("#confirmarGuardado").unbind();
    $("#confirmarGuardado").click(function (e) {
        var valEjec = parseInt($("#hfResultadoValIntervEjec").val()) || 0;
        var valHo = parseInt($("#hfResultadoValInterv").val()) || 0;
        if (valEjec > 0)
            $("#hfConfirmarValIntervEjec").val(1);
        if (valHo > 0)
            $("#hfConfirmarValIntervHo").val(1);
        guardarIntervencion();
    });

    $("#btnRegresarTop").unbind();
    $('#btnRegresarTop').click(function () {
        location.href = controlador + "IntervencionesRegistro" + "?progCodi=" + INTERVENCION_WEB.Progrcodi + '&tipoProgramacion=' + INTERVENCION_WEB.Evenclasecodi;
    });

    if ($("#AccionNuevo").val() != '') {
    }
    else {
        habilitarPr25Edit();
    }

    $('#btnBuscarEquipoPopup').unbind();
    $('#btnBuscarEquipoPopup').click(function () {
        visualizarBuscarEquipo();
    });

    $("#ChkPr25").unbind();
    $("#ChkPr25").on('click', function () {
        habilitarPr25();
    });

    $("#cboTipoIndisp25").unbind();
    $(document).on('change', '#cboTipoIndisp25', function () {
        if ($("#AccionNuevo").val() != '') {
            $("#Entidad_Interpr").val("");
        }
        else {
            var tipoInd = $(this).val();
            if (tipoInd == "PT" || tipoInd == "FT" || tipoInd == "-1") {
                $("#Entidad_Interpr").val("");
            }
        }
    });

    $("#btnActualizar").unbind();
    $("#btnActualizar").click(function () {
        $("#hfConfirmarValIntervEjec").val(0);
        $("#hfConfirmarValIntervHo").val(0);
        $("#hfResultadoValIntervEjec").val(0);
        $("#hfResultadoValInterv").val(0);
        guardarIntervencion();
    });

    //Cambia Tipo Intervención
    $("#cboTipoEventoPopup").unbind();
    $('#cboTipoEventoPopup').on("change", function () {
        if (OPCION_ACTUAL == OPCION_EDITAR) {
            if (INTERVENCION_WEB.Tipoevencodi != $("#cboTipoEvento").val()) {
                $("#codseguimiento").val("");
            }
            else {
                $("#codseguimiento").val(INTERVENCION_WEB.Intercodsegempr);
            }
        }

        //si el agente cambia de tipo de intervencion, el sistema verifica si corresponde sustento
        ActualizarIntervencionJson();
        var msj = validarIntervencionFormulario(INTERVENCION_WEB);
        if (msj == "") {
            st_VerificarIntervencionPuedeTenerSustento(INTERVENCION_WEB.Evenclasecodi);
        }

    });

    //Cambia Descripción
    $("#descrip").unbind();
    $("#descrip").on('change', function () {
        $("#descrip").val(validarTextoSinSaltoLinea($("#descrip").val()));

        //edición
        if (OPCION_ACTUAL == OPCION_EDITAR) {
            if (INTERVENCION_WEB.Interdescrip != $("#descrip").val()) {
                var tieneCambio = validarExisteCambioDescripcionOEquipo(INTERVENCION_WEB.Intercodi, INTERVENCION_WEB.Equicodi, INTERVENCION_WEB.Interdescrip, $("#descrip").val());
                if (tieneCambio) {
                    $("#codseguimiento").val("");
                } else {
                    $("#codseguimiento").val(INTERVENCION_WEB.Intercodsegempr);
                }
            }
            else {
                $("#codseguimiento").val(INTERVENCION_WEB.Intercodsegempr);
            }
        } else {
            $("#codseguimiento").val("");
        }
    });
    $('#descrip').keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            return false;
        }
    })

    //Cambia justificación
    $("#justificacion").unbind();
    $('#justificacion').keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            return false;
        }
    })
}

//Obtener Intervención
function mostrarIntervencion(objParam) {
    $("#popupFormIntervencion").html("");

    INTERVENCION_WEB = null;
    INTERVENCION_GLOBAL = null;
    LISTA_SECCION_ARCHIVO_X_INTERVENCION = [];
    PLANTILLA_FORMULARIO_INCLUSION = null;
    PLANTILLA_FORMULARIO_EXCLUSION = null;

    $.ajax({
        type: 'POST',
        url: controlador + "ObtenerIntervencion",
        dataType: 'json',
        data: objParam,
        success: function (evt) {
            if (evt.Resultado != "-1") {
                INTERVENCION_WEB = evt.Entidad;
                INTERVENCION_GLOBAL = evt;

                //acciones
                if (evt.AccionNuevo) {
                    OPCION_ACTUAL = OPCION_NUEVO;
                } else {
                    if (evt.AccionEditar) {
                        OPCION_ACTUAL = OPCION_EDITAR;
                    }
                    else {
                        OPCION_ACTUAL = OPCION_VER;
                    }
                }

                //formulario de sustento
                if (INTERVENCION_WEB.Sustento != null) {
                    if (INTERVENCION_WEB.Sustento.FlagTieneInclusion) PLANTILLA_FORMULARIO_INCLUSION = JSON.parse(JSON.stringify(INTERVENCION_WEB));
                    if (INTERVENCION_WEB.Sustento.FlagTieneExclusion) PLANTILLA_FORMULARIO_EXCLUSION = JSON.parse(JSON.stringify(INTERVENCION_WEB));
                }

                //Inicializar Formulario
                $("#popupFormIntervencion").html(generarHtmlFormularioIntervencionFlotante(evt));
                inicializarVistaFormulario();
                cargarEventosJs();

                //si el agente editar un registro de linea, el sistema verifica si puede tener o no sustento
                if (OPCION_ACTUAL == OPCION_EDITAR) {
                    st_VerificarIntervencionPuedeTenerSustento(INTERVENCION_WEB.Evenclasecodi);
                }

                //mostrar archivos
                var seccion = {
                    Inpstidesc: '',
                    EsEditable: false,
                    ListaArchivo: INTERVENCION_WEB.ListaArchivo,
                    Modulo: TIPO_MODULO_INTERVENCION,
                    Progrcodi: INTERVENCION_WEB.Progrcodi,
                    Carpetafiles: INTERVENCION_WEB.Intercarpetafiles,
                    Subcarpetafiles: 0,
                    TipoArchivo: TIPO_ARCHIVO_INTERVENCION,
                    IdDiv: `html_archivos`,
                    IdDivVistaPrevia: `vistaprevia`,
                    IdPrefijo: arch_getIdPrefijo(0)
                };
                LISTA_SECCION_ARCHIVO_X_INTERVENCION.push(seccion);
                arch_cargarHtmlArchivoEnPrograma(seccion.IdDiv, seccion);

                //mostrar ventana flotante
                $('#popupFormIntervencion').bPopup({
                    modalClose: false,
                    easing: 'easeOutBack',
                    speed: 450,
                    transition: 'slideDown',
                    onClose: function () {
                        $('#popup').empty();
                    }
                });
            } else {
                alert("Ha ocurrido un error: " + evt.Mensaje);
            }
        },
        error: function (err) {
            alert("Error al cargar Intervención");
        }
    });
}

//Generar html
function generarHtmlFormularioIntervencionFlotante(model) {

    var htmlTitulo = "";
    if (model.AccionNuevo) {
        htmlTitulo +=
            `Registro de Intervención`
    }
    else if (model.AccionEditar) {
        htmlTitulo += `
            Editar Intervención
        `;
    }
    else {
        htmlTitulo += `
         Ver Intervención
        `;
    }

    var htmlBotonAccion = "";
    if (!model.EsDesabilitado) {
        htmlBotonAccion += `   
                <input type="button" value="Actualizar" id="btnActualizar"  style="float: right;">
                <input type="button" value="Confirmar" id="confirmarGuardado" style="float: right;display: none">
   `;
    }

    var htmlFormulario = _getHtmlFormulario(model);

    var html = `
        <div class="popup-title">
            <span>${htmlTitulo}</span>

            <input type="button" id="bSalir" value="Salir" class='b-close' style='float: right; margin-right: 35px;' />
            ${htmlBotonAccion}
        </div>

        <div class="ast" style="height: 600px; overflow-y: auto">

            <table cellpadding="5">
                <tr class="fila_val_intervenciones_ejec" style="display: none">
                    <td colspan="2">
                        <table style="margin-top: 0px; margin-bottom: 0px;">
                            <tr>
                                <td>
                                    <i>Presione el botón <b>Confirmar</b> <span style="color: blue;">Se guardará con fechas y horas iguales al Mantenimiento Programado.</span></i>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="fila_val_intervenciones_alertas" style="display: none">
                    <td colspan="2">
                        <table style="margin-top: 0px; margin-bottom: 0px;">
                            <tr>
                                <td>
                                    <div id="div_msj_val_intervenciones" style="text-indent: 0px;">
                                        <div id="div_msj_val_ho"></div>
                                        <div id="div_msj_val_scada"></div>
                                        <div id="div_msj_val_ems"></div>
                                        <div id="div_msj_val_idcc"></div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <i>Presione el botón <b>Confirmar</b> para guardar la intervención, <span style="color: blue;">esta acción enviará un correo electrónico al Administrador.</span></i>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <div class="content-hijo" id="mainLayoutEdit" style="padding-top:8px; padding-left: 0px; padding-right: 0px;" hidden>

                <div class="errorTxt"></div>
                <div id="mensaje2" class="action-alert" style="margin:0; margin-bottom:10px" hidden>Por Favor complete los datos solicitados</div>

                ${htmlFormulario}
            </div>
        </div>
    `;

    return html;
}

function _getHtmlFormulario(model) {
    var htmlSeleccionarPrograma = "";
    htmlSeleccionarPrograma = `
            <tr>
                <td align="right">Tipo Programación:</td>
                <td>
                    <select style="background-color:white" onchange="listarProgramacionesXHorizonte();" id="cboTipoProgramacionPopup" name="Entidad.Evenclasecodi">
                    </select>
                </td>
                <td style="width:70px" align="right">Programación:</td>
                <td colspan="3">
                    <select style="background-color:white;width: 100%;" onchange="obtenerFechaOperacionPopup();" id="cboProgramacionPopup" name="Entidad.Progrcodi"></select>

                    <div id="programacionInfo" style="display: none"></div>
                </td>
            </tr>
    `;

    var visibilidadSustentoIncl = "display: none";
    var idTipoprog = parseInt($("#idTipoProgramacion").val()) || 0;
    //if (OPCION_ACTUAL == OPCION_NUEVO && (EVENCLASECODI_SEMANAL == idTipoprog || EVENCLASECODI_DIARIO == idTipoprog)) visibilidadSustentoIncl = "";

    var htmlFormulario = `
<input type="hidden" id="InterCodi" value="${model.IdIntervencion}" />
<input type="hidden" id="ProgrcodiForm" value="${model.IdProgramacion}" />
<input type="hidden" id="sEmprcodi" value="${model.sIdsEmpresas}" />
<input type="hidden" id="Progrfechafin" value="" />
<input type="hidden" id="ProgrfechafinH" value="" />
<input type="hidden" id="ProgrfechafinM" value="" />
<input type="hidden" id="Progrfechaini" value="" />
<input type="hidden" id="ProgrfechainiH" value="" />
<input type="hidden" id="ProgrfechainiM" value="" />
<input type="hidden" id="NombreModulo" value="${model.sModulo}" />

<input type="hidden" id="hfConfirmarValIntervEjec" name="FlagConfirmarValInterEjec" value="0" />
<input type="hidden" id="hfConfirmarValIntervHo" name="FlagConfirmarValInter" value="0" />
<input type="hidden" id="hfResultadoValIntervEjec" value="0" />
<input type="hidden" id="hfResultadoValInterv" value="0" />
<input type="hidden" id="hfFechaHoraActualEjec" value="${model.FechaHoy}" />

<input type="hidden" id="estado" value="" />

<input type="hidden" id="AccionNuevo" value="${model.AccionNuevo}" />
<input type="hidden" id="AccionEditar" value="${model.AccionEditar}" />

<input type="hidden" id="EsDasabilitadoCodSeguimiento" value="${model.EsDasabilitadoCodSeguimiento}" />
<input type="hidden" id="EsDasabilitado" value="${model.EsDesabilitado}" />

<table id="tablaDatos" cellpadding="5" style='width: 100%;'>
    <thead>

        ${htmlSeleccionarPrograma}

        <tr>
            <td>Empresa:</td>
            <td>
                <input type="hidden" id="cboEmpresaPopup" name="Entidad.Emprcodi" />
                <input type="text" id="nomEmpresa" disabled />
            </td>
            <td style="width:30px">Ubicación:</td>
            <td>
                <input type="hidden" id="cboUbicacionPopup" name="Entidad.Areacodi">
                <input type="text" id="nomUbicacion" disabled>
            </td>
            <td style="width:30px">Equipo:</td>
            <td>
                <input type="hidden" id="cboEquipoPopup" name="Entidad.Equicodi">
                <input type="text" id="nomEquipo" disabled style="width: 155px;">

                <input type="hidden" id="famAbrev">
                <input type="hidden" id="hffamcodi">
                <input type="hidden" id="hfgrupotipocogen">
                <input type="button" value="Buscar" id="btnBuscarEquipoPopup">
            </td>
        </tr>

        <tr class="fila_form_reg">
            <td style='width: 113px;'>Tipo Intervención:</td>
            <td>
                <select style="background-color:white" id="cboTipoEventoPopup" onchange="causas();" name="Entidad.Tipoevencodi">
                </select>
            </td>
            <td style="width:65px">Causa:</td>
            <td style="width: 228px">
                <select style="background-color:white" id="cboCausaPopup" name="Entidad.Subcausacodi">
                </select>
            </td>
            <td style="width: 116px;">Clase Programación:</td>
            <td>
                <select style="background-color:white;width: 170px;" id="cboClaseProgramacionPopup" name="Entidad.Claprocodi">
                </select>
            </td>
        </tr>

        <tr class="fila_form_reg">
            <td>Fecha inicio:</td>
            <td class="editor-label" id="fechaInicio" style="width:112px; background-color:white; padding-bottom: 0px;">
                <input class="txtFecha" type="text" value="" id="inifecha" style="width: 122px" />

                <table style="width:30px;float: right;">
                    <tr>
                        <td style="padding-top: 0px;">
                            <input type="text" id="inihora" value="" style="background-color: white; width: 30px" min="0" max="24" maxlength="2" autocomplete="off" />
                        <td style="padding-top: 0px"><strong>:</strong></td>
                        <td style="padding-top: 0px">
                            <input type="text" id="iniminuto" value="" style="background-color: white; width: 30px" min="0" max="59" maxlength="2" autocomplete="off" />
                        </td>
                    </tr>
                </table>
            </td>

            <td rowspan="2">Disponibilidad:</td>
            <td rowspan="2">
                <div>
                    <input type="radio" id="fueraservicio" name="Interindispo" value="" checked="checked"> <span>F/S (Fuera de Servicio)</span>
                </div>
                <div>
                    <input type="radio" id="enservicio" name="Interindispo" value=""> <span>E/S (En Servicio)</span>
                </div>
            </td>
            <td rowspan="2" colspan="6">
                <div>
                    <input type="checkbox" value="" id="ChkInterinterrup"> <span>Provoca Interrupción</span>

                    <input type="checkbox" value="" id="ChkInterconexionprov" style='margin-left: 43px;'> <span>Intervención Provisional</span>
                </div>
                <div>
                    <input type="checkbox" value="" id="ChkIntersistemaaislado"> <span>Sistema Aislado</span>
                </div>
            </td>
        </tr>
        <tr class="fila_form_reg">
            <td style='padding-top: 0px;'>Fecha Fin:</td>
            <td class="editor-label" id="fechaFin" style="width: 100px; background-color:white;padding-top: 0px;">
                <input class="txtFecha" type="text" value="" id="finfecha" style="width: 122px" />

                <table style="width:30px;float: right;">
                    <tr>
                        <td style="padding-top: 0px">
                            <input type="text" id="finhora" required pattern="[0-9]{1,2}" value="" style="background-color: white; width: 30px" min="0" max="24" maxlength="2" autocomplete="off" />
                        </td>
                        <td style="padding-top: 0px"><strong>:</strong></td>
                        <td style="padding-top: 0px">
                            <input type="text" id="finminuto" value="" style="background-color: white; width: 30px" min="0" max="59" maxlength="2" autocomplete="off" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr></tr>

<!--Segunda seccion -->
        <tr class="fila_form_reg" id="trOperador" hidden>
            <td>Operador:</td>
            <td>
                <input type="hidden" id="Operadoremprcodi" name="Entidad.Operadoremprcodi" value="" />
                <input type="text" id="nomOperador" value="" disabled />
            </td>
        </tr>

        <tr class="fila_form_reg">
            <td>MW Indisponibles:</td>
            <td>
                <input type="text" id="MWIndisponibles" value="" style="background-color: white; width: 50px" autocomplete="off" />
            </td>
            <td rowspan="2" colspan="5">
                <!--Inicio PR25 Indisponibilidades-->
                <table style="border: 2px solid blue;    float: left;width:500px" id="tblIndisponibilidades" hidden>
                    <tr>
                        <td colspan="2" style="font-style: italic; padding-bottom: 15px;">
                            La coordinación de estos campos declarativos (que no afectan al Programa) <br /> debe realizarse con <b style="color: blue">sme_pr25@coes.org.pe</b>
                        </td>
                    </tr>


                    <tr>
                        <td style="text-align: right;">
                            <input type="checkbox" value="" id="ChkPr25">
                        </td>
                        <td>
                            Corresponde a Indisponibilidad en aplicación del PR-25.
                        </td>
                    </tr>

                    <tr id="tr_cb_tipoindisp" style="display: none">
                        <td style="text-align: right;">Tipo:</td>
                        <td>
                            <select style="background-color:white" id="cboTipoIndisp25" onchange="tipoIndisp25();" name="Entidad.Intertipoindisp">
                            </select>
                        </td>
                    </tr>

                    <tr id="tr_pr" style="display: none">
                        <td>Potencia Restringida:</td>
                        <td>
                            <input type="text" id="potrestringida" value="" style="background-color: white; width: 50px" autocomplete="off" /> MW
                        </td>
                    </tr>

                    <tr id="tr_asocproc" style="display: none">
                        <td style="text-align: right;">
                            <input type="radio" id="asocprocSI" name="asocproc_form" value="S"> <span>Sí</span>
                            <input type="radio" id="asocprocNO" name="asocproc_form" value="N"> <span>No</span>
                        </td>
                        <td>
                            Asociado al proceso Productivo
                        </td>
                    </tr>

                </table>
                <!--Fin PR25 Indisponibilidades-->
            </td>
        </tr>
        <tr class="fila_form_reg">
            <td>Cod. Seguimiento:</td>
            <td>
                <input type="text" id="codseguimiento" value="" style="background-color: white;" autocomplete="off" />
            </td>
        </tr>
        <tr class="fila_form_reg">
            <td>Descripción:</td>
            <td colspan="5">
                <textarea id="descrip" name="Interdescrip" value="" style="background-color: white; width:100%; height:44px"></textarea>
            </td>
        </tr>
        <tr class="fila_form_reg" id="trJustificacion" hidden>
            <td>Justificación:</td>
            <td colspan="5">
                <textarea id="justificacion" name="Interjustifaprobrechaz" value="" style="background-color: white; width:100%; height:44px"></textarea>
            </td>
        </tr>
        <tr class="fila_form_reg" id="trSustento">
            <td></td>
            <td colspan="5">
                <input type="button" value="Sustento de Inclusión" id="btnSustentoInclPopup" onclick="st_CargarFormularioSustento('${TIPO_INCLUSION}', 0)" style='${visibilidadSustentoIncl}'>
                <input type="button" value="Sustento de Exclusión" id="btnSustentoExclPopup" onclick="st_CargarFormularioSustento('${TIPO_EXCLUSION}', 0)" style='display: none'>
            </td>
        </tr>
    </thead>

</table>

<div id="divmensaje" style="margin-top: 8px;" hidden>
    <i><strong>(*) Al cambiar el tipo de intervención y/o Descripción se guardará como una nueva intervención</strong> </i>
</div>


<div id="html_archivos" style="margin-top: 10px;">

</div>
                    
<div>
    <iframe id="vistaprevia" style="width: 100%; height:500px;" frameborder="0" hidden></iframe>
</div>

    `;

    return htmlFormulario;
}

//inicializa vista formulario
function inicializarVistaFormulario() {
    document.getElementById('Progrfechaini').value = INTERVENCION_GLOBAL.Progrfechaini;
    document.getElementById('ProgrfechainiH').value = INTERVENCION_GLOBAL.ProgrfechainiH;
    document.getElementById('ProgrfechainiM').value = INTERVENCION_GLOBAL.ProgrfechainiM;

    document.getElementById('Progrfechafin').value = INTERVENCION_GLOBAL.Progrfechafin;
    document.getElementById('ProgrfechafinH').value = INTERVENCION_GLOBAL.ProgrfechafinH;
    document.getElementById('ProgrfechafinM').value = INTERVENCION_GLOBAL.ProgrfechafinM;

    document.getElementById('NombreModulo').value = INTERVENCION_GLOBAL.sModulo;

    switch (OPCION_ACTUAL) {
        case OPCION_VER:
            $("#mainLayoutEdit").show();

            $("#cboTipoProgramacionPopup").prop('disabled', 'disabled');
            $("#cboProgramacionPopup").prop('disabled', 'disabled');
            _setearFechasDescProgramacion(INTERVENCION_GLOBAL);

            $("#nomEmpresa").prop('disabled', 'disabled');
            $("#nomUbicacion").prop('disabled', 'disabled');
            $("#nomEquipo").prop('disabled', 'disabled');
            $("#btnBuscarEquipoPopup").hide();
            //$("#tblIndisponibilidades").hide();

            $("#cboTipoEventoPopup").prop('disabled', 'disabled');
            document.getElementById('cboTipoEventoPopup').style.backgroundColor = '';
            $("#cboCausaPopup").prop('disabled', 'disabled');
            document.getElementById('cboCausaPopup').style.backgroundColor = '';
            $("#cboClaseProgramacionPopup").prop('disabled', 'disabled');
            document.getElementById('cboClaseProgramacionPopup').style.backgroundColor = '';

            $("#inifecha").prop('disabled', 'disabled');
            //$(".Zebra_DatePicker_Icon")[2].classList.add('Zebra_DatePicker_Icon_Disabled');
            document.getElementById('inifecha').style.backgroundColor = '';
            $("#inihora").prop('disabled', 'disabled');
            document.getElementById('inihora').style.backgroundColor = '';
            $("#iniminuto").prop('disabled', 'disabled');
            document.getElementById('iniminuto').style.backgroundColor = '';
            $("#finfecha").prop('disabled', 'disabled');
            //$(".Zebra_DatePicker_Icon")[3].classList.add('Zebra_DatePicker_Icon_Disabled');
            document.getElementById('finfecha').style.backgroundColor = '';
            $("#finhora").prop('disabled', 'disabled');
            document.getElementById('finhora').style.backgroundColor = '';
            $("#finminuto").prop('disabled', 'disabled');
            document.getElementById('finminuto').style.backgroundColor = '';

            $("#fueraservicio").prop('disabled', 'disabled');
            $("#enservicio").prop('disabled', 'disabled');
            $("#ChkInterinterrup").prop('disabled', 'disabled');
            //$("#ChkIntermantrelev").prop('disabled', 'disabled');
            $("#ChkInterconexionprov").prop('disabled', 'disabled');
            $("#ChkIntersistemaaislado").prop('disabled', 'disabled');

            $("#MWIndisponibles").prop('disabled', 'disabled');
            document.getElementById('MWIndisponibles').style.backgroundColor = '';
            $("#codseguimiento").prop('disabled', 'disabled');
            document.getElementById('codseguimiento').style.backgroundColor = '';
            $("#descrip").prop('disabled', 'disabled');
            document.getElementById('descrip').style.backgroundColor = '';
            $("#trJustificacion").show();
            $("#justificacion").prop('disabled', 'disabled');
            document.getElementById('justificacion').style.backgroundColor = '';

            $("#divarchivo").hide();
            $("#container").show();
            $("#btnSelectFile").hide();
            $('.file-carga-titulo').hide();
            $("#filelist").show();

            if (INTERVENCION_GLOBAL.Esindisponilidadpr25) {
                $("#tblIndisponibilidades").show();
            }

            //.................LLENAR CAMPOS.....................
            $("#cboEmpresaPopup").val(INTERVENCION_WEB.Emprcodi);
            $("#nomEmpresa").val(INTERVENCION_WEB.EmprNomb);
            $("#cboUbicacionPopupPopup").val(INTERVENCION_WEB.Areacodi);
            $("#nomUbicacion").val(INTERVENCION_WEB.AreaNomb);
            $("#cboEquipoPopup").val(INTERVENCION_WEB.Equicodi);
            $("#nomEquipo").val(INTERVENCION_WEB.Equiabrev);

            //Listas desplegables
            inicializaDesplegables();

            $("#cboTipoEventoPopup").val(INTERVENCION_WEB.Tipoevencodi);
            $("#cboCausaPopup").val(INTERVENCION_WEB.Subcausacodi);
            $("#cboClaseProgramacionPopup").val(INTERVENCION_WEB.Claprocodi);

            $("#inifecha").val(INTERVENCION_WEB.IniFecha);
            $("#inihora").val(INTERVENCION_WEB.IniHora);
            $("#iniminuto").val(INTERVENCION_WEB.IniMinuto);
            $("#finfecha").val(INTERVENCION_WEB.FinFecha);
            $("#finhora").val(INTERVENCION_WEB.FinHora);
            $("#finminuto").val(INTERVENCION_WEB.FinMinuto);

            $("#MWIndisponibles").val(INTERVENCION_WEB.Intermwindispo);
            $("#codseguimiento").val(INTERVENCION_WEB.Intercodsegempr);
            $("#descrip").val(INTERVENCION_WEB.Interdescrip);
            $("#justificacion").val(INTERVENCION_WEB.Interjustifaprobrechaz);

            //radio
            if (INTERVENCION_WEB.Interindispo == "F") {
                $("#fueraservicio").prop('checked', true);
            }
            if (INTERVENCION_WEB.Interindispo == "E") {
                $("#enservicio").prop('checked', true);
            }

            //checks
            if (INTERVENCION_WEB.Interinterrup == "S") {
                $("#ChkInterinterrup").prop('checked', true);
            }
            //if (INTERVENCION_WEB.Intermantrelev == "1") {
            //    $("#ChkIntermantrelev").prop('checked', true);
            //}
            if (INTERVENCION_WEB.Interconexionprov == "S") {
                $("#ChkInterconexionprov").prop('checked', true);
            }
            if (INTERVENCION_WEB.Intersistemaaislado == "S") {
                $("#ChkIntersistemaaislado").prop('checked', true);
            }

            //PR25 Indisponibilidades
            if (INTERVENCION_GLOBAL.Esindisponilidadpr25) {
                $("#tblIndisponibilidades").show();
                if (INTERVENCION_GLOBAL.ChkPr25)
                    $("#ChkPr25").prop('checked', true);

                $("#cboTipoIndisp25").val(INTERVENCION_WEB.Intertipoindisp);
                $("#potrestringida").val(INTERVENCION_WEB.Interpr);

                if (INTERVENCION_WEB.Interasocproc == "S") {
                    $("#asocprocSI").prop('checked', true);
                    $("#asocprocNO").prop('checked', false);
                }
                if (INTERVENCION_WEB.Interasocproc == "N") {
                    $("#asocprocSI").prop('checked', false);
                    $("#asocprocNO").prop('checked', true);
                }

                habilitarPr25Edit();

                $("#ChkPr25").prop('disabled', 'disabled');
                $("#cboTipoIndisp25").prop('disabled', 'disabled');
                $("#potrestringida").prop('disabled', 'disabled');
                $("#asocprocSI").prop('disabled', 'disabled');
                $("#asocprocNO").prop('disabled', 'disabled');
            }
            else {
                $("#tblIndisponibilidades").hide();
            }

            //sustento de inclusión / exclusión
            st_mostrarBtnEnFormularioPrincipal();

            //....................................................
            break;
        case OPCION_EDITAR:
            $("#mainLayoutEdit").show();
            //$("#trOperador").show();
            $("#trJustificacion").show();
            $("#divmensaje").show();
            $("#btnSelectFile").show();

            $("#cboTipoProgramacionPopup").prop('disabled', 'disabled');
            $("#cboProgramacionPopup").prop('disabled', 'disabled');
            _setearFechasDescProgramacion(INTERVENCION_GLOBAL);

            $("#nomEmpresa").prop('disabled', 'disabled');
            $("#nomUbicacion").prop('disabled', 'disabled');
            $("#nomEquipo").prop('disabled', 'disabled');
            //$("#nomOperador").prop('disabled', 'disabled');
            $("#btnBuscarEquipoPopup").show();
            //$("#tblIndisponibilidades").hide();

            $("#cboTipoEventoPopup").removeAttr('disabled');
            document.getElementById('cboTipoEventoPopup').style.backgroundColor = 'white';
            $("#cboCausaPopup").removeAttr('disabled');
            document.getElementById('cboCausaPopup').style.backgroundColor = 'white';
            $("#cboClaseProgramacionPopup").removeAttr('disabled');
            document.getElementById('cboClaseProgramacionPopup').style.backgroundColor = 'white';

            $("#codseguimiento").prop('disabled', 'disabled');
            document.getElementById('codseguimiento').style.backgroundColor = '';
            document.getElementById('inifecha').style.backgroundColor = 'white';
            document.getElementById('finfecha').style.backgroundColor = 'white';

            //.................LLENAR CAMPOS....................
            $("#cboEmpresaPopup").val(INTERVENCION_WEB.Emprcodi);
            $("#nomEmpresa").val(INTERVENCION_WEB.EmprNomb);
            $("#cboUbicacionPopup").val(INTERVENCION_WEB.Areacodi);
            $("#nomUbicacion").val(INTERVENCION_WEB.AreaNomb);
            $("#cboEquipoPopup").val(INTERVENCION_WEB.Equicodi);
            $("#nomEquipo").val(INTERVENCION_WEB.Equiabrev);
            $("#hffamcodi").val(INTERVENCION_WEB.Famcodi);
            $("#hfgrupotipocogen").val(INTERVENCION_WEB.Grupotipocogen);

            //Listas desplegables
            inicializaDesplegables();

            $("#cboTipoEventoPopup").val(INTERVENCION_WEB.Tipoevencodi);
            $("#cboCausaPopup").val(INTERVENCION_WEB.Subcausacodi);
            $("#cboClaseProgramacionPopup").val(INTERVENCION_WEB.Claprocodi);

            $("#inifecha").val(INTERVENCION_WEB.IniFecha);
            $("#inihora").val(INTERVENCION_WEB.IniHora);
            $("#iniminuto").val(INTERVENCION_WEB.IniMinuto);
            $("#finfecha").val(INTERVENCION_WEB.FinFecha);
            $("#finhora").val(INTERVENCION_WEB.FinHora);
            $("#finminuto").val(INTERVENCION_WEB.FinMinuto);

            //$("#Operadoremprcodi").val(INTERVENCION_WEB.Operadoremprcodi);
            //$("#nomOperador").val(INTERVENCION_WEB.Operadornomb);
            $("#MWIndisponibles").val(INTERVENCION_WEB.Intermwindispo);
            $("#codseguimiento").val(INTERVENCION_WEB.Intercodsegempr);
            $("#descrip").val(INTERVENCION_WEB.Interdescrip);
            $("#justificacion").val(INTERVENCION_WEB.Interjustifaprobrechaz);

            //radio
            if (INTERVENCION_WEB.Interindispo == "F") {
                $("#fueraservicio").prop('checked', true);
            }
            if (INTERVENCION_WEB.Interindispo == "E") {
                $("#enservicio").prop('checked', true);
            }

            //checks
            if (INTERVENCION_WEB.Interinterrup == "S") {
                $("#ChkInterinterrup").prop('checked', true);
            }
            //if (INTERVENCION_WEB.Intermantrelev == "1") {
            //    $("#ChkIntermantrelev").prop('checked', true);
            //}
            if (INTERVENCION_WEB.Interconexionprov == "S") {
                $("#ChkInterconexionprov").prop('checked', true);
            }
            if (INTERVENCION_WEB.Intersistemaaislado == "S") {
                $("#ChkIntersistemaaislado").prop('checked', true);
            }

            //PR25 Indisponibilidades
            if (INTERVENCION_GLOBAL.Esindisponilidadpr25) {
                $("#tblIndisponibilidades").show();
                if (INTERVENCION_GLOBAL.ChkPr25)
                    $("#ChkPr25").prop('checked', true);

                $("#cboTipoIndisp25").val(INTERVENCION_WEB.Intertipoindisp);
                $("#potrestringida").val(INTERVENCION_WEB.Interpr);

                if (INTERVENCION_WEB.Interasocproc == "S") {
                    $("#asocprocSI").prop('checked', true);
                    $("#asocprocNO").prop('checked', false);
                }
                if (INTERVENCION_WEB.Interasocproc == "N") {
                    $("#asocprocSI").prop('checked', false);
                    $("#asocprocNO").prop('checked', true);
                }

                habilitarPr25Edit();
            }
            else {
                $("#tblIndisponibilidades").hide();
            }

            //sustento de inclusión / exclusión
            st_mostrarBtnEnFormularioPrincipal();

            //....................................................
            break;
        case OPCION_NUEVO:
            $("#mainLayoutEdit").show();
            $("#btnActualizar").val('Registrar');

            $("#trJustificacion").hide();
            $("#btnSelectFile").show();
            $("#btnBuscarEquipoPopup").show();

            document.getElementById('inifecha').style.backgroundColor = 'white';
            document.getElementById('finfecha').style.backgroundColor = 'white';
            $("#codseguimiento").prop('disabled', 'disabled');
            document.getElementById('codseguimiento').style.backgroundColor = '';

            $("#cboEmpresaPopup").val(INTERVENCION_WEB.Emprcodi);
            $("#nomEmpresa").val(INTERVENCION_WEB.EmprNomb);
            $("#cboUbicacionPopup").val(INTERVENCION_WEB.Areacodi);
            $("#nomUbicacion").val(INTERVENCION_WEB.AreaNomb);
            $("#cboEquipoPopup").val(INTERVENCION_WEB.Equicodi);
            $("#nomEquipo").val(INTERVENCION_WEB.Equiabrev);

            //Listas desplegables
            inicializaDesplegables();

            //.................LLENAR CAMPOS....................
            $("#cboTipoProgramacionPopup").val(INTERVENCION_GLOBAL.IdTipoProgramacion);
            $("#cboProgramacionPopup").val(INTERVENCION_GLOBAL.IdProgramacion);
            _setearFechasDescProgramacion(INTERVENCION_GLOBAL);

            $("#inifecha").val(INTERVENCION_WEB.IniFecha);
            $("#inihora").val(INTERVENCION_WEB.IniHora);
            $("#iniminuto").val(INTERVENCION_WEB.IniMinuto);
            $("#finfecha").val(INTERVENCION_WEB.FinFecha);
            $("#finhora").val(INTERVENCION_WEB.FinHora);
            $("#finminuto").val(INTERVENCION_WEB.FinMinuto);

            //$("#fueraservicio").attr('checked');
            $("#fueraservicio").prop('checked', true);
            $("#MWIndisponibles").val(INTERVENCION_WEB.Intermwindispo);
            $("#codseguimiento").val(INTERVENCION_WEB.Intercodsegempr);
            //.........................................................

            //PR25 Indisponibilidades
            if (INTERVENCION_GLOBAL.Esindisponilidadpr25) {
                $("#tblIndisponibilidades").show();
                $("#ChkPr25").val(INTERVENCION_GLOBAL.ChkPr25);
                $("#cboTipoIndisp25").val(INTERVENCION_WEB.Intertipoindisp);

                //model.Entidad.Interasocproc = "N";
                $("#asocprocNO").prop('checked', true);
            }
            else {
                $("#tblIndisponibilidades").hide();
            }
            break;
    }
}

//Inicializa Listas Desplegables
function inicializaDesplegables() {

    $('#cboTipoProgramacionPopup').empty();
    var optionTipoPrograma = '<option value =' + INTERVENCION_GLOBAL.IdTipoProgramacion + '>' + INTERVENCION_GLOBAL.Evenclasedesc + '</option>';
    $('#cboTipoProgramacionPopup').append(optionTipoPrograma);

    $('#cboProgramacionPopup').empty();
    var optionProgramacion = '<option value =' + INTERVENCION_GLOBAL.IdProgramacion + '>' + INTERVENCION_GLOBAL.NombreProgramacion + '</option>';
    $('#cboProgramacionPopup').append(optionProgramacion);

    $('#cboTipoEventoPopup').empty();
    var optionTipoInterv = '<option value="" >----- Seleccione tipo intervención ----- </option>';
    $.each(INTERVENCION_GLOBAL.ListaCboIntervencion, function (k, v) {
        optionTipoInterv += '<option value =' + v.Tipoevencodi + '>' + v.Tipoevendesc + '</option>';
    })
    $('#cboTipoEventoPopup').append(optionTipoInterv);

    $('#cboCausaPopup').empty();
    //var optionCausas = '<option value="-1" >----- Seleccione una causa ----- </option>';
    var optionCausas = '';
    $.each(INTERVENCION_GLOBAL.ListaCausas, function (k, v) {
        optionCausas += '<option value =' + v.Subcausacodi + '>' + v.Subcausadesc + '</option>';
    })
    $('#cboCausaPopup').append(optionCausas);

    $('#cboClaseProgramacionPopup').empty();
    var optionClaseProgr = '<option value="" >----- Seleccione una clase ----- </option>';
    $.each(INTERVENCION_GLOBAL.ListaClaseProgramacion, function (k, v) {
        optionClaseProgr += '<option value =' + v.Claprocodi + '>' + v.Clapronombre + '</option>';
    })
    $('#cboClaseProgramacionPopup').append(optionClaseProgr);

    //PR25 Indisponibilidades
    if (INTERVENCION_GLOBAL.Esindisponilidadpr25) {
        $('#cboTipoIndisp25').empty();
        var optionTipoIndisp25 = '<option value="-1" >----- NO DEFINIDO ----- </option>';
        $.each(INTERVENCION_GLOBAL.ListaTipoindispPr25, function (k, v) {
            optionTipoIndisp25 += '<option value =' + v.String1 + '>' + v.String2 + '</option>';
        })
        $('#cboTipoIndisp25').append(optionTipoIndisp25);
    }

}

//guardar Intervención
async function guardarIntervencion() {

    ActualizarIntervencionJson();

    var msj = validarIntervencionFormulario(INTERVENCION_WEB);
    if (msj == "") {
        await st_VerificarIntervencionPuedeTenerSustento(INTERVENCION_WEB.Evenclasecodi);

        var flagContinuar = true;
        if (!st_FlagTieneFormularioCompleto()) {
            flagContinuar = false;
        } else {
            flagContinuar = true;
        }

        //guardar datos del formulario
        if (flagContinuar) {
            _guardarIntervencionBD();
        }
    } else {
        mostrarAlerta(msj);
    }
}

function _guardarIntervencionBD() {
    limpiarMensaje();
    $("#btnActualizar").hide();
    $("#confirmarGuardado").hide();

    var dataJson = JSON.stringify(INTERVENCION_WEB).replace(/\/Date/g, "\\\/Date").replace(/\)\//g, "\)\\\/");
    var confirmarvalinter = $("#hfConfirmarValIntervHo").val();
    var confirmarvalinterejec = $("#hfConfirmarValIntervEjec").val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        traditional: true,
        url: controler + "IntervencionGuardar",
        data: {
            dataJson: dataJson,

            opcion: OPCION_ACTUAL,
            confirmarvalinter: confirmarvalinter,
            confirmarvalinterejec: confirmarvalinterejec
        },
        success: function (result) {
            if (result.Resultado != '-1' && result.IdIntervencion > 0) {

                $('#popupFormIntervencion').bPopup().close();
                alert(result.StrMensaje);
                mostrarLista(); //IntervencionesRegistro.js
            }
            else {
                $("#mensaje2").hide();
                //Validación de intervenciones
                if (result.Resultado == "2") {
                    $("#confirmarGuardado").show();
                    $("#hfResultadoValIntervEjec").val(result.TieneValidacionEjecIgualProg);
                    $("#hfResultadoValInterv").val(result.TieneValidaciones);

                    interv_mostrarAdvertenciaEjecutadoNoProgramado();
                    interv_mostrarAdvertencias(result.Entidad.Interindispo, result.ListaValidacionHorasOperacion, result.ListaValidacionScada, result.ListaValidacionEms, result.ListaValidacionIDCC);
                } else {
                    if (OPCION_ACTUAL == OPCION_NUEVO) {
                        $("#registrar").show();
                    }
                    else
                        $("#btnActualizar").show();
                    alert(result.StrMensaje);
                    $("#mensaje").addClass("action-error");
                    $("#mensaje").html("Hubo un error al guardar los datos");

                }
            }
            setTimeout(function () { $("#mensaje").fadeOut(800).fadeIn(800).fadeOut(500).fadeIn(500).fadeOut(300); }, 3000);
        },
        error: function (err) {
            alert('Ha ocurrido un error.');
        }
    });
}

//Json para nueva Intervención
function ActualizarIntervencionJson() {

    var interdescrip = $("#descrip").val();
    var justifaprobrechaz = $("#justificacion").val();
    if (interdescrip != null) interdescrip = interdescrip.trim();
    if (justifaprobrechaz != null) justifaprobrechaz = justifaprobrechaz.trim();
    $("#descrip").val(interdescrip);
    $("#justificacion").val(justifaprobrechaz);

    if (OPCION_ACTUAL == OPCION_NUEVO) {
        INTERVENCION_WEB.Progrcodi = $("#ProgrcodiForm").val();

        INTERVENCION_WEB.Emprcodi = $("#cboEmpresaPopup").val();
        INTERVENCION_WEB.Areacodi = $("#cboUbicacionPopup").val();
        INTERVENCION_WEB.Equicodi = parseInt($("#cboEquipoPopup").val()) || 0;
        INTERVENCION_WEB.Famcodi = parseInt($("#hffamcodi").val()) || 0;
        INTERVENCION_WEB.Tipoevencodi = $("#cboTipoEventoPopup").val();
        INTERVENCION_WEB.Subcausacodi = $("#cboCausaPopup").val();
        INTERVENCION_WEB.Claprocodi = $("#cboClaseProgramacionPopup").val();

        INTERVENCION_WEB.IniFecha = $("#inifecha").val();
        INTERVENCION_WEB.IniHora = $("#inihora").val();
        INTERVENCION_WEB.IniMinuto = $("#iniminuto").val();
        INTERVENCION_WEB.FinFecha = $("#finfecha").val();
        INTERVENCION_WEB.FinHora = $("#finhora").val();
        INTERVENCION_WEB.FinMinuto = $("#finminuto").val();
        INTERVENCION_WEB.Intermwindispo = $("#MWIndisponibles").val();
        INTERVENCION_WEB.Intercodsegempr = $("#codseguimiento").val();
        INTERVENCION_WEB.Interdescrip = $("#descrip").val();

        //valores actuales para formulario de sustento
        INTERVENCION_WEB.InterfechainiDesc = INTERVENCION_WEB.IniFecha + " " + INTERVENCION_WEB.IniHora + ":" + INTERVENCION_WEB.IniMinuto;
        INTERVENCION_WEB.InterfechafinDesc = INTERVENCION_WEB.FinFecha + " " + INTERVENCION_WEB.FinHora + ":" + INTERVENCION_WEB.FinMinuto;
        INTERVENCION_WEB.TipoEvenDesc = $("#cboTipoEventoPopup option:selected").text() ?? "";
    }
    if (OPCION_ACTUAL == OPCION_EDITAR) {
        INTERVENCION_WEB.Tipoevencodi = $("#cboTipoEventoPopup").val();
        INTERVENCION_WEB.Subcausacodi = $("#cboCausaPopup").val();
        INTERVENCION_WEB.Claprocodi = $("#cboClaseProgramacionPopup").val();

        INTERVENCION_WEB.IniFecha = $("#inifecha").val();
        INTERVENCION_WEB.IniHora = $("#inihora").val();
        INTERVENCION_WEB.IniMinuto = $("#iniminuto").val();
        INTERVENCION_WEB.FinFecha = $("#finfecha").val();
        INTERVENCION_WEB.FinHora = $("#finhora").val();
        INTERVENCION_WEB.FinMinuto = $("#finminuto").val();
        INTERVENCION_WEB.Intermwindispo = $("#MWIndisponibles").val();
        INTERVENCION_WEB.Intercodsegempr = $("#codseguimiento").val();
        INTERVENCION_WEB.Interdescrip = $("#descrip").val();
        INTERVENCION_WEB.Interjustifaprobrechaz = $("#justificacion").val();

        //valores actuales para formulario de sustento
        INTERVENCION_WEB.InterfechainiDesc = INTERVENCION_WEB.IniFecha + " " + INTERVENCION_WEB.IniHora + ":" + INTERVENCION_WEB.IniMinuto;
        INTERVENCION_WEB.InterfechafinDesc = INTERVENCION_WEB.FinFecha + " " + INTERVENCION_WEB.FinHora + ":" + INTERVENCION_WEB.FinMinuto;
        INTERVENCION_WEB.TipoEvenDesc = $("#cboTipoEventoPopup option:selected").text() ?? "";
    }

    //radio
    var fueraservicio = $("#fueraservicio").is(':checked');
    var enservicio = $("#enservicio").is(':checked');
    if (fueraservicio) {
        INTERVENCION_WEB.Interindispo = "F";
    }
    if (enservicio) {
        INTERVENCION_WEB.Interindispo = "E";
    }

    //checks
    var chkInterinterrup = $("#ChkInterinterrup").is(':checked');
    //var chkIntermantrelev = $("#ChkIntermantrelev").is(':checked');
    var chkInterconexionprov = $("#ChkInterconexionprov").is(':checked');
    var chkIntersistemaaislado = $("#ChkIntersistemaaislado").is(':checked');
    if (chkInterinterrup) {
        INTERVENCION_WEB.Interinterrup = "S";
    } else {
        INTERVENCION_WEB.Interinterrup = "N";
    }
    //if (chkIntermantrelev) {
    //    INTERVENCION_WEB.Intermantrelev = "1";
    //} else {
    //    INTERVENCION_WEB.Intermantrelev = "0";
    //}
    if (chkInterconexionprov) {
        INTERVENCION_WEB.Interconexionprov = "S";
    } else {
        INTERVENCION_WEB.Interconexionprov = "N";
    }
    if (chkIntersistemaaislado) {
        INTERVENCION_WEB.Intersistemaaislado = "S";
    } else {
        INTERVENCION_WEB.Intersistemaaislado = "N";
    }

    //PR25 Indisponibilidades
    if (INTERVENCION_GLOBAL.Esindisponilidadpr25) {
        var cogeneracion = $("#hfgrupotipocogen").val();

        if ($("#ChkPr25").is(':checked')) {
            var valor = $("#cboTipoIndisp25").val();
            INTERVENCION_WEB.Intertipoindisp = valor;
            if (valor == "PP" || valor == "FP") {
                INTERVENCION_WEB.Interpr = $("#potrestringida").val();
            }

            if (cogeneracion == "S") {
                var asocprocSI = $("#asocprocSI").is(':checked');
                var asocprocNO = $("#asocprocNO").is(':checked');
                if (asocprocSI) {
                    INTERVENCION_WEB.Interasocproc = "S";
                }
                if (asocprocNO) {
                    INTERVENCION_WEB.Interasocproc = "N";
                }
            }
        }
    }

    //archivos
    INTERVENCION_WEB.ListaArchivo = LISTA_SECCION_ARCHIVO_X_INTERVENCION[0].ListaArchivo;

    //sustento de inclusión / exclusión
    INTERVENCION_WEB.Sustento = null;
    if (PLANTILLA_FORMULARIO_INCLUSION != null) INTERVENCION_WEB.Sustento = PLANTILLA_FORMULARIO_INCLUSION.Sustento;
    INTERVENCION_WEB.SustentoOld = null;
    if (PLANTILLA_FORMULARIO_EXCLUSION != null) INTERVENCION_WEB.SustentoOld = PLANTILLA_FORMULARIO_EXCLUSION.Sustento;
}

//validar Intervencion
function validarIntervencionFormulario(obj) {
    var validacion = "<ul>";
    var flag = true;
    if (obj.Emprcodi == null || obj.Emprcodi == '') {
        validacion = validacion + "<li>Empresa: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (obj.Areacodi == null || obj.Areacodi == '') {
        validacion = validacion + "<li>Ubicación: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (obj.Equicodi == null || obj.Equicodi == '') {
        validacion = validacion + "<li>Equipo: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (obj.Tipoevencodi == null || obj.Tipoevencodi == '') {
        validacion = validacion + "<li>Tipo de Intervención: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (obj.Subcausacodi == null || obj.Subcausacodi == '') {
        validacion = validacion + "<li>Causa: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    if (obj.Claprocodi == null || obj.Claprocodi == '') {
        validacion = validacion + "<li>Clase Programación: campo requerido.</li>";//Campo Requerido
        flag = false;
    }

    //............................HORA FIN.........................
    if (obj.FinHora == null || obj.FinHora == '') {
        validacion = validacion + "<li>Hora fin: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero(obj.FinHora)) {
            var horafin = parseInt(obj.FinHora);
            if (horafin > 24) {
                validacion = validacion + "<li>Hora Fin: Ingrese un valor menor o igual que 24.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>Hora Fin: Debe ser un número.</li>";
            flag = false;
        }
    }

    //............................MINUTO FIN.........................
    if (obj.FinMinuto == null || obj.FinMinuto == '') {
        validacion = validacion + "<li>Minuto fin: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero(obj.FinMinuto)) {
            var minutofin = parseInt(obj.FinMinuto);
            if (minutofin > 59) {
                validacion = validacion + "<li>Minuto fin: Ingrese un valor menor o igual que 59.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>Minuto Fin: Debe ser un número.</li>";
            flag = false;
        }
    }

    //............................HORA iNICIO.........................
    if (obj.IniHora == null || obj.IniHora == '') {
        validacion = validacion + "<li>Hora Inicio: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero(obj.IniHora)) {
            var horainicio = parseInt(obj.IniHora);
            if (horainicio > 24) {
                validacion = validacion + "<li>Hora Inicio: Ingrese un valor menor o igual que 24.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>Hora Inicio: Debe ser un número.</li>";
            flag = false;
        }
    }

    //............................MINUTO INICIO.........................
    if (obj.IniMinuto == null || obj.IniMinuto == '') {
        validacion = validacion + "<li>Minuto Inicio: campo requerido.</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero(obj.IniMinuto)) {
            var minutoini = parseInt(obj.IniMinuto);
            if (minutoini > 59) {
                validacion = validacion + "<li>Minuto Inicio: Ingrese un valor menor o igual que 59.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>Minuto Inicio: Debe ser un número.</li>";
            flag = false;
        }
    }

    //............................FECHAS.........................
    //validar que la fecha registro no supere la fecha actual
    var horainicial = obj.IniHora + "/" + obj.IniMinuto + "/" + "00";
    var horafinal = obj.FinHora + "/" + obj.FinMinuto + "/" + "00";
    var fecha = new Date;
    var HoraActual = fecha.getHours() + "/" + fecha.getMinutes() + "/" + fecha.getSeconds();
    var diaActualEjec = $("#hfFechaHoraActualEjec").val();

    //esta permitido el registro de mantenimientos ejecutados para todo el dia
    if (parseInt(obj.FinHora) <= 24 && parseInt(obj.IniHora) <= 23 && parseInt(obj.FinMinuto) <= 59 && parseInt(obj.IniMinuto) <= 59) {
        //............................FECHA FIN.........................
        if (obj.FinFecha == null || obj.FinFecha == '') {
            validacion = validacion + "<li>Fecha Fin: campo requerido.</li>";//Campo Requerido
            flag = false;
        }
        else {//MasGrandeQueI
            var fechaFinProg = obtenerFechaFinProgramacion24(document.getElementById('Progrfechafin').value);
            if (formatFecha(obj.FinFecha, obj.FinHora, obj.FinMinuto) > fechaFinProg) {
                validacion = validacion + "<li> Fecha Fin: Debe ser menor o igual que la fecha de Fin del Programa.</li>";
                flag = false;
            }
            else {
                if (formatFecha(obj.FinFecha, obj.FinHora, obj.FinMinuto) <= formatFecha(obj.IniFecha, obj.IniHora, obj.IniMinuto)) {//MasGrandeQue
                    validacion = validacion + "<li>Fecha Fin: debe ser mayor que la fecha Inicio.</li>";
                    flag = false;
                }

                //SÍ SE VALIDA EJECUTADOS EN INTRANET
                if (INTERVENCION_WEB.Evenclasecodi == 1 && obj.FinFecha == diaActualEjec) {
                    if (validarFechaRegistro(horafinal, HoraActual)) {
                        validacion = validacion + "<li>Fecha Fin: Debe ser menor a la fecha y hora Actual.</li>"; //validar hora del sistema con horaInicio
                        flag = false;
                    }
                }
            }
        }
        //............................FECHA INICIO.........................
        if (obj.IniFecha == null || obj.IniFecha == '') {
            validacion = validacion + "<li>Fecha Inicio: campo requerido.</li>";//Campo Requerido
            flag = false;
        }
        else {//MasGrandeQueF
            var fechaIniProg = formatFecha(document.getElementById('Progrfechaini').value, document.getElementById('ProgrfechainiH').value, document.getElementById('ProgrfechainiM').value);
            if (formatFecha(obj.IniFecha, obj.IniHora, obj.IniMinuto) < fechaIniProg) {
                validacion = validacion + "<li>Fecha Inicio: Debe ser mayor o igual que la fecha de Inicio del Programa.</li>";
                flag = false;
            }

            //SÍ SE VALIDA EJECUTADOS EN INTRANET
            if (INTERVENCION_WEB.Evenclasecodi == 1 && formatFecha(obj.FinFecha, obj.FinHora, obj.FinMinuto) == fecha) {
                if (validarFechaRegistro(horainicial, HoraActual)) {
                    validacion = validacion + "<li>Fecha Inicio: Debe ser menor a la fecha y hora Actual.</li>"; //validar hora del sistema con horaInicio
                    flag = false;
                }
            }
        }
    }

    //............................ Provoca Interrupción.........................
    var chkInterinterrup = $("#ChkInterinterrup").is(':checked');
    if (chkInterinterrup) {
        var mwindisp = parseFloat(obj.Intermwindispo) || 0;
        if (mwindisp <= 0) {
            validacion = validacion + "<li>MW Indisponibles: Debe ser un número mayor o igual a cero.</li>";
            flag = false;
        }
    }

    //............................MW INDISPONIBLES.........................
    if (obj.Intermwindispo == null || obj.Intermwindispo == '') {
        validacion = validacion + "<li>MW Indisponibles: campo requerido</li>";//Campo Requerido
        flag = false;
    }
    else {
        if (validarNumero(obj.Intermwindispo)) {
            var mwindisp = parseFloat(obj.Intermwindispo);

            if (mwindisp < 0) {
                validacion = validacion + "<li>MW Indisponibles: Debe ser un número mayor o igual a cero.</li>";
                flag = false;
            }
        }
        else {
            validacion = validacion + "<li>MW Indisponibles: Debe ser un número.</li>";
            flag = false;
        }
    }

    //............................CÓDIGO SEGUIMIENTO.........................
    if ($("#codseguimiento").val() != '') {
        var validarCodSeg = validarCodSeguimiento(obj);
        if (validarCodSeg == false) {
            validacion = validacion + "<li>Código de seguimiento: Valor inválido.</li>";//Campo Requerido
            flag = false;
        }
    }

    //............................DESCRIPCIÓN.............................
    if (obj.Equicodi != null) {
        if (obj.Interdescrip == null || obj.Interdescrip == '') {
            validacion = validacion + "<li>Descripción: campo requerido.</li>";//Campo Requerido
            flag = false;
        }
        else {
            if (obj.Interdescrip.length > 500) {
                validacion = validacion + "<li>Descripción: El texto supera los 500 caracteres.</li>";//supera 500 caracteres
                flag = false;
            }
            else {
                if (obj.Equicodi > 0) {
                    var validarDesc = validarExisteIntervencionDuplicada(obj);
                    if (validarDesc) {
                        validacion = validacion + "<li>Descripción: Existe intervencion con mismo equipo, fecha inicio, fecha final e igual descripción.</li>";//Campo Requerido
                        flag = false;
                    }
                }
            }
        }
    }

    validacion = validacion + "</ul>";

    if (flag == true) validacion = "";

    return validacion;
}

mostrarAlerta = function (alerta) {
    $('#mensaje2').html(alerta);
    $('#mensaje2').css("display", "block");
}

limpiarMensaje = function () {
    $('#mensaje2').html("");
    $('#mensaje2').css("display", "none");
}

function validarCodSeguimiento(obj) {
    var bRes = true;
    $.ajax({
        type: 'POST',
        url: controler + 'ValidarCodigoSeguimiento',
        data: {
            codigoSeguimiento: $("#codseguimiento").val(),
            equiCodi: obj.Equicodi
        },
        dataType: 'json',
        cache: false,
        async: false,
        success: function (result) {
            bRes = result;
        },
        error: function (err) {
            alert('Ha ocurrido un error');
        }
    });
    return bRes;
}

function validarExisteIntervencionDuplicada(obj) {
    var bRes = true;
    var intercodi = 0;
    if (OPCION_ACTUAL != 3) {
        intercodi = $("#InterCodi").val();
    }

    $.ajax({
        type: 'POST',
        url: controler + 'ValidarExisteIntervencionDuplicada',
        data: {
            descripcion: obj.Interdescrip,
            equiCodi: obj.Equicodi,
            fechaIni: obj.IniFecha,
            horaIni: obj.IniHora,
            minutosIni: obj.IniMinuto,
            fechaFin: obj.FinFecha,
            horaFin: obj.FinHora,
            minutosFin: obj.FinMinuto,
            interCodi: intercodi,
            programacion: $("#ProgrcodiForm").val(),
            tipoevencodi: obj.Tipoevencodi
        },
        dataType: 'json',
        cache: false,
        async: false, //secuencial
        success: function (result) {
            bRes = result;
        },
        error: function (err) {
            bRes = false;
        }
    });
    return bRes;
}

function validarExisteCambioDescripcionOEquipo(intercodi, equicodi, descripcionOriginal, descripcionNueva) {
    var bRes = true;

    $.ajax({
        type: 'POST',
        url: controler + 'ValidarExisteCambioDescripcionOEquipo',
        data: {
            intercodi: intercodi,
            equicodi: equicodi,
            descripcionOriginal: descripcionOriginal,
            descripcionNueva: descripcionNueva
        },
        dataType: 'json',
        cache: false,
        async: false, //secuencial
        success: function (result) {
            bRes = result;
        },
        error: function (err) {
            bRes = false;
        }
    });

    return bRes;
}

function validarNumero(texto) {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(texto);
};

function validarLetraNumero(texto) {
    return /^(\d+[a-z]|[a-z]+\d)[a-z\d]*$/i.test(texto);
};

function validarTextoSinSaltoLinea(texto) {
    if (texto == null) texto = "";
    texto = texto.replace(/(?:\r\n|\r|\n)/g, " ");
    texto = texto.trim();

    return texto;
}

//..................................................................................................................
function _setearFechasDescProgramacion(model) {
    document.getElementById('Progrfechaini').value = model.Progrfechaini;
    document.getElementById('ProgrfechainiH').value = model.ProgrfechainiH;
    document.getElementById('ProgrfechainiM').value = model.ProgrfechainiM;

    $("#inifecha").val(model.Progrfechaini);
    $("#inihora").val(model.ProgrfechainiH);
    $("#iniminuto").val(model.ProgrfechainiM);

    document.getElementById('Progrfechafin').value = model.Progrfechafin;
    document.getElementById('ProgrfechafinH').value = model.ProgrfechafinH;
    document.getElementById('ProgrfechafinM').value = model.ProgrfechafinM;

    $("#finfecha").val(model.Progrfechafin);
    $("#finhora").val(model.ProgrfechafinH);
    $("#finminuto").val(model.ProgrfechafinM);

    $('#inifecha').unbind();
    $('#inifecha').Zebra_DatePicker({
        readonly_element: false,
        direction: [document.getElementById('Progrfechaini').value, convertirDateToString(obtenerFechaFinProgramacion24(document.getElementById('Progrfechafin').value))],
    });
    $('#finfecha').unbind();
    $('#finfecha').Zebra_DatePicker({
        readonly_element: false,
        direction: [document.getElementById('Progrfechaini').value, convertirDateToString(obtenerFechaFinProgramacion24(document.getElementById('Progrfechafin').value))],
    });

    $('#programacionInfo').html("<b>" + model.ProgrfechainiDesc + " al " + model.ProgrfechafinDesc + "</b>");
}

function causas() {
    var vClaProCodi = $('#cboTipoEventoPopup').val();

    if (vClaProCodi != "" || vClaProCodi == null) {
        $.ajax({
            type: 'POST',
            url: controler + "ListarCboCausa",
            datatype: 'json',
            data: JSON.stringify({ claProCodi: vClaProCodi }),
            contentType: "application/json",
            success: function (modelo) {
                $('#cboCausaPopup').empty();
                //var option = '<option value="-1" >----- Seleccione  ----- </option>';
                var option = '';
                $.each(modelo.ListaCausas, function (k, v) {
                    option += '<option value =' + v.Subcausacodi + '>' + v.Subcausadesc + '</option>';
                })
                $('#cboCausaPopup').append(option);
            }
        });
    } else {
        $('#cboCausaPopup').empty();
        //var option = '<option value="-1">----- Seleccione un tipo programación ----- </option>';
        var option = '<option value="-1">NO IDENTIFICADO</option>';
        $('#cboCausaPopup').append(option);
    }
}

function formatFecha(sFecha, sHora, sMinute) {   // DD/MM/AAAA HH:mm
    if (sFecha == "") return false;
    var sDia = sFecha.substring(0, 2);
    var sMes = sFecha.substring(3, 5);
    var sAnio = sFecha.substring(6, 10);

    return new Date(sAnio, sMes - 1, sDia, sHora, sMinute);
}

function obtenerFechaFinProgramacion24(fechaFinProgramado) {

    if (fechaFinProgramado == "") return false;
    var sDia = fechaFinProgramado.substring(0, 2);
    var sMes = fechaFinProgramado.substring(3, 5);
    var sAnio = fechaFinProgramado.substring(6, 10);
    var fechaProg = new Date(sAnio, sMes - 1, sDia);

    return new Date(fechaProg.setDate(fechaProg.getDate() + 1));
}

//Convierte objeto Date to string format DD/MM/YYYY
function convertirDateToString(date) {
    var d = date.getDate();
    var m = date.getMonth();   // JavaScript months are 0-11
    m += 1;
    var y = date.getFullYear();

    return (('0' + d).slice(-2) + "/" + ('0' + m).slice(-2) + "/" + y);
}

///////////////////////////
/// Búsqueda Equipo
///////////////////////////
function visualizarBuscarEquipo() {

    if (APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO == 0) {
        cargarBusquedaEquipo(APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO);
    } else {
        openBusquedaEquipo();
    }

    APP_MOSTRAR_MODAL_EQUIPO_YA_CARGADO++;
}

function cargarBusquedaEquipo(flag) {
    $.ajax({
        type: "POST",
        url: controlador + "BusquedaEquipo",
        data: {
            filtroFamilia: -1
        },
        global: false,
        success: function (evt) {
            $('#busquedaEquipo').html(evt);
            openBusquedaEquipo();
        },
        error: function (req, status, error) {
            mostrarError();
        }
    });
}

function openBusquedaEquipo() {
    $('#busquedaEquipo').bPopup({
        easing: 'easeOutBack',
        speed: 450,
        transition: 'slideDown'
    });
    $('#txtFiltro').focus();
}

function seleccionarEquipo(equicodi, equinomb, Areanomb, Emprnomb, Famabrev, emprcodi, areacodi, famcodi, grupotipocogen) {
    $("#cboEquipoPopup").val('');
    $("#nomEquipo").val('');
    $("#cboEmpresaPopup").val('');
    $("#famAbrev").val('');
    //$('#cboUbicacionPopup').val();

    $("#nomEmpresa").val(Emprnomb);
    $("#cboEmpresaPopup").val(emprcodi);
    $("#sEmprcodi").val("[" + emprcodi + "]");

    $("#famAbrev").val(Famabrev);

    $("#nomEquipo").val(equinomb);
    $("#cboEquipoPopup").val(equicodi);

    $("#nomUbicacion").val(Areanomb);

    $("#cboUbicacionPopup").val(areacodi);

    $("#hffamcodi").val(famcodi);

    $("#hfgrupotipocogen").val(grupotipocogen);

    setTipoIndisp();

    $('#busquedaEquipo').bPopup().close();

    habilitarSeccionIndispPR25(famcodi);

    // Limpiar interfaz
    INTERVENCION_WEB.Arecodi = parseInt(areacodi);
    INTERVENCION_WEB.AreaNomb = Areanomb ?? "";

    INTERVENCION_WEB.Emprcodi = parseInt(emprcodi);
    INTERVENCION_WEB.EmprNomb = Emprnomb ?? "";

    INTERVENCION_WEB.Equicodi = parseInt(equicodi);
    INTERVENCION_WEB.Equiabrev = equinomb ?? "";

    if (OPCION_ACTUAL == OPCION_EDITAR) {
        //setear valores
        $("#codseguimiento").val("");
        $("#justificacion").val("");
    }

    var idTipoprog = parseInt($("#idTipoProgramacion").val()) || 0;
    if (OPCION_ACTUAL == OPCION_NUEVO && (EVENCLASECODI_SEMANAL == idTipoprog || EVENCLASECODI_DIARIO == idTipoprog)) {
        var existeFam = LISTA_FAMCODI_SUSTENTO_OBLIGATORIO.find(x => x == famcodi) != null || LISTA_FAMCODI_SUSTENTO_OPCIONAL.find(x => x == famcodi) != null;
        if (existeFam) {
            $("#btnSustentoInclPopup").show();
        } else {
            $("#btnSustentoInclPopup").hide();
        }
    }
}

function validarFechaRegistro(fecha1, fecha2) {
    var segundosfecha1 = convertirASegundos(fecha1);
    var segundosfecha2 = convertirASegundos(fecha2);

    var diferencia = segundosfecha2 - segundosfecha1;

    if (diferencia < 0)
        return true;
    else
        return false;
}

function convertirASegundos(tiempo) {

    var x = tiempo.split('/');
    var hor = x[0];
    var min = x[1];
    var sec = x[2];
    return (Number(sec) + (Number(min) * 60) + (Number(hor) * 3600));
}

///////////////////////////////////////////////////////////////////////////////////////////////////
/// Validaciones con otros aplicativos
///////////////////////////////////////////////////////////////////////////////////////////////////
function interv_mostrarAdvertenciaEjecutadoNoProgramado() {
    var valEjec = parseInt($("#hfResultadoValIntervEjec").val()) || 0;
    if (valEjec) {
        $(".fila_val_intervenciones_ejec").show();
    }
}

function interv_mostrarAdvertencias(tipoDisp, listaVal, listaScada, listaEms, listaIdcc) {
    var valHo = parseInt($("#hfResultadoValInterv").val()) || 0;
    if (valHo > 0) {
        $("#div_msj_val_intervenciones").show();
        $(".fila_val_intervenciones_alertas").show();
    }

    //HO
    var htmlHo = htmlAlertaIntervHo(tipoDisp, listaVal);
    if (htmlHo != '') {
        $("#div_msj_val_ho").html(htmlHo);
        $("#div_msj_val_ho").show();
    }

    //SCADA
    var htmlScada = htmlAlertaIntervScada(tipoDisp, listaScada);
    if (htmlScada != '') {
        $("#div_msj_val_scada").html(htmlScada);
        $("#div_msj_val_scada").show();
    }

    //EMS
    var htmlEms = htmlAlertaIntervEms(tipoDisp, listaEms);
    if (htmlEms != '') {
        $("#div_msj_val_ems").html(htmlEms);
        $("#div_msj_val_ems").show();
    }

    //IDCC
    var htmlIdcc = htmlAlertaIntervIdcc(tipoDisp, listaIdcc);
    if (htmlIdcc != '') {
        $("#div_msj_val_idcc").html(htmlIdcc);
        $("#div_msj_val_idcc").show();
    }
}

function htmlAlertaIntervHo(tipoDisp, listaHo) {
    var valHo = parseInt($("#hfResultadoValInterv").val()) || 0;
    if (valHo > 0 && listaHo != null && listaHo.length > 0) {
        var html = '';
        $("#div_msj_val_intervenciones").prop('class', 'action-alert2');

        //
        if (tipoDisp == "F") {
            html += '<span>Unidades que tienen Horas de Operación en servicio:</span>';
            html += `
                <table class="pretty tabla-icono tabla-ems" style="width: 700px;">
                    <thead>
                        <tr>
                            <th style="">Unidad</th>
                            <th>Modo de operación</th>
                            <th style="">Hora Inicio</th>
                            <th style="">Hora Fin</th>
                            <th>Observación</th>
                        </tr>
                    </thead>
                    <tbody>
                `;

            //Intervenciones
            for (var j = 0; j < listaHo.length; j++) {
                var regInterv = listaHo[j];
                var unidad = regInterv.Equinomb;
                var modoOp = regInterv.ModoOp;
                var horaIni = regInterv.FechaIniDesc;
                var horaFin = regInterv.FechaFinDesc;
                var descrip = regInterv.Hopdesc;

                html += `
                    <tr>
                        <td>${unidad}</td>
                        <td style="text-align:left;">${modoOp}</td>
                        <td>${horaIni}</td>
                        <td>${horaFin}</td>
                        <td style="text-align:left;">${descrip}</td>
                    </tr>
                `;
            }
            html += `
                    </tbody>
                </table>
                `
                ;
        } else {
            html += '<span>Rangos de fecha cuyos equipos no tienen horas de operación:</span>';
            html += `
                <table class="pretty tabla-icono tabla-ems" style="width: 300px;">
                    <thead>
                        <tr>
                            <th style="">Unidad</th>
                            <th style="">Hora Inicio</th>
                            <th style="">Hora Fin</th>
                        </tr>
                    </thead>
                    <tbody>
                `;

            //Intervenciones
            for (var j = 0; j < listaHo.length; j++) {
                var regInterv = listaHo[j];
                var unidad = regInterv.Equinomb;
                var horaIni = regInterv.FechaIniDesc;
                var horaFin = regInterv.FechaFinDesc;

                html += `
                    <tr>
                        <td>${unidad}</td>
                        <td>${horaIni}</td>
                        <td>${horaFin}</td>
                    </tr>
                `;
            }
            html += `
                    </tbody>
                </table>
                `
                ;
        }
        return html;
    }
    return '';
}

function htmlAlertaIntervScada(tipoDisp, listaScada) {
    var valScada = parseInt($("#hfResultadoValInterv").val()) || 0;
    if (valScada > 0 && listaScada != null && listaScada.length > 0) {
        var html = '';
        $("#div_msj_val_intervenciones").prop('class', 'action-alert2');

        if (tipoDisp == "F") {
            html += '<span>Unidades que tienen señales Scada:</span>';
            html += `
                <table class="pretty tabla-icono tabla-ems" style="width: 700px;">
                    <thead>
                        <tr>
                            <th style="">Equipo</th>
                            <th>Medida</th>
                            <th style="">Hora Inicio</th>
                            <th style="">Hora Fin</th>
                        </tr>
                    </thead>
                    <tbody>
                `;

            //Intervenciones
            for (var j = 0; j < listaScada.length; j++) {
                var regInterv = listaScada[j];
                var equipo = regInterv.Equinomb;
                var medida = regInterv.Tipoinfoabrev;
                var horaIni = regInterv.FechaIniDesc;
                var horaFin = regInterv.FechaFinDesc;

                html += `
                    <tr>
                        <td>${equipo}</td>
                        <td style="text-align:left;">${medida}</td>
                        <td>${horaIni}</td>
                        <td>${horaFin}</td>
                    </tr>
                `;
            }
            html += `
                    </tbody>
                </table>
                `
                ;
        } else {
            html += '<span>Rangos de fecha cuyos equipos no presentan señales Scada:</span>';
            html += `
                <table class="pretty tabla-icono tabla-ems" style="width: 300px;">
                    <thead>
                        <tr>
                            <th style="">Equipo</th>
                            <th>Medida</th>
                            <th style="">Hora Inicio</th>
                            <th style="">Hora Fin</th>
                        </tr>
                    </thead>
                    <tbody>
                `;

            //Intervenciones
            for (var j = 0; j < listaScada.length; j++) {
                var regInterv = listaScada[j];
                var equipo = regInterv.Equinomb;
                var medida = regInterv.Tipoinfoabrev;
                var horaIni = regInterv.FechaIniDesc;
                var horaFin = regInterv.FechaFinDesc;

                html += `
                    <tr>
                        <td>${equipo}</td>
                        <td style="text-align:left;">${medida}</td>
                        <td>${horaIni}</td>
                        <td>${horaFin}</td>
                    </tr>
                `;
            }
            html += `
                    </tbody>
                </table>
                `
                ;
        }
        return html;
    }
    return '';
}

function htmlAlertaIntervEms(tipoDisp, listaEms) {
    var valEms = parseInt($("#hfResultadoValInterv").val()) || 0;
    if (valEms > 0 && listaEms != null && listaEms.length > 0) {
        var html = '';
        $("#div_msj_val_intervenciones").prop('class', 'action-alert2');

        if (tipoDisp == "F") {
            html += '<span>Unidades que tienen señales estimadas del EMS:</span>';
            html += `
                <table class="pretty tabla-icono tabla-ems" style="width: 700px;">
                    <thead>
                        <tr>
                            <th style="">Equipo</th>
                            <th>Medida</th>
                            <th style="">Hora Inicio</th>
                            <th style="">Hora Fin</th>
                        </tr>
                    </thead>
                    <tbody>
                `;

            //Intervenciones
            for (var j = 0; j < listaEms.length; j++) {
                var regInterv = listaEms[j];
                var equipo = regInterv.Equinomb;
                var medida = regInterv.Tipoinfoabrev;
                var horaIni = regInterv.FechaIniDesc;
                var horaFin = regInterv.FechaFinDesc;

                html += `
                    <tr>
                        <td>${equipo}</td>
                        <td style="text-align:left;">${medida}</td>
                        <td>${horaIni}</td>
                        <td>${horaFin}</td>
                    </tr>
                `;
            }
            html += `
                    </tbody>
                </table>
                `
                ;
        } else {
            html += '<span>Rangos de fecha cuyos equipos no presentan señales estimadas del EMS:</span>';
            html += `
                <table class="pretty tabla-icono tabla-ems" style="width: 300px;">
                    <thead>
                        <tr>
                            <th style="">Equipo</th>
                            <th>Medida</th>
                            <th style="">Hora Inicio</th>
                            <th style="">Hora Fin</th>
                        </tr>
                    </thead>
                    <tbody>
                `;

            //Intervenciones
            for (var j = 0; j < listaEms.length; j++) {
                var regInterv = listaEms[j];
                var equipo = regInterv.Equinomb;
                var medida = regInterv.Tipoinfoabrev;
                var horaIni = regInterv.FechaIniDesc;
                var horaFin = regInterv.FechaFinDesc;

                html += `
                    <tr>
                        <td>${equipo}</td>
                        <td style="text-align:left;">${medida}</td>
                        <td>${horaIni}</td>
                        <td>${horaFin}</td>
                    </tr>
                `;
            }
            html += `
                    </tbody>
                </table>
                `
                ;
        }
        return html;
    }
    return '';
}

function htmlAlertaIntervIdcc(tipoDisp, listaIdcc) {
    var valEms = parseInt($("#hfResultadoValInterv").val()) || 0;
    if (valEms > 0 && listaIdcc != null && listaIdcc.length > 0) {
        var html = '';
        $("#div_msj_val_intervenciones").prop('class', 'action-alert2');

        if (tipoDisp == "F") {
            html += '<span>Unidades que tienen datos IDCC:</span>';
            html += `
                <table class="pretty tabla-icono tabla-ems" style="width: 700px;">
                    <thead>
                        <tr>
                            <th>Formato</th>
                            <th style="">Equipo</th>
                            <th>Medida</th>
                            <th style="">Hora Inicio</th>
                            <th style="">Hora Fin</th>
                        </tr>
                    </thead>
                    <tbody>
                `;

            //Intervenciones
            for (var j = 0; j < listaIdcc.length; j++) {
                var regInterv = listaIdcc[j];
                var formato = regInterv.TipoFuenteDatoDesc;
                var equipo = regInterv.Equinomb;
                var medida = regInterv.Tipoinfoabrev;
                var horaIni = regInterv.FechaIniDesc;
                var horaFin = regInterv.FechaFinDesc;

                html += `
                    <tr>
                        <td style="text-align:left;">${formato}</td>
                        <td>${equipo}</td>
                        <td style="text-align:left;">${medida}</td>
                        <td>${horaIni}</td>
                        <td>${horaFin}</td>
                    </tr>
                `;
            }
            html += `
                    </tbody>
                </table>
                `
                ;
        } else {
            html += '<span>Rangos de fecha cuyos equipos no tienen datos IDCC:</span>';
            html += `
                <table class="pretty tabla-icono tabla-ems" style="width: 300px;">
                    <thead>
                        <tr>
                            <th>Formato</th>
                            <th style="">Equipo</th>
                            <th>Medida</th>
                            <th style="">Hora Inicio</th>
                            <th style="">Hora Fin</th>
                        </tr>
                    </thead>
                    <tbody>
                `;

            //Intervenciones
            for (var j = 0; j < listaIdcc.length; j++) {
                var regInterv = listaIdcc[j];
                var formato = regInterv.TipoFuenteDatoDesc;
                var equipo = regInterv.Equinomb;
                var medida = regInterv.Tipoinfoabrev;
                var horaIni = regInterv.FechaIniDesc;
                var horaFin = regInterv.FechaFinDesc;

                html += `
                    <tr>
                        <td style="text-align:left;">${formato}</td>
                        <td>${equipo}</td>
                        <td style="text-align:left;">${medida}</td>
                        <td>${horaIni}</td>
                        <td>${horaFin}</td>
                    </tr>
                `;
            }
            html += `
                    </tbody>
                </table>
                `
                ;
        }
        return html;
    }
    return '';
}

//#region Indisponiblidades PR25

function habilitarSeccionIndispPR25(famcodi) {
    inhabilitarSeccionIndispPR25();
    limpiarIndisponibilidadPr25()
    $("#ChkPr25").removeAttr('checked');

    if (listaGenerCent.includes(famcodi) && INTERVENCION_GLOBAL.Esindisponilidadpr25) {
        $("#tblIndisponibilidades").show();
        habilitarPr25();
    }
}

function inhabilitarSeccionIndispPR25() {
    $("#tblIndisponibilidades").hide();
}

function limpiarIndisponibilidadPr25() {
    $("#cboTipoIndisp25").prop("selectedIndex", 0);
    $("#Entidad_Interpr").val("");
    $("#ChkPr25").removeAttr('checked');
    $('input[name="Entidad.Interasocproc"]').removeAttr('checked');
}

function habilitarPr25Edit() {
    $("#tr_cb_tipoindisp").hide();
    $("#tr_pr").hide();
    $("#tr_asocproc").hide();

    var cogeneracion = $("#hfgrupotipocogen").val();
    if ($("#ChkPr25").is(':checked')) {
        $("#tr_cb_tipoindisp").show();
        tipoIndisp25();

        if (cogeneracion == "S")
            $("#tr_asocproc").show();
    }
}

function habilitarPr25() {
    $("#tr_cb_tipoindisp").hide();
    $("#tr_pr").hide();
    $("#tr_asocproc").hide();

    var cogeneracion = $("#hfgrupotipocogen").val();
    if ($("#ChkPr25").is(':checked')) {
        $("#tr_cb_tipoindisp").show();

        if (cogeneracion == "S") {
            $("input[name='Entidad.Interasocproc'][value='N']").prop("checked", true);
            $("#tr_asocproc").show();
        }
    }
}

function tipoIndisp25() {
    var valor = $("#cboTipoIndisp25").val();
    $("#tr_pr").hide();
    if (valor == "PP" || valor == "FP") {
        $("#tr_pr").show();
    }
}

function setTipoIndisp() {
    var famcodi = parseInt($("#hffamcodi").val()) || 0;
    $("#cboTipoIndisp25").empty();

    var html = '';
    html += '<option value="-1">NO DEFINIDO</option>';

    if (famcodi != 4 && famcodi != 2) {
        html += '<option value="PT">Indisponibilidad Programada Total</option>';
        html += '<option value="PP">Indisponibilidad Programada Parcial</option>';
        html += '<option value="FT">Indisponibilidad Fortuita Total</option>';
        html += '<option value="FP">Indisponibilidad Fortuita Parcial</option>';
    } else {
        html += '<option value="PT">Indisponibilidad Programada Total</option>';
    }
    $("#cboTipoIndisp25").html(html);
}

//#endregion